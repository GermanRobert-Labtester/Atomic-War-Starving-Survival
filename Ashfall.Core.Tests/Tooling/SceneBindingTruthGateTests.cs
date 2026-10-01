// SPDX-License-Identifier: MIT
// ASHFALL — SceneBindingTruthGateTests (P096).
//
// In-suite mirror of scripts/ci/scene-binding-truth-gate.sh so the contract is
// enforced by `bin/run-scoped-tests` as well as by CI, and so a violation names
// its row instead of failing as one opaque bash exit code.
//
// The five defect classes covered are the ones behind P089–P095:
//   1. an empty src/**/*Content.cs that no scene attaches as its root script
//      (a stub masquerading as a scene-binding shim),
//   2. a .tscn declaring [ext_resource type="Script"] it never assigns, so the
//      root instantiates as a plain Control while looking script-bound,
//   3. a declared script path that does not exist on disk,
//   4. a production PanelSceneLoader.Load<T>(scene) whose T the scene root's
//      attached script does not satisfy — PackedScene.Instantiate<T> is an
//      unbox.any hard cast, so this throws InvalidCastException at runtime
//      (the DailyBriefingModal defect),
//   5. a scene bound through a *Content type whose root ships `visible = false`,
//      which renders a blank surface because AshfallDashboardShell.SetContent
//      does not reset Visible (the WaterTreatmentPanel defect).
//
// TEST-AGGREGATION: source_rows=5 aggregate_cases=5 saved_cases=0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public class SceneBindingTruthGateTests
    {
        private const string ResPrefix = "res://";

        private sealed class SceneInfo
        {
            public string FsPath = string.Empty;
            /// <summary>res:// script path assigned to the ROOT node, or null.</summary>
            public string? RootScriptRes;
            /// <summary>Script ext_resources declared but never assigned anywhere.</summary>
            public readonly List<string> Unassigned = new();
            /// <summary>Declared script paths missing from disk.</summary>
            public readonly List<string> Dangling = new();
            /// <summary>True when the root node block sets `visible = false`.</summary>
            public bool RootHidden;
        }

        private static string FindRepoRoot()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                if (File.Exists(Path.Combine(search, "project.godot")) &&
                    File.Exists(Path.Combine(search, "Ashfall.csproj")))
                {
                    return search;
                }
                string? parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            throw new InvalidOperationException(
                "Could not locate repository root from " + Directory.GetCurrentDirectory());
        }

        private static string ResToFs(string root, string resPath)
            => Path.Combine(root, resPath.StartsWith(ResPrefix, StringComparison.Ordinal)
                ? resPath.Substring(ResPrefix.Length)
                : resPath);

        private static IEnumerable<string> EnumFiles(string root, string subdir, string pattern)
        {
            string dir = Path.Combine(root, subdir);
            if (!Directory.Exists(dir)) return Array.Empty<string>();
            return Directory.EnumerateFiles(dir, pattern, SearchOption.AllDirectories)
                .Where(p => !p.Contains("/obj/") && !p.Contains("/bin/") && !p.Contains("/.git/"))
                .OrderBy(p => p, StringComparer.Ordinal);
        }

        private static readonly Regex ExtResourceScript = new(
            @"^\[ext_resource\s+type=""Script""\s+path=""(?<path>[^""]+)""\s+id=""(?<id>[^""]+)""",
            RegexOptions.Compiled);
        private static readonly Regex AssignScript = new(
            @"^script = ExtResource\(""(?<id>[^""]+)""\)", RegexOptions.Compiled);
        private static readonly Regex NodeHeader = new(
            @"^\[node\s+name=""(?<name>[^""]*)""(?<rest>.*)\]", RegexOptions.Compiled);
        private static readonly Regex ClassDecl = new(
            @"\bclass\s+(?<name>[A-Za-z_]\w*)\s*(?::\s*(?<base>[A-Za-z_]\w*))?",
            RegexOptions.Compiled);
        private static readonly Regex LoadCall = new(
            @"PanelSceneLoader\.Load<(?<type>[A-Za-z_]\w*)>\(\s*(?<arg>[^)]*?)\s*\)",
            RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex StringDeclaration = new(
            @"(?:const\s+)?(?:string|var)\s+(?<name>[A-Za-z_]\w*)\s*=\s*(?<value>[^;\r\n]+);",
            RegexOptions.Compiled);
        private static readonly Regex IdentifierExpression = new(
            @"^[A-Za-z_]\w*$", RegexOptions.Compiled);
        private static readonly Regex BlockComment = new(
            @"/\*.*?\*/", RegexOptions.Compiled | RegexOptions.Singleline);
        // `(?<!:)` keeps `res://` inside string literals from being read as a comment.
        private static readonly Regex LineComment = new(
            @"(?<!:)//.*", RegexOptions.Compiled);

        /// <summary>
        /// Remove comments so a prose sentence containing the word "class" cannot be
        /// mistaken for a type declaration.
        /// </summary>
        private static string StripComments(string text)
            => LineComment.Replace(BlockComment.Replace(text, " "), " ");

        private static bool TryResolveSceneArgument(string expression, string source, int beforeIndex, out string scenePath)
        {
            scenePath = string.Empty;
            expression = expression.Trim().Trim('(', ')').Trim();
            if (TryReadStringLiteral(expression, out scenePath)) return scenePath.StartsWith(ResPrefix, StringComparison.Ordinal);

            if (IdentifierExpression.IsMatch(expression))
            {
                Match declaration = StringDeclaration.Matches(source)
                    .Cast<Match>()
                    .Where(m => m.Index < beforeIndex && m.Groups["name"].Value == expression)
                    .LastOrDefault()!;
                if (declaration == null) return false;
                return TryResolveSceneArgument(declaration.Groups["value"].Value, source,
                    declaration.Index, out scenePath);
            }

            var parts = SplitStringConcatenation(expression);
            if (parts.Count < 2) return false;
            var builder = new System.Text.StringBuilder();
            foreach (string part in parts)
            {
                if (!TryResolveSceneArgument(part, source, beforeIndex, out string value)) return false;
                builder.Append(value);
            }
            scenePath = builder.ToString();
            return scenePath.StartsWith(ResPrefix, StringComparison.Ordinal);
        }

        private static bool TryReadStringLiteral(string expression, out string value)
        {
            value = string.Empty;
            if (!(expression.StartsWith("\"", StringComparison.Ordinal) ||
                  expression.StartsWith("@\"", StringComparison.Ordinal) ||
                  expression.StartsWith("$\"", StringComparison.Ordinal) ||
                  expression.StartsWith("$@\"", StringComparison.Ordinal) ||
                  expression.StartsWith("@$\"", StringComparison.Ordinal))) return false;
            bool verbatim = expression.StartsWith("@\"", StringComparison.Ordinal) ||
                            expression.StartsWith("$@\"", StringComparison.Ordinal) ||
                            expression.StartsWith("@$\"", StringComparison.Ordinal);
            bool interpolated = expression.StartsWith("$\"", StringComparison.Ordinal) ||
                                expression.StartsWith("$@\"", StringComparison.Ordinal) ||
                                expression.StartsWith("@$\"", StringComparison.Ordinal);
            if (expression.Count(c => c == '"') != 2) return false;
            int quote = expression.IndexOf('"');
            if (quote < 0 || expression.Length <= quote + 1 || expression[^1] != '"') return false;
            string body = expression.Substring(quote + 1, expression.Length - quote - 2);
            if (interpolated && body.Contains('{')) return false;
            value = verbatim ? body.Replace("\"\"", "\"", StringComparison.Ordinal) :
                body.Replace("\\\\", "\\", StringComparison.Ordinal).Replace("\\\"", "\"", StringComparison.Ordinal);
            return true;
        }

        private static List<string> SplitStringConcatenation(string expression)
        {
            var parts = new List<string>();
            bool inString = false;
            bool verbatim = false;
            int start = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                if (c == '"' && (i == 0 || expression[i - 1] != '\\' || verbatim))
                {
                    if (inString && verbatim && i + 1 < expression.Length && expression[i + 1] == '"')
                    {
                        i++;
                        continue;
                    }
                    inString = !inString;
                    if (inString) verbatim = i > 0 && expression[i - 1] == '@';
                    continue;
                }
                if (c == '+' && !inString)
                {
                    parts.Add(expression.Substring(start, i - start).Trim());
                    start = i + 1;
                }
            }
            if (parts.Count == 0) return parts;
            parts.Add(expression.Substring(start).Trim());
            return parts;
        }

        /// <summary>
        /// Parse every .tscn under assets/ and src/ into its binding facts. The
        /// root node is the first [node] block with no parent= attribute; only a
        /// `script = ExtResource(...)` inside that block binds the root type.
        /// </summary>
        private static List<SceneInfo> ParseScenes(string root)
        {
            var scenes = new List<SceneInfo>();
            foreach (string path in EnumFiles(root, "assets", "*.tscn")
                         .Concat(EnumFiles(root, "src", "*.tscn")))
            {
                var info = new SceneInfo { FsPath = path };
                var declared = new Dictionary<string, string>(StringComparer.Ordinal);
                var assignedIds = new HashSet<string>(StringComparer.Ordinal);
                bool inRoot = false;

                foreach (string raw in File.ReadAllLines(path))
                {
                    string line = raw.TrimEnd();

                    var ext = ExtResourceScript.Match(line);
                    if (ext.Success)
                    {
                        declared[ext.Groups["id"].Value] = ext.Groups["path"].Value;
                        continue;
                    }

                    var assign = AssignScript.Match(line);
                    if (assign.Success)
                    {
                        assignedIds.Add(assign.Groups["id"].Value);
                        if (inRoot && info.RootScriptRes == null)
                            info.RootScriptRes = declared.TryGetValue(assign.Groups["id"].Value, out var rp) ? rp : null;
                        continue;
                    }

                    var node = NodeHeader.Match(line);
                    if (node.Success)
                    {
                        inRoot = !node.Groups["rest"].Value.Contains("parent=", StringComparison.Ordinal);
                        continue;
                    }

                    if (inRoot && line == "visible = false") info.RootHidden = true;
                }

                foreach (var kv in declared)
                {
                    if (!assignedIds.Contains(kv.Key))
                        info.Unassigned.Add($"id=\"{kv.Key}\" -> {kv.Value}");
                    if (!File.Exists(ResToFs(root, kv.Value)))
                        info.Dangling.Add(kv.Value);
                }

                scenes.Add(info);
            }
            return scenes;
        }

        private static string Rel(string root, string path)
            => Path.GetRelativePath(root, path).Replace('\\', '/');

        [Fact]
        public void GateScript_IsRegisteredInCiManifest()
        {
            string root = FindRepoRoot();
            string script = Path.Combine(root, "scripts", "ci", "scene-binding-truth-gate.sh");
            Assert.True(File.Exists(script),
                $"P096 gate script must exist at scripts/ci/scene-binding-truth-gate.sh (looked at {script})");

            string manifest = File.ReadAllText(Path.Combine(root, "docs", "ci", "CI_GATE_MANIFEST.json"));
            Assert.Contains("scene_binding_truth", manifest);
            Assert.Contains("scripts/ci/scene-binding-truth-gate.sh", manifest);
            Assert.Contains("Scene Binding Truth Gate: PASS", manifest);
        }

        [Fact]
        public void NoStubContentScript_UnlessASceneAttachesItAsRoot()
        {
            string root = FindRepoRoot();
            var scenes = ParseScenes(root);
            var attached = new HashSet<string>(
                scenes.Where(s => s.RootScriptRes != null)
                      .Select(s => ResToFs(root, s.RootScriptRes!)),
                StringComparer.Ordinal);

            var stubs = new List<string>();
            foreach (string cs in EnumFiles(root, "src", "*Content.cs"))
            {
                // Strip line comments, blank lines, lone braces and the boilerplate
                // header (using/namespace/class declaration). Anything left is a member.
                var members = File.ReadAllLines(cs)
                    .Select(l => Regex.Replace(l, "//.*$", string.Empty))
                    .Select(l => l.Trim())
                    .Where(l => l.Length > 0)
                    .Where(l => !Regex.IsMatch(l, @"^[{}];?$"))
                    .Where(l => !Regex.IsMatch(l, @"^(using |namespace |\[|public partial class |public class |internal partial class |internal class )"))
                    .ToList();

                if (members.Count == 0 && !attached.Contains(cs))
                    stubs.Add($"{Rel(root, cs)} (empty type body; no .tscn attaches it as a root script)");
            }

            Assert.True(stubs.Count == 0,
                "P089/P090 regression — stub content script(s) that are not live scene-root bindings:\n  " +
                string.Join("\n  ", stubs));
        }

        [Fact]
        public void NoSceneDeclaresAnUnassignedOrDanglingScript()
        {
            string root = FindRepoRoot();
            var problems = new List<string>();
            foreach (var s in ParseScenes(root))
            {
                foreach (string u in s.Unassigned)
                    problems.Add($"{Rel(root, s.FsPath)} declares [ext_resource type=\"Script\" {u}] but never assigns it " +
                                 "via `script = ExtResource(...)` — the root instantiates as a plain Control while looking script-bound");
                foreach (string d in s.Dangling)
                    problems.Add($"{Rel(root, s.FsPath)} declares script {d} which does not exist on disk");
            }

            Assert.True(problems.Count == 0,
                "P095/P096 regression — scene script declarations that lie:\n  " +
                string.Join("\n  ", problems));
        }

        [Fact]
        public void EveryProductionLoadRequest_IsSatisfiedByTheSceneRootScript()
        {
            string root = FindRepoRoot();
            var byFs = ParseScenes(root).ToDictionary(s => s.FsPath, StringComparer.Ordinal);

            // Base types a generic probe (selftest / layout harness) legitimately asks for.
            var genericProbes = new HashSet<string>(StringComparer.Ordinal) { "Node", "Control", "CanvasItem" };

            var mismatches = new List<string>();
            int checkedCalls = 0;
            foreach (string cs in EnumFiles(root, "src", "*.cs"))
            {
                string text = StripComments(File.ReadAllText(cs));
                foreach (Match m in LoadCall.Matches(text))
                {
                    string want = m.Groups["type"].Value;
                    if (genericProbes.Contains(want)) continue;
                    checkedCalls++;

                    string expression = m.Groups["arg"].Value;
                    if (!TryResolveSceneArgument(expression, text, m.Index, out string sceneRes))
                    {
                        mismatches.Add($"{Rel(root, cs)} requests Load<{want}>({expression}) with a dynamic path that cannot be resolved from a string literal or preceding local/const string declaration");
                        continue;
                    }

                    string sceneFs = ResToFs(root, sceneRes);
                    if (!byFs.TryGetValue(sceneFs, out var scene))
                    {
                        mismatches.Add($"{Rel(root, cs)} requests Load<{want}>(\"{sceneRes}\") but that scene does not exist");
                        continue;
                    }

                    if (scene.RootScriptRes == null)
                    {
                        mismatches.Add($"{Rel(root, cs)} requests Load<{want}>(\"{sceneRes}\") but the scene root has NO script " +
                                       $"attached, so Instantiate<{want}> throws InvalidCastException (unbox.any hard cast)");
                        continue;
                    }

                    string scriptFs = ResToFs(root, scene.RootScriptRes);
                    if (!File.Exists(scriptFs)) continue; // already reported as dangling

                    var decl = ClassDecl.Match(StripComments(File.ReadAllText(scriptFs)));
                    string got = decl.Success ? decl.Groups["name"].Value : "<none>";
                    string baseType = decl.Success ? decl.Groups["base"].Value : string.Empty;
                    if (got != want && baseType != want)
                    {
                        mismatches.Add($"{Rel(root, cs)} requests Load<{want}>(\"{sceneRes}\") but the scene root script " +
                                       $"{scene.RootScriptRes} declares class {got} : {baseType}");
                    }
                }
            }

            Assert.True(checkedCalls > 0,
                "expected at least one concrete production PanelSceneLoader.Load<T> call site to verify");
            Assert.True(mismatches.Count == 0,
                "P091 regression — production scene load requests the scene root cannot satisfy:\n  " +
                string.Join("\n  ", mismatches));
        }

        [Fact]
        public void DynamicSceneArguments_ResolveStringLocalsAndRejectRuntimeValues()
        {
            const string valid = "const string panelPath = \"res://assets/ui/panels/WaterTreatmentPanel.tscn\";\n" +
                                 "var scenePath = panelPath;\n" +
                                 "PanelSceneLoader.Load<WaterTreatmentPanelContent>(scenePath);";
            var validCall = LoadCall.Match(valid);
            Assert.True(validCall.Success, "dynamic Load<T>(pathVariable) call should be found");
            Assert.True(TryResolveSceneArgument(validCall.Groups["arg"].Value, valid, validCall.Index, out string resolved),
                $"could not resolve '{validCall.Groups["arg"].Value}' from the preceding declarations");
            Assert.Equal("res://assets/ui/panels/WaterTreatmentPanel.tscn", resolved);

            const string unresolved = "PanelSceneLoader.Load<WaterTreatmentPanelContent>(GetScenePath());";
            var unresolvedCall = LoadCall.Match(unresolved);
            Assert.True(unresolvedCall.Success, "runtime Load<T>(expression) call should be found");
            Assert.False(TryResolveSceneArgument(unresolvedCall.Groups["arg"].Value, unresolved,
                unresolvedCall.Index, out _), "unverifiable runtime expressions must not silently pass the gate");
        }

        [Fact]
        public void BoundContentScenes_AreNotShippedPreHidden()
        {
            string root = FindRepoRoot();
            var hidden = new List<string>();
            foreach (var s in ParseScenes(root))
            {
                if (s.RootScriptRes == null || !s.RootHidden) continue;
                string scriptFs = ResToFs(root, s.RootScriptRes);
                string name = Path.GetFileNameWithoutExtension(scriptFs);
                // Only *Content roots are embedded content sub-scenes; a top-level
                // panel/modal the host shows and hides itself may start hidden.
                if (!name.EndsWith("Content", StringComparison.Ordinal)) continue;
                hidden.Add($"{Rel(root, s.FsPath)} binds {name} but its root sets `visible = false`; " +
                           "AshfallDashboardShell.SetContent does not reset Visible, so the surface renders blank");
            }

            Assert.True(hidden.Count == 0,
                "P092/P094 regression — bound content scene(s) that would render empty:\n  " +
                string.Join("\n  ", hidden));
        }
    }
}
