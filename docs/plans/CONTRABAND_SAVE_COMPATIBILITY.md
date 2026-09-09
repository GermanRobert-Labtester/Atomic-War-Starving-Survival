# CONTRABAND SAVE COMPATIBILITY — Plan 147 Task A / Task C.4, C.11–C.12

## 1. State surface

`ContrabandStashSystem` owns exactly one persisted state:

```csharp
ContrabandStashState {
    string systemId;                     // "contraband_stash"
    Dictionary<string,int> claimedDayByEntry;  // entryId -> claim day
}
```

- The catalog itself is **immutable definition data** and is never serialized
  (Plan Task C.12). Only claims persist.
- The activation map is **code** (`DefaultActivations`), also never persisted —
  it is re-registered on construction, so changing an activation's grant/day
  gate is a code change, not a save migration.

## 2. Old-save compatibility (Task C.11)

| Save vintage | Behavior |
|---|---|
| Pre-Plan-147 saves (no contraband section) | `RestoreState(null)` → fresh state, zero claims, all activated stashes available. **No wealth gained or lost by loading** — the system touches inventory only inside an explicit `TryClaimStash`. Pinned by `Stash_OldSave_WithNoContrabandState_ChangesNothing`. |
| Saves with acquired canonical items | Canonical items already sit in inventory sections owned by the inventory system; the contraband section is orthogonal. A stash whose item the player already owns via other routes is still claimable (once) — the item is the authority, provenance is not tracked by design. |
| Saves mid-trade (future barter route) | Not applicable to this slice; the stash route has no intermediate transactional state (the inventory bill is atomic). |
| Saves after stash discovery (post-claim) | `claimedDayByEntry` round-trips; replay claims are blocked (`Stash_SaveRoundTrip_PreservesOnceOnlyClaims`). |

## 3. Host integration contract (WIRED — Plan 147 follow-up session)

The Godot host wiring is complete (`src/Main.Plans147.cs`, `src/Host/ContrabandSaveStore.cs`):

1. `SetupContrabandStash()` (called from both `Main.SaveOrchestrator` and
   `Main.CampaignServices` setup blocks): constructs the catalog from
   `res://…/narrative/bunker_contraband_barter.json`, **fails closed** (layer
   stays inert) when `ContrabandCatalogValidator` rejects the file, registers
   `DefaultActivations()`, restores persisted state, and subscribes
   `OnStashClaimed` → journal feedback (the game's single feedback strip).
2. `SaveContrabandStash()` → `CaptureSection("contraband_stash", …)` into the
   single campaign envelope; the store is a `SaveStoreHub.FromCodec` façade
   (`SchemaVersionedEnvelope`, checksummed, atomic write) — the
   `SaveStoreCoverageGateTests` sweep passes automatically.
3. `SaveSectionRegistry`: `contraband_stash` → `contraband_stash_save.json`
   (metadata + file-name map). Contract matrices updated
   (`VersionReportContractTests`, `ComprehensiveSaveStoreCorruptionAndMigrationTests`,
   `ARCHITECTURE_TEST_MAP.md`, `SELFTEST_MANIFEST.json`).
4. Daily tick `TickContrabandStashDay(day)` emits once-per-entry discovery
   rumors via the deduped journal; a stash becoming visible never mutates
   simulation state.
5. Player route: `Main.ClaimContrabandStash(entryId)` (UI/CLI callable; no
   panel by design). Claim feedback = one journal entry per claim.
6. Headless proof: `godot --headless -- --contraband-stash-selftest`
   (validation → day gate → once-only claim → canonical grant → no-side-effect
   reads → checksummed save round-trip → post-restore replay block).

No `CaptureState`-compat risk exists for other systems: the contraband section
is additive and self-contained.

## 4. Determinism & replay

- No RNG in the system at all → same-day availability is a pure function of
  state (pinned by the identical-systems availability-sequence test).
- `CaptureState` returns a deep clone (serializer round-trip), so snapshots
  can't alias live state (`Stash_RestoreState_IsDeepClone_NotSharedReferences`).
