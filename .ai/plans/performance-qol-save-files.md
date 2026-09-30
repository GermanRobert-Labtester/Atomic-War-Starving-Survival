# Plan 3 — Campaign backup recovery and resumption

STATUS: APPROVED BY USER

Editorial revision: 2026-09-28. Existing scope and approval retained.

> **Editorial polish (prose pass, non-contractual):** the Framing section below is commentary on
> intent and craft only. It changes no scope, no ownership, no contract item and no acceptance
> criterion. **Acceptance** below remains binding — including its final, deliberately narrow claim.

---

## 0. Framing — The Crash Window

> *"Backup rotation protects committed generations. It does not protect progress that has never
> been saved."*

That sentence is the most important thing in this plan and it is buried in item 4 where the
engineers put it. Save systems are sold on their recovery rate and judged on their honesty about
what is unrecoverable. This plan is honest.

Three validated generations. A player confirmation that **warns later progress will be lost** —
not a silent overwrite, not a best guess. Cancel preserves the session exactly as it stands.
Recovery restores through the *normal load path*, which means recovery is not a second way to load
a game; it is the ordinary way, pointed at a different generation.

**Tone & register.** Custodial, calm, scrupulous. The vocabulary is the vault: *generation,
rotation, atomic write, envelope, manifest, checksum, fallback*. Prose should read like someone
explaining a safety deposit arrangement to the person whose valuables are in it.

**The interesting limit.** `A bounded interrupted-write check … does not prove protection from
every filesystem or power-loss failure.` The plan runs a crash-kill probe, verifies the committed
primary and three backups survive, and then declines the general claim. That restraint is the
difference between a save system you can trust and a save system that only says you can.

**The second layer.** The bounded claim is the kindest thing one system can do for another. This
plan runs the crash, verifies the primary and the three backups, and then declines the general
theorem — because a save system that promises everything has told you nothing. What it protects is
not data; it is the player's willingness to believe tomorrow will load.

**Texture (second prose pass — commentary only).**

- "Interrupted write, bounded check": the plan proves what it proves and stops exactly where its evidence stops.
- The golden is captured before the edit. Always. The past is evidence here, never raw material.
- Navigation QoL is politeness encoded: fewer clicks between a player and their own campaign.

*Register below unchanged — these fragments are texture, not new recorded items.*

**The polish layer (third prose pass — commentary only).**

*(Non-contractual: craft commentary only. No scope, ownership, contract item, acceptance criterion
or register row changes. The register below is unchanged.)*

- The crash window is the interval the plan refuses to lie about: rotation protects *committed*
  generations, and progress that was never saved is beyond every system, including this one. The
  warning dialog is that sentence translated into care.
- Recovery goes through the normal load path — it is not a second door, it is the same door pointed
  at a different generation. One way in is one way to be wrong, which is the point.
- Three validated generations, an atomic write and a checksum are furniture; the trust lives in
  the deliberately narrow claim at the end, the one that declines the general protection it cannot
  prove.

> "Trust in a save system is trust in what it admits it cannot save."

---

## Outcome

Recover a damaged primary save from up to three validated backup generations,
with player confirmation, and resume the saved campaign day and valid panel.

## Contract and sequence

1. Keep `SaveSlotService` as the sole rotation/recovery authority. Validate source
   generations before rotation, preserve atomic writes, and propagate failures
   through `SaveLoadHostSession` and the existing save orchestrator.
2. Offer recovery only when a backup validates. Show its campaign day and warn
   that later progress will be lost. Cancel preserves the current session;
   confirmation restores through the normal load path. Report recovery failures.
3. Use manifest v3 for `lastPanelId`; retain the existing `currentDay` authority.
   Preserve v1/v2 checksum compatibility and a safe route fallback when saved
   panel metadata is absent or no longer usable.
4. Reuse the existing Continue entry point and capture/restore lifecycle.
   Record autosave triggers and the remaining crash window: backup rotation
   protects committed generations, not progress that has never been saved.

## Ownership

- Core: `Assets/Ashfall.Core/Save/{SaveSlotService,CampaignSaveEnvelope,SaveSlotTypes}.cs`.
- Host/UI: `src/Host/SaveLoadHostSession.cs`,
  `src/Main.{SaveOrchestrator,GameFlow,UiPanels}.cs`, `src/UI/SaveLoadPanel.cs`.
- Verification: `Ashfall.Core.Tests/SaveSlotServiceTests.cs`,
  `Ashfall.Core.Tests/Save/ActiveSaveSlotPersistenceTests.cs`, and
  `src/Main.UiTests.StartingCohortLifecycle.cs`.

## Acceptance

- Round-trip restores payload, day and panel; legacy manifests still load.
- Corrupt primary and backup cases select a validated candidate or report that
  recovery is unavailable. Failed saves cannot report success.
- The existing lifecycle probe covers navigation, dialog focus, cancel and
  confirmation using isolated temporary saves.
- A bounded interrupted-write check preserves previously committed saves and
  demonstrates explicit recovery. It does not prove protection from every
  filesystem or power-loss failure.

Reuse existing tests through `bin/run-scoped-tests`; serialize runtime checks
with the integrator. Repository cleanup remains in the parent QoL package.

## Open Items & Deliberate Limits (register — not acceptance criteria)

Drawn from this plan's own **Acceptance** text and item 4. Not defects — the honest boundary of
what backup rotation can promise. Any later pass that resolves one must re-run the probe.

| # | Open item | What is actually known | Who may resolve it (later, re-probed) |
|---|---|---|---|
| PS-OM-1 | **The crash window on never-saved progress.** | Explicitly recorded: rotation protects *committed* generations only. Unsaved progress is unprotected and the plan says so. | Only a narrower autosave interval — and even then, never fully. |
| PS-OM-2 | Does this survive every filesystem or power-loss failure? | **"It does not prove protection from every filesystem or power-loss failure."** A bounded interrupted-write check was run, not a general proof. | Hardware-backed fault injection with a stated matrix. |
| PS-OM-3 | What is the third generation for? | Three validated generations are kept. The retention depth is specified, not argued for. | Never — texture by omission. |
| PS-OM-4 | What happens to a panel that no longer exists? | Manifest v3 stores `lastPanelId` with **a safe route fallback** when metadata is absent or unusable. The fallback's target is not described. | The UI owner, if a fallback target is ever documented. |
| PS-OM-5 | Why is recovery *explicit* rather than automatic? | Player confirmation is required and cancel preserves the session. The plan does not justify the choice; it simply makes it. | Never — a rule, not a gap. |
