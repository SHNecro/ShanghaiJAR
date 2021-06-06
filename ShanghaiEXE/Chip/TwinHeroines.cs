using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSGame;
using NSObject;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSChip
{
    internal class TwinHeroines : ChipBase
    {
        private int count1 = 0;
        private int count2 = 0;
        private List<Point> positions = new List<Point>();
        private List<Point> positions2 = new List<Point>();
        private int chargeEffect = -1;
        private const int interval = 20;
        private const int speed = 2;
        private int[] motionList;
        private int nowmotion;
        private bool swing;
        private bool end;
        private Vector2 xPosition;
        private int attackCount;
        private int attacking;
        private int chargeanime;
        private const int s = 5;
        private Point animePoint;

        public TwinHeroines(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 290;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.TwinHeroinesName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 100;
            this.subpower = 0;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.TwinHeroinesDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
            this.motionList = new int[4] { 0, 1, 2, 3 };
        }

        protected Point Animation(int waittime)
        {
            int[] interval = new int[6] { 4, 1, 1, 8, 1, 60 };
            int[] xpoint = new int[6] { 0, 8, 12, 13, 16, 17 };
            for (int index = 0; index < interval.Length; ++index)
                interval[index] *= 5;
            int y = 0;
            return CharacterAnimation.ReturnKai(interval, xpoint, y, waittime);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            ++this.chargeanime;
            if (this.moveflame)
            {
                switch (this.nowmotion)
                {
                    case 0:
                        switch (this.frame)
                        {
                            case 1:
                                this.xPosition = character.positionDirect;
                                character.positionDirect.X -= 8 * this.UnionRebirth(character.union);
                                character.positionDirect.Y -= 8f;
                                this.xPosition.X += 48 * this.UnionRebirth(character.union);
                                this.xPosition.Y += 8f;
                                this.sound.PlaySE(SoundEffect.warp);
                                this.animePoint = new Point();
                                this.chargeEffect = 0;
                                break;
                            case 5:
                                this.sound.PlaySE(SoundEffect.charge);
                                this.chargeEffect = 1;
                                break;
                            case 30:
                                this.chargeEffect = 2;
                                break;
                            case 55:
                                this.chargeEffect = 0;
                                ++this.nowmotion;
                                this.frame = 0;
                                int num = 18;
                                while (this.positions.Count < num)
                                {
                                    for (int x = 0; x < battle.panel.GetLength(0); ++x)
                                    {
                                        for (int y = 0; y < battle.panel.GetLength(1); ++y)
                                        {
                                            if (battle.panel[x, y].color == character.UnionEnemy)
                                            {
                                                this.positions.Add(new Point(x, y));
                                                this.positions2.Add(new Point(x, y));
                                            }
                                        }
                                    }
                                }
                                this.positions2 = this.positions2.OrderBy<Point, Guid>(i => Guid.NewGuid()).ToList<Point>();
                                this.positions = this.positions.OrderBy<Point, Guid>(i => Guid.NewGuid()).ToList<Point>();
                                break;
                        }
                        break;
                    case 1:
                        int waitflame = this.frame % 6;
                        character.animationpoint = this.AnimeBuster(waitflame % 3);
                        this.animePoint.X = !this.swing ? this.AnimeSlash2(waitflame).X : this.AnimeSlash1(waitflame).X;
                        switch (waitflame)
                        {
                            case 0:
                                if (this.attackCount >= 24)
                                {
                                    ++this.nowmotion;
                                    this.frame = 0;
                                    break;
                                }
                                this.sound.PlaySE(SoundEffect.damageenemy);
                                battle.effects.Add(new NormalChargehit(this.sound, battle, this.positions[this.count1].X, this.positions[this.count1].Y, 2));
                                ++this.count1;
                                if (this.count1 >= this.positions.Count)
                                    this.count1 = 0;
                                this.swing = !this.swing;
                                new BombAttack(this.sound, character.parent, character.union == Panel.COLOR.red ? 0 : 5, 0, character.union, 0, 1, 1, new Point(5, 3), this.element)
                                {
                                    bright = false
                                }.invincibility = false;
                                break;
                            case 3:
                                this.sound.PlaySE(SoundEffect.sword);
                                this.sound.PlaySE(SoundEffect.damageenemy);
                                battle.effects.Add(new NormalChargehit(this.sound, battle, this.positions[this.count1].X, this.positions[this.count1].Y, 3));
                                ++this.count1;
                                if (this.count1 >= this.positions.Count)
                                    this.count1 = 0;
                                AttackBase attackBase = new SwordAttack(this.sound, battle, this.positions2[this.count2].X, this.positions2[this.count2].Y, character.union, 0, 3, this.element, false, false);
                                attackBase.hitting = false;
                                battle.attacks.Add(attackBase);
                                ++this.count2;
                                if (this.count2 >= this.positions2.Count)
                                    this.count2 = 0;
                                BombAttack bombAttack = new BombAttack(this.sound, character.parent, character.union == Panel.COLOR.red ? 0 : 5, 0, character.union, this.Power(character), 1, 1, new Point(5, 3), this.element);
                                bombAttack.invincibility = false;
                                bombAttack.bright = false;
                                if ((uint)this.attacking > 0U)
                                    bombAttack.power = 0;
                                ++this.attacking;
                                if (this.attacking >= 4)
                                    this.attacking = 0;
                                ++this.attackCount;
                                battle.attacks.Add(bombAttack);
                                break;
                        }
                        break;
                    case 2:
                        switch (this.frame)
                        {
                            case 30:
                                character.animationpoint = new Point();
                                character.PositionDirectSet();
                                this.chargeEffect = -1;
                                this.end = true;
                                ++this.nowmotion;
                                this.frame = 0;
                                break;
                        }
                        break;
                }
            }
            if (this.end && this.BlackOutEnd(character, battle))
                base.Action(character, battle);
            this.FlameControl(2);
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
                    this._rect = new Rectangle(56 * 3, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic2", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceTwinHeroinesCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceTwinHeroinesCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceTwinHeroinesCombo1Line3")
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
                int num1 = this.number - 1;
                int num2 = num1 % 40;
                int num3 = num1 / 40;
                int num4 = 0;
                if (select)
                    num4 = 1;
                this._rect = new Rectangle(480, 64 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, 0, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (this.chargeEffect > -1)
            {
                if (this.end)
                    return;
                Point point = new Point(character.animationpoint.X, 4);
                int x1 = (int)character.positionDirect.X;
                Point shake = this.Shake;
                int x2 = shake.X;
                double num1 = x1 + x2;
                int y1 = (int)character.positionDirect.Y;
                shake = this.Shake;
                int y2 = shake.Y;
                double num2 = y1 + y2;
                this._position = new Vector2((float)num1, (float)num2);
                this._rect = new Rectangle(point.X * character.Wide, point.Y * character.Height, character.Wide, character.Height);
                dg.DrawImage(dg, character.picturename, this._rect, false, this._position, Color.White);
                if (this.chargeEffect == 1)
                {
                    this._rect = new Rectangle(this.chargeanime % 16 / 2 * 64, 0, 64, 64);
                    double num3 = character.position.X * 40.0 + 0.0 + 8.0;
                    shake = this.Shake;
                    double x3 = shake.X;
                    double num4 = num3 + x3;
                    double num5 = character.position.Y * 24.0 + 58.0;
                    shake = this.Shake;
                    double y3 = shake.Y;
                    double num6 = num5 + y3;
                    this._position = new Vector2((float)num4, (float)num6);
                    dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                }
                else if (this.chargeEffect == 2)
                {
                    this._rect = new Rectangle(this.chargeanime % 16 / 2 * 64, 64, 64, 64);
                    double num3 = character.position.X * 40.0 + 0.0 + 8.0;
                    shake = this.Shake;
                    double x3 = shake.X;
                    double num4 = num3 + x3;
                    double num5 = character.position.Y * 24.0 + 58.0;
                    shake = this.Shake;
                    double y3 = shake.Y;
                    double num6 = num5 + y3;
                    this._position = new Vector2((float)num4, (float)num6);
                    dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                }
                this._rect = new Rectangle(96 * this.animePoint.X, 0, 96, 96);
                this._position = new Vector2(this.xPosition.X, this.xPosition.Y);
                dg.DrawImage(dg, "yorihime", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
                if (this.chargeEffect == 1)
                {
                    this._rect = new Rectangle(this.chargeanime % 16 / 2 * 64, 0, 64, 64);
                    double num3 = character.position.X * 40.0 + 32.0 + 12.0;
                    shake = this.Shake;
                    double x3 = shake.X;
                    double num4 = num3 + x3;
                    double num5 = character.position.Y * 24.0 + 76.0;
                    shake = this.Shake;
                    double y3 = shake.Y;
                    double num6 = num5 + y3;
                    this._position = new Vector2((float)num4, (float)num6);
                    dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                }
                else if (this.chargeEffect == 2)
                {
                    this._rect = new Rectangle(this.chargeanime % 16 / 2 * 64, 64, 64, 64);
                    double num3 = character.position.X * 40.0 + 32.0 + 12.0;
                    shake = this.Shake;
                    double x3 = shake.X;
                    double num4 = num3 + x3;
                    double num5 = character.position.Y * 24.0 + 76.0;
                    shake = this.Shake;
                    double y3 = shake.Y;
                    double num6 = num5 + y3;
                    this._position = new Vector2((float)num4, (float)num6);
                    dg.DrawImage(dg, "charge", this._rect, false, this._position, Color.FromArgb(200, byte.MaxValue, byte.MaxValue, byte.MaxValue));
                }
            }
            else
                this.BlackOutRender(dg, character.union);
        }

        protected Point AnimeMove(int waitflame)
        {
            return CharacterAnimation.Return(new int[9]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        7,
        8,
        9
            }, new int[8] { 2, 1, 0, 1, 2, 2, 1, 0 }, 1, waitflame);
        }

        protected Point AnimeBuster(int waitflame)
        {
            return CharacterAnimation.Return(new int[3]
            {
        0,
        1,
        2
            }, new int[3] { 5, 6, 5 }, 0, waitflame);
        }

        protected Point AnimeSlash1(int waitflame)
        {
            return CharacterAnimation.Return(new int[7]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        100
            }, new int[7] { 5, 6, 7, 8, 9, 10, 11 }, 0, waitflame);
        }

        protected Point AnimeSlash2(int waitflame)
        {
            return CharacterAnimation.Return(new int[7]
            {
        0,
        1,
        2,
        3,
        4,
        5,
        100
            }, new int[7] { 12, 13, 14, 15, 16, 17, 18 }, 0, waitflame);
        }

        private Point AnimeSlash3(int waitflame)
        {
            return CharacterAnimation.Return(new int[7]
            {
        5,
        6,
        7,
        8,
        9,
        10,
        100
            }, new int[7] { 19, 20, 21, 22, 23, 24, 25 }, 0, waitflame);
        }

        protected int TargetX(CharacterBase character, SceneBattle battle)
        {
            List<CharacterBase> characterBaseList = new List<CharacterBase>();
            foreach (CharacterBase characterBase in battle.AllHitter())
            {
                if (characterBase is EnemyBase)
                {
                    if (characterBase.union == character.UnionEnemy)
                        characterBaseList.Add(characterBase);
                }
                else if (characterBase is ObjectBase)
                {
                    ObjectBase objectBase = (ObjectBase)characterBase;
                    if ((objectBase.unionhit || objectBase.union == character.union) && character.UnionEnemy == objectBase.StandPanel.color)
                        characterBaseList.Add(characterBase);
                }
            }
            bool flag = false;
            int num = character.union == Panel.COLOR.red ? 6 : -1;
            foreach (CharacterBase characterBase in characterBaseList)
            {
                if (characterBase.position.Y == character.position.Y)
                {
                    flag = true;
                    if (character.union == Panel.COLOR.red)
                    {
                        if (num > characterBase.position.X)
                            num = characterBase.position.X;
                    }
                    else if (num < characterBase.position.X)
                        num = characterBase.position.X;
                }
            }
            if (flag)
                return num - this.UnionRebirth(character.union);
            foreach (CharacterBase characterBase in characterBaseList)
            {
                if (characterBase.position.Y != character.position.Y)
                {
                    if (character.union == Panel.COLOR.red)
                    {
                        if (num > characterBase.position.X)
                            num = characterBase.position.X;
                    }
                    else if (num < characterBase.position.X)
                        num = characterBase.position.X;
                }
            }
            return num - this.UnionRebirth(character.union);
        }
    }
}


