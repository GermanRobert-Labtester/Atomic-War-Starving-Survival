# PLAN-DISCOVERY-STATE-108 — Known, Visited & Rumored Knowledge with Map Degradation

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-CARTOGRAPHY-LANDMARKS-70, PLAN-SPATIAL-SIM-AUTHORITY-95, PLAN-REFERENCE-INTEGRITY-34.
**Non-goals:** no map rendering work, no new coordinate system; the route and
place authorities named in Plan 95 stay the source of geometry.

## 1. Outcome
Plan 70 wires landmarks; Plan 95 maps spatial authorities. Between them sits
**knowledge state**: what the holdfast knows about a place (rumor), has
verified (visited), or has lost (damaged map). `World/DamagedMapSystem.cs`,
`World/DamagedMapCatalog.cs`, and `World/FieldGuideCatalog.cs` exist, and
`LivingMapRouteProjection` already projects route state — but no contract says
which authority owns "known", how rumor differs from truth, or how map damage
degrades knowledge rather than geometry.

| Deliverable | Detail |
|---|---|
| Knowledge states | `rumored → verified → stale/lost` with the event that moves each edge |
| Owner map | one owner per state per subject (place id, route edge); projections never write state |
| Degradation | damaged-map effects remove or fuzz knowledge, never the underlying place; recovery restores knowledge only where earned |
| Sharing | faction/npc knowledge sharing grants rumor state only, with source recorded |
| Save truth | knowledge restores with its owning section; a projection-only load loses nothing |

## 2. Evidence
- `Assets/Ashfall.Core/World/DamagedMapSystem.cs`, `DamagedMapCatalog.cs`, `FieldGuideCatalog.cs`.
- Plan 95: authority map for places/territory/routes; read-model rule for projections.
- Plan 70 owns landmark discovery wiring; this plan owns the state semantics behind it.
- Plan 34: `loc_` id family (117 ids / 59 files) is the subject space.

## 3. Packages
- **DST-108A** state machine doc + subject/owner table.
- **DST-108B** transition tests: rumored→verified (arrival), verified→stale (time/damage), stale→verified (revisit).
- **DST-108C** degradation semantics test: damage removes knowledge, place record intact, recovery path restores only earned knowledge.
- **DST-108D** sharing path: npc/faction grant produces `rumored` with a source field and no verified flag.
- **DST-108E** save round-trip + projection-only load check.

## 4. Acceptance & verification
- No transition exists outside the table; a projection write attempt fails the guard.
- Damaged map: place still exists; knowledge state changes per catalog effect.
- Save/load preserves state; loading with the projection absent loses nothing.
- `bash scripts/run_test.sh Ashfall.Core.Tests/World/`.

## 5. Risks
Knowledge becoming an authority overrides geometry → geometry stays in Plan 95 owners; this plan's guard enforces it.
Rumor/truth ambiguity in UI → state enum is explicit and tested, no boolean shortcuts.

---

## 6. Expanded census (4 files · 857 lines)

Scope: `Assets/Ashfall.Core/World/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 2 · Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DamagedMapCatalog.cs` | 317 | Catalog | — | 0 | 0 | 0 |
| `DamagedMapSystem.cs` | 205 | System | **yes** | 0 | 0 | 0 |
| `FieldGuideCatalog.cs` | 199 | Catalog | **yes** | 0 | 0 | 2 |
| `TravelGraphKnowledgeGate.cs` | 136 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `research_knowledge.json` | object[3 keys] |

**State surfaces:** `FieldGuideCatalog.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/World/` |
| Test references | 11 name references across the test tree |
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

Domain files: 4. Other plans referencing their names: **6**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `EVIDENCE` | 3 |
| `PLAN-CARTOGRAPHY-LANDMARKS-70` | 3 |
| `PLAN-ECOLOGY-WILDLIFE-26` | 1 |
| `PLAN-SPATIAL-SIM-AUTHORITY-95` | 1 |
| `PLAN-CODEX-SURFACE-TRUTH-110` | 1 |
| `PLAN-WORLD-FAMILY-TRUTH-267` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DST-108A` | no name match — resolve at claim time |
| `DST-108B` | `DamagedMapCatalog.cs`, `DamagedMapSystem.cs` |
| `DST-108C` | `TravelGraphKnowledgeGate.cs`, `DamagedMapCatalog.cs`, `DamagedMapSystem.cs` |
| `DST-108D` | `FieldGuideCatalog.cs` |
| `DST-108E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **6** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Host/CodexHostSession.cs`, `src/Host/ExpeditionHostSession.cs`, `src/Host/HostCli.WastelandInhabitants.cs`, `src/Host/WorldHostSession.cs`, `src/Main.Codex.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/Codex/CodexProjectionTests.cs`, `Ashfall.Core.Tests/FieldGuidePersistenceTests.cs`, `Ashfall.Core.Tests/World/DamagedMapSystemTests.cs`, `Ashfall.Core.Tests/World/Plan20WastelandInhabitantsTests.cs`, `Ashfall.Core.Tests/World/Plan85_71DamagedMapPowerIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **1**; isolated: **2**.

| From | → To |
|---|---|
| `DamagedMapCatalog` | `DamagedMapSystem` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `field_guide` |
| `knowledge` |
| `travel_encounters` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--travel-encounter-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnEquipmentDamaged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnToolDamaged` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/damaged_map_zones.json` |
| `Assets/StreamingAssets/Data/discovery_consequences.json` |
| `Assets/StreamingAssets/Data/field_guide.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_field_reports_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/faction_field_documents.json` |
| `Assets/StreamingAssets/Data/narrative/field_reports_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |
| `Assets/StreamingAssets/Data/research_knowledge.json` |
| `Assets/StreamingAssets/Data/travel_encounters.json` |

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

Host files (`src/`) whose names share a domain token: **3**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/CollectibleDiscoverySaveStore.cs` |
| `src/Host/FieldGuideSaveStore.cs` |
| `src/Host/TravelEncounterSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `collectible_discovery` | no |
| `field_guide` | no |
| `knowledge` | no |
| `travel_encounters` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **10**
(CODEX_ONLY 6, GAMEPLAY_CONSUMED 2, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `damaged_map_zones.json` | GAMEPLAY_CONSUMED |
| `field_guide.json` | UNRESOLVED |
| `narrative/blast_gate_mechanical_audits.json` | CODEX_ONLY |
| `narrative/expedition_field_reports.json` | CODEX_ONLY |
| `narrative/expedition_field_reports_batch_2.json` | CODEX_ONLY |
| `narrative/faction_field_documents.json` | CODEX_ONLY |
| `narrative/field_reports_expansion.json` | CODEX_ONLY |
| `narrative/wildlife_field_encounter_logs.json` | CODEX_ONLY |
| `research_knowledge.json` | GAMEPLAY_CONSUMED |
| `travel_encounters.json` | UNRESOLVED |

**Verdict:** 2 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 4 (laddered 0) · RNG streams 0 · host files 3 · catalogs 22 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DISCOVERY-STATE-108
wave: 9
status: PROPOSED — foreman claim required
packages: DST-108A, DST-108B, DST-108C, DST-108D, DST-108E
claim paths:
  - src/Host/CollectibleDiscoverySaveStore.cs  # §19 candidate host surface
  - src/Host/FieldGuideSaveStore.cs  # §19 candidate host surface
  - src/Host/TravelEncounterSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/damaged_map_zones.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/discovery_consequences.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --travel-encounter-selftest
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
