# PLAN-AUTOMATED-QA-CAMPAIGNS-74 — Appendix A: Matrix Runners by Domain

**Generated:** 2026-09-21 from `HostCliRegistry` (227 verbs grouped
into 31 domains).
**Use:** QA-74A — the **focus axis** of the scenario matrix. Each tier
(7-day fast / 30-day nightly / 180-day weekly) runs a rotation over
preset × seed × domain using the domain's runner verbs below, asserting the
invariants from the main plan.

## Matrix focus axis

| Domain | Runner count | Verbs |
|---|---:|---|
| `core` | 134 | `--7-day-smoke-selftest`, `--accessibility-selftest`, `--advanced-industrial-recon-selftest`, `--agriculture-selftest`, `--amphibious-draisine-selftest`, `--aquaponics-selftest`, `--arbitration-selftest`, `--asset-coverage-report` … |
| `save` | 14 | `--chemical-dependency-save-selftest`, `--duty-roster-save-selftest`, `--expansion-hub-save-selftest`, `--holdfast-save-selftest`, `--holdfast-trade-save-selftest`, `--inventory-save-selftest`, `--journal-save-selftest`, `--save-load-failure-selftest` … |
| `shelter` | 12 | `--shelter-actor-physics-selftest`, `--shelter-atmosphere-selftest`, `--shelter-decor-selftest`, `--shelter-hazard-loop-selftest`, `--shelter-hazard-selftest`, `--shelter-interior-selftest`, `--shelter-noise-selftest`, `--shelter-operations-selftest` … |
| `expansion` | 6 | `--all-expansions-selftest`, `--disease-expansion-selftest`, `--expansion-06-selftest`, `--expansion-08-selftest`, `--expansion-09-selftest`, `--expansions-selftest` |
| `expedition` | 5 | `--expedition-encounter-bridge-selftest`, `--expedition-panel-lifecycle`, `--expedition-panel-uitest`, `--expedition-playtest-selftest`, `--expedition-selftest` |
| `holdfast` | 5 | `--holdfast-briefing`, `--holdfast-runtime-selftest`, `--holdfast-runtime-ui-test`, `--holdfast-runtime-uitest`, `--holdfast-selftest` |
| `panel` | 5 | `--panel-bind-lifecycle-selftest`, `--panel-bind-selftest`, `--panel-lifecycle-selftest`, `--player-panels-ui-test`, `--player-panels-uitest` |
| `campaign` | 3 | `--campaign-journey-selftest`, `--propaganda-campaign-selftest`, `--real-campaign-journey-selftest` |
| `duty` | 3 | `--duty-roster-loop-selftest`, `--duty-roster-selftest`, `--duty-roster-uitest` |
| `world` | 3 | `--evolving-world-selftest`, `--world-playtest-selftest`, `--world-selftest` |
| `survivor` | 3 | `--survivor-death-selftest`, `--survivors-selftest`, `--survivors-uitest` |
| `audio` | 2 | `--audio-selftest`, `--audio-test` |
| `caravan` | 2 | `--caravan-selftest`, `--traveling-caravan-selftest` |
| `combat` | 2 | `--combat-breaching-selftest`, `--combat-selftest` |
| `data` | 2 | `--data-integrity-selftest`, `--user-data-dir` |
| `dose` | 2 | `--dose-ledger-selftest`, `--dose-uitest` |
| `economy` | 2 | `--economy-selftest`, `--economy-uitest` |
| `journal` | 2 | `--journal-selftest`, `--journal-uitest` |
| `weather` | 2 | `--journal-weather-panel-selftest`, `--weather-save-selftest` |
| `medical` | 2 | `--medical-selftest`, `--medical-ward-save-selftest` |
| `muster` | 2 | `--muster-selftest`, `--muster-uitest` |
| `onboarding` | 2 | `--onboarding-journey-selftest`, `--onboarding-selftest` |
| `radio` | 2 | `--radio-catalog-selftest`, `--radio-selftest` |
| `rail` | 2 | `--rail-grinding-selftest`, `--rail-grinding-uitest` |
| `verdict` | 2 | `--verdict-selftest`, `--verdict-uitest` |
| `census` | 1 | `--census-selftest` |
| `crossing` | 1 | `--crossing-selftest` |
| `difficulty` | 1 | `--difficulty-selftest` |
| `greenhouse` | 1 | `--greenhouse-selftest` |
| `power` | 1 | `--sofc-power-selftest` |
| `vehicle` | 1 | `--vehicle-garage-selftest` |
