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
        // PackedScene.Instantiate<T>() compiles to `unbox.any !!T` — a hard cast.
        // A scene root whose attached Script is not a T therefore escapes as a bare
        // InvalidCastException carrying no scene path, no requested type and no
        // actual type. Instantiate once and test assignability so the mismatch is
        // reported truthfully (P094 empty/unbound-surface guard).
        var root = packed.Instantiate();
        if (root is not T node)
        {
            string actual = DescribeRoot(root);
            root?.Free();
            throw new SceneBindingException(
                resPath, nameof(PanelSceneLoader), "<root>", typeof(T).Name, actualPath: actual,
                "the scene root is not assignable to the requested C# type. Either attach a " +
                "Script to the scene root that derives from the requested type, or load the " +
                "scene with the type its root Script actually declares. A scene that declares " +
                "an [ext_resource type=\"Script\"] it never assigns via `script = ExtResource(...)` " +
                "instantiates as a plain Control and always fails this cast.",
                nodeHint: false);
        }
        return node;
    }

    /// <summary>
    /// Truthful description of what a scene root actually is, for diagnostics:
    /// the runtime type plus the attached Script's res:// path, or an explicit
    /// statement that no Script is attached.
    /// </summary>
    private static string DescribeRoot(Node? root)
    {
        if (root == null) return "<null root>";
        var scriptVariant = root.GetScript();
        if (scriptVariant.VariantType == Variant.Type.Nil)
            return $"{root.GetType().Name} (scene root has NO Script attached)";
        var script = scriptVariant.As<Script>();
        string path = string.IsNullOrEmpty(script?.ResourcePath) ? "<unknown>" : script!.ResourcePath;
        return $"{root.GetType().Name} (scene root Script = {path})";
    }
}
