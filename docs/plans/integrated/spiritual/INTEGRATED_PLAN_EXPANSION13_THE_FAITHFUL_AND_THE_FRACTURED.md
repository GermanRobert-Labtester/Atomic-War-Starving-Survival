# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-e-expansion21-expansion13-traumabond-xp08f6-2026-09-26`
> **Not committed** (per user direction). See §5 for the closeout evidence.


> **Package:** `EXPANSION-13-THE-FAITHFUL-AND-THE-FRACTURED` (UNBLOCK Program Wave 8 item 4)
> **Category:** spiritual
> **Plan type:** host integration of a sealed Core engine over the existing spiritual owner. No second faith ledger.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

# PLAN-EXPANSION-13-THE-FAITHFUL-AND-THE-FRACTURED — Spiritual Ritual Calendar Host Integration


## 1. Objective

`SpiritualRitualCalendarEngine` is sealed and tested but has **no host seam**:
nothing in `src/` calls it. The authored `spiritual_rituals.json` corpus is
loaded by `SpiritualCatalogLoader` into the live
`SpiritualMeaningCoordinator`, but no ritual is ever *evaluated*, no cooldown is
ever enforced, no holy-day observance is ever scheduled, and the ideological
friction mitigation the engine computes is never applied.

**Bounded outcome:**

1. `SpiritualRitualHostSession` reads the **already loaded** authored ritual
   definitions from the live `SpiritualCatalog` (no reload, no second catalog).
2. It tracks last-performed day per ritual id in its own bounded
   `spiritual_ritual` checksummed save section — a cooldown ledger only, not a
   piety meter and not a second ritual registry.
3. `SpiritualRitualCalendarEngine.EvaluateRitualObservance` is the sole verdict
   authority for allowed/cooldown/morale-delta/friction-reduction. Unknown or
   cooling-down rituals are refused without mutating state.
4. The morale delta is applied through the canonical morale owner
   (`NeedsSystem.Modify`) and the friction mitigation is reported to the
   ideological-friction owner through its existing seam. No morale or friction
   state is duplicated.
5. Holy-day observances (`GetScheduledObservance`) are exposed per campaign day
   for the day report and panels.
6. `--spiritual-ritual-selftest` proves catalog binding, cooldown enforcement,
   low-morale comfort scaling, holy-day window scheduling, determinism, and a
   save round-trip of the cooldown ledger.

**Non-goals (hard boundaries):** no piety/faith meter; no new ritual content;
no second ritual registry; no grief or memorial logic change (owned by
`SpiritualMeaningCoordinator`); no RNG; no Unity.

---

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
|---|---|
| Engine is pure/static | `Assets/Ashfall.Core/Spiritual/SpiritualRitualCalendarEngine.cs` |
| Engine has **zero** host references | `grep -rn SpiritualRitualCalendarEngine src/` → no hits |
| Engine has 5 passing Core tests | `Ashfall.Core.Tests/Spiritual/SpiritualRitualCalendarEngineTests.cs` |
| Ritual catalog is authored and already loaded | `spiritual_rituals.json` → `SpiritualCatalogLoader.Load` → `SpiritualCatalog.Rituals` in `src/Main.Spiritual.cs SetupSpiritual()` |
| Spiritual save store + section already exist | `src/Host/SpiritualSaveStore.cs`, section `spiritual_meaning` |
| Morale authority exists | `NeedsSystem.Modify(survivorId, NeedKind.Morale, delta)` |
| Ideological friction owner exists | `IdeologicalFrictionSystem` (`ideological_friction` section, hosted) |

---

## 3. Files

**New:** `src/Host/SpiritualRitualHostSession.cs`,
`src/Host/SpiritualRitualSaveStore.cs`,
`src/Host/HostCli.SpiritualRitual.cs`, `src/Main.SpiritualRitual.cs`,
`Ashfall.Core.Tests/Spiritual/PlanExpansion13SpiritualRitualHostIntegrationTests.cs`

**Edited:** `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (`spiritual_ritual`
section + filename, section-count pin bump),
`Assets/Ashfall.Core/HostCliRegistry.cs`, `src/Host/HostCli.cs`,
`src/Main.Application.cs`, `src/Main.CampaignOwners.cs` (phase-5 day owner with
pre-day snapshot rollback), `src/Main.SaveOrchestrator.cs` (setup + save mirror),
`src/Main.Lifecycle.cs` (reset), `docs/architecture/ARCHITECTURE_TEST_MAP.md`.

---

## 4. Acceptance

Host + Core test builds 0 errors / 0 warnings;
`--spiritual-ritual-selftest` green; focused spiritual + save-section tests green;
section-count pin updated with a ledger comment.


---

## 5. Closeout evidence (2026-09-26)

The Core authority named in this plan is now bound to the live canonical owners
and verified headless. Exact commands and results:

1. **Host seam:** `src/Host/SpiritualRitualHostSession.cs` binds the **already
   loaded** authored `SpiritualCatalog` (19 rituals from `spiritual_rituals.json`) —
   no reload, no second catalog — and `SpiritualRitualCalendarEngine` stays the sole
   verdict authority for allowed / cooldown / morale-delta / friction-reduction.
2. **Exactly one authority per concern:** the bounded `spiritual_ritual` checksummed
   save section (`spiritual_ritual_save.json`, `SpiritualRitualSaveStore`) holds only
   the last-performed day per ritual id. No piety meter, no second ritual registry,
   no grief or memorial logic change. Morale routes into the canonical morale owner
   (`NeedsSystem.Modify`); friction mitigation is reported to the ideological-friction
   owner through its existing seam.
3. **CLI probe:** `--spiritual-ritual-selftest` (**11/11 PASS** headless) covering
   catalog binding, engine verdicts, unknown-ritual refusal without mutation,
   authored-cooldown enforcement (the probe selects a ritual with cooldown >= 2 days),
   low-morale comfort scaling (150 -> 225 permille), deterministic holy-day windows,
   friction-mitigation derivation, save round-trip, and reset.
4. **Tests:** `PlanExpansion13SpiritualRitualHostIntegrationTests` **7/7 PASS**;
   pre-existing `SpiritualRitualCalendarEngineTests` still green.
5. **Lifecycle:** registered save section `spiritual_ritual` (section-count pin 302 ->
   305 in `ComprehensiveSaveStoreCorruptionAndMigrationTests` with a ledger comment),
   phase-5 day owner with `IPreDaySnapshotRestore` pre-day rollback, setup on both
   campaign paths, save mirror in `Main.SaveOrchestrator.cs`, lifecycle reset.
6. **Generated artifacts:** selftest manifest (257), CLI catalog (313), save-store
   matrix (307 stores), port contract (307 seams), and the `ARCHITECTURE_GRAPH`
   entry for `spiritual_ritual`.

**Deferred with named reasons:** no ritual calendar UI panel (presentation follow-on);
automatic ritual producers (a survivor choosing to observe a rite) are a separate
slice, not silently claimed.


---


---
