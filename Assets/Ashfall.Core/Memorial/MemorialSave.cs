using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Memorial
{
    /// <summary>Checksummed save envelope for the memorial ledger.</summary>
    [Serializable]
    public class MemorialSave
    {
        public const int CurrentSaveVersion = 1;
        public const int MigrationFromVersion = 1;
        public int saveVersion = CurrentSaveVersion;
        public int simDay;
        public MemorialState State = new MemorialState();
        public string Checksum = string.Empty;
    }
}
