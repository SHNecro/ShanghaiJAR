using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class DarkSand : ChipBase
  {
    private const int speed = 2;

    public DarkSand(IAudioEngine s)
      : base(s)
    {
      this.number = 259;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.DarkSandName");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 130;
      this.subpower = 0;
      this.regsize = 99;
      this.reality = 5;
      this._break = false;
      this.shadow = false;
      this.dark = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.D;
      this.code[1] = ChipFolder.CODE.D;
      this.code[2] = ChipFolder.CODE.D;
      this.code[3] = ChipFolder.CODE.D;
      var information = NSGame.ShanghaiEXE.Translate("Chip.DarkSandDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      try
      {
        Point[] randamPanel = this.GetRandamPanel(5, character.UnionEnemy, false, character, false);
        Panel.COLOR color = character.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red;
        if ((uint) randamPanel.Length > 0U)
        {
          for (int index = 0; index < 5; ++index)
          {
            int num = this.power + this.pluspower;
            battle.attacks.Add(this.Paralyze(new SandHoleAttack(this.sound, battle, randamPanel[index % randamPanel.Length].X, randamPanel[index % randamPanel.Length].Y, character.union, this.Power(character), 300, 3, SandHoleAttack.MOTION.init, this.element)));
          }
        }
      }
      catch
      {
      }
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
        this._rect = new Rectangle(224, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic13", this._rect, true, p, Color.White);
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
        int x = 560;
        int num1 = 64;
        int num2 = this.number - 1;
        int num3 = num2 % 40;
        int num4 = num2 / 40;
        int num5 = 0;
        if (select)
          num5 = 1;
        this._rect = new Rectangle(x, num1 + num5 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }
  }
}

