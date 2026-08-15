using System;
using Godot;
using UnityEngine;

// Both namespaces are in scope here, and they collide on the very names this test exercises.
using UTexture2D = UnityEngine.Texture2D;
using UColor = UnityEngine.Color;
using UInput = UnityEngine.Input;
using UTime = UnityEngine.Time;
using UMathf = UnityEngine.Mathf;
using URandom = UnityEngine.Random;

namespace Ashfall.Bridge
{
    /// <summary>
    /// Headless self-test for the shim's failure policy. Runs when the game is launched with
    /// `--bridge-selftest` (after `--`).
    ///
    /// This does not test that the shim is complete — it tests that its <em>incompleteness is
    /// honest</em>: semantic gaps throw, cosmetic gaps stay quiet, and members that are correctly
    /// inert keep answering. Without this, a later "cleanup" could quietly turn a throwing member
    /// back into a plausible default and nothing would notice.
    /// </summary>
    public static class BridgeSelfTest
    {
        public static int Run()
        {
            int passed = 0;
            int total = 0;

            void Check(bool condition, string name)
            {
                total++;
                if (condition)
                {
                    passed++;
                    GD.Print($"  [PASS] {name}");
                }
                else
                {
                    GD.Print($"  [FAIL] {name}");
                }
            }

            bool Throws(Action action)
            {
                try
                {
                    action();
                    return false;
                }
                catch (NotImplementedException)
                {
                    return true;
                }
            }

            GD.Print("[BridgeSelfTest] begin");
            BridgeGap.ThrowOnSemanticGap = true;
            BridgeGap.ResetReported();

            // --- Semantic gaps must throw ---------------------------------------------------
            Check(Throws(() => UnityEngine.Object.Instantiate(new TextAsset("x"))), "Instantiate throws");
            Check(Throws(() => PlayerPrefs.Save()), "PlayerPrefs.Save throws");
            Check(Throws(() => { var _ = Camera.main; }), "Camera.main throws");
            Check(Throws(() => new UTexture2D(1, 1).EncodeToPNG()), "EncodeToPNG throws");
            Check(Throws(() => new UTexture2D(1, 1).GetPixels()), "GetPixels throws");

            // --- Cosmetic gaps must NOT throw -----------------------------------------------
            var source = new AudioSource();
            Check(!Throws(() => source.Play()), "AudioSource.Play is quiet");
            Check(!Throws(() => source.PlayOneShot(new AudioClip())), "PlayOneShot is quiet");
            Check(!Throws(() => new UTexture2D(1, 1).SetPixel(0, 0, UColor.black)), "SetPixel is quiet");
            Check(!Throws(() => new UTexture2D(1, 1).Apply()), "Texture2D.Apply is quiet");

            // --- Correctly inert members keep answering -------------------------------------
            Check(!UInput.GetKey(KeyCode.A), "Input.GetKey answers false");
            Check(!Application.isEditor, "Application.isEditor answers false");
            Check(!Throws(() => UnityEngine.Object.Destroy(new TextAsset("x"))), "Destroy is a no-op");
            Check(!Throws(() => UnityEngine.Object.DontDestroyOnLoad(new TextAsset("x"))), "DontDestroyOnLoad is a no-op");

            // --- Cosmetic hits are reported exactly once ------------------------------------
            BridgeGap.ResetReported();
            source.Play();
            source.Play();
            int afterTwoPlays = CountReported("AudioSource.Play");
            Check(afterTwoPlays == 1, "cosmetic gap reported once, not per call");

            // --- Suppression collects instead of throwing -----------------------------------
            BridgeGap.ResetReported();
            BridgeGap.ThrowOnSemanticGap = false;
            bool threw = Throws(() => PlayerPrefs.Save());
            Check(!threw, "suppressed semantic gap does not throw");
            Check(CountReported("PlayerPrefs.Save") == 1, "suppressed semantic gap is recorded");
            BridgeGap.ThrowOnSemanticGap = true;

            // --- H1: lifecycle hooks actually dispatch --------------------------------------
            BridgeRuntime.ResetForTests();
            var probe = new LifecycleProbe();
            BridgeRuntime.Tick(0.5f);
            Check(probe.Awakes == 1, "Awake fires once");
            Check(probe.Enables == 1, "OnEnable fires once");
            Check(probe.Starts == 1, "Start fires once");
            Check(probe.Updates == 1 && probe.LateUpdates == 1, "Update + LateUpdate fire");

            BridgeRuntime.Tick(0.5f);
            Check(probe.Awakes == 1 && probe.Starts == 1, "Awake/Start do not re-fire");
            Check(probe.Updates == 2, "Update fires per tick");

            probe.enabled = false;
            Check(probe.Disables == 1, "OnDisable fires on disable");
            BridgeRuntime.Tick(0.5f);
            Check(probe.Updates == 2, "disabled behaviour is not updated");
            probe.enabled = true;
            BridgeRuntime.Tick(0.5f);
            Check(probe.Updates == 3 && probe.Enables == 2, "re-enable resumes updates");

            BridgeRuntime.Shutdown();
            Check(probe.Destroys == 1, "OnDestroy fires on shutdown");

            // Private hooks are the norm in _Game; a virtual-override pump would miss them.
            BridgeRuntime.ResetForTests();
            var privateProbe = new PrivateHookProbe();
            BridgeRuntime.Tick(0.1f);
            Check(privateProbe.Ran, "private void Update() is discovered");

            // --- H1: coroutines actually run ------------------------------------------------
            BridgeRuntime.ResetForTests();
            var runner = new CoroutineProbe();
            BridgeRuntime.Tick(0.1f);               // Awake/Start
            Coroutine handle = runner.Begin();
            Check(runner.Stage == 1, "coroutine runs its first segment immediately");
            BridgeRuntime.Tick(0.1f);
            Check(runner.Stage == 2, "coroutine resumes after yield return null");
            BridgeRuntime.Tick(0.1f);               // inside WaitForSeconds(0.25)
            Check(runner.Stage == 2, "WaitForSeconds holds the coroutine");
            BridgeRuntime.Tick(0.2f);               // 0.1 + 0.2 >= 0.25
            Check(runner.Stage == 3, "WaitForSeconds releases after the interval");
            BridgeRuntime.Tick(0.1f);
            Check(runner.Stage == 4 && handle.IsDone, "nested routine completes and handle finishes");

            BridgeRuntime.ResetForTests();
            var stopProbe = new CoroutineProbe();
            BridgeRuntime.Tick(0.1f);
            stopProbe.Begin();
            stopProbe.StopAllCoroutines();
            BridgeRuntime.Tick(0.1f);
            Check(stopProbe.Stage == 1, "StopAllCoroutines halts the routine");

            // --- M1/M2: Time is driven, and timeScale is honoured ---------------------------
            BridgeRuntime.ResetForTests();
            BridgeRuntime.Tick(0.25f);
            Check(UMathf.Abs(UTime.deltaTime - 0.25f) < 0.0001f, "deltaTime reflects real frame time");
            Check(UMathf.Abs(UTime.unscaledDeltaTime - 0.25f) < 0.0001f, "unscaledDeltaTime tracks raw delta");

            UTime.timeScale = 0f;
            BridgeRuntime.Tick(0.25f);
            Check(UMathf.Abs(UTime.deltaTime) < 0.0001f, "timeScale 0 freezes deltaTime");
            Check(UMathf.Abs(UTime.unscaledDeltaTime - 0.25f) < 0.0001f, "timeScale 0 leaves unscaledDeltaTime alone");

            UTime.timeScale = 2f;
            BridgeRuntime.Tick(0.25f);
            Check(UMathf.Abs(UTime.deltaTime - 0.5f) < 0.0001f, "timeScale 2 doubles deltaTime");
            UTime.timeScale = 1f;

            // --- H2: RNG is seedable and reproducible ---------------------------------------
            URandom.InitState(12345);
            float[] first = { URandom.value, URandom.value, URandom.value };
            URandom.InitState(12345);
            bool reproducible = UMathf.Abs(URandom.value - first[0]) < 0.000001f
                && UMathf.Abs(URandom.value - first[1]) < 0.000001f
                && UMathf.Abs(URandom.value - first[2]) < 0.000001f;
            Check(reproducible, "same seed reproduces the same sequence");

            URandom.InitState(99);
            float other = URandom.value;
            Check(UMathf.Abs(other - first[0]) > 0.000001f, "a different seed diverges");
            Check(URandom.state == 99, "Random.state round-trips the seed");

            GD.Print($"[BridgeSelfTest] result: {passed}/{total} PASS, FAIL count {total - passed}");
            GD.Print(passed == total ? "BRIDGE_SELFTEST PASS" : "BRIDGE_SELFTEST FAIL");
            return passed == total ? 0 : 1;
        }

        /// <summary>Public hooks, counted so ordering and repeat-fire rules can be asserted.</summary>
        private sealed class LifecycleProbe : MonoBehaviour
        {
            public int Awakes, Enables, Starts, Updates, LateUpdates, Disables, Destroys;

            private void Awake() => Awakes++;
            private void OnEnable() => Enables++;
            private void Start() => Starts++;
            private void Update() => Updates++;
            private void LateUpdate() => LateUpdates++;
            private void OnDisable() => Disables++;
            private void OnDestroy() => Destroys++;
        }

        /// <summary>Mirrors how _Game declares hooks: private, non-virtual, found by name only.</summary>
        private sealed class PrivateHookProbe : MonoBehaviour
        {
            public bool Ran;

            private void Update() => Ran = true;
        }

        private sealed class CoroutineProbe : MonoBehaviour
        {
            public int Stage;

            public Coroutine Begin() => StartCoroutine(Sequence());

            private System.Collections.IEnumerator Sequence()
            {
                Stage = 1;
                yield return null;
                Stage = 2;
                yield return new WaitForSeconds(0.25f);
                Stage = 3;
                yield return Inner();
            }

            private System.Collections.IEnumerator Inner()
            {
                yield return null;
                Stage = 4;
            }
        }

        private static int CountReported(string member)
        {
            int count = 0;
            foreach (string entry in BridgeGap.Reported)
            {
                if (entry == member) count++;
            }

            return count;
        }

        private static System.Collections.IEnumerator Empty()
        {
            yield break;
        }
    }
}
