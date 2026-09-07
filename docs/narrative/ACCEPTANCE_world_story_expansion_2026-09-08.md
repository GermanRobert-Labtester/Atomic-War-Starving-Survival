# ACCEPTANCE — World Story Expansion (2026-09-08)

Slice: **new authored world/story content**, targeted at gaps that were proven necessary rather
than chosen for volume. Two sources of evidence drove target selection: (1) which catalogs have a
real runtime loader, and (2) which quarantined tests specify content that was never written.

## What was added

| File | Consumer (verified) | Entries | Total | Prose |
|---|---|---|---|---|
| `incidents.json` | `EventsHostSession.LoadIncidents()` (`res://` read model) | +20 | 5 → 25 | 20 titles + 20 bodies, ~1,900 words |
| `faction_lore.json` | `CombatCatalogLoader` (registry), `FactionDisplayNameCatalog`, `WarlordDoctrineCatalog` | +22 | 23 → 45 | 170 blocks, ~4,750 words |
| `door_encounters.json` | `DoorEncounterSystem` | 0 (1 canon fix) | 80 | prose-only repair |
| `Ashfall.Core.Tests.csproj` | — | 1 line | 80 → 79 quarantines | unquarantined `Plan57IncidentTests.cs` |

### 1. Plan 57 — the 20 missing shelter incidents (live catalog)

`Plan57IncidentTests.cs` was quarantined (csproj line 44) and specified **25 incidents**; the file
had 5. The 20 missing ids, their 8-category distribution, weight bands, phase gating and required
grounding phrases were all pinned by that test, so this was a documented-unlanded plan rather than
invented content. Written to the full contract:

- **Categories** (documented, deliberately *not* serialized — Case D forbids dead fields):
  environmental 3, security 3, medical 3, social 3, equipment 3, supply 2, external 2,
  psychological 1.
- **Phase gating**: 6 early (`0<minDay<30`), 9 mid (30–69), 5 late (≥70);
  `incident_exchange_anniversary` at exactly day 90.
- **Weights**: 11 distinct values across the 25; all new ≤1.3, all within 0.1–1.5.
- **Grounding phrases** required by the test, all present: `not cloud`, `sealed service access`,
  `tool marks`, `upstream`, `transmitter`, `every eleven minutes`, `Rebuilder`, `Iron Garrison`.
- **Original 5 preserved byte-for-byte** (verified field-by-field against HEAD).
- **Schema held to the 5 consumed fields only**; the raw file contains none of `"category"`,
  `"maxDay"`, `"choices"`, `"consequences"`, `"faction"`, `"system_link"`, `"cooldown"`.
- ASCII-only, indent 4, trailing newline — matching the file exactly.

**The quarantined test was unquarantined and now passes 12/12.** A dead spec became a live
regression gate.

### 2. Faction lore — 22 factions that act in the world but had no lore

59 distinct faction ids are referenced across the data authority; only 23 had lore. This matters
mechanically, not just cosmetically: `CombatCatalogLoader.Load` **throws `FormatException`** when a
combatant's `faction_id` does not resolve in `faction_lore.json` (`Plan10RemediationTests`), and
`FactionDisplayNameCatalog.LoadFromJson` takes UI display names from it — so these factions were
rendering as raw ids.

Authored for: Archivists of the Before, Lamplighters, Quiet House, Grain Exchange, Sun-Seekers,
Osteophages, Tally, Undertow, Cold Count, Deserter Coalition, Provisioned, Long Walk, Scavenger
Guild, Iron Raiders, Tempest, Black Flotilla, Silent Foundry, the Office, the Cutters, the
Compact, the Fleet, the Scale.

Every entry is grounded in existing canon rather than invented:

- **`currents.json`** (live: `CurrentsCatalog`/`MusterHostSession`/`CurrentsRosterWidget`) supplied
  display_name, alignment, home_region, is_active, wants, offers, signature_quote and access_rule
  for 15 of the 22. **All 15 display names match canon exactly**; wants/offers were carried into
  `tribute_demands`/`tech_offerings`; access rules were dramatized into origin stories (the
  Lamplighters' eleven-day lamp sequence, the Tally's double reading, the Undertow's price agreed
  in the water, the Cold Count's four-name roster, the Guild's permanent blacklist).
- **`faction_territory.json`** supplied territory, classification, scale and resource interest for
  the rest (Black Flotilla's lighthouse bluff and marine optics, the Office's weighbridge network,
  the Cutters' brine basins, the Fleet's roadstead, the Tally's Lock Seven, the Scavenger Guild's
  Tinker's Notch).
- **`characters.json`** supplied named figures, used as written: Mirael Tesk, Mara Veln, Kaspar
  Drej, Joren Malk, Edor Vale, Yara Holm, Ivy Corrigan, Halloran Vesk.
- **Quest chains** already in the authority were referenced as history: Bone Pickers, Blood Tithe,
  Broken Spears, Iron Slaves, Plowshares, Hospital Ships, Quarantine Purge, First Refusal.
- **`crossing_factions.json`** supplied The Scale / The Compact alignments and wants.
- Test-pinned names respected verbatim: "The Black Flotilla" (`Plan23FlotillaFactionDepthTests:42`),
  "The Silent Foundry" (`ShelterMachineTellTests:87`).

Craft decisions worth recording:

- `faction_iron_raiders` and `faction_osteophages` use the file's **6-key form** (no
  dialogue_style, no signature_quote) on purpose: `currents.json` gives both an *empty*
  signature_quote, and for the Raiders the absence of offers is explicitly "the design". Omitting a
  voice is more faithful than inventing one. Both keysets already existed in the file (13/10).
- 85 relationship edges were authored between factions, values restricted to the existing
  10-value vocabulary (`hostile`, `suspicious`, `neutral`, `hostile-open`, `wary`,
  `transactional`, `estranged`, `wary-truce`, `mentor-archetype`, `parallel`), so the factions now
  form a web rather than 45 isolated dossiers.
- No lore `signature_quote` duplicates its `currents.json` quote verbatim (one near-duplicate on
  The Tally was rewritten so the two catalogs complement instead of repeat). All 33 lore quotes are
  unique.

**Deliberately excluded:** `faction_blank_rows` (`DutyRosterIntegrationTests:80` asserts lore must
*not* contain it), and the sentinel/archetype ids that are not world factions —
`faction_independent`, `faction_rebel`, `faction_military`, `faction_order`, `faction_iron_clique`,
`faction_meridian`, `faction_unlisted`, `faction_unknown_intelligence`,
`faction_automated_infrastructure`, `faction_independent_survivors`, `faction_civil_defense`,
`faction_doctrine_archetype_*`. Giving a sentinel a dossier would surface it in the UI as a faction.

### 3. Canon repair — a contradiction introduced in round 4 (commit `04806918`)

`door_encounter_tempest_barograph_seller` characterized `faction_the_tempest` as **storm-chasers**
("Kessa Vane, storm-chaser", "fuel cells for the chase rig"). Authoritative canon in
`currents.json` says the opposite: The Tempest is a **metering/relay service** —
*"The Tempest is not alive and has no preferences. It serves, it meters, and it waits for a human
to read the meter"*, wants `maintenance_time`/`readings`/`a_presented_count`, offers
`machine_log_access`/`scheduled_q`/`archive_proof`. Corroborated by the pre-existing
`door_encounter_verdict_relay_repair` (a Tempest technician in insulated gauntlets at a conduit
array).

Fixed in place, prose only. The visitor is now **Meter-Reader Kessa Vane**, on Tempest business to
read the conduit meter, carrying a barograph drum that is her *own off-contract log* — "the Tempest
has never once mentioned weather". The gameplay value is unchanged (storm-track intel for two fuel
cells) and the contradiction became canon depth. Verified unchanged: `visitorFaction`, `minDay`,
`maxDay`, `threatLevel`, and every mechanical choice field (`requiredItemId` `fuel_cell`, qty 2,
morale +10/−5, standing +10/−5). One `choiceId` renamed (`choice_send_the_chaser_on` →
`choice_decline_the_drum`); re-verified unique across all 184 choice ids.

## Findings — where content would be dead, reported not authored

Quarantined tests are unlanded content specs. 80 quarantine entries exist; 27 reference
data-authority JSON. Most were **not** filled, because the catalog has no loader:

- **F10 — `narrative_questlines.json`: 4 of 12 specified questlines, and no loader.**
  `NarrativeQuestlineCatalogTests` (quarantined) pins 12 and names the 8 missing survivors
  (`marcus_olejnik`, `the_teacher`, `the_chef`, `suki_tanaka`, `the_priest`, `the_reporter`,
  `the_electrician`, `the_hunter` — all 8 exist in `survivors.json`). But **nothing in Core or src
  reads the `questlines` array**; `ContentUtilizationScanner` claims `NarrativeEncounterSystem` and
  a `NarrativePanel` that does not exist (only `FactionsNarrativePanel`). Authoring the 8 arcs
  would satisfy a dead test and reach no player. Needs a loader + route first.
- **F11 — `wall_carving_templates.json`: 3 items, no consumer at all.**
- **F12 — `codex_entries.json` (63 entries) and `cassette_sets.json` (4 sets): no consumer.**
  A 63-entry player codex that nothing loads is the largest single block of unreachable prose in
  the data authority.
- **F13 — `standing_record_factions.json`: 1 of 8 factions.** Live consumers
  (`FactionIconCatalog`, `AssetRegistry`), and `StandingRecordFactionExpansionTests` pins all 8
  with *exact* prose strings including signature quotes. Fillable, but it is transcription of a
  written spec rather than authoring, and the test also requires unique non-empty
  `signature_quote` per entry. Left as the recommended next target.
- **F14 — `characters.json` is count-pinned at exactly 84** (`NpcArcDataTests:49`, live), alongside
  pins for 24 arcs / 8 flagship / 30+ arc encounters / 4 arc signals. The cast cannot be extended
  without moving those pins, so no characters were added.
- **F15 — 40 faction ids are referenced across the data authority with no lore entry**; 22 were
  authored here. The remaining 18 are sentinels, archetypes, station owners, `faction_blank_rows`,
  or single-reference ids (e.g. `faction_wasteland_outlaws`, `faction_salvagers`,
  `faction_permafrost_nomads`, `faction_black_cross`, `faction_the_underwrite`) that need canon
  research before they get a dossier.

## Verification gates

| Gate | Result |
|---|---|
| JSON validity, all edited files | PASS (re-parsed after every write) |
| `dotnet test Ashfall.Core.Tests` (full) | PASS — **9407/9407**, 0 failed |
| `Plan57IncidentTests` (newly unquarantined) | PASS — **12/12** |
| `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| `godot --headless -- --data-integrity-selftest` | PASS — 0 errors / 0 warnings, 298 catalogs |
| HEAD-vs-working-tree audit | PASS — incidents: +20, 0 lost, **0 originals altered**; faction_lore: +22, 0 lost, **0 originals altered**; door_encounters: 80 → 80, exactly 1 entry altered (the canon fix), prose fields only |
| Tone scan (real countries / fantasy / glorified violence) over 190 new prose blocks | PASS — 0 hits |
| Vocabulary closure | PASS — relationship values, tribute/tech free-form consistent with existing entries, both lore keysets pre-existing |
| Id uniqueness | PASS — 25 incident ids, 45 faction ids, 184 door choice ids |
