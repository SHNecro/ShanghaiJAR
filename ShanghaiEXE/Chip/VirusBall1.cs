using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSGame;
using NSNet;
using Common.Vectors;
using System.Drawing;
using System.Linq;
using System;

namespace NSChip
{
    internal class VirusBall1 : ChipBase
    {
        protected int id = 0;
        private const int start = 3;
        private const int speed = 3;

        public VirusBall1(IAudioEngine s, bool set)
          : base(s)
        {
            this.number = 310;
            this.name = ShanghaiEXE.Translate("Chip.VrsBall1Name");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 10;
            this.subpower = 0;
            this.regsize = 35;
            this.reality = 5;
            this._break = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.asterisk;
            this.code[1] = ChipFolder.CODE.asterisk;
            this.code[2] = ChipFolder.CODE.asterisk;
            this.code[3] = ChipFolder.CODE.asterisk;
            var information = ShanghaiEXE.Translate("Chip.VrsBall1Desc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];
            if (SaveData.HAVEVirus[this.id] == null || !set)
            {
                this.Init();
            }
            else
            {
                Virus haveViru = SaveData.HAVEVirus[this.id];
                this.element = ChipBase.ELEMENT.normal;
                this.power = 0;
                this.subpower = 0;
                this.regsize = 35;
                this.reality = 5;
                this._break = false;
                this.powerprint = false;
                this.shadow = false;
                this.code[0] = haveViru.Code;
                this.code[1] = this.code[0];
                this.code[2] = this.code[0];
                this.code[3] = this.code[0];
                information = ShanghaiEXE.Translate("Chip.VrsBall1FilledDesc");
                this.information[0] = information[0];
                this.information[1] = information[1];
                this.information[2] = information[2];
                this.Init();
                this.name = haveViru.Name;
            }
        }

        public override void Action(CharacterBase character, SceneBattle battle)
        {
            if (SaveData.HAVEVirus[this.id] == null)
            {
                if (character.waittime == 3)
                    this.sound.PlaySE(SoundEffect.throwbomb);
                character.animationpoint = CharacterAnimation.BombAnimation(character.waittime);
                if (character.waittime == 6)
                {
                    int num = this.power + this.pluspower;
                    int t = 40;
                    Point end = new Point(character.position.X + 3 * this.UnionRebirth(character.union), character.position.Y);
                    battle.attacks.Add(this.Paralyze(new VirusBall(this.sound, character.parent, character.position.X, character.position.Y, character.union, this.Power(character), 1, new Vector2(character.positionDirect.X, character.positionDirect.Y - 16f), end, t, this.id)));
                }
                if (character.waittime != 25)
                    return;
                base.Action(character, battle);
            }
            else if (character.waittime <= 3)
                character.animationpoint = new Point(0, 1);
            else if (character.waittime <= 12)
                character.animationpoint = new Point((character.waittime - 3) / 3, 1);
            else if (character.waittime < 24)
                character.animationpoint = new Point(3, 1);
            else if (character.waittime == 24)
            {
                if (character.Canmove(character.positionold))
                {
                    this.sound.PlaySE(SoundEffect.enterenemy);
                    Virus haveViru;
                    EnemyBase e;
                    haveViru = SaveData.HAVEVirus[this.id];

                    var newVirus = new Virus
                    {
                        type = haveViru.type,
                        eatBug = haveViru.eatBug,
                        eatError = haveViru.eatError,
                        eatFreeze = haveViru.eatFreeze,
                    };
                    if ((character as Player)?.addonSkill[73] == true)
                    {
                        var enemies = battle.AllChara().Where(c => character.UnionEnemy == c.union);
                        var maxEnemyRank = enemies.Max(c => (c as EnemyBase)?.version ?? 0);

                        if (enemies.Any(c => c is NaviBase))
                        {
                            maxEnemyRank++;
                        }

                        var rankUpgrades = Math.Max(0, maxEnemyRank - haveViru.rank - (3 - haveViru.rank));
                        newVirus.eatBug += 20 * rankUpgrades;
                        newVirus.ReturnVirus(newVirus.type);
                    }
                    e = new EnemyBase(this.sound, battle, character.positionold.X, character.positionold.Y, 0, character.union, newVirus.rank);
                    EnemyBase enemyBase = EnemyBase.EnemyMake(newVirus.type, e, false);
                    enemyBase.number = battle.enemys.Count;
                    if (this.randomSeed != null)
                        enemyBase.randomSeed = this.randomSeed;
                    enemyBase.HPset(newVirus.HP);
                    enemyBase.power = newVirus.Power;
                    if (enemyBase.Canmove(enemyBase.position))
                    {
                        battle.enemys.Add(enemyBase);
                        enemyBase.Init();
                        enemyBase.InitAfter();
                    }
                    else
                        battle.effects.Add(new MoveEnemy(this.sound, battle, character.positionold.X, character.positionold.Y));
                }
            }
            else if (character.waittime >= 48)
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
                if (this.sound != null)
                {
                    if (SaveData.HAVEVirus[this.id] == null)
                    {
                        this._rect = new Rectangle(56 * (13 + this.id), 0, 56, 48);
                        dg.DrawImage(dg, "chipgraphic9", this._rect, true, p, Color.White);
                    }
                    else
                    {
                        this._rect = new Rectangle(56 * (SaveData.HAVEVirus[this.id].type - 1), 0, 56, 48);
                        dg.DrawImage(dg, "chipgraphic11", this._rect, true, p, Color.White);
                    }
                }
                else
                {
                    this._rect = new Rectangle(56 * (13 + this.id), 0, 56, 48);
                    dg.DrawImage(dg, "chipgraphic9", this._rect, true, p, Color.White);
                }
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
                this._rect = new Rectangle(32, 80 + num4 * 96, 16, 16);
                dg.DrawImage(dg, "chipicon", this._rect, true, p, Color.White);
            }
            base.IconRender(dg, p, select, custom, c, noicon);
        }

        public override void Render(IRenderer dg, CharacterBase character)
        {
            if (SaveData.HAVEVirus[this.id] == null)
            {
                if (character.waittime >= 6)
                    return;
                this._rect = new Rectangle(0, 0, 16, 16);
                Point point = new Point();
                if (character.waittime <= 3)
                {
                    point.X = -22 * this.UnionRebirth(character.union);
                    point.Y = 22;
                }
                else
                {
                    point.X = -10 * this.UnionRebirth(character.union);
                    point.Y = 4;
                }
                this._position = new Vector2(character.positionDirect.X + Shake.X + point.X, character.positionDirect.Y + Shake.Y + point.Y);
                dg.DrawImage(dg, "bombs", this._rect, false, this._position, Color.White);
            }
            else
            {
                this._rect = new Rectangle(character.animationpoint.X * character.Wide, 4 * character.Height, character.Wide, character.Height);
                this._position = new Vector2(character.positionDirect.X + Shake.X, character.positionDirect.Y + Shake.Y);
                dg.DrawImage(dg, character.picturename, this._rect, false, this._position, character.union == Panel.COLOR.blue, Color.White);
            }
        }
    }
}
