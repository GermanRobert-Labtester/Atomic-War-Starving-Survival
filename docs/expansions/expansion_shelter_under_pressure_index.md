# ASHFALL — "THE SHELTER UNDER PRESSURE": Family Index & Boundary Sheet
**Status:** Story-director coordination sheet. **Proposal — not a claim, not an authorization.** 2026-09-29.
Covers the four **director's own picks**, labelled **P1–P4** (no subject list was given for this round). They were first numbered 13–16 in-session; the user's later list assigned 13–16 to Faith and Schism, The Underworld, The Deep and The Sky (`expansion_new_pressures_and_places_index.md`), so this set is relabelled to avoid a clash. Sibling sets: Year Two (Days 361–720); "The world moves without you" (`expansion_world_moves_without_you_index.md`, subjects 4–7); "New ways to play" (`expansion_new_ways_to_play_index.md`, subjects 8–12); "New pressures and places" (`expansion_new_pressures_and_places_index.md`, subjects 13–16).

| # | Expansion | Prose plan | Integration plan | Prefix |
|---|---|---|---|---|
| P1 | **The Ration Wars** — the pantry as politics | `expansion_ration_wars_plan.md` | `.ai/plans/ration-wars-2026-09-29.md` | `RW-` |
| P2 | **The Long Siege** — a raid is a night, a siege is a season | `expansion_long_siege_plan.md` | `.ai/plans/long-siege-2026-09-29.md` | `LS-` |
| P3 | **The Record Keepers** — the shelter decides what it remembers | `expansion_record_keepers_plan.md` | `.ai/plans/record-keepers-2026-09-29.md` | `RK-` |
| P4 | **The Deep Works** — the shelter goes down | `expansion_deep_works_plan.md` | `.ai/plans/deep-works-2026-09-29.md` | `DW-` |

All four are `STATUS: DRAFT — awaiting user approval`. No per-package derived plans were written.

> *"The first twelve expansions widened the world. These four turned around and looked at the
> building."*
>
> Pressure is not damage. Pressure is what happens to a *community* when it has to keep deciding
> under conditions that do not improve: who eats, who remembers, who digs, who holds the gate
> through a season. None of these four adds a monster and none adds a battle. They add a
> **calendar**.

## 1. What these four have in common
The first twelve subjects widened the *world* and the *ways to play*. These four turn inward: **they are about the shelter as an institution under strain** — how it feeds people, defends itself over time, remembers itself, and grows downward. Each one **connects existing owners and adds one small ledger**; none adds a second authority.

## 2. What each one found (the central gap)

| Plan | Central gap (verified by reading) |
|---|---|
| RW | Conflict meter judges against the **mean** and, because priority bonuses (1.30 / 1.15 / 1.0 / 0.75) clamp to 1.0, **cannot tell Critical, High and Standard apart**; the ration *tier* never reaches it; Hoarding has no law; nobody keeps the book. |
| LS | One raid source (Iron Raiders), one production caller of the resolver, strength 3 or 6; a raid ends at dawn and leaves nothing behind. |
| RK | Every record is perfectly safe: no medium, no place, no loss. Ink fade fields are printed and never applied; `unlockedEvidenceIds` has no reader; the Shelter Archive is a projection whose only direct writer is a ten-day milestone heartbeat. |
| DW | Four undergrounds, one real seam (node flood → sector, same id). The shelter's shaft never opens onto a gallery; galleries are discovered from surface anchors only; nothing is *kept*; the unused `ExpansionTunnel` project slot has no consumer. |

## 3. Shared things (build once)

| Shared thing | Built by | Read by |
|---|---|---|
| **Siege Table** (a Table Rule + tier preset) | RW-P1 (Table Rules) | LS |
| **Place** for a record (room; later a dry held drift) | RK-P1 | DW (Store), RW (the Book) |
| **Exit drift** as a route (runner, sally, relief) | DW-P4/P6 | LS, CC |
| **Sap / collapse** contract (`TryCollapseNode`) | DW-P7 | LS |
| **Evidence weight from a record** | RK-P7 (`JusticeSystem.AddEvidence`) | RW (the Book as evidence), QW |
| **Seeded RNG stream ids** | integrator adds once | RW-P3/P5, LS-P3, RK-P3, DW-P3/P4 |
| **Read-only "closed/blocked" flags** | LS (roads cut), PY (gate protocol) | LR, DW |

## 4. Owners (no overlaps)

| Concern | Owner | Others |
|---|---|---|
| Resentment, theft, confrontation | `RationConflictSystem` | RW adds an optional expected-portion provider |
| Tiers, protocols, priority groups | `ResourceRationingSystem` | RW reads |
| Raid resolution, traps, perimeter | `DefenseSystem` / `PerimeterDefenseSystem` | LS nests one DTO field, no logic change |
| Raid opportunity | `IronRaidersSystem` | LS starts a siege from a repelled raid by doctrine only |
| Watch, acoustics, gate | Expansion 36 owners | LS reads |
| Journal, memorial | `JournalSystem`, `MemorialSystem` | RK reads |
| Archive timeline | `ShelterArchiveSystem` (projection) | RK adds an optional overlay parameter |
| Transcription | `ArchiveDeskSystem` | RK nests custody rows |
| Node state, shoring, ventilation | `SubterraneanSystem` | DW adds `TryCollapseNode` + a nested ledger |
| Hazard sectors, bulkheads, rescue | `ExcavationHazardSystem` | DW calls only |
| Shaft, stability, crews | `ShelterExpansionSystem` | DW uses the project pipeline |
| Tunnel segments | `TunnelNetworkSystem` | DW registers one segment per Exit drift (if allowed) |

## 5. Integrator-owned shared paths touched by more than one plan (serialise)
`src/Main.CampaignOwners.cs` (RW, LS, RK, DW day-owners) · `CampaignStreamIds` (all four) · `CatalogIntegrityValidator.cs` (all four) · `src/Host/ExpeditionHostSession.cs` (**LS dispatch gate + CC party + DC naval line — three plans**) · `src/Host/RationConflictHostSession.cs` (RW; sole `SetAllocation` caller) · `src/Main.Defense.Integration.cs`, `src/Main.Muster.cs` (LS) · `src/Main.Subterranean.cs`, `src/Host/SubterraneanHostSession.cs`, `src/Main.TunnelNetwork.cs` (DW) · `Assets/Ashfall.Core/Subterranean/SubterraneanSave.cs` (DW; checksum) · `Assets/Ashfall.Core/Shelter/ShelterExpansionSystem.cs` and `shelter_construction.json` (DW; shared with any shelter-expansion work) · `Assets/Ashfall.Core/ArchiveDeskSystem.cs`, `Shelter/ShelterArchiveSystem.cs` (RK) · `retention_policies.json` (RW additive row).

## 6. Dependencies and recommended order

```
RW-P0 (fairness/tier/withdrawal-reason audit) ─► RW-P1..  ;  RW-P1 (Table Rules) ─► LS Siege Table (soft)
RK-P0 (overlay signature, death event)        ─► RK-P1..  ;  RK-P1 (Place) ─► DW Store (soft)
DW-P0 (save checksum, shaft paths, node ids)  ─► DW-P1..  ;  DW-P7 (collapse) ─► LS sap (soft)
LS-P0 (dispatch gate, morale read)            ─► LS-P1..  ;  DW Exit ─► LS runner/sally (soft)
```

**Recommended sequence:** four **P0 audits in parallel** (read-only) → **RW** first (smallest change; it also exposes the priority-saturation gap) → **RK** (self-contained overlay) → **DW** (needs its save-checksum and shaft-path answers) → **LS** (largest host footprint; useful alone, richer with the other three). Soft hooks ship dark until both ends exist.

## 7. Conflicts found while writing (Rule 6 — logged, not resolved here)
1. **"The Night Watch" is Expansion 36 and already built** (staffing, perimeter, gate, acoustics). The subject was reframed as *The Long Siege*; the Watch is read-only.
2. **Raid source is single.** Only `Main.Muster.cs` (Iron Raiders) calls the resolver in production; strength is 3 or 6.
3. **Priority saturation.** After `Clamp01`, Critical, High and Standard all equal 1.0 in the conflict meter (RW E3b).
4. **Ration tier does not reach the conflict meter** on the host path (RW E5, VERIFY no other path).
5. **Hoarding has no law**; Shelter Governance owns the statute book — RW consumes it.
6. **The Shelter Archive is a projection**; a "gap" cannot be stored in it, only overlaid (RK DEC-RK-01/03).
7. **Ink fade and `unlockedEvidenceIds` are dead ends** (printed/unread).
8. **Journal authors and voices already exist** — RK adds no author or slant.
9. **Two shaft-start paths diverge**: blueprint-driven (14 d; 25/15/6; up to level 5) vs hard-coded (12 d; 20/10; class default 3). (DEC-DW-02)
10. **`ConstructionProjectType.ExpansionTunnel` is unused** — candidate slot for the breakthrough (DEC-DW-03).
11. **A node → sector flood bridge already exists** (same id, increases only) — DW reuses it and adds no second bridge.
12. **No expedition start gate** exists — LS needs a dispatch refusal (DEC-LS-04); the same host file is shared with CC and DC.
13. **The subterranean save is checksummed** — additive fields must be proven safe (DEC-DW-01).
14. **Overlaps with earlier families** — Year Two owns outposts (LS), Reconstruction Tree owns capability vs RK accounts, Shelter Governance owns laws (RW), Quiet War owns informants (RK/LS leaks).

## 8. Combined decision surface (all unsigned)
RW: DEC-RW-01…10 · LS: DEC-LS-01…10 · RK: DEC-RK-01…10 · DW: DEC-DW-01…10. **Blocking-first:** DEC-RW-03 (ledger scope) and DEC-RW-05 (hoard as a real transfer), DEC-LS-02 (siege state home) and DEC-LS-04 (dispatch gate), DEC-RK-01 (custody overlay home), DEC-DW-01 (save checksum), DEC-DW-02 (two shaft paths), DEC-DW-03 (breakthrough slot).

## 9. Player-facing arc when all four exist (illustrative)
Late autumn: the Toll digs in on the east ridge and the road to the salt depot closes (LS). The table goes to Half and the quartermaster opens the Book in the mess hall (RW). The east drift — a dry gallery the Works broke into in September — carries the records to safety and, at night, a runner (DW, RK). On the eleventh night the Diggers' sap breaks the east wall; the crew chief looks at the player. The Chronicle, when it comes, notes that days 212 to 219 have no account, and that the bulkhead held.

## 10. What this sheet is not
Not a ledger entry, not a claim, not an approval. The foreman records `INTEGRATION_PLANS.md` entries and `WORKTREE_OWNERSHIP.md` claims; the user sets `STATUS: APPROVED BY USER` on any plan that should ship.

---

## The deeper layer — the family as a shape (second prose pass)

*(Second prose pass, non-contractual: texture and writing guidance only — not a claim, not an
authorization. The shared-silences register below is unchanged; the fragments are content
candidates, not new recorded questions.)*

**The second layer.** The first twelve expansions widened the world; these four turned around and
looked at the building. Pressure is not damage — it is what happens to a community that must keep
deciding under conditions that do not improve: who eats, who remembers, who digs, who holds the
gate. None adds a monster or a battle. They add a *calendar*.

**What the family leaves between its members.**

> "Days 212 to 219 have no account, and the bulkhead held."

> "The Book is opened in the mess hall; the records go out through the east drift. Both are acts of remembering."

> "A siege is a schedule, a drift is a promise, a Gap is a wound with a date on it, and a half-table is arithmetic."

*(Texture only. The silences below are the register; nothing here adds to them.)*

---

## What this family refuses to answer (shared silences — cross-expansion)

These are **shared across the four plans** and are only safe while *neither* side fills them. See
also `.ai/plans/OPEN_MYSTERY_INDEX_2026-09-29.md` §3.

- **Whether a siege is a battle or a schedule.** *The Long Siege* owns the action; *The Deep Works*
  owns the drift. Neither may narrate the other's side of a countermine.
- **What the Pantry Book is written in.** *The Ration Wars* keeps a ledger; *The Record Keepers*
  owns custody. Medium and slant belong to the Keeper and are never invented here.
- **Whether a Gap is a fact or a wound.** *The Record Keepers* tags it and stops. *Shelter
  Governance* may order a Strike; the Keeper may refuse. That refusal is not explained.
- **What the shelter is holding *up*.** *The Deep Works* props a roof; *The Long Siege* tests one.
  The load-bearing question is deliberately never asked.
