using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class PonpocoJizou1 : ChipBase
  {
    private const int speed = 2;

    public PonpocoJizou1(IAudioEngine s)
      : base(s)
    {
      this.number = 109;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.PonpocoJizou1Name");
      this.element = ChipBase.ELEMENT.eleki;
      this.power = 100;
      this.subpower = 0;
      this.regsize = 47;
      this.reality = 1;
      this.obje = true;
      this._break = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.G;
      this.code[1] = ChipFolder.CODE.H;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.PonpocoJizou1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      Point point = new Point(character.position.X + this.UnionRebirth(character.union), character.position.Y);
      this.sound.PlaySE(SoundEffect.enterenemy);
      battle.effects.Add(new MoveEnemy(this.sound, battle, point.X, point.Y));
      if (character.InAreaCheck(point) && character.NoObject(point) && !battle.panel[point.X, point.Y].Hole)
        battle.objects.Add(new Jizou(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, 1, this.Power(character)));
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
        this._rect = new Rectangle(0, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic16", this._rect, true, p, Color.White);
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

    public override void Render(IRenderer dg, CharacterBase player)
    {
    }
  }
}

