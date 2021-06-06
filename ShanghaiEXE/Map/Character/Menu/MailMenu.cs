using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSMap.Character.Menu
{
    internal class MailMenu : MenuBase
    {
        private readonly List<NSGame.Mail> mails = new List<NSGame.Mail>();
        private MailMenu.SCENE nowscene;
        private EventManager eventmanager;
        private int cursol;
        private int top;
        private int cursolanime;
        private bool iconanime;

        private int overTop
        {
            get
            {
                return this.mails.Count - 5;
            }
        }

        private int select
        {
            get
            {
                return this.cursol + this.top;
            }
        }

        public MailMenu(IAudioEngine s, Player p, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
            for (int index = 1; index <= this.savedata.mail.Count; ++index)
            {
                Mail mail = new MailItem(this.savedata.mail[this.savedata.mail.Count - index]);
                mail.read = this.savedata.mailread[this.savedata.mail.Count - index];
                this.mails.Add(mail);
            }
            this.eventmanager = new EventManager(this.sound);
        }

        public override void UpDate()
        {
            switch (this.nowscene)
            {
                case MailMenu.SCENE.fadein:
                    if (this.Alpha > 0)
                    {
                        this.Alpha -= 51;
                        break;
                    }
                    this.nowscene = MailMenu.SCENE.select;
                    break;
                case MailMenu.SCENE.select:
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
                    if (this.cursolanime < 3)
                        break;
                    this.cursolanime = 0;
                    break;
                case MailMenu.SCENE.fadeout:
                    if (this.Alpha < byte.MaxValue)
                    {
                        this.Alpha += 51;
                        break;
                    }
                    this.topmenu.Return();
                    break;
            }
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
            {
                this.sound.PlaySE(SoundEffect.decide);
                this.eventmanager = this.mails[this.select].MakeEvent(this.sound);
                if (!this.mails[this.select].read)
                {
                    this.mails[this.select].read = true;
                    this.savedata.mailread[this.savedata.mailread.Count - this.select - 1] = true;
                }
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                this.nowscene = MailMenu.SCENE.fadeout;
            }
            if (this.waittime <= 0)
            {
                int num1 = 4;
                if (Input.IsPush(Button.Up) && this.select > 0)
                {
                    if (this.cursol > 0)
                        --this.cursol;
                    else
                        --this.top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                }
                if (Input.IsPush(Button.Down))
                {
                    if (this.select >= this.mails.Count - 1)
                        return;
                    if (this.cursol < num1)
                        ++this.cursol;
                    else
                        ++this.top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                }
                else if (Input.IsPush(Button._R))
                {
                    int num2 = this.savedata.mail.Count - (num1 + 1) - this.top;
                    if (num2 > num1)
                        num2 = num1;
                    if (num2 > 0)
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.top += num2;
                    }
                    this.waittime = Input.IsPress(Button._R) ? 10 : 4;
                }
                else
                {
                    if (!Input.IsPush(Button._L))
                        return;
                    int num2 = this.top;
                    if (num2 > num1)
                        num2 = num1;
                    if (num2 > 0)
                    {
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.top -= num2;
                    }
                    this.waittime = Input.IsPress(Button._L) ? 10 : 4;
                }
            }
            else
                --this.waittime;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(480, 624, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            for (int index = 0; index < 5 && this.top + index < this.mails.Count; ++index)
            {
                this._rect = new Rectangle(!this.mails[this.top + index].read ? (this.iconanime ? 496 : 480) : 512, 600, 16, 16);
                this._position = new Vector2(32f, 16 + 16 * index);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                this._position.X = 48f;
                dg.DrawMiniText(this.mails[this.top + index].title, this._position, Color.White);
                this._position.X = 144f;
                dg.DrawMiniText(this.mails[this.top + index].parson, this._position, Color.White);
            }
            this._rect = new Rectangle(112 + 16 * this.cursolanime, 160, 16, 16);
            this._position = new Vector2(16f, 16 + this.cursol * 16);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            float num = this.overTop != 0 && this.top != 0 ? 72f / overTop * top : 0.0f;
            this._rect = new Rectangle(176, 168, 8, 8);
            this._position = new Vector2(224f, 16f + num);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);
            else if (!this.savedata.FlagList[0])
            {
                this._position = new Vector2(5f, 108f);
                this._rect = new Rectangle(0, 0, 40, 48);
                dg.DrawImage(dg, "Face1", this._rect, true, this._position, Color.White);
                this._position = new Vector2(48f, 108f);
                var dialogue = ShanghaiEXE.Translate("Mail.MailReadTextDialogue1");
                dg.DrawText(dialogue[0], this._position);
                this._position = new Vector2(48f, 124f);
                dg.DrawText(dialogue[1], this._position);
            }
            if (this.nowscene != MailMenu.SCENE.fadein && this.nowscene != MailMenu.SCENE.fadeout)
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
