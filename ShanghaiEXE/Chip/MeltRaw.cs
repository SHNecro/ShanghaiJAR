using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class MeltRaw : ChipBase
  {
    private const int start = 1;
    private const int speed = 2;

    public MeltRaw(IAudioEngine s)
      : base(s)
    {
      this.number = 154;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.MeltRawName");
      this.element = ChipBase.ELEMENT.heat;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 21;
      this.reality = 2;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.G;
      this.code[1] = ChipFolder.CODE.M;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.T;
      var information = NSGame.ShanghaiEXE.Translate("Chip.MeltRawDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime <= 1)
        character.animationpoint = new Point(0, 1);
      else if (character.waittime <= 7)
        character.animationpoint = new Point((character.waittime - 1) / 2, 1);
      else if (character.waittime < 15)
        character.animationpoint = new Point(3, 1);
      else if (character.waittime == 15)
      {
        BombAttack bombAttack = new BombAttack(this.sound, character.parent, character.union == Panel.COLOR.red ? 0 : 5, 0, character.union, 0, 1, 1, new Point(5, 3), this.element);
        bombAttack.badstatus[(int) this.element] = true;
        bombAttack.badstatustime[(int) this.element] = 600;
        bombAttack.bright = false;
        bombAttack.effect = true;
        bombAttack.panelChange = false;
        bombAttack.canCounter = false;
        bombAttack.invincibility = false;
        bombAttack.knock = false;
        character.parent.attacks.Add(bombAttack);
      }
      else
      {
        if (character.waittime < 31)
          return;
        base.Action(character, battle);
      }
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
        this._rect = new Rectangle(280, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic7", this._rect, true, p, Color.White);
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

    public override void Render(IRenderer dg, CharacterBase character)
    {
      this._rect = new Rectangle(character.animationpoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

