# Plan 210 — Waste Management & Sanitation: Closeout

**Status:** COMPLETE · **Date:** 2026-09-13 · **Batch:** `PLANS-210-213-FLAGSHIP-ECONOMY`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/Shelter/SanitationSystem.cs` (`sanitation`), `SanitationFacilityCatalog.cs`, `SanitationConsequenceRules.cs` |
| Data | `Assets/StreamingAssets/Data/sanitation_facilities.json` (9 facilities; closed waste-type + room-tag vocabularies) |
| Host | `src/Host/SanitationHostSession.cs`, `src/Host/SanitationSaveStore.cs`, `src/Main.Sanitation.cs` |
| Save | `SaveSectionRegistry` row (`shelter`) + `sanitation_save.json` |
| UI | `src/UI/SanitationPanel.cs`, route `sanitation` (Expanded; text/label severity — never color-only) |
| Day owner | `hygiene` (phase 3 — sorts before `medical_disease`) |
| Tests | Catalog 8/8 · engine 20/20 · wiring 2/2 · cross-plan sweep inside 8/8 |

## Contract guarantees

- **Room-level waste (D2).** Organic scales with population; chemical/radioactive accumulate ONLY through the producer API (`EmitWaste`) — no silent auto-emission, no silent deletion.
- **Hygiene is derived, never stored.** Waste burden → per-room/shelter permille → Excellent/Acceptable/Poor/Squalid/Hazardous. Active spill caps room hygiene.
- **Disease seam is one-directional.** Sanitation supplies a bounded exposure modifier `[1.0, 2.0]`; `DiseaseSystem.TryExpose` owns infection outcomes (§170.8). Wave 6 wired the daily sweep through the AUTHORED `foul_water_draw` cholera source.
- **Compost is type-guarded.** Organic → batches → existing `item_compost_humus` (live consumer); chemical/radioactive rejected by Core type check.
- **Spills are deterministic.** Overcapacity-only trigger; no arbitrary catastrophe within safe capacity; resolved by critical cleaning + capacity headroom; persisted exactly across save/restore.
- **Morale coupling is reversible.** `mark_sanitation_hazardous` set on collapse, CLEARED on recovery — temporary mess never becomes permanent damage.
- **Legacy saves load clean** — zero waste, no spill, neutral modifiers. Never a hazardous shelter materializing from a pre-210 save.

## Deferred (flagged, not silent)

Power-grid `powered`/`staffed` feed for facilities and per-room occupancy distribution land with Wave 7+ integration passes; facility state already carries the fields.
