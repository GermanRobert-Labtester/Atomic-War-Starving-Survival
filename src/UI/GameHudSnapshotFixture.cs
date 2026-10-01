// SPDX-License-Identifier: MIT
using System;
using Godot;
using AtomicWar.GodotApp;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// Deterministic fixture for the GameHudOverlay snapshot target. Projects a
/// fixed day / health / radiation / needs state so the needs glance row renders
/// a stable, reviewable image without binding a live campaign.
///
/// <para>The orchestrator invokes the fixture before the mounted node enters
/// the SceneTree, so the HUD's labels are still unassigned. The projection is
/// therefore applied on <see cref="Node.Ready"/> (or immediately if the node is
/// already ready), which still lands before the capture frame.</para>
/// </summary>
internal static class GameHudSnapshotFixture
{
    public static IDisposable? Bind(Node node)
    {
        if (node is not GameHudOverlay hud)
            return null;

        if (hud.IsNodeReady())
            Apply(hud);
        else
            hud.Ready += () => Apply(hud);
        return null;
    }

    private static void Apply(GameHudOverlay hud)
    {
        hud.UpdateState(12, 340, "holdfast_watch", "FalloutStorm");
        hud.UpdateHealth(64, 100);
        hud.UpdateRadiation(41f);
        hud.UpdateNeeds(78, 62, 55, 44, 71, new Ashfall.Core.Survivors.NeedsProfile());
        hud.UpdateOnboardingProgress("3/7 · FOOD");
    }
}
