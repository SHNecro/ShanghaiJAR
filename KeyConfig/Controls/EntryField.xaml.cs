using Common.Configuration;
using OpenTK.Input;
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using DXJoystick = SlimDX.DirectInput.Joystick;
using DXJoystickState = SlimDX.DirectInput.JoystickState;
using TKKey = OpenTK.Input.Key;
using TKKeyboard = OpenTK.Input.Keyboard;
using Common.ExtensionMethods;

namespace KeyConfig.Controls
{
    /// <summary>
    /// Interaction logic for EntryField.xaml
    /// </summary>
    public partial class EntryField : UserControl
    {
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(EntryField), new PropertyMetadata(null));
		public static readonly DependencyProperty EntryTextProperty = DependencyProperty.Register("EntryText", typeof(string), typeof(EntryField), new PropertyMetadata(null));
		public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register("DisplayText", typeof(string), typeof(EntryField), new PropertyMetadata(null));

		private static readonly List<TKKey> TKKeys;
		private static readonly List<XInputGamePadButton> TKButtons;

        private static DXJoystick DXJoystick;
        private static DirectInput DirectInput = new DirectInput();
        private static HashSet<TKKey> TKKeysHeld;
        private static bool[] DXButtonsHeld = new bool[256];
		private static Dictionary<XInputGamePadButton, bool> TKButtonsHeld;
		private static bool?[] DXButtonInitialState;
		private static Dictionary<int, Dictionary<XInputGamePadButton, bool?>> TKButtonsInitialState;
        private static bool FieldSet = false;

        static EntryField()
		{
			TKKeys = Enum.GetValues(typeof(TKKey)).Cast<TKKey>().ToList();
			TKButtons = Enum.GetValues(typeof(XInputGamePadButton)).Cast<XInputGamePadButton>().ToList();

			TKKeysHeld = new HashSet<TKKey>();
            TKButtonsHeld = TKButtons.ToDictionary(b => b, b => false);

            TKButtonsInitialState = new Dictionary<int, Dictionary<XInputGamePadButton, bool?>>();

            var inputPoll = new Timer(50);
            inputPoll.Elapsed += (sender, args) =>
            {
				var inputPressed = false;

				try
				{
					var keyState = TKKeyboard.GetState();
					if (keyState.IsConnected)
					{
						foreach (var key in TKKeys)
						{
							if (keyState.IsKeyDown(key))
							{
								if (!TKKeysHeld.Contains(key))
								{
									OnTKKeyPress(key);
								}
								TKKeysHeld.Add(key);
								inputPressed = true;
							}
							else
							{
								TKKeysHeld.Remove(key);
							}
						}
					}
				}
				catch { }

                if (CreateController())
                {
                    var buttons = CaptureControllerInput();
					if (DXButtonInitialState == null)
					{
						DXButtonInitialState = buttons.Cast<bool?>().ToArray();
					}

                    for (int i = 0; buttons != null && i < buttons.Length; i++)
                    {
						if (buttons[i] != DXButtonInitialState[i])
						{
							DXButtonInitialState[i] = null;
						}

						if (DXButtonInitialState[i] == null)
						{
							if (buttons[i])
							{
								if (!DXButtonsHeld[i])
								{
									OnButtonPress(i);
								}
								DXButtonsHeld[i] = true;
								inputPressed = true;
							}
							else
							{
								DXButtonsHeld[i] = false;
							}
						}
                    }
                }

				for (int i = 0; i <= 3; i++)
                {
                    var isFirstRun = TKButtonsInitialState.Count <= i || TKButtonsInitialState[i] == null;
                    if (isFirstRun)
                    {
                        TKButtonsInitialState[i] = new Dictionary<XInputGamePadButton, bool?>();
                    }

                    var initialState = TKButtonsInitialState[i];

                    try
					{
						var gamePadState = GamePad.GetState(i);

						if (gamePadState.IsConnected)
						{
							foreach (var button in TKButtons)
							{
								var buttonIsPressed = gamePadState.IsPressed(button);

								if (isFirstRun)
								{
                                    initialState.TryGetValue(button, out var state);
                                    initialState[button] = (state ?? false) || buttonIsPressed;
									continue;
								}

								if (initialState[button] != null
                                    && initialState[button] != buttonIsPressed)
								{
                                    initialState[button] = null;
								}

								if (initialState[button] == null)
								{
									if (buttonIsPressed)
									{
										if (!TKButtonsHeld[button])
										{
											OnTKButtonPress(button);
										}
										TKButtonsHeld[button] = true;
										inputPressed = true;
									}
									else
									{
										TKButtonsHeld[button] = false;
									}
								}
							}
						}
					}
					catch
                    {
                        TKButtonsInitialState[i] = null;
                    }
				}

				if (!inputPressed && FieldSet)
				{
					InputComplete();
				}

                inputPoll.Start();
            };
            inputPoll.AutoReset = false;
            inputPoll.Start();

            Application.Current.Exit += (sender, args) =>
            {
                DXJoystick?.Dispose();
                DirectInput?.Dispose();
                inputPoll.Close();
            };
        }

		private static event Action InputComplete;
        private static event Action<TKKey> OnTKKeyPress;
		private static event Action<int> OnButtonPress;
		private static event Action<XInputGamePadButton> OnTKButtonPress;

		public EntryField()
        {
            InitializeComponent();

			EntryField.InputComplete += this.EntryField_OnInputComplete;
            EntryField.OnTKKeyPress += this.EntryField_OnTKKeyPress;
			EntryField.OnButtonPress += this.EntryField_OnButtonPress;
			EntryField.OnTKButtonPress += this.EntryField_OnTKButtonPress;

			Application.Current.Exit += (sender, args) =>
			{
				EntryField.InputComplete -= this.EntryField_OnInputComplete;
				EntryField.OnTKKeyPress -= this.EntryField_OnTKKeyPress;
                EntryField.OnButtonPress -= this.EntryField_OnButtonPress;
				EntryField.OnTKButtonPress -= this.EntryField_OnTKButtonPress;
			};
        }

		private void EntryField_OnInputComplete()
		{
			if (this.IsKeyboardFocusWithin && FieldSet)
			{
				this.Dispatcher.Invoke(() =>
				{
					FieldSet = false;
					this.MoveToNextUIElement();
				});
			}
		}

        private void EntryField_OnTKKeyPress(TKKey key)
        {
            if (this.IsKeyboardEntry && this.IsKeyboardFocusWithin)
            {
                this.Dispatcher.Invoke(() =>
				{
					this.DisplayText = key.ToString();
					this.EntryText = key.ToSHKeyCode().ToString();
					FieldSet = true;
                });
            }
        }

        private void EntryField_OnButtonPress(int buttonPress)
        {
            if (!this.IsKeyboardEntry && this.IsKeyboardFocusWithin)
            {
                this.Dispatcher.Invoke(() =>
				{
					this.DisplayText = buttonPress.FromSHButtonCode().ToString();
					this.EntryText = buttonPress.ToString();
					FieldSet = true;
                });
            }
		}

		private void EntryField_OnTKButtonPress(XInputGamePadButton buttonPress)
		{
			if (!this.IsKeyboardEntry && this.IsKeyboardFocusWithin)
			{
				this.Dispatcher.Invoke(() =>
				{
					this.DisplayText = buttonPress.ToString();
					this.EntryText = buttonPress.ToSHButtonCode().ToString();
					FieldSet = true;
				});
			}
		}

		public string LabelText
        {
            get { return (string)this.GetValue(LabelTextProperty); }
            set { this.SetValue(LabelTextProperty, value); }
		}

		public string EntryText
		{
			get { return (string)this.GetValue(EntryTextProperty); }
			set { this.SetValue(EntryTextProperty, value); }
		}

		public string DisplayText
		{
			get { return (string)this.GetValue(DisplayTextProperty); }
			set { this.SetValue(DisplayTextProperty, value); }
		}

		public bool LastEntry { get; set; }
        public bool IsKeyboardEntry { get; set; } = true;

        private void MoveToNextUIElement()
        {
            if (this.LastEntry)
            {
                return;
            }

            // Creating a FocusNavigationDirection object and setting it to a
            // local field that contains the direction selected.
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

            // MoveFocus takes a TraveralReqest as its argument.
            TraversalRequest request = new TraversalRequest(focusDirection);
            
            // Change keyboard focus.
            if (System.Windows.Input.Keyboard.FocusedElement is UIElement elementWithFocus)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private static bool CreateController()
        {
            if (DXJoystick != null)
            {
                return true;
            }
            int num = 1;
            foreach (var device in DirectInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                if (num == 1)
                {
                    DXJoystick = new DXJoystick(DirectInput, device.InstanceGuid);
                    Application.Current.Dispatcher.Invoke(() => {
                        try
                        {
                            DXJoystick.SetCooperativeLevel(new WindowInteropHelper(Application.Current.MainWindow).Handle, CooperativeLevel.Exclusive | CooperativeLevel.Background);
                        }
                        catch { }
                    });
                    break;
                }
                ++num;
            }
            if (DXJoystick != null)
            {
                foreach (var deviceObjectInstance in DXJoystick.GetObjects())
                {
                    if ((uint)(deviceObjectInstance.ObjectType & ObjectDeviceType.Axis) > 0U)
                        DXJoystick.GetObjectPropertiesById((int)deviceObjectInstance.ObjectType).SetRange(-1000, 1000);
                }
            }
            return DXJoystick != null;
        }

        private static bool[] CaptureControllerInput()
        {
            try
            {
                if (DXJoystick == null || !DXJoystick.Acquire().IsSuccess)
                    return null;
                bool[] flagArray = new bool[256];
                DXJoystickState currentState = DXJoystick.GetCurrentState();
                flagArray = currentState.GetButtons();
                int length = flagArray.Length;
                if (currentState.X > 500)
                    flagArray[102] = true;
                else if (currentState.X < -500)
                    flagArray[103] = true;
                if (currentState.Y > 500)
                    flagArray[101] = true;
                else if (currentState.Y < -500)
                    flagArray[100] = true;
                return flagArray;
            }
            catch
            {
                return null;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
