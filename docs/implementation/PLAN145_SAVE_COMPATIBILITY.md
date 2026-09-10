# Plan 145 Save Compatibility & State Discipline

Specification of state neutrality, schema version preservation, and backward compatibility for graffiti activation.

## 1. Zero New Save Sections

Because Plan 145 adopts **Model A (Time-Unlocked Ambient Text)**, wall text eligibility is a pure function of:
```csharp
f(locationId, campaignDay) -> IReadOnlyList<BunkerGraffitiEntry>
```

- **NO new save store:** No `GraffitiSaveStore` or new section is added to campaign save files.
- **NO save version bump:** Campaign save schema remains completely unaffected.
- **Old save compatibility:** Any existing save loaded on Day `D` will immediately and correctly project all markings authored up to Day `D` at whatever room or wasteland location the player visits.

## 2. Eleven-Point Compatibility Verification Matrix

| # | Test Scenario | Expected Outcome |
|---|---|---|
| 1 | Old save before posting unlock day (Day < recorded_day) | Posting is not shown in room/location inspection; no error. |
| 2 | Old save after unlock day (Day >= recorded_day) | Posting is immediately visible on inspection; no false notifications or popups. |
| 3 | Location undiscovered in wasteland | Location markings remain hidden behind standard fog-of-war. |
| 4 | Location discovered in wasteland | Markings appear inside `MapDetailPanel` when sector intelligence is viewed. |
| 5 | Save / Reload at inspected location | Inspection view reconstructs deterministically from catalog + current day. |
| 6 | Source file reordered | Postings sort deterministically by (`recorded_day`, `posting_id`); file order does not alter UI presentation. |
| 7 | Source gains a new posting | Newly added posting cleanly integrates into its mapped location upon reaching its unlock day. |
| 8 | Source removes a deprecated posting | Catalog simply stops yielding the posting; no save corruption or broken references. |
| 9 | Location mapping updated | Postings project to updated location immediately upon catalog reload. |
| 10 | Panel open during save / host rebind | Rebind invokes `RefreshView()` cleanly without state desync or memory leaks. |
| 11 | Repeated catalog load | Calling `Load()` multiple times does not duplicate entries or corrupt internal indexes. |
