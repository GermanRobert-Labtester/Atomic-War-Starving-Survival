using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Bridge;

namespace UnityEngine
{
    public class MissingReferenceException : Exception
    {
        public MissingReferenceException() { }
        public MissingReferenceException(string message) : base(message) { }
    }

    // =========================================================================
    // Core Attributes
    // =========================================================================
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class SerializeField : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class HeaderAttribute : Attribute
    {
        public string header { get; }
        public HeaderAttribute(string header) { this.header = header; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class TooltipAttribute : Attribute
    {
        public string tooltip { get; }
        public TooltipAttribute(string tooltip) { this.tooltip = tooltip; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class RangeAttribute : Attribute
    {
        public float min { get; }
        public float max { get; }
        public RangeAttribute(float min, float max) { this.min = min; this.max = max; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CreateAssetMenuAttribute : Attribute
    {
        public string menuName { get; set; } = "";
        public string fileName { get; set; } = "";
        public int order { get; set; } = 0;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class HideInInspector : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class TextAreaAttribute : Attribute
    {
        public int minLines { get; }
        public int maxLines { get; }
        public TextAreaAttribute() : this(3, 3) { }
        public TextAreaAttribute(int minLines, int maxLines) { this.minLines = minLines; this.maxLines = maxLines; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class MinAttribute : Attribute
    {
        public float min { get; }
        public MinAttribute(float min) { this.min = min; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DefaultExecutionOrderAttribute : Attribute
    {
        public int order { get; }
        public DefaultExecutionOrderAttribute(int order) { this.order = order; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RequireComponent : Attribute
    {
        public Type m_Type0 { get; }
        public RequireComponent(Type type) { m_Type0 = type; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ExecuteAlways : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DisallowMultipleComponent : Attribute { }

    public enum FullScreenMode { ExclusiveFullScreen, FullScreenWindow, MaximizedWindow, Windowed }

    public enum HideFlags
    {
        None = 0,
        HideInHierarchy = 1,
        HideInInspector = 2,
        DontSaveInEditor = 4,
        NotEditable = 8,
        DontSaveInBuild = 16,
        DontUnloadUnusedAsset = 32,
        DontSave = 52,
        HideAndDontSave = 61
    }

    // =========================================================================
    // Mathf
    // =========================================================================
    public static class Mathf
    {
        public const float PI = 3.14159274f;
        public const float Infinity = float.PositiveInfinity;
        public const float NegativeInfinity = float.NegativeInfinity;
        public const float Deg2Rad = 0.0174532924f;
        public const float Rad2Deg = 57.29578f;
        public const float Epsilon = 1.401298E-45f;

        public static float Sin(float f) => MathF.Sin(f);
        public static float Cos(float f) => MathF.Cos(f);
        public static float Tan(float f) => MathF.Tan(f);
        public static float Asin(float f) => MathF.Asin(f);
        public static float Acos(float f) => MathF.Acos(f);
        public static float Atan(float f) => MathF.Atan(f);
        public static float Atan2(float y, float x) => MathF.Atan2(y, x);
        public static float Sqrt(float f) => MathF.Sqrt(f);
        public static float Abs(float f) => MathF.Abs(f);
        public static int Abs(int value) => Math.Abs(value);
        public static float Min(float a, float b) => MathF.Min(a, b);
        public static int Min(int a, int b) => Math.Min(a, b);
        public static float Min(params float[] values)
        {
            if (values == null || values.Length == 0) return 0f;
            float min = values[0];
            for (int i = 1; i < values.Length; i++) if (values[i] < min) min = values[i];
            return min;
        }
        public static int Min(params int[] values)
        {
            if (values == null || values.Length == 0) return 0;
            int min = values[0];
            for (int i = 1; i < values.Length; i++) if (values[i] < min) min = values[i];
            return min;
        }
        public static float Max(float a, float b) => MathF.Max(a, b);
        public static int Max(int a, int b) => Math.Max(a, b);
        public static float Max(params float[] values)
        {
            if (values == null || values.Length == 0) return 0f;
            float max = values[0];
            for (int i = 1; i < values.Length; i++) if (values[i] > max) max = values[i];
            return max;
        }
        public static int Max(params int[] values)
        {
            if (values == null || values.Length == 0) return 0;
            int max = values[0];
            for (int i = 1; i < values.Length; i++) if (values[i] > max) max = values[i];
            return max;
        }
        public static float Pow(float f, float p) => MathF.Pow(f, p);
        public static float Exp(float f) => MathF.Exp(f);
        public static float Log(float f, float p) => MathF.Log(f, p);
        public static float Log(float f) => MathF.Log(f);
        public static float Log10(float f) => MathF.Log10(f);
        public static float Ceil(float f) => MathF.Ceiling(f);
        public static float Floor(float f) => MathF.Floor(f);
        public static float Round(float f) => MathF.Round(f);
        public static int CeilToInt(float f) => (int)MathF.Ceiling(f);
        public static int FloorToInt(float f) => (int)MathF.Floor(f);
        public static int RoundToInt(float f) => (int)MathF.Round(f);
        public static float Sign(float f) => f >= 0f ? 1f : -1f;
        public static float Clamp(float value, float min, float max) => Math.Clamp(value, min, max);
        public static int Clamp(int value, int min, int max) => Math.Clamp(value, min, max);
        public static float Clamp01(float value) => Math.Clamp(value, 0f, 1f);
        public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp01(t);
        public static float LerpUnclamped(float a, float b, float t) => a + (b - a) * t;
        public static float InverseLerp(float a, float b, float value)
        {
            if (Math.Abs(a - b) < 1e-6f) return 0f;
            return Clamp01((value - a) / (b - a));
        }
        public static bool Approximately(float a, float b) => MathF.Abs(b - a) < MathF.Max(1e-6f * MathF.Max(MathF.Abs(a), MathF.Abs(b)), 1e-6f);
        public static float MoveTowards(float current, float target, float maxDelta)
        {
            if (MathF.Abs(target - current) <= maxDelta) return target;
            return current + MathF.Sign(target - current) * maxDelta;
        }
    }

    // =========================================================================
    // Debug & Logging
    // =========================================================================
    public static class Debug
    {
        public static void Log(object message) => Godot.GD.Print(message?.ToString() ?? "null");
        public static void Log(object message, object context) => Godot.GD.Print($"[{context}] {message}");
        public static void LogWarning(object message) => Godot.GD.PushWarning(message?.ToString() ?? "null");
        public static void LogWarning(object message, object context) => Godot.GD.PushWarning($"[{context}] {message}");
        public static void LogError(object message) => Godot.GD.PushError(message?.ToString() ?? "null");
        public static void LogError(object message, object context) => Godot.GD.PushError($"[{context}] {message}");
        public static void LogException(Exception exception) => Godot.GD.PrintErr(exception?.ToString());
        public static void Assert(bool condition) { if (!condition) throw new System.Diagnostics.UnreachableException("Assertion failed"); }
        public static void Assert(bool condition, string message) { if (!condition) throw new System.Diagnostics.UnreachableException($"Assertion failed: {message}"); }
    }

    // =========================================================================
    // Random
    // =========================================================================
    /// <summary>
    /// The determinism invariant is "same seed => same simulation in both engines". The previous
    /// shim held a clock-seeded System.Random and exposed no InitState, so that invariant was
    /// unsatisfiable for every UnityEngine.Random call site.
    ///
    /// Note that reproducibility here only extends to this shim's own sequence. Ashfall's core
    /// systems should keep using Ashfall.Core's ISeededRng, which is shared with the Unity host.
    /// </summary>
    public static class Random
    {
        private static System.Random _rng = new System.Random();

        /// <summary>The last seed passed to <see cref="InitState"/>, or null if never seeded.</summary>
        public static int? LastSeed { get; private set; }

        /// <summary>Reseed the generator. Unity's entry point for reproducible runs.</summary>
        public static void InitState(int seed)
        {
            LastSeed = seed;
            _rng = new System.Random(seed);
        }

        /// <summary>
        /// Unity exposes Random.state as an opaque struct. Callers only ever round-trip it, so the
        /// seed stands in for it here.
        /// </summary>
        public static int state
        {
            get => LastSeed ?? 0;
            set => InitState(value);
        }

        public static float value => (float)_rng.NextDouble();

        public static float Range(float min, float max)
        {
            return min + (float)_rng.NextDouble() * (max - min);
        }

        public static int Range(int min, int max)
        {
            if (min >= max) return min;
            return _rng.Next(min, max);
        }

        public static Vector2 insideUnitCircle
        {
            get
            {
                float angle = Range(0f, Mathf.PI * 2f);
                float radius = MathF.Sqrt(value);
                return new Vector2(MathF.Cos(angle) * radius, MathF.Sin(angle) * radius);
            }
        }
    }

    // =========================================================================
    // Color & Math Structures
    // =========================================================================
    [Serializable]
    public struct Color : IEquatable<Color>
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color(float r, float g, float b, float a = 1f)
        {
            this.r = r; this.g = g; this.b = b; this.a = a;
        }

        public static Color red => new Color(1f, 0f, 0f, 1f);
        public static Color green => new Color(0f, 1f, 0f, 1f);
        public static Color blue => new Color(0f, 0f, 1f, 1f);
        public static Color white => new Color(1f, 1f, 1f, 1f);
        public static Color black => new Color(0f, 0f, 0f, 1f);
        public static Color yellow => new Color(1f, 0.92f, 0.016f, 1f);
        public static Color cyan => new Color(0f, 1f, 1f, 1f);
        public static Color magenta => new Color(1f, 0f, 1f, 1f);
        public static Color gray => new Color(0.5f, 0.5f, 0.5f, 1f);
        public static Color grey => gray;
        public static Color clear => new Color(0f, 0f, 0f, 0f);

        public static Color Lerp(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color(
                a.r + (b.r - a.r) * t,
                a.g + (b.g - a.g) * t,
                a.b + (b.b - a.b) * t,
                a.a + (b.a - a.a) * t
            );
        }

        public static Color operator +(Color a, Color b) => new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
        public static Color operator -(Color a, Color b) => new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
        public static Color operator *(Color a, float b) => new Color(a.r * b, a.g * b, a.b * b, a.a * b);
        public static Color operator *(float b, Color a) => a * b;
        public static Color operator *(Color a, Color b) => new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
        public static bool operator ==(Color a, Color b) => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
        public static bool operator !=(Color a, Color b) => !(a == b);

        public bool Equals(Color other) => this == other;
        public override bool Equals(object? obj) => obj is Color c && this == c;
        public override int GetHashCode() => HashCode.Combine(r, g, b, a);
        public override string ToString() => $"RGBA({r:F3}, {g:F3}, {b:F3}, {a:F3})";
    }

    [Serializable]
    public struct Color32
    {
        public byte r; public byte g; public byte b; public byte a;
        public Color32(byte r, byte g, byte b, byte a) { this.r = r; this.g = g; this.b = b; this.a = a; }
        public static implicit operator Color(Color32 c) => new Color(c.r / 255f, c.g / 255f, c.b / 255f, c.a / 255f);
        public static implicit operator Color32(Color c) => new Color32((byte)(Mathf.Clamp01(c.r) * 255f), (byte)(Mathf.Clamp01(c.g) * 255f), (byte)(Mathf.Clamp01(c.b) * 255f), (byte)(Mathf.Clamp01(c.a) * 255f));
    }

    [Serializable]
    public struct Vector2 : IEquatable<Vector2>
    {
        public float x;
        public float y;

        public Vector2(float x, float y) { this.x = x; this.y = y; }

        public static Vector2 zero => new Vector2(0f, 0f);
        public static Vector2 one => new Vector2(1f, 1f);
        public static Vector2 up => new Vector2(0f, 1f);
        public static Vector2 down => new Vector2(0f, -1f);
        public static Vector2 left => new Vector2(-1f, 0f);
        public static Vector2 right => new Vector2(1f, 0f);

        public float magnitude => MathF.Sqrt(x * x + y * y);
        public float sqrMagnitude => x * x + y * y;
        public Vector2 normalized => magnitude > 1e-5f ? this / magnitude : zero;

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator *(Vector2 a, float d) => new Vector2(a.x * d, a.y * d);
        public static Vector2 operator *(float d, Vector2 a) => new Vector2(a.x * d, a.y * d);
        public static Vector2 operator /(Vector2 a, float d) => new Vector2(a.x / d, a.y / d);
        public static bool operator ==(Vector2 a, Vector2 b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

        public static float Distance(Vector2 a, Vector2 b) => (a - b).magnitude;
        public static float Dot(Vector2 a, Vector2 b) => a.x * b.x + a.y * b.y;

        public bool Equals(Vector2 other) => this == other;
        public override bool Equals(object? obj) => obj is Vector2 v && this == v;
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"({x:F1}, {y:F1})";
    }

    [Serializable]
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y) { this.x = x; this.y = y; }

        public static Vector2Int zero => new Vector2Int(0, 0);
        public static Vector2Int one => new Vector2Int(1, 1);
        public static Vector2Int up => new Vector2Int(0, 1);
        public static Vector2Int down => new Vector2Int(0, -1);
        public static Vector2Int left => new Vector2Int(-1, 0);
        public static Vector2Int right => new Vector2Int(1, 0);

        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.x + b.x, a.y + b.y);
        public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.x - b.x, a.y - b.y);
        public static bool operator ==(Vector2Int a, Vector2Int b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Vector2Int a, Vector2Int b) => !(a == b);

        public bool Equals(Vector2Int other) => this == other;
        public override bool Equals(object? obj) => obj is Vector2Int v && this == v;
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => $"({x}, {y})";
    }

    [Serializable]
    public struct Vector3 : IEquatable<Vector3>
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z = 0f) { this.x = x; this.y = y; this.z = z; }

        public static Vector3 zero => new Vector3(0f, 0f, 0f);
        public static Vector3 one => new Vector3(1f, 1f, 1f);
        public static Vector3 up => new Vector3(0f, 1f, 0f);
        public static Vector3 down => new Vector3(0f, -1f, 0f);
        public static Vector3 left => new Vector3(-1f, 0f, 0f);
        public static Vector3 right => new Vector3(1f, 0f, 0f);
        public static Vector3 forward => new Vector3(0f, 0f, 1f);
        public static Vector3 back => new Vector3(0f, 0f, -1f);

        public float magnitude => MathF.Sqrt(x * x + y * y + z * z);
        public float sqrMagnitude => x * x + y * y + z * z;
        public Vector3 normalized => magnitude > 1e-5f ? this / magnitude : zero;

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3 operator *(Vector3 a, float d) => new Vector3(a.x * d, a.y * d, a.z * d);
        public static Vector3 operator *(float d, Vector3 a) => new Vector3(a.x * d, a.y * d, a.z * d);
        public static Vector3 operator /(Vector3 a, float d) => new Vector3(a.x / d, a.y / d, a.z / d);
        public static bool operator ==(Vector3 a, Vector3 b) => a.x == b.x && a.y == b.y && a.z == b.z;
        public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);

        public static implicit operator Vector2(Vector3 v) => new Vector2(v.x, v.y);
        public static implicit operator Vector3(Vector2 v) => new Vector3(v.x, v.y, 0f);

        public static float Distance(Vector3 a, Vector3 b) => (a - b).magnitude;
        public static float Dot(Vector3 a, Vector3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

        public bool Equals(Vector3 other) => this == other;
        public override bool Equals(object? obj) => obj is Vector3 v && this == v;
        public override int GetHashCode() => HashCode.Combine(x, y, z);
        public override string ToString() => $"({x:F1}, {y:F1}, {z:F1})";
    }

    [Serializable]
    public struct Vector3Int : IEquatable<Vector3Int>
    {
        public int x; public int y; public int z;
        public Vector3Int(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3Int zero => new Vector3Int(0, 0, 0);
        public static Vector3Int one => new Vector3Int(1, 1, 1);
        public static bool operator ==(Vector3Int a, Vector3Int b) => a.x == b.x && a.y == b.y && a.z == b.z;
        public static bool operator !=(Vector3Int a, Vector3Int b) => !(a == b);
        public bool Equals(Vector3Int other) => this == other;
        public override bool Equals(object? obj) => obj is Vector3Int v && this == v;
        public override int GetHashCode() => HashCode.Combine(x, y, z);
    }

    [Serializable]
    public struct Rect
    {
        public float x;
        public float y;
        public float width;
        public float height;

        public Rect(float x, float y, float width, float height)
        {
            this.x = x; this.y = y; this.width = width; this.height = height;
        }

        public Rect(Vector2 position, Vector2 size)
        {
            this.x = position.x; this.y = position.y; this.width = size.x; this.height = size.y;
        }

        public float xMin { get => x; set { width += x - value; x = value; } }
        public float yMin { get => y; set { height += y - value; y = value; } }
        public float xMax { get => x + width; set => width = value - x; }
        public float yMax { get => y + height; set => height = value - y; }
        public Vector2 position { get => new Vector2(x, y); set { x = value.x; y = value.y; } }
        public Vector2 size { get => new Vector2(width, height); set { width = value.x; height = value.y; } }

        public bool Contains(Vector2 point) => point.x >= xMin && point.x < xMax && point.y >= yMin && point.y < yMax;
        public bool Overlaps(Rect other) => other.xMax > xMin && other.xMin < xMax && other.yMax > yMin && other.yMin < yMax;
    }

    public struct Bounds
    {
        public Vector3 center { get; set; }
        public Vector3 size { get; set; }
        public Vector3 extents => size * 0.5f;
        public Vector3 min => center - extents;
        public Vector3 max => center + extents;
        public Bounds(Vector3 center, Vector3 size) { this.center = center; this.size = size; }
        public bool Contains(Vector3 point) => point.x >= min.x && point.x <= max.x && point.y >= min.y && point.y <= max.y && point.z >= min.z && point.z <= max.z;
    }

    public class Keyframe
    {
        public float time { get; set; }
        public float value { get; set; }
        public Keyframe(float time, float value) { this.time = time; this.value = value; }
    }

    public class AnimationCurve
    {
        private readonly List<Keyframe> _keys = new();
        public Keyframe[] keys => _keys.ToArray();
        public int length => _keys.Count;

        public AnimationCurve() { }
        public AnimationCurve(params Keyframe[] keys) { if (keys != null) _keys.AddRange(keys); }

        public static AnimationCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
        {
            return new AnimationCurve(new Keyframe(timeStart, valueStart), new Keyframe(timeEnd, valueEnd));
        }

        public static AnimationCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
        {
            return new AnimationCurve(new Keyframe(timeStart, valueStart), new Keyframe(timeEnd, valueEnd));
        }

        public float Evaluate(float time)
        {
            if (_keys.Count == 0) return 0f;
            if (_keys.Count == 1 || time <= _keys[0].time) return _keys[0].value;
            if (time >= _keys[^1].time) return _keys[^1].value;

            for (int i = 0; i < _keys.Count - 1; i++)
            {
                if (time >= _keys[i].time && time <= _keys[i + 1].time)
                {
                    float t = (time - _keys[i].time) / (_keys[i + 1].time - _keys[i].time);
                    return Mathf.Lerp(_keys[i].value, _keys[i + 1].value, t);
                }
            }
            return _keys[^1].value;
        }

        public int AddKey(float time, float value)
        {
            _keys.Add(new Keyframe(time, value));
            _keys.Sort((a, b) => a.time.CompareTo(b.time));
            return _keys.Count - 1;
        }
    }

    // =========================================================================
    // Core Hierarchy: Object, ScriptableObject, Component, GameObject, Camera
    // =========================================================================
    public class Object
    {
        public string name { get; set; } = "";
        public HideFlags hideFlags { get; set; } = HideFlags.None;

        // Genuine no-ops. Shim objects are plain CLR objects owned by the GC, so destruction is
        // whatever the last reference drop does. The one Unity behaviour that is lost is fake-null
        // (`obj == null` becoming true after Destroy); the shim never nulls a reference, so no call
        // site can rely on it. Nothing is leaked and no game logic reads a destroyed-ness flag.
        public static void Destroy(Object? obj) { }
        public static void DestroyImmediate(Object? obj) { }

        // Genuine no-op: nothing is destroyed on a scene load that never happens.
        public static void DontDestroyOnLoad(Object target) { }

        // Returning `original` would alias rather than clone — every mutation of the "copy" would
        // silently write through to the source object. That is worse than not compiling.
        public static T Instantiate<T>(T original) where T : Object =>
            BridgeGap.Semantic<T>("Object.Instantiate<T>", "Cloning would return the original instance, so writes to the copy would corrupt the source.");
        public static T Instantiate<T>(T original, Transform? parent, bool instantiateInWorldSpace = false) where T : Object =>
            BridgeGap.Semantic<T>("Object.Instantiate<T>(parent)", "Cloning would return the original instance, so writes to the copy would corrupt the source.");
        public static Object Instantiate(Object original) =>
            BridgeGap.Semantic<Object>("Object.Instantiate", "Cloning would return the original instance, so writes to the copy would corrupt the source.");
        public static Object Instantiate(Object original, Transform? parent, bool instantiateInWorldSpace = false) => original;

        public static bool operator true(Object? x) => !(x is null);
        public static bool operator false(Object? x) => x is null;
        public static bool operator !(Object? x) => x is null;
        public static implicit operator bool(Object? exists) => !(exists is null);

        public static bool operator ==(Object? x, Object? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return ReferenceEquals(x, y);
        }
        public static bool operator !=(Object? x, Object? y) => !(x == y);
        public override bool Equals(object? obj) => obj is Object o && this == o;
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => string.IsNullOrEmpty(name) ? GetType().Name : name;
    }

    public class ScriptableObject : Object
    {
        public static T CreateInstance<T>() where T : ScriptableObject => Activator.CreateInstance<T>();
        public static ScriptableObject CreateInstance(Type type) => (ScriptableObject)Activator.CreateInstance(type)!;
    }

    public class Component : Object
    {
        public GameObject gameObject { get; internal set; } = null!;
        public Transform transform => gameObject?.transform ?? null!;

        public T? GetComponent<T>() where T : class => gameObject?.GetComponent<T>();
        public T? GetComponentInChildren<T>(bool includeInactive = false) where T : class => gameObject?.GetComponentInChildren<T>(includeInactive);
        public T[] GetComponentsInChildren<T>(bool includeInactive = false) where T : class => gameObject?.GetComponentsInChildren<T>(includeInactive) ?? Array.Empty<T>();
    }

    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public Vector3 localPosition { get; set; }
        public Vector3 localScale { get; set; } = Vector3.one;
        public Transform? parent { get; set; }
        public int childCount => _children.Count;
        private readonly List<Transform> _children = new();

        public Transform GetChild(int index) => _children[index];
        public void SetParent(Transform? p, bool worldPositionStays = true) { parent = p; if (p != null && !_children.Contains(this)) p._children.Add(this); }
    }

    public class GameObject : Object
    {
        public Transform transform { get; }
        public bool activeSelf { get; private set; } = true;
        public bool activeInHierarchy => activeSelf;
        public string tag { get; set; } = "Untagged";
        public int layer { get; set; } = 0;

        private readonly List<Component> _components = new();

        public GameObject()
        {
            transform = new Transform { gameObject = this };
            _components.Add(transform);
        }

        public GameObject(string name) : this()
        {
            this.name = name;
        }

        public void SetActive(bool active) => activeSelf = active;

        public T AddComponent<T>() where T : Component
        {
            var comp = (T)Activator.CreateInstance(typeof(T))!;
            comp.gameObject = this;
            _components.Add(comp);
            return comp;
        }

        public T? GetComponent<T>() where T : class
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T match) return match;
            }
            return null;
        }

        public T? GetComponentInChildren<T>(bool includeInactive = false) where T : class
        {
            return GetComponent<T>();
        }

        public T[] GetComponentsInChildren<T>(bool includeInactive = false) where T : class
        {
            var list = new List<T>();
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T match) list.Add(match);
            }
            return list.ToArray();
        }

        public static GameObject? FindWithTag(string tag) => null;
        public static GameObject[] FindGameObjectsWithTag(string tag) => Array.Empty<GameObject>();
    }

    public class MonoBehaviour : Component
    {
        private bool _enabled = true;

        public MonoBehaviour()
        {
            // Registration is what makes Awake/Start/Update reachable. It does not start
            // anything: hooks fire only once a host calls BridgeRuntime.Tick.
            BridgeRuntime.Register(this);
        }

        public bool enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value) return;
                _enabled = value;
                BridgeRuntime.OnEnabledChanged(this, value);
            }
        }

        public Coroutine StartCoroutine(System.Collections.IEnumerator routine) =>
            BridgeRuntime.StartCoroutine(this, routine);

        public void StopCoroutine(Coroutine routine) => BridgeRuntime.StopCoroutine(this, routine);

        public void StopAllCoroutines() => BridgeRuntime.StopAllCoroutines(this);
    }

    public sealed class Coroutine : YieldInstruction
    {
        public System.Collections.IEnumerator Routine { get; }

        /// <summary>True once the routine has run to completion or been stopped.</summary>
        public bool IsDone { get; private set; }

        public Coroutine(System.Collections.IEnumerator routine) { Routine = routine; }

        internal void MarkFinished() => IsDone = true;
    }

    public class YieldInstruction { }
    public class WaitForSeconds : YieldInstruction
    {
        public float Seconds { get; }
        public WaitForSeconds(float seconds) { Seconds = seconds; }
    }
    public class WaitForEndOfFrame : YieldInstruction { }

    public class Camera : Component
    {
        // Returning null makes every `Camera.main.X` an opaque NullReferenceException at the call
        // site. Naming the gap costs nothing and points at the fix.
        public static Camera? main =>
            BridgeGap.Semantic<Camera?>("Camera.main", "There is no camera registry behind the shim, so this would be an unexplained NullReferenceException at the call site.");
        public RenderTexture? targetTexture { get; set; }
        public Color backgroundColor { get; set; } = Color.black;
        public float orthographicSize { get; set; } = 5f;
    }

    // =========================================================================
    // Assets & Resources
    // =========================================================================
    public class TextAsset : Object
    {
        public string text { get; set; } = "";
        public byte[] bytes { get; set; } = Array.Empty<byte>();
        public TextAsset() { }
        public TextAsset(string text) { this.text = text; }
    }

    public class Texture : Object
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    public enum TextureFormat { RGBA32, ARGB32, RGB24, Alpha8 }
    public enum FilterMode { Point, Bilinear, Trilinear }
    public enum TextureWrapMode { Repeat, Clamp, Mirror }

    public class Texture2D : Texture
    {
        public static Texture2D whiteTexture { get; } = new Texture2D(1, 1);
        public static Texture2D blackTexture { get; } = new Texture2D(1, 1);

        public Texture2D(int width, int height) { this.width = width; this.height = height; }
        public Texture2D(int width, int height, TextureFormat format, bool mipChain) : this(width, height) { }
        public Texture2D(int width, int height, TextureFormat format, bool mipChain, bool linear) : this(width, height) { }

        // Cosmetic: the shim holds no pixel buffer, and a headless host renders nothing.
        public void SetPixel(int x, int y, Color color) => BridgeGap.Cosmetic("Texture2D.SetPixel");
        public void SetPixels(Color[] colors) => BridgeGap.Cosmetic("Texture2D.SetPixels");
        public void Apply() => BridgeGap.Cosmetic("Texture2D.Apply");

        // Semantic: these feed data outward. An empty array reads as a legitimately blank texture,
        // and EncodeToPNG's empty result would be written to disk as a 0-byte "PNG".
        public Color[] GetPixels() =>
            BridgeGap.Semantic<Color[]>("Texture2D.GetPixels", "An empty array is indistinguishable from a genuinely blank texture.");
        public byte[] EncodeToPNG() =>
            BridgeGap.Semantic<byte[]>("Texture2D.EncodeToPNG", "The empty result would be written out as a 0-byte file with a .png extension.");
    }

    public class RenderTexture : Texture
    {
        public static RenderTexture? active { get; set; }
        public bool IsCreated() => true;
        public void Release() { }
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer = 0) => new RenderTexture { width = width, height = height };
        public static void ReleaseTemporary(RenderTexture temp) { }
    }

    public class Sprite : Object
    {
        public Texture2D texture { get; set; } = Texture2D.whiteTexture;
        public Rect rect { get; set; }
        public Vector2 pivot { get; set; }

        public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit = 100f)
        {
            return new Sprite { texture = texture, rect = rect, pivot = pivot };
        }
    }

    public class AudioClip : Object
    {
        public float length { get; set; }
        public int channels { get; set; }
        public int frequency { get; set; }

        public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream)
        {
            return new AudioClip { name = name, length = (float)lengthSamples / frequency, channels = channels, frequency = frequency };
        }

        public void SetData(float[] data, int offsetSamples) => BridgeGap.Cosmetic("AudioClip.SetData");
    }

    public class AudioSource : Component
    {
        public AudioClip? clip { get; set; }
        public float volume { get; set; } = 1f;
        public float pitch { get; set; } = 1f;
        public bool loop { get; set; }
        // Consistent with Play() being a cosmetic no-op: nothing is ever playing.
        public bool isPlaying => false;
        public bool enabled { get; set; } = true;
        public bool playOnAwake { get; set; } = true;
        public float spatialBlend { get; set; } = 0f;

        // Cosmetic: no audio device is wired up. Throwing here would make the headless host
        // unusable for the simulation work it exists to run.
        public void Play() => BridgeGap.Cosmetic("AudioSource.Play");
        public void Stop() => BridgeGap.Cosmetic("AudioSource.Stop");
        public void Pause() => BridgeGap.Cosmetic("AudioSource.Pause");
        public void PlayOneShot(AudioClip clip, float volumeScale = 1f) => BridgeGap.Cosmetic("AudioSource.PlayOneShot");
    }

    public static class AudioListener
    {
        public static float volume { get; set; } = 1f;
        public static bool pause { get; set; } = false;
    }

    public class Font : Object { }

    public static class Resources
    {
        public static T? Load<T>(string path) where T : Object
        {
            string globalPath = Godot.ProjectSettings.GlobalizePath($"res://Assets/Resources/{path}");
            if (!File.Exists(globalPath))
            {
                globalPath = Godot.ProjectSettings.GlobalizePath($"res://Resources/{path}");
            }
            if (File.Exists(globalPath))
            {
                if (typeof(T) == typeof(TextAsset))
                {
                    return new TextAsset(File.ReadAllText(globalPath)) as T;
                }
            }
            return null;
        }

        public static Object? Load(string path) => Load<Object>(path);
        public static T[] LoadAll<T>(string path) where T : Object => Array.Empty<T>();
        public static void UnloadUnusedAssets() => GC.Collect();
    }

    // =========================================================================
    // Time, Application, Screen, PlayerPrefs, JsonUtility
    // =========================================================================
    /// <summary>
    /// Driven by <see cref="Ashfall.Bridge.BridgeRuntime.Tick"/>. Previously deltaTime was the
    /// constant 0.016666f, so every accumulator in game code advanced at a fictional fixed rate
    /// regardless of real frame time, and timeScale was a property nothing read — pausing and
    /// slow-motion silently did nothing.
    /// </summary>
    public static class Time
    {
        private static float s_time;
        private static float s_unscaledTime;
        private static float s_deltaTime = 1f / 60f;
        private static float s_unscaledDeltaTime = 1f / 60f;
        private static float s_fixedDeltaTime = 0.02f;

        public static float time => s_time;
        public static float deltaTime => s_deltaTime;
        public static float unscaledTime => s_unscaledTime;
        public static float unscaledDeltaTime => s_unscaledDeltaTime;
        public static float fixedDeltaTime => s_fixedDeltaTime;
        public static float timeScale { get; set; } = 1f;
        public static int frameCount => Ashfall.Bridge.BridgeRuntime.FrameCount;

        internal static void AdvanceFrame(float unscaledDelta)
        {
            if (unscaledDelta < 0f) unscaledDelta = 0f;
            s_unscaledDeltaTime = unscaledDelta;
            s_unscaledTime += unscaledDelta;

            float scale = timeScale < 0f ? 0f : timeScale;
            s_deltaTime = unscaledDelta * scale;
            s_time += s_deltaTime;
        }

        internal static void SetFixedDelta(float fixedDelta) => s_fixedDeltaTime = fixedDelta;

        internal static void ResetForTests()
        {
            s_time = 0f;
            s_unscaledTime = 0f;
            s_deltaTime = 1f / 60f;
            s_unscaledDeltaTime = 1f / 60f;
            s_fixedDeltaTime = 0.02f;
            timeScale = 1f;
        }
    }

    public static class Application
    {
        public static bool isPlaying => true;
        // NOT a gap: this genuinely is a built player, not the Unity editor.
        public static bool isEditor => false;
        public static string companyName => "IndieStudio";
        public static string productName => "Ashfall";
        public static string dataPath => Godot.ProjectSettings.GlobalizePath("res://");
        public static string streamingAssetsPath => Godot.ProjectSettings.GlobalizePath("res://StreamingAssets");
        public static string persistentDataPath => Godot.OS.GetUserDataDir() ?? Godot.ProjectSettings.GlobalizePath("user://");
        public static int targetFrameRate { get; set; } = 60;
        public static void Quit() => Godot.Engine.GetMainLoop();
    }

    public struct Resolution
    {
        public int width { get; set; }
        public int height { get; set; }
        public float refreshRateRatio => 60f;
        public override string ToString() => $"{width}x{height}";
    }

    public static class Screen
    {
        public static int width => 1920;
        public static int height => 1080;
        public static float dpi => 96f;
        public static bool fullScreen { get; set; }
        public static FullScreenMode fullScreenMode { get; set; } = FullScreenMode.Windowed;
        public static Resolution currentResolution => new Resolution { width = 1920, height = 1080 };
        public static Resolution[] resolutions => new[]
        {
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 1600, height = 900 },
            new Resolution { width = 1366, height = 768 },
            new Resolution { width = 1280, height = 720 }
        };

        public static void SetResolution(int width, int height, FullScreenMode fullscreenMode)
        {
            fullScreenMode = fullscreenMode;
        }

        public static void SetResolution(int width, int height, bool fullscreen)
        {
            fullScreen = fullscreen;
        }
    }

    public enum CursorLockMode { None, Locked, Confined }

    public static class Cursor
    {
        public static bool visible { get; set; } = true;
        public static CursorLockMode lockState { get; set; } = CursorLockMode.None;
    }

    public static class PlayerPrefs
    {
        private static readonly Dictionary<string, string> _store = new();

        public static void SetString(string key, string val) => _store[key] = val;
        public static string GetString(string key, string def = "") => _store.TryGetValue(key, out var v) ? v : def;
        public static void SetInt(string key, int val) => _store[key] = val.ToString();
        public static int GetInt(string key, int def = 0) => _store.TryGetValue(key, out var v) && int.TryParse(v, out var i) ? i : def;
        public static void SetFloat(string key, float val) => _store[key] = val.ToString();
        public static float GetFloat(string key, float def = 0f) => _store.TryGetValue(key, out var v) && float.TryParse(v, out var f) ? f : def;
        public static bool HasKey(string key) => _store.ContainsKey(key);
        public static void DeleteKey(string key) => _store.Remove(key);
        public static void DeleteAll() => _store.Clear();
        // The backing store above is a process-lifetime Dictionary. Save() returning quietly would
        // tell the caller its data is durable when it evaporates on exit.
        public static void Save() =>
            BridgeGap.Semantic("PlayerPrefs.Save", "The backing store is in-memory only, so nothing survives process exit.");
    }

    public static class JsonUtility
    {
        private static readonly JsonSerializerOptions _opts = new()
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true
        };

        public static string ToJson(object obj, bool prettyPrint = false)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = prettyPrint, IncludeFields = true });
        }

        public static T FromJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _opts)!;
        }

        public static object? FromJson(string json, Type type)
        {
            return JsonSerializer.Deserialize(json, type, _opts);
        }

        public static void FromJsonOverwrite(string json, object target)
        {
            try
            {
                var doc = JsonDocument.Parse(json);
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    var field = target.GetType().GetField(prop.Name);
                    if (field != null)
                    {
                        if (field.FieldType == typeof(string)) field.SetValue(target, prop.Value.GetString());
                        else if (field.FieldType == typeof(int)) field.SetValue(target, prop.Value.GetInt32());
                        else if (field.FieldType == typeof(float)) field.SetValue(target, (float)prop.Value.GetDouble());
                        else if (field.FieldType == typeof(bool)) field.SetValue(target, prop.Value.GetBoolean());
                    }
                }
            }
            catch { }
        }
    }
}
