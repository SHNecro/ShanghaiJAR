using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;
using System.Threading;

namespace NSMap.Character.Menu
{
    internal class Save : MenuBase
    {
        private Save.SCENE nowscene;
        private Save.SELECTSCENE select;
        private readonly string time;
        private readonly string comp;
        private readonly string chips;
        private readonly string money;
        private bool yesno;
        private bool threadEnd;
        private readonly SceneMain main;
        private readonly Thread thread_1;

        public Save(IAudioEngine s, Player p, TopMenu t, SaveData save, SceneMain m)
          : base(s, p, t, save)
        {
            this.main = m;
            this.yesno = true;
            this.Alpha = byte.MaxValue;
            this.time = this.savedata.GetTime();
            this.comp = this.savedata.GetHaveManyChips();
            this.chips = this.savedata.GetHaveChips();
            this.money = this.savedata.Money.ToString();
            this.thread_1 = new Thread(new ThreadStart(() => this.savedata.SaveFile(this.main.parent)));
        }

        public override void UpDate()
        {
            switch (this.nowscene)
            {
                case Save.SCENE.fadein:
                    if (this.Alpha > 0)
                    {
                        this.Alpha -= 51;
                        break;
                    }
                    this.nowscene = Save.SCENE.select;
                    break;
                case Save.SCENE.select:
                    this.Control();
                    this.FlamePlus();
                    if (this.select != Save.SELECTSCENE.saved || this.threadEnd)
                        break;
                    this.threadEnd = true;
                    this.thread_1.Abort();
                    break;
                case Save.SCENE.fadeout:
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
            if (this.select == Save.SELECTSCENE.saving)
            {
                if (!this.savedata.saveEnd)
                    return;
                this.sound.PlaySE(SoundEffect.getchip);
                this.select = Save.SELECTSCENE.saved;
            }
            else
            {
                if (Input.IsPress(Button._A))
                {
                    this.sound.PlaySE(SoundEffect.decide);
                    if (this.select != Save.SELECTSCENE.saved)
                    {
                        if (!this.yesno)
                            this.nowscene = Save.SCENE.fadeout;
                        else if (this.select == Save.SELECTSCENE.select)
                        {
                            this.yesno = true;
                            this.select = Save.SELECTSCENE.question;
                        }
                        else
                        {
                            this.FolderSave();
                            this.savedata.saveEnd = false;
                            this.thread_1.Start();
                            this.select = Save.SELECTSCENE.saving;
                        }
                    }
                    else
                        this.nowscene = Save.SCENE.fadeout;
                }
                if (Input.IsPress(Button._B))
                {
                    this.sound.PlaySE(SoundEffect.cancel);
                    this.nowscene = Save.SCENE.fadeout;
                }
                if (this.waittime <= 0 && this.select != Save.SELECTSCENE.saved)
                {
                    if (Input.IsPush(Button.Left))
                    {
                        this.yesno = !this.yesno;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Left) ? 10 : 4;
                    }
                    if (Input.IsPush(Button.Right))
                    {
                        this.yesno = !this.yesno;
                        this.sound.PlaySE(SoundEffect.movecursol);
                        this.waittime = Input.IsPress(Button.Right) ? 10 : 4;
                    }
                }
                else
                    --this.waittime;
            }
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(240, 624, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(608, 584, 40, 16);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(568, 216, 192, 88);
            this._position = new Vector2(24f, 16f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            int num = 18;
            this._position = new Vector2(32f, 24f);
            string text1 = ShanghaiEXE.Translate("Save.PlayTime");
            dg.DrawText(text1, this._position, true);
            this._position.X = 192f;
            string time = this.time;
            this._position.X -= time.Length * 6;
            dg.DrawText(time, this._position, true);
            this._position.X = 32f;
            this._position.Y += num;
            string text2 = ShanghaiEXE.Translate("Save.Library");
            dg.DrawText(text2, this._position, true);
            this._position.X = 200f;
            string comp = this.comp;
            this._position.X -= comp.Length * 6;
            dg.DrawText(comp, this._position, true);
            this._position.X = 32f;
            this._position.Y += num;
            string text3 = ShanghaiEXE.Translate("Save.Chips");
            dg.DrawText(text3, this._position, true);
            this._position.X = 190f;
            string text4 = string.Format(ShanghaiEXE.Translate("Save.ChipCountFormat"), this.chips);
            this._position.X -= text4.Length * 6;
            dg.DrawText(text4, this._position, true);
            this._position.X = 32f;
            this._position.Y += num;
            string text5 = ShanghaiEXE.Translate("Save.Money");
            dg.DrawText(text5, this._position, true);
            this._position.X = 192f;
            string text6;
            if (this.savedata.Money < this.savedata.moneyover)
            {
                text6 = string.Format("{0}Z", this.money);
                this._position.X -= text6.Length * 6;
            }
            else
            {
                text6 = ShanghaiEXE.Translate("Save.BillionaireMoney");
                this._position.X = 102f;
            }
            dg.DrawText(text6, this._position, true);
            this._position = new Vector2(5f, 108f);
            this._rect = new Rectangle(0, 0, 40, 48);
            if (!this.savedata.FlagList[0])
                dg.DrawImage(dg, "Face1", this._rect, true, this._position, Color.White);
            switch (this.select)
            {
                case Save.SELECTSCENE.select:
                    this._position = new Vector2(48f, 108f);
                    if (!this.savedata.FlagList[0])
                    {
                        dg.DrawText(ShanghaiEXE.Translate("Save.SaveQuestion"), this._position);
                    }
                    else
                    {
                        dg.DrawText(ShanghaiEXE.Translate("Save.SaveQuestionSpecial"), this._position);
                    }
                    this.YesNo(dg);
                    break;
                case Save.SELECTSCENE.question:
                    this._position = new Vector2(48f, 108f);
                    if (!this.savedata.FlagList[0])
                    {
                        dg.DrawText(ShanghaiEXE.Translate("Save.OverwriteQuestion"), this._position);
                    }
                    else
                    {
                        dg.DrawText(ShanghaiEXE.Translate("Save.OverwriteQuestionSpecial"), this._position);
                    }
                    this.YesNo(dg);
                    break;
                case Save.SELECTSCENE.saving:
                    this._position = new Vector2(48f, 108f);
                    if (!this.savedata.FlagList[0])
                    {
                        dg.DrawText(ShanghaiEXE.Translate("Save.SaveInProgress"), this._position);
                    }
                    else
                    {
                        dg.DrawText(ShanghaiEXE.Translate("Save.SaveInProgressSpecial"), this._position);
                    }
                    break;
                case Save.SELECTSCENE.saved:
                    this._position = new Vector2(48f, 108f);
                    if (!this.savedata.FlagList[0])
                    {
                        dg.DrawText(ShanghaiEXE.Translate("Save.SaveComplete"), this._position);
                    }
                    else
                    {
                        dg.DrawText(ShanghaiEXE.Translate("Save.SaveCompleteSpecial"), this._position);
                    }
                    break;
            }
        }

        private void YesNo(IRenderer dg)
        {
            var options = ShanghaiEXE.Translate("Save.Options");
            this._position = new Vector2(64f, 124f);
            dg.DrawText(options[0], this._position);
            this._position = new Vector2(128f, 124f);
            dg.DrawText(options[1], this._position);
            this._position = new Vector2(this.yesno ? 48f : 112f, 124f);
            this._rect = new Rectangle(240 + this.frame / 4 % 3 * 16, 48, 16, 16);
            dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
        }

        private void FolderSave()
        {
            this.main.FolderSave();
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
        }

        private enum SELECTSCENE
        {
            select,
            question,
            saving,
            saved,
        }
    }
}
