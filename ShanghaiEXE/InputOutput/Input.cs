using NSGame;
using System;
using System.Collections.Generic;

namespace NSShanghaiEXE.InputOutput
{
    public static class Input
    {
        public static List<bool[]> inputRecord = new List<bool[]>();
        public static List<bool[]> inputRecordEnemy = new List<bool[]>();
        private static ShanghaiEXE parent;
        public const int RecordFlame = 30;

        public static void FormSetting(ShanghaiEXE f)
        {
            Input.parent = f;
        }

        public static bool NumPress(int num)
        {
            return Input.parent.MyKeyBoard.IsPress(num) || Input.parent.MyKeyBoard.IsPress(89 + num);
        }

        public static bool IsPress(Button buttun)
        {
            switch (buttun)
            {
                case Button.Up:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._Up) || Input.parent.Controller.IsPress(Input.parent.Controller._Up);
                case Button.Right:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._Right) || Input.parent.Controller.IsPress(Input.parent.Controller._Right);
                case Button.Down:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._Down) || Input.parent.Controller.IsPress(Input.parent.Controller._Down);
                case Button.Left:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._Left) || Input.parent.Controller.IsPress(Input.parent.Controller._Left);
                case Button._A:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._A) || Input.parent.Controller.IsPress(Input.parent.Controller._A);
                case Button._B:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._B) || Input.parent.Controller.IsPress(Input.parent.Controller._B);
                case Button._L:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._L) || Input.parent.Controller.IsPress(Input.parent.Controller._L);
                case Button._R:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._R) || Input.parent.Controller.IsPress(Input.parent.Controller._R);
                case Button._Start:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._Start) || Input.parent.Controller.IsPress(Input.parent.Controller._Start);
                case Button._Select:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._Select) || Input.parent.Controller.IsPress(Input.parent.Controller._Select);
                case Button.Esc:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard._Esc);
                case Button.Turbo:
                    return Input.parent.MyKeyBoard.IsPress(Input.parent.MyKeyBoard.Turbo) || Input.parent.Controller.IsPress(Input.parent.Controller.Turbo);
                default:
                    return false;
            }
        }

        public static bool IsPush(Button buttun)
        {
            switch (buttun)
            {
                case Button.Up:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._Up) || Input.parent.Controller.IsPush(Input.parent.Controller._Up);
                case Button.Right:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._Right) || Input.parent.Controller.IsPush(Input.parent.Controller._Right);
                case Button.Down:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._Down) || Input.parent.Controller.IsPush(Input.parent.Controller._Down);
                case Button.Left:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._Left) || Input.parent.Controller.IsPush(Input.parent.Controller._Left);
                case Button._A:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._A) || Input.parent.Controller.IsPush(Input.parent.Controller._A);
                case Button._B:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._B) || Input.parent.Controller.IsPush(Input.parent.Controller._B);
                case Button._L:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._L) || Input.parent.Controller.IsPush(Input.parent.Controller._L);
                case Button._R:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._R) || Input.parent.Controller.IsPush(Input.parent.Controller._R);
                case Button._Start:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._Start) || Input.parent.Controller.IsPush(Input.parent.Controller._Start);
                case Button._Select:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._Select) || Input.parent.Controller.IsPush(Input.parent.Controller._Select);
                case Button.Esc:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard._Esc);
                case Button.Turbo:
                    return Input.parent.MyKeyBoard.IsPush(Input.parent.MyKeyBoard.Turbo) || Input.parent.Controller.IsPush(Input.parent.Controller.Turbo);
                default:
                    return false;
            }
        }

        public static bool IsUp(Button buttun)
        {
            switch (buttun)
            {
                case Button.Up:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._Up) || Input.parent.Controller.IsUp(Input.parent.Controller._Up);
                case Button.Right:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._Right) || Input.parent.Controller.IsUp(Input.parent.Controller._Right);
                case Button.Down:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._Down) || Input.parent.Controller.IsUp(Input.parent.Controller._Down);
                case Button.Left:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._Left) || Input.parent.Controller.IsUp(Input.parent.Controller._Left);
                case Button._A:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._A) || Input.parent.Controller.IsUp(Input.parent.Controller._A);
                case Button._B:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._B) || Input.parent.Controller.IsUp(Input.parent.Controller._B);
                case Button._L:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._L) || Input.parent.Controller.IsUp(Input.parent.Controller._L);
                case Button._R:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._R) || Input.parent.Controller.IsUp(Input.parent.Controller._R);
                case Button._Start:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._Start) || Input.parent.Controller.IsUp(Input.parent.Controller._Start);
                case Button._Select:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._Select) || Input.parent.Controller.IsUp(Input.parent.Controller._Select);
                case Button.Esc:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard._Esc);
                case Button.Turbo:
                    return Input.parent.MyKeyBoard.IsUp(Input.parent.MyKeyBoard.Turbo) || Input.parent.Controller.IsUp(Input.parent.Controller.Turbo);
                default:
                    return false;
            }
        }

        public static bool IR_Push(Button button, int flame)
        {
            try
            {
                if (flame < 0)
                    return Input.IsPush(button);
                return Input.inputRecord[flame][(int)button];
            }
            catch
            {
                return false;
            }
        }

        public static bool IR_Press(Button button, int flame)
        {
            try
            {
                if (flame < 0)
                    return Input.IsPress(button);
                return Input.inputRecord[flame][(int)button] && Input.inputRecord[flame][(int)button] != Input.inputRecord[flame + 1][(int)button];
            }
            catch
            {
                return false;
            }
        }

        public static bool IR_Up(Button button, int flame)
        {
            try
            {
                if (flame < 0)
                    return Input.IsUp(button);
                return !Input.inputRecord[flame][(int)button] && Input.inputRecord[flame][(int)button] != Input.inputRecord[flame + 1][(int)button];
            }
            catch
            {
                return false;
            }
        }

        public static bool IR_PushE(Button button, int flame)
        {
            if (flame < 0)
                flame = 0;
            return Input.inputRecordEnemy[flame][(int)button];
        }

        public static bool IR_PressE(Button button, int flame)
        {
            if (flame < 0)
                flame = 0;
            return Input.inputRecordEnemy[flame][(int)button] && Input.inputRecordEnemy[flame][(int)button] != Input.inputRecordEnemy[flame + 1][(int)button];
        }

        public static bool IR_UpE(Button button, int flame)
        {
            if (flame < 0)
                flame = 0;
            return !Input.inputRecordEnemy[flame][(int)button] && Input.inputRecordEnemy[flame][(int)button] != Input.inputRecordEnemy[flame + 1][(int)button];
        }

        public static void InputSave()
        {
            bool[] flagArray = new bool[Enum.GetNames(typeof(Button)).Length];
            for (int index = 0; index < Enum.GetNames(typeof(Button)).Length; ++index)
                flagArray[index] = Input.IsPush((Button)index);
            Input.inputRecord.Insert(0, flagArray);
            if (Input.inputRecord.Count < 30)
                return;
            Input.inputRecord.RemoveAt(Input.inputRecord.Count - 1);
        }
    }
}
