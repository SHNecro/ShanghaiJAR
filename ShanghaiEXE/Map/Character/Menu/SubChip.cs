using Common;
using ExtensionMethods;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace NSMap.Character.Menu
{
    internal class SubChip : MenuBase
    {
        private string[] info = new string[3];
        private SubChip.SCENE nowscene;
        private readonly EventManager eventmanager;
        private int cursolanime;
        private int cursol;
        private int printHP;
        private bool question;

        private int Cursol
        {
            get
            {
                return this.cursol;
            }
            set
            {
                this.cursol = value;
                if (this.cursol < 0)
                    this.cursol = 6;
                if (this.cursol <= 6)
                    return;
                this.cursol = 0;
            }
        }

        public SubChip(IAudioEngine s, Player p, TopMenu t, EventManager e, SaveData save)
          : base(s, p, t, save)
        {
            this.eventmanager = e;
            this.Alpha = byte.MaxValue;
            this.info = SubChip.InfoSet(this.cursol);
            this.printHP = this.savedata.HPnow;
        }

        public override void UpDate()
        {
            if (this.printHP < this.savedata.HPnow)
                ++this.printHP;
            if (this.printHP > this.savedata.HPnow)
                --this.printHP;
            if (this.eventmanager.playevent)
            {
                this.eventmanager.UpDate();
            }
            else
            {
                switch (this.nowscene)
                {
                    case SubChip.SCENE.fadein:
                        if (this.Alpha > 0)
                        {
                            this.Alpha -= 51;
                            break;
                        }
                        this.nowscene = SubChip.SCENE.select;
                        break;
                    case SubChip.SCENE.select:
                        if (this.question)
                        {
                            switch (this.savedata.selectQuestion)
                            {
                                case 0:
                                    switch (this.Cursol)
                                    {
                                        case 0:
                                            this.sound.PlaySE(SoundEffect.repair);
                                            this.savedata.HPNow += this.savedata.HPMax / 2;
                                            break;
                                        case 1:
                                            this.sound.PlaySE(SoundEffect.repair);
                                            this.savedata.HPNow = this.savedata.HPMax;
                                            break;
                                        case 2:
                                            this.savedata.runSubChips[0] = true;
                                            this.savedata.ValList[16] = 12000;
                                            break;
                                        case 3:
                                            this.savedata.runSubChips[1] = true;
                                            this.savedata.ValList[17] = 12000;
                                            break;
                                        case 4:
                                            this.savedata.runSubChips[2] = true;
                                            break;
                                        case 5:
                                            this.savedata.runSubChips[3] = true;
                                            this.savedata.ValList[18] = 12000;
                                            break;
                                        case 6:
                                            this.player.openmystery = true;
                                            this.nowscene = SubChip.SCENE.fadeout;
                                            break;
                                    }
                                    if (this.Cursol != 6)
                                    {
                                        --this.savedata.haveSubChis[this.cursol];
                                    }
                                    break;
                            }
                            this.question = false;
                            break;
                        }
                        this.Control();
                        if (this.frame % 10 == 0)
                            ++this.cursolanime;
                        if (this.cursolanime >= 3)
                            this.cursolanime = 0;
                        this.FlamePlus();
                        break;
                    case SubChip.SCENE.fadeout:
                        if (this.Alpha < byte.MaxValue)
                        {
                            this.Alpha += 51;
                            break;
                        }
                        this.topmenu.Return();
                        break;
                }
            }
        }

        private void Control()
        {
            if (Input.IsPress(Button._A))
            {
                if (!this.savedata.isJackedIn)
                {
                    this.eventmanager.events.Clear();
                    this.sound.PlaySE(SoundEffect.error);
                    this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                    var dialogue = ShanghaiEXE.Translate("SubChip.InvalidRealWorldDialogue1");
                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
                }
                else if (this.savedata.haveSubChis[this.cursol] == 0)
                {
                    this.eventmanager.events.Clear();
                    this.sound.PlaySE(SoundEffect.error);
                    this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                    var dialogue = ShanghaiEXE.Translate("SubChip.InvalidOutOfSubChipsDialogue1");
                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
                }
                else if (this.cursol < 2 && this.savedata.HPMax == this.savedata.HPnow)
                {
                    this.eventmanager.events.Clear();
                    this.sound.PlaySE(SoundEffect.error);
                    this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                    var dialogue = ShanghaiEXE.Translate("SubChip.InvalidFullHPDialogue1");
                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
                }
                else if (this.cursol >= 2 && this.cursol <= 5 && this.savedata.runSubChips[this.cursol - 2])
                {
                    this.eventmanager.events.Clear();
                    this.sound.PlaySE(SoundEffect.error);
                    this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                    var dialogue = ShanghaiEXE.Translate("SubChip.InvalidAlreadyInUseDialogue1");
                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
                }
                else if (this.cursol == 6 && !this.EventCheck())
                {
                    this.eventmanager.events.Clear();
                    this.sound.PlaySE(SoundEffect.error);
                    this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                    var dialogue = ShanghaiEXE.Translate("SubChip.InvalidNothingToUnlockDialogue1");
                    this.eventmanager.AddEvent(new CommandMessage(this.sound, this.eventmanager, dialogue[0], dialogue[1], dialogue[2], dialogue.Face, this.savedata));
                    this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
                }
                else
                {
                    this.eventmanager.events.Clear();
                    if (this.cursol == 6)
                    {
                        this.savedata.selectQuestion = 0;
                    }
                    else
                    {
                        this.eventmanager.AddEvent(new OpenMassageWindow(this.sound, this.eventmanager));
                        var questionDialogue = ShanghaiEXE.Translate("SubChip.UseQuestion");
                        var options = ShanghaiEXE.Translate("SubChip.UseOptions");
                        this.eventmanager.AddEvent(new Question(this.sound, this.eventmanager, questionDialogue[0], questionDialogue[1], options[0], options[1], false, true, FACE.Shanghai.ToFaceId(), this.savedata));
                        this.eventmanager.AddEvent(new CloseMassageWindow(this.sound, this.eventmanager));
                    }
                    this.question = true;
                }
            }
            if (Input.IsPress(Button._B))
            {
                this.sound.PlaySE(SoundEffect.cancel);
                this.nowscene = SubChip.SCENE.fadeout;
            }
            if (Input.IsPress(Button.Up))
            {
                this.sound.PlaySE(SoundEffect.movecursol);
                --this.Cursol;
                this.info = SubChip.InfoSet(this.Cursol);
            }
            if (!Input.IsPress(Button.Down))
                return;
            this.sound.PlaySE(SoundEffect.movecursol);
            ++this.Cursol;
            this.info = SubChip.InfoSet(this.Cursol);
        }

        private bool EventCheck()
        {
            this.player.eventhit = this.player.parent.CanMove_EventHit(MapCharacterBase.ANGLE.none);
            if (this.player.hitEvent > -1 && this.player.parent.Field.Events[this.player.hitEvent] is MysteryData)
            {
                MysteryData mysteryData = (MysteryData)this.player.parent.Field.Events[this.player.hitEvent];
                if (mysteryData.itemData.type == 2 && !this.savedata.GetMystery[mysteryData.itemData.flugNumber])
                    return true;
            }
            return false;
        }

        public static string[] InfoSet(int c)
        {
            var dialogue = new Dialogue();
            switch (c)
            {
                case 0:
                    dialogue = ShanghaiEXE.Translate("SubChip.HalfEnrgDescDialogue1");
                    break;
                case 1:
                    dialogue = ShanghaiEXE.Translate("SubChip.FullEnrgDescDialogue1");
                    break;
                case 2:
                    dialogue = ShanghaiEXE.Translate("SubChip.FireWallDescDialogue1");
                    break;
                case 3:
                    dialogue = ShanghaiEXE.Translate("SubChip.OpenPortDescDialogue1");
                    break;
                case 4:
                    dialogue = ShanghaiEXE.Translate("SubChip.Anti-VrsDescDialogue1");
                    break;
                case 5:
                    dialogue = ShanghaiEXE.Translate("SubChip.VirusScnDescDialogue1");
                    break;
                case 6:
                    dialogue = ShanghaiEXE.Translate("SubChip.CrakToolDescDialogue1");
                    break;
            }
            return new string[3] { dialogue[0], dialogue[1], dialogue[2] };
        }

        public static string NameSet(int c)
        {
            switch (c)
            {
                case 0:
                    return ShanghaiEXE.Translate("SubChip.HalfEnrg");
                case 1:
                    return ShanghaiEXE.Translate("SubChip.FullEnrg");
                case 2:
                    return ShanghaiEXE.Translate("SubChip.FireWall");
                case 3:
                    return ShanghaiEXE.Translate("SubChip.OpenPort");
                case 4:
                    return ShanghaiEXE.Translate("SubChip.Anti-Vrs");
                case 5:
                    return ShanghaiEXE.Translate("SubChip.VirusScn");
                case 6:
                    return ShanghaiEXE.Translate("SubChip.CrakTool");
            }
            return "";
        }

        public static int PriceSet(int c)
        {
            switch (c)
            {
                case 0:
                    return 500;
                case 1:
                    return 1000;
                case 2:
                    return 500;
                case 3:
                    return 50;
                case 4:
                    return 100;
                case 5:
                    return 10000;
                case 6:
                    return 5000;
                default:
                    return 0;
            }
        }

        private static string[] ActiveSubChipSprites = {
            "SubChip.UsedChipFirewall",
            "SubChip.UsedChipOpenPort",
            "SubChip.UsedChipAnti-Vrs",
            "SubChip.UsedChipVirusScn"
        };
        private Tuple<string, Rectangle> GetActiveSubChipSprite(int index)
        {
            return ShanghaiEXE.languageTranslationService.GetLocalizedSprite(ActiveSubChipSprites[index]);
        }

        public override void Render(IRenderer dg)
        {
            var subChipScreenSprite = ShanghaiEXE.languageTranslationService.GetLocalizedSprite("SubChip.FullScreen");
            this._rect = subChipScreenSprite.Item2;
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, subChipScreenSprite.Item1, this._rect, true, this._position, Color.White);
            for (int index = 0; index < 7; ++index)
            {
                this._rect = new Rectangle(8 * this.savedata.haveSubMemory, 0, 8, 16);
                this._position = new Vector2(104f, 24 + 16 * index);
                dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
                this._rect = new Rectangle(8 * this.savedata.haveSubChis[index], 0, 8, 16);
                this._position = new Vector2(88f, 24 + 16 * index);
                dg.DrawImage(dg, "font", this._rect, true, this._position, Color.Yellow);
            }
            for (int index = 0; index < this.savedata.runSubChips.Length; ++index)
            {
                if (this.savedata.runSubChips[index])
                {
                    var activeSubChipSprite = this.GetActiveSubChipSprite(index);
                    this._rect = activeSubChipSprite.Item2;
                    this._position = new Vector2(144f, 24 + 16 * index);
                    dg.DrawImage(dg, activeSubChipSprite.Item1, this._rect, true, this._position, Color.White);
                }
            }
            foreach (var data in ((IEnumerable<string>)this.info).Select((v, i) => new
            {
                v,
                i
            }))
            {
                string v = data.v;
                Vector2 point = new Vector2(130f, 102 + 16 * data.i);
                dg.DrawMiniText(v, point, Color.Black);
            }
            this._rect = new Rectangle(112 + 16 * this.cursolanime, 160, 16, 16);
            this._position = new Vector2(8f, 26 + 16 * this.cursol);
            dg.DrawImage(dg, "menuwindows", this._rect, true, this._position, Color.White);
            this._rect = new Rectangle(80, 0, 44, 16);
            this._position = new Vector2(24f, 8f);
            dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, Color.White);
            int[] numArray = this.ChangeCount(this.printHP);
            Color color1 = savedata.HPMax * 0.3 <= savedata.HPnow && this.savedata.HPnow >= this.printHP ? (this.savedata.HPnow <= this.printHP ? Color.White : Color.FromArgb(byte.MaxValue, 50, byte.MaxValue, 150)) : Color.FromArgb(byte.MaxValue, byte.MaxValue, 150, 50);
            for (int index = 0; index < numArray.Length; ++index)
            {
                this._rect = new Rectangle(numArray[index] * 8, 0, 8, 16);
                this._position = new Vector2(36 - index * 8, this._position.Y);
                dg.DrawImage(dg, "font", this._rect, false, this._position, color1);
            }
            if (this.eventmanager.playevent)
                this.eventmanager.Render(dg);
            if (this.nowscene != SubChip.SCENE.fadein && this.nowscene != SubChip.SCENE.fadeout)
                return;
            Color color2 = Color.FromArgb(this.Alpha, 0, 0, 0);
            this._rect = new Rectangle(0, 0, 240, 160);
            this._position = new Vector2(0.0f, 0.0f);
            dg.DrawImage(dg, "fadescreen", this._rect, true, this._position, color2);
        }

        private enum SCENE
        {
            fadein,
            select,
            fadeout,
        }
    }
}
