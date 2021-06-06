using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using Common.Vectors;
using System.Drawing;

namespace NSChip
{
    internal class ScissorManV1 : ChipBase
  {
    public int waittime;
    protected Point[] posis;
    protected int time;
    protected Vector2 endposition;
    protected float movex;
    protected float movey;
    protected float plusy;
    protected float speedy;
    protected float plusing;
    private const int startspeed = 4;
    protected int flyflame;
    protected const int start = 44;
    protected const int speed = 2;
    protected bool colory;
    protected int attackMode;
    protected Point target;
    protected Point animationpoint;

    public ScissorManV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 233;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ScissorManV1Name");
      this.element = ChipBase.ELEMENT.leaf;
      this.power = 170;
      this.subpower = 0;
      this.regsize = 34;
      this.reality = 3;
      this._break = true;
      this.obje = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.S;
      this.code[1] = ChipFolder.CODE.B;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      this.animationpoint.X = -1;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ScissorManV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
      this.colory = true;
    }

    protected Point AnimeMeteorSickle1(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[3]
      {
        1,
        1,
        10
      }, new int[3]{ 20, 21, 22 }, 0, waittime);
    }

    protected Point AnimeMeteorSickle2(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[2]
      {
        1,
        1
      }, new int[2]{ 23, 24 }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
        return;
      this.FlameControl(3);
      switch (this.attackMode)
      {
        case 0:
          if (this.moveflame)
          {
            ++this.waittime;
            this.animationpoint = this.AnimeMeteorSickle1(this.waittime);
            switch (this.waittime)
            {
              case 1:
                character.animationpoint.X = -1;
                this.sound.PlaySE(SoundEffect.warp);
                this.posis = new Point[2];
                this.posis[0] = new Point(character.position.X + 2 * this.UnionRebirth(character.union), character.position.Y);
                this.posis[1].X = character.union == Panel.COLOR.blue ? 0 : 5;
                this.posis[1].Y = this.Random.Next(3);
                battle.attacks.Add(new Dummy(this.sound, battle, this.posis[0].X, this.posis[0].Y, character.union, new Point(), 30, true));
                break;
              case 3:
                this.waittime = 0;
                ++this.attackMode;
                this.Throw(character);
                break;
            }
            break;
          }
          break;
        case 1:
          if (this.moveflame)
          {
            this.animationpoint = this.AnimeMeteorSickle2(this.waittime % 2 + 1);
            ++this.waittime;
          }
          if (this.flyflame == this.time / 3)
          {
            this.sound.PlaySE(SoundEffect.knife);
            battle.attacks.Add(this.Paralyze(new DelayScissor(this.sound, battle, this.posis[0].X, this.posis[0].Y, character.union, this.Power(character), 16, this.element, new Vector2((float)(character.position.X * 40.0 + 4.0 + (character.union == Panel.COLOR.red ? 40.0 : 0.0)), (float)(character.position.Y * 24.0 + 48.0) + this.plusy), !this.colory, 100)));
            battle.attacks.Add(new Dummy(this.sound, battle, this.posis[1].X, this.posis[1].Y, character.union, new Point(), 30, true));
          }
          if (this.flyflame == this.time / 3 * 2)
          {
            this.sound.PlaySE(SoundEffect.knife);
            battle.attacks.Add(this.Paralyze(new DelayScissor(this.sound, battle, this.posis[1].X, this.posis[1].Y, character.union, this.Power(character), 16, this.element, new Vector2((float)(character.position.X * 40.0 + 4.0 + (character.union == Panel.COLOR.red ? 40.0 : 0.0)), (float)(character.position.Y * 24.0 + 48.0) + this.plusy), !this.colory, 100)));
          }
          if (this.flyflame == this.time)
          {
            this.animationpoint.X = 0;
            this.waittime = 0;
            ++this.attackMode;
          }
          else
          {
            this.plusy -= this.speedy;
            this.speedy -= this.plusing;
          }
          ++this.flyflame;
          break;
        case 2:
          ++this.waittime;
          if (this.waittime > 60 && this.BlackOutEnd(character, battle))
          {
            base.Action(character, battle);
            break;
          }
          break;
      }
    }

    private void Throw(CharacterBase character)
    {
      this.animationpoint.X = 9;
      this.waittime = 0;
      this.time = 60;
      this.flyflame = 0;
      this.endposition = new Vector2((float) (character.position.X * 40.0 + 4.0 + (character.union == Panel.COLOR.red ? 40.0 : 0.0)), (float) (character.position.Y * 24.0 + 48.0));
      this.movex = 0.0f;
      this.movey = 0.0f;
      this.plusy = 0.0f;
      this.speedy = 4f;
      this.plusing = this.speedy / (this.time / 2);
      this.flyflame = 0;
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
        this._rect = new Rectangle(504, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic19", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(480, 64 + num4 * 96, 16, 16);
        dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
      }
      base.IconRender(dg, p, select, custom, c, noicon);
    }

    public override void Render(IRenderer dg, CharacterBase character)
    {
      if (character.animationpoint.X == -1 && this.waittime < 44)
      {
        this._rect = new Rectangle(160 * this.animationpoint.X, 0, 160, 144);
        this._position = new Vector2((float) (character.position.X * 40.0 + 4.0 + (character.union == Panel.COLOR.red ? 40.0 : 0.0)), (float) (character.position.Y * 24.0 + 48.0) + this.plusy);
        dg.DrawImage(dg, "ScissorMan", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

