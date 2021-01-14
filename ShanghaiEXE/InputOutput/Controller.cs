using Common.Configuration;
using Common.ExtensionMethods;
using NSGame;
using OpenTK.Input;
using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using Joystick = SlimDX.DirectInput.Joystick;

namespace NSShanghaiEXE.InputOutput
{
    public class Controller : IDisposable
	{
		private static readonly List<XInputGamePadButton> TKButtons;

		public static CooperativeLevel ctl = CooperativeLevel.Background;
        private static DirectInput directInput;
		private static int MinimumButtonCode;
		private static int MaximumButtonCode;
        private Joystick stick;
        private readonly ShanghaiEXE form;

		private HashSet<int> initialStuckDXButtons;
        private Dictionary<int, Dictionary<XInputGamePadButton, bool?>> initialStuckTKButtons;

        private HashSet<int> push;
		private HashSet<int> press;
		private HashSet<int> up;

		static Controller()
		{
			TKButtons = Enum.GetValues(typeof(XInputGamePadButton)).Cast<XInputGamePadButton>().ToList();
			MinimumButtonCode = 0;
			MaximumButtonCode = 256;

			directInput = new DirectInput();
		}

        public Controller(ShanghaiEXE f)
		{
			this.form = f;
			this.push = new HashSet<int>();
			this.press = new HashSet<int>();
			this.up = new HashSet<int>();

            this.initialStuckTKButtons = new Dictionary<int, Dictionary<XInputGamePadButton, bool?>>();

			for (int index = 0; index < 30; ++index)
                Input.inputRecord.Add(new bool[Enum.GetNames(typeof(Button)).Length]);
            this.CreateDevice();
        }

		public int _Up
        {
            get
            {
                return SaveData.Pad[1, 0];
            }
            set
            {
                SaveData.Pad[1, 0] = value;
            }
        }

        public int _Right
        {
            get
            {
                return SaveData.Pad[1, 1];
            }
            set
            {
                SaveData.Pad[1, 1] = value;
            }
        }

        public int _Down
        {
            get
            {
                return SaveData.Pad[1, 2];
            }
            set
            {
                SaveData.Pad[1, 2] = value;
            }
        }

        public int _Left
        {
            get
            {
                return SaveData.Pad[1, 3];
            }
            set
            {
                SaveData.Pad[1, 3] = value;
            }
        }

        public int _A
        {
            get
            {
                return SaveData.Pad[1, 4];
            }
            set
            {
                SaveData.Pad[1, 4] = value;
            }
        }

        public int _B
        {
            get
            {
                return SaveData.Pad[1, 5];
            }
            set
            {
                SaveData.Pad[1, 5] = value;
            }
        }

        public int _L
        {
            get
            {
                return SaveData.Pad[1, 6];
            }
            set
            {
                SaveData.Pad[1, 6] = value;
            }
        }

        public int _R
        {
            get
            {
                return SaveData.Pad[1, 7];
            }
            set
            {
                SaveData.Pad[1, 7] = value;
            }
        }

        public int _Start
        {
            get
            {
                return SaveData.Pad[1, 8];
            }
            set
            {
                SaveData.Pad[1, 8] = value;
            }
        }

        public int _Select
        {
            get
            {
                return SaveData.Pad[1, 9];
            }
            set
            {
                SaveData.Pad[1, 9] = value;
            }
        }

        public int Turbo
        {
            get
            {
                return SaveData.Pad[1, 11];
            }
            set
            {
                SaveData.Pad[1, 11] = value;
            }
        }

        public void GetKeyData()
        {
            if (this.stick == null)
                return;

			var buttons = new HashSet<int>();
			try
			{
				if (this.stick.Acquire().IsSuccess)
                {
                    var currentState = this.stick.GetCurrentState();
					var buttonState = currentState.GetButtons();
					for (int i = 0; i < buttonState.Length; i++)
					{
						if (buttonState[i])
                        {
                            buttons.Add(i);
						}
						if (currentState.X > 500)
						{
							buttons.Add(102);
						}
						else if (currentState.X < -500)
						{
							buttons.Add(103);
						}
						if (currentState.Y > 500)
						{
							buttons.Add(101);
						}
						else if (currentState.Y < -500)
						{
							buttons.Add(100);
						}
                    }

                    if (this.initialStuckDXButtons == null)
                    {
                        this.initialStuckDXButtons = buttons;
                    }
                    else
                    {
                        foreach (var button in this.initialStuckDXButtons)
                        {
                            if (!buttons.Contains(button))
                            {
                                this.initialStuckDXButtons.Remove(button);
                            }

                            buttons.Remove(button);
                        }
                    }
                }
            }
            catch { }
			
			for (int i = 0; i <= 3; i++)
            {
                var isFirstRun = initialStuckTKButtons.Count <= i || initialStuckTKButtons[i] == null;
                if (isFirstRun)
                {
                    initialStuckTKButtons[i] = new Dictionary<XInputGamePadButton, bool?>();
                }

                var initialState = initialStuckTKButtons[i];

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
                                    buttons.Add(button.ToSHButtonCode());
                                }
                            }
						}
					}
				}
				catch
                {
                    this.initialStuckTKButtons[i] = null;
                }
			}

			if (buttons.Any())
			{
				MinimumButtonCode = Math.Min(MinimumButtonCode, buttons.Min());
				MaximumButtonCode = Math.Max(MaximumButtonCode, buttons.Max());
			}

            for (var i = MinimumButtonCode; i <= MaximumButtonCode; i++)
			{
				var buttonPressed = buttons.Contains(i);

				if (buttons.Contains(i))
				{
					if (this.push.Contains(i))
					{
						this.press.Remove(i);
					}
					else
					{
						this.press.Add(i);
					}
					this.push.Add(i);
				}
				else
				{
					if (this.push.Contains(i))
					{
						this.up.Add(i);
					}
					else
					{
						this.up.Remove(i);
					}
					this.push.Remove(i);
					this.press.Remove(i);
				}
			}
		}

		public bool IsPush(int key)
        {
            return this.push.Contains(key);
		}

		public bool IsPress(int key)
		{
			return this.press.Contains(key);
		}

		public bool IsUp(int key)
        {
            return this.up.Contains(key);
		}

		private bool CreateDevice()
        {
            int num = 1;
            foreach (DeviceInstance device in Controller.directInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                if (num == (int)CONTROLLERTYPE.Pad1)
                {
                    this.stick = new Joystick(Controller.directInput, device.InstanceGuid);
                    this.stick.SetCooperativeLevel(form, CooperativeLevel.Exclusive | Controller.ctl);
                    break;
                }
                ++num;
            }
            if (this.stick != null)
            {
                foreach (DeviceObjectInstance deviceObjectInstance in this.stick.GetObjects())
                {
                    if ((uint)(deviceObjectInstance.ObjectType & ObjectDeviceType.Axis) > 0U)
                        this.stick.GetObjectPropertiesById((int)deviceObjectInstance.ObjectType).SetRange(-1000, 1000);
                }
            }
            return this.stick != null;
        }

        public void Dispose()
        {
            this.stick.Dispose();
        }
    }
}
