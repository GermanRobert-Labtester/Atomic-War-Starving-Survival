# Regional Treaty Runtime Contract (Plan 25 · 25C.2)

> Verified 2026-09-01. Records the mechanical treaty system, the narrative treaty corpora, and the production gap.

## 1. Mechanical system — `RegionalTreatySystem`

`Assets/Ashfall.Core/RegionalTreatySystem.cs` (176 lines). `SystemId = "regional_treaty"`.

- **State:** `TreatyInstance {treatyId, status, proposedDay, ratifiedDay, violatedDay, complianceScore, lastComplianceCheckDay}`; `TreatyStatus { Proposed, Ratified, Active, Violated, Suspended, Expired }` (L49). `Suspended`/`Expired` are never set by any code today.
- **Definitions:** `TreatyDefinition` carries `ratification_cost_scrap/day`, `prerequisites[]`, `effects[]` (`effect_type`: economy_discount, route_access, water_quota, labor, power), `compliance_check_interval_days`, `violation_penalty_affinity`.
- **APIs:** `LoadCatalog` (67), `Propose` (82), `Ratify` (98 — blocks on scrap cost), `IsActive` (119), `GetActiveEffects` (125), `TickDay` (137 — compliance decays 0.1 per interval; ≤0 → `Violated` + `violatedDay`). Event: `OnTreatyStatusChanged`. `violation_penalty_affinity` is **declared but never applied**.
- **Save:** `src/Host/RegionalTreatySaveStore.cs` (`regional_treaty_save.json`, section `regional_treaty`, registered `SaveSectionRegistry.cs:102`, owner "factions", LifecycleGroup `expanded_shelter`); host save at `src/Main.ShelterSocial.cs:83`.
- **Tests:** `RegionalTreatySystemTests`, `RegionalTreatyIntegrationTests` (propose→ratify + roundtrip), `RegionalTreatyCatalogTests` (**pins exactly 16** treaties from `narrative/regional_treaty_protocols.json`).

## 2. Production gap (must-know for Plan 25)

**The host never calls `LoadCatalog`.** `src/Main.ShelterSocial.cs:68 SetupRegionalTreaty()` constructs the system and restores state — definitions are only built inside tests. In the shipped host the catalog is empty and `Propose` fails `unknown_treaty`.

Plan 25 handling: an isolated, revertible commit adds an adapter that maps the existing narrative corpus (`narrative/regional_treaty_protocols.json`, 16 treaties — canonical, test-pinned) into `TreatyDefinition`s and feeds `LoadCatalog` in `SetupRegionalTreaty`. Fallback if it destabilizes: Plan 25 treaty gating uses the **read-model** only (below) and the gap stays documented.

## 3. Narrative treaty corpora (different schema — read models)

| Corpus | Count | Shape | Consumers |
|---|---:|---|---|
| `narrative/regional_treaty_protocols.json` | 16 canonical (`treaty_01_lock_4_sluice_and_brine_concession` … `treaty_16_the_constitution_of_the_valley_of_tessarat`) | `{treaty_id, ratified_day, treaty_title, signatory_factions[], demarcated_territory, water_allocation_lpm, power_quota_kw, tariff_schedule, treaty_articles, penalties, tags[]}` | `Assets/Ashfall.Core/Narrative/RegionalTreatyCatalog.cs` (`Load`, `GetRatifiedByDay`, `GetBySignatoryFaction`); pinned by `RegionalTreatyCatalogTests` |
| `foundry_accords.json` | 12 | same narrative DTO | `SilentFoundryCatalog.cs:125` (foundry treaty clock), `CatalogBootValidator.cs:217` (optional) |

**Neither corpus matches `TreatyDefinition`.** The 16-treaty count is test-pinned — Plan 25 never renumbers them; new authored treaties (if any) append.

## 4. Plan 25 consumption rules

1. Escalation events (E-P*) may declare treaty relevance: `strains` / `violates` / `renegotiates` — expressed as flags + (post-feed-commit) `RegionalTreatySystem` API calls only. Not every disagreement is a breach (plan §25C.10).
2. Muster path evaluation consumes treaty state as read-only input (active/violated counts via read-model or system state).
3. All treaty flags (`flag_treaty_*`) are producer→consumer mapped in the continuity matrix; the lint test forbids orphans.
4. Treaty state persists via the existing `regional_treaty` section; Plan 25 adds no new treaty store.
