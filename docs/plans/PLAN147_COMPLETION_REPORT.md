# PLAN 147 COMPLETION REPORT — sessions of 2026-09-06 (×2)

## Scope honesty statement

Session 1 delivered **Task A in full** and the **§13 minimal vertical slice**
of Task B (three representative entries proven end-to-end at the Core level),
plus the Task A/C audit documents. **Session 2 completed the host/Godot
wiring** (save-contract §3): Setup/Save triad, SaveStoreHub checksummed
section, SaveSectionRegistry entry, contract-matrix gates, journal feedback
surface and a dedicated headless selftest. Plan 147 as a whole is **still NOT
fully complete**: the barter acquisition route and activation of the remaining
17 records are explicitly pending (see §Remaining work).

## Session 2 — host wiring delivered

| File | Purpose |
|---|---|
| `src/Host/ContrabandSaveStore.cs` | Thin `SaveStoreHub.FromCodec` façade — checksummed `SchemaVersionedEnvelope`, atomic writes; passes `SaveStoreCoverageGateTests` by construction |
| `src/Main.Plans147.cs` | `Ensure/Setup/Save` triad + `ClaimContrabandStash(entryId)` player command + `TickContrabandStashDay` (once-per-entry journal rumors); catalog load **fails closed** on validator rejection |
| `src/Host/ContrabandStashSelfTest.cs` + CLI verb `--contraband-stash-selftest` (alias `--contraband-selftest`) | Headless proof: validation → day gate → once-only claim → canonical grant → no-side-effect reads → checksummed save round-trip → post-restore replay block |
| `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` | `contraband_stash` section (metadata + `contraband_stash_save.json`) |
| `Assets/Ashfall.Core/HostCliRegistry.cs` | Core enum + descriptor for the new verb |
| Contract matrices updated | `ARCHITECTURE_TEST_MAP.md` (row 165 + deep-evidence + lifecycle matrix), `VersionReportContractTests` (165 sections / 159 envelopes), `ComprehensiveSaveStoreCorruptionAndMigrationTests` (165), `docs/ci/SELFTEST_MANIFEST.json` (110 tests) |

### Session 2 verification gates (all PASS)

1. `dotnet build Ashfall.csproj` — clean
2. `dotnet test` full suite — **10285/10285 PASS**
3. `godot --headless -- --contraband-stash-selftest` — PASS (exit 0)
4. `--save-store-checksum-selftest` (Gate A sweep incl. new store) — PASS
5. `--data-integrity-selftest` — 299 catalogs, 0 errors; `--bridge-selftest` — exit 0

(The transient `SaveWireContractTests.NarrativeState` arcState failure
observed mid-session belonged to the concurrent EncounterCatalog stream and
resolved on their commit; final full-suite run is fully green.)

## Delivered

### Code (engine-agnostic Core)

| File | Purpose |
|---|---|
| `Assets/Ashfall.Core/Narrative/ContrabandCatalogValidator.cs` | Raw-JSON validation: 45-key frozen mechanics allowlist, id/tier/price/tag rules, NaN/Infinity + range-class enforcement. Closes the silent-drop hole where 31 untyped keys implied phantom mechanics. |
| `Assets/Ashfall.Core/Narrative/ContrabandStashSystem.cs` | Once-only, deterministic, day-gated stash discovery runtime. Grants canonical items through atomic `InventoryBill`; capacity-blocked claims don't consume the stash; `CaptureState/RestoreState` house pattern; no RNG; no mechanics field executed. |
| `Ashfall.Core.Tests/Narrative/ContrabandPlan147Tests.cs` | 28 tests: validator contract (12), slice acquisition/discovery (11), authorized-effect ownership (3), determinism/no-side-effect (2). |

### Documents (docs/plans/)

`PLAN147_BASELINE.md`, `CONTRABAND_ENTRY_MATRIX.md`,
`CONTRABAND_MECHANICS_AUTHORITY_MATRIX.md`, `CONTRABAND_ITEM_IDENTITY_MATRIX.md`,
`CONTRABAND_STASH_LOCATION_MATRIX.md`, `CONTRABAND_TRADE_AND_ARBITRAGE_AUDIT.md`,
`CONTRABAND_SAVE_COMPATIBILITY.md`, `PLAN147_REGRESSION_MATRIX.md` (this file
completes the nine required deliverables).

## Mechanics-field disposition summary (all 45 keys)

- **LIVE-MAPPED (via canonical identity, not via the JSON values): 2 routes**
  - morale (sugar slice) → `sugar.moraleEffect` through the item-use pipeline
  - agriculture (wheat slice) → `GreenhouseExpansionCatalog.CropCatalog` wheat crop
- **DESCRIPTIVE / PRESENTATION-ONLY: 39 keys** — incl. every
  `tribunal_suspicion_rate`, all scrip/falsification fields, radio range, EMP
  shield, illumination hours, detection precision, door override, faction
  influence, loyalty, noise, battery metrics.
- **DEFERRED (owner exists, identity missing): 4 groups** — hunger/calorie
  (needs item identity), chemical dependency risk + pain relief (needs
  canonical `morphine` item), infection risk (needs disease-use hook), fuel
  theft (needs designed action).
- **REMOVE-CANDIDATE (kept as prose only): counterfeit-scrip pair**
  (`scrip_purchasing_falsification`, `vending_machine_jam_chance`) — no scrip
  currency exists; plan §10 forbids inventing one.
- **Zero new subsystems, zero generic executors, zero duplicate item rows.**

## Reachable contraband entries (content-utilization proof)

| Entry | Tier | Producer route | Canonical grant | Gate |
|---|---|---|---|---|
| `contraband_card_deck_pinned_kings` | 1 | stash discovery (day ≥ 3) | `item_playing_cards` ×1 | once per campaign |
| `contraband_unrationed_sugar_brick` | 2 | stash discovery (day ≥ 8) | `sugar` ×8 | once per campaign |
| `contraband_century_seed_grain_vial` | 3 | stash discovery (day ≥ 20) | `item_seed_wheat` ×1 | once per campaign |

All other 17 records: **explicitly deferred list** — not discoverable,
tradable or usable through any route (fail-closed on claim attempts), pending
per-record identity/owner review per the identity matrix.

## Verification gates (all PASS)

1. `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — clean
2. `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — **10273/10273 PASS**
3. `dotnet build Ashfall.csproj` — 0 errors, 0 warnings
4. `godot --headless -- --data-integrity-selftest` — 299 catalogs, 0 errors
5. `godot --headless -- --bridge-selftest` — exit 0

## Remaining work (next sessions)

1. ~~**Host wiring (Godot)**~~ — **DONE (session 2)**: Setup/Save triad,
   `SaveStoreHub` checksummed section, registry entry, contract gates,
   journal feedback surface, `--contraband-stash-selftest`
   (see `CONTRABAND_SAVE_COMPATIBILITY.md` §3).
2. **Morphine vertical slice (highest-value follow-up):** author a canonical
   `morphine` item (data authority) whose use routes through
   `ChemicalDependencySystem.OnSubstanceConsumed` exactly once per authorized
   consumption; then activate the record. Includes the
   buy→use→dependency-once regression tests.
3. **Barter acquisition route:** caravan stock listings priced in canonical
   trade-value units with existing stock-pinning semantics
   (`CONTRABAND_TRADE_AND_ARBITRAGE_AUDIT.md` §4).
4. **Per-record review of the remaining 14 item-concept rows** against new
   canonical items as those are authored (candles, coffee, lard…), each
   requiring a mechanics-matrix row first (validator enforces the review).
5. **Social consequence design (Plan B.11–12):** possession consequences only
   via real encounters/inspections when such content is authored; nothing
   polls suspicion today and nothing should.

## Final invariant check

Contraband now *combines* existing systems (catalog content + canonical
inventory + day-gated discovery) in a way that is deliberately inert: no
mechanics field is executed, no second authority exists, and the validator
makes sure it stays that way until a disposition is consciously recorded.
