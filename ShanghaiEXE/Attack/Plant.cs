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
    internal class Plant : AttackBase
    {
        private readonly int roop = 0;
        private const int hit = 3;
        private readonly CharacterBase chara;

        public Plant(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele,
          CharacterBase chara)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.invincibility = false;
            this.knock = true;
            this.breaking = true;
            this.canCounter = false;
            this.chara = chara;
            this.speed = 1;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 64);
            this.frame = 0;
            this.BadStatusSet(CharacterBase.BADSTATUS.stop, 12 * 6);
            this.StandPanel.State = Panel.PANEL._grass;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.hitting)
                this.PanelBright();
            if (this.moveflame)
            {
                if (this.animationpoint.X < 3)
                    this.animationpoint.X = this.frame;
                else
                    this.animationpoint.X = 3 + this.frame % 2;
                switch (this.frame)
                {
                    case 1:
                        this.hitflag[this.position.X, this.position.Y] = false;
                        break;
                    case 2:
                        this.hitflag[this.position.X, this.position.Y] = false;
                        break;
                    case 8:
                        this.hitflag[this.position.X, this.position.Y] = false;
                        break;
                    case 14:
                        this.hitflag[this.position.X, this.position.Y] = false;
                        break;
                    case 20:
                        this.hitflag[this.position.X, this.position.Y] = false;
                        this.flag = false;
                        break;
                }
                if (this.frame > 3)
                {
                    if (!this.chara.flag)
                        this.flag = false;
                    if (this.chara.position != this.position)
                        this.flag = false;
                    if (this.chara is NaviBase && this.chara.invincibility)
                        this.flag = false;
                }
            }
            this.FlameControl(12);
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 48, 1064, 48, 56);
            dg.DrawImage(dg, "towers", this._rect, false, this._position, this.rebirth, Color.White);
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
    }
}
