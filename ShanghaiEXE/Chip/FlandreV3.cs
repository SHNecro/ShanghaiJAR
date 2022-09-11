using NSAttack;
using NSBattle;
using NSBattle.Character;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEffect;
using NSEnemy;
using NSNet;
using NSObject;
using Common.Vectors;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Linq;

namespace NSChip
{
    internal class FlandreV3 : FlandreV1
    {
        private const int interval = 20;
        private const int speed = 2;
        private int[] motionList;
        private int nowmotion;
        private bool end;
        private int command;
        protected int xPosition;
        private const int s = 5;
        protected Point animePoint;
        private int swrInt = 3;

        private List<Point> targetMulti = new List<Point>();

        public FlandreV3(IAudioEngine s)
          : base(s)
        {
            this.navi = true;

            this.number = 205;
            this.name = NSGame.ShanghaiEXE.Translate("Chip.FlandreV3Name");
            this.element = ChipBase.ELEMENT.normal;
            this.power = 150;
            this.subpower = 50;
            this.regsize = 77;
            this.reality = 5;
            this.swordtype = false;
            this._break = false;
            this.dark = false;
            this.shadow = false;
            this.powerprint = true;
            this.code[0] = ChipFolder.CODE.F;
            this.code[1] = ChipFolder.CODE.L;
            this.code[2] = ChipFolder.CODE.F;
            this.code[3] = ChipFolder.CODE.L;

            var information = NSGame.ShanghaiEXE.Translate("Chip.FlandreV3Desc");
            this.information[0] = information[0];
            this.information[1] = information[1];
            this.information[2] = information[2];


            this.Init();
            this.motionList = new int[4] { 0, 1, 2, 3 };
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
                this._rect = new Rectangle(56 * 8, 0, 56, 48);
                dg.DrawImage(dg, "chipgraphic23", this._rect, true, p, Color.White);
            }
            base.GraphicsRender(dg, p, c, false, printstatus);
        }




    }
}

