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
    internal class NapalmBomb : AttackBase
    {
        private readonly NapalmBomb.TYPE type;
        private readonly NapalmBomb.TYPE ctype;
        private readonly int time;
        private Vector2 endposition;
        private readonly float movex;
        private readonly float movey;
        private float plusy;
        private float speedy;
        private readonly float plusing;
        private const int startspeed = 6;
        private readonly int heattime;

        public NapalmBomb(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          Point end,
          int t,
          NapalmBomb.TYPE ty,
          int heattime)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            this.heattime = heattime;
            this.breaking = true;
            this.hitting = false;
            this.speed = s;
            this.positionDirect = v;
            this.time = t;
            this.position = end;
            this.invincibility = false;
            this.endposition = new Vector2(end.X * 40 + 20, end.Y * 24 + 80);
            this.movex = (v.X - this.endposition.X) / t;
            this.movey = (v.Y - this.endposition.Y) / t;
            this.plusy = 0.0f;
            this.speedy = 6f;
            this.plusing = this.speedy / (this.time / 2);
            this.rebirth = this.union == Panel.COLOR.red;
            this.type = ty;
        }

        public override void Updata()
        {
            if (this.frame % 5 == 0)
                this.bright = !this.bright;
            if (this.bright)
                this.PanelBright();
            if (this.frame == this.time - 2)
                this.hitting = true;
            if (this.frame == this.time)
            {
                this.hitting = false;
                this.flag = false;
                if (this.InArea && !this.StandPanel.Hole)
                {
                    this.sound.PlaySE(SoundEffect.heat);
                    switch (this.type)
                    {
                        case NapalmBomb.TYPE.single:
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.heattime));
                            break;
                        case NapalmBomb.TYPE.closs:
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X - 1, this.position.Y, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X + 1, this.position.Y, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.heattime));
                            break;
                        case NapalmBomb.TYPE.big:
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X - 1, this.position.Y - 1, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X, this.position.Y - 1, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X + 1, this.position.Y - 1, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X - 1, this.position.Y, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X + 1, this.position.Y, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X - 1, this.position.Y + 1, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X, this.position.Y + 1, this.union, this.power, 1, this.heattime));
                            this.parent.attacks.Add(new PanelHeat(this.sound, this.parent, this.position.X + 1, this.position.Y + 1, this.union, this.power, 1, this.heattime));
                            break;
                        case NapalmBomb.TYPE.fuhatu:
                            this.sound.PlaySE(SoundEffect.canon);
                            this.ShakeStart(5, 5);
                            this.parent.objects.Add(new FireNaparm(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.power, this.heattime));
                            break;
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
            this._rect = new Rectangle(16, 0, 16, 16);
            dg.DrawImage(dg, "bombs", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.sound.PlaySE(SoundEffect.canon);
            this.flag = false;
            this.ShakeStart(5, 5);
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;
            this.sound.PlaySE(SoundEffect.canon);
            this.ShakeStart(5, 5);
            this.flag = false;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.sound.PlaySE(SoundEffect.canon);
            this.flag = false;
            this.ShakeStart(5, 5);
            if (p.Element == ChipBase.ELEMENT.heat && this.type == NapalmBomb.TYPE.fuhatu)
            {
                int x = Eriabash.SteelX(this, this.parent);
                this.sound.PlaySE(SoundEffect.bombbig);
                this.ShakeStart(4, 90);
                this.parent.effects.Add(new RandomBomber(this.sound, this.parent, Bomber.BOMBERTYPE.flashbomber, 2, new Point(x, 0), new Point(6, 2), this.union, 36));
                for (int pX = 0; pX < this.parent.panel.GetLength(0); ++pX)
                {
                    for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                    {
                        if (this.parent.panel[pX, pY].color == this.UnionEnemy)
                            this.parent.attacks.Add(new BombAttack(this.sound, this.parent, pX, pY, this.union, this.power, 1, ChipBase.ELEMENT.normal));
                    }
                }
            }
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.sound.PlaySE(SoundEffect.canon);
            this.flag = false;
            this.ShakeStart(5, 5);
            if (e.Element == ChipBase.ELEMENT.heat && this.type == NapalmBomb.TYPE.fuhatu)
            {
                int x = Eriabash.SteelX(this, this.parent);
                this.sound.PlaySE(SoundEffect.bombbig);
                this.ShakeStart(4, 90);
                this.parent.effects.Add(new RandomBomber(this.sound, this.parent, Bomber.BOMBERTYPE.flashbomber, 2, new Point(x, 0), new Point(6, 2), this.union, 36));
                for (int pX = 0; pX < this.parent.panel.GetLength(0); ++pX)
                {
                    for (int pY = 0; pY < this.parent.panel.GetLength(1); ++pY)
                    {
                        if (this.parent.panel[pX, pY].color == this.UnionEnemy)
                            this.parent.attacks.Add(new BombAttack(this.sound, this.parent, pX, pY, this.union, this.power, 1, ChipBase.ELEMENT.normal));
                    }
                }
            }
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.sound.PlaySE(SoundEffect.canon);
            this.flag = false;
            this.ShakeStart(5, 5);
            return true;
        }

        public enum TYPE
        {
            single,
            closs,
            big,
            fuhatu,
        }
    }
}
