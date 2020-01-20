using Common.EncodeDecode;
using System;
using System.IO;
using System.Threading;

namespace ResourcePackingBuildStep
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.Error.WriteLine("Invalid arguments, expected \"ResourcePackingBuildStep.exe <pack|encode> <packed directory> <output file prefix|directory>\"");
                return -1;
            }

            if (args[0] == "pack")
            {
                var packComplete = false;
                TCDEncodeDecode.PackTCDFile(
                    args[1],
                    args[2],
                    (fileName, progress) =>
                    {
                        if (fileName != null)
                        {
                            Console.WriteLine($"({100 * progress:F1}%) {fileName}");
                        }
                        else
                        {
                            packComplete = true;
                        }
                    });

                while (!packComplete)
                {
                    Thread.Sleep(1);
                }

                Console.WriteLine($"(100.0%) Directory packed into:");
                Console.WriteLine($"         > {args[1]}Resource.tcd");
                Console.WriteLine($"         > {args[1]}Pattern.tcd");
            }
            else if (args[0] == "encode")
            {
                Directory.CreateDirectory(args[2]);

                var mapfiles = Directory.GetFiles(args[1]);

                for (int i = 0; i < mapfiles.Length; i++)
                {
                    var map = mapfiles[i];
                    var contents = TCDEncodeDecode.ReadTextFile(map, false);
                    var encodedContents = TCDEncodeDecode.EncodeMap(contents);

                    var filename = Path.GetFileNameWithoutExtension(map);
                    var newPath = Path.Combine(args[2], Path.ChangeExtension(filename, ".she"));
                    TCDEncodeDecode.SaveFile(newPath, encodedContents);

                    Console.WriteLine($"({100 * ((double)i / mapfiles.Length):F1}%) {filename}");
                }

                Console.WriteLine($"(100.0%) Map files encoded to directory:");
                Console.WriteLine($"         > {args[2]}");
            }
            else
            {
                Console.Error.WriteLine($"Invalid operation \"{args[0]}\", expected \"pack\" or \"encode\"");
                return -1;
            }


            return 0;
        }
    }
}
