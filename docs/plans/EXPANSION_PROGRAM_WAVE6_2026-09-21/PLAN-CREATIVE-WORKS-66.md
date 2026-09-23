# PLAN-CREATIVE-WORKS-66 — Art, Music, Theatre, Literature & Exhibitions

**Wave 6 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-VERTICAL-CULTURE-04, PLAN-RECREATION-MORALE-50.
**Non-goals:** no real artworks, songs, or texts; all works are generated from
authored fragments and survivor traits; no second culture authority.

## Outcome
Creative culture exists as fragments: `Culture/CultureCreationSystem.cs`
(orphan), `DocumentationSystem` (orphan), `ProceduralEulogyEngine` (orphan),
`VinylMoraleSystem`, `CassettePlaybackSystem`, `PublicBroadsheetPressEngine`,
`ShelterMuseumSystem`, `TimeCapsuleSystem`, and the journal/archive stack. This
plan makes **making things** a survivor activity with identity, meaning, and
legacy.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Visual art | `CultureCreationSystem` | paint, sculpt, carve | works with traits, morale, display value |
| Music | cassette/vinyl + composition | compose, perform, record | performances, records, mood |
| Theatre | social venues | stage a play | bonds, satire (faction reactions) |
| Literature | `DocumentationSystem`, journal | write, compile, print | manuals, memoirs, history |
| Exhibitions | `ShelterMuseumSystem` | curate, open to visitors | reputation, morale, trade interest |
| Meaning | `ProceduralEulogyEngine`, memorial | mark loss | grief work, archive entries |
| Legacy | `TimeCapsuleSystem`, archive | seal, bequeath | NG+ context, epilogue callbacks |

## Evidence
- Core: `Culture/` (3 files), `Print/PublicBroadsheetPressEngine.cs`, `Audio/CassettePlaybackSystem.cs`, `Journal/ProceduralEulogyEngine.cs`, `ShelterMuseumSystem`, `TimeCapsuleSystem`.
- Data: `shelter_celebrations.json`, `verdict_radio`/`radio_programs` for formats, `journal_voice_prose.json` (33 keys, 7 personalities), `library_manuals.json`.
- Sealed prior: Plan 190 lore (mapped), Plan 178 creation-to-vault (5/5), Plan 162 archive (8/8), Plan 212 capsules (5/5), Plan 95 journal voice (7/7).
- Contracts: one journal/archive; works are items with provenance; no copied text.

## Packages
- **CW-66A** art works: a creation action produces an item with quality + provenance; display in the museum/quarters gives bounded morale.
- **CW-66B** music: composition from style fragments; performance events; recordings as items playable in venues.
- **CW-66C** theatre: scripted small plays with roles; morale/bond effects; faction satire risks standing.
- **CW-66D** literature: manuals improve study; memoirs/records feed the archive; printing costs paper.
- **CW-66E** exhibitions: curated sets give reputation and visitor interest (ties Plan 30/68).
- **CW-66F** meaning: eulogies/memorial works are authored from survivor facts; archive links.
- **CW-66G** content volumes: +12 art styles, +10 songs, +8 plays, +10 texts, +6 exhibition themes; all original.

## Acceptance & verification
- Every work is an item with provenance and a display/consumer path; no orphan outputs; determinism.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Culture/`; `--culture-selftest`; `--journal-selftest`.

## Risks
Prose generation quality → authored fragment pools + personality variants; review samples.

---

## 6. Expanded census (3 files · 1,149 lines)

Scope: `Assets/Ashfall.Core/Culture/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CultureCreationSystem.cs` | 287 | System | **yes** | 0 | 1 | 2 |
| `ShelterFestivalEngine.cs` | 323 | System | **yes** | 0 | 0 | 4 |
| `ShelterMuseumSystem.cs` | 539 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 1 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `agriculture_items.json` | array[2] |
| `muster_faction_culture.json` | array[25] |
| `museum_collection_templates.json` | object[3 keys] |
| `apiculture_red_light_audits.json` | array[8] |

**State surfaces:** `CultureCreationSystem.cs`, `ShelterFestivalEngine.cs`, `ShelterMuseumSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Culture/` |
| Test references | 4 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

Domain files: 3. Other plans referencing their names: **6**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-VERTICAL-CULTURE-04` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 3 |
| `PLAN-SHELTER-DECOR-TRUTH-225` | 2 |
| `PLAN-SILENT-FAILURE-35` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CW-66A` | `CultureCreationSystem.cs`, `ShelterMuseumSystem.cs` |
| `CW-66B` | no name match — resolve at claim time |
| `CW-66C` | no name match — resolve at claim time |
| `CW-66D` | no name match — resolve at claim time |
| `CW-66E` | no name match — resolve at claim time |
| `CW-66F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **0** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs`, `Ashfall.Core.Tests/Culture/Plan218MuseumIntegrationTests.cs`, `Ashfall.Core.Tests/Culture/ShelterFestivalEngineTests.cs`, `Ashfall.Core.Tests/Survivors/Plan178ArtCultureIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **15** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `expanded_shelter` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |
| `shelter_noise` |
| `shelter_prisoners` |
| `shelter_reputation` |
| `shelter_schedule` |
| `shelter_security` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **12** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |
| `--shelter-noise-selftest` |
| `--shelter-operations-selftest` |
| `--shelter-ops-selftest` |
| `--shelter-physics-selftest` |
| `--shelter-reputation-selftest` |
| `--shelter-security-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/museum_collection_templates.json` |
| `Assets/StreamingAssets/Data/muster_faction_culture.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |
| `Assets/StreamingAssets/Data/shelter_celebrations.json` |
| `Assets/StreamingAssets/Data/shelter_components.json` |
| `Assets/StreamingAssets/Data/shelter_construction.json` |
| `Assets/StreamingAssets/Data/shelter_governance_blocs.json` |
| `Assets/StreamingAssets/Data/shelter_insulation_catalog.json` |
| `Assets/StreamingAssets/Data/shelter_machine_identities.json` |
| `Assets/StreamingAssets/Data/shelter_origins.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (94 files, 794 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Culture` | 7 | 40 |
| `Shelter` | 87 | 754 |

**Verdict:** 794 cases sit under matching regions — run those first (`Culture`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **49**
(12 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/ShelterAssignmentHostSession.cs` |
| `src/Host/ShelterAtmosphereHostSession.cs` |
| `src/Host/ShelterAtmosphereSaveStore.cs` |
| `src/Host/ShelterAtmosphereSelfTest.cs` |
| `src/Host/ShelterBarterSaveStore.cs` |
| `src/Host/ShelterDecorHostSession.cs` |
| `src/Host/ShelterDecorSaveStore.cs` |
| `src/Host/ShelterDecorSelfTest.cs` |
| `src/Host/ShelterEspionageSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **15**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `expanded_shelter` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |
| `shelter_fire` | no |
| `shelter_noise` | no |
| `shelter_prisoners` | no |
| `shelter_reputation` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **8**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 2, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `muster_faction_culture.json` | UNRESOLVED |
| `narrative/shelter_notices_expansion.json` | CODEX_ONLY |
| `narrative/shelter_songs_expansion.json` | CODEX_ONLY |
| `shelter_machine_identities.json` | UNRESOLVED |
| `shelter_room_identities.json` | UNRESOLVED |
| `shelter_rooms.json` | UNRESOLVED |
| `shelter_schedules.json` | GAMEPLAY_CONSUMED |
| `shelter_social_events.json` | GAMEPLAY_CONSUMED |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 15 (laddered 0) · RNG streams 1 · host files 13 · catalogs 20 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CREATIVE-WORKS-66
wave: 6
status: PROPOSED — foreman claim required
packages: CW-66A, CW-66B, CW-66C, CW-66D, CW-66E, CW-66F, CW-66G
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/ShelterAssignmentHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/museum_collection_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/muster_faction_culture.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Culture/
  - godot --headless --path . -- --shelter-actor-physics-selftest
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
