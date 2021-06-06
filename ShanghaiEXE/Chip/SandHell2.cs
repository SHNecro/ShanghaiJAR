using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class SandHell2 : ChipBase
  {
    private const int speed = 2;

    public SandHell2(IAudioEngine s)
      : base(s)
    {
      this.number = 119;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SandHell2Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 100;
      this.subpower = 0;
      this.regsize = 34;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.obje = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.D;
      this.code[1] = ChipFolder.CODE.H;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.S;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SandHell2Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      Point[] randamPanel = this.GetRandamPanel(2, character.UnionEnemy, false, character, false);
      Panel.COLOR color = character.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red;
      if ((uint) randamPanel.Length > 0U)
      {
        for (int index = 0; index < 2; ++index)
        {
          int num = this.power + this.pluspower;
          battle.attacks.Add(this.Paralyze(new SandHoleAttack(this.sound, battle, randamPanel[index % randamPanel.Length].X, randamPanel[index % randamPanel.Length].Y, character.union, this.Power(character), 300, 1, SandHoleAttack.MOTION.init, this.element)));
        }
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
        this._rect = new Rectangle(448, 0, 56, 48);
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
  }
}

