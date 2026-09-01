// SPDX-License-Identifier: MIT
// ASHFALL — Typed node binding for designer-owned .tscn panels (Ticket #125).
//
// Why this exists
// ───────────────
// Pre-Ticket-125 panels constructed their layouts in C# (`new VBoxContainer`,
// `new Label`, `AddChild(...)`). Each panel grew imperative layout code in
// its constructor and a getter for every interesting node. The C# file
// therefore mixed composition (where do the labels sit), presentation
// (what string does each label hold), and command handling (the action
// invoked when the button is pressed).
//
// Ticket #125 splits that work: scenes own composition, C# owns binding and
// application behavior. Scenes are designer-editable .tscn files arranged in
// assets/ui/{components,panels,modals,scenes}/. Each scene's root Control
// exposes a unique-name in owner for every node the C# binder expects.
//
// SceneBinder is the single typed-binding surface those C# binders use.
// It replaces the 1,000+ unsanitised GetNode("Panel/VBox/Button") calls
// that previously littered the codebase with a uniform, fail-fast lookup
// driven by the unique_name_in_owner attribute. Each Require* call emits
// an actionable diagnostic naming:
//   • the scene resource path the binder belongs to,
//   • the binder type the developer wrote,
//   • the expected unique-name and the type the binder expects,
//   • the actual node path if found but with the wrong type,
// so a malformed scene fails the build/tests with a message a designer
// can act on — never the ambiguous NullReferenceException buried ten
// frames deep in _Process.

using System;
using System.Collections.Generic;
using Godot;

namespace AtomicWar.GodotApp.UI;

public sealed class SceneBindingException : Exception
{
    public string ScenePath { get; }
    public string BinderName { get; }
    public string NodeName { get; }
    public string ExpectedType { get; }
    public string? ActualPath { get; }

    public SceneBindingException(
        string scenePath,
        string binderName,
        string nodeName,
        string expectedType,
        string? actualPath,
        string message)
        : base($"SceneBindingException: {message}" +
               $"\n  Scene         : {scenePath}" +
               $"\n  Binder        : {binderName}" +
               $"\n  Expected Node : %'{nodeName}' : {expectedType}" +
               (actualPath != null ? $"\n  Actual Path   : {actualPath}" : "") +
               $"\nExpected scene file should declare a [node name=\"{nodeName}\" type=\"{expectedType}\"] " +
               "with unique_name_in_owner=true.")
    {
        ScenePath = scenePath;
        BinderName = binderName;
        NodeName = nodeName;
        ExpectedType = expectedType;
        ActualPath = actualPath;
    }
}

/// <summary>
/// Typed node binding helper for designer-owned scenes.
/// Cache all bindings in <see cref="Bind"/> (typically called from _Ready),
/// never call <see cref="Require{T}"/> per-frame.
/// </summary>
public sealed class SceneBinder
{
    private readonly Node _root;
    private readonly string _scenePath;
    private readonly Type _binderType;
    private readonly Dictionary<(string, Type), Node> _cache = new();

    /// <summary>
    /// Construct a binding host for a scene whose root is <paramref name="root"/>.
    /// The scene path is used for diagnostics; the binder type scopes the
    /// exception message so the developer sees exactly which C# binder failed.
    /// </summary>
    public SceneBinder(Node root, Type binderType)
    {
        _root = root ?? throw new ArgumentNullException(nameof(root));
        _binderType = binderType ?? throw new ArgumentNullException(nameof(binderType));
        _scenePath = root.SceneFilePath ?? "<unsaved>";
    }

    /// <summary>
    /// Resolve and cache every required binding. Throws <see cref="SceneBindingException"/>
    /// if any required unique-name node is missing or has the wrong type.
    /// </summary>
    public void Require<T>(string uniqueName) where T : Node
    {
        var key = (uniqueName, typeof(T));
        if (_cache.ContainsKey(key)) return;

        // Godot exposes unique-name_in_owner nodes via the absolute "%Name" path.
        var node = _root.GetNodeOrNull($"%{uniqueName}");
        if (node == null)
        {
            throw new SceneBindingException(
                _scenePath, _binderType.FullName ?? "<unknown>", uniqueName,
                typeof(T).Name, actualPath: null,
                "required scene node not found. The scene must declare a unique_name_in_owner=true " +
                "node of this name.");
        }
        if (!(node is T typed))
        {
            throw new SceneBindingException(
                _scenePath, _binderType.FullName ?? "<unknown>", uniqueName,
                typeof(T).Name, actualPath: node.GetPath().ToString(),
                "scene node was found but is not the expected type. The scene author " +
                "should change the node's type to match the contract.");
        }
        _cache[key] = typed;
    }

    /// <summary>
    /// Optional binding. Returns null when the unique-name is absent or has
    /// the wrong type; does NOT throw. Suitable for nodes whose absence is
    /// not a contract violation.
    /// </summary>
    public T? Optional<T>(string uniqueName) where T : Node
    {
        var key = (uniqueName, typeof(T));
        if (_cache.TryGetValue(key, out var cached)) return (T)cached;
        var node = _root.GetNodeOrNull($"%{uniqueName}");
        if (node is T typed) _cache[key] = typed;
        return null;
    }

    public T Get<T>(string uniqueName) where T : Node
    {
        Require<T>(uniqueName);
        return (T)_cache[(uniqueName, typeof(T))];
    }

    /// <summary>
    /// Runtime-type variant used by the headless probe to validate declared
    /// scene contracts without a generic type parameter at each call site.
    /// </summary>
    public void Require(Type nodeType, string uniqueName)
    {
        var key = (uniqueName, nodeType);
        if (_cache.ContainsKey(key)) return;

        var node = _root.GetNodeOrNull($"%{uniqueName}");
        if (node == null)
        {
            throw new SceneBindingException(
                _scenePath, _binderType.FullName ?? "<unknown>", uniqueName,
                nodeType.Name, actualPath: null,
                "required scene node not found.");
        }
        if (!nodeType.IsInstanceOfType(node))
        {
            throw new SceneBindingException(
                _scenePath, _binderType.FullName ?? "<unknown>", uniqueName,
                nodeType.Name, actualPath: node.GetPath().ToString(),
                "scene node exists but does not match the declared type.");
        }
        _cache[key] = node;
    }

    public Node? Optional(Type nodeType, string uniqueName)
    {
        var key = (uniqueName, nodeType);
        if (_cache.TryGetValue(key, out var cached)) return cached;
        var node = _root.GetNodeOrNull($"%{uniqueName}");
        if (node != null && nodeType.IsInstanceOfType(node)) _cache[key] = node;
        return null;
    }
}
