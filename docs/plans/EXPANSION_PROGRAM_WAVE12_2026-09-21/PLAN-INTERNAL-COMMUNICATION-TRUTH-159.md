# PLAN-INTERNAL-COMMUNICATION-TRUTH-159 — Bulletins, Internal Mail & Time Capsules

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RADIO-MEDIA-42, PLAN-PRINT-MEDIA-TRUTH-128, PLAN-FEEDBACK-SURFACE-TRUTH-138.
**Implementation scaffold:** [`PLAN-INTERNAL-COMMUNICATION-TRUTH-159_APPENDIX-A_SCAFFOLD.md`](PLAN-INTERNAL-COMMUNICATION-TRUTH-159_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-PRINT-MEDIA-TRUTH-128` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no broadcast (Plan 42), no press (Plan 128), no notification
system (Plan 138) — this plan is the holdfast's internal record channel.

## 1. Outcome
`Communication/` holds two unclaimed types: `InternalCommunicationSystem.cs`
(host-unreachable, risk score 4 in Plan 1 Appendix L) and
`TimeCapsuleSystem.cs`. Internal communication (bulletins, notices, internal
mail) and time capsules (messages deliberately addressed to the future) are
distinct from radio, print, and transient feedback — and only the first has a
sealed owner named in Plan 1.

| Deliverable | Detail |
|---|---|
| Bulletin model | internal notices with audience (post, role, all), author, day, and retention; distinct from feedback toasts (Plan 138) |
| Internal mail | addressed messages between residents/sites with delivery state; ties to Plan 122's obligations where a promise is stated |
| Time capsules | created with an open day/condition; opening is an event that surfaces the stored record, never generated text |
| Reach truth | who can read a bulletin/mail is derived from role/assignment owners (Plan 141), not a private list |
| Persistence | bulletins, mail, and sealed capsules restore; a capsule cannot open early or re-open |

## 2. Evidence
- `Assets/Ashfall.Core/Communication/InternalCommunicationSystem.cs`, `TimeCapsuleSystem.cs` (verified).
- Plan 1 Appendix A/L: `InternalCommunicationSystem` host-unreachable; score 4 (stateful, no host partial).
- Plans 42/128/138 own the other three channels by explicit boundary.
- Plan 141 owns role/office assignment that audience resolution reads.

## 3. Packages
- **ICT-159A** bulletin model + audience resolution from Plan 141.
- **ICT-159B** internal mail + delivery states; obligation link where applicable.
- **ICT-159C** time capsule lifecycle (create → sealed → open-day/condition → opened) + no-early-open test.
- **ICT-159D** read-record-only rule: no generated text on open.
- **ICT-159E** persistence round-trip for all three record types.

## 4. Acceptance & verification
- Audience resolution matches role assignments; an unaddressed reader cannot see a notice.
- A capsule opens exactly once, on/after its condition, and shows its stored record.
- Save/load preserves sealed state; reloading cannot re-open or advance the day.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Communication/` (create if absent).

## 5. Risks
Channel duplication with 138 → bulletins are durable records, feedback is transient; the boundary table names both.
Early-open exploit → the open condition is stored and validated, not recomputed loosely.

---

## 6. Expanded census (2 files · 969 lines)

Scope: `Assets/Ashfall.Core/Communication/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `InternalCommunicationSystem.cs` | 509 | System | **yes** | 0 | 0 | 2 |
| `TimeCapsuleSystem.cs` | 460 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `nvis_communications_catalog.json` | object[2 keys] |
| `communications_networks.json` | object[5 keys] |
| `communication_templates.json` | object[2 keys] |

**State surfaces:** `InternalCommunicationSystem.cs`, `TimeCapsuleSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Communication/` |
| Test references | 3 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **6**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-ANOMALY-PHANTOM-63` | 1 |
| `PLAN-CREATIVE-WORKS-66` | 1 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `ICT-159A` | no name match — resolve at claim time |
| `ICT-159B` | `InternalCommunicationSystem.cs` |
| `ICT-159C` | `TimeCapsuleSystem.cs` |
| `ICT-159D` | no name match — resolve at claim time |
| `ICT-159E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 6. Host files: **3** · Test files: **4** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/TimeCapsuleHostSession.cs`, `src/Host/TimeCapsuleSelfTest.cs`, `src/Main.TimeCapsule.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Communication/Plan211InternalCommunicationIntegrationTests.cs`, `Ashfall.Core.Tests/Communication/Plan212TimeCapsuleIntegrationTests.cs`, `Ashfall.Core.Tests/Communication/TimeCapsuleSystemTests.cs`, `Ashfall.Core.Tests/Communications/Plan157CommunicationsIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/communications_networks.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `communication` |
| `nvis_communications` |
| `time_capsules` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--real-main-journey-selftest` |
| `--time-capsule-selftest` |
| `--time-capsules-selftest` |

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

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/narrative/pneumatic_carrier_capsule_logs.json` |
| `Assets/StreamingAssets/Data/time_capsules.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (3 files, 17 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Communication` | 3 | 17 |

**Verdict:** 17 cases sit under matching regions — run those first (`Communication`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **5**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/TimeCapsuleHostSession.cs` |
| `src/Host/TimeCapsuleSaveStore.cs` |
| `src/Host/TimeCapsuleSelfTest.cs` |
| `src/Main.TimeCapsule.cs` |
| `src/UI/TimeCapsulePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `communication` | no |
| `time_capsules` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(CODEX_ONLY 1).

| Catalog | Classification |
|---|---|
| `narrative/pneumatic_carrier_capsule_logs.json` | CODEX_ONLY |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

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
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 5 · catalogs 4 · test regions 1 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INTERNAL-COMMUNICATION-TRUTH-159
wave: 12
status: PROPOSED — foreman claim required
packages: ICT-159A, ICT-159B, ICT-159C, ICT-159D, ICT-159E
claim paths:
  - src/Host/TimeCapsuleHostSession.cs  # §19 candidate host surface
  - src/Host/TimeCapsuleSaveStore.cs  # §19 candidate host surface
  - src/Host/TimeCapsuleSelfTest.cs  # §19 candidate host surface
  - src/Main.TimeCapsule.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/communication_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/pneumatic_carrier_capsule_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Communication/
  - godot --headless --path . -- --real-main-journey-selftest
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
