using Common.Configuration;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ExtensionMethods
{
	public static class InputExtensionMethods
	{
		private const double Sensitivity = 0.5;

		private static readonly Dictionary<Key, int> SHKeyCodes;
		private static readonly Dictionary<int, Key> ReverseSHKeyCodes;
		private static readonly Dictionary<XInputGamePadButton, Func<GamePadState, bool>> ButtonGetters;
		private static readonly Dictionary<XInputGamePadButton, int> SHButtonCodes;
		private static readonly Dictionary<int, XInputGamePadButton> ReverseSHButtonCodes;

		static InputExtensionMethods()
		{
			SHKeyCodes = new Dictionary<Key, int>
			{
				{ Key.Up, 132 },
				{ Key.Down, 50 },
				{ Key.Left, 76 },
				{ Key.Right, 118 },
				{ Key.A, 10 },
				{ Key.B, 11 },
				{ Key.C, 12 },
				{ Key.D, 13 },
				{ Key.E, 14 },
				{ Key.F, 15 },
				{ Key.G, 16 },
				{ Key.H, 17 },
				{ Key.I, 18 },
				{ Key.J, 19 },
				{ Key.K, 20 },
				{ Key.L, 21 },
				{ Key.M, 22 },
				{ Key.N, 23 },
				{ Key.O, 24 },
				{ Key.P, 25 },
				{ Key.Q, 26 },
				{ Key.R, 27 },
				{ Key.S, 28 },
				{ Key.T, 29 },
				{ Key.U, 30 },
				{ Key.V, 31 },
				{ Key.W, 32 },
				{ Key.X, 33 },
				{ Key.Y, 34 },
				{ Key.Z, 35 },
				{ Key.F1, 54 },
				{ Key.F2, 55 },
				{ Key.F3, 56 },
				{ Key.F4, 57 },
				{ Key.F5, 58 },
				{ Key.F6, 59 },
				{ Key.F7, 60 },
				{ Key.F8, 61 },
				{ Key.F9, 62 },
				{ Key.F10, 63 },
				{ Key.F11, 64 },
				{ Key.F12, 65 },
				{ Key.F13, -13 },
				{ Key.F14, -14 },
				{ Key.F15, -15 },
				{ Key.F16, -16 },
				{ Key.F17, -17 },
				{ Key.F18, -18 },
				{ Key.F19, -19 },
				{ Key.F20, -20 },
				{ Key.F21, -21 },
				{ Key.F22, -22 },
				{ Key.F23, -23 },
				{ Key.F24, -24 },
				{ Key.F25, -25 },
				{ Key.F26, -26 },
				{ Key.F27, -27 },
				{ Key.F28, -28 },
				{ Key.F29, -29 },
				{ Key.F30, -30 },
				{ Key.F31, -31 },
				{ Key.F32, -32 },
				{ Key.F33, -33 },
				{ Key.F34, -34 },
				{ Key.F35, -35 },
				{ Key.Keypad0, 89 },
				{ Key.Keypad1, 90 },
				{ Key.Keypad2, 91 },
				{ Key.Keypad3, 92 },
				{ Key.Keypad4, 93 },
				{ Key.Keypad5, 94 },
				{ Key.Keypad6, 95 },
				{ Key.Keypad7, 96 },
				{ Key.Keypad8, 97 },
				{ Key.Keypad9, 98 },
				{ Key.KeypadPlus, 104 },
//				{ Key.KeypadAdd, 104 },
				{ Key.KeypadMinus, 102 },
//				{ Key.KeypadSubtract, 102 },
				{ Key.KeypadPeriod, 103 },
//				{ Key.KeypadDecimal, 103 },
				{ Key.KeypadDivide, 105 },
				{ Key.KeypadEnter, 100 },
				{ Key.KeypadMultiply, 106 },
				{ Key.Number0, 0 },
				{ Key.Number1, 1 },
				{ Key.Number2, 2 },
				{ Key.Number3, 3 },
				{ Key.Number4, 4 },
				{ Key.Number5, 5 },
				{ Key.Number6, 6 },
				{ Key.Number7, 7 },
				{ Key.Number8, 8 },
				{ Key.Number9, 9 },
				{ Key.NumLock, 88 },
				{ Key.LAlt, 77 },
//				{ Key.AltLeft, 77 },
				{ Key.LBracket, 74 },
//				{ Key.BracketLeft, 74 },
				{ Key.LControl, 75 },
//				{ Key.ControlLeft, 75 },
				{ Key.LShift, 78 },
//				{ Key.ShiftLeft, 78 },
				{ Key.LWin, 79 },
//				{ Key.WinLeft, 79 },
				{ Key.RAlt, 119 },
//				{ Key.AltRight, 119 },
				{ Key.RBracket, 115 },
//				{ Key.BracketRight, 115 },
				{ Key.RControl, 116 },
//				{ Key.ControlRight, 116 },
				{ Key.RShift, 120 },
//				{ Key.ShiftRight, 120 },
				{ Key.RWin, 121 },
//				{ Key.WinRight, 121 },
//				{ Key.Back, 42 },
				{ Key.BackSlash, 43 },
				{ Key.BackSpace, 42 },
				{ Key.CapsLock, 45 },
//				{ Key.Clear, 94 },
				{ Key.Comma, 47 },
				{ Key.Command, -1 },
				{ Key.Delete, 49 },
				{ Key.End, 51 },
				{ Key.Enter, 117 },
				{ Key.Escape, 53 },
				{ Key.Grave, 69 },
				{ Key.Home, 70 },
				{ Key.Insert, 71 },
				{ Key.LastKey, -2 },
				{ Key.Menu, 39 },
				{ Key.Minus, 83 },
				{ Key.NonUSBackSlash, -3 },
				{ Key.PageDown, 108 },
				{ Key.PageUp, 109 },
				{ Key.Pause, 110 },
				{ Key.Period, 111 },
				{ Key.Plus, 52 },
				{ Key.PrintScreen, 128 },
				{ Key.Quote, 38 },
				{ Key.ScrollLock, 122 },
				{ Key.Semicolon, 123 },
				{ Key.Slash, 124 },
				{ Key.Sleep, -4 },
				{ Key.Space, 126 },
				{ Key.Tab, 129 },
//				{ Key.Tilde, 69 },
				{ Key.Unknown, -5 },
			};

			ReverseSHKeyCodes = new Dictionary<int, Key>();
			foreach (var kvp in SHKeyCodes)
			{
				if (!ReverseSHKeyCodes.ContainsKey(kvp.Value)
					|| ReverseSHKeyCodes[kvp.Value].ToString().Length > kvp.Key.ToString().Length)
				ReverseSHKeyCodes[kvp.Value] = kvp.Key;
			}

			ButtonGetters = new Dictionary<XInputGamePadButton, Func<GamePadState, bool>>
			{
				{ XInputGamePadButton.L1, (state) => state.Buttons.LeftShoulder == ButtonState.Pressed },
				{ XInputGamePadButton.L2, (state) => state.Triggers.Left >= Sensitivity },
				{ XInputGamePadButton.L3, (state) => state.Buttons.LeftStick == ButtonState.Pressed },
				{ XInputGamePadButton.R1, (state) => state.Buttons.RightShoulder == ButtonState.Pressed },
				{ XInputGamePadButton.R2, (state) => state.Triggers.Right >= Sensitivity },
				{ XInputGamePadButton.R3, (state) => state.Buttons.RightStick == ButtonState.Pressed },
				{ XInputGamePadButton.LUp, (state) => state.ThumbSticks.Left.Y >= Sensitivity },
				{ XInputGamePadButton.LDown, (state) => state.ThumbSticks.Left.Y <= -Sensitivity },
				{ XInputGamePadButton.LLeft, (state) => state.ThumbSticks.Left.X <= -Sensitivity },
				{ XInputGamePadButton.LRight, (state) => state.ThumbSticks.Left.X >= Sensitivity },
				{ XInputGamePadButton.RUp, (state) => state.ThumbSticks.Right.Y >= Sensitivity },
				{ XInputGamePadButton.RDown, (state) => state.ThumbSticks.Right.Y <= -Sensitivity },
				{ XInputGamePadButton.RLeft, (state) => state.ThumbSticks.Right.X <= -Sensitivity },
				{ XInputGamePadButton.RRight, (state) => state.ThumbSticks.Right.X >= Sensitivity },
				{ XInputGamePadButton.DPadUp, (state) => state.DPad.IsUp },
				{ XInputGamePadButton.DPadDown, (state) => state.DPad.IsDown },
				{ XInputGamePadButton.DPadLeft, (state) => state.DPad.IsLeft },
				{ XInputGamePadButton.DPadRight, (state) => state.DPad.IsRight },
				{ XInputGamePadButton.A, (state) => state.Buttons.A == ButtonState.Pressed },
				{ XInputGamePadButton.B, (state) => state.Buttons.B == ButtonState.Pressed },
				{ XInputGamePadButton.X, (state) => state.Buttons.X == ButtonState.Pressed },
				{ XInputGamePadButton.Y, (state) => state.Buttons.Y == ButtonState.Pressed },
				{ XInputGamePadButton.Select, (state) => state.Buttons.Back == ButtonState.Pressed },
				{ XInputGamePadButton.Center, (state) => state.Buttons.BigButton == ButtonState.Pressed },
				{ XInputGamePadButton.Start, (state) => state.Buttons.Start == ButtonState.Pressed }
			};

			SHButtonCodes = new Dictionary<XInputGamePadButton, int>
			{
				{ XInputGamePadButton.L1, 4 },
				{ XInputGamePadButton.L2, -4 },
				{ XInputGamePadButton.L3, 8 },
				{ XInputGamePadButton.R1, 5 },
				{ XInputGamePadButton.R2, -5 },
				{ XInputGamePadButton.R3, 9 },
				{ XInputGamePadButton.LUp, 100 },
				{ XInputGamePadButton.LDown, 101 },
				{ XInputGamePadButton.LLeft, 103 },
				{ XInputGamePadButton.LRight, 102 },
				{ XInputGamePadButton.RUp, -100 },
				{ XInputGamePadButton.RDown, -101 },
				{ XInputGamePadButton.RLeft, -103 },
				{ XInputGamePadButton.RRight, -102 },
				{ XInputGamePadButton.DPadUp, -104 },
				{ XInputGamePadButton.DPadDown, -105 },
				{ XInputGamePadButton.DPadLeft, -107 },
				{ XInputGamePadButton.DPadRight, -108 },
				{ XInputGamePadButton.A, 0 },
				{ XInputGamePadButton.B, 1 },
				{ XInputGamePadButton.X, 2 },
				{ XInputGamePadButton.Y, 3 },
				{ XInputGamePadButton.Select, 6 },
				{ XInputGamePadButton.Center, -10 },
				{ XInputGamePadButton.Start, 7 }
			};

			ReverseSHButtonCodes = SHButtonCodes.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
		}

		public static bool IsPressed(this GamePadState state, XInputGamePadButton button) => ButtonGetters[button](state);

		public static int ToSHButtonCode(this XInputGamePadButton button) => SHButtonCodes[button];
		public static XInputGamePadButton? FromSHButtonCode(this int buttonCode)
		{
			if (ReverseSHButtonCodes.TryGetValue(buttonCode, out var button))
			{
				return button;
			}
			else
			{
				return null;
			}
		}

		public static int ToSHKeyCode(this Key key) => SHKeyCodes[key];
		public static Key? FromSHKeyCode(this int keyCode)
		{
			if (ReverseSHKeyCodes.TryGetValue(keyCode, out var key))
			{
				return key;
			}
			else
			{
				return null;
			}
		}

		public static IEnumerable<Key> UniqueTKKeys => SHKeyCodes.Keys;
	}
}
