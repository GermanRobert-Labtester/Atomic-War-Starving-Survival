using System;
using System.Collections.Generic;
using System.IO;
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
    ///   Read     → read GetTexture().GetImage().SavePng
    ///   Cleanup  → restore overlays, remove SubViewport
    ///   Idle     → next target
    /// </summary>
    public partial class SnapshotOrchestrator : Node
    {
        public bool Done = false;
        public int Exit = 0;

        public static void RunWithTargets(IEnumerable<SnapshotHarness.Target> targets, string outputRoot)
        {
            var inst = new SnapshotOrchestrator();
            inst.Begin(targets, outputRoot);
        }

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

        public void Begin(IEnumerable<SnapshotHarness.Target> targets, string outputRoot)
        {
            _outputRoot = outputRoot;
            Directory.CreateDirectory(outputRoot);
            _queue.Clear();
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
                    _outPath = Path.Combine(_outputRoot, $"{_current.StableId}.png");
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
            try { mounted.Call("Open"); } catch { }

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
                if (tex == null) { Fail("no-texture"); return; }
                var img = tex.GetImage();
                if (img == null) { Fail("no-image"); return; }
                var err = img.SavePng(_outPath);
                if (err != Error.Ok) { Fail($"SavePng-err={err}"); return; }
                var bytes = new FileInfo(_outPath).Length;
                GD.Print($"  [PASS] {_current.StableId} -> {_outPath} ({bytes}B)");
                _phase = Phase.Cleanup;
            }
            catch (Exception e) { Fail($"read-exception: {e.Message}"); }
        }

        private void Fail(string why)
        {
            GD.Print($"  [FAIL] {_current.StableId}: {why}");
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
            Done = true; Exit = 0;
            GD.Print($"UI_SNAPSHOT_UITEST DONE");
            SetProcess(false);
            (Engine.GetMainLoop() as SceneTree)?.CallDeferred(SceneTree.MethodName.Quit, 0);
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
