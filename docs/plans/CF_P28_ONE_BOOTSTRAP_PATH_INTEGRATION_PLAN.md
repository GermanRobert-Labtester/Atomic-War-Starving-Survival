# CF-P28 — One manifest bootstrap for fresh and restored campaigns

**Package:** `CF-P28-ONE-BOOTSTRAP-PATH` · completion-first Plan 10 · C2[9].
**Historical status:** FULLY INTEGRATED AND SEALED, 2026-09-19, as recorded by the
original closeout and ownership ledger. This editorial revision does not reseal it.
**Editorial review:** 2026-09-28. Current source inspected; no runtime tests rerun.

## Outcome

Fresh campaigns and restored campaigns execute the same declarative subsystem
bootstrap. Starting another campaign in the same process must discard the previous
campaign's enrolled sessions and panel bindings before constructing replacements.

The package closes the fresh-path residual after
`DEBT-PLAN28-MAIN-CONSTRUCTOR-MIGRATION`; it does not reopen that retired migration.

## Current contract

| Boundary | Source and requirement |
|---|---|
| Fresh campaign | `src/Main.CampaignServices.cs`: `ComposeCampaign` invokes `ExecuteSubsystemManifestBootstrap` after `SetupExpansions` and before cross-wiring. |
| Restore | `src/Main.SaveOrchestrator.cs` invokes the same bootstrap executor before completing restoration. This package uses that existing path. |
| Manifest adapter | `src/Main.Lifecycle.cs` binds descriptors to existing setup actions and delegates to `SubsystemManifest.ExecuteSetup`. Core manifest ordering remains authoritative. |
| Reset | `ResetAllSessionsInMemory` runs the existing reset registry. `ResetEnrolledFlagshipSessions` clears memorial, black-market and garage sessions, including related panel bindings. |
| Repeat setup | Existing live instances must survive repeated setup calls; idempotent setup methods remain the construction boundary. |

The current adapter has 18 manifest descriptors; the composition probe tracks
19 session fields because medical and medical-ward are separate instances.
Do not confuse descriptor count with session count or freeze either across future
approved subsystem additions.

Lifetime limitation: setup delegates use a process-global registry while the
registration guard belongs to each `Main` instance. This package does not redesign
that lifetime; multi-instance behavior requires separate evidence.

## Implementation boundaries

1. Reuse the manifest and existing setup delegates. Keep unrelated explicit setup
   calls until a separate dependency audit establishes that they can be removed.
2. Enroll the missing reset actions through the existing lifecycle registry.
   Unbind/remove stale black-market and vehicle panels before dropping sessions;
   clear memorial state so the next campaign can reconstruct it.
3. Keep day-owner registration, save sections, seeded RNG and gameplay authority
   in their existing owners. No second bootstrap registry or subsystem cache.
4. Verify in-process campaign switching as well as initial boot. A non-null field
   alone does not prove that it belongs to the current campaign.

Historical implementation paths: `src/Main.CampaignServices.cs`,
`src/Main.Lifecycle.cs`, `src/Main.UiTests.CompositionRoot.cs`,
`src/Main.UiTests.RealCampaignJourney.cs`, and
`Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs`.
Current edit ownership remains governed by [the ownership ledger](../../WORKTREE_OWNERSHIP.md).

## Acceptance

- Fresh and restore paths call the same manifest executor at the required point.
- Repeated setup preserves already-live instances. Panel opening may construct
  a previously-null lazy session; it must not replace an existing live session.
- Reset clears enrolled sessions and stale bindings; new-game and restore flows
  rebuild against the selected campaign without state leaking from the previous one.
- The composition-root and real-campaign-journey probes exercise actual runtime
  wiring; static source parity supplements those probes.
- Save/load and day advancement continue through their existing owners.

## Historical evidence and limits

The September 19 closeout records bootstrap parity **6/6**, manifest tests **7/7**,
black-market wiring **3/3**, vehicle integration **5/5**, host build **0 errors**,
7-day smoke **10/10**, vehicle probe **27/27**, sky defense **17/17**, and save/load
failure paths **8/8**. It also records passing player-panel, journey and composition
probes, plus `MainTriadDriftGateTests` **7/7** after the earlier gate defects were fixed.

An older row in [the integration ledger](../../INTEGRATION_PLANS.md) still describes
`%Content` binding and `SetupDifficulty` gate limitations; the original closeout
and ownership ledger record their later resolution. This revision preserves that
distinction rather than presenting either historical record as a fresh test run.

For future code changes, select affected targets through `bin/run-scoped-tests`
and run the relevant bounded Godot probe at 15 FPS. This editorial task checks
source references, links and whitespace only; it introduces no new acceptance run.
