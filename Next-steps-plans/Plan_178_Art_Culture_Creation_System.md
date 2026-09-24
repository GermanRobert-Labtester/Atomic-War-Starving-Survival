# Plan 178 — Art & Culture Creation — Canonical Vault Composition

> Integration plan revision: 2026-09-24. Source of truth: current repository source and data, then
AGENTS.md, then
[docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](../docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md).
This document is a planning artifact. It does not claim paths or authorize a competing implementation
package.

## 1. Objective and bounded outcome

Reconcile the existing artwork record with the signed vault/hobby/decor authority map, then implement a
single real creation command whose output reaches an existing consumer and survives its approved save
lineage.

**Current state:** CORE ONLY: CultureCreationSystem and six art forms exist with capture/restore, but no src
reference to the system was found. The signed 178/190 authority map proposes creation through existing
vault/hobby/decor interfaces and forbids a second archive save; its statement that ArtCreationSystem is
absent does not erase the differently named CultureCreationSystem. Numeric Main.Plans178_181 is child
development, not art.

**Non-goals:** No second art engine, duplicate vault recording, standalone art save section without an
ownership decision, imaginary inventory item, decoration bonus applied twice, or inert art-form catalog
route.

**First deliverable:** a signed authority/custody disposition and exact path claim. A read-only projection
may proceed after its owners agree; any mutable integration needs a separate approved vertical slice and
save/reload evidence.

## 2. Authority and evidence status

The current or candidate domain authority is `CultureCreationSystem currently holds artwork records;
CulturalArchiveVaultSystem, SurvivorDowntimeSystem and ShelterDecorSystem own adjacent outcomes`. Source and
adjacent paths inspected for this revision (some are candidate consumers rather than active bindings):
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`, `src/Main.FlagshipInstitutions.cs`,
`src/Main.Plans178_181.cs`, `docs/culture/PLAN_178_190_CREATION_LORE_AUTHORITY_MAP.md`,
`src/UI/ArchiveDeskPanel.cs`, and `Assets/StreamingAssets/Data/art_forms.json`. The focused test starting
point is `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs`. Persistence boundary under audit:
`UNDECIDED for artwork: no culture_creation registry section observed; cultural_archives owns vault state`.
Paths are evidence pointers, not advance claims.
The master expansion authority v2.0 supplies the planning discipline and continuity map: C9 interiority, C1
decor, C16 progression and C17 panels; the master points to the signed 178/190 lore authority map and
demands that item identity, archive content, and creation records remain distinct. Its Part II requires live
premise checks, bounded subject scope, explicit evidence labels, and a duplication firewall. The relevant
deep maps and lane matrices guide coverage; they do not override newer code. The master compilation itself
warns against padding and stale repository assumptions. This plan therefore records concrete contracts and
treats older task lists as intent pending current verification.

## 3. Current contract and collision firewall

- **Evidence or explicit premise:** CultureCreationSystem.LoadCatalog reads art_forms, but CreateArtwork
  accepts an ArtMedium directly and does not check form_id, required_skill, required_materials, or
  creation_time_days.
- **Evidence or explicit premise:** CreateArtwork uses optional ISeededRng, produces art_{sequence}
  identity, quality and masterwork, updates cultural identity, and emits events.
- **Evidence or explicit premise:** DisplayArtwork mutates only the local ArtworkRecord.DisplayLocation;
  GetShelterCultureMoraleBonus computes a capped projection, but no host consumer was found.
- **Evidence or explicit premise:** No CultureCreationSystem reference in src and no culture_creation save
  section in SaveSectionRegistry were found at this audit.
- **Evidence or explicit premise:** The signed docs/culture/PLAN_178_190_CREATION_LORE_AUTHORITY_MAP.md owns
  the 178/190 boundary: creation may append to vault recordings/chronicles or hobby output; second archive
  save is OUT.
- **Evidence or explicit premise:** ArchiveDeskPanel is an adjacent surface, not evidence of an art command;
  an implementation must select a reachable current player route.

### Culture creation authority dossier

The signed 178/190 map is a required premise. It says a player creation command should append to the
`CulturalArchiveVaultSystem` or a hobby output and should not create a second archive section. The Core
`CultureCreationSystem` exists under a different name than the map's absent `ArtCreationSystem`; its artwork
records contain creator, title, medium, theme, day, quality, display location, and identity. This is a
genuine ownership collision. The foreman must choose whether the record is a bounded creation
transaction/projection within vault custody, or whether there is an explicitly separate artwork authority
with a justified persistence section. Until then, no host save wiring should be coded.

The `art_forms.json` fields imply skill, materials, duration, morale, value, and placement rules, but the
current `CreateArtwork` method does not consume a `form_id` or any of those prerequisites. A concrete
command should choose one form row, read canonical skill and inventory, reserve/commit costs atomically,
honor duration through an existing work/job owner if approved, and produce one record. If that design
exceeds the signed authority, limit the first slice to an instantaneous, no-cost vault entry and state the
reduced scope honestly.

Artwork display is currently only a string field. A decor owner must verify a location exists and is
available; then a local display pointer may be updated. The morale projection is at most 15 in Core, but
another morale system must own the actual survivor/shelter effect. Prove whether the bonus is a live query,
reversible mark, or one-time event; do not add both a daily bonus and a creation event for the same work.

The first reviewable outcome is one art form → one canonical creator command → one vault chronicle/recording
→ reload → truthful surface. Masterworks and cultural identity follow only once a stable artifact identity
and custody are established.

Before code edits, inspect existing equivalent producers and consumers with `rg`, read their public methods
and save snapshots, and write a one-page premise note. If another live owner already mutates the target
concern, use it. If a required write API does not exist, return the proposed consumer hook to the foreman as
an authority decision. Do not create a side ledger to bypass that missing API.

## 4. Ownership matrix

| Concern | Current or proposed owner | Implementation rule |
|---|---|---|
| authored definitions | `Assets/StreamingAssets/Data/art_forms.json` | Validate schema, IDs, references and finite ranges before use. |
| source fact | `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` | Keep domain invariants in Core; source emits a typed fact. |
| host composition | `src/Main.FlagshipInstitutions.cs` | Wire only current owners and lifetime-safe event subscriptions. |
| campaign save | `UNDECIDED for artwork: no culture_creation registry section observed; cultural_archives owns vault state` | Use the existing section or seek an explicit section decision if none exists. |
| player view | `src/UI/ArchiveDeskPanel.cs` | Render current state, blockers and effects without gameplay mutation. |
| cross-system effect | `destination subsystem named in each phase` | Call the owner method once; do not copy its mutable state. |

## 5. Data and identity contract

Canonical IDs come from the existing catalogs and runtime facts. Case handling, trimming and comparison must
match the owning system. Proposed new IDs need a schema and reference validator before a producer can emit
them. Read the current JSON fields before extending a row; do not use the old plan’s desired row count as an
acceptance measure. Reject duplicates or conflicting definitions with per-row diagnostics. Treat display
names and prose as presentation, never identity. Preserve ordinal ordering for stable output and save
checksum inputs.
A change to catalog shape requires an old-catalog compatibility rule, one example valid row, one invalid
row, and a reader inventory. New prose must state a real observed consequence; it cannot promise trade,
safety, quest or reward behavior until a consumer reads the corresponding Core fact. Avoid real-world names
or copied narrative.

## 6. C# implementation sketch

```csharp
// Decision-gated composition sketch, not an approved new save path.
ArtworkRecord artwork = creation.CreateArtwork(creatorId, title, medium, theme, day, skill, rng);
// Commit the approved archive/vault output through CulturalArchiveVaultSystem.
// A display command asks ShelterDecorSystem to accept a location; then
// CultureCreationSystem.DisplayArtwork records the accepted placement.
// If the vault rejects, do not leave an orphaned artwork or spent materials.
```

The snippet shows ownership and call direction, not a paste-ready replacement. The implementing agent must
confirm names and signatures against the live tree immediately before coding. Keep `Assets/Ashfall.Core/`
free of Godot and Unity references. If an adapter needs a callback, bind it in the current host lifetime,
unsubscribe on disposal, and keep domain decisions in Core.

## 7. State, save and migration

Inspect `docs/culture/PLAN_178_190_CREATION_LORE_AUTHORITY_MAP.md` and the registry for the named concern;
absence of a section is a decision gate, not permission to invent one. Write down exact DTO version,
constructor baseline, capture point, restore point, and dirty flag. For a stateless bridge, persist only the
source and destination authorities; do not create a bridge section. If a new mutable field is genuinely
required, version the existing owner DTO and prove migration from the preceding schema. Never equate a panel
cache with campaign state.
The restore sequence is: catalog validated; source state restored; destination state restored; adapters
bound; pending effects reconciled once; UI bound; day processing resumes. A new campaign starts with an
explicit empty/default state. A legacy save without the field takes that same baseline without inventing
past events. Unknown future schema versions must fail or degrade according to the existing save contract,
with a visible diagnostic. Corrupt single records must not silently reset an entire unrelated section.

## 8. Event, day and failure semantics

One semantic fact should carry stable source ID, campaign day, owning entity ID, content ID if any, and
enough context for the destination owner to decide. The host may route the fact but may not calculate a
competing gameplay rule. Define when the fact is emitted, when a command is committed, and what is durable
before the next callback. A read-only query must not create journal entries or rewards on every panel
refresh.
Use the canonical day owner only for behavior that genuinely changes with time. Preserve the current day
phase ordering and forked seeded RNG contract. No wall-clock seed, hash-order iteration, or System.Random
belongs in deterministic Core. Replay after load must not repeat a completed effect; an effect that was not
committed must remain recoverable. When a destination owner rejects a command, keep the source fact and show
a reason rather than writing a partial substitute effect.

## 9. Player commands and UI

Inspect the current route to `src/UI/ArchiveDeskPanel.cs` and its bind/open/close methods. Expose only
commands backed by an existing Core method or a specifically planned method in the named owner. The panel
should show source state, currently legal action, expected cost, blocker, committed outcome, and uncertainty
where the game cannot know more. Refresh on authoritative state change and after restore; unbind
subscriptions on close/disposal. Preserve keyboard/controller back and readable contrast.

## 10. Dependency-ordered implementation phases

### Phase 0 — Premise and claim

**Action:** Read current source/data; diff the old plan against delivered work; claim exact paths before an
edit. **Gate:** A signed package lists files, owner, non-goals, acceptance and focused command.
**Dependency:** Phase 0 requires a claim; later phases require the preceding gate. Shared composition roots
remain integrator-owned; if a phase needs them, package the exact seam for the integrator.

### Phase 1 — Core/consumer contract

**Action:** Identify one producer fact and one destination owner. Add the smallest typed query or command if
the owner agrees. **Gate:** A domain unit exercise proves input, refusal, and stable identity.
**Dependency:** Phase 0 requires a claim; later phases require the preceding gate. Shared composition roots
remain integrator-owned; if a phase needs them, package the exact seam for the integrator.

### Phase 2 — Data validation

**Action:** Extend only the existing catalog as required by Phase 1; validate references and ranges.
**Gate:** One valid and one invalid row produce clear, reproducible results. **Dependency:** Phase 0
requires a claim; later phases require the preceding gate. Shared composition roots remain integrator-owned;
if a phase needs them, package the exact seam for the integrator.

### Phase 3 — Save and restore

**Action:** Bind existing section; introduce versioned state only for genuinely new durable facts. **Gate:**
Round-trip, old-save baseline, and replay-after-restore agree. **Dependency:** Phase 0 requires a claim;
later phases require the preceding gate. Shared composition roots remain integrator-owned; if a phase needs
them, package the exact seam for the integrator.

### Phase 4 — Host composition

**Action:** Bind in the current session at correct setup order, route result to destination owner once.
**Gate:** A focused host-level check observes the real destination effect. **Dependency:** Phase 0 requires
a claim; later phases require the preceding gate. Shared composition roots remain integrator-owned; if a
phase needs them, package the exact seam for the integrator.

### Phase 5 — UI and narrative

**Action:** Bind existing panel or claimed route to a read model; show action and blocker truthfully.
**Gate:** A headless or bounded runtime check opens/refreshes/closes without a stale claim. **Dependency:**
Phase 0 requires a claim; later phases require the preceding gate. Shared composition roots remain
integrator-owned; if a phase needs them, package the exact seam for the integrator.

### Phase 6 — Acceptance and handoff

**Action:** Run only directly affected focused test files and necessary runtime probe. **Gate:** Handoff
includes exact commands/results, limitations, and shared paths untouched. **Dependency:** Phase 0 requires a
claim; later phases require the preceding gate. Shared composition roots remain integrator-owned; if a phase
needs them, package the exact seam for the integrator.

## 11. File impact map

| Path | Action after claim | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` | READ / bounded MODIFY | domain contract | medium |
| `Assets/StreamingAssets/Data/art_forms.json` | READ / bounded MODIFY | authored rules | medium |
| `src/Main.FlagshipInstitutions.cs` | READ / bounded MODIFY | session wiring | high |
| `src/Main.Plans178_181.cs` | READ / integrator MODIFY | composition root | high |
| `docs/culture/PLAN_178_190_CREATION_LORE_AUTHORITY_MAP.md` | READ / bounded MODIFY | persistence | high |
| `src/UI/ArchiveDeskPanel.cs` | READ / bounded MODIFY | truthful surface | medium |
| `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` | READ / focused MODIFY | contract verification | low |

No paths in this table are claimed by this document. At implementation time compare them with
`WORKTREE_OWNERSHIP.md` and assign the exact package. Do not edit a currently claimed path.

## 12. Focused acceptance and rollback

Use `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` for the first
contract check, then only the directly affected save and host targets identified by Phase 0. Run a Godot
headless probe only if the claimed change affects the runtime host path. A compile result cannot establish
live route reachability. Acceptance requires: (1) one real input; (2) one canonical consumer effect; (3) one
durable restore; (4) repeat delivery without duplicate effect; (5) visible correct state and blocker; (6) no
new authority.
Keep changes in reviewable phase commits. If a consumer hook fails, revert that phase without replacing the
canonical owner; preserve old saves and catalog compatibility. If a migrated section cannot load, stop
before adding UI or content and give the integrator the exact schema and fixture. Record any deferred edge
with a current evidence pointer and promotion condition rather than claiming it complete.

## 13. Detailed integration acceptance cards

The following cards are planning checks, not a request to create one test method per card. Each card has one
feature-specific expected outcome; crosscutting checks below it define the evidence needed to accept that
outcome. Select the smallest independent cases that prove the changed contract, save/load, determinism,
lifecycle, and cross-system behavior. “Legacy intent” cards are explicitly conditional: first prove their
premise and owner, then either promote as a separate bounded package or mark them retired. This prevents the
2026-09-01 plan text from resurrecting already delivered or contradictory architecture.

### 01. painting art form [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns painting art form. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** form_painting must resolve its catalog row; any skill/material/time rule is
a proposed command requirement, not a behavior of current CreateArtwork.

**Fresh campaign path.** For painting art form, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For painting art form, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For painting art form, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For painting art form, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For painting art form, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For painting art form, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For painting art form, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For painting art form, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 02. sculpture art form [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns sculpture art form. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** form_sculpture may create one artwork only after an approved material
consumption and archive destination are named.

**Fresh campaign path.** For sculpture art form, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For sculpture art form, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For sculpture art form, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For sculpture art form, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For sculpture art form, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For sculpture art form, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For sculpture art form, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For sculpture art form, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 03. music composition art form [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns music composition art form. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** form_music_composition needs a durable artifact or performance identity; do
not count an unplayed composition as a radio broadcast.

**Fresh campaign path.** For music composition art form, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For music composition art form, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For music composition art form, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For music composition art form, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For music composition art form, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For music composition art form, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For music composition art form, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For music composition art form, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 04. poetry art form [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns poetry art form. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** form_poetry may append a vault chronicle only through
CulturalArchiveVaultSystem and must not spawn a parallel journal ledger.

**Fresh campaign path.** For poetry art form, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For poetry art form, Deliver the same source fact twice, including a retry after
a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed applied
marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For poetry art form, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For poetry art form, Replace one referenced catalog ID with an unresolved value in an
isolated fixture. The data validator should report the authored error; runtime must refuse that one effect
or show a specific unavailable reason. It must not invent a fallback faction, location, survivor, item,
quest, or animal. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For poetry art form, Exercise the same fact at day 1, at the relevant threshold, and
after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For poetry art form, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For poetry art form, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For poetry art form, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 05. storytelling art form [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns storytelling art form. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** form_storytelling must reconcile with OralLorePerformanceSystem first-heard
and replay rules before an art command is enabled.

**Fresh campaign path.** For storytelling art form, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For storytelling art form, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For storytelling art form, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For storytelling art form, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For storytelling art form, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For storytelling art form, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For storytelling art form, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For storytelling art form, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 06. craftwork art form [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns craftwork art form. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** form_craftwork must not invent an inventory item if its form row lacks a
canonical item mapping.

**Fresh campaign path.** For craftwork art form, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For craftwork art form, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For craftwork art form, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For craftwork art form, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For craftwork art form, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For craftwork art form, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For craftwork art form, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For craftwork art form, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 07. form skill prerequisite [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns form skill prerequisite. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** Compare required_skill with the current skill authority; CreateArtwork does
not currently enforce this threshold.

**Fresh campaign path.** For form skill prerequisite, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For form skill prerequisite, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For form skill prerequisite, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For form skill prerequisite, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For form skill prerequisite, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For form skill prerequisite, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For form skill prerequisite, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For form skill prerequisite, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 08. form material prerequisite [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns form material prerequisite. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** Preflight all required_materials against Inventory and decide rollback if
archive append rejects after consumption.

**Fresh campaign path.** For form material prerequisite, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For form material prerequisite, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For form material prerequisite, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For form material prerequisite, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For form material prerequisite, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For form material prerequisite, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For form material prerequisite, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For form material prerequisite, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 09. creation duration [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns creation duration. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** creation_time_days cannot be claimed as elapsed time until an existing
job/day owner carries pending work and restores it.

**Fresh campaign path.** For creation duration, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For creation duration, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For creation duration, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For creation duration, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For creation duration, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For creation duration, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For creation duration, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For creation duration, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 10. artist identity and day [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns artist identity and day. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** CreatorSurvivorId and CreationDay must come from the canonical roster and
campaign clock; reject unknown or departed creators per signed rule.

**Fresh campaign path.** For artist identity and day, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For artist identity and day, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For artist identity and day, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For artist identity and day, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For artist identity and day, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For artist identity and day, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For artist identity and day, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For artist identity and day, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 11. artwork stable ID [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns artwork stable ID. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** ArtworkId art_{NextSequence} must remain stable and unique across
capture/restore and repeated command submission.

**Fresh campaign path.** For artwork stable ID, Start a new seeded campaign with the smallest legal source
fact. Invoke the current owner through its normal host entry point, then inspect the projected outcome at
the intended consumer. State the exact identifier and day in the focused fixture. The card passes only when
the named destination read model or canonical command has the expected result; otherwise record a bounded
dependency. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For artwork stable ID, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For artwork stable ID, Capture the relevant existing section after the source fact and
before the player views it. Restore into a fresh host/session and compare domain state, consumer state, and
visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery path
has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Invalid reference.** For artwork stable ID, Replace one referenced catalog ID with an unresolved value in
an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For artwork stable ID, Exercise the same fact at day 1, at the relevant threshold,
and after a later day tick. Run setup in both legal host orders if the dependency may initialize lazily. The
final state must follow canonical campaign day and deterministic ordering, with no callback registered
twice. Acceptance evidence should name the actual method, stable ID, source day, destination state field or
read model, and exact refusal code if the operation is unavailable.

**Player surface.** For artwork stable ID, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For artwork stable ID, Trace the fact from producer to destination authority.
Show the precise command/query on the destination owner, the one save section it already uses, and a visible
downstream difference. If no such command exists, record the need as a named integration dependency rather
than writing state into the producer. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Determinism and bounds.** For artwork stable ID, Replay identical seed, content and ordered facts in two
fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that defines
the allowed range. If this card needs randomness, fork from the existing campaign stream and make the event
key stable. Acceptance evidence should name the actual method, stable ID, source day, destination state
field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 12. seeded quality and masterwork [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns seeded quality and masterwork. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** For a fixed campaign RNG fork, QualityScore and IsMasterwork match after
replay; quality is not recomputed on restore.

**Fresh campaign path.** For seeded quality and masterwork, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For seeded quality and masterwork, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For seeded quality and masterwork, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For seeded quality and masterwork, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For seeded quality and masterwork, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For seeded quality and masterwork, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For seeded quality and masterwork, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For seeded quality and masterwork, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 13. title validation and display text [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns title validation and display text. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** Title fallback and trimming must be tested with blank and long input;
user-facing text must not be used as artifact identity.

**Fresh campaign path.** For title validation and display text, Start a new seeded campaign with the
smallest legal source fact. Invoke the current owner through its normal host entry point, then inspect the
projected outcome at the intended consumer. State the exact identifier and day in the focused fixture. The
card passes only when the named destination read model or canonical command has the expected result;
otherwise record a bounded dependency. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For title validation and display text, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For title validation and display text, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For title validation and display text, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For title validation and display text, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For title validation and display text, Open the existing panel before and after the
effect and after reloading. The panel reads a Core or host read model, displays the actual blocker and
outcome, and preserves close/back/focus behavior. A label that announces a benefit absent from the owning
system fails this card. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For title validation and display text, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For title validation and display text, Replay identical seed, content and
ordered facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at
the owner that defines the allowed range. If this card needs randomness, fork from the existing campaign
stream and make the event key stable. Acceptance evidence should name the actual method, stable ID, source
day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 14. archive chronicle output [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns archive chronicle output. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** One accepted creation appends one chronicle/recording to the existing
vault, with an exact artifact/source identity for dedup.

**Fresh campaign path.** For archive chronicle output, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For archive chronicle output, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For archive chronicle output, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For archive chronicle output, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For archive chronicle output, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For archive chronicle output, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For archive chronicle output, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For archive chronicle output, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 15. oral performance overlap [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns oral performance overlap. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** Oral-lore first-heard IDs stay with OralLorePerformanceSystem; a story
artwork can reference them without copying mutable state.

**Fresh campaign path.** For oral performance overlap, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For oral performance overlap, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For oral performance overlap, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For oral performance overlap, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For oral performance overlap, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For oral performance overlap, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For oral performance overlap, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For oral performance overlap, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 16. hobby output overlap [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns hobby output overlap. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** SurvivorDowntimeSystem output_item_id remains the hobby reward owner; an
art command cannot mint it independently.

**Fresh campaign path.** For hobby output overlap, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For hobby output overlap, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For hobby output overlap, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For hobby output overlap, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For hobby output overlap, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For hobby output overlap, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For hobby output overlap, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For hobby output overlap, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 17. decoration placement legality [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns decoration placement legality. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** DisplayArtwork should run only after ShelterDecorSystem accepts a real
location and must clear/move the old placement consistently.

**Fresh campaign path.** For decoration placement legality, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For decoration placement legality, Deliver the same source fact twice, including
a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly
designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For decoration placement legality, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For decoration placement legality, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For decoration placement legality, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For decoration placement legality, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For decoration placement legality, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For decoration placement legality, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 18. shelter morale projection [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns shelter morale projection. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** GetShelterCultureMoraleBonus is capped at 15; prove a canonical morale
consumer reads it once and reverses it on undisplay.

**Fresh campaign path.** For shelter morale projection, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For shelter morale projection, Deliver the same source fact twice, including a
retry after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For shelter morale projection, Capture the relevant existing section after the source
fact and before the player views it. Restore into a fresh host/session and compare domain state, consumer
state, and visible text. Repeat with the save taken immediately before the consumer effect to prove the
recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For shelter morale projection, Replace one referenced catalog ID with an unresolved
value in an isolated fixture. The data validator should report the authored error; runtime must refuse that
one effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For shelter morale projection, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For shelter morale projection, Open the existing panel before and after the effect and
after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For shelter morale projection, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For shelter morale projection, Replay identical seed, content and ordered facts
in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 19. artwork save custody [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns artwork save custody. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** No culture_creation save section is registered; obtain a signed custody
decision before adding host serialization.

**Fresh campaign path.** For artwork save custody, Start a new seeded campaign with the smallest legal
source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For artwork save custody, Deliver the same source fact twice, including a retry
after a UI refresh or host rebind. The owner must use its existing stable ID or an explicitly designed
applied marker so the consumer mutation and player notification occur once. Do not solve duplication by a
process-local boolean that disappears on reload. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Save and restore.** For artwork save custody, Capture the relevant existing section after the source fact
and before the player views it. Restore into a fresh host/session and compare domain state, consumer state,
and visible text. Repeat with the save taken immediately before the consumer effect to prove the recovery
path has an explicit pending/completed contract. Acceptance evidence should name the actual method, stable
ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Invalid reference.** For artwork save custody, Replace one referenced catalog ID with an unresolved value
in an isolated fixture. The data validator should report the authored error; runtime must refuse that one
effect or show a specific unavailable reason. It must not invent a fallback faction, location, survivor,
item, quest, or animal. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For artwork save custody, Exercise the same fact at day 1, at the relevant
threshold, and after a later day tick. Run setup in both legal host orders if the dependency may initialize
lazily. The final state must follow canonical campaign day and deterministic ordering, with no callback
registered twice. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For artwork save custody, Open the existing panel before and after the effect and after
reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and preserves
close/back/focus behavior. A label that announces a benefit absent from the owning system fails this card.
Acceptance evidence should name the actual method, stable ID, source day, destination state field or read
model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For artwork save custody, Trace the fact from producer to destination
authority. Show the precise command/query on the destination owner, the one save section it already uses,
and a visible downstream difference. If no such command exists, record the need as a named integration
dependency rather than writing state into the producer. Acceptance evidence should name the actual method,
stable ID, source day, destination state field or read model, and exact refusal code if the operation is
unavailable.

**Determinism and bounds.** For artwork save custody, Replay identical seed, content and ordered facts in
two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner that
defines the allowed range. If this card needs randomness, fork from the existing campaign stream and make
the event key stable. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

### 20. old-save empty artwork baseline [CURRENT ACCEPTANCE SCENARIO — VERIFY EFFECT]

**Source and ownership:** This card concerns old-save empty artwork baseline. Start from
`Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and `Assets/StreamingAssets/Data/art_forms.json` and
trace any effect through `src/Main.FlagshipInstitutions.cs` to its current destination owner. Persistence
must follow this boundary: `UNDECIDED for artwork: no culture_creation registry section observed;
cultural_archives owns vault state`. Verify the exact source and destination sections; a new section
requires an ownership decision. Keep the source fact distinct from the consumer mutation. Catalog evidence:
No catalog row directly matches this integration case; verify the current producer and destination API
before implementation.

**Feature-specific acceptance:** Old saves lacking artwork state must show an empty, truthful creation view
without inventing historical works from archive tomes.

**Fresh campaign path.** For old-save empty artwork baseline, Start a new seeded campaign with the smallest
legal source fact. Invoke the current owner through its normal host entry point, then inspect the projected
outcome at the intended consumer. State the exact identifier and day in the focused fixture. The card passes
only when the named destination read model or canonical command has the expected result; otherwise record a
bounded dependency. Acceptance evidence should name the actual method, stable ID, source day, destination
state field or read model, and exact refusal code if the operation is unavailable.

**Repeat and idempotency.** For old-save empty artwork baseline, Deliver the same source fact twice,
including a retry after a UI refresh or host rebind. The owner must use its existing stable ID or an
explicitly designed applied marker so the consumer mutation and player notification occur once. Do not solve
duplication by a process-local boolean that disappears on reload. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Save and restore.** For old-save empty artwork baseline, Capture the relevant existing section after the
source fact and before the player views it. Restore into a fresh host/session and compare domain state,
consumer state, and visible text. Repeat with the save taken immediately before the consumer effect to prove
the recovery path has an explicit pending/completed contract. Acceptance evidence should name the actual
method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Invalid reference.** For old-save empty artwork baseline, Replace one referenced catalog ID with an
unresolved value in an isolated fixture. The data validator should report the authored error; runtime must
refuse that one effect or show a specific unavailable reason. It must not invent a fallback faction,
location, survivor, item, quest, or animal. Acceptance evidence should name the actual method, stable ID,
source day, destination state field or read model, and exact refusal code if the operation is unavailable.

**Day/order boundary.** For old-save empty artwork baseline, Exercise the same fact at day 1, at the
relevant threshold, and after a later day tick. Run setup in both legal host orders if the dependency may
initialize lazily. The final state must follow canonical campaign day and deterministic ordering, with no
callback registered twice. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Player surface.** For old-save empty artwork baseline, Open the existing panel before and after the effect
and after reloading. The panel reads a Core or host read model, displays the actual blocker and outcome, and
preserves close/back/focus behavior. A label that announces a benefit absent from the owning system fails
this card. Acceptance evidence should name the actual method, stable ID, source day, destination state field
or read model, and exact refusal code if the operation is unavailable.

**Cross-owner consequence.** For old-save empty artwork baseline, Trace the fact from producer to
destination authority. Show the precise command/query on the destination owner, the one save section it
already uses, and a visible downstream difference. If no such command exists, record the need as a named
integration dependency rather than writing state into the producer. Acceptance evidence should name the
actual method, stable ID, source day, destination state field or read model, and exact refusal code if the
operation is unavailable.

**Determinism and bounds.** For old-save empty artwork baseline, Replay identical seed, content and ordered
facts in two fresh runs; compare the existing section state and consumer read model. Clamp only at the owner
that defines the allowed range. If this card needs randomness, fork from the existing campaign stream and
make the event key stable. Acceptance evidence should name the actual method, stable ID, source day,
destination state field or read model, and exact refusal code if the operation is unavailable.

**Focused selection:** Reuse `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` if it covers this
contract. Add a case only for a new public, save, lifecycle, mutation, deterministic, or cross-owner
behavior. Record this card as passed, deferred with an owner, or stale with source evidence; do not leave an
ambiguous “implemented” label.

## 14. Legacy plan reconciliation register

The original 2026-09-01 task list is preserved as intent here in condensed form. Each entry is a premise
question, never an instruction to create a duplicate class or save section. Current code and the live
ownership ledger decide whether it becomes a claim.
- **L01:** Create `ArtCreationSystem.cs` in `Assets/Ashfall.Core/Culture/`. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L02:** Define `ArtForm` DTO: `formId`, `formName`
  (painting/sculpture/music/poetry/storytelling/theater), `requiredSkill` (art skill level),
  `requiredMaterials` (list of items), `creationTime` (ticks), `moraleBoost` (base value), `culturalValue`
  (0-100), `description`. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L03:** Define `Artwork` DTO: `artworkId`, `artworkName`, `artForm`, `creatorId` (survivor),
  `creationDay`, `quality` (0-100), `theme` (hope/loss/resistance/nature/love/death), `location` (where
  displayed/performed), `culturalValue` (accumulated), `description`. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L04:** Define `CulturalEvent` DTO: `eventId`, `eventType`
  (exhibition/concert/reading/performance/workshop), `artworks` (list of artwork IDs), `participants` (list
  of survivor IDs), `audience` (list), `moraleBoost`, `culturalImpact`. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L05:** Define `ArtCreationState` DTO: list of artworks created, list of cultural events held, shelter
  cultural identity tags, total cultural value, art skill per survivor. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L06:** Implement `CaptureState/RestoreState` with schema versioning. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L07:** Define art forms:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L08:** Define art themes:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L09:** Define creation mechanics:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L10:** Define display/performance:. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L11:** Define cultural events:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L12:** Define cultural identity:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L13:** Add deterministic seeding: art creation uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L14:** Wire into `GameBootstrap`: `SetupArtCreation`, `TickArtCreation`, `SaveArtCreation`. Verify
  against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must
  be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L15:** Create `ArtTemplateCatalogLoader` for art form definitions. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L16:** Implement art creation:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L17:** Implement artwork display:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L18:** Implement performances:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L19:** Implement cultural events:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L20:** Implement cultural identity:. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L21:** Implement art trading:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L22:** Implement art education:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs`
  and the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L23:** Create art events:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L24:** Add art quest hooks:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and
  the destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L25:** Implement art UI:. Verify against `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the
  destination owner; disposition must be DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L26:** Add art journal: automatic log of art events. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L27:** Implement art tutorial: first creation explains system. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L28:** Add art tooltips: hover over artwork shows details. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L29:** Create 15 art templates in data file. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L30:** Wire into `ShelterDecorSystem`: visual art displayed as decor. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L31:** Connect to `NeedsSystem`: art provides morale boost. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L32:** Integrate with `SkillProgressionSystem`: art skill progression. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L33:** Connect to `FactionBranchCoordinator`: art trading affects standing. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L34:** Wire into `CraftingSystem`: art materials from crafting. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L35:** Connect to `SeasonalEventSystem` (Plan 170): cultural events at festivals. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L36:** Implement old-save compatibility: existing saves get empty art state. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L37:** Add deterministic seeding: creation uses `ISeededRng`. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L38:** Create exploit prevention: art requires materials and time. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L39:** Add tests: art creation, display, performances, events, trading, save round-trip. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L40:** Verify catalog integrity: all art form/template IDs resolve. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L41:** Test edge cases: no art (no culture), extensive art (rich culture). Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L42:** Verify headless behavior: art processes correctly without UI. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L43:** Add data-integrity-selftest: art templates validate against item catalogs. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.
- **L44:** Create `--art-creation-selftest` verb for CI validation. Verify against
  `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` and the destination owner; disposition must be
  DELIVERED, RESIDUAL, BLOCKED, or RETIRED before editing.

## 15. Handoff contract

**MUST PRESERVE:** one Core/domain owner per concern, current JSON authority, existing save lineage,
deterministic replay, and claimed-path discipline.

**MUST ADD:** only the scoped producer-to-consumer path and its observable result after the Phase 0 premise
audit.

**MUST NOT ADD:** a parallel registry, default fake facts, unowned UI mutation, duplicate resource, morale,
dose, archive, achievement, profile, or faction state, Unity dependency, or a full-suite gate for this
document edit.

**VERIFY WITH:** `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs` plus
targeted owner save/host checks selected at claim time under `TEST_POLICY.md`.

**FIRST SAFE IMPLEMENTATION STEP:** publish a current premise note and exact path claim, then implement the
first accepted vertical slice. This plan does not itself alter production code.
