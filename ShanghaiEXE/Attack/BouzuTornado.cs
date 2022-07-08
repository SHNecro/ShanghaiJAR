using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;
using System;

namespace NSAttack
{
    internal class BouzuTornado : AttackBase
    {
        private const int hit = 8;
        private readonly bool outscreen;
        private readonly int movespeed;
        private readonly int roop;
        private readonly int roopcount;
        private readonly bool panelTrail;

        private readonly Func<BouzuTornado, Point> targetingFunc;
        private int count;
        private bool move;
        private int angle;


        public BouzuTornado(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele,
          int s,
          int movespeed,
          int movecount,
          bool panelTrail = true,
          Func<BouzuTornado, Point> targetingFunc = null)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.roop = movecount;
            this.invincibility = false;
            this.movespeed = movespeed;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.effecting = true;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40, this.position.Y * 24 + 48);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 48);
            this.frame = 0;
            this.sound.PlaySE(SoundEffect.shoot);
            if (this.union == Panel.COLOR.red)
                movespeed *= -1;

            this.panelTrail = panelTrail;
            this.targetingFunc = targetingFunc;
        }

        public override void PositionDirectSet()
        {
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40, this.position.Y * 24 + 48);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 48);
        }

        public override void InitAfter()
        {
            Point point = this.targetingFunc?.Invoke(this) ?? this.RandomTarget();
            if (point.Y == this.position.Y && point.X == this.position.X)
                this.angle = 5;
            if (point.Y < this.position.Y)
            {
                if (this.InAreaCheck(new Point(this.position.X, this.position.Y - 1)))
                    this.angle = 2;
                else
                    this.angle = 5;
            }
            else if (point.Y > this.position.Y)
            {
                if (this.InAreaCheck(new Point(this.position.X, this.position.Y + 1)))
                    this.angle = 3;
                else
                    this.angle = 5;
            }
            else if (point.X > this.position.X)
            {
                if (this.InAreaCheck(new Point(this.position.X + 1, this.position.Y)))
                {
                    if (this.union == Panel.COLOR.blue)
                        this.angle = 1;
                    else
                        this.angle = 0;
                }
                else
                    this.angle = 5;
            }
            else
            {
                if (point.X >= this.position.X)
                    return;
                this.angle = !this.InAreaCheck(new Point(this.position.X - 1, this.position.Y)) ? 5 : (this.union != Panel.COLOR.blue ? 1 : 0);
            }
        }

        public override void Updata()
        {
            if (this.panelTrail)
            {
                if (this.InArea && !this.StandPanel.Hole)
                {
                    switch (this.element)
                    {
                        case ChipBase.ELEMENT.heat:
                            this.StandPanel.State = Panel.PANEL._burner;
                            break;
                        case ChipBase.ELEMENT.aqua:
                            this.StandPanel.State = Panel.PANEL._ice;
                            break;
                        case ChipBase.ELEMENT.eleki:
                            this.StandPanel.State = Panel.PANEL._thunder;
                            break;
                        case ChipBase.ELEMENT.leaf:
                            this.StandPanel.State = Panel.PANEL._grass;
                            break;
                        case ChipBase.ELEMENT.poison:
                            this.StandPanel.State = Panel.PANEL._poison;
                            break;
                        case ChipBase.ELEMENT.earth:
                            this.StandPanel.State = Panel.PANEL._sand;
                            break;
                    }
                }
            }
            else
            {
                if (this.hitting)
                    this.PanelBright();
            }

            if (this.move)
            {
                if (this.angle < 6)
                {
                    if (this.SlideMove(movespeed, this.angle))
                    {
                        this.SlideMoveEnd();
                        this.PositionDirectSet();
                        this.move = false;
                        this.count = 0;
                        Point point = this.targetingFunc?.Invoke(this) ?? this.RandomTarget();
                        if (point.Y == this.position.Y && point.X == this.position.X)
                            this.angle = 5;
                        if (point.Y < this.position.Y && this.InAreaCheck(new Point(this.position.X, this.position.Y - 1)))
                            this.angle = 2;
                        else if (point.Y > this.position.Y && this.InAreaCheck(new Point(this.position.X, this.position.Y + 1)))
                            this.angle = 3;
                        else if (point.X > this.position.X && this.InAreaCheck(new Point(this.position.X + 1, this.position.Y)))
                            this.angle = this.union != Panel.COLOR.blue ? 0 : 1;
                        else if (point.X < this.position.X && this.InAreaCheck(new Point(this.position.X - 1, this.position.Y)))
                            this.angle = this.union != Panel.COLOR.blue ? 1 : 0;
                        else
                            this.angle = 5;
                    }
                }
                else
                {
                    this.PositionDirectSet();
                    this.move = false;
                    this.count = 0;
                }
            }
            if (this.moveflame)
            {
                ++this.animationpoint.X;
                if (this.animationpoint.X >= 6)
                {
                    this.animationpoint.X = 0;
                    ++this.count;
                    if (this.count > this.roop && !this.move)
                    {
                        this.count = 0;
                        this.move = true;
                    }
                }
            }
            this.FlameControl(2);
            base.Updata();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
                return;
            double x1 = positionDirect.X + 4 * this.UnionRebirth;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double num2 = positionDirect.Y - 8.0;
            shake = this.Shake;
            double y = shake.Y;
            double num3 = num2 + y;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle(this.animationpoint.X * 32, 48 * (int)this.Element, 32, 48);
            dg.DrawImage(dg, "tornado", this._rect, true, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            this.flag = false;
            switch (this.element)
            {
                case ChipBase.ELEMENT.heat:
                    this.StandPanel.State = Panel.PANEL._burner;
                    break;
                case ChipBase.ELEMENT.aqua:
                    this.StandPanel.State = Panel.PANEL._ice;
                    break;
                case ChipBase.ELEMENT.eleki:
                    this.StandPanel.State = Panel.PANEL._thunder;
                    break;
                case ChipBase.ELEMENT.leaf:
                    this.StandPanel.State = Panel.PANEL._grass;
                    break;
                case ChipBase.ELEMENT.poison:
                    this.StandPanel.State = Panel.PANEL._poison;
                    break;
                case ChipBase.ELEMENT.earth:
                    this.StandPanel.State = Panel.PANEL._sand;
                    break;
            }
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            this.flag = false;
            switch (this.element)
            {
                case ChipBase.ELEMENT.heat:
                    this.StandPanel.State = Panel.PANEL._burner;
                    break;
                case ChipBase.ELEMENT.aqua:
                    this.StandPanel.State = Panel.PANEL._ice;
                    break;
                case ChipBase.ELEMENT.eleki:
                    this.StandPanel.State = Panel.PANEL._thunder;
                    break;
                case ChipBase.ELEMENT.leaf:
                    this.StandPanel.State = Panel.PANEL._grass;
                    break;
                case ChipBase.ELEMENT.poison:
                    this.StandPanel.State = Panel.PANEL._poison;
                    break;
                case ChipBase.ELEMENT.earth:
                    this.StandPanel.State = Panel.PANEL._sand;
                    break;
            }
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            switch (this.element)
            {
                case ChipBase.ELEMENT.heat:
                    this.StandPanel.State = Panel.PANEL._burner;
                    break;
                case ChipBase.ELEMENT.aqua:
                    this.StandPanel.State = Panel.PANEL._ice;
                    break;
                case ChipBase.ELEMENT.eleki:
                    this.StandPanel.State = Panel.PANEL._thunder;
                    break;
                case ChipBase.ELEMENT.leaf:
                    this.StandPanel.State = Panel.PANEL._grass;
                    break;
                case ChipBase.ELEMENT.poison:
                    this.StandPanel.State = Panel.PANEL._poison;
                    break;
                case ChipBase.ELEMENT.earth:
                    this.StandPanel.State = Panel.PANEL._sand;
                    break;
            }
            return true;
        }
    }
}
