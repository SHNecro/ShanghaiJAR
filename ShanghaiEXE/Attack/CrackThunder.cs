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
    internal class CrackThunder : AttackBase
    {
        private CrackThunder.MOTION motion;
        private readonly bool crack;

        public CrackThunder(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          bool crack)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.eleki)
        {
            this.canCounter = false;
            this.element = ChipBase.ELEMENT.eleki;
            this.speed = 3;
            this.knock = true;
            this.invincibility = true;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 82);
            this.sound.PlaySE(SoundEffect.thunder);
            this.motion = CrackThunder.MOTION.init;
            this.frame = 0;
            this.crack = crack;
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.over)
                return;
            if (this.moveflame)
            {
                switch (this.motion)
                {
                    case CrackThunder.MOTION.init:
                        this.animationpoint.X = this.frame;
                        if (this.frame >= 3)
                        {
                            this.hitting = true;
                            this.motion = CrackThunder.MOTION.set;
                            if (this.crack)
                            {
                                this.ShakeStart(4, 3);
                                this.sound.PlaySE(SoundEffect.canon);
                                this.StandPanel.Crack();
                            }
                            break;
                        }
                        break;
                    case CrackThunder.MOTION.set:
                        this.animationpoint.X = this.frame;
                        if (this.frame >= 3)
                        {
                            this.hitting = false;
                            this.frame = 0;
                            this.motion = CrackThunder.MOTION.end;
                            break;
                        }
                        break;
                    case CrackThunder.MOTION.end:
                        this.flag = false;
                        break;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.animationpoint.X >= 3)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double num2 = positionDirect.Y - 70.0;
            shake = this.Shake;
            double y = shake.Y;
            double num3 = num2 + y;
            this._position = new Vector2((float)num1, (float)num3);
            this._rect = new Rectangle(128 + this.animationpoint.X * 24, 0, 24, 136);
            dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
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
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, ChipBase.ELEMENT.eleki));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, ChipBase.ELEMENT.eleki));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, ChipBase.ELEMENT.eleki));
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
