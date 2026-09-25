# Plan 54 — Combat Catalog Expansion: Current-Seam Integration Architecture

> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE-ROUND-7`
> **Claim:** `claim-oldest-15-piagents-quality-rebase-round7-2026-09-25`
> **Document class:** evidence-backed implementation plan; planning-only artifact
> **Domain:** ballistics, combat authority, and equipment condition
> **Read-only design authority:** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
> **Authority SHA-256:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
> **Target depth:** 150k–170k intermediate quality checkpoint, then 250,000+ characters as a quality target, not a ceiling; no padding or unsupported completion claims
> **Scope:** Core/data/host/UI/save/determinism architecture and verification planning only

## Executive summary

Expand combat content only through the live TacticalCombatSystem, CombatCatalog, BallisticsSystem, inventory/equipment bridge, and existing save/test contracts.

This document supersedes stale generated or historical claims in the selected plan path. It distinguishes current evidence, required delta, safe extension seam, ownership, persistence, determinism, presentation, failure behavior, focused verification, rollback, and the remaining implementation handoff. It is not an implementation report.

## Selection and premise record

- Original Git `HEAD` baseline: `5759` characters.
- Current worktree copy: `524304` characters before this rebuild.
- Selection rule: next-lowest original `HEAD` character count after excluding every path completed in Rounds 1–6 and any active claim.
- Current worktree generated text was not used as proof of implementation because it may contain stale counts, repetitive expansion, or unsupported pass language.
- User-supplied authority path contained a spacing variation; the canonical repository path used here is `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`.

## Current source and data dossier

### Current evidence: `Assets/Ashfall.Core/Combat/CombatCatalog.cs`
- Role: current source/owner candidate
- Worktree status: `M Assets/Ashfall.Core/Combat/CombatCatalog.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d16451f581da6da106751f340f2aa5ad50a52ddea05c5634508bf6846365bb68`
- Snapshot size: 32516 characters; 677 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0096:     /// </summary>
0097:     public static class CombatCatalog
0098:     {
0099:         private static readonly Dictionary<string, CombatWeaponDefinition> s_weapons =
...
0109:         {
0110:             // Data authority is JSON (Assets/StreamingAssets/Data/combat_catalog.json).
0111:             // This populates the registry from that file so code never owns the
0112:             // values (Invariant #6). Idempotent: a host that already loaded the
...
0123:             if (!string.IsNullOrEmpty(dataDir))
0124:                 CombatCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
0125:
0126:             // Crash-avoidance fallback for environments with no data directory
...
0258:             if (string.IsNullOrEmpty(combatantId)) return null;
0259:             var def = CombatCatalog.GetCombatant(combatantId);
0260:             if (def == null) return null;
0261:             return Build(def);
...
0271:         {
0272:             var def = CombatCatalog.GetCombatant(combatantId)
0273:                 ?? throw new System.Collections.Generic.KeyNotFoundException(
0274:                     "CombatantFactory: no registered combatant with id '" + combatantId + "'.");
...
0287:                 return false;
0288:             var def = CombatCatalog.GetCombatant(combatantId);
0289:             if (def == null) return false;
0290:             result = Build(def);
...
0299:             // legacy hand-coded `new CombatantState { Id = "enemy_..." }`
0300:             // token inside TacticalCombatSystem.cs and src/Host/CombatHostSession.cs
0301:             // already does this; the factory must not insert
0302:             // nondeterministic ids into the simulation (Invariant 4).
...
0325:     // ---------------------------------------------------------------------
0326:     // Combat data authority loader — reads Assets/StreamingAssets/Data/combat_catalog.json
0327:     // ---------------------------------------------------------------------
0328:
...
0391:     [Serializable]
0392:     internal sealed class CombatCatalogRoot
0393:     {
0394:         public int schema_version = 1;
...
0403:     /// Engine-agnostic loader for the Combat data authority
0404:     /// (combat_catalog.json, snake_case). Maps onto the camelCase runtime
0405:     /// definitions. Returns false (leaving the registry untouched) when the
0406:     /// file is absent; surfaces parse/schema errors as exceptions so a bad
...
0410:     {
0411:         public const string FileName = "combat_catalog.json";
0412:         public const int CurrentSchemaVersion = 2;
0413:
...
```

### Current evidence: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`
- Role: current source/owner candidate
- Worktree status: `M Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a2f1de89201692d23312afa55cc4f532506f56299928bd00c13774c877b7c25b`
- Snapshot size: 21383 characters; 440 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0031:     /// </summary>
0032:     public partial class TacticalCombatSystem
0033:     {
0034:         public const string SystemId = "combat_system";
...
0037:         public const int DefaultSuppressDuration = 1;
0038:         public const int MaxRicochetBounces = BallisticsSystem.MaxRicochetCount;
0039:
0040:         private CombatState _state = new CombatState();
...
0058:
0059:         public TacticalCombatSystem(CombatState? state = null, CombatHostPorts? ports = null)
0060:         {
0061:             if (state != null) _state = state;
...
0274:                 var w = _state.Weapons[i];
0275:                 var def = CombatCatalog.GetWeapon(w.WeaponId);
0276:                 if (def != null && string.IsNullOrEmpty(w.AmmoId)) w.AmmoId = def.caliber;
0277:                 if (w.AmmoRemaining <= 0 && def != null) w.AmmoRemaining = def.burst * 10;
...
0351:             if (target == null || target.IsPlayer || target.HasFled) return ActionPreflight.Blocked("Invalid or eliminated target");
0352:             var def = CombatCatalog.GetWeapon(weapon.WeaponId);
0353:             int burst = def != null ? Math.Max(1, def.burst) : 1;
0354:             var mods = GetStanceMods(CurrentStance());
...
0368:             if (weapon == null) return ActionPreflight.Blocked("Survivor has no weapon");
0369:             var def = CombatCatalog.GetWeapon(weapon.WeaponId);
0370:             if (def == null || !def.isSuppressionCapable) return ActionPreflight.Blocked("Weapon cannot suppress (requires rifle or LMG)");
0371:             if (weapon.IsJammed) return ActionPreflight.Blocked("Weapon is jammed (clear jam first)");
```

### Current evidence: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs`
- Role: current source/owner candidate
- Worktree status: `M Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `70dc102c1569f7bc447dfba3592a71373f5f3a76bee1366b489c872e9aa6db76`
- Snapshot size: 26288 characters; 583 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0007: {
0008:     public partial class TacticalCombatSystem
0009:     {
0010:         // ══ Player actions & Hit Resolution ══════════════════════════════
...
0064:
0065:             var def = CombatCatalog.GetWeapon(weapon.WeaponId);
0066:             if (def == null) { res.Message = "Unknown weapon: " + weapon.WeaponId; return res; }
0067:
...
0088:                 {
0089:                     res.Message = "No " + (CombatCatalog.GetAmmo(weapon.AmmoId)?.displayName ?? weapon.AmmoId) + " ammunition.";
0090:                     Notify();
0091:                     return res;
...
0109:             // ── Degradation ──
0110:             float degrade = WeaponConditionSystem.ComputeDegradePerBurst(weapon) * mods.Degrade;
0111:             WeaponConditionSystem.Degrade(weapon, degrade);
0112:
...
0123:
0124:             bool burstFailure = WeaponConditionSystem.TryWeaponBurst(weapon, rng);
0125:             if (burstFailure)
0126:             {
...
0135:             // ── Ballistics ──
0136:             var ammo = CombatCatalog.GetAmmo(weapon.AmmoId);
0137:             var coverMaterial = CombatCatalog.GetMaterial("material_concrete"); // default rubble cover
0138:             var armorMaterial = CombatCatalog.GetMaterial(GetArmorMaterialId(shooter)!);
...
0174:
0175:             var outcome = BallisticsSystem.Resolve(ctx, rng);
0176:
0177:             var ev = new CombatEvent
...
0213:             if (weapon == null) { res.Message = shooter.Name + " has no weapon."; return res; }
0214:             var def = CombatCatalog.GetWeapon(weapon.WeaponId);
0215:             if (def == null || !def.isSuppressionCapable)
0216:             {
...
0268:             var perks = PerksFor(c.SurvivorId, _state.Seed);
0269:             int ticks = perks != null ? perks.GetJamClearTicks(c.SurvivorId) : WeaponConditionSystem.DefaultJamClearTicks;
0270:             bool cleared = WeaponConditionSystem.TickJamClear(weapon, ticks);
0271:             perks?.RecordWeaponJamSurvived(c.SurvivorId);
...
0341:
0342:             int cost = WeaponConditionSystem.GetScrapRepairCost(weapon);
0343:             if (cost <= 0)
0344:                 return CommandPreview.Unavailable(PlayerCommandCode.RepairWeapon, "no_scrap_needed", "combat.no_scrap_needed", stateVersion);
...
0392:
0393:             int cost = WeaponConditionSystem.GetScrapRepairCost(weapon);
0394:             bool ok = _condition.TryFieldRepair(weapon, _ports);
0395:             if (!ok)
...
```

### Current evidence: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Damage.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `d9c8c44d0fbe1d0df69fc809bc1fb7c4abbe7ba69319e8a84e9422e5f3c570d3`
- Snapshot size: 14351 characters; 356 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0006: {
0007:     public partial class TacticalCombatSystem
0008:     {
0009:         // ══ Damage, Status, Turn Management & Resolution ═════════════════
...
0169:                 if (w.OwnerSurvivorId == null) continue;
0170:                 WeaponConditionSystem.ExposeToAsh(w, severity);
0171:                 affected++;
0172:             }
```

### Current evidence: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Targeting.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `43781303a0d5c1e5ebe17295e7107b870ba69349e20bd96d221e0f10ee365ab8`
- Snapshot size: 4588 characters; 130 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0006: {
0007:     public partial class TacticalCombatSystem
0008:     {
0009:         // ══ Targeting / query helpers ═════════════════════════════════════
...
0126:             var w = WeaponOf(c);
0127:             return w == null ? 0f : WeaponConditionSystem.ComputeJamChance(w);
0128:         }
0129:     }
```

### Current evidence: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs`
- Role: current source/owner candidate
- Worktree status: `M Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `976900f9a0545ba959e38530abb0c6addafd71956995542b20c21054c8e1c498`
- Snapshot size: 20949 characters; 458 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0006: {
0007:     public partial class TacticalCombatSystem
0008:     {
0009:         // ══ Snapshot for the host / UI ════════════════════════════════════
...
0048:                     IsLastStand = c.IsLastStand,
0049:                     WeaponName = w != null ? (CombatCatalog.GetWeapon(w.WeaponId)?.displayName ?? w.WeaponId) : "—",
0050:                     WeaponConditionPct = w != null ? (int)Math.Round(w.ConditionPct * 100f) : 0,
0051:                     WeaponJammed = w != null && w.IsJammed,
...
0065:                 var w = weapons[i];
0066:                 var def = CombatCatalog.GetWeapon(w.WeaponId);
0067:                 snap.Weapons.Add(new WeaponSnapshot
0068:                 {
...
0072:                     ConditionPct = (int)Math.Round(w.ConditionPct * 100f),
0073:                     JamChancePct = (int)Math.Round(WeaponConditionSystem.ComputeJamChance(w) * 100f),
0074:                     IsJammed = w.IsJammed,
0075:                     ScrapRepairCost = WeaponConditionSystem.GetScrapRepairCost(w),
```

### Current evidence: `Assets/Ashfall.Core/Combat/BallisticsSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `cb82487673a5c437bec30e4f577088dae9f5845a2eb8e1aaf6dc3cd5a1ed7119`
- Snapshot size: 14753 characters; 330 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0079:     /// </summary>
0080:     public static class BallisticsSystem
0081:     {
0082:         public const int MaxRicochetCount = 2;
```

### Current evidence: `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a870ad96a12001a9821b7ac883fe2606e0e3241333b4cfe4b8abf86f312561ba`
- Snapshot size: 15129 characters; 319 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0129:     /// </summary>
0130:     public class WeaponConditionSystem
0131:     {
0132:         public const float Pristine = 1f;
...
0149:             if (weapon == null) return 0f;
0150:             var def = CombatCatalog.GetWeapon(weapon.WeaponId);
0151:             if (def == null) return 0f;
0152:
...
0183:             if (weapon == null) return 0f;
0184:             var def = CombatCatalog.GetWeapon(weapon.WeaponId);
0185:             if (def == null) return 0f;
0186:             int burst = Math.Max(1, def.burst);
...
0235:             if (weapon == null || rng == null) return false;
0236:             var def = CombatCatalog.GetWeapon(weapon.WeaponId);
0237:             if (def == null || !def.isJuryRigged) return false;
0238:             if (!WAmmoIsMilitary(weapon.AmmoId)) return false;
...
0255:             if (weapon == null) return 0;
0256:             var def = CombatCatalog.GetWeapon(weapon.WeaponId);
0257:             if (def == null) return 0;
0258:             // Cost scales with how far the weapon has degraded.
...
0314:         {
0315:             var a = CombatCatalog.GetAmmo(ammoId);
0316:             return a != null && a.isMilitaryTier;
0317:         }
```

### Current evidence: `Assets/Ashfall.Core/Combat/CombatTypes.cs`
- Role: current source/owner candidate
- Worktree status: `M Assets/Ashfall.Core/Combat/CombatTypes.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `12ece45da14bdf6d0d7648998528cc4d0c86eea3d1e34010ca3a8d2fb7e828bf`
- Snapshot size: 18164 characters; 458 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0116:
0117:         // ── AI trait fields populated from CombatCatalog (Plan 10 ─
0118:         // strata populated by <see cref="Ashfall.Core.Combat.CombatantFactory"/>
0119:         // when an encounter setup hands it a `combatant_*` id. Defaults are
...
0309:
0310:         public string SystemId = TacticalCombatSystem.SystemId;
0311:         public int SaveVersion = CurrentSaveVersion;
0312:         public string EncounterId = string.Empty;
...
0319:         public int Phase = (int)CombatPhase.Setup;
0320:         public string PlayerStance = TacticalCombatSystem.StanceId(TacticalStance.HoldPosition);
0321:         public int RoundNumber = 0;
0322:         public bool Resolved;
```

### Current evidence: `src/Host/CombatHostSession.cs`
- Role: current source/owner candidate
- Worktree status: `M src/Host/CombatHostSession.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `bbf3298128d237590932b551ca7d02cb5f36c41b3bee5b94fd58bcbdbc5aa64a`
- Snapshot size: 35728 characters; 832 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0021:     /// </summary>
0022:     public sealed class CombatHostSession : HostSessionBase, IWiringReporter
0023:     {
0024:         public const int DemoSeed = 4242;
...
0029:
0030:         public TacticalCombatSystem Engine { get; }
0031:         public event Action<CombatFactionConsequence>? FactionConsequenceApplied;
0032:
...
0071:         public string LastEvent { get; private set; } = string.Empty;
0072:         public CombatHostSession(TacticalCombatSystem engine = null!, CombatHostPorts ports = null!, ICampaignRngManager? campaignRng = null)
0073:         {
0074:             _campaignRng = campaignRng;
...
0114:                 state.AppliedFactionConsequenceIds);
0115:             state.FactionConsequences = TacticalCombatSystem.CloneFactionConsequences(
0116:                 new List<CombatFactionConsequence>(consequences));
0117:             return applied;
...
0257:
0258:         public static CombatHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null)
0259:         {
0260:             // The weapon/ammo/material catalog is the data authority
...
0274:                 {
0275:                     CombatCatalogLoader.Load(dataDir, files, json);
0276:                 }
0277:                 catch (Exception ex)
...
0281:             }
0282:             if (CombatCatalog.GetWeapon("weapon_assault_rifle") == null)
0283:                 CombatCatalog.SeedDefaults();
0284:
...
0386:                 // The primary tracked weapon must go to the survivor the
0387:                 // ENGINE will actually shoot with. TacticalCombatSystem resolves
0388:                 // its shooter over LivingPlayers(), which SORTS by ordinal
0389:                 // combatant Id — so it is NOT players[0] (roster order). Assigning
...
0489:         {
0490:             if (TacticalCombatSystem.TryParseStance(stanceId, out var s))
0491:             {
0492:                 var r = Engine.SetStance(s);
...
0628:             const int maxPumps = 5;
0629:             while (_realtimeAccum >= TacticalCombatSystem.RealtimeSimDt && pumps < maxPumps)
0630:             {
0631:                 Engine.TickRealtime(TacticalCombatSystem.RealtimeSimDt, _realtimeInput, new SeededRng(RollSeed()));
...
0820:             {
0821:                 SessionId = "CombatHostSession",
0822:                 RequiredCollaborators = reqCollabs,
0823:                 BoundCollaborators = boundCollabs,
```

### Current evidence: `src/Host/CombatSaveStore.cs`
- Role: current source/owner candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `118e5f5bbab00d775a7fe8b4e09a94849e84300eef61d3f0f1bbcdaede97fc1a`
- Snapshot size: 2285 characters; 49 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0004: // Core State : Ashfall.Core.Combat.CombatState
0005: // Host Caller: Main.Expeditions / CombatHostSession
0006: // Purpose    : Tactical combat encounters, ballistics resolution, enemy status, and weapon wear
0007: // ============================================================================
```

### Current evidence: `src/UI/CombatPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `M src/UI/CombatPanel.cs`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `329125dc5858e70419b0b7c87bcb4d7a9f757affc3af2c21f5eab3677b50a4a4`
- Snapshot size: 22446 characters; 491 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0012:     ///
0013:     /// Presents the live CombatHostSession: active encounter, phase, turn, stance,
0014:     /// combatants (lane / health / cover / armor / downed / pinned / last-stand),
0015:     /// weapon condition, jam state and ammunition, combat log and captured loot.
...
0024:
0025:         private CombatHostSession _combat = null!;
0026:         private bool _bound;
0027:
...
0054:         /// <summary>Typed binding to the real combat host session.</summary>
0055:         public void Bind(CombatHostSession combat)
0056:         {
0057:             _combat = combat;
...
0329:             var row3 = Row();
0330:             _btnHold = Btn("HOLD LINE", () => DoAction(() => _combat.ActionStance(TacticalCombatSystem.StanceId(TacticalStance.HoldPosition))));
0331:             row3.AddChild(_btnHold);
0332:             _btnAdvance = Btn("ADVANCE", () => DoAction(() => _combat.ActionStance(TacticalCombatSystem.StanceId(TacticalStance.Advance))));
```

### Current evidence: `src/UI/CombatDetailPanel.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `454cc0e5df3439afbc8074a08609ba0ec26194701bfef75ac3595e43c65548c7`
- Snapshot size: 6593 characters; 159 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0011:     /// ASHFALL — Combat Detail panel (real integration).
0012:     /// Reads the live CombatHostSession snapshot: battle information, current
0013:     /// tactical stance and its trade-offs, casualties &amp; losses, and outcomes
0014:     /// (captured loot, morale/injury consequences reaching real state).
...
0019:
0020:         private CombatHostSession _combat = null!;
0021:         private bool _bound;
0022:
...
0030:
0031:         public void Bind(CombatHostSession combat)
0032:         {
0033:             _combat = combat;
...
0055:             AddLine(_tacticsData, "Current Stance: " + snap.StanceId);
0056:             var hold = TacticalCombatSystem.GetStanceMods(TacticalStance.HoldPosition);
0057:             var adv = TacticalCombatSystem.GetStanceMods(TacticalStance.Advance);
0058:             var sup = TacticalCombatSystem.GetStanceMods(TacticalStance.SuppressiveFire);
```

### Current evidence: `Assets/StreamingAssets/Data/combat_catalog.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `e837b53cbab1b7595a12f58c09cd7dbac753ef1736a4fbf69757a4d83f36a1c6`
- Snapshot size: 20771 characters; 549 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0002:   "schema_version": 2,
0003:   "collection_id": "combat_catalog",
0004:   "weapons": [
0005:     {
```

### Current evidence: `Assets/StreamingAssets/Data/items.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
- Snapshot size: 390056 characters; 9660 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "items": [
0004:     {
0005:       "id": "item_decon_chelator_concentrate",
0006:       "displayName": "Chelator Concentrate",
0007:       "description": "A sealed glass ampoule of chelating agent concentrate. The label is half dissolved but the formula is standard: binds heavy radionuclides into a water-soluble complex for rinse removal. One ampoule per decon cycle. The pre-war stock won't last forever, and the synthesis requires a working pharma bench.",
0008:       "type": "Consumable",
0009:       "stackMax": 20,
0010:       "weight": 0.2,
0011:       "tradeValue": 8
0012:     },
0013:     {
0014:       "id": "item_lead_lined_effluent_filter",
0015:       "displayName": "Lead-Lined Effluent Filter",
0016:       "description": "A cylindrical filtration cartridge with a lead-foil inner liner and activated charcoal matrix. Installed in the decon airlock effluent tank to capture radionuclide-laden particulates before they can be sluiced into the general water system. Good for approximately five hundred liters of contaminated wash water before replacement is required.",
0017:       "type": "Equipment",
0018:       "stackMax": 5,
0019:       "weight": 3.5,
0020:       "tradeValue": 15
```

### Current evidence: `Assets/StreamingAssets/Data/ballistics_workbench_catalog.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `1ff41be85b6728816fc3e9e8a01020bbd4034ef6d5cd9be98bc8a6f01bfe27aa`
- Snapshot size: 1116 characters; 33 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "profiles": [
0004:     {
0005:       "id": "part_ballistics_rifle_standard",
0006:       "profile_id": "part_ballistics_rifle_standard",
0007:       "weapon_tag": "rifle",
0008:       "base_dispersion_moa": 3.8,
0009:       "wear_per_shot_factor": 0.12,
0010:       "corrosive_ammo_wear_modifier": 1.45,
0011:       "overpressure_wear_modifier": 1.7,
0012:       "headspace_warning_threshold": 0.55,
0013:       "headspace_failure_threshold": 0.85,
0014:       "max_calibration_bonus": 0.2,
0015:       "maintenance_recipe_id": "recipe_ballistics_refurbish_rifle",
0016:       "supported_ammo_tags": ["ammo_rifle"]
0017:     },
0018:     {
0019:       "id": "part_ballistics_sidearm_standard",
0020:       "profile_id": "part_ballistics_sidearm_standard",
```

### Current evidence: `Assets/StreamingAssets/Data/combat_arenas.json`
- Role: authoritative JSON data candidate
- Worktree status: `?? Assets/StreamingAssets/Data/combat_arenas.json`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `de283146d88276f7cbaa247aa582a630d65dc87941502e96e7f44de478b475c6`
- Snapshot size: 1133 characters; 38 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "collection_id": "combat_arenas",
0004:   "arenas": [
0005:     {
0006:       "id": "arena_lane_spine_default",
0007:       "width": 24.0,
0008:       "height": 10.0,
0009:       "walk_speed": 2.4,
0010:       "run_speed": 4.8,
0011:       "climb_speed": 1.6,
0012:       "flee_speed": 5.2,
0013:       "lane_spines": [
0014:         { "lane": 0, "x": 4.0 },
0015:         { "lane": 1, "x": 12.0 },
0016:         { "lane": 2, "x": 20.0 }
0017:       ],
0018:       "cover_nodes": [
0019:         { "id": "cover_p_left", "x": 3.0, "y": 2.0, "radius": 1.2, "cover_rating": 0.35 },
0020:         { "id": "cover_e_right", "x": 21.0, "y": 2.0, "radius": 1.2, "cover_rating": 0.35 }
```

### Current evidence: `Assets/StreamingAssets/Data/defenses.json`
- Role: authoritative JSON data candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `c80afe579aa4c9ee8796f7e49d0b0c7861292120e36f8fcc692ee873a371874b`
- Snapshot size: 1917 characters; 65 lines
- Evidence excerpt (bounded; not a generated API):
```text
0001: {
0002:   "schema_version": 1,
0003:   "traps": [
0004:     {
0005:       "id": "trap_perimeter_snare",
0006:       "display_name": "Perimeter Snare Line",
0007:       "defense_type": "snare",
0008:       "base_strength": 1,
0009:       "max_hp": 60,
0010:       "activation_chance": 0.75,
0011:       "capture_chance": 0.15,
0012:       "concealed": true,
0013:       "placement_tag": "perimeter",
0014:       "build_costs": { "scrap_metal": 2 },
0015:       "reset_costs": { "scrap_metal": 1 },
0016:       "repair_costs": { "scrap_metal": 2 },
0017:       "tags": ["perimeter", "concealed"]
0018:     },
0019:     {
0020:       "id": "trap_choke_deadfall",
```

### Current evidence: `src/UI/CombatHudOverlay.cs`
- Role: Godot presentation surface candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `31c163d59941ba17a930db1ecadacead0d5f2c99fdc6ee453cc488cba6a5399f`
- Snapshot size: 20288 characters; 452 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0017: /// existing `CombatPanel.cs` (Phase 9 modal). The HUD reads from
0018: /// `Ashfall.Core.Combat.TacticalCombatSystem` via the user's own
0019: /// `CombatHostSession`. Four tiles:
0020: ///
...
0046:
0047:     private CombatHostSession? _host;
0048:
0049:     public bool IsBound => _host != null;
...
0157:         _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
0158:             "Bind a CombatHostSession to see live combat event stream."));
0159:         actionRow.AddChild(_detailBox);
0160:
...
0299:             _detailBox.AddChild(AshfallUiHelpers.MakeMetadata(
0300:                 "Combat HUD offline. Bind a CombatHostSession to see live combat event stream."));
0301:             return;
0302:         }
```

### Current evidence: `Ashfall.Core.Tests/Plan54CombatCatalogTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `678ce466638c151d1b14d5bea4679b8a01603adc967d38ce08601b28be4eec45`
- Snapshot size: 14583 characters; 339 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0024:
0025: public class Plan54CombatCatalogTests : CatalogTestBase
0026: {
0027:     private static void ReloadCatalog()
...
0038:         ReloadCatalog();
0039:         Assert.Equal(20, CombatCatalog.WeaponIds.Count);
0040:     }
0041:
...
0052:         foreach (var id in originalFive)
0053:             Assert.True(CombatCatalog.HasWeapon(id), $"baseline weapon {id} must remain registered");
0054:     }
0055:
...
0079:         {
0080:             var def = CombatCatalog.GetWeapon(kv.Key);
0081:             Assert.NotNull(def);
0082:             Assert.Equal(kv.Value, def!.caliber);
...
0099:         {
0100:             var def = CombatCatalog.GetWeapon(kv.Key);
0101:             Assert.True(def != null, $"Plan 54 weapon {kv.Key} missing from combat_catalog.json");
0102:             Assert.Equal(kv.Value, def!.caliber);
...
0116:         ReloadCatalog();
0117:         foreach (var id in CombatCatalog.WeaponIds)
0118:         {
0119:             var def = CombatCatalog.GetWeapon(id)!;
...
0130:         var tuples = new HashSet<string>();
0131:         foreach (var id in CombatCatalog.WeaponIds)
0132:         {
0133:             var def = CombatCatalog.GetWeapon(id)!;
...
0145:         ReloadCatalog();
0146:         var used = new HashSet<string>(CombatCatalog.WeaponIds.Select(w => CombatCatalog.GetWeapon(w)!.caliber));
0147:         Assert.Contains("ammo_762x54r", used);
0148:         Assert.Contains("ammo_12g_buck", used);
...
0157:         ReloadCatalog();
0158:         Assert.Equal(12, CombatCatalog.CombatantIds.Count);
0159:     }
0160:
...
0172:         foreach (var id in baseline)
0173:             Assert.True(CombatCatalog.HasCombatant(id), $"baseline combatant {id} must remain registered");
0174:     }
0175:
...
0180:
0181:         var veteran = CombatCatalog.GetCombatant("combatant_salvage_veteran");
0182:         Assert.NotNull(veteran);
0183:         Assert.Equal("human", veteran!.kind);
...
```

### Current evidence: `Ashfall.Core.Tests/CombatCatalogValidationTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `a454557b914be016da16a799ed0b24fe1ecb517c8148209f1213be3c32a8f13a`
- Snapshot size: 3716 characters; 97 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0015:     /// </summary>
0016:     public class CombatCatalogValidationTests
0017:     {
0018:         private static string TempDir()
...
0026:         {
0027:             return CombatCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
0028:         }
0029:
...
0033:         {
0034:             CombatCatalog.Clear();
0035:             CombatCatalog.SeedDefaults();
0036:         }
...
0039:         {
0040:             File.WriteAllText(Path.Combine(dir, CombatCatalogLoader.FileName), json);
0041:         }
0042:
...
0089:                 Assert.True(TryLoad(dir));
0090:                 Assert.NotNull(CombatCatalog.GetWeapon("weapon_x"));
0091:                 Assert.NotNull(CombatCatalog.GetAmmo("ammo_9"));
0092:                 Assert.NotNull(CombatCatalog.GetMaterial("material_rock"));
```

### Current evidence: `Ashfall.Core.Tests/CombatBallisticsTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `56a14ecea89e28332b617a962ffb013da01250f931138a6ad012b30705135f8e`
- Snapshot size: 8197 characters; 191 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0012:         {
0013:             CombatCatalog.SeedDefaults();
0014:             return new BallisticContext
0015:             {
...
0063:             // acc roll consumed first; any value < accuracy=1 => not a miss.
0064:             var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5));
0065:             Assert.Equal(BallisticResult.DirectHit, o.Result);
0066:             Assert.Equal(ctx.WeaponDamage, o.DamageDealt, 3);
...
0076:             ctx.WeaponAccuracy = 0f; // always >= accuracy => miss
0077:             var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.2));
0078:             Assert.Equal(BallisticResult.Missed, o.Result);
0079:             Assert.Equal(BallisticReason.AccuracyFail, o.Reason);
...
0087:                 Target = Hostile("en1", cover: 1f),
0088:                 Cover = CombatCatalog.GetMaterial("material_concrete")
0089:             };
0090:             var ctx = Base(st);
...
0107:             // acc(0.5), caught(0.9 => not blocked), graze(0.1 => penetrate), ricochet(0.9)
0108:             var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.9, 0.1, 0.9));
0109:             Assert.Equal(BallisticResult.DirectHit, o.Result);
0110:             Assert.True(o.DamageDealt < ctx.WeaponDamage, "penetration leaves less than full energy");
...
0125:             // acc(0.5); barrier reduces fully => blocked
0126:             var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5));
0127:             Assert.Equal(BallisticResult.Blocked, o.Result);
0128:             Assert.Equal(BallisticReason.BarrierBlocked, o.Reason);
...
0143:             // acc(0.5), ricochet(0.1 < 0.6 trigger), then secondary iter ricochet(0.9 >= 0.6 no) => direct hit
0144:             var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.1, 0.9));
0145:             Assert.Equal(BallisticResult.DirectHit, o.Result);
0146:             Assert.Equal("en2", o.ResolvedTargetId);
...
0163:             // force maximal ricochets; ensure chain stops at the cap.
0164:             var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.0, 0.0, 0.0, 0.0, 0.0));
0165:             int ricochetSteps = 0;
0166:             foreach (var s in o.Path) if (s.StartsWith("ricochet->")) ricochetSteps++;
...
0178:                 Target = Hostile("en1"),
0179:                 Armor = CombatCatalog.GetMaterial("armor_plate")
0180:             };
0181:             var ctx = Base(st);
...
0185:             ctx.WeaponDamage = 5f;
0186:             var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5));
0187:             Assert.Equal(BallisticResult.Stopped, o.Result);
0188:             Assert.Equal(BallisticReason.ArmorAbsorbed, o.Reason);
```

### Current evidence: `Ashfall.Core.Tests/CombatWeaponConditionTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `ebc05eec5113fde0abeb5dcc23ee0935b79cf614d537abe7142e193807828d78`
- Snapshot size: 5180 characters; 146 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0009:     {
0010:         public CombatWeaponConditionTests() => CombatCatalog.SeedDefaults();
0011:
0012:         private static WeaponInstanceState Rifle(string ammo = "ammo_556") => new WeaponInstanceState
...
0035:             var w = Rifle();
0036:             float pristine = WeaponConditionSystem.ComputeJamChance(w);
0037:             WeaponConditionSystem.Degrade(w, 0.8f); // condition 0.2 < threshold 0.25
0038:             float degraded = WeaponConditionSystem.ComputeJamChance(w);
...
0045:             var w = Pipe();
0046:             WeaponConditionSystem.Degrade(w, 1f);
0047:             float chance = WeaponConditionSystem.ComputeJamChance(w);
0048:             Assert.True(chance <= 1f && chance >= 0f);
...
0054:             var w = Pipe();
0055:             bool jammed = WeaponConditionSystem.TryJammed(w, new StubRng(7, 0.0)); // roll < chance => jam
0056:             Assert.True(jammed);
0057:             Assert.True(w.IsJammed);
...
0064:             var w = Pipe();
0065:             WeaponConditionSystem.TryJammed(w, new StubRng(7, 0.0));
0066:             bool done1 = WeaponConditionSystem.TickJamClear(w, 2);
0067:             Assert.False(done1); // not fully cleared yet (ticks > 2)
...
0075:             var w = Rifle();
0076:             WeaponConditionSystem.Degrade(w, 0.6f);
0077:             int cost = WeaponConditionSystem.GetScrapRepairCost(w);
0078:             Assert.True(cost >= 1, "repair has a scrap cost");
...
0092:             var w = Rifle();
0093:             WeaponConditionSystem.Degrade(w, 0.5f);
0094:             bool ok = new WeaponConditionSystem().TryFieldRepair(w, CombatHostPorts.NoOp(),
0095:                 (id, n) => false);
...
0104:             float before = w.ConditionPct;
0105:             WeaponConditionSystem.ExposeToAsh(w, 1f);
0106:             Assert.True(w.IsJammed);
0107:             Assert.True(w.ConditionPct < before);
...
0118:                 var w = Pipe("ammo_556");
0119:                 if (WeaponConditionSystem.TryWeaponBurst(w, new SeededRng(seed)))
0120:                 {
0121:                     sawBurst = true;
...
0131:             var w = Rifle("ammo_556"); // not jury-rigged
0132:             bool burst = WeaponConditionSystem.TryWeaponBurst(w, new StubRng(1, 0.0));
0133:             Assert.False(burst);
0134:         }
...
0139:             var w = Rifle();
0140:             int pristine = WeaponConditionSystem.GetScrapRepairCost(w);
0141:             WeaponConditionSystem.Degrade(w, 0.9f);
0142:             int ruined = WeaponConditionSystem.GetScrapRepairCost(w);
```

### Current evidence: `Ashfall.Core.Tests/CombatSaveRoundTripTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `84d1047ff4d8721849585d7cd533aae13077c8e73d20ffa6824eb3690cfdca0a`
- Snapshot size: 4887 characters; 123 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0010:     {
0011:         public CombatSaveRoundTripTests() => CombatCatalog.SeedDefaults();
0012:
0013:         private static TacticalCombatSystem Engine()
...
0038:
0039:             var restored = new TacticalCombatSystem();
0040:             restored.RestoreState(save);
0041:
...
0059:
0060:             var restored = new TacticalCombatSystem();
0061:             restored.RestoreState(loaded);
0062:
...
0081:             };
0082:             var migrated = TacticalCombatSystem.Migrate(legacy);
0083:             Assert.Equal(CombatState.CurrentSaveVersion, migrated.SaveVersion);
0084:             Assert.True(migrated.Phase >= (int)CombatPhase.Setup && migrated.Phase <= (int)CombatPhase.Retreated,
...
0104:
0105:         private static void DoReplay(TacticalCombatSystem sys)
0106:         {
0107:             var rng = new SeededRng(999);
...
0115:
0116:         private static List<string> EventDetails(TacticalCombatSystem sys)
0117:         {
0118:             var list = new List<string>();
```

### Current evidence: `Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `8b55510b5dd18342adf5083f62981cb640ad4d53678cb08cd1627557292339c7`
- Snapshot size: 13517 characters; 315 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0013:         {
0014:             CombatCatalog.SeedDefaults();
0015:         }
0016:
...
0060:
0061:             var sys1 = new TacticalCombatSystem();
0062:             sys1.BeginEncounter("enc_replay", "exp_1", "loc_alpha", "Alpha Ruins", 1, seed, CreateRoster(), CreateWeapons(), enemyCount: 2, enemyHealth: 30);
0063:
...
0119:             // Verify sys applies doctrine
0120:             var sys = new TacticalCombatSystem();
0121:             sys.DoctrineCapability = researched;
0122:             Assert.Same(researched, sys.DoctrineCapability);
...
0133:
0134:             var sys = new TacticalCombatSystem(null, ports);
0135:             sys.BeginEncounter("enc_trauma", "exp_1", "loc_alpha", "Alpha Ruins", 1, 1234, CreateRoster(1), CreateWeapons(1), enemyCount: 1, enemyHealth: 10);
0136:
...
0154:             var saved = sys.CaptureState();
0155:             var sys2 = new TacticalCombatSystem(null, ports);
0156:             sys2.RestoreState(saved);
0157:
...
0167:         {
0168:             var sys = new TacticalCombatSystem();
0169:             var weapons = CreateWeapons(1);
0170:             weapons[0].ConditionPct = 0.80f;
...
0174:             // Mid-combat degradation
0175:             WeaponConditionSystem.Degrade(sys.State.Weapons[0], 0.05f); // 0.80 -> 0.75
0176:             Assert.Equal(0.75f, sys.State.Weapons[0].ConditionPct, 2);
0177:
...
0182:             // Restore into fresh system
0183:             var sys2 = new TacticalCombatSystem();
0184:             sys2.RestoreState(saved);
0185:             Assert.Equal(0.80f, sys2.GetBoundWeaponStartCondition("w_inst_0"), 2);
...
0206:
0207:             var sys = new TacticalCombatSystem(null, ports);
0208:             sys.BeginEncounter("enc_ammo", "exp_1", "loc_alpha", "Alpha Ruins", 1, 9999, CreateRoster(1), CreateWeapons(1), enemyCount: 1, enemyHealth: 30);
0209:
...
0230:             // Run system 1 uninterrupted to turn 3
0231:             var sys1 = new TacticalCombatSystem();
0232:             sys1.BeginEncounter("enc_cont", "exp_1", "loc_alpha", "Alpha Ruins", 1, seed, CreateRoster(2), CreateWeapons(2), enemyCount: 3, enemyHealth: 40);
0233:
...
0247:             // Create sys2 from mid-encounter save and continue with identical seed/commands
0248:             var sys2 = new TacticalCombatSystem();
0249:             sys2.RestoreState(midSave);
0250:             var rngTurn2_B = new SeededRng(seed + 1);
...
```

### Current evidence: `Ashfall.Core.Tests/Combat/JourneyCombatLoopContractTests.cs`
- Role: focused verification candidate
- Worktree status: `clean`
- Evidence status: CURRENT WORKTREE SNAPSHOT; recheck before implementation if another package changes it.
- SHA-256: `88650eba30b51632be66bde6eb4d7b32005496ca1928566b144ed3b123e54613`
- Snapshot size: 23204 characters; 487 lines
- Evidence excerpt (bounded; not a generated API):
```text
...
0035:     {
0036:         public JourneyCombatLoopContractTests() => CombatCatalog.SeedDefaults();
0037:
0038:         private static List<CombatantState> Roster()
...
0088:         /// Simulated carried stock plus the host's ConsumeAmmo/ConsumeItem ports,
0089:         /// mirroring CombatHostSession.WireRealState: rounds are drawn from stock
0090:         /// and a driving loop tops the stock up from carried reserve.
0091:         /// </summary>
...
0099:             /// <summary>Mirrors the host's survivor health record, which
0100:             /// CombatHostSession.WireRealState maintains additively
0101:             /// (min(maxHealth, health + delta)) and returns as the NEW health.</summary>
0102:             public readonly Dictionary<string, float> SurvivorHealth = new Dictionary<string, float>();
...
0133:
0134:         private static TacticalCombatSystem BeginJourneyEncounter(int seed = 4242, CarriedStock? stock = null)
0135:         {
0136:             var system = new TacticalCombatSystem();
...
0197:             victim.IsDowned = true;
0198:             victim.BleedTurnsRemaining = TacticalCombatSystem.DefaultBleedTurns;
0199:             float healthBefore = victim.Health;
0200:
...
0211:             // no bleed-out death event may name this survivor.
0212:             for (int turn = 0; turn < TacticalCombatSystem.DefaultBleedTurns + 2; turn++)
0213:             {
0214:                 var outcome = system.EndTurn(new SeededRng(4242 + turn));
...
0230:             victim.IsDowned = true;
0231:             victim.BleedTurnsRemaining = TacticalCombatSystem.DefaultBleedTurns;
0232:
0233:             for (int turn = 0; turn < TacticalCombatSystem.DefaultBleedTurns + 1; turn++)
...
0447:
0448:         private static string? EngineActiveShooterId(TacticalCombatSystem system)
0449:         {
0450:             var players = system.State.Combatants.FindAll(c => c.IsPlayer && !c.HasFled);
...
0456:
0457:         private static string? EngineShooterWeaponInstanceId(TacticalCombatSystem system)
0458:         {
0459:             string? id = EngineActiveShooterId(system);
...
0464:
0465:         private static string EngineActiveShooterAmmoId(TacticalCombatSystem system)
0466:         {
0467:             var instanceId = EngineShooterWeaponInstanceId(system);
...
0471:
0472:         private static string? RawCombatantsOrderShooterId(TacticalCombatSystem system)
0473:         {
0474:             var shooter = system.State.Combatants.Find(
...
```

## Current JSON audit

#### `Assets/StreamingAssets/Data/combat_catalog.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, weapons, ammo, materials, combatants`
- `weapons`: list count=20; sample IDs=['weapon_pipe_rifle', 'weapon_scrap_shotgun', 'weapon_bolt_rifle', 'weapon_assault_rifle', 'weapon_lmg', 'weapon_pipe_shotgun', 'weapon_nail_driver', 'weapon_rebar_spear']
- `ammo`: list count=14; sample IDs=['ammo_357', 'ammo_12g', 'ammo_308', 'ammo_556', 'ammo_762', 'ammo_9x19', 'ammo_22lr', 'ammo_762x54r']
- `materials`: list count=7; sample IDs=['material_wood', 'material_concrete', 'material_metal', 'material_rebar', 'armor_cloth', 'armor_kevlar', 'armor_plate']
- `combatants`: list count=12; sample IDs=['combatant_burrower_mite', 'combatant_spore_hound', 'combatant_armored_boar', 'combatant_feral_mutt', 'combatant_pale_crawler', 'combatant_chrome_loper', 'combatant_conscript_levy', 'combatant_warlord_veteran']
- `schema_version`: `2`
- `collection_id`: `combat_catalog`
- SHA-256: `e837b53cbab1b7595a12f58c09cd7dbac753ef1736a4fbf69757a4d83f36a1c6`
#### `Assets/StreamingAssets/Data/items.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, items`
- `items`: list count=724; sample IDs=['item_decon_chelator_concentrate', 'item_lead_lined_effluent_filter', 'item_heavy_neoprene_scrub_brush', 'item_sealed_waste_bin', 'item_theodolite_brass_precision', 'item_surveyor_stadia_rod', 'item_datum_plate_bronze', 'item_concrete_mix']
- `schema_version`: `1`
- SHA-256: `15bfc2f1283b4610cfaf756c1c6ad3f9af11281e5886fe7ba7ecba37a65600e7`
#### `Assets/StreamingAssets/Data/ballistics_workbench_catalog.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, profiles`
- `profiles`: list count=2; sample IDs=['part_ballistics_rifle_standard', 'part_ballistics_sidearm_standard']
- `schema_version`: `1`
- SHA-256: `1ff41be85b6728816fc3e9e8a01020bbd4034ef6d5cd9be98bc8a6f01bfe27aa`
#### `Assets/StreamingAssets/Data/combat_arenas.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, collection_id, arenas`
- `arenas`: list count=1; sample IDs=['arena_lane_spine_default']
- `schema_version`: `1`
- `collection_id`: `combat_arenas`
- SHA-256: `de283146d88276f7cbaa247aa582a630d65dc87941502e96e7f44de478b475c6`
#### `Assets/StreamingAssets/Data/defenses.json` — CURRENT JSON SNAPSHOT
- Top-level keys: `schema_version, traps`
- `traps`: list count=4; sample IDs=['trap_perimeter_snare', 'trap_choke_deadfall', 'trap_concealed_capture_pit', 'trap_spike_border']
- `schema_version`: `1`
- SHA-256: `c80afe579aa4c9ee8796f7e49d0b0c7861292120e36f8fcc692ee873a371874b`
## Symbol and caller audit

#### `CombatCatalog` — HOST_REFERENCE_PRESENT — core/declaration=33, host=6, test=84
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:97` (declaration) — public static class CombatCatalog
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:259` (core) — var def = CombatCatalog.GetCombatant(combatantId);
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:272` (core) — var def = CombatCatalog.GetCombatant(combatantId)
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:288` (core) — var def = CombatCatalog.GetCombatant(combatantId);
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:432` (core) — CombatCatalog.Clear();
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:440` (core) — CombatCatalog.Register(new CombatWeaponDefinition
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:467` (core) — CombatCatalog.Register(new CombatAmmoDefinition
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:484` (core) — CombatCatalog.Register(new CombatMaterialDefinition
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:502` (core) — CombatCatalog.Register(new CombatantDefinition
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:524` (core) — ValidateRegistered(root, CombatCatalog.WeaponIds, CombatCatalog.AmmoIds,
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:525` (core) — CombatCatalog.MaterialIds, CombatCatalog.CombatantIds, dataDirectory, files, json);
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:44` (core) — CombatCatalog.SeedDefaults();
- `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs:150` (core) — var def = CombatCatalog.GetWeapon(weapon.WeaponId);
- `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs:184` (core) — var def = CombatCatalog.GetWeapon(weapon.WeaponId);
- `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs:236` (core) — var def = CombatCatalog.GetWeapon(weapon.WeaponId);
- `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs:256` (core) — var def = CombatCatalog.GetWeapon(weapon.WeaponId);
- `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs:315` (core) — var a = CombatCatalog.GetAmmo(ammoId);
- `Assets/Ashfall.Core/Combat/CombatTypes.cs:117` (core) — // ── AI trait fields populated from CombatCatalog (Plan 10 ─
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs:49` (core) — WeaponName = w != null ? (CombatCatalog.GetWeapon(w.WeaponId)?.displayName ?? w.WeaponId) : "—",
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs:66` (core) — var def = CombatCatalog.GetWeapon(w.WeaponId);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:65` (core) — var def = CombatCatalog.GetWeapon(weapon.WeaponId);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:89` (core) — res.Message = "No " + (CombatCatalog.GetAmmo(weapon.AmmoId)?.displayName ?? weapon.AmmoId) + " ammunition.";
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:136` (core) — var ammo = CombatCatalog.GetAmmo(weapon.AmmoId);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:137` (core) — var coverMaterial = CombatCatalog.GetMaterial("material_concrete"); // default rubble cover
- … 99 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `TacticalCombatSystem` — HOST_REFERENCE_PRESENT — core/declaration=30, host=38, test=97
- `Assets/Ashfall.Core/Survivors/NeedsPerformanceBridge.cs:118` (core) — /// Plan 137 — Contract for combat systems (e.g. TacticalCombatSystem) to query needs-derived combat modifiers.
- `Assets/Ashfall.Core/Combat/CombatAiMove.cs:4` (core) — // TacticalCombatSystem.Damage.cs, and any future host code can all ask
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Breaching.cs:12` (declaration) — public partial class TacticalCombatSystem
- `Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs:140` (core) — /// returns a bounded modifier for TacticalCombatSystem.
- `Assets/Ashfall.Core/Combat/BallisticsWorkbenchSystem.cs:268` (core) — /// bounded projection only; combat resolution remains in TacticalCombatSystem.
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:300` (core) — // token inside TacticalCombatSystem.cs and src/Host/CombatHostSession.cs
- `Assets/Ashfall.Core/Combat/CombatDoctrineCapability.cs:9` (core) — /// instead it projects this capability context into TacticalCombatSystem.
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:129` (core) — var sys = new TacticalCombatSystem(null!, ports);
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:145` (core) — Check(sys.State.PlayerStance == TacticalCombatSystem.StanceId(TacticalStance.HoldPosition), "stance set & serialized");
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:163` (core) — var restored = new TacticalCombatSystem();
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:193` (core) — var migrated = TacticalCombatSystem.Migrate(legacy);
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:204` (core) — var handoff = new TacticalCombatSystem(null!, CombatHostPorts.NoOp());
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:246` (core) — private static TacticalCombatSystem MakeEngine()
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:248` (core) — var sys = new TacticalCombatSystem();
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:263` (core) — private static List<string> RunScenario(TacticalCombatSystem sys)
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:280` (core) — private static string? TargetOf(TacticalCombatSystem sys)
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Damage.cs:7` (declaration) — public partial class TacticalCombatSystem
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Targeting.cs:7` (declaration) — public partial class TacticalCombatSystem
- `Assets/Ashfall.Core/Combat/CombatTypes.cs:310` (core) — public string SystemId = TacticalCombatSystem.SystemId;
- `Assets/Ashfall.Core/Combat/CombatTypes.cs:320` (core) — public string PlayerStance = TacticalCombatSystem.StanceId(TacticalStance.HoldPosition);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs:7` (declaration) — public partial class TacticalCombatSystem
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:8` (declaration) — public partial class TacticalCombatSystem
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs:32` (declaration) — public partial class TacticalCombatSystem
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs:59` (core) — public TacticalCombatSystem(CombatState? state = null, CombatHostPorts? ports = null)
- … 141 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `BallisticsSystem` — NO_HOST_REFERENCE_IN_SNAPSHOT — core/declaration=4, host=0, test=12
- `Assets/Ashfall.Core/Combat/BallisticsSystem.cs:80` (declaration) — public static class BallisticsSystem
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:175` (core) — var outcome = BallisticsSystem.Resolve(ctx, rng);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs:38` (core) — public const int MaxRicochetBounces = BallisticsSystem.MaxRicochetCount;
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:968` (core) — ["combat_catalog.json"] = new[] { "TacticalCombatSystem", "BallisticsSystem", "WeaponConditionSystem" },
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:64` (test) — var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5));
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:77` (test) — var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.2));
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:92` (test) — var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.1));
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:108` (test) — var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.9, 0.1, 0.9));
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:126` (test) — var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5));
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:144` (test) — var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.1, 0.9));
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:164` (test) — var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5, 0.0, 0.0, 0.0, 0.0, 0.0));
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:167` (test) — Assert.True(ricochetSteps <= BallisticsSystem.MaxRicochetCount,
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:168` (test) — $"ricochet chain exceeded bound ({ricochetSteps} > {BallisticsSystem.MaxRicochetCount})");
- `Ashfall.Core.Tests/CombatBallisticsTests.cs:186` (test) — var o = BallisticsSystem.Resolve(ctx, new StubRng(1, 0.5));
- `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs:418` (test) — var resA = BallisticsSystem.Resolve(ctxA, rngA);
- `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs:419` (test) — var resB = BallisticsSystem.Resolve(ctxB, rngB);
#### `WeaponConditionSystem` — HOST_REFERENCE_PRESENT — core/declaration=26, host=1, test=22
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:56` (core) — float c0 = WeaponConditionSystem.ComputeJamChance(weapon);
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:58` (core) — Check(WeaponConditionSystem.GetScrapRepairCost(weapon) >= 1, "pristine pipe rifle has a positive scrap repair cost");
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:61` (core) — WeaponConditionSystem.Degrade(weapon, 0.72f);
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:62` (core) — float cLow = WeaponConditionSystem.ComputeJamChance(weapon);
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:64` (core) — WeaponConditionSystem.ClearJam(weapon);
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:76` (core) — WeaponConditionSystem.ExposeToAsh(ashWeapon, 1f);
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:79` (core) — Check(WeaponConditionSystem.ComputeJamChance(ashWeapon) >= 0.5f, "ash-fouled jam chance is severe");
- `Assets/Ashfall.Core/Combat/CombatHeadlessDemo.cs:104` (core) — if (WeaponConditionSystem.TryWeaponBurst(probeWeapon, probe))
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Damage.cs:170` (core) — WeaponConditionSystem.ExposeToAsh(w, severity);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Targeting.cs:127` (core) — return w == null ? 0f : WeaponConditionSystem.ComputeJamChance(w);
- `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs:130` (declaration) — public class WeaponConditionSystem
- `Assets/Ashfall.Core/Combat/WeaponEquipmentBridge.cs:9` (core) — /// instance, 0–100) and combat's <see cref="WeaponConditionSystem"/>
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs:73` (core) — JamChancePct = (int)Math.Round(WeaponConditionSystem.ComputeJamChance(w) * 100f),
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs:75` (core) — ScrapRepairCost = WeaponConditionSystem.GetScrapRepairCost(w),
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:110` (core) — float degrade = WeaponConditionSystem.ComputeDegradePerBurst(weapon) * mods.Degrade;
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:111` (core) — WeaponConditionSystem.Degrade(weapon, degrade);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:114` (core) — bool jammed = WeaponConditionSystem.TryJammed(weapon, rng);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:124` (core) — bool burstFailure = WeaponConditionSystem.TryWeaponBurst(weapon, rng);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:269` (core) — int ticks = perks != null ? perks.GetJamClearTicks(c.SurvivorId) : WeaponConditionSystem.DefaultJamClearTicks;
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:270` (core) — bool cleared = WeaponConditionSystem.TickJamClear(weapon, ticks);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:342` (core) — int cost = WeaponConditionSystem.GetScrapRepairCost(weapon);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:393` (core) — int cost = WeaponConditionSystem.GetScrapRepairCost(weapon);
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:397` (core) — res.Message = "Cannot repair — need " + cost + " " + WeaponConditionSystem.ScrapMaterialId + ".";
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs:457` (core) — w.CachedJamChance = WeaponConditionSystem.ComputeJamChance(w);
- … 25 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `CombatHostSession` — HOST_REFERENCE_PRESENT — core/declaration=3, host=33, test=3
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:300` (core) — // token inside TacticalCombatSystem.cs and src/Host/CombatHostSession.cs
- `Assets/Ashfall.Core/Ports/PortContract.cs:67` (core) — /// <summary>Expected caller ID or host session (e.g., "CombatHostSession").</summary>
- `src/Main.Plans198_201.cs:63` (host) — // EvaluateActorExposure is invoked from CombatHostSession.ActionEndTurn.
- `src/Main.Expeditions.cs:46` (host) — private CombatHostSession _combat = null!;
- `src/Main.Expeditions.cs:355` (host) — _combat = CombatHostSession.Create(_dataDir, _campaignDay.Rng);
- `src/Main.Expeditions.cs:367` (host) — // CombatHostSession.ValidatePorts / WeaponConditionSystem's
- `src/Main.Expeditions.cs:440` (host) — private void SetupExpeditionCombatHandoff(CombatHostSession combat)
- `src/Main.Expeditions.cs:452` (host) — state.dangerLevel, CombatHostSession.DefaultAmbushEnemyCount);
- `src/Main.Muster.cs:186` (host) — int enemyCount = Math.Max(1, Math.Min(remaining, CombatHostSession.DefaultAmbushEnemyCount + 1));
- `src/Main.UiTests.RealCampaignJourney.cs:59` (host) — /// Survivor id CombatHostSession.StartCombat's default loadout gives the
- `src/Main.UiTests.RealCampaignJourney.cs:251` (host) — // CombatHostSession.StartCombat binds a tracked instance
- `src/Main.UiTests.RealCampaignJourney.cs:254` (host) — // CombatHostSession.StartCombat's default-loadout branch
- `src/Main.UiTests.RealCampaignJourney.cs:275` (host) — // CombatHostSession.StartCombat builds at most 4 players as
- `src/Main.UiTests.RealCampaignJourney.cs:308` (host) — // bypass): CombatHostSession's default loadout projects
- `src/Main.UiTests.RealCampaignJourney.cs:431` (host) — // CombatHostSession.StartCombat's default-loadout branch
- `src/Host/PortContractSelfTest.cs:158` (host) — var combatSession = new CombatHostSession();
- `src/Host/CombatSaveStore.cs:5` (host) — // Host Caller: Main.Expeditions / CombatHostSession
- `src/Host/CombatHostSession.cs:22` (declaration) — public sealed class CombatHostSession : HostSessionBase, IWiringReporter
- `src/Host/CombatHostSession.cs:72` (host) — public CombatHostSession(TacticalCombatSystem engine = null!, CombatHostPorts ports = null!, ICampaignRngManager? campaignRng = null)
- `src/Host/CombatHostSession.cs:258` (host) — public static CombatHostSession Create(string dataDir, ICampaignRngManager? campaignRng = null)
- `src/Host/CombatHostSession.cs:285` (host) — var session = new CombatHostSession(campaignRng: campaignRng);
- `src/Host/CombatHostSession.cs:821` (host) — SessionId = "CombatHostSession",
- `src/UI/CombatDetailPanel.cs:12` (host) — /// Reads the live CombatHostSession snapshot: battle information, current
- `src/UI/CombatDetailPanel.cs:20` (host) — private CombatHostSession _combat = null!;
- … 15 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
#### `combat_catalog` — HOST_REFERENCE_PRESENT — core/declaration=16, host=7, test=11
- `Assets/Ashfall.Core/Combat/CombatAiMove.cs:16` (core) — /// combat_catalog.json. The literal-string set is narrow and the
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:110` (core) — // Data authority is JSON (Assets/StreamingAssets/Data/combat_catalog.json).
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:326` (core) — // Combat data authority loader — reads Assets/StreamingAssets/Data/combat_catalog.json
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:395` (core) — public string collection_id = "combat_catalog";
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:404` (core) — /// (combat_catalog.json, snake_case). Maps onto the camelCase runtime
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:411` (core) — public const string FileName = "combat_catalog.json";
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs:615` (core) — throw new FormatException("combat_catalog.json failed validation:\n" + string.Join("\n", errors));
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:41` (core) — "combat_catalog.json", "verdict_data.json", "verdict_items.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:367` (core) — ["combat_catalog.json"] = new[] { "CombatCatalog" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:682` (core) — ["combat_catalog.json"] = "CombatCatalog",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:968` (core) — ["combat_catalog.json"] = new[] { "TacticalCombatSystem", "BallisticsSystem", "WeaponConditionSystem" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1365` (core) — "expeditions.json", "warlord_doctrines.json", "combat_catalog.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1438` (core) — ["combat_catalog.json"] = new[] { "CombatPanel" },
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1676` (core) — "warlord_doctrines.json", "combat_catalog.json",
- `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs:1713` (core) — "warlord_doctrines.json", "combat_catalog.json",
- `Assets/Ashfall.Core/IO/CatalogBootValidator.cs:221` (core) — RegisterCatalog("combat_catalog.json", "Combat Catalog", CatalogClassification.Optional);
- `src/Host/ContentUtilizationRuntimeCollector.cs:1001` (host) — string path = Path.Combine(dataDir, "combat_catalog.json");
- `src/Host/ContentUtilizationRuntimeCollector.cs:1003` (host) — instr.RecordCatalogOpened("combat_catalog.json", "CombatCatalog");
- `src/Host/ContentUtilizationRuntimeCollector.cs:1005` (host) — instr.RecordCatalogDeserialized("combat_catalog.json", 1);
- `src/Host/ContentUtilizationRuntimeCollector.cs:1006` (host) — instr.RecordDefinitionsRegistered("combat_catalog.json", "CombatCatalog", 1);
- `src/Host/ContentUtilizationRuntimeCollector.cs:1008` (host) — catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] combat_catalog.json: {ex.Message}"); }
- `src/Host/CombatHostSession.cs:261` (host) — // (combat_catalog.json). Without loading it here, every real
- `src/Host/CombatHostSession.cs:279` (host) — GD.PrintErr($"[Combat] combat_catalog.json load failed, falling back to defaults: {ex.Message}");
- `Ashfall.Core.Tests/Plan10RemediationTests.cs:89` (test) — File.WriteAllText(Path.Combine(dir, "combat_catalog.json"), json);
- … 10 additional occurrences omitted from this excerpt; the implementation phase must inspect the full call graph.
## Read-only authority alignment

Authority file: `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md`
Authority SHA-256: `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
Total lines in snapshot: 5510

The following excerpts are read-only orientation anchors. They do not override current source/data evidence or create implementation authority.
#### authority lines 41-44
00041: **DR-02 — The docs tree has substantially more subdirectories than the v1.0 map. VERIFIED.**
00042: Live `docs/` subdirectories observed in the audit include (selection; the listing was long and partially truncated): `adr/`, `agents/`, `architecture/`, `archive/`, `balance/`, `bodymind/`, `campaign/`, `cartography/`, `ci/`, `cli/`, `collectibles/`, `combat/`, `content/`, `contracts/`, `crafting/`, `crossing/`, `culture/`, `decisions/`, `design/`, `discovery/`, `duty_roster/`, `ecology/`, `economy/`, `endgame/`, `expansions/`, `expeditions/`, `faction_war/`, `factions/`, `foreman/`, `forensics/`, `foundry/`, `gaps/`, `governance/`, `greenhouse/`, `health/`, `holdfast/`, `hygiene/`, `i18n/`, `implementation/`, `incidents/`, `integration/`, `journal/`, `lore/`, `maritime/`, `medical/`, `memorials/`, `mods/`, `moral/`, `moral_choice/`, `muster/`, `narrative/`, `onboarding/`, `orbital/`, `perf/`, `phantoms/`, `plans/`, `power/`, `process/`, `production/`, and a `player_surface_manifest.json`. Two of these — `gaps/` and `incidents/` — are first-class *expansion feedstock*: directories whose entire purpose is to record what is missing or broken. The Factory Protocol (Part II, step 2) now treats `docs/gaps/` and `docs/incidents/` as mandatory inputs.
00043:
00044: **DR-03 — New top-level authority documents absent from the v1.0 docs map. VERIFIED.**
#### authority lines 171-174
00171: | C5 | Scavenging E[value] re-runs after any loot authoring (Plan 76.2 harness pattern); vehicle dominance follow-ups against the live dominance table | HIGH CONFIDENCE |
00172: | C7 | Tribute-cycle sustainability (7-day cadence) versus mid-game income; embargo economic pressure | HIGH CONFIDENCE |
00173: | C11 | Price-shock and rumor-band systemic outcomes; debt-interest runaway analysis; black-market pricing tiers | HIGH CONFIDENCE |
00174: | C12 | Winter resource compression (Days 90–180): calories, fuel, filters, morale — sustainability-day math per difficulty preset | HIGH CONFIDENCE |
#### authority lines 184-187
00184: | C13 | Epilogue evidence persistence: which Day-360+ facts survive into the Day-3650 window | HIGH CONFIDENCE |
00185: | Cross-cutting | Mid-event and mid-combat save round-trips for exactly-once effect classes beyond the rescue-signal runtime (which models the pattern) | PROPOSAL |
00186:
00187: ### 3.5 Lane E — UI, UX, and accessibility
#### authority lines 397-400
00397: ### What must not change
00398: Year-of-Ash canon (window 180–360), warlord tribute cadence (7 days), weather gate semantics, difficulty authority ownership (XP W1 is ACTIVE — no new difficulty scalars outside it).
00399:
00400: ### Recommended integration route
#### authority lines 671-674
00671:
00672: **A-03 · C2 · Casebook expansion for ARS latent-to-manifest phases.** Subject: medical casebook entries mirroring the authored ARS phase structure (latent-to-manifest, multi-day cadence, v1.0 Part 3.2). Evidence: `dweller_medical_casebook` exists; ARS pathology systems are canon. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
00673:
00674: **A-04 · C2 · Dose-treatment narrative pairing.** Subject: therapy-note and casebook twins for each row of `MEDICAL_DOSE_TREATMENT_MATRIX.md` (live, DR-03), so every mechanical treatment has a clinical-document voice. Evidence: matrix document verified live. Route: DATA-ONLY. Confidence: HIGH CONFIDENCE.
#### authority lines 789-792
00789:
00790: **C-05 · C7 · Tribute sustainability per doctrine.** Subject: 7-day tribute cadence versus mid-game income across doctrines and difficulty presets; identify mathematically unsustainable demand spirals versus intended harshness. Evidence: doctrine catalog live; cadence canon. Route: harness + report. Confidence: HIGH CONFIDENCE.
00791:
00792: **C-06 · C7 · Embargo pressure modeling.** Subject: `trade_embargoes.json` impact on settlement price bands; verify embargoes produce legible price signal, not noise. Evidence: embargo catalog verified live. Route: harness. Confidence: HIGH CONFIDENCE.
#### authority lines 807-810
00807:
00808: **C-14 · Cross · Time-to-kill and combat economy audit.** Subject: `combat_catalog.json` damage/armor cadence versus ammunition scarcity across difficulty presets. Evidence: combat catalog and hardcore tuning live. Route: harness. Confidence: PROPOSAL.
00809:
00810: ## 2.4 Lane D — Save, state, and compatibility seeds (D-01 … D-08)
#### authority lines 944-947
00944:
00945: **DM-7 — Factions and war (C7).** Owners: stance engine, doctrines, war system/chain runner, tributes, treaties, embargoes, espionage, psyops, counter-intelligence, musters, labor camps, bounty board. Live catalogs: `factions`, `faction_lore`, `faction_territory`, `faction_intelligence`, branch catalogs (independent/military/rebel), faction war family (communiques/dialogue/events/journal/radio/location_overrides), `warlord_doctrines`, `muster_*` family (five), `labor_camps`, `bounty_board`, `regional_treaties`, `trade_embargoes`, `foundry_accords`, `holdfast_factions`, `crossing_factions`. Hosts: Espionage, PsyOps, CounterIntelligence, Muster, FactionBranch, RegionalTreaty. Openings: A-16, A-17, A-18, B-10 (GATE), B-11 (GATE), C-05, C-06, G-05, plus the F-004 muster campaign. Constraint: all standing effects through `FactionStanceEngine`.
00946:
00947: **DM-8 — Radio and information (C8).** Owners: radio system, stations, programs, intercepts, distress signals (sealed runtime), rumors, sound ranging, direction finding, NVIS, heliograph. Live catalogs: `radio`, `radio_stations`, `radio_programs`, `radio_intercepts`, `radio_distress_signals` (+ expansion), `comms_targets`, `sound_ranging_catalog`, `direction_finding_catalog`, `nvis_communications_catalog`, `heliograph`. Hosts: Radio, RadioProgramProduction, SoundRanging, Heliograph. Sealed: distress content (`CF-P1-DISTRESS-CONTENT-SEAL`); availability consumer retired. Openings: A-19, A-20, B-12, B-13, B-25 (coordinated), E-04, F-05, G-06. Constraint: genuine-never-hostile invariant; no new signal scenarios without signature.
#### authority lines 960-963
00960:
00961: **DM-15 — Defense and security (C15).** Owners: perimeter defenses, defense grid, sky defense ordnance and armor, chemical defense, orbital harrow telemetry, interlocks, EMP effects. Live catalogs: `perimeter_defenses`, `defenses`, `sky_defense_ordnance`, `sky_layer_armor_catalog`, `chemical_weapons`, `orbital_harrow_events`, `railway_interlock_catalog`. Hosts: DefenseGrid, SkyDefense, ChemWarfareDefense, OrbitalHarrowTelemetrySystem. Openings: A-30, B-22. Constraint: sky-armor-to-weather bridge already partially built; verify before extending.
00962:
00963: **DM-16 — Progression and meta (C16).** Owners: skills, research, collectibles, trophies, achievements, difficulty presets, XP wave, codex, field guide, L10N, mods, settings, input, cohort tuning, apprenticeship, library study. Live catalogs: `skills`, `research_knowledge`, `collectibles`, `trophies`, `difficulty_presets`, `cohort_tuning`, `apprenticeship_catalog`, `library_manuals`, `cultural_archive_tomes`, `codex_entries`, `field_guide`. Hosts: Codex, Research, Collectibles, Difficulty, Apprenticeship, LibraryStudy, Mods, Onboarding, StartingLevel. Openings: B-06, B-23, C-11, D-08, E-03, G-08, J-02. Constraint: XP W1 owns difficulty authority while ACTIVE.
#### authority lines 1137-1140
01137: - 2026-09-27 — Volume 13: ten per-lane expansion playbooks (Lane A through J) with worked seed-to-verified-tranche examples and cross-lane sequencing rules — ~11,500 — cumulative ~274,000
01138: - 2026-09-27 — Volume 14: two worked wave charters (ASH-EXP-1 narrative wave, ASH-EXP-2 economy wave), the wave execution runbook, the re-audit cadence contract, and the anti-scope-creep review checklist — ~7,900 — cumulative ~282,000
01139: - 2026-09-27 — Volume 15: nine remaining Lane A seed expansions into full plans (FP-A09, FP-A14, FP-A15, FP-A20, FP-A23, FP-A24, FP-A25, FP-A29, FP-A30), closing Lane A's compressed-seed backlog, including the FP-A25 200-record quest prose audit tranche program (Tranche 0 census plus eight authoring tranches plus completion regression) — ~19,000 — cumulative ~301,000
01140: - 2026-09-27 — Volume 16: worked content tranche library — twelve PROPOSAL-model JSON tranche examples per established genre (glitch, load-shed, assay, interlock report, marginalia, rundown, intake, quest prose, sighting log, ordnance manifest, almanac), each with validation notes and the three mandatory focused tests — ~12,400 — cumulative ~313,000
#### authority lines 1796-1799
01796: Premise evidence: VERIFIED catalog live; `LedgerDebtSystem` with consequence dispatchers canon; debt seals in handoff records.
01797: Must not change: debt math, interest cadence, consequence vocabulary.
01798: Route: DATA-ONLY.
01799: Continuity: statements must reconcile with the ledger's own arithmetic; consequence threats only from the closed vocabulary.
#### authority lines 1910-1913
01910: Parameters: preset; starting supplies; expected fuel income schedule (expedition, trade, processing); storm-window drawdown spikes.
01911: Method: seeded 180-day simulation with fixed roster and authored expedition cadence; record reserve curve per week.
01912: Outputs: reserve curves per preset; days-to-zero; the week where income first fails to cover draw.
01913: Acceptance: byte-identical replay; curves reconcile with the Plan 122/125 closeout evidence.
#### authority lines 1949-1952
01949: Question: which debt trajectories are mathematically unrecoverable, and does every unrecoverable state still have a modeled recovery path (per the recovery grammar)?
01950: Parameters: `ledger_debt_templates.json` rows; income models (early/mid/late); interest cadence.
01951: Method: seeded loan-event simulation per template; record balance curves; classify unrecoverable states; cross-check each against consequence dispatcher outputs for recovery affordances.
01952: Outputs: runaway-state list; recovery-path audit table; any state lacking recovery becomes a Lane B finding, not a tuning edit.
#### authority lines 1978-1981
01978:
01979: ## 8.11 Harness H-C11 — Combat cadence (serves C-14)
01980:
01981: Question: what is time-to-kill and expected ammunition expenditure per combat class per preset, and does scarcity make any class unwinnable without Retreat as the only option?
#### authority lines 2457-2460
02457: Subject: instrument the shell components (dashboard shell, metric cards, data grids) during a 15-FPS headless session and record per-frame allocations; publish to `docs/perf/`; optimize only what the numbers demonstrate.
02458: Premise evidence: VERIFIED 15-FPS headless runtime sessions are the canon test cadence; VERIFIED the Performance Core family and CI performance gate exist; VERIFIED the atlas flags per-frame allocations as a candidate class.
02459: Why this: the lane's own rule — profile before rewrite — makes measurement the entire first plan; any repair it justifies gets its own plan with before/after numbers.
02460: Must not change: nothing (measurement only).
#### authority lines 2658-2661
02658: Verification: the report names each loop step with the minute it first surfaced, or records its absence.
02659: Open premises: scheduling a manual session at the 15-FPS test cadence.
02660:
02661: ## Volume 11 sequencing
#### authority lines 2697-2700
02697: - Option A — Full per-strike emission: every strike in the chain war emits a journal record, radio item, and sound-ranging observation. Consequence: rich autonomous-world texture; risk of journal and radio spam during active wars (the world-state notification contract bans alarm spam — the volume question is a design question, not a tuning afterthought).
02698: - Option B — Per-strike emission with a deterministic throttle: strikes emit at full cadence into the war-journal corpus (the dedicated record), but player-facing surfaces (radio strip, briefing) receive strikes only when they cross a relevance threshold (location adjacency to player-visited or treaty-relevant sites — the `PlayerVisitedTrigger` precedent supplies the adjacency key). Consequence: the record is complete while the surface stays legible.
02699: - Option C — Keep stage/chain-level projection only. Consequence: the sealed behavior stands; the atlas's flagged opening stays closed with rationale.
02700:
#### authority lines 2791-2794
02791: **Contract:** profile before rewrite; numbers in `docs/perf/`; no optimization without a demonstrated cost; correctness and readability outrank micro-gains.
02792: **Evidence inputs:** the Performance Core family, the CI performance gate, the 15-FPS headless cadence, the atlas's flagged candidate classes.
02793: **Worked example (FP-F02, compressed):** seed "storm-window tick concentration" → sweep: identify the co-firing systems and the instrumentation surface → run: window/non-window cost traces across 180–360 → publish: cost ratios and the noise floor → route: any demonstrated spike becomes a repair plan with the before numbers as its baseline.
02794: **Failure modes:** optimizing on aesthetics; trading determinism for speed (never); measuring once and claiming a trend.
… 10 additional authority matches omitted; the implementation owner must cite the exact relevant section at execution time.
## Objective and success definition

The objective is to add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop. The success condition is not merely a larger catalog or a more attractive panel. Success requires a current owner, a reachable consumer, a durable state decision, deterministic behavior, truthful UI, explicit failure semantics, and a focused verification handoff.

## Current reality, requested behavior, and minimum delta

**Existing behavior.** CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.

**Requested behavior.** Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.

**Minimum safe delta.** Extend `combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers` only after the current owner and caller audit confirms the missing link. If the old plan’s proposed system already exists, convert the task into a bounded maintenance/reachability package rather than creating a replacement.

## Non-goals and collision exclusions

- No parallel gameplay authority, save store, ledger, selector, event bus, simulation, or UI-owned rule.
- No Unity restoration, Unity dependency, or engine types in Core.
- No edits to authored data or production code in this planning-only pass.
- No broad test suite, full runtime soak, generated index rewrite, or unrelated documentation cleanup.
- No invented API, count, save section, or caller claim. Unknowns remain named unknowns.

## Current reality and required delta

**Current reality.** CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.

**Required delta.** Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.

**Primary seam.** combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers

**Non-goals.** This planning package does not modify production, authored data, saves, tests, UI, generated indexes, assets, or runtime code. It does not approve a new owner, authorize Unity work, or turn a historical plan claim into current evidence. The following terms are design hypotheses to verify during implementation, not facts asserted by this document: `CombatCatalog`, `TacticalCombatSystem`, `BallisticsSystem`, `WeaponConditionSystem`, `CombatHostSession`, `combat_catalog`.

## Integration framework

The integration framework is deliberately owner-first:

1. **Read and classify current state.** Start with the named Core owner, current JSON, host session, save store, and focused tests. Record whether the feature is live, partially wired, dormant, stale, or decision-gated.
2. **Choose one authority per concern.** Extend the current owner when it exists. If no owner exists, stop at the architecture decision boundary and name the new authority decision rather than creating a parallel store, selector, ledger, panel, or simulation.
3. **Author data against consumers.** A JSON row is not integrated merely because it parses. Every row must have a current loader, a current consumer, a visible or mechanically observable outcome, and a validation path.
4. **Route effects through existing events/seams.** Core emits facts; host sessions translate them; UI presents truthful state. Do not place gameplay calculations in a panel or Godot callback.
5. **Persist through the owning save path.** Capture and restore must be implemented before a feature is called persistent. Old versions, nulls, empty collections, checksums, and mid-event saves are explicit cases.
6. **Verify narrowly.** Use the smallest existing test file or a new focused test for an uncovered confirmed contract. Keep Core, data, host, UI, save, determinism, and cross-system checks distinguishable.
7. **Roll back by boundary.** A failed expansion should disable its adapter or authored tranche without corrupting the owning state or requiring a destructive reset.

## Code architecture

### Core layer

- Put reusable rules, validation, state transitions, deterministic selection, and read models in `Assets/Ashfall.Core/`.
- Keep Core engine-free. No Godot, Unity, `Texture2D`, `JsonUtility`, wall-clock, or unseeded randomness belongs in a Core contract.
- Extend existing models and public methods when the current API already expresses the concern. A new DTO, interface, event, or catalog loader is justified only when it removes a real ownership or boundary problem.
- Make invalid input observable through the owner’s normal result/diagnostic path. Do not silently coerce malformed content into a successful state.
- Keep deterministic ordering explicit: use ordinal IDs, stable catalog order, bounded collections, and the existing seeded RNG fork for any stochastic choice.

### Data layer

- Author under `Assets/StreamingAssets/Data/` using the existing schema and snake_case IDs.
- Prefer additive fields and existing collections over parallel catalogs.
- Validate IDs, references, ranges, and consumer reachability through the current catalog integrity pipeline.
- Record schema version and old-data behavior in the plan and implementing handoff.
- Data prose may describe a consequence only when the consequence is expressible through a current owner and event.

### Host layer

- Load the catalog in the current host/session owner, not in a panel constructor.
- Subscribe once to owner events, translate facts into existing journal/radio/UI signals, and dispose subscriptions with the session.
- Bind day/hour/event triggers through the existing campaign owner. Do not create a second clock or update loop.
- Rehydrate from the existing save owner and mark dirty only for actual canonical mutations.
- Keep host code free of duplicate gameplay math; it may format, route, and adapt.

### Presentation layer

- Panels expose current commands and truthful current state.
- They display unavailable/blocked reasons, provenance, and the next legitimate action rather than simulating a result.
- Preserve keyboard/controller close/back behavior, focus order, readable contrast, and reduced-motion/accessibility settings.
- Refresh from owner events and lifecycle state; never use a panel cache as authority.
- Snapshot or accessibility fixtures are verification artifacts, not gameplay state.

## Ownership and state matrix

| Concern | Authoritative owner | Adapter responsibility | Persistence rule | Verification gate |
|---|---|---|---|---|
| Domain rules | TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation | Bind inputs and translate facts | Existing owner DTO/save | Core focused tests |
| Authored content | Current JSON catalog and loader | Load/validate once | Catalog version/defaults | Data integrity |
| Runtime lifecycle | Existing host/session owner | Setup, event subscription, disposal | Existing save/session | Host wiring |
| Presentation | Existing Godot surface | Format and command dispatch | No gameplay state | UI/focus/headless |
| Diagnostics | Existing logging/telemetry owner | Correlate ID and phase | Bounded/non-authoritative | Failure test |
| Historical authority | Read-only master document | Cite relevant section only | Never persisted | Plan QA |

## Data, event, and command flow

`authoritative JSON / player command / current owner event` → validation at the owning seam → canonical Core state transition → typed fact/event → host session projection → journal/radio/UI refresh → existing save owner if the transition mutated durable state.

The flow is intentionally one-way for authority. UI may send a command, but it cannot directly mutate the domain. Events may be consumed by several read-only projections, but only the owner writes the state. If a proposed feature needs a second writer, that is an architecture failure, not an invitation to add another event bus.

## State, API, and compatibility contract

The implementing agent must confirm the actual public API and write the final signatures in the implementation handoff. At minimum, expose:

- a read-only query/projection for the current state;
- an explicit command or owner method for each player-visible mutation;
- a typed fact/event for meaningful state changes;
- capture/restore methods on the existing state owner;
- a diagnostic result for invalid or unavailable data;
- a stable key for idempotent commands and replay;
- a bounded, ordinal-stable collection for any retained history.

Old saves must default missing additive fields to the documented neutral value. New required fields need a versioned migration. Null and empty semantics must differ deliberately: null means unavailable/not supplied; empty means validly no records, unless the current owner’s contract says otherwise. Do not infer a new save section from the plan title.

## Determinism and replay

For every proposed random or time-dependent element, name the seed source, stream/fork, draw order, retry behavior, and tie-break rule. Prefer no randomness for validation, lookup, and deterministic UI state. If an existing owner uses `ISeededRng`, reuse its campaign stream/fork rather than constructing a private generator. Wall-clock time, `System.Random`, `Guid.NewGuid`, hash iteration order, filesystem enumeration order, and frame timing are prohibited in deterministic Core behavior.

The replay acceptance test must use two equivalent runs with identical seed, catalog snapshot, state, day/hour, and command sequence. Compare canonical state, event order, resource/ledger deltas, and persistence payload. Presentation-only differences are acceptable only when they are explicitly non-authoritative and do not alter commands or outcomes.

## Save, restore, and migration

Before implementation claims persistence:

1. Identify the current save-section owner and DTO.
2. Add or reuse capture/restore through that owner.
3. Deep-copy mutable collections so restoring does not alias runtime state.
4. Define old-version defaults for every new field.
5. Define behavior for missing catalogs, unknown IDs, partial records, and corrupted checksums.
6. Test capture → serialize → restore → continued mutation, plus a mid-transition reload.
7. Confirm a save/load pair does not duplicate one-shot events or reapply a quest/choice/ledger mutation.

No plan-created “state cache” is allowed. If a new authority is genuinely required, the package must pause for a decision and name its owner, section, migration, and test contract.

## Failure and edge behavior

The implementation must cover null/empty state, empty catalogs, duplicate IDs, missing references, stale old saves, invalid numeric values, extreme but bounded values, unavailable owners, dead or absent participants, repeated commands, simultaneous events, host reload, missing UI, missing audio/journal, new-game reset, teardown, and deterministic replay. The expected result should preserve the last valid state, report a useful diagnostic, and avoid presenting a fabricated success. The detailed failure matrix below expands these cases for 54.

## Test strategy and focused commands

The plan-only pass does not execute tests. The implementing package should reuse existing focused files first, run a new file alone, and stay below the repository’s focused-test policy unless a foreman-approved hypothesis requires more. The current candidate commands are listed in the verification matrix and must be revalidated against the worktree at implementation start. A passing compile is not proof of host wiring, persistence, determinism, or player reachability.

## Phased implementation and rollback

The detailed phase table below is the implementation contract. Each phase has a completion gate and a “must not touch yet” boundary. Rollback is additive and local: disable the adapter or remove the authored tranche, retain the owner’s last valid state, and never reset the shared worktree or shared save registry to hide a failure.

## Dependency-ordered implementation phases
| Phase | Outcome | Work | Boundary | Completion gate |
|---|---|---|---|---|
| 0 | Premise recheck and baseline | Confirm exact owner/API/catalog counts, current claims, dirty paths, and active decision gates. | No edits to production. | A written evidence table and focused baseline commands. |
| 1 | Owner and collision map | Trace current callers, save owner, event seam, and duplicate/legacy candidates. | No new catalog or state. | Single-owner map with zero unresolved authority collisions. |
| 2 | Core contract or bounded extension | Add only the smallest pure contract needed by the confirmed gap, or document that no Core change is needed. | No Godot/UI/data authoring. | Core tests for boundaries, transitions, invalid data, and determinism. |
| 3 | Persistence and migration contract | Implement capture/restore/old-save defaults through the existing owner. | No unrelated save sections. | Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass. |
| 4 | Authored data tranche | Author schema-valid rows only after consumer fields are known; validate references and reachability. | No prose-only orphan rows. | Data integrity and consumer coverage pass for the tranche. |
| 5 | Host/event wiring | Load, subscribe, translate, and dispose in the current host/session owner. | No panel gameplay math. | Host wiring test proves event → projection and setup/teardown. |
| 6 | Presentation and accessibility | Expose truthful state, commands, focus, controller/keyboard behavior, and feedback. | No new authority in UI. | Panel route/focus/headless checks pass; snapshots only through the owning harness. |
| 7 | End-to-end and replay | Run a bounded scenario, save/reload, paired seeded replay, and cross-system consequence check. | No full-suite default. | Named commands/results and limitations recorded. |
| 8 | Balance/content polish | Tune only authored values with current harnesses; remove dead rows and polish truthful text. | No hidden tuning or parallel scalar. | Content review confirms no dominated/unreachable row and no unsupported claim. |
| 9 | Rollback and closeout | Document feature disablement, migration reversal, owner handoff, and residual debt. | No unowned cleanup. | Foreman review accepts or records a blocker. |

### Phase-specific implementation questions
#### Phase 0: Premise recheck and baseline
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No edits to production.
- What proves completion? A written evidence table and focused baseline commands.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 1: Owner and collision map
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No new catalog or state.
- What proves completion? Single-owner map with zero unresolved authority collisions.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 2: Core contract or bounded extension
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No Godot/UI/data authoring.
- What proves completion? Core tests for boundaries, transitions, invalid data, and determinism.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 3: Persistence and migration contract
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No unrelated save sections.
- What proves completion? Round-trip, deep-copy, corrupt/invalid, and mid-event tests are required to pass.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 4: Authored data tranche
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No prose-only orphan rows.
- What proves completion? Data integrity and consumer coverage pass for the tranche.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 5: Host/event wiring
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No panel gameplay math.
- What proves completion? Host wiring test proves event → projection and setup/teardown.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 6: Presentation and accessibility
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No new authority in UI.
- What proves completion? Panel route/focus/headless checks pass; snapshots only through the owning harness.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 7: End-to-end and replay
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No full-suite default.
- What proves completion? Named commands/results and limitations recorded.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 8: Balance/content polish
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No hidden tuning or parallel scalar.
- What proves completion? Content review confirms no dominated/unreachable row and no unsupported claim.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

#### Phase 9: Rollback and closeout
- What current evidence must be reread? CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.
- What is the smallest safe change? Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.
- Which owner is touched? TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation
- What must remain untouched? No unowned cleanup.
- What proves completion? Foreman review accepts or records a blocker.
- What is the rollback if the gate fails? Keep the prior owner contract, remove only the bounded adapter/data tranche, and preserve the save schema.

## Ownership matrix and file impact map
The following is an impact map for a future implementation package, not a request to edit these paths in this planning-only task.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Combat/CombatCatalog.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Damage.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Targeting.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Combat/BallisticsSystem.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `Assets/Ashfall.Core/Combat/CombatTypes.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/CombatHostSession.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/Host/CombatSaveStore.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/CombatPanel.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after claim`: `src/UI/CombatDetailPanel.cs` — TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; change only the confirmed owner seam.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/combat_catalog.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/items.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/ballistics_workbench_catalog.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/combat_arenas.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after data owner claim`: `Assets/StreamingAssets/Data/defenses.json` — authored content; schema/additive compatibility required.
- `READ/MODIFY only after presentation claim`: `src/UI/CombatPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/CombatDetailPanel.cs` — thin projection and command surface only.
- `READ/MODIFY only after presentation claim`: `src/UI/CombatHudOverlay.cs` — thin projection and command surface only.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Plan54CombatCatalogTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/CombatCatalogValidationTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/CombatBallisticsTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/CombatWeaponConditionTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/CombatSaveRoundTripTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs` — focused verification; no test created to mask a stale premise.
- `READ/MODIFY only after test owner claim`: `Ashfall.Core.Tests/Combat/JourneyCombatLoopContractTests.cs` — focused verification; no test created to mask a stale premise.

## Out of scope
- No unrelated refactor.
- No Unity restoration or dependency.
- No generated index or unrelated documentation regeneration.
- No broad test suite or runtime soak by default.
- No new save owner, registry, selector, or simulation unless a signed architecture decision names it.

## Definition of done
- The current owner and public API are cited from the implementation snapshot.
- combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers is wired end to end or the plan explicitly closes as already integrated.
- Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Data references, schema version, old-save behavior, focused tests, and rollback are recorded.
- No stale “sealed”, “approved”, or pass-count language is used without current evidence.

## Numbered implementation contract

# 1. Objective

Deliver only the bounded delta described as: Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop. The objective is measured by current-owner reachability, truthful state, deterministic replay, and a safe implementation handoff rather than by document length.

# 2. Current Reality

CombatCatalog, TacticalCombatSystem and its action/damage/targeting/persistence partials, BallisticsSystem, WeaponConditionSystem, CombatHostSession, CombatSaveStore, combat panels, and focused combat tests are live. The old plan’s claimed catalog size and standalone combat manager are historical assumptions, not current API facts.

# 3. Required Delta

Add or correct authored weapons, ammunition, combatants, armor, condition, and encounter bindings only when the live selectors consume them, with deterministic action ordering, bounded state, and no duplicate combat loop.

# 4. Evidence

Use the current source/data dossier, JSON audit, symbol/caller audit, and read-only authority excerpts in this document. The canonical authority is docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md with SHA-256 911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c; current source/data remain the implementation truth when the authority is descriptive or historical.

# 5. Existing Extension Seams

Primary seam: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers. Before creating anything, verify the current public method, event, host session, save store, and consumer named in the dossier. A new abstraction is justified only when this seam cannot express the confirmed delta.

# 6. Proposed Architecture

Use the owner-first Core → data → host → presentation architecture described above. The proposed architecture is a bounded extension of TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation; it does not create a parallel gameplay system.

# 7. Ownership Matrix

Canonical ownership: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation. JSON owns authored content, Core owns pure rules/state transitions, the host owns lifecycle/adapters, and Godot panels own presentation only. Every proposed write must be assigned to exactly one row of that matrix.

# 8. Data Flow

INPUT (catalog, command, current state) → VALIDATION (owner/schema/reference checks) → CORE STATE → DOMAIN FACT → HOST PROJECTION → UI FEEDBACK → SAVE OWNER. The reverse UI path is a command request, never a direct state mutation.

# 9. State Model

State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. The implementing agent must document fields, defaults, lifecycle, mutation, reset, persistence, and migration against the actual current DTO before editing.

# 10. API/Contracts

Expose only the current owner’s read query, command/mutation, typed fact/event, capture/restore, diagnostic result, idempotency key, and stable ordering needed for combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers. Confirm signatures from source; never copy historical API names from the old plan.

# 11. Data Changes

Data changes must extend current catalogs under Assets/StreamingAssets/Data/. For each row, validate schema_version, snake_case ID, references, ranges, default behavior, loader, consumer, and observable outcome. The record review ledger applies this rule to Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.

# 12. Save/Load

Persistence must use the current owner identified by the dossier. Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. Require capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input behavior, checksum handling, and mid-event reload before claiming persistence.

# 13. Determinism

Determinism contract: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution. Reuse existing seeded RNG forks, ordinal ordering, bounded state, and invariant culture formatting. A compile-green result is not replay evidence.

# 14. System/Event Wiring

Wire the confirmed event or command through combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers. Define event order, daily/hourly triggers, idempotency, subscriber lifetime, missing-owner behavior, and the exact host projection. Do not add a second event authority.

# 15. Godot Integration

Godot integration is limited to the current host/session and named presentation surfaces. UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs'] Preserve focus, controller/keyboard close/back, contrast, reduced motion, refresh, and disposal behavior.

# 16. Narrative/Content Integration

Content must describe only effects expressible by the current owner. Record-level action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable. Validate narrative references, continuity, voice, and player-visible consequence without making prose a hidden gameplay authority.

# 17. Failure Modes

Failure behavior: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics. The failure matrix covers null/empty state, missing/duplicate references, old/corrupt saves, unavailable owners, dead or hostile participants, repeated commands, simultaneous events, reload, missing UI/audio/journal, and deterministic replay.

# 18. Test Strategy

Use the smallest existing focused test first; run a new test alone; keep save/load, determinism, lifecycle, mutation, fuzzing, state-transition, and cross-system workflows independently reported. The plan-only package does not claim these commands were run.

# 19. Dependency-Ordered Phases

Follow phases 0–9: premise recheck, owner/collision map, Core contract, persistence/migration, data tranche, host/event wiring, presentation/accessibility, end-to-end/replay, balance/polish, and rollback/closeout. Each phase has a completion gate and a must-not-touch boundary above.

# 20. File Impact Map

The future implementation package may modify only the confirmed owner/data/host/UI/test paths listed in the dossier and only after claiming them. This Round 7 planning package intentionally modifies none of those production paths.

# 21. Risks

Primary risk: Realtime combat is actively claimed by another package; this plan describes the shared authority and does not modify or duplicate realtime files. Additional risks are dirty-worktree drift, stale catalog counts, missing host callers, shared save seams, decision-gated authority, accessibility regressions, and false completion claims. Each risk has a stop/escalate rule in the handoff.

# 22. Out of Scope

No unrelated refactor, Unity restoration, new parallel authority, broad test suite, generated-index rewrite, asset production, or opportunistic gameplay tuning is included.

# 23. Rollback Strategy

Rollback is local: disable the adapter, remove only the bounded authored tranche, preserve the owner’s last valid state, and keep the save schema readable. Never reset the shared worktree or hide a failure with a destructive migration.

# 24. Definition of Done

The implementing package is done only when combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers is wired or proven already integrated, current data validates and reaches a consumer, save/determinism/failure/UI contracts pass focused verification, accessibility is truthful, and the handoff records limitations. This document itself remains planning-only.

# 25. Implementation Handoff

MUST PRESERVE the current owner, Godot/Core boundary, JSON authority, save/determinism contracts, and accessibility. MUST ADD only the smallest confirmed extension and focused evidence. MUST NOT invent APIs or claim unrun tests. FIRST SAFE STEP: reread the first current owner/catalog/host/test path and write a live-versus-stale premise table before any implementation edit.

## Detailed record-by-record integration ledger

### Record review 001: `weapon_pipe_rifle`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 002: `weapon_scrap_shotgun`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 003: `weapon_bolt_rifle`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 004: `weapon_assault_rifle`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 005: `weapon_lmg`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 006: `weapon_pipe_shotgun`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 007: `weapon_nail_driver`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 008: `weapon_rebar_spear`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 009: `weapon_molotov_thrower`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 010: `weapon_service_rifle`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 011: `weapon_marksman_rifle`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 012: `weapon_smg`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 013: `weapon_sidearm`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 014: `weapon_rust_mosin`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 015: `weapon_farm_carbine`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 016: `weapon_revolver`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 017: `weapon_coach_shotgun`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 018: `weapon_trail_carbine`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 019: `weapon_battle_rifle`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 020: `weapon_quiet_carbine`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 021: `ammo_357`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 022: `ammo_12g`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 023: `ammo_308`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 024: `ammo_556`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 025: `ammo_762`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 026: `ammo_9x19`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 027: `ammo_22lr`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 028: `ammo_762x54r`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 029: `ammo_357_jhp`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 030: `ammo_12g_buck`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 031: `ammo_308_incendiary`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 032: `ammo_556_subsonic`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 033: `ammo_improvised_rod`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 034: `ammo_improvised_burn`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 035: `material_wood`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 036: `material_concrete`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 037: `material_metal`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 038: `material_rebar`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 039: `armor_cloth`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 040: `armor_kevlar`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 041: `armor_plate`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 042: `combatant_burrower_mite`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 043: `combatant_spore_hound`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 044: `combatant_armored_boar`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 045: `combatant_feral_mutt`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 046: `combatant_pale_crawler`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 047: `combatant_chrome_loper`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 048: `combatant_conscript_levy`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 049: `combatant_warlord_veteran`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 050: `combatant_flotilla_marine`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 051: `combatant_desperate_scavenger`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 052: `combatant_salvage_veteran`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 053: `combatant_hydro_pump_warden`
- Source: `Assets/StreamingAssets/Data/combat_catalog.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 054: `item_decon_chelator_concentrate`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 055: `item_lead_lined_effluent_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 056: `item_heavy_neoprene_scrub_brush`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 057: `item_sealed_waste_bin`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 058: `item_theodolite_brass_precision`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 059: `item_surveyor_stadia_rod`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 060: `item_datum_plate_bronze`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 061: `item_concrete_mix`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 062: `item_forged_rotor_shaft`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 063: `item_magnetic_bearing_coil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 064: `item_high_vacuum_pump`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 065: `item_containment_ring_steel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 066: `item_reinforced_concrete_vault`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 067: `item_seismic_damper_pad`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 068: `item_vacuum_pump_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 069: `item_bearing_grease`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 070: `item_rotor_balancing_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 071: `item_portable_pid_detector`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 072: `item_detector_sensor_module`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 073: `item_hermetic_sample_ampoule`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 074: `item_hot_dust_drum`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 075: `item_sludge_cake`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 076: `item_tailings_drum`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 077: `dosimeter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 078: `geiger_counter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 079: `iodine_pills`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 080: `anti_rad`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 081: `gas_mask`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 082: `hazmat_suit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 083: `water_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 084: `air_filter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 085: `clean_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 086: `irradiated_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 087: `canned_food`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 088: `fuel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 089: `cloth`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 090: `scrap_metal`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 091: `bandage`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 092: `raw_meat`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 093: `cooked_meat`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 094: `dirty_water`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 095: `morphine`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 096: `chelation_agent`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 097: `potassium_iodide`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 098: `medical_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 099: `battery`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 100: `calibration_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 101: `tweezers`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 102: `splint`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 103: `antibiotics`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 104: `jewelry`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 105: `diamond`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 106: `currency`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 107: `mechanical_parts`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 108: `electronic_scrap`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 109: `item_radiosonde`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 110: `solar_cell`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 111: `chemicals`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 112: `handheld_radio`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 113: `engine`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 114: `roots`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 115: `berries`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 116: `vacuum_tube`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 117: `spring_mechanism`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 118: `phonograph_needle`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 119: `projector_bulb`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 120: `lubricant_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 121: `film_reel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 122: `antenna_coil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 123: `soldering_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 124: `music_box_comb`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 125: `spring_key`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 126: `typewriter_ribbon`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 127: `machine_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 128: `camera_lens_cleaner`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 129: `photographic_film`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 130: `item_acoustic_decoy`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 131: `item_ammonium_nitrate_sack`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 132: `item_amnestic_syrup`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 133: `item_anchor_notes`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 134: `item_ash_ghillie`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 135: `item_bio_plastic`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 136: `item_black_water_vial`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 137: `item_co2_scrubber_cartridge`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 138: `item_epoxy_injector`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 139: `item_faraday_mesh`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 140: `item_frostbite_salve`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 141: `item_fungicide_fogger`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 142: `item_galvanized_rebar`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 143: `item_glycol_antifreeze_canister`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 144: `item_hermetic_hatch_silicone_gasket`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 145: `item_high_tensile_steel_culvert_brace`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 146: `item_insulated_snowmobile_battery`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 147: `item_lead_shielded_sample_cask`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 148: `item_lead_visor`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 149: `item_lithium_salts`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 150: `item_mine_prod`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 151: `item_mycelium_bricks`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 152: `item_prussian_blue_chelating_pellets`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 153: `item_radon_detector_electret`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 154: `item_rebreather_scrubber`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 155: `item_ro_membrane`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 156: `item_scopolamine_root`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 157: `item_sealed_lead_pig`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 158: `item_snow_goggles_improvised`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 159: `item_sound_baffling`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 160: `item_suitcase_locked`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 161: `item_surgical_bone_chisel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 162: `item_teddy_bear`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 163: `item_thermal_paste`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 164: `item_welders_glass`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 165: `aa_batteries`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 166: `alcohol_wipes_box_10_of_10`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 167: `ammo_762x54r_jhp_ap`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 168: `antiseptic_1l_of_1l`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 169: `battery_pack`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 170: `box_of_nails_10`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 171: `canned_soup`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 172: `childrens_books`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 173: `cigarette_lighter`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 174: `clean_water_jug`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 175: `cooking_oil`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 176: `copper_wire_10m_of_10m`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 177: `diesel_fuel`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 178: `dried_rations`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 179: `faraday_pack`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

### Record review 180: `field_surgical_kit`
- Source: `Assets/StreamingAssets/Data/items.json`
- Current classification: authored record; consumer reachability must be proven, not assumed.
- Required integration action: Trace every catalog record to a live combat selector, item/ammo reference, condition or armor consumer, encounter caller, and persistence/test gate before treating it as playable.
- Primary owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State/save rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI truth rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Failure/edge rule: missing references, duplicate IDs, empty optional values, old data, unavailable owner, and repeated activation must have an explicit expected result.
- Focused verification: data integrity plus the smallest existing test file that covers this owner; add a new test only if the confirmed contract is uncovered.
- Rollback: remove or revert only the authored record/adapter change; never reset unrelated runtime state or shared ledgers.

## Precision scenario matrix
Each row is a future implementation checkpoint, not a claim that the current repository already passes it.
### Scenario 01: fresh campaign before the owner is initialized
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 02: old save restored at day zero
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 03: old save restored after a partial event
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 04: catalog unavailable at startup
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 05: catalog contains an empty collection
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 06: duplicate canonical ID
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 07: reference points to a missing item
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 08: reference points to a missing location
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 09: reference points to a missing faction
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 10: unavailable optional owner
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 11: owner disabled by difficulty or policy
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 12: unpowered infrastructure
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 13: zero resources
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 14: negative or malformed numeric input
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 15: large but bounded collection
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 16: dead survivor or unavailable participant
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 17: hostile faction state
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 18: repeated player command
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 19: simultaneous day events
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 20: mid-transition save
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 21: reload after event dispatch
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 22: missing UI surface
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 23: stale presentation cache
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 24: missing audio cue
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 25: missing journal owner
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 26: missing save owner
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 27: corrupt save payload
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 28: checksum mismatch
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 29: RNG fork unavailable
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 30: unordered dictionary iteration
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 31: clock boundary at midnight
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 32: seasonal boundary
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 33: weather gate closure
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 34: route closure
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 35: trade or treaty conflict
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 36: choice already resolved
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 37: ending owner unavailable
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 38: optional content absent
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 39: mod or compatibility row absent
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 40: concurrent package changes source
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 41: headless session without UI
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 42: snapshot fixture unavailable
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 43: controller/keyboard focus path
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 44: screen reader/high contrast path
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 45: asset/resource fallback
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 46: final archive projection
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 47: new-game reset
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 48: legacy content migration
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 49: consumer not wired
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 50: host setup order reversed
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 51: teardown/disposal
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 52: replay after reload
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 53: telemetry/diagnostic emission
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 54: authority conflict discovered
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

### Scenario 55: future implementation acceptance boundary
- Domain lens: ballistics, combat authority, and equipment condition.
- Seam under test: combat_catalog.json/items.json -> CombatCatalog and BallisticsSystem -> TacticalCombatSystem selectors/actions/damage -> CombatHostSession/save store -> combat UI and expedition/patrol callers.
- Expected authority: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Required behavior: apply the canonical owner’s existing contract; do not invent a fallback state that looks like gameplay success.
- Save/determinism check: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state. All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI/accessibility check: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Verification: use the smallest focused Core/data/host test that covers the confirmed behavior; no broad test run is implied by this plan.
- Rollback: preserve the last valid state and reverse only the record or adapter responsible for the failure.

## Failure and rejection matrix
The failure contract is intentionally strict: an unavailable feature is preferable to a convincing but unauthoritative simulation.
### Failure 01: null state
- Detection: accept an explicit empty/default state only where the owner contract permits it.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 02: empty catalog
- Detection: report a data-integrity gap and keep the previous safe projection.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 03: duplicate ID
- Detection: reject the row with a stable diagnostic rather than last-write-wins.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 04: missing reference
- Detection: do not create a phantom entity; expose the unresolved dependency.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 05: stale save
- Detection: migrate or default only through the owner’s versioned restore path.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 06: corrupt checksum
- Detection: refuse the corrupted section and preserve unrelated valid sections.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 07: host reload
- Detection: rehydrate through the same owner and event registration path.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 08: new game
- Detection: clear transient host state and initialize owner defaults exactly once.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 09: deterministic replay
- Detection: same seed, day, catalog, and state must produce the same fact/order.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 10: UI unavailable
- Detection: retain canonical state and defer presentation without re-running mutation.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 11: disposal
- Detection: unregister listeners and release host resources deterministically.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 12: concurrent claim
- Detection: stop and hand off rather than editing a shared seam.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 13: authority conflict
- Detection: name the conflict and defer the architectural decision.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 14: missing test
- Detection: do not claim integration; add the focused contract test in the implementing package.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 15: unsupported API
- Detection: use the current public API or mark the premise stale.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 16: legacy Unity reference
- Detection: do not restore it; port only through the Godot/Core boundary.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 17: engine reference in Core
- Detection: reject the change and move only presentation adaptation to src/.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 18: unowned save state
- Detection: reject the change until the owner and migration path are explicit.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 19: UI-owned gameplay
- Detection: reject the panel mutation and route the command to Core.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

### Failure 20: false completion claim
- Detection: downgrade the handoff to planning/static evidence only.
- Owner response: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- Player-facing truth: Unknown weapon/ammo IDs, invalid armor, dead combatants, empty mags, duplicate encounters, invalid ranges, and interrupted saves fail closed through the existing resolver and diagnostics.
- Persistence response: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism response: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- Verification: focused test or static source/data assertion appropriate to the failure class; no invented runtime pass.

## Layered focused verification matrix
Commands are exact paths only where the current test file exists. They are future implementation gates for this plan-only package.
### Verification layer 01: Core unit
- Coverage: valid input, boundary, missing optional field, duplicate/unknown reference, state invariant.
- Domain contract: CombatCatalog.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 02: Core transition
- Coverage: one legal transition, one illegal transition, repeated transition, cancellation/rollback.
- Domain contract: TacticalCombatSystem.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 03: save round-trip
- Coverage: capture, serialize, restore, deep-copy isolation, old version/defaults.
- Domain contract: BallisticsSystem.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 04: data integrity
- Coverage: schema_version, snake_case IDs, duplicate IDs, references, ranges, collection shape.
- Domain contract: WeaponConditionSystem.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 05: host wiring
- Coverage: setup, command dispatch, event subscription, refresh, disposal, missing owner.
- Domain contract: CombatHostSession.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 06: UI projection
- Coverage: truthful current state, disabled action, focus order, controller/keyboard close/back.
- Domain contract: combat_catalog.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 07: headless/selftest
- Coverage: bounded deterministic scenario and diagnostics without a renderer.
- Domain contract: CombatCatalog.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 08: cross-system
- Coverage: owner event to consumer, ordering, idempotency, no parallel state.
- Domain contract: TacticalCombatSystem.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 09: replay
- Coverage: same seed/day/input produces same state hash and fact order.
- Domain contract: BallisticsSystem.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 10: content utilization
- Coverage: authored record has a current loader, consumer, and observable outcome.
- Domain contract: WeaponConditionSystem.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 11: migration
- Coverage: old save and current catalog remain readable or fail with a named reason.
- Domain contract: CombatHostSession.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

### Verification layer 12: rollback
- Coverage: feature flag/owner boundary permits disabling the delta without data loss.
- Domain contract: combat_catalog.
- Owner: TacticalCombatSystem owns combat resolution; CombatCatalog owns definitions; inventory owns custody; medical owns wounds; host owns session lifecycle and presentation.
- State rule: Combat session, weapon condition, ammunition custody, wounds, and encounter outcome use their existing owners and persistence; catalog definitions remain authored data, not mutable save state.
- Determinism rule: All hit, jam, AI, and action-order rolls use ISeededRng forks and stable ordinal tie-breaks; no real-time frame timing may alter gameplay resolution.
- UI rule: ['src/UI/CombatPanel.cs', 'src/UI/CombatDetailPanel.cs', 'src/UI/CombatHudOverlay.cs']
- Test selection: prefer an existing focused file; run it alone first for a new test; keep save/load, determinism, lifecycle, mutation, and cross-system workflows separate.
- Acceptance evidence: record command, result, selected tests, limitations, and any current-worktree concurrency caveat.

#### Current focused command 01
- Test: `Ashfall.Core.Tests/Plan54CombatCatalogTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Plan54CombatCatalogTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 02
- Test: `Ashfall.Core.Tests/CombatCatalogValidationTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/CombatCatalogValidationTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 03
- Test: `Ashfall.Core.Tests/CombatBallisticsTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/CombatBallisticsTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 04
- Test: `Ashfall.Core.Tests/CombatWeaponConditionTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/CombatWeaponConditionTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 05
- Test: `Ashfall.Core.Tests/CombatSaveRoundTripTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/CombatSaveRoundTripTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 06
- Test: `Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

#### Current focused command 07
- Test: `Ashfall.Core.Tests/Combat/JourneyCombatLoopContractTests.cs`
- Command: `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/JourneyCombatLoopContractTests.cs`
- Scope: use only for the confirmed owner/contract; this plan does not claim it has been run in this planning pass.

> **Structured body length before post-250K polishing:** 514,087 characters.
# Post-250K deep polishing pass

The architecture body above reached 514,165 characters before this pass. This is a second deep polishing phase, not a license to pad. Re-read the plan as an implementer and tighten it against the current source/data snapshot.

## Deep polish A — authority and collision recheck

- Re-run the owner/caller audit for: `CombatCatalog`, `TacticalCombatSystem`, `BallisticsSystem`, `WeaponConditionSystem`, `CombatHostSession`, `combat_catalog`.
- Confirm each proposed mutation has exactly one writer. A host adapter may translate a fact; a panel may display it; neither becomes authority.
- Check for legacy or parallel names before proposing any new class, DTO, event, catalog, save section, or RNG stream.
- Treat the master authority as a read-only design lens. Current source/data wins when they disagree, and the disagreement is recorded rather than hidden.
- Recheck the live worktree status recorded in each evidence dossier. A dirty source path is a coordination warning, not a stable acceptance result.

## Deep polish B — data and consumer precision

- For every candidate row, name the loader, consumer, reference validator, and observable outcome.
- Replace counts copied from the old plan with current JSON counts or an explicit census task.
- Remove any row whose only consumer is a test, a prose generator, or a panel.
- Preserve additive schema compatibility and explain old-data defaults.
- Check that narrative text does not promise an effect that the current owner cannot produce.

## Deep polish C — state, save, and replay precision

- Confirm the existing save owner and DTO before naming a field.
- Test capture/restore, deep-copy isolation, old-version defaults, partial/corrupt input, and mid-event reload.
- Confirm repeated commands and replay cannot duplicate one-shot effects.
- Use the existing seeded stream/fork; document every random draw and tie-break.
- Treat a UI refresh as a projection, never as persistence or mutation.

## Deep polish D — failure and player truth

- Walk null, empty, missing, duplicate, stale, hostile, dead, unavailable, and repeated-event cases.
- Ensure blocked/unavailable states are legible and do not masquerade as success.
- Keep accessibility behavior attached to the same command/state surface as the visual behavior.
- Record which failures are diagnostic-only, which defer presentation, and which halt the transition.

## Deep polish E — implementation readiness

- Replace generic file lists with owner-specific action verbs and completion gates.
- Keep the phase order dependency-safe: premise, owner, Core, persistence, data, host, presentation, end-to-end, polish, closeout.
- Give the next implementer exact focused commands, test selection rationale, and stop conditions.
- Keep a final residual-risk list for decision-gated or concurrent work rather than hiding it in prose.

# Post-250K deep polishing pass — second pass

The first post-250K pass above checked structure and owner collisions. This second pass is a separate adversarial review after the plan has reached its depth checkpoint; it is not a duplicate paragraph exercise.

## Second-pass adversarial questions

- What would a reviewer incorrectly assume after reading only the executive summary?
- Which sentence describes historical intent but could be mistaken for current behavior?
- Which current owner, save path, host session, or event seam is missing from the impact map?
- Which data row has a valid ID but no reachable consumer?
- Which UI label could claim a consequence before the Core transition succeeds?
- Which failure currently falls through as a default success, duplicate event, or stale cache?
- Which random choice lacks a named seed/fork/tie-break?
- Which old-save field lacks a default, migration, or deep-copy test?
- Which concurrent package or decision gate could invalidate the proposed path?
- What is the smallest rollback that leaves the previous owner state readable?

## Second-pass correction protocol

For every answer, classify the issue as `CURRENT_EVIDENCE`, `PROPOSED_EXTENSION`, `DECISION_GATE`, `CONCURRENT_CLAIM`, or `OUT_OF_SCOPE`. Update the relevant numbered section and record the exact path/command that would close the issue. If no current evidence supports a claim, remove the claim rather than softening it with adjectives. If a proposed change needs a new architecture decision, stop at the gate and name the decision owner. This pass must leave the plan more precise, not merely longer.

## Second-pass acceptance

- [ ] Every “current” statement has a path or is marked as an open premise.
- [ ] Every “implemented” statement names a current caller or is downgraded to catalog-only.
- [ ] Every proposed state field names its save owner and migration behavior.
- [ ] Every proposed random/temporal behavior names determinism treatment.
- [ ] Every UI consequence has a command/state source.
- [ ] Every failure has a safe expected outcome.
- [ ] Every focused command resolves or is labeled future implementation work.
- [ ] Every decision-gated or concurrently claimed seam is explicit.

# Final precision and reaccuracy pass

1. Re-read every current path cited in the dossier and mark missing paths as open premises.
2. Re-run the hash verifier and data JSON parse check at the final snapshot.
3. Re-run the symbol occurrence audit and distinguish declarations, host consumers, and test-only references.
4. Check all current focused test commands resolve to existing files.
5. Remove unsupported historical counts, “sealed” claims, invented APIs, and unproven caller assertions.
6. Reconcile the plan title and requested delta with the actual current owner; if the old plan is already implemented or stale, state maintenance or blocked scope plainly.
7. Confirm Core remains engine-free, JSON remains authoritative, and no parallel authority is proposed.
8. Confirm UI, failure, save, determinism, and accessibility contracts are concrete.
9. Confirm rollback is local and does not require destructive state reset.
10. Record limitations honestly: this package is planning-only and does not run implementation tests.

# Full repolishing phase

The final repolishing pass is a quality audit over the complete document, not an append-only slogan. Read the plan from executive summary through handoff as one artifact.

- **Coherence:** every section uses the same owner names, state terms, and delta.
- **Evidence:** every important claim points to a current path, current data record, read-only authority anchor, or explicit unknown.
- **Architecture:** no duplicate system, parallel save, second RNG, engine dependency, or UI authority is smuggled into a phase.
- **Mechanics:** effects are expressed through existing commands/events and have a visible or auditable consequence.
- **Continuity:** references, days, locations, factions, and narrative facts use canonical IDs and current catalogs.
- **Failure:** invalid and unavailable states are defined, bounded, and truthful.
- **Persistence:** capture/restore and migration are named for every durable delta; no new section is casually proposed.
- **Determinism:** random sources, ordering, and replay comparisons are explicit.
- **UI:** panels are thin, accessible, refreshable, and non-authoritative.
- **Testing:** each layer has a focused command or a clear reason it must be authored later.
- **Rollback:** every phase has a local reversal and preservation rule.
- **Handoff:** the next implementer can start with the first safe step without interpreting the plan around stale prose.

## Repolish acceptance checklist

- [ ] Current owner/API confirmed from source.
- [ ] Current data schema and references confirmed.
- [ ] Existing save owner and migration path confirmed.
- [ ] Existing host/event seam confirmed.
- [ ] Existing UI surface and accessibility path confirmed.
- [ ] Focused test paths resolve or are labeled future work.
- [ ] Deterministic replay and idempotency contract stated.
- [ ] Failure and old-save behavior stated.
- [ ] No unsupported completion claim remains.
- [ ] Rollback and owner handoff are actionable.
- [ ] Plan remains implementation-ready without production edits in this package.

# Implementation handoff

## MUST PRESERVE

- The current owner named in this plan and the one-authority rule.
- Godot as the presentation/host authority and Core as engine-free domain logic.
- Authoritative JSON under `Assets/StreamingAssets/Data/`.
- Existing save ownership, migration semantics, seeded RNG contracts, and event ordering.
- Existing accessibility, focus, controller/keyboard close/back, and truthful UI behavior.

## MUST ADD

- Only the smallest confirmed Core/data/host/UI extension needed by the current delta.
- Exact catalog validation, consumer reachability, save/restore, failure, determinism, and focused tests.
- A bounded content tranche whose records are consumed and observable.
- A local rollback/disablement path and a concise implementation handoff.

## MUST NOT DO

- Do not create a second ledger, save store, selector, event authority, simulation, or panel-owned rule.
- Do not edit production, data, tests, UI, assets, or generated indexes during this planning package.
- Do not restore Unity or add engine references to Core.
- Do not claim tests, host wiring, save integration, or player reachability from static intent alone.
- Do not use unseeded randomness, wall-clock ordering, or hash iteration order for deterministic behavior.

## VERIFY WITH

- The exact focused `scripts/run_test.sh` commands listed in this plan after the implementation claim is opened.
- Current catalog integrity and consumer/utilization checks.
- A bounded Core test, save round-trip/migration test, host wiring test, and deterministic replay comparison.
- Godot headless/UI checks only when the confirmed implementation touches the host/UI path.

## FIRST SAFE IMPLEMENTATION STEP

Re-read the current owner, the first current catalog, the first current host/session, and the first focused test listed in the dossier. Write a one-page premise table that classifies each as live, partial, stale, or blocked. Do not author content or code until the table has one owner and one non-overlapping implementation seam.
