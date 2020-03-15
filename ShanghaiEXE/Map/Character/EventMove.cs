using NSShanghaiEXE.InputOutput.Audio;
using NSGame;

namespace NSMap.Character
{
    public class EventMove : AllBase
    {
        public EventMove.MOVE move;
        public int length;
        private MapCharacterBase character;
        public float flame;

        public EventMove(IAudioEngine s, EventMove.MOVE m, int l, MapCharacterBase c)
          : base(s)
        {
            this.move = m;
            this.length = this.move == EventMove.MOVE.jump ? 60 : l;
            this.character = c;
        }

        public void Move(float speed)
        {
            bool flag = true;
            switch (this.move)
            {
                case EventMove.MOVE.up:
                    if (!this.character.angleLock)
                        this.character.Angle = MapCharacterBase.ANGLE.UPRIGHT;
                    if (!this.character.playeHit || this.character.player || this.character.parent.loadflame > 0)
                    {
                        this.character.position.Y -= this.Speed(speed);
                        break;
                    }
                    flag = false;
                    break;
                case EventMove.MOVE.down:
                    if (!this.character.angleLock)
                        this.character.Angle = MapCharacterBase.ANGLE.DOWNLEFT;
                    if (!this.character.playeHit || this.character.player || this.character.parent.loadflame > 0)
                    {
                        this.character.position.Y += this.Speed(speed);
                        break;
                    }
                    flag = false;
                    break;
                case EventMove.MOVE.left:
                    if (!this.character.angleLock)
                        this.character.Angle = MapCharacterBase.ANGLE.UPLEFT;
                    if (!this.character.playeHit || this.character.player || this.character.parent.loadflame > 0)
                    {
                        this.character.position.X -= this.Speed(speed);
                        break;
                    }
                    flag = false;
                    break;
                case EventMove.MOVE.right:
                    if (!this.character.angleLock)
                        this.character.Angle = MapCharacterBase.ANGLE.DOWNRIGHT;
                    if (!this.character.playeHit || this.character.player || this.character.parent.loadflame > 0)
                    {
                        this.character.position.X += this.Speed(speed);
                        break;
                    }
                    flag = false;
                    break;
                case EventMove.MOVE.jump:
                    if (this.character.jumpingFlame < 2)
                    {
                        ++this.character.jumpingFlame;
                        this.character.jumpY -= 4;
                        break;
                    }
                    if (this.character.jumpingFlame < 4)
                    {
                        ++this.character.jumpingFlame;
                        this.character.jumpY += 4;
                        break;
                    }
                    this.flame = length;
                    this.character.jump = false;
                    this.character.jumpingFlame = 0;
                    this.character.jumpFlame = 0;
                    this.character.jumpY = 0;
                    break;
                case EventMove.MOVE.wait:
                    if (!this.character.floating)
                    {
                        this.character.CharaAnimeFlame = 0;
                        break;
                    }
                    break;
                case EventMove.MOVE.wayup:
                    this.character.Angle = MapCharacterBase.ANGLE.UPRIGHT;
                    break;
                case EventMove.MOVE.waydown:
                    this.character.Angle = MapCharacterBase.ANGLE.DOWNLEFT;
                    break;
                case EventMove.MOVE.wayleft:
                    this.character.Angle = MapCharacterBase.ANGLE.UPLEFT;
                    break;
                case EventMove.MOVE.wayright:
                    this.character.Angle = MapCharacterBase.ANGLE.DOWNRIGHT;
                    break;
                case EventMove.MOVE.wayfix:
                    this.character.angleLock = true;
                    break;
                case EventMove.MOVE.wayfixend:
                    this.character.angleLock = false;
                    break;
                case EventMove.MOVE.warpup:
                    if (!this.character.angleLock)
                        this.character.Angle = MapCharacterBase.ANGLE.UPRIGHT;
                    this.character.position.Y -= length;
                    break;
                case EventMove.MOVE.warpdown:
                    if (!this.character.angleLock)
                        this.character.Angle = MapCharacterBase.ANGLE.DOWNLEFT;
                    this.character.position.Y += length;
                    break;
                case EventMove.MOVE.warpleft:
                    if (!this.character.angleLock)
                        this.character.Angle = MapCharacterBase.ANGLE.UPLEFT;
                    this.character.position.X -= length;
                    break;
                case EventMove.MOVE.warpright:
                    if (!this.character.angleLock)
                        this.character.Angle = MapCharacterBase.ANGLE.DOWNRIGHT;
                    this.character.position.X += length;
                    break;
            }
            if (this.character.playeHit && speed != 0.0 && !this.character.floating && this.character.parent.loadflame <= 0)
            {
                this.character.StartEvent();
                this.character.CharaAnimeFlame = 0;
            }
            else
            {
                switch (this.move)
                {
                    case EventMove.MOVE.up:
                    case EventMove.MOVE.down:
                    case EventMove.MOVE.left:
                    case EventMove.MOVE.right:
                        if (speed == 0.0)
                            speed = 3f;
                        if (!this.character.floating)
                        {
                            int speed1 = (int)(8.0 - speed);
                            if (this.character is Player && ((Player)this.character).run)
                                speed1 *= 2;
                            this.FlameControl(speed1);
                            if (this.moveflame)
                                ++this.character.CharaAnimeFlame;
                        }
                        if (flag)
                        {
                            this.flame += this.Speed(speed);
                            break;
                        }
                        break;
                    case EventMove.MOVE.warpup:
                    case EventMove.MOVE.warpdown:
                    case EventMove.MOVE.warpleft:
                    case EventMove.MOVE.warpright:
                        this.flame = length;
                        break;
                    default:
                        this.FlameControl(1);
                        if (this.moveflame)
                        {
                            ++this.flame;
                            break;
                        }
                        break;
                }
            }
        }

        public float Speed(float speed)
        {
            if (speed == 0.0)
                return 0.0f;
            return speed / 8f;
        }

        public EventMove Clone(MapCharacterBase c)
        {
            return new EventMove(this.sound, this.move, this.length, c);
        }

        public void EventSet(MapCharacterBase c)
        {
            this.character = c;
        }

        public bool MoveEnd()
        {
            bool flag = flame >= (double)this.length;
            if (flag)
                this.flame = 0.0f;
            return flag;
        }

        public enum MOVE
        {
            up,
            down,
            left,
            right,
            jump,
            wait,
            wayup,
            waydown,
            wayleft,
            wayright,
            wayfix,
            wayfixend,
            warpup,
            warpdown,
            warpleft,
            warpright,
        }
    }
}
