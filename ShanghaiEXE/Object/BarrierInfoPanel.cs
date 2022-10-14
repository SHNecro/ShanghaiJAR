using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;
using NSChip;
using System.Collections.Generic;
using System;
using NSEffect;
using System.Linq;

namespace NSObject
{
    internal class BarrierInfoPanel : ObjectBase
    {
        private static Rectangle PanelTextureRect = new Rectangle(200, 169, 32, 20);
        private static Rectangle ElementRingTextureRect = new Rectangle(200, 189, 16, 16);

        private static Dictionary<ChipBase.ELEMENT, Point> ElementTexturePoints = new Dictionary<ChipBase.ELEMENT, Point>
        {
            { ChipBase.ELEMENT.normal, new Point(0, 0) },
            { ChipBase.ELEMENT.heat, new Point(1, 0) },
            { ChipBase.ELEMENT.aqua, new Point(2, 1) },
            { ChipBase.ELEMENT.eleki, new Point(2, 0) },
            { ChipBase.ELEMENT.leaf, new Point(1, 1) },
            { ChipBase.ELEMENT.poison, new Point(3, 1) },
            { ChipBase.ELEMENT.earth, new Point(3, 0) }
        };

        private static Dictionary<ChipBase.ELEMENT, Tuple<Vector2, Color>> ElementTextLocations = new Dictionary<ChipBase.ELEMENT, Tuple<Vector2, Color>>
        {
            { ChipBase.ELEMENT.normal, Tuple.Create(new Vector2(106, 48 + 18 + 18), Color.White) },
            { ChipBase.ELEMENT.heat, Tuple.Create(new Vector2(106 - 28, 48 - 18), Color.FromArgb(240, 136, 0)) },
            { ChipBase.ELEMENT.aqua, Tuple.Create(new Vector2(106 - 28, 48 + 18), Color.FromArgb(88, 128, 248)) },
            { ChipBase.ELEMENT.eleki, Tuple.Create(new Vector2(106 + 28, 48 + 18), Color.FromArgb(248, 240, 50)) },
            { ChipBase.ELEMENT.leaf, Tuple.Create(new Vector2(106 + 28, 48 - 18), Color.FromArgb(56, 224, 144)) },
            { ChipBase.ELEMENT.poison, Tuple.Create(new Vector2(106 + 52, 48), Color.FromArgb(176, 65, 241)) },
            { ChipBase.ELEMENT.earth, Tuple.Create(new Vector2(106 - 52, 48), Color.FromArgb(214, 162, 24)) }
        };

        private Func<ChipBase.ELEMENT, int> elementAmountFunc;

        private bool breaking;

        private ScreenBlack screenGreyOut;
        private List<ScreenObjectFade> infoText;

        private Point panelPosition;

        public BarrierInfoPanel(
          IAudioEngine s,
          SceneBattle p,
          Panel.COLOR union,
          int pX,
          int pY,
          Func<ChipBase.ELEMENT, int> elementLightUpFunc)
          : base(s, p, 5, 2, union)
        {
            this.height = 128;
            this.wide = 96;
            this.hp = 10;
            this.hpmax = this.hp;
            this.hitbreak = false;
            this.unionhit = false;
            this.overslip = false;
            this.Noslip = true;
            this.effecting = true;
            this.downprint = true;
            this.rebirth = this.union == Panel.COLOR.red;
            this.invincibilitytime = -1;
            this.invincibility = true;
            this.guard = CharacterBase.GUARD.noDamage;
            this.panelPosition = new Point(pX, pY);
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 83);
            this.nohit = true;

            this.elementAmountFunc = elementLightUpFunc;
            this.infoText = new List<ScreenObjectFade>();
            this.blackOutObject = true;
        }

        public bool? ForcedShowState { get; set; }

        public bool AllowedToBreak { get; set; }

        public override void Updata()
        {
            if (!this.breaking)
            {
                this.hp = 99999;
                if (this.moveflame)
                {
                    switch ((this.frame / 6) % 4)
                    {
                        case 0:
                            this.animationpoint.X = 0;
                            break;
                        case 1:
                            this.animationpoint.X = 1;
                            break;
                        case 2:
                            this.animationpoint.X = 2;
                            break;
                        case 3:
                            this.animationpoint.X = 1;
                            break;
                    }
                }
                this.FlameControl(4);

                if (this.ForcedShowState ?? this.parent.player.position == this.panelPosition)
                {
                    if (this.screenGreyOut == null)
                    {
                        this.screenGreyOut = new ScreenBlack(
                            this.sound,
                            this.parent,
                            Vector2.Zero,
                            new Point(5, 2),
                            ChipBase.ELEMENT.normal,
                            0,
                            false,
                            Color.FromArgb(96, Color.Black),
                            6);
                        this.screenGreyOut.downprint = false;
                        this.parent.effects.Add(this.screenGreyOut);

                        foreach (var elementEntry in ElementTextLocations)
                        {
                            var hidden = elementEntry.Key != ChipBase.ELEMENT.normal && this.elementAmountFunc(elementEntry.Key) <= 0;

                            var elementIndicator = new ScreenObjectFade(
                                this.sound,
                                this.parent,
                                "bomber",
                                () => new Rectangle(0 + 32 * (this.frame % 3), 805, 32, 32),
                                elementEntry.Value.Item1 - new Vector2(8, 7),
                                new Point(5, 2),
                                0,
                                false,
                                () =>
                                {
                                    hidden = elementEntry.Key != ChipBase.ELEMENT.normal && this.elementAmountFunc(elementEntry.Key) <= 0;
                                    if (elementEntry.Key == ChipBase.ELEMENT.normal || this.elementAmountFunc(elementEntry.Key) > 0)
                                    {
                                        return Color.Transparent;
                                    }

                                    // 0: None, 1: part of cancel trio, 2: is last cancelled part of trio, 3: is last cancelling part of trio
                                    var hintState = 0;
                                    var cancellingElements = GetEffectiveElements(elementEntry.Key);
                                    var elementsToBeCancelled = ElementTextLocations.Keys.Where(e =>
                                    {
                                        var elementHasDamageToBeCancelled = e != ChipBase.ELEMENT.normal && this.elementAmountFunc(e) > 0;
                                        var elementCancellingElements = GetEffectiveElements(e);
                                        var elementOnlyNeedsThis = elementCancellingElements.All(ee => ee == elementEntry.Key || this.elementAmountFunc(ee) > 0);
                                        return elementHasDamageToBeCancelled && elementOnlyNeedsThis;
                                    });

                                    if (cancellingElements.Any(e => this.elementAmountFunc(e) > 0))
                                    {
                                        hintState = 1;
                                    }
                                    if (cancellingElements.All(e => this.elementAmountFunc(e) > 0))
                                    {
                                        hintState = 2;
                                    }
                                    else if (elementsToBeCancelled.Any())
                                    {
                                        hintState = 3;
                                    }

                                    hidden &= hintState == 0;
                                    
                                    switch (hintState)
                                    {
                                        default:
                                        case 0:
                                            return Color.Transparent;
                                        case 1:
                                            // By modulation, blue
                                            return Color.FromArgb(128, Color.White);
                                        case 2:
                                            // By modulation, green
                                            return Color.Yellow;
                                        case 3:
                                            // By modulation, green
                                            //return Color.White;
                                            return Color.Yellow;
                                    }
                                },
                                8);
                            this.parent.effects.Add(elementIndicator);
                            this.infoText.Add(elementIndicator);

                            var elementIcon = new ScreenObjectFade(
                                this.sound,
                                this.parent,
                                "battleobjects",
                                new Rectangle(216 + 16 * (int)elementEntry.Key, 88, 16, 16),
                                elementEntry.Value.Item1,
                                new Point(5, 2),
                                0,
                                false,
                                () => 
                                {
                                    if (hidden)
                                    {
                                        return Color.Transparent;
                                    }

                                    return Color.White;
                                },
                                8);
                            this.parent.effects.Add(elementIcon);
                            this.infoText.Add(elementIcon);

                            var elementText = new ScreenObjectFade(
                                this.sound,
                                this.parent,
                                () => this.elementAmountFunc(elementEntry.Key).ToString(),
                                elementEntry.Value.Item1 + new Vector2(16 + 2 - 1, 0 - 1),
                                new Point(5, 2),
                                0,
                                false,
                                () =>
                                {
                                    if (hidden)
                                    {
                                        return Color.Transparent;
                                    }

                                    return elementEntry.Value.Item2;
                                },
                                8);
                            this.parent.effects.Add(elementText);
                            this.infoText.Add(elementText);
                        }

                        var totalDamageText = new ScreenObjectFade(
                            this.sound,
                            this.parent,
                            () => ElementTextLocations.Select(kvp => this.elementAmountFunc(kvp.Key)).Sum().ToString(),
                            new Vector2(106, 48) + new Vector2(16 + 2 - 1 - 8, 0 - 1),
                                new Point(5, 2),
                            0,
                            false,
                            Color.Red,
                            8);
                        this.parent.effects.Add(totalDamageText);
                        this.infoText.Add(totalDamageText);
                    }
                }
                else
                {
                    if (this.screenGreyOut != null)
                    {
                        this.screenGreyOut.end = true;
                        this.screenGreyOut = null;

                        foreach (var elementInfoText in this.infoText)
                        {
                            elementInfoText.end = true;
                        }
                        this.infoText.Clear();
                    }
                }
            }
            else
            {
                var fadeProgress = this.frame / 60.0;
                this.color = Color.FromArgb((int)Math.Max(0, 255 - fadeProgress * 255), Color.White);

                this.FlameControl(1);

                if (this.frame > 60)
                {
                    this.flag = false;
                }
            }

            base.Updata();
        }

        public override void Break()
        {
            if (!this.AllowedToBreak)
            {
                return;
            }

            this.frame = 0;
            this.breaking = true;

            if (this.screenGreyOut != null)
            {
                this.screenGreyOut.end = true;
                this.screenGreyOut = null;

                foreach (var elementInfoText in this.infoText)
                {
                    elementInfoText.end = true;
                }
                this.infoText.Clear();
            }
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * PanelTextureRect.Width + PanelTextureRect.X, PanelTextureRect.Y, PanelTextureRect.Width, PanelTextureRect.Height);
            this._position = new Vector2(positionDirect.X + Shake.X, positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "heavenbarrier", this._rect, false, this._position, this.rebirth, this.color);

            foreach (var kvp in ElementTexturePoints)
            {
                if (kvp.Key == ChipBase.ELEMENT.normal || this.elementAmountFunc.Invoke(kvp.Key) > 0)
                {
                    this._rect = new Rectangle(
                        kvp.Value.X * ElementRingTextureRect.Width + ElementRingTextureRect.X,
                        kvp.Value.Y * ElementRingTextureRect.Height + ElementRingTextureRect.Y,
                        ElementRingTextureRect.Width,
                        ElementRingTextureRect.Height);
                    this._position = new Vector2(positionDirect.X + Shake.X, positionDirect.Y + Shake.Y);
                    dg.DrawImage(dg, "heavenbarrier", this._rect, false, this._position, this.rebirth, this.color);
                }
            }
        }

        private static ChipBase.ELEMENT[] GetEffectiveElements(ChipBase.ELEMENT elem)
        {
            switch (elem)
            {
                case ChipBase.ELEMENT.heat:
                    return new[] { ChipBase.ELEMENT.aqua, ChipBase.ELEMENT.earth };
                case ChipBase.ELEMENT.aqua:
                    return new[] { ChipBase.ELEMENT.eleki, ChipBase.ELEMENT.poison };
                case ChipBase.ELEMENT.eleki:
                    return new[] { ChipBase.ELEMENT.leaf, ChipBase.ELEMENT.earth };
                case ChipBase.ELEMENT.leaf:
                    return new[] { ChipBase.ELEMENT.heat, ChipBase.ELEMENT.poison };
                case ChipBase.ELEMENT.poison:
                    return new[] { ChipBase.ELEMENT.heat, ChipBase.ELEMENT.eleki };
                case ChipBase.ELEMENT.earth:
                    return new[] { ChipBase.ELEMENT.aqua, ChipBase.ELEMENT.leaf };
                default:
                    return new ChipBase.ELEMENT[0];
            }
        }
    }
}
