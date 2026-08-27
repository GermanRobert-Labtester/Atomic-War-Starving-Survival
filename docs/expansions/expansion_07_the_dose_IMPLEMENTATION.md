# ASHFALL: THE DOSE — Implementation Approach & Player Experience

**Companion to `docs/expansions/expansion_07_the_dose_plan.md`.** This document is the build
blueprint and the player-facing surface. It does not restate the design bible; it says *how to
implement it as-is* and *how the player meets the antagonists and the expansion*.

---

## PART A — HOW TO IMPLEMENT IT (AS SPECIFIED, NO DEVIATION)

### A1. New core systems (4 files, plain C#, zero engine refs)

| File | System | State field | Events | Save/Restore |
|---|---|---|---|---|
| `Assets/Ashfall.Core/DoseLedgerSystem.cs` | `DoseLedgerSystem` | `DoseLedgerSystemState` | `OnDoseCorrected`, `OnBandReached`, `OnLedgerCalibrated` | deep-copy `CaptureState` / `RestoreState` |
| `Assets/Ashfall.Core/SickListSystem.cs` | `SickListSystem` | `SickListSystemState` | `OnDiagnosed`, `OnReleased`, `OnPalliativeAssigned` | same |
| `Assets/Ashfall.Core/CohortSystem.cs` | `CohortSystem` | `CohortSystemState` | `OnChildBooked`, `OnBaselineCorrected` | same |
| `Assets/Ashfall.Core/VoluntaryRegisterSystem.cs` | `VoluntaryRegisterSystem` | `VoluntaryRegisterSystemState` | `OnVolunteered`, `OnVolunteerCompleted` | same |

**Every system follows the house pattern proven by the other expansions:**
- `[Serializable]` state DTOs with public fields (SaveChecksum walks public fields).
- `OnStateChanged` raised on *every* mutation so the host dirty-flags the save.
- `ISeededRng` for the flux-ambiguity roll (deterministic, host-independent).
- Defensive deep copy in `CaptureState`; null-safe `RestoreState` that re-adds defaults.
- No UnityEngine / Godot / JsonUtility. Serialize through `IJsonSerializer`.

**DoseLedgerSystem mechanics (from the bible, exact):**
- Incoming exposure (`float mSv`, `IReadOnlyList<string> livingSurvivorIds`) with an optional
  `bool highEnergyEvent`. If `highEnergyEvent`, roll `fluxFactor = 0.85 + rng.NextFloat()*0.30`.
- Only survivors with an assigned `DosimeterTag` are booked. Unbooked rads are dropped from the
  ledger but were never applied to `Survivor` — the ledger is a *record*, not the physics engine.
- `antiRadBefore` attenuates incoming; `antiRadAfter` attenuates the booked amount.
- Band thresholds (Green/Amber/Red/Black) are consts. On crossing, fire `OnBandReached`.
- Calibration: after `ReadingsPerCalibration=40`, set `calibrationOverdue`; a `dosimeter` item
  refunds `readingsSinceLastCalibration=0` and clears the flag.

**SickListSystem mechanics:**
- `Diagnose(survivorId, band, day)` appends a named band. Re-diagnosis moves the band; it never
  deletes history (the ledger is ink).
- `AssignPalliative(survivorId, plan)` writes a `palliativePlan` and fires `OnPalliativeAssigned`.
- A Black-band survivor is *not* removed — they remain on the roster and can still volunteer.

**CohortSystem mechanics:**
- `BookChild(childId, parentIds, guessBand, birthDay)` — guess is a string "low"/"medium"/"high".
- `CorrectBaseline(childId, trueBand)` — stores the correction, fires `OnBaselineCorrected`,
  and *does not auto-post to the ledger* (the bible says a dosimeter read *may* be booked separately).
- `children` list is never pruned; the first generation cannot rewrite the board.

**VoluntaryRegisterSystem mechanics:**
- `Volunteer(survivorId, task, day)` appends a pending entry.
- `CompleteVolunteer(survivorId, doseIncurred, day)` banks the dose into the ledger (via the host
  wiring the two systems together), moves the sick band if crossed, writes `reasonText`, closes
  the entry.
- The four systems are wired *at the host layer*, the same way LedgerDebtSystem composes
  CrossingArbitrationSystem — never inside the core.

### A2. Save envelope

One standalone `Assets/Ashfall.Core/DoseLedgerSave.cs` with `DoseLedgerSave` +
`DoseLedgerSaveCodec` mirroring the Duty Roster pattern byte-for-byte:
```
saveVersion=1 ; simDay ; doseLedger ; sickList ; cohort ; voluntaryRegister ; Checksum
```
`Capture(simDay, ...4 systems)` → `Encode` (always recompute checksum) → `Decode` (reject
tampered/checksumless/newer) → `Restore(...4 systems)`.

### A3. Host session + store + wiring

- `src/Host/DoseLedgerHostSession.cs` — constructs the 4 systems, subscribes each `OnStateChanged`
  → own `StateChanged`, exposes `CaptureSave`/`RestoreSave`, plus demo helpers
  (`ScribeReading`, `DiagnoseDemo`, `BookDemoChild`, `SignDemoVolunteer`).
- `src/Host/DoseLedgerSaveStore.cs` — `user://dose_ledger_save.json`, `pathOverride`, identical
  thin pattern to the other stores.
- `src/Main.cs` — 4-6 buttons under a "THE DOSE" section, load-on-setup, save-on-quit, diagnostics
  section. Reuses the dirty-flag + coalesced-flush pattern.
- `src/Host/HostCli.cs` — `--dose-ledger-selftest`.

### A4. Data (JSON) — if a catalog is wanted

`Assets/StreamingAssets/Data/dose_registers.json` — one table for each of the four registers of
band/plan/guess vocabulary, so the host never hardcodes display strings. Ids all snake_case,
following the existing catalog schema.

### A5. Verification

- `dotnet test` — 4 new xunit files (roundtrip, band transition, flux determinism, cohort
  correction, volunteer banking, null tolerance).
- `godot --headless -- --dose-ledger-selftest` — four-system demo + save roundtrip + tamper.
- Full selftest battery must stay green. Godot-only; no Unity run.

---

## PART B — HOW THE PLAYER SEES THE ANTAGONISTS

**Lag:** The antagonists of The Dose are not enemies. They are four people who keep books, and
the closest thing the shelter has to a conscience with a pen. The player does not fight them; the
player *outlasts a line of questioning at a table*. They appear in the UI as a chaired scene —
the room the player walks into — not as a faction bar.

**`npc_dr_irina_vel` — the Radiation Registrar (owns the Dose Ledger pen).**
The player first meets her the night the dosimeter is taken apart. She holds a red pencil and will
not let the player read *up* a number to make it easier. When an exposure event posts a reading,
she asks one question: "How much do you want written?" — not playing dumb, just giving the player
the choice that is already theirs. Her dialogue is terse, her border cases are real, and she
remembers every number the player refused to book. She cannot be bribed into an easier total; the
numerous refusals accumulate as a silent tally the player may never see.

**`npc_wyn_omah` — the Sick-room Nurse (owns the Sick List).**
Sister Wyn does not argue triage with the player. She *presents the bed order* and waits. If the
player names a Red survivor, she writes it and does not thank them; if the player hides a name,
she does not accuse them — she asks whether the morphine stays on the tray, and the room records
the answer. Her will-not is mercy that erases another name: she refuses to shuffle the bed order
so one person's comfort costs another's care. Her dialogue is liturgical and exhausted, never
judging, never warm.

**`npc_piet_abar` — the Clockmaker (calibration, not a register).**
Piet is the analogue of the dosimeter dial and the only honest voice about error. He tells the
player that every figure on the ledger has a drift and that the drift is *normal*. He will not lie
about the drift to make a reading land softer. The player meets him at the calibration bench; his
offer is always the same key (reset accuracy) for the same price (time and a scannable object).
He is the antagonist of false precision — the enemy, for the player, is the *certainty* the other
three registers pretend to have.

**`npc_saria_voss` — the Midwife (owns the Cohort).**
Saria is the hardest antagonist because she is *correct* about the uncertainty. When a child is
born she asks the player to choose a guess band — low, honest, or refused — and then, weeks later,
she may bring the child's true number. She will not book a guess as a truth, and she keeps the
children's board in chalk so it can be erased. Her dialogue forces the player to choose between a
kinder story and a truer one, and she never tells them which is which.

**UI presence of the four:** each appears as a named entry in a chaired row (name, one-line
disposition, and their current register's most recent line). No agent faces are necessary — the
*words* are the portrait. Selecting one opens a three-to-five-line card; a diegetic one-button
action (Book / Name / Assign / Sign) commits the player's choice and closes the scene.

---

## PART C — HOW THE PLAYER SEES THE EXPANSION

**Entry:** The Dose unlocks when a `dosimeter` first exists (the `quest_the_dose_the_first_reading`
gate). From that moment there are four tabs of a single "Dose Register" surface — not four menus,
one folder of paperwork.

**The surface (diegetic, cold):**
- **Ledger tab** — rows of survivor names and cumulative mSv, with a small `§` for flux-ambiguous
  readings and a red `overdue` marker when calibration lapses. The player's action is "book a
  reading"; the drone of the dial is the only ambiance.
- **Sick tab** — Green/Amber/Red/Black bands with named rows. Action: "assign care" or "name to
  the Voluntary Register."
- **Cohort tab** — the chalk board: child names, parent names, and a *guess* in pencil. Action:
  "book a baseline" or "leave uncounted."
- **Voluntary tab** — a signature list. Action: "sign an hour" and, later, "mark it done"; the
  dose lands back on the Ledger tab the moment it completes.

**Player agency is the act of writing.** Every button spends scarce ink (a `dosimeter` tag, a
`morphine` unit, a calibration key). Refusing to write is a valid outcome the ledger records as
silence. There is no success meter; the four quest lines close when the player *chooses what is
recorded*, not when they *fix* anything.

**Failure is legible and human.** A survivor the player never booked, never named, never signed
still dies of their dose off-screen — and the bunker remembers that the page for them is blank.
Nothing is hidden from the player; the emptiness is the message.

**Antagonists as the expansion's spine:** the four accountants are the reason the four tabs exist,
and their refusal-to-lie is what makes each tab uncomfortable. The player leaves The Dose not
having "won" — they have *kept a book*, or declined to, and either way the shelter now has a
document that says who was counted.