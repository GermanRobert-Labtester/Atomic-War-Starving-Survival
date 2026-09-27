# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-g-cassettes-guiltsources-flotilla-recordintegrity-2026-09-26`
> **Not committed** (per user direction). See §6 for the closeout evidence.

---

# PLAN-CASSETTE-PLAYBACK-SETS — Cultural Cassette Set Playback Host Integration

> **STATUS: APPROVED BY USER**
> **Package:** `CASSETTE-PLAYBACK-SETS`
> **Category:** audio / culture
> **Plan type:** host integration of an unhosted Core authority the repo itself declares orphaned.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/Audio/CassettePlaybackSystem.cs` and the orphaned
`CassetteSetCatalogLoader` over the authored `cassette_sets.json`, so collected
tape parts play, award morale **once**, complete sets, and reveal their authored
hidden caches.

**Bounded outcome:** catalog loaded → acquisition derived from the live inventory
→ play → morale through the canonical needs owner → set completion → cache items
through the canonical inventory owner → the repo's own content-certification
orphan flag flips to active.

**Non-goals:** no second tape ledger, no new morale authority, no new item
ledger, no authored-content invention, no UI panel.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| **The repo declares this orphan** | `src/Main.ContentCertification.cs:85-86`: comment *"Families whose consumers are still orphans in the host graph."* then `MarkCatalogLoaded("cassette_sets.json", false)` + `MarkConsumerActive("CassettePlaybackSystem", false)` |
| Declared consumer | `src/Host/ContentCertificationHostSession.cs:60` family `cassette_sets` → `cassette_sets.json` → `CassettePlaybackSystem` |
| Orphan loader | `CassetteSetCatalogLoader.Load(dataDir, fileIO, json)` — zero callers in `src/` |
| Authored data | `Assets/StreamingAssets/Data/cassette_sets.json` (sets, parts, `hidden_cache_location`, `hidden_cache_items`, `completion_narrative`) |
| Live morale authority | `NeedsSystem.Modify(id, NeedKind.Morale, delta)` |
| **Morale is recorded but never applied** | `PlayPart` computes `moraleBoost` and adds `totalMoraleAwarded`, but the `_vinylMorale != null` branch is an empty comment stub |
| Live inventory | `_holdfastRuntime.Trade.Inventory` / `_inventory.Inventory` |
| Live journal | `JournalSystem.TryAddRawEntry` |
| No save section | `SaveSectionRegistry` has no `cassette_playback` |

`VinylMoraleSystem` stays the vinyl-record authority; cassettes are a separate
collection, so tape morale routes to the survivor needs owner, not the turntable.

## 3. Files

### New — Host
- `src/Host/CassettePlaybackHostSession.cs` (+ `CassettePlaybackSaveStore`)
- `src/Host/HostCli.CassettePlayback.cs`
- `src/Main.CassettePlayback.cs`

### Modified — Core
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — `cassette_playback` section + filename
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--cassette-playback-selftest`

### Modified — Host
- `src/Host/HostCli.cs`, `src/Main.Application.cs`
- `src/Main.SaveOrchestrator.cs`, `src/Main.Lifecycle.cs`
- `src/Main.ContentCertification.cs` — orphan flags replaced with measured truth

### Modified — Tests
- `Ashfall.Core.Tests/Audio/PlanCassettePlaybackHostIntegrationTests.cs` (new)
- `Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` (pin)

## 4. Acceptance

1. `--cassette-playback-selftest` ≥ 10/10: authored catalog load, unknown-tape
   refusal, not-owned refusal, duplicate acquisition refusal, first-play morale
   exactly once, replay without a second award, final-part bonus, set completion,
   cache reveal through the inventory owner, save/restore, certification truth.
2. Focused xUnit suite green.
3. `CassettePlaybackSystem` stays the sole tape-collection authority; morale only
   through `NeedsSystem`, items only through the canonical inventory.
4. Content certification reports `cassette_sets.json` loaded and
   `CassettePlaybackSystem` active, derived from live state (never hardcoded).
5. Adjacent gates green.

## 5. Deferred with named reasons

- No cassette UI panel: the cultural surfaces are shared.
- No brownout/broadcast coupling to `VinylMoraleSystem`: that is the turntable's
  own authority and needs a signed cross-system rule.

## 6. Closeout evidence (2026-09-26)

**Verified fully integrated 2026-09-26** (no-commit
seal session): **not a partial.** Full Core↔host↔route↔persistence↔observable chain is
live and proven.

| Item | Evidence |
| --- | --- |
| Headless probe | `--cassette-playback-selftest` **12/12** |
| Focused xUnit | `PlanCassettePlaybackTests` **10/10** |
| Save section | cassette_playback (cassette_playback_save.json) |
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
