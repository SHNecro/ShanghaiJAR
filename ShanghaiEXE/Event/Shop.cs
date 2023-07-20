using Common;
using NSAddOn;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character.Menu;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEvent
{
    internal class Shop : EventBase
    {
        private readonly string[] massage = new string[3];
        private Shop.SCENE nowscene;
        private readonly EventManager eventmanager;
        private readonly int shopType;
        private readonly int assistant;
        private readonly int moneyType;
        private readonly Goods[] goods;
        public const int manyShops = 40;
        public const int manyGoods = 10;
        protected const string texture = "menuwindows";
        private readonly MapField field;
        private bool shopmode;
        private readonly int faceSeet;
        private readonly byte faceNo;
        private int cursolanime;
        private byte alpha;
        private readonly int shopNo;
        private readonly int overTop;
        private int cursol;
        private int top;
        private int waittime;
        private const byte waitlong = 10;
        private const byte waitshort = 4;
        private bool yesnoSelect;
        private int price;
        private bool help;
        private bool sell;
        private bool mono;
        private bool auto;
		private int autoFrame;
		private int autoFrameRaw;
         
		private int Select
        {
            get
            {
                return this.cursol + this.top;
            }
        }

        public Shop(
          IAudioEngine s,
          EventManager m,
          int No,
          int type,
          int astype,
          int face,
          byte faceno,
		  bool mono,
          bool auto,
          Goods[] gs,
          MapField f,
          SaveData save,
          int moneyType)
          : base(s, m, save)
        {
            this.NoTimeNext = false;
            this.nowscene = Shop.SCENE.fadein;
            this.moneyType = moneyType;
            this.goods = gs;
            this.shopType = type;
            this.field = f;
            this.assistant = astype;
            this.faceSeet = face;
            this.faceNo = faceno;
            this.mono = mono;
            this.auto = auto;
            this.shopNo = No;
            this.eventmanager = new EventManager(this.sound);
            this.overTop = this.goods.Length - 5;
            if (this.overTop < 0)
                this.overTop = 0;
            switch (this.assistant)
            {
                case 0:
                    this.massage[0] = ShanghaiEXE.Translate("Shop.Assistant1Line1");
                    break;
                case 1:
                    this.massage[0] = ShanghaiEXE.Translate("Shop.Assistant2Line1");
                    break;
                case 2:
                    this.massage[0] = ShanghaiEXE.Translate("Shop.Assistant3Line1");
                    break;
				case 3:
					this.massage[0] = ShanghaiEXE.Translate("Shop.Assistant4Line1");
					break;
				case 4:
					this.massage[0] = ShanghaiEXE.Translate("Shop.Assistant5Line1");
					break;
			}
            this.massage[1] = this.shopType != 3 ? ShanghaiEXE.Translate("Shop.AssistantLine2") : "";
            this.massage[2] = ShanghaiEXE.Translate("Shop.AssistantLine3");
        }

        public override void Update()
		{
			this.autoFrameRaw++;
			if (this.autoFrameRaw++ >= 5)
			{
				this.autoFrameRaw = 0;

				this.autoFrame = (this.autoFrame + 1) % 6;
			}

			switch (this.nowscene)
            {
                case Shop.SCENE.fadein:
                    if (!this.shopmode)
                    {
                        this.alpha += 51;
                        if (this.alpha >= byte.MaxValue)
                        {
                            this.alpha = byte.MaxValue;
                            this.shopmode = true;
                            break;
                        }
                        break;
                    }
                    this.alpha -= 51;
                    if (this.alpha <= 0)
                        this.nowscene = Shop.SCENE.select;
                    break;
                case Shop.SCENE.select:
                    if (this.eventmanager.playevent)
                        this.eventmanager.UpDate();
                    if (!this.eventmanager.playevent)
                    {
                        if (this.yesnoSelect)
                        {
                            if (this.sell)
                                this.Sell();
                            else
                                this.Buy();
                        }
                        else
                            this.Control();
                        break;
                    }
                    break;
                case Shop.SCENE.fadeout:
                    if (this.shopmode)
                    {
                        this.alpha += 51;
                        if (this.alpha >= byte.MaxValue)
                        {
                            this.alpha = byte.MaxValue;
                            this.shopmode = false;
                            break;
                        }
                        break;
                    }
                    this.alpha -= 51;
                    if (this.alpha <= 0)
                    {
                        this.nowscene = Shop.SCENE.fadein;
                        this.cursol = 0;
                        this.top = 0;
                        this.EndCommand();
                    }
                    break;
            }
            this.FlameControl(10);
            if (!this.moveflame)
                return;
            ++this.cursolanime;
            if (this.cursolanime >= 3)
                this.cursolanime = 0;
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
            {
                if (!this.help)
                {
                    if (this.savedata.ShopCount[this.shopNo, this.Select] < this.goods[this.Select].stock || this.goods[this.Select].stock == 0)
                    {
                        this.sound.PlaySE(SoundEffect.decide);
                        this.BuyEvent();
                    }
                    else if (this.shopType != 3)
                    {
                        this.sound.PlaySE(SoundEffect.error);
                        var dialogue = new Dialogue();
                        switch (this.assistant)
                        {
                            case 0:
                                dialogue = ShanghaiEXE.Translate("Shop.Assistant1OutOfStockDialogue1");
                                break;
                            case 1:
                                dialogue = ShanghaiEXE.Translate("Shop.Assistant2OutOfStockDialogue1");
                                break;
                            case 2:
                                dialogue = ShanghaiEXE.Translate("Shop.Assistant3OutOfStockDialogue1");
                                break;
							case 3:
								dialogue = ShanghaiEXE.Translate("Shop.Assistant4OutOfStockDialogue1");
								break;
							case 4:
								dialogue = ShanghaiEXE.Translate("Shop.Assistant5OutOfStockDialogue1");
								break;
						}
                        this.eventmanager.events.Clear();
                        this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, this.faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
                    }
                    else
                    {
                        this.sell = true;
                        this.SellEvent();
                    }
                }
                else
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.help = false;
                }
            }
            else if (Input.IsPress(Button._B))
            {
                if (!this.help)
                {
                    this.sound.PlaySE(SoundEffect.cancel);
                    this.nowscene = Shop.SCENE.fadeout;
                }
                else
                {
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.help = false;
                }
            }
            else if (Input.IsPress(Button._R) && this.shopType != 3)
            {
                this.sound.PlaySE(SoundEffect.movecursol);
                if (!this.help)
                    this.help = true;
                else
                    this.help = false;
            }
            else if (this.waittime <= 0)
            {
                if (Input.IsPush(Button.Up))
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
                    if (!Input.IsPush(Button.Down) || this.Select >= (this.goods.Length > 10 ? 9 : this.goods.Length - 1))
                        return;
                    if (this.cursol < 4)
                        ++this.cursol;
                    else
                        ++this.top;
                    this.sound.PlaySE(SoundEffect.movecursol);
                    this.waittime = Input.IsPress(Button.Down) ? 10 : 4;
                }
            }
            else
                --this.waittime;
        }

        public override void Render(IRenderer dg)
        {
            if (this.shopmode)
            {
                this.field.back.Render(dg);
                this._rect = new Rectangle(656, 0, 160, 104);
                this._position = new Vector2(0.0f, 0.0f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                Vector2 point;
                for (int index = 0; index < Math.Min(5, this.goods.Length); ++index)
                {
                    int num = 0;
                    Color color = Color.White;
                    if (this.goods[this.top + index].stock > 0 && this.savedata.ShopCount[this.shopNo, this.top + index] >= this.goods[this.top + index].stock)
                        color = Color.Gray;
                    switch (this.shopType)
                    {
                        case 0:
                            if (this.goods[this.top + index].numberNo > 0)
                            {
                                ChipS chipS = new ChipS(this.goods[this.top + index].numberNo, this.goods[this.top + index].numberSub);
                                AllBase.NAME[] nameArray = this.Nametodata(chipS.Name);
                                point = new Vector2(16f, 12 + index * 16);
                                this._position = new Vector2(point.X, point.Y);
                                DrawBlockCharacters(dg, nameArray, 16, this._position, color, out this._rect, out this._position);
                                this._position = new Vector2(88f, 12 + 16 * index);
                                this._rect = new Rectangle(chipS.Code * 8, 32, 8, 16);
                                dg.DrawImage(dg, "font", this._rect, true, this._position, color);
                                num = this.goods[this.top + index].price;
                                break;
                            }
                            AllBase.NAME[] nameArray1 = this.Nametodata(ShanghaiEXE.Translate("Shop.HPMemory"));
                            point = new Vector2(16f, 12 + index * 16);
                            this._position = new Vector2(point.X, point.Y);
                            DrawBlockCharacters(dg, nameArray1, 16, this._position, color, out this._rect, out this._position);
                            num = this.goods[this.top + index].price + this.goods[this.top + index].numberSub * this.savedata.ShopCount[this.shopNo, index];
                            if (this.savedata.ShopCount[this.shopNo, this.top + index] >= this.goods[this.top + index].stock)
                                num -= this.goods[this.top + index].numberSub;
                            break;
                        case 1:
                            AllBase.NAME[] nameArray2 = this.Nametodata(SubChip.NameSet(this.goods[this.top + index].numberNo));
                            point = new Vector2(16f, 12 + index * 16);
                            this._position = new Vector2(point.X, point.Y);
                            DrawBlockCharacters(dg, nameArray2, 16, this._position, color, out this._rect, out this._position);
                            num = SubChip.PriceSet(this.goods[this.top + index].numberNo) - this.goods[this.top + index].numberSub;
                            break;
                        case 2:
                            AllBase.NAME[] nameArray3 = this.Nametodata(AddOnBase.AddOnSet(this.goods[this.top + index].numberNo, this.goods[this.top + index].numberSub).name);
                            point = new Vector2(16f, 12 + index * 16);
                            this._position = new Vector2(point.X, point.Y);
                            DrawBlockCharacters(dg, nameArray3, 16, this._position, color, out this._rect, out this._position);
                            AllBase.NAME[] nameArray4 = this.Nametodata(AddOnBase.ColorToAlphabet(this.goods[this.top + index].numberSub));
                            point = new Vector2(88f, 12 + index * 16);
                            this._position = new Vector2(point.X, point.Y);
                            DrawBlockCharacters(dg, nameArray4, 16, this._position, color, out this._rect, out this._position);
                            num = this.goods[this.top + index].price;
                            break;
                        case 3:
                            point = new Vector2(16f, 14 + index * 16);
                            ++point.X;
                            ++point.Y;
                            dg.DrawMiniText(Shop.INTERIOR.GetItem(this.goods[this.top + index].numberNo), point, Color.FromArgb(32, 32, 32));
                            --point.X;
                            --point.Y;
                            dg.DrawMiniText(Shop.INTERIOR.GetItem(this.goods[this.top + index].numberNo), point, Color.White);
                            num = this.goods[this.top + index].price;
                            break;
                    }
                    this._position = new Vector2(8f, 12 + 16 * index);
                    AllBase.NAME[] nameArray5 = this.moneyType != 0 ? this.Nametodata(num.ToString()) : this.Nametodata(num.ToString() + "Z");
                    point = new Vector2(152 - nameArray5.Length * 8, 12 + index * 16);
                    foreach (var data in ((IEnumerable<AllBase.NAME>)nameArray5).Select((v, j) => new
                    {
                        v,
                        j
                    }))
                    {
                        this._rect = new Rectangle((int)data.v * 8, 16, 8, 16);
                        this._position = new Vector2(point.X + 8 * data.j, point.Y);
                        dg.DrawImage(dg, "font", this._rect, true, this._position, color);
                    }
                }
                this._rect = new Rectangle(240 + 16 * this.cursolanime, 48, 16, 16);
                this._position = new Vector2(0.0f, 12 + this.cursol * 16);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                float num1 = this.overTop != 0 && this.top != 0 ? 80f / overTop * top : 0.0f;
                this._rect = new Rectangle(176, 168, 8, 8);
                this._position = new Vector2(152f, 8f + num1);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                if (this.top > 0)
                {
                    this._rect = new Rectangle(160 + 8 * this.cursolanime, 208, 8, 8);
                    this._position = new Vector2(84f, 4f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                if (this.top < this.overTop)
                {
                    this._rect = new Rectangle(184 + 8 * this.cursolanime, 208, 8, 8);
                    this._position = new Vector2(84f, 92f);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                }
                this._rect = new Rectangle(0, 0, 240, 56);
                this._position = new Vector2(0.0f, 104f);
                dg.DrawImage(dg, "window", this._rect, true, this._position, Color.White);
                if (!this.help)
                {
                    this._position = new Vector2(5f, 108f);
                    if (this.auto)
				    {
					    this._rect = new Rectangle(40 * this.autoFrame, faceNo * 48, 40, 48);
				    }
				    else if (this.mono)
				    {
					    this._rect = new Rectangle(200, faceNo * 48, 40, 48);
				    }
                    else
                    {
						this._rect = new Rectangle(0, faceNo * 48, 40, 48);
					}
                    string te = "Face" + this.faceSeet.ToString();
                    dg.DrawImage(dg, te, this._rect, true, this._position, Color.White);
                    if (!this.eventmanager.playevent)
                    {
                        for (int index = 0; index < this.massage.Length; ++index)
                        {
                            this._position = new Vector2(48f, 108 + 16 * index);
                            dg.DrawText(this.massage[index], this._position, this.savedata);
                        }
                    }
                }
                else
                {
                    string[] strArray = new string[3] { "", "", "" };
                    switch (this.shopType)
                    {
                        case 0:
                            if ((uint)this.goods[this.Select].numberNo > 0U)
                            {
                                ChipFolder chipFolder = new ChipFolder(null);
                                chipFolder.SettingChip(this.goods[this.Select].numberNo);
                                this._position = new Vector2(80f, 8f);
                                if (chipFolder.chip.dark)
                                    this._rect = new Rectangle(848, 216, 80, 96);
                                else if (chipFolder.chip.navi)
                                    this._rect = new Rectangle(848, 120, 80, 96);
                                else
                                    this._rect = new Rectangle(680, 120, 80, 96);
                                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                                this._position = new Vector2(89f, 25f);
                                chipFolder.chip.GraphicsRender(dg, this._position, this.goods[this.Select].numberSub, true, true);
                                strArray = chipFolder.chip.information;
                                break;
                            }
                            var dialogue = ShanghaiEXE.Translate("Shop.HPMemoryDescriptionDialogue1");
                            strArray[0] = dialogue[0];
                            strArray[1] = dialogue[1];
                            break;
                        case 1:
                            strArray = SubChip.InfoSet(this.goods[this.Select].numberNo);
                            break;
                        case 2:
                            AddOnBase addOnBase = AddOnBase.AddOnSet(this.goods[this.Select].numberNo, this.goods[this.Select].numberSub);
                            addOnBase.Render(dg, false, false, new Vector2(72f, 40f), this);
                            strArray = addOnBase.infomasion;
                            break;
                    }
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                        this._position = new Vector2(48f, 108 + 16 * index);
                        dg.DrawText(strArray[index], this._position, this.savedata);
                    }
                }
                switch (this.moneyType)
                {
                    case 0:
                        this._rect = new Rectangle(488, 232, 80, 40);
                        this._position = new Vector2(160f, 0.0f);
                        dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
                        break;
                    case 1:
                        {
                            var bugFragSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Shop.BugFragCounter");
                            this._rect = bugFragSprite.Item2;
                            this._position = new Vector2(160f, 0.0f);
                            dg.DrawImage(dg, bugFragSprite.Item1, this._rect, true, this._position, Color.White);
                        }
                        break;
                    case 2:
                        {
                            var bugFragSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Shop.FrzFragCounter");
                            this._rect = bugFragSprite.Item2;
                            this._position = new Vector2(160f, 0.0f);
                            dg.DrawImage(dg, bugFragSprite.Item1, this._rect, true, this._position, Color.White);
                        }
                        break;
                    case 3:
                        {
                            var bugFragSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("Shop.ErrFragCounter");
                            this._rect = bugFragSprite.Item2;
                            this._position = new Vector2(160f, 0.0f);
                            dg.DrawImage(dg, bugFragSprite.Item1, this._rect, true, this._position, Color.White);
                        }
                        break;
                }
                if (this.moneyType == 0)
                {
                    if (this.savedata.Money < this.savedata.moneyover)
                    {
                        AllBase.NAME[] nameArray = this.Nametodata(this.savedata.Money.ToString() + "Z");
                        point = new Vector2(232 - nameArray.Length * 8, 16f);
                        foreach (var data in ((IEnumerable<AllBase.NAME>)nameArray).Select((v, j) => new
                        {
                            v,
                            j
                        }))
                        {
                            this._rect = new Rectangle((int)data.v * 8, 88, 8, 16);
                            this._position = new Vector2(point.X + 8 * data.j, point.Y);
                            dg.DrawImage(dg, "font", this._rect, true, this._position, Color.White);
                        }
                    }
                    else
                    {
                        this._position = new Vector2(164f, 16f);
                        this.TextRender(dg, ShanghaiEXE.Translate("Shop.Billionaire"), false, this._position, false);
                    }
                }
                else if (this.savedata.havePeace[this.moneyType - 1] < this.savedata.moneyover)
                {
                    AllBase.NAME[] nameArray = this.Nametodata(string.Format(ShanghaiEXE.Translate("Shop.ZennyFormat"), this.savedata.havePeace[this.moneyType - 1]));
                    point = new Vector2(232 - nameArray.Length * 8, 16f);
                    this._position = new Vector2(point.X, point.Y);
                    DrawBlockCharacters(dg, nameArray, 88, this._position, Color.White, out this._rect, out this._position);
                }
                else
                {
                    this._position = new Vector2(164f, 16f);
                    this.TextRender(dg, ShanghaiEXE.Translate("Shop.Billionaire"), false, this._position, false);
                }
                this._rect = new Rectangle(432, 232, 56, 40);
                this._position = new Vector2(176f, 56f);
                dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            }
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);
            if (this.alpha <= 0)
                return;
            Color color1 = Color.FromArgb(alpha, Color.Black);
            Rectangle _rect = new Rectangle(0, 0, 240, 160);
            Vector2 _point = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", _rect, true, _point, color1);
        }

        private void BuyEvent()
        {
            string str1 = "";
            string str2 = "";
            switch (this.shopType)
            {
                case 0:
                    if (this.goods[this.Select].numberNo > 0)
                    {
                        ChipS chipS = new ChipS(this.goods[this.Select].numberNo, this.goods[this.Select].numberSub);
                        str1 = chipS.Name;
                        str2 = chipS.Code == 26 ? "　＊" : "　" + Enum.GetName(typeof(ChipFolder.CODE), (ChipFolder.CODE)chipS.Code);
                        this.price = this.goods[this.Select].price;
                        break;
                    }
                    str1 = ShanghaiEXE.Translate("Shop.HPMemory");
                    this.price = this.goods[this.Select].price + this.goods[this.Select].numberSub * this.savedata.ShopCount[this.shopNo, this.Select];
                    break;
                case 1:
                    str1 = SubChip.NameSet(this.goods[this.Select].numberNo);
                    this.price = SubChip.PriceSet(this.goods[this.Select].numberNo) - this.goods[this.Select].numberSub;
                    break;
                case 2:
                    str1 = AddOnBase.AddOnSet(this.goods[this.Select].numberNo, this.goods[this.Select].numberSub).name;
                    if (this.goods[this.Select].numberSub > 0)
                        str2 = "　" + AddOnBase.ColorToString(this.goods[this.Select].numberSub);
                    this.price = this.goods[this.Select].price;
                    break;
                case 3:
                    str1 = Shop.INTERIOR.GetItem(this.goods[this.Select].numberNo);
                    this.price = this.goods[this.Select].price;
                    break;
            }
            this.eventmanager.events.Clear();
            if (this.moneyType == 0)
            {
                var question = new Dialogue();
                var options = ShanghaiEXE.Translate("Shop.ZennyOptions");
                switch (this.assistant)
                {
                    case 0:
                        question = ShanghaiEXE.Translate("Shop.Assistant1ZennyDialogue1QuestionFormat").Format(str1, str2, this.price);
                        break;
                    case 1:
                        question = ShanghaiEXE.Translate("Shop.Assistant2ZennyDialogue1QuestionFormat").Format(str1, str2, this.price);
                        break;
                    case 2:
                        question = ShanghaiEXE.Translate("Shop.Assistant3ZennyDialogue1QuestionFormat").Format(str1, str2, this.price);
                        break;
					case 3:
						question = ShanghaiEXE.Translate("Shop.Assistant4ZennyDialogue1QuestionFormat").Format(str1, str2, this.price);
						break;
					case 4:
						question = ShanghaiEXE.Translate("Shop.Assistant5ZennyDialogue1QuestionFormat").Format(str1, str2, this.price);
						break;
				}
                this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager,
                    question[0],
                    question[1],
                    options[0],
                    options[1],
                    true, true, faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
            }
            else
            {
                var question = new Dialogue();
                var options = ShanghaiEXE.Translate("Shop.OtherOptions");
                switch (this.assistant)
                {
                    case 0:
                        question = ShanghaiEXE.Translate("Shop.Assistant1OtherDialogue1QuestionFormat").Format(str1, str2);
                        break;
                    case 1:
                        question = ShanghaiEXE.Translate("Shop.Assistant2OtherDialogue1QuestionFormat").Format(str1, str2);
                        break;
                    case 2:
                        question = ShanghaiEXE.Translate("Shop.Assistant3OtherDialogue1QuestionFormat").Format(str1, str2);
                        break;
					case 3:
						question = ShanghaiEXE.Translate("Shop.Assistant4OtherDialogue1QuestionFormat").Format(str1, str2);
						break;
					case 4:
						question = ShanghaiEXE.Translate("Shop.Assistant5OtherDialogue1QuestionFormat").Format(str1, str2);
						break;
				}
                this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager,
                    question[0],
                    question[1],
                    options[0],
                    options[1],
                    true, true, faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
            }
            this.yesnoSelect = true;
        }

        private void SellEvent()
        {
            string str1 = "";
            string str2 = "";
            if (this.shopType == 3)
            {
                str1 = Shop.INTERIOR.GetItem(this.goods[this.Select].numberNo);
                this.price = this.goods[this.Select].price / 2;
            }
            var question = new Dialogue();
            var options = ShanghaiEXE.Translate("Shop.OtherOptions");
            switch (this.assistant)
            {
                case 0:
                    question = ShanghaiEXE.Translate("Shop.Assistant1SellQuestionFormat").Format(str1, str2, this.price);
                    break;
                case 1:
                    question = ShanghaiEXE.Translate("Shop.Assistant2SellQuestionFormat").Format(str1, str2, this.price);
                    break;
                case 2:
                    question = ShanghaiEXE.Translate("Shop.Assistant3SellQuestionFormat").Format(str1, str2, this.price);
                    break;
				case 3:
					question = ShanghaiEXE.Translate("Shop.Assistant4SellQuestionFormat").Format(str1, str2, this.price);
					break;
				case 4:
					question = ShanghaiEXE.Translate("Shop.Assistant5SellQuestionFormat").Format(str1, str2, this.price);
					break;
			}
            this.eventmanager.events.Clear();
            this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, question[0], question[1], options[0], options[1], true, true, faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
            this.yesnoSelect = true;
        }

        private void Sell()
        {
            this.yesnoSelect = false;
            if (this.savedata.selectQuestion == 0)
            {
                string text1 = "";
                switch (this.assistant)
                {
                    case 0:
                        text1 = ShanghaiEXE.Translate("Shop.Assistant1Sold");
                        break;
                    case 1:
                        text1 = ShanghaiEXE.Translate("Shop.Assistant2Sold");
                        break;
                    case 2:
                        text1 = ShanghaiEXE.Translate("Shop.Assistant3Sold");
                        break;
					case 3:
						text1 = ShanghaiEXE.Translate("Shop.Assistant4Sold");
						break;
					case 4:
						text1 = ShanghaiEXE.Translate("Shop.Assistant5Sold");
						break;
				}
                this.sell = false;
                this.savedata.Money += this.price;
                --this.savedata.ShopCount[this.shopNo, this.Select];
                this.sound.PlaySE(SoundEffect.counterhit);
                if (this.shopType == 3)
                    this.savedata.FlagList[this.goods[this.Select].numberSub] = false;
                this.eventmanager.events.Clear();
                this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, text1, "", "", true, this.faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
            }
            else
            {
				var dialogue = default(Dialogue);
                switch (this.assistant)
				{
					case 0:
						dialogue = ShanghaiEXE.Translate("Shop.Assistant1CancelledDialogue1");
						break;
					case 1:
						dialogue = ShanghaiEXE.Translate("Shop.Assistant2CancelledDialogue1");
						break;
					case 2:
						dialogue = ShanghaiEXE.Translate("Shop.Assistant3CancelledDialogue1");
						break;
					case 3:
						dialogue = ShanghaiEXE.Translate("Shop.Assistant4CancelledDialogue1");
						break;
					case 4:
						dialogue = ShanghaiEXE.Translate("Shop.Assistant5CancelledDialogue1");
						break;
				}
                this.eventmanager.events.Clear();
                this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, this.faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
            }
        }

        private void Buy()
        {
            this.yesnoSelect = false;
            int num = this.moneyType == 0 ? this.savedata.Money : this.savedata.havePeace[this.moneyType - 1];
            if (this.savedata.selectQuestion == 0)
            {
                if (this.price <= num)
                {
                    bool flag = true;
                    switch (this.shopType)
                    {
                        case 0:
                            if ((uint)this.goods[this.Select].numberNo > 0U && this.savedata.Havechip[this.goods[this.Select].numberNo, this.goods[this.Select].numberSub] >= 99)
                            {
                                flag = false;
                                break;
                            }
                            break;
                        case 1:
                            if (this.savedata.haveSubChis[this.goods[this.Select].numberNo] >= this.savedata.haveSubMemory)
                            {
                                flag = false;
                                break;
                            }
                            break;
                    }
                    if (flag)
                    {
                        var dialogue = new Dialogue();
                        switch (this.assistant)
                        {
                            case 0:
                                dialogue = ShanghaiEXE.Translate("Shop.Assistant1BoughtDialogue1");
                                break;
                            case 1:
                                dialogue = ShanghaiEXE.Translate("Shop.Assistant2BoughtDialogue1");
                                break;
                            case 2:
                                dialogue = ShanghaiEXE.Translate("Shop.Assistant3BoughtDialogue1");
                                break;
							case 3:
								dialogue = ShanghaiEXE.Translate("Shop.Assistant4BoughtDialogue1");
								break;
							case 4:
								dialogue = ShanghaiEXE.Translate("Shop.Assistant5BoughtDialogue1");
								break;
						}
                        if (this.moneyType == 0)
                            this.savedata.Money -= this.price;
                        else
                            this.savedata.havePeace[this.moneyType - 1] -= this.price;
                        ++this.savedata.ShopCount[this.shopNo, this.Select];
                        this.sound.PlaySE(SoundEffect.counterhit);
                        switch (this.shopType)
                        {
                            case 0:
                                if (this.goods[this.Select].numberNo == 0)
                                {
                                    this.savedata.HPmax += 20;
                                    this.savedata.HPnow += 20;
                                    break;
                                }
                                this.savedata.AddChip(this.goods[this.Select].numberNo, this.goods[this.Select].numberSub, true);
                                if (this.goods[this.Select].numberNo == 311)
                                {
                                    this.savedata.FlagList[68] = true;
                                    this.savedata.haveCaptureBomb = 2;
                                }
                                if (this.goods[this.Select].numberNo == 312)
                                    this.savedata.haveCaptureBomb = 3;
                                break;
                            case 1:
                                ++this.savedata.haveSubChis[this.goods[this.Select].numberNo];
                                break;
                            case 2:
                                this.savedata.GetAddon(AddOnBase.AddOnSet(this.goods[this.Select].numberNo, this.goods[this.Select].numberSub));
                                break;
                            case 3:
                                this.savedata.interiors.Add(new Interior(this.goods[this.Select].numberNo, 0, 0, false, false));
                                break;
                        }
                        this.eventmanager.events.Clear();
                        this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, this.faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
                    }
                    else
                    {
                        this.sound.PlaySE(SoundEffect.error);
                        this.eventmanager.events.Clear();
                        var dialogue = ShanghaiEXE.Translate("Shop.InventoryFullDialogue1");
                        this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, dialogue.Face, dialogue.Face.Mono, this.savedata));
                    }
                }
                else
                {
                    this.sound.PlaySE(SoundEffect.error);
                    this.eventmanager.events.Clear();
                    if (this.moneyType == 0)
					{
						var dialogue = new Dialogue();
						switch (this.assistant)
						{
							case 4:
								dialogue = ShanghaiEXE.Translate("Shop.InsufficientZennyDialogue2");
								break;
                            default:
								dialogue = ShanghaiEXE.Translate("Shop.InsufficientZennyDialogue1");
								break;
						}
                        this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, this.faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
                    }
                    else
					{
						var dialogue = new Dialogue();
						switch (this.assistant)
						{
							case 4:
								dialogue = ShanghaiEXE.Translate("Shop.InsufficientOtherDialogue2");
								break;
							default:
								dialogue = ShanghaiEXE.Translate("Shop.InsufficientOtherDialogue1");
								break;
						}
                        this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, this.faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
                    }
                }
            }
            else
            {
                var dialogue = new Dialogue();
                switch (this.assistant)
                {
                    case 0:
                        dialogue = ShanghaiEXE.Translate("Shop.Assistant1CancelledDialogue1");
                        break;
                    case 1:
                        dialogue = ShanghaiEXE.Translate("Shop.Assistant2CancelledDialogue1");
                        break;
                    case 2:
                        dialogue = ShanghaiEXE.Translate("Shop.Assistant3CancelledDialogue1");
                        break;
					case 3:
						dialogue = ShanghaiEXE.Translate("Shop.Assistant4CancelledDialogue1");
						break;
					case 4:
						dialogue = ShanghaiEXE.Translate("Shop.Assistant5CancelledDialogue1");
						break;
				}
                this.eventmanager.events.Clear();
                this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], true, this.faceSeet, this.faceNo, this.mono, this.auto, this.savedata));
            }
        }

        public static class INTERIOR
        {
            private static List<string> items = new List<string> {
                ShanghaiEXE.Translate("Interior.Item1"),
                ShanghaiEXE.Translate("Interior.Item2"),
                ShanghaiEXE.Translate("Interior.Item3"),
                ShanghaiEXE.Translate("Interior.Item4"),
                ShanghaiEXE.Translate("Interior.Item5"),
                ShanghaiEXE.Translate("Interior.Item6"),
                ShanghaiEXE.Translate("Interior.Item7"),
                ShanghaiEXE.Translate("Interior.Item8"),
                ShanghaiEXE.Translate("Interior.Item9"),
                ShanghaiEXE.Translate("Interior.Item10"),
                ShanghaiEXE.Translate("Interior.Item11"),
                ShanghaiEXE.Translate("Interior.Item12"),
                ShanghaiEXE.Translate("Interior.Item13"),
                ShanghaiEXE.Translate("Interior.Item14"),
                ShanghaiEXE.Translate("Interior.Item15"),
                ShanghaiEXE.Translate("Interior.Item16"),
                ShanghaiEXE.Translate("Interior.Item17"),
                ShanghaiEXE.Translate("Interior.Item18"),
                ShanghaiEXE.Translate("Interior.Item19"),
                ShanghaiEXE.Translate("Interior.Item20"),
                ShanghaiEXE.Translate("Interior.Item21"),
                ShanghaiEXE.Translate("Interior.Item22"),
                ShanghaiEXE.Translate("Interior.Item23"),
                ShanghaiEXE.Translate("Interior.Item24"),
                ShanghaiEXE.Translate("Interior.Item25"),
                ShanghaiEXE.Translate("Interior.Item26"),
                ShanghaiEXE.Translate("Interior.Item27"),
                ShanghaiEXE.Translate("Interior.Item28"),
                ShanghaiEXE.Translate("Interior.Item29"),
                ShanghaiEXE.Translate("Interior.Item30"),
                ShanghaiEXE.Translate("Interior.Item31"),
                ShanghaiEXE.Translate("Interior.Item32"),
                ShanghaiEXE.Translate("Interior.Item33"),
                ShanghaiEXE.Translate("Interior.Item34"),
                ShanghaiEXE.Translate("Interior.Item35"),
                ShanghaiEXE.Translate("Interior.Item36"),
                ShanghaiEXE.Translate("Interior.Item37"),
                ShanghaiEXE.Translate("Interior.Item38"),
                ShanghaiEXE.Translate("Interior.Item39"),
                ShanghaiEXE.Translate("Interior.Item40"),
                ShanghaiEXE.Translate("Interior.Item41"),
                ShanghaiEXE.Translate("Interior.Item42"),
                ShanghaiEXE.Translate("Interior.Item43"),
                ShanghaiEXE.Translate("Interior.Item44"),
                ShanghaiEXE.Translate("Interior.Item45"),
                ShanghaiEXE.Translate("Interior.Item46"),
                ShanghaiEXE.Translate("Interior.Item47"),
                ShanghaiEXE.Translate("Interior.Item48"),
                ShanghaiEXE.Translate("Interior.Item49"),
                ShanghaiEXE.Translate("Interior.Item50"),
                ShanghaiEXE.Translate("Interior.Item51"),
                ShanghaiEXE.Translate("Interior.Item52"),
                ShanghaiEXE.Translate("Interior.Item53")
            };

            public static string GetItem(int index)
            {
                if (index < items.Count)
                    return items[index];

                var fallbackKey = $"Interior.Item{index + 1}";
                if (ShanghaiEXE.languageTranslationService.CanTranslate(fallbackKey))
                    return ShanghaiEXE.Translate(fallbackKey);

                return "NO_ITEM";
            }
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
        }

        private enum SHOPSCENE
        {
            select,
            help,
            question,
        }
    }
}
