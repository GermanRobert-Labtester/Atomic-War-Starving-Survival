# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Plan Quad Package L — Gate-Proven Debt Seals (Four Independent Integrations)

**Status:** APPROVED BY USER — **FULLY INTEGRATED**
**Date:** 2026-09-26
**Claim:** `claim-quad-l-gate-proven-debt-seals-2026-09-26`

## Selection Method

Every plan in this package was located by a **failing gate in the repository's own
suite**, not by a plan document, audit, or census row. That is the highest-integrity
source of integration work available: the repo itself states, with an executable
assertion, that a contract is broken.

The four plans named in `AGENTS.md`'s "ACTIVE QUEUE" were premise-audited and found
**already integrated** (Rule 7 — evidence over the queue):

| AGENTS.md queue row | Source evidence |
|---|---|
| `CF-P1-DISTRESS-CONTENT-SEAL` | `docs/radio/DISTRESS_SIGNAL_PR3_CLOSEOUT.md` reads `Status: SEALED (2026-09-19)` |
| `CF-P5-RESTOCK-RECONCILE` | plan header `LIVE and GREEN since 2026-09-17`; `RestockAllocationEngine.Allocate` live |
| `CF-P6-VEHICLE-ARMOR-GRADES` | plan header `INTEGRATED`; `VehicleArmorGradeCatalogLoader`/`GetArmorGrade`/`InstallArmorGrade` live in src |
| `CF-P28-ONE-BOOTSTRAP-PATH` | plan header `FULLY INTEGRATED AND SEALED`; `ExecuteSubsystemManifestBootstrap` runs from `Main.CampaignServices.cs:75` |

Candidate Core authorities were also rejected for cause: every `*Engine`/`*C4ISystem`
variant is a **subclass of an already-integrated base** (`MaritimeDiveSystem` is bound
through `StealthDiveInstance` at `src/Host/MaritimeHostSession.cs:40`), the save codecs
and migration projections duplicate owned state, and the systems that persist through
composite envelopes (`WeatherSondeSystem` via the world section) are already wired.
`--content-utilization-selftest` reports **0 orphans**, exhausting that category.

---

## 1. F16 — Persistence fail-closed must leave the day uncommitted

**Authority:** `CampaignDayCoordinator` (Core, `Assets/Ashfall.Core/Campaign/`).

**Defect proven by:** `FollowUpRemediationGateTests.F16_CampaignDayCoordinator_PersistenceFailure_ArmsPendingRestore`
(`Expected: -1, Actual: 2`).

**Root cause:** `Advance` committed `_lastAdvancedDay = day` and called
`Calendar.SetDay(day)` *before* `persistence.PersistBeforeBriefing`. When persistence
threw in fail-closed mode, the campaign header claimed a day that had no save on disk.
The failure path also returned a **new** `DayAdvancedEventArgs`, discarding every owner
tick report already collected.

**Fix:**
- Capture `committedBefore` (the prior `_lastAdvancedDay`) and `calendarBefore`
  (the prior `Calendar.CurrentDay`) before committing.
- On a persistence throw in fail-closed mode, roll **both** back and arm
  `_pendingRestoreDay`, so a retry re-ticks from a clean baseline.
- New private helper `ReportsWithPersistenceFailure` preserves the collected owner
  reports and appends the persistence failure as its own report rather than replacing
  the set.

**Own-test bug found and corrected:** my first version asserted
`Calendar.CurrentDay == 2` after a failed advance to day 2. That was my error — a fresh
`CampaignCalendar` defaults to day 1, so a failed advance must leave it at **1**. The
production behaviour was correct; the test was fixed, not the code.

**Files:** `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`

## 2. ConsequenceLedgerSourceGate — no private in-memory flag ledger in production

**Authority:** `src/Host/HostCli.KnockWhitelist.cs` (host CLI probe).

**Defect proven by:** `ConsequenceLedgerSourceGateTests.SourceGate_NoProductionPrivateInMemoryFlagLedgerConstruction`
(two hits at `HostCli.KnockWhitelist.cs:L27` and `:L33`).

**Root cause:** the probe constructed `new InMemoryFlagLedger()` twice, opening a second
flag authority beside the campaign's single `ConsequenceLedger` (Rule 5).

**Fix:** one shared `Ashfall.Core.Flags.CampaignConsequenceLedger` drives all checks.
The "no flag recorded" condition for check 4 is expressed with `ClearAll()` on that same
ledger instead of constructing a second one; check 5 re-sets the gated flag so it still
proves refusal is a property of the knock id, not of the flags.

**Verification:** all 6 probe checks still pass (`--knock-whitelist-selftest`), and no
`new InMemoryFlagLedger(` remains anywhere under `src/`.

## 3. PortContractGate — classify the unclassified Core integration seam

**Authority:** `docs/ci/port_contract_policy.json` (integrator-owned governance).

**Defect proven by:** `PortContractGateTests.AllCoreIntegrationSeams_AreTrackedInPolicy`
(`SurvivorLetterDeliverySystem.BindCatalog` untracked).

**Fix:** added the seam entry with `classification: HOST_REQUIRED` — verified by
`src/Host/SurvivorLetterDeliveryHostSession.cs:64` (`system.BindCatalog(catalog)`) that
the seam is genuinely called from production — and bumped `total_seams` 307 → 308.

## 4. VersionReportContractTests — refresh the stale persistence pins

**Authority:** `Ashfall.Core.Tests/VersionReportContractTests.cs`.

**Defect proven by:** two tests frozen at `261` envelopes / `267` sections while the
registered section list has grown.

**Fix:** refreshed to the **measured** truth — 314 sections = **6 versioned Core codecs +
308 checksum envelopes** — with a comment recording the measurement date and derivation,
so the pins name the real inventory instead of a stale pair (Rule 7).

---

## Verification

| Check | Result |
|---|---|
| Core + host builds | **Build succeeded, 0 errors** |
| `FollowUpRemediationGateTests.F16` | **green** |
| `ConsequenceLedgerSourceGateTests` | **green** |
| `PortContractGateTests.AllCoreIntegrationSeams_AreTrackedInPolicy` | **green** |
| `VersionReportContractTests` (both) | **green** |
| `PlanQuadPackageLCoreTests` (new, 5) | **5/5 green** |
| Adjacent sweep (campaign/ledger/registry/drift) | **128/128 green** |
| `--knock-whitelist-selftest` | **6/6 PASS** |
| `--data-integrity-selftest` | **0 errors, SELFTEST PASS** |

Port contract `total_seams`: 307 → **308**. Save sections: unchanged (no new section).

## Recorded for the next batch (not started here, deliberately)

Two verified-clean, previously-unintegrated Core authorities exist and are the honest
queue for the next package: `MaritimeExplorationSystem`
(`Assets/Ashfall.Core/Maritime/MaritimeExplorationSystem.cs`, 13 public commands, no
Core or src consumer) and `RailwayInterlockEngine`
(`Assets/Ashfall.Core/Expeditions/RailwayInterlockEngine.cs`, `TickDay`, only
`ContentUtilizationScanner` references it).

## Non-Goals / Not Done

- **No commit** (user instruction).
- The 206 machine-specific `file://`/`/home/` documentation links reported by
  `DocLinkValidationGateTests` were **left untouched** — that sweep is a separate,
  large mechanical pass over a shared doc tree and is not part of this package.
- Salvaged before execution: a full "Quad Package K" host integration for the
  `IndependentBranchSystem`/`MilitaryBranchSystem`/`RebelBranchSystem`/`PrpfStandingSystem`
  family was **written then completely deleted** when the architecture map revealed
  `FactionBranchCoordinator` already instantiates all four (Rule 5). No partial was left.
