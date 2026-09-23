# ASHFALL — WAVE 4 INTEGRATION PROGRAM · PLAN 1 OF 6

# SAVE, STATE & MIGRATION INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W4 (six-plan integration wave — the shelter's remaining machinery)
**Document:** W4-01
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W4-02 (world/travel), W4-03 (infrastructure), W4-04 (ecology), W4-05 (society), W4-06 (medicine)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates **state and memory**: how every system captures what it owns, how
that capture is versioned, migrated, checksummed, restored, and how a player's world
survives time, patches, and mistakes. It extends the existing save spine — one envelope,
one section registry, one slot service, one checksum — and it never invents a parallel
save store, a second serialization authority, or a private snapshot.

### 0.1 Two selection levels

| Level | Choice | Granularity |
|---|---|---|
| **Level 1** | Plan Path **A**, **B**, or **C** | the whole plan's posture |
| **Level 2** | ten decision points, each **A/B/C** | per-concern depth |

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Round-Trip | 1–10 | — | — |
| B One Save Authority | 1,2,5 | 3,4,6,7,8 | 9,10 |
| C Portable Across Versions | — | 3,4,6 | 1,2,5,7,8,9,10 |

### 0.3 The Wave 4 rule for this plan

> **One save authority, one section per concern, one checksum, one migration path.**
> `SaveSectionRegistry` owns registration; `CampaignSaveEnvelope`/`SaveEnvelopeHelper`
> own the envelope; `SaveSlotService` owns slots; `SaveChecksum` owns integrity;
> `SaveWireContract` owns the wire shape. No system writes its own file, keeps its own
> cache of another system's state, or restores around the registry.

### 0.4 Vocabulary

| Term | Meaning |
|---|---|
| section | one registered, named slice of campaign state owned by exactly one system |
| envelope | the versioned container that carries all sections plus metadata |
| wire contract | the declared, versioned shape of serialized state |
| round-trip | capture → serialize → deserialize → restore → equal |
| neutral load | missing/unknown state loads as an authored default, never an exception |
| checksum | culture-invariant digest proving content stability |
| migration | a documented, ordered transform from an older schema to the current one |
| slot | a named save destination (auto, quick, manual, profile) |
| Triad | the save/restore/determinism gate family that must stay green |
| cross-run state | meta progress outside any campaign slot (DEC-20 boundary) |

---

## 1. Premise audit — what P0 must verify (Rule 7)

A plan is not proof that an API, section, or behavior still exists. Before any edit,
P0 re-verifies each premise in source and data. The items below are **claims to check**,
not findings:

```text
[ ] Assets/Ashfall.Core/Save/ files and their current public surface:
    CampaignEnvelopeBuilder, CampaignSaveEnvelope, SaveEnvelopeHelper,
    SaveSectionRegistry, SaveSlotService, SaveSlotTypes, SaveStore,
    SchemaVersionedEnvelope
[ ] src/Main.SaveOrchestrator.cs current call graph (note the .theirs file —
    confirm which copy is authoritative and whether it is tracked)
[ ] Assets/Ashfall.Core/SaveChecksum.cs + SaveWireContract.cs current contracts
[ ] HoldfastSaveFrozen.cs + HoldfastSave.cs + ExpansionHubSave.cs + ExpansionQuestSave.cs
[ ] DoseLedgerSave.cs, MedicalPipelineSave.cs, MedicalWardSave.cs, PathogenStrainSave.cs,
    IndependentBranchSave.cs, MilitaryBranchSave.cs, RebelBranchSave.cs,
    PrpfSave.cs, WeightOfChoicesSave.cs — every *Save.cs owner
[ ] CrossRunProfileStore.cs boundary vs campaign slots
[ ] src/Main.Lifecycle.cs reset/dispose paths and what they null
[ ] every stateful Core system's capture/restore methods (names + signatures)
[ ] test coverage: which save round-trip/determinism tests exist and which are
    quarantined (Twin_ASHFall/quarantine/manifests/)
[ ] save-file location and write discipline on disk (one writer, atomicity)
```

### 1.1 Evidence posture

- Proposal only. No paths claimed; no authorization implied; no ledger rows added.
- JSON data authority unchanged; save format is code-owned state, not JSON data.
- Determinism (Rule 4): any RNG state that affects outcome must be captured with
  the system that consumes it; `System.Random` remains forbidden in Core.
- Focused verification per `TEST_POLICY.md`; nothing broad runs by default.
- Rule 5: extending existing owners only. A new save section requires its own
  owner to exist first, with a capture/restore path designed before persistence.

### 1.2 Known anchors (verify, don't trust)

| Anchor | Why it matters |
|---|---|
| `SaveSectionRegistry` | the only registration door; duplicates are a defect |
| `SchemaVersionedEnvelope` | versioning must be additive and integer |
| `SaveChecksum` | culture-invariant formatting is a repeat hazard |
| `Main.Lifecycle.cs` | reset paths are where stale state survives between slots |
| `CrossRunProfileStore` | the one legitimate "outside the slot" store (DEC-20) |
| `UNBLOCK-04` | ledger/census truth ruling that save-adjacent claims obey |

---

## 2. The three Plan Paths

### 2.1 Path A — Truth & Round-Trip

Audit every section and every owner: does what is declared get captured, does what is
captured get restored, and does restore equal capture? Path A produces the section
ledger, the round-trip matrix, and a ranked repair list. It changes nothing until the
ledger exists — then fixes only proven asymmetries.

### 2.2 Path B — One Save Authority

Unify the seams: one registry entry per owner, one envelope path, one slot service,
one checksum, one migration ladder. Path B removes private snapshots, duplicate
serialization, and restore-order dependence; it makes save/load a staged pipeline
with defined ordering and failure semantics.

### 2.3 Path C — Portable Across Versions

Make the save survive patches, long campaigns, and future schema changes: a migration
table with tested steps, a compatibility policy (additive, neutral-load, bounded
transforms), size/perf budgets, and a release discipline where every stateful change
ships with its capture/restore and its migration note.

---

## 3. The ten decision points

Each point states the owner and current anchor, the decision to make, the three path
postures, a verification sketch, and what is never allowed.

### 3.1 Point 1 — Section inventory and ownership

**Owner anchor:** `SaveSectionRegistry`, `CampaignEnvelopeBuilder`.
**Decision:** produce a complete, current ledger of every save section, its owning
system, its schema version, and its consumers — then enforce one section per concern.

- **A:** enumerate; reconcile registry vs actual envelope contents; file orphans and
  duplicates; no code change beyond ledger + tests that assert registry integrity.
- **B:** registration discipline; a stateful system that is not registered is a gate
  failure; forbid private `*Save` files outside the envelope.
- **C:** ownership inheritance rules for expansions (new system ⇒ section + migration
  note + version bump in one package).

**Verify:** registry enumeration test; duplicate-key rejection; envelope content diff
against the ledger.
**Never:** a second envelope, a hidden per-system file, or a section owned by two
systems.

### 3.2 Point 2 — Capture/restore symmetry

**Owner anchor:** every `*Save.cs` owner plus `Main.SaveOrchestrator.cs`.
**Decision:** every captured field restores; every restored field was captured. Build
the field-level round-trip matrix and repair asymmetries with focused tests.

- **A:** matrix per section (capture field → restore field → equality); list mismatches.
- **B:** restore-order contract (dependencies first); idempotent restore; no restore
  that depends on host panel state.
- **C:** long-arc growth rules: when a field changes meaning, add a conversion test.

**Verify:** per-section round-trip test with a populated fixture; equality of
post-restore state; no exceptions on neutral/partial input.
**Never:** restore that silently drops fields, or capture that depends on wall-clock.

### 3.3 Point 3 — Wire contract and schema versioning

**Owner anchor:** `SaveWireContract`, `SchemaVersionedEnvelope`.
**Decision:** freeze the wire policy: additive fields only, integer `schema_version`,
explicit defaults for absent members, no renames without a migration step.

- **A:** audit actual contract usage; find fields outside the declared shape.
- **B:** one serializer path; forbid ad-hoc conversion in hosts; culture-invariant
  number/date formatting everywhere.
- **C:** version-bump checklist: bump, write migration, test old fixture, note in the
  release record.

**Verify:** old-fixture load test; unknown-field tolerance; version mismatch behavior
(documented, non-destructive).
**Never:** silent shape changes, float formatting without invariant culture, or a
second serializer.

### 3.4 Point 4 — Migration and legacy neutrality

**Owner anchor:** `SaveEnvelopeHelper`, `SaveSectionRegistry`, migration notes.
**Decision:** define the migration ladder: each schema delta has an ordered transform,
and absence of a section loads neutral without gameplay harm or exception.

- **A:** inventory known legacy shapes (frozen Holdfast save, older campaign
  envelopes) and their real loading behavior.
- **B:** one migration entry point; transforms are pure, deterministic, and tested
  against stored fixtures; no host-side patches.
- **C:** a published compatibility statement per release: "saves from version N load
  with these transformations; these features default."

**Verify:** fixture-based migration tests (old → current); neutral-load tests for
missing sections; no exception classification in logs.
**Never:** destructive migration, best-effort guessing, or loading old saves through
a "if version < X" maze scattered across hosts.

### 3.5 Point 5 — Checksum stability and determinism

**Owner anchor:** `SaveChecksum`, seeded RNG owners.
**Decision:** checksums are stable across machines, cultures, and load orders; RNG
state that affects outcomes is captured with its consumer; paired replays match.

- **A:** audit checksum inputs; find culture-sensitive formatting, dictionary-order
  dependence, and timestamp inclusion.
- **B:** canonical digest input order; invariant culture; checksum covers exactly the
  agreed surface; a schema version is part of the digest input policy.
- **C:** determinism regression policy: same seed + same inputs ⇒ same checksum,
  enforced by a paired replay test per release.

**Verify:** two-run checksum equality; culture-switch run (e.g., tr-TR) equality;
ordered-section hashing test.
**Never:** `System.Random` in Core, wall-clock seeds, hash-order dependence.

### 3.6 Point 6 — Corruption detection and recovery

**Owner anchor:** `SaveStore`, `SaveChecksum`, slot types.
**Decision:** corrupt or truncated saves are detected, reported in player terms, and
never destroy the last good data; recovery paths are explicit.

- **A:** enumerate failure modes (truncation, tamper, partial write, disk error) and
  current behavior for each.
- **B:** write discipline: temp + atomic replace; keep last-good backup; checksum
  verify before accepting; typed error result instead of exception leakage.
- **C:** player-facing recovery: "the save could not be loaded; the previous save is
  available" with a canonical message, and a support path that never leaks stack text.

**Verify:** injected-truncation test; tampered-checksum test; backup fallback test;
message-canonicalization check.
**Never:** overwriting the only good copy, crashing to desktop, or silently loading
partial state as if valid.

### 3.7 Point 7 — Slot authority and lifecycle

**Owner anchor:** `SaveSlotService`, `Main.Lifecycle.cs`, `Main.PanelLifecycle.cs`.
**Decision:** slots behave: one writer at a time, defined autosave/quicksave points,
clean reset on slot switch, and no state bleeding between campaigns.

- **A:** map every write trigger (autosave, quick, manual, exit, milestone) and every
  reset path; find stale-state survivors.
- **B:** a single save orchestrator; host panels request, never write; reset/dispose
  order documented; lifecycle test asserts no cross-slot leakage.
- **C:** long-campaign hygiene: rolling autosaves, size watch, save-time budget at
  scale.

**Verify:** slot-switch leakage test (state A after load B); double-write guard;
reset-null audit.
**Never:** two systems writing the same slot, save on a timer that ignores gameplay
state, or a host panel serializing anything.

### 3.8 Point 8 — Cross-run and meta-state boundary

**Owner anchor:** `CrossRunProfileStore`, DEC-20 ruling.
**Decision:** campaign state stays in slots; only explicitly sanctioned meta state
(e.g., cross-run history) lives outside, with its own versioning, and it never
becomes a hidden gameplay authority.

- **A:** audit what is stored outside slots today; classify each as sanctioned meta or
  leakage.
- **B:** a single profile store; no gameplay-affecting values read from it without a
  documented contract; clean deletion on player request.
- **C:** meta-state policy in release notes; privacy stance (local-only, no PII).

**Verify:** profile-store round-trip; deletion test; gameplay-isolation assertion.
**Never:** gameplay outcomes read from cross-run memory, or a second profile file.

### 3.9 Point 9 — Size and performance budgets

**Owner anchor:** `SaveStore`, envelope builder, orchestrator.
**Decision:** saves stay small and fast: per-section size ledger, capture/restore time
budgets, no per-frame writes, and no unbounded growth (histories, logs, journals).

- **A:** measure current sizes/times per section at representative campaign age.
- **B:** bounded collections everywhere (ring buffers, caps with authored retention);
  compress only if the format already allows and the win is real.
- **C:** budget per release: payload ceiling, capture-time ceiling, and a size
  regression check in the focused pipeline.

**Verify:** size ledger snapshot; capture-time measurement; bounded-collection tests.
**Never:** unbounded logs in the save, write-on-tick, or compression that breaks
checksums.

### 3.10 Point 10 — Save/load surfaces and failure feedback

**Owner anchor:** save/load panels (W3-06 coordination), `SaveSlotService` results.
**Decision:** the player can see truthful slot state (age, campaign day, version) and
every failure produces a canonical, actionable message — never silence, never
exception text.

- **A:** audit surfaces: what slot metadata is shown, what is fabricated, what is stale.
- **B:** surfaces read the slot service; no local caching of slot state; failure
  messages carry text refs; loading state is visible and cancellable.
- **C:** version transparency: slots show which build wrote them and whether migration
  will occur, in restrained language.

**Verify:** W3-06 route/focus/lane kits over save surfaces; canonical-message scan;
stale-metadata test after external file change.
**Never:** a save surface that writes, a fabricated date, or an error dialog with a
raw exception.

---

## 4. Selection sheet

```text
ASHFALL WAVE 4 · PLAN W4-01 · SELECTION SHEET

Plan Path:   [ ] A Truth & Round-Trip   [ ] B One Save Authority   [ ] C Portable Across Versions

Points (mark A/B/C or leave default):
 1 section inventory ....... [ ]
 2 capture/restore ......... [ ]
 3 wire contract ........... [ ]
 4 migration ............... [ ]
 5 checksum/determinism .... [ ]
 6 corruption recovery ..... [ ]
 7 slot authority .......... [ ]
 8 cross-run boundary ...... [ ]
 9 size/perf budgets ....... [ ]
10 save surfaces ........... [ ]

Selected by: ____________   Date: ________   Foreman: ____________
```

---

## 5. Phase ladder

| Phase | Name | Exit |
|---|---|---|
| P0 | Premise audit + section ledger | ledger filed; premises re-verified; package claim prepared |
| P1 | Round-trip matrix + repairs | matrix green for selected points; focused tests added |
| P2 | Contract + migration ladder | old fixtures load; neutral-load proven |
| P3 | Checksum/determinism enforcement | paired replay + culture checks green |
| P4 | Slot discipline + recovery | leakage/truncation/backup tests green |
| P5 | Budgets + surfaces | size/time ledger; surface kits green; messages canonical |
| P6 | Closeout | evidence pack; Triad green; limitations recorded |

Each phase is independently selectable; P0 is mandatory for every path.

---

## 6. Non-goals, never-touch, one-authority

**Non-goals**

- No save-format redesign "because it would be cleaner."
- No cloud sync, no encryption, no compression scheme invention.
- No gameplay balance changes; no new sections for systems that do not exist yet.
- No host-side serialization, ever.

**Never-touch**

- `SaveChecksum` invariants and `SchemaVersionedEnvelope` version semantics.
- `CrossRunProfileStore` boundary (DEC-20).
- Quarantined tests without the TEST_POLICY re-enable procedure.
- Any production path not claimed in `WORKTREE_OWNERSHIP.md` for the package.

**One authority per concern**

| Concern | Owner |
|---|---|
| registration | `SaveSectionRegistry` |
| envelope/serialization | `CampaignSaveEnvelope` + `SaveEnvelopeHelper` + `SaveWireContract` |
| slots | `SaveSlotService` + `SaveStore` |
| integrity | `SaveChecksum` |
| orchestration | `Main.SaveOrchestrator.cs` |
| meta boundary | `CrossRunProfileStore` |

---

## 7. Verification and acceptance

- **T1 static:** registry/envelope/section ledger consistency; no private save files;
  no host serialization; checksum input audit.
- **T2 focused:** per-section round-trip; old-fixture migration; neutral load;
  corruption injection; slot-switch leakage; culture-switch checksum; bounded
  collections; surface kits (with W3-06).
- **T3 soak:** 60-day campaign with autosave cycles: save size trend, capture time
  trend, checksum stability, zero drift.
- **Acceptance:** evidence pack (ledger, matrix, test outputs, size/time charts) plus
  the Annex U signature. A compile-green result is not acceptance.

## 8. Handoffs and dependencies

| Direction | Detail |
|---|---|
| W2-01 / W2-02 | generated checks and silent-failure rules extend to save paths |
| W3-06 | save/load surfaces join the route/focus/lane kits |
| W4-02..06 | every new stateful system in those plans ships its section + migration note |
| UNBLOCK-04 | ledger/census truth rules apply to any save-adjacent claim |
| Integrator | owns the Triad gate and the release compatibility statement |

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

**This annex is not part of the integration plan.** It exists so the plan can be
released for execution without entangling its technical content with authorization.

## U.1 What this plan releases

| Release | Unblocks |
|---|---|
| U1 | save round-trip audits currently deferred as "needs save-fuzz first" |
| U2 | migration ladder work blocked on "no compatibility policy declared" |
| U3 | slot-lifecycle fixes blocked on D19c statics findings (shared service nulls) |
| U4 | surface truth work blocked on "slot metadata is not canonical yet" |
| U5 | determinism enforcement blocked on checksum-input policy |

## U.2 Signature block

```text
ASHFALL WAVE 4 · PLAN W4-01 · RELEASE SIGNATURE
HEAD: ________  Date: ________
[ ] P0 premise audit completed and filed
[ ] section ledger exists; duplicates/orphans listed
[ ] no path claimed outside the package
[ ] focused test targets named
[ ] rollback position recorded
Signed: ________   Foreman: ________
```

## U.3 Never-touches

- No edit to another Wave 4 plan's claimed paths.
- No re-open of sealed debt rows (e.g., `DEBT-194-CRISIS-PRODUCER-WIRE`).
- No change to DEC-20, DEC-05, or the string-freeze ruling (D22).
- No revival of quarantined tests without the documented procedure.

## U.4 Release rule

> This plan executes only after U.2 is signed. Until then it is read-only planning:
> useful for scheduling, never for editing.

---

*End of W4-01 — Part I. Expansion parts (II+) continue on the established Wave 3
pattern: deep designs, playbooks, verification kits, case studies, Q&A, Path C
designs, checklists, and closure.*---

# W4-01 · PART II — DEEP DESIGN: SECTION INVENTORY AND ROUND-TRIP (POINTS 1–2)

> This part turns §3.1 and §3.2 of Part I into implementable design: contracts,
> schemas, algorithms, and test specifications. Nothing here changes an owner;
> everything here makes the owner's obligations explicit.

## II.1 The section ledger: schema and meaning

The section ledger is the plan's first deliverable. It is a table; if it does not
exist, nothing else in this plan can be verified.

```yaml
section_ledger:
  - key: "holdfast.campaign"            # unique, stable, dot-namespaced
    owner: "HoldfastSession"            # exactly one owning type
    save_type: "HoldfastSave"           # the serialized record
    schema_version: 7                   # integer, only increases
    captured: ["day", "phase", "flags", "statics..."]
    restored: ["day", "phase", "flags", "statics..."]
    migrations:                         # ordered, from version N to N
      - from: 5
        to: 6
        note: "flags map widened; default false"
      - from: 6
        to: 7
        note: "statics split into shared services; null-safe load"
    consumers: ["Main.Lifecycle", "Main.SaveOrchestrator", "panels:journal"]
    size_budget_bytes: 120000
    tests: ["HoldfastSaveRoundTripTests"]
  - key: "expansion.hub"
    owner: "ExpansionHubSave"
    ...
```

### II.1.1 Ledger rules

```text
L1  every row has exactly one owner (Rule 5)
L2  every row has a schema_version integer
L3  captured and restored lists must correspond; a deliberate asymmetry needs a
    written note and a test that asserts the asymmetry is intentional
L4  every migration has a from/to and a note; migrations are ordered and dense
    (no gaps: 5->6, 6->7, never 5->7 alone)
L5  consumers list every code path that reads the section after load
L6  size budgets are set from measurement, not taste; they may rise only with an
    evidence note
L7  tests name the focused target that proves the row
```

### II.1.2 The ledger gate

A static check (`T1` family) reads the ledger and the registry and fails when:

- a registered section has no ledger row;
- an owner type exists in the codebase with a `*Save` type but no ledger row;
- two rows share a key or an owner;
- a migration list has a gap;
- a captured/restored asymmetry lacks a note.

The gate is cheap, deterministic, and catches drift the moment a new system adds
state without paperwork.

## II.2 Ownership: what "one owner" means pedantically

An owner is the single type that:

1. holds the live state;
2. exposes capture/restore (or a save record constructed only by it);
3. is the only type allowed to decide what is durable;
4. is named in the ledger.

A host partial (`Main.*.cs`) may **call** capture/restore, never **define** it.
A panel may **read** restored state, never hold a second copy.

Anti-patterns the ledger exposes:

| Anti-pattern | Smell | Repair shape |
|---|---|---|
| shadow state | panel keeps its own copy of system values | delete copy; render owner reads |
| split owner | two types serialize parts of one concern | merge choose one owner; migrate |
| silent statics | shared service cached in a static that never nulls | lifecycle-owned, reset in `Main.Lifecycle` |
| envelope bypass | system writes its own file | route through registry |
| version sprawl | per-field versioning instead of per-section | single integer per section |

## II.3 The round-trip matrix: field-level specification

The matrix is one row per durable field:

```text
section          | field            | capture source      | restore target        | equality rule | test
holdfast.campaign| day              | session.Day         | session.Day           | exact int     | RT-01
holdfast.campaign| phase            | session.Phase       | session.Phase         | enum exact    | RT-02
holdfast.campaign| flags[]          | flag ledger         | flag ledger           | set equality  | RT-03
holdfast.campaign| statics.rngState | seeded rng cursor   | seeded rng cursor     | exact         | RT-04
```

### II.3.1 Equality rules

| Type | Rule |
|---|---|
| integers | exact |
| enums | exact by name (never by ordinal on the wire) |
| floats (if unavoidable) | canonical formatted round-trip with invariant culture |
| sets/lists | order-insensitive where order is not meaning; otherwise exact |
| dictionaries | key-set equality plus per-key equality |
| derived values | never persisted; recompute-or-refuse policy |

### II.3.2 The recompute-or-refuse policy

A field that can be derived (totals, cached coefficients, display strings) must not
be persisted. If a legacy save contains it, the migration drops it and the owner
recomputes on restore. This one rule kills a whole class of drift.

### II.3.3 Test shape (focused)

```text
1. build a populated fixture via public API (not reflection)
2. capture -> serialize -> deserialize -> restore
3. assert field equality per matrix rules
4. assert no exception on a neutral fixture (all defaults)
5. assert no exception on a legacy fixture (pre-migration)
```

Each section gets one such test file; the round-trip is the contract.

## II.4 Worked example: the static-services hazard

D19c statics (`_silentFoundry`, `_sharedSkillProgression`, `_sharedFactionStance`)
are the canonical example of why capture/restore is not enough: a shared service
cached in a field that the lifecycle reset path does not null will survive a slot
switch and leak the previous campaign's state into the next.

**Design rule that prevents it:**

```text
R1  every shared service is created per session, never in a static initializer
R2  Main.Lifecycle reset/dispose nulls every session-scoped service reference
R3  the slot-switch test asserts each named service reference is a fresh instance
R4  the ledger names the owner for each service's durable state
```

The case is recorded here because it is the highest-value single repair this plan
can make — leaks between campaigns are invisible until a player reports
impossible behavior.

*End of Part II. Continues in Part III (wire contract, migration, checksum).*---

# W4-01 · PART III — DEEP DESIGN: WIRE CONTRACT, MIGRATION, CHECKSUM (POINTS 3–5)

## III.1 The wire contract

`SaveWireContract` is the declared shape of serialized state. The design goal is
boring predictability: additive growth, explicit defaults, no culture, no order.

### III.1.1 Serialization rules

```text
W1  one serializer path for the whole envelope (no per-section JSON writers)
W2  field names are stable identifiers; renaming requires a migration
W3  absent member => authored default (neutral load), never an exception
W4  unknown member => ignored on load, preserved only if the format has a slot
    for it (forward compatibility note)
W5  numbers use invariant culture, round-trip format
W6  enums serialize by name, never by ordinal
W7  collections serialize in a canonical order (sorted keys) so checksums are
    stable
W8  the schema_version is written as an integer at the envelope level and stays
    at the envelope level
```

### III.1.2 Worked wire snippet

```jsonc
{
  "section": "holdfast.campaign",
  "version": 7,
  "state": {
    "day": 412,
    "phase": "Evening",
    "flags": { "gate_opened": true, "well_capped": false },
    "rng": { "seed": 910273, "cursor": 18844 }
  }
}
```

The snippet shows the four durable categories: counters, enums, fact sets, and rng
cursors. Everything else (display strings, derived totals) is absent by policy.

### III.1.3 The version-bump checklist

```text
[ ] change is additive OR has a migration
[ ] new fields have authored defaults
[ ] old fixture loads without exception
[ ] new fixture loads in the previous build? (only when forward policy claims it)
[ ] round-trip test updated
[ ] ledger schema_version incremented
[ ] release note sentence drafted
```

## III.2 The migration ladder

### III.2.1 Ladder invariants

```text
M1  migrations are pure functions: old -> new, no side effects, no I/O
M2  migrations are ordered and dense; the ladder is walked step by step
M3  every step has a fixture that exercises it
M4  a step never deletes information silently; drops are documented
M5  failure of a step aborts the load with a typed error; no partial apply
M6  migrations do not run on every load; a save at current version is untouched
```

### III.2.2 The ladder in practice

```text
v5 -> v6 : flags widened. old flags string parsed into map; unknown -> false.
v6 -> v7 : statics became session services. old serialized service blobs
           dropped; services rebuilt at bootstrap; ledger notes the null-safe
           load requirement in Main.Lifecycle.
v7 -> v8 : (proposed) dose ledger section moves to DoseLedgerSave ownership;
           old embedded dose fields converted once, then ignored.
```

### III.2.3 Legacy-neutral fare

The frozen Holdfast save and older envelopes must load with *authored defaults* for
anything absent. The design rule:

```text
N1  absence is a fact, not an error
N2  defaults are authored constants, not zero-by-accident
N3  neutral load is tested for every section (missing-section test)
N4  a neutral load must be playable, even if thin
```

## III.3 Checksum and determinism

### III.3.1 The checksum input policy

```text
C1  input = ordered concatenation of section keys + canonical section payloads
C2  canonical means: sorted keys, invariant numbers, by-name enums, no
    timestamps, no machine data, no culture
C3  the checksum covers durable state only: transient UI state is absent by
    construction
C4  the checksum algorithm and its input policy are versioned with the envelope
C5  equality means equality across machines, cultures, and load order
```

### III.3.2 The culture trap, worked

A save written under `tr-TR` renders the float `1.5` as `"1,5"` and the date
`2027-03-01` in local form. Two machines then disagree on the checksum for the
same logical state. The repair is not a patch at one call site; it is the
serializer-level culture rule (W5) plus a test that runs the digest under a
switched culture and asserts equality.

### III.3.3 RNG capture contract

```text
R1  the seeded RNG is the only randomness source in Core (Rule 4)
R2  a consumer that persists decisions persists the rng cursor with them
R3  replay: same seed + same inputs => same cursor sequence => same checksum
R4  a save/load boundary never reseeds from wall clock or hash order
```

### III.3.4 Paired replay test shape

```text
run A: fresh session, seed S, scripted 30 days, checksum H1
run B: load A at day 15, continue to day 30, checksum H2
assert H1 == H2
```

If H1 != H2 the divergence is in either a non-captured cursor (capture bug) or an
order-dependent digest (canonicalization bug). The test names which by bisecting
across sections.

*End of Part III. Continues in Part IV (corruption, slots, cross-run).*---

# W4-01 · PART IV — DEEP DESIGN: CORRUPTION, SLOTS, CROSS-RUN (POINTS 6–8)

## IV.1 Corruption detection and recovery

### IV.1.1 Failure taxonomy

| Class | Cause | Detection | Required response |
|---|---|---|---|
| truncation | power loss mid-write | length/parse failure | fall back to last-good; message |
| tamper | edited bytes | checksum mismatch | refuse; offer backup; never overwrite |
| partial | two writers raced | envelope validation | refuse; log a defect; keep both files |
| schema | older build | version + migration | migrate or refuse with reason |
| media | disk error | I/O result | typed error; retry once; message |

### IV.1.2 The write protocol

```text
1. serialize to a temp file in the same directory
2. flush and close
3. verify the temp (parse + checksum)
4. rotate: current -> backup (one generation)
5. atomic replace: temp -> current
6. verify the current
```

The protocol's cost is negligible at save cadence; its benefit is that the last
good save is always one step behind at worst, never destroyed.

### IV.1.3 Recovery messaging

```text
"Save could not be loaded: the file is damaged."
"The previous save from day 409 is available."
[Load previous]  [Back]

No stack trace. No file path in player copy. No silent partial state.
```

### IV.1.4 The corruption test kit

```text
K1  truncate the file at 50%, load: expect typed failure + backup offered
K2  flip one payload byte: expect checksum failure + backup offered
K3  corrupt the backup, load: expect primary works
K4  set version far future: expect refusal with reason, no crash
K5  empty file: expect refusal with reason
K6  valid file, missing section: expect neutral load
```

## IV.2 Slot authority and lifecycle

### IV.2.1 Write triggers (the complete list)

| Trigger | When | Retention |
|---|---|---|
| autosave | authored cadence (day boundaries/milestones) | rolling N (authored) |
| quicksave | player command | one |
| manual | player command | player count |
| exit | leaving session | one |
| checkpoint | major arc transitions (authored) | one |

Every trigger routes through the orchestrator; no system writes on its own timer.
A save that happens mid-crossing/mid-dive uses the session resume pattern (W4-02)
rather than snapshotting a half-state.

### IV.2.2 Slot-switch protocol

```text
1. quiesce systems (no ticks mid-switch)
2. write nothing; the old session is abandoned per player intent
3. tear down: lifecycle reset nulls every session service (R2/R3)
4. build new session from envelope
5. restore sections in dependency order
6. resume ticks
7. assert: no reference identity from the old session remains
```

### IV.2.3 The leakage test

```text
load slot A; record identity of 12 named services and 40 sampled state values
load slot B
assert every sampled identity is fresh and every value matches B's fixture
```

## IV.3 The cross-run boundary

### IV.3.1 What may live outside a slot (DEC-20)

```text
allowed: cross-run history summaries (completions, outcomes), authored meta
         unlocks with no live gameplay effect, settings, keybinds
forbidden: any value a live system reads to decide an outcome
```

### IV.3.2 The boundary test

For each profile-store field, the test asks: "Does any gameplay system read this
during a campaign?" If yes, the field is a defect and moves into a slot (or the
reader is rerouted). The profile store has its own version, its own migration, and
a deletion command that clears it without touching slots.

### IV.3.3 Privacy and portability

```text
P1  local-only; no network; no PII
P2  deletion is complete and provable by test
P3  exports (if ever added) are explicit, user-initiated, and documented
```

*End of Part IV. Continues in Part V (budgets and surfaces).*---

# W4-01 · PART V — DEEP DESIGN: BUDGETS AND SURFACES (POINTS 9–10)

## V.1 Size and performance budgets

### V.1.1 Budget table (proposed starting points; measured at P0)

| Section class | Soft ceiling | Hard ceiling | Notes |
|---|---|---|---|
| campaign core | 40 KB | 80 KB | day/phase/flags/rng |
| survivor records (per 100) | 60 KB | 120 KB | bounded fields only |
| world/knowledge | 40 KB | 90 KB | tiers, evolution state |
| inventory/equipment | 30 KB | 60 KB | stacks bounded |
| medical/dose | 20 KB | 40 KB | ledger rows capped |
| narrative/journal | 60 KB | 120 KB | ring buffer if needed |
| all sections total | 400 KB | 800 KB | campaign at day 200 |

Capture time targets: full save ≤ 120 ms at day 200 on target hardware; per-day
autosave overhead invisible (< 5 ms amortized).

### V.1.2 The unbounded-growth audit

```text
audit list: journals, medical logs, encounter history, debt rows, event queues,
            discovery lists, broadcast archives, ledger rows
rule: every growing list has a cap, a retention policy, and a written reason
      for its number
repair: rings for history, compaction for ledgers, authored retention windows
```

### V.1.3 The size-regression check

The focused pipeline records payload size at fixture day counts (30/90/200) and
fails when a section exceeds its hard ceiling or total size grows > 15% without an
evidence note in the package.

## V.2 Save/load surfaces

### V.2.1 Slot metadata contract

Every slot surface shows, from the slot service (never cached):

```text
campaign day | shelter name | real timestamp | build version | schema version
state: ok | migrating | damaged | incompatible  (authored text refs)
```

### V.2.2 Interaction rules

```text
S1  load is cancellable and shows progress in restrained language
S2  save shows the post-save state truthfully (no "saved!" before the write)
S3  a failed save leaves the previous state and says so
S4  overwrite requires confirmation naming the slot
S5  migration shows a one-line notice before load when it will occur
```

### V.2.3 Failure copy (canonical refs)

```text
save_error_write      "The save could not be written. The previous save is intact."
save_error_load       "This save could not be opened. It may be from a newer build."
save_error_damaged    "This save is damaged. The previous save from day {day} is available."
save_notice_migrated  "This save was updated from an earlier version."
save_backup_restored  "The previous save was restored."
```

### V.2.4 The stale-metadata case

An external file change (cloud folder sync, manual copy) can age slot metadata
behind the surface. The repair is a refresh-on-open rule plus a filesystem
timestamp check; the surface never caches across open.

*End of Part V. Continues in Part VI (playbooks).*---

# W4-01 · PART VI — PLAYBOOKS

## VI.1 The P0 audit playbook (half day per package)

```text
1. freeze HEAD; record it
2. enumerate: registry entries, envelope keys, *Save types, stateful owners
3. build the section ledger from source, not memory
4. run the ledger gate; record failures
5. sample three sections; hand-verify capture/restore correspondence
6. list lifecycle reset paths; find named services not nulled
7. list every write trigger; find writers outside the orchestrator
8. write the premise note: what was verified, what contradicts the plan
9. claim paths; prepare the package row
```

Output: `P0_LEDGER.md`, `P0_PREMISES.md`, package row draft. No code change.

## VI.2 The section authoring playbook (for any system adding state)

```text
1. is the data durable? if it can be recomputed, don't persist it
2. does an owner exist? if not, stop: Rule 5 forbids a second owner
3. declare the record: fields, types, defaults
4. write capture/restore in the owner (not the host)
5. register the section; assign a stable key
6. write the round-trip test (populated + neutral fixtures)
7. add the ledger row with size budget and test name
8. if shape changed: bump version, write migration, add fixture
9. update any surface read (through owners)
```

Checklist per PR touching state:

```text
[ ] owner unchanged or explicitly extended
[ ] capture/restore symmetric (or noted)
[ ] round-trip test exists and runs alone first
[ ] neutral load verified
[ ] ledger updated
[ ] checksum input unchanged or policy updated deliberately
[ ] no host-side serialization
```

## VI.3 The migration authoring playbook

```text
1. add a fixture at the old version (store it under tests/fixtures/saves/)
2. write the pure transform old -> new
3. add a unit test: fixture in, current out, invariants asserted
4. verify the full ladder: every older fixture still loads
5. update the ledger and release note sentence
6. check checksum stability: same content, new version, deterministic digest
7. check size effect: a migration may grow the payload; budget it
```

Migration review questions:

```text
- what happens to a player mid-arc when this field changes meaning?
- what is the authored default for absent data, and is it fair?
- can the transform fail? what does the player see then?
- is the old fixture committed so this never regresses?
```

## VI.4 The corruption response playbook (support)

```text
1. never ask the player to send a save with personal data (there is none)
2. reproduce with the reported class: truncation/tamper/schema
3. check the backup generation and the migration ladder
4. if reproducible: add a fixture; add a test; fix class-wide, not one file
5. if media-related: document; no code change without a deterministic repro
6. update the failure taxonomy if a new class appeared
```

## VI.5 The slot-switch review playbook

```text
1. run the leakage test with current fixtures
2. when it fails, bisect by service identity, then by value sample
3. find the surviving reference: static? event subscription? timer? cache?
4. repair at the lifecycle owner (null and recreate), not at the leak site only
5. add the named service to the leakage test list so it can never regress
6. re-run: leak test + round-trip + Triad
```

## VI.6 The release playbook (save-facing)

```text
[ ] compatibility statement written: what loads, what migrates, what defaults
[ ] migration ladder exercised against all committed fixtures
[ ] checksum policy unchanged or versioned
[ ] size budget report reviewed
[ ] surface copy updated for any new states
[ ] backup/rotation verified on a dirty profile
```

## VI.7 Anti-pattern drills

**Drill 1 — the convenience cache.** A panel caches slot metadata to "avoid re-reading."
Player syncs a folder; the panel lies. Fix: refresh on open; owner read; no cache.

**Drill 2 — the helpful autosave.** A system calls "save" after every mutation "so
nothing is lost." Writes race, corruption class appears. Fix: orchestrator triggers;
mutations mark dirty; cadence authored.

**Drill 3 — the orphan section.** A new system serializes state; the ledger has no
row; a later refactor deletes the writer. The save bloats with dead bytes. Fix:
ledger gate rejects unregistered writers at CI time.

**Drill 4 — the wall-clock id.** A save stores `DateTime.Now` in a durable field.
Two runs differ; checksums diverge; replay "mysteriously" fails. Fix: no clock in
durable state; gameplay time is the campaign clock.

**Drill 5 — the partial restore.** Restore half-applies, throws, and leaves live
state mixed. Fix: stage the decoded record fully before touching live state;
apply only after decode succeeds.

*End of Part VI. Continues in Part VII (verification catalog).*---

# W4-01 · PART VII — VERIFICATION CATALOG

> Three tiers per `TEST_POLICY.md`. T1 is static and seconds-fast; T2 is focused
> and bounded; T3 is a seeded soak with a written reason. No tier runs the full
> suite.

## VII.1 T1 — static checks

| ID | Check | Fails when |
|---|---|---|
| T1.1 | ledger gate | unregistered writer, duplicate key/owner, migration gap |
| T1.2 | private-save scan | `*Save` file outside the envelope/registry family |
| T1.3 | host-serialization scan | serialization call inside `Main.*`/panels |
| T1.4 | culture scan | `ToString()` without invariant culture in save paths |
| T1.5 | clock scan | `DateTime.Now`/`Environment.TickCount` in durable writers |
| T1.6 | unbounded scan | growing collections in durable fields without caps |
| T1.7 | checksum-input audit | non-canonical member in digest input list |
| T1.8 | surface-source audit | save surfaces reading anything but the slot service |

All T1 checks are grep/AST-level and run in seconds.

## VII.2 T2 — focused tests per point

### Point 1 — inventory
```text
T2.1.1 registry enumerates; every row has owner + version + test
T2.1.2 duplicate key rejected with typed error
T2.1.3 owner-with-save-type but no row => ledger gate fails (negative test)
```

### Point 2 — round-trip
```text
T2.2.1 per-section populated round-trip (matrix-driven)
T2.2.2 per-section neutral fixture (missing fields => defaults, no exception)
T2.2.3 asymmetric field with note => test asserts intentional asymmetry
T2.2.4 derived-value policy: derived fields absent from the wire
```

### Point 3 — wire contract
```text
T2.3.1 enum-by-name: rename survives, ordinal change does not affect
T2.3.2 unknown member ignored, load succeeds
T2.3.3 missing member default applied, load succeeds
T2.3.4 number formatting invariant under culture switch
```

### Point 4 — migration
```text
T2.4.1 each ladder step: fixture(N) -> current, invariants asserted
T2.4.2 full ladder: fixture(initial) -> current in one load
T2.4.3 failing transform returns typed error; live state untouched
T2.4.4 a current-version save is not transformed (no-op proof)
```

### Point 5 — checksum/determinism
```text
T2.5.1 paired replay: run A vs load-continue run B => equal digest
T2.5.2 culture-switch replay => equal digest
T2.5.3 shuffle section load order => equal digest
T2.5.4 rng cursor round-trips exactly
```

### Point 6 — corruption/recovery
```text
T2.6.1 truncation => typed failure + backup offered
T2.6.2 tamper => checksum failure + backup offered
T2.6.3 backup fallback loads
T2.6.4 future-version refusal with reason
T2.6.5 atomic write: no partial current after simulated crash between steps
```

### Point 7 — slots/lifecycle
```text
T2.7.1 slot-switch leakage (identity + value sample)
T2.7.2 double-write guard: second writer refused
T2.7.3 reset-null audit: every named service fresh after reset
T2.7.4 save-trigger inventory: no writer outside orchestrator (negative test)
```

### Point 8 — cross-run boundary
```text
T2.8.1 profile round-trip + version
T2.8.2 deletion clears fully (no residue)
T2.8.3 gameplay-isolation: no live reader of profile fields (audit test)
```

### Point 9 — budgets
```text
T2.9.1 size ledger snapshot at day 30/90/200 within ceilings
T2.9.2 capture time under budget at day 200
T2.9.3 bounded collections: history caps enforced
```

### Point 10 — surfaces
```text
T2.10.1 slot metadata truth after external file change
T2.10.2 save failure leaves previous state + canonical message
T2.10.3 migration notice shown when applicable
T2.10.4 W3-06 kits over save/load surfaces (route/focus/lane)
```

## VII.3 T3 — seeded soak

```
run: 60 campaign days, seeded, autosave + quicksave + manual mix
assert: size trend bounded; capture time trend bounded; zero checksum drift;
        zero unregistered sections; zero exceptions in save paths
evidence: size/time chart, digest series, event log extract
```

Soak only runs when a save-shape change lands; it is not a standing cost.

## VII.4 Evidence formats

```yaml
run: T2.2.1
date: 2026-09-21
head: 5be1a30a
section: holdfast.campaign
result: pass
fields_checked: 41
asymmetries_noted: 1 (ui_cache, intentional)
artifacts: [test-output.txt]
```

## VII.5 Coverage map

| Point | T1 | T2 | T3 |
|---|---|---|---|
| 1 inventory | ✓ | ✓ | — |
| 2 round-trip | — | ✓ | ✓ |
| 3 wire | ✓ | ✓ | — |
| 4 migration | — | ✓ | ✓ |
| 5 checksum | ✓ | ✓ | ✓ |
| 6 corruption | — | ✓ | — |
| 7 slots | ✓ | ✓ | ✓ |
| 8 cross-run | — | ✓ | — |
| 9 budgets | ✓ | ✓ | ✓ |
| 10 surfaces | ✓ | ✓ | — |

*End of Part VII. Continues in Part VIII (worked threads).*---

# W4-01 · PART VIII — WORKED THREADS AND FINDINGS

> Three threads, each walked end to end. Findings are illustrative and must be
> re-verified at P0 (Rule 7); IDs are bookkeeping for the repair queue.

## VIII.1 Thread A — "the campaign that remembered the last one"

**Report:** after loading save B, the journal showed entries from save A for about
a minute; then they vanished.

**Walk:**

```text
1. reproduce: load A, play 3 days, load B, open journal immediately -> stale rows
2. bisect: journal rows come from a cached list refreshed on open
3. root: the journal panel holds a snapshot from the previous session; its
   refresh subscription survives the slot switch because the reset path does not
   null the panel's cached reference
4. deeper: the shared narrative service behind it is a session-scoped static
   that reset does not clear (D19-class hazard)
5. repair: (a) panel refresh reads owner on open, never caches across open;
   (b) lifecycle reset nulls the service; leakage test names it
6. verify: leakage test (T2.7.1), journal surface kit (T2.10.4), round-trip
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-01 | panel caches narrative rows across session | leakage | read-on-open rule |
| SA-02 | session service not nulled on reset | lifecycle | reset-null + test list |
| SA-03 | no leakage test covered the service | coverage | add to T2.7.1 list |

## VIII.2 Thread B — "the save that grew all summer"

**Report:** save files reached 2.1 MB by day 300; loading slowed visibly.

**Walk:**

```text
1. measure per section at day 30/90/200/300 -> growth concentrated in three areas
2. journal section: unbounded entries (every minor event appended)
3. medical log: unbounded rows (every dose reading persisted)
4. discovery list: duplicates appended on re-discovery (no set semantics)
5. repair: rings with authored retention for journal/log; set semantics for
   discovery; migration drops older duplicates; size ceilings enforced
6. verify: T2.9.1 size ledger; bounded-collection tests; round-trip unchanged
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-04 | journal unbounded | growth | ring, retention 400 |
| SA-05 | dose readings persisted per tick | growth | aggregate per day |
| SA-06 | discovery list allowed duplicates | growth/logic | set semantics |
| SA-07 | no size budgets existed | coverage | ceilings + regression check |

## VIII.3 Thread C — "the save that only failed on one machine"

**Report:** a player's save refused to load after switching OS language to Turkish;
others loaded the same file fine.

**Walk:**

```text
1. checksum mismatch reproduced under tr-TR; file parses fine
2. root: float field formatted with default culture in one writer; "1.5"->"1,5"
   changes the digest input
3. second site: a date field formatted locally in a debug string persisted in an
   older schema (dead field, still digesting)
4. repair: invariant culture at the serializer level (W5); drop dead field via
   migration; add culture-switch test (T2.5.2)
5. verify: paired replay + culture replay + full ladder
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-08 | culture-dependent float in digest input | determinism | invariant formatting |
| SA-09 | dead debug field persisted and hashed | hygiene | migrate-drop |
| SA-10 | no culture-switch test | coverage | add T2.5.2 |

## VIII.4 Thread D — "the autosave race"

**Report:** rarely, a quicksave written while an autosave ran produced a save that
loaded with one section missing.

**Walk:**

```text
1. reproduce with forced overlap in test harness
2. root: two writers, no mutex; atomic replace of one raced the other
3. repair: single orchestrator queue; dirty flag + coalescing; writers serialized
4. verify: T2.7.2 double-write guard; T2.6.5 atomicity; soak
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-11 | concurrent writers | integrity | serialize through orchestrator |
| SA-12 | no write-queue test | coverage | add T2.7.2 |

## VIII.5 Thread E — "the migration that ate a flag"

**Report:** after a patch, an older save loaded with a quest gate reopened.

**Walk:**

```text
1. compare fixtures: flag present in old save, absent after migration
2. root: migration v5->v6 rebuilt the flags map from a partial source and
   defaulted the missing key to false
3. repair: migration reads both legacy sources; default true only where the old
   version's gameplay implied open; fixture added
4. verify: T2.4.1 per-step invariant; full ladder; arc test (quest state)
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-13 | lossy flag transform | migration | complete source read + fixture |
| SA-14 | no invariant test on flags | coverage | add assertion set |

## VIII.6 Thread F — "the profile that steered the game"

**Report:** a "cross-run" score appeared to influence shelter morale at campaign
start.

**Walk:**

```text
1. repro: fresh campaign after a high prior score -> morale band starts higher
2. root: a live reader consulted the profile store for an authored "veteran"
   bonus; DEC-20 boundary violated (gameplay reads cross-run)
3. repair: either the bonus becomes explicit, signed, and in-slot (recorded at
   campaign creation), or it is removed; the profile store keeps only history
4. verify: T2.8.3 gameplay-isolation audit
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-15 | gameplay read cross-run store | boundary | record into slot at creation or remove |
| SA-16 | boundary audit missing | coverage | add T2.8.3 |

## VIII.7 The findings summary

| Class | Count | Pattern |
|---|---|---|
| leakage/lifecycle | 3 | session services and panel caches |
| growth | 4 | unbounded histories and logs |
| determinism | 3 | culture and digest inputs |
| integrity | 2 | writer races and atomicity |
| migration | 2 | lossy transforms, missing fixtures |
| boundary | 2 | cross-run reads, missing audit |
| coverage | 4 | missing tests that would have caught the above |

The pattern is consistent: **save defects are rarely in the serializer; they are in
ownership, bounds, and discipline around it.** The plan's gates target exactly those.

*End of Part VIII. Continues in Part IX (Q&A).*---

# W4-01 · PART IX — QUESTIONS AND ANSWERS

**Q1. Why does a plan about code need a section ledger?**
Because state ownership is invisible in code review. The ledger makes it a table
that fails a check. Without it, "who owns this field" is archaeology.

**Q2. What if two systems reasonably share state?**
Then one owns it and the other reads through a contract, or the shared piece is
extracted into its own owner with its own section. Shared mutable state without an
owner is the defect.

**Q3. How do we treat settings and keybinds?**
They are settings-domain state with their own store and version — not campaign
sections. The wave touches them only to confirm the boundary.

**Q4. Must every field round-trip exactly?**
Durable fields yes. Derived fields must not be durable at all. The matrix records
which is which.

**Q5. What about floating point?**
Avoid in durable state where possible; where unavoidable, use canonical
invariant-culture round-trip formatting and treat exactness as defined by that
format, not by binary equality.

**Q6. Why not persist display strings to save CPU?**
Because they rot: a later text fix would not reach old saves, and translation
would desync. Surfaces render from owners.

**Q7. How do we version a section independently of the envelope?**
Per-section integer versions plus an envelope format version. The envelope governs
the container; sections govern their payloads.

**Q8. What is a migration fixture and where does it live?**
A small save payload at a known older version, committed under the test tree,
loaded by tests. It makes migrations evergreen.

**Q9. How do we decide what to drop in a migration?**
If a field is derived, dead, or duplicated elsewhere: drop with a note. If it is a
fact with meaning: transform, never drop.

**Q10. When is a checksum failure a bug rather than corruption?**
When two identical logical states produce different digests. That is a
canonicalization bug; corruption is when the bytes changed.

**Q11. Can we add compression to shrink saves?**
Only if the format already supports it and the checksum covers the uncompressed
logical payload. Compression is a storage decision, not a model change.

**Q12. What about cloud sync conflicts?**
Out of scope, but the last-good backup and typed errors make the failure
survivable; the surface offers the backup.

**Q13. How often should autosave run?**
At authored milestones/day boundaries, never on a wall-clock timer, never after
every mutation. Cadence is a design parameter with a size/perf budget.

**Q14. What if the player deletes a save folder file manually?**
The slot service reports the slot absent; the surface refreshes; no crash. If a
backup remains, it is offered.

**Q15. How do we test backward compatibility forever?**
Fixtures plus the ladder test. Every released schema shape gets a committed
fixture — the cost is tiny and the coverage is permanent.

**Q16. Is the profile store versioned?**
Yes, with the same discipline: integer version, migrations, neutral load,
deletion test.

**Q17. What is the Triad?**
The save/restore/determinism gate family. It must be green for every package that
touches state.

**Q18. How do we handle mid-session save during a scripted sequence?**
Define quiesce points; scripted sequences either forbid save or record their step
in the section. W4-02 sessions (crossings/dives) specify theirs.

**Q19. What about save scumming?**
Out of scope as a prohibition; the design only guarantees determinism and honest
state. Design decisions about rerolls belong to gameplay plans.

**Q20. Who writes the release compatibility statement?**
The integrator, from the ledger + ladder + budget report.

**Q21. What is the single most common bug this plan prevents?**
The slot-switch leak: state from one campaign surviving into another.

**Q22. The second most common?**
Unbounded growth: histories that quietly inflate saves.

**Q23. How do we find leaks we cannot reproduce?**
The leakage test does not need a player report; it samples every named service on
every fixture switch. Coverage beats luck.

**Q24. Should we hash the whole file or the logical payload?**
The logical payload, canonicalized. File-level hashes change with whitespace and
compression and prove less.

**Q25. What about anti-cheat?**
Not a goal. Integrity here means "the player's state is not silently lost or
corrupted," not "the player cannot edit their own save."

**Q26. How do we treat achievements-like flags?**
If they affect gameplay: in-slot, owned, migrated. If they are pure history: the
profile store, no gameplay read. Never both.

**Q27. What does "neutral load is playable" mean exactly?**
A fixture with only defaults must boot, tick, and be savable without exceptions or
authored-missing special cases in hosts.

**Q28. How do we handle a section whose owner was deleted?**
Migration maps it to nothing (drop with note) or to its successor owner. The
ledger documents the succession.

**Q29. Can a host cache decoded state during load?**
During load, staging buffers are fine; after apply, the host holds no copy. The
restore is an event, not a data source.

**Q30. What evidence closes this plan?**
Ledger + matrix + test outputs + size/time chart + compatibility statement + the
Annex U signature.

**Q31. What is the role of the size budget at P0?**
It converts the plan's promises into numbers. A budget that is never measured is a
wish.

**Q32. Is there a "smallest possible" version of this plan?**
Yes: Path A, points 1–2 only — ledger plus round-trip. That alone removes the
largest class of unknown state.

**Q33. What does Path C add beyond A and B?**
Time: migrations as an ongoing discipline, budgets as a release contract, and
portability statements that make every future patch safe for old saves.

**Q34. How do we avoid plan sprawl?**
This plan changes no gameplay. If a fix requires gameplay design, it routes to the
owning wave (W4-02..06 or W3-*).

**Q35. The sentence that closes every review?**
"Name the owner, name the section, name the test — or it does not ship."

*End of Part IX. Continues in Part X (Path C designs).*---

# W4-01 · PART X — PATH C IMPLEMENTATION DESIGNS (C1–C10)

> Path C makes the save portable across versions as an ongoing discipline. Each
> design is a self-contained proposal under the same one-authority rule.

## X.C1 — The compatibility contract per release

```text
statement template:
  "Saves from build N-1 and N-2 load after migration; sections added since load
   neutral; sections removed are dropped with a note. No save is ever migrated
   destructively."
deliverable: a paragraph in the release notes, generated from the ledger diff
acceptance: ladder tests pass for every committed fixture
```

## X.C2 — The fixture bank

```text
structure: tests/fixtures/saves/v<version>-<shape>.save
policy: one fixture per released shape; tiny payloads; committed forever
growth: append-only; old fixtures never edited
use: ladder tests, corruption tests, size comparisons
```

## X.C3 — The migration registry

```text
shape: a single ordered list of pure transforms with from/to/note/test
enforcement: dense ladder (no gaps); each step has its own unit test
review: a migration without a fixture is rejected at T1
```

## X.C4 — The portability matrix

| Change type | Migration needed | Fixture | Note required |
|---|---|---|---|
| additive field with default | no | no | yes |
| field rename | yes | yes | yes |
| field meaning change | yes | yes | yes + arc review |
| section move between owners | yes | yes | yes |
| section removal | yes (drop) | yes | yes |
| enum member removal | yes (map) | yes | yes |

## X.C5 — The size governor

```text
per-release: budget table reviewed; hard ceilings enforced in CI
per-section: size in the ledger; growth needs an evidence note
tooling: a size report at day 30/90/200 from the soak fixture
```

## X.C6 — The determinism contract

```text
policy: any change touching digest input or rng capture ships with a paired
        replay test and a culture-switch run
enforcement: T2.5 family is mandatory for these changes
```

## X.C7 — The recovery UX ladder

```text
level 1: load fails -> offer backup (one action)
level 2: backup fails -> offer file location for manual rescue (restrained copy)
level 3: nothing loads -> start fresh is explicit, never automatic
```

## X.C8 — The version transparency surface

```text
slot rows show: build, schema, will-migrate marker
load shows: a single-line notice if migration runs
never: silent migration of a player's only copy; the backup is written first
```

## X.C9 — The long-campaign endurance design

```text
rolling autosaves: authored count, oldest recycled
budget: size/time at day 200 and day 400
retention: journals/logs bounded; the chronicle (W3-01) keeps the meaning
```

## X.C10 — The termination rule

```text
Path C ends when: ladder tested, fixtures committed, budgets enforced, replay
green under culture switch, compatibility statement generated — and every future
package inherits the checklist automatically.
```

*End of Part X. Continues in Part XI (checklists and worksheets).*---

# W4-01 · PART XI — CHECKLISTS, WORKSHEETS, AND TEMPLATES

## XI.1 The P0 worksheet

```text
PACKAGE: ________  HEAD: ________  DATE: ________
[ ] registry entries enumerated (count: ____)
[ ] *Save types enumerated (count: ____)
[ ] ledger drafted; rows: ____
[ ] ledger gate run; failures: ____
[ ] lifecycle resets enumerated (count: ____)
[ ] named services without reset: ____
[ ] write triggers enumerated (count: ____)
[ ] writers outside orchestrator: ____
[ ] premises contradicted by evidence: ____ (attach)
[ ] paths claimed; overlaps checked
```

## XI.2 The section authoring worksheet

```text
SECTION KEY: ____________  OWNER: ____________
durable? [ ] yes [ ] no -> if no, stop
recomputable? [ ] yes -> do not persist
fields (n): ____   version: ____
[ ] capture written in owner
[ ] restore written in owner
[ ] defaults authored (list top 3)
[ ] round-trip test named: ____________
[ ] neutral-load test named: ____________
[ ] ledger row added
[ ] checksum input reviewed
[ ] surface reads routed through owner
```

## XI.3 The migration worksheet

```text
FROM: ____  TO: ____  SECTION: ____________
[ ] fixture committed at FROM
[ ] transform written (pure)
[ ] invariant assertions listed (n): ____
[ ] full ladder still passes
[ ] drop-notes written (if any): ____________
[ ] release note sentence: ____________
[ ] size effect measured: +____ bytes
```

## XI.4 The corruption drill card

```text
truncate -> expect: ____________  actual: ____
tamper   -> expect: ____________  actual: ____
future   -> expect: ____________  actual: ____
empty    -> expect: ____________  actual: ____
missing section -> expect: ____________ actual: ____
```

## XI.5 The leakage drill card

```text
services in test list (n): ____
after switch:
  fresh identities: ____ / ____
  value match:      ____ / ____
leaks found: ____________
repair owner: ____________
re-run: [ ] pass
```

## XI.6 The size report template

```text
DAY   30     90      200
total KB   ____   ____    ____
max section: ____________ (____ KB)
capture ms:  ____   ____    ____
ceiling violations: ____
trend note: ____________
```

## XI.7 The compatibility note template

```text
Build ____ supports saves from builds ____ and later.
Sections added since: ____________ (load neutral)
Migrations applied: ____ (listed in ladder)
No destructive changes: [ ]
Backup written before migration on a player's only copy: [ ]
```

*End of Part XI. Continues in Part XII (field guide and closure).*---

# W4-01 · PART XII — FIELD GUIDE, MAINTENANCE CALENDAR, AND CLOSURE DISCIPLINE

## XII.1 The one-page field guide

```text
WHEN STATE CHANGES:        owner declares -> section registered -> test -> ledger
WHEN SHAPE CHANGES:        bump -> migrate -> fixture -> release note
WHEN A BUG SMELLS SAVED:   check ownership first, serializer second
WHEN A PLAYER REPORTS LOSS: reproduce the class; never hand-patch a file
WHEN IN DOUBT:             the ledger is the truth; write it down before code
```

## XII.2 The maintenance calendar

| Cadence | Task |
|---|---|
| per PR touching state | section checklist; ledger diff |
| weekly | leakage test on current fixtures; size report glance |
| per release | ladder + fixtures; compatibility note; budget review |
| seasonal | fixture bank growth; dead-section sweep; retention review |
| post-incident | taxonomy update; new fixture; class-wide fix |

## XII.3 The dead-section sweep

```text
run quarterly: for each ledger row, ask "does any consumer read this after load?"
if no consumer for two releases: propose drop-with-note (Path C) or restore
consumption (the owning wave). Dead durable state is future corruption.
```

## XII.4 The ownership review script

```text
1. pick three sections at random
2. for each: open the owner, find capture, find restore
3. ask: could any other type decide this state? (if yes: defect)
4. ask: is any field recomputable? (if yes: drop proposal)
5. ask: is any field unbounded? (if yes: cap proposal)
6. record; repeat next week
```

## XII.5 The incident protocol (save loss)

```text
1. treat as severity-1: a lost campaign is a lost trust
2. secure evidence: copies of all slot files, backup, build, platform
3. classify with the taxonomy
4. reproduce with fixtures; if impossible, record the class and harden anyway
5. fix class-wide; add fixtures and tests
6. write a short postmortem; the plan's ledger absorbs any new rule
```

## XII.6 The closure measurement

```text
[ ] ledger complete for all current sections
[ ] round-trip matrix green
[ ] ladder tested for every fixture
[ ] paired replay + culture replay green
[ ] corruption kit green
[ ] leakage kit green
[ ] cross-run boundary audit green
[ ] budgets measured and enforced
[ ] surfaces truthful; canonical messages
[ ] compatibility statement generated
```

## XII.7 The closing statement

```text
A save is a promise that tomorrow the world will still be there, exactly where
the player left it. This plan exists so that promise is kept mechanically —
by ownership, bounds, versions, and tests — and not by hope.
```

*End of Part XII. Continues in Part XIII (appendix: schemas and formats).*---

# W4-01 · PART XIII — APPENDIX: SCHEMAS, FORMATS, AND REGISTERS

## XIII.1 Envelope format (illustrative)

```jsonc
{
  "envelope_version": 3,
  "schema": "campaign",
  "written_by": { "build": "0.9.4", "platform": "linux" },
  "campaign": { "name": "...", "day": 412 },
  "sections": [ { "key": "...", "version": 7, "state": { } } ],
  "checksum": { "algorithm": "sha256", "scope": "canonical-sections" }
}
```

Notes: `written_by` is metadata, excluded from the digest; `sections` order is
canonicalized before hashing; nothing else is free-form.

## XIII.2 Section registry entry (illustrative)

```csharp
// shape only; final names verified at P0
sealed record SaveSection(
    string Key,
    int Version,
    Func<object> Capture,
    Action<object> Restore,
    object NeutralFactory);
```

The registry holds these; the envelope builder walks them in a stable order.

## XIII.3 The failure taxonomy register

| Code | Meaning | Player copy ref |
|---|---|---|
| SV-001 | truncation | `save_error_damaged` |
| SV-002 | checksum mismatch | `save_error_damaged` |
| SV-003 | future schema | `save_error_load` |
| SV-004 | migration failure | `save_error_load` |
| SV-005 | write failure | `save_error_write` |
| SV-006 | backup restored | `save_backup_restored` |
| SV-007 | write suppressed (busy) | silent + retry (no message) |

## XIII.4 The budget register (living table)

| Section | Budget | Current (day 200) | Headroom |
|---|---|---|---|
| holdfast.campaign | 80 KB | ____ | ____ |
| survivors | 120 KB | ____ | ____ |
| world | 90 KB | ____ | ____ |
| inventory | 60 KB | ____ | ____ |
| medical/dose | 40 KB | ____ | ____ |
| journal | 120 KB | ____ | ____ |
| **total** | 800 KB | ____ | ____ |

## XIII.5 The test-name register

| Area | Test |
|---|---|
| ledger | `SaveSectionLedgerTests` |
| round-trip | `<Owner>RoundTripTests` (one per section) |
| migration | `SaveMigrationLadderTests` |
| replay | `SaveReplayDeterminismTests` |
| culture | `SaveCultureInvarianceTests` |
| corruption | `SaveCorruptionRecoveryTests` |
| leakage | `SaveSlotLeakageTests` |
| boundary | `CrossRunBoundaryTests` |
| policy | `SaveDerivedFieldPolicyTests` |

## XIII.6 The ownership card (per section)

```text
SECTION: ____________  OWNER: ____________
durable since: ____  current version: ____
migrations: ____  fixtures: ____
consumers: ____  surfaces: ____
budget: ____  test: ____
notes: ____________
```

*End of Part XIII. Continues in Part XIV (reviewer packet).*---

# W4-01 · PART XIV — REVIEWER PACKET

## XIV.1 The reviewer's mandate

A reviewer of any save-touching change answers four questions:

```text
1. who owns the state, and is that unchanged?
2. what section, version, and test prove it?
3. what happens to a save that predates this change?
4. what can go wrong at write/load time, and what does the player see?
```

If any answer is missing, the change is RETURNED — not rejected — with the missing
piece named.

## XIV.2 The review script (10 minutes)

```text
[ ] ledger diff present and consistent
[ ] capture/restore symmetry (or noted asymmetry with test)
[ ] derived-field policy respected
[ ] no new writers outside the orchestrator
[ ] no culture/clock/hash-order in digest input
[ ] old fixtures still load (ladder output attached)
[ ] size delta within budget
[ ] surface changes (if any) read owners
[ ] messages canonical (no exception text)
[ ] rollback position stated
```

## XIV.3 The three review verdicts

```text
SIGNED:    all ten lines; evidence attached
RETURNED:  named gaps; the author re-submits
ESCALATED: ownership or boundary question beyond the package
```

## XIV.4 The reviewers' calibration set

```text
Example 1: additive field with default -> SIGNED with fixture note
Example 2: field dropped without note  -> RETURNED (information loss)
Example 3: new writer in a host panel  -> RETURNED (Rule 5)
Example 4: float added to digest input -> RETURNED (determinism)
Example 5: section renamed without migration -> ESCALATED (compat policy)
```

## XIV.5 The drills

**Drill A — spot the owner.** Given three fields, name the owner and the section;
time limit 3 minutes. If the answer needs code archaeology, the ledger has a gap.

**Drill B — spot the leak.** Given a diff that adds a session service, say where it
is nulled and which test names it. If neither exists, the diff is incomplete.

**Drill C — spot the growth.** Given a diff that appends to a list, say the cap and
retention. "Unbounded" is never the answer.

**Drill D — spot the culture.** Given a diff with `ToString()` on a number in a
save path, name the failure mode and the test.

**Drill E — spot the silent loss.** Given a migration that drops a field, decide:
note + fixture, or redesign. "Probably unused" is not evidence.

## XIV.6 The reviewer's card

```text
Owner? Section? Test? Old save? Failure copy?
Five questions. Ten lines. One verdict.
```

*End of Part XIV. Continues in Part XV (operations manual).*---

# W4-01 · PART XV — OPERATIONS MANUAL

## XV.1 Roles

| Role | Responsibility |
|---|---|
| section owner | declares, captures, restores, migrates its state |
| integrator | ledger gate, fixtures, compatibility statement, Triad |
| reviewer | the five questions, the ten lines, one verdict |
| support | incident protocol, taxonomy updates |
| foreman | path claims, package rows, release approval |

## XV.2 The daily rhythm (active phase)

```text
morning:  ledger gate + leakage test on the current tree
midday:   repairs in flight; fixtures added as defects are found
evening:  round-trip + ladder on the changed sections; notes filed
```

## XV.3 The weekly rhythm

```text
- dead-section sweep sample (three sections)
- size report glance; any ceiling trend flagged
- new failure classes tabled; fixtures queued
- review queue cleared to SIGNED/RETURNED only
```

## XV.4 The release rhythm

```text
T-7:  compatibility statement draft; ladder full run
T-3:  corruption kit; replay + culture runs; budget report
T-1:  surface copy check; backup/rotation on a dirty profile
T-0:  statement published with the build
```

## XV.5 Escalation paths

| Situation | Escalates to | Timing |
|---|---|---|
| ownership conflict | foreman | same day |
| digest policy change | integrator + foreman | before merge |
| player-visible loss report | severity-1 protocol | immediate |
| boundary question (cross-run) | governance (DEC-20) | before change |
| budget breach | integrator | before release |

## XV.6 The dependency map

```text
SaveSectionRegistry -> owners (capture/restore)
CampaignSaveEnvelope -> registry (order, versions)
SaveWireContract -> envelope (shape)
SaveChecksum -> envelope (canonical input)
SaveSlotService/SaveStore -> envelope (files, backups)
Orchestrator -> all of the above (triggers, quiesce)
Lifecycle -> orchestrator (reset, switch)
```

## XV.7 The stop-the-line list

```text
1. a section that cannot round-trip
2. a writer outside the orchestrator
3. digest input with culture/clock/order dependence
4. a migration without a fixture
5. an unbounded durable collection
6. a gameplay read of the cross-run store
```

Any stop-the-line item halts the release until fixed or downgraded by the foreman
with a written reason.

*End of Part XV. Continues in Part XVI (advanced topics).*---

# W4-01 · PART XVI — ADVANCED TOPICS

## XVI.1 Forward compatibility

If a save from a newer build is opened by an older one, the older build must not
silently misread state. Policy options:

```text
A  refuse with reason (default; safest)
B  read known sections, ignore unknown, warn (only if the wave signs it)
C  never allowed (silent partial reading)
```

Forward compatibility is declared, never accidental. The envelope version and an
optional "minimum reader" field support the policy.

## XVI.2 Compression and storage

```text
- allowed only as a storage-layer transform over the canonical payload
- the checksum covers the uncompressed logical payload
- the temp/verify/rotate protocol applies unchanged
- a small-save preference: no compression while payload < threshold
```

## XVI.3 Save-slot-adjacent systems

| System | Boundary |
|---|---|
| settings | own store; not campaign state |
| keybinds | own store; not campaign state |
| screenshots/notes | external files; not read during play |
| mods (future) | must register sections like any owner; no sidecar saves |

## XVI.4 Very long campaigns

```text
test fixture: seeded 400-day campaign
watch: size curve, capture time curve, archive retention, digest stability
design: histories become summarized records (chronicle) rather than raw logs
```

## XVI.5 Multi-language safety

```text
the save stores identifiers, not display text
text refs survive translation
a language switch never affects digest input
surfaces re-render from owners on language change
```

## XVI.6 Determinism at the seam

The save/load boundary is a determinism seam: crossing it must be a no-op for the
logical future. The paired replay test is the proof; section capture order must not
influence outcomes (only the digest is order-canonicalized, not gameplay).

## XVI.7 The deletion and reset design

```text
delete slot: removes slot + backups for that slot; other slots untouched
reset campaign: in-slot neutral state, not file deletion
delete profile: clears cross-run store; slots untouched
each path is tested and each shows a truthful confirmation
```

## XVI.8 What this plan deliberately does not do

```text
- no serialization rewrite
- no network/cloud
- no anti-tamper arms race
- no change to what is persisted for gameplay reasons (owners decide)
- no speculative sections for systems that do not exist
```

*End of Part XVI. Continues in Part XVII (worked threads, continued).*---

# W4-01 · PART XVII — WORKED THREADS, CONTINUED

## XVII.1 Thread G — "the dose that doubled"

**Report:** a survivor's dosimeter read roughly twice the expected value after
loading a mid-expedition save.

**Walk:**

```text
1. repro: expedition save with ambient dose tick; load; continue one day
2. compare ledgers: day's exposure appears once pre-save and once post-load
3. root: an ambient exposure tick re-applies the cursor day at restore because
   the "last applied day" marker is not persisted with the dose section
4. second look: the marker exists but in another section restored after dose,
   so dose applies before the marker is known
5. repair: persist the marker with its consumer (dose), not apart; ordering test;
   one ledger write path
6. verify: double-count scenario (T2.3 family equivalents), paired replay
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-17 | exposure marker in the wrong section | ownership | co-locate with dose |
| SA-18 | restore order not declared | order | dependency order + test |
| SA-19 | no double-apply scenario test | coverage | add scenario |

## XVII.2 Thread H — "the journal that forgot the war"

**Report:** after a migration, the journal lost its middle; recent entries remained,
early ones gone.

**Walk:**

```text
1. old fixture: entries present; after ladder: suffix only
2. root: retention ring applied during migration with "newest wins" using an
   unstable ordering (ties broken by hash order)
3. repair: stable ordering by (day, sequence id); retention applied only at
   runtime growth, not during migration; fixture re-asserts the ordered tail
4. verify: migration invariant + ring behavior test
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-20 | migration invoked runtime retention | design | separate migrate vs runtime |
| SA-21 | unstable ordering on ties | determinism | sequence id tiebreak |

## XVII.3 Thread I — "the buildings that came back"

**Report:** loading an old save after an upgrade restored a demolished structure.

**Walk:**

```text
1. compare fixtures: demolition flag absent in old save; default false
2. root: the flag lived only in a derived UI list (not durable) pre-upgrade;
   the data needed to reconstruct it was not persisted
3. repair options: (a) derive from the structure's condition history if it exists;
   (b) authored default with a migration note and a one-time rebuild offer; the
   choice is a design decision recorded in the ledger
4. verify: ladder fixture + arc test
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-22 | history necessary for meaning was not durable | design | persist fact or accept default |
| SA-23 | migration note process missing for design defaults | process | note required |

## XVII.4 Thread J — "the save that shrank"

**Report:** after cleanup, saves shrank; a player's dead-son memorial flag vanished.

**Walk:**

```text
1. audit of dropped fields: three were "probably unused" (no consumer grep hit)
2. one had a consumer through reflection/JSON by name — grep missed it
3. repair: drop audit must include dynamic consumers (reflection, data bindings);
   consumer search broadened; field restored via no-op migration for older saves
4. verify: consumer coverage test pattern; fixture
```

**Findings:**

| ID | Finding | Class | Repair |
|---|---|---|---|
| SA-24 | drop audit missed dynamic consumers | process | broaden search |
| SA-25 | no consumer-coverage pattern for durability | coverage | add pattern |

## XVII.5 The pattern summary

```text
G: co-location and order — state needed to apply a fact must live with it
H: migration vs runtime — retention is a runtime behavior, not a transform
I: durability of meaning — if a fact is needed to reconstruct history, persist it
J: consumer audits — dynamic consumers exist; searches must cover them
```

Each thread's repair is a rule that outlives the thread. The threads together are
the plan's argument: saves fail at semantics, not at serialization.

*End of Part XVII. Continues in Part XVIII (answer key and failure atlas).*---

# W4-01 · PART XVIII — THE ANSWER KEY AND FAILURE ATLAS

## XVIII.1 The answer key (common review questions)

```text
Q: state added, where does it go?        A: owner's section; ledger row first
Q: shape changed, what ships?            A: bump + migration + fixture + note
Q: computed value, persist?              A: no; recompute or refuse
Q: list grows, what bounds it?           A: cap + retention + reason
Q: two writers?                          A: one orchestrator; others mark dirty
Q: culture in digest?                    A: invariant formatting at serializer
Q: save during a scripted scene?         A: quiesce or record the step
Q: player edited their save?             A: allowed; integrity means no silent loss
Q: old save missing a section?           A: neutral defaults; playable
Q: profile field read by gameplay?       A: defect; record in slot at creation
```

## XVIII.2 The failure atlas (extended)

| Symptom | Likely owner | First check |
|---|---|---|
| values revert after load | capture asymmetry | matrix |
| values vanish between slots | lifecycle null | leakage test |
| one machine refuses the save | canonicalization | culture replay |
| slow load at day 200 | unbounded section | size report |
| duplicate application | marker mismatch | co-location audit |
| quest state flips | lossy migration | ladder fixture |
| autosave "didn't happen" | suppressed write | write queue + dirty flag |
| backup missing | rotation order | write protocol |
| profile leaks into play | boundary reader | isolation audit |
| exception on load | defaults missing | neutral fixtures |

## XVIII.3 The five-minute triage

```text
1. is it one save or all saves?        -> corruption vs model
2. is it one machine or all?           -> canonicalization
3. is it after load or after switch?   -> restore vs lifecycle
4. is it early or late campaign?       -> growth vs defaults
5. is it in one system or many?        -> owner vs envelope
```

## XVIII.4 The permanent rules (one page)

```text
R1  owner first
R2  durable = facts only
R3  additive or migrated, never silent
R4  canonical or it does not hash
R5  bounded or it does not persist
R6  one writer
R7  neutral load plays
R8  backup before replace
R9  message before stack trace
R10 the ledger is the truth
```

*End of Part XVIII. Continues in Part XIX (the save-hygiene curriculum).*---

# W4-01 · PART XIX — THE SAVE-HYGIENE CURRICULUM

> A short course for anyone who will touch durable state. Eight lessons, each
> ending with a concrete exercise.

## XIX.1 Lesson 1 — Ownership

State has one owner. If two systems need the same fact, one holds it and the
other reads. Exercise: pick three fields from a system you know; name the owner of
each. If you hesitate, the ledger has a gap.

## XIX.2 Lesson 2 — Durability

Durable means "needed to reconstruct the player's world." Exercise: sort ten
fields into durable/derived/dead. Derived fields are recomputed; dead fields are
dropped with a note.

## XIX.3 Lesson 3 — Symmetry

Capture and restore are one contract. Exercise: write the field pair table for a
section you own; mark any asymmetry and justify it in one sentence.

## XIX.4 Lesson 4 — Versions

Versions are integers on sections; migrations are pure, ordered, and tested.
Exercise: sketch the ladder for a rename you would perform, including the fixture
you would commit.

## XIX.5 Lesson 5 — Canonicalization

Machines must agree. Exercise: find one place where a number or date is formatted
for storage; convert it to invariant culture in your head and name the test.

## XIX.6 Lesson 6 — Bounds

Nothing grows without a cap and a reason. Exercise: list the three longest-lived
collections in your domain; propose caps and retention windows.

## XIX.7 Lesson 7 — Failure

Every failure has a class and a message. Exercise: write the player copy for a
write failure, a damaged file, and a migration notice — no jargon, no stack.

## XIX.8 Lesson 8 — The seam

Save/load must be a no-op for the logical future. Exercise: describe a paired
replay for your domain and what divergence would mean.

## XIX.9 The graduation exercise

Take one real section end-to-end: ledger row, round-trip test, neutral fixture,
migration note, size budget, surface reads, failure copy. When that section is
signed by a reviewer, the curriculum is complete.

*End of Part XIX. The expansion continues with Part XX in the next session.*---

# W4-01 · PART XX — EXTENDED SCENARIOS AND SOAK DESIGNS

> Ten scenarios that exercise the plan's guarantees. Each names its fixture, its
> assertions, and the failure class it would catch.

## XX.1 Scenario S1 — The long campaign

```text
fixture: seeded 400-day campaign, autosave daily, quicksave weekly
assert: size curve bounded; capture time < budget; digest stable per seed;
        archive summaries replace raw logs past retention
catches: growth, retention, digest drift
```

## XX.2 Scenario S2 — The upgrade ladder

```text
fixture: one save per released schema shape (bank)
assert: every fixture loads; migrations fire exactly once; no double transform
catches: ladder gaps, idempotency faults
```

## XX.3 Scenario S3 — The slot carousel

```text
fixture: three campaigns A/B/C; 20 switches in mixed order
assert: leakage test green each switch; no cross-contamination of flags,
        journals, rosters, or rng cursors
catches: lifecycle leaks, static retention
```

## XX.4 Scenario S4 — The hostile environment

```text
fixture: saves truncated, tampered, version-bumped beyond current, emptied
assert: typed failures; backup offered; no crash; no silent partial load
catches: corruption handling, copy accuracy
```

## XX.5 Scenario S5 — The culture switch

```text
fixture: same logical save written under three locales
assert: digests equal; loads equal; no field changes
catches: canonicalization faults
```

## XX.6 Scenario S6 — The interruption

```text
fixture: kill the process between write steps (temp, rotate, replace)
assert: current and backup are each parseable or absent; never half-written
catches: atomicity faults
```

## XX.7 Scenario S7 — The dense day

```text
fixture: 30 days with scripted heavy events (battles, outbreaks, migrations)
assert: sections stay within budgets; journal ring behaves; no lost consequences
catches: growth under load, ring semantics
```

## XX.8 Scenario S8 — The tinkerer

```text
fixture: player edits a save using a text editor (allowed)
assert: either loads (valid edit) or refuses with reason (invalid); checksum
        failure gives backup path; no hidden corruption of unrelated sections
catches: validation gaps
```

## XX.9 Scenario S9 — The replay pair

```text
fixture: scripted 60 days; run A fresh, run B load-at-30-continue
assert: digests equal; event logs equivalent; rng cursors equal
catches: determinism seams
```

## XX.10 Scenario S10 — The clean desk

```text
fixture: full ledger; every section has owner/test/budget/consumer
assert: dead-section sweep finds nothing; budget report within ceilings
catches: ownership drift, paperwork rot
```

## XX.11 Soak cadence

```text
per state-shape change: S1 (short form), S9
per release: S2, S4, S5, S9, S10
quarterly: S3, S6, S7, S8 full forms
```

*End of Part XX. Continues in Part XXI (Q&A, second band).*---

# W4-01 · PART XXI — Q&A, SECOND BAND (Q36–Q80)

**Q36. How does this plan treat mods?**
Mods must register sections like any owner: unique key, integer version, capture/
restore, neutral load, and a migration table. A mod-owned file beside the envelope
is a defect.

**Q37. What if a mod's section is missing when the mod is uninstalled?**
The section is dropped with a note; the remaining save loads neutral. Nothing else
breaks.

**Q38. Should the game support disabling a save's warnings?**
No — the failure copy is short and factual. It is not noise; it is the record that
the file wasn't lost.

**Q39. How do we treat generated content IDs in saves?**
Persist stable ids, never list indices. A content reorder must not move a
survivor's home.

**Q40. What about derived localization keys in saves?**
Never. Save identifiers; resolve text at render.

**Q41. Do we version individual fields?**
No. Section integer version only. Field-level versioning multiplies states
combinatorially.

**Q42. What is "dense ladder" in one sentence?**
From any released version, the next transform always exists; no version skips.

**Q43. How do we know a section is dead?**
No consumer reads it after load for two releases, and no migration writes it.
Then drop-with-note is proposed.

**Q44. What if a consumer exists only in a quarantined test?**
Quarantined tests are not consumers for durability purposes. The drop decision
requires current-source evidence.

**Q45. How do we handle saves created by a debug build?**
Debug builds write a marker in `written_by`; release refuses nothing but reports
the build. Debug-only fields never reach the wire.

**Q46. What about "New Game+" style state?**
If it affects play, it lives in the slot, created at campaign start; never read
live from the profile store.

**Q47. Where do we document a save's shape?**
The ledger + the wire contract comments + the ownership card. Three views, one
truth.

**Q48. How do we prevent "temporary" private writers?**
T1.2/T1.3 fail the build. Temporary becomes permanent; the gate exists because of
that.

**Q49. How do we audit a hostile save?**
Load it in the corruption kit. Parsing must never execute content from the file:
no code paths, no format strings, no reflection on names.

**Q50. What about save timestamps?**
Display-only metadata lives outside the section payload (envelope metadata),
excluded from the digest.

**Q51. Do we checksum per section?**
One digest over canonical sections. Per-section digests are a possible refinement
if diagnostics need it, with the same canonicalization rules.

**Q52. What is the recovery story for a checksum failure?**
Never overwrite; offer the backup; if the player insists, allow "load anyway"
only as an explicit, signed emergency feature — default is refuse + backup.

**Q53. Why not always keep five backups?**
Because risk is "last good," not "many goods," and disk hygiene matters. One
generation plus a rotation note; expansions may raise the count with a budget.

**Q54. How is the orchestrator tested?**
Write-trigger inventory test (no external writers), queue/coalesce test, and the
interruption scenario (S6).

**Q55. What does "quiesce" mean mechanically?**
Ticks stop; pending consequences drain to a defined point; sessions freeze; then
the write runs. Sessions that cannot quiesce must declare that their step is
persisted (W4-02).

**Q56. What if two sections must be mutually consistent at load?**
Restore in dependency order and assert the invariant; if the invariant fails, the
load is refused with the migration/repair path named. Never auto-repair silently.

**Q57. Is there a scenario for disk-full?**
Yes: write failure class; temp cleaned; previous save intact; message shown.
Added to S4's variants.

**Q58. How do we keep the ledger accurate under time pressure?**
It is part of the PR. The gate does not negotiate; a missing row fails the check.

**Q59. What is the minimum a new system must do to be "save-ready"?**
Owner + section + round-trip test + ledger row + neutral load. Four things, no
exceptions.

**Q60. What is the plan's one-line law?**
One owner, one section, one version, one digest, one writer — or it does not
ship.

**Q61. Should RNG cursors be per-system or shared?**
Per consumer that makes persisted decisions, with the shared seeded source. Co-
locate the cursor with the decisions it produced.

**Q62. What happens if a cursor is lost but decisions persist?**
The replay diverges: failures on future rolls differ. This is exactly the pattern
the paired replay test catches; treat as severity-1 for determinism.

**Q63. How do we handle localized player names in saves?**
Names are data (user input), stored verbatim; display is raw text. No formatting
of user text on the wire.

**Q64. Can we migrate while a campaign is loaded?**
No. Migration is a load-time transform. A running session is never rewritten in
place.

**Q65. What about read-only media?**
Write failure class; the session continues unsaved with a warning at the next
save attempt. Never pretend a save happened.

**Q66. Where does the compatibility statement live?**
Release notes plus the docs tree; generated from the ledger diff at release.

**Q67. What makes a fixture "tiny"?**
Just enough fields to exercise the transform: tens of records, not hundreds. The
point is version shape, not volume.

**Q68. Do we test with real player saves?**
Only with consent and only as anonymized shape references. Fixtures are synthetic
by policy; player files confirm classes, never enter the repo.

**Q69. What is the end state for diagnostics?**
A failing load names the section and the class, and support can route it in
minutes. No dump dives.

**Q70. How often do budgets change?**
Only with evidence: measured growth plus an authored reason, signed in the
package. Budgets are not vibes.

**Q71. Is there a "save version" players should see?**
Yes: schema version in slot rows, factual. It becomes meaningful when migration
occurs.

**Q72. What about achievements unlocking from saves?**
If platform achievements exist later, they read facts from the loaded state or
fire at event time; never a live read of the profile store during play.

**Q73. How do we handle a section owned by an agent-added system?**
Same as any owner. Wave 4's plans (W4-02..06) inherit this plan's checklist for
every new stateful system they add.

**Q74. What is the rule for test fidelity?**
Tests use public APIs to build fixtures. Reflection-built state hides bugs that
players will find.

**Q75. How do we keep review fast?**
The five questions; the ten lines; the drills. Review is a checklist, not a
reading circle.

**Q76. What if the ledger and code disagree?**
Code is the fact; the ledger is updated in the same PR with a note. Drift is a
defect, not a debate.

**Q77. What is the smallest meaningful Path C increment?**
A ladder plus committed fixtures for the two most recently released shapes.

**Q78. What ends the plan?**
The closure measurement (§XII.6) green and the Annex U signature. Everything
after is maintenance under these rules.

**Q79. What is the plan's attitude to player saves?**
They are the player's; the game's job is to keep them exactly, never to own them.

**Q80. The last word?**
Keep the promise: tomorrow, the world is still there.

*End of Part XXI. Continues in Part XXII (worked threads, third band).*---

# W4-01 · PART XXII — WORKED THREADS, THIRD BAND (K–P)

## XXII.1 Thread K — "the roster that kept a ghost"

**Report:** shelter roster showed a deceased survivor's name as "unassigned" after
load, blocking a job slot.

**Walk:**

```text
1. repro: death on day 40; save; load; roster row exists with null assignment
2. root: death record and roster entry are in different sections; the roster's
   removal ran before the death section restored, so the row survived with a
   dangling reference
3. repair: dependency order (death records before rosters) OR roster restore
   validates references against the survivor section and drops dangling rows
   with a test
4. verify: cross-section invariant test; slot carousel
```

| ID | Class | Repair |
|---|---|---|
| SA-26 | cross-section restore order | order contract |
| SA-27 | dangling reference tolerated | validation + test |

## XXII.2 Thread L — "the map that un-fogged itself"

**Report:** a player's unexplored region became "Surveyed" after load.

**Walk:**

```text
1. fixture comparison: tier map present; after load, one node defaulted
2. root: tier enum ordinal serialized in an old shape; migration mapped by
   ordinal after an enum reorder upstream
3. repair: serialize by name (W6); migration maps old ordinals explicitly; fixture
4. verify: enum rename/insert tests
```

| ID | Class | Repair |
|---|---|---|
| SA-28 | ordinal serialization | by-name rule |
| SA-29 | reorder without migration | fixture + map |

## XXII.3 Thread M — "the inventory that duplicated overnight"

**Report:** stack of bandages doubled after a crash-recovery load.

**Walk:**

```text
1. root: write interrupted; backup rotation restored the previous day; a
   post-save pickup "re-applied" because the pickup was journaled outside the
   save while the inventory was in it
2. insight: the two durable facts lived in different timing domains
3. repair: pickups are consequences of the same tick as their effect; no
   side-channel journaling; the consequence ledger (W3-01) records once keys
4. verify: interaction test between consequence keys and inventory sections
```

| ID | Class | Repair |
|---|---|---|
| SA-30 | split timing domains | single tick ownership |
| SA-31 | no cross-domain restore test | add interaction test |

## XXII.4 Thread N — "the 9 MB save"

**Report:** one player's save is 9 MB while others are 300 KB.

**Walk:**

```
1. section sizes: encounter history 8.4 MB
2. root: each encounter instance kept full snapshots of party state for replay
3. repair: store deltas or seeds; replay reconstructs; cap retained instances
4. migration: rewrite old snapshots to seeds where deterministic; else drop
   snapshots with note
5. verify: size ledger + replay reconstruction test
```

| ID | Class | Repair |
|---|---|---|
| SA-32 | redundant snapshots | seeds/deltas |
| SA-33 | no per-section size visibility | size ledger |

## XXII.5 Thread O — "the settings that moved into the save"

**Report:** changing keybinds mid-campaign seemed to affect a loaded save.

**Walk:**

```
1. audit: a "controls" block was serialized inside the campaign section
2. root: convenience write; settings authority duplicated (Rule 5)
3. repair: remove from campaign; settings own their store; migration drops with
   note (player-configured values are read from settings at load)
4. verify: boundary test between settings and campaign
```

| ID | Class | Repair |
|---|---|---|
| SA-34 | settings duplicated in save | boundary + drop note |
| SA-35 | no settings/campaign boundary test | add test |

## XXII.6 Thread P — "the migration that ran twice"

**Report:** after two consecutive loads, a bonus was applied twice.

**Walk:**

```
1. fixture: version N save loads; transform sets a "migrated" fact; second load
   reruns the transform because the version was never bumped after first apply
2. root: migration mutated the version only in memory; the write path did not
   persist the bumped version
3. repair: version bump is part of the envelope state after migration; test
   asserts a load-migrate-save-load cycle applies exactly once
4. verify: idempotency cycle test
```

| ID | Class | Repair |
|---|---|---|
| SA-36 | migration not persisted | version write-through |
| SA-37 | no load-migrate-save-load test | add cycle test |

## XXII.7 The third-band summary

```text
K: cross-section references — order or validate
L: enums — names on the wire, mappings in migrations
M: timing domains — one tick owns effects and their records
N: replay storage — seeds/deltas, not snapshots
O: boundaries — settings are not campaign state
P: idempotency — a migration runs once, ever
```

Findings SA-26..SA-37 join the repair queue; each repair graduates into a test.

*End of Part XXII. Continues in Part XXIII (tables appendix).*---

# W4-01 · PART XXIII — MASTER TABLE APPENDIX

## XXIII.1 The section inventory template (fill at P0)

| # | Section key | Owner type | Version | Fields | Consumers | Budget | Test | Status |
|---|---|---|---|---|---|---|---|---|
| 1 | holdfast.campaign | HoldfastSession | 7 | 41 | orchestrator, panels | 80 KB | RT-holds | live |
| 2 | expansion.hub | ExpansionHubSave | 3 | 22 | hub host, panels | 40 KB | RT-hub | live |
| 3 | expansion.quests | ExpansionQuestSave | 2 | 18 | quest host | 30 KB | RT-quest | live |
| 4 | dose.ledger | DoseLedgerSave | 2 | 14 | medical host, UI | 20 KB | RT-dose | live |
| 5 | medical.pipeline | MedicalPipelineSave | 1 | 19 | triage host | 25 KB | RT-med | live |
| 6 | medical.ward | MedicalWardSave | 1 | 12 | ward host | 15 KB | RT-ward | live |
| 7 | disease.strain | PathogenStrainSave | 1 | 9 | outbreak host | 12 KB | RT-path | live |
| 8 | branches.independent | IndependentBranchSave | 2 | 16 | branch coordinator | 20 KB | RT-ind | live |
| 9 | branches.military | MilitaryBranchSave | 2 | 16 | branch coordinator | 20 KB | RT-mil | live |
| 10 | branches.rebel | RebelBranchSave | 2 | 16 | branch coordinator | 20 KB | RT-reb | live |
| 11 | politics.prpf | PrpfSave | 2 | 13 | standing system | 18 KB | RT-prpf | live |
| 12 | politics.choices | WeightOfChoicesSave | 1 | 11 | narrative host | 15 KB | RT-choices | live |
| 13 | profile.crossrun | CrossRunProfileStore | 2 | 7 | profile panel | 8 KB | RT-profile | boundary |

Rows above are the known `*Save` family from the P0 anchor list; the table is
completed and corrected at P0 — it is a locator, not a finding.

## XXIII.2 The matrix template (per section)

| Field | Capture | Restore | Rule | Test |
|---|---|---|---|---|
| day | `Day` | `Day` | int exact | RT-01 |
| phase | `Phase` | `Phase` | enum name | RT-02 |
| flags | map | map | set equality | RT-03 |
| rng.seed | `Seed` | `Seed` | int exact | RT-04 |
| rng.cursor | `Cursor` | `Cursor` | int exact | RT-05 |
| … | … | … | … | … |

## XXIII.3 The migration table template

| From | To | Transform | Fixture | Note | Size Δ |
|---|---|---|---|---|---|
| — | — | — | — | — | — |

## XXIII.4 The failure-copy register

| Ref | Copy | Tone note |
|---|---|---|
| save_error_write | "The save could not be written. The previous save is intact." | calm, factual |
| save_error_load | "This save could not be opened. It may be from a newer build." | no blame |
| save_error_damaged | "This save is damaged. The previous save from day {day} is available." | offers path |
| save_notice_migrated | "This save was updated from an earlier version." | one line |
| save_backup_restored | "The previous save was restored." | states fact |

## XXIII.5 The trigger table

| Trigger | Cadence | Retention | Notes |
|---|---|---|---|
| autosave | authored milestones | rolling | never timer-only |
| quicksave | player command | 1 | never during unquiesced scene |
| manual | player command | player | confirmed overwrite |
| exit | leaving session | 1 | quiesces first |
| checkpoint | arc transitions | 1 | authored list |

## XXIII.6 The severity table

| Class | Player impact | Response time | Release blocker |
|---|---|---|---|
| silent loss | campaign loss | immediate | yes |
| corruption undetected | possible loss | 24 h | yes |
| migration fault | wrong state | 24 h | yes |
| digest drift | none visible | 1 week | yes (determinism) |
| growth | load time | release cycle | no (budget note) |
| surface lie | confusion | release cycle | no |

*End of Part XXIII. Continues in Part XXIV (expansion integration notes).*---

# W4-01 · PART XXIV — EXPANSION INTEGRATION NOTES

> How the other five Wave 4 plans (and future expansion systems) register durable
> state under this plan's rules. This part is the contract W4-02..06 inherit.

## XXIV.1 The five-line requirement for any new stateful system

```text
1. OWNER: one type; no shared mutable state without an owner
2. SECTION: stable key; integer version; ledger row with budget
3. TESTS: populated round-trip + neutral load, both public-API fixtures
4. BOUNDS: every collection has a cap and a retention reason
5. MIGRATION: shape changes ship with bump + fixture + note
```

A system that cannot name all five is not ready to persist. It may run in memory
and be recomputed at load — or it waits.

## XXIV.2 Per-plan notes

**W4-02 (world/travel).** New durable state likely: knowledge tiers (already owned
by the gate — extend, don't fork), evolution state, field-guide entries (ring if
growth unbounded), dive/crossing session cursors (co-locate with sessions), route
projection caches (never durable). Weather gate states (deterministic from data +
clock — recompute; do not persist).

**W4-03 (infrastructure).** New durable state: machinery condition records (bounded
per machine), pump/breaker states (facts), hardening levels (persistent), cascade
event history (ring), consumption histories (aggregate per day, not per tick).
Power grid state that is derivable from components should not be persisted twice.

**W4-04 (ecology).** New durable state: soil states per plot (bounded by plots),
crop stages (facts), wild population counters (small ints), migration position
(cursor), blight states (bounded), soil/crop histories (aggregate per season).
Yields are derived — never persist computed yields.

**W4-05 (society).** New durable state: standing values (one per faction), treaty
records (bounded), branch influence (small ints), census facts (one per resident),
verdict records (ring with retention), legitimacy inputs (derived recompute or
facts — choose one and test). Information/rumor items (bounded queue with
retention).

**W4-06 (medicine).** New durable state: affliction lists per survivor (bounded),
dose cursor (co-located with ledger), pipeline reservations (keyed, bounded),
vigil states (fact), records (ring), dependency states (fact per survivor).
Diagnostic results (facts, bounded per survivor). Medical histories (aggregate).

## XXIV.3 The cross-plan §II.1 requirements

Every plan's P0 must produce its own state ledger section, using W4-01's template,
and every package row must name its sections. The integrator's Triad gate checks
the union of all five ledgers.

## XXIV.4 The shared-nothing rule

No Wave 4 plan reads another's save section directly. Cross-plan needs are
expressed as events/contracts (e.g., W4-03 water quality read by W4-06 via its
owner). The save is not an API.

## XXIV.5 The integration checklist (per plan)

```text
[ ] state ledger section filed
[ ] five-line requirement satisfied for each new system
[ ] round-trip + neutral tests named
[ ] bounds and retention authored
[ ] migrations for any shape change since base
[ ] W4-01 ledger gate extended to the new rows
[ ] release note sentences drafted
```

*End of Part XXIV. Continues in Part XXV (governance and measurement).*---

# W4-01 · PART XXVII — WORKED THREADS, FOURTH BAND (Q–V)

## XXVII.1 Thread Q — "the vigil that resumed wrong"

**Report:** a survivor in end-of-life vigil appeared mid-pipeline after load,
with a treatment queued that had already resolved.

**Walk:**

```text
1. fixtures: vigil state and pipeline reservation lived in different sections
2. root: vigil advanced during load restore because the pipeline restored after
   it and re-read a state that had moved on
3. second look: the vigil machine legitimately depends on pipeline state for its
   next step — so either the dependency orders them, or one record owns both
4. repair: order pipeline before vigil; assert invariant (a queued item cannot
   target a resolved vigil); test the pair
5. verify: cross-section invariant scenario
```

| ID | Class | Repair |
|---|---|---|
| SA-38 | implicit cross-section dependency | declared order |
| SA-39 | no paired invariant test | add pair test |

## XXVII.2 Thread R — "the treaty that remembered its signature twice"

**Report:** loading a save after a summit applied the treaty's standing bonus
twice.

**Walk:**

```text
1. root: the summit consequence writes standing, and the treaty record also
   applies a signing bonus at restore ("ensure" logic)
2. repair: consequences apply at event time, never at restore; the restore path
   reads state, it does not perform actions (the read/act separation)
3. verify: consequence-once test with a load between apply and observe
```

| ID | Class | Repair |
|---|---|---|
| SA-40 | restore performed an action | read/act separation |
| SA-41 | no load-between test | add scenario |

## XXVII.3 Thread S — "the fields that forgot their owner"

**Report:** after a refactor, weather state persisted inside the campaign section
"for convenience."

**Walk:**

```text
1. root: the weather owner had no section; someone attached the cache to the
   nearest one
2. repair: weather is deterministic from (data, campaign clock, elevation);
   drop the cache with a migration note; recompute on load
3. verify: derived-field policy test + neutrality fixture
```

| ID | Class | Repair |
|---|---|---|
| SA-42 | cache attached to wrong owner | recompute; drop note |
| SA-43 | no derived-field policy gate | policy test |

## XXVII.4 Thread T — "the pause that saved ten times"

**Report:** a paused session saw repeated autosaves with identical content.

**Walk:**

```text
1. root: autosave trigger checked "time since last" (real seconds), not campaign
   progress; pause kept ticking the check
2. repair: triggers are campaign events; a paused game saves zero times;
   unchanged-state writes are coalesced
3. verify: trigger inventory + pause scenario
```

| ID | Class | Repair |
|---|---|---|
| SA-44 | wall-clock trigger | campaign-event triggers |
| SA-45 | no unchanged-state coalescing | dirty flag + coalesce test |

## XXVII.5 Thread U — "the save that didn't survive its own copy"

**Report:** copying the save folder while the game wrote corrupted the copy on
another machine.

**Walk:**

```text
1. root: documentation issue, not code: the game must state the safe way to
   move saves (exit first, then copy), and the backup generation exists for the
   uncopyable case
2. repair: surface copy in the save screen ("To move saves, exit the game
   first."), keep the write protocol; support script for the incident
3. verify: copy-while-playing scenario S13 variant
```

| ID | Class | Repair |
|---|---|---|
| SA-46 | prevention copy missing | surface line |
| SA-47 | no doc for the copy case | support note |

## XXVII.6 Thread V — "the profile that grew forever"

**Report:** the cross-run store accumulated one record per day across all
campaigns—tens of thousands of rows.

**Walk:**

```text
1. root: history appended per day instead of per campaign completion
2. repair: profile rows are campaign summaries (one per run); day-level history
   belongs to the slot; retention on summaries by count
3. verify: profile bounds test + deletion test
```

| ID | Class | Repair |
|---|---|---|
| SA-48 | wrong granularity in profile | summarize per run |
| SA-49 | no profile bound | cap + test |

## XXVII.7 The fourth-band summary

```text
Q: dependencies — order or merge ownership
R: restore vs act — restore reads; events act
S: derived data — never durable
T: triggers — campaign events, not wall clock
U: communication — tell the player the safe path
V: granularity — history belongs to the right store
```

The bands keep returning the same laws: ownership, order, derived policy,
trigger discipline, and honest communication. That repetition is the point.

*End of Part XXVII. Continues in Part XXVIII (Q&A, third band).*---

# W4-01 · PART XXVIII — Q&A, THIRD BAND (Q81–Q120)

**Q81. What is the read/act separation?**
Restore reads state; events act on it. A restore that triggers gameplay is a bug
even when it happens to produce the right number once.

**Q82. Why is wall-clock autosave wrong?**
Because saves are about progress, not minutes. A paused or idling game should not
accumulate identical writes, and a fast-forwarded game should not save once.

**Q83. Should loading emit a "world changed" summary?**
Restrained and optional; it may help players after long gaps. Facts only.

**Q84. Can a section be owned by a static class?**
No. Statics have lifetimes that ignore sessions. The D19c statics are the
cautionary tale.

**Q85. What happens to in-flight timers on save?**
Timers are runtime; their meaning (what the timer is for) is durable if it affects
future state. Persist the intent, not the clock.

**Q86. What does "dependency order" mean concretely?**
A declared partial order over sections, with a test asserting invariants after
restore. Cycles are defects and are redesigned, not sorted around.

**Q87. How do we handle an owner that is also a panel?**
Panels never own durable state. Extract the state to a system owner; the panel
reads.

**Q88. What if the owner is genuinely the right place but the file is a host
partial?**
Then the host partial calls into the Core owner; the capture/restore lives with
the owner's data, and the host merely orchestrates.

**Q89. What is the "one file" rule?**
One envelope per slot plus its backup. No sidecars. Debug dumps are separate and
never required to load.

**Q90. How do we handle a section that is too large but must exist?**
Aggregate: store summaries plus keys, not raw history; retention with authored
windows; or split into a ring that keeps meaning per W3-01's chronicle design.

**Q91. Is binary serialization allowed?**
If the wire contract supports it with the same canonicalization and fixtures, yes
— but text is debuggable and the current spine is text; the plan does not
churn formats.

**Q92. What do we do when two migrations conflict?**
The ladder is linear by definition. A conflict means someone edited history;
rebuild the ladder from the fixtures, never ship two paths.

**Q93. What is the smallest possible migration test?**
One fixture, one step, one invariant list. If it takes longer to write than the
migration, the migration is too broad.

**Q94. How are nested records versioned?**
With their section. Do not version sub-records independently; that path leads to
combinatorial states.

**Q95. When can we delete the old backup?**
On successful rotate, one generation stays; older generations are removed by
retention. The previous good state must always exist at least until the next
successful save.

**Q96. What is the policy for "cheating" a corrupted save back to life?**
Allowed only as an explicit support/expert path, logged, and never automatic. The
default path preserves data.

**Q97. How do saves interact with the string freeze (D22)?**
Save state carries identifiers; any new string refs follow freeze policy and the
corpus routing (W2-06). No display strings in saves.

**Q98. What is the plan's stance on save editing by players?**
Allowed; the game neither fights nor requires it. Validation must fail safely.

**Q99. How do we verify the ledger in CI cheaply?**
Grep/AST checks over the save family plus the generated ledger; seconds, not
minutes.

**Q100. How do the wave's other plans consume this plan's rules?**
By the §XXIV five-line requirement and their own P0 ledgers; the integrator
checks the union.

**Q101. What is the incident threshold for a public postmortem?**
Any silent-loss incident: written, brief, public within the release cycle.

**Q102. Do we need to save UI state?**
Window positions/settings: settings store. Panel state that is gameplay-factual
(e.g., selected survivor): derive from campaign facts or leave transient. Never
persist transient UI as campaign state.

**Q103. What about a "resume where you were" feature?**
That is campaign state (scene + step) and must be owned, versioned, and validated.
If a scene cannot resume, the save returns to a safe point with a notice.

**Q104. How do we test that?**
Resume fixtures per scene class; safe-point fallback test; no partial-scene
resumes.

**Q105. What is the rule for "last save wins" vs "newest file"?**
The slot service decides by slot, not by file mtime. Files are an implementation
detail.

**Q106. Should migration notices be suppressible?**
The one-line notice may be dismissible for the session; it may never be hidden
permanently. Players deserve to know their save changed.

**Q107. What is the digest algorithm policy?**
Stable, documented, and part of the wire policy; changing it is a versioned
event.

**Q108. How do we check the backup is real?**
Parse + checksum the backup on rotate, and in the recovery kit. An unverified
backup is a rumor.

**Q109. What about SSD wear from autosave?**
Write cadence is authored; atomic replace is small; budgets include bytes per
campaign day. If a device concern appears, cadence is a data parameter.

**Q110. How do we treat a save written by a future build with an older mod set?**
Policy A (refuse with reason) unless a signed compatibility mode exists.

**Q111. What is the single gate that catches the most?**
The round-trip matrix. It is boring and it is the contract.

**Q112. What is the second?**
The ladder. Fixtures are cheap and they protect every future patch.

**Q113. What is the review's job, in one line?**
To ask the five questions until the answers are boring.

**Q114. What does "done" mean for a section?**
Owner, version, matrix green, neutral fixture, budget, consumers, and a name in
the ledger.

**Q115. What does "done" mean for the plan?**
Closure measurement green, evidence pack filed, Annex U signed.

**Q116. What remains after closure?**
The calendar: weekly checks, release runs, quarterly sweeps — and the gate that
never stops.

**Q117. The one thing never to accept?**
Silent loss. Everything else can be scheduled; this cannot.

**Q118. The one habit to build?**
Write the ledger before the code.

**Q119. The one sentence for new contributors?**
Name the owner, name the section, name the test.

**Q120. The promise?**
Tomorrow, the world is still there — exactly where the player left it.

*End of Part XXVIII. Continues in Part XXIX (the save corpus development).*---

# W4-01 · PART XXIX — THE SAVE CORPUS AND TEST ASSET DEVELOPMENT

## XXIX.1 The corpus

```text
tests/fixtures/saves/
  v1-holdfast-frozen.save          (frozen legacy shape)
  v2-campaign-initial.save
  v3-campaign-with-doses.save
  v4-campaign-branches.save
  v5-campaign-flags-string.save    (pre-widening)
  v6-campaign-flags-map.save
  v7-campaign-statics-split.save
  corrupt-truncated.save
  corrupt-tampered.save
  corrupt-empty.save
  profile-v1.store
  profile-v2.store
```

Policy: fixtures are synthetic, tiny, committed, never edited once released.
They are the plan's historical memory.

## XXIX.2 Fixture authoring rules

```text
F1  build via a small generator script kept in the test tree, not by hand
F2  each fixture names: what shape it captures, why it exists, which ladder step
    consumes it
F3  payloads stay under 8 KB unless the point is size
F4  no personal data, no real names; placeholders only
F5  a fixture without a test is dead weight and is removed
```

## XXIX.3 The generator pattern

```text
fixture-gen --shape v6 --out v6-campaign-flags-map.save
  reads a compact spec (yaml), emits the wire bytes with the historical
  serializer rules of that version
```

The generator makes it possible to produce a new historical shape when a pillar
changes, without hand-crafting bytes.

## XXIX.4 The corruption kit corpus

```text
truncate: cut at 25/50/75%
tamper: flip a byte inside a section payload; flip one in the digest
empty: zero bytes; header only
future: version+10; unknown section key
```

Each corruption fixture pairs with an expected typed failure and copy ref.

## XXIX.5 The size corpus

```text
campaign-day-30.save / day-90.save / day-200.save / day-400.save
```

Generated by the soak recipe; used by the budget report and the size governor.

## XXIX.6 The corpus calendar

| Cadence | Action |
|---|---|
| per release | add one fixture for the released shape if changed |
| quarterly | verify every fixture still loads; prune dead tests only |
| yearly | corpus review: names, notes, and generator scripts current |

## XXIX.7 The corpus anti-patterns

```text
- editing a released fixture to "make the test pass" (forbidden)
- fixtures built by reflection (hidden coupling)
- fixture per bug instead of per shape (skipping the ladder logic)
- growing fixtures to full campaigns (slow; defeats the purpose)
- storing fixtures outside the repo (they disappear with the CI cache)
```

*End of Part XXIX. Continues in Part XXX (wire examples appendix).*---

# W4-01 · PART XXX — WIRE EXAMPLES APPENDIX

> Worked shapes for the most common serialization situations. Illustrative; the
> final field names are verified at P0.

## XXX.1 A flag map

```jsonc
"flags": {
  "gate_opened": true,
  "well_capped": false,
  "mara_left": true
}
```

Rules: keys are stable identifiers; unknown keys load as missing (false);
ordering canonicalized for the digest.

## XXX.2 A bounded history ring

```jsonc
"journal": {
  "entries": [ { "id": "j_0212", "day": 212, "kind": "loss" } ],
  "retained": 400,
  "dropped_total": 63
}
```

Rules: the ring cap is authored; `dropped_total` preserves the truth that history
continued beyond retention.

## XXX.3 A per-survivor record

```jsonc
"survivors": [ {
  "id": "sv_014",
  "name_ref": "survivor_014",
  "body": { "limbs": [ "left_leg_loss" ], "grip": "simple_only" },
  "dose_cursor": 18844,
  "vigil": null
} ]
```

Rules: ids stable; names are data or refs per source; body facts minimal; cursor
co-located with decisions.

## XXX.4 A reservation ledger

```jsonc
"reservations": [ {
  "key": "res_0093",
  "kind": "surgery",
  "patient": "sv_014",
  "operator": "sv_002",
  "day": 213,
  "state": "scheduled"
} ]
```

Rules: keys unique; state is an enum by name; resolved rows move to a bounded
record list, not deleted silently.

## XXX.5 A knowledge tier map

```jsonc
"map_knowledge": {
  "nodes": { "n_07": "surveyed", "n_12": "rumored" },
  "sources": [ { "node": "n_07", "source": "expedition", "day": 180 } ]
}
```

Rules: tiers are names; sources preserve provenance; absent nodes are Unknown by
definition (no default rows needed).

## XXX.6 A treaty record

```jsonc
"treaties": [ {
  "id": "t_003",
  "party": "f_ash",
  "terms": [ "grain_share", "safe_passage" ],
  "signed_day": 140,
  "duration": 120,
  "state": "active",
  "breaches": []
} ]
```

Rules: terms are identifiers; expiry derives from day+duration (not persisted);
breach history bounded.

## XXX.7 The envelope assembly order

```text
1. enumerate registry in stable key order
2. capture each; canonicalize
3. compute digest over the canonical payload
4. write metadata (build/platform; excluded from digest)
5. write envelope
6. temp -> verify -> rotate -> replace
```

## XXX.8 The load pipeline

```text
1. read + parse envelope
2. policy check (version/boundary)
3. walk ladder if needed
4. compute + verify digest
5. stage all records
6. apply in dependency order
7. assert invariants
8. emit "session ready"
```

No step acts on the world except step 6 (restore), and step 6 only writes state.

*End of Part XXX. Continues in Part XXXI (reviewer case files).*---

# W4-01 · PART XXXI — REVIEWER CASE FILES

> Four worked review cases showing verdict discipline. Each is a pattern a
> reviewer will meet again.

## XXXI.1 Case 1 — the "small" additive field

**Diff:** adds `last_taught_day` to the apprenticeship section, default `-1`.

**Review:**

```text
owner?        unchanged (apprenticeship save)
section?      existing row; version 2 -> 3
test?         round-trip updated; neutral fixture returns -1
old save?     absent field defaults -1 (no lesson in progress); playable
failure copy? none needed
size?         +8 bytes per record; within budget
verdict:      SIGNED with note "default -1 means no active lesson"
```

## XXXI.2 Case 2 — the field that "nobody used"

**Diff:** removes `mood_cache` from the survivor record; grep showed no consumers.

**Review:**

```text
dynamic consumers? one data binding reads the key by name in the roster panel
verdict: RETURNED — restore or migrate-derive; a name-based consumer exists
```

Lesson: the consumer search must include data bindings and reflection by name;
the case is now a drill (J/SA-24).

## XXXI.3 Case 3 — the convenient static

**Diff:** caches a translated string table in a static for speed.

**Review:**

```text
rule 5?    not state ownership per se, but session-scoped content
risk?      stale after language change; survives slot switch
verdict:   RETURNED — render-time bind or session-scoped store with reset
```

Lesson: caches without lifetime ownership are leaks waiting to be filed.

## XXXI.4 Case 4 — the migration that "simplifies"

**Diff:** during ladder, drops a legacy "notes" field deemed obsolete.

**Review:**

```text
information loss? notes were player-written in an early build
alternative?      keep as a bounded archive list; one entry per old note
verdict:          RETURNED — players' words are data; transform, never drop
```

Lesson: when the data is the player's own creation, the default flips: preserve.

## XXXI.5 The review patterns

| Pattern | Tell | Verdict |
|---|---|---|
| additive + default + test | small, clean | SIGNED |
| removal with grep-only evidence | "no consumers" | RETURNED |
| cache for speed | static or field | RETURNED until lifetime owned |
| simplification that drops | "obsolete" | RETURNED if player-created |
| rename with mapping + fixture | complete | SIGNED |
| boundary read | profile used in play | ESCALATED |

## XXXI.6 The case-file habit

Every RETURNED review gets one paragraph in the case file: diff summary, the
question that failed, the repair. Over a year this file becomes the reviewer's
training set — and the plan's living memory.

*End of Part XXXI. Continues in Part XXXII (rollout plan).*---

# W4-01 · PART XXXII — ROLLOUT PLAN

## XXXII.1 Week 1 — the ledger

```text
mon: P0 audit; enumerate registry/owners/saves
tue: draft ledger; run gate; record failures
wed: sample three sections; hand-verify
thu: lifecycle resets; named services list
fri: premise note; package row draft; claim paths
exit: ledger filed; P0 worksheet complete
```

## XXXII.2 Week 2 — round-trip

```text
mon: matrix for the four largest sections
tue–wed: repairs for asymmetries found
thu: neutral fixtures for all sections
fri: T2 run on changed sections; review
exit: round-trip green; neutral load green
```

## XXXII.3 Week 3 — versioning and migration

```text
mon: wire contract audit; culture/clock scans
tue: ladder reconstruction from current shapes
wed: fixture authoring (v5..v7 as needed)
thu: ladder tests; failing transforms fixed
fri: release-note sentences; review
exit: ladder green; fixtures committed
```

## XXXII.4 Week 4 — determinism and recovery

```text
mon: checksum input policy check; ordered digest
tue: paired replay + culture replay
wed: corruption kit; write protocol verification
thu: backup/rotation on a dirty profile
fri: evidence pack part 1; review
exit: replay green; recovery green
```

## XXXII.5 Week 5 — slots and boundaries

```text
mon: leakage test list; first run
tue–wed: leak repairs (D19-class services)
thu: cross-run boundary audit; profile tests
fri: write-trigger inventory; orchestrator tests
exit: leakage green; boundary green
```

## XXXII.6 Week 6 — budgets and surfaces

```text
mon: size ledger measurement (day 30/90/200)
tue: growth repairs; caps; retention
wed: surface audit; canonical copy
thu: W3-06 kits over save/load; stale-metadata
fri: soak short run; evidence pack part 2
exit: budgets enforced; surfaces truthful
```

## XXXII.7 Week 7 — closeout

```text
mon: closure measurement run
tue: limitations; open items; queue handoff
wed: compatibility statement; release notes
thu: Annex U signature; foreman review
fri: retrospective; calendar starts
exit: plan closed under L3 maturity
```

## XXXII.8 Staffing and effort

```text
one builder full-time for 7 weeks, or two builders for 4 with a shared
integrator. The plan is deliberately linear: each week's exit is the next
week's premise.
```

*End of Part XXXII. Continues in Part XXXIII (risk register).*---

# W4-01 · PART XXXIII — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation | Trigger |
|---|---|---|---|---|---|
| R1 | hidden writers discovered late | medium | high | T1.2/T1.3 gates early; inventory wk 1 | failing gate |
| R2 | D19 statics spread further | medium | high | leakage test names them; reset audit | leak found |
| R3 | migration ladder gaps | medium | high | fixture bank; ladder test | missing step |
| R4 | growth unbounded in one system | high | medium | size ledger; caps | budget breach |
| R5 | culture sensitivity in new code | medium | medium | invariant rule + culture replay | replay fail |
| R6 | cross-run reads reappear | low | high | boundary audit each release | audit fail |
| R7 | surface lies about slot state | medium | medium | W3-06 kits; refresh-on-open | stale report |
| R8 | recovery path untested until needed | medium | high | corruption kit per release | incident |
| R9 | plan scope creep into gameplay | medium | medium | §6 non-goals; route to owning wave | design debate |
| R10 | fixture rot (not updated) | low | low | quarterly corpus check | check skip |

## XXXIII.1 The three headline risks

**R2 — statics.** The known D19c services are the plan's first leak target; the
repair is lifestyle change (per-session lifecycle), not a patch. Without the
leakage test, the risk returns with every new shared service.

**R3 — ladder gaps.** Each gap is a save from an era that can no longer load. The
fixture bank plus dense-ladder rule converts this from "hopefully" to "checked."

**R8 — untested recovery.** The recovery path is used on the worst day. It must be
drilled on good days: the corruption kit runs every release, not when needed.

## XXXIII.2 Risk treatment order

```text
1. R2, R3, R8: structural; fix first (weeks 3–5)
2. R1, R4, R7: discipline; gates and budgets (weeks 1, 6)
3. R5, R6: policy; enforced by replay/audit (weeks 4, 5)
4. R9, R10: governance; non-goals and calendar
```

## XXXIII.3 The residual statement

After closure, the standing risks are R4 (growth, bounded by budget enforcement),
R5 (new code, caught by replay), and R9 (scope, held by governance). All three
have mechanical guards; none rely on memory. That is the acceptable end state.

*End of Part XXXIII. Continues in Part XXXIV (the save contract).*---

# W4-01 · PART XXXIV — THE SAVE CONTRACT (FORMAL STATEMENT)

> The plan's normative core, stated once, in one place. Everything else in this
> document is elaboration.

## XXXIV.1 The contract

```text
1. Each durable fact has exactly one owner.
2. Each owner exposes capture and restore; both are total functions over
   authored defaults.
3. The envelope carries versioned sections in a canonical order.
4. Serialization is additive; shape changes are migrated, never silent.
5. The digest covers canonical durable state only; it is equal across machines,
   cultures, and load orders.
6. A save is written by one orchestrator through temp/verify/rotate/replace.
7. A load stages, checks, then applies in dependency order; it acts on nothing.
8. A save that cannot load refuses with a class and a copy; a backup is offered.
9. Every growing collection has a cap and a retention reason.
10. The player's words and history are data; migrations transform, never drop.
```

## XXXIV.2 The invariants (testable)

```text
I1  round_trip(section) == section            for every section, both fixtures
I2  load(write(s)) == s                       logical equality
I3  digest(same_state) equal under locale/order machines
I4  write triggers ⊆ orchestrator set
I5  neutral_load plays 30 days without exception
I6  corrupt(file) → typed failure + backup offer, never silent
I7  migration applied exactly once, ever
I8  no field is both durable and derived
I9  no gameplay decision reads profile state
I10 size(section, day) within budget
```

## XXXIV.3 The ownership table (final form)

| Concern | Owner | Never |
|---|---|---|
| registration | registry | ad-hoc writers |
| envelope | builder | host serialization |
| shape | wire contract | inline converters |
| ports | slot service/store | panel writes |
| integrity | checksum | culture/clock input |
| ordering | restore plan | implicit dependencies |
| meta | profile store | gameplay reads |

## XXXIV.4 The acceptance walk

```text
walk the ten invariants one by one against the evidence pack:
I1 matrix output; I2 load tests; I3 replay logs; I4 trigger inventory;
I5 neutral fixtures; I6 corruption kit; I7 idempotency cycle; I8 policy tests;
I9 boundary audit; I10 size report.
Any unwalked invariant blocks closure.
```

## XXXIV.5 The closing oath

```text
We keep the player's world exactly: where it was, what it was, and what it
meant. When we cannot, we say so clearly and keep the last good copy safe.
```

*End of Part XXXIV. Continues in Part XXXV (field guide and closure measurement).*---

# W4-01 · PART XXXVI — EXTENDED GLOSSARY AND ABBREVIATION REGISTER

| Term | Definition |
|---|---|
| additive change | a shape change that only adds fields with authored defaults |
| assert-walk | the closure ritual of checking each invariant against evidence |
| backup generation | the single previous good save retained by rotation |
| boundary | the line between campaign slots and cross-run/profile state |
| canonicalization | normalization that makes logical equality equal bytes |
| capture | the read of owner state into a serializable record |
| co-location | keeping a fact and the state needed to apply it in one section |
| dense ladder | a migration chain with no missing versions |
| derived field | a value computed from durable facts; never persisted |
| digest | the checksum over canonical durable payload |
| dirty flag | a marker that meaningful state changed since last save |
| fixture | a committed historical save payload used by ladder tests |
| neutral load | loading with authored defaults for absent data |
| orchestrator | the single writer of saves |
| owner | the one type that holds and governs a class of state |
| quiesce | the freeze of ticks before a save or switch |
| read/act separation | restore reads; events act; never the reverse |
| restore | the application of decoded state to owners |
| ring | a bounded history that drops oldest with a retention reason |
| round-trip | capture → serialize → deserialize → restore → equal |
| slot | a save destination governed by the slot service |
| stage-then-apply | decode fully before touching live state |
| Triad | the save/restore/determinism gate family |
| trigger | an authored cause for a save (milestone, command, exit) |
| wire contract | the declared serialized shape of a section |

## XXXVI.1 Abbreviations

```text
RT   round-trip
LN   neutral load
ML   migration ladder
PR   paired replay
CR   culture replay
LK   leakage kit
CK   corruption kit
BP   boundary probe
SZ   size report
SC   surface check
```

## XXXVI.2 The identifier conventions

```text
section keys:      domain.concern        (lowercase, dot)
test names:        <Owner><Concern>Tests (PascalCase)
finding ids:       SA-nn                 (sequential, never reused)
failures refs:     save_error_*          (snake_case refs)
fixture names:     v<N>-<shape>.save     (lowercase, hyphens)
```

*End of Part XXXVI. Continues in Part XXXVII (the wave interface map).*---

# W4-01 · PART XXXVII — THE WAVE 4 INTERFACE MAP

> How the six Wave 4 plans meet. This plan owns the save spine; the others own
> domains that plug into it.

## XXXVII.1 The interfaces

| From | To W4-01 | Contract |
|---|---|---|
| W4-02 world | knowledge tiers, evolution, sessions | sections + resume grammar |
| W4-03 infrastructure | machinery condition, hardening, cycle counts | sections + budgets |
| W4-04 ecology | soil, crops, populations, blight | sections + season aggregates |
| W4-05 society | standing, treaties, census, verdicts | sections + bounded records |
| W4-06 medicine | afflictions, dose cursor, reservations, vigil | sections + co-location rules |

## XXXVII.2 The cross-plan laws

```text
X1  no plan reads another plan's section directly (events/contracts only)
X2  sessions that can be saved mid-run declare quiesce or persist their step
X3  co-location: a marker and the decision it gates live together
X4  bounded histories: each plan names caps for its growing lists
X5  deterministic evolution: no wall-clock state anywhere durable
X6  every new stateful system passes the five-line requirement
```

## XXXVII.3 The shared test assets

```text
leakage service list: maintained centrally; each plan may append services
fixture bank: expands per plan with v<N> shapes under their domains
budget register: one table; row per section; plans contribute rows
failure copy register: one table; plans reuse refs, add none ad hoc
```

## XXXVII.4 The integration order

```text
1. W4-01 spine (this plan) — ledger, round-trip, ladder, replay
2. W4-02 sessions (crossings/dives) — resume grammar
3. W4-06 medical co-location (dose cursor) — the known high-risk case
4. W4-03 condition/hardening — bounded records
5. W4-04 ecology — aggregate-per-season discipline
6. W4-05 society — bounded treaty/verdict records
```

The order front-loads the interfaces most likely to expose ownership defects.

## XXXVII.5 The integrator's union ledger

```text
one file: docs/plans/wave4_integration/STATE_LEDGER_UNION.md (proposed)
content: all sections across the six plans, budgets, owners, tests
gate: the T1.1 ledger gate reads the union, not per-plan fragments
```

*End of Part XXXVII. Continues in Part XXXVIII (audit scripts and drills).*---

# W4-01 · PART XXXVIII — AUDIT SCRIPTS AND COPY-PASTE CHECKS

> Shell/AST sketches an integrator can run today. All are read-only and take
> seconds.

## XXXVIII.1 Private writer scan

```bash
# any save-writing call outside the orchestrator/registry family?
rg -n "WriteAllText|File\.Write|Serialize|Save\(" src/ Assets/Ashfall.Core \
  --glob '!**/Save/**' --glob '!**/SaveOrchestrator*' -t cs
# expectation: zero hits outside the save family (review exceptions by hand)
```

## XXXVIII.2 Culture scan in save paths

```bash
rg -n "ToString\(\)" Assets/Ashfall.Core/Save src/Main.SaveOrchestrator.cs
# expectation: zero non-invariant format calls in durable writers
```

## XXXVIII.3 Clock scan in durable writers

```bash
rg -n "DateTime\.Now|DateTime\.UtcNow|Environment\.TickCount|Stopwatch" \
  Assets/Ashfall.Core/Save Assets/Ashfall.Core/**/*Save*.cs
# expectation: zero hits in durable capture paths
```

## XXXVIII.4 Unbounded collection suspicion

```bash
rg -n "List<|Dictionary<|HashSet<" Assets/Ashfall.Core --glob '*Save*.cs'
# for each hit: find the cap; if none, file a growth finding
```

## XXXVIII.5 Owner/section correspondence

```bash
ls Assets/Ashfall.Core/**/*Save*.cs Assets/Ashfall.Core/Save/*.cs
# cross-check against the ledger rows; missing row = gate failure
```

## XXXVIII.6 Digest input review

```bash
rg -n "checksum|digest|Canonical" Assets/Ashfall.Core/Save*
# read each site; confirm canonical ordering + invariant formatting
```

## XXXVIII.7 Ledger gate (conceptual)

```text
parse ledger yaml -> set of keys
parse registry -> set of keys
parse *Save types -> set of owners
fail on any difference between the three sets
fail on duplicate keys/owners; fail on ladder gaps
```

## XXXVIII.8 The weekly one-liner set

```bash
bash scripts/run_test.sh <save-focused-target>
# plus the scans above; record outputs in the weekly note
```

These scripts do not replace tests; they catch drift before tests have something
to catch.

*End of Part XXXVIII. Continues in Part XXXIX (the closing narrative).*---

# W4-01 · PART XXXIX — THE CLOSING NARRATIVE

## XXXIX.1 What this plan was really about

The save system looks like plumbing. In play, it is memory itself. Every
relationship the player built, every scar the shelter carries, every promise the
world made — all of it lives in a file that must hold, exactly. When it holds, no
one notices. When it fails, trust fails, and no feature compensates for a lost
campaign.

So the plan is not about serializers. It is about the discipline that makes
memory reliable:

```text
ownership   — one voice per fact
bounds      — memory that stays the size of its meaning
versions    — the past remains readable
canonicity  — every machine tells the same truth
order       — restore is a state, not an action
honesty     — failures are named in the player's language
```

## XXXIX.2 The three promises

```text
P1  The world resumes exactly: same facts, same future, same meaning.
P2  The old save still opens: migrated, never mutilated.
P3  The failure is survivable: named, backed up, recoverable.
```

## XXXIX.3 The human measure

A player returning after a long absence should find their shelter where they left
it and their dead where they buried them. That sentence is the plan's real
acceptance criterion; every table above serves it.

## XXXIX.4 The mechanics of trust

```text
- the ledger makes ownership visible
- the matrix makes round-trip mechanical
- the ladder makes time safe
- the digest makes truth checkable
- the kit makes disaster survivable
- the calendar makes all of it permanent
```

## XXXIX.5 The warning

The danger is not that the plan fails loudly. It is that it succeeds for a year
and then a small convenience — one cache, one static, one wall-clock field — is
added without its owner. The gates exist for that day: they do not forget, and
they do not negotiate.

## XXXIX.6 The last line of the narrative

```text
Write it down. Own it. Bound it. Version it. Prove it. Then the world holds.
```

*End of Part XXXIX. Continues in Part XL (final control).*---

# W4-01 · PART XL — THE FINAL ANSWER KEY AND QUICK TABLES

## XL.1 The one-page quick table

| Situation | Do this | Never |
|---|---|---|
| new durable state | owner → section → test → ledger | write a file |
| shape change | bump → migrate → fixture → note | silent change |
| computed value | recompute or refuse | persist |
| growing list | cap + retention reason | unbounded append |
| two writers | orchestrator + dirty flag | race |
| culture risk | invariant formatting + replay | default culture |
| clock risk | campaign clock only | wall clock in save |
| load failure | typed class + backup offer | stack trace |
| migration | pure transform + fixture | side effects |
| restore | read state only | act on world |
| slot switch | null services + leakage test | trust memory |
| profile | summaries only | gameplay reads |
| review | five questions, ten lines | vibes |

## XL.2 The reviewer's ten lines (final form)

```text
[ ] owner named and unchanged (or extended)
[ ] section key/version/ledger row present
[ ] capture/restore symmetric (or noted + tested)
[ ] derived fields excluded
[ ] bounds present for every collection
[ ] migration + fixture for any shape change
[ ] no culture/clock/order in digest input
[ ] no new writers outside orchestrator
[ ] old fixtures load (ladder output attached)
[ ] failure copy canonical; size within budget
```

## XL.3 The closure checklist (final form)

```text
[ ] ledger complete and gated
[ ] round-trip matrix green (all sections, both fixtures)
[ ] ladder green for every fixture; idempotency proven
[ ] paired + culture + order replays green
[ ] corruption kit green; backup fallback proven
[ ] leakage kit green; named services fresh after switch
[ ] cross-run boundary audit green; deletion proven
[ ] budgets measured and enforced at day 30/90/200
[ ] surfaces truthful; copy reviewed; W3-06 kits green
[ ] evidence pack filed; Annex U signed
```

## XL.4 The escalation quick card

| Signal | Class | Action |
|---|---|---|
| save refused, backup offered | recovery | normal path; no alarm |
| silent loss reported | severity-1 | incident protocol immediately |
| digest mismatch on one machine | determinism | culture/order bisect same day |
| save grows > budget | growth | cap + retention; release note |
| migration fault | correctness | halt release; ladder fix + fixture |
| leak between slots | lifecycle | reset-null + test; audit similar services |

## XL.5 The three sentences to memorize

```text
1. Name the owner, name the section, name the test.
2. Restore reads; events act; saves write once.
3. The world resumes exactly, or we say why not.
```

*End of Part XL. Continues in Part XLI (final control).*---

---

# W4-01 · APPENDIX A — FINDINGS CROSSWALK AND REPAIR LEDGER

> All 49 findings from the worked threads, consolidated with disposition. The
> repair queue handed to implementation.

| ID | Law | Class | Disposition |
|---|---|---|---|
| SA-01 | surfaces read on open, never cache across | leakage | P1 |
| SA-02 | session services null on reset | lifecycle | P5 |
| SA-03 | leakage list maintained centrally | coverage | kit |
| SA-04 | histories have caps + reasons | growth | P6 |
| SA-05 | aggregate per day, not per tick | growth | P6 |
| SA-06 | discovery uses set semantics | growth | P6 |
| SA-07 | budgets measured, not assumed | coverage | size report |
| SA-08 | invariant formatting at serializer | determinism | P4 |
| SA-09 | dead fields dropped with note | hygiene | migration + fixture |
| SA-10 | culture replay standing gate | coverage | kit |
| SA-11 | one writer; others mark dirty | integrity | P5 |
| SA-12 | write-queue test exists | coverage | kit |
| SA-13 | transforms read all sources | migration | P3 |
| SA-14 | per-step invariants asserted | coverage | ladder |
| SA-15 | gameplay never reads profile | boundary | P5 |
| SA-16 | boundary audit standing | coverage | kit |
| SA-17 | markers co-locate with consumer | ownership | P3 |
| SA-18 | cross-section order declared | order | P3 |
| SA-19 | double-apply scenario exists | coverage | kit |
| SA-20 | migration ≠ runtime retention | design | P3 |
| SA-21 | ties broken by stable ordinal | determinism | P4 |
| SA-22 | meaning-bearing facts durable | design | decision + note |
| SA-23 | design defaults need notes | process | review |
| SA-24 | audit covers dynamic consumers | process | guidance |
| SA-25 | consumer-coverage pattern | coverage | kit |
| SA-26 | cross-section references ordered | order | P3 |
| SA-27 | dangling references validated | integrity | validation |
| SA-28 | enums by name on the wire | wire | P2 |
| SA-29 | reorders ship with mappings | migration | ladder |
| SA-30 | one tick owns effects + records | design | P3 |
| SA-31 | cross-domain interaction tested | coverage | kit |
| SA-32 | replay stores seeds/deltas | growth | P6 |
| SA-33 | per-section size visibility | coverage | size ledger |
| SA-34 | settings are not campaign state | boundary | P2 |
| SA-35 | settings/campaign boundary test | coverage | kit |
| SA-36 | migration version write-through | correctness | P3 |
| SA-37 | load-migrate-save-load test | coverage | cycle test |
| SA-38 | implicit dependencies declared | order | P3 |
| SA-39 | paired invariant tests | coverage | kit |
| SA-40 | restore reads; never acts | design | P3 |
| SA-41 | load-between consequence test | coverage | kit |
| SA-42 | caches are never durable | design | drop + fixture |
| SA-43 | derived-field policy gate | coverage | policy test |
| SA-44 | triggers are campaign events | design | P5 |
| SA-45 | unchanged-state coalescing | design | dirty flag |
| SA-46 | safe-copy guidance surfaced | communication | surface |
| SA-47 | support note for copy incidents | process | docs |
| SA-48 | profile granularity per run | boundary | P5 |
| SA-49 | profile has bounds | coverage | cap + test |

## A.1 The deduplicated laws

```text
1  ownership: one owner; markers co-locate; settings separate
2  order: declared dependencies; paired invariants
3  durability: meaning persists; derived absent; caches never
4  canonical: invariant formatting; stable tiebreaks; by-name enums
5  bounds: caps with reasons; aggregates over raw history
6  integrity: one writer; validated references; atomic replace
7  migration: dense, pure, write-through, fixture-backed, read-all-sources
8  acts: events act; restore reads; triggers are campaign events
9  communication: safe paths surfaced; failures named
10 coverage: every repair graduates to a test
```

*End of Appendix A. Continues in Appendix B (P0 checklist).*

---

# W4-01 · APPENDIX B — P0 PREMISE EVIDENCE CHECKLIST

```text
REGISTRY/ENVELOPE
[ ] SaveSectionRegistry API; enumeration count = ____
[ ] CampaignSaveEnvelope fields; version int; section list shape
[ ] CampaignEnvelopeBuilder build order; hardcoded knowledge?
[ ] SaveEnvelopeHelper read/write; error behavior
[ ] SchemaVersionedEnvelope version semantics; bump list
[ ] SaveStore paths; atomicity; backup code present/absent
[ ] SaveSlotService/Types slot model + metadata

OWNERS/WRITERS
[ ] every *Save type enumerated
[ ] capture/restore names per owner
[ ] host callers listed (orchestrator, lifecycle)
[ ] .theirs file status resolved
[ ] reset methods listed; services nulled per method

HAZARDS
[ ] D19c statics: _silentFoundry, _sharedSkillProgression,
    _sharedFactionStance — created where, nulled where
[ ] D19a SeededRng(147) fallback location/effect
[ ] culture-sensitive durable formatting found/absent
[ ] wall-clock in durable capture found/absent
[ ] unbounded durable collections found/listed

TESTS
[ ] save tests listed; quarantine manifest read
[ ] areas with no tests listed
[ ] focused commands recorded

FILES
[ ] save directory location(s); naming
[ ] profile store location/contents
[ ] debug dumps written; required to load?

VERDICT per row: VERIFIED | CONTRADICTED | NOT-FOUND (+ one line evidence).
CONTRADICTED revises the plan; NOT-FOUND drops the claim.
```

*End of Appendix B. Continues in Appendix C (cards).*

---

# W4-01 · APPENDIX C — QUICK CARDS

## C.1 Decision tree

```text
Durable? no → done. yes → Owner? no → stop (Rule 5).
Owner yes → Fact? no → recompute-or-refuse.
Fact yes → section → capture/restore → tests → ledger → shape change?
→ bump + migrate + fixture + note.
```

## C.2 Failure card

```text
TRUNCATED → typed + backup     TAMPERED → typed + backup
FUTURE    → refused            EMPTY    → refused
IO ERROR  → typed + retry      BUSY     → suppress + retry
```

## C.3 Trigger card

```text
AUTOSAVE milestones | QUICKSAVE command | MANUAL confirmed
EXIT quiesced | CHECKPOINT arcs
NEVER: tick, mutation, timer, panel
```

## C.4 Digest card

```text
INPUT canonical sections | EXCLUDE metadata/transient/derived
EQUAL across machines/locales/orders | VERSIONED policy changes
```

## C.5 Bounds card

```text
journals ring | logs per-day | ledgers resolved-archive
discoveries sets | histories summaries | queues drained/expiring
```

## C.6 Copy card

```text
write:   "The save could not be written. The previous save is intact."
load:    "This save could not be opened. It may be from a newer build."
damaged: "This save is damaged. The previous save from day {day} is available."
migrate: "This save was updated from an earlier version."
restore: "The previous save was restored."
move:    "To move saves, exit the game first."
```

## C.7 Closure card

```text
Ledger green · matrix green · ladder green · replay green · recovery green ·
leakage green · boundary green · budgets enforced · surfaces true — signed.
```

*End of Appendix C. The plan closes with Part XLI (final control) below.*

# W4-01 · PART XLI — FINAL CONTROL

## XLI.1 The document's binding summary

```text
Binding: the save contract (§XXXIV), the ten invariants (I1–I10), the stop-the-
line list (§XV.7), the permanent rules (§XVIII.4), and the closure checklist
(§XL.3). Proposal only; execution requires Annex U.
```

## XLI.2 The completion declaration

**W4-01 is complete as a plan.** Parts I–XLI + Annex U. It executes in seven
weeks (Part XXXII) under the signs named in Annex U, and it leaves behind: one
ledger, one matrix, one ladder, one digest policy, one recovery kit, one
calendar — and a save that keeps its promise.

## XLI.3 The final version record

| Version | Change |
|---|---|
| v4.0 | Part I base (plan body, paths, points, annex) |
| v4.1 | Parts II–V deep designs |
| v4.2 | Parts VI–VIII playbooks, verification, threads A–F |
| v4.3 | Parts IX–XIII Q&A, Path C, checklists, field guide, appendix |
| v4.4 | Parts XIV–XIX reviewer packet, operations, advanced, threads G–J, answer key, curriculum |
| v4.5 | Parts XX–XXVIII scenarios, Q&A 2, threads K–V, tables, expansion notes, governance, scenarios 2, Q&A 3 |
| v4.6 | Parts XXIX–XXXVI corpus, wire appendix, case files, rollout, risks, contract, closure, glossary |
| v4.7 | Parts XXXVII–XLI interface map, scripts, narrative, answer key, final control |

## XLI.4 The handoff line

```text
W4-02 begins with the ledger this plan produced:
its sessions, tiers, and evolutions enter the union ledger under the same laws.
```

*Document control: W4-01 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-01.*
---

# W4-01 · PART XLII — YEAR ONE OF THE SAVE PROGRAM

> What the discipline looks like after the plan closes. A program, not a
> project.

## XLII.1 The standing commitments

```text
C1  every PR touching state passes the ten review lines
C2  the ledger gate runs in the focused pipeline
C3  the leakage kit runs weekly on the current tree
C4  the ladder + replay run every release
C5  the corruption kit runs every release
C6  the calendar (§XXXV.3) runs every quarter
C7  the compatibility statement ships with every release
C8  the curriculum is given to every new contributor
```

## XLII.2 The quarterly view

```text
Q1  ledger refresh; fixtures current; budget re-measure
Q2  dead-section sweep; retention review; recovery drill
Q3  ownership sample; migration dry-run on the oldest fixture
Q4  full corruption kit; next-year budget; retrospective
```

## XLII.3 The signals of health

```text
- a new contributor can name owners without archaeology
- reviews are fast because the questions are boring
- no incident report has the word "unexplained"
- budgets hold; growth is caught in the week it appears
- old saves load; the ladder is boring
```

## XLII.4 The signals of rot

```text
- ledger rows missing "temporarily"
- fixtures edited to pass
- gates skipped "for this release"
- a new cache/static that no test names
- the calendar "who owns this?" unanswered
```

When three rot signals appear together, the plan's L4 state has lapsed; run a
one-week refresher (Part XXXII weeks 1, 4, and 7, abbreviated).

## XLII.5 The annual retrospective agenda

```text
1. incidents and near-misses: classes, causes, repairs
2. budget reality vs table: re-baseline or fix growth
3. ledger quality: sample ten rows; count discrepancies
4. fixture bank: add the year's shapes; remove dead tests
5. curriculum: what did new contributors stumble on?
6. contract: is anything in §XXXIV no longer true?
```

## XLII.6 The end state

```text
A save program is healthy when nobody thinks about it — and every save that
loads proves it worked.
```

*End of Part XLII. Continues in Part XLIII (final Q&A band).*
---

# W4-01 · PART XLIII — FINAL Q&A BAND (Q121–Q150)

**Q121. What makes this plan different from "just testing saves"?**
It makes ownership and bounds mechanical. Tests prove; the ledger and gates
prevent. Without the prevention half, every test is written after the bug.

**Q122. What does "the ledger is the truth" mean when the ledger is wrong?**
The ledger is corrected with evidence in the same PR that corrects the code.
Disagreement is a defect, not a debate.

**Q123. How large should a section be before splitting?**
There is no size that implies a split; there is a size that implies aggregation
or retention. Splitting sections to dodge budgets is a smell.

**Q124. Is a "cache" ever acceptable in a durable record?**
Only as a derived field marked non-durable, recomputed at restore, or omitted.
If it is durable, it must be a fact.

**Q125. What is the acceptable latency for a save?**
Invisible at day boundaries, under 120 ms full-write at day 200 on target
hardware. Numbers are re-baselined annually with evidence.

**Q126. What if the game crashes during autosave?**
The temp/replace protocol ensures current is old-or-new, never mixed. The
interruption scenario proves it quarterly.

**Q127. Who owns the backup?**
The slot service. Rotation, verification, and offering are its contract.

**Q128. What does support need to triage a save issue in five minutes?**
The failure class, the section name, the build, and the backup path. The plan's
messages and diagnostics provide all four.

**Q129. Can a player opt out of autosave?**
Cadence is a design parameter; the wave does not decide game UX. If options
exist, the mechanism stays unchanged under them.

**Q130. How do we treat "continue" from the main menu?**
That is a slot load with metadata displayed; same rules; no special path.

**Q131. Do we need to migrate the profile store too?**
Yes, independently; its ladder is separate and its deletion test proves
isolation.

**Q132. What if a future feature needs more profile state?**
It is added with version, migration, bounds, and the boundary audit — and it is
still forbidden to gameplay.

**Q133. What is the "third place" problem?**
State in three places: live, cached, saved. The plan allows two: live and saved.
Caches are the third place and they drift.

**Q134. How does the plan handle tool-assisted save inspection?**
Debug tooling may read the envelope; it must not be required to load or save and
must never write outside the orchestrator.

**Q135. What's the smallest unit of progress?**
One section: matrix row, neutral fixture, ledger line. Small units compound.

**Q136. What's the largest unit that should ship together?**
One concern's shape change: owner + capture + migration + fixture + surfaces.
Bigger bundles hide which piece broke.

**Q137. How do we know the plan worked?**
Zero silent-loss incidents, boring reviews, stable budgets, old saves loading —
and no one able to remember the last save bug.

**Q138. What is the one-number dashboard?**
Silent-loss incidents: zero. Everything else is supporting evidence.

**Q139. What if a migration is too complex to test cheaply?**
Then it is too complex; split it into steps with individual fixtures. Complexity
in migrations is risk in players' hands.

**Q140. Are migrations allowed to use gameplay code?**
No. Pure transforms over the record. Gameplay rules evolve; the ladder must not
depend on the current balance.

**Q141. How do we handle a save made in a debug scenario?**
Debug shapes are excluded from the release ladder; a note in `written_by` and a
refusal in release builds if the shape is invalid.

**Q142. What is the "boring review" metric?**
Review duration and return rate. Fast, rarely-returned reviews mean the gates
work upstream.

**Q143. What's the relationship to the determinism guard?**
This plan owns capture; the determinism discipline owns replay reproducibility.
The paired test is the seam where both are proven together.

**Q144. Should the digest include the section list itself?**
Yes — ordered keys are part of the canonical input; a missing section must change
the digest, not hide.

**Q145. What about empty sections?**
Allowed, canonical, and cheap; an empty section is a fact ("nothing happened
yet"), not an error.

**Q146. How do we sunset a whole domain?**
Mark its sections deprecated; keep loading them per the ladder; stop consuming;
after two releases, drop with note. Players never lose records mid-transition.

**Q147. What is the plan's stance on refactors of the save spine itself?**
Allowed only as versioned events with fixtures: the spine is state. Refactor
bravely, version carefully.

**Q148. What remains genuinely hard?**
Cross-section semantic invariants: pairs whose truth depends on both records.
The plan orders and tests them, but they deserve the most review attention.

**Q149. What is the one habit to protect above all?**
Writing the owner line before the field. Everything else follows.

**Q150. The final sentence of the plan?**
The world is still there, exactly where the player left it — and we can prove it.

*End of Part XLIII. Continues in Part XLIV (the last word).*
---

# W4-01 · PART XLIV — THE LAST WORD

## XLIV.1 The plan in one paragraph

Give every fact a single owner; give every owner a versioned, bounded section;
give every section a round-trip test and a neutral fixture; give every shape
change a dense, pure migration with a fixture; make the digest canonical; make
one orchestrator write through temp/verify/rotate/replace; make restore read and
never act; make the profile store a history and never an input; and run the
calendar forever. That is the whole plan.

## XLIV.2 What was deliberately not claimed

```text
- not a serializer rewrite; the current spine is extended, not replaced
- not a gameplay audit; balance and content belong to their waves
- not a networking feature; cloud/sync is out of scope
- not an anti-cheat program; the player's save is the player's
- not a day-one perfection promise; the ladder grows with the game
```

## XLIV.3 The three laws that survived every thread

```text
1. Ownership: one voice per fact, always.
2. Order: the meaning of a fact includes when it is applied.
3. Honesty: what cannot be preserved is stated, never hidden.
```

## XLIV.4 The proof obligation

Every claim in this plan is either (a) a rule that a test enforces, (b) a
procedure that the calendar runs, or (c) a statement of scope. There is no
fourth category — no aspirations, no "should be fine."

## XLIV.5 The closing words

```text
Saves are memory. Memory is trust. Trust is the whole game.
Keep the promise mechanically, or the game is only as good as its last crash.
```

*End of Part XLIV. Continues in Part XLV (final declaration).*
---

---

# W4-01 · APPENDIX D — EVIDENCE TEMPLATES

## D.1 The round-trip evidence file

```yaml
run: RT-<section>
date: ____  head: ____
section: ____________  version: __
fixture: populated | neutral | legacy
fields_checked: __
equalities: __ / __
asymmetries:
  - field: ____  reason: ____  test: ____
result: pass | fail
artifacts: [test-output.txt]
```

## D.2 The ladder evidence file

```yaml
run: ML-<date>
fixtures: __  steps: __
per_step:
  - from: _ to: _  fixture: ____  invariants: __  result: pass
full_ladder: pass | fail
idempotency_cycle: pass | fail
artifacts: [ladder-output.txt]
```

## D.3 The replay evidence file

```yaml
run: PR-<date>
seed: ____  days: __
run_a_digest: ____
run_b_digest: ____
culture_switch_digest: ____
order_shuffle_digest: ____
equal: true | false
```

## D.4 The recovery evidence file

```yaml
run: CK-<date>
cases:
  - case: truncation   expect: typed+backup   actual: ____
  - case: tamper       expect: typed+backup   actual: ____
  - case: future       expect: refused        actual: ____
  - case: empty        expect: refused        actual: ____
  - case: missing      expect: neutral        actual: ____
result: pass | fail
```

## D.5 The leakage evidence file

```yaml
run: LK-<date>
switches: 20
services_checked: __
fresh_identities: __ / __
value_samples_ok: __ / __
leaks: []
result: pass | fail
```

## D.6 The size evidence file

```yaml
run: SZ-<date>
day30:  { total_kb: __, max_section: ____ }
day90:  { total_kb: __, max_section: ____ }
day200: { total_kb: __, max_section: ____ }
capture_ms_day200: __
violations: []
trend_note: ____
```

## D.7 The closure pack index

```text
P0_LEDGER.md
P0_PREMISES.md
RT-*.yaml  ML-*.yaml  PR-*.yaml  CK-*.yaml  LK-*.yaml  SZ-*.yaml
SC-surface-kits.txt
compat-statement.md
findings-crosswalk.md (Appendix A exported)
```

*End of Appendix D. Continues in Appendix E (reader's map).*

---

# W4-01 · APPENDIX E — READER'S MAP

## E.1 If you have five minutes

Read: §0 (how to read), §3.1–3.2 (the first two decision points), §XL.2 (the
reviewer's ten lines), §XL.5 (three sentences).

## E.2 If you are the builder

Read in order: §1 (premises), Part II (ledger + matrix), Part VII (verification),
Part XXXII (rollout), Appendix B (P0 form).

## E.3 If you are the reviewer

Read: Part XIV (packet), Part XXXI (case files), Part XLI/XL (final control),
Appendix A (crosswalk).

## E.4 If you are support

Read: Part IV.1 (taxonomy), Appendix C (cards), §XXXV.3 (calendar), Part XI.4
(corruption drill card).

## E.5 If you are the foreman

Read: Annex U, §6 (non-goals), Part XXXIII (risks), §XXXV.2 (acceptance table).

## E.6 If you are a future contributor

Read: Part XIX (curriculum), Part XXXVI (glossary), §XXIV (expansion rules),
Part XLII (year one).

## E.7 The map of the map

```text
Part I     the plan (paths, points, selection, annex)
II–V       deep designs (10 points)
VI         playbooks
VII        verification
VIII–XVII  worked threads (A–J, findings SA-01..SA-25)
XVIII–XIX  answer key, curriculum
XX–XXII    scenarios, Q&A, threads K–P (SA-26..SA-37)
XXIII–XXVIII tables, expansion notes, governance, scenarios 2, threads Q–V
           (SA-38..SA-49), Q&A 3
XXIX–XXXVI corpus, wire appendix, cases, rollout, risks, contract, closure,
           glossary
XXXVII–XL  interface map, scripts, narrative, final answer key, final control
XLI–XLV    final control, year one, Q&A 4, last word, declaration
App A–E    crosswalk, P0 form, cards, evidence templates, this map
```

*End of Appendix E. The plan closes in Part XLV below.*

---

# W4-01 · APPENDIX F — EXTENDED DRILLS AND THE HYGIENE READING LIST

## F.1 The advanced drills (quarterly rotation)

**Drill 6 — The hostile fixture.** Craft one save that is valid JSON but violates
an invariant (reservation targeting a resolved vigil; roster row with a missing
survivor). Verify the load refuses it with the invariant named, or repairs per
the declared rule — never misloads.

**Drill 7 — The copy-folder soup.** Take a slot folder, duplicate it, edit one
copy, rename both, point the game at it. Verify: slots list what exists; damaged
ones refuse; backups are per-slot; nothing crashes.

**Drill 8 — The language hop.** Play, save, switch language, load, save, switch
back. Verify: digest stability; no text in the wire; surfaces re-render; no
migration fires.

**Drill 9 — The year jump.** Load a fixture one ladder step behind while the
running game has content two releases newer. Verify: transforms apply; new
content defaults; the compatibility note's claims hold literally.

**Drill 10 — The clean desk simulation.** Given a module, produce its ledger row
blind. Compare to reality. Every discrepancy is a documentation finding.

## F.2 The reading list

```text
1. the ledger (always current)
2. the save contract (§XXXIV)
3. the failure taxonomy and copy register
4. the fixture bank README
5. the calendar and its owner
6. the three sentences (§XL.5)
```

## F.3 The one-hour onboarding

```text
0:00–0:10  read §0, §XL.5, Appendix C
0:10–0:25  read the ledger and pick one section
0:25–0:40  open the owner: find capture, restore, tests
0:40–0:50  run the round-trip test; read the output
0:50–1:00  write one review question for the section and ask it
```

## F.4 The maturity self-assessment

```text
[ ] I can name the owner of any state I touch
[ ] I know where the fixtures live and how to add one
[ ] I can recite the failure classes
[ ] I know what the digest includes and excludes
[ ] I know the calendar's owner by name
[ ] I have run the focused checks myself
Score 6/6: ready to review others' save changes.
```

## F.5 The cautionary wall

```text
"Temporary" second writer        → still there three years later
"Nobody reads this field"        → a binding read it by name
"It's just a cache"              → it drifted and a player saw it
"We'll test migrations later"    → a release shipped a gap
"The profile read is harmless"   → a new run started different
Every wall entry is a real save bug's beginning. The plan is the wall.
```

*End of Appendix F. Continues in Part XLVI (final end).*

# W4-01 · PART XLV — FINAL DECLARATION AND END OF DOCUMENT

## XLV.1 The final declaration

**W4-01 is complete.** Parts I–XLV, Appendices A–C. Proposal only; no execution
without Annex U (Part I §U.2). Binding within this document: the save contract
(§XXXIV), the ten invariants (I1–I10), the stop-the-line list (§XV.7), the
permanent rules (§XVIII.4), the closure checklist (§XL.3), the calendar
(§XXXV.3), and the laws of §XLIV.3.

## XLV.2 The artifact index

| Artifact | Location |
|---|---|
| section ledger | P0 output; `P0_LEDGER.md` |
| round-trip matrix | matrix output per section |
| fixture bank | `tests/fixtures/saves/` |
| ladder tests | `SaveMigrationLadderTests` (final names at P0) |
| replay tests | `SaveReplayDeterminismTests`, `SaveCultureInvarianceTests` |
| recovery kit | `SaveCorruptionRecoveryTests` |
| leakage kit | `SaveSlotLeakageTests` |
| size report | generated at day 30/90/200 |
| compatibility statement | release notes, per release |
| findings crosswalk | Appendix A (49 rows) |

## XLV.3 The handoff to W4-02

```text
W4-02 inherits: the union-ledger requirement, the session resume grammar, the
co-location law (markers with consumers), and the five-line requirement for
every new world system. Its crossings and dives are the first true test of the
resume discipline.
```

## XLV.4 End of document

```text
The promise is simple and mechanical:
tomorrow, the world is still there — exactly where the player left it.

*Document control: W4-01 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-01.*
```
---

---

# W4-01 · APPENDIX G — SEED LEDGER ROWS

> Draft rows for the known `*Save` family, to be verified and corrected at P0.
> They exist so the ledger starts from evidence-shaped work, not a blank page.

```yaml
- key: holdfast.campaign
  owner: HoldfastSession
  version: 7
  captured: [day, phase, flags, statics_rng_seed, statics_rng_cursor]
  restored: [day, phase, flags, statics_rng_seed, statics_rng_cursor]
  migrations: [5->6 flags widening, 6->7 statics split]
  consumers: [orchestrator, lifecycle, journal, roster]
  budget: 80KB
  tests: [HoldfastSaveRoundTripTests]
  status: live
- key: expansion.hub
  owner: ExpansionHubSave
  version: 3
  captured: [unlocks, construction, machine_states]
  restored: [unlocks, construction, machine_states]
  migrations: [2->3 machine condition]
  consumers: [hub host]
  budget: 40KB
  tests: [ExpansionHubRoundTripTests]
  status: live
- key: expansion.quests
  owner: ExpansionQuestSave
  version: 2
  captured: [active, completed, flags]
  restored: [active, completed, flags]
  consumers: [quest host]
  budget: 30KB
  tests: [ExpansionQuestRoundTripTests]
  status: live
- key: dose.ledger
  owner: DoseLedgerSave
  version: 2
  captured: [rows, per_survivor_totals, last_applied_day]
  restored: [rows, per_survivor_totals, last_applied_day]
  note: last_applied_day co-located with dose (SA-17)
  consumers: [medical host, dosimeter UI]
  budget: 20KB
  tests: [DoseLedgerRoundTripTests]
  status: live
- key: medical.pipeline
  owner: MedicalPipelineSave
  version: 1
  captured: [queue, reservations, schedules]
  restored: [queue, reservations, schedules]
  consumers: [triage host, ward UI]
  budget: 25KB
  tests: [MedicalPipelineRoundTripTests]
  status: live
- key: medical.ward
  owner: MedicalWardSave
  version: 1
  captured: [beds, occupants, records]
  restored: [beds, occupants, records]
  consumers: [ward host]
  budget: 15KB
  tests: [MedicalWardRoundTripTests]
  status: live
- key: disease.strain
  owner: PathogenStrainSave
  version: 1
  captured: [strains, immunity, quarantine]
  restored: [strains, immunity, quarantine]
  consumers: [outbreak host]
  budget: 12KB
  tests: [PathogenStrainRoundTripTests]
  status: live
- key: branches.independent
  owner: IndependentBranchSave
  version: 2
  captured: [influence, states, decisions]
  restored: [influence, states, decisions]
  consumers: [branch coordinator]
  budget: 20KB
  tests: [IndependentBranchRoundTripTests]
  status: live
- key: branches.military
  owner: MilitaryBranchSave
  version: 2
  captured: [influence, states, decisions]
  restored: [influence, states, decisions]
  consumers: [branch coordinator]
  budget: 20KB
  tests: [MilitaryBranchRoundTripTests]
  status: live
- key: branches.rebel
  owner: RebelBranchSave
  version: 2
  captured: [influence, states, decisions]
  restored: [influence, states, decisions]
  consumers: [branch coordinator]
  budget: 20KB
  tests: [RebelBranchRoundTripTests]
  status: live
- key: politics.prpf
  owner: PrpfSave
  version: 2
  captured: [standing, weights, history_summary]
  restored: [standing, weights, history_summary]
  consumers: [standing system]
  budget: 18KB
  tests: [PrpfRoundTripTests]
  status: live
- key: politics.choices
  owner: WeightOfChoicesSave
  version: 1
  captured: [records, keys]
  restored: [records, keys]
  consumers: [narrative host]
  budget: 15KB
  tests: [WeightOfChoicesRoundTripTests]
  status: live
- key: profile.crossrun
  owner: CrossRunProfileStore
  version: 2
  captured: [runs_summaries, settings_extras]
  restored: [runs_summaries, settings_extras]
  note: gameplay never reads this (boundary)
  consumers: [profile panel, deletion path]
  budget: 8KB
  tests: [CrossRunProfileTests, CrossRunBoundaryTests]
  status: boundary
```

## G.1 How to use these rows

```text
1. do not trust names or versions — P0 verifies each
2. fill the migration column from real history, not from this draft
3. set budgets from measurement, replacing the placeholder numbers
4. add any section this list missed; the gate will find them anyway
```

## G.2 The seed matrix for the largest section

| Field | Capture | Restore | Rule | Test |
|---|---|---|---|---|
| day | `Day` | `Day` | int | RT-01 |
| phase | `Phase` | `Phase` | enum name | RT-02 |
| flags | map | map | set equality | RT-03 |
| rng.seed | `Seed` | `Seed` | int | RT-04 |
| rng.cursor | `Cursor` | `Cursor` | int | RT-05 |
| statics.* | services | rebuilt | not durable | RT-06 |

Note RT-06: the statics split (v6→v7) is the canonical example of a migration
that drops non-durable data and requires the lifecycle to rebuild services.

*End of Appendix G. The plan's true end follows in Part XLVI.*

---

# W4-01 · APPENDIX H — HANDOFF CARDS AND SIGN-OFF

## H.1 Handoff card to W4-02 (world/travel)

```text
INHERITS: five-line requirement, union ledger, co-location law
OWES: session resume grammar for crossings and dives
RISK: mid-session save points; declare quiesce or persist the step
TEST DEBT: session resume fixtures per class
```

## H.2 Handoff card to W4-03 (infrastructure)

```text
INHERITS: budgets, bounds, aggregate-per-day discipline
OWES: machinery condition records; hardening persistence
RISK: histories that grow per tick; condition computed twice
TEST DEBT: condition round-trip; cascade state save/load
```

## H.3 Handoff card to W4-04 (ecology)

```text
INHERITS: derived-field policy (yields recomputed), bounds for populations
OWES: soil/plot state; migration cursors; blight records
RISK: per-tick ecology history; duplicated population counters
TEST DEBT: grow-cycle round-trip; season aggregates
```

## H.4 Handoff card to W4-05 (society)

```text
INHERITS: bounded records (treaties, verdicts), census facts
OWES: standing values; branch influence; legitimacy inputs
RISK: information queues growing unboundedly; duplicate counting
TEST DEBT: treaty round-trip; census consistency across load
```

## H.5 Handoff card to W4-06 (medicine)

```text
INHERITS: co-location (dose marker), reservation keys, vigil facts
OWES: affliction lists; pipeline reservations; records rings
RISK: cross-section invariants (vigil × pipeline); dose double-apply
TEST DEBT: paired invariant tests; reservation uniqueness
```

## H.6 The sign-off

```text
W4-01 hands off one ledger, ten invariants, five cards, and one calendar.
The next plan's first task is to read them — and to add its rows.
```

*End of Appendix H. True end follows in Part XLVI.*

---

# W4-01 · APPENDIX I — THE LAST MEASUREMENTS

```text
W4-01 at a glance
  parts:        I–XLVI
  appendices:   A–I
  findings:     49 (SA-01..SA-49, crosswalk Appendix A)
  invariants:   10 (I1–I10, §XXXIV.2)
  gates:        ledger, matrix, ladder, replay, recovery, leakage, boundary,
                budgets, surfaces
  calendar:     weekly, per-release, quarterly, yearly
  rollout:      7 weeks (Part XXXII)
  handoffs:     5 cards (Appendix H)
  laws:         ownership, order, honesty (§XLIV.3)
```

The plan is complete. It requires Annex U before execution and it keeps the
calendar after closure. It exists to keep one sentence true:

> Tomorrow, the world is still there — exactly where the player left it.

*End of Appendix I. True end follows in Part XLVI.*

# W4-01 · PART XLVI — TRUE END OF DOCUMENT

## XLVI.1 The final recorded state

```text
Document:   W4-01 SAVE, STATE & MIGRATION INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XLVI · Appendices A–F
Findings:   SA-01 .. SA-49 (crosswalk in Appendix A)
Signatures: Annex U (Part I) — required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a
```

## XLVI.2 The three sentences, repeated once more

```text
Name the owner, name the section, name the test.
Restore reads; events act; saves write once.
The world resumes exactly, or we say why not.
```

## XLVI.3 The final line

*The world is still there — exactly where the player left it. That is the whole
plan, and that is the promise it keeps.*

*Document control: W4-01 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-01.*
---

*Reader's note: this plan is one of six in Wave 4. It was written to be read
and executed independently; its laws, however, are inherited by W4-02 through
W4-06, and its ledger is the first page of the union ledger. If you remember
one thing, remember the three sentences: name the owner, name the section,
name the test — restore reads, events act, saves write once — the world
resumes exactly, or we say why not.*

*Document control: W4-01 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
true end of W4-01.*

*Keep the ledger. Run the calendar. Prove the promise. — W4-01, closed.*

*The promise is kept mechanically, or it is not kept at all. — end.*
