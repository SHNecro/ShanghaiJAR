using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MarisaX : ChipBase
  {
    private const int speed = 2;
    private Point animePoint;

    public MarisaX(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 400;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MarisaXName");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 230;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.M;
      this.code[1] = ChipFolder.CODE.M;
      this.code[2] = ChipFolder.CODE.M;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MarisaXDesc");
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

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
        return;
      this.animePoint = this.Animation(character.waittime);
      switch (character.waittime)
      {
        case 1:
          character.animationpoint.X = -1;
          this.sound.PlaySE(SoundEffect.warp);
          break;
        case 50:
          this.sound.PlaySE(SoundEffect.beam);
          AttackBase a = new Beam(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, false);
          a.positionDirect.Y += 3f;
          character.parent.attacks.Add(this.Paralyze(a));
          break;
      }
      if (character.waittime > 140 && this.BlackOutEnd(character, battle))
        base.Action(character, battle);
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 193.5f;
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
        dg.DrawImage(dg, "chipgraphic18", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(592, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.animationpoint.X == -1)
      {
        this._rect = new Rectangle(48 * this.animePoint.X, 128, 48, 64);
        this._position = new Vector2((float) (character.position.X * 40.0 + 16.0 + 8.0), (float) (character.position.Y * 24.0 + 58.0));
        dg.DrawImage(dg, "marisa", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

