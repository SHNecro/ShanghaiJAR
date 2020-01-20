﻿using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using SlimDX;
using System.Drawing;

namespace NSChip
{
  internal class HellsHockey : ChipBase
  {
    private const int start = 1;
    private const int speed = 2;

    public HellsHockey(MyAudio s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.number = 289;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.HellsHockeyName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 100;
      this.subpower = 0;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.none;
      this.code[1] = ChipFolder.CODE.none;
      this.code[2] = ChipFolder.CODE.none;
      this.code[3] = ChipFolder.CODE.none;
      var information = NSGame.ShanghaiEXE.Translate("Chip.HellsHockeyDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime <= 1)
        character.animationpoint = new Point(0, 1);
      else if (character.waittime <= 7)
        character.animationpoint = new Point((character.waittime - 1) / 2, 1);
      else if (character.waittime <= 15)
        character.animationpoint = new Point(3, 1);
      else
        base.Action(character, battle);
      if (character.waittime != 5)
        return;
      int num = this.power + this.pluspower;
      this.sound.PlaySE(MyAudio.SOUNDNAMES.knife);
      character.parent.attacks.Add(this.Paralyze(new ShellHockey(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 1, this.element)));
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
      string[] strArray = new string[3]
      {
        ShanghaiEXE.Translate("Chip.ProgramAdvanceHellsHockeyCombo1Line1"),
        ShanghaiEXE.Translate("Chip.ProgramAdvanceHellsHockeyCombo1Line2"),
        ShanghaiEXE.Translate("Chip.ProgramAdvanceHellsHockeyCombo1Line3")
      };
      for (int index = 0; index < strArray.Length; ++index)
      {
        this._position = new Vector2(p.X - 12f, p.Y - 8f + index * 16);
        this.TextRender(dg, strArray[index], false, this._position, false, Color.LightBlue);
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
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      this._rect = new Rectangle(character.animationpoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}


