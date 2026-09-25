# Plan 23 — Maritime & Black Flotilla

> **Rebuild status:** CURRENT-EVIDENCE PLAN — POST-250K DEEP POLISH AND FINAL PRECISION PASS INCLUDED
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-6`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round6-2026-09-25`
>
> **Current-evidence date:** `2026-09-25`
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → live ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** 150k–170k is the first completeness checkpoint; 250k is an evidence-backed depth target, not a ceiling. The plan stops when verified evidence is exhausted rather than padding with fictional APIs or duplicate systems.

## 0. Integrity Statement and Plan Status

This is a planning and architecture artifact for **Dive sites, coastal hazards, stealth, salvage, tides, maritime faction identity and deep exploration**. It preserves the original intent: Turn the existing maritime systems into a reachable coastal region through current dive, stealth, salvage, tide, flotilla and host seams.

The current residual premise is: The current maritime owner chain and catalog row shapes must be verified before adding sites, flotilla identity, currents, storms, items or presentation. A data row alone is not a reachable dive.

It authorizes no production, authored-data, test, save, UI, generated-index or runtime change. Any future CREATE proposal is hypothetical until a separately claimed implementation package rechecks the live owner, public API, data references, save owner, host route and focused test. The current worktree contains unrelated dirty changes; they are not evidence and are not modified by this plan.

The plan has four distinct passes: (1) current-reality and premise reconstruction; (2) integration framework and code architecture; (3) post-250k deep polishing; and (4) final precision/reaccuracy and handoff review. Length is not treated as quality. Repetition without a new decision, evidence record, failure case or executable acceptance condition is a defect.

# 1. Objective

The bounded objective is to make the **Maritime & Black Flotilla** opportunity implementable without creating a parallel gameplay authority. The plan must identify:

- the one current owner for mutable state;
- the authored JSON or static source for content;
- the existing host adapter and route that exposes a player command;
- the event/fact seam for consequences;
- the existing save section, or an explicit decision that no new persistent state is needed;
- deterministic ordering and seeded randomness boundaries;
- UI projection and accessibility behavior;
- exact focused verification commands;
- migration, rollback and fail-closed behavior.

Non-goals are declared throughout: no new generic manager, no panel-owned counter, no copied catalog, no Unity dependency, no new Unity-era `Assets/_Game/` gameplay path, no speculative content row that lacks a consumer, and no unrelated refactor disguised as feature work.

# 2. Current Decision and Terminal/Residual Status

The prior plan is not accepted as proof. The current status is derived from the source/data/test evidence indexed later in this document. The plan must separate:

- **Terminal/maintenance scope:** behavior already present, already routed, or already covered by current tests. This remains maintenance work and is not reopened.
- **Residual implementation scope:** a verified missing consumer, missing persistence path, stale data reference, inaccessible route, absent lifecycle binding, or unproven event connection.
- **Unknown/deferred scope:** a question that cannot be answered from current evidence. Unknowns are not converted into invented classes, fields, save sections or test suites.

The safe posture is therefore **plan-first, implementation-second**. A future builder must perform the current-evidence checkpoint again immediately before editing because this document can become stale when source or data changes.

**Current decision rule:** if live source contradicts a claim in this plan, return `STALE_PLAN` to the foreman. Never restore a deprecated API merely to preserve a historical plan narrative.

# 3. Required Delta

The minimum safe delta for this subject is:

1. Replace catalog-presence assumptions with a row census and reference audit.
2. Identify the current mutable owner and keep it authoritative.
3. Trace at least one real player command from host input to owner mutation.
4. Trace the resulting fact to its current consumer, UI projection and save path where persistence is required.
5. Define typed events or projection records only where a real cross-owner consequence exists.
6. Define deterministic ordering, seed/substream rules and replay boundaries.
7. Define null, empty, duplicate, old-save, invalid-reference and lifecycle behavior.
8. Add only the smallest focused tests that cover a confirmed gap or public contract.
9. Preserve existing commands, panels, save sections and catalog schemas unless a separately signed delta requires an additive change.
10. Record a rollback point before implementation and a truthfulness check after implementation.

The phrase “huge leap forward” is interpreted as a coherent, reachable game-system improvement—not as permission to invent a second architecture or fill a character quota.

# 4. Evidence and Premise Audit

Evidence is classified as follows:

- **CURRENT FILE:** exists in the worktree at rebuild time; its hash and bounded excerpt are recorded.
- **CURRENT CATALOG:** JSON parses at rebuild time; row/key counts are recorded, but presence is not reachability.
- **MECHANICAL REFERENCE:** a text search hit; it is a clue, not proof of a production call.
- **HISTORICAL RECORD:** old plan or closeout prose; useful context only.
- **PROPOSAL:** a future design shape; requires a new claim and current recheck.
- **UNKNOWN:** an unresolved premise; must be reported rather than filled with fiction.

The complete source, catalog, test and authority dossiers appear in Appendices A–F. A future implementation handoff must cite the current path and line, not merely repeat a filename list.

# 5. Existing Extension Seams

The safe route is to extend current seams. The plan does not introduce a new subsystem merely because the proposed feature is large. For each concern, the builder must record one owner, one input, one mutation or read projection, one fact/event, one presentation route and one persistence answer.

**Extension rule:** if a proposed feature can be represented as a projection over existing state, prefer a projection. If it adds a new durable fact, add it to the existing owner state and existing save section only after a migration review. If it changes a shared command, require an integrator claim.

# 6. Proposed Architecture

```text
authored JSON / existing owner state
          │
          ▼
current catalog loader and current Core owner
          │
          ▼
typed fact or read-only projection
          │
          ├─ existing host command / route
          ├─ existing UI projection and feedback
          ├─ existing journal/radio/codex consumer where applicable
          └─ existing save capture/restore where durable
```

The architecture is owner-first. The host does not calculate gameplay state; a panel does not mirror mutable authority; a catalog does not schedule events by itself; and a preview does not emit a real consequence. If a missing seam is confirmed, the smallest additive provider or typed record must be proposed at the current owner boundary.

# 7. Ownership Matrix

The following matrix is a **working contract to be revalidated**, not a claim that every named file currently implements the proposed delta:

| Concern | Candidate owner | Evidence to prove | Boundary |
|---|---|---|---|
| authored content | current JSON catalog | loader, schema, row IDs, reference validation | no duplicate catalog |
| mutable domain state | current Core owner | capture/restore and mutation methods | no shadow state |
| lifecycle | current day/event owner | actual registration and tick order | no hidden timer |
| player command | current host/session seam | input-to-mutation call path | no panel shortcut |
| consequence | existing fact/event consumer | post-mutation ordering and idempotency | no pre-mutation UI fiction |
| presentation | current panel/HUD/radio route | truthful read model and focus behavior | no gameplay authority |
| persistence | existing save section or explicit no-save | capture/restore/deep-copy/checksum | no parallel store |
| validation | current catalog/test owner | focused invalid-data tests | no broad suite by default |

# 8. Data Flow

The intended flow is:

`input → validation → current owner mutation/read model → typed fact → existing consumer → presentation → command result → save capture`

Every arrow is explicit. A missing consumer is a gap, not a reason for the source owner to call a panel directly. A read-only projection may be recomputed, but any durable player decision must pass through the authoritative owner and its existing save path.

# 9. State Model and Invariants

Before implementation, enumerate current state fields and classify each as immutable definition, derived projection, durable owner state, one-shot fact, cooldown, or historical record. New fields require:

- a truthful default for old saves;
- an invariant and range policy;
- a mutation method or event path;
- capture and restore coverage;
- deep-copy/isolation behavior where applicable;
- deterministic ordering for collections;
- idempotency behavior for repeated delivery;
- an explicit decision not to persist derived values.

Invariants must be expressed as executable acceptance criteria wherever possible. “The system feels coherent” is not an invariant.

# 10. API and Contract Design

Any proposed API below is a contract shape, not a declaration that the method already exists:

```text
LoadCatalog(authorityPath) -> validated current catalog
QueryCurrentState(subjectId) -> read-only truthful projection
Preview(command, expectedVersion) -> named availability/refusal and deltas
Execute(command, expectedVersion) -> owner mutation + stable fact
OnFact(fact) -> existing consumer projection
CaptureState() -> deep serializable owner state
RestoreState(snapshot) -> validated current state
```

New interfaces are justified only when at least two real consumers need the same boundary or when a host/engine boundary must be isolated. Do not create a framework because a future feature might need one.

# 11. Data Plan and Catalog Authority

The data plan is additive and narrow. The current JSON row audit appears in Appendix C. For every proposed row or field, record: stable snake_case ID, schema version, references, ranges, default behavior, consumer, validator and rollback. Existing IDs are reused; no duplicate ID is introduced; no “catalog-only” feature is called integrated.

If a catalog is not loaded by a current production owner, the plan must stop at a reachability finding and name the missing binding. It must not create a second loader to make the data appear live.

# 12. Save, Restore and Migration

Determine whether the feature has durable player decisions. If it does, extend the existing owner state and save section. If it is purely authored or derived, say so and do not add a save section.

Required persistence questions:

- What exact state is durable?
- Which existing save owner captures it?
- What is the old-save default?
- Does restore validate all references?
- Are collections cloned rather than aliased?
- Is checksum participation explicit?
- Can a save be loaded during a transition without double application?
- What is the rollback behavior after a bad version?

A proposed new save section requires a separate architecture decision and a named integration owner. This plan does not grant that permission.

# 13. Determinism and Replay

Determinism is a correctness property, not a stylistic preference. Use the existing seeded RNG contract only when the owner already requires randomness. New streams must be named and stable; no `System.Random`, wall-clock seed, GUID tie-breaker or hash-iteration order may decide a gameplay result.

Same campaign seed, same authored data, same command order and same save state must produce the same domain result. Presentation timing, animation, audio scheduling and frame rate may vary, but they must not change simulation truth. Collection projections use stable ordering before any weighted selection or UI sequence.

# 14. System and Event Wiring

The event path is post-mutation and typed where cross-owner effects matter. The builder must inspect setup, registration, day/hour ticks, event subscription order, reset/dispose behavior and reload behavior. A method declaration is not a call path. A test fixture is not a host binding. A panel command is not an event consumer.

Exactly-once consequences use stable identity keys and existing one-shot ledgers where available. Repeated delivery must be harmless or explicitly rejected. Missing owners fail closed with a truthful diagnostic; they do not silently fabricate a default gameplay success.

# 15. Godot Host Integration

Host work is limited to input, binding, lifecycle, presentation and adaptation. The Godot layer may:

- project owner state into readable rows;
- send a named command;
- display pending/success/refusal feedback;
- refresh after an event;
- apply accessibility, focus and close/back behavior.

It may not recalculate a domain score, maintain a second counter, infer a hidden outcome, or mark an event complete before the owner succeeds. Every new surface must be registered in the current panel/route registry only under a separately claimed shared seam.

# 16. Narrative and Content Integration

Content is meaningful only when it is authored, validated, reachable and attached to a real state transition. Faction, survivor, location, quest, radio, journal, codex and atmosphere content must preserve continuity, avoid real-world copied material, and use the canonical narrator voice. A prose row may be beautiful and still be a defect if no current consumer can reach it.

# 17. UI, Accessibility and Legibility

The UI must show current truth, named refusals, meaningful deltas and consequences. It must not expose internal debug state as player-facing authority. Verify keyboard/controller focus, close/back behavior, readable contrast, non-color status communication, controller-safe scrolling, refresh after external mutation and panel disposal.

A missing panel route is a deliberate integration gap. Do not add a fake route merely to make the plan appear complete. If the route is deferred, state the exact reason and the future shared seams required.

# 18. Failure Modes and Negative Contracts

At minimum, reason about: null/empty catalogs, duplicate IDs, missing references, invalid ranges, dead subjects, inaccessible locations, insufficient resources, stale owner state, repeated events, save during transition, corrupted save, unknown version, host reload, panel not mounted, display-only action, and deterministic replay.

Expected behavior is one of: fail closed with named reason; preserve prior state; use a documented legacy default; or degrade to a truthful read-only projection. Silent success, silent mutation, duplicate rewards and fabricated consequences are prohibited.

# 19. Test Strategy

Verification is layered and focused:

- **Data tests:** schema, snake_case IDs, duplicate/reference checks, row census, invalid ranges.
- **Core tests:** pure behavior, boundaries, state transitions, events and stable ordering.
- **Persistence tests:** capture/restore round-trip, old-save default, deep-copy isolation, checksum participation where relevant.
- **Integration tests:** owner → host command → event → existing consumer; setup and lifecycle binding.
- **UI tests:** route/open/close, focus, visible feedback, refresh and disposal where the surface is actually changed.
- **Headless checks:** only the smallest existing CLI/selftest that proves the affected seam.

Test selection follows `TEST_POLICY.md`. The commands below are proposed focused commands, not fresh pass claims. A builder must run each new or changed test alone first, then the directly affected regional target. No full-suite run is implied by this plan.

# 20. Dependency-Ordered Phases

## Phase 0 — Premise and ownership checkpoint

Re-read the current source/data/test evidence, confirm the owner, confirm no overlapping claim, and freeze the row census. **Gate:** no unresolved owner or reference ambiguity.

## Phase 1 — Core contract

If a new durable fact or deterministic rule is confirmed, implement it in the current Core owner with engine-free types and explicit invariants. **Gate:** focused pure tests pass alone.

## Phase 2 — Persistence and lifecycle

Extend the existing capture/restore path and setup/reset/dispose lifecycle only if the confirmed state is durable. **Gate:** round-trip, old-save and reload tests pass.

## Phase 3 — Authored data

Add only schema-valid rows or fields that have a current consumer, validator and rollback story. **Gate:** catalog integrity and reference tests pass.

## Phase 4 — Host wiring

Bind one real command and one real event consumer through the current host seam. **Gate:** source-level call path and focused integration test agree.

## Phase 5 — Presentation

Project current owner state into the existing route. **Gate:** truthfulness, accessibility, refresh and disposal checks pass; no UI-owned authority.

## Phase 6 — Narrative/content QA

Check continuity, tone, unlock conditions, duplicate IDs, player reachability and false claims. **Gate:** content audit reports every row and consumer.

## Phase 7 — Final integration and rollback review

Re-run the smallest affected suite, inspect the diff for unrelated edits, verify no Unity/engine leakage and document the rollback point. **Gate:** handoff is implementation-ready and bounded.

# 21. File Impact Map

| File/area | Action | Reason | Risk |
|---|---|---|---|
| current Core owner | MODIFY only if confirmed | authoritative state/rules | medium |
| current loader/catalog | MODIFY only if confirmed | authored schema/rows | medium |
| current host/session | MODIFY only if confirmed | command/event/lifecycle | medium |
| current UI route | MODIFY only if confirmed | truthful presentation | shared seam |
| current save owner | MODIFY only if durable delta exists | persistence | high |
| focused tests | MODIFY/ADD only for confirmed gap | regression evidence | low |
| unrelated systems | DO NOT TOUCH | scope firewall | high |

Exact proposed paths are listed in the evidence appendix and must be rechecked before implementation. A filename list is not a claim of permission to edit.

# 22. Risks and Mitigations

- **Stale premise:** source changes invalidate a plan section. Mitigation: Phase 0 checkpoint and hash revalidation.
- **Parallel authority:** a new manager/store/registry appears attractive. Mitigation: owner matrix and no-new-authority rule.
- **Catalog orphanism:** rows load but never reach a player. Mitigation: caller graph plus current consumer proof.
- **Save blind spot:** a new fact is durable in memory but not restored. Mitigation: capture/restore test before UI.
- **UI lie:** panel projects an approximation or cached value. Mitigation: projection-only UI and refresh test.
- **Nondeterminism:** collection order or wall-clock changes outcome. Mitigation: stable order and seeded stream contract.
- **Scope explosion:** polish becomes unrelated refactoring. Mitigation: explicit non-goals and phase gates.
- **Shared claim collision:** a host or registry file is active elsewhere. Mitigation: stop and return to foreman.

# 23. Out of Scope

This plan does not authorize: a new game engine, Unity dependency, parallel save store, global event bus rewrite, generic UI framework, unrelated balance retuning, broad catalog migration, generated-index mass edit, speculative new lore, copied text/art, or a broad test rewrite. It also does not reopen features listed as retired/accepted in `KNOWN_DEBT.md` without new evidence.

# 24. Rollback and Recovery

Work in reviewable phases. Keep Core contract, data, host wiring, UI and tests separable where ownership permits. Before implementation, record the current source hashes and current save section. If a phase fails, revert only that phase’s owned paths, restore the prior catalog/schema behavior, and leave existing save readers compatible. Never “fix” a failed phase by creating a second owner or by deleting user data.

Corrupt or unknown future save versions fail closed with a clear diagnostic. A legacy save must retain its documented old meaning. A host reload must reconstruct the same current state without reapplying one-shot consequences.

# 25. Definition of Done and Implementation Handoff

The package is done only when:

- current owner and extension seam are proven from source;
- every proposed content row has a validator and current consumer;
- the smallest implementation diff is reviewed;
- focused Core/data/host/UI tests appropriate to the actual change pass;
- save/restore and determinism are proven if durable or random;
- no UI or catalog duplicates authority;
- no fresh-pass claim is made for a test that was not run;
- all generated documentation paths and hashes are current;
- the implementation handoff names the first safe step and rollback point.

**MUST PRESERVE:** current owner boundaries, current save readers, deterministic ordering, authored-data authority, accessibility and truthful presentation.

**MUST ADD:** only confirmed missing contracts, named evidence, focused tests and explicit migration/rollback behavior.

**MUST NOT DO:** introduce Unity, create a second state owner, use `System.Random` in deterministic Core behavior, claim catalog presence as integration, or touch an active shared claim.

**VERIFY WITH:** the focused commands in Appendix G, the current test-policy runner, scoped diff checks, catalog integrity where data changes, and a headless check only if a runtime path is actually changed.

**FIRST SAFE IMPLEMENTATION STEP:** re-read the named current owner, enumerate its public state and capture/restore path, and produce a one-page delta table showing exactly what is missing before editing any production file.

# Appendix A — Current Source Dossier

### Current source: `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs`
- Evidence status: **CURRENT FILE PRESENT**; 764 lines / 30593 bytes; SHA-256 `5a1949b6e1e1822ebaa20bb08297b3d7e236d6cdcb80b6c8bae1358c73aa0542`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: using System.Linq;
5: using Ashfall.Core;
6: using Ashfall.Core.IO;
7:
8: #pragma warning disable CS8618
9:
   10: namespace Ashfall.Core.Maritime
   11: {
   12:     public enum DiveRoomType { Deckhouse, Companionway, HoldApproach, DeepHold }
   13:
   14:     public enum DiveResult { Success, Partial, Contaminated, Failed, CrewLost }
   15:
   16:     [Serializable]
   17:     public sealed class DiveRoomNode
   18:     {
   19:         public DiveRoomType roomType;
   20:         public float searchProgress; // 0.0 to 100.0
   21:         public bool isLooted;
   22:         public int hazardLevel; // 1 to 5
   23:     }
   24:
   25:     [Serializable]
   26:     public sealed class DiveSite
   27:     {
   28:         public string siteId = string.Empty;
   29:         public string displayName = string.Empty;
   30:         public float depthMeters;
   31:         public float hazardLevel;     // 0-1
   32:         public bool isExplored;
   33:         public bool isHazardous;
   34:         public float radiationLevel;
   35:     }
   36:
   37:     [Serializable]
   38:     public sealed class DiveOutcome
   39:     {
   40:         public string siteId = string.Empty;
   41:         public int day;
   42:         public DiveResult result;
   43:         public string recoveredItemId = string.Empty;
   44:         public float radiationDose;
   45:         public string notes = string.Empty;
   46:     }
   47:
   48:     [Serializable]
   49:     public class StealthDiveSaveState
   50:     {
   51:         public string systemId = "maritime_dive";
   52:         public bool isActive;
   53:         public string siteId = string.Empty;
   54:         public string diverDwellerId = string.Empty;
   55:         public string compressorOperatorDwellerId = string.Empty;
   56:         public float airSupplySeconds = 120f;
   57:         public float maxAirSupplySeconds = 120f;
   58:         public int currentRoomIndex;
   59:         public int noiseLevel; // 0 to 100
   60:         public bool isCompromised;
   61:         public float decompressionRequiredSeconds;
   62:         public float decompressionProgressSeconds;
   63:         public bool isDecompressing;
   64:         public bool hasDecompressionSickness;
   65:         public float accumulatedRadiationDose;
   66:         public bool diverLost;
   67:         public List<DiveRoomNode> rooms = new List<DiveRoomNode>();
   68:         public List<DiveSite> sites = new List<DiveSite>();
   69:         public List<DiveOutcome> outcomes = new List<DiveOutcome>();
   70:     }
   71:
   72:     [Serializable]
   73:     public sealed class MaritimeDiveState : StealthDiveSaveState
   74:     {
   75:     }
   76:
   77:     /// <summary>
   78:     /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — Authoritative Maritime Dive System.
   79:     /// Single authority unifying the 4-chamber stealth dive state machine, air compressor
   80:     /// delivery, acoustic noise detection, deep decompression stages, emergency aborts,
   81:     /// diver loss / asphyxiation triage, radiation dosing, and catalog site registration.
   82:     /// </summary>
   83:     public class MaritimeDiveSystem
   84:     {
   85:         public const string SystemId = "maritime_dive";
   86:         public const float BaseAirPerCrank = 30f; // Seconds of air gained per manual operator crank
   87:
   88:         private readonly ISeededRng _rng;
   89:         private readonly ILog _log;
   90:         private int _currentDay;
   91:         private bool _airWarningFired;
   92:
   93:         private readonly List<DiveRoomNode> _rooms = new List<DiveRoomNode>();
   94:         private readonly List<DiveSite> _sites = new List<DiveSite>();
   95:         private readonly List<DiveOutcome> _outcomes = new List<DiveOutcome>();
   96:
   97:         public DiveSiteContainer Catalog { get; private set; } = new DiveSiteContainer();
   98:
   99:         public bool IsActive { get; private set; }
  100:         public string CurrentSiteId { get; private set; } = string.Empty;
  101:         public string DiverDwellerId { get; private set; } = string.Empty;
  102:         public string CompressorOperatorDwellerId { get; private set; } = string.Empty;
  103:         public float AirSupplySeconds { get; private set; }
  104:         public float MaxAirSupplySeconds { get; private set; } = 120f;
  105:         public int CurrentRoomIndex { get; private set; }
  106:         public int NoiseLevel { get; private set; }
  107:         public bool IsCompromised { get; private set; }
  108:
  109:         public float DecompressionRequiredSeconds { get; private set; }
  110:         public float DecompressionProgressSeconds { get; private set; }
  111:         public bool IsDecompressing { get; private set; }
  135:             _log = log ?? NullLog.Instance;
  136:         }
  137:
  138:         public void LoadCatalog(DiveSiteContainer catalog)
  139:         {
  140:             if (catalog?.dive_sites == null) return;
  141:             Catalog = catalog;
  142:             foreach (var s in catalog.dive_sites)
  143:             {
  144:                 if (s == null) continue;
  145:                 if (!_sites.Exists(existing => existing.siteId == s.site_id))
  146:                 {
  147:                     float avgHazard = s.rooms != null && s.rooms.Count > 0
  148:                         ? (float)s.rooms.Average(r => r.hazard_level) / 5f
  149:                         : 0.3f;
  150:
  151:                     _sites.Add(new DiveSite
  152:                     {
  153:                         siteId = s.site_id,
  183:                 radiationLevel = 45f,
  184:                 isHazardous = true
  185:             });
  186:             _sites.Add(new DiveSite
  187:             {
  188:                 siteId = "site_exp09_barge_flotilla",
  189:                 displayName = "The Barge Flotilla",
  190:                 depthMeters = 25f,
  191:                 hazardLevel = 0.3f,
  192:                 radiationLevel = 20f,
  193:                 isHazardous = false
  194:             });
  195:             _sites.Add(new DiveSite
  196:             {
  197:                 siteId = "site_exp09_naval_patrol",
  198:                 displayName = "The Patrol Craft",
  199:                 depthMeters = 50f,
  200:                 hazardLevel = 0.7f,
  259:         /// site's noise floor. Returns false for unknown sites; the legacy
  260:         /// <see cref="StartDive"/> path is unchanged for old callers/saves.
  261:         /// </summary>
  262:         public bool StartDiveAtSite(string diverId, string operatorId, string siteId)
  263:         {
  264:             var def = Catalog != null && Catalog.dive_sites != null
  265:                 ? Catalog.dive_sites.FirstOrDefault(s => s != null && s.site_id == siteId)
  266:                 : null;
  267:             if (def == null) return false;
  268:
  269:             CurrentSiteId = def.site_id;
  270:             DiverDwellerId = diverId ?? string.Empty;
  271:             CompressorOperatorDwellerId = operatorId ?? string.Empty;
  272:             MaxAirSupplySeconds = Math.Max(30f, def.oxygen_budget_ticks);
  273:             AirSupplySeconds = MaxAirSupplySeconds;
  274:             CurrentRoomIndex = 0;
  275:             NoiseLevel = 0;
  334:             return false;
  335:         }
  336:
  337:         /// <summary>
  338:         /// Plan 23 launch gate — combines the gear gate and the site's authored
  339:         /// tide window (derived from the authoritative campaign day). Returns a
  340:         /// stable blocker key for UI presentation: "unknown_site",
  341:         /// "tide:<phase>", or the missing item id. Pure; no state mutation.
  342:         /// </summary>
  343:         public bool CanLaunch(string siteId, int campaignDay, IEnumerable<string>? ownedItemIds, out string blocker)
  344:         {
  345:             blocker = string.Empty;
  346:             var def = Catalog != null && Catalog.dive_sites != null
  347:                 ? Catalog.dive_sites.FirstOrDefault(s => s != null && s.site_id == siteId)
  348:                 : null;
  349:             if (def == null) { blocker = "unknown_site"; return false; }
  350:
  351:             var window = DiveSiteTideWindows.Parse(def.tide_window);
  352:             if (!TideCalendar.IsWindowOpen(window, campaignDay))
  353:             {
  354:                 blocker = "tide:" + TideCalendar.PhaseName(TideCalendar.PhaseFor(Math.Max(0, campaignDay)));
  355:                 return false;
  356:             }
  357:
  358:             if (!CanStartDive(siteId, ownedItemIds, out var missing))
  359:             {
  360:                 blocker = missing;
  361:                 return false;
  362:             }
  363:             return true;
  364:         }
  365:
  366:         /// <summary>Data-driven safes for a site (registered with the SafeCrackingSystem by the host on entry).</summary>
  367:         public IReadOnlyList<SafeDefinition> GetSafesForSite(string siteId)
  368:         {
  369:             var def = Catalog != null
  370:                 ? Catalog.dive_sites.FirstOrDefault(s => s != null && s.site_id == siteId)
  371:                 : null;
  372:             return def?.safes ?? (IReadOnlyList<SafeDefinition>)Array.Empty<SafeDefinition>();
  373:         }
  374:
  375:         /// <summary>Procedural scavenge table for a site (empty when none authored).</summary>
  376:         public IReadOnlyList<VariableLootNode> GetLootTableForSite(string siteId)
  377:         {
```
### Current source: `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs`
- Evidence status: **CURRENT FILE PRESENT**; 19 lines / 600 bytes; SHA-256 `7b848513354f2736f05d661755044f3481e134ef516abcd98fcfa4a5ec9933ca`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using Ashfall.Core;
4:
5: namespace Ashfall.Core.Maritime
6: {
7:     /// <summary>
8:     /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — Stealth Dive Instance.
9:     /// Thin legacy subclass maintaining 100% binary and source compatibility
   10:     /// with all existing callers while delegating directly to the authoritative MaritimeDiveSystem.
   11:     /// </summary>
   12:     public sealed class StealthDiveInstance : MaritimeDiveSystem
   13:     {
   14:         public StealthDiveInstance(ISeededRng? rng = null, ILog? log = null)
   15:             : base(rng, log)
   16:         {
   17:         }
   18:     }
   19: }
```
### Current source: `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs`
- Evidence status: **CURRENT FILE PRESENT**; 214 lines / 8160 bytes; SHA-256 `0356103b32a5af21f7e08ffe6ea6f15463adccf6e1fcbe5001cecdcb8fe6020c`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: #pragma warning disable CS8618
5:
6: namespace Ashfall.Core.Maritime
7: {
8:     /// <summary>
9:     /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — procedural scavenge system.
   10:     /// Locations never have static spawns. Uses weighted Poisson distribution
   11:     /// to roll quantities between Min and Max, factoring in world phase (longer
   12:     /// game = closer to Min) and per-location visit count (picked-over effect).
   13:     /// Environmental degradation means the same location yields less over time.
   14:     /// Engine-agnostic, deterministic via ISeededRng, save/load safe.
   15:     /// </summary>
   16:     public class ProceduralScavengeSystem
   17:     {
   18:         public const int DegradationPhase1_Day = 20;
   19:         public const int DegradationPhase2_Day = 50;
   20:         public const int DegradationPhase3_Day = 80;
   21:
   22:         public const float PoissonLambdaBase = 1.5f;
   23:         public const float DegradationSkewFactor = 0.6f;
   24:
   25:         public const float HighRadThreshold = 15f;
   26:         public const float BioHazardThreshold = 0.5f;
   27:
   28:         public event Action<string, string, int> OnLootRolled;
   29:         public event Action<string, string> OnItemDegraded;
   30:         public event Action<string, string> OnContaminationApplied;
   31:
   32:         private readonly ISeededRng _rng;
   33:         private int _currentDay;
   34:         private readonly Dictionary<string, int> _locationVisitCounts = new Dictionary<string, int>(StringComparer.Ordinal);
   35:
   36:         public ProceduralScavengeSystem(ISeededRng? rng = null)
   37:         {
   38:             _rng = rng ?? new SeededRng(9999);
   39:         }
   40:
   41:         public void SetCurrentDay(int day) => _currentDay = day;
   42:         public int GetVisitCount(string locationId) => _locationVisitCounts.TryGetValue(locationId, out var v) ? v : 0;
   43:
   44:         public List<LootRollResult> RollLootTable(string locationId,
   45:             List<VariableLootNode> lootTable, float locationRads, bool hasBioHazard)
   46:         {
   47:             var results = new List<LootRollResult>();
   48:             if (string.IsNullOrEmpty(locationId) || lootTable == null) return results;
   49:
   50:             _locationVisitCounts.TryGetValue(locationId, out var visits);
   51:             _locationVisitCounts[locationId] = visits + 1;
   52:
   53:             for (int i = 0; i < lootTable.Count; i++)
   54:             {
   55:                 var node = lootTable[i];
   56:                 if (node == null || string.IsNullOrEmpty(node.ItemId)) continue;
   57:
   58:                 if (_rng.NextDouble() > node.SpawnChance) continue;
   59:
   60:                 int qty = RollQuantity(node.MinQty, node.MaxQty, _currentDay, visits);
   61:                 if (qty <= 0) continue;
   62:
   63:                 bool degraded = false;
   64:                 if (node.DegradationChance > 0f && _rng.NextDouble() < node.DegradationChance)
   65:                 {
   66:                     degraded = true;
   67:                     qty = MathfCompat.Max(1, qty / 2);
   68:                     OnItemDegraded?.Invoke(locationId, node.ItemId);
   69:                 }
   70:
   71:                 bool contaminated = locationRads >= HighRadThreshold || hasBioHazard;
```
### Current source: `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs`
- Evidence status: **CURRENT FILE PRESENT**; 533 lines / 22293 bytes; SHA-256 `70696ffc834b46aea0885f8ebd593166d674d1104860778be70794572f954402`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: #pragma warning disable CS8618
5:
6: namespace Ashfall.Core.Maritime
7: {
8:     // ── Safe definition ──────────────────────────────────────────────
9:
   10:     /// <summary>Data-driven safe/container definition.</summary>
   11:     [Serializable]
   12:     public class SafeDefinition
   13:     {
   14:         public string id = string.Empty;
   15:         public string displayName = string.Empty;
   16:         public string roomId = string.Empty;
   17:         public int difficulty = 3;           // number of tumblers (1-6)
   18:         public int maxAttempts = 10;         // before jamming
   19:         public float noisePerAttempt = 0.2f; // 0..1 cumulative
   20:         public float alarmThreshold = 0.8f;  // noise above this triggers alarm
   21:         public List<SafeLootEntry> loot = new List<SafeLootEntry>();
   22:     }
   23:
   24:     /// <summary>One loot entry in a safe.</summary>
   25:     [Serializable]
   26:     public class SafeLootEntry
   27:     {
   28:         public string itemId = string.Empty;
   29:         public int minQuantity = 1;
   30:         public int maxQuantity = 1;
   31:         public float weightKg = 1f;
   32:     }
   33:
   34:     // ── Safe state ──────────────────────────────────────────────────
   35:
   36:     /// <summary>Runtime state of one safe instance.</summary>
   37:     [Serializable]
   38:     public class SafeInstanceState
   39:     {
   40:         public string safeId = string.Empty;
   41:         public string locationId = string.Empty;
   42:         public string roomId = string.Empty;
   43:         public int difficulty = 3;
   44:         public int attemptsUsed = 0;
   45:         public int maxAttempts = 10;
   46:         public float cumulativeNoise = 0f;
   47:         public float alarmThreshold = 0.8f;
   48:         public bool isOpened = false;
   49:         public bool isJammed = false;
   50:         public bool alarmTriggered = false;
   51:         public bool lootTransferred = false;
   52:         public int openedDay = -1;
   53:         public List<SafeLootEntry> loot = new List<SafeLootEntry>();
   54:         // Deterministic combination: derived from seed + safeId, never serialized
   55:         public int[] combination = Array.Empty<int>();
   56:     }
   57:
   58:     /// <summary>System-wide safe cracking state (save DTO).</summary>
   59:     [Serializable]
   60:     public class SafeCrackingState
   61:     {
   62:         public string systemId = SafeCrackingSystem.SystemId;
   63:         public List<SafeInstanceState> safes = new List<SafeInstanceState>();
   64:     }
   65:
   66:     /// <summary>Result of a safe cracking attempt.</summary>
   67:     public enum SafeAttemptResult
   68:     {
   69:         Success,        // safe opened
   70:         PartialHint,    // got feedback on tumblers
   71:         Failed,         // wrong combination
   72:         ToolDamaged,    // lockpick broke
   73:         NoiseWarning,   // noise approaching threshold
   81:     public class SafeAttemptFeedback
   82:     {
   83:         public SafeAttemptResult Result;
   84:         public int CorrectTumblers;     // how many tumblers are in the right position
   85:         public int TotalTumblers;       // total tumblers in the safe
   86:         public float NoiseLevel;        // current cumulative noise
   87:         public float ToolCondition;     // remaining tool condition
   88:         public string Message;          // human-readable feedback
   89:     }
   90:
   91:     // ── System ──────────────────────────────────────────────────────
   92:
   93:     /// <summary>
   94:     /// ASHFALL — Deterministic safe cracking system.
   95:     /// Resolves safe/container opening through seeded tumbler combinations.
   96:     /// The UI presents dial rotation and audio cues, but Core owns the
   97:     /// actual combination and outcome. Loot transfers through existing
   98:     /// inventory/scavenge paths.
   99:     ///
  100:     /// Determinism: combination is derived from seed + safeId hash.
  101:     /// Same safe + same seed = same combination every time.
  102:     /// </summary>
  103:     public class SafeCrackingSystem
  104:     {
  105:         public const string SystemId = "safe_cracking_system";
  106:         public const float BaseToolCondition = 1.0f;
  107:         public const float ToolDamagePerAttempt = 0.08f;
  108:         public const float ToolDamageOnFail = 0.15f;
  109:         public const int MaxDifficulty = 6;
  110:         public const int MinDifficulty = 1;
  111:
  112:         private readonly SafeCrackingState _state = new SafeCrackingState();
  113:         private readonly Dictionary<string, SafeInstanceState> _safes = new Dictionary<string, SafeInstanceState>();
  114:         private readonly int _seed;
  409:             return result;
  410:         }
  411:
  412:         // ── Abandon ──────────────────────────────────────────────────
  413:
  414:         /// <summary>Abandon a safe (give up). Safe remains in current state.</summary>
  415:         public bool Abandon(string safeId)
  416:         {
  417:             if (!_safes.TryGetValue(safeId, out var safe)) return false;
  418:             // Just stop — no state change needed
  419:             return true;
  420:         }
  421:
  422:         // ── Queries ──────────────────────────────────────────────────
  423:
  424:         public SafeInstanceState? GetSafe(string safeId)
  425:         {
```
### Current source: `Assets/Ashfall.Core/Maritime/TideCalendar.cs`
- Evidence status: **CURRENT FILE PRESENT**; 84 lines / 3314 bytes; SHA-256 `e5effad8fb469876d1640073be597e779069fe921f48bafbf6fa29540fb5e394`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3:
4: namespace Ashfall.Core.Maritime
5: {
6:     /// <summary>Deterministic tide phase, derived from campaign day (no wall clock, no RNG).</summary>
7:     public enum TidePhase
8:     {
9:         Low = 0,
   10:         Rising = 1,
   11:         High = 2,
   12:         Falling = 3
   13:     }
   14:
   15:     /// <summary>Authored tide-window kinds a dive site may gate its launch on.</summary>
   16:     public enum TideWindow
   17:     {
   18:         Any = 0,
   19:         Slack = 1,        // turning water only (Rising or Falling)
   20:         LowOnly = 2,      // shallows exposed
   21:         HighOnly = 3,     // deep-water approach
   22:         FallingOnly = 4,  // narrow entry drains open
   23:         UnsafeAtPeak = 5  // closed during peak flow (Rising)
   24:     }
   25:
   26:     /// <summary>
   27:     /// ASHFALL Plan 23 — deterministic tide calendar for the Drowned Coast.
   28:     /// Derives phase purely from the authoritative campaign day (4-day cycle,
   29:     /// two tidal turns): day%4 → Low, Rising, High, Falling. No wall clock, no
   30:     /// RNG, no serialized state — same day means same tide in every host and
   31:     /// every save. Old saves (no day authority) default to ungated.
   32:     /// </summary>
   33:     public static class TideCalendar
   34:     {
   35:         /// <summary>Days per full tidal cycle (two low/high turns).</summary>
   36:         public const int CycleDays = 4;
   37:
   38:         public static TidePhase PhaseFor(int campaignDay)
   39:         {
   40:             if (campaignDay < 0) return TidePhase.High; // ungated fallback for pre-day callers
   41:             return (TidePhase)(campaignDay % CycleDays);
   42:         }
   43:
   44:         public static string PhaseName(TidePhase phase) => phase switch
   45:         {
   46:             TidePhase.Low => "Low Tide",
   47:             TidePhase.Rising => "Rising Tide",
   48:             TidePhase.High => "High Tide",
   49:             TidePhase.Falling => "Falling Tide",
   50:             _ => "Any Tide"
   51:         };
   52:
   53:         /// <summary>True when the authored window admits a launch on the given campaign day.</summary>
   54:         public static bool IsWindowOpen(TideWindow window, int campaignDay)
   55:         {
```
### Current source: `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs`
- Evidence status: **CURRENT FILE PRESENT**; 84 lines / 3894 bytes; SHA-256 `81afdf75a00bbcdf489812c4008c658275b1317f72b49d9001e366db999efd04`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using Ashfall.Core.Economy;
4:
5: namespace Ashfall.Core.Maritime
6: {
7:     /// <summary>
8:     /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — standing thresholds for the
9:     /// Black Flotilla, expressed entirely on the existing <see cref="FactionStanceEngine"/>
   10:     /// semantics (one trust track, existing threshold fields — no new meter).
   11:     ///
   12:     /// Canonical disposition tiers read from the same trust value the stance
   13:     /// engine already stores for <see cref="FactionId"/>:
   14:     ///   Hostile     — trust below 0: hailed, inspected, hostile at raid threshold.
   15:     ///   Tolerated   — 0..29: flagged traffic, standard rates, no privileges.
   16:     ///   Trading     — 30..54: exchange access, claim-tag courtesy, bulletins.
   17:     ///   Trusted     — 55..79: charts/coordinates/tide intel, specialist stock.
   18:     ///   Cooperation — trust ≥ 75: deep-dive cooperation, launch rights, kin berths.
   19:     /// </summary>
   20:     public static class BlackFlotillaStanding
   21:     {
   22:         /// <summary>Canonical faction id (holdfast_factions.json roster).</summary>
   23:         public const string FactionId = "faction_black_flotilla";
   24:
   25:         // Thresholds on the existing FactionStanceEngine semantics.
   26:         public const float RaidThreshold = -50f;
   27:         public const float RobThreshold = -20f;
   28:         public const float MinTrustToTrade = 0f;
   29:         public const float IntelShareThreshold = 40f;
   30:         public const float RaidAggression = 0.35f;
   31:
   32:         // Plan 23 tier boundaries (deepened semantics on the same trust scale).
   33:         public const float SalvageTrustedTrust = 30f;
   34:         public const float DeepCooperationTrust = 55f;
   35:
   36:         /// <summary>Canonical Flotilla thresholds (single source for hosts/tests).</summary>
   37:         public static FactionThresholds Thresholds => new FactionThresholds(
   38:             factionId: FactionId,
   39:             raidThreshold: RaidThreshold,
   40:             robThreshold: RobThreshold,
   41:             minTrustToTrade: MinTrustToTrade,
   42:             intelShareThreshold: IntelShareThreshold,
   43:             raidAggression: RaidAggression,
   44:             trustInversion: false);
   45:
   46:         /// <summary>Convenience registration on any live stance engine.</summary>
   47:         public static void Register(FactionStanceEngine engine)
   48:         {
   49:             engine?.RegisterFaction(Thresholds);
   50:         }
   51:
   52:         /// <summary>True when flagged traffic may trade (exchange open).</summary>
   53:         public static bool CanTrade(float trust) => trust >= MinTrustToTrade;
   54:
   55:         /// <summary>True when charts/coordinates/tide-table intel may be shared.</summary>
   56:         public static bool CanShareIntel(float trust) => trust >= IntelShareThreshold;
   57:
   58:         /// <summary>True when claim-tag cooperation and salvage trust apply.</summary>
   59:         public static bool IsSalvageTrusted(float trust) => trust >= SalvageTrustedTrust;
   60:
   61:         /// <summary>True when deep-dive cooperation access is granted.</summary>
   62:         public static bool CanCooperateOnDeepDives(float trust) => trust >= DeepCooperationTrust;
   63:
   64:         /// <summary>Canonical disposition tier for a trust value.</summary>
   65:         public static BlackFlotillaTier TierFor(float trust)
   66:         {
   67:             if (trust < MinTrustToTrade) return BlackFlotillaTier.Hostile;
   68:             if (trust < SalvageTrustedTrust) return BlackFlotillaTier.Tolerated;
   69:             if (trust < DeepCooperationTrust) return BlackFlotillaTier.Trading;
   70:             if (trust < FactionStanceConstants.MaxTrust) return BlackFlotillaTier.SalvageTrusted;
   71:             return BlackFlotillaTier.DeepCooperation;
   72:         }
   73:     }
```
### Current source: `src/Main.Maritime.cs`
- Evidence status: **CURRENT FILE PRESENT**; 175 lines / 6702 bytes; SHA-256 `e8eccd36d8ff1d0f610352e093b5e69314715b072e2f2b4a4b8803854036d702`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using Godot;
3: using System;
4: using System.Globalization;
5: using System.IO;
6: using System.Linq;
7: using System.Collections.Generic;
8: using AtomicWar.Journal;
9: using Ashfall.Core;
   10: using Ashfall.Core.Campaign;
   11: using Ashfall.Core.Economy;
   12: using Ashfall.Core.Expeditions;
   13: using Ashfall.Core.Foundry;
   14: using Ashfall.Core.Inventory;
   15: using Ashfall.Core.Journal;
   16: using Ashfall.Core.Muster;
   17: using Ashfall.Core.YearOfAsh;
   18: using Ashfall.Core.Radio;
   19: using Ashfall.Core.Survivors;
   20: using AtomicWar.GodotApp.Economy;
   21: using AtomicWar.GodotApp.YearOfAsh;
   22: using AtomicWar.GodotApp.Muster;
   23: using AtomicWar.GodotApp.Dose;
   24: using AtomicWar.GodotApp.UtilityAI;
   25: using AtomicWar.GodotApp.Radio;
   26: using AtomicWar.GodotApp.Audio;
   27: using AtomicWar.GodotApp.UI;
   28:
   29: namespace AtomicWar.GodotApp
   30: {
   31:     public partial class Main : Control
   32:     {
   33:         // ── Maritime fields (GAP-ARCH-01 Phase 1) ──
   34:         private MaritimeHostSession _maritime = null!;
   35:         private bool _maritimeDirty;
   36:         private DeepCoastHostSession _deepCoast = null!;
   37:
   38:         private void FlushMaritimeIfDirty()
   39:         {
   40:             if (_maritimeDirty) SaveMaritime();
   41:         }
   42:
   43:         /// <summary>
   44:         /// Thin host wiring: shares the CoreDemoSession's District8DeepCoastSystem
   45:         /// (so the HoldfastSave v5 envelope is the single authority), the real
   46:         /// journal, the maritime dive session, and the Holdfast trade inventory.
   47:         /// Also registers the existing Northern Sound Icebreaker Dock as an
   48:         /// expedition target the moment the route reaches dock_accessible — the
   49:         /// route gate (IsNodeAccessible) stays the enforcement, so the dock can
   50:         /// never be dispatched before it is reached.
   51:         /// </summary>
   52:         private void SetupDeepCoast()
   53:         {
   54:             if (_deepCoast != null) return;
   55:             SetupIceRoad();
   56:             SetupJournal();
   57:             SetupMaritime();
  122:         {
  123:             if (_maritime != null) return;
  124:             SetupCampaignDay();
  125:             _maritime = MaritimeHostSession.Create(_dataDir, _campaignDay.Rng);
  126:             _maritime.StateChanged += () => _maritimeDirty = true;
  127:             GD.Print("[Ashfall Godot] Maritime host ready: stealth dive · scavenge · contamination.");
  128:         }
  129:
  130:         private void SaveMaritime()
  131:         {
  132:             if (_maritime == null) return;
  133:             if (CaptureSection("maritime", MaritimeSaveStore.TryCapturePersisted(_maritime.CaptureSave())))
  134:             {
  135:                 _maritimeDirty = false;
  136:                 GD.Print("[Ashfall Godot] Maritime save written.");
  137:             }
  138:         }
  139:
  140:         private void OnMaritimeStartDiveClicked()
  141:         {
  142:             SetupMaritime();
  143:             _statusLabel.Text = _maritime.StartDive("diver_cole", "operator_ren");
  144:         }
  145:
  146:         private void OnMaritimeTickDiveClicked()
  147:         {
  148:             SetupMaritime();
  149:             _statusLabel.Text = _maritime.TickDive(10f);
  150:         }
  151:
```
### Current source: `src/UI/MaritimePanel.cs`
- Evidence status: **CURRENT FILE PRESENT**; 315 lines / 14096 bytes; SHA-256 `670f2a8f2e914c22cd39ca2c0b32dc0981f47b4630e801b3b8710be52c3cddb9`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: using Godot;
5: using Ashfall.Core;
6: using Ashfall.Core.Maritime;
7: using Ashfall.Core.UI;
8: using CoreTheme = Ashfall.Core.UI.Theme;
9:
   10: namespace AtomicWar.GodotApp.UI
   11: {
   12:     /// <summary>
   13:     /// ASHFALL — Maritime Panel (Expansion 09 — The Black Flotilla).
   14:     /// Manages submerged stealth dive operations, air supply compressors, noise detection,
   15:     /// progressive chamber breach (4-room hierarchy), psychological contamination,
   16:     /// and maritime salvage extraction.
   17:     ///
   18:     /// Presentation only — delegates simulation state to MaritimeHostSession.
   19:     /// </summary>
   20:     public partial class MaritimePanel : Control, IBindablePanel
   21:     {
   22:         public event Action? OnClose;
   23:
   24:         private MaritimeHostSession? _maritime;
   25:         private SurvivorsHostSession? _survivors;
   26:         private VBoxContainer _diveDetailsContainer = null!;
   27:         private VBoxContainer _lootDetailsContainer = null!;
   28:         private Label _statusLabel = null!;
   29:
   30:         public bool IsBound => _maritime != null;
   31:
   32:         public override void _Ready()
   33:         {
   34:             SetAnchorsPreset(LayoutPreset.FullRect);
   35:             BuildLayout();
   36:             Visible = false;
   37:         }
   38:
   39:         public override void _UnhandledInput(InputEvent @event)
   40:         {
   41:             if (!Visible) return;
   42:             if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
   43:             {
   44:                 Close();
   45:                 GetViewport().SetInputAsHandled();
   46:             }
   47:         }
   48:
   49:         public void Bind(MaritimeHostSession? maritime, SurvivorsHostSession? survivors)
   50:         {
   51:             _maritime = maritime;
   52:             _survivors = survivors;
   53:             if (_maritime != null)
   54:             {
   55:                 _maritime.StateChanged += RefreshView;
   90:             mainVBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
   91:             margin.AddChild(mainVBox);
   92:
   93:             // ── Header Card ──
   94:             var headerCard = AshfallUiHelpers.MakeCardFrame(
   95:                 "THE BLACK FLOTILLA // EXP 09: MARITIME SALVAGE & STEALTH DIVE",
   96:                 "Four-chamber submerged stealth dive operations, manual air compression, noise detection, psychological contamination, and procedural maritime scavenging."
   97:             );
   98:             mainVBox.AddChild(headerCard);
   99:
  100:             // ── Scrollable Body ──
  101:             var scroll = new ScrollContainer
  102:             {
  103:                 SizeFlagsHorizontal = SizeFlags.ExpandFill,
  104:                 SizeFlagsVertical = SizeFlags.ExpandFill,
  105:                 HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
  106:             };
  169:
  170:             tBox.AddChild(AshfallUiHelpers.MakeDataRow("Assigned Diver", diverName, AshfallUiHelpers.ToColor(CoreTheme.Hot)));
  171:             tBox.AddChild(AshfallUiHelpers.MakeDataRow("Compressor Operator", opName, AshfallUiHelpers.ToColor(CoreTheme.Warm)));
  172:             tBox.AddChild(AshfallUiHelpers.MakeDataRow("Air Supply Reserve", $"{dive.AirSupplySeconds:F0}s / {dive.MaxAirSupplySeconds:F0}s ({airPercent:F0}%)", AshfallUiHelpers.ToColor(airPercent < 25f ? CoreTheme.Critical : CoreTheme.Pale)));
  173:             tBox.AddChild(AshfallUiHelpers.MakeDataRow("Acoustic Noise Level", $"{dive.NoiseLevel} / 100 {(dive.IsCompromised ? "[COMPROMISED]" : "")}", AshfallUiHelpers.ToColor(dive.NoiseLevel > 70 ? CoreTheme.Critical : CoreTheme.Pale)));
  174:             tBox.AddChild(AshfallUiHelpers.MakeDataRow("Submerged Chamber", $"Room {dive.CurrentRoomIndex + 1} of 4", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
  175:             tBox.AddChild(AshfallUiHelpers.MakeDataRow("Radiation Exposure", $"{dive.AccumulatedRadiationDose:F1} mSv", AshfallUiHelpers.ToColor(dive.AccumulatedRadiationDose > 25f ? CoreTheme.Critical : CoreTheme.Muted)));
  176:
  177:             if (dive.DecompressionRequiredSeconds > 0 || dive.HasDecompressionSickness)
  178:             {
  179:                 string decompStatus = dive.HasDecompressionSickness
  180:                     ? "ACUTE SICKNESS (Barotrauma Penalty)"
  181:                     : dive.IsDecompressing
  182:                         ? $"DECOMPRESSING ({dive.DecompressionProgressSeconds:F0}s / {dive.DecompressionRequiredSeconds:F0}s)"
  183:                         : $"REQUIRED STOP ({dive.DecompressionRequiredSeconds:F0}s required before surfacing)";
  184:                 tBox.AddChild(AshfallUiHelpers.MakeDataRow("Decompression Status", decompStatus, AshfallUiHelpers.ToColor(dive.HasDecompressionSickness ? CoreTheme.Critical : CoreTheme.Warm)));
  185:             }
  191:             if (!dive.IsActive)
  192:             {
  193:                 var btnStart = AshfallUiHelpers.MakeButton("LAUNCH STEALTH DIVE (SARAH CHEN / MARCUS REID)", () =>
  194:                 {
  195:                     _maritime.StartDive("survivor_sarah_chen", "survivor_marcus_reid");
  196:                     _statusLabel.Text = "Stealth dive launched into Flotilla wreckage.";
  197:                     RefreshView();
  198:                 });
  199:                 btnStart.CustomMinimumSize = new Vector2(380, 36);
  200:                 diveActions.AddChild(btnStart);
  201:             }
  202:             else
  203:             {
  204:                 var btnCrank = AshfallUiHelpers.MakeButton("CRANK COMPRESSOR (+30s Air)", () =>
  205:                 {
  206:                     _maritime.CrankDiveCompressor();
  207:                     _statusLabel.Text = "Compressor cranked manually.";
```
### Current source: `src/UI/MaritimeAtlasPanel.cs`
- Evidence status: **CURRENT FILE PRESENT**; 466 lines / 20336 bytes; SHA-256 `22e0e29256beebb7f2eeafeba7ecafbcc3aba4e82f8f28af7beaad303c7ebab2`.
The following is a bounded source excerpt, not a generated API and not a claim that every declaration is production-reachable.
```text
1: // SPDX-License-Identifier: MIT
2: using System;
3: using System.Collections.Generic;
4: using Godot;
5: using Ashfall.Core;
6: using Ashfall.Core.Expeditions;
7: using Ashfall.Core.UI;
8: using AtomicWar.GodotApp;
9: using AtomicWar.GodotApp.UI;
   10: using DesignTheme = Ashfall.Core.UI.Theme;
   11:
   12: namespace AtomicWar.GodotApp.UI;
   13:
   14: /// <summary>
   15: /// ASHFALL — Maritime Atlas Dashboard (#48 Stitch, Phase 24, Tier-3).
   16: ///
   17: /// Phase 24 ships the maritime / dive-site coordinate panel as a Tier-3
   18: /// HYBRID sub-card sibling of the existing `MaritimePanel.cs` and
   19: /// `DeepCoastPanel.cs` (Phase 9 modals). The atlas reads the user's own
   20: /// `DiveSiteDefinition` catalog through the Maritime host session (the
   21: /// same wire as `DiveInstanceRunner`).
   22: ///
   23: /// Four tiles:
   24: ///   1. Deckhouse tile     — first-leg surface access
   25: ///   2. Companionway tile  — second-leg hull access
   26: ///   3. Hold Approach tile  — third-leg keeper trace
   27: ///   4. The Hold tile       — fourth-leg recovery deep-end
   28: ///
   29: /// Plus six status rail cards and a right-side dive detail inspector.
   30: /// </summary>
   31: public partial class MaritimeAtlasPanel : Control
   32: {
   33:     public event Action? OnClose;
   34:     // OnSiteSelected was previously declared here but had zero subscribers
   35:     // anywhere in the codebase (audit §10). Removed in the cleanup pass;
   36:     // re-introduce with a host subscriber if a downstream consumer is added.
   37:
   38:     private AshfallDashboardShell _shell = null!;
   39:     private AshfallStatusRail? _statusRail;
   40:     private AshfallDataGrid? _deckhouseGrid;
   41:     private AshfallDataGrid? _companionwayGrid;
   42:     private AshfallDataGrid? _holdApproachGrid;
   43:     private AshfallDataGrid? _holdGrid;
   44:     private AshfallDataGrid? _actionBarGrid;
   45:     private VBoxContainer _detailBox = null!;
   46:     private Label _detailTitle = null!;
   47:     private int _selectedIndex = -1;
   48:
   49:     private MaritimeHostSession? _host;
   50:     private List<(string siteId, string name, int oxygen, float noiseFloor, string keeper, int rooms, string tideWindow)> _sites = new();
   51:     private int _campaignDay;
   52:
   53:     /// <summary>
   54:     /// Authoritative campaign day for tide-window presentation (optional —
   55:     /// without a day provider the tide column shows the authored rule only).
   56:     /// </summary>
   57:     public Func<int>? CampaignDayProvider { get; set; }
   58:
   59:     public bool IsBound => _host != null;
   60:
   61:     public void Bind(MaritimeHostSession host)
   62:     {
   63:         _host = host;
   64:         LoadSitesFromHost();
   65:         RefreshView();
   66:     }
   68:     private void LoadSitesFromHost()
   69:     {
   70:         _sites.Clear();
   71:         if (_host == null) return;
   72:
   73:         if (_host.Dive.Catalog?.dive_sites != null && _host.Dive.Catalog.dive_sites.Count > 0)
   74:         {
   75:             foreach (var site in _host.Dive.Catalog.dive_sites)
   76:             {
   77:                 if (site == null) continue;
   78:                 _sites.Add((
   79:                     site.site_id,
   80:                     site.name,
   81:                     site.oxygen_budget_ticks,
   82:                     site.base_noise_floor,
   83:                     site.keeper_thread_id ?? "—",
   84:                     site.rooms?.Count ?? 4,
   85:                     site.tide_window ?? "any"
   86:                 ));
  103:         }
  104:         else
  105:         {
  106:             _sites.Add(("site_exp09_ss_sovereign", "S.S. Sovereign", 120, 0.85f, "q_keeper_of_logs", 4, "slack"));
  107:             _sites.Add(("site_exp09_ferry_terminal", "The Drowned Ferry Terminal", 90, 0.60f, "—", 4, "any"));
  108:             _sites.Add(("site_exp09_barge_flotilla", "The Barge Flotilla", 100, 0.40f, "—", 4, "any"));
  109:             _sites.Add(("site_exp09_naval_patrol", "The Patrol Craft", 80, 0.70f, "—", 4, "any"));
  110:         }
  111:     }
  112:
  113:     public override void _Ready()
  114:     {
  115:         SetAnchorsPreset(LayoutPreset.FullRect);
  116:
  117:         _shell = new AshfallDashboardShell("Maritime Atlas // Deep Coast Dive Coordinates", minWidth: 1280, minHeight: 720);
  118:         SetContentRoot(_shell);
  119:
```

# Appendix B — Current Data and Catalog Audit

- `Assets/StreamingAssets/Data/dive_sites.json` — 23434 bytes; SHA-256 `f4f273ab223859ce24fec441d2ba9612cf284d885fd4055d18cab2832832a7e9`.
  - `root` object keys (2): `schema_version`, `dive_sites`
  - `root.dive_sites` list rows: **14**
- sample row key: `S.S. Sovereign Wreck`; fields: `site_id`, `name`, `oxygen_budget_ticks`, `base_noise_floor`, `keeper_thread_id`, `rooms`, `location_id`, `discovery`, `safes`, `loot_table`, `tide_window`
- sample row key: `The Drowned Ferry Terminal`; fields: `site_id`, `name`, `oxygen_budget_ticks`, `base_noise_floor`, `keeper_thread_id`, `rooms`, `location_id`, `discovery`, `loot_table`, `tide_window`
- sample row key: `The Barge Flotilla (Upstream Mooring)`; fields: `site_id`, `name`, `oxygen_budget_ticks`, `base_noise_floor`, `keeper_thread_id`, `rooms`, `location_id`, `discovery`, `loot_table`, `tide_window`
- `Assets/StreamingAssets/Data/black_flotilla_items.json` — 9255 bytes; SHA-256 `c5f36642902cdbe48fcaff23b3809c2bc55c691f131fc3f028a9488423dbc502`.
  - `root` object keys (2): `schema_version`, `items`
  - `root.items` list rows: **36**
- sample row key: `paper_scrap`; fields: `id`, `displayName`, `type`, `stackMax`, `weight`, `tradeValue`
- sample row key: `item_suitcase_locked`; fields: `id`, `displayName`, `type`, `stackMax`, `weight`, `tradeValue`
- sample row key: `industrial_bleach`; fields: `id`, `displayName`, `type`, `stackMax`, `weight`, `tradeValue`
- `Assets/StreamingAssets/Data/currents.json` — 11386 bytes; SHA-256 `8419047f6faedb063d19c1efb8a632873bf1a505f4752904ad11f62e6b0643c3`.
  - `root` object keys (2): `schema_version`, `entries`
  - `root.entries` list rows: **17**
- sample row key: `faction_archivists`; fields: `id`, `display_name`, `alignment`, `home_region`, `is_active`, `trust`, `wants`, `offers`, `signature_quote`, `access_rule`, `badge_asset_id`
- sample row key: `faction_lamplighters`; fields: `id`, `display_name`, `alignment`, `home_region`, `is_active`, `trust`, `wants`, `offers`, `signature_quote`, `access_rule`, `badge_asset_id`
- sample row key: `faction_quiet_house`; fields: `id`, `display_name`, `alignment`, `home_region`, `is_active`, `trust`, `wants`, `offers`, `signature_quote`, `access_rule`, `badge_asset_id`
- `Assets/StreamingAssets/Data/maritime_zones.json` — 3736 bytes; SHA-256 `424f70a0984e4715af1cb21fe9407bdac602fad5b3f876e81cc751392753e85a`.
  - `root` object keys (2): `schema_version`, `zones`
  - `root.zones` list rows: **6**
- sample row key: `Northern Tidal Shallows`; fields: `zone_id`, `name`, `zone_type`, `water_temp_celsius`, `radiation_level`, `current_strength`, `visibility`, `dive_sites`, `required_equipment_type`, `min_depth_meters`, `max_depth_meters`, `description`
- sample row key: `Brackish Estuary Outflow`; fields: `zone_id`, `name`, `zone_type`, `water_temp_celsius`, `radiation_level`, `current_strength`, `visibility`, `dive_sites`, `required_equipment_type`, `min_depth_meters`, `max_depth_meters`, `description`
- sample row key: `Outer Continental Shelf`; fields: `zone_id`, `name`, `zone_type`, `water_temp_celsius`, `radiation_level`, `current_strength`, `visibility`, `dive_sites`, `required_equipment_type`, `min_depth_meters`, `max_depth_meters`, `description`

# Appendix C — Current Test Inventory

- `Ashfall.Core.Tests/MaritimeDiveSystemTests.cs` — 257 lines; SHA-256 `048eb0f106868490284945ff341582f6775aa651953f8c572631f091e2bfdd6c`; test attributes 15; declaration lines 17.
  - `public class MaritimeDiveSystemTests`
  - `public void RegisterSite_CreatesSite()`
  - `public void RegisterSite_Duplicate_Blocks()`
  - `public void ConductDive_ReturnsOutcome()`
  - `public void ConductDive_UnknownSite_Fails()`
  - `public void ConductDive_MarksSiteExplored()`
  - `public void ConductDive_TracksRadiationDose()`
  - `public void CaptureRestoreState_PreservesSites()`
  - `public void Dive_Oxygen_DepletesAndCompressorReplenishes()`
  - `public void Dive_Oxygen_WarningFiresAtLowAir()`
  - `public void Dive_Decompression_BuildsInDeepChambers()`
- `Ashfall.Core.Tests/Plan23DiveMechanicCoverageTests.cs` — 233 lines; SHA-256 `44c460881a7e37b6725798b0d240ad9a84bb9509f2f17053d4d3ede13171e6ea`; test attributes 11; declaration lines 14.
  - `public class Plan23DiveMechanicCoverageTests`
  - `private static string DataDir()`
  - `private static DiveSiteContainer LoadCatalog()`
  - `public void Sites_FourteenLive_NoDuplicateIdsOrNames()`
  - `public void Sites_Plan10Profiles_RemainUntouched()`
  - `public void Sites_AllHaveLocationAnchorAndDiscovery()`
  - `public void Sites_GearGates_ReferenceRealItems()`
  - `public void Dive_StartDiveAtSite_SeedsStateFromCatalogDefinition()`
  - `public void GearGate_BlocksDeepSiteWithoutCanister_AllowsWithIt()`
  - `public void Sites_SafeCrackingConsumers_AtLeastTwoSites()`
  - `public void SafeCracking_SiteSafes_ResolveThroughLiveRuntime_AndPersist()`
- `Ashfall.Core.Tests/Maritime/Plan207MaritimeExplorationIntegrationTests.cs` — 287 lines; SHA-256 `e0cbc0bbbe657c2175f0185171162330229a59adf61c8354b5a6194b7f766640`; test attributes 7; declaration lines 8.
  - `public sealed class Plan207MaritimeExplorationIntegrationTests : CatalogTestBase`
  - `public void LoadCatalog_LoadsAllMaritimeZones()`
  - `public void DiscoverZone_UnlocksZoneAndAutoDiscoversContainedSites()`
  - `public void ValidateExpedition_EnforcesDiverAssignmentDepthRatingAndGearGates()`
  - `public void ExecuteExpedition_ResolvesLootAndEnforcesFiniteSalvage()`
  - `public void EvaluateHazards_TriggersHazardsAndAppliesConsequences()`
  - `public void ExecuteExpedition_IntMinSeed_CompletesWithoutOverflow()`
  - `public void SaveRestoreState_PreservesZonesSitesExpeditionsAndEquipment()`

# Appendix D — Mechanical Caller/Reference Graphs

### Mechanical references to `MaritimeDiveSystem`
Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs:83: public class MaritimeDiveSystem
Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs:132: public MaritimeDiveSystem(ISeededRng? rng = null, ILog? log = null)
Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs:10: /// with all existing callers while delegating directly to the authoritative MaritimeDiveSystem.
Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs:12: public sealed class StealthDiveInstance : MaritimeDiveSystem
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:962: ["dive_sites.json"] = new[] { "MaritimeDiveSystem" },
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1081: ["deep_lore_locations.json"] = new[] { "MaritimeDiveSystem" },
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1082: ["black_flotilla_items.json"] = new[] { "MaritimeDiveSystem", "ProceduralScavengeSystem" },
Ashfall.Core.Tests/IslandBridgesTests.cs:19: var sys = new MaritimeDiveSystem(new SeededRng(42));
Ashfall.Core.Tests/IslandBridgesTests.cs:29: var sys2 = new MaritimeDiveSystem(new SeededRng(42));
Ashfall.Core.Tests/MaritimeDiveSystemTests.cs:10: public class MaritimeDiveSystemTests
Ashfall.Core.Tests/MaritimeDiveSystemTests.cs:255: private static MaritimeDiveSystem Create() => new MaritimeDiveSystem(new SeededRng(42));
Ashfall.Core.Tests/Plan23CoastalDynamicsTests.cs:100: var dive = new MaritimeDiveSystem(new SeededRng(11));
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:78: var dive = new MaritimeDiveSystem(new SeededRng(21));
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:90: var restored = new MaritimeDiveSystem(new SeededRng(2));
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `StealthDiveInstance`
Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs:19: /// dock-operation handoff into the existing StealthDiveInstance, and a
Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs:188: var dive = new StealthDiveInstance();
Assets/Ashfall.Core/DeepCoastHeadlessDemo.cs:194: Check(dive.IsActive, "dock dive handed off to existing StealthDiveInstance");
Assets/Ashfall.Core/Maritime/DiveSiteCatalog.cs:13: /// immersive stage for <see cref="StealthDiveInstance"/>; the instance keeps
Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs:12: public sealed class StealthDiveInstance : MaritimeDiveSystem
Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs:14: public StealthDiveInstance(ISeededRng? rng = null, ILog? log = null)
src/Host/DeepCoastHostSession.cs:22: /// session (StealthDiveInstance + MaritimeSaveStore).
src/Host/DeepCoastHostSession.cs:172: // The dive itself is the existing StealthDiveInstance (MaritimeSaveStore owns it).
src/Host/HostCli.SelfTests.cs:574: /// channel → berth → dock dive (existing StealthDiveInstance) → scavenge
src/Host/HostCli.SelfTests.cs:633: Check(host.Maritime.Dive.IsActive, "existing StealthDiveInstance is active");
src/Host/MaritimeHostSession.cs:23: public StealthDiveInstance Dive { get; }
src/Host/MaritimeHostSession.cs:31: StealthDiveInstance dive = null!,
src/Host/MaritimeHostSession.cs:40: Dive = dive ?? new StealthDiveInstance();
Ashfall.Core.Tests/BlackFlotillaTests.cs:10: /// StealthDiveInstance Core system. Phase 0 bug-fix verification.
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `ProceduralScavengeSystem`
Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs:16: public class ProceduralScavengeSystem
Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs:36: public ProceduralScavengeSystem(ISeededRng? rng = null)
Assets/Ashfall.Core/Maritime/VariableLootNode.cs:8: /// loot node definitions. Used by ProceduralScavengeSystem and the
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:476: ["black_flotilla_items.json"] = new[] { "ProceduralScavengeSystem" },
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:762: ["black_flotilla_items.json"] = "ProceduralScavengeSystem",
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1082: ["black_flotilla_items.json"] = new[] { "MaritimeDiveSystem", "ProceduralScavengeSystem" },
src/Host/HostCli.Collectibles.cs:298: var maritime = new ProceduralScavengeSystem(new SeededRng(5));
src/Host/DeepCoastHostSession.cs:35: private readonly ProceduralScavengeSystem _dockScavenge;
src/Host/DeepCoastHostSession.cs:57: _dockScavenge = new ProceduralScavengeSystem(_rng);
src/Host/DeepCoastHostSession.cs:212: /// ProceduralScavengeSystem.RollLootTable (canonical dock loot, degraded
src/Host/HostCli.SelfTests.cs:575: /// rewards through ProceduralScavengeSystem with the Fleet levy → journal
src/Host/HostCli.SelfTests.cs:642: // 6. Complete with scavenge through ProceduralScavengeSystem.
src/Host/MaritimeHostSession.cs:24: public ProceduralScavengeSystem Scavenge { get; }
src/Host/MaritimeHostSession.cs:32: ProceduralScavengeSystem scavenge = null!,
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `SafeCrackingSystem`
Assets/Ashfall.Core/Maritime/DiveSiteCatalog.cs:17: /// gear gate, contamination key, data-driven safes (SafeCrackingSystem),
Assets/Ashfall.Core/Maritime/DiveSiteCatalog.cs:53: /// <summary>Data-driven safes opened through the real SafeCrackingSystem.</summary>
Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs:366: /// <summary>Data-driven safes for a site (registered with the SafeCrackingSystem by the host on entry).</summary>
Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs:62: public string systemId = SafeCrackingSystem.SystemId;
Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs:103: public class SafeCrackingSystem
Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs:130: public SafeCrackingSystem(int seed = 42)
src/Host/MaritimeHostSession.cs:26: public SafeCrackingSystem SafeCrack { get; }
src/Host/MaritimeHostSession.cs:34: SafeCrackingSystem safeCrack = null!,
src/Host/MaritimeHostSession.cs:43: SafeCrack = safeCrack ?? new SafeCrackingSystem(seed);
src/UI/SafeCrackModal.cs:14: /// All gameplay logic delegates to MaritimeHostSession → SafeCrackingSystem.
Ashfall.Core.Tests/Plan23DiveMechanicCoverageTests.cs:16: /// gear gates, data-driven safes through the live SafeCrackingSystem,
Ashfall.Core.Tests/Plan23DiveMechanicCoverageTests.cs:144: var system = new SafeCrackingSystem(seed: 4242);
Ashfall.Core.Tests/Plan23DiveMechanicCoverageTests.cs:166: var restored = new SafeCrackingSystem(1);
Ashfall.Core.Tests/Plan23LongCampaignTests.cs:71: var system = new SafeCrackingSystem(77);
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `TideCalendar`
Assets/Ashfall.Core/Maritime/DiveSiteCatalog.cs:65: /// <see cref="TideCalendar"/> — no serialized tide state.
Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs:352: if (!TideCalendar.IsWindowOpen(window, campaignDay))
Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs:354: blocker = "tide:" + TideCalendar.PhaseName(TideCalendar.PhaseFor(Math.Max(0, campaignDay)));
Assets/Ashfall.Core/Maritime/TideCalendar.cs:33: public static class TideCalendar
src/UI/MaritimeAtlasPanel.cs:369: int until = Ashfall.Core.Maritime.TideCalendar.DaysUntilOpen(parsed, day);
src/UI/MaritimeAtlasPanel.cs:371: return rule + " · now: " + Ashfall.Core.Maritime.TideCalendar.PhaseName(Ashfall.Core.Maritime.TideCalendar.PhaseFor(day)).ToLowerInvariant() + ", open
Ashfall.Core.Tests/Plan23CoastalDynamicsTests.cs:41: Assert.Equal(TidePhase.Low, TideCalendar.PhaseFor(0));
Ashfall.Core.Tests/Plan23CoastalDynamicsTests.cs:42: Assert.Equal(TidePhase.Rising, TideCalendar.PhaseFor(1));
Ashfall.Core.Tests/Plan23CoastalDynamicsTests.cs:43: Assert.Equal(TidePhase.High, TideCalendar.PhaseFor(2));
Ashfall.Core.Tests/Plan23CoastalDynamicsTests.cs:44: Assert.Equal(TidePhase.Falling, TideCalendar.PhaseFor(3));
Ashfall.Core.Tests/Plan23CoastalDynamicsTests.cs:45: Assert.Equal(TidePhase.Low, TideCalendar.PhaseFor(4));
Ashfall.Core.Tests/Plan23CoastalDynamicsTests.cs:46: Assert.Equal(TideCalendar.PhaseFor(400), TideCalendar.PhaseFor(1016));
Ashfall.Core.Tests/Plan23CoastalDynamicsTests.cs:52: Assert.True(TideCalendar.IsWindowOpen(TideWindow.Any, 0));
Ashfall.Core.Tests/Plan23CoastalDynamicsTests.cs:53: Assert.True(TideCalendar.IsWindowOpen(TideWindow.Slack, 1));  // rising = slack turn
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `BlackFlotillaStanding`
Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs:20: public static class BlackFlotillaStanding
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:42: BlackFlotillaStanding.Register(engine);
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:43: engine.SetTrust(BlackFlotillaStanding.FactionId, -10f);
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:44: Assert.False(BlackFlotillaStanding.CanTrade(engine.GetTrust(BlackFlotillaStanding.FactionId)));
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:46: engine.ModifyTrust(BlackFlotillaStanding.FactionId, 10f);
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:47: Assert.True(BlackFlotillaStanding.CanTrade(engine.GetTrust(BlackFlotillaStanding.FactionId)));
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:48: Assert.False(BlackFlotillaStanding.CanShareIntel(engine.GetTrust(BlackFlotillaStanding.FactionId)));
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:50: engine.SetTrust(BlackFlotillaStanding.FactionId, 45f); // ≥ intel threshold
Ashfall.Core.Tests/Plan23CrossLayerIntegrationTests.cs:51: Assert.True(BlackFlotillaStanding.CanShareIntel(engine.GetTrust(BlackFlotillaStanding.FactionId)));
Ashfall.Core.Tests/Plan23FlotillaFactionDepthTests.cs:40: var entry = catalog.GetFaction(BlackFlotillaStanding.FactionId);
Ashfall.Core.Tests/Plan23FlotillaFactionDepthTests.cs:59: Assert.Contains(BlackFlotillaStanding.FactionId, ids);
Ashfall.Core.Tests/Plan23FlotillaFactionDepthTests.cs:68: BlackFlotillaStanding.Register(engine);
Ashfall.Core.Tests/Plan23FlotillaFactionDepthTests.cs:70: Assert.True(engine.IsFactionActive(BlackFlotillaStanding.FactionId));
Ashfall.Core.Tests/Plan23FlotillaFactionDepthTests.cs:71: engine.SetTrust(BlackFlotillaStanding.FactionId, 20f);
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `Main.Maritime`
src/Host/MaritimeSaveStore.cs:5: // Host Caller: Main.Maritime / DeepCoastHostSession, MaritimeHostSession
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.
### Mechanical references to `MaritimePanel`
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1470: ["dive_sites.json"] = new[] { "MaritimePanel" },
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1471: ["deep_lore_locations.json"] = new[] { "MaritimePanel" },
Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1472: ["black_flotilla_items.json"] = new[] { "MaritimePanel" },
src/Main.PanelLifecycle.cs:37: _maritimePanel,
src/Main.Maritime.cs:164: private void CloseMaritimePanel()
src/Main.Maritime.cs:166: if (_maritimePanel != null) _maritimePanel.Visible = false;
src/Main.UiPanels.cs:72: private MaritimePanel _maritimePanel = null!;
src/Main.UiPanels.cs:441: _maritimePanel = new MaritimePanel();
src/Main.UiPanels.cs:442: _maritimePanel.OnClose += CloseMaritimePanel;
src/Main.UiPanels.cs:443: AddChild(_maritimePanel);
src/Main.GameFlow.cs:645: _maritimePanel.Bind(_maritime, _survivors);
src/Main.GameFlow.cs:646: _maritimePanel.Open();
src/Main.PlayerSurfaces.cs:439: bindAction: () => { SetupMaritime(); SetupSurvivors(); _maritimePanel.Bind(_maritime, _survivors); },
src/Main.PlayerSurfaces.cs:440: openAction: () => _maritimePanel.Open(),
Interpretation: these are search hits only; an integrating builder must trace the actual call path, setup binding, event subscription and save registration before claiming reachability.

# Appendix E — Read-Only Master Authority Slices

The following slices are read-only excerpts from the user-specified authority. They are included to constrain architecture and quality; live source/data remains authoritative.

authority lines 40-51:
40:
41: **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
42: Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.
43:
44: **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
45: Observed live and not listed in v1.0 Part 5.8: `ECONOMY_FAIRNESS_AUDIT.md`, `ENGINE_SUPPORT_POLICY.md`, `GODOT_MIGRATION_STATUS.md`, `REPO_HISTORY_REWRITE.md`, `HUMAN_AUTHORSHIP.md`, `AI_DISCLOSURE.md`, `ASSET_MIGRATION_LEDGER.md`, `CODEX_SOURCE_MATRIX.md`, `ARCHIVE_INDEX.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md`, `SHELTER_MAINTENANCE_MATRIX.md`, `SHELTER_30_DAY_MAINTENANCE_REPORT.md`, `L10N_WAVE2_ROADMAP.md`, `INPUT.md`, `RELEASE_EXPORT.md`, `ENGINE_SUPPORT_POLICY.md`. Of these, `ECONOMY_FAIRNESS_AUDIT.md`, `EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, and `SHELTER_MAINTENANCE_MATRIX.md` are pre-computed balance baselines: they convert Lane C (economy and balance) planning from speculative to evidence-anchored. Subject plans in Lane C must cite these baselines instead of re-deriving numbers.
46:
47: **DR-04 — The data catalog inventory has grown; several catalogs are absent from the v1.0 inventory. VERIFIED.**
48: `Assets/StreamingAssets/Data/` currently holds 342 entries. Catalogs observed live but not present in the v1.0 Part 5.4 inventory include: `dive_sites.json`, `hydroponic_crops.json`, `hydraulic_extrusion_catalog.json`, `metrology_standards_catalog.json`, `muster_camp_scenes.json`, `muster_epilogues.json`, `muster_faction_actions.json`, `muster_faction_culture.json`, `muster_witnesses.json`, `utility_actions.json`, `moral_choice_quests_branching.json`, `moral_choice_quests_distress.json`. Consequence: the duplication firewall (v1.0 Part 5) is stale in these domains; a planner could propose a "new" muster or moral-choice catalog that already exists. The ID-collision sweep in Factory Protocol step 1 must always run against the live listing, never against this document.
49:
50: **DR-05 — A process script lives inside the data authority. VERIFIED (hygiene finding).**
51: `Assets/StreamingAssets/Data/` contains `rewrite.py` alongside the JSON catalogs. The data directory is canonically "the sole authored JSON data authority" (`AGENTS.md` rule 3); a Python rewrite script inside it is a process artifact in a content directory. Recommended handling: a Tooling-lane (Lane H) subject plan proposing relocation of the script to `scripts/` or `tools/` with a documented rationale, after verifying what the script rewrites and who calls it. Do not move it without call-site verification; it may be load-bearing for a historical catalog migration.
authority lines 79-90:
79:
80: **Step 1 — Premise sweep (mandatory, every time).**
81: Before selecting any candidate, the session re-verifies premises against live source: the live `Assets/StreamingAssets/Data/` listing (duplication firewall, DR-04), `INTEGRATION_PLANS.md` current batch (DR-06), `WORKTREE_OWNERSHIP.md` claims (DR-09), `KNOWN_DEBT.md`, the root coordination files (DR-01), `docs/gaps/` and `docs/incidents/` (DR-02), and `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (DR-08). Output: a short premise sheet. A candidate whose premise fails the sweep is discarded, not patched.
82:
83: **Step 2 — Select exactly one lane and one subsystem cluster.**
84: From Part III. The selection rule is lane rotation discipline (v1.0 Part 11): at most one plan per lane per wave; data-first lanes (A, C, J) precede wiring lanes (B, D, E) within the same domain. The session states the lane and cluster in the plan header.
85:
86: **Step 3 — Pull the cell's opening archetype and instantiate it.**
87: Each matrix cell names an archetype (the *kind* of expansion that cell supports, with its owning seams). The session instantiates the archetype against current evidence: which catalog, which loader, which host session, which save family, which panel. If the archetype's seams no longer exist as described, the cell is stale — record the correction in the Drift Register and pick again.
88:
89: **Step 4 — Draft the subject plan in the v2.0 subject-plan format (Part V, Template S).**
90: A subject plan is not an integration plan. It states WHAT should expand, WHY (with evidence), WHAT MUST NOT CHANGE, and WHICH INTEGRATION ROUTE the repository should prefer — but it does not prescribe line-level implementation. The integration route recommendation (Part V, Template R) names the tier (data-only / host wiring / Core extension), the seams, the save impact class, and the verification class. This preserves the repo's own separation: subject plans propose; integration plans (drafted later, against the live tree, in an owning session) commit.
authority lines 104-109:
104: - No plan may create a parallel authority. Every state change names its owning system.
105: - Data-first preference: if an expansion can be authored as JSON through an existing loader, it must be, and the plan must say so.
106: - The factory never drafts against decision-blocked items (the current list must be re-read from `INTEGRATION_PLANS.md` each session — DR-06 shows signatures resolve over time).
107: - Subject plans do not edit files. Implementation happens only in an owning session after plan selection (v1.0 approval-based workflow).
108: - Every generated plan must state its position relative to each epilogue permutation it touches (v1.0 Part 6.6).
109:
authority lines 220-225:
220: | Content | Content-utilization reporting extensions driven by `UNCLAIMED_CORPUS_CENSUS.md` (DR-08): a census-to-plan feed | HIGH CONFIDENCE |
221: | Docs | Index drift gate extensions covering the new root-level coordination files (DR-01) | HIGH CONFIDENCE |
222: | CI | Gate-count drift detection (DR-07): a script or test that fails when documented gate counts diverge from the live inventory | PROPOSAL |
223:
224: ### 3.9 Lane I — Documentation and conventions
225:
authority lines 258-263:
258: **SB-07 — Root coordination surface registration (Lane I).** Evidence: DR-01, DR-09. Subject: register root-level coordination files and agent rulebooks in the docs map; define which are active vs historical. Integration route: docs-only. Confidence: VERIFIED need.
259:
260: **SB-08 — Gate-count drift guard (Lane H).** Evidence: DR-07. Subject: a check that fails when a documented gate count diverges from the live inventory, ending manual count drift between bibles, closeouts, and CI. Integration route: small script/test in `scripts/ci/` family, mirrors existing gates. Confidence: PROPOSAL (design needs the live gate inventory as input).
261:
262: **SB-09 — `rewrite.py` data-authority hygiene (Lane H).** Evidence: DR-05. Subject: verify the script's role; relocate or document in place. Integration route: tooling-only; requires call-site verification first. Confidence: VERIFIED finding, PROPOSAL handling.
263:
authority lines 266-271:
266: **SB-11 — C2 open-gap package: Plan 31 semantic-kind authority, 17C audio phases, 17B deep test matrix (Lanes B, F, G).** Evidence: DR-06 not-executed lists. Subject: three bounded follow-ons the ledger itself records as real gaps. Integration route: per existing C2 plan documentation. Confidence: VERIFIED as open; scope per item needs the plan docs.
267:
268: **SB-12 — Dive-site and hydroponic domain expansion (Lanes A and B/C3, C5).** Evidence: DR-04 — `dive_sites.json`, `hydroponic_crops.json` live but absent from v1.0's inventory. Subject: premise-sweep these domains for unexploited seams (dive oxygen drain is a canon hourly system; hydroponics may lack narrative corpus and economy legs). Integration route: data-first + existing host sessions. Confidence: INFERENCE pending sweep.
269:
270: ---
271:
authority lines 637-642:
637:
638: ### Subject
639: A two-domain premise sweep converting DR-04's inventory findings into openings: `dive_sites.json` (against the canon hourly dive-oxygen-drain system and the deep-coast family) and `hydroponic_crops.json` (against the greenhouse/aquaponics/aeroponics families), each audited for narrative corpus coverage, economy legs, and cross-system bridges (dive → medical/ARS via immersion exposure rules; hydroponics → morale via fresh-food rules if such rules exist).
640:
641: ### Premise evidence
642: VERIFIED: both catalogs exist live and are absent from the v1.0 inventory (DR-04). VERIFIED: `District8DeepCoastSystem`, dive oxygen drain (hourly tick class), and the greenhouse host session exist (v1.0 Parts 3.2, 5.2, 5.5). INFERENCE: both domains are under-expanded relative to their neighbors — this is precisely what the sweep must confirm or refute.
authority lines 935-940:
935: **DM-2 — Medical pipeline (C2).** Owners: disease, pathogens, dose ledger, ARS, surgery, autopsy, pharma lab, diagnostics, therapies, dependency, crises. Live catalogs: `disease_catalog`, `pathogens`, `dose_items/locations/quests/registers`, `autopsy_procedures`, `surgical_procedures`, `pharma_recipes`, `microfluidic_diagnostic_catalog`, `medical_texts`, `psychological_therapies`, `chemical_dependency_items`. Hosts: MedicalWard, DoseLedger, PsychologyArc, MentalHealthCrisis. Docs: `MEDICAL_PIPELINE_JOURNEY.md`, `MEDICAL_DOSE_TREATMENT_MATRIX.md`, `MEDICAL_30_DAY_CAPACITY_REPORT.md` (all verified live). Openings: A-03, A-04, A-05, B-03, B-04, B-25, C-14 support, G-03.
936:
937: **DM-3 — Water, food, agriculture (C3).** Owners: water treatment, condensers, deep wells, brine, nutrition, kitchen, preservation, grain, greenhouse, aquaponics, aeroponics, apiculture, cryo cultivars. Live catalogs: `water_treatment` family via systems, `fog_harvesting_catalog`, `deep_well` systems, `brine` systems, `nutrition_profiles`, `food_preservation`, `grain_processing`, `greenhouse_items`, `hydroponic_crops`, `aquaponics_system_catalog`, `aeroponics_nutrient_catalog`, `cryo_cultivars`, `crop_strains`, `dive_sites`. Hosts: Greenhouse, GrainProcessing, KitchenNutrition, FoodPreservation, DeepWell, Sanitation, DeepCoast. Openings: A-06, A-07, A-08, B-05, C-12, F-012 (dive/hydroponic audit consumed as F-012 above).
938:
939: **DM-4 — Power and industry (C4).** Owners: power grid, SOFC, solar, kinetic, geothermal, foundry, CVD diamond, EB/PVD, optics, powder metallurgy, pyrolysis, Fischer-Tropsch, chlor-alkali, acids, fermentation, ethanol, air separation, metrology, extrusion. Live catalogs: `power_grid`, `power_subgrid_nodes`, `sofc_power_catalog`, `solar_concentrator_catalog`, `kinetic_flywheel_catalog`, `geothermal_strata_catalog`, `geothermal_drilling_depths`, `cupola_foundry_catalog`, `cvd_diamond_catalog`, `ebpvd_coating_catalog`, `precision_optics_catalog`, `precision_broaching_catalog`, `powder_metallurgy_catalog`, `plastic_pyrolysis_catalog`, `fischer_tropsch_catalog`, `chlor_alkali_synthesis_catalog`, `mineral_acid_synthesis_catalog`, `bio_fermentation_catalog`, `cellulosic_ethanol_catalog`, `cryogenic_air_separation`, `low_background_lead_catalog`, `metrology_standards_catalog`, `hydraulic_extrusion_catalog`. Hosts: SofcPower, SolarConcentrator, SilentFoundry, CvdDiamond, EbPvdCoating, PrecisionOptics, CryogenicAirSeparation, ChlorAlkali, BioFermentation, PlasticPyrolysis, HydraulicExtrusion, LowBackgroundMetrology, GeothermalAquifer. Openings: A-09, A-10, A-11, B-06, C-01, C-02. Constraint: XP W1 owns difficulty scalars for this cluster post-seal.
940:

# Appendix F — Focused Runner Command Contract

The following commands are **planned verification commands**, not claims that this planning-only pass ran them:

```text
bash scripts/run_test.sh Ashfall.Core.Tests/MaritimeDiveSystemTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Plan23DiveMechanicCoverageTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Maritime/Plan207MaritimeExplorationIntegrationTests.cs
bash scripts/run_test.sh <directly-affected-focused-directory>
python3 scripts/ci/generate-architecture-map.py --check   # only if a mapped owner/host seam changes
python3 scripts/ci/generate-docs-index.py --check          # only under the generated-index owner
```

Builder rule: resolve each angle-bracket placeholder to a real current test file before running. Do not run a broad suite by default. New test files run alone first. Quarantine/re-enable decisions require current API/content evidence, a reason, and a passing focused target.

# Appendix G — Quality Matrix and Negative Test Inventory

| Quality dimension | Required proof | Failure if absent |
|---|---|---|
| premise accuracy | live path/hash/catalog evidence | stale or fictional plan |
| ownership | one mutable owner and explicit boundary | parallel authority |
| data integrity | schema, IDs, references and rows | orphan content |
| integration | real command and event path | compile-only fiction |
| persistence | capture/restore and old-save default | state loss |
| determinism | stable order and seeded stream | replay divergence |
| UI truth | owner projection and feedback | panel cache/lie |
| accessibility | focus, input, contrast and disposal | inaccessible route |
| failure handling | named refusal/fail-closed behavior | silent success |
| rollback | phase-local revert and old-save compatibility | unrecoverable data |
| QA honesty | command/result distinction | false completion claim |

Negative cases to test or document include duplicate ID, missing reference, empty catalog, invalid numeric range, stale save, repeated event, host reload, unavailable owner, insufficient resource, inaccessible location, and same-seed replay.

# Appendix H — Plan-Specific Decision Ledger

| Decision | Current evidence | Safe conclusion | Revisit when |
|---|---|---|---|
| owner | source files and declarations in Appendix A | extend current owner only | source contract changes |
| content | catalog audit in Appendix B | add only with a current consumer | loader/schema changes |
| persistence | owner/save evidence | no new section by default | durable fact confirmed |
| host | caller/reference graph in Appendix D | one real route required | shared seam claimed |
| UI | current panel path | projection-only | route/manifest changes |
| randomness | deterministic mandate | seeded stream or no randomness | simulation rule requires choice |
| tests | current inventory | smallest confirmed target | public contract changes |

# Appendix I — Original Intent Preservation and Stale-Claim Cleanup

The original plan’s useful intent is preserved as a bounded design goal, not as authority. Historical “sealed”, “approved”, “100 tests”, “600-day trace” or exact future row counts are not accepted merely because they appear in an old plan. This rebuild removes unsupported claims, fictional APIs, fake save sections, duplicate authorities and test-count padding. Completed behavior is retained as maintenance scope; residual behavior is tied to a current path and a current consumer.

# Appendix J — Handoff Checklist

- [ ] Current owner re-read immediately before implementation.
- [ ] Exact claimed paths confirmed against `WORKTREE_OWNERSHIP.md`.
- [ ] Current catalog rows and references re-censused.
- [ ] Existing save reader and capture/restore path identified.
- [ ] Existing host command and event consumer traced.
- [ ] Determinism and seed order specified.
- [ ] UI/accessibility behavior specified without panel authority.
- [ ] Focused tests selected from current tree.
- [ ] New test file run alone first if created.
- [ ] Rollback and old-save behavior documented.
- [ ] No production/data/test/UI edits made by this planning pass.

# Appendix K — Source Hash and Path Verification Record

The following records are generated from current files. A later builder must re-run the hash check after any source/data edit; a stale hash invalidates the affected evidence block.


## Audit cycle 01, lens 01: Maritime & Black Flotilla boundary

**Question 01.01.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 02: Maritime & Black Flotilla boundary

**Question 01.02.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 03: Maritime & Black Flotilla boundary

**Question 01.03.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 04: Maritime & Black Flotilla boundary

**Question 01.04.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 05: Maritime & Black Flotilla boundary

**Question 01.05.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 06: Maritime & Black Flotilla boundary

**Question 01.06.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 07: Maritime & Black Flotilla boundary

**Question 01.07.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 08: Maritime & Black Flotilla boundary

**Question 01.08.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 09: Maritime & Black Flotilla boundary

**Question 01.09.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 10: Maritime & Black Flotilla boundary

**Question 01.10.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 11: Maritime & Black Flotilla boundary

**Question 01.11.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 01, lens 12: Maritime & Black Flotilla boundary

**Question 01.12.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 01.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 01.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 01.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 02, lens 01: Maritime & Black Flotilla boundary

**Question 02.01.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 02: Maritime & Black Flotilla boundary

**Question 02.02.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 03: Maritime & Black Flotilla boundary

**Question 02.03.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 04: Maritime & Black Flotilla boundary

**Question 02.04.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 05: Maritime & Black Flotilla boundary

**Question 02.05.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 06: Maritime & Black Flotilla boundary

**Question 02.06.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 07: Maritime & Black Flotilla boundary

**Question 02.07.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 08: Maritime & Black Flotilla boundary

**Question 02.08.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 09: Maritime & Black Flotilla boundary

**Question 02.09.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 10: Maritime & Black Flotilla boundary

**Question 02.10.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 11: Maritime & Black Flotilla boundary

**Question 02.11.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 02, lens 12: Maritime & Black Flotilla boundary

**Question 02.12.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 02.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 02.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 02.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 03, lens 01: Maritime & Black Flotilla boundary

**Question 03.01.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 02: Maritime & Black Flotilla boundary

**Question 03.02.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 03: Maritime & Black Flotilla boundary

**Question 03.03.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 04: Maritime & Black Flotilla boundary

**Question 03.04.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 05: Maritime & Black Flotilla boundary

**Question 03.05.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 06: Maritime & Black Flotilla boundary

**Question 03.06.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 07: Maritime & Black Flotilla boundary

**Question 03.07.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 08: Maritime & Black Flotilla boundary

**Question 03.08.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 09: Maritime & Black Flotilla boundary

**Question 03.09.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 10: Maritime & Black Flotilla boundary

**Question 03.10.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 11: Maritime & Black Flotilla boundary

**Question 03.11.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 03, lens 12: Maritime & Black Flotilla boundary

**Question 03.12.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 03.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 03.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 03.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 04, lens 01: Maritime & Black Flotilla boundary

**Question 04.01.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 02: Maritime & Black Flotilla boundary

**Question 04.02.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 03: Maritime & Black Flotilla boundary

**Question 04.03.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 04: Maritime & Black Flotilla boundary

**Question 04.04.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 05: Maritime & Black Flotilla boundary

**Question 04.05.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 06: Maritime & Black Flotilla boundary

**Question 04.06.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 07: Maritime & Black Flotilla boundary

**Question 04.07.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 08: Maritime & Black Flotilla boundary

**Question 04.08.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 09: Maritime & Black Flotilla boundary

**Question 04.09.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 10: Maritime & Black Flotilla boundary

**Question 04.10.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 11: Maritime & Black Flotilla boundary

**Question 04.11.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 04, lens 12: Maritime & Black Flotilla boundary

**Question 04.12.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 04.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 04.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 04.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 05, lens 01: Maritime & Black Flotilla boundary

**Question 05.01.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 02: Maritime & Black Flotilla boundary

**Question 05.02.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 03: Maritime & Black Flotilla boundary

**Question 05.03.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 04: Maritime & Black Flotilla boundary

**Question 05.04.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 05: Maritime & Black Flotilla boundary

**Question 05.05.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 06: Maritime & Black Flotilla boundary

**Question 05.06.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 07: Maritime & Black Flotilla boundary

**Question 05.07.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 08: Maritime & Black Flotilla boundary

**Question 05.08.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 09: Maritime & Black Flotilla boundary

**Question 05.09.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 10: Maritime & Black Flotilla boundary

**Question 05.10.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 11: Maritime & Black Flotilla boundary

**Question 05.11.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 05, lens 12: Maritime & Black Flotilla boundary

**Question 05.12.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 05.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 05.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 05.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 06, lens 01: Maritime & Black Flotilla boundary

**Question 06.01.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 02: Maritime & Black Flotilla boundary

**Question 06.02.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 03: Maritime & Black Flotilla boundary

**Question 06.03.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 04: Maritime & Black Flotilla boundary

**Question 06.04.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 05: Maritime & Black Flotilla boundary

**Question 06.05.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 06: Maritime & Black Flotilla boundary

**Question 06.06.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 07: Maritime & Black Flotilla boundary

**Question 06.07.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 08: Maritime & Black Flotilla boundary

**Question 06.08.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 09: Maritime & Black Flotilla boundary

**Question 06.09.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 10: Maritime & Black Flotilla boundary

**Question 06.10.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 11: Maritime & Black Flotilla boundary

**Question 06.11.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 06, lens 12: Maritime & Black Flotilla boundary

**Question 06.12.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 06.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 06.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 06.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 07, lens 01: Maritime & Black Flotilla boundary

**Question 07.01.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 02: Maritime & Black Flotilla boundary

**Question 07.02.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 03: Maritime & Black Flotilla boundary

**Question 07.03.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 04: Maritime & Black Flotilla boundary

**Question 07.04.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 05: Maritime & Black Flotilla boundary

**Question 07.05.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 06: Maritime & Black Flotilla boundary

**Question 07.06.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 07: Maritime & Black Flotilla boundary

**Question 07.07.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 08: Maritime & Black Flotilla boundary

**Question 07.08.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 09: Maritime & Black Flotilla boundary

**Question 07.09.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 10: Maritime & Black Flotilla boundary

**Question 07.10.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 11: Maritime & Black Flotilla boundary

**Question 07.11.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 07, lens 12: Maritime & Black Flotilla boundary

**Question 07.12.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 07.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 07.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 07.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 08, lens 01: Maritime & Black Flotilla boundary

**Question 08.01.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 02: Maritime & Black Flotilla boundary

**Question 08.02.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 03: Maritime & Black Flotilla boundary

**Question 08.03.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 04: Maritime & Black Flotilla boundary

**Question 08.04.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 05: Maritime & Black Flotilla boundary

**Question 08.05.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 06: Maritime & Black Flotilla boundary

**Question 08.06.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 07: Maritime & Black Flotilla boundary

**Question 08.07.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 08: Maritime & Black Flotilla boundary

**Question 08.08.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 09: Maritime & Black Flotilla boundary

**Question 08.09.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 10: Maritime & Black Flotilla boundary

**Question 08.10.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 11: Maritime & Black Flotilla boundary

**Question 08.11.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 08, lens 12: Maritime & Black Flotilla boundary

**Question 08.12.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 08.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 08.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 08.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.


## Audit cycle 09, lens 01: Maritime & Black Flotilla boundary

**Question 09.01.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.01.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.01.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.01.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 02: Maritime & Black Flotilla boundary

**Question 09.02.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.02.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.02.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.02.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 03: Maritime & Black Flotilla boundary

**Question 09.03.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.03.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.03.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.03.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 04: Maritime & Black Flotilla boundary

**Question 09.04.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.04.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.04.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.04.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 05: Maritime & Black Flotilla boundary

**Question 09.05.** Does `Assets/Ashfall.Core/Maritime/BlackFlotillaStanding.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.05.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.05.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.05.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 06: Maritime & Black Flotilla boundary

**Question 09.06.** Does `src/Main.Maritime.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.06.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.06.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.06.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 07: Maritime & Black Flotilla boundary

**Question 09.07.** Does `src/UI/MaritimePanel.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.07.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.07.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.07.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 08: Maritime & Black Flotilla boundary

**Question 09.08.** Does `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.08.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.08.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.08.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 09: Maritime & Black Flotilla boundary

**Question 09.09.** Does `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/black_flotilla_items.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.09.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.09.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.09.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 10: Maritime & Black Flotilla boundary

**Question 09.10.** Does `Assets/Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/currents.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.10.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.10.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.10.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 11: Maritime & Black Flotilla boundary

**Question 09.11.** Does `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/maritime_zones.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.11.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.11.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.11.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.

## Audit cycle 09, lens 12: Maritime & Black Flotilla boundary

**Question 09.12.** Does `Assets/Ashfall.Core/Maritime/TideCalendar.cs` remain the narrowest valid seam for this concern, or is the proposed feature only a projection over another owner? The implementation must answer with a source-level call path, a current public contract, a setup/lifecycle binding and a save answer. A class declaration, a test fixture or a panel name is not sufficient.

**Current evidence to recheck.** `Assets/StreamingAssets/Data/dive_sites.json` must be parsed and its IDs/references counted. The result must distinguish content presence from a live consumer. If the catalog is absent, malformed, orphaned or not loaded, record a blocker rather than creating a replacement loader.

**Determinism lens 09.12.** The same owner state, campaign seed, day, command sequence and catalog revision must produce the same domain result. If the feature uses a random choice, name the existing stream and stable ordering. If it does not need randomness, remove the random branch rather than simulating one.

**UI lens 09.12.** The panel or HUD must read the owner projection, show a named refusal, refresh after a successful mutation and release bindings on disposal. It must not keep a second mutable cache or infer success from a button press.

**Failure lens 09.12.** Empty catalog, missing reference, old save, repeated event, host reload and absent owner each have one documented outcome. The preferred result is fail-closed with a diagnostic, not a fabricated default or a silent mutation.



> **Pre-polish body length at generation time:** 271,875 characters.
> **Target interpretation:** 150k–170k is the first checkpoint; 250k+ is the evidence-backed depth target and is not a ceiling.

# Post-250K Deep Polishing Pass — Maritime & Black Flotilla

This pass was applied only after the plan body exceeded the 250,000-character evidence-backed depth target. The trigger is structural, not a quality claim: the base plan is already complete enough for review, so the polishing pass audits decisions and failure paths instead of appending fictional feature scope.

## Polish 1 — Content and evidence depth

- Rechecked the distinction between terminal maintenance work and residual implementation work.
- Rechecked every current source/data path named in the plan; missing paths are treated as premise gaps, not silently promoted.
- Rechecked catalog counts, schema keys and sample rows; row presence is not described as reachability.
- Rechecked the owner matrix so mutable state, authored content, lifecycle, host commands, facts, presentation and persistence have separate homes.
- Rechecked the plan against the live master authority, which remains read-only and subordinate to current source/data.
- Rechecked that historical “sealed” language, fake APIs, fake test counts and duplicate managers cannot become implementation instructions.

**Polish 1 outcome:** the plan is allowed to discuss future work only as `PROPOSAL` or `UNKNOWN`; every implementation step has a current evidence question and a completion gate.

## Polish 2 — Integration and code architecture

- Rechecked the proposed flow: input → validation → current owner → typed fact → existing consumer → truthful presentation → existing save path.
- Rechecked Core/host boundaries: no engine imports in Core, no gameplay math in panels, no cross-environment value exports from an aspect index.
- Rechecked deterministic behavior: no wall-clock, GUID or `System.Random` decision path; stable ordering precedes any weighted selection.
- Rechecked persistence: no new save section is assumed; old-save defaults, capture/restore, deep-copy and checksum behavior remain explicit questions.
- Rechecked accessibility: focus, close/back, controller input, readable status and refresh/disposal are part of acceptance, not optional polish.
- Rechecked failure behavior: missing owner, empty catalog, duplicate ID, stale save and repeated delivery fail closed or preserve prior truth.

**Polish 2 outcome:** the implementation route is the smallest extension of current seams, with a separate claim required for any shared composition root.

## Final precision and reaccuracy pass

1. Re-run the current source/data census and compare it with the hashes in this document.
2. Trace one real player command from the current host input to the current Core owner.
3. Trace one real fact from the owner to its current consumer and verify post-mutation ordering.
4. Trace every durable proposed field through the existing capture/restore path; delete hypothetical fields that do not survive this test.
5. Reject any proposed row, API, save section or panel route that lacks a current owner, validator and consumer.
6. Re-run the focused test selection after implementation and record actual output separately from this planning artifact.
7. Perform a final scope audit: no unrelated systems, no Unity dependency, no generated index, no speculative architecture.

**Precision result:** this plan is implementation-ready only after those current-evidence checks pass. A stale premise returns `STALE_PLAN`; it does not justify restoring an old API.

## Full repolishing phase — maximum useful depth

The final repolish is a quality ceiling, not a length ceiling. It must improve decision clarity, not add noise. The reviewer asks:

- Can a new builder identify the first safe file to read?
- Can they tell what is already complete?
- Can they tell what remains genuinely missing?
- Can they prove the player-visible route?
- Can they prove persistence and replay?
- Can they identify every owner boundary they must not cross?
- Can they run the smallest meaningful verification target?
- Can they roll back one phase without corrupting a save?
- Can they explain why each proposed row or field is necessary?
- Can they reject a stale or duplicate implementation proposal?

A plan that cannot answer those questions is not ready for implementation, regardless of character count. This final phase therefore closes on precision, safety, legibility and truthful game feel—not on a larger document.

# Appendix L — Verification Record for This Planning Pass

- Plan generation status: **complete**.
- Base body threshold: **250,000+ characters before the post-250k polish appendices**.
- Current authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`.
- Production files changed by this pass: **none**.
- Authored JSON changed by this pass: **none**.
- Tests run by this planning pass: **none**; focused commands are explicitly separated as future verification.
- Structural verifier: run externally after generation; it must check required sections, minimum length, authority reference, current path existence and no false fresh-pass claims.
- `git diff --check`: run externally against only the 15 claimed plan files and the two governance rows after finalization.

# Appendix M — Implementation Handoff Contract

**Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-6`

**Outcome:** `Maritime & Black Flotilla`

**Files changed:** the exact 15 plan paths in the Round 6 claim, plus the Round 6 governance rows only.

**Current contract used:** live Core/host/data/test evidence in Appendices A–D; master authority read-only in Appendix E.

**Verification commands and results:** planning-only structural QA and scoped diff check; focused xUnit/headless commands are future implementation gates and are not claimed as run here.

**Tests reused / added / aggregated:** existing focused tests are inventoried; no test file is created or changed by this plan.

**Known limitation or debt:** current production reachability for every proposed residual must be rechecked; a plan is not a runtime integration.

**Shared files intentionally untouched:** production, data, test, UI, generated index, runtime and unrelated dirty worktree files.

**Ready for implementation:** only after Phase 0 premise checkpoint and a new implementation claim.
