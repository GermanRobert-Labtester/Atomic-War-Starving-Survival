using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.UtilityAI
{
    /// <summary>One point on a data-driven response curve (replaces AnimationCurve).</summary>
    [Serializable]
    public class CurvePoint
    {
        public float x = 0f;
        public float y = 0f;
    }

    /// <summary>
    /// Piecewise-linear response curve with input clamping (Unity AnimationCurve
    /// parity: Evaluate clamps x to the first/last key; empty curve = identity).
    /// </summary>
    public class ResponseCurve
    {
        private readonly CurvePoint[]? _points;

        public ResponseCurve(CurvePoint[] points)
        {
            if (points == null || points.Length == 0)
            {
                _points = points;
                return;
            }
            // Normalize: interpolation assumes ascending x. A malformed catalog
            // must not silently mis-evaluate (debug-loop defect); sort a copy.
            _points = new CurvePoint[points.Length];
            for (int i = 0; i < points.Length; i++)
                _points[i] = new CurvePoint { x = points[i].x, y = points[i].y };
            System.Array.Sort(_points, (a, b) => a.x.CompareTo(b.x));
        }

        public static readonly ResponseCurve Identity = new ResponseCurve(new[]
        {
            new CurvePoint { x = 0f, y = 0f },
            new CurvePoint { x = 1f, y = 1f }
        });

        public float Evaluate(float x)
        {
            if (_points == null || _points.Length == 0) return x; // identity passthrough (audit A4)
            if (_points.Length == 1) return _points[0].y;
            if (x <= _points[0].x) return _points[0].y;
            if (x >= _points[_points.Length - 1].x) return _points[_points.Length - 1].y;
            for (int i = 1; i < _points.Length; i++)
            {
                if (x <= _points[i].x)
                {
                    var a = _points[i - 1];
                    var b = _points[i];
                    float span = b.x - a.x;
                    if (span <= 1e-6f) return b.y;
                    float t = (x - a.x) / span;
                    return a.y + (b.y - a.y) * t;
                }
            }
            return _points[_points.Length - 1].y;
        }
    }

    /// <summary>
    /// Data-driven Utility AI action definition (the JSON is the authority).
    /// Mirrors the Unity SurvivorAction fields the crossing companions use.
    /// </summary>
    [Serializable]
    public class UtilityActionDef
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string description = string.Empty;
        public float basePriority = 0.1f;
        public float weight = 1.0f;
        public bool isOverrideAction = false;
        public string[] tags = Array.Empty<string>();
        public CurvePoint[] curvePoints = null!; // null/empty = identity
        public float baseScore = 0f;            // EvaluateRaw baseline
        public float fatigueGate = 0f;          // 0 = off; raw = 0 when fatigue exceeds
        public float skillBonusFactor = 0f;     // + skill * factor (clamped)

        private ResponseCurve _curve;

        public float EvaluateRaw(AIActionContext context)
        {
            if (context == null || !context.IsAlive) return 0f;
            if (fatigueGate > 0f && context.Fatigue > fatigueGate) return 0f;

            float score = baseScore;
            if (skillBonusFactor > 0f && context.CraftingSkill > 0f)
                score += context.CraftingSkill * skillBonusFactor;
            return Math.Max(0f, Math.Min(1f, score));
        }

        public bool HasTag(string tag)
        {
            if (string.IsNullOrEmpty(tag) || tags == null) return false;
            for (int i = 0; i < tags.Length; i++)
                if (tags[i] == tag) return true;
            return false;
        }

        public ResponseCurve Curve
        {
            get
            {
                if (_curve == null)
                    _curve = curvePoints != null && curvePoints.Length > 0
                        ? new ResponseCurve(curvePoints)
                        : ResponseCurve.Identity;
                return _curve;
            }
        }
    }

    /// <summary>
    /// Per-call decision context (survivor-agnostic). Needs/traits/flags are
    /// plain values the host fills from its own survivor model (audit: the
    /// Unity AIContext carried the whole Survivor object).
    /// </summary>
    public class AIActionContext
    {
        public string SurvivorId = string.Empty;
        public bool IsAlive = true;
        public float Fatigue = 0f;         // 0..100
        public float CraftingSkill = 0f;   // 0..1
        public bool IsListless = false;
        public bool HasHazmat = false;
        public HashSet<string> Traits = new HashSet<string>(StringComparer.Ordinal);

        public bool HasTrait(string trait) => Traits.Contains(trait);
    }

    /// <summary>Known trait gates and action tags for the veto matrix (audit: Unity quest vetoes).</summary>
    public static class UtilityTags
    {
        public const string TraitCoward = "coward";
        public const string TraitGodComplex = "god_complex";
        public const string TraitPacifist = "pacifist";
        public const string TraitBlind = "blind";
        public const string TraitExCon = "ex_con";
        public const string TraitHitman = "hitman";
        public const string TraitGermaphobe = "germaphobe";

        public const string TagLoudLabor = "loud_labor";
        public const string TagMenialLabor = "menial_labor";
        public const string TagWeapon = "weapon";
        public const string TagGun = "gun";
        public const string TagOrder = "order";
        public const string TagMedicalTriage = "medical_triage";
        public const string TagFarming = "farming";
        public const string TagMedical = "medical";
    }
}
