# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-g-cassettes-guiltsources-flotilla-recordintegrity-2026-09-26`
> **Not committed** (per user direction). See §6 for the closeout evidence.

---

# PLAN-BLACK-FLOTILLA-STANDING — Maritime Faction Standing Threshold Host Integration

> **STATUS: APPROVED BY USER**
> **Package:** `BLACK-FLOTILLA-STANDING`
> **Category:** maritime / factions
> **Plan type:** register authored faction policy into the already-live stance engine.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` into the LIVE
`FactionStanceEngine` held by `DeepCoastHostSession`, so the Black Flotilla is
evaluated on its authored thresholds and Plan-23 trust tiers instead of the
engine's generic fallback.

**Bounded outcome:** the flotilla's authored `FactionThresholds` are registered
once at host construction, the tier verdicts become reachable, and the maritime
surface reports which gate produced a refusal.

**Non-goals:** no new trust ledger, no new standing authority, no new save
section (trust is owned by the stance engine), no invented thresholds.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| Orphan | `BlackFlotillaStanding` has **zero** consumers in `src/` and zero runtime consumers in Core |
| Authored policy | `RaidThreshold -50`, `RobThreshold -20`, `MinTrustToTrade 0`, `IntelShareThreshold 40`, `RaidAggression 0.35`, Plan-23 tiers `SalvageTrustedTrust 30`, `DeepCooperationTrust 55` |
| Faction is real data | `faction_black_flotilla` appears in `faction_lore.json`, `characters.json`, `caravan_trade_routes.json` |
| Live stance engine | `src/Host/DeepCoastHostSession.cs:30` `public FactionStanceEngine Stances`, `:52` constructed, `:102-104` `ModifyTrust(...)`, `:332-333` trust readout |
| **Falls back to defaults today** | `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs:95` synthesises a generic `FactionThresholds` when a faction was never registered |
| **Registration precedent** | `src/Foundry/SilentFoundryHostSession.cs:163` `GuildStanceEngine.RegisterFaction(new FactionThresholds(...))` with the same numbers |
| Ready-made helper | `BlackFlotillaStanding.Register(FactionStanceEngine)` and `.Thresholds` already exist, uncalled |

## 3. Files

### New — Host
- `src/Host/BlackFlotillaStandingHostSession.cs`
- `src/Host/HostCli.BlackFlotillaStanding.cs`

### Modified — Core
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--black-flotilla-standing-selftest`

### Modified — Host
- `src/Host/DeepCoastHostSession.cs` — register authored thresholds at construction
- `src/Host/HostCli.cs`, `src/Main.Application.cs`, `src/Main.DeepCoast.cs`
- `scripts/ci/generate-architecture-map.py`

### Modified — Tests
- `Ashfall.Core.Tests/Maritime/PlanBlackFlotillaStandingHostIntegrationTests.cs` (new)

## 4. Acceptance

1. `--black-flotilla-standing-selftest` ≥ 9/9: authored thresholds registered on
   the live engine, tier boundaries exact (`-50/-20/0/30/40/55`), trade/intel/
   salvage/deep-dive gates agree with the tier, unregistered-faction fallback is
   gone, standing changes re-resolve the tier, determinism.
2. Focused xUnit suite green.
3. `FactionStanceEngine` stays the only trust store; this package only registers
   policy and reads verdicts.
4. Adjacent gates green (no section-count change).

## 5. Deferred with named reasons

- No flotilla raid scheduler: raid generation is the faction-war owner's call.
- No UI panel: the maritime surface is shared.

## 6. Closeout evidence (2026-09-26)

**Verified fully integrated 2026-09-26** (no-commit
seal session): **not a partial.** Full Core↔host↔route↔persistence↔observable chain is
live and proven.

| Item | Evidence |
| --- | --- |
| Headless probe | `--black-flotilla-standing-selftest` **10/10** |
| Focused xUnit | `PlanBlackFlotillaStandingTests` **9/9** |
| Save section | none — trust already persists in FactionStanceEngine |
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
