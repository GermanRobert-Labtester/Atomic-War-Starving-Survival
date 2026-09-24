# UNBLOCK — Plans 165 & 166: Modding Support + Shelter Identity

**Status:** DONE 2026-09-24 (integrator, user-authorized).
**Claim:** `claim-unblock-plans-165-166-2026-09-24`.
**Evidence:** `--mod-support-selftest` 12/12, `--shelter-identity-selftest` 12/12,
host + Core builds 0 errors / 0 warnings.

## Premise (verified in source before editing)

Both were listed as partials with **0 host references**.

- **Plan 165** — `ModSupportSystem` and `mod_manifest_schema.json` existed, but
  the host's mod path (`ModRuntime` + `JsonModLayering`) never consulted the
  authority: no specification load, no manifest contract validation, no
  dependency DAG, no conflict detection, no deterministic load order.
- **Plan 166** — `ShelterIdentitySystem` and `shelter_origins.json` existed with
  0 host references: no origin load, no naming, no persistence, no day owner.

## Plan 165 — Modding Support & Mod Data Contract

**Core**
- `ModManifestSpecificationLoader` (new, strict): rejects unsupported schema,
  empty schema_name, empty/duplicate allowed catalogs, empty/duplicate required
  fields, and a non-positive contract version.
- `ModSupportSystem.BindSpecification` (validated seam) and `GetCensus()`
  (`ModSupportCensus`); `RestoreState` is schema-gated.

**Host**
- `ModSupportHostSession` loads the authored specification through the strict
  loader, discovers and registers every mod directory's manifest, and exposes
  `EligibleModIds()` (status Enabled), `ResolveLoadOrder()` (deterministic,
  dependency-first), `DetectConflicts()`, and the census.
- `ModRuntime.Prepare` now creates the authority, registers discovered mods, and
  passes only the ids the authority judged eligible to `JsonModLayering`. The
  null-guarded layering engine stays the overlay materializer; it is no longer
  the only thing deciding which mods are eligible.
- Enable/disable persists through `UserSettingsData.EnabledMods` (the sole
  settings authority) — no campaign save section, because mod enablement is a
  user preference, not campaign state.
- CLI probe `--mod-support-selftest` (12 checks: strict spec, invalid id,
  incompatible range, missing dependency, cycle, load order, conflict, census,
  toggle, capture/restore, schema gate).

## Plan 166 — Shelter Identity, Naming & Origin

**Core**
- `ShelterOriginCatalogLoader` (new, strict): rejects unsupported schema,
  empty/duplicate origin ids, empty display names, origins without bonuses, and
  basis points outside -5000..5000.
- `ShelterIdentitySystem.BindOrigins` (validated seam), `GetCensus()`
  (`ShelterIdentityCensus`), and a schema-gated `RestoreState`.

**Host**
- `ShelterIdentityHostSession` + `ShelterIdentitySaveStore` (registered section
  `shelter_identity`, `shelter_identity_save.json`).
- `Main.ShelterIdentity.cs` binds the authored origins, selects the
  deterministic founding origin on a fresh campaign (government bunker when
  authored, else ordinal-first) and records the ordinal-first survivor as
  founder. A phase-5 day owner with `IPreDaySnapshotRestore` runs the identity
  tick inside the fail-closed advance; `shelter_identity_ticked` is an internal
  heartbeat.
- Community actions are recorded from canonical day facts (`medical_admitted`
  today); the known-for tags and infamy derive from that profile.
- CLI probe `--shelter-identity-selftest` (12 checks).

## Authority boundaries

- **Faction standing** stays owned by `FactionWarSystem`; this host never writes
  a competing faction reputation. The identity's `reputation_by_faction` is left
  unpopulated by the host until a designed reconciliation seam exists.
- **`JsonModLayering`** remains the overlay materializer; `ModSupportSystem` is
  the manifest-contract authority that decides eligibility and order.
- **Mod enablement** persists in user settings, never a campaign section.

## Deferred with named reasons

- Trade / raid / isolation community-action axes: no canonical emitted day kind
  exists, so they stay unbound rather than being fabricated here.
- The faction-reputation projection: `FactionWarSystem` is the canonical owner;
  a mirror would be a second store without a designed reconciliation seam.
- Shelter identity presentation (name/emblem in the shelter panel header) and a
  mod-management UI panel: presentation only.
- The preset high-frequency attenuation analogue is not applicable here.

## Verification

```
host + Core builds: 0 errors / 0 warnings
--mod-support-selftest 12/12          --shelter-identity-selftest 12/12
--data-integrity-selftest PASS        --port-contract-selftest PASS (296 seams)
--7-day-smoke-selftest PASS           --selftest-manifest 168 tests
Ashfall.Core.Tests/Mods/ 60/60        Ashfall.Core.Tests/Shelter/ 785/785
Ashfall.Core.Tests/Save/ 1543/1543    Ashfall.Core.Tests/Tooling/ 122/122
Ashfall.Core.Tests/Campaign/ 257/257
architecture map 238 subsystems (100%) · save-store matrix 240 · docs index 4417
```
