// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Disease;
using Ashfall.Core.Research;
using Ashfall.Core.Shelter;
using System;
using System.Collections.Generic;
using System.IO;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PrewarArchiveDecryptionSystem? _archiveDecryption62;

        private bool _archiveDecryption62Dirty;

        public PrewarArchiveDecryptionSystem? ArchiveDecryptionSystem => _archiveDecryption62;

        private void SavePrewarArchives()
        {
            if (_archiveDecryption62 != null)
            {
                CaptureSection(PrewarArchiveSaveStore.SectionName,
                    PrewarArchiveSaveStore.TryCapturePersisted(_archiveDecryption62.CaptureState()));
                _archiveDecryption62Dirty = false;
            }
        }

    }
}
