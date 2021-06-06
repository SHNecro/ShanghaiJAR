using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class SeedCanon1 : ChipBase
  {
    private int posiminus = 0;
    private const int shotend = 58;
    private const int shotstart = 40;

    public SeedCanon1(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 4;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.SeedCanon1Name");
      this.element = ChipBase.ELEMENT.leaf;
      this.power = 120;
      this.regsize = 18;
      this.reality = 1;
      this.subpower = 0;
      this._break = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.N;
      this.code[2] = ChipFolder.CODE.T;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.SeedCanon1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    private Point Animation(int waittime)
    {
      return CharacterAnimation.Return(new int[4]
      {
        5,
        40,
        42,
        45
      }, new int[7]{ 4, 5, 6, 5, 2, 1, 0 }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (character.waittime < 5)
        character.animationpoint = new Point(4, 0);
      else if (character.waittime < 40)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime < 58)
      {
        character.animationpoint = new Point(6, 0);
        if (character.waittime < 22)
          character.positionDirect.X -= (character.waittime - 15) * this.UnionRebirth(character.union);
      }
      else if (character.waittime < 63)
      {
        character.animationpoint = new Point(5, 0);
        character.PositionDirectSet();
      }
      else if (character.waittime == 63)
        base.Action(character, battle);
      if (character.waittime == 43)
      {
        this.ShakeStart(10, 5);
        this.sound.PlaySE(SoundEffect.canon);
        battle.effects.Add(new BulletBigShells(this.sound, battle, character.position, character.positionDirect.X + 4 * character.UnionRebirth, character.positionDirect.Y + 16f, 26, character.union, 20 + this.Random.Next(20), 2, 0));
      }
      if (character.waittime >= 40 && character.waittime < 41)
        character.positionDirect.X -= 6 * this.UnionRebirth(character.union);
      if (character.waittime != 45)
        return;
      int num = this.power + this.pluspower;
      character.parent.attacks.Add(this.Paralyze(new BustorShot(this.sound, character.parent, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), BustorShot.SHOT.seedcanon, this.element, false, 0)));
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
        this._rect = new Rectangle(616, 0, 56, 48);
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
      if (character.waittime < 5)
        return;
      this._rect = new Rectangle(960, 720, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X = 360;
      else if (character.waittime < 11)
        this._rect.X = 480;
      else if (character.waittime < 12)
        this._rect.X = 600;
      else if (character.waittime < 13)
        this._rect.X = 720;
      else if (character.waittime < 14)
        this._rect.X = 840;
      else if (character.waittime < 15)
        this._rect.X = 960;
      else if (character.waittime > 40 && character.waittime < 42)
        this._rect.X = 1080;
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      if (character.waittime >= 40)
      {
        this._position = character.positionDirect;
        this._position.X += 48 * this.UnionRebirth(character.union);
        this._position.Y += 14f;
        this._rect = new Rectangle((character.waittime - 40) / 3 * 64, 32, 64, 64);
        dg.DrawImage(dg, "shot", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
    }
  }
}

