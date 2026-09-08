# Foundry Treaty Production Handoff

Foundry production remains owned by `SilentFoundrySystem` and
`foundry_production.json`. Plan 103 does not add a production modifier token,
queue mutation, heat rule, or synthetic shutdown.

The Coal Window rows influence only the existing market demand surface:

- `met`: coal `-0.25`, fuel `-0.15`;
- `missed`: coal `+0.30`, fuel `+0.15`.

Those adjustments can make future inputs cheaper or scarcer when the host
applies a live policy, but they do not alter a current heat or retroactively
change a quota. The new policy rows are currently trigger-deferred because the
Core assessor has no typed Coal Window or Membrane Repair cycle.
