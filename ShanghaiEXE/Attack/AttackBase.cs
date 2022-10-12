using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSEffect;
using NSEnemy;
using NSObject;
using Common.Vectors;
using System.Drawing;

namespace NSAttack
{
    public class AttackBase : CharacterBase
    {
        public bool knock = true;
        public new bool invincibility = true;
        public bool breakinvi = false;
        public bool breaking = false;
        public bool effectMode = false;
        public bool rehit = false;
        public bool canCounter = true;
        public bool panelChange = true;
        public int invincibilitytimeA = 120;
        public bool GrassWeak = true;
        public Point hitrange;
        protected bool bright;
        public bool parry;
        public bool hitting;
        public bool noElementWeak;
        public bool throughObject;
        public bool hitted;
        public int power;
        protected int subpower;
        protected bool over;
        public static long AtIDRed;
        public static long AtIDBlue;
        public string keyName;
        private int damage;

        public AttackBase(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele)
          : base(s, p)
        {
            this.element = ele;
            for (int index1 = 0; index1 < this.hitflag.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.hitflag.GetLength(1); ++index2)
                    this.hitflag[index1, index2] = false;
            }
            this.position = new Point(pX, pY);
            this.power = po;
            this.union = u;
            if (this.position.X < 0 || this.position.X >= 6)
            {
                this.flag = false;
                this.over = true;
            }
            this.keyName = this.ToString();
            if (this.union == Panel.COLOR.red)
            {
                this.keyName = this.keyName + "*" + AttackBase.AtIDRed.ToString();
                ++AttackBase.AtIDRed;
            }
            else
            {
                this.keyName = this.keyName + "*" + AttackBase.AtIDBlue.ToString();
                ++AttackBase.AtIDBlue;
            }
            this.number = 200;
        }

        public AttackBase(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          ChipBase.ELEMENT ele,
          bool counter)
          : base(s, p)
        {
            this.element = ele;
            for (int index1 = 0; index1 < this.hitflag.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.hitflag.GetLength(1); ++index2)
                    this.hitflag[index1, index2] = false;
            }
            this.position = new Point(pX, pY);
            this.power = po;
            this.union = u;
            if (this.position.X < 0 || this.position.X >= 6 || this.position.Y < 0 || this.position.Y >= 3)
            {
                this.flag = false;
                this.over = true;
            }
            this.canCounter = counter;
        }

        public AttackBase(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int subp)
          : base(s, p)
        {
            for (int index1 = 0; index1 < this.hitflag.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.hitflag.GetLength(1); ++index2)
                    this.hitflag[index1, index2] = false;
            }
            this.position = new Point(pX, pY);
            this.power = po;
            this.subpower = subp;
            this.union = u;
            this.invincibilitytimeA = 120;
        }

        public AttackBase(
          IAudioEngine s,
          SceneBattle p,
          int pX,
          int pY,
          Panel.COLOR u,
          int po,
          int subp,
          bool counter)
          : base(s, p)
        {
            for (int index1 = 0; index1 < this.hitflag.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.hitflag.GetLength(1); ++index2)
                    this.hitflag[index1, index2] = false;
            }
            this.position = new Point(pX, pY);
            this.power = po;
            this.subpower = subp;
            this.union = u;
            this.canCounter = counter;
        }

        public virtual bool HitCheck(Point charaposition)
        {
            if (this.PositionOver(charaposition) || this.hitflag[charaposition.X, charaposition.Y])
                return false;
            int y = this.position.Y;
            int num1 = y + this.hitrange.Y;
            int num2;
            int num3;
            if (this.union == Panel.COLOR.red)
            {
                num2 = this.position.X;
                num3 = num2 + this.hitrange.X;
            }
            else
            {
                num3 = this.position.X;
                num2 = num3 - this.hitrange.X;
            }
            return charaposition.X >= num2 && charaposition.X <= num3 && charaposition.Y >= y && charaposition.Y <= num1;
        }

        public void PanelChange()
        {
            for (int index1 = 0; index1 <= this.hitrange.X; ++index1)
            {
                for (int index2 = 0; index2 <= this.hitrange.Y; ++index2)
                {
                    int x = this.position.X + index1 * this.UnionRebirth;
                    int y = this.position.Y + index2;
                    if (this.InAreaCheck(new Point(x, y)))
                    {
                        switch (this.element)
                        {
                            case ChipBase.ELEMENT.heat:
                                if (this.parent.panel[x, y].State == Panel.PANEL._grass)
                                    this.parent.panel[x, y].State = Panel.PANEL._nomal;
                                if (this.parent.panel[x, y].State == Panel.PANEL._ice)
                                {
                                    this.parent.panel[x, y].State = Panel.PANEL._nomal;
                                    break;
                                }
                                break;
                            case ChipBase.ELEMENT.eleki:
                                if (this.parent.panel[x, y].State == Panel.PANEL._ice && this.hitted && this.hitflag[x, y])
                                {
                                    this.parent.panel[x, y].State = Panel.PANEL._nomal;
                                    break;
                                }
                                break;
                            case ChipBase.ELEMENT.poison:
                                if (this.parent.panel[x, y].State == Panel.PANEL._grass)
                                {
                                    this.parent.panel[x, y].State = Panel.PANEL._poison;
                                    break;
                                }
                                break;
                        }
                    }
                }
            }
        }

        public void BadStatusSet(CharacterBase.BADSTATUS bad, int time)
        {
            this.badstatus[(int)bad] = true;
            this.badstatustime[(int)bad] = time;
        }

        public virtual bool HitCheck(AttackBase attack)
        {
            int y1 = this.position.Y;
            int num1 = y1 + this.hitrange.Y;
            int num2;
            int num3;
            if (this.union == Panel.COLOR.red)
            {
                num2 = this.position.X;
                num3 = num2 + this.hitrange.X;
            }
            else
            {
                num3 = this.position.X;
                num2 = num3 - this.hitrange.X;
            }
            int y2 = attack.position.Y;
            int num4 = y1 + attack.hitrange.Y;
            int num5;
            int num6;
            if (this.union == Panel.COLOR.red)
            {
                num5 = attack.position.X;
                num6 = num2 + attack.hitrange.X;
            }
            else
            {
                num6 = attack.position.X;
                num5 = num3 - attack.hitrange.X;
            }
            return (num5 >= num2 && num5 <= num3 || num6 >= num2 && num5 <= num3 || (num2 >= num5 && num2 <= num6 || num3 >= num5 && num2 <= num6)) && (y2 >= y1 && y2 <= num1 || num4 >= y1 && y2 <= num3 || (y1 >= y2 && y1 <= num4 || num1 >= y2 && y1 <= num4));
        }

        protected new void HitFlagReset()
        {
            for (int index1 = 0; index1 < this.hitflag.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.hitflag.GetLength(1); ++index2)
                    this.hitflag[index1, index2] = false;
            }
        }

        protected AttackBase StateCopy(AttackBase a)
        {
            a.badstatus = this.badstatus;
            a.badstatustime = this.badstatustime;
            a.canCounter = this.canCounter;
            a.color = this.color;
            a.invincibility = this.invincibility;
            return a;
        }

        public virtual bool HitCheck(Point charaposition, Panel.COLOR charaunion)
        {
            if (this.union != charaunion)
                return this.HitCheck(charaposition);
            return false;
        }

        public void PanelBright()
        {
            if (!this.flag)
                return;
            int y1 = this.position.Y;
            int num1 = y1 + this.hitrange.Y;
            int num2;
            int num3;
            if (!this.rebirth)
            {
                num2 = this.position.X;
                num3 = num2 + this.hitrange.X;
            }
            else
            {
                num3 = this.position.X;
                num2 = num3 - this.hitrange.X;
            }
            for (int x = 0; x < this.parent.panel.GetLength(0); ++x)
            {
                for (int y2 = 0; y2 < this.parent.panel.GetLength(1); ++y2)
                {
                    if (x >= num2 && x <= num3 && y2 >= y1 && y2 <= num1)
                        this.parent.PanelBright(x, y2);
                }
            }
        }

        public void PanelBright(bool bright)
        {
            if (!(this.flag & bright))
                return;
            int y1 = this.position.Y;
            int num1 = y1 + this.hitrange.Y;
            int num2;
            int num3;
            if (this.union == Panel.COLOR.red)
            {
                num2 = this.position.X;
                num3 = num2 + this.hitrange.X;
            }
            else
            {
                num3 = this.position.X;
                num2 = num3 - this.hitrange.X;
            }
            for (int x = 0; x < this.parent.panel.GetLength(0); ++x)
            {
                for (int y2 = 0; y2 < this.parent.panel.GetLength(1); ++y2)
                {
                    if (x >= num2 && x <= num3 && y2 >= y1 && y2 <= num1)
                        this.parent.PanelBright(x, y2);
                }
            }
        }

        public virtual bool HitEvent(Player c)
        {
            if (c.motion != Player.PLAYERMOTION._death && (!c.invincibility || this.breakinvi))
            {
                if (this.breakinvi && !this.invincibility)
                    c.invincibilitytime = 0;
                int num = 1;
                if (c.standbarrier)
                    num *= 2;
                if (c.guard == CharacterBase.GUARD.armar && !this.breaking)
                    num *= 2;
                this.damage = this.DamageMath(c) / num;
                this.hitflag[c.position.X, c.position.Y] = true;
                c.hitLog.Add(this.keyName);
                if (this.BarierCheck(c, this.damage))
                {
                    this.sound.PlaySE(SoundEffect.damageplayer);
                    if (c.guard != CharacterBase.GUARD.noDamage)
                        c.Hp -= this.damage;
                    if (c.body == CharacterBase.BODY.Synchro)
                    {
                        c.synchroDamage = true;
                        foreach (CharacterBase characterBase in this.parent.AllChara())
                        {
                            if (characterBase.union == c.UnionEnemy)
                                characterBase.Hp -= this.damage;
                        }
                    }
                    if (this.invincibility && this.element != ChipBase.ELEMENT.leaf)
                        c.invincibilitytime = (int)(invincibilitytimeA * (c.ghostDouble ? 1.5 : 1.0));
                    if (this.knock && c.guard != CharacterBase.GUARD.armar)
                    {
                        c.shield = CharacterBase.SHIELD.none;
                        c.Dameged(this.badstatus[3]);
                        if ((uint)c.step > 0U)
                        {
                            c.position = c.stepPosition;
                            c.PositionDirectSet();
                            if (c.step == CharacterBase.STEP.shadow)
                                c.nohit = false;
                            c.flying = c.flyflag;
                            c.step = CharacterBase.STEP.none;
                        }
                    }
                    for (int index = 0; index < this.badstatus.Length; ++index)
                    {
                        if (this.badstatus[index] && c.Element != (ChipBase.ELEMENT)index && !c.Badstatusresist && c.badstatustime[index] >= 0)
                        {
                            c.badstatus[index] = true;
                            c.badstatustime[index] = this.badstatustime[index];
                        }
                    }
                    if (this.parent.blackOut)
                    {
                        if (c.Hp > 0)
                            c.ShakeSingleStart(4, 30);
                        else
                            this.sound.PlaySE(SoundEffect.clincher);
                    }
                    c.Dameged(this);
                    if (c.armarCount == 0 && c.guard == CharacterBase.GUARD.armar)
                    {
                        c.guard = CharacterBase.GUARD.none;
                        this.sound.PlaySE(SoundEffect.breakObject);
                    }
                    return true;
                }
            }
            if (c.armarCount == 0 && c.guard == CharacterBase.GUARD.armar)
            {
                c.guard = CharacterBase.GUARD.none;
                this.sound.PlaySE(SoundEffect.breakObject);
            }
            return false;
        }

        public virtual bool HitEvent(EnemyBase c)
        {
            if (!c.invincibility || this.breakinvi)
            {
                if (this.breakinvi && !this.invincibility)
                    c.invincibilitytime = 0;
                int num = 1;
                if (c.standbarrier)
                    num *= 2;
                if (c.guard == CharacterBase.GUARD.armar)
                    num *= 2;
                this.damage = this.DamageMath(c) / num;
                this.hitflag[c.position.X, c.position.Y] = true;
                bool flag = false;
                if (this.BarierCheck(c, this.damage))
                {
                    this.sound.PlaySE(SoundEffect.damageenemy);
                    if (c.guard != CharacterBase.GUARD.noDamage)
                        c.Hp -= this.damage;
                    if (c.body == CharacterBase.BODY.Synchro)
                    {
                        foreach (CharacterBase characterBase in this.parent.AllChara())
                        {
                            if (characterBase.union == c.UnionEnemy)
                                characterBase.Hp -= this.damage;
                        }
                    }
                    if (!this.parent.blackOut && c.counterTiming && (this.canCounter && !c.badstatus[3]) && c.union == Panel.COLOR.blue)
                    {
                        flag = true;
                        if (this.parent.manyCounter < 3)
                            ++this.parent.manyCounter;
                        this.parent.CounterHit();
                        c.badstatus[3] = true;
                        c.badstatustime[3] = 100;
                        c.counterTiming = false;
                        if (c.Hp <= 0)
                        {
                            switch (c.Element)
                            {
                                case ChipBase.ELEMENT.normal:
                                case ChipBase.ELEMENT.poison:
                                    if (c.race == EnemyBase.ENEMY.navi && !(c is ChipUsingNaviBase))
                                    {
                                        this.parent.lastCounterBug = 3;
                                        break;
                                    }
                                    ++this.parent.lastCounterBug;
                                    break;
                                case ChipBase.ELEMENT.aqua:
                                case ChipBase.ELEMENT.leaf:
                                    if (c.race == EnemyBase.ENEMY.navi && !(c is ChipUsingNaviBase))
                                    {
                                        this.parent.lastCounterFreeze = 3;
                                        break;
                                    }
                                    ++this.parent.lastCounterFreeze;
                                    break;
                                default:
                                    if (c.race == EnemyBase.ENEMY.navi && !(c is ChipUsingNaviBase))
                                    {
                                        this.parent.lastCounterError = 3;
                                        break;
                                    }
                                    ++this.parent.lastCounterError;
                                    break;
                            }
                            c.positionDirect.X += 20f;
                        }
                    }
                    for (int index = 0; index < this.badstatus.Length; ++index)
                    {
                        if (this.badstatus[index] && c.Element != (ChipBase.ELEMENT)index && !c.Badstatusresist && c.badstatustime[index] >= 0)
                        {
                            c.badstatus[index] = true;
                            c.badstatustime[index] = this.badstatustime[index];
                        }
                    }
                    if (this.parent.blackOut)
                    {
                        if (c.Hp > 0)
                        {
                            c.ShakeSingleStart(2, 32);
                            if (c.race == EnemyBase.ENEMY.navi)
                            {
                                NaviBase naviBase = (NaviBase)c;
                                if (this.knock && !naviBase.superArmor && naviBase.guard != CharacterBase.GUARD.armar || naviBase.badstatus[3])
                                {
                                    naviBase.Motion = NaviBase.MOTION.knockback;
                                    naviBase.SlideReset();
                                    if (naviBase is ChipUsingNaviBase)
                                        ((ChipUsingNaviBase)naviBase).waittimeSub = naviBase.badstatus[3] ? 2 : 0;
                                    else if (!naviBase.badstatus[3])
                                        naviBase.waittime = 0;
                                    else
                                        naviBase.waittime = 2;
                                }
                                if (this.invincibility && !naviBase.badstatus[3])
                                    c.invincibilitytime = (int)(invincibilitytimeA * (c.ghostDouble ? 1.5 : 1.0));
                            }
                        }
                        else
                        {
                            this.sound.PlaySE(SoundEffect.bomb);
                            this.sound.PlaySE(SoundEffect.eriasteal2);
                        }
                    }
                    else if (c.race == EnemyBase.ENEMY.virus)
                    {
                        c.whitetime = 4;
                    }
                    else
                    {
                        c.whitetime = 4;
                        NaviBase naviBase = (NaviBase)c;
                        if (this.knock && this.guard != CharacterBase.GUARD.armar && !naviBase.superArmor || naviBase.badstatus[3])
                        {
                            naviBase.shield = CharacterBase.SHIELD.none;
                            naviBase.shieldtime = 0;
                            naviBase.ReflectP = 0;
                            naviBase.Motion = NaviBase.MOTION.knockback;
                            if ((uint)c.step > 0U)
                            {
                                c.position = c.stepPosition;
                                c.PositionDirectSet();
                                if (c.step == CharacterBase.STEP.shadow)
                                    c.nohit = false;
                                c.flying = c.flyflag;
                                c.step = CharacterBase.STEP.none;
                            }
                            if (naviBase is ChipUsingNaviBase)
                                ((ChipUsingNaviBase)naviBase).waittimeSub = naviBase.badstatus[3] ? 2 : 0;
                            else if (!naviBase.badstatus[3])
                                naviBase.waittime = 0;
                            else
                                naviBase.waittime = 2;
                        }
                        if (this.invincibility && !flag && !this.badstatus[3] && this.element != ChipBase.ELEMENT.leaf && (!naviBase.badstatus[4] || this.element == ChipBase.ELEMENT.heat))
                        {
                            c.invincibilitytime = this.invincibilitytimeA;
                            if (naviBase.badstatus[3])
                            {
                                naviBase.badstatus[3] = false;
                                naviBase.badstatustime[3] = 0;
                            }
                        }
                    }
                    c.Dameged(this);
                    if (c.armarCount == 0 && c.guard == CharacterBase.GUARD.armar)
                    {
                        c.guard = CharacterBase.GUARD.none;
                        this.sound.PlaySE(SoundEffect.breakObject);
                    }
                    return true;
                }
            }
            if (c.armarCount == 0 && c.guard == CharacterBase.GUARD.armar)
            {
                c.guard = CharacterBase.GUARD.none;
                this.sound.PlaySE(SoundEffect.breakObject);
            }
            return false;
        }

        public virtual bool HitEvent(ObjectBase o)
        {
            if ((o.union != this.union || o.unionhit) && !o.invincibility && !this.throughObject)
            {
                int num = 1;
                if (o.standbarrier)
                    num *= 2;
                if (o.guard == CharacterBase.GUARD.armar)
                    num *= 2;
                this.damage = this.DamageMath(o) / num;
                o.hit = true;
                this.hitflag[o.position.X, o.position.Y] = true;
                if (this.BarierCheck(o, this.damage))
                {
                    if (this.damage == 0)
                        return true;
                    this.sound.PlaySE(SoundEffect.damageenemy);
                    if (this.breaking && o.guard != CharacterBase.GUARD.guard && !o.slipping)
                        o.Hp = 0;
                    else if (o.guard != CharacterBase.GUARD.noDamage)
                        o.Hp -= this.damage;
                    o.whitetime = 4;
                    this.hitflag[o.position.X, o.position.Y] = true;
                    if (this.parent.blackOut)
                    {
                        if (o.Hp > 0)
                            o.ShakeSingleStart(2, 32);
                        else
                            this.sound.PlaySE(SoundEffect.clincher);
                    }
                    o.Dameged(this);
                    return true;
                }
            }
            return false;
        }

        public int DamageMath(CharacterBase c)
        {
            this.hitted = true;
            int num1 = this.power;
            if (c.guard == CharacterBase.GUARD.Sarmar)
            {
                if (this.breaking)
                {
                    num1 *= 2;
                }
                else
                {
                    this.sound.PlaySE(SoundEffect.damagezero);
                    num1 = 1;
                }
            }
            float num2 = 1f;
            if ((this.WeakPoint(this.Element, c.Element) || this.noElementWeak && c.Element == ChipBase.ELEMENT.normal) && num1 > 0)
            {
                num2 = c.WeakArmor ? 1.5f : 2f;
                this.parent.player.PluspointWitch(20);
                this.parent.effects.Add(new WeakPoint(this.sound, this.parent, c.positionDirect, c.position));
            }
            if (this.InAreaCheck(c.position))
            {
                switch (this.parent.panel[c.position.X, c.position.Y].state)
                {
                    case Panel.PANEL._grass:
                        if (this.Element == ChipBase.ELEMENT.heat)
                        {
                            num2 *= 2f;
                            this.parent.effects.Add(new WeakPointBlue(this.sound, this.parent, c.positionDirect, c.position));
                            break;
                        }
                        break;
                    case Panel.PANEL._ice:
                        if (this.Element == ChipBase.ELEMENT.eleki)
                        {
                            num2 *= 2f;
                            this.parent.effects.Add(new WeakPointBlue(this.sound, this.parent, c.positionDirect, c.position));
                            break;
                        }
                        break;
                }
            }
            if (c.badstatus[(int)ChipBase.ELEMENT.heat] && this.Element == ChipBase.ELEMENT.aqua)
            {
                num2 *= 2f;
                this.parent.effects.Add(new WeakPointBlue(this.sound, this.parent, c.positionDirect, c.position));
                if (c.badstatustime[(int)ChipBase.ELEMENT.heat] > -1)
                {
                    c.badstatustime[(int)ChipBase.ELEMENT.heat] = 0;
                }
            }
            if (c.badstatus[(int)ChipBase.ELEMENT.aqua] && this.Element == ChipBase.ELEMENT.eleki)
            {
                num2 *= 2f;
                this.parent.effects.Add(new WeakPointBlue(this.sound, this.parent, c.positionDirect, c.position));
                if (c.badstatustime[(int)ChipBase.ELEMENT.aqua] > -1)
                {
                    c.badstatustime[(int)ChipBase.ELEMENT.aqua] = 0;
                }
            }
            if (c.badstatus[(int)ChipBase.ELEMENT.leaf] && this.Element == ChipBase.ELEMENT.heat)
            {
                num2 *= 2f;
                this.parent.effects.Add(new WeakPointBlue(this.sound, this.parent, c.positionDirect, c.position));
                if (c.badstatustime[(int)ChipBase.ELEMENT.leaf] > -1)
                {
                    c.badstatustime[(int)ChipBase.ELEMENT.leaf] = 0;
                }
            }
            return (int)(num1 * (double)num2);
        }

        public virtual bool BarierCheck(CharacterBase c, int damage)
        {
            bool flag = false;
            if (c.shield != CharacterBase.SHIELD.none && !this.breaking)
            {
                this.sound.PlaySE(SoundEffect.damagezero);
                this.parent.effects.Add(new Guard(this.sound, this.parent, new Vector2(c.positionDirect.X + this.Random.Next(-16, 16), c.positionDirect.Y + this.Random.Next(-16, 16)), 2, c.position));
                damage = 0;
                if (!c.shieldUsed)
                {
                    switch (c.shield)
                    {
                        case CharacterBase.SHIELD.Reflect:
                            BustorShot bustorShot1 = new BustorShot(this.sound, this.parent, c.position.X - this.UnionRebirth, c.position.Y, this.UnionEnemy, this.power * 2, BustorShot.SHOT.reflect, ChipBase.ELEMENT.normal, false, 0)
                            {
                                blackOutObject = false
                            };
                            this.parent.attacks.Add(bustorShot1);
                            break;
                        case CharacterBase.SHIELD.ReflectP:
                            BustorShot bustorShot2 = new BustorShot(this.sound, this.parent, c.position.X - this.UnionRebirth, c.position.Y, this.UnionEnemy, c.ReflectP, BustorShot.SHOT.reflect, ChipBase.ELEMENT.normal, false, 0)
                            {
                                blackOutObject = false
                            };
                            this.parent.attacks.Add(bustorShot2);
                            break;
                        case CharacterBase.SHIELD.Repair:
                            this.sound.PlaySE(SoundEffect.repair);
                            c.Hp += this.power;
                            BustorShot bustorShot3 = new BustorShot(this.sound, this.parent, c.position.X - this.UnionRebirth, c.position.Y, this.UnionEnemy, this.power, BustorShot.SHOT.reflect, ChipBase.ELEMENT.normal, false, 0)
                            {
                                blackOutObject = false
                            };
                            this.parent.attacks.Add(bustorShot3);
                            break;
                    }
                    c.shieldUsed = true;
                }
                return false;
            }
            if (c.guard == CharacterBase.GUARD.guard && !this.breaking)
            {
                c.NoDameged(this);
                this.sound.PlaySE(SoundEffect.damagezero);
                this.parent.effects.Add(new Guard(this.sound, this.parent, new Vector2(c.positionDirect.X + this.Random.Next(-16, 16), c.positionDirect.Y + this.Random.Next(-16, 16)), 2, c.position));
                damage = 0;
                return false;
            }
            if (c.guard == CharacterBase.GUARD.armar && !this.breaking)
            {
                damage /= 2;
                if (c.armarCount > 0)
                    --c.armarCount;
                this.sound.PlaySE(SoundEffect.damagezero);
            }
            else if (c.guard == CharacterBase.GUARD.armar && this.breaking)
            {
                c.guard = CharacterBase.GUARD.none;
                c.armarCount = 0;
                this.sound.PlaySE(SoundEffect.breakObject);
            }
            switch (c.barrierType)
            {
                case CharacterBase.BARRIER.Barrier:
                case CharacterBase.BARRIER.HealBarrier:
                case CharacterBase.BARRIER.FloteBarrier:
                    if (c.barrierEX)
                        this.sound.PlaySE(SoundEffect.damageenemy);
                    else
                        this.sound.PlaySE(SoundEffect.damagezero);
                    c.barierPower -= damage;
                    if (c.barierPower <= 0)
                        c.DeleteBarier();
                    damage = 0;
                    break;
                case CharacterBase.BARRIER.PowerAura:
                    this.sound.PlaySE(SoundEffect.damagezero);
                    if (damage >= c.barierPower)
                        c.DeleteBarier();
                    damage = 0;
                    break;
                case CharacterBase.BARRIER.ElementsAura:
                    this.sound.PlaySE(SoundEffect.damagezero);
                    if ((uint)this.Element > 0U)
                        c.DeleteBarier();
                    damage = 0;
                    break;
                case CharacterBase.BARRIER.MetalAura:
                    this.sound.PlaySE(SoundEffect.damagezero);
                    if (this.breaking)
                        c.DeleteBarier();
                    damage = 0;
                    break;
                default:
                    flag = true;
                    break;
            }
            return flag;
        }

        private bool WeakPoint(ChipBase.ELEMENT attackele, ChipBase.ELEMENT damageele)
        {
            bool flag = false;
            switch (damageele)
            {
                case ChipBase.ELEMENT.heat:
                    if (attackele == ChipBase.ELEMENT.aqua || attackele == ChipBase.ELEMENT.earth)
                    {
                        flag = true;
                        break;
                    }
                    break;
                case ChipBase.ELEMENT.aqua:
                    if (attackele == ChipBase.ELEMENT.eleki || attackele == ChipBase.ELEMENT.poison)
                    {
                        flag = true;
                        break;
                    }
                    break;
                case ChipBase.ELEMENT.eleki:
                    if (attackele == ChipBase.ELEMENT.leaf || attackele == ChipBase.ELEMENT.earth)
                    {
                        flag = true;
                        break;
                    }
                    break;
                case ChipBase.ELEMENT.leaf:
                    if (attackele == ChipBase.ELEMENT.heat || attackele == ChipBase.ELEMENT.poison)
                    {
                        flag = true;
                        break;
                    }
                    break;
                case ChipBase.ELEMENT.poison:
                    if (attackele == ChipBase.ELEMENT.heat || attackele == ChipBase.ELEMENT.eleki)
                    {
                        flag = true;
                        break;
                    }
                    break;
                case ChipBase.ELEMENT.earth:
                    if (attackele == ChipBase.ELEMENT.aqua || attackele == ChipBase.ELEMENT.leaf)
                    {
                        flag = true;
                        break;
                    }
                    break;
            }
            return flag;
        }
    }
}
