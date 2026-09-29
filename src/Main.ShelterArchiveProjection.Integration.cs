// SPDX-License-Identifier: MIT
using Ashfall.Core.Cognition;
using Ashfall.Core.Shelter;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

        /// <summary>
        /// Plan 162 bounded integration: expose a searchable archive index
        /// over the persisted journal and memorial authorities. This does not
        /// create a parallel archive ledger or persistence path.
        /// </summary>
        public IReadOnlyList<ArchiveEntry> BuildShelterArchiveProjection()
        {
            SetupJournal();
            SetupMemorial();
            return ShelterArchiveSystem.ProjectCanonicalSources(_journal, _memorial);
        }

    }
}
