using NSBattle;
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
    internal class FlanGear : AttackBase
    {
        private readonly FlanGear.MOTION motion;
        private readonly int time;

        public FlanGear(
          MyAudio so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          FlanGear.MOTION m)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            if (this.InArea)
            {
                if (this.parent.panel[this.position.X, this.position.Y].state == Panel.PANEL._break)
                    return;
                this.canCounter = false;
                this.time = 1;
                this.speed = 3;
                this.invincibility = true;
                this.animationpoint.X = 0;
                this.hitrange = new Point(0, 0);
                this.breaking = true;
                this.hitting = false;
                this.rebirth = this.union == Panel.COLOR.red;
                this.positionre = this.position;
                this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 82);
                this.sound.PlaySE(MyAudio.SOUNDNAMES.wave);
                this.motion = m;
                this.frame = 0;
            }
            else
                this.flag = false;
        }

        public override void Updata()
        {
            if (this.hitting)
                this.PanelBright();
            if (this.over)
                return;
            if (this.moveflame)
            {
                //this.animationpoint.X = this.frame;
                //if (60 % this.frame == 12) { this.animationpoint.X++; }
                if (this.frame == 2) { this.animationpoint.X++; }
                if (this.frame == 4) { this.animationpoint.X++; }
                if (this.frame == 6) { this.animationpoint.X++; }
                if (this.frame == 8) { this.animationpoint.X++; }
                if (this.frame > 8 && (this.frame % 2 == 0)) { this.animationpoint.X=3; }
                if (this.frame > 8 && (this.frame % 2 != 0)) { this.animationpoint.X=4; }
                //this.hitting = true;
                switch (this.frame)
                {
                    case 2:
                        this.hitting = true;
                        break;
                    case 19:
                        this.hitting = false;
                        break;
                    case 20:
                        this.flag = false;
                        this.over = true;
                        break;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y - 4f + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 100, 210, 100, 62);
            dg.DrawImage(dg, "flandreAttack", this._rect, false, this._position, this.rebirth, Color.White);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            bool result = false;
            int posX = this.position.X;
            int posY = this.position.Y;
            this.PanelBright();
            if (base.HitCheck(charaposition, charaunion)) { result = true; }

            //if (this.position.X > 0) { this.position.X--; }
            this.position.X--; this.PanelBright();
            if (base.HitCheck(charaposition, charaunion)) { result = true; }
            this.position.X = posX;
            //if (this.position.X < 2) { this.position.X++; }
            this.position.X++; this.PanelBright();
            if (base.HitCheck(charaposition, charaunion)) { result = true; }
            this.position.X = posX;

            //if (this.position.Y > 0) { this.position.Y--; }
            this.position.Y--; this.PanelBright();
            if (base.HitCheck(charaposition, charaunion)) { result = true; }
            this.position.Y = posY;
            //if (this.position.Y < 2) { this.position.Y++; }
            this.position.Y++; this.PanelBright();
            if (base.HitCheck(charaposition, charaunion)) { result = true; }
            this.position.Y = posY;

            return result;
            //return base.HitCheck(charaposition, charaunion);
        }

        public override bool HitCheck(Point charaposition)
        {
            bool result = false;
            int posX = this.position.X;
            int posY = this.position.Y;

            if (base.HitCheck(charaposition)) { result = true; }

            //if (this.position.X > 0) { this.position.X--; }
            this.position.X--;
            if (base.HitCheck(charaposition)) { result = true; }
            this.position.X = posX;
            //if (this.position.X < 2) { this.position.X++; }
            this.position.X++;
            if (base.HitCheck(charaposition)) { result = true; }
            this.position.X = posX;

            //if (this.position.Y > 0) { this.position.Y--; }
            this.position.Y--;
            if (base.HitCheck(charaposition)) { result = true; }
            this.position.Y = posY;
            //if (this.position.Y < 2) { this.position.Y++; }
            this.position.Y++;
            if (base.HitCheck(charaposition)) { result = true; }
            this.position.Y = posY;

            return result;

            //return base.HitCheck(charaposition);
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }

        public enum MOTION
        {
            init,
            set,
            end,
        }
    }
}
