using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class CirnoX : ChipBase
  {
    private const int start = 44;
    private const int speed = 2;
    private Point animePoint;

    public CirnoX(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 401;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.CirnoXName");
      this.element = ChipBase.ELEMENT.aqua;
      this.power = 210;
      this.subpower = 30;
      this.regsize = 99;
      this.reality = 5;
      this._break = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.C;
      this.code[2] = ChipFolder.CODE.C;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.CirnoXDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    protected Point Animation(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[10]
      {
        1,
        4,
        4,
        4,
        4,
        12,
        4,
        4,
        4,
        4
      }, new int[10]{ -1, 9, 8, 7, 3, 0, 3, 7, 13, 14 }, 0, waittime);
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
          this.sound.PlaySE(SoundEffect.shotwave);
          Tower tower = new Tower(this.sound, battle, character.position.X, character.position.Y, character.union, 0, -1, ChipBase.ELEMENT.aqua);
          tower.hitting = false;
          battle.attacks.Add(tower);
          break;
        case 28:
          this.sound.PlaySE(SoundEffect.water);
          break;
        case 44:
          this.sound.PlaySE(SoundEffect.sand);
          character.parent.attacks.Add(this.Paralyze(new CirnoChip(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), true)));
          break;
      }
      if (character.waittime > 100 && this.BlackOutEnd(character, battle))
        base.Action(character, battle);
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 220.5f;
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
        this._rect = new Rectangle(56, 0, 56, 48);
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
      if (character.animationpoint.X == -1 && character.waittime < 44)
      {
        this._rect = new Rectangle(40 * this.animePoint.X, 112, 40, 56);
        this._position = new Vector2((float) (character.position.X * 40.0 + 20.0), (float) (character.position.Y * 24.0 + 58.0));
        dg.DrawImage(dg, "cirno", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

