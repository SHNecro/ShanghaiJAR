using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSGame;
using NSMap;
using NSMap.Character;

namespace NSEvent
{
    internal class EventMove : EventBase
    {
        public int ID;
        public string IDname;
        private readonly bool nameOn;
        private readonly NSMap.Character.EventMove[] moves;
        private readonly SceneMap map;
        private readonly MapField field;
        private bool movesAdded;

        public EventMove(
          IAudioEngine s,
          EventManager m,
          int id,
          NSMap.Character.EventMove[] mo,
          SceneMap ma,
          MapField fi,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.ID = id;
            this.moves = mo;
            this.map = ma;
            this.field = fi;
        }

        public EventMove(
          IAudioEngine s,
          EventManager m,
          string id,
          NSMap.Character.EventMove[] mo,
          SceneMap ma,
          MapField fi,
          SaveData save)
          : base(s, m, save)
        {
            this.NoTimeNext = true;
            this.IDname = id;
            this.moves = mo;
            this.map = ma;
            this.field = fi;
            this.nameOn = true;
        }

        public override void Update()
        {
            if (!this.nameOn)
            {
                if (this.ID == -1)
                {
                    for (int index = 0; index < this.moves.Length; ++index)
                    {
                        if (this.moves[index] != null)
                            this.moves[index].EventSet(map.Player);
                    }
                    this.map.Player.moveOrder = new NSMap.Character.EventMove[this.moves.Length];
                    this.moves.CopyTo(map.Player.moveOrder, 0);
                    this.map.Player.MoveEndPosiSet();
                    this.map.Player.movingOrder = 0;
                }
                else
                {
                    for (int index = 0; index < this.moves.Length; ++index)
                        this.moves[index].EventSet(this.field.Events[this.ID]);
                    this.field.Events[this.ID].moveOrder = (NSMap.Character.EventMove[])this.moves.Clone();
                    this.field.Events[this.ID].MoveEndPosiSet();
                    this.field.Events[this.ID].movingOrder = 0;
                }
            }
            else if (this.IDname == "プレイヤー")
            {
                for (int index = 0; index < this.moves.Length; ++index)
                {
                    if (this.moves[index] != null)
                        this.moves[index].EventSet(map.Player);
                }
                this.map.Player.moveOrder = new NSMap.Character.EventMove[this.moves.Length];
                this.moves.CopyTo(map.Player.moveOrder, 0);
                this.map.Player.MoveEndPosiSet();
                this.map.Player.movingOrder = 0;
            }
            else
            {
                try
                {
                    MapEventBase mapEventBase = this.field.Events.Find(e => e.ID == this.IDname);
                    for (int index = 0; index < this.moves.Length; ++index)
                        this.moves[index].EventSet(mapEventBase);
                    mapEventBase.moveOrder = (NSMap.Character.EventMove[])this.moves.Clone();
                    mapEventBase.MoveEndPosiSet();
                    mapEventBase.movingOrder = 0;
                }
                catch
                {
                }
            }
            this.movesAdded = true;
            this.EndCommand();
        }

        public override void SkipUpdate()
        {
            if (!this.movesAdded)
            {
                this.Update();
            }

            if (this.IDname == "プレイヤー")
            {
                this.manager.parent.Player.MoveEndPosi();
                this.manager.parent.Player.moveOrder = new NSMap.Character.EventMove[0];
                this.manager.parent.Player.movingOrder = 0;
            }
            else
            {
                foreach (MapEventBase mapEventBase in this.field.Events)
                {
                    if (mapEventBase.ID == this.IDname)
                    {
                        mapEventBase.MoveEndPosi();
                        mapEventBase.moveOrder = new NSMap.Character.EventMove[0];
                        mapEventBase.movingOrder = 0;
                    }
                }
            }
        }

        public override void Render(IRenderer dg)
        {
            this.NoTimesRender(dg);
        }
    }
}
