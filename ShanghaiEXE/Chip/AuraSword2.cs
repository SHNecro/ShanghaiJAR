﻿using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using SlimDX;
using System.Drawing;

namespace NSChip
{
    internal class AuraSword2 : ChipBase
  {
    private int count = 0;
    private bool aura = false;
    private const int start = 3;
    private const int speed = 8;

    public AuraSword2(MyAudio s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.infight = true;
      this.sideOnly = true;
      this.swordtype = true;
      this.number = 125;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.AuraSword2Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 60;
      this.subpower = 0;
      this.regsize = 22;
      this.reality = 2;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.O;
      this.code[2] = ChipFolder.CODE.R;
      this.code[3] = ChipFolder.CODE.V;
      var information = NSGame.ShanghaiEXE.Translate("Chip.AuraSword2Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime == 3)
      {
        this.sound.PlaySE(MyAudio.SOUNDNAMES.sword);
        if ((uint) character.barrierType > 0U)
        {
          this.sound.PlaySE(MyAudio.SOUNDNAMES.shoot);
          this.aura = true;
        }
      }
      character.animationpoint = CharacterAnimation.SworsAnimation(character.waittime);
      if (this.count < 1)
      {
        if (character.waittime >= 21)
        {
          ++this.count;
          character.waittime = 0;
        }
      }
      else if (character.waittime >= 30)
        base.Action(character, battle);
      if (character.waittime != 10)
        return;
      int num = this.power + this.pluspower;
      AttackBase a = new SonicBoom(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 8, this.element, this.aura);
      a.invincibility = this.count >= 1;
      character.parent.attacks.Add(this.Paralyze(a, character));
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
        this._rect = new Rectangle(616, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic2", this._rect, true, p, Color.White);
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
      if (character.waittime > 25)
        return;
      this._rect = new Rectangle(character.animationpoint.X * character.Wide, 5 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

