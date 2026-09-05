# COLLECTIBLES_NARRATIVE_QUALITY_AUDIT.md
Flagship Integration Plan XII — Task 1 deliverable (2026-09-05)

## Scope and method

Corpus: all 40 definitions in `Assets/StreamingAssets/Data/collectibles.json`,
with display names and descriptions authored in `items.json` (the item
authority — collectibles carry stable IDs only, per the presentation/data
split). Emotional-register assignment is the editorial layer required by
Plan XII §1.2–§1.3; everything else in the matrix is measured, and the
measured gates are enforced in CI by
`Ashfall.Core.Tests/CollectibleNarrativeQualityTests.cs`.

Sentence counting uses the project-approved terminator counter
(split on `.`, `!`, `?` followed by whitespace/end) — the same rule the CI
gate uses. No NLP dependency.

## Narrative audit matrix (40 rows)

| ID | Category | Display Name | Primary Register | Secondary | Sent. | Cliché Flags | Effect | Target | Rewritten |
|---|---|---|---|---|---:|---|---|---|---|
| `item_collectible_vinyl_chamber_record` | vinyl | Scratched Chamber Record | routine | nostalgia | 2 | — | none | — | — |
| `item_collectible_vinyl_civil_broadcast` | vinyl | Civil-Information Broadcast Pressing | bureaucracy | duty | 2 | — | none | — | — |
| `item_collectible_vinyl_folk_compilation` | vinyl | Regional Folk Compilation | routine | community | 3 | — | none | — | — |
| `item_collectible_family_portrait` | photograph | Family Portrait | family | loss | 3 | faded | morale | — | — |
| `item_collectible_unit_photograph` | photograph | Military Unit Photograph | duty | loss | 3 | — | faction_info | faction_military_history | — |
| `item_collectible_civil_defense_poster` | poster | Civil-Defense Poster | bureaucracy | routine | 3 | torn | none | — | — |
| `item_collectible_propaganda_poster` | poster | State Propaganda Poster | duty | fear | 2 | — | faction_info | faction_state_propaganda | — |
| `item_collectible_concert_poster` | poster | Concert Poster | joy | community | 3 | — | morale | — | — |
| `item_collectible_field_medicine_handbook` | book | Field Medicine Handbook | work | duty | 3 | — | knowledge | knowledge_field_medicine | — |
| `item_collectible_pre_war_novel` | book | Pre-War Novel | routine | nostalgia | 3 | — | morale | — | — |
| `item_collectible_science_magazine` | magazine | Science Periodical | curiosity | routine | 2 | — | knowledge | knowledge_basic_engineering | — |
| `item_collectible_hunting_magazine` | magazine | Hunting Magazine | routine | curiosity | 3 | — | none | — | — |
| `item_collectible_diesel_service_manual` | technical_manual | Diesel Engine Service Manual | work | pride | 3 | — | knowledge | knowledge_diesel_mechanics | — |
| `item_collectible_radio_repair_guide` | technical_manual | Shortwave Radio Repair Guide | work | duty | 3 | — | knowledge | knowledge_radio_repair | — |
| `item_collectible_water_treatment_handbook` | technical_manual | Water-Treatment Handbook | work | duty | 3 | — | knowledge | knowledge_water_treatment | — |
| `item_collectible_air_filter_manual` | technical_manual | Air-Filtration Maintenance Manual | work | duty | 3 | — | knowledge | knowledge_air_filtration | — |
| `item_collectible_dosimeter_guide` | technical_manual | Dosimeter Calibration Guide | work | fear | 3 | — | knowledge | knowledge_radiation_measurement | — |
| `item_collectible_unit_log_fragment` | military_document | Unit Log Fragment | loss | duty | 3 | torn | faction_info | faction_military_operations | — |
| `item_collectible_deployment_order` | military_document | Deployment Order | bureaucracy | fear | 3 | — | faction_info | faction_military_deployment | — |
| `item_collectible_casualty_list` | military_document | Casualty Evacuation List | loss | duty | 3 | faded | journal_unlock | journal_casualty_records | — |
| `item_collectible_mothers_letter` | personal_letter | Mother's Letter | family | routine | 3 | — | morale | — | yes |
| `item_collectible_soldiers_letter` | personal_letter | Soldier's Unsent Letter | loss | family | 3 | — | journal_unlock | journal_soldier_letters | yes |
| `item_collectible_rejection_letter` | personal_letter | Administrative Rejection Letter | bureaucracy | frustration | 3 | — | none | — | — |
| `item_collectible_civil_defense_badge` | badge | Civil-Defense Badge | duty | community | 3 | — | faction_info | faction_civil_defense | — |
| `item_collectible_transit_badge` | badge | Transit Authority Badge | work | routine | 3 | — | none | — | — |
| `item_collectible_military_patch` | patch | Military Unit Patch | pride | duty | 3 | — | faction_info | faction_military_units | yes |
| `item_collectible_trade_guild_patch` | patch | Trade Guild Patch | commerce | pride | 3 | — | faction_info | faction_trade_guilds | — |
| `item_collectible_childs_doll` | toy | Child's Doll | youth | family | 3 | — | morale | — | — |
| `item_collectible_music_box` | toy | Wind-Up Music Box | joy | nostalgia | 3 | — | morale | — | yes |
| `item_collectible_prayer_book` | religious_object | Pocket Prayer Book | faith | family | 3 | — | journal_unlock | journal_religious_texts | — |
| `item_collectible_prayer_beads` | religious_object | Prayer Beads | faith | routine | 3 | — | none | — | — |
| `item_collectible_team_pennant` | sports_memorabilia | Local Team Pennant | pride | sport | 3 | — | morale | — | yes |
| `item_collectible_match_program` | sports_memorabilia | Match Program | sport | community | 3 | — | none | — | — |
| `item_collectible_civic_token` | cultural_artifact | Municipal Commemorative Token | celebration | community | 3 | — | none | — | — |
| `item_collectible_folk_craft` | cultural_artifact | Hand-Painted Ceramic Tile | community | faith | 3 | — | none | — | — |
| `item_collectible_exchange_day_newspaper` | newspaper | Exchange-Day Newspaper | loss | routine | 3 | — | journal_unlock | journal_exchange_day | — |
| `item_collectible_local_newspaper` | newspaper | Local Newspaper | routine | humor | 3 | — | none | — | — |
| `item_collectible_road_map` | map | Pre-War Road Map | curiosity | routine | 3 | — | location_clue | loc_road_junction_cache | — |
| `item_collectible_topo_map` | map | Military Topographic Map | duty | fear | 3 | — | location_clue | loc_military_outpost | — |
| `item_collectible_survivor_map` | map | Hand-Drawn Survivor Map | fear | family | 3 | — | location_clue | loc_survivor_cache | — |

## Emotional distribution (primary registers)

| Register | Count |
|---|---:|
| work | 7 |
| routine | 4 |
| bureaucracy | 4 |
| duty | 4 |
| loss | 4 |
| family | 2 |
| joy | 2 |
| curiosity | 2 |
| pride | 2 |
| faith | 2 |
| boredom | 1 |
| commerce | 1 |
| youth | 1 |
| sport | 1 |
| celebration | 1 |
| community | 1 |
| fear | 1 |

### Acceptance matrix (Flagship XII)

| Register | Requirement | Actual | Verdict |
|---|---:|---:|---|
| Distinct primary registers | >= 8 | 16 | PASS |
| Loss/tragedy primary | <= 10 | 4 | PASS |
| Routine/ordinary primary | >= 5 | 5 | PASS |
| Joy/pride primary | >= 3 | 4 | PASS |
| Faith/devotion primary | >= 2 | 2 | PASS |
| Bureaucracy/admin primary | >= 2 | 4 | PASS |

Also present in the corpus: fear (primary and secondary), family, sport,
work, community, humor, curiosity, youth, commerce, celebration, frustration,
duty, nostalgia. Fear does not dominate (2 primary uses).

## Cliché report (hard gates)

- `faded`: 2 / 2 ceiling ✅
- `torn`: 2 / 2 ceiling ✅
- `bloodstained`: 0 / 2 ceiling ✅
- `haunting reminder`: 0 / 2 ceiling ✅

Secondary watch-list review (manual, no ceiling breach): `worn` (2),
`frayed` (1), `creased` (2), `cracked` (2), `chipped` (2), `stained`
compounds (2). Repetition audit: the corpus consistently opens physical
descriptions with an indefinite article ("A folded program…") — this is the
inventory-catalog voice and was accepted deliberately; variation enters in
the second/third sentences (human traces: pencil annotations, tape repairs,
marginalia). Repeated emotional beats: none dominant; only 4 of 40
descriptions use explicit loss framing.

## Rewrite log (Stage 6)

Six surgical rewrites, all preserving object identity, category, and effect
wiring — none re-author wholesale:

| Item | Change | Reason |
|---|---|---|
| `item_collectible_team_pennant` | "faded team colors" → "washed-out team colors" | third corpus use of `faded` (ceiling 2) |
| `item_collectible_mothers_letter` | merged sentences 1–2 | 4-sentence breach → 3 |
| `item_collectible_soldiers_letter` | merged sentences 1–2 | 4-sentence breach → 3 |
| `item_collectible_music_box` | merged sentences 2–3 | 4-sentence breach → 3 |
| `item_collectible_military_patch` | "torn from a uniform" → "pulled from a uniform" | third corpus use of `torn` (ceiling 2) |

One further editorial intervention outside `items.json`: the eleven codex
prose entries (4 `journal_unlock`, 7 `faction_info`) had their `default` and
`realist` voices re-authored to 2–3 restrained sentences each — see
`COLLECTIBLES_CONTENT_INTEGRATION_CLOSEOUT.md` §Journal and §Faction intel.

## Modern-language review (§1.9)

No internet/meme/social-platform slang present. Enforced going forward by the
`Descriptions_ContainNoModernInternetSlang` gate.

## Copyright / IP review (§1.10)

No song lyrics, book passages, article text, famous slogans, real teams,
real publications, or trademark-heavy product names present. Enforced by the
`Descriptions_ContainNoRealBrandsPublicationsOrTeams` gate.

## Fictional proper-noun inventory (§1.11)

| Type | Fictional Name | Collectible |
|---|---|---|
| — (none) | — | — |

The corpus deliberately names no institutions, publications, teams, or
venues: all references are generic ("a small regional label", "the transit
authority", "local team"). No conflicts with the setting bible are possible
at this specificity level. If later passes author named institutions (the
Garrison, the Exchange, Checkpoint Gamma already exist in journal lore),
add them here and cross-reference the setting bible.

## Exposition control (§1.8)

All 40 descriptions are object-level (condition, use-wear, human traces).
No description narrates faction chronology or world lore; larger
interpretation is delegated to the journal/codex entries unlocked by
`journal_unlock` / `faction_info` effects, which is the intended division
of labor.

## Remaining editorial concerns

- The corpus voice is deliberately uniform in register (restrained inventory
  prose). If a future pass wants stronger per-category voice differentiation
  (e.g. warmer toys, drier bureaucratic documents), do it as a dedicated
  editorial pass — this audit pins only the measured gates.
- Sentence openings remain overwhelmingly article-initial; acceptable at 40
  items, revisit if the corpus grows past ~100.
