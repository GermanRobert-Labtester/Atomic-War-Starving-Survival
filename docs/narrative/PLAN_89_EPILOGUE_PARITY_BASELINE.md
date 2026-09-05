# Plan 89 — Epilogue Parity & Forensic Baseline

> **Catalog Authority:** `Assets/StreamingAssets/Data/muster_epilogues.json`
> **Selection Authority:** `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`
> **Host Consumer:** `src/Host/MusterHostSession.cs` & `src/Main.Muster.cs`
> **Presentation Chronicle:** `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs`

---

## 1. Forensic Reconciliation of Baseline

Before Plan 89, `muster_epilogues.json` contained **12 verified outcomes**:
- 4 Muster core approach endings (`the_open_muster`, `the_amnesty`, `the_corridor`, `the_blood_price`)
- 2 Coastal Hydro-Barons water war endings (`the_rate_card_revised`, `the_administrator`)
- 2 Cold Count provenance research endings (`the_measured_truth_contested`, `the_measured_truth`)
- 1 Uninvestigated fallback ending (`unwritten`)
- 3 The Verdict census outcomes (`ending_verdict_the_sector_recounts`, `ending_verdict_the_count_is_held`, `ending_verdict_the_offer_is_a_lease`)

### Architectural Discovery
1. **`EpilogueMatrix.cs` Topology:**
   The file previously contained `EndingDefinition` and `EpilogueMatrixLoader`. The selection logic was historically dispersed across questline approach selection in `MusterSystem.cs` and `VerdictEndingEvaluator.cs`.
   Plan 89 introduced the canonical, deterministic `EpilogueMatrix` evaluator and `EpilogueMatrixInput` into `EpilogueMatrix.cs` to resolve multi-system campaign outcomes under an authoritative precedence ladder.
2. **Catalog Integrity Registry:**
   `CatalogIntegrityValidator.cs` maintained `"ending_id"` in `DefinitionKeys`, but did not include `"ending_key"`. Because `ending_` was in `KnownPrefixes`, new entries with prefix `ending_` were treated as unresolved references. Adding `"ending_key"` to `DefinitionKeys` established `muster_epilogues.json` as a valid definition authority for ending keys.

---

## 2. Original 12 Epilogues Parity Table

| # | Ending Key | Title | Category | Words | Sentences | Source / Selection Path | Reachable |
|---|---|---|---|---|---|---|---|
| 1 | `the_open_muster` | The Open Muster | Muster Core | 49 | 4 | `MusterSystem` QuestApproach.B (`quest_the_muster`) | Yes |
| 2 | `the_amnesty` | The Amnesty | Muster Core | 47 | 3 | `MusterSystem` QuestApproach.A (`quest_the_muster`) | Yes |
| 3 | `the_corridor` | The Corridor | Muster Core | 55 | 3 | `MusterSystem` QuestApproach.C (`quest_the_muster`) | Yes |
| 4 | `the_blood_price` | The Blood Price | Muster Core | 46 | 4 | `MusterSystem` QuestApproach.D (`quest_the_muster`) | Yes |
| 5 | `the_rate_card_revised` | The Rate Card, Revised | Hydro Barons | 52 | 3 | `MusterSystem` QuestApproach.A/B/D (`quest_the_rate_card_war`) | Yes |
| 6 | `the_administrator` | The Administrator | Hydro Barons | 49 | 3 | `MusterSystem` QuestApproach.C (`quest_the_rate_card_war`) | Yes |
| 7 | `the_measured_truth_contested` | The Measured Truth, Contested | Cold Count | 46 | 3 | `MusterSystem` QuestApproach.B (`quest_four_names_on_the_roster`) | Yes |
| 8 | `the_measured_truth` | The Measured Truth | Cold Count | 44 | 4 | `MusterSystem` QuestApproach.A (`quest_four_names_on_the_roster`) | Yes |
| 9 | `unwritten` | Unwritten | Fallback | 51 | 3 | Uninvestigated / unresolved campaign state | Yes |
| 10 | `ending_verdict_the_sector_recounts` | The Sector Recounts | The Verdict | 49 | 3 | `VerdictEndingEvaluator` (Count presented & honored) | Yes |
| 11 | `ending_verdict_the_count_is_held` | The Count Is Held | The Verdict | 51 | 4 | `VerdictEndingEvaluator` (Count held / declined) | Yes |
| 12 | `ending_verdict_the_offer_is_a_lease` | The Offer Is a Lease | The Verdict | 51 | 2 | `VerdictEndingEvaluator` (Count converted to lease) | Yes |

All 12 original entries remain word-for-word byte preserved.
