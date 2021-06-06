using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using Common.Vectors;
using System.Drawing;

namespace NSEffect
{
    internal class EnemyShadow : EffectBase
    {
        public bool print = true;
        private readonly EnemyBase MainEnemy;
        public Point slide;

        public EnemyShadow(IAudioEngine s, SceneBattle p, EnemyBase MainEnemy, bool rebirth)
          : base(s, p, MainEnemy.position.X, MainEnemy.position.Y)
        {
            this.rebirth = rebirth;
            this.position = new Point(MainEnemy.position.X, MainEnemy.position.Y);
            this.MainEnemy = MainEnemy;
            this.positionDirect = MainEnemy.positionDirect;
            this.downprint = true;
        }

        public override void Updata()
        {
            this.position = new Point(this.MainEnemy.position.X, this.MainEnemy.position.Y);
            this.positionDirect = this.MainEnemy.positionDirect;
            this.positionDirect.X += slide.X;
            this.positionDirect.Y += slide.Y;
            this.flag = this.MainEnemy.flag;
        }

        public override void PositionDirectSet()
        {
        }

        public override void Render(IRenderer dg)
        {
            if (!this.print)
                return;
            this._position = new Vector2((int)this.positionDirect.X + 10 + 4 * this.MainEnemy.UnionRebirth + this.Shake.X, (int)this.positionDirect.Y + 2 + this.Shake.Y);
            this._rect = new Rectangle(this.MainEnemy.animationpoint.X * this.MainEnemy.Wide, this.MainEnemy.Height * 6, this.MainEnemy.Wide, this.MainEnemy.Height);
            this.color = Color.FromArgb(MainEnemy.alfha, Color.White);
            dg.DrawImage(dg, this.MainEnemy.picturename, this._rect, false, this._position, this.rebirth, this.color);
        }
    }
}
