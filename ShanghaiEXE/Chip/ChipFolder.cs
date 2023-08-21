using NSShanghaiEXE.InputOutput.Audio;
using NSGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSChip
{
    public class ChipFolder : AllBase
    {
        public static Dictionary<int, Func<IAudioEngine, ChipBase>> Chips;

        static ChipFolder()
        {
            Chips = new Dictionary<int, Func<IAudioEngine, ChipBase>>();
            Chips[1] = (sound) => new Reygun(sound);
            Chips[2] = (sound) => new MegaReygun(sound);
            Chips[3] = (sound) => new GigaReygun(sound);
            Chips[4] = (sound) => new SeedCanon1(sound);
            Chips[5] = (sound) => new SeedCanon2(sound);
            Chips[6] = (sound) => new SeedCanon3(sound);
            Chips[7] = (sound) => new ChargeCanon1(sound);
            Chips[8] = (sound) => new ChargeCanon2(sound);
            Chips[9] = (sound) => new ChargeCanon3(sound);
            Chips[10] = (sound) => new MedousaEye(sound);
            Chips[11] = (sound) => new Hakkero1(sound);
            Chips[12] = (sound) => new Hakkero2(sound);
            Chips[13] = (sound) => new Hakkero3(sound);
            Chips[14] = (sound) => new FireArm1(sound);
            Chips[15] = (sound) => new FireArm2(sound);
            Chips[16] = (sound) => new FireArm3(sound);
            Chips[17] = (sound) => new ShotWave(sound);
            Chips[18] = (sound) => new ShootWave(sound);
            Chips[19] = (sound) => new GroundWave(sound);
            Chips[20] = (sound) => new GigantWave(sound);
            Chips[21] = (sound) => new PoisonShot(sound);
            Chips[22] = (sound) => new BreakShot(sound);
            Chips[23] = (sound) => new ColdShot(sound);
            Chips[24] = (sound) => new ColdAir1(sound);
            Chips[25] = (sound) => new ColdAir2(sound);
            Chips[26] = (sound) => new ColdAir3(sound);
            Chips[27] = (sound) => new ShellArmor1(sound);
            Chips[28] = (sound) => new ShellArmor2(sound);
            Chips[29] = (sound) => new ShellArmor3(sound);
            Chips[30] = (sound) => new BrocraLink1(sound);
            Chips[31] = (sound) => new BrocraLink2(sound);
            Chips[32] = (sound) => new BrocraLink3(sound);
            Chips[33] = (sound) => new ElekiChain1(sound);
            Chips[34] = (sound) => new ElekiChain2(sound);
            Chips[35] = (sound) => new ElekiChain3(sound);
            Chips[36] = (sound) => new TrapNet(sound);
            Chips[37] = (sound) => new FireNet(sound);
            Chips[38] = (sound) => new PoisonNet(sound);
            Chips[39] = (sound) => new ElekiNet(sound);
            Chips[40] = (sound) => new GraviBall1(sound);
            Chips[41] = (sound) => new GraviBall2(sound);
            Chips[42] = (sound) => new GraviBall3(sound);
            Chips[43] = (sound) => new DustBomb(sound);
            Chips[44] = (sound) => new MagicBomb(sound);
            Chips[45] = (sound) => new StarBomb(sound);
            Chips[46] = (sound) => new HeavyAnchor1(sound);
            Chips[47] = (sound) => new HeavyAnchor2(sound);
            Chips[48] = (sound) => new HeavyAnchor3(sound);
            Chips[49] = (sound) => new LjiOtama1(sound);
            Chips[50] = (sound) => new LjiOtama2(sound);
            Chips[51] = (sound) => new LjiOtama3(sound);
            Chips[52] = (sound) => new PowerOfEarth(sound);
            Chips[53] = (sound) => new BubbleBlust1(sound);
            Chips[54] = (sound) => new BubbleBlust2(sound);
            Chips[55] = (sound) => new BubbleBlust3(sound);
            Chips[56] = (sound) => new RebirthShield(sound);
            Chips[57] = (sound) => new LifeShield(sound);
            Chips[58] = (sound) => new ReflectShield(sound);
            Chips[59] = (sound) => new Knife(sound);
            Chips[60] = (sound) => new SilverKnife(sound);
            Chips[61] = (sound) => new HakurouBlade(sound);
            Chips[62] = (sound) => new Sword(sound);
            Chips[63] = (sound) => new HeatSword(sound);
            Chips[64] = (sound) => new LeafSword(sound);
            Chips[65] = (sound) => new IceSword(sound);
            Chips[66] = (sound) => new BraveSword(sound);
            Chips[67] = (sound) => new CrossSword(sound);
            Chips[68] = (sound) => new ThujigiriSword(sound);
            Chips[69] = (sound) => new ThujigiriCross(sound);
            Chips[70] = (sound) => new RoukanBlade(sound);
            Chips[71] = (sound) => new Lance(sound);
            Chips[72] = (sound) => new KnightLance(sound);
            Chips[73] = (sound) => new PaladinLance(sound);
            Chips[74] = (sound) => new DigDrill1(sound);
            Chips[75] = (sound) => new DigDrill2(sound);
            Chips[76] = (sound) => new DigDrill3(sound);
            Chips[77] = (sound) => new ElekiDrill(sound);
            Chips[78] = (sound) => new FlaeGun1(sound);
            Chips[79] = (sound) => new FlaeGun2(sound);
            Chips[80] = (sound) => new FlaeGun3(sound);
            Chips[81] = (sound) => new BronzeNapalm(sound);
            Chips[82] = (sound) => new MetalNapalm(sound);
            Chips[83] = (sound) => new MithrillNapalm(sound);
            Chips[84] = (sound) => new FuhathuNapalm(sound);
            Chips[85] = (sound) => new Storm(sound);
            Chips[86] = (sound) => new HellStorm(sound);
            Chips[87] = (sound) => new ElekiStorm(sound);
            Chips[88] = (sound) => new LeafStorm(sound);
            Chips[89] = (sound) => new AquaStorm(sound);
            Chips[90] = (sound) => new SandStorm(sound);
            Chips[91] = (sound) => new BioStorm(sound);
            Chips[92] = (sound) => new BackWind(sound);
            Chips[93] = (sound) => new PushWind(sound);
            Chips[94] = (sound) => new ChainGun1(sound);
            Chips[95] = (sound) => new ChainGun2(sound);
            Chips[96] = (sound) => new ChainGun3(sound);
            Chips[97] = (sound) => new DragnoBreath1(sound);
            Chips[98] = (sound) => new DragnoBreath2(sound);
            Chips[99] = (sound) => new DragnoBreath3(sound);
            Chips[100] = (sound) => new PanelShoot1(sound);
            Chips[101] = (sound) => new PanelShoot2(sound);
            Chips[102] = (sound) => new PanelShoot3(sound);
            Chips[103] = (sound) => new Tomahawk(sound);
            Chips[104] = (sound) => new MegaTomahawk(sound);
            Chips[105] = (sound) => new GigaTomahawk(sound);
            Chips[106] = (sound) => new DeathWiper1(sound);
            Chips[107] = (sound) => new DeathWiper2(sound);
            Chips[108] = (sound) => new DeathWiper3(sound);
            Chips[109] = (sound) => new PonpocoJizou1(sound);
            Chips[110] = (sound) => new PonpocoJizou2(sound);
            Chips[111] = (sound) => new PonpocoJizou3(sound);
            Chips[112] = (sound) => new Railgun1(sound);
            Chips[113] = (sound) => new Railgun2(sound);
            Chips[114] = (sound) => new Railgun3(sound);
            Chips[115] = (sound) => new KarehaWave1(sound);
            Chips[116] = (sound) => new KarehaWave2(sound);
            Chips[117] = (sound) => new KarehaWave3(sound);
            Chips[118] = (sound) => new SandHell1(sound);
            Chips[119] = (sound) => new SandHell2(sound);
            Chips[120] = (sound) => new SandHell3(sound);
            Chips[121] = (sound) => new ShotGun1(sound);
            Chips[122] = (sound) => new ShotGun2(sound);
            Chips[123] = (sound) => new ShotGun3(sound);
            Chips[124] = (sound) => new AuraSword1(sound);
            Chips[125] = (sound) => new AuraSword2(sound);
            Chips[126] = (sound) => new AuraSword3(sound);
            Chips[127] = (sound) => new ElekiFang1(sound);
            Chips[128] = (sound) => new ElekiFang2(sound);
            Chips[129] = (sound) => new ElekiFang3(sound);
            Chips[130] = (sound) => new MonkeyPole1(sound);
            Chips[131] = (sound) => new MonkeyPole2(sound);
            Chips[132] = (sound) => new MonkeyPole3(sound);
            Chips[133] = (sound) => new BioSpray1(sound);
            Chips[134] = (sound) => new BioSpray2(sound);
            Chips[135] = (sound) => new BioSpray3(sound);
            Chips[136] = (sound) => new BusterAnp(sound);
            Chips[137] = (sound) => new DanmakuValucun(sound);
            Chips[138] = (sound) => new ZSaber(sound);
            Chips[139] = (sound) => new TripleRod(sound);
            Chips[140] = (sound) => new ZeroKnuckle(sound);
            Chips[141] = (sound) => new WhiteCard(sound);
            Chips[142] = (sound) => new Cube(sound);
            Chips[143] = (sound) => new MetalCube(sound);
            Chips[144] = (sound) => new WhiteSuzuran(sound);
            Chips[145] = (sound) => new BlueSuzuran(sound);
            Chips[146] = (sound) => new Okatazuke(sound);
            Chips[147] = (sound) => new Barrier(sound);
            Chips[148] = (sound) => new HealBarrier(sound);
            Chips[149] = (sound) => new FloatBarrier(sound);
            Chips[150] = (sound) => new PowerAura(sound);
            Chips[151] = (sound) => new ElementsAura(sound);
            Chips[152] = (sound) => new MetalAura(sound);
            Chips[153] = (sound) => new BubbleLotion(sound);
            Chips[154] = (sound) => new MeltRaw(sound);
            Chips[155] = (sound) => new BlindReaf(sound);
            Chips[156] = (sound) => new GraviField(sound);
            Chips[157] = (sound) => new TimeStopper(sound);
            Chips[158] = (sound) => new Eriabash(sound);
            Chips[159] = (sound) => new EriaGuard(sound);
            Chips[160] = (sound) => new BurnerRoad(sound);
            Chips[161] = (sound) => new BurnerStage(sound);
            Chips[162] = (sound) => new IceRoad(sound);
            Chips[163] = (sound) => new IceStage(sound);
            Chips[164] = (sound) => new GrassRoad(sound);
            Chips[165] = (sound) => new GrassStage(sound);
            Chips[166] = (sound) => new ThunderRoad(sound);
            Chips[167] = (sound) => new ThunderStage(sound);
            Chips[168] = (sound) => new SandRoad(sound);
            Chips[169] = (sound) => new SandStage(sound);
            Chips[170] = (sound) => new PoisonRoad(sound);
            Chips[171] = (sound) => new PoisonStage(sound);
            Chips[172] = (sound) => new RefreRoad(sound);
            Chips[173] = (sound) => new RefreStage(sound);
            Chips[174] = (sound) => new Repair20(sound);
            Chips[175] = (sound) => new Repair50(sound);
            Chips[176] = (sound) => new Repair100(sound);
            Chips[177] = (sound) => new Repair150(sound);
            Chips[178] = (sound) => new Repair200(sound);
            Chips[179] = (sound) => new Repair300(sound);
            Chips[180] = (sound) => new Repair500(sound);
            Chips[181] = (sound) => new Resist(sound);
            Chips[182] = (sound) => new GhostBody(sound);
            Chips[183] = (sound) => new ShadowBody(sound);
            Chips[184] = (sound) => new SynchroBody(sound);
            Chips[185] = (sound) => new QuickCustom(sound);
            Chips[186] = (sound) => new SlowCustom(sound);
            Chips[187] = (sound) => new CustomMax(sound);
            Chips[188] = (sound) => new PowerPlus10(sound);
            Chips[189] = (sound) => new PowerPlus30(sound);
            Chips[190] = (sound) => new ParayzeCassette(sound);
            Chips[191] = (sound) => new MarisaV1(sound);
            Chips[192] = (sound) => new MarisaV2(sound);
            Chips[193] = (sound) => new MarisaV3(sound);
            Chips[194] = (sound) => new SakuyaV1(sound);
            Chips[195] = (sound) => new SakuyaV2(sound);
            Chips[196] = (sound) => new SakuyaV3(sound);
            Chips[197] = (sound) => new TankmanV1(sound);
            Chips[198] = (sound) => new TankmanV2(sound);
            Chips[199] = (sound) => new TankmanV3(sound);
            Chips[200] = (sound) => new SpannerManV1(sound);
            Chips[201] = (sound) => new SpannerManV2(sound);
            Chips[202] = (sound) => new SpannerManV3(sound);
            /* 203 - 205 */
            Chips[203] = (sound) => new FlandreV1(sound);
            Chips[204] = (sound) => new FlandreV2(sound);
            Chips[205] = (sound) => new FlandreV3(sound);

            Chips[206] = (sound) => new HakutakuManV1(sound);
            Chips[207] = (sound) => new HakutakuManV2(sound);
            Chips[208] = (sound) => new HakutakuManV3(sound);
            Chips[209] = (sound) => new TortoiseManV1(sound);
            Chips[210] = (sound) => new TortoiseManV2(sound);
            Chips[211] = (sound) => new TortoiseManV3(sound);
            Chips[212] = (sound) => new BeatleManV1(sound);
            Chips[213] = (sound) => new BeatleManV2(sound);
            Chips[214] = (sound) => new BeatleManV3(sound);
            Chips[215] = (sound) => new YorihimeV1(sound);
            Chips[216] = (sound) => new YorihimeV2(sound);
            Chips[217] = (sound) => new YorihimeV3(sound);
            Chips[218] = (sound) => new CirnoV1(sound);
            Chips[219] = (sound) => new CirnoV2(sound);
            Chips[220] = (sound) => new CirnoV3(sound);
            Chips[221] = (sound) => new MedicineV1(sound);
            Chips[222] = (sound) => new MedicineV2(sound);
            Chips[223] = (sound) => new MedicineV3(sound);
            Chips[224] = (sound) => new IkuV1(sound);
            Chips[225] = (sound) => new IkuV2(sound);
            Chips[226] = (sound) => new IkuV3(sound);
            Chips[227] = (sound) => new PyroManV1(sound);
            Chips[228] = (sound) => new PyroManV2(sound);
            Chips[229] = (sound) => new PyroManV3(sound);
            Chips[230] = (sound) => new MrasaV1(sound);
            Chips[231] = (sound) => new MrasaV2(sound);
            Chips[232] = (sound) => new MrasaV3(sound);
            Chips[233] = (sound) => new ScissorManV1(sound);
            Chips[234] = (sound) => new ScissorManV2(sound);
            Chips[235] = (sound) => new ScissorManV3(sound);
            Chips[236] = (sound) => new ChenV1(sound);
            Chips[237] = (sound) => new ChenV2(sound);
            Chips[238] = (sound) => new ChenV3(sound);
            Chips[239] = (sound) => new RanV1(sound);
            Chips[240] = (sound) => new RanV2(sound);
            Chips[241] = (sound) => new RanV3(sound);
            Chips[242] = (sound) => new YoumuV1(sound);
            Chips[243] = (sound) => new YoumuV2(sound);
            Chips[244] = (sound) => new YoumuV3(sound);
            Chips[245] = (sound) => new DruidMnV1(sound);
            Chips[246] = (sound) => new DruidMnV2(sound);
            Chips[247] = (sound) => new DruidMnV3(sound);
            /* 248 - 251 */
            Chips[252] = (sound) => new UthuhoV1(sound);
            Chips[253] = (sound) => new UthuhoV2(sound);
            Chips[254] = (sound) => new UthuhoV3(sound);

            Chips[255] = (sound) => new DarkReygun(sound);
            Chips[256] = (sound) => new DarkSpark(sound);
            Chips[257] = (sound) => new DarkBreath(sound);
            Chips[258] = (sound) => new DarkWiper(sound);
            Chips[259] = (sound) => new DarkSand(sound);
            Chips[260] = (sound) => new DarkFang(sound);
            Chips[261] = (sound) => new DarkAnchor(sound);
            Chips[262] = (sound) => new DarkAutumn(sound);
            Chips[263] = (sound) => new DarkHurricane(sound);
            Chips[264] = (sound) => new DarkRepair(sound);
            Chips[265] = (sound) => new DarkAura(sound);
            Chips[266] = (sound) => new DarkFlae(sound);
            // FlandreDS: 266 -> 313
            Chips[267] = (sound) => new UthuhoDS(sound);
            Chips[268] = (sound) => new YorihimeDS(sound);
            Chips[269] = (sound) => new MimaDS(sound);
            Chips[270] = (sound) => new RanDS(sound);
            // Display: Kikuri ??
            // Display: FlandreDS ??

            Chips[271] = (sound) => new HiCanon(sound);
            Chips[272] = (sound) => new HiMegaCanon(sound);
            Chips[273] = (sound) => new MegaHalberd(sound);
            Chips[274] = (sound) => new GigaHalberd(sound);
            Chips[275] = (sound) => new BigTyphoon(sound);
            Chips[276] = (sound) => new FlashBurn(sound);
            Chips[277] = (sound) => new EXSprayGun(sound);
            Chips[278] = (sound) => new AuraSlash(sound);
            Chips[279] = (sound) => new EndlessSahara(sound);
            Chips[280] = (sound) => new MassDriver(sound);
            Chips[281] = (sound) => new ElementSword(sound);
            Chips[282] = (sound) => new PlantPrison(sound);
            Chips[283] = (sound) => new SwordOffGun(sound);
            Chips[284] = (sound) => new MasterSpark(sound);
            Chips[285] = (sound) => new SathujinDoll(sound);
            Chips[286] = (sound) => new KishinCanon(sound);
            Chips[287] = (sound) => new HyperSpanner(sound);
            Chips[288] = (sound) => new DisasterCrow(sound);
            Chips[289] = (sound) => new HellsHockey(sound);
            Chips[290] = (sound) => new TwinHeroines(sound);
            Chips[291] = (sound) => new FreezerSword(sound);
            Chips[292] = (sound) => new BioHazard(sound);
            Chips[293] = (sound) => new DrillBreaker(sound);
            Chips[294] = (sound) => new RainAnchor(sound);
            Chips[295] = (sound) => new MiraiEigouZan(sound);
            Chips[296] = (sound) => new DreamMeteo(sound);
            Chips[297] = (sound) => new BeastBreath(sound);
            Chips[298] = (sound) => new ProtonThunder(sound);
            Chips[299] = (sound) => new JusticeRey(sound);
            Chips[300] = (sound) => new InfiniteHands(sound);
            Chips[301] = (sound) => new FalBreazer(sound);
            Chips[302] = (sound) => new MasterStyle(sound);
            /* 303 - 309 */
            Chips[310] = (sound) => new VirusBall1(sound, true);
            Chips[311] = (sound) => new VirusBall2(sound, true);
            Chips[312] = (sound) => new VirusBall3(sound, true);

            Chips[313] = (sound) => new FlandreDS(sound);
            /* 313 - 349 */
            Chips[350] = (sound) => new ElekiChainX(sound);
            Chips[351] = (sound) => new GraviBallX(sound);
            Chips[352] = (sound) => new AuraSwordX(sound);
            Chips[353] = (sound) => new BubbleBlustX(sound);
            Chips[354] = (sound) => new PoisonShotX(sound);
            Chips[355] = (sound) => new ColdAirX(sound);
            Chips[356] = (sound) => new RailgunX(sound);
            Chips[357] = (sound) => new StormX(sound);
            Chips[358] = (sound) => new ShotGunX(sound);
            Chips[359] = (sound) => new ChainGunX(sound);
            Chips[360] = (sound) => new MetalNapalmX(sound);
            Chips[361] = (sound) => new BioSprayX(sound);
            Chips[362] = (sound) => new ThujigiriSwordX(sound);
            Chips[363] = (sound) => new TomahawkX(sound);
            Chips[364] = (sound) => new BrocraLinkX(sound);
            Chips[365] = (sound) => new RebirthShieldX(sound);
            Chips[366] = (sound) => new FireArmX(sound);
            Chips[367] = (sound) => new MonkeyPoleX(sound);
            Chips[368] = (sound) => new PonpocoJizouX(sound);
            Chips[369] = (sound) => new DigDrillX(sound);
            Chips[370] = (sound) => new OmegaSaber(sound);
            Chips[371] = (sound) => new SeedCanonX(sound);
            Chips[372] = (sound) => new ChargeCanonX(sound);
            Chips[373] = (sound) => new ShellArmorX(sound);
            Chips[374] = (sound) => new MagicBombX(sound);
            Chips[375] = (sound) => new LjiOtamaX(sound);
            Chips[376] = (sound) => new FlaeGunX(sound);
            Chips[377] = (sound) => new PanelShootX(sound);
            /* 378 - 379 */
            Chips[380] = (sound) => new TurtleHockey(sound);
            /* 381 - 399 */
            Chips[400] = (sound) => new MarisaX(sound);
            Chips[401] = (sound) => new CirnoX(sound);
            Chips[402] = (sound) => new SakuyaX(sound);
            Chips[403] = (sound) => new MedicineX(sound);
            Chips[404] = (sound) => new TankmanX(sound);
            Chips[405] = (sound) => new IkuX(sound);
            Chips[406] = (sound) => new SpannerManX(sound);
            Chips[407] = (sound) => new YorihimeX(sound);
            Chips[408] = (sound) => new TortoiseManX(sound);
            Chips[409] = (sound) => new HakutakuManX(sound);
            Chips[410] = (sound) => new PyroManX(sound);
            Chips[411] = (sound) => new BeatleManX(sound);
            Chips[412] = (sound) => new MrasaX(sound);
            Chips[413] = (sound) => new ScissorManX(sound);
            Chips[414] = (sound) => new ChenX(sound);
            Chips[415] = (sound) => new RanX(sound);
            /* 416 - 417 */
            Chips[418] = (sound) => new YoumuX(sound);
            Chips[419] = (sound) => new DruidMnX(sound);
            Chips[430] = (sound) => new Kikuri(sound);
        }

        public static IList<ProgramAdvanceEntry> ProgramAdvances = new[]
        {
            new ProgramAdvanceEntry { Chips = new[] { 1, 1, 1 }, ProgramAdvance = 271 },
            new ProgramAdvanceEntry { Chips = new[] { 2, 2, 2 }, ProgramAdvance = 272 },
            new ProgramAdvanceEntry { Chips = new[] { 59, 62, 71 }, ProgramAdvance = 273 },
            new ProgramAdvanceEntry { Chips = new[] { 60, 67, 72 }, ProgramAdvance = 274 },
            new ProgramAdvanceEntry { Chips = new[] { 92, 93, 25 }, ProgramAdvance = 275 },
            new ProgramAdvanceEntry { Chips = new[] { 34, 87, 190 }, ProgramAdvance = 276 },
            new ProgramAdvanceEntry { Chips = new[] { 21, 171, 134 }, ProgramAdvance = 277 },
            new ProgramAdvanceEntry { Chips = new[] { 147, 150, 125 }, ProgramAdvance = 278 },
            new ProgramAdvanceEntry { Chips = new[] { 90, 41, 119 }, ProgramAdvance = 279 },
            new ProgramAdvanceEntry { Chips = new[] { 142, 143, 113 }, ProgramAdvance = 280 },
            new ProgramAdvanceEntry { Chips = new[] { 63, 64, 65 }, ProgramAdvance = 281 },
            new ProgramAdvanceEntry { Chips = new[] { 131, 116, 155 }, ProgramAdvance = 282 },
            new ProgramAdvanceEntry { Chips = new[] { 95, 8, 122 }, ProgramAdvance = 283 },
            new ProgramAdvanceEntry { Chips = new[] { 12, 44, 192 }, ProgramAdvance = 284 },
            new ProgramAdvanceEntry { Chips = new[] { 60, 157, 195 }, ProgramAdvance = 285 },
            new ProgramAdvanceEntry { Chips = new[] { 5, 8, 198 }, ProgramAdvance = 286 },
            new ProgramAdvanceEntry { Chips = new[] { 104, 82, 201 }, ProgramAdvance = 287 },
            new ProgramAdvanceEntry { Chips = new[] { 64, 116, 207 }, ProgramAdvance = 288 },
            new ProgramAdvanceEntry { Chips = new[] { 28, 57, 210 }, ProgramAdvance = 289 },
            new ProgramAdvanceEntry { Chips = new[] { 66, 72, 216 }, ProgramAdvance = 290 },
            new ProgramAdvanceEntry { Chips = new[] { 65, 163, 219 }, ProgramAdvance = 291 },
            new ProgramAdvanceEntry { Chips = new[] { 144, 91, 222 }, ProgramAdvance = 292 },
            new ProgramAdvanceEntry { Chips = new[] { 77, 128, 225 }, ProgramAdvance = 293 },
            new ProgramAdvanceEntry { Chips = new[] { 47, 54, 231 }, ProgramAdvance = 294 },
            new ProgramAdvanceEntry { Chips = new[] { 70, 61, 243 }, ProgramAdvance = 295 },
            new ProgramAdvanceEntry { Chips = new[] { 107, 12, 265 }, ProgramAdvance = 296 },
            new ProgramAdvanceEntry { Chips = new[] { 18, 75, 257 }, ProgramAdvance = 297 },
            new ProgramAdvanceEntry { Chips = new[] { 137, 79, 260 }, ProgramAdvance = 298 },
            new ProgramAdvanceEntry { Chips = new[] { 140, 22, 256 }, ProgramAdvance = 299 },
            new ProgramAdvanceEntry { Chips = new[] { 183, 101, 264 }, ProgramAdvance = 300 },
            new ProgramAdvanceEntry { Chips = new[] { 86, 149, 263 }, ProgramAdvance = 301 },
            new ProgramAdvanceEntry { Chips = new[] { 52, 151, 141 }, ProgramAdvance = 302 }
        };

        public bool inchip;
        public ChipBase chip;
        public int codeNo;

        public static ProgramAdvanceEntry ProgramAdvanceCheck(IList<ChipS> chipStart)
        {
            foreach (var pa in ProgramAdvances)
            {
                if (chipStart.Count < pa.Chips.Count || chipStart.First().number != pa.Chips.First())
                {
                    continue;
                }

                var isPa = false;
                if (pa.IsAlphabetical)
                {
                    var chipCodes = chipStart.Take(pa.Chips.Count).Select(c => c.Code).ToArray();
                    var validCodes = Enumerable.Range(0, 4).Select(i => new ChipS(pa.Chips.First(), i).Code).ToArray();
                    isPa = chipCodes.Count(c => c == (int)CODE.asterisk) <= 1
                        && chipCodes.Aggregate((previous, current) =>
                        {
                            if (previous == -1 || current == (int)CODE.A || previous == (int)CODE.Z)
                            {
                                return -1;
                            }

                            var effectivePrevious = previous == (int)CODE.asterisk ? current - 1 : previous;
                            var effectiveCurrent = current == (int)CODE.asterisk ? previous + 1 : current;
                            if (!validCodes.Contains(effectivePrevious) || !validCodes.Contains(effectiveCurrent))
                            {
                                return -1;
                            }

                            return effectiveCurrent == effectivePrevious + 1 ? effectiveCurrent : -1;
                        }) != -1;
                }
                else
                {
                    isPa = chipStart.Take(pa.Chips.Count).Select(c => c.number).SequenceEqual(pa.Chips);
                }

                if (isPa)
                {
                    return pa;
                }
            }
            
            return null;
        }

        public ChipBase ReturnChip(int key)
        {
            if (Chips.ContainsKey(key))
            {
                return Chips[key].Invoke(this.sound);
            }
            else
            {
                return new DammyChip(this.sound);
            }
        }

        public ChipFolder(IAudioEngine s)
          : base(s)
        {
            this.inchip = false;
            this.chip = new DammyChip(this.sound);
        }

        public void SettingChip(int key)
        {
            this.chip = this.ReturnChip(key);
            this.inchip = true;
        }

        public enum CODE
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z,
            asterisk,
            none,
        }

        public class ProgramAdvanceEntry
        {
            public IList<int> Chips { get; set; }
            public int ProgramAdvance { get; set; }

            public bool IsAlphabetical => this.Chips.Distinct().Count() == 1;
        }
    }
}
