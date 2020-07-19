using Common.EncodeDecode;
using System.IO;

namespace Common.OpenAL
{
    public interface ISoundLoadStrategy : ILoadStrategy
    {
        WAVData ProvideSound(string file);
    }
}
