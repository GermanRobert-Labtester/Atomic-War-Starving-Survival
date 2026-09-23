# PLAN-COMMITMENTS-OBLIGATIONS-TRUTH-122 — Promises, Favors & Their Consequences

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-FAMILY-DYNASTY-43, PLAN-NARRATIVE-GRAPH-18, PLAN-SAVE-GOVERNANCE-12.
**Non-goals:** no second relationship system, no currency (Plan 96 owns value),
no quest duplication (Plan 18 owns authored chains).

## 1. Outcome
`Commitments/CommitmentSystem.cs` is host-unreachable (Plan 1 Appendix A) and
the `Narrative/` letter-delivery pair (`LetterDeliverySystem`,
`SurvivorLetterDeliverySystem`) is likewise unwired. Relationships themselves
are owned elsewhere (Plan 43 family/dynasty); **obligations** — who promised
what, by when, kept or broken — are the missing record. Today a broken promise
is either forgotten or expressed as an unexplained relationship drop.

| Deliverable | Detail |
|---|---|
| Obligation record | promisor, beneficiary, subject (item/service/visit), due day, state (`open → kept / broken / waived`) |
| Creation paths | dialogue/quest/contract outcomes create obligations via one hook; no panel-created records |
| Consequence rules | each outcome maps to a documented effect on the existing relationship/standing owners — never a local score |
| Reminder surface | due-soon obligations surface through an existing notice channel (no new UI system required) |
| Expiry | due day evaluates on the canonical day boundary; catch-up after load is correct |

## 2. Evidence
- Plan 1 Appendix A/G: `CommitmentSystem` host-unreachable; `LetterDeliverySystem`, `SurvivorLetterDeliverySystem` (Narrative/) also unreachable.
- Plan 43 owns the relationship graph the consequences write to.
- `SaveSectionRegistry`: an existing section family hosts the record (no new section without Plan 87 policy).
- Plan 18 owns authored chains that may create obligations.

## 3. Packages
- **COT-122A** obligation record + state machine.
- **COT-122B** creation hook wiring (quest/contract/dialogue outcomes).
- **COT-122C** consequence table + tests against the relationship owner.
- **COT-122D** due reminder via an existing channel.
- **COT-122E** day-boundary expiry incl. catch-up + save round-trip.

## 4. Acceptance & verification
- Every created obligation ends in exactly one terminal state; no silent drop.
- Consequences are observable in the relationship owner's values (not a parallel counter).
- Save/load mid-obligation preserves due day; catch-up expires correctly.
- `bash scripts/run_test.sh` on the narrative/relationship regions.

## 5. Risks
Second relationship score → consequences write through Plan 43's owner; the test asserts no local counter.
Reminder spam → one notice per state change plus one due-soon notice, bounded.

---

## 6. Expanded census (9 files · 1,593 lines)

Scope: `Assets/Ashfall.Core/Commitments/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · Loader 1 · Support 3 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CommitmentCatalogLoader.cs` | 145 | Loader | — | 0 | 0 | 0 |
| `CommitmentDefinition.cs` | 37 | Support | — | 0 | 0 | 0 |
| `CommitmentReadModel.cs` | 71 | Support | — | 0 | 0 | 0 |
| `CommitmentSystem.cs` | 314 | System | **yes** | 0 | 0 | 4 |
| `LetterDeliverySystem.cs` | 200 | System | **yes** | 0 | 0 | 2 |
| `PersonalLetterCatalog.cs` | 267 | Catalog | — | 0 | 0 | 0 |
| `PersonalLetterProjection.cs` | 154 | Support | — | 0 | 0 | 0 |
| `SurvivorLetterCatalog.cs` | 120 | Catalog | — | 0 | 0 | 0 |
| `SurvivorLetterDeliverySystem.cs` | 285 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `commitments.json` | object[2 keys] |
| `letters_expansion.json` | object[4 keys] |
| `survivor_letters_lost_kin.json` | object[3 keys] |
| `unsent_letters_batch_2.json` | object[3 keys] |

**State surfaces:** `CommitmentSystem.cs`, `LetterDeliverySystem.cs`, `SurvivorLetterDeliverySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Commitments/` (create if absent) |
| Test references | 12 name references across the test tree |
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

Domain files: 9. Other plans referencing their names: **6**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-VERTICAL-CULTURE-04` | 2 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 1 |
| `PLAN-TEXT-PACK-LOCALIZATION-88` | 1 |
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `COT-122A` | no name match — resolve at claim time |
| `COT-122B` | no name match — resolve at claim time |
| `COT-122C` | no name match — resolve at claim time |
| `COT-122D` | no name match — resolve at claim time |
| `COT-122E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 9; intra-domain edges: **5**; isolated files:
**1**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `CommitmentCatalogLoader` | `CommitmentDefinition` |
| `CommitmentSystem` | `CommitmentDefinition` |
| `CommitmentSystem` | `CommitmentReadModel` |
| `PersonalLetterProjection` | `PersonalLetterCatalog` |
| `SurvivorLetterDeliverySystem` | `SurvivorLetterCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `CommitmentDefinition` | 2 |
| `CommitmentReadModel` | 1 |
| `PersonalLetterCatalog` | 1 |
| `SurvivorLetterCatalog` | 1 |
| `CommitmentCatalogLoader` | 0 |
| `CommitmentSystem` | 0 |
| `LetterDeliverySystem` | 0 |
| `PersonalLetterProjection` | 0 |
| `SurvivorLetterDeliverySystem` | 0 |

**Class split:** hub 0 · sink 4 · source 4 · isolated 1.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 9. Host files: **1** · Test files: **6** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.ShelterInfrastructure.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/Campaign/CommitmentSystemTests.cs`, `Ashfall.Core.Tests/Campaign/Plan33_38IntelCalendarIntegrationTests.cs`, `Ashfall.Core.Tests/LetterDeliverySystemTests.cs`, `Ashfall.Core.Tests/NarrativeAndFactionWarIntegrationTests.cs`, `Ashfall.Core.Tests/PersonalLetterCatalogTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/slice_seven_days.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `personal_quests` |
| `survivor_fate` |
| `survivor_mental_health` |
| `survivor_relations` |
| `survivor_social` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--personal-quest-selftest` |
| `--personal-quests-selftest` |
| `--survivor-death-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **8**.

| Event | First declaration |
|---|---|
| `OnEntryRead` | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnSurvivorExposed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnSurvivorFate` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnSurvivorJoined` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnTreatyDeliveryAccepted` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyDeliveryMissed` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/commitments.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/narrative/personal_effects_inventory_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_letters_lost_kin.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_profiles_expansion.json` |
| `Assets/StreamingAssets/Data/personal_belongings.json` |
| `Assets/StreamingAssets/Data/personal_quests.json` |
| `Assets/StreamingAssets/Data/starting_survivor_cohorts.json` |
| `Assets/StreamingAssets/Data/survivor_life_stages.json` |
| `Assets/StreamingAssets/Data/survivor_roles.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **25**
(6 of them panels/HUD).

| Host file |
|---|
| `src/Host/FactionIconLoader.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/PersonalQuestHostSession.cs` |
| `src/Host/PersonalQuestSaveStore.cs` |
| `src/Host/PersonalQuestSelfTest.cs` |
| `src/Host/SurvivorDeathLegacyHostSession.cs` |
| `src/Host/SurvivorDeathLegacySaveStore.cs` |
| `src/Host/SurvivorDeathLegacySelfTest.cs` |
| `src/Host/SurvivorFateSaveStore.cs` |
| `src/Host/SurvivorMentalHealthSaveStore.cs` |
| `src/Host/SurvivorRelationsHostSession.cs` |
| `src/Host/SurvivorRelationsSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **5**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `personal_quests` | no |
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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **7**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 1, OPTIONAL 2, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `narrative/personal_effects_inventory_batch_2.json` | CODEX_ONLY |
| `narrative/survivor_letters_lost_kin.json` | CODEX_ONLY |
| `narrative/survivor_profiles_expansion.json` | CODEX_ONLY |
| `personal_quests.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 5 (laddered 0) · RNG streams 0 · host files 13 · catalogs 19 · test regions 0 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-COMMITMENTS-OBLIGATIONS-TRUTH-122
wave: 10
status: PROPOSED — foreman claim required
packages: COT-122A, COT-122B, COT-122C, COT-122D, COT-122E
claim paths:
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - src/Host/LoaderWiringSelfTest.cs  # §19 candidate host surface
  - src/Host/PersonalQuestHostSession.cs  # §19 candidate host surface
  - src/Host/PersonalQuestSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/commitments.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --personal-quest-selftest
dependencies:
  - coordinate: 6 other plan(s) name these artifacts (§12)
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
