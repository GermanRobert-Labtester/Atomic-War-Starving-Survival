# CLAIM READINESS INDEX — ASHFALL Expansion & Integration Program (2026-09-21)

**Status:** PROPOSED — reference for the foreman. This document is **not a
claim**, does not modify `INTEGRATION_PLANS.md` or `WORKTREE_OWNERSHIP.md`, and
grants no path ownership. It exists so that every one of the 276 programme plans
can be picked up, claimed, and executed without further discovery.

**Source of truth:** `INTEGRATION_PLANS.md` (queue), `WORKTREE_OWNERSHIP.md`
(paths), `TEST_POLICY.md` (tests), `KNOWN_DEBT.md` (debt). If this index and
those files disagree, those files win.

---

## 1. What "claim-ready" means here

Each plan carries its own **§24 Claim readiness** block containing:

- a readiness verdict (247 READY · 29 READY-WITH-NOTES · 0 NEEDS-AUTHORING),
- a class (governance / seed / hub / standard / free-start),
- a copy-paste `WORKTREE_OWNERSHIP.md` claim block: plan id, wave, packages,
  candidate claim paths, focused verification commands, dependencies,
- a 12-point structural checklist (status, wave, depends-on, non-goals,
  outcome, evidence, packages, acceptance, risks, verification, §12, §13),
- pre-claim actions listing anything still to author.

| Metric | Count |
|---|---:|
| Plans with §24 claim readiness | **276 / 276** |
| READY | 247 |
| READY-WITH-NOTES | 29 |
| NEEDS-AUTHORING | 0 |
| Governance / seed | 14 |
| Hubs (≥15 incoming) | 19 |
| Standard (1–14 incoming) | 221 |
| Free starts (0 incoming) | 22 |

---

## 2. Recommended execution order

### Stage 0 — governance spine (serialise; everything depends on it)

- **PLAN-BUILD-ERGONOMICS-56** — 258 incoming · READY
- **PLAN-HOST-CLI-CONTRACT-86** — 257 incoming · READY
- **PLAN-TEST-WELFARE-17** — 0 incoming · READY
- **PLAN-PROGRAMME-CLOSEOUT-100** — 268 incoming · READY
- **PLAN-HOTFIX-DRILL-99** — 256 incoming · READY
- **PLAN-AUTOMATED-QA-CAMPAIGNS-74** — 0 incoming · READY
- **PLAN-RELEASE-OPS-20** — 0 incoming · READY-WITH-NOTES
- **PLAN-DATA-AUTHORITY-14** — 0 incoming · READY
- **PLAN-DEBT-DRAIN-24** — 53 incoming · READY-WITH-NOTES
- **PLAN-DOC-ATLAS-CURRENCY-115** — 53 incoming · READY-WITH-NOTES

These are the universal-adjacency plans (Build Ergonomics 258, Host CLI Contract
257, Test Welfare 255, Closeout 268, Hotfix 256, QA Campaigns 171). Landing them
first converts "every plan cites them" from a coordination risk into a satisfied
precondition. Claim paths are document/build surfaces only — no Core code.

### Stage 1 — wave seeds

- **PLAN-ORPHAN-SEAL-01** — 45 incoming · READY-WITH-NOTES
- **PLAN-INTEGRATION-KIT-02** — 25 incoming · READY-WITH-NOTES
- **PLAN-UNBLOCK-03** — 36 incoming · READY-WITH-NOTES
- **PLAN-LAUNCH-FACE-06** — 27 incoming · READY-WITH-NOTES

Orphan Seal defines the seal batches; Unblock defines the entry order; the
Integration Kit defines patterns; Launch Face defines the visible surface.

### Stage 2 — hubs (coordinate before claiming)

| # | Plan | Coupling in | Host | Cats | Test regions |
|---|---:|---:|---:|---:|---:|
| 123 | `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 75 | 18 | 22 | 10 |
| 65 | `PLAN-ELECTRONICS-COMPUTING-65` | 33 | 18 | 22 | 5 |
| 262 | `PLAN-CORE-ROOT-FAMILY-TRUTH-262` | 32 | 19 | 22 | 4 |
| 264 | `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 28 | 17 | 22 | 9 |
| 35 | `PLAN-SILENT-FAILURE-35` | 27 | 27 | 22 | 10 |
| 31 | `PLAN-ARCHITECTURE-BOUNDARY-31` | 26 | 24 | 22 | 10 |
| 22 | `PLAN-DATA-CONSUMER-22` | 25 | 22 | 22 | 8 |
| 261 | `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 25 | 23 | 22 | 5 |
| 25 | `PLAN-INPUT-HARDENING-25` | 23 | 25 | 22 | 10 |
| 263 | `PLAN-MEDICAL-FAMILY-TRUTH-263` | 23 | 22 | 22 | 4 |
| 59 | `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 20 | 26 | 22 | 10 |
| 75 | `PLAN-DEV-TOOLING-TRUTH-75` | 19 | 25 | 22 | 10 |
| 271 | `PLAN-INVENTORY-FAMILY-TRUTH-271` | 19 | 12 | 22 | 2 |
| 280 | `PLAN-TRIO-FAMILY-TRUTH-280` | 18 | 15 | 22 | 2 |
| 23 | `PLAN-SELFTEST-TRUTH-23` | 17 | 16 | 15 | 9 |
| 270 | `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 17 | 24 | 22 | 5 |
| 60 | `PLAN-ORIGINALITY-LICENSING-60` | 16 | 25 | 22 | 9 |
| 265 | `PLAN-SHELTER-FAMILY-TRUTH-265` | 16 | 14 | 22 | 4 |
| 91 | `PLAN-HOST-EVENT-ARCHIVE-91` | 15 | 14 | 22 | 1 |

Hubs are not blockers — they are coordination points. A hub claim should either
land before the plans that name its files, or explicitly note the wait.

### Stage 3 — standard domains (1–14 incoming)

See the full table in §3. Batch these freely; each carries its own claim block
and verification command.

### Stage 4 — free starts (0 incoming) — maximum parallelism

`PLAN-CORE-ONLY-REGISTRY-11`, `PLAN-SAVE-GOVERNANCE-12`, `PLAN-DETERMINISM-REPLAY-13`, `PLAN-ASSET-PIPELINE-19`, `PLAN-ONBOARDING-TRUTH-55`, `PLAN-COLLECTIBLES-RELICS-67`, `PLAN-ANCIENT-RUINS-VAULTS-84`, `PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132`, `PLAN-UTILITY-AI-TRUTH-133`, `PLAN-THIRDONARY-COVENANT-TRUTH-134`, `PLAN-STANDING-RECORD-TRUTH-139`, `PLAN-NPC-ARCS-TRUTH-143`, `PLAN-STARTING-LEVEL-TRUTH-145`, `PLAN-KNOCK-WHITELIST-TRUTH-155`, `PLAN-JOURNEY-CONTEXT-TRUTH-156`, `PLAN-PORT-CONTRACT-TRUTH-157`, `PLAN-TRAVEL-ENCOUNTER-TRUTH-177`, `PLAN-RADIATION-BACKGROUND-TRUTH-189`, `PLAN-CROSSING-QUEST-TRUTH-190`, `PLAN-TUNNEL-NETWORK-TRUTH-194`, `PLAN-SOLAR-CONCENTRATOR-TRUTH-217`, `PLAN-POLITICS-SYSTEM-TRUTH-221`

These 22 plans name no artifacts that other plans reference. They are the safest
parallel starting points and the recommended first coding wave.

---

## 3. Full index (276 plans)

| # | Plan | Wave | Readiness | Packs | Cpl | Host | Cats | Reg | Ladder | RNG |
|---|---|---|---|---:|---:|---:|---:|---:|---:|---:|
| 1 | `PLAN-ORPHAN-SEAL-01` | — | READY-WITH-NOTES | 0 | 45 | 24 | 22 | 10 | 1 | 15 |
| 2 | `PLAN-INTEGRATION-KIT-02` | — | READY-WITH-NOTES | 0 | 25 | 25 | 22 | 10 | 1 | 14 |
| 3 | `PLAN-UNBLOCK-03` | — | READY-WITH-NOTES | 0 | 36 | 25 | 22 | 10 | 1 | 19 |
| 4 | `PLAN-VERTICAL-CULTURE-04` | — | READY-WITH-NOTES | 0 | 9 | 16 | 22 | 3 | 0 | 2 |
| 5 | `PLAN-VERTICAL-BODY-INDUSTRY-05` | — | READY-WITH-NOTES | 0 | 9 | 20 | 22 | 2 | 0 | 8 |
| 6 | `PLAN-LAUNCH-FACE-06` | — | READY-WITH-NOTES | 0 | 27 | 26 | 22 | 10 | 1 | 15 |
| 11 | `PLAN-CORE-ONLY-REGISTRY-11` | — | READY | 5 | 0 | 20 | 22 | 8 | 1 | 7 |
| 12 | `PLAN-SAVE-GOVERNANCE-12` | — | READY | 6 | 0 | 23 | 22 | 8 | 1 | 13 |
| 13 | `PLAN-DETERMINISM-REPLAY-13` | — | READY | 6 | 0 | 21 | 19 | 10 | 0 | 9 |
| 14 | `PLAN-DATA-AUTHORITY-14` | — | READY | 6 | 0 | 24 | 22 | 10 | 1 | 17 |
| 15 | `PLAN-UI-SURFACE-15` | — | READY | 6 | 2 | 12 | 2 | 0 | 0 | 0 |
| 16 | `PLAN-RUNTIME-PERF-16` | — | READY | 6 | 9 | 12 | 14 | 2 | 0 | 0 |
| 17 | `PLAN-TEST-WELFARE-17` | — | READY | 6 | 0 | 20 | 15 | 4 | 0 | 8 |
| 18 | `PLAN-NARRATIVE-GRAPH-18` | — | READY | 6 | 8 | 14 | 22 | 3 | 0 | 2 |
| 19 | `PLAN-ASSET-PIPELINE-19` | — | READY | 6 | 0 | 20 | 22 | 6 | 0 | 6 |
| 20 | `PLAN-RELEASE-OPS-20` | — | READY-WITH-NOTES | 6 | 0 | 25 | 22 | 10 | 0 | 18 |
| 21 | `PLAN-EVENT-WIRING-21` | — | READY-WITH-NOTES | 4 | 8 | 19 | 22 | 1 | 1 | 6 |
| 22 | `PLAN-DATA-CONSUMER-22` | — | READY | 5 | 25 | 22 | 22 | 8 | 0 | 12 |
| 23 | `PLAN-SELFTEST-TRUTH-23` | — | READY-WITH-NOTES | 5 | 17 | 16 | 15 | 9 | 1 | 4 |
| 24 | `PLAN-DEBT-DRAIN-24` | — | READY-WITH-NOTES | 5 | 53 | 27 | 22 | 10 | 1 | 20 |
| 25 | `PLAN-INPUT-HARDENING-25` | — | READY-WITH-NOTES | 6 | 23 | 25 | 22 | 10 | 1 | 19 |
| 26 | `PLAN-ECOLOGY-WILDLIFE-26` | — | READY-WITH-NOTES | 7 | 3 | 10 | 9 | 2 | 0 | 4 |
| 27 | `PLAN-MARITIME-DEEPWATER-27` | — | READY | 6 | 3 | 15 | 10 | 2 | 0 | 2 |
| 28 | `PLAN-WEATHER-ATMOSPHERE-28` | — | READY | 7 | 8 | 16 | 22 | 3 | 0 | 4 |
| 29 | `PLAN-WARLORDS-DIPLOMACY-29` | — | READY | 7 | 10 | 19 | 22 | 3 | 1 | 1 |
| 30 | `PLAN-TRANSPORT-EXPEDITION-30` | — | READY | 7 | 14 | 17 | 22 | 4 | 0 | 5 |
| 31 | `PLAN-ARCHITECTURE-BOUNDARY-31` | — | READY-WITH-NOTES | 5 | 26 | 24 | 22 | 10 | 1 | 9 |
| 32 | `PLAN-LIFECYCLE-SEALING-32` | — | READY-WITH-NOTES | 5 | 9 | 10 | 8 | 1 | 1 | 0 |
| 33 | `PLAN-TEMPORAL-AUTHORITY-33` | — | READY-WITH-NOTES | 5 | 2 | 7 | 1 | 1 | 0 | 0 |
| 34 | `PLAN-REFERENCE-INTEGRITY-34` | — | READY-WITH-NOTES | 5 | 13 | 0 | 3 | 0 | 0 | 0 |
| 35 | `PLAN-SILENT-FAILURE-35` | — | READY-WITH-NOTES | 5 | 27 | 27 | 22 | 10 | 0 | 17 |
| 36 | `PLAN-BELIEF-IDEOLOGY-36` | — | READY | 6 | 4 | 6 | 4 | 1 | 0 | 0 |
| 37 | `PLAN-JUSTICE-LAW-37` | — | READY-WITH-NOTES | 6 | 7 | 14 | 22 | 2 | 1 | 2 |
| 38 | `PLAN-SCIENCE-EDUCATION-38` | — | READY | 7 | 2 | 13 | 18 | 1 | 0 | 0 |
| 39 | `PLAN-FOOD-CUISINE-39` | — | READY | 7 | 4 | 1 | 4 | 1 | 0 | 0 |
| 40 | `PLAN-SHELTER-ARCHITECTURE-40` | — | READY | 7 | 8 | 13 | 20 | 1 | 0 | 1 |
| 41 | `PLAN-ESPIONAGE-COUNTERINTEL-41` | — | READY-WITH-NOTES | 6 | 6 | 15 | 22 | 3 | 0 | 1 |
| 42 | `PLAN-RADIO-MEDIA-42` | — | READY-WITH-NOTES | 7 | 7 | 18 | 22 | 3 | 0 | 2 |
| 43 | `PLAN-FAMILY-DYNASTY-43` | — | READY | 7 | 6 | 5 | 3 | 0 | 0 | 0 |
| 44 | `PLAN-CRIME-SYNDICATES-44` | — | READY-WITH-NOTES | 7 | 7 | 15 | 11 | 1 | 0 | 3 |
| 45 | `PLAN-INDUSTRY-AUTOMATION-45` | — | READY-WITH-NOTES | 7 | 11 | 17 | 22 | 1 | 0 | 5 |
| 46 | `PLAN-WATER-AGRICULTURE-46` | — | READY-WITH-NOTES | 7 | 6 | 12 | 10 | 1 | 0 | 3 |
| 47 | `PLAN-PANDEMIC-PUBLIC-HEALTH-47` | — | READY-WITH-NOTES | 7 | 11 | 18 | 18 | 1 | 0 | 6 |
| 48 | `PLAN-ENERGY-NUCLEAR-48` | — | READY-WITH-NOTES | 7 | 6 | 20 | 22 | 2 | 0 | 8 |
| 49 | `PLAN-SIGNALS-REMOTE-SENSING-49` | — | READY-WITH-NOTES | 7 | 10 | 16 | 22 | 3 | 1 | 3 |
| 50 | `PLAN-RECREATION-MORALE-50` | — | READY | 7 | 13 | 7 | 5 | 1 | 0 | 0 |
| 51 | `PLAN-ACCESSIBILITY-CLOSURE-51` | 6 | READY | 5 | 6 | 22 | 22 | 7 | 0 | 9 |
| 52 | `PLAN-LOCALIZATION-READINESS-52` | 6 | READY | 5 | 5 | 12 | 7 | 2 | 0 | 4 |
| 53 | `PLAN-PLATFORM-PARITY-53` | 6 | READY | 6 | 5 | 2 | 0 | 0 | 0 | 0 |
| 54 | `PLAN-SETTINGS-INTEGRITY-54` | 6 | READY | 5 | 6 | 12 | 6 | 1 | 0 | 0 |
| 55 | `PLAN-ONBOARDING-TRUTH-55` | 6 | READY | 6 | 0 | 5 | 0 | 0 | 0 | 0 |
| 56 | `PLAN-BUILD-ERGONOMICS-56` | 6 | READY | 5 | 258 | 25 | 22 | 10 | 0 | 14 |
| 57 | `PLAN-RUNTIME-RESILIENCE-57` | 6 | READY | 6 | 10 | 12 | 14 | 1 | 1 | 0 |
| 58 | `PLAN-TELEMETRY-PRIVACY-58` | 6 | READY | 5 | 11 | 12 | 12 | 1 | 0 | 0 |
| 59 | `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 6 | READY | 5 | 20 | 26 | 22 | 10 | 0 | 14 |
| 60 | `PLAN-ORIGINALITY-LICENSING-60` | 6 | READY-WITH-NOTES | 5 | 16 | 25 | 22 | 9 | 0 | 17 |
| 61 | `PLAN-BASE-DEFENSE-RAIDS-61` | 6 | READY | 6 | 7 | 14 | 12 | 1 | 0 | 3 |
| 62 | `PLAN-COMBAT-DEPTH-62` | 6 | READY | 7 | 7 | 16 | 22 | 2 | 0 | 2 |
| 63 | `PLAN-ANOMALY-PHANTOM-63` | 6 | READY | 6 | 8 | 9 | 16 | 0 | 0 | 2 |
| 64 | `PLAN-MENTAL-HEALTH-THERAPY-64` | 6 | READY | 7 | 4 | 6 | 6 | 0 | 0 | 0 |
| 65 | `PLAN-ELECTRONICS-COMPUTING-65` | 6 | READY | 7 | 33 | 18 | 22 | 5 | 1 | 6 |
| 66 | `PLAN-CREATIVE-WORKS-66` | 6 | READY | 7 | 6 | 13 | 20 | 2 | 0 | 1 |
| 67 | `PLAN-COLLECTIBLES-RELICS-67` | 6 | READY | 7 | 0 | 6 | 4 | 1 | 0 | 1 |
| 68 | `PLAN-LABOUR-PROFESSIONS-68` | 6 | READY | 7 | 3 | 26 | 22 | 10 | 1 | 14 |
| 69 | `PLAN-SHELTER-POLITICS-69` | 6 | READY | 6 | 3 | 13 | 19 | 2 | 0 | 1 |
| 70 | `PLAN-CARTOGRAPHY-LANDMARKS-70` | 6 | READY | 7 | 11 | 16 | 22 | 1 | 0 | 4 |
| 71 | `PLAN-HOST-COMPOSITION-GOVERNANCE-71` | 7 | READY-WITH-NOTES | 5 | 10 | 25 | 22 | 10 | 1 | 14 |
| 72 | `PLAN-THREADING-ASYNCHRONY-72` | 7 | READY | 5 | 3 | 14 | 14 | 1 | 0 | 4 |
| 73 | `PLAN-BALANCE-DIFFICULTY-INTEGRATION-73` | 7 | READY | 7 | 4 | 19 | 14 | 10 | 0 | 7 |
| 74 | `PLAN-AUTOMATED-QA-CAMPAIGNS-74` | 7 | READY | 6 | 0 | 25 | 22 | 10 | 1 | 7 |
| 75 | `PLAN-DEV-TOOLING-TRUTH-75` | 7 | READY | 6 | 19 | 25 | 22 | 10 | 2 | 11 |
| 76 | `PLAN-ACHIEVEMENTS-COMPLETION-TRUTH-76` | 7 | READY | 5 | 1 | 20 | 11 | 8 | 1 | 8 |
| 77 | `PLAN-CONTENT-PIPELINE-QA-77` | 7 | READY | 6 | 8 | 12 | 12 | 0 | 0 | 1 |
| 78 | `PLAN-BIONICS-ENHANCEMENT-78` | 7 | READY | 7 | 8 | 8 | 13 | 1 | 0 | 1 |
| 79 | `PLAN-AUTONOMOUS-MACHINES-79` | 7 | READY | 8 | 2 | 6 | 4 | 0 | 0 | 1 |
| 80 | `PLAN-CRISIS-DISASTER-RESPONSE-80` | 7 | READY | 7 | 4 | 12 | 12 | 1 | 0 | 1 |
| 81 | `PLAN-MUTATION-HEREDITY-81` | 7 | READY | 7 | 7 | 7 | 0 | 0 | 0 | 1 |
| 82 | `PLAN-NOMADS-CARAVAN-CULTURE-82` | 7 | READY | 7 | 11 | 15 | 22 | 3 | 0 | 3 |
| 83 | `PLAN-DEEP-STRATA-83` | 7 | READY | 7 | 12 | 14 | 22 | 1 | 0 | 2 |
| 84 | `PLAN-ANCIENT-RUINS-VAULTS-84` | 7 | READY | 7 | 0 | 16 | 16 | 1 | 0 | 3 |
| 85 | `PLAN-ASYLUM-REFUGEES-85` | 7 | READY | 7 | 3 | 26 | 22 | 9 | 0 | 8 |
| 86 | `PLAN-HOST-CLI-CONTRACT-86` | 8 | READY | 10 | 257 | 22 | 22 | 9 | 3 | 8 |
| 87 | `PLAN-SAVE-MIGRATION-CORRIDOR-87` | 8 | READY | 5 | 12 | 13 | 10 | 1 | 0 | 1 |
| 88 | `PLAN-TEXT-PACK-LOCALIZATION-88` | 8 | READY | 5 | 2 | 10 | 7 | 2 | 0 | 4 |
| 89 | `PLAN-DETERMINISM-CROSS-HOST-89` | 8 | READY | 5 | 14 | 19 | 22 | 8 | 0 | 7 |
| 90 | `PLAN-DATA-SCHEMA-COVERAGE-90` | 8 | READY | 5 | 2 | 5 | 5 | 0 | 0 | 0 |
| 91 | `PLAN-HOST-EVENT-ARCHIVE-91` | 8 | READY | 5 | 15 | 14 | 22 | 1 | 0 | 1 |
| 92 | `PLAN-MOD-CONTENT-BOUNDARY-92` | 8 | READY | 5 | 1 | 1 | 10 | 0 | 0 | 0 |
| 93 | `PLAN-INVENTORY-CONSERVATION-93` | 8 | READY | 5 | 14 | 8 | 5 | 1 | 0 | 0 |
| 94 | `PLAN-DEPRECATED-TREE-RETIREMENT-94` | 8 | READY | 5 | 12 | 25 | 22 | 10 | 0 | 17 |
| 95 | `PLAN-SPATIAL-SIM-AUTHORITY-95` | 8 | READY | 5 | 10 | 21 | 22 | 2 | 0 | 5 |
| 96 | `PLAN-ECONOMY-LEDGER-TRUTH-96` | 8 | READY | 5 | 12 | 20 | 22 | 3 | 1 | 7 |
| 97 | `PLAN-AUDIO-MIX-AUTHORITY-97` | 8 | READY | 5 | 9 | 14 | 22 | 3 | 0 | 2 |
| 98 | `PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98` | 8 | READY | 5 | 9 | 12 | 4 | 1 | 0 | 0 |
| 99 | `PLAN-HOTFIX-DRILL-99` | 8 | READY | 5 | 256 | 20 | 16 | 5 | 0 | 8 |
| 100 | `PLAN-PROGRAMME-CLOSEOUT-100` | 8 | READY | 5 | 268 | 26 | 22 | 10 | 0 | 14 |
| 101 | `PLAN-DUTY-ROSTER-TRUTH-101` | 9 | READY | 5 | 10 | 13 | 22 | 2 | 2 | 1 |
| 102 | `PLAN-SCENARIO-AUTHORING-102` | 9 | READY | 5 | 2 | 14 | 15 | 2 | 0 | 2 |
| 103 | `PLAN-SHELTER-CAPACITY-AUTHORITY-103` | 9 | READY | 5 | 5 | 15 | 22 | 3 | 0 | 3 |
| 104 | `PLAN-CAMPAIGN-PORTABILITY-104` | 9 | READY | 5 | 9 | 7 | 2 | 1 | 0 | 0 |
| 105 | `PLAN-SAVE-SLOT-UX-105` | 9 | READY | 5 | 13 | 12 | 2 | 0 | 0 | 0 |
| 106 | `PLAN-INPUT-REBINDING-106` | 9 | READY | 5 | 5 | 12 | 6 | 1 | 0 | 0 |
| 107 | `PLAN-DAILY-ROUTINE-AUTHORITY-107` | 9 | READY | 5 | 3 | 20 | 22 | 8 | 1 | 6 |
| 108 | `PLAN-DISCOVERY-STATE-108` | 9 | READY | 5 | 6 | 3 | 22 | 0 | 0 | 0 |
| 109 | `PLAN-CONTRACT-BOARD-109` | 9 | READY | 5 | 9 | 18 | 22 | 2 | 0 | 6 |
| 110 | `PLAN-CODEX-SURFACE-TRUTH-110` | 9 | READY | 5 | 3 | 12 | 10 | 2 | 0 | 0 |
| 111 | `PLAN-SESSION-DURABILITY-111` | 9 | READY | 5 | 10 | 25 | 22 | 10 | 0 | 18 |
| 112 | `PLAN-CRAFT-QUALITY-TRUTH-112` | 9 | READY | 5 | 3 | 8 | 8 | 1 | 0 | 0 |
| 113 | `PLAN-SKILL-PROGRESSION-TRUTH-113` | 9 | READY | 5 | 3 | 4 | 2 | 1 | 0 | 0 |
| 114 | `PLAN-SAVE-PREVIEW-METADATA-114` | 9 | READY | 5 | 12 | 8 | 3 | 1 | 0 | 0 |
| 115 | `PLAN-DOC-ATLAS-CURRENCY-115` | 9 | READY-WITH-NOTES | 5 | 53 | 23 | 22 | 10 | 1 | 7 |
| 116 | `PLAN-NOISE-DISCIPLINE-TRUTH-116` | 10 | READY | 5 | 3 | 25 | 22 | 7 | 0 | 16 |
| 117 | `PLAN-THERMAL-EXPOSURE-TRUTH-117` | 10 | READY | 5 | 11 | 3 | 6 | 0 | 0 | 0 |
| 118 | `PLAN-PRESERVATION-TRUTH-118` | 10 | READY | 5 | 9 | 3 | 13 | 0 | 0 | 0 |
| 119 | `PLAN-MAINTENANCE-DECAY-TRUTH-119` | 10 | READY | 5 | 10 | 13 | 22 | 2 | 0 | 1 |
| 120 | `PLAN-RUMOR-PROPAGATION-TRUTH-120` | 10 | READY | 5 | 7 | 16 | 10 | 1 | 0 | 4 |
| 121 | `PLAN-INVESTIGATION-EVIDENCE-TRUTH-121` | 10 | READY | 5 | 5 | 14 | 22 | 2 | 1 | 2 |
| 122 | `PLAN-COMMITMENTS-OBLIGATIONS-TRUTH-122` | 10 | READY | 5 | 6 | 13 | 19 | 0 | 0 | 0 |
| 123 | `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 10 | READY | 5 | 75 | 18 | 22 | 10 | 0 | 4 |
| 124 | `PLAN-ACUTE-TRAUMA-CARE-124` | 10 | READY | 5 | 11 | 11 | 9 | 1 | 0 | 1 |
| 125 | `PLAN-RECIPE-REACHABILITY-TRUTH-125` | 10 | READY | 5 | 3 | 5 | 6 | 0 | 0 | 0 |
| 126 | `PLAN-BACKSTORY-REVEAL-TRUTH-126` | 10 | READY | 5 | 4 | 17 | 22 | 7 | 0 | 4 |
| 127 | `PLAN-SECRETS-CONFESSION-TRUTH-127` | 10 | READY | 5 | 3 | 0 | 2 | 0 | 0 | 0 |
| 128 | `PLAN-PRINT-MEDIA-TRUTH-128` | 10 | READY | 5 | 6 | 24 | 22 | 9 | 0 | 13 |
| 129 | `PLAN-MORALE-UNREST-TRUTH-129` | 10 | READY | 5 | 4 | 22 | 22 | 10 | 0 | 15 |
| 130 | `PLAN-MUSTER-COALITION-TRUTH-130` | 10 | READY | 5 | 3 | 15 | 22 | 0 | 0 | 1 |
| 131 | `PLAN-PLAYER-COMMAND-TRUTH-131` | 11 | READY | 5 | 1 | 7 | 1 | 2 | 0 | 0 |
| 132 | `PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132` | 11 | READY | 5 | 0 | 13 | 18 | 2 | 0 | 1 |
| 133 | `PLAN-UTILITY-AI-TRUTH-133` | 11 | READY | 5 | 0 | 7 | 2 | 0 | 0 | 0 |
| 134 | `PLAN-THIRDONARY-COVENANT-TRUTH-134` | 11 | READY | 5 | 0 | 12 | 8 | 0 | 0 | 0 |
| 135 | `PLAN-SKY-DEFENSE-TRUTH-135` | 11 | READY | 5 | 2 | 12 | 2 | 1 | 0 | 3 |
| 136 | `PLAN-MORAL-CHOICE-TRUTH-136` | 11 | READY | 5 | 2 | 23 | 22 | 1 | 0 | 1 |
| 137 | `PLAN-ENDGAME-EVALUATION-TRUTH-137` | 11 | READY | 5 | 5 | 12 | 8 | 2 | 0 | 0 |
| 138 | `PLAN-FEEDBACK-SURFACE-TRUTH-138` | 11 | READY | 5 | 2 | 10 | 3 | 0 | 0 | 1 |
| 139 | `PLAN-STANDING-RECORD-TRUTH-139` | 11 | READY | 5 | 0 | 11 | 13 | 0 | 0 | 0 |
| 140 | `PLAN-ADVANCED-MACHINERY-CONTRACTS-TRUTH-140` | 11 | READY | 5 | 1 | 22 | 22 | 6 | 2 | 8 |
| 141 | `PLAN-INSTITUTIONS-TRUTH-141` | 11 | READY | 5 | 1 | 7 | 4 | 0 | 1 | 0 |
| 142 | `PLAN-MEMORY-DECAY-TRUTH-142` | 11 | READY | 5 | 1 | 24 | 22 | 8 | 0 | 5 |
| 143 | `PLAN-NPC-ARCS-TRUTH-143` | 11 | READY | 5 | 0 | 1 | 7 | 0 | 0 | 0 |
| 144 | `PLAN-SANATORIUM-TRUTH-144` | 11 | READY | 5 | 1 | 1 | 5 | 0 | 0 | 0 |
| 145 | `PLAN-STARTING-LEVEL-TRUTH-145` | 11 | READY | 5 | 0 | 4 | 5 | 0 | 0 | 0 |
| 146 | `PLAN-YEAR-OF-ASH-TRUTH-146` | 12 | READY | 5 | 5 | 16 | 22 | 0 | 1 | 1 |
| 147 | `PLAN-BOOTSTRAP-GATE-TRUTH-147` | 12 | READY | 5 | 9 | 10 | 8 | 1 | 1 | 0 |
| 148 | `PLAN-CATALOG-BOOT-TRUTH-148` | 12 | READY | 5 | 2 | 5 | 6 | 0 | 0 | 1 |
| 149 | `PLAN-HEIRLOOM-PHANTOM-TRUTH-149` | 12 | READY | 5 | 6 | 4 | 6 | 0 | 0 | 1 |
| 150 | `PLAN-PROPAGANDA-TRUTH-150` | 12 | READY | 5 | 1 | 23 | 22 | 10 | 0 | 11 |
| 151 | `PLAN-TREATY-CONSEQUENCES-TRUTH-151` | 12 | READY | 5 | 1 | 28 | 22 | 10 | 0 | 15 |
| 152 | `PLAN-ARCHAEOLOGY-TRUTH-152` | 12 | READY | 5 | 2 | 17 | 13 | 7 | 1 | 5 |
| 153 | `PLAN-WAYSTATION-NETWORK-TRUTH-153` | 12 | READY | 5 | 1 | 10 | 4 | 0 | 0 | 0 |
| 154 | `PLAN-VEHICLE-CUSTOMIZATION-TRUTH-154` | 12 | READY | 5 | 6 | 23 | 22 | 10 | 2 | 15 |
| 155 | `PLAN-KNOCK-WHITELIST-TRUTH-155` | 12 | READY | 5 | 0 | 23 | 22 | 8 | 0 | 8 |
| 156 | `PLAN-JOURNEY-CONTEXT-TRUTH-156` | 12 | READY | 5 | 0 | 19 | 11 | 8 | 1 | 7 |
| 157 | `PLAN-PORT-CONTRACT-TRUTH-157` | 12 | READY | 5 | 0 | 19 | 22 | 5 | 1 | 5 |
| 158 | `PLAN-RAIL-MAINTENANCE-TRUTH-158` | 12 | READY | 5 | 2 | 16 | 22 | 7 | 0 | 4 |
| 159 | `PLAN-INTERNAL-COMMUNICATION-TRUTH-159` | 12 | READY | 5 | 6 | 5 | 4 | 1 | 0 | 0 |
| 160 | `PLAN-GENERATIONAL-MILESTONE-TRUTH-160` | 12 | READY | 5 | 2 | 16 | 22 | 4 | 2 | 4 |
| 161 | `PLAN-ESPIONAGE-SYSTEM-TRUTH-161` | 13 | READY | 5 | 6 | 15 | 22 | 3 | 0 | 1 |
| 162 | `PLAN-MORALE-CONTAGION-TRUTH-162` | 13 | READY | 5 | 2 | 6 | 2 | 0 | 0 | 0 |
| 163 | `PLAN-AQUAPONICS-TRUTH-163` | 13 | READY | 5 | 3 | 4 | 9 | 0 | 0 | 2 |
| 164 | `PLAN-AQUIFER-MONITORING-TRUTH-164` | 13 | READY | 5 | 5 | 12 | 10 | 0 | 0 | 0 |
| 165 | `PLAN-PERIMETER-DEFENSE-TRUTH-165` | 13 | READY | 5 | 5 | 12 | 4 | 1 | 0 | 3 |
| 166 | `PLAN-TRADE-EMBARGO-TRUTH-166` | 13 | READY | 5 | 2 | 26 | 22 | 9 | 1 | 15 |
| 167 | `PLAN-PHARMACEUTICAL-TRUTH-167` | 13 | READY | 5 | 2 | 1 | 2 | 0 | 0 | 0 |
| 168 | `PLAN-WEATHER-SONDE-TRUTH-168` | 13 | READY | 5 | 3 | 13 | 11 | 1 | 0 | 1 |
| 169 | `PLAN-CULTURAL-ARCHIVE-TRUTH-169` | 13 | READY | 5 | 1 | 13 | 13 | 0 | 0 | 0 |
| 170 | `PLAN-NARRATIVE-CONTINUITY-TRUTH-170` | 13 | READY | 5 | 2 | 13 | 18 | 2 | 0 | 1 |
| 171 | `PLAN-FACTION-BRANCH-TRUTH-171` | 13 | READY | 5 | 3 | 18 | 22 | 0 | 0 | 0 |
| 172 | `PLAN-METROLOGY-TRUTH-172` | 13 | READY | 5 | 3 | 9 | 4 | 1 | 0 | 3 |
| 173 | `PLAN-LEADERSHIP-TRUTH-173` | 13 | READY | 5 | 1 | 21 | 22 | 8 | 0 | 6 |
| 174 | `PLAN-RATIONING-TRUTH-174` | 13 | READY | 5 | 8 | 3 | 3 | 1 | 0 | 1 |
| 175 | `PLAN-WORKSHOP-TRUTH-175` | 13 | READY | 5 | 1 | 23 | 22 | 9 | 0 | 9 |
| 176 | `PLAN-NARRATIVE-ARC-EVENT-TRUTH-176` | 14 | READY | 5 | 2 | 24 | 22 | 10 | 0 | 19 |
| 177 | `PLAN-TRAVEL-ENCOUNTER-TRUTH-177` | 14 | READY | 5 | 0 | 7 | 4 | 0 | 0 | 0 |
| 178 | `PLAN-BIOFERMENTATION-TRUTH-178` | 14 | READY | 5 | 1 | 23 | 22 | 7 | 0 | 12 |
| 179 | `PLAN-FLUID-LOGISTICS-TRUTH-179` | 14 | READY | 5 | 2 | 12 | 20 | 1 | 0 | 0 |
| 180 | `PLAN-PNEUMATIC-DISPATCH-TRUTH-180` | 14 | READY | 5 | 2 | 12 | 22 | 4 | 1 | 0 |
| 181 | `PLAN-KINETIC-STORAGE-TRUTH-181` | 14 | READY | 5 | 1 | 24 | 22 | 10 | 0 | 15 |
| 182 | `PLAN-MICROFLUIDIC-DIAGNOSTIC-TRUTH-182` | 14 | READY | 5 | 2 | 7 | 1 | 0 | 0 | 1 |
| 183 | `PLAN-CHEMICAL-RECON-TRUTH-183` | 14 | READY | 5 | 2 | 24 | 22 | 9 | 0 | 13 |
| 184 | `PLAN-BALLISTICS-WORKBENCH-TRUTH-184` | 14 | READY | 5 | 2 | 2 | 3 | 0 | 0 | 0 |
| 185 | `PLAN-NARRATIVE-ENCOUNTER-TRUTH-185` | 14 | READY | 5 | 2 | 25 | 22 | 9 | 0 | 15 |
| 186 | `PLAN-PSYCHOLOGICAL-ARC-TRUTH-186` | 14 | READY | 5 | 1 | 23 | 22 | 10 | 1 | 19 |
| 187 | `PLAN-PLASTIC-PYROLYSIS-TRUTH-187` | 14 | READY | 5 | 1 | 23 | 22 | 6 | 0 | 12 |
| 188 | `PLAN-COATING-TECH-TRUTH-188` | 14 | READY | 5 | 2 | 14 | 11 | 0 | 0 | 2 |
| 189 | `PLAN-RADIATION-BACKGROUND-TRUTH-189` | 14 | READY | 5 | 0 | 15 | 20 | 3 | 0 | 3 |
| 190 | `PLAN-CROSSING-QUEST-TRUTH-190` | 14 | READY | 5 | 0 | 12 | 17 | 1 | 0 | 0 |
| 191 | `PLAN-GEOTHERMAL-PLANT-TRUTH-191` | 15 | READY | 5 | 5 | 12 | 9 | 0 | 0 | 0 |
| 192 | `PLAN-DOCUMENT-DISCOVERY-TRUTH-192` | 15 | READY | 5 | 1 | 21 | 22 | 7 | 0 | 8 |
| 193 | `PLAN-SEISMIC-DYNAMICS-TRUTH-193` | 15 | READY | 5 | 2 | 22 | 22 | 9 | 0 | 8 |
| 194 | `PLAN-TUNNEL-NETWORK-TRUTH-194` | 15 | READY | 5 | 0 | 8 | 16 | 5 | 0 | 1 |
| 195 | `PLAN-RELATIONSHIP-DECAY-TRUTH-195` | 15 | READY | 5 | 2 | 24 | 22 | 7 | 0 | 15 |
| 196 | `PLAN-HEALTH-HISTORY-TRUTH-196` | 15 | READY | 5 | 1 | 22 | 22 | 9 | 0 | 8 |
| 197 | `PLAN-PRISONER-TRUTH-197` | 15 | READY | 5 | 2 | 24 | 16 | 7 | 0 | 15 |
| 198 | `PLAN-FORCED-LABOR-TRUTH-198` | 15 | READY | 5 | 1 | 23 | 19 | 8 | 2 | 7 |
| 199 | `PLAN-CHLOR-ALKALI-TRUTH-199` | 15 | READY | 5 | 3 | 23 | 18 | 5 | 0 | 12 |
| 200 | `PLAN-FINAL-WISH-TRUTH-200` | 15 | READY | 5 | 1 | 3 | 2 | 0 | 0 | 0 |
| 201 | `PLAN-ECHO-TRUTH-201` | 15 | READY | 5 | 1 | 3 | 0 | 0 | 0 | 1 |
| 202 | `PLAN-FISCHER-TROPSCH-TRUTH-202` | 15 | READY | 5 | 4 | 14 | 14 | 0 | 0 | 2 |
| 203 | `PLAN-CAREGIVING-TRUTH-203` | 15 | READY | 5 | 1 | 25 | 22 | 9 | 0 | 18 |
| 204 | `PLAN-DOSIMETER-CALIBRATION-TRUTH-204` | 15 | READY | 5 | 1 | 2 | 0 | 0 | 0 | 1 |
| 205 | `PLAN-BLACK-PROJECTS-TRUTH-205` | 15 | READY | 5 | 4 | 16 | 10 | 0 | 0 | 3 |
| 206 | `PLAN-CRYO-VAULT-TRUTH-206` | 16 | READY | 5 | 1 | 24 | 22 | 9 | 0 | 14 |
| 207 | `PLAN-DEFENSE-COMMAND-TRUTH-207` | 16 | READY | 5 | 2 | 12 | 4 | 2 | 0 | 3 |
| 208 | `PLAN-CRAFT-ARCHIVE-TRUTH-208` | 16 | READY | 5 | 1 | 12 | 15 | 0 | 0 | 0 |
| 209 | `PLAN-RADIO-STATION-TRUTH-209` | 16 | READY | 5 | 3 | 16 | 22 | 2 | 0 | 2 |
| 210 | `PLAN-PSYOPS-TRUTH-210` | 16 | READY | 5 | 1 | 1 | 0 | 0 | 0 | 0 |
| 211 | `PLAN-DISCOVERY-CONSEQUENCE-TRUTH-211` | 16 | READY | 5 | 3 | 26 | 22 | 8 | 0 | 14 |
| 212 | `PLAN-DYNAMIC-QUESTLINE-TRUTH-212` | 16 | READY | 5 | 2 | 13 | 22 | 2 | 0 | 1 |
| 213 | `PLAN-SURGICAL-WARD-TRUTH-213` | 16 | READY | 5 | 11 | 15 | 13 | 1 | 0 | 3 |
| 214 | `PLAN-SOCIAL-DYNAMICS-TRUTH-214` | 16 | READY | 5 | 1 | 23 | 22 | 10 | 0 | 9 |
| 215 | `PLAN-NARCOTICS-TRUTH-215` | 16 | READY | 5 | 1 | 24 | 22 | 10 | 0 | 15 |
| 216 | `PLAN-PROCEDURAL-NARRATIVE-TRUTH-216` | 16 | READY | 5 | 1 | 25 | 22 | 9 | 1 | 16 |
| 217 | `PLAN-SOLAR-CONCENTRATOR-TRUTH-217` | 16 | READY | 5 | 0 | 23 | 22 | 8 | 1 | 11 |
| 218 | `PLAN-WEATHER-INTELLIGENCE-TRUTH-218` | 16 | READY | 5 | 3 | 13 | 11 | 1 | 0 | 1 |
| 219 | `PLAN-EXPEDITION-VEHICLE-TRUTH-219` | 16 | READY | 5 | 11 | 13 | 22 | 0 | 0 | 1 |
| 220 | `PLAN-PRECISION-OPTICS-TRUTH-220` | 16 | READY | 5 | 7 | 9 | 5 | 1 | 0 | 3 |
| 221 | `PLAN-POLITICS-SYSTEM-TRUTH-221` | 17 | READY | 5 | 0 | 17 | 11 | 7 | 1 | 5 |
| 222 | `PLAN-JUSTICE-SYSTEM-TRUTH-222` | 17 | READY | 5 | 2 | 25 | 22 | 10 | 0 | 13 |
| 223 | `PLAN-CEREMONY-SYSTEM-TRUTH-223` | 17 | READY | 5 | 1 | 16 | 10 | 6 | 1 | 4 |
| 224 | `PLAN-INTERNAL-SECURITY-TRUTH-224` | 17 | READY | 5 | 3 | 13 | 20 | 3 | 0 | 1 |
| 225 | `PLAN-SHELTER-DECOR-TRUTH-225` | 17 | READY | 5 | 5 | 13 | 19 | 1 | 0 | 1 |
| 226 | `PLAN-CHEMICAL-SYNTHESIS-TRUTH-226` | 17 | READY | 5 | 6 | 14 | 14 | 0 | 0 | 2 |
| 227 | `PLAN-WORLD-EVOLUTION-TRUTH-227` | 17 | READY | 5 | 1 | 6 | 6 | 0 | 0 | 2 |
| 228 | `PLAN-FACTION-BRANCH-STATUS-TRUTH-228` | 17 | READY | 5 | 3 | 18 | 22 | 0 | 0 | 0 |
| 229 | `PLAN-DREAM-SYSTEM-TRUTH-229` | 17 | READY | 5 | 1 | 19 | 22 | 8 | 0 | 5 |
| 230 | `PLAN-TRAUMA-SYSTEM-TRUTH-230` | 17 | READY | 5 | 3 | 8 | 4 | 1 | 0 | 1 |
| 231 | `PLAN-MORAL-BRANCHING-TRUTH-231` | 17 | READY | 5 | 1 | 27 | 22 | 9 | 1 | 15 |
| 232 | `PLAN-DESPERATION-TRUTH-232` | 17 | READY | 5 | 3 | 23 | 22 | 10 | 0 | 13 |
| 233 | `PLAN-RESPIRATORY-DEGENERATION-TRUTH-233` | 17 | READY | 5 | 3 | 0 | 1 | 0 | 0 | 0 |
| 234 | `PLAN-CONTRABAND-STASH-TRUTH-234` | 17 | READY | 5 | 1 | 7 | 22 | 0 | 0 | 0 |
| 235 | `PLAN-HELIOGRAPH-TRUTH-235` | 17 | READY | 5 | 5 | 14 | 22 | 1 | 1 | 1 |
| 236 | `PLAN-AMBIENT-TEXT-TRUTH-236` | 17 | READY | 5 | 2 | 5 | 5 | 0 | 0 | 0 |
| 237 | `PLAN-FIELD-DISCOVERY-TRUTH-237` | 17 | READY | 5 | 2 | 5 | 18 | 0 | 0 | 0 |
| 238 | `PLAN-LORE-ARCHIVE-TRUTH-238` | 17 | READY | 5 | 3 | 13 | 22 | 1 | 0 | 0 |
| 239 | `PLAN-LATENT-EXPERT-TRUTH-239` | 17 | READY | 5 | 4 | 4 | 2 | 1 | 0 | 0 |
| 240 | `PLAN-CARBON-COMPOSITE-TRUTH-240` | 17 | READY | 5 | 1 | 0 | 3 | 0 | 0 | 0 |
| 241 | `PLAN-QUARANTINE-STRAIN-TRUTH-241` | 18 | READY | 5 | 9 | 14 | 7 | 0 | 0 | 2 |
| 242 | `PLAN-WEAPON-CONDITION-TRUTH-242` | 18 | READY | 5 | 2 | 4 | 5 | 1 | 0 | 0 |
| 243 | `PLAN-SHELTER-PRISONER-TRUTH-243` | 18 | READY | 5 | 3 | 14 | 20 | 1 | 0 | 1 |
| 244 | `PLAN-SURVIVOR-ROSTER-TRUTH-244` | 18 | READY | 5 | 9 | 23 | 22 | 7 | 2 | 8 |
| 245 | `PLAN-CONTRACTOR-ROSTER-TRUTH-245` | 18 | READY | 5 | 8 | 14 | 22 | 2 | 1 | 1 |
| 246 | `PLAN-GUILT-INSOMNIA-TRUTH-246` | 18 | READY | 5 | 1 | 0 | 2 | 0 | 0 | 0 |
| 247 | `PLAN-QUEST-RUNTIME-TRUTH-247` | 18 | READY | 5 | 2 | 13 | 22 | 4 | 1 | 1 |
| 248 | `PLAN-TRADE-TELL-TRUTH-248` | 18 | READY | 5 | 10 | 18 | 22 | 2 | 0 | 6 |
| 249 | `PLAN-CASCADE-COORDINATOR-TRUTH-249` | 18 | READY | 5 | 1 | 2 | 1 | 0 | 0 | 0 |
| 250 | `PLAN-UV-CORONA-DETECTION-TRUTH-250` | 18 | READY | 5 | 1 | 1 | 1 | 0 | 0 | 1 |
| 251 | `PLAN-CIPHER-CHAIN-TRUTH-251` | 18 | READY | 5 | 2 | 23 | 22 | 10 | 1 | 9 |
| 252 | `PLAN-SUCCESSION-LEGACY-TRUTH-252` | 18 | READY | 5 | 7 | 13 | 17 | 2 | 0 | 0 |
| 253 | `PLAN-VOLUNTARY-REGISTER-TRUTH-253` | 18 | READY | 5 | 2 | 13 | 22 | 2 | 2 | 1 |
| 254 | `PLAN-MUSTER-FACTIONS-TRUTH-254` | 18 | READY | 5 | 1 | 12 | 21 | 1 | 0 | 1 |
| 255 | `PLAN-AUDIO-CONDITION-TRUTH-255` | 18 | READY | 5 | 7 | 13 | 21 | 3 | 0 | 1 |
| 256 | `PLAN-SKY-ARMOR-TRUTH-256` | 18 | READY | 5 | 1 | 0 | 3 | 0 | 0 | 0 |
| 257 | `PLAN-MATERIAL-SHIELDING-TRUTH-257` | 18 | READY | 5 | 1 | 13 | 19 | 1 | 0 | 1 |
| 258 | `PLAN-RADIO-RECORDING-TRUTH-258` | 18 | READY | 5 | 2 | 26 | 22 | 9 | 1 | 16 |
| 259 | `PLAN-CAMPAIGN-EPILOGUE-TRUTH-259` | 18 | READY | 5 | 1 | 7 | 4 | 1 | 0 | 0 |
| 260 | `PLAN-GEOTHERMAL-AQUIFER-TRUTH-260` | 18 | READY | 5 | 5 | 12 | 10 | 0 | 0 | 0 |
| 261 | `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 19 | READY | 5 | 25 | 23 | 22 | 5 | 0 | 12 |
| 262 | `PLAN-CORE-ROOT-FAMILY-TRUTH-262` | 19 | READY | 5 | 32 | 19 | 22 | 4 | 1 | 5 |
| 263 | `PLAN-MEDICAL-FAMILY-TRUTH-263` | 19 | READY | 5 | 23 | 22 | 22 | 4 | 1 | 13 |
| 264 | `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 19 | READY | 5 | 28 | 17 | 22 | 9 | 0 | 4 |
| 265 | `PLAN-SHELTER-FAMILY-TRUTH-265` | 19 | READY | 4 | 16 | 14 | 22 | 4 | 0 | 2 |
| 266 | `PLAN-RADIO-FAMILY-TRUTH-266` | 19 | READY | 4 | 12 | 20 | 22 | 5 | 1 | 3 |
| 267 | `PLAN-WORLD-FAMILY-TRUTH-267` | 19 | READY | 4 | 3 | 16 | 22 | 2 | 0 | 4 |
| 268 | `PLAN-FACTIONS-STATE-FAMILY-TRUTH-268` | 19 | READY | 4 | 14 | 21 | 22 | 4 | 1 | 2 |
| 269 | `PLAN-EXPEDITION-FAMILY-TRUTH-269` | 19 | READY | 5 | 14 | 21 | 22 | 5 | 0 | 9 |
| 270 | `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 19 | READY | 4 | 17 | 24 | 22 | 5 | 1 | 9 |
| 271 | `PLAN-INVENTORY-FAMILY-TRUTH-271` | 19 | READY | 5 | 19 | 12 | 22 | 2 | 0 | 0 |
| 272 | `PLAN-CAMPAIGN-FAMILY-TRUTH-272` | 19 | READY | 5 | 7 | 15 | 16 | 1 | 0 | 3 |
| 273 | `PLAN-COMBAT-FAMILY-TRUTH-273` | 19 | READY | 5 | 10 | 17 | 22 | 2 | 0 | 3 |
| 274 | `PLAN-CONTENT-ACCEPTANCE-FAMILY-TRUTH-274` | 19 | READY | 4 | 8 | 13 | 12 | 0 | 0 | 1 |
| 275 | `PLAN-MUSTER-FAMILY-TRUTH-275` | 19 | READY | 5 | 5 | 16 | 22 | 1 | 0 | 1 |
| 276 | `PLAN-MORALCHOICE-LOADER-FAMILY-TRUTH-276` | 19 | READY | 4 | 2 | 23 | 22 | 1 | 0 | 1 |
| 277 | `PLAN-UI-CONTRACT-FAMILY-TRUTH-277` | 19 | READY | 5 | 4 | 14 | 22 | 2 | 0 | 0 |
| 278 | `PLAN-FOUNDRY-FAMILY-TRUTH-278` | 19 | READY | 5 | 7 | 16 | 20 | 2 | 0 | 4 |
| 279 | `PLAN-PERF-HARNESS-FAMILY-TRUTH-279` | 19 | READY | 5 | 1 | 12 | 12 | 2 | 0 | 0 |
| 280 | `PLAN-TRIO-FAMILY-TRUTH-280` | 19 | READY | 4 | 18 | 15 | 22 | 2 | 0 | 3 |

---

## 4. Residual gaps — re-verified (2026-09-21, Wave 20)

The finalisation pass reported three gaps. Re-verification showed **one is
real**; the other two were false negatives produced by the readiness detector's
own format rules.

| Reported gap | Reported | Re-verified truth | Owner |
|---|---:|---|---|
| No package IDs | 6 | **3 real** — `PLAN-ORPHAN-SEAL-01`, `PLAN-INTEGRATION-KIT-02`, `PLAN-UNBLOCK-03` (prose-organised). The other three use the legal letter+digit format (`### C4-1`, `### B5-1`, `### F6-1`). | `PLAN-READINESS-PACKAGE-IDS-281` |
| No verification command | 5 | **0 real** — all five cite real commands from families the detector did not know (`python3 scripts/ci/*.py --check`, `bash scripts/ci/*.sh --strict`). | `PLAN-READINESS-VERIFICATION-CONTRACT-282` |
| Wave line not `**Wave N` form | 20 | **0 real** — 19 use the legal `**Wave:** N (date) · **Kind:**` form; 1 is a Wave 1 header variant. | `PLAN-READINESS-HEADER-NORMALISATION-283` |
| Detector blindness (systemic) | — | Four false-negative classes found and fixed by hand; no regression test existed. | `PLAN-READINESS-AUDITOR-284` |

**Net residual work:** author package IDs for three plans (281). Everything else
is a contract + detector correction plus the auditor that prevents recurrence.

## 5. Honesty notes

- Readiness is **structural**, computed from each plan's own text. It is not a
  correctness proof and not a claim of foreman approval.
- Coupling, host, catalog, region, ladder and RNG counts come from identifier
  and token joins against the live repository (see `EVIDENCE.md`). Token joins
  can miss synonyms; every plan's §24 says so where it matters.
- Four layer corrections are logged in `EVIDENCE.md` (plans 245, 253, 74, 115,
  and the §14/§15/§18 registry-join fixes). Corrected values are the ones shown
  here.
- Production code was never modified while producing this programme: 473
  pre-existing dirty files preserved.
