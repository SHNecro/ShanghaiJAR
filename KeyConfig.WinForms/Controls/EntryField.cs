using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Common.Configuration;
using Common.ExtensionMethods;
using OpenTK.Input;
using Timer = System.Windows.Forms.Timer;
using TKKey = OpenTK.Input.Key;
using TKKeyboard = OpenTK.Input.Keyboard;


namespace KeyConfig.WinForms.Controls
{
    public class EntryField : TextBox
    {
		private static event Action InputComplete;
        private static event Action<TKKey> OnTKKeyPress;
		private static event Action<int> OnButtonPress;
		private static event Action<XInputGamePadButton> OnTKButtonPress;
		private static readonly List<TKKey> TKKeys;
		private static readonly List<XInputGamePadButton> TKButtons;

		//private static DXJoystick DXJoystick;
		//private static DirectInput DirectInput = new DirectInput();
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

            var inputPoll = new Timer();
            inputPoll.Interval = 50;
            inputPoll.Tick += (sender, args) =>
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

				/*
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
                */

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
            //inputPoll.AutoReset = false;
            inputPoll.Start();

            Application.ApplicationExit += (sender, args) =>
            {;
                //DXJoystick?.Dispose();
                //DirectInput?.Dispose();
                inputPoll.Stop();
                inputPoll.Dispose();
                Environment.Exit(0);
            };
        }
        
		public EntryField()
		{
			this.ReadOnly = true;
            //InitializeComponent();

			EntryField.InputComplete += this.EntryField_OnInputComplete;
            EntryField.OnTKKeyPress += this.EntryField_OnTKKeyPress;
			EntryField.OnButtonPress += this.EntryField_OnButtonPress;
			EntryField.OnTKButtonPress += this.EntryField_OnTKButtonPress;

			Application.ApplicationExit += (sender, args) =>
			{
				EntryField.InputComplete -= this.EntryField_OnInputComplete;
				EntryField.OnTKKeyPress -= this.EntryField_OnTKKeyPress;
                EntryField.OnButtonPress -= this.EntryField_OnButtonPress;
				EntryField.OnTKButtonPress -= this.EntryField_OnTKButtonPress;
			};
        }

		private void EntryField_OnInputComplete()
		{
			if (this.Focused && FieldSet)
			{
				this.Invoke(new Action(() =>
				{
					FieldSet = false;
					this.MoveToNextUIElement();
				}));
			}
		}

        private void EntryField_OnTKKeyPress(TKKey key)
        {
            if (this.IsKeyboardEntry && this.Focused)
            {
                this.Invoke(new Action(() =>
                {
	                this.Text = key.ToString();
	                this.KeyCode = key.ToSHKeyCode();
	                FieldSet = true;
                }));
            }
        }

        private void EntryField_OnButtonPress(int buttonPress)
        {
            if (!this.IsKeyboardEntry && this.Focused)
            {
	            this.Invoke(new Action(() =>
				{
					this.Text = buttonPress.FromSHButtonCode().ToString();
					this.KeyCode = buttonPress;
					FieldSet = true;
                }));
            }
		}

		private void EntryField_OnTKButtonPress(XInputGamePadButton buttonPress)
		{
			if (!this.IsKeyboardEntry && this.Focused)
			{
				this.Invoke(new Action(() =>
				{
					this.Text = buttonPress.ToString();
					this.KeyCode = buttonPress.ToSHButtonCode();
					FieldSet = true;
				}));
			}
		}

		private int keyCode;
		public int KeyCode
		{
			get
			{
				return this.keyCode;
			}

			set
			{
				if (this.IsKeyboardEntry)
				{
					var key = value.FromSHKeyCode();
					this.Text = key.ToString();
				}
				else
				{
					var button = value.FromSHButtonCode();
					this.Text = button.ToString();
				}
			}
		}
		
		public bool LastEntry { get; set; }
        public bool IsKeyboardEntry { get; set; } = true;

        private void MoveToNextUIElement()
        {
            if (this.LastEntry)
            {
                return;
            }

            Console.WriteLine("next");
            this.SelectNextControl(this, true, false, false, true);
        }

	    /*
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
        */

	    /*
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
        */
    }
}