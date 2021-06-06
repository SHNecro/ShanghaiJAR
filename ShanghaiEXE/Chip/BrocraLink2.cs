using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class BrocraLink2 : ChipBase
  {
    private const int speed = 2;

    public BrocraLink2(IAudioEngine s)
      : base(s)
    {
      this.number = 31;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.BrocraLink2Name");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 200;
      this.subpower = 0;
      this.regsize = 40;
      this.reality = 2;
      this._break = false;
      this.shadow = false;
      this.obje = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.I;
      this.code[2] = ChipFolder.CODE.K;
      this.code[3] = ChipFolder.CODE.L;
      var information = NSGame.ShanghaiEXE.Translate("Chip.BrocraLink2Desc");
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
        battle.attacks.Add(this.Paralyze(new Brocla(this.sound, battle, point.X, point.Y, character.union, this.Power(character), Panel.PANEL._burner, true, this.element)));
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
        dg.DrawImage(dg, "chipgraphic14", this._rect, true, p, Color.White);
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

