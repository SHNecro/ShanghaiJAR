namespace NSGame
{
    internal class Debug
    {
        public static bool Encount = true;
        public static bool RegularMove = true;
        public static bool MaskMapFile = false;
        public static bool EncountMax = false;
        public static bool ChipStock = false;
        public const bool MasterMode = true;
        public const bool DevMess = false;
        public const bool EnglishSystemOld = false;
        public const bool DevMode = false;
        public const bool Output = false;

        public static void DebugSet()
        {
            Debug.Encount = true;
            Debug.RegularMove = true;
            Debug.MaskMapFile = true;
            Debug.EncountMax = false;
            Debug.ChipStock = false;
        }

        public static void DebugMess(string mess)
        {
        }

        public static void OutputChipList()
        {
        }
    }
}
