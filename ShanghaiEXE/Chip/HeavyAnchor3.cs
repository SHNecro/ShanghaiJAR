﻿using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
    internal class HeavyAnchor3 : ChipBase
  {
    private const int start = 3;
    private const int speed = 3;

    public HeavyAnchor3(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 48;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.HeavyAnchor3Name");
      this.element = ChipBase.ELEMENT.aqua;
      this.power = 220;
      this.subpower = 0;
      this.regsize = 41;
      this.reality = 3;
      this._break = true;
      this.crack = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.J;
      this.code[1] = ChipFolder.CODE.N;
      this.code[2] = ChipFolder.CODE.R;
      this.code[3] = ChipFolder.CODE.T;
      var information = NSGame.ShanghaiEXE.Translate("Chip.HeavyAnchor3Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 3)
        this.sound.PlaySE(SoundEffect.throw_);
      character.animationpoint = CharacterAnimation.BombAnimation(character.waittime);
      if (character.waittime == 6)
      {
        int num = this.power + this.pluspower;
        Point end = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y);
        battle.attacks.Add(this.Paralyze(new AnchorBomb(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, 30, AnchorBomb.TYPE.closs, -1)));
      }
      if (character.waittime != 25)
        return;
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
        this._rect = new Rectangle(280, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic20", this._rect, true, p, Color.White);
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
      if (character.waittime >= 6)
        return;
      this._rect = new Rectangle(0, 0, 16, 16);
      Point point = new Point();
      if (character.waittime <= 3)
      {
        point.X = -22 * this.UnionRebirth(character.union);
        point.Y = 22;
      }
      else
      {
        point.X = -10 * this.UnionRebirth(character.union);
        point.Y = 4;
      }
      double x1 = character.positionDirect.X;
      Point shake = this.Shake;
      double x2 = shake.X;
      double num1 = x1 + x2 + point.X;
      double y1 = character.positionDirect.Y;
      shake = this.Shake;
      double y2 = shake.Y;
      double num2 = y1 + y2 + point.Y;
      this._position = new Vector2((float) num1, (float) num2);
      dg.DrawImage(dg, "bombs", this._rect, false, this._position, Color.White);
    }
  }
}

