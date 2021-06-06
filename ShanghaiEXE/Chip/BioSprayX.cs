using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class BioSprayX : ChipBase
  {
    private const int shotend = 10;

    public BioSprayX(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-2, 0);
      this.number = 361;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.BioSprayXName");
      this.element = ChipBase.ELEMENT.poison;
      this.power = 320;
      this.regsize = 99;
      this.reality = 5;
      this.subpower = 2;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.B;
      this.code[2] = ChipFolder.CODE.B;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.BioSprayXDesc");
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
      ++this.frame;
      if (this.frame < this.power / this.subpower && (!Input.IsUp(Button._A) || !(character is Player)))
        return;
      this.frame = 0;
      base.Action(character, battle);
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 135.5f;
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
        this._rect = new Rectangle(616, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic17", this._rect, true, p, Color.White);
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
        int num1 = this.number - 1;
        int num2 = num1 % 40;
        int num3 = num1 / 40;
        int num4 = 0;
        if (select)
          num4 = 1;
        this._rect = new Rectangle(224, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
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

