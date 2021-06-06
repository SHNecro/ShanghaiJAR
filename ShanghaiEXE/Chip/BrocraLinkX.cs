using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class BrocraLinkX : ChipBase
  {
    private const int speed = 2;

    public BrocraLinkX(IAudioEngine s)
      : base(s)
    {
      this.number = 364;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.BrocraLinkXName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 300;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = false;
      this.shadow = false;
      this.crack = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.B;
      this.code[2] = ChipFolder.CODE.B;
      this.code[3] = ChipFolder.CODE.X;
      var information = NSGame.ShanghaiEXE.Translate("Chip.BrocraLinkXDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      try
      {
        Panel.COLOR color = character.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red;
        Point point = this.GetRandamPanel(1, character.UnionEnemy, false, character, false)[0];
        int num = this.power + this.pluspower;
        this.sound.PlaySE(SoundEffect.enterenemy);
        battle.attacks.Add(this.Paralyze(new Brocla(this.sound, battle, point.X, point.Y, character.union, this.Power(character), Panel.PANEL._crack, true, this.element)));
      }
      catch
      {
      }
      base.Action(character, battle);
    }

    public override void Init()
    {
      base.Init();
      this.sortNumber = 32.5f;
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
        this._rect = new Rectangle(784, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic17", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(272, 80 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }
  }
}

