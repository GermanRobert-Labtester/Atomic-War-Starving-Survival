# PLAN-PLAYER-COMMAND-TRUTH-131 — Command Envelope, Preview Fidelity & Action Log

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-HOST-CLI-CONTRACT-86, PLAN-SILENT-FAILURE-35, PLAN-SAVE-GOVERNANCE-12.
**Implementation scaffold:** [`PLAN-PLAYER-COMMAND-TRUTH-131_APPENDIX-A_SCAFFOLD.md`](PLAN-PLAYER-COMMAND-TRUTH-131_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-SILENT-FAILURE-35` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new UI framework, no command bus replacing existing event
seams, no panel-local validation.

## 1. Outcome
`PlayerCommand/` already defines the vocabulary — `PlayerCommandCode`,
`CommandContext`, `CommandPreview`, `CommandResult`, `CampaignActionLog` — but
nothing states the contract: that a preview matches the eventual result, that a
refusal names a code rather than throwing, and that the action log is a **log**
(replayable facts) rather than a second source of truth.

| Deliverable | Detail |
|---|---|
| Envelope rule | every player-originated mutation enters through a command context; a direct mutation outside it is a defect with a named file |
| Preview fidelity | for each command code, preview and result agree on the fields they share; a mismatch test exists per code |
| Refusal codes | each code maps to a typed reason and a player-readable message (Plan 138 can render them); no raw exception text |
| Action log truth | the log records facts (code, day, subject ids, outcome) and never feeds simulation; an independence test proves absence changes nothing |
| Coverage table | command codes ↔ the systems that dispatch them; an unused code is either wired or explicitly retired |

## 2. Evidence
- `Assets/Ashfall.Core/PlayerCommand/`: `CampaignActionLog.cs`, `CommandContext.cs`, `CommandPreview.cs`, `CommandResult.cs`, `PlayerCommandCode.cs` (verified).
- Plan 86 owns the CLI-side contract; this plan owns the player-side envelope.
- Plan 35 owns silent-failure removal; refusals here must be typed, not swallowed.
- Plan 138 renders refusal messages from the code table.

## 3. Packages
- **PCT-131A** command code census + coverage table.
- **PCT-131B** envelope enforcement note + scan for direct mutation paths (report, not rewrite).
- **PCT-131C** preview↔result fidelity tests per code.
- **PCT-131D** refusal code table + message wiring into Plan 138's catalog.
- **PCT-131E** action-log independence test (log on/off → same checksum).

## 4. Acceptance & verification
- Every dispatched code appears once in the coverage table; fidelity tests pass per code.
- A refusal returns a code and renders player text; no stack trace reaches a panel.
- Log-off runs match log-on checksums.
- `bash scripts/run_test.sh Ashfall.Core.Tests/PlayerCommand/` (create the region if absent).

## 5. Risks
Envelope becoming ceremony → codes are already the app's own vocabulary; this plan verifies, not replaces.
Log as authority → independence test is permanent.

---

## 6. Expanded census (4 files · 296 lines)

Scope: `Assets/Ashfall.Core/PlayerCommand/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CommandContext.cs` | 29 | Support | — | 0 | 0 | 0 |
| `CommandPreview.cs` | 101 | Support | — | 0 | 0 | 0 |
| `CommandResult.cs` | 109 | Support | **yes** | 0 | 0 | 0 |
| `PlayerCommandCode.cs` | 57 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/PlayerCommand/` |
| Test references | 6 name references across the test tree |
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

Domain files: 4. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-FEEDBACK-SURFACE-TRUTH-138` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `PCT-131A` | `CommandContext.cs`, `CommandPreview.cs`, `CommandResult.cs` |
| `PCT-131B` | no name match — resolve at claim time |
| `PCT-131C` | `CommandPreview.cs`, `CommandResult.cs` |
| `PCT-131D` | no name match — resolve at claim time |
| `PCT-131E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **13** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 13 | `src/Host/AirlockSecurityHostSession.cs`, `src/Host/CaregivingHostSession.cs`, `src/Host/ChemicalDependencyHostSession.cs`, `src/Host/CombatHostSession.cs`, `src/Host/CraftingHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/CommandContractTests.cs`, `Ashfall.Core.Tests/CraftingCommandTests.cs`, `Ashfall.Core.Tests/PlayerCommand/ProseSuccessInferenceSourceGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **1**; isolated: **3**.

| From | → To |
|---|---|
| `CommandResult` | `CommandPreview` |

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

Events whose name shares a domain token: **5**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (33 files, 188 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Campaign` | 32 | 187 |
| `PlayerCommand` | 1 | 1 |

**Verdict:** 188 cases sit under matching regions — run those first (`Campaign`, `PlayerCommand`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Main.Campaign.cs` |
| `src/Main.CampaignOwners.cs` |
| `src/Main.CampaignServices.cs` |
| `src/Main.UiTests.RealCampaignJourney.cs` |
| `src/Muster/FactionActionPanel.cs` |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 7 · catalogs 1 · test regions 2 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PLAYER-COMMAND-TRUTH-131
wave: 11
status: PROPOSED — foreman claim required
packages: PCT-131A, PCT-131B, PCT-131C, PCT-131D, PCT-131E
claim paths:
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - src/Host/CampaignDaySaveStore.cs  # §19 candidate host surface
  - src/Main.Campaign.cs  # §19 candidate host surface
  - src/Main.CampaignOwners.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/
  - godot --headless --path . -- --campaign-journey-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
