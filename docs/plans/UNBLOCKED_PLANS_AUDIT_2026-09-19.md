# ASHFALL — Unblocked Plans Audit (2026-09-19)

**Auditor role:** read-only queue audit requested by the foreman (user).
**Repo state audited:** branch `Zcode_Branch`, HEAD `fc73a306` (2026-09-19 02:27)
plus the current uncommitted worktree (the 2026-09-18/19 completion-first
execution session). **Zero production change** — this audit adds this document
and the `AGENTS.md` queue handoff only; no `Assets/`, `src/`, data, or test
file was modified.

**Method:** re-ran the unblock classification of the live queue authorities
(`INTEGRATION_PLANS.md` current batch, the fifteen-plan completion-first
program, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, and the decision
register/packet) against **current source evidence**, per AGENTS.md Rule 7.
Focused tests were executed only to verify sealing claims (all green, listed
in §8).

---

## 1. Executive finding

**8 plans are available for integration right now** — unblocked, not yet
executed, and requiring no new foreman signature (2 of the 8 need only the
standard premise audit before the first edit, per the current-evidence rule).

The 2026-09-18 → 2026-09-19 completion-first execution wave landed **10 debt
seals**, completed **6 of the 15 completion-first roster plans**, and sealed
**all 5 PARTIALLY-SEALED census anchors** (C2[9]–C2[13]). The measured
corpus queue drops from **117 → 112 nonterminal rows** (111 `AUDIT-PENDING`
+ 1 `READY-UNCLAIMED`) once the anchors flip to `SEALED`.

## 2. What was unblocked *and executed* since the last certified queue state

Baseline: the Wave 11 Part 2 C2 census refresh (2026-09-18 17:51), which
certified **no Wave 12 head** because of outstanding foreman signatures and
unverified prerequisites. All of those blockers are now resolved in source:

| # | Sealed item | Current evidence (verified 2026-09-19) |
|---|---|---|
| 1 | `DEBT-PLAN24-MEDICAL-WARD-STAFFING` (D1, option b) | `duty_roles.json` `ward` role; `MedicalWardSystem.StaffingPreflight`; binding in `src/Main.Medical.cs`; scratch fixture seeds `ward` (`Ashfall.Core.Tests/IntegrityScratchFixture.cs:56`); closeout `docs/plans/PLAN_24_CLOSEOUT.md` = **CLOSED** (both signatures resolved; only the environment-blocked snapshot rebaseline remains) |
| 2 | `DEBT-PLAN28-MAIN-CONSTRUCTOR-MIGRATION` (D23 item 1) | `SubsystemDescriptor.SetupAction` + `ExecuteSubsystemManifestBootstrap` (`src/Main.Lifecycle.cs:542`), invoked from `RestoreAllSubsystemsFromDisk()` (`src/Main.SaveOrchestrator.cs:163`) |
| 3 | `DEBT-PLAN30-CONSEQUENCE-REACH` (D6) | `WireFactionWarConsequenceRouting()` in `src/Main.YearOfAsh.cs`: clash/decree → radio intercept + journal + sound-ranging; stage/chain events routed in the 2026-09-19 session |
| 4 | `DEBT-PLAN30-RUNTIME-CLOCK` (D5) | `FactionWarChainRunner.ToAuthoredDay` maps playable 180→authored 480 (offset 300); **`FactionWarClockTests` 1/1 PASS (re-run today)** |
| 5 | `DEBT-PLAN32-MAP-ORPHANS` (D9) | 10 authored `loc_*` stubs in `locations.json`; loader gate `AllMapNodes_ExistInLocationsCatalog` |
| 6 | `DEBT-PLAN32-GRAPH-TRAVEL` (D8 + D10) | Expeditions consume map distance + Unknown-fog dispatch refusal; caravans expand hops via `WastelandMapSystem.PlanRoute` (`Assets/Ashfall.Core/World/WastelandMapSystem.cs:581`) and wait on Unknown nodes; trade-network `travel_days` overlay; wildlife seed-graph overlay (**`WildlifeMapOverlayTests` 1/1 PASS (re-run today)**). Aviation/naval migration remains a documented residual, not a blocker |
| 7 | `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY` (34B/34C) | XP-01 catalog live + `SetupDifficulty` host bind; completion-history schema v2 stamps `difficultyPresetId` (`Assets/Ashfall.Core/Endgame/CampaignCompletionHistory.cs:44`); **`CampaignCompletionHistoryTests` 11/11 PASS (re-run today)** |
| 8 | `DEBT-PLAN36-PORT-CONTRACT-CLOSURE` (36C) | **`generate-port-contract.py --check` PASS (re-run today): 262 seams, 180 HOST_REQUIRED bound, 0 DEFERRED**; `BindCraftResultGate` bound via `CraftingHostSession.Create`; spiritual coordinator wired (DX-02 closed) |
| 9 | `DEBT-PLAN123-SOUND-RANGING-PRODUCER` (D14) | `FactionWarSystem` per-strike `HostileFireObservation` → `SoundRangingHostSession.RecordHostileFire` |
| 10 | `DEBT-PLAN125-SOFC-INVENTORY-FUEL` (D15) | `ConsumeSofcFuel` (clean→treated→dirty tiers, grid-reserve fallback) in `src/Main.Plans122to125.cs` |

Additional completed work verified outside the debt ledger:

- **Plan 26A tranche-2 (forbidden-path sweep) is COMPLETE**: the
  `CatalogPathForbiddenGateTests` allowlist now contains only the authority
  itself (`src/Host/CatalogPath.cs`) and the gate doc states "tranche-2
  complete"; **2/2 PASS (re-run today)**. Observation: two lowercase
  `res://assets/StreamingAssets/Data/...` references
  (`src/Audio/AudioCueCatalog.cs:407`,
  `src/Main.FlagshipInstitutions.cs:45`) escape the gate's ordinal
  `Assets/StreamingAssets/Data` pattern — the integrator should confirm
  whether these are live reads that still need `CatalogPath` routing.
- **XP-01 first slice + partial binding** shipped under the active batch
  `XP-WAVE1-DIFFICULTY-AUTHORITY` (catalog/director/provider, host setup,
  one consumer, completion-history tag — remainder is Available item 5 below).

### Completion-first roster scorecard (15 plans)

| Roster # | Package | Verdict |
|---|---|---|
| 01 | `CF-P24-CLOSURE` | **EXECUTED** (Plan 24 CLOSED; snapshot rebaseline environment-blocked, not claimable headlessly) |
| 06 | `CF-P30-WAR-PROJECTION-CONSUMERS` | **EXECUTED** (D5/D6/D7/D14 cluster) |
| 07 | `CF-P32-GRAPH-TRAVEL` | **EXECUTED** (expedition + caravan + trade network + fog gating; aviation/naval residual documented) |
| 08 | `CF-P34-DIFFICULTY-CHRONICLE` | **EXECUTED** |
| 09 | `CF-P36C-PORT-SEAM-SWEEP` | **EXECUTED** (0 DEFERRED) |
| 11 | `CF-P26A-FORBIDDEN-PATH-SWEEP` | **EXECUTED** (tranche-2 complete) |
| 02, 03, 04, 10, 13 | see §3 | **AVAILABLE** (5) |
| 05, 12, 14, 15 | see §4 | **DECISION-BLOCKED** (4) |

## 3. The 8 plans available for integration now

| # | Plan / package | Scope | Why it is unblocked (current evidence) | Entry gate |
|---|---|---|---|---|
| 1 | `CF-P1-DISTRESS-CONTENT-SEAL` (program Plan 02) | Validator rules (expired-with-no-consequence, trap-grammar-on-genuine, max-2 follow-ups), population replay through the real V6 codec, audio-cue registry verification, `--content-utilization-selftest` 0 orphans, PR3 closeout | Mechanism sealed since Waves 3–5; content is shipped (17+7 `follow_up_signals`, 66+36 `audio_cue` occurrences in the two catalogs); the three validator rules and the PR3 closeout do not exist yet (grep-verified) | none — verify-and-seal |
| 2 | `CF-P5-RESTOCK-RECONCILE` (program Plan 03) | Ledger truth only: reconcile the restock rows across `INTEGRATION_PLANS.md` / `KNOWN_DEBT.md` / register | Implementation is live in production: `ShelterBarterSystem.ComputeItemPriorityScore` (`Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs:283`) sorts restock stock (`:307-316`) with a `PriorityScorer` seam; **`Plan147RestockPriorityTests` 6/6 PASS (re-run today)**; DEC-05 is already `SIGNED` in the register | foreman ratification line (F2) is wording-only; ledger paths are integrator-owned |
| 3 | `CF-P6-VEHICLE-ARMOR-GRADES` (program Plan 04) | 4 vehicle armor grade tiers on the Plan 50 decoration seam | The D6 deferral condition ("no vehicle owner exists") is false — `VehicleGarageSystem` + decoration seam + `--vehicle-garage-selftest` 19/19 landed; no armor-grade implementation exists (grep-verified) | new claim + premise note |
| 4 | `CF-P28-ONE-BOOTSTRAP-PATH` (program Plan 10) | Run the subsystem manifest bootstrap on the fresh-game path too, so fresh and restore lifecycles agree | Sealed migration executes the bootstrap only on the restore path: sole call site `src/Main.SaveOrchestrator.cs:163`; `ComposeCampaign()` (`src/Main.CampaignServices.cs:25+`) never calls it (verified) — the two paths still diverge | none — bounded host change |
| 5 | `CF-XP01-DIFFICULTY-FULL-BINDING` (program Plan 13, active batch XP W1) | Preset selection at campaign creation, `difficulty_preset_id` persistence, remaining scalar consumers, host session/panel surface, per-consumer premise notes | Catalog/director live; host resolves **default preset only** (`src/Main.Difficulty.cs:28`, `ResolveProvider(null)`); exactly one consumer (`HostileEncounterMult`, `src/Main.EvolvingWorld.cs:193`); no difficulty save section (grep-verified); no panel/CLI surface (grep-verified) | per-consumer premise checks under the already-authorized W1 claim `claim-xp-wave1-difficulty-2026-09-18` |
| 6 | `E1/Plan 53` (census `READY-UNCLAIMED`) | Ambition governance programme E1A–E1P | Prerequisite Plan 29 (C1[7]) is SEALED; census row already classifies it READY | claim per census protocol |
| 7 | `C2[15]/Plan 37` — Input, Focus & Controller Reality | Input reality, focus navigation, controller parity, rebinding | Declared prerequisites C2[9]/Plan 28 (sealed 2026-09-18) and C1[8]/Plan 31 (sealed) are both resolved — the last dependency blocker is gone | standard premise audit first (per the C2 census refresh) |
| 8 | `C2[21]/Plan 48` — Release Craft | Versions, tags, changelog, artifact provenance, save-safe hotfixes | Declared prerequisite C1[7]/Plan 29 is sealed | standard premise audit first |

Counting rule (stated plainly): *available* = the plan's recorded blockers are
verifiably gone, it is not yet executed, and it needs no new foreman
product/schema/design signature. Items 7–8 satisfy every recorded blocker but
still require the repo-standard premise audit before the first edit, exactly
as the census refresh requires for any head selection.

## 4. Still blocked — do not start without the named signature

- `CF-P3-SEMANTIC-KIND-AUTHORITY` (roster 05) — **D11** (pinned
  `GenericSectionTitle` no-silent-drop contract in `DayEventVocabularyTests`).
- `CF-QD-QUARANTINE-DRAIN-TRANCHE` (roster 12) — **D21/F11**; 48 active
  `Compile Remove` exclusions remain (count verified).
- `CF-XP04-ECONOMY-LEGS` (roster 14 / XP-04) — **F13** design sign-off.
- `CF-XP06-BODY-INTEGRITY` (roster 15 / XP-06) — **F14** schema sign-off
  (DEC-03 successor); `ItemDefinition`/`EquipSlot` still has no
  handedness/limb-requirement field.
- **XP-08** (trade routes / seasonal migration) — hard-depends on XP-04 funds;
  **XP-07** — sequenced after XP-04; **XP-09/XP-10** — premise-check-first
  against `RETIRED` register rows DEC-17 (presenter skill tree) and DEC-18
  (phobia growth).
- **EN-01 … EN-08** — proposals; each needs one foreman authorization. Their
  gates are now closer (EN-02/EN-06/EN-07 prerequisites were sealed by this
  wave) but they remain unauthorized.
- **C1[16]/Plan 49** — `DECIDED-DEFERRED` pending C2[18]/Plan 42 and
  C2[20]/Plan 46 premise audits.
- **C3 HOLDs** 192/199 (D20) — recheck conditions unmet. **174/175 LIFTED & SEALED 2026-09-23** (this 2026-09-19 audit predates the lift; see `KNOWN_DEBT.md` / `DEC-313` / `DEC-314` / `DEC-317`).
- Remaining decision-packet items: D3 (water_sample_contaminated), D4 (ratify
  DEC-15), D13 (Plans 126–129 header — stale header verified still present at
  `src/Main.Plans126_129.cs:3-6`), D16 (flooded-route tags — premise now
  verified: **0** `flooded`/`amphibious` tags in `wasteland_map_v1.json`),
  D19a/D19b/D19c, D22 (string freeze), and the environment-blocked Plan 24
  snapshot rebaseline (renderer-capable session required).
- **109 census `AUDIT-PENDING` rows** — tranche-2 per-clause audits have not
  run; they are audit work, not certified integration plans.

## 5. Premise-consumed XP pillars (not double-counted)

- **XP-02** (GraphTravelPlanner/RouteTopologyModel/SurveyKnowledgeStore) —
  pillar files do not exist, but the capability landed through the canonical
  owner (`WastelandMapSystem.PlanRoute` + fog gating) under the one-authority
  rule. The pillar as authored is premise-consumed; any distinct remainder
  (survey knowledge read model) needs a fresh premise check, not a copy of the
  plan.
- **XP-03** (WorldClockHorizon/WarChain*Route) — war clock and
  radio/journal/sound-ranging routing landed; economy/chronicle routes and the
  severity horizon are not built and overlap EN-01, which is
  authorization-gated.
- **XP-05** — already live (SOFC inventory fuel); verify-and-record only, per
  the W1 premise correction.

## 6. Queue counts (measured)

| Measure | Before (2026-09-18 refresh) | Now (2026-09-19) |
|---|---:|---:|
| Corpus nonterminal rows | 117 | **112** (111 `AUDIT-PENDING` + 1 `READY-UNCLAIMED`; anchors C2[9]–C2[13] sealed) |
| PARTIALLY-SEALED anchors | 5 | **0** |
| Completion-first roster | 15 open | **6 executed · 5 available · 4 decision-blocked** |
| Decision-packet blockers resolved by execution | — | D1, D2, D5, D6, D7, D8, D9, D10, D14, D15, D23-item-1 |
| Debt seals landed 2026-09-18/19 | — | **10** |
| Available for integration | 1 (`E1` READY-UNCLAIMED) | **8** (§3) |

## 7. Ledger-truth residuals found (for the integrator; not fixed by this audit)

1. `docs/governance/DECISION_REGISTER.md` DEC-01/DEC-16 still read
   `DEFERRED-WITH-CONDITION` although D1/D2 were resolved and implemented
   2026-09-18 (`PLAN_24_CLOSEOUT.md` header: "both signature items resolved").
   The register invariant ("zero items unsigned without a condition") is
   still formally false.
2. DEC-05's evidence field says `Plan147RestockPriorityTests` "14/14 PASS";
   the current file passes **6/6** (count drift, verdict unchanged).
3. `INTEGRATION_PLANS.md` still describes merchant restock priority as
   "Still deferred with authority question" while DEC-05 is `SIGNED` and the
   implementation is live (see §3 item 2).
4. Census anchors C2[9]–C2[13] have not yet been flipped to `SEALED`; this
   audit supplies the sealing evidence for each row (§2).
5. `AGENTS.md`'s handoff pointed every agent at the completed AGY C1 UI panel
   wave (decision packet G4) — **fixed by this audit** per the foreman's
   instruction; the 13 client rulebooks were regenerated with
   `scripts/ci/sync-agent-rulebooks.py` and the rulebook integrity test re-run.
6. The Wave 12 Seal-steps documents (Parts 1.2/2.2/3.1/3.2 and
   `Unblocking-tasks/` Parts 4–10) remain uncertified plan documents with
   their own predecessor chains; they are outside the counted live queue.

## 8. Verification commands and results (run 2026-09-19)

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs` | 6/6 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/FactionWarClockTests.cs` | 1/1 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/CampaignCompletionHistoryTests.cs` | 11/11 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/World/WildlifeMapOverlayTests.cs` | 1/1 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/CatalogPathForbiddenGateTests.cs` | 2/2 PASS |
| `python3 scripts/ci/generate-port-contract.py --check` | PASS — 262 seams, 180 HOST_REQUIRED, 0 deferred |
| `python3 scripts/ci/sync-agent-rulebooks.py` | 13 client rulebooks regenerated from canonical `AGENTS.md` |

No full-suite run was performed (TEST_POLICY.md focused-verification rule).
Static evidence (greps/reads) is cited inline per claim.

## 9. Confidence and unknowns

- High confidence on every §2 seal and §3 availability item (source + test or
  gate evidence re-verified today).
- Medium confidence on C2[15]/C2[21] *scope* freshness: their per-clause
  content was authored before the 2026-09-18/19 execution wave; the premise
  audit must confirm the plans still describe real gaps (the same rule that
  retired XP-02/XP-03 premises).
- Unknown (not investigated, out of scope): the `main`-branch findings in
  `Seal-steps/482913_ASHFALL_REPOSITORY_AUDIT_AND_CODEX_EXECUTION_PLAN_2026-09-19.md`
  (port-contract token-level caller proof, stranded PR #55 delta, CI
  ruleset). They concern the remote default branch, not this local queue
  count.
