// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Combat;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// T27 — deterministic live-encounter fixture for the CombatHudOverlay
/// snapshot target. Builds a seeded realtime encounter against a session
/// (the same construction the game uses) so the monitor renders its LIVE
/// preflight action rows instead of the unbound fixture rows.
/// </summary>
internal static class CombatHudSnapshotFixture
{
    public static IDisposable? Bind(Node node)
    {
        if (node is not CombatHudOverlay panel)
            return null;

        if (CombatCatalog.GetWeapon("weapon_assault_rifle") == null)
            CombatCatalog.SeedDefaults();
        CombatArenaCatalog.SeedDefaults();

        var session = new CombatHostSession();
        var roster = new List<CombatantState>
        {
            new CombatantState { Id = "p_yuki", Name = "Yuki", SurvivorId = "survivor_yuki", IsPlayer = true, Health = 82, MaxHealth = 100, ArmorRating = 0.4f, CoverRating = 0.3f, Lane = 1 },
            new CombatantState { Id = "p_mikhail", Name = "Gunner Mikhail", SurvivorId = "survivor_gunner_mikhail", IsPlayer = true, Health = 61, MaxHealth = 100, ArmorRating = 0.5f, CoverRating = 0.2f, Lane = 1 }
        };
        var weapons = new List<WeaponInstanceState>
        {
            new WeaponInstanceState { InstanceId = "w0", WeaponId = "weapon_assault_rifle", OwnerSurvivorId = "survivor_yuki", ConditionPct = 0.86f, AmmoId = "ammo_556", AmmoRemaining = 21 },
            new WeaponInstanceState { InstanceId = "w1", WeaponId = "weapon_pipe_rifle", OwnerSurvivorId = "survivor_gunner_mikhail", ConditionPct = 0.72f, AmmoId = "ammo_357", AmmoRemaining = 9 }
        };
        if (!session.Engine.BeginEncounter(
                "enc_snapshot", "exp_snapshot", "loc_denial_cut", "The Denial Cut", 1, 42,
                roster, weapons, enemyCount: 3, enemyHealth: 40))
            throw new InvalidOperationException("Combat HUD snapshot fixture encounter failed to start.");
        if (!session.Engine.EnableRealtime(CombatArenaCatalog.DefaultArenaId, new SeededRng(42)))
            throw new InvalidOperationException("Combat HUD snapshot fixture realtime arm failed.");

        // Walk a few deterministic ticks so lanes, cooldowns and the event
        // log show a mid-fight state rather than a bare spawn.
        var idle = new CombatInputFrame { SubjectId = "p_yuki" };
        var rng = new SeededRng(42);
        for (int i = 0; i < 30; i++)
            session.Engine.TickRealtime(TacticalCombatSystem.RealtimeSimDt, idle, rng);

        panel.Bind(session);
        panel.Open();
        return null; // the session is pure C#; no Godot resources to release
    }
}
