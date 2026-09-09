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

## 3. Host integration contract (PENDING — not yet wired)

The Godot host has **no** contraband wiring yet. When wired (follow-up task),
the house rules are:

1. `Setup`: construct `BunkerContrabandCatalog` from
   `narrative/bunker_contraband_barter.json`, construct
   `ContrabandStashSystem` with the inventory session + item-catalog lookup,
   register `DefaultActivations()`.
2. `Save`: capture `ContrabandStashSystem.CaptureState()` into a new campaign
   section. The store MUST be a `SaveStoreHub` façade (checksummed
   `SchemaVersionedEnvelope`, atomic write) per Initiative #41/#42 — a
   hand-rolled envelope would fail `SaveStoreCoverageGateTests`.
3. `Load`: `RestoreState` from the section; missing section (legacy campaign)
   → `null` → fresh state. `allowLegacyBareState` may stay false (no
   pre-envelope format ever existed).
4. `SaveSectionRegistry`: new key + file name required for the envelope
   whitelist.
5. The checksummed-envelope + coverage-gate sweep tests must be extended to the
   new store (pattern: `SaveStoreChecksumSweepTests`).

No `CaptureState`-compat risk exists for other systems: the contraband section
is additive and self-contained.

## 4. Determinism & replay

- No RNG in the system at all → same-day availability is a pure function of
  state (pinned by the identical-systems availability-sequence test).
- `CaptureState` returns a deep clone (serializer round-trip), so snapshots
  can't alias live state (`Stash_RestoreState_IsDeepClone_NotSharedReferences`).
