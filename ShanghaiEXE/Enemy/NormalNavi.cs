using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSEffect;
using System.Drawing;
using System.Linq;

namespace NSEnemy
{
    internal class NormalNavi : ChipUsingNaviBase
    {
        private readonly NAVITYPE navitype;
        private int no2ChipUsed;

        public NormalNavi(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          byte n,
          Panel.COLOR u,
          byte v,
          int h,
          int c1,
          int c2,
          int c3,
          string names)
          : base(s, p, pX, pY, n, u, v, h, names, $"navi{v}")
        {
            this.InitialHP = h;
            this.InitialChips = new[] { c1, c2, c3 };
            this.SetDroppedChips();
            this.InitialName = names;

            switch (this.version)
            {
                case 13:
                case 19:
                    this.navitype = NormalNavi.NAVITYPE.blackroab;
                    this.Flying = true;
                    break;
                case 16:
                case 17:
                case 18:
                    this.navitype = NormalNavi.NAVITYPE.blackroab;
                    this.pain = true;
                    break;
                default:
                    this.navitype = NormalNavi.NAVITYPE.normal;
                    break;
            }
        }

        public int InitialHP { get; set; }

        public int[] InitialChips { get; }

        public string InitialName { get; set; }

        protected override void InitializeChips()
        {
            this.SetDroppedChips();

            this.chips = new[] { this.dropchips[0], this.dropchips[1], this.dropchips[2], this.dropchips[3], this.dropchips[4], };
        }

        protected override void SetUsedChip()
        {
            base.SetUsedChip();

            var chipWeights = this.no2ChipUsed < this.HpMax / 700
                ? new[] { 50, 20, 30 }
                : new[] { 70, 30, 0 };
            var num = this.Random.Next(chipWeights.Sum());
            for (var i = 0; i < chipWeights.Length; i++)
            {
                num -= chipWeights[i];
                if (num < 0)
                {
                    this.usechip = i;
                    break;
                }
            }

        }

        protected override void OnAttack()
        {
            base.OnAttack();

            if (this.usechip == 2)
                ++this.no2ChipUsed;
        }

        protected override void MovementIdle()
        {
            base.MovementIdle();

            if (this.navitype == NAVITYPE.blackroab)
            {
                ++this.animeFlame;
                if (this.animeFlame / 4 > 6)
                    this.animeFlame = 0;
                this.animationpoint = this.AnimeNeutral(this.animeFlame / 4);
            }
        }

        protected override void MovementEffect()
        {
            base.MovementEffect();

            if (this.navitype == NAVITYPE.blackroab)
            {
                this.parent.effects.Add(new StepShadow(this.sound, this.parent, new Rectangle(this.animationpoint.X * this.wide, (this.version < 4 ? 0 : 2) * this.height, this.wide, this.height), this.positionDirect, this.picturename, this.rebirth, this.position));
            }
        }

        protected override void MovementAnimation()
        {
            base.MovementAnimation();

            if (this.navitype == NAVITYPE.blackroab)
            {
                ++this.animeFlame;
                if (this.animeFlame / 4 > 6)
                    this.animeFlame = 0;
                this.animationpoint = this.AnimeNeutral(this.animeFlame / 4);
            }
        }

        private void SetDroppedChips()
        {
            ChipFolder chipFolder1 = new ChipFolder(this.sound);
            chipFolder1.SettingChip(this.InitialChips[0]);
            chipFolder1.codeNo = this.Random.Next(4);
            this.dropchips[0] = chipFolder1;
            ChipFolder chipFolder2 = new ChipFolder(this.sound);
            chipFolder2.SettingChip(this.InitialChips[1]);
            chipFolder2.codeNo = this.Random.Next(4);
            this.dropchips[1] = chipFolder2;
            ChipFolder chipFolder3 = new ChipFolder(this.sound);
            chipFolder3.SettingChip(this.InitialChips[2]);
            chipFolder3.codeNo = this.Random.Next(4);
            this.dropchips[2] = chipFolder3;
            ChipFolder chipFolder4 = new ChipFolder(this.sound);
            chipFolder4.SettingChip(this.InitialChips[1]);
            chipFolder4.codeNo = this.Random.Next(4);
            this.dropchips[3] = chipFolder4;
            ChipFolder chipFolder5 = new ChipFolder(this.sound);
            chipFolder5.SettingChip(this.InitialChips[0]);
            chipFolder5.codeNo = this.Random.Next(4);
            this.dropchips[4] = chipFolder5;
        }

        private Point AnimeNeutral(int waitflame)
        {
            return this.Return(new int[7] { 0, 1, 2, 3, 4, 5, 6 }, new int[7]
            {
                0,
                1,
                2,
                3,
                2,
                1,
                0
            }, 0, waitflame);
        }

        private enum NAVITYPE
        {
            normal,
            blackroab,
            sprite,
            shanghai,
        }
    }
}
