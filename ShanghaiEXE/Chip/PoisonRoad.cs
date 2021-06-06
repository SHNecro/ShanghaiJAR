using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class PoisonRoad : ChipBase
  {
    private const int speed = 2;

    public PoisonRoad(IAudioEngine s)
      : base(s)
    {
      this.number = 170;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PoisonRoadName");
      this.element = ChipBase.ELEMENT.poison;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 22;
      this.reality = 2;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.asterisk;
      this.code[1] = ChipFolder.CODE.asterisk;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PoisonRoadDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 1)
      {
        this.sound.PlaySE(SoundEffect.eriasteal2);
        for (int pX = 0; pX < battle.panel.GetLength(0); ++pX)
        {
          int y = character.position.Y;
          if (battle.panel[pX, y].State != Panel.PANEL._none && battle.panel[pX, y].State != Panel.PANEL._un)
          {
            battle.panel[pX, y].state = Panel.PANEL._poison;
            battle.effects.Add(new Smoke(this.sound, battle, pX, y, this.element));
          }
        }
      }
      if (character.waittime < 12)
        return;
      if (character is Player)
        ((Player) character).PluspointWing(20);
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
        this._rect = new Rectangle(672, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic8", this._rect, true, p, Color.White);
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

