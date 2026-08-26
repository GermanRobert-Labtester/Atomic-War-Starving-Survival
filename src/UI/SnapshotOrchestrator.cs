using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// SnapshotOrchestrator — single-frame-per-panel capture driver for
    /// ASHFALL UI snapshot regression.
    ///
    /// Pipeline (per Target):
    ///   Idle     → Mount: build SubViewport + isolated root + bg + panel
    ///   Mounted  → _Ready() runs on next frame, panels against Control
    ///             type may flip Visible=false
    ///   FramesWait → a couple of process frames, ensure panel _Ready runs
    ///   Reshow   → re-enforce Visible=true on the panel so layouts draw
    ///   RenderOnce → SubViewport.UpdateMode.Once + several frames
    ///   Read     → read GetTexture().GetImage().SavePng, then (per mode)
    ///             diff against the golden or overwrite it
    ///   Cleanup  → restore overlays, remove SubViewport
    ///   Idle     → next target
    ///
    /// Modes:
    ///   CaptureOnly — write captures into outputRoot (legacy behaviour)
    ///   Diff        — capture into captureRoot, compare against goldenRoot,
    ///                 report MATCH / NEW / DRIFT(pixels %) / FAIL per panel;
    ///                 exit code 1 when any drift or capture failure occurred
    ///   Regenerate  — capture straight into goldenRoot (overwrites goldens)
    ///
    /// Rendering requirements: SubViewport texture reads need a real renderer.
    /// The documented capture environment (docs/ui/snapshot_manifest.json) is
    /// Forward+ on an X11 display; the headless dummy renderer yields empty
    /// framebuffers, which this orchestrator reports as a per-panel FAIL
    /// rather than silently writing blank goldens.
    /// </summary>
    public partial class SnapshotOrchestrator : Node
    {
        public bool Done = false;
        public int Exit = 0;

        public enum CaptureMode { CaptureOnly, Diff, Regenerate }

        private sealed record TargetResult(string Id, string Verdict, string Detail);

        private readonly List<TargetResult> _results = new();
        private CaptureMode _mode = CaptureMode.CaptureOnly;
        private string _goldenRoot = "";
        private string _captureRoot = "";

        private Queue<Target> _queue = new();
        private Target _current;
        private string _outputRoot = "";
        private int _ticksLeft;
        private SubViewport? _sub;
        private List<(Node, bool)> _hidden = new();
        private string _outPath = "";
        private Node? _mountedPanel;

        enum Phase { Idle, Mounted, FramesWait, Reshow, RenderOnce, Read, Cleanup, Done }
        private Phase _phase = Phase.Idle;

        private struct Target
        {
            public string StableId;
            public string Title;
            public string PanelCtor;
            public int Width;
            public int Height;
        }

        /// <summary>Legacy capture-only entry: writes PNGs into outputRoot.</summary>
        public void Begin(IEnumerable<SnapshotHarness.Target> targets, string outputRoot)
        {
            BeginInternal(targets, CaptureMode.CaptureOnly, outputRoot, outputRoot);
        }

        /// <summary>Diff mode: capture into captureRoot, compare against goldenRoot.</summary>
        public void BeginDiff(IEnumerable<SnapshotHarness.Target> targets, string goldenRoot, string captureRoot)
        {
            BeginInternal(targets, CaptureMode.Diff, goldenRoot, captureRoot);
        }

        /// <summary>Regenerate mode: capture directly into goldenRoot (overwrites goldens).</summary>
        public void BeginRegenerate(IEnumerable<SnapshotHarness.Target> targets, string goldenRoot)
        {
            BeginInternal(targets, CaptureMode.Regenerate, goldenRoot, goldenRoot);
        }

        private void BeginInternal(
            IEnumerable<SnapshotHarness.Target> targets,
            CaptureMode mode,
            string goldenRoot,
            string captureRoot)
        {
            _mode = mode;
            _goldenRoot = goldenRoot;
            _captureRoot = captureRoot;
            _outputRoot = captureRoot;
            Directory.CreateDirectory(captureRoot);
            _queue.Clear();
            _results.Clear();
            foreach (var t in targets)
                _queue.Enqueue(new Target
                {
                    StableId = t.StableId,
                    Title = t.Title,
                    PanelCtor = t.PanelCtor,
                    Width = t.Width,
                    Height = t.Height,
                });
            _phase = Phase.Idle;
            SetProcess(true);
        }

        public override void _Process(double delta)
        {
            if (_phase == Phase.Done) return;

            switch (_phase)
            {
                case Phase.Idle:
                    if (_queue.Count == 0) { FinishAll(); return; }
                    _current = _queue.Dequeue();
                    _outPath = Path.Combine(_captureRoot, $"{_current.StableId}.png");
                    try { Mount(); _phase = Phase.Mounted; _ticksLeft = 2; }
                    catch (Exception e) { Fail($"mount-exception: {e.Message}"); }
                    break;

                case Phase.Mounted:
                    if (--_ticksLeft <= 0) { _phase = Phase.FramesWait; _ticksLeft = 8; }
                    break;

                case Phase.FramesWait:
                    if (--_ticksLeft <= 0) {
                        // Re-enforce visibility after _Ready has potentially reset it
                        if (_mountedPanel is CanvasItem ci)
                        {
                            ci.Visible = true;
                            if (ci is Control ctl) ctl.SetAnchorsPreset(Control.LayoutPreset.FullRect);
                            if (ci is Control pc) TraverseVisible(pc);
                        }
                        _phase = Phase.Reshow; _ticksLeft = 12;
                    }
                    break;

                case Phase.Reshow:
                    if (--_ticksLeft <= 0) {
                        if (_sub == null) { Fail("no-sub"); break; }
                        // Force a render before the read. Earlier tick=2 fired while the
                        // HYBRID-shell nested layouts were still resolving, returning an
                        // all-zero framebuffer. Push Always -> Once and ForceDraw so the
                        // next framebuffer pull has the laid-out content.
                        _sub.RenderTargetUpdateMode = SubViewport.UpdateMode.Always;
                        _sub.RenderTargetUpdateMode = SubViewport.UpdateMode.Once;
                        RenderingServer.ForceDraw(false);
                        _phase = Phase.RenderOnce; _ticksLeft = 6;
                    }
                    break;

                case Phase.RenderOnce:
                    if (--_ticksLeft <= 0) { Read(); }
                    break;

                case Phase.Cleanup:
                    Cleanup();
                    break;
            }
        }

        private void Mount()
        {
            var sub = new SubViewport
            {
                Name = $"Snap_{_current.StableId}",
                Size = new Vector2I(_current.Width, _current.Height),
                RenderTargetUpdateMode = SubViewport.UpdateMode.Always,
                Disable3D = true,
                TransparentBg = false, // composite against bg
            };
            var root = new Control { Name = $"SnapRoot_{_current.StableId}" };
            root.SetAnchorsPreset(Control.LayoutPreset.FullRect);
            sub.AddChild(root);

            var bg = new ColorRect { Color = new Color(0.10f, 0.09f, 0.07f, 1.0f) };
            bg.SetAnchorsPreset(Control.LayoutPreset.FullRect);
            root.AddChild(bg);

            var asm = typeof(SnapshotOrchestrator).Assembly;
            var type = asm.GetType(_current.PanelCtor);
            if (type == null) { throw new Exception($"type-not-found: {_current.PanelCtor}"); }
            var inst = (Node)Activator.CreateInstance(type);
            Node mounted;
            if (inst is Control ctl)
            {
                ctl.SetAnchorsPreset(Control.LayoutPreset.FullRect);
                ctl.Visible = true;
                root.AddChild(ctl);
                mounted = ctl;
            }
            else
            {
                var wrap = new CenterContainer();
                wrap.SetAnchorsPreset(Control.LayoutPreset.FullRect);
                root.AddChild(wrap);
                wrap.AddChild(inst);
                mounted = inst;
            }
            try { mounted!.Call("Open"); } catch (Exception ex) { GD.PrintErr($"[SnapshotOrchestrator] Open call failed: {ex.Message}"); }

            var hostRoot = (Engine.GetMainLoop() as SceneTree)?.Root;
            if (hostRoot == null) { throw new Exception("no-SceneTree-root"); }
            hostRoot.AddChild(sub);

            HideOverlays(hostRoot, sub, _hidden);

            _sub = sub;
            _mountedPanel = mounted;
        }

        private void Read()
        {
            try
            {
                if (_sub == null) { Fail("no-sub-inread"); return; }
                var tex = _sub.GetTexture();
                if (tex == null) { Fail("no-texture (renderer unavailable — SubViewport reads need a real display/renderer, not --headless)"); return; }
                var img = tex.GetImage();
                if (img == null) { Fail("no-image (renderer unavailable — SubViewport reads need a real display/renderer, not --headless)"); return; }
                if (img.GetWidth() <= 0 || img.GetHeight() <= 0)
                { Fail($"empty framebuffer {img.GetWidth()}x{img.GetHeight()} (renderer unavailable — SubViewport reads need a real display/renderer, not --headless)"); return; }

                // Guard against blank captures: a fully uniform framebuffer means
                // the panel never drew (documented headless/dummy-renderer failure).
                img.Convert(Image.Format.Rgba8);
                if (IsUniformFramebuffer(img))
                { Fail("uniform framebuffer — panel content did not render (headless dummy renderer?)"); return; }

                var err = img.SavePng(_outPath);
                if (err != Error.Ok) { Fail($"SavePng-err={err}"); return; }
                var bytes = new FileInfo(_outPath).Length;

                switch (_mode)
                {
                    case CaptureMode.Diff:
                        EvaluateAgainstGolden(bytes);
                        break;
                    case CaptureMode.Regenerate:
                        GD.Print($"  [PASS] {_current.StableId} -> {_outPath} ({bytes}B)");
                        _results.Add(new TargetResult(_current.StableId, "PASS", $"{bytes}B"));
                        break;
                    default:
                        GD.Print($"  [PASS] {_current.StableId} -> {_outPath} ({bytes}B)");
                        _results.Add(new TargetResult(_current.StableId, "PASS", $"{bytes}B"));
                        break;
                }
                _phase = Phase.Cleanup;
            }
            catch (Exception e) { Fail($"read-exception: {e.Message}"); }
        }

        private void EvaluateAgainstGolden(long captureBytes)
        {
            string goldenPath = Path.Combine(_goldenRoot, $"{_current.StableId}.png");
            if (!File.Exists(goldenPath))
            {
                GD.Print($"  [NEW]   {_current.StableId} (no golden at {goldenPath}; capture at {_outPath})");
                _results.Add(new TargetResult(_current.StableId, "NEW", $"capture={_outPath}"));
                return;
            }

            // Fast path: byte-identical files are certainly identical renders.
            var captureBytesArr = File.ReadAllBytes(_outPath);
            var goldenBytesArr = File.ReadAllBytes(goldenPath);
            if (captureBytesArr.Length == goldenBytesArr.Length &&
                captureBytesArr.AsSpan().SequenceEqual(goldenBytesArr))
            {
                GD.Print($"  [MATCH] {_current.StableId} ({captureBytes}B)");
                _results.Add(new TargetResult(_current.StableId, "MATCH", $"{captureBytes}B"));
                return;
            }

            // Slow path: pixel-level diff for a meaningful drift report.
            var golden = Image.LoadFromFile(goldenPath);
            var capture = Image.LoadFromFile(_outPath);
            if (golden == null || capture == null)
            {
                Drift(goldenPath, -1, -1, "golden-or-capture-decode-failed");
                return;
            }
            golden.Convert(Image.Format.Rgba8);
            capture.Convert(Image.Format.Rgba8);
            if (golden.GetWidth() != capture.GetWidth() || golden.GetHeight() != capture.GetHeight())
            {
                Drift(goldenPath, -1, (long)golden.GetWidth() * golden.GetHeight(),
                    $"size-mismatch golden {golden.GetWidth()}x{golden.GetHeight()} vs capture {capture.GetWidth()}x{capture.GetHeight()}");
                return;
            }

            var ga = golden.GetData().ToArray();
            var ca = capture.GetData().ToArray();
            long total = ga.Length / 4;
            long diff = 0;
            for (int i = 0; i < ga.Length; i += 4)
                if (ga[i] != ca[i] || ga[i + 1] != ca[i + 1] || ga[i + 2] != ca[i + 2] || ga[i + 3] != ca[i + 3])
                    diff++;

            Drift(goldenPath, diff, total, $"{diff} of {total} pixels ({100f * diff / Math.Max(1, total):0.00}%)");
        }

        private void Drift(string goldenPath, long diffPixels, long totalPixels, string detail)
        {
            GD.Print($"  [DRIFT] {_current.StableId}: {detail} — capture={_outPath} golden={goldenPath}");
            _results.Add(new TargetResult(_current.StableId, "DRIFT", detail));
        }

        private static bool IsUniformFramebuffer(Image img)
        {
            var data = img.GetData().ToArray();
            if (data.Length < 4) return true;
            byte r = data[0], g = data[1], b = data[2], a = data[3];
            // Full scan: 1280×800 RGBA ≈ 4MB — cheap against writing a blank golden.
            for (int i = 4; i < data.Length; i += 4)
                if (data[i] != r || data[i + 1] != g || data[i + 2] != b || data[i + 3] != a)
                    return false;
            return true;
        }

        private void Fail(string why)
        {
            GD.Print($"  [FAIL] {_current.StableId}: {why}");
            _results.Add(new TargetResult(_current.StableId, "FAIL", why));
            _phase = Phase.Cleanup;
        }

        private void Cleanup()
        {
            RestoreOverlays(_hidden);
            _hidden.Clear();
            if (_sub != null)
            {
                var hostRoot = (Engine.GetMainLoop() as SceneTree)?.Root;
                hostRoot?.CallDeferred(Node.MethodName.RemoveChild, _sub);
                _sub.QueueFree();
                _sub = null;
            }
            _mountedPanel = null;
            _phase = Phase.Idle;
        }

        private void FinishAll()
        {
            Done = true;

            int match = 0, isNew = 0, drift = 0, fail = 0, pass = 0;
            var driftIds = new List<string>();
            var failIds = new List<string>();
            foreach (var r in _results)
            {
                switch (r.Verdict)
                {
                    case "MATCH": case "PASS": match++; pass++; break;
                    case "NEW": isNew++; break;
                    case "DRIFT": drift++; driftIds.Add(r.Id); break;
                    case "FAIL": fail++; failIds.Add(r.Id); break;
                }
            }

            string tag = _mode == CaptureMode.Regenerate ? "UI_SNAPSHOT_REGENERATE" : "UI_SNAPSHOT_UITEST";
            GD.Print($"{tag} SUMMARY: {_results.Count} targets — {match} match, {isNew} new, {drift} drift, {fail} fail");
            if (driftIds.Count > 0)
                GD.Print($"{tag} DRIFT: {string.Join(", ", driftIds)}");
            if (failIds.Count > 0)
                GD.Print($"{tag} FAIL: {string.Join(", ", failIds)}");

            // Diff mode is a gate: any drift or capture failure fails the run.
            // NEW goldens are reported but do not fail (they need approval, not repair).
            bool ok = _mode != CaptureMode.Diff || (drift == 0 && fail == 0);
            Exit = ok ? 0 : 1;
            GD.Print(ok ? $"{tag} PASS" : $"{tag} FAIL");
            if (_mode == CaptureMode.Diff && fail > 0)
                GD.Print($"{tag} HINT: capture failures with 'renderer unavailable' mean the run used --headless; " +
                         "SubViewport reads need a real display (see docs/ui/snapshot_manifest.json — Forward+ on DISPLAY=:0).");

            SetProcess(false);
            (Engine.GetMainLoop() as SceneTree)?.CallDeferred(SceneTree.MethodName.Quit, Exit);
            _phase = Phase.Done;
        }

        private static void HideOverlays(Node root, Node except, List<(Node, bool)> hidden)
        {
            if (root == except) return;
            if (root is CanvasItem ci && ci.Visible)
            {
                hidden.Add((root, ci.Visible));
                ci.Visible = false;
            }
            foreach (var child in root.GetChildren())
                HideOverlays(child, except, hidden);
        }

        private static void RestoreOverlays(List<(Node, bool)> hidden)
        {
            foreach (var (n, prev) in hidden)
                if (n is CanvasItem ci) ci.Visible = prev;
        }

        private static void TraverseVisible(Node root)
        {
            if (root is CanvasItem ci) ci.Visible = true;
            foreach (var c in root.GetChildren())
                TraverseVisible(c);
        }
    }
}
