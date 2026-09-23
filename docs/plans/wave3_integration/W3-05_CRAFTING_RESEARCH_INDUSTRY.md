# ASHFALL — WAVE 3 INTEGRATION PROGRAM · PLAN 5 OF 6

# CRAFTING, RESEARCH & INDUSTRY INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W3 (six-plan integration wave)
**Document:** W3-05
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W3-01 (narrative), W3-02 (economy), W3-03 (psychology), W3-04 (combat), W3-06 (UI)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates **making things**: crafting, recipes, foundry/metallurgy,
chemical synthesis, glass/ceramics/printing, salvage/research knowledge,
workshops, and material flows. It extends existing owners — one recipe
authority, one metallurgy authority, one research authority — and it never
invents a parallel production system.

### 0.1 Two selection levels

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Truth & Reach | audit recipes, research unlocks, and industry catalogs; fix unreachable or unconsumed data |
| **B** | One Workshop Floor | unify crafting/research seams, material flow, and station prerequisites; make outputs legible |
| **C** | Industrial Society | production chains, specialization, and long-arc industry on existing owners |

**Level 2:** ten points, each A/B/C (§4.2).

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Reach | 1–10 | — | — |
| B One Workshop Floor | 1,4,8 | 2,3,5,6,7,9 | 10 |
| C Industrial Society | — | 2,5 | 1,3,4,6,7,8,9,10 |

### 0.3 The Wave 3 rule for this plan

> **One production authority per material family.** `CraftingSystem` +
> `RecipeCatalogLoader` own recipes; `SilentFoundrySystem` (with its partial
> family) owns foundry/metallurgy/glass/heat/material; synthesis engines own
> their chemistry chains; `ResearchSystem` owns knowledge unlocks. No parallel
> recipe list, no second metallurgy calculator, no research duplication.

### 0.4 Vocabulary

| Term | Meaning |
|---|---|
| recipe | authored input → output transformation |
| station | required facility/tool |
| prerequisite | knowledge/material/power requirement |
| material flow | inputs consumed from owners |
| knowledge | research unlock state |
| chain | multi-step production |
| byproduct | secondary/waste output |
| salvage | knowledge/parts recovered from relics |

---

## 1. Executive summary

ASHFALL's production stack is deep and already modular:

- **Crafting:** `CraftingSystem`, `CraftContext`, `RecipeCatalogLoader`,
  `PharmaRecipeCatalogLoader`, `ChemicalSynthesisSystem` (+catalog),
  `RelicCatalogLoader`, `TrapRecipeIntegrity`.
- **Foundry:** `SilentFoundrySystem` (+ `.Glassworks`, `.Heat`, `.Material`,
  `.Metallurgy`, `.TreatyLabor` partials), `SilentFoundryCatalog`,
  `SilentFoundryConsequencePolicy`, `FoundryActionSurface`,
  `MaterialProfileCatalog`, `MetallurgyHeavyCatalog`,
  `PowderMetallurgySystem`, `HydraulicExtrusionEngine`,
  `SaltMineExtractionSystem`, `GlassworksCatalog`.
- **Shelter industry:** `CupolaFoundryEngine` (+catalog),
  `ShelterWorkshopSystem`, `ChlorAlkaliSynthesisEngine`,
  `FischerTropschSynthesisEngine`, `CvdDiamondSynthesisEngine`,
  `CryogenicAirSeparation` (data), `ElectrostaticFiltration` (data).
- **Knowledge:** `ResearchSystem`, `ResearchKnowledgeCatalogLoader`/`Def`,
  `ResearchEligibility`, `ResearchState`, `KnowledgeAcquisitionSource`,
  `PrewarArchiveCatalog`/`DecryptionSystem`, `TechSalvageCatalog`.
- **Craft catalogs:** `CeramicsKilnCatalog`, `GlassblowingDistillationCatalog`,
  `OpticsGlassworksCatalog`, `PaperPrintingCatalog`, `GrainMillingCatalog`
  (+discovery/projection), `BunkerBlueprintCatalog`, `CrucibleFoundryCatalog`,
  `RoboticsSystem`.

The gaps:

1. **Recipe reachability** — many catalogs; no single map proving every recipe
   is craftable (Point 1).
2. **Station/prerequisite truth** — recipes declare requirements; consumption
   is unverified (Point 2).
3. **Material flow** — inputs must come from owners (inventory/water/power);
   byproducts must go somewhere (Point 3).
4. **Foundry partial cohesion** — five partials + surface + policy; the
   write/read map and consequence policy need verification (Point 4).
5. **Chemistry chains** — ChlorAlkali/FischerTropsch/CVD/cryogenic/filtration
   engines; their input/output truth and consequences need audit (Point 5).
6. **Glass/ceramics/printing/milling** — narrative catalogs; consumption
   status unknown (Point 6).
7. **Knowledge acquisition** — research/salvage/archive decryption sources;
   unlock routing needs one truth (Point 7).
8. **Workshop/station gating** — workshop system exists; gating of recipes by
   station/facility needs verification (Point 8).
9. **Output legibility** — the player should see what a recipe needs and why
   it is blocked (Point 9).
10. **Industrial chains** — multi-step production (ore → metal → part → item)
    as a read model and (C) real chain state (Point 10).

---

## 2. Verified current state

### 2.1 Crafting core

| Component | Role |
|---|---|
| `CraftingSystem` + `CraftContext` | recipe execution |
| `RecipeCatalogLoader` | recipe data |
| `ChemicalSynthesisSystem` + catalog | synthesis recipes |
| `PharmaRecipeCatalogLoader` | pharma recipes |
| `RelicCatalogLoader` | relic salvage |
| `TrapRecipeIntegrity` | trap recipe integrity |

### 2.2 Foundry family

| Component | Role |
|---|---|
| `SilentFoundrySystem` (+5 partials) | foundry core, glassworks, heat, material, metallurgy, treaty labor |
| `SilentFoundryCatalog` + `ConsequencePolicy` | data + policy |
| `FoundryActionSurface` | actions |
| `MaterialProfileCatalog`, `MetallurgyHeavyCatalog` | material data |
| `PowderMetallurgySystem`, `HydraulicExtrusionEngine` | processes |
| `SaltMineExtractionSystem` | raw input |
| `GlassworksCatalog` | glass data |

### 2.3 Shelter industry

`CupolaFoundryEngine`/catalog, `ShelterWorkshopSystem`,
`ChlorAlkaliSynthesisEngine`, `FischerTropschSynthesisEngine`,
`CvdDiamondSynthesisEngine`; data: `cryogenic_air_separation.json`,
`electrostatic_filtration_catalog.json`.

### 2.4 Knowledge

`ResearchSystem`, knowledge catalog/def/eligibility/state,
`KnowledgeAcquisitionSource`, `PrewarArchiveCatalog` + decryption,
`TechSalvageCatalog`, `BunkerBlueprintCatalog`.

### 2.5 Craft catalogs

`CeramicsKilnCatalog`, `GlassblowingDistillationCatalog`,
`OpticsGlassworksCatalog`, `PaperPrintingCatalog`, `GrainMillingCatalog`
(+discovery/projection), `CrucibleFoundryCatalog`, `RoboticsSystem`.

### 2.6 Known integration facts

- The foundry family's host session exists (`SilentFoundryHostSession`) with
  save riding the expansion hub; the W2-02 D19c finding concerns its stale
  lifecycle (coordinate).
- Plan 189 sealed piezometer/water bridging; foundry heat/cooling likely reads
  those owners.
- W3-04's condition/repair loop consumes crafting outputs.

---

## 3. Scope, non-goals, rules

### 3.1 In scope

- Recipe reachability and prerequisite truth.
- Station/facility gating verification.
- Material flow and byproduct routing.
- Foundry partial cohesion and consequence policy.
- Chemistry chain truth.
- Glass/ceramics/printing/milling consumption.
- Knowledge acquisition/unlock routing.
- Workshop gating.
- Recipe legibility (why blocked).
- Industrial chains (read model; C real state).

### 3.2 Non-goals

- New production systems.
- Balance/tuning (W2-03; values may be measured).
- Prose/names (W2-06).
- Save schema without signature.
- Combat/economy/psych mechanics (other W3 plans).

### 3.3 Rules

1. One recipe authority; catalogs are data.
2. Every input consumed from an owner; every byproduct owned.
3. Knowledge gates are read from `ResearchSystem`, not local flags.
4. Foundry policy (consequences) applies once per batch with attribution.
5. Determinism: process yields with any randomness use seeded RNG.
6. No hidden recipes (debug-only entries excluded from reachability or
   archived).

---

## 4. Plan Path and decision index

### 4.1 The ten points

| # | Point | Default |
|---|---|---|
| 1 | Recipe reachability map | B |
| 2 | Station and prerequisite truth | B |
| 3 | Material flow and byproducts | B |
| 4 | Foundry cohesion and consequence policy | B |
| 5 | Chemistry chain truth | B |
| 6 | Glass/ceramics/printing/milling consumption | A |
| 7 | Knowledge acquisition and unlock routing | B |
| 8 | Workshop/station gating | A |
| 9 | Recipe legibility surfaces | B |
| 10 | Industrial chain model | C |

### 4.2 Selection sheet

```text
PLAN W3-05 — CRAFTING, RESEARCH & INDUSTRY
Plan Path: [ ] A Truth & Reach  [ ] B One Workshop Floor (default)  [ ] C Industrial Society

01 recipe reachability .... [A] [B] [C]   default B
02 station/prereqs ........ [A] [B] [C]   default B
03 material flow .......... [A] [B] [C]   default B
04 foundry cohesion ....... [A] [B] [C]   default B
05 chemistry chains ....... [A] [B] [C]   default B
06 glass/ceramics/print ... [A] [B] [C]   default A
07 knowledge routing ...... [A] [B] [C]   default B
08 workshop gating ........ [A] [B] [C]   default A
09 recipe legibility ...... [A] [B] [C]   default B
10 industrial chains ...... [A] [B] [C]   default C
```

---

## 5. Decision Point 1 — Recipe reachability map (default B)

### 5.1 The design question

Which recipes are actually craftable in a campaign? The plan needs a map:
recipe → prerequisites → station → materials → consumer.

### 5.2 Path A — Reachability report

- Enumerate recipes across all catalogs; check prerequisites are satisfiable
  (knowledge exists, materials exist, station exists).
- Publish `docs/crafting/RECIPE_REACHABILITY.md`.

### 5.3 Path B — Repairs + gate

- Repair unreachable recipes (missing prerequisite refs, typos, missing
  station) or archive them with reasons.
- Gate: every recipe has satisfiable prerequisites or an archive entry.

### 5.4 Path C — Chain map

Path B, plus a generated production graph (material → intermediate → item)
used for authoring and for W3-02 supply checks.

### 5.5 Acceptance

- Every recipe classified; gate green.

---

## 6. Decision Point 2 — Station and prerequisite truth (default B)

### 6.1 The design question

Recipes declare stations/tools/knowledge/power; the consumption of those
declarations must be verified.

### 6.2 Path A — Declaration audit

- For each recipe: declared prerequisites vs. checked-in-execution.
- Report unchecked declarations.

### 6.3 Path B — Enforced prerequisites

- Execution checks every declared requirement through its owner (station from
  workshop/facility state, knowledge from research, power from grid, tool from
  inventory).
- Tests: craft fails without each requirement; succeeds when all present.

### 6.4 Path C — Requirement alternatives

Path B, plus authored alternatives (either tool A or B) where intended.

### 6.5 Acceptance

- No unchecked declaration; failure messages name the missing requirement.

---

## 7. Decision Point 3 — Material flow and byproducts (default B)

### 7.1 The design question

Inputs come from inventory/water/power owners; byproducts must go somewhere
(stored, vented, hazardous).

### 7.2 Path A — Flow audit

- For each recipe/process: inputs consumed from which owner; outputs to which
  owner; byproducts handled how.
- Report inputs that appear from nowhere and byproducts that vanish.

### 7.3 Path B — Owned flows

- Every consumption calls the owning API; every output/byproduct routes to an
  owner (inventory, water state, atmosphere/ventilation per the foundry's
  existing ventilation binding).
- Tests: consuming reduces the owner's value; byproduct appears or is
  accounted (with attribution).

### 7.4 Path C — Mass balance

Path B, plus a mass-balance test per chain (inputs ≈ outputs + byproducts
within authored tolerance) as a data-integrity gate.

### 7.5 Acceptance

- No material from nowhere; byproducts accounted.

---

## 8. Decision Point 4 — Foundry cohesion and consequence policy (default B)

### 8.1 The design question

The foundry family spans five partials, a surface, a catalog, and a consequence
policy. The audit: is the read/write map coherent, and does policy apply once
per batch?

### 8.2 Path A — Cohesion audit

- Map partials → state they mutate; surface → commands; policy → consequences.
- Report overlap and unowned state.

### 8.3 Path B — Policy truth

- Consequence policy applies per authored batch/event exactly once, attributed
  (labor/treaty/safety consequences); ventilation/smoke routes through the
  existing ventilation binding (verified `BindVentilation`).
- Coordinate W2-02's D19c lifecycle finding (host session reset).
- Tests: one batch = one consequence; ventilation effect applied; policy
  attribution recorded.

### 8.4 Path C — Foundry doctrine

Path B, plus authored doctrine choices (throughput vs. safety vs. quality) on
the foundry owner.

### 8.5 Acceptance

- Coherent map; policy once; ventilation route verified.

---

## 9. Decision Point 5 — Chemistry chain truth (default B)

### 9.1 The design question

Synthesis engines (chlor-alkali, Fischer-Tropsch, CVD diamond, cryogenic
separation, electrostatic filtration) have inputs/outputs and hazards. Each
chain's truth needs audit.

### 9.2 Path A — Chain audit

- Per engine: inputs, outputs, energy/heat, hazards, byproducts, consumer.
- Report chains with no real consumer or no hazard handling.

### 9.3 Path B — Verified chains

- Inputs/outputs route through owners; hazards (chlorine, CO, pressure) route
  to environment/health owners; power/heat consumed.
- Tests: chain consumes/produces; hazard event handled; no unowned output.

### 9.4 Path C — Chemical industry arcs

Path B, plus authored production goals (a chemical program) through research
and narrative.

### 9.5 Acceptance

- Every chain consumes/produces through owners; hazards handled.

---

## 10. Decision Point 6 — Glass/ceramics/printing/milling consumption (default A)

### 10.1 The design question

These narrative catalogs (`CeramicsKilnCatalog`, `OpticsGlassworksCatalog`,
`PaperPrintingCatalog`, `GrainMillingCatalog`, `GlassblowingDistillationCatalog`)
exist; are they consumed?

### 10.2 Path A — Consumption audit

- For each catalog: consumer, recipe source, station, output.
- Report unconsumed catalogs.

### 10.3 Path B — Consumption binds

- Bind unconsumed catalogs into their crafting/foundry owners (the catalogs
  are data; the owners execute).
- Tests: a ceramic/glass/print/mill recipe is craftable end to end.

### 10.4 Path C — Specialty industries

Path B, plus authored specialty chains (optics for instruments, printing for
archives) with W2-06 content.

### 10.5 Acceptance

- No unconsumed craft catalog; end-to-end test per family.

---

## 11. Decision Point 7 — Knowledge acquisition and unlock routing (default B)

### 11.1 The design question

Research knowledge comes from study, prewar archives (decryption), tech
salvage, and discoveries. Unlocks must route through `ResearchSystem` once.

### 11.2 Path A — Source audit

- Map each knowledge source → acquisition → unlock → consumer.
- Report sources that grant nothing or unlocks that grant repeats.

### 11.3 Path B — One unlock path

- All acquisitions grant through the research owner; unlocks apply once
  (idempotency key per knowledge id); consumers read eligibility.
- Tests: acquisition unlocks; repeat acquisition no-ops; eligibility matches
  state.

### 11.4 Path C — Research programs

Path B, plus authored multi-step research programs (prerequisites across
knowledge) riding the research state; prose by W2-06.

### 11.5 Acceptance

- No dead sources; unlocks once; consumers read eligibility.

---

## 12. Decision Point 8 — Workshop/station gating (default A)

### 12.1 The design question

`ShelterWorkshopSystem` exists; facilities/stations gate recipes. Are gates
enforced and legible?

### 12.2 Path A — Gate audit

- For each station-gated recipe: gate source, enforcement, message.
- Report paper gates.

### 12.3 Path B — Enforced gating

- Station gating reads the workshop/facility owner; blocked crafts explain
  which station is missing.
- Tests: gated recipe blocked without station; allowed with it.

### 12.4 Path C — Workshop upgrades

Path B, plus authored station upgrades (better tools) through shelter upgrades.

### 12.5 Acceptance

- Gates enforced and explained.

---

## 13. Decision Point 9 — Recipe legibility surfaces (default B)

### 13.1 The design question

The player should see what a recipe needs and why it is blocked: materials,
station, knowledge, power.

### 13.2 Path A — Legibility audit

- Compare the craft UI's claims to owner values.
- Report discrepancies.

### 13.3 Path B — Requirement decomposition

- The craft surface shows each requirement and its current satisfaction,
  sourced from owners (no UI math), following the W2-03/W3-02 legibility
  pattern.
- Tests: displayed requirements match owner state; blocked reason is accurate.

### 13.4 Path C — Craft planning

Path B, plus a read-only "what can I make now" list composed from eligibility
(read-only).

### 13.5 Acceptance

- Requirements truthful; blocked reasons accurate.

---

## 14. Decision Point 10 — Industrial chain model (default C)

### 14.1 The design question

Multi-step chains (ore → metal → part → item) and their bottlenecks are the
industrial identity. A read model (B) and optional real chain state (C) are the
options.

### 14.2 Path A — Chain audit

- Map chains across catalogs/engines; report missing links.

### 14.3 Path B — Chain read model

- `ProductionChainModel`: current stock of intermediates, bottleneck step, and
  estimated throughput, composed from owners.
- Consumed by planning surfaces and W3-02 supply.

### 14.4 Path C — Chain state

Path B, plus authored chain state (work-in-progress at stations) on the
workshop/foundry owner (additive, signed), enabling real pipelining.

### 14.5 Acceptance

- Chains mapped; read model owner-sourced; (C) state signed and tested.

---

## 15. Execution phases

### CR0 — Premise freeze (1 day)

- Verify systems/data; run crafting/foundry selftests; produce
  `P0_CRAFT_PREMISE.md`.

### CR1 — Reachability and prerequisites (Points 1 + 2)

- Report + repairs + gate.

### CR2 — Flows and foundry (Points 3 + 4)

- Material routing; policy once; ventilation verified.

### CR3 — Chemistry and catalogs (Points 5 + 6)

- Chain truth; consumption binds.

### CR4 — Knowledge and gating (Points 7 + 8)

- Unlock routing; station gates.

### CR5 — Legibility (Point 9)

- Requirement decomposition.

### CR6 — Chains (Point 10, C)

- Read model; signed state.

### CR7 — Closeout

- Evidence; ledger proposals; Annex U.

---

## 16. Verification plan

| Point | Evidence |
|---|---|
| 1 | reachability map; gate |
| 2 | enforcement tests per requirement |
| 3 | consumption/production tests |
| 4 | policy once; ventilation route |
| 5 | chain consume/produce; hazard handling |
| 6 | end-to-end per family |
| 7 | unlock once; eligibility |
| 8 | gate enforced/explained |
| 9 | displayed = owner; blocked accurate |
| 10 | chain map; (C) state tests |

Commands:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Crafting
bash scripts/run_test.sh Ashfall.Core.Tests/Foundry    # if present
bash scripts/run_test.sh Ashfall.Core.Tests/Research
godot --headless --path . -- --data-integrity-selftest
```

---

## 17. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | reachability repairs change balance | M | M | truth only; measure, don't tune |
| 2 | mass-balance gate false positives | M | L | authored tolerance; allowlist |
| 3 | foundry policy double-apply | M | M | idempotency |
| 4 | chemistry hazards unhandled | M | H | route to health/environment owners |
| 5 | knowledge unlock repeat | M | M | keyed idempotency |
| 6 | station gates paper-only | M | M | enforcement tests |
| 7 | legibility drift | M | L | displayed-equals-owner tests |
| 8 | chain state persistence | M | H | signed line only |
| 9 | concurrent claims on foundry files | M | H | single-writer; W2-02 coordinate |
| 10 | scope creep to new industry systems | M | H | Wave 3 rule |

---

## 18. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| CR0 | `W3-05-CR0-PREMISE` | premise doc |
| CR1 | `W3-05-CR1-REACHABILITY` | recipe map + gate + repairs |
| CR2 | `W3-05-CR2-FLOWS-FOUNDRY` | material routing; policy |
| CR3 | `W3-05-CR3-CHEM-CATALOGS` | chains; consumption |
| CR4 | `W3-05-CR4-KNOWLEDGE-GATES` | unlocks; station gates |
| CR5 | `W3-05-CR5-LEGIBILITY` | requirement surface |
| CR6 | `W3-05-CR6-CHAINS` | chain model (+ state) |
| CR7 | `W3-05-CR7-CLOSEOUT` | evidence + proposals |

Coordination: W3-02 owns economy flows; W3-04 owns condition/repair consumers;
W3-01 owns narrative ties; W2-02 owns foundry lifecycle repair; W2-06 owns
prose.

---

## 19. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | keep report | recipes unverified |
| 2 | remove enforcement | paper requirements |
| 3 | remove binds | flows unverified |
| 4 | keep audit | policy double-apply risk |
| 5 | keep audit | chains unverified |
| 6 | keep audit | catalogs unconsumed |
| 7 | remove routing | unlocks unverified |
| 8 | keep audit | gates paper-only |
| 9 | remove surface | recipes opaque |
| 10 | keep map | chains unknown |

---

## 20. DoD and handoff

**Path A:** premise + all audits complete.

**Path B:** all of A, plus enforced prerequisites, owned flows, policy truth,
verified chains, consumption binds, unlock routing, station gates, and
legibility — each tested.

**Path C:** all of B, plus chain read model and signed chain state.

**Handoff:** outcome, files, contract (recipe/flow/knowledge authorities),
commands, limitations, untouched shared paths, ledger proposals, Annex U.

### 20.1 First safe step

> CR0 only: premise and selftests. No recipe changes first.

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

## U.1 What W3-05 releases

| Blocked item | Mechanism | Gate |
|---|---|---|
| Expansions 27–31 (Thread/Lesson/Glass/Press/Kiln) | Recipe/catalog consumption gives each its production substrate | CR1/CR3 |
| Expansion 16 robotics half | Robotics/foundry outputs verified | CR3 |
| W3-04 condition/repair loop | Repair recipes and material consumption verified | CR2 |
| W3-02 supply/trade | Production inputs/outputs verified for supply composition | CR2/CR6 |
| XP-04 economy legs (consumer) | Crafting outputs feed the economy's goods truth | CR1 |
| EN-03 pressure (craft inputs) | Chains feed supply state | CR6 |
| Plan 49 depth passes (industry) | Reachability evidence for its content certification | CR1 |
| W3-01 narrative (craft arcs) | Verified unlock/chain state for story gates | CR4 |

## U.2 Signatures needed

```text
[ ] I authorize CR0 premise + selftests.
[ ] I authorize CR1 recipe reachability repairs + gate.
[ ] I authorize CR2 material routing + foundry policy truth.
[ ] I authorize CR3 chemistry chain truth + catalog consumption binds.
[ ] I authorize CR4 knowledge unlock routing + station gates.
[ ] I authorize CR5 requirement legibility surface.
[ ] I authorize CR6 chain model (+ signed chain state: [ ] no [ ] signed).
```

## U.3 What W3-05 never touches for unblocking

- Balance bands (W2-03).
- Prose (W2-06).
- Economy owners (W3-02) except as consumers.
- Combat owners (W3-04) except as consumers.
- Save schema without signature.

## U.4 The production-release rule

A crafting feature releases when its recipe, prerequisites, inputs, and outputs
all resolve through owners. A recipe that produces an item no owner accepts
releases nothing.

---

# APPENDICES

## A.1 Selection sheet

```text
ASHFALL WAVE 3 · PLAN 5 (CRAFTING/RESEARCH) · SELECTION
Date: ______  Foreman: ______  HEAD: ______
PLAN PATH: [ ] A Truth & Reach  [ ] B One Workshop Floor (default)  [ ] C Industrial Society

01 recipe reachability .... [A] [B] [C]   default B
02 station/prereqs ........ [A] [B] [C]   default B
03 material flow .......... [A] [B] [C]   default B
04 foundry cohesion ....... [A] [B] [C]   default B
05 chemistry chains ....... [A] [B] [C]   default B
06 glass/ceramics/print ... [A] [B] [C]   default A
07 knowledge routing ...... [A] [B] [C]   default B
08 workshop gating ........ [A] [B] [C]   default A
09 recipe legibility ...... [A] [B] [C]   default B
10 industrial chains ...... [A] [B] [C]   default C
Signature: ________________
```

## A.2 Glossary

| Term | Meaning |
|---|---|
| reachability | a recipe can be executed in a campaign |
| prerequisite | declared requirement (station/knowledge/material/power) |
| flow | consumption/output routing through owners |
| policy | foundry consequence application |
| unlock | knowledge acquisition effect |
| chain | multi-step production |
| bottleneck | the limiting step in a chain |
| legibility | player-visible requirement decomposition |

**End of Part I.** Proposal only; executes nothing; releases nothing without
U.2 signatures.

---

# PART II — DEEP DESIGN SPECIFICATIONS (CONTINUED → 180K)# W3-05 · PART II — DEEP DESIGN: POINTS 1–5

> Appended 2026-09-21. Part I (summary contract) + this expansion. Proposal
> only. One production authority per material family (§0.3) binds throughout.

---

## §II.1 Decision Point 1 — Recipe reachability map (default B)

### II.1.1 The problem, precisely

ASHFALL's crafting surface spans a large catalog family: `RecipeCatalogLoader`
(general), `PharmaRecipeCatalogLoader` (medicine), `ChemicalSynthesisSystem` +
catalog, `RelicCatalogLoader` (salvage), plus foundry/industry catalogs for
glass, ceramics, printing, milling, optics, crucibles, powders, extrusion,
salt, and shelter engines (cupola, chlor-alkali, Fischer-Tropsch, CVD diamond,
cryogenic separation, electrostatic filtration). A recipe can fail to be
craftable in four distinct ways:

1. **Missing prerequisite** — knowledge/material/station referenced but never
   obtainable.
2. **Missing station** — a facility that no system can build/provide.
3. **Missing input** — an ingredient with no source in the world.
4. **Missing consumer** — the output is useless (an economic dead end).

All four classes are silent: the game boots, the recipe appears in a menu (or
not), and nobody notices. Reachability makes them loud.

### II.1.2 The reachability algorithm (spec)

```text
input:  all recipe catalogs + knowledge catalog + station sources + item sources
output: classification per recipe

for each recipe R:
  P := declaredPrerequisites(R)          # knowledge, station, tool, power
  I := declaredInputs(R)                 # item ids + quantities
  O := declaredOutputs(R)

  knowledge reachable?  := every K in P.knowledge is grantable (source exists)
  station reachable?    := every S in P.station is buildable/provided
  inputs reachable?     := every item in I has a source chain:
                              raw spawn | salvage | harvest | purchased | produced
  outputs consumed?     := every item in O has a consumer:
                              recipe input | build cost | quest award | economy sale
  classify(R):
    REACHABLE          all true
    KNOWLEDGE-LOCKED   knowledge chain has no source
    STATION-LOCKED     station chain has no source
    INPUT-DEAD         an ingredient has no source
    OUTPUT-DEAD        outputs have no consumer
    ORPHANED           no catalog loader reads it
    DEBUG-ONLY         marked test content
```

### II.1.3 Source-chain resolution

The interesting work is `has a source chain`:

```text
SourceNote(item):
  raw:        appears in world spawn/harvest tables (authored)
  salvage:    relic/scavenge output (authored)
  purchased:  market good (W3-02 quote exists)
  produced:   recipe output (recursive; cycles allowed if seeded by raw)
Cycle rule: a production cycle with no raw entry point is dead (a closed
  loop of ingredients nobody spawns). The algorithm detects cycles and
  reports the loop.
Depth rule: resolve to depth N (authored, e.g., 8) and report chains longer
  than N for review (deep chains are fragile).
```

### II.1.4 Consumer resolution

`has a consumer` checks the reverse graph:

```text
ConsumerNote(item):
  recipe:    used as input by >=1 reachable recipe
  build:     building/shelter cost list
  award:     quest/reward vocabulary (W3-01)
  economy:   market good with an atlas row (W3-02)
  narrative: authored item (keepsake, memorial artifact)
  tool:      equipable/tool use (condition systems)
```

An item with none of these is OUTPUT-DEAD; either wire a consumer or archive
the recipe with a reason (never delete silently — the corpus rule).

### II.1.5 The classification gate

```text
gate R1: shipped recipes have no ORPHANED/DEBUG-ONLY in the live set
gate R2: KNOWLEDGE-LOCKED/STATION-LOCKED/INPUT-DEAD/OUTPUT-DEAD are counted
         in a baseline that may not grow (ratchet)
gate R3: every repair/archive carries a reason line
gate R4: the map regenerates and diffs cleanly (drift gate)
```

### II.1.6 Deliverables

```text
docs/crafting/RECIPE_REACHABILITY.md   (generated table + authored reasons)
docs/crafting/SOURCE_CHAINS.md         (item -> chain -> entry point)
docs/crafting/OUTPUT_CONSUMERS.md      (item -> consumer)
docs/crafting/REACHABILITY_BASELINE.md (ratchet)
```

### II.1.7 Failure classes and tests

| Class | Example | Test |
|---|---|---|
| knowledge-locked | pharma tier 3 with no book source | Recipe_KnowledgeChain |
| station-locked | optics bench buildable? | Recipe_StationChain |
| input-dead | reagent with no spawn/recipe | Recipe_InputSource |
| output-dead | crafted trophy with no consumer | Recipe_OutputConsumer |
| cycle-dead | A needs B needs A, no raw | Recipe_CycleDetection |
| orphaned | loader never registered | Recipe_LoaderRegistration |
| depth | chain of 12 steps | Recipe_ChainDepthReport |

### II.1.8 Cost model

Catalog harvest (1-2 days), chain resolver + cycle detection (2-3 days), map
generation + gates (2 days), ranked repairs list (1 day).

---

## §II.2 Decision Point 2 — Station and prerequisite truth (default B)

### II.2.1 The problem, precisely

Recipes declare requirements. The failure modes:

1. **Unchecked declaration** — the requirement is descriptive prose in a data
   field nothing reads (paper prerequisite).
2. **Partially checked** — knowledge checked, station not.
3. **Wrong owner read** — station checked against a local flag rather than
   the facility owner.
4. **No failure message** — a blocked craft says "cannot craft" without
   naming the missing requirement (legibility failure feeding Point 9).

### II.2.2 The requirement taxonomy

```text
RequirementKind:
  knowledge   -> ResearchSystem eligibility (W3-05 P7)
  station     -> facility/workshop owner state (shelter or foundry)
  tool        -> equipment owner (inventory; condition bands apply)
  power       -> grid/power owner (draw per craft)
  heat        -> foundry thermal owner (heat range per process)
  environment -> authored (ventilation, cooling — foundry/ventilation owner)
  labor       -> schedule/roster owner (craft time + effort)
  quantity    -> inventory owner (inputs)
```

Every kind maps to exactly one owner API. The audit walks each recipe's
declarations and verifies the owner read exists and is enforced at execution.

### II.2.3 The execution gate

```text
CanCraft(recipe, actor, station):
  for kind in recipe.requirements:
     check := owner.check(kind, requirement, actor, station)
     if !check.ok: return Blocked(kind, requirement, check.reason)
  return Ready

Execute(recipe, ...):
  assert Ready (re-run at execution; no TOCTOU on station/power)
  consume inputs (Point 3)
  spend time/labor
  produce outputs
  apply byproduct routing
  log to the craft ledger (attribution)
```

### II.2.4 TOCTOU discipline (time-of-check to time-of-use)

A craft checked as ready may find power gone at execution. Rules:

- requirements re-checked at execution start (cheap);
- a mid-craft interruption has authored behavior (pause/fail/material loss
  per class) — the option is authored, not accidental;
- power/heat draws are reserved or consumed atomically through their owners.

### II.2.5 The requirement table

```text
docs/crafting/REQUIREMENTS.md
  recipe -> [kind, value, owner, checked_at (exec), message_ref]
```

The table is generated; the message refs are corpus strings (the blocked-reason
copy shown to the player). Missing message refs are findings (a block without
an explanation is the exact failure Point 9 exists to fix).

### II.2.6 Failure classes and tests

| Class | Example | Test |
|---|---|---|
| paper knowledge | field never read | Recipe_KnowledgeChecked |
| paper station | facility ignored | Recipe_StationChecked |
| wrong owner | local flag for station | Recipe_OwnerSource |
| silent block | message missing | Recipe_BlockMessage |
| TOCTOU | power vanishes mid-craft | Recipe_RecheckAtExecution |
| interruption | authored behavior tested | Recipe_InterruptBehavior |

### II.2.7 Cost model

Taxonomy mapping (1 day), enforcement audit per catalog family (3 days),
message registration (1 day), tests (2 days).

---

## §II.3 Decision Point 3 — Material flow and byproducts (default B)

### II.3.1 The problem, precisely

Crafting consumes and produces items, power, water, heat, and waste. Failure
modes:

1. **Material from nowhere** — an output produced without consuming the
   declared inputs (or consuming from a local cache instead of the owner).
2. **Vanishing byproduct** — slag/ash/smoke exists in the data and disappears
   at runtime (no owner, no consequence).
3. **Double consumption** — two owners both deduct the input (craft + UI).
4. **Untracked power** — thermal/electrical draw not metered.
5. **Ventilation bypass** — smoke/exhaust effects not routed to the verified
   ventilation binding (the foundry family already binds ventilation per the
   `BindVentilation` evidence).

### II.3.2 The flow contract

```text
CraftFlow:
  consume: [owner, item/quantity]     # exactly once, via owner API
  produce: [owner, item/quantity]     # inventory or facility output tray
  byproduct: [owner, stream, quantity]# slag/ash/exhaust/heat
  utilities: {power, water, heat}     # metered draws
Ledger entry (attribution):
  {recipe, actor, day, consumes, produces, byproducts, utilities}
```

### II.3.3 Byproduct routing table (authoring)

| Process family | Byproduct stream | Owner |
|---|---|---|
| smelting | slag | facility storage (foundry owner) |
| smelting | exhaust | ventilation/atmosphere owner |
| glass | cullet | inventory (reusable) |
| chemistry | waste brine/chlorine | W3-03 health (exposure) + atmosphere |
| milling | chaff | inventory/feed (authored) |
| burning/heat | ash | atmosphere (W2-04 handshake) |
| filtration | residue | facility storage |

Every stream has an owner and an authored disposition; a stream with no owner
fails the static check.

### II.3.4 Mass-balance gate (C path)

```text
per recipe/chain: inputs ≈ outputs + byproducts within authored tolerance
violations: either authoring error (fix data) or a hidden consumer (finding
  a second writer of the stream)
```

The B path runs mass-balance as an audit report; the C path gates it.

### II.3.5 Failure classes and tests

| Class | Example | Test |
|---|---|---|
| free output | output without input deduction | Flow_ConsumeOnce |
| double consume | craft and UI both deduct | Flow_SingleConsume |
| lost byproduct | slag not stored/routed | Flow_ByproductRouted |
| unowned stream | exhaust no owner | Flow_StreamOwner |
| unmetered utility | power draw absent | Flow_UtilityMetered |
| ventilation bypass | smoke ignores binding | Flow_VentilationBound |

### II.3.6 Cost model

Flow-contract implementation (3-4 days across owners), routing table authoring
(2 days), meter audits (1-2 days), tests (2 days).

---

## §II.4 Decision Point 4 — Foundry cohesion and consequence policy (default B)

### II.4.1 The problem, precisely

The silent foundry family spans core + `.Glassworks`, `.Heat`, `.Material`,
`.Metallurgy`, `.TreatyLabor` partials, plus `FoundryActionSurface`,
`SilentFoundryCatalog`, `SilentFoundryConsequencePolicy`, and
`SilentFoundryHostSession`. The audit targets:

1. **Partial cohesion** — read/write overlap: two partials mutating one state
   family without a single owner.
2. **Policy double-apply** — consequence policy (labor/treaty/safety effects)
   applied per batch and again per tick.
3. **Ventilation route** — the verified binding must be the only atmosphere
   path.
4. **Host lifecycle** — the D19c finding: `_silentFoundry` created in
   `SetupSilentFoundry()` (early return path) and never nulled by reset —
   coordinate with W2-02's repair rather than duplicating it.

### II.4.2 The partial cohesion map

```text
PARTIAL MAP (to generate)
  state family -> [writers], [readers], tick/no-tick
  overlap => finding (single owner or documented aggregation)
```

Deliverable: `docs/crafting/FOUNDRY_COHESION.md` with the map and the
overlap findings.

### II.4.3 The consequence policy model

```text
ConsequencePolicy:
  triggers: [batch_complete, shift_end, incident, treaty_delivery]
  effects: [labor strain, treaty obligation progress, safety incident,
            material spoilage, heat buildup]
  keys: policy:<batch-id>:<trigger>     # once-only
  attribution: policy application log
```

Rules:

1. One application per key; re-application logs a duplicate finding.
2. Effects route to owners (W3-03 strain, W3-02 obligations, W2-04 thermal).
3. Policy is data-driven (`SilentFoundryConsequencePolicy`) — no hidden code
   branches adding extra effects.

### II.4.4 Treaty labor

`.TreatyLabor` ties foundry output to treaty obligations (W3-02 C-P6).
The audit: obligations consume real production; completion is observable;
default has consequences through the existing ladder.

### II.4.5 Failure classes and tests

| Class | Example | Test |
|---|---|---|
| partial overlap | two partials write heat | Foundry_PartialOwnership |
| policy double | effects twice per batch | Foundry_PolicyOnce |
| ventilation bypass | atmosphere effect unbound | Foundry_VentilationSinglePath |
| lifecycle staleness | session persists after reset | Foundry_SessionReset (W2-02 coord) |
| treaty orphan | obligation with no output link | Foundry_TreatyWired |

### II.4.6 Cost model

Map generation (2 days), policy key wiring (2 days), ventilation verification
(1 day), lifecycle coordination (1-2 days with W2-02), tests (2 days).

---

## §II.5 Decision Point 5 — Chemistry chain truth (default B)

### II.5.1 The problem, precisely

The synthesis engines (chlor-alkali, Fischer-Tropsch, CVD diamond, cryogenic
air separation, electrostatic filtration, powder metallurgy, hydraulic
extrusion, salt extraction) each model a real chain with inputs, outputs,
energy, hazards, and byproducts. Failure modes:

1. **Unconsumed chain** — an engine exists with no recipe/UI path reaching it.
2. **Hazard-free chemistry** — chlorine/pressure/heat hazards authored in
   lore but not routed to health/environment owners.
3. **Energy fiction** — processes that should draw power/heat draw nothing.
4. **Stranded output** — chain products with no consumer (Point 1's
   OUTPUT-DEAD at engine scale).
5. **Unseeded variability** — yield variance rolled unseeded.

### II.5.2 The chain table

```text
docs/crafting/CHEM_CHAINS.md
  engine -> input chain -> outputs -> utilities (power/heat) ->
            hazards -> byproducts -> consumers
```

Each engine gets a row; rows are verified against the live systems at P0.

### II.5.3 Hazard routing

| Hazard | Route |
|---|---|
| toxic gas (chlorine, CO) | ventilation (concentration band) → W3-03 exposure |
| pressure/vessel | incident event → health + facility damage |
| thermal | heat owner → fire hazard (W2-04) |
| corrosive | facility condition decay + injury events |
| cryogenic | authored injury event on mishandling |

Hazards surface as authored events with warnings (the crisis standard
applies); no silent injury mechanics.

### II.5.4 Yield and quality

```text
yield = authored base × inputs quality (authored bands) × station condition
        × seeded variance (bounded)
quality = authored bands per process; outputs tagged (W3-02 pricing can read
          quality if authored — a documented interface, not an assumption)
```

### II.5.5 Failure classes and tests

| Class | Example | Test |
|---|---|---|
| orphan engine | no recipe reaches it | Chem_EngineReached |
| hazard unrouted | chlorine ignores health | Chem_HazardRouted |
| energy fiction | process draws nothing | Chem_UtilitiesMetered |
| stranded output | product no consumer | Chem_OutputConsumer |
| unseeded yield | variance random | Chem_SeededVariance |
| quality untracked | bands ignored | Chem_QualityBands |

### II.5.6 Cost model

Chain table (2 days), hazard routing (2-3 days), utility metering (1-2 days),
tests (2 days).

---

## §II.6 Points 1–5 execution order

```text
Day 1-2   P1 catalog harvest; source/consumer graph
Day 3-5   P1 chains + cycle detection + map
Day 6-7   P2 taxonomy + enforcement audit
Day 8-10  P3 flow contract + routing tables
Day 11-13 P4 partial map + policy keys + ventilation
Day 14-16 P5 chain table + hazard routing
Day 17-18 consolidation + repairs ranked + handoffs
```

Shared artifacts: the source/consumer graph (P1) feeds P5's stranded-output
audit; the flow contract (P3) is consumed by P4's policy and P5's yields; the
requirement table (P2) supplies the block reasons used by the legibility point
(P9).

---

*End of Part II. Continues in Part III (Points 6–10).*# W3-05 · PART III — DEEP DESIGN: POINTS 6–10

---

## §III.1 Decision Point 6 — Glass/ceramics/printing/milling consumption (default A)

### III.1.1 The problem, precisely

These catalogs exist as data: `CeramicsKilnCatalog`,
`GlassblowingDistillationCatalog`, `OpticsGlassworksCatalog`,
`PaperPrintingCatalog`, `GrainMillingCatalog` (+ its discovery/projection
companions), `CrucibleFoundryCatalog`, `BunkerBlueprintCatalog`. The audit
question: **is each catalog consumed by a live crafting path?**

Failure modes:

1. catalog data loaded but no recipes registered (dead family);
2. recipes registered but stations never buildable (STATION-LOCKED at family
   scale);
3. outputs with no consumers (trophies);
4. discovery/projection companions writing state nobody reads.

### III.1.2 The consumption contract

```text
for each catalog C:
  loader: which loader reads C? (registered at startup?)
  recipes: count and classification (P1)
  stations: which stations do they need? buildable?
  outputs: consumers per item (P1)
  UI: which surface lists these recipes? (W3-06 cross-check)
  result: LIVE | DATA-ONLY | DEAD
```

`DATA-ONLY` is legitimate for a catalog intended for future content (authored
note); `DEAD` (a loader exists but nothing reaches) is a finding.

### III.1.3 The family bind table

| Family | Loader | Station family | Consumer chain |
|---|---|---|---|
| ceramics | shelter/craft loader | kiln | storage, construction, trade |
| glassblowing | foundry loader | glassworks | optics, containers, trade |
| optics | foundry loader | optics bench | instruments, science, trade |
| printing | shelter loader | press | archives, propaganda, W3-01 |
| milling | shelter loader | mill | food production, feed |
| crucibles | foundry loader | crucible furnace | metallurgy inputs |
| blueprints | shelter loader | n/a (knowledge) | construction unlocks |

Each cell verified at P0; empty cells become either bind work (B path) or an
authored data-only note.

### III.1.4 Tests

```text
Catalog_LoaderRegistered_<family>
Catalog_RecipesClassified_<family>
Catalog_StationBuildable_<family>
Catalog_OutputsConsumed_<family>
Catalog_SurfaceListed_<family>
```

### III.1.5 Cost model

Family audit (2-3 days), binds + repairs (3-4 days), tests (1-2 days).

---

## §III.2 Decision Point 7 — Knowledge acquisition and unlock routing (default B)

### III.2.1 The problem, precisely

Knowledge in ASHFALL arrives through: research (`ResearchSystem`, knowledge
catalogs, eligibility, state), salvage (`TechSalvageCatalog`), prewar archives
(`PrewarArchiveCatalog` + decryption), blueprints, and authored discovery
events. Failure modes:

1. **Dead source** — an acquisition path grants nothing;
2. **Repeat grant** — re-acquiring re-fires unlock effects (double
   consequences: recipes duplicated, standing granted twice);
3. **Unread eligibility** — consumers check local flags instead of the
   research owner;
4. **Orphan knowledge** — a knowledge id no consumer reads (unlock with no
   effect);
5. **Unseeded decryption** — archive outcomes random without determinism.

### III.2.2 The knowledge graph

```text
docs/research/KNOWLEDGE_GRAPH.md
  knowledge_id -> sources[] (research study, salvage item, archive, discovery)
              -> grants[]  (recipes, capabilities, dialogue, buildings)
              -> prerequisites[] (other knowledge)
              -> consumers[] (verified reads)
```

Generated from data + code reads. Cycles checked (knowledge requiring itself
indirectly is a dead node unless a signed exception).

### III.2.3 One-unlock routing

```text
Acquire(knowledge_id, source):
  if owner.Has(knowledge_id): return NoOp (logged)
  owner.Grant(knowledge_id)
  apply grants once (key knowledge:<id>)
  record source + attribution (ledger)
```

Every grant (recipe unlock, capability, narrative gate) keys off the knowledge
id; grants are idempotent. The audit scripts double-acquisition and asserts
NoOp.

### III.2.4 Decryption determinism

Archive decryption uses the seeded RNG contract; success bands authored by
archive class; the outcome table is data. Replay equality tested.

### III.2.5 Failure classes and tests

| Class | Example | Test |
|---|---|---|
| dead source | archive grants nothing | Knowledge_SourceGrants |
| repeat grant | second acquire re-fires | Knowledge_Idempotent |
| local flag consumer | recipe checks its own flag | Knowledge_OwnerRead |
| orphan knowledge | no consumer reads id | Knowledge_ConsumerExists |
| unseeded decrypt | archive random | Knowledge_Seeded |
| cycle | A needs B needs A | Knowledge_CycleCheck |

### III.2.6 Cost model

Graph generation (2-3 days), idempotent routing (2 days), consumer audit (2
days), tests (2 days).

---

## §III.3 Decision Point 8 — Workshop/station gating (default A)

### III.3.1 The problem, precisely

`ShelterWorkshopSystem` and facility state gate crafting. Failure modes:

1. **Paper gate** — station declared but not checked (recipes craftable at
   the wrong place);
2. **Invisible gate** — blocked without a reason (legibility);
3. **Station identity drift** — two id spaces for the same station (data vs.
   facility owner);
4. **Upgrade bypass** — a station upgrade that changes nothing (tier 2 bench
   = tier 1 behavior).

### III.3.2 The gate contract

```text
GateCheck(recipe, station):
  required := recipe.station
  actual   := facilityOwner.Station(id) -> {exists, tier, condition, powered}
  pass := exists && tier >= required.tier && (powered if required)
  blocked reason refs: missing | tier | condition | power
```

The mapping recipe→station id must use the facility owner's id space; a second
id space is a finding (identity drift).

### III.3.3 Upgrade semantics

```text
tier effects (authored): recipe tiers unlocked, speed band, quality band,
  power draw
every tier has >=1 authored effect; otherwise the tier is decorative
```

### III.3.4 Tests

```text
Gate_EnforcedAtExecution
Gate_ReasonRefs
Gate_IdSpaceSingle
Upgrade_TierEffects_<tier>
Gate_PowerRequirement
```

### III.3.5 Cost model

Id-space reconciliation (1-2 days), enforcement audit (2 days), upgrade
effects (1-2 days), tests (1 day).

---

## §III.4 Decision Point 9 — Recipe legibility surfaces (default B)

### III.4.1 The problem, precisely

The craft surface must answer three questions truthfully:

1. **Can I make this now?** (readiness)
2. **What is missing?** (the blocking requirement, named)
3. **What will it cost?** (materials, power, time, station)

Failure modes: UI math (computing requirements locally), stale readiness
(cached), missing block reasons, cost shown without units, and the classic:
the surface says ready, execution blocks.

### III.4.2 The legibility contract

```text
CraftView(recipe):
  readiness := CanCraft(recipe)         # owner call, live
  requirements[] := each with {kind, value, satisfied, reason_ref}
  costs := {items (from inventory read), power, time}
  output := {item, quantity, quality bands if authored}
```

Rules:

1. The view never computes; it renders owner calls (surface purity, W3-06).
2. `readiness` re-checked on open and before execute; the execute path is the
   truth (no TOCTOU lies).
3. Every unsatisfied requirement shows its authored message.
4. Output preview shows quality bands (if authored) rather than a precise
   number when the value varies.

### III.4.3 Tests

```text
Legibility_ReadyMatchesExecution
Legibility_BlockReasonPerKind_<kind>
Legibility_CostsOwnerSourced
Legibility_QualityBandsShown
Legibility_NoUiMath
Legibility_RecheckOnExecute
```

### III.4.4 Cost model

View contract (2 days), surface work (with W3-06) (2-3 days), tests (2 days).

---

## §III.5 Decision Point 10 — Industrial chain model (default C, read model at B)

### III.5.1 The problem, precisely

Multi-step chains (ore → metal → part → tool; brine → chlorine → polymer;
grain → flour → bread) are ASHFALL's industrial identity. The B-path gap: no
honest read model of what the shelter's production actually can sustain; the
C-path gap: no work-in-progress state at stations.

### III.5.2 The read model (B)

```text
ProductionChainModel (derived, read-only):
  for each chain step:
    stock(intermediate), throughput cap per day (station capacity × labor),
    current bottleneck (the step with min(cap, input availability)),
    estimated chain output per day, days-to-starve for inputs
  outputs: consumed by planning surfaces (W3-06) and W3-02 supply checks
```

Purity: reads owners only; recomputed per view/tick; never authoritative.

### III.5.3 The chain state (C)

```text
WorkInProgress:
  station, recipe, progress, inputs_reserved, started_day,
  actor, quality roll seed
persisted via the crafting/workshop owner's section (signed)
behavior: pause/resume per authored interruption rules (P2 TOCTOU)
```

This enables real pipelining and scheduling; it is the plan's deepest C item.

### III.5.4 Bottleneck rules

```text
bottleneck := argmin over steps of effective capacity
  effective capacity = min(station throughput, labor available,
                           input availability, utility available)
```

The model reports the bottleneck with a reason; the audit cross-checks the
reported bottleneck against actual soak production (accuracy: the shelter's
real limiting factor is the one named).

### III.5.5 Tests

```text
Chain_ModelReadOnly
Chain_BottleneckAccuracy (soak)
Chain_OutputEstimateAccuracy
Chain_NoSecondAuthority
Wip_Persistence (C)
Wip_InterruptBehavior (C)
Wip_ReservationIntegrity (C)
```

### III.5.6 Cost model

Read model (3-4 days), accuracy harness (2 days), (C) WIP state (4-5 days
signed).

---

## §III.6 Points 6–10 execution order

```text
Day 1-3   P6 family audit + binds
Day 4-6   P7 knowledge graph + idempotent routing
Day 7-8   P8 gate enforcement + id space
Day 9-10  P9 legibility contract (with W3-06)
Day 11-14 P10 model build + harness (B); WIP deferred to C
Day 15    consolidation + handoffs
```

Cross-links: P7's grants unlock P8's tiers and P1's knowledge-locked recipes;
P9 renders P2's requirement table and P10's chain model; P6's families feed
P10's chains.

---

## §III.7 Whole-plan invariants

```text
1. one recipe authority per family; loaders are registrations, not owners
2. every requirement checked at execution through its owner
3. every material consumed and every byproduct routed
4. knowledge unlocks once; consumers read eligibility
5. stations gate in one id space
6. surfaces render, never compute
7. chains are derived until signed otherwise
8. deterministic yields/decryption
9. no hidden recipes; no dead outputs
```

---

*End of Part III. Continues in Part IV (playbooks).*# W3-05 · PART IV — CRAFTING AUTHORING PLAYBOOKS

---

## §IV.1 Recipe authoring playbook

### IV.1.1 Recipe template

```yaml
recipe: steel_tool_head
family: metallurgy
station: crucible_furnace (tier 2)
knowledge: [basic_smelting, alloy_work]
inputs: {scrap_steel: 4, flux: 1}
utilities: {heat: high_band, power: none, labor: 2h}
outputs: {tool_head: 1}
byproducts: {slag: 2 (facility storage), exhaust: smoke_band (ventilation)}
quality: authored bands (worn | sound | fine)
yield: base 1 (authored variance seeded)
time: 2h
message_refs: {missing_station: craft_msg_no_furnace, missing_knowledge: ...}
```

### IV.1.2 Authoring rules

1. **Inputs from the source graph** — every input has an entry point
   (raw/salvage/purchased/produced); the reachability gate enforces.
2. **Outputs with consumers** — every output has a listed consumer or an
   authored data-only note.
3. **Byproducts assigned** — every stream has an owner disposition (P3
   routing table).
4. **Utilities metered** — power/heat/water/labor declared and consumed.
5. **Quality and yield authored from bands** — no precise hidden numbers.
6. **Message refs required** for each requirement kind (the block copy).
7. **Family and station ids from the owner id spaces** — no invented ids.
8. **Tone** — item names/descriptions follow the corpus (W2-06).

### IV.1.3 Review sheet

```text
[ ] inputs sourced; source chain ≤ authored depth
[ ] outputs consumed or noted
[ ] byproducts routed
[ ] utilities + labor consumed
[ ] requirement message refs present
[ ] id spaces match owners
[ ] tests authored (reachability, flow, gate, yield)
[ ] tone review scheduled
```

### IV.1.4 Recipe anti-patterns

| Anti-pattern | Symptom | Fix |
|---|---|---|
| phantom input | reagent no source | add source or change input |
| free output | no consumption | route inputs |
| invisible byproduct | waste vanishes | assign owner |
| station prose | requirement unchecked | enforce |
| decorative quality | bands unused | wire consumers or remove |
| duplicate recipe | two paths to one output | merge or differentiate |
| orphan knowledge | unlock reads nothing | add consumer or remove |

---

## §IV.2 Station authoring playbook

### IV.2.1 Station template

```yaml
station: glassworks
family: foundry
build_cost: {brick: 20, glass_sand: 10, tools: 1}
power: low continuous
space: 1 bay
tiers:
  1: {unlocks: [basic_glass], speed: 1.0}
  2: {unlocks: [optics_blank], speed: 1.2, power: +1}
  3: {unlocks: [lens_grinding], speed: 1.3, quality: +1 band}
condition: decays without maintenance (authored rate)
crew: 1-2 (skill bands affect speed/quality)
```

### IV.2.2 Rules

1. **Buildable** — station costs come from the source graph; reachability
   gate covers stations.
2. **Tier effects authored** — every tier unlocks or improves something
   measurable.
3. **Condition matters** — decay, maintenance, effects on speed/quality.
4. **Crew skill bands** affect outcomes within authored ranges.
5. **One id space** — station ids match the facility owner.

### IV.2.3 Review sheet

```text
[ ] build cost sourced; station reachable
[ ] tiers have effects; no decorative tier
[ ] condition/maintenance authored
[ ] crew effects authored (bounded)
[ ] id space verified
```

---

## §IV.3 Material flow authoring playbook

### IV.3.1 Flow declaration template

```yaml
process: chlor_alkali
consume: {brine: 10, power: 5, labor: 1h}
produce: {chlorine: 6, caustic_soda: 5, hydrogen: 1}
byproducts: {waste_heat: thermal_band (heat owner),
             trace_chlorine: gas_band (ventilation -> W3-03 exposure)}
meters: {power: owner read, heat: owned band}
```

### IV.3.2 Rules

1. Every consume/produce/byproduct line names an owner.
2. Utility draws metered at the owner (no free power/heat).
3. Hazard byproducts have warning + effect paths (W3-03).
4. Mass balance report run per process (B) / gated (C).
5. The craft ledger records the flow with attribution.

### IV.3.3 Review sheet

```text
[ ] all lines owned
[ ] utilities metered
[ ] hazards routed + warned
[ ] mass-balance report within tolerance or authored deviation
[ ] ledger entry format
```

---

## §IV.4 Foundry policy authoring playbook

### IV.4.1 Policy template

```yaml
policy_trigger: batch_complete
effects:
  labor_strain: +0.05 (W3-03)
  treaty_progress: +1 unit (W3-02 obligation)
  safety_incident: authored probability per heat band (seeded)
  spoilage: if material hot-stored (facility condition)
keys: policy:<batch-id>:batch_complete
ventilation: bound (must reference the existing binding)
```

### IV.4.2 Rules

1. **Once per key**; duplicates logged as findings.
2. **Effects routed**, never applied to local state.
3. **Incidents authored** per heat/condition bands with warnings where
   severe.
4. **Ventilation single-path** — all atmosphere effects through the binding.
5. **Treaty obligations observable** (progress surfaces via W3-02).

### IV.4.3 Review sheet

```text
[ ] keys defined; once-per-key tested
[ ] effects routed to owners
[ ] incidents authored + warned
[ ] ventilation binding referenced
[ ] treaty progress routed
[ ] lifecycle coordination noted (W2-02)
```

---

## §IV.5 Chemistry chain authoring playbook

### IV.5.1 Chain template

```yaml
engine: fischer_tropsch
inputs: {syngas: X, catalyst: Y, power: Z, heat: band}
outputs: {liquid_fuel: N}
hazards: {pressure_event: authored, thermal: band}
yield: authored; quality bands
maintenance: catalyst replacement interval; reactor condition decay
consumers: [fuel uses: vehicles, generators, trade]
```

### IV.5.2 Rules

1. **Reachability** — the engine is reachable through recipes/UI (P1
   classification).
2. **Hazards routed + warned** — exposure/health/thermal all have owners.
3. **Utilities metered.**
4. **Outputs consumed.**
5. **Yield seeded**; quality banded.
6. **Maintenance authored** — no perpetual chemistry.

### IV.5.3 Review sheet

```text
[ ] chain reached; outputs consumed
[ ] hazards routed (health/atmosphere/heat)
[ ] utilities metered
[ ] seeded yield + quality bands
[ ] maintenance cycle authored
[ ] tone/name review (no real-world weapons phrasing)
```

---

## §IV.6 Knowledge authoring playbook

### IV.6.1 Knowledge template

```yaml
knowledge: alloy_work
sources:
  study: {prereq: [basic_smelting], time: 4 days, station: desk}
  salvage: {item: tech_dataslate, chance_band: probable}
  archive: {decryption: medium}
grants:
  recipes: [steel_tool_head, alloy_plate]
  capabilities: [foundry alloy selection]
  dialogue/narrative: [narrative gate refs]
consumers: [recipe loader, facility tier, narrative gate]
```

### IV.6.2 Rules

1. **Source chain** — at least one source reachable (P1-style resolution).
2. **Grants one-time** — keyed idempotency.
3. **Consumers verified** — every grant has a reader; every consumer reads
   eligibility.
4. **Determinism** for chance-based sources (seeded bands).
5. **Naming** — knowledge ids stable and descriptive; display strings from
   the corpus.

### IV.6.3 Review sheet

```text
[ ] ≥1 reachable source
[ ] grants keyed + tested idempotent
[ ] consumers verified (no orphan knowledge)
[ ] seeded chance sources
[ ] narrative grants registered (W3-01)
```

---

## §IV.7 Legibility authoring playbook

### IV.7.1 View blocks

```text
Readiness block:   owner call result (ready/blocked by <kind>)
Requirement rows:  kind, value, satisfied, message ref
Cost block:        items, power, time (owner-sourced)
Output block:      item, quantity, quality bands
```

### IV.7.2 Rules

1. Render, never compute (no UI math).
2. Re-check readiness at execute; the view's word is provisional by design.
3. Every block has a source row in the surface table (W3-06).
4. Block messages are corpus strings; missing refs are findings.
5. Quality shown in bands when variance exists.

### IV.7.3 Review sheet

```text
[ ] blocks sourced
[ ] requirement messages present per kind
[ ] execute re-check verified
[ ] no local arithmetic in the surface
[ ] quality bands shown
```

---

## §IV.8 Crafting anti-pattern catalogue (30)

| # | Anti-pattern | Class |
|---|---|---|
| 1 | phantom input | reachability |
| 2 | free output | flow |
| 3 | invisible byproduct | flow |
| 4 | station prose | gate |
| 5 | decorative tier | station |
| 6 | orphan knowledge | knowledge |
| 7 | repeat unlock | knowledge |
| 8 | local-flag consumer | knowledge |
| 9 | dead family (data only, no loader) | catalog |
| 10 | stranded output | consumer |
| 11 | cycle-dead chain | reachability |
| 12 | unmetered power | flow |
| 13 | hazard-free chemistry | safety |
| 14 | unseeded yield | determinism |
| 15 | quality bands unwired | design |
| 16 | maintenance-free reactor | design |
| 17 | block message missing | legibility |
| 18 | UI math | purity |
| 19 | readiness cache stale | purity |
| 20 | id-space drift | gate |
| 21 | policy double-apply | policy |
| 22 | ventilation bypass | safety |
| 23 | treaty orphan | policy |
| 24 | depth-12 chain | fragility |
| 25 | duplicate recipe | authority |
| 26 | DEBUG item in live catalogue | hygiene |
| 27 | output consumed only by itself | cycle |
| 28 | yield variance unbounded | bounds |
| 29 | station decay absent | fidelity |
| 30 | chemistry names from reality | tone |

---

*End of Part IV. Continues in Part V (verification catalog).*# W3-05 · PART V — VERIFICATION CATALOG AND INTEGRITY GATES

---

## §V.1 Verification tiers

```text
T1 STATIC (seconds)
  reachability map, source/consumer graph, requirement table,
  flow routing table, knowledge graph, id-space check, registry drift
T2 FOCUSED RUNTIME (minutes)
  per-point kits: recipe execution, gate enforcement, flow consumption,
  policy once, chem hazards, unlock idempotency, legibility equality
T3 SOAK (hours, shared)
  20 seeds × 60 days: production chains, knowledge progression,
  foundry policy, chemistry maintenance, station decay
```

---

## §V.2 T1 — static checks

| Check | Input | Detects | Output |
|---|---|---|---|
| C1.1 reachability | catalogs + sources | classified recipes | class + reason |
| C1.2 source graph | item chains | cycle-dead, deep chains | loop/item list |
| C1.3 consumer graph | output reverse | OUTPUT-DEAD | item list |
| C1.4 requirement table | recipe declarations | paper/unchecked | recipe, kind |
| C1.5 flow routing | byproduct declarations | unowned streams | stream list |
| C1.6 knowledge graph | sources/grants/consumers | orphans, cycles | knowledge id |
| C1.7 id spaces | stations/knowledge ids | drift | mismatched ids |
| C1.8 message refs | block reasons | missing copy | recipe, kind |
| C1.9 registry drift | generated docs | stale registries | diff |

### V.2.1 Generated artifacts

```bash
# illustrative; generators are deliverables of this package
bash scripts/ci/generate-craft-reachability.sh --check
bash scripts/ci/generate-knowledge-graph.sh --check
bash scripts/ci/craft-requirements-check.sh
bash scripts/ci/craft-flow-check.sh
```

### V.2.2 Ratchet policy

Unreachable/stranded findings start in a counted baseline that may not grow;
each repair lowers it. The gate fails on growth, not on historical debt.

---

## §V.3 T2 — focused kits (case level)

### V.3.1 Recipe execution kit

```text
KR.1 every family: a representative recipe crafts end-to-end
KR.2 inputs consumed from owners exactly once
KR.3 outputs land in an owner (inventory/facility)
KR.4 byproducts routed per table
KR.5 utilities metered
KR.6 yield within authored bands across N=20 runs (seeded)
KR.7 quality band follows station/labor bands
```

### V.3.2 Gate kit

```text
KG.1 blocked without station (reason ref present)
KG.2 blocked without knowledge
KG.3 blocked without power (required)
KG.4 tier gating per authored unlocks
KG.5 id space single (station resolved from owner)
KG.6 TOCTOU: state change between check and execute handled per authored
```

### V.3.3 Flow kit

```text
KF.1 consume-once scan (ledger math)
KF.2 double-consume detection (two owners)
KF.3 byproduct owner write present
KF.4 utility draws recorded
KF.5 ventilation binding present for exhaust streams
```

### V.3.4 Policy kit

```text
KP.1 batch policy applies once per key
KP.2 duplicate application logged as finding
KP.3 effects routed (strain/obligation/incident)
KP.4 incidents warned where authored
KP.5 treaty progress observable
```

### V.3.5 Chemistry kit

```text
KC.1 engine reached via a recipe/UI path
KC.2 outputs consumed
KC.3 hazards routed (health/atmosphere/heat)
KC.4 utilities metered
KC.5 maintenance intervals authored + tested
KC.6 seeded yield replay
```

### V.3.6 Knowledge kit

```text
KK.1 source grants (each source type)
KK.2 idempotent second acquire
KK.3 consumers read eligibility (sample)
KK.4 no orphan knowledge
KK.5 cycle-free graph
KK.6 seeded chance sources
```

### V.3.7 Station kit

```text
KS.1 build costs consumed
KS.2 tier effects measurable (speed/quality/unlock)
KS.3 condition decay + maintenance
KS.4 crew skill bands affect outcomes
```

### V.3.8 Legibility kit

```text
KL.1 readiness equals execution (no lies)
KL.2 block reasons per kind (copy present)
KL.3 costs owner-sourced
KL.4 quality bands shown when variable
KL.5 no surface arithmetic
```

### V.3.9 Chain model kit

```text
KM.1 model is read-only (no writes)
KM.2 bottleneck accuracy vs. soak
KM.3 output estimate accuracy (± tolerance)
KM.4 no second estimator (single model authority)
KM.5 (C) WIP reservation integrity
```

---

## §V.4 T3 — soak design

### V.4.1 Scenario

```text
20 seeds × 60 days
scripted: build stations, craft chains, acquire knowledge, run foundry
measure:
  chain outputs per day vs. model projections
  bottlenecks vs. authored expectations
  policy applications vs. keys
  hazard events + warnings
  station condition curves
  knowledge progression timelines
assertions:
  no free outputs (ledger)
  no duplicate policy applications
  no unowned byproducts
  hazard events warned (log)
  maintenance cycles honored (condition within bands)
  deterministic yields per seed
```

### V.4.2 Report format

```yaml
run: T3-2027-01-a
seeds: 20 days: 60
chains: {steps_tracked: 14, output_error_max: 0.18, bottleneck_matches: 20}
flow: {free_outputs: 0, unowned_byproducts: 0, double_consumes: 0}
policy: {applications: 340, duplicates: 0, incidents: 6, warned: 6}
knowledge: {acquired: 41, idempotent: 41, orphans: 0}
stations: {decay_within_bands: 20, maintenance_events: 55}
determinism: pass
```

---

## §V.5 Evidence and closeout

```text
docs/evidence/w3-05/
  T1-*.yaml T2-*.yaml T3-*.yaml
  findings/CR-###.md
  repairs/
  P0_CRAFT_PREMISE.md
```

Closeout acceptance:

```text
[ ] reachability baseline recorded; gate green
[ ] requirement enforcement verified per kind
[ ] flow ownership complete; mass-balance report filed
[ ] foundry policy once; ventilation single path
[ ] chemistry chains reached; hazards routed
[ ] catalogs bound (or data-only noted)
[ ] knowledge idempotent; no orphans
[ ] gates enforced; tiers effective
[ ] legibility truthful
[ ] chain model accurate; (C) WIP signed
```

---

## §V.6 Failure triage

```text
free output              -> stop: flow authority broken
unowned byproduct        -> fix (owner fan-out)
paper requirement        -> fix (enforce)
policy double-apply      -> fix (key)
hazard unrouted          -> fix (safety)
repeat unlock            -> fix (idempotency)
id-space drift           -> fix (owner ids)
surface lie              -> fix (purity)
chain model error        -> recalibrate/review
flake                    -> quarantine per repo policy
```

---

## §V.7 Test naming and focus

```text
Recipe_<Check>      Requirements_<Check>
Flow_<Check>        Foundry_<Check>
Chem_<Check>        Knowledge_<Check>
Station_<Check>     Legibility_<Check>
Chain_<Check>       Wip_<Check> (C)
```

---

## §V.8 Cost model

| Tier | Effort |
|---|---|
| T1 generators + checks | 5-6 days |
| T2 kits (9) | 9-11 days |
| T3 soak instrumentation | 3-4 days |
| Closeout | 1-2 days |

---

*End of Part V. Continues in Part VI (worked case studies).*# W3-05 · PART VI — WORKED CASE STUDIES

> Three end-to-end threads through the production stack, with quantified
> findings and repairs: the ore-to-tool chain, the chlorine incident, and the
> archive campaign. Illustrative IDs; verified at P0.

---

## §VI.1 Case study 1 — Ore to tool (the full chain)

### VI.1.1 The chain

```text
scavenge ore/scrap -> crucible smelt -> steel ingot -> forge part -> tool head
                   -> haft (wood) -> assembly -> finished tool -> consumer
```

Crosses: `CrucibleFoundryCatalog`, `PowderMetallurgySystem`,
`HydraulicExtrusionEngine` (bars), `ShelterWorkshopSystem` (assembly),
inventory, condition system (finished tool condition), W3-02 (trade).

### VI.1.2 Reachability trace

| Step | Classification | Evidence |
|---|---|---|
| ore/scrap | REACHABLE | salvage tables + ruin spawns |
| flux | INPUT-DEAD (initial) | declared, no source |
| crucible furnace | STATION-LOCKED (initial) | build cost included flux |
| steel ingot | REACHABLE after flux repair | recipe chain |
| forge part | REACHABLE | workshop tier 2 |
| haft | REACHABLE | wood harvest |
| assembly | REACHABLE | workshop tier 1 |
| finished tool | OUTPUT: consumed by condition/equipment + trade | verified |

### VI.1.3 Findings

| # | Finding | Class | Repair |
|---|---|---|---|
| CP-01 | flux had no source (INPUT-DEAD) — blocked the entire chain | reachability | add flux source (salt/ash process) or substitute lime (authored) |
| CP-02 | crucible furnace build cost referenced flux, deepening the dead chain | reachability | build cost revised to sourced materials |
| CP-03 | smelting exhaust stream had no owner | flow | bind to ventilation |
| CP-04 | slag accumulated with no disposition (silent growth) | flow | facility storage + periodic clearing task |
| CP-05 | extrusion bars had no consumer until forging (fine) but recipe menu showed them as craftable with a dead consumer flag | consumer graph | consumer graph corrected |
| CP-06 | finished tool quality bands existed but condition system ignored them (all tools equivalent) | quality | condition initial band from quality |
| CP-07 | smelting power draw declared but unmetered (free heat) | flow | meter via heat owner |
| CP-08 | deep chain depth 7 flagged for review; reduced to 6 by merging ingot/powder steps (authored) | fragility | chain simplified |

### VI.1.4 The chain model output (post-repair)

```text
bottleneck: crucible throughput (1 batch/day) — reported accurately
output estimate: 0.8 tools/day steady (with labor 1); model error vs. soak: 12%
days-to-starve on scrap: 9 (range 6-13) — matches soak within tolerance
```

### VI.1.5 What the case teaches

1. **One dead input kills an entire industry** (CP-01/02) — the reachability
   map's highest-value catch.
2. **Byproduct silence is structural** (CP-03/04): foundries were designed as
   producers only; flow ownership must be explicit.
3. **Quality bands are promises** (CP-06): if authored, they must be consumed
   somewhere.
4. **Chains have a sweet spot** (CP-08): depth > 6 is fragile and slow to
   play.

---

## §VI.2 Case study 2 — The chlorine incident

### VI.2.1 Setup

The chlor-alkali engine runs for an in-game week to produce caustic soda for
soap. Trace chlorine is a declared byproduct. A ventilation fault (condition
band) develops mid-week.

### VI.2.2 Expected sequence

```text
day 0  engine start: brine + power consumed; outputs pooled
       byproduct: trace chlorine -> ventilation concentration band
day 1-3 nominal: concentration low; no events
day 4  ventilation condition decays (maintenance skipped) -> band rises
day 5  authored warning: workers cough (bark); a scene hook available
       player options: shut down, repair ventilation, ignore
day 5  if ignored: exposure event (W3-03 trauma/exposure class) + health hit
day 6  incident scene: the shelter reacts (W3-01); care routes (W3-03)
```

### VI.2.3 Findings

| # | Finding | Class | Repair |
|---|---|---|---|
| CC-01 | trace chlorine had no concentration model (band absent) | flow | authored band + accumulation rule |
| CC-02 | the warning bark had no trigger (event never fired) | warning | wire to band threshold with lead time |
| CC-03 | "shut down" option didn't stop accumulation (residual gas lingered — correct in fiction, but unmodeled) | fidelity | authored decay of concentration after shutdown |
| CC-04 | exposure routed to health but not trauma (a survivor witnessed a collapse) | cross-plan | W3-03 trigger ref added |
| CC-05 | ventilation repair consumed no parts (free fix) | routing | W3-05 materials |
| CC-06 | the incident had no authored scene (silent health hit) | dignity/visibility | scene authored, journal entry |
| CC-07 | chlorine names in data used a real-world industrial label with a warning-acronym | tone | rewrite per W2-06 tone |

### VI.2.4 Tests added

```text
Chem_ChlorineConcentrationBands
Chem_VentilationFaultWarning
Chem_ShutdownDecay
Chem_ExposureRoutes_HealthAndTrauma
Chem_RepairConsumesMaterials
Chem_IncidentSceneRegistered
```

### VI.2.5 What the case teaches

1. **Chemistry needs time models** (CC-01/03): concentration and decay, not
   binary present/absent.
2. **Warnings are wiring** (CC-02): a bark with no trigger is decoration.
3. **Cross-plan routing is part of chemistry** (CC-04): exposure is not only
   a health number; it is an event people witnessed.
4. **Tone is a data concern** (CC-07): industrial labels carry real-world
   weight; fictional language keeps the game's world its own.

---

## §VI.3 Case study 3 — The archive campaign

### VI.3.1 Setup

Three prewar archives exist; the player pursues knowledge through decryption.
Chain: find archive -> decrypt (time + luck band) -> grant knowledge ->
unlock recipes/tiers -> production expansion.

### VI.3.2 Expected sequence

```text
week 0  archive A located (expedition, W2-05 location)
week 1  decryption attempt: medium band; partial success (fragment)
week 2  fragment grants prereq knowledge; full archive grants alloy_work
        recipes unlocked (steel_tool_head, alloy_plate)
week 3  archive B: chance-based; unseeded roll initially (finding)
week 4  tier 2 crucible unlocked; chain output expands
week 5  archive C: dead source (its knowledge already granted elsewhere)
```

### VI.3.3 Findings

| # | Finding | Class | Repair |
|---|---|---|---|
| CA-01 | decryption used unseeded randomness | determinism | seeded bands |
| CA-02 | re-decrypting archive A re-granted recipes (double unlock) | idempotency | keyed grants |
| CA-03 | archive C's knowledge had a second source; the graph showed a cycle | graph | cycle resolved (source deduped) |
| CA-04 | fragment knowledge had no displays (menu showed raw ids) | legibility | display strings registered |
| CA-05 | unlocked tier 2 had no measurable speed effect | station | tier effect authored |
| CA-06 | knowledge C's consumer was a dialogue gate only (no gameplay effect) — legitimate but marked data-only | consumer | authored note |
| CA-07 | decrypt progress was invisible between attempts (uncertainty monotony) | visibility | progress/notes authored |

### VI.3.4 The knowledge graph outcome

```text
nodes: 38 knowledge ids; edges: sources 57, grants 61, prereqs 44
cycles: 0 (after CA-03)
orphans: 0 (after C6 classification)
baseline: 2 data-only nodes (authored notes)
```

### VI.3.5 What the case teaches

1. **Determinism applies to scholarship too** (CA-01).
2. **Grants are effects** (CA-02): unlocking is a consequence and must be
   once.
3. **Graphs reveal silent duplicates** (CA-03): two sources for one knowledge
   is usually an authoring accident.
4. **Unlocks must open something** (CA-05): a tier that changes nothing is
   theft from the player.

---

## §VI.4 The three-case coverage matrix

| Case | P1 | P2 | P3 | P4 | P5 | P6 | P7 | P8 | P9 | P10 |
|---|---|---|---|---|---|---|---|---|---|---|
| ore-to-tool | ● | ● | ● | ● | | | | ● | ● | ● |
| chlorine | | ● | ● | ● | ● | | | | ● | |
| archive | ● | | | | | ● | ● | ● | ● | ● |

Every point is exercised by at least one case; all three together touch all
ten.

---

## §VI.5 The seeded findings catalogue (from the cases)

| # | Finding | Kit |
|---|---|---|
| CP-01..08 | chain reachability/flow/quality | T1 + KR/KF |
| CC-01..07 | chemistry time models/hazards/warnings | KC + KL |
| CA-01..07 | knowledge determinism/idempotency/graphs | KK + KS |

---

*End of Part VI. Continues in Part VII (Q&A and operational model).*# W3-05 · PART VII — EXTENDED Q&A AND OPERATIONAL MODEL

---

## §VII.1 Governance Q&A (Q1–Q10)

**Q1. Who owns recipes?**
The loaders register data; the crafting/foundry/engine owners execute.
Loaders are not authorities; the owners are.

**Q2. Can a builder add a recipe family?**
Yes: catalog + loader registration + reachability classification + flow
routing + gate enforcement + tests. New families enter the map on the next
generation.

**Q3. Does Path B change saves?**
No: all state lives in existing owners. C-path WIP is signed per item.

**Q4. What if an owner is missing for a station kind (e.g., no heat owner)?**
P0 finding; Path B proposes the minimal binding with a signed line, or the
requirement becomes a documented authored exception (never a silent pass).

**Q5. How is mass balance handled at B?**
As a report; deviations are findings (data error or hidden consumer). The
gate arrives at C.

**Q6. Do we touch balance values?**
No: yield/quality/time are authored values; measurement goes to W2-03. The
plan audits truth, not tuning.

**Q7. How is tone handled for process names?**
Corpus review (W2-06); industrial real-world labels are rewritten into the
game's fictional register. The chemistry names from reality are rejected
(anti-pattern 30).

**Q8. What about modding/expansion recipes?**
They enter as data; the gates apply the same. No special path.

**Q9. How does the plan avoid becoming a factory sim?**
Bounded scope: recipes/stations/flows are audited and bound; no new
automation systems; the chain model is read-only until signed.

**Q10. What is the smallest Path A?**
P0 + reachability map + source/consumer graphs + requirement/flow
declaration audits. No repairs.

---

## §VII.2 Method Q&A (Q11–Q25)

**Q11. Why does reachability start with sources, not recipes?**
Because source resolution is the expensive, high-yield half; recipes
classify quickly once sources are known.

**Q12. Why is a cycle without a raw entry "dead"?**
Because it cannot be started in-game; a loop with no seed material is
production fiction (the loop itself is detected and reported).

**Q13. Why is OUTPUT-DEAD treated at the same severity as INPUT-DEAD?**
A craftable item with no consumer is a content hole that wastes player time;
it is silent to designers unless the reverse graph is built.

**Q14. Why must byproducts have owners?**
Because otherwise "waste" is a fifth element that vanishes; in a survival
game, waste should exist, cost, and matter (or be authored away).

**Q15. Why does the policy key include batch id?**
So repeat triggers (shift end + batch complete) don't double-apply; the key
encodes the identity of the occurrence.

**Q16. Why is TOCTOU re-checked at execution?**
Because the world changes between menu and craft; a checked-then-lost world
must not produce free outputs.

**Q17. Why bands for quality?**
Precision implies simulation depth the game doesn't have; bands keep author
control and prevent false precision.

**Q18. Why chain depth limits?**
Deep chains are fragile (one dead link kills all), slow to play, and hard to
verify; the report allows review, not a hard ban.

**Q19. Why is legibility a truth test, not a design test?**
Because the surface either equals execution or it lies; there is no aesthetic
of lying.

**Q20. Why single id spaces?**
Two id spaces for one station is how gates silently fail; the reconciliation
is mandatory.

**Q21. Why does the knowledge graph include narrative grants?**
Because an unlock that only opens dialogue is still a consumer; marking it
data-only or narrative keeps orphans honest.

**Q22. What about recipes that exist for trade only (no local use)?**
Legitimate: the consumer is the market (W3-02). The graph lists it.

**Q23. What is a "documented authored exception"?**
A gate deviation recorded with owner, reason, and expiry; visible, never
silent.

**Q24. How are discovery/projection companions (milling) handled?**
As producers of discovery state; their consumers are checked like any other
(what reads the discovered recipe/projection?).

**Q25. How do we keep audits incremental?**
Registries regenerate and diff; new content classifies against existing
graphs; only changed families re-audit.

---

## §VII.3 Tooling Q&A (Q26–Q35)

**Q26. New tooling?**
Reachability generator, source/consumer graph, requirement checker, flow
checker, knowledge graph generator, registry suite.

**Q27. Reused tooling?**
Catalog registry family, content-acceptance gates, soak harness, evidence
writer, surface purity checks.

**Q28. How big is the map?**
Hundreds of recipes — the generated table is large but mechanical; authored
columns stay small (reasons/notes).

**Q29. Where do displays live?**
Block messages and item strings in the corpus (W2-06); ids stay stable.

**Q30. How is the graph kept from drifting?**
`--check` in CI; drift fails the focused pipeline for crafting data changes.

**Q31. Can the reachability resolver time out?**
Bounded depth + memoized source resolution; worst-case is linear per item.

**Q32. How are seeded yields expressed?**
Authored bands + seeded selection; the test replays N=20 and asserts
distribution bounds.

**Q33. What evidence per repair?**
before/after classification or flows, command, HEAD, owner, reviewer.

**Q34. How are hazard events tested?**
Scripted ventilation decay -> threshold -> warning -> event; assert order and
routing.

**Q35. How do we verify "no UI math"?**
Surface source table + grep for arithmetic on requirement/cost fields.

---

## §VII.4 Content Q&A (Q36–Q45)

**Q36. What if a recipe is intentionally obscure (rare knowledge)?**
Authored rarity with a signpost (W3-01); reachability still required; rarity
is not unknowability.

**Q37. Can an output be consumed only by another rare recipe?**
Yes; the chain is followed to a final consumer or the terminal is marked
data-only with a reason.

**Q38. What about decorative crafted items (trophies)?**
Legitimate if authored: the consumer is the shelter surface/memorial
(narrative). The note is explicit.

**Q39. Fungible vs. unique items in flows?**
Unique items (author artifacts) are excluded from crafting flows by design;
the exclusion list is authored.

**Q40. Can recipes require other crafted items cyclically as catalysts?**
A catalyst loop is permitted only with a raw entry; otherwise the
cycle-dead rule applies.

**Q41. How are salvage outputs handled?**
Salvage tables are sources; their yields enter the source graph as
`salvage` entries. Dead salvage outputs are OUTPUT-DEAD findings.

**Q42. What is the policy when a repair changes a recipe's inputs?**
The change goes through the registry update; existing saves are unaffected
(recipes are data); the change is recorded in the version log.

**Q43. How are "capability" grants (alloy selection) modeled?**
As knowledge-granted capabilities read by the relevant owner; the consumer
check covers them.

**Q44. Do stations need tie-ins with shelters (space)?**
Yes where authored; space consumption is a requirement kind read from the
shelter owner.

**Q45. What tone for failure copy?**
Plain, in-world, never mocking: "the furnace is cold", not "error 42".

---

## §VII.5 Operational model

### VII.5.1 Staffing

| Role | Count | Responsibility |
|---|---|---|
| systems engineer | 1 | graphs, checks, flow contract |
| content designer | 1 | recipes, stations, flows, knowledge per playbooks |
| verifier | shared | kits/soak/evidence |
| W2-06 liaison | part-time | strings/tone |

### VII.5.2 Schedule (5-6 weeks)

```text
Week 1  P0 + P1 source/consumer graphs
Week 2  P1 map + repairs; P2 requirements
Week 3  P3 flows; P4 foundry
Week 4  P5 chemistry; P6 catalogs
Week 5  P7 knowledge; P8 gates; P9 legibility
Week 6  P10 model; soak; closeout
```

### VII.5.3 Cadence and handoffs

```text
daily: T1 checks; twice weekly: findings triage
weekly: registry regeneration; soak spot-run
handoffs: W3-02 (outputs/prices), W3-04 (repairs/materials), W3-01
  (knowledge narrative grants), W3-06 (surfaces), W2-04 (atmosphere/heat)
```

---

## §VII.6 Re-execution notes

Registries regenerate; graphs diff; changed families re-audit. New
expansions (27–31) enter the map immediately; their reachability is the
expansion's gate to release.

---

*End of Part VII. Continues in Part VIII (C-path, matrices, appendices).*# W3-05 · PART VIII — C-PATH DESIGNS, MATRICES, AND APPENDICES

---

## §VIII.1 C-path designs

### VIII.1.1 C1 — Production planning board

A read-only board over the chain model: what can the shelter produce today,
what's blocked and why, and what the next unlock would change. Pure
projection of owners; surfaces by W3-06. Cost ~3 days.

### VIII.1.2 C2 — Work-in-progress at stations

The signed WIP state (P10): batches occupy stations, reserve inputs, carry
quality seeds, and pause per interruptions. Enables scheduling and
pipelining. Persistence via the crafting owner's section; round-trip tests.
Cost ~4-5 days.

### VIII.1.3 C3 — Apprentice and mastery

Crafters accumulate authored mastery bands per family through repetition
(bounded), improving speed/quality/yield within bands. Stored on the
survivor/work owner; consumers read bands. Cost ~4 days.

### VIII.1.4 C4 — Tool quality ecology

Tool condition (W3-04 P2) and crafted quality compose into a tool-quality
ecology: better tools → better outputs → better tools. Bounded and authored;
no runaway (diminishing/caps). Cost ~3 days.

### VIII.1.5 C5 — Salvage forensics

Salvage yields become knowledge-bearing: dissecting a relic can grant
partial knowledge (dismantle → study → fragment) through the knowledge graph.
Cost ~4 days, coordinates W3-01.

### VIII.1.6 C6 — Industrial hazards doctrine

Authored safety doctrine choices (speed vs. safety) modifying incident rates
and warnings through existing owners. Cost ~3 days.

### VIII.1.7 C7 — Trade-grade production

Authored quality grades for export (W3-02 pricing reads grades), letting
production choices aim at markets. Interface documented; no new economy
state. Cost ~3 days.

### VIII.1.8 C-bundle recommendation

```text
first: C2 WIP + C1 board (the gameplay core)
then:  C3 mastery + C4 tool ecology (depth)
then:  C5 salvage forensics (discovery)
last:  C6 hazards + C7 trade grades
```

### VIII.1.9 C signature block

```text
[ ] C1 planning board   [ ] C2 WIP state     [ ] C3 mastery
[ ] C4 tool ecology     [ ] C5 salvage       [ ] C6 hazards doctrine
[ ] C7 trade grades
```

---

## §VIII.2 Interaction matrix

| Receiver | Interface | Direction |
|---|---|---|
| W3-02 economy | outputs as market goods; quality bands; prices | out |
| W3-02 economy | input prices at craft execution | in |
| W3-04 combat | repair recipes; ordnance/tool conditions | out |
| W3-01 narrative | knowledge grants; archive scenes; item lore | out/in |
| W3-03 psychology | labor strain; hazard exposure; care supplies | out |
| W3-06 UI | craft surfaces; blocked reasons; chain board | out |
| W2-02 repair | foundry lifecycle coordination | coordinate |
| W2-03 balance | yield/quality measurements | out |
| W2-04 environment | atmosphere/heat/ash interfaces | out/in |
| W2-06 prose | item/process strings; tone | out |

### VIII.2.1 Never-cross list

```text
[ ] never a second recipe authority
[ ] never free outputs or vanishing waste
[ ] never unchecked requirements
[ ] never repeat unlocks
[ ] never UI math on costs/requirements
[ ] never a second chain estimator
[ ] never save sections without signature
[ ] never real-world process labels in the fictional register
```

---

## §VIII.3 Expanded glossary

| Term | Definition |
|---|---|
| byproduct stream | declared waste/heat/gas with an owner |
| chain depth | number of steps from raw to final |
| consumer graph | reverse map output → consumers |
| cycle-dead | production loop with no raw entry |
| data-only | catalog marked for future content (authored note) |
| flow contract | consume/produce/byproduct/utility ownership |
| knowledge graph | sources/grants/prereqs/consumers map |
| mass balance | inputs ≈ outputs + byproducts within tolerance |
| message ref | authored copy for a blocked requirement |
| meter | utility draw recorded at its owner |
| paper gate | station requirement declared but unchecked |
| quality band | authored output quality class |
| ratchet | counted baseline that may not grow |
| reachability | craftable end-to-end in a campaign |
| requirement kind | knowledge/station/tool/power/heat/environment/labor/quantity |
| source chain | item's path back to a raw entry point |
| station tier | authored upgrade level with effects |
| TOCTOU | check-then-execute state change |
| WIP | work-in-progress at a station (C) |
| yield band | seeded output quantity range |

---

## §VIII.4 Artifact index

| Artifact | Type |
|---|---|
| `docs/crafting/RECIPE_REACHABILITY.md` | generated |
| `docs/crafting/SOURCE_CHAINS.md` | generated |
| `docs/crafting/OUTPUT_CONSUMERS.md` | generated |
| `docs/crafting/REQUIREMENTS.md` | generated |
| `docs/crafting/FLOW_ROUTING.md` | authored table |
| `docs/crafting/FOUNDRY_COHESION.md` | audit doc |
| `docs/crafting/CHEM_CHAINS.md` | authored table |
| `docs/research/KNOWLEDGE_GRAPH.md` | generated |
| `docs/crafting/CHAIN_MODEL.md` | authored spec |
| `docs/evidence/w3-05/*` | evidence |

---

## §VIII.5 Expanded signature sheet

```text
ASHFALL WAVE 3 · PLAN 5 (CRAFTING) · EXECUTION SIGNATURES
HEAD: ________  Date: ________  Foreman: ________

[ ] P0 premise + P1 reachability graphs
[ ] P1 repairs (cap: ___)
[ ] P2 requirement taxonomy + enforcement
[ ] P3 flow contract + byproduct routing
[ ] P4 foundry cohesion + policy once + ventilation
[ ] P5 chemistry chains + hazard routing
[ ] P6 catalog binds (list data-only exceptions)
[ ] P7 knowledge idempotent routing
[ ] P8 station gates + tiers
[ ] P9 legibility contract (with W3-06)
[ ] P10 chain model (B)   (C WIP: [ ] no [ ] signed)
[ ] C3 mastery  [ ] C5 salvage  [ ] C7 trade grades (signed individually)

Retained: no free outputs; no paper gates; no second authority.
```

---

*End of Part VIII. Continues in Part IX (checklists, sketches, defaults).*# W3-05 · PART IX — IMPLEMENTATION CHECKLISTS, SKETCHES, AND DEFAULT PARAMETERS

---

## §IX.1 Point 1 checklist — reachability

```text
[ ] enumerate all catalogs + loaders (registry)
[ ] build source graph (raw/salvage/purchased/produced)
[ ] build consumer graph (recipe/build/award/economy/narrative/tool)
[ ] cycle detection with raw-entry rule
[ ] chain-depth report (threshold authored)
[ ] classify all recipes into the six classes
[ ] publish the map + baseline; wire --check
[ ] rank repairs (cap per phase); reasons for every archive
```

## §IX.2 Point 2 checklist — requirements

```text
[ ] taxonomy mapping per requirement kind -> owner API
[ ] audit every declaration for a real read
[ ] enforce at execution (CanCraft + re-check)
[ ] message refs per kind (corpus)
[ ] TOCTOU behavior authored per class
[ ] tests: each kind blocked/allowed
```

## §IX.3 Point 3 checklist — flows

```text
[ ] consume/produce/byproduct/utility ownership on every process
[ ] byproduct routing table (streams -> owners)
[ ] utility metering (power/heat/water)
[ ] ventilation binding for exhaust
[ ] craft ledger + attribution
[ ] mass-balance report (B) / gate (C)
```

## §IX.4 Point 4 checklist — foundry

```text
[ ] partial cohesion map; overlaps resolved/documented
[ ] policy keys (once per trigger occurrence)
[ ] ventilation single path confirmed
[ ] lifecycle coordination with W2-02 (D19c)
[ ] treaty labor wiring (obligations observable)
[ ] incident authoring with warnings
```

## §IX.5 Point 5 checklist — chemistry

```text
[ ] chain table per engine; reachability
[ ] hazard routing (health/atmosphere/heat) + warnings
[ ] utilities metered; maintenance cycles
[ ] seeded yield + quality bands
[ ] stranded outputs checked
```

## §IX.6 Point 6 checklist — catalogs

```text
[ ] loader registration per family
[ ] recipes classified; stations buildable; outputs consumed
[ ] surfaces list the recipes (W3-06 check)
[ ] data-only exceptions documented with reasons
```

## §IX.7 Point 7 checklist — knowledge

```text
[ ] graph generated (sources/grants/prereqs/consumers)
[ ] cycle + orphan checks
[ ] idempotent grants (keyed)
[ ] seeded chance sources
[ ] consumers read eligibility
```

## §IX.8 Point 8 checklist — stations

```text
[ ] id space reconciliation
[ ] gates enforced at execution with reasons
[ ] tier effects measurable
[ ] condition decay + maintenance
[ ] crew skill bands bounded
```

## §IX.9 Point 9 checklist — legibility

```text
[ ] view contract (readiness/requirements/costs/output)
[ ] render-only (no math) + source table
[ ] re-check at execute
[ ] messages per kind
[ ] quality bands shown
```

## §IX.10 Point 10 checklist — chain model

```text
[ ] read-only model (analysis)
[ ] bottleneck + estimates with ranges
[ ] accuracy harness
[ ] no second estimator
[ ] (C) WIP signature + reservation integrity
```

---

## §IX.11 Reference sketch — reachability resolver (illustrative)

```text
Classify(recipe):
    for k in recipe.knowledge:   if !KnowledgeReachable(k): return KNOWLEDGE_LOCKED
    for s in recipe.stations:    if !StationReachable(s):  return STATION_LOCKED
    for i in recipe.inputs:      if !InputReachable(i, depth=0): return INPUT_DEAD
    for o in recipe.outputs:     if !OutputConsumed(o):    return OUTPUT_DEAD
    return REACHABLE

InputReachable(item, depth):
    if depth > MAX_DEPTH: report(DEEP, item); return true (review)
    if Raw(item) || Salvage(item) || Purchased(item): return true
    for r in Producers(item):
        if Producing(r) && All(InputReachable(x, depth+1) for x in r.inputs):
            return true
    return false      # cycle with no raw entry ends here

OutputConsumed(item):
    return Consumers(item).nonEmpty || AuthoredTerminal(item)
```

## §IX.12 Reference sketch — flow execution (illustrative)

```text
Execute(recipe, actor, station):
    blocked := CanCraft(recipe, actor, station)   # re-check
    if blocked: return Blocked(blocked)

    consume := inventory.TryConsume(recipe.inputs, actor)  # once
    if !consume.ok: return Blocked(consume.reason)

    utilities.Commit(recipe.utilities)            # metered
    schedule.Spend(recipe.labor, actor)

    produced := facility.Produce(recipe.outputs, station)
    for stream in recipe.byproducts:
        stream.Owner.(stream.kind).Deliver(stream.quantity)

    ledger.Record(recipe, actor, day, consume, produced, recipe.byproducts)
    return Done
```

## §IX.13 Reference sketch — knowledge grant (illustrative)

```text
AcquireK(id, source):
    if research.Has(id): log NoOp; return
    research.Grant(id)
    key := "knowledge:" + id
    if ledger.Seen(key): log Duplicate; return
    for g in grants(id):
        g.ApplyOnce(key)          # recipes, capabilities, gates
    ledger.Mark(key); ledger.RecordSource(id, source)
```

---

## §IX.14 Default parameter tables (proposals)

### IX.14.1 Chain depth

| Parameter | Value | Note |
|---|---|---|
| review threshold | 6 steps | beyond → simplify/review |
| hard cap | 9 steps | beyond → redesign |
| resolve depth | 12 | resolver budget |

### IX.14.2 Yield bands (proposal)

| Process class | Base | Variance | Quality bands |
|---|---|---|---|
| smelting | 1 | ±10% seeded | worn/sound/fine |
| chemistry | authored | ±15% seeded | standard/refined |
| milling | 5 | ±1 seeded | feed/flour |
| glass | 1 | ±20% (breakage) | rough/clear/fine |

### IX.14.3 Byproduct dispositions

| Stream | Owner | Cadence |
|---|---|---|
| slag | facility storage | clear when full |
| exhaust | ventilation | continuous band |
| ash | atmosphere | event-driven |
| cullet | inventory | reusable |
| brine waste | facility + health risk | disposal task |

### IX.14.4 Policy timing

| Trigger | Key scope | Notes |
|---|---|---|
| batch_complete | per batch | once |
| shift_end | per shift | once |
| incident | per incident | once |
| treaty_delivery | per delivery | once |

---

*End of Part IX. Continues in Part X (field guide and final control).*# W3-05 · PART X — FIELD GUIDE, MAINTENANCE, AND FINAL CONTROL

---

## §X.1 Field guide: "I can't craft this"

```text
1. Is the recipe REACHABLE?          map -> classification + reason
2. Knowledge?                        research eligibility reads
3. Station?                          facility owner; tier; id space
4. Tools/condition?                  equipment owner; broken tool?
5. Inputs?                           inventory; source chain live?
6. Power/heat?                       utility owners; metered draw
7. Environment?                      ventilation/cooling bindings
8. Labor/schedule?                   roster; effort available
9. Message shown?                    block reason ref exists?
10. Execute re-check?                TOCTOU behavior authored?
```

## §X.2 Field guide: "I crafted this and nothing changed"

```text
1. Outputs land in an owner?         facility tray/inventory
2. Consumed inputs actually deducted?
3. Byproducts routed (no backpressure)?
4. Ledger entry present?             attribution
5. Policy triggers fired once?       keys
6. Output consumed?                  reverse graph
7. Quality/yield bands applied?      station/labor inputs
```

## §X.3 Field guide: "the chain stalls"

```text
1. Bottleneck from the model?        named step
2. Input starved?                    source chain / caravan
3. Station down?                     condition/maintenance
4. Labor missing?                    roster
5. Utility starved?                  power/heat
6. Policy blocking?                  rationing/treaty priority
7. WIP stuck? (C)                    interruption behavior
```

## §X.4 Field guide: "chemistry scared me"

```text
1. Hazard routed?                    health/atmosphere/heat owners
2. Warnings fired?                   band thresholds with lead
3. Concentration model?              accumulation + decay
4. Maintenance authored?             intervals enforced
5. Scene authored for incidents?     visibility/dignity
6. Tone pass?                        fictional labels only
```

---

## §X.5 Maintenance calendar

```text
per content change:
  [ ] new recipes classified; inputs sourced; outputs consumed
  [ ] requirements declared; message refs present
  [ ] flows owned; utilities metered
  [ ] knowledge grants keyed
weekly:
  [ ] registry --check; baseline review
  [ ] soak spot-run; flow assertions
monthly:
  [ ] chain model accuracy re-sample
  [ ] station decay curves re-check
  [ ] policy key audit (duplicate log)
per release:
  [ ] T3 full; findings triage
  [ ] expansion families (27-31) reachability as a release gate
```

---

## §X.6 Escalation map

| Finding | Owner | Escalation |
|---|---|---|
| free output | engineer | stop-the-line |
| unowned byproduct | engineer | fix (owner fan-out) |
| paper requirement | engineer | fix (enforcement) |
| repeat unlock | engineer | fix (idempotency) |
| hazard unrouted | safety | stop-the-line |
| surface lie | engineer | fix (purity) |
| chain model error | designer | review/recalibrate |
| tone issue | writer | W2-06 review |
| dead expansion family | designer | release gate blocks |

---

## §X.7 Quick reference: the crafting rules

```text
1. sources first: nothing craftable comes from nowhere
2. consumers second: nothing crafted goes nowhere
3. requirements are checks, not prose
4. waste has an owner
5. unlocks happen once
6. stations gate in one id space
7. surfaces render, never compute
8. yields seeded; quality banded
9. chains derived until signed
10. no hidden recipes; no silent blocks
```

---

## §X.8 The industrial promise

Production is how a shelter stops being a survivor camp and becomes a place
people build a life. Every rule here serves that turn: ores that mean tools,
tools that mean repairs, repairs that mean walls that hold. The audit's job
is to make sure the promise is real — that when the player invests days into
a chain, the world actually changes because of it.

> **Nothing comes from nowhere. Nothing goes nowhere. That is the whole
> discipline.**

---

## §X.9 Final control (W3-05)

**W3-05 complete at the expanded target** (Parts I–X plus the closing
addendum that follows). Proposal only. No execution without Annex U and
§VIII.5 signatures. Binding within this document: the never-cross list
(§VIII.2.1), the flow ownership rule, the idempotent grant rule, and the
ratchet.

*Document control: W3-05 · Wave 3 (expanded) · HEAD 5be1a30a ·
end of W3-05.*# W3-05 · PART XI — ACCEPTANCE MATRIX AND SEEDED FINDINGS CATALOGUE (FINAL ADDENDUM)

---

## §XI.1 Acceptance matrix (10 points × 5 dimensions)

| # | Point | Truth | Ownership | Bounds | Consumed | Tests |
|---|---|---|---|---|---|---|
| 1 | reachability | every recipe classified | catalogs registered | depth/caps | sources + consumers | Recipe_* |
| 2 | requirements | declared = enforced | owner per kind | re-check at exec | messages | Requirements_* |
| 3 | flows | no free output | owner per line | meters | byproducts | Flow_* |
| 4 | foundry | cohesion map | partials single-writer | policy keys | ventilation | Foundry_* |
| 5 | chemistry | chains real | engine owners | seeded yields | outputs consumed | Chem_* |
| 6 | catalogs | families bound | loaders register | n/a | surfaces list | Catalog_* |
| 7 | knowledge | graph complete | research owner | seeded chance | grants consumed | Knowledge_* |
| 8 | stations | gates enforced | facility owner | tiers/condition | build costs | Station_* |
| 9 | legibility | view = execution | owner reads | no math | messages | Legibility_* |
| 10 | chains | model accurate | read-only | ranges | single model | Chain_* |

---

## §XI.2 Seeded findings catalogue (40)

### XI.2.1 Reachability (C01–C08)

| # | Finding | Repair |
|---|---|---|
| C01 | phantom input | add source / substitute |
| C02 | station build cost includes dead item | revise cost |
| C03 | output with no consumer | consumer / archive note |
| C04 | cycle-dead loop | raw entry or redesign |
| C05 | depth-8 chain | simplify |
| C06 | orphaned recipe (unloaded catalog) | register loader |
| C07 | debug recipe live | mark/exclude |
| C08 | duplicate recipe for one output | merge/differentiate |

### XI.2.2 Requirements (C09–C14)

| C09 | paper knowledge | enforce |
| C10 | paper station | enforce |
| C11 | wrong owner (local flag) | owner call |
| C12 | silent block | message ref |
| C13 | TOCTOU free craft | re-check |
| C14 | interruption behavior absent | author |

### XI.2.3 Flows (C15–C20)

| C15 | free output | consume inputs |
| C16 | double consume | single deduction |
| C17 | unowned byproduct | routing table |
| C18 | unmetered power/heat | meter |
| C19 | ventilation bypass | binding |
| C20 | mass-balance violation | data fix or hidden-consumer finding |

### XI.2.4 Foundry/policy (C21–C25)

| C21 | partial overlap | single owner |
| C22 | policy double-apply | keys |
| C23 | treaty orphan | wire production |
| C24 | incident unreflect | authored events |
| C25 | lifecycle staleness | W2-02 coordination |

### XI.2.5 Chemistry (C26–C30)

| C26 | orphan engine | reachability wire |
| C27 | hazard unrouted | route to owners |
| C28 | unseeded yield | seeded facade |
| C29 | stranded output | consumer |
| C30 | maintenance-free reactor | interval |

### XI.2.6 Knowledge (C31–C35)

| C31 | dead source | grant or remove |
| C32 | repeat grant | idempotency key |
| C33 | local-flag consumer | research eligibility |
| C34 | orphan knowledge | consumer or data-only note |
| C35 | unseeded decrypt | seeded bands |

### XI.2.7 Stations/legibility/chains (C36–C40)

| C36 | decorative tier | author effect |
| C37 | id-space drift | reconcile |
| C38 | block message missing | register copy |
| C39 | UI math on costs | render owner reads |
| C40 | duplicate chain estimator | single model |

---

## §XI.3 Kit coverage

| Kit | Findings |
|---|---|
| Recipe_* | C01–C08 |
| Requirements_* | C09–C14 |
| Flow_* | C15–C20 |
| Foundry_* | C21–C25 |
| Chem_* | C26–C30 |
| Knowledge_* | C31–C35 |
| Station_* | C36–C37 |
| Legibility_* | C38–C39 |
| Chain_* | C40 |

Every seeded finding has a catching kit; holes in this mapping are kit
findings themselves.

---

## §XI.4 Repair ranking rule

```text
1. stop-the-line: free outputs, hazards unrouted, surface lies
2. authority: single-writer/idempotency violations
3. safety: paper requirements, missing warnings
4. completeness: dead sources/outputs/orphans
5. fidelity: decay/maintenance/meters
6. cosmetic: copy/notes
cap 20/phase; overflow -> debt
```

---

## §XI.5 The completion meter (W3-05)

```text
[ ] T1 green: reachability, graphs, requirements, flows, knowledge, drift
[ ] T2 kits green (9)
[ ] T3 assertions: no free outputs; policy once; hazards warned;
    maintenance honored; determinism pass
[ ] baseline recorded; ratchet holding
[ ] catalog bind table complete; data-only notes authored
[ ] (C) WIP signed and round-tripped
```

---

## §XI.6 Evidence minimum per repair

```text
before classification/flow, after, command, HEAD, owner, reviewer
```

---

## §XI.7 Version record

| Version | Change |
|---|---|
| v1.0–v1.3 | Parts I–III (summary, deep designs 1–10) |
| v1.4 | Part IV authoring playbooks |
| v1.5 | Part V verification catalog |
| v1.6 | Part VI worked case studies |
| v1.7 | Part VII Q&A + operational |
| v1.8 | Part VIII C-path + matrices + appendices |
| v1.9 | Part IX checklists + sketches + defaults |
| v2.0 | Part X field guide + control |
| v2.1 | Part XI this final addendum |

---

## §XI.8 Final control (W3-05)

**W3-05 complete.** Proposal only. Execution requires Annex U (Part I §U.2)
and §VIII.5 phase signatures. The never-cross list, the flow ownership rule,
the idempotent grant rule, and the ratchet are binding within this document.

> A shelter's industry is its argument with the wasteland: we can make what
> we need. This plan exists so that argument is always winnable, honest, and
> true to the cost of making things.

*Document control: W3-05 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-05.*# W3-05 · PART XII — CATALOG FAMILY DOSSIER

> A per-family dossier of every crafting/industry catalog family: what it is,
> what reads it, what it needs, and the audit focus. Verified at P0; this
> dossier is the scope map for the execution phases.

---

## §XII.1 Family: general crafting

**Owner:** `CraftingSystem` + `RecipeCatalogLoader` + `CraftContext`.
**Consumers:** shelter workshop surfaces, expedition crafting, NPC work.

Audit focus:

```text
[ ] recipe registration complete (every catalog file loaded)
[ ] requirement enforcement (knowledge/station/tool)
[ ] flow ownership on every recipe
[ ] consumer graph on outputs
[ ] craft ledger attribution
```

## §XII.2 Family: pharma/medicine

**Owner:** `PharmaRecipeCatalogLoader` + chemistry engines.
**Consumers:** infirmary, care (W3-03), trade.

Audit focus:

```text
[ ] reagent source chains (INPUT-DEAD risk high)
[ ] hazard routing (dosage, contamination events authored)
[ ] quality bands (potency) consumed by care effect?
[ ] knowledge gating (medical tiers)
```

## §XII.3 Family: chemistry synthesis

**Systems:** `ChemicalSynthesisSystem`, `ChlorAlkaliSynthesisEngine`,
`FischerTropschSynthesisEngine`, `CvdDiamondSynthesisEngine`, plus data for
cryogenic air separation and electrostatic filtration.

Audit focus:

```text
[ ] each engine reached (recipes/UI)
[ ] hazard routing + warnings
[ ] utility metering (power/heat)
[ ] maintenance cycles
[ ] output consumers
[ ] seeded yields
```

## §XII.4 Family: foundry/metallurgy

**Systems:** `SilentFoundrySystem` (+5 partials), `SilentFoundryCatalog`,
`MaterialProfileCatalog`, `MetallurgyHeavyCatalog`, `PowderMetallurgySystem`,
`HydraulicExtrusionEngine`, `CrucibleFoundryCatalog`,
`SaltMineExtractionSystem`.

Audit focus:

```text
[ ] partial cohesion map
[ ] policy once-per-key
[ ] ventilation single path
[ ] material quality bands consumed
[ ] treaty labor wiring
```

## §XII.5 Family: glass/ceramics/optics

**Catalogs:** `GlassworksCatalog`, `GlassblowingDistillationCatalog`,
`CeramicsKilnCatalog`, `OpticsGlassworksCatalog`; engine: `.Glassworks`
partial.

Audit focus:

```text
[ ] stations buildable (kiln/glassworks/bench)
[ ] breakage variance seeded
[ ] optics outputs consumed (instruments, science)
[ ] ceramics consumers (storage, construction)
```

## §XII.6 Family: paper/printing

**Catalog:** `PaperPrintingCatalog`; consumers: archives, W3-01 narrative.

Audit focus:

```text
[ ] press station buildable; paper source chain
[ ] printed outputs consumed (archive/desk systems)
[ ] narrative integration (W3-01) for printed texts
```

## §XII.7 Family: milling/food

**Catalog:** `GrainMillingCatalog` (+ discovery/projection).

Audit focus:

```text
[ ] grain source chain (harvest/spawn/purchase)
[ ] feed vs. food outputs consumed (needs/economy)
[ ] discovery/projection consumers (who reads them?)
[ ] spoilage flows (inventory decay)
```

## §XII.8 Family: shelter engines

**Systems:** `ShelterWorkshopSystem`, `CupolaFoundryEngine` (+catalog).

Audit focus:

```text
[ ] station ids align with shelter facility owner
[ ] engine inputs/outputs owned
[ ] heat/ventilation integration
[ ] crew skill bands
```

## §XII.9 Family: salvage/relic

**System:** `RelicCatalogLoader`, `TechSalvageCatalog`.

Audit focus:

```text
[ ] salvage yields enter source graph
[ ] dissembly outputs consumed (components)
[ ] salvage knowledge sources (C5 interface)
[ ] dead salvaged outputs
```

## §XII.10 Family: blueprints/knowledge artifacts

**Catalog:** `BunkerBlueprintCatalog`; research systems.

Audit focus:

```text
[ ] blueprint grants keyed (unlock once)
[ ] blueprint consumers (building unlocks)
[ ] display strings registered
```

## §XII.11 Family: robotics

**System:** `RoboticsSystem`.

Audit focus:

```text
[ ] robot production chain reachable
[ ] robot outputs consumed (work contributions)
[ ] maintenance/energy cycles authored
[ ] no second labor authority (robots feed the same owners)
```

## §XII.12 Family: extraction

**Systems:** `SaltMineExtractionSystem` and authored resource nodes.

Audit focus:

```text
[ ] extraction sites reachable (map/location ownership)
[ ] yields flow to inventory/economy
[ ] depletion authored (or infinite-allowed with reason)
[ ] hazard/labor effects
```

## §XII.13 The dossier summary table

| Family | Reachability | Flow | Hazards | Knowledge | Priority |
|---|---|---|---|---|---|
| general | primary | primary | low | medium | 1 |
| pharma | high | high | high | high | 1 |
| chemistry | high | high | critical | medium | 1 |
| foundry | primary | primary | high | medium | 1 |
| glass/ceramics | medium | medium | medium | low | 2 |
| paper | medium | low | low | medium | 3 |
| milling | high | medium | low | low | 1 |
| shelter engines | high | high | high | medium | 2 |
| salvage | high | medium | low | high | 2 |
| blueprints | low | low | none | high | 2 |
| robotics | medium | medium | medium | high | 3 |
| extraction | high | high | medium | low | 2 |

Priority 1 families run in the first pass; 2 in the second; 3 as content
allows.

---

## §XII.14 Cross-family interactions

```text
chemistry -> pharma (reagents)
foundry -> shelter engines (parts, metals)
glass -> optics -> science/desk systems
milling -> food -> needs/economy
salvage -> foundry (scrap) and knowledge (forensics)
robotics -> workshop output (labor contribution)
extraction -> chemistry/foundry (raw)
```

Each arrow is a source/consumer edge the graphs must show; missing arrows are
where chains silently die.

---

*End of Part XII. Continues in Part XIII (the authoring candidate catalogue).*# W3-05 · PART XIII — REFERENCE TABLES AND WORKED AUTHORING EXAMPLES

---

## §XIII.1 Worked example: authoring a new recipe (from scratch)

**Intent:** a shelter can make a water filter from charcoal, cloth, and
gravel.

### Step 1 — Model

```text
family: general (workshop)
station: workbench tier 1
knowledge: basic_water_safety (research or salvage)
inputs: charcoal 2, cloth 1, gravel 3
utilities: none (labor 1h)
outputs: water_filter 1
byproducts: none (trim waste -> facility storage: ash/sweepings, small)
```

### Step 2 — Source checks

```text
charcoal: produced (kiln/burn) -> check kiln exists; else salvage
cloth: salvage/textile recycling -> source?
gravel: raw (river/spoil) -> spawn table?
knowledge: research catalog entry + source
```

Findings expected: gravel often lacks a source in survival games; the audit
catches it here (a canonical INPUT-DEAD).

### Step 3 — Consumer checks

```text
water_filter: consumed by water system (purification action)
              or inventory (equipable?) -> verify owner accepts it
```

### Step 4 — Requirement messages

```text
missing_knowledge: "you don't know how to make that yet"
missing_station: "you need a workbench"
missing_input: "you're short on {item}"
```

### Step 5 — Flow/utility

```text
labor consumed from schedule; trim waste routed; ledger recorded.
```

### Step 6 — Tests

```text
Recipe_WaterFilter_Crafts (KR.1)
Recipe_WaterFilter_SourceLive (C1.2)
Recipe_WaterFilter_Consumed (C1.3)
Requirements_WaterFilter_Blocks (KG.1-2)
```

### Step 7 — Review sheet run; tone check on name/description.

---

## §XIII.2 Worked example: station tier effect (the decorative trap)

**Bad authoring:**

```yaml
tier_2: {unlocks: []}        # nothing
```

**Finding:** decorative tier (C36).

**Good authoring:**

```yaml
tier_2:
  unlocks: [steel_tool_head]
  speed: +20% (band)
  quality: +1 band
  power: +1 (metered)
  condition_decay: slower (authored)
```

Every field measurable; the test asserts speed/quality deltas.

---

## §XIII.3 Worked example: byproduct routing (the vanishing smoke)

```text
process: smelt
byproduct: exhaust, band: heavy
owner: ventilation binding (verified BindVentilation)
downstream: atmosphere concentration -> W2-04 band -> worker exposure (if
  concentration high) -> W3-03 trigger
test: Flow_VentilationBound + Chem_BandAccumulation
```

The full path is three owners; the audit checks each link.

---

## §XIII.4 Worked example: knowledge idempotency (the repeat unlock)

```yaml
knowledge: water_safety
grants: [water_filter recipe, purification capability]
```

Two sources (research + salvage) both grant. Test: acquire via A; acquire via
B; assert recipes exist once, capability once, no duplicate journal entries.

---

## §XIII.5 Worked example: chain model reading

```text
chain: raw grain -> flour -> bread
stocks: grain 40, flour 3, bread 2
throughput: mill 8/day, bakery 6/day
inputs: grain supply 5/day (harvest/caravan)
bottleneck: grain supply (5/day < bakery cap)
output estimate: 5 bread/day; days-to-starry: n/a (supply-driven)
reason line: "the mill waits on grain, not capacity"
```

The audit checks the reason against the actual limiter (grain) — the model
must be right for the right reason.

---

## §XIII.6 Reference: the requirement-kind owner matrix

| Kind | Owner | Read | Failure message kind |
|---|---|---|---|
| knowledge | ResearchSystem | eligibility | knowledge |
| station | facility owner | exists/tier | station |
| tool | equipment owner | equip/condition | tool |
| power | grid owner | draw available | power |
| heat | foundry thermal | band availability | heat |
| environment | ventilation owner | band | environment |
| labor | schedule owner | effort | labor |
| quantity | inventory owner | counts | inputs |
| space | shelter owner | bays | space |

---

## §XIII.7 Reference: yield/quality composition

```text
yield  = base × input_quality × tool_band × station_band × seeded_var
quality = base_band + station_tier + labor_skill ± authored spread
```

All terms banded or bounded; tests assert monotone direction per input.

---

## §XIII.8 Reference: the crafting ledger entry

```yaml
entry:
  recipe: water_filter
  actor: survivor_erev
  station: workbench_1
  day: 44
  consumes: {charcoal: 2, cloth: 1, gravel: 3}
  produces: {water_filter: 1}
  byproducts: {sweepings: 0.2}
  utilities: {labor: 1h}
  quality: sound
  seeded: campaign#44
```

The ledger is the audit's ground truth for flow assertions.

---

## §XIII.9 Reference: the surface source table (craft screen)

| Block | Source |
|---|---|
| readiness | CanCraft (live) |
| requirement rows | requirement table + owner checks |
| cost items | inventory reads |
| power/time | owner reads |
| output preview | recipe + quality bands |
| block message | corpus ref per kind |

---

## §XIII.10 Reference: expansion family gates (27–31)

| Expansion | Crafting substrate | Release gate |
|---|---|---|
| 27 Thread | textile/paper chains | reachability + flows |
| 28 Lesson | printed/knowledge chains | knowledge graph |
| 29 Glass | glass/optics chains | station + outputs |
| 30 Press | printing chains | press station + consumers |
| 31 Kiln | ceramics/milling chains | kiln + food flows |

Each expansion's content enters the map; its release follows a green
reachability/flow gate.

---

## §XIII.11 Reference: the recipe authoring checklist (compact)

```text
[ ] source chain or raw entry
[ ] consumer or data-only note
[ ] station + tier + id space
[ ] knowledge source + grants keyed
[ ] inputs/utilities/flow ownership
[ ] byproducts routed
[ ] quality/yield bands
[ ] message refs per kind
[ ] tests authored
[ ] tone checked
```

---

*End of Part XIII. Continues in Part XIV (runbooks and closeout).*# W3-05 · PART XIV — EXECUTION RUNBOOK: P0 TO CLOSEOUT

---

## §XIV.1 Preconditions

```text
[ ] Annex U.2 signatures for the phases being run
[ ] §VIII.5 C items signed or excluded
[ ] P0 premise scheduled first
[ ] claim registered (WORKTREE_OWNERSHIP)
[ ] test policy reviewed (focused runs)
[ ] W2-02 coordination noted for foundry lifecycle
```

## §XIV.2 P0 — premise (day 1–3)

```text
1. verify owners at HEAD: CraftingSystem, loaders (general/pharma/relic),
   ChemicalSynthesisSystem, SilentFoundry family + surface/policy/session,
   MaterialProfileCatalog, MetallurgyHeavyCatalog, PowderMetallurgySystem,
   HydraulicExtrusionEngine, SaltMineExtractionSystem, CupolaFoundryEngine,
   ShelterWorkshopSystem, ResearchSystem family, catalogs (ceramics, glass,
   optics, printing, milling(+disc/proj), blueprint, crucible), RoboticsSystem
2. capture file:line evidence; note any signature drift
3. data inventory: every crafting catalog file and its loader registration
4. selftests: run what exists (data-integrity/smoke); record outputs
5. known-gap row: any catalog without a loader; any engine without a path
6. write docs/crafting/P0_CRAFT_PREMISE.md
```

Exit: every premise verified or marked changed with a re-audit note.

## §XIV.3 P1 — reachability

```text
1. build source graph (raw/salvage/purchased/produced)
2. build consumer graph (recipe/build/award/economy/narrative/tool)
3. classify every recipe (six classes)
4. cycle detection + depth report
5. publish map + baseline; wire generators + --check
6. rank repairs (cap 20); archive notes with reasons
```

Expected outputs: the map table; baseline file; repair list.

## §XIV.4 P2 — requirements

```text
1. map each requirement kind to its owner API
2. audit declarations; enforce at execution
3. register block messages per kind
4. author TOCTOU/interruption behavior
5. run Requirements_* kit
```

## §XIV.5 P3 — flows

```text
1. declare consume/produce/byproduct/utilities per process
2. build the flow routing table
3. meter utilities; bind ventilation
4. craft ledger entries
5. run Flow_* kit + mass-balance report
```

## §XIV.6 P4 — foundry

```text
1. generate the partial cohesion map; resolve overlaps
2. key policy applications; test once
3. verify ventilation single path
4. coordinate lifecycle with W2-02 (D19c)
5. wire treaty labor; test observability
6. author incidents + warnings
```

## §XIV.7 P5 — chemistry

```text
1. chain table per engine; reachability check
2. hazard routing + concentration/decay models
3. meter utilities; maintenance intervals
4. seeded yields + quality bands
5. run Chem_* kit
```

## §XIV.8 P6 — catalogs

```text
1. loader registration audit per family
2. class/station/consumer verification
3. surface listing check (W3-06)
4. data-only notes for exceptions
```

## §XIV.9 P7 — knowledge

```text
1. generate the graph; cycle/orphan checks
2. idempotent grant routing; seeded chance sources
3. consumer eligibility reads
4. run Knowledge_* kit
```

## §XIV.10 P8 — stations

```text
1. reconcile id spaces
2. enforce gates at execution with reasons
3. author tier effects; condition/maintenance
4. crew skill bands
5. run Station_* kit
```

## §XIV.11 P9 — legibility

```text
1. implement the view contract (with W3-06)
2. source table rows; render-only
3. execute re-check
4. messages per kind
5. run Legibility_* kit
```

## §XIV.12 P10 — chain model

```text
1. implement read-only model (analysis)
2. bottleneck/estimates with ranges
3. accuracy harness
4. unify surfaces on the model
5. (C) WIP per signature
```

## §XIV.13 Soak

```text
1. configure the shared harness
2. run 20×60
3. assert: no free outputs, policy once, hazards warned, maintenance bands,
   model accuracy, determinism
4. triage findings; repairs ranked
5. evidence pack
```

## §XIV.14 Closeout

```text
1. final kits + soak on frozen HEAD
2. evidence pack
3. closeout memo
4. Annex U releases recorded
5. debt rows for deferrals
```

### Closeout memo template

```text
OUTCOME:
FILES:
CONTRACT: reachability classes; enforced requirements; owned flows;
  idempotent knowledge; gated stations; truthful surfaces; derived chains
COMMANDS: T1/T2/T3 + results
LIMITATIONS:
SHARED PATHS TOUCHED:
LEDGER PROPOSALS:
ANNEX U RELEASES EARNED:
```

## §XIV.15 In-execution decisions

| Situation | Decision |
|---|---|
| a family has no loader at P0 | register (B) or mark data-only with reason |
| an input has no source | add source / substitute / archive recipe (ranked) |
| a hazard has no owner | stop; owner binding required |
| a policy trigger lacks a key | add key before content seals |
| an expansion family fails reachability | release gate blocks the expansion |
| a repair implies a new system | escalate (Rule 5) |
| flake | quarantine with reason + hypothesis |

## §XIV.16 Maintenance

```text
per content: classification + flows for new items
weekly: T1; soak spot-run
per release: T3; expansion family gates
```

---

*End of Part XIV. Continues in Part XV (corrections and Q&A supplement).*# W3-05 · PART XV — CORRECTIONS CATALOGUE, Q&A SUPPLEMENT, AND COMMON MISTAKES

---

## §XV.1 The top-40 corrections (what reviewers return most)

| # | Correction | Fix |
|---|---|---|
| 1 | recipe with unnamed input source | source chain / substitute |
| 2 | output with no reader | consumer or data-only note |
| 3 | station as prose | enforce gate |
| 4 | free craft (no consumption) | flow ownership |
| 5 | smoke/haze vanishes | ventilation binding |
| 6 | slag grows silently | storage + clearing task |
| 7 | policy applies per tick | keys per occurrence |
| 8 | treaty obligation orphaned | wire production output |
| 9 | knowledge grants again | idempotency key |
| 10 | knowledge consumer checks local flag | research eligibility |
| 11 | decrypt random | seeded bands |
| 12 | tier with no effect | author effect or remove |
| 13 | id spaces differ | reconcile to owner |
| 14 | block message missing | register copy |
| 15 | surface computes costs | render owner reads |
| 16 | quality bands unwired | consume in condition/economy |
| 17 | yield variance unbounded | clamp/band |
| 18 | maintenance-free reactor | interval + decay |
| 19 | hazard event uninformed | warnings + scene |
| 20 | chemistry naming from reality | fictional register |
| 21 | chain depth 9+ | simplify |
| 22 | duplicate recipe | merge/differentiate |
| 23 | debug content live | exclude |
| 24 | salvage output dead | consumer |
| 25 | blueprint grants unkeyed | key |
| 26 | milling discovery unread | consumer or note |
| 27 | feed vs food confusion | separate item classes |
| 28 | robot work not routed | feed labor owner |
| 29 | extraction infinite | depletion or authored note |
| 30 | compound recipe hidden inputs | declare all |
| 31 | stations consume no space | space requirement |
| 32 | utility draw unmetered | meter |
| 33 | interruption unhandled | author behavior |
| 34 | TOCTOU free craft | re-check |
| 35 | ledger missing | record entries |
| 36 | message copy mocking | plain in-world voice |
| 37 | item name collides | rename (display), ids stable |
| 38 | recipe unlocked by two disjoint paths | graph merge |
| 39 | station tier gating skipped | enforce tier |
| 40 | chain model guesses | owner-sourced reads |

---

## §XV.2 Q&A supplement (Q46–Q65)

**Q46. Where does the mass-balance tolerance live?**
Authored per process class in the flow table; deviations are findings, not
failures (B) / gate (C).

**Q47. Are crafted items fungible with purchased ones?**
By default yes (same item id); if distinctions matter (quality), the band is
authored and consumers read it. No parallel item authorities.

**Q48. What stops recipe sprawl?**
The map classifies; duplicates and dead outputs surface immediately; content
review sees the cost of a new recipe before it ships.

**Q49. How are processes with no inputs (gathering) modeled?**
As raw sources (spawn/harvest), not recipes; the source graph lists them.

**Q50. Can a station be a person (NPC crafter)?**
NPCs use the same stations/recipes through their work owners; no special
crafting NPCs.

**Q51. What if a plant/machine is both station and process?**
Model the machine as a station that provides an engine process; one owner
(facility), one flow.

**Q52. How is heat modeled — item or band?**
A band owned by the thermal/foundry owner; processes require bands, not
counted heat units (unless authored otherwise).

**Q53. What about recipe variants (same output, different inputs)?**
Authored as variants; the consumer graph counts the output once; reachability
checks each variant.

**Q54. What if an expansion adds a parallel recipe family?**
It enters the map; the owner must be the existing crafting/engine owner —
no new authority.

**Q55. Are failure catastrophes (explosions) allowed?**
Authored hazard events with warnings and consequences; rare and reviewed;
never silent or farming-able.

**Q56. How are knowledge speeds shown?**
Time estimates in bands (days/weeks), authored; no precise false precision.

**Q57. Do repairs to recipes change saves?**
No; recipe data is campaign-independent. Station/facility state is save state
and unaffected.

**Q58. What about a crafted key item required by narrative?**
It flows normally; narrative reads inventory; no special path. The item's
consumer is the narrative gate (listed).

**Q59. How is "loot quality" from salvage distinguished?**
Salvage yields bands; recipes reading inputs consume bands per authored
rules; no hidden tables.

**Q60. How does the plan treat modded content?**
Same gates; registration and classification are the entry cost.

**Q61. What is the worst silent failure in crafting?**
A free output (duplication loop): it breaks economy and progression at once.
That's why it is stop-the-line.

**Q62. Second worst?**
Paper requirements (gates that don't gate) — they make the station/tech tree
illusory.

**Q63. What makes the audit sustainable?**
Generated maps + ratchets + incremental re-classification. No manual
spreadsheets.

**Q64. How do we know the chain model is honest?**
It reads owners; it never writes; accuracy is measured against soak actuals.

**Q65. What is the single sentence for a crafting contributor?**
"Name where it comes from, where it goes, and who says yes."

---

## §XV.3 Author mistakes to avoid (quick list)

```text
do not invent item ids for requirements
do not store quality outside the item/band owners
do not bypass the ledger to "save time"
do not hide inputs for difficulty
do not let byproducts vanish
do not gate with prose
do not compute in the surface
do not grant twice
do not label chemistry with the real world
do not add depth "for realism"
```

---

## §XV.4 The crafting review standard (one page)

```text
1. SOURCE       every input has a path from raw
2. SINK         every output has a consumer
3. GATE         every requirement is enforced and explained
4. FLOW         every stream has an owner; no free outputs
5. ONCE         policy and unlocks apply once
6. TIER         every upgrade changes something measurable
7. HONEST       surfaces equal execution; models read owners
8. SEEDED       yields, decrypts, variances deterministic
9. DEPTH        chains shallow enough to play
10. TONE        fictional register; no real-world labels
```

---

*End of Part XV. Continues in Part XVI (final appendices and control).*# W3-05 · PART XVI — SAMPLE DOCUMENTS, GLOSSARY SUPPLEMENT, AND HANDOFF INDEX

---

## §XVI.1 Sample: reachability map row

```markdown
| recipe | class | reason | evidence |
|---|---|---|---|
| water_filter | REACHABLE | sources: charcoal(kiln), cloth(salvage), gravel(spawn) | sources.md#water_filter |
| lens_grinding | STATION-LOCKED | optics_bench has no build cost entry | stations.md#optics |
| alloy_plate | REACHABLE | knowledge alloy_work via archive A | kg #alloy_work |
| signal_flare | OUTPUT-DEAD | no consumer (craft/build/award/economy/narrative) | consumers.md |
| trophy_horn | REACHABLE (data-only) | consumer: memorial/shelter display (narrative) | note #N12 |
```

## §XVI.2 Sample: requirement table row

```markdown
| recipe | kind | value | owner | message |
|---|---|---|---|---|
| steel_tool_head | knowledge | basic_smelting | research | craft_msg_knowledge |
| steel_tool_head | station | crucible_furnace t2 | facility | craft_msg_furnace_t2 |
| steel_tool_head | heat | high_band | thermal | craft_msg_heat |
| steel_tool_head | labor | 2h | schedule | craft_msg_labor |
```

## §XVI.3 Sample: flow routing row

```markdown
| process | stream | quantity | owner | disposition |
|---|---|---|---|---|
| smelt | slag | 2 | facility storage | clear when full (task) |
| smelt | exhaust | smoke_heavy | ventilation binding | concentration band -> exposure |
| chlor_alkali | trace_chlorine | band | ventilation | band -> W3-03 trigger |
| glass | cullet | 1 | inventory | reusable input |
```

## §XVI.4 Sample: knowledge graph entry

```markdown
## alloy_work
- sources: study (basic_smelting, 4 days), archive A (medium), salvage dataslate (probable)
- grants: steel_tool_head, alloy_plate (recipes); foundry alloy selection (capability)
- prereqs: basic_smelting
- consumers: recipe loader; facility tier 2; narrative gate n_alloy
- status: REACHABLE; idempotent verified
```

## §XVI.5 Sample: chain model view

```markdown
## Chain: ore -> tool (day 44)
| step | stock | cap/day | input avail | bottleneck |
|---|---|---|---|---|
| ore | 22 | — | 4/day | — |
| ingot | 5 | 3 | ore 4/day | — |
| part | 2 | 2 | ingot 3/day | part station (2/day) |
| tool | 1 | 2 | part 2/day | — |
Est: 2 tools/day max; actual limiter: crucible batch (2/day).
Days-to-starve (scrap): 5 (range 3-7).
```

## §XVI.6 Sample finding file

```markdown
FINDING CR-11
class: flow
state: smelting exhaust
evidence: data/foundry/processes.json:88 (byproduct declared, no owner)
scenario: ore-to-tool case, step 3
expected: exhaust routed via ventilation binding
observed: loop applies atmosphere effect to a local counter (never read)
repair: bind to BindVentilation; remove local counter
owner: foundry
status: repaired (evidence/repairs/CR-11.md)
```

---

## §XVI.7 Glossary supplement

| Term | Definition |
|---|---|
| authored exception | gate deviation with owner, reason, expiry |
| capability grant | non-recipe unlock (e.g., alloy selection) |
| consumer note | authored terminal for decorative outputs |
| craft ledger | attribution record of every craft |
| depth report | chains exceeding the review threshold |
| engine row | chemistry chain table entry per engine |
| family dossier | per-catalog audit scope map |
| flow routing | stream → owner disposition table |
| inbound gate | expansion family release gate (reachability/flow) |
| meter | utility consumption recorded at owner |
| negotiation-free | (n/a) |
| output tray | facility staging for produced items |
| projection companion | data that extends a catalog (milling) |
| raw entry | spawn/harvest source that seeds production |
| recipe variant | alternate input set for one output |
| sink | consumer that terminates an output's chain |
| source note | classification of an item's origin |
| starve (chain) | input depletion forecast in the model |
| tier effect | measurable change from a station upgrade |
| WIP | work-in-progress at a station (C) |

---

## §XVI.8 Handoff index

| Receiver | Deliverable |
|---|---|
| W3-02 | output goods + quality bands; input prices at craft |
| W3-04 | repair recipes; tool/ordnance condition materials |
| W3-01 | knowledge narrative grants; archive scenes; item lore |
| W3-03 | hazard exposure triggers; care supplies; labor strain |
| W3-06 | craft views; block messages; chain board |
| W2-02 | foundry lifecycle coordination |
| W2-03 | yield/quality measurements |
| W2-04 | atmosphere/heat/ash interfaces |
| W2-06 | strings and tone |
| UNBLOCK-03 | freeze routing for new strings |

---

## §XVI.9 Known limitations (formal)

```text
L1  Mass balance is report-only at B (gate at C).
L2  Quality bands' consumers depend on W3-02/W3-04 interface decisions.
L3  WIP (C2) shape depends on the crafting owner's persistence review at P0.
L4  Robotics depth depends on the existing system's scope (P0).
L5  Extraction depletion rules (if  present) require the location owner's
    current state model.
L6  Some families may be authored data-only by design; notes required.
L7  The chain model's accuracy target is authored (proposal ±15% absolute).
```

---

*End of Part XVI. Continues in Part XVII (final control).*# W3-05 · PART XVII — EXPANSION INTEGRATION, SCENARIO SUPPLEMENT, AND FINAL CONTROL

---

## §XVII.1 Expansion integration (27–31) in detail

### XVII.1.1 Expansion 27 — Thread (textiles)

```text
substrate needed: fiber sources (plant/animal/salvage), spinning/carding,
  weaving/knitting station, dye processes.
audit at arrival:
  [ ] fiber source chain (raw entries)
  [ ] station buildable (loom/knitting)
  [ ] outputs consumed (clothing, bandages, trade)
  [ ] dye byproducts routed
  [ ] knowledge gating (textile tiers)
release gate: reachability + flow green
```

### XVII.1.2 Expansion 28 — Lesson (knowledge/education)

```text
substrate: printed materials, desk/study stations, knowledge graph edges.
audit at arrival:
  [ ] study sources reachable
  [ ] grants keyed; consumers verified
  [ ] printed outputs consumed (desks/archives)
  [ ] no duplicate knowledge ids
release gate: knowledge graph green
```

### XVII.1.3 Expansion 29 — Glass (glass/optics)

```text
substrate: sand source, glassworks, optics bench, lens/pane outputs.
audit at arrival:
  [ ] sand raw entry; cullet loop closed
  [ ] stations buildable (tiers)
  [ ] optics outputs consumed (instruments/science/trade)
  [ ] breakage variance seeded
release gate: station + consumer green
```

### XVII.1.4 Expansion 30 — Press (printing)

```text
substrate: paper chain, press station, ink process.
audit at arrival:
  [ ] paper source chain (pulp/rag)
  [ ] press buildable; ink flow owned
  [ ] printed consumers (archives/W3-01 content)
release gate: catalog bind green
```

### XVII.1.5 Expansion 31 — Kiln (ceramics/milling)

```text
substrate: clay/grain sources, kiln/mill, ceramics/flour outputs.
audit at arrival:
  [ ] clay + grain raw entries
  [ ] kiln/mill buildable; fuel/heat metered
  [ ] ceramics consumers (storage/construction); flour -> food
release gate: family bind green
```

### XVII.1.6 The common expansion checklist

```text
[ ] new items enter the source graph
[ ] new recipes classified
[ ] new stations buildable with ids in owner space
[ ] new knowledge in the graph, keyed
[ ] flows + byproducts owned
[ ] surfaces list the new recipes
[ ] tests per family added
[ ] tone review scheduled
```

---

## §XVII.2 Scenario supplement: five shorter threads

### XVII.2.1 The first kiln

Player builds the first kiln (materials, space, heat). Thread: fuel source
(wood/charcoal), firing cycle (heat band), ceramics outputs (containers),
failure events (cracked batch, authored). Findings class: heat metering,
fuel chain, breakage variance, consumer wiring.

### XVII.2.2 The medicine shortage

Pharma chain breaks at a reagent. Thread: substitute search (salvage,
trade), dosage quality bands, infirmary consumption, W3-03 care dependency.
Findings: INPUT-DEAD, quality-band consumer, knowledge gate.

### XVII.2.3 The smith's hands

A crafter accumulates mastery (C3) and strain (W3-03). Thread: mastery bands
improve output; strain bands degrade; rest/rotation policy. Findings: mastery
bounds, strain routing, work-owner reads.

### XVII.2.4 The salvage run

Relic dissembly yields components + a knowledge fragment (C5). Thread: source
graph entry, fragment grant, component consumers. Findings: salvage consumers,
fragment idempotency, dead fragments.

### XVII.2.5 The export batch

Trade-grade production (C7): a batch crafted for export quality; caravan
prices read grades (W3-02). Thread: quality targets, grading, sale, standing.
Findings: grade consumer, quality determinism, economy handshake.

---

## §XVII.3 The crafting contributor's close (five sentences)

```text
1. Every input has a path from the world.
2. Every output has a place in it.
3. Every gate is a promise the game keeps.
4. Every waste stream has a home.
5. Every unlock happens once and opens something real.
```

---

## §XVII.4 Reading card

```text
ASHFALL W3-05 · CRAFTING/RESEARCH/INDUSTRY · READING CARD

WHAT:    nothing from nowhere; nothing to nowhere; gates that gate;
         unlocks once; waste owned; surfaces truthful.
WHY:     industry is a shelter's argument with the wasteland.
HOW:     P0 premise -> P1 reachability -> P2 requirements -> P3 flows ->
         P4 foundry -> P5 chemistry -> P6 catalogs -> P7 knowledge ->
         P8 stations -> P9 legibility -> P10 chains.
GATES:   T1 maps/graphs/checks; T2 nine kits; T3 soak (free outputs=0,
         policy duplicates=0, hazards warned, model accurate).
STOPS:   free outputs; unowned byproducts; paper gates; repeat unlocks.
SIGN:    Annex U + §VIII.5.
```

---

## §XVII.5 Version record (complete)

| Version | Change |
|---|---|
| v2.2 | Part XII catalog family dossier |
| v2.3 | Part XIII reference tables + worked examples |
| v2.4 | Part XIV execution runbook |
| v2.5 | Part XV corrections + Q&A supplement |
| v2.6 | Part XVI samples + glossary + handoffs |
| v2.7 | Part XVII expansion integration + close |

---

## §XVII.6 Final control (W3-05)

**W3-05 complete.** Proposal only. Execution requires Annex U (Part I §U.2)
and §VIII.5 phase signatures. Binding within this document: the never-cross
list (§VIII.2.1), flow ownership, idempotent grants, the ratchet, and the
review standard (§XV.4).

> What a shelter makes is what a shelter becomes. Keep the chains true.

*Document control: W3-05 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-05.*# W3-05 · PART XVIII — THE AUTHORING WORKSHEET PACK (FINAL ADDENDUM)

> Five complete, filled worksheets — the reference answers for the authoring
> playbooks. Each worksheet is implementation-ready. Final part of W3-05.

---

## §XVIII.1 Worksheet 1 — Recipe: bread (milling → baking)

```yaml
recipe: bread
family: milling/food
station: bakery_oven (tier 1)
knowledge: [basic_baking]
inputs: {flour: 2, water: 1, salt: 0.2, fuel: small}
utilities: {heat: medium_band, labor: 1h}
outputs: {bread: 6}
byproducts: {ash: 0.1 (atmosphere), crumbs: 0 (n/a)}
quality: {stale | plain | good}
yield: base 6, ±1 seeded (oven condition)
time: 1h
messages: {station: msg_need_oven, knowledge: msg_need_baking, inputs: msg_short}
sources:
  flour: produced (mill; grain raw)
  water: well/rain (raw)
  salt: produced (extraction) or purchased
  fuel: wood/charcoal (raw/produced)
consumers:
  bread: needs (food) + trade (economy)
tests: [Recipe_Bread_Crafts, Requirements_Bread_Blocks, Flow_Bread_Utilities]
```

**Checks that matter:** grain raw entry (else INPUT-DEAD); oven station
buildable; bread eaten through needs owner; heat metered/fuel consumed.

---

## §XVIII.2 Worksheet 2 — Recipe: bandage (pharma/textile)

```yaml
recipe: bandage
family: pharma
station: clean_bench (tier 1)
knowledge: [basic_first_aid]
inputs: {cloth: 2, boiled_water: 0.5}
utilities: {labor: 0.5h, heat: low_band (boiling)}
outputs: {bandage: 3}
byproducts: {wastewater: small (facility drain)}
quality: {rough | clean | sterile}   # sterile requires pressure stroke (tier 2)
yield: 3 (breakage with roughness)
sources:
  cloth: salvage/textile (source chain)
  boiled_water: water + heat (utility chain)
consumers:
  bandage: care (W3-03 treatment effect) + trade
notes: sterile quality requires tier 2 + fuel; quality consumed by infection
  risk reduction (authored interface)
tests: [Recipe_Bandage_Crafts, Chem_SterileBandConsumer, Flow_BandageWaste]
```

**Checks that matter:** cloth source chain; sterile band consumed by care;
wastewater routed; no free sterile tier.

---

## §XVIII.3 Worksheet 3 — Recipe: glass pane (glassworks)

```yaml
recipe: glass_pane
family: glass
station: glassworks (tier 1)
knowledge: [basic_glassmaking]
inputs: {sand: 4, soda_ash: 1, cullet: 1}
utilities: {heat: high_band, labor: 1h}
outputs: {glass_pane: 1}
byproducts: {cullet_fines: 0.5 (reusable), exhaust: low (ventilation)}
quality: {cloudy | clear | optical}
yield: 1 with breakage variance ±20% (authored)
sources:
  sand: raw (quarry/spoil)
  soda_ash: produced (burn process) or purchased
  cullet: recycled (loop closes)
consumers:
  glass_pane: construction (windows) + optics chain + trade
tests: [Recipe_GlassPane_BreakageSeeded, Flow_CulletLoop, Station_GlassworksTier]
```

**Checks that matter:** cullet loop must seed from production (else
cycle-dead); breakage seeded; pane consumers verified.

---

## §XVIII.4 Worksheet 4 — Recipe: printed notice (press)

```yaml
recipe: printed_notice
family: printing
station: press (tier 1)
knowledge: [literacy_work? authored]
inputs: {paper: 2, ink: 0.5}
utilities: {labor: 0.5h}
outputs: {notice: 10}
byproducts: {spoiled_sheets: 0.2 (recyclable)}
sources:
  paper: produced (pulp chain)
  ink: produced (soot/binder) or purchased
consumers:
  notice: shelter surfaces (postings) + narrative props + trade
notes: content strings on the notice are corpus items (W2-06); template ids
  stable
tests: [Recipe_Notice_Crafts, Knowledge_PrintingGrants, Flow_InkOwned]
```

**Checks that matter:** paper source chain; ink flow; notice consumer
(narrative prop rather than a mere item); no free printing.

---

## §XVIII.5 Worksheet 5 — Recipe: water filter (general)

```yaml
recipe: water_filter
family: general
station: workbench (tier 1)
knowledge: [basic_water_safety]
inputs: {charcoal: 2, cloth: 1, gravel: 3}
utilities: {labor: 1h}
outputs: {water_filter: 1}
byproducts: {sweepings: 0.2 (facility)}
quality: {crude | sound}
consumers: water system (purification action) + trade
sources:
  charcoal: kiln/burn (fuel chain)
  cloth: salvage
  gravel: raw
tests: [Recipe_WaterFilter_SourceLive, Requirements_WaterFilter_Blocks]
```

**Checks that matter:** charcoal chain (kiln reachable), gravel raw entry,
filter consumed by the water owner's purification.

---

## §XVIII.6 Worksheet 6 — Station: the proper kiln

```yaml
station: kiln
family: ceramics
build_cost: {brick: 24, clay: 12, stone: 8}
space: 1 bay
heat: self (fuel)
tiers:
  1: {unlocks: [pottery_basic], speed: 1.0, fuel: normal}
  2: {unlocks: [ceramic_filter, firebrick], speed: 1.1, quality: +1}
condition: decay per firing cycle (authored); relining task
crew: 1 (skill bands)
byproducts: {smoke: ventilation, ash: facility}
tests: [Station_KilnBuild, Station_KilnTierEffects, Flow_KilnSmoke, Flow_KilnAsh]
```

---

## §XVIII.7 Worksheet 7 — Flow: the boiling process

```yaml
process: boiling
consume: {water: 1, fuel: small, power: none}
produce: {boiled_water: 1}
byproducts: {steam: atmosphere (benign), waste_heat: thermal (local band)}
hazards: {scald: authored minor risk without care}
meters: {fuel: owner read, heat: band}
tests: [Flow_BoilingInputs, Flow_SteamRouted, Flow_HeatLocal]
```

---

## §XVIII.8 Worksheet 8 — Knowledge: water hygiene

```yaml
knowledge: basic_water_safety
sources:
  study: {prereq: none, time: 1 day, station: none}
  salvage: {item: medical_pamphlet, chance_band: likely}
grants:
  recipes: [water_filter, boiled_water process]
  capabilities: [water inspection]
consumers: [recipe loader, water system inspection action]
tests: [Knowledge_WaterSafetyGrants, Knowledge_WaterSafetyIdempotent]
```

---

## §XVIII.9 Worksheet 9 — Policy: foundry shift end

```yaml
trigger: shift_end
effects:
  labor_strain: +0.03 (W3-03)
  spoilage: if materials hot (facility condition check)
  safety_inspection: authored probability (seeded) per condition band
keys: policy:<batch-ids>:shift_end
tests: [Foundry_ShiftPolicyOnce, Foundry_SpoilageAuthored]
```

---

## §XVIII.10 Worksheet 10 — Chain: brine to soap (multi-step)

```yaml
steps:
  1 brine -> chlor_alkali -> caustic_soda
  2 fat + caustic_soda -> saponify -> soap
  3 soap -> consumers (hygiene needs + trade)
bottlenecks: caustic output vs. saponify capacity
byproducts: glycerin (consumer: authored chemistry) + waste_heat
hazards: caustic handling (authored burn events)
tests: [Chain_BrineSoap_Bottleneck, Flow_CausticHazard, Chem_SaponifyYield]
```

**Checks that matter:** glycerin consumer exists (else stranded); caustic
hazard routed; chain depth 3 (healthy).

---

## §XVIII.11 The worksheet pack summary

| # | Worksheet | Point exercised |
|---|---|---|
| 1 | bread | sources/consumers/heat |
| 2 | bandage | pharma/quality/hazards |
| 3 | glass pane | station/cullet loop |
| 4 | printed notice | printing/narrative consumer |
| 5 | water filter | general/knowledge gate |
| 6 | kiln | station tiers/byproducts |
| 7 | boiling | flow/meters |
| 8 | water hygiene | knowledge idempotency |
| 9 | shift end | policy keys |
| 10 | brine-soap | chain/hazards/consumers |

Every worksheet cross-references its tests; the pack doubles as the
implementation reference during phases P1–P10.

---

*End of Part XVIII. W3-05 closes here (complete).*# W3-05 · PART XIX — TEST SCENARIO BOOK: SCRIPTED WALKTHROUGHS

> Full scripted scenarios for the nine kits — step sequences, expected
> outputs, and the assertion lines. The verifier runs these as-is.

---

## §XIX.1 Scenario R1 — General recipe end-to-end

```text
setup: workshop tier 1; knowledge basic_water_safety; inputs stocked
step 1  open craft view; assert readiness ready
step 2  assert requirement rows: knowledge ok, station ok, inputs ok
step 3  execute; assert ledger entry
step 4  assert inputs deducted exactly once (compare before/after)
step 5  assert output in inventory (or tray)
step 6  assert byproduct routed (sweepings in facility)
step 7  repeat execute to depletion; assert block message per missing input
assertions: no free outputs; message refs present; ledger attribution
```

## §XIX.2 Scenario R2 — Recipe blocked, each requirement

```text
R2a knowledge missing -> block reason kind=knowledge; execution refuses
R2b station missing   -> block kind=station
R2c station tier low  -> block kind=station (tier)
R2d power missing     -> block kind=power
R2e heat unavailable  -> block kind=heat
R2f tool broken       -> block kind=tool
R2g environment bad   -> block kind=environment (ventilation)
R2h labor unavailable -> block kind=labor
assert: each block produces its authored message; no execution
```

## §XIX.3 Scenario R3 — TOCTOU

```text
step 1  view says ready (power available)
step 2  script state change: power cut before execute
step 3  execute -> assert blocked at execution (kind=power)
step 4  no inputs consumed (atomicity)
step 5  author interruption behavior if craft started: pause/fail per class
assert: re-check enforced; no free output
```

## §XIX.4 Scenario F1 — Flow ownership

```text
step 1  craft smelt process
step 2  assert consume lines deducted from inventory
step 3  assert slag delivered to facility storage
step 4  assert exhaust delivered to ventilation binding (counter on owner)
step 5  assert power draw recorded by grid owner
step 6  assert craft ledger matches all streams
assert: no unowned stream; no double consumption
```

## §XIX.5 Scenario F2 — Mass balance (B report)

```text
step 1  run 20 crafts of a process
step 2  sum inputs, outputs, byproducts
step 3  compare against authored tolerance
assert: within tolerance OR deviation listed with suspected hidden consumer
```

## §XIX.6 Scenario C1 — Chemistry hazard chain

```text
step 1  run chlor-alkali with healthy ventilation; assert no events
step 2  degrade ventilation condition band
step 3  advance days; assert concentration band rises
step 4  assert warning event fired at threshold (bark/scene)
step 5  ignore: assert exposure event routed to health + W3-03 trigger
step 6  shut down: assert concentration decays (authored curve)
step 7  repair ventilation: assert materials consumed; band normal
assertions: warning-before-harm; routed exposure; no silent injury
```

## §XIX.7 Scenario K1 — Knowledge acquisition paths

```text
K1a study source: spend time; assert grant once
K1b salvage source: seeded success; assert grant once
K1c archive source: decrypt seeded; assert grant once
K1d second acquire any path: assert NoOp; no duplicate recipe/capability
K1e consumer check: recipe unlocks; facility tier unlocks; narrative gate
assert: idempotent grants; consumers read eligibility
```

## §XIX.8 Scenario S1 — Station lifecycle

```text
step 1  build station: assert costs consumed; id in facility owner
step 2  craft tier-1 recipe; assert speed band 1.0
step 3  upgrade tier 2: assert costs; speed/quality deltas measurable
step 4  skip maintenance: assert condition decays per authored rate
step 5  degraded condition: assert speed/quality penalties
step 6  maintenance: assert materials; condition restored
assert: tiers have effects; decay authored; repairs cost
```

## §XIX.9 Scenario L1 — Legibility equivalence

```text
step 1  open view for each requirement combination (matrix)
step 2  assert readiness == execution result for each
step 3  assert cost block equals owner values
step 4  assert quality bands shown when variable
step 5  change state between view and execute; assert re-check + notice
assert: no lies; no UI math
```

## §XIX.10 Scenario M1 — Chain model accuracy

```text
step 1  run a production day; record actual outputs
step 2  compare model projection
step 3  assert within authored error (±15% proposal)
step 4  assert reported bottleneck matches the actual limiter
step 5  assert model writes nothing (state diff empty)
assert: honest read model
```

## §XIX.11 Scenario P1 — Policy once

```text
step 1  trigger batch_complete; assert policy applied once (key)
step 2  trigger shift_end; assert separate key applied once
step 3  replay the same batch id; assert NoOp (duplicate log)
step 4  assert effects routed to owners (strain/obligation/incident)
assert: once-per-occurrence
```

---

## §XIX.12 The scenario coverage matrix

| Scenario | Points exercised | Kit |
|---|---|---|
| R1 | 1,2,3,9 | KR/KG/KL |
| R2 | 2 | KG |
| R3 | 2 | KG |
| F1/F2 | 3 | KF |
| C1 | 5 | KC |
| K1 | 7 | KK |
| S1 | 8 | KS |
| L1 | 9 | KL |
| M1 | 10 | KM |
| P1 | 4 | KP |

All ten points covered by the scenario book; every scenario has scripted
assertions (no manual judgment).

---

*End of Part XIX. Continues in Part XX (verification commands and close).*# W3-05 · PART XX — VERIFICATION COMMANDS, EVIDENCE, AND CLOSEOUT CONFIRMATION

---

## §XX.1 Command inventory (as verified at P0; placeholders until then)

```bash
# static
bash scripts/ci/generate-craft-reachability.sh --check
bash scripts/ci/generate-knowledge-graph.sh --check
bash scripts/ci/craft-requirements-check.sh
bash scripts/ci/craft-flow-check.sh

# focused kits (xUnit)
bash scripts/run_test.sh Ashfall.Core.Tests/Crafting
bash scripts/run_test.sh Ashfall.Core.Tests/Research
bash scripts/run_test.sh Ashfall.Core.Tests/Foundry

# host checks
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --7day-smoke-selftest
```

P0 verifies which commands exist and their actual names; the runbook's command
block is updated before execution begins.

---

## §XX.2 Evidence tree

```text
docs/evidence/w3-05/
  T1-reachability-<date>.yaml
  T1-knowledge-<date>.yaml
  T1-requirements-<date>.yaml
  T1-flows-<date>.yaml
  T2-<kit>-<date>.yaml          (nine)
  T3-<date>.yaml + flows.csv
  findings/CR-###.md
  repairs/CR-###.md
  baseline/RECIPE_BASELINE-<date>.md
  P0_CRAFT_PREMISE.md
```

## §XX.3 Evidence minimum per finding

```text
finding id, class, scenario, expected, observed, owner, repair, before/after,
command, HEAD, reviewer
```

---

## §XX.4 Closeout confirmation checklist

```text
[ ] P0 premise filed; owners verified
[ ] reachability map + baseline; ratchet holds
[ ] requirements enforced (all kinds)
[ ] flows owned; mass-balance report filed
[ ] foundry cohesion map; policy keys verified
[ ] chemistry chains reached; hazards routed and warned
[ ] catalog binds complete; data-only notes authored
[ ] knowledge graph clean; grants idempotent
[ ] station gates enforced; tiers effective
[ ] legibility truthful (view = execution)
[ ] chain model accurate; (C) WIP signed
[ ] T3 soak assertions green
[ ] closeout memo written; Annex U releases recorded
```

---

## §XX.5 The closeout memo (W3-05 version)

```text
OUTCOME: production stack audited; recipes classified; requirements enforced;
flows owned; foundry policy once; chemistry hazards routed; knowledge
idempotent; stations gated; surfaces truthful; chains modeled.
FILES: docs/crafting/*.md; docs/research/KNOWLEDGE_GRAPH.md; kits; evidence.
CONTRACT: reachability classes; requirement kinds; flow ownership; policy
keys; knowledge idempotency; station id space; surface source tables.
COMMANDS: T1/T2/T3 + results summary.
LIMITATIONS: mass balance report-only (B); WIP deferred unless signed;
quality-band consumers depend on W3-02/W3-04 interfaces; family notes.
SHARED PATHS TOUCHED: <list>
LEDGER PROPOSALS: <debt rows>
ANNEX U RELEASES EARNED: expansions 27-31 substrate; W3-04 repair loop;
W3-02 supply; EXP crystal.
```

---

## §XX.6 Post-closeout maintenance commitments

```text
- new recipes classified within the sprint they land
- baselines reviewed monthly; ratchet never grows silently
- chain model re-sampled per release
- expansion families pass their entry gate before content release
- tone/register review for all process names through W2-06
```

---

*End of Part XX.*

# W3-05 · PART XXI — FINAL CONTROL AND READING CARD

```text
ASHFALL W3-05 · READING CARD (FINAL)

CORE RULE:      nothing from nowhere; nothing to nowhere
SECOND RULE:    gates that gate; unlocks that open; waste with homes
THIRD RULE:     surfaces tell the truth; models read owners
FOURTH RULE:    seeded yields; banded quality; shallow chains
FIFTH RULE:     fictional register only

FILES:          docs/crafting/*, docs/research/KNOWLEDGE_GRAPH.md
SIGNATURES:     Annex U + §VIII.5
STOP-LINES:     free outputs; unowned byproducts; paper gates; repeat unlocks
STOP CONDITIONS: T1 green; kits green; soak assertions; ratchet holding
```

**W3-05 is complete.** Proposal only; executes nothing. Binding within this
document: the never-cross list (§VIII.2.1), flow ownership, idempotent grants,
the ratchet, and the review standard.

*Document control: W3-05 · Wave 3 (expanded, final) · HEAD 5be1a30a · end of
W3-05.*# W3-05 · PART XXII — THE STRING AND ID REGISTER (CORPUS HANDOFF)

> Every string and id the plan's systems need, listed for the corpus
> handoff (W2-06) — a working register, not prose. Proposal-only.

---

## §XXII.1 Requirement block messages

```text
craft_msg_knowledge            "you don't know how to make that yet"
craft_msg_station_missing      "you need {station}"
craft_msg_station_tier         "{station} isn't built for that yet"
craft_msg_tool                 "you need working {tool}"
craft_msg_power                "nothing to power it"
craft_msg_heat                 "it can't get hot enough"
craft_msg_environment          "the air can't take it here"
craft_msg_labor                "nobody's free to do it"
craft_msg_inputs               "you're short on {item}"
craft_msg_space                "there's no room for it"
```

## §XXII.2 Surface labels

```text
craft_surface_ready            "ready"
craft_surface_blocked          "blocked"
craft_surface_cost             "costs"
craft_surface_time             "takes"
craft_surface_output           "makes"
craft_surface_quality_note     "quality varies"
craft_surface_requeue          "the world changed; price/needs updated"
chain_surface_bottleneck       "waiting on {step}"
chain_surface_starve           "{item}: about {days} days left"
```

## §XXII.3 Process and station display names (fictional register)

```text
station_kiln                   "kiln"
station_glassworks             "glassworks"
station_press                  "press"
station_workbench              "workbench"
station_crucible_furnace       "crucible furnace"
process_smelt                  "smelting"
process_boil                   "boiling"
process_chlor_alkali           "caustic work"
process_fischer_tropsch        "fuel synthesis"
process_cvd                    "crystal growth"
process_milling                "milling"
```

Real-world trademarked/industrial labels are rejected; these are the in-world
register (W2-06 owns final wording).

## §XXII.4 Knowledge display names

```text
know_basic_water_safety        "water safety"
know_basic_smelting            "basic smelting"
know_alloy_work                "alloy work"
know_basic_glassmaking         "glassmaking"
know_basic_baking              "baking"
know_printing                  "printing"
know_chlor_alkali              "caustic chemistry"
```

## §XXII.5 Stable id spaces (for owners)

```text
stations:   station_<family>_<name>          (facility owner space)
knowledge:  know_<topic>                     (research owner space)
recipes:    recipe_<output>[_<variant>]      (craft owner space)
items:      existing item id space (no new namespace)
processes:  process_<verb>                   (foundry/engine space)
```

Rule: any new id must be checked against the owner's registry before authoring;
duplicate ids fail the static check.

---

## §XXII.6 The register maintenance

```text
[ ] every new string registered before seal
[ ] every id checked against owner space
[ ] display strings routed through W2-06/freeze when declared
[ ] removed strings removed from the register (no ghosts)
```

---

*End of Part XXII. Continues in Part XXIII (Q&A third set and close).*# W3-05 · PART XXIII — FINAL Q&A, TRAP LIST, AND CONTROL

---

## §XXIII.1 Q&A third set (Q66–Q80)

**Q66. What is the very first thing to build?**
The source/consumer graphs. Everything else classifies against them.

**Q67. What if an owner API doesn't exist for a requirement kind?**
P0 finding; propose the binding (signed) or author an exception; never pass
silently.

**Q68. Can recipes be locked behind narrative only?**
Yes — knowledge grants can come from narrative events (W3-01) as long as the
grant is keyed and the consumer verified.

**Q69. How do we handle temporary/seasonal inputs?**
Source entries carry availability windows (authored); the reachability
classifier warns when a recipe is only seasonally craftable (a reviewed
state).

**Q70. What about fuel amnesty (small quantities ignored)?**
Valid as an authored rule (e.g., candle recipes); the rule is data, not a
code branch.

**Q71. How are master recipes (multiple outputs) handled?**
Each output has its consumer checked; the byproduct list covers extras; no
hidden outputs.

**Q72. What if two stations can make the same item at different quality?**
Both entries exist; quality bands differentiate; the consumer reads bands.

**Q73. Should stations be gated by power tier?**
Yes where authored; power requirements are a requirement kind.

**Q74. How is labor skill represented?**
Skill bands on the work owner (C3 mastery); consumers read bands; no local
skill stats.

**Q75. Can a player craft while starving?**
Policy question (authored): labor availability from the schedule/needs; the
plan only ensures the gate reads real state.

**Q76. How do we avoid tooltip/readme drift?**
Surfaces render owners; docs are generated where possible; authored docs
carry review dates.

**Q77. What is the acceptance for a family marked data-only?**
A reason, an owner, and an expiry/review date. Data-only without a note is a
finding.

**Q78. How do repairs interact with old saves?**
Recipe/station data is not save state; facility state is; repairs to data do
not need migration. Any exception is flagged at P0.

**Q79. What is the single most valuable gate?**
The reachability baseline ratchet: it converts a static content problem into
a monotone improvement without a big-bang cleanup.

**Q80. When is the plan "done"?**
When the completion meter (§XI.5) is full and the closeout memo is filed.

---

## §XXIII.2 The trap list (for builders)

```text
TRAP 1  "I'll source it later"       -> INPUT-DEAD forever; source now
TRAP 2  "the UI can compute it"      -> purity break; render owners
TRAP 3  "just this once" on keys     -> double-apply culture; key everything
TRAP 4  "byproducts don't matter"    -> silent growth; route streams
TRAP 5  "it's only a small hazard"   -> unrouted exposure; route + warn
TRAP 6  "knowledge is harmless"      -> repeat unlocks; key grants
TRAP 7  "one more tier"              -> decorative tiers; author effects
TRAP 8  "prose is a gate"            -> paper requirement; enforce
TRAP 9  "flavor names are fine"      -> real-world weight; fictional register
TRAP 10 "the model can estimate"     -> authority drift; read owners only
```

---

## §XXIII.3 The ten-point sanity re-check (before close)

```text
1.  every recipe has a source and a sink
2.  every requirement is enforced and explained
3.  every stream has an owner; no free outputs
4.  every policy/unlock happens once
5.  every station tier changes something
6.  every surface equals execution
7.  every chain is derived, read-only (until signed)
8.  every variance is seeded; every quality banded
9.  every family is bound or noted
10. every expansion family passes its entry gate
```

---

## §XXIII.4 Version record and control

| Version | Change |
|---|---|
| v2.8 | Part XIX test scenario book |
| v2.9 | Part XX verification commands + closeout |
| v3.0 | Part XXI final card |
| v3.1 | Part XXII string/id register |
| v3.2 | Part XXIII this final control |

**W3-05 is complete.** Proposal only. Execution requires Annex U and §VIII.5
signatures. Binding: never-cross list, flow ownership, idempotent grants,
ratchet, review standard.

> Nothing comes from nowhere; nothing goes nowhere; every gate keeps its
> word.

*Document control: W3-05 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-05.*# W3-05 · PART XXIV — ACCEPTANCE RECAP AND EVIDENCE INDEX (FINAL)

---

## §XXIV.1 The acceptance recap (one page)

```text
POINT 1  REACHABILITY   every recipe classified; baseline ratcheted
POINT 2  REQUIREMENTS   every kind enforced at execution; messages present
POINT 3  FLOWS          every stream owned; no free outputs; mass balance filed
POINT 4  FOUNDRY        cohesion map; policy once; ventilation single path
POINT 5  CHEMISTRY      chains reached; hazards routed and warned; utilities metered
POINT 6  CATALOGS       families bound or noted; surfaces list recipes
POINT 7  KNOWLEDGE      graph clean; grants keyed; consumers verified
POINT 8  STATIONS       one id space; gates enforced; tiers effective
POINT 9  LEGIBILITY     view equals execution; reasons per kind; no UI math
POINT 10 CHAINS         read model accurate; bottleneck reasons honest
(C)      WIP            signed; reservation integrity; round-trip
```

## §XXIV.2 The kit recap

| Kit | Asserts |
|---|---|
| KR Recipe_* | crafts end-to-end; flows; yields |
| KG Requirements_* | each block kind; TOCTOU |
| KF Flow_* | once-consume; byproduct owners; meters |
| KP Foundry_* | policy keys; ventilation; lifecycle |
| KC Chem_* | reached; hazards; seeded yields; maintenance |
| KK Knowledge_* | grants once; graph clean; consumers |
| KS Station_* | gates; tiers; decay |
| KL Legibility_* | truth; reasons; no math |
| KM Chain_* | accuracy; read-only; (C) WIP |

## §XXIV.3 Evidence index

```text
docs/evidence/w3-05/
  T1-{reachability,knowledge,requirements,flows}-*.yaml
  T2-{KR,KG,KF,KP,KC,KK,KS,KL,KM}-*.yaml
  T3-*.yaml + flows.csv
  findings/CR-*.md    repairs/CR-*.md
  baseline/RECIPE_BASELINE-*.md
  P0_CRAFT_PREMISE.md
```

## §XXIV.4 The four stop-the-line failures (restated)

```text
1. free output (no consumption)        -> economy break
2. unowned byproduct (silent stream)   -> invisible accumulation
3. paper requirement (gate that fails) -> illusory tech tree
4. repeat unlock (grant twice)         -> progression break
```

Any of these open blocks closeout; they are reason alone to halt a phase.

## §XXIV.5 The rollback rules

```text
[ ] every repair is a data edit with before/after classification
[ ] station/flow changes are config, not save migration
[ ] any save-shape change (C WIP) has its own signed rollback plan
[ ] ratchet baselines are versioned; reverting a repair restores its line
[ ] no repair ships without its test
```

---

*End of Part XXIV.*

# W3-05 · PART XXV — CLOSING TABLES AND FINAL CONTROL

## §XXV.1 The family priority table (execution waves)

```text
Wave 1 (primary): general craft, pharma, chemistry, foundry, milling
Wave 2 (second):  glass/ceramics, shelter engines, salvage, extraction,
                  blueprints
Wave 3 (as feasible): paper, robotics
```

## §XXV.2 The interface table (owners and reads)

| Interface | Owner | Read by |
|---|---|---|
| research eligibility | ResearchSystem | recipe loader, stations |
| facility state | shelter/foundry owner | requirements |
| inventory counts | inventory owner | inputs/outputs |
| utilities (power/heat) | grid/thermal owners | requirements |
| ventilation | ventilation binding | byproducts |
| schedule/labor | roster owner | requirements |
| economy quotes | market owner (W3-02) | trade-grade outputs |
| condition bands | equipment owner (W3-04) | tools/repairs |

## §XXV.3 The definition of done (final)

```text
[ ] §XI.5 completion meter full
[ ] §XXIV.1 recap all true
[ ] evidence pack filed
[ ] closeout memo written
[ ] Annex U releases recorded
[ ] no stop-the-line open
[ ] (C) items signed or excluded
```

## §XXV.4 The final word

Industrial play in ASHFALL is the slow argument that the world can be made to
carry people again. This plan keeps that argument honest line by line: what
comes in, what goes out, who says yes, and what the surface promises. If the
plan is followed, a player can look at their workshop and know exactly why the
kiln is cold, what it will take to light it, and what changes when it burns.
That knowledge — not the output number — is the real product of industry.

> Keep the chains true.

**W3-05 complete at 180k-class.** Proposal only; no execution without Annex U
and §VIII.5 signatures.

*Document control: W3-05 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-05.*# W3-05 · PART XXVI — THE REFERENCE CARD PACK

> One-screen cards a builder can print: the flow check, the knowledge check,
> the station check, and the chain check. Final operational cards.

---

## §XXVI.1 The flow card

```text
BEFORE A RECIPE SHIPS, ANSWER:
  where does every input come from? (source chain)
  who deducts them? (one owner)
  where do outputs go? (consumer)
  where do byproducts go? (owner per stream)
  what meters the utilities? (power/heat/water)
  what does the ledger record? (attribution)
IF ANY LINE IS BLANK: STOP.
```

## §XXVI.2 The knowledge card

```text
KNOWLEDGE SHIPS WHEN:
  at least one source is reachable
  grants are keyed (once)
  every grant has a consumer (recipe/capability/gate)
  chance sources are seeded
  display strings are registered
IF A GRANT OPENS NOTHING: REWORK.
```

## §XXVI.3 The station card

```text
STATION SHIPS WHEN:
  build cost uses sourced items
  id lives in the facility owner's space
  every tier unlocks or improves something measurable
  condition decays and maintenance exists
  power/space requirements are read, not written
IF THE TIER IS DECORATIVE: CUT IT.
```

## §XXVI.4 The chain card

```text
A CHAIN IS HEALTHY WHEN:
  depth ≤ 6 steps (review beyond)
  every step's bottleneck is named
  projections are ranges, not points
  the model reads owners and writes nothing
  a starving input is visible before it bites
IF THE MODEL WRITES: STOP. (authority break)
```

## §XXVI.5 The surface card

```text
A CRAFT SURFACE IS TRUE WHEN:
  readiness equals execution (re-checked)
  every block names its missing requirement
  costs come from owners
  quality shows as bands when variable
  nothing is computed locally
IF THE SURFACE LIES: PULL IT.
```

---

*End of Part XXVI. Continues in Part XXVII (the final control).*# W3-05 · PART XXVII — CROSS-PLAN CHECKLISTS, INDEX, AND FINAL CONTROL

---

## §XXVII.1 Cross-plan interface checklists

### XXVII.1.1 With W3-02 (economy)

```text
[ ] crafted outputs listed as market goods where traded
[ ] quality bands consumed by pricing (if authored)
[ ] input prices read at craft execution (quote API)
[ ] no parallel price tables in crafting
[ ] caravan/supply blocks include production outputs (flow model)
```

### XXVII.1.2 With W3-04 (combat/security)

```text
[ ] repair recipes exist for condition bands (weapons/armor/tools)
[ ] ordnance crafting/production chains reachable
[ ] material consumption in repairs matches W3-04 tables
[ ] no free repairs anywhere
```

### XXVII.1.3 With W3-01 (narrative)

```text
[ ] knowledge grants from archives/scenes registered
[ ] printed materials consumed by narrative props
[ ] item lore strings registered (W2-06)
[ ] discovery/projection consumers wired to narrative where authored
```

### XXVII.1.4 With W3-03 (psychology)

```text
[ ] hazard exposure triggers routed
[ ] care supplies (bandages/medicine) consumed in care
[ ] labor strain from crafting shifts routed
[ ] mastery/labor effects do not bypass owners
```

### XXVII.1.5 With W3-06 (UI)

```text
[ ] craft surface source table complete
[ ] block messages per requirement kind
[ ] chain model board surface (read-only)
[ ] no UI math; re-render on state change
```

---

## §XXVII.2 Full artifact index (W3-05)

```text
docs/crafting/
  RECIPE_REACHABILITY.md      (generated)
  SOURCE_CHAINS.md            (generated)
  OUTPUT_CONSUMERS.md         (generated)
  REQUIREMENTS.md             (generated)
  FLOW_ROUTING.md             (authored)
  FOUNDRY_COHESION.md         (audit)
  CHEM_CHAINS.md              (authored)
  CHAIN_MODEL.md              (authored spec)
  STRING_ID_REGISTER.md       (register)
  BASELINE.md                 (ratchet)
docs/research/
  KNOWLEDGE_GRAPH.md          (generated)
docs/evidence/w3-05/          (evidence)
```

---

## §XXVII.3 The plan's ten rules (final form)

```text
1. sources first
2. sinks second
3. gates are enforced promises
4. streams have owners
5. unlocks happen once
6. tiers change something
7. surfaces tell the truth
8. variance is seeded; quality is banded
9. chains are derived until signed
10. the register stays fictional
```

---

## §XXVII.4 The final control

**W3-05 is complete at the expanded target.** Parts I–XXVII. Proposal only;
no execution without Annex U (Part I §U.2) and §VIII.5 signatures. Binding
within this document: the never-cross list (§VIII.2.1), flow ownership,
idempotent grants, the ratchet, and the review standard (§XV.4).

Two closing lines for the record:

> A recipe is a promise that the world can be shaped. Keep the promise, and
> the wasteland becomes a workshop; break it, and the workshop becomes a
> cupboard of useless parts.

> Nothing comes from nowhere. Nothing goes nowhere.

*Document control: W3-05 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-05.*# W3-05 · PART XXVIII — CLOSING APPENDIX: THE COMPLETION STATEMENT AND METRICS

---

## §XXVIII.1 What "complete" means for W3-05

```text
COMPLETE means, in order:
  1. the maps exist and regenerate (reachability, sources, consumers,
     requirements, flows, knowledge)
  2. the gates run in the focused pipeline and the ratchet holds
  3. the kits pass (nine)
  4. the soak holds (no free outputs; policy once; hazards warned;
     maintenance honored; model accurate; determinism)
  5. the surfaces tell the truth (legibility)
  6. the (C) items are signed or excluded
  7. the closeout memo and Annex U releases are filed
```

## §XXVIII.2 The metric definitions

| Metric | Definition | Target |
|---|---|---|
| reachability coverage | classified recipes / total | 100% |
| free-output count | ledger-detected free outputs in soak | 0 |
| unowned byproduct count | streams without owner at runtime | 0 |
| duplicate policy applications | key violations logged | 0 |
| knowledge duplicate grants | re-grant events | 0 |
| station gate misses | execution without requirement | 0 |
| surface lies | readiness/execution mismatches | 0 |
| model error | projection vs. actuals (absolute) | ≤15% |
| seed determinism | replay mismatches | 0 |
| ratchet delta | baseline growth per phase | ≤0 |

## §XXVIII.3 The consolidation rule

```text
every metric above maps to exactly one kit/check;
no metric is tracked by hand;
dashboards are generated from evidence, not authored.
```

## §XXVIII.4 The final signature line

```text
I, the undersigned, confirm that the completion meter is full,
the stop-lines are clear, the evidence is filed, and the binding rules of
W3-05 are intact. Signed: ________  Date: ______
```

*End of Part XXVIII. W3-05 closes complete.*

---

# W3-05 · CLOSING ADDENDUM — THE CHAIN AUDIT RECORD

## The chain audit record template

```text
CHAIN: <name>            DATE: ____
Steps: ______________
Sources: each step's raw entry verified: [ ]
Consumers: terminal verified: [ ]
Flows: streams owned: [ ]  meters: [ ]
Gates: requirements enforced: [ ]
Determinism: yields seeded: [ ]
Depth: ____ (≤6 preferred)
Result: READY | RETURNED (notes: ____)
```

## The final chain ledger (to fill)

| Chain | Steps | Terminal | Result |
|---|---|---|---|
| ore→tool | 6 | equipment/trade | |
| brine→soap | 3 | hygiene/trade | |
| grain→bread | 3 | food | |
| sand→lens | 4 | optics/science | |
| rag→paper→notice | 4 | narrative/economy | |
| relic→parts | 2 | repair/craft | |

## Closing line

> Every chain is an argument with the wasteland. Keep the argument honest:
> from the ground, to the hand, to the place it is used — and back again.

**W3-05 complete (180k-class).** Proposal only; execution requires Annex U
and §VIII.5 signatures.

*Document control: W3-05 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-05.*

---

# W3-05 · PART XXIX — FINAL REVIEW CARD AND METRICS CLOSE

## XXIX.1 The final review card

```text
CRAFTING SHIPS WHEN:
  sources and sinks exist for every recipe
  requirements are enforced and explained
  streams are owned; no free outputs
  unlocks happen once
  stations gate and tiers matter
  surfaces equal execution
  chains are derived and accurate
  yields are seeded; quality is banded
  the register stays fictional
```

## XXIX.2 The final metric run (expected shape)

```yaml
run: T3-final
seeds: 20 days: 60
recipes_classified: 100%
free_outputs: 0
unowned_byproducts: 0
policy_duplicates: 0
knowledge_duplicates: 0
gate_misses: 0
surface_lies: 0
chain_model_error_max: 0.15
determinism: pass
ratchet_delta: 0
```

## XXIX.3 The closing statement

```text
Every recipe is a sentence in the shelter's argument with the world:
this is where it comes from, this is what it makes, this is who it is for.
Keep the sentences true.
```

**W3-05 complete.** Proposal only; execution requires Annex U and §VIII.5.

*Document control: W3-05 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-05.*

---

# W3-05 · PART XXX — THE FINAL LINE AND CLOSE

```text
Nothing comes from nowhere.
Nothing goes nowhere.
Every gate keeps its word.
Every unlock opens something.
Every waste has a home.
Every surface tells the truth.
```

**W3-05 complete.** Proposal only; no execution without Annex U and §VIII.5.

*Document control: W3-05 · Wave 3 (expanded, final) · HEAD 5be1a30a ·
end of W3-05.*

---

# W3-05 · PART XXXI — THE ANSWER KEY

```text
phantom input        -> source chain or archive
output no consumer   -> consumer or data-only note
paper requirement    -> enforce + message
unowned byproduct    -> routing table
policy double-apply  -> occurrence key
hazard unrouted      -> owner + warning
repeat unlock        -> idempotency key
id-space drift       -> owner id space
surface lie          -> render owner reads
chain guess          -> model reads owners
```

**W3-05 complete.** Proposal only; execution requires Annex U and §VIII.5.

*Document control: W3-05 · final addendum · HEAD 5be1a30a.*

---

# W3-05 · FINAL SIGNATURE CARD

```text
ASHFALL WAVE 3 · PLAN 5 · FINAL SIGNATURE
HEAD: ________  Date: ________
[ ] P1 reachability        [ ] P2 requirements      [ ] P3 flows
[ ] P4 foundry             [ ] P5 chemistry         [ ] P6 catalogs
[ ] P7 knowledge           [ ] P8 stations          [ ] P9 legibility
[ ] P10 chains             [ ] T1/T2/T3 green       [ ] ratchet holding
Signed: ________  Foreman: ________
```

*End of W3-05 · final.*

---

# W3-05 · CLOSING MEASUREMENT DECLARATION

```text
The crafting plan is closed when nothing comes from nowhere, nothing goes
nowhere, every gate keeps its word, and the ratchet holds at zero growth.
Measurement is the closeout; assumption is not.
```

*End of W3-05 (final).*