using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class GrassStage : ChipBase
  {
    private const int speed = 2;

    public GrassStage(IAudioEngine s)
      : base(s)
    {
      this.number = 165;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.GrassStageName");
      this.element = ChipBase.ELEMENT.leaf;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 20;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.G;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.GrassStageDesc");
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
          for (int pY = 0; pY < battle.panel.GetLength(1); ++pY)
          {
            if (battle.panel[pX, pY].State != Panel.PANEL._none && battle.panel[pX, pY].State != Panel.PANEL._un)
            {
              battle.panel[pX, pY].state = Panel.PANEL._grass;
              battle.effects.Add(new Smoke(this.sound, battle, pX, pY, this.element));
            }
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
        this._rect = new Rectangle(112, 0, 56, 48);
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

