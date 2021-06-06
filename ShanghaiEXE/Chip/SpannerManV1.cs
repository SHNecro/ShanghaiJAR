using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class SpannerManV1 : ChipBase
  {
    private const int speed = 2;
    protected Point animePoint;
    private const int end = 46;

    public SpannerManV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 200;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SpannerManV1Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 80;
      this.subpower = 0;
      this.regsize = 35;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.S;
      this.code[1] = ChipFolder.CODE.N;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SpannerManV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[26]
      {
        1,
        1,
        1,
        2,
        1,
        1,
        1,
        2,
        1,
        1,
        1,
        1,
        1,
        2,
        1,
        1,
        1,
        2,
        1,
        1,
        1,
        15,
        1,
        1,
        1,
        100
      }, new int[26]
      {
        4,
        3,
        2,
        0,
        0,
        17,
        18,
        19,
        20,
        21,
        22,
        23,
        24,
        25,
        0,
        10,
        11,
        12,
        13,
        14,
        15,
        16,
        0,
        2,
        3,
        4
      }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      int num = 3;
      if (this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
      {
        if (character.waittime / num <= 46)
          this.animePoint = this.Animation(character.waittime / num);
        if (character.waittime % num == 0)
        {
          switch (character.waittime / num)
          {
            case 1:
              character.animationpoint.X = -1;
              this.sound.PlaySE(SoundEffect.warp);
              break;
            case 15:
              this.sound.PlaySE(SoundEffect.thunder);
              AttackBase a1 = new BombAttack(this.sound, battle, character.position.X + character.UnionRebirth, character.position.Y, character.union, this.Power(character), 1, 1, ChipBase.ELEMENT.eleki);
              a1.badstatus[3] = true;
              a1.badstatustime[3] = 180;
              a1.invincibility = false;
              battle.attacks.Add(this.Paralyze(a1));
              break;
            case 17:
              this.sound.PlaySE(SoundEffect.thunder);
              break;
            case 25:
              this.sound.PlaySE(SoundEffect.quake);
              this.ShakeStart(1, 80);
              AttackBase a2 = new WaveAttsck(this.sound, battle, character.position.X + character.UnionRebirth, character.position.Y, character.union, this.Power(character), 3, 0, this.element);
              a2.invincibility = false;
              battle.attacks.Add(this.Paralyze(a2));
              AttackBase a3 = new WaveAttsck(this.sound, battle, character.position.X + character.UnionRebirth, character.position.Y - 1, character.union, this.Power(character), 3, 0, this.element);
              a3.invincibility = false;
              battle.attacks.Add(this.Paralyze(a3));
              AttackBase a4 = new WaveAttsck(this.sound, battle, character.position.X + character.UnionRebirth, character.position.Y + 1, character.union, this.Power(character), 3, 0, this.element);
              a4.invincibility = false;
              battle.attacks.Add(this.Paralyze(a4));
              break;
            case 46:
              this.animePoint.X = -1;
              battle.effects.Add(new MoveEnemy(this.sound, battle, character.position.X, character.position.Y));
              break;
          }
        }
      }
      if (character.waittime / num <= 46 || !this.BlackOutEnd(character, battle))
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
        this._rect = new Rectangle(504, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic15", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(112 * this.animePoint.X, 0, 112, 96);
        this._position = new Vector2((float) (character.position.X * 40.0 + 20.0), (float) (character.position.Y * 24.0 + 48.0));
        dg.DrawImage(dg, "spannerman", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }

    protected int TargetX(CharacterBase character, SceneBattle battle)
    {
      List<CharacterBase> characterBaseList = new List<CharacterBase>();
      foreach (CharacterBase characterBase in battle.AllHitter())
      {
        if (characterBase is EnemyBase)
        {
          if (characterBase.union == character.UnionEnemy)
            characterBaseList.Add(characterBase);
        }
        else if (characterBase is ObjectBase)
        {
          ObjectBase objectBase = (ObjectBase) characterBase;
          if ((objectBase.unionhit || objectBase.union == character.union) && character.UnionEnemy == objectBase.StandPanel.color)
            characterBaseList.Add(characterBase);
        }
      }
      bool flag = false;
      int num = character.union == Panel.COLOR.red ? 6 : -1;
      foreach (CharacterBase characterBase in characterBaseList)
      {
        flag = true;
        if (character.union == Panel.COLOR.red)
        {
          if (num > characterBase.position.X)
            num = characterBase.position.X;
        }
        else if (num < characterBase.position.X)
          num = characterBase.position.X;
      }
      return num;
    }
  }
}

