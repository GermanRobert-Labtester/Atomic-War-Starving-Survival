# Plans 118–121 — Advanced industrial/reconnaissance closeout

Date: 2026-09-10. Status: Core/catalog/validation tranche complete; host
production projections are intentionally deferred where the repository has no
matching live owner.

| Plan | Core | Data | Focused tests | Selftest | Determinism/save proof |
|---|---|---|---:|---|---|
| 118 | PASS | PASS | 7 | PASS 7/7 | active-batch capture/restore; combined midpoint replay |
| 119 | PASS | PASS | 6 | PASS 6/6 | observation capture/restore; combined seeded confidence |
| 120 | PASS | PASS | 5 | PASS 5/5 | active-cure capture/restore; combined midpoint replay |
| 121 | PASS | PASS | 5 | PASS 5/5 | active-transect capture/restore; idempotent lead |

## Combined evidence

`--advanced-industrial-recon-selftest` passed 8/8: 60 daily snapshots,
catalog loading, production/composite completion, field observations, bounded
trajectory, same-seed byte equality, different-seed divergence and day-55
save/reload equality. It writes:

- `artifacts/advanced-industrial-recon-60d.json`
- `artifacts/advanced-industrial-recon-60d.md`

The four catalogs are schema-versioned, loader-registered, consumer-mapped,
and covered by `--data-integrity-selftest` (303 catalogs, 0 findings) and
`--content-utilization-selftest` (587 catalogs, 245 gameplay-consumed,
0 orphaned; one pre-existing greenhouse warning remains).

## Scope boundary

The engines use atomic inventory ports, seeded RNG, typed state and explicit
projection seams. No Godot types enter Core; UV/GPR do not mutate power/map/
excavation truth; composites do not apply a global vehicle bonus; lubricants do
not apply a global wear reduction. Dedicated host panels, live day-owner
registration and per-system save-store wiring are follow-up work because those
production owners were absent from the reconnaissance map. This is not a claim
that the entire flagship roadmap is UI-complete.

## Validation commands

```text
dotnet build Ashfall.csproj --no-restore --verbosity:minimal
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore --filter 'FullyQualifiedName~FischerTropschSynthesisEngineTests|FullyQualifiedName~UvCoronaDetectionEngineTests|FullyQualifiedName~CarbonCompositeEngineTests|FullyQualifiedName~GroundPenetratingRadarEngineTests' --verbosity:minimal
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --synthetic-lubricant-selftest
godot --headless --path . -- --uv-corona-selftest
godot --headless --path . -- --carbon-composite-selftest
godot --headless --path . -- --gpr-cartography-selftest
godot --headless --path . -- --advanced-industrial-recon-selftest
```

Final verification evidence (2026-09-10):

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore --verbosity:minimal`: **PASS 10,847/10,847**.
- `python3 scripts/ci/run-gates.py --tier fast`: **PASS 47/47** in 169.23s.
- Targeted `real_campaign_journey` gate, including its required host build: **PASS 2/2** in 13.28s.
- `--data-integrity-selftest`: **PASS 303/303**, 0 findings.
- `--content-utilization-selftest`: **PASS**, 587 catalogs, 245 gameplay-consumed, 0 orphaned; one pre-existing warning remains in the deep-chain report.
- Standalone selftests: **PASS** — 118 `7/7`, 119 `6/6`, 120 `5/5`, 121 `5/5`.

The 60-day artifact is a deterministic Core/catalog contract proof for this
repository state. It does not claim production UI or dedicated save-store
coverage for systems that have no existing live host owner; those remain
explicitly deferred in the scope boundary above.
