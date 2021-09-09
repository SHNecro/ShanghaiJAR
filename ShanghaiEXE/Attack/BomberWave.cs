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
    internal class BomberWave : AttackBase
    {
        private readonly int nextmake;
        private readonly int panelchange;
        private readonly Bomber.BOMBERTYPE type;

        public BomberWave(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          int panelchange,
          Bomber.BOMBERTYPE type,
          ChipBase.ELEMENT ele)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.type = type;
            this.panelchange = panelchange;
            this.nextmake = s;
            this.speed = 4;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 5, this.position.Y * 24 + 42);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 5, this.position.Y * 24 + 42);
            this.frame = 0;
            if (this.parent.panel[this.position.X, this.position.Y].Hole)
                this.flag = false;
            else
                this.sound.PlaySE(SoundEffect.shotwave);
            switch (panelchange)
            {
                case 1:
                    this.StandPanel.state = Panel.PANEL._sand;
                    break;
                case 2:
                    this.StandPanel.Crack();
                    break;
            }
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hitting)
                this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.frame == 13)
                    this.flag = false;
                if (this.frame == this.nextmake)
                {
                    this.hitting = false;
                    this.positionre.X = this.union == Panel.COLOR.red ? this.position.X + 1 : this.position.X - 1;
                    if (this.positionre.X >= 0
                        && this.positionre.X < 6
                        && this.positionre.Y >= 0
                        && this.positionre.Y < 3
                        && (!this.parent.panel[this.positionre.X, this.positionre.Y].Hole))
                        this.parent.attacks.Add(this.StateCopy(new WaveAttsck(this.sound, this.parent, this.positionre.X, this.positionre.Y, this.union, !this.badstatus[1] ? this.power : this.power / 2, this.nextmake, this.panelchange, this.element)));
                    this.positionre = this.position;
                }
                if (this.union == Panel.COLOR.red)
                    this.positionDirect = new Vector2(this.position.X * 40 - 5, this.position.Y * 24 + 42);
                else
                    this.positionDirect = new Vector2((this.position.X + 1) * 40 + 5, this.position.Y * 24 + 42);
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.over || !this.flag)
                return;
            this._rect = new Rectangle(this.animationpoint.X * 48, this.animationpoint.Y * 48, 48, 48);
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, "bomber", this._rect, false, this._position, this.rebirth, Color.White);
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
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            return base.HitEvent(e);
        }

        public override bool HitEvent(ObjectBase o)
        {
            return base.HitEvent(o);
        }
    }
}
