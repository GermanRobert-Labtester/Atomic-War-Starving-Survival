# PLAN-READINESS-AUDITOR-284 — Readiness auditor and index regeneration

**Wave 20 · Kind:** CLAIM-READINESS CLOSURE · **Status:** PROPOSED — foreman claim required.
**Depends on:** `PLAN-READINESS-PACKAGE-IDS-281`,
`PLAN-READINESS-VERIFICATION-CONTRACT-282`,
`PLAN-READINESS-HEADER-NORMALISATION-283`;
`docs/ci/CI_GATE_MANIFEST.json` (57 existing gates) for wiring.
**Non-goals:** no production code (`Assets/Ashfall.Core/`, `src/`,
`Ashfall.Core.Tests/` stay untouched); no new CI gate without the gate manifest
being updated through its owning generator; no claim to be authoritative over
`INTEGRATION_PLANS.md` or `WORKTREE_OWNERSHIP.md`.

## 1. Outcome

One tool, committed under the programme's `tools/`, reproduces every §24
readiness verdict and regenerates `CLAIM_READINESS_INDEX.md` byte-for-byte, so
the index and the plans cannot drift apart. The tool's own tests include
fixtures for the four false-negative classes this programme already hit, making
the detector's blind spots regression-tested rather than discovered by hand.

## 2. Premise evidence (re-verified 2026-09-21)

Four detector false-negative classes were found while finalising:

| Class | Cause | Example | Reported → true |
|---|---|---|---|
| Package IDs | rule knew two formats, missed letter+digit prefixes | `### C4-1 — …` | 6 → **3** |
| Verification | rule knew three families, missed `scripts/ci/*.py --check` and `*.sh` gates | `PLAN-DOC-ATLAS-CURRENCY-115` | 5 → **0** |
| Wave header | rule knew one form | `**Wave:** 3 (2026-09-21) · **Kind:** …` | 20 → **0** |
| Coupling count | phrase alternation missed "those names" | `PLAN-CORE-ROOT-FAMILY-262` (32 incoming) | hubs reported as free starts |

Additionally, one index value was produced by a miscounted counter and later
corrected (§20–§23 pass). All five corrections are recorded in `EVIDENCE.md`
under rounds 79–85.

## 3. Packages

- ****284A** — `claim-readiness-audit.py`.** Reproduce the §24 checks:
  status/wave/depends/non-goals/outcome/evidence/packages/acceptance/risks/
  verification/§12/§13, plus the coupling count, class, readiness verdict, and
  the five gap classes. Two modes: `--report` (human table) and `--check`
  (exit non-zero on any real gap; prints the gap class and file).
- ****284B** — Index generator.** `generate-claim-readiness-index.py` emits
  `CLAIM_READINESS_INDEX.md` deterministically from the plans and the repository
  indexes (host/test/data/registry joins), with `--check` for CI. The index
  becomes a generated artifact, not a hand-written one.
- ****284C** — Fixtures for the four classes.** A `fixtures/` directory with one
  minimal plan per false-negative class (digit-prefix package, CI-gate command,
  alternate wave header, "those names" coupling phrase). The test asserts the
  auditor counts each as **ready** — the regression that would have caught every
  error this wave was created to fix.
- ****284D** — Gate wiring proposal.** Propose a single report-only gate entry
  via the existing `docs/ci/CI_GATE_MANIFEST.json` generator (57 gates today),
  promoted to blocking only after two clean cycles. Do not hand-edit the
  manifest.

## 4. Acceptance and verification

| Check | Command | Pass condition |
|---|---|---|
| Reproduces verdicts | `python3 …/tools/claim-readiness-audit.py --report` | 276 plans; counts match §24 text |
| Index is generated, not drifting | `python3 …/tools/generate-claim-readiness-index.py --check` | exit 0, no diff |
| Fixtures pass | `bash scripts/run_test.sh <fixture region>/` or the tool's own `--self-test` | 4/4 classes pass |
| Real gaps only | `claim-readiness-audit.py --check` | 0 after 281 lands; before that, exactly the 3 package gaps |
| No production touched | `git status --short -- Assets/Ashfall.Core src Ashfall.Core.Tests` | unchanged (473 pre-existing dirty files preserved) |
| Docs index green | `python3 scripts/ci/generate-docs-index.py --check` | green |

## 5. Risks

- **Tool becomes authority.** The auditor reports; it does not approve. Its
  output must never be read as a claim, and it must not write to
  `INTEGRATION_PLANS.md` or `WORKTREE_OWNERSHIP.md`.
- **Heuristic joins masquerading as truth.** Token joins miss synonyms. Keep the
  "not a conclusion" language in every generated section, and never let
  `--check` fail on a semantic judgement (only on structural facts).
- **CI churn.** A new gate perturbs the manifest and can break unrelated runs.
  Mitigation: report-only first, promote after two clean cycles, wire through the
  manifest's generator.
- **Fixture rot.** Fixtures encode today's formats; if formats change, fixtures
  must change with the contract. Each fixture names the contract rule it pins.

---

## 12. Cross-plan coupling
Document-artifact incoming edges: **279**.

| Plan | Mentions |
|---|---:|
| `PLAN-INTEGRATION-KIT-02` | 3 |
| `PLAN-LAUNCH-FACE-06` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-UNBLOCK-03` | 3 |
| `PLAN-DATA-AUTHORITY-14` | 3 |
| `PLAN-DETERMINISM-REPLAY-13` | 3 |
| `PLAN-RUNTIME-PERF-16` | 3 |
| `PLAN-DATA-CONSUMER-22` | 3 |

---

## 13. Authority binding map
Host files: **46** · Test regions: **1** · Data catalogs: **3**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 46 | `src/Host/HostCli.Plans122to125.cs`, `src/Host/HostCli.Plans139_141.cs`, `src/Host/HostCli.Plans162_165.cs`, `src/Host/HostCli.PlansB86_B89.cs`, `src/Host/HostCli.SelfTestManifest.cs` |
| Test regions | 1 | `Integration` |
| Data catalogs | 3 | `Assets/StreamingAssets/Data/mod_manifest_schema.json`, `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json`, `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

---

## 14. Save-section ownership
Matching section keys: **0** (versioned ladders: **0**).

| Section key |
|---|
| — | no section key shares a token with this scope |

---

## 15. CLI and selftest coverage
Matching flags: **5**.

| Flag |
|---|
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--selftest-manifest` |
| `--test-manifest` |

---

## 16. Event-route reachability
Events sharing a scope token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this scope |

---

## 17. Data-catalog binding
Matching catalogs: **3**.

| Catalog | Classification |
|---|---|
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` | n/a |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` | n/a |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` | n/a |

---

## 18. Test-region mapping
Matching regions: **1** (16 files, 74 cases). Root-level files outside regions: 560 files / 5731 cases.

| Region | Files | Cases |
|---|---:|---:|
| `Integration` | 16 | 74 |

---

## 19. Host-surface route map
Matching host files: **46**.

| Host file |
|---|
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |
| `src/Host/HostCli.Plans162_165.cs` |
| `src/Host/HostCli.PlansB86_B89.cs` |
| `src/Host/HostCli.SelfTestManifest.cs` |
| `src/Host/Plans130To133HostSessions.cs` |
| `src/Host/Plans74To77HostSessions.cs` |
| `src/Host/UniqueClaimSaveStore.cs` |
| `src/Main.Plans110_113.cs` |
| `src/Main.Plans122to125.cs` |

---

## 20. Save schema ladder
Matched sections: **0**; laddered: **0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this scope |

---

## 21. Determinism / RNG stream binding
Matching seeded streams: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this scope |

---

## 22. Content-consumption classification
Matching catalogs: **3**.

| Catalog | Classification |
|---|---|
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` | n/a |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` | n/a |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` | n/a |

---

## 23. Flag wiring
Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this scope |

---

## 24. Claim readiness
**Readiness:** READY-WITH-NOTES (11/12) · **Class:** governance/closure · **Coupling:** 279

**Proposed claim block**

```
plan: PLAN-READINESS-AUDITOR-284
wave: 20
status: PROPOSED — foreman claim required
packages: author at claim time
claim paths:
  - docs/plans/EXPANSION_PROGRAM_WAVE20_2026-09-21/  # this plan
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Integration/
  - godot --headless --path . -- --plans-122-125-balance-soak
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
