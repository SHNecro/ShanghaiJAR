﻿using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using SlimDX;
using System.Drawing;

namespace NSChip
{
    internal class ShotGun1 : ChipBase
  {
    private const int shotend = 10;

    public ShotGun1(MyAudio s)
      : base(s)
    {
      this.rockOnPoint = new Point(-2, 0);
      this.number = 121;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ShotGun1Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 20;
      this.regsize = 14;
      this.reality = 1;
      this.subpower = 0;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.P;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ShotGun1Desc");
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
      else if (character.waittime == 15)
        base.Action(character, battle);
      if (character.waittime != 6)
        return;
      int num = this.power + this.pluspower;
      battle.effects.Add(new BulletBigShells(this.sound, battle, character.position, character.positionDirect.X + 4 * character.UnionRebirth, character.positionDirect.Y, 26, character.union, 20 + this.Random.Next(20), 2, 0));
      character.parent.attacks.Add(this.Paralyze(new ShotGun(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), this.element)));
      character.parent.attacks.Add(this.Paralyze(new ShotGun(this.sound, character.parent, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y - 1, character.union, this.Power(character), this.element)));
      character.parent.attacks.Add(this.Paralyze(new ShotGun(this.sound, character.parent, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), this.element)));
      character.parent.attacks.Add(this.Paralyze(new ShotGun(this.sound, character.parent, character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y + 1, character.union, this.Power(character), this.element)));
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
        this._rect = new Rectangle(224, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic4", this._rect, true, p, Color.White);
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
      this._rect = new Rectangle(character.Wide, 6 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 6)
        this._rect.X = 0;
      else if (character.waittime >= 8 && character.waittime < 10)
        this._position.X -= 2 * this.UnionRebirth(character.union);
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

