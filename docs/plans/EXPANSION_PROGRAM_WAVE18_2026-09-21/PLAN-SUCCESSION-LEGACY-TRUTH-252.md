# PLAN-SUCCESSION-LEGACY-TRUTH-252 — Inheritance of Roles, Records & Debts

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-GENERATIONAL-MILESTONE-TRUTH-160, PLAN-INSTITUTIONS-TRUTH-141, PLAN-HEIRLOOM-PHANTOM-TRUTH-149, PLAN-COMMITMENTS-OBLIGATIONS-TRUTH-122.
**Non-goals:** no milestone firing (Plan 160), no office ledger mechanics
(Plan 141), no heirloom records (Plan 149), no obligations (Plan 122).

## 1. Outcome
`Legacy/GenerationalSuccessionEngine.cs` (**166 lines**) is reachable and
unaddressed: what passes to the next generation when someone dies or steps
down — roles, records, debts of gratitude, and possessions. Each item has an
owner (Plans 141/149/122/43); the **transfer-on-death orchestration** is
unowned, so bequests either vanish or collide.

| Deliverable | Detail |
|---|---|
| Succession manifest | what a person may leave (roles, holdings, obligations, heirlooms) with the owner per item |
| Ordering | documented resolution order so one death does not race multiple owners |
| Defaults | with no will/next-of-kin, holdings go to a documented default (holdfast pool) — never vanish |
| Records | the succession is recorded (who received what, day) and readable |
| Save truth | pending successions restore; no double transfer on load |

## 2. Evidence
- `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` (166 lines; unaddressed — Wave 18 audit).
- Plan 141's ledger for roles; Plan 149 heirlooms; Plan 122 obligations; Plan 43 relations.
- Plan 160's milestones observe successions.
- Plan 93 verifies item transfers.

## 3. Packages
- **SLT-252A** manifest + owner table.
- **SLT-252B** ordering/race fixture.
- **SLT-252C** default-pool tests (nothing vanishes).
- **SLT-252D** succession record + readability.
- **SLT-252E** save round-trip; no double transfer.

## 4. Acceptance & verification
- Every item in the manifest lands with its owner (or the default pool) exactly once.
- Save/load mid-succession does not double-transfer.
- `bash scripts/run_test.sh` on the legacy/survivors region.

## 5. Risks
Disappearing property → the default pool is a fixture.
Race conditions → fixed resolution order is tested.

---

## 6. Expanded census (4 files · 1,532 lines)

Scope: `Assets/Ashfall.Core/Legacy/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampaignLegacySystem.cs` | 305 | System | — | 0 | 0 | 2 |
| `GenerationalSuccessionEngine.cs` | 166 | System | **yes** | 0 | 0 | 2 |
| `GenerationalSystem.cs` | 456 | System | — | 0 | 0 | 2 |
| `SurvivorDeathLegacySystem.cs` | 605 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `death_legacy_templates.json` | object[2 keys] |
| `legacy_traits.json` | object[2 keys] |

**State surfaces:** `CampaignLegacySystem.cs`, `GenerationalSuccessionEngine.cs`, `GenerationalSystem.cs`, `SurvivorDeathLegacySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Legacy/` |
| Test references | 10 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 4. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-FAMILY-DYNASTY-43` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-MUTATION-HEREDITY-81` | 1 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SLT-252A` | no name match — resolve at claim time |
| `SLT-252B` | no name match — resolve at claim time |
| `SLT-252C` | no name match — resolve at claim time |
| `SLT-252D` | `GenerationalSuccessionEngine.cs` |
| `SLT-252E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **8** · Test files: **10** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Host/ExpansionHostSession.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/SurvivorDeathLegacyHostSession.cs`, `src/Host/SurvivorDeathLegacySelfTest.cs`, `src/Main.Plans178_181.cs` |
| Tests (`Ashfall.Core.Tests/`) | 10 | `Ashfall.Core.Tests/GenerationalLineageExtensionTests.cs`, `Ashfall.Core.Tests/HeirloomSystemTests.cs`, `Ashfall.Core.Tests/Integration/Plans178_181_CampaignContinuityTests.cs`, `Ashfall.Core.Tests/Legacy/Plan140GenerationalLegacyIntegrationTests.cs`, `Ashfall.Core.Tests/StandaloneCoreSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **0**; isolated: **4**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `death_legacy` |
| `survivor_fate` |
| `survivor_mental_health` |
| `survivor_relations` |
| `survivor_social` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **5** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--death-legacy-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--survivor-death-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnSuccessionTriggered` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |
| `OnSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnSurvivorExposed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnSurvivorFate` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnSurvivorJoined` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/legacy_traits.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_letters_lost_kin.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_profiles_expansion.json` |
| `Assets/StreamingAssets/Data/starting_survivor_cohorts.json` |
| `Assets/StreamingAssets/Data/survivor_life_stages.json` |
| `Assets/StreamingAssets/Data/survivor_roles.json` |
| `Assets/StreamingAssets/Data/survivor_voice_lines.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (33 files, 192 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `Legacy` | 1 | 5 |

**Verdict:** 192 cases sit under matching regions — run those first (`Campaign`, `Legacy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **24**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/GenerationalSaveStore.cs` |
| `src/Host/SurvivorDeathLegacyHostSession.cs` |
| `src/Host/SurvivorDeathLegacySaveStore.cs` |
| `src/Host/SurvivorDeathLegacySelfTest.cs` |
| `src/Host/SurvivorFateSaveStore.cs` |
| `src/Host/SurvivorMentalHealthSaveStore.cs` |
| `src/Host/SurvivorRelationsHostSession.cs` |
| `src/Host/SurvivorRelationsSaveStore.cs` |
| `src/Host/SurvivorSocialSaveStore.cs` |
| `src/Main.Campaign.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **7**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |
| `death_legacy` | no |
| `survivor_fate` | no |
| `survivor_mental_health` | no |
| `survivor_relations` | no |
| `survivor_social` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **5**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 1, OPTIONAL 2).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `narrative/survivor_letters_lost_kin.json` | CODEX_ONLY |
| `narrative/survivor_profiles_expansion.json` | CODEX_ONLY |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 7 (laddered 0) · RNG streams 0 · host files 13 · catalogs 17 · test regions 2 · flags 5

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SUCCESSION-LEGACY-TRUTH-252
wave: 18
status: PROPOSED — foreman claim required
packages: SLT-252A, SLT-252B, SLT-252C, SLT-252D, SLT-252E
claim paths:
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - src/Host/GenerationalSaveStore.cs  # §19 candidate host surface
  - src/Host/SurvivorDeathLegacyHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 7 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
