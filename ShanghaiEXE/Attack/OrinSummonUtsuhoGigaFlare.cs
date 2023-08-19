using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;
using NSAttack;

namespace NSAttack
{
    internal class OrinSummonUtsuhoGigaFlare : AttackBase
    {
        private bool movestart = false;
        private const int plusy = 70;
        private readonly int movespeed;
        private int palette;

        public OrinSummonUtsuhoGigaFlare(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          Vector2 v,
          ChipBase.ELEMENT ele,
          int movespeed,
          int pal)
          : base(so, p, pX, pY, u, po, ele)
        {
            this.movespeed = movespeed;
            this.invincibility = true;
            //this.breakinvi = true;
            this.breaking = true;
            this.speed = s;
            this.positionDirect = v;
            this.positionold = this.position;
            this.OldPD = this.positionDirect;
            //this.animationpoint.X = 2;
            this.palette = pal;
        }

        public override void Updata()
        {
            //
            if (this.frame > 7 && this.frame < 40)
                this.animationpoint = this.AnimeGigantFlear(this.frame / 4);


            this.PanelBright();
            switch (this.frame)
            {
                case 1:
                    this.animationpoint.X = 0;
                    this.animationpoint.Y = 0;
                    this.sound.PlaySE(SoundEffect.warp);
                    break;
                case 8:
                    this.sound.PlaySE(SoundEffect.dark);
                    this.sound.PlaySE(SoundEffect.charge);
                    this.animationpoint.X = 0;
                    this.animationpoint.Y = 4;
                    break;
                case 14:
                    OrinGigantFlear die = new OrinGigantFlear(this.sound, this.parent, this.position.X + this.UnionRebirth, this.position.Y, this.union, this.power, 2, false);
                    die.breaking = true;
                    this.parent.attacks.Add(die);

                    break;
                case 40:
                    this.animationpoint.X = 0;
                    this.animationpoint.Y = 0;
                    break;
                case 52:
                    
                    this.flag = false;
                    break;
            }

            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            /*
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 50, 0, 50, 50);
            dg.DrawImage(dg, "OrinAttack1", this._rect, false, this._position, this.rebirth, Color.White);

            */

            int num = 0;
            if (this.palette == 3 || this.palette == 4)
                num = 2160;
            if (this.palette == 2)
                num = 1440;
            this._rect = new Rectangle(120 * this.animationpoint.X, num + 144 * this.animationpoint.Y, 120, 144);
            this._position = new Vector2(this.position.X * 40f + 24 * this.UnionRebirth + 32, (float)(this.position.Y * 24.0 + 22.0));
            if (this.palette != 4)
                dg.DrawImage(dg, "Uthuho", this._rect, false, this._position, this.union == Panel.COLOR.red, Color.White);
            else
                dg.DrawImage(dg, "UthuhoAlter", this._rect, false, this._position, this.union == Panel.COLOR.red, Color.White);

        }

        private Point AnimeGigantFlear(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[16]
            {
                1,
                1,
                1,
                1,
                1,
                4,
                1,
                1,
                1,
                1,
                15,
                1,
                1,
                1,
                1,
                100
            }, new int[16]
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
                9,
                10,
                9,
                8,
                7,
                6,
                5
            }, 4, waittime);
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (!base.HitCheck(charaposition, charaunion))
                return false;
            this.sound.PlaySE(SoundEffect.dark);

            this.flag = false;
            return true;
        }

        public override bool HitCheck(Point charaposition)
        {
            if (!base.HitCheck(charaposition))
                return false;

            this.sound.PlaySE(SoundEffect.dark);


            this.flag = false;
            return true;
        }

        public override bool HitEvent(Player p)
        {
            if (!base.HitEvent(p))
                return false;
            this.sound.PlaySE(SoundEffect.dark);

            return true;
        }

        public override bool HitEvent(EnemyBase e)
        {
            if (!base.HitEvent(e))
                return false;
            this.sound.PlaySE(SoundEffect.dark);


            return true;
        }

        public override bool HitEvent(ObjectBase o)
        {
            if (!base.HitEvent(o))
                return false;
            this.sound.PlaySE(SoundEffect.dark);

            return true;
        }
    }
}
