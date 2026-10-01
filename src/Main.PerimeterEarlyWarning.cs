// SPDX-License-Identifier: MIT
// ============================================================================
// Perimeter radar host composition. The Core engine remains the sole contact
// classification authority; the host supplies the canonical weather owner's
// storm/dust fact and a deterministic seeded roll, and exposes the radar draw
// for the power owner to consume.
// ============================================================================

using System;
using Ashfall.Core.Defense;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PerimeterEarlyWarningHostSession? _perimeter;
        private bool _perimeterDirty;
        private Ashfall.Core.ISeededRng? _perimeterRng;

        public PerimeterEarlyWarningHostSession? PerimeterEarlyWarningSession => _perimeter;

        public void SetupPerimeterEarlyWarning()
        {
            if (_perimeter != null) return;
            var saved = PerimeterEarlyWarningSaveStore.TryLoad();
            _perimeter = PerimeterEarlyWarningHostSession.Create(saved);
            _perimeter.StateChanged += () => _perimeterDirty = true;
            _perimeterRng = new Ashfall.Core.SeededRng(unchecked(7951 * 733 + "perimeter_early_warning".Length));
        }

        public void SetPerimeterRadarMode(RadarOperationalMode mode)
        {
            SetupPerimeterEarlyWarning();
            _perimeter!.SetMode(mode);
            _perimeterDirty = true;
        }

        public void CalibratePerimeterRadar(int deltaPermille)
        {
            SetupPerimeterEarlyWarning();
            _perimeter!.CalibrateSensors(deltaPermille);
            _perimeterDirty = true;
        }

        /// <summary>
        /// Runs one deterministic scan sweep. The storm/dust fact is read from
        /// the canonical weather owner; the roll is drawn from the seeded RNG.
        /// </summary>
        public RadarContact? SweepPerimeterRadar(
            string sector, int actualDistanceMeters, bool isHostile, bool isStormOrDust, int currentTick)
        {
            SetupPerimeterEarlyWarning();
            int roll = _perimeterRng?.Next(0, 1001) ?? 0;
            var contact = _perimeter!.ProcessScanSweep(
                sector, actualDistanceMeters, isHostile, isStormOrDust, currentTick, roll);
            _perimeterDirty = true;
            return contact;
        }

        public (int Contacts, int PowerWatts, int RangeMeters) GetPerimeterReadout()
        {
            SetupPerimeterEarlyWarning();
            if (_perimeter == null) return (0, 0, 0);
            return _perimeter.GetReadout();
        }

        public int GetPerimeterRadarPowerDraw()
        {
            SetupPerimeterEarlyWarning();
            return _perimeter?.Engine.PowerDrawWatts ?? 0;
        }

        public void TickPerimeterEarlyWarning(int day)
        {
            SetupPerimeterEarlyWarning();
            if (_perimeter == null) return;
            _perimeterDirty = true;
        }

        public void SavePerimeterEarlyWarning()
        {
            if (_perimeter == null) return;
            var state = _perimeter.CaptureState();
            if (CaptureSection(PerimeterEarlyWarningSaveStore.SectionName, PerimeterEarlyWarningSaveStore.TryCapturePersisted(state)))
                _perimeterDirty = false;
        }

        public void ResetPerimeterEarlyWarning()
        {
            _perimeter = null;
            _perimeterDirty = false;
        }
    }
}
