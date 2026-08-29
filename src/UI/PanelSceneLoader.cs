// SPDX-License-Identifier: MIT
// ASHFALL — PackedScene-based panel factory (Ticket #125).
//
// Returns a strongly-typed panel/binder from a designer-owned scene. Preloads
// the PackedScene once via ResourceLoader and caches it by absolute path so
// repeated reopenings (modal stack, tab switching) do not re-import the scene.
//
// Resources from scenes are kept alive across the process lifetime; the scene
// they were loaded from is not modified in any way that would invalidate the
// import — the only mutation in production is class-namespaced void patching,
// which Godot allows on PackedScene instances only when the consumer is the
// editor. We never mutate; we instantiate, parent, free.

using System;
using System.Collections.Concurrent;
using Godot;

namespace AtomicWar.GodotApp.UI;

public static class PanelSceneLoader
{
    private static readonly ConcurrentDictionary<string, PackedScene> _cache = new();

    /// <summary>
    /// Load a PackedScene from a canonical res:// path and instantiate a
    /// strongly-typed Control/MarginContainer/PanelContainer/etc. derived from it.
    /// Throws <see cref="SceneBindingException"/> if the scene cannot be loaded.
    /// </summary>
    public static T Load<T>(string resPath) where T : Node
    {
        if (string.IsNullOrEmpty(resPath))
            throw new ArgumentException("resPath must not be empty", nameof(resPath));
        if (!resPath.StartsWith("res://", StringComparison.Ordinal))
            throw new ArgumentException("resPath must start with res://", nameof(resPath));

        var packed = _cache.GetOrAdd(resPath, path =>
        {
            if (!ResourceLoader.Exists(path))
                throw new SceneBindingException(
                    resPath, nameof(PanelSceneLoader), "-", typeof(T).Name, actualPath: null,
                    $"scene resource does not exist or is not on the canonical icon path. " +
                    "Ticket #124 allows case-normalized fallbacks for textures, " +
                    "but scene resources must use the exact res:// case.");
            var ps = ResourceLoader.Load<PackedScene>(path);
            if (ps == null)
                throw new SceneBindingException(
                    resPath, nameof(PanelSceneLoader), "-", typeof(T).Name, actualPath: null,
                    "ResourceLoader.Load returned null — the file exists but Godot cannot " +
                    "parse it. Run scripts/ci/scene-lint.py or 'godot --check-only'.");
            return ps;
        });
        if (!packed.CanInstantiate())
            throw new SceneBindingException(
                resPath, nameof(PanelSceneLoader), "-", typeof(T).Name, actualPath: null,
                "the PackedScene is not instantiable; usually a missing require node or " +
                "broken ExtResource. Run scene-lint.py for actionable diagnostics.");
        var node = packed.Instantiate<T>();
        if (node == null)
            throw new SceneBindingException(
                resPath, nameof(PanelSceneLoader), "-", typeof(T).Name, actualPath: null,
                "PackedScene.Instantiate<T>() returned null; the scene root must have the " +
                "Script attached at design time that derives from the requested C# type.");
        return node;
    }
}
