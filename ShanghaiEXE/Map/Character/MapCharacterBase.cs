using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using Common.Vectors;
using System;
using System.Drawing;

namespace NSMap.Character
{
    public class MapCharacterBase : AllBase
    {
        public int walkanime = 6;
        public EventMove[] moveOrder = new EventMove[0];
        protected MapCharacterBase.ANGLE angle;
        public bool moving;
        protected float walkSpeed;
        public bool jump;
        public int jumpY;
        public int jumpFlame;
        public int jumpingFlame;
        public bool NoPrint;
        protected int animespeed;
        protected byte animeflame;
        public bool angleLock;
        public Vector2 moveEndPosition;
        public int movingOrder;
        public Vector3 position;
        public Vector2 positionQ;
        public SceneMap parent;
        public MapField field;
        public bool playeHit;
        public int rendType;
        public int floor;
        public bool player;
        public int index;
        public MapCharacterBase.ANGLE EndAngle;
        public bool floating;
        public bool noShadow;

        public virtual MapCharacterBase.ANGLE Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                this.angle = value;
                if (this.angle < MapCharacterBase.ANGLE.DOWN)
                    this.angle += 8;
                if (this.angle <= MapCharacterBase.ANGLE.DOWNLEFT)
                    return;
                this.angle = MapCharacterBase.ANGLE.DOWN;
            }
        }

        public float Speed
        {
            get
            {
                return this.walkSpeed;
            }
        }

        public virtual byte CharaAnimeFlame
        {
            set
            {
                this.animeflame = value;
                if (animeflame <= this.walkanime)
                    return;
                this.animeflame = 1;
            }
            get
            {
                return this.animeflame;
            }
        }

        public virtual byte AnimeFlame
        {
            set
            {
                this.animeflame = value;
            }
            get
            {
                return this.animeflame;
            }
        }

        public virtual Vector3 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        public MapCharacterBase(
          IAudioEngine s,
          SceneMap p,
          Point po,
          int floor,
          MapCharacterBase.ANGLE a,
          MapField fi)
          : base(s)
        {
            this.parent = p;
            this.angle = a;
            this.field = fi;
            this.floor = floor;
            if (this.field != null)
                this.position = new Vector3(po.X, po.Y, floor);
            else
                this.position = new Vector3(po.X, po.Y, 0.0f);
        }

        public void MoveEndPosiSet()
        {
            Vector2 vector2 = new Vector2(this.position.X, this.position.Y);
            bool flag = false;
            this.EndAngle = this.Angle;
            for (int index = 0; index < this.moveOrder.Length; ++index)
            {
                switch (this.moveOrder[index].move)
                {
                    case EventMove.MOVE.up:
                    case EventMove.MOVE.warpup:
                        if (!flag)
                            this.EndAngle = MapCharacterBase.ANGLE.UPRIGHT;
                        vector2.Y -= moveOrder[index].length;
                        break;
                    case EventMove.MOVE.down:
                    case EventMove.MOVE.warpdown:
                        if (!flag)
                            this.EndAngle = MapCharacterBase.ANGLE.DOWNLEFT;
                        vector2.Y += moveOrder[index].length;
                        break;
                    case EventMove.MOVE.left:
                    case EventMove.MOVE.warpleft:
                        if (!flag)
                            this.EndAngle = MapCharacterBase.ANGLE.UPLEFT;
                        vector2.X -= moveOrder[index].length;
                        break;
                    case EventMove.MOVE.right:
                    case EventMove.MOVE.warpright:
                        if (!flag)
                            this.EndAngle = MapCharacterBase.ANGLE.DOWNRIGHT;
                        vector2.X += moveOrder[index].length;
                        break;
                    case EventMove.MOVE.wayup:
                        if (!flag)
                        {
                            this.EndAngle = MapCharacterBase.ANGLE.UPRIGHT;
                            break;
                        }
                        break;
                    case EventMove.MOVE.waydown:
                        if (!flag)
                        {
                            this.EndAngle = MapCharacterBase.ANGLE.DOWNLEFT;
                            break;
                        }
                        break;
                    case EventMove.MOVE.wayleft:
                        if (!flag)
                        {
                            this.EndAngle = MapCharacterBase.ANGLE.UPLEFT;
                            break;
                        }
                        break;
                    case EventMove.MOVE.wayright:
                        if (!flag)
                        {
                            this.EndAngle = MapCharacterBase.ANGLE.DOWNRIGHT;
                            break;
                        }
                        break;
                    case EventMove.MOVE.wayfix:
                        flag = true;
                        break;
                    case EventMove.MOVE.wayfixend:
                        flag = false;
                        break;
                }
            }
            this.moveEndPosition = vector2;
            this.moving = true;
        }

        public void MoveEndPosi()
        {
            if (!this.moving)
                return;
            this.position = new Vector3(this.moveEndPosition, this.position.Z);
            this.Angle = this.EndAngle;
            this.moving = false;
        }

        protected void ChangeQuarter()
        {
            this.positionQ.X = (float)(120.0 + parent.MapsizeX / 2.0 + position.X * 2.0 - position.Y * 2.0);
            this.positionQ.Y = (float)(32.0 + position.X + position.Y + position.Z + 24.0);
        }

        public virtual void Update()
        {
        }

        public void StartEvent()
        {
            bool flag = false;
            if (this is MapEventBase && ((MapEventBase)this).LunPage.hitform)
                flag = true;
            if (flag)
                return;
            Player player = this.parent.Player;
            double num = MyMath.Degree((float)Math.Atan2(position.Y - (double)player.Position.Y, position.X - (double)player.Position.X));
            this.Angle = num <= 45.0 || num > 135.0 ? (num <= 135.0 || num > 225.0 ? (num <= 225.0 || num > 315.0 ? MapCharacterBase.ANGLE.UPLEFT : MapCharacterBase.ANGLE.DOWNLEFT) : MapCharacterBase.ANGLE.DOWNRIGHT) : MapCharacterBase.ANGLE.UPRIGHT;
        }

        public bool PlayerHit(MapCharacterBase.ANGLE a)
        {
            if (this.player)
                return true;
            return this.parent.Player.canmove[(int)a];
        }

        public virtual void Render(IRenderer dg)
        {
            if (this.noShadow)
                return;
            Rectangle _rect = new Rectangle(!this.floating ? animeflame * 32 : 0, 384, 32, 48);
            dg.DrawImage(dg, "charachip1", _rect, false, this._position, false, Color.White);
        }

        public virtual float RendSetter()
        {
            return this.Position.X + this.Position.Y;
        }

        // orig:
        //       UP 
        // LEFT -||-  RIGHT 
        //      DOWN
        // new:
        //   WEST  /  NORTH
        // ---------------
        // SOUTH / EAST
        public enum ANGLE
        {
            DOWN,
            DOWNRIGHT, // EAST
            RIGHT,
            UPRIGHT, // NORTH
            UP,
            UPLEFT, // WEST
            LEFT,
            DOWNLEFT, // SOUTH
            none,
        }
    }
}
