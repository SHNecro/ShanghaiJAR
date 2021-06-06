using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class PowerAura : ChipBase
  {
    private const int speed = 2;

    public PowerAura(IAudioEngine s)
      : base(s)
    {
      this.number = 150;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PowerAuraName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 20;
      this.regsize = 37;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.shild = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.D;
      this.code[2] = ChipFolder.CODE.F;
      this.code[3] = ChipFolder.CODE.P;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PowerAuraDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      this.sound.PlaySE(SoundEffect.enterenemy);
      character.barrierType = CharacterBase.BARRIER.PowerAura;
      character.barierPower = 80;
      character.barierTime = 7200;
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
        this._rect = new Rectangle(728, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic1", this._rect, true, p, Color.White);
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

    public override void Render(IRenderer dg, CharacterBase player)
    {
    }
  }
}

