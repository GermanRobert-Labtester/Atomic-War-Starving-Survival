# Plan 173 — Shelter Radio Production & Audience Response

## Goal

Let the shelter create programs and observe their social effect—presenter effort, resource cost,
audience response, and follow-on opportunities—without duplicating the shared radio schedule,
network hardware, faction corpus, or propaganda model.

## Scope boundary

- Plan 24 owns station identities, frequencies, and the unified broadcast schedule.
- Plan 73 owns passive faction-broadcast content.
- Plan 157 owns range, interception, encryption, and jamming.
- Plan 168 owns propaganda message truth/effect resolution.
- This plan owns player-authored program templates and their execution against an existing Plan 24
  broadcast slot. It must not define a second `BroadcastSchedule`, station/frequency catalog, or
  communications network.

## Task 1 — Program production contract

1. Add a Core `RadioProgramProductionSystem` with program templates, required equipment,
   presenter assignment, preparation cost, and a reference to an existing schedule slot.
2. Define outcome facts: delivered/cancelled, quality band, audience response, and reusable
   follow-up hooks. Keep frequency, range, and airtime ownership external.
3. Reuse Plan 168 for propaganda effects and Plan 157 for whether the broadcast reaches an
   audience; do not recalculate either locally.

## Task 2 — Presenter and audience loop

1. Let qualified survivors prepare news, education, entertainment, emergency, and storytelling
   programs from data templates.
2. Model audience response as a deterministic consequence of the received broadcast result,
   presenter capability, and program quality—not as an independent radio network.
3. Surface legible outcomes: morale, reputation signals, faction reaction hooks, and opportunities
   such as a listener contact or a requested follow-up broadcast.
4. Save only program production history and unresolved follow-up hooks in the campaign section.

## Task 3 — Integration and validation

1. Consume a Plan 24 schedule slot, Plan 157 delivery result, and Plan 168 propaganda result via
   explicit interfaces.
2. Add tests for cancelled broadcasts, missing equipment, presenter eligibility, deterministic
   audience response, save/load, and no duplicate schedule ownership.
3. Add `radio_programs.json` only for player-program templates; it contains no frequency or
   schedule authority.

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

## Definition of Done

- One schedule and one communications authority remain in Plans 24 and 157.
- Shelter programs have presenter, cost, delivery, and audience consequences.
- Propaganda and faction-radio results are consumed, never reimplemented.
