# ASHFALL — Medical Architecture Decisions (D1–D7)

**Purpose:** settle the seven forks the medical-depth plan (Plan 09, integrated as Plan 60) assumed
away. Every line below was read from source at `ccac926e`. Content authoring is blocked until the
decisions in §"Decision" are accepted, because catalog shape follows engine shape.

Companion docs: `Next-steps-plans/Plan_60_Medicine_Made_Legible_Plan09_Integrated.md`.

---

## The disease engine, as it actually is

| Aspect | Source of truth |
|---|---|
| Catalog fields | `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs` — `id, display_name, vector, incubation_days, illness_days, infectivity, lethality, spread_interval_days, spread_radius, countermeasure_item_id, guidance, source_note`; `DiseaseVector` = `Water/Air/Blood/Spore` (`:11–17`); `SchemaVersion = 1` |
| Per-infection state | `DiseaseInfectionState { survivor_id, infected_day, days_sick, quarantined }` (`DiseaseSystem.cs:42–47`) — **four fields, no severity, no phase, no treatment** |
| Per-disease state | `DiseaseEntryState { disease_id, vector_type, spread_timer, outbreak_active, deaths_during_outbreak, outbreaks_total, outbreaks_prevented, recovered_total, deaths_total, infections_total, infected[] }` |
| Progression | `TickDaily(day, candidates)` → spread attempt when `spread_timer >= SpreadInterval(def)` unless `IsVectorBlocked(entry.vector_type)`; then `ResolveOutcomes(entry, def, day)` |
| **Outcome rule** | `DiseaseSystem.cs` — `if (patient.days_sick < def.illness_days) continue;` then `bool died = def.lethality > 0f && _rng.NextDouble() < def.lethality;` → death or recovery, then removal |
| Countermeasures | `IsVectorBlocked` maps `water_purified / vents_sealed / tools_sterilized / air_filtration` (`:693–702`); verbs `PurifyWater()`, `SealVents()`, `SterilizeTools()`, `SetAirFiltration()` + `ResetWaterPurification()`; host wrappers in `src/Disease/DiseaseHostSession.cs:95–109` |
| Outbreak entry point | `IDiseaseOutbreakSource` (`:18`) consumed by `TriggerOutbreak` (`:316`); live implementations `src/Host/DiseaseOutbreakHostAdapter.cs:32` (`SumpFloodingSource`) and `:46` (`ExcavationSource`) |
| Reachability | `DiseaseHostSession` constructed at `src/Main.Medical.cs:365`; `grep` for `PurifyWater\|SealVents\|SterilizeTools\|SetAirFiltration` outside that session → **0 callers**; `ResetWaterPurification` → **0 callers** |
| Player surfaces | `src/UI/Disease/DiseasePanel.cs`-style surface: disease appears in `src/UI/AfflictionsPanel.cs`, `src/UI/MedicalPanel.cs`, `src/UI/FeedbackMessages.cs`; `MedicalPanel.cs:533` carries a comment about consuming the countermeasure "through the inventory" |
| Authored clinical text | `guidance` + `source_note` are read **only** by `DiseaseHeadlessDemo.cs:57–58` (which validates `countermeasure_item_id` existence) |

**The two facts that decide everything:**

1. **There is no intervention path.** Outcome is `days_sick ≥ illness_days` → single `lethality`
   roll. Nothing a doctor does changes it, because nothing reads treatment.
2. **The prevention path is unreachable and sticky.** The four vector-blocking verbs exist and are
   host-wrapped, but no UI or day owner calls them, and the only reset function is uncalled —
   so a protocol that *were* applied would never expire.

---

## D1 — Phase model: derived, never authored

**Decision:** do **not** add an authored `phases[]` timeline. Derive stage from existing fields —
`days_sick < incubation_days` → *exposed/incubating*; `incubation_days ≤ days_sick < illness_days`
→ *ill*; `days_sick ≥ illness_days` → *outcome pending*. Implement as **one** pure Core function
(e.g. `DiseaseProgression.StageOf(def, daysSick)`) consumed by every surface.

**Why:** `ResolveOutcomes` already treats the catalog this way (`illness_days` is the only
progression bound). An authored phase list would be a second timeline that the engine does not drive
— exactly the "two authorities for one fact" defect this project keeps finding.

**Consequences:** a new pure function + unit tests at the day boundaries (0, `incubation_days-1`,
`incubation_days`, `illness_days-1`, `illness_days`); `DiseaseInfectionState` needs **no** new
fields; every UI reads the function; the derived stage becomes the "3-phase" expression the
medical plan asked for, without new state to save or migrate.

---

## D2 — Diagnostic tells: render what exists, add the missing key only

**Decision:** tells are a **presentation** addition: keep `guidance` (player-facing protocol text)
and `source_note` (provenance) as they are, and add **one** field set —
`tell_key`, `tell_secondary_key`, `timing_clue_key` (localization keys, not prose) — rendered from
the derived stage. No symptom authority, no symptom simulation.

**Why:** the authored clinical text is already in the catalog with **zero runtime consumers**. A
second symptom model would compete with `AfflictionContracts`/`AfflictionsPanel`, which already
exist.

**Consequences:** catalog `SchemaVersion` 1 → 2 with a load-time default (missing tells = "no
specific tell"), so old catalogs load unchanged; a data test asserts no disease lacks a tell and no
tell is shared by every disease (else diagnosis becomes trivial); symptom overlap is allowed and
expected; diagnosis remains a gated act through `SickListSystem.Diagnose`, never a free render of
hidden state.

---

## D3 — Treatment roles require a hook, not a field

**Decision:** add a **narrow, single** intervention seam to the disease engine, then attach roles to
it. Minimal shape:

```csharp
// Core, on DiseaseSystem only
public bool TryTreat(string survivorId, string diseaseId, string itemId, int day, TreatmentKind kind)
public sealed class TreatmentRecord { survivorId, diseaseId, itemId, day, kind, dosesApplied }
```

with `TreatmentKind { Curative, Suppressive, Symptomatic, Supportive }`, and outcome resolution
changed from a bare `lethality` roll to `lethality` adjusted by authored per-role effect +
treated-early bonus, **bounded** and deterministic via the existing `_rng`.

**Why:** "every new disease has a treatment path" cannot be true while `ResolveOutcomes` ignores
treatment. Roles without a hook are documentation, not mechanics.

**Consequences:** persisted `TreatmentRecord` list per disease entry → **save-schema bump** with a
migration defaulting to "no treatment history"; `countermeasure_item_id` keeps its *prevention*
meaning (it is documented as vector neutralisation) and must **not** be silently redefined as a cure;
treatment consumes through the existing single item-consumption path so the dose does what the item
says; windows matter because the effect is a function of `daysSick` at treatment time; a test
asserts no single item is `Curative` for more than a bounded share of the catalog (no universal
cure).

---

## D4 — Windows are derived, expiry is authored

**Decision:** treatment window = derived from catalog bounds (`incubation_days`, `illness_days`, an
optional `treatable_days` defaulting to `illness_days`). Protocol/countermeasure state gets
**authored duration and decay**, applied in the existing day tick.

**Why:** a window that lives only in UI text is decorative, and vector-blocking state that never
expires ("purify once, safe forever") is an exploit and a dead end for design.

**Consequences:** `ResetWaterPurification()`'s twin for vents/tools/filtration plus expiry timers set
in the catalog or a small `disease_protocol.json`; the day owner advances decay; a test asserts every
`IsVectorBlocked` flag can return to false within a bounded number of days; player-facing text says
"until" not "always".

---

## D5 — Sick list: one band ladder, two named sources

**Decision:** keep the existing band ladder as the single urgency scale, but state its source
explicitly: `SickBand.band` is currently a **dose** band
(`DoseLedgerSystem.BandGreen..Black`), while disease severity is a different fact. Either map disease
severity into the same ladder with a documented rule, or add a named `severitySource` — **never**
let one integer silently mean two things.

**Why:** `SickBand { survivorId, band, diagnosedDay, releaseDay, palliativePlan }` is the surface the
player triages against. If the band silently means "radiation" while the panel implies "illness",
ward routing and quarantine decisions become guesswork for the player and for tests.

**Consequences:** an explicit `Diagnose(survivorId, band, day)` caller from disease progression
(today the disease engine and the sick list are separate authorities with no documented bridge),
plus `AssignPalliative(plan)` gaining a writer (the field exists, unused), and a contract test that
the same band value means the same thing in ward admission, sick list, and the epilogue's dose
accounting.

---

## D6 — Vigil needs a surface and a day clock, not more states

**Decision:** (a) expose vigil through the **existing** medical/sick-list/caregiving surfaces — no new
panel class unless a measured failure shows they cannot carry it; (b) convert vigil progression from
real-time to campaign time.

**Why:** `VigilStateMachine.StartVigil(dwellerId, names, duration)` is called only from
`src/Host/HostCli.PanelTests.cs:1633,1647`; `src/Host/MedicalHostSession.cs:62–65` subscribes the four
events and `:211–219` reports/skips — but **nothing in gameplay starts a vigil**, and
`Tick(float deltaSeconds)` advances on **frame delta**. In a manual-advance, save/load-by-day game,
a real-time clock is both unreachable and a determinism hazard (and `VigilStateMachine` has no
`TickDay`-style entry).

**Consequences:** one `StartVigil` call site driven by terminal prognosis (via D5's bridge), a
day-based progression path with the existing `VigilSaveState` round-trip, a "no active vigil"
honest empty state, and only *then* vignette authoring — vignettes with no surface are how content
becomes the next orphan catalog.

---

## D7 — Grief has one authority and is currently doubly unbound

**Decision:** bind the existing chain end to end; do not create a grief model or a grief stat.

Verified state:

| Link | Reality |
|---|---|
| `IGriefSink.ApplyDispersion(deceasedId, survivingRelationshipIds, qualityScale, …)` (`Memorial/MemorialSystem.cs:52`) | invoked internally at `:190` via `GriefSink?.…` |
| `MemorialSystem.GriefSink { get; set; }` (`:143`) | **never assigned in `src/`** → the `?.` silently no-ops in play (`CapturingGriefSink` at `:65` is a fidelity/test helper) |
| `SurvivorRelationsSystem.ApplyGrief(survivorId, amount)` (`:98`) | **no host caller**; a test file even documents it — `Ashfall.Core.Tests/Memorial/MemorialGriefPortTests.cs` — *"never called from the"* game |
| Death quality multipliers | authored in code: `Peaceful 0.5 / Rushed 1.0 / Unattended 1.25` (`:46–50`) |

**Consequences:** one adapter implementation injected at medical/memorial setup time; the
`DeathQuality`→multiplier table moves to data with the code values as fallback; a test asserts a
death with N related survivors changes exactly those N relationships once and only once across
save/reload (no double application); and a ceiling test enforcing the plan's own rule that a peaceful
death *reduces grief burden* rather than paying morale.

---

## Order of work implied by these decisions

| Priority | Item | Why first |
|---|---|---|
| P0 | D5 bridge (disease → sick list) + `palliativePlan` writer | Without it, nothing downstream (ward, vigil, palliative) has a subject |
| P0 | D7 inject `GriefSink` and bind `ApplyGrief` | Two lines of wiring; the entire "death affects the living" claim depends on it |
| P1 | D3 intervention hook + D2 tell keys (schema 1→2 with defaults) | The only genuine engine additions in the whole scope; unblocks "treatment path" for real |
| P1 | D6 vigil surface + day clock | No vignette, comfort item, or wish link can be verified before a player can start a vigil |
| P2 | D4 protocol expiry + reachability of the four verbs | Fixes the sticky-prevention exploit and makes counterplay exist in the UI, not just in Core |
| P2 | 60B/60C content, outbreak budgets, codex, balance runs | Authoring once the schema and surfaces can hold it |

**Rejected:** authored disease phases (parallel timeline); a symptom simulation authority; a second
palliative state machine; a new vigil panel; redefining `countermeasure_item_id` as a cure; a
morale-for-good-death reward path; any pharma batch expansion before D3 lands.

**Standing rule for this layer:** a medical fact has exactly one owner, one surface that renders it,
and one test that proves a player can act on it. Anything that fails all three is documentation,
not a system.

---

## Implementation status

| Decision | State | Landed as |
|---|---|---|
| **D1** derived stage | ✅ implemented | `Assets/Ashfall.Core/Disease/DiseaseTriage.cs` — `StageOf` / `IsTerminalPrognosis`, no authored phase list, no new save fields |
| **D2** tells as presentation | ✅ implemented | catalog `tell` / `tell_secondary` / `timing_clue` authored for all 15 illnesses (schema 2, additive); `DiseaseTriage.PictureOf` is the single clinical projection; the ward inspector renders a CLINICAL NOTE (signs, timing, guidance, survival chance, prognosis) and `--disease-selftest` gates tell coverage and uniqueness |
| **D3** intervention hook | ✅ implemented | `DiseaseCatalog.DiseaseTreatment` + `treatments[]` (catalog `SchemaVersion` 1→2, additive), `DiseaseSystem.TryTreat` / `GetEffectiveLethality` / `OnTreatmentApplied`, patient-scoped `lethality_reduction` capped at `MaxLethalityReduction`, one dose per patient per day; `DiseaseHostSession.Treat` + `BindSupply` bound to the inventory authority; ward inspector offers GIVE … [role] per bed |
| **D4** windows + protocol expiry | 🟨 partial | treatment windows are enforced by authored `max_days` (7 of 15 illnesses curable only early); **protocol expiry is still open** — `ResetWaterPurification` and siblings have no day-tick caller |
| **D5** one band ladder, named source | ✅ implemented | `SickBand.severitySource` + `sourceId` (additive; pre-D5 rows restore as `dose`), `SickListSystem.Diagnose(…, source, sourceId)` overload, and the daily reconcile in `src/Main.MedicalTriage.cs` driven from the disease snapshot after the `medical_disease` tick |
| **D6** vigil surface + clock | ✅ implemented (revised) | **the real-time vigil was kept deliberately** — spending minutes at a bedside is the design, so converting it to a day tick would have destroyed it. What was missing was reachability and a rule: `MedicalHostSession.HoldVigil` + `TickVigil` driven from `Main._Process`, a **KEEP VIGIL** action on the bed, completion recorded on the consequence ledger (`VigilCare`, flag `flag_vigil_kept_*`) and read back by `SurvivorFateSystem.DeriveDeathQuality`. A test pins that tick granularity cannot change the outcome |
| **D7** bind grief, one authority | ✅ implemented | `Assets/Ashfall.Core/Memorial/RelationsGriefSink.cs` injected in `EnsureMemorialGriefSink()`; `SurvivorRelationsSystem.RelatedIds` + `SurvivorSocialCoordinator.Relations`; `SurvivorFateSystem` now passes `MoraleDelta`, `DeathQuality` (derived from caregiving/ward + resolved wish) and the surviving relation ids |

Verified with: `dotnet test` **5499 passed** (medical/disease/memorial slice 290 green; the 4 remaining failures are another agent's in-flight `Plan 21` phantom-memory/gossip files and their `file:///` doc links — none medical), `dotnet build Ashfall.csproj` 0 errors / 0 warnings, and `--disease-selftest` 65/65 (was 36), `--medical-selftest` 15/15, `--expansions-selftest` 10/10, `--day1-selftest`, `--real-campaign-journey-selftest`, `--7-day-smoke-selftest` 10/10, `--data-integrity-selftest` 142 catalogs / 0 errors, `triad-drift-gate` PASS.

**Verification of the host bridge:** `--dose-uitest` now asserts, in a real host session, that an
infection produces an illness-sourced sick-list row at a raised band, that a dose spends exactly
one item from the inventory authority, that care improves *that patient's* odds only, and that a
late presentation is refused with `outside_window`. Core side is pinned by
`Ashfall.Core.Tests/Medical/DiseaseTriageBridgeTests.cs` + `DiseaseTreatmentTests.cs`, and the Godot
`--disease-selftest` grew from 36 to 61 checks for the same contract.

**Still open:** **D4**'s protocol expiry half — `ResetWaterPurification` and its three siblings
still have no day-tick caller, so a vector protocol, once applied, would never lapse; and the sick
list still shows bands without the D2 clinical note (only the ward renders it), so a diagnosis
surface for lay survivors is the next slice. Palliative authoring (vignettes, comfort items,
memorial outcomes) is **now unblocked**: the vigil has a surface and the clinical text has a home.
