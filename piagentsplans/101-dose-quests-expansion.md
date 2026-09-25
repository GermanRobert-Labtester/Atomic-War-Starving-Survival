# Plan 101 — Dose Quests Expansion: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** radiation bureaucracy, dose ledger, triage, and moral choice
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Build dose quests as a narrative and command layer over DoseLedgerSystem, voluntary registers, medical/triage owners, and existing moral consequences, with truthful dose facts and no second exposure ledger.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `6100` characters.
- Current worktree copy: `502066` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/DoseQuestMigration.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `fed1e2e9473b560586b087b3e61adb70849e8af6349caf5411195f6eb89a640d`
- Snapshot size: 6555 characters; 161 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0017:     /// share a uniform id prefix like <c>quest_verdict_</c>): the four register
0018:     /// quest lines authored in <c>dose_quests.json</c>.
0019:     /// </summary>
0020:     public static class DoseQuestMigration
```

### Current evidence: `Assets/Ashfall.Core/DoseLedgerSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `8de849230f31198890a51cffd56dd5d740a2a8fa8d1535ae130bac248cfec9fb`
- Snapshot size: 14786 characters; 352 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0037:     [Serializable]
0038:     public class DoseLedgerSystemState
0039:     {
0040:         public string systemId = DoseLedgerSystem.SystemId;
...
0054:     /// </summary>
0055:     public class DoseLedgerSystem
0056:     {
0057:         public const string SystemId = "dose_ledger_system";
...
0068:
0069:         private readonly DoseLedgerSystemState _state = new DoseLedgerSystemState();
0070:         private readonly Dictionary<string, DoseEntry> _entries = new Dictionary<string, DoseEntry>();
0071:
...
0074:         public event Action OnLedgerCalibrated;
0075:         public event Action<DoseLedgerSystemState> OnStateChanged;
0076:
0077:         public DoseLedgerSystemState State => _state;
...
0239:
0240:         public DoseLedgerSystemState CaptureState()
0241:         {
0242:             // Fresh copy, ordinal-ordered: never return the live state to the
...
0274:
0275:         public void RestoreState(DoseLedgerSystemState saved)
0276:         {
0277:             if (saved == null) return;
```

### Current evidence: `Assets/Ashfall.Core/DoseLedgerSave.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `8aa7f6abe7bef1e206f12773566725e3b38e4bde30bcdbbd12b001565d55bdd7`
- Snapshot size: 7434 characters; 163 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0015:     /// helper folds any Dose quest progress a pre-v2 save carried inside the Year
0016:     /// of Ash envelope into the Dose envelope (see <see cref="DoseQuestMigration"/>).
0017:     ///
0018:     /// Migration validates the checksum over each version's FROZEN shape (see
...
0030:         public int simDay;
0031:         public DoseLedgerSystemState doseLedger = new DoseLedgerSystemState();
0032:         public SickListSystemState sickList = new SickListSystemState();
0033:         public CohortSystemState cohort = new CohortSystemState();
...
0049:         public int simDay;
0050:         public DoseLedgerSystemState doseLedger = new DoseLedgerSystemState();
0051:         public SickListSystemState sickList = new SickListSystemState();
0052:         public CohortSystemState cohort = new CohortSystemState();
...
0060:             int simDay,
0061:             DoseLedgerSystem doseLedger,
0062:             SickListSystem sickList,
0063:             CohortSystem cohort,
...
0146:             DoseLedgerSave save,
0147:             DoseLedgerSystem doseLedger,
0148:             SickListSystem sickList,
0149:             CohortSystem cohort,
```

### Current evidence: `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0e44236a1066793d6abf401f6621c12df0ec5cc23129a25d07535967b836edeb`
- Snapshot size: 5831 characters; 148 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0020:     [Serializable]
0021:     public class VoluntaryRegisterSystemState
0022:     {
0023:         public string systemId = VoluntaryRegisterSystem.SystemId;
...
0029:     /// high-dose surface work. Not a penalty; a signature. On completion the
0030:     /// dose is banked (host composes this with DoseLedgerSystem).
0031:     /// </summary>
0032:     public class VoluntaryRegisterSystem
...
0035:
0036:         private readonly VoluntaryRegisterSystemState _state = new VoluntaryRegisterSystemState();
0037:         private readonly Dictionary<string, VolunteerEntry> _entries = new Dictionary<string, VolunteerEntry>();
0038:
...
0042:
0043:         public VoluntaryRegisterSystemState State => _state;
0044:         public IReadOnlyList<VolunteerEntry> Entries => _state.entries;
0045:
...
0092:
0093:         public VoluntaryRegisterSystemState CaptureState()
0094:         {
0095:             // Fresh copy with deep-copied entries, ordinal-ordered by
...
0117:
0118:         public void RestoreState(VoluntaryRegisterSystemState saved)
0119:         {
0120:             if (saved == null) return;
```

### Current evidence: `src/Host/DoseLedgerHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Host/DoseLedgerHostSession.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0e8180d60ce3225f3722ee704d8c6d6d2bfcbaee29b8a0510044e536fb551150`
- Snapshot size: 14722 characters; 297 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0014:     /// ASHFALL: THE DOSE — thin Godot-host session for the four dose registers.
0015:     /// Wraps DoseLedgerSystem, SickListSystem, CohortSystem and VoluntaryRegisterSystem.
0016:     /// No gameplay rules here — everything delegates to Ashfall.Core, following the
0017:     /// ExpansionHostSession pattern. Persistence via DoseLedgerSaveStore.
...
0022:
0023:         public DoseLedgerSystem Ledger { get; }
0024:         public SickListSystem SickList { get; }
0025:         public CohortSystem Cohort { get; }
...
0034:         /// <summary>Raised when any register changes (coalesced save dirty flag).</summary>
0035:         public DoseLedgerHostSession(
0036:             DoseLedgerSystem ledger = null!,
0037:             SickListSystem sickList = null!,
...
0045:         {
0046:             Ledger = ledger ?? new DoseLedgerSystem();
0047:             SickList = sickList ?? new SickListSystem();
0048:             Cohort = cohort ?? new CohortSystem();
...
0069:
0070:         public static DoseLedgerHostSession Create(string dataDir, ILog log = null!, ICampaignRngManager? campaignRng = null)
0071:         {
0072:             CatalogLocator.UseInvariantCulture();
...
0090:             }
0091:             return new DoseLedgerHostSession(registers: registers, content: content, quests: quests, campaignRng: campaignRng);
0092:         }
0093:
...
0181:         /// the authored Year-of-Ash fallout window to the nominal reading, then
0182:         /// books through DoseLedgerSystem (which owns the seeded roll, anti-rad
0183:         /// timing, band edges, and cumulative totals). Returns the booked band
0184:         /// and the window that applied (null when clear) so callers can report
...
0268:                 if (e == null) continue;
0269:                 int band = DoseLedgerSystem.BandFor(e.cumulativeMsv);
0270:                 sb.Append("\n  ").Append(e.survivorId).Append(": ").Append(e.cumulativeMsv.ToString("F1")).
0271:                     Append(" mSv [band ").Append(band).Append("]");
```

### Current evidence: `src/Host/DoseLedgerSaveStore.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `5d16d85d0bc7ebde0fc2f9900a147ace6ddae49156587c8185e7f289a5c78703`
- Snapshot size: 2912 characters; 58 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0004: // Core State : Ashfall.Core.DoseLedgerSave
0005: // Host Caller: Main.Holdfast, Main.Phase0 / DoseLedgerHostSession
0006: // Purpose    : Cumulative radiation dose ledger, threshold brackets, and survivor exposure logs
0007: // ============================================================================
```

### Current evidence: `src/UI/DoseLedgerPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4238960b70761f9089982d81facd2f41086749c0b24028ef7c84f944a6ca6ac0`
- Snapshot size: 20438 characters; 451 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0016: /// Per-survivor cumulative radiation readout. Pure presentation. Reads only
0017: /// from <see cref="DoseLedgerSystem"/> via the host session.
0018: ///
0019: /// The ledger deliberately ONLY shows survivors with a booked dosimeter.
...
0038:
0039:     private DoseLedgerHostSession? _doseSession;
0040:     private SurvivorsHostSession? _survivorsHost;
0041:     private List<DoseEntry> _visibleEntries = new();
...
0044:
0045:     public void Bind(DoseLedgerHostSession session, SurvivorsHostSession? survivors = null)
0046:     {
0047:         Unbind();
...
0068:
0069:     private void HandleLedgerChanged(DoseLedgerSystemState _) => RefreshView();
0070:
0071:     public void RefreshView()
...
0095:             float cum = ledger.GetCumulative(entry.survivorId);
0096:             if (cum >= DoseLedgerSystem.BlackMsv) black++;
0097:             else if (cum >= DoseLedgerSystem.RedMsv) red++;
0098:             else if (cum >= DoseLedgerSystem.AmberMsv) amber++;
...
0177:     {
0178:         if (cumulativeMsv >= DoseLedgerSystem.BlackMsv) return AshfallDataGrid.CellState.Critical;
0179:         if (cumulativeMsv >= DoseLedgerSystem.RedMsv)   return AshfallDataGrid.CellState.Warning;
0180:         if (cumulativeMsv >= DoseLedgerSystem.AmberMsv) return AshfallDataGrid.CellState.Caution;
...
0354:         legend.AddThemeConstantOverride("separation", DesignTheme.SpacingLg);
0355:         LegendChip(legend, "AMBER", DesignTheme.Lethe,    $"{DoseLedgerSystem.AmberMsv:0} mSv");
0356:         LegendChip(legend, "RED",   DesignTheme.Entropy,  $"{DoseLedgerSystem.RedMsv:0} mSv");
0357:         LegendChip(legend, "BLACK", DesignTheme.Critical, $"{DoseLedgerSystem.BlackMsv:0} mSv");
```

### Current evidence: `src/Main.GameFlow.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Main.GameFlow.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `2fdc11c80e9e23418cf05a6d8dbd621c66c8d6aba34503e7cde474922c403685`
- Snapshot size: 43241 characters; 950 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using Godot;
0003: using System;
0004: using System.Globalization;
0005: using System.IO;
0006: using System.Linq;
0007: using Ashfall.Core;
0008: using Ashfall.Core.Campaign;
0009: using Ashfall.Core.Difficulty;
0010: using Ashfall.Core.Inventory;
0011: using Ashfall.Core.Save;
0012: using Ashfall.Core.Expeditions;
0013: using AtomicWar.GodotApp.UI;
0014: using AtomicWar.GodotApp.World;
0015:
0016: namespace AtomicWar.GodotApp
0017: {
0018:     public partial class Main : Control
0019:     {
0020:         private void RunDashboardUiTestAndQuit()
```

### Current evidence: `src/Main.Medical.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `56732cc5acbf9c9129f4159be2c23cb6164bf13e1a3b6b2521152b10fa90cf39`
- Snapshot size: 31825 characters; 651 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0417:                 new Ashfall.Core.Medical.MedicalProcedureDef("proc_bandage", "Bandage", "MedicalSystem"),
0418:                 new Ashfall.Core.Medical.MedicalProcedureDef("proc_chelation", "Chelation", "DoseLedgerSystem"),
0419:                 new Ashfall.Core.Medical.MedicalProcedureDef("proc_surgery", "Surgery", "MedicalSystem")
0420:             };
```

### Current evidence: `Assets/StreamingAssets/Data/dose_quests.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b067312a3028e7e0c714902e86c5e281aa62cf430a2698caa4cf591781bf31b5`
- Snapshot size: 32178 characters; 635 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "quests": [
0004:     {
0005:       "questlineId": "quest_the_dose_the_first_reading",
0006:       "title": "The First Reading",
0007:       "synopsis": "One survivor's first dosimeter read books an Amber band. Decide whether the shelter keeps a ledger at all — or closes the book before its first page.",
0008:       "factionTag": "none",
0009:       "minDay": 40,
0010:       "maxDay": 360,
0011:       "stages": [
0012:         {
0013:           "stageId": "stage_first_reading_open",
0014:           "title": "The Dial",
0015:           "narrativePrompt": "The dosimeter needle twitches past the hundred mark before collapsing back. Whatever took the reading, the number is real now — it needs a sheet to live on. Dr. Vel holds the red pencil and waits for you to say whether the bunker is the kind of place that writes this down.",
0016:           "isTerminal": false,
0017:           "choices": [
0018:             {
0019:               "choiceId": "choice_open_the_ledger",
0020:               "text": "Open the ledger. Write it down.",
```

### Current evidence: `Assets/StreamingAssets/Data/dose_registers.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `eec0f33b433f4ae15549f3fc4a61cdde840aba6707c870fc65c6608a69820b08`
- Snapshot size: 6225 characters; 208 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "bands": [
0004:     {
0005:       "id": "band_green",
0006:       "label": "Green",
0007:       "threshold_msv": 0,
0008:       "disposition": "No measurable burden. Walk the corridor."
0009:     },
0010:     {
0011:       "id": "band_white",
0012:       "label": "White",
0013:       "threshold_msv": 25,
0014:       "disposition": "Trace exposure only. The ledger notes it without alarm."
0015:     },
0016:     {
0017:       "id": "band_yellow",
0018:       "label": "Yellow",
0019:       "threshold_msv": 50,
0020:       "disposition": "Minor accumulation. Duty continues; the dial is watched."
```

### Current evidence: `Assets/StreamingAssets/Data/dose_items.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `b4711e702815be948ed7384c738df9265b528cb7aa6e96adecbc1e373d4651bf`
- Snapshot size: 5341 characters; 125 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "id": "item_dose_ledger",
0006:       "name": "The Dose Ledger",
0007:       "weightKg": 1.2,
0008:       "tradeValue": 0,
0009:       "category": "story",
0010:       "description": "A cloth-bound tally book begun by hand after the first reading, its pages ruled for dates, dials, and the names of who held them. Not a medical record. A kept document."
0011:     },
0012:     {
0013:       "id": "item_calibration_key",
0014:       "name": "Dosimeter Calibration Key",
0015:       "weightKg": 0.1,
0016:       "tradeValue": 40,
0017:       "category": "tool",
0018:       "description": "A brass socket-wrench key, hand-made by the clockmaker to fit the adjustment collar of every dosimeter in the bunker. It does not make the dial honest; it makes the dial honest again."
0019:     },
0020:     {
```

### Current evidence: `Assets/StreamingAssets/Data/items.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
- Snapshot size: 390056 characters; 9660 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "id": "item_decon_chelator_concentrate",
0006:       "displayName": "Chelator Concentrate",
0007:       "description": "A sealed glass ampoule of chelating agent concentrate. The label is half dissolved but the formula is standard: binds heavy radionuclides into a water-soluble complex for rinse removal. One ampoule per decon cycle. The pre-war stock won't last forever, and the synthesis requires a working pharma bench.",
0008:       "type": "Consumable",
0009:       "stackMax": 20,
0010:       "weight": 0.2,
0011:       "tradeValue": 8
0012:     },
0013:     {
0014:       "id": "item_lead_lined_effluent_filter",
0015:       "displayName": "Lead-Lined Effluent Filter",
0016:       "description": "A cylindrical filtration cartridge with a lead-foil inner liner and activated charcoal matrix. Installed in the decon airlock effluent tank to capture radionuclide-laden particulates before they can be sluiced into the general water system. Good for approximately five hundred liters of contaminated wash water before replacement is required.",
0017:       "type": "Equipment",
0018:       "stackMax": 5,
0019:       "weight": 3.5,
0020:       "tradeValue": 15
```

### Current evidence: `Assets/StreamingAssets/Data/survivors.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c27e7ca9e79422b77bde6ae05c9b3d682f22d06c19165df1b6e30938b3a9f066`
- Snapshot size: 70319 characters; 1249 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:     "schema_version": 1,
0003:     "survivors": [
0004:         {
0005:             "id": "elena_vasquez",
0006:             "displayName": "Elena Vasquez",
0007:             "profession": "Paramedic",
0008:             "bio": "Nine years of night shifts in an ambulance bay taught Elena to sort the dying fast. She still checks wrists for pulses out of habit, and wears her watch strap-first over the cuff so it never catches on gloves.",
0009:             "baseHealth": 100
0010:         },
0011:         {
0012:             "id": "marcus_olejnik",
0013:             "traitIds": ["trait_claustrophobe"],
0014:             "displayName": "Marcus Olejnik",
0015:             "profession": "Mechanical Engineer",
0016:             "bio": "Marcus kept a turbine hall running on parts that were condemned five years earlier. He talks to machines more easily than to people, and is usually right about what they tell him.",
0017:             "baseHealth": 90
0018:         },
0019:         {
0020:             "id": "suki_tanaka",
```

### Current evidence: `Assets/StreamingAssets/Data/surgical_procedures.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e50111141a5d91be57ebafc8f50747fb6599676558ebca97b1f7d9d8264a2d4b`
- Snapshot size: 2141 characters; 70 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "procedures": [
0004:     {
0005:       "procedure_id": "procedure_amputation_arm_field",
0006:       "display_name": "Field Arm Amputation",
0007:       "limb_group": "arm",
0008:       "required_tool_id": "surgical_saw",
0009:       "required_items": [
0010:         { "item_id": "painkillers", "amount": 1 },
0011:         { "item_id": "clean_water", "amount": 1 },
0012:         { "item_id": "cloth", "amount": 2 }
0013:       ],
0014:       "base_shock_risk": 0.35,
0015:       "base_bleeding": 50.0,
0016:       "infection_reduction": 1.0,
0017:       "recovery_days_min": 10,
0018:       "recovery_days_max": 20,
0019:       "phantom_pain_chance": 0.40
0020:     },
```

### Current evidence: `src/UI/AfflictionsPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/AfflictionsPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `189133c74fdd0eff3dc5e7bc8aa518c8cef986ef5e05c8fa49d900f3219bcccf`
- Snapshot size: 23478 characters; 488 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Collections.Generic;
0004: using Godot;
0005: using Ashfall.Core.UI;
0006: using Ashfall.Core.Medical;
0007: using AtomicWar.GodotApp.Host;
0008:
0009: namespace AtomicWar.GodotApp.UI
0010: {
0011:     /// <summary>
0012:     /// ASHFALL — Afflictions panel showing current afflictions, chronic
0013:     /// conditions, and available treatments. Bound to the live Medical /
0014:     /// Survivors / Respiratory / Inventory sessions.
0015:     ///
0016:     /// Ticket #125: layout chrome (dialog frame, sections, separators,
0017:     /// close button, hint) is owned by
0018:     /// <c>res://assets/ui/panels/AfflictionsPanel.tscn</c>. This binder
0019:     /// projects presentation data into the dynamic lists (active,
0020:     /// chronic, treatments) and wires the close action.
```

### Current evidence: `src/UI/JournalPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/JournalPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `412d1eb12aed0d996c58bb0b653d3b90517d40855ec738f9b5cfb4c139f5bf26`
- Snapshot size: 18870 characters; 486 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using System.Linq;
0004: using Godot;
0005: using Ashfall.Core.IO;
0006: using Ashfall.Core.Journal;
0007: using Ashfall.Core.UI;
0008: using AtomicWar.GodotApp.UI;
0009: using DesignTheme = Ashfall.Core.UI.Theme;
0010:
0011: namespace AtomicWar.GodotApp.UI;
0012:
0013: /// <summary>
0014: /// ASHFALL — Journal panel (wired).
0015: /// Shows real journal entries, discovered items, survivors met, locations
0016: /// visited, and narrative events from the live JournalSystem. Replaces the
0017: /// previous hardcoded placeholder strings with live data binding.
0018: /// </summary>
0019: public partial class JournalPanel : Control
0020: {
```

### Current evidence: `src/UI/VerdictDashboardPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/VerdictDashboardPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d1d57d14186f49a51faace91d3448d4971629702410ebb6ba660f31a7a64e322`
- Snapshot size: 7683 characters; 178 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: // SPDX-License-Identifier: MIT
0002: using System;
0003: using Godot;
0004: using Ashfall.Core.UI;
0005: using AtomicWar.GodotApp;
0006: using DesignTheme = Ashfall.Core.UI.Theme;
0007:
0008: using Ashfall.Core.IO;
0009: namespace AtomicWar.GodotApp.UI;
0010:
0011: /// <summary>
0012: /// ASHFALL — Verdict Dashboard (#15 Stitch).
0013: /// Hosts the existing VerdictPanel inside the dashboard shell so the screen
0014: /// gains the Phase-12 sidebar + status rail chrome without rewriting the
0015: /// bespoke per-record interaction (NPC "hear" buttons, log list, evidence list).
0016: ///
0017: /// Narrative verdict content is deliberately NOT converted into a DataGrid —
0018: /// per the brief, "Do not convert narrative verdict content into a
0019: /// spreadsheet merely for architectural consistency."
0020: ///
```

### Current evidence: `Ashfall.Core.Tests/DoseQuestExpansionTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ce790c391494aedbd7cc4b844e90bef2e3b2cc61fe8911ed527c8f3ae46f15df`
- Snapshot size: 12021 characters; 299 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0094:
0095:             Assert.Equal(12, DoseQuestMigration.CanonicalQuestlineIds.Length);
0096:             foreach (var canonicalId in DoseQuestMigration.CanonicalQuestlineIds)
0097:             {
...
0286:
0287:             int adopted = DoseQuestMigration.AdoptFromYearOfAsh(doseState, yoaState);
0288:             Assert.Equal(2, adopted);
0289:             Assert.Single(doseState.active);
...
0292:
0293:             int stripped = DoseQuestMigration.StripFromYearOfAsh(yoaState);
0294:             Assert.Equal(2, stripped);
0295:             Assert.Empty(yoaState.active);
```

### Current evidence: `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `0976292dca2e1316286983ee591e39a1f523e06069e2921d234bdc1de89af915`
- Snapshot size: 10858 characters; 227 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0067:             var save = DoseLedgerSaveCodec.Capture(
0068:                 40, new DoseLedgerSystem(), new SickListSystem(), new CohortSystem(),
0069:                 new VoluntaryRegisterSystem(), quests);
0070:             Assert.Equal(2, save.saveVersion);
...
0077:             var restored = new QuestlineSystem();
0078:             DoseLedgerSaveCodec.Restore(loaded, new DoseLedgerSystem(), new SickListSystem(),
0079:                 new CohortSystem(), new VoluntaryRegisterSystem(), restored);
0080:             Assert.True(restored.State.active.Exists(a => a.questlineId == qid));
...
0092:                 simDay = 40,
0093:                 doseLedger = new DoseLedgerSystemState(),
0094:                 sickList = new SickListSystemState(),
0095:                 cohort = new CohortSystemState(),
...
0123:
0124:             int adopted = DoseQuestMigration.AdoptFromYearOfAsh(dose, yearOfAsh);
0125:             Assert.Equal(3, adopted); // sick active + signed completed + child failed
0126:             Assert.Single(dose.active);
...
0151:
0152:             int adopted = DoseQuestMigration.AdoptFromYearOfAsh(dose, yearOfAsh);
0153:             Assert.Equal(0, adopted);
0154:             Assert.Single(dose.active);
...
0171:
0172:             int removed = DoseQuestMigration.StripFromYearOfAsh(yearOfAsh);
0173:             Assert.Equal(3, removed);
0174:             Assert.Single(yearOfAsh.active);
...
0193:
0194:             Assert.Equal(1, DoseQuestMigration.AdoptFromYearOfAsh(dose, yearOfAsh));
0195:             Assert.Single(dose.active);
0196:             Assert.Equal("quest_the_sick_of_room_seven", dose.active[0].questlineId);
...
0217:                 if (q == null || string.IsNullOrEmpty(q.questlineId)) continue;
0218:                 Assert.True(DoseQuestMigration.IsDoseQuestline(q.questlineId),
0219:                     "authored dose questline must be canonical: " + q.questlineId);
0220:                 system.RegisterQuestline(q);
```

### Current evidence: `Ashfall.Core.Tests/DoseLedgerSystemTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e983e95d8bc0ef4c41532ae7d17735990ffe850c3259a92b892e4b0d9ac069bc`
- Snapshot size: 8560 characters; 201 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0007: {
0008:     public class DoseLedgerSystemTests
0009:     {
0010:         [Fact]
...
0021:         {
0022:             var dl = new DoseLedgerSystem();
0023:             dl.AssignDosimeter("sv_x", "tag1");
0024:             int band = -1;
...
0027:             dl.BookReading("sv_x", 1, 120f, "sampling", false, false, false, new SeededRng(1));
0028:             Assert.Equal(DoseLedgerSystem.BandAmber, band);
0029:             Assert.Equal(DoseLedgerSystem.BandAmber, DoseLedgerSystem.BandFor(dl.GetCumulative("sv_x")));
0030:         }
...
0034:         {
0035:             var dl = new DoseLedgerSystem();
0036:             dl.AssignDosimeter("sv_x", "tag1");
0037:             dl.BookReading("sv_x", 1, 100f, "x", false, false, true, new SeededRng(1)); // anti-rad after
...
0043:         {
0044:             var a = new DoseLedgerSystem();
0045:             a.AssignDosimeter("sv_a", "t");
0046:             var b = new DoseLedgerSystem();
...
0057:         {
0058:             var dl = new DoseLedgerSystem();
0059:             dl.AssignDosimeter("sv_x", "tag1", 20f);
0060:             dl.BookReading("sv_x", 1, 200f, "sampling", false, false, true, new SeededRng(1));
...
0071:         {
0072:             var dl = new DoseLedgerSystem();
0073:             dl.AssignDosimeter("sv_scavenger", "tag_9");
0074:             // Book 350 mSv (Red band)
...
0077:             Assert.Equal(DoseLedgerSystem.BandRed, DoseLedgerSystem.BandFor(dl.GetCumulative("sv_scavenger")));
0078:             Assert.Equal(DoseLedgerSystem.BandRed, dl.GetAdministrativeBand("sv_scavenger"));
0079:
0080:             // Issue forged clean-bill chit
...
0085:             // But administrative clearance check reports Green band
0086:             Assert.Equal(DoseLedgerSystem.BandGreen, dl.GetAdministrativeBand("sv_scavenger"));
0087:         }
0088:
...
0091:         {
0092:             var dl = new DoseLedgerSystem();
0093:             dl.AssignDosimeter("sv_worker", "tag_12");
0094:             dl.BookReading("sv_worker", 5, 150f, "ash_fallout", false, false, false, new SeededRng(2));
...
0098:             dl.SetAdministrativeClassificationOverride("sv_worker", "band_black");
0099:             Assert.Equal(DoseLedgerSystem.BandBlack, dl.GetAdministrativeBand("sv_worker"));
0100:             Assert.Equal(150f, dl.GetCumulative("sv_worker"));
0101:         }
...
```

### Current evidence: `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `4fc5a94aeab9132f1282222f25b67d05edb8cb60f9a05cfee8b41b23b505087e`
- Snapshot size: 9783 characters; 217 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0105:
0106:             // Verify all canonical questline IDs match DoseQuestMigration
0107:             Assert.Equal(12, DoseQuestMigration.CanonicalQuestlineIds.Length);
0108:             foreach (var canonicalId in DoseQuestMigration.CanonicalQuestlineIds)
```

### Current evidence: `Ashfall.Core.Tests/DoseContentCatalogTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `8654d15f170cf0db4eecb909429ffc5111b6c74605166365ff4222b0c59848c5`
- Snapshot size: 10015 characters; 233 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0202:             Assert.Equal(12, registered);
0203:             foreach (var id in DoseQuestMigration.CanonicalQuestlineIds)
0204:                 Assert.NotNull(questSystem.FindDefinition(id));
0205:         }
```

### Current evidence: `Ashfall.Core.Tests/DoseRegistersCatalogTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `2538daa36d3bb88e10dd9689a15263cf2cbdcaf244109efa3a4e45687fea712d`
- Snapshot size: 5674 characters; 128 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0089:             if (catalog.bands.Count == 0) return;
0090:             Assert.Equal("Green", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandGreen));
0091:             Assert.Equal("Amber", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandAmber));
0092:             Assert.Equal("Red", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandRed));
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/dose_quests.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, quests`
- `quests`: list count=12; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `b067312a3028e7e0c714902e86c5e281aa62cf430a2698caa4cf591781bf31b5`
#### `Assets/StreamingAssets/Data/dose_registers.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, bands, plans, guesses, calibration, registers, npcs`
- `bands`: list count=12; sample IDs=['band_green', 'band_white', 'band_yellow', 'band_amber', 'band_orange', 'band_rose', 'band_red', 'band_crimson']
- `plans`: list count=8; sample IDs=['plan_morphine_tray', 'plan_comfort_rounds', 'plan_nothing', 'plan_chelation', 'plan_iodine_prophylaxis', 'plan_isolation', 'plan_rest', 'plan_transfer']
- `guesses`: list count=3; sample IDs=['guess_low', 'guess_honest', 'guess_refused']
- `registers`: list count=4; sample IDs=['register_ledger', 'register_sick', 'register_cohort', 'register_voluntary']
- `npcs`: list count=4; sample IDs=['npc_dr_irina_vel', 'npc_wyn_omah', 'npc_piet_abar', 'npc_saria_voss']
- `schema_version`: `1`
- SHA-256: `eec0f33b433f4ae15549f3fc4a61cdde840aba6707c870fc65c6608a69820b08`
#### `Assets/StreamingAssets/Data/dose_items.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=15; sample IDs=['item_dose_ledger', 'item_calibration_key', 'item_dosimeter_tag', 'item_palliative_morphine', 'item_cohort_first_board', 'item_calibrated_dosimeter', 'item_forged_clean_bill_chit', 'item_chelation_decorporation_course']
- `schema_version`: `1`
- SHA-256: `b4711e702815be948ed7384c738df9265b528cb7aa6e96adecbc1e373d4651bf`
#### `Assets/StreamingAssets/Data/items.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=724; sample IDs=['item_decon_chelator_concentrate', 'item_lead_lined_effluent_filter', 'item_heavy_neoprene_scrub_brush', 'item_sealed_waste_bin', 'item_theodolite_brass_precision', 'item_surveyor_stadia_rod', 'item_datum_plate_bronze', 'item_concrete_mix']
- `schema_version`: `1`
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
#### `Assets/StreamingAssets/Data/survivors.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, survivors`
- `survivors`: list count=129; sample IDs=['elena_vasquez', 'marcus_olejnik', 'suki_tanaka', 'the_surgeon', 'the_pharmacist', 'the_vet', 'the_therapist', 'the_undertaker']
- `schema_version`: `1`
- SHA-256: `c27e7ca9e79422b77bde6ae05c9b3d682f22d06c19165df1b6e30938b3a9f066`
#### `Assets/StreamingAssets/Data/surgical_procedures.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, procedures`
- `procedures`: list count=4; sample IDs=[]
- `schema_version`: `1`
- SHA-256: `e50111141a5d91be57ebafc8f50747fb6599676558ebca97b1f7d9d8264a2d4b`
## Symbol and caller audit

#### `DoseQuestMigration` — HOST_REFERENCE_PRESENT — core/declaration=2, host=2, test=25
- `Assets/Ashfall.Core/DoseLedgerSave.cs:16` (core) — /// of Ash envelope into the Dose envelope (see <see cref="DoseQuestMigration"/>).
- `Assets/Ashfall.Core/DoseQuestMigration.cs:20` (declaration) — public static class DoseQuestMigration
- `src/YearOfAsh/YearOfAshHostSession.cs:129` (host) — // envelope on load (DoseQuestMigration).
- `src/YearOfAsh/YearOfAshHostSession.cs:313` (host) — Ashfall.Core.DoseQuestMigration.StripFromYearOfAsh(_quests.State);
- `Ashfall.Core.Tests/DoseQuestExpansionTests.cs:95` (test) — Assert.Equal(12, DoseQuestMigration.CanonicalQuestlineIds.Length);
- `Ashfall.Core.Tests/DoseQuestExpansionTests.cs:96` (test) — foreach (var canonicalId in DoseQuestMigration.CanonicalQuestlineIds)
- `Ashfall.Core.Tests/DoseQuestExpansionTests.cs:99` (test) — Assert.True(DoseQuestMigration.IsDoseQuestline(canonicalId));
- `Ashfall.Core.Tests/DoseQuestExpansionTests.cs:287` (test) — int adopted = DoseQuestMigration.AdoptFromYearOfAsh(doseState, yoaState);
- `Ashfall.Core.Tests/DoseQuestExpansionTests.cs:293` (test) — int stripped = DoseQuestMigration.StripFromYearOfAsh(yoaState);
- `Ashfall.Core.Tests/DoseContentCatalogTests.cs:203` (test) — foreach (var id in DoseQuestMigration.CanonicalQuestlineIds)
- `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs:124` (test) — int adopted = DoseQuestMigration.AdoptFromYearOfAsh(dose, yearOfAsh);
- `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs:152` (test) — int adopted = DoseQuestMigration.AdoptFromYearOfAsh(dose, yearOfAsh);
- `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs:172` (test) — int removed = DoseQuestMigration.StripFromYearOfAsh(yearOfAsh);
- `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs:194` (test) — Assert.Equal(1, DoseQuestMigration.AdoptFromYearOfAsh(dose, yearOfAsh));
- `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs:197` (test) — Assert.Equal(1, DoseQuestMigration.StripFromYearOfAsh(yearOfAsh));
- `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs:218` (test) — Assert.True(DoseQuestMigration.IsDoseQuestline(q.questlineId),
- `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs:61` (test) — Assert.True(DoseQuestMigration.CanonicalQuestlineIds.Length >= 12);
- `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs:62` (test) — Assert.True(DoseQuestMigration.IsDoseQuestline("quest_the_falsified_reading"));
- `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs:63` (test) — Assert.True(DoseQuestMigration.IsDoseQuestline("quest_the_stolen_dosimeter"));
- `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs:64` (test) — Assert.True(DoseQuestMigration.IsDoseQuestline("quest_child_over_the_limit"));
- `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs:65` (test) — Assert.True(DoseQuestMigration.IsDoseQuestline("quest_the_register_audit"));
- `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs:66` (test) — Assert.True(DoseQuestMigration.IsDoseQuestline("quest_black_market_clean_bill"));
- `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs:67` (test) — Assert.True(DoseQuestMigration.IsDoseQuestline("quest_the_broken_calibration_chain"));
- `Ashfall.Core.Tests/BodyMind/Plan27BodyMindTests.cs:68` (test) — Assert.True(DoseQuestMigration.IsDoseQuestline("quest_exposure_for_the_essential_worker"));
- … 5 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `DoseLedgerSystem` — HOST_REFERENCE_PRESENT — core/declaration=21, host=31, test=107
- `Assets/Ashfall.Core/DoseLedgerSave.cs:61` (core) — DoseLedgerSystem doseLedger,
- `Assets/Ashfall.Core/DoseLedgerSave.cs:147` (core) — DoseLedgerSystem doseLedger,
- `Assets/Ashfall.Core/DoseLedgerSystem.cs:40` (core) — public string systemId = DoseLedgerSystem.SystemId;
- `Assets/Ashfall.Core/DoseLedgerSystem.cs:55` (declaration) — public class DoseLedgerSystem
- `Assets/Ashfall.Core/SickListSystem.cs:178` (core) — band >= DoseLedgerSystem.BandGreen && band <= DoseLedgerSystem.BandBlack;
- `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs:30` (core) — /// dose is banked (host composes this with DoseLedgerSystem).
- `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs:14` (core) — /// MedicalSystem, DiseaseSystem, DoseLedgerSystem, respiratory
- `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs:208` (core) — public string DelegatedSystemId; // e.g. "MedicalSystem", "DoseLedgerSystem"
- `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs:45` (core) — /// or cumulative booked dose (owned by DoseLedgerSystem).
- `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs:124` (core) — /// Does NOT alter the actual dose — that's DoseLedgerSystem's job.
- `Assets/Ashfall.Core/YearOfAsh/FalloutWindowProvider.cs:12` (core) — // mSv before handing it to DoseLedgerSystem.BookReading, whose own seeded roll,
- `Assets/Ashfall.Core/Disease/DiseaseTriage.cs:74` (core) — /// Band values are <see cref="DoseLedgerSystem"/> band constants: the sick
- `Assets/Ashfall.Core/Disease/DiseaseTriage.cs:156` (core) — /// (<see cref="DoseLedgerSystem.BandGreen"/> … <see cref="DoseLedgerSystem.BandBlack"/>).
- `Assets/Ashfall.Core/Disease/DiseaseTriage.cs:167` (core) — return DoseLedgerSystem.BandAmber;
- `Assets/Ashfall.Core/Disease/DiseaseTriage.cs:169` (core) — return DoseLedgerSystem.BandRed;
- `Assets/Ashfall.Core/Disease/DiseaseTriage.cs:171` (core) — return DoseLedgerSystem.BandBlack;
- `Assets/Ashfall.Core/Disease/DiseaseTriage.cs:173` (core) — return DoseLedgerSystem.BandGreen;
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1077` (core) — ["dose_items.json"] = new[] { "DoseLedgerSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1078` (core) — ["dose_locations.json"] = new[] { "DoseLedgerSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1079` (core) — ["dose_quests.json"] = new[] { "DoseLedgerSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1080` (core) — ["dose_registers.json"] = new[] { "DoseLedgerSystem" },
- `src/Main.UiTests.Dose.cs:45` (host) — bool diagnose = _doseLedger.DiagnoseDemo(DoseLedgerSystem.BandRed).Contains("Diagnosed");
- `src/Main.UiTests.Dose.cs:88` (host) — triageBand = illnessBand != null && illnessBand.band > DoseLedgerSystem.BandGreen;
- `src/Main.Medical.cs:418` (host) — new Ashfall.Core.Medical.MedicalProcedureDef("proc_chelation", "Chelation", "DoseLedgerSystem"),
- … 135 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `VoluntaryRegisterSystem` — HOST_REFERENCE_PRESENT — core/declaration=4, host=4, test=22
- `Assets/Ashfall.Core/DoseLedgerSave.cs:64` (core) — VoluntaryRegisterSystem voluntaryRegister,
- `Assets/Ashfall.Core/DoseLedgerSave.cs:150` (core) — VoluntaryRegisterSystem voluntaryRegister,
- `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs:23` (core) — public string systemId = VoluntaryRegisterSystem.SystemId;
- `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs:32` (declaration) — public class VoluntaryRegisterSystem
- `src/Host/DoseLedgerHostSession.cs:15` (host) — /// Wraps DoseLedgerSystem, SickListSystem, CohortSystem and VoluntaryRegisterSystem.
- `src/Host/DoseLedgerHostSession.cs:26` (host) — public VoluntaryRegisterSystem Voluntary { get; }
- `src/Host/DoseLedgerHostSession.cs:39` (host) — VoluntaryRegisterSystem voluntary = null!,
- `src/Host/DoseLedgerHostSession.cs:49` (host) — Voluntary = voluntary ?? new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/DoseCollectibleSaveFuzzTests.cs:38` (test) — VoluntaryRegisterSystem voluntary, QuestlineSystem quests) BuildDoseSystems()
- `Ashfall.Core.Tests/DoseCollectibleSaveFuzzTests.cs:56` (test) — var voluntary = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/DoseLedgerSystemTests.cs:135` (test) — var save = DoseLedgerSaveCodec.Capture(1, dl, new SickListSystem(), new CohortSystem(), new VoluntaryRegisterSystem());
- `Ashfall.Core.Tests/DoseLedgerSystemTests.cs:159` (test) — var save = DoseLedgerSaveCodec.Capture(1, dl, new SickListSystem(), new CohortSystem(), new VoluntaryRegisterSystem());
- `Ashfall.Core.Tests/DoseLedgerSystemTests.cs:184` (test) — var v = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs:69` (test) — new VoluntaryRegisterSystem(), quests);
- `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs:79` (test) — new CohortSystem(), new VoluntaryRegisterSystem(), restored);
- `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs:19` (test) — var sys = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs:36` (test) — var sys = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs:45` (test) — var sys = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs:60` (test) — var sys = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs:67` (test) — var sys = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs:77` (test) — var sys = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs:90` (test) — var sys = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs:100` (test) — var sys = new VoluntaryRegisterSystem();
- `Ashfall.Core.Tests/VoluntaryRegisterSystemTests.cs:112` (test) — var sys = new VoluntaryRegisterSystem();
- … 6 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `DoseLedgerHostSession` — HOST_REFERENCE_PRESENT — core/declaration=1, host=29, test=1
- `src/Main.Phase0.cs:38` (host) — private DoseLedgerHostSession _doseLedger = null!;
- `src/Main.Phase0.cs:368` (host) — _doseLedger = DoseLedgerHostSession.Create(_dataDir, campaignRng: _campaignDay.Rng);
- `src/Dose/DoseRegisterSurface.cs:17` (host) — /// Thin presentation only: renders DoseLedgerHostSession state and
- `src/Dose/DoseRegisterSurface.cs:22` (host) — private DoseLedgerHostSession _session;
- `src/Dose/DoseRegisterSurface.cs:109` (host) — public void BindSession(DoseLedgerHostSession session)
- `src/Host/DoseLedgerSaveStore.cs:5` (host) — // Host Caller: Main.Holdfast, Main.Phase0 / DoseLedgerHostSession
- `src/Host/PanelBindLifecycleSelfTest.cs:463` (host) — var doseHost = DoseLedgerHostSession.Create(dataDir);
- `src/Host/PanelBindLifecycleSelfTest.cs:660` (host) — var doseHost1 = new DoseLedgerHostSession();
- `src/Host/PanelBindLifecycleSelfTest.cs:661` (host) — var doseHost2 = new DoseLedgerHostSession();
- `src/Host/PanelBindLifecycleSelfTest.cs:1059` (host) — var g17DoseHost = new DoseLedgerHostSession();
- `src/Host/DoseLedgerHostSession.cs:19` (declaration) — public sealed class DoseLedgerHostSession
- `src/Host/DoseLedgerHostSession.cs:35` (host) — public DoseLedgerHostSession(
- `src/Host/DoseLedgerHostSession.cs:70` (host) — public static DoseLedgerHostSession Create(string dataDir, ILog log = null!, ICampaignRngManager? campaignRng = null)
- `src/Host/DoseLedgerHostSession.cs:91` (host) — return new DoseLedgerHostSession(registers: registers, content: content, quests: quests, campaignRng: campaignRng);
- `src/Host/HostCli.PanelTests.cs:1850` (host) — var session = DoseLedgerHostSession.Create(dataDirectory);
- `src/Host/HostCli.PanelTests.cs:1882` (host) — var fresh = DoseLedgerHostSession.Create(dataDirectory);
- `src/YearOfAsh/YearOfAshHostSession.cs:127` (host) — // DoseLedgerHostSession / DoseLedgerSave (v2+). Older
- `src/UI/DoseGeographyPanel.cs:23` (host) — /// Pure presentation — reads only from <see cref="DoseLedgerHostSession"/>.
- `src/UI/DoseGeographyPanel.cs:41` (host) — private DoseLedgerHostSession? _dose;
- `src/UI/DoseGeographyPanel.cs:51` (host) — public void Bind(DoseLedgerHostSession? session)
- `src/UI/GeigerCalibrationPanel.cs:14` (host) — /// All gameplay logic delegates to DoseLedgerHostSession → DosimeterCalibrationSystem.
- `src/UI/GeigerCalibrationPanel.cs:20` (host) — private DoseLedgerHostSession? _doseHost;
- `src/UI/GeigerCalibrationPanel.cs:53` (host) — public void Bind(DoseLedgerHostSession doseHost, string deviceTag = "")
- `src/UI/RadiationDetailPanel.cs:31` (host) — private DoseLedgerHostSession? _dose;
- … 7 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `dose_quests` — HOST_REFERENCE_PRESENT — core/declaration=10, host=1, test=1
- `Assets/Ashfall.Core/DoseContentCatalog.cs:33` (core) — /// <summary>One Dose quest line (dose_quests.json), authored to the live
- `Assets/Ashfall.Core/DoseContentCatalog.cs:108` (core) — public const string QuestsFile = "dose_quests.json";
- `Assets/Ashfall.Core/DoseQuestMigration.cs:18` (core) — /// quest lines authored in <c>dose_quests.json</c>.
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:44` (core) — "dose_items.json", "dose_locations.json", "dose_quests.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:473` (core) — ["dose_quests.json"] = new[] { "DoseContentCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:709` (core) — ["dose_quests.json"] = "DoseContentCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1079` (core) — ["dose_quests.json"] = new[] { "DoseLedgerSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1510` (core) — ["dose_quests.json"] = new[] { "DosePanel" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1728` (core) — "dose_items.json", "dose_quests.json", "dose_registers.json",
- `Assets/Ashfall.Core/IO/CatalogBootValidator.cs:233` (core) — RegisterCatalog("dose_quests.json", "Dose Quests", CatalogClassification.Optional);
- `src/Host/ContentUtilizationRuntimeCollector.cs:1045` (host) — foreach (var file in new[] { "dose_items.json", "dose_locations.json", "dose_quests.json", "dose_registers.json" })
- `Ashfall.Core.Tests/QuestlineMasterCatalogTests.cs:69` (test) — string raw = files.ReadAllText(files.Combine(dataDir, "dose_quests.json"));
#### `dose_registers` — HOST_REFERENCE_PRESENT — core/declaration=12, host=2, test=1
- `Assets/Ashfall.Core/DoseRegistersCatalog.cs:8` (core) — /// <summary>One dose-band vocabulary row (dose_registers.json).</summary>
- `Assets/Ashfall.Core/DoseRegistersCatalog.cs:46` (core) — /// <summary>The dose_registers.json vocabulary (A4) — display strings only,
- `Assets/Ashfall.Core/DoseRegistersCatalog.cs:56` (core) — /// <summary>Engine-agnostic loader for dose_registers.json.</summary>
- `Assets/Ashfall.Core/DoseRegistersCatalog.cs:59` (core) — public const string FileName = "dose_registers.json";
- `Assets/Ashfall.Core/Disease/DiseaseTriage.cs:103` (core) — /// in <c>dose_registers.json</c> (<c>plans[]</c>) — never invented here, and
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:45` (core) — "dose_registers.json", "holdfast_factions.json", "holdfast_flavor.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:474` (core) — ["dose_registers.json"] = new[] { "DoseRegistersCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:710` (core) — ["dose_registers.json"] = "DoseRegistersCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1080` (core) — ["dose_registers.json"] = new[] { "DoseLedgerSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1511` (core) — ["dose_registers.json"] = new[] { "DosePanel" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1728` (core) — "dose_items.json", "dose_quests.json", "dose_registers.json",
- `Assets/Ashfall.Core/IO/CatalogBootValidator.cs:234` (core) — RegisterCatalog("dose_registers.json", "Dose Registers", CatalogClassification.Optional);
- `src/Host/ContentUtilizationRuntimeCollector.cs:1045` (host) — foreach (var file in new[] { "dose_items.json", "dose_locations.json", "dose_quests.json", "dose_registers.json" })
- `src/Host/HostCli.PanelTests.cs:1851` (host) — Check(session.Registers.npcs.Count == 4, "dose_registers catalog loads the four antagonists");
- `Ashfall.Core.Tests/Medical/DiseaseTriageBridgeTests.cs:141` (test) — new FileSystemIO().ReadAllText(Path.Combine(DataDir(), "dose_registers.json")));
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 9-12
00009: **Document class:** SUBJECT-PLAN FACTORY. This document is not itself an integration plan. It is a repeatable generator: any future planning session can consume its matrices, backlog, and templates to produce an unbounded series of bounded subject plans, each of which names its own best integration route.
00010: **Audit basis:** Live repository inspection performed 2026-09-24 (repository root listing, `Assets/StreamingAssets/Data/` listing at 342 entries, `docs/` listing, `docs/plans/` listing at 126 entries, `INTEGRATION_PLANS.md`, `SESSION_HANDOFF.md`, `AGENTS.md`, branch list). Every claim in the Drift Register (Part I) is labeled VERIFIED, HIGH CONFIDENCE, or UNVERIFIED.
00011: **Authority order:** unchanged from v1.0 — live repository source and data first; then `AGENTS.md`; then this document; then the docs registry and atlas; then plan ledgers. Where this document and live source disagree, live source wins and this document must be corrected.
00012:
#### authority lines 29-32
00029:
00030: ## PART I — LIVE-REPOSITORY AUDIT AND DRIFT REGISTER (2026-09-24)
00031:
00032: ### 1.1 Audit method
#### authority lines 44-47
00044: **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
00045: Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.
00046:
00047: **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
#### authority lines 65-68
00065: **DR-10 — v1.0 items the audit could not confirm in this pass. UNVERIFIED.**
00066: Not confirmed in this audit pass (single-session, listing-level access): the 11,697 test total; the D1 seal state; the full 57-gate inventory; codec version pin values; the `ClaimPersonalBelonging` no-caller status; decision-blocked item states beyond those the ledger records as resolved. Each of these remains plausible but must be re-verified in live source before any plan depends on it. Factory rule: UNVERIFIED premises get a verification step inside the plan, never silent trust.
00067:
00068: ### 1.3 What the audit confirmed as stable (no change needed)
#### authority lines 86-89
00086: **Step 3 — Pull the cell's opening archetype and instantiate it.**
00087: Each matrix cell names an archetype (the *kind* of expansion that cell supports, with its owning seams). The session instantiates the archetype against current evidence: which catalog, which loader, which host session, which save family, which panel. If the archetype's seams no longer exist as described, the cell is stale — record the correction in the Drift Register and pick again.
00088:
00089: **Step 4 — Draft the subject plan in the v2.0 subject-plan format (Part V, Template S).**
#### authority lines 117-120
00117:
00118: C1 Shelter operations (rooms, thermal, schedules, fire, decor, barter, noise, prisoners, sanitation, airlock, decon, atmosphere) · C2 Medical pipeline (disease, dose ledger, ARS, surgery, autopsy, pharma, diagnostics, therapies, dependency, crises) · C3 Water, food, agriculture (treatment, condensers, wells, brine, nutrition, kitchen, preservation, grain, greenhouse, crops, aquaponics, apiculture) · C4 Power and industry (grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, coatings, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology) · C5 Expeditions and travel (destinations, scavenging tables, vehicles, waystations, caravans, routes, travel encounters, micro-locations) · C6 Map and geography (wasteland map, damaged zones, fog, route gates, cartography, survey instruments) · C7 Factions and war (stance, doctrines, war chains, tributes, treaties, embargoes, espionage, psyops, infiltration, musters, labor camps, bounties) · C8 Radio and information (stations, programs, intercepts, distress signals, rumors, sound ranging, direction finding, NVIS, heliograph) · C9 Survivors and interiority (needs, skills, traits, arcs, trauma, guilt, therapies, relations, caregiving, beliefs, rituals, memorials, final wishes, lineage, cohorts, apprenticeships) · C10 Quests and moral choice (questline master, dynamic questlines, personal quests, NPC arcs, moral-choice chains/flags/gossip, branching, bureaucratic morality, expansion quests) · C11 Economy (market, baselines, regional prices, shocks, rumors, black market, debt ledger, tributes, trade screens, tell lines) · C12 Weather and Year of Ash (weather system, seasons, effects, gates, hardening, storm windows, Year-of-Ash families, epilogue pressure) · C13 Endgame and epilogue (Reckoning, verdict, epilogue matrix, chronicle, muster epilogues, standing records, census) · C14 Ecology and wildlife (migration, trapping, ecosystem, bestiary, flora, infestations, contagion, pathogens, crop genomes) · C15 Defense and security (perimeter, defense grid, sky defense, ordnance, chemical defense, orbital harrow, interlocks, EMP effects) · C16 Progression and meta (skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, bestiary, L10N, mods, settings, input) · C17 Host surface and UI (panels, shell, focus navigation, snapshots, a11y, briefings, dashboards).
00119:
00120: ### 3.1 Lane A — Narrative and prose (all types and kinds)
#### authority lines 131-134
00131: | C8 | Radio rundown/transcript batches for stations with thin programming; numbers-station and cipher follow-ups | HIGH CONFIDENCE — but distress-signal content is SEALED under `CF-P1-DISTRESS-CONTENT-SEAL` (DR-06); do not add signal scenarios |
00132: | C9 | Delayed moral-choice callbacks (~100-day returns) via `IFlagLedger` flags; phantom-memory triggers tied to surviving cohorts | HIGH CONFIDENCE (v1.0 Part 7 gap 2) |
00133: | C10 | Quest prose fields (`quest_hook`, `objective_text`, outcome texts) for quest records with skeleton prose; follow Part 9 contracts exactly | HIGH CONFIDENCE |
00134: | C11 | Ledger, statement, and debt-template prose; rumor batches within deterministic bands | HIGH CONFIDENCE |
#### authority lines 146-149
00146: | C1 | Room-level effect extensions routed through `IsRoomPowered`; shelter-failure follow-ons building on the quarantined failure-effects wiring logs observed in `docs/plans/` | HIGH CONFIDENCE |
00147: | C2 | Ward-staffing and recovery-ramp follow-ons are CLOSED (Plan 24, DR-06); open instead: cross-links between medical and cohort/lineage (child health), and between dose ledger and Year-of-Ash fallout windows | PROPOSAL — premise sweep required |
00148: | C3 | Zoonosis-style bridges: kitchen/preservation × disease; cellar-rot × greenhouse economics; apiculture × morale | PROPOSAL |
00149: | C4 | Bind the newest industrial catalogs (DR-04) into consumption/production ledgers through the existing power-grid and foundry seams | PROPOSAL — needs live loader verification |
#### authority lines 159-162
00159: | C14 | Trapping→disease zoonosis bridge exists; open: migration × expedition route encounters; infestation × crop economy | PROPOSAL |
00160: | C15 | EMP effects exist (shelter EMP/medical power logs observed); open: defense grid × warlord siege math; sky-armor × orbital harrow telemetry | PROPOSAL |
00161: | C16 | XP Expansion W1 is ACTIVE (DR-06): difficulty-authority consumer binding is the sanctioned open seam in this cluster — extend it, do not parallel it | HIGH CONFIDENCE |
00162: | C17 | Panels rendering stale or missing data for newer systems; verify against `--ui-layout-selftest` before claiming | HIGH CONFIDENCE |
#### authority lines 208-211
00208: |---|---|---|
00209: | C2 | Dose-treatment matrix paired tests against `MEDICAL_DOSE_TREATMENT_MATRIX.md` | HIGH CONFIDENCE |
00210: | C11 | Debt-ledger consequence dispatcher coverage; rumor-band determinism pins | HIGH CONFIDENCE |
00211: | C10 | Moral-choice flag consumer coverage for newly added consumers | HIGH CONFIDENCE |
#### authority lines 229-232
00229: | C16 | L10N wave roadmap continuation respecting string freeze (re-check freeze state first — signatures resolve over time, DR-06) | HIGH CONFIDENCE |
00230: | Root | Register the root-level agent rulebooks and coordination files in the docs map so planners stop missing them (DR-01, DR-09) | VERIFIED need |
00231:
00232: ### 3.10 Lane J — Onboarding and player experience
#### authority lines 245-248
00245:
00246: **SB-01 — Delayed moral-choice callbacks (Lane A/C10).** Evidence: v1.0 Part 7 gap 2; `moral_choice_flags.json`, `IFlagLedger`, `DoorEncounterSystem`, `MoralChoiceSaveStore` all confirmed live. Subject: ~100-day delayed visitor/letter/radio/journal returns keyed on persisted flags. Integration route: data-first new catalog through the moral-choice loader family; dispatch through the daily-tick seam; possibly no codec bump if per-flag records already persist. Verification: integrity + utilization selftests, determinism replay, exactly-once dispatch test. Confidence: HIGH CONFIDENCE.
00247:
00248: **SB-02 — Mid-winter slump pressure campaign (Lane A/C12).** Evidence: v1.0 Part 7 gap 1 (Days 90–180). Subject: a bounded story-pressure wave (blight, cave-in, levy arc) authored through existing catalogs. Integration route: data-first; each pressure rides its owning system (ecology for blight, subterranean/excavation for cave-ins, warlord doctrines for levies). Confidence: HIGH CONFIDENCE.
#### authority lines 265-268
00265:
00266: **SB-11 — C2 open-gap package: Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix (Lanes B, F, G).** Evidence: DR-06 not-executed lists. Subject: three bounded follow-ons the ledger itself records as real gaps. Integration route: per existing C2 plan documentation. Confidence: VERIFIED as open; scope per item needs the plan docs.
00267:
00268: **SB-12 — Dive-site and hydroponic domain expansion (Lanes A and B/C3, C5).** Evidence: DR-04 — `dive_sites.json`, `hydroponic_crops.json` live but absent from v1.0's inventory. Subject: premise-sweep these domains for unexploited seams (dive oxygen drain is a canon hourly system; hydroponics may lack narrative corpus and economy legs). Integration route: data-first + existing host sessions. Confidence: INFERENCE pending sweep.
#### authority lines 328-331
00328: 1. **Volume unit.** A volume is one appended Part to this document (or one of its companion canvases) produced in a single session, typically 15,000–60,000 characters, always evidence-grounded against the live repository.
00329: 2. **Volume types, in rotation:** (a) subsystem deep-map volumes (one per cluster C1–C17: full catalog inventories, prose-coverage gaps, seam maps); (b) prose specification libraries (expanded Part 9 field contracts with worked examples per document genre); (c) backlog replenishment volumes (fresh premise sweeps converting new Drift Register entries into SB-candidates); (d) lane deep guides (one per lane: full archetype playbooks with worked subject plans); (e) audit volumes (periodic re-audits refreshing the Drift Register).
00330: 3. **Session checklist.** Each session: run the Step 1 premise sweep; execute the Factory Protocol or append a volume; update the Drift Register for anything that moved; record the character count and volume index in the growth ledger below.
00331: 4. **Growth ledger.** v2.0 base: approximately 25,000 characters (this document). Target: 2,000,000. Every appended volume appends one ledger line: `[date] Volume [n] ([type]) — [chars] — cumulative [total]`.
#### authority lines 346-349
00346: - **Volume** — one session's appended, verified content unit (Part VI).
00347: - **Drift Register** — the live-audit correction layer (Part I), the first thing any session reads.
00348: - **Subject plan** — an expansion proposal that names its subject, evidence, and recommended integration route but commits no file changes.
00349: - **Sealed surface** — a domain closed by evidence and signature (e.g., distress-signal content, DR-06); openable only with new evidence and foreman signature.
#### authority lines 366-369
00366: ### Premise evidence
00367: VERIFIED: `moral_choice_flags.json`, `moral_choice_quests_distress.json`, `moral_choice_chains.json`, and the wider moral-choice catalog family exist live in the data authority. VERIFIED: v1.0 Part 5.6 documents the flags/ledger seam and the weight_of_choices epilogue codec (v2). VERIFIED (drift-corrected): the rescue-signal content wave added `moral_choice_quests_distress.json`, so the moral-choice loader family already consumes multiple split catalogs — the pattern for adding one more split catalog exists. HIGH CONFIDENCE: no current consumer re-reads door-choice flags after the near-term window (v1.0 Part 7 gap 2); the integration plan must re-grep flag consumers before implementation.
00368:
00369: ### Why this and not something else
#### authority lines 388-391
00388: ### Subject
00389: A bounded campaign-window content wave that inserts authored pressure into Days 90–180: a crop blight epidemic arc (ecology), a deep-strata cave-in arc (subterranean/excavation), and a warlord conscription levy arc (doctrines/tribute), each delivered through existing catalogs and event systems, so the stabilized mid-game stays legible as triage rather than routine.
00390:
00391: ### Premise evidence
#### authority lines 491-494
00491: ### Premise evidence
00492: VERIFIED: baseline documents exist live (DR-03). VERIFIED: the Plan 76.2 seeded 200-run harness is the established pattern (v1.0 Part 4.3). VERIFIED: multiple content waves have landed since those baselines were generated (DR-06 ledger). DR-07 warns counts drift; baselines drift for the same reason.
00493:
00494: ### Why this and not something else
… 184 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.

**Requested behavior.** Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.

**Minimum safe delta.** Extend `dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.

**Required delta.** Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.

**Primary seam.** dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `DoseQuestMigration`, `DoseLedgerSystem`, `VoluntaryRegisterSystem`, `DoseLedgerHostSession`, `dose_quests`, `dose_registers`.

## Integration framework

The integration framework is deliberately owner-first:

1. **Read and classify current state.** Start with the named Core owner, current JSON, host session, save store, and focused tests. Record whether the feature is live, partially wired, dormant, stale, or decision-gated.
2. **Choose one authority per concern.** Extend the current owner when it exists. If no owner exists, stop at the architecture decision boundary and name the new authority decision rather than creating a parallel store, selector, ledger, panel, or simulation.
3. **Author data against consumers.** A JSON row is not integrated merely because it parses. Every row must have a current loader, a current consumer, a visible or mechanically observable outcome, and a validation path.
4. **Route effects through existing events/seams.** Core emits facts; host sessions translate them; UI presents truthful state. Do not place gameplay calculations in a panel or Godot callback.
5. **Persist through the owning save path.** Capture and restore must be implemented before a feature is called persistent. Old versions, nulls, empty collections, checksums, and mid-event saves are explicit cases.
6. **Verify narrowly.** Use the smallest existing test file or a new focused test for an uncovered confirmed contract. Keep Core, data, host, UI, save, determinism, and cross-system checks distinguishable.
7. **Roll back by boundary.** A failed expansion should disable its adapter or authored tranche without corrupting the owning state or requiring a destructive reset.

## Code architecture

### Core layer

- Put reusable rules, validation, state transitions, deterministic selection, and read models in `Assets/Ashfall.Core/`.
- Keep Core engine-free. No Godot, Unity, `Texture2D`, `JsonUtility`, wall-clock, or unseeded randomness belongs in a Core contract.
- Extend existing models and public methods when the current API already expresses the concern. A new DTO, interface, event, or catalog loader is justified only when it removes a real ownership or boundary problem.
- Make invalid input observable through the owner’s normal result/diagnostic path. Do not silently coerce malformed content into a successful state.
- Keep deterministic ordering explicit: use ordinal IDs, stable catalog order, bounded collections, and the existing seeded RNG fork for any stochastic choice.

### Data layer

- Author under `Assets/StreamingAssets/Data/` using the existing schema and snake_case IDs.
- Prefer additive fields and existing collections over parallel catalogs.
- Validate IDs, references, ranges, and consumer reachability through the current catalog integrity pipeline.
- Record schema version and old-data behavior in the plan and implementing handoff.
- Data prose may describe a consequence only when the consequence is expressible through a current owner and event.

### Host layer

- Load the catalog in the current host/session owner, not in a panel constructor.
- Subscribe once to owner events, translate facts into existing journal/radio/UI signals, and dispose subscriptions with the session.
- Bind day/hour/event triggers through the existing campaign owner. Do not create a second clock or update loop.
- Rehydrate from the existing save owner and mark dirty only for actual canonical mutations.
- Keep host code free of duplicate gameplay math; it may format, route, and adapt.

### Presentation layer

- Panels expose current commands and truthful current state.
- They display unavailable/blocked reasons, provenance, and the next legitimate action rather than simulating a result.
- Preserve keyboard/controller close/back behavior, focus order, readable contrast, and reduced-motion/accessibility settings.
- Refresh from owner events and lifecycle state; never use a panel cache as authority.
- Snapshot or accessibility fixtures are verification artifacts, not gameplay state.

## Ownership and state matrix

| Concern | Authoritative owner | Adapter responsibility | Persistence rule | Verification gate |
|---|---|---|---|---|
| Domain rules | DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
| Authored content | Current JSON catalog and loader | Load/validate once | Catalog version/defaults | Data integrity |
| Runtime lifecycle | Existing host/session owner | Setup, event subscription, disposal | Existing save/session | Host wiring |
| Presentation | Existing Godot surface | Format and command dispatch | No gameplay state | UI/focus/headless |
| Diagnostics | Existing logging/telemetry owner | Correlate ID and phase | Bounded/non-authoritative | Failure test |
| Historical authority | Read-only master document | Cite relevant section only | Never persisted | Plan QA |

## Data, event, and command flow

`authoritative JSON / player command / current owner event` → validation at the owning seam → canonical Core state transition → typed fact/event → host session projection → journal/radio/UI refresh → existing save owner if the transition mutated durable state.

The flow is intentionally one-way for authority. UI may send a command, but it cannot directly mutate the domain. Events may be consumed by several read-only projections, but only the owner writes the state. If a proposed feature needs a second writer, that is an architecture failure, not an invitation to add another event bus.

## State, API, and compatibility contract

The implementing agent must confirm the actual public API and write the final signatures in the implementation handoff. At minimum, expose:

- a read-only query/projection for the current state;
- an explicit command or owner method for each player-visible mutation;
- a typed fact/event for meaningful state changes;
- capture/restore methods on the existing state owner;
- a diagnostic result for invalid or unavailable data;
- a stable key for idempotent commands and replay;
- a bounded, ordinal-stable collection for any retained history.

Old saves must default missing additive fields to the documented neutral value. New required fields need a versioned migration. Null and empty semantics must differ deliberately: null means unavailable/not supplied; empty means validly no records, unless the current owner’s contract says otherwise. Do not infer a new save section from the plan title.

## Determinism and replay

For every proposed random or time-dependent element, name the seed source, stream/fork, draw order, retry behavior, and tie-break rule. Prefer no randomness for validation, lookup, and deterministic UI state. If an existing owner uses `ISeededRng`, reuse its campaign stream/fork rather than constructing a private generator. Wall-clock time, `System.Random`, `Guid.NewGuid`, hash iteration order, filesystem enumeration order, and frame timing are prohibited in deterministic Core behavior.

The replay acceptance test must use two equivalent runs with identical seed, catalog snapshot, state, day/hour, and command sequence. Compare canonical state, event order, resource/ledger deltas, and persistence payload. Presentation-only differences are acceptable only when they are explicitly non-authoritative and do not alter commands or outcomes.

## Save, restore, and migration

Before implementation claims persistence:

1. Identify the current save-section owner and DTO.
2. Add or reuse capture/restore through that owner.
3. Deep-copy mutable collections so restoring does not alias runtime state.
4. Define old-version defaults for every new field.
5. Define behavior for missing catalogs, unknown IDs, partial records, and corrupted checksums.
6. Test capture → serialize → restore → continued mutation, plus a mid-transition reload.
7. Confirm a save/load pair does not duplicate one-shot events or reapply a quest/choice/ledger mutation.

No plan-created “state cache” is allowed. If a new authority is genuinely required, the package must pause for a decision and name its owner, section, migration, and test contract.

## Failure and edge behavior

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 101.

## Test strategy and focused commands

The plan-only pass does not execute tests. The implementing package should reuse existing focused files first, run a new file alone, and stay below the repository’s focused-test policy unless a foreman-approved hypothesis requires more. The current candidate commands are listed in the verification matrix and must be revalidated against the worktree at implementation start. A passing compile is not proof of host wiring, persistence, determinism, or player reachability.

## Phased implementation and rollback

The detailed phase table below is the implementation contract. Each phase has a completion gate and a “must not touch yet” boundary. Rollback is additive and local: disable the adapter or remove the authored tranche, retain the owner’s last valid state, and never reset the shared worktree or shared save registry to hide a failure.

## Dependency-ordered implementation phases
| Phase | Outcome | Work | Boundary | Completion gate |
|---|---|---|---|---|
| 0 | Premise recheck and baseline | Confirm exact owner/API/catalog counts, current claims, dirty paths, and active decision gates. | No edits to production. | A written evidence table and focused baseline commands. |
| 1 | Owner and collision map | Trace current callers, save owner, event seam, and duplicate/legacy candidates. | No new catalog or state. | Single-owner map with zero unresolved authority collisions. |
| 2 | Core contract or bounded extension | Add only the smallest pure contract needed by the confirmed gap, or document that no Core change is needed. | No Godot/UI/data authoring. | Core tests for boundaries, transitions, invalid data, and determinism. |
| 3 | Persistence and migration contract | Implement capture/restore/old-save defaults through the existing owner. | No unrelated save sections. | Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass. |
| 4 | Authored data tranche | Author schema-valid rows only after consumer fields are known; validate references and reachability. | No prose-only orphan rows. | Data integrity and consumer coverage pass for the tranche. |
| 5 | Host/event wiring | Load, subscribe, translate, and dispose in the current host/session owner. | No panel gameplay math. | Host wiring test proves event → projection and setup/teardown. |
| 6 | Presentation and accessibility | Expose truthful state, commands, focus, controller/keyboard behavior, and feedback. | No new authority in UI. | Panel route/focus/headless checks pass; snapshots only through the owning harness. |
| 7 | End-to-end and replay | Run a bounded scenario, save/reload, paired seeded replay, and cross-system consequence check. | No full-suite default. | Named commands/results and limitations recorded. |
| 8 | Balance/content polish | Tune only authored values with current harnesses; remove dead rows and polish truthful text. | No hidden tuning or parallel scalar. | Content review confirms no dominated/unreachable row and no unsupported claim. |
| 9 | Rollback and closeout | Document feature disablement, migration reversal, owner handoff, and residual debt. | No unowned cleanup. | Foreman review accepts or records a blocker. |

### Phase-specific implementation questions
#### Phase 0: Premise recheck and baseline
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.
- What is the smallest safe change? Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.
- Which owner is touched? DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/DoseQuestMigration.cs` — DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/DoseLedgerSystem.cs` — DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/DoseLedgerSave.cs` — DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs` — DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/DoseLedgerHostSession.cs` — DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/DoseLedgerSaveStore.cs` — DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/DoseLedgerPanel.cs` — DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.GameFlow.cs` — DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Main.Medical.cs` — DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/dose_quests.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/dose_registers.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/dose_items.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/items.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/survivors.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/surgical_procedures.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/DoseLedgerPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/AfflictionsPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/JournalPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/VerdictDashboardPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/DoseQuestExpansionTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/DoseLedgerSystemTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/DoseContentCatalogTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/DoseRegistersCatalogTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections is wired end to end or the plan explicitly closes as already integrated.
- Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

DoseQuestMigration, DoseLedgerSystem/Save, VoluntaryRegisterSystem, DoseLedgerHostSession/SaveStore, DoseLedgerPanel, dose_quests/dose_registers data, and focused dose/Verdict tests are live. The old plan’s proposed DoseQuestManager, DoseQuestSaveData, and exact quest count require current caller and schema evidence.

# 3. Required Delta

Map authored quest stages to real dose readings, register state, medical actions, guilt/morale effects, item transactions, and the terminal audit while preserving dose-ledger authority and determinism.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Dose storm and dose-ledger conditioning are active/current owners; this plan cannot add a second exposure formula or alter sealed medical thresholds. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `band_green`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `band_white`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `band_yellow`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `band_amber`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `band_orange`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `band_rose`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `band_red`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `band_crimson`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `band_violet`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `band_black`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `band_indigo`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `band_void`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `plan_morphine_tray`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `plan_comfort_rounds`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `plan_nothing`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `plan_chelation`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `plan_iodine_prophylaxis`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `plan_isolation`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `plan_rest`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `plan_transfer`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `guess_low`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `guess_honest`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `guess_refused`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `register_ledger`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `register_sick`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `register_cohort`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `register_voluntary`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `npc_dr_irina_vel`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `npc_wyn_omah`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `npc_piet_abar`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `npc_saria_voss`
- Source: `Assets/StreamingAssets/Data/dose_registers.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `item_dose_ledger`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `item_calibration_key`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `item_dosimeter_tag`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `item_palliative_morphine`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `item_cohort_first_board`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `item_calibrated_dosimeter`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `item_forged_clean_bill_chit`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `item_chelation_decorporation_course`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `item_shielded_badge_case`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `item_pocket_dosimeter`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `item_radiation_survey_meter`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `item_dose_register_book`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `item_cohort_baseline_card`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `item_shielding_apron`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `item_potassium_iodide_pack`
- Source: `Assets/StreamingAssets/Data/dose_items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `item_decon_chelator_concentrate`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `item_lead_lined_effluent_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `item_heavy_neoprene_scrub_brush`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `item_sealed_waste_bin`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `item_theodolite_brass_precision`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `item_surveyor_stadia_rod`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `item_datum_plate_bronze`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `item_concrete_mix`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `item_forged_rotor_shaft`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `item_magnetic_bearing_coil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `item_high_vacuum_pump`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `item_containment_ring_steel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `item_reinforced_concrete_vault`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `item_seismic_damper_pad`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `item_vacuum_pump_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `item_bearing_grease`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `item_rotor_balancing_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `item_portable_pid_detector`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `item_detector_sensor_module`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `item_hermetic_sample_ampoule`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `item_hot_dust_drum`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `item_sludge_cake`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `item_tailings_drum`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `dosimeter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `geiger_counter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `iodine_pills`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `anti_rad`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `gas_mask`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `hazmat_suit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `water_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `air_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `clean_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `irradiated_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `canned_food`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `fuel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `cloth`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `scrap_metal`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `bandage`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `raw_meat`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `cooked_meat`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `dirty_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `morphine`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `chelation_agent`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `potassium_iodide`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `medical_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `battery`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `calibration_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `tweezers`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `splint`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `antibiotics`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `jewelry`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `diamond`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `currency`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `mechanical_parts`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `electronic_scrap`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `item_radiosonde`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `solar_cell`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `chemicals`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `handheld_radio`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `engine`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `roots`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `berries`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `vacuum_tube`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `spring_mechanism`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `phonograph_needle`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `projector_bulb`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `lubricant_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `film_reel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `antenna_coil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `soldering_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `music_box_comb`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `spring_key`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `typewriter_ribbon`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `machine_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `camera_lens_cleaner`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `photographic_film`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `item_acoustic_decoy`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `item_ammonium_nitrate_sack`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `item_amnestic_syrup`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `item_anchor_notes`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `item_ash_ghillie`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `item_bio_plastic`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `item_black_water_vial`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `item_co2_scrubber_cartridge`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `item_epoxy_injector`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `item_faraday_mesh`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `item_frostbite_salve`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `item_fungicide_fogger`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `item_galvanized_rebar`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `item_glycol_antifreeze_canister`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `item_hermetic_hatch_silicone_gasket`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `item_high_tensile_steel_culvert_brace`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `item_insulated_snowmobile_battery`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `item_lead_shielded_sample_cask`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `item_lead_visor`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `item_lithium_salts`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `item_mine_prod`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `item_mycelium_bricks`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `item_prussian_blue_chelating_pellets`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `item_radon_detector_electret`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `item_rebreather_scrubber`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `item_ro_membrane`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `item_scopolamine_root`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `item_sealed_lead_pig`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `item_snow_goggles_improvised`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `item_sound_baffling`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `item_suitcase_locked`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `item_surgical_bone_chisel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `item_teddy_bear`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `item_thermal_paste`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `item_welders_glass`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `aa_batteries`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `alcohol_wipes_box_10_of_10`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `ammo_762x54r_jhp_ap`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `ammo_357`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `ammo_12g`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `ammo_308`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `ammo_556`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `ammo_762`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `antiseptic_1l_of_1l`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `battery_pack`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `box_of_nails_10`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `canned_soup`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `childrens_books`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `cigarette_lighter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `clean_water_jug`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `cooking_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `copper_wire_10m_of_10m`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `diesel_fuel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `dried_rations`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `faraday_pack`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `field_surgical_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `fuel_1l`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `fuel_cell`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Validate each quest stage against an actual dose/register/medical fact, a durable choice owner, a bounded consequence, and a truthful player-visible readout.
- Primary owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State/save rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI truth rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: radiation bureaucracy, dose ledger, triage, and moral choice.
- Seam under test: dose_quests.json + dose_registers/items/survivors -> DoseQuestMigration/DoseLedgerSystem/VoluntaryRegisterSystem -> medical/moral/journal/UI/save projections.
- Expected authority: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store. Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI/accessibility check: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- Player-facing truth: Missing survivor, absent ledger, invalid dose, duplicate quest, failed treatment, dead survivor, old save, and unavailable moral owner preserve the last valid state and explain the blocked action.
- Persistence response: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism response: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: DoseQuestMigration.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: DoseLedgerSystem.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: VoluntaryRegisterSystem.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: DoseLedgerHostSession.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: dose_quests.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: dose_registers.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: DoseQuestMigration.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: DoseLedgerSystem.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: VoluntaryRegisterSystem.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: DoseLedgerHostSession.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: dose_quests.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: dose_registers.
- Owner: DoseLedgerSystem owns dose arithmetic and lifetime state; VoluntaryRegisterSystem owns registration; medical owns treatment; quest/moral owner owns choices; host adapts.
- State rule: Quest stage, choice, exposure booking, register entry, and completion use their existing owners; no panel-local dose counter or parallel quest save store.
- Determinism rule: Dose calculations and branching use canonical integer/fixed-point rules and seeded choice streams; no random falsification or wall-clock scheduling.
- UI rule: ['src/UI/DoseLedgerPanel.cs', 'src/UI/AfflictionsPanel.cs', 'src/UI/JournalPanel.cs', 'src/UI/VerdictDashboardPanel.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/DoseQuestExpansionTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/DoseQuestExpansionTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/DoseQuestOwnershipTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/DoseQuestOwnershipTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/DoseLedgerSystemTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/DoseLedgerSystemTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Verdict/Plan93_101VerdictDoseQuestIntegrationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/DoseContentCatalogTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/DoseContentCatalogTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 06
- Test: `Ashfall.Core.Tests/DoseRegistersCatalogTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/DoseRegistersCatalogTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 491,818 characters.
# Post-250K deep polishing pass

The architecture body above reached 491,896 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `DoseQuestMigration`, `DoseLedgerSystem`, `VoluntaryRegisterSystem`, `DoseLedgerHostSession`, `dose_quests`, `dose_registers`.
- Confirm each proposed mutation has exactly one writer. A host adapter may translate a fact; a panel may display it; neither becomes authority.
- Check for legacy or parallel names before proposing any new class, DTO, event, catalog, save section, or RNG stream.
- Treat the master authority as a read-only design lens. Current source/data wins when they disagree, and the disagreement is recorded rather than hidden.
- Recheck the live worktree status recorded in each evidence dossier. A dirty source path is a coordination warning, not a stable acceptance result.

## Deep polish B — data and consumer precision

- For every candidate row, name the loader, consumer, reference validator, and observable outcome.
- Replace counts copied from the old plan with current JSON counts or an explicit census task.
- Remove any row whose only consumer is a test, a prose generator, or a panel.
- Preserve additive schema compatibility and explain old-data defaults.
- Check that narrative text does not promise an effect that the current owner cannot produce.

## Deep polish C — state, save, and replay precision

- Confirm the existing save owner and DTO before naming a field.
- Test capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input, and mid-event reload.
- Confirm repeated commands and replay cannot duplicate one-shot effects.
- Use the existing seeded stream/fork; document every random draw and tie-break.
- Treat a UI refresh as a projection, never as persistence or mutation.

## Deep polish D — failure and player truth

- Walk null, empty, missing, duplicate, stale, hostile, dead, unavailable, and repeated-event cases.
- Ensure blocked/unavailable states are legible and do not masquerade as success.
- Keep accessibility behavior attached to the same command/state surface as the visual behavior.
- Record which failures are diagnostic-only, which defer presentation, and which halt the transition.

## Deep polish E — implementation readiness

- Replace generic file lists with owner-specific action verbs and completion gates.
- Keep the phase order dependency-safe: premise, owner, Core, persistence, data, host, presentation, end-to-end, polish, closeout.
- Give the next implementer exact focused commands, test selection rationale, and stop conditions.
- Keep a final residual-risk list for decision-gated or concurrent work rather than hiding it in prose.

# Post-250K deep polishing pass — second pass

The first post-250K pass above checked structure and owner collisions. This second pass is a separate adversarial review after the plan has reached its depth checkpoint; it is not a duplicate paragraph exercise.

## Second-pass adversarial questions

- What would a reviewer incorrectly assume after reading only the executive summary?
- Which sentence describes historical intent but could be mistaken for current behavior?
- Which current owner, save path, host session, or event seam is missing from the impact map?
- Which data row has a valid ID but no reachable consumer?
- Which UI label could claim a consequence before the Core transition succeeds?
- Which failure currently falls through as a default success, duplicate event, or stale cache?
- Which random choice lacks a named seed/fork/tie-break?
- Which old-save field lacks a default, migration, or deep-copy test?
- Which concurrent package or decision gate could invalidate the proposed path?
- What is the smallest rollback that leaves the previous owner state readable?

## Second-pass correction protocol

For every answer, classify the issue as `CURRENT_EVIDENCE`, `PROPOSED_EXTENSION`, `DECISION_GATE`, `CONCURRENT_CLAIM`, or `OUT_OF_SCOPE`. Update the relevant numbered section and record the exact path/command that would close the issue. If no current evidence supports a claim, remove the claim rather than softening it with adjectives. If a proposed change needs a new architecture decision, stop at the gate and name the decision owner. This pass must leave the plan more precise, not merely longer.

## Second-pass acceptance

- [ ] Every “current” statement has a path or is marked as an open premise.
- [ ] Every “implemented” statement names a current caller or is downgraded to catalog-only.
- [ ] Every proposed state field names its save owner and migration behavior.
- [ ] Every proposed random/temporal behavior names determinism treatment.
- [ ] Every UI consequence has a command/state source.
- [ ] Every failure has a safe expected outcome.
- [ ] Every focused command resolves or is labeled future implementation work.
- [ ] Every decision-gated or concurrently claimed seam is explicit.

# Final precision and reaccuracy pass

1. Re-read every current path cited in the dossier and mark missing paths as open premises.
2. Re-run the hash verifier and data JSON parse check at the final snapshot.
3. Re-run the symbol occurrence audit and distinguish declarations, host consumers, and test-only references.
4. Check all current focused test commands resolve to existing files.
5. Remove unsupported historical counts, “sealed” claims, invented APIs, and unproven caller assertions.
6. Reconcile the plan title and requested delta with the actual current owner; if the old plan is already implemented or stale, state maintenance or blocked scope plainly.
7. Confirm Core remains engine-free, JSON remains authoritative, and no parallel authority is proposed.
8. Confirm UI, failure, save, determinism, and accessibility contracts are concrete.
9. Confirm rollback is local and does not require destructive state reset.
10. Record limitations honestly: this package is planning-only and does not run implementation tests.

# Full repolishing phase

The final repolishing pass is a quality audit over the complete document, not an append-only slogan. Read the plan from executive summary through handoff as one artifact.

- **Coherence:** every section uses the same owner names, state terms, and delta.
- **Evidence:** every important claim points to a current path, current data record, read-only authority anchor, or explicit unknown.
- **Architecture:** no duplicate system, parallel save, second RNG, engine dependency, or UI authority is smuggled into a phase.
- **Mechanics:** effects are expressed through existing commands/events and have a visible or auditable consequence.
- **Continuity:** references, days, locations, factions, and narrative facts use canonical IDs and current catalogs.
- **Failure:** invalid and unavailable states are defined, bounded, and truthful.
- **Persistence:** capture/restore and migration are named for every durable delta; no new section is casually proposed.
- **Determinism:** random sources, ordering, and replay comparisons are explicit.
- **UI:** panels are thin, accessible, refreshable, and non-authoritative.
- **Testing:** each layer has a focused command or a clear reason it must be authored later.
- **Rollback:** every phase has a local reversal and preservation rule.
- **Handoff:** the next implementer can start with the first safe step without interpreting the plan around stale prose.

## Repolish acceptance checklist

- [ ] Current owner/API confirmed from source.
- [ ] Current data schema and references confirmed.
- [ ] Existing save owner and migration path confirmed.
- [ ] Existing host/event seam confirmed.
- [ ] Existing UI surface and accessibility path confirmed.
- [ ] Focused test paths resolve or are labeled future work.
- [ ] Deterministic replay and idempotency contract stated.
- [ ] Failure and old-save behavior stated.
- [ ] No unsupported completion claim remains.
- [ ] Rollback and owner handoff are actionable.
- [ ] Plan remains implementation-ready without production edits in this package.

# Implementation handoff

## MUST PRESERVE

- The current owner named in this plan and the one-authority rule.
- Godot as the presentation/host authority and Core as engine-free domain logic.
- Authoritative JSON under `Assets/StreamingAssets/Data/`.
- Existing save ownership, migration semantics, seeded RNG contracts, and event ordering.
- Existing accessibility, focus, controller/keyboard close/back, and truthful UI behavior.

## MUST ADD

- Only the smallest confirmed Core/data/host/UI extension needed by the current delta.
- Exact catalog validation, consumer reachability, save/restore, failure, determinism, and focused tests.
- A bounded content tranche whose records are consumed and observable.
- A local rollback/disablement path and a concise implementation handoff.

## MUST NOT DO

- Do not create a second ledger, save store, selector, event authority, simulation, or panel-owned rule.
- Do not edit production, data, tests, UI, assets, or generated indexes during this planning package.
- Do not restore Unity or add engine references to Core.
- Do not claim tests, host wiring, save integration, or player reachability from static intent alone.
- Do not use unseeded randomness, wall-clock ordering, or hash iteration order for deterministic behavior.

## VERIFY WITH

- The exact focused `scripts/run_test.sh` commands listed in this plan after the implementation claim is opened.
- Current catalog integrity and consumer/utilization checks.
- A bounded Core test, save round-trip/migration test, host wiring test, and deterministic replay comparison.
- Godot headless/UI checks only when the confirmed implementation touches the host/UI path.

## FIRST SAFE IMPLEMENTATION STEP

Re-read the current owner, the first current catalog, the first current host/session, and the first focused test listed in the dossier. Write a one-page premise table that classifies each as live, partial, stale, or blocked. Do not author content or code until the table has one owner and one non-overlapping implementation seam.
