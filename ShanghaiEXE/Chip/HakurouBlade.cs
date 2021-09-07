using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class HakurouBlade : ChipBase
    {
        private const int start = 3;
        private const int speed = 2;

        public HakurouBlade(IAudioEngine s)
          : base(s)
        {
            this.rockOnPoint = new Point(-1, 0);
            this.infight = true;
            this.swordtype = true;
            this.number = 61;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.HakurouBladeName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 150;
            this.subpower = 0;
            this.regsize = 46;
            this.reality = 4;
            this._break = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.H;
            this.code[1] = ChipFolder.CODE.L;
            this.code[2] = ChipFolder.CODE.R;
            this.code[3] = ChipFolder.CODE.Y;
            var information = NSGame.ShanghaiEXE.Translate("Chip.HakurouBladeDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (character.waittime == 3)
                this.sound.PlaySE(SoundEffect.sword);
            character.animationpoint = CharacterAnimation.SworsAnimation(character.waittime);
            if (character.waittime >= 30)
                base.Action(character, battle);
            if (character.waittime != 10)
                return;
            int num = this.power + this.pluspower;
            bool flag = false;
            if (character is Player && ((Player)character).style == Player.STYLE.shinobi)
                flag = true;
            foreach (CharacterBase characterBase in battle.AllChara())
            {
                if (characterBase.union == character.UnionEnemy && (characterBase.guard != CharacterBase.GUARD.none || characterBase.invincibilitytime > 0 || characterBase.nohit || (uint)characterBase.barrierType > 0U))
                {
                    var hpDecrease = this.Power(character);
                    characterBase.Hp -= hpDecrease;
                    this.sound.PlaySE(SoundEffect.damageenemy);
                    characterBase.Dameged(new NSAttack.AttackBase(this.sound, battle, characterBase.position.X, characterBase.position.Y, character.union, hpDecrease, ELEMENT.normal));
                }
            }
        }

        public override void GraphicsRender(
          IRenderer dg,
          Vector2 p,
          int c,
          bool printgraphics,
          bool printstatus)
        {
            if (printgraphics)
            {
                this._rect = new Rectangle(448, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic9", this._rect, true, p, Color.White);
            }
            base.GraphicsRender(dg, p, c, printgraphics, printstatus);
        }

        public override void IconRender(
          IRenderer dg,
          Vector2 p,
          bool select,
          bool custom,
          int c,
          bool noicon)
        {
            if (!noicon)
            {
                this._rect = this.IconRect(select);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (character.waittime > 25)
                return;
            this._rect = new Rectangle(character.animationpoint.X * character.Wide, 5 * character.Height, character.Wide, character.Height);
            this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
            dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
        }
    }
}

