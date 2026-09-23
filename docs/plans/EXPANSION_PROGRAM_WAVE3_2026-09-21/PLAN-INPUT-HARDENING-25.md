# PLAN-INPUT-HARDENING-25 — Untrusted Input, Path Safety & Mod Contract Sealing

**Wave:** 3 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim. Foreman claim required.
**Depends on:** PLAN-SAVE-GOVERNANCE-12 (save envelope), PLAN-RELEASE-OPS-20
(secrets policy), PLAN-DATA-AUTHORITY-14 (catalog ids).
**Non-goals:** no anti-cheat, no encryption, no server authority; this is
robustness and data-only modding.

---

## 1. Outcome

The game consumes four classes of input it does not fully control: **save
files** (user-writable), **mod data** (third-party), **settings** (user-edited),
and **CLI flags** (build automation). Authored catalogs are semi-trusted (they
ship with the build). This plan makes every untrusted path validate before it
can reach gameplay, a path, or the UI.

Deliverables:

1. a **threat model and data-flow map** for the four input classes;
2. **save validation**: bounded sizes, depths, string lengths, id allowlists,
   enum strictness, no path construction from save ids;
3. **mod contract sealing**: manifest validation, override conflicts,
   deterministic load order, data-only enforcement (no code execution);
4. **path safety**: every id→path conversion normalized and allowlisted;
5. **UI/text safety**: render-safe strings and length caps;
6. **secret hygiene verification** across logs, artifacts, and test fixtures.

---

## 2. Evidence

| Fact | Value | Source |
|---|---:|---|
| Save stores | 181 | `ls src/Host/*SaveStore.cs` |
| Registry sections | 204 | `SaveSectionRegistry` |
| Fuzz gate | campaign envelope fuzzing + mutation (xUnit) | `CI_GATE_MANIFEST.json` |
| Mod contract | `ModSupportSystem`, `mod_manifest_schema.json`, `JsonModLayeringTests` (19/19) | Plans 165/47 evidence |
| Mod support host-pending | 0 host refs | PLAN-UNBLOCK-03 §2.2 |
| Settings | `UserSettings`/`UserSettingsStore`; colorblind, a11y flags | settings tree |
| Path gates | forbidden-path, case-collision, legacy-path gates | `scripts/ci` |
| Catch policy | `catch-policy-gate.sh` | `scripts/ci` |
| Secrets | `.env` present and gitignored; no keys in prompts/logs rule | `AGENTS.md` rule 9 |
| Journal/UI text | player-facing strings from catalogs and save | journal/UI surfaces |
| Asset resolution | 4-step precedence with fallback interception | `AssetManifest` |

---

## 3. Packages

### IH-25A — Threat model and data-flow map
- `docs/security/INPUT_THREAT_MODEL.md`: for each input class — trust level,
  entry point, parser, validation today, blast radius (save corruption, path
  escape, UI text, resource exhaustion, RNG manipulation), and the target
  control.
- **Acceptance:** every parser is listed with its validation status (validated /
  partial / none); no class omitted.
- **Verify:** static review; link from `AGENTS.md` source-of-truth table by the
  foreman.

### IH-25B — Save input validation
- Rules: max file size, max nesting depth, max array length, max string length
  per field class, ids validated against catalogs where they must exist,
  enums strict (unknown → default or reject, never cast blindly), numbers
  range-clamped, no duplicate keys accepted silently.
- Unknown future fields remain ignored (forward compat), but the envelope must
  fail closed on structural violation.
- **Acceptance:** fuzz families gain size/depth/string cases; a hostile save
  cannot crash, cannot allocate without bound, cannot place a path outside
  `user://`.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Save/` (fuzz files
  first) + `godot --headless --path . -- --save-load-ui-failure-selftest`.

### IH-25C — Mod contract sealing
- Data-only enforcement: mods may contribute catalog rows and overrides per the
  schema; they may not execute code, define new authority, or write saves.
- Validate: manifest schema, version range, dependency DAG (missing/cyclic),
  deterministic load order, override conflict detection, and per-catalog
  override limits.
- **Acceptance:** a malicious manifest (path traversal in a file reference,
  oversized payload, cyclic deps, duplicate id) is rejected with a typed
  error; a valid pack loads deterministically across restarts; host wiring
  exists (closes the host-pending artifact).
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Mods/`;
  `godot --headless --path . -- --mods-selftest` (existing `HostCli.Mods.cs`).

### IH-25D — Path safety
- Every id→path conversion (asset, catalog, save, mod file) must pass a
  normalize + allowlist check: no `..`, no absolute paths, no separators in
  ids, no case collisions, extension allowlist.
- **Acceptance:** a validator helper is the single path builder; the
  forbidden-path and case gates extend to it; a hostile id fails with a typed
  error.
- **Verify:** focused tests + `bash scripts/ci/case-collision-gate.sh` +
  the forbidden-path gate.

### IH-25E — UI/text safety
- Player-facing strings from untrusted input (mod names, save-stored names,
  survivor names) are length-capped and rendered as plain text; no rich-text
  markup execution, no unbounded tooltip growth.
- **Acceptance:** UI snapshot for a hostile string (very long, markup-like,
  RTL/control chars) renders safely; names are trimmed and normalized on entry.
- **Verify:** a focused UI test + snapshot.

### IH-25F — Secret hygiene verification
- Verify: `.env` untracked and excluded from builds; no token/key appears in
  artifacts, logs, test fixtures, or docs; CI workflows use no plaintext
  secrets; a scan gate for common key patterns (bounded, no false-positive
  spam).
- **Acceptance:** scan gate green; on a synthetic positive it fails.
- **Verify:** `python3 scripts/ci/secret-scan.py --check` (new) +
  `git check-ignore .env`.

---

## 4. Risk register

| Risk | Mitigation |
|---|---|
| Over-strict validation breaks legitimate legacy saves | unknowns ignored; limits generous (measured against the golden corpus); defaults for absent fields |
| Mod strictness blocks the community | data-only schema is the published contract; rejection errors name the exact field |
| Path helper becomes a bottleneck | single helper, no allocation on the hot path; validate once at load |
| Secret scan false positives | allowlist for documented test constants; report-only on docs first |

## 5. Verification

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Save/
bash scripts/run_test.sh Ashfall.Core.Tests/Mods/
godot --headless --path . -- --mods-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
python3 scripts/ci/secret-scan.py --check
bash scripts/ci/case-collision-gate.sh
```

---

## 6. Expanded census (bespoke: input handling surface)

This plan hardens input handling, so the census covers the action set and the
handling call sites.

| Metric | Value |
|---|---:|
| Declared actions | 22 |
| Input-handling call sites | 402 |
| Files with handlers | 125 |

**Actions:** `ashfall_close`, `ashfall_confirm`, `ashfall_events`, `ashfall_expeditions`, `ashfall_forecast`, `ashfall_guidance`, `ashfall_help`, `ashfall_holdfast`, `ashfall_holdfast_build`, `ashfall_holdfast_status`, `ashfall_journal`, `ashfall_journal_tab_1`, `ashfall_journal_tab_2`, `ashfall_journal_tab_3`, `ashfall_journal_tab_4`, `ashfall_journal_tab_5`, `ashfall_nav_down`, `ashfall_nav_left`, `ashfall_nav_right`, `ashfall_nav_up`, `ashfall_next_tab`, `ashfall_weather_history`
**Top handler files:** `AshfallInputActions.cs`, `AshfallFocusNavigator.cs`, `MapLocationMarkerView.cs`, `RoomHotspotView.cs`, `InventoryDetailPanel.cs`, `KeyBindingApplicator.cs`

## 7. Expanded surface: handling contract

| Rule | Detail |
|---|---|
| Action set | the declared actions are the closed set |
| Focus safety | a handled key never steals focus from text entry |
| Modal discipline | an input in a modal does not leak to the world |
| Release | held inputs release on focus loss/pause |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focus-loss fixture | no stuck held keys after focus loss |
| Modal fixture | input does not leak past the modal |
| Action parity | declared actions == handled actions |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Focus-loss and modal fixtures.
3. Action parity check.
4. Regression: fixture set.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Action | handled exactly once per context |
| Focus | no stuck input after focus loss |
| Modal | no leak to world |
| Parity | every declared action has a handler |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not add actions.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 12. Other plans referencing them: **23**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-LAUNCH-FACE-06` | 3 |
| `PLAN-RELEASE-OPS-20` | 3 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 3 |
| `PLAN-RUNTIME-PERF-16` | 2 |
| `PLAN-BOOTSTRAP-GATE-TRUTH-147` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `PLAN-UNBLOCK-03` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `AGENTS.md` |
| `AshfallFocusNavigator.cs` |
| `AshfallInputActions.cs` |
| `CI_GATE_MANIFEST.json` |
| `HostCli.Mods.cs` |
| `InventoryDetailPanel.cs` |
| `KeyBindingApplicator.cs` |
| `MapLocationMarkerView.cs` |
| `RoomHotspotView.cs` |
| `catch-policy-gate.sh` |
| `docs/security/INPUT_THREAT_MODEL.md` |
| `mod_manifest_schema.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `IH-25A` | `docs/security/INPUT_THREAT_MODEL.md` |
| `IH-25B` | `AshfallInputActions.cs`, `docs/security/INPUT_THREAT_MODEL.md` |
| `IH-25C` | no name match — resolve at claim time |
| `IH-25D` | no name match — resolve at claim time |
| `IH-25E` | no name match — resolve at claim time |
| `IH-25F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 12. Host files: **38** · Test files: **15** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 38 | `src/Host/AshfallInputActions.cs`, `src/Host/ChemicalReconHostSession.cs`, `src/Host/CoreDemoSession.cs`, `src/Host/GeodeticSurveyHostSession.cs`, `src/Host/HoldfastTerminalPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 15 | `Ashfall.Core.Tests/DataRuleComplianceTests.cs`, `Ashfall.Core.Tests/Mods/Plan165ModdingIntegrationTests.cs`, `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`, `Ashfall.Core.Tests/NewSaveStoreTriadTests.cs`, `Ashfall.Core.Tests/Plan12AGenerationTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/standing_gates.json`, `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **11** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `geodetic_survey` |
| `holdfast` |
| `holdfast_trade` |
| `inventory` |
| `radiation` |
| `recon_telemetry` |
| `unique_claims` |
| `weather_hardening` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **24** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--chemical-dependency-save-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--holdfast-briefing` |
| `--holdfast-runtime-selftest` |
| `--holdfast-runtime-ui-test` |
| `--holdfast-runtime-uitest` |
| `--holdfast-save-selftest` |
| `--holdfast-selftest` |
| `--holdfast-trade-save-selftest` |
| `--ice-road-tick-demo` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **16**.

| Event | First declaration |
|---|---|
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnLocationMutated` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationOwnerChanged` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationRecast` | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` |
| `OnLocationRevealed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |
| `OnRadiationDoseResetRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnRadiationExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |
| `OnRoomEntered` | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` |
| `OnRoomUnlocked` | `Assets/Ashfall.Core/StandingRecord/LocationLayoutSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/autonomy_actions.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **14** (177 files, 1327 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `Mods` | 3 | 34 |
| `Needs` | 4 | 21 |

**Verdict:** 1327 cases sit under matching regions — run those first (`Audio`, `Balance`, `Combat`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **611**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **41**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `amputation` | no |
| `anomaly_hazard` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan_trade_network` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **19**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **119**
(CODEX_ONLY 36, GAMEPLAY_CONSUMED 65, OPTIONAL 4, UNRESOLVED 14).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 14 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** hub · **Coupling (incoming plans):** 23
**Surface:** save sections 41 (laddered 1) · RNG streams 19 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INPUT-HARDENING-25
wave: —
status: PROPOSED — foreman claim required
packages: IH-25A, IH-25B, IH-25C, IH-25D, IH-25E, IH-25F
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/autonomy_actions.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 23 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
