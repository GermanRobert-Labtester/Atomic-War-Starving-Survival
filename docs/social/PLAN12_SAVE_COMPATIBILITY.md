# Plan 12 — Save Compatibility

Documents save/load behavior for all Plan 12 systems, ensuring old saves remain loadable and new saves are deterministic.

## Save Sections

| Section Key | File Name | Owner | Lifecycle Group | Schema Version |
|-------------|-----------|-------|-----------------|----------------|
| `shelter_decor` | `shelter_decor_save.json` | `ShelterDecorSystem` | `expanded_shelter` | 1 |
| (embedded in survivor_social) | — | `SurvivorSocialCoordinator` | `survivor` | 1 |
| (embedded in cohort) | — | `CohortSystem` | `survivor` | 1 |
| (embedded in apprenticeship) | — | `ApprenticeshipSystem` | `survivor` | 1 |

## Old-Save Compatibility

### Pre-Plan 12 Saves

When loading a save from before Plan 12 implementation:

| System | Default Behavior |
|--------|-----------------|
| `CohortSystem` | Existing children preserved. No new maturation events fired retroactively. |
| `GenerationalLineageExtension` | Empty lineage state. No fabricated parent-child links. |
| `ApprenticeshipSystem` | No active pairs. No completed skill grants. |
| `IdeologicalFrictionSystem` | No beliefs registered. No friction detected. |
| `RationConflictSystem` | Zero resentment. Fair allocation baseline. |
| `ShelterDecorSystem` | Empty placements. No decor, no plaques. |
| `MemorialSystem` | Existing memorials preserved. No automatic plaque generation. |
| Event flags | No Plan 12 flags set. Events eligible but not triggered. |
| Questlines | Plan 12 questlines present but not started. |

### No Fabricated History

Old saves must **not** retroactively fabricate:
- Guardianships or adoptions
- Apprenticeship completions
- Decor placements
- Memorial plaques
- Belief assignments
- Friction events that already occurred
- Ration grievances

Only state that can be unambiguously reconstructed from existing authoritative sources is derived.

## New Save State

### ShelterDecorSystem

```json
{
  "schema_version": 1,
  "placements": [
    {
      "roomId": "loc_lobby",
      "slotId": "north_wall",
      "itemId": "item_decor_poster_ration",
      "dayInstalled": 30,
      "isMemorialPlaque": false,
      "memorialSurvivorId": null,
      "plaqueSourceHeirloomId": null
    }
  ]
}
```

**Canonical ordering:** Placements sorted by (roomId, slotId) ordinal.

### CohortSystem (Extended)

Existing `CohortChild` DTO unchanged. New fields (if any) default to:
- `isMatured`: false (one-way flag, persists through save/load)
- `maturationDay`: -1 (unset)

### ApprenticeshipSystem

Existing state preserved. New completed skill IDs tracked in `completedSkillIds` list.

### IdeologicalFrictionSystem

Existing state preserved. New belief registrations tracked in belief map.

### RationConflictSystem

Existing state preserved. Resentment values, allocation history, fairness metrics all round-trip.

## Deterministic Serialization

- All collections sorted by stable keys (ordinal string comparison)
- No hash-map iteration order dependency
- No UI state, display text, or transient selection state serialized
- Float values use culture-invariant formatting (SaveChecksum rules)

## Save/Load Round-Trip Verification

Verified by tests:
- `Plan12CDecorTests.DecorSystem_CaptureRestore_IsolatesSnapshot`
- `Plan12CDecorTests.DecorSystem_Restore_PreservesMemorialPlaqueMetadata`
- `Plan12AGenerationTests.Cohort_PersistsMaturationThroughSaveRoundTrip`
- `Plan12DCrossSystemContinuityTests` (pending-state persistence tests)

## Migration Policy

- No migration from imagined history (see Old-Save Compatibility above)
- Schema version bumps: increment `schema_version`, add migration logic if needed
- Forward compatibility: unknown fields ignored on load
- Backward compatibility: new fields default to empty/zero/false
