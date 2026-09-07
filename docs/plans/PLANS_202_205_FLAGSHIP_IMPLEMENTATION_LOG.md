# Plans 202–205 Flagship Integration Log — Shelter Resilience & Long-Range Logistics

**Flagship:** waste-plastic fuel recovery (202), perimeter defense extension (203),
subterranean mushroom cultivation extension (204), cargo airdrop recovery (205).
**Plan class:** Major flagship full-integration roadmap (renumbered from the
proposed 102–105 — those numbers were consumed by the foundry/narrative/trade
streams; see `docs/plans/PLANS_202_205_RECONNAISSANCE.md`).

## Wave record

| Wave | Plan | Commit(s) | Closeout |
|---|---|---|---|
| A — Reconnaissance | all | (in `d951903c`) | `docs/plans/PLANS_202_205_RECONNAISSANCE.md` |
| B — Fungi extension | 204 | `d951903c`, `b255c752` | `docs/shelter/PLAN_204_MUSHROOM_CULTIVATION_CLOSEOUT.md` |
| C — Plastic pyrolysis | 202 | `1e65f6b8` | `docs/shelter/PLAN_202_PLASTIC_PYROLYSIS_CLOSEOUT.md` |
| D — Perimeter extension | 203 | `17dd8297` | `docs/combat/PLAN_203_PERIMETER_DEFENSE_CLOSEOUT.md` |
| E — Cargo airdrop | 205 | `00802b18` | `docs/expeditions/PLAN_205_CARGO_AIRDROP_CLOSEOUT.md` |
| F — Unified verification | all | this commit | this log |

## Wave F — unified deterministic verification

`Ashfall.Core.Tests/Integration/Plans202To205CampaignIntegrationTests.cs`:
all four engines on forked campaign streams over a shared inventory, driven by
identical day-indexed inputs (storm every 5th day, weekly brownout, day-table
wind).

1. **§12 same-seed replay** — 20 continuous days, two runs, byte-identical
   four-engine state snapshots.
2. **§13 split convergence** — 30-day campaign split at day 15 (serialize all
   four engine states, restore into fresh instances, continue) converges
   exactly with the continuous run.
3. **§13 multi-point splits** — splits at days 7/14/21/28 also converge.
4. **Connected-loop assertions** — real fuel and carbon in inventory, fungi
   food in inventory, airdrop cargo transferred, no negative inventory,
   weather wear demonstrably bit the perimeter, intrusion log bounded.

**Enabler:** engines' tick-time probability rolls were migrated to the house
**day-derived fresh-seed pattern** (WeatherSystem precedent — each roll reseeds
from the campaign day, no engine-internal RNG sequence crosses a save/load):
pyrolysis hazard + incident rolls, perimeter false alarms + barrel jams,
fungi bloom + substrate-prep rolls. Existing single-engine tests re-verified
(69/69 across the four suites).

## Cross-plan integration results (§14 scenario, verified)

- Storms wear perimeter emplacements while brownouts stall retort batches —
  one weather truth, two consumers, no duplicate hazards.
- Retort fuel fractions claim into the shared inventory the expeditions draw from.
- Airdrop fuel/engineering crates transfer through the expedition's own
  capacity enforcement — no shelter-injection bypass.
- Fungi harvests feed the kitchen item chain; contamination disposal is a
  real transaction (burn costs fuel drawn from the same stock the retort uses).

## Final gate status at closeout

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | 0 errors |
| `dotnet test Ashfall.Core.Tests` | 9169/9170 — sole failure is the concurrent pharma stream's unstaged `docs/INDEX.md` reorganization (deleted `Next-steps-plans` targets), pre-dating and unrelated to this flagship |
| `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| `--data-integrity-selftest` | PASS — 0 findings across 298 catalogs |
| `--bridge-selftest` | PASS |
| `--save-store-checksum-selftest` Gate A | PASS |

## Concurrent-stream notes (per integration precedent)

- The pharma stream's untracked `tablet_manufacturing_catalog.json` carries an
  unresolved `room_pharma_lab` id — owned by that stream.
- The doc-reorganization stream's deleted `Next-steps-plans/Plan_14/15_*`
  files break the DocLink gate — owned by that stream.
- The railway-interlock and aquifer-piezometer streams landed in-flight
  mid-wave; the aquifer tests were quarantined via the csproj mechanism until
  that stream stabilizes them.

## Architecture rule, restated

Core owns simulation; existing authorities own their state; cross-system
effects travel through typed APIs/events; Godot presents; saves preserve;
tests prove. All four engines follow the day-derived fresh-seed determinism
pattern — a 30-day replay produces identical state for identical seed, inputs,
and commands, even across save/load splits.
