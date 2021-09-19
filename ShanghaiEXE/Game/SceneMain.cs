using NSBattle;
using NSChip;
using NSShanghaiEXE.InputOutput;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEvent;
using NSMap;
using NSNet;
using System.Threading;

namespace NSGame
{
    public class SceneMain : SceneBase
    {
        public ChipFolder[,] chipfolder = new ChipFolder[3, 30];
        private SceneMain.PLAYSCENE nowscene;
        private SceneMain.PLAYSCENE nextscene;
        private bool changeScene;
        public SceneBattle battlescene;
        public SceneMap mapscene;
        public EventManager eventmanager;
        private int packetLossTime;
        private bool packetGetSucces;

        public SceneMain.PLAYSCENE NowScene
        {
            set
            {
                this.nextscene = value;
                this.changeScene = true;
            }
        }

        public SceneMain(IAudioEngine s, ShanghaiEXE p, SaveData save)
          : base(s, p, save)
        {
            this.eventmanager = new EventManager(this.mapscene, this.sound);
            for (int index1 = 0; index1 < this.chipfolder.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.chipfolder.GetLength(1); ++index2)
                {
                    this.chipfolder[index1, index2] = new ChipFolder(this.sound);
                    this.chipfolder[index1, index2].SettingChip(this.savedata.chipFolder[index1, index2, 0]);
                    this.chipfolder[index1, index2].codeNo = this.savedata.chipFolder[index1, index2, 1];
                }
            }
            this.parent = p;
            this.battlescene = new SceneBattle(this.sound, p, this, this.eventmanager, true, 0, true, "", this.savedata);
            this.mapscene = new SceneMap(this.sound, p, this, this.eventmanager, save);
            this.nowscene = SceneMain.PLAYSCENE.map;
        }

        public void FolderReset()
        {
            for (int index1 = 0; index1 < this.chipfolder.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.chipfolder.GetLength(1); ++index2)
                    this.chipfolder[index1, index2].SettingChip(this.chipfolder[index1, index2].chip.number);
            }
        }

        public void FolderSave()
        {
            for (int index1 = 0; index1 < this.chipfolder.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.chipfolder.GetLength(1); ++index2)
                {
                    this.savedata.chipFolder[index1, index2, 0] = this.chipfolder[index1, index2].chip.number;
                    this.savedata.chipFolder[index1, index2, 1] = this.chipfolder[index1, index2].codeNo;
                }
            }
        }

        public void FolderLoad()
        {
            for (int index1 = 0; index1 < this.chipfolder.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < this.chipfolder.GetLength(1); ++index2)
                {
                    this.chipfolder[index1, index2].chip.number = this.savedata.chipFolder[index1, index2, 0];
                    this.chipfolder[index1, index2].codeNo = this.savedata.chipFolder[index1, index2, 1];
                }
            }
        }

        public override void Updata()
        {
            if (this.mapscene.loadflame > 0)
                --this.mapscene.loadflame;
            switch (this.nowscene)
            {
                case SceneMain.PLAYSCENE.battle:
                    Input.InputSave();
                    this.battlescene.Updata();
                    break;
                case SceneMain.PLAYSCENE.map:
                    this.mapscene.Updata();
                    break;
            }
            if (this.ShakeFlag)
                this.Shaking();
            if (this.savedata.saveEnd)
            {
                if (Input.IsPush(Button.Esc))
                    this.Reset();
                if (Input.IsPush(Button._L) && Input.IsPush(Button._R) && Input.IsPush(Button._Select) && Input.IsPush(Button._Start))
                    this.Reset();
                if (Input.IsPush(Button._A) && Input.IsPush(Button._B) && Input.IsPush(Button._Select) && Input.IsPush(Button._Start))
                    this.Reset();
            }
            this.savedata.TimePlus();

            if (this.changeScene)
                this.ChangeScene();
            if (!this.savedata.saveEndnowsub)
                this.savedata.saveEndnow = false;
            else
                this.savedata.saveEndnowsub = false;
        }

        private void Reset()
        {
            this.mapscene.loadflame = 120;
            this.savedata.Init();
            var loadThread = new Thread(new ThreadStart(() => this.savedata.Load(this.parent)));
            loadThread.Start();
            this.sound.StopBGM();
            this.sound.BGMVolumeSet(100);
            this.parent.ChangeOfSecne(Scene.Title);
        }

        public override void Render(IRenderer dg)
        {
            switch (this.nowscene)
            {
                case SceneMain.PLAYSCENE.battle:
                    this.battlescene.Render(dg);
                    break;
                case SceneMain.PLAYSCENE.map:
                    this.mapscene.Render(dg);
                    break;
            }
        }

        private void ChangeScene()
        {
            this.nowscene = this.nextscene;
            this.changeScene = false;
        }

        public enum PLAYSCENE
        {
            battle,
            map,
        }
    }
}
