using NSAddOn;
using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSMap;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace NSGame
{
    public class DebugMode
    {
        private int plusValue = 1;
        private string[] txt;
        private string[,] codes;
        private int code;
        private readonly SaveData savedata;
        private readonly IAudioEngine sound;
        public bool menuOn;
        private bool topmenu;
        private int topCursor;
        private int subCursor;
        private readonly string[] menus;
        private DebugMode.MENU nowmenu;
        private readonly SceneMap parent;
        protected Rectangle _rect;
        protected Vector2 _position;
        private string[] addons;
        protected string[] names;
        private bool valueSet;

        private void StyleNames()
        {
            this.txt = new string[5];
            for (int index = 0; index < this.savedata.style.Length; ++index)
                this.txt[index] = this.savedata.style[index].name;
            this.subCursor = 0;
        }

        public void StyleContorol()
        {
            if (Input.IsPress(Button.Up))
            {
                --this.subCursor;
                if (this.subCursor < 0)
                    this.subCursor = this.txt.Length - 1;
            }
            if (Input.IsPress(Button.Down))
            {
                ++this.subCursor;
                if (this.subCursor > this.txt.Length - 1)
                    this.subCursor = 0;
            }
            if (this.subCursor > 0)
            {
                if (Input.IsPress(Button._R))
                {
                    ++this.savedata.style[this.subCursor].style;
                    if (this.savedata.style[this.subCursor].style > 6)
                        this.savedata.style[this.subCursor].style = 1;
                    this.txt[this.subCursor] = this.savedata.style[this.subCursor].name;
                }
                if (Input.IsPress(Button._L))
                {
                    --this.savedata.style[this.subCursor].style;
                    if (this.savedata.style[this.subCursor].style < 1)
                        this.savedata.style[this.subCursor].style = 6;
                    this.txt[this.subCursor] = this.savedata.style[this.subCursor].name;
                }
                if (Input.IsPress(Button.Left))
                {
                    ++this.savedata.style[this.subCursor].element;
                    if (this.savedata.style[this.subCursor].element > 6)
                        this.savedata.style[this.subCursor].element = 1;
                    this.txt[this.subCursor] = this.savedata.style[this.subCursor].name;
                }
                if (Input.IsPress(Button.Right))
                {
                    --this.savedata.style[this.subCursor].element;
                    if (this.savedata.style[this.subCursor].element < 1)
                        this.savedata.style[this.subCursor].element = 6;
                    this.txt[this.subCursor] = this.savedata.style[this.subCursor].name;
                }
            }
            if (!Input.IsPress(Button._B))
                return;
            this.topmenu = true;
        }

        public void StyleRender(IRenderer dg)
        {
            Color yellow1 = Color.Yellow;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawMiniText(" スタイル変更", this._position, yellow1);
            Color yellow2 = Color.Yellow;
            this._position = new Vector2(0.0f, 144f);
            dg.DrawMiniText("RL：スタイル　右左：属性", this._position, yellow2);
            Color white1 = Color.White;
            int num = this.subCursor / 10;
            for (int index = 0; index < this.txt.Length; ++index)
            {
                Color color = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(16f, 16 + 12 * index);
                dg.DrawMiniText(this.txt[index], this._position, color);
            }
            Color white2 = Color.White;
            this._position = new Vector2(0.0f, 16 + 12 * (this.subCursor % 10));
            dg.DrawMicroText("にｱ", this._position, white2);
        }

        private void PeaceNames()
        {
            this.txt = new string[3];
            this.txt[0] = "バグ";
            this.txt[1] = "エラー";
            this.txt[2] = "フリーズ";
            this.subCursor = 0;
        }

        public void PeaceContorol()
        {
            if (Input.IsPress(Button.Up))
            {
                --this.subCursor;
                if (this.subCursor < 0)
                    this.subCursor = this.txt.Length - 1;
            }
            if (Input.IsPress(Button.Down))
            {
                ++this.subCursor;
                if (this.subCursor > this.txt.Length - 1)
                    this.subCursor = 0;
            }
            if (Input.IsPress(Button._R))
                this.savedata.havePeace[this.subCursor] += 10;
            if (Input.IsPress(Button._L))
                this.savedata.havePeace[this.subCursor] -= 10;
            if (!Input.IsPress(Button._B))
                return;
            this.topmenu = true;
        }

        public void PeaceRender(IRenderer dg)
        {
            Color yellow1 = Color.Yellow;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawMiniText(" かけら増減", this._position, yellow1);
            Color yellow2 = Color.Yellow;
            this._position = new Vector2(0.0f, 144f);
            dg.DrawMiniText("R：プラス　L：マイナス", this._position, yellow2);
            Color white1 = Color.White;
            int num1 = this.subCursor / 10;
            for (int index = 0; index < this.txt.Length; ++index)
            {
                int num2 = this.savedata.havePeace[index];
                Color color1 = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(80f, 16 + 12 * index);
                dg.DrawMiniText(num2.ToString(), this._position, color1);
                Color color2 = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(16f, 16 + 12 * index);
                dg.DrawMiniText(this.txt[index], this._position, color2);
            }
            Color white2 = Color.White;
            this._position = new Vector2(0.0f, 16 + 12 * (this.subCursor % 10));
            dg.DrawMicroText("にｱ", this._position, white2);
        }

        private void HertzNames()
        {
            this.txt = new string[2];
            this.txt[0] = "ヘルツ";
            this.txt[1] = "コア";
            this.subCursor = 0;
        }

        public void HertzContorol()
        {
            if (Input.IsPress(Button.Up))
            {
                --this.subCursor;
                if (this.subCursor < 0)
                    this.subCursor = this.txt.Length - 1;
            }
            if (Input.IsPress(Button.Down))
            {
                ++this.subCursor;
                if (this.subCursor > this.txt.Length - 1)
                    this.subCursor = 0;
            }
            if (Input.IsPress(Button._R))
            {
                switch (this.subCursor)
                {
                    case 0:
                        ++this.savedata.MaxHz;
                        break;
                    case 1:
                        ++this.savedata.MaxCore;
                        break;
                }
            }
            if (Input.IsPress(Button._L))
            {
                switch (this.subCursor)
                {
                    case 0:
                        --this.savedata.MaxHz;
                        break;
                    case 1:
                        --this.savedata.MaxCore;
                        break;
                }
            }
            if (!Input.IsPress(Button._B))
                return;
            this.topmenu = true;
        }

        public void HertzRender(IRenderer dg)
        {
            Color yellow1 = Color.Yellow;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawMiniText("コア・ヘルツ増減", this._position, yellow1);
            Color yellow2 = Color.Yellow;
            this._position = new Vector2(0.0f, 144f);
            dg.DrawMiniText("R：プラス　L：マイナス", this._position, yellow2);
            Color white1 = Color.White;
            int num1 = this.subCursor / 10;
            for (int index = 0; index < this.txt.Length; ++index)
            {
                int num2 = 0;
                switch (index)
                {
                    case 0:
                        num2 = this.savedata.MaxHz;
                        break;
                    case 1:
                        num2 = this.savedata.MaxCore;
                        break;
                }
                Color color1 = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(80f, 16 + 12 * index);
                dg.DrawMiniText(num2.ToString(), this._position, color1);
                Color color2 = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(16f, 16 + 12 * index);
                dg.DrawMiniText(this.txt[index], this._position, color2);
            }
            Color white2 = Color.White;
            this._position = new Vector2(0.0f, 16 + 12 * (this.subCursor % 10));
            dg.DrawMicroText("にｱ", this._position, white2);
        }

        private void MoneyNames()
        {
            this.txt = new string[8];
            for (int index1 = 0; index1 < this.txt.Length; ++index1)
            {
                this.txt[index1] = "1";
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    // ISSUE: explicit reference operation
                    this.txt[index1] += "0";
                }
            }
            this.subCursor = 0;
        }

        public void MoneyContorol()
        {
            if (Input.IsPress(Button.Up))
            {
                --this.subCursor;
                if (this.subCursor < 0)
                    this.subCursor = this.txt.Length - 1;
            }
            if (Input.IsPress(Button.Down))
            {
                ++this.subCursor;
                if (this.subCursor > this.txt.Length - 1)
                    this.subCursor = 0;
            }
            if (Input.IsPress(Button.Left))
            {
                this.subCursor -= 10;
                if (this.subCursor < 0)
                    this.subCursor += this.names.Length - 10;
            }
            if (Input.IsPress(Button._R))
                this.savedata.Money += int.Parse(this.txt[this.subCursor]);
            if (Input.IsPress(Button._L))
            {
                this.savedata.Money -= int.Parse(this.txt[this.subCursor]);
                if (this.savedata.Money < 0)
                    this.savedata.Money = 0;
            }
            if (!Input.IsPress(Button._B))
                return;
            this.topmenu = true;
        }

        public void MoneyRender(IRenderer dg)
        {
            Color yellow1 = Color.Yellow;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawMiniText("所持金増減", this._position, yellow1);
            Color yellow2 = Color.Yellow;
            this._position = new Vector2(0.0f, 144f);
            dg.DrawMiniText("R：プラス　L：マイナス", this._position, yellow2);
            Color yellow3 = Color.Yellow;
            this._position = new Vector2(120f, 0.0f);
            dg.DrawMiniText("所持金 " + this.savedata.Money.ToString(), this._position, yellow3);
            Color white1 = Color.White;
            int num = this.subCursor / 10;
            for (int index = 0; index < this.txt.Length; ++index)
            {
                Color color = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(16f, 16 + 12 * index);
                dg.DrawMiniText(this.txt[index] + " ゼニー", this._position, color);
            }
            Color white2 = Color.White;
            this._position = new Vector2(0.0f, 16 + 12 * (this.subCursor % 10));
            dg.DrawMicroText("にｱ", this._position, white2);
        }

        private void ChipNames()
        {
            this.codes = new string[450, 4];
            this.names = new string[450];
            List<string> stringList = new List<string>();
            for (int key = 0; key < 450; ++key)
            {
                ChipFolder chipFolder = new ChipFolder(null);
                chipFolder.chip = chipFolder.ReturnChip(key);
                stringList.Add(chipFolder.chip.name);
                for (int index = 0; index < 4; ++index)
                    this.codes[key, index] = chipFolder.chip.code[index].ToString();
            }
            this.names = stringList.ToArray();
        }

        public void ChipContorol()
        {
            if (Input.IsPress(Button.Up))
            {
                --this.subCursor;
                if (this.subCursor < 0)
                    this.subCursor = this.names.Length - 1;
            }
            if (Input.IsPress(Button.Down))
            {
                ++this.subCursor;
                if (this.subCursor > this.names.Length - 1)
                    this.subCursor = 0;
            }
            if (Input.IsPress(Button.Left))
            {
                this.subCursor -= 10;
                if (this.subCursor < 0)
                    this.subCursor += this.names.Length - 10;
            }
            if (Input.IsPress(Button._R))
            {
                ++this.code;
                if (this.code >= 4)
                    this.code = 0;
            }
            if (Input.IsPress(Button._L))
            {
                --this.code;
                if (this.code < 0)
                    this.code = 3;
            }
            if (Input.IsPress(Button.Right))
            {
                this.subCursor += 10;
                if (this.subCursor > this.names.Length - 1)
                    this.subCursor -= this.names.Length;
            }
            if (Input.IsPress(Button._A))
            {
                for (int index = 0; index < 1; ++index)
                    this.savedata.AddChip(this.subCursor, this.code, true);
            }
            if (Input.IsPress(Button._Select))
                this.savedata.LosChip(this.subCursor, this.code);
            if (!Input.IsPress(Button._B))
                return;
            this.topmenu = true;
        }

        public void ChipRender(IRenderer dg)
        {
            Color yellow1 = Color.Yellow;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawMiniText("チップ管理", this._position, yellow1);
            Color yellow2 = Color.Yellow;
            this._position = new Vector2(120f, 0.0f);
            dg.DrawMiniText("コード " + this.code.ToString(), this._position, yellow2);
            Color white1 = Color.White;
            int num = this.subCursor / 10;
            for (int index = 0; index < 10; ++index)
            {
                Color yellowGreen = Color.YellowGreen;
                this._position = new Vector2(16f, 16 + 12 * index);
                dg.DrawMiniText((num * 10 + index).ToString(), this._position, yellowGreen);
                Color color1 = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(40f, 16 + 12 * index);
                dg.DrawMiniText(this.names[num * 10 + index], this._position, color1);
                Color color2 = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(120f, 16 + 12 * index);
                dg.DrawMiniText(this.codes[num * 10 + index, this.code], this._position, color2);
                Color lightBlue = Color.LightBlue;
                this._position = new Vector2(196f, 16 + 12 * index);
                dg.DrawMiniText(this.savedata.Havechip[num * 10 + index, this.code].ToString(), this._position, lightBlue);
            }
            Color white2 = Color.White;
            this._position = new Vector2(0.0f, 16 + 12 * (this.subCursor % 10));
            dg.DrawMicroText("にｱ", this._position, white2);
        }

        public DebugMode(IAudioEngine s, SaveData savedata, SceneMap parent)
        {
            this.sound = s;
            this.savedata = savedata;
            this.parent = parent;
            this.menus = Enum.GetNames(typeof(DebugMode.MENU));
            this.menuOn = true;
            this.topmenu = true;
        }

        public void Update()
        {
            if (this.topmenu)
            {
                this.MenuInit();
                this.TopContorol();
            }
            else
            {
                switch (this.nowmenu)
                {
                    case DebugMode.MENU.フラグ管理:
                        this.FlagContorol();
                        break;
                    case DebugMode.MENU.変数管理:
                        this.ValueContorol();
                        break;
                    case DebugMode.MENU.チップ増減:
                        this.ChipContorol();
                        break;
                    case DebugMode.MENU.アドオン入手:
                        this.AddonContorol();
                        break;
                    case DebugMode.MENU.所持金増減:
                        this.MoneyContorol();
                        break;
                    case DebugMode.MENU.コアヘルツ増減:
                        this.HertzContorol();
                        break;
                    case DebugMode.MENU.かけら増減:
                        this.PeaceContorol();
                        break;
                    case DebugMode.MENU.スタイル変更:
                        this.StyleContorol();
                        break;
                }
            }
        }

        public void MenuInit()
        {
            this.subCursor = 0;
            switch (this.nowmenu)
            {
                case DebugMode.MENU.フラグ管理:
                    this.FlagNames();
                    break;
                case DebugMode.MENU.変数管理:
                    this.ValueNames();
                    break;
                case DebugMode.MENU.チップ増減:
                    this.ChipNames();
                    break;
                case DebugMode.MENU.クイックバトル:
                    this.menuOn = false;
                    this.parent.eventmanager.events.Clear();
                    this.parent.eventmanager.AddEvent(new NSEvent.Battle(this.sound, this.parent.eventmanager, 65, 1, 4, 1, 0, 0, 0, 0, "", 0, 2, 5, 2, 0, 0, 0, 0, "", 0, 2, 5, 0, 0, 0, 0, 0, "", Panel.PANEL._nomal, Panel.PANEL._nomal, 0, false, true, true, true, "VSnavi", 37, this.savedata));
                    this.parent.eventmanager.AddEvent(new Fade(this.sound, this.parent.eventmanager, 10, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, false, this.savedata));
                    this.parent.eventmanager.playevent = true;
                    break;
                case DebugMode.MENU.アドオン入手:
                    this.AddonNames();
                    break;
                case DebugMode.MENU.所持金増減:
                    this.MoneyNames();
                    break;
                case DebugMode.MENU.コアヘルツ増減:
                    this.HertzNames();
                    break;
                case DebugMode.MENU.かけら増減:
                    this.PeaceNames();
                    break;
                case DebugMode.MENU.スタイル変更:
                    this.StyleNames();
                    break;
            }
        }

        public void Contorol()
        {
        }

        public void Render(IRenderer dg)
        {
            Color color = Color.FromArgb(200, Color.Blue);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color);
            if (this.topmenu)
            {
                this.TopRender(dg);
            }
            else
            {
                switch (this.nowmenu)
                {
                    case DebugMode.MENU.フラグ管理:
                        this.FlagRender(dg);
                        break;
                    case DebugMode.MENU.変数管理:
                        this.ValueRender(dg);
                        break;
                    case DebugMode.MENU.チップ増減:
                        this.ChipRender(dg);
                        break;
                    case DebugMode.MENU.アドオン入手:
                        this.AddonRender(dg);
                        break;
                    case DebugMode.MENU.所持金増減:
                        this.MoneyRender(dg);
                        break;
                    case DebugMode.MENU.コアヘルツ増減:
                        this.HertzRender(dg);
                        break;
                    case DebugMode.MENU.かけら増減:
                        this.PeaceRender(dg);
                        break;
                    case DebugMode.MENU.スタイル変更:
                        this.StyleRender(dg);
                        break;
                }
            }
        }

        public void TopContorol()
        {
            if (Input.IsPress(Button.Up))
            {
                --this.topCursor;
                if (this.topCursor < 0)
                    this.topCursor = this.menus.Length - 1;
            }
            if (Input.IsPress(Button.Down))
            {
                ++this.topCursor;
                if (this.topCursor > this.menus.Length - 1)
                    this.topCursor = 0;
            }
            if (Input.IsPress(Button._A))
            {
                this.topmenu = false;
                this.nowmenu = (DebugMode.MENU)this.topCursor;
                this.MenuInit();
            }
            if (!Input.IsPress(Button._B))
                return;
            this.menuOn = false;
        }

        public void TopRender(IRenderer dg)
        {
            Color yellow = Color.Yellow;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawMiniText("デバッグメニュー", this._position, yellow);
            Color white = Color.White;
            for (int index = 0; index < this.menus.Length; ++index)
            {
                this._position = new Vector2(16f, 16 + 12 * index);
                dg.DrawMiniText(this.menus[index], this._position, white);
            }
            this._position = new Vector2(0.0f, 16 + 12 * this.topCursor);
            dg.DrawMicroText("にｱ", this._position, white);
        }

        private void AddonNames()
        {
            this.addons = new string[100];
            for (int number = 0; number < 100; ++number)
            {
                try
                {
                    AddOnBase addOnBase = AddOnBase.AddOnSet(number, 0);
                    this.addons[number] = addOnBase.name;
                }
                catch
                {
                    this.addons[number] = "－－けつばん－－";
                }
            }
            this.code = 0;
        }

        public void AddonContorol()
        {
            if (Input.IsPress(Button.Up))
            {
                --this.subCursor;
                if (this.subCursor < 0)
                    this.subCursor = this.addons.Length - 1;
            }
            if (Input.IsPress(Button.Down))
            {
                ++this.subCursor;
                if (this.subCursor > this.addons.Length - 1)
                    this.subCursor = 0;
            }
            if (Input.IsPress(Button.Left))
            {
                this.subCursor -= 10;
                if (this.subCursor < 0)
                    this.subCursor += this.addons.Length - 10;
            }
            if (Input.IsPress(Button._R))
            {
                ++this.code;
                if (this.code >= Enum.GetNames(typeof(AddOnBase.ProgramColor)).Length)
                    this.code = 0;
            }
            if (Input.IsPress(Button._L))
            {
                --this.code;
                if (this.code < 0)
                    this.code = Enum.GetNames(typeof(AddOnBase.ProgramColor)).Length - 1;
            }
            if (Input.IsPress(Button.Right))
            {
                this.subCursor += 10;
                if (this.subCursor > this.addons.Length - 1)
                    this.subCursor -= this.addons.Length;
            }
            if (Input.IsPress(Button._A))
            {
                try
                {
                    this.savedata.GetAddon(AddOnBase.AddOnSet(this.subCursor, this.code));
                    this.sound.PlaySE(SoundEffect.getchip);
                }
                catch
                {
                    this.sound.PlaySE(SoundEffect.error);
                }
            }
            if (!Input.IsPress(Button._Select)) { }
            if (!Input.IsPress(Button._B))
                return;
            this.topmenu = true;
            this.menuOn = false;
        }

        public void AddonRender(IRenderer dg)
        {
            Color yellow1 = Color.Yellow;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawMiniText("アドオン追加", this._position, yellow1);
            Color yellow2 = Color.Yellow;
            this._position = new Vector2(120f, 0.0f);
            string str = "";
            switch (this.code)
            {
                case 0:
                    str = "灰";
                    break;
                case 1:
                    str = "桃";
                    break;
                case 2:
                    str = "空";
                    break;
                case 3:
                    str = "赤";
                    break;
                case 4:
                    str = "青";
                    break;
                case 5:
                    str = "緑";
                    break;
                case 6:
                    str = "黄";
                    break;
                case 7:
                    str = "闇";
                    break;
            }
            dg.DrawMiniText("カラー　 " + str, this._position, yellow2);
            Color white1 = Color.White;
            int num = this.subCursor / 10;
            for (int index = 0; index < 10; ++index)
            {
                Color yellowGreen = Color.YellowGreen;
                this._position = new Vector2(16f, 16 + 12 * index);
                dg.DrawMiniText((num * 10 + index).ToString(), this._position, yellowGreen);
                Color color = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(40f, 16 + 12 * index);
                dg.DrawMiniText(this.addons[num * 10 + index], this._position, color);
            }
            Color white2 = Color.White;
            this._position = new Vector2(0.0f, 16 + 12 * (this.subCursor % 10));
            dg.DrawMicroText("にｱ", this._position, white2);
        }

        private void FlagNames()
        {
            string path = "Editer/flaglist.txt";
            if (!File.Exists(path))
                return;
            StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("Shift_JIS"));
            this.names = new string[2000];
            List<string> stringList = new List<string>();
            for (int index = 0; index < this.names.Length; ++index)
            {
                string str;
                if ((str = streamReader.ReadLine()) != null)
                    stringList.Add(str);
                else
                    stringList.Add("");
            }
            streamReader.Close();
            this.names = stringList.ToArray();
        }

        public void FlagContorol()
        {
            if (Input.IsPress(Button.Up))
            {
                --this.subCursor;
                if (this.subCursor < 0)
                    this.subCursor = this.names.Length - 1;
            }
            if (Input.IsPress(Button.Down))
            {
                ++this.subCursor;
                if (this.subCursor > this.names.Length - 1)
                    this.subCursor = 0;
            }
            if (Input.IsPress(Button.Left))
            {
                this.subCursor -= 10;
                if (this.subCursor < 0)
                    this.subCursor += this.names.Length - 10;
            }
            if (Input.IsPress(Button._R))
            {
                for (int index = 0; index < 10; ++index)
                {
                    this.subCursor += 10;
                    if (this.subCursor > this.names.Length - 1)
                        this.subCursor -= this.names.Length;
                }
            }
            if (Input.IsPress(Button._L))
            {
                for (int index = 0; index < 10; ++index)
                {
                    this.subCursor -= 10;
                    if (this.subCursor < 0)
                        this.subCursor += this.names.Length - 10;
                }
            }
            if (Input.IsPress(Button.Right))
            {
                this.subCursor += 10;
                if (this.subCursor > this.names.Length - 1)
                    this.subCursor -= this.names.Length;
            }
            if (Input.IsPress(Button._A))
            {
                int num = this.subCursor / 10;
                this.savedata.FlagList[this.subCursor] = !this.savedata.FlagList[this.subCursor];
            }
            if (!Input.IsPress(Button._B))
                return;
            this.topmenu = true;
        }

        public void FlagRender(IRenderer dg)
        {
            Color yellow = Color.Yellow;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawMiniText("フラグ管理", this._position, yellow);
            Color white1 = Color.White;
            int num = this.subCursor / 10;
            for (int index = 0; index < 10; ++index)
            {
                Color yellowGreen = Color.YellowGreen;
                this._position = new Vector2(16f, 16 + 12 * index);
                dg.DrawMiniText((num * 10 + index).ToString(), this._position, yellowGreen);
                Color color1 = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(40f, 16 + 12 * index);
                dg.DrawMiniText(this.names[num * 10 + index], this._position, color1);
                Color color2 = this.savedata.FlagList[num * 10 + index] ? Color.LightBlue : Color.MediumVioletRed;
                this._position = new Vector2(196f, 16 + 12 * index);
                dg.DrawMiniText(this.savedata.FlagList[num * 10 + index].ToString(), this._position, color2);
            }
            Color white2 = Color.White;
            this._position = new Vector2(0.0f, 16 + 12 * (this.subCursor % 10));
            dg.DrawMicroText("にｱ", this._position, white2);
        }

        private void ValueNames()
        {
            string path = "Editer/valuelist.txt";
            if (!File.Exists(path))
                return;
            StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("Shift_JIS"));
            this.names = new string[200];
            List<string> stringList = new List<string>();
            for (int index = 0; index < this.names.Length; ++index)
            {
                string str;
                if ((str = streamReader.ReadLine()) != null)
                    stringList.Add(str);
                else
                    stringList.Add("");
            }
            streamReader.Close();
            this.names = stringList.ToArray();
        }

        public void ValueContorol()
        {
            if (this.valueSet)
            {
                if (Input.IsPress(Button.Up))
                    this.savedata.ValList[this.subCursor] -= this.plusValue;
                if (Input.IsPress(Button.Down))
                    this.savedata.ValList[this.subCursor] += this.plusValue;
                if (Input.IsPress(Button.Left) || Input.IsPress(Button._L))
                {
                    this.plusValue /= 10;
                    if (this.plusValue <= 0)
                        this.plusValue = 1;
                }
                if (Input.IsPress(Button.Right) || Input.IsPress(Button._R))
                {
                    this.plusValue *= 10;
                    if (this.plusValue <= 0)
                        this.plusValue = 1;
                }
                if (!Input.IsPress(Button._A) && !Input.IsPress(Button._B))
                    return;
                this.valueSet = false;
            }
            else
            {
                if (Input.IsPress(Button.Up))
                {
                    --this.subCursor;
                    if (this.subCursor < 0)
                        this.subCursor = this.names.Length - 1;
                }
                if (Input.IsPress(Button.Down))
                {
                    ++this.subCursor;
                    if (this.subCursor > this.names.Length - 1)
                        this.subCursor = 0;
                }
                if (Input.IsPress(Button.Left))
                {
                    this.subCursor -= 10;
                    if (this.subCursor < 0)
                        this.subCursor += this.names.Length - 10;
                }
                if (Input.IsPress(Button._R))
                {
                    for (int index = 0; index < 10; ++index)
                    {
                        this.subCursor += 10;
                        if (this.subCursor > this.names.Length - 1)
                            this.subCursor -= this.names.Length;
                    }
                }
                if (Input.IsPress(Button._L))
                {
                    for (int index = 0; index < 10; ++index)
                    {
                        this.subCursor -= 10;
                        if (this.subCursor < 0)
                            this.subCursor += this.names.Length - 10;
                    }
                }
                if (Input.IsPress(Button.Right))
                {
                    this.subCursor += 10;
                    if (this.subCursor > this.names.Length - 1)
                        this.subCursor -= this.names.Length;
                }
                if (Input.IsPress(Button._A))
                    this.valueSet = true;
                if (Input.IsPress(Button._B))
                    this.topmenu = true;
            }
        }

        public void ValueRender(IRenderer dg)
        {
            Color yellow1 = Color.Yellow;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawMiniText("変数管理", this._position, yellow1);
            if (this.valueSet)
            {
                Color yellow2 = Color.Yellow;
                this._position = new Vector2(180f, 0.0f);
                dg.DrawMiniText("＋" + this.plusValue.ToString(), this._position, yellow2);
            }
            Color white1 = Color.White;
            int num = this.subCursor / 10;
            for (int index = 0; index < 10; ++index)
            {
                Color yellowGreen = Color.YellowGreen;
                this._position = new Vector2(16f, 16 + 12 * index);
                dg.DrawMiniText((num * 10 + index).ToString(), this._position, yellowGreen);
                Color color1 = index == this.subCursor % 10 ? Color.Orange : Color.White;
                this._position = new Vector2(40f, 16 + 12 * index);
                dg.DrawMiniText(this.names[num * 10 + index], this._position, color1);
                Color color2 = index != this.subCursor % 10 || !this.valueSet ? Color.Yellow : Color.LightBlue;
                this._position = new Vector2(180f, 16 + 12 * index);
                dg.DrawMiniText(this.savedata.ValList[num * 10 + index].ToString(), this._position, color2);
            }
            Color white2 = Color.White;
            this._position = new Vector2(0.0f, 16 + 12 * (this.subCursor % 10));
            dg.DrawMicroText("にｱ", this._position, white2);
        }

        private enum MENU
        {
            フラグ管理,
            変数管理,
            チップ増減,
            クイックバトル,
            アドオン入手,
            所持金増減,
            コアヘルツ増減,
            かけら増減,
            スタイル変更,
        }
    }
}
