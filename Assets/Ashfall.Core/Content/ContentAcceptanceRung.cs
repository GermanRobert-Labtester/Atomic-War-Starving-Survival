// SPDX-License-Identifier: MIT
// ASHFALL Core: Content Acceptance Rung
//
// Ticket REM-009 / R18 — 8-rung content acceptance ladder.
// A definition or catalog must meet its declared rung before beta acceptance.

namespace Ashfall.Core.Content
{
    /// <summary>
    /// The 8 rungs of content acceptance in ASHFALL.
    /// Higher rungs imply all preceding rungs are satisfied.
    /// </summary>
    public enum ContentAcceptanceRung
    {
        /// <summary>1. Valid JSON, parses without syntax errors.</summary>
        PARSES = 1,

        /// <summary>2. All definition IDs match naming conventions and cross-references resolve.</summary>
        IDS_RESOLVE = 2,

        /// <summary>3. A catalog loader opens and deserializes the content.</summary>
        LOADED = 3,

        /// <summary>4. At least one production consumer system references/queries the catalog.</summary>
        CONSUMER_EXISTS = 4,

        /// <summary>5. Content is reachable by player action, simulation loop, or event trigger.</summary>
        PLAYER_OR_SIM_REACHABLE = 5,

        /// <summary>6. Consuming this content causes an authoritative gameplay/state/stat effect.</summary>
        EFFECT_PRODUCED = 6,

        /// <summary>7. Content is rendered or presented on a UI panel, journal, HUD, or dialog.</summary>
        PRESENTED = 7,

        /// <summary>8. If stateful, dynamic modifications survive a save/load roundtrip.</summary>
        SAVE_ROUNDTRIP = 8
    }
}
