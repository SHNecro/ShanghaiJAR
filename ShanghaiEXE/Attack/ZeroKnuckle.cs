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
    internal class ZeroKnuckle : AttackBase
    {
        public new bool bright = true;
        private readonly int time;
        public bool effect;
        private readonly CharacterBase character;
        public bool get;

        public ZeroKnuckle(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          ChipBase.ELEMENT ele,
          CharacterBase character)
          : base(so, p, pX, pY, u, po, ele)
        {
            if (!this.flag)
                return;
            this.character = character;
            this.upprint = true;
            this.speed = s;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 0);
            this.hitting = true;
            this.rebirth = this.union == Panel.COLOR.blue;
            this.positionre = this.position;
            if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 - 5, this.position.Y * 24 + 48);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40 + 5, this.position.Y * 24 + 48);
            this.frame = 0;
            this.time = 1;
        }

        public override void Updata()
        {
            if (this.over)
                return;
            if (this.bright)
                this.PanelBright();
            if (this.moveflame)
            {
                this.animationpoint.X = this.frame;
                if (this.frame >= this.time)
                    this.flag = false;
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
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
            if (this.effect)
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, p.position.X, p.position.Y, 1, this.element));
            if (this.character is Player)
            {
                Player character = (Player)this.character;
                ChipFolder chipFolder = new ChipFolder(this.sound);
                if (p.haveChip[0] != null)
                {
                    character.haveChip.Insert(0, chipFolder.ReturnChip(p.haveChip[0].number));
                    ++character.numOfChips;
                    character.haveChip.RemoveAll(a => a == null);
                }
            }
            p.LossChip();
            this.get = true;
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            if (this.effect)
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, e.position.X, e.position.Y, 1, this.element));
            if (this.character is Player)
            {
                Player character = (Player)this.character;
                ChipFolder chipFolder = new ChipFolder(this.sound);
                int index = this.Random.Next(5);
                character.haveChip.Insert(0, chipFolder.ReturnChip(e.dropchips[index].chip.number));
                ++character.numOfChips;
                character.haveChip.RemoveAll(a => a == null);
            }
            this.get = true;
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            if (this.effect)
                this.parent.effects.Add(new Elementhit(this.sound, this.parent, o.position.X, o.position.Y, 1, this.element));
            return true;
        }
    }
}
