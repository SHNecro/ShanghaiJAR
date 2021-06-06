using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class BeatleManV1 : ChipBase
  {
    protected int endflame = -1;
    private const int speed = 2;
    protected Point animationpoint;
    protected bool bright;
    protected int buganime;
    protected BugBall a;

    public BeatleManV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 212;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.BeatleManV1Name");
      this.element = ChipBase.ELEMENT.earth;
      this.power = 90;
      this.subpower = 20;
      this.regsize = 41;
      this.reality = 3;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.B;
      this.code[1] = ChipFolder.CODE.W;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.BeatleManV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
        return;
      if (character.waittime % 2 == 0)
        this.bright = !this.bright;
      if (character.waittime % 3 == 0)
      {
        ++this.buganime;
        if (this.buganime > 3)
          this.buganime = 0;
        ++this.frame;
        switch (this.frame)
        {
          case 1:
            character.animationpoint.X = -1;
            this.sound.PlaySE(SoundEffect.warp);
            this.animationpoint.X = 10;
            break;
          case 3:
            this.animationpoint.X = 9;
            break;
          case 5:
            this.animationpoint.X = 0;
            break;
          case 10:
            this.animationpoint.X = 9;
            break;
          case 12:
            this.animationpoint.X = 12;
            break;
          case 15:
            this.sound.PlaySE(SoundEffect.rockopen);
            battle.effects.Add(new ScreenFlash(this.sound, battle));
            break;
          case 16:
            Vector2 pdEnd = new Vector2((float) (character.position.X * 40.0 + 32.0), (float) (character.position.Y * 24.0 + 16.0));
            foreach (AttackBase attack in battle.attacks)
            {
              if (attack.hitting)
              {
                attack.flag = false;
                this.pluspower += this.subpower;
                battle.effects.Add(new PowerBall(this.sound, battle, attack.positionDirect, pdEnd, attack.position, 15));
              }
            }
            using (List<ObjectBase>.Enumerator enumerator = battle.objects.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                ObjectBase current = enumerator.Current;
                if (!current.nohit)
                {
                  current.flag = false;
                  this.pluspower += this.subpower;
                  battle.effects.Add(new PowerBall(this.sound, battle, current.positionDirect, pdEnd, current.position, 15));
                }
              }
              break;
            }
case 25:
            this.animationpoint.X = 5;
            break;
          case 27:
            this.sound.PlaySE(SoundEffect.canon);
            this.animationpoint.X = 6;
            this.a = new BugBall(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, character.union, this.Power(character), 2, this.element, 8);
            this.a.BadStatusSet(CharacterBase.BADSTATUS.heavy, 300);
            character.parent.attacks.Add(this.Paralyze(a));
            break;
        }
        if (this.frame > 27 && this.a != null && !this.a.flag)
          this.endflame = this.frame + 10;
        if (this.endflame > 0 && this.endflame >= this.frame)
        {
          if (this.endflame == this.frame)
          {
            this.animationpoint.X = -1;
            battle.effects.Add(new MoveEnemy(this.sound, battle, character.position.X, character.position.Y));
          }
          if (this.BlackOutEnd(character, battle))
            base.Action(character, battle);
        }
      }
      if (this.animationpoint.X == 3 || this.animationpoint.X == 4)
        this.animationpoint.X = this.bright ? 4 : 3;
      if (this.animationpoint.X == 7 || this.animationpoint.X == 8)
        this.animationpoint.X = this.bright ? 8 : 7;
      if (this.animationpoint.X == 11 || this.animationpoint.X == 12)
        this.animationpoint.X = this.bright ? 12 : 11;
      if (this.frame > 140 && this.BlackOutEnd(character, battle))
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
        this._rect = new Rectangle(168, 0, 56, 48);
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
      if (character.animationpoint.X == -1)
      {
        this._rect = new Rectangle(112 * this.animationpoint.X, 0, 112, 112);
        this._position = new Vector2((float) (character.position.X * 40.0 + 32.0), (float) (character.position.Y * 24.0 + 48.0));
        dg.DrawImage(dg, "beetleman", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
        if (this.animationpoint.X != 11 && this.animationpoint.X != 12)
          return;
        this._rect = new Rectangle(32 * this.buganime, 1384, 32, 32);
        this._position = new Vector2((float) (character.position.X * 40.0 + 32.0), (float) (character.position.Y * 24.0 + 16.0));
        dg.DrawImage(dg, "shot", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }
  }
}

