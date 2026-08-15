using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Ashfall.Bridge
{
    /// <summary>
    /// The lifecycle pump behind the UnityEngine shim: magic-method dispatch, coroutine
    /// scheduling, and the clock that feeds <see cref="UnityEngine.Time"/>.
    ///
    /// Unity does not call <c>Update</c> through a virtual override — gameplay code declares
    /// <c>private void Update()</c> and the engine finds it by name. So this dispatches by
    /// reflection too, cached per type. A base class with virtual methods would silently never
    /// fire for the 158 MonoBehaviours in <c>_Game</c>, which is the failure mode this replaces.
    ///
    /// Host wiring: call <see cref="Tick"/> once per frame with the frame delta, and
    /// <see cref="Shutdown"/> on exit. Until a host calls Tick, nothing is pumped — registration
    /// alone does not start a behaviour.
    /// </summary>
    public static class BridgeRuntime
    {
        private sealed class Hooks
        {
            public MethodInfo Awake;
            public MethodInfo OnEnable;
            public MethodInfo Start;
            public MethodInfo Update;
            public MethodInfo LateUpdate;
            public MethodInfo FixedUpdate;
            public MethodInfo OnDisable;
            public MethodInfo OnDestroy;
        }

        private sealed class Entry
        {
            public WeakReference<MonoBehaviour> Ref;
            public Hooks Hooks;
            public bool Awoken;
            public bool Started;
            public bool EnableFired;
            public bool Destroyed;
        }

        private const BindingFlags HookFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        private static readonly string[] HookNames =
        {
            "Awake", "OnEnable", "Start", "Update", "LateUpdate", "FixedUpdate", "OnDisable", "OnDestroy"
        };

        private static readonly object s_lock = new object();
        private static readonly Dictionary<Type, Hooks> s_hookCache = new Dictionary<Type, Hooks>();
        private static readonly List<Entry> s_entries = new List<Entry>();
        private static readonly List<CoroutineRunner> s_coroutines = new List<CoroutineRunner>();

        // Exceptions thrown from a hook are logged once per (type, hook) and then suppressed.
        // Unity logs and keeps calling; doing the same verbatim would emit 60 identical stack
        // traces a second in a headless run, so the call keeps happening and only the log is
        // deduplicated.
        private static readonly HashSet<string> s_loggedFaults = new HashSet<string>(StringComparer.Ordinal);

        /// <summary>Frames pumped since process start.</summary>
        public static int FrameCount { get; private set; }

        /// <summary>Live registered behaviours. Diagnostic; compacts dead weak references.</summary>
        public static int BehaviourCount
        {
            get { lock (s_lock) { Compact(); return s_entries.Count; } }
        }

        /// <summary>Coroutines currently scheduled.</summary>
        public static int CoroutineCount
        {
            get { lock (s_lock) { return s_coroutines.Count; } }
        }

        internal static void Register(MonoBehaviour behaviour)
        {
            if (behaviour == null) return;
            lock (s_lock)
            {
                s_entries.Add(new Entry
                {
                    Ref = new WeakReference<MonoBehaviour>(behaviour),
                    Hooks = ResolveHooks(behaviour.GetType())
                });
            }
        }

        /// <summary>
        /// Called when <c>enabled</c> flips. Before Awake has run the change is ignored: the
        /// initial activation in <see cref="Tick"/> fires OnEnable in the correct order.
        /// </summary>
        internal static void OnEnabledChanged(MonoBehaviour behaviour, bool value)
        {
            Entry entry = null;
            lock (s_lock)
            {
                entry = Find(behaviour);
            }

            if (entry == null || !entry.Awoken || entry.Destroyed) return;
            if (value && !entry.EnableFired)
            {
                entry.EnableFired = true;
                Invoke(behaviour, entry.Hooks.OnEnable, "OnEnable");
            }
            else if (!value && entry.EnableFired)
            {
                entry.EnableFired = false;
                Invoke(behaviour, entry.Hooks.OnDisable, "OnDisable");
            }
        }

        /// <summary>
        /// Advance one frame. <paramref name="unscaledDelta"/> is the real frame time; the scaled
        /// delta handed to game code is this multiplied by <see cref="UnityEngine.Time.timeScale"/>.
        /// </summary>
        public static void Tick(float unscaledDelta)
        {
            Time.AdvanceFrame(unscaledDelta);
            FrameCount++;

            List<Entry> snapshot;
            lock (s_lock)
            {
                Compact();
                // Iterate a copy: a hook may register or destroy behaviours mid-frame.
                snapshot = new List<Entry>(s_entries);
            }

            // Awake for every new behaviour before any Start, matching Unity's ordering.
            for (int i = 0; i < snapshot.Count; i++)
            {
                Entry entry = snapshot[i];
                if (entry.Awoken || entry.Destroyed) continue;
                if (!entry.Ref.TryGetTarget(out MonoBehaviour behaviour)) continue;
                entry.Awoken = true;
                Invoke(behaviour, entry.Hooks.Awake, "Awake");
            }

            RunPhase(snapshot, PhaseKind.EnableAndStart);
            RunPhase(snapshot, PhaseKind.Update);
            StepCoroutines();
            RunPhase(snapshot, PhaseKind.LateUpdate);
        }

        /// <summary>
        /// Advance the fixed-step hooks. Separate from <see cref="Tick"/> because a host may run
        /// physics on its own cadence; Ashfall's does not, so this is opt-in.
        /// </summary>
        public static void FixedTick(float fixedDelta)
        {
            Time.SetFixedDelta(fixedDelta);

            List<Entry> snapshot;
            lock (s_lock) { Compact(); snapshot = new List<Entry>(s_entries); }
            RunPhase(snapshot, PhaseKind.FixedUpdate);
        }

        private enum PhaseKind { EnableAndStart, Update, LateUpdate, FixedUpdate }

        private static void RunPhase(List<Entry> snapshot, PhaseKind phase)
        {
            for (int i = 0; i < snapshot.Count; i++)
            {
                Entry entry = snapshot[i];
                if (entry.Destroyed || !entry.Awoken) continue;
                if (!entry.Ref.TryGetTarget(out MonoBehaviour behaviour)) continue;
                if (!behaviour.enabled)
                {
                    if (entry.EnableFired)
                    {
                        entry.EnableFired = false;
                        Invoke(behaviour, entry.Hooks.OnDisable, "OnDisable");
                    }

                    continue;
                }

                switch (phase)
                {
                    case PhaseKind.EnableAndStart:
                        if (!entry.EnableFired)
                        {
                            entry.EnableFired = true;
                            Invoke(behaviour, entry.Hooks.OnEnable, "OnEnable");
                        }

                        if (!entry.Started)
                        {
                            entry.Started = true;
                            Invoke(behaviour, entry.Hooks.Start, "Start");
                        }

                        break;
                    case PhaseKind.Update: Invoke(behaviour, entry.Hooks.Update, "Update"); break;
                    case PhaseKind.LateUpdate: Invoke(behaviour, entry.Hooks.LateUpdate, "LateUpdate"); break;
                    case PhaseKind.FixedUpdate: Invoke(behaviour, entry.Hooks.FixedUpdate, "FixedUpdate"); break;
                }
            }
        }

        /// <summary>Fire OnDisable/OnDestroy for everything still live, then clear the registry.</summary>
        public static void Shutdown()
        {
            List<Entry> snapshot;
            lock (s_lock)
            {
                snapshot = new List<Entry>(s_entries);
                s_entries.Clear();
                s_coroutines.Clear();
            }

            for (int i = 0; i < snapshot.Count; i++)
            {
                Entry entry = snapshot[i];
                if (entry.Destroyed || !entry.Ref.TryGetTarget(out MonoBehaviour behaviour)) continue;
                entry.Destroyed = true;
                if (entry.EnableFired)
                {
                    entry.EnableFired = false;
                    Invoke(behaviour, entry.Hooks.OnDisable, "OnDisable");
                }

                if (entry.Awoken) Invoke(behaviour, entry.Hooks.OnDestroy, "OnDestroy");
            }
        }

        /// <summary>Drop all registrations and clock state without firing teardown hooks.</summary>
        public static void ResetForTests()
        {
            lock (s_lock)
            {
                s_entries.Clear();
                s_coroutines.Clear();
                s_loggedFaults.Clear();
            }

            FrameCount = 0;
            Time.ResetForTests();
        }

        // -- coroutines ------------------------------------------------------------------

        internal static Coroutine StartCoroutine(MonoBehaviour owner, IEnumerator routine)
        {
            if (routine == null) throw new ArgumentNullException(nameof(routine));
            var coroutine = new Coroutine(routine);
            var runner = new CoroutineRunner(owner, coroutine);
            lock (s_lock) { s_coroutines.Add(runner); }

            // Unity runs the first segment immediately rather than waiting a frame.
            runner.Step();
            return coroutine;
        }

        internal static void StopCoroutine(MonoBehaviour owner, Coroutine coroutine)
        {
            if (coroutine == null) return;
            lock (s_lock)
            {
                for (int i = s_coroutines.Count - 1; i >= 0; i--)
                {
                    if (s_coroutines[i].Coroutine == coroutine)
                    {
                        s_coroutines[i].Coroutine.MarkFinished();
                        s_coroutines.RemoveAt(i);
                    }
                }
            }
        }

        internal static void StopAllCoroutines(MonoBehaviour owner)
        {
            lock (s_lock)
            {
                for (int i = s_coroutines.Count - 1; i >= 0; i--)
                {
                    if (ReferenceEquals(s_coroutines[i].Owner, owner))
                    {
                        s_coroutines[i].Coroutine.MarkFinished();
                        s_coroutines.RemoveAt(i);
                    }
                }
            }
        }

        private static void StepCoroutines()
        {
            List<CoroutineRunner> snapshot;
            lock (s_lock) { snapshot = new List<CoroutineRunner>(s_coroutines); }

            for (int i = 0; i < snapshot.Count; i++)
            {
                CoroutineRunner runner = snapshot[i];
                if (!runner.Step()) continue;
                lock (s_lock) { s_coroutines.Remove(runner); }
            }
        }

        // -- reflection ------------------------------------------------------------------

        private static Hooks ResolveHooks(Type type)
        {
            lock (s_hookCache)
            {
                if (s_hookCache.TryGetValue(type, out Hooks cached)) return cached;

                var hooks = new Hooks();
                foreach (string name in HookNames)
                {
                    MethodInfo method = FindHook(type, name);
                    switch (name)
                    {
                        case "Awake": hooks.Awake = method; break;
                        case "OnEnable": hooks.OnEnable = method; break;
                        case "Start": hooks.Start = method; break;
                        case "Update": hooks.Update = method; break;
                        case "LateUpdate": hooks.LateUpdate = method; break;
                        case "FixedUpdate": hooks.FixedUpdate = method; break;
                        case "OnDisable": hooks.OnDisable = method; break;
                        case "OnDestroy": hooks.OnDestroy = method; break;
                    }
                }

                s_hookCache[type] = hooks;
                return hooks;
            }
        }

        private static MethodInfo FindHook(Type type, string name)
        {
            // Walk the hierarchy explicitly: BindingFlags.NonPublic does not surface private
            // members of base types, and gameplay code routinely declares these private.
            for (Type current = type; current != null && current != typeof(object); current = current.BaseType)
            {
                MethodInfo method = current.GetMethod(name, HookFlags, null, Type.EmptyTypes, null);
                if (method != null && method.DeclaringType == current && !method.IsAbstract) return method;
            }

            return null;
        }

        private static void Invoke(MonoBehaviour behaviour, MethodInfo method, string hookName)
        {
            if (method == null) return;
            try
            {
                method.Invoke(behaviour, null);
            }
            catch (TargetInvocationException ex)
            {
                LogFaultOnce(behaviour, hookName, ex.InnerException ?? ex);
            }
            catch (Exception ex)
            {
                LogFaultOnce(behaviour, hookName, ex);
            }
        }

        private static void LogFaultOnce(MonoBehaviour behaviour, string hookName, Exception ex)
        {
            string key = behaviour.GetType().FullName + "." + hookName;
            lock (s_lock)
            {
                if (!s_loggedFaults.Add(key)) return;
            }

            Godot.GD.PushError(
                $"[bridge] {key} threw: {ex}. The hook keeps being called, as it would in Unity; " +
                "further exceptions from this hook are not logged.");
        }

        private static Entry Find(MonoBehaviour behaviour)
        {
            for (int i = 0; i < s_entries.Count; i++)
            {
                if (s_entries[i].Ref.TryGetTarget(out MonoBehaviour candidate) && ReferenceEquals(candidate, behaviour))
                    return s_entries[i];
            }

            return null;
        }

        private static void Compact()
        {
            for (int i = s_entries.Count - 1; i >= 0; i--)
            {
                if (!s_entries[i].Ref.TryGetTarget(out _)) s_entries.RemoveAt(i);
            }
        }
    }
}
