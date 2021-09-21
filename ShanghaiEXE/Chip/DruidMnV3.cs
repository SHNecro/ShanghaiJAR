using NSAttack;
using NSBattle;
using NSBattle.Character;
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
    internal class DruidMnV3 : ChipBase
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
            new Animation { Frame = 9, Row = 2, Delay = BurstWarning + (BurstSpacing * 6 * 3) },

            new Animation { Frame = 0, Row = 6, Delay = 4 },
            new Animation { Frame = 1, Row = 6, Delay = 2 },
            new Animation { Frame = 2, Row = 6, Delay = 4 },
            new Animation { Frame = 3, Row = 6, Delay = 2 },
            new Animation { Frame = 4, Row = 6, Delay = 4 },
            new Animation { Frame = 5, Row = 6, Delay = 4 },
            new Animation { Frame = 6, Row = 6, Delay = 2 },
            new Animation { Frame = 7, Row = 6, Delay = 2 },
            new Animation { Frame = 8, Row = 6, Delay = 4 },
            new Animation { Frame = 9, Row = 6, Delay = 2 },
            new Animation { Frame = 10, Row = 6, Delay = 4 },
            new Animation { Frame = 11, Row = 6, Delay = 4 },
            new Animation { Frame = 11, Row = 6, Delay = WaveSpeed * 10 },
        };
        private static readonly IEnumerable<Tuple<Animation, int>> AnimationFrameTimings = AnimationTiming.ToFrameTimings();

        private const int BurstSpacing = 3;
        private const int BurstWarning = 60;

        private const int WaveAttackDelay = 16;
        private const int WaveSpeed = 3;

        private const int PostAttackDelay = 40;

        private int poisonPower;

        private List<Tuple<Point, int>> burstWarnings;
        private Point animePoint;
        private bool afterimage;

        private int? initialEnemyHPSum;

        public DruidMnV3(IAudioEngine s)
          : base(s)
        {
            this.navi = true;
            this.number = 247;
            this.name = ShanghaiEXE.Translate("Chip.DruidManV3Name");
            this.element = ChipBase.ELEMENT.poison;
            this.power = 150;
            this.poisonPower = 60;
            this.subpower = 0;
            this.regsize = 66;
            this.reality = 5;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.D;
            this.code[1] = ChipFolder.CODE.A;
            this.code[2] = ChipFolder.CODE.D;
            this.code[3] = ChipFolder.CODE.A;
            var informationDialogue = NSGame.ShanghaiEXE.Translate("Chip.DruidManV3Desc");
            this.information[0] = informationDialogue[0];
            this.information[1] = informationDialogue[1];
            this.information[2] = informationDialogue[2];
            this.Init();
            this.burstWarnings = new List<Tuple<Point, int>>();
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
            this.animePoint = this.Animation(character.waittime);
            if (character.waittime == 0)
            {
                    character.animationpoint.X = -1;
                    this.sound.PlaySE(SoundEffect.warp);
                this.initialEnemyHPSum = battle.AllChara().Where(c => character.UnionEnemy == c.union).Sum(c => c.Hp);
            }
            else if (character.waittime >= AnimationTiming.First().Delay && character.waittime < AnimationTiming.TakeWhile(a => a.Row == 2 || a.Row == 0).Sum(a => a.Delay))
            {
                var burstAttackTime = character.waittime - AnimationTiming.First().Delay;
                this.burstWarnings.ForEach(bw =>
                {
                    if (bw.Item2 == burstAttackTime)
                    {
                        var burst = this.Paralyze(new Tower(
                            this.sound,
                            character.parent,
                            bw.Item1.X,
                            bw.Item1.Y,
                            character.union,
                            this.Power(character),
                            -1,
                            ChipBase.ELEMENT.poison));
                        character.parent.attacks.Add(burst);
                    }
                });
                if (burstAttackTime % BurstSpacing == 0)
                {
                    var columnProgress = (burstAttackTime / BurstSpacing) % 6;
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
                            true));
                    }
                }
            }
            else if (character.waittime >= AnimationTiming.TakeWhile(a => a.Row == 2 || a.Row == 0).Sum(a => a.Delay) && character.waittime < AnimationTiming.Sum(a => a.Delay))
            {
                if (this.initialEnemyHPSum != null)
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

                var waveAttackTime = character.waittime - AnimationTiming.TakeWhile(a => a.Row == 2 || a.Row == 0).Sum(a => a.Delay) - AnimationTiming.First().Delay;

                if (waveAttackTime >= WaveAttackDelay)
                {
                    var waveDelay = waveAttackTime - WaveAttackDelay;
                    if (waveDelay / (WaveSpeed * 2) < 6 && waveDelay % (WaveSpeed * 2) == 0)
                    {
                        var waveAttack = new DruidManWave(
                            this.sound,
                            character.parent,
                            character.rebirth ? 5 : 0,
                            character.position.Y,
                            character.union,
                            waveDelay / (WaveSpeed * 2) < 1 ? this.Power(character) / 2 : 0,
                            WaveSpeed,
                            this.element);
                        waveAttack.BadStatusSet(CharacterBase.BADSTATUS.poison, this.poisonPower * 8);
                        character.parent.attacks.Add(this.Paralyze(waveAttack));
                    }
                }
            }
            else if (character.waittime >= AnimationTiming.Sum(a => a.Delay) && character.waittime < AnimationTiming.Sum(a => a.Delay) + PostAttackDelay)
            {
                this.afterimage = true;
            }
            else if (character.waittime >= AnimationTiming.Sum(a => a.Delay) + PostAttackDelay && this.BlackOutEnd(character, battle))
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
                this._rect = new Rectangle(784, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic19", this._rect, true, p, Color.White);
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
                    dg.DrawImage(dg, "druidman", this._rect, false, this._position, player.union == Panel.COLOR.red, Color.White);
                }
                else if (player.waittime % 2 == 0)
                {
                    this._rect = new Rectangle(0, 0, FullFrameRect.Width, FullFrameRect.Height);
                    dg.DrawImage(dg, "druidman", this._rect, false, this._position, player.union == Panel.COLOR.red, Color.FromArgb(60, 0, 0));
                }
            }
            else
                this.BlackOutRender(dg, player.union);
        }
    }
}

