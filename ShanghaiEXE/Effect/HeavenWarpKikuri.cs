using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEffect
{
    internal class HeavenWarpKikuri : EffectBase
    {
        private static string Texture = "body3";
        private static readonly Rectangle TextureRect = new Rectangle(820, 340, 128, 128);

        private static int Divisions = 8;
        private static int BlockSpeed = 4;

        private long[] ySpeeds;
        private long[] yOffsets;
        private IList<Point> movingBlocks;

        public HeavenWarpKikuri(IAudioEngine s, Vector2 pd, Point posi)
          : base(s, null, posi.X, posi.Y)
        {
            this.positionDirect = pd;

            this.ySpeeds = Enumerable.Repeat(1L, Divisions * Divisions).ToArray();
            this.yOffsets = Enumerable.Repeat(0L, Divisions * Divisions).ToArray();
            this.movingBlocks = new List<Point>();
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                if (this.frame % BlockSpeed == 0)
                {
                    var unusedBlocks = Enumerable.Range(0, Divisions)
                        .SelectMany(x => Enumerable.Range(0, Divisions).Select(y =>
                    {
                        return new Point(x, y);
                    })).Where(p => !this.movingBlocks.Contains(p)).ToArray();

                    if (unusedBlocks.Any())
                    {
                        this.movingBlocks.Add(unusedBlocks[this.Random.Next(unusedBlocks.Length)]);
                    }
                }

                foreach (var block in this.movingBlocks)
                {
                    var index = Index(block.X, block.Y);
                    if (this.yOffsets[index] > 2400)
                    {
                        continue;
                    }

                    this.yOffsets[index] += this.ySpeeds[index];
                    this.ySpeeds[index] *= 2;
                }
            }

            this.FlameControl(3);

            if (this.yOffsets.All(y => y > 2400))
            {
                this.flag = false;
            }
        }

        public override void Render(IRenderer dg)
        {
            if (!this.flag)
            {
                return;
            }

            for (var x = 0; x < Divisions; x++)
            {
                for (var y = 0; y < Divisions; y++)
                {
                    var blockSize = new Size(TextureRect.Width / Divisions, TextureRect.Height / Divisions);
                    var blockPos = new Point(TextureRect.X + x * blockSize.Width, TextureRect.Y + y * blockSize.Height);

                    this._rect = new Rectangle(blockPos, blockSize);
                    this._position = new Vector2(this.positionDirect.X + this.Shake.X + (x * blockSize.Width),
                        positionDirect.Y + this.Shake.Y + (y * blockSize.Height) - yOffsets[Index(x, y)]);
                    this.color = Color.White;
                    dg.DrawImage(dg, Texture, this._rect, false, this._position, this.rebirth, this.color);
                }
            }
        }

        private static int Index(int x, int y)
        {
            return Divisions * y + x;
        }
    }
}
