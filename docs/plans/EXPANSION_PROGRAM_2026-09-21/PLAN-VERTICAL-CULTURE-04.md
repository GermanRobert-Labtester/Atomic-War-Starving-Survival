# PLAN-VERTICAL-CULTURE-04 — New Mechanics Vertical: Culture, Memory & Identity

**Program:** ASHFALL Expansion & Integration Program (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Builder per package; Integrator for the culture
day owner, the journal/audio seams, and any new save section.
**Depends on:** PLAN-ORPHAN-SEAL-01 Wave 7 for the base wiring of these
authorities; this plan adds the player-facing mechanics and content that make
them worth wiring.
**Expanded appendix:** [`PLAN-VERTICAL-CULTURE-04_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-VERTICAL-CULTURE-04_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's culture/voice/narrative
systems, each mapped to its parent-plan mechanic row.
**Non-goals:** no machinery for global history simulation; no second journal;
no parallel archive store (`ShelterArchiveSystem` and `MemorialSystem` remain
canonical); no copied real-world holidays, texts, or iconography (tone rule:
fictional, restrained, human).

---

## 1. Outcome

Turn the already-delivered but unwired culture authorities into a played
vertical: a bunker that keeps a calendar, keeps its dead, keeps its stories,
and keeps an identity the player can see. Nine mechanics, each bound to an
existing authority, each with one observable consequence per day and a
save/reload-clean lifecycle.

| # | Mechanic | Authority (existing) | Player action | Observable outcome |
|---|---|---|---|---|
| 1 | Bunker Almanac | `SeasonalCelebrationSystem`, `SpiritualRitualCalendarEngine`, `ShelterFestivalEngine` | schedule / skip a holiday or observance | morale swing, tradition streak, resource cost, journal entry |
| 2 | Memory Room | `ShelterMuseumSystem`, `HeirloomSystem`, `TrophySystem` | place a relic/keepsake/trophy on exhibit; appoint a curator | daily visitor morale; bereavement comfort; exhibit condition |
| 3 | The Walls | `ProceduralEulogyEngine`, `MemorialSystem`, `ShelterArchiveSystem` | commission an eulogy; etch a name | grief dispersion, archive entry, chronicle callback |
| 4 | The Broadsheet | `PublicBroadsheetPressEngine` | write/publish/withhold a weekly issue | shelter morale, faction perception, rumor accuracy |
| 5 | Voices & Cassettes | `SurvivorVoiceSystem`, `VoiceLineSelectionEngine`, `VoiceLineDispatchCoordinator`, `CassettePlaybackSystem` | assign a broadcast/recording slot | bark delivery on life triggers; ambient audio bed; accessible captions |
| 6 | Post | `LetterDeliverySystem`, `SurvivorLetterDeliverySystem` | send a letter with a caravan | reply after travel days; relationship/standing delta |
| 7 | Confessionals | `ConfessionSecretSystem`, `NpcMemorySystem` | hear a secret; keep or reveal it | trust, guilt, exposure consequences |
| 8 | Shelter Identity | `ShelterIdentitySystem` | name the shelter, choose a mark and a saying | identity shown on every panel header; morale tie-break |
| 9 | Legacy Ledger | `CampaignLegacySystem`, `CrossRunProfileStore`, `TimeCapsuleSystem` | seal a time capsule; end a campaign | New Game+ starting context; epilogue callback |

Cross-program note: DEC-32 (origin modifier), DEC-33 (cross-run profile),
DEC-31 (completion summary) and Plan 32/200/212 families are the signed
foundations; this plan is the gameplay surface, not a new decision.

---

## 2. Premise evidence

- The nine authorities are host-unreachable today
  (`docs/plans/EXPANSION_PROGRAM_2026-09-21/PLAN-ORPHAN-SEAL-01.md` §2);
  `CultureCreationSystem`, `ShelterFestivalEngine`, `ShelterMuseumSystem`
  have 3+ tests each, none host-referenced.
- The canonical anchors already exist and must be extended, not duplicated:
  `MemorialSystem`, `JournalSystem`, `ChroniclePanel` (wired 2026-09-20),
  `ShelterArchiveSystem` (Plan 162), `TimeCapsuleSystem` (Plan 212),
  `ShelterAtmosphereSystem` (Plan 220), `Plan 169 AudioAccessibilityCoordinator`.
- Audio binding pattern is proven: `AudioManager.RefreshDomainBindings` +
  `IShelterOperationsAudioProvider` (BUG-WIRING-REPAIR-HARDENING 2026-09-20).
- Content pipeline is proven: 703 data JSON files, `CatalogIntegrityValidator`,
  `--data-integrity-selftest`, `--content-utilization-selftest`.
- The save pattern is proven: one `*HostSession` per subsystem,
  `SaveStoreHub.FromCodec` + `SchemaVersionedEnvelope`, matrix regeneration.

---

## 3. Authority and seam map

| Concern | Extend (do not create) |
|---|---|
| Culture day ticks | a new `culture` day owner in `src/Main.CampaignOwners.cs` (ordering after `spiritual`, before `mental_health`) |
| Calendar truth | `SeasonalCelebrationSystem` (phase catalog already authored) + `SpiritualRitualCalendarEngine` |
| Journal/feedback | `JournalSystem.TryAddRawEntry` with stable dedup keys (never free-text duplication) |
| Memorial/archive | `MemorialSystem`, `ShelterArchiveSystem`, `ProceduralEulogyEngine` projection |
| Audio | `AudioManager` event bridge + Plan 169 accessibility coordinator (visual notification for every critical cue) |
| Radio/voice | `RadioHostSession` + `SurvivorVoiceSystem` bark dispatch |
| Save | one new `culture` section **only if** calendar/exhibit state cannot ride `shelter_atmosphere`/`memorial`/`journal`; integrator decides in the claim |
| Panels | extend `Main.PlayerSurfaces` descriptors; no free-floating panels; every route behind `PanelRouteGateTests` |
| RNG | new snake_case streams: `culture_festival`, `culture_exhibit`, `culture_post`, `culture_confession`; all via `CampaignRngManager.Fork` |

---

## 4. Packages

### C4-1 — Culture day owner and calendar loop (foundation)
- **Outcome:** one owner ticks the calendar, resolves observance windows,
  applies resource costs, and emits day events; `SeasonalCelebrationSystem`
  and `SpiritualRitualCalendarEngine` are consumed, not re-implemented.
- **Paths:** `src/Main.CampaignOwners.cs`, `src/Host/CultureHostSession.cs`
  (new), `Assets/Ashfall.Core/Events/SeasonalCelebrationSystem.cs` (bind only),
  `Assets/StreamingAssets/Data/shelter_celebrations.json` (extend),
  focused tests.
- **Acceptance:** deterministic same-seed celebration set across a 40-day run;
  skip penalties reversible; no new save section beyond the single `culture`
  section if approved; reload mid-streak preserves streak.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Events/`;
  `godot --headless --path . -- --culture-selftest` (new probe).

### C4-2 — Festival scheduling and tradition streaks
- **Outcome:** player schedules one festival per window with scale
  small/medium/large; participant scaling from living roster; tradition streak
  multipliers; authored observances for observed losses.
- **Content:** +15 authored celebration rows (fictional, wasteland-native),
  +10 observance rows, resource bands.
- **Acceptance:** morale delta is bounded and explainable in the panel;
  starvation/rationing gate blocks feast unless the player overrides;
  celebration streaks persist.
- **Verify:** focused festival tests + `--content-utilization-selftest`.

### C4-3 — Memory Room (museum + exhibits + curator)
- **Outcome:** assign relics/keepsakes/trophies to exhibit slots; a curator
  survivor (existing role/skill system) improves exhibit condition; visitors
  gain bounded daily morale; damaged exhibits generate a repair task.
- **Paths:** `ShelterMuseumSystem`, `HeirloomSystem`, `TrophySystem` binds;
  new room tag consumption from `shelter_rooms.json` (existing room), museum
  panel route; exhibit state in the `culture` section.
- **Acceptance:** exhibit morale is bounded and does not stack infinitely;
  item removal returns the item intact; equipment/inventory authority untouched.
- **Verify:** focused museum tests + inventory regression suite.

### C4-4 — The Walls (eulogy + memorial + archive callback)
- **Outcome:** on a death, the player may commission a eulogy (cost, time,
  quality from the speaker's skills); the etched name appears on the memorial
  surface; the archive records a canonical entry with source links.
- **Paths:** `ProceduralEulogyEngine`, `MemorialSystem`,
  `ShelterArchiveSystem` binds; `memorial`/`archive` read-only projection into
  one panel; no new store.
- **Acceptance:** exactly one eulogy per death; grief dispersion scales with
  death quality (0.5 peaceful → 1.25 unattended); reload does not re-roll.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/`.

### C4-5 — The Broadsheet (weekly press)
- **Outcome:** each 7 days the press composes an issue from canonical day
  events, rumors (`RumorSystem`), and market facts; player publishes, edits
  tone, or withholds; consequences route to morale and faction standing.
- **Paths:** `PublicBroadsheetPressEngine` bind; `RumorSystem` read; journal
  entry per issue; panel route; issue history bounded (use `RollingLog<T>`
  from Plan 55 retention policy, max 24 issues).
- **Acceptance:** no invented facts — every line derives from a canonical
  producer; withholding has a cost; determinism across replay.
- **Verify:** focused press tests + rumor suite.

### C4-6 — Voices and cassettes
- **Outcome:** barks fire on life triggers through the voice selector and
  dispatcher; cassettes play as ambient/planned broadcasts; Plan 169
  accessibility delivers captions for critical cues; silence states respected.
- **Paths:** `SurvivorVoiceSystem`, `VoiceLineSelectionEngine`,
  `VoiceLineDispatchCoordinator`, `CassettePlaybackSystem` binds through the
  existing audio bridge; `survivor_voice_lines.json` extended.
- **Acceptance:** cooldowns enforced; no orphan geiger/loop on end; captions
  present for critical cues; no audio bus clipping (calibration doc).
- **Verify:** `godot --headless --path . -- --audio-selftest`;
  `MachineTellAudioSyncTests`.

### C4-7 — Post and letters
- **Outcome:** send a letter with a caravan/settlement contact; delivery after
  authored travel days; possible reply; relationship and standing deltas.
- **Paths:** `LetterDeliverySystem`, `SurvivorLetterDeliverySystem` binds;
  caravan day owner hook; `NpcMemorySystem` remembers the correspondence.
- **Acceptance:** no letter duplication on reload; undeliverable letters
  return with an authored notice; determinism from seeded delivery roll.
- **Verify:** focused narrative tests + caravan suite.

### C4-8 — Confessionals
- **Outcome:** survivors may confess a secret (hidden agenda / moral choice /
  personal quest fallout); player chooses keep/reveal/handle; exposure has
  typed consequences.
- **Paths:** `ConfessionSecretSystem` bind; `HiddenAgendaSystem`,
  `MoralChoice` flags, `NpcMemorySystem` reads only.
- **Acceptance:** secrets never leak outside the chosen branch; reveal routes
  through existing faction/morale owners; no parallel secret store.
- **Verify:** focused hidden-agenda/confession tests.

### C4-9 — Shelter identity and heraldry
- **Outcome:** name, mark, and saying chosen at setup and editable later;
  identity shown in headers, radio sign-on, broadsheet masthead, and epilogue;
  small morale tie-break when identity matches shelter values.
- **Paths:** `ShelterIdentitySystem` bind; `Main.PlayerSurfaces` header
  projection; identity in the `culture` (or existing `campaign`) section.
- **Acceptance:** identity persists; no copyrighted or real-world symbols
  (authored palette/shape set only); UI text length bounded.
- **Verify:** focused identity tests + UI snapshot gate.

### C4-10 — Legacy ledger and time capsule
- **Outcome:** seal a capsule (authored content + mementos) for New Game+; at
  campaign end, legacy traits transfer through the signed DEC-32/33 paths;
  epilogue cites the culture vertical.
- **Paths:** `CampaignLegacySystem`, `CrossRunProfileStore`, `TimeCapsuleSystem`
  binds; `UnifiedEndingResolver` read-only contribution; profile store stays
  user-level (DEC-20/33 boundaries).
- **Acceptance:** no campaign state leaks into profile beyond the signed
  contract; capsule content deterministic; 3-generation trait evolution tested.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Legacy/`.

---

## 5. Content expansion volumes (initial)

| Catalog | Current (baseline) | Target | Rules |
|---|---:|---:|---|
| `shelter_celebrations.json` | 5 holidays / 3 anniversary types / 3 scales | +15 observances | fictional, wasteland-native, no real holidays |
| museum exhibit templates | 0 (system-side) | 30 | relic/keepsake/trophy families, condition bands |
| broadsheet templates | 0 | 40 blocks | derived from canonical event vocabulary only |
| voice lines | `survivor_voice_lines.json` (existing) | +60 lines / 8 registers | tone: restrained, human, no slogans |
| letter templates | 0 | 20 | sender/recipient archetypes, caravan delay bands |
| confession/secret archetypes | existing hidden-agenda set | +8 | no real-world group references |
| identity marks | 0 | 24 shapes / 12 palettes | original geometry, accessibility contrast checked |
| eulogy fragments | `ProceduralEulogyEngine` internals | +40 fragments | no copied poetry/prose |

All content lands through `CatalogIntegrityValidator`; `--data-integrity-selftest`
must walk clean; every row must have a live consumer (kit gate).

---

## 6. Risk register

| Risk | Mitigation |
|---|---|
| Vertical becomes a morale-buff printer | bounded deltas, diminishing returns via existing morale mark system, starvation gates |
| Panel becomes gameplay authority | panels call Core commands only; route gate + liveness gate |
| Content feels like real-world religion | fictional-only content rule; `ZealotrySystem` fictional guard reused as precedent |
| Audio/voice work outruns string freeze | schedule C4-6 after U3 (PLAN-UNBLOCK-03); captions first |
| Save sprawl | single `culture` section, integrator-approved; else ride existing sections |
| Determinism drift | seeded streams, day stamps, replay test per package |

## 7. Focused verification (program level)

```bash
godot --headless --path . -- --culture-selftest        # new probe, added by C4-1
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Events/
bash scripts/run_test.sh Ashfall.Core.Tests/Memorial/
bash scripts/run_test.sh Ashfall.Core.Tests/Legacy/
```

## 8. Handoff notes

Each C4-N package uses the standard handoff block. The vertical is "integrated"
only when the kit's `--integration-selftest` lists the authority, the culture
day owner ticks, one panel route resolves, and a reload preserves the state.

---

## 6. Expanded census (15 files · 4,300 lines)

Domain: Culture, Spiritual, Voice, Print.

| Metric | Value |
|---|---:|
| Files | 15 |
| Lines | 4,300 |
| Banned refs | 0 |
| Save surfaces | 9 |
| Matched catalogs | 11 |

**Largest files:** `DocumentationSystem.cs` 698, `CulturalArchiveVaultSystem.cs` 675, `ShelterMuseumSystem.cs` 539, `ShelterFestivalEngine.cs` 323, `SurvivorVoiceSystem.cs` 316, `CultureCreationSystem.cs` 287, `PublicBroadsheetPressEngine.cs` 272, `VoiceLineDispatchCoordinator.cs` 253

## 7. Expanded surface: vertical contract

| Rule | Detail |
|---|---|
| Owners | each mechanic names an existing owner; the vertical composes, never duplicates |
| Data | catalogs resolve through loaders; consumable content is reachable |
| Determinism | seeded variance only; no wall clock |
| Save | state rides registered sections; keys per Plan 1 Appendix Q |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Culture/` |
| Consumer proof | one fixture per newly wired catalog |
| Determinism | scanned; counts above |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Wire owners; no parallel state.
3. Catalog consumer fixtures.
4. Regression: focused region + census.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Mechanic | composes owners; no duplicate authority |
| Catalog | consumer proven |
| State | registered; round-trips |
| Fixture | focused and green |

**Non-goals unchanged:** the vertical composes; it does not add authorities.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 8. Other plans referencing them: **9**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 6 |
| `EVIDENCE` | 6 |
| `PLAN-CREATIVE-WORKS-66` | 5 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 3 |
| `PLAN-SHELTER-DECOR-TRUTH-225` | 2 |
| `PLAN-SILENT-FAILURE-35` | 1 |
| `PLAN-RADIO-MEDIA-42` | 1 |
| `PLAN-PRINT-MEDIA-TRUTH-128` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `C4-1` | `CultureCreationSystem.cs` |
| `C4-2` | `ShelterFestivalEngine.cs` |
| `C4-3` | `ShelterMuseumSystem.cs` |
| `C4-4` | `CulturalArchiveVaultSystem.cs` |
| `C4-5` | `PublicBroadsheetPressEngine.cs` |
| `C4-6` | `SurvivorVoiceSystem.cs` |
| `C4-7` | no name match — resolve at claim time |
| `C4-8` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 8; intra-domain edges: **1**; isolated files:
**6**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `CulturalArchiveVaultSystem` | `DocumentationSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `DocumentationSystem` | 1 |
| `CulturalArchiveVaultSystem` | 0 |
| `CultureCreationSystem` | 0 |
| `PublicBroadsheetPressEngine` | 0 |
| `ShelterFestivalEngine` | 0 |
| `ShelterMuseumSystem` | 0 |
| `SurvivorVoiceSystem` | 0 |
| `VoiceLineDispatchCoordinator` | 0 |

**Class split:** hub 0 · sink 1 · source 1 · isolated 6.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 8. Host files: **1** · Test files: **12** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.FlagshipInstitutions.cs` |
| Tests (`Ashfall.Core.Tests/`) | 12 | `Ashfall.Core.Tests/Content/Plan49ContentAtmosphereIntegrationTests.cs`, `Ashfall.Core.Tests/CulturalArchiveVaultTests.cs`, `Ashfall.Core.Tests/Culture/CultureCreationSystemTests.cs`, `Ashfall.Core.Tests/Culture/DocumentationSystemTests.cs`, `Ashfall.Core.Tests/Culture/Plan178CreationToVaultTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **28** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `archive_desk` |
| `black_projects_archive` |
| `cryo_vault` |
| `cultural_archives` |
| `expanded_shelter` |
| `grain_milling_archive` |
| `hydrogeology_archive` |
| `leatherwork_archive` |
| `pneumatic_dispatch` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **13** (matched by domain keyword
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

Events whose name shares a domain token: **10**.

| Event | First declaration |
|---|---|
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnCulturalBroadcast` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
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
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/cultural_archive_tomes.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/documentation_templates.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/journal_voice_prose.json` |
| `Assets/StreamingAssets/Data/museum_collection_templates.json` |
| `Assets/StreamingAssets/Data/muster_faction_culture.json` |
| `Assets/StreamingAssets/Data/narrative/architect_vault_audits.json` |
| `Assets/StreamingAssets/Data/narrative/screw_press_felt_reports.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (97 files, 808 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Culture` | 7 | 40 |
| `Shelter` | 87 | 754 |
| `Voice` | 3 | 14 |

**Verdict:** 808 cases sit under matching regions — run those first (`Culture`, `Shelter`, `Voice`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **82**
(21 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/CryoVaultSaveStore.cs` |
| `src/Host/CulturalArchiveSaveStore.cs` |
| `src/Host/GrainMillingArchiveSaveStore.cs` |
| `src/Host/HoldfastDispatchLog.cs` |
| `src/Host/HydroGeologyArchiveSaveStore.cs` |
| `src/Host/LeatherworkArchiveSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **28**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `archive_desk` | no |
| `black_projects_archive` | no |
| `cryo_vault` | no |
| `cultural_archives` | no |
| `expanded_shelter` | no |
| `grain_milling_archive` | no |
| `hydrogeology_archive` | no |
| `leatherwork_archive` | no |
| `pneumatic_dispatch` | no |
| `shelter` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `shelter` |
| `vertical_ascent` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **20**
(CODEX_ONLY 8, GAMEPLAY_CONSUMED 5, OPTIONAL 2, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `journal_voice_prose.json` | GAMEPLAY_CONSUMED |
| `muster_faction_culture.json` | UNRESOLVED |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |
| `narrative/screw_press_felt_reports.json` | CODEX_ONLY |
| `narrative/shelter_notices_expansion.json` | CODEX_ONLY |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_expelled_survivor` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 28 (laddered 0) · RNG streams 2 · host files 16 · catalogs 22 · test regions 3 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-VERTICAL-CULTURE-04
wave: —
status: PROPOSED — foreman claim required
packages: author package list at claim time
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/archive_categories.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Culture/
  - godot --headless --path . -- --shelter-actor-physics-selftest
dependencies:
  - coordinate: 9 other plan(s) name these artifacts (§12)
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
| packages | **no** |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: packages.
