using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using Common.Vectors;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace NSEvent
{
    internal class InteriorSetting : EventBase
    {
        private static readonly ICollection<int> RareInteriors = new[] { 51, 52 };

        private InteriorSetting.NOWSCENE nowschene;
        private InteriorSetting.MENU menu;
        private bool sort;
        private bool yesnoSelect;
        private int overTop;
        private int top1;
        private int top2;
        private Point oldpoji;
        private bool oldset;
        private bool oldspin;
        private int cursol1;
        private int cursol2;
        private int cursolanime;
        private const byte waitlong = 10;
        private const byte waitshort = 4;
        protected const string texture = "menuwindows";
        private int waittime;
        private readonly EventManager eventmanager;
        private readonly SceneMap parent;

        private int Select1
        {
            get
            {
                return this.cursol1 + this.top1;
            }
        }

        private int Select2
        {
            get
            {
                return this.cursol2 + this.top2;
            }
        }

        private int Cursol
        {
            get
            {
                return !this.sort ? this.cursol1 : this.cursol2;
            }
            set
            {
                if (!this.sort)
                    this.cursol1 = value;
                else
                    this.cursol2 = value;
            }
        }

        private int Select
        {
            get
            {
                return !this.sort ? this.Select1 : this.Select2;
            }
        }

        private int Top
        {
            get
            {
                return !this.sort ? this.top1 : this.top2;
            }
            set
            {
                if (!this.sort)
                    this.top1 = value;
                else
                    this.top2 = value;
            }
        }

        public InteriorSetting(IAudioEngine s, EventManager m, SceneMap parent, SaveData save)
          : base(s, m, save)
        {
            this.parent = parent;
            this.NoTimeNext = false;
            this.eventmanager = new EventManager(this.sound);
        }

        public override void Update()
        {
            if (this.eventmanager.playevent)
                this.eventmanager.UpDate();
            else if (this.yesnoSelect)
            {
                this.yesnoSelect = false;
                if (this.savedata.selectQuestion == 0)
                {
                    this.sound.PlaySE(SoundEffect.clincher);
                    this.savedata.interiors.RemoveAt(this.Select);
                    if (this.top1 > 0)
                        --this.top1;
                    else if (this.cursol1 > 0)
                        --this.cursol1;
                    this.parent.Field.InteriorSet();
                    this.overTop = this.savedata.interiors.Count - 5;
                    if (this.savedata.interiors.Count <= 0)
                    {
                        this.parent.setCameraOn = false;
                        this.parent.Player.NoPrint = false;
                        this.parent.cameraPlus.X = 0.0f;
                        this.nowschene = InteriorSetting.NOWSCENE.menu;
                    }
                }
            }
            else
                this.Control();
            if (this.waittime > 0)
                --this.waittime;
            this.FlameControl(10);
            if (!this.moveflame)
                return;
            ++this.cursolanime;
            if (this.cursolanime >= 3)
                this.cursolanime = 0;
        }

        private void Control()
        {
            switch (this.nowschene)
            {
                case InteriorSetting.NOWSCENE.menu:
                    if (Input.IsPress(Button._A))
                    {
                        if (this.savedata.interiors.Count > 0)
                        {
                            this.sound.PlaySE(SoundEffect.decide);
                            this.overTop = this.savedata.interiors.Count - 5;
                            this.nowschene = InteriorSetting.NOWSCENE.select;
                            break;
                        }
                        this.sound.PlaySE(SoundEffect.error);
                        break;
                    }
                    if (Input.IsPress(Button._B))
                    {
                        this.sound.PlaySE(SoundEffect.cancel);
                        this.EndCommand();
                        break;
                    }
                    if (this.waittime > 0)
                        break;
                    if (Input.IsPush(Button.Up))
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        if (this.menu > InteriorSetting.MENU.put)
                            --this.menu;
                        else
                            this.menu = InteriorSetting.MENU.trash;
                        this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                    }
                    else if (Input.IsPush(Button.Down))
                    {
                        if (this.menu < InteriorSetting.MENU.trash)
                            ++this.menu;
                        else
                            this.menu = InteriorSetting.MENU.put;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                    }
                    break;
                case InteriorSetting.NOWSCENE.select:
                    if (Input.IsPress(Button._A))
                    {
                        if (this.savedata.interiors.Count > 0)
                        {
                            this.sound.PlaySE(SoundEffect.decide);
                            switch (this.menu)
                            {
                                case InteriorSetting.MENU.put:
                                    this.oldset = this.savedata.interiors[this.Select].set;
                                    this.savedata.interiors[this.Select].set = true;
                                    this.oldpoji = new Point(this.savedata.interiors[this.Select].posiX, this.savedata.interiors[this.Select].posiY);
                                    this.oldspin = this.savedata.interiors[this.Select].rebirth;
                                    this.parent.Field.InteriorSet();
                                    this.nowschene = InteriorSetting.NOWSCENE.move;
                                    return;
                                case InteriorSetting.MENU.remove:
                                    this.savedata.interiors[this.Select].set = false;
                                    this.parent.Field.InteriorSet();
                                    return;
                                case InteriorSetting.MENU.sort:
                                    if (this.sort)
                                    {
                                        Interior interior = this.savedata.interiors[this.Select1];
                                        if (this.Select1 < this.Select2)
                                        {
                                            this.savedata.interiors.Insert(this.Select2, interior);
                                            this.savedata.interiors.RemoveAt(this.Select1);
                                        }
                                        else
                                        {
                                            this.savedata.interiors.RemoveAt(this.Select1);
                                            this.savedata.interiors.Insert(this.Select2, interior);
                                        }
                                        this.sound.PlaySE(SoundEffect.docking);
                                        this.top1 = this.top2;
                                        this.cursol1 = this.cursol2;
                                        this.sort = false;
                                        return;
                                    }
                                    this.top2 = this.top1;
                                    this.cursol2 = this.cursol1;
                                    this.sort = true;
                                    return;
                                case InteriorSetting.MENU.trash:
                                    this.EventMake();
                                    return;
                                default:
                                    return;
                            }
                        }
                        else
                        {
                            this.sound.PlaySE(SoundEffect.error);
                            break;
                        }
                    }
                    else
                    {
                        if (Input.IsPress(Button._B))
                        {
                            this.sound.PlaySE(SoundEffect.cancel);
                            if (!this.sort)
                            {
                                this.parent.setCameraOn = false;
                                this.parent.Player.NoPrint = false;
                                this.parent.cameraPlus.X = 0.0f;
                                this.nowschene = InteriorSetting.NOWSCENE.menu;
                                break;
                            }
                            this.sort = false;
                            break;
                        }
                        if (this.waittime > 0)
                            break;
                        if (Input.IsPush(Button.Up))
                        {
                            if (this.Select > 0)
                            {
                                if (this.Cursol > 0)
                                    --this.Cursol;
                                else
                                    --this.Top;
                                this.sound.PlaySE(SoundEffect.movecursol);
                                this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                            }
                        }
                        else if (Input.IsPush(Button.Down))
                        {
                            if (this.Select < this.savedata.interiors.Count - 1)
                            {
                                if (this.Cursol < 4)
                                    ++this.Cursol;
                                else
                                    ++this.Top;
                                this.sound.PlaySE(SoundEffect.movecursol);
                                this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                            }
                        }
                        else if (Input.IsPush(Button._R))
                        {
                            int num = this.overTop - this.Top;
                            if (num > 5)
                                num = 5;
                            if (num > 0)
                            {
                                this.sound.PlaySE(SoundEffect.movecursol);
                                this.Top += num;
                            }
                            this.waittime = Input.IsPress(Button._R) ? 10 : 4;
                        }
                        else if (Input.IsPush(Button._L))
                        {
                            int num = this.Top;
                            if (num > 5)
                                num = 5;
                            if (num > 0)
                            {
                                this.sound.PlaySE(SoundEffect.movecursol);
                                this.Top -= num;
                            }
                            this.waittime = Input.IsPress(Button._L) ? 10 : 4;
                        }
                        break;
                    }
                case InteriorSetting.NOWSCENE.move:
                    if (Input.IsPress(Button._A))
                    {
                        this.sound.PlaySE(SoundEffect.decide);
                        this.nowschene = InteriorSetting.NOWSCENE.select;
                        break;
                    }
                    if (Input.IsPress(Button._B))
                    {
                        this.sound.PlaySE(SoundEffect.cancel);
                        this.savedata.interiors[this.Select].posiX = this.oldpoji.X;
                        this.savedata.interiors[this.Select].posiY = this.oldpoji.Y;
                        this.savedata.interiors[this.Select].rebirth = this.oldspin;
                        this.savedata.interiors[this.Select].set = this.oldset;
                        this.parent.Field.InteriorSet();
                        this.nowschene = InteriorSetting.NOWSCENE.select;
                        break;
                    }
                    if (Input.IsPress(Button._L))
                    {
                        this.sound.PlaySE(SoundEffect.lance);
                        this.savedata.interiors[this.Select].rebirth = !this.savedata.interiors[this.Select].rebirth;
                        this.parent.Field.InteriorSet();
                        break;
                    }
                    if (this.waittime != 0)
                        break;
                    if (Input.IsPush(Button.Up) && this.savedata.interiors[this.Select].posiY > -32)
                    {
                        if (Input.IsPush(Button._R))
                        {
                            if (this.savedata.interiors[this.Select].posiY % 8 == 0)
                                this.savedata.interiors[this.Select].posiY -= 8;
                            else
                                this.savedata.interiors[this.Select].posiY = this.savedata.interiors[this.Select].posiY / 8 * 8;
                        }
                        else
                            --this.savedata.interiors[this.Select].posiY;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                        this.parent.Field.InteriorSet();
                        this.CameraSet(true);
                    }
                    if (Input.IsPush(Button.Down) && this.savedata.interiors[this.Select].posiY < 232)
                    {
                        if (Input.IsPush(Button._R))
                        {
                            if (this.savedata.interiors[this.Select].posiY % 8 == 0)
                                this.savedata.interiors[this.Select].posiY += 8;
                            else
                                this.savedata.interiors[this.Select].posiY = this.savedata.interiors[this.Select].posiY / 8 * 8 + 8;
                        }
                        else
                            ++this.savedata.interiors[this.Select].posiY;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                        this.parent.Field.InteriorSet();
                        this.CameraSet(true);
                    }
                    if (Input.IsPush(Button.Left) && this.savedata.interiors[this.Select].posiX > -32)
                    {
                        if (Input.IsPush(Button._R))
                        {
                            if (this.savedata.interiors[this.Select].posiX % 8 == 0)
                                this.savedata.interiors[this.Select].posiX -= 8;
                            else
                                this.savedata.interiors[this.Select].posiX = this.savedata.interiors[this.Select].posiX / 8 * 8;
                        }
                        else
                            --this.savedata.interiors[this.Select].posiX;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Left) ? 10 : 4;
                        this.parent.Field.InteriorSet();
                        this.CameraSet(true);
                    }
                    if (Input.IsPush(Button.Right) && this.savedata.interiors[this.Select].posiX < 232)
                    {
                        if (Input.IsPush(Button._R))
                        {
                            if (this.savedata.interiors[this.Select].posiX % 8 == 0)
                                this.savedata.interiors[this.Select].posiX += 8;
                            else
                                this.savedata.interiors[this.Select].posiX = this.savedata.interiors[this.Select].posiX / 8 * 8 + 8;
                        }
                        else
                            ++this.savedata.interiors[this.Select].posiX;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Right) ? 10 : 4;
                        this.parent.Field.InteriorSet();
                        this.CameraSet(true);
                    }
                    break;
            }
        }

        private void CameraSet(bool set)
        {
            if (set)
            {
                this.parent.setCameraOn = true;
                this.parent.Player.NoPrint = true;
                this.parent.setCamera = new Vector3(savedata.interiors[Select].posiX, savedata.interiors[Select].posiY, 0.0f);
                if (this.nowschene == InteriorSetting.NOWSCENE.select)
                    this.parent.cameraPlus.X = -40f;
                else
                    this.parent.cameraPlus.X = 0.0f;
            }
            else
            {
                this.parent.Player.NoPrint = false;
                this.parent.setCameraOn = false;
            }
        }

        private void EventMake()
        {
            this.eventmanager.events.Clear();
            if (RareInteriors.Contains(this.savedata.interiors[this.Select].number))
            {
                var dialogue = ShanghaiEXE.Translate("ChipTrader.RareChipStopDialogue1");
                this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, dialogue.Face.Mono, dialogue.Face.Auto, this.savedata));
                this.yesnoSelect = false;
            }
            else
            {
                var question = ShanghaiEXE.Translate("Interior.DiscardQuestion");
                var options = ShanghaiEXE.Translate("Interior.DiscardOptions");
                this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, question[0], question[1], options[0], options[1], true, true, question.Face, this.savedata));
                this.yesnoSelect = true;
            }
        }

        public override void Render(IRenderer dg)
        {
            Vector2 vector2 = new Vector2();
            switch (this.nowschene)
            {
                case InteriorSetting.NOWSCENE.menu:
                    this._rect = new Rectangle(816, 0, 64, 96);
                    this._position = new Vector2(8f, 0.0f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    string text1 = "";
                    for (int index = 0; index < 4; ++index)
                    {
                        Color color = Color.White;
                        if (this.savedata.interiors.Count <= 0)
                            color = Color.FromArgb(160, 160, 160);
                        this._position = new Vector2(16f, 16 + 16 * index);
                        switch (index)
                        {
                            case 0:
                                text1 = ShanghaiEXE.Translate("Interior.Place");
                                break;
                            case 1:
                                text1 = ShanghaiEXE.Translate("Interior.Store");
                                break;
                            case 2:
                                text1 = ShanghaiEXE.Translate("Interior.Sort");
                                break;
                            case 3:
                                text1 = ShanghaiEXE.Translate("Interior.Discard");
                                break;
                        }
                        Vector2 position = this._position;
                        ++position.X;
                        ++position.Y;
                        dg.DrawMiniText(text1, position, Color.FromArgb(32, 32, 32));
                        --position.X;
                        --position.Y;
                        dg.DrawMiniText(text1, position, color);
                    }
                    this._rect = new Rectangle(240 + 16 * this.cursolanime, 48, 16, 16);
                    this._position = new Vector2(0.0f, 12 + (int)this.menu * 16);
                    dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                    break;
                case InteriorSetting.NOWSCENE.select:
                    this.CameraSet(this.savedata.interiors[this.Select].set);
                    this._rect = new Rectangle(656, 0, 88, 104);
                    this._position = new Vector2(0.0f, 0.0f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    this._rect = new Rectangle(808, 0, 8, 104);
                    this._position = new Vector2(88f, 0.0f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    for (int index = 0; index < Math.Min(5, this.savedata.interiors.Count); ++index)
                    {
                        vector2 = new Vector2(16f, 14 + index * 16);
                        ++vector2.X;
                        ++vector2.Y;
                        IRenderer IRenderer1 = dg;
                        string text2 = Shop.INTERIOR.GetItem(this.savedata.interiors[this.Top + index].number);
                        Vector2 point1 = vector2;
                        Color color1 = Color.FromArgb(32, 32, 32);
                        IRenderer1.DrawMicroText(text2, point1, color1);
                        --vector2.X;
                        --vector2.Y;
                        Color color2 = Color.White;
                        if (this.Select1 == this.Top + index && this.sort)
                            color2 = Color.FromArgb(128, byte.MaxValue, byte.MaxValue);
                        else if (!this.savedata.interiors[this.Top + index].set)
                            color2 = Color.FromArgb(128, 128, 128);
                        IRenderer IRenderer2 = dg;
                        string text3 = Shop.INTERIOR.GetItem(this.savedata.interiors[this.Top + index].number);
                        Vector2 point2 = vector2;
                        Color white = Color.White;
                        IRenderer2.DrawMicroText(text3, point2, white);
                    }
                    if (!this.sort)
                    {
                        this._rect = new Rectangle(240 + 16 * this.cursolanime, 48, 16, 16);
                        this._position = new Vector2(0.0f, 12 + (this.sort ? this.cursol2 : this.cursol1) * 16);
                        dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                    }
                    else
                    {
                        this._rect = new Rectangle(936, 0, 24, 16);
                        this._position = new Vector2(0.0f, 4 + (this.sort ? this.cursol2 : this.cursol1) * 16);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                    float num = this.overTop != 0 && this.Top != 0 ? 80f / overTop * Top : 0.0f;
                    this._rect = new Rectangle(176, 168, 8, 8);
                    this._position = new Vector2(88f, 8f + num);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    if (this.Top > 0)
                    {
                        this._rect = new Rectangle(160 + 8 * this.cursolanime, 208, 8, 8);
                        this._position = new Vector2(40f, 4f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    }
                    if (this.Top < this.overTop)
                    {
                        this._rect = new Rectangle(184 + 8 * this.cursolanime, 208, 8, 8);
                        this._position = new Vector2(40f, 92f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        break;
                    }
                    break;
                case InteriorSetting.NOWSCENE.move:
                    this._rect = new Rectangle(880, 0, 56, 48);
                    this._position = new Vector2(184f, 0.0f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                    this.CameraSet(this.savedata.interiors[this.Select].set);
                    break;
            }
            if (!this.eventmanager.playevent)
                return;
            this.eventmanager.Render(dg);
        }

        private enum NOWSCENE
        {
            menu,
            select,
            move,
        }

        private enum MENU
        {
            put,
            remove,
            sort,
            trash,
        }
    }
}
