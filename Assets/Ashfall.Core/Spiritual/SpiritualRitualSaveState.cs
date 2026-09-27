// SPDX-License-Identifier: MIT
// ============================================================================
// Core State DTO : SpiritualRitualSaveState
// Subsystem      : EXPANSION-13-THE-FAITHFUL-AND-THE-FRACTURED
// Purpose        : Bounded ritual cooldown ledger — last-performed campaign day
//                  per authored ritual id. Additive: an old save that omits the
//                  field restores as "never performed" per the repository's
//                  optional-field migration tolerance.
// ============================================================================

using System;
using System.Collections.Generic;

namespace Ashfall.Core.Spiritual
{
    [Serializable]
    public sealed class SpiritualRitualSaveState
    {
        public int schema_version = 1;
        public Dictionary<string, int> LastPerformedDay = new Dictionary<string, int>(StringComparer.Ordinal);
    }
}
