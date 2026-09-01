# Memorial Decor Provenance — Plan 12C

Connects `ShelterDecorSystem` plaques to `MemorialSystem` without duplicating death authority.

## Provenance Model

A memorial plaque decor item carries:
- `IsMemorialPlaque`: true
- `MemorialSurvivorId`: the survivor being commemorated
- `PlaqueSourceHeirloomId`: the keepsake item that was converted into a plaque

The memorial record in `MemorialSystem` remains authoritative for death truth. The plaque is a display reference, not a second source of death data.

## Plaque-Producing Conditions

| Condition | Source | Plaque Variant |
|-----------|--------|---------------|
| Fulfilled final wish | `FinalWishSystem` resolution | `item_decor_memorial_plaque_generic` |
| Managed vigil | Vigil quest outcome | `item_decor_memorial_plaque_carving` |
| Recognized shelter death | `MemorialSystem` record creation | `item_decor_memorial_plaque_generic` |
| Child's art memorial | Social event (child creates art for deceased) | `item_decor_memorial_plaque_drawing` |

## Plaque Resolution

`ShelterDecorSystem.ResolvePlaqueItemId(heirloomItemId)`:
1. If heirloom ID matches a registered kind-specific plaque → return kind-specific ID
2. If heirloom ID doesn't match any kind → return `item_decor_memorial_plaque_generic`
3. If heirloom ID is empty → return empty string (bypass fallback)

`ShelterDecorSystem.ResolvePlaqueSlot(survivorId, heirloomId, room, slot, day)`:
1. Resolve plaque item ID from heirloom
2. Create `DecorPlacement` with `IsMemorialPlaque = true`
3. Set `MemorialSurvivorId` and `PlaqueSourceHeirloomId`
4. Return placement ready for assignment

## Duplicate Prevention

- One plaque per memorial survivor (second plaque attempt rejected)
- Same-name survivors distinguished by survivor ID, not display name
- Plaque removal/reinstallation preserves provenance metadata

## Old-Save Compatibility

- Memorials created before Plan 12: no automatic plaque generation
- Player must explicitly create plaque from memorial (no fabricated history)
- Empty plaque state on old saves (no default placements)

## Save Contract

Plaque metadata persists through `ShelterDecorSystem.CaptureState/RestoreState`:
- `IsMemorialPlaque`, `MemorialSurvivorId`, `PlaqueSourceHeirloomId` all round-trip
- Verified by `Plan12CDecorTests.DecorSystem_Restore_PreservesMemorialPlaqueMetadata`

## Invalid Reference Handling

- If `MemorialSurvivorId` references a deleted/invalid survivor: plaque remains displayable but shows "Unknown Survivor" in UI
- If `PlaqueSourceHeirloomId` references a deleted item: plaque remains, heirloom reference shown as "Lost Item"
- No crash on invalid references; graceful degradation
