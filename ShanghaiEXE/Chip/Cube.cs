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
    internal class Cube : ChipBase
  {
    private const int speed = 2;

    public Cube(IAudioEngine s)
      : base(s)
    {
      this.number = 142;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.CubeName");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 0;
      this.subpower = 0;
      this.regsize = 11;
      this.reality = 1;
      this.obje = true;
      this._break = false;
      this.shadow = false;
      this.powerprint = false;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.S;
      this.code[2] = ChipFolder.CODE.T;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.CubeDesc");
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
      {
        ObjectBase objectBase = new NSObject.Cube(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union);
        if (character is Player && ((Player) character).addonSkill[67])
          objectBase.HPset(objectBase.Hp * 2);
        battle.objects.Add(objectBase);
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
        this._rect = new Rectangle(0, 0, 56, 48);
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

    public override void Render(IRenderer dg, CharacterBase player)
    {
    }
  }
}

