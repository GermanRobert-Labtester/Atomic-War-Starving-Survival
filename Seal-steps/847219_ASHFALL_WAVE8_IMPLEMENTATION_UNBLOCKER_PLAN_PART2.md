# ASHFALL — WAVE 8 IMPLEMENTATION UNBLOCKER MASTER PLAN — PART 2

**Document role:** execution-grade continuation of the Wave 8 blocker-unblocking package.

**Scope:** only the blocker classes and continuation frontier established by the supplied Wave 8 continuation evidence. This document does not introduce unrelated mechanics, rebalance systems, invent authorities, or silently convert undecided design into implementation. Every implementation path begins by re-verifying current repository truth at `HEAD`.

**Primary execution set:**
- C1 — black-market trade actions and the unsigned funds/goods settlement decision.
- C2 — amputation equipment restrictions, expedition limp integration, and the print-only avatar placeholder.
- C3 — Plans 174/175/191 plus mapping/disposition for 192/199.
- D1 — standing red tests, stale completion truth, and architecture-map reconciliation.
- D2 — recorded decisions that were never executed, led by the retired SurvivorInspection projection.
- D3 — Godot shutdown resource/RID warning triage and lifetime-gate closure.

**Continuation frontier:** merchant-restock priority; SignalTrust availability consumer; C2[2] deferred real gaps; C1.4–C1.10; distress follow-up/audio-cue content; test-quarantine `Compile Remove` entries; cloud-seeding runtime consumption; F1/F9 governance decisions. These are not implemented by this document unless re-verification proves that a package is both still blocked and authority-safe to promote.

---

## 0. EXECUTION CONTRACT

### 0.1 Definition of “unblocked”

A task is **UNBLOCKED** only when one of these terminal outcomes is proven:

1. **IMPLEMENTED:** the missing integration/presentation/verification path exists, is owned by the correct authority, and its focused acceptance gates pass.
2. **DECIDED-DEFERRED:** a previously unsigned blocker received a recorded decision to remain deferred/read-only/out-of-scope, and source/docs no longer imply that implementation exists.
3. **RETIRED:** a duplicated, obsolete, or deliberately abandoned path is removed or clearly quarantined according to an already-recorded decision.
4. **VERIFIED-RESOLVED:** re-verification at `HEAD` proves the blocker was already sealed by concurrent work; the task closes with evidence and performs no redundant implementation.
5. **ROUTED-REPAIR:** a baseline problem is proven to be a real regression outside the current package and receives a named repair package, reproducible case, owner, and acceptance gate. It must not remain “pre-existing” without ownership.

A documentation edit by itself is never sufficient unless the blocker is explicitly a decision/disposition/documentation-truth problem.

### 0.2 Hard rules

- Claim paths before edits through the repository’s ownership ledger.
- Re-run premise discovery at `HEAD` immediately before implementation.
- Use existing authorities; do not create a second currency, inventory, limb, expedition, survivor-projection, or resource-lifetime system.
- Stop at signature gates. A missing decision is not permission to pick a design.
- Keep migrations behavior-preserving unless the signed decision explicitly changes behavior.
- Every production change needs a focused test or selftest that would fail if the change regressed.
- Generated artifacts are regenerated from their inputs; generated outputs are never hand-edited.
- UI reads models and routes commands; it does not recompute domain truth.
- Save changes are additive/versioned when needed and avoided when state can remain derived.
- Event-driven refresh replaces per-frame polling where the surrounding subsystem already uses events.
- Disabled UI actions expose the reason as text; color is reinforcement only.
- Exactly-once domain events remain exactly-once through save/restore and UI interaction.
- One task is executed at a time unless file ownership and dependencies prove true independence.

### 0.3 Required evidence bundle per task

Every handoff must contain:
- `premise.md` or equivalent note: blocker statement, `HEAD` commit, exact current owner paths, proof the blocker still exists.
- path-claim record.
- decision record when signature-gated.
- change inventory grouped by owner.
- focused test commands and exact result counts.
- build result.
- relevant selftest result.
- save/load result when persisted or exactly-once state is touched.
- replay/determinism result when state transitions are changed.
- UI lifecycle/a11y/snapshot result when UI is touched.
- generated-artifact check result when maps/manifests/indexes are touched.
- documentation/debt-ledger changes.
- explicit remaining blockers.

### 0.4 Stop-work conditions

Stop implementation and convert the finding into a signed decision or routed repair when:
- the expected owner does not exist;
- two owners both claim authority over the same state;
- a new persistence scope would be required but the plan did not authorize one;
- a supposedly presentation-only change requires domain policy changes;
- the blocker disappeared at `HEAD`;
- a focused parity test proves the written plan’s premise is wrong;
- the only way to make the test green is weakening an invariant;
- a generated file would need manual editing;
- a concurrent worktree owns the required path;
- a “fix” would mask a resource leak with global shutdown cleanup instead of correcting lifetime ownership.

---

# 1. CRITICAL PATH AND RELEASE ORDER

## 1.1 Part 2 task order

1. **D1 first when baseline ambiguity prevents trustworthy verification.** It may classify real regressions but does not absorb them.
2. **C1 and C2 require foreman signatures before behavior implementation.** Forensics and decision memos may proceed before signature.
3. **C3 is decision-work first.** It does not implement promoted endgame plans; it produces dispositions and promotable packages.
4. **D2 can run without a new decision only for the SurvivorInspection retirement because that decision already exists.**
5. **D3 runs against the cleanest available panel-heavy baseline and should preferably follow any panel work that changes lifetime/disposal behavior.**
6. The Wave 9 frontier is re-verified only after these tasks have updated the debt truth.

## 1.2 Parallelism rule

Parallel work is allowed only when:
- claimed paths do not overlap;
- no task consumes an output from the other;
- both tasks have independent verification harnesses;
- neither task touches Campaign/DailyBriefing while that area is claimed elsewhere;
- generated artifacts are not concurrently regenerated from shared inputs.

C1 and C2 are usually separable. D1 and D2 can be separable if the architecture-map generator inputs do not overlap. D3 must not race a panel-lifetime rewrite.

---

# TASK C1 — Unblock Black-Market Trade Actions — Funds/Goods Settlement Decision and Action Surface

## Outcome

Resolve the unsigned settlement design, then expose truthful Buy/Sell/Loan/Repay actions through the existing black-market authority without introducing parallel currency, inventory, restock, or heat/trust policy.

## Verified starting premises

- The black-market Core already owns atomic Buy/Sell with preflights and loan behavior.
- `BlackMarketPanel.cs` currently renders prices, debt, heat, and trust but not Buy/Sell/Loan/Repay controls.
- The blocker is explicitly the undefined funds/goods legs and therefore requires a foreman decision before implementation.
- Legitimate currency settlement and canonical inventory ownership must be reused.
- Same-day reopen must not reroll/restock the market.

## Decision gate

Required before any settlement or player-action behavior is changed. The memo must decide currency owner, goods-delivery semantics, and whether existing Core heat/trust effects remain unchanged.

## Authority boundaries

- Black-market domain owner: remains the only authority for black-market policy, preflights, loan state, heat/trust effects already owned there.
- Currency owner: discover the legitimate market settlement authority and reuse it; black-market code may request settlement but may not create a second wallet.
- Inventory owner: canonical item/inventory authority moves contraband item IDs; panel never mutates inventory directly.
- UI owner: `BlackMarketPanel.cs` and its existing host/bind session route commands and display results only.
- Save owner: existing black-market debt/loan state remains authoritative; no new persisted section unless the signed goods-delivery model truly requires one.

## Required deliverables


- `C1_PREMISE_EVIDENCE.md` — current blocker proof, exact paths, current commit, ownership claims.
- `C1_DECISION.md` — signed choice and the exact behavior/authority consequences.
- `C1_CHANGE_MATRIX.md` — owner/path/change/reason/verification for every touched production or test file.
- `C1_ACCEPTANCE.md` — command log, result counts, save/replay/UI evidence applicable to the task.
- `C1_HANDOFF.md` — terminal status, remaining debt, rollback notes, and next owner.

## Phase 0 — Re-verify blocker and claim paths

1. Locate the Plan 211 closeout, debt row, black-market authority documentation, `BlackMarketSystem`, `BlackMarketPanel.cs`, the panel’s host/session binder, and the legitimate `MarketSystem` settlement path.
2. Record exact method names for buy, sell, loan, repay, preflight, tick/restock, and state-query APIs. Do not infer method names from the plan.
3. Prove the panel has no action controls at `HEAD` and that no parallel black-market panel was added by concurrent work.
4. Prove contraband stock IDs resolve to canonical item IDs and record the inventory mutation API currently used by legitimate trade.
5. Claim only the smallest paths needed for the decision memo first. Expand the claim after signature if implementation is authorized.

## Phase 1 — Settlement forensics and signed decision

1. Trace one successful legitimate purchase end-to-end: UI/host command → market preflight → currency debit → inventory grant → result/event. Capture the exact transaction ordering and failure behavior.
2. Trace one legitimate sale end-to-end: inventory removal/reservation → currency credit → market stock change. Determine whether the current code provides atomicity or compensating rollback.
3. Trace black-market Core Buy/Sell today and identify which settlement legs are internal, delegated, or absent.
4. Inspect any contraband stash/location matrix. If stash semantics already exist, the decision memo must treat them as an existing constraint rather than reopening the design.
5. Write a decision table with at least: immediate inventory delivery, stash delivery if supported by existing authority, and read-only deferral. For each option list required owner calls, save implications, failure atomicity, and UI consequence.
6. Explicitly state that new heat/trust policy is out of scope. If Core already applies these effects, the UI surfaces them; it does not add another effect.
7. Obtain signature. Store the signed choice next to the blocker evidence and quote the chosen transaction semantics in the implementation handoff.

## Phase 2 — Domain/host seam

1. If existing Core methods already provide the signed transaction semantics, make no Core redesign. Add only the host/session adapter necessary to route commands and return typed results.
2. If the signed design requires one missing leg, extend the owning domain at the narrowest seam. Use canonical currency/inventory APIs and preserve atomicity: failed currency debit must not grant goods; failed inventory transfer must not leave currency consumed.
3. Represent preflight failures as stable reason IDs or the repository’s existing result type. UI text derives from those results; it must not duplicate policy.
4. Loan and repay commands must preserve due-date and exactly-once overdue behavior. UI invocation cannot create a second overdue evaluator.
5. Ensure read-only state queries remain side-effect free. Merely opening or refreshing the panel must never advance restock, interest, debt, trust, or heat.

## Phase 3 — Panel action surface

1. Add action controls adjacent to the stock rows using the panel’s existing layout vocabulary. Each action must have a deterministic focus target and a text reason when unavailable.
2. BUY disabled states must distinguish at least the failures actually emitted by Core, such as insufficient funds, access/tier restriction, or no stock. Do not manufacture reason categories not returned by the authority.
3. SELL must use canonical player inventory availability; never infer sellability from panel-local counts.
4. TAKE LOAN and REPAY use the existing debt state. Due-date context remains visible before action.
5. After a command, render the returned outcome and refresh from authoritative state. Do not optimistically mutate the visible stock/wallet and then hope the domain matches.
6. Keep refresh event-driven through the existing underworld tick/action-result events. Reopening the panel on the same day must display the same stock unless domain time actually advanced.
7. Preserve keyboard close/back, row navigation, and focus restoration after an action.

## Phase 4 — Verification

1. Write command tests for success and every reachable preflight failure. Assertions include currency delta, inventory delta, stock delta, debt delta, and unchanged state on failure.
2. Write an atomicity test around the signed funds/goods ordering. Inject or construct a failing second leg using existing test seams; assert no half-transaction remains.
3. Write a same-day reopen/no-reroll test. Opening, closing, and reopening the panel must not call restock or change stock.
4. Run the existing economy test directory and any black-market focused tests before broader aggregates.
5. Re-run the debt-spiral/overdue scenario across save/restore and assert the overdue event remains exactly once.
6. Run panel route, bind-lifecycle, a11y, and snapshot gates for the changed surface.
7. Run build and data-integrity gates. Data should be unchanged unless the signed decision explicitly requires authored data.

## Phase 5 — Closure

1. Update the Plan 211 closeout and deferred-decision ledger with the signed outcome.
2. If the decision is read-only, remove language implying actions are pending implementation and mark the blocker DECIDED-DEFERRED.
3. If actions ship, provide an action matrix: command → preflight → funds owner → goods owner → domain side effects → UI result → test.
4. Record any remaining non-blocking UX opportunities separately; do not leave them in the blocker row.

## Acceptance test matrix

1. **Gate 1:** Black-market buy success and each Core failure reason.
2. **Gate 2:** Black-market sell success and each Core failure reason.
3. **Gate 3:** Funds/goods transaction atomicity.
4. **Gate 4:** Loan creation and repay path.
5. **Gate 5:** Overdue exactly-once across save/restore.
6. **Gate 6:** Same-day panel reopen does not reroll/restock.
7. **Gate 7:** Panel keyboard/focus/disabled-reason behavior.
8. **Gate 8:** Panel lifecycle dispose/unsubscribe.
9. **Gate 9:** Economy focused suite, build, integrity, snapshot.

## Failure handling / rollback rules

- If the signed settlement requires a new wallet or duplicate inventory store, stop: the design violates authority reuse.
- If UI wiring changes trade economics relative to headless Core tests, revert UI-side policy and expose Core results only.
- If transaction atomicity cannot be guaranteed with existing APIs, do not ship partial actions; route a narrow domain-transaction repair package.
- If snapshot change reveals layout overflow, fix layout; do not hide action states.

## Definition of done


- The original blocker statement is re-checked against `HEAD` and is no longer true, or its signed disposition makes the remaining behavior intentionally out of scope.
- No authority duplication was introduced.
- Every production behavior change has a focused regression gate.
- Save/load and exactly-once behavior are proven where applicable.
- UI changes pass route/lifecycle/accessibility/snapshot gates where applicable.
- Generated artifacts pass their `--check`/regeneration workflow where applicable.
- Build passes with no new warning debt attributable to the task.
- Debt, audit, closeout, or portfolio truth reflects the final state.
- The handoff names any remaining blocker rather than hiding it in prose.


# TASK C2 — Unblock Amputation Integration — Equipment Contract, Limp Consumer, and Avatar Placeholder Truth

## Outcome

Make `LimbCondition` materially constrain the owners that need it, without moving equipment or expedition policy into the medical system, and execute the recorded decision that no survivor-avatar integration exists yet.

## Verified starting premises

- `LimbCondition` lacks a consumer outside the amputation triage surface.
- The equipment restriction debt is blocked on the equipment owner's contract.
- `RefreshSurvivorVisuals` is a print-only no-op/TODO path and the recorded decision keeps avatar work out of scope until an avatar owner exists.
- Equipment restrictions belong in the equipment owner; movement/route penalties belong in the expedition owner.
- No presentation-only limb illusion may substitute for actual domain restrictions.

## Decision gate

Required for the equipment/expedition contract. The avatar non-integration decision already exists; only delete-vs-honest-stub disposition may need a lightweight confirmation if repository policy requires it.

## Authority boundaries

- Amputation/medical owner: writes limb condition and exposes public read state.
- Equipment owner: owns equip eligibility/preflight and all slot/weapon restrictions.
- Expedition owner: owns route-speed or travel modifier consumption.
- UI equipment surface: displays preflight reason; it does not compute limb restrictions.
- Avatar/presentation: no new owner is created in this task. The existing placeholder is deleted or made explicitly non-functional according to the recorded decision.

## Required deliverables


- `C2_PREMISE_EVIDENCE.md` — current blocker proof, exact paths, current commit, ownership claims.
- `C2_DECISION.md` — signed choice and the exact behavior/authority consequences.
- `C2_CHANGE_MATRIX.md` — owner/path/change/reason/verification for every touched production or test file.
- `C2_ACCEPTANCE.md` — command log, result counts, save/replay/UI evidence applicable to the task.
- `C2_HANDOFF.md` — terminal status, remaining debt, rollback notes, and next owner.

## Phase 0 — Current-truth census

1. Search the whole tree for `LimbCondition`, amputation state APIs, equipment slot/equip preflights, route/travel speed modifiers, `RefreshSurvivorVisuals`, and the TODO locations.
2. Record every current consumer of limb state. If a real equipment or expedition consumer already landed, compare it with the debt row and close only the missing remainder.
3. Verify whether portraits still resolve through the fallback survivor asset path and whether any true avatar owner now exists.
4. Claim medical read-state paths only if an additive public query is required; otherwise leave the medical owner untouched.

## Phase 1 — Contract decision

1. Document the existing equipment model: slots, handedness representation, equip preflight shape, disabled-reason vocabulary, prosthetic item representation if any.
2. Document the expedition model: where route speed is composed, how bounded modifiers are registered, and how survivor-specific constraints enter the travel calculation.
3. Write a contract matrix keyed by existing limb states. Rows must be limited to rules supported by the signed design and current model. Do not invent prosthetic grades or equipment categories absent from source/data.
4. Separate concerns explicitly: arm state may affect two-handed equipment eligibility; leg state may feed expedition movement; medical UI remains informational/triage.
5. Define intact-state parity as a hard invariant: survivors with intact limbs must produce identical equipment and expedition outcomes before and after integration.
6. Obtain signature before implementing any new restriction or travel penalty.

## Phase 2 — Equipment consumer

1. Add a limb-state read at the equipment owner’s preflight seam. Prefer an existing dependency/read-model interface over reaching through UI or global state.
2. Return the repository’s existing preflight result type with a stable limb-related reason. The equipment system remains the writer of equip eligibility.
3. Apply restrictions only to item/slot combinations explicitly authorized by the contract. Do not globally disable gear because a limb is impaired.
4. Ensure restored old saves with an amputation state produce the same preflight result immediately after load; no migration should be required if the limb state already persists canonically.
5. Update equipment UI to display the returned reason and keep the action keyboard reachable even when disabled-state explanation must be read.

## Phase 3 — Expedition limp consumer

1. Use the expedition owner’s existing modifier seam if present. If no such seam exists, stop and route the missing owner contract rather than modifying travel values from medical code.
2. Map only the signed limb state to a bounded travel-speed effect. Keep base route calculations untouched.
3. Guarantee deterministic composition order with other travel modifiers using the expedition system’s current ordering convention.
4. Write parity coverage for intact survivors and targeted coverage for affected limb states.
5. Save/load should require no new state when the modifier is derived from persisted limb state. Prove this with a mid-expedition or pre-route restore test as supported by current harnesses.

## Phase 4 — Avatar placeholder cleanup

1. Trace all calls to `RefreshSurvivorVisuals`. If the method has no meaningful consumer and exists only to print/log, delete the dead path and its misleading TODO.
2. If a stable call contract must remain for compilation or sequencing, replace the body with an explicit documented no-op that cites the recorded out-of-scope decision. It must not log as though visuals were refreshed.
3. Do not create sprites, amputated portrait variants, animation swapping, or conditional asset paths. The point is truthfulness, not hidden feature expansion.
4. Update the debt/audit record so future agents do not rediscover the same placeholder as an implementation gap.

## Phase 5 — Verification and closure

1. Run limb-state equip preflight tests for every state represented by the signed matrix.
2. Run old-save restore coverage for amputation state and immediate equip behavior.
3. Run expedition parity/fingerprint coverage: intact unchanged, affected state altered only through the signed modifier.
4. Run medical, equipment, and expedition focused suites; then build and fast aggregate gates.
5. Run equipment panel a11y/snapshot gates for new disabled reasons.
6. Update the authority matrix so future work can see medical=writer, equipment=eligibility consumer, expedition=movement consumer.
7. Move the debt row to SEALED or SPLIT-SEALED with the avatar work explicitly deferred by the pre-existing decision.

## Acceptance test matrix

1. **Gate 1:** Equip preflight intact limb.
2. **Gate 2:** Equip preflight affected arm state for signed item classes.
3. **Gate 3:** Equip preflight prosthetic-related state only if canonical model already contains it.
4. **Gate 4:** Old save restores limb state and restriction.
5. **Gate 5:** Intact expedition fingerprint unchanged.
6. **Gate 6:** Affected-limb expedition modifier applied once.
7. **Gate 7:** No medical-panel-side equip gating.
8. **Gate 8:** Equipment UI disabled reason and accessibility.
9. **Gate 9:** Placeholder call-path deletion/stub compilation.
10. **Gate 10:** Build and focused suites.

## Failure handling / rollback rules

- If the equipment model cannot represent the signed restriction without a schema redesign, stop and split that redesign into a separate signed package.
- If expedition has no canonical modifier seam, do not write directly to route speed from amputation code.
- If deleting the avatar placeholder breaks a real consumer, restore the hook and document the newly discovered owner rather than recreating fake functionality.
- If intact parity changes, revert the integration until the extra effect is isolated.

## Definition of done


- The original blocker statement is re-checked against `HEAD` and is no longer true, or its signed disposition makes the remaining behavior intentionally out of scope.
- No authority duplication was introduced.
- Every production behavior change has a focused regression gate.
- Save/load and exactly-once behavior are proven where applicable.
- UI changes pass route/lifecycle/accessibility/snapshot gates where applicable.
- Generated artifacts pass their `--check`/regeneration workflow where applicable.
- Build passes with no new warning debt attributable to the task.
- Debt, audit, closeout, or portfolio truth reflects the final state.
- The handoff names any remaining blocker rather than hiding it in prose.


# TASK C3 — Unblock the 170–199 Endgame Portfolio — Disposition Plans 174/175/191 and Map 192/199

## Outcome

Eliminate the portfolio’s ambiguous blocked frontier by proving whether each remaining plan should be promoted, retired, or held, while producing implementation-ready first packages only for signed promotions.

## Verified starting premises

- Plans 174, 175, and 191 are recorded as blocked/unstarted.
- Plans 192 and 199 remain unmapped and require authority mapping before implementation decisions.
- The item-inspection half related to 191 is already substantially implemented, so collision/remaining-scope analysis is mandatory.
- Meta/New Game+ potentially introduces a new save scope and cannot be casually appended to campaign save state.
- This task is disposition/planning work; it does not implement the promoted features.

## Decision gate

Required for PROMOTE/RETIRE/HOLD disposition. HOLD must name a concrete recheck condition. PROMOTE produces a ledger package but no production implementation in this task.

## Authority boundaries

- Portfolio/family maps: documentation truth and authority mapping only.
- Integration ledger: receives first executable package for promoted plans.
- Existing survivor arc/echo owners constrain Plan 174.
- Existing item inspection/valuation owners constrain Plan 191.
- Campaign ending/generational owners constrain Plan 175.
- Caravan/waystation and wildlife/faction-ecology owners are investigated for 192/199 rather than assumed.

## Required deliverables


- `C3_PREMISE_EVIDENCE.md` — current blocker proof, exact paths, current commit, ownership claims.
- `C3_DECISION.md` — signed choice and the exact behavior/authority consequences.
- `C3_CHANGE_MATRIX.md` — owner/path/change/reason/verification for every touched production or test file.
- `C3_ACCEPTANCE.md` — command log, result counts, save/replay/UI evidence applicable to the task.
- `C3_HANDOFF.md` — terminal status, remaining debt, rollback notes, and next owner.

## Phase 0 — Evidence pack

1. Read the portfolio rows and remaining-family-map document for 174/175/191 and collect exact reasons each was not promoted.
2. Search current source for survivor backstory generation, survivor arc/echo systems, item inspection/appraisal/valuation, epilogue/meta-state, trade-route establishment, seasonal migration, caravan/waystation, wildlife migration, and faction ecology.
3. Build a collision table: planned capability → current owner → overlap level (none/partial/full) → missing behavior → likely disposition.
4. Do not use old plan wording as proof that code is absent; absence is established by current source search.

## Phase 1 — Plan 174 survivor backstories

1. Inventory current survivor profile generation inputs and outputs and the arc/echo narrative layers that already add history or context.
2. Determine whether Plan 174 asks for durable state, generated text, tags used by gameplay, or only flavor. This matters because a flavor-only generator may duplicate existing narrative texture.
3. List unique capabilities not currently supplied. If no unique capability remains, recommend RETIRE in the memo with collision evidence.
4. If unique capability remains, define the smallest first package that extends an existing narrative owner instead of creating a parallel backstory engine.
5. Define acceptance in observable terms: generated result consumed by an existing surface or mechanic, deterministic under seed where required, save semantics explicit.

## Phase 2 — Plan 175 meta/New Game+

1. Map campaign ending and epilogue outputs, generational links, unlock/progression state, and current save roots.
2. Separate run-local campaign state from cross-run meta-state. Treat cross-run persistence as a new scope requiring explicit versioning, reset behavior, and compatibility decisions.
3. Produce at least three disposition options grounded in current architecture: retire as out-of-scope; hold until save-scope policy exists; promote with Phase 0 devoted solely to meta-state ownership/versioning.
4. Do not define carry-over items, stats, or unlock balance in this task. Those are product choices after architecture safety is decided.
5. If promoted, the first package must be a save-boundary/authority package, not a content implementation package.

## Phase 3 — Plan 191 item identification/appraisal

1. Inventory the current `ItemInspectionModel` and related valuation/market reads. Record what is already player-visible and what remains only in historical plan text.
2. Distinguish identification (unknown → known state) from appraisal (value/quality estimate) and ordinary inspection (display metadata).
3. If current inspection already satisfies the player need without persistent unidentified state, determine whether the remaining plan should be retired rather than inventing hidden-item state.
4. If a real appraisal gap remains, define it through existing valuation/economy authorities. UI must consume those values; it cannot calculate an independent appraisal price.
5. Prefer promotion only when a distinct, source-supported remaining capability exists.

## Phase 4 — Plan 192 trade-route map

1. Map current caravan routes, waystations, regional economy, travel topology, and route persistence.
2. Ask one architecture question: what existing owner would be authoritative if the player establishes a route? Record candidate owners and conflicts.
3. Produce a read-only family map that identifies likely command owner, state owner, save owner, presentation surfaces, and blockers.
4. Do not implement route creation or add state.

## Phase 5 — Plan 199 seasonal migration map

1. Map wildlife migration and faction/ecology systems currently present.
2. Determine whether seasonal migration is wildlife-only, faction/civilian movement, world-population state, or a historical plan umbrella. Do not assume the meaning.
3. Produce the family map with candidate authority seams and collision risks.
4. Do not add seasonal clocks, migration state, or content.

## Phase 6 — Signed disposition and ledger update

1. Create a five-row decision table for 174/175/191/192/199 with evidence, PROMOTE/RETIRE/HOLD recommendation, and exact consequence.
2. For HOLD, define a measurable condition such as 'meta-save owner approved' or 'trade-route authority selected'; never write 'later'.
3. For RETIRE, add status banners to historical plan docs and update debt/portfolio truth.
4. For PROMOTE, create only the first package in the integration ledger with premise evidence, exact discoverable paths, acceptance gates, and focused verification.
5. Run documentation/index generation checks and prove zero production changes.
6. Close the portfolio decision frontier only when all five have a signed terminal/active disposition.

## Acceptance test matrix

1. **Gate 1:** Zero production file changes.
2. **Gate 2:** Docs-index generator/check.
3. **Gate 3:** Family-map references resolve.
4. **Gate 4:** Each promoted package names an owner, paths to discover/claim, acceptance, and verification.
5. **Gate 5:** Each retired plan has a status banner and portfolio/debt truth update.
6. **Gate 6:** Each held plan has a concrete recheck condition.

## Failure handling / rollback rules

- If current source disproves a historical blocked reason, rewrite the disposition evidence rather than preserving the stale premise.
- If 175 promotion cannot name a meta-state owner, HOLD is mandatory.
- If 174 duplicates arc/echo behavior, do not create a second narrative generator for plan-completion optics.
- If 191 requires inventing unidentified-state persistence to justify itself, stop and reassess whether retirement is more truthful.

## Definition of done


- The original blocker statement is re-checked against `HEAD` and is no longer true, or its signed disposition makes the remaining behavior intentionally out of scope.
- No authority duplication was introduced.
- Every production behavior change has a focused regression gate.
- Save/load and exactly-once behavior are proven where applicable.
- UI changes pass route/lifecycle/accessibility/snapshot gates where applicable.
- Generated artifacts pass their `--check`/regeneration workflow where applicable.
- Build passes with no new warning debt attributable to the task.
- Debt, audit, closeout, or portfolio truth reflects the final state.
- The handoff names any remaining blocker rather than hiding it in prose.


# TASK D1 — Unblock Verification Truth — Standing Failures, Stale Source Comments, and Architecture Map Reconciliation

## Outcome

Convert the inherited ambiguous baseline into accounted-for truth: every standing failure either fixed as a stale contract, proven resolved, or routed to an owned repair package; every completion marker/map row reflects current source.

## Verified starting premises

- Five full-suite failures were inherited from the 210–213 handoff without completed triage.
- A Plan 126–129 source header incorrectly says completed plans are pending.
- The architecture map contains dozens of GAP rows known to include stale entries.
- Plan 24’s 14/15 needs characterization issue is handled in Part 1/A4, but D1 establishes the class-level baseline discipline.
- Real regressions must not be silently fixed inside truth-reconciliation work.

## Decision gate

No signature needed for stale-test rematches, comment truth fixes, or generator-input reconciliation. Real regressions become separate owned repair packages.

## Authority boundaries

- Test contract owner: only stale assertions are rematched here.
- Production bug owner: receives real regressions as separate repair packages.
- Architecture-map source graph/generator inputs: corrected when stale; generated output is never edited by hand.
- Source comments/docs: corrected to current completed/pending truth without renaming plan files.

## Required deliverables


- `D1_PREMISE_EVIDENCE.md` — current blocker proof, exact paths, current commit, ownership claims.
- `D1_CHANGE_MATRIX.md` — owner/path/change/reason/verification for every touched production or test file.
- `D1_ACCEPTANCE.md` — command log, result counts, save/replay/UI evidence applicable to the task.
- `D1_HANDOFF.md` — terminal status, remaining debt, rollback notes, and next owner.

## Phase 0 — Freeze the baseline

1. Locate the 210–213 handoff and extract the exact five failing test identities and messages. Prefer recorded evidence.
2. If the identities were not recorded, perform one explicitly sanctioned diagnostic full-suite run and capture only the necessary failure list, duration, seed/environment, and commit.
3. Create a five-row triage ledger before changing code.

## Phase 1 — Failure classification

1. Run each failing test file or smallest test target in isolation.
2. Classify as REAL REGRESSION, STALE CONTRACT, or ALREADY FIXED.
3. For stale contract, prove the production behavior matches newer documented/live truth and update only the assertion/fixture required to represent that truth. Add a drift comment explaining the old vs current model.
4. For real regression, create a repair package containing reproduction, suspected owner, impact, and acceptance. Do not change production code in D1.
5. For already-fixed, record the passing evidence and remove the inherited-failure label.

## Phase 2 — Source-comment truth pass

1. Correct the known `Main.Plans126_129.cs` stale pending header after verifying completion logs.
2. Search sibling Plan source files for completion-state comments using terms like pending/not yet/TODO only as candidates; each candidate must be checked against authoritative completion logs before editing.
3. Keep edits surgical. Do not rename source files to repair historical numbering drift.
4. Where a TODO is genuinely live, leave it and, if necessary, link it to a debt row rather than erasing it.

## Phase 3 — Architecture-map reconciliation

1. Run the architecture-map generator/check on current source and capture the current GAP set.
2. Sample known stale rows first and prove whether their source inputs still omit existing panels/routes.
3. Trace each stale output back to the graph/registry/test fixture that feeds the generator and correct that input.
4. Regenerate and check. Never patch the emitted map text directly.
5. For true gaps, create or link the appropriate panel/route debt package; do not implement those gaps inside D1.

## Phase 4 — Baseline policy hardening

1. Add a release-checklist field requiring the standing-failure count to be zero or explicitly linked to owner packages.
2. Ensure handoffs cannot use 'pre-existing failures' as an unbounded exemption; each failure identity must remain named.
3. Keep this lightweight: do not build a brittle source-comment linter that guesses semantic truth.

## Phase 5 — Verification and closure

1. Re-run each triaged test target.
2. Run adjacent directories only when fixtures/contracts moved.
3. Run architecture-map `--check`, build, and verify-fast.
4. Update debt/audit rows with the new count: zero, or exact remainder with package IDs.
5. Produce a baseline table containing previous status, classification, action, current status, and owner.

## Acceptance test matrix

1. **Gate 1:** Each of the five inherited failure targets in isolation.
2. **Gate 2:** Adjacent suites for any stale fixture rematch.
3. **Gate 3:** Architecture map generation/check.
4. **Gate 4:** Build.
5. **Gate 5:** Verify-fast.
6. **Gate 6:** No hand-edited generated output.
7. **Gate 7:** Standing failure ledger resolves every row.

## Failure handling / rollback rules

- If a test is red because production is wrong, revert any temptation to change the assertion and route a repair.
- If generator output changes unexpectedly beyond reconciled rows, inspect generator inputs before accepting the diff.
- If a source comment has no authoritative completion log, leave it unchanged and record ambiguity rather than guessing.

## Definition of done


- The original blocker statement is re-checked against `HEAD` and is no longer true, or its signed disposition makes the remaining behavior intentionally out of scope.
- No authority duplication was introduced.
- Every production behavior change has a focused regression gate.
- Save/load and exactly-once behavior are proven where applicable.
- UI changes pass route/lifecycle/accessibility/snapshot gates where applicable.
- Generated artifacts pass their `--check`/regeneration workflow where applicable.
- Build passes with no new warning debt attributable to the task.
- Debt, audit, closeout, or portfolio truth reflects the final state.
- The handoff names any remaining blocker rather than hiding it in prose.


# TASK D2 — Execute Recorded Decisions — SurvivorInspection Retirement and Dead-Data Truth

## Outcome

Finish decisions that already exist so debt rows can actually close, beginning with the retired duplicate SurvivorInspection projection and the recorded primary-wins dead-data rows.

## Verified starting premises

- The duplicate SurvivorInspection projection was already retired by decision but its code remains.
- `SurvivorInspectionHostSession` and `SurvivorInspectionSnapshot` must be rechecked for consumers before deletion.
- Five expansion rows were recorded as primary-wins overridden/dead data and remain a recurring source of ambiguity.
- Deletion of content/data rows still requires the relevant authority; documentation of known-dead rows can proceed without pretending the rows are live.

## Decision gate

No new signature for deleting the zero-consumer SurvivorInspection projection if re-verification confirms the recorded retirement still applies. Data-row deletion needs explicit authority; default is an authoritative dead-data register.

## Authority boundaries

- Survivor inspection duplicate projection: retirement execution only.
- Canonical survivor inspection/read surface: untouched unless consumer discovery proves the retirement premise obsolete.
- Expansion data authority: controls physical row deletion.
- Debt/audit docs: record execution and prevent rediscovery.

## Required deliverables


- `D2_PREMISE_EVIDENCE.md` — current blocker proof, exact paths, current commit, ownership claims.
- `D2_CHANGE_MATRIX.md` — owner/path/change/reason/verification for every touched production or test file.
- `D2_ACCEPTANCE.md` — command log, result counts, save/replay/UI evidence applicable to the task.
- `D2_HANDOFF.md` — terminal status, remaining debt, rollback notes, and next owner.

## Phase 0 — Re-prove zero consumers

1. Search source, tests, docs, reflection/registry lists, serialization type registries, DI/composition roots, and generators for both SurvivorInspection types.
2. Classify each match as declaration, construction, type reference, test-only reference, documentation, or live consumer.
3. If any live consumer exists, stop deletion and route adoption/decision review. The recorded retirement cannot override new reality.

## Phase 1 — Delete the retired projection

1. Delete only the retired duplicate files and direct test fixtures that assert their existence rather than behavior needed elsewhere.
2. Do not migrate consumers to another projection in this package unless a newly discovered consumer makes that necessary and receives explicit ownership.
3. Run build immediately after deletion to expose hidden compile-time consumers.
4. Run survivor-focused tests and architecture-map generation/check because tracked type inventories may change.

## Phase 2 — Close debt truth

1. Update the debt row from retired-but-present/quarantined to RETIRED/SEALED with execution date and decision reference.
2. Update the partial-plan audit so P10 no longer reports a pending delete-or-adopt choice.
3. If generated architecture/type inventories changed, regenerate them from source.

## Phase 3 — Formalize dead-data rows

1. Read the PR2 tranche evidence and list the exact five primary-wins overridden rows and the reason they cannot win.
2. Create a single authoritative dead-data register in the relevant contract/closeout documentation if one does not already exist.
3. For each row record ID, source catalog, override mechanism, reason retained, and deletion authority required.
4. Do not insert comments into JSON unless the schema/formats already permit them.
5. Run integrity checks and confirm any expected primary-wins warnings match the registered row set exactly.

## Phase 4 — Sibling execution sweep

1. Search debt rows for QUARANTINED/RETIRED/DECIDED states whose only remaining work is execution.
2. Produce a candidate list; do not auto-execute unrelated candidates in this package.
3. Each candidate must include the recorded decision reference and current evidence that its preconditions are met.

## Phase 5 — Verification and closure

1. Run survivor and inventory/data neighborhood tests.
2. Run build, integrity, architecture-map check, docs-index check when docs changed, and verify-fast.
3. Provide an execution table: decision → artifact/code action → test evidence → final debt state.
4. Keep data deletion separate unless explicitly signed.

## Acceptance test matrix

1. **Gate 1:** Whole-tree zero-consumer proof before deletion.
2. **Gate 2:** Build immediately after deletion.
3. **Gate 3:** Survivor-focused tests.
4. **Gate 4:** Architecture map generation/check.
5. **Gate 5:** Integrity warnings exactly match registered dead rows.
6. **Gate 6:** Inventory/data neighborhood tests.
7. **Gate 7:** Docs index check.
8. **Gate 8:** Verify-fast.

## Failure handling / rollback rules

- If a live SurvivorInspection consumer exists, restore/retain the projection and route adoption review.
- If architecture generation still expects the retired type, update the generator input based on actual architecture rather than adding a dummy type.
- If dead-data warnings differ from the five registered rows, treat the delta as a new finding; do not broaden the register to hide it.

## Definition of done


- The original blocker statement is re-checked against `HEAD` and is no longer true, or its signed disposition makes the remaining behavior intentionally out of scope.
- No authority duplication was introduced.
- Every production behavior change has a focused regression gate.
- Save/load and exactly-once behavior are proven where applicable.
- UI changes pass route/lifecycle/accessibility/snapshot gates where applicable.
- Generated artifacts pass their `--check`/regeneration workflow where applicable.
- Build passes with no new warning debt attributable to the task.
- Debt, audit, closeout, or portfolio truth reflects the final state.
- The handoff names any remaining blocker rather than hiding it in prose.


# TASK D3 — Unblock Shutdown Cleanliness — Resource/RID Warning Classification and Lifetime Repair

## Outcome

Turn the existing Godot shutdown resource/RID warnings from ambiguous release noise into either a proven-benign documented class or a fixed owner-lifetime defect guarded by repeat-cycle verification.

## Verified starting premises

- Panel-heavy/a11y selftest shutdown has emitted resource/RID warnings.
- A repository leak-triage guide already defines the diagnostic method and is the process authority.
- A passing functional selftest with unclassified shutdown warnings is not a clean release signal.
- Global forced-free cleanup at process exit is forbidden because it masks ownership defects.

## Decision gate

No product signature. Follow the existing diagnostics guide; fix only the owning lifetime path or document benign shutdown-order noise with evidence.

## Authority boundaries

- Leaking panel/surface owns node disposal and signal unsubscription.
- Resource owner owns texture/buffer/resource disposal.
- Bridge/adapter owner owns signal disconnect lifetime.
- Selftest/lifecycle harness owns repeat-cycle guard if it can assert leak-free lifetime cheaply.

## Required deliverables


- `D3_PREMISE_EVIDENCE.md` — current blocker proof, exact paths, current commit, ownership claims.
- `D3_CHANGE_MATRIX.md` — owner/path/change/reason/verification for every touched production or test file.
- `D3_ACCEPTANCE.md` — command log, result counts, save/replay/UI evidence applicable to the task.
- `D3_HANDOFF.md` — terminal status, remaining debt, rollback notes, and next owner.

## Phase 0 — Reproduce and capture

1. Read the leak diagnostics guide completely before modifying code.
2. Run the exact selftest that produced the warning and capture full shutdown text, counts, resource/RID identifiers, and whether exit code remains success.
3. Run panel-bind-lifecycle and settings/panel-heavy selftests to determine whether the warning is specific to a surface or common shutdown behavior.
4. Establish a repeatable invocation and preserve logs as evidence.

## Phase 1 — Classify

1. Use the guide’s telemetry to distinguish node leaks, resource leaks, signal/subscription leaks, or benign shutdown-order warnings.
2. Correlate node counts and resource counts before opening, after closing, and after repeated cycles.
3. For suspected signal leaks, inspect bind/unbind and event subscription symmetry.
4. For suspected resource leaks, identify the allocating owner and normal teardown path; do not start by adding global cleanup.

## Phase 2 — Repair the owner lifetime

1. Node leak: free/unparent through the owning panel/surface teardown using the repository’s established lifecycle pattern.
2. Signal leak: disconnect/unsubscribe exactly where the subscription lifetime ends; avoid anonymous subscriptions that cannot be removed.
3. Resource leak: dispose/release at the owner that allocated or retained the resource, including snapshot/capture buffers if evidence points there.
4. Keep the repair minimal and scoped. Do not change rendering policy or introduce exit-time sweeping.

## Phase 3 — Prove benign warnings when applicable

1. If telemetry is flat across repeated cycles and the warning is strictly shutdown-order noise, document the exact signature and proof in the diagnostics guide.
2. The documentation must state what distinguishes the benign signature from a real leak so future warnings are not blanket-ignored.
3. Keep the selftest output explicit: benign-known warning may be annotated, but unexpected warning signatures remain failures/findings.

## Phase 4 — Regression gate

1. If the harness supports it without excessive fragility, add per-cycle node/resource count assertions to the panel lifecycle selftest.
2. Run multiple open/close cycles of the implicated surface and assert counts return to baseline.
3. Run a11y, lifecycle, settings, audio/bridge, and export-smoke-adjacent paths that share the lifetime class.
4. Run one unchanged rendering snapshot to prove disposal changes did not alter visible output.

## Phase 5 — Closure

1. Update the Plan 24 shutdown-warning note with exact classification and disposition.
2. Update the diagnostics guide with the case pattern.
3. Run build and verify-fast.
4. Run one replay/fingerprint check to prove teardown-only changes do not affect deterministic simulation.
5. Provide a leak-class table: warning signature → class → owner → fix/proof → regression gate.

## Acceptance test matrix

1. **Gate 1:** Original a11y selftest reproduction.
2. **Gate 2:** Panel lifecycle selftest.
3. **Gate 3:** Repeated open/close cycle telemetry.
4. **Gate 4:** Settings/panel-heavy selftests.
5. **Gate 5:** Audio/bridge selftest when signal lifetime is implicated.
6. **Gate 6:** Export-smoke-adjacent shutdown check.
7. **Gate 7:** Unchanged snapshot target.
8. **Gate 8:** Build and verify-fast.
9. **Gate 9:** Replay/fingerprint unchanged.

## Failure handling / rollback rules

- If the only successful fix is a process-exit forced free, revert it and continue owner-lifetime diagnosis.
- If disposal changes visible state before close, restore correct lifetime boundaries.
- If a warning is called benign without flat repeated-cycle telemetry, the classification is insufficient and the task remains open.

## Definition of done


- The original blocker statement is re-checked against `HEAD` and is no longer true, or its signed disposition makes the remaining behavior intentionally out of scope.
- No authority duplication was introduced.
- Every production behavior change has a focused regression gate.
- Save/load and exactly-once behavior are proven where applicable.
- UI changes pass route/lifecycle/accessibility/snapshot gates where applicable.
- Generated artifacts pass their `--check`/regeneration workflow where applicable.
- Build passes with no new warning debt attributable to the task.
- Debt, audit, closeout, or portfolio truth reflects the final state.
- The handoff names any remaining blocker rather than hiding it in prose.



# 8. WAVE 9 FRONTIER — RE-VERIFY BEFORE PROMOTION

This section is a **promotion filter**, not an authorization to implement everything listed. Its purpose is to ensure the next wave starts from live truth and does not reproduce stale blocker archaeology.

## 8.1 Merchant-restock priority decision

### Current blocker class
Decision-blocked seam. The next agent must first locate the prior Wave 1 task, current merchant/restock authority, and any signed design created since that wave.

### Re-verification sequence
1. Find the debt/plan row that names merchant-restock priority.
2. Locate the current restock algorithm and every consumer of its output.
3. Determine whether “priority” is still undefined or whether later economy work implicitly established the rule.
4. If a rule exists in production but docs still call it blocked, route to D1-style truth reconciliation.
5. If no rule exists and behavior cannot be derived from an existing authority, prepare a decision memo rather than changing restock order.
6. The memo must define only the priority policy; stock generation, pricing, and market timing remain under their current owners.
7. Promotion criterion: signed policy + named owner + focused deterministic restock test.
8. Retirement criterion: product decides existing neutral/current ordering is canonical and no prioritization feature is needed.

### Do not
- hide priority in UI sorting;
- use nondeterministic collection order;
- couple merchant priority to unrelated player reputation without a signed rule;
- change restock frequency while solving order.

## 8.2 SignalTrust availability consumer decision

### Current blocker class
Decision-blocked consumer seam. A SignalTrust-like value exists or was planned, but the player-facing/system consumer remains undefined.

### Re-verification sequence
1. Find the canonical SignalTrust writer and determine whether it currently affects any gameplay gate.
2. Enumerate all current readers.
3. Read the original task’s intended “availability” meaning instead of guessing.
4. Identify candidate existing authorities that could legally consume the value.
5. If a consumer already exists, close stale debt.
6. If no consumer exists, write a memo that defines what availability is allowed to gate and which authority owns the decision.
7. Promotion requires a signed consumer contract and a testable player-visible consequence.
8. If the value is purely informational by product choice, record DECIDED-DEFERRED/READ-ONLY.

### Do not
- add arbitrary percentage modifiers;
- let UI directly gate content from the number;
- create a second trust value;
- add save state if the trust value already persists canonically.

## 8.3 C2[2] deferred real gaps

The continuation evidence names four remaining categories: Plan 31 semantic-kind authority, 17C alert ducking/concurrency, Phase E acquisition sweep, and 17B deep test matrix. Treat each as a separate re-verification package.

### Plan 31 semantic-kind authority
- Locate the plan’s semantic-kind concept and current type/schema ownership.
- Prove whether a canonical authority emerged later.
- If two systems independently encode the same semantic kind, the next task is an authority consolidation decision, not a third abstraction.
- Promotion requires: single writer/definition owner, migration impact known, serialization impact known, and focused compatibility tests.

### 17C alert ducking/concurrency
- Locate current alert/audio concurrency policy and the audio bridge.
- Reproduce the specific missing behavior.
- Determine whether the gap is policy, mixer capability, or event-routing.
- Promotion package must name the audio authority and define deterministic priority/ducking semantics.
- No UI-owned volume manipulation.

### Phase E acquisition sweep
- Re-read the original Phase E acceptance.
- Inventory current acquisition/content paths.
- Convert findings into exact missing consumers or data mappings, not a broad “sweep” task.
- Promote only bounded packages with owner, path, and focused test.

### 17B deep test matrix
- Map existing focused tests against the original matrix.
- Treat already-covered rows as closed.
- Promote only uncovered high-value seams.
- No duplicate tests that assert the same behavior through a slower path.

## 8.4 C1.4–C1.10 chain

### Gating rule
Before any C1.4–C1.10 package is claimed:
1. Read current C1 completion/chain documentation.
2. Check worktree ownership for Campaign/DailyBriefing.
3. Re-evaluate dependencies because earlier Wave 8 work may have closed prerequisites.
4. Do not touch a claimed briefing surface.
5. If C1.4 crisis prediction remains blocked only by the concurrent claim, mark WAITING-ON-OWNERSHIP with the exact owner rather than redesigning around it.
6. Promote later C1 tasks only if their predecessors’ outputs are actually available at `HEAD`.

### Crisis-prediction safety
Any predictive surface must consume existing domain truth and deterministic projections. It must not:
- advance simulation to “peek” at future random outcomes;
- duplicate campaign state;
- mutate the daily briefing while computing a preview;
- claim certainty when the underlying system exposes only risk indicators.

## 8.5 Distress follow-up and `audio_cue` content tranches

### Current blocker class
Mechanism-complete/content-empty.

### Re-verification
1. Prove the runtime consumer and schema exist.
2. Count authored rows using the mechanism.
3. Identify whether zero/low usage is intentional.
4. If content is required, promote a bounded authored tranche with validation and content-utilization gates.
5. Keep implementation code unchanged unless the authored data reveals a schema/runtime bug.
6. Audio cue IDs must resolve through the canonical audio registry.
7. Distress follow-up entries must route through the existing event/narrative authority.

### Acceptance
- every new row validates;
- utilization proves runtime consumption;
- no orphan IDs;
- deterministic selection rules preserved;
- no filler content added merely to raise utilization counts.

## 8.6 Test quarantine `Compile Remove` entries

### Current blocker class
Per-file contract ambiguity.

### Re-verification
1. Enumerate only entries still marked `Compile Remove`.
2. For each, locate current production symbol/API and determine why the test no longer compiles.
3. Classify:
   - production regression;
   - stale test contract;
   - obsolete feature/test;
   - package/reference/tooling issue.
4. Stale tests get rematched only when current production truth is independently proven.
5. Obsolete tests require a retirement record.
6. Production regressions become repair packages.
7. Remove quarantine status only after the test compiles and runs in the normal harness.
8. The goal is not “quarantine count zero at any cost”; the goal is every entry dispositioned truthfully.

## 8.7 Cloud seeding data-without-runtime-consumer

### Current blocker class
Authored data exists but runtime consumption is absent or unproven.

### Re-verification
1. Locate the cloud-seeding data schema/catalog rows.
2. Search all runtime reads of the relevant IDs/fields.
3. Determine whether a later weather/environment system already consumes equivalent data.
4. If no consumer exists, identify the intended authority from plan documentation.
5. Do not implement a consumer until the owner and gameplay effect are signed.
6. If the data is obsolete, prefer retirement/removal through data authority over inventing a mechanic to justify it.
7. Promotion requires: owner, effect contract, save implications, deterministic weather interaction, and content-utilization test.

## 8.8 F1/F9 governance decisions

### Current blocker class
Governance/ownership decision.

### Promotion rule
No production implementation until:
- the named governance question is restated from current docs;
- affected owners are identified;
- conflicting responsibilities are listed;
- the foreman signs a single authority outcome;
- existing behavior migration/compatibility is scoped.

Governance tasks are successful when ambiguity disappears. They do not need feature code to count as closure.

---

# 9. CROSS-TASK TEST POLICY

## 9.1 Focused-first ladder

Use the narrowest rung that can falsify the change:

1. single new/changed test;
2. owning test file;
3. owning subsystem directory/suite;
4. relevant selftest;
5. build;
6. fast sanctioned aggregate;
7. full suite only when a named acceptance window or diagnostic hypothesis requires it.

A passing broader suite never substitutes for a missing focused regression test.

## 9.2 Parity tests

Use parity tests whenever the change is an integration or authority migration:
- intact limb behavior before/after C2;
- headless black-market behavior before/after UI exposure;
- map output semantics before/after generator-input reconciliation;
- survivor behavior before/after orphan deletion;
- rendering before/after D3 disposal changes.

Parity assertions should compare authoritative outputs, not incidental log text.

## 9.3 Save/load tests

Required when:
- debt/loan exactly-once state is exercised;
- limb state affects a newly wired consumer after restore;
- a promoted future plan adds a new save scope;
- a transaction introduces persisted delivery/stash state.

Preferred pattern:
1. construct seeded state;
2. advance to a meaningful midpoint;
3. capture save;
4. continue uninterrupted and record authoritative terminal state;
5. restore midpoint in a fresh runtime;
6. replay the same inputs;
7. compare authoritative terminal state field-by-field or through the repository’s canonical fingerprint.

## 9.4 Determinism tests

No new randomness is permitted merely to implement these blockers. Where existing randomness participates:
- same seed must reproduce;
- UI opening/closing must not consume RNG;
- save/restore must not reseed or duplicate side effects;
- modifier composition order must be stable;
- collection ordering must not define policy accidentally.

## 9.5 UI gates

Any changed player surface must pass:
- route/registry reachability when route topology changes;
- bind/unbind lifecycle;
- keyboard navigation;
- focus restoration;
- text explanation for disabled/error states;
- no color-only semantics;
- supported-resolution overflow checks;
- snapshot evidence for intentional visual changes;
- no domain recomputation in the panel.

## 9.6 Generated-artifact gates

When architecture maps, indexes, manifests, or generated matrices are touched:
- modify their source inputs;
- regenerate;
- run `--check` or repository-equivalent;
- review the full diff for unrelated churn;
- never “fix” generated output manually.

---

# 10. DECISION MEMO STANDARD

Every foreman-signature memo in Part 2 uses this exact structure:

## Decision ID
Unique blocker/task identifier.

## Current verified fact
One paragraph containing only repository evidence.

## Why implementation cannot proceed safely
Name the missing authority/policy.

## Existing owners that constrain the decision
Table: owner | current responsibility | relevant API/state | must-not-own.

## Options
For each option:
- behavior;
- owner;
- data/save consequence;
- UI consequence;
- compatibility consequence;
- test consequence;
- retirement/defer consequence.

## Recommended option
Recommendation is architecture-focused and must not hide product tradeoffs.

## Non-options
Explicitly list prohibited shortcuts such as duplicate currency, panel-owned gating, global leak sweeping, or parallel narrative systems.

## Signature
Chosen option, signer, date, and any conditions.

No implementation commit that depends on the decision may precede this signature.

---

# 11. IMPLEMENTATION HANDOFF STANDARD

Every completed package hands off in this order:

1. **Terminal state:** IMPLEMENTED / DECIDED-DEFERRED / RETIRED / VERIFIED-RESOLVED / ROUTED-REPAIR.
2. **Premise:** exact blocker that existed at start.
3. **Current commit/worktree.**
4. **Claims used.**
5. **Decision record**, if any.
6. **Files changed**, grouped by authority.
7. **Behavior before.**
8. **Behavior after.**
9. **Persistence impact.**
10. **Determinism impact.**
11. **UI impact.**
12. **Focused tests and counts.**
13. **Selftests.**
14. **Build result.**
15. **Generated-artifact checks.**
16. **Snapshot/a11y/lifecycle evidence.**
17. **Debt/docs updated.**
18. **Known remaining blocker.**
19. **Rollback point.**
20. **Next safe package.**

A handoff that says only “tests pass” is incomplete.

---

# 12. PART 2 MASTER ACCEPTANCE MATRIX

| Task | Primary blocker | Decision required | Production change allowed | Mandatory close evidence |
|---|---|---:|---:|---|
| C1 | Undefined black-market funds/goods legs + missing action surface | Yes | After signature | settlement contract, action tests, no-reroll, economy suite, UI gates |
| C2 | Missing limb consumers + dishonest avatar placeholder | Yes for equipment/expedition | After signature | equip preflights, intact parity, expedition test, placeholder disposition |
| C3 | 174/175/191 blocked; 192/199 unmapped | Yes | No feature implementation | five signed dispositions, two maps, promoted package entries, docs check |
| D1 | inherited failures + stale truth/map rows | No | stale-contract fixes only | five-row triage, map check, zero/accounted failures, verify-fast |
| D2 | retirement/dead-data decisions not executed | No for recorded retirement | deletion/registration only | zero-consumer proof, build, map/integrity, debt closure |
| D3 | shutdown RID/resource warnings | No | lifetime repair only | classification, repeat-cycle proof, clean/benign shutdown, lifecycle gate |

---

# 13. FINAL CLOSEOUT PROCEDURE

Execute this procedure after all six Part 2 tasks reach a terminal state.

1. Read each task handoff and verify its terminal state is one of the allowed closure states.
2. Confirm every signed decision is linked from the debt/closeout row it resolved.
3. Confirm no task created a new parallel authority.
4. Confirm all generated artifacts pass their checks.
5. Confirm the inherited-failure ledger is zero or every remainder has a repair package.
6. Confirm the architecture map no longer reports known-stale gaps.
7. Confirm the SurvivorInspection retired code is absent unless re-verification discovered a live consumer.
8. Confirm the five dead-data rows have an authoritative register or a separately signed deletion outcome.
9. Confirm black-market UI behavior cannot reroll stock by reopening.
10. Confirm limb integration leaves intact survivors behaviorally unchanged.
11. Confirm the avatar placeholder no longer claims functionality that does not exist.
12. Confirm shutdown warnings are either absent or match a specifically proven/documented benign signature.
13. Re-read the Wave 9 frontier against current `HEAD`.
14. Strike any frontier item already resolved by concurrent work.
15. Split any surviving broad frontier item into bounded packages.
16. Require decisions before promoting any still-unsigned seam.
17. Update the master blocker ledger with only surviving real blockers.
18. Run the sanctioned closeout aggregate (`verify-fast` or repository equivalent).
19. Run build.
20. Produce the Wave 8 Part 2 closeout with the acceptance matrix, evidence links, and exact next promotable package.

---

# 14. NON-GOALS FOR THE ENTIRE PART 2

- No new currency system.
- No duplicate inventory authority.
- No black-market-specific wallet.
- No UI-owned economics.
- No restock-on-refresh control.
- No avatar/animation system.
- No cosmetic amputation illusion presented as gameplay integration.
- No medical ownership of equipment eligibility.
- No medical ownership of expedition speed.
- No speculative New Game+ implementation.
- No second survivor-backstory engine when existing narrative owners already satisfy the need.
- No invented unidentified-item persistence solely to keep Plan 191 alive.
- No hand editing of architecture-map output.
- No weakening tests to erase real regressions.
- No unbounded “pre-existing failure” exemption.
- No deleting live consumers because an old retirement decision exists.
- No deleting authored rows without data-authority approval.
- No global forced-free at shutdown.
- No blanket suppression of Godot resource/RID warnings.
- No Wave 9 implementation before re-verification.
- No feature work inside governance-only tasks.
- No filler content whose only purpose is to make a utilization metric look better.

---

# 15. EXECUTIVE READY-TO-RUN CHECKLIST

Before beginning any task:
- [ ] pull/rebase or otherwise establish current `HEAD`;
- [ ] read the task’s blocker source and current closeout/debt row;
- [ ] re-run source search proving the blocker still exists;
- [ ] record exact current owner paths;
- [ ] check worktree claims;
- [ ] claim paths;
- [ ] identify whether a signature is required;
- [ ] run the narrow baseline tests;
- [ ] record baseline counts;
- [ ] only then edit.

Before declaring completion:
- [ ] original blocker statement is no longer true or is intentionally closed by signed disposition;
- [ ] focused regression tests pass;
- [ ] owner suite passes;
- [ ] build passes;
- [ ] save/load verified if relevant;
- [ ] replay/determinism verified if relevant;
- [ ] UI lifecycle/a11y/snapshot gates pass if relevant;
- [ ] generated artifacts check if relevant;
- [ ] debt/audit/closeout truth updated;
- [ ] no hidden TODO/no-op still claims the feature;
- [ ] handoff lists remaining blockers exactly;
- [ ] next package is safe and bounded.

**End of Part 2.**

# APPENDIX A — EXECUTION CONTROL SHEETS

The following control sheets convert the main task bodies into agent-ready implementation checkpoints. They are intentionally narrower than the task narratives: each sheet names the exact proof expected before editing, the implementation boundary, the minimum test evidence, and the conditions that prevent false closure.

## A.C1 — Black-market action surface control sheet

### Pre-edit proof
- Record the exact `BlackMarketSystem` public methods currently available for Buy, Sell, loan creation, repayment, preflight, stock reads, and underworld tick/restock.
- Record the exact host/session type currently used by `BlackMarketPanel.cs`.
- Record the currency mutation API used by the legitimate market.
- Record the inventory mutation API used by legitimate trade.
- Record whether contraband stock entries resolve directly to canonical item IDs or require an existing stash/location translation.
- Record the current same-day stock identity/fingerprint so UI reopen behavior can be compared later.
- Confirm no other panel or command path already exposes black-market trade actions.

### Signed-design payload
The decision record is incomplete unless it explicitly answers:
1. Which authority debits and credits currency?
2. Which authority removes and grants goods?
3. Is delivery immediate, or does an already-existing stash/location owner mediate delivery?
4. Which operation happens first for Buy?
5. Which operation happens first for Sell?
6. What happens when the second transaction leg fails?
7. Does `BlackMarketSystem` already own the heat/trust side effects, and are they unchanged?
8. Is loan state already persisted by the black-market owner?
9. Is read-only panel refresh guaranteed to be side-effect free?
10. Is “keep the market read-only” an acceptable signed terminal outcome?

### Transaction invariants
- A failed purchase changes neither player currency nor player inventory.
- A failed sale changes neither player inventory nor player currency.
- Stock cannot go negative.
- The panel cannot grant an item itself.
- The panel cannot debit currency itself.
- The panel cannot calculate a substitute price.
- A Buy or Sell result displayed to the player must correspond to the authoritative result returned by the domain path.
- Reopening the panel without an underworld/day transition must not consume RNG, reroll stock, advance interest, or fire overdue handling.
- A loan action may not bypass an existing eligibility/preflight check.
- Repayment may not make debt negative.
- Overdue handling remains exactly once across save/restore.
- A failed action leaves focus and panel state usable.

### Minimum implementation diff
A clean implementation should normally contain:
- one signed decision artifact;
- zero or one narrow Core change, only if the signed settlement leg is genuinely absent;
- one host/session command-routing change;
- one `BlackMarketPanel.cs` action-surface change;
- focused tests for action routing/atomicity;
- snapshot/a11y/lifecycle updates caused by the new controls;
- closeout/debt truth updates.

Any larger diff needs written justification in the handoff.

### Mandatory negative tests
- Buy with insufficient funds.
- Buy when stock is unavailable.
- Buy when an existing access/tier preflight rejects the action.
- Sell when the player lacks the required quantity.
- Loan attempt when the existing policy rejects another loan.
- Repay with insufficient funds if that is a reachable domain state.
- Close and reopen on the same day.
- Save/restore while debt is active, then advance through the overdue edge.
- Trigger an action, close the panel, reopen, and prove there is no duplicate result event.
- Trigger a failed action and prove neither transaction leg partially committed.

### Closure evidence
The C1 closeout must contain a table with columns:
`Action | Core method | Currency owner | Goods owner | Failure result | Side effects | UI control | Focus behavior | Test`.
Every row must reference current implementation evidence. If the signed outcome is read-only, the table contains a single disposition row explaining why no actions are wired and why the blocker is considered decided rather than forgotten.

---

## A.C2 — Amputation/equipment/expedition control sheet

### Pre-edit proof
- Enumerate every `LimbCondition` writer and reader.
- Record the canonical persisted representation of limb state.
- Record the equipment owner’s current equip preflight entry point.
- Record how weapon handedness or slot requirements are represented today.
- Record whether prosthetics already exist as canonical items/state; do not infer them from design prose.
- Record the expedition owner’s current route-speed/modifier composition path.
- Record every call to `RefreshSurvivorVisuals`.
- Record whether any real avatar owner now exists.
- Capture an intact-survivor equipment and expedition fingerprint before changes.

### Contract matrix requirement
The signed contract must be expressed as a table with:
`Limb state | Equipment implication | Equipment owner action | Expedition implication | Expedition owner action | Persistence change | UI reason`.

Rows are allowed only for states that exist in the current canonical model. If the repository has only broad intact/amputated state, the contract cannot invent prosthetic grades or rehabilitation stages.

### Equipment invariants
- Medical/amputation code remains the writer of limb state.
- Equipment code owns equip eligibility.
- UI merely displays the equipment preflight result.
- An intact survivor’s legal equipment set is unchanged.
- Unrelated equipment categories remain unaffected.
- Restoring an old save immediately produces the correct equip preflight result from restored limb state.
- No equipment rule is duplicated in `AmputationTriagePanel`.
- A disabled equip action explains the reason in text.

### Expedition invariants
- Expedition/travel owns speed and route composition.
- The amputation system does not write route speed.
- The modifier is derived from canonical limb state when possible.
- Intact survivor travel remains unchanged.
- Modifier application is exactly once.
- Composition with weather, burden, injury, vehicle, or other existing modifiers follows the expedition owner’s existing order.
- Save/load does not need a new field when the effect is derivable.

### Avatar-placeholder disposition
The placeholder portion closes only when one of these is true:
- the dead `RefreshSurvivorVisuals` path is removed because it has no consumer; or
- a necessary compatibility hook remains but is an explicit documented no-op that does not print a false success claim.

The task does **not** close by creating a quick portrait swap. The prior product decision keeps actual avatar integration out of scope until a real owner exists.

### Mandatory tests
- Intact limb + ordinary equipment = legacy result.
- Signed affected limb state + affected equipment class = explicit preflight rejection.
- Signed affected limb state + unaffected equipment class = remains legal.
- Save/restore of affected survivor = identical preflight result.
- Intact expedition = identical fingerprint.
- Affected expedition = only the signed route modifier changes.
- Repeated route evaluation does not stack the limb modifier.
- Equipment panel shows the reason and remains keyboard operable.
- Placeholder deletion/stub produces no runtime path claiming avatar refresh happened.

### Closure evidence
Provide three matrices:
1. `Limb state → equipment result`.
2. `Limb state → expedition result`.
3. `Avatar placeholder call site → final disposition`.
Debt closure is not allowed if any matrix row still says “TODO”, “later”, or “assumed”.

---

## A.C3 — Endgame portfolio decision control sheet

### Required current-state evidence per plan
For each of 174, 175, 191, 192, and 199 capture:
- historical intent;
- current code owners that overlap it;
- current player-visible capability;
- missing unique capability, if any;
- persistence impact;
- likely authority owner;
- collision risk;
- disposition candidate;
- exact condition required before implementation.

### Plan 174 decision standard
A promotion is justified only if the current survivor profile/arc/echo stack lacks a distinct capability that the plan can add without a parallel narrative authority. The memo must answer:
- Is generated backstory stored, derived, or purely presentation?
- Does any gameplay system consume it?
- Is it deterministic under survivor-generation seed?
- Does it overlap current arcs, echoes, relationships, or profile text?
- Which current owner would absorb the capability?
- What is the smallest package that proves usefulness?

If the only remaining value is duplicate flavor text, RETIRE is the truthful result.

### Plan 175 decision standard
Before PROMOTE:
- cross-run vs run-local state must be separated;
- the owner of meta-state must be named;
- versioning must be specified at package level;
- reset/new-profile semantics must be acknowledged;
- ending-gated unlock inputs must be named;
- existing campaign saves must remain readable;
- no carry-over balance rule is silently chosen.

If these conditions cannot be met, HOLD with a named condition is preferable to speculative implementation.

### Plan 191 decision standard
The review must separate:
- ordinary inspection;
- identification state;
- appraisal/value estimation;
- market price explanation.

A plan is not promoted merely because historical prose says “identification/appraisal”. Promotion requires a current, unsatisfied, distinct capability. If `ItemInspectionModel` plus existing economy valuation already answers the player need, retirement or scope reduction is valid.

### Plans 192 and 199 mapping standard
The family maps must identify candidate authority seams without implementing:
- command owner;
- state owner;
- save owner;
- runtime tick/event owner;
- data source;
- presentation surface;
- dependencies;
- collisions;
- unresolved decision.

A map that says only “caravan system” or “wildlife system” without locating the current owner path is insufficient.

### Disposition syntax
Use exactly one:
- `PROMOTE — <first package id/title>`
- `RETIRE — <reason current architecture/product no longer needs it>`
- `HOLD — <specific condition that reopens review>`

Avoid “maybe”, “later”, “pending”, or “TBD” as final dispositions.

### Closure evidence
The portfolio closes when:
- all five rows have signatures;
- 192/199 have current family maps;
- every promoted plan has a first package in the integration ledger;
- every retired plan has a status banner;
- every held plan has a measurable trigger;
- docs index/check passes;
- production diff is empty.

---

## A.D1 — Verification-truth control sheet

### Five-failure triage table
Use these columns:
`Failure ID | Test target | First known handoff | Current repro | Classification | Production truth source | Action | Owner package | Final status`.

No row may end as “pre-existing”.

### Classification rules
**STALE CONTRACT**
- current production behavior is independently proven correct;
- docs/current model show the assertion represents older truth;
- changing the assertion does not hide an actual invariant break;
- a drift comment states why old expectation changed.

**REAL REGRESSION**
- current behavior violates an active contract/invariant;
- test remains valid;
- create repair package and leave D1 implementation scope.

**ALREADY FIXED**
- isolated target passes at current `HEAD`;
- capture result and strike inherited failure status.

### Source-comment truth pass
A candidate comment may be changed only when:
- an authoritative completion log or source implementation proves it false;
- the edit is limited to truth/status;
- no code rename or plan-number reshuffle is performed.

A live TODO stays live.

### Architecture-map reconciliation rules
- run generator against current source;
- compare output to known stale examples;
- trace stale rows to graph/registry/fixture input;
- fix source input;
- regenerate;
- inspect diff;
- route genuine gaps elsewhere.

Do not convert a true gap into “implemented” merely to reduce the GAP count.

### Baseline release rule
The release checklist must expose:
- standing failure count;
- exact identities if nonzero;
- owner package for each;
- date/commit last reproduced.

This turns “pre-existing failures” from an exemption into an accountable ledger.

### Closure evidence
D1 is complete only when:
- all five inherited failures are classified;
- stale tests are green;
- real regressions have repair owners;
- known stale source header is corrected;
- architecture map passes check;
- known stale rows are reconciled;
- verify-fast and build pass at the reconciled baseline.

---

## A.D2 — Recorded-decision execution control sheet

### SurvivorInspection zero-consumer proof
Search these categories:
- direct constructions;
- dependency injection/composition registration;
- interface implementation references;
- serializer/type registries;
- panel/host bindings;
- tests;
- reflection/type-name lookup;
- source generators;
- docs only.

The retirement can execute only if all runtime-consumer categories are empty.

### Deletion package boundary
Allowed:
- delete retired projection types;
- delete tests whose only purpose is asserting that duplicate projection;
- remove stale registration/generator input referencing the deleted type;
- update debt/audit truth.

Not allowed:
- redesign canonical survivor inspection;
- migrate unrelated callers opportunistically;
- delete canonical survivor fields;
- change save schema unless a real hidden serialization dependency is discovered and separately scoped.

### Dead-data register schema
Each of the five rows should contain:
`Row ID | Catalog/file | Primary winner | Why this row can never win | Runtime impact | Warning signature | Retention reason | Delete authority | Recheck condition`.

The register’s purpose is to make intentional dead data distinguishable from accidental unreachable data.

### Warning discipline
Integrity output is acceptable only if:
- expected warnings correspond exactly to registered intentional rows;
- any additional warning is treated as a new finding;
- removed warnings are explained by an authorized deletion/change.

Do not broaden “known warnings” to silence novel problems.

### Sibling sweep output
The sibling sweep produces candidates only. For each:
`Debt ID | Recorded decision | Current precondition | Evidence | Suggested execution package`.
Do not execute them inside D2 unless separately claimed and approved.

### Closure evidence
- zero-consumer proof stored;
- retired files absent;
- build passes;
- survivor tests pass;
- architecture map passes;
- dead-data register covers exact five rows;
- integrity warnings match the register;
- debt/audit rows say executed, not merely decided.

---

## A.D3 — Leak-triage control sheet

### Capture requirements
Each reproduction log records:
- command/selftest;
- commit;
- headless/display mode;
- exact warning text;
- resource/RID count;
- exit code;
- panels/surfaces opened;
- cycle count;
- node/resource count before and after if the diagnostics guide exposes it.

### Classification evidence
**Node leak**
- node/tree count grows or objects remain attached after expected close;
- offending surface/lifecycle path identified.

**Signal/subscription leak**
- subscriber survives node lifetime or repeated open/close duplicates callbacks;
- missing disconnect/unsubscribe identified.

**Resource leak**
- retained texture/buffer/resource count grows or RID remains owned past teardown;
- allocator/owner identified.

**Benign shutdown-order noise**
- repeated operational cycles return to baseline;
- memory/node/resource telemetry remains flat;
- warning appears only during final process teardown;
- exact known signature documented.

### Repair constraints
- fix the owning teardown method;
- maintain bind/unbind symmetry;
- release only resources the owner owns;
- avoid global singleton “free everything” routines added solely to silence shutdown;
- avoid swallowing stderr/warning text;
- avoid changing selftest success criteria to ignore all warnings.

### Repeat-cycle gate
A useful cycle gate should:
1. establish baseline;
2. open the implicated panel/surface;
3. bind;
4. exercise representative interaction;
5. close/unbind/free;
6. allow deferred frees/lifecycle completion;
7. sample counts;
8. repeat N times;
9. assert no monotonic growth and return-to-baseline within the guide’s tolerated semantics.

### Cross-check surfaces
After a fix, run every test path sharing the lifetime mechanism:
- a11y panel instantiation;
- panel bind lifecycle;
- settings panels if common shell is involved;
- audio bridge if signal subscriptions are implicated;
- snapshot/capture path if buffers/resources are implicated;
- export smoke or equivalent release shutdown path.

### Closure evidence
The final leak table contains:
`Warning signature | Reproducer | Classification | Owner | Root cause | Fix or benign proof | Repeat-cycle result | Release-path result`.

The release note may say “known benign” only for exact signatures proven under this procedure.

---

# APPENDIX B — PART 2 REVIEW GATES

## B.1 Architecture review
Before merge, answer yes/no:
- Is there exactly one writer for each state introduced or consumed?
- Did any panel gain domain logic?
- Did any task invent an authority because the expected one was missing?
- Did any task add persistence that could have remained derived?
- Did any task change balance while claiming to perform parity integration?
- Did any task hand-edit generated output?
- Did any task turn an unsigned decision into code?
- Did any task silently broaden scope because a neighboring gap was convenient?

Any “yes” to a prohibited pattern blocks merge.

## B.2 Save compatibility review
For C1/C2 and any future promoted package:
- identify all new/changed persisted fields;
- identify default behavior for old saves;
- identify capture and restore owner;
- run round-trip;
- run mid-flow restore;
- compare terminal state;
- confirm exactly-once ledgers remain stable;
- document schema version only when actually changed.

If there are no persisted changes, state that explicitly and prove the new behavior derives from existing saved state.

## B.3 Player-observable truth review
For every UI-related change:
- Is the displayed state authoritative?
- Can the player understand why an action is disabled?
- Does action feedback reflect the real domain result?
- Does reopening the panel change simulation state accidentally?
- Is focus predictable?
- Is keyboard-only operation possible?
- Does color have a text/symbol counterpart?
- Do snapshots show intended changes only?

## B.4 Debt-ledger review
Every touched blocker row must end in a truthful state. Never leave:
- BLOCKED after its decision is signed;
- PARTIAL after all named acceptance gates pass;
- RETIRED while duplicate code still exists;
- “pre-existing failure” without identity/owner;
- “GAP” when the generated architecture evidence proves the route exists;
- “implemented” when only documentation changed.

## B.5 Handoff quality review
A reviewer should be able to answer from the handoff alone:
- what was blocked;
- why;
- what decision was required;
- what changed;
- which authority owns it;
- what remained unchanged;
- which tests prove it;
- whether save/load changed;
- whether player-visible behavior changed;
- what remains open.

If the reviewer must rediscover these facts in the repository, the handoff is incomplete.

---

# APPENDIX C — NEXT-WAVE PROMOTION TEMPLATE

Use this template for each Wave 9 survivor after re-verification:

## `<ID> — <title>`

**Live blocker:** exact statement verified at `HEAD`.

**Current owner:** exact system/type/path.

**Why still blocked:** missing decision / missing consumer / missing package / content-empty / test-contract ambiguity / ownership conflict.

**What changed since prior wave:** current evidence only.

**Promotion type:** implementation / decision / mapping / verification / content tranche / repair.

**Claim set:** exact paths to be claimed.

**Forbidden neighboring scope:** list adjacent systems not owned by this package.

**Acceptance:**
1. observable result;
2. focused test;
3. save/load requirement;
4. determinism requirement;
5. UI requirement;
6. generated-data requirement;
7. closeout/debt update.

**Stop conditions:** missing owner, concurrent claim, stale premise, unsigned product behavior, persistence scope not approved.

**Terminal outcomes:** IMPLEMENTED / DECIDED-DEFERRED / RETIRED / VERIFIED-RESOLVED / ROUTED-REPAIR.

This template prevents Wave 9 from becoming a backlog dump. A frontier item is promoted only when its live blocker is proven and bounded.
