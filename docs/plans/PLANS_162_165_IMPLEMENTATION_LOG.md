# Plans 162–165 — Implementation Log

Journal per ashfall-implement discipline. Companion authority map:
`PLANS_162_165_RECONNAISSANCE.md`. Spec: pasted flagship integration plan
(Plans 162–165), 2026-09-05.

---

## Phase A — Reconnaissance

Status: PASS (pending baseline completion)

Changed:
- `docs/plans/PLANS_162_165_RECONNAISSANCE.md` (new — full authority map)

Result:
- 5 audits complete (agriculture, defense, psychology, wildlife, shared infra).
- 9 documented divergences from plan text; none architecture-invalidating.
  Composition decisions: agriculture layers on GreenhouseSystem; defense
  composes PerimeterDefenseSystem at the Main.Muster raid seam; psychology
  wires the existing Sanatorium as therapy authority; wildlife ecosystem
  extends WildlifeMigrationSystem (single population store).

Baseline:
- (recorded below when the background run completes)

Divergences: see recon doc §F.

---

## Phase B — Shared contracts

Status: NOT STARTED

---

## Phase C — Plan 162 (AgricultureSystem)

Status: PASS (Core + data + host + UI + tests + gates)

Changed:
- `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` (new — plot strain layer, medium/toxicity, pests, authored mutations, compost, harvest enrichment, one-shot narratives; greenhouse remains growth authority, ticked exactly once inside)
- `Assets/Ashfall.Core/Farming/CropStrainCatalog.cs` (new — DTOs + loader + deterministic validator incl. compost value-loop rule)
- `Assets/Ashfall.Core/Farming/NutritionDiversitySystem.cs` (new — 14-day diet window, 5 diversity categories, deficiency events; consequences applied by host via NeedsSystem.Modify)
- Data: `crop_strains.json` (10 strains incl. 2 authored hardy variants, 3 pests, 2 lossy compost recipes), `nutrition_profiles.json` (14 food profiles), `agriculture_items.json` (compost humus + pest treatment dust; registered in ItemCatalogLoader)
- Save: `agriculture` section (`agriculture_save.json`, combined agri+nutrition envelope, checksum + legacy fallback); SaveOrchestrator Save/Restore hooks
- Host: `AgricultureHostSession` + `AgricultureSaveStore` + `Main.Plans162_165.cs` (Setup/Save/actions/environment snapshot); `GreenhouseFoundryDayOwner` now routes the greenhouse tick through agriculture (legacy fallback kept); `InventoryHostSession.OnConsumed` hook → RecordMeal; deficiency morale pressure ≤ 3/day via NeedsSystem
- RNG: per-day forks of `agriculture.pest` / `agriculture.mutation` (stateless continuation)
- UI: `FarmingPanel` (bound, plot-identity selection, real commands with costs, compost section, LastEvent, Escape close) + `farming` registry route (Live) + ConfigureActions + expandedIds + FARMING dashboard nav
- Selftest: `--agriculture-selftest` (11 gates)
- Utilization: crop_strains/nutrition_profiles/agriculture_items registered GAMEPLAY_CONSUMED

Tests:
- AgricultureSystemTests 21 + AgriculturePersistenceTests 8 = 29/29 PASS (`dotnet test -c Release --filter Agriculture`)

Result:
- growth composes power(light)+weather(ash/rad) → greenhouse phases; water bands accumulate plot toxicity once per watering; mutation drawn once per lifecycle at maturity only when pressure ≥ threshold (no draw otherwise — budget test); pests deterministic; compost lossy & atomic; first-harvest/blight narratives one-shot; save/restore continuation equals uninterrupted run; old saves (bare state) load with defaults.

Divergences:
- Plan's 7-phase Growth enum mapped onto existing GreenhouseStage (no parallel phase machine).
- Catalog named crop_strains.json; strains profile EXISTING CropCatalog seeds only.
- Water bands: Clean/Marginal/Unsafe derived from items + WaterTreatmentSystem.incomingContaminationLevel.

Remaining: Phase G cross-plan (weather already feeds env snapshot; raid/wildlife hooks come with Plans 163/165).

Baseline record (Phase A):
- Core build PASS; host build PASS; agriculture tests 29/29; data-integrity PASS (262 catalogs, 0 errors); utilization gate PASS; PanelRouteGateTests 19/19; bridge + save-store-checksum selftests PASS.
- FULL `dotnet test` (Debug) could NOT complete at baseline: two independent full-suite runs (mine + a concurrent stream's) both pegged a testhost at ~99% CPU >1h without finishing — consistent with the 2026-09-05 UI-audit verification record ("Full xUnit run: INCOMPLETE/UNKNOWN"). Filtered Release-config runs used for phase gates.

---

## Phase D — Plan 163 (DefenseSystem)

Status: NOT STARTED

---

## Phase D — Plan 163 (DefenseSystem)

Status: PASS (Core + data + host + raid seam + UI + tests + gates)

Changed:
- `Assets/Ashfall.Core/Defense/DefenseSystem.cs` (new — trap layer: DefenseTrapDefinition, armed/sprung/broken installations with reset ≠ repair, capture outcomes, bounded structured raid log, PerimeterStrengthBreakdown composing PerimeterDefenseSystem, ResolvePreCombatRaid; TrapCatalogLoader + validator)
- Data: `defenses.json` (4 traps: perimeter snare, chokepoint deadfall, concealed capture pit, spike border — lossy costs in scrap_metal)
- Save: `settlement_defenses` section (`settlement_defenses_save.json`)
- Host: `DefenseHostSession` + `DefenseSaveStore` + `Main.Plans162_165` wiring; capture handoff → `EnsurePrisoners().TakePrisoner` (finally wiring the unwired intake); **raid seam**: `Main.Muster.OnIronRaidersRaidExecuted` now resolves traps → perimeter emplacements BEFORE combat — repelled raids never reach survivors, breaches escalate with enemy count scaled to survivors; turret power = grid-level brownout state (no invented room); RNG forks `defense.targeting`/`defense.capture` per raid
- UI: `DefenseGridPanel` (route `defense_grid`, install/reset/repair/drill commands with costs, raid log, perimeter breakdown) + dashboard nav
- Selftest: `--defense-selftest` (10 gates); utilization entries for defenses.json

Tests: DefenseSystemTests 11 + DefensePersistenceTests 5 = 16 new; filter run 40/40 PASS (incl. pre-existing perimeter tests).

Result: static defenses resolve before survivor combat; sprung traps never re-fire without reset; broken traps require repair before reset; captures hand off to the single captive authority; raid log bounded at 50; post-restore engagement equals uninterrupted.

Divergences: turret/wall definitions stay in `perimeter_defenses.json` (PerimeterDefenseSystem owns emplacements — plan §6.2's conditional); `defenses.json` authors only the trap layer; emplacement power is grid-level (power_grid.json has no defense room).

---

## Phase E — Plan 164 (PsychologicalArcSystem)

Status: PASS (Core + data + host + port bridge + UI + tests + gates)

Changed:
- `Assets/Ashfall.Core/Survivors/PsychologicalArcSystem.cs` (new — fictional arc model: Latent→Emerging→Acute→Crisis→Recovering→Resolved; sustained-exposure trigger (never one spike); conditional cooldown-gated behaviors routed to owning authorities; private-stash ledger with conservation; bounded non-stacking catharsis ≤0.15; IsEligibleForWork gate only for shutdown arcs at Acute+; MentalArcCatalogLoader + validator)
- Data: `mental_arcs.json` (4 arcs: compulsive stashing, fire fixation 6% chance Crisis-only, persecutory crisis, shutdown withdrawal)
- Save: `psychological_arcs` section; host `PsychologyArcHostSession`/`SaveStore`
- Sanatorium bridge: `HostSurvivorConditionPort` now composes arc conditions (`arc_*` → HasArc, ApplyRecoveryProgress → ApplyTreatmentProgress) — the wired Sanatorium stays THE therapy authority (plan §7.14 preferred branch)
- Day tick: phase-4 `psychology_arcs_162` owner (after phase-3 needs finalize), forks psychology.arc_trigger/.arc_behavior/.recovery; stress reader = NeedsSystem.Morale; hoarding host callback moves 1 canned_food (>2 held) deterministically; fire requests → ShelterFireHazardSystem.Ignite; refusal → needs morale + relations affinity; withdrawal → hygiene decay
- UI: `PsychologyArcPanel` (route `psychology_arcs`, stash SEARCH/RETURN intervention, work-gate column, treatment progress; therapy stays sanatorium-side) + nav
- Selftest: `--psychology-selftest` (9 gates); utilization entries for mental_arcs.json

Tests: PsychologicalArcSystemTests 13/13 (trigger, stages, treatment, catharsis bounds, stash conservation/discovery/return, fire conditional, work gating, replay equivalence, old-save defaults).

Result: arcs emerge only from sustained canonical stress; behaviors are conditional opportunities; hoarded items are ledgered and returnable (nothing vanishes); escalation events fire once per transition; treatment resolves with bounded catharsis; post-restore continuation matches uninterrupted.

Divergences: acute-stress permille composes the port's existing combat-trauma reading with arc stage; relations penalty targets the first other roster survivor (no global assignment field on Main — documented v1 bound).

---

## Phase F — Plan 165 (WildlifeEcosystemSystem)

Status: NOT STARTED

---

## Phase F — Plan 165 (WildlifeEcosystemSystem)

Status: NOT STARTED

---

## Phase G — Cross-plan integration

Status: NOT STARTED

---

## Phase H — Content closure

Status: NOT STARTED

---

## Phase I — Persistence/replay

Status: NOT STARTED

---

## Phase J — Full CI

Status: NOT STARTED
