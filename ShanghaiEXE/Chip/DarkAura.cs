using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DarkAura : ChipBase
  {
    private const int speed = 2;

    public DarkAura(IAudioEngine s)
      : base(s)
    {
      this.number = 265;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DarkAuraName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this.dark = true;
      this.shild = true;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.W;
      this.code[1] = ChipFolder.CODE.W;
      this.code[2] = ChipFolder.CODE.W;
      this.code[3] = ChipFolder.CODE.W;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DarkAuraDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      this.sound.PlaySE(SoundEffect.enterenemy);
      character.barrierType = CharacterBase.BARRIER.PowerAura;
      character.barierPower = 200;
      character.barierTime = 1350;
      if (character is Player)
        ((Player) character).PluspointGaia(20);
      base.Action(character, battle);
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
        dg.DrawImage(dg, "chipgraphic13", this._rect, true, p, Color.White);
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
        int x = 544;
        int num1 = 80;
        int num2 = this.number - 1;
        int num3 = num2 % 40;
        int num4 = num2 / 40;
        int num5 = 0;
        if (select)
          num5 = 1;
        this._rect = new Rectangle(x, num1 + num5 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase player)
    {
    }
  }
}

