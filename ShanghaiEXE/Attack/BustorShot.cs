using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using System.Collections.Generic;
using System.Drawing;

namespace NSAttack
{
    internal class BustorShot : AttackBase
    {
        private readonly BustorShot.SHOT shot;
        private readonly bool nopoweup;
        private bool seedbomb;

        public BustorShot(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          BustorShot.SHOT s,
          ChipBase.ELEMENT ele,
          bool bre,
          int speed = 0)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.speed = speed;
            this.breaking = bre;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.shot = s;
            this.frame = 0;
            this.positionDirect.Y = 82 + 24 * this.position.Y;
            if (s != BustorShot.SHOT.canon && s != BustorShot.SHOT.railgun && s != BustorShot.SHOT.reflect && s != BustorShot.SHOT.seedcanon)
            {
                this.canCounter = false;
                this.invincibility = false;
                this.knock = false;
            }
            else
                this.knock = true;
            if (this.shot == BustorShot.SHOT.reflect)
                this.parent.effects.Add(new NormalChargehit(this.sound, this.parent, this.position.X, this.position.Y, 2));
            if (this.shot == BustorShot.SHOT.ranShot)
            {
                this.parent.effects.Add(new NormalChargehit(this.sound, this.parent, this.position.X, this.position.Y, 2));
                this.knock = true;
                this.invincibility = true;
                this.bright = true;
            }
        }

        public override void Updata()
        {
            if (this.over)
                return;
            this.positionDirect.X = 40 * this.position.X + 20;
            if (this.moveflame)
            {
                if (this.position.X <= 5 && this.position.X >= 0)
                {
                    if (this.shot != BustorShot.SHOT.railgun) { }
                    if (this.shot == BustorShot.SHOT.reflect)
                    {
                        this.sound.PlaySE(SoundEffect.chain);
                        this.parent.effects.Add(new NormalChargehit(this.sound, this.parent, this.position.X, this.position.Y, 2));
                    }
                    int x = this.position.X;
                    this.position.X = this.union == Panel.COLOR.red ? this.position.X + 1 : this.position.X - 1;
                    if (this.shot == BustorShot.SHOT.railgun)
                    {
                        this.parent.effects.Add(new Elementhit(this.sound, this.parent, this.position.X, this.position.Y, 1, ChipBase.ELEMENT.eleki));
                        this.parent.panel[x, this.position.Y].Crack();
                    }
                    if (this.shot == BustorShot.SHOT.ranShot)
                        this.parent.effects.Add(new NormalChargehit(this.sound, this.parent, this.position.X, this.position.Y, 2));
                }
            }
            if (!this.InArea)
                this.flag = false;
            if (this.shot == BustorShot.SHOT.seedcanon && (this.position.X >= 5 || this.position.X <= 0))
                this.SeedCanon();
            this.PanelBright(this.bright);
            this.FlameControl();
        }

        private void SeedCanon()
        {
            if (this.seedbomb)
                return;
            this.ShakeStart(5, 30);
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 1));
            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.element))));
            this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y + 1, Bomber.BOMBERTYPE.bomber, 1));
            this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y - 1, Bomber.BOMBERTYPE.bomber, 2));
            this.parent.attacks.Add(this.StateCopy(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.element))));
            foreach (Point poji in new List<Point>()
      {
        this.position,
        new Point(this.position.X, this.position.Y),
        new Point(this.position.X, this.position.Y + 1),
        new Point(this.position.X, this.position.Y - 1)
      })
            {
                if (this.InAreaCheck(poji) && !this.parent.panel[poji.X, poji.Y].Hole)
                {
                    if (this.element == ChipBase.ELEMENT.leaf)
                    {
                        Panel.PANEL panel = Panel.PANEL._grass;
                        this.parent.panel[poji.X, poji.Y].state = panel;
                    }
                }
            }
            this.seedbomb = true;
        }

        public override void Render(IRenderer dg)
        {
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!this.flag || !base.HitCheck(charaposition, charaunion))
                return false;
            if (this.shot != BustorShot.SHOT.reflect)
                this.flag = false;
            switch (this.shot)
            {
                case BustorShot.SHOT.bustor:
                    this.parent.effects.Add(new Basterhit(this.sound, this.parent, this.position.X, this.position.Y, 2));
                    break;
                case BustorShot.SHOT.normalcharge:
                    this.parent.effects.Add(new NormalChargehit(this.sound, this.parent, this.position.X, this.position.Y, 1));
                    break;
                case BustorShot.SHOT.canon:
                    this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 1));
                    break;
                case BustorShot.SHOT.seedcanon:
                    this.SeedCanon();
                    break;
                case BustorShot.SHOT.railgun:
                    this.ShakeStart(5, 5);
                    this.sound.PlaySE(SoundEffect.breakObject);
                    this.parent.effects.Add(new Elementhit(this.sound, this.parent, this.position.X, this.position.Y, 1, ChipBase.ELEMENT.eleki));
                    break;
            }
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!this.flag || !base.HitCheck(charaposition))
                return false;
            if (this.shot != BustorShot.SHOT.reflect)
                this.flag = false;
            switch (this.shot)
            {
                case BustorShot.SHOT.bustor:
                    this.parent.effects.Add(new Basterhit(this.sound, this.parent, this.position.X, this.position.Y, 2));
                    break;
                case BustorShot.SHOT.normalcharge:
                    this.parent.effects.Add(new NormalChargehit(this.sound, this.parent, this.position.X, this.position.Y, 1));
                    break;
                case BustorShot.SHOT.canon:
                    this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 1));
                    break;
                case BustorShot.SHOT.seedcanon:
                    this.SeedCanon();
                    break;
                case BustorShot.SHOT.railgun:
                    this.ShakeStart(5, 5);
                    this.sound.PlaySE(SoundEffect.breakObject);
                    this.parent.effects.Add(new Elementhit(this.sound, this.parent, this.position.X, this.position.Y, 1, ChipBase.ELEMENT.eleki));
                    break;
            }
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (this.shot != BustorShot.SHOT.railgun) { }
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (this.shot != BustorShot.SHOT.railgun) { }
            return base.HitEvent(e);
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (this.shot != BustorShot.SHOT.railgun) { }
            return base.HitEvent(o);
        }

        public enum SHOT
        {
            bustor,
            normalcharge,
            canon,
            seedcanon,
            railgun,
            reflect,
            ranShot,
        }
    }
}
