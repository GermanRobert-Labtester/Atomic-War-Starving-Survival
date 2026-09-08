# Moral Band Range Contract & Authority

## 1. Canonical Moral Bands
The canonical moral-band authority is `Ashfall.Core.MoralChoice.MoralPathBand`:

| Ordinal | Token | Enum Name | Moral Score Range (`MoralChoiceSystem.BandForScore`) |
|---|---|---|---|
| 0 | `very_evil` | `MoralPathBand.VeryEvil` | `score <= -100` |
| 1 | `evil` | `MoralPathBand.Evil` | `-99 <= score <= -50` |
| 2 | `slightly_evil` | `MoralPathBand.SlightlyEvil` | `-49 <= score < 0` |
| 3 | `neutral` | `MoralPathBand.Neutral` | `score == 0` |
| 4 | `slightly_positive` | `MoralPathBand.SlightlyPositive` | `1 <= score < 50` |
| 5 | `positive` | `MoralPathBand.Positive` | `50 <= score < 100` |
| 6 | `very_positive` | `MoralPathBand.VeryPositive` | `score >= 100` |

---

## 2. Comparison Semantics
In `IndependentBranchSystem.cs`:
- Bands are parsed to `MoralPathBand` via `ParseBand(string token)`.
- Comparison operators (`<`, `<=`, `>`, `>=`) evaluate ordinal integers `0` through `6`.
- Range inclusivity is closed: `band >= min && band <= max`.

---

## 3. Seven-Band Partition Contract for New Branches
For all 7 newly authored branches (IND-9 through IND-15), endings are structured into an exact 3-piece partition:
- **Ending C (Dark / Catastrophic):** `very_evil` .. `slightly_evil` (ordinals 0, 1, 2)
- **Ending B (Mixed / Neutral / Complicated):** `neutral` .. `slightly_positive` (ordinals 3, 4)
- **Ending A (Constructive / Unifying / Virtuous):** `positive` .. `very_positive` (ordinals 5, 6)

Every valid moral band maps to exactly one ending with 0 gaps and 0 overlaps.
