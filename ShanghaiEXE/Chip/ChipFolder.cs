using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;

namespace NSChip
{
    public class ChipFolder : AllBase
    {
        public bool inchip;
        public ChipBase chip;
        public int codeNo;

        public ChipBase ReturnChip(int key)
        {
            switch (key)
            {
                case 0:
                    return new DammyChip(this.sound);
                case 1:
                    return new Reygun(this.sound);
                case 2:
                    return new MegaReygun(this.sound);
                case 3:
                    return new GigaReygun(this.sound);
                case 4:
                    return new SeedCanon1(this.sound);
                case 5:
                    return new SeedCanon2(this.sound);
                case 6:
                    return new SeedCanon3(this.sound);
                case 7:
                    return new ChargeCanon1(this.sound);
                case 8:
                    return new ChargeCanon2(this.sound);
                case 9:
                    return new ChargeCanon3(this.sound);
                case 10:
                    return new MedousaEye(this.sound);
                case 11:
                    return new Hakkero1(this.sound);
                case 12:
                    return new Hakkero2(this.sound);
                case 13:
                    return new Hakkero3(this.sound);
                case 14:
                    return new FireArm1(this.sound);
                case 15:
                    return new FireArm2(this.sound);
                case 16:
                    return new FireArm3(this.sound);
                case 17:
                    return new ShotWave(this.sound);
                case 18:
                    return new ShootWave(this.sound);
                case 19:
                    return new GroundWave(this.sound);
                case 20:
                    return new GigantWave(this.sound);
                case 21:
                    return new PoisonShot(this.sound);
                case 22:
                    return new BreakShot(this.sound);
                case 23:
                    return new ColdShot(this.sound);
                case 24:
                    return new ColdAir1(this.sound);
                case 25:
                    return new ColdAir2(this.sound);
                case 26:
                    return new ColdAir3(this.sound);
                case 27:
                    return new ShellArmor1(this.sound);
                case 28:
                    return new ShellArmor2(this.sound);
                case 29:
                    return new ShellArmor3(this.sound);
                case 30:
                    return new BrocraLink1(this.sound);
                case 31:
                    return new BrocraLink2(this.sound);
                case 32:
                    return new BrocraLink3(this.sound);
                case 33:
                    return new ElekiChain1(this.sound);
                case 34:
                    return new ElekiChain2(this.sound);
                case 35:
                    return new ElekiChain3(this.sound);
                case 36:
                    return new TrapNet(this.sound);
                case 37:
                    return new FireNet(this.sound);
                case 38:
                    return new PoisonNet(this.sound);
                case 39:
                    return new ElekiNet(this.sound);
                case 40:
                    return new GraviBall1(this.sound);
                case 41:
                    return new GraviBall2(this.sound);
                case 42:
                    return new GraviBall3(this.sound);
                case 43:
                    return new DustBomb(this.sound);
                case 44:
                    return new MagicBomb(this.sound);
                case 45:
                    return new StarBomb(this.sound);
                case 46:
                    return new HeavyAnchor1(this.sound);
                case 47:
                    return new HeavyAnchor2(this.sound);
                case 48:
                    return new HeavyAnchor3(this.sound);
                case 49:
                    return new LjiOtama1(this.sound);
                case 50:
                    return new LjiOtama2(this.sound);
                case 51:
                    return new LjiOtama3(this.sound);
                case 52:
                    return new PowerOfEarth(this.sound);
                case 53:
                    return new BubbleBlust1(this.sound);
                case 54:
                    return new BubbleBlust2(this.sound);
                case 55:
                    return new BubbleBlust3(this.sound);
                case 56:
                    return new RebirthShield(this.sound);
                case 57:
                    return new LifeShield(this.sound);
                case 58:
                    return new ReflectShield(this.sound);
                case 59:
                    return new Knife(this.sound);
                case 60:
                    return new SilverKnife(this.sound);
                case 61:
                    return new HakurouBlade(this.sound);
                case 62:
                    return new Sword(this.sound);
                case 63:
                    return new HeatSword(this.sound);
                case 64:
                    return new LeafSword(this.sound);
                case 65:
                    return new IceSword(this.sound);
                case 66:
                    return new BraveSword(this.sound);
                case 67:
                    return new CrossSword(this.sound);
                case 68:
                    return new ThujigiriSword(this.sound);
                case 69:
                    return new ThujigiriCross(this.sound);
                case 70:
                    return new RoukanBlade(this.sound);
                case 71:
                    return new Lance(this.sound);
                case 72:
                    return new KnightLance(this.sound);
                case 73:
                    return new PaladinLance(this.sound);
                case 74:
                    return new DigDrill1(this.sound);
                case 75:
                    return new DigDrill2(this.sound);
                case 76:
                    return new DigDrill3(this.sound);
                case 77:
                    return new ElekiDrill(this.sound);
                case 78:
                    return new FlaeGun1(this.sound);
                case 79:
                    return new FlaeGun2(this.sound);
                case 80:
                    return new FlaeGun3(this.sound);
                case 81:
                    return new BronzeNapalm(this.sound);
                case 82:
                    return new MetalNapalm(this.sound);
                case 83:
                    return new MithrillNapalm(this.sound);
                case 84:
                    return new FuhathuNapalm(this.sound);
                case 85:
                    return new Storm(this.sound);
                case 86:
                    return new HellStorm(this.sound);
                case 87:
                    return new ElekiStorm(this.sound);
                case 88:
                    return new LeafStorm(this.sound);
                case 89:
                    return new AquaStorm(this.sound);
                case 90:
                    return new SandStorm(this.sound);
                case 91:
                    return new BioStorm(this.sound);
                case 92:
                    return new BackWind(this.sound);
                case 93:
                    return new PushWind(this.sound);
                case 94:
                    return new ChainGun1(this.sound);
                case 95:
                    return new ChainGun2(this.sound);
                case 96:
                    return new ChainGun3(this.sound);
                case 97:
                    return new DragnoBreath1(this.sound);
                case 98:
                    return new DragnoBreath2(this.sound);
                case 99:
                    return new DragnoBreath3(this.sound);
                case 100:
                    return new PanelShoot1(this.sound);
                case 101:
                    return new PanelShoot2(this.sound);
                case 102:
                    return new PanelShoot3(this.sound);
                case 103:
                    return new Tomahawk(this.sound);
                case 104:
                    return new MegaTomahawk(this.sound);
                case 105:
                    return new GigaTomahawk(this.sound);
                case 106:
                    return new DeathWiper1(this.sound);
                case 107:
                    return new DeathWiper2(this.sound);
                case 108:
                    return new DeathWiper3(this.sound);
                case 109:
                    return new PonpocoJizou1(this.sound);
                case 110:
                    return new PonpocoJizou2(this.sound);
                case 111:
                    return new PonpocoJizou3(this.sound);
                case 112:
                    return new Railgun1(this.sound);
                case 113:
                    return new Railgun2(this.sound);
                case 114:
                    return new Railgun3(this.sound);
                case 115:
                    return new KarehaWave1(this.sound);
                case 116:
                    return new KarehaWave2(this.sound);
                case 117:
                    return new KarehaWave3(this.sound);
                case 118:
                    return new SandHell1(this.sound);
                case 119:
                    return new SandHell2(this.sound);
                case 120:
                    return new SandHell3(this.sound);
                case 121:
                    return new ShotGun1(this.sound);
                case 122:
                    return new ShotGun2(this.sound);
                case 123:
                    return new ShotGun3(this.sound);
                case 124:
                    return new AuraSword1(this.sound);
                case 125:
                    return new AuraSword2(this.sound);
                case 126:
                    return new AuraSword3(this.sound);
                case sbyte.MaxValue:
                    return new ElekiFang1(this.sound);
                case 128:
                    return new ElekiFang2(this.sound);
                case 129:
                    return new ElekiFang3(this.sound);
                case 130:
                    return new MonkeyPole1(this.sound);
                case 131:
                    return new MonkeyPole2(this.sound);
                case 132:
                    return new MonkeyPole3(this.sound);
                case 133:
                    return new BioSpray1(this.sound);
                case 134:
                    return new BioSpray2(this.sound);
                case 135:
                    return new BioSpray3(this.sound);
                case 136:
                    return new BusterAnp(this.sound);
                case 137:
                    return new DanmakuValucun(this.sound);
                case 138:
                    return new ZSaber(this.sound);
                case 139:
                    return new TripleRod(this.sound);
                case 140:
                    return new ZeroKnuckle(this.sound);
                case 141:
                    return new WhiteCard(this.sound);
                case 142:
                    return new Cube(this.sound);
                case 143:
                    return new MetalCube(this.sound);
                case 144:
                    return new WhiteSuzuran(this.sound);
                case 145:
                    return new BlueSuzuran(this.sound);
                case 146:
                    return new Okatazuke(this.sound);
                case 147:
                    return new Barrier(this.sound);
                case 148:
                    return new HealBarrier(this.sound);
                case 149:
                    return new FloatBarrier(this.sound);
                case 150:
                    return new PowerAura(this.sound);
                case 151:
                    return new ElementsAura(this.sound);
                case 152:
                    return new MetalAura(this.sound);
                case 153:
                    return new BubbleLotion(this.sound);
                case 154:
                    return new MeltRaw(this.sound);
                case 155:
                    return new BlindReaf(this.sound);
                case 156:
                    return new GraviField(this.sound);
                case 157:
                    return new TimeStopper(this.sound);
                case 158:
                    return new Eriabash(this.sound);
                case 159:
                    return new EriaGuard(this.sound);
                case 160:
                    return new BurnerRoad(this.sound);
                case 161:
                    return new BurnerStage(this.sound);
                case 162:
                    return new IceRoad(this.sound);
                case 163:
                    return new IceStage(this.sound);
                case 164:
                    return new GrassRoad(this.sound);
                case 165:
                    return new GrassStage(this.sound);
                case 166:
                    return new ThunderRoad(this.sound);
                case 167:
                    return new ThunderStage(this.sound);
                case 168:
                    return new SandRoad(this.sound);
                case 169:
                    return new SandStage(this.sound);
                case 170:
                    return new PoisonRoad(this.sound);
                case 171:
                    return new PoisonStage(this.sound);
                case 172:
                    return new RefreRoad(this.sound);
                case 173:
                    return new RefreStage(this.sound);
                case 174:
                    return new Repair20(this.sound);
                case 175:
                    return new Repair50(this.sound);
                case 176:
                    return new Repair100(this.sound);
                case 177:
                    return new Repair150(this.sound);
                case 178:
                    return new Repair200(this.sound);
                case 179:
                    return new Repair300(this.sound);
                case 180:
                    return new Repair500(this.sound);
                case 181:
                    return new Resist(this.sound);
                case 182:
                    return new GhostBody(this.sound);
                case 183:
                    return new ShadowBody(this.sound);
                case 184:
                    return new SynchroBody(this.sound);
                case 185:
                    return new QuickCustom(this.sound);
                case 186:
                    return new SlowCustom(this.sound);
                case 187:
                    return new CustomMax(this.sound);
                case 188:
                    return new PowerPlus10(this.sound);
                case 189:
                    return new PowerPlus30(this.sound);
                case 190:
                    return new ParayzeCassette(this.sound);
                case 191:
                    return new MarisaV1(this.sound);
                case 192:
                    return new MarisaV2(this.sound);
                case 193:
                    return new MarisaV3(this.sound);
                case 194:
                    return new SakuyaV1(this.sound);
                case 195:
                    return new SakuyaV2(this.sound);
                case 196:
                    return new SakuyaV3(this.sound);
                case 197:
                    return new TankmanV1(this.sound);
                case 198:
                    return new TankmanV2(this.sound);
                case 199:
                    return new TankmanV3(this.sound);
                case 200:
                    return new SpannerManV1(this.sound);
                case 201:
                    return new SpannerManV2(this.sound);
                case 202:
                    return new SpannerManV3(this.sound);
                case 203:
                    return new DammyChip(this.sound);
                case 204:
                    return new DammyChip(this.sound);
                case 205:
                    return new DammyChip(this.sound);
                case 206:
                    return new HakutakuManV1(this.sound);
                case 207:
                    return new HakutakuManV2(this.sound);
                case 208:
                    return new HakutakuManV3(this.sound);
                case 209:
                    return new TortoiseManV1(this.sound);
                case 210:
                    return new TortoiseManV2(this.sound);
                case 211:
                    return new TortoiseManV3(this.sound);
                case 212:
                    return new BeatleManV1(this.sound);
                case 213:
                    return new BeatleManV2(this.sound);
                case 214:
                    return new BeatleManV3(this.sound);
                case 215:
                    return new YorihimeV1(this.sound);
                case 216:
                    return new YorihimeV2(this.sound);
                case 217:
                    return new YorihimeV3(this.sound);
                case 218:
                    return new CirnoV1(this.sound);
                case 219:
                    return new CirnoV2(this.sound);
                case 220:
                    return new CirnoV3(this.sound);
                case 221:
                    return new MedicineV1(this.sound);
                case 222:
                    return new MedicineV2(this.sound);
                case 223:
                    return new MedicineV3(this.sound);
                case 224:
                    return new IkuV1(this.sound);
                case 225:
                    return new IkuV2(this.sound);
                case 226:
                    return new IkuV3(this.sound);
                case 227:
                    return new PyroManV1(this.sound);
                case 228:
                    return new PyroManV2(this.sound);
                case 229:
                    return new PyroManV3(this.sound);
                case 230:
                    return new MrasaV1(this.sound);
                case 231:
                    return new MrasaV2(this.sound);
                case 232:
                    return new MrasaV3(this.sound);
                case 233:
                    return new ScissorManV1(this.sound);
                case 234:
                    return new ScissorManV2(this.sound);
                case 235:
                    return new ScissorManV3(this.sound);
                case 236:
                    return new ChenV1(this.sound);
                case 237:
                    return new ChenV2(this.sound);
                case 238:
                    return new ChenV3(this.sound);
                case 239:
                    return new RanV1(this.sound);
                case 240:
                    return new RanV2(this.sound);
                case 241:
                    return new RanV3(this.sound);
                case 242:
                    return new YoumuV1(this.sound);
                case 243:
                    return new YoumuV2(this.sound);
                case 244:
                    return new YoumuV3(this.sound);
                case 245:
                    return new DruidMnV1(this.sound);
                case 246:
                    return new DruidMnV2(this.sound);
                case 247:
                    return new DruidMnV3(this.sound);
                case 248:
                    return new DammyChip(this.sound);
                case 249:
                    return new DammyChip(this.sound);
                case 250:
                    return new DammyChip(this.sound);
                case 251:
                    return new DammyChip(this.sound);
                case 252:
                    return new UthuhoV1(this.sound);
                case 253:
                    return new UthuhoV2(this.sound);
                case 254:
                    return new UthuhoV3(this.sound);
                case 255:
                    return new DarkReygun(this.sound);
                case 256:
                    return new DarkSpark(this.sound);
                case 257:
                    return new DarkBreath(this.sound);
                case 258:
                    return new DarkWiper(this.sound);
                case 259:
                    return new DarkSand(this.sound);
                case 260:
                    return new DarkFang(this.sound);
                case 261:
                    return new DarkAnchor(this.sound);
                case 262:
                    return new DarkAutumn(this.sound);
                case 263:
                    return new DarkHurricane(this.sound);
                case 264:
                    return new DarkRepair(this.sound);
                case 265:
                    return new DarkAura(this.sound);
                case 266:
                    return new FlandreDS(this.sound);
                case 267:
                    return new UthuhoDS(this.sound);
                case 268:
                    return new YorihimeDS(this.sound);
                case 269:
                    return new MimaDS(this.sound);
                case 270:
                    return new RanDS(this.sound);
                case 271:
                    return new HiCanon(this.sound);
                case 272:
                    return new HiMegaCanon(this.sound);
                case 273:
                    return new MegaHalberd(this.sound);
                case 274:
                    return new GigaHalberd(this.sound);
                case 275:
                    return new BigTyphoon(this.sound);
                case 276:
                    return new FlashBurn(this.sound);
                case 277:
                    return new EXSprayGun(this.sound);
                case 278:
                    return new AuraSlash(this.sound);
                case 279:
                    return new EndlessSahara(this.sound);
                case 280:
                    return new MassDriver(this.sound);
                case 281:
                    return new ElementSword(this.sound);
                case 282:
                    return new PlantPrison(this.sound);
                case 283:
                    return new SwordOffGun(this.sound);
                case 284:
                    return new MasterSpark(this.sound);
                case 285:
                    return new SathujinDoll(this.sound);
                case 286:
                    return new KishinCanon(this.sound);
                case 287:
                    return new HyperSpanner(this.sound);
                case 288:
                    return new DisasterCrow(this.sound);
                case 289:
                    return new HellsHockey(this.sound);
                case 290:
                    return new TwinHeroines(this.sound);
                case 291:
                    return new FreezerSword(this.sound);
                case 292:
                    return new BioHazard(this.sound);
                case 293:
                    return new DrillBreaker(this.sound);
                case 294:
                    return new RainAnchor(this.sound);
                case 295:
                    return new MiraiEigouZan(this.sound);
                case 296:
                    return new DreamMeteo(this.sound);
                case 297:
                    return new BeastBreath(this.sound);
                case 298:
                    return new ProtonThunder(this.sound);
                case 299:
                    return new JusticeRey(this.sound);
                case 300:
                    return new InfiniteHands(this.sound);
                case 301:
                    return new FalBreazer(this.sound);
                case 302:
                    return new MasterStyle(this.sound);
                case 303:
                    return new DammyChip(this.sound);
                case 304:
                    return new DammyChip(this.sound);
                case 305:
                    return new DammyChip(this.sound);
                case 306:
                    return new DammyChip(this.sound);
                case 307:
                    return new DammyChip(this.sound);
                case 308:
                    return new DammyChip(this.sound);
                case 309:
                    return new DammyChip(this.sound);
                case 310:
                    return new VirusBall1(this.sound, key == 310);
                case 311:
                    return new VirusBall2(this.sound, key == 311);
                case 312:
                    return new VirusBall3(this.sound, key == 312);
                case 313:
                    return new DammyChip(this.sound);
                case 314:
                    return new DammyChip(this.sound);
                case 315:
                    return new DammyChip(this.sound);
                case 316:
                    return new DammyChip(this.sound);
                case 317:
                    return new DammyChip(this.sound);
                case 318:
                    return new DammyChip(this.sound);
                case 319:
                    return new DammyChip(this.sound);
                case 350:
                    return new ElekiChainX(this.sound);
                case 351:
                    return new GraviBallX(this.sound);
                case 352:
                    return new AuraSwordX(this.sound);
                case 353:
                    return new BubbleBlustX(this.sound);
                case 354:
                    return new PoisonShotX(this.sound);
                case 355:
                    return new ColdAirX(this.sound);
                case 356:
                    return new RailgunX(this.sound);
                case 357:
                    return new StormX(this.sound);
                case 358:
                    return new ShotGunX(this.sound);
                case 359:
                    return new ChainGunX(this.sound);
                case 360:
                    return new MetalNapalmX(this.sound);
                case 361:
                    return new BioSprayX(this.sound);
                case 362:
                    return new ThujigiriSwordX(this.sound);
                case 363:
                    return new TomahawkX(this.sound);
                case 364:
                    return new BrocraLinkX(this.sound);
                case 365:
                    return new RebirthShieldX(this.sound);
                case 366:
                    return new FireArmX(this.sound);
                case 367:
                    return new MonkeyPoleX(this.sound);
                case 368:
                    return new PonpocoJizouX(this.sound);
                case 369:
                    return new DigDrillX(this.sound);
                case 370:
                    return new OmegaSaber(this.sound);
                case 371:
                    return new SeedCanonX(this.sound);
                case 372:
                    return new ChargeCanonX(this.sound);
                case 373:
                    return new ShellArmorX(this.sound);
                case 374:
                    return new MagicBombX(this.sound);
                case 375:
                    return new LjiOtamaX(this.sound);
                case 376:
                    return new FlaeGunX(this.sound);
                case 377:
                    return new PanelShootX(this.sound);
                case 378:
                    return new DammyChip(this.sound);
                case 379:
                    return new DammyChip(this.sound);
                case 380:
                    return new TurtleHockey(this.sound);
                case 381:
                    return new DammyChip(this.sound);

                case 400:
                    return new MarisaX(this.sound);
                case 401:
                    return new CirnoX(this.sound);
                case 402:
                    return new SakuyaX(this.sound);
                case 403:
                    return new MedicineX(this.sound);
                case 404:
                    return new TankmanX(this.sound);
                case 405:
                    return new IkuX(this.sound);
                case 406:
                    return new SpannerManX(this.sound);
                case 407:
                    return new YorihimeX(this.sound);
                case 408:
                    return new TortoiseManX(this.sound);
                case 409:
                    return new HakutakuManX(this.sound);
                case 410:
                    return new PyroManX(this.sound);
                case 411:
                    return new BeatleManX(this.sound);
                case 412:
                    return new MrasaX(this.sound);
                case 413:
                    return new ScissorManX(this.sound);
                case 414:
                    return new ChenX(this.sound);
                case 415:
                    return new RanX(this.sound);
                case 416:
                    return new UthuhoV2(this.sound);
                case 417:
                    return new UthuhoV3(this.sound);
                case 418:
                    return new YoumuX(this.sound);
                case 419:
                    return new DruidMnX(this.sound);
                case 430:
                    return new Kikuri(this.sound);
                default:
                    return new DammyChip(this.sound);
            }
        }

        public ChipFolder(MyAudio s)
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
    }
}
