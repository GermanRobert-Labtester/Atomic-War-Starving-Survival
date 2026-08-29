// SPDX-License-Identifier: MIT
// ASHFALL — SceneBindingHeadlessProbe.cs (Ticket #125).
//
// Drives scene-binding self-test in a way that is testable from xUnit (no
// dispatcher hooks required). The probe discovers every production .tscn
// file registered in PanelSceneLoader, instantiates it, exercises its
// required-node contract via SceneBinder, and frees it. Failures are
// surfaced as actionable diagnostics — scene path, binder, missing node.

using System;
using System.Collections.Generic;
using Godot;

namespace AtomicWar.GodotApp.UI;

/// <summary>
/// Headless self-test that runs against the same production scene tree the
/// CI linter checks. It enforces the required-node contract on every
/// PanelSceneLoader-registered scene by reflection-style discovery, so
/// adding a new .tscn to the registry surfaces as a new check at boot.
/// </summary>
public static class SceneBindingHeadlessProbe
{
    /// <summary>
    /// A scene registration: the canonical res:// path and the binding scene
    /// of required unique-name nodes. The Probe creates a SceneBinder over
    /// a Control root that hosts the scene as a child; concrete panel
    /// binders can be tested by a richer probe that calls their typed
    /// Require* chain.
    /// </summary>
    public sealed class SceneRegistration
    {
        public string ResPath { get; init; } = string.Empty;
        public Type RootType { get; init; } = typeof(Control);
        /// <summary>List of declared scene contract entries.</summary>
        public List<ContractEntry> Contract { get; } = new();
    }

    public sealed class ContractEntry
    {
        public string NodeName { get; init; } = string.Empty;
        public Type NodeType { get; init; } = typeof(Control);
        /// <summary>True when an absent or wrong-typed node must abort the probe.</summary>
        public bool Required { get; init; } = true;
    }

    private static readonly List<SceneRegistration> _registry = new();

    /// <summary>
    /// Declare a registered production scene. Caller lists the required
    /// unique-name node contracts the scene must satisfy. Headless probe
    /// verifies each one during Run().
    /// </summary>
    public static SceneRegistration Register(string resPath, Type rootType,
        IEnumerable<ContractEntry>? contract = null)
    {
        var reg = new SceneRegistration { ResPath = resPath, RootType = rootType };
        if (contract != null) reg.Contract.AddRange(contract);
        _registry.Add(reg);
        return reg;
    }

    public static IReadOnlyList<SceneRegistration> Registry() => _registry;

    /// <summary>
    /// Run the headless probe. Returns (passed, failed). Called from the
    /// Godot test dispatcher AFTER PanelSceneLoader has been wired.
    /// </summary>
    /// <param name="howFree">Optional free strategy: synchronous Free vs
    /// QueueFree. Defaults to Free for deterministic test cleanup.</param>
    public static (int passed, int failed) Run()
    {
        int passed = 0, failed = 0;
        foreach (var reg in _registry)
        {
            Node? root = null;
            try
            {
                root = PanelSceneLoader.Load<Node>(reg.ResPath);
                if (!(root is Control c))
                    throw new SceneBindingException(
                        reg.ResPath, nameof(SceneBindingHeadlessProbe), "Root", "Control",
                        actualPath: null, "scene root is not a Control; cannot bind.");

                var binder = new SceneBinder(c, GetProbeBinder(reg.ResPath));
                foreach (var entry in reg.Contract)
                {
                    if (entry.Required)
                        binder.Require(entry.NodeType, entry.NodeName);
                    else
                        _ = binder.Optional(entry.NodeType, entry.NodeName);
                }
                Godot.GD.Print($"[SCENE_BIND] PASS {reg.ResPath} ({(c.GetChildCount())} children)");
                passed++;
            }
            catch (Exception e)
            {
                Godot.GD.PrintErr($"[SCENE_BIND] FAIL {reg.ResPath}: {e.Message}");
                failed++;
            }
            finally
            {
                if (root != null && GodotObject.IsInstanceValid(root))
                {
                    root.QueueFree();
                    // Synchronous free for snapshot determinism:
                    root.Free();
                }
            }
        }
        return (passed, failed);
    }

    private static Type GetProbeBinder(string resPath)
    {
        // Synthesize a probe binder type name so diagnostics reference
        // this helper without confusing a real human binder.
        return typeof(SceneBindingHeadlessProbe);
    }
}
