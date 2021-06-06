using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    internal class flanLevF : AttackBase
    {
        //private readonly FlanGear.MOTION motion;
        private readonly int time;
        int[] knifeX = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] knifeY = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int passPower;
        int preX;
        int preY;
        Panel.COLOR cCol;

        public flanLevF(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po, int s)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.normal)
        {
            this.passPower = po;
            this.preX = pX;
            this.preY = pY;

            this.positionre = this.position;
            this.positionDirect = new Vector2(this.position.X * 40 + 20, this.position.Y * 24 + 82);
            this.frame = 0;
            this.speed = s;
            this.cCol = u;
            this.hitrange = new Point(1, 2);
            this.hitting = true;
            /*
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
                this.sound.PlaySE(SoundEffect.wave);
                this.motion = m;
                this.frame = 0;
            }
            else
                this.flag = false;
            */
        }

        public override void Updata()
        {
            this.animationpoint = this.swordSwing(this.frame);
            if (this.over)
                return;

            switch (this.frame)
            {
                case 3:
                    this.sound.PlaySE(SoundEffect.sword);
                    AttackBase attackBase5 = new Halberd(this.sound, this.parent, this.position.X + cUnionRebirth, this.position.Y, this.union, this.power, 4, ChipBase.ELEMENT.heat, false);
                    {

                    }
                    break;
                case 14:
                    this.over = true;
                    this.flag = false;
                    break;

            }

            this.FlameControl();

        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y - 4f + Shake.Y);
            //this._position = new Vector2(this.position.X, this.position.Y);
            this._rect = new Rectangle(this.animationpoint.X * 140, 0, 140, 120);
            dg.DrawImage(dg, "flandreAttack", this._rect, false, this._position, this.rebirth, Color.White);
        }

        private Point swordSwing(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[7]
            {
        2,
        2,
        2,
        2,
        2,
        2,
        2
            }, new int[7] { 0, 1, 2, 3, 4, 5, 6 }, 0, waittime);
        }

        public int cUnionRebirth
        {
            get
            {
                return this.union == Panel.COLOR.red ? 1 : -1;
            }
        }



    }
}
