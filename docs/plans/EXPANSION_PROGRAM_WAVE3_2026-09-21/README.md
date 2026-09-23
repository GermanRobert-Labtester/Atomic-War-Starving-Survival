# ASHFALL Expansion & Integration Program — Wave 3 (2026-09-21)

Ten more proposed plans (21–30): **five gap-sealing and five expansion**.
Produced from a read-only audit of HEAD `5be1a30a`. **None is a claim.**

## Gap sealing (21–25)

| # | Plan | Gap | Evidence |
|---|---|---|---|
| 21 | [`PLAN-EVENT-WIRING-21.md`](PLAN-EVENT-WIRING-21.md) | Dead events | 478 Core events, **84 with zero subscribers**; prior repairs were incident-driven |
| 22 | [`PLAN-DATA-CONSUMER-22.md`](PLAN-DATA-CONSUMER-22.md) | Field/row consumption | Catalog-level audit only; three field-level bugs already sealed (hardcoded blower, equipability, difficultyPresetId) |
| 23 | [`PLAN-SELFTEST-TRUTH-23.md`](PLAN-SELFTEST-TRUTH-23.md) | Probes that cannot fail | 202 verbs, 29 CLI partials; export precedent of non-failing verification |
| 24 | [`PLAN-DEBT-DRAIN-24.md`](PLAN-DEBT-DRAIN-24.md) | Accepted debt, stale claims, doc sprawl | 4 ACCEPTED + 1 QUARANTINED debt rows, **46 open claims**, uncertified plan trees |
| 25 | [`PLAN-INPUT-HARDENING-25.md`](PLAN-INPUT-HARDENING-25.md) | Untrusted input | 181 save stores, host-pending mod contract, path/id surface, secrets rule |

## Expansion (26–30)

| # | Plan | Frontier | Evidence |
|---|---|---|---|
| 26 | [`PLAN-ECOLOGY-WILDLIFE-26.md`](PLAN-ECOLOGY-WILDLIFE-26.md) | Ecosystems, quotas, bestiary, infestation | `WildlifeEcosystemSystem`, `WildlifeHarvestQuotaEngine`, `BestiarySystem`, `EcologicalInfestation*`, 4 data catalogs |
| 27 | [`PLAN-MARITIME-DEEPWATER-27.md`](PLAN-MARITIME-DEEPWATER-27.md) | Tides, dives, salvage, flotilla, ice road | 15 maritime files + 5 data catalogs; 2 host-unreachable authorities |
| 28 | [`PLAN-WEATHER-ATMOSPHERE-28.md`](PLAN-WEATHER-ATMOSPHERE-28.md) | Forecast truth, nuclear winter, fallout, seeding | 6 host-unreachable weather authorities, 11 weather data catalogs |
| 29 | [`PLAN-WARLORDS-DIPLOMACY-29.md`](PLAN-WARLORDS-DIPLOMACY-29.md) | Doctrines, summits, treaties, territory | `Warlords/` + `Diplomacy/` + `Treaties/` + 22 faction data files |
| 30 | [`PLAN-TRANSPORT-EXPEDITION-30.md`](PLAN-TRANSPORT-EXPEDITION-30.md) | Modal travel, rail, air, fleet, outposts | 16 transport catalogs; `ModalTravelDispatchEngine`, `RailwayInterlockEngine`, `ColonySystem` host-unreachable |

## Recommended order

```
21 EVENT-WIRING ─┐
22 DATA-CONSUMER ├─ while Wave 1 wiring runs (they shrink the same gap)
23 SELFTEST-TRUTH─┘
24 DEBT-DRAIN ───── before new waves (frees claims + archives stale trees)
25 INPUT-HARDENING─ before any mod/public build
26–30 expansions ── after PLAN-ORPHAN-SEAL-01 waves for their systems
```

## Relationship to Waves 1–2

- Wave 1: orphan sealing, integration kit, unblocking, two verticals, launch face.
- Wave 2: governance, saves, determinism, data, UI, perf, tests, narrative, assets, release ops.
- Wave 3: five more **sealers** (events, field data, selftests, debt, inputs) and
  five new **frontiers** (ecology, maritime, weather, politics, transport).

Nothing in Wave 3 duplicates a Wave 1/2 package: the sealers target *different
gaps*, and the expansions name the Wave 1 wave that wires their base systems
first.

## Shared rules

- Core engine-free; extend the named owner; no parallel stores.
- JSON is authority; every new row needs a live consumer.
- Seeded RNG only; replay equals across mid-run save/reload.
- Focused verification per `TEST_POLICY.md`; no full-suite defaults.
- Fictional, original content only.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
