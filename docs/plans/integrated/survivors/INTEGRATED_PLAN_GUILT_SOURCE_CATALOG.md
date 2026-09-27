# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-g-cassettes-guiltsources-flotilla-recordintegrity-2026-09-26`
> **Not committed** (per user direction). See §6 for the closeout evidence.

---

# PLAN-GUILT-SOURCE-CATALOG — Authored Guilt Source Resolution Host Integration

> **STATUS: APPROVED BY USER**
> **Package:** `GUILT-SOURCE-CATALOG`
> **Category:** survivors / psychology
> **Plan type:** bind authored data to an already-live owner, removing call-site literals.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/Survivors/GuiltSourceCatalog.cs` (authored
`guilt_sources.json`) to the **already-live** `GuiltInsomniaSystem`, so guilt
severity and the survivor-facing description come from authored data instead of
hardcoded floats at each call site.

**Bounded outcome:** one resolution seam — a choice pattern resolves to the
authored `{severity, title, description}`; unknown patterns are refused loudly
rather than silently assigned a number.

**Non-goals:** no second guilt ledger, no new insomnia authority, no new save
section (guilt already persists), no invented guilt sources.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| Catalog orphan | `GuiltSourceCatalog` has **zero** consumers in `src/` and zero runtime consumers in Core (only `ContentUtilizationScanner`'s declaration tables name it) |
| Authored data | `Assets/StreamingAssets/Data/guilt_sources.json` — `choice_pattern`, `severity`, `title`, `description` with `{name}` templating |
| Declared consumer | `ContentUtilizationScanner.cs:399/726/1005` maps `guilt_sources.json → GuiltInsomniaSystem` |
| Live owner | `src/Host/Phase0HostSession.cs:351` `Guilt = new GuiltInsomniaSystem()`; `:235` exposes it; `:64` persists `guilt` in the Phase-0 aggregate |
| **Hardcoded severities** | `Phase0HostSession.cs:719` `RecordGuilt(survivorId, sourceId, severity)`; callers pass literals: `HostCli.PanelTests.cs:2748` `0.9f`, `src/UI/Phase0Panel.cs:254` `0.8f` |
| Authored text unused | `GuiltSourceDefinition.FormatDescription(survivorName)` has no caller |

## 3. Files

### New — Host
- `src/Host/GuiltSourceHostSession.cs`
- `src/Host/HostCli.GuiltSources.cs`

### Modified — Core
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--guilt-sources-selftest`

### Modified — Host
- `src/Host/Phase0HostSession.cs` — authored resolution seam over the existing `Guilt` owner
- `src/UI/Phase0Panel.cs` — literal severity replaced by authored resolution
- `src/Main.Phase0.cs`, `src/Host/HostCli.cs`, `src/Main.Application.cs`
- `scripts/ci/generate-architecture-map.py`

### Modified — Tests
- `Ashfall.Core.Tests/Survivors/PlanGuiltSourceCatalogHostIntegrationTests.cs` (new)

## 4. Acceptance

1. `--guilt-sources-selftest` ≥ 9/9: catalog load, authored severity match,
   `{name}` templating, unknown-pattern refusal with no mutation, no duplicate
   record for the same source/day, threshold behaviour preserved.
2. Focused xUnit suite green; every recorded guilt carries an authored severity,
   and no call site passes a literal.
3. `GuiltInsomniaSystem` remains the sole guilt/insomnia authority; the catalog is
   read-only data.
4. Adjacent gates green (no section-count change).

## 5. Deferred with named reasons

- No new guilt sources: authoring content is the narrative lane's authority.
- No guilt→morale rewrite: `GuiltInsomniaSystem` already owns the consequences.

## 6. Closeout evidence (2026-09-26)

**Verified fully integrated 2026-09-26** (no-commit
seal session): **not a partial.** Full Core↔host↔route↔persistence↔observable chain is
live and proven.

| Item | Evidence |
| --- | --- |
| Headless probe | `--guilt-sources-selftest` **10/10** |
| Focused xUnit | `PlanGuiltSourceCatalogTests` **8/8** |
| Save section | none — guilt already persists in the Phase-0 aggregate |
| Host build | `dotnet build Ashfall.csproj` — 0 errors |
| Core test build | `dotnet build Ashfall.Core.Tests` — 0 errors |
| Registry pin | `ComprehensiveSaveStoreCorruptionAndMigrationTests` — 309 sections, 1932 assertions green with all adjacent gates |
| Adjacent gates | `SaveSectionRegistryTests`, `PersistentFilenameRegistry`, `DayEventVocabulary`, `DayEventParitySourceGate`, `HostCliActionParityGate`, `MainTriadDriftGate` (both), `SaveStoreMatrixGate`, `PortContractGate` — all green |
| Regenerated | selftest manifest **277** (275 headless) · CLI catalog **337 / 547** · save-store matrix **311** · port contract **307** |

**Authority boundary held (Rule 5):** no parallel ledger, no new morale/needs/item/
trust authority, no invented content — every value comes from the existing owners or
the authored JSON.

**No commit** (user directive). `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md`
intentionally unwritten (foreman / named-integrator only).
