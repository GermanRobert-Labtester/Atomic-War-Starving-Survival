# ACCEPTANCE — Content Expansion Pass (2026-09-07)

Slice: **content addition** — new authored entries in existing, runtime-consumed catalogs.
Distinct from the 2026-09-02/09-06 reauthoring passes, which edited prose in place only.
No new systems, no code paths, no schema changes; every entry uses vocabulary already present
in its file. One test count-pin moved because content was deliberately added (see F5).

## Added content

| File | Consumer (verified) | Entries | Total | Texts | Words | Choices |
|---|---|---|---|---|---|---|
| `door_encounters.json` | `DoorEncounterCatalogLoader` / `DoorEncounterSystem` | +12 | 68 → 80 | 86 | 3,183 | 31 |
| `quests_expansion_05.json` | `ExpansionQuestSystem` (loader list, `ExpansionQuestSystem.cs:353`) | +8 | 29 → 37 | 72 | 2,693 | 24 |
| `quests_expansion_06.json` | `ExpansionQuestSystem` (`:354`) | +6 | 13 → 19 | 54 | 2,198 | 18 |
| `confession_secrets.json` | `ConfessionSecretCatalog` / `ConfessionSecretSystem` | +8 | 26 → 34 | 56 | 4,223 | — |
| `radio.json` | `RadioBroadcastCatalog.LoadBaseRadioJson` | +18 | 65 → 83 | 18 | 1,319 | — |
| `narrative_encounters.json` | `NarrativeEncounterCatalogLoader` | +10 | 3 → 13 | 60 | 1,204 | 40 |
| `bunker_graffiti_postings.json` | **none — see F1** | +12 | 18 → 30 | 24 | 652 | — |
| **Total** | | **+74** | | **370** | **15,471** | **113** |

### What was added, and why those gaps

- **12 door encounters** (Year of Ash late campaign, day 185–360): garrison cartographer mapping
  your intake, Freeholder brine-well contract with a child-boarding clause, Guild brake-rider with
  a filed-down Garrison eagle on his crate, a scavenger child paid in a name cut on stone, the Ash
  Sign Door Witness counting doors, Foundry bell-caster melting casings, Hydro sub-auditor offering
  a smudged ledger, a well-diviner, a Rebuilder's unlabeled vault seed tin, a Supply Corps filter
  requisition for a hospital two valleys over, a Tempest barograph seller, and a Penal Battalion
  deserter in stolen boots. Fills the thin-faction tail: `ordnance_foundry` 1→2, `penal_battalion`
  1→2, `supply_corps` 1→2, `railway_guild` 2→3, `salt_freeholders` 2→3.
- **14 quests** incl. two multi-link chains authored through real quest-id prerequisites (the
  `quest_black_flotilla_trade` pattern; resolved by `IsCompleted`): the three-link **drowned
  archive** chain (`quest_the_drowned_archive` → `quest_names_on_the_door` →
  `quest_the_registry_deed`, names-before-infrastructure → refugees asking for the dead → the
  cistern field with six shafts and one marked DON'T) and the two-link **Nine Rails spur** chain
  (`quest_spur_of_nine_rails` → `quest_the_rails_decision`). Plus the chalk school, a
  two-part numbers-station investigation, green-water week, the Meridian census taker, chalk
  supply, the grave-marker carpenter, the springs audit, and the long-shift dog.
- **8 confession secrets** — one per under-served archetype (surgeon, quartermaster, journalist,
  mechanic, pilot, refugee, overseer, teacher), full 24-field schema with all five resolution
  branches (forgiveness / grudge / expose / blackmail / keep) authored per secret. All 25
  archetypes now carry at least one secret; 9 carry two.
- **18 radio broadcasts** giving three authored station personas their first content:
  **Voice of the Vitrified Crater** (104.2, liturgical — 6), **The Open Airways classroom &
  lineman** (91.3 — 6), **Automated Emergency Beacon Array** (142.85 teletype — 3), plus 3
  numbers-station transmissions at 99.0 MHz. Before this pass `radio.json` had zero entries at
  88.4 / 91.3 / 142.85 and one non-liturgical entry at 104.2, so all 65 broadcasts resolved to
  `station_civil_defense`. Message lengths trimmed to 376–423 chars against the file's 132–395.
- **10 expedition set-pieces** in the core encounter catalog, which held only 3 while
  `micro_locations.json` carries 28 quick site vignettes. The core slot is for character-driven
  scenes: the census carrier whose thirteenth door is yours, the other listener papering a relay
  hut with groups of five, the tower classroom broadcasting to a register of four absent names,
  the crater glass-blower, the surveyor still running lines for the dead, the orchard whose outer
  rows are staked, the ferryman whose toll is one true thing, the dog keeping a sealed door, the
  quarry witness with two contradicting accounts, and the grave with a garrison name over
  battalion boots.

### Cross-content web (deliberate, not incidental)

New entries reference each other through fiction, not IDs, so the world reads as one place:
census taker quest ↔ census carrier encounter ↔ Meridian standing; numbers-station quest ↔
ninth-night listener encounter ↔ the three 99.0 MHz transmissions (same interval, same
"RAIL. CUT." bearing, same municipal cistern codes as the archive chain's six-shaft sketch);
91.3 classroom broadcasts ↔ the tower classroom encounter ↔ `quest_the_chalk_school` ↔
`quest_chalk_supply` ↔ the teacher's unsent-letters secret; 104.2 crater sermons ↔ the
glass-blower encounter; penal battalion deserter at the door ↔ the wrong-name grave.

## Balance discipline

Every numeric field was audited against the **pre-existing envelope of its own catalog** before
finalizing. Four violations were found and normalized (74 values changed):

| Field | Pre-existing | As first authored | Normalized |
|---|---|---|---|
| `confession_secrets.blackmail_hardening_delta` | 0.1–0.25 | 0.25–0.35 | capped 0.25 (5 entries) |
| `confession_secrets.keep_trust_delta` | 15–35 | 35–45 | capped 35 (7 entries) |
| `narrative_encounters.moraleDelta` | −3–5 | −8–10 | rescaled monotonically (39 of 40 choices) |
| `narrative_encounters.guiltDelta` | 0–5 | 0–12 | rescaled monotonically (18 of 40 choices) |
| `narrative_encounters.stealthWeightMultiplier` | 0.5–1.3 | 1.2–1.4 | capped 1.3 (1 entry) |
| `door_encounters.baseGuiltDelta` | 0–40 | −5–20 | floor 0 (1 choice) |
| `quests_expansion_05.minDay/maxDay` | 20–110 / 90–320 | 40–150 / 180–330 | clamped (3 windows) |

Rescaling was order-preserving and sign-preserving, so each encounter's moral gradient survives
(the selfless choice still pays best and costs least guilt). All 19 audited fields across the four
catalogs now report in-range. Encounters are frequent random events, so the tighter pre-existing
swing envelope is treated as deliberate rather than as an absence of authoring.

Note on `narrative_encounters` weights: the 3 core entries all sat on the `EncounterDefinition`
C# defaults (`0.5` / `1.5`), so they are not an authoring precedent. The 28 `micro_locations`
entries in the same loader namespace do author variation (stealth 0.8/1.0/1.1/1.3, speed
0.5/0.6/0.8); new scenes follow that precedent — a person waiting quietly is found by careful
travel and missed by rushing.

## Findings (reported, not fixed — outside this slice)

- **F1 — `bunker_graffiti_postings.json` has no runtime consumer.** Zero references in
  `Assets/Ashfall.Core/` or `src/` outside `CatalogIntegrityRules.cs`/`CatalogIntegrityValidator.cs`,
  and no entry in `ContentUtilizationScanner`. All 30 postings (18 pre-existing + 12 new) are
  authored, validated, and unreachable. Pre-existing condition; the 12 additions extend an
  unwired catalog and are flagged so nobody counts them as player-facing. Needs a loader plus a
  wall/overlay route before any of it is seen.
- **F2 — `station_numbers_sigint` (14.487 MHz) is unreachable.** `RadioReceiverPlan` bands start
  at 50.0 MHz (VHF-Low 50–150, VHF-High 150–300, UHF-Low 300–550, UHF-High 550–950) and
  `TuneDelta` clamps to band, so the dial cannot reach 14.487. That station also has zero authored
  content in any radio file. Deliberately **not** authored into: dead data. The numbers-station
  material was placed at 99.0 MHz (reachable, and already the file's `NumbersStation` frequency),
  and the unreachable-band fact was made diegetic in the relay's continuity poll
  (`NUMBERS ARRAY, SHORTWAVE: NOT POLLABLE FROM THIS BAND`).
- **F3 — dangling `equipment_requirements`.** `radio_stations.json` references
  `equipment_receiver_standard`, `equipment_shortwave_receiver`, `equipment_vhf_beacon_receiver`;
  none exist in `items.json`. Not caught by `CatalogIntegrityValidator` (those keys are not in the
  TIER-2 reference list).
- **F4 — duplicate ash-cult faction ids.** `door_encounters.json` uses both `faction_ash_sign`
  (8 entries) and `cult_of_ash_sign` (1). Likely id drift; standing deltas on the outlier may not
  reach the intended faction.
- **F5 — count pin moved.** `NarrativeEncounterSystemTests.Catalog_LoadsTheThreeUnityEncounters`
  asserted `defs.Count == 62`; the loader aggregates three files (13 core set-pieces + 28
  micro-locations + 31 `enc_arc_*` NPC arcs), so deliberate addition makes it 72. Updated with a
  comment recording the composition. The three `Assert.Contains` Unity-parity checks — the actual
  intent of the test — are unchanged, as is `Catalog_UnityWeightParity` (it only inspects
  `enc_dead_letter_office`).
- **F6 — pre-existing test-isolation defect.**
  `Plan45Phase2BindingTests.TravelCatalog_CreatureEncountersCarryCombatantTags` fails when run
  alone (`tag 'pack_canine' binds unregistered id 'combatant_feral_mutt'`) and passes in the full
  suite. `CombatCatalog` is a static registry populated when `combat_catalog.json` is loaded, so
  the test depends on another test having run first. `combatant_feral_mutt` is defined at
  `combat_catalog.json:387` and mapped at `EnemyCompositionSelector.cs:151`. Not caused by this
  slice: `travel_encounters.json` (mtime 19:59), `combat_catalog.json`, and the Combat sources were
  untouched here, and the test passed in the full run before these additions.
- **F7 — `echoes.json` deliberately not extended.** `ContentUtilizationScanner:448` records
  `loaderPatterns["echoes.json"] = Array.Empty<string>(); // Future content, no loader`.
- **F8 — `wasteland_grave_epitaphs.json` has no gap.** Keyed by the closed `SurvivorDeathCause`
  enum's 8 values; adding a cause would never be selected.
- **F9 — `verdict_data.json`'s 3 endings are complete, not thin.** Each carries a `trigger`
  expression consumed by `VerdictEndingEvaluator`/`CampaignOutcomeEvaluator`, and the three cover
  the census decision matrix (presented+honored / not presented / declined). Extending requires
  evaluator code, not prose.
- **Concurrent streams.** The working tree is shared. During this slice other streams modified
  `trade_tell_lines.json`, `warlord_doctrines.json`, `feedback_messages.json`,
  `year_of_ash_questlines.json`, and added tests (suite count moved 9351 → 9357 → 9364 across
  three runs). Changes here are confined to the 7 data files and 1 test file listed above.

## Verification gates

| Gate | Result |
|---|---|
| JSON validity, all 7 edited files | PASS (re-parsed after every write) |
| `dotnet test Ashfall.Core.Tests` (full) | PASS — 9364/9364, 0 failed |
| `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| `godot --headless -- --data-integrity-selftest` | PASS — 0 errors / 0 warnings, 298 catalogs |
| `godot --headless -- --bridge-selftest` | PASS, exit 0 |
| `DataRuleComplianceTests` | PASS |
| Tone scan (real countries / fantasy / glorified violence) over 370 new texts | 1 hit, STYLE only: `"We do not say the glass is holy"` — an explicit negation of sanctity, anti-supernatural, compliant |
| Id uniqueness (per-file + cross-file where the loader enforces it) | PASS — 0 collisions; radio ids checked against `faction_radio_corpus.json` per `FactionRadioBroadcastExpansionTests`; encounter ids checked against all 31 in the shared builder namespace, which throws on duplicates |
| Numeric envelopes vs pre-existing catalog ranges | PASS — 19/19 fields in range after normalization |
| Placeholder / token preservation | PASS — `{name}` in all 8 secrets; quest-id prerequisites resolve; `minDay <= maxDay` everywhere |
