using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests;

public class CombatCommandTests
{
    [Fact]
    public void PreviewPlayerFieldRepair_Available_WhenWeaponNeedsRepair()
    {
        var sys = Engine(2, 40);
        var weapon = sys.State.Weapons[0];
        weapon.ConditionPct = 0.5f;

        var preview = sys.PreviewPlayerFieldRepair(weapon.OwnerSurvivorId, stateVersion: 10L);

        Assert.True(preview.IsAvailable);
        Assert.Equal(10L, preview.StateVersion);
        Assert.Equal("combat.preview_field_repair", preview.MessageKey);
    }

    [Fact]
    public void ExecutePlayerFieldRepair_StalePreview_RejectsWithoutMutation()
    {
        var sys = Engine(2, 40);
        var weapon = sys.State.Weapons[0];

        var result = sys.ExecutePlayerFieldRepair(weapon.OwnerSurvivorId, expectedStateVersion: 99L, currentStateVersion: 100L);

        Assert.False(result.IsSuccess);
        Assert.Equal("stale_preview", result.FailureCode);
    }

    private static TacticalCombatSystem Engine(int enemyCount, float enemyHealth)
    {
        var sys = new TacticalCombatSystem(null, CombatHostPorts.NoOp());
        sys.BeginEncounter("enc_t", "exp", "loc", "Loc", 1, 99, PlayerRoster(), RifleWeapons(), enemyCount, enemyHealth);
        return sys;
    }

    private static List<CombatantState> PlayerRoster(int n = 1)
    {
        var list = new List<CombatantState>();
        for (int i = 0; i < n; i++)
            list.Add(new CombatantState
            {
                Id = "p" + i,
                Name = "Survivor " + i,
                SurvivorId = "sv" + i,
                IsPlayer = true,
                Health = 100,
                MaxHealth = 100
            });
        return list;
    }

    private static List<WeaponInstanceState> RifleWeapons(int n = 1)
    {
        var list = new List<WeaponInstanceState>();
        for (int i = 0; i < n; i++)
            list.Add(new WeaponInstanceState
            {
                InstanceId = "w" + i,
                WeaponId = "weapon_assault_rifle",
                OwnerSurvivorId = "sv" + i,
                ConditionPct = 0.9f,
                AmmoId = "ammo_556",
                AmmoRemaining = 50
            });
        return list;
    }
}
