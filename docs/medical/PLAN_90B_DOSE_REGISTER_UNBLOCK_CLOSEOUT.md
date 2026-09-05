# PLAN 90B — DOSE REGISTER UNBLOCK & 12-BAND / 8-PLAN EXPANSION — CLOSEOUT

**Final status: ✅ COMPLETE — fully supported.**

Plan 90 closed BLOCKED because the Core ladder was compiled (4 hardcoded
thresholds, 4-case `BandIdFor`, 4-member `DoseBandResult`). Plan 90B executed
the unblock list from that closeout (§5) and shipped the preserved design.

---

## 1. Runtime changes (the unblock)

| Change | File |
|---|---|
| 12-rung default ladder (`0/25/50/100/150/200/300/400/500/600/800/1000` mSv) + `BandPale…BandSlate` constants (`BandCount=12`); ranks 0/3/6/9 are the preserved Green/Amber/Red/Black anchors | `DoseLedgerSystem.cs` |
| `ConfigureLadder(catalog)` — derives the instance ladder from the register catalog (ascending, strictly-increasing validated; degenerate catalog → default ladder). **Data remains the runtime authority.** | `DoseLedgerSystem.cs` |
| Instance `BandOf(mSv)` — highest threshold ≤ dose against the configured ladder (thresholds inclusive; above top rung → top rank). `BookReading`, `GetAdministrativeBand` now use it; static `BandFor` kept on the default ladder for static contexts | `DoseLedgerSystem.cs` |
| `BandIdFor` — default-ladder rank → id, clamped to the ends (never silently to Green); `BandLabel(catalog, rank)` — ranks the catalog bands by ascending `threshold_msv` (file order irrelevant), clamped | `DoseRegistersCatalog.cs` |
| `DoseBandResult` widened to 12 ranks (+ `NoEntry`) | `DoseLedgerSystem.cs` |
| `DoseLedgerSave` envelope **v3** — shape unchanged; decode of pre-v3 saves remaps sick-list band ints `{1→Amber(3), 2→Red(6), 3→Black(9)}`; v1 frozen-shape checksum validation untouched | `DoseLedgerSave.cs` |
| Host wiring: session ctor calls `Ledger.ConfigureLadder(Registers)`; `LedgerLine`/`RenderLedger` use instance `BandOf` | `DoseLedgerHostSession.cs`, `DoseRegisterSurface.cs` |
| `DoseLedgerPanel` color mapping intentionally **unchanged** — it maps via the preserved anchor constants (100/300/600), so golden snapshot `dose_ledger_default.png` content is unaffected | — |

## 2. Data shipped (`dose_registers.json`)

**12 bands** — anchors preserved verbatim (Green/Amber/Red/Black ids, labels,
thresholds, dispositions untouched); eight insertions:

| Rank | ID | Label | mSv |
|---:|---|---|---:|
| 0 | `band_green` | Green | 0 |
| 1 | `band_pale` | Pale | 25 |
| 2 | `band_yellow` | Yellow | 50 |
| 3 | `band_amber` | Amber | 100 |
| 4 | `band_orange` | Orange | 150 |
| 5 | `band_rose` | Rose | 200 |
| 6 | `band_red` | Red | 300 |
| 7 | `band_crimson` | Crimson | 400 |
| 8 | `band_violet` | Violet | 500 |
| 9 | `band_black` | Black | 600 |
| 10 | `band_indigo` | Indigo | 800 |
| 11 | `band_slate` | Slate | 1000 |

(`band_void` renamed `band_slate` per Plan 90 tone review — bureaucratic, not melodramatic.)

**8 plans** — first three preserved; `plan_morphine_tray` cost corrected from
the dead `"morphine"` reference to the real `painkillers` item:

| # | ID | Cost |
|---|---|---|
| 1 | `plan_morphine_tray` | `painkillers` |
| 2 | `plan_comfort_rounds` | `time` |
| 3 | `plan_nothing` | `none` |
| 4 | `plan_observation` | `time` |
| 5 | `plan_bed_rest` | `time` |
| 6 | `plan_fluids` | `clean_water` |
| 7 | `plan_supportive_care` | `medical_kit` |
| 8 | `plan_pain_control` | `painkillers` |

All item costs resolve in `items.json`. Guesses (3), calibration, registers (4),
NPCs (4): byte-preserved.

## 3. Save compatibility

- **Dose ledger:** band always derived from saved `cumulativeMsv` — no migration needed.
- **Sick list:** envelope v2 (and v1) band ints remapped on decode to the preserved anchor ranks; `Decode` bumps to v3. Old saves load with identical semantics — a legacy "Red" name is still rank 6.
- **Admin overrides:** legacy band-id strings resolve through the default vocabulary (all four legacy ids are ranks 0/3/6/9).
- **Cohort `trueBand`:** opaque band-id strings; all legacy ids remain in the catalog.

## 4. Boundary matrix (pinned by `BandLadder_TwelveRungBoundaryMatrix`, 24 cases)

`0→G, 25→Pale, 50→Yellow, 100→Amber, 150→Orange, 200→Rose, 300→Red, 400→Crimson,
500→Violet, 600→Black, 800→Indigo, 1000→Slate` — thresholds inclusive (`>=`),
each edge verified at `n` and `n−0.1`; above 1000 clamps to Slate; determinism
pinned (same dose → same band, no RNG). Static and instance ladders agree on
every edge.

## 5. Verification

| Check | Result |
|---|---|
| `dotnet test Ashfall.Core.Tests` | **PASS — 7003/7003** (28 new ladder/migration/boundary tests included) |
| `dotnet build Ashfall.csproj` | **PASS — 0 errors, 0 warnings** |
| `--data-integrity-selftest` | **PASS — 208/208 catalogs, 0 errors** |
| `--content-utilization-selftest` | **PASS** |
| `--save-store-checksum-selftest` (Gate A) | **PASS** |
| Test updates | `DoseRegistersCatalogTests` (12/8/3 pins, anchors, ordering, label clamp), `DoseLedgerSystemTests` (boundary matrix, determinism, ConfigureLadder, v2 remap), `DoseQuestOwnershipTests` (v3 version pins + v1 remap), `Plan27BodyMindTests` (400→350 mSv booking — 400 is now Crimson, test intent unchanged) |

*Note: one transient full-suite run showed 16 `GreenhouseItemCatalogTests`
failures from concurrent Plan 91 work landing mid-edit in this shared tree;
they passed on the immediate re-run and are unrelated to this change.*

## 6. Rejected (unchanged from Plan 90 guardrail review)

KI prophylaxis, generic chelation, decontamination, controlled isolation, and
medical transfer remain **rejected** — no radioactive-iodine applicability
model, no radionuclide/contamination state, and no transfer runtime exist. The
register stays an honest ledger: plans are inscriptions and resource
commitments, not cures.

## 7. Follow-up debt (non-blocking)

- `docs/bodymind/DOSE_REGISTER_STATE_MODEL.md` and
  `DOSE_INSTITUTION_CONSEQUENCE_MATRIX.md` still document the four-band model —
  refresh in a docs pass. *(done, Plan 90B follow-through)*
- The JSON `calibration`/`registers` blocks remain undeserialized (no DTO
  fields) — pre-existing, unchanged.
- `DiseaseTriage.SickBandFor` still maps illness stages onto the anchor rungs
  (Ill→Amber, Terminal→Red, OutcomePending→Black). Finer illness triage on the
  new rungs is optional design work, deliberately not done here.

## 8. Real-gameplay confirmation (journey selftest)

`--real-campaign-journey-selftest` now ends with a register segment: the
journey survivor is tagged (`AssignDosimeter`), and the real zone-shift doses
production `ExposeToZone` produced are booked as dial readings. Result in the
composed campaign:

```
register traversal: Green → Pale → Pale (cumulative 26.8 mSv from ordinary shifts)
```

Ordinary play demonstrably moves a survivor across the ladder's administrative
rungs — and surfaced a design fact worth keeping visible: the physical acute
dose **caps at 100 mSv** (`RadiationSystem`), so a saturated survivor books no
further acute increments (shift 2 booked 0.0 mSv; `BookReading` correctly
rejects zero-dose readings). The unbounded accumulator is the separate
`LifetimeRadiationExposure` ledger. Consequently the register's upper rungs
(Amber and above on the dial side; Orange and above against lifetime-scale
totals) are campaign-scale outcomes, exactly as the consequence matrix states.
If the register should one day track lifetime exposure rather than acute dial
readings, that is an explicit design decision for the owner — not silently
changed here.
