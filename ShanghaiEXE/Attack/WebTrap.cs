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
using System.Linq;

namespace NSAttack
{
    internal class WebTrap : AttackBase
    {
        private readonly int time;

        public WebTrap(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          int time,
          ChipBase.ELEMENT element)
          : base(so, p, pX, pY, u, po, element)
        {
            if (!this.flag)
                return;
            this.knock = true;
            this.downprint = true;
            this.invincibility = true;
            this.canCounter = false;
            this.time = time;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 82);
            this.frame = 0;
            if (this.StandPanel.Hole)
                this.flag = false;

            var existingObject = p.attacks.FirstOrDefault(a => a is WebTrap && a.position == this.position);
            if (existingObject != null)
            {
                this.parent.effects.Add(new Smoke(this.sound, this.parent, this.position.X, this.position.Y, ChipBase.ELEMENT.normal));
                existingObject.flag = false;
            }
        }

        public void Init()
        {
            switch (this.element)
            {
                case ChipBase.ELEMENT.heat:
                    this.BadStatusSet(CharacterBase.BADSTATUS.stop, 180);
                    this.BadStatusSet(CharacterBase.BADSTATUS.melt, 600);
                    break;
                case ChipBase.ELEMENT.eleki:
                    this.BadStatusSet(CharacterBase.BADSTATUS.stop, 180);
                    this.BadStatusSet(CharacterBase.BADSTATUS.paralyze, 180);
                    break;
                case ChipBase.ELEMENT.leaf:
                    this.BadStatusSet(CharacterBase.BADSTATUS.stop, 180);
                    break;
                case ChipBase.ELEMENT.poison:
                    this.BadStatusSet(CharacterBase.BADSTATUS.stop, 180);
                    this.BadStatusSet(CharacterBase.BADSTATUS.poison, 6000);
                    break;
                case ChipBase.ELEMENT.earth:
                    this.BadStatusSet(CharacterBase.BADSTATUS.stop, 600);
                    break;
            }
        }

        public override void Updata()
        {
            this.union = this.parent.panel[this.position.X, this.position.Y].color == Panel.COLOR.blue ? Panel.COLOR.red : Panel.COLOR.blue;
            if (this.over)
                return;
            if (this.StandPanel.Hole)
                this.flag = false;
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame / 3 % 5;
                if (this.frame >= this.time)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            if (this.frame > this.time - 60 && this.frame % 3 != 0)
                return;
            double x1 = positionDirect.X;
            Point shake = this.Shake;
            double x2 = shake.X;
            double num1 = x1 + x2;
            double y1 = positionDirect.Y;
            shake = this.Shake;
            double y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1, (float)num2);
            int num3 = 0;
            switch (this.element)
            {
                case ChipBase.ELEMENT.heat:
                    num3 = 1;
                    break;
                case ChipBase.ELEMENT.eleki:
                    num3 = 4;
                    break;
                case ChipBase.ELEMENT.leaf:
                    num3 = 0;
                    break;
                case ChipBase.ELEMENT.poison:
                    num3 = 2;
                    break;
                case ChipBase.ELEMENT.earth:
                    num3 = 3;
                    break;
            }
            this._rect = new Rectangle(40 * num3, 16, 40, 24);
            dg.DrawImage(dg, "bombs", this._rect, false, this._position, false, Color.White);
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
