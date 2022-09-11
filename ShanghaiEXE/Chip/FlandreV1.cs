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
using System;
using System.Linq;

namespace NSChip
{
    internal class FlandreV1 : ChipBase
    {
        private const int interval = 20;
        private const int speed = 2;
        private int[] motionList;
        private int nowmotion;
        private bool end;
        private int command;
        protected int xPosition;
        private const int s = 5;
        protected Point animePoint;
        private int swrInt = 3;

        private List<Point> targetMulti = new List<Point>();

        public FlandreV1(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            
            this.number = 203;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.FlandreV1Name");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 90;
            this.subpower = 30;
            this.regsize = 16;
            this.reality = 3;
            this.swordtype = false;
            this._break = false;
            this.dark = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.F;
            this.code[1] = ChipFolder.CODE.L;
            this.code[2] = ChipFolder.CODE.asterisk;
            this.code[3] = ChipFolder.CODE.asterisk;
            
            var information = NSGame.ShanghaiEXE.Translate("Chip.FlandreV1Desc");
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
                                //this.animePoint.X = this.AnimeMove(this.frame).X;
                                this.animePoint.X = 0;
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
                                        //this.xPosition = this.TargetX(character, battle);
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
                                this.animePoint.X = this.AnimeBomb(this.frame).X;
                                int flame2 = this.frame;

                                if (flame2 == 8)
                                {
                                    
                                    this.sound.PlaySE(SoundEffect.sword);

                                    //this.targetMulti[0] = point1;

                                    for (int i = 0; i < this.swrInt; i++)
                                    {
                                        Point point = this.RandomPanel(false, false, false, character.UnionEnemy, battle, 0);
                                        if (i == 0)
                                        {
                                            point = this.RandomTarget(character, battle);
                                        }
                                        battle.attacks.Add(this.Paralyze(new KnifeAttack(this.sound, battle, point.X, point.Y, character.union, this.subpower, 2, ChipBase.ELEMENT.heat, false)));
                                    }
                                    

                                }


                                if (flame2 == 16)
                                {
                                    this.nowmotion++;
                                    this.frame = 0;
                                }

                                
                                break;
                            }

                        case 2:
                            {
                                this.animePoint.X = this.AnimeGear(this.frame).X;
                                int flame4 = this.frame;

                                if (flame4 == 6)
                                {
                                    Point point = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y);

                                    battle.attacks.Add(this.Paralyze(new FlanGear(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 0)));
                                    //FlanGear(this.sound, this.parent, knifeX[0], knifeY[0], this.union, this.Power, 0)
                                }

                                if (flame4 == 19)
                                {
                                    this.nowmotion++;
                                    this.frame = 0;
                                }

                                break;
                            }

                        case 3:
                            {
                                int flame3 = this.frame;
                                if (flame3 != 6 && flame3 == 10)
                                {
                                    character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, this.xPosition, character.position.Y));
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
                this._rect = new Rectangle(56 * 6, 0, 56, 48);
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
                int num1 = this.number - 1;
                int num2 = num1 % 40;
                int num3 = num1 / 40;
                int num4 = 0;
                if (select)
                    num4 = 1;
                this._rect = new Rectangle(480, 64 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.animationpoint.X == -1)
            {
                if (this.end)
                    return;
                this._rect = new Rectangle(78 * this.animePoint.X, 0, 78, 78);
                this._position = new Vector2((float)(xPosition * 40.0 + 20.0) * this.UnionRebirth(character.union), (float)(character.position.Y * 24.0 + 46.0));
                dg.DrawImage(dg, "flandre", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
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


        private Point AnimeGear(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[10]
            {
        1,
        1,
        1,
        1,
        1,
        1,
        10,
        1,
        1,
        1
            }, new int[11] { 0, 4, 5, 6, 7, 8, 11, 5, 4, 0, 0 }, 0, waittime);
        }

        private Point AnimeBomb(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[6]
            {
        2,
        2,
        2,
        2,
        2,
        2
            }, new int[6] { 0, 15, 16, 17, 18, 19 }, 0, waittime);
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
                1,
                100
            };
            int[] xpoint = new int[]
            {
                20,
                21,
                22,
                0,
                1,
                2,
                23,
                24,
                25,
                26,
                27,
                28
            };
            int y = 0;
            return CharacterAnimation.Return(array, xpoint, y, waitflame);
        }

        
    }
}

