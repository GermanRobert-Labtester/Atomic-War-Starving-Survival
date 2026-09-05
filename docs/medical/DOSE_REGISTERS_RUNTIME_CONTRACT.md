# DOSE REGISTERS RUNTIME CONTRACT

> Plan 90 / Task 90A. Evidence-based contract for how `dose_registers.json` is
> consumed. Every claim carries a file:line reference. Read-only audit — no code
> or data was changed.

---

## 1. Data file

- **Path:** `Assets/StreamingAssets/Data/dose_registers.json`
- **Loader:** `Assets/Ashfall.Core/DoseRegistersCatalog.cs` — `DoseRegistersCatalogLoader.Load(dataDir, fileIO, json)`
- **Serializer:** injected `IJsonSerializer` (core `SystemTextJsonSerializer`); snake_case JSON keys bind to snake_case DTO fields (pinned by `DoseRegistersCatalogTests.Load_FindsTheFourAntagonists` — `action_label` binding parity).
- **Failure mode:** missing file, empty file, or parse exception → empty catalog + `CatalogDiagnostics.Warn` (`DoseRegistersCatalog.cs:55-60`). Never throws.

## 2. DTO structure (`DoseRegistersCatalog.cs:9-53`)

| DTO | Fields | Notes |
|---|---|---|
| `DoseBandDef` | `id, label, threshold_msv (float), disposition` | `threshold_msv` is **display data only** — never read by selection logic (§4) |
| `DosePlanDef` | `id, label, cost (string), note` | `cost` is a freeform string, **never consumed** (§7) |
| `DoseGuessDef` | `id, label, pencil (bool), note` | narrative pencil-guess vocabulary; **not coupled to band ladder** (§9) |
| `DoseRegisterNpcDef` | `id, name, register, disposition, action_label, action` | the four register antagonists |
| `DoseRegistersCatalog` | `bands, plans, guesses, npcs` (lists) | note: the JSON `calibration` and `registers` blocks have **no DTO fields** — they are loaded and silently dropped by deserialization (verified: no `calibration`/`registers` property on the catalog class) |

**No validators.** The loader performs zero count, ordering, uniqueness, or
threshold validation. Whatever JSON parses is what ships.

## 3. Band lookup algorithm — `DoseLedgerSystem.BandFor` (`DoseLedgerSystem.cs:173-178`)

```csharp
public static int BandFor(float mSv)
{
    if (mSv >= BlackMsv) return BandBlack;   // 600
    if (mSv >= RedMsv)   return BandRed;     // 300
    if (mSv >= AmberMsv) return BandAmber;   // 100
    return BandGreen;                        // 0
}
```

- **Model:** highest hardcoded threshold ≤ cumulative dose.
- **Threshold inclusivity:** `>=` — a reading of exactly 100.0 mSv is Amber, 300.0 is Red, 600.0 is Black. 99.9 is Green.
- **Above top band:** any value ≥ 600 is Black. `DoseLedgerSystemState.ceilingMsv = 600f` is stored (`DoseLedgerSystem.cs:47`) but `BandFor` uses the `BlackMsv` **constant**, not the state field.
- **The catalog's `threshold_msv` values are never consulted for selection.** They are vocabulary for display only. Changing JSON thresholds changes nothing at runtime.
- **Determinism:** pure static function of the dose. No RNG.

## 4. Band identity plumbing — the four-band hardcode

The integer band (0–3) is the currency of the whole system:

| Site | Mechanism | Reference |
|---|---|---|
| `DoseLedgerSystem.BandGreen/Amber/Red/Black` | `const int 0/1/2/3` | `DoseLedgerSystem.cs:63-66` |
| `DoseBandResult` enum | `NoEntry=-1, Green=0, Amber=1, Red=2, Black=3` — return of `BookReading` | `DoseLedgerSystem.cs:315-321` |
| `DoseRegistersCatalogLoader.BandIdFor(int)` | `switch` on 0–3 → `band_green/amber/red/black`, **default returns `band_green`** — any future band ≥ 4 silently mislabels as Green | `DoseRegistersCatalog.cs:103-112` |
| `DoseRegistersCatalogLoader.BandLabel(catalog, int)` | `BandIdFor(band)` → linear scan of `catalog.bands` by **id**, not index | `DoseRegistersCatalog.cs:97-101` |
| `GetAdministrativeBand` override parse | 4 literal string compares (`band_green/amber/red/black`); unknown override strings fall through to true dose band | `DoseLedgerSystem.cs:213-216` |
| `DiseaseTriage.SickBandFor` | illness stage → the same 4 constants (Ill→Amber, Terminal→Red, OutcomePending→Black) | `Disease/DiseaseTriage.cs:160-174` |
| `SickBand.band` | `int` saved in the sick-list section; comment pins `BandGreen..BandBlack` | `SickListSystem.cs:11` |
| `CohortSystem.trueBand` | opaque band-ID **string**, stored unvalidated | `CohortSystem.cs:13,80-86` |

**Key consequence:** because `BandLabel` matches by ID (not index), the four
existing IDs may sit anywhere in the JSON `bands` array. But band integers ≥ 4
can never be produced by `BandFor`, and `BandIdFor` would mislabel them as
Green anyway.

## 5. Sorting behavior

- Selection (`BandFor`) is threshold-constant-based — **file order is irrelevant**.
- `BandLabel` scans by ID — file order irrelevant.
- No code sorts `catalog.bands` by `threshold_msv`.
- JSON order is presentation-order only, and only if a future UI renders the table (none does today).

## 6. Save model

- **Dose ledger** (`DoseLedgerSystem.CaptureState`, `DoseLedgerSystem.cs:239-283`): saves `cumulativeMsv`, `baselineMsv`, `shieldingFactor`, `administrativeClassificationOverride` (band-ID **string**), `hasForgedCleanBill`, readings history. **Band is not saved — it is derived** from dose via `BandFor` on load. Preferred model confirmed.
- **Sick list** (`SickListSystem.CaptureState`): saves `band` as **int** (0–3 semantics pinned in the DTO comment) and `palliativePlan` as a string. A new band value ≥ 4 in this field would be loadable but meaningless to every consumer.
- **Cohort** (`CohortSystem`): saves `trueBand` string verbatim.
- Save-roundtrip determinism is pinned by `SickListSystemTests` and `DoseLedgerSystemTests`.

## 7. Care-plan execution model

Plans are **labels on a string field**, not behaviors:

1. `SickListSystem.AssignPalliative(survivorId, plan)` stores the plan string on `SickBand.palliativePlan` (`SickListSystem.cs:104-114`). It accepts **any** non-empty string — no catalog validation.
2. Auto-assignment: `DiseaseTriage.PalliativePlanFor` (`DiseaseTriage.cs:196-202`) returns **only** `Plans.MorphineTray` (terminal, lethality ≥ 0.5) or `Plans.ComfortRounds` (terminal, lower lethality), else `null`. This is the sole runtime writer of illness-sourced plans.
3. Display: `DoseRegisterSurface.PlanLabel` (`src/Dose/DoseRegisterSurface.cs:212-217`) scans `catalog.plans` by ID and falls back to the raw ID. **Dynamic** — a plan added to JSON renders if something assigns it.
4. **No code anywhere reads `DosePlanDef.cost`.** No item is consumed, no time charged, no event fired. The plan is a ledger inscription, not a treatment.
5. One manual writer: `DoseRegisterSurface.OnAssignMorphine` (`src/Dose/DoseRegisterSurface.cs:305-310`) hardcodes `"plan_morphine_tray"` (demo button).

**Do not assume plans reduce dose. They do not, anywhere.**

## 8. Cost grammar

`cost` is a freeform string with no parser. Existing data uses:

- `"morphine"` — **unresolvable**: no `morphine` item exists in `items.json` (verified by exact-ID grep). Pre-existing dead reference in shipped data.
- `"time"` — sentinel in active use (`plan_comfort_rounds`).
- `"none"` — sentinel in active use (`plan_nothing`).

Sentinels are honored only by reader convention; nothing enforces them.

## 9. Guesses & calibration

- Guesses (`guess_low/honest/refused`) are pencil-mark vocabulary for the **cohort chalk board**. `CohortSystem` guess bands are the strings `low/medium/high` (see `DoseRegisterSurface.GuessLabel`, `src/Dose/DoseRegisterSurface.cs:219-228`), **not** band IDs. Expanding the band ladder does not touch guesses.
- `CorrectBaseline` takes any band-ID string (`CohortSystemTests` use `band_red/amber/black`); opaque and unvalidated.
- The JSON `calibration` block (`key: dosimeter`, `drift_note`) and `registers` block are **not deserialized** — no DTO fields exist. Runtime calibration is `DoseLedgerSystem.Calibrate` + `readingsSinceLastCalibration` (40-reading cycle), fully code-side.

## 10. UI consumers (`src/`)

| Consumer | Use | Count-hardcoded? |
|---|---|---|
| `DoseRegisterSurface.RenderLedger` (`:159-161`) | `BandFor` + `BandLabel` per survivor row | renders current band only — **no band table** |
| `DoseRegisterSurface.RenderSick` (`:176-192`) | `BandLabel(b.band)` + `PlanLabel` | dynamic list |
| `DoseLedgerHostSession.LedgerLine` (`:232-238`) | prints `[band N]` integer | count-agnostic |
| `GeigerCalibrationPanel._errorBandLabel` | **unrelated** — device error band, not dose register | — |

**UI capacity verdict:** the UI renders only the *current* band and assigned
plan. Nothing iterates all bands. Twelve bands would not overflow anything —
**the UI is not the blocker; the Core selection ladder is.**

## 11. Test pins (why a data-only JSON expansion breaks CI)

`Ashfall.Core.Tests/DoseRegistersCatalogTests.cs`:

| Test | Pin | Breaks with 12/8 JSON? |
|---|---|---|
| `Load_FindsFourBandsThreePlansThreeGuesses` | `bands.Count == 4`, `plans.Count == 3` | **YES — exact count** |
| `Load_BandThresholdsBind` | `bands[1]=100`, `bands[2]=300`, `bands[3]=600` — **index**-based | **YES — any mid-insert reorders indices** |
| `BandLabel_MapsCoreBandsToVocabulary` | labels of IDs `band_green…black` | No (ID-based) |
| `Load_FindsTheFourAntagonists` / `Characters_RegisterTheFourAntagonists` | 4 NPCs | No |
| `Load_MissingDirectoryReturnsEmptyCatalog` | empty fallback | No |

Additional behavioral pins that assume the 4-band ladder:
`DoseLedgerSystemTests` (threshold edges at 100/300/600), `SickListSystemTests`,
`DiseaseTriageBridgeTests` (stage→band mapping), `Plan27BodyMindTests`,
`Plan81DoseLocationsExpansionTests` (`DoseBandResult.Green`).

## 12. Contract summary

1. Band selection = hardcoded constants 100/300/600 → ints 0–3. Catalog thresholds are inert.
2. Band ≥ 4 is unrepresentable (`BandIdFor` default → Green) and unsavable-with-meaning.
3. Plans = display strings on `SickBand.palliativePlan`; auto-writer knows exactly 2 IDs; costs never consumed.
4. UI is dynamic and table-free; not a capacity risk.
5. Two catalog tests pin exact counts and index positions.
6. **A 12-band / 8-plan JSON cannot ship data-only without breaking pinned tests and adding permanently unselectable bands.** See closeout: BLOCKED.
