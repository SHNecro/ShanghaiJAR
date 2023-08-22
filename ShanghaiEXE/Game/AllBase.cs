using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
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
		protected IAudioEngine sound;
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
                // no idea what the point of this is, theoretically entire section can be removed and will be just as random
                //if (this.randomN != null)
                //    return this.randomN;
                //++AllBase.randomUseCount;
                //if (AllBase.randomUseCount > 9999)
                //{
                //    AllBase.randomUseCount = 0;
                //    AllBase.random = new Random(Environment.TickCount);
                //}
                // 
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

		public AllBase(IAudioEngine ad)
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
			for (var i = 0; i < numArray.Length; i++)
			{
				int num = (int)MyMath.Pow(10f, i);
				numArray[i] = m / num % 10;
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
			var specialCharIdxs = new List<int>();
			for (var i = 0; i < charArray.Length; i++)
			{
				if (i + 1 >= charArray.Length)
				{
					break;
				}
				if (charArray[i] == 'V' && (charArray[i + 1] == '1' || charArray[i + 1] == '2' || (charArray[i + 1] == '3' || charArray[i + 1] == '4') || (charArray[i + 1] == '5' || charArray[i + 1] == '6' || (charArray[i + 1] == '7' || charArray[i + 1] == '8')) || charArray[i + 1] == '9' || charArray[i + 1] == '0'))
					specialCharIdxs.Add(i - specialCharIdxs.Count);
			}
			for (var i = 0; i < charArray.Length; i++)
			{
				try
				{
					if (i >= name.Length) break;

					if (specialCharIdxs.Contains(i))
					{
						strArray[i] = name.Substring(i, 2);
						name = name.Remove(i + 1, 1);
					}
                    else
                    {
                        strArray[i] = name.Substring(i, 1);
                    }

                    var replacement = strArray[i];
                    switch (replacement)
                    {
                        case "+":
                        case "＋":
                            replacement = nameof(NAME.plus);
                            break;
                        case "-":
                        case "－":
                            replacement = nameof(NAME.ー);
                            break;
                        case "*":
                            replacement = nameof(NAME.kakeru);
                            break;
                        case "/":
                            replacement = nameof(NAME.slash);
                            break;
                        case "=":
                        case "＝":
                            replacement = nameof(NAME.equal);
                            break;
                        case ":":
                        case "：":
                            replacement = nameof(NAME.colon);
                            break;
                        case "?":
                        case "？":
                            replacement = nameof(NAME.question);
                            break;
                        case "!":
                        case "！":
                            replacement = nameof(NAME.exclamation);
                            break;
                        case ".":
                            replacement = nameof(NAME.dot);
                            break;
                        case ",":
                            replacement = nameof(NAME.readdot);
                            break;
                        case "&":
                            replacement = nameof(NAME.and);
                            break;
                        case "０":
                        case "0":
                            replacement = nameof(NAME.zero);
                            break;
                        case "１":
                        case "1":
                            replacement = nameof(NAME.one);
                            break;
                        case "２":
                        case "2":
                            replacement = nameof(NAME.two);
                            break;
                        case "３":
                        case "3":
                            replacement = nameof(NAME.three);
                            break;
                        case "４":
                        case "4":
                            replacement = nameof(NAME.four);
                            break;
                        case "５":
                        case "5":
                            replacement = nameof(NAME.five);
                            break;
                        case "６":
                        case "6":
                            replacement = nameof(NAME.six);
                            break;
                        case "７":
                        case "7":
                            replacement = nameof(NAME.seven);
                            break;
                        case "８":
                        case "8":
                            replacement = nameof(NAME.eight);
                            break;
                        case "９":
                        case "9":
                            replacement = nameof(NAME.night);
                            break;
                        case "Ｖ":
                            replacement = nameof(NAME.V);
                            break;
                        case "a":
                            replacement = nameof(NAME.al);
                            break;
                        case "b":
                            replacement = nameof(NAME.bl);
                            break;
                        case "c":
                            replacement = nameof(NAME.cl);
                            break;
                        case "d":
                            replacement = nameof(NAME.dl);
                            break;
                        case "e":
                            replacement = nameof(NAME.el);
                            break;
                        case "f":
                            replacement = nameof(NAME.fl);
                            break;
                        case "g":
                            replacement = nameof(NAME.gl);
                            break;
                        case "h":
                            replacement = nameof(NAME.hl);
                            break;
                        case "i":
                            replacement = nameof(NAME.il);
                            break;
                        case "j":
                            replacement = nameof(NAME.jl);
                            break;
                        case "k":
                            replacement = nameof(NAME.kl);
                            break;
                        case "m":
                            replacement = nameof(NAME.ml);
                            break;
                        case "l":
                            replacement = nameof(NAME.ll);
                            break;
                        case "n":
                            replacement = nameof(NAME.nl);
                            break;
                        case "o":
                            replacement = nameof(NAME.ol);
                            break;
                        case "p":
                            replacement = nameof(NAME.pl);
                            break;
                        case "q":
                            replacement = nameof(NAME.ql);
                            break;
                        case "r":
                            replacement = nameof(NAME.rl);
                            break;
                        case "s":
                            replacement = nameof(NAME.sl);
                            break;
                        case "t":
                            replacement = nameof(NAME.tl);
                            break;
                        case "u":
                            replacement = nameof(NAME.ul);
                            break;
                        case "v":
                            replacement = nameof(NAME.vl);
                            break;
                        case "w":
                            replacement = nameof(NAME.wl);
                            break;
                        case "x":
                            replacement = nameof(NAME.xl);
                            break;
                        case "y":
                            replacement = nameof(NAME.yl);
                            break;
                        case "z":
                            replacement = nameof(NAME.zl);
                            break;
                        case "・":
                        case "･":
                            replacement = nameof(NAME.ten);
                            break;
                        case "＊":
                            replacement = nameof(NAME.asterisk);
                            break;
                        case "　":
                        case " ":
                            replacement = nameof(NAME.no);
                            break;
                        case "(":
                        case "（":
                            replacement = nameof(NAME.marukakko);
                            break;
                        case ")":
                        case "）":
                            replacement = nameof(NAME.marukakkoTojiru);
                            break;
                        case "[":
                        case "「":
                            replacement = nameof(NAME.kagikakko);
                            break;
                        case "]":
                        case "」":
                            replacement = nameof(NAME.kagikakkoTojiru);
                            break;
                    }
                    strArray[i] = replacement;
				}
				catch
				{
					break;
				}
			}
			for (var i = 0; i < charArray.Length; i++)
			{
				if (strArray[i] != null)
                {
                    NAME character;
                    var parseSuccess = Enum.TryParse(strArray[i], out character);

                    if (parseSuccess)
                    {
                        nameArray[i] = character;
                    }
                    else
                    {
                        if (strArray[i].Length == 1)
                        {
                            nameArray[i] = (NAME)(-strArray[i][0]);
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid multi-character block character");
                        }
                    }
                }
            }
			return nameArray;
		}

        public static void DrawBlockCharacters(IRenderer dg, IList<NAME> charactersOrFallback, int textTypeOffset, Vector2 position, Color color, out Rectangle finalRect, out Vector2 finalPosition, bool reversed = false)
        {
            var currentRect = new Rectangle();
            var currentPosition = position;
            for (int i = 0; i < charactersOrFallback.Count; i++)
            {
                var characterOrFallback = charactersOrFallback[i];
                var characterOffset = (int)characterOrFallback;
                if (characterOffset >= 0)
                {
                    var rect = new Rectangle(characterOffset * 8, textTypeOffset, 8, 16);
                    dg.DrawImage(dg, "font", rect, true, currentPosition, color);
                    currentPosition.X += reversed ? -8 : 8;
                    currentRect = rect;
                }
                else
                {
                    var contiguousText = charactersOrFallback.Skip(i).TakeWhile(n => n < 0);
                    i += contiguousText.Count() - 1;

                    var shadow = textTypeOffset < 48;
                    var plain = textTypeOffset >= 48 && textTypeOffset < 72;
                    var outlined = textTypeOffset >= 72;

                    var offsets = new int[0, 0];
                    if (shadow)
                    {
                        offsets = new int[,]
                        {
                        { 1, 1 }
                        };
                    }
                    else if (outlined)
                    {
                        offsets = new int[,]
                        {
                        { -1, -1 }, { 0, -1 }, { 1, -1 },
                        { -1, 0 }, { 1, 0 },
                        { -1, 1 }, { 0, 1 }, { 1, 1 },
                        };
                    }
                    
                    var charString = new string(contiguousText.Select(n => (char)(-(int)n)).ToArray());
                    var textWidth = dg.GetTextMeasurer().MeasureMiniText(charString);

                    if (reversed)
                    {
                        currentPosition.X -= textWidth.Width;
                    }

                    for (var entryIndex = 0; entryIndex < offsets.GetLength(0); entryIndex++)
                    {
                        var offPosition = new Vector2(currentPosition.X + offsets[entryIndex, 0], currentPosition.Y + offsets[entryIndex, 1]);
                        dg.DrawMiniText(charString, offPosition, Color.Black);
                    }

                    dg.DrawMiniText(charString, currentPosition, color);

                    if (reversed)
                    {
                        currentPosition.X -= 8;
                    }
                    else
                    {
                        currentPosition.X += textWidth.Width;
                    }
                }
            }

            finalPosition = currentPosition;
            finalRect = currentRect;
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
			if (!LeftRight)
			{
				vector2 = position;
			}
			else
			{
				vector2 = new Vector2(position.X - 8 * (nameArray.Length - 1), position.Y);
				nameArray = ((IEnumerable<AllBase.NAME>)nameArray).Reverse<AllBase.NAME>().ToArray<AllBase.NAME>();
			}
            this._position = new Vector2(vector2.X, vector2.Y);
            DrawBlockCharacters(dg, nameArray, shadow ? 16 : 88, this._position, Color.White, out this._rect, out this._position, LeftRight);
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
			if (!LeftRight)
			{
				vector2 = position;
			}
			else
			{
				vector2 = new Vector2(position.X - 8 * (nameArray.Length - 1), position.Y);
			}
            this._position = new Vector2(vector2.X, vector2.Y);
            DrawBlockCharacters(dg, nameArray, shadow ? 16 : 88, this._position, color, out this._rect, out this._position);
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
			no = 0,
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
			none
		}
	}
}
