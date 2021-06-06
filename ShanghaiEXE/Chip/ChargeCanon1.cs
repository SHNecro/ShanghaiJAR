using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ChargeCanon1 : ChipBase
  {
    private const int shotend = 128;
    private bool anime;
    private bool soundPlay;

    public ChargeCanon1(IAudioEngine s)
      : base(s)
    {
      this.rockOnPoint = new Point(-3, 0);
      this.number = 7;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ChargeCanon1Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 200;
      this.regsize = 24;
      this.reality = 2;
      this.subpower = 0;
      this._break = true;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.F;
      this.code[1] = ChipFolder.CODE.M;
      this.code[2] = ChipFolder.CODE.Y;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ChargeCanon1Desc");
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
        15,
        17,
        20
      }, new int[7]{ 4, 5, 6, 5, 2, 1, 0 }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.soundPlay && this.chargeFlag)
      {
        this.soundPlay = true;
        this.sound.PlaySE(SoundEffect.CommandSuccess);
      }
      if (character.waittime > 15 && character.waittime % 2 == 0)
        this.anime = !this.anime;
      if (character.waittime == 15)
      {
        this.sound.PlaySE(SoundEffect.machineRunning);
        this.sound.PlaySE(SoundEffect.charge);
      }
      if (character.waittime < 5)
        character.animationpoint = new Point(4, 0);
      else if (character.waittime < 15)
        character.animationpoint = new Point(5, 0);
      else if (character.waittime < 128)
      {
        character.animationpoint = new Point(6, 0);
        if (character.waittime < 17)
          character.positionDirect.X -= (character.waittime - 15) * this.UnionRebirth(character.union);
      }
      else if (character.waittime < 133)
      {
        character.animationpoint = new Point(5, 0);
        character.PositionDirectSet();
      }
      else if (character.waittime == 133)
        base.Action(character, battle);
      if (character.waittime == 108)
      {
        this.ShakeStart(10, 30);
        this.sound.PlaySE(SoundEffect.bombbig);
      }
      if (character.waittime != 110)
        return;
      int num = this.power + this.pluspower;
      if (this.chargeFlag)
      {
        EYEBall eyeBall1 = new EYEBall(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character) / 2, 2, character.positionDirect, ChipBase.ELEMENT.normal, 20);
        eyeBall1.positionDirect.X += 40 * this.UnionRebirth(character.union);
        eyeBall1.positionDirect.Y += 16f;
        character.parent.attacks.Add(this.Paralyze(eyeBall1));
        EYEBall eyeBall2 = new EYEBall(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y - 1, character.union, this.Power(character) / 2, 2, character.positionDirect, ChipBase.ELEMENT.normal, 20);
        eyeBall2.positionDirect.X += 40 * this.UnionRebirth(character.union);
        eyeBall2.positionDirect.Y += -8f;
        character.parent.attacks.Add(this.Paralyze(eyeBall2));
        EYEBall eyeBall3 = new EYEBall(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y + 1, character.union, this.Power(character) / 2, 2, character.positionDirect, ChipBase.ELEMENT.normal, 20);
        eyeBall3.positionDirect.X += 40 * this.UnionRebirth(character.union);
        eyeBall3.positionDirect.Y += 40f;
        character.parent.attacks.Add(this.Paralyze(eyeBall3));
      }
      else
      {
        EYEBall eyeBall = new EYEBall(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, character.positionDirect, ChipBase.ELEMENT.normal, 20);
        eyeBall.positionDirect.X += 40 * this.UnionRebirth(character.union);
        eyeBall.positionDirect.Y += 16f;
        character.parent.attacks.Add(this.Paralyze(eyeBall));
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
        this._rect = new Rectangle(672, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic21", this._rect, true, p, Color.White);
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
      this._rect = new Rectangle(1200, 600, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.waittime < 10)
        this._rect.X -= 120;
      else if (character.waittime >= 15 && character.waittime < 108)
      {
        if (this.anime)
          this._rect.X += 120;
      }
      else if (character.waittime >= 108 && character.waittime < 128)
      {
        this._rect.X += 120;
        this._position.X -= 2 * this.UnionRebirth(character.union);
      }
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      if (character.waittime >= 108 && character.waittime < 128)
      {
        this._position = character.positionDirect;
        this._position.X += 32 * this.UnionRebirth(character.union);
        this._position.Y += 14f;
        this._rect = new Rectangle((character.waittime - 10) / 3 * 64, 32, 64, 64);
        dg.DrawImage(dg, "shot", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
      }
    }
  }
}

