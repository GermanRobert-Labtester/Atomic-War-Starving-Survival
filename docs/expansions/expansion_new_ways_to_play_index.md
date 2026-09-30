# ASHFALL — "NEW WAYS TO PLAY": Family Index & Boundary Sheet
**Status:** Story-director coordination sheet. **Proposal — not a claim, not an authorization.** 2026-09-29.
Covers subjects 8–12 of the user's list. Sibling sets: Year Two (Days 361–720) and "The world moves without you" (subjects 4–7, `expansion_world_moves_without_you_index.md`).

| # | Expansion | Prose plan | Integration plan | Prefix |
|---|---|---|---|---|
| 8 | **Radio Free Ashfall** — run your own broadcast | `expansion_radio_free_ashfall_plan.md` | `.ai/plans/radio-free-ashfall-2026-09-29.md` | `RF-` |
| 9 | **The Reconstruction Tree** — research and rebuild lost knowledge | `expansion_reconstruction_tree_plan.md` | `.ai/plans/reconstruction-tree-2026-09-29.md` | `RT-` |
| 10 | **Shelter Governance: The Assembly** — laws, courts, internal opposition | `expansion_shelter_governance_plan.md` | `.ai/plans/shelter-governance-2026-09-29.md` | `SG-` |
| 11 | **The Quiet War** — espionage, informants, false door visitors | `expansion_quiet_war_plan.md` | `.ai/plans/quiet-war-2026-09-29.md` | `QW-` |
| 12 | **Crews and Companions** — expeditions become parties | `expansion_crews_and_companions_plan.md` | `.ai/plans/crews-and-companions-2026-09-29.md` | `CC-` |

All five are `STATUS: DRAFT — awaiting user approval`. No per-package derived plans were written.

> *"A new way to play is not a new button. It is a reason to press one of the ones you already have."*
>
> Read these five together and a pattern appears: every one of them is **a verb the shelter already
> had, given somebody to point at**. Broadcasting existed; now it has a call sign. Vetting existed;
> now it has a visitor. Research existed; now it has *bearers* who can die. The machinery is not
> new. The stakes of pulling the handle are.

## 1. What these five have in common
Unlike subjects 4–7, **the machinery mostly exists**: radio has a production system, PsyOps and triangulation; research has fragments-in-waiting (salvage, archives, decay); governance has *five* owners; espionage has agents, informants and vetting; companions have care and grief. Each plan therefore **connects existing owners and adds one small ledger** — none adds a second authority.

## 2. What each one found (the central gap)

| Plan | Central gap (verified by reading) |
|---|---|
| RF | The shelter is never a *source*: no call sign, audience, mail, or signature. Six stations are all NPC-owned; two programs authored. |
| RT | Research never lapses (19 read sites, no revocation); the tree is 62 nodes with 31 roots. Skills decay, manuals and archives exist, but nothing ties knowledge to *bearers*. |
| SG | Five governance owners that don't talk; bloc scope words (~21) ≠ policy scopes (3), so most consent evaluations are empty; opposition has no action; `guardDeficiency` is hard-coded 0 in the daily politics call; Hoarding/Desertion have no law. |
| QW | `VetCandidate` has no host caller; door visitors have no hidden identity; agents are keyed by survivor id, visitors have none. |
| CC | Expeditions are one-per-survivor by rule; companions are a modifier on a handler; naval `crew_min/max` unread. |

## 3. Shared things (build once)

| Shared thing | Built by | Read by |
|---|---|---|
| **The gate adapter** (one seam on the airlock/door/visitor trio) | QW-P0/P2 (design signed with LR-P6 and PY-P3/P4) | LR, PY, QW, RF, CC |
| **Crew contract** (`roleNeeded[]`, `crewMin/Max`, `memberIds[]`, `cohesion`) | CC-P8 | *The Drowned Coast* (boats), *The Long Line: Freight* (wagons) |
| **Scope vocabulary map** (`assembly_scope_map.json`) | SG-P1 | PY (gate protocol), LR (admission), RF (content), CC (conscription), QW (informant use) |
| **News grades / canonical regions** | *The Living Region* LR-P1/P7 | RF audience, QW regional weights |
| **Fragment source seam** | RT-P3 | DC dives, RF mail, CC finds |
| **Seeded RNG stream ids** | integrator adds once | RF-P5, RT-P5, SG, QW-P1, CC-P5 |
| **Read-only "closed/blocked" flags** | LF, DC, PY (family 4–7) | RF, SG |

## 4. The gate adapter — one design, five consumers

**Owners (unchanged):** `AirlockSecuritySystem` = decision point; `DoorEncounterSystem` = authored knock content; `VisitorIntegrationSystem` = the stay.

| Plan | Uses the gate to… |
|---|---|
| Living Region | deliver refugee-wave **petitions** (families, workers, sick) |
| Plague Year | apply the **Gate Protocol** bias (Screen → Inspect/Quarantine offers; Sealed → TurnAway) and carry outbreak **vectors** |
| Quiet War | attach **Claim/Truth** to each arrival; run the tells and verbs |
| Radio Free Ashfall | deliver **signature** visitors (probes, invitations) |
| Crews and Companions | receive **returning parties** (with a possible stranger) |

**Rule:** one adapter function `OnArrival(visitor, cause)` composes causes in a fixed order (cause → claim/truth → protocol → decision preview); no plan writes to the airlock, door, or visitor state except through the adapter's public calls. **Proposed home:** a host file agreed in QW-P0. If P0 finds the door resolves against a demo roster (QW E2), *all five* plans are blocked on it.

## 5. Owners (no overlaps)

| Concern | Owner | Others |
|---|---|---|
| Player program jobs/station | `RadioProgramProductionSystem` | RF nests |
| Research completion/capability | `ResearchSystem` | RT overlays (opt-in) |
| Governance | `PoliticsSystem`, `PolicySystem`, `ShelterGovernanceEngine`, `JusticeSystem`, `LeadershipSystem` | SG reads/routes |
| Gate | airlock / door / visitor trio | QW/LR/PY/RF/CC via adapter |
| Agents/informants | `CounterIntelligenceSystem`, `InformantNetworkSystem` | QW derives |
| Expeditions | `ExpeditionSystem` (one per survivor) | CC coordinator beside |
| Companions | `CompanionAnimalSystem` | CC party presence |

## 6. Integrator-owned shared paths touched by more than one plan (serialise)
`src/Main.CampaignOwners.cs` (RF, RT, SG, QW, CC day-owners) · `CampaignStreamIds` (all five) · `CatalogIntegrityValidator.cs` (all five) · `src/Host/ExpeditionHostSession.cs` (CC; DC naval line) · `src/Main.SubsystemComposition.cs` (SG-P8, the `guardDeficiency` zero) · `Assets/StreamingAssets/Data/research_knowledge.json`, `wasteland_laws.json`, `door_encounters.json`, `radio_programs.json` (additive rows; owner files) · the gate adapter file (QW/LR/PY).

## 7. Dependencies and recommended order

```
QW-P0 (gate convergence; demo-roster check) ─► LR-P6, PY-P3/P4, RF-P4, QW-P2   (blocks all five if the door uses a demo roster)
CC-P0 (E11: double encounter roll?)         ─► CC-P2..   ; CC-P8 ─► DC crews, LF crews
SG-P0 (prove F5, scope map)                 ─► SG-P1..   ; SG-P8 registers cross-plan scopes
RT-P0 (bearer mapping for 20 nodes)         ─► RT-P1..
RF-P0 (own-slot without editing the catalog)─► RF-P1..
```

**Recommended sequence:** five **P0 audits in parallel** (read-only) → **SG** first (fixes a live inconsistency and helps every other plan by unifying scopes) → **QW + gate adapter** (with LR/PY) → **CC** (contract feeds DC/LF) → **RF** → **RT**. Soft hooks ship dark until both ends exist.

## 8. Conflicts found while writing (Rule 6 — logged, not resolved here)
1. **Gate = three systems** (airlock, door encounters, visitor integration); prior plans said "not yet located" — corrected in LR E11 and PY E13.
2. **Door encounters resolve against `DemoRoster`** in `Main.YearOfAsh` — VERIFY whether live play uses a real roster.
3. **Bloc scope words vs policy scopes** — consent evaluation mostly empty.
4. **Coup risk ignores guards** (`guardDeficiency = 0`).
5. **Hoarding and Desertion crimes have no law.**
6. **`VetCandidate` has no host caller.**
7. **Expedition "one per survivor"** blocks a naive party; solution is a coordinator + the existing query hooks (double-roll risk to be proved in CC-P0).
8. **Naval `crew_min/max` unread** — will be served by the crew contract.
9. **Overlap with Year Two:** Council of Succession (SG boundary), children/apprentices (RT, CC, RF), second-shelter custody (RF relay).
10. **Names:** "The Quiet" (Expansion 41) ≠ *The Quiet War*; noted, distinct ids.

## 9. Combined decision surface (all unsigned)
RF: DEC-RF-01…10 · RT: DEC-RT-01…10 · SG: DEC-SG-01…10 · QW: DEC-QW-01…10 · CC: DEC-CC-01…10. **Blocking-first:** DEC-QW-01/06 (gate), DEC-SG-08 (`guardDeficiency`), DEC-CC-04 (encounter rolling), DEC-RT-04 (parallel projects), DEC-RF-02 (station nesting).

## 10. What this sheet is not
Not a ledger entry, not a claim, not an approval. The foreman records `INTEGRATION_PLANS.md` entries and `WORKTREE_OWNERSHIP.md` claims; the user sets `STATUS: APPROVED BY USER`.

## 11. Related (added later 2026-09-29)
Director's picks P1–P4 (*The Ration Wars*, *The Long Siege*, *The Record Keepers*, *The Deep Works*) are in `docs/expansions/expansion_shelter_under_pressure_index.md`; subjects 13–16 (*Faith and Schism*, *The Underworld*, *The Deep*, *The Sky*) are in `docs/expansions/expansion_new_pressures_and_places_index.md`. They add a third consumer of the expedition host file (`ExpeditionHostSession.cs`: Long Siege dispatch gate, alongside CC and DC) and consume Shelter Governance's Hoarding statute (Ration Wars).

---

## The deeper layer — the family as a shape (second prose pass)

*(Second prose pass, non-contractual: texture and writing guidance only — not a claim, not an
authorization. The shared-silences register below is unchanged; the fragments are content
candidates, not new recorded questions.)*

**The second layer.** A new way to play is not a new button; it is a reason to press one of the
ones you already have. Every one of the five is a verb the shelter already had, given somebody to
point at. The machinery is not new. The stakes of pulling the handle are.

**What the family leaves between its members.**

> "Broadcasting existed; now it has a call sign. Vetting existed; now it has a visitor. Research existed; now it has bearers who can die."

> "One gate, three systems, five consumers, one adapter — agreed once, shared by four families."

> "The signal interceptor: one person, or a technique. Both plans touch it. Neither may resolve it."

*(Texture only. The silences below are the register; nothing here adds to them.)*

---

## What this family refuses to answer (shared silences — cross-expansion)

These are **shared across the five plans** and are only safe while *neither* side fills them. See
also `.ai/plans/OPEN_MYSTERY_INDEX_2026-09-29.md` §3.

- **The "signal interceptor"** — one person, or a technique? *Radio Free Ashfall* and *The Quiet
  War* both touch it. Resolving it in one plan steals the other's dread.
- **The shared gate adapter** at the shelter door — agreed once here (§4), used by four families.
  No second gate, ever.
- **Whether a broadcast is a copy or a voice.** *The Record Keepers* treats broadcast as a copy
  medium; *Radio Free Ashfall* treats it as identity. Both are true and neither is settled.
- **What a fragment is worth.** *The Reconstruction Tree* treats fragments as contested
  intelligence; *The Quiet War* may trade or steal them. Custody stays with the ledger.
