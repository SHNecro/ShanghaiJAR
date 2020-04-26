using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using SlimDX;
using System.Collections.Generic;
using System.Drawing;
using System;

namespace NSMap.Character
{
    public class MapEffect : MapCharacterBase
    {
        private static readonly Dictionary<int, Func<IAudioEngine, Vector3, MapField, EffectBase>> EffectFactory = new Dictionary<int, Func<IAudioEngine, Vector3, MapField, EffectBase>>
        {
            [0] = (sound, position, field) => new AliceJump(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [1] = (sound, position, field) => new Flash(sound, null, new Vector2(position.X, position.Y), new Point(0, 0)),
            [2] = (sound, position, field) => new ShanghaiIN(sound, new Vector2(position.X, position.Y), new Point(0, 0), true),
            [3] = (sound, position, field) => new Bomber(sound, null, Bomber.BOMBERTYPE.bomber, new Vector2(position.X, position.Y), 3, new Point(0, 0)),
            [4] = (sound, position, field) => new ItemGet(sound, new Vector2(position.X, position.Y), new Point(0, 0), field.save),
            [5] = (sound, position, field) => new ShanghaiOUT(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [6] = (sound, position, field) => new Smoke(sound, null, new Vector2(position.X, position.Y), new Point(0, 0), ChipBase.ELEMENT.aqua),
            [7] = (sound, position, field) => new IceBreak(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [8] = (sound, position, field) => new IceMake(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [9] = (sound, position, field) => new AlicePhone(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [10] = (sound, position, field) => new AlicePowder(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [11] = (sound, position, field) => new FightShanhaiBack(sound, new Vector2(position.X, position.Y), new Point(0, 0), 1),
            [12] = (sound, position, field) => new FightShanhaiBack(sound, new Vector2(position.X, position.Y), new Point(0, 0), 3),
            [13] = (sound, position, field) => new MultiIN(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [14] = (sound, position, field) => new BadAir(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            [15] = (sound, position, field) => new CubeOpen(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [16] = (sound, position, field) => new FightShanhaiBack(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            [17] = (sound, position, field) => new FightShanhaiBack(sound, new Vector2(position.X, position.Y), new Point(0, 0), 2),
            [18] = (sound, position, field) => new FightMarisa(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            [19] = (sound, position, field) => new FightMarisa(sound, new Vector2(position.X, position.Y), new Point(0, 0), 1),
            [20] = (sound, position, field) => new FightMarisa(sound, new Vector2(position.X, position.Y), new Point(0, 0), 2),
            [21] = (sound, position, field) => new FightMarisa(sound, new Vector2(position.X, position.Y), new Point(0, 0), 3),
            [22] = (sound, position, field) => new SlashYorihime(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            [23] = (sound, position, field) => new SlashYorihime(sound, new Vector2(position.X, position.Y), new Point(0, 0), 1),
            [24] = (sound, position, field) => new SlashYorihime(sound, new Vector2(position.X, position.Y), new Point(0, 0), 2),
            [25] = (sound, position, field) => new SlashYorihime(sound, new Vector2(position.X, position.Y), new Point(0, 0), 3),
            [26] = (sound, position, field) => new G7(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [27] = (sound, position, field) => new FallMedi(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            [28] = (sound, position, field) => new DruidAttack(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [29] = (sound, position, field) => new AliceBed(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [30] = (sound, position, field) => new MoveEnemy(sound, null, new Vector2(position.X, position.Y), new Point(0, 0)),
            [31] = (sound, position, field) => new JinjaGate(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [32] = (sound, position, field) => new AlicePowderLeft(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [33] = (sound, position, field) => new Basterhit(sound, null, new Vector2(position.X, position.Y), 3, new Point(0, 0)),
            [34] = (sound, position, field) => new Guard(sound, null, new Vector2(position.X, position.Y), 3, new Point(0, 0)),
            [35] = (sound, position, field) => new FireBall(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [36] = (sound, position, field) => new BadAir(sound, new Vector2(position.X, position.Y), new Point(0, 0), 1),
            [37] = (sound, position, field) => new BadAir(sound, new Vector2(position.X, position.Y), new Point(0, 0), 2),
            [38] = (sound, position, field) => new Smoke(sound, null, new Vector2(position.X, position.Y), new Point(0, 0), ChipBase.ELEMENT.normal),
            [39] = (sound, position, field) => new Elementhit(sound, null, new Vector2(position.X, position.Y), 2, new Point(0, 0), ChipBase.ELEMENT.eleki),
            [40] = (sound, position, field) => new GunHit(sound, null, new Vector2(position.X, position.Y)),
            [41] = (sound, position, field) => new HeavenWater(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [42] = (sound, position, field) => new ROMHead(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [43] = (sound, position, field) => new KikuriFade(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
			[44] = (sound, position, field) => new ShanghaiIN(sound, new Vector2(position.X, position.Y), new Point(0, 0), false),
			[45] = (sound, position, field) => new NormalChargehit(sound, null, new Vector2(position.X, position.Y), 2, new Point(0, 0)),
            [46] = (sound, position, field) => new AliceBed(sound, new Vector2(position.X, position.Y), new Point(0, 0), true),
            [47] = (sound, position, field) => new FireBall(sound, new Vector2(position.X, position.Y), new Point(0, 0), true),
            [48] = (sound, position, field) => new KeystoneUnlock(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [49] = (sound, position, field) => new PetalBreeze(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            [50] = (sound, position, field) => new ShanghaiHeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false),
            [51] = (sound, position, field) => new ShanghaiHeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true),
            [52] = (sound, position, field) => new SageFloat(sound, new Vector2(position.X, position.Y), new Point(0, 0), false, true),
            [53] = (sound, position, field) => new SageFloat(sound, new Vector2(position.X, position.Y), new Point(0, 0), true, true),
            [54] = (sound, position, field) => new SageFloat(sound, new Vector2(position.X, position.Y), new Point(0, 0), false, false),
            [55] = (sound, position, field) => new SageFloat(sound, new Vector2(position.X, position.Y), new Point(0, 0), true, false)
        };

    public EffectBase effect;

        private EffectBase EffectSet(int key)
        {
            return EffectFactory[key](this.sound, this.position, this.field);
        }

        public bool Flag
        {
            get
            {
                return this.effect.flag;
            }
            set
            {
                this.effect.flag = value;
            }
        }

        public MapEffect(
          IAudioEngine s,
          SceneMap p,
          Point po,
          int floor,
          MapField fi,
          int effectNo,
          int rendType)
          : base(s, p, po, floor, MapCharacterBase.ANGLE.none, fi)
        {
            this.position.Z = floor * (this.field.Height / 2);
            this.effect = this.EffectSet(effectNo);
            this.rendType = rendType;
        }

        public override void Update()
        {
            this.effect.Updata();
            if (this.effect.position.X == 0 && (uint)this.effect.position.Y <= 0U)
                return;
            this.position.X += effect.position.X;
            this.position.Y += effect.position.Y;
            this.effect.position = new Point();
        }

        public override void Render(IRenderer dg)
        {
            this.ChangeQuarter();
            this.effect.positionDirect = new Vector2(this.positionQ.X - this.field.camera.X + Shake.X, (float)(positionQ.Y - (double)this.field.camera.Y + Shake.Y - 2.0));
            this.effect.Render(dg);
        }

        public override float RendSetter()
        {
            return (float)(position.X + 2.0 + position.Y + 2.0);
        }
    }
}
