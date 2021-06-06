using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using NSEnemy;

namespace NSNet
{
    public class ShanghaiBase : Player
    {
        public Virus[] viruss = new Virus[3];
        public int[] enemyName = new int[10];
        public string nextStyleP;
        public Player.STYLE nextStyle;
        public ChipBase.ELEMENT nextelEment;
        internal ShanghaiDS parentChara;

        public override int Hp
        {
            get
            {
                return base.Hp;
            }
            set
            {
            }
        }

        internal ShanghaiBase(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          SceneMain main,
          byte bp,
          byte br,
          byte bc,
          MindWindow m,
          SaveData save,
          ShanghaiDS parentChara)
          : base(s, p, pX, pY, main, bp, br, bc, m, save)
        {
            this.union = Panel.COLOR.blue;
            this.rebirth = true;
            this.number = -2;
            this.parentChara = parentChara;
        }

        public ShanghaiBase(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          SceneMain main,
          byte bp,
          byte br,
          byte bc,
          MindWindow m,
          SaveData save)
          : base(s, p, pX, pY, main, bp, br, bc, m, save)
        {
            this.union = Panel.COLOR.blue;
            this.rebirth = true;
            this.number = -2;
        }

        public void ParamSet(
          int HP,
          bool[] addonSkill,
          string stylename,
          int[] virusNo,
          int[] virusBug,
          int[] virusFreeze,
          int[] virusError)
        {
            this.number = 1;
            this.HPset(HP);
            this.union = Panel.COLOR.blue;
            this.rebirth = true;
            this.picturename = stylename;
            this.addonSkill = addonSkill;
            for (int index = 0; index < this.viruss.Length; ++index)
            {
                if (virusNo[index] >= 0)
                {
                    this.viruss[index] = new Virus
                    {
                        type = virusNo[index],
                        eatBug = virusBug[index],
                        eatFreeze = virusFreeze[index],
                        eatError = virusError[index]
                    };
                }
            }
            this.alfha = 0;
            this.printhp = true;
            this.number = -2;
            this.positionold = new Point(5, 1);
            this.AddOn();
        }

        public void Nameprint(IRenderer dg)
        {
            int num1 = 0;
            if (this.parent == null || !this.parent.namePrint)
                return;
            List<AllBase.NAME> nameList = new List<AllBase.NAME>();
            for (int index = 0; index < this.enemyName.Length; ++index)
                nameList.Add((AllBase.NAME)this.enemyName[index]);
            for (int index = nameList.Count - 1; index >= 0 && nameList[index] == AllBase.NAME.no; --index)
                nameList[index] = AllBase.NAME.none;
            nameList.RemoveAll(s => s == AllBase.NAME.none);
            AllBase.NAME[] array = nameList.ToArray();
            int length = array.Length;
            int num2 = 0;
            int num3 = 240 - array.Length * 8;
            this.color = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            this._rect = new Rectangle(320, 104, 8, 16);
            this._position = new Vector2(num3 - 8 + num2, num1 * 16);
            dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, this.color);
            for (int index = 0; index < length; ++index)
            {
                this._position = new Vector2(num3 + 8 * index + num2, num1 * 16);
                this._rect = new Rectangle(328, 104, 8, 16);
                dg.DrawImage(dg, "battleobjects", this._rect, true, this._position, this.color);
                this._rect = new Rectangle((int)array[index] * 8, 88, 8, 16);
                dg.DrawImage(dg, "font", this._rect, true, this._position, this.color);
            }
        }
    }
}
