using Common;
using System.IO;
using System.Reflection;

namespace MapEditor.Core
{
	public class TranslationEntry
	{
		public Dialogue Dialogue { get; set; }

		public string FilePath { get; set; }

		public string FilePathShort
		{
			get
			{
				var assemblyLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar;
				return this.FilePath.Replace(assemblyLoc, "");
			}
		}
	}
}
