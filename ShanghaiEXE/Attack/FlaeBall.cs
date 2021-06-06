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
    internal class FlaeBall : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        private readonly int movespeed;
        private readonly bool cross;
        private bool stop;
        private readonly bool load3;
        private bool init;
        private int manymove;

        public FlaeBall(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele,
          int movespeed = 4,
          bool cross = false,
          bool load3 = true)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.load3 = load3;
            this.movespeed = movespeed;
            this.invincibility = false;
            this.speed = s;
            this.positionDirect = v;
            this.cross = cross;
            this.power /= 2;
            this.OldPD = this.positionDirect;
        }

        public override void Updata()
        {
            this.PanelBright();
            if (this.frame >= 3 && !this.movestart)
            {
                this.movestart = true;
                this.hitting = true;
                this.sound.PlaySE(SoundEffect.canon);
            }
            if (!this.init)
            {
                foreach (CharacterBase characterBase in this.parent.AllHitter())
                {
                    if (characterBase.position.X == this.position.X + this.UnionRebirth && (characterBase.position.Y == this.position.Y || characterBase.position.Y == this.position.Y + 1 || characterBase.position.Y == this.position.Y - 1) && characterBase.union == this.UnionEnemy && !characterBase.nohit)
                    {
                        this.stop = true;
                        this.frame = 0;
                        break;
                    }
                }
                this.init = true;
            }
            if (this.moveflame)
            {
                if (!this.stop)
                {
                    if (this.movestart)
                    {
                        this.positionDirect.X += this.union == Panel.COLOR.red ? movespeed : -this.movespeed;
                        this.manymove += this.movespeed;
                        if (!this.load3)
                        {
                            if (this.manymove % 40 == 0)
                            {
                                foreach (CharacterBase characterBase in this.parent.AllHitter())
                                {
                                    if (characterBase.position.X == this.position.X + this.UnionRebirth && (characterBase.position.Y == this.position.Y || characterBase.position.Y == this.position.Y + 1 || characterBase.position.Y == this.position.Y - 1) && characterBase.union == this.UnionEnemy && !characterBase.nohit)
                                    {
                                        this.stop = true;
                                        this.frame = 0;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (this.manymove / 40 >= 2)
                        {
                            this.stop = true;
                            this.frame = 0;
                        }
                        this.position = this.Calcposition(this.positionDirect, 36, false);
                        if (positionDirect.X < 0.0 || positionDirect.X > 240.0)
                            this.flag = false;
                    }
                }
                else
                {
                    if (this.frame % 2 == 0)
                    {
                        ++this.animationpoint.X;
                        if (this.animationpoint.X >= 2)
                            this.animationpoint.X = 0;
                    }
                    if (this.frame > 30)
                    {
                        this.sound.PlaySE(SoundEffect.bombmiddle);
                        this.flag = false;
                        this.power *= 2;
                        int num1 = 0;
                        int num2 = 0;
                        for (int index = 0; index < 5; ++index)
                        {
                            switch (index - 1)
                            {
                                case 0:
                                    num1 = -1;
                                    num2 = -1;
                                    break;
                                case 1:
                                    num1 = -1;
                                    num2 = 1;
                                    break;
                                case 2:
                                    num1 = 1;
                                    num2 = -1;
                                    break;
                                case 3:
                                    num1 = 1;
                                    num2 = 1;
                                    break;
                                default:
                                    num1 = 0;
                                    num2 = 0;
                                    break;
                            }
                            this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + num1, this.position.Y + num2, this.union, this.power, 1, this.element)));
                            this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X + num1, this.position.Y + num2, Bomber.BOMBERTYPE.flashbomber, 2));
                        }
                        if (!this.cross)
                        {
                            for (int index = 1; index < 5; ++index)
                            {
                                switch (index - 1)
                                {
                                    case 0:
                                        num1 = -1;
                                        num2 = 0;
                                        break;
                                    case 1:
                                        num1 = 0;
                                        num2 = -1;
                                        break;
                                    case 2:
                                        num1 = 1;
                                        num2 = 0;
                                        break;
                                    case 3:
                                        num1 = 0;
                                        num2 = 1;
                                        break;
                                }
                                this.parent.attacks.Add(this.StateCopy(new BombAttack(this.sound, this.parent, this.position.X + num1, this.position.Y + num2, this.union, this.power, 1, this.element)));
                                this.parent.effects.Add(new Bomber(this.sound, this.parent, this.position.X + num1, this.position.Y + num2, Bomber.BOMBERTYPE.flashbomber, 2));
                            }
                        }
                    }
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 24, 992, 24, 24);
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
            this.ShakeStart(5, 8);
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, p.position.X, p.position.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.ShakeStart(5, 8);
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, e.position.X, e.position.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.sound.PlaySE(SoundEffect.bombmiddle);
            this.parent.effects.Add(new Bomber(this.sound, this.parent, o.position.X, o.position.Y, Bomber.BOMBERTYPE.flashbomber, 2));
            return true;
        }
    }
}
