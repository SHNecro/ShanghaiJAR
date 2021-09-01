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

        private Func<ChipBase.ELEMENT, int> elementAmountFunc;

        private bool breaking;

        private ScreenBlack screenGreyOut;

        public BarrierInfoPanel(
          IAudioEngine s,
          SceneBattle p,
          Panel.COLOR union,
          Func<ChipBase.ELEMENT, int> elementLightUpFunc)
          : base(s, p, 5, 3, union)
        {
            var pX = 0;
            var pY = 1;

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
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 83);

            this.elementAmountFunc = elementLightUpFunc;
        }

        public override void Updata()
        {
            if (!breaking)
            {
                if (this.moveflame)
                {
                    switch (this.frame)
                    {
                        case 1:
                            this.animationpoint.X = 0;
                            break;
                        case 2:
                            this.animationpoint.X = 1;
                            break;
                        case 3:
                            this.animationpoint.X = 2;
                            break;
                        case 4:
                            this.animationpoint.X = 1;
                            this.frame = 0;
                            break;
                    }
                }
                this.FlameControl(24);

                if (this.parent.player.position == new Point(0, 1))
                {
                    if (this.screenGreyOut == null)
                    {
                        this.screenGreyOut = new ScreenBlack(this.sound, this.parent, Vector2.Zero, new Point(5, 2), ChipBase.ELEMENT.normal, 0, false, Color.FromArgb(96, Color.Black), 6);
                        this.screenGreyOut.downprint = false;
                        this.parent.effects.Add(this.screenGreyOut);
                    }
                }
                else
                {
                    if (this.screenGreyOut != null)
                    {
                        this.screenGreyOut.end = true;
                        this.screenGreyOut = null;
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
            this.frame = 0;
            this.breaking = true;

            if (this.screenGreyOut != null)
            {
                this.screenGreyOut.end = true;
                this.screenGreyOut = null;
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
    }
}
