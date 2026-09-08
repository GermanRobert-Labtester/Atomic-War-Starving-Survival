# Plan 103 — Foundry Treaty Consequences Closeout

## Summary

Expanded `foundry_treaty_consequences.json` from 6 to exactly 15 policy rows.
The original six rows are preserved; nine new rows cover five Plan 102
accords.

## Runtime contract

Repository inspection confirmed that the roadmap's generic
`mechanical_effect` field does not exist. The live schema is:

- `treaty_id`;
- `faction_id`;
- `outcome` (`met`, `missed`, `violated`);
- `standing_delta`;
- `reason`;
- `market_modifiers[]` (`good_id`, `demand_delta`, `reason`).

Lookup is `(treaty_id, outcome)`. The affected faction is a verified treaty
signatory; current Foundry policy rows use `faction_silent_foundry` because
the live ledger and host stance mirror are Foundry-owned.

## Nine additions

| Treaty | Outcomes | Standing |
|---|---|---:|
| `treaty_saltworks_access` | `met`, `violated` | +3 / -10 |
| `treaty_coal_window` | `met`, `missed` | +3 / -5 |
| `treaty_membrane_repair` | `met`, `violated` | +4 / -12 |
| `treaty_crisis_mutual_aid` | `met`, `violated` | +4 / -14 |
| `treaty_the_incident_book` | `met` | +2 |

The plan's “breached” wording is represented by the runtime value
`violated`. No unsupported outcome alias was authored.

## Coverage

- Policies before: 6.
- Policies added: 9.
- Policies after: 15.
- Foundry accords: 10.
- Treaties with two policies: 7.
- Treaties with one policy: 1.
- Treaties with no policy: 2 (`treaty_apprentice_exchange`,
  `treaty_the_cluster_charter`).

The two uncovered agreements have no typed outcome trigger in the current
Core; adding inert rows would misrepresent runtime coverage. The eight
pre-existing regional accord records remain unchanged and are not silently
given Foundry consequences.

## Mechanical and persistence behavior

Standing is clamped and mirrored through the existing stance engine. Market
modifiers route through `MarketSystem`; no inventory, access, production,
contamination, or war subsystem is duplicated. Application remains one-shot
per `(treaty_id, assessment day)` and the existing save envelope persists the
applied record without reapplying it after reload.

## Verification

- JSON parse and key audit: 15 policies, 9 additions, 15 unique keys.
- Plan 103 policy suite plus existing Foundry consequence tests: 41/41.
- `--silent-foundry-selftest`: 28/28.
- `--data-integrity-selftest`: 298/298, 0 errors.
- `--content-utilization-selftest`: CI gate PASS; 581 catalogs, 0 orphaned.
- Full xUnit: 9,852/9,852.
- `dotnet build Ashfall.csproj`: 0 warnings, 0 errors.

The repository-wide fast tier remains blocked by a pre-existing trailing
whitespace finding at
`docs/narrative/PLAN_89_MUSTER_EPILOGUES_EXPANSION_CLOSEOUT.md`; the unrelated
file was intentionally left untouched.

## Deferred

Typed assessment triggers for the five new treaty groups, Apprentice Exchange,
and future access/production/environment/war/epilogue consumers remain
follow-on work. The stable policy IDs and outcome contract are ready for those
systems.
