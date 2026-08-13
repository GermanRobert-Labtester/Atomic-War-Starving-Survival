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

### Phase 3 — CrossingArbitrationSystem (Standing) + `quest_crossing_the_standing`
- Bible §5.1 + main quest. `StandingRuling {topic, backers[], shape}`; **3-backer rule**; overturn support; principled backer cap on bribery; events `OnStandingCalled/RulingMade/RulingOverturned`. Save/load.
- Gate: **cross-tool QA** (vouch × backers coupling).

### Phase 4 — Underwrite + Compact blocs + LedgerDebtSystem
- `LedgerDebtSystem` §5.3 (`DebtContract{debtorId,principal,termDays,rate,forfeit}`), contract-shown-twice, forfeit named up front, `OnContractSigned/Paid/Renegotiated/OnForfeitTriggered/OnLedgerTampered`. Save/load.
- `quest_crossing_the_terms`, `quest_crossing_the_petition`, `quest_crossing_the_forfeit`, `quest_crossing_the_vote_that_isnt`.
- Dessa + Perrin + Wyn + Ivo NPCs. Gate: **cross-tool QA** (debt × vouch × backers).

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
| System | Coupled vars | Reviewer (not implementer) |
|---|---|---|
| `VouchAccessSystem` (P1) | vouch state × future Standing backers × future debt terms | must review diff only |
| `CrossingArbitrationSystem` (P3) | ruling backers × vouch × Standing repeats | must review diff only |
| `LedgerDebtSystem` (P4) | contract × vouch × backers × forfeit | must review diff only |