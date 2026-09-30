# Plan 2 — Domain ownership and day-advance orchestration

STATUS: APPROVED BY USER

Editorial revision: 2026-09-28. Existing scope and approval retained.

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no contract item and no acceptance
> criterion. **Acceptance** below remains binding.

---

## 0. Framing — The Map of Who Owns What

> *"Missing local callers alone do not prove dead code."*

That single line is the ethical centre of this plan, and it is worth sitting with. A repository is
full of things nobody calls: reflection targets, Godot signal handlers, save hooks that fire only
on a load that has not happened yet, paths reachable from a console the player will never open.
The absence of a call site is an *observation*. It is not a verdict.

Plan 2 is about making ownership traceable — mapping `Main.Plans*.cs` bundles to their real
domains, splitting what was mixed and merging what was scattered — while preserving member bodies,
method names and Godot UID associations **exactly**. A rename that changes behaviour is not a
rename; it is an edit wearing a rename's clothes, and the plan requires it be recorded as one.

**Tone & register.** Cartographic, patient, precise. The vocabulary is the estate: *bundle, domain,
fragment, destination, seam, coordinator*. Prose should read like a surveyor describing land that
is already inhabited.

**The interesting seam.** `CampaignDayHostSession` *wraps* `CampaignDayCoordinator` — registration,
ordering, rollback, RNG and persistence all stay where they were — and publishes the committed day
through `IEventBus` **only after success**. That ordering is a promise: nobody hears about a day
that did not happen. And `A successful compile alone does not prove lifecycle integration` is the
card's final and most important sentence.

**The second layer.** This plan is about *publication*: a day is committed, then and only then
announced, and the ordering is the entire ethic. "Nobody hears about a day that did not happen" is
a courtesy to every listener in the system — and a promise the rollback path has to keep at three
in the morning when the write failed halfway.

**Texture (second prose pass — commentary only).**

- Ordering is manners: persist, succeed, and only then speak. The `IEventBus` publication is the system saying *it is done* rather than *it is being attempted*.
- "A successful compile alone does not prove lifecycle integration." The sentence is the plan's spine and the reason its acceptance is written the way it is.
- Three backups and a crash-kill probe: trust in a save system is not a feeling, it is a verb performed on the filesystem.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, contract item, acceptance criterion
or register row changes. The register below is unchanged.)*

- "Missing local callers alone do not prove dead code" is a rule about *evidence*, and it governs
  the whole file: reflection targets, signal handlers and load hooks are things that have not
  happened yet, not things that never will.
- A rename that changes behaviour is an edit wearing a rename's clothes, and the plan requires it
  be recorded as one. Nouns are cheap; behaviour is the estate.
- The day is published only after success — nobody hears about a day that did not happen. That
  ordering is the smallest promise in the file and the most load-bearing.

> "A surveyor describing land that is already inhabited: the map must change nothing about who
> lives there."

---

## Outcome

Make host responsibilities traceable by domain while preserving gameplay,
deterministic ordering, save ownership and CLI entry points.

## Contract and sequence

1. Map historical `Main.Plans*.cs` bundles to their actual domains. Split mixed
   bundles; merge same-domain fragments. Preserve member bodies, method names,
   and Godot UID associations. Missing local callers alone do not prove dead code.
2. Keep cross-domain lifecycle composition in `Main.SubsystemComposition.cs`.
   Update source probes and references affected by renamed paths.
3. Route day advancement through `CampaignDayHostSession`, wrapping the existing
   `CampaignDayCoordinator`. Registration, ordering, rollback, RNG and persistence
   remain with the coordinator. Publish the committed day through `IEventBus`
   only after success; unsubscribe on disposal and reset during host teardown.
4. Split `HostCli.PanelTests.cs` by command into `HostCli.Command.*.cs` without
   changing dispatch or assertions. Coordinate export exclusions with Plan 1.

## Ownership

The [decomposition map](../../docs/architecture/MAIN_DECOMPOSITION_MAP.md)
records exact historical sources, destinations and current domain dependencies;
it replaces the duplicated filename inventory in this document.

Shared seams: `src/Main.Campaign.cs`, `src/Main.CampaignOwners.cs`,
`src/Main.Holdfast.cs`, `src/Main.Lifecycle.cs`, and
`src/Host/CampaignDayHostSession.cs`. The integrator owns their acceptance.

Reference adjustments also cover `src/Host/HostCli.RailTrackMaintenance.cs` and
the existing tests `Plan211InternalCommunicationHostWiringTests`,
`Plan218MuseumHostWiringTests`, and `Plan24NeedsSourceMigrationTests`.
Current claims remain governed by `WORKTREE_OWNERSHIP.md`.

## Acceptance

- Every historical bundle has a destination; live references resolve.
- Mechanical moves preserve member bodies. Any behavior repair is identified
  separately and receives its own focused verification.
- `MainTriadDriftGateTests`, affected source-path tests and relevant CLI commands
  pass; runtime checks cover day commit, failure and host reset/disposal.
- Both build configurations compile and retain the intended command boundary.

Run affected tests through `bin/run-scoped-tests`; do not repeat the full suite
after renames. A successful compile alone does not prove lifecycle integration.

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's own **Acceptance** and contract text. Not defects — the honest limits of
what a mechanical decomposition can prove. Any later pass that resolves one must say so separately.

| # | Open item | What is actually known | Who may resolve it (later, separately verified) |
|---|---|---|---|
| PH-OM-1 | Is any moved code actually dead? | **"Missing local callers alone do not prove dead code."** A move preserves bodies; it does not judge them. | A reachability case made per member, with its own verification. |
| PH-OM-2 | Did the decomposition change behaviour? | Member bodies, method names and Godot UID associations are preserved. Behaviour repair is *identified separately* if it occurs. | Its own focused package, never folded into a rename. |
| PH-OM-3 | Does the lifecycle actually integrate? | **"A successful compile alone does not prove lifecycle integration."** Runtime checks cover commit, failure and reset/disposal. | A lifecycle probe, not a build. |
| PH-OM-4 | Who owns the shared seams? | `Main.Campaign`, `Main.CampaignOwners`, `Main.Holdfast`, `Main.Lifecycle`, `CampaignDayHostSession` — integrator-accepted, and `WORKTREE_OWNERSHIP.md` changes hourly. | The integrator, at each package start. |
| PH-OM-5 | Why did the bundles end up mixed? | The decomposition map records exact historical sources and destinations. *How* they came to be mixed is not recorded. | Never — texture by omission. |
