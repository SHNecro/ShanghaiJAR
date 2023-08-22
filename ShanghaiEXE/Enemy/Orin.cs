using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSChip;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSShanghaiEXE.ExtensionMethods;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;
using NSEffect;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NSEnemy
{
    internal class Orin : NaviBase
    {
        private readonly int[] powers = new int[4] { 20, 10, 30, 0 };
        private readonly int nspeed = 2;
        private readonly int moveroop;
        //private Orin.ATTACK attack;
        private int roopneutral;
        private int roopmove;
        private int atackroop;
        private readonly bool atack;

        private bool ready;
        private bool kartFunny;
        private int attackCount;
        private int kartAniAdv;
        private int atacks;
        private int attackMode;
        //private Point delayTarg;

        private int attack_pattern = 0;
        private Point target;

        private Point[] posis;

        private bool hadAura;
        private int rayDrop;
        private int rayDance;
        private int rayIndex;

        private int atkTestInc = 0;
        private int soulRestInc = 0;
        private int soulRestLvl = 20;

        private bool first_up_yours = true;
        private bool first_dying = true;

        private Orin.ATK_LIST orin_attack;
        private enum ATK_LIST
        {
            tail_launch,
            orb_drop,
            tail_hoc,
            orin_sprint,
            kart_dash,
            rand_spirt,
            summon_mook_1,
            summon_mook_2,
            summon_mook_team,
            summon_utsuho_verti_dive,
            summon_utsuho_horiz_dive,
            summon_utsuho_giga_flare,
            meteor_dance,
            tail_launch_tester

        }

        //

        private Point AtkSet_0_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 50, 30, 20 },
                new int[3] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.tail_hoc },
                0, waittime );
        }

        private Point AtkSet_0_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 30, 35, 35 },
                new int[3] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.tail_hoc },
                0, waittime);
        }

        private Point AtkSet_1_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[2] { 60, 40 },
                new int[2] {
                    (int)ATK_LIST.orin_sprint,
                    (int)ATK_LIST.kart_dash},
                0, waittime);
        }

        private Point AtkSet_1_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[2] { 50, 50 },
                new int[2] {
                    (int)ATK_LIST.orin_sprint,
                    (int)ATK_LIST.kart_dash},
                0, waittime);
        }

        private Point AtkSet_2_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 20, 20, 60 },
                new int[3] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.rand_spirt },
                0, waittime);
        }

        private Point AtkSet_2_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 30, 30, 40 },
                new int[3] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.rand_spirt },
                0, waittime);
        }

        private Point AtkSet_3_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[2] { 40, 60},
                new int[2] {
                    (int)ATK_LIST.summon_mook_1,
                    (int)ATK_LIST.summon_mook_2},
                0, waittime);
        }

        private Point AtkSet_3_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[2] { 60, 40 },
                new int[2] {
                    (int)ATK_LIST.summon_mook_1,
                    (int)ATK_LIST.summon_mook_2},
                0, waittime);
        }

        private Point AtkSet_3_a(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[1] { 100 },
                new int[1] {
                    (int)ATK_LIST.summon_mook_team},
                0, waittime);
        }

        private Point AtkSet_4_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[2] { 40, 60 },
                new int[2] {
                    (int)ATK_LIST.summon_utsuho_horiz_dive,
                    (int)ATK_LIST.summon_utsuho_verti_dive},
                0, waittime);
        }

        private Point AtkSet_4_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[2] { 60, 40 },
                new int[2] {
                    (int)ATK_LIST.summon_utsuho_horiz_dive,
                    (int)ATK_LIST.summon_utsuho_verti_dive},
                0, waittime);
        }

        private Point AtkSet_5_a(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[1] { 100 },
                new int[1] {
                    (int)ATK_LIST.summon_utsuho_giga_flare},
                0, waittime);
        }

        private Point AtkSet_6_a(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[1] { 100 },
                new int[1] {
                    (int)ATK_LIST.meteor_dance},
                0, waittime);
        }

        //

        private Point AtkSet_ex0_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 40, 35, 25 },
                new int[3] {
                    (int)ATK_LIST.summon_utsuho_horiz_dive,
                    (int)ATK_LIST.summon_utsuho_verti_dive,
                    (int)ATK_LIST.summon_utsuho_giga_flare },
                0, waittime);
        }

        private Point AtkSet_ex0_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 10, 15, 75 },
                new int[3] {
                    (int)ATK_LIST.summon_utsuho_horiz_dive,
                    (int)ATK_LIST.summon_utsuho_verti_dive,
                    (int)ATK_LIST.summon_utsuho_giga_flare },
                0, waittime);
        }

        private Point AtkSet_ex1_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[5] { 25, 25, 20, 15, 15 },
                new int[5] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.orin_sprint,
                    (int)ATK_LIST.kart_dash,
                    (int)ATK_LIST.rand_spirt},
                0, waittime);
        }

        private Point AtkSet_ex1_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[5] { 15, 20, 30, 25, 10 },
                new int[5] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.orin_sprint,
                    (int)ATK_LIST.kart_dash,
                    (int)ATK_LIST.rand_spirt},
                0, waittime);
        }

        private Point AtkSet_ex2_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[7] { 20, 20, 20, 10, 10, 10, 10 },
                new int[7] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.orin_sprint,
                    (int)ATK_LIST.kart_dash,
                    (int)ATK_LIST.rand_spirt,
                    (int)ATK_LIST.summon_utsuho_horiz_dive,
                    (int)ATK_LIST.summon_utsuho_verti_dive},
                0, waittime);
        }

        private Point AtkSet_ex2_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[7] { 10, 10, 20, 10, 10, 20, 20 },
                new int[7] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.orin_sprint,
                    (int)ATK_LIST.kart_dash,
                    (int)ATK_LIST.rand_spirt,
                    (int)ATK_LIST.summon_utsuho_horiz_dive,
                    (int)ATK_LIST.summon_utsuho_verti_dive},
                0, waittime);
        }

        private Point AtkSet_ex3_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[9] { 15, 15, 15, 10, 10, 10, 10, 5, 15 },
                new int[9] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.orin_sprint,
                    (int)ATK_LIST.kart_dash,
                    (int)ATK_LIST.rand_spirt,
                    (int)ATK_LIST.summon_utsuho_horiz_dive,
                    (int)ATK_LIST.summon_utsuho_verti_dive,
                    (int)ATK_LIST.summon_utsuho_giga_flare,
                    (int)ATK_LIST.meteor_dance},
                0, waittime);
        }

        private Point AtkSet_ex3_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[9] { 10, 10, 15, 10, 5, 5, 15, 5, 25 },
                new int[9] {
                    (int)ATK_LIST.tail_launch,
                    (int)ATK_LIST.orb_drop,
                    (int)ATK_LIST.orin_sprint,
                    (int)ATK_LIST.kart_dash,
                    (int)ATK_LIST.rand_spirt,
                    (int)ATK_LIST.summon_utsuho_horiz_dive,
                    (int)ATK_LIST.summon_utsuho_verti_dive,
                    (int)ATK_LIST.summon_utsuho_giga_flare,
                    (int)ATK_LIST.meteor_dance},
                0, waittime);
        }

        private Point AtkSet_exUY_w0(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[2] { 50, 50 },
                new int[2] {
                    (int)ATK_LIST.summon_utsuho_giga_flare,
                    (int)ATK_LIST.meteor_dance},
                0, waittime);
        }

        private Point AtkSet_exUY_w1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[2] { 40, 60 },
                new int[2] {
                    (int)ATK_LIST.summon_utsuho_giga_flare,
                    (int)ATK_LIST.meteor_dance},
                0, waittime);
        }

        //

        private void pattern_helper_v1_0()
        {
            //
            int rand = this.Random.Next(100);
            int atkIndex = 0;

            switch (this.attack_pattern)
            {
                case 0:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    break;
                case 1:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    break;
                case 2:
                    //
                    atkIndex = AtkSet_1_w0(rand).X;
                    break;
                case 3:
                    //
                    atkIndex = AtkSet_2_w0(rand).X;
                    break;
                case 4:
                    //
                    if (summon_exists())
                    {
                        atkIndex = AtkSet_2_w0(rand).X;
                    }
                    else
                    {
                        atkIndex = AtkSet_3_w0(rand).X;
                    }
                    break;
                case 5:
                    //
                    atkIndex = AtkSet_1_w0(rand).X;
                    break;
                case 6:
                    //
                    atkIndex = AtkSet_4_w0(rand).X;
                    break;
                default:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    this.attack_pattern = 0;
                    break;
            }

            this.orin_attack = (Orin.ATK_LIST)atkIndex;
            this.attack_pattern++;

        }

        private void pattern_helper_v1_1()
        {
            //
            int rand = this.Random.Next(100);
            int atkIndex = 0;

            switch (this.attack_pattern)
            {
                case 0:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    break;
                case 1:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    break;
                case 2:
                    //
                    atkIndex = AtkSet_1_w1(rand).X;
                    break;
                case 3:
                    //
                    atkIndex = AtkSet_2_w1(rand).X;
                    break;
                case 4:
                    //
                    if (summon_exists())
                    {
                        atkIndex = AtkSet_2_w1(rand).X;
                    }
                    else
                    {
                        atkIndex = AtkSet_3_w1(rand).X;
                    }
                    break;
                case 5:
                    //
                    atkIndex = AtkSet_1_w1(rand).X;
                    break;
                case 6:
                    //
                    atkIndex = AtkSet_4_w1(rand).X;
                    break;
                default:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    this.attack_pattern = 0;
                    break;
            }

            this.orin_attack = (Orin.ATK_LIST)atkIndex;
            this.attack_pattern++;

        }

        private void pattern_helper_sp_0()
        {
            //
            int rand = this.Random.Next(100);
            int atkIndex = 0;

            switch (this.attack_pattern)
            {
                case 0:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    break;
                case 1:
                    //
                    atkIndex = AtkSet_2_w0(rand).X;
                    break;
                case 2:
                    //
                    atkIndex = AtkSet_1_w0(rand).X;
                    break;
                case 3:
                    //
                    atkIndex = AtkSet_2_w0(rand).X;
                    break;
                case 4:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    break;
                case 5:
                    //
                    if (summon_exists())
                    {
                        atkIndex = AtkSet_2_w0(rand).X;
                    }
                    else
                    {
                        atkIndex = AtkSet_3_a(rand).X;
                    }
                    break;
                case 6:
                    //
                    atkIndex = AtkSet_1_w0(rand).X;
                    break;
                case 7:
                    //
                    atkIndex = AtkSet_ex0_w0(rand).X;
                    break;
                default:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    this.attack_pattern = 0;
                    break;
            }

            this.orin_attack = (Orin.ATK_LIST)atkIndex;
            this.attack_pattern++;

        }

        private void pattern_helper_sp_1()
        {
            //
            int rand = this.Random.Next(100);
            int atkIndex = 0;

            switch (this.attack_pattern)
            {
                case 0:
                    //
                    atkIndex = AtkSet_1_w1(rand).X;
                    break;
                case 1:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    break;
                case 2:
                    //
                    atkIndex = AtkSet_2_w1(rand).X;
                    break;
                case 3:
                    //
                    atkIndex = AtkSet_1_w1(rand).X;
                    break;
                case 4:
                    //
                    atkIndex = AtkSet_2_w1(rand).X;
                    break;
                case 5:
                    //
                    if (summon_exists())
                    {
                        atkIndex = AtkSet_0_w1(rand).X;
                    }
                    else
                    {
                        atkIndex = AtkSet_3_a(rand).X;
                    }
                    break;
                case 6:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    break;
                case 7:
                    //
                    atkIndex = AtkSet_ex0_w1(rand).X;
                    break;
                default:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    
                    if (this.attack_pattern > 7) this.attack_pattern = 0; else this.attack_pattern = 7;
                    break;
            }

            this.orin_attack = (Orin.ATK_LIST)atkIndex;
            this.attack_pattern++;

        }

        private void pattern_helper_ds_0()
        {
            //
            int rand = this.Random.Next(100);
            int atkIndex = 0;

            switch (this.attack_pattern)
            {
                case 0:
                    //
                    atkIndex = AtkSet_1_w0(rand).X;
                    break;
                case 1:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    break;
                case 2:
                    //
                    atkIndex = AtkSet_1_w0(rand).X;
                    break;
                case 3:
                    //
                    atkIndex = AtkSet_2_w0(rand).X;
                    break;
                case 4:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    break;
                case 7:
                    //
                    if (summon_exists())
                    {
                        atkIndex = AtkSet_2_w0(rand).X;
                    }
                    else
                    {
                        atkIndex = AtkSet_3_w0(rand).X;
                    }
                    break;
                case 6:
                    //
                    atkIndex = AtkSet_2_w0(rand).X;
                    break;
                case 5:
                    //
                    atkIndex = AtkSet_ex0_w0(rand).X;
                    break;
                default:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    this.attack_pattern = 0;
                    break;
            }

            this.orin_attack = (Orin.ATK_LIST)atkIndex;
            this.attack_pattern++;

        }

        private void pattern_helper_ds_1()
        {
            //
            int rand = this.Random.Next(100);
            int atkIndex = 0;

            switch (this.attack_pattern)
            {
                case 0:
                    //
                    atkIndex = AtkSet_1_w1(rand).X;
                    break;
                case 1:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    break;
                case 2:
                    //
                    atkIndex = AtkSet_1_w1(rand).X;
                    break;
                case 3:
                    //
                    atkIndex = AtkSet_2_w1(rand).X;
                    break;
                case 4:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    break;
                case 7:
                    //
                    if (summon_exists())
                    {
                        atkIndex = AtkSet_2_w1(rand).X;
                    }
                    else
                    {
                        atkIndex = AtkSet_3_w1(rand).X;
                    }
                    break;
                case 6:
                    //
                    atkIndex = AtkSet_2_w1(rand).X;
                    break;
                case 5:
                    //
                    atkIndex = AtkSet_ex0_w1(rand).X;
                    break;
                default:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    if (this.attack_pattern > 7) this.attack_pattern = 0; else this.attack_pattern = 7;
                    break;
            }

            this.orin_attack = (Orin.ATK_LIST)atkIndex;
            this.attack_pattern++;

        }

        private void pattern_helper_dssp_0()
        {
            //
            int rand = this.Random.Next(100);
            int atkIndex = 0;

            switch (this.attack_pattern)
            {
                case 0:
                    //
                    atkIndex = AtkSet_ex1_w0(rand).X;
                    break;
                case 1:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    break;
                case 2:
                    //
                    atkIndex = AtkSet_ex2_w0(rand).X;
                    break;
                case 4:
                    //
                    atkIndex = AtkSet_1_w0(rand).X;
                    break;
                case 3:
                    //
                    if (summon_exists())
                    {
                        atkIndex = AtkSet_2_w0(rand).X;
                    }
                    else
                    {
                        atkIndex = AtkSet_3_a(rand).X;
                    }
                    break;
                case 5:
                    //
                    atkIndex = AtkSet_ex3_w0(rand).X;
                    break;
                case 6:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    break;
                case 7:
                    //
                    atkIndex = AtkSet_6_a(rand).X;
                    break;
                case 8:
                    //
                    atkIndex = AtkSet_ex0_w0(rand).X;
                    break;
                case 9:
                    //
                    atkIndex = AtkSet_ex1_w0(rand).X;
                    break;
                case 10:
                    //
                    atkIndex = AtkSet_4_w0(rand).X;
                    break;
                case 11:
                    //
                    atkIndex = AtkSet_ex1_w0(rand).X;
                    break;
                case 12:
                    //
                    atkIndex = AtkSet_ex3_w0(rand).X;
                    break;
                case 13:
                    //
                    atkIndex = AtkSet_ex1_w0(rand).X;
                    break;
                case 14:
                    //
                    atkIndex = AtkSet_6_a(rand).X;
                    break;


                default:
                    //
                    atkIndex = AtkSet_0_w0(rand).X;
                    if (this.attack_pattern > 14) this.attack_pattern = 0; else this.attack_pattern = 11;
                    break;
            }

            this.orin_attack = (Orin.ATK_LIST)atkIndex;
            this.attack_pattern++;

        }

        private void pattern_helper_dssp_1()
        {
            //
            int rand = this.Random.Next(100);
            int atkIndex = 0;

            switch (this.attack_pattern)
            {
                case 0:
                    //
                    atkIndex = AtkSet_ex1_w1(rand).X;
                    break;
                case 1:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    break;
                case 2:
                    //
                    atkIndex = AtkSet_ex2_w1(rand).X;
                    break;
                case 4:
                    //
                    atkIndex = AtkSet_1_w1(rand).X;
                    break;
                case 3:
                    //
                    if (summon_exists())
                    {
                        atkIndex = AtkSet_2_w1(rand).X;
                    }
                    else
                    {
                        atkIndex = AtkSet_3_a(rand).X;
                    }
                    break;
                case 5:
                    //
                    atkIndex = AtkSet_ex3_w1(rand).X;
                    break;
                case 6:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    break;
                case 7:
                    //
                    atkIndex = AtkSet_6_a(rand).X;
                    break;
                case 8:
                    //
                    atkIndex = AtkSet_ex0_w1(rand).X;
                    break;
                case 9:
                    //
                    atkIndex = AtkSet_ex1_w1(rand).X;
                    break;
                case 10:
                    //
                    atkIndex = AtkSet_4_w1(rand).X;
                    break;
                case 11:
                    //
                    atkIndex = AtkSet_ex1_w1(rand).X;
                    break;
                case 12:
                    //
                    atkIndex = AtkSet_ex3_w1(rand).X;
                    break;
                case 13:
                    //
                    atkIndex = AtkSet_ex1_w1(rand).X;
                    break;
                case 14:
                    //
                    atkIndex = AtkSet_6_a(rand).X;
                    break;


                default:
                    //
                    atkIndex = AtkSet_0_w1(rand).X;
                    if (this.attack_pattern > 14) this.attack_pattern = 0; else this.attack_pattern = 11;
                    break;
            }

            this.orin_attack = (Orin.ATK_LIST)atkIndex;
            this.attack_pattern++;

        }


        //

        public Orin(IAudioEngine s, SceneBattle p, int pX, int pY, byte n, Panel.COLOR u, byte v)
          : base(s, p, pX, pY, n, u, v)
        {
            for (int index = 0; index < this.dropchips.Length; ++index)
                this.dropchips[index] = new ChipFolder(this.sound);
            this.element = ChipBase.ELEMENT.normal;
            //this.version = 4;


            switch (this.version)
            {
                case 1:
                    this.name = ShanghaiEXE.Translate("Enemy.OrinName1");
                    //this.fancyName = true;
                    this.power = 160;
                    this.hp = 2600;

                    this.nspeed = 2;

                    this.moveroop = 1;
                    break;
                case 2:
					this.name = ShanghaiEXE.Translate("Enemy.OrinName2");
					//this.fancyName = true;
					this.power = 200;
                    this.hp = 3400;

                    this.nspeed = 2;

                    this.moveroop = 1;
                    break;
                case 3:
					this.name = ShanghaiEXE.Translate("Enemy.OrinName3");
					//this.fancyName = true;


					this.power = 180;
                    this.hp = 3000;

                    this.nspeed = 2;

                    this.barierPower = 111;
                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                    this.barierTime = -1;

                    this.moveroop = 1;
                    break;

                default:
					this.name = ShanghaiEXE.Translate("Enemy.OrinName4");

					this.nspeed = 1;
                    this.power = 300;
                    this.hp = 4444;
                    this.moveroop = 3;

                    this.barierPower = 333;
                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                    this.barierTime = -1;

                    this.version = 4;

                    break;
            }

            this.picturename = "Orin";

            if (this.version > 2)
            {
                this.picturename = "OrinAlter";
            }
            

            this.race = EnemyBase.ENEMY.navi;
            this.Flying = false;
            this.wide = 130;
            this.height = 130;
            this.hpmax = this.hp;
            this.speed = this.nspeed;
            this.hpprint = this.hp;
            this.printhp = true;
            this.effecting = false;
            this.Flying = false;
            this.ready = false;
            this.roopmove = n * 3;
            this.PositionDirectSet();
            switch (this.version)
            {
                default:
                    this.dropchips[0].chip = new UthuhoV3(this.sound);
                    this.dropchips[0].codeNo = 1;
                    this.dropchips[1].chip = new UthuhoV3(this.sound);
                    this.dropchips[1].codeNo = 1;
                    this.dropchips[2].chip = new UthuhoV3(this.sound);
                    this.dropchips[2].codeNo = 1;
                    this.dropchips[3].chip = new UthuhoV3(this.sound);
                    this.dropchips[3].codeNo = 1;
                    this.dropchips[4].chip = new UthuhoV3(this.sound);
                    this.dropchips[4].codeNo = 1;
                    this.havezenny = 666666;
                    break;
            }



        }

        public override void PositionDirectSet()
        {
            this.positionDirect = new Vector2((float)(position.X * 40.0 + 16.0), (float)(position.Y * 24.0 + 58.0));
        }

        protected override void Moving()
        {
            this.neutlal = this.Motion == NaviBase.MOTION.neutral;
            switch (this.Motion)
            {
                case NaviBase.MOTION.neutral:
                    /*
                    this.animationpoint = this.AnimeNeutral(this.frame);
                    if (this.frame >= 7)
                    {
                        this.frame = 0;
                        //++this.roopneutral;
                    }*/
                    //this.animationpoint = new Point((this.frame / 6) % 6, 0);
                    if (this.moveflame)
                        ++this.waittime;


                    if (this.moveflame)
                    {
                        //this.animationpoint = this.AnimeNeutral(this.frame);
                        this.animationpoint.X = 0;
                        this.animationpoint.Y = 0;
                        // code isn't cooperating with attempts to make orin's idle animate
                        if (this.waittime >= 12 / version || this.atack)
                        {
                            this.waittime = 0;
                            ++this.roopneutral;
                            if (this.roopneutral >= 1 && this.parent.nowscene != SceneBattle.BATTLESCENE.end)
                            {
                                this.roopneutral = 0;
                                if (this.roopmove > this.moveroop && !this.badstatus[4])
                                {
                                    this.roopmove = this.version > 0 ? this.Random.Next(-1, this.moveroop + 1) : 0;
                                    ++this.atackroop;
                                    this.speed = 1;
                                    if (!this.atack)
                                    {
                                        //int index = this.Random.Next(this.version > 1 ? 3 : 1);

                                        int randMax = 1;
                                        int index = this.Random.Next(randMax);

                                        this.orin_attack = (Orin.ATK_LIST)index;
                                        this.powerPlus = this.powers[index];

                                        
                                        if (this.version == 1)
                                        {
                                            if (this.hp >= this.hpmax / 2)
                                            {
                                                pattern_helper_v1_0();
                                            }
                                            else
                                            {
                                                pattern_helper_v1_1();
                                            }
                                        }

                                        if (this.version == 2)
                                        {
                                            if (this.hp >= this.hpmax / 2)
                                            {
                                                pattern_helper_sp_0();
                                            }
                                            else
                                            {
                                                pattern_helper_sp_1();
                                            }
                                        }

                                        if (this.version == 3)
                                        {
                                            if (this.hp >= this.hpmax / 2)
                                            {
                                                pattern_helper_ds_0();
                                            }
                                            else
                                            {
                                                pattern_helper_ds_1();
                                            }
                                        }

                                        if (this.version == 4)
                                        {
                                            if (this.hp >= this.hpmax / 2)
                                            {
                                                pattern_helper_dssp_0();
                                            }
                                            else
                                            {
                                                if (this.first_up_yours)
                                                {
                                                    this.orin_attack = Orin.ATK_LIST.meteor_dance;
                                                    this.first_up_yours = false;
                                                }
                                                else
                                                {
                                                    pattern_helper_dssp_1();
                                                }
                                                
                                            }

                                            

                                        }

                                    }
                                    this.waittime = 0;
                                    this.ready = false;
                                    this.attackCount = 0;
                                    this.atacks = 0;
                                    this.attackMode = 0;
                                    this.rayDance = 0;
                                    this.Motion = NaviBase.MOTION.attack;
                                    this.counterTiming = true;

                                    if (this.version == 3)
                                        if (this.barrierType == CharacterBase.BARRIER.None)
                                        {
                                            this.soulRestInc++;
                                            if (this.soulRestInc >= this.soulRestLvl)
                                            {
                                                this.barierPower = 111;
                                                this.barrierType = CharacterBase.BARRIER.PowerAura;
                                                this.barierTime = -1;
                                                this.soulRestInc = 0;
                                            }
                                        }
                                        else
                                        {
                                            this.soulRestInc = 0;
                                        }

                                    if (this.version == 4)
                                    {
                                        
                                            this.barrierType = CharacterBase.BARRIER.None;
                                        
                                    }




                                }
                                else
                                {
                                    this.waittime = 0;
                                    if (this.atack)
                                        this.roopmove = this.moveroop + 1;
                                    this.Motion = NaviBase.MOTION.move;
                                }
                            }
                        }
                        break;
                    }
                    break;



                case NaviBase.MOTION.attack:
                    if (this.moveflame)
                    {
                        ++this.waittime;
                        if (this.moveflame)
                        {
                            switch (this.orin_attack)
                            {
                                case Orin.ATK_LIST.tail_launch:
                                    //
                                    this.attack_tail_launch();

                                    break;
                                case Orin.ATK_LIST.orb_drop:
                                    //
                                    this.attack_orb_drop();

                                    break;
                                case Orin.ATK_LIST.tail_hoc:
                                    //
                                    this.attack_tail_hoc();

                                    break;
                                case Orin.ATK_LIST.orin_sprint:
                                    //
                                    this.attack_sprint();

                                    break;
                                case Orin.ATK_LIST.kart_dash:
                                    //
                                    this.attack_kart_dash();

                                    break;
                                case Orin.ATK_LIST.rand_spirt:
                                    //
                                    this.attack_rand_spirit();

                                    break;
                                case Orin.ATK_LIST.summon_mook_1:
                                    //
                                    this.attack_summon_mook_1();

                                    break;
                                case Orin.ATK_LIST.summon_mook_2:
                                    //
                                    this.attack_summon_mook_2();

                                    break;
                                case Orin.ATK_LIST.summon_mook_team:
                                    //
                                    this.attack_summon_mook_team();

                                    break;
                                case Orin.ATK_LIST.summon_utsuho_verti_dive:
                                    //
                                    this.attack_summon_utsuho_verti_dive();

                                    break;
                                case Orin.ATK_LIST.summon_utsuho_horiz_dive:
                                    //
                                    this.attack_summon_utsuho_horiz_dive();

                                    break;
                                case Orin.ATK_LIST.summon_utsuho_giga_flare:
                                    //
                                    this.attack_summon_utsuho_giga_flare();

                                    break;
                                case Orin.ATK_LIST.meteor_dance:
                                    //
                                    this.attack_meteor_dance();

                                    break;
                                case Orin.ATK_LIST.tail_launch_tester:
                                    //
                                    this.attack_tail_launch();

                                    break;

                            }
                            if (this.version == 4)
                            if (this.Motion == NaviBase.MOTION.neutral)
                            {
                                    this.barierPower = 333;
                                    this.barrierType = CharacterBase.BARRIER.PowerAura;
                                    this.barierTime = -1;

                            }
                        }
                        break;
                    }
                    break;
                case NaviBase.MOTION.move:
                    this.animationpoint = this.AnimeMove(this.waittime);
                    if (this.moveflame)
                    {
                        switch (this.waittime)
                        {
                            case 0:
                                /*
                                if (this.version == 4)
                                {
                                    if (!aura_checker())
                                    {

                                    }
                                }
                                */
                                this.MoveRandom(false, false);
                                if (this.position == this.positionre)
                                {
                                    this.Motion = NaviBase.MOTION.neutral;
                                    this.frame = 0;
                                    this.roopneutral = 0;
                                    ++this.roopmove;
                                    break;
                                }
                                break;
                            case 3:
                                this.position = this.positionre;
                                this.PositionDirectSet();
                                break;
                            case 5:
                                this.Motion = NaviBase.MOTION.neutral;
                                this.frame = 0;
                                this.roopneutral = 0;
                                ++this.roopmove;
                                break;
                        }
                        ++this.waittime;
                        break;
                    }
                    break;
                case NaviBase.MOTION.knockback:
                    switch (this.waittime)
                    {
                        case 2:
                            this.NockMotion();
                            this.counterTiming = false;
                            this.effecting = false;
                            //
                            this.superArmor = false;
                            this.rebirth = this.union == Panel.COLOR.red;
                            this.Noslip = false;
                            this.speed = this.nspeed;
                            this.ready = false;
                            //
                            this.PositionDirectSet();
                            break;
                        case 3:
                            this.NockMotion();
                            break;
                        case 15:
                            this.animationpoint = new Point(4, 0);
                            this.PositionDirectSet();
                            break;
                        case 21:
                            this.animationpoint = new Point(0, 0);
                            this.waittime = 0;
                            this.Motion = NaviBase.MOTION.neutral;
                            break;
                    }
                    if (this.waittime >= 2 && this.waittime <= 6)
                        this.positionDirect.X -= this.UnionRebirth(this.union);
                    ++this.waittime;


                    break;
            }
            this.FlameControl();
            this.MoveAftar();
        }

        public override void NockMotion()
        {
            this.animationpoint = new Point(9, 0);
        }

        public override void Render(IRenderer dg)
        {
            if (this.alfha < byte.MaxValue)
                this.color = Color.FromArgb(alfha, this.mastorcolor);
            else
                this.color = this.mastorcolor;
            double num1 = (int)this.positionDirect.X + this.Shake.X;
            int y1 = (int)this.positionDirect.Y;
            Point shake = this.Shake;
            int y2 = shake.Y;
            double num2 = y1 + y2;
            this._position = new Vector2((float)num1-10, (float)num2-18);

            int yTmp = 2;
            if (this.version == 1) { yTmp = 0; }
            if (this.version == 2) { yTmp = 2; }
            if (this.version == 3) { yTmp = 0; }

            this._rect = new Rectangle(this.animationpoint.X * this.wide, 780 * (yTmp) + this.animationpoint.Y * this.height, this.wide, this.height);
            if (this.Hp <= 0)
            {
                this.rd = this._rect;
                this.NockMotion();
                int num3 = this.animationpoint.X * this.wide;
                shake = this.Shake;
                int x1 = shake.X;
                int x2 = num3 + x1;
                int y3 = 780 * (yTmp) + this.animationpoint.Y * this.height;
                int wide = this.wide;
                int height1 = this.height;
                shake = this.Shake;
                int y4 = shake.Y;
                int height2 = height1 + y4;
                this._rect = new Rectangle(x2, y3, wide, height2);
                //this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height), this._position, this.picturename);
                this.Death(this._rect, new Rectangle(this.animationpoint.X * this.wide, this.height, this.wide, this.height)
                {
                    Y = 780 + this.animationpoint.Y * this.height
                }, this._position, this.picturename);


                if (this.first_dying)
                {

                    foreach (EnemyBase go in this.parent.enemys)
                    {
                        if (go is OrinMook1)
                        {
                            go.flag = false;
                            
                        }
                        if (go is OrinMook2)
                        {
                            go.flag = false;
                        }


                    }

                    this.first_dying = false;
                }


            }
            if (this.whitetime == 0)
            {
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            else
            {
                //this._rect.Y = this.height;
                this._rect.Y = 780 + this.animationpoint.Y * this.height;
                dg.DrawImage(dg, this.picturename, this._rect, false, this._position, this.rebirth, this.color);
            }
            this.HPposition = new Vector2((int)this.positionDirect.X + 8, (int)this.positionDirect.Y - 8 - 32 + 8);
            this.Nameprint(dg, this.printNumber);
        }

        public override void BarrierRend(IRenderer dg)
        {
            this.BarrierRender(dg, new Vector2(this.positionDirect.X + 0f, this.positionDirect.Y + 0f));
        }

        public override void BarrierPowerRend(IRenderer dg)
        {
            this.BarrierPowerRender(dg, new Vector2(this.positionDirect.X + 0f, this.positionDirect.Y + 0f));
        }
        //


        private Point AnimeNeutral(int waittime)
        {
            return this.Return(
                new int[6] { 0, 1, 2, 3, 4, 5 },
                new int[6] { 0, 1, 2, 3, 4, 5 },
                0, waittime);

        }

        private Point AnimeNeutral_s(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 2, 2, 2 },
                new int[3] { 0, 1, 2 },
                0, waittime);

        }

        private Point AnimeMove(int waitflame)
        {
            return this.Return(new int[5] { 0, 1, 2, 4, 5 }, new int[5]
            {
        0,
        7,
        6,
        7,
        0
            }, 0, waitflame);
        }

        private Point AnimeTailLaunch_x(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[9] {4,  4,  4,  4, 4, 4, 2, 2, 2},
                new int[9] {10, 11, 12, 0, 1, 2, 3, 4, 5},
                0,
                waittime
                );
        }

        private Point AnimeTailLaunch_y(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[9] { 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                new int[9] { 0, 0, 0, 1, 1, 1, 1, 1, 1 },
                0,
                waittime
                );
        }

        private Point AnimeOrbDrop(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[5] { 4, 4, 4, 4, 4+60},
                new int[5] { 4, 5, 6, 7, 8},
                1,
                waittime
                );
        }

        private Point AnimeTailHoc_x(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[7] { 4, 4,  4,  8,  12, 4, 4+60},
                new int[7] { 8, 9, 10, 11, 12, 0, 1},
                0,
                waittime
                );
        }

        private Point AnimeTailHoc_y(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[7] { 4, 4, 4, 8, 12, 4, 4+60 },
                new int[7] { 1, 1, 1, 1, 1, 2, 2 },
                0,
                waittime
                );
        }

        private Point AnimeSprint_1(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[6] { 2, 2, 2, 2, 2, 2 + 60 },
                new int[6] { 0, 1, 2, 3, 4, 5 },
                2,
                waittime
                );
        }

        private Point AnimeSprint_2(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 2, 2, 2 },
                new int[3] { 6, 7, 8 },
                2,
                waittime
                );
        }

        private Point AnimeKartDeploy_x(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[9] { 4, 4,  4,  4,  4, 4, 4, 4, 4 + 60 },
                new int[9] { 9, 10, 11, 12, 0, 1, 2, 3, 4 },
                0,
                waittime
                );
        }

        private Point AnimeKartDeploy_y(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[9] { 4, 4, 4, 4, 4, 4, 4, 4, 4 + 60 },
                new int[9] { 2, 2, 2, 2, 3, 3, 3, 3, 3 },
                0,
                waittime
                );
        }

        private Point AnimeKartDash(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[3] { 3, 3, 3 },
                new int[3] { 5, 6, 7 },
                3,
                waittime
                );
        }

        private Point AnimeKartDashFunny(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[6] { 4, 4, 4, 4, 4, 4 },
                new int[6] { 6, 7, 8, 9, 10, 11 },
                5,
                waittime
                );
        }

        private Point AnimeRandSpirit_x(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[8] { 4,  4,  4,  4, 4, 4, 4, 4 },
                new int[8] { 9, 10, 11, 12, 0, 1, 2, 3 },
                0,
                waittime
                );
        }

        private Point AnimeRandSpirit_y(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[8] { 4, 4, 4, 4, 4, 4, 4, 4 },
                new int[8] { 3, 3, 3, 3, 4, 4, 4, 4 },
                0,
                waittime
                );
        }

        private Point AnimeOrinDance(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[6] { 4, 4, 4, 4, 4, 4 },
                new int[6] { 4, 5, 6, 7, 8, 9 },
                4,
                waittime
                );
        }

        private Point AnimeUtsuhoCall_x(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[9] { 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                new int[9] { 10, 11, 12, 0, 1, 2, 3, 4, 5 },
                0,
                waittime
                );
        }

        private Point AnimeUtsuhoCall_y(int waittime)
        {
            return CharacterAnimation.ReturnKai(
                new int[9] { 4, 4, 4, 4, 4, 4, 4, 4, 4 },
                new int[9] { 4, 4, 4, 5, 5, 5, 5, 5, 5 },
                0,
                waittime
                );
        }
        //

        public void attack_tail_launch()
        {
            this.animationpoint.X = AnimeTailLaunch_x(this.waittime).X;
            this.animationpoint.Y = AnimeTailLaunch_y(this.waittime).X;

            switch (this.waittime)
            {
                case 16:
                    this.counterTiming = false;

                    int butMov = 6 + this.version*2;

                    this.sound.PlaySE(SoundEffect.canon);
                    this.parent.attacks.Add(new OrinFireballDouble(this.sound, this.parent, this.position.X, this.position.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, butMov, this.version));

                    break;
                case 26:
                    this.roopneutral = 0;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (!this.atack)
                    {
                        this.speed = this.nspeed;
                        break;
                    }
                    break;
            }
        }

        public void attack_orb_drop()
        {
            this.animationpoint = AnimeOrbDrop(this.waittime);

            switch (this.waittime)
            {
                case 12:
                    this.counterTiming = false;

                    //int butMov = 8;
                    this.sound.PlaySE(SoundEffect.water);

                    this.target = this.RandomTarget(this.union);
                    int z = 80;

                    OrinSoul soul = new OrinSoul(this.sound, this.parent, this.target.X, this.target.Y, z, null, 30, this.version);
                    OrinOrbDrop orb = new OrinOrbDrop(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power / 2, 2, new Vector2(soul.positionDirect.X, soul.positionDirect.Y - z), this.target, 120, OrinOrbDrop.TYPE.single, true, OrinOrbDrop.TYPE.single, true, true);
                    orb.invincibility = true;
                    //orb.BadStatusSet(CharacterBase.BADSTATUS.melt, 600);
                    //orb.element = ChipBase.ELEMENT.heat;
                    orb.palette = this.version;
                    soul.attack = orb;
                    
                    this.parent.effects.Add(soul);

                    
                    break;
                case 20:
                    this.roopneutral = 0;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (!this.atack)
                    {
                        this.speed = this.nspeed;
                        break;
                    }
                    break;
            }
        }

        public void attack_tail_hoc()
        {
            this.animationpoint.X = AnimeTailHoc_x(this.waittime).X;
            this.animationpoint.Y = AnimeTailHoc_y(this.waittime).X;

            switch (this.waittime)
            {
                case 20:
                    this.counterTiming = false;

                    this.target = this.RandomTarget(this.union);

                    int x_add = -1;
                    if (this.union != 0)
                    {
                        x_add = 1;
                    }

                    int targ_x = Math.Min(5,Math.Max(this.target.X + x_add, 0));

                    int butMov = 8 + this.version;
                    this.sound.PlaySE(SoundEffect.canon);
                    //this.parent.attacks.Add(new OrinFireballDouble(this.sound, this.parent, targ_x, this.position.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, butMov));
                    /*
                    OrinTailHoc knifeAttack = new KnifeAttack(this.sound, this.parent, targ_x, 0, this.union, this.Power, 2, ChipBase.ELEMENT.heat, false);
                    this.parent.attacks.Add(knifeAttack);

                    KnifeAttack knifeAttack2 = new KnifeAttack(this.sound, this.parent, targ_x, 2, this.union, this.Power, 2, ChipBase.ELEMENT.heat, false);
                    this.parent.attacks.Add(knifeAttack2);
                    */
                    int s = 2;
                    int aS = 2;

                    if (this.version == 4) aS = 1;

                    Point tmpPos = this.position;
                    this.position.X = targ_x;
                    this.position.Y = 0;
                    this.PositionDirectSet();
                    this.parent.attacks.Add(new OrinTailHoc(this.sound, this.parent, targ_x, 0, this.union, this.Power, s, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, true, aS, this.version));
                    this.position.Y = 2;
                    this.PositionDirectSet();
                    this.parent.attacks.Add(new OrinTailHoc(this.sound, this.parent, targ_x, 2, this.union, this.Power, s, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, false, aS, this.version));
                    
                    this.position = tmpPos;
                    this.PositionDirectSet();
                    // wonky workaround for lack of knowledge on direct coord settings/functions
                    break;
                case 34:
                    this.roopneutral = 0;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (!this.atack)
                    {
                        this.speed = this.nspeed;
                        break;
                    }
                    break;
            }
        }

        //

        public void attack_sprint()
        {

            if (!this.ready)
            {
                this.animationpoint = this.AnimeSprint_1(this.waittime);
                switch (this.waittime)
                {
                    case 1:
                        this.counterTiming = true;
                        this.sound.PlaySE(SoundEffect.futon);
                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X, this.position.Y, this.union, new Point(9, 0), 30, true));
                        break;
                    case 15:
                        this.superArmor = true;
                        this.overMove = true;
                        this.counterTiming = false;
                        this.ready = true;
                        this.effecting = true;
                        this.waittime = 0;
                        this.sound.PlaySE(SoundEffect.shoot);
                        break;
                }
            }
            else
            {
                this.animationpoint = this.AnimeSprint_2(this.waittime);
                if (positionDirect.X >= 0.0 && positionDirect.X <= 240.0)
                {
                    this.nohit = false;
                    this.AttackMake(this.Power, 0, 0, true);
                    //this.AttackMake(this.Power, 0, -1, false);
                    //this.AttackMake(this.Power, 0, 1, false);
                }
                else
                    this.nohit = true;

                int tmp = this.version;
                int atkLim = 1;
                if (this.version != 1)
                    atkLim = 2;
                

                this.positionDirect.X += Math.Min(8 + tmp, 12) * (!this.rebirth ? -1 : 1);
                this.position.X = this.Calcposition(this.positionDirect, this.height, false).X;
                int num = 240;
                if (positionDirect.X < (double)-num || positionDirect.X > (double)(240 + num))
                {
                    ++this.attackCount;
                    if (this.attackCount >= atkLim)
                    {
                        this.rebirth = false;
                        this.effecting = false;
                        this.superArmor = false;
                        this.motion = NaviBase.MOTION.move;
                        this.overMove = false;
                        this.nohit = false;
                        this.animationpoint = new Point();
                        this.roopneutral = 0;
                        this.frame = 0;
                        this.waittime = 0;
                        this.attackCount = 0;
                        this.speed = this.nspeed;
                    }
                    else
                    {
                        //this.rebirth = true;
                        
                        if (this.rebirth)
                        {
                            this.rebirth = false;
                        }
                        else
                        {
                            this.rebirth = true;
                        }

                        this.sound.PlaySE(SoundEffect.shoot);
                        this.HitFlagReset();
                        this.positionre = new Point(this.position.X, this.position.Y);
                        this.position = this.positionre;
                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.union == Panel.COLOR.blue ? 5 : 0, this.position.Y, this.union, new Point(9, 0), 30, true));
                        this.PositionDirectSet();
                    }
                }


            }




        }

        public void attack_kart_dash()
        {
            if (!this.ready)
            {

                this.animationpoint.X = AnimeKartDeploy_x(this.waittime).X;
                this.animationpoint.Y = AnimeKartDeploy_y(this.waittime).X;

                switch (this.waittime)
                {
                    case 1:
                        this.sound.PlaySE(SoundEffect.charge);
                        break;
                    case 17:
                        this.counterTiming = true;
                        this.sound.PlaySE(SoundEffect.quake);
                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.position.X, this.position.Y, this.union, new Point(9, 0), 30, true));
                        break;
                    case 25:
                        this.superArmor = true;
                        this.overMove = true;
                        this.counterTiming = false;
                        this.ready = true;
                        this.effecting = true;
                        this.waittime = 0;
                        this.sound.PlaySE(SoundEffect.shoot);

                        this.kartFunny = false;
                        this.kartAniAdv = 0;
                        break;
                }
            }
            else
            {
                if (!this.kartFunny)
                    this.animationpoint = this.AnimeKartDash(this.kartAniAdv % 9);
                else
                    this.animationpoint = this.AnimeKartDashFunny(this.kartAniAdv % 24);

                if (positionDirect.X >= 0.0 && positionDirect.X <= 240.0)
                {
                    this.nohit = false;
                    this.AttackMake(this.Power, 0, 0, true);
                    //this.AttackMake(this.Power, 0, -1, false);
                    //this.AttackMake(this.Power, 0, 1, false);
                }
                else
                    this.nohit = true;

                int tmp = this.version;
                int atkLim = 2 + this.version;
                this.kartAniAdv++;

                if (this.version == 4)
                {
                    tmp += 2;
                    atkLim += 2;
                }

                this.positionDirect.X += Math.Min(8 + tmp, 15) * (!this.rebirth ? -1 : 1);
                this.position.X = this.Calcposition(this.positionDirect, this.height, false).X;
                int num = 240;
                if (positionDirect.X < (double)-num || positionDirect.X > (double)(240 + num))
                {
                    ++this.attackCount;
                    if (this.attackCount >= atkLim)
                    {
                        //this.rebirth = false;
                        this.effecting = false;
                        this.superArmor = false;
                        this.motion = NaviBase.MOTION.move;
                        this.overMove = false;
                        this.nohit = false;
                        this.animationpoint = new Point();
                        this.roopneutral = 0;
                        this.frame = 0;
                        this.waittime = 0;
                        this.attackCount = 0;
                        this.speed = this.nspeed;
                    }
                    else
                    {

                        if (this.Random.Next(8) > 5)
                            this.kartFunny = true;
                        else
                            this.kartFunny = false;

                        this.sound.PlaySE(SoundEffect.shoot);
                        this.HitFlagReset();
                        //this.animationpoint.X = 0;
                        this.parent.effects.Add(new MoveEnemy(this.sound, this.parent, this.position.X, this.position.Y));
                        this.positionre = new Point(this.union == Panel.COLOR.blue ? 5 : 0, this.RandomTarget().Y);
                        this.position = this.positionre;
                        this.PositionDirectSet();
                    }
                }


            }



        }

        public void attack_rand_spirit()
        {

            int lim = 3 + this.version;
            this.animationpoint.X = AnimeRandSpirit_x(this.rayDance).X;
            this.animationpoint.Y = AnimeRandSpirit_y(this.rayDance).X;

            // 32 for 'mid'?
            // 56 length

            this.rayDance++;
            if (this.rayDance >= 4*8)
            {
                this.rayDance = 0;
            }

            switch (this.waittime)
            {
                case 1:
                    //this.MoveRandom(false, false, this.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red, 2);
                    //this.target = this.positionre;
                    //this.positionre = this.position;

                    this.target = this.RandomTarget();
                    this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, this.target.Y, this.union, new Point(), 24, true));
                    //

                    break;

                case 16:
                    this.counterTiming = false;
                    break;
                case 32:
                    //
                    //this.positionre = this.position;
                    OrinSpirit knifeAttack = new OrinSpirit(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, 3, ChipBase.ELEMENT.heat, false, this.version);
                    this.parent.attacks.Add(knifeAttack);

                    this.sound.PlaySE(SoundEffect.dark);

                    ++this.atacks;
                    if (this.atacks < lim)
                    {
                        this.MoveRandom(false, false, this.union == Panel.COLOR.red ? Panel.COLOR.blue : Panel.COLOR.red, 1);
                        this.target = this.positionre;
                        this.positionre = this.position;
                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.target.X, this.target.Y, this.union, new Point(), 24, true));
                        //
                    }

                    break;
                case 64:
                    //
                    OrinSpirit knifeAttack2 = new OrinSpirit(this.sound, this.parent, this.target.X, this.target.Y, this.union, this.Power, 3, ChipBase.ELEMENT.heat, false, this.version);

                    this.parent.attacks.Add(knifeAttack2);

                    this.sound.PlaySE(SoundEffect.dark);

                    ++this.atacks;
                    this.waittime = 0;
                    break;
                
                
            }

            if (this.atacks > lim)
            {
                this.roopneutral = 0;
                this.Motion = NaviBase.MOTION.neutral;
                if (!this.atack)
                {
                    this.speed = this.nspeed;
                    //
                }
            }
            

        }

        public void attack_summon_mook_1()
        {
            //
            this.animationpoint = AnimeOrinDance(this.waittime % 24);

            switch (this.waittime)
            {
                case 12:
                    //
                    this.counterTiming = false;

                    break;
                case 24:
                    //

                    var spawnPanels = this.RandomMultiPanel(1, this.union, false);
                    var spawnPanel = spawnPanels.Length == 0 ? null : new Point?(spawnPanels[0]);
                    if (spawnPanel != null)
                    {
                        this.sound.PlaySE(SoundEffect.dark);
                        this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.FromArgb(0x80, 0x60, 0x40, 0x60), 15));

                        OrinMook1 orinSummon = new OrinMook1(this.sound, this.parent, spawnPanel.Value.X, spawnPanel.Value.Y, 42, this.union, 11);
                        orinSummon.Hp = 150;
                        if (this.version != 1)
                        {
                            orinSummon.Hp = 180;
                        }
                        this.parent.enemys.Add(orinSummon);
                        orinSummon.Init();
                        orinSummon.InitAfter();

                    }

                    break;
                case 36:
                    this.roopneutral = 0;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (!this.atack)
                    {
                        this.speed = this.nspeed;
                        break;
                    }
                    break;
            }

        }

        public void attack_summon_mook_2()
        {
            //
            this.animationpoint = AnimeOrinDance(this.waittime % 24);

            switch (this.waittime)
            {
                case 12:
                    //
                    this.counterTiming = false;

                    break;
                case 24:
                    //

                    var spawnPanels = this.RandomMultiPanel(1, this.union, false);
                    var spawnPanel = spawnPanels.Length == 0 ? null : new Point?(spawnPanels[0]);
                    if (spawnPanel != null)
                    {
                        this.sound.PlaySE(SoundEffect.dark);
                        this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.FromArgb(0x80, 0x60, 0x40, 0x60), 15));

                        OrinMook2 orinSummon = new OrinMook2(this.sound, this.parent, spawnPanel.Value.X, spawnPanel.Value.Y, 42, this.union, 11);
                        orinSummon.Hp = 150;
                        if (this.version != 1)
                        {
                            orinSummon.Hp = 180;
                        }
                        this.parent.enemys.Add(orinSummon);
                        orinSummon.Init();
                        orinSummon.InitAfter();

                    }

                    break;
                case 36:
                    this.roopneutral = 0;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (!this.atack)
                    {
                        this.speed = this.nspeed;
                        break;
                    }
                    break;
            }

        }

        public void attack_summon_mook_team()
        {
            //
            this.animationpoint = AnimeOrinDance(this.waittime % 24);

            switch (this.waittime)
            {
                case 12:
                    //
                    this.counterTiming = false;

                    break;
                case 24:
                    //

                    var spawnPanels = this.RandomMultiPanel(1, this.union, false);
                    var spawnPanel = spawnPanels.Length == 0 ? null : new Point?(spawnPanels[0]);
                    if (spawnPanel != null)
                    {
                        this.sound.PlaySE(SoundEffect.dark);
                        this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.FromArgb(0x80, 0x60, 0x40, 0x60), 15));

                        OrinMook1 orinSummon = new OrinMook1(this.sound, this.parent, spawnPanel.Value.X, spawnPanel.Value.Y, 42, this.union, 11);
                        orinSummon.Hp = 200;
                        this.parent.enemys.Add(orinSummon);
                        orinSummon.Init();
                        orinSummon.InitAfter();


                    }

                    spawnPanels = this.RandomMultiPanel(1, this.union, false);
                    spawnPanel = spawnPanels.Length == 0 ? null : new Point?(spawnPanels[0]);
                    if (spawnPanel != null)
                    {
                        this.sound.PlaySE(SoundEffect.dark);
                        this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.FromArgb(0x80, 0x60, 0x40, 0x60), 15));

                        OrinMook2 orinSummon2 = new OrinMook2(this.sound, this.parent, spawnPanel.Value.X, spawnPanel.Value.Y, 42, this.union, 11);
                        orinSummon2.Hp = 200;
                        this.parent.enemys.Add(orinSummon2);
                        orinSummon2.Init();
                        orinSummon2.InitAfter();

                    }

                    break;
                case 36:
                    this.roopneutral = 0;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (!this.atack)
                    {
                        this.speed = this.nspeed;
                        break;
                    }
                    break;
            }

        }
        // not usable v1

        public void attack_summon_utsuho_verti_dive()
        {
            //
            this.animationpoint.X = AnimeUtsuhoCall_x(this.waittime).X;
            this.animationpoint.Y = AnimeUtsuhoCall_y(this.waittime).X;

            switch (this.waittime)
            {
                case 12:
                    //
                    this.counterTiming = false;

                    break;
                case 24:
                    //
                    
                    this.target = RandomTarget();
                    if (this.target != null)
                    {
                        //this.sound.PlaySE(SoundEffect.dark);
                        //this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.FromArgb(0x80, 0x60, 0x40, 0x60), 15));
                        int butMov = 1 + this.version;
                        int butSpd = 3;

                        if (this.version != 1)
                        {
                            butSpd = 2;
                        }

                        if (this.version == 4)
                        {
                            butSpd = 1;
                        }

                        Point tmpPos = this.position;
                        this.position.X = this.target.X;
                        this.position.Y = 0;
                        this.PositionDirectSet();

                        this.parent.attacks.Add(new OrinSummonUtsuhoVerti(this.sound, this.parent, this.target.X, 0, this.union, this.Power, butSpd, new Vector2(this.positionDirect.X + 4f, this.positionDirect.Y - 30f), ChipBase.ELEMENT.normal, butMov, this.version));

                        this.position = tmpPos;
                        this.PositionDirectSet();

                    }

                    break;
                case 36:
                    this.roopneutral = 0;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (!this.atack)
                    {
                        this.speed = this.nspeed;
                        break;
                    }
                    break;
            }
        }

        public void attack_summon_utsuho_horiz_dive()
        {
            //
            //this.animationpoint = AnimeOrinDance(this.waittime % 24);
            this.animationpoint.X = AnimeUtsuhoCall_x(this.waittime).X;
            this.animationpoint.Y = AnimeUtsuhoCall_y(this.waittime).X;

            switch (this.waittime)
            {
                case 12:
                    //
                    this.counterTiming = false;

                    break;
                case 24:
                    //

                    var spawnPanels = this.RandomMultiPanel(1, this.union, false);
                    var spawnPanel = spawnPanels.Length == 0 ? null : new Point?(spawnPanels[0]);
                    if (spawnPanel != null)
                    {
                        this.sound.PlaySE(SoundEffect.dark);
                        //this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.FromArgb(0x80, 0x60, 0x40, 0x60), 15));

                        this.parent.attacks.Add(new OrinSummonUtsuhoHoriz(this.sound, this.parent, spawnPanel.Value.X, spawnPanel.Value.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, 2, this.version));


                    }

                    break;
                case 36:
                    this.roopneutral = 0;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (!this.atack)
                    {
                        this.speed = this.nspeed;
                        break;
                    }
                    break;
            }

        }

        public void attack_summon_utsuho_giga_flare()
        {
            //
            this.animationpoint.X = AnimeUtsuhoCall_x(this.waittime).X;
            this.animationpoint.Y = AnimeUtsuhoCall_y(this.waittime).X;

            switch (this.waittime)
            {
                case 12:
                    //
                    this.counterTiming = false;

                    break;
                case 24:
                    //

                    var spawnPanels = this.RandomMultiPanel(1, this.union, false);
                    var spawnPanel = spawnPanels.Length == 0 ? null : new Point?(spawnPanels[0]);
                    if (spawnPanel != null)
                    {
                        this.sound.PlaySE(SoundEffect.dark);
                        //this.parent.effects.Add(new FlashFead(this.sound, this.parent, Color.FromArgb(0x80, 0x60, 0x40, 0x60), 15));

                        this.parent.attacks.Add(new OrinSummonUtsuhoGigaFlare(this.sound, this.parent, spawnPanel.Value.X, spawnPanel.Value.Y, this.union, this.Power, 2, new Vector2(this.positionDirect.X, this.positionDirect.Y + 16f), ChipBase.ELEMENT.normal, 2, this.version));


                    }

                    break;
                case 36:
                    this.roopneutral = 0;
                    this.Motion = NaviBase.MOTION.neutral;
                    if (!this.atack)
                    {
                        this.speed = this.nspeed;
                        break;
                    }
                    break;
            }
        }
        // not usable v1

        public void attack_meteor_dance()
        {
            //
            if (!this.ready)
            {
                switch (this.waittime)
                {
                    case 1:
                        //
                        this.meteor_dance_target_helper();
                        this.animationpoint.X = 11;
                        this.animationpoint.Y = 4;
                        this.sound.PlaySE(SoundEffect.charge);
                        break;
                    case 12:
                        //
                        this.sound.PlaySE(SoundEffect.beamlong);
                        this.ShakeStart(3, 90);
                        break;
                    case 24:
                        //
                        this.counterTiming = false;
                        this.sound.PlaySE(SoundEffect.dark);

                        this.hadAura = aura_checker();
                        this.barierPower = 333;
                        this.barrierType = CharacterBase.BARRIER.PowerAura;
                        this.barierTime = -1;
                        this.animationpoint.X = 0;
                        this.animationpoint.Y = 0;
                        break;
                    case 48:
                        //
                        
                        this.ready = true;
                        this.waittime = 0;
                        this.rayDrop = 0;
                        this.rayDance = 0;
                        break;
                }
            }
            else
            {
                this.rayDance++;
                if (this.rayDance >= 24)
                {
                    this.rayDance = 0;
                }
                this.animationpoint = AnimeOrinDance(this.rayDance);
                //this.animationpoint = AnimeOrinDance((this.waittime*2) % 24);


                switch (this.waittime)
                {
                    case 2:
                        //
                        this.rayIndex = this.rayDrop % this.posis.Length;
                        this.parent.attacks.Add(new Dummy(this.sound, this.parent, this.posis[this.rayIndex].X, this.posis[this.rayIndex].Y, this.union, new Point(), 14, true));

                        

                        break;
                    case 10:
                        //
                        this.sound.PlaySE(SoundEffect.beam);
                        int s = 2;
                        //index = this.rayDrop % this.posis.Length;
                        this.parent.attacks.Add(new MeteorRay(this.sound, this.parent, this.posis[this.rayIndex].X, this.posis[this.rayIndex].Y, this.union, this.Power, s, ChipBase.ELEMENT.normal));

                        this.rayDrop++;
                        break;
                    case 16:
                        //
                        this.waittime = 0;
                        if (this.rayDrop >= 14)
                        {
                            this.roopneutral = 0;
                            this.Motion = NaviBase.MOTION.neutral;
                            this.speed = this.nspeed;

                            if (!this.hadAura)
                            {
                                this.barrierType = CharacterBase.BARRIER.None;
                            }
                        }
                        break;
                }
            }
            
        }
        // use sissorman slash rush code with meteor rays
        // dssp exclusive?

        // hadAura

        //

        

        public void meteor_dance_target_helper()
        {
            List<Point> source = new List<Point>();
            for (int x = 0; x < this.parent.panel.GetLength(0); ++x)
            {
                for (int y = 0; y < this.parent.panel.GetLength(1); ++y)
                {
                    if (this.parent.panel[x, y].color == this.UnionEnemy)
                        source.Add(new Point(x, y));
                }
            }
            this.posis = source.OrderBy<Point, Guid>(i => Guid.NewGuid()).ToArray<Point>();
        }

        public bool summon_exists()
        {
            bool exists = false;
            foreach (EnemyBase go in this.parent.enemys)
            {
                if (go is OrinMook1) exists = true;
                if (go is OrinMook2) exists = true;
            }

            return exists;
        }
        // why doesn't druidman have something like this

        public bool aura_checker()
        {
            if (this.barrierType != CharacterBase.BARRIER.None)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

