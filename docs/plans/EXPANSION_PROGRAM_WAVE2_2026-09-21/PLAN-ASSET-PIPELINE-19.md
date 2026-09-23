# PLAN-ASSET-PIPELINE-19 — Asset Truth, Fallback Elimination & Audio Registry Parity

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (registry + gates) with asset
production/import Builders.
**Depends on:** PLAN-INTEGRATION-KIT-02 (gates), PLAN-CORE-ONLY-REGISTRY-11
(retired asset families).
**Non-goals:** no style redesign, no copied art (external references only as
direction; production assets are original), no new asset format.

---

## 1. Outcome

Assets are already gated (asset registry, decode gate, orphan sweep, audio cue
gates) but the registry runs in **non-strict mode** and the last integration
ledger recorded **12 missing item-icon fallbacks** left visible. The programme
adds new systems (culture exhibits, industry machines, outpost markers, comms
channels) that need art and audio. This plan makes asset truth strict and keeps
the new demand bounded and original.

Deliverables:

1. **strict asset resolution** in release builds: zero fallbacks, zero missing;
2. **fallback elimination**: the known 12 item icons plus any surfaced by the
   strict run;
3. **audio registry parity**: every declared cue resolves to a file, every file
   to a cue; loudness normalization and format policy enforced;
4. **LFS/import hygiene**: asset provenance rows, import presets, orphan sweep;
5. **generation pipeline hardening** for procedural/original assets with a
   provenance rule;
6. a **scene-port completion ledger** for the remaining Unity-era asset trees.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| Git LFS files | 3,858 | `git lfs ls-files \| wc -l` |
| `.git` size / pack | 1.8 GB / 134.17 MiB | `git count-objects -vH`, `du -sh .git` |
| Asset registry (data) | 355 assets in 6 families | `Assets/StreamingAssets/Data/asset_registry.json` |
| Asset registry (artifact) | 1,706 entries | `artifacts/asset_registry.json` |
| Strict mode | `false` | same |
| Missing item-icon fallbacks | 12 recorded, left visible | INTEGRATION_PLANS closeout 2026-09-20 |
| Audio catalogs | `audio_cues.json`, `audio_accessibility_cues.json`, `shelter_audio_cues.json`, `audio_logs_expansion_05.json` | `Data/` listing |
| Existing gates | `godot-asset-gate.sh`, `asset-decode-gate.py`, `asset-orphan-sweep.sh`, `audio-asset-gate.py` | `scripts/ci` |
| Generators | `generate-asset-registry.py`, `generate-audio-catalog.py`, `generate_item_icons.py` | `scripts/ci` |
| Skills | `ashfall-design`, `ashfall-foundry`, `ashfall-asset-counter`, `ashfall-audio-qa`, `ashfall-scene-port` | `.agents/skills` |

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Resolution | `AssetManifest` 4-step precedence (explicit → stem → family fallback → missing) |
| Registry | `asset_registry.json` + `generate-asset-registry.py --check` |
| Audio cues | cue catalogs + `generate-audio-catalog.py --check`, `AudioManager` bindings |
| Import | Godot-native `assets/` tree, `.import` files, import presets recorded in the registry |
| Provenance | `source_model_provenance` fields already in the registry rows |
| Policy | `docs/audio/AUDIO_POLICY.md`, asset migration reports under `artifacts/` |

---

## 4. Packages

### AP-19A — Strict asset mode and fallback elimination
- Produce the full fallback/missing list from a strict run; triage into
  `produce` (generate/import an original asset), `alias` (point at an existing
  family-appropriate asset with a registry row), or `remove` (the reference is
  obsolete).
- **Acceptance:** strict run reports 0 fallbacks, 0 missing; the 12 item icons
  are resolved; the strict flag is enabled in the release export profile.
- **Verify:** `python3 scripts/ci/generate-asset-registry.py --check`;
  `bash scripts/ci/godot-asset-gate.sh --strict`.

### AP-19B — Registry truth
- Every registry row: family, path, import preset, provenance
  (`authored|generated|migrated`), consumer or consumer class, and fallback.
  The generated artifact and the authored data file must agree (`--check`).
- **Acceptance:** no row without provenance and import preset; the artifact
  regeneration is deterministic; a new asset reference without a row fails the
  gate.
- **Verify:** `python3 scripts/ci/generate-asset-registry.py --check`.

### AP-19C — Audio registry parity
- Parse all cue catalogs and all audio imported files: every cue resolves to a
  file; every file maps to ≥1 cue (or is registered as ambience/music with a
  consumer); loudness normalization within the policy band; format policy
  (sample rate/bit depth/compression) enforced.
- Wire the Plan 169 accessibility coordinator and Plan 52 scarcity machine to
  the catalogs so critical cues carry captions and mix presets.
- **Acceptance:** parity gate green; no orphan audio files; no cue without a
  producer event; critical cues have a visual notification mapping.
- **Verify:** `python3 scripts/ci/generate-audio-catalog.py --check`;
  `python3 scripts/ci/audio-asset-gate.py`; `godot --headless --path . -- --audio-selftest`.

### AP-19D — LFS and import hygiene
- Collect: LFS object health, oversized binaries, duplicate files (case-folded
  and content hash), and assets with no registry row. Coordinate repo-size
  remediation with PLAN-RELEASE-OPS-20 (no history rewrite here).
- **Acceptance:** duplicate/orphan counts reported and reduced; case collisions
  zero; no new asset family without an import preset.
- **Verify:** `bash scripts/ci/asset-orphan-sweep.sh`;
  `bash scripts/ci/case-collision-gate.sh`.

### AP-19E — Generation pipeline hardening
- For generated assets (procedural textures, icons, audio): pin tool + seed +
  parameters in a manifest so the asset is reproducible; store the recipe, not
  just the output; require a human review note for each batch.
- **Acceptance:** every generated file has a recipe row; re-running a recipe
  reproduces the asset byte-identically (or within a documented tolerance for
  lossy formats); no external copyrighted source is used.
- **Verify:** the recipe runner `--check` + the asset gate.

### AP-19F — Scene-port completion ledger
- Inventory the remaining Unity-era trees (`Assets/art`, `assets/ui`, `assets/
  audio/radio` per the scene-port skill) and mark: ported, superseded, or
  retired. Each row names the Godot destination path or the retirement reason.
- **Acceptance:** no unclassified Unity-era asset family; retired trees are
  excluded from imports; the ledger is linked from the Core-only registry where
  relevant.
- **Verify:** `python3 scripts/audit_assets.py` + the ledger review.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Strict mode blocks a release for one icon | produce/alias quickly; the gate reports the exact list; no blanket suppress |
| Generated assets lack provenance | recipe requirement before merge |
| Audio loudness recalibration touches shipped feel | policy band with measured targets; critical-cue captions unaffected |
| LFS cleanup drifts toward history rewrite | this plan reports; PLAN-RELEASE-OPS-20 owns any destructive decision |
| Fallback "alias" hides a real gap | alias requires a registry row and a family match; strict mode still flags family fallback use |

## 6. Verification summary

```bash
python3 scripts/ci/generate-asset-registry.py --check
python3 scripts/ci/generate-audio-catalog.py --check
python3 scripts/ci/audio-asset-gate.py
bash scripts/ci/godot-asset-gate.sh --strict
bash scripts/ci/asset-orphan-sweep.sh
bash scripts/ci/case-collision-gate.sh
godot --headless --path . -- --audio-selftest
```

## 7. Change control

Asset registry and cue catalogs are generated or authored authority; no runtime
system writes them. Every new asset lands with a registry row and a consumer.
External references inform direction only; shipped assets are original.

---

## 6. Expanded census (bespoke: asset pipeline surface)

This plan governs asset import/wiring, so the census counts the Godot asset
tree and its import sidecars.

| Metric | Value |
|---|---:|
| Asset files (`assets/`) | 8289 |
| `.import` sidecars | 4075 |
| Distinct extensions | 23 |

**Top extensions:** `.import` 4075, `.png` 2069, `.jpg` 1666, `.wav` 194, `.mp3` 83, `.html` 60, `.svg` 47, `.tscn` 26, `(none)` 12, `.ogg` 9

## 7. Expanded surface: pipeline contract

| Rule | Detail |
|---|---|
| Native imports | assets enter through Godot's importer; no hand-edited generated files |
| Registry | every wired asset appears in the asset registry |
| Dedupe | byte-identical assets are reported (one canonical copy) |
| Deprecated trees | `Assets/_Game/` and Unity-era folders are not extended (Plan 94) |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Orphan assets | asset without registry/wiring row reported |
| Duplicates | hash-dedupe report |
| Import integrity | `.import` companion exists per importable asset |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) and orphan report.
2. Dedupe report.
3. Registry completion for wired assets.
4. Regression: counts per asset batch.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Asset | imported natively; registry row exists |
| Duplicate | canonical copy chosen; others removed |
| Import | sidecar present; no hand edits |
| Tree | no new files in deprecated folders |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not re-import assets.

---

## 12. Cross-plan coupling

This plan governs artifacts rather than a `.cs` domain; the domain set is the
plan's own backticked artifact list (15 files). Other plans referencing
those artifacts: **10**.

**Incoming plan edges (top 8):**

| Plan | Artifact mentions |
|---|---:|
| `PLAN-AUDIO-MIX-AUTHORITY-97` | 4 |
| `PLAN-AUDIO-CONDITION-TRUTH-255` | 4 |
| `PLAN-RELEASE-OPS-20` | 3 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 2 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-DATA-AUTHORITY-14` | 1 |
| `PLAN-DATA-CONSUMER-22` | 1 |
| `PLAN-SHELTER-ARCHITECTURE-40` | 1 |

**Artifacts (first 12):**

| Artifact |
|---|
| `Assets/StreamingAssets/Data/asset_registry.json` |
| `artifacts/asset_registry.json` |
| `asset-decode-gate.py` |
| `asset-orphan-sweep.sh` |
| `asset_registry.json` |
| `audio-asset-gate.py` |
| `audio_accessibility_cues.json` |
| `audio_cues.json` |
| `audio_logs_expansion_05.json` |
| `docs/audio/AUDIO_POLICY.md` |
| `generate-asset-registry.py` |
| `generate-audio-catalog.py` |

**Package → candidate artifacts (heuristic by name overlap):**

| Package | Candidate artifacts |
|---|---|
| `AP-19A` | `Assets/StreamingAssets/Data/asset_registry.json`, `artifacts/asset_registry.json`, `asset-decode-gate.py` |
| `AP-19B` | `Assets/StreamingAssets/Data/asset_registry.json`, `artifacts/asset_registry.json`, `asset_registry.json` |
| `AP-19C` | `Assets/StreamingAssets/Data/asset_registry.json`, `artifacts/asset_registry.json`, `asset_registry.json` |
| `AP-19D` | no name match — resolve at claim time |
| `AP-19E` | no name match — resolve at claim time |
| `AP-19F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; artifacts are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 13. Host files: **2** · Test files: **6** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Audio/AudioCueCatalog.cs`, `src/Main.Plans50_53.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/Assets/Plan50AssetTruthIntegrationTests.cs`, `Ashfall.Core.Tests/Audio/Plan169AudioAccessibilityIntegrationTests.cs`, `Ashfall.Core.Tests/Campaign/Plans50_53_SharedIntegrationTests.cs`, `Ashfall.Core.Tests/Hygiene/Plan56RepositoryClassificationIntegrationTests.cs`, `Ashfall.Core.Tests/Tooling/AudioCueIntegrityTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

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

Matching flags in `HostCliRegistry.cs`: **24** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--accessibility-selftest` |
| `--asset-coverage-report` |
| `--asset-registry-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--checksum-sweep-selftest` |
| `--data-integrity-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-actor-physics-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnItemAdded` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnItemDegraded` | `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` |
| `OnItemRemoved` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/accessibility_profiles.json` |
| `Assets/StreamingAssets/Data/asset_registry.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/expansion_item_tags.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/item_degradation.json` |
| `Assets/StreamingAssets/Data/item_description_texts.json` |
| `Assets/StreamingAssets/Data/leadership_policies.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **6** (134 files, 1060 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `Shelter` | 87 | 754 |

**Verdict:** 1060 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Combat`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **231**
(26 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **26**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan_trade_network` | no |
| `combat` | no |
| `encounter_choice` | no |
| `equipment_condition` | no |
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `holdfast_trade` | no |
| `host_event` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **6**.

| Stream |
|---|
| `acoustic_detection` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **114**
(CODEX_ONLY 73, GAMEPLAY_CONSUMED 24, OPTIONAL 7, UNRESOLVED 10).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `environmental_texts_expansion_05.json` | GAMEPLAY_CONSUMED |
| `expansion_item_tags.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |

**Verdict:** 10 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 26 (laddered 0) · RNG streams 6 · host files 20 · catalogs 22 · test regions 6 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ASSET-PIPELINE-19
wave: —
status: PROPOSED — foreman claim required
packages: AP-19A, AP-19B, AP-19C, AP-19D, AP-19E, AP-19F
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/asset_registry.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --accessibility-selftest
dependencies:
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
