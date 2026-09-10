# Plan 145 Day & Discovery Semantics

Specification of temporal unlocking, query contracts, and save-neutral discovery semantics for graffiti and wall text.

## 1. Primary Model: Model A (Time-Unlocked Ambient Text)

Plan 145 adopts **Model A (Time-unlocked ambient text)**:

- A wall posting is part of the physical environment once the campaign reaches `recorded_day`.
- **Query Rule:** `posting.recorded_day <= currentDay`.
- **Eligibility:**
  - On Day `N - 1`: The posting is not yet written / not yet eligible.
  - On Day `N`: The posting is authored and becomes eligible for viewing.
  - On Day `N + 1` and all future days: The posting remains permanently legible on the wall.
- **Repeatability:** Wall text is ambient physical geometry. It is repeatedly readable whenever the player inspects that location or room, without vanishing or expiring.

## 2. Invariant: No Retroactive Consequence on Day Advance

`recorded_day` represents an eligibility boundary, NOT an event trigger.
- Advancing the day does NOT emit an alert, mutation, or notification solely because a posting becomes queryable.
- Loading an older save (e.g. at Day 100) simply exposes all postings authored up to Day 100 without triggering "new" popups, quest updates, or state changes.
- Reading wall text is strictly presentation-only.

## 3. Timeline Distribution Across the Corpus

The 76 postings span from Day 1 to Day 3650 (10 full years):

| Era | Day Range | Postings Count | Content Focus |
|---|---|---:|---|
| **Immediate Crisis** | Days 1–14 | 14 | Key locations, pump directions, initial stoker warnings, biscuit complaints |
| **Early Adaptation** | Days 15–30 | 18 | School announcements, cold bunk complaints, water rationing, first theft accusations |
| **First Quarter** | Days 31–100 | 18 | Medical triage tallies, air filter occlusions, clandestine slogans, radio whispers |
| **Year One** | Days 101–365 | 7 | Sentry orders, shoemaker resole notes, crypt psalms, assembly pledges |
| **The Long Middle** | Days 366–2000 | 11 | Foundry quotas, rat races, candle strikes, red snow sentry rhymes, wedding bands |
| **Ten-Year Epilogue** | Days 2001–3650 | 8 | Secret poetry shelves, pancake feast, charter ratification, final outer blast door greeting |

## 4. Query Contract in `BunkerGraffitiCatalog`

```csharp
public List<BunkerGraffitiEntry> GetUnlockedByDay(int currentDay);
public List<BunkerGraffitiEntry> GetPostingsForTarget(string targetId, int currentDay);
```

- If `currentDay < 0`, returns an empty list.
- If `currentDay >= 3650`, returns all eligible postings for that location.
- Results are deterministically sorted by `recorded_day` ascending, then by `posting_id` ordinal.
