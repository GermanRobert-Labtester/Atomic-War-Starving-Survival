# Faction War Save Contract & Persistence Independence

> **Core Invariants:** Invariant 3 (Save Compatibility), Invariant 4 (Determinism)
> **Test Suite:** `Ashfall.Core.Tests.FactionWarLocationOverridesExpansionTests.BaseLocationDisplay_RestoredWhenOverrideExpires`

---

## 1. Zero Save Pollution Invariant

A fundamental strength of the Faction War location override architecture is that **no save state is allocated or mutated by location overrides**.

### 1.1 Pure Functional Evaluation
Overrides are evaluated as a pure function:
```
(locationId: string, currentDay: int) -> Option<FactionWarLocationOverride>
```
Because `currentDay` is already stored authoritatively in the master campaign clock (`IClock` / `CampaignSaveData`), and location identities are static catalog references:
- **No new save fields** are introduced to `CampaignSaveData.cs`.
- **No new save stores** are added to `src/Save/`.
- **No migration schemas** are required for existing player save files.
- `SaveChecksum` hashes for ongoing campaigns remain 100% valid and unaffected.

---

## 2. Cross-Version Save Compatibility

1. **Old Saves Loaded in New Game Version:**
   When an existing save file created at Day 250 is loaded into the new build, `FactionWarContentCatalog` immediately evaluates active overrides for Day 250 across all 20 entries. Locations such as `loc_garrison_checkpoint_gamma` seamlessly reflect their occupied state without any save-migration step.

2. **Save Round-Trip Invariance:**
   Saving and reloading at any day writes the exact same campaign save envelope before and after Plan 124.

3. **Restoration on Expiry:**
   When the campaign clock advances past `activeUntilDay`, the selector returns `null`. UI presentation layers immediately fall back to the base location's display name and description. No "cleared" flag needs to be persisted in save data.
