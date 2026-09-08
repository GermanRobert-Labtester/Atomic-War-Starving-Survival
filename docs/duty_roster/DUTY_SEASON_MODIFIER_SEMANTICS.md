# Duty Season Modifier Semantics

## `encounterWeight` — live consumer, traced chain

```
catalog.GetSeasonForDay(day).encounterWeight          (relative weight, not a probability)
  → DutyRosterHostSession.ActivateSecondWinter()      (src/Host/DutyRosterHostSession.cs:288)
  → ShelterEncounterSystem.SetSecondWinter(multiplier, day)
  → _state.encounterWeightMultiplier = multiplier <= 0f ? 1f : multiplier   (assignment)
  → persisted in ShelterEncounterSystemState (save round-trip via DutyRosterSaveCodec)
  → cleared by ClearSecondWinter() → resets to 1f
```

Properties:
- **Replacement, not accumulation.** Each activation assigns the value; transitions never stack. A multi-season future consumer must call `SetSecondWinter(newWeight, day)` — the old value is overwritten, so "season replaces prior season" is guaranteed by the existing setter.
- **Clamp:** non-positive → 1.0 at the setter. Upper clamp: none in code; catalog validation bounds it to [0.5, 2.5].
- **Sampled:** at activation time (host action), not per tick — no repeated-tick accumulation.
- It is a **relative weight multiplier** on shelter-encounter pressure. UI prose must not claim "N× as many encounters" unless selection math proves a linear frequency relationship.

## `steamTripChanceBoost` — authored data, consumer deferred

- **No live runtime consumer exists** (verified by grep across Core, src, and tests). The only Core "steam trip" mechanic is `BrineWaterSystem`'s membrane-integrity threshold event, which does not read this field.
- The catalog loader parses it; `DutyRosterSeasonCatalogTests` validates its range ([0.0, 0.15]) and the pinned `season_first_siege` value (0.03).
- Base chance, additive-vs-multiplicative composition, and clamps are **unknown because no consumer exists** — they were not invented. Balance targets treat it as a bounded authored signal for a future consumer (most plausibly an external-trip opportunity modifier, per the planning brief).
- **Follow-on requirement:** any future consumer must (a) define composition against its base chance, (b) clamp the final probability to [0,1], (c) apply the seasonal value exactly once per decision, (d) replace (not accumulate) across season transitions.

## Application-exactly-once guarantees

- Encounter weight enters state exactly once per activation (assignment semantics).
- Season selection itself applies no modifiers — it returns the entry; consumers read fields directly.
- No second multiplication site exists anywhere in Core/src for either field (verified by grep).
