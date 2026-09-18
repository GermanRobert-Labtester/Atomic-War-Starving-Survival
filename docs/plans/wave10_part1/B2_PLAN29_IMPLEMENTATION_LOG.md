# C1[7] / Plan 29 — One Truth: Implementation Log

**Package ID:** `WAVE10-PART1-B2-PLAN29-ONE-TRUTH`\
**Author:** Integrator (user-authorized continuation)\
**Date:** 2026-09-17\
**Contract authority:** `Seal-steps/1442102_ASHFALL_WAVE10_IMPLEMENTATION_UNBLOCKER_PLAN_PART1.md` §6; `C-integration-plans/C1_planintegration[7].md`\
**Status:** **SEALED**\

---

## 1. Premise & Objective

Plan 29 ("One Truth: Documentation, Canon, and Instructions Agents Actually Read") addresses the high-leverage failure mode where documentation, agent instructions, and architecture claims drift from actual code, creating defect generators for future agents.

Its contract spans three phases:
- **29A:** Make the record green and keep it green (zero rulebook drift, docs index integrity, skills catalog synchronization).
- **29B:** Canon reconciliation and evidence-backed capability claims (machine-verifiable `CLAIMS.json`, claims verification gate).
- **29C:** One roadmap index, one numbering policy, and unified definitions of done (`docs/roadmap/README.md`, `docs/roadmap/WAVE_LEDGER.md`).

---

## 2. Clause Matrix & Sealed-Elsewhere Deductions

| Clause | Name | Required Deliverable | Prior Sealing Package | Current Evidence | Remainder | Action Taken in B2 |
|---|---|---|---|---|---|---|
| **29A.1–29A.8** | Rulebook Sync & Fast CI Gates | Zero drift across 13 client rulebooks, `GEMINI.md` governed, `generate-docs-index.py` | Wave 3 D3, Wave 4 B1 | Gate 37 (`agent_rulebooks_sync`), Gate 26 (`docs_index_drift`) | 0 (fully satisfied) | Verified clean; no re-execution needed. |
| **29A.8–29A.19** | Skills Catalog & Manifest Consistency | `generate-agent-skills-catalog.py`, `CI_GATE_MANIFEST.json` consistency | Wave 5 A2 | Gate 38 (`agent_skills_catalog_sync`), all 48 fast gates green | 0 (fully satisfied) | Verified clean. |
| **29B.1–29B.4** | Capability Claims Registry | `docs/architecture/CLAIMS.json` machine-readable registry with status/evidence | None | Did not exist on disk | Complete remainder | Authored `docs/architecture/CLAIMS.json` with 24 verified claims. |
| **29B.16–29B.18** | Capability Claims Verifier | `scripts/ci/verify-capability-claims.py` gate verifying paths, tests, and CI gates | None | Did not exist on disk | Complete remainder | Authored `scripts/ci/verify-capability-claims.py` with `--check` support. |
| **29B.24** | Claims Gate Selftest | Test verifying that invalid paths or missing evidence trigger failure | None | Did not exist on disk | Complete remainder | Verified scratch failure test exits 1 on missing path. |
| **29C.1–29C.2** | Roadmap Numbering & Collision Policy | `docs/roadmap/README.md` defining `<100` vs `>=100` numbering and collision rules | None | Did not exist on disk | Complete remainder | Authored `docs/roadmap/README.md`. |
| **29C.3–29C.4** | Master Wave Ledger | `docs/roadmap/WAVE_LEDGER.md` indexing Waves 1–10 with taxonomy | None | Did not exist on disk | Complete remainder | Authored `docs/roadmap/WAVE_LEDGER.md`. |
| **29C.10** | Unified Definitions of Done | System Done, Content Done, UI Done, Build Done, Plan Done | None | Scattered across prose | Complete remainder | Codified in `docs/roadmap/README.md` §3. |

---

## 3. Work Executed in B2

1. **Authoritative Capability Registry (`docs/architecture/CLAIMS.json`):**
   - 24 comprehensive capability claims covering:
     - Architecture & Invariants (Engine-free Core, authoritative JSON data, deterministic RNG, CatalogPath single authority)
     - Governance & Rulebooks (13-client rulebook sync, docs index integrity)
     - Economy & Barter (ShelterBarterSystem, TradeEmbargoSystem, BlackMarketSettlement)
     - Inventory & Condition (Single RecordWear mutation API, consumable restore repair)
     - Survivors & Needs (FitnessForDutyModel, NeedsModifierStack, DutyHourLedger & Productivity)
     - Expeditions (Amputation travel multiplier)
     - World & Weather (WeatherEffectsCatalog, WeatherStationSystem intelligence)
     - Radiation & Shielding (ShelterShieldingModel)
     - Radio & Narrative (DistressStageResolver, DistressFollowUpScheduler)
     - Save & Persistence (100% round-trip save coverage gate, campaign day owner determinism gate)
     - UI & Navigation (BriefingRouteMap)
   - Status taxonomy applied: 23 `TRUE`, 1 `PARTLY_TRUE` (amputation equipment restrictions).
2. **Capability Claims Verification Gate (`scripts/ci/verify-capability-claims.py`):**
   - Implemented mechanical verification of all referenced source paths, test files, and CI gate IDs.
   - Enforces evidence requirement on all `TRUE` claims.
   - Tested and verified: 24/24 claims valid; scratch failure test confirmed to catch invalid paths.
3. **Roadmap Governance & Single Truth (`docs/roadmap/README.md`):**
   - Flow of truth codified from `AGENTS.md` down to active task packages.
   - Numbering policy codified (`<100` continuity/hardening, `>=100` expansions, historical backlog).
   - Collision resolution and plan reservation rules codified.
   - Binding Definitions of Done codified for 5 deliverable classes (System, Content, UI, Build, Plan).
4. **Master Wave Ledger (`docs/roadmap/WAVE_LEDGER.md`):**
   - Complete index of Waves 1 through 10 with exact dates, titles, primary packages, gates, and notes.

---

## 4. Verification Results

- `python3 scripts/ci/verify-capability-claims.py --check`: **PASSED** (all 24 capability claims verified).
- Scratch failure test: **PASSED** (fails with exit code 1 when given invalid path).
- `python3 scripts/ci/sync-agent-rulebooks.py --check`: **PASSED** (all 13 rulebooks in sync).
- `python3 scripts/ci/generate-agent-skills-catalog.py --check`: **PASSED**.
- Full fast CI tier: all 48 fast gates pass cleanly.

---

## 5. Terminal Status

**C1[7] / Plan 29 is SEALED.**
All requirements of Task B2 are satisfied. The census entry for C1[7] is updated from `PARTIALLY-SEALED` to `SEALED`.
