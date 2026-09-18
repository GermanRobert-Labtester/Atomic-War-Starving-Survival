# WAVE 11 PART 2 — TASK B3 IMPLEMENTATION LOG

## C2[12] Plan 34 — Difficulty Authority, Immutable Campaign Completion Record, and Chronicle/Epilogue Projection

### Terminal state: PARTIALLY-SEALED

**Source baseline:** `033df2b7` plus the user-owned dirty worktree, 2026-09-18.\
**Authority revision:** The user explicitly authorized B3 on 2026-09-18. The revision recorded at the head of `C2_planintegration[12].md` permits a named user-level, append-only completion-history store. It supersedes only the historical prohibition on that store. Plan 175 still owns profile rewards, unlocks, and New Game+.

## Dependencies and claims

| Item | State | Evidence |
|---|---|---|
| Plan 19A epilogue authority | SEALED-ELSEWHERE | `EpilogueContextFactory` remains a pure engine-free factory; `Main.Endgame.BuildCampaignOutcomeSnapshot()` supplies its canonical inputs. |
| Campaign completion event | SEALED-ELSEWHERE | `EndgameHostSession` owns the terminal transition and now publishes the already-sealed `CampaignEpilogueReport` as an observation event. |
| Difficulty authority / 34B | UNSEALED | No canonical campaign difficulty ID, preset catalog, or completion difficulty owner exists at current `HEAD`; B3 does not invent one. |
| Chronicle completion-history projection / 34C | UNSEALED | Existing chronicle/epilogue surfaces continue to own ending presentation; no plan-defined completion-history read model or panel was present to extend safely. |
| Part 2 claim | ACTIVE | `claim-wave11-part2-execution-2026-09-18` in `WORKTREE_OWNERSHIP.md`. |

## Premise reconciliation

| C2[12] axis | Existing state | Executed B3 delta | Final state |
|---|---|---|---|
| Ending selection | Plan 19 canonical epilogue factory and host projection | None. The recorder takes the emitted ending ID and frozen `EpilogueContextInputs`; it contains no ending branch logic. | SEALED-ELSEWHERE |
| Immutable completion history | Absent | `CampaignCompletionHistoryService` derives a semantic record, validates its prior chain, deduplicates the same completed run, then appends a checksummed record. | SEALED-HERE |
| Cross-campaign persistence | No existing completion store | `CompletionHistoryStore` persists atomically at `user://completion_history.json`, outside every campaign save slot. | SEALED-HERE |
| Observation-only recording | Endgame was already authoritative | Main observes `CampaignSealed` after the terminal report, saves the terminal campaign state, then appends history. No campaign state, ending, reward, or RNG is changed by the recorder. | SEALED-HERE |
| Tamper evidence | `SaveChecksum` is the current checksum discipline | Canonical semantic record payloads are individually checksummed and hash-linked to their predecessor. Invalid history is reported and never silently rewritten as valid. | SEALED-HERE |
| Difficulty record | No current canonical source | Deliberately omitted rather than stored as a display string or inferred from unrelated mechanic difficulty values. | UNSEALED / 34B |
| Chronicle projection | Existing ending surfaces only | No second ending/history presentation owner added. A future 34C consumer must read this history through its read API after its contract is current. | UNSEALED / 34C |

## Ownership and record contract

- `EpilogueContextFactory` remains the sole ending-context authority.
- `CampaignCompletionHistoryService` is an engine-free append/validate/query domain service; its public surface has no update, replace, or delete operation.
- `CompletionHistoryStore` is the user-level persistence adapter. Missing storage is an empty history; malformed or checksum-invalid storage remains invalid and is not auto-healed.
- The completion identity uses the existing profile ID, slot ID, and campaign seed together with canonical terminal semantic facts. It is deliberately not a mutable campaign display name or a save-manifest generation ID (which changes on every save).
- The store is application-contract append-only and checksum-validated/tamper-evident. It is not a claim of adversarial anti-cheat or protection against a user who can recompute checksums.

## Files changed

- `Assets/Ashfall.Core/Endgame/CampaignCompletionHistory.cs`
- `Ashfall.Core.Tests/Endgame/CampaignCompletionHistoryTests.cs`
- `src/Host/CompletionHistoryStore.cs`
- `src/Host/CompletionHistorySelfTest.cs`
- `src/Host/EndgameHostSession.cs`
- `src/Host/HostCli.SelfTests.cs`
- `src/Main.Endgame.cs`
- `C-integration-plans/C2_planintegration[12].md`
- `WORKTREE_OWNERSHIP.md`
- `docs/governance/DECISION_REGISTER.md`
- `KNOWN_DEBT.md`
- `INTEGRATION_PLANS.md`
- `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`
- `docs/plans/wave11_part2/C1_DECISION_REGISTER_PASS.md`
- `docs/plans/wave11_part2/C2_CENSUS_REFRESH.md`
- `docs/INDEX.md` (generated through `scripts/ci/generate-docs-index.py`)

## Save / determinism

- No campaign-save section or campaign-save migration was added.
- The history file outlives campaign deletion, replacement, and new-campaign creation.
- An absent user store defaults to an empty history.
- Record semantic payloads and checksum linkage derive from deterministic terminal facts. The recorder does not enter campaign simulation state or alter its fingerprint.
- No wall-clock timestamp, `System.Random`, filesystem enumeration order, or UI text is part of semantic identity.

## Verification

| Command | Purpose | Result |
|---|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/CampaignCompletionHistoryTests.cs` | Record derivation, append-only/idempotence, cross-run identity, checksum-chain tamper detection, serialization, immutable snapshots, deterministic semantics, observation-only canonical input, and no mutation API. | PASS — 9/9 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Endgame/EndgameSystemTests.cs` | Protect the existing terminal authority. | PASS — 9/9 |
| `dotnet build Ashfall.csproj --no-restore` | Core/host compile. | PASS — 0 warnings, 0 errors |
| `godot --headless --path . -- --endings-selftest` | Existing ending coverage plus B3 host-event forwarding and user-store journey: missing store, first append, fresh-store reload, duplicate suppression, same-ending second campaign, and corruption rejection. | PASS — existing endings 11/11; `B3 COMPLETION HISTORY PASS` |

## Governance updates

- Census `C2[12]`: `DECISION-BLOCKED` → `PARTIALLY-SEALED`.
- Decision register records the user-authorized user-level-history boundary as `DEC-20`.
- `DEBT-PLAN34-DIFFICULTY-CHRONICLE-AUTHORITY` tracks the exact 34B/34C remainder without treating it as a reason to reopen the B3 store.
- The integration ledger and Wave 11 C1/C2 handoff documents now distinguish sealed history from the still-unsealed difficulty and chronicle axes.

## Remaining blocker and next queue movement

**Remaining blocker:** A canonical difficulty authority and a plan-defined chronicle/history consumer are absent. B3 must not infer either from display text or mechanic-local tuning.

**Next queue movement:** C2[12]'s completion-history slice is sealed. Its 34B/34C remainder remains queued behind an authority/presentation contract; C2[13] remains the next independently actionable Part 2 corpus node.
