using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSGame;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class VirusBall : AttackBase
    {
        private readonly SaveData savedata;
        private readonly VirusBall.TYPE type;
        private readonly VirusBall.TYPE ctype;
        private readonly int time;
        private Vector2 endposition;
        private readonly float movex;
        private readonly float movey;
        private float plusy;
        private float speedy;
        private readonly float plusing;
        private const int startspeed = 6;
        private readonly bool cluster;
        private new readonly int number;

        public VirusBall(
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
          int number)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            this.number = number;
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
            this.savedata = this.parent.parent.savedata;
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
                if (this.InArea && !this.StandPanel.Hole)
                {
                    this.sound.PlaySE(SoundEffect.bomb);
                    this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X, this.position.Y, Bomber.BOMBERTYPE.bomber, 2));
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
            this._rect = new Rectangle(32, 0, 16, 16);
            dg.DrawImage(dg, "bombs", this._rect, false, this._position, this.rebirth, Color.White);
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
            if (e.Hp <= 0 && e.race == EnemyBase.ENEMY.virus && (int)e.ID >= 1 && (int)e.ID <= 41)
            {
                this.savedata.HaveVirus[this.number] = new Virus()
                {
                    type = (int)e.ID,
                    code = this.Random.Next(26)
                };
                this.sound.PlaySE(SoundEffect.getchip);
                this.parent.effects.Add(new Get(this.sound, this.parent, this.position.X, this.position.Y));
                this.savedata.FlagList[(int)(299 + e.ID)] = true;
            }
            else if (e is DammyEnemy)
            {
                DammyEnemy dammyEnemy = (DammyEnemy)e;
                if (dammyEnemy.MainEnemy.Hp <= 0 && dammyEnemy.race == EnemyBase.ENEMY.virus)
                {
                    this.savedata.HaveVirus[this.number] = new Virus()
                    {
                        type = (int)e.ID,
                        code = this.Random.Next(26)
                    };
                    this.sound.PlaySE(SoundEffect.getchip);
                    this.parent.effects.Add(new Get(this.sound, this.parent, this.position.X, this.position.Y));
                    this.savedata.FlagList[(int)(299 + e.ID)] = true;
                }
            }
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
            single,
            closs,
            big,
        }
    }
}
