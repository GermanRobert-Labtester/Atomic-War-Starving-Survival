# ASHFALL: NOBODY'S CHARTER — Integration & Architectural Pipeline

> **Source bible:** `docs/expansions/expansion_03_nobodys_charter_plan.md` (1119 lines, status "for review — no data, no C#").
> **EiC intent:** "integrate fully the plan in phases," via an architectural pipeline that mirrors how the two sister packs (Holdfast, Duty Roster) were already integrated.
> **Authoritative editor:** Unity `6000.5.5f1` — batch EditMode gate: `$HOME/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics -projectPath . -runTests -testPlatform EditMode`.
> **House rules enforced at every gate:** save/load safe (`ISaveable`), event-driven (events raised on state change), ids in a master list (never a new string literal), no seventh `faction_lore.json` Power, no `WorldStateConsequenceSystem._hegemony` entry, cross-tool QA for any system introducing ≥2 coupled variables.

---

## 1. Architectural stance (why this shape)

Nobody's Charter is integrated exactly like the two sister packs, because it must read their flags (Appendix A of the bible) and must stand alone when they are absent.

| Concern | Decision | Evidence in codebase |
|---|---|---|
| Factions | New `crossing_factions.json` (Currents-shaped DTO) — **not** `faction_lore.json` | `holdfast_factions.json` + `HoldfastFactionsCatalogLoader` |
| Locations | New `crossing_locations.json`, merged into the live `LocationCatalogSO` at boot | `HoldfastLocationsCatalogLoader.ApplyToCatalog` |
| Quests | New `crossing_quests.json` + host-wired chain; quest ids registered in a master constant class | `HoldfastQuestCatalogLoader`, `QuestlineSO.Ids`, `ExpansionQuestConstants` |
| Items | New `crossing_items.json` + loader; merged into `ItemCatalogSO` | `HoldfastItemsCatalogLoader.MaterialiseAll` |
| NPCs / companions | `characters.json` additions + `NPC_*` classes + Utility-AI bias hooks | `CharactersCatalogLoader`, `GameBootstrap.DeepLoreWiring.DiscoverCharactersAtLocation` |
| Save/load | Every new stateful system registers via `SaveSystem.Register(ISaveable)` | `GameBootstrap.ExpansionSaveables`, `ISaveable`, `IceRoadSystemState` |
| Endings / mutation | Wired through `WorldStateConsequenceSystem.TryApplyMutation` + world-history paragraphs | `WorldStateConsequenceSystem` const mutations |
| Boot | New partial `GameBootstrap.NobodyCharter.cs`, called from `InitDeepLore` after `BootCurrents()` / `BootHoldfast()` | `GameBootstrap.DeepLoreWiring.InitDeepLore` |
| Events / choices | `EventRunner` pool + `EventRunner.OnChoiceApplied` host hooks | `GameBootstrap.Holdfast.cs` choice wiring |

### Id constraint (mandatory)
Re-grep **every** new id against `locations.json`, `currents.json`, `faction_lore.json`, `characters.json`, `items.json`, Holdfast + Duty Roster ids **before** writing data. No collisions. No seventh Power.

### Coupled-variable QA gate
Systems introducing ≥2 coupled variables (e.g. vouch state ↔ Standing backers ↔ debt terms) MUST be reviewed by a different tool than the one that wrote it — reviewer sees the diff + spec only (Prompt #26).
---

## 2. The pipeline (phases)

Each phase is a complete, compilable, committable step. "One system per task."

### Phase 0 — Foundations
- Locate + read the source bible. ✱ done
- Verify existing expansion-integration architecture (data → loader → boot partial → save → tests). ✱ done
- Baseline gate: batch EditMode green **before** edits (inherited tree is dirty — establish a known-good compile timestamp).

### Phase 1 — Crossing gate + founding catalog  ← *implemented in this change-set*
The bible's Appendix C Sprint 1. Establishes the region's identity and the **social** gate.
- `Assets/StreamingAssets/Data/crossing_factions.json` — Scale / Underwrite / Compact, Currents-shaped `{id, display_name, alignment, home_region, is_active, trust, wants, offers, signature_quote, access_rule, badge_asset_id}`.
- `Assets/_Game/Data/CrossingFactionsCatalogLoader.cs` — loader + `GetById`, mirrors `HoldfastFactionsCatalogLoader`.
- `Assets/_Game/Core/VouchAccessSystem.cs` — **the gate.** Plain C#; state: `vouchedBy`, `vouchBurned`, `accessSoftened`, `lastResortUsed`. Events: `OnVouchGranted`, `OnVouchBurned`, `OnAccessSoftened`. `ISaveable` (`CaptureState`/`RestoreState`). Bible §5.2.
- Register opening-arc quest ids in a master constant class `CrossingIds`.
- `characters.json` → add `npc_osran_kell`, `npc_mattis_cray`.
- `NPC_OsranKell.cs`, `NPC_MattisCray.cs` — state classes + events + save, mirroring `NPC_Undertow`.
- `crossing_locations.json` → `loc_crossing_viaduct_gate`, `loc_crossing_scalehouse`, `loc_crossing_stallrow`, `loc_crossing_watchtower`; merge at boot.
- `GameBootstrap.NobodyCharter.cs` — `BootNobodyCharter()` partial + boot call from `InitDeepLore`; saveable registration.
- `Assets/Tests/EditMode/NobodyCharterVouchAccessTests.cs` — vouch grant / burn / soften / save-round-trip / idempotent restore.
- **Gate:** JSON validation + batch EditMode compile+tests green. Cross-tool review of vouch × (future) backers × (future) debt.

### Phase 2 — Scale bloc (first weigh, Stallrow trade, calibration)  ← *implemented in this change-set*
- `loc_crossing_weighbridge`, `loc_crossing_underwrite_hall`, `loc_crossing_records_room`. ✱ added
- `crossing_items.json`: `item_vouch_token_crossing`, `item_calibration_weight`, trade goods. ✱ added + loader
- SCALE quests: `quest_crossing_first_weigh`, `quest_crossing_scale_integrity` (side). ✱ registered (cards + CrossingIds)
- Osran companion behaviour surface. Gate: compile + tests. ✱ done

### Phase 3 — CrossingArbitrationSystem (Standing) + `quest_crossing_the_standing`  ← *implemented in this change-set*
- Bible §5.1 + main quest. `StandingRuling {topic, backers[], shape}`; **3-backer rule**; overturn support; principled backer cap on bribery; events `OnStandingCalled/RulingMade/RulingOverturned`. Save/load.
- ✱ System + 3-backer rule + overturn + principled majority (pre-existing from the big integration commit); this change-set adds the missing spec surfaces:
  - `TryBribeBacker` / `BribeResult {Invalid, Accepted, RefusedPrincipled}` — principled backers refuse outright and the refusal is a public mark (`bribeMarks`, `refusedBribes` recorded once, deduped); a bought ruling holds **Rigged, never Honest**; `OnBribeRefused` event.
  - Re-Standing: an overturned ruling can be re-Stood (`CallStanding` on an Overturned topic starts a fresh pending ruling; `GetRuling` = latest match; `GetRulingHistory` keeps the board's history; `standingRepeats` counter). Nothing is permanently settled.
  - Overturn validation: counters must be 3+ distinct, living backers, a *different* set from the holders (§5.1 "a different 3+ backers").
  - `IsRulingActive` (held or bought = on the board) for "who controls X" queries; `IsRulingHeld` stays honest-only.
  - `_Game/Core` `CaptureState` fixed to deep-copy (snapshot semantics) + null-safe `RestoreState`.
  - Host wiring: `GameBootstrap.TryBribeCrossingBacker`, `OnBribeRefused` log; headless demo + core tests + EditMode tests extended (bribe cap, re-Standing, overturn validation, snapshot isolation, round-trip of new fields).
- **Gate:** cross-tool QA (vouch × backers coupling) — reviewer (different tool) returned FAIL with 7 findings; 6 addressed in this change-set (deep-copy snapshot, refusal dedup/record, overturn "different set" validation, Honest/Rigged + IsRulingActive docs, §5.1 header, fixture-id check = false alarm — all ids are in `characters.json`); finding 1 (dual-copy `_Game`/`Ashfall.Core` fork) is a pre-existing architecture debt of the whole expansion suite — tracked as a follow-up de-fork task, not Phase 3 scope. `dotnet test` green + Godot build clean (worktree-isolated).

### Phase 4 — Underwrite + Compact blocs + LedgerDebtSystem  ← *implemented in this change-set*
- `LedgerDebtSystem` §5.3 (`DebtContract{debtorId,principal,termDays,rate,forfeit}`), contract-shown-twice, forfeit named up front, `OnContractSigned/Paid/Renegotiated/OnForfeitTriggered/OnLedgerTampered`. Save/load.
- ✱ **De-forked:** the divergent `_Game/Core/LedgerDebtSystem.cs` host twin is deleted; the Unity host now consumes the single engine-agnostic `Ashfall.Core.LedgerDebtSystem` (bootstrap, `LedgerDebtSaveable`, EditMode tests updated to the core API — read-twice `PresentContract`, `SignContract(debtorId, day)`, `PayContract`, `TickDaily` forfeit, one-shot `TamperLedger`, `TotalOwed`).
- ✱ **Term-end renegotiation** (§5.3 "on term end: … renegotiated"): signed ink can be renegotiated only on the last day of its term (extends term, adjusts rate, forfeit stays named); no silent amendment mid-term. Contested renegotiation is gated at the host layer by a fresh Standing (`GameBootstrap.RenegotiateCrossingContract(..., contested, standingTopic)` → requires `Arbitration.IsRulingHeld`).
- ✱ **Bloc POIs added** to `crossing_locations.json`: `loc_crossing_the_lockup`, `loc_crossing_granary_pledge`, `loc_crossing_nightfire` (Underwrite), `loc_crossing_petition_tent`, `loc_crossing_founders_marker`, `loc_crossing_the_annex` (Compact) — resolves the dangling `petition_tent` quest target; ids registered in `CrossingIds.Locations`.
- ✱ **Wyn Sabler**: `npc_wyn_sabler` added to `characters.json` (was missing); `NPC_WynSabler.cs` (terms-recital / flee-with-grain / honoured paths, events, save); `CrossingIds.Npcs.WynSabler`; bootstrap wiring + `WynSablerSaveable` + event-driven registration.
- ✱ Core test/demo debtor ids fixed from wrong stand-ins (`npc_wren`, `npc_ivor_lasko`) to the canonical `npc_wyn_sabler` / `npc_ivo_fenn`.
- `quest_crossing_the_terms`, `quest_crossing_the_petition`, `quest_crossing_the_forfeit`, `quest_crossing_the_vote_that_isnt` — cards already registered in `crossing_quests.json` (✱ verified).
- Dessa + Perrin + Ivo NPCs ✱ already present; Wyn added above.
- **Gate:** cross-tool QA (debt × vouch × backers × forfeit) — reviewer (fresh sub-agent, diff + spec §5.3 only) returned **FAIL with 8 findings; 6 fixed in this change-set**:
  1. *(blocker)* Contested-renegotiation Standing gate lived in an optional host wrapper (`contested` opt-in, bypassable by direct core calls, no freshness check) → gate moved INTO `LedgerDebtSystem.RenegotiateContract(contested, freshStanding)`; host composes the callback (`IsCrossingStandingFresh`: held honestly AND `dayCalled` within new `LedgerDebtSystem.StandingFreshDays = 3`; no day supplied → no pass).
  2. *(major)* `TotalOwed` ignored `paid` → settled debt now owes 0.
  3. *(major)* Paid contract locked the debtor out forever and could be silently overwritten by `PresentContract` → settled ink is **archived** (`closedContracts`, never rewritten; round-trips through save); unresolved forfeit also blocks a new draft.
  4. *(minor)* `NPC_WynSabler.CaptureState` aliased live state → deep copy both directions (ledger `RestoreState` now also takes a defensive copy).
  5. *(minor)* Dead `CrossingQuestEntry` using-alias removed; stray trailing newline in EditMode tests.
  - NOT fixed here (tracked): the `_Game/Core` twins of `CrossingArbitrationSystem`/`VouchAccessSystem` (pre-existing suite-wide de-fork follow-up from Phase 3; the Phase 4 seam itself no longer depends on them via the callback gate); Duty Roster saveable ids with no characters.json rows belong to the parallel Duty Roster stream, not this change-set.
  - Core tests +7 (paid-total, archive/no-overwrite, forfeit blocks draft, contested gate ×3, closed-contract round-trip); EditMode +2.

### Phase 5 — The Charter mystery
- `quest_crossing_the_marker`, `quest_crossing_three_dry_pages`; `item_charter_three_pages`; records-room truth; `mutation_crossing_charter_revealed`. Gate: compile + tests.

### Phase 6 — Endings + world-state mutation + enc/crisis layer
- Endgame `quest_crossing_who_holds_the_ledger`; 4 narrative endings + `ending_crossing_none`; `world_history` second paragraph at records_room/weighbridge.
- 10 `enc_nc_*` encounters + 5 crises wired to existing `ExpeditionSystem`/crisis pacing.
- Gate: PlayMode smoke + full EditMode; cross-tool QA.

### Phase 7 — Hardening + full-suite + docs
- Companion Utility-AI actions (`Action_WeighGoods`, `Action_ReadContract`, `Action_CanvasSupport`, `Action_RunVouch`, seed `_worldSeed+1811`).
- Full EditMode + PlayMode, cold-clone build, update `docs/CI.md` pass count.

---

## 3. Recurring setup (per phase)
1. Restate goal in 2 lines. 2. List files touched/created. 3. Re-grep id collisions. 4. Implement (thin MonoBehaviours; logic in plain C#; events). 5. Save-wire via `SaveSystem.Register`. 6. Verify batch EditMode. 7. Commit per AGENTS.md. 8. Provide exact next prompt.

## 4. Cross-tool QA register
| System | Coupled vars | Reviewer (not implementer) | Outcome |
|---|---|---|---|
| `VouchAccessSystem` (P1) | vouch state × future Standing backers × future debt terms | must review diff only | — |
| `CrossingArbitrationSystem` (P3) | ruling backers × vouch × Standing repeats | different tool (sub-agent) | FAIL → findings addressed in change-set (see Phase 3 gate notes); re-verify in Phase 4 gate |
| `LedgerDebtSystem` (P4) | contract × vouch × backers × forfeit | different tool (sub-agent, diff + spec §5.3 only) | FAIL (8 findings) → 6 fixed in change-set (see Phase 4 gate notes); 2 deferred as tracked debt (Arbitration/Vouch twin de-fork; Duty Roster stream ids) |
