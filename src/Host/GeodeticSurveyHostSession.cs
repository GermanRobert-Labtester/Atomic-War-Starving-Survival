using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot adapter for the Core geodetic survey authority (Plan 79).
    /// Presentation (GeodeticSurveyPanel) is a Wave 6 google-stitch deliverable —
    /// see the "Missing UI panels" registry in AGENTS.md.
    /// </summary>
    public sealed class GeodeticSurveyHostSession : HostSessionBase
    {
        public GeodeticSurveyEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public GeodeticSurveyHostSession(GeodeticSurveyEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnMonumentEstablished += m =>
            {
                LastEvent = $"Datum monument established: {m.surveyPointId}.";
                RaiseStateChanged();
            };
            System.OnTriangleResolved += t =>
            {
                LastEvent = $"Survey triangle resolved (accuracy {t.accuracy:F2}).";
                RaiseStateChanged();
            };
            System.OnShortcutUnlocked += routeId =>
            {
                LastEvent = $"Surveyed route unlocked: {routeId}.";
                RaiseStateChanged();
            };
            System.OnSurveyChanged += () => { RaiseStateChanged(); };
        }

        public ActionResult EstablishMonument(string surveyPointId, int day, Func<string, int, bool> consumeItems)
        {
            var res = System.EstablishMonument(surveyPointId, day, consumeItems);
            if (res.IsFailure) LastEvent = "Monument establishment blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public override void Save()
        {
            if (!IsDirty) return;
            GeodeticSurveySaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
