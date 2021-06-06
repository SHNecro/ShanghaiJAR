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
    internal class ChainGun1 : ChipBase
  {
    private Point[] target = new Point[3];
    private const int shotend = 10;
    private const int shotinterval = 4;
    private bool rockon;
    private int shot;

    public ChainGun1(IAudioEngine s)
      : base(s)
    {
      this.number = 94;
      this.name = NSGame.ShanghaiEXE.Translate("Chip.ChainGun1Name");
      this.element = ChipBase.ELEMENT.normal;
      this.power = 40;
      this.regsize = 27;
      this.reality = 1;
      this.subpower = 0;
      this._break = false;
      this.shadow = false;
      this.powerprint = true;
      this.code[0] = ChipFolder.CODE.C;
      this.code[1] = ChipFolder.CODE.I;
      this.code[2] = ChipFolder.CODE.N;
      this.code[3] = ChipFolder.CODE.asterisk;
      var information = NSGame.ShanghaiEXE.Translate("Chip.ChainGun1Desc");
      this.information[0] = information[0];
      this.information[1] = information[1];
      this.information[2] = information[2];
      this.Init();
    }

    public override void Action(CharacterBase character, SceneBattle battle)
    {
      if (!this.rockon)
      {
        int index = 0;
        while (index < 3)
        {
          bool flag = true;
          foreach (CharacterBase characterBase in battle.AllChara())
          {
            if (characterBase.union == character.UnionEnemy)
            {
              flag = false;
              this.target[index] = characterBase.position;
              ++index;
              if (index >= 3)
                break;
            }
          }
          if (flag)
            break;
        }
        this.rockon = true;
      }
      character.animationpoint = ChainGun1.Animation(character.waittime);
      if (character.waittime == 24)
        base.Action(character, battle);
      if (character.waittime % 8 == 0)
        this.sound.PlaySE(SoundEffect.vulcan);
      if (character.waittime % 8 != 4)
        return;
      int num = this.power + this.pluspower;
      Point point = this.target[this.shot];
      BombAttack bombAttack = new BombAttack(this.sound, battle, point.X, point.Y, character.union, this.Power(character), 1, ChipBase.ELEMENT.normal);
      if (this.shot < 2)
        bombAttack.invincibility = false;
      battle.attacks.Add(this.Paralyze(bombAttack));
      battle.effects.Add(new GunHit(this.sound, battle, point.X, point.Y, character.union));
      battle.effects.Add(new BulletShells(this.sound, battle, character.position, character.positionDirect.X + 4 * character.UnionRebirth, character.positionDirect.Y, 26, character.union, 20 + this.Random.Next(20), 2, 0));
      ++this.shot;
    }

    public static Point Animation(int waittime)
    {
      int[] interval = new int[12];
      for (int index = 0; index < 12; ++index)
        interval[index] = 4 * index;
      int[] xpoint = new int[14]
      {
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6,
        5,
        6
      };
      int y = 0;
      return CharacterAnimation.Return(interval, xpoint, y, waittime);
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
        this._rect = new Rectangle(728, 0, 56, 48);
        dg.DrawImage(dg, "chipgraphic4", this._rect, true, p, Color.White);
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
      this._rect = new Rectangle(8 * character.Wide, 2 * character.Height, character.Wide, character.Height);
      this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
      if (character.animationpoint.X == 6)
      {
        this._rect.X += character.Wide;
        this._position.X -= 2 * this.UnionRebirth(character.union);
      }
      dg.DrawImage(dg, "weapons", this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
    }
  }
}

