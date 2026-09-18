# ASHFALL — Foreman Decision Packet

**Date:** 2026-09-18 · **Prepared for:** Foreman (user) · **Prepared by:** Integrator
**Scope:** every decision currently parked on a foreman signature, swept from all live ledgers.
**Method:** static inspection only — no production code changed to produce this document.

## How to use this packet

Each item ends with a **Sign-off line**. Copy it, replace the bracketed choice, and paste it back.
A verdict is recorded verbatim the way `docs/plans/wave8_part2/C2_DECISION.md:32` records one
(`**Foreman signature received:** "I approve C2-D3"`), so an explicit quote is what closes an item.

Nothing here may be implemented before its line is signed — AGENTS.md Rule 10.

**Counts:** 23 decision items (D1–D23) · 6 governance-hygiene defects (G1–G6).
Of the 23: **11 pure policy** (one line each), **12 schema/architecture or product calls**.
Four items (D16–D19c) carry **unverified premises** and are labelled as such — do not sign those
without a re-audit.

---

## 0. Premise corrections — read before signing anything

Three widely-repeated claims in the ledgers are **false**. They are restated in documents you may
have already read, so they are corrected here first.

### 0.1 "All 5 Plan 30 projection events have zero `src/` subscribers" — false, it is 4 of 5

`OnFactionStandingChanged` **has a live production subscriber**:
`src/YearOfAsh/FactionWarMapWidget.cs:75` subscribes, `:85` unsubscribes, `:89` handles; the widget
is instantiated in production at `src/Main.YearOfAsh.cs:261` (field declared `:39`).

Verified **zero** `src/` subscribers (Core declaration + `Invoke` only):

| Event | Declared | Invoked |
|---|---|---|
| `OnTerritorialClashOccurred` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs:70` | `:202` |
| `OnDecreeEnacted` | `FactionWarSystem.cs:69` | `:119` |
| `OnStageSurfaced` | `Assets/Ashfall.Core/YearOfAsh/FactionWarChainRunner.cs:311` | `:424` |
| `OnStageResolved` | `FactionWarChainRunner.cs:312` | `:481` |
| `OnChainResolved` *(not previously listed anywhere)* | `FactionWarChainRunner.cs:313` | `:489` |

Documents asserting the false 5/5 claim, needing correction:
`docs/plans/wave11_part1/B1_PLAN30_IMPLEMENTATION_LOG.md:57` and
`docs/plans/UNCLAIMED_CORPUS_CENSUS.md:53`.

Also confirmed from the same log: `RecordWarLocationVisited` (`src/YearOfAsh/YearOfAshHostSession.cs:215`)
and `ResolveWarChoice` (`:220`) have **zero** `src/` callers; `FactionWarChainRunner.TickDay` is wired at
`:165` and `SimulateDailyFriction` at `:164`. The log's line cite for the day gate is **stale** — it says
`src/Main.CampaignOwners.cs:1059-1062`; the real gate is `src/Main.CampaignOwners.cs:1396-1399`
(`if (day >= 180 && day <= 360)` … `_m._yearOfAsh.TickDay(day);`). Clamp constants confirmed at
`Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs:36-37` (`StartDay = 180`, `EndDay = 360`).

### 0.2 `A1_BRIEFING_DEFERRED.md` is not an open gate

Its own header closes it: `A1_BRIEFING_DEFERRED.md:3` — `**Status:** RESOLVED (Core predictor complete
& verified; DailyBriefing consumer integrated 2026-09-17)`. The Plan 31 semantic-kind item lives in
`WAVE9_PART1_CLOSEOUT.md` and `WORKTREE_OWNERSHIP.md` instead — see **D11**.

### 0.3 Radio weather authority is already signed; the sweep is the stale record

`docs/governance/DECISION_REGISTER.md:29` (`DEC-15`, status **`SIGNED`**) already records the verdict
`WAVE10_MICRO_DEFERRAL_SWEEP.md:38` calls `DECISION-NEEDED`. See **D4** — it needs ratification of an
existing verdict, not a fresh decision.

---

## 1. The two signatures that close Plan 24

`docs/plans/PLAN_24_CLOSEOUT.md:3` states the plan is
`CLOSED-WITH-DEFERRALS — PENDING TWO FOREMAN SIGNATURES`. These are those two. Signing both flips
Plan 24 to closed and lets the `C1-PLAN24-SURVIVOR-LEDGER` claim move ACTIVE → DONE.

### D1 — Medical ward staffing authority

- **Status:** `OPEN — AWAITING SIGNATURE`
- **Evidence:** `docs/plans/PLAN_24_CLOSEOUT.md:22` — *"| 3 | Ward staffing | **OPEN — AWAITING SIGNATURE** | A2 implementation-selective memo (options a/b/c; recommendation b). No authority fabricated |"*; corroborated at `docs/plans/C1_planintegration[5]_IMPLEMENTATION_LOG.md:170`, `WORKTREE_OWNERSHIP.md:28`, `docs/plans/WAVE10_MICRO_DEFERRAL_SWEEP.md:40`, `docs/governance/DECISION_REGISTER.md:15` (`DEC-01`, `DEFERRED-WITH-CONDITION`).
- **Blocks:** Plan 24 acceptance row 3; the 24B `WorkerProductivityContract` medical producer row; `MedicalWardSystem` procedure staffing. Verification target `Plan24DutyRosterFitnessTests`.
- **Options** (verbatim, `C1_planintegration[5]_IMPLEMENTATION_LOG.md:159-163`):
  - **(a)** staffing requirements on `MedicalProcedureDef` (`min_staff` / `staff_skill_id`, `RunProcedure` preflight)
  - **(b)** ward duty-role contract — additive `ward` row in `duty_roles.json`, roster preflight
  - **(c)** defer staffing
- **Recorded recommendation** (`:166-168`): *"(b) — it is the 24A pattern (data-authored role contract through the existing roster engine), needs no new authority, and makes ward staffing visible on the duty surface. (a) is viable but forks a staffing model beside the roster; (c) leaves the 24A subscope permanently open."*
- **Conflict to resolve:** `DECISION_REGISTER.md:15` reframes this as already chosen in principle — *"Option B … selected in principle; deferred until medical ward capacity reaches >4 patients"* — while `PLAN_24_CLOSEOUT.md:22` says `OPEN — AWAITING SIGNATURE`. No user signature quote exists in either. One line closes both readings.
- **Blast radius if (b):** `Assets/StreamingAssets/Data/duty_roles.json` (verified exactly 5 roles today — `night_watch`, `mess`, `hatch_opener`, `intake_sleeper`, `expedition` at lines 26/41/56/71/86; **no** ward role), `DutyRosterSystem.cs`, `MedicalWardSystem.cs`, `src/UI/DutyRosterPanel.cs`, `Plan24DutyRosterFitnessTests`. **No new save section** — assignments already persist.
- **Type:** (b) data + small code · (a) schema · (c) pure policy.
- **Sign-off line:** `D1 ward staffing: I choose option [(a) / (b) / (c)].`

### D2 — Affliction-specific recovery ramp

- **Status:** `OPEN — AWAITING SIGNATURE`
- **Evidence:** `docs/plans/PLAN_24_CLOSEOUT.md:28` — *"| 9 | Affliction-specific recovery ramp | **OPEN — AWAITING SIGNATURE** | A3 design note (options i/ii/iii; admissions carry no cause field; no dormant data). No ramp authority fabricated |"*; corroborated at `C1_planintegration[5]_IMPLEMENTATION_LOG.md:374` and `:390`, `WORKTREE_OWNERSHIP.md:28`, `WAVE10_MICRO_DEFERRAL_SWEEP.md:41`, `DECISION_REGISTER.md:30` (`DEC-16`).
- **Blocks:** Plan 24 acceptance row 9 / subscope 24C. Verification target `Plan24SurvivorJourneyTests`.
- **Key premise** (`C1_planintegration[5]_IMPLEMENTATION_LOG.md:375-380`): recovery timing is *already* data-authored per disease (`illness_days`, immunity windows) and per duty contract (`discharge_recovery_days: 2`); admissions carry **no** cause field and no authored recovery-rate field sits unconsumed anywhere. So a per-affliction curve **cannot be wired as a consumer** — it needs new clinical semantics, which is a product decision, not an unblock.
- **Options** (verbatim, `:382-388`):
  - **(i)** cause-tracked discharge ramp — admissions gain an additive `cause` field, `duty_roles.json` gains per-cause recovery windows
  - **(ii)** declare the uniform data-authored ramp complete — *"the existing discharge_recovery_days projection IS the ramp (uniform by authoring)"*
  - **(iii)** defer
- **Recorded recommendation** (`:390`): *"(i) if affliction-specific recovery is a design goal; otherwise (iii)."*
- **Blast radius if (i):** `MedicalWardSystem` admission call sites, one additive save field, `duty_roles.json` per-cause windows, fitness projection. **If (ii) or (iii): zero code, docs only.**
- **Type:** (i) schema + code · (ii)/(iii) pure policy.
- **Sign-off line:** `D2 affliction recovery ramp: I choose option [(i) / (ii) / (iii)].`

---

## 2. The Plan 30 / FactionWar cluster — sign these four together

D5, D6, D7 and D14 all touch the same producer (`FactionWarSystem` / `FactionWarChainRunner`) and the
same event (`OnTerritorialClashOccurred`). Signing them in one pass avoids editing that file twice.

### D5 — Plan 30 runtime clock (**the headline blocker**)

- **Status:** `DECISION-BLOCKED`
- **Evidence:** `docs/plans/wave11_part1/B1_PLAN30_IMPLEMENTATION_LOG.md:31` — *"either the war chains fire within the 180–360 window (requiring the calendar to use Timeline.CurrentDay, not _simDay), or the runner must be ticked past day 360 in extended play. **This requires a foreman decision before implementation.**"*; restated at `:65`, `WAVE11_PART1_CLOSEOUT.md:110` (under `## Open Decision Blocks (Require Foreman Signature)`, `:108`) and `:67`, `UNCLAIMED_CORPUS_CENSUS.md:53`, `WORKTREE_OWNERSHIP.md:11`.
- **Independent corroboration with a richer option set** — `docs/forensics/FACTION_WAR_COMMUNIQUE_SURFACE_FORENSIC_REPORT.md:334-339`: *"whether the war arc's 480–607 axis is intended for (a) a post-360 extended-campaign mode, (b) a future calendar re-banding, or (c) an epilogue-matrix evaluation horizon only. The authored corpus, the epilogue saga horizon (360→3,650 days), and the uncapped campaign calendar all point to (a), but no host clock implements it today."*
- **Blocks:** census row `C2[10]` (Plan 30 — "The War Runs Without You"); all of 30B; the no-action integration test.
- **Content currently unreachable in every campaign:** war chains authored for days 480–607, communiqués 489–607, radio 480–600 (`B1_PLAN30_IMPLEMENTATION_LOG.md:26-27`).
- **Options:** **(a)** post-360 extended campaign / uncapped `_simDay` · **(b)** re-band the calendar so chains fire inside 180–360 · **(c)** epilogue-matrix evaluation horizon only. Forensic evidence leans **(a)**; the B1 log frames it as a binary (`Timeline.CurrentDay` capped vs `_simDay` uncapped).
- **Blast radius:** if (a) — `src/Main.CampaignOwners.cs:1396-1399`, `YearOfAshTimelineSystem` clamp semantics, `YearOfAshHostSession.TickDay`, post-360 timeline save/restore, determinism re-pinning. If (b) — re-authoring chain/communiqué/radio day windows across the YearOfAsh catalogs (data-heavy, no clock change).
- **Type:** architecture + code (a), or data (b). **Not a pure policy call.**
- **Sign-off line:** `D5 Plan 30 runtime clock: I choose [(a) uncapped post-360 / (b) re-band into 180-360 / (c) epilogue horizon only].`

### D6 — Plan 30 consequence-reach scope (30B)

- **Evidence:** `B1_PLAN30_IMPLEMENTATION_LOG.md:66` — *"**Consequence reach scope:** Which consequence routes (economy shock, expedition risk, radio, airlock) are in scope for the next implementation package?"*; `:57` — *"This violates C2[10] §7.9 ("No invisible autonomy") — the world ticks, but no player consequence exists."*; `WAVE11_PART1_CLOSEOUT.md:68` marks it `ARCHITECTURE-GAP`.
- **Recorded recommendation** (`B1_PLAN30_IMPLEMENTATION_LOG.md:79`): *"Wire at minimum one consequence reach path (e.g., OnTerritorialClashOccurred → radio) as proof-of-concept."*
- **Blocks:** census `C2[10]`; also gates **D14**, which wants the same producer.
- **Options:** name which of economy shock / expedition risk / radio / airlock security is in the next package.
- **Blast radius:** subscribers in `src/` for the 4 confirmed-unsubscribed events (§0.1), plus whichever Core owner is routed to. **Bounded if you pick one route.**
- **Sign-off line:** `D6 30B consequence reach: first route is [economy / expedition / radio / airlock].`

### D7 — Plan 30 scope for 30C (caravan / waystation / wildlife / maritime autonomy)

- **Evidence:** `B1_PLAN30_IMPLEMENTATION_LOG.md:67` — *"**30C scope:** Are caravan/wildlife autonomy phases to be included in the same package or a separate one?"*; context at `:41-43` — *"These are the '30C' phases and were not evaluated in this pass. They represent significant independent work and may overlap with plan authority already claimed elsewhere."*
- **Recorded lean** (`:80`): *"Promote 30B full consequence reach and 30C to a subsequent package"* — i.e. separate.
- **Type:** pure scheduling call.
- **Sign-off line:** `D7 30C scope: [same package as 30B / separate later package / not built].`

### D14 — Plan 123 §4.1 hostile-fire emitter (*found by sweep; not previously in any packet*)

- **Evidence:** `docs/combat/PLAN_123_SOUND_RANGING_AUTHORITY_MAP.md:9` — *"Engine defines HostileFireObservation input; emitter wiring = foreman decision (master map §4.1). Default: engine owns input type, Phase 7 wires producer."*; `docs/combat/PLAN_123_SOUND_RANGING_CLOSEOUT.md:53` — *"**No per-strike hostile-fire emitter exists upstream** (plan §24 premise drift): FactionWarSystem counts aggregate strikes only … wiring a producer (extending FactionWar) is a separate foreman-approved workstream."*; also `docs/PLANS_122_125_LATE_TECH_MOBILITY_CLOSEOUT.md:71,81`, `SESSION_HANDOFF.md:53`, `docs/PLANS_122_125_SECOND_TOOL_REVIEW.md:51`.
- **Verified state:** the **consumer is fully built** — `Assets/Ashfall.Core/Combat/SoundRangingThreatEngine.cs:83` (`HostileFireObservation`), `:217` (intake), save section registered at `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:147` (`sound_ranging`), RNG stream at `Assets/Ashfall.Core/Random/CampaignRngStream.cs:35`. The **producer is not**: `FactionWarSystem.cs:201-202` only increments an aggregate counter and invokes the event.
- **Blocks:** Plan 123 in-play reachability — the sound-ranging panel currently receives observations **only from the test harness**.
- **Options:** extend `FactionWarSystem` to emit per-strike observations (the map's stated default), or leave the engine harness-fed.
- **Coupling:** sign with **D6** — the recommended 30B proof-of-concept is `OnTerritorialClashOccurred → radio`, and the same event is Plan 123's desired producer. One edit serves both.
- **Blast radius:** `FactionWarSystem.cs`, the sound-ranging host session, `src/Main.Plans122to125.cs`, the `sound_ranging` save section, plus a map threat-marker type if projection is wanted.
- **Sign-off line:** `D14 Plan 123 emitter: [extend FactionWarSystem as the per-strike producer / leave harness-fed].`

---

## 3. The Plan 32 / map cluster

### D9 — 32A orphan map nodes (**verified; cheapest real content win in this packet**)

- **Status:** `CONSOLIDATION-PROPOSAL` / `DATA-GAP`
- **Evidence:** `docs/plans/wave11_part1/B2_PLAN32_IMPLEMENTATION_LOG.md:66` — *"These are navigable map nodes the player can travel to or through, but the content authority has no canonical location record for them … Adding minimal stub records to locations.json is a bounded data-hygiene task."*; `WAVE11_PART1_CLOSEOUT.md:69`; `WORKTREE_OWNERSHIP.md:11`.
- **Independently verified:** all **10** node IDs return zero matches in `Assets/StreamingAssets/Data/locations.json`. Three spot-checked as real graph nodes with route edges in `wasteland_map_v1.json`: `loc_municipal_seed_vault` (node `:145`, edges `:468/:476/:486/:494`), `loc_sealed_triage_annex` (node `:189`, edges `:594/:602/:612/:620`), `loc_electrical_maintenance_exchange` (node `:244`, edges `:774/:782/:792/:800`). Full list at `B2_PLAN32_IMPLEMENTATION_LOG.md:55-64`.
- **Blocks:** `C2[11]` sub-scope §2.4 (graph integrity) — `PARTIALLY-SATISFIED`.
- **Options:** **(i)** author 10 minimal `locations.json` stubs · **(ii)** extend `WastelandMapCatalogLoader` to cross-validate node IDs against the location catalog and fail loud · **(iii)** remove the 10 nodes from the map. The log proposes **(i)+(ii)** together (`:71-74`).
- **Blast radius if (i)+(ii):** `locations.json` (+10 records), `WastelandMapCatalogLoader`, `CatalogIntegrityValidator`, `ContentUtilizationScanner`, `WastelandMapTests` (68/68 baseline).
- **Type:** data + small validator code.
- **Sign-off line:** `D9 orphan map nodes: I choose [(i) stubs / (ii) loader gate / (i)+(ii) both / (iii) remove nodes].`

### D8 — 32B graph-native travel (**largest blast radius in the packet — do not sign casually**)

- **Status:** `PROMOTED — Requires Full-Scope Foreman Decision` / `ARCHITECTURE-GAP`
- **Evidence:** `B2_PLAN32_IMPLEMENTATION_LOG.md:79`; `WAVE11_PART1_CLOSEOUT.md:111` — *"What is the bounded 32B migration plan for graph-native expedition travel? Blast radius includes: ExpeditionSystem, AviationSystem, NavalSystem, CaravanSystem, and all route/save paths."*; also `:70` and `:58`.
- **Gap verified:** `grep WastelandMapSystem` → no results in `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`; `grep DistanceKm` → no results in the same file; `grep WastelandMapSystem` → no results in `Economy/CaravanTradeNetworkSystem.cs` (recorded at `B2_PLAN32_IMPLEMENTATION_LOG.md:47-52`).
- **Blocks:** `C2[11]` sub-scopes §2.5, §2.6, §2.7 — all `NOT SATISFIED`. Also gates **D10** (32C), which *"Must follow 32B"* (`:88`).
- **Options as recorded:** full-scope migration of all four systems at once, vs a bounded first slice. The log names the smallest coherent step: `ExpeditionSystem.Estimate` consuming `WastelandMapSystem` route distance (`:49`).
- **⚠ Composition risk that must be specified in the signature:** `ExpeditionState` already carries **two** speed multipliers — `survivorSpeedMultiplier` (added by C2-D3, `docs/plans/wave8_part2/C2_DECISION.md:36-40`) and `weatherSpeedMultiplier` (Plan 20C, claim `claim-c2-plan20c-consumers-2026-09-15`). A graph-derived travel factor becomes a **third** on the same seam, so the multiplication/clamp order has to be named explicitly or it will be invented at implementation time.
- **Blast radius:** `ExpeditionSystem`, `AviationSystem`, `NavalSystem`, `CaravanSystem`/`CaravanTradeNetworkSystem`, all route + save paths.
- **Sign-off line:** `D8 32B graph-native travel: scope is [full four-system migration / bounded first slice: ExpeditionSystem.Estimate only / not built]; multiplier composition order is [specify].`

### D10 — 32C geographic knowledge gating

- **Status:** `PROMOTED`, dependency-gated on D8
- **Evidence:** `B2_PLAN32_IMPLEMENTATION_LOG.md:85-88` — *"Gate ExpeditionSystem dispatch on WastelandMapSystem.GetFogState for target node; Expose discovery state to expedition launch UI (precision gating); **Must follow 32B**"*; `WAVE11_PART1_CLOSEOUT.md:59`.
- **Blocks:** `C2[11]` §2.8 (`NOT SATISFIED`) and §2.9 (`HOST-PARTIAL` — `WastelandMapView.cs` does consult fog state, but `ExpeditionSystem` launch does not gate on knowledge).
- **Sign-off line:** `D10 32C knowledge gating: [approved, after 32B / declined].`

### D16 — Flooded-route topology tags (**premise NOT independently verified**)

- **Evidence:** `docs/PLANS_122_125_LATE_TECH_MOBILITY_CLOSEOUT.md:72,81`; `docs/expeditions/PLAN_125_AMPHIBIOUS_DRAISINE_CLOSEOUT.md:60`; `SESSION_HANDOFF.md:54`.
- **Caveat:** the current absence of flooded/amphibious tags in `wasteland_map_v1.json` was **not verified**. Re-audit before signing.
- **Blocks:** Plan 125 in-play reachability — the draisine capability exists but no route exercises it.
- **Batching:** overlaps **D9** (same file, same map owner) — one data tranche can serve both.
- **Sign-off line:** `D16 flooded-route tags: [author them / re-audit first / declined].`

---

## 4. Self-contradictory item — needs an explicit tie-break

### D3 — `water_sample_contaminated` equipability (**three documents disagree**)

- **The contradiction:**
  1. `docs/decisions/WATER_SAMPLE_CONTAMINATED_DECISION_MEMO.md:6` — *"**Status:** AWAITING FOREMAN SIGNATURE (Preserving Option B pending review)"*; `:73` — *"**Recommendation:** Retain Option B (or sign Option A in a dedicated save-migration wave)."*
  2. `docs/plans/WAVE10_MICRO_DEFERRAL_SWEEP.md:80` — **the opposite**: *"Recommendation: Strip isEquipable: true and radProtection: 20 to avoid misleading inventory filtering."*
  3. `docs/governance/DECISION_REGISTER.md:23` (`DEC-09`, `DEFERRED-WITH-CONDITION`) — sides with the memo: *"Retained as intentional wasteland lore quirk … do not strip without catalog review."*
- **Verified data** (the docs' "line 3426" refers to one field, not the row): `"id"` at `Assets/StreamingAssets/Data/items.json:3411`; `"radProtection": 20` at `:3417`; `"isEquipable": true` at `:3426`; `"equipSlot": "Body"` at `:3427`.
- **The sweep's premise is imprecise:** it claims no gameplay system consumes this equipability. The memo records a live consumer at `:40` — *"Inventory.DegradeEquippedGear: Degrades if worn in the Body slot during radiation exposure."* So the degrade path **does** consume it.
- **Options** (memo §5, `:44-56`): **A** — correct classification (`isEquipable:false`, `equipSlot:"None"`, `radProtection:0`, `durability:0`, `degradeRate:0`). **B** — intentionally retain, reinterpreted diegetically as a *"Radiation Dosimeter / Chemical Indicator Flask"*.
- **Cost of A, as recorded** (`:52`, `:64-66`): *"Any existing save file where a survivor has water_sample_contaminated in their EquippedGear body slot would need migration handling on load"*, and the equipable-protective-body-item count drops 6 → 5 in `Ashfall.Core.Tests/Inventory/Plan21ProtectiveWearTests.cs`.
- **Blast radius:** A = 5 fields on 1 row + one test count pin + a save-load unequip/migration path. B = zero code, **but** the reinterpreted lore should then be authored into the description for consistency.
- **Interaction:** same `items.json` region as **D12**.
- **Sign-off line:** `D3 water_sample_contaminated: I choose [Option A strip / Option B retain as lore], overriding the conflicting record in [name the document].`

---

## 5. Schema / architecture decisions

### D12 — Amputation equipment restriction (signed schema package)

- **Status:** `SPLIT-SEALED` (avatar + expedition halves sealed; **equipment half BLOCKED**)
- **Evidence:** `KNOWN_DEBT.md:28` (`DEBT-AMPUTATION-EQUIPMENT-RESTRICTION`) — *"**Equipment half BLOCKED → separate package:** ItemDefinition/EquipSlot has no handedness/limb-requirement field, so an arm-state equip restriction needs a schema redesign (C2 §2 forbids doing it inside C2)."*; promotion condition: *"Equipment: signed schema package. **Never add presentation-only limb illusions**"*; `docs/plans/wave8_part2/C2_DECISION.md:19,21-25,59`; `WAVE10_MICRO_DEFERRAL_SWEEP.md:90-92`; `DECISION_REGISTER.md:17` (`DEC-03`, execution package *"Wave 11 Equipment Schema Tranche"*); `docs/gaps/PARTIAL_PLANS_VERIFIED_AUDIT.md:41` (`P9`).
- **Interim truth, stated plainly** (`C2_DECISION.md:27`): *"amputation still does not restrict gear. No presentation-only limb illusion is used as a substitute."* Declining this decision makes that permanent.
- **Note:** a real signature exists for the expedition half — `C2_DECISION.md:32` records `**Foreman signature received:** "I approve C2-D3"`. Only the equipment half is open.
- **Blast radius if signed:** `Assets/Ashfall.Core/Inventory/ItemDefinitions.cs`, the `EquipSlot` enum/model, `Inventory.cs` equip preflight, **every 2-handed-weapon and every boot row in `items.json`** (new field or documented default), `AmputationSystem`, `ExpeditionPanel` + inventory equip affordances, `CatalogIntegrityValidator`, `Plan21ProtectiveWearTests`, `C2AmputationTravelTests`. Second-largest radius after D8.
- **Sign-off line:** `D12 amputation equipment schema: [signed — author the handedness/limb-requirement package / declined — amputation permanently does not restrict gear].`

### D19b — Radio triangulation station identity

- **Evidence:** `docs/ui/PANEL_AUTHORITY_OWNERSHIP.md:66-70` — *"**stationId default "station_alpha"** on RadioHostSession.RecordObservation — every player-recorded triangulation observation currently carries this synthetic station identity because the panel has no station selection. A station-selection seam is a design change, not a Plan 16 repair."*; corroborated `docs/plans/C1_planintegration[2].md:1273`.
- **Type:** design/schema — observation records carry a station identity, so persisted records are affected.
- **Blast radius:** `RadioHostSession`, the triangulation panel, persisted observation records.
- **Sign-off line:** `D19b station identity: [author a station-selection seam / keep synthetic station_alpha / re-audit].`

### D20 — C3 endgame HOLD rows (four separate "name the owner" signatures)

- **Status:** the disposition itself is `SIGNED` (`0 PROMOTE · 1 RETIRE · 4 HOLD`); each HOLD names a precondition requiring a **further** signature.
- **Evidence:** `docs/plans/wave8_part2/C3_DECISION.md` (five-row table); `KNOWN_DEBT.md:24` (`DEBT-PLANS170-199-PORTFOLIO`, `ACCEPTED`, owner **Foreman**); recheck conditions verbatim at `docs/plans/wave8_part2/C3_HANDOFF.md:16-24`; `DECISION_REGISTER.md:22` (`DEC-14`) and `:18` (`DEC-04`); corroborating `docs/world/PLAN_192_199_ROUTES_MIGRATION_AUTHORITY_MAP.md:44` and `docs/foreman/PLAN_170_199_REMAINING_FAMILY_MAPS.md:20` (*"none until product decision"*).
- **The four recheck conditions, verbatim:**

| Plan | Condition to lift the HOLD | Type |
|---|---|---|
| **174** | *"a signed extension point on SurvivorEnrichmentService/TradeSpecialtySystem (not a new BackstorySystem) has a consumed gameplay surface"* | architecture-owner naming |
| **175** | *"a signed cross-run profile-store owner (versioned/checksummed, outside campaign slots) exists and Plan 34/149 completion-fact producers are verified"* | **product call** (Meta Profile / NG+) |
| **192** | *"a player-route DTO with standing/raid/save seams is signed in a map amendment"* | schema — must **not** touch NPC owners (`CaravanTradeRouteCatalog` / `CaravanTradeNetworkSystem` / `TravelingCaravanSystem`) |
| **199** | *"product names a human population owner distinct from fauna"* | **product call** (seasonal human/faction migration) |

- **Note:** 191 is `RETIRED` and closed. 181/193/194 are explicitly out of C3 scope (`C3_HANDOFF.md:26-28`).
- **Sign-off line:** `D20: 174 [lift/hold] · 175 [lift/hold] · 192 [lift/hold] · 199 [lift/hold].`

---

## 6. Pure policy calls — one line each

### D4 — Ratify DEC-15 (radio weather authority)

Substance already settled and matching the sweep's own recommendation
(`WAVE10_MICRO_DEFERRAL_SWEEP.md:77` — *"Retain the weather station as the sole forecast authority"*;
`DECISION_REGISTER.md:29` `DEC-15` = `SIGNED`; verified again at
`docs/plans/wave10_part2/B4_PLAN33_INTEL_VALUE_LOG.md:35-37`). What is missing is a **user**
signature — the register was authored by the integrator under Wave 10 Part 2 Task E1
(`WAVE10_PART2_CLOSEOUT.md:30`). Blocks nothing operationally, but the register's own invariant
(`DECISION_REGISTER.md:7` — *"Zero items may remain unsigned without a condition"*) is false until ratified.
- **Sign-off line:** `D4: I ratify DEC-15 (WeatherStationSystem is the sole forecast authority; radio weather stays diegetic).`

### D11 — Plan 31 semantic-kind briefing re-grouping (**the only item with no recorded recommendation**)

- **Evidence:** `WAVE9_PART1_CLOSEOUT.md:18` — *"**SEMANTIC AUTHORITY COMPLETE — CONSUMER WIRING BLOCKED ON CONTRACT DECISION** … the briefing consumer is not wired because the C2/Plan 17 no-silent-drop contract pins GenericSectionTitle ("System Activity") as the section for every unhandled kind (DayEventVocabularyTests), and B1 §6.12 forbids an unrelated briefing rewrite."*; `WORKTREE_OWNERSHIP.md:25`; `INTEGRATION_PLANS.md:41-43`.
- **Scope note:** the *authority* side is sealed — `docs/plans/wave10_part2/B3_PLAN31_RECONCILIATION.md:31-32` marks 31A.1/31A.2 `SEALED`, and claim `claim-wave10-part1-c1-plan31-2026-09-17` is `DONE` for 31B routes + 31C day records. **Only the section-grouping rewrite remains.**
- **Options:** **(i)** keep `GenericSectionTitle` pinned for unhandled kinds, leave the briefing flat (status quo, zero cost) · **(ii)** authorise re-grouping the 10 `SemanticKind` categories onto named briefing sections, which requires **amending the pinned no-silent-drop contract** in `DayEventVocabularyTests`.
- **Blast radius if (ii):** `DailyBriefingReportBuilder.cs`, `DayEventVocabulary.cs`, `DayEventVocabularyTests.cs` (8 cases — the pin), `DailyBriefingReportBuilderTests.cs` (13 cases), `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`, parity gate `DayEventParitySourceGateTests`.
- **Sign-off line:** `D11 semantic-kind briefings: [(i) keep the flat pinned contract / (ii) authorise re-grouping and amend the pin].`

### D13 — Plans 126–129 header / plan-numbering authority

- **Evidence:** `docs/plans/wave8_part2/D1_HANDOFF.md:34` — *"AMBIGUOUS — needs a foreman numbering/authority decision before any header or plan-status edit"*; reasoning at `:21-25`; `INTEGRATION_PLANS.md` (*"recorded, not guessed"*); `WORKTREE_OWNERSHIP.md` (claim `claim-wave8-part2-d1-verification-truth-2026-09-17`).
- **Verified:** the header is live at `src/Main.Plans126_129.cs:3-6` (*"Plans 127-129 land in follow-up waves … (tethered recon drone, continuous steel casting, lidar)"*). A grep for `ReconDrone|SteelCasting|ContinuousCasting|Lidar|TetheredDrone` across `Assets/Ashfall.Core/**/*.cs` returns **zero** relevant hits — no implementation exists.
- **Contradiction to adjudicate:** an earlier audit reached the opposite conclusion — `docs/gaps/PARTIAL_PLANS_VERIFIED_AUDIT.md:36` (row S2) says *"Stale header comment; correct the comment, do not rebuild."* That document self-declares `SUPERSEDED` at `:3` and predates D1, but **D1 declined to act rather than overturning S2 — both readings are still on the record.**
- **Options:** **(a)** rule it numbering drift → retarget the header comment to the real 127/128/129 subjects and close · **(b)** rule the drone/caster/lidar work genuinely un-built → open a scoped plan · **(c)** leave as-is permanently.
- **Sign-off line:** `D13 Plans 126-129: [(a) numbering drift, fix the comment / (b) genuinely un-built, open a plan / (c) leave as-is].`

### D15 — SOFC inventory-fuel binding (**a live placeholder: the SOFC runs on free fuel**)

- **Evidence:** `SESSION_HANDOFF.md:50` and `:44` — *"**Highest-value surfaced follow-up: SOFC FuelConsumer placeholder** (FuelConsumer = units => true — the SOFC consumes no inventory fuel until the inventory owner binds the real check)."*; `docs/PLANS_122_125_LATE_TECH_MOBILITY_CLOSEOUT.md:73,80`; `docs/PLANS_122_125_SECOND_TOOL_REVIEW.md:51`.
- **Verified:** `src/Main.Plans122to125.cs:51` — `FuelConsumer = units => true, // inventory owner binds the real check in Phase 9`. The seam exists at `src/Host/SofcPowerHostSession.cs:22` (`public Func<float, bool>? FuelConsumer { get; set; }`), consumed at `:91` and `:111-112`. Test/CLI stubs at `src/Host/HostCli.Plans122to125.cs:55,479,725`.
- **Options:** inventory is the fuel owner, or grid units are. Then *"a small wiring package"*.
- **Blast radius:** genuinely small — one binding at `Main.Plans122to125.cs:51`, `SofcPowerHostSession`, and whichever owner is named. **The seam already exists.**
- **Sign-off line:** `D15 SOFC fuel owner: [inventory / grid units].`

### D19a — Barter constructible before a campaign exists?

- **Evidence:** `docs/ui/PANEL_AUTHORITY_OWNERSHIP.md:62-65` — *"new SeededRng(147) when _campaignDay is null (src/Main.Plans147.cs). In-game the campaign stream is always used; the fallback only fires pre-campaign. Whether barter should be constructible before a campaign exists is a product decision."*; corroborated `docs/plans/C1_planintegration[2].md:1271`.
- **Blast radius:** `src/Main.Plans147.cs` only. Pure policy.
- **Sign-off line:** `D19a pre-campaign barter: [allowed / must not be constructible].`

### D21 — Test-quarantine drain authorisation

- **Evidence:** `KNOWN_DEBT.md:9` (`DEBT-TEST-QUARANTINE-2026-09-12`, owner **Foreman**) — *"51 remaining csproj Compile Remove entries (Twin tree absent; sources recoverable from git) … Next catalog-shaped candidate: AutopsyProcedures."*; promotion condition *"Per-file: current API/content contract, source returns to project, focused target passes"*; policy at `TEST_POLICY.md:39-44` (*"A green compile alone is not re-enable evidence."*).
- **Ledger is stale on both counts:** AutopsyProcedures is already `RETIRED`/sealed (`KNOWN_DEBT.md:15`, sealed 2026-09-17), and the real count is **50 active** `Compile Remove` entries (56 matches minus 6 commented out at csproj lines 111, 113, 143, 151, 154, 156).
- **Additional verified fact:** the `Twin_ASHFall/` archive the row points at **does not exist** — recovery is git-only, from `1d216b98^`.
- **Sign-off line:** `D21 quarantine drain: [authorise a per-file batch / not worth it / correct the ledger row only].`

### D22 — String freeze (gates both VO and localization)

- **Evidence:** `DECISION_REGISTER.md:25` (`DEC-11`) — *"Full VO audio generation deferred until dialogue string freeze and audio bus loudness calibration … recheck: Campaign dialogue text freeze"*; `:27` (`DEC-13`) — *"Multi-language translation catalog deferred until UI string extraction and string freeze in Wave 12 … recheck: UI string extraction pass"*.
- **What is actually being asked:** these are sequencing gates, not open questions — but both recheck triggers depend on a **string freeze only you can declare**. Without it, VO and localization stay parked indefinitely.
- **Sign-off line:** `D22 string freeze: [declared, scope+date / deferred again].`

### D23 — Schedule or drop three promoted-but-unscheduled packages

| Item | Status + evidence |
|---|---|
| Plan 28 main-constructor migration | `PROMOTED-TO-QUEUE`, package `QUEUE-PLAN28-MAIN-CONSTRUCTOR-MIGRATION`, target `src/Main.*.cs` setup methods, *"Staged migration of Setup* calls into descriptor SetupAction delegates, gated by MainTriadDriftGateTests"* — `docs/plans/wave10_part2/C2_PLAN28_ORCHESTRATION_SPINE.md:5,38,42-49`; `WAVE10_PART2_CLOSEOUT.md:33` |
| Plan 31B subject-focus queries + snapshots | `DECIDED-DEFERRED (Queued with Plan 31B UI remainder)` — `WAVE10_MICRO_DEFERRAL_SWEEP.md:42,61`; `WORKTREE_OWNERSHIP.md` claim `claim-wave10-part1-c1-plan31-2026-09-17` |
| Plan 24 snapshot rebaseline | `ENVIRONMENT-BLOCKED (documented)` — *"Headless renderer unavailable (SubViewport needs a real display) … the snapshot gate remains environment-dependent and must run on a renderer-capable session."* Intended render changes enumerated at `docs/plans/PLAN_24_CLOSEOUT.md:32-42`; row 12 at `:31`, note at `:44-45` |

- **Sign-off line:** `D23: Plan 28 migration [schedule/drop] · 31B remainder [schedule/drop] · snapshot rebaseline [schedule/drop].`

---

## 7. Items with UNVERIFIED premises — re-audit before signing

These appear in ledgers as follow-ups but have **no recorded options** and were **not verified
against current source**. They are listed so they are not lost, not so they can be signed.

### D17 — Diamond → `ExcavationSystem` downtime accounting
`docs/PLANS_122_125_LATE_TECH_MOBILITY_CLOSEOUT.md:83` (*"5. Diamond → ExcavationSystem downtime accounting at the condition sink."*); `docs/PLANS_122_125_SECOND_TOOL_REVIEW.md:51`. No options recorded anywhere; premise unverified. **Needs a re-audit before it belongs in a signature packet.**

### D18 — `WasteHeatTargetRoomProvider` kitchen policy
`docs/PLANS_122_125_LATE_TECH_MOBILITY_CLOSEOUT.md:74` (*"placeholder pending the thermal-coordinator allocator"*). No options recorded; premise unverified. Likely code + a small policy call (which room receives waste heat).

### D19c — Long-lived shared instances across new-game (**possibly a live save-contamination bug**)
`docs/ui/PANEL_AUTHORITY_OWNERSHIP.md:71-77` — *"_silentFoundry / _sharedFactionStance / _sharedSkillProgression are not nulled by any lifecycle participant; they survive session swaps and have their state restored through their save sections (load path). Whether new-game (as opposed to load) fully resets their state should be verified by the owning foundry/apprenticeship packages before Plan 16 claims 16B.9 closed for them."* The document explicitly does **not** claim it is a bug. **Worth verifying independently of any signature** — if new-game does not reset them, that is a real defect, not a decision.

---

## 8. Governance hygiene — these make the packet itself untrustworthy

Not decisions. They are reasons the ledgers currently disagree with each other, which matters because
you are being asked to sign against them.

### G1 — Four Wave 11 decisions were never added to `KNOWN_DEBT.md`
`docs/plans/wave11_part1/WAVE11_PART1_CLOSEOUT.md:63-70` says *"Per audit, the following items should be recorded in KNOWN_DEBT.md by the foreman/integrator"* and lists 4 (C2[10] runtime clock, C2[10] 30B, C2[11] orphans, C2[11] 32B). `KNOWN_DEBT.md` was read in full: **none of the four are present.** The closeout's own close condition (`:114` — *"KNOWN_DEBT entries added by integrator for C2[10] and C2[11] remainder"*) is **unmet**. → These are D5/D6/D9/D8.

### G2 — `DECISION_REGISTER.md` predates Wave 11 and contains none of it
Last updated 2026-09-17 (`:5`); Wave 11 Part 1 closed 2026-09-18 (`WAVE11_PART1_CLOSEOUT.md:5`). Highest ID is `DEC-19`. D5–D10 have no register entry, so the register's invariant (`:7` — *"Zero items may remain unsigned without a condition"*) is **currently false**. Wave 11 Part 2 Task C1 exists precisely to fix this — `Seal-steps/7227502_ASHFALL_WAVE11_IMPLEMENTATION_UNBLOCKER_PLAN_PART2.md:9` (*"C1 — Standing Decision Register recurrence"*), `:805-807`, `:826-841` — and is **still unexecuted** (it sits in `Seal-steps/`, not `Seal-steps/Completed/`). This packet is the input that task needs.

### G3 — Four Wave 9 Part 2 memos have blank signature blocks but are recorded downstream as SIGNED
`docs/plans/wave9_part2/C1_DECISION.md:84`, `C2_DECISION.md:60`, `C3_DECISION.md:57`, `D2_DECISION.md:60` all read `[PENDING FOREMAN DECISION]`. Yet `DECISION_REGISTER.md:19/20/21/24` marks DEC-05/06/07/08 `SIGNED`, `INTEGRATION_PLANS.md` records *"Availability consumer — RETIRED (Wave 9 Part 2, Option B Approved)"*, and `docs/balance/BALANCE_SIM_radiation_exposure_20A.md:110-118` records *"All findings (F1–F8) are formally closed per signed foreman authorization."* DEC-08's work was **verified as actually landed** (`git ls-files` → 0 tracked files under `.qwen/`/`.codex/`/`.cursor/`, 36 under `.agents/`; `.gitignore:214-217`). **Clerical — back-fill or annotate the memo lines; do not re-decide.**

### G4 — 🔴 `AGENTS.md` / `QWEN.md` point every agent at work that is already done
Both carry `## ACTIVE HANDOFF — AGY (Antigravity): C1 UI PANEL WAVE`, instructing agents to
*"generate/extend the `economy_detail` and `traveling_caravan` panels."* Verified complete:
`src/UI/EconomyDetailPanel.cs` and `src/UI/TravelingCaravanPanel.cs` both exist (with `.cs.uid`
sidecars) and both are registered — `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs:26`
(`R("economy_detail", "Economy Detail", PanelGroup.Dashboard, new[] { "economy" })`) and `:105`
(`R("traveling_caravan", "Traveling Caravan", PanelGroup.Expanded)`).
**This is the first file every agent reads, and it is actively misdirecting them into finished work.**
Fixing it requires editing canonical `AGENTS.md`, regenerating the 13 client rulebooks
(`sync-agent-rulebooks.py` / the `ashfall-agents` skill), and re-running `AgentRuleIntegrityTests`
(which `KNOWN_DEBT.md` records as asserting rulebook invariants).

### G5 — `POTENTIALCLUTTER.md` Section H is part-executed; one part violates Rule 1
Status verified against `POTENTIALCLUTTER.md:696`:
- H1.a gitignore AI workspaces — **done** for `.qwen/.codex/.cursor/.mistral` (`.gitignore:214-217`); `.kiro/` and `.zcode/` **not** present.
- **H1.b `.gitattributes` Unity rewrite — NOT done.** `.gitattributes:11-12` still define `[attr]unity-json` / `[attr]unity-yaml`, and `:26-49` still apply them to `*.asmdef`, `*.anim`, `*.asset`, `*.mat`, etc. **This is a live contradiction of AGENTS.md Rule 1 ("Godot is authoritative; Unity is retired").**
- H1.c `.qwen/skills` reconcile — **done** by untracking (see G3).
- H1.d SPDX — **done** (`KNOWN_DEBT.md` `DEBT-WORKTREE-DECLUTTER-2026-09-12`: *"SPDX … added to 1,867 C# files"*).
- H2.b 111 orphan `.meta` — **done** (`git ls-files | grep -c '\.meta$'` → 0).
- `POTENTIALCLUTTER.md:725` — *"Untracked addons/ziva_agent/ is a real anomaly but a foreman-level decision is needed before any action."* — **still unresolved**; the directory exists.
- `POTENTIALCLUTTER.md:727-728` — `BUG_HUNT_25..29` and `PLANS_*_FORENSIC_REPORT` in `docs/forensics/` have zero references in governance or code; archive decision **still unresolved**.

### G6 — Stale items verified CLOSED (listed so they are not re-litigated)
- `docs/systems/CONDITION_LEDGER_OWNERSHIP.md:48-54` ("D2 (foreman decision)" on equipped-item restore-repair) — **closed by implementation**: `Assets/Ashfall.Core/Inventory/Inventory.cs:983` `public bool TryRepairEquippedGear(EquippedItem item)`, shipped under Plan 22 (claim `claim-c2-plan22-consumables-2026-09-15`). The doc was never updated.
- `docs/forensics/SHELTER_CASCADE_SEAMS_FORENSIC_REPORT.md:267-268` (un-quarantining `PowerGridCatalogTests` needs a room-count decision) — **resolved**: `PowerGridCatalogTests` does not appear in the csproj `Compile Remove` list, i.e. it is compiled and running.
- `docs/foreman/PLANS_210_213_FLAGSHIP_ECONOMY_AUTHORITY_MAP.md:187` (`## 4. DECISIONS PENDING FOREMAN APPROVAL`, D1–D7) — **approved**: the same file's header at `:4` reads `**Status:** ACCEPTED — §4 defaults approved by foreman 2026-09-13 (D1–D7 as proposed)`. The section heading is stale.
- `docs/gaps/PARTIAL_PLANS_VERIFIED_AUDIT.md` §1 rows P1–P8 (all `UNWIRED`/`UNSTARTED`) — **all sealed**, each mapping to a `RETIRED` row in `KNOWN_DEBT.md` (DEBT-189, -176, -177, -178, -182, -188, -194, -198). The doc self-declares `SUPERSEDED` at `:3`.
- `docs/debug/10LOOP_UNWIRED_CODE_AUDIT.md:81` (*"These are missing features, not defects"*) — **premise has drifted.** The audit is dated 2026-09-02 at HEAD `7738facc` (`:12`); all four loader/system pairs now have host-session references: `src/Host/WorldHostSession.cs:33` (`AtmosphereTextSystem`), `:36` (`EnvironmentalTextSystem`), `src/Host/ExpansionHostSession.cs:44`/`:197` (`DebtTemplateCatalog` + loader). Whether they have *downstream consumers* was **not** verified — if the "content surface venue" question is still live it needs a fresh audit, not a signature.
- `docs/plans/C2_PLANINTEGRATION_2_CLOSURE_REPORT.md:133-145` — items 2 and 3 (17C Phase E sweep, 17B deep matrix) are closed by `WAVE9_PART1_CLOSEOUT.md` task B2 (*"CLOSED (Scope Map Published)"*, *"17B Matrix: MATRIX COMPLETE via Plan17BRouteVisibilityMatrixTests (8/8)"*). Item 1 (Plan 31) is superseded by Wave 10 Part 1 C1 + Wave 10 Part 2 B3, leaving only **D11**. Item 4 (`TravelingCaravanHostSession CS7036`) was **not verified**.

---

## 9. Suggested signing order (maximum unblock per signature)

1. **D5 + D6 + D7 + D14** together — one Plan 30 / FactionWar producer session. D6 and D14 both want
   `OnTerritorialClashOccurred`, so signing them together avoids editing `FactionWarSystem` twice.
2. **D1 + D2** together — both Plan 24, both medical. These two signatures are literally what flips
   Plan 24 from `CLOSED-WITH-DEFERRALS` to `CLOSED` (`PLAN_24_CLOSEOUT.md:3`).
3. **D3** alone — it contradicts itself across three documents and touches the same `items.json`
   region as D12.
4. **D9 + D16** as one map-data tranche — cheapest real content win; D9 is fully verified, D16 needs a
   premise re-check first.
5. **D8** only after it is bounded — **do not sign it in the same breath as D9.** Largest blast radius
   in the packet, and the multiplier composition order must be named in the signature.
6. **D4, D11, D13, D15, D19a, D21, D22, D23** — the one-liners.
7. **G1 + G2 + G4** — have the integrator record the four missing Wave 11 debt rows, refresh the
   `AGENTS.md`/`QWEN.md` handoff, and run Wave 11 Part 2 Task C1
   (`Seal-steps/7227502_…PART2.md:803`) so the register is truthful **before the next packet**.

---

## 10. What this packet does not claim

- No production code, data, or test was modified to produce it.
- Items marked **unverified** (D16, D17, D18, D19c, and `C2_PLANINTEGRATION_2_CLOSURE_REPORT.md:145`
  item 4) must be re-audited before a signature is acted on.
- Every other claim above carries a `file:line` citation and was checked against current source, per
  AGENTS.md Rule 7 — including three widely-repeated claims that turned out to be false (§0).
