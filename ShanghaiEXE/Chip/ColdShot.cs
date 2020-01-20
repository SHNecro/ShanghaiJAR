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
  internal class ColdShot : ChipBase
  {
    private bool open;
    private const int shotend = 28;

    public ColdShot(MyAudio s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 23;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ColdShotName");
      this.element = ChipBase.ELEMENT.aqua;
      this.power = 110;
      this.subpower = 0;
      this.regsize = 40;
      this.reality = 3;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.A;
      this.code[1] = ChipFolder.CODE.C;
      this.code[2] = ChipFolder.CODE.J;
      this.code[3] = ChipFolder.CODE.O;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ColdShotDesc");
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
      else if (character.waittime < 28)
      {
        this.open = true;
        character.animationpoint = new Point(6, 0);
        if (character.waittime < 17)
          character.positionDirect.X -= (character.waittime - 15) * this.UnionRebirth(character.union);
      }
      else if (character.waittime < 33)
      {
        character.animationpoint = new Point(5, 0);
        character.PositionDirectSet();
      }
      else if (character.waittime == 33)
        base.Action(character, battle);
      if (character.waittime != 18)
        return;
      this.sound.PlaySE(MyAudio.SOUNDNAMES.chain);
      Point end = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y);
      Vector2 v = new Vector2(character.positionDirect.X + 30 * this.UnionRebirth(character.union), character.positionDirect.Y - 3f);
      int num = this.power + this.pluspower;
      NSAttack.PoisonShot poisonShot = new NSAttack.PoisonShot(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, v, this.element, end, 10, true, NSAttack.PoisonShot.TYPE.ice);
      poisonShot.badstatus[(int) this.element] = true;
      poisonShot.badstatustime[(int) this.element] = 1200;
      poisonShot.invincibility = false;
      character.parent.attacks.Add(this.Paralyze(poisonShot));
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

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.waittime < 5)
        return;
      this._rect = new Rectangle(!this.open ? character.Wide : character.Wide * 2, character.Height * 3, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X = 0;
      else if (character.waittime >= 15 && character.waittime < 28)
        this._position.X -= 2 * this.UnionRebirth(character.union);
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

