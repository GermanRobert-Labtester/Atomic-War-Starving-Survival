# ASHFALL Plan 62 — Deep-Strata Relic Archaeology & Archive Decryption Authority Map

**Subsystem:** Pre-War Encrypted Archive Decryption & Relic Archaeology
**Core Authority:** `Assets/Ashfall.Core/Research/PrewarArchiveDecryptionSystem.cs`
**Data Authority:** `Assets/StreamingAssets/Data/prewar_archives.json` (`schema_version: 1`)
**Save Store:** `src/Host/PrewarArchiveSaveStore.cs` (`prewar_archives`)
**Host & Presentation:** `src/UI/ResearchDetailPanel.cs`, `assets/ui/panels/ResearchDetailPanel.tscn`, `src/Main.Plans62_65.cs`

---

## 1. Subsystem Architecture

Plan 62 integrates deep-strata archaeology with high-tech research progression:
1. **Archive Recovery:** Expeditions and subterranean excavation (`ExcavationSystem`) uncover encrypted optical storage plates, magnetic wire reels, and sub-surface cryptographic wafers.
2. **Laboratory Decryption Pipeline:**
   - **Phase 1: Stabilization & Cleaning:** Solvents and precision cleaning (`room_workshop_precision` or `room_laboratory_research`) to prevent data corruption. Consumes cleaning chemicals.
   - **Phase 2: Cryptanalysis & Signal Extraction:** Assigned researchers with analytical/mathematical skills (`skill_cold_analysis`, `skill_electronics`, `skill_science`) spend research progress to decipher security layers.
   - **Phase 3: Synthesis & Knowledge Integration:** Once 100% decrypted, archives yield instant technological unlocks in `ResearchSystem`, unlock codex entries in `JournalSystem`, or provide rare crafting schematics.

---

## 2. Catalog Schema (`prewar_archives.json`)

Each archive definition contains:
- `id`: Unique string key (e.g., `archive_orbital_telemetry_array`, `archive_subsurface_hydrology_grid`, `archive_geothermal_tap_specifications`, `archive_cryogenic_preservation_protocols`).
- `title`: Display name.
- `description`: Lore and provenance background.
- `encryption_grade`: `basic`, `military_grade`, `quantum_lattice`, `orbital_command`.
- `required_apparatus`: Required shelter room (`room_laboratory_research`, `room_workshop_precision`).
- `decryption_effort`: Base research points needed.
- `chemical_cost`: Item ID and count needed for cleaning (e.g., `item_solvent`, `item_acid_cleaner`).
- `reward_research_ids`: Array of research node IDs unlocked upon completion.
- `reward_codex_id`: Journal codex entry unlocked.
- `reward_schematic_ids`: Array of fabrication/foundry recipe IDs unlocked.

---

## 3. Core Domain Classes

```csharp
namespace Ashfall.Core.Research
{
    public enum ArchiveDecryptionStatus
    {
        Discovered = 0,
        Stabilizing = 1,
        Decrypting = 2,
        Completed = 3,
        Corrupted = 4
    }

    public sealed class PrewarArchiveRecord
    {
        public string ArchiveId { get; set; } = string.Empty;
        public ArchiveDecryptionStatus Status { get; set; }
        public float Progress { get; set; }
        public float TargetProgress { get; set; }
        public string AssignedSurvivorId { get; set; } = string.Empty;
        public int DayStarted { get; set; }
        public int DayCompleted { get; set; }
    }
}
```

---

## 4. Invariants & Determinism

- Decryption daily progress = `BaseProgressRate * (1.0f + WorkerSkillBonus) * PowerEfficiencyMultiplier`.
- If power fails (blackout), progress drops to 0 for that day.
- RNG events (data corruption risk or breakthrough) use `ISeededRng` with purpose `archive_decrypt`.
- Zero UnityEngine or Godot dependencies in Core.
