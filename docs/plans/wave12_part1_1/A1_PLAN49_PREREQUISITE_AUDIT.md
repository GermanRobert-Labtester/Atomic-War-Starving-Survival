# Wave 12 Part 1.1 A1 — Plan 49 Prerequisite Audit

**Status:** `DECIDED-DEFERRED` — evidence-only conflict-resolution tranche

**Authorization:** foreman, 2026-09-18. The authorized response to the
Plan 49 / Plan 42 / Plan 46 ordering conflict is an audit-only Plan 49 tranche
until the prerequisite chain is truthfully certified.

## Scope and boundary

This tranche may capture and classify current content-utilization evidence. It
does not modify production code, catalog data, content consumers, selectors,
voice delivery, metrics infrastructure, save schema, generated *gameplay*
indexes, or historical corpus plans. The generated documentation index is
refreshed through its canonical source process only. This tranche cannot mark
C1[16] implemented or remove a Plan 49 acceptance exemption.

## Current-source evidence

| Requirement | Source | Current status | Result |
|---|---|---|---|
| Plan 49 requires Plan 42A/42B voice and density rails | `C1_planintegration[16].md` header and §§49A.17–49A.19 | C2[18]/Plan 42 is `AUDIT-PENDING` | Not ready. |
| Plan 42 needs authored survivor identity and port enforcement | `C2_planintegration[18].md` header and §4 | C2[17] is `AUDIT-PENDING`; C2[13]/Plan 36 is `PARTIALLY-SEALED` | Plan 42 cannot be promoted as a sealed prerequisite. |
| Plan 49 requires Plan 46B reachability evidence | `C1_planintegration[16].md` header, §4, §8 | C2[20]/Plan 46 is `AUDIT-PENDING` | Formal reachability closure is not ready. |
| Plan 46 requires difficulty labels and release/onboarding rails | `C2_planintegration[20].md` header and §4 | C2[12]/Plan 34 is `PARTIALLY-SEALED`; C2[16]/Plan 39 is `AUDIT-PENDING` | Plan 46 cannot supply certified 46B evidence yet. |

The original sequence therefore has an unresolved dependency cycle: C1[16]
requires Plan 42, while the supplied Wave 12 ordering places C2[18]/Plan 42 in
Part 1.2 after C1[16]; Plan 46 is neither certified nor scheduled before it.

## Baseline captured

At `033df2b725c81f1843fff5796a04261c4d72de00`:

- `godot --headless --path . -- --content-utilization-selftest` passed.
- The scanner reported 619 catalogs / 12,161 definitions, 10 verified orphan
  catalogs, 99 unresolved catalogs, and 4 exempted catalogs.
- The Plan 49 deep-chain gate passed with one warning:
  `hydroponic_crops.json` has no registered `HydroponicBiomeSystem` consumer.

This is scanner evidence, not the Plan 46B synthetic-player reachability
evidence that Plan 49 requires.

## Row-level provenance and classification

The scanner's expected-consumer labels are discovery hints, not a Level-3
runtime path. This audit searched the current `Assets/` and `src/` source tree
for each catalog filename and for construction/loading of the named owner. A
loader that is used only in Core tests does not establish gameplay reachability.

| Catalog | Rows | Current-source evidence | Audit classification | Required next authority / condition |
|---|---:|---|---|---|
| `audio_logs_expansion_05.json` | 30 | No production filename reference exists outside `ContentUtilizationScanner`; Plan 49B.13--15 names the family but requires a delivery and playback choice. | `DECISION-BLOCKED` | Choose one existing delivery authority per family: Plan 35 item/collection delivery or the archive record owner. Plan 42 voice/playback remains excluded from this tranche. |
| `cassette_sets.json` | 12 | The live radio authority records player-made broadcasts (`RadioRecordingSystem`); the live morale authority loads `narrative/vinyl_record_archive.json`, not this file. No cassette-set production loader exists. | `OUT-OF-SCOPE / DECISION-BLOCKED` | A separate collection/content package must define item delivery, completion, journal, and playback semantics. It is not a scanner-metadata repair. |
| `confession_secrets.json` | 38 | `ConfessionSecretCatalog` and `ConfessionSecretSystem` are Core-tested, but no production composition constructs the system or loads the catalog. Plan 49C.17--20 and C2 Plan 44B Phase H require the pair-event/social-history seam. | `DEPENDENCY-BLOCKED` | Promote/reverify Plan 44B's pair-event owner and compose it with relations, guilt, moral, save, and host APIs; no second secret system. |
| `guilt_sources.json` | 40 | `Phase0HostSession` constructs `GuiltInsomniaSystem`, but no production loader binds this catalog; current callers supply source id/severity directly. Plan 49C.20 permits it only as confession metadata. | `RESERVED` | Keep as Plan 66 vocabulary until a Plan 44B confession integration requires a validated source mapping. Do not create another guilt ledger. |
| `item_degradation.json` | 5 | `EquipmentConditionSystem.LoadProfiles` exists, but no production caller loads this file; the host constructs the system with default profiles. | `ROUTED-REPAIR` | Equipment-condition owner must decide whether authored profiles supersede defaults, then bind/load and characterize it in an equipment package. It is not Plan 49 narrative content. |
| `memorials_expansion_05.json` | 27 | The campaign composes `MemorialSystem` from its save owner, but no expansion-catalog loader exists. Plan 49B.13 explicitly asks for its classification. | `ACTIVATE-CANDIDATE, PREREQUISITE-BLOCKED` | A future Plan 49B package must define whether rows are generated/personal memorial variants or world-history records and use the canonical memorial/archive owner. Requires the Plan 42/46 readiness chain. |
| `narrative_encounters_expansion.json` | 29 | `NARRATIVE_ENCOUNTER_RUNTIME_CONTRACT.md` confirms `NarrativeEncounterCatalogLoader` currently loads only the base and NPC-arc files; this expansion file is not loaded. | `ACTIVATE-CANDIDATE, PREREQUISITE-BLOCKED` | Extend the existing narrative loader only after Plan 46 supplies the required reachability/balance evidence; characterize deterministic selection and restore before activation. |
| `phantom_heirlooms.json` | 12 | `HeirloomCatalog`/`HeirloomSystem` are Core-tested, but no production composition or catalog load exists. | `ROUTED-REPAIR` | Plan 41 memory/heirloom host ownership must be reverified and the existing save/host seam completed in a dedicated repair; no parallel heirloom store. |
| `trade_screen_scenarios.json` | 15 | The current trade contract proves rows can be requested explicitly by ID, but records that default settlement, patrol, and debt producers plus precedence are deliberately deferred. | `DECISION-BLOCKED` | Use the documented future producer/precedence decision; do not inject a default scenario merely to improve utilization. |
| `wall_carving_templates.json` | 3 bands / 60 texts | `Plan68WallCarvingTests` explicitly records a consumer-absent catalog; C2 Plan 44B Phase J places it under memorial/mourning/relationship context. | `DEPENDENCY-BLOCKED` | Plan 44B must provide the pair/family/mourning event source and existing decor/memorial projection before any deterministic selection is added. |

### Evidence boundaries

- `ContentUtilizationScanner` currently maps several of these files to
  aspirational owner names. That map is not a production binding and remains a
  useful audit hint only.
- Existing closeout prose that calls the Core-only confession/heirloom systems
  “integrated” conflicts with current host composition. This audit does not
  rewrite the historical closeouts; it routes the missing host path as a
  repair/dependency.
- The scanner's 99 `unresolved` catalogs are outside this audit. They are not
  automatically Plan 49 work and were not promoted into scope.

## Audit conclusion

No verified orphan in the Plan 49 families has a current, deterministic,
save-safe Level-3 consumer path that can be activated without one of the
blocked authorities above. The evidence-only package therefore ends
`DECIDED-DEFERRED`; C1[16] remains `AUDIT-PENDING` rather than being
misreported as either sealed or ready.

## Promotion conditions

C1[16] may be promoted from `AUDIT-PENDING` only after:

1. C2[18]/Plan 42 has a current premise/claim decision, including its identity
   and port-contract prerequisites;
2. C2[20]/Plan 46 has a current premise/claim decision and can produce the
   required reachability evidence; and
3. the Plan 49 candidate packet identifies a canonical consumer and a
   deterministic, save-safe path for every proposed activation.

## Commands and outcomes

| Purpose | Command | Outcome |
|---|---|---|
| Utilization baseline | `godot --headless --path . -- --content-utilization-selftest` | PASS; 619 catalogs, 12,161 definitions, 10 orphan catalogs, 99 unresolved, 4 exempted; one hydroponic deep-chain warning. |
| Runtime/host premise search | `rg` over catalog filenames, loaders, constructors, and host composition in `Assets/` and `src/` | Confirmed the current missing loaders/composition described in the table; no production changes made. |
| Plan/prerequisite recheck | Read C1[16], C2[18], C2[20], census, ownership, test policy, debt, and workflow authorities at current `HEAD` | Plan 42 and Plan 46 cannot be promoted truthfully from their current `AUDIT-PENDING` states. |
| Documentation index | `python3 scripts/ci/generate-docs-index.py --check` (before), then `python3 scripts/ci/generate-docs-index.py` and `--check` | Initial check correctly reported the existing index stale; canonical regeneration indexed 2,493 documents and final check passed. |

## Handoff

- **C1[16]/Plan 49:** remains `AUDIT-PENDING`; no activation, voice delivery,
  formal reachability certification, data edit, or generated-index change was
  performed.
- **Plan 42 / C2[18]:** needs current survivor-identity and port-contract
  decisions before any promotion.
- **Plan 46 / C2[20]:** needs the difficulty/release/onboarding prerequisite
  audit before it can supply formal reachability evidence.
- **Next executable work:** resolve and claim those prerequisite packages, then
  reopen only the Plan 49 rows with a named canonical consumer and bounded
  acceptance test.
