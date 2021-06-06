using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap.Character;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEvent
{
    public class VirusManager : EventBase
    {
        private readonly List<string> questlist = new List<string>();
        private readonly List<int> questlistNumber = new List<int>();
        private VirusManager.SCENE nowscene;
        private readonly EventManager eventmanager;
        private byte alpha;
        protected const string texture = "menuwindows";
        private bool shopmode;
        private int cursol;
        private bool release;
        private int cursolanime;
        private bool cursolanime2;
        private bool selectedForB;
        private bool right;
        private bool selected;
        private int selectedCursol;
        private readonly int overTop;
        private bool help;
        private int top;
        private int waittime;
        private const byte waitlong = 10;
        private const byte waitshort = 4;
        private readonly InfoMessage info;

        private int Select
        {
            get
            {
                return this.cursol + this.top;
            }
        }

        public VirusManager(IAudioEngine s, EventManager m, Player player, SaveData save)
          : base(s, m, save)
        {
            this.info = player.info;
            this.eventmanager = new EventManager(this.sound);
            this.NoTimeNext = false;
            this.overTop = this.questlistNumber.Count - 8;
            if (this.overTop >= 0)
                return;
            this.overTop = 0;
        }

        public override void Update()
        {
            switch (this.nowscene)
            {
                case VirusManager.SCENE.fadein:
                    if (!this.shopmode)
                    {
                        this.alpha += 51;
                        if (this.alpha < byte.MaxValue)
                            break;
                        this.alpha = byte.MaxValue;
                        this.shopmode = true;
                        break;
                    }
                    this.alpha -= 51;
                    if (this.alpha <= 0)
                    {
                        this.savedata.selectQuestion = 1;
                        this.nowscene = VirusManager.SCENE.select;
                    }
                    break;
                case VirusManager.SCENE.select:
                    this.FlameControl(1);
                    if (this.moveflame)
                    {
                        if (this.frame % 10 == 0)
                            ++this.cursolanime;
                        if (this.cursolanime >= 3)
                            this.cursolanime = 0;
                        if (this.frame % 3 == 0)
                            this.cursolanime2 = !this.cursolanime2;
                    }
                    if (this.release)
                        this.eventmanager.UpDate();
                    else
                        this.Control();
                    if (!this.release || this.eventmanager.playevent)
                        break;
                    this.release = false;
                    if (this.savedata.selectQuestion == 0)
                    {
                        if (this.right)
						{
							this.savedata.stockVirus.RemoveAt(this.Select);
							if (this.Select >= this.savedata.stockVirus.Count)
							{
								if (this.top > 0)
								{
									--this.top;
								}
								else if (this.cursol > 0)
								{
									--this.cursol;
								}
								else
								{
									this.right = false;
								}
							}
							if (this.selectedCursol >= this.savedata.stockVirus.Count)
							{
								this.selected = false;
							}
						}
                        else
						{
							this.savedata.HaveVirus[this.cursol] = null;
						}
                    }
                    break;
                case VirusManager.SCENE.fadeout:
                    if (this.shopmode)
                    {
                        this.alpha += 51;
                        if (this.alpha < byte.MaxValue)
                            break;
                        this.alpha = byte.MaxValue;
                        this.shopmode = false;
                        break;
                    }
                    this.alpha -= 51;
                    if (this.alpha <= 0)
                    {
                        this.nowscene = VirusManager.SCENE.fadein;
                        this.cursol = 0;
                        this.top = 0;
                        this.manager.parent.main.FolderReset();
                        this.EndCommand();
                    }
                    break;
            }
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.Decide();
            }
            else if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                if (this.selected)
                    this.selected = false;
                else if (this.help)
                    this.help = false;
                else
                    this.nowscene = VirusManager.SCENE.fadeout;
            }
            if (Input.IsPress(Button._Start))
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.help = !this.help;
            }
            if (Input.IsPress(Button._Select))
            {
				if (!this.right && this.savedata.HaveVirus[this.cursol] == null)
				{
					this.sound.PlaySE(SoundEffect.error);
				}
				else
				{
					this.sound.PlaySE(SoundEffect.decide);
					this.release = true;
					this.MessageMake();
				}
            }
            else if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up))
                {
                    if (this.right)
                    {
                        if (this.Select <= 0)
                            return;
                        if (this.cursol > 0)
                            --this.cursol;
                        else
                            --this.top;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                    }
                    else
                    {
                        if (this.cursol > 0)
                            --this.cursol;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                    }
                }
                else if (Input.IsPush(Button.Down))
                {
                    if (this.right)
                    {
                        if (this.Select >= this.savedata.stockVirus.Count - 1)
                            return;
                        if (this.cursol < 3)
                            ++this.cursol;
                        else if (this.right)
                            ++this.top;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                    }
                    else if (this.Select < this.savedata.haveCaptureBomb - 1)
                    {
                        if (this.cursol < this.savedata.haveCaptureBomb)
                            ++this.cursol;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                    }
                }
                else if (Input.IsPush(Button.Left))
                {
                    if (!this.right)
                        return;
                    this.right = false;
                    if (this.cursol >= this.savedata.haveCaptureBomb)
                        this.cursol = this.savedata.haveCaptureBomb - 1;
                    this.sound.PlaySE(SoundEffect.movecursol);
                }
                else
                {
                    if (!Input.IsPush(Button.Right) || (this.right || this.savedata.stockVirus.Count <= 0))
                        return;
                    this.right = true;
                    this.sound.PlaySE(SoundEffect.movecursol);
                }
            }
            else
                --this.waittime;
        }

        private void Decide()
        {
            if (!this.selected)
            {
                this.selectedForB = this.right;
                this.selected = true;
                this.selectedCursol = !this.right ? this.cursol : this.Select;
            }
            else
            {
                if (this.selectedForB)
                {
                    if (this.right)
                    {
                        if (this.Select != this.selectedCursol)
                        {
                            Virus stockViru = this.savedata.stockVirus[this.Select];
                            this.savedata.stockVirus[this.Select] = this.savedata.stockVirus[this.selectedCursol];
                            this.savedata.stockVirus[this.selectedCursol] = stockViru;
                        }
                        else
                        {
                            bool flag = false;
                            for (int index = 0; index < this.savedata.haveCaptureBomb; ++index)
                            {
                                if (this.savedata.HaveVirus[index] == null)
                                {
                                    this.savedata.HaveVirus[index] = this.savedata.stockVirus[this.Select];
                                    this.savedata.stockVirus.RemoveAt(this.Select);
                                    if (this.top > 0)
                                        --this.top;
                                    else if (this.cursol > 0)
                                        --this.cursol;
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                                this.sound.PlaySE(SoundEffect.error);
                        }
                    }
                    else
                    {
                        Virus stockViru = this.savedata.stockVirus[this.selectedCursol];
                        if (this.savedata.HaveVirus[this.cursol] != null)
                            this.savedata.stockVirus[this.selectedCursol] = this.savedata.HaveVirus[this.cursol];
                        else
                            this.savedata.stockVirus.RemoveAt(this.selectedCursol);
                        this.savedata.HaveVirus[this.cursol] = stockViru;
                    }
                }
                else if (this.right)
                {
                    Virus stockViru = this.savedata.stockVirus[this.Select];
                    if (this.savedata.HaveVirus[this.selectedCursol] != null)
                        this.savedata.stockVirus[this.Select] = this.savedata.HaveVirus[this.selectedCursol];
                    else
                        this.savedata.stockVirus.RemoveAt(this.Select);
                    this.savedata.HaveVirus[this.selectedCursol] = stockViru;
                }
                else if (this.cursol != this.selectedCursol)
                {
                    Virus haveViru = this.savedata.HaveVirus[this.cursol];
                    this.savedata.HaveVirus[this.cursol] = this.savedata.HaveVirus[this.selectedCursol];
                    this.savedata.HaveVirus[this.selectedCursol] = haveViru;
                }
                else if (this.savedata.HaveVirus[this.selectedCursol] != null)
                {
                    this.savedata.stockVirus.Add(this.savedata.HaveVirus[this.cursol]);
                    this.savedata.HaveVirus[this.cursol] = null;
                }
                this.selected = false;
            }
            if (!this.right || this.savedata.stockVirus.Count > 0)
                return;
            this.right = false;
            this.selectedCursol = 0;
        }

        private void MessageMake()
        {
            this.savedata.selectQuestion = 1;
            this.eventmanager.events.Clear();
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            var question = ShanghaiEXE.Translate("VirusManager.UnneededVirusQuestion");
            var options = ShanghaiEXE.Translate("VirusManager.UnneededVirusOptions");
            this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, question[0], question[1], options[0], options[1], false, true, question.Face, this.savedata, true));
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
        }

        public override void Render(IRenderer dg)
        {
            if (this.shopmode)
            {
                this._position = new Vector2(0.0f, 0.0f);
                this._rect = new Rectangle(480, 304, 240, 160);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                Vector2 vector2;
                if (!this.help || !this.right)
                {
                    for (int index = 0; index < this.savedata.haveCaptureBomb; ++index)
                    {
                        this._position = new Vector2(0.0f, 24 + 32 * index);
                        this._rect = new Rectangle(840, 640, 120, 32);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        if (this.savedata.HaveVirus[index] != null)
                        {
                            AllBase.NAME[] nameArray = this.Nametodata(this.savedata.HaveVirus[index].Name);
                            vector2 = new Vector2(8f, 32 + index * 32);
                            this._position = new Vector2(vector2.X, vector2.Y);
                            DrawBlockCharacters(dg, nameArray, 88, this._position, Color.White, out this._rect, out this._position);
                            this._position = new Vector2(88f, 32 + 32 * index);
                            this._rect = new Rectangle(this.savedata.HaveVirus[index].code * 8, 120, 8, 16);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                        }
                        else
                        {
                            var emptyText = ShanghaiEXE.Translate("Virus.Empty");
                            AllBase.NAME[] nameArray = this.Nametodata(emptyText);
                            vector2 = new Vector2(8f, 32 + index * 32);
                            this._position = new Vector2(vector2.X, vector2.Y);
                            DrawBlockCharacters(dg, nameArray, 16, this._position, Color.White, out this._rect, out this._position);
                        }
                    }
                    this._position = new Vector2(8f, 120f);
                    this._rect = new Rectangle(720, 672, 104, 40);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                if (!this.help || this.right)
                {
                    if (this.top > 0)
                    {
                        this._position = new Vector2(120f, -8f);
                        this._rect = new Rectangle(720, 640, 120, 32);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                    for (int index = 0; index < 6; ++index)
                    {
                        int num = index;
                        if (this.top + num < this.savedata.stockVirus.Count && this.savedata.stockVirus[num + this.top] != null)
                        {
                            this._position = new Vector2(120f, 24 + 32 * num);
                            this._rect = new Rectangle(720, 640, 120, 32);
                            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                            AllBase.NAME[] nameArray = this.Nametodata(this.savedata.stockVirus[num + this.top].Name);
                            vector2 = new Vector2(128f, 32 + num * 32);
                            this._position = new Vector2(vector2.X, vector2.Y);
                            DrawBlockCharacters(dg, nameArray, 88, this._position, Color.White, out this._rect, out this._position);
                            this._position = new Vector2(208f, 32 + 32 * num);
                            this._rect = new Rectangle(this.savedata.stockVirus[num + this.top].code * 8, 120, 8, 16);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                        }
                        else
                            break;
                    }
                }
                if (this.help)
                {
                    int num1 = 56;
                    Virus virus;
                    int num2;
                    if (this.right)
                    {
                        virus = this.savedata.stockVirus[this.Select];
                        num2 = 4;
                    }
                    else
                    {
                        virus = this.savedata.HaveVirus[this.cursol];
                        num2 = 124;
                    }
                    if (virus != null)
                    {
                        this._position = new Vector2(num2 + 56, 48f);
                        virus.Render(dg, this._position, true);
                        this._rect = new Rectangle(568, 128, 112, 88);
                        this._position = new Vector2(num2, 16 + num1);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        ChipFolder.CODE code = (ChipFolder.CODE)virus.code;
                        string txt1 = virus.Name + "　" + code.ToString();
                        this._position = new Vector2(num2 + 8, 20 + num1);
                        this.TextRender(dg, txt1, false, this._position, false);
                        this._rect = new Rectangle(232, 0, 24, 16);
                        this._position = new Vector2(num2 + 16, 44 + num1);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        int num3 = 100 + virus.eatBug * 5;
                        string txt2 = (num3 / 100).ToString() + "." + (num3 % 100 < 10 ? "0" : "") + (num3 % 100).ToString();
                        this._position = new Vector2(num2 + 48, 44 + num1);
                        this.TextRender(dg, txt2, false, this._position, true, Color.Yellow);
                        this._position = new Vector2(num2 + 16, 60 + num1);
                        this.TextRender(dg, ShanghaiEXE.Translate("VirusManager.HP"), false, this._position, true);
                        this._position = new Vector2(num2 + 72, 60 + num1);
                        int num4 = virus.HP;
                        string txt3 = num4.ToString();
                        this.TextRender(dg, txt3, true, this._position, true, Color.Yellow);
                        this._position = new Vector2(num2 + 16, 76 + num1);
                        this.TextRender(dg, ShanghaiEXE.Translate("VirusManager.Power"), false, this._position, true);
                        this._position = new Vector2(num2 + 72, 76 + num1);
                        num4 = virus.Power;
                        string txt4 = num4.ToString();
                        this.TextRender(dg, txt4, true, this._position, true, Color.Yellow);
                        this._rect = new Rectangle(320, 0, 16, 48);
                        this._position = new Vector2(num2 + 88, 44 + num1);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                    else
                    {
                        this._position = new Vector2(num2, 52 + num1);
                        string txt = "EMPTY";
                        this.TextRender(dg, txt, false, this._position, true);
                    }
                }
                this._rect = new Rectangle(240 + 16 * this.cursolanime, 48, 16, 16);
                this._position = new Vector2(!this.right ? -8f : 112f, 32 + 32 * this.cursol);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                if (this.selected && this.cursolanime2)
                {
                    if (!this.selectedForB)
                    {
                        if (!this.help || !this.right)
                        {
                            int cursolanime = this.cursolanime;
                            this._rect = new Rectangle(240 + 0, 48, 16, 16);
                            this._position = new Vector2(-8f, 32 + this.selectedCursol * 32);
                            dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                        }
                    }
                    else if ((!this.help || this.right) && (this.selectedCursol - this.top >= 0 && this.selectedCursol - this.top < 4))
                    {
                        int cursolanime = this.cursolanime;
                        this._rect = new Rectangle(240 + 0, 48, 16, 16);
                        this._position = new Vector2(112f, 32 + (this.selectedCursol - this.top) * 32);
                        dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                    }
                }
                this._position = new Vector2(0.0f, 0.0f);
                this._rect = new Rectangle(720, 624, 240, 16);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);
            if (this.alpha <= 0)
                return;
            Color color = Color.FromArgb(alpha, Color.Black);
            Rectangle _rect = new Rectangle(0, 0, 240, 160);
            Vector2 _point = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", _rect, true, _point, color);
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
        }
    }
}
