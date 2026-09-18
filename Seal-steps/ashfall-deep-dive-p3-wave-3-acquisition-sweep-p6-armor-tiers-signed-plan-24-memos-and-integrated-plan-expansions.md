# ASHFALL — Deep-Dive Expansion Document

**Document class:** Companion implementation plan to `ashfall-unblocked-integration-plans`. Where that document scoped six unblocked packages, this one deepens four of them at the request of the foreman (2026-09-18):

- **Part A** — P3 Wave 3 fully enumerated: every acquisition route in the repo, classified by current silence, before any authoring.
- **Part B** — P6 deepened: armor plate grade tiers designed against the live metal scarcity and commodity data.
- **Part C** — Plan 24 decision memos drafted in signed form: A1-floor (ward staffing) and B1 (affliction recovery ramp), annotated.
- **Part D** — Expansion packages for plans that are already integrated but have identified headroom: Plan 174 (companion animals) and Plan 212 (dynamic economy).

**Evidence basis (new pulls for this document):** `Assets/StreamingAssets/Data/commodity_baselines.json` (12 categories, scarcity floors/ceilings in permille), `Assets/StreamingAssets/Data/scavenging_tables.json` (finite depletion model, hazard-tagged entries, rarity tiers common/uncommon/rare), plus the integration ledger corpus cited in the parent document. Where a number in Part B derives from these files it is marked **[grounded]**; where it is a design proposal awaiting authored confirmation it is marked **[proposed]**.

**Standing invariants (inherited, non-negotiable):** fresh claims in `WORKTREE_OWNERSHIP.md`; build 0/0; data-integrity selftest PASS with new catalogs walked clean; no new RNG streams; frozen-shape save migrations only; exactly-once player-visible consequences; a11y gate (words never color-only); docs index regenerated; closeout + ledger row per package.

---

# PART A — P3 Wave 3: Acquisition Route Enumeration and Silence Classification

## A.1 Method

The 17C Phase E closure condition was "acquisition-flavored events ... not consistently classified or surfaced; some routes are silent today." Before authoring, this part enumerates the routes from repo evidence (systems, catalogs, and event vocabulary landed through Wave 8 Part 2) and classifies each on three axes:

- **Emission:** `EMITS` (an event id in `DayEventVocabulary` fires on acquisition), `PARTIAL` (fires on some paths but not all — e.g. only the aggregate, only one caller, or only the failure twin), `SILENT` (nothing reaches the briefing/journal/audio surfaces).
- **Surfaces:** which of B (daily briefing), J (journal), P (panel strip), A (audio) currently show it.
- **Dedup risk:** whether two systems meet at this route and could double-emit (the closure report's explicit warning).

**Classification legend for the fix column:** `KEEP` (already correct — becomes a pinned matrix row), `EMIT-ADD` (add one vocabulary event where silent), `EMIT-COMPLETE` (complete the partial emission paths), `DEDUPE-RULE` (needs an authored precedence rule, not more events).

## A.2 The acquisition route census

### A.2.1 Scavenging and world looting

| # | Route | Authority / evidence | Emission | Surfaces | Fix |
|---|---|---|---|---|---|
| 1 | **Shelter-local scavenging** (finite loot tables) | `scavenging_tables.json` — finite depletion model, per-entry hazard tags, rarity tiers | `PARTIAL` — hazard exposure emits; the *gain* itself surfaces only as inventory delta | — | `EMIT-COMPLETE` |
| 2 | **Loot-site single resolution** (anomaly-linked sites) | `AnomalyHazardSystem` one-shot loot-site resolution (Plan 176) | `PARTIAL` — resolution event exists; acquired-item summary does not | J | `EMIT-COMPLETE` |
| 3 | **Expedition salvage** (destination loot) | `ExpeditionHostSession` + destinations catalog (Plan 32 recon + expansions) | `EMITS` on return summary | B, J | `KEEP` |
| 4 | **Rescue-mission salvage branch** | `DistressRescueMissionManager` — salvage granted exactly once, rep 0 | `SILENT` on the salvage *contents*; mission outcome emits | J (outcome only) | `EMIT-ADD` |
| 5 | **Companion fetch** (working animals retrieving) | `CompanionAnimalSystem` (Plan 174) | `SILENT` | — | `EMIT-ADD` |

### A.2.2 Production and crafting

| # | Route | Authority / evidence | Emission | Surfaces | Fix |
|---|---|---|---|---|---|
| 6 | **Foundry forging COMPLETE** | `SilentFoundrySystem` deterministic pass; FORGING PASS strip (player-operable) | `PARTIAL` — panel strip shows the pass live; the daily briefing does not summarize completed forgings | P | `EMIT-COMPLETE` |
| 7 | **CVD diamond synthesis batch** | `CvdDiamondSynthesisEngine` (determinism fixed in the rescue batch) | `EMITS` (batch event) | B, J | `KEEP` |
| 8 | **Kitchen meal production** (`ServeMeal` / Serve All) | Kitchen & crew table (Plan 22B); `OnMealServed` day event | `EMITS` | B, J | `KEEP` |
| 9 | **Greenhouse / harvest yield** | Foundry greenhouse production (Plan 22 lineage + expansions) | `PARTIAL` — yield tick emits aggregate; harvest quality variance does not | B | `EMIT-COMPLETE` |
| 10 | **Aeroponics nutrient production** | `aeroponics_nutrient_catalog.json` | `SILENT` | — | `EMIT-ADD` |
| 11 | **Agriculture plots** | `agriculture_items.json` | `PARTIAL` — planting emits; harvest summary aggregates only | B | `EMIT-COMPLETE` |
| 12 | **General crafting recipes** | Crafting recipe expansion corpus (Batch waves) | `PARTIAL` — depends on station; some stations silent | varies | `EMIT-COMPLETE` (station-by-station sweep) |
| 13 | **Sanitation byproduct (compost fertilizer)** | `SanitationSystem` — compost fertilizer id (Plan 210 D7) | `SILENT` | — | `EMIT-ADD` |
| 14 | **Water collection / purification** | Water source management (Plan 189 lineage) | `PARTIAL` — contamination events emit; clean-water gain is inventory-silent | — | `EMIT-COMPLETE` |

### A.2.3 Trade and economy

| # | Route | Authority / evidence | Emission | Surfaces | Fix |
|---|---|---|---|---|---|
| 15 | **Merchant Buy** | `MarketSystem` canonical trade + trade-pressure recording (v2) | `PARTIAL` — price/pressure factors exist in `ExplainPrice`; acquisition line in briefing does not | — | `EMIT-COMPLETE` |
| 16 | **Merchant Sell / Barter** | same | `PARTIAL` (losses, not acquisitions — classify as the inverse flavor; out of Phase E scope, noted for the matrix) | — | matrix row only |
| 17 | **Black-market Buy** (signed settlement) | `WAVE8-PART2-C1-BLACK-MARKET-ACTIONS` — stateless settlement service, typed results | `PARTIAL` — typed results drive panel feedback; day-event emission depends on heat threshold only | P | `EMIT-COMPLETE` + `DEDUPE-RULE` with #15 |
| 18 | **Holdfast terminal trade** | Holdfast delegation (Plan 22A lineage) | `PARTIAL` | P | `EMIT-COMPLETE` |
| 19 | **Merchant restock day** (world-side stock gain) | Plan 147 day-gated cadence; priority pending P5 | `SILENT` (intentional? — restock is world-side; recommendation: one aggregate line, see A.4) | — | `EMIT-ADD` (aggregate) |
| 20 | **Ration distribution / received goods** | Ration journey (Plan 24) | `EMITS` (grievance/negative paths stronger than positive) | B, J | `KEEP` + balance note |
| 21 | **Survivor barter / informal economy** | Plan 213 lineage | `SILENT` | — | `EMIT-ADD` |

### A.2.4 Gifts, inheritance, and world events

| # | Route | Authority / evidence | Emission | Surfaces | Fix |
|---|---|---|---|---|---|
| 22 | **Survivor death legacy / inheritance** | Plan 206 lineage | `EMITS` | B, J | `KEEP` |
| 23 | **Faction gifts / tribute** | Faction standing systems (Verdict/Foundry/Crossing corpora) | `PARTIAL` — standing events emit; item payload silent | J | `EMIT-COMPLETE` |
| 24 | **Wildlife kill / hunting yield** | `WildlifeEcosystemSystem` + trapping catalog (Plan 136 lineage) | `PARTIAL` — encounter events emit; meat/hide gain is inventory-silent | — | `EMIT-COMPLETE` |
| 25 | **Trapping pipeline yield** | same catalog | `SILENT` | — | `EMIT-ADD` (merged into #24's family per A.4 rule 3) |
| 26 | **Radio-derived acquisitions** (coordinates from answered signals) | Rescue-signal runtime; follow-up payloads pending P1 | `SILENT` (by design until P1 lands — ordering dependency) | — | defer to P1 |

## A.3 Classification summary (the pre-authoring truth)

- **`KEEP` (correct today):** 5 routes (#3, 7, 8, 20, 22).
- **`EMIT-ADD` (fully silent, need one event each):** 6 routes after merging trapping into the hunting family (#4, 5, 10, 13, 19, 21).
- **`EMIT-COMPLETE` (partial emission):** 11 routes — the dominant failure mode is *aggregate-only or failure-only* emission: the game tells you what went wrong more faithfully than what you gained. This is a legibility asymmetry, and it becomes Phase E's design thesis (A.4).
- **`DEDUPE-RULE`:** 2 seams (#15/#17 merchant vs black-market; #2/#4 loot-site vs rescue-salvage).

## A.4 Phase E design thesis and authoring rules

1. **The acquisition digest, not acquisition spam.** Phase E must not add 20 new alert-class events (that would collide with the P3 Wave 2 alert-concurrency cap). Design: one `Progression`/`Acquisition` sub-flavored vocabulary family, and the daily briefing gains a single **acquisitions digest line** summarizing the day's gains by source class: e.g. "Gains: scavenged 6 items, forged 2, harvested 4 (greenhouse), bought 3 (market)." Individual routes keep their existing specific events; only *silent* routes get a new id, and it is digest-class, not alert-class.
2. **Exactly-once per acquisition.** Every `EMIT-ADD`/`EMIT-COMPLETE` event fires once per underlying acquisition transaction, keyed where possible by the transaction's existing identity (e.g. salvage grant id, forging batch id, settlement result). Tests pin idempotency per route.
3. **Dedupe precedence rules (authored, in the matrix doc):**
   - Black-market Buy (#17) suppresses the generic merchant-acquisition event (#15) when both would fire — the typed settlement result is the richer event and wins.
   - Rescue salvage (#4) suppresses the generic loot-site event (#2) — the mission context is the fuller story.
   - Companion fetch (#5) emits under the scavenging class with the companion named (it is flavor of #1, not a new family).
   - Trapping yield (#25) emits under the hunting family (#24) with source "trap" — one family, two sources.
4. **Restock (#19) stays aggregate and world-toned:** "the merchant's shelves were restocked overnight" — no itemization (world-side state, per the C1 invariant that UI open never advances stock; the event is day-owner-timed only).
5. **Radio-derived acquisitions (#26) are explicitly deferred to P1** — ordering dependency recorded in both packages' closeouts so the Phase E matrix ships with a documented hole rather than a premature seam.
6. **Balance note on #20:** ration distribution's negative paths (grievances) outshine positive receipt. Phase E adds receipt-class digest coverage, which incidentally rebalances briefing tone — flagged in the balance findings section of the closeout for foreman review, no data edited.

## A.5 Phase E verification (extends P3 Wave 3)

- New test suite `Campaign/AcquisitionSweepMatrixTests.cs`: generated from the census table above — every route has an asserted emission class, every `DEDUPE-RULE` seam has a suppression test, every `EMIT-ADD` has an exactly-once test with save/restore mid-day parity.
- Digest line golden-day pins (wording frozen; kind-classified by P3 Wave 1).
- `docs/campaign/ACQUISITION_EVENT_SWEEP.md` ships the census table with before/after columns and the dedupe rules; the parity matrix and source gates regenerate.
- Full verification protocol per the parent document §7.2.

---

# PART B — P6 Deepened: Armor Plate Grade Tiers vs. Metal Scarcity

## B.1 Scarcity grounding

From `commodity_baselines.json` **[grounded]**: the `materials` category is **low-elasticity** with a wide proportional scarcity band — base multiplier 950‰, floor 600‰, ceiling 1600‰. Low elasticity means trade pressure moves materials prices slowly (0.4× response class); the 600‰ floor means materials become cheap when the world is glutted, and 1600‰ expensive in shortage. Design consequence: **metal is the stable-but-squeezeable resource** — perfect for a consumable armor economy whose cost curve is dominated by *quantity* per grade rather than exotic ingredients.

From `scavenging_tables.json` **[grounded]**: `scrap_metal` is a `common` tier entry (weight 20, qty 1–3 on the hospital table — representative of urban tables generally), and rarity tiers run common / uncommon / rare. Higher-grade metals must therefore live at `uncommon`/`rare` weights on industrial-biased tables (the excavation-sites and foundry-adjacent catalogs own those tables).

**Design conclusion:** four grades, staircasing *quantity of scrap* and *rarity of alloying input*, never inventing new material ids. All inputs are existing items; recipes are data in the metallurgy catalog.

## B.2 The four grades

| Grade | Recipe (existing items only) **[proposed]** | Input scarcity class | Forging difficulty (pass stages) | Mitigation (vehicle-flagged risk, bp) **[proposed]** | Wear absorption | Integrity pool |
|---|---|---|---|---|---|---|
| **G1 — Scrap Plate** | 4× `scrap_metal` | common only | standard pass, 1 heat cycle | −1000 | 20% of trip wear to plate | 100 |
| **G2 — Sheet Plate** | 6× `scrap_metal` + 1 uncommon metal input | common + uncommon | 2 heat cycles, shape tolerance tightens | −1500 | 25% | 140 |
| **G3 — Composite Plate** | 8× `scrap_metal` + 2 uncommon + 1 rare input | mixed | 3 cycles + inspect gate strictness +1 | −2000 | 30% | 180 |
| **G4 — Alloyed Heavy Plate** | 12× `scrap_metal` + 3 uncommon + 2 rare | rare-weighted | full pass, inspect gate +2, heat window narrow | −2500 | 35% | 240 |

**Derivation of the mitigation ladder [proposed]:** top grade sits at −2500bp per the parent document's cap; grades staircase at 500bp intervals so each upgrade is a *felt* but not transformative improvement (a 25% increment per tier on the vehicle-flagged risk slice). G1 at −1000bp is deliberately meaningful: even scrap armor changes expedition composition, making the foundry relevant to vehicle play from the first recipe unlocked.

**Derivation of wear absorption [proposed]:** the fraction of trip-distance wear redirected from hull to plate. Capped at 35% at G4 so hull wear — the immobilization authority — always retains the majority share (65%+), preserving Plan 50's signed wear semantics. Plates are sacrificial and re-forgable; hull is not.

## B.3 Economy interlock (why these numbers)

- **Materials elasticity interlock:** because `materials` is low-elasticity **[grounded]**, heavy plate demand (12 scrap per G4) cannot be price-spiked out of reach by a single trading day — the ±3500bp pressure cap and 0.4× response class protect the armor loop from market whiplash. Conversely, sustained foundry demand *slowly* raises materials indices, making the scarcity ceiling (1600‰ **[grounded]**) the long-run brake on armor proliferation. This is the intended interlock: armor is a strategic materials sink.
- **Scavenging pressure interlock:** scrap is common-tier and quantity-weighted (qty 1–3 **[grounded]**); a G4 costs 12 scrap ≈ 4–12 scavenging trips' yield from urban tables. Armor thereby *routes* through the exploration loop rather than short-circuiting it.
- **Foundry time interlock:** higher grades consume more heat cycles of the deterministic pass — the same daily forging-pass capacity that CVD diamonds and metallurgy reconciliation compete for. Armor, diamonds, and tooling become alternative uses of one capacity: a genuine production decision, not a stat purchase.
- **Re-forge loop:** repairing a plate costs 50% **[proposed]** of its scrap input (no rare re-input — rare inputs are *shaping* costs, consumed at first forge only). This makes rare metals the license and scrap the running cost: elegant scarcity separation.

## B.4 Fitting rules (bounded, typed)

- Max plates per vehicle: 3 **[proposed]** (authored per vehicle class in the vehicle catalog; heavy vehicles may authorize 4).
- Total mitigation is **capped at the top-grade bound** (−2500bp) regardless of count and mix: fitting 3× G1 does not exceed one G4's mitigation; multiple plates instead add integrity pools (longer sacrificial runway) and widen wear absorption up to the per-grade fraction. Validator + test enforce the cap (anti-stacking rule from the parent document).
- Plate-to-slot assignment is fixed at fit time; a destroyed plate leaves an empty slot until re-forged and re-fitted (no hot-swap mid-expedition — dispatch-time state only).
- Immobilization: plates never immobilize; hull wear retains that authority byte-identically (parent document constraint, restated as a test).

## B.5 Legibility (UI contract)

- `VehicleGaragePanel` mod rows: "armor plate (composite): 62% — worn" (words + number, never color-only).
- Expedition profile explain rows gain a typed `armor` factor line (the same explain-row pattern as `ExplainPrice` category/shock rows): "vehicle risk reduced 2000bp — composite plate (integrity 140/180)". The player sees *why* the risk changed.
- Foundry panel: plate recipes listed data-driven with input costs; the FORGING PASS strip already narrates the multi-cycle passes live.

## B.6 Test matrix addendum (extends P6 Wave 1/2)

- Grade ladder table test: all four grades forge, fit, absorb, and re-forge per the table above.
- Anti-stacking: 3× G1 mitigation ≤ single G4 mitigation cap.
- Scarcity interlock simulation: 60-day run with aggressive armor production → materials index rises but stays within authored bounds **[grounded floors/ceilings]**; food-class availability unaffected.
- Capacity competition: forging a G4 on a full-capacity day is refused exactly once with a textual reason (same preflight pattern as FeedAllCrew).
- Wear-order pin: per-trip wear hits plates before hull within the redirected fraction; fresh-plate refit before a trip does not dodge that trip's wear (the cycling exploit guard from the parent document).
- Save/restore mid-wear fingerprint parity across all grades.

---

# PART C — Plan 24 Signed Decision Memos (Annotated)

## C.1 Memo D-A: Ward Staffing — SIGNED as A1-floor

```
DECISION MEMO — D-A: Ward Staffing
Package: PLAN-24-RESIDUALS-STAFFING-RECOVERY-RAMP (Wave 1)
Decision: Option A1 with authored floor (A1-floor)
Signed: Roberts Taube (foreman), 2026-09-18
Status: SIGNED — implementation unblocked
```

**Decision text.** Medical wards contribute **full treatment throughput only when the duty roster has ≥1 eligible medical specialist assigned during duty hours**. Unstaffed wards run at an **authored throughput multiplier with a hard floor of 0.4** (40%). The floor value is catalog data, never code.

**Annotations (why each clause exists):**

1. *Hard gate, not soft bonus.* Plan 24's thesis is duty-hour labor as a real economy — if staffing only nudges numbers, the roster decision is decoration. A1 makes "who works the ward tonight" a consequential choice: the same seam that already gates work eligibility, overwork, and skill-to-yield elsewhere in the labor cluster.
2. *The 0.4 floor.* The rejected failure mode is the specialist-poor death spiral: a shelter that lost its only medic would otherwise face 0% ward throughput, converting one bad event into an unrecoverable campaign. The floor keeps unstaffed wards functional-but-poor, which is a *pressure* (find, train, or trade for a medic) rather than a *sentence*. 0.4 specifically: low enough that staffing is clearly worth it (a 2.5× swing), high enough that a 30-day no-specialist simulation stays survivable — that simulation is a mandatory Wave 1 test, so the number is falsifiable.
3. *Eligibility by skill tags, not a new role system.* `staff_skill_tags` (default `["medical"]`) reuses the existing certification-tier seam that already gates work eligibility. No new skill system, no new save fields — assignment state already persists in the roster.
4. *Duty-hour scoped.* Throughput applies only during assigned duty hours — the same boundary the Plan 24 labor tests already pin. A night-shift medic does not accelerate a day-shift ward; this keeps the overwork and duty-hour arithmetic coherent.
5. *Authored per facility.* `staffed_throughput_multiplier` lives on the ward facility row. Different shelter tiers can express different staffing cultures (a field hospital may be more staff-dependent than a bunker ward) without code changes.

**Implementation pointers (from the parent document P2.2, unchanged):** catalog DTO fields `staffed_throughput_multiplier` (default 1.0, floor 0.4 enforced by validator) + `staff_skill_tags`; throughput routed through the existing duty-hour labor cluster query; ward becomes an assignable station in the existing assignment UI; ~10-case test suite including the no-specialist 30-day simulation; zero hardcoded multipliers (source-gated).

**Explicitly rejected:** A2 (soft ±15% bonus) — recorded so future audits know the alternative was considered and why it failed the meaningful-decision test.

## C.2 Memo D-B: Affliction-Specific Recovery Ramp — SIGNED as B1

```
DECISION MEMO — D-B: Affliction Recovery Model
Package: PLAN-24-RESIDUALS-STAFFING-RECOVERY-RAMP (Wave 2)
Decision: Option B1 (per-affliction ramp curve, integer bp)
Signed: Roberts Taube (foreman), 2026-09-18
Status: SIGNED — implementation unblocked
```

**Decision text.** Each affliction row authors a **2–3 point recovery ramp** mapping normalized severity → recovery-speed multiplier (e.g. `[(0.0, 1600), (0.5, 1000), (1.0, 400)]` in bp: fast early recovery, slow tail). Recovery arithmetic stays **deterministic and integer-scaled** (bp, invariant culture, no floats in persistence). Rows without a ramp keep the legacy flat path **byte-identical**.

**Annotations (why each clause exists):**

1. *Why a ramp at all.* B2 (flat per-affliction constant) wastes the affliction catalog's granularity. B1 makes **when you treat** a real decision: a fever caught on day one clears fast (1600‰ of baseline speed); the same fever nursed at half severity crawls (400‰). Early medicine is cheap medicine — the pharmacist's economy and the triage decision become the same system.
2. *Why monotone-decreasing by default.* Fast-early/slow-tail is the intended clinical texture. The `chronic: true` flag is the authored escape hatch for afflictions designed to linger (late-stage radiation sickness should not speed up), and the validator permits a non-monotone ramp only when that flag is present — the escape is explicit, never accidental.
3. *Integer bp, invariant culture.* Directly inherited from the economy's bounded-lerp convention: floats in persistence are how determinism dies. The ramp is a pure interpolation function; the same severity always maps to the same multiplier in every locale and every replay.
4. *Optional field, provider-unset pattern.* Absent ramp → legacy flat path, byte-identical, tested. Old saves load unchanged; no migration; rollback is behavior-neutral. This is the same seam shape as `RoomPowerProvider` — the repo has proven it repeatedly.
5. *Composition order with medicine doses.* Ramp multiplies **baseline** recovery only; dose effects (needs/radiation/contamination effects through `DoseLedgerSystem`) apply additively **after**. One test pins the composition order permanently — the double-count failure mode (ramp × dose stacking multiplicatively) is the one real regression risk here, and it is closed by a named test, not by convention.
6. *Legibility requirement.* The medical timeline UI must show the **projected recovery days from the ramp**, not just today's delta. A hidden curve makes early-treatment decisions unmakeable; the whole point of B1 is that the player can see that treating now costs 3 days and treating later costs 9.
7. *Validator bounds.* Points ≥2, x strictly increasing in [0,1], multiplier bounded [100, 2000]‰, monotone-decreasing y unless `chronic: true`. Malformed authored ramps are data-integrity errors, not runtime surprises.

**Implementation pointers (from the parent document P2.3, unchanged):** ramp on the affliction catalog DTO; pure severity-normalized interpolation; ~12-case suite incl. ramp-vs-flat 30-day fingerprint, save/restore mid-recovery (no restart-of-ramp exploit), and the composition-order pin.

**Wave sequencing per the parent document:** Wave 1 = D-A, Wave 2 = D-B, each independently claimable and revertible. Both closeouts update `docs/plans/PLAN_24_CLOSEOUT.md` from CLOSED-WITH-DEFERRALS to CLOSED, with these memos referenced as the signature evidence.

---

# PART D — Expansion Packages for Integrated Plans

Two plans that are already integrated and accepted have clearly identified headroom recorded in their own closeouts and the ledger's deferred-items notes. These are scoped as small, claimable expansion packages — they are *not* reopenings; each extends a sealed system along a seam its closeout already named.

## D.1 `PLAN-174-EXPANSION-COMPANION-ROLES` (extends Plan 174 — companion animals)

### Current state (integrated, green)

Plan 174 shipped persistent companions with deterministic bond/training, the canonical food port (preferred→fallback→partial), bounded guard/pack/morale queries routed into DefenseSystem / ExpeditionSystem / NeedsSystem, bounded once-only grief, veterinary treat command, and old-save adoption — 16/16 + 6/6, plus the 30-day unified replay across Plans 174–177. The species catalog includes the tameable ash hound.

### The headroom (named in the ledger, never filled)

The companion queries that exist are **static capability reads**: a trained guard dog adds a bounded defense bonus; a pack animal adds carry capacity. What no wave shipped is **companion roles as ongoing work** — companions that participate in the duty-hour economy, need upkeep beyond food, and generate their own event vocabulary. Three specific gaps:

1. **Companion upkeep is food-only.** The kennel feeds; nothing else wears. No illness (beyond the treat command's reactive use), no aging, no equipment.
2. **Roles are reads, not assignments.** A companion cannot be *assigned* a duty (patrol route, haul circuit, watch shift) the way survivors are rostered — so the Plan 24 labor cluster and the companion system never meet.
3. **Bond has no expression surface.** Bond/training numbers persist and gate queries, but a high-bond companion behaves identically to a low-bond one in daily play apart from bounded stat deltas — no behavioral tells, no vocabulary events.

### Expansion design (three bounded additions, one seam each)

**(1) Companion duty assignments (the roster seam).**
- Companions become assignable to a small closed set of duty slots: `patrol` (routes guard bonus into DefenseSystem per duty hour rather than as a constant), `haul` (pack capacity contributes only during assigned haul windows — expedition prep days), `watch` (night-watch morale query, replacing the always-on morale bonus).
- Bounded: max 1 active duty per companion; duty interacts with the Plan 24 duty-hour boundary (a companion on watch does not also patrol); assignment persists in the existing companion save section (additive field, no new section, frozen-shape rules).
- This makes kennel management a *scheduling* decision, interlocking directly with the ward-staffing decision signed in Part C — the roster screen gains one column, the same assignment UI pattern.

**(2) Companion wear and afflictions (the upkeep seam).**
- Companions gain a bounded wear track: `fatigue` from consecutive duty days (recover on rest days — the duty roster already knows rest), and a small closed affliction set drawn from the wildlife ecosystem's existing health vocabulary (not the human affliction catalog — no cross-system catalog pollution).
- The existing veterinary treat command becomes the treatment path (already transactional through canonical inventory); the existing bounded grief-once guard covers death. Aging: companions author a `senior_day`; senior companions keep capability but gain fatigue faster — a gentle decline curve, never a cliff, per the genuine-never-hostile philosophy.
- Event vocabulary: companion fatigue, affliction, recovery, and senior transition each get one digest-class event (kind: `Progression` for recovery, `Warning` for fatigue/affliction) — registered with `KindFor` per P3 Wave 1 so this expansion inherits the semantic-kind authority for free.

**(3) Bond expression (the legibility seam).**
- Three behavioral tells, all data-driven and deterministic: a high-bond companion (authored threshold) is named in fetch acquisitions (already the Part A #5 rule — the companion's name appears in the digest line); a high-bond companion's grief event for *its* handler's death is distinct from the generic grief event (one authored variant, exactly-once); a low-bond companion may refuse a duty assignment (bounded refusal chance from the existing seeded sub-stream — refusal is textual and retryable next day, never a lockout).
- No new stats, no new save fields beyond the duty assignment; bond's existing persistence is the only input.

### Package shape

- **Wave 1 (Core):** duty assignment model + fatigue/wear + senior thresholds on the companion catalog DTO (optional fields, legacy byte-identical); ~14-case suite.
- **Wave 2 (Host + UI):** roster column, kennel panel duty rows, vocabulary events through `KindFor`, name-in-digest wiring; ~8-case host suite + a11y/lifecycle gates.
- **Recon note (implementation-time):** confirm the exact companion save section shape and the DefenseSystem query call sites against the current tree before claiming paths — the Plan 174 claim in `WORKTREE_OWNERSHIP.md` records the original paths; three waves have landed since.
- **Rollback:** optional fields + additive queries; revert as one package; unassigned companions resume the always-on legacy bonuses (the provider-unset pattern once again).
- **Balance intent:** companions move from "passive stat items that eat" to "roster members with schedules, health, and relationships" — the same thesis Plan 24 applied to survivors, extended to the kennel. Explicitly out of scope: companion breeding, new species, combat micromanagement.

## D.2 `PLAN-212-EXPANSION-PRICE-LEGIBILITY-AND-MERCHANT-STOCK-RESPONSE` (extends Plan 212 — dynamic economy)

### Current state (integrated, accepted)

Plan 212 shipped the v2 MarketSystem: commodity baselines (12 categories, grounded above), category indices, trade pressure with elasticity classes, shocks with `OnShockStarted`/`OnShockExpired`, `ExplainPrice` typed factor rows, bounded-lerp daily ticks, weather→shock mapping, and the market-rumor band bridge from `FOLLOWUPS-210-213-THINSEAMS`. The ledger's own deferred list for the wave named what did not land: **merchant stock response** (deferred to a host/save package that was folded into later waves) and **trade rumors from real market state** (subsequently sealed by the rumor band bridge). Merchant restock *priority* is P5. What remains genuinely unfilled is narrower and more interesting: **the player cannot see the market they are inside.**

### The headroom

1. **`ExplainPrice` exists; the player-facing price screen does not consume it as a live market view.** The typed factor rows (Demand / Category / Shock) are consumed by tests and the trade flow, but there is no panel that shows *the state of the market* — active shocks, pressure direction per category, distance from baseline. The economy is a simulation the player is blind inside.
2. **Merchant stock does not respond to shocks.** Prices move; shelves do not. A blizzard spikes food *prices* (weather→shock mapping) but the merchant's *stock mix* is unaffected until P5's priority ordering lands — and even then, priority orders the restock *set* without expressing shock-driven depth changes.
3. **No price history read model.** The daily tick produces a rich series; nothing persists a bounded window for the player to observe trend ("food is climbing, third day running").

### Expansion design (three bounded additions)

**(1) Market board panel (the legibility seam).**
- A read-only `MarketBoardPanel` over the existing v2 state: per category — current index vs. baseline band (floor/ceiling **[grounded]**), pressure direction (one of three words: rising/falling/easing — derived from the decayed pressure sign and magnitude bands), active shocks with authored remaining-duration text ("blizzard scarcity, 2 days").
- Zero new state: pure read model over MarketSystem's capture; the panel never advances any tick (the C1 panel-open invariant, tested). A11y: all information in words + numbers, never color-only; three-band pressure language keeps cognitive load low.
- One explain affordance per row: selecting a category shows its `ExplainPrice` factor rows verbatim — the machinery already built becomes the player's tooltip.

**(2) Shock-driven stock depth (the merchant seam — deliberately narrower than P5).**
- Where P5 orders *which* goods restock, this expansion adds one bounded rule: an active shock in a category raises the *quantity band* of that category's stock rows on the next restock day (authored multiplier, e.g. +500‰ **[proposed]**, clamped to the row's capacity). Rationale: scarcity events make merchants pile in what's suddenly expensive — the world visibly reacts to its own crisis on the shelf, not just the price tag.
- Pure function of signed inputs (shock severity/duration + row capacity), deterministic, no RNG, no new save fields (stock state already persists). Authored in the merchant stock catalog; absent rule → legacy restock quantities byte-identical.
- Ordering dependency: implements cleanly before or after P5 (they compose: priority picks the set, depth scales the quantity); both orderings get a composition test.

**(3) Bounded price history read model (the trend seam).**
- The market save section gains a bounded 14-day **[proposed]** rolling window of per-category index snapshots (sorted, deterministic, frozen-shape migration with old saves filling the neutral no-history default — the RadioSave V4→V5 pattern).
- The market board renders the window as a textual trend line per category ("950 → 980 → 1010 → 1080, rising 3 days") — words and numbers, no chart widget, keeping the UI restrained per repo cadence. Rumor interlock: the market-rumor band bridge already projects shock start/expiry into radio text; the trend line gives that rumor a verifiable referent — a player who hears "prices are moving" can check the board and act, closing the rumor→decision loop the band bridge opened.

### Package shape

- **Wave 1 (Core read models):** price-history window (save migration) + shock-depth pure rule + tests (~12 cases: window boundedness, migration neutrality, depth clamp, composition with legacy restock).
- **Wave 2 (Host + panel):** `MarketBoardPanel` with explain affordance, trend rendering, a11y/lifecycle/snapshot gates; host wiring tests (~8 cases incl. panel-open-never-ticks, restore-never-replays).
- **Interlocks to respect:** P5 (composition test both orderings); Part A route #19 (the aggregate restock digest line now also reflects shock-depth days — one wording decision, pinned by golden-day test); the P3 Wave 1 kind authority classifies the panel's data as `WorldState` (never alert-class — markets inform, they do not alarm).
- **Rollback:** panel is revertible standalone; the history window is an additive save field with neutral migration (dropping it on rollback is a no-op for loads); the depth rule is optional-catalog (provider-unset pattern). Three independently revertible seams inside one claim, each with its own test gate.
- **Balance intent:** the v2 economy becomes *legible enough to strategize around* — buy before the shock, sell into the spike, trust the rumor because the trend line confirms it. No numbers in the simulation change; only the player's access to it does. That is the highest-leverage cheap expansion in the repo: all the machinery is already built, tested, and signed.

---

# Closing Note

This document plus its parent now cover: six unblocked packages (16 waves), a fully enumerated acquisition census (26 routes, classified), a grounded armor grade ladder, two signed decision memos, and two integrated-plan expansions (5 waves). Every package preserves the standing invariants, claims exact paths, and ships with closeouts, test matrices, balance rationale, exploit analysis, and rollback plans. The recommended global claim order remains: **P1 → P2 (now signed, ready) → P5 → P6 (grades specified) → P3 (census complete) → P4**, with D.1 and D.2 claimable in parallel after P3 Wave 1 lands the kind authority they both consume.

---

# PART E — Implementation Pseudocode and Test Specifications for Key Seams

This part provides implementation-level detail for the four highest-risk seams in the combined package set. All pseudocode is **design communication, not production code** — it follows repo conventions (pure Core functions, permille integer math, provider-unset legacy paths, exactly-once ledgers) but must be adapted to the actual tree at claim time.

## E.1 Ward staffing throughput (P2 Wave 1, memo D-A)

```csharp
// Core — pure function, no host state, no RNG
public static int ComputeWardThroughputBp(
    WardFacilityRow facility,          // catalog row: staffed_throughput_multiplier_permille, staff_skill_tags
    DutyRosterSnapshot roster,          // existing Plan 24 labor-cluster read model
    int dutyHourWindowStart,
    int dutyHourWindowEnd)
{
    // Legacy path: facility authored no staffing block -> multiplier 1000, byte-identical
    if (facility.Staffing is null) return 1000;

    bool staffed = roster.HasAssignedSpecialist(
        facility.Staffing.SkillTags, dutyHourWindowStart, dutyHourWindowEnd);

    int multiplier = staffed
        ? facility.Staffing.StaffedMultiplierPermille          // default 1000
        : facility.Staffing.UnstaffedMultiplierPermille;       // floor 400, validator-enforced

    return Math.Clamp(multiplier, 400, 1000);   // hard floor is code-level; the authored value is data
}
```

**Test specification (`Plan24WardStaffingTests.cs`, 10 cases):**

| # | Case | Assertion |
|---|---|---|
| 1 | Legacy absence | facility without staffing block → 1000‰, output byte-identical to pre-package run (golden capture) |
| 2 | Staffed ward | eligible specialist on duty during window → authored staffed multiplier |
| 3 | Unstaffed floor | no specialist → authored unstaffed multiplier, never below 400‰ |
| 4 | Wrong-tag specialist | specialist with non-matching skill tags does not count (tag intersection empty) |
| 5 | Duty-hour boundary | specialist assigned to a disjoint hour window does not count (no overlap credit) |
| 6 | Multi-ward | two wards, one specialist: only the ward whose window matches gets credit; per-facility independence |
| 7 | Save/restore mid-treatment | treatment in progress across a save boundary resumes with identical projected completion day (fingerprint) |
| 8 | Validator rule | staffing block with multiplier < 400 or > 1000 fails data-integrity with catalog/row/field named in the error |
| 9 | No-specialist 30-day simulation | shelter with zero medical specialists, unstaffed 400‰ floor: no unrecoverable death spiral (survivor outcomes bounded; the falsifiability test for the 0.4 number) |
| 10 | Source gate | grep-style sweep: no hardcoded staffing multipliers outside the catalog loader (mirrors the RNG source gate technique) |

## E.2 Recovery ramp interpolation (P2 Wave 2, memo D-B)

```csharp
// Core — pure, integer bp, invariant culture, zero allocation in hot path
public static int RecoverySpeedPermille(AfflictionRow affliction, int severityPermille)
{
    // Legacy path: no authored ramp -> flat 1000 (byte-identical)
    if (affliction.RecoveryRamp is null || affliction.RecoveryRamp.Points.Count < 2)
        return 1000;

    var pts = affliction.RecoveryRamp.Points;   // validator guarantees: >=2, x strictly increasing in [0,1000], y in [100,2000]
    int x = Math.Clamp(severityPermille, 0, 1000);

    if (x <= pts[0].X) return pts[0].Y;
    if (x >= pts[^1].X) return pts[^1].Y;

    for (int i = 1; i < pts.Count; i++)
    {
        if (x <= pts[i].X)
        {
            // integer linear interpolation, no floats
            int span = pts[i].X - pts[i - 1].X;
            int offset = x - pts[i - 1].X;
            return pts[i - 1].Y + (pts[i].Y - pts[i - 1].Y) * offset / span;
        }
    }
    return pts[^1].Y;   // unreachable; total function by construction
}

// Daily application — composition order is the pinned contract:
//   newSeverity = severity - (baselineRecovery * RecoverySpeedPermille(severity) / 1000) - doseEffects
// Ramp multiplies BASELINE only; dose effects are additive AFTER. Test #5 pins this forever.
```

**Test specification (`Plan24RecoveryRampTests.cs`, 12 cases):** interpolation table over authored ramps (exact bp values at and between points); legacy-absence parity; composition-order pin (ramp × dose cannot stack multiplicatively — assert exact expected value for a fixed scenario); monotone validator acceptance; `chronic: true` non-monotone acceptance + rejection without flag; save/restore mid-recovery continues on the same ramp (severity-persisted, no restart-of-ramp exploit: a survivor at 600‰ severity saved and restored recovers at 600‰'s multiplier, not the head of the curve); exactly-once daily application; 30-day ramp-vs-flat comparison fingerprint; bounds rejection (y outside [100,2000]); UI read-model projection (projected recovery days shown for three severity checkpoints); invariant-culture parse test (ramp authored with `.` and `,` decimal locales round-trips identically); multi-affliction survivor independence.

## E.3 Availability weighting (P4 Wave 1, model M1)

```csharp
// Core — eligibility check seam; provider-unset -> legacy byte-identical
public bool IsEligibleToday(SignalDefinition signal, int currentDay, RadioRollContext ctx)
{
    if (!MeetsBaseEligibility(signal, currentDay)) return false;
    if (signal.IsDiscovered || missionManager.IsResolvedOrDispatched(signal.Id)) return false;  // exempt classes

    // NEW: trust-shaped availability (M1). The roll lives on the EXISTING
    // StableHash-derived distress sub-stream — no new RNG stream, ever.
    if (availabilityProvider is not null)
    {
        int weightPermille = availabilityProvider.GetAvailabilityPermille(trustLedger.Score);
        // weightPermille in [500, 1500], monotone in trust, integer-only (sealed policy)
        int roll = ctx.DistressSubStream.NextBipolarChance(signal.Id, currentDay);  // existing deterministic roll
        // Scale the qualification threshold, never the RNG: same seed + same trust history
        //  -> identical schedule; different trust -> smoothly shifted schedule.
        return roll < ScaleThreshold(signal.BaseChancePermille, weightPermille);
    }

    return ctx.DistressSubStream.Qualifies(signal.Id, currentDay, signal.BaseChancePermille);  // legacy
}
```

**Key determinism argument (to be proven by test, not asserted in review):** the sub-stream draw sequence is unchanged; availability only transforms the threshold each draw is compared against. Therefore a trust-history replay is a pure function of (seed, ledger score trajectory) — the 30-day three-profile fingerprint tests are the proof. Band thresholds for the player-facing line: `thin` < 800‰, `steady` 800–1200‰, `busy` > 1200‰ **[proposed]**.

## E.4 Acquisition digest aggregation (Part A, Phase E)

```csharp
// Core — pure fold over the day's acquisition events; kind = Progression/Acquisition
public static string BuildAcquisitionsDigestLine(IReadOnlyList<DayEvent> dayEvents)
{
    // Closed source classes, fixed render order (deterministic, no RNG, no culture variance):
    //   scavenged | forged | harvested | caught | bought | received
    // Rules from A.4: exactly-once events in, dedupe already applied upstream by
    // the precedence rules (#17>#15, #4>#2), companion fetch counted under scavenged
    // with the companion named, trapping counted under caught with source "trap".
    // Output shape (golden-day pinned):
    //   "Gains: scavenged 6 items, forged 2, harvested 4 (greenhouse), bought 3 (market)."
    // Zero items -> line omitted entirely (no "Gains: nothing" noise).
}
```

**Test specification (`AcquisitionSweepMatrixTests.cs`, generated):** one row per census route (26) asserting emission class; suppression tests for both dedupe seams; exactly-once + save/restore-mid-day parity for each `EMIT-ADD` route; digest golden-day pins for five authored days (empty day, single-class day, all-classes day, dedupe-collision day, restock-with-shock-depth day); zero-item omission; kind classification of every new id (`Progression` digest class, never `Warning`).

---

# PART F — Two Further Integrated-Plan Expansions

## F.1 `PLAN-175-EXPANSION-RITUAL-CADENCE-DEPTH` (extends Plan 175 — zealotry)

### Current state (integrated, green)

Plan 175 shipped the fictional-only zealotry system: loader guard, deterministic conversion with real resistance, bounded buff queries (morale authority applies), transactional ritual cadence through canonical inventory, an escalation ladder hard-stopping at typed `AssaultThreat`, capped PsyOps broadcast, crisis→bounded morale damage routing, and friction-belief old-save adoption — 20/20 + 5/5 plus the unified replay.

### The headroom

1. **Rituals are transactions, not occasions.** The cadence is authored (transactional, inventory-consuming) but rituals do not *vary* — the same rite, same inputs, same text, every time. Ritual repetition with zero texture reads as a resource drain, not a living faith.
2. **The escalation ladder stops at `AssaultThreat` and stays there.** The typed hard-stop was correct (host routes violence, Core never simulates it), but the *de-escalation* side was never authored: a shelter that pulled back from the brink has no path back down the ladder — fervor only ever ratchets.
3. **Fictional-only is enforced, but flavor-thin.** One religion catalog (`wasteland_religions.json`) exists; conversion resistance and friction-belief are systemic, but the corpus is small enough that zealotry playtests collapse into repetitive patterns quickly.

### Expansion design (three bounded additions)

**(1) Ritual variation via seeded flavor tables.** Each ritual definition gains an optional `flavor_variants` list (3–5 authored texts); the existing deterministic sub-stream picks per-occurrence by (ritual id, day) — same day, same variant, always. Inputs stay fixed (the transactional contract is untouched); only narration varies. Deterministic, zero new systems, pure catalog content plus one selection call.

**(2) De-escalation ladder rungs.** Mirror-image authored conditions for stepping *down*: consecutive calm days without incident at each fervor tier, a successful crisis-averted event, or an authored atonement ritual (consumes scarce goods — de-escalation has a price, mirroring how escalation costs standing). The `AssaultThreat` hard-stop remains absolute; de-escalation can never skip tiers (one rung per qualifying period). Host routing unchanged — this is ladder data + tier-transition rules in Core, exactly-once transitions tested.

**(3) Religion corpus tranche.** Author 3–4 additional fictional religions into the existing catalog exercising distinct mechanical identities: a scarcity-cult (rituals consume food — tension with the ration journey), a memory-faith (rituals interact with the archive/journal vocabulary — synergy with the `Progression` kind class), a techno-reverence sect (rituals consume foundry output — interlock with P6 armor production). Each is catalog data through the sealed loader; the fictional-only guard already rejects real-world references at load.

**Package shape:** one wave of catalog content + two small Core rules (variant selection, de-escalation transitions); ~12-case suite; closeout notes the design intent — faith becomes a system with *texture and a way back down*, not a one-way ratchet.

## F.2 `RESCUE-SIGNALS-EXPANSION-PRESENTATION-DEPTH` (extends the rescue-signal runtime)

### Current state (integrated, green)

The rescue-signal runtime (Tasks 1–4 + nine follow-on waves) is complete and accepted: dispatch, authenticity, consequences, sender survival, V6 save, trust ledger, follow-up scheduler, audio cues, the RESCUE SIGNALS panel strip.

### The headroom

1. **The trust ledger is invisible.** Trust exists in [0,100], drives availability (P4) and follow-up trap weighting, but the player never sees their own standing. The P4 traffic-band line covers the availability *consequence*; nothing covers the *cause* ("you have ignored four genuine calls").
2. **Deadline pressure has no audio/visual arc.** The strip shows the deadline as text; there is no rising urgency as the window closes — the audio cue fires once on Intercept and goes quiet, even as a sender approaches death.
3. **Answered-signal outcomes lack a memorial.** Sender survival/death is journaled, but rescued survivors vanish from radio play — no epilogue touch, no "the warden you saved still broadcasts on channel 3" texture, despite the epilogue chronicle system (Plan lineage 96) having exactly the seam for it.

### Expansion design (three bounded additions)

**(1) Trust strip legibility.** One line on RadioPanel: trust band in words (`distrusted` / `unknown` / `reliable` / `trusted` **[proposed]** bands over [0,100]) plus a one-sentence cause summary generated from the ledger's last three events ("answered two, ignored one"). No raw numbers — bands only, a11y-safe, digest-class kind.

**(2) Deadline urgency arc.** Two additions, both deterministic: (a) the stage resolver's existing stage transitions already carry `audio_cue` overrides — author late-stage cues for the deadline-window signals (content-only, P1-compatible); (b) the panel strip gains an authored urgency word per stage (`distant` / `urgent` / `final hours`) — text derived from days-remaining thresholds, kind `Warning` only in the final band. No new state; pure derivation from the persisted deadline.

**(3) Rescued-sender epilogue hooks.** The epilogue chronicle gains one typed fact per resolved rescue (already persisted: mission outcome, sender identity, arrival branch). Two authored epilogue sentence families (live rescue / remains recovered) consumed by the existing `EpilogueContextFactory.Build` — the 32/32-branch reachability test extends naturally. Plus one post-rescue ambient touch: a rescued sender's origin frequency becomes eligible for a single `Progression`-class ambient broadcast within N days (authored, bounded once, pure flavor through the existing faction radio corpus pattern).

**Package shape:** mostly content + read models; one wave Core (epilogue facts) + one wave presentation (strip, cues); ~10-case suite; ordering dependency: lands after P1 so late-stage cues and follow-up content author coherently.

---

# PART G — Risk Register, Exploit Catalog, and Rollout Schedule

## G.1 Consolidated risk register (all packages)

| Risk | Affected | Likelihood | Impact | Mitigation | Owner test |
|---|---|---|---|---|---|
| Determinism regression from new seams | P2, P4, P5, P6, D.2 | Medium | High | No new RNG streams (source-gated); threshold-scaling not draw-altering; continuous==save/restore fingerprints every wave | Every package's fingerprint tests |
| Save bloat from new persisted fields | P6 (plates), D.2 (history window), D.1 (assignments) | Low | Medium | Bounded collections (3 plates, 14-day window, 1 duty slot); caps validator-enforced | Boundedness cases |
| Alert/event spam overwhelming briefing | P3 all waves, Part A | Medium | Medium | Digest-class not alert-class; concurrency cap N=3 from Wave 2; zero-item omission | Golden-day pins + 20-event stress test |
| Double-counting across composing systems | P2 (ramp × dose), P5 (priority × depth), Part A (dedupe seams) | Medium | High | Pinned composition-order tests; authored precedence rules; both-ordering composition tests where packages interact | Named composition tests per seam |
| Softlock from hard gates | P2 (A1 ward staffing) | Low | High | 0.4 authored floor + no-specialist 30-day falsifiability simulation | Simulation case #9 |
| Content authoring drift from contracts | P1, F.1, F.2 | Medium | Low | Validators tightened in the same wave as content; content-utilization selftest catches orphans | `--content-utilization-selftest` |
| Worktree collision with unclaimed dirty changes | All | Medium | Medium | Fresh claims with exact paths; premise-correction recon before each claim (the Plan 174 lesson: three waves landed since its claim) | `WORKTREE_OWNERSHIP.md` discipline |
| UI regression on shared panels (RadioPanel, roster, garage) | P4, D.1, P6, F.2 | Medium | Medium | Panel lifecycle + a11y + snapshot gates every presentation wave; focus-restoration tests | `--panel-bind-lifecycle-selftest`, `--ui-a11y-selftest` |
| Migration mishandling old saves | D.2 (history window) | Low | High | Frozen-shape migration to neutral default, RadioSave V4→V5 pattern; migration version-pin tests | Migration pin cases |

## G.2 Exploit catalog (player-side degenerate strategies, pre-analyzed)

1. **Trust farming then coasting (P4):** farm answers to 100, then ignore everything. Bounded ledger + monotone availability map means coasting decays only via *future* ignores; availability floor 500‰ never locks the mechanic. Verdict: intended redemption-arc tension, not an exploit. Test pins the coasting curve.
2. **Restock steering via market dumping (P5):** dump a category to suppress its restock priority. Bounded by ±3500bp pressure cap, 0.75/day decay, and the 0.4× low-elasticity response on materials. Max single-day ordering swing pinned by test.
3. **Armor plate cycling (P6):** refit fresh plates pre-trip to dodge wear. Closed by per-trip wear ordering: the trip's wear hits the plates fitted at dispatch time; re-forge costs 50% scrap regardless. Wear-order pin test.
4. **Armor stacking to invulnerability (P6):** 3× G1 ≤ single G4 mitigation cap; total mitigation hard-capped at −2500bp on the vehicle-flagged slice only; survivor-side risk untouched. Anti-stacking validator + test.
5. **Follow-up trust pumping (P1):** chaining answers for trust. Follow-up answers route the same +2 delta; no chain bonus; 30-day all-answer replay asserts the [0,100] ceiling holds. Content cap: max 2 follow-ups per parent.
6. **Ward-staffing hour gaming (P2):** assign a specialist for the minimum overlapping hour. Duty-hour boundary test requires window overlap credit only (no proration exploit — the multiplier applies to the ward's throughput for the day if the window is covered, authored as day-granular, documented).
7. **Digest-scraping for free intel (Part A):** the acquisitions digest reveals what survivors produced without walking to the station. Accepted: it is *your* shelter's digest — internal legibility, not world intel. Restock line (#19) is aggregate-only, so no merchant intel leaks.
8. **Ritual de-escalation cycling (F.1):** escalate to a tier, atone, repeat for... nothing — de-escalation grants no buff, consumes goods, and fervor tiers gate buffs monotonically. Cycling is strictly a resource loss. Documented as closed.

## G.3 Rollout schedule (recommended claim sequence with dependencies)

```
Week 1:  P1 W1 (content+validators)  ->  P1 W2 (population tests)
Week 2:  P2 W1 (ward staffing — memo signed)  ||  P5 W1 (restock rules, parallel-safe, disjoint paths)
Week 3:  P2 W2 (recovery ramp)       ||  P5 W2 (host wiring + memo closeout)
Week 4:  P6 W1 (armor core+recipes)  ->  P6 W2 (decoration+garage UI)
Week 5:  P3 W1 (kind authority)  -- the keystone; vocabulary frozen after this point
Week 6:  P3 W2 (alert concurrency)  ||  D.2 W1 (economy read models — kind-gated)
Week 7:  P3 W3 (acquisition sweep, census in Part A)  ||  D.2 W2 (market board panel)
Week 8:  P3 W4 (deep matrix + closeout)
Week 9:  P4 W1–W2 (availability consumer — benefits from P1's population)
Week 10: P4 W3 (presentation)  ||  F.2 (rescue presentation depth — after P1 by design)
Week 11: D.1 (companion roles — after P3 W1 for KindFor)  ||  F.1 (zealotry depth, independent)
Week 12: Full-suite verification pass, docs index regen, ledger reconciliation, foreman acceptance review
```

**Dependency notes:** P3 W1 must precede D.1, D.2, F.1, F.2 (all consume `KindFor`); P1 must precede F.2 (late-stage cues + epilogue hooks author coherently with follow-up content); P5 and P6 compose freely with D.2's stock-depth rule (composition tests in both orderings); P2 is fully independent and signed — it can start immediately.

## G.4 Acceptance review checklist (foreman-facing, per package)

- [ ] Claim id fresh; paths exact; dirty worktree preserved
- [ ] Build 0/0 all targets; full suite zero regressions
- [ ] Data-integrity PASS; catalog registry regenerated; content-utilization PASS (0 orphans)
- [ ] Determinism fingerprints stable; no new RNG streams (source gate)
- [ ] Save migrations frozen-shape with neutral defaults + version pins
- [ ] Exactly-once proven for every player-visible consequence
- [ ] A11y: words never color-only; focus restored; lifecycle PASS
- [ ] Balance rationale documented with derived numbers ([grounded]/[proposed] marked)
- [ ] Exploits analyzed with named closing tests
- [ ] Closeout doc + ledger row + implementation log updated; docs index PASS
- [ ] Decision memos referenced where signature-gated (D-A, D-B signed 2026-09-18)

*End of deep-dive expansion document. Parent: `ashfall-unblocked-integration-plans`. Combined coverage: 8 packages (6 unblocked + 4 expansions), 21 waves, 26-route acquisition census, 4-grade armor ladder grounded in live catalog data, 2 signed memos, 10-seam pseudocode and test specifications, 8-entry exploit catalog, 9-entry risk register, 12-week rollout schedule.*