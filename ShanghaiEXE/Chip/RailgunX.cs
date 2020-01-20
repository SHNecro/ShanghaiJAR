﻿using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering.DirectX9;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
  internal class RailgunX : ChipBase
  {
    private bool open;
    private const int shotend = 10;

    public RailgunX(MyAudio s)
      : base(s)
    {
      this.rockOnPoint = new Point(-2, 0);
      this.number = 356;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.RailgunXName");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 250;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = false;
      this.crack = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.R;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.R;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.RailgunXDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime < 1)
        character.animationpoint = new Point(4, 0);
      else if (character.waittime < 3)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime < 10)
        character.animationpoint = new Point(6, 0);
      else if (character.waittime == 5)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime == 24)
        base.Action(character, battle);
      if (character.waittime != 6)
        return;
      this.sound.PlaySE(MyAudio.SOUNDNAMES.canon);
      Point point = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y);
      Vector2 vector2 = new Vector2(character.positionDirect.X + 30 * this.UnionRebirth(character.union), character.positionDirect.Y - 3f);
      character.parent.attacks.Add(this.Paralyze(new BustorShot(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), BustorShot.SHOT.railgun, ChipBase.ELEMENT.eleki, false, 0)));
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 114.5f;
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
        this._rect = new Rectangle(336, 0, 56, 48);
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
        this._rect = new Rectangle(144, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime < 5)
        return;
      int x = 10 * character.Wide;
      int height1 = character.Height;
      int wide = character.Wide;
      int height2 = character.Height;
      this._rect = new Rectangle(x, 0, wide, height2);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime > 5 && character.waittime < 8)
        this._rect.X += 120;
      else if (character.waittime >= 15 && character.waittime < 10)
        this._position.X -= 2 * this.UnionRebirth(character.union);
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

