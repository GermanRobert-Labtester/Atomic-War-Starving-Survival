# ACCEPTANCE — Waystation & Faction Culture Expansion (2026-09-09)

Slice: new authored entries in two live, thin catalogs, extending the round-5 faction dossiers
(`657056b7`) from ideology into lived practice and named places.

## Added content

| File | Consumer (verified) | Entries | Total | Prose |
|---|---|---|---|---|
| `waystations.json` | `WaystationCatalogLoader` → `WaystationNetworkSystem`, rendered by `src/UI/WaystationNetworkPanel.cs` | +8 | 6 → 14 | 8 keepers, specialties, `local_problem` ~1,050 words total with names |
| `muster_faction_culture.json` | `FactionCultureCatalog.LoadEntries` | +19 | 6 → 25 | 19 titles + bodies, ~2,070 words |

### 8 waystations, one per round-5 faction, at real unused nodes

| Waystation | Node | Region | Keeper | Faction |
|---|---|---|---|---|
| South Beacon Lamp Post | `loc_south_beacon_tower` | deep_coast | Lamptender Orris Vail | Lamplighters |
| Tinker's Notch Swap Post | `loc_settlement_tinkers_notch` | dead_suburbs | Registrar Odie Vant | Scavenger Guild |
| Brine-Pan Hollow Waystation | `loc_settlement_brine_pans` | the_toll | **Assayer Mira Vos** | The Cutters |
| Slate Hollow Enclave Post | `loc_settlement_slate_hollow` | high_scarp | Counter Ilse Marr | The Cold Count |
| Iron Siding Redoubt Post | `loc_settlement_iron_siding` | industrial_belt | Sergeant-Marshal Ada Kesk | Deserter Coalition |
| The Weighbridge Freight Post | `loc_weighbridge` | industrial_belt | **Clerk Edor Vale** | The Office |
| Pilgrim Switchbacks Waystation | `loc_pilgrim_switchbacks` | high_scarp | **Walker Kaspar Drej** | The Long Walk |
| St Brigid's Almshouse Post | `loc_st_brigids_almshouse` | the_cluster | Runner Tev Alderney | The Quiet House |

Bolded keepers are canon figures already named in the data authority, reused rather than
reinvented. Others follow the file's title+surname convention (Warden Kessel, Deacon Vane, Foreman
Taggart, Mistress Corvo, Diver Renn, Weigher Orlov).

Each `local_problem` is an unresolved operational trouble drawn from that faction's canon: the
Guild's two-crew claim dispute at a drop shaft, the Cutters' low assay, the Cold Count's three
missing sheets of shielding lead, a Garrison patrol walking the Coalition's perimeter at dusk, two
consignments on one receipt number at the weighbridge, the Walk's washed-out upper landing with
Drej's never-collected odds, and an almshouse with four beds against eleven names on the tag board.

### 19 culture entries — practice, not restated ideology

One per round-5 dossier faction that lacked coverage (the three already covered —
`scavenger_guild`, `iron_raiders`, `deserter_coalition` — were skipped). Each dramatizes the
faction's `currents.json` access rule as daily custom: the Archivists' two-witness reading, the
Lamplighters' eleven dark nights, the Quiet House tag written exactly as given, the Grain Exchange
board nobody guards, the Osteophages' chute-and-bell, the Tally's second reading, the Undertow's
rope-first arithmetic, the Cold Count's four-name roster, the Provisioned inventory tour, the Long
Walk's posted-and-never-collected odds, the Tempest's human meter ritual, the Flotilla's count
before a third party, the Foundry's single queue, the Office's shared drawer, the Cutters' assay
before the sale, the Compact's second signature, the Fleet's one book holding rescue and purge, and
the Scale's noon weighing. Faction coverage in this catalog goes 4 → 23.

## Constraint compliance

- **References resolve**: all 8 `node_id`s exist in `locations.json` (169 ids) and none was already
  used by the 6 existing waystations; all 24 `stock_item_ids` exist in `items.json` (660); all 19
  `faction_id`s resolve in `faction_lore.json` (45 as of round 5).
- **Rejected candidates**: `lamp_oil`, `brass_fittings`, `sewing_kit`, `welders_glass` appear in
  `currents.json` wants but are **not** items, so they were kept out of `stock_item_ids`.
- **Vocabulary closed**: `services` restricted to the file's existing 9 tokens; relationship and
  alignment values unchanged.
- **Numeric envelope** (existing 6 entries): condition 70–90, filter_health 75–95, defense_rating
  3–5. Values are grounded in each node's `locations.json` `dangerLevel` / `baseRadsPerHour` —
  St Brigid's (danger 7, 40 rads/hr) at 72/76 and the Switchbacks (danger 5, 52 rads/hr, 5 h
  travel) at 70/75, versus South Beacon (danger 2, 9 rads/hr) at 88/92.
- **Prose length held inside existing maxima, not widened**: `local_problem` 200–209 (existing max
  210); culture `body` 527–583 (existing max 583). Eleven entries were trimmed after measurement
  rather than adjusting the budget.
- **Uniqueness**: ids, waystation display names, keeper names, culture titles and bodies all unique.
- Format preserved per file: indent 2, trailing newline, literal UTF-8.

### Region handling (deliberate)

`region` is free-form: `WaystationNetworkSystem` falls back to `"settlement"` when empty and passes
it to `RegionalSupplyRouter.TagsForOrigin`, which always includes `"general"` and degrades safely
for unrecognised regions. `WaystationNetworkPanel` renders it to the player as
`"{name} ({region})"`. New entries use **settlement-accurate** regions from `settlements.json`
(`the_toll`, `the_cluster`), following the file's own precedent — existing entries already use
`high_scarp` and `dead_suburbs`, which the router does not special-case either.

## Findings

- **F16 — `narrative_questlines.json` is data-complete but still unreachable.** A concurrent stream
  landed Plan 104's 12 questlines and unquarantined `NarrativeQuestlineCatalogTests` (10/10 PASS),
  and `docs/quests/PLAN_104_NARRATIVE_QUESTLINES_CLOSEOUT.md` states **COMPLETE & FULLY VERIFIED**
  and names `Assets/Ashfall.Core/Narrative/NarrativeQuestlineCatalog.cs` /
  `NarrativeQuestlineData.cs` as its Core classes. **Neither file exists at HEAD**, and the only
  Core reference to the JSON is `CollectibleCatalogIntegrityValidator`. So 12 authored survivor arcs
  pass their catalog test while reaching no player, and the closeout overclaims. My round-5 F10 is
  therefore only half resolved: data landed, wiring did not.
- **F17 — round-4's graffiti postings are STILL unreachable; the live catalog is a different file
  with a different schema.** `BunkerGraffitiCatalog.LoadFromDirectory` reads
  `Data/narrative/bunker_graffiti_postings.json` (36 postings; schema
  `posting_id / recorded_day / location / medium / author_signature / category / content /
  morale_effect / tags`) plus `Data/narrative/graffiti_expansion.json` (40). The **root**
  `Data/bunker_graffiti_postings.json` — 30 postings including the 12 added in `04806918` — uses a
  different schema (`id / title / text / triggerWorldFlag / minDay / weight`) and is read by nothing
  except `CatalogIntegrityValidator`. Two same-named files in different directories with different
  schemas is itself a dual-authority hazard. **So round-4 finding F1 still holds**: those 12 are
  authored and validated but reach no player. Both live files are count-pinned by
  `BunkerGraffitiCatalogTests` (`Assert.Equal(36, catalog.AllPostings.Count)`, plus first/last entry
  assertions, 15 year-one postings, and `Assert.Equal(76, catalog.Count)` for the merged set), so
  making wall-voice content reachable requires re-authoring in the canonical schema *and* moving
  both pins. Deliberately not done here — that is another stream's catalog (Plans 17/29/145) and
  warrants its own change.
- **F18 — waystation node divergence between JSON and C#.** `WaystationCatalogLoader` carries
  hardcoded fallback waystations whose `node_id`s differ from the JSON's for the same stations
  (`loc_shrine_switchback_waystation` vs `loc_cut_radiation_zone_alpha`, `loc_motel_verity` vs
  `loc_cut_merchant_caravanserai`, `loc_grain_silo` vs `loc_holdfast`). Both sets resolve in
  `locations.json`, so nothing dangles, but the fallback and the authority describe different
  networks. Pre-existing; not modified here.
- **F19 — region vocabulary split.** `settlements.json` uses `coastal_shelf` for Cape Beacon while
  `waystations.json` uses `deep_coast` for the same coast; `standing_record_factions.json` uses
  `the_cut` where `currents.json` uses `the_toll`. Three parallel region vocabularies are in the
  wild. New entries follow the file they live in.
- Still unloader'd (reported, not filled): `codex_entries.json` (63), `cassette_sets.json` (4),
  `wall_carving_templates.json` (3), `echoes.json` (23, explicitly exempted).

## Verification gates

| Gate | Result |
|---|---|
| JSON validity, both files | PASS (re-parsed after write) |
| `dotnet test Ashfall.Core.Tests` (full) | PASS — **10556/10556**, 0 failed |
| `dotnet build Ashfall.csproj` | PASS — 0 errors |
| `godot --headless -- --data-integrity-selftest` | PASS — 0 errors / 0 warnings, 299 catalogs |
| HEAD-vs-working-tree audit | PASS — waystations +8, culture +19, **0 lost, 0 pre-existing entries altered** |
| Reference resolution (node_id / stock_item_ids / faction_id) | PASS — 0 unresolved |
| Numeric envelopes vs pre-existing ranges | PASS — 3/3 fields in range |
| Prose length vs pre-existing maxima | PASS — 2/2 fields in range |
| Tone scan (real countries / fantasy / glorified violence) | PASS — 0 hits over 65 new prose blocks |
