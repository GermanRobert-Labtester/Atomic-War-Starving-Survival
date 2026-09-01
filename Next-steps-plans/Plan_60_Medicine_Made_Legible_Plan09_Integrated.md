# Plan 60 — Medicine Made Legible: Plan 09 Integrated & Re-Baselined

> **Origin:** the submitted flagship *Plan 09 — Medical & Disease Depth: Pathogens, Pharma Purpose
> & Palliative Care*. This document **integrates** it: same mission, same non-negotiables, same
> evidence discipline — but re-baselined against repository reality at `ccac926e`, with every one of
> its 34 tasks (9A–9AH) given a disposition, and the remaining work regrouped into six executable
> tasks whose order follows what the code actually needs.
>
> **Mission (unchanged):** make medicine a longitudinal management problem — diagnosed, monitored,
> treated, contained, sometimes endured — **without creating parallel medical systems**.
>
> **Why integration was required:** the submitted plan assumed **7 diseases** and asked for 15. The
> catalog already holds **15**, and all four vectors are covered. It also assumed phases, treatment
> windows and diagnostic tells were authorable; they are **not representable in the current schema**,
> while two authored clinical text fields (`guidance`, `source_note`) are read by **nothing at
> runtime**. So the real gap is not content volume. It is **legibility, causality, and binding**.

---

## 1. Verified re-baseline (replaces the submitted "Verified baseline" table)

| Submitted claim | Repo reality @ `ccac926e` | Consequence |
|---|---|---|
| `disease_catalog.json` — **7 diseases** → target 15+ | **15 diseases**: vectors `water 5 / air 4 / blood 3 / spore 3` | **9A is complete** as data. 9A's *schema* requirements (phases, tell, treatment window) are **not** satisfiable — see row below |
| "Every new disease must define … incubation, **phases**, treatment window, treatment response, diagnostic tell" | Actual disease fields: `id, display_name, vector, incubation_days, illness_days, infectivity, lethality, spread_interval_days, spread_radius, countermeasure_item_id, guidance, source_note` (`Assets/Ashfall.Core/Disease/DiseaseCatalog.cs:83–88` + data). Runtime state is `DiseaseInfectionState`; `TryGetInfection(…, out int daysSick, out bool quarantined)` (`DiseaseSystem.cs:614`) — **two fields only** | There is **no phase model, no severity/trend, no treatment-window, no tell field**. The "3-phase progression" the plan assumes does not exist in the engine |
| Diagnostic readability (9B) is an authoring problem | `guidance` and `source_note` exist in data and are consumed **only by a headless demo** (`DiseaseHeadlessDemo.cs:57–58`, which checks `countermeasure_item_id` presence). No UI or system reads them | **9B is a delivery problem**: authored clinical text is already present and displayed nowhere |
| Player surfaces for disease exist | Disease appears in only 3 UI files: `src/UI/AfflictionsPanel.cs`, `src/UI/MedicalPanel.cs`, `src/UI/FeedbackMessages.cs`. `DiseaseSystem` also exposes `OnInfection` (`:154`), `EventInfection = "disease_infection"` (`:25`), `TotalInfectionsHistory` (`:814`) | Small, thin surface; no trend/prognosis display. 9B/9J become the same piece of work |
| Waterborne/excavation world triggers must be built (9D, 9G, 9H) | `IDiseaseOutbreakSource` exists (`Assets/Ashfall.Core/Disease/IDiseaseOutbreakSource.cs:18`) with a documented source-id convention (`sump_flooding`, `excavation`); `DiseaseSystem.TriggerOutbreak(source, …)` (`:316–317`); **two host implementations already live**: `src/Host/DiseaseOutbreakHostAdapter.cs:32` `SumpFloodingSource`, `:46` `ExcavationSource`, invoking outbreak at `:144` and `:175` | The interface + first two sources are **done**. Remaining: more sources, budget/cooldowns, legibility, and *not* inventing a second contamination authority |
| Dependency care needs items (9L) | `chemical_dependency_items.json` = **13 items** with `item_id, dependency_kind, description, severity` | Coverage exists on the item side; 9L becomes "verify class coverage, reuse first", not "add 4–6" |
| Relapse must integrate with stress (9N) | `ChemicalDependencySystem.ReportStress(survivorId, source, magnitude)` + `OnStressReported` (`:93`, invoked `:148`), rules in a **C# static table** `Medical/StressRelapseRules.cs:30` (`ComputeDelta(magnitude, kind)`). `grep -rn "ReportStress" src/` → **no callers**; `OnStressReported` → **no subscribers** | The producer API exists and is **unbound at both ends**: nothing reports stress into it, nothing listens. 9N is wiring + moving the table to data, not new mechanics |
| Palliative/vigil contract (9R, 9T) | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs:26` with `OnVigilStarted`, `OnNameRecited`, `OnPhantomKnock`, `OnVigilCompleted` (`:42–45`) and state `isActive/dwellerId/phantomKnockFired/wasSkipped/isCompleted`; host-wired at `src/Host/MedicalHostSession.cs:18,35`. `grep -rn "BeginVigil\|Vigil\." src/UI src/Main*.cs` → **zero** | Vigil is a live object with **no reach to the player**. 9R/9T must start with a surface, not with prose — vignettes have nowhere to land today |
| Terminal/palliative field | `SickListSystem.cs`: `SickBand { survivorId, band, diagnosedDay, releaseDay, palliativePlan }` (`:10–14`), `Diagnose` / `Release` / `AssignPalliative` (`:45,62,71`). **`band` is a dose band** (`DoseLedgerSystem.BandGreen..Black`) | `palliativePlan` already exists (9R's field, unauthored). But the sick list is **radiation-band** based, not disease-severity based — the submitted plan's "disease severity feeds Sick List/triage" is a genuine missing link, not a display tweak |
| Ward bed classes used (9I) | `MedicalWardSystem.cs`: `MedicalBedCategory` (`:179`), `Admit(…, bool isolation = false)` (`:170–175`), `MedicalAdmissionStatus` (`:224`), `MedicalWardEventKind` (`:277`) | Categories + isolation flag exist; the question is whether disease content **routes** into them — 9I becomes a contract + test task |
| Memorial outcomes + grief (9V, 9W) | `Memorial/MemorialSystem.cs`: `enum DeathQuality` (`:14`), `enum MemorialOutcome` (`:28`), documented grief multipliers **Peaceful 0.5 / Rushed 1.0 / Unattended 1.25** (`:47–49`), `IGriefSink`. Separately verified: `ApplyGrief` has **1 Core reference (its own declaration) and 3 test files, 0 host callers** | Outcomes + quality model exist. **The grief sink is unbound** — 9V's "death context affects the living" is blocked on one call site, exactly the failure mode this project keeps hitting |
| Final wishes feed vigil (9U) | `final_wishes.json` exists; earlier content scan classified it `exempt_no_source_evidence` with **no consumers** | 9U needs a wish-state consumer before a vigil link; the wish data is currently unread |
| Pharma purpose (9C) | `pharma_recipes.json` = **25 recipes** (confirmed); `countermeasure_item_id` is a **single item per disease**, so "curative/suppressive/symptomatic/supportive" has no schema slot | 9C's role taxonomy requires a small, honest schema addition — or it collapses into one number |
| Cross-system medical state survives saves (9AC) | `medical_disease` **is** one of the 19 day-advance owners; disease state persists | Migration/round-trip work remains (no evidence of mid-illness or mid-vigil fixtures) |

**Net:** of 34 submitted tasks, **2 are already satisfied as data** (9A partially, 9L partially), **6 are
blocked on a missing schema field rather than on content** (9A's phase/tell requirements, 9C's roles,
9B, 9I routing, 9J, 9N), **1 is blocked on a surface that doesn't exist** (9R/9T/9U), and **the rest
(≈24) remain genuinely open**. The integrated plan below reflects that.

---

## 1b. Second-pass findings (deeper source read) — and one correction to §1

A follow-up pass over the disease and vigil code changed the weighting of this plan, so it is
recorded here rather than left implicit.

| # | Verified fact | Consequence for the plan |
|---|---|---|
| F1 | **`ResolveOutcomes` has no treatment input**: `if (patient.days_sick < def.illness_days) continue;` then `bool died = def.lethality > 0f && _rng.NextDouble() < def.lethality` (`Assets/Ashfall.Core/Disease/DiseaseSystem.cs`) | "Every new disease has a valid treatment path" is **unachievable by authoring**. 60C needs one narrow intervention hook (D3) before roles/windows/curative-vs-supportive mean anything |
| F2 | `countermeasure_item_id` is documented as *vector neutralisation* (`DiseaseCatalog.cs:80–85`: water → `clean_water`, air/spore → `gas_mask`/`hazmat_suit`, blood → `antibiotics`) and drives `IsVectorBlocked` (`DiseaseSystem.cs:693–702`) over `water_purified / vents_sealed / tools_sterilized / air_filtration` | The field is **prevention, not cure**. It must not be silently repurposed; the plan's "treatment response" is a different, missing mechanic |
| F3 | Prevention is host-wrapped but **unreachable and sticky**: `src/Disease/DiseaseHostSession.cs:95–109` expose `PurifyWater/SealVents/SterilizeTools/SetAirFiltration`, yet `grep` for those calls outside that file → **0**; `ResetWaterPurification()` also has **0 callers** | No counterplay exists in the UI, and a once-applied protocol would never expire. 60B's "mitigation by existing actions" is therefore *not* satisfied by current code |
| F4 | **Corrections to §1's grief rows (mine, not the submitted plan's):** the sink method is `IGriefSink.ApplyDispersion(...)` (`Memorial/MemorialSystem.cs:52`, invoked at `:190`), and the property `GriefSink` (`:143`) is **never assigned in `src/`**, so that call no-ops in play. Separately `SurvivorRelationsSystem.ApplyGrief(survivorId, amount)` (`:98`) has **no host caller** — `Ashfall.Core.Tests/Memorial/MemorialGriefPortTests.cs` states the same. §1's shorthand "`ApplyGrief` … 1 Core ref + 3 test refs" merged two distinct links | **Grief is doubly unbound**: an injected-sink that is never injected, and a relations method that is never called. Both are wiring, and both are cheap — highest value-per-line in this scope |
| F5 | Vigil is subscribed but never started, and it ticks on **frame time**: `StartVigil(dwellerId, names, duration)` is called only from `src/Host/HostCli.PanelTests.cs:1633,1647`; `src/Host/MedicalHostSession.cs:62–65` subscribes all four events, `:211–219` skips/reports; `VigilStateMachine.Tick(float deltaSeconds)` | 60E's ordering is now mandatory rather than preferable: **surface first, then a day-based clock, then vignettes**. A real-time vigil in a day-advance game is also a determinism hazard |
| F6 | Catalog `SchemaVersion = 1` (`DiseaseCatalog.cs:107`) and `SickBand` has 5 fields (`survivorId, band, diagnosedDay, releaseDay, palliativePlan`) with `band` documented as `DoseLedgerSystem.BandGreen..Black` | Tell/treatment additions are **schema migrations**, not edits — defaults must keep version-1 files loading; and D5's dose-band-vs-disease-severity ambiguity is real, in the type |

**Re-weighted first moves:** D5 bridge → `GriefSink` injection + `ApplyGrief` binding → D3
intervention hook (+ schema 1→2) → vigil surface and day clock → protocol reachability/expiry →
only then catalog and vignette authoring.

Decisions with their evidence and rejected alternatives:
`docs/medical/ARCHITECTURE_DECISIONS.md` (D1–D7).

**Implemented so far** (see the status table in that doc): **D1** derived clinical staging
(`Assets/Ashfall.Core/Disease/DiseaseTriage.cs`), **D5** the illness → sick-list bridge with a
named severity source (`Assets/Ashfall.Core/SickListSystem.cs`, `src/Main.MedicalTriage.cs`, called
from the `medical_disease` day owner), and **D7** the bound grief chain
(`Assets/Ashfall.Core/Memorial/RelationsGriefSink.cs`, `SurvivorRelationsSystem.RelatedIds`,
`SurvivorFateSystem` now supplying quality + mourners). **D4 (protocol expiry) remains open** — treatment, clinical text and the vigil are now live:
`DiseaseSystem.TryTreat` + catalog `schema_version` 2 (`treatments[]`, `tell`, `tell_secondary`,
`timing_clue`), the ward's CLINICAL NOTE / `GIVE … [role]` / `KEEP VIGIL` actions, and
`VigilCare` recording a kept vigil on the consequence ledger where `DeriveDeathQuality` reads it.
Vector protocols still never lapse, and the sick list still shows a band without the clinical note. Verified: `dotnet test` 5402/5402 (29 new),
`dotnet build Ashfall.csproj` 0/0, and `--disease-selftest`, `--medical-selftest`,
`--expansions-selftest`, `--day1-selftest`, `--real-campaign-journey-selftest`,
`--7-day-smoke-selftest`, `triad-drift-gate`, `warning-baseline-gate` all green (`--disease-selftest`
36 → 61 checks; 91 medical/disease xUnit tests).

---

## 2. Task disposition map (9A–9AH → this plan)

| Submitted task | Disposition | Now in |
|---|---|---|
| 9A pathogen catalog | **DONE as data** (15 diseases, 4 vectors). Its phase/tell/window requirements are **re-scoped** to a schema addition | 60A |
| 9B diagnostic tells | **RE-SCOPED** — delivery, not authoring | 60A |
| 9C treatment matrix / pharma purpose | **OPEN**, needs role field | 60C |
| 9D waterborne outbreaks | **PARTIALLY DONE** (`SumpFloodingSource` live) — extend + make legible | 60B |
| 9E airborne/respiratory integration | **OPEN** (do not double-apply with `RespiratoryDegenerationSystem`) | 60B, 60C |
| 9F blood/contact integration | **OPEN** (needs real exposure hooks: wounds, hygiene, reused equipment) | 60B |
| 9G spore/fungal integration | **PARTIALLY DONE** (`ExcavationSource` live) — deepen + differentiate symptoms | 60B, 60A |
| 9H regional/event-driven outbreaks | **OPEN** (trigger matrix, cooldowns, ≥4 templates) | 60B |
| 9I ward routing / bed classes | **OPEN** (contract + routing + tests) | 60C |
| 9J sick-list & prognosis clarity | **MERGED** with 60A (same surfaces); the dose-band/disease-severity mismatch is the real work | 60A |
| 9K dependency class coverage | **OPEN** (enumerate + verify each class has care) | 60D |
| 9L detox-support items | **PARTIALLY DONE** (13 items) — reuse-first verification instead of batch | 60D |
| 9M staged detox protocols | **OPEN** (protocol state does not exist; smallest honest addition only) | 60D |
| 9N relapse ↔ stress | **BLOCKED→WIRING**: producer unbound both ends; table lives in C# | 60D |
| 9O dependency backstories | **OPEN** (narrative; must not become personality) | 60D |
| 9P dependency trade demand | **OPEN** (existing market hooks only, no farming incentive) | 60D |
| 9Q expedition withdrawal | **OPEN** (no second model) | 60D |
| 9R palliative contract | **BLOCKED on a surface** (vigil has 0 UI/Main callers) | 60E |
| 9S comfort-care items | **OPEN**, reuse-first; `palliativePlan` field already exists | 60E |
| 9T vigil vignettes | **OPEN** — needs 60E's surface first | 60E |
| 9U final wishes ↔ vigil | **OPEN** — wish data currently unread | 60E |
| 9V grief via relations | **BLOCKED on one unbound sink** (`ApplyGrief`, test-only) | 60E |
| 9W memorial outcomes ×3 | **PARTIALLY DONE** (`MemorialOutcome` enum + multipliers) — author/deepen, don't duplicate | 60E |
| 9X memorial wall display | **OPEN**, must not build a décor framework | 60E |
| 9Y radio warnings | **OPEN** (respect information availability) | 60B |
| 9Z codex learned-diagnosis | **OPEN** (unlock on diagnosis/treatment, not globally) | 60A, 60B |
| 9AA item/resource balance audit | **OPEN** | 60C |
| 9AB event/outbreak budget | **OPEN** | 60B |
| 9AC save & migration hardening | **OPEN** | 60F |
| 9AD content-utilization gate | **OPEN** (extend existing scan; medical families) | 60F |
| 9AE determinism harness | **OPEN** | 60F |
| 9AF long-horizon medical simulation | **OPEN** | 60F |
| 9AG UI/accessibility regression | **OPEN** | 60F |
| 9AH full medical regression matrix | **OPEN** (10 scenarios) | 60F |

---

## 3. Architecture decisions to make **before** authoring anything

These are the forks the submitted plan assumed away. Each needs a short ADR with a decision, and
each must be settled before content work, because content shape follows schema shape.

| # | Decision | Options | Constraint |
|---|---|---|---|
| D1 | **Phase model** | (a) derive phases from existing `incubation_days` / `illness_days` + `daysSick` (no new field); (b) author explicit `phases[]` | Prefer **(a)** — it makes the catalog *simpler* and adds no parallel timeline; explicit phases only if the derived model can't express a real clinical case |
| D2 | **Diagnostic tell** | (a) promote `guidance`/`source_note` to the surfaces, add `tell_key`; (b) new symptom system | **(a)** only — a second symptom authority is forbidden by the plan's own non-negotiables |
| D3 | **Treatment role taxonomy** | add `treatment_role` (`curative\|suppressive\|symptomatic\|supportive`) to the disease↔item link | Must stay one field on existing structures; single `countermeasure_item_id` becomes an ordered list only if D4 requires it |
| D4 | **Treatment windows** | derive from `daysSick` vs a `treatable_days` bound | Derived, deterministic, no scheduler |
| D5 | **Sick list semantics** | reconcile that `band` is a **dose** band while disease severity is separate | One band ladder with an explicit severity source, or two named fields — never one field meaning two things |
| D6 | **Vigil surface** | reuse `MedicalPanel`/`CaregivingPanel`/sick-list detail vs new panel | No new panel class unless an existing surface demonstrably cannot carry it |
| D7 | **Grief binding** | bind `IGriefSink.ApplyGrief` to the existing relation/morale authorities | One grief authority, already present — this is a call site, not a system |

---

## 4. The integrated work

### Task 60A — Diagnostic legibility: land the authored clinical text, make infection state readable

**Goal:** a player can notice that several survivors who drank from the same intake are following
the same pattern, and can act on that before the panel says "infected".

**Primary files:** `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs` (+ `disease_catalog.json`),
`DiseaseSystem.cs`, `SickListSystem.cs`, `src/UI/AfflictionsPanel.cs`, `src/UI/MedicalPanel.cs`,
`src/UI/FeedbackMessages.cs`, survivor-detail/sick-list surfaces, `docs/medical/` matrices.

**Substeps**
1. Resolve **D1–D2, D5** (ADR per decision) and freeze the field set before editing data — no schema
   edits after authoring starts.
2. Confirm the derived phase model reproduces the plan's 3 stages from existing fields
   (`incubation_days`, `illness_days`, `daysSick`); write the derivation as one pure Core function
   and unit-test its boundaries (day 0, last incubation day, phase transition, recovery day).
3. Add `tell_key` (+ optional `tell_secondary_key`, `timing_clue_key`) to the catalog DTO and JSON,
   snake_case, `schema_version` bumped, all existing 15 diseases filled — no half-migrated catalog.
4. **Route the already-authored `guidance` and `source_note`** into surfaces instead of re-writing
   them; delete from the file anything that duplicates what the engine does not know.
5. Render infection state where decisions happen: sick-list row (disease, phase, trend arrow,
   urgency, isolation, palliative plan) and survivor-detail clinical block — **one authority**,
   panels read, never recompute (`DiseaseSystem.TryGetInfection` is the seam).
6. Add trend without certainty: express "worsening/improving" from the same derivation as phases;
   where diagnosis is uncertain, keep uncertainty visible rather than naming the pathogen.
7. Preserve the testing/diagnosis mechanic (no auto-naming of every infection): diagnosis advances
   through `SickListSystem.Diagnose` + the codex path (60B step 8), not by rendering hidden fields.
8. Symptom text must be concrete clinical language (sputum, fever pattern, rash distribution,
   wound odour, timing after water exposure) and must allow **plausible overlap** between diseases —
   no exclusive one-symptom-per-disease key.
9. Tone gate: no sensationalised terminal or outbreak copy; `source_note` stays diegetic
   (a clinician's note, not a stat line); symptom strings must not contradict each other across
   surfaces (one string table).
10. Text is keyed for localization (never inline UI literals), and each surface is checked for
    overflow at a text-scale-up variant.
11. Build the four matrices as generated artifacts, not typed documents:
    `DISEASE_VECTOR_MATRIX.md`, `DIAGNOSTIC_TELL_MATRIX.md`, `MEDICAL_STATE_MODEL.md`,
    `SICK_LIST_CONTRACT.md` — generated from the catalog + code so they cannot drift.
12. Tests: phase derivation per disease; tell coverage (no disease without at least one, no tell
    shared by every disease); UI-reads-authority tests; snapshot for healthy/early/established/
    severe/quarantined/palliative states; no-duplicate-render-path assertion.

**Acceptance:** every disease is distinguishable *in play* by tell + timing + exposure clue; no
authored clinical text is unread; no second symptom authority exists.

---

### Task 60B — Outbreak causality: world state → exposure → disease, bounded and traceable

**Goal:** disease arrives because of something real (flood, failed filtration, deep dig, crowding,
a caravan that came through a hot sector), and the player can trace it after the fact.

**Primary files:** `src/Host/DiseaseOutbreakHostAdapter.cs`, `Assets/Ashfall.Core/Disease/IDiseaseOutbreakSource.cs`,
`DiseaseSystem.cs`, `WaterTreatmentSystem.cs`, `BrineWaterSystem.cs`, `SumpFloodingSystem.cs`,
`ShelterScheduleSystem.cs` (crowding), `TravelingCaravanSystem.cs`, `District8DeepCoastSystem.cs`,
`RespiratoryDegenerationSystem.cs`, `DecontaminationSystem.cs`, event/outbreak data,
`docs/medical/OUTBREAK_TRIGGER_MATRIX.md`.

**Substeps**
1. Inventory the **two live sources** (`SumpFloodingSource`, `ExcavationSource`) and write the
   source-id convention into `docs/medical/` before adding any third — the interface is already the
   pattern; the discipline is what's missing.
2. Add `IDiseaseOutbreakSource` implementations **only** where a real state exists: water intake /
   failed filtration (`WaterTreatmentSystem`), brine contamination, caravan arrival, crowding
   (bunk/schedule state). Each carries `SourceId`, a scope (which survivors were exposed), and a
   reason string id for attribution.
3. Do **not** create a second contamination authority: reuse existing contamination/filtration
   state as the trigger, and assert in a test that one source id maps to one authority.
4. Implement `TriggerOutbreak` call paths with **eligibility and cooldowns authored in data**
   (min day, season/weather gate, prior-outbreak interval, per-source budget) so "outbreak spam" is
   structurally impossible rather than tuned away later.
5. Build `OUTBREAK_TRIGGER_MATRIX.md`: trigger → vector → diseases → exposure scope → warning
   channel → mitigation (existing actions) → aftermath, with at least **4 event templates** whose
   mitigation is a real player action already in the game (boil, filter, isolate, close a sector,
   stop a dig).
6. Emit an attributable transition per outbreak stage (`outbreak_detected`, `exposure_group_infected`,
   `outbreak_contained`, `outbreak_faded`) through the existing day-event channel so the report says
   *why*, and never a bare count.
7. **Airborne/respiratory coherence (9E):** determine whether `RespiratoryDegenerationSystem` and
   airborne disease share an exposure input; keep them distinct systems, add a documented
   no-double-application rule, and test it explicitly (a survivor in poor air with influenza must not
   take two independent respiratory penalties from one cause).
8. **Blood/contact grounding (9F):** infection only from real hooks — open wounds, contaminated
   tools, reused equipment **only if condition is tracked by an existing authority**, hygiene state.
   No free-floating infection rolls.
9. **Spore differentiation (9G):** fungal presentation must read differently from airborne viral
   (onset timing, no fever spike pattern, excavation/damp provenance), and remain environmentally
   grounded — no fantasy spores, no impossible mutation.
10. **Radio & communication (9Y):** author warnings for floodborne/airborne/spore/caravan cases
   through existing radio channels; a broadcast may only assert what its source could know, and must
   not name an undiagnosed pathogen; mark VO candidates rather than producing them here.
11. **Learned diagnosis (9Z):** codex/journal entries unlock on diagnose, treat, document, or archive
   — never the full catalog at start; include transmission, tells, treatment notes, cautions; keep
   hidden probabilities hidden unless intended.
12. **Event budget (9AB):** cap simultaneous outbreaks and overlapping medical crises with a
   deterministic, data-driven rule, and keep genuine concurrency possible (a bad winter with two
   illnesses is legitimate; five simultaneous plagues is a bug).
13. Tests: per-source determinism, cooldown/eligibility, no-double-respiratory-damage, radio
   information-respect test (no undiagnosed pathogen named), codex unlock path, save round-trip of an
   active outbreak, and a stress test that a 30-day flood + storm cannot avalanche.

**Acceptance:** every outbreak traces to a named authority's state; counterplay exists before the
fact; medical crises overlap but never cascade unfairly.

---

### Task 60C — Pharma purpose and ward routing: make the 25 recipes mean something clinical

**Goal:** treatment becomes a decision with roles, windows and scarcity — and the ward's
categories are actually exercised by the disease content.

**Primary files:** `pharma_recipes.json`, `PharmaLabSystem.cs`, `MedicalTreatmentCatalog.cs`,
`MedicalWardSystem.cs`, `MedicalPipelineCoordinator.cs`, `DiseaseCatalog.cs` (role field),
`AfflictionContracts.cs`, items authority, `docs/medical/DISEASE_TREATMENT_MATRIX.md`,
`WARD_TRIAGE_CONTRACT.md`, `MEDICAL_REWARD_RESOURCE_AUDIT.md`.

**Substeps**
1. Audit all 25 recipe **outputs** (not recipes): map each to disease treatment, supportive care,
   dependency care, palliative use, or *no current use*; publish the table before changing anything.
2. Resolve **D3/D4**: add `treatment_role` + derived `treatable_days`; keep the existing single
   `countermeasure_item_id` unless a disease genuinely needs two-role treatment, in which case an
   ordered list with the same field name family — never a second treatment structure.
3. Reuse underused outputs before authoring any new drug; the submitted plan's pharma rule
   (no batch expansion) is now the **default answer** to "this disease has no treatment".
4. Enforce role distinctness: no output may be `curative` for more than a defined share of the
   catalog; tests assert no universal cure and no single dominant recipe.
5. Make windows matter: treat inside → shorter/severer-phase outcome; treat late → partial response;
   treat never → authored terminal/chronic consequence; all derived from `daysSick`, deterministic.
6. Ensure treatment does **not** instantly erase progression unless the disease says so; chronic
   decline must persist for diseases that declare it (and land in 60D/60E's dependency/palliative
   surfaces rather than in a private health flag).
7. **Ward triage contract (9I):** map severity/contagion → `MedicalBedCategory` + `isolation`;
   document which of the categories the catalog actually exercises, identify the unused ones, and
   reach them through content (or delete the implication that they're used). No new bed class.
8. Bed scarcity must produce a visible tradeoff with an honest refusal — never a silently dropped
   admission, and never a queue that hides a death (impossible-assignment test).
9. Isolation must interact with duty and contact: a quarantined survivor cannot be posted to a shift
   or share a bunk line, routed through the fitness/duty verdict — no second quarantine model.
10. **Resource audit (9AA):** track demand per class (antimicrobials, analgesics, sedatives,
    diagnostics, ward supplies, dependency support) against `PharmaLabSystem` throughput across a
    120-day run; flag impossible demand, overabundance, and infinite-value loops (a treat→trade→treat
    cycle is a bug, not an economy).
11. Consumption must go through the game's single consume/effect path so a dose actually does what its
    item says (no parallel medical consumption), and each application is one attributable event.
12. Tests: role distinctness, window response curves, ward routing per severity, isolation/duty
    interaction, demand-vs-production per class, save round-trip mid-treatment, and a
    determinism check that repeated treatment clicks don't double-dose.
13. Docs: `DISEASE_TREATMENT_MATRIX.md`, `WARD_TRIAGE_CONTRACT.md`,
    `MEDICAL_REWARD_RESOURCE_AUDIT.md` — generated from the catalogs + the pipeline, cited by file:line.

**Acceptance:** every pharma output has a clinical purpose; roles are distinct; ward categories are
exercised; treatment is a timed decision; no exploit loop exists.

---

### Task 60D — Dependency as managed care: bind the unbound hooks, add protocol state, keep it coherent in the field

**Goal:** dependence becomes a supply-and-judgment problem with a viable care path per class —
including in the field — and stress→relapse finally connects, using hooks that already exist.

**Primary files:** `ChemicalDependencySystem.cs`, `Medical/StressRelapseRules.cs`,
`chemical_dependency_items.json`, `GuiltInsomniaSystem.cs`, `SurvivorSocialCoordinator.cs`
(ration conflict/leadership friction/morale), `TraumaBondSystem.cs`, `CombatTraumaSystem`,
`ExpeditionSystem.cs`/`ExpeditionHostSession.cs`, `DutyRosterSystem.cs`, market/trade hooks,
`docs/medical/DEPENDENCY_CLASS_MATRIX.md`, `DETOX_PROTOCOL_MATRIX.md`.

**Substeps**
1. Enumerate the actual dependency-class type in `ChemicalDependencySystem` (do **not** assume the
   plan's "4 classes" — read the code) and build the class matrix: exposure source, tolerance,
   withdrawal profile, craving, relapse hook, care options, ward and expedition implications, save
   semantics.
2. Audit the **13** `chemical_dependency_items.json` entries against those classes; find classes with
   no care path before authoring anything new (9L becomes a coverage proof, not an item batch).
3. **Bind the stress producer side (9N, the headline fix):** report real stress into
   `ReportStress(...)` from the sources that already exist — guilt records, a witnessed death, ration
   conflict, combat trauma, sustained low morale — each with a named `source` string id; no new
   stress accumulator.
4. **Subscribe the consumer side** so care staff, radio/journal and the day report can see
   `OnStressReported` exactly once (single-handler subscription discipline, no double-fire on rebind).
5. Move the relapse rule table from `StressRelapseRules.cs:30` into data with the same semantics
   (kind × magnitude band → delta), keeping `ComputeDelta` pure and tested; the C# table stays as the
   fallback for missing data, not as the authority.
6. Keep relapse probabilistic and *manageable*: maintenance/taper lowers risk; a stress spike is not
   a guaranteed punishment; a cold-turkey path exists but is one option among several, never the
   only one, and never the default.
7. **Protocol state (9M):** if none exists, add the smallest Core record —
   `DetoxProtocolState { survivorId, kind, stage, scheduledDoses, monitoringInterval,
   symptomThreshold, escalationRule, startedDay }` — driven inside the existing medical day tick, and
   **no second treatment scheduler**; the ward follows it, not the reverse.
8. Protocol lifecycle tests: start, advance, pause, fail, relapse, complete; escalation on threshold
   breach; persistence across save/load without restarting or double-dosing.
9. **Expedition coherence (9Q):** dependency state must keep ticking while a survivor is deployed;
   withdrawal does not pause because a survivor happens to be away; preparation with maintenance doses
   and taper supplies consumes through the normal path; no expedition-only dependency model.
10. **Trade demand (9P):** bind support-medicine demand to existing market/price state with bounded
    regional scarcity; explicit anti-incentive guard — no mechanic that profits the player from
    creating dependency, and no runaway price spiral for medicine the sick need.
11. **Backstories (9O):** 4–6 survivor-specific dependency origins (pre-Exchange prescription,
    battlefield analgesia, chronic pain, sedative, stimulant, post-trauma self-medication) surfaced
    only where relevant (survivor detail, codex, memorial text) — never as a personality modifier, and
    in language that does not stigmatise; validate chronology.
12. Balance simulation: dependency incidence, withdrawal severity, protocol success rate, supply
    demand over 120 days — before any new support item is authorised.
13. Tests: class coverage matrix, binding (stress reported → relapse risk changes → care reduces it),
    expedition continuity, protocol persistence, determinism, and the negative test that relapse is
    not guaranteed by any single stress event.

**Acceptance:** every class has a viable, documented care path; the two unbound hooks are bound; a
dependency is a logistics problem with real relief, not a morale debuff.

---

### Task 60E — Palliative care and remembrance: give vigil a surface and bind grief once

**Goal:** a terminal survivor moves from treatment to comfort care through one authoritative path, a
vigil is something the player can perform and see, and how someone died changes the people left
behind — through the systems that already own grief.

**Primary files:** `Medical/VigilStateMachine.cs`, `src/Host/MedicalHostSession.cs`,
`CaregivingSystem.cs`, `SickListSystem.cs` (`palliativePlan`), `Memorial/MemorialSystem.cs`
(`DeathQuality`, `MemorialOutcome`, `IGriefSink`, `ApplyGrief`), `SurvivorRelationsSystem.cs`,
`TraumaBondSystem.cs`, `final_wishes.json`, comfort/medical items, memorial/décor surfaces,
`ProceduralEulogyEngine`, epitaph/heirloom catalogs, `docs/medical/PALLIATIVE_CARE_CONTRACT.md`,
`VIGIL_RELATIONSHIP_MATRIX.md`, `MEMORIAL_OUTCOME_MATRIX.md`.

**Substeps**
1. **Create the vigil surface first (D6)** — `VigilStateMachine` is host-wired but has **zero**
   gameplay/UI callers today; extend the medical/sick-list/caregiving surface rather than adding a
   panel class. Until this step lands, no vignette may be authored (nowhere to render it).
2. Read and document the existing vigil states and transitions
   (`isActive`, `phantomKnockFired`, `wasSkipped`, `isCompleted`, the four events); define the
   contract in `PALLIATIVE_CARE_CONTRACT.md`; **no new terminal-state enum** if one is representable
   through these plus `SickBand.palliativePlan`.
3. Terminal entry: prognosis → `AssignPalliative(plan)` (the field exists, unused) → ward
   palliative category → vigil availability, all in one chain, one owner per fact.
4. Comfort care via existing items and caregiving actions only: analgesia, sedative comfort,
   anti-dyspnoea, nausea control, presence, familiar object — with **familiar-object comfort as
   relation/narrative state, not medicine**. No "peace points" consumable.
5. Wire final wishes (9U): give `final_wishes.json` a real consumer; fulfilled / impossible /
   ignored / partially fulfilled states feed vigil outcome through the existing projection — not a
   duplicate wish store, and no large mechanical buff.
6. **Bind the grief sink (9V):** the single highest-leverage line in this plan — connect
   `IGriefSink.ApplyGrief` (verified: 1 Core declaration, 3 test files, **0 host callers**) into the
   existing relation/morale authorities so death context reaches the living; bounded effects, and no
   second grief model.
7. Death-quality inputs already modelled: attended peaceful vigil vs unattended vs sudden vs rushed —
   map them to `DeathQuality` and the **existing** grief multipliers (0.5 / 1.0 / 1.25) rather than
   inventing new coefficients; verify no combination silently produces an unmodelled state.
8. Vigour test the anti-reward rule: a good death must **reduce or intensify grief burden**, never
   pay the player morale; add a test asserting the ceiling on any comfort-care-driven morale delta,
   and that no repeatable loop exists (attend → buff → repeat).
9. Author **6–8 vigil vignettes** keyed to real state only (relationship, consciousness, comfort,
   final-wish status, presence), each selection deterministic and repeat-safe, restrained in tone, no
   melodrama, no restating of facts the state does not hold; keyed text for localization.
10. Memorial outcomes (9W): confirm the `MemorialOutcome` enum members against the plan's three
    (burial / memorial wall / ash scattering) and **deepen existing ones** rather than duplicating;
    eligibility from location, resources, relationship, and final wish; persisted; no resource
    generation.
11. Memorial display (9X): wire outcomes into whatever remembrance surface exists; where none can
    carry them, publish the deferral **contract** (what an integration must expose) instead of
    building a décor framework inside medicine.
12. Remembrance continuity: memorial/vigil state must be readable by the eulogy/epilogue paths
    (existing `ProceduralEulogyEngine`, epitaph and heirloom catalogs) — those are the artifacts with
    no host consumer today, so this step also closes their gap rather than adding a third record.
13. Tests: full chain prognosis → palliative → vigil → death → grief → memorial; no double-fire on
    reload of an active vigil; vignette state-fidelity (no line renders a false fact); grief bounded
    and attributable; memorial persists once and is never duplicated; save/load of every terminal
    stage.

**Acceptance:** terminal care is one authoritative path with a surface the player can reach, wishes
matter, the living are changed through the systems that already own relationships, and death is not a
morale farm.

---

### Task 60F — Hardening: migration, reachability, determinism, long horizon, legibility

**Goal:** prove the whole medical layer is coherent, persistent, reachable, fair and readable —
and make it stay that way with gates, not good intentions.

**Files:** medical save stores, `SaveSectionRegistry`, `Ashfall.Core.Tests/*Disease*/*Medical*/*Dependency*/*Vigil*`,
the content-utilisation scan, `DiseaseHeadlessDemo`, `--disease-selftest`,
`--expansions-selftest`, `--save-load-ui-failure-selftest`, `--real-campaign-journey-selftest`,
`docs/medical/MEDICAL_SAVE_MIGRATION.md`, `MEDICAL_CONTINUITY_AUDIT.md`,
`MEDICAL_REGRESSION_MATRIX.md`, snapshot suite.

**Substeps**
1. Inventory every persisted medical field (disease, isolation, sick bands incl. `palliativePlan`,
   dependency state, protocol state, vigil state, memorial selection) into one table with its owner.
2. Author safe defaults so an old save with no protocol/vigil data loads unchanged, and record it in
   `MEDICAL_SAVE_MIGRATION.md`.
3. Round-trip matrix (≥12 cases): pre-existing save, active known disease, active new disease,
   incubating, mid-treatment, dependency, active taper, relapsed state, terminal prognosis, active
   vigil, completed memorial, and repeated save/load.
4. Reload idempotence assertions: no re-rolled phase, no duplicate dose, no restarted taper, no
   refired death event, no duplicate memorial, no double grief application.
5. Cross-system continuity audit (`MEDICAL_CONTINUITY_AUDIT.md`): one owner per fact — vector
   source, contamination, infection, severity, isolation, protocol, wish, grief, memorial — each row
   citing the authority's file:line.
6. Content-reachability scan for medical families: disease triggerable → treatment reachable →
   tell reachable → protocol reachable → vignette reachable → memorial outcome reachable; orphan
   content fails, with an explicitly allowlisted set of intentionally rare states.
7. Determinism harness: fingerprinted fixtures replaying exposure → incubation → phases → treatment →
   recovery/death, plus dependency accrual → withdrawal → protocol → relapse → stabilisation, and
   vigil transitions; identical state must produce identical traces, and no medical progression may
   read wall-clock, GUIDs, unordered iteration, or UI frame timing.
8. UI-preview safety: opening a medical surface must not mutate clinical state (a dedicated test,
    because previews that tick are how a diagnosis changes by looking at it).
9. Long-horizon simulation (120–180 days, several seeds, baseline vs expanded): incidence,
    severe-case share, deaths, pharma consumption, ward occupancy, dependency and palliative load —
    tune **incidence and exposure** before touching base health mechanics, and publish the curves.
10. Failure-mode checks from the runs: runaway epidemic, permanently saturated ward, impossible pharma
    demand, and disease trivialised by excess treatment each get a named assertion.
11. UI/accessibility regression across sick list, disease detail, diagnostic clues, dependency state,
    protocol, palliative and vigil surfaces: no colour-only critical status, text-scale-up overflow
    check, terminal state clear without sensationalism, symptom strings unclipped.
12. The 10-scenario regression matrix (water→diagnose→treat; excavation→spore→respiratory;
    wound→infection→ward; dependency→taper→stress relapse→stable; expedition withdrawal;
    prognosis→vigil→wish→memorial; attended vs unattended death grief; old save with active disease;
    save during taper; save during vigil) executed as one CI scenario set with the plan's report
    format, and each row asserting no duplicate authority.
13. Register the medical gates (reachability scan, determinism fingerprint, cross-version save
    matrix, long-horizon run) with owners and self-proofs; a gate that has never failed is a rumour.
14. Run the full medical gate set: `--disease-selftest`, medical/dependency selftests,
    `--data-integrity-selftest`, `--save-load-ui-failure-selftest`, `--expansions-selftest`,
    `dotnet test`, host build, `verify-fast.sh`.

**Acceptance:** the submitted plan's Definition of Done is checked by commands, not by a checklist of
intentions.

---

## 5. Order, and why

```
D1–D7 ADRs ─► 60A (legibility) ─► 60C (treatment roles + ward) ─► 60D (dependency wiring)
                     │                                            ─► 60E (palliative + grief)
                     └────────► 60B (outbreak causality; needs tells to be readable)
60F gates run continuously; 60F steps 1–4 land WITH each of 60A–60E, not after.
```

**Recommended sequence:** 60A → 60B → 60C → 60D → 60E → 60F, with three items jumping the queue
because they are single-line bindings, not designs: **`ApplyGrief` (60E step 6)**,
**`ReportStress` producers (60D step 3)**, and **`palliativePlan`'s writer (60E step 3)**. Those
three are the difference between a medical simulation that exists and one the player can meet.

**Why this order differs from the submitted one:** the plan sequenced authoring (catalog → tells →
outbreaks) because it believed content volume was the gap. Verified reality is the opposite: the
catalog is full, the text is authored, the interfaces exist — so **schema decisions, surfaces and
bindings come first**, and content work becomes the small remaining slice.

---

## 6. Guardrails carried forward, unchanged

- **No** second disease runtime, affliction model, detox engine, palliative state machine, ward
  triage system, grief model, or parallel pharma economy.
- **No** batch recipe expansion; reuse underused outputs first; no universal cure; no single recipe
  dominating every condition.
- **No** fantasy pathology: no zombies, rage viruses, supernatural infection or impossible spores.
- **No** moralising through mechanics; dependency care is management, not virtue; comfort care is
  not a morale farm and death is not "another failed health bar".
- **No** invented transmission vector unless the repository proves one already exists.
- **No** new symptom/diagnosis authority alongside the one being extended.
- **No** UI that recomputes clinical truth a Core system already owns.
- **No** medical event duplication on reload; **no** wall-clock, GUID, unordered-iteration, or
  platform-enumeration dependency in clinical progression.
- **Nothing** authored before the field that represents it exists; **nothing** claimed complete
  without the command that proves it.

---

## 7. Definition of done (integrated)

Re-baselined from the submitted list — the strikethroughs are items the repository already satisfies,
the additions are what reality turned out to require:

- ~~Disease catalog contains ≥15 diseases~~ **(already: 15)** · ~~all four vectors covered~~
  **(already: 5/4/3/3)** · ~~at least 2 diseases world-triggered~~ **(already: sump flooding +
  excavation via `IDiseaseOutbreakSource`)**
- ADRs D1–D7 written, with the derived-phase decision recorded.
- Every disease has a **rendered** tell; every authored `guidance`/`source_note` field reaches a
  surface or is removed.
- `treatment_role` + derived treatment windows exist, and the 25-recipe audit shows every output with
  a purpose (or an explicit deletion).
- Ward categories and isolation are exercised by catalog content, with a routing test per class.
- Sick list distinguishes dose band from disease severity, shows urgency and trend, and states
  prognosis without feigning certainty.
- `ReportStress` has real producers; `OnStressReported` has at least one subscriber; relapse rules are
  data with a deterministic pure evaluator; no class lacks a care path.
- Detox protocol state exists (smallest possible), persists, and survives reload without restart.
- Dependency remains coherent on expedition, with preparation consuming normally.
- Vigil has a reachable surface; `palliativePlan` has a writer; `ApplyGrief` is bound to the existing
  relation authorities; ≥6 vignettes exist and render only true state.
- ≥3 memorial outcomes exist and persist, with a deferral contract where no display surface exists.
- 4+ bounded outbreak templates with cooldowns; medical crises never avalanche; radio warnings respect
  what their source could know.
- Old saves load; every active medical state round-trips; no reload duplicates a dose, death, vigil,
  or memorial.
- Determinism fingerprints stable; long-horizon runs survive without runaway epidemic or permanently
  saturated ward; accessibility regression passes on all seven medical surfaces.
- No parallel medical subsystem was introduced, and **no medical gate remains unowned or
  unproven-failable.**

---

## 8. Final regression report format (carried from the submitted plan, with re-baseline fields)

```text
Plan 60 (Plan 09 integrated) — Final Regression

Re-baseline delta:
- diseases at plan authoring: 7        diseases now: <n>
- outbreak sources at plan authoring: 0  sources now: <list>
- previously unbound hooks: ApplyGrief, ReportStress, palliativePlan, guidance/source_note
  → now bound: <yes/partial/no per hook>

Build / tests / gates:
- dotnet build Ashfall.csproj / dotnet test / --data-integrity-selftest /
  --disease-selftest / --expansions-selftest / --save-load-ui-failure-selftest: PASS/FAIL

Disease:
- total / vector counts / diseases missing a rendered tell / invalid countermeasure refs
- outbreak templates / world-triggered diseases / simultaneous-outbreak violations

Legibility:
- surfaces showing phase, trend, urgency, isolation, prognosis
- codex unlock path / guidance + source_note consumers / contradictory-symptom issues

Ward & treatment:
- bed categories exercised / isolation routing / treatment-role distribution
- universal-cure or dominant-recipe issues / pharma outputs with no purpose

Dependency:
- classes / care paths per class / support items / protocol state / relapse tests
- expedition continuity tests / stress producers bound / consumer subscribers bound

Palliative & remembrance:
- vigil surface / states exercised / vignettes authored & reachable
- final-wish integration / grief binding / memorial outcomes / duplicate-memorial issues

Save & determinism:
- migration cases (12) / idempotence on reload / determinism fingerprints stable

Balance:
- incidence / severe share / deaths / pharma demand / ward occupancy / dependency load
- compared to pre-Plan-60 baseline, with tuning notes

Deferred (explicit):
- <list, each with an owner and a reason>
```

Plan 60 is **not** complete while any tell is unwritten or unrendered, any outbreak arises from
invented parallel state, dependency care is cold-turkey-only, grief has two owners, an authored field
has no reader, a gate has no owner, or old saves break.
