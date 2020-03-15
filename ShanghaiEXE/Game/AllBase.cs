using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Button = NSShanghaiEXE.InputOutput.Button;
using System.Text;

namespace NSGame
{
    public class AllBase
	{
		public static bool DrawDebug = false;
		protected static Random random = new Random(Environment.TickCount);
		protected Button[] commandList = new Button[20];
		protected Rectangle _rect;
		protected Vector2 _position;
		protected MyAudio sound;
		protected static int randomUseCount;
		public Random randomN;
		protected int frame;
		public Point shakeSingle;
		private int shakeSingleLevel;
		private bool shakeSingleFlag;
		private int shakeSingleFlame;
		public static Point shake;
		private static int shakeLevel;
		private static bool shakeFlag;
		private static int shakeFlame;
		private int flamesub;
		protected bool moveflame;
		protected int commandTime;

		private static readonly Dictionary<string, string> SpecialText = new Dictionary<string, string>
		{
			{ "EX", "V4" },
			{ "SP", "V5" },
			{ "DS", "V6" },
			{ "RV", "V7" },
			{ "BX", "V8" }
		};

		public Random Random
		{
			get
			{
				if (this.randomN != null)
					return this.randomN;
				++AllBase.randomUseCount;
				if (AllBase.randomUseCount > 9999)
				{
					AllBase.randomUseCount = 0;
					AllBase.random = new Random(Environment.TickCount);
				}
				return AllBase.random;
			}
		}

		public Point Shake
		{
			get
			{
				return new Point(AllBase.shake.X + this.shakeSingle.X, AllBase.shake.Y + this.shakeSingle.Y);
			}
		}

		public bool ShakeFlag
		{
			get
			{
				return AllBase.shakeFlag;
			}
		}

		public AllBase(MyAudio ad)
		{
			this.sound = ad;
		}

		public void FlamePlus()
		{
			++this.frame;
			if (this.frame < int.MaxValue)
				return;
			this.frame = 0;
		}

		protected void CommandReset()
		{
			this.commandList = new Button[20];
		}

		protected void CommandAdd(Button button)
		{
			for (int index = 0; index < this.commandList.Length - 1; ++index)
				this.commandList[this.commandList.Length - index - 1] = this.commandList[this.commandList.Length - index - 2];
			this.commandList[0] = button;
		}

		protected void CommandInput(string button, Player player)
		{
			if (player.InputIsPress(Button._A) && button.Contains('A'))
			{
				this.CommandAdd(Button._A);
			}
			else if (player.InputIsPress(Button._B) && button.Contains('B'))
			{
				this.CommandAdd(Button._B);
			}
			else if (player.InputIsPress(Button._L) && button.Contains('L'))
			{
				this.CommandAdd(Button._L);
			}
			else if (player.InputIsPress(Button._R) && button.Contains('R'))
			{
				this.CommandAdd(Button._R);
			}
			else if (player.InputIsPress(Button._Start) && button.Contains('S'))
			{
				this.CommandAdd(Button._Start);
			}
			else if (player.InputIsPress(Button._Select) && button.Contains('s'))
			{
				this.CommandAdd(Button._Select);
			}
			else if (player.InputIsPress(Button.Up) && button.Contains('上'))
			{
				this.CommandAdd(Button.Up);
			}
			else if (player.InputIsPress(Button.Down) && button.Contains('下'))
			{
				this.CommandAdd(Button.Down);
			}
			else if (player.InputIsPress(Button.Left) && button.Contains('左'))
			{
				this.CommandAdd(Button.Left);
			}
			else if (player.InputIsPress(Button.Right) && button.Contains('右'))
			{
				this.CommandAdd(Button.Right);
			}
			++this.commandTime;
		}

		protected void CommandInput(string button)
		{
			if (Input.IsPress(Button._A) && button.Contains('A'))
			{
				this.CommandAdd(Button._A);
			}
			else if (Input.IsPress(Button._B) && button.Contains('B'))
			{
				this.CommandAdd(Button._B);
			}
			else if (Input.IsPress(Button._L) && button.Contains('L'))
			{
				this.CommandAdd(Button._L);
			}
			else if (Input.IsPress(Button._R) && button.Contains('R'))
			{
				this.CommandAdd(Button._R);
			}
			else if (Input.IsPress(Button._Start) && button.Contains('S'))
			{
				this.CommandAdd(Button._Start);
			}
			else if (Input.IsPress(Button._Select) && button.Contains('s'))
			{
				this.CommandAdd(Button._Select);
			}
			else if (Input.IsPress(Button.Up) && button.Contains('上'))
			{
				this.CommandAdd(Button.Up);
			}
			else if (Input.IsPress(Button.Down) && button.Contains('下'))
			{
				this.CommandAdd(Button.Down);
			}
			else if (Input.IsPress(Button.Left) && button.Contains('左'))
			{
				this.CommandAdd(Button.Left);
			}
			else if (Input.IsPress(Button.Right) && button.Contains('右'))
			{
				this.CommandAdd(Button.Right);
			}
			++this.commandTime;
		}

		protected bool CommandCheck(string command)
		{
			List<char> charList = new List<char>();
			for (int index = 0; index < command.Length; ++index)
				charList.Add(command[index]);
			for (int index1 = 0; index1 < charList.Count; ++index1)
			{
				Button buttun = Button.Esc;
				int index2 = charList.Count - 1 - index1;
				if (charList[index2] == 'A')
					buttun = Button._A;
				if (charList[index2] == 'B')
					buttun = Button._B;
				if (charList[index2] == 'R')
					buttun = Button._R;
				if (charList[index2] == 'L')
					buttun = Button._L;
				if (charList[index2] == 's')
					buttun = Button._Select;
				if (charList[index2] == 'S')
					buttun = Button._Start;
				if (charList[index2] == '上')
					buttun = Button.Up;
				if (charList[index2] == '右')
					buttun = Button.Right;
				if (charList[index2] == '左')
					buttun = Button.Left;
				if (charList[index2] == '下')
					buttun = Button.Down;
				if (this.commandList[index1] != buttun)
					return false;
			}
			return true;
		}

		protected void FlameControl(int speed)
		{
			++this.flamesub;
			if (this.flamesub >= speed)
			{
				++this.frame;
				this.moveflame = true;
				this.flamesub = 0;
				if (this.frame < int.MaxValue)
					return;
				this.frame = 0;
			}
			else
				this.moveflame = false;
		}

		public int[] ChangeCount(int m)
		{
			int[] numArray = new int[m.ToString().Length];
			foreach (var data in ((IEnumerable<int>)numArray).Select((v, i) => new
			{
				v,
				i
			}))
			{
				int num = (int)MyMath.Pow(10f, data.i);
				numArray[data.i] = m / num % 10;
			}
			return numArray;
		}

		public void FlameReset()
		{
			this.flamesub = 0;
			this.frame = 0;
		}

		public AllBase.NAME[] Nametodata(string name)
		{
			if (name == null)
				return new AllBase.NAME[1];
			foreach (var text in SpecialText.Keys)
			{
				var swapText = new StringBuilder("---");
				while (name.Contains(swapText.ToString()))
				{
					swapText.Append("-");
				}
				name = name.Replace($"@{text}", swapText.ToString());
				name = name.Replace(text, SpecialText[text]);
				name = name.Replace(swapText.ToString(), text);
			}
			char[] charArray = name.ToCharArray();
			string[] strArray = new string[charArray.Length];
			AllBase.NAME[] nameArray = new AllBase.NAME[charArray.Length];
			int num = -1;
			foreach (var data in ((IEnumerable<char>)charArray).Select((va, i) => new
			{
				va,
				i
			}))
			{
				if (data.i + 1 >= charArray.Length)
				{
					break;
				}
				if (charArray[data.i] == 'V' && (charArray[data.i + 1] == '1' || charArray[data.i + 1] == '2' || (charArray[data.i + 1] == '3' || charArray[data.i + 1] == '4') || (charArray[data.i + 1] == '5' || charArray[data.i + 1] == '6' || (charArray[data.i + 1] == '7' || charArray[data.i + 1] == '8')) || charArray[data.i + 1] == '9' || charArray[data.i + 1] == '0'))
					num = data.i;
			}
			foreach (var data in ((IEnumerable<char>)charArray).Select((va, i) => new
			{
				va,
				i
			}))
			{
				try
				{
					if (num == data.i)
					{
						strArray[data.i] = name.Substring(data.i, 2);
						name = name.Remove(data.i + 1, 1);
					}
					else
						strArray[data.i] = name.Substring(data.i, 1);
					if (strArray[data.i] == "+" || strArray[data.i] == "＋")
						strArray[data.i] = "plus";
					if (strArray[data.i] == "-" || strArray[data.i] == "－")
						strArray[data.i] = "ー";
					if (strArray[data.i] == "*")
						strArray[data.i] = "kakeru";
					if (strArray[data.i] == "/")
						strArray[data.i] = "slash";
					if (strArray[data.i] == "=" || strArray[data.i] == "＝")
						strArray[data.i] = "equal";
					if (strArray[data.i] == ":" || strArray[data.i] == "：")
						strArray[data.i] = "colon";
					if (strArray[data.i] == "?" || strArray[data.i] == "？")
						strArray[data.i] = "question";
					if (strArray[data.i] == "!" || strArray[data.i] == "！")
						strArray[data.i] = "exclamation";
					if (strArray[data.i] == ".")
						strArray[data.i] = "dot";
					if (strArray[data.i] == ",")
						strArray[data.i] = "readdot";
					if (strArray[data.i] == "&")
						strArray[data.i] = "and";
					if (strArray[data.i] == "０" || strArray[data.i] == "0")
						strArray[data.i] = "zero";
					if (strArray[data.i] == "１" || strArray[data.i] == "1")
						strArray[data.i] = "one";
					if (strArray[data.i] == "２" || strArray[data.i] == "2")
						strArray[data.i] = "two";
					if (strArray[data.i] == "３" || strArray[data.i] == "3")
						strArray[data.i] = "three";
					if (strArray[data.i] == "４" || strArray[data.i] == "4")
						strArray[data.i] = "four";
					if (strArray[data.i] == "５" || strArray[data.i] == "5")
						strArray[data.i] = "five";
					if (strArray[data.i] == "６" || strArray[data.i] == "6")
						strArray[data.i] = "six";
					if (strArray[data.i] == "７" || strArray[data.i] == "7")
						strArray[data.i] = "seven";
					if (strArray[data.i] == "８" || strArray[data.i] == "8")
						strArray[data.i] = "eight";
					if (strArray[data.i] == "９" || strArray[data.i] == "9")
						strArray[data.i] = "night";
					if (strArray[data.i] == "Ｖ")
						strArray[data.i] = "V";
					if (strArray[data.i] == "a")
						strArray[data.i] = "al";
					if (strArray[data.i] == "b")
						strArray[data.i] = "bl";
					if (strArray[data.i] == "c")
						strArray[data.i] = "cl";
					if (strArray[data.i] == "d")
						strArray[data.i] = "dl";
					if (strArray[data.i] == "e")
						strArray[data.i] = "el";
					if (strArray[data.i] == "f")
						strArray[data.i] = "fl";
					if (strArray[data.i] == "g")
						strArray[data.i] = "gl";
					if (strArray[data.i] == "h")
						strArray[data.i] = "hl";
					if (strArray[data.i] == "i")
						strArray[data.i] = "il";
					if (strArray[data.i] == "j")
						strArray[data.i] = "jl";
					if (strArray[data.i] == "k")
						strArray[data.i] = "kl";
					if (strArray[data.i] == "m")
						strArray[data.i] = "ml";
					if (strArray[data.i] == "l")
						strArray[data.i] = "ll";
					if (strArray[data.i] == "n")
						strArray[data.i] = "nl";
					if (strArray[data.i] == "o")
						strArray[data.i] = "ol";
					if (strArray[data.i] == "p")
						strArray[data.i] = "pl";
					if (strArray[data.i] == "q")
						strArray[data.i] = "ql";
					if (strArray[data.i] == "r")
						strArray[data.i] = "rl";
					if (strArray[data.i] == "s")
						strArray[data.i] = "sl";
					if (strArray[data.i] == "t")
						strArray[data.i] = "tl";
					if (strArray[data.i] == "u")
						strArray[data.i] = "ul";
					if (strArray[data.i] == "v")
						strArray[data.i] = "vl";
					if (strArray[data.i] == "w")
						strArray[data.i] = "wl";
					if (strArray[data.i] == "x")
						strArray[data.i] = "xl";
					if (strArray[data.i] == "y")
						strArray[data.i] = "yl";
					if (strArray[data.i] == "z")
						strArray[data.i] = "zl";
					if (strArray[data.i] == "・" || strArray[data.i] == "･")
						strArray[data.i] = "ten";
					if (strArray[data.i] == "＊")
						strArray[data.i] = "asterisk";
					if (strArray[data.i] == "　" || strArray[data.i] == " ")
						strArray[data.i] = "no";
					if (strArray[data.i] == "(" || strArray[data.i] == "（")
						strArray[data.i] = "marukakko";
					if (strArray[data.i] == ")" || strArray[data.i] == "）")
						strArray[data.i] = "marukakkoTojiru";
					if (strArray[data.i] == "[" || strArray[data.i] == "「")
						strArray[data.i] = "kagikakko";
					if (strArray[data.i] == "]" || strArray[data.i] == "」")
						strArray[data.i] = "kagikakkoTojiru";
				}
				catch
				{
					break;
				}
			}
			foreach (var data in ((IEnumerable<char>)charArray).Select((va, i) => new
			{
				va,
				i
			}))
			{
				if (strArray[data.i] != null)
					nameArray[data.i] = (AllBase.NAME)Enum.Parse(typeof(AllBase.NAME), strArray[data.i], true);
			}
			return nameArray;
		}

		public void TextRender(
		  IRenderer dg,
		  string txt,
		  bool LeftRight,
		  Vector2 position,
		  bool shadow)
		{
			AllBase.NAME[] nameArray = this.Nametodata(txt);
			Vector2 vector2;
			int num1;
			if (!LeftRight)
			{
				vector2 = position;
				num1 = 8;
			}
			else
			{
				vector2 = new Vector2(position.X - 8 * (nameArray.Length - 1), position.Y);
				nameArray = ((IEnumerable<AllBase.NAME>)nameArray).Reverse<AllBase.NAME>().ToArray<AllBase.NAME>();
				num1 = -8;
			}
			int num2 = shadow ? 16 : 32;
			foreach (var data in ((IEnumerable<AllBase.NAME>)nameArray).Select((v, j) => new
			{
				v,
				j
			}))
			{
				this._rect = new Rectangle((int)data.v * 8, shadow ? 16 : 88, 8, 16);
				this._position = new Vector2(vector2.X + num1 * data.j, vector2.Y);
				dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
			}
		}

		public void TextRender(
		  IRenderer dg,
		  string txt,
		  bool LeftRight,
		  Vector2 position,
		  bool shadow,
		  Color color)
		{
			AllBase.NAME[] nameArray = this.Nametodata(txt);
			Vector2 vector2;
			int num1;
			if (!LeftRight)
			{
				vector2 = position;
				num1 = 8;
			}
			else
			{
				vector2 = new Vector2(position.X - 8 * (nameArray.Length - 1), position.Y);
				num1 = 8;
			}
			int num2 = shadow ? 16 : 32;
			foreach (var data in ((IEnumerable<AllBase.NAME>)nameArray).Select((v, j) => new
			{
				v,
				j
			}))
			{
				this._rect = new Rectangle((int)data.v * 8, shadow ? 16 : 88, 8, 16);
				this._position = new Vector2(vector2.X + num1 * data.j, vector2.Y);
				dg.DrawImage(dg, "font", this._rect, true, this._position, color);
			}
		}

		public void ShakeStart(int level)
		{
			AllBase.shakeFlame = -1;
			AllBase.shakeFlag = true;
			AllBase.shakeLevel = level;
		}

		public void ShakeStart(int level, int flame)
		{
			if (AllBase.shakeFlame < flame)
				AllBase.shakeFlame = flame;
			AllBase.shakeFlag = true;
			AllBase.shakeLevel = level;
		}

		public void ShakeEnd()
		{
			AllBase.shakeFlag = false;
			AllBase.shakeFlame = -1;
			AllBase.shake = new Point();
		}

		public void Shaking()
		{
			AllBase.shake = new Point(this.Random.Next(-AllBase.shakeLevel, AllBase.shakeLevel), this.Random.Next(-AllBase.shakeLevel, AllBase.shakeLevel));
			if (AllBase.shakeFlame == 0)
				this.ShakeEnd();
			if (AllBase.shakeFlame < 0)
				return;
			--AllBase.shakeFlame;
		}

		public void ShakingSingle()
		{
			if (!this.shakeSingleFlag)
				return;
			this.shakeSingle = new Point(this.Random.Next(-this.shakeSingleLevel, this.shakeSingleLevel), this.Random.Next(-this.shakeSingleLevel, this.shakeSingleLevel));
			if (this.shakeSingleFlame <= 0)
				this.ShakeSingleEnd();
			if (this.shakeSingleFlame >= 0)
				--this.shakeSingleFlame;
		}

		public void ShakeSingleStart(int level, int flame)
		{
			if (AllBase.shakeFlame < flame)
				this.shakeSingleFlame = flame;
			this.shakeSingleFlag = true;
			this.shakeSingleLevel = level;
		}

		public void ShakeSingleEnd()
		{
			this.shakeSingleFlag = false;
			this.shakeSingleFlame = -1;
			this.shakeSingle = new Point();
		}

		public enum NAME
		{
			no,
			ア,
			イ,
			ウ,
			エ,
			オ,
			カ,
			キ,
			ク,
			ケ,
			コ,
			サ,
			シ,
			ス,
			セ,
			ソ,
			タ,
			チ,
			ツ,
			テ,
			ト,
			ナ,
			ニ,
			ヌ,
			ネ,
			ノ,
			ハ,
			ヒ,
			フ,
			ヘ,
			ホ,
			マ,
			ミ,
			ム,
			メ,
			モ,
			ヤ,
			ユ,
			ヨ,
			ラ,
			リ,
			ル,
			レ,
			ロ,
			ワ,
			ヲ,
			ン,
			ガ,
			ギ,
			グ,
			ゲ,
			ゴ,
			ザ,
			ジ,
			ズ,
			ゼ,
			ゾ,
			ダ,
			ヂ,
			ヅ,
			デ,
			ド,
			バ,
			ビ,
			ブ,
			ベ,
			ボ,
			パ,
			ピ,
			プ,
			ペ,
			ポ,
			ヴ,
			ァ,
			ィ,
			ゥ,
			ェ,
			ォ,
			ッ,
			ャ,
			ュ,
			ョ,
			Ω,
			Σ,
			plus,
			ー,
			kakeru,
			slash,
			equal,
			colon,
			question,
			exclamation,
			V2,
			V3,
			V4,
			V5,
			V6,
			V7,
			V8,
			dot,
			readdot,
			and,
			zero,
			one,
			two,
			three,
			four,
			five,
			six,
			seven,
			eight,
			night,
			A,
			B,
			C,
			D,
			E,
			F,
			G,
			H,
			I,
			J,
			K,
			L,
			M,
			N,
			O,
			P,
			Q,
			R,
			S,
			T,
			U,
			V,
			W,
			X,
			Y,
			Z,
			asterisk,
			al,
			bl,
			cl,
			dl,
			el,
			fl,
			gl,
			hl,
			il,
			jl,
			kl,
			ll,
			ml,
			nl,
			ol,
			pl,
			ql,
			rl,
			sl,
			tl,
			ul,
			vl,
			wl,
			xl,
			yl,
			zl,
			ten,
			marukakko,
			marukakkoTojiru,
			kagikakko,
			kagikakkoTojiru,
			none,
		}
	}
}
