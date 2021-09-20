using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class AuraSlash : ChipBase
    {
        private int count = 0;
        private bool aura = false;
        private const int start = 3;
        private const int speed = 8;

        public AuraSlash(IAudioEngine s)
          : base(s)
        {
            this.rockOnPoint = new Point(-3, 0);
            this.infight = true;
            this.swordtype = true;
            this.number = 278;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.AuraSlashName");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 60;
            this.subpower = 0;
            this._break = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.AuraSlashDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (character.waittime == 3)
            {
                this.sound.PlaySE(SoundEffect.sword);
                if ((uint)character.barrierType > 0U)
                {
                    this.sound.PlaySE(SoundEffect.shoot);
                    this.aura = true;
                }
            }
            character.animationpoint = CharacterAnimation.SworsAnimation(character.waittime);
            if (this.count < 5)
            {
                if (character.waittime >= 21)
                {
                    ++this.count;
                    character.waittime = 0;
                }
            }
            else if (character.waittime >= 30)
                base.Action(character, battle);
            if (character.waittime != 10)
                return;
            int num = this.power + this.pluspower;
            AttackBase a = new SonicBoom(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 8, this.element, true);
            a.invincibility = this.count >= 5;
            character.parent.attacks.Add(this.Paralyze(a, character));
        }

        public override void GraphicsRender(
          IRenderer dg,
          Vector2 p,
          int c,
          bool printgraphics,
          bool printstatus)
        {
            if (!printgraphics)
                return;
            switch (c % 2)
            {
                case 0:
                    this._rect = new Rectangle(848, 320, 74, 79);
                    dg.DrawImage(dg, "menuwindows", this._rect, true, p - new Vector2(9, 16), Color.White);
                    this._rect = new Rectangle(56 * 7, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray = 
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceAuraSlashCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceAuraSlashCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceAuraSlashCombo1Line3")
                    };
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                        this._position = new Vector2(p.X - 12f, p.Y - 8f + index * 16);
                        this.TextRender(dg, strArray[index], false, this._position, false, Color.LightBlue);
                    }
                    return;
            }
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
                int num = 0;
                if (select)
                    num = 1;
                this._rect = new Rectangle(624, 80 + num * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, 0, noicon);
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


