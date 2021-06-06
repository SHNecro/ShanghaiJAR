using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System;

namespace NSMap.Character
{
    public class MapEffect : MapCharacterBase
    {
        // Editor effects list at MapEditor.Models.Elements.Enums.EffectTypeNumber
        private static readonly List<Func<IAudioEngine, Vector3, MapField, EffectBase>> EffectFactory = new List<Func<IAudioEngine, Vector3, MapField, EffectBase>>
        {
            (sound, position, field) => new AliceJump(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new Flash(sound, null, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new ShanghaiIN(sound, new Vector2(position.X, position.Y), new Point(0, 0), true),
            (sound, position, field) => new Bomber(sound, null, Bomber.BOMBERTYPE.bomber, new Vector2(position.X, position.Y), 3, new Point(0, 0)),
            (sound, position, field) => new ItemGet(sound, new Vector2(position.X, position.Y), new Point(0, 0), field.save),
            (sound, position, field) => new ShanghaiOUT(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new Smoke(sound, null, new Vector2(position.X, position.Y), new Point(0, 0), ChipBase.ELEMENT.aqua),
            (sound, position, field) => new IceBreak(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new IceMake(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new AlicePhone(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new AlicePowder(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new FightShanhaiBack(sound, new Vector2(position.X, position.Y), new Point(0, 0), 1),
            (sound, position, field) => new FightShanhaiBack(sound, new Vector2(position.X, position.Y), new Point(0, 0), 3),
            (sound, position, field) => new MultiIN(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new BadAir(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            (sound, position, field) => new CubeOpen(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new FightShanhaiBack(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            (sound, position, field) => new FightShanhaiBack(sound, new Vector2(position.X, position.Y), new Point(0, 0), 2),
            (sound, position, field) => new FightMarisa(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            (sound, position, field) => new FightMarisa(sound, new Vector2(position.X, position.Y), new Point(0, 0), 1),
            (sound, position, field) => new FightMarisa(sound, new Vector2(position.X, position.Y), new Point(0, 0), 2),
            (sound, position, field) => new FightMarisa(sound, new Vector2(position.X, position.Y), new Point(0, 0), 3),
            (sound, position, field) => new SlashYorihime(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            (sound, position, field) => new SlashYorihime(sound, new Vector2(position.X, position.Y), new Point(0, 0), 1),
            (sound, position, field) => new SlashYorihime(sound, new Vector2(position.X, position.Y), new Point(0, 0), 2),
            (sound, position, field) => new SlashYorihime(sound, new Vector2(position.X, position.Y), new Point(0, 0), 3),
            (sound, position, field) => new G7(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new FallMedi(sound, new Vector2(position.X, position.Y), new Point(0, 0), 0),
            (sound, position, field) => new DruidAttack(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new AliceBed(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new MoveEnemy(sound, null, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new JinjaGate(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new AlicePowderLeft(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new Basterhit(sound, null, new Vector2(position.X, position.Y), 3, new Point(0, 0)),
            (sound, position, field) => new Guard(sound, null, new Vector2(position.X, position.Y), 3, new Point(0, 0)),
            (sound, position, field) => new FireBall(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new BadAir(sound, new Vector2(position.X, position.Y), new Point(0, 0), 1),
            (sound, position, field) => new BadAir(sound, new Vector2(position.X, position.Y), new Point(0, 0), 2),
            (sound, position, field) => new Smoke(sound, null, new Vector2(position.X, position.Y), new Point(0, 0), ChipBase.ELEMENT.normal),
            (sound, position, field) => new Elementhit(sound, null, new Vector2(position.X, position.Y), 2, new Point(0, 0), ChipBase.ELEMENT.eleki),
            (sound, position, field) => new GunHit(sound, null, new Vector2(position.X, position.Y)),
            (sound, position, field) => new HeavenWater(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new ROMHead(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new KikuriFade(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
			(sound, position, field) => new ShanghaiIN(sound, new Vector2(position.X, position.Y), new Point(0, 0), false),
			(sound, position, field) => new NormalChargehit(sound, null, new Vector2(position.X, position.Y), 2, new Point(0, 0)),
            (sound, position, field) => new AliceBed(sound, new Vector2(position.X, position.Y), new Point(0, 0), true),
            (sound, position, field) => new FireBall(sound, new Vector2(position.X, position.Y), new Point(0, 0), true),
            (sound, position, field) => new KeystoneUnlock(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new PetalBreeze(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true),
            (sound, position, field) => new SageFloat(sound, new Vector2(position.X, position.Y), new Point(0, 0), false, true),
            (sound, position, field) => new SageFloat(sound, new Vector2(position.X, position.Y), new Point(0, 0), true, true),
            (sound, position, field) => new SageFloat(sound, new Vector2(position.X, position.Y), new Point(0, 0), false, false),
            (sound, position, field) => new SageFloat(sound, new Vector2(position.X, position.Y), new Point(0, 0), true, false),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, true, "charachip18", new Rectangle(18, 192 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true, true, "charachip18", new Rectangle(18, 192 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, true, "charachip18", new Rectangle(448 + 18, 384 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true, true, "charachip18", new Rectangle(448 + 18, 384 + 48, 24, 48)),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.UPRIGHT, FreeSoul.DissolveCharacter.Ghost),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.DOWNRIGHT, FreeSoul.DissolveCharacter.Ghost),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.DOWNLEFT, FreeSoul.DissolveCharacter.Ghost),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.UPLEFT, FreeSoul.DissolveCharacter.Ghost),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.UPRIGHT, FreeSoul.DissolveCharacter.Alive),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.DOWNRIGHT, FreeSoul.DissolveCharacter.Alive),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.DOWNLEFT, FreeSoul.DissolveCharacter.Alive),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.UPLEFT, FreeSoul.DissolveCharacter.Alive),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.none, FreeSoul.DissolveCharacter.NoBody),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.UPRIGHT, FreeSoul.DissolveCharacter.Sprite),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.DOWNRIGHT, FreeSoul.DissolveCharacter.Sprite),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.DOWNLEFT, FreeSoul.DissolveCharacter.Sprite),
            (sound, position, field) => new FreeSoul(sound, new Vector2(position.X, position.Y), new Point(0, 0), ANGLE.UPLEFT, FreeSoul.DissolveCharacter.Sprite),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true, true, "charachip20", new Rectangle(15, 32, 30, 64), false),
            (sound, position, field) => new Noise(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, true, "charachip18", new Rectangle(448 + 18, 192 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true, true, "charachip18", new Rectangle(448 + 18, 192 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, true, "charachip19", new Rectangle(448 + 18, 672 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true, true, "charachip19", new Rectangle(448 + 18, 672 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, false, "charachip19", new Rectangle(448 + 18, 576 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true, false, "charachip19", new Rectangle(448 + 18, 576 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, true, "charachip19", new Rectangle(448 + 18, 576 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true, true, "charachip19", new Rectangle(448 + 18, 576 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, false, "charachip19", new Rectangle(448 + 18, 672 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true, false, "charachip19", new Rectangle(448 + 18, 672 + 48, 24, 48)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, true, "charachip20", new Rectangle(15, 96 + 32, 30, 64), false),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, false, "charachip20", new Rectangle(15, 32, 30, 64), false),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, true, "charachip20", new Rectangle(15, 32, 30, 64), false),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, false, "charachip20", new Rectangle(15, 96 + 32, 30, 64), false),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, false, true, "charachip18", new Rectangle(18, 288 + 32, 24, 64)),
            (sound, position, field) => new HeavenWarp(sound, new Vector2(position.X, position.Y), new Point(0, 0), field, true, true, "charachip18", new Rectangle(18, 288 + 32, 24, 64)),
            (sound, position, field) => new HeavenWarpKikuri(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
            (sound, position, field) => new HeavenTreeEvent(sound, new Vector2(position.X, position.Y), new Point(0, 0)),
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
