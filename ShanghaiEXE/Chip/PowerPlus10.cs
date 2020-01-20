﻿using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using SlimDX;
using System.Drawing;

namespace NSChip
{
  internal class PowerPlus10 : ChipBase
  {
    private const int speed = 2;

    public PowerPlus10(MyAudio s)
      : base(s)
    {
      this.number = 188;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PowerPlus10Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 4;
      this.reality = 1;
      this.plusing = true;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.asterisk;
      this.code[1] = ChipFolder.CODE.asterisk;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PowerPlus10Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime != 1)
        return;
      this.sound.PlaySE(MyAudio.SOUNDNAMES.heat);
      Vector2 pd = new Vector2(character.positionDirect.X - 8f, character.positionDirect.Y - 32f);
      battle.effects.Add(new Smoke(this.sound, battle, pd, character.position, ChipBase.ELEMENT.normal));
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
        this._rect = new Rectangle(392, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic3", this._rect, true, p, Color.White);
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

