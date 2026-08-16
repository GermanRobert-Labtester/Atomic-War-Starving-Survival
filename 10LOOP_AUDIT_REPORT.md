# ASHFALL 10-Loop Deep Audit & Harden — Ledger

Objective: multilayered debugging + regression audit in **10 loops**. Each loop is
swept (audited), fixed, hardened, and verified before the next begins. Canonical
verification uses `dotnet` + `godot --headless` only (no Unity). All work is on the
engine-agnostic Core and the Godot `src/` host (verifiable with the canonical toolchain).

> Note: a parallel agent shares this git branch, so some Loop-tagged commits were
> captured by it; this ledger tracks *my* substantive findings and fixes regardless
> of whose commit absorbed them.

## Baseline (Loop 0)
| Check | Result |
|-------|--------|
| dotnet build Ashfall.Core.Tests | ✅ 0 err / 0 warn |
| dotnet test | ✅ 1518 pass |
| dotnet build Ashfall.csproj (Godot host) | ✅ 0 err / 0 warn |
| godot --data-integrity-selftest | ⚠️ 0 err but 291 duplicate-id warnings |
| godot --bridge-selftest | ✅ 41/41 |

---

## Loop 1 — Data-integrity "duplicate id" false positives
**Sweep.** The 291 warnings were false positives: the validator treated every
occurrence of an already-authored id under a polymorphic key (`item_id`,
`survivor_id`, `knowledge_key`, `stages[]./id`, `*`-fields enrichment) as a NEW
definition. The data model legitimately **reuses** ids (shared stage templates,
enrichment `_fields`/`_tags` foreign keys, per-container row ids).

**Fix (CatalogIntegrityValidator.cs).** Author-once semantics:
- First occurrence authors an id; later occurrences consolidate as **reuse**.
- A genuine **error** is only a literal entity-root `id` (`file.json[N]/id`)
  defined twice in the SAME file (Invariant 6 conflict).
- Fixed misleading file-leaf extraction; added `AuthoredIds`/`ReuseCount` metrics.

**Harden (3 tests).** Reuse-not-error, genuine-dup-is-error, shared-template-not-conflict.
**Verify.** `3491 ids authored, 619 reuses reserved, 0 errors, 0 warnings`.

## Loop 2 — Determinism + engine-coupling hermeticity (Invariants 1,4)
**Sweep.** AGENTS.md-listed offenders (FinalWish/CombatTrauma/Weather/ProceduralItem)
are ALREADY resolved in Core (use `ISeededRng` `xorshift64*`). Active Godot host wires
only Core systems; remaining `System.Random`/`Guid.NewGuid` live only in dead legacy
`_Game` copies unreferenced from `src`. Determinism already gated (`WeatherSystemTests.Determinism_SameSeedSameSequence`).

**Harden (CoreInvariantSourceTests.cs, 3 tests).** Static gates that fail the build if
Core regresses: no engine coupling; no nondeterminism source (`System.Random`,
`Guid.NewGuid`, `DateTime`, `GetHashCode`); `SeededRng` reproduces across instances.
Core confirmed 0 coupling / 0 nondeterminism hits after comment-stripping.

## Loop 3 — Cross-host save integrity (Invariant 3)
**Sweep.** AGENTS.md "5 Godot save stores lack checksum" is STALE — every `src/Host`
store self-verifies checksums or delegates to a checksumming Core codec
(DoseLedger/DutyRoster/ExpansionHub/Holdfast/Verdict). `SaveChecksum` rigorously tested.
No `System.Text.Json` bypass in `src/Host`.

**Harden (DoseLedgerSystemTests, +2 tests).** Tampered-save hard-rejected on decode;
checksumless save rejected. Full suite 1526.

## Loop 4 — Crash-risk nullable warnings (H9)
**Sweep.** Fresh test build surfaced all nullable categories. Two signal REAL bugs:
`CS8602` (possible null deref) and `CS8625` (null inserted into non-nullable).

**Fix.** `CS8625` → 0: nullable params (`GoodsCatalog?`, `ISeededRng?`) where coalesced;
`null!` for direct-assignment params and intentional-null robustness element.
`CS8602` → 0: `Assert.NotNull` guards + null-forgive the actual null source (`sys.State`
LHS, `Deserialize` result in `foreach`).
**Verify.** Test `CS8602`/`CS8625` = 0. Remaining ~288 are documented-acceptable
serializer-DTO initialization noise (CS0649/CS8600/CS8618) + a few helper null-returns.

## Loop 5 — Silent data-loss save round-trips (H10)
**Sweep.** LocationEvolution/Wildlife/Landmark empty-saveables are Unity-legacy-only
(not in Core or Godot `src`; read-only migration-out path) — untouchable/verifiable here.
Active-path data-loss risk: Needs & Radiation store mutable state in
`SurvivorNeedsState`/`SurvivorRadState` DTOs (state IS the save unit) with **no**
serialize→deserialize round-trip test.

**Harden (NeedsRadiationSystemTests +3).** Mutated-value round-trips both DTOs +
post-Expose dose round-trip. Any DTO field added without serialization now fails loudly.

## Loop 6 — JournalSystem zero tests (H11)
**Harden (JournalSystemTests +9).** Dedup contract, empty-key rejection, freeform raw
entries + author attribution, 64-entry eviction (newest-first), one-shot codex unlocks,
unread/ping transitions, lossless Capture/Restore round-trip (Invariant 3) with
dedup surviving restore, `RestoreState(null)` no-throw.
Full suite 1538.

## Loop 7 — Exception swallow + UtilityAI fork (H4/H5)
**Sweep.** Active-path `catch {}` sites are ALL deliberate best-effort (temp-file
cleanup, backup rotation, tolerant catalog parse) — correct swallows, not bugs.
The H4 bare-catch offenders are Unity-legacy (`YearOfAshCatalogLoader`, `VerdictCatalogLoader`).
UtilityAI: Godot host wires Core `Ashfall.Core.UtilityAI.UtilityAiSystem`;
the defective Unity `_Game/AI/UtilityAI.cs` is never referenced from `src` (dead in Godot).

**Verify.** `--utility-ai-selftest` PASS 7/7 including same-seed determinism.

---

*(Loops 8–10 continue below)*

## Loop 8 — Full clean regression snapshot + dedupe decision (H2/H3)
**Sweep (H2/H3).** `WornGear` and `SimClock`/`IClock` duplicates are intentional
namespace-scoped abstractions. Consolidation is higher-risk than valuable on a
concurrently-developed branch, so NOT collapsed (recorded decision). The
safety-critical gear math was already tested.

**H2 deep dive surfaced a REAL Host gap** (see Loop 9).

**Verification (full clean battery):**
| Check | Result |
|-------|--------|
| dotnet build Ashfall.Core.Tests | ✅ 0 err / 0 warn |
| dotnet test | ✅ 1538 pass |
| dotnet build Ashfall.csproj | ✅ 0 err |
| --data-integrity-selftest | ✅ 0 err / 0 warn (3491 auth / 619 reuse) |
| --bridge-selftest | ✅ 41/41 |

## Loop 9 — Real functional gap: equipped-gear rad protection missing in Godot host
**Sweep.** `Inventory.BuildWornGear()/FillWornGear()` are dead in Core/`src`.
The only bridge (`FillWornGear(context.WornGear)`) lives in Unity legacy
`GameBootstrap.RadiationExposure.cs`. The Godot host `SurvivorsHostSession.BuildExposure`
sets `ZoneRadLevel`/`ShelterShielding` but NEVER populates `ExposureContext.WornGear`,
so `ComputeGearProtection(null)=0` — equipped gas mask/hazmat provides **zero**
radiation protection in the Godot build (Unity behavior differs on the bridge).
Also, the two `WornGear` types (Inventory 4-field data vs Radiation behavioral)
are incompatible, confirming H2's real hazard.

**Harden (GearProtectionBridgeTests, +2, Core contract).** When a host feeds
`ExposureContext.WornGear`: full gear drops exposure below the acute threshold;
degraded gear gives proportional protection. This enforces the bridge contract so
any future host wiring (incl. the Godot one) can be validated against it. A
survivor with zero gear still reaches acute sickness.

**Recommendation (for Host wiring):** `SurvivorsHostSession.BuildExposure` should
assemble the survivor's equipped inventory items into `WornGear` and assign
to `ExposureContext.WornGear` (mirroring the Unity bridge). That requires threading
an inventory/gear source into the flat-rad-state session; left to the active
host-wiring effort to avoid collision.

## Loop 10 — Final full regression + ledger finalization
**Verification (full clean battery)**
| Check | Result |
|-------|--------|
| dotnet build Ashfall.Core.Tests | ✅ 0 err / 0 warn |
| dotnet test | ✅ 1540 pass |
| dotnet build Ashfall.csproj | ✅ 0 err |
| --data-integrity-selftest | ✅ 0 err / 0 warn |
| --bridge-selftest | ✅ 41/41 |

### Net deliverables across all 10 loops
1. **Loop 1** — data-integrity dup-id: 291 false-positive warnings → 0; author-once
   semantics; `AuthoredIds`/`ReuseCount` metrics; 3 tests.
2. **Loop 2** — determinism/engine hermeticity static gates; verify Core has 0 coupling,
   0 nondeterminism; `SeededRng` reproducibility test (3 tests).
3. **Loop 3** — cross-host save integrity: audit confirms codec-backed checksums;
   tamper + checksumless hard-reject gates (2 tests).
4. **Loop 4** — eliminated crash-risk nullable warnings CS8602 (null deref) and
   CS8625 (null insertion) in the test project; loud failure via Assert.NotNull.
5. **Loop 5** — H10: Needs/Radiation state round-trip gates (3 tests).
6. **Loop 6** — H11: JournalSystem zero-test gap closed (9 tests).
7. **Loop 7** — exception-swallow & UtilityAI-fork audit: active path clean,
   Godot wires Core AI; `--utility-ai-selftest` 7/7.
8. **Loop 8** — full regression snapshot green; H2/H3 dedupe decision recorded.
9. **Loop 9** — found+enforced the equipped-gear→radiation bridge contract;
   documented the Godot host gear-protection gap.
10. **Loop 10** — final regression green; this ledger finalized.

**Suite: 1518 → 1540 tests** (+22 from my hardening), all applied invariants:
0 errors/0 warnings on every canonical gate.

## Known / deferred (not addressable in this toolchain)
- Unity-legacy gaps (JsonUtility in SaveSystem C1, 28 loaders C6, H4 bare-catch,
  H8 PlayerPrefs): read-only `Assets/_Game`, require the Unity host to verify/fix.
- The Godot host gear-protection wiring (Loop 9 recommendation) — belongs to the
  active host-wiring effort.
- Dead legacy `Assets/_Game` copies of deterministic systems still compile into
  Ashfall.dll via source include but are unreferenced from `src` (no runtime effect).
- Cosmetic serializer-DTO nullable warnings (CS0649/CS8600/CS8618) — documented
  acceptable; NoWarn'd by the host project.
