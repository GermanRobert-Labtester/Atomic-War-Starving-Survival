# WAVE 11 PART 2 — TASK B4 IMPLEMENTATION LOG

## C2[13] Plan 36 — Port Contracts, Host Wiring Validation, and Unbound-Effect Closure

### Terminal state: PARTIALLY-SEALED

## Premise reconciliation

| Plan 36 requirement | Current source | Final state |
|---|---|---|
| One classified seam inventory | `docs/ci/port_contract_policy.json` contains 144 rows: 105 `HOST_REQUIRED`, 14 `DEFERRED`, 11 `LIVE_VIA_CORE`, 14 `TEST_ONLY` | PARTIALLY-SEALED |
| Static CI contract gate | `src/Host/PortContractSelfTest.cs` reads the policy and performs name-based `src/` caller checks; it is not registered as a host CLI flag or CI gate | OPEN |
| Core-side `PortContract` vocabulary and generated artifact | No `Assets/Ashfall.Core/Ports/PortContract.cs`, `scripts/ci/generate-port-contract.py`, or generated manifest exists | OPEN |
| Runtime host wiring report and boot validation | No `HostSessionContracts`, `IWiringReporter`, or aggregate session-swap validation exists | OPEN |
| Gate failure proof | `Ashfall.Core.Tests/Tooling/PortContractGateTests.cs` is absent and remains excluded in the project file; remediation ticket records stale classifications | OPEN |
| Explicit exemption debt/ratchet | `DEFERRED` policy rows carry expiry dates, but the selftest checks wall-clock expiry and does not prove host binding or require a shrink-only baseline | PARTIALLY-SEALED |

## Evidence bundle

- Historical scope read: `C2_planintegration[13].md` requires one engine-free vocabulary, generated static gate, runtime validation, boot/session-replacement validation, and a long-tail sweep.
- Policy audit: all 144 recorded `file_path` values resolve, but source presence is not proof of a live host binding.
- Existing test proof: `HostPortContractTests.cs` verifies `CombatHostPorts` local null/required semantics only; it is not a corpus-wide host-wiring validator.
- Quarantine: `docs/remediation/tickets/PORT_CONTRACT_GATE_REQUARANTINE.md` names stale `HOST_REQUIRED` classifications and requires a refreshed policy before re-enablement.
- Runtime attempt: `godot --headless --path . -- --port-contract-selftest` booted with normal user-data access but reported `--port-contract-selftest` as unrecognized, so it did not execute the test. The run also exposed an unrelated `EconomyDetailPanel` scene-binding failure in the existing dirty worktree; neither result validates Plan 36.

## Scope / ownership decision

Implementing Plan 36 now would introduce a shared Core contract, generator, CI manifest, host composition model, and session lifecycle validation across many live owners. That is the plan-defined cross-repository consolidation, not a safe incidental delta. It is promoted as `DEBT-PLAN36-PORT-CONTRACT-CLOSURE`; no provider API was rewritten and no false “bound” classification was inferred from grep.

## Save / determinism

No production behavior changed. The future contract must remain engine-free, bind deterministically during composition, and rebind on New Game/Load rather than serializing delegates.

## Verification

| Command | Result |
|---|---|
| Policy/path audit | 144/144 policy file paths present; classifications recorded above |
| `godot --headless --path . -- --port-contract-selftest` | Did **not** execute the requested selftest: CLI flag is unrecognized after boot; unrelated UI scene-binding exception reported |

## Governance updates

- Census `C2[13]`: `AUDIT-PENDING` → `PARTIALLY-SEALED`.
- Debt: `DEBT-PLAN36-PORT-CONTRACT-CLOSURE` promoted.
- No production or test source changed.

## Next queue movement

Create one bounded Plan 36A/36B contract package before attempting the remaining 36C sweep. C2[13] must not be marked SEALED until the new preventive gate can fail and all in-scope seams receive a real host/neutral/retired classification.
