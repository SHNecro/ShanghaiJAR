using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MetalAura : ChipBase
  {
    private const int speed = 2;

    public MetalAura(IAudioEngine s)
      : base(s)
    {
      this.number = 152;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MetalAuraName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 20;
      this.regsize = 71;
      this.reality = 5;
      this._break = false;
      this.powerprint = false;
      this.shild = true;
      this.code[0] = ChipFolder.CODE.M;
      this.code[1] = ChipFolder.CODE.T;
      this.code[2] = ChipFolder.CODE.K;
      this.code[3] = ChipFolder.CODE.G;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MetalAuraDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      this.sound.PlaySE(SoundEffect.enterenemy);
      character.barrierType = CharacterBase.BARRIER.MetalAura;
      character.barierPower = 0;
      character.barierTime = 1600;
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
        this._rect = new Rectangle(840, 0, 56, 48);
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

