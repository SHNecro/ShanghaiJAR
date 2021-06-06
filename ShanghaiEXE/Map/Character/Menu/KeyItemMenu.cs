using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSMap.Character.Menu
{
    internal class KeyItemMenu : MenuBase
    {
        private readonly List<NSGame.KeyItem> keyitems = new List<NSGame.KeyItem>();
        private KeyItemMenu.SCENE nowscene;
        private int cursol;
        private int top;
        private int cursolanime;
        private bool infonext;
        private int infopage;

        private int overTop
        {
            get
            {
                return this.keyitems.Count - 9;
            }
        }

        private int select
        {
            get
            {
                return this.cursol + this.top * 2;
            }
        }

        public KeyItemMenu(IAudioEngine s, Player p, TopMenu t, SaveData save)
          : base(s, p, t, save)
        {
            for (int index = 0; index < this.savedata.keyitem.Count; ++index)
                this.keyitems.Add(new KeyItem(this.savedata.keyitem[index]));
            if (this.keyitems.Count <= 0)
                return;
            this.ChangeItem();
        }

        public override void UpDate()
        {
            switch (this.nowscene)
            {
                case KeyItemMenu.SCENE.fadein:
                    if (this.Alpha > 0)
                    {
                        this.Alpha -= 51;
                        break;
                    }
                    this.nowscene = KeyItemMenu.SCENE.select;
                    break;
                case KeyItemMenu.SCENE.select:
                    this.Control();
                    this.FlamePlus();
                    if (this.frame % 10 == 0)
                        ++this.cursolanime;
                    if (this.cursolanime < 3)
                        break;
                    this.cursolanime = 0;
                    break;
                case KeyItemMenu.SCENE.fadeout:
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
            if (this.infonext && Input.IsPress(Button._A))
            {
                ++this.infopage;
                this.infonext = this.keyitems[this.select].info.Count > 3 + 3 * this.infopage;
                this.sound.PlaySE(SoundEffect.movecursol);
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                this.nowscene = KeyItemMenu.SCENE.fadeout;
            }
            if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up) && this.select > 1)
                {
                    if (this.cursol > 1)
                        this.cursol -= 2;
                    else
                        --this.top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Up) ? 10 : 4;
                    this.ChangeItem();
                }
                if (Input.IsPush(Button.Down) && this.select < this.keyitems.Count - 2)
                {
                    if (this.cursol < 8)
                        this.cursol += 2;
                    else
                        ++this.top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                    this.ChangeItem();
                }
                if (Input.IsPush(Button.Left) && this.select > 0)
                {
                    if (this.cursol > 0)
                    {
                        --this.cursol;
                    }
                    else
                    {
                        --this.top;
                        ++this.cursol;
                    }
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Left) ? 10 : 4;
                    this.ChangeItem();
                }
                if (!Input.IsPush(Button.Right) || this.select >= this.keyitems.Count - 1)
                    return;
                if (this.cursol < 9)
                {
                    ++this.cursol;
                }
                else
                {
                    ++this.top;
                    --this.cursol;
                }
                this.sound.PlaySE(SoundEffect.movecursol);
                this.waittime = Input.IsPress(Button.Right) ? 10 : 4;
                this.ChangeItem();
            }
            else
                --this.waittime;
        }

        private void ChangeItem()
        {
            this.infopage = 0;
            this.infonext = this.keyitems[this.select].info.Count > 3;
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(480, 624, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(544, 584, 64, 16);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            if (!this.savedata.FlagList[0])
            {
                this._position = new Vector2(5f, 108f);
                this._rect = new Rectangle(0, 0, 40, 48);
                dg.DrawImage(dg, "Face1", this._rect, true, this._position, Color.White);
            }
            if (this.keyitems.Count > 0)
            {
                for (int index = 0; index < 3 && this.keyitems[this.select].info.Count > this.infopage * 3 + index; ++index)
                {
                    string text = this.keyitems[this.select].info[this.infopage * 3 + index];
                    this._position = new Vector2(48f, 108 + 16 * index);
                    dg.DrawText(text, this._position, this.savedata);
                }
            }
            for (int index = 0; index < 10 && this.top * 2 + index < this.keyitems.Count; ++index)
            {
                this._position = new Vector2(32 + 96 * (index % 2), 16 + 16 * (index / 2));
                dg.DrawMiniText(this.keyitems[this.top * 2 + index].name, this._position, Color.White);
            }
            if (this.infonext)
            {
                this._position = new Vector2(224f, 140f);
                this._rect = new Rectangle(240 + this.cursolanime % 3 * 16, 0, 16, 16);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
            }
            this._rect = new Rectangle(112 + 16 * this.cursolanime, 160, 16, 16);
            this._position = new Vector2(16 + 96 * (this.cursol % 2), 16 + 16 * (this.cursol / 2));
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            float num = this.overTop != 0 && this.top != 0 ? 82f / overTop * (this.top * 2) : 0.0f;
            this._rect = new Rectangle(176, 168, 8, 8);
            this._position = new Vector2(224f, 16f + num);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            if (this.nowscene != KeyItemMenu.SCENE.fadein && this.nowscene != KeyItemMenu.SCENE.fadeout)
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
