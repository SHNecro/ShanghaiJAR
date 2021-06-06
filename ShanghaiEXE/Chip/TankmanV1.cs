using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;

namespace NSChip
{
    internal class TankmanV1 : ChipBase
  {
    private Point[,] target = new Point[2, 9];
    private const int aspeed = 3;
    private int waittime;
    private int spin;
    private int action;
    protected bool command;
    private const int start = 44;
    private const int speed = 2;
    private Point animePoint;

    public TankmanV1(IAudioEngine s)
      : base(s)
    {
      this.navi = true;
      this.number = 197;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.TankmanV1Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 100;
      this.subpower = 20;
      this.regsize = 28;
      this.reality = 3;
      this._break = true;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.T;
      this.code[1] = ChipFolder.CODE.R;
      this.code[2] = ChipFolder.CODE.asterisk;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.TankmanV1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    protected Point Animation(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[10]
      {
        1,
        4,
        4,
        4,
        4,
        12,
        4,
        4,
        4,
        4
      }, new int[10]{ -1, 9, 8, 7, 3, 0, 3, 7, 13, 14 }, 0, waittime);
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.BlackOut(character, battle, this.name, this.Power(character).ToString()))
        return;
      if (character.waittime == 1)
      {
        character.animationpoint.X = -1;
        this.sound.PlaySE(SoundEffect.warp);
      }
      if (character is Player)
      {
        Player player = (Player) character;
        if (this.action <= 1 && !this.command)
        {
          this.CommandInput("B", player);
          if (this.CommandCheck("BBB"))
          {
            this.command = true;
            this.sound.PlaySE(SoundEffect.CommandSuccess);
            this.subpower = 10;
          }
        }
      }
      switch (this.action)
      {
        case 0:
          this.animePoint = this.AnimeGatlingReady(this.waittime);
          if (this.waittime >= 21)
          {
            int x = Eriabash.SteelX(character, character.parent);
            int y = 0;
            bool flag = false;
            for (int index = 0; index < this.target.GetLength(1); ++index)
            {
              this.target[0, index] = new Point(x, y);
              if (flag)
              {
                if (y <= 0)
                {
                  if (x < 5 && x > 0)
                    x += this.UnionRebirth(character.union);
                  flag = !flag;
                }
                else
                  --y;
              }
              else if (y >= 2)
              {
                if (x < 5 && x > 0)
                  x += this.UnionRebirth(character.union);
                flag = !flag;
              }
              else
                ++y;
            }
            this.sound.PlaySE(SoundEffect.machineRunning);
            ++this.action;
            this.waittime = 0;
            break;
          }
          break;
        case 1:
          this.animePoint = this.AnimeGatling1(this.waittime);
          if (this.waittime == 3)
          {
            ++this.spin;
            this.waittime = 0;
            if (this.spin >= 2)
            {
              ++this.action;
              this.spin = 0;
              if (this.command)
              {
                int num = 0;
                Point point = new Point(-1, -1);
                foreach (CharacterBase characterBase in battle.AllChara())
                {
                  if (!(characterBase is DammyEnemy) && characterBase.union == character.UnionEnemy && num < characterBase.Hp)
                  {
                    num = characterBase.Hp;
                    point = characterBase.position;
                  }
                }
                if (point.X >= 0 && point.Y >= 0)
                {
                  for (int index = 0; index < this.target.GetLength(1); ++index)
                    this.target[0, index] = point;
                }
              }
            }
            break;
          }
          break;
        case 2:
          this.animePoint = this.AnimeGatling2(this.waittime);
          if (this.waittime == 3)
          {
            this.sound.PlaySE(SoundEffect.vulcan);
            Point point = this.target[0, this.spin];
            battle.effects.Add(new GunHit(this.sound, battle, point.X, point.Y, character.union));
            BombAttack bombAttack = new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.subpower, 1, this.element);
            bombAttack.invincibility = false;
            battle.attacks.Add(this.Paralyze(bombAttack));
            List<EffectBase> effects = battle.effects;
            IAudioEngine sound = this.sound;
            SceneBattle p = battle;
            Point position = character.position;
            double x = character.positionDirect.X;
            this.UnionRebirth(character.union);
            double num1 = x - 0.0;
            double num2 = character.positionDirect.Y + 8.0;
            int union = (int) character.union;
            int time = 40 + this.Random.Next(20);
            BulletShells bulletShells = new BulletShells(sound, p, position, (float) num1, (float) num2, 32, (Panel.COLOR) union, time, 2, 0);
            effects.Add(bulletShells);
            battle.effects.Add(new BulletShells(this.sound, battle, character.position, character.positionDirect.X - 16 * this.UnionRebirth(character.union), character.positionDirect.Y + 8f, 32, character.union, 40 + this.Random.Next(20), 2, 0));
            break;
          }
          if (this.waittime == 9)
          {
            ++this.spin;
            this.waittime = 0;
            if (this.spin >= 9)
              ++this.action;
            break;
          }
          break;
        case 3:
          this.animePoint = this.AnimeCanonReady(this.waittime);
          if (this.waittime >= 21)
          {
            ++this.action;
            this.waittime = 0;
            break;
          }
          break;
        case 4:
          this.animePoint = this.AnimeCanon(this.waittime);
          if (this.waittime == 3)
          {
            this.sound.PlaySE(SoundEffect.canon);
            battle.attacks.Add(this.Paralyze(new CanonBullet(this.sound, battle, character.position.X + this.UnionRebirth(character.union), character.position.Y, new Vector2(character.positionDirect.X + 32 * this.UnionRebirth(character.union), character.positionDirect.Y + 8f), character.union, this.Power(character), this.element, false)));
            battle.effects.Add(new BulletBigShells(this.sound, battle, character.position, character.positionDirect.X - 16 * this.UnionRebirth(character.union), character.positionDirect.Y - 16f, 32, character.union, 40 + this.Random.Next(20), 2, 0));
            break;
          }
          if (this.waittime >= 54 && this.BlackOutEnd(character, battle))
          {
            base.Action(character, battle);
            break;
          }
          break;
      }
      ++this.waittime;
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
        dg.DrawImage(dg, "chipgraphic12", this._rect, true, p, Color.White);
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
        this._rect = new Rectangle(88 * this.animePoint.X, 0, 88, 64);
        this._position = new Vector2((float) (character.position.X * 40.0 + 8.0 + (character.union == Panel.COLOR.red ? 24.0 : 0.0)), (float) (character.position.Y * 24.0 + 58.0));
        dg.DrawImage(dg, "tankman", this._rect, false, this._position, character.union == Panel.COLOR.red, Color.White);
      }
      else
        this.BlackOutRender(dg, character.union);
    }

    private Point AnimeCanonReady(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[7]
      {
        3,
        3,
        3,
        3,
        3,
        3,
        3
      }, new int[7]{ 2, 3, 20, 21, 22, 23, 24 }, 0, waittime);
    }

    private Point AnimeCanon(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[5]
      {
        3,
        3,
        3,
        3,
        3
      }, new int[6]{ 24, 25, 26, 27, 28, 24 }, 0, waittime);
    }

    private Point AnimeGatlingReady(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[7]
      {
        3,
        3,
        3,
        3,
        3,
        3,
        3
      }, new int[7]{ 2, 3, 4, 5, 6, 7, 8 }, 0, waittime);
    }

    private Point AnimeGatling1(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[3]
      {
        3,
        3,
        3
      }, new int[3]{ 8, 9, 10 }, 0, waittime);
    }

    private Point AnimeGatling2(int waittime)
    {
      return CharacterAnimation.ReturnKai(new int[4]
      {
        3,
        3,
        3,
        3
      }, new int[4]{ 8, 11, 12, 8 }, 0, waittime);
    }
  }
}

