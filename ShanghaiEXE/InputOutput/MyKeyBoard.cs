using Common.ExtensionMethods;
using NSGame;
using OpenTK.Input;
using System.Collections.Generic;

namespace NSShanghaiEXE.InputOutput
{
    public class MyKeyBoard
	{
		private static readonly IEnumerable<Key> TKKeys;

		private readonly HashSet<int> push;
        private readonly HashSet<int> press;
        private readonly HashSet<int> up;
        public bool disabled = false;

		static MyKeyBoard()
		{
			TKKeys = InputExtensionMethods.UniqueTKKeys;
		}

        public int _Up
        {
            get
            {
                return SaveData.Pad[0, 0];
            }
            set
            {
                SaveData.Pad[0, 0] = value;
            }
        }

        public int _Right
        {
            get
            {
                return SaveData.Pad[0, 1];
            }
            set
            {
                SaveData.Pad[0, 1] = value;
            }
        }

        public int _Down
        {
            get
            {
                return SaveData.Pad[0, 2];
            }
            set
            {
                SaveData.Pad[0, 2] = value;
            }
        }

        public int _Left
        {
            get
            {
                return SaveData.Pad[0, 3];
            }
            set
            {
                SaveData.Pad[0, 3] = value;
            }
        }

        public int _A
        {
            get
            {
                return SaveData.Pad[0, 4];
            }
            set
            {
                SaveData.Pad[0, 4] = value;
            }
        }

        public int _B
        {
            get
            {
                return SaveData.Pad[0, 5];
            }
            set
            {
                SaveData.Pad[0, 5] = value;
            }
        }

        public int _L
        {
            get
            {
                return SaveData.Pad[0, 6];
            }
            set
            {
                SaveData.Pad[0, 6] = value;
            }
        }

        public int _R
        {
            get
            {
                return SaveData.Pad[0, 7];
            }
            set
            {
                SaveData.Pad[0, 7] = value;
            }
        }

        public int _Start
        {
            get
            {
                return SaveData.Pad[0, 8];
            }
            set
            {
                SaveData.Pad[0, 8] = value;
            }
        }

        public int _Select
        {
            get
            {
                return SaveData.Pad[0, 9];
            }
            set
            {
                SaveData.Pad[0, 9] = value;
            }
        }

        public int _Esc
        {
            get
            {
                return SaveData.Pad[0, 10];
            }
            set
            {
                SaveData.Pad[0, 10] = value;
            }
        }

        public int Turbo
        {
            get
            {
                return SaveData.Pad[0, 11];
            }
            set
            {
                SaveData.Pad[0, 11] = value;
            }
        }

        public MyKeyBoard(ShanghaiEXE f)
        {
			this.push = new HashSet<int>();
			this.press = new HashSet<int>();
			this.up = new HashSet<int>();
			Input.FormSetting(f);
        }

        public void GetKeyData()
        {
			var state = Keyboard.GetState();

			foreach(var key in TKKeys)
			{
				var shKeyCode = key.ToSHKeyCode();
				if (state.IsKeyDown(key))
				{
					if (this.push.Contains(shKeyCode))
					{
						this.press.Remove(shKeyCode);
					}
					else
					{
						this.press.Add(shKeyCode);
					}
					this.push.Add(shKeyCode);
				}
				else
				{
					if (this.push.Contains(shKeyCode))
					{
						this.up.Add(shKeyCode);
					}
					else
					{
						this.up.Remove(shKeyCode);
					}
					this.push.Remove(shKeyCode);
					this.press.Remove(shKeyCode);
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

        public void SetKeyData(ref HashSet<int> _push, ref HashSet<int> _press, ref HashSet<int> _up)
        {
			_push = new HashSet<int>(this.push);
			_press = new HashSet<int>(this.press);
			_up = new HashSet<int>(this.up);
		}
    }
}
