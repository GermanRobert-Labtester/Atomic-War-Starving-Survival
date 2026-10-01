# ASHFALL — First 30 Minutes Human Smoke Checklist

Use this checklist to verify the game is **graphically seeable and playable**
end-to-end in a normal (non-headless) session. It follows the authored
`OnboardingCatalog.FirstHour` journey so the human path and the automated gates
(`OnboardingWiringGateTests`, `FirstHourPlaythroughSmokeTests`,
`--player-panels-uitest`, `--seven-day-slice-selftest`) describe the same loop.

**Target:** ≤ 30 minutes. **FPS target:** 15 FPS unless the build says otherwise.
**Build:** `bash launch.sh` (editor) or the exported binary under `builds/`.
Attach screenshots to the release ticket; one per numbered step.

## Before you start

1. Delete any existing profile so onboarding starts fresh.
2. Start a **new campaign** and confirm the opening protocol modal appears with
   the Day-1 objective text (not an empty or stale label). Screenshot **S1**.
3. Confirm the HUD status bar shows the current first-hour objective.

## Golden path

| # | Action | Expected (visible evidence) | Stage sigil |
|---|---|---|---|
| 1 | Open **Water Treatment** and start a batch | Batch appears with progress; stores show water | `water.treatment_started` |
| 2 | Open **Power Grid** and toggle a breaker | Grid state visibly changes; no dead panel | `power.breaker_toggled` |
| 3 | Open **Inventory** and consume a food ration | Ration count drops; survivor need responds | `food.ration_consumed` |
| 4 | Open **Duty Roster**, assign one survivor | Assignment confirmed; fitness reason shown | `duty.assigned` |
| 5 | Open **Dose Ledger** | Dose readout renders with real values | `dose.read` |
| 6 | Open **Research**, start one node | Node shows in-progress, not instant | `research.started` |
| 7 | Open **Expeditions**, dispatch a sortie | Sortie leaves; cost is shown honestly | `expedition.dispatched` |

After step 7, the first-hour sequence must report **complete** and the status bar
must switch to the completion copy. Screenshot **S2** (journey complete).

## Save / load durability

8. Save the campaign, quit to the menu, and load it again.
9. Confirm the completed first-hour stages are **not** re-demanded and the
   objective does not regress. Screenshot **S3**.

## Failure / edge behaviour

10. Trigger one refusal (e.g. dispatch with no supplies). Confirm the game shows
    a readable refusal, not a silent no-op.
11. Open **Afflictions** with a tracked chronic condition and confirm FIT /
    REMOVE rows render with the authored material cost and honest
    missing-material state (T18).

## Sign-off

- [ ] All 7 stages reached with visible feedback.
- [ ] No placeholder squares on the seven surfaces above.
- [ ] No unhandled error in the log during the loop.
- [ ] Save/load resumes without re-demanding finished stages.
- [ ] Screenshots S1–S3 attached.

If any step fails, file it against the owning system named in the checklist row
and link `docs/qa/FIRST_HOUR_SMOKE_TEST.md`.
