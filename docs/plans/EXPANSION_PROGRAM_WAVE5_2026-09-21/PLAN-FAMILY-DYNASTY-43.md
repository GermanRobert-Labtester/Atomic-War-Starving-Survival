# PLAN-FAMILY-DYNASTY-43 — Households, Childhood, Coming of Age & Inheritance

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-ORPHAN-SEAL-01 Wave 4, PLAN-VERTICAL-CULTURE-04,
PLAN-UNBLOCK-03 U1c (cross-run profile).
**Expanded appendix:** [`PLAN-FAMILY-DYNASTY-43_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-FAMILY-DYNASTY-43_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's survivor/voice/narrative/
generational systems, each mapped to its parent-plan mechanic row.
**Non-goals:** no real-world family law, no graphic content involving minors,
no second survivor store.

---

## 1. Outcome

The generational promise exists in fragments: `RomanceFamilySystem` (sealed,
4 stages + family units), `GenerationalSystem`/
`GenerationalLineageExtension`, `SecondGenerationMilestoneEngine` (orphan),
`CampaignLegacySystem` (orphan, 20 legacy traits), `ChildDevelopmentSystem`
(orphan), `AgingSystem`/`SurvivorAgingProgressionEngine` (orphans),
`education_curriculum.json`, `romance_courtship.json`,
`family_name_templates.json`. This plan makes a **dynasty** playable across a
multi-year campaign.

Player loop: **form bonds → raise children → teach a trade → come of age →
inherit traits → lose and remember → carry forward**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Household | `RomanceFamilySystem`, family units | pair, adopt, house | household morale, quarters |
| Childhood | `ChildDevelopmentSystem`, education | care, play, school | development stages, traits |
| Coming of age | `SecondGenerationMilestoneEngine` | assign a role/trade | new adult survivor, specialty |
| Aging | `AgingSystem`, `SurvivorAgingProgressionEngine` | adapt duties | capability curves, retirement |
| Legacy | `CampaignLegacySystem`, `legacy_traits.json` | record, bequeath | inherited traits, NG+ context |
| Loss | `SurvivorDeathLegacySystem`, guardianship | console, adopt | bereavement, guardianship |
| Endings | `UnifiedEndingResolver` | conclude the campaign | generational epilogue |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core | `GenerationalSystem` + `GenerationalLineageExtension`, `RomanceFamilySystem` (sealed 6/6), `SecondGenerationMilestoneEngine`, `CampaignLegacySystem`, `ChildDevelopmentSystem`, `AgingSystem`, `SurvivorAgingProgressionEngine`, `SurvivorDeathLegacySystem` |
| Data | `romance_courtship.json`, `family_name_templates.json`, `legacy_traits.json`, `education_curriculum.json`, `death_legacy_templates.json` |
| Sealed prior | Plan 150 romance/family (6/6), Plan 140 legacy (5/5), Plan 206 death legacy (7/7), Plan 176 campaign age clock, `DEC-32`/`DEC-33` |
| Boundaries | `CrossRunProfileStore` is user-level only; no campaign state leaks |

---

## 3. Packages

### FM-43A — Households and family units
- Bind `RomanceFamilySystem` to quarters/rooms and the survivor roster:
  household morale, shared quarters effects, and family-unit benefits; no new
  survivor store.
- **Acceptance:** family unit state persists; effects route through relations/
  morale; dissolution and adoption handled.
- **Verify:** `Plan150RomanceFamilyIntegrationTests` extends.

### FM-43B — Childhood and development
- `ChildDevelopmentSystem` stages children (care, play, school, chores);
  education (PLAN-SCIENCE-EDUCATION-38) provides proficiency; trauma from
  events is bounded and age-appropriate (no graphic content).
- **Acceptance:** child survivors age visibly; development gates coming of age;
  determinism; content tone reviewed.
- **Verify:** survivor lifecycle suites.

### FM-43C — Coming of age and roles
- `SecondGenerationMilestoneEngine` marks milestones (first duty, first
  expedition, trade mastery); the survivor becomes a full roster member with a
  specialty.
- **Acceptance:** milestone events fire once; role assignment uses the duty
  owner; no hidden stat jumps.
- **Verify:** `Plan185`/roster suites.

### FM-43D — Aging and capability curves
- `AgingSystem` + `SurvivorAgingProgressionEngine` model capability across
  years: experience up, physical capacity down; retirement/light duty exists.
- **Acceptance:** curves are visible and explainable; no sudden death from age;
  duties can be adapted.
- **Verify:** focused aging tests + balance sim.

### FM-43E — Legacy and inheritance
- `CampaignLegacySystem` records shelter improvements, faction memory, and
  inherited traits; `SecondGenerationMilestoneEngine` triggers trait evolution
  after three generations; NG+ applies the signed DEC-32/33 contracts only.
- **Acceptance:** inheritance deterministic; profile store stays user-level;
  no campaign save leakage.
- **Verify:** `Plan140GenerationalLegacyIntegrationTests` extends.

### FM-43F — Guardianship, loss and remembrance
- When a parent dies, guardianship passes with a memorial/bequest path
  (`SurvivorDeathLegacySystem`); children's grief is bounded and visible;
  family names persist (templates).
- **Acceptance:** no orphaned-child dead-end; bequest delivery canonical;
  grief dispersion by death quality.
- **Verify:** Plan 206 suites + memorial.

### FM-43G — Content volumes
- +8 courtship events, +12 childhood beats, +10 milestones, +10 family names,
  +8 epilogue clauses; fictional; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Tone risk around children | protective framing, no graphic harm, death handled with restraint |
| Long campaigns stall without children | adoption, wards, and found-family paths |
| Legacy becomes power creep | bounded trait bands; NG+ difficulty scales |
| Aging frustrates players | roles adapt; knowledge transfer keeps value |

## 5. Verification

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/
bash scripts/run_test.sh Ashfall.Core.Tests/Legacy/
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --cohort-lifecycle-selftest
```

---

## 6. Expanded census (3 files · 840 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SecondGenerationMilestoneEngine.cs` | 229 | System | **yes** | 0 | 0 | 0 |
| `GenealogyBridge.cs` | 183 | Support | — | 0 | 0 | 0 |
| `RelationshipDecaySystem.cs` | 428 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `relationship_decay_profiles.json` | object[2 keys] |
| `relationship_bands.json` | object[3 keys] |

**State surfaces:** `RelationshipDecaySystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 4 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **6**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-MUTATION-HEREDITY-81` | 1 |
| `PLAN-GENERATIONAL-MILESTONE-TRUTH-160` | 1 |
| `PLAN-RELATIONSHIP-DECAY-TRUTH-195` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `FM-43A` | no name match — resolve at claim time |
| `FM-43B` | no name match — resolve at claim time |
| `FM-43C` | no name match — resolve at claim time |
| `FM-43D` | no name match — resolve at claim time |
| `FM-43E` | no name match — resolve at claim time |
| `FM-43F` | no name match — resolve at claim time |
| `FM-43G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **3** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/RelationshipDecayHostSession.cs`, `src/Host/RelationshipDecaySelfTest.cs`, `src/Main.RelationshipDecay.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Generations/SecondGenerationMilestoneEngineTests.cs`, `Ashfall.Core.Tests/Survivors/Plan182RelationshipDecayIntegrationTests.cs`, `Ashfall.Core.Tests/Survivors/Plan217GenealogyIntegrationTests.cs`, `Ashfall.Core.Tests/Survivors/RelationshipDecaySystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `relationship_decay` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--day1-to-day2-milestone-selftest` |
| `--relationship-decay-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnSpecialtyMilestone` | `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/memory_decay_rates.json` |
| `Assets/StreamingAssets/Data/relationship_bands.json` |
| `Assets/StreamingAssets/Data/relationship_decay_profiles.json` |

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

Host files (`src/`) whose names share a domain token: **5**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/RelationshipDecayHostSession.cs` |
| `src/Host/RelationshipDecaySaveStore.cs` |
| `src/Host/RelationshipDecaySelfTest.cs` |
| `src/Main.RelationshipDecay.cs` |
| `src/UI/RelationshipDecayPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `relationship_decay` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **0**
(none).

| Catalog | Classification |
|---|---|
| — | no catalog filename shares a token with this domain |

**Verdict:** no catalog matches — the domain is code-authoritative or its data lives in a broader catalog.

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
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 5 · catalogs 3 · test regions 0 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-FAMILY-DYNASTY-43
wave: —
status: PROPOSED — foreman claim required
packages: FM-43A, FM-43B, FM-43C, FM-43D, FM-43E, FM-43F, FM-43G
claim paths:
  - src/Host/RelationshipDecayHostSession.cs  # §19 candidate host surface
  - src/Host/RelationshipDecaySaveStore.cs  # §19 candidate host surface
  - src/Host/RelationshipDecaySelfTest.cs  # §19 candidate host surface
  - src/Main.RelationshipDecay.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/memory_decay_rates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/relationship_bands.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --day1-to-day2-milestone-selftest
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
