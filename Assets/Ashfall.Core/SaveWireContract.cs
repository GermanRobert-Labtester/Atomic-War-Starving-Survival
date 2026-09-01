using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Save wire-format contract — the shape every Core save DTO must produce
    /// so a save written by the Godot host (via SystemTextJsonSerializer) is
    /// readable by a Unity host (via JsonUtility).
    ///
    /// Why this exists: Unity's <c>JsonUtility</c> and
    /// <c>System.Text.Json</c> serialize the *same* object to *different* bytes.
    /// The first round of cross-host saves hard-rejected the other side as
    /// corrupt. The replacement contract fixes that by pinning the format
    /// explicitly and testing it.
    ///
    /// Rules — a save DTO that ships to disk must satisfy ALL of these:
    ///
    /// 1. **Public fields, not properties.** <c>JsonUtility</c> serializes
    ///    fields only. <c>SystemTextJsonSerializer</c> uses
    ///    <c>IncludeFields = true</c> so it sees fields too.
    /// 2. **Plain CLR types only.** No <c>Dictionary&lt;,&gt;</c>, no interfaces,
    ///    no abstract types, no <c>object</c>-typed slots. Lists of plain
    ///    types are fine; lists of polymorphic shapes are not.
    /// 3. **No null/empty ambiguity at the type level.** Strings default to
    ///    <c>string.Empty</c>, lists default to empty. The
    ///    <see cref="SaveChecksum"/> normalizer absorbs the
    ///    null-vs-empty difference so the hash is identical either way.
    /// 4. **Field names match across hosts.** A field called
    ///    <c>encounterId</c> in Core is a field called <c>encounterId</c> on
    ///    the Unity POCO. PascalCase vs camelCase matters: pick one per DTO
    ///    and stick to it (current Core DTOs use camelCase).
    /// 5. **Every DTO carries a <c>schema_version</c> field.** Loaded by the
    ///    codec; missing or stale triggers migration or rejection.
    /// 6. **The save envelope carries a <c>Checksum</c> field** as the last
    ///    field written. The <see cref="SaveChecksum.Compute"/> skips that
    ///    field name, so a writer never has to blank it before hashing.
    ///
    /// The compliance test is <c>Ashfall.Core.Tests/SaveWireContractTests.cs</c>:
    /// every Core save DTO listed below must round-trip through the wire
    /// format and produce identical bytes regardless of which serializer
    /// wrote it.
    /// </summary>
    public static class SaveWireContract
    {
        /// <summary>
        /// The Core save DTOs covered by this contract. Adding a new DTO
        /// without adding it here and writing a test for it is a contract
        /// violation. Listed as strings for grep-ability.
        /// </summary>
        public static readonly IReadOnlyList<string> CoveredDtoTypes = new[]
        {
            "Ashfall.Core.Narrative.NarrativeEncounterState",
            "Ashfall.Core.Narrative.EncounterResolutionRecord",
            "Ashfall.Core.Narrative.PendingSurfacedEncounter",
            "Ashfall.Core.Medical.ChemicalDependencyLedgerState",
            "Ashfall.Core.Medical.ChemicalDependencyState",
            "Ashfall.Core.World.WorldWeatherState",
            "Ashfall.Core.Expeditions.ExpeditionState",
            "Ashfall.Core.Journal.JournalSave"
        };
    }
}
