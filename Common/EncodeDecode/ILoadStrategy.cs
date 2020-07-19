using System;
using System.Collections.Generic;

namespace Common.EncodeDecode
{
    public interface ILoadStrategy
    {
        event EventHandler<LoadProgressUpdatedEventArgs> ProgressUpdated;

        void Load();

        IEnumerable<string> GetProvidableFiles();

        bool CanProvideFile(string fileName);
    }
}
