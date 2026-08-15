using System;
using System.Collections.Generic;
using System.Text;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Environment;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Expansion IV — Chapter 45 HUD Wiring.
    /// Bridges the four overlay components to DiegeticHudView paint methods.
    ///
    ///   • StructuralStressWireframe  → PaintStructuralEntropy
    ///   • LetheDripGauge            → PaintLetheDripGauge
    ///   • OzoneScourgeOverlay       → PaintOzoneScourge
    ///   • MemoryFlashVignette       → PaintMemoryFlash
    ///   • GenerationalPsychologySystem → PaintGenerationalReadout
    ///
    /// Attach to the same GameObject as DiegeticHudController.
    /// Assign references via Inspector or Bind() from GameBootstrap.
    /// </summary>
    [RequireComponent(typeof(DiegeticHudController))]
    public class Expansion4HudController : MonoBehaviour
    {
        // ---- Inspector-wired Expansion IV component references ----------
        [SerializeField] private StructuralStressWireframe _structuralWireframe;
        [SerializeField] private LetheDripGauge            _letheDripGauge;
        [SerializeField] private OzoneScourgeOverlay        _ozoneOverlay;
        [SerializeField] private MemoryFlashVignette        _memoryFlash;

        // ---- System references injected via Bind() ----------------------
        private StructuralEntropySystem    _entropySystem;
        private LetheProtocolSystem        _letheSystem;
        private OzoneScourgeSystem         _ozoneSystem;
        private GenerationalPsychologySystem _genSystem;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        // ---- Cached state to avoid per-frame string allocs --------------
        private DiegeticHudController _controller;
        private bool   _genPanelEverOpened;
        private string _lastGenEvent   = string.Empty;
        private string _lastGenBody    = string.Empty;
        private int    _lastPreWar     = -1;
        private int    _lastBunkerBorn = -1;

        // Reusable room-state list fed into PaintStructuralEntropy.
        private readonly List<(bool isSpalling, float corrosion)> _roomStateCache
            = new List<(bool, float)>(32);

        // StringBuilder for generational cohort body text (avoids allocs per tick).
        private readonly StringBuilder _genSb = new StringBuilder(256);

        // ----------------------------------------------------------------

        private void Awake()
        {
            _controller = GetComponent<DiegeticHudController>();
        }

        /// <summary>
        /// Called from GameBootstrap after all systems are wired.
        /// Any parameter may be null — the controller gracefully no-ops
        /// for systems that haven't been initialised yet.
        /// </summary>
        public void Bind(
            StructuralEntropySystem    entropySystem,
            LetheProtocolSystem        letheSystem,
            OzoneScourgeSystem         ozoneSystem,
            GenerationalPsychologySystem genSystem,
            Func<IReadOnlyList<Survivor>> getSurvivors,
            StructuralStressWireframe  wireframe  = null,
            LetheDripGauge             dripGauge  = null,
            OzoneScourgeOverlay        ozoneComp  = null,
            MemoryFlashVignette        flashComp  = null)
        {
            _entropySystem = entropySystem;
            _letheSystem   = letheSystem;
            _ozoneSystem   = ozoneSystem;
            _genSystem     = genSystem;
            _getSurvivors  = getSurvivors;

            if (wireframe  != null) _structuralWireframe = wireframe;
            if (dripGauge  != null) _letheDripGauge      = dripGauge;
            if (ozoneComp  != null) _ozoneOverlay         = ozoneComp;
            if (flashComp  != null) _memoryFlash          = flashComp;

            // Bind the drip gauge to the Lethe system so it subscribes to
            // OnReservoirLevelChanged — it won't self-subscribe if Bind hasn't been called.
            if (_letheDripGauge != null && _letheSystem != null)
                _letheDripGauge.BindLetheSystem(_letheSystem);

            // Bind the ozone overlay to the ozone system.
            if (_ozoneOverlay != null && _ozoneSystem != null)
                _ozoneOverlay.BindOzoneSystem(_ozoneSystem);

            // Bind the structural wireframe to the entropy system.
            if (_structuralWireframe != null && _entropySystem != null)
                _structuralWireframe.BindEntropySystem(_entropySystem);
        }

        // ----------------------------------------------------------------
        // Per-frame paint pump
        // ----------------------------------------------------------------

        private void Update()
        {
            if (_controller == null || !_controller.IsBuilt) return;
            var view = _controller.View;
            if (view == null) return;

            PumpStructuralEntropy(view);
            PumpLetheDrip(view);
            PumpOzoneScourge(view);
            PumpMemoryFlash(view);
            PumpGenerational(view);
        }

        // ---- Structural Entropy ----------------------------------------

        private void PumpStructuralEntropy(DiegeticHudView view)
        {
            bool open = _structuralWireframe != null && _structuralWireframe.IsOverlayActive
                        && _entropySystem != null;

            if (!open)
            {
                view.PaintStructuralEntropy(false, 1f, string.Empty, null);
                return;
            }

            float integrity = _entropySystem.ShelterIntegrity;
            string status = BuildEntropyStatus(integrity);

            // Populate room state cache from the entropy system's room list.
            _roomStateCache.Clear();
            var rooms = _entropySystem.Rooms;
            if (rooms != null)
            {
                int n = Mathf.Min(rooms.Count, 20);
                for (int i = 0; i < n; i++)
                {
                    var r = rooms[i];
                    _roomStateCache.Add((r.IsSpalling, r.RebarCorrosion));
                }
            }

            view.PaintStructuralEntropy(true, integrity, status, _roomStateCache);
        }

        private static string BuildEntropyStatus(float integrity)
        {
            if (integrity >= 0.80f) return $"INTEGRITY: {integrity:P0} — NOMINAL";
            if (integrity >= 0.50f) return $"INTEGRITY: {integrity:P0} — DEGRADED";
            if (integrity >= 0.30f) return $"INTEGRITY: {integrity:P0} — CRITICAL — EVACUATE LOWER LEVELS";
            return $"INTEGRITY: {integrity:P0} — COLLAPSE IMMINENT";
        }

        // ---- Lethe Drip Gauge ------------------------------------------

        private void PumpLetheDrip(DiegeticHudView view)
        {
            // The Lethe gauge co-locates with the water purification terminal.
            // We open it whenever the Lethe system is initialised.
            bool open = _letheSystem != null;
            if (!open)
            {
                view.PaintLetheDripGauge(false, 1f, string.Empty, false);
                return;
            }

            float level     = _letheSystem.ReservoirLevel;
            bool  isRedLine = _letheDripGauge != null && _letheDripGauge.IsRedLineWarning;
            string status   = isRedLine
                ? $"LETHE RES: {level:P0} — RED LINE — SUPPRESSION FAILING"
                : $"LETHE RES: {level:P0}";

            view.PaintLetheDripGauge(open, level, status, isRedLine);
        }

        // ---- Ozone Scourge Overlay -------------------------------------

        private void PumpOzoneScourge(DiegeticHudView view)
        {
            bool scourgeActive = _ozoneSystem != null && _ozoneSystem.IsOzoneScourgeActive();
            if (!scourgeActive)
            {
                view.PaintOzoneScourge(false, 0f, false, string.Empty);
                return;
            }

            float progress = _ozoneOverlay != null
                ? Mathf.Clamp01(_ozoneOverlay.UnshieldedStareTimer / 2.0f)
                : 0f;
            bool warningVisible = _ozoneOverlay != null && _ozoneOverlay.IsWarningActive;

            string status = warningVisible
                ? "UV EXPOSURE EXCEEDS SAFE THRESHOLD. EQUIP FILTER NOW."
                : "UV ALERT: SURFACE FEED OVEREXPOSED.";

            view.PaintOzoneScourge(true, progress, warningVisible, status);
        }

        // ---- Memory Flash Vignette ------------------------------------

        private void PumpMemoryFlash(DiegeticHudView view)
        {
            bool flashing = _memoryFlash != null && _memoryFlash.IsFlashing;
            view.PaintMemoryFlash(flashing);
        }

        // ---- Generational Psychology Readout --------------------------

        private void PumpGenerational(DiegeticHudView view)
        {
            if (_genSystem == null || _getSurvivors == null)
            {
                if (_genPanelEverOpened) view.PaintGenerationalReadout(false, 0, 0, string.Empty, string.Empty);
                return;
            }

            var survivors = _getSurvivors();
            int preWar     = 0;
            int bunkerBorn = 0;

            if (survivors != null)
            {
                foreach (var s in survivors)
                {
                    if (!s.IsAlive) continue;
                    if (s.IsBunkerBorn) bunkerBorn++;
                    else preWar++;
                }
            }

            // Only open the panel once the first Bunker-Born has come of age.
            bool hasComingOfAge = bunkerBorn > 0;
            if (!hasComingOfAge && !_genPanelEverOpened)
            {
                view.PaintGenerationalReadout(false, preWar, 0, string.Empty, string.Empty);
                return;
            }
            _genPanelEverOpened = true;

            // Rebuild body text only when counts change (avoids alloc every tick).
            if (preWar != _lastPreWar || bunkerBorn != _lastBunkerBorn)
            {
                _lastPreWar     = preWar;
                _lastBunkerBorn = bunkerBorn;
                int total = preWar + bunkerBorn;
                float bbRatio = total > 0 ? (float)bunkerBorn / total : 0f;

                _genSb.Clear();
                _genSb.AppendLine($"PRE-WAR      {preWar,3} [{BuildBarAscii(1f - bbRatio, 16)}]");
                _genSb.Append    ($"BUNKER-BORN  {bunkerBorn,3} [{BuildBarAscii(bbRatio, 16)}]");
                _lastGenBody = _genSb.ToString();
            }

            // Latest coming-of-age event from the system.
            string eventLine = _genSystem.LastComingOfAgeEvent ?? string.Empty;
            if (!string.Equals(eventLine, _lastGenEvent, StringComparison.Ordinal))
                _lastGenEvent = eventLine;

            view.PaintGenerationalReadout(true, preWar, bunkerBorn, _lastGenBody, _lastGenEvent);
        }

        // ----------------------------------------------------------------
        // Helpers
        // ----------------------------------------------------------------

        private static string BuildBarAscii(float ratio, int width)
        {
            int fill = Mathf.RoundToInt(Mathf.Clamp01(ratio) * width);
            return new string('█', fill) + new string('░', width - fill);
        }
    }
}
