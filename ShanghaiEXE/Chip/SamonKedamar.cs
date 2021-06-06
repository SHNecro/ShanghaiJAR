using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class SamonKedamar : ChipBase
  {
    private const int start = 1;
    private const int speed = 2;

    public SamonKedamar(IAudioEngine s)
      : base(s)
    {
      this.number = 201;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SamonKedamarName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 0;
      this._break = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.K;
      this.code[1] = ChipFolder.CODE.M;
      this.code[2] = ChipFolder.CODE.S;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SamonKedamarDesc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
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
        if (!character.Canmove(character.positionold))
          return;
        this.sound.PlaySE(SoundEffect.enterenemy);
        battle.enemys.Add(new Kedamar(this.sound, battle, character.positionold.X, character.positionold.Y, (byte)battle.enemys.Count, Panel.COLOR.red, 2));
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
        this._rect = new Rectangle(56, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic11", this._rect, true, p, Color.White);
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
        int num = 0;
        if (select)
          num = 1;
        this._rect = new Rectangle(192, num * 16, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase player)
    {
      this._rect = new Rectangle(player.animationpoint.X * 68, 272, player.Wide, player.Height);
      this._position = new Vector2(player.positionDirect.X + Shake.X, player.positionDirect.Y + Shake.Y);
      dg.DrawImage(dg, player.picturename, this._rect, false, this._position, Color.White);
    }
  }
}

