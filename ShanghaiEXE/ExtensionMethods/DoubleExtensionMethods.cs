namespace NSShanghaiEXE.ExtensionMethods
{
	public static class DoubleExtensionMethods
	{
		public static bool Is(this double d, double eq)
		{
            return System.Math.Abs(d - eq) < double.Epsilon * 8;
		}

        public static bool Is0(this double d) => d.Is(0);

        public static bool Is(this float d, float eq)
        {
            return System.Math.Abs(d - eq) < float.Epsilon * 8;
        }

        public static bool Is0(this float d) => d.Is(0);
    }
}
