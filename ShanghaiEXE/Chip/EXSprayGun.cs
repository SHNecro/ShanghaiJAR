using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class EXSprayGun : ChipBase
    {
        private const int start = 5;
        private const int speed = 3;

        public EXSprayGun(IAudioEngine s)
          : base(s)
        {
            this.rockOnPoint = new Point(-2, 0);
            this.number = 277;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.EXSprayGunName");
            this.element = ChipBase.ELEMENT.poison;
            this.power = 600;
            this.subpower = 5;
            this._break = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.none;
            this.code[1] = ChipFolder.CODE.none;
            this.code[2] = ChipFolder.CODE.none;
            this.code[3] = ChipFolder.CODE.none;
            var information = NSGame.ShanghaiEXE.Translate("Chip.EXSprayGunDesc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            this.Init();
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (character.animationpoint.X != 5 || character.waittime == 0)
            {
                this.sound.PlaySE(SoundEffect.switchon);
                character.animationpoint = new Point(5, 0);
            }
            bool gas = false;
            if (character.waittime % 2 == 0)
            {
                gas = true;
                this.sound.PlaySE(SoundEffect.lance);
            }
            battle.attacks.Add(new PoisonGas(this.sound, battle, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y - 1, character.union, this.subpower, gas, this.element));
            battle.attacks.Add(new PoisonGas(this.sound, battle, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y, character.union, this.subpower, gas, this.element));
            battle.attacks.Add(new PoisonGas(this.sound, battle, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y + 1, character.union, this.subpower, gas, this.element));
            battle.attacks.Add(new PoisonGas(this.sound, battle, character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y - 1, character.union, this.subpower, gas, this.element));
            battle.attacks.Add(new PoisonGas(this.sound, battle, character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y, character.union, this.subpower, gas, this.element));
            battle.attacks.Add(new PoisonGas(this.sound, battle, character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y + 1, character.union, this.subpower, gas, this.element));
            ++this.frame;
            if (this.frame < this.power / this.subpower && (!Input.IsUp(Button._A) || !(character is Player)))
                return;
            this.frame = 0;
            base.Action(character, battle);
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
                    this._rect = new Rectangle(56 * 6, 48 * 0, 56, 48);
                    dg.DrawImage(dg, "pagraphic1", this._rect, true, p, Color.White);
                    return;
                case 1:
                    string[] strArray =
                    {
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceEXSprayGunCombo1Line1"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceEXSprayGunCombo1Line2"),
                        ShanghaiEXE.Translate("Chip.ProgramAdvanceEXSprayGunCombo1Line3")
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
            if (character.waittime < 0)
                return;
            this._rect = new Rectangle(8 * character.Wide, character.Height, character.Wide, character.Height);
            this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
            if (character.waittime % 2 == 1)
                this._rect.X += character.Wide;
            dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
        }
    }
}


