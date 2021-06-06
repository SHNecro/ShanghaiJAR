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
    internal class TortoiseManV1 : ChipBase
  {
    private const int speed = 2;
    protected Point animePoint;

    public TortoiseManV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 209;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.TortoiseManV1Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 80;
      this.subpower = 0;
      this.regsize = 47;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.T;
      this.code[1] = ChipFolder.CODE.H;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.TortoiseManV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      int num = 3;
      return CharacterAnimation.ReturnKai(new int[21]
      {
        num,
        num,
        num,
        num,
        num,
        num,
        num * 4,
        num,
        num * 3,
        num,
        num,
        num,
        num * 3,
        num,
        num,
        num,
        num * 6,
        num,
        num,
        num,
        100
      }, new int[21]
      {
        7,
        6,
        5,
        4,
        3,
        2,
        0,
        11,
        12,
        16,
        17,
        11,
        12,
        16,
        17,
        11,
        12,
        13,
        14,
        15,
        15
      }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
      {
        if (character.waittime <= 140)
          this.animePoint = this.Animation(character.waittime);
        switch (character.waittime)
        {
          case 1:
            character.animationpoint.X = -1;
            this.sound.PlaySE(SoundEffect.warp);
            break;
          case 40:
            this.sound.PlaySE(SoundEffect.knock);
            break;
          case 58:
            this.sound.PlaySE(SoundEffect.knock);
            break;
          case 73:
            this.ShakeStart(1, 67);
            break;
          case 85:
            int pX = this.TargetX(character, battle);
            int pY = 1;
            this.sound.PlaySE(SoundEffect.wave);
            battle.attacks.Add(this.Paralyze(new MadWave(this.sound, battle, pX, pY, character.union, this.Power(character), 6, this.element)));
            break;
          case 140:
            this.animePoint.X = -1;
            battle.effects.Add(new MoveEnemy(this.sound, battle, character.position.X, character.position.Y));
            break;
        }
      }
      if (character.waittime <= 160 || !this.BlackOutEnd(character, battle))
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
        this._rect = new Rectangle(336, 0, 56, 48);
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
        this._rect = new Rectangle(128 * this.animePoint.X, 0, 128, 112);
        this._position = new Vector2((float) (character.position.X * 40.0 + 16.0 + 8.0), (float) (character.position.Y * 24.0 + 48.0));
        dg.DrawImage(dg, "TortoiseMan", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
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

