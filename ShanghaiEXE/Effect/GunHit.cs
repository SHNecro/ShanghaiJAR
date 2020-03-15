﻿using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSEffect
{
    internal class GunHit : EffectBase
    {
        private const byte _speed = 1;

        public GunHit(MyAudio s, SceneBattle p, int pX, int pY, Panel.COLOR union)
          : base(s, p, pX, pY)
        {
            this.speed = 1;
            this.positionDirect = new Vector2(pX * 40 + 20, pY * 24 + 70);
            if (union != Panel.COLOR.red)
                return;
            this.rebirth = true;
        }

        public GunHit(MyAudio s, SceneBattle p, Vector2 posid)
          : base(s, p, 0, 0)
        {
            this.speed = 1;
            this.positionDirect = posid;
            if (this.union != Panel.COLOR.red)
                return;
            this.rebirth = true;
        }

        public override void Updata()
        {
            this.animationpoint = this.MoveAnimation(this.frame);
            if (this.frame > 4)
                this.flag = false;
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._rect = new Rectangle(this.animationpoint.X * 48, 736, 48, 48);
            this._position = this.positionDirect;
            dg.DrawImage(dg, "shot", this._rect, false, this._position, this.rebirth, Color.White);
        }

        private Point MoveAnimation(int waittime)
        {
            int[] numArray1 = new int[5] { 0, 1, 2, 3, 4 };
            int[] numArray2 = new int[5] { 0, 1, 2, 3, 4 };
            int y = 0;
            int index1 = 0;
            for (int index2 = 1; index2 < numArray1.Length; ++index2)
            {
                if (waittime <= numArray1[index2] && waittime > numArray1[index2 - 1])
                    index1 = index2;
            }
            if (index1 != numArray2.Length)
                return new Point(numArray2[index1], y);
            return new Point(0, 0);
        }
    }
}
