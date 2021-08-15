using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System;

namespace NSAttack
{
    internal class MasterBeam : AttackBase
    {
        private bool start;
        private bool end;
        private int damage;
        private Tuple<CharacterBase, int>[] hitter;

        public MasterBeam(
          IAudioEngine so,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int s,
          bool pl)
          : base(so, p, pX, pY, u, po, ChipBase.ELEMENT.eleki)
        {
            if (!this.flag)
                return;
            this.invincibility = true;
            this.upprint = false;
            this.downprint = true;
            this.speed = s;
            this.element = ChipBase.ELEMENT.eleki;
            this.animationpoint.X = 0;
            this.hitrange = new Point(0, 2);
            this.hitting = false;
            this.rebirth = this.union == Panel.COLOR.blue;
            --this.position.Y;
            this.positionre = this.position;
            if (pl)
            {
                if (this.union == Panel.COLOR.red)
                    this.positionDirect = new Vector2(this.position.X * 40 + 5, this.position.Y * 24 + 38);
                else
                    this.positionDirect = new Vector2((this.position.X + 1) * 40 - 5, this.position.Y * 24 + 42);
            }
            else if (this.union == Panel.COLOR.red)
                this.positionDirect = new Vector2(this.position.X * 40 + 2, this.position.Y * 24 + 34);
            else
                this.positionDirect = new Vector2((this.position.X + 1) * 40, this.position.Y * 24 + 42);
            this.frame = 0;
        }

        public override void Updata()
        {
            if (this.moveflame)
            {
                if (this.animationpoint.X < 5)
                    ++this.animationpoint.X;
                switch (this.frame)
                {
                    case 4:
                        this.ShakeStart(6);
                        break;
                }
                if (this.frame > 4 && this.hitrange.X < 6)
                    ++this.hitrange.X;
                if (this.hitrange.X >= 6 && !this.start && !this.end)
                {
                    this.start = true;
                    List<Tuple<CharacterBase, int>> characterBaseList = new List<Tuple<CharacterBase, int>>();
                    foreach (CharacterBase characterBase in this.parent.AllHitter())
                    {
                        if (characterBase.position.Y >= this.position.Y && characterBase.position.Y <= this.position.Y + this.hitrange.Y && !characterBase.nohit)
                        {
                            Func<int> calcDamageMultiplier = () => this.DamageMath(characterBase) / this.power;
                            if (this.union == Panel.COLOR.red)
                            {
                                if (characterBase.position.X >= this.position.X)
                                {
                                    characterBaseList.Add(Tuple.Create(characterBase, calcDamageMultiplier()));
                                }
                            }
                            else if (characterBase.position.X <= this.position.X)
                            {
                                characterBaseList.Add(Tuple.Create(characterBase, calcDamageMultiplier()));
                            }
                        }
                    }
                    this.hitter = characterBaseList.ToArray();
                }
                if (this.end && this.animationpoint.X < 10)
                    ++this.animationpoint.X;
                else if (this.end && this.animationpoint.X >= 10)
                {
                    this.ShakeEnd();
                    this.flag = false;
                }
            }
            if (this.start)
            {
                foreach (Tuple<CharacterBase, int> characterHit in this.hitter)
                {
                    var characterBase = characterHit.Item1;
                    var damageMultiplier = characterHit.Item2;
                    if (characterBase.Hp > 0)
                    {
                        this.sound.PlaySE(SoundEffect.damageenemy);
                        characterBase.Hp -= 2 * damageMultiplier;
                        characterBase.Dameged(this);
                        if (characterBase.Hp <= 0)
                            this.sound.PlaySE(SoundEffect.clincher);
                    }
                }
                this.damage += 2;
                if (this.damage >= this.power)
                {
                    this.power = 0;
                    this.hitting = true;
                    this.breaking = true;
                    this.end = true;
                    this.start = false;
                }
            }
            this.FlameControl();
        }

        public override void Render(IRenderer dg)
        {
            this._position = new Vector2(this.positionDirect.X + Shake.X, this.positionDirect.Y + Shake.Y);
            this._rect = new Rectangle(this.animationpoint.X * 48, 616, 48, 96);
            dg.DrawImage(dg, "bomber", this._rect, true, this._position, this.rebirth, Color.White);
            for (int index = 1; index < this.hitrange.X; ++index)
            {
                this._position = new Vector2(this.positionDirect.X + index * 48 * this.UnionRebirth + Shake.X, this.positionDirect.Y + Shake.Y);
                if (this.animationpoint.X < 6)
                    this._rect = new Rectangle((index + 1 < this.hitrange.X ? 1 : 0) * 48, 712, 48, 96);
                else
                    this._rect = new Rectangle((this.animationpoint.X - 4) * 48, 712, 48, 96);
                dg.DrawImage(dg, "bomber", this._rect, true, this._position, this.rebirth, Color.White);
            }
        }

        public override bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            return base.HitCheck(charaposition, charaunion);
        }

        public override bool HitCheck(Point charaposition)
        {
            return base.HitCheck(charaposition);
        }

        public override bool HitEvent(Player p)
        {
            return base.HitEvent(p);
        }

        public override bool HitEvent(EnemyBase e)
        {
            return base.HitEvent(e);
        }

        public override bool HitEvent(ObjectBase o)
        {
            return base.HitEvent(o);
        }
    }
}
