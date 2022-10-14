using NSAttack;
using NSCharge;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSGame;
using NSNet;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NSEnemy;
using System;

namespace NSBattle.Character
{
    public class Player : CharacterBase
    {
        private bool vusterFire = true;
        protected bool canMove = true;
        public bool printplayer = true;
        public List<ChipBase> haveChip = new List<ChipBase>();
        public Player.PLAYERMOTION motion = Player.PLAYERMOTION._neutral;
        public Player.ChargeShot chargeFix = Player.ChargeShot.none;
        public int moveWaitTime = 10;
        public int[] stylepoint = new int[6];
        public List<string> hitLog = new List<string>();
        protected bool canUseChip = true;
        public Player.BUSTOR bustor;
        public bool charge;
        protected bool chargeOff;
        public bool chargeMax;
        public int chargeTime;
        public Player.STYLE style;
        public bool synchroDamage;
        public byte damaged;
        public MindWindow mind;
        public byte manymove;
        public const byte maxChips = 6;
        public ChipBase usingChip;
        public int numOfChips;
        // Per-battle buster stats, permanent values in savedata.busterspec
        public byte busterPower;
        public byte busterRapid;
        public byte busterCharge;
        public bool needAbuutonPush;
        private Player.ChargeRL chargeRL;
        public Player.Rbutton Rbutton_;
        private ChargeBase RAction;
        public bool RbuttonUse;
        protected bool nomove;
        public Player.Lbutton Lbutton_;
        private ChargeBase LAction;
        public bool LbuttonUse;
        public int Rtimer;
        public int Ltimer;
        public const int moveTimeDefault = 10;
        public const int moveTimeShinobi = 6;
        private int chargeWait;
        private const int chargeWaitTime = 180;
        private ChargeBase chargeShot;
        public int setstyle;
        protected SaveData savedata;
        public bool chargeStock;
        private bool isUsedChip;
        public bool busterBlue;
        private int ringAnime;
        private int ringFlame;
        public bool[] addonSkill;
        private const int underwait = 10;
        private bool kishikaisei;
        private RockOnCursol rockonCursol;
        private RockOnTarget rockOnTarget;
        private Point NextPosi;
        private int nextchipwait;
        private int synkFlame;
        public int chipPain;

        public int mindNow
        {
            get
            {
                return (int)this.mind.MindNow;
            }
            set
            {
                this.mind.MindNow = (MindWindow.MIND)value;
            }
        }

        public int positionX
        {
            set
            {
                this.position.X = value;
                this.PositionDirectSet();
            }
        }

        private int RingAnime
        {
            get
            {
                return this.ringAnime;
            }
            set
            {
                this.ringAnime = value;
                if (this.ringAnime < 6)
                    return;
                this.ringAnime = 0;
            }
        }

        public override bool Badstatusresist
        {
            get
            {
                return this.badstatusresist || this.style == Player.STYLE.witch;
            }
            set
            {
                this.badstatusresist = value;
            }
        }

        public override bool Noslip
        {
            get
            {
                return this.noslip || this.style == Player.STYLE.wing;
            }
            set
            {
                this.noslip = value;
            }
        }

        public Player(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          SceneMain main,
          byte bp,
          byte br,
          byte bc,
          MindWindow m,
          SaveData save)
          : base(s, p)
        {
            this.number = 0;
            this.wide = 120;
            this.height = 120;
            this.savedata = save;
            this.setstyle = this.savedata.setstyle;
            for (int index = 0; index < this.stylepoint.Length; ++index)
                this.stylepoint[index] = 0;
            this.stylepoint[0] = 5;
            this.picturename = this.StyleGraphics(this.savedata.setstyle);
            this.busterPower = bp;
            this.busterRapid = br;
            this.busterCharge = bc;
            this.chargeWait = this.chargeShot.chargetime - busterCharge * this.chargeShot.shorttime;
            this.mind = m;
            this.hpmax = this.savedata.HPMax;
            this.hp = this.savedata.HPnow;
            this.hpprint = this.hp;
            this.position = new Point(pX, pY);
            this.positionre = this.position;
            this.animationpoint = new Point(0, 0);
            this.PositionDirectSet();
            this.union = Panel.COLOR.red;
            for (int index = 0; index < this.haveChip.Count; ++index)
                this.haveChip[index] = new ChipBase(this.sound);
            this.positionnow = this.position;
            this.moveWaitTime = this.style != Player.STYLE.shinobi ? 10 : 6;
            this.number = -1;
            this.addonSkill = this.savedata.addonSkill;
            this.alfha = byte.MaxValue;
            this.printhp = false;
            this.usingChip = new ChipBase(s);
            this.positionold = new Point(0, 1);

            this.chipPain = this.addonSkill[65] ? 30 : 0;
        }

        public void Setstyle(int sets)
        {
            this.picturename = this.StyleGraphics(sets);
            this.animationpoint = new Point(0, 0);
            for (int index = 0; index < this.badstatus.Length; ++index)
            {
                if (this.badstatustime[index] >= 0)
                {
                    this.badstatus[index] = false;
                    this.badstatustime[index] = 0;
                }
            }
            this.frame = 0;
            switch (this.motion)
            {
                case PLAYERMOTION._chip:
                case PLAYERMOTION._charge:
                    break;
                default:
                    this.motion = Player.PLAYERMOTION._neutral;
                    break;
            }
        }

        public void AddOn()
        {
            if (this.addonSkill[46])
            {
                for (int index = 0; index < 3; ++index)
                    this.parent.panel[this.union == Panel.COLOR.red ? 1 : 4, index].inviolability = true;
            }
            if (this.addonSkill[47])
            {
                Panel[,] panel1 = this.parent.panel;
                int upperBound1 = panel1.GetUpperBound(0);
                int upperBound2 = panel1.GetUpperBound(1);
                for (int lowerBound1 = panel1.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
                {
                    for (int lowerBound2 = panel1.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
                    {
                        Panel panel2 = panel1[lowerBound1, lowerBound2];
                        if (panel2.state != Panel.PANEL._none && panel2.state != Panel.PANEL._un)
                            panel2.state = Panel.PANEL._nomal;
                    }
                }
                foreach (ObjectBase objectBase in this.parent.objects)
                {
                    if (objectBase.StandPanel.color == this.union)
                    {
                        objectBase.flag = false;
                        objectBase.effecting = true;
                    }
                }
                if (this.Canmove(new Point(1, 1)))
                {
                    this.position = new Point(1, 1);
                    this.PositionDirectSet();
                }
            }
            if (this.addonSkill[58])
            {
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    for (int index2 = 0; index2 < 3; ++index2)
                    {
                        switch (index1)
                        {
                            case 0:
                                this.parent.panel[index1, index2].state = Panel.PANEL._un;
                                this.parent.panel[index1, index2].noRender = true;
                                break;
                            case 1:
                                this.parent.panel[index1, index2].inviolability = true;
                                if (this.parent.panel[index1, index2].Hole)
                                {
                                    this.parent.panel[index1, index2].state = Panel.PANEL._nomal;
                                }
                                break;
                            case 2:
                                this.parent.panel[index1, index2].inviolability = true;
                                this.parent.panel[index1, index2].color = Panel.COLOR.blue;
                                break;
                        }
                    }
                }
                foreach (ObjectBase objectBase in this.parent.objects)
                {
                    if (objectBase.StandPanel.color == this.union)
                        objectBase.flag = false;
                }
            }
            if (this.addonSkill[49])
                this.ghostDouble = true;
            if (this.addonSkill[53])
                this.chargeBypass = true;
            if (this.addonSkill[50])
                this.chargeStock = true;
            if (this.addonSkill[28])
                this.yuzuriai = true;
            if (this.addonSkill[54])
                this.ponkothu = true;
            if (this.addonSkill[55])
                this.blackMind = true;
            if (this.addonSkill[56])
                this.parizeDamage = true;
            if (this.addonSkill[63])
                this.nomove = true;
            if (this.addonSkill[48])
            {
                this.sound.PlaySE(SoundEffect.barrier);
                this.barrierType = CharacterBase.BARRIER.Barrier;
                this.barierTime = -1;
            }
            if (this.addonSkill[51])
                this.mind.anger = true;
            if (this.savedata.FlagList[742])
            {
                this.badstatus[1] = true;
                this.eturnalMelt = true;
                this.badstatustime[1] = -1;
            }
            if (this.addonSkill[40])
            {
                this.badstatus[1] = true;
                this.badstatustime[1] = -1;
            }
            if (this.addonSkill[41])
            {
                this.badstatus[2] = true;
                this.badstatustime[2] = -1;
            }
            if (this.addonSkill[42])
            {
                this.badstatus[4] = true;
                this.badstatustime[4] = -1;
            }
            if (this.addonSkill[43])
            {
                this.badstatus[6] = true;
                this.badstatustime[6] = -1;
            }
            if (this.addonSkill[44])
            {
                this.badstatus[5] = true;
                this.badstatustime[5] = -1;
            }
            if (this.addonSkill[0])
                this.bustor = Player.BUSTOR.assault;
            if (this.addonSkill[66])
                this.parent.CustomSpeedChange(1);
            this.busterBlue = this.addonSkill[38];
        }

        public void ChargeTimeSet()
        {
            this.chargeWait = this.chargeShot.chargetime - busterCharge * this.chargeShot.shorttime;
        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 44);
        }

        private string StyleGraphics(int sets)
        {
            string str = "";
            bool flag = false;
            switch (this.savedata.style[sets].style)
            {
                case 0:
                    str = "shanghai";
                    flag = true;
                    this.style = Player.STYLE.normal;
                    break;
                case 1:
                    str = "Fighter";
                    this.style = Player.STYLE.fighter;
                    break;
                case 2:
                    str = "Shinobi";
                    this.style = Player.STYLE.shinobi;
                    break;
                case 3:
                    str = "Doctor";
                    this.style = Player.STYLE.doctor;
                    break;
                case 4:
                    str = "Gaia";
                    this.style = Player.STYLE.gaia;
                    break;
                case 5:
                    str = "Wing";
                    this.style = Player.STYLE.wing;
                    break;
                case 6:
                    str = "Witch";
                    this.style = Player.STYLE.witch;
                    break;
            }
            this.StyleAbilitySet();
            if (flag)
            {
                this.element = ChipBase.ELEMENT.normal;
                return "shanghai";
            }
            switch (this.savedata.style[sets].element)
            {
                case 1:
                    str += "Heat";
                    this.element = ChipBase.ELEMENT.heat;
                    break;
                case 2:
                    str += "Aqua";
                    this.element = ChipBase.ELEMENT.aqua;
                    break;
                case 3:
                    str += "Eleki";
                    this.element = ChipBase.ELEMENT.eleki;
                    break;
                case 4:
                    str += "Leaf";
                    this.element = ChipBase.ELEMENT.leaf;
                    break;
                case 5:
                    str += "Poison";
                    this.element = ChipBase.ELEMENT.poison;
                    break;
                case 6:
                    str += "Earth";
                    this.element = ChipBase.ELEMENT.earth;
                    break;
            }
            return str;
        }

        public void StyleAbilitySet()
        {
            this.Flying = false;
            this.weakarmor = false;
            if (this.addonSkill == null)
                this.addonSkill = this.savedata.addonSkill;
            if (this.addonSkill[13])
                this.chargeFix = Player.ChargeShot.オーラソード;
            if (this.addonSkill[19])
                this.chargeFix = Player.ChargeShot.ショットガン;
            if (this.addonSkill[14])
                this.chargeFix = Player.ChargeShot.ダストボム;
            if (this.addonSkill[15])
                this.chargeFix = Player.ChargeShot.バルカン;
            if (this.addonSkill[16])
                this.chargeFix = Player.ChargeShot.フォールナイフ;
            if (this.addonSkill[17])
                this.chargeFix = Player.ChargeShot.ブラストカノン;
            if (this.addonSkill[18])
                this.chargeFix = Player.ChargeShot.ランス;
            if (this.addonSkill[20])
                this.chargeFix = Player.ChargeShot.リペア;
            if (this.addonSkill[21])
                this.Rbutton_ = Player.Rbutton.スターシールド;
            if (this.addonSkill[23])
                this.Rbutton_ = Player.Rbutton.パネルクラック;
            if (this.addonSkill[22])
                this.Rbutton_ = Player.Rbutton.ホールリペア;
            if (this.addonSkill[24])
                this.Lbutton_ = Player.Lbutton.ボロキューブ;
            if (this.addonSkill[25])
                this.Lbutton_ = Player.Lbutton.ムカイカゼ;
            if (this.addonSkill[26])
                this.Lbutton_ = Player.Lbutton.オイカゼ;
            if (this.addonSkill[27])
                this.Lbutton_ = Player.Lbutton.ロックオン;
            switch (this.style)
            {
                case Player.STYLE.normal:
                    this.chargeShot = new ChargeNomal(this.sound, this);
                    break;
                case Player.STYLE.fighter:
                    this.chargeShot = new ChargeFighter(this.sound, this);
                    break;
                case Player.STYLE.shinobi:
                    this.chargeShot = new ChargeShinobi(this.sound, this);
                    break;
                case Player.STYLE.doctor:
                    this.chargeShot = new ChargeDoctor(this.sound, this);
                    break;
                case Player.STYLE.gaia:
                    this.chargeShot = new ChargeGaia(this.sound, this);
                    break;
                case Player.STYLE.wing:
                    this.Flying = true;
                    this.chargeShot = new ChargeWing(this.sound, this);
                    break;
                case Player.STYLE.witch:
                    this.weakarmor = true;
                    this.chargeShot = new ChargeWitch(this.sound, this);
                    break;
            }
            switch (this.chargeFix)
            {
                case Player.ChargeShot.オーラソード:
                    this.chargeShot = new ChargeAuraSword(this.sound, this);
                    break;
                case Player.ChargeShot.ダストボム:
                    this.chargeShot = new ChargeDustBomb(this.sound, this);
                    break;
                case Player.ChargeShot.バルカン:
                    this.chargeShot = new ChargeVulcan(this.sound, this);
                    break;
                case Player.ChargeShot.フォールナイフ:
                    this.chargeShot = new ChargeFallKnife(this.sound, this);
                    break;
                case Player.ChargeShot.ブラストカノン:
                    this.chargeShot = new ChargeBlastCanon(this.sound, this);
                    break;
                case Player.ChargeShot.ランス:
                    this.chargeShot = new ChargeLance(this.sound, this);
                    break;
                case Player.ChargeShot.ショットガン:
                    this.chargeShot = new ChargeShotGun(this.sound, this);
                    break;
                case Player.ChargeShot.リペア:
                    this.chargeShot = new ChargeRepair(this.sound, this);
                    break;
            }
            switch (this.Rbutton_)
            {
                case Player.Rbutton.スターシールド:
                    this.RAction = new RShield(this.sound, this);
                    break;
                case Player.Rbutton.ホールリペア:
                    this.RAction = new RHoleRepair(this.sound, this);
                    break;
                case Player.Rbutton.パネルクラック:
                    this.RAction = new RHoleMake(this.sound, this);
                    break;
            }
            switch (this.Lbutton_)
            {
                case Player.Lbutton.ボロキューブ:
                    this.LAction = new LCube(this.sound, this);
                    break;
                case Player.Lbutton.ムカイカゼ:
                    this.LAction = new LMukaikaze(this.sound, this);
                    break;
                case Player.Lbutton.オイカゼ:
                    this.LAction = new LOikaze(this.sound, this);
                    break;
                case Player.Lbutton.ロックオン:
                    this.LAction = new LBeastRock(this.sound, this);
                    break;
            }
        }

        public string StyleGraphicsName(int sets)
        {
            string str = "";
            bool flag = false;
            switch (this.savedata.style[sets].style)
            {
                case 0:
                    str = "shanghai";
                    flag = true;
                    break;
                case 1:
                    str = "Fighter";
                    break;
                case 2:
                    str = "Shinobi";
                    break;
                case 3:
                    str = "Doctor";
                    break;
                case 4:
                    str = "Gaia";
                    break;
                case 5:
                    str = "Wing";
                    break;
                case 6:
                    str = "Witch";
                    break;
            }
            if (flag)
                return "shanghai";
            switch (this.savedata.style[sets].element)
            {
                case 1:
                    str += "Heat";
                    break;
                case 2:
                    str += "Aqua";
                    break;
                case 3:
                    str += "Eleki";
                    break;
                case 4:
                    str += "Leaf";
                    break;
                case 5:
                    str += "Poison";
                    break;
                case 6:
                    str += "Earth";
                    break;
            }
            return str;
        }

        public void PluspointFighter(int point)
        {
            this.stylepoint[0] += point;
        }

        public void PluspointShinobi(int point)
        {
            this.stylepoint[1] += point;
        }

        public void PluspointDoctor(int point)
        {
            this.stylepoint[2] += point;
        }

        public void PluspointGaia(int point)
        {
            this.stylepoint[3] += point;
        }

        public void PluspointWing(int point)
        {
            this.stylepoint[4] += point;
        }

        public void PluspointWitch(int point)
        {
            this.stylepoint[5] += point;
        }

        public override void Updata()
        {
            CustomGauge customgauge = this.parent.customgauge;
            this.synkFlame = 0;
            ++this.synkFlame;
            if (this.Hp <= 0 && this.addonSkill[70] && !this.kishikaisei)
            {
                this.hp = 1;
                this.kishikaisei = true;
            }
            if (this.Hp <= 0 && this.motion != Player.PLAYERMOTION._death)
            {
                if (this.parent.nowscene == SceneBattle.BATTLESCENE.end)
                {
                    this.sound.PlaySE(SoundEffect.counterhit);
                    this.hp = 1;
                }
                else
                {
                    this.parent.blackOutChips.RemoveAll(c => c.userNum == this.number);
                    this.sound.PlaySE(SoundEffect.death);
                    this.motion = Player.PLAYERMOTION._death;
                    this.chargeTime = 0;
                    this.chargeMax = false;
                    this.charge = false;
                    this.animationpoint = new Point(6, 2);
                    for (int index = 0; index < this.badstatus.Length; ++index)
                    {
                        this.badstatus[index] = false;
                        this.badstatustime[index] = 0;
                    }
                }
            }
            this.neutlal = this.motion == Player.PLAYERMOTION._neutral;
            if (this.numOfChips > 0)
                this.haveChip[0]?.HaveUpdate(this);
            if (this.badstatus[3] && this.motion != Player.PLAYERMOTION._damage)
            {
                if (this.slipping)
                    this.slipflame = 4;
                this.motion = Player.PLAYERMOTION._damage;
                this.animationpoint = new Point(5, 2);
                this.waittime = 0;
                this.chargeTime = 0;
                this.chargeMax = false;
                this.usingChip = new ChipBase(this.sound);
            }
            if (this.style == Player.STYLE.gaia)
                this.standbarrier = this.motion == Player.PLAYERMOTION._neutral;
            else
                this.standbarrier = false;
            if (this.rockonCursol == null)
            {
                this.rockonCursol = new RockOnCursol(this.sound, this.parent, this.position.X, this.position.Y, this);
                this.parent.effects.Add(rockonCursol);
            }
            if (this.rockOnTarget == null)
            {
                this.rockOnTarget = new RockOnTarget(this.sound, this.parent, this.position.X, this.position.Y, this);
                this.parent.effects.Add(rockOnTarget);
            }
            if (this.rockonMode && this.parent.nowscene != SceneBattle.BATTLESCENE.end && this.parent.nowscene != SceneBattle.BATTLESCENE.dead)
            {
                Point point = new Point(6, 1);
                this.rockon = false;
                foreach (CharacterBase characterBase in this.parent.AllChara())
                {
                    if (characterBase.position.X >= this.position.X && characterBase.position.Y == this.position.Y && (characterBase.position.X < point.X && !characterBase.nohit) && characterBase.union == this.UnionEnemy)
                    {
                        point = characterBase.position;
                        this.rockon = true;
                    }
                }
                if (this.rockon)
                {
                    this.rockonPosition = point;
                    this.rockOnTarget.position = this.rockonPosition;
                    if (!this.rockOnTarget.on)
                    {
                        this.rockOnTarget.on = true;
                        this.rockonCursol.on = false;
                        this.sound.PlaySE(SoundEffect.rockon);
                    }
                }
                else
                {
                    if (!this.rockonCursol.on)
                    {
                        this.rockOnTarget.on = false;
                        this.rockonCursol.on = true;
                    }
                    this.rockonPosition = new Point(-1, -1);
                }
            }
            else
            {
                this.rockOnTarget.on = false;
                this.rockonCursol.on = false;
                this.rockon = false;
            }
            if (this.slipping && this.motion == Player.PLAYERMOTION._neutral || this.knockslip)
            {
                this.Slip(68);
            }
            else
            {
                if (this.parent.nowscene != SceneBattle.BATTLESCENE.end && this.motion != Player.PLAYERMOTION._death && this.parent.manyenemys > 0)
                {
                    if (!this.LRControl(customgauge) && !this.badstatus[3])
                        this.Control(customgauge);
                    base.Updata();
                }
                if (this.charge && this.chargeTime == 30)
                    this.sound.PlaySE(SoundEffect.charge);
                if (!this.badstatus[3])
                    this.Moving();
            }
            if (this.mind.MindNow == MindWindow.MIND.fullsync)
                this.RingAnimation();
            if (this.Rtimer > 0)
                --this.Rtimer;
            if (this.Ltimer > 0)
                --this.Ltimer;
            if (!this.addonSkill[64])
                return;
            this.invincibilitytime = 0;
            this.invincibility = false;
            this.barierTime = 0;
            this.barrierType = CharacterBase.BARRIER.None;
        }

        public virtual bool LRControl(CustomGauge custom)
        {
            if (!custom.Customflag || this.parent.blackOut || !Input.IsPress(Button._L) && !Input.IsPress(Button._R))
                return false;
            this.ButtonLRCustom(custom);
            return true;
        }

        protected void ButtonLRCustom(CustomGauge custom)
        {
            if (this.savedata.addonSkill[45])
            {
                this.Hp -= this.Hp > 50 ? 50 : this.Hp - 1;
                this.sound.PlaySE(SoundEffect.damageplayer);
            }
            this.parent.custom.Init();
            this.parent.nowscene = SceneBattle.BATTLESCENE.custom;
        }

        protected void ButtonUp()
        {
            if (!this.Canmove(new Point(this.position.X, this.position.Y - 1)))
                return;
            this.motion = Player.PLAYERMOTION._move;
            this.positionre = new Point(this.position.X, this.position.Y - 1);
            this.waittime = 0;
            this.canMove = false;
        }

        protected void ButtonDown()
        {
            if (!this.Canmove(new Point(this.position.X, this.position.Y + 1)))
                return;
            this.motion = Player.PLAYERMOTION._move;
            this.positionre = new Point(this.position.X, this.position.Y + 1);
            this.waittime = 0;
            this.canMove = false;
        }

        protected void ButtonLeft()
        {
            if (!this.Canmove(new Point(this.position.X - this.UnionRebirth, this.position.Y)))
                return;
            this.motion = Player.PLAYERMOTION._move;
            this.positionre = new Point(this.position.X - this.UnionRebirth, this.position.Y);
            this.waittime = 0;
            this.canMove = false;
        }

        protected void ButtonRight()
        {
            if (!this.Canmove(new Point(this.position.X + this.UnionRebirth, this.position.Y)))
                return;
            this.motion = Player.PLAYERMOTION._move;
            this.positionre = new Point(this.position.X + this.UnionRebirth, this.position.Y);
            this.waittime = 0;
            this.canMove = false;
        }

        protected void ButtonA()
        {
            this.PositionDirectSet();
            this.needAbuutonPush = true;
            this.canUseChip = false;
            if (!this.isUsedChip)
            {
                this.isUsedChip = true;
                this.PluspointFighter(-5);
            }
            bool chargeMax = this.chargeMax;
            if (this.addonSkill[31])
            {
                this.sound.PlaySE(SoundEffect.repair);
                this.Hp += 30;
            }
            if (this.chipPain != 0)
            {
                this.sound.PlaySE(SoundEffect.damageplayer);
                this.Hp -= this.chipPain;
            }
            if (this.chargeBypass && this.chargeMax)
                this.haveChip[0].pluspower += 10;
            if (this.style == Player.STYLE.witch && (uint)this.haveChip[0].element > 0U)
            {
                if (this.haveChip[0].element == this.element)
                    this.haveChip[0].pluspower += 20;
                else
                    this.haveChip[0].pluspower += 10;
            }
            if (this.style == Player.STYLE.fighter && this.haveChip[0].element == ChipBase.ELEMENT.normal)
                this.haveChip[0].pluspower += 10;
            if (!this.chargeStock)
            {
                this.charge = false;
                this.chargeOff = false;
                this.chargeMax = false;
                this.chargeTime = 0;
            }
            this.usingChip = this.haveChip[0];
            this.usingChip.chargeFlag = chargeMax;
            string str = this.haveChip[0].power.ToString();
            if (this.haveChip[0].pluspower > 0)
                str = str + " +" + this.haveChip[0].pluspower.ToString();
            if (this.mind.MindNow == MindWindow.MIND.fullsync || this.mind.MindNow == MindWindow.MIND.angry)
                str += " *2";
            this.haveChip[0].powertxt = str;
            if (this.usingChip.powerprint && (this.mind.MindNow == MindWindow.MIND.fullsync || this.mind.MindNow == MindWindow.MIND.angry))
            {
                this.sound.PlaySE(SoundEffect.shoot);
                this.usingChip.power *= 2;
                this.usingChip.pluspower *= 2;
                this.mind.MindNow = MindWindow.MIND.normal;
            }
            else if (this.mind.MindNow == MindWindow.MIND.smile)
            {
                this.sound.PlaySE(SoundEffect.repair);
                this.Hp += (int)(HpMax * 0.3);
                this.mind.MindNow = MindWindow.MIND.normal;
                this.parent.effects.Add(new Repair(this.sound, this.parent, new Vector2((int)this.positionDirect.X - 14, (int)this.positionDirect.Y), this.speed, this.position));
            }
            else if (this.haveChip[0].dark)
                this.mind.mindNow = MindWindow.MIND.normal;
            this.LossChip();
            if (this.rockon)
            {
                this.rockonChipPosition = this.usingChip.rockOnPoint;
                this.RockonStep();
            }
            else if ((this.body == CharacterBase.BODY.Shadow || this.style == Player.STYLE.shinobi & chargeMax) && this.usingChip.shadow)
                this.Step();
            this.motion = Player.PLAYERMOTION._chip;
            this.waittime = 0;
            this.canMove = false;
            this.nextchipwait = 5;
        }

        protected void ButtonB()
        {
            this.charge = false;
            this.chargeOff = false;
            if (this.chargeMax)
            {
                bool flag = true;
                if (this.ponkothu && this.Random.Next(3) < 2)
                    flag = false;
                if (flag)
                {
                    this.PluspointFighter(5);
                    this.chargeTime = 0;
                    if (this.body == CharacterBase.BODY.Shadow && this.chargeShot.shadow)
                        this.Step();
                    this.motion = Player.PLAYERMOTION._charge;
                    this.chargeRL = Player.ChargeRL.Charge;
                    this.vusterFire = true;
                }
                else
                {
                    this.sound.PlaySE(SoundEffect.switchon);
                    this.vusterFire = false;
                    this.motion = Player.PLAYERMOTION._neutral;
                    this.charge = false;
                    this.chargeOff = false;
                    this.chargeMax = false;
                    this.chargeTime = 0;
                }
                this.chargeMax = false;
            }
            else
                this.motion = Player.PLAYERMOTION._buster;
            this.waittime = 0;
            this.canMove = false;
        }

        protected void ButtonL()
        {
            this.chargeRL = Player.ChargeRL.LAction;
            this.motion = Player.PLAYERMOTION._charge;
            if (!this.chargeStock)
            {
                this.charge = false;
                this.chargeOff = false;
                this.chargeMax = false;
                this.chargeTime = 0;
            }
            this.waittime = 0;
            this.canMove = false;
        }

        protected void ButtonR()
        {
            this.chargeRL = Player.ChargeRL.RAction;
            this.motion = Player.PLAYERMOTION._charge;
            if (!this.chargeStock)
            {
                this.charge = false;
                this.chargeOff = false;
                this.chargeMax = false;
                this.chargeTime = 0;
            }
            this.waittime = 0;
            this.canMove = false;
        }

        protected void ChargeStart()
        {
            this.chargeTime = 0;
            this.charge = true;
        }

        protected void ChargeEnd()
        {
            this.chargeOff = true;
        }

        public void BlackOutControl()
        {
            if (Input.IsPress(Button._A)
                && !this.parent.blackOutStopper
                && this.numOfChips > 0
                && this.parent.nowscene != SceneBattle.BATTLESCENE.custom
                && this.haveChip[0]?.timeStopper == true
                && this.parent.blackOutChips[0].nameAlpha == byte.MaxValue)
            {
                this.ButtonA();
                this.usingChip.chipUseEnd = false;
                this.usingChip.BlackOut(this, this.parent, this.usingChip.name, this.usingChip.Power(this).ToString());
            }
        }

        public virtual void Control(CustomGauge custom)
        {
            if (!this.canUseChip && !Input.IsPush(Button._A))
                this.canUseChip = true;
            if (this.canMove && this.motion != Player.PLAYERMOTION._damage && (this.motion != Player.PLAYERMOTION._chip && this.motion != Player.PLAYERMOTION._charge) && !this.badstatus[7] && !this.badstatus[8])
            {
                if (Input.IsPush(Button.Up) && !this.nomove)
                    this.ButtonUp();
                else if (Input.IsPush(Button.Down) && !this.nomove)
                    this.ButtonDown();
                else if (Input.IsPush(Button.Left) && !this.nomove)
                    this.ButtonLeft();
                else if (Input.IsPush(Button.Right) && !this.nomove)
                    this.ButtonRight();
            }
            if (this.motion == Player.PLAYERMOTION._neutral)
            {
                this.canMove = true;
                if (!Input.IsPush(Button._A))
                    this.needAbuutonPush = false;
                if (Input.IsPush(Button._A) && !this.needAbuutonPush && (this.numOfChips > 0 && this.canUseChip && this.haveChip[0] != null) && this.parent.nowscene != SceneBattle.BATTLESCENE.custom && this.nextchipwait == 0)
                    this.ButtonA();
                else if (this.charge && !Input.IsPush(Button._B) && !this.addonSkill[69] || (this.chargeOff || Input.IsPush(Button._B) && this.bustor != Player.BUSTOR.normal) || this.addonSkill[69] && Input.IsPush(Button._B))
                    this.ButtonB();
                else if (Input.IsPress(Button._R) && !custom.Customflag && (this.parent.nowscene != SceneBattle.BATTLESCENE.custom && this.Rbutton_ != Player.Rbutton.none) && !this.RbuttonUse && this.Rtimer <= 0)
                    this.ButtonR();
                else if (Input.IsPress(Button._L) && !custom.Customflag && (this.parent.nowscene != SceneBattle.BATTLESCENE.custom && this.Lbutton_ != Player.Lbutton.none) && !this.LbuttonUse && this.Ltimer <= 0)
                    this.ButtonL();
                if (this.motion != Player.PLAYERMOTION._chip)
                {
                    if (this.addonSkill[69] && !this.charge)
                        this.ChargeStart();
                    if (!this.charge && Input.IsPush(Button._B) && this.bustor == Player.BUSTOR.normal)
                        this.ChargeStart();
                    if (this.charge && Input.IsUp(Button._B))
                        this.ChargeEnd();
                }
            }
            if (this.nextchipwait <= 0)
                return;
            --this.nextchipwait;
        }

        public void LossChip()
        {
            if (this.numOfChips == 0)
            {
                return;
            }

            --this.numOfChips;
            for (int index = 0; index < this.numOfChips; ++index)
                this.haveChip[index] = this.haveChip[index + 1];
            this.haveChip[this.numOfChips] = null;
        }

        private void Moving()
        {
            switch (this.motion)
            {
                case Player.PLAYERMOTION._neutral:
                    if (this.waittime == 1)
                    {
                        this.animationpoint = new Point(0, 0);
                        break;
                    }
                    break;
                case Player.PLAYERMOTION._move:
                    if (this.waittime == 3)
                    {
                        this.position = this.positionre;
                        this.PositionDirectSet();
                        if (this.manymove < 3)
                            ++this.manymove;
                    }
                    if (this.waittime == this.moveWaitTime)
                    {
                        this.PositionDirectSet();
                        this.motion = Player.PLAYERMOTION._neutral;
                    }
                    if (this.moveWaitTime == 6)
                    {
                        this.animationpoint = CharacterAnimation.MoveAnimationS(this.waittime);
                        break;
                    }
                    this.animationpoint = CharacterAnimation.MoveAnimation(this.waittime);
                    break;
                case Player.PLAYERMOTION._buster:
                    if (this.bustor == Player.BUSTOR.assault)
                    {
                        this.chargeTime = 0;
                        bool flag = true;
                        if (this.ponkothu && this.Random.Next(3) < 2)
                            flag = false;
                        this.animationpoint = CharacterAnimation.BusterAnimation(this.waittime);
                        int num1 = this.busterRapid >= 3 ? busterRapid + 2 : busterRapid;
                        if (this.waittime == 10 - num1)
                        {
                            if (flag)
                            {
                                this.vusterFire = true;
                                this.sound.PlaySE(SoundEffect.vulcan);
                                break;
                            }
                            this.vusterFire = false;
                            this.sound.PlaySE(SoundEffect.switchon);
                            break;
                        }
                        if (this.waittime == 12 - num1)
                        {
                            int num2 = this.style != Player.STYLE.fighter ? busterPower : busterPower * 2;
                            this.PluspointFighter(1);
                            if (flag)
                            {
                                if (this.addonSkill[36])
                                    this.parent.effects.Add(new BulletShells(this.sound, this.parent, this.position, this.positionDirect.X + 20 * this.UnionRebirth, this.positionDirect.Y + 12f, 26, this.union, 20 + this.Random.Next(20), 2, 0));
                                if (this.addonSkill[37])
                                    this.parent.effects.Add(new BulletBigShells(this.sound, this.parent, this.position, this.positionDirect.X + 20 * this.UnionRebirth, this.positionDirect.Y + 12f, 26, this.union, 20 + this.Random.Next(20), 2, 0));
                                this.parent.attacks.Add(new AssaultBuster(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, !this.badstatus[1] ? num2 : Math.Max(1, num2 / 2), busterPower / 2, busterCharge, this.busterBlue, ChipBase.ELEMENT.normal));
                            }
                            if (Input.IsPush(Button._B) && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.waittime = 0;
                                break;
                            }
                            this.canMove = true;
                            break;
                        }
                        if (this.waittime == 14 - num1)
                        {
                            this.motion = Player.PLAYERMOTION._neutral;
                            this.animationpoint = new Point();
                            this.canMove = true;
                            break;
                        }
                        break;
                    }
                    bool flag1 = true;
                    if (this.ponkothu && this.Random.Next(3) < 2)
                        flag1 = false;
                    this.animationpoint = CharacterAnimation.BusterAnimation(this.waittime);
                    switch (this.waittime)
                    {
                        case 4:
                            if (flag1)
                            {
                                this.vusterFire = true;
                                if (this.addonSkill[36])
                                    this.parent.effects.Add(new BulletShells(this.sound, this.parent, this.position, this.positionDirect.X + 20 * this.UnionRebirth, this.positionDirect.Y + 12f, 26, this.union, 20 + this.Random.Next(20), 2, 0));
                                if (this.addonSkill[37])
                                    this.parent.effects.Add(new BulletBigShells(this.sound, this.parent, this.position, this.positionDirect.X + 20 * this.UnionRebirth, this.positionDirect.Y + 12f, 26, this.union, 20 + this.Random.Next(20), 2, 0));
                                this.sound.PlaySE(SoundEffect.buster);
                                break;
                            }
                            this.vusterFire = false;
                            this.sound.PlaySE(SoundEffect.switchon);
                            break;
                        case 6:
                            if (flag1)
                            {
                                int num = this.style != Player.STYLE.fighter ? busterPower : busterPower * 2;
                                this.PluspointFighter(2);
                                this.parent.attacks.Add(new BustorShot(this.sound, this.parent, this.position.X, this.position.Y, this.union, !this.badstatus[1] ? num : Math.Max(1, num / 2), BustorShot.SHOT.bustor, ChipBase.ELEMENT.normal, false, 0));
                            }
                            this.canMove = true;
                            break;
                        case 9:
                            this.canMove = true;
                            break;
                    }
                    if (this.waittime == 35 - busterRapid * 5)
                    {
                        this.motion = Player.PLAYERMOTION._neutral;
                        this.waittime = 0;
                    }
                    break;
                case Player.PLAYERMOTION._charge:
                    switch (this.chargeRL)
                    {
                        case Player.ChargeRL.RAction:
                            this.RAction.Action();
                            break;
                        case Player.ChargeRL.LAction:
                            this.LAction.Action();
                            break;
                        default:
                            this.chargeShot.Action();
                            break;
                    }
                    this.canMove = true;
                    break;
                case Player.PLAYERMOTION._damage:
                    switch (this.waittime)
                    {
                        case 1:
                            this.PositionDirectSet();
                            break;
                        case 2:
                            this.animationpoint = new Point(5, 2);
                            break;
                        case 15:
                            this.animationpoint = new Point(4, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.motion = Player.PLAYERMOTION._neutral;
                            break;
                    }
                    if (this.waittime >= 2 && this.waittime <= 6)
                        this.positionDirect.X -= UnionRebirth;
                    this.canMove = true;
                    break;
                case Player.PLAYERMOTION._chip:
                    // Short-circuiting, only evaluate BlackOut if timeStopper true
                    if (!this.usingChip.timeStopper || this.usingChip.BlackOut(this, this.parent, this.usingChip.name, this.usingChip.Power(this).ToString()))
                    {
                        if (!this.usingChip.chipUseEnd)
                            this.usingChip.Action(this, this.parent);
                        if (this.usingChip.chipUseEnd)
                            this.usingChip.ActionEnd(this, this.parent);
                    }
                    this.canMove = true;
                    break;
                case Player.PLAYERMOTION._death:
                    if (this.alfha > 0)
                    {
                        this.alfha -= 5;
                        break;
                    }
                    this.alfha = 0;
                        this.parent.nowscene = SceneBattle.BATTLESCENE.dead;
                    break;
            }
            if (this.waittime < ushort.MaxValue)
                ++this.waittime;
            if (!this.parent.blackOut)
            {
                if (this.charge && this.chargeTime < this.chargeWait)
                    ++this.chargeTime;
                if (this.chargeTime >= this.chargeWait && !this.chargeMax)
                {
                    this.chargeMax = true;
                    this.sound.PlaySE(SoundEffect.chargemax);
                }
            }
            this.MoveAftar();
        }

        public void Dameged(bool paralyze)
        {
            this.PluspointGaia(5);
            this.canMove = false;
            bool flag = false;
            if (((this.style != Player.STYLE.gaia ? 1 : (this.guard == CharacterBase.GUARD.armar ? 1 : 0)) | (paralyze ? 1 : 0)) != 0)
            {
                ++this.damaged;
                if (this.slipping)
                    this.slipflame = 4;
                this.motion = Player.PLAYERMOTION._damage;
                if (this.parizeDamage)
                {
                    this.badstatus[3] = true;
                    this.badstatustime[3] = 60;
                    flag = true;
                    this.invincibilitytime = 0;
                }
                this.animationpoint = new Point(5, 2);
                this.waittime = 0;
                this.chargeTime = 0;
                this.chargeMax = false;
                this.usingChip = new ChipBase(this.sound);
            }
            if (this.badstatus[3] && !flag)
            {
                this.badstatus[3] = false;
                this.badstatustime[3] = 0;
            }
            if (this.mind.MindNow != MindWindow.MIND.pinch)
            {
                this.mind.MindNow = Hp < hpmax * 0.3 ? MindWindow.MIND.pinch : MindWindow.MIND.normal;
                if (this.mind.MindNow == MindWindow.MIND.pinch && this.mind.anger)
                    this.mind.MindNow = MindWindow.MIND.angry;
            }
            if (damaged % 6 != 5)
                return;
            this.mind.MindNow = MindWindow.MIND.pinch;
            if (this.mind.MindNow == MindWindow.MIND.pinch && this.mind.anger)
                this.mind.MindNow = MindWindow.MIND.angry;
        }

        private bool moveanswer(Point poti, Point next)
        {
            int index1 = poti.X + next.X;
            int index2 = poti.Y + next.Y;
            return index1 >= 0 && index1 < 6 && index2 >= 0 && index2 < 3 && this.parent.panel[index1, index2].color == Panel.COLOR.red;
        }

        private byte ChargeEffect()
        {
            return this.chargeTime < this.chargeWait ? (byte)0 : (byte)1;
        }

        private void RingAnimation()
        {
            if (this.ringFlame % 4 == 0)
                ++this.RingAnime;
            ++this.ringFlame;
        }

        public override void Render(IRenderer dg)
        {
            if (this.printplayer)
            {
                this._rect = new Rectangle(this.animationpoint.X * this.Wide, this.animationpoint.Y * this.Height, this.Wide, this.Height);
                int x1 = (int)this.positionDirect.X;
                Point shake1 = this.Shake;
                int x2 = shake1.X;
                double num1 = x1 + x2;
                int y1 = (int)this.positionDirect.Y;
                shake1 = this.Shake;
                int y2 = shake1.Y;
                double num2 = y1 + y2;
                this._position = new Vector2((float)num1, (float)num2);
                if (this.motion != Player.PLAYERMOTION._death)
                {
                    if (this.alfha < byte.MaxValue && !this.parent.blackOut)
                        this.color = Color.FromArgb(alfha, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                    else
                        this.color = this.mastorcolor;
                    if (this.invincibility && !this.parent.blackOut)
                        this.color = Color.FromArgb(sbyte.MaxValue * (this.invincibilitytime % 3), color.R, color.G, color.B);
                }
                else
                {
                    this.color = Color.FromArgb(alfha, Color.White);
                    if (this.parent.blackOut)
                        this.color = Color.FromArgb(byte.MaxValue, Color.White);
                }
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                Point shake2;
                if (this.mind.MindNow == MindWindow.MIND.fullsync)
                {
                    if (!this.mind.perfect)
                    {
                        this._rect = new Rectangle(this.animationpoint.X * this.Wide, this.animationpoint.Y * this.Height + 840, this.Wide, this.Height);
                        int x3 = (int)this.positionDirect.X;
                        Point shake3 = this.Shake;
                        int x4 = shake3.X;
                        double num3 = x3 + x4;
                        int y3 = (int)this.positionDirect.Y;
                        shake3 = this.Shake;
                        int y4 = shake3.Y;
                        double num4 = y3 + y4;
                        this._position = new Vector2((float)num3, (float)num4);
                        this.color = Color.FromArgb(60, Color.White);
                        dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                    }
                    this._rect = new Rectangle(this.RingAnime * this.Wide, 2 * this.Height, this.Wide, this.Height);
                    int num5 = (int)this.positionDirect.X + 2;
                    shake2 = this.Shake;
                    int x5 = shake2.X;
                    double num6 = num5 + x5;
                    int y5 = (int)this.positionDirect.Y;
                    shake2 = this.Shake;
                    int y6 = shake2.Y;
                    double num7 = y5 + y6;
                    this._position = new Vector2((float)num6, (float)num7);
                    dg.DrawImage(dg, "Silhouette", this._rect, false, this._position, this.rebirth, Color.White);
                    this.color = Color.FromArgb(alfha, Color.White);
                }
                else if (this.mind.MindNow == MindWindow.MIND.angry)
                {
                    this._rect = new Rectangle(this.animationpoint.X * this.Wide, this.animationpoint.Y * this.Height + 840, this.Wide, this.Height);
                    int x3 = (int)this.positionDirect.X;
                    Point shake3 = this.Shake;
                    int x4 = shake3.X;
                    double num3 = x3 + x4;
                    int y3 = (int)this.positionDirect.Y;
                    shake3 = this.Shake;
                    int y4 = shake3.Y;
                    double num4 = y3 + y4;
                    this._position = new Vector2((float)num3, (float)num4);
                    this.color = Color.FromArgb(60, Color.Red);
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                }
                else if (this.mind.MindNow == MindWindow.MIND.dark)
                {
                    this._rect = new Rectangle(this.animationpoint.X * this.Wide, this.animationpoint.Y * this.Height + 840, this.Wide, this.Height);
                    int x3 = (int)this.positionDirect.X;
                    Point shake3 = this.Shake;
                    int x4 = shake3.X;
                    double num3 = x3 + x4;
                    int y3 = (int)this.positionDirect.Y;
                    shake3 = this.Shake;
                    int y4 = shake3.Y;
                    double num4 = y3 + y4;
                    this._position = new Vector2((float)num3, (float)num4);
                    this.color = Color.FromArgb(120, Color.FromArgb(63, 0, 143));
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                }
                this.color = Color.White;
                if ((this.motion == Player.PLAYERMOTION._buster || this.motion == Player.PLAYERMOTION._neutral) && (this.animationpoint == new Point(5, 0) || this.animationpoint == new Point(6, 0)))
                {
                    Point point = new Point(this.animationpoint.X, 4);
                    this._rect = new Rectangle(point.X * this.Wide, point.Y * this.Wide, this.Wide, this.Height);
                    int x3 = (int)this.positionDirect.X;
                    shake2 = this.Shake;
                    int x4 = shake2.X;
                    double num3 = x3 + x4;
                    int y3 = (int)this.positionDirect.Y;
                    shake2 = this.Shake;
                    int y4 = shake2.Y;
                    double num4 = y3 + y4;
                    this._position = new Vector2((float)num3, (float)num4);
                    dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
                    if (this.vusterFire && (this.waittime >= 2 && this.waittime < 10))
                    {
                        int num5 = this.busterBlue ? 96 : 0;
                        if (this.bustor == Player.BUSTOR.normal)
                            this._rect = new Rectangle((this.waittime - 2) / 3 * 32 + num5, 0, 32, 16);
                        else if (this.bustor == Player.BUSTOR.assault)
                            this._rect = new Rectangle((this.waittime - 2) / 3 * 32 + num5, 16, 32, 16);
                        int num6 = (int)this.positionDirect.X + 42 * this.UnionRebirth;
                        shake2 = this.Shake;
                        int x5 = shake2.X;
                        double num7 = num6 + x5;
                        int num8 = (int)this.positionDirect.Y + 14;
                        shake2 = this.Shake;
                        int y5 = shake2.Y;
                        double num9 = num8 + y5;
                        this._position = new Vector2((float)num7, (float)num9);
                        dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                    }
                }
                if (this.charge && this.chargeTime > 30 && this.parent.manyenemys > 0)
                {
                    this._rect = new Rectangle(this.mastorflame % 16 / 2 * 64, this.ChargeEffect() * 64, 64, 64);
                    int x3 = (int)this.positionDirect.X;
                    shake2 = this.Shake;
                    int x4 = shake2.X;
                    double num3 = x3 + x4;
                    int num4 = (int)this.positionDirect.Y + 16;
                    shake2 = this.Shake;
                    int y3 = shake2.Y;
                    double num5 = num4 + y3;
                    this._position = new Vector2((float)num3, (float)num5);
                    dg.DrawImage(dg, "charge", this._rect, false, this._position, this.rebirth, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                }
                if (this.motion == Player.PLAYERMOTION._charge)
                {
                    switch (this.chargeRL)
                    {
                        case Player.ChargeRL.RAction:
                            this.RAction.Render(dg, this.positionDirect, this.picturename);
                            break;
                        case Player.ChargeRL.LAction:
                            this.LAction.Render(dg, this.positionDirect, this.picturename);
                            break;
                        default:
                            this.chargeShot.Render(dg, this.positionDirect, this.picturename);
                            break;
                    }
                }
            }
            if (this.usingChip.chipUseEnd || !this.usingChip.blackOutLend)
                return;
            this.usingChip.Render(dg, this);
        }

        public override void RenderUP(IRenderer dg)
        {
            
            if (this.numOfChips > 0 && !this.parent.blackOut)
            {
                for (int index = this.haveChip.Count - 1; index >= 0; --index)
                {
                    if (this.haveChip[index] != null && this.haveChip[index].printIcon)
                    {
                        this._position = new Vector2(this.position.X * 40 + 16 - 2 * index + this.Shake.X, this.position.Y * 24 + 40 - 16 - 2 * index + this.Shake.Y);
                        this._rect = new Rectangle(296, 104, 16, 16);
                        dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, Color.White);
                        this.haveChip[index].IconRender(dg, this._position, false, false, 0, false);
                    }
                }
                if (this.haveChip.Count > 0 && this.haveChip[0]?.name != null)
                {
                    int length = this.haveChip[0].name.Length;
                    this._position = new Vector2(8f, 144f);
                    this.TextRender(dg, this.haveChip[0].name, false, this._position, true);
                    if (this.haveChip[0].powerprint)
                    {
                        string txt = this.haveChip[0].power.ToString();
                        int pluspower = this.haveChip[0].pluspower;
                        if (this.chargeBypass && this.chargeMax)
                            pluspower += 10;
                        if (this.style == Player.STYLE.witch && (uint)this.haveChip[0].element > 0U)
                        {
                            if (this.haveChip[0].element == this.element)
                                pluspower += 20;
                            else
                                pluspower += 10;
                        }
                        if (this.style == Player.STYLE.fighter && this.haveChip[0].element == ChipBase.ELEMENT.normal)
                            pluspower += 10;
                        if (pluspower > 0)
                            txt = txt + " +" + pluspower.ToString();
                        if (this.mind.MindNow == MindWindow.MIND.fullsync || this.mind.MindNow == MindWindow.MIND.angry)
                            txt += " *2";
                        this._position = new Vector2(24 + length * 8, 144f);
                        this.TextRender(dg, txt, false, this._position, true, Color.FromArgb(byte.MaxValue, 230, 30));
                    }
                }
            }
            this._rect = new Rectangle(80, 0, 44, 16);
            this._position = this.parent.positionHPwindow;
            dg.DrawImage(dg, "battleobjects", this._rect, false, this._position, Color.White);
            this.HPRender(dg, new Vector2(this.parent.positionHPwindow.X + 12f, this.parent.positionHPwindow.Y - 1f));
            
            if (this.usingChip?.chipUseEnd == true)
                return;
            this.usingChip?.BlackOutRender(dg, this.union);
        }

        public override void BarrierRend(IRenderer dg)
        {
            this.BarrierRender(dg, new Vector2(this.positionDirect.X, this.positionDirect.Y + 24f));
        }

        public override void BarrierPowerRend(IRenderer dg)
        {
            this.BarrierPowerRender(dg, new Vector2(this.positionDirect.X, this.positionDirect.Y + 24f));
        }

        public enum STYLE
        {
            normal,
            fighter,
            shinobi,
            doctor,
            gaia,
            wing,
            witch,
        }

        public enum BUSTOR
        {
            normal,
            assault,
        }

        public enum PLAYERMOTION
        {
            _neutral,
            _wait,
            _move,
            _buster,
            _charge,
            _damage,
            _chip,
            _death,
        }

        public enum ChargeRL
        {
            Charge,
            RAction,
            LAction,
        }

        public enum ChargeShot
        {
            none,
            // AuraSwd
            オーラソード,
            // DustBomb
            ダストボム,
            // Vulcan
            バルカン,
            // FallKnife
            フォールナイフ,
            // BlastCannon
            ブラストカノン,
            // Lance
            ランス,
            // Shotgun
            ショットガン,
            // Repair
            リペア,
        }

        public enum Rbutton
        {
            none,
            // StarShield
            スターシールド,
            // Hole Fix
            ホールリペア,
            // PnlCrak
            パネルクラック,
        }

        public enum Lbutton
        {
            none,
            // Cube
            ボロキューブ,
            // HeadWind
            ムカイカゼ,
            // TailWind
            オイカゼ,
            // Lock-on
            ロックオン,
        }
    }
}
