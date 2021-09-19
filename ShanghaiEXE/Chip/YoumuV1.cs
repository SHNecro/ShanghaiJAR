using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSNet;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class YoumuV1 : ChipBase
  {
    private const int interval = 20;
    private const int speed = 2;
    private int[] motionList;
    private int nowmotion;
    private bool end;
    private Point target;
    private int command;
    protected int xPosition;
    private const int s = 5;
    protected Point animePoint;

    public YoumuV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 242;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.YoumuV1Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 80;
      this.subpower = 0;
      this.regsize = 34;
      this.reality = 3;
      this.swordtype = true;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.Y;
      this.code[1] = ChipFolder.CODE.K;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.YoumuV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
      this.motionList = new int[4]{ 0, 1, 2, 3 };
    }

    protected Point Animation(int waittime)
    {
      int[] interval = new int[6]{ 4, 1, 1, 8, 1, 60 };
      int[] xpoint = new int[6]{ 0, 8, 12, 13, 16, 17 };
      for (int index = 0; index < interval.Length; ++index)
        interval[index] *= 5;
      int y = 0;
      return CharacterAnimation.ReturnKai(interval, xpoint, y, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
        return;
      if (this.moveflame && this.nowmotion == 0)
      {
        switch (this.frame)
        {
          case 1:
            character.animationpoint.X = -1;
            this.xPosition = character.position.X;
            this.sound.PlaySE(SoundEffect.warp);
            this.animePoint = new Point(2, 0);
            break;
          case 2:
            this.animePoint = new Point(1, 0);
            break;
          case 3:
            this.animePoint = new Point(0, 0);
            this.target = new Point(character.position.X, character.position.Y);
            this.target.X = this.TargetX(character, battle) + this.UnionRebirth(character.union);
            if (this.target.X < 0)
              this.target.X = 0;
            if (this.target.X > 5)
            {
              this.target.X = 5;
              break;
            }
            break;
          case 15:
            this.sound.PlaySE(SoundEffect.pikin);
            battle.effects.Add(new Flash(this.sound, battle, character.positionDirect, character.position));
            this.animePoint = new Point(0, 4);
            break;
          case 18:
            this.animePoint = new Point(1, 4);
            break;
          case 19:
            this.xPosition = character.union == Panel.COLOR.red ? 5 : 0;
            this.animePoint = new Point(2, 4);
            break;
          case 20:
            this.animePoint = new Point(3, 4);
            break;
          case 25:
            this.sound.PlaySE(SoundEffect.breakObject);
            SwordCloss swordCloss = new SwordCloss(this.sound, battle, this.target.X, this.target.Y, character.union, this.Power(character), 2, this.element, false);
            swordCloss.breakinvi = true;
            battle.attacks.Add(this.Paralyze(swordCloss));
            break;
          case 51:
            this.animePoint = new Point(2, 4);
            break;
          case 52:
            this.animePoint = new Point(2, 0);
            break;
          case 53:
            this.animePoint = new Point(-1, 0);
            character.animationpoint.X = 0;
            this.end = true;
            break;
        }
      }
      if (this.end && this.BlackOutEnd(character, battle))
        base.Action(character, battle);
      this.FlameControl(2);
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
        if (this.end)
          return;
        this._rect = new Rectangle(this.animePoint.X * 104, this.animePoint.Y * 96, 104, 96);
        this._position = new Vector2((float) (xPosition * 40.0 + 28.0) + 4 * this.UnionRebirth(character.union), (float) (character.position.Y * 24.0 + 48.0));
        dg.DrawImage(dg, "youmu", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }

    protected Point AnimeMove(int waitflame)
    {
      return CharacterAnimation.Return(new int[4]
      {
        1,
        1,
        1,
        100
      }, new int[4]{ 0, 1, 2, 3 }, 5, waitflame);
    }

    protected Point AnimeSlash1(int waitflame)
    {
      return CharacterAnimation.Return(new int[7]
      {
        5,
        6,
        7,
        8,
        9,
        10,
        100
      }, new int[7]{ 5, 6, 7, 8, 9, 10, 11 }, 0, waitflame);
    }

    protected Point AnimeSlash2(int waitflame)
    {
      return CharacterAnimation.Return(new int[7]
      {
        5,
        6,
        7,
        8,
        9,
        10,
        100
      }, new int[7]{ 12, 13, 14, 15, 16, 17, 18 }, 0, waitflame);
    }

    private Point AnimeSlash3(int waitflame)
    {
      return CharacterAnimation.Return(new int[7]
      {
        5,
        6,
        7,
        8,
        9,
        10,
        100
      }, new int[7]{ 19, 20, 21, 22, 23, 24, 25 }, 0, waitflame);
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
        else if (characterBase is Player)
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
        if (characterBase.position.Y == character.position.Y)
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
      }
      if (flag)
        return num - this.UnionRebirth(character.union);
      foreach (CharacterBase characterBase in characterBaseList)
      {
        if (characterBase.position.Y != character.position.Y)
        {
          if (character.union == Panel.COLOR.red)
          {
            if (num > characterBase.position.X)
              num = characterBase.position.X;
          }
          else if (num < characterBase.position.X)
            num = characterBase.position.X;
        }
      }
      return num;
    }
  }
}

