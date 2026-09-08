# Plan 67 — Cassette Sets Expansion — Closeout

**Status: COMPLETE**

Date: 2026-09-06. Mode: pure DATA + narrative-authoring pass. Zero Core/host code changes.

## Runtime schema (see `CASSETTE_SET_RUNTIME_CONTRACT.md`)

`cassette_sets.json` — sets of `{set_id, set_title, total_parts, parts[{part, item_id, title, description}], hidden_cache_location, hidden_cache_items, completion_narrative?}`. Part `item_id` is a definition position; `hidden_cache_*` and `completion_narrative` are integrity-validated references. No `location_hint` / `journal_unlock` fields exist; none were added.

## Existing set audit

4 existing sets audited and preserved untouched: `checkpoint_kilo` (military, 4 parts), `hospital_saint_maren` (hospital triage, 3), `family_bunker` (family bunker, 3), `resistance_broadcasts` (pirate radio, 4). Plan 06B's 23 echoes are single-object vignettes — no set-level overlap. Full matrix: `PLAN_67_CASSETTE_COVERAGE_MATRIX.md`.

## Final catalog — 12 sets / 48 parts

| # | set_id | Title | Parts | Speaker | Ending mode |
|---|---|---|---:|---|---|
| 1 | checkpoint_kilo | The Last Days of Checkpoint Kilo | 4 | corporal | final warning |
| 2 | hospital_saint_maren | The Saint Maren Tapes | 3 | doctor | transfer of responsibility |
| 3 | family_bunker | The Martinez Family Recordings | 3 | father | implied death |
| 4 | resistance_broadcasts | The Free Radio Tapes | 4 | pirate broadcaster | sign-off under threat |
| 5 | field_hospital_7 | Field Hospital 7 | 5 | nurse/orderly | departure; tags left behind |
| 6 | evacuation_train | The Evacuation Train | 4 | conductor | dispersal at end of line |
| 7 | station_14 | Station 14 | 6 | radio operator | operational continuity |
| 8 | greenhouse_tapes | The Greenhouse Tapes | 3 | agri-technician | seed stock transferred |
| 9 | fathers_tapes | Father's Tapes | 4 | father | unresolved apology |
| 10 | dam_keeper_log | The Dam Keeper's Log | 5 | dam operator | deliberate manual shutdown |
| 11 | teachers_recordings | The Teacher's Recordings | 3 | teacher | routine maintained |
| 12 | quarantine_tapes | The Quarantine Tapes | 4 | public-health doctor | criteria without means |

New parts: 5+4+6+3+4+5+3+4 = **34**. Every `total_parts` matches actual count; part numbers are ordered 1..N with no duplicates; new `item_id`s are unique.

## Items — 34 part records

34 `Media` items added to `items.json` (`cassette_<set>_<n>`, ids matching the part `item_id` definitions), `stackMax: 1`, `weight: 0.1`, `tradeValue` 8–9, `moraleEffect` 2–3 — consistent with the existing `item_cassette_tape` and the collectible-item pattern.

## Scavenging placement — all 8 new sets (target was 6)

34 entries added to `scavenging_tables.json` (weight 4–6, quantity 1, uncommon for early parts / rare for later). Story-shaped spread, e.g. Station 14 across relay mast, observatory, metro, archive, concert hall, transit depot; Dam Keeper across substation, industrial district, waterworks, geothermal plant, tank farm. No unmerged table IDs used (Plan 46 tables were merged: 49 tables). Uniqueness via `stackMax: 1`; cassette parts cannot flood loot (low weights).

## Hidden caches — 8 resolved references

Each new set carries a `hidden_cache_location` (existing locations: `prewar_medical_cache`, `loc_transit_authority_hq`, `loc_radio_relay_mast`, `loc_seed_library_annex`, `suburban_house`, `location_substation_omega`, `loc_school_gymnasium`, `loc_shelter_infirmary`) and `hidden_cache_items` (all resolving item ids).

## Completion narratives — 4 journal hooks (target met)

`narrative_cassette_field_hospital_7_complete`, `narrative_cassette_station_14_complete`, `narrative_cassette_dam_keeper_log_complete`, `narrative_cassette_quarantine_tapes_complete` added to `events.json` (`weight: 1`, `minDay: 18`, existing pattern). Each synthesizes what the whole set establishes beyond any single tape. Father's/Teacher's/Greenhouse/Train intentionally left without a codex summary per plan guidance.

## Deferred / not applicable (verified, not failed)

- **No set-cassette playback/collection runtime exists** (`RadioRecordingSystem` is a separate broadcast-recording feature). Sequencing, out-of-order discovery, duplicate-acquisition and save-round-trip behavior (67J.15–67J.17) therefore have no runtime state to exercise; the data layer is placement- and integrity-safe by construction. Runtime playback remains a future host feature and will read this catalog as-is.
- No morale values added beyond item-level `moraleEffect`; `VinylMoraleSystem` untouched.
- No new scavenging tables, no Plan 71 IDs, no canon expansion (fictional names only: Voss, Ostrowski, Kolar, Rehn, Ehlers, Halim, Mira, Havelkow, Mirefield, Havel Junction).

## Verification record

| Check | Result |
|---|---|
| `godot --headless -- --data-integrity-selftest` | **PASS** — 0 findings, 0 errors/0 warnings, 298 catalogs |
| `godot --headless -- --content-utilization-selftest` | **PASS** — CI gate PASS, Orphaned 0, Unresolved 70 (unchanged from baseline) |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS** — 9407/9407 (one intermediate failure — a transient `moraleEffect` string-typed field in my own batch — was fixed and re-run clean) |
| `dotnet build Ashfall.csproj` | **PASS** — 0 warnings, 0 errors |
| Catalog count validation | 12 sets / 48 parts / 34 new items / 34 placements — script-verified |

## Concurrent-work note

`items.json` and `scavenging_tables.json` carried pre-existing uncommitted description rewrites from another stream in the working tree. Those edits were preserved byte-for-byte (JSON round-trip); my changes are additive-only. Commit scoping should account for that in-flight work.
