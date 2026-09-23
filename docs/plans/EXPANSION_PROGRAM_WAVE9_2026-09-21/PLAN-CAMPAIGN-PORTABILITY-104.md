# PLAN-CAMPAIGN-PORTABILITY-104 — Campaign Export/Import for Support & Sharing

**Wave 9 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SAVE-GOVERNANCE-12, PLAN-SAVE-MIGRATION-CORRIDOR-87, PLAN-TELEMETRY-PRIVACY-58.
**Non-goals:** no cloud service, no network upload, no account system, no
cross-version guarantee beyond the Plan 87 policy.

## 1. Outcome
Support and community sharing need a **single-file campaign export**: envelope
+ sections + a manifest of versions, verifiable by checksum, re-importable into
the same or a newer build per the migration policy. Today saves exist per slot
(`SaveStore`, `SaveSlotService`, `SaveEnvelopeHelper` checksums) but there is no
portable artifact, no scrub step, and no import validation path.

| Deliverable | Detail |
|---|---|
| Export artifact | one file (e.g. `.ashfall-campaign`) with envelope, section blobs, and a plain-text manifest (build, section versions, day count, checksum) |
| Import validation | checksum verify, version check against Plan 87 policy, typed refusal reasons — never a half-imported campaign |
| Privacy scrub | manifest excludes machine paths and user names; recorder/archive files are never bundled (Plans 58/91 consent applies) |
| Quarantine import | an import lands in a new slot; it never overwrites the active slot |
| Round-trip proof | export → import → checksum equality across the retained sections |

## 2. Evidence
- `Save/SaveStore.cs`, `Save/SaveSlotService.cs`, `Save/SaveSlotTypes.cs` (`SaveProfileId`, `SaveSlotId`), `Save/SaveEnvelopeHelper.cs` (checksum compute/verify).
- `SaveSectionRegistry`: 204 sections with `SectionFileNames` as the envelope whitelist — the export uses the same whitelist, not a new list.
- Plan 87 owns the version policy the import consults; Plan 58 governs what may be included from telemetry.
- `scripts/run_test.sh` provides the focused harness; no new test framework.

## 3. Packages
- **CPT-104A** artifact format + manifest template (documented, versioned by the envelope).
- **CPT-104B** exporter: whitelist-driven, checksummed, no extra files.
- **CPT-104C** importer: verify → version policy → new-slot write; typed failures.
- **CPT-104D** scrub check: scan the artifact for path/name-shaped strings; fail if found.
- **CPT-104E** round-trip + corrupt-import tests (truncated file, flipped byte, wrong version).

## 4. Acceptance & verification
- Round-trip checksum equality on a scripted save; corrupted artifacts refused with the failing check named.
- Scrub scan finds no machine paths or profile names in a fixture export.
- Import never overwrites an existing slot (asserted).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/`.

## 5. Risks
Export becoming a save-format fork → it is the envelope copied, with the registry whitelist, not a new codec.
Privacy leak via telemetry files → they are outside the whitelist by construction; the scrub check verifies.

---

## 6. Expanded census (4 files · 677 lines)

Scope: `Assets/Ashfall.Core/Save/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampaignEnvelopeBuilder.cs` | 116 | Support | **yes** | 0 | 0 | 0 |
| `CampaignSaveEnvelope.cs` | 180 | Support | — | 0 | 0 | 2 |
| `SaveEnvelopeHelper.cs` | 320 | Support | **yes** | 0 | 0 | 0 |
| `SchemaVersionedEnvelope.cs` | 61 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `campaign_epilogues.json` | object[2 keys] |
| `propaganda_campaigns.json` | object[3 keys] |

**State surfaces:** `CampaignSaveEnvelope.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Save/` |
| Test references | 24 name references across the test tree |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 4. Other plans referencing them: **9**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-SAVE-MIGRATION-CORRIDOR-87` | 4 |
| `PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98` | 4 |
| `PLAN-SAVE-PREVIEW-METADATA-114` | 4 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-DETERMINISM-CROSS-HOST-89` | 1 |
| `PLAN-SAVE-SLOT-UX-105` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `CPT-104A` | `SchemaVersionedEnvelope.cs`, `CampaignEnvelopeBuilder.cs`, `CampaignSaveEnvelope.cs` |
| `CPT-104B` | no name match — resolve at claim time |
| `CPT-104C` | `SchemaVersionedEnvelope.cs` |
| `CPT-104D` | no name match — resolve at claim time |
| `CPT-104E` | `SchemaVersionedEnvelope.cs` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **119** · Test files: **23** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 119 | `src/Host/AirlockSecuritySaveStore.cs`, `src/Host/AmphibiousDraisineSaveStore.cs`, `src/Host/AmputationSaveStore.cs`, `src/Host/AnomalyHazardSaveStore.cs`, `src/Host/ApprenticeshipSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 23 | `Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryStateTests.cs`, `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`, `Ashfall.Core.Tests/Host/HostSessionStateSemanticsTests.cs`, `Ashfall.Core.Tests/Journeys/EndToEndPlayerJourneyTests.cs` |
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

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **2**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (32 files, 187 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |

**Verdict:** 187 cases sit under matching regions — run those first (`Campaign`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Main.Campaign.cs` |
| `src/Main.CampaignOwners.cs` |
| `src/Main.CampaignServices.cs` |
| `src/Main.UiTests.RealCampaignJourney.cs` |
| `src/UI/MainMenuBuilder.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 7 · catalogs 2 · test regions 1 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CAMPAIGN-PORTABILITY-104
wave: 9
status: PROPOSED — foreman claim required
packages: CPT-104A, CPT-104B, CPT-104C, CPT-104D, CPT-104E
claim paths:
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - src/Main.Campaign.cs  # §19 candidate host surface
  - src/Main.CampaignOwners.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/mod_manifest_schema.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
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
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
