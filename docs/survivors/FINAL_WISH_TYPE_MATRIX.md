# Final Wish Type Matrix

**Document:** `docs/survivors/FINAL_WISH_TYPE_MATRIX.md`

---

## 1. Type Distribution Overview

The 30 wishes span 10 primary wish types plus 2 legacy initial categories:

| Wish Type | Original 8 | New 22 | Total 30 | Runtime Step Count Contract |
|---|---|---|---|---|
| `teach_lesson` | 3 | 3 | 6 | 2 steps (explicit in switch) |
| `deliver_letter` | 0 | 3 | 3 | 2 steps (explicit in switch) |
| `see_a_place` | 0 | 2 | 2 | 2 steps (`_ => 2` default branch) |
| `see_the_sky` (legacy) | 1 | 0 | 1 | 1 step (explicit in switch) |
| `reconcile` | 1 | 2 | 3 | 2 steps (explicit in switch) |
| `die_with_dignity` | 0 | 3 | 3 | 2 steps (`_ => 2` default branch) |
| `last_meal` | 0 | 2 | 2 | 2 steps (`_ => 2` default branch) |
| `confess` | 0 | 2 | 2 | 2 steps (`_ => 2` default branch) |
| `protect_someone` | 0 | 2 | 2 | 2 steps (`_ => 2` default branch) |
| `return_a_relic` | 0 | 2 | 2 | 2 steps (`_ => 2` default branch) |
| `retrieve_heirloom` (legacy) | 2 | 0 | 2 | 2 steps (explicit in switch) |
| `name_a_successor` | 0 | 1 | 1 | 2 steps (`_ => 2` default branch) |
| `build_memorial` (legacy) | 1 | 0 | 1 | 3 steps (explicit in switch) |
| **Totals** | **8** | **22** | **30** | — |

---

## 2. Runtime Switch Compatibility

In `FinalWishSystem.cs:134-143`:
```csharp
int requiredSteps = state.wishType switch
{
    WishRetrieveHeirloom => 2,
    WishDeliverLetter => 2,
    WishBuildMemorial => 3,
    WishTeachLesson => 2,
    WishReconcile => 2,
    WishSeeTheSky => 1,
    _ => 2
};
```
Every single new wish type (`see_a_place`, `die_with_dignity`, `last_meal`, `confess`, `protect_someone`, `return_a_relic`, `name_a_successor`) routes safely and deterministically to the 2-step default, matching their authored 2-step structure. Zero engine code changes required.
