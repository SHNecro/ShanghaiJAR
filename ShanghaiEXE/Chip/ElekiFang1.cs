﻿using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
    internal class ElekiFang1 : ChipBase
  {
    private const int speed = 4;
    private const int shotend = 16;

    public ElekiFang1(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-1, 0);
      this.number = sbyte.MaxValue;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ElekiFang1Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 80;
      this.subpower = 0;
      this.regsize = 18;
      this.reality = 1;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.A;
      this.code[1] = ChipFolder.CODE.I;
      this.code[2] = ChipFolder.CODE.R;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ElekiFang1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime < 5)
        character.animationpoint = new Point(4, 0);
      else if (character.waittime < 15)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime < 16)
        character.animationpoint = new Point(6, 0);
      else if (character.waittime < 21)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime == 40)
        base.Action(character, battle);
      if (character.waittime != 16)
        return;
      this.sound.PlaySE(SoundEffect.thunder);
      int num = this.power + this.pluspower;
      ElekiFang elekiFang = new ElekiFang(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 1, this.element, false);
      character.parent.attacks.Add(this.Paralyze(elekiFang));
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
        this._rect = new Rectangle(0, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic21", this._rect, true, p, Color.White);
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

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime < 5)
        return;
      this._rect = new Rectangle(11 * character.Wide, 6 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X -= 120;
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

