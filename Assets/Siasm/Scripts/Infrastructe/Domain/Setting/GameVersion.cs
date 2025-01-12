using System;

namespace Siasm
{
    [Serializable]
    public class GameVersion
    {
        public int latestVersion = 1;
        public int currentLoadedVersion = 0;

        public bool IsLatestVersion()
        {
            return latestVersion == currentLoadedVersion;
        }
    }
}
