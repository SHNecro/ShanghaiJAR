using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeAuraSword : ChargeBase
    {
        private bool aura = false;
        private const int start = 3;
        private const int speed = 8;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public ChargeAuraSword(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 200;
            this.power = 10;
            this.shorttime = 20;
            this.character = player;
            this.battle = this.player.parent;
        }

        public override void Action()
        {
            this.character.animationpoint = CharacterAnimation.SworsAnimation(this.character.waittime);
            if (this.character.waittime == 3)
            {
                this.sound.PlaySE(SoundEffect.sword);
                if ((uint)this.character.barrierType > 0U)
                {
                    this.sound.PlaySE(SoundEffect.shoot);
                    this.aura = true;
                }
            }
            this.character.animationpoint = CharacterAnimation.SworsAnimation(this.character.waittime);
            if (this.character.waittime >= 30)
            {
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
                this.aura = false;
            }
            if (this.character.waittime != 10)
                return;
            int power = this.power;
            AttackBase a = new SonicBoom(this.sound, this.character.parent, this.character.position.X + this.UnionRebirth(this.character.union), this.character.position.Y, this.character.union, this.Power, 8, ChipBase.ELEMENT.normal, this.aura);
            a.invincibility = true;
            this.character.parent.attacks.Add(this.CounterNone(a));
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            if (this.character.waittime > 25)
                return;
            this._rect = new Rectangle(this.character.animationpoint.X * this.character.Wide, 5 * this.character.Height, this.character.Wide, this.character.Height);
            this._position = new Vector2(this.character.positionDirect.X + Shake.X, this.character.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, this.character.picturename, this._rect, false, this._position, this.character.union == Panel.COLOR.blue, Color.White);
        }

        public Point MoveAnimation(int waittime)
        {
            return CharacterAnimation.ReturnKai(new int[4]
            {
        4,
        14,
        4,
        18
            }, new int[4] { 0, 1, 2, 3 }, 6, waittime);
        }
    }
}
