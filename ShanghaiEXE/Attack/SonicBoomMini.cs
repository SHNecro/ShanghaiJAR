﻿using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using SlimDX;
using System.Drawing;

namespace NSAttack
{
    internal class SonicBoomMini : AttackBase
    {
        private readonly bool penetration;

        public SonicBoomMini(
          MyAudio so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele,
          bool par)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.penetration = par;
            if (this.penetration)
                this.power *= 2;
            this.upprint = false;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 10, this.position.Y * 24 + 56);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 10, this.position.Y * 24 + 56);
            this.frame = 0;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            this.PanelBright();
            this.positionDirect.X += this.union == Panel.COLOR.red ? speed : -this.speed;
            this.position = this.Calcposition(this.positionDirect, 32, false);
            if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                this.flag = false;
        }

        public override void Render(IRenderer dg)
        {
            if (!this.flag)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2 + 6.0 - 10 * this.UnionRebirth;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            this._rect = new Rectangle(this.animationpoint.X * 48, 0, 48, 32);
            switch (this.element)
            {
                case ChipBase.ELEMENT.heat:
                    this.color = Color.FromArgb(byte.MaxValue, byte.MaxValue, 55, 55);
                    break;
                case ChipBase.ELEMENT.aqua:
                    this.color = Color.FromArgb(byte.MaxValue, 50, 155, byte.MaxValue);
                    break;
                case ChipBase.ELEMENT.eleki:
                    this.color = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
                    break;
                case ChipBase.ELEMENT.leaf:
                    this.color = Color.FromArgb(byte.MaxValue, 155, byte.MaxValue, 55);
                    break;
                case ChipBase.ELEMENT.poison:
                    this.color = Color.FromArgb(byte.MaxValue, 100, 50, byte.MaxValue);
                    break;
                case ChipBase.ELEMENT.earth:
                    this.color = Color.FromArgb(byte.MaxValue, 160, 100, 50);
                    break;
                default:
                    this.color = Color.White;
                    break;
            }
            dg.DrawImage(dg, "sword", this._rect, true, this._position, this.rebirth, !this.penetration ? this.color : Color.Yellow);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            if (!this.penetration)
                this.flag = false;
            this.parent.effects.Add(new Basterhit(this.sound, this.parent, charaposition.X, charaposition.Y, 2));
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            if (!this.penetration)
                this.flag = false;
            this.parent.effects.Add(new Basterhit(this.sound, this.parent, charaposition.X, charaposition.Y, 2));
            return true;
        }

        public override bool HitEvent(Player p)
        {
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            if (!this.penetration)
                this.flag = false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            if (!this.penetration)
                this.flag = false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
