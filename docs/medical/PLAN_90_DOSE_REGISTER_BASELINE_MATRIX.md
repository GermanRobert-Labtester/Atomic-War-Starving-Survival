# PLAN 90 — DOSE REGISTER BASELINE MATRIX

> Plan 90 / Task 90B. The current content of `Assets/StreamingAssets/Data/dose_registers.json`
> recorded as stable baseline. No IDs, thresholds, labels, or notes were changed.

---

## 90B.1 — Current bands (4)

| ID | Label | Threshold mSv | Disposition | Referenced elsewhere? |
|---|---|---:|---|---|
| `band_green` | Green | 0 | "No measurable burden. Walk the corridor." | `DoseRegistersCatalog.BandIdFor` case 0; `DoseLedgerSystem.GetAdministrativeBand` string-compare; `DoseLedgerSystem.BandGreen=0`; `DoseBandResult.Green`; test `BandLabel_MapsCoreBandsToVocabulary` |
| `band_amber` | Amber | 100 | "The ledger shows a number worth watching." | `BandIdFor` case 1; `GetAdministrativeBand`; `BandAmber=1`; `DiseaseTriage` Ill→Amber; `CohortSystemTests` (`CorrectBaseline … "band_amber"`) |
| `band_red` | Red | 300 | "Named on the sick list. Care is a choice, not a cure." | `BandIdFor` case 2; `GetAdministrativeBand`; `BandRed=2`; UI demos (`DiagnoseDemo(DoseLedgerSystem.BandRed)` in `Main.Phase0.cs:303`, `HostCli.PanelTests.cs:1202`, `Main.UiTests.Dose.cs:44`); `CohortSystemTests`; `docs/bodymind/DOSE_INSTITUTION_CONSEQUENCE_MATRIX.md` |
| `band_black` | Black | 600 | "The band the registrar will not soften. Still on the roster." | `BandIdFor` case 3; `GetAdministrativeBand`; `BandBlack=3`; `DoseLedgerSystemState.ceilingMsv=600`; test `Load_BandThresholdsBind` (`bands[3]=600`) |

Existing dispositions already respect the medical-tone guardrail: none claims
automatic death from cumulative mSv. Black's "the registrar will not soften" is
institutional voice, not a prognosis. **No tone correction required** (Task 90E
result: preserve all four verbatim).

## 90B.2 — Current plans (3)

| ID | Label | Cost | Note | Referenced elsewhere? |
|---|---|---|---|---|
| `plan_morphine_tray` | Morphine tray | `"morphine"` (**unresolved — no such item in items.json**) | "Mercy with a schedule. The tray is refilled on the same day every week." | `DiseaseTriage.Plans.MorphineTray`; auto-assigned for terminal lethality ≥ 0.5 (`DiseaseTriage.PalliativePlanFor`); `SickListSystemTests` (assign + save round-trip); `DiseaseTriageBridgeTests:224`; demo button `DoseRegisterSurface.OnAssignMorphine`; `Main.UiTests.Dose.cs:45` |
| `plan_comfort_rounds` | Comfort rounds | `"time"` | "Someone sits the night. The sick room has a chair for it now." | `DiseaseTriage.Plans.ComfortRounds`; auto-assigned for terminal lethality < 0.5 |
| `plan_nothing` | Nothing | `"none"` | "A refusal is a choice the ledger records as silence." | **No code reference** — selectable-in-principle vocabulary only |

## 90B.3 — ID preservation verdict

All 4 band IDs and 3 plan IDs are load-bearing:

- 4 band IDs: pinned by `BandIdFor` switch, `GetAdministrativeBand` string
  compares, the `DoseBandResult` enum, `DiseaseTriage` stage mapping, and the
  `BandLabel_MapsCoreBandsToVocabulary` test.
- 2 of 3 plan IDs: pinned by `DiseaseTriage.Plans` constants and consumed by the
  auto-palliative writer. `plan_nothing` is unreferenced but is the shipped
  "no intervention" option.

**Preserved. Zero renames.**

## 90B.4 — Threshold preservation verdict

The 0 / 100 / 300 / 600 anchors are not just data — they are **compiled** into
`DoseLedgerSystem` (`AmberMsv=100f`, `RedMsv=300f`, `BlackMsv=600f`,
`DoseLedgerSystem.cs:60-62`) and re-pinned by `DoseLedgerSystemTests` boundary
assertions and `Load_BandThresholdsBind`. Thresholds preserved trivially (no
data change shipped at all — see closeout, mode BLOCKED).

## 90B.5 — Remaining catalog sections (unchanged)

| Section | Count | Notes |
|---|---:|---|
| `guesses` | 3 | `guess_low/honest/refused`; pencil vocabulary for the cohort board; decoupled from band ladder |
| `calibration` | 1 | **not deserialized** (no DTO field); runtime calibration is code-side (40-reading cycle) |
| `registers` | 4 | `register_ledger/sick/cohort/voluntary`; **not deserialized** |
| `npcs` | 4 | `npc_dr_irina_vel`, `npc_wyn_omah`, `npc_piet_abar`, `npc_saria_voss`; pinned by two tests; also mirrored in `characters.json` |

NPC dispositions reference registers and honesty-about-drift themes, never band
counts or thresholds. No "four bands" text anywhere in data (grep verified).
