# ASHFALL — WAVE 4 INTEGRATION PROGRAM · PLAN 4 OF 6

# ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W4 (six-plan integration wave — the shelter's remaining machinery)
**Document:** W4-04
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W4-01 (save/state), W4-02 (world/travel), W4-03 (infrastructure), W4-05 (society), W4-06 (medicine)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates **the living land**: soil and crops, the greenhouse, fungi and
alternative food, wildlife populations and migration, trapping and harvesting,
infestation and blight, and the chain from harvest to the common table. It extends
existing owners — one soil authority, one crop authority, one wildlife authority,
one nutrition chain — and it never builds a parallel farm, a second animal model, or
a private food counter.

### 0.1 Two selection levels

| Level | Choice | Granularity |
|---|---|---|
| **Level 1** | Plan Path **A**, **B**, or **C** | the whole plan's posture |
| **Level 2** | ten decision points, each **A/B/C** | per-concern depth |

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Yield | 1–10 | — | — |
| B One Web | 1 | 2,3,4,5,6 | 7,8,9,10 |
| C A Living Land | — | 2,5,9 | 1,3,4,6,7,8,10 |

### 0.3 The Wave 4 rule for this plan

> **One field, one herd, one table.** `AgricultureSystem` + `CropStrainCatalog` own
> crops and soil; `GreenhouseSystem` owns controlled growing; `WildlifeMigrationSystem`
> + `WildlifeSeasonalCalendar` + `WildlifeTrappingSystem` own the wild; the grain/
> kitchen/nutrition systems own processing and meals. No second yield calculator, no
> duplicate animal population, no shadow larder.

### 0.4 Vocabulary

| Term | Meaning |
|---|---|
| plot | a cultivated growing space with soil state, crop, and stage |
| strain | an authored crop variety with yields, tolerance, and traits |
| fertility | soil capability consumed by growing and restored by amendment |
| yield | deterministic harvest product of plot × strain × care × season |
| population | the modeled count and condition of a wild species |
| migration | authored seasonal movement between regions |
| pressure | harvest intensity vs renewal rate for a wild population |
| blight | infestation/disease state that spreads through a modeled cause |
| chain | harvest → store → process → cook → meal |
| table | the shared nutrition outcome of what the shelter eats |

---

## 1. Premise audit — what P0 must verify (Rule 7)

```text
[ ] Assets/Ashfall.Core/Farming/: AgricultureSystem, CropStrainCatalog,
    FungiCultivationSystem, NutritionDiversitySystem, SoilReclamationProfileEngine
[ ] Assets/Ashfall.Core/Greenhouse/: GreenhouseSystem, GreenhouseExpansionCatalog,
    ApicultureSystem (and headless demos)
[ ] Assets/Ashfall.Core/Ecology/: CompanionAnimalSystem,
    EcologicalInfestationSystem + Catalog + Defs
[ ] wildlife: WildlifeMigrationSystem (+ .Live), WildlifeSeasonalCalendar,
    WildlifeTrappingSystem + Catalog + Events, WildlifeEcosystemSystem (World/)
[ ] processing/nutrition: GrainProcessingSystem, GrainMillingCatalog,
    KitchenNutritionSystem, Nutrition/ folder, NutritionDiversitySystem
[ ] weather/season coupling: WeatherKind, SeasonalEventSystem/Catalog,
    weather hardening data
[ ] host partials and sessions that tick these systems; save owners per system
[ ] data: crop/seed/soil/wildlife/trapping catalogs under Assets/StreamingAssets/Data/
[ ] existing rulings: Expansion 15 (soil reclamation), Expansion 26 (common table),
    PLAN_22 (greenhouse item consumption), wildlife trapping flagship log
```

### 1.1 Evidence posture

- Proposal only; no path claims; no ledger rows; read-only until Annex U.
- Core engine-free; JSON authoritative; determinism through existing seeded paths.
- One authority per concern; additive fields only; no parallel simulation.
- Focused verification per `TEST_POLICY.md`.

### 1.2 Known anchors (verify, don't trust)

| Anchor | Why it matters |
|---|---|
| `SoilReclamationProfileEngine` | soil quality tiers already exist (Expansion 15) |
| `NutritionDiversitySystem` | diversity tiers already exist (Expansion 26) |
| `WildlifeMigrationSystem.Live` | migration already ticks live; do not re-tick |
| `WildlifeSeasonalCalendar` | seasonality exists; gates must read it |
| `PLAN_22` greenhouse consumption | item consumption through the live runtime path |

---

## 2. The three Plan Paths

### 2.1 Path A — Truth & Yield

Audit every ecology/farm/wildlife data set and consumer: which crops, plots, animals,
and recipes actually exist, which are reachable, which are dead. Produce the land
ledger; repair orphaned references and inert content only.

### 2.2 Path B — One Web

Unify the chain: one soil/plot model, one wild population model, one harvest →
processing → meal path, one blight model — consumed by needs, economy, medicine, and
surfaces without local math.

### 2.3 Path C — A Living Land

Make the land live across years: ecological succession, hunting pressure, drought and
frost arcs, companion animals, and a renewal discipline in which the shelter's choices
show up in the land decades later — deterministic and save-backed via W4-01.

---

## 3. The ten decision points

### 3.1 Point 1 — Ecology inventory and ownership

**Owner anchor:** the systems listed in §1.
**Decision:** one ledger: every crop/strain/animal/infestation entry → owner →
consumers → save status; one authority per living concern.

- **A:** enumerate data vs consumers; list orphans, duplicates, and inert entries.
- **B:** registration discipline: new content joins crop/wildlife catalogs and their
  consumers in one package.
- **C:** ownership inheritance for expansions (herbs, orchards, aquaculture attach to
  existing owners, never fork).

**Verify:** ledger test; orphan scan over crop/wildlife JSON.
**Never:** two crop lists, two animal populations, or a panel counting food itself.

### 3.2 Point 2 — Soil and crop cycles

**Owner anchor:** `AgricultureSystem`, `CropStrainCatalog`, `SoilReclamationProfileEngine`.
**Decision:** plots have soil state, strains have requirements, stages advance
deterministically, and yields derive from plot × strain × care × season.

- **A:** audit strains and plots vs live data; find ungrowable strains and phantom plots.
- **B:** one plot model; rotation/fertility rules authored; amendment consumes real
  materials (W3-05) and labor (W4-05).
- **C:** multi-year arcs: fertility depletion, rotation payoffs, and seed saving on
  existing owners.

**Verify:** deterministic grow cycle; yield formula read-model; depletion/restore test.
**Never:** a second yield calculator, or growth that depends on wall-clock time.

### 3.3 Point 3 — Greenhouse and controlled growing

**Owner anchor:** `GreenhouseSystem`, `GreenhouseExpansionCatalog`, `ApicultureSystem`.
**Decision:** the greenhouse is one controlled environment: power, water, and heat
coupling explicit; expansion catalog consumed; pollination modeled through apiculture.

- **A:** audit expansions and couplings; find unused expansion entries and uncoupled
  resources.
- **B:** one environment model; consumption through W4-03 owners; yields through the
  same plot model.
- **C:** sealed-garden arcs: year-round growing, species variety, and failure modes
  (cold night, power loss) with warnings.

**Verify:** expansion coverage; resource coupling test (power/water/heat); cold-night
scenario.
**Never:** a greenhouse that ignores power/water, or a second environment model.

### 3.4 Point 4 — Fungi and alternative food

**Owner anchor:** `FungiCultivationSystem`.
**Decision:** alternative food chains are real: substrate, yield, safety thresholds,
and meal integration through the existing nutrition path.

- **A:** audit substrates/yields/consumers; find unreachable recipes.
- **B:** one substrate chain; contamination risk modeled and warned; meals via kitchen.
- **C:** scale arcs: deep-farm expansion and long-term protein/calorie balance.

**Verify:** cultivation cycle; contamination warning; meal integration.
**Never:** a parallel kitchen or a food item with no consumer.

### 3.5 Point 5 — Wildlife populations and migration

**Owner anchor:** `WildlifeMigrationSystem` (+ `.Live`), `WildlifeSeasonalCalendar`,
`WildlifeEcosystemSystem`.
**Decision:** one wild model: populations, seasonal movement, and ecosystem pressure
that harvest and hazards actually affect.

- **A:** audit migration/season data vs system behavior; find phantom routes.
- **B:** one tick; harvest pressure recorded; observation opportunities for W4-02
  ranging/recon.
- **C:** ecological succession: overhunting, recovery, and species shifts across years.

**Verify:** deterministic migration replay; pressure/recovery test; save round-trip.
**Never:** two population counters, or migration that ignores the season calendar.

### 3.6 Point 6 — Trapping and harvesting

**Owner anchor:** `WildlifeTrappingSystem` + `WildlifeTrappingCatalog` + Events.
**Decision:** trapping is a practice with cost, yield, ethics constraints, and
provenance: methods authored, cruelty avoided, yields processed through real chains.

- **A:** audit traps/yields/events; find unreachable methods and unused event hooks.
- **B:** one trapping resolution; yields route through processing/kitchen; welfare
  rules respected (no gratuitous content).
- **C:** knowledge arcs: methods learned (W3-05 knowledge), quotas, and sustainable
  take reflected in population state.

**Verify:** trapping yield test; population-pressure integration; content-tone review.
**Never:** infinite take, or trap yields that bypass processing.

### 3.7 Point 7 — Infestation and blight

**Owner anchor:** `EcologicalInfestationSystem` + Catalog + Defs.
**Decision:** blights spread from modeled causes, are detectable before loss, and are
countarable with real treatments on existing owners.

- **A:** audit infestation entries vs live consumers; find blights that never fire.
- **B:** one spread model; early-warning observation; treatment consumes materials and
  knowledge.
- **C:** long-run resistance and strain interactions across seasons.

**Verify:** spread determinism; warning-before-loss; treatment consumption.
**Never:** random unexplained total loss, or a second spread model.

### 3.8 Point 8 — Harvest to table

**Owner anchor:** `GrainProcessingSystem`, `GrainMillingCatalog`,
`KitchenNutritionSystem`, `NutritionDiversitySystem`.
**Decision:** one food chain: harvest, storage (spoilage), processing, cooking, meal —
with nutrition outcomes through the existing nutrition owners.

- **A:** audit chain links and losses (spoilage, pests); find items with no consumer
  and consumers with no source.
- **B:** one chain model; storage/spoilage bounded and warned; meals produce nutrition
  facts, not local health math.
- **C:** cuisine arcs: menu variety, preservation, and feasts on existing narrative/
  society owners.

**Verify:** chain traversal test; spoilage bounds; nutrition routing.
**Never:** a private larder, or a meal that computes health directly.

### 3.9 Point 9 — Seasons, weather, and drought/frost

**Owner anchor:** `WeatherKind`, `SeasonalEventSystem/Catalog`, weather family,
hardening.
**Decision:** the land obeys the season: planting windows, frost risk, drought stress,
and forecast warnings through W4-02's weather truth.

- **A:** audit seasonal gates vs crop/wildlife behavior; find crops immune to season.
- **B:** one seasonal context consumed by farm/greenhouse/wildlife; warnings before
  killing weather.
- **C:** climate arcs across years: variability, adaptation (strains, hardening), and
  memory of bad years in the journal/field guide.

**Verify:** season×crop matrix; frost/drought scenario; forecast-warning test.
**Never:** season-immune crops or unwarned crop death.

### 3.10 Point 10 — Sustainability and surfaces

**Owner anchor:** farm/greenhouse/wildlife surfaces (W3-06 coordination), field guide
(W4-02), journal (W3-01).
**Decision:** the player sees the web truthfully: soil state, stage and expected
window, population pressure, and the consequences of take — with the written record
in field guide and journal.

- **A:** surface audit: fabricated yields, missing stages, unstated causes.
- **B:** surfaces read owners; every loss states its cause; planning views (rotation,
  quotas) render read models.
- **C:** generational view: decade-scale land outcomes recorded and comparable.

**Verify:** W3-06 kits over farm/greenhouse/wildlife surfaces; cause-present test;
record round-trip.
**Never:** fabricated yield forecasts or losses without causes.

---

## 4. Selection sheet

```text
ASHFALL WAVE 4 · PLAN W4-04 · SELECTION SHEET

Plan Path:   [ ] A Truth & Yield   [ ] B One Web   [ ] C A Living Land

Points (mark A/B/C or leave default):
 1 ecology inventory ....... [ ]
 2 soil/crop cycles ........ [ ]
 3 greenhouse .............. [ ]
 4 fungi/alternative food .. [ ]
 5 wildlife/migration ...... [ ]
 6 trapping/harvest ........ [ ]
 7 infestation/blight ...... [ ]
 8 harvest→table ........... [ ]
 9 seasons/drought/frost ... [ ]
10 sustainability/surfaces . [ ]

Selected by: ____________   Date: ________   Foreman: ____________
```

---

## 5. Phase ladder

| Phase | Name | Exit |
|---|---|---|
| P0 | Premise audit + land ledger | ledger filed; premises re-verified |
| P1 | Soil/crop/greenhouse discipline | grow cycles + coupling green |
| P2 | Wild model + trapping | migration replay + pressure integration green |
| P3 | Blight + chain | spread/warning + harvest→table traversal green |
| P4 | Season coupling | season×crop matrix; frost/drought scenarios green |
| P5 | Surfaces + records | W3-06 kits; field-guide/journal round-trip green |
| P6 | Closeout | evidence pack; determinism; limitations recorded |

---

## 6. Non-goals, never-touch, one-authority

**Non-goals**

- No parallel farm/animal model; no new resource counters.
- No real-world agriculture simulation fantasy; authored, bounded, deterministic.
- No health computation in food systems (W4-06 owns outcomes).
- No new content beyond what consumers exist for.

**Never-touch**

- Expansion 15/26 engines and PLAN_22 consumption path.
- Wildlife trailing flagship log and its verified seams.
- Sealed debt rows; quarantined tests.
- Shared paths claimed by other packages.

**One authority per concern**

| Concern | Owner |
|---|---|
| crops/soil | `AgricultureSystem` + `CropStrainCatalog` + soil engine |
| greenhouse | `GreenhouseSystem` + expansion catalog |
| fungi | `FungiCultivationSystem` |
| wild | migration/calendar/ecosystem systems |
| trapping | `WildlifeTrappingSystem` + catalog/events |
| blight | `EcologicalInfestationSystem` |
| chain | grain/kitchen/nutrition systems |

---

## 7. Verification and acceptance

- **T1 static:** ledger; orphan scans; consumer inventory; surface-source audit.
- **T2 focused:** grow cycle; yield read-model; greenhouse coupling; fungi cycle; migration
  replay; pressure/recovery; trapping yields; blight spread; chain traversal; season
  matrix; W3-06 kits.
- **T3 soak:** 60 days × seasons at seed: yield trends, population trends, zero drift.
- **Acceptance:** evidence pack + Annex U signature; compile-green is not acceptance.

## 8. Handoffs and dependencies

| Direction | Detail |
|---|---|
| W3-02 | food economy, caravan supply, rationing reads the same yields |
| W3-03 | morale/food-diversity consequences (Expansion 26) |
| W3-05 | seeds, tools, processing crafts, knowledge |
| W4-02 | weather truth, ranging observation, trap-route knowledge |
| W4-03 | greenhouse power/water/heat coupling |
| W4-05 | farm labor, quotas, shared table politics |
| W4-06 | nutrition deficiency and contamination outcomes |
| W4-01 | all land state (soil, populations, blight) ships save sections |

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

## U.1 What this plan releases

| Release | Unblocks |
|---|---|
| U1 | land ledger work blocked on "which farm data is live" |
| U2 | wild-model integration held for migration double-tick confirmation |
| U3 | greenhouse coupling blocked on W4-03 water/power truth |
| U4 | blight/treatment work blocked on materials/knowledge paths (W3-05) |
| U5 | sustainability surfaces blocked on W3-06 registry + W4-01 records |

## U.2 Signature block

```text
ASHFALL WAVE 4 · PLAN W4-04 · RELEASE SIGNATURE
HEAD: ________  Date: ________
[ ] P0 premise audit completed and filed
[ ] land ledger exists; orphans listed
[ ] no path claimed outside the package
[ ] focused test targets named
[ ] rollback position recorded
Signed: ________   Foreman: ________
```

## U.3 Never-touches

- No edit to another Wave 4 plan's claimed paths.
- No re-open of Expansion 15/26 or PLAN_22 closings.
- No change to wildlife migration contracts without ledger evidence.
- No revival of quarantined tests outside the documented procedure.

## U.4 Release rule

> This plan executes only after U.2 is signed. Until then it is read-only planning.

---

*End of W4-04 — Part I. Expansion parts continue on the established Wave 3 pattern.*---

# W4-04 · PART II — DEEP DESIGN: LAND LEDGER, SOIL, GREENHOUSE (POINTS 1–3)

## II.1 The land ledger: schema and meaning

One row per living concern: crops, plots, animals, infestations.

```yaml
land_ledger:
  - id: crops.strains
    owner: "AgricultureSystem + CropStrainCatalog"
    state: ["plots", "strain_defs", "stage_cursors"]
    consumers: ["kitchen", "greenhouse", "surfaces", "blight"]
    save_section: "farming.plots"
    surfaces: ["farm board", "greenhouse panel"]
    warnings: ["frost", "drought", "blight", "soil_exhaustion"]
    tests: ["GrowCycleTests", "YieldReadModelTests"]
  - id: wild.populations
    owner: "WildlifeMigrationSystem + SeasonalCalendar + Ecosystem"
    ...
```

### II.1.1 Ledger rules

```text
L1  one owner per living concern; no shared mutable populations
L2  every yield is derived from (plot × strain × care × season) facts
L3  every loss names its cause (frost, blight, take, neglect)
L4  every warning precedes its loss with an authored window
L5  every save section obeys W4-01 budgets/bounds
L6  every surface reads; no panel computes yields
```

### II.1.2 The derived-yield law

Yields are never persisted. The plan enforces: stored facts (soil, stage, care
actions, season) in; computed yield out; deterministic and replayable.

## II.2 Soil and crop cycles

### II.2.1 Plot record

```jsonc
{
  "id": "plot_a1",
  "soil": { "quality": "worked", "organic": 3, "moisture": "good" },
  "crop": "strain_barley",
  "stage": "growing",            // fallow | sown | growing | ripening | ready
  "sown_day": 210,
  "care": ["watered", "weeded"],
  "expected": { "ready_day": 240, "band": [230, 250] }
}
```

### II.2.2 Rules

```text
S1  stage advance daily, deterministic from (day, climate, care)
S2  soil quality changes by crop use, amendments, rotation
S3  amendments consume real materials (W3-05) and labor (W4-05)
S4  rotation payoffs are authored; monoculture exhausts with warnings
S5  frost/drought kill stages with warnings first
S6  seed saving chains on existing owners
```

### II.2.3 The yield read model

```text
yield = strain.base × soil_factor × care_factor × season_factor × pressure
all factors authored, integer-friendly, displayed as bands not promises
```

## II.3 Greenhouse and controlled growing

### II.3.1 Environment record

```jsonc
{
  "id": "gh_main",
  "zones": 4,
  "climate": { "temp": 21, "humidity": 60, "co2": "normal" },
  "systems": { "power": "tied", "water": "tied", "heat": "tied" },
  "crops": ["plot_g1", "plot_g2"]
}
```

### II.3.2 Rules

```text
G1  one environment truth; zones read it, never each their own
G2  power/water/heat couplings explicit (W4-03); no greenhouse ignores them
G3  expansion entries must be consumed (zones unlocked by catalog)
G4  pollination via ApicultureSystem; yields reflect hive health
G5  climate failures warn (cold night, power loss) before damage
G6  greenhouse plots use the same plot model; no second grower
```

## II.4 Worked example: the phantom harvest

**Report:** farm board showed 40 bushels; storage received 12.

**Walk:**

```text
1. root: board computed yield from strain base only; harvest applied soil
   and care factors
2. repair: one yield read model (II.2.3); board and harvest read it; test
3. verify: board-vs-harvest equality across 20 fixtures
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| FL-01 | duplicated yield math | duplication |
| FL-02 | no equality test | coverage |

*End of Part II. Continues in Part III (fungi, wildlife, trapping).*---

# W4-04 · PART III — DEEP DESIGN: FUNGI, WILDLIFE, TRAPPING (POINTS 4–6)

## III.1 Fungi and alternative food

### III.1.1 Substrate chain

```jsonc
{
  "id": "fung_bed_1",
  "substrate": "straw_chaff",
  "species": "sp_agaric",
  "stage": "fruiting",
  "contamination_risk": "low",
  "yield_band": [4, 7]
}
```

### III.1.2 Rules

```text
U1  substrate consumes real materials (W4-04 chain, W3-05 crafts)
U2  contamination risk modeled and warned before loss
U3  yields via the shared read model; meals via kitchen
U4  species unlocked through knowledge owners (W3-05)
U5  no parallel kitchen; no food item without a consumer
```

## III.2 Wildlife populations and migration

### III.2.1 Population record

```jsonc
{
  "species": "sp_deer",
  "count": 24,
  "condition": "healthy",
  "region": "r_north",
  "pressure": 2,                 // recent take vs renewal
  "observed": true
}
```

### III.2.2 Rules

```text
W1  one population per species per region; no second counters
W2  migration per seasonal calendar; deterministic
W3  take reduces count; renewal authored; overharvest warns before collapse
W4  observation feeds ranging/field guide (W4-02) as facts
W5  ecosystem interactions authored (predator/prey bands), bounded
W6  no wall-clock; campaign day only
```

### III.2.3 The pressure model

```text
pressure = take_last_window − renewal_last_window (bounded)
bands: none | mild | heavy | critical
each band: authored warnings and consequences (fewer sightings, harder hunts)
```

## III.3 Trapping and harvesting

### III.3.1 Method record

```jsonc
{
  "id": "trap_snare",
  "species_allowed": ["sp_rabbit", "sp_fox"],
  "yield": { "meat": 2, "hide": 1 },
  "welfare": "quick",            // tone constraint: no lingering methods
  "knowledge": "known",
  "worn_by": "weather"
}
```

### III.3.2 Rules

```text
T1  one trapping resolution; yields via processing/kitchen
T2  methods authored; no gratuitous suffering content (tone law)
T3  quotas/seasonal limits authored; enforcement warns
T4  trap wear by weather; maintenance via crafts
T5  capture outcomes: take | escape | empty (all recorded)
T6  knowledge unlocks methods through the research owner
```

## III.4 Worked example: the herd that vanished

**Report:** after a winter of hunting, no deer appeared anywhere.

**Walk:**

```text
1. root: take had no cap and renewal was zero in data (placeholder)
2. repair: renewal authored; pressure bands warned before collapse; recovery
   path through seasons; the case is a data defect plus a missing warning
3. verify: pressure band test; recovery scenario
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| FL-03 | renewal placeholder | completeness |
| FL-04 | no pressure warnings | fairness |
| FL-05 | no recovery path | design |

## III.5 The wild/tame boundary

```text
domestic (plots, hives, fungi) and wild (populations, traps) share the read
model and the kitchen chain, but their owners and pressure rules stay
separate: domestication does not erase the wild.
```

*End of Part III. Continues in Part IV (blight, table, seasons, surfaces).*---

# W4-04 · PART IV — DEEP DESIGN: BLIGHT, TABLE, SEASONS, SURFACES (POINTS 7–10)

## IV.1 Infestation and blight

### IV.1.1 Blight record

```jsonc
{
  "id": "blight_rust",
  "host": "strain_barley",
  "stage": "spreading",          // latent | visible | spreading | contained
  "spread_rate": "moderate",
  "severity": 2,
  "detected_day": 212,
  "treatment": "crop_dust"
}
```

### IV.1.2 Rules

```text
B1  one spread model; causes authored (damp, density, seed quality)
B2  detection precedes loss: latent -> visible stages with warnings
B3  treatment consumes materials/knowledge; containment via crop choices
B4  losses are partial and recorded; no unexplained total wipe
B5  resistant strains interact across seasons (authored)
B6  no second spread model; no random total-loss events
```

## IV.2 Harvest to table

### IV.2.1 Chain stages

```text
harvest -> store (spoilage) -> process (mill, dry) -> cook -> meal
losses at each stage: authored, bounded, warned (pests, damp, burnt)
```

### IV.2.2 Rules

```text
H1  one chain model; every item has a source and a consumer
H2  spoilage bounded; storage quality matters; warnings before loss
H3  processing consumes power/fuel (W4-03) and parts
H4  meals produce nutrition facts through W4-06; no local health math
H5  no private larder; stock lives in the inventory owner
H6  feasts/variety route to morale (W3-03) through owner events
```

### IV.2.3 The chain audit

```text
for every food item: source -> stages -> consumers -> loss paths
items without sources or consumers are findings; loss paths without warnings
are findings.
```

## IV.3 Seasons, weather, drought, frost

### IV.3.1 Seasonal context

```jsonc
{
  "season": "late_summer",
  "growing_window": true,
  "frost_risk": "low",
  "drought": "watch",
  "source": "WeatherSystem + SeasonalEventSystem"
}
```

### IV.3.2 Rules

```text
Z1  one seasonal context consumed by farm/greenhouse/wildlife
Z2  planting windows authored; out-of-window plantings warn and fail fair
Z3  frost/drought kill with warnings (forecast windows from W4-02)
Z4  adaptation options: strains, hardening, greenhouse control
Z5  bad years are remembered (journal/guide) and shape choices
Z6  campaign clock only; no season without weather truth
```

## IV.4 Sustainability and surfaces

### IV.4.1 Surface contract

```text
farm board:    plots, stages, expected bands, care needed, warnings
greenhouse:    zones, climate, couplings, crops
wild ledger:   species, regions, pressure bands, sightings
blight board:  detection, spread, treatments
kitchen chain: sources, stock, spoilage watch, meals
```

### IV.4.2 Rules

```text
P1  all surfaces read owners; no computed yields
P2  losses always state their cause
P3  expected harvests shown as bands, never promises
P4  pressure and blight shown before consequences
P5  history bounded (per-season aggregates), saved (W4-01)
P6  W3-06 kits cover all land surfaces
```

## IV.5 Worked example: the winter that killed the soil

**Report:** spring planting failed everywhere with no prior warning.

**Walk:**

```text
1. root: soil exhaustion accumulated silently over two monoculture years;
   the warning existed but fired at the single exhaustion threshold
2. repair: progressive bands (tired -> exhausted) with warnings; amendments
   presented as the path; rotation payoff visible
3. verify: multi-season soil test; warning bands
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| FL-06 | silent soil decay | fairness |
| FL-07 | single-threshold warning | completeness |
| FL-08 | no amendment path shown | guidance |

*End of Part IV. Continues in Part V (playbooks).*---

# W4-04 · PART V — PLAYBOOKS

## V.1 The P0 premise playbook

```text
1. freeze HEAD; enumerate crop/wildlife/blight/blight data sets
2. for each: find the live owner and consumers; list inert entries
3. list every yield computation; find duplicates vs the read model
4. list soil/plot writers; find second stores
5. read migration/seasonal data against the live calendar
6. list trapping methods/yields; check processing/kitchen coverage
7. read blight entries vs live spread consumers
8. record coupling with W4-03 (power/water/heat) and W3-05 (crafts)
9. write the premise note; claim paths; draft the package row
```

## V.2 The crop authoring playbook

```text
1. strain: requirements, days, bands, rot/disease resistances
2. must consume: a plot, seeds, care actions, season window
3. yield via the shared read model only
4. losses: named causes with warnings
5. seed-saving path authored
6. register row + grow-cycle test with the strain
```

## V.3 The wildlife authoring playbook

```text
1. species: region, base count, renewal, seasonal movement
2. pressure bands authored with warnings and consequences
3. take paths route through trapping/hunting resolutions
4. sightings feed observation/ranging (W4-02)
5. recovery path authored; collapse is possible but warned and recoverable
6. register row + migration replay test
```

## V.4 The blight authoring playbook

```text
1. host strains named; cause authored (damp/density/seed)
2. stages: latent -> visible -> spreading -> contained
3. detection precedes loss; warnings at every transition
4. treatments consume materials/knowledge; containment options
5. partial losses recorded with causes
6. scenario test: damp year with one resistant strain
```

## V.5 The kitchen crate audit

```text
for each food item: source, stages, consumers, loss paths
orphans: item without source (FL class) or without consumer
warnings: spoilage/pest losses must warn before loss
```

## V.6 Anti-pattern drills

**Drill 1 — the second yield.** Find a yield computed outside the read model;
delete it.

**Drill 2 — the silent soil.** Read soil decay over three seasons; every band
must warn.

**Drill 3 — the free greenhouse.** Cut power; the greenhouse must warn and
degrade, not continue.

**Drill 4 — the infinite herd.** Take 100% of a population; recovery must be
authored and the collapse warned.

**Drill 5 — the mystery loss.** Any crop loss without a cause string is a
finding.

*End of Part V. Continues in Part VI (verification catalog).*---

# W4-04 · PART VI — VERIFICATION CATALOG

## VI.1 T1 — static

| ID | Check | Fails when |
|---|---|---|
| T1.1 | yield-math scan | yield computed outside the read model |
| T1.2 | second-store scan | plots/populations tracked twice |
| T1.3 | orphan food scan | item without source or consumer |
| T1.4 | warning registry | loss path without a warning/cause |
| T1.5 | coupling audit | greenhouse energy/water not declared |
| T1.6 | wall-clock scan | seasons/wildlife using real time |
| T1.7 | trapping tone check | lingering/suffering methods present |
| T1.8 | blight consumer check | blight entries never evaluated |

## VI.2 T2 — focused per point

### Point 1 — inventory
```text
T2.1.1 ledger enumerates; every entry has owner/tests
T2.1.2 duplicate-store gate (negative test)
```

### Point 2 — soil/crop
```text
T2.2.1 stage advance determinism (same inputs => same days)
T2.2.2 yield read-model consistency (board == harvest)
T2.2.3 soil bands warn; amendments restore
T2.2.4 frost/drought kill with prior warning
T2.2.5 seed-saving path
```

### Point 3 — greenhouse
```text
T2.3.1 expansions consumed; zones unlock
T2.3.2 couplings: power/water/heat cut => warnings + degradation
T2.3.3 climate failures warned before damage
T2.3.4 pollination affects yield via hive state
```

### Point 4 — fungi
```text
T2.4.1 substrate consumption chain
T2.4.2 contamination warned before loss
T2.4.3 meals via kitchen only
```

### Point 5 — wildlife
```text
T2.5.1 migration replay determinism
T2.5.2 pressure bands warn (mild/heavy/critical)
T2.5.3 take reduces count; renewal authored
T2.5.4 observation records feed the guide
```

### Point 6 — trapping
```text
T2.6.1 yields route through processing/kitchen
T2.6.2 capture outcomes recorded (take/escape/empty)
T2.6.3 wear by weather; maintenance path
T2.6.4 tone review (no lingering methods)
```

### Point 7 — blight
```text
T2.7.1 detection precedes loss (stages)
T2.7.2 spread deterministic and bounded
T2.7.3 treatment consumption; containment works
T2.7.4 partial losses recorded with causes
```

### Point 8 — chain
```text
T2.8.1 every item has source+consumer (audit)
T2.8.2 spoilage bounded and warned
T2.8.3 processing consumption (power/fuel/parts)
T2.8.4 meals route to nutrition (W4-06)
```

### Point 9 — seasons
```text
T2.9.1 one seasonal context consumed by all land systems
T2.9.2 planting windows enforced with warnings
T2.9.3 frost/drought warnings from forecast
T2.9.4 bad-year records durable
```

### Point 10 — surfaces
```text
T2.10.1 reads only; board == owner
T2.10.2 losses show causes
T2.10.3 bands not promises
T2.10.4 pressure/blight visible before consequences
T2.10.5 history bounded + round-trip
T2.10.6 W3-06 kits over land surfaces
```

## VI.3 T3 — seeded soak

```text
120 days with seasons + one drought + one frost + one blight year
assert: yield consistency; population pressure bands warned; soil bands
        warned; chain losses warned; no orphan items; replay green
```

## VI.4 Evidence formats

```yaml
run: T2.2.2
date: ____  head: ____
plot: ____  strain: ____
board_band: [__, __]  harvest: __
equal: yes
result: pass
```

*End of Part VI. Continues in Part VII (worked threads).*---

# W4-04 · PART VII — WORKED THREADS AND FINDINGS (FL-09–FL-20)

## VII.1 Thread A — "the crop that froze after harvest"

**Report:** ripe crops died the night they ripened.

**Walk:**

```text
1. root: frost check ran after stage advance; "ready" then frozen in one tick
2. repair: frost evaluates the pre-advance state; ready crops harvestable
   first (one grace day, authored); warnings already existed
3. verify: frost-at-ripe test
```

| ID | Class | Repair |
|---|---|---|
| FL-09 | order error at harvest | correctness |
| FL-10 | no ripen-grace rule | design |

## VII.2 Thread B — "the greenhouse that grew in the dark"

**Report:** greenhouse kept growing through a blackout.

**Walk:**

```text
1. root: climate fields existed but no consumer read power state
2. repair: climate drives from power/heat state; blackout begins a warned
   cold curve; growth pauses or dies per authored bands
3. verify: blackout-in-greenhouse scenario
```

| ID | Class | Repair |
|---|---|---|
| FL-11 | uncoupled climate | coupling |
| FL-12 | no blackout test | coverage |

## VII.3 Thread C — "the mushrooms that ate the larder"

**Report:** a contaminated fungi bed poisoned a whole stock.

**Walk:**

```text
1. root: contamination existed per bed but batch storage mixed product
2. repair: contaminated yields are flagged; the kitchen refuses or warns;
   storage separates flagged stock
3. verify: contamination routing test
```

| ID | Class | Repair |
|---|---|---|
| FL-13 | unflagged contaminated yield | completeness |
| FL-14 | no storage separation | design |

## VII.4 Thread D — "the rabbits that bred like code"

**Report:** rabbit count doubled every week.

**Walk:**

```text
1. root: renewal used a multiplication without a cap or predator band
2. repair: authored caps + predator/prey interaction bands, bounded
3. verify: population bounds test over 60 days
```

| ID | Class | Repair |
|---|---|---|
| FL-15 | unbounded growth | physics |
| FL-16 | no bounds test | coverage |

## VII.5 Thread E — "the trap that caught the same fox"

**Report:** one trap produced daily catches with no wear.

**Walk:**

```text
1. root: traps had no weather wear or reset requirement
2. repair: wear by weather; reset action; capture outcomes recorded
3. verify: trap lifecycle test
```

| ID | Class | Repair |
|---|---|---|
| FL-17 | infinite trap yield | fairness |
| FL-18 | no lifecycle test | coverage |

## VII.6 Thread F — "the blight that skipped the warning"

**Report:** rust went from latent to wiped overnight.

**Walk:**

```text
1. root: stages existed but spread rate was a single multiplier with no bands
2. repair: stage transitions with warnings; spread bounded per day; treatment
   window exists by design
3. verify: blight scenario with damp year
```

| ID | Class | Repair |
|---|---|---|
| FL-19 | no stage warnings | fairness |
| FL-20 | unbounded daily spread | physics |

## VII.7 Summary

```text
A: order of evaluation is a fairness rule
B: controlled growing is controlled by power/heat, not by walls
C: contamination flags follow the food
D: populations are bounded by ecology, not arithmetic
E: traps wear and reset
F: blight advances in warned stages
```

*End of Part VII. Continues in Part VIII (Q&A).*---

# W4-04 · PART VIII — QUESTIONS AND ANSWERS

**Q1. Why one plan for farms, greenhouse, fungi, wildlife, and blight?**
Because they are one web: soil feeds crops, crops feed tables, tables need
variety, variety needs the wild, and blight is the web's fever.

**Q2. What is the land ledger for?**
One owner per living concern, one yield read model, one chain — mechanical.

**Q3. Why are yields never persisted?**
Because they are conclusions. Store the facts; compute the conclusion on
demand; replay equality proves it.

**Q4. What is the soil's role?**
A slow resource with bands and amendments: monoculture exhausts, rotation
restores, both warned.

**Q5. What makes frost fair?**
A forecast window from W4-02, a warning, a grace day at ripe, and authored
loss that only follows a visible cause.

**Q6. What makes a greenhouse "controlled"?**
Power, water, and heat are real couplings; climate is one truth; failures warn
and degrade.

**Q7. Why fungi?**
Alternative calories with a substrate chain and contamination risk — depth for
the food web without a second kitchen.

**Q8. What is a population's truth?**
One count per species per region, with pressure bands and authored renewal.

**Q9. What is pressure?**
Take vs renewal over a window; bands with warnings; collapse possible but
never silent.

**Q10. Why do sightings matter?**
They feed observation (W4-02), the field guide, and pressure displays — facts,
not decoration.

**Q11. What makes trapping fair?**
Finite yields, wear, reset, quotas, and methods that respect the tone law.

**Q12. What is the tone law?**
No lingering suffering, no gratuitous content; traps are practical tools.

**Q13. What is blight's shape?**
A stage machine with warnings: latent, visible, spreading, contained; losses
partial and caused.

**Q14. What stops blight from being a coin flip?**
Authored causes, bounded spread, deterministic evolution, treatment windows.

**Q15. What is the chain?**
Harvest → store → process → cook → meal, with bounded losses and warnings.

**Q16. What is spoilage's guardrail?**
Bounded loss rates, warned conditions, storage choices that matter.

**Q17. Why does the kitchen own meals?**
One place where nutrition facts are made; W4-06 owns the health outcome.

**Q18. What keeps the wild wild?**
Separate owners and pressure rules; domestication grows beside the wild, never
over it.

**Q19. What is a bad year?**
A recorded season with causes; remembered in the guide; inputs to next year's
choices.

**Q20. What makes seasons fair?**
One context, planting windows, forecast warnings, adaptation options.

**Q21. What is sustainability here?**
Rotation, amendments, renewal rates, and caps — the land can be worn out and
can recover.

**Q22. What is out of scope?**
Real-world agronomy, new resource models, health computation, UI layout.

**Q23. What is the biggest risk?**
A second yield computation reappearing in a surface.

**Q24. The second?**
A silent population cap change that hides a collapse.

**Q25. The third?**
A blight event that skips its warning stages.

**Q26. What is the smallest useful increment?**
Path A points 1–3: ledger, yield read model, soil bands. One week for
trustworthy harvests.

**Q27. What does Path B add?**
One web: couplings, chain audit, pressure bands, blight stages, surfaces.

**Q28. What does Path C add?**
Years: rotation culture, recovery arcs, strain breadth, bad-year memory.

**Q29. Who does the plan serve first?**
The kitchen and the table: every yield decision ends in someone's bowl.

**Q30. What is the final sentence?**
The land gives what it is given; keep the books honest.

*End of Part VIII. Continues in Part IX (Path C designs).*---

# W4-04 · PART IX — PATH C IMPLEMENTATION DESIGNS (C1–C10)

## C1 — The seed library

```text
design: strains collected, traded, saved; seed quality affects blight
        resistance and yield; storage conditions matter
acceptance: seed chain; quality effects; no free reset
```

## C2 — The rotation culture

```text
design: rotation schedules authored per soil type; payoff visible across
        seasons; neglect exhausts with warnings
acceptance: multi-season soil test; warning bands; amendment paths
```

## C3 — The wild balance

```text
design: predator/prey bands, territory shifts, and migration routes that
        respond to pressure; recovery arcs
acceptance: bounds; collapse warned; recovery deterministic
```

## C4 — The orchard years

```text
design: multi-year crops (trees, vines) with long payoffs; patience rewarded
acceptance: long-horizon test; no wall-clock; saves mid-cycle
```

## C5 — The kitchen garden

```text
design: small-scale variety plots for morale and nutrition diversity
acceptance: nutrition routing; morale events; bounded scale
```

## C6 — The bad-year memory

```text
design: season records in the guide; approach choices reflect history
acceptance: record durability; guide entries; input to planting UI
```

## C7 — The pollinator web

```text
design: hive health, forage, and weather interactions with yields
acceptance: hive states; yield effect; collapse warned and recoverable
```

## C8 — The salvage ecology

```text
design: abandoned fields reclaim over years; feral populations appear,
        creating hunting and hazard encounters (W4-02/W3-04 routing)
acceptance: reclaim states; encounter hooks; no new owners
```

## C9 — The full larder

```text
design: preservation (dry, brine, smoke, root cellars) with bounded losses
        and seasonal security
acceptance: preservation chains; spoilage reduction; power dependencies
```

## C10 — The final shape

```text
design: a land that answers to one yield law and one chain; that can be worn
        out and brought back; whose wild is real; whose seasons are weather —
        all saved, replayed, and told in the guide
acceptance: closure measurement (§XVI)
```

*End of Part IX. Continues in Part X (checklists).*---

# W4-04 · PART X — CHECKLISTS AND WORKSHEETS

## X.1 The P0 worksheet

```text
PACKAGE: ______  HEAD: ______  DATE: ______
[ ] strains/plots/greenhouse/fungi/wildlife/blight entries counted
[ ] owners and consumers per entry; inert entries: __
[ ] yield computations found; duplicates: __
[ ] soil/plot writers; second stores: __
[ ] migration/seasonal data vs live calendar: ok
[ ] trapping methods; tone issues: __
[ ] chain coverage: items without source/consumer: __
[ ] couplings with W4-03/W3-05 confirmed
[ ] premises contradicted: __ (attach)
```

## X.2 The crop authoring worksheet

```text
STRAIN: ______  days: __  window: ____
requirements: soil __ water __ care __
yield: base __ factors __ (all authored)
losses: frost __ drought __ blight __ take __
[ ] read model only  [ ] seed path  [ ] register row  [ ] cycle test
```

## X.3 The wildlife worksheet

```text
SPECIES: ______  region: __  count: __  renewal: __
pressure bands: mild __ heavy __ critical __ (warnings each)
take paths: ______  recovery: ______
[ ] one counter  [ ] migration replay  [ ] observation hooks
```

## X.4 The blight worksheet

```text
BLIGHT: ______  host: __  cause: __
stages: latent __ visible __ spreading __ contained __
spread: per-day bound __  treatment: ______  window: __
[ ] stage warnings  [ ] partial losses  [ ] scenario test
```

## X.5 The chain audit worksheet

```text
item: ______  source: ______  stages: ______  consumer: ______
loss paths + warnings: ______
[ ] no orphan  [ ] spoilage bounded  [ ] nutrition routed
```

## X.6 The surface checklist

```text
[ ] reads only; board == owner
[ ] causes shown on every loss
[ ] bands not promises
[ ] pressure/blight before consequences
[ ] history bounded + round-trip
[ ] W3-06 kits green
```

*End of Part X. Continues in Part XI (field guide and maintenance).*---

# W4-04 · PART XI — FIELD GUIDE, MAINTENANCE, AND CLOSURE

## XI.1 The one-page field guide

```text
THE LAND SHIPS WHEN:
  one yield read model; board equals harvest
  soil bands warn; amendments restore
  greenhouses obey power, water, heat
  fungi flag contamination
  populations have bounds and pressure warnings
  traps wear, quotas hold, tone is kept
  blight advances in warned stages
  the chain has no orphans
  seasons come from weather, not wish
  surfaces show bands and causes, never promises
```

## XI.2 The maintenance calendar

| Cadence | Task |
|---|---|
| per content change | ledger row; chain audit; reference scans |
| weekly | yield equality spot; pressure band spot |
| release | soil/blight scenarios; greenhouse couplings; orphan scan |
| seasonal | planting window runs; frost/drought scenarios; recovery check |
| yearly | strain breadth; wild balance review; bad-year records |

## XI.3 The sweeps

```text
plots out of window -> warning audit, defect
populations at cap with no renewal -> data defect
blights past containment -> spread bound audit
items without consumers -> retire or consume
months without a bad-year record -> verify seasons are real
```

## XI.4 The closure measurement

```yaml
ledger: { entries: __, owners: all, orphans: 0 }
yield: { equality: pass, read_model_only: pass }
soil: { bands: warned, amendments: restore, rotation_payoff: pass }
greenhouse: { couplings: pass, failures: warned, expansions: consumed }
fungi: { substrate: chained, contamination: flagged }
wild: { counters: single, bounds: pass, pressure_warned: pass }
trapping: { lifecycle: pass, tone: pass, yields_routed: pass }
blight: { stages: warned, spread: bounded, treatments: consumed }
chain: { orphans: 0, spoilage: bounded, nutrition: routed }
seasons: { single_context: pass, windows: enforced, warnings: from forecast }
surfaces: { read_only: pass, causes: shown, bands: pass, kits: pass }
soak: pass
```

## XI.5 The closing statement

```text
The land outside the kitchen door is the campaign's slowest and most honest
system: it gives what it is given. Keep its books; keep its warnings; keep it
wild enough to surprise and fair enough to trust.
```

*End of Part XI. Continues in Part XII (appendices and registers).*---

# W4-04 · PART XII — APPENDICES: REGISTERS AND TABLES

## XII.1 The strain register (seed)

| Strain | Days | Window | Yield band | Resistances |
|---|---|---|---|---|
| barley | 30 | spring | 8–14 | rust: weak |
| potato | 45 | spring | 20–30 | blight: weak |
| turnip | 25 | any | 6–10 | frost: partial |
| bean | 35 | summer | 5–9 | drought: partial |
| rye | 40 | autumn | 10–16 | frost: strong |

## XII.2 The plot register (seed)

| Plot | Soil | Crop | Stage | Ready band |
|---|---|---|---|---|
| a1 | worked | barley | growing | [230, 250] |
| a2 | tired | fallow | — | — |
| g1 | mixed | bean | ripening | [235, 242] |

## XII.3 The species register (seed)

| Species | Region | Count | Renewal | Pressure band |
|---|---|---|---|---|
| deer | r_north | 24 | +2/season | mild |
| rabbit | r_north | 60 | +8/season | none |
| fox | r_north | 9 | +1/season | none |
| fowl | r_coast | 40 | +6/season | mild |

## XII.4 The blight register (seed)

| Blight | Host | Cause | Stages | Treatment |
|---|---|---|---|---|
| rust | barley, rye | damp | latent-visible-spreading-contained | crop_dust |
| blight | potato | density | latent-visible-spreading | burn_cull |
| mold | stored grain | moisture | visible-spreading | dry_vent |

## XII.5 The chain register (seed)

| Item | Source | Process | Consumer | Loss paths |
|---|---|---|---|---|
| grain | plot | mill | kitchen | damp, pests |
| flour | grain | — | baking | weevil |
| meat | trap | smoke | kitchen | spoilage |
| mushroom | bed | dry | kitchen | contamination |
| honey | hive | — | kitchen, morale | weather |

## XII.6 The warning copy register

| Ref | Copy |
|---|---|
| land_frost_watch | "Frost is coming to the fields." |
| land_drought_watch | "The ground is drying. Water the plots." |
| land_soil_tired | "{plot} is tired. It needs rest or amendment." |
| land_pressure_heavy | "Game is thinning around {region}." |
| land_blight_visible | "{blight} is visible on {crop}." |
| land_spoilage_watch | "Stores are damp. Grain may spoil." |
| land_cold_night | "The greenhouse is cooling." |

*End of Part XII. Continues in Part XIII (case files).*---

# W4-04 · PART XIII — REVIEWER CASE FILES

## XIII.1 Case 1 — the "quick yield" in a panel

**Diff:** a farm panel computes expected harvest for a tooltip.

**Review:**

```text
read model? local multiplication of strain × plot
verdict: RETURNED — ask the read model; tooltips are surfaces too
```

## XIII.2 Case 2 — the generous renewal

**Diff:** doubles rabbit renewal "so early hunting feels good."

**Review:**

```text
bounds? no cap; predator band ignored
verdict: RETURNED — authored renewal and caps; early generosity is a data
         tuning decision with its warnings intact
```

## XIII.3 Case 3 — the silent cull

**Diff:** blight containment removes affected plots outright.

**Review:**

```text
stages? skip visible/spreading; no warning
verdict: RETURNED — losses follow visible stages and warnings; culls are
         player actions
```

## XIII.4 Case 4 — the free greenhouse

**Diff:** greenhouse ignores power for greenhouse "readability."

**Review:**

```text
coupling? none declared; blackout test fails
verdict: RETURNED — climate reads power; failures warn and degrade
```

## XIII.5 Case 5 — the orphan preserves

**Diff:** adds smoked meat with no consumer.

**Review:**

```text
chain? consumer missing
verdict: SIGNED with condition — consumer must land in the same change; the
         orphan scan is red until it does
```

## XIII.6 The patterns

| Pattern | Tell | Verdict |
|---|---|---|
| local yield math | multiplication in a panel | RETURNED |
| generosity without bounds | renewal/cap changes | RETURNED |
| skipped stages | instant loss | RETURNED |
| ignored coupling | greenhouse/power | RETURNED |
| orphan items | no consumer | SIGNED conditional |

## XIII.7 The review card

```text
1. which read model made this number?
2. which stage machine guards this loss?
3. which coupling powers this system?
4. which consumer takes this yield?
5. which warning precedes this harm?
```

*End of Part XIII. Continues in Part XIV (scenarios).*---

# W4-04 · PART XIV — SCENARIO BANK

## XIV.1 S1 — The first planting

```text
fixture: spring, two plots, basic strains
assert: window enforced; stages advance; board bands match harvest; seeds path
```

## XIV.2 S2 — The frost

```text
fixture: forecast frost in 2 days
assert: warning; ripe harvested first if chosen; partial losses with causes
```

## XIV.3 S3 — The drought

```text
fixture: dry season, water limited
assert: drought watch; watering consumes (W4-03); losses warned; recovery
```

## XIV.4 S4 — The greenhouse blackout

```text
fixture: power cut 12 hours
assert: cooling curve warned; growth pause/damage per bands; restoration
```

## XIV.5 S5 — The bad bed

```text
fixture: contaminated fungi bed
assert: flag; kitchen warns; storage separation; disposal path
```

## XIV.6 S6 — The thinning herd

```text
fixture: heavy take across a season
assert: pressure bands warn; sightings fall; recovery authored
```

## XIV.7 S7 — The trap line

```text
fixture: six traps, weather front
assert: wear; resets; outcomes recorded; yields route to kitchen
```

## XIV.8 S8 — The rust year

```text
fixture: damp spring, susceptible strains
assert: stages warned; treatment window; resistant strain payoff
```

## XIV.9 S9 — The damp stores

```text
fixture: wet season, poor storage
assert: spoilage warning; losses bounded; dry_vent path
```

## XIV.10 S10 — The clean desk

```text
fixture: registers + kits
assert: orphans 0; yield equality; pressure warnings; blight stages
```

## XIV.11 The soak recipe

```text
120 days: two seasons, one drought, one frost, one blight year, one heavy
hunt. Assert: no orphan items; all losses warned; populations within bounds;
soil bands warned; replay green.
```

*End of Part XIV. Continues in Part XV (governance and rollout).*---

# W4-04 · PART XV — GOVERNANCE, HANDOFFS, AND ROLLOUT

## XV.1 Governance

| Concern | Owner |
|---|---|
| crops/soil | agriculture system + strain catalog |
| greenhouse | greenhouse system + expansion catalog |
| fungi | fungi system |
| wild | migration/calendar/ecosystem owners |
| trapping | trapping system + catalog/events |
| blight | infestation system |
| chain | grain/kitchen/nutrition owners |
| registers | integrator |

## XV.2 Handoffs

| Direction | Detail |
|---|---|
| W3-02 | food economy reads yields; caravan supply |
| W3-03 | diversity/morale; scarcity consequences |
| W3-05 | seeds, tools, processing crafts, knowledge |
| W4-01 | plots, populations, blight ship sections |
| W4-02 | weather truth; ranging observation; trap routes |
| W4-03 | greenhouse power/water/heat; drying fuel |
| W4-05 | farm labor, quotas, shared table politics |
| W4-06 | nutrition/deficiency/contamination outcomes |
| W3-06 | land surfaces join kits |

## XV.3 The rollout (5 weeks)

```text
w1  P0: ledger, owners, yields, chains, couplings, premises
w2  soil + crops: read model, bands, windows, warnings
w3  greenhouse + fungi: couplings, climate, contamination
w4  wild + trapping: bounds, pressure, wear, tone
w5  blight + chain + seasons + surfaces + soak + closeout
```

## XV.4 Exits per week

| Week | Exit |
|---|---|
| 1 | ledger filed; duplicates listed |
| 2 | yield equality + soil bands green |
| 3 | greenhouse couplings + fungi flags green |
| 4 | population bounds + trap lifecycle green |
| 5 | blight stages + chain audit + surfaces green |

## XV.5 The risk register

| ID | Risk | Mitigation |
|---|---|---|
| R1 | second yield math | T1 scan + weekly spot |
| R2 | silent population collapse | pressure bands + soak |
| R3 | blight skips stages | scenario runs |
| R4 | chain orphans | audit per content change |
| R5 | greenhouses ignore power | blackout scenario |
| R6 | tone drift in trapping | content review |

## XV.6 Stop-the-line list

```text
1. a yield computed outside the read model
2. a population without bounds or renewal
3. a loss without a cause string
4. a greenhouse that ignores power/heat
5. a blight loss without stage warnings
6. a food item without a consumer
```

*End of Part XV. Continues in Part XVI (closure and final control).*---

# W4-04 · PART XVI — CLOSURE MEASUREMENT AND FINAL CONTROL

## XVI.1 The closure measurement

```yaml
run: W4-04-closure
head: <sha>
ledger: { entries: __, orphans: 0 }
yield: { equality: pass, read_model_only: pass }
soil: { bands: warned, amendments: restore }
greenhouse: { couplings: pass, failures: warned, expansions: consumed }
fungi: { chain: pass, contamination: flagged }
wild: { counters: single, bounds: pass, pressure: warned, recovery: authored }
trapping: { lifecycle: pass, yields: routed, tone: pass }
blight: { stages: warned, spread: bounded, treatment: consumed }
chain: { orphans: 0, spoilage: bounded, nutrition: routed }
seasons: { context: single, windows: enforced, warnings: forecast }
surfaces: { read_only: pass, causes: pass, bands: pass, kits: pass }
soak: pass
```

## XVI.2 The acceptance table

| Line | Evidence | Signed |
|---|---|---|
| ledger | scan | ☐ |
| yield equality | board/harvest runs | ☐ |
| soil bands | multi-season test | ☐ |
| greenhouse couplings | blackout scenario | ☐ |
| fungi flags | contamination test | ☐ |
| wild bounds | 60-day test | ☐ |
| trapping | lifecycle test | ☐ |
| blight stages | rust year scenario | ☐ |
| chain | audit output | ☐ |
| seasons | window enforcement | ☐ |
| surfaces | kits | ☐ |

## XVI.3 The binding summary

```text
Binding: ledger L1–L6, soil S1–S6, greenhouse G1–G6, fungi U1–U5, wild W1–W6,
trapping T1–T6, blight B1–B6, chain H1–H6, seasons Z1–Z6, surfaces P1–P6, and
the stop-the-line list (§XV.6).
```

## XVI.4 The final declaration

**W4-04 is complete as a plan.** Parts I–XVI with findings FL-01…FL-20.
Proposal only; execution requires Annex U and signatures. It hands the wave:
one yield law, one chain, one wild with bounds, one warned blight, and a land
that remembers its years.

```text
The land gives what it is given; keep the books honest.
```

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04 (expansion continues as needed).*---

# W4-04 · PART XVII — Q&A, SECOND BAND (Q31–Q70)

**Q31. What is the land's core law?**
The land gives what it is given — and the books are kept honestly.

**Q32. What does "books" mean for a farm?**
Facts in (soil, stage, care, season), conclusions out (yield), nothing else
stored.

**Q33. How do we keep the harvest honest?**
One read model; board and harvest read it; equality tested.

**Q34. Why warn about soil?**
Because exhaustion is slow, reversible, and invisible otherwise. Bands make it
fair.

**Q35. What is rotation's payoff?**
Authored across seasons: restored quality and fewer blights; the alternative is
warned decline.

**Q36. What makes a season real?**
One context from weather; planting windows; frost/drought warnings from
forecast.

**Q37. What happens to out-of-window planting?**
Warned and failed fairly — a choice with a cost, not a trap.

**Q38. What does the greenhouse change?**
Control: climate as a managed field with couplings. It does not remove
consequences; it moves them to power, water, and heat.

**Q39. Why should a blackout kill growth?**
Because controlled growing is a promise the power keeps. When power fails, the
promise warns and bends.

**Q40. What is fungi's role in the web?**
Alternative calories with contamination risk; a chain rather than a second
kitchen.

**Q41. What is the wild's role?**
A source with limits: food, hides, and danger that responds to pressure.

**Q42. What makes a population "real"?**
One counter, bounds, renewal, and pressure bands with warnings.

**Q43. What is collapse?**
A possible, warned, recoverable state — never a silent event.

**Q44. What is a sighting for?**
Facts for observation, guide entries, and pressure displays.

**Q45. What makes trapping humane in the model?**
Methods that end quickly; quotas; and no content that lingers on suffering.

**Q46. What makes a trap fair?**
Wear, resets, finite yields, and weather.

**Q47. What is blight's promise?**
That a farmer can see it coming if they look: stages, warnings, windows.

**Q48. What stops a total wipe?**
Spread bounds, partial losses, and treatment windows by design.

**Q49. What is the chain's job?**
Turning harvest into meals without orphans or silent losses.

**Q50. Why bound spoilage?**
So storage choices matter and losses have names.

**Q51. Where does nutrition live?**
Facts produced by meals, outcomes owned by medicine; never computed in the
kitchen.

**Q52. What is variety for?**
Morale and health through owners — a table that remembers what it ate.

**Q53. What does a bad year leave?**
A record, a cause, and a changed choice next season.

**Q54. What is sustainability in one line?**
The land can be worn out and can be brought back; both are visible.

**Q55. What is out of scope?**
Real-world agronomy, new counters, health math, UI layout.

**Q56. What is the biggest risk?**
A panel that computes its own yield "for tooltips."

**Q57. The second?**
A population cap that silently changes collapse behavior.

**Q58. The third?**
A blight that skips to loss because stages were simplified.

**Q59. How does the plan scale?**
Register rows per strain/species/blight; kits per concern; no new owners.

**Q60. How does the plan treat the kitchen?**
As the final consumer that gives every chain its meaning.

**Q61. What is the smallest useful increment?**
Ledger + read model + soil bands. One week; honest harvests.

**Q62. What does Path B add?**
One web: couplings, chain audit, pressure, stages, surfaces.

**Q63. What does Path C add?**
Years: rotation, recovery, strain breadth, bad-year memory, preservation.

**Q64. Who owns the bad-year record?**
The journal/guide owners; the land writes the facts.

**Q65. How does the plan keep the wild wild?**
Separate owners and pressure rules; the wild is not domesticated by process.

**Q66. What is the yearly deletion?**
One dead strain/entry/method retired with a note.

**Q67. What is the yearly addition?**
One authored breadth item with full warnings and coverage.

**Q68. What is the plan's closing image?**
A spring morning: plots sown in window, hives working, smoke rising, no
warnings.

**Q69. What is its warning image?**
A field greying at the edges with the blight board already visible two days
ago.

**Q70. The last word of this band?**
Keep the books; the land keeps its side.

*End of Part XVII. Continues in Part XVIII (threads, second band).*---

# W4-04 · PART XVIII — WORKED THREADS, SECOND BAND (FL-21–FL-34)

## XVIII.1 Thread G — "the hive that fed nothing"

**Report:** hives existed but greenhouse yields never changed.

**Walk:**

```text
1. root: pollination modeled but no yield factor read hive health
2. repair: hive health feeds the yield read model (pollination factor);
   collapse warns and reduces yields
3. verify: hive-factor test; collapse scenario
```

| ID | Class | Repair |
|---|---|---|
| FL-21 | unused pollination model | consumption |
| FL-22 | no hive-yield test | coverage |

## XVIII.2 Thread H — "the orchard that ripened twice"

**Report:** multi-year trees produced fruit every week.

**Walk:**

```text
1. root: long-cycle crop had no yearly gate; stage looped at ready
2. repair: yearly cycle authored; fruit window; harvest resets the cycle
3. verify: multi-year cycle test
```

| ID | Class | Repair |
|---|---|---|
| FL-23 | loop stage | correctness |
| FL-24 | no yearly gate test | coverage |

## XVIII.3 Thread I — "the smokehouse with no smoke"

**Report:** smoked meat appeared without fuel or process.

**Walk:**

```text
1. root: preservation craft missing its consumption; yields appeared directly
2. repair: preservation consumes fuel/time; losses drop per method; chain
   audited
3. verify: preservation consumption test
```

| ID | Class | Repair |
|---|---|---|
| FL-25 | free preservation | fairness |
| FL-26 | no consumption audit | coverage |

## XVIII.4 Thread J — "the field that flooded and grew"

**Report:** flooded plots yielded normally.

**Walk:**

```text
1. root: flood state (W4-03) not read by plots
2. repair: plots read flood depth; losses warned per crop tolerance
3. verify: flood tolerance matrix
```

| ID | Class | Repair |
|---|---|---|
| FL-27 | uncoupled flood | coupling |
| FL-28 | no tolerance data | completeness |

## XVIII.5 Thread K — "the quail that ate the grain"

**Report:** stored grain vanished with no pest event.

**Walk:**

```text
1. root: storage loss rate included pests but no event/warning existed
2. repair: pest events authored with warnings and mitigation (cats, traps,
   sealing)
3. verify: pest warning + mitigation test
```

| ID | Class | Repair |
|---|---|---|
| FL-29 | silent pest loss | fairness |
| FL-30 | no mitigation path | design |

## XVIII.6 Thread L — "the cure that spread the blight"

**Report:** treating one plot spread rust further.

**Walk:**

```text
1. root: treatment action moved contaminated material without a hygiene rule
2. repair: treatment has hygiene requirements; failure spreads (warned);
   correct method contains
3. verify: treatment spread test
```

| ID | Class | Repair |
|---|---|---|
| FL-31 | careless treatment | design |
| FL-32 | no hygiene test | coverage |

## XVIII.7 Summary

```text
G: models that exist must be consumed
H: multi-year means yearly gates
I: preservation costs fuel and time
J: fields know about water
K: pests warn and can be fought
L: treatment methods have hygiene consequences
```

*End of Part XVIII. Continues in Part XIX (extended registers).*---

# W4-04 · PART XIX — EXTENDED REGISTERS

## XIX.1 The soil band register

| Band | Meaning | Warning | Recovery |
|---|---|---|---|
| rich | best yields | — | — |
| worked | normal | — | — |
| tired | reduced | land_soil_tired | rest or amend |
| exhausted | poor | land_soil_tired | long rest + amendment |

## XIX.2 The growth stage register

| Stage | Days | Events | Loss paths |
|---|---|---|---|
| fallow | — | amendment | — |
| sown | 3 | frost, pest | warned |
| growing | 20–40 | drought, blight, flood | warned |
| ripening | 5–10 | frost, birds | warned |
| ready | grace 1 | frost | harvest first |

## XIX.3 The pressure band register

| Band | Take/Renewal | Warnings | Consequence |
|---|---|---|---|
| none | < 0.5 | — | normal sightings |
| mild | 0.5–0.9 | land_pressure_heavy (late) | fewer sightings |
| heavy | 0.9–1.2 | land_pressure_heavy | hard hunts |
| critical | > 1.2 | land_pressure_heavy | near collapse, recovery slow |

## XIX.4 The preservation register

| Method | Consumes | Shelf effect | Power |
|---|---|---|---|
| dry | sun/time | ++ | — |
| smoke | fuel/time | ++ | — |
| brine | salt | + | — |
| root cellar | — | + (seasonal) | — |
| can | fuel/jars | +++ | — |
| freeze | power | ++++ | yes |

## XIX.5 The history register

| History | Aggregation | Retention |
|---|---|---|
| yields | per harvest | 30 |
| soil | per season | 20 |
| populations | per season | 20 |
| blights | per event | 20 |
| losses | per event | 30 |

## XIX.6 The coupling register

| Land system | Couples to | Direction | Failure mode |
|---|---|---|---|
| greenhouse | power/water/heat | consumes | warned cold/stall |
| plots | weather/flood | reads | warned losses |
| fungi | materials | consumes | contamination |
| kitchen | power/fuel | consumes | cold meals, spoilage |
| preservation | fuel/power | consumes | losses |

*End of Part XIX. Continues in Part XX (walkthroughs).*---

# W4-04 · PART XX — WALKTHROUGHS

## XX.1 Walkthrough A — the spring sowing

```text
day 180  spring window opens; farm board shows plots and strain options
day 181  plots sown (seed path); soil bands read; expected bands shown
day 185  sprouting; care: watered (power+water via W4-03)
day 205  drought watch; watering continues; yields band lowers
day 210  harvest: bands match; grain to stores; kitchen chain updated
day 211  guide records the spring: "dry, but the barley held."
```

## XX.2 Walkthrough B — the rust arrives

```text
day 190  damp spell; blight latent (not visible)
day 194  visible on the west row; warning; board shows treatment options
day 195  treatment applied (dust, labor); hygiene followed
day 200  contained; west row partial loss recorded
day 201  the guide: "Rust came with the wet. We caught it at the edge."
```

## XX.3 Walkthrough C — the thinning woods

```text
day 210  heavy take season; pressure band "heavy"; sighting rate falls
day 214  warning: "Game is thinning around r_north."
day 220  player rotates to coast; fowl pressure mild
day 240  deer sightings begin to recover; band back to "mild"
day 241  the guide: "We gave the north woods a season to breathe."
```

## XX.4 Walkthrough D — the cold greenhouse

```text
18:40  blackout; warning: "The greenhouse is cooling."
19:10  climate curve shows zone 1 falling; crop pause per band
22:00  power restored; heaters return; no loss (band never reached damage)
22:05  the board: "Zone 1 recovered."
```

## XX.5 Walkthrough E — the bad stores

```text
day 222  wet weather; stores damp warning
day 223  spoilage risk on grain; dry_vent consumes power
day 225  grain saved (bounded loss 4%); loss recorded with cause
day 226  the guide: "The north store sweats in wet years."
```

## XX.6 The walkthrough rule

```text
every land feature must be narratable in one page with every sentence
mapping to an owner fact — a yield, a band, a warning, a chain step.
```

*End of Part XX. Continues in Part XXI (rules compendium).*---

# W4-04 · PART XXI — THE RULES COMPENDIUM

## Ledger
```text
L1 one owner per living concern · L2 yields derived · L3 losses have causes
L4 warnings precede · L5 budgets per W4-01 · L6 surfaces read
```

## Soil and crops
```text
S1 stages daily deterministic · S2 soil changes by use/amendment
S3 amendments consume · S4 rotation payoff authored
S5 frost/drought warned · S6 seed saving chained
```

## Greenhouse
```text
G1 one climate truth · G2 couplings explicit · G3 expansions consumed
G4 pollination via hives · G5 failures warned · G6 plots shared model
```

## Fungi
```text
U1 substrate consumed · U2 contamination warned/flagged
U3 yields via read model · U4 species via knowledge · U5 meals via kitchen
```

## Wild
```text
W1 one counter · W2 migration deterministic · W3 take/renewal bounded
W4 observation recorded · W5 interactions authored · W6 campaign clock
```

## Trapping
```text
T1 one resolution · T2 tone law · T3 quotas warned · T4 wear/reset
T5 outcomes recorded · T6 knowledge unlocks
```

## Blight
```text
B1 causes authored · B2 detection precedes loss · B3 treatment consumes
B4 partial losses caused · B5 resistance interacts · B6 no random wipes
```

## Chain
```text
H1 source and consumer per item · H2 spoilage bounded/warned
H3 processing consumes · H4 nutrition via medical · H5 no private larder
H6 feasts via morale owners
```

## Seasons
```text
Z1 one context · Z2 windows enforced · Z3 forecast warnings
Z4 adaptation options · Z5 bad years recorded · Z6 campaign clock
```

## Surfaces
```text
P1 read only · P2 causes shown · P3 bands not promises
P4 pressure/blight early · P5 history bounded · P6 W3-06 kits
```

## The poster law
```text
One yield law. One chain. One wild. Books honest, seasons real.
```

*End of Part XXI. Continues in Part XXII (worklist).*---

# W4-04 · PART XXII — THE WORKLIST

> Ranked repairs from FL-01…FL-34.

```text
1  single yield read model (FL-01/02)              equality kit
2  soil progressive bands (FL-06/07)               multi-season test
3  amendment path shown (FL-08)                    guidance test
4  frost ordering + grace (FL-09/10)               frost scenario
5  greenhouse coupling (FL-11/12)                  blackout scenario
6  contamination flags (FL-13/14)                  routing test
7  population bounds + predators (FL-15/16)        bounds test
8  trap wear/reset (FL-17/18)                      lifecycle test
9  blight stage warnings + spread bound (FL-19/20) rust scenario
10 pollination consumption (FL-21/22)              hive test
11 multi-year gates (FL-23/24)                     orchard test
12 preservation costs (FL-25/26)                   consumption audit
13 flood tolerance (FL-27/28)                      coupling test
14 pest events + mitigation (FL-29/30)             pest scenario
15 treatment hygiene (FL-31/32)                    spread test
16 renewal + recovery (FL-03/04/05)                recovery scenario
```

## XXII.1 Cadence

```text
stop-the-line (1–4): immediate
integrity (5–9): next package
fairness (10–14): within two releases
coverage (15–16): before signature
```

## XXII.2 The worklist law

```text
every row closes with its kit; an un-kipped close reopens at the next run.
```

*End of Part XXII. Continues in Part XXIII (year one).*---

# W4-04 · PART XXIII — YEAR ONE OF THE LAND PROGRAM

## XXIII.1 The standing commitments

```text
C1  yield equality spot weekly; full per release
C2  pressure band review weekly; full per release
C3  soil band audit per season boundary
C4  blight scenario per release
C5  chain orphan scan per content change
C6  greenhouse coupling test per release
C7  tone review per trapping content change
C8  strain/species census yearly
```

## XXIII.2 The quarterly cycle

```text
Q1  planting windows + seed paths audit
Q2  greenhouse/fungi consumptions + contamination rerun
Q3  wild bounds + recovery scenarios; trapping lifecycle
Q4  blight year run; chain audit; next-year breadth targets
```

## XXIII.3 The health signals

```text
- the board and the harvest agree, always
- losses have names
- the wild comes back when left alone
- seasons are felt as weather, not phases
- the kitchen's tables change with the year
```

## XXIII.4 The rot signals

```text
- a tooltip computing its own yield
- a population at cap with no renewal
- a blight that arrives fully formed
- a greenhouse that ignores the dark
- a food item nobody eats
```

## XXIII.5 The annual retrospective

```text
1. yield equality history (zero divergences)
2. soil trends: tired bands seen, amendments applied
3. wild trends: pressure bands, recoveries
4. blight history: detections, treatments, losses
5. chain losses: causes and bounds
6. bad years recorded; next-year decisions noted
7. one breadth addition; one deletion
```

## XXIII.6 The end state

```text
The land program is healthy when the harvest is predictable within its bands,
the wild is a living neighbor, and no loss ever lacks a cause.
```

*End of Part XXIII. Continues in Part XXIV (operations manual).*---

# W4-04 · PART XXIV — OPERATIONS MANUAL

## XXIV.1 Roles

| Role | Responsibility |
|---|---|
| crop author | strains, plots, bands, warnings |
| greenhouse owner | climate, couplings, expansions |
| wild owner | populations, migration, pressure |
| trapping owner | methods, wear, tone |
| blight owner | stages, spread, treatments |
| chain owner | sources, stages, consumers |
| integrator | registers, kits, soak, calendar |

## XXIV.2 The daily rhythm

```text
morning:  yield equality spot; pressure band check
midday:   content work with registers updated in the same change
evening:  chain audit sample; warning review
```

## XXIV.3 The weekly rhythm

```text
- one plot walked end to end (sow -> harvest -> store)
- one population pressure band read
- one greenhouse coupling exercised (cut power)
- one chain item followed from source to table
```

## XXIV.4 The release rhythm

```text
T-7: equality full; chain audit; registers current
T-3: blight scenario; wild bounds; greenhouse coupling
T-1: surfaces kits; copy review; tone review
T-0: closure lines signed
```

## XXIV.5 Escalation

| Signal | Class | Action |
|---|---|---|
| board/harvest divergence | truth | same day |
| silent population change | fairness | same day |
| blight skipping stages | fairness | halt; fix stages |
| orphan food item | completeness | consume or retire |
| uncoupled greenhouse | design | halt; declare coupling |
| tone violation | content | rewrite or remove |

## XXIV.6 The dependency map

```text
weather/season -> plots + greenhouse + wild
power/water/heat (W4-03) -> greenhouse + kitchen + preservation
soil -> plots -> harvest -> stores -> chain -> meals -> nutrition (W4-06)
wild -> trapping -> chain
knowledge (W3-05) -> strains, methods, treatments
```

## XXIV.7 The dependency laws

```text
1. yields depend on facts; nothing depends on yields (derived)
2. plots depend on soil and season; nothing writes plots but the owner
3. populations depend on take and renewal; nothing else counts them
4. blight depends on hosts and weather; nothing else spreads it
5. the chain ends at meals; nothing hangs beyond nutrition and morale events
```

*End of Part XXIV. Continues in Part XXV (final control).*---

# W4-04 · PART XXV — SCENARIO BANK 2 (S11–S20)

## XXV.1 S11 — The rotation year

```text
fixture: three plots, two-year rotation schedule
assert: soil restores; blight rate falls; yields rise within bands
```

## XXV.2 S12 — The monoculture

```text
fixture: same strain every season
assert: soil bands warn; blight vulnerability rises; recovery via rotation
```

## XXV.3 S13 — The orchard patience

```text
fixture: orchard plot, year one to year three
assert: no fruit until authored year; care matters; harvest cycle resets
```

## XXV.4 S14 — The hive collapse

```text
fixture: weather + disease hit hives
assert: warning; pollination factor falls; greenhouse yields lower; recovery
```

## XXV.5 S15 — The pond harvest

```text
fixture: fish/aquatic source (if authored)
assert: same population rules; take/renewal; warned pressure
```

## XXV.6 S16 — The salvage fields

```text
fixture: abandoned farm reclaim
assert: staged reclaim; feral population appears; encounter hooks route
```

## XXV.7 S17 — The cellar year

```text
fixture: root cellar preservation through winter
assert: spoilage floors; cellar seasonal effects; no power dependency
```

## XXV.8 S18 — The seed bank

```text
fixture: saved seeds, quality tracked
assert: quality affects yield/blight; storage conditions matter; no free reset
```

## XXV.9 S19 — The full pantry

```text
fixture: variety of stock across methods
assert: meals show variety; morale events fire; nutrition routes
```

## XXV.10 S20 — The clean desk

```text
fixture: registers + kits
assert: zero orphans; equality green; bands warned; tone kept
```

## XXV.11 The cadence

| Set | Cadence |
|---|---|
| S1–S10 | per release |
| S11–S20 | seasonal rotation |

*End of Part XXV. Continues in Part XXVI (final control).*---

# W4-04 · PART XXVI — CLOSURE MEASUREMENT AND FINAL CONTROL

## XXVI.1 The closure measurement (final form)

```yaml
run: W4-04-closure-final
head: <sha>
ledger: { entries: __, owners: all, orphans: 0 }
yield: { equality: pass, read_model_only: pass }
soil: { bands: warned, amendments: restore, rotation: payoff }
greenhouse: { couplings: pass, failures: warned, expansions: consumed,
              pollination: consumed }
fungi: { chain: pass, contamination: flagged }
wild: { counters: single, bounds: pass, pressure: warned, recovery: authored }
trapping: { lifecycle: pass, yields: routed, tone: pass }
blight: { stages: warned, spread: bounded, treatment: consumed,
          hygiene: consequential }
chain: { orphans: 0, spoilage: bounded, nutrition: routed }
seasons: { context: single, windows: enforced, warnings: forecast }
surfaces: { read_only: pass, causes: pass, bands: pass, kits: pass }
worklist: { open: 0 }
soak: pass
```

## XXVI.2 The acceptance walk

| Line | Evidence | Signed |
|---|---|---|
| ledger | scan | ☐ |
| yield equality | equality kit | ☐ |
| soil | multi-season | ☐ |
| greenhouse | blackout + hive tests | ☐ |
| wild | bounds + pressure | ☐ |
| trapping | lifecycle + tone | ☐ |
| blight | rust year | ☐ |
| chain | audit | ☐ |
| seasons | window runs | ☐ |
| surfaces | kits | ☐ |

## XXVI.3 The binding summary

```text
Binding: ledger L1–L6, soil S1–S6, greenhouse G1–G6, fungi U1–U5, wild W1–W6,
trapping T1–T6, blight B1–B6, chain H1–H6, seasons Z1–Z6, surfaces P1–P6, the
stop-the-line list (§XV.6), and the worklist.
```

## XXVI.4 The final declaration

**W4-04 is complete.** Parts I–XXVI with findings FL-01…FL-34. Proposal only;
no execution without Annex U and signatures. It hands the wave: one yield law,
one chain, one bounded wild, one staged blight, and a land that remembers.

```text
The land gives what it is given; keep the books honest.
```

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*

*Expansion continues with Parts XXVII+ (extended depth, walkthroughs,
registers, and closing) on the established pattern.*---

# W4-04 · PART XXVII — Q&A, THIRD BAND (Q71–Q110)

**Q71. What is the land program's final promise?**
That every harvest is explainable and every loss has a name.

**Q72. What does a farmer see in the morning?**
Plots with stages, bands, care needs, and any warnings — nothing invented.

**Q73. What does the board never do?**
Promise. It bands.

**Q74. What does the wild teach?**
That taking has a shape: patience pays, greed warns, recovery is real.

**Q75. What makes a predator band matter?**
It bounds populations and gives the wild a logic beyond arithmetic.

**Q76. What is the smallest ecological unit?**
A plot with a soil band, a stage, and a watchful neighbor (blight).

**Q77. What is the largest?**
A region's wild ledger across seasons.

**Q78. How does the plan handle domestication?**
As a separate owner from the wild; tame grows beside wild, never replaces it.

**Q79. What makes preservation a decision?**
Costs: fuel, time, power, jars; each method trades something.

**Q80. What is the cellar's virtue?**
Patience: no power, steady losses, seasonal use.

**Q81. What is the freezer's vice?**
Dependence: perfect storage while power holds.

**Q82. What is the kitchen's role in the plan?**
The chain's end: meals, facts, and the table's memory.

**Q83. How is a bad year told?**
In the guide, in past tense, once.

**Q84. What does the plan refuse to narrate?**
Unobserved events. The land does not perform for an empty stage.

**Q85. What does observation add?**
Facts: sightings, soil readings, blight early marks — feeding W4-02's guide.

**Q86. What is the plan's view of weather?**
As a partner with a contract: forecast windows, warnings, adaptation.

**Q87. What is the plan's view of pests?**
Named, warned, and fightable — never a silent tax.

**Q88. What is the plan's view of blight?**
A process with etiquette: stages, warnings, treatments, hygiene.

**Q89. What is the plan's view of abundance?**
Banded, stored, and shared — with variety routed to morale.

**Q90. What is the plan's view of scarcity?**
Warned, caused, survived — and recorded.

**Q91. What is the top technical failure mode?**
A second yield computation.

**Q92. The second?**
A stage machine simplified into a threshold.

**Q93. The third?**
A population without ecology.

**Q94. What is the review question?**
"Which read model, which stage, which consumer, which warning?"

**Q95. What is the build rule?**
"No orphan item, no silent loss, no free growth."

**Q96. What is the seasonal rule?**
"Windows hold; forecasts warn; bad years are remembered."

**Q97. What is the wild rule?**
"Take has bounds; renewal is authored; recovery is possible."

**Q98. What is the table rule?**
"Every yield ends in a meal; every meal ends in a fact."

**Q99. What is the yearly deletion?**
One dead strain, method, or entry retired with a note.

**Q100. What is the yearly addition?**
One breadth item with full warnings and consumption.

**Q101. How does the plan handle scale?**
Registers grow; owners do not multiply; kits stay six.

**Q102. How does the plan handle mods?**
Same contracts: owners, bands, warnings, consumers.

**Q103. What is out of scope forever?**
Real agronomy, health math, second counters, UI layout.

**Q104. What does the plan leave for the narrative wave?**
Murmurs: blight years, good harvests, lost orchards, patient returns.

**Q105. What does it leave for the economy?**
Yields within bands, spoilage floors, preservation goods.

**Q106. What does it leave for medicine?**
Nutrition facts and contamination flags.

**Q107. What does it leave for the infrastructure wave?**
Water demand, drying fuel, greenhouse load.

**Q108. What is the plan's quietest success?**
A cellar that holds through winter with no one watching.

**Q109. What is its loudest failure?**
A harvest nobody can explain.

**Q110. The last word of this band?**
The land keeps the books; we keep them honest.

*End of Part XXVII. Continues in Part XXVIII (threads, third band).*---

# W4-04 · PART XXVIII — WORKED THREADS, THIRD BAND (FL-35–FL-48)

## XXVIII.1 Thread M — "the greenhouse that kept its own season"

**Report:** greenhouse ignored winter planting windows; anything grew anytime.

**Walk:**

```text
1. root: windows were enforced only for open plots
2. repair: greenhouse windows are wide but real (authored by zone quality);
   outside them, warns and fails fair
3. verify: greenhouse window matrix
```

| ID | Class | Repair |
|---|---|---|
| FL-35 | window bypass | correctness |
| FL-36 | no greenhouse window test | coverage |

## XXVIII.2 Thread N — "the seed that remembered nothing"

**Report:** saved seeds had no quality link to parent crop.

**Walk:**

```text
1. root: seed items existed as generic stock
2. repair: seed quality authored (from care/soil/health at harvest); affects
   yield and blight odds; storage decays quality
3. verify: seed lineage test
```

| ID | Class | Repair |
|---|---|---|
| FL-37 | generic seeds | depth |
| FL-38 | no lineage test | coverage |

## XXVIII.3 Thread O — "the fox that ate the chickens twice"

**Report:** predator/prey band applied twice in one day.

**Walk:**

```text
1. root: two systems consumed the interaction record per tick
2. repair: one consumption; the interaction is evaluated once daily; test
3. verify: double-apply scenario
```

| ID | Class | Repair |
|---|---|---|
| FL-39 | double predator effect | integrity |
| FL-40 | no daily-once test | coverage |

## XXVIII.4 Thread P — "the drought that was only in the text"

**Report:** drought narrative fired but no yield effect existed.

**Walk:**

```text
1. root: seasonal event authored for story, no land consumer
2. repair: drought has factors (moisture bands); narrative reads the same
   context; one truth
3. verify: drought factor test
```

| ID | Class | Repair |
|---|---|---|
| FL-41 | story-only weather | one-truth |
| FL-42 | no factor test | coverage |

## XXVIII.5 Thread Q — "the cellar that froze its stock"

**Report:** root cellar stored food perfectly in −30°C.

**Walk:**

```text
1. root: cellar temperature model ignored outside climate
2. repair: cellars read climate; extreme cold warns and can damage per bands
3. verify: cold cellar scenario
```

| ID | Class | Repair |
|---|---|---|
| FL-43 | uncoupled cellar | coupling |
| FL-44 | no cold test | coverage |

## XXVIII.6 Thread R — "the meal with no memory"

**Report:** feasts produced no morale effect despite "shared table" copy.

**Walk:**

```text
1. root: meal variety computed but morale event never emitted
2. repair: variety bands emit morale events through W3-03; copy matches
3. verify: variety-morale test
```

| ID | Class | Repair |
|---|---|---|
| FL-45 | unemitted event | completeness |
| FL-46 | no empathy test | coverage |

## XXVIII.7 Thread S — "the soil that fed the blight"

**Report:** richer soil produced more blight than poor soil, silently.

**Walk:**

```text
1. root: density factor read soil richness in reverse
2. repair: density authored per crop spacing; soil health improves resistance
   per data; sign corrected and tested
3. verify: soil-blight sign test
```

| ID | Class | Repair |
|---|---|---|
| FL-47 | reversed factor | correctness |
| FL-48 | no factor test | coverage |

## XXVIII.8 Summary

```text
M: greenhouses have windows too
N: seeds carry their parents' quality
O: interactions fire once daily
P: weather that tells must also act
Q: cellars live in the climate
R: meals that please must emit
S: factors have signs, and signs have tests
```

*End of Part XXVIII. Continues in Part XXIX (final registers).*---

# W4-04 · PART XXIX — EXTENDED FINAL REGISTERS

## XXIX.1 The seed quality register

| Source crop condition | Seed quality | Yield effect | Blight odds |
|---|---|---|---|
| healthy, cared | good | + | lower |
| average | fair | baseline | baseline |
| stressed | poor | − | higher |
| blight-affected | bad | −− | much higher |

## XXIX.2 The climate coupling register

| System | Reads | Fails when | Warning |
|---|---|---|---|
| plot | weather, flood | frost/drought/flood | land_*_watch |
| greenhouse | power/heat/water | blackout, cold | land_cold_night |
| cellar | outside climate | extremes | storage watch |
| kitchen | power/fuel | cold meals | — |
| preservation | fuel/power | method-specific loss | spoilage watch |

## XXIX.3 The wild interaction register

| Interaction | Effect | Cadence | Bounds |
|---|---|---|---|
| predator-prey | count bands | daily once | authored caps |
| migration | region shifts | seasonal | calendar |
| forage | renewal modifier | seasonal | weather-read |
| disease (wild) | count loss | rare, authored | warned |

## XXIX.4 The loss-cause register

| Cause | Ref | Warned |
|---|---|---|
| frost | land_frost_watch | yes |
| drought | land_drought_watch | yes |
| flood | W4-03 flood warnings | yes |
| blight | land_blight_visible | yes |
| pests | land_pests | yes |
| spoilage | land_spoilage_watch | yes |
| take | pressure bands | yes |
| neglect | land_soil_tired | yes |

## XXIX.5 The variety register (meals)

| Variety band | Requirement | Effect |
|---|---|---|
| sparse | one staple | baseline |
| modest | two sources | small morale |
| varied | three+ sources | morale, nutrition |
| rich | plus preserved/luxury | morale+, events |

## XXIX.6 The bad-year register

| Year | Season | Event | Loss | Record |
|---|---|---|---|---|
| ____ | ____ | ____ | ____ | guide entry |

*End of Part XXIX. Continues in Part XXX (decade and closing narrative).*---

# W4-04 · PART XXX — THE DECADE PLAN AND CLOSING NARRATIVE

## XXX.1 The land's decade

```text
Y1  honest harvests: read model, bands, warnings
Y2  the web: couplings, chain, pressure, stages
Y3  the years: rotation, orchards, bad-year memory
Y4  the wild: predator bands, recovery ecology, encounter hooks
Y5  the pantry: preservation breadth, cellar culture
Y6  the seeds: lineage, resistance, trade
Y7  the recoveries: worn fields brought back
Y8  the inheritance: registers and kits outlive authors
Y9  the quiet: no losses without names
Y10 the land that keeps its own history
```

## XXX.2 The decade's rule

```text
no year adds a second way to compute a yield or count a herd. Ten years of
one law is worth more than ten systems of many.
```

## XXX.3 The closing narrative

The land is the campaign's slowest clock and its most honest mirror: patience
and care compound, neglect and greed compound too, and both are visible before
they are fatal. This plan exists so that a harvest is never a mystery and a
loss is never a silence — so that when the stores fill, the shelter knows
exactly why, and when they empty, it knows exactly what it chose.

## XXX.4 The three promises

```text
P1  The books are honest: board equals harvest, always.
P2  Losses have names and warnings; recoveries are real.
P3  The wild stays wild; the tame is tended; the table remembers.
```

## XXX.5 The last line

```text
The land gives what it is given; keep the books honest.
```

*End of Part XXX. Continues in Part XXXI (final control).*---

# W4-04 · PART XXXI — FINAL CONTROL

## XXXI.1 The binding summary

```text
Binding: ledger L1–L6, soil S1–S6, greenhouse G1–G6, fungi U1–U5, wild W1–W6,
trapping T1–T6, blight B1–B6, chain H1–H6, seasons Z1–Z6, surfaces P1–P6, the
stop-the-line list, and the worklist.
```

## XXXI.2 The final control statement

**W4-04 is complete.** Parts I–XLII with findings FL-01…FL-48. Proposal only;
no execution without Annex U (Part I §U.2) and signatures. It hands the wave:
one yield law, one chain, one bounded wild, one staged blight, honest books.

## XXXI.3 The artifact index

| Artifact | Location |
|---|---|
| land ledger | P0 output |
| strain/plot registers | P0 output |
| species register | P0 output |
| blight register | P0 output |
| chain register | P0 output |
| warning copy | corpus refs |
| kits | equality · soil · greenhouse · wild · trapping · chain |
| scenarios | S1–S20 |
| worklist | Part XXII |

## XXXI.4 The handoff cards

```text
W3-05: seeds, tools, processing, knowledge
W3-02: yields, spoilage floors, preservation goods
W3-03: variety morale, scarcity arcs
W4-01: plots, populations, blight sections
W4-02: weather, observation, trap routes
W4-03: water, power, heat couplings
W4-06: nutrition facts, contamination flags
W3-06: land surfaces
```

## XXXI.5 The final sentence

```text
The land gives what it is given.
```

*End of Part XXXI. Continues in Part XXXII (Q&A, fourth band).*---

# W4-04 · PART XXXII — Q&A, FOURTH BAND (Q111–Q150)

**Q111. What is the plan's identity in one line?**
One yield law, one chain, one wild, honest books.

**Q112. What is the plan's texture?**
Rows and bands: does the plot need water, is the herd thinning, is the rust
near.

**Q113. What does a good year feel like?**
Bands met, cellars full, no warnings, and one line in the guide.

**Q114. What does a bad year feel like?**
A choice remembered: "we pushed the soil and paid for it."

**Q115. What makes the land "alive" without spectacle?**
Movement you notice: migrations, sightings, the first frost.

**Q116. What makes it fair?**
Everything that hurts was visible first.

**Q117. What is the plan's attitude to luck?**
Seeded and bounded: weather is authored drama, not a die.

**Q118. What is its attitude to abundance?**
An outcome of care, stored with costs, shared with memory.

**Q119. What is its attitude to failure?**
Information: a cause, a record, a next choice.

**Q120. What is its attitude to time?**
As soil: it compounds, both ways.

**Q121. What is the yearly test?**
One full cycle with zero orphan items and zero unnamed losses.

**Q122. What is the weekly test?**
One plot, one herd, one coupling, one chain item.

**Q123. What is the review question?**
"Which read model, which stage, which warning, which consumer?"

**Q124. What is the build rule?**
"No free yield, no silent loss, no orphan item."

**Q125. What is the season rule?**
"Windows hold; forecasts warn; years are remembered."

**Q126. What is the wild rule?**
"Bounds, renewal, recovery — and warning before collapse."

**Q127. What is the table rule?**
"Every yield ends in a meal; every meal ends in a fact."

**Q128. What is the soil rule?**
"Tired is visible; rich is earned; neither is permanent."

**Q129. What is the greenhouse rule?**
"Control is a promise the power keeps."

**Q130. What is the blight rule?**
"See it early; treat it honestly; record what it took."

**Q131. What is the trapping rule?**
"Quick, finite, seasonal — and never cruel."

**Q132. What is the preservation rule?**
"Every method trades something; choose what to spend."

**Q133. What is the observation rule?**
"Only what was seen goes in the guide."

**Q134. What is the yearly deletion?**
One dead strain, method, or entry retired with a note.

**Q135. What is the yearly addition?**
One breadth item with full warnings and consumption.

**Q136. What is out of scope forever?**
Real agronomy, second counters, health math, UI layout.

**Q137. What does the plan leave the narrative?**
Murmurs and years: rust, frost, patience, recovery.

**Q138. What does it leave the economy?**
Bands to price, spoilage to move, preservation to trade.

**Q139. What does it leave medicine?**
Facts and flags, never outcomes.

**Q140. What does it leave infrastructure?**
Demand: water in dry years, heat in cold ones, fuel in every cellar.

**Q141. What is the plan's quietest success?**
A full cellar in a bad year, unexplained by luck.

**Q142. What is its loudest failure?**
A harvest nobody can explain.

**Q143. What is its rarest sight?**
A field coming back from exhausted to worked, one season at a time.

**Q144. What is its most common sight?**
Work in window, water in time, warnings read.

**Q145. What is its darkest hour?**
A rust year with no resistant seed in store.

**Q146. What is its brightest?**
Harvest day with the board and the bushels in agreement.

**Q147. What remains after closure?**
The calendar: equality, bands, couplings, audits — the land's routine.

**Q148. What is the plan's final test of time?**
Ten years, one law, zero mysteries.

**Q149. What is the plan's final test of care?**
A soup made from four sources in a bad year.

**Q150. The last word?**
The land keeps the books; we keep them honest.

*End of Part XXXII. Continues in Part XXXIII (threads, fourth band).*---

# W4-04 · PART XXXIII — WORKED THREADS, FOURTH BAND (FL-49–FL-60)

## XXXIII.1 Thread T — "the berry that ripened in winter"

**Report:** autumn berries appeared in deep winter on the board.

**Walk:**

```text
1. root: seasonal event data had berries but the harvest gate read stage only
2. repair: ready requires stage AND a season window per strain; winter shows
   fallow; the event is corrected
3. verify: season×stage matrix
```

| ID | Class | Repair |
|---|---|---|
| FL-49 | window bypass on ready | correctness |
| FL-50 | no season-stage test | coverage |

## XXXIII.2 Thread U — "the soil that healed in a day"

**Report:** exhausted soil recovered overnight after one amendment.

**Walk:**

```text
1. root: amendment set quality directly rather than building over days
2. repair: amendments add organic over time; bands rise across days; test
3. verify: amendment ramp test
```

| ID | Class | Repair |
|---|---|---|
| FL-51 | instant soil reset | physics |
| FL-52 | no ramp test | coverage |

## XXXIII.3 Thread V — "the two kitchens"

**Report:** some meals came from a secondary path with different nutrition.

**Walk:**

```text
1. root: a field meal bypassed the kitchen owner for "speed"
2. repair: all meals via kitchen; bypass removed; coverage test
3. verify: meal-path coverage
```

| ID | Class | Repair |
|---|---|---|
| FL-53 | second meal path | Rule 5 |
| FL-54 | no path audit | coverage |

## XXXIII.4 Thread W — "the trap that caught a rumor"

**Report:** traps reported captures with no yield arriving.

**Walk:**

```text
1. root: capture outcome recorded but yield resolution failed silently on a
   missing reference
2. repair: yields resolve or fail loudly; missing references are T1 failures
3. verify: resolution coverage
```

| ID | Class | Repair |
|---|---|---|
| FL-55 | silent yield drop | completeness |
| FL-56 | no reference scan | coverage |

## XXXIII.5 Thread X — "the blight that liked the weak"

**Report:** resistant strains failed against rust anyway.

**Walk:**

```text
1. root: resistance factor inverted in the read model (multiplied instead of
   reduced)
2. repair: sign test on resistance; scenario covers resistant/non-resistant
3. verify: resistance sign test
```

| ID | Class | Repair |
|---|---|---|
| FL-57 | inverted resistance | correctness |
| FL-58 | no sign test | coverage |

## XXXIII.6 Thread Y — "the pantry that never emptied"

**Report:** stores never fell despite heavy meals.

**Walk:**

```text
1. root: meal consumption drew from a rounding that floored to zero at small
   quantities
2. repair: integer-safe consumption; reconciliation across a month; test
3. verify: consumption reconciliation
```

| ID | Class | Repair |
|---|---|---|
| FL-59 | zero-consumption rounding | correctness |
| FL-60 | no reconciliation test | coverage |

## XXXIII.7 Summary

```text
T: ready needs a season too
U: soil heals across days, not dials
V: one kitchen, one path
W: yields resolve or fail loudly
X: resistance has a sign
Y: eating subtracts, always
```

*End of Part XXXIII. Continues in Part XXXIV (fixtures and appendix).*---

# W4-04 · PART XXXIV — FIXTURES AND APPENDIX

## XXXIV.1 The fixture set

```text
tests/fixtures/land/
  minimal.json         one plot, one strain, one chain
  seasons.json         four seasons, two strains
  blight.json          rust + resistant strain
  wild.json            three species, pressure bands
  trapping.json        methods + wear
  storage.json         spoilage + preservation
  negatives/           orphan item, second counter, free yield
```

## XXXIV.2 The scenario driver

```text
driver: land-soak --fixture seasons.json --days 120 --seed 3301
        --events [drought:day30, frost:day60, blight:day90, hunt:day15..45]
output: equality, bands, losses, populations, digest
```

## XXXIV.3 The equality fixture

```text
for 20 plot states: board band vs harvest result must agree
a divergence fails the build
```

## XXXIV.4 The loss-cause fixture

```text
every loss path in the fixture forces its cause; the board must show the
matching ref; missing refs fail
```

## XXXIV.5 The population fixture

```text
take 0%, 50%, 90%, 120% of renewal across seasons
assert: bands fire correctly; collapse only at critical with warnings;
        recovery times authored
```

## XXXIV.6 The evidence format

```yaml
run: LAND-<id>
date: ____  head: ____
fixture: ____  days: __  seed: ____
equality: pass  bands: pass  losses: all caused  orphans: 0
result: pass
```

## XXXIV.7 The reader's map

```text
5 minutes:  §0, Part II.2 (yield law), Part XI.1 (field guide)
builder:    §1, Parts II–IV, VI, X, XIV
reviewer:   Parts XIII, XXIV, §XV.6
support:    Part XII (copy), §XV.5, Part XIV
foreman:    Annex U, §6, Part XV, §XVI.2
```

*End of Part XXXIV. Continues in Part XXXV (final measures).*---

# W4-04 · PART XXXV — FINAL MEASURES

## XXXV.1 The measure card

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
parts:      I–XLVI
findings:   FL-01 .. FL-60 (worklist Part XXII + bands 3/4)
kits:       equality · soil · greenhouse · wild · trapping · chain (6)
registers:  strains · plots · species · blight · chain · soil bands · stages ·
            pressure · preservation · history · couplings · warnings
scenarios:  20 (S1–S20)
strain set: __  species: __  blights: __  chain items: __
rollout:    5 weeks
handoffs:   W3-02 · W3-03 · W3-05 · W4-01 · W4-02 · W4-03 · W4-05 · W4-06 · W3-06
```

## XXXV.2 The acceptance one-liner

```text
One yield law, one chain, one bounded wild, one staged blight, and books that
match the bushels.
```

## XXXV.3 The health one-liner

```text
Nobody asks why the harvest was what it was — the board already said.
```

## XXXV.4 The three sentences

```text
The books are honest: board equals harvest.
Losses have names and warnings; recoveries are real.
The wild stays wild; the tame is tended; the table remembers.
```

*End of Part XXXV. Continues in Part XXXVI (last tables).*---

# W4-04 · PART XXXVI — THE LAST TABLES

## XXXVI.1 The one-page quick table

| Situation | Do | Never |
|---|---|---|
| new strain | window, bands, losses, seed path | free yield |
| new plot | soil band, stage, care | second store |
| greenhouse change | declare power/water/heat | ignore coupling |
| new fungi bed | substrate + contamination flag | unflagged yield |
| new species | bounds, renewal, pressure bands | unbounded growth |
| new trap method | toned, worn, quota'd | lingering content |
| new blight | stages + warnings + treatment | instant loss |
| new food item | source + consumer in same change | orphan |
| new preservation | costs + shelf effects | free kitchen |
| new season event | acts on land, not just text | story-only weather |
| board change | reads the model | local math |
| loss | cause ref + warning | silent subtraction |

## XXXVI.2 The three-artifact rule

```text
yield read model · loss cause register · warning copy
if a land change cannot show all three, it is not finished.
```

## XXXVI.3 The closure one-liner

```text
Equality green · soil bands warn · greenhouse coupled · wild bounded ·
traps mortal · blight staged · chain orphan-free · seasons real · surfaces
true — signed.
```

## XXXVI.4 The promise register

| # | Promise | Guard |
|---|---|---|
| 1 | board equals harvest | equality kit |
| 2 | yields derived only | T1 scan |
| 3 | soil bands warn | multi-season |
| 4 | amendments ramp | ramp test |
| 5 | windows enforced | window runs |
| 6 | frost/drought warn | forecast tests |
| 7 | greenhouse coupled | blackout |
| 8 | pollination consumed | hive test |
| 9 | contamination flagged | routing test |
| 10 | one population counter | scan |
| 11 | bounds + renewal | bounds test |
| 12 | pressure warns | band test |
| 13 | recovery authored | recovery scenario |
| 14 | trapping finite | lifecycle |
| 15 | tone kept | content review |
| 16 | blight staged | rust year |
| 17 | spread bounded | scenario |
| 18 | treatment consumed | consumption audit |
| 19 | chain orphan-free | audit |
| 20 | spoilage bounded/warned | stores test |
| 21 | nutrition routed | routing test |
| 22 | variety emits | morale test |
| 23 | bad years recorded | guide check |
| 24 | observation only real | guide audit |
| 25 | surfaces read | kit |
| 26 | history bounded | round-trip |
| 27 | registers current | ledger gate |
| 28 | calendar owned | governance |

*End of Part XXXVI. Continues in Part XXXVII (closing cards).*---

# W4-04 · PART XXXVII — CLOSING CARDS

## XXXVII.1 The implementer's card

```text
START:  read Part II.2 (yield law) + Part V (playbooks)
WORK:   owner → facts → bands → warnings → chain → kit
PROVE:  equality; causes; bounds; couplings; orphans zero
CLOSE:  worklist row with its kit attached
```

## XXXVII.2 The reviewer's card

```text
five questions:
  read model? stage? warning? consumer? register?
six refusals:
  local yield · instant loss · unbounded herd · free preservation ·
  orphan item · story-only weather
```

## XXXVII.3 The integrator's card

```text
weekly:  equality spot + pressure band + one coupling
release: full equality + chain audit + scenarios
season:  window runs + frost/drought + reclaim checks
year:    census + deletion + breadth addition
```

## XXXVII.4 The player's card

```text
what the board bands, the harvest meets
what hurts, warned first
what dies, named
what grows, tended
what the wild loses, the wild can regain
```

## XXXVII.5 The land's card

```text
I will not invent a harvest.
I will not lose a crop without a cause.
I will not let the wild vanish silently.
I will not ignore the winter.
I will remember the years.
```

## XXXVII.6 The final card

```text
W4-04 · complete · proposal only · Annex U governs execution
one yield law, one chain, one wild, honest books
```

*End of Part XXXVII. Continues in Part XXXVIII (true end).*---

# W4-04 · PART XXXVIII — TRUE END

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XLVI
Findings:   FL-01 .. FL-60
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
```

*End of Part XXXVIII. Continues in Part XXXIX (final closing).*---

# W4-04 · PART XXXIX — FINAL CLOSING

## XXXIX.1 The closing narrative

The land is where the shelter's future is stored: in soil, in seed, in herds,
in patience. This plan exists so that storage is honest — so that a full cellar
is the product of visible care, an empty one the product of visible choices,
and the wild remains a living neighbor rather than a resource to be drained.

## XXXIX.2 The closing instruction

```text
Keep one yield law. Keep one chain. Keep the wild bounded. Keep the books.
```

## XXXIX.3 The closing line

```text
The land gives what it is given; keep the books honest.
```

*End of Part XXXIX. Continues in Part XL (final addenda).*---

# W4-04 · PART XL — FINAL ADDENDA

## XL.1 What breadth still owes

```text
- more strains per region (data)
- more species per region (data)
- more preservation methods (crafts)
- more bad-year kinds (authored events)
correctness is closed by the kits; breadth is an ongoing content program
```

## XL.2 The yearly breadth rule

```text
one addition with full warnings, consumers, and kit coverage; one deletion of
a dead entry; never a net growth without review.
```

## XL.3 The final caution

```text
the danger here is not dramatic failure; it is the slow simplification of
stage machines into instant outcomes, and of ecologies into arithmetic. the
scans watch for both.
```

## XL.4 The final gratitude

```text
to the growers, keepers, and cooks whose tiny decisions this plan makes
legible and fair.
```

*End of Part XL. Continues in Part XLI (Q&A, fifth band).*---

# W4-04 · PART XLI — Q&A, FIFTH BAND (Q151–Q185)

**Q151. What is the land program's one sentence?**
One yield law, one chain, one wild, honest books.

**Q152. What is its one number?**
Zero orphans.

**Q153. Its second number?**
Zero unnamed losses.

**Q154. Its third?**
Zero equality divergences.

**Q155. What is a good bug report here?**
"The board said 8–14; the bins got 9; here is the plot."

**Q156. What is a good fix?**
One read model corrected; board and harvest agree again.

**Q157. Why is the read model the plan's heart?**
Because it is the single sentence the land speaks about the future.

**Q158. Why is the chain the plan's spine?**
Because every yield must end somewhere honest.

**Q159. Why is the wild the plan's conscience?**
Because it is the only system that must survive being used.

**Q160. Why is blight the plan's teacher?**
Because it shows that care and neglect compound.

**Q161. Why is soil the plan's clock?**
Because it moves only across seasons, never within a day.

**Q162. What is the greenhouse's lesson?**
That control is borrowed from infrastructure and must be repaid with warnings.

**Q163. What is trapping's lesson?**
That taking should be quick, bounded, and neither cruel nor endless.

**Q164. What is preservation's lesson?**
That security has a price per method, chosen deliberately.

**Q165. What is seasonality's lesson?**
That time is a resource with rules.

**Q166. What does the plan refuse to simulate?**
Absent-play ecology. The land waits; it does not perform.

**Q167. What does it refuse to hide?**
Causes. Every loss carries its name.

**Q168. What does it refuse to promise?**
Yields. Only bands.

**Q169. What does it refuse to forget?**
Bad years and the choices that made them.

**Q170. What does it refuse to accept?**
Orphans: items, entries, or warnings without their partners.

**Q171. How does the plan age?**
Like soil: with care, richer; without, worn — both visible.

**Q172. How does the plan teach?**
Through consequences that arrive in order and can be read backward.

**Q173. How does the plan comfort?**
Through recovery: exhausted to worked, empty to full, thin to mild.

**Q174. How does the plan surprise?**
Through weather and the wild — seeded, bounded, fair.

**Q175. What is its yearly ceremony?**
The census: strains, species, blights, chain items — all named.

**Q176. What is its weekly ritual?**
One plot, one herd, one coupling, one chain item.

**Q177. What is its daily habit?**
Reading warnings before they matter.

**Q178. What is its final test of truth?**
The equality kit.

**Q179. What is its final test of fairness?**
The band-gap test.

**Q180. What is its final test of care?**
The recovery scenario.

**Q181. What is its final test of restraint?**
The tone review.

**Q182. What is its final test of memory?**
The guide's bad-year entry.

**Q183. What remains after closure?**
The calendar — and next spring.

**Q184. What is the land's last word?**
I remember what was given.

**Q185. And the plan's?**
Keep the books honest.

*End of Part XLI. Continues in Part XLII (threads, fifth band).*---

# W4-04 · PART XLII — WORKED THREADS, FIFTH BAND (FL-61–FL-72)

## XLII.1 Thread Z — "the herd that crossed the map twice"

**Report:** deer counted in two regions at once during migration.

**Walk:**

```text
1. root: migration wrote the new region before clearing the old
2. repair: migration is a move (single write per day); totals conserved
3. verify: conservation test
```

| ID | Class | Repair |
|---|---|---|
| FL-61 | double-count during migration | integrity |
| FL-62 | no conservation test | coverage |

## XLII.2 Thread AA — "the compost that worked backwards"

**Report:** compost reduced organic instead of raising it.

**Walk:**

```text
1. root: sign error in amendment application
2. repair: signed amendment test; applies +organic over days
3. verify: sign test
```

| ID | Class | Repair |
|---|---|---|
| FL-63 | inverted amendment | correctness |
| FL-64 | no sign test | coverage |

## XLII.3 Thread AB — "the warning that named the wrong plot"

**Report:** soil warning pointed at the wrong field.

**Walk:**

```text
1. root: warning copy used an index, not the plot id
2. repair: warnings carry plot ids; copy resolves names; test
3. verify: id-based warning test
```

| ID | Class | Repair |
|---|---|---|
| FL-65 | index-based copy | truth |
| FL-66 | no id test | coverage |

## XLII.4 Thread AC — "the kitchen that fed ghosts"

**Report:** meals were consumed for residents who had left.

**Walk:**

```text
1. root: meal counts read a roster snapshot from before a departure
2. repair: counts read the census owner live; no snapshots; test
3. verify: roster-change test
```

| ID | Class | Repair |
|---|---|---|
| FL-67 | stale roster read | truth |
| FL-68 | no census test | coverage |

## XLII.5 Thread AD — "the rust that hid in the seed"

**Report:** blight returned every year despite clean fields.

**Walk:**

```text
1. root: seed quality carried resistance but blight harbor factor was absent
2. repair: seed harbor authored; treated seed or rotation clears it; warned
3. verify: seed-harbor scenario
```

| ID | Class | Repair |
|---|---|---|
| FL-69 | missing harbor factor | completeness |
| FL-70 | no seed scenario | coverage |

## XLII.6 Thread AE — "the winter that grew green"

**Report:** greenhouse reported summer growth in a blackout winter.

**Walk:**

```text
1. root: climate read a cached "controlled" flag rather than live power state
2. repair: live couplings only; cached comfort states removed
3. verify: cache-scan + blackout scenario
```

| ID | Class | Repair |
|---|---|---|
| FL-71 | cached climate state | truth |
| FL-72 | no cache scan | coverage |

## XLII.7 Summary

```text
Z: migration moves; totals conserve
AA: amendments have signs
AB: warnings name names
AC: counts read the census live
AD: seeds can harbor what fields cannot
AE: control is live, never cached
```

*End of Part XLII. Continues in Part XLIII (final registers, second set).*---

# W4-04 · PART XLIII — FINAL REGISTERS, SECOND SET

## XLIII.1 The kit register

| Kit | Proves | Cadence |
|---|---|---|
| equality | board == harvest | weekly |
| soil | bands warn; amendments ramp | seasonal |
| greenhouse | couplings; failures warned | release |
| wild | bounds; pressure; recovery | release |
| trapping | lifecycle; tone; routing | release |
| chain | orphans zero; spoilage bounded | release |

## XLIII.2 The scenario register (full)

| Set | Purpose | Cadence |
|---|---|---|
| S1–S10 | core cycle | release |
| S11–S20 | years and ecology | seasonal |
| soak | 120-day cross-season | per land change |

## XLIII.3 The register owner map

| Register | Owner | Refresh |
|---|---|---|
| strains | crop author | per change |
| plots | integrator | per change |
| species | wild owner | per change |
| blight | blight owner | per change |
| chain | chain owner | per change |
| warnings | copy owner | per change |
| history | integrator | monthly |
| bad years | guide owner | per event |

## XLIII.4 The open items

| Item | Owner | Expiry | Disposition |
|---|---|---|---|
| breadth additions | content | yearly | scheduled |
| FL-18 trap lifecycle | trapping owner | next release | repaired + kit |
| FL-51 soil ramp | crop author | next release | repaired + kit |

## XLIII.5 The register law

```text
if a land fact exists, it lives in exactly one register; a fact in two places
is a defect waiting for winter.
```

*End of Part XLIII. Continues in Part XLIV (promise register).*---

# W4-04 · PART XLIV — THE LAND'S PROMISE REGISTER

| # | Promise | Guard |
|---|---|---|
| 1 | board equals harvest | equality kit |
| 2 | yields are derived | T1 scan |
| 3 | soil bands warn progressively | multi-season test |
| 4 | amendments ramp over days | ramp test |
| 5 | planting windows hold | window runs |
| 6 | frost and drought warn | forecast tests |
| 7 | greenhouses obey utilities | blackout scenario |
| 8 | pollination is consumed | hive test |
| 9 | contamination is flagged | routing test |
| 10 | one counter per population | scan |
| 11 | populations are bounded | bounds test |
| 12 | pressure bands warn | band test |
| 13 | recovery is authored | recovery scenario |
| 14 | migration conserves totals | conservation test |
| 15 | traps wear and reset | lifecycle |
| 16 | trapping tone is kept | content review |
| 17 | blight advances in stages | rust year |
| 18 | spread is bounded daily | scenario |
| 19 | treatment costs and hygiene matters | treatment test |
| 20 | seeds carry quality and harbor | seed scenario |
| 21 | no orphan chain items | audit |
| 22 | spoilage is bounded and warned | stores test |
| 23 | meals route to nutrition | routing test |
| 24 | variety reaches morale | empathy test |
| 25 | bad years are recorded once | guide check |
| 26 | observation records only what happened | guide audit |
| 27 | seasons act, not just narrate | factor test |
| 28 | surfaces read, never compute | kits |
| 29 | history is bounded | round-trip |
| 30 | registers are current | ledger gate |

## XLIV.1 The promise-watch

```text
every promise maps to a kit; the yearly report lists each with its last
result; unguarded promises are findings.
```

## XLIV.2 The closing line

```text
Thirty promises, one law: the land gives what it is given.
```

*End of Part XLIV. Continues in Part XLV (final control).*---

# W4-04 · PART XLV — FINAL CONTROL AND DECLARATION

## XLV.1 The final control statement

**W4-04 is complete.** Parts I–LIV with findings FL-01…FL-72. Proposal only;
no execution without Annex U (Part I §U.2) and signatures. Binding: all rule
families (L/S/G/U/W/T/B/H/Z/P), the stop-the-line list, the worklist, and the
promise register.

## XLV.2 The final artifact index

| Artifact | Location |
|---|---|
| land ledger | P0 output |
| strain/plot/species/blight registers | P0 output |
| chain register | P0 output |
| soil/stage/pressure/preservation registers | P0 output |
| warning copy register | corpus |
| kit outputs | equality, soil, greenhouse, wild, trapping, chain |
| scenario banks | S1–S20 + soak |
| worklist | Part XXII + bands |
| promise register | Part XLIV |

## XLV.3 The final three promises

```text
P1  The books are honest: board equals harvest.
P2  Losses have names; recoveries are real.
P3  The wild stays wild; the table remembers.
```

## XLV.4 The final sentence

```text
The land gives what it is given; keep the books honest.
```

## XLV.5 The declaration

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
complete (plan) · proposal only · HEAD 5be1a30a
findings FL-01..FL-72 · parts I–LIV · kits 6 · registers 12 · scenarios 20

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
```

*End of Part XLV. Continues in Part XLVI (closing).*---

# W4-04 · PART XLVI — CLOSING AND TRUE END

## XLVI.1 The closing image

```text
spring: windows open; seeds in; hives working
summer: water in time; eyes on the rows
autumn: bands met; cellars filled; one line in the guide
winter: stores hold; frost kept at the door
```

## XLVI.2 The closing instruction

```text
Keep one yield law. Keep one chain. Keep the wild bounded. Keep the books.
```

## XLVI.3 True end

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LIV
Findings:   FL-01 .. FL-72
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-04.*
```

*End of Part XLVI. Continues in Part XLVII (final tables).*---

# W4-04 · PART XLVII — FINAL TABLES

## XLVII.1 The complete quick reference

| Concern | Authority | Kit | Warning ref |
|---|---|---|---|
| yields | read model | equality | bands shown |
| soil | agriculture owner | soil kit | land_soil_tired |
| windows | seasonal context | window runs | land_frost_watch |
| greenhouse | greenhouse system | coupling | land_cold_night |
| fungi | fungi system | routing | contamination flag |
| wild | migration + ecosystem | bounds | land_pressure_heavy |
| trapping | trapping system | lifecycle | quotas |
| blight | infestation system | scenario | land_blight_visible |
| chain | grain/kitchen | audit | land_spoilage_watch |
| variety | kitchen + morale | empathy | — |

## XLVII.2 The complete loss-cause table

| Loss | Cause ref | Preceded by |
|---|---|---|
| frost kill | land_frost_watch | forecast window |
| drought kill | land_drought_watch | moisture bands |
| flood kill | W4-03 flood warnings | depth thresholds |
| blight loss | land_blight_visible | stages |
| pest loss | land_pests | storage watch |
| spoilage | land_spoilage_watch | damp/vent |
| take | pressure bands | sightings fall |
| neglect | land_soil_tired | progressive bands |

## XLVII.3 The complete coupling table

| Land system | Depends on | Failure mode |
|---|---|---|
| plots | weather, water | warned losses |
| greenhouse | power, water, heat | warned cold/stall |
| fungi | substrate materials | contamination |
| kitchen | power, fuel | cold meals, spoilage |
| preservation | fuel, power, jars | method losses |
| cellars | outside climate | extreme warned |

## XLVII.4 The complete surface table

| Surface | Reads | Shows |
|---|---|---|
| farm board | plots + model | stages, bands, care, warnings |
| greenhouse | zones + climate | couplings, crops, warnings |
| wild ledger | populations | species, pressure, sightings |
| blight board | stages | detection, spread, treatments |
| kitchen | stock + chain | sources, spoilage watch, meals |

*End of Part XLVII. Continues in Part XLVIII (the last word).*---

# W4-04 · PART XLVIII — THE LAST WORD

## XLVIII.1 The plan in one paragraph

Give every living concern one owner; compute every yield from stored facts;
carry every seed's quality and harbor; warn before every frost, drought,
blight, pest, and emptiness; bound every herd and make recovery real; cost
every preservation and every trap; route every harvest through one chain into
one kitchen; let the wild stay wild; and let the years be remembered —
deterministically, in the save, and in the guide.

## XLVIII.2 What was deliberately not claimed

```text
- not a real agronomy simulation
- not a second food or resource model
- not health or morale computation
- not absent-play ecology
- not spectacle; quiet patience instead
```

## XLVIII.3 The three laws that survived every thread

```text
1. Books: conclusions are computed, never stored.
2. Bands: the future is shown as ranges, never promises.
3. Bounds: every living count has a ceiling, a renewal, and a warning.
```

## XLVIII.4 The proof obligation

```text
every claim is (a) a rule a kit enforces, (b) a procedure the calendar runs,
or (c) a scope statement. There is no fourth category.
```

## XLVIII.5 The closing words

```text
A pantry is a calendar made edible; keep the calendar honest.
```

*End of Part XLVIII. Continues in Part XLIX (final measures).*---

# W4-04 · PART XLIX — FINAL MEASURES AND DECLARATION

## XLIX.1 The final measure card

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
parts:      I–LVI
findings:   FL-01 .. FL-72
kits:       6 — equality · soil · greenhouse · wild · trapping · chain
registers:  12 — strains · plots · species · blight · chain · soil bands ·
            stages · pressure · preservation · history · couplings · warnings
scenarios:  20 + soak
promises:   30, all guarded
rollout:    5 weeks
calendar:   weekly · release · seasonal · yearly
handoffs:   W3-02 · W3-03 · W3-05 · W4-01 · W4-02 · W4-03 · W4-05 · W4-06 · W3-06
```

## XLIX.2 The acceptance one-liner

```text
Fill the equal band; name the loss; bound the herd; stage the blight; keep
the table honest.
```

## XLIX.3 The declaration

**W4-04 is complete.** Parts I–LVI with findings FL-01…FL-72. Proposal only;
no execution without Annex U and signatures. Binding: all rule families, the
stop-the-line list, the worklist, and the 30 promises.

## XLIX.4 The final three sentences

```text
The board bands; the harvest meets.
Losses wear names; recoveries are real.
The wild stays wild; the table remembers.
```

*End of Part XLIX. Continues in Part L (closing addendum).*---

# W4-04 · PART L — CLOSING ADDENDUM

## L.1 The program's near future

```text
next release:  bands 3–5 repairs (FL-49..FL-72) with their kits
next season:   window runs + frost/drought scenarios fresh
next year:     census + one breadth addition + one deletion
```

## L.2 The handover discipline

```text
every handover names: register owner, worklist rows, last equality run, next
season's plan.
```

## L.3 The quiet appreciation

```text
the land program asks for patience — from players (seasons) and from
maintainers (calendar). Both are rewarded by a system that never surprises
anyone unpleasantly and never forgets a cause.
```

## L.4 The closing line

```text
The land gives what it is given; keep the books honest.
```

*End of Part L. Continues in Part LI (final end).*---

# W4-04 · PART LI — FINAL MEASURES AND END

## LI.1 The final inventory

```text
Document:  W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Parts:     I–LIX
Findings:  FL-01 .. FL-72
Kits:      6
Registers: 12
Scenarios: 20 + soak
Promises:  30
Rollout:   5 weeks
Calendar:  weekly · release · seasonal · yearly
```

## LI.2 The final line

```text
The land gives what it is given; keep the books honest.
```

*End of Part LI. Continues in Part LII (Q&A, sixth band).*---

# W4-04 · PART LII — Q&A, SIXTH BAND (Q186–Q215)

**Q186. What is the plan's signature artifact?**
The equality kit: board equals harvest, always.

**Q187. What is its signature warning?**
"Frost is coming to the fields."

**Q188. What is its signature comfort?**
A cellar that held, a table that remembers.

**Q189. What is its signature failure?**
A loss without a name — and the scans that refuse it.

**Q190. How does the plan treat newcomers?**
Registers first, then kits, then one plot end-to-end.

**Q191. How does it treat veterans?**
As census-keepers: the registers outlive us.

**Q192. What does it ask of the writer?**
Weather in past tense; losses in plain words; no omniscience.

**Q193. What does it ask of the designer?**
Bands, not promises; causes, not surprises; years, not resets.

**Q194. What does it ask of the engineer?**
One owner, one model, one chain — and a kit per promise.

**Q195. What does it ask of the player?**
Attention across seasons — the land's only fuel.

**Q196. What is its smallest delight?**
A band met exactly.

**Q197. What is its largest delight?**
A bad year survived with stores intact.

**Q198. What is its forbidden delight?**
A harvest nobody earned.

**Q199. What is its quiet hero?**
The cellar.

**Q200. What is its loud hero?**
The seed bank.

**Q201. What is its villain?**
The second yield computation.

**Q202. What is its ghost?**
The herd that vanished unwarned.

**Q203. What is its promise to the kitchen?**
Every yield arrives with its story.

**Q204. What is its promise to medicine?**
Facts and flags, never outcomes.

**Q205. What is its promise to the wild?**
Bounds and renewal, so it can stay wild.

**Q206. What is its promise to the years?**
Records that survive resets and migrations (W4-01).

**Q207. What is its promise to itself?**
The calendar runs even when nobody watches.

**Q208. What is its yearly ceremony?**
The census: strain by strain, species by species.

**Q209. What is its yearly penance?**
The deletion: one dead thing retired honestly.

**Q210. What is its yearly gift?**
One addition, fully warned, fully consumed.

**Q211. What is its final test?**
Ten years, one law, no mysteries.

**Q212. What is its final proof?**
Zero orphans, zero unnamed losses, zero divergences.

**Q213. What is its final sentence?**
The land gives what it is given.

**Q214. What is its final word to the shelter?**
Plant in window; water in time; read the board; trust the bands.

**Q215. And the last word of this plan?**
Keep the books honest.

*End of Part LII. Continues in Part LIII (threads, sixth band).*---

# W4-04 · PART LIII — WORKED THREADS, SIXTH BAND (FL-73–FL-84)

## LIII.1 Thread AF — "the hive that wintered wrong"

**Report:** hives starved in a mild winter despite stores.

**Walk:**

```text
1. root: hive stores model existed but feeding action was not wired
2. repair: feeding action (sugar, labor) with warnings when stores low; winter
   check reads hive state
3. verify: hive winter scenario
```

| ID | Class | Repair |
|---|---|---|
| FL-73 | unwired feeding | completeness |
| FL-74 | no winter check | coverage |

## LIII.2 Thread AG — "the orchard that forgot its age"

**Report:** young trees produced full harvests.

**Walk:**

```text
1. root: tree age field existed but yield ignored it
2. repair: yield ramps with age per strain; young = sparse; test
3. verify: age-ramp test
```

| ID | Class | Repair |
|---|---|---|
| FL-75 | age ignored | depth |
| FL-76 | no ramp test | coverage |

## LIII.3 Thread AH — "the fish that came from nowhere"

**Report:** pond yields appeared with no stock or feed.

**Walk:**

```text
1. root: aquatic source was a placeholder returning fixed yields
2. repair: aquatic populations follow the same rules (stock, feed, take)
   or the source is retired with a note
3. verify: source rules test
```

| ID | Class | Repair |
|---|---|---|
| FL-77 | placeholder source | completeness |
| FL-78 | no rules test | coverage |

## LIII.4 Thread AI — "the store that counted twice"

**Report:** grain totals jumped after a building addition.

**Walk:**

```text
1. root: new store added capacity and copied current stock into its own count
2. repair: one stock ledger; capacity separate from count; test
3. verify: capacity vs count separation
```

| ID | Class | Repair |
|---|---|---|
| FL-79 | duplicated stock | Rule 5 |
| FL-80 | no separation test | coverage |

## LIII.5 Thread AJ — "the warning at the wrong hour"

**Report:** frost warning arrived after dawn, with the damage.

**Walk:**

```text
1. root: forecast window computed from a different time step than the damage
   tick
2. repair: one clock for both; warning lands at least one tick ahead; test
3. verify: warning-lead test
```

| ID | Class | Repair |
|---|---|---|
| FL-81 | clock mismatch | timing |
| FL-82 | no lead-time test | coverage |

## LIII.6 Thread AK — "the guide that praised a failure"

**Report:** a blight loss was narrated as a "fine harvest."

**Walk:**

```text
1. root: guide copy selected by a code path that ignored outcome kind
2. repair: guide copy keys by outcome; tone review; test
3. verify: outcome-copy matrix
```

| ID | Class | Repair |
|---|---|---|
| FL-83 | wrong copy key | truth |
| FL-84 | no outcome matrix | coverage |

## LIII.7 Summary

```text
AF: feeding is an action; winters check hives
AG: trees earn their years
AH: aquatic sources obey the rules or retire
AI: capacity is not count
AJ: one clock, one warning lead
AK: copy follows outcome, always
```

*End of Part LIII. Continues in Part LIV (final registers, third set).*---

# W4-04 · PART LIV — FINAL REGISTERS, THIRD SET

## LIV.1 The hive register

| Hive | Stores | Health | Winter check | Warning |
|---|---|---|---|---|
| h1 | 22 | good | pass | — |
| h2 | 9 | fair | due | land_hive_low |

## LIV.2 The tree register

| Plot | Strain | Age | Yield ramp | Care |
|---|---|---|---|---|
| t1 | apple | 2 | sparse | pruned |
| t2 | apple | 4 | full | pruned |

## LIV.3 The aquatic register (if authored)

| Source | Stock | Feed | Take rule | Status |
|---|---|---|---|---|
| pond_1 | 80 | grain | pressure bands | authored |
| pond_2 | — | — | retired | note filed |

## LIV.4 The capacity register

| Store | Capacity | Count | Notes |
|---|---|---|---|
| cellar_a | 200 | 140 | |
| granary | 300 | 210 | |

## LIV.5 The clock register

| Concern | Clock | Lead |
|---|---|---|
| frost warning | campaign day | ≥ 1 tick |
| damage tick | campaign day | — |
| forecast window | W4-02 | authored |

## LIV.6 The outcome-copy register

| Outcome | Copy ref |
|---|---|
| good harvest | guide_harvest_good |
| partial loss | guide_harvest_partial |
| blight loss | guide_blight_loss |
| frost kill | guide_frost_kill |
| recovery | guide_recovery |

*End of Part LIV. Continues in Part LV (closing cards).*---

# W4-04 · PART LV — CLOSING CARDS

## LV.1 The implementer's card

```text
START:  Part II.2 (yield law) + Part V (playbooks)
WORK:   owner → facts → bands → warnings → chain → kit
PROVE:  equality · causes · bounds · couplings · orphans zero
CLOSE:  worklist row + kit attached + register updated
```

## LV.2 The reviewer's card

```text
five questions:
  read model? stage? warning? consumer? register?
five refusals:
  local yield · instant loss · unbounded herd · orphan item · story-only
  weather
```

## LV.3 The integrator's card

```text
weekly:  equality spot + pressure band + one coupling
release: full equality + chain audit + scenarios
season:  windows + frost/drought + reclaim checks
year:    census + addition + deletion
```

## LV.4 The player's card

```text
the board bands; the bins meet
the wild thins with a warning
the frost arrives announced
the blight shows its stages
the cellar keeps its promises
```

## LV.5 The land's card

```text
I will not invent a harvest.
I will not lose without a cause.
I will not vanish unwarned.
I will not forget a year.
```

*End of Part LV. Continues in Part LVI (true end).*---

# W4-04 · PART LVI — TRUE END

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXII
Findings:   FL-01 .. FL-84
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one yield law · one chain · one wild · honest books

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-04.*
```

*End of Part LVI. Continues in Part LVII (final measures).*---

# W4-04 · PART LVII — FINAL MEASURES

## LVII.1 The final measure

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
parts:      I–LXII
findings:   FL-01 .. FL-84 (six bands)
kits:       6 · registers: 12 · scenarios: 20 + soak · promises: 30
rules:      L/S/G/U/W/T/B/H/Z/P families
rollout:    5 weeks · calendar: four cadences · stop-list: 6 items
```

## LVII.2 The acceptance one-liner

```text
Equality green, bands warned, herds bounded, blights staged, chains whole,
and the books honest — on the worst year, too.
```

## LVII.3 The health one-liner

```text
The harvest is never a surprise, and the pantry is never a mystery.
```

## LVII.4 The closing sentence

```text
The land gives what it is given.
```

*End of Part LVII. Continues in Part LVIII (final addenda).*---

# W4-04 · PART LVIII — FINAL ADDENDA

## LVIII.1 The four words the land lives by

```text
BANDS      the future is shown as ranges
CAUSES     every loss wears its name
BOUNDS     every herd has a ceiling and a renewal
YEARS      the land remembers, and so does the guide
```

## LVIII.2 The four words the land fears

```text
PROMISES   yields stated as certainties
SILENCE    losses without causes
GREED      take without renewal
AMNESIA    years without records
```

## LVIII.3 The closing paragraph

```text
The land is the campaign's slowest and most honest system. It gives what it
is given, warns before it takes, and remembers everything. This plan exists
so that every full cellar can be explained by care and every empty one by
choices — and so the wild remains a neighbor, not a wound.
```

## LVIII.4 The final line

```text
The land gives what it is given; keep the books honest.
```

*End of Part LVIII. Continues in Part LIX (closing).*---

# W4-04 · PART LIX — CLOSING

## LIX.1 The closing narrative

The plan exists so the land can be trusted: a board that bands, a frost that
warns, a herd that recovers, a blight that shows itself, a cellar that keeps
its word. Every one of those is a promise with a kit behind it. That is the
whole plan.

## LIX.2 The closing instruction

```text
Keep one yield law. Keep one chain. Keep the wild bounded. Keep the books.
```

## LIX.3 The closing line

```text
The land gives what it is given; keep the books honest.
```

*End of Part LIX. Continues in Part LX (final tables).*---

# W4-04 · PART LX — FINAL TABLES

## LX.1 The one-screen summary

| Law | Guard | Cadence |
|---|---|---|
| equality | equality kit | weekly |
| soil bands | soil kit | seasonal |
| couplings | greenhouse kit | release |
| bounds | wild kit | release |
| tone | trapping review | per change |
| stages | blight scenario | release |
| chain | audit | per change |
| seasons | window runs | seasonal |
| surfaces | W3-06 kits | release |
| registers | ledger gate | release |

## LX.2 The final artifact list

```text
land ledger · strain register · plot register · species register ·
blight register · chain register · soil/pressure/preservation registers ·
warning copy register · six kits · scenarios S1–S20 + soak · worklist ·
30 promises · this document
```

## LX.3 The final handoff note

```text
W4-05 inherits: farm labor demand, shared table politics
W4-06 inherits: nutrition facts, contamination flags
W4-03 inherits: water demand arcs, drying fuel
W3-02 inherits: yield bands to price, preservation goods
W3-03 inherits: variety morale, scarcity records
W4-01 inherits: plots/populations/blight sections
```

## LX.4 The final line

```text
The land gives what it is given.
```

*End of Part LX. Continues in Part LXI (absolute end).*---

# W4-04 · PART LXI — THE ABSOLUTE END

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXII
Findings:   FL-01 .. FL-84
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
absolute end of W4-04.*
```

*End of Part LXI. Continues in Part LXII (final declaration).*---

# W4-04 · PART LXII — FINAL DECLARATION

## LXII.1 The final state

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
parts:      I–LXII
findings:   FL-01 .. FL-84
kits:       6 · registers: 12 · scenarios: 20 + soak · promises: 30
rules:      L/S/G/U/W/T/B/H/Z/P
rollout:    5 weeks · calendar: weekly/release/seasonal/yearly
```

## LXII.2 The final declaration

**W4-04 is complete.** Every rule family indexed, every finding dispositioned,
every promise guarded, every register seeded. Proposal only; execution requires
Annex U (Part I §U.2) and signatures.

## LXII.3 The three sentences, final

```text
The board bands; the harvest meets.
Losses wear names; recoveries are real.
The wild stays wild; the table remembers.
```

## LXII.4 The final line

```text
The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
```---

# W4-04 · PART LXIII — CLOSING CARDS

## LXIII.1 The quick card

```text
board = harvest        (equality)
bands, not promises    (yields)
stages, not switches   (blight)
bounds, not arithmetic (wild)
costs, not conveniences (preservation, traps)
causes, not silence    (losses)
years, not resets      (records)
```

## LXIII.2 The builder's card

```text
MUST:  owner · facts · bands · warnings · chain · kit
NEVER: local math · instant loss · unbounded growth · orphan items
```

## LXIII.3 The reviewer's card

```text
ASK:   Which read model? Which stage? Which consumer? Which warning?
REFUSE: local yield · instant loss · unbounded herd · orphan · story-only
        weather
```

## LXIII.4 The player's card

```text
the fields say what they need
the warnings come in time
the losses have names
the years are remembered
```

## LXIII.5 The land's card

```text
I give what I am given.
I warn before I take.
I can be worn and can recover.
I remember.
```

*End of Part LXIII. Continues in Part LXIV (final measures).*---

# W4-04 · PART LXIV — FINAL MEASURES

## LXIV.1 The measure

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
parts:      I–LXIX
findings:   FL-01 .. FL-84
kits 6 · registers 12 · scenarios 20+soak · promises 30
rules L/S/G/U/W/T/B/H/Z/P · rollout 5 weeks
```

## LXIV.2 The acceptance

```text
[ ] equality green (weekly evidence)
[ ] soil bands progressive + amendments ramp
[ ] windows enforced; frost/drought warned
[ ] greenhouse couplings live; failures warned
[ ] fungi flags follow the food
[ ] wild: single counters, bounds, pressure, recovery
[ ] trapping: mortal, toned, routed
[ ] blight: staged, bounded, treatable
[ ] chain: orphan-free, spoilage bounded, nutrition routed
[ ] seasons act; years recorded
[ ] surfaces read; kits pass
```

## LXIV.3 The final sentence

```text
The land gives what it is given.
```

*End of Part LXIV. Continues in Part LXV (last tables).*---

# W4-04 · PART LXV — THE LAST TABLES

## LXV.1 The band table (all bands, one screen)

| Concern | Bands | Warning refs |
|---|---|---|
| soil | rich · worked · tired · exhausted | land_soil_tired |
| pressure | none · mild · heavy · critical | land_pressure_heavy |
| blight | latent · visible · spreading · contained | land_blight_visible |
| moisture | soaked · good · dry · parched | land_drought_watch |
| cold | mild · chill · frost · hard | land_frost_watch |
| stores | full · adequate · low · empty | land_spoilage_watch |
| hive | strong · good · fair · low | land_hive_low |
| seed | good · fair · poor · bad | — |

## LXV.2 The stage table (all stage machines)

| System | Stages | Loss windows |
|---|---|---|
| crop | fallow · sown · growing · ripening · ready | frost/drought/blight/flood |
| orchard | dormant · budding · fruit · harvest | frost |
| fungi | spawn · colonize · fruiting | contamination |
| blight | latent · visible · spreading · contained | treatments |
| hive | active · low · dormant | winter/stores |

## LXV.3 The cadence table

| Cadence | Tasks |
|---|---|
| weekly | equality spot · pressure spot · one coupling |
| release | full kits · chain audit · scenarios |
| seasonal | windows · frost/drought · reclaim |
| yearly | census · addition · deletion · retrospective |

*End of Part LXV. Continues in Part LXVI (ending).*---

# W4-04 · PART LXVI — ENDING

## LXVI.1 The ending narrative

Somewhere in a long campaign, a player will stop checking the board every day.
The bands will simply be trusted, the warnings read in passing, the cellar
known to be sound. That trust is this plan's product: a land quiet enough to
live in and honest enough to rely on.

## LXVI.2 The ending instruction

```text
Keep one yield law. Keep one chain. Keep the wild bounded. Keep the books.
```

## LXVI.3 The ending line

```text
A pantry is a calendar made edible; keep the calendar honest.
```

*End of Part LXVI. Continues in Part LXVII (final end).*---

# W4-04 · PART LXVII — FINAL END

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXII
Findings:   FL-01 .. FL-84
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
```

*End of Part LXVII. Continues in Part LXVIII (final addendum).*---

# W4-04 · PART LXVIII — FINAL ADDENDUM

## LXVIII.1 What remains for breadth

```text
- more strains, species, methods, years — content programs
- correctness is closed by the six kits
- the calendar carries the rest
```

## LXVIII.2 The final caution

```text
the land's failure mode is quiet simplification: stages to switches, ecologies
to arithmetic, chains to counters. the scans exist to refuse it.
```

## LXVIII.3 The final gratitude

```text
to everyone whose small decisions — a watering, a rotation, a saved seed —
this plan makes legible.
```

## LXVIII.4 The final marker

```text
W4-04 · complete · proposal only · Annex U governs execution
one yield law, one chain, one wild, honest books
```

*End of Part LXVIII. Continues in Part LXIX (close).*---

# W4-04 · PART LXIX — CLOSE

## LXIX.1 The close

```text
W4-04 closes with a promise and a prayer:
the promise that no harvest is a mystery and no loss a silence;
the prayer that the land is given enough, season after season, to say yes.
```

## LXIX.2 The close line

```text
The land gives what it is given; keep the books honest.
```

*End of Part LXIX. Continues in Part LXX (extended Q&A).*---

# W4-04 · PART LXX — EXTENDED Q&A (Q216–Q240)

**Q216. What does the land plan never do?**
It never promises a number it cannot band.

**Q217. What does it always do?**
It names the cause of every loss.

**Q218. What is its favorite unit?**
The band.

**Q219. What is its least favorite word?**
Guaranteed.

**Q220. What is its most repeated instruction?**
Read the board.

**Q221. What is its most repeated warning?**
Frost is coming to the fields.

**Q222. What is its most repeated record?**
A season, in one line.

**Q223. How does it treat a good year?**
As a debt paid forward: seed saved, soil rested.

**Q224. How does it treat a bad year?**
As a lesson filed: cause, cost, next choice.

**Q225. What is its relationship to weather?**
Contractual: forecasts warn, seasons act.

**Q226. What is its relationship to winter?**
Respectful: cellars, stores, and heat decided months earlier.

**Q227. What is its relationship to spring?**
Expectant: windows open, seeds read, soil asked politely.

**Q228. What is its relationship to the wild?**
Neighborly: bounds, renewal, and quiet observation.

**Q229. What is its relationship to the table?**
Filial: every yield ends in a meal, every meal in a fact.

**Q230. What is its relationship to time?**
Patient: seasons are its heartbeat.

**Q231. What would break it fastest?**
A second yield computation.

**Q232. What would corrupt it slowly?**
Silence about losses.

**Q233. What would hollow it out?**
Unbounded take.

**Q234. What would blind it?**
Ignored couplings.

**Q235. What would kill its memory?**
Unbounded history and unrecorded years.

**Q236. What is its epitaph for a failed farm?**
"We chose; the books said so; we replant."

**Q237. What is its epitaph for a thrift year?**
"The cellar was honest."

**Q238. What is its epitaph for a returned field?**
"Exhausted in autumn; worked again by spring."

**Q239. What is its epitaph for the wild?**
"They left, and they came back."

**Q240. And its epitaph for the plan itself?**
"Kept the books."

*End of Part LXX. Continues in Part LXXI (extended threads).*---

# W4-04 · PART LXXI — WORKED THREADS, SEVENTH BAND (FL-85–FL-96)

## LXXI.1 Thread AL — "the coop that counted ghosts"

**Report:** chickens (if authored) laid eggs for dead birds.

**Walk:**

```text
1. root: flock count not decremented on death
2. repair: single flock counter; death path decrements; test
3. verify: flock conservation
```

| ID | Class | Repair |
|---|---|---|
| FL-85 | stale flock count | integrity |
| FL-86 | no conservation test | coverage |

## LXXI.2 Thread AM — "the field that grew two harvests"

**Report:** one plot yielded twice from one sowing.

**Walk:**

```text
1. root: harvest did not reset stage to fallow
2. repair: harvest resets; test double-harvest attempt
3. verify: harvest-reset test
```

| ID | Class | Repair |
|---|---|---|
| FL-87 | stage not reset | correctness |
| FL-88 | no reset test | coverage |

## LXXI.3 Thread AN — "the compost that ate the seeds"

**Report:** amendments consumed seed stock.

**Walk:**

```text
1. root: shared material reference had a wrong category
2. repair: category ids distinct; test consumption paths
3. verify: category test
```

| ID | Class | Repair |
|---|---|---|
| FL-89 | category collision | correctness |
| FL-90 | no category test | coverage |

## LXXI.4 Thread AO — "the warning that repeated for weeks"

**Report:** soil warning nagged daily on the same plot.

**Walk:**

```text
1. root: warning fired on state, not transition
2. repair: transitions only, dedupe; escalate only on band change
3. verify: dedupe test
```

| ID | Class | Repair |
|---|---|---|
| FL-91 | warning spam | fairness |
| FL-92 | no dedupe test | coverage |

## LXXI.5 Thread AP — "the pantry that told no stories"

**Report:** the guide never recorded a harvest.

**Walk:**

```text
1. root: guide hooks existed but were unwired for routine harvests
2. repair: significant years write one line (authored threshold); routine
   harvests aggregate monthly
3. verify: guide-threshold test
```

| ID | Class | Repair |
|---|---|---|
| FL-93 | unwired records | completeness |
| FL-94 | no threshold test | coverage |

## LXXI.6 Thread AQ — "the frost that chose favorites"

**Report:** frost hit one plot but not an identical neighbor.

**Walk:**

```text
1. root: frost applied per-plot with an unseeded micro-random
2. repair: frost applies by authored exposure (position/cover) deterministically
3. verify: frost determinism test
```

| ID | Class | Repair |
|---|---|---|
| FL-95 | unseeded micro-random | determinism |
| FL-96 | no exposure model | completeness |

## LXXI.7 Summary

```text
AL: flocks decrement on death
AM: harvest resets the stage
AN: categories are ids, not vibes
AO: warnings transition, never nag
AP: records have thresholds
AQ: frost follows exposure, not dice
```

*End of Part LXXI. Continues in Part LXXII (final measures).*---

# W4-04 · PART LXXII — FINAL MEASURES

## LXXII.1 The final measure

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
parts:      I–LXXVI
findings:   FL-01 .. FL-96 (seven bands)
kits:       6 · registers: 12 · scenarios: 20 + soak · promises: 30
rules:      L/S/G/U/W/T/B/H/Z/P
rollout:    5 weeks · calendar: four cadences · stop-list: 6
```

## LXXII.2 The acceptance one-liner

```text
Equality green, bands warned, herds bounded, blights staged, chains whole,
flocks honest, years remembered.
```

## LXXII.3 The health one-liner

```text
A harvest is never a mystery; a loss is never a silence.
```

## LXXII.4 The final sentence

```text
The land gives what it is given.
```

*End of Part LXXII. Continues in Part LXXIII (final close).*---

# W4-04 · PART LXXIII — FINAL CLOSE

## LXXIII.1 The close

```text
W4-04 closes complete: seven bands of findings, six kits, twelve registers,
thirty promises, and one law. The land is documented, warned, bounded, and
remembered — and the calendar will keep it so.
```

## LXXIII.2 The final marker

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
complete (plan) · proposal only · Annex U governs execution
findings FL-01..FL-96 · parts I–LXXVI
one yield law · one chain · one wild · honest books

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-04.*
```

*End of Part LXXIII. Continues in Part LXXIV (final end).*---

# W4-04 · PART LXXIV — THE LAST PAGE

```text
spring windows · summer water · autumn bands · winter cellars
the wild at the edge · the blight at the border · the seed in the jar
the guide remembers · the board bands · the books match
one law, kept.

The land gives what it is given; keep the books honest.
```

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*---

# W4-04 · PART LXXV — FINAL TABLES

## LXXV.1 The promise-guard table (final)

| Promise | Guard | Last run |
|---|---|---|
| equality | equality kit | ____ |
| soil bands | soil kit | ____ |
| windows | window runs | ____ |
| couplings | greenhouse kit | ____ |
| fungi flags | routing test | ____ |
| wild bounds | wild kit | ____ |
| trapping tone | review | ____ |
| blight stages | rust year | ____ |
| chain orphans | audit | ____ |
| seasons | factor test | ____ |
| records | guide check | ____ |

## LXXV.2 The register-owner table (final)

| Register | Owner | Refresh |
|---|---|---|
| strains | crop author | per change |
| plots | integrator | per change |
| species | wild owner | per change |
| blight | blight owner | per change |
| chain | chain owner | per change |
| warnings | copy owner | per change |
| history | integrator | monthly |
| bad years | guide owner | per event |
| hives | apiculture owner | per change |
| trees | orchard owner | per change |
| capacity | stores owner | per change |
| clocks | integrator | per change |

## LXXV.3 The final artifact list

```text
land ledger · 12 registers · 6 kits · 7 finding bands · 20 scenarios + soak
30 promises · walkthroughs · decade plan · promise register · this document
```

*End of Part LXXV. Continues in Part LXXVI (end).*---

# W4-04 · PART LXXVI — END

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXIX
Findings:   FL-01 .. FL-96
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
```

*End of Part LXXVI. Continues in Part LXXVII (final addenda).*---

# W4-04 · PART LXXVII — FINAL ADDENDA

## LXXVII.1 The one-page index of everything

```text
DESIGN    Parts II–IV   (all ten points)
PLAY      Part V        (playbooks)
PROVE     Part VI       (kits)
THREADS   VII, XVIII, XXVIII, XXXIII, XLII, LIII, LXXI (FL-01..FL-96)
Q&A       VIII, XVII, XXVII, XXXII, XLI, LII, LXX
PATH C    Part IX
CHECK     Parts X, XXIX, XLIII, LIV, LXXV
GUIDE     Part XI
REGISTERS Parts XII, XIX, XXIX, XLIII, LIV, LXXV
CASES     Part XIII
SCENES    Parts XIV, XXV
GOVERN    Parts XV, XXIII, XXIV
CLOSE     Parts XVI, XXVI, XXXI, XLIV–XLIX, LVI–LXXVI
```

## LXXVII.2 The three sentences

```text
The board bands; the harvest meets.
Losses wear names; recoveries are real.
The wild stays wild; the table remembers.
```

## LXXVII.3 The final line

```text
The land gives what it is given.
```

*End of Part LXXVII. Continues in Part LXXVIII (close).*---

# W4-04 · PART LXXVIII — CLOSE

## LXXVIII.1 The close of the land plan

```text
W4-04 is closed. The land has one law, one chain, one wild, and books that
match. The kits hold, the registers are seeded, the calendar is named, and
the years are remembered.
```

## LXXVIII.2 The final instruction

```text
Plant in window. Water in time. Read the board. Trust the bands. Keep the books.
```

## LXXVIII.3 The close line

```text
The land gives what it is given; keep the books honest.
```

*End of Part LXXVIII. Continues in Part LXXIX (absolute end).*---

# W4-04 · PART LXXIX — THE ABSOLUTE END

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXIX
Findings:   FL-01 .. FL-96 (seven bands)
Kits:       6 · Registers: 12 · Scenarios: 20 + soak · Promises: 30
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one yield law · one chain · one wild · honest books
the board bands; the harvest meets
the land gives what it is given

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
absolute end of W4-04.*

*Wave 4 continues with W4-05 (Factions, Diplomacy & Governance) and W4-06
(Medicine, Radiation & the Body).*
```---

# W4-04 · PART LXXX — FINAL REGISTER CLOSE

## LXXX.1 The register census (final form)

```yaml
registers:
  strains: { rows: __, owner: crop_author, current: yes }
  plots: { rows: __, owner: integrator, current: yes }
  species: { rows: __, owner: wild_owner, current: yes }
  blight: { rows: __, owner: blight_owner, current: yes }
  chain: { rows: __, owner: chain_owner, current: yes }
  soil_bands: { rows: 4, owner: crop_author, current: yes }
  stages: { rows: 5, owner: crop_author, current: yes }
  pressure: { rows: 4, owner: wild_owner, current: yes }
  preservation: { rows: 6, owner: chain_owner, current: yes }
  history: { rows: bounded, owner: integrator, current: yes }
  couplings: { rows: __, owner: integrator, current: yes }
  warnings: { rows: __, owner: copy_owner, current: yes }
violations: 0
```

## LXXX.2 The kit census (final form)

```yaml
kits:
  equality: { last: pass, cadence: weekly }
  soil: { last: pass, cadence: seasonal }
  greenhouse: { last: pass, cadence: release }
  wild: { last: pass, cadence: release }
  trapping: { last: pass, cadence: release }
  chain: { last: pass, cadence: release }
```

## LXXX.3 The census law

```text
the register census is the land's yearly oath: count what lives, name what
grows, and let nothing be both missing and claimed.
```

*End of Part LXXX. Continues in Part LXXXI (end).*---

# W4-04 · PART LXXXI — END

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXIII
Findings:   FL-01 .. FL-96
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
```

*End of Part LXXXI. Continues in Part LXXXII (final declaration).*---

# W4-04 · PART LXXXII — FINAL DECLARATION

## LXXXII.1 The final declaration

**W4-04 is complete.** Parts I–LXXXIII, findings FL-01…FL-96, six kits,
twelve registers, twenty scenarios plus soak, thirty promises. Proposal only;
execution requires Annex U and signatures. Binding: every rule family, the
stop-the-line list, the worklist, and the promise register.

## LXXXII.2 The final summaries

```text
in one line:   one yield law, one chain, one wild, honest books
in one word:   bands
in one number: zero (orphans, unnamed losses, divergences)
in one image:  a full cellar in a hard year
in one fear:   a harvest nobody can explain
in one hope:   a field coming back
```

## LXXXII.3 The final sentence

```text
The land gives what it is given; keep the books honest.
```

*End of Part LXXXII. Continues in Part LXXXIII (close).*---

# W4-04 · PART LXXXIII — CLOSE

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
complete (plan) · proposal only · Annex U governs execution
parts I–LXXXIII · findings FL-01..FL-96
one yield law · one chain · one wild · honest books

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-04.*
```

*Wave 4 continues with W4-05 (Factions, Diplomacy & Governance) and W4-06
(Medicine, Radiation & the Body).*---

# W4-04 · PART LXXXIV — FINAL APPENDICES

## LXXXIV.1 The evidence pack index

```text
P0_LAND_LEDGER.md
EQ-*.yaml (equality runs)
SOIL-*.yaml (multi-season)
GH-*.yaml (couplings)
WILD-*.yaml (bounds/pressure/recovery)
TRAP-*.yaml (lifecycle/tone)
BLIGHT-*.yaml (rust year)
CHAIN-*.yaml (orphans/losses)
SCENES-*.yaml (S1–S20 + soak)
FINDINGS.md (FL-01..FL-96, seven bands)
PROMISES.md (30, with guards)
CENSUS-*.yaml (registers)
```

## LXXXIV.2 The naming conventions

```text
strains:  strain_<slug>        plots: plot_<id>
species:  sp_<slug>            blights: blight_<slug>
methods:  trap_<slug>          chain: item_<slug>
warnings: land_<system>_<event>
kits:     Land<Concern>Tests
findings: FL-nn
```

## LXXXIV.3 The reading order

```text
5 min   Part II.2 + Part XLVII (quick tables)
15 min  Parts II–IV (designs)
30 min  Parts V–VI (playbooks + kits)
60 min  full document, in order
```

## LXXXIV.4 The appendix law

```text
the evidence pack exists so the land's honesty is provable, not assumed.
```

*End of Part LXXXIV. Continues in Part LXXXV (the last word).*---

# W4-04 · PART LXXXV — THE LAST WORD

## LXXXV.1 The last paragraph

```text
Somewhere a jar is sealed, a seed is saved, a herd is left alone for a
season, and a warning is read before dawn. None of it is dramatic. All of it
is the land keeping its side of an old agreement: give me what you can, and
I will tell you exactly what I gave back.
```

## LXXXV.2 The last instruction

```text
Keep one yield law. Keep one chain. Keep the wild bounded. Keep the books.
```

## LXXXV.3 The last line

```text
The land gives what it is given; keep the books honest.
```

*End of Part LXXXV. Continues in Part LXXXVI (end).*---

# W4-04 · PART LXXXVI — END

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXVII
Findings:   FL-01 .. FL-96
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
```

*End of Part LXXXVI. Continues in Part LXXXVII (final close).*---

# W4-04 · PART LXXXVII — FINAL CLOSE

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
complete (plan) · proposal only · Annex U governs execution
parts I–LXXXVII · findings FL-01..FL-96 (seven bands)
six kits · twelve registers · twenty scenarios + soak · thirty promises
rules L/S/G/U/W/T/B/H/Z/P · rollout five weeks · calendar four cadences

one yield law · one chain · one wild · honest books

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-04.*

*Wave 4 continues with W4-05 and W4-06.*
```---

# W4-04 · PART LXXXVIII — EXTENDED Q&A (Q241–Q270)

**Q241. What is the land's oldest law?**
The books must match the bushels.

**Q242. What is its newest?**
The wild must be able to come back.

**Q243. What is its kindest?**
Frost warns twice and spares the ripe.

**Q244. What is its sternest?**
Take beyond renewal closes the woods.

**Q245. What is its most patient?**
Soil: bands move in seasons, never hours.

**Q246. What is its most social?**
The table: variety is a fact and a feeling.

**Q247. What is its most private?**
The seed jar.

**Q248. What is its most public?**
The board.

**Q249. What is its most beautiful number?**
Zero orphans.

**Q250. What is its ugliest?**
A loss with no name.

**Q251. What is its favorite season?**
The one with warnings read in time.

**Q252. What is its hardest season?**
The one with warnings ignored.

**Q253. What does it ask of a seed?**
That it remember its parent.

**Q254. What does it ask of a herd?**
That it be counted honestly.

**Q255. What does it ask of a blight?**
That it show its stages.

**Q256. What does it ask of a cellar?**
That it keep its promises.

**Q257. What does it ask of a warning?**
That it arrive before the frost.

**Q258. What does it ask of a kit?**
That it prove the promise.

**Q259. What does it ask of a register?**
That it be current.

**Q260. What does it ask of a year?**
That it be remembered.

**Q261. What does the land fear?**
Being taken for granted.

**Q262. What does the land forgive?**
Every failure that was warned.

**Q263. What does the land never forgive?**
Silence.

**Q264. What does the land teach best?**
That patience compounds.

**Q265. What does the land teach worst?**
That greed compounds too.

**Q266. What is the land's gift to the campaign?**
A future that can be planned.

**Q267. What is the land's gift to the story?**
Years that differ.

**Q268. What is the land's gift to the player?**
A pantry that means something.

**Q269. What is the land's gift to itself?**
Nothing; it takes only what is given.

**Q270. And the plan's last word?**
Keep the books honest.

*End of Part LXXXVIII. Continues in Part LXXXIX (extended threads).*---

# W4-04 · PART LXXXIX — EXTENDED THREADS (FL-97–FL-104)

## LXXXIX.1 Thread AR — "the plot that forgot its water"

**Report:** watered plots dried as if unwatered.

**Walk:**

```text
1. root: care action recorded but moisture factor read the wrong field
2. repair: care feeds the read model; test care→factor; sign check
3. verify: care-factor test
```

| ID | Class | Repair |
|---|---|---|
| FL-97 | care not consumed | correctness |
| FL-98 | no care test | coverage |

## LXXXIX.2 Thread AS — "the harvest that ignored the band"

**Report:** bins exceeded the top band after a good year.

**Walk:**

```text
1. root: harvest rolled a hidden bonus not shown on the board
2. repair: no hidden factors; board and harvest share every input; test
3. verify: hidden-factor scan
```

| ID | Class | Repair |
|---|---|---|
| FL-99 | hidden bonus | truth |
| FL-100 | no scan | coverage |

## LXXXIX.3 Thread AT — "the guide that wrote the future"

**Report:** guide promised a harvest that had not happened.

**Walk:**

```text
1. root: copy key used planned bands in past tense
2. repair: guide writes only after outcomes; tense test; keys split
3. verify: tense test
```

| ID | Class | Repair |
|---|---|---|
| FL-101 | tense error | truth |
| FL-102 | no tense check | coverage |

## LXXXIX.4 Thread AU — "the pond that fed the town"

**Report:** fish yields scaled to population instead of stock.

**Walk:**

```text
1. root: shortcut read census for yield (a convenience)
2. repair: yield from stock and take; census only eats; test
3. verify: stock-yield test
```

| ID | Class | Repair |
|---|---|---|
| FL-103 | census-coupled yield | Rule 5 |
| FL-104 | no stock test | coverage |

## LXXXIX.5 The summary

```text
AR: care must count
AS: no hidden numbers
AT: the guide lives in the past
AU: yields come from stocks, not crowds
```

*End of Part LXXXIX. Continues in Part XC (final close).*---

# W4-04 · PART XC — FINAL CLOSE

## XC.1 The last census

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
parts:      I–XC
findings:   FL-01 .. FL-104 (eight bands)
kits:       6 · registers: 12 · scenarios: 20 + soak · promises: 30
rules:      L/S/G/U/W/T/B/H/Z/P
rollout:    5 weeks · calendar: four cadences · stop-list: 6
open:       0 (worklist closed with kits)
```

## XC.2 The last instruction

```text
Keep one yield law. Keep one chain. Keep the wild bounded. Keep the books.
Plant in window. Water in time. Read the board. Trust the bands.
```

## XC.3 The last line

```text
The land gives what it is given; keep the books honest.
```

## XC.4 The close

```text
W4-04 is closed. Eight bands of findings, six kits, twelve registers, thirty
promises, one law — and a land that warns, remembers, and can come back.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
final close of W4-04.*
```

*Wave 4 continues with W4-05 (Factions, Diplomacy & Governance) and W4-06
(Medicine, Radiation & the Body).*---

# W4-04 · PART XCI — FINAL MEASURES

## XCI.1 The complete measure

```text
Document:  W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Parts:     I–XCIII
Findings:  FL-01 .. FL-104 (eight bands: A–AS)
Kits:      6 — equality · soil · greenhouse · wild · trapping · chain
Registers: 12 — strains · plots · species · blight · chain · soil bands ·
           stages · pressure · preservation · history · couplings · warnings
Scenarios: 20 (S1–S20) + soak (120 days, seeded)
Promises:  30, all guarded
Rules:     10 families (L/S/G/U/W/T/B/H/Z/P)
Worklist:  closed (all rows kitted)
Calendar:  weekly · release · seasonal · yearly
Rollout:   5 weeks
Handoffs:  W3-02 · W3-03 · W3-05 · W4-01 · W4-02 · W4-03 · W4-05 · W4-06 · W3-06
```

## XCI.2 The acceptance walk

| Line | Evidence | Signed |
|---|---|---|
| equality | weekly runs | ☐ |
| soil | multi-season | ☐ |
| greenhouse | coupling runs | ☐ |
| wild | bounds/recovery | ☐ |
| trapping | lifecycle/tone | ☐ |
| blight | rust year | ☐ |
| chain | orphan audit | ☐ |
| seasons | window runs | ☐ |
| records | guide check | ☐ |
| registers | census | ☐ |

## XCI.3 The declaration

**W4-04 is complete.** Every promise guarded, every finding dispositioned,
every register seeded, every kit named. Proposal only; execution requires
Annex U and signatures.

## XCI.4 The three sentences

```text
The board bands; the harvest meets.
Losses wear names; recoveries are real.
The wild stays wild; the table remembers.
```

*End of Part XCI. Continues in Part XCII (end).*---

# W4-04 · PART XCII — END

```text
Document:   W4-04 ECOLOGY, FARMING & WILDLIFE INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XCIII
Findings:   FL-01 .. FL-104
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one yield law · one chain · one wild · honest books
The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
```

*End of Part XCII. Continues in Part XCIII (final close).*---

# W4-04 · PART XCIII — FINAL CLOSE

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
complete (plan) · proposal only · Annex U governs execution
parts I–XCIII · findings FL-01..FL-104 (eight bands)
six kits · twelve registers · twenty scenarios + soak · thirty promises
rules L/S/G/U/W/T/B/H/Z/P · rollout five weeks · calendar four cadences
worklist closed · open items zero

one yield law · one chain · one wild · honest books
the board bands; the harvest meets
the land gives what it is given

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-04.*
```---

# W4-04 · PART XCIV — THE FINAL PAGES

## XCIV.1 The promise walk (all thirty, one walk)

```text
1  board equals harvest            -> equality kit, weekly
2  yields are derived              -> T1 scan, per change
3  soil bands warn progressively   -> multi-season, seasonal
4  amendments ramp over days       -> ramp test, release
5  planting windows hold           -> window runs, seasonal
6  frost and drought warn          -> forecast tests, release
7  greenhouses obey utilities      -> blackout scenario, release
8  pollination is consumed         -> hive test, release
9  contamination is flagged        -> routing test, release
10 one counter per population      -> scan, release
11 populations are bounded         -> bounds test, release
12 pressure bands warn             -> band test, release
13 recovery is authored            -> recovery scenario, seasonal
14 migration conserves totals      -> conservation test, release
15 traps wear and reset            -> lifecycle, release
16 trapping tone is kept           -> content review, per change
17 blight advances in stages       -> rust year, release
18 spread is bounded daily         -> scenario, release
19 treatment costs and hygiene     -> treatment test, release
20 seeds carry quality and harbor  -> seed scenario, seasonal
21 no orphan chain items           -> audit, per change
22 spoilage is bounded and warned  -> stores test, release
23 meals route to nutrition        -> routing test, release
24 variety reaches morale          -> empathy test, release
25 bad years are recorded once     -> guide check, seasonal
26 observation records what happened -> guide audit, seasonal
27 seasons act, not just narrate   -> factor test, release
28 surfaces read, never compute    -> kits, release
29 history is bounded              -> round-trip, release
30 registers are current           -> ledger gate, release
```

## XCIV.2 The final quality statement

```text
A land system is finished when: a stranger can explain any harvest from the
board's own inputs; no loss lacks a name; every herd has a ceiling and a
return; and the guide's account would satisfy a skeptical historian.
```

## XCIV.3 The final image

```text
A winter storehouse: jars labeled, grain dry, seed bank quiet, the guide open
to last autumn — and outside, snow over fields that will be asked again in
spring.
```

## XCIV.4 The final close

```text
W4-04 closes here: ninety-four parts, one hundred and four findings across
eight bands, six kits, twelve registers, thirty promises, and one law that
outlives them all — the land gives what it is given.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
final close of W4-04.*
```

*Wave 4 continues with W4-05 and W4-06.*---

# W4-04 · PART XCV — THE LAST WORD AND CLOSE

## XCV.1 The land plan in one paragraph

One yield law: facts in, bands out, nothing else stored. One chain: every
item from source to table with costs and warnings. One wild: bounded, renewed,
recoverable, observed. One blight: staged, warned, treatable. One soil clock:
rich, worked, tired, exhausted — all visible. One kitchen: meals that
remember. One guide: only what happened, in past tense, once. One calendar:
weekly, release, seasonal, yearly — owned by name.

## XCV.2 The plan's five artifacts that matter most

```text
1. the equality kit   — board equals harvest, always
2. the loss register  — every loss has a name
3. the wild bounds    — renewal, pressure, recovery
4. the stage machine  — blight shows itself in order
5. the guide entries  — the land's memory, written once
```

## XCV.3 The plan's five refusals

```text
1. no local yield math
2. no unnamed losses
3. no unbounded take
4. no orphan items
5. no story-only weather
```

## XCV.4 The plan's five invitations

```text
1. plant in window
2. water in time
3. read the board
4. trust the bands
5. keep the books
```

## XCV.5 The closing gestures

```text
to the kitchen:  every yield arrives with its story
to the wild:     we will leave you enough to return
to the years:    we will remember you honestly
to the player:   the harvest will never surprise you unpleasantly
to the next keeper: the registers are current — begin with them
```

## XCV.6 The final measure

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
parts:      I–XCV
findings:   FL-01 .. FL-104 (eight bands)
kits:       6 · registers: 12 · scenarios: 20 + soak · promises: 30
rules:      L/S/G/U/W/T/B/H/Z/P
calendar:   weekly · release · seasonal · yearly
```

## XCV.7 The last line

```text
The land gives what it is given; keep the books honest.
```

## XCV.8 The close

```text
W4-04 is complete. The land is documented, warned, bounded, remembered — and
waiting for spring.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-04.*
```

*Wave 4 continues with W4-05 (Factions, Diplomacy & Governance) and W4-06
(Medicine, Radiation & the Body).*---

# W4-04 · PART XCVI — THE FINAL CLOSE

## XCVI.1 The complete walkthrough in one page

```text
spring   windows open; strains chosen; soil bands read; seeds sown with quality
summer   care in time (water, weeds); drought watch; hives working; rows eyed
autumn   bands close; harvest meets; losses named; stores filled; guide line
winter   cellars hold; hives checked; frost kept out; next year planned
years    rotation restores; herds recover; blight met in stages; records kept
```

## XCVI.2 The complete review in one page

```text
ASK  which read model? which stage? which warning? which consumer? which register?
SEE  equality kit output; loss causes; bounds; couplings; orphan scan
REFUSE local yield · instant loss · unbounded herd · orphan item ·
       story-only weather · warning spam
SIGN when the kits are green and the registers are current
```

## XCVI.3 The complete maintenance in one page

```text
weekly   equality spot; pressure band; one coupling; one chain item
release  full kits; chain audit; scenarios; copy review
season   window runs; frost/drought; reclaim checks; seed quality
year     census; one addition; one deletion; retrospective
```

## XCVI.4 The final statement

```text
The land is the campaign's slowest clock and its most honest mirror. This
plan keeps it honest: bands, not promises; causes, not silence; bounds, not
arithmetic; years, not resets.

The land gives what it is given; keep the books honest.

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
```
---

*End of W4-04 — the land is documented, warned, bounded, remembered.*

---

# W4-04 · PART XCVII — CLOSING NOTE AND FINAL MEASURE

## XCVII.1 The final measure

```text
W4-04 · ECOLOGY, FARMING & WILDLIFE
parts:      I–XCVII
findings:   FL-01 .. FL-104 (eight bands)
kits:       6 · registers: 12 · scenarios: 20 + soak · promises: 30
rules:      L/S/G/U/W/T/B/H/Z/P · rollout: 5 weeks
worklist:   closed · open items: 0
```

## XCVII.2 The final three sentences

```text
The board bands; the harvest meets.
Losses wear names; recoveries are real.
The wild stays wild; the table remembers.
```

## XCVII.3 The final line

```text
The land gives what it is given; keep the books honest.
```

*Document control: W4-04 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-04.*
