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

namespace NSAttack
{
    internal class Wheel : AttackBase
    {
        private bool movestart = false;
        private Wheel.Mothion mothion;
        private const int plusy = 70;
        private readonly int movespeed;
        private int movemany;

        public Wheel(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele,
          int movespeed = 4)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.movespeed = movespeed;
            this.speed = s;
            this.positionDirect = v;
            this.positionDirect.Y += 8f;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.frame >= 3 && !this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
            }
            if (this.moveflame)
            {
                if (this.frame % 2 == 0)
                {
                    ++this.animationpoint.X;
                    if (this.animationpoint.X > 1)
                        this.animationpoint.X = 0;
                }
                switch (this.mothion)
                {
                    case Wheel.Mothion.go:
                        this.positionDirect.X += this.union == Panel.COLOR.red ? movespeed : -this.movespeed;
                        this.movemany += this.movespeed;
                        if (this.movemany % 40 == 0)
                        {
                            foreach (CharacterBase characterBase in this.parent.AllHitter())
                            {
                                if (characterBase.position.X == this.position.X && characterBase.union == this.UnionEnemy && !characterBase.nohit)
                                {
                                    if (characterBase.position.Y > this.position.Y)
                                        this.mothion = Wheel.Mothion.down;
                                    else if (characterBase.position.Y < this.position.Y)
                                        this.mothion = Wheel.Mothion.up;
                                }
                            }
                            break;
                        }
                        break;
                    case Wheel.Mothion.up:
                        this.positionDirect.Y -= movespeed;
                        break;
                    case Wheel.Mothion.down:
                        this.positionDirect.Y += movespeed;
                        break;
                }
                if (this.mothion != Wheel.Mothion.go) { }
                this.position = this.Calcposition(this.positionDirect, 36, false);
                if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2((float)(positionDirect.X + (double)this.Shake.X + 8.0), (float)(positionDirect.Y + (double)this.Shake.Y + 4.0));
            this._rect = new Rectangle(72 + this.animationpoint.X * 32, 920, 32, 48);
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, Color.White);
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
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, ChipBase.ELEMENT.aqua));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, ChipBase.ELEMENT.aqua));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, ChipBase.ELEMENT.aqua));
            return true;
        }

        private enum Mothion
        {
            go,
            up,
            down,
        }
    }
}
