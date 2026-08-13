using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashfall.Bridge
{
    /// <summary>
    /// Drives one coroutine's <see cref="IEnumerator"/>, honouring the yield instructions Unity
    /// gameplay code actually uses. The previous shim constructed a <see cref="Coroutine"/> handle
    /// and dropped the iterator without a single MoveNext, so callers believed work was scheduled
    /// that never ran.
    ///
    /// Supported yields: <c>null</c> and <see cref="WaitForEndOfFrame"/> (resume next frame),
    /// <see cref="WaitForSeconds"/> (scaled time), a nested <see cref="Coroutine"/> (resume when it
    /// finishes), and a raw nested <see cref="IEnumerator"/> (run to completion inline, as Unity
    /// does). Anything else resumes next frame rather than failing the run.
    /// </summary>
    internal sealed class CoroutineRunner
    {
        private readonly Stack<IEnumerator> _stack = new Stack<IEnumerator>();

        internal MonoBehaviour Owner { get; }
        internal Coroutine Coroutine { get; }

        private float _waitRemaining;
        private Coroutine _waitingOn;

        internal CoroutineRunner(MonoBehaviour owner, Coroutine coroutine)
        {
            Owner = owner;
            Coroutine = coroutine;
            _stack.Push(coroutine.Routine);
        }

        /// <summary>Advance one frame. Returns true when the coroutine has finished.</summary>
        internal bool Step()
        {
            if (Coroutine.IsDone) return true;

            // A destroyed or disabled owner suspends the routine, matching Unity: coroutines do
            // not advance on a disabled behaviour.
            if (Owner != null && !Owner.enabled) return false;

            if (_waitingOn != null)
            {
                if (!_waitingOn.IsDone) return false;
                _waitingOn = null;
            }

            if (_waitRemaining > 0f)
            {
                _waitRemaining -= Time.deltaTime;
                if (_waitRemaining > 0f) return false;
            }

            while (_stack.Count > 0)
            {
                IEnumerator current = _stack.Peek();
                bool moved;
                try
                {
                    moved = current.MoveNext();
                }
                catch (Exception ex)
                {
                    Godot.GD.PushError($"[bridge] coroutine threw and was stopped: {ex}");
                    Coroutine.MarkFinished();
                    return true;
                }

                if (!moved)
                {
                    _stack.Pop();
                    // An inner routine finishing resumes the outer one on the same frame, which
                    // is what `yield return SubRoutine()` means in Unity.
                    continue;
                }

                object yielded = current.Current;
                switch (yielded)
                {
                    case null:
                    case WaitForEndOfFrame _:
                        return false;
                    case WaitForSeconds wait:
                        _waitRemaining = wait.Seconds;
                        return false;
                    case Coroutine nested:
                        if (nested.IsDone) continue;
                        _waitingOn = nested;
                        return false;
                    case IEnumerator inner:
                        _stack.Push(inner);
                        continue;
                    default:
                        // Unknown YieldInstruction (WaitForFixedUpdate, custom types). Treating it
                        // as "resume next frame" keeps the routine progressing instead of
                        // stalling it forever.
                        return false;
                }
            }

            Coroutine.MarkFinished();
            return true;
        }
    }
}
