using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSAttack
{
    internal class PoisonShot : AttackBase
    {
        private readonly PoisonShot.TYPE type;
        private readonly int time;
        private Vector2 endposition;
        private readonly float movex;
        private readonly float movey;
        private float plusy;
        private float speedy;
        private readonly float plusing;
        private const int startspeed = 6;
        private new bool bright;
        private readonly bool cross;

        public PoisonShot(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele,
          Point end,
          int t,
          bool cross,
          PoisonShot.TYPE ty)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.cross = cross;
            this.hitting = false;
            this.speed = s;
            this.positionDirect = v;
            this.time = t;
            this.position = end;
            this.endposition = new Vector2(end.X * 40 + 20, end.Y * 24 + 80);
            this.movex = (v.X - this.endposition.X) / t;
            this.movey = (v.Y - this.endposition.Y) / t;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (this.time / 2);
            this.rebirth = this.union == Panel.COLOR.red;
            this.type = ty;
            if (this.type != PoisonShot.TYPE.poison)
                return;
            this.badstatus[5] = true;
            this.badstatustime[5] = 180;
        }

        public override void Updata()
        {
            if (this.frame % 5 == 0)
                this.bright = !this.bright;
            if (this.bright)
                this.PanelBright();
            if (this.frame == this.time)
            {
                this.hitting = true;
                this.flag = false;
                if (!this.StandPanel.Hole)
                {
                    List<Point> pointList = new List<Point>();
                    pointList.Add(this.position);
                    if (this.cross)
                    {
                        pointList.Add(new Point(this.position.X + 1, this.position.Y));
                        pointList.Add(new Point(this.position.X, this.position.Y + 1));
                        pointList.Add(new Point(this.position.X - 1, this.position.Y));
                        pointList.Add(new Point(this.position.X, this.position.Y - 1));
                    }
                    foreach (Point point in pointList)
                    {
                        if (this.InAreaCheck(point))
                        {
                            if (!this.parent.panel[point.X, point.Y].Hole)
                            {
                                Panel.PANEL panel = Panel.PANEL._nomal;
                                switch (this.type)
                                {
                                    case PoisonShot.TYPE.poison:
                                        panel = Panel.PANEL._poison;
                                        break;
                                    case PoisonShot.TYPE.break_:
                                        panel = Panel.PANEL._crack;
                                        break;
                                    case PoisonShot.TYPE.ice:
                                        panel = Panel.PANEL._ice;
                                        break;
                                    case PoisonShot.TYPE.grass:
                                        panel = Panel.PANEL._grass;
                                        break;
                                    case PoisonShot.TYPE.sand:
                                        panel = Panel.PANEL._sand;
                                        break;
                                }
                                this.parent.panel[point.X, point.Y].state = panel;
                            }
                            this.parent.effects.Add(new Elementhit(this.sound, this.parent, this.positionDirect, 2, point, this.element));
                        }
                    }
                }
            }
            else
            {
                this.positionDirect.X -= this.movex;
                this.positionDirect.Y -= this.movey;
                this.plusy += this.speedy;
                this.speedy -= this.plusing;
                if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y - this.plusy + Shake.Y);
            this._rect = new Rectangle(192, (int)this.type * 16, 16, 16);
            dg.DrawImage(dg, "poisorlin", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            return base.HitCheck(charaposition, charaunion);
        }

        public override bool HitCheck(Point charaposition)
        {
            return base.HitCheck(charaposition);
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.flag = false;
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.flag = false;
            return true;
        }

        public enum TYPE
        {
            poison,
            break_,
            ice,
            grass,
            sand,
        }
    }
}
