# ASHFALL Master Wave Ledger

**Canonical source:** Plan 29 / C1[7] "One Truth"\
**Authority level:** Authoritative historical and active index of development waves, executed packages, and status across ASHFALL.

---

## 1. Wave Status Taxonomy

- **`executed`** — All planned packages completed, acceptance verified against current source, and CI gates green.
- **`in-flight`** — Active package claimed in `WORKTREE_OWNERSHIP.md` with implementation underway.
- **`proposed`** — Ranked candidate package ready for authorization once prerequisites and claims clear.
- **`superseded`** — Wave scope replaced by newer architectural decision or authority.
- **`historical`** — Archived pre-foreman development tranches.

---

## 2. Master Wave Table

| Wave ID | Title | Date | Status | Primary Packages | Completion Gates | Notes |
|---|---|---|---|---|---|---|
| **Wave 1** | Distress Signal Stage Contract | 2026-09-13 | `executed` | DistressStageResolver, outcome hints, stage integrity | `DistressStageResolverTests`, `--data-integrity-selftest` | Monotonic stage resolver live; 63 hints across 43 identities. |
| **Wave 2** | Signal Trust & Follow-Up Chaining | 2026-09-13 | `executed` | SignalTrustLedger, DistressFollowUpScheduler, RadioSave V6 | `DistressFollowUpTests`, `RadioSaveMigrationTests` | Delayed consequences live; self-contained payloads prevent graph cycles. |
| **Wave 3** | Agent Rulebook Synchronization | 2026-09-14 | `executed` | `sync-agent-rulebooks.py`, AGENTS.md canonical source | Gate 37 (`agent_rulebooks_sync`) | All 13 client rulebooks synced byte-identically with zero drift budget. |
| **Wave 4** | Documentation Atlas & Architecture Truth | 2026-09-14 | `executed` | `generate-docs-index.py`, `docs/INDEX.md` | Gate 26 (`docs_index_drift`) | 2,450+ markdown documents indexed and verified against source inputs. |
| **Wave 5** | Agent Skills Catalog Integrity | 2026-09-15 | `executed` | `generate-agent-skills-catalog.py`, `AGENT_SKILLS_INDEX.md` | Gate 38 (`agent_skills_catalog_sync`) | Centralized skills discovery and verification across all agent clients. |
| **Wave 6** | Environmental & Shielding Authority | 2026-09-15 | `executed` | Plans 20A, 20B, 20C (WeatherEffectsCatalog, ShelterShieldingModel, WeatherStationSystem) | `WeatherEffectsCatalogTests`, `Plan20BShelterShieldingTests`, `World` suite (439/439) | 22 weather kinds in data; forecast miss attribution in daily briefing. |
| **Wave 7** | Inventory Wear & Food Authority | 2026-09-15 | `executed` | Plans 21, 22 (RecordWear single API, KitchenNutritionSystem, consumable repair recipes) | `Plan21ProtectiveWearTests`, `Plan22OneFoodAuthorityTests`, `Inventory` suite (84/84) | Direct consumption authority; condition degradation with exactly-once failure. |
| **Wave 8** | Survivor Ledger, Black Market & Travel | 2026-09-16 / 2026-09-17 | `executed` | Plan 24 (NeedsModifierStack, DutyHourLedger, Mourning), Plan 211 (BlackMarketSettlement), Plan 190 (Amputation travel multiplier) | `Plan24NeedsModifierStackTests`, `Plan211BlackMarketSettlementTests`, `C2AmputationTravelTests` | 9 needs families unified; immediate canonical trade settlement; avatar placeholder removed. |
| **Wave 9** | Barter Restock, Distress Audio & Balance | 2026-09-17 | `executed` | Plan 147 (Merchant restock priority), SignalTrust retirement, Plan 20A F1–F8 audit, distress audio content | `Plan147RestockPriorityTests`, `DistressAudioCueTests`, Fast CI tier (47/47) | Restock priority live; 4 dead data items registered; radiation balance findings recorded. |
| **Wave 10 (Part 1)** | Corpus Census, Test Fidelity & One Truth | 2026-09-17 | `executed` | Task A1 (Corpus Census, 131 files), Task A2 (Claim Hygiene), Task A3 (Micro-Deferrals), Task B1 (Plan 27 Tests That Mean It), Task B2 (Plan 29 One Truth) | Gate 48 (`coverage-gate.sh`), `verify-capability-claims.py`, Fast CI tier (48/48) | Census DAG mapped; golden save fixtures pinned; CLAIMS.json registry; roadmap governance active. |
| **Wave 10 (Part 2)** | Amputation Equipment & Ranked Queue Heads | 2026-09-17 | `proposed` | C2 equipment restriction schema extension, Plan 31B replay diagnostics, ranked corpus heads | Target unit suites, fast CI tier | Next authorized execution package. |
