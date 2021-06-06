using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSChip
{
    internal class MasterStyle : ChipBase
    {
        private string[] texname = new string[7]
        {
      "FighterLeaf",
      "ShinobiEleki",
      "WingPoison",
      "GaiaEarth",
      "DoctorHeat",
      "WitchAqua",
      ""
        };
        private Vector2[] tryPosition = new Vector2[3];
        private Point[] attackPosition = new Point[6];
        private double angle = -270.0;
        private const int speed = 2;
        private MasterStyle.MOTION motion;
        private Point startPosition;
        private Vector2 attackPositionD;
        private bool attackRevers;
        private int atackEndFlame;
        private int subAnime;
        private int r;
        private Point animePoint;
        private BombAttack attack;
        private AttackBase effectattack;

        public MasterStyle(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 302;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.MasterStyleName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 100;
            this.subpower = 0;
            this.regsize = 23;
            this.reality = 3;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.MasterStyleDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        private Point Animation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[21]
            {
        1,
        2,
        2,
        4,
        12,
        4,
        4,
        4,
        4,
        4,
        74,
        54,
        this.power / 2,
        24,
        4,
        4,
        4,
        4,
        2,
        2,
        4
            }, new int[22]
            {
        -1,
        3,
        2,
        1,
        0,
        4,
        5,
        6,
        7,
        10,
        11,
        12,
        13,
        12,
        5,
        6,
        4,
        0,
        1,
        2,
        3,
        -1
            }, 0, waittime);
        }

        public void PositionDirectSet(int x, int y)
        {
            this.attackPositionD = new Vector2(x * 40 + 20, y * 24 + 44);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            switch (this.motion)
            {
                case MasterStyle.MOTION._0_setup:
                    switch (character.waittime)
                    {
                        case 2:
                            character.animationpoint = new Point(1, 0);
                            break;
                        case 4:
                            character.animationpoint = new Point(2, 0);
                            break;
                        case 6:
                            character.animationpoint = new Point(3, 0);
                            break;
                        case 7:
                            character.animationpoint = new Point(99, 0);
                            break;
                        case 10:
                            this.startPosition = character.position;
                            character.animationpoint = new Point(-1, 0);
                            character.waittime = 0;
                            this.motion = MasterStyle.MOTION._1_start;
                            for (int index = 0; index < this.tryPosition.Length; ++index)
                                this.tryPosition[index] = character.positionDirect;
                            break;
                    }
                    break;
                case MasterStyle.MOTION._1_start:
                    switch (character.waittime)
                    {
                        case 1:
                            this.sound.PlaySE(SoundEffect.pikin);
                            break;
                        case 30:
                            this.sound.PlaySE(SoundEffect.dark);
                            break;
                        case 70:
                            int index1 = 0;
                            while (index1 < 3)
                            {
                                foreach (CharacterBase characterBase in battle.AllChara())
                                {
                                    if (!(characterBase is DammyEnemy) && characterBase.union == character.UnionEnemy)
                                    {
                                        if (index1 + 3 < this.attackPosition.Length)
                                        {
                                            this.attackPosition[index1] = characterBase.position;
                                            this.attackPosition[index1 + 3] = characterBase.position;
                                            ++index1;
                                        }
                                        else
                                            break;
                                    }
                                }
                            }
                            this.NextMotion(-1, 0, character);
                            this.atackEndFlame = 28;
                            break;
                    }
                    if (this.r < 24 && character.waittime > 30)
                    {
                        ++this.r;
                        this.angle += 0.05;
                        for (int index2 = 0; index2 < this.tryPosition.Length; ++index2)
                        {
                            this.tryPosition[index2] = character.positionDirect;
                            this.tryPosition[index2].X += r * (float)Math.Cos(this.angle - 360 * index2);
                            this.tryPosition[index2].Y -= r * (float)Math.Sin(this.angle - 360 * index2);
                        }
                        break;
                    }
                    break;
                case MasterStyle.MOTION._2_LeafFighter:
                    switch (character.waittime)
                    {
                        case 2:
                            this.animePoint.Y = 0;
                            this.animePoint.X = 3;
                            break;
                        case 4:
                            this.animePoint.X = 2;
                            break;
                        case 6:
                            this.animePoint.X = 1;
                            break;
                        case 8:
                            this.animePoint.X = 0;
                            break;
                        case 10:
                            this.animePoint.Y = 3;
                            break;
                        case 12:
                            this.animePoint.X = 1;
                            break;
                        case 14:
                            this.animePoint.X = 2;
                            break;
                        case 16:
                            this.animePoint.X = 3;
                            this.sound.PlaySE(SoundEffect.lance);
                            this.attack = new BombAttack(this.sound, battle, this.attackPosition[0].X, this.attackPosition[0].Y, character.union, this.Power(character), 1, ChipBase.ELEMENT.normal);
                            this.attack.element = ChipBase.ELEMENT.leaf;
                            this.attack.hitrange.X = 1;
                            this.attack.breaking = true;
                            this.attack.rebirth = this.attackRevers;
                            battle.attacks.Add(attack);
                            break;
                    }
                    if (character.waittime >= this.atackEndFlame)
                    {
                        if (character.waittime == this.atackEndFlame)
                        {
                            this.animePoint.Y = 0;
                            this.animePoint.X = 0;
                        }
                        else if (character.waittime == this.atackEndFlame + 2)
                            this.animePoint.X = 1;
                        else if (character.waittime == this.atackEndFlame + 4)
                            this.animePoint.X = 2;
                        else if (character.waittime == this.atackEndFlame + 6)
                            this.animePoint.X = 3;
                        else if (character.waittime == this.atackEndFlame + 8)
                            this.animePoint.X = -1;
                        else if (character.waittime == this.atackEndFlame + 10)
                        {
                            this.NextMotion(1, 1, character);
                            this.atackEndFlame = 26;
                        }
                        break;
                    }
                    break;
                case MasterStyle.MOTION._3_ElekiShinobi:
                    switch (character.waittime)
                    {
                        case 2:
                            this.animePoint.Y = 0;
                            this.animePoint.X = 3;
                            break;
                        case 4:
                            this.animePoint.X = 2;
                            break;
                        case 6:
                            this.animePoint.X = 1;
                            break;
                        case 8:
                            this.animePoint.X = 0;
                            break;
                        case 10:
                            this.animePoint.Y = 1;
                            break;
                        case 12:
                            this.animePoint.X = 1;
                            this.sound.PlaySE(SoundEffect.sword);
                            this.effectattack = new SwordAttack(this.sound, battle, this.attackPosition[1].X, this.attackPosition[1].Y, character.union, 0, 3, ChipBase.ELEMENT.eleki, false, false);
                            this.effectattack.hitting = false;
                            this.effectattack.rebirth = this.attackRevers;
                            this.effectattack.PositionDirectSet();
                            battle.attacks.Add(this.effectattack);
                            break;
                        case 14:
                            this.animePoint.X = 2;
                            break;
                        case 16:
                            this.animePoint.X = 3;
                            this.attack = new BombAttack(this.sound, battle, this.attackPosition[1].X, this.attackPosition[1].Y - 1, character.union, this.Power(character), 1, ChipBase.ELEMENT.normal);
                            this.attack.element = ChipBase.ELEMENT.eleki;
                            this.attack.hitrange.Y = 3;
                            this.attack.rebirth = this.attackRevers;
                            battle.attacks.Add(attack);
                            break;
                        case 18:
                            this.animePoint.X = 4;
                            break;
                        case 20:
                            this.animePoint.X = 5;
                            break;
                        case 22:
                            this.animePoint.X = 6;
                            break;
                    }
                    if (character.waittime >= this.atackEndFlame)
                    {
                        if (character.waittime == this.atackEndFlame)
                        {
                            this.animePoint.Y = 0;
                            this.animePoint.X = 0;
                        }
                        else if (character.waittime == this.atackEndFlame + 2)
                            this.animePoint.X = 1;
                        else if (character.waittime == this.atackEndFlame + 4)
                            this.animePoint.X = 2;
                        else if (character.waittime == this.atackEndFlame + 6)
                            this.animePoint.X = 3;
                        else if (character.waittime == this.atackEndFlame + 8)
                            this.animePoint.X = -1;
                        else if (character.waittime == this.atackEndFlame + 10)
                        {
                            this.NextMotion(-2, 2, character);
                            this.atackEndFlame = 32;
                        }
                        break;
                    }
                    break;
                case MasterStyle.MOTION._4_PoisonWing:
                    switch (character.waittime)
                    {
                        case 2:
                            this.animePoint.Y = 0;
                            this.animePoint.X = 3;
                            break;
                        case 4:
                            this.animePoint.X = 2;
                            break;
                        case 6:
                            this.animePoint.X = 1;
                            break;
                        case 8:
                            this.animePoint.X = 0;
                            break;
                        case 10:
                            this.animePoint.Y = 1;
                            break;
                        case 12:
                            this.animePoint.X = 1;
                            this.sound.PlaySE(SoundEffect.shoot);
                            break;
                        case 14:
                            this.animePoint.X = 2;
                            break;
                        case 16:
                            this.animePoint.X = 3;
                            this.effectattack = new NSAttack.Storm(this.sound, battle, this.attackPosition[2].X, this.attackPosition[2].Y, character.union, this.Power(character) / 4, 4, ChipBase.ELEMENT.poison);
                            battle.attacks.Add(this.effectattack);
                            break;
                        case 18:
                            this.animePoint.X = 4;
                            break;
                        case 20:
                            this.animePoint.X = 5;
                            break;
                        case 22:
                            this.animePoint.X = 6;
                            break;
                    }
                    if (character.waittime >= this.atackEndFlame)
                    {
                        if (character.waittime == this.atackEndFlame)
                        {
                            this.animePoint.Y = 0;
                            this.animePoint.X = 0;
                        }
                        else if (character.waittime == this.atackEndFlame + 2)
                            this.animePoint.X = 1;
                        else if (character.waittime == this.atackEndFlame + 4)
                            this.animePoint.X = 2;
                        else if (character.waittime == this.atackEndFlame + 6)
                            this.animePoint.X = 3;
                        else if (character.waittime == this.atackEndFlame + 8)
                            this.animePoint.X = -1;
                        else if (character.waittime == this.atackEndFlame + 10)
                        {
                            this.NextMotion(1, 3, character);
                            this.atackEndFlame = 40;
                        }
                        break;
                    }
                    break;
                case MasterStyle.MOTION._5_EarthGaia:
                    switch (character.waittime)
                    {
                        case 2:
                            this.animePoint.Y = 0;
                            this.animePoint.X = 3;
                            break;
                        case 4:
                            this.animePoint.X = 2;
                            break;
                        case 6:
                            this.animePoint.X = 1;
                            break;
                        case 8:
                            this.animePoint.X = 0;
                            break;
                        case 10:
                            this.animePoint.Y = 3;
                            break;
                        case 12:
                            this.animePoint.X = 1;
                            break;
                        case 14:
                            this.animePoint.X = 2;
                            break;
                        case 16:
                            this.animePoint.X = 3;
                            this.sound.PlaySE(SoundEffect.drill2);
                            this.attack = new BombAttack(this.sound, battle, this.attackPosition[3].X, this.attackPosition[3].Y, character.union, this.Power(character) / 4, 1, ChipBase.ELEMENT.normal);
                            this.attack.element = ChipBase.ELEMENT.earth;
                            this.attack.hitrange.X = 1;
                            this.attack.breaking = true;
                            this.attack.rebirth = this.attackRevers;
                            battle.attacks.Add(attack);
                            break;
                        case 19:
                            ++this.subAnime;
                            break;
                        case 22:
                            ++this.subAnime;
                            this.attack = new BombAttack(this.sound, battle, this.attackPosition[3].X, this.attackPosition[3].Y, character.union, this.Power(character) / 4, 1, ChipBase.ELEMENT.normal);
                            this.attack.element = ChipBase.ELEMENT.earth;
                            this.attack.hitrange.X = 1;
                            this.attack.breaking = true;
                            this.attack.rebirth = this.attackRevers;
                            battle.attacks.Add(attack);
                            break;
                        case 25:
                            this.subAnime = 0;
                            break;
                        case 28:
                            ++this.subAnime;
                            this.attack = new BombAttack(this.sound, battle, this.attackPosition[3].X, this.attackPosition[3].Y, character.union, this.Power(character) / 4, 1, ChipBase.ELEMENT.normal);
                            this.attack.element = ChipBase.ELEMENT.earth;
                            this.attack.hitrange.X = 1;
                            this.attack.breaking = true;
                            this.attack.rebirth = this.attackRevers;
                            battle.attacks.Add(attack);
                            break;
                        case 31:
                            ++this.subAnime;
                            break;
                        case 34:
                            ++this.subAnime;
                            this.attack = new BombAttack(this.sound, battle, this.attackPosition[3].X, this.attackPosition[3].Y, character.union, this.Power(character) / 4, 1, ChipBase.ELEMENT.normal);
                            this.attack.element = ChipBase.ELEMENT.earth;
                            this.attack.hitrange.X = 1;
                            this.attack.breaking = true;
                            this.attack.rebirth = this.attackRevers;
                            battle.attacks.Add(attack);
                            break;
                        case 37:
                            this.subAnime = 0;
                            break;
                    }
                    if (character.waittime >= this.atackEndFlame)
                    {
                        if (character.waittime == this.atackEndFlame)
                        {
                            this.animePoint.Y = 0;
                            this.animePoint.X = 0;
                        }
                        else if (character.waittime == this.atackEndFlame + 2)
                            this.animePoint.X = 1;
                        else if (character.waittime == this.atackEndFlame + 4)
                            this.animePoint.X = 2;
                        else if (character.waittime == this.atackEndFlame + 6)
                            this.animePoint.X = 3;
                        else if (character.waittime == this.atackEndFlame + 8)
                            this.animePoint.X = -1;
                        else if (character.waittime == this.atackEndFlame + 10)
                        {
                            this.NextMotion(-2, 4, character);
                            this.atackEndFlame = 24;
                        }
                        break;
                    }
                    break;
                case MasterStyle.MOTION._6_HeatDoctor:
                    switch (character.waittime)
                    {
                        case 2:
                            this.animePoint.Y = 0;
                            this.animePoint.X = 3;
                            break;
                        case 4:
                            this.animePoint.X = 2;
                            break;
                        case 6:
                            this.animePoint.X = 1;
                            break;
                        case 8:
                            this.animePoint.X = 0;
                            break;
                        case 10:
                            this.animePoint.Y = 6;
                            break;
                        case 12:
                            this.animePoint.X = 1;
                            break;
                        case 14:
                            this.animePoint.X = 2;
                            this.sound.PlaySE(SoundEffect.chain);
                            this.effectattack = new InjectBullet(this.sound, battle, this.attackPosition[4].X - 2 * (this.attackRevers ? -1 : 1), this.attackPosition[4].Y, character.union, this.Power(character), this.texname[4], ChipBase.ELEMENT.heat);
                            this.effectattack.badstatus[1] = false;
                            this.effectattack.rebirth = this.attackRevers;
                            battle.attacks.Add(this.effectattack);
                            break;
                        case 16:
                            this.animePoint.X = 3;
                            break;
                    }
                    if (character.waittime >= this.atackEndFlame)
                    {
                        if (character.waittime == this.atackEndFlame)
                        {
                            this.animePoint.Y = 0;
                            this.animePoint.X = 0;
                        }
                        else if (character.waittime == this.atackEndFlame + 2)
                            this.animePoint.X = 1;
                        else if (character.waittime == this.atackEndFlame + 4)
                            this.animePoint.X = 2;
                        else if (character.waittime == this.atackEndFlame + 6)
                            this.animePoint.X = 3;
                        else if (character.waittime == this.atackEndFlame + 8)
                            this.animePoint.X = -1;
                        else if (character.waittime == this.atackEndFlame + 10)
                        {
                            this.NextMotion(1, 5, character);
                            this.atackEndFlame = 42;
                        }
                        break;
                    }
                    break;
                case MasterStyle.MOTION._7_AquaWhith:
                    switch (character.waittime)
                    {
                        case 2:
                            this.animePoint.Y = 0;
                            this.animePoint.X = 3;
                            break;
                        case 4:
                            this.animePoint.X = 2;
                            break;
                        case 6:
                            this.animePoint.X = 1;
                            break;
                        case 8:
                            this.animePoint.X = 0;
                            break;
                        case 10:
                            this.animePoint.Y = 6;
                            break;
                        case 12:
                            this.animePoint.X = 1;
                            break;
                        case 14:
                            this.animePoint.X = 2;
                            break;
                        case 16:
                            this.animePoint.X = 3;
                            break;
                        case 18:
                            this.animePoint.X = 4;
                            break;
                        case 20:
                            this.animePoint.X = 5;
                            break;
                        case 22:
                            this.animePoint.X = 6;
                            break;
                        case 24:
                            this.sound.PlaySE(SoundEffect.fire);
                            this.effectattack = new ElementFire(this.sound, battle, this.attackPosition[5].X, this.attackPosition[5].Y, character.union, this.Power(character), ChipBase.ELEMENT.aqua, false, 0);
                            this.effectattack.rebirth = this.attackRevers;
                            this.effectattack.PositionDirectSet();
                            this.effectattack.invincibility = false;
                            battle.attacks.Add(this.effectattack);
                            break;
                        case 28:
                            this.sound.PlaySE(SoundEffect.fire);
                            this.effectattack = new ElementFire(this.sound, battle, this.attackPosition[5].X + (this.attackRevers ? -1 : 1), this.attackPosition[5].Y, character.union, this.Power(character), ChipBase.ELEMENT.aqua, false, 0);
                            this.effectattack.rebirth = this.attackRevers;
                            this.effectattack.PositionDirectSet();
                            this.effectattack.invincibility = false;
                            battle.attacks.Add(this.effectattack);
                            break;
                        case 32:
                            this.sound.PlaySE(SoundEffect.fire);
                            this.effectattack = new ElementFire(this.sound, battle, this.attackPosition[5].X + 2 * (this.attackRevers ? -1 : 1), this.attackPosition[5].Y, character.union, this.Power(character), ChipBase.ELEMENT.aqua, false, 0);
                            this.effectattack.rebirth = this.attackRevers;
                            this.effectattack.PositionDirectSet();
                            this.effectattack.invincibility = false;
                            battle.attacks.Add(this.effectattack);
                            break;
                    }
                    if (character.waittime >= this.atackEndFlame)
                    {
                        if (character.waittime == this.atackEndFlame)
                        {
                            this.animePoint.Y = 0;
                            this.animePoint.X = 0;
                        }
                        else if (character.waittime == this.atackEndFlame + 2)
                            this.animePoint.X = 1;
                        else if (character.waittime == this.atackEndFlame + 4)
                            this.animePoint.X = 2;
                        else if (character.waittime == this.atackEndFlame + 6)
                            this.animePoint.X = 3;
                        else if (character.waittime == this.atackEndFlame + 8)
                            this.animePoint.X = -1;
                        else if (character.waittime == this.atackEndFlame + 10)
                        {
                            this.animePoint.Y = 0;
                            this.animePoint.X = 0;
                            ++this.motion;
                            character.waittime = 0;
                            this.atackEndFlame = 180;
                            character.position = this.startPosition;
                        }
                        break;
                    }
                    break;
                case MasterStyle.MOTION._8_Finish:
                    switch (character.waittime)
                    {
                        case 4:
                            Charge charge1 = new Charge(this.sound, battle, 0, 0);
                            charge1.positionDirect = new Vector2(this.tryPosition[0].X, this.tryPosition[0].Y + 16f);
                            battle.effects.Add(charge1);
                            Charge charge2 = new Charge(this.sound, battle, 0, 0);
                            charge2.positionDirect = new Vector2(this.tryPosition[1].X, this.tryPosition[1].Y + 16f);
                            battle.effects.Add(charge2);
                            Charge charge3 = new Charge(this.sound, battle, 0, 0);
                            charge3.positionDirect = new Vector2(this.tryPosition[2].X, this.tryPosition[2].Y + 16f);
                            battle.effects.Add(charge3);
                            break;
                        case 54:
                            this.animePoint.Y = 1;
                            break;
                        case 56:
                            this.animePoint.X = 1;
                            break;
                        case 58:
                            this.animePoint.X = 2;
                            break;
                        case 60:
                            this.animePoint.X = 3;
                            BombAttack bombAttack = new BombAttack(this.sound, battle, character.union == Panel.COLOR.blue ? 5 : 0, 0, character.union, this.Power(character), 1, ChipBase.ELEMENT.normal);
                            bombAttack.hitrange = new Point(6, 3);
                            bombAttack.breaking = true;
                            bombAttack.throughObject = true;
                            battle.attacks.Add(bombAttack);
                            this.sound.PlaySE(SoundEffect.bombbig);
                            this.ShakeStart(4, 90);
                            int x = Eriabash.SteelX(character, battle);
                            battle.effects.Add(new RandomBomber(this.sound, battle, Bomber.BOMBERTYPE.flashbomber, 2, new Point(x, 0), new Point(6, 2), character.union, 36));
                            break;
                        case 180:
                            this.animePoint.Y = 0;
                            this.animePoint.X = 0;
                            break;
                    }
                    if (character.waittime >= this.atackEndFlame)
                    {
                        if (character.waittime == this.atackEndFlame)
                        {
                            this.animePoint.Y = 0;
                            this.animePoint.X = 0;
                        }
                        else if (character.waittime == this.atackEndFlame + 2)
                            this.animePoint.X = 1;
                        else if (character.waittime == this.atackEndFlame + 4)
                            this.animePoint.X = 2;
                        else if (character.waittime == this.atackEndFlame + 6)
                            this.animePoint.X = 3;
                        else if (character.waittime == this.atackEndFlame + 8)
                            this.animePoint.X = -1;
                        else if (character.waittime == this.atackEndFlame + 38)
                        {
                            character.animationpoint.Y = 0;
                            character.animationpoint.X = 3;
                        }
                        else if (character.waittime == this.atackEndFlame + 40)
                            character.animationpoint.X = 2;
                        else if (character.waittime == this.atackEndFlame + 42)
                            character.animationpoint.X = 1;
                        else if (character.waittime == this.atackEndFlame + 44)
                            character.animationpoint.X = 0;
                        else if (character.waittime > this.atackEndFlame + 44 && this.BlackOutEnd(character, battle))
                            base.Action(character, battle);
                        break;
                    }
                    break;
            }
        }

        private void NextMotion(int Xmove, int i, CharacterBase character)
        {
            ++this.motion;
            character.waittime = 0;
            this.animePoint = new Point(-1, 0);
            if (Xmove < 0)
            {
                if (this.attackPosition[i].X + Xmove < 0)
                {
                    this.attackRevers = true;
                    Xmove *= -1;
                }
                else
                    this.attackRevers = false;
            }
            else if (this.attackPosition[i].X + Xmove > 5)
            {
                this.attackRevers = false;
                Xmove *= -1;
            }
            else
                this.attackRevers = true;
            this.sound.PlaySE(SoundEffect.knife);
            character.position = new Point(this.attackPosition[i].X + Xmove, this.attackPosition[i].Y);
            this.PositionDirectSet(this.attackPosition[i].X + Xmove, this.attackPosition[i].Y);
        }

        public override void GraphicsRender(
          IRenderer dg,
          Vector2 p,
          int c,
          bool printgraphics,
          bool printstatus)
        {
            if (!printgraphics)
                return;
            switch (c % 2)
            {
                case 0:
                    this._rect = new Rectangle(848, 320, 74, 79);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, p - new Vector2(9, 16), Color.White);
                    this._rect = new Rectangle(56 * 7, 48 * 1, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMasterStyleCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMasterStyleCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceMasterStyleCombo1Line3")
                    };
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                        this._position = new Vector2(p.X - 12f, p.Y - 8f + index * 16);
                        this.TextRender(dg, strArray[index], false, this._position, false, Color.LightBlue);
                    }
                    return;
            }
        }

        public override void IconRender(
          IRenderer dg,
          Vector2 p,
          bool select,
          bool custom,
          int c,
          bool noicon)
        {
            if (!noicon)
            {
                int num = 0;
                if (select)
                    num = 1;
                this._rect = new Rectangle(624, 80 + num * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, 0, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.animationpoint.X == -1)
            {
                switch (this.motion)
                {
                    case MasterStyle.MOTION._1_start:
                        for (int index = 0; index < this.tryPosition.Length; ++index)
                        {
                            this._rect = new Rectangle(character.Wide * this.animePoint.X, character.Height * this.animePoint.Y, character.Wide, character.Height);
                            this._position = new Vector2(this.tryPosition[index].X + Shake.X, this.tryPosition[index].Y + Shake.Y);
                            dg.DrawImage(dg, this.texname[index], this._rect, false, this._position, (uint)character.union > 0U, Color.White);
                            this._rect = new Rectangle(character.Wide * this.animePoint.X, character.Height * (this.animePoint.Y + 7), character.Wide, character.Height);
                            this._position = new Vector2(this.tryPosition[index].X + Shake.X, this.tryPosition[index].Y + Shake.Y);
                            Color color = Color.FromArgb((int)(byte.MaxValue - 10 * this.r), Color.White);
                            dg.DrawImage(dg, this.texname[index], this._rect, false, this._position, (uint)character.union > 0U, color);
                        }
                        break;
                    case MasterStyle.MOTION._2_LeafFighter:
                        this._rect = new Rectangle(character.Wide * this.animePoint.X, character.Height * this.animePoint.Y, character.Wide, character.Height);
                        this._position = new Vector2(this.attackPositionD.X + Shake.X, this.attackPositionD.Y + Shake.Y);
                        dg.DrawImage(dg, this.texname[0], this._rect, false, this._position, this.attackRevers, Color.White);
                        if (this.animePoint.Y != 3)
                            break;
                        this._rect = new Rectangle(this.animePoint.X * (character.Wide * 2) - character.Wide, 6 * character.Height, character.Wide * 2, character.Height);
                        double num1 = attackPositionD.X + (double)(character.Wide / 2 * (this.attackRevers ? -1 : 1));
                        Point shake1 = this.Shake;
                        double x1 = shake1.X;
                        double num2 = num1 + x1;
                        double y1 = attackPositionD.Y;
                        shake1 = this.Shake;
                        double y2 = shake1.Y;
                        double num3 = y1 + y2;
                        this._position = new Vector2((float)num2, (float)num3);
                        dg.DrawImage(dg, this.texname[0], this._rect, false, this._position, this.attackRevers, Color.White);
                        break;
                    case MasterStyle.MOTION._3_ElekiShinobi:
                        this._rect = new Rectangle(character.Wide * this.animePoint.X, character.Height * this.animePoint.Y, character.Wide, character.Height);
                        this._position = new Vector2(this.attackPositionD.X + Shake.X, this.attackPositionD.Y + Shake.Y);
                        dg.DrawImage(dg, this.texname[1], this._rect, false, this._position, this.attackRevers, Color.White);
                        if (this.animePoint.Y != 1)
                            break;
                        this._rect = new Rectangle(this.animePoint.X * character.Wide, 5 * character.Height, character.Wide, character.Height);
                        this._position = new Vector2(this.attackPositionD.X + Shake.X, this.attackPositionD.Y + Shake.Y);
                        dg.DrawImage(dg, this.texname[1], this._rect, false, this._position, this.attackRevers, Color.White);
                        break;
                    case MasterStyle.MOTION._4_PoisonWing:
                        this._rect = new Rectangle(character.Wide * this.animePoint.X, character.Height * this.animePoint.Y, character.Wide, character.Height);
                        this._position = new Vector2(this.attackPositionD.X + Shake.X, this.attackPositionD.Y + Shake.Y);
                        dg.DrawImage(dg, this.texname[2], this._rect, false, this._position, this.attackRevers, Color.White);
                        if (this.animePoint.Y != 1)
                            break;
                        this._rect = new Rectangle(this.animePoint.X * character.Wide, character.Height, character.Wide, character.Height);
                        double x2 = attackPositionD.X;
                        Point shake2 = this.Shake;
                        double x3 = shake2.X;
                        double num4 = x2 + x3;
                        double y3 = attackPositionD.Y;
                        shake2 = this.Shake;
                        double y4 = shake2.Y;
                        double num5 = y3 + y4;
                        this._position = new Vector2((float)num4, (float)num5);
                        dg.DrawImage(dg, "slash", this._rect, false, this._position, this.attackRevers, Color.White);
                        break;
                    case MasterStyle.MOTION._5_EarthGaia:
                        this._rect = new Rectangle(character.Wide * this.animePoint.X, character.Height * this.animePoint.Y, character.Wide, character.Height);
                        this._position = new Vector2(this.attackPositionD.X + Shake.X, this.attackPositionD.Y + Shake.Y);
                        dg.DrawImage(dg, this.texname[3], this._rect, false, this._position, this.attackRevers, Color.White);
                        if (this.animePoint.Y != 3)
                            break;
                        this._rect = new Rectangle(this.animePoint.X * (character.Wide * 2) + this.subAnime * (character.Wide * 2), character.Height, character.Wide * 2, character.Height);
                        double num6 = attackPositionD.X + (double)(character.Wide / 2 * (this.attackRevers ? -1 : 1));
                        Point shake3 = this.Shake;
                        double x4 = shake3.X;
                        double num7 = num6 + x4;
                        double y5 = attackPositionD.Y;
                        shake3 = this.Shake;
                        double y6 = shake3.Y;
                        double num8 = y5 + y6;
                        this._position = new Vector2((float)num7, (float)num8);
                        dg.DrawImage(dg, "Lances", this._rect, false, this._position, this.attackRevers, Color.White);
                        break;
                    case MasterStyle.MOTION._6_HeatDoctor:
                        this._rect = new Rectangle(character.Wide * this.animePoint.X, character.Height * this.animePoint.Y, character.Wide, character.Height);
                        this._position = new Vector2(this.attackPositionD.X + Shake.X, this.attackPositionD.Y + Shake.Y);
                        dg.DrawImage(dg, this.texname[4], this._rect, false, this._position, this.attackRevers, Color.White);
                        break;
                    case MasterStyle.MOTION._7_AquaWhith:
                        this._rect = new Rectangle(character.Wide * this.animePoint.X, character.Height * this.animePoint.Y, character.Wide, character.Height);
                        this._position = new Vector2(this.attackPositionD.X + Shake.X, this.attackPositionD.Y + Shake.Y);
                        dg.DrawImage(dg, this.texname[5], this._rect, false, this._position, this.attackRevers, Color.White);
                        break;
                    case MasterStyle.MOTION._8_Finish:
                        for (int index = 0; index < this.tryPosition.Length; ++index)
                        {
                            this._rect = new Rectangle(character.Wide * this.animePoint.X, character.Height * this.animePoint.Y, character.Wide, character.Height);
                            this._position = new Vector2(this.tryPosition[index].X + Shake.X, this.tryPosition[index].Y + Shake.Y);
                            dg.DrawImage(dg, this.texname[3 + index], this._rect, false, this._position, (uint)character.union > 0U, Color.White);
                            if (this.animePoint.Y == 1)
                            {
                                this._rect = new Rectangle(this.animePoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
                                this._position = new Vector2(this.tryPosition[index].X + Shake.X, this.tryPosition[index].Y + Shake.Y);
                                dg.DrawImage(dg, this.texname[3 + index], this._rect, false, this._position, this.attackRevers, Color.White);
                            }
                        }
                        break;
                }
            }
            else
                this.BlackOutRender(dg, character.union);
        }

        private enum MOTION
        {
            _0_setup,
            _1_start,
            _2_LeafFighter,
            _3_ElekiShinobi,
            _4_PoisonWing,
            _5_EarthGaia,
            _6_HeatDoctor,
            _7_AquaWhith,
            _8_Finish,
        }
    }
}


