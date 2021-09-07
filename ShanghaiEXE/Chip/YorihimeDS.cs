using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSNet;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class YorihimeDS : ChipBase
    {
        private const int interval = 20;
        private const int speed = 2;
        private int[] motionList;
        private int nowmotion;
        private bool end;
        private int command;
        protected int xPosition;
        protected int yPosition;
        private const int s = 5;
        protected Point animePoint;

        public YorihimeDS(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.libraryDisplayId = NSGame.ShanghaiEXE.Translate("DataList.IllegalChipDisplayId");
            this.number = 268;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.YorihimeDSName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 400;
            this.subpower = 0;
            this.regsize = 99;
            this.reality = 5;
            this.swordtype = true;
            this._break = true;
            this.dark = true;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.Y;
            this.code[1] = ChipFolder.CODE.Y;
            this.code[2] = ChipFolder.CODE.Y;
            this.code[3] = ChipFolder.CODE.Y;
            var information = NSGame.ShanghaiEXE.Translate("Chip.YorihimeDSDesc");
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
            if (base.BlackOut(character, battle, this.name, base.Power(character).ToString()))
            {
                if (this.moveflame)
                {
                    switch (this.nowmotion)
                    {
                        case 0:
                            {
                                this.animePoint.X = this.AnimeMove(this.frame).X;
                                int flame = this.frame;
                                switch (flame)
                                {
                                    case 1:
                                        character.animationpoint.X = -1;
                                        this.xPosition = character.position.X;
                                        this.sound.PlaySE(SoundEffect.warp);
                                        break;
                                    case 2:
                                    case 4:
                                        break;
                                    case 3:
                                        if (this.command == 5)
                                        {
                                            this.xPosition = character.position.X;
                                            this.nowmotion++;
                                            this.frame = 0;
                                        }
                                        break;
                                    case 5:
                                        this.xPosition = this.TargetX(character, battle);

                                        //base.UnionRebirth(character.union)

                                        int x = character.union == Panel.COLOR.blue ? 2 : 3;
                                        this.xPosition = x;
                                        this.yPosition = 1;
                                        

                                        if (this.xPosition < 0)
                                        {
                                            this.xPosition = 0;
                                        }
                                        if (this.xPosition > 5)
                                        {
                                            this.xPosition = 5;
                                        }
                                        break;
                                    default:
                                        if (flame == 9)
                                        {
                                            this.nowmotion++;
                                            this.frame = 0;
                                        }
                                        break;
                                }
                                break;
                            }
                        case 1:
                            {
                                this.animePoint.X = this.AnimeSlash4(this.frame).X;
                                int flame2 = this.frame;
                                if (flame2 != 5)
                                {
                                    if (flame2 != 6)
                                    {
                                        if (flame2 == 10)
                                        {
                                            this.nowmotion++;
                                            this.frame = 0;
                                        }
                                    }
                                    else
                                    {
                                        this.sound.PlaySE(SoundEffect.bombmiddle);
                                        base.ShakeStart(2, 16);
                                        for (int i = 0; i < 2; i++)
                                        {
                                            for (int j = 0; j < 3; j++)
                                            {
                                                AttackBase attackBase = new BombAttack(this.sound, battle, this.xPosition + (1 + i) * base.UnionRebirth(character.union), j, character.union, base.Power(character), 4, this.element);
                                                attackBase.breaking = true;
                                                attackBase.invincibility = false;
                                                battle.attacks.Add(this.Paralyze(attackBase));
                                                battle.effects.Add(new Shock(this.sound, battle, attackBase.position.X, attackBase.position.Y, 2, Panel.COLOR.red));
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case 2:
                            {
                                int flame3 = this.frame;
                                if (flame3 != 6 && flame3 == 10)
                                {
                                    character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, this.xPosition, this.yPosition));
                                    this.end = true;
                                    this.nowmotion++;
                                    this.frame = 0;
                                }
                                break;
                            }
                    }
                }
                if (this.end && base.BlackOutEnd(character, battle))
                {
                    base.Action(character, battle);
                }
                base.FlameControl(2);
            }

        }

        public override void GraphicsRender(
          IRenderer dg,
          Vector2 p,
          int c,
          bool printgraphics,
          bool printstatus)
        {
            if (printgraphics)
            {
                this._rect = new Rectangle(168, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic23", this._rect, true, p, Color.White);
            }
            base.GraphicsRender(dg, p, c, printgraphics, printstatus);
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
                int num = this.number - 1;
                int num2 = num % 40;
                int num3 = num / 40;
                int num4 = 0;
                if (select)
                {
                    num4 = 1;
                }
                this._rect = new Rectangle(16, 80 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, true);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.animationpoint.X == -1)
            {
                if (this.end)
                    return;
                this._rect = new Rectangle(96 * this.animePoint.X, 0, 96, 96);
                this._position = new Vector2((float)(xPosition * 40.0 + 48.0) + 4 * this.UnionRebirth(character.union), (float)(yPosition * 24.0 + 44.0));
                dg.DrawImage(dg, "yorihimeDS", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
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

        protected Point AnimeSlash1(int waitflame)
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
            }, new int[7] { 5, 6, 7, 8, 9, 10, 11 }, 0, waitflame);
        }

        protected Point AnimeSlash2(int waitflame)
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

        private Point AnimeSlash4(int waitflame)
        {
            int[] array = new int[]
            {
                35,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                100
            };
            int[] xpoint = new int[]
            {
                26,
                27,
                28,
                29,
                30,
                31,
                32,
                8,
                9,
                10,
                11
            };
            // need to fix this later so that the opening animation goes into the crouch -> warp -> slash set
            int y = 0;
            return CharacterAnimation.Return(array, xpoint, y, waitflame);
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
                else if (characterBase is Player)
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

