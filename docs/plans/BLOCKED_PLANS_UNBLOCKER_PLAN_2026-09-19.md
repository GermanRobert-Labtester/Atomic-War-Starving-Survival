# ASHFALL — Unblocker Plan for the Blocked Plan Queue (2026-09-19)

**Role:** read-only planning pass, no production/data/test change.
**Repo state:** branch `Zcode_Branch`, worktree as of 2026-09-19 (post the
2026-09-18/19 completion-first execution wave). Verified against current
source, not against old doc claims (AGENTS.md Rule 7).
**Input authorities:** `AGENTS.md` (active queue), `INTEGRATION_PLANS.md`,
`WORKTREE_OWNERSHIP.md`, `KNOWN_DEBT.md`, `docs/governance/DECISION_REGISTER.md`,
`docs/governance/DECISION_PACKET_2026-09-18.md`,
`docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md`,
`docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, `docs/plans/wave8_part2/C3_DECISION.md`,
the XP expansion proposal (`Seal-steps/ashfall-feature-expansion-proposal-part-1...md`).

This is a governance/decision plan, not a single-feature integration plan, so
it adapts the standard planning template: instead of Core/Data/Save sections
for one feature, it produces **signature bundles** — the smallest sets of
foreman decisions that unblock the largest amount of queued work per edit to
a shared file, in dependency order — plus the execution package each bundle
releases.

---

## 1. Objective

Turn the ASHFALL plan queue's blocked items into either (a) a signed decision
with a named execution package, or (b) an explicit decline/defer with a
recheck trigger — so no plan sits in limbo without an owner and a next step.
This plan does not implement any package; it sequences the signatures needed
to release them and names who executes what once signed.

## 2. Non-goals

- Does not re-litigate anything already `SIGNED`/`RETIRED` in
  `DECISION_REGISTER.md` or `KNOWN_DEBT.md`.
- Does not implement XP-04, XP-06, EN-01…08, Plan 49, or any C3 HOLD.
- Does not edit `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, or
  `KNOWN_DEBT.md` (foreman/integrator-only per `AI_AGENT_WORKFLOW.md`).
- Does not treat the 90KB+ `Seal-steps/*.md` proposal documents as certified
  authority — several duplicate each other and predate the 2026-09-19
  execution wave; only current source and the dated governance docs above are
  used as evidence here.

## 3. Current reality — what is already resolved (do not re-sign)

The 2026-09-18/19 execution wave already closed most of the original 23-item
decision packet. Re-verified today:

| Item | Resolution | Evidence |
|---|---|---|
| D1, D2 (Plan 24 ward staffing + recovery ramp) | Sealed, option (b) / (ii) | `KNOWN_DEBT.md` → `DEBT-PLAN24-MEDICAL-WARD-STAFFING` RETIRED |
| D5, D6, D7 (Plan 30 clock + consequence-reach + 30C scope) | Sealed | `KNOWN_DEBT.md` → `DEBT-PLAN30-RUNTIME-CLOCK`, `DEBT-PLAN30-CONSEQUENCE-REACH` RETIRED |
| D8, D9, D10 (Plan 32 orphans + graph travel + knowledge gating) | Sealed | `KNOWN_DEBT.md` → `DEBT-PLAN32-MAP-ORPHANS`, `DEBT-PLAN32-GRAPH-TRAVEL` RETIRED |
| D14 (Plan 123 hostile-fire emitter) | Sealed | `KNOWN_DEBT.md` → `DEBT-PLAN123-SOUND-RANGING-PRODUCER` RETIRED |
| D15 (SOFC fuel owner) | Sealed, inventory | `KNOWN_DEBT.md` → `DEBT-PLAN125-SOFC-INVENTORY-FUEL` RETIRED |
| D23 item 1 (Plan 28 constructor migration) | Sealed | `KNOWN_DEBT.md` → `DEBT-PLAN28-MAIN-CONSTRUCTOR-MIGRATION` RETIRED |
| G1 (4 missing Wave 11 debt rows) | Fixed — the 4 rows exist and are RETIRED | `KNOWN_DEBT.md` |
| G4 (`AGENTS.md` misdirecting agents to finished work) | Fixed — current `AGENTS.md` carries the current queue, no stale AGY handoff | verified by reading current `AGENTS.md` |

Confirmed still blocked today (re-verified, not assumed from the packet):

- **D21 quarantine drain:** `grep -c 'Compile Remove' Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` → 55 total, 6 commented, **48 active** (matches `AGENTS.md`'s current count, not the packet's stale "50").
- **C1[16]/Plan 49 prerequisites:** `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` still lists `C2[18]`/Plan 42 and `C2[20]`/Plan 46 as `AUDIT-PENDING` with no verdict — Plan 49 is correctly gated.
- **G5 gitattributes:** `.gitattributes:11-63` still declares `unity-json`/`unity-yaml` custom attributes and applies them to `*.asmdef`, `*.anim`, `*.mat`, `*.prefab`, etc. — a live, if inert, contradiction of Rule 1. Not a decision item, but a housekeeping defect worth folding into the same pass since it touches the same governance file family.

## 4. Required delta

Close every still-open item from `DECISION_PACKET_2026-09-18.md` §3–§7 and
`UNBLOCKED_PLANS_AUDIT_2026-09-19.md` §4 with an explicit sign-off line,
bundled so that items touching the same file are signed together, then hand
each signed bundle to its named execution package.

## 5. Signature bundles, in dependency order

Bundling rule: group by shared file/owner to avoid two builders editing the
same seam in sequence, and put pure one-line policy calls first so they don't
block anything behind them.

### Bundle A — Pure policy, zero blast radius (sign first, unblocks nothing else but costs nothing)

| Item | Sign-off line | Unblocks |
|---|---|---|
| D4 | `I ratify DEC-15 (WeatherStationSystem is the sole forecast authority; radio weather stays diegetic).` | Closes the register's own "zero unsigned" invariant gap — no code change |
| D19a | `Pre-campaign barter: [allowed / must not be constructible].` | `src/Main.Plans147.cs` fallback RNG policy only |
| D13 | `Plans 126-129: [(a) numbering drift, fix the comment / (b) genuinely un-built, open a plan / (c) leave as-is].` | Closes a 2-year-old ambiguity; (a) is a one-line comment fix, (b) opens a new plan, (c) is free |
| D21 | `Quarantine drain: [authorise a per-file batch / not worth it / correct the ledger row only].` | If authorised: `TEST_POLICY.md`-governed per-file re-enable of the 48 active `Compile Remove` entries |

No execution package needed for D4/D19a/D13(c)/D21(decline) — they are
documentation-only closures. D13(b) and D21(authorise) each spawn a scoped
follow-up package sized to one file at a time, per `TEST_POLICY.md`
quarantine rules.

### Bundle B — The self-contradiction, sign alone

| Item | Sign-off line | Unblocks |
|---|---|---|
| D3 | `water_sample_contaminated: I choose [Option A strip / Option B retain as lore], overriding the conflicting record in [name the document].` | `Assets/StreamingAssets/Data/items.json:3411-3427`; Option A touches `Plan21ProtectiveWearTests` count + a save-migration note; Option B is docs-only |

Kept separate because it contradicts itself across three governance
documents (`WATER_SAMPLE_CONTAMINATED_DECISION_MEMO.md`,
`WAVE10_MICRO_DEFERRAL_SWEEP.md`, `DECISION_REGISTER.md` DEC-09) and shares
`items.json` with Bundle D (D12/F14) — sign D3 before D12 so both edits to
that catalog region land in one pass, not two.

### Bundle C — String freeze (sequencing gate, sign before touching EN-05/XP-09/localization)

| Item | Sign-off line | Unblocks |
|---|---|---|
| D22 | `String freeze: [declared, scope+date / deferred again].` | `DEC-11` (VO pipeline) and `DEC-13` (localization) recheck triggers both depend on this; also gates `EN-05` (Signal Continuity & Voice) |

Declaring a scope+date here is what turns two indefinitely-parked decision
rows into a scheduled follow-on wave. Deferring again costs nothing but keeps
both rows parked.

### Bundle D — Equipment schema (D12/F14, largest single-catalog blast radius after D8, already-resolved)

| Item | Sign-off line | Unblocks |
|---|---|---|
| D12 (= F14 / XP-06's named blocker) | `Amputation equipment schema: [signed — author the handedness/limb-requirement package / declined — amputation permanently does not restrict gear].` | `ItemDefinition`/`EquipSlot` schema, `Inventory.cs` equip preflight, every 2-handed weapon + boot row in `items.json`, `AmputationSystem`, `CatalogIntegrityValidator`, `Plan21ProtectiveWearTests`, `C2AmputationTravelTests`; downstream: all of `XP-06-BODY-INTEGRITY` (prosthetics catalog, rehab arc, equip gating) |

This is the single decision the whole `XP-06` pillar and `EN-04`
(Rehabilitation Medicine) are waiting on. Sign it with an explicit grip-class
vocabulary (proposal on record: `simple` vs `full` — two classes only) so the
schema isn't invented ad hoc at implementation time.

### Bundle E — Economy funds authority (F13, gates three XP pillars + one EN)

| Item | Sign-off line | Unblocks |
|---|---|---|
| F13 (`XP-04-ECONOMY-LEGS` design sign-off) | `FundsLedger canonical funds authority: [signed / declined]`; `Merchant restock priority design: [signed as proposed / amend weights]`; `Wave-1 opt-in surfaces: [black market + caravan only / other]` | `XP-04` (black-market trade legs, heat/attention, fence purity); downstream: `XP-07` (sequenced after XP-04), `XP-08` (hard-depends on XP-04 funds), `EN-03` (Underground Economy Pressure) |

Three sub-lines because the proposal (`ashfall-feature-expansion-proposal...`
§XP-04.8) names three separate foreman calls. Sign together — same
`FundsLedger` file, same session.

### Bundle F — Radio/design micro-decisions (small, independent, batch for one editing pass)

| Item | Sign-off line | Unblocks |
|---|---|---|
| D16 | `Flooded-route tags: [author them / re-audit first / declined].` | Premise unverified — **re-audit `wasteland_map_v1.json` for flooded/amphibious tags before signing**, per the packet's own caveat. Shares the map file with the already-sealed D9, so batch any resulting data tranche with the next map-touching package, not a new one. |
| D19b | `Station identity: [author a station-selection seam / keep synthetic station_alpha / re-audit].` | `RadioHostSession`, triangulation panel, persisted observation records |
| D19c | *(not a decision — an audit action)* verify whether `_silentFoundry`/`_sharedFactionStance`/`_sharedSkillProgression` (present in `src/Main.UiPanels.cs`, `Main.GameFlow.cs`, `Main.CampaignServices.cs`, `Main.PlayerSurfaces.cs`, `Main.CampaignOwners.cs`, `Main.UiTests.SilentFoundry.cs`, `Main.ExpansionHub.cs`, `Main.PanelLifecycle.cs`, `Main.Economy.cs`) are correctly reset on new-game vs load, before any Plan 16 closure claims 16B.9 done for them | If it verifies a real defect: routes to a bounded repair package, not a decision |

### Bundle G — C3 endgame HOLDs (four independent product/architecture calls, sign whichever the roadmap needs next — no forced order between them)

| Plan | Sign-off line | Type | Unblocks |
|---|---|---|---|
| 174 | `174: [lift — name the extension point on SurvivorEnrichmentService/TradeSpecialtySystem / hold]` | architecture-owner naming | mechanical origin effects |
| 175 | `175: [lift — name the cross-run profile-store owner / hold]` | **product call** (Meta Profile / NG+) | requires Plan 34/149 completion-fact producers already verified (they are, per `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY` RETIRED) |
| 192 | `192: [lift — sign the player-route DTO with standing/raid/save seams in a map amendment / hold]` | schema, must not touch NPC caravan owners | player trade routes |
| 199 | `199: [lift — name a human population owner distinct from fauna / hold]` | **product call** | seasonal human/faction migration |

These four are independent of each other and of Bundles A–F; sequence them by
roadmap priority, not technical dependency.

### Bundle H — EN-01…EN-08 authorizations (each is a standalone foreman go/no-go; list current prerequisite state, not a recommendation)

| Item | Prerequisite state (verified 2026-09-19) | Authorization line |
|---|---|---|
| EN-01 Difficulty-Consequence Weave | Builds on `XP-WAVE1-DIFFICULTY-AUTHORITY` (active, partial — Bundle-independent, see §6) and sealed Plan 30/34 | `EN-01: [authorize / not yet].` |
| EN-02 The Living Map | Builds on sealed Plan 32 (graph travel) + sealed Plan 30 (war fronts) | `EN-02: [authorize / not yet].` |
| EN-03 Underground Economy Pressure | **Hard-blocked on Bundle E (F13)** | `EN-03: [authorize once F13 signs / not yet].` |
| EN-04 Rehabilitation Medicine | **Hard-blocked on Bundle D (D12/F14)** | `EN-04: [authorize once D12 signs / not yet].` |
| EN-05 Signal Continuity & Voice | Builds on `CF-P1-DISTRESS-CONTENT-SEAL` (available now, §6) + **gated by Bundle C (D22)** for the voice/VO half | `EN-05: [authorize / not yet].` |
| EN-06 One Bootstrap Path | Builds on `CF-P28-ONE-BOOTSTRAP-PATH` (available now, §6) | `EN-06: [authorize / not yet].` |
| EN-07 Chronicle & Aspiration Readiness | Builds on sealed Plan 34 + XP-01 partial + `E1`/Plan 53 (available now, §6) | `EN-07: [authorize / not yet].` |
| EN-08 Ledger Truth Program | Builds on `CF-P5-RESTOCK-RECONCILE` (available now, §6) + **gated by Bundle A (D21)** for the quarantine-drain leg | `EN-08: [authorize / not yet].` |

EN-03/EN-04/EN-05/EN-08 are listed as blocked-on-bundle so the foreman does
not authorize them before their named bundle signs — doing so would just
recreate the same blocker one layer up.

### Bundle I — Plan 49 (no signature possible yet; audit-only path)

Plan 49 (`C1[16]`) cannot be signed into this packet: its own prerequisites
(`C2[18]`/Plan 42, `C2[20]`/Plan 46) are `AUDIT-PENDING`, not decision-blocked.
The correct next action is a premise audit of Plan 42 and Plan 46 (read-only,
sweep-role), not a foreman signature. Route it as: **audit Plan 42 → audit
Plan 46 → if both certify real gaps, then and only then does Plan 49 enter a
future decision packet.**

## 6. Companion fast path — already-available, no signature needed

Independent of every bundle above, `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md`
§3 lists 8 packages that need no new foreman signature (2 need only the
standard premise audit). They are not re-derived here; they are the
non-decision-gated half of the same queue and can execute in parallel with
any bundle above without file collisions, per `WORKTREE_OWNERSHIP.md`:

`CF-P1-DISTRESS-CONTENT-SEAL`, `CF-P5-RESTOCK-RECONCILE`,
`CF-P6-VEHICLE-ARMOR-GRADES`, `CF-P28-ONE-BOOTSTRAP-PATH`,
`CF-XP01-DIFFICULTY-FULL-BINDING`, `E1`/Plan 53, `C2[15]`/Plan 37,
`C2[21]`/Plan 48.

## 7. Ownership matrix

| Concern | Owner |
|---|---|
| Drafting/copying sign-off lines | Foreman (user) |
| Recording signed verdicts | Foreman/Integrator → `DECISION_REGISTER.md` (new `DEC-` rows for D3/D4/D11/D12/D13/D16/D19a/D19b/D21/D22, C3 lift rows, EN-01…08 authorizations) |
| Executing a bundle's named package after signature | Builder, one package at a time, disjoint paths per `WORKTREE_OWNERSHIP.md` |
| Plan 42/Plan 46 premise audits | Sweep role (read-only) |
| `.gitattributes` Unity-attribute cleanup (G5) | Builder, bounded — remove `unity-json`/`unity-yaml` attribute declarations and their file-pattern rows once confirmed no `.asmdef`/`.anim`/`.mat`/etc. files remain tracked |
| This plan document itself | Read-only deliverable; does not require a signature to exist, only to act on |

## 8. Risks

- **Re-signing already-sealed items.** D1/D2/D5–D10/D14/D15/D23-item-1 are
  closed; presenting them again in a new packet would waste a signature and
  contradict `KNOWN_DEBT.md`. §3 of this plan exists specifically to prevent
  that.
- **Signing D8-adjacent items without naming multiplier composition order.**
  Already resolved (D8 sealed), but any *new* travel-multiplier consumer
  (e.g., from EN-02) must still declare where it sits relative to
  `survivorSpeedMultiplier` and `weatherSpeedMultiplier` — carry this
  constraint forward into EN-02's own premise check.
- **Signing D3 or D12 out of order** re-touches the same `items.json` region
  twice. Bundle B before Bundle D, or combine them in one editing session.
- **Authorizing EN-03/EN-04/EN-05/EN-08 before their gating bundle signs**
  recreates the blocker one layer up in a new plan name.
- **Treating Seal-steps proposal documents as certified.** Several
  90KB+ documents in `Seal-steps/` duplicate and partially contradict this
  packet's evidence (they predate the 2026-09-19 seals). Do not cite them as
  authority in a signed decision; cite `KNOWN_DEBT.md`/`DECISION_REGISTER.md`/
  current source instead.

## 9. Out of scope

- Implementing any bundle's named package (separate builder packages after
  signature).
- Re-auditing items already marked `SIGNED`/`RETIRED`.
- The 109 census `AUDIT-PENDING` rows not named above — those need
  tranche-2 per-clause audits, not signatures.
- The environment-blocked Plan 24 snapshot rebaseline (`D23` item 3) —
  needs a renderer-capable session, not a decision.

## 10. Rollback / decline path

Every bundle above has an explicit decline option in its sign-off line
(`declined`, `not yet`, `hold`, `deferred again`). Declining costs nothing —
the item stays `DEFERRED-WITH-CONDITION` or `HOLD` with its existing recheck
trigger untouched. No bundle requires an irreversible commitment; schema
bundles (D3, D12, D19b) are the only ones with real blast radius, and each
names its blast radius before the sign-off line so the foreman can decline
with full information.

## 11. Definition of done

- Every item in `DECISION_PACKET_2026-09-18.md` §3–§7 and
  `UNBLOCKED_PLANS_AUDIT_2026-09-19.md` §4 has either a dated sign-off quote
  in `DECISION_REGISTER.md` or an explicit "re-audit first" routing (D16,
  D19c, Plan 49's Plan 42/46 audits).
- `DECISION_REGISTER.md`'s own invariant ("zero items unsigned without a
  condition") is true, not aspirational.
- No bundle is signed while its blast-radius file is mid-edit by another
  active claim (check `WORKTREE_OWNERSHIP.md` immediately before signing,
  not just before this plan was written).

## 12. Implementation handoff

**MUST PRESERVE:** every already-sealed decision in §3; do not reopen without
a new documented drift, per each debt row's own promotion condition.

**MUST ADD:** one dated sign-off line per still-open item, routed through
`DECISION_REGISTER.md`, before any builder touches the file(s) that item's
row names.

**MUST NOT DO:** sign D3/D12 in separate sessions when they share
`items.json`; authorize an EN-item ahead of its gating bundle; treat a
`Seal-steps/*.md` proposal document as equivalent to `KNOWN_DEBT.md` or
`DECISION_REGISTER.md` evidence.

**VERIFY WITH:** for each executed package, its own focused test file per
`TEST_POLICY.md` (`bash scripts/run_test.sh <path>`); no full-suite run by
default.

**FIRST SAFE STEP:** present Bundle A (D4, D19a, D13, D21) to the foreman —
zero blast radius, closes the register's standing-invariant gap, and costs
nothing if declined.
