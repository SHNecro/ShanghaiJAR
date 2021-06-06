using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSCharge
{
    internal class ChargeVulcan : ChargeBase
    {
        private const int shotend = 10;
        private const int shotinterval = 4;
        private readonly CharacterBase character;
        private readonly SceneBattle battle;

        public ChargeVulcan(IAudioEngine s, Player p)
          : base(s, p)
        {
            this.chargetime = 200;
            this.power = 10;
            this.shorttime = 10;
            this.character = player;
            this.battle = this.player.parent;
        }

        public override void Action()
        {
            this.character.animationpoint = ChargeVulcan.Animation(this.character.waittime);

            // # of shots: 2 shots per power
            var numShots = player.busterPower * 2;

            // At max Chrg (5), the player can chain knockbacks infinitely (barely)
            var maxShots = numShots >= 16;
            if (maxShots)
            {
                numShots = 15;
            }

            var firingTime = 8 * numShots;
            if (this.character.waittime >= firingTime)
            {
                this.End();
                this.player.animationpoint = new Point(0, 0);
                this.player.motion = Player.PLAYERMOTION._neutral;
            }

            if (this.character.waittime % 8 != 4)
                return;

            // Damage per shot: 10 (5 if melt status)
            var damage = this.Power / player.busterPower;

            // If capped at 15 shots, increase per-shot damage (adjusted for even numbers per shot)
            if (maxShots)
            {
                numShots = 15;
                damage += player.busterPower - 8 + 1;

                var shotNumber = this.character.waittime / 8;
                var shotAdjustmentNumber = shotNumber % 3;
                var perShotAdjustment = (player.busterPower - shotAdjustmentNumber - 1) / 3 - 2;

                if (this.player.badstatus[1])
                {
                    perShotAdjustment = -perShotAdjustment - 1;
                }

                damage += perShotAdjustment;
            }

            this.battle.effects.Add(new BulletShells(this.sound, this.battle, this.character.position, this.character.positionDirect.X + 4 * this.character.UnionRebirth, this.character.positionDirect.Y + 8f, 26, this.character.union, 20 + this.Random.Next(20), 2, 0));
            this.sound.PlaySE(SoundEffect.vulcan);

            this.character.parent.attacks.Add(this.CounterNone(new Vulcan(this.sound, this.character.parent, this.character.position.X + this.UnionRebirth(this.character.union), this.character.position.Y, this.character.union, damage, Vulcan.SHOT.Vulcan, ChipBase.ELEMENT.normal, false)));
        }

        public override void Render(IRenderer dg, Vector2 position, string picturename)
        {
            this._rect = new Rectangle(10 * this.character.Wide, 4 * this.character.Height, this.character.Wide, this.character.Height);
            this._position = new Vector2(this.character.positionDirect.X + Shake.X, this.character.positionDirect.Y + Shake.Y);
            if (this.character.waittime % 8 >= 2 && this.character.waittime % 8 <= 3)
                this._rect.X += 120;
            else if (this.character.waittime % 8 >= 4 && this.character.waittime % 8 <= 5)
                this._rect.X += 240;
            dg.DrawImage(dg, "weapons", this._rect, false, this._position, this.character.union == Panel.COLOR.blue, Color.White);
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

        public static Point Animation(int waittime)
        {
            int[] interval = new int[12];
            for (int index = 0; index < 12; ++index)
                interval[index] = 4 * index;
            int[] xpoint = new int[14]
            {
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6
            };
            int y = 0;
            return CharacterAnimation.Return(interval, xpoint, y, waittime);
        }
    }
}
