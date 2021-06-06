using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace NSChip
{
    internal class RanV1 : ChipBase
  {
    protected int hit = 3;
    public int waittime;
    private const int speed = 2;
    protected int attackMode;
    protected int[] targetY;
    protected int atacks;
    private Charge chargeEffect;
    protected Point animePoint;

    public RanV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 239;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.RanV1Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 20;
      this.subpower = 0;
      this.regsize = 32;
      this.reality = 3;
      this.hit = 5;
      this.targetY = new int[21];
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.R;
      this.code[1] = ChipFolder.CODE.Y;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.RanV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[18]
      {
        1,
        2,
        2,
        4,
        12,
        4,
        4,
        4,
        4,
        4,
        60,
        4,
        4,
        4,
        4,
        2,
        2,
        4
      }, new int[19]
      {
        -1,
        3,
        2,
        1,
        0,
        4,
        5,
        6,
        7,
        8,
        9,
        5,
        6,
        4,
        0,
        1,
        2,
        3,
        -1
      }, 0, waittime);
    }

    private Point AnimeSingleShot(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[4]
      {
        8,
        3,
        3,
        100
      }, new int[4]{ 0, 3, 4, 5 }, 0, waittime);
    }

    private Point AnimeMachinegunRay(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[6]
      {
        3,
        3,
        3,
        3,
        3,
        100
      }, new int[4]{ 5, 6, 7, 5 }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
        return;
      switch (this.attackMode)
      {
        case 0:
          if (this.waittime > 2)
            this.animePoint = this.AnimeSingleShot(this.waittime);
          switch (this.waittime)
          {
            case 1:
              character.animationpoint.X = -1;
              this.animePoint.X = 0;
              this.sound.PlaySE(SoundEffect.warp);
              break;
            case 18:
              this.animePoint.X = 5;
              ++this.attackMode;
              this.waittime = 0;
              break;
          }
break;
case 1:
          switch (this.waittime)
          {
            case 1:
              this.animePoint.X = 5;
              break;
            case 8:
              this.chargeEffect = new Charge(this.sound, battle, character.position.X, character.position.Y);
              battle.effects.Add(chargeEffect);
              break;
            case 40:
              this.chargeEffect.chargeEffect = 2;
              break;
            case 72:
              this.chargeEffect.flag = false;
              for (int index = 0; index < this.targetY.Length; ++index)
                this.targetY[index] = index >= this.hit * 3 ? this.Random.Next(3) : index % 3;
              this.targetY = ((IEnumerable<int>) this.targetY).OrderBy<int, Guid>(i => Guid.NewGuid()).ToArray<int>();
              ++this.attackMode;
              this.waittime = 0;
              break;
          }
break;
case 2:
          this.animePoint = this.AnimeMachinegunRay(this.waittime);
          switch (this.waittime)
          {
            case 3:
              this.sound.PlaySE(SoundEffect.gun);
              battle.attacks.Add(this.Paralyze(new BustorShot(this.sound, battle, character.position.X + this.UnionRebirth(character.union), this.targetY[this.atacks], character.union, this.Power(character), BustorShot.SHOT.ranShot, this.element, false, 6)));
              Debug.WriteLine(atacks);
              break;
            case 9:
              ++this.atacks;
              if (this.atacks >= this.targetY.Length)
              {
                ++this.attackMode;
                this.animePoint.X = 2;
              }
              this.waittime = 0;
              break;
          }
break;
case 3:
          switch (this.waittime)
          {
            case 30:
              this.animePoint.X = -1;
              character.parent.effects.Add(new MoveEnemy(this.sound, character.parent, character.position.X, character.position.Y));
              break;
            case 40:
              character.animationpoint.X = 0;
              break;
          }
          if (this.waittime > 40 && this.BlackOutEnd(character, battle))
          {
            base.Action(character, battle);
            break;
          }
          break;
      }
      ++this.waittime;
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
        dg.DrawImage(dg, "chipgraphic22", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(480, 64 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.animationpoint.X == -1)
      {
        this._rect = new Rectangle(128 * this.animePoint.X, 0, 128, 96);
        this._position = new Vector2((float) (character.position.X * 40.0 + 8.0 + 24.0), (float) (character.position.Y * 24.0 + 44.0));
        dg.DrawImage(dg, "ran", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

