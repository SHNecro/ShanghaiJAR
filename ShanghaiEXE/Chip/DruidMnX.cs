using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;
using NSShanghaiEXE.Common;
using System.Collections.Generic;
using System;
using NSShanghaiEXE.ExtensionMethods;
using System.Linq;
using NSGame;

namespace NSChip
{
    internal class DruidMnX : ChipBase
    {
        private static readonly Rectangle FullFrameRect = new Rectangle(0, 0, 104, 99);
        private static readonly Rectangle BodyRect = new Rectangle(47, 16, 48, 83);

        private static readonly Vector2 SpriteOffset = new Vector2(-BodyRect.Width / 2.0f, -8);

        private static readonly Animation[] AnimationTiming =
        {
            new Animation { Frame = 0, Row = 0, Delay = 24 },
            new Animation { Frame = 0, Row = 2, Delay = 2 },
            new Animation { Frame = 1, Row = 2, Delay = 4 },
            new Animation { Frame = 2, Row = 2, Delay = 6 },
            new Animation { Frame = 3, Row = 2, Delay = 3 },
            new Animation { Frame = 4, Row = 2, Delay = 2 },
            new Animation { Frame = 5, Row = 2, Delay = 2 },
            new Animation { Frame = 6, Row = 2, Delay = 3 },
            new Animation { Frame = 7, Row = 2, Delay = 3 },
            new Animation { Frame = 8, Row = 2, Delay = 3 },
            new Animation { Frame = 9, Row = 2, Delay = BurstWarning + (BurstSpacing * 6 * 3) }
        };
        private static readonly IEnumerable<Tuple<Animation, int>> AnimationFrameTimings = AnimationTiming.ToFrameTimings();

        private const int BurstSpacing = 3;
        private const int BurstWarning = 60;

        private const int PostAttackDelay = 40;

        private List<Tuple<Point, int>> burstWarnings;
        private Point animePoint;
        private bool afterimage;

        private int? initialEnemyHPSum;

        private bool isElemental;

        public DruidMnX(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 419;
            this.name = ShanghaiEXE.Translate("Chip.DruidManXName");
            this.element = ChipBase.ELEMENT.poison;
            this.power = 200;
            this.subpower = 0;
            this.regsize = 99;
            this.reality = 5;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.D;
            this.code[1] = ChipFolder.CODE.D;
            this.code[2] = ChipFolder.CODE.D;
            this.code[3] = ChipFolder.CODE.X;
            var informationDialogue = NSGame.ShanghaiEXE.Translate("Chip.DruidManXDesc");
            this.information[0] = informationDialogue[0];
            this.information[1] = informationDialogue[1];
            this.information[2] = informationDialogue[2];
            this.Init();
            this.burstWarnings = new List<Tuple<Point, int>>();
        }

        public override void Init()
        {
            base.Init();
            this.sortNumber = 247.5f;
        }

        private Point Animation(int waittime)
        {
            var currentFrame = AnimationFrameTimings.FirstOrDefault(t => t.Item2 > waittime)?.Item1 ?? AnimationTiming.Last();
            return new Point(currentFrame.Frame, currentFrame.Row);
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
                return;
            var totalAnimationTime = this.isElemental
                ? AnimationTiming.Select((a, i) => i == AnimationTiming.Length - 1 ? BurstWarning + this.ExtraElementalTime(BurstSpacing * 6 * 3) : a.Delay).Sum()
                : AnimationTiming.Sum(a => a.Delay);
            this.animePoint = this.Animation(character.waittime);
            if (character.waittime == 0)
            {
                character.animationpoint.X = -1;
                this.sound.PlaySE(SoundEffect.warp);
                this.initialEnemyHPSum = battle.AllChara().Where(c => character.UnionEnemy == c.union).Sum(c => c.Hp);
            }
            else if (character.waittime < AnimationTiming.First().Delay)
            {
                if (Input.IsPress(Button._A) && !this.isElemental)
                {
                    this.isElemental = true;
                    this.sound.PlaySE(SoundEffect.CommandSuccess);
                }
            }
            else if (character.waittime >= AnimationTiming.First().Delay && character.waittime < totalAnimationTime)
            {
                var burstAttackTime = character.waittime - AnimationTiming.First().Delay;
                this.burstWarnings.ForEach(bw =>
                {
                    if (bw.Item2 == burstAttackTime)
                    {
                        if (this.isElemental)
                        {
                            if (Input.IsPush(Button.Left))
                            {
                                this.element = ELEMENT.aqua;
                            }
                            else if (Input.IsPush(Button.Right))
                            {
                                this.element = ELEMENT.earth;
                            }
                            else if (Input.IsPush(Button.Up))
                            {
                                this.element = ELEMENT.eleki;
                            }
                            else if (Input.IsPush(Button.Down))
                            {
                                this.element = ELEMENT.heat;
                            }
                            else if (Input.IsPush(Button._L))
                            {
                                this.element = ELEMENT.poison;
                            }
                            else if (Input.IsPush(Button._R))
                            {
                                this.element = ELEMENT.leaf;
                            }
                        }

                        var towerAttack = (AttackBase)new Tower(
                            this.sound,
                            character.parent,
                            bw.Item1.X,
                            bw.Item1.Y,
                            character.union,
                            this.Power(character),
                            -1,
                            this.element);
                        switch (this.element)
                        {
                            case ChipBase.ELEMENT.heat:
                                break;
                            case ChipBase.ELEMENT.aqua:
                                break;
                            case ChipBase.ELEMENT.eleki:
                                towerAttack = new CrackThunder(
                                    this.sound,
                                    character.parent,
                                    bw.Item1.X,
                                    bw.Item1.Y,
                                    character.union,
                                    this.Power(character),
                                    false);
                                break;
                            case ChipBase.ELEMENT.leaf:
                                break;
                            case ChipBase.ELEMENT.poison:
                                break;
                            case ChipBase.ELEMENT.earth:
                                towerAttack = new SandHoleAttack(
                                    this.sound,
                                    character.parent,
                                    bw.Item1.X,
                                    bw.Item1.Y,
                                    character.union,
                                    this.Power(character),
                                    4,
                                    0,
                                    SandHoleAttack.MOTION.set,
                                    ChipBase.ELEMENT.earth);
                                break;
                        }
                        
                        character.parent.attacks.Add(this.Paralyze(towerAttack));
                    }
                });
                var modifiedBurstSpacing = this.isElemental ? this.ExtraElementalTime(BurstSpacing) : BurstSpacing;
                if (burstAttackTime % modifiedBurstSpacing == 0)
                {
                    var columnProgress = (burstAttackTime / modifiedBurstSpacing) % 6;
                    var column = character.union == Panel.COLOR.red ? columnProgress : (5 - columnProgress);
                    var validRows = Enumerable.Range(0, 3).Where(r =>
                        !character.parent.panel[column, r].Hole
                        && !this.burstWarnings.Any(bw => bw.Item1.X == column && bw.Item1.Y == r)).ToArray();
                    var row = validRows.Length > 0 ? validRows[this.Random.Next(0, validRows.Length)] : -1;
                    if (row != -1)
                    {
                        this.burstWarnings.Add(Tuple.Create(new Point(column, row), burstAttackTime + BurstWarning));
                        character.parent.attacks.Add(new Dummy(
                            this.sound,
                            character.parent,
                            column,
                            row,
                            character.union,
                            new Point(0, 0),
                            BurstWarning,
                            !this.isElemental));
                    }
                }
            }
            else if (character.waittime >= totalAnimationTime && character.waittime < totalAnimationTime + PostAttackDelay)
            {
                this.afterimage = true;

                if (!this.isElemental && this.initialEnemyHPSum != null)
                {
                    var afterBurstEnemyHpSum = battle.AllChara().Where(c => character.UnionEnemy == c.union).Sum(c => c.Hp);
                    var drainedLife = this.initialEnemyHPSum.Value - afterBurstEnemyHpSum;
                    if (drainedLife > 0)
                    {
                        this.sound.PlaySE(SoundEffect.repair);
                        character.Hp += drainedLife;
                    }

                    this.initialEnemyHPSum = null;
                }
            }
            else if (character.waittime >= totalAnimationTime + PostAttackDelay && this.BlackOutEnd(character, battle))
            {
                base.Action(character, battle);
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
                this._rect = new Rectangle(1008, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic18", this._rect, true, p, Color.White);
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
                this._rect = new Rectangle(592, 80 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase player)
        {
            if (player.animationpoint.X == -1)
            {
                var positionDirect = new Vector2(40.0f + (BodyRect.Width / 2.0f), 24.0f + (FullFrameRect.Height / 2.0f));
                var spritePositionDirect =  positionDirect + SpriteOffset + new Vector2(player.union == Panel.COLOR.red ? BodyRect.Width - 16 : 0, 0);
                this._position = spritePositionDirect;
                if (!this.afterimage)
                {
                    this._rect = new Rectangle(FullFrameRect.Width * this.animePoint.X, FullFrameRect.Height * this.animePoint.Y, FullFrameRect.Width, FullFrameRect.Height);
                    dg.DrawImage(dg, "druidmanSP", this._rect, false, this._position, player.union == Panel.COLOR.red, Color.White);
                }
                else if (player.waittime % 2 == 0)
                {
                    this._rect = new Rectangle(0, 0, FullFrameRect.Width, FullFrameRect.Height);
                    dg.DrawImage(dg, "druidmanSP", this._rect, false, this._position, player.union == Panel.COLOR.red, Color.FromArgb(60, 0, 0));
                }
            }
            else
                this.BlackOutRender(dg, player.union);
        }

        private int ExtraElementalTime(int baseTime)
        {
            return baseTime * 2;
        }
    }
}

