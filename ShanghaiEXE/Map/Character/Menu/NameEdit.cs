using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSGame;
using SlimDX;
using System.Collections.Generic;
using System.Drawing;

namespace NSMap.Character.Menu
{
    internal class NameEdit : MenuBase
    {
        private readonly int[] name = new int[10];
        private readonly int[,] charlistJP = new int[6, 17]
        {
      {
        1,
        2,
        3,
        4,
        5,
        0,
        31,
        32,
        33,
        34,
        35,
        0,
        47,
        48,
        49,
        50,
        51
      },
      {
        6,
        7,
        8,
        9,
        10,
        0,
        36,
        0,
        37,
        0,
        38,
        0,
        52,
        53,
        54,
        55,
        56
      },
      {
        11,
        12,
        13,
        14,
        15,
        0,
        39,
        40,
        41,
        42,
        43,
        0,
        57,
        58,
        59,
        60,
        61
      },
      {
        16,
        17,
        18,
        19,
        20,
        0,
        44,
        0,
        45,
        0,
        46,
        0,
        62,
        63,
        64,
        65,
        66
      },
      {
        21,
        22,
        23,
        24,
        25,
        0,
        73,
        74,
        75,
        76,
        77,
        0,
        67,
        68,
        69,
        70,
        71
      },
      {
        26,
        27,
        28,
        29,
        30,
        0,
        79,
        80,
        81,
        78,
        72,
        0,
        85,
        165,
        101,
        91,
        90
      }
        };
        private readonly int[,] charlistEN = new int[6, 17]
        {
      {
        112,
        113,
        114,
        115,
        116,
        0,
        139,
        140,
        141,
        142,
        143,
        0,
        103,
        104,
        105,
        106,
        107
      },
      {
        117,
        118,
        119,
        120,
        121,
        0,
        144,
        145,
        146,
        147,
        148,
        0,
        108,
        109,
        110,
        111,
        102
      },
      {
        122,
        123,
        124,
        125,
        126,
        0,
        149,
        150,
        151,
        152,
        153,
        0,
        84,
        85,
        86,
        87,
        88
      },
      {
         sbyte.MaxValue,
        128,
        129,
        130,
        131,
        0,
        154,
        155,
        156,
        157,
        158,
        0,
        101,
        90,
        91,
        92,
        93
      },
      {
        132,
        133,
        134,
        135,
        136,
        0,
        159,
        160,
        161,
        162,
        163,
        0,
        94,
        95,
        96,
        97,
        98
      },
      {
        137,
        0,
        138,
        82,
        83,
        0,
        164,
        89,
        165,
        99,
        100,
        0,
        166,
        167,
        168,
        169,
        0
      }
        };
        private readonly List<NSGame.Mail> mails = new List<NSGame.Mail>();
        private NameEdit.SCENE nowscene;
        private readonly EventManager eventmanager;
        private int edit;
        private bool eng;
        private Point cursol;
        private readonly int top;
        private int cursolanime;
        private bool iconanime;
        private readonly MenuBase backmenu;

        public NameEdit(IAudioEngine s, Player p, TopMenu t, SaveData save, MenuBase backmenu)
          : base(s, p, t, save)
        {
            this.backmenu = backmenu;
            this.name = this.savedata.netWorkName;
            this.eventmanager = new EventManager(this.sound);
            for (int index = this.name.Length - 1; index >= 0; --index)
            {
                if ((uint)this.name[index] > 0U)
                {
                    this.edit = index + 1;
                    if (this.edit >= this.name.Length)
                    {
                        this.edit = this.name.Length - 1;
                        break;
                    }
                    break;
                }
            }
            this.eng = ShanghaiEXE.language == 1;
            bool flag = false;
            for (int index = 0; index < this.name.Length; ++index)
            {
                if ((uint)this.name[index] > 0U)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
                return;
            this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
            var dialogue = ShanghaiEXE.Translate("NameEdit.NameFailureDialogue1");
            this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
            this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
        }

        public override void UpDate()
        {
            switch (this.nowscene)
            {
                case NameEdit.SCENE.fadein:
                    if (this.Alpha > 0)
                    {
                        this.Alpha -= 51;
                        break;
                    }
                    this.nowscene = NameEdit.SCENE.select;
                    break;
                case NameEdit.SCENE.select:
                    if (this.eventmanager.playevent)
                        this.eventmanager.UpDate();
                    else
                        this.Control();
                    this.FlamePlus();
                    if (this.frame % 10 == 0)
                    {
                        ++this.cursolanime;
                        this.iconanime = !this.iconanime;
                    }
                    if (this.cursolanime < 2)
                        break;
                    this.cursolanime = 0;
                    break;
                case NameEdit.SCENE.fadeout:
                    if (this.Alpha < byte.MaxValue)
                    {
                        this.Alpha += 51;
                        break;
                    }
                    if (this.backmenu is NetWork)
                    {
                        this.savedata.netWorkName = this.name;
                        ((NetWork)this.backmenu).nowscene = NetWork.SCENE.fadein;
                    }
                    break;
            }
        }

        private void OK()
        {
            bool flag = false;
            for (int index = 0; index < this.name.Length; ++index)
            {
                if ((uint)this.name[index] > 0U)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                this.sound.PlaySE(SoundEffect.docking);
                this.nowscene = NameEdit.SCENE.fadeout;
            }
            else
            {
                this.sound.PlaySE(SoundEffect.error);
                this.eventmanager.events.Clear();
                this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                var dialogue = ShanghaiEXE.Translate("NameEdit.NameInvalidDialogue1");
                this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
            }
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
            {
                this.sound.PlaySE(SoundEffect.decide);
                if (this.cursol.X < this.charlistJP.GetLength(1))
                {
                    this.name[this.edit] = this.eng ? this.charlistEN[this.cursol.Y, this.cursol.X] : this.charlistJP[this.cursol.Y, this.cursol.X];
                    if (this.edit < this.name.Length - 1)
                    {
                        ++this.edit;
                    }
                    else
                    {
                        this.cursol.X = this.charlistJP.GetLength(1);
                        this.cursol.Y = 2;
                    }
                }
                else
                {
                    switch (this.cursol.Y)
                    {
                        case 0:
                            this.eng = ShanghaiEXE.language == 1;
                            break;
                        case 1:
                            this.eng = ShanghaiEXE.language != 1;
                            break;
                        default:
                            this.OK();
                            break;
                    }
                }
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                if (this.edit == this.name.Length - 1 && (uint)this.name[this.name.Length - 1] > 0U)
                    this.name[this.name.Length - 1] = 0;
                else if (this.edit > 0)
                {
                    this.name[this.edit - 1] = 0;
                    --this.edit;
                }
                else
                    this.name[this.edit] = 0;
            }
            if (Input.IsPress(Button._Start))
            {
                if (this.cursol.X == this.charlistJP.GetLength(1) && this.cursol.Y >= 2)
                {
                    this.OK();
                }
                else
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.cursol.X = this.charlistJP.GetLength(1);
                    this.cursol.Y = 2;
                }
            }
            if (Input.IsPress(Button._Select))
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.eng = !this.eng;
            }
            if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up))
                {
                    if (this.cursol.Y > 2 && this.cursol.X == this.charlistJP.GetLength(1))
                        this.cursol.Y = 2;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    --this.cursol.Y;
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                }
                if (Input.IsPush(Button.Down))
                {
                    if (this.cursol.Y > 1 && this.cursol.X == this.charlistJP.GetLength(1))
                        this.cursol.Y = -1;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    ++this.cursol.Y;
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                }
                if (Input.IsPush(Button.Left))
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    --this.cursol.X;
                    this.waittime = Input.IsPress(Button.Left) ? 10 : 4;
                }
                if (Input.IsPush(Button.Right))
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    ++this.cursol.X;
                    this.waittime = Input.IsPress(Button.Right) ? 10 : 4;
                }
                if (this.cursol.X < 0)
                    this.cursol.X = this.charlistJP.GetLength(1);
                if (this.cursol.X > this.charlistJP.GetLength(1))
                    this.cursol.X = 0;
                if (this.cursol.Y < 0)
                    this.cursol.Y = this.charlistJP.GetLength(0) - 1;
                if (this.cursol.Y >= this.charlistJP.GetLength(0))
                    this.cursol.Y = 0;
                if (Input.IsPush(Button._R))
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    if (this.edit < this.name.Length - 1)
                        ++this.edit;
                    this.waittime = Input.IsPress(Button._R) ? 10 : 4;
                }
                else
                {
                    if (!Input.IsPush(Button._L))
                        return;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    if (this.edit > 0)
                        --this.edit;
                    this.waittime = Input.IsPress(Button._L) ? 10 : 4;
                }
            }
            else
                --this.waittime;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(0, 1264, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(528, 1104, 104, 40);
            this._position = new Vector2(72f, 8f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            for (int index = 0; index < this.name.Length; ++index)
            {
                this._position = new Vector2(80 + 8 * index, 24f);
                if (index == this.edit && this.cursolanime == 0 && (this.cursol.X != this.charlistJP.GetLength(1) || this.cursol.Y < 2))
                {
                    this._rect = new Rectangle(264, 1264, 8, 16);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                else if (this.name[index] == 0 && (this.cursol.X != this.charlistJP.GetLength(1) || this.cursol.Y < 2))
                {
                    this._rect = new Rectangle(272, 1264, 8, 16);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                else
                {
                    this._rect = new Rectangle(8 * this.name[index], 16, 8, 16);
                    dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                }
            }
            if (!this.eng)
            {
                for (int index1 = 0; index1 < this.charlistJP.GetLength(1); ++index1)
                {
                    for (int index2 = 0; index2 < this.charlistJP.GetLength(0); ++index2)
                    {
                        this._rect = new Rectangle(8 * this.charlistJP[index2, index1], 16, 8, 16);
                        this._position = new Vector2(32 + 8 * index1, 48 + 16 * index2);
                        dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                    }
                }
            }
            else
            {
                for (int index1 = 0; index1 < this.charlistEN.GetLength(1); ++index1)
                {
                    for (int index2 = 0; index2 < this.charlistEN.GetLength(0); ++index2)
                    {
                        this._rect = new Rectangle(8 * this.charlistEN[index2, index1], 16, 8, 16);
                        this._position = new Vector2(32 + 8 * index1, 48 + 16 * index2);
                        dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                    }
                }
            }
            this._position = new Vector2(184f, 48f);
            this._rect = ShanghaiEXE.language == 1
                ? new Rectangle(240, 1280, 24, 16)
                : new Rectangle(240, 1264, 24, 16);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._position = new Vector2(184f, 64f);
            this._rect = ShanghaiEXE.language == 1
                ? new Rectangle(240, 1264, 24, 16)
                : new Rectangle(240, 1280, 24, 16);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._position = new Vector2(184f, 80f);
            this._rect = ShanghaiEXE.language == 1
                ? new Rectangle(240, 1312, 24, 16)
                : new Rectangle(240, 1296, 24, 16);
            this._rect = new Rectangle(240, 1312, 24, 16);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            if (this.cursol.X < this.charlistJP.GetLength(1))
            {
                this._rect = new Rectangle(240, 1328 + 16 * this.cursolanime, 8, 16);
                this._position = new Vector2(32 + 8 * this.cursol.X, 48 + 16 * this.cursol.Y);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            else
            {
                int num = this.cursol.Y;
                if (num > 2)
                    num = 2;
                this._rect = new Rectangle(248, 1328 + 16 * this.cursolanime, 24, 16);
                this._position = new Vector2(184f, 48 + 16 * num);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);
            else if (this.savedata.FlagList[0]) { }
            if (this.nowscene != NameEdit.SCENE.fadein && this.nowscene != NameEdit.SCENE.fadeout)
                return;
            Color color = Color.FromArgb(this.Alpha, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
        }
    }
}
