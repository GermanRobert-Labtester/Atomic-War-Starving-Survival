# WAVE 11 PART 2 — TASK B4 IMPLEMENTATION LOG

## C2[13] Plan 36 — Port Contracts, Host Wiring Validation, and Unbound-Effect Closure

### Terminal state: PARTIALLY-SEALED (Plan 36A/36B SEALED; Plan 36C long-tail sweep tracked as debt)

**Source baseline:** `033df2b7` plus the active Wave 11 Part 2 claim and user worktree, 2026-09-18.\
**Execution scope:** Plan 36A (Core port contract vocabulary, authoritative policy classification, CI generator, test dequarantine, failure proofs) + Plan 36B (Host session contracts, wiring reporter, runtime validator, host CLI selftest registration). Plan 36C (long-tail sweep of deferred seams) is explicitly deferred to future feature waves.

---

## Dependencies and claims

| Item | State | Evidence |
|---|---|---|
| Part 2 claim | ACTIVE | `claim-wave11-part2-execution-2026-09-18` in `WORKTREE_OWNERSHIP.md`. |
| Core engine-free rule | ENFORCED | `Assets/Ashfall.Core/Ports/PortContract.cs` contains pure domain C#; tested via `PortContractGateTests.CorePorts_AreEngineFree`. |
| Seam policy classification | SEALED | `docs/ci/port_contract_policy.json` classifies all 248 active Core public Bind/Wire/Register/Configure seams. |
| CI contract generator | SEALED | `scripts/ci/generate-port-contract.py` validates policy, parses source, and generates `PORT_CONTRACT.md` and `port-contract.json`. |
| CI gate registration | SEALED | Added `port_contract_gate` to fast gates in `docs/ci/CI_GATE_MANIFEST.json` (53 total gates, 50 fast gates). |
| Runtime wiring reporter | SEALED | `src/Host/HostSessionContracts.cs` defines `IWiringReporter`, `HostWiringValidator`, and structured reporter contracts; wired on `CombatHostSession`. |
| Host CLI selftest | SEALED | `--port-contract-selftest` registered in `HostCliRegistry.cs`, `HostCli.cs`, `Main.Application.cs`, and `SELFTEST_MANIFEST.json`. |
| Gate dequarantine | SEALED | `Ashfall.Core.Tests/Tooling/PortContractGateTests.cs` restored with 8/8 passing tests including 3 proof-of-failure assertions; Ticket #47 CLOSED. |

---

## Premise reconciliation

| Plan 36 requirement | Audit state (B4 log) | Executed delta (36A/36B) | Final state |
|---|---|---|---|
| One classified seam inventory | 144 rows, stale entries, 105 untracked seams | Reconciled Ticket #47: purged stale `DoseLedgerSystem.ConfigureLadder` and `TradeSpecialtySystem.BindToCrafting`, classified all 248 active Core seams (176 `HOST_REQUIRED`, 34 `LIVE_VIA_CORE`, 21 `TEST_ONLY`, 17 `DEFERRED`). All 176 `HOST_REQUIRED` verified with active callers in `src/`. | SEALED |
| Static CI contract gate | Absent; selftest not registered | Created `scripts/ci/generate-port-contract.py` with `--check` mode; registered as fast gate `port_contract_gate` in `CI_GATE_MANIFEST.json`. | SEALED |
| Core-side `PortContract` vocabulary & generated artifact | No `PortContract.cs` or generated manifest | Implemented engine-free `Assets/Ashfall.Core/Ports/PortContract.cs`; generated `docs/architecture/PORT_CONTRACT.md` and `docs/architecture/port-contract.json`. | SEALED |
| Runtime host wiring report & session validation | No `HostSessionContracts` or `IWiringReporter` | Created `src/Host/HostSessionContracts.cs` with `IWiringReporter`, `HostWiringValidator`, structured table output, and machine metrics (`HOST_SESSIONS`, `HOST_WIRING_REQUIRED`, `HOST_WIRING_BOUND`, `HOST_WIRING_MISSING`, `HOST_FALLBACKS_ACTIVE`); implemented on `CombatHostSession`. | SEALED |
| Gate failure proof | `PortContractGateTests.cs` quarantined/absent | Restored `PortContractGateTests.cs` (8/8 tests pass) with explicit proof-of-failure tests (`ProofOfFailure_StaleHostRequiredFails`, `ProofOfFailure_ExpiredDeferredFails`, `ProofOfFailure_UnclassifiedSeamFails`). Removed `Compile Remove` from project file. | SEALED |
| Explicit exemption debt/ratchet | `DEFERRED` lacked shrink-only enforcement | All 17 `DEFERRED` rows carry dated expiry (2026-Q4/2027-Q1); gate asserts zero active `src/` callers (preventing silent degradation); tracked in `KNOWN_DEBT.md`. | SEALED |

---

## Architecture and contract details

### 1. Engine-Free Core Vocabulary (`Assets/Ashfall.Core/Ports/PortContract.cs`)
- `PortRequirement`: `Mandatory`, `OptionalWithFallback`, `Deferred`.
- `PortLifecycleStage`: `BootRegistration`, `SessionInit`, `SessionSwap`, `TearDown`.
- `[PortContractAttribute]`: Machine-readable metadata for domain ports (`Name`, `Requirement`, `Description`, `FallbackType`).
- `PortContractDefinition`: Pure record model describing declaring system, method name, stage, and requirement.
- `PortValidationResult` & `IPortValidator`: Decoupled validation interface avoiding engine references.

### 2. Policy and Ratchet (`docs/ci/port_contract_policy.json`)
- Exactly 248 Core public Bind/Wire/Register/Configure seams tracked:
  - **176 `HOST_REQUIRED`**: Core hooks that MUST be wired by the host. 100% verified to have active call sites in `src/`.
  - **34 `LIVE_VIA_CORE`**: Seams wired within Core domain orchestration (e.g. system coordinators, composite fixtures).
  - **21 `TEST_ONLY`**: Seams used exclusively in unit/integration test fixtures (e.g. mock injectors, test resets).
  - **17 `DEFERRED`**: Historical or future seams with active expiry dates; CI gate strictly asserts no `src/` caller exists without reclassifying to `HOST_REQUIRED`.

### 3. Runtime Host Contracts (`src/Host/HostSessionContracts.cs`)
- `IWiringReporter`: Interface for host sessions to report their collaborator wiring status.
- `HostWiringValidator`: Validates all registered sessions and produces formatted status tables and Plan 36 metrics:
  - `HOST_SESSIONS`: Active registered host sessions.
  - `HOST_WIRING_REQUIRED`: Total required collaborator slots across sessions.
  - `HOST_WIRING_BOUND`: Bound collaborator slots.
  - `HOST_WIRING_MISSING`: Unbound required slots (causes selftest failure).
  - `HOST_FALLBACKS_ACTIVE`: Slots using allowed fallbacks.
- Integrated into `CombatHostSession` and registered via `Main.Expeditions.cs` / cleaned up in `Main.Lifecycle.cs`.

### 4. Host CLI and Selftest
- `--port-contract-selftest` registered in `HostCliRegistry.cs`, `HostCli.cs`, and `Main.Application.cs`.
- Validates:
  1. Authoritative policy schema, expiry dates, and static `src/` caller integrity.
  2. Runtime registered host sessions and their collaborator wiring.
- Emits standard `HostCli.EmitSummary` for CI scrapers and exits 0 on success.
- Manifest regenerated: `docs/ci/SELFTEST_MANIFEST.json` contains 123 cataloged selftests.

---

## Files changed and created

- `Assets/Ashfall.Core/Ports/PortContract.cs` (Created: pure engine-free port contracts)
- `Assets/Ashfall.Core/HostCliRegistry.cs` (Modified: registered PortContractSelfTest descriptor)
- `docs/ci/port_contract_policy.json` (Modified: refreshed to 248 seams, resolved Ticket #47)
- `scripts/ci/generate-port-contract.py` (Created: CI contract generator and gate)
- `docs/architecture/PORT_CONTRACT.md` (Generated: human-readable port contract specification)
- `docs/architecture/port-contract.json` (Generated: machine-readable port contract manifest)
- `docs/ci/CI_GATE_MANIFEST.json` (Modified: added `port_contract_gate`, bumped fast gates to 50)
- `docs/ci/SELFTEST_MANIFEST.json` (Regenerated: 123 tests cataloged)
- `src/Host/HostSessionContracts.cs` (Created: runtime host session contracts and wiring reporter)
- `src/Host/CombatHostSession.cs` (Modified: implemented `IWiringReporter`)
- `src/Host/PortContractSelfTest.cs` (Modified: enhanced with policy check, host wiring check, metrics emission)
- `src/Host/HostCli.cs` (Modified: added `--port-contract-selftest` dispatch)
- `src/Main.Application.cs` (Modified: registered selftest handler)
- `src/Main.Expeditions.cs` (Modified: registered `CombatHostSession` with `HostWiringValidator`)
- `src/Main.Lifecycle.cs` (Modified: unregistered session on combat reset)
- `Ashfall.Core.Tests/Tooling/PortContractGateTests.cs` (Created: 8 gate test cases with proof of failure)
- `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` (Modified: removed `Compile Remove="Tooling/PortContractGateTests.cs"`)
- `docs/remediation/tickets/PORT_CONTRACT_GATE_REQUARANTINE.md` (Modified: CLOSED / RESOLVED)
- `KNOWN_DEBT.md` (Modified: `DEBT-PLAN36-PORT-CONTRACT-CLOSURE` updated to PARTIALLY-SEALED)
- `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (Modified: updated C2[13] execution evidence)
- `INTEGRATION_PLANS.md` (Modified: recorded Plan 36A/36B sealed status)
- `docs/plans/wave11_part2/B4_PLAN36_IMPLEMENTATION_LOG.md` (Created: this implementation log)

---

## Save and determinism

- No changes to save state structures or save versions.
- All port contracts bind deterministically during host composition and session lifecycle events.
- Core remains 100% engine-free; no delegates, closures, or engine references are persisted.

---

## Verification

| Command | Purpose | Result |
|---|---|---|
| `python3 scripts/ci/generate-port-contract.py --check` | Verify policy validity, parse Core seams, and verify generated artifacts match. | PASS — 248 seams checked, 0 errors |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PortContractGateTests.cs` | Validate policy integrity, caller existence, absence of DEFERRED callers, engine-freedom, and 3 failure proofs. | PASS — 8/8 tests (882 ms) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Host/SelfTestManifestGateTests.cs` | Verify selftest manifest parity and registration. | PASS — 4/4 tests |
| `bash scripts/run_test.sh Ashfall.Core.Tests/HostCliHelpContractTests.cs` | Verify Host CLI help output and flag contracts. | PASS — 2/2 tests |
| `dotnet build Ashfall.csproj --no-restore` | Core & host compilation check. | PASS — 0 warnings, 0 errors |
| `bash scripts/ci/run-godot-bounded.sh --path . -- --port-contract-selftest` | Execute runtime host CLI selftest in headless Godot session. | PASS — `PORT_CONTRACT_SELFTEST PASS` |

---

## Governance updates

- Census `C2[13]`: Status remains `PARTIALLY-SEALED`; recorded completion of Plan 36A/36B.
- Ticket #47 (`PORT_CONTRACT_GATE_REQUARANTINE.md`): Status changed to `CLOSED / RESOLVED`.
- `KNOWN_DEBT.md`: `DEBT-PLAN36-PORT-CONTRACT-CLOSURE` updated from `PROMOTED` to `PARTIALLY-SEALED`. Plan 36C tracked for long-tail sweep.
- `INTEGRATION_PLANS.md`: Updated to document Plan 36A/36B sealed status.

---

## Remaining work and next queue movement

- **Plan 36C (Long-tail sweep):** As future feature waves touch the 17 `DEFERRED` systems (e.g. `ShelterRadioStationSystem.BindWeatherNoiseProvider`, `InsarDeformationSystem.BindSurfaceDeformation`), their seams will either receive real host bindings and transition to `HOST_REQUIRED` or be retired.
- **Next queue movement:** Task B4 (Plan 36A/36B) is complete. The next candidate in the Wave 11 Part 2 queue or Wave 12 can be addressed per foreman direction.
