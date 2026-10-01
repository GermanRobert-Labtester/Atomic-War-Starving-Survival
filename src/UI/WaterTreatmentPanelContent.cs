// SPDX-License-Identifier: MIT
// ASHFALL — Scene-root binding type for res://assets/ui/panels/WaterTreatmentPanel.tscn.
//
// This class is intentionally empty and is NOT a placeholder. It is the typed root
// the scene attaches (`script = ExtResource("1_water")` on the scene root), which is
// what makes PanelSceneLoader.Load<WaterTreatmentPanelContent>() assignable: Godot's
// PackedScene.Instantiate<T> is an `unbox.any` hard cast, so the scene root's Script
// must derive from the requested type.
//
// Composition lives in the .tscn (ContentStack, DetailText, CharcoalButton,
// DistillButton, OsmosisButton, ReplaceFilterButton — all unique_name_in_owner).
// Behavior lives in WaterTreatmentPanel, which resolves those nodes through
// SceneBinder and asserts the surface is renderable via RequireNonEmptySurface.
// Adding members here would fork that authority; do not.
using Godot;

namespace AtomicWar.GodotApp.UI;

public partial class WaterTreatmentPanelContent : Control { }
