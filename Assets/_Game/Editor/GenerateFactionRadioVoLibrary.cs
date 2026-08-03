#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using AtomicWar._Game.UI;
using AtomicWar._Game.Economy;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Creates / refreshes the FactionRadioVoLibrary asset and assigns WAV
    /// stubs under Assets/_Game/Audio/Radio/. Also patches HUD-like objects
    /// in the open scene that carry RadioInterceptHUD / FactionRadioVoHook.
    /// </summary>
    public static class GenerateFactionRadioVoLibrary
    {
        public const string LibraryPath = "Assets/_Game/Data/Generated/FactionRadioVoLibrary.asset";
        public const string AudioDir = "Assets/_Game/Audio/Radio";

        [MenuItem("ASHFALL/Audio/Generate Faction Radio VO Library")]
        public static void Generate()
        {
            var lib = AssetDatabase.LoadAssetAtPath<FactionRadioVoLibrarySO>(LibraryPath);
            if (lib == null)
            {
                lib = ScriptableObject.CreateInstance<FactionRadioVoLibrarySO>();
                Directory.CreateDirectory(Path.GetDirectoryName(LibraryPath) ?? "Assets");
                AssetDatabase.CreateAsset(lib, LibraryPath);
            }

            lib.DefaultStaticHiss = LoadClip("radio_static_hiss.wav");
            lib.ChannelClips = new[]
            {
                Entry(DynamicEconomySystem.GetParleyChannelTag(FactionSO.Ids.MilitaryRemnants),
                    "vo_ch7_milband.wav"),
                Entry(DynamicEconomySystem.GetParleyChannelTag(FactionSO.Ids.ScavengerCamp),
                    "vo_ch3_ash_road.wav"),
                Entry(DynamicEconomySystem.GetParleyChannelTag(FactionSO.Ids.DoomsdayPreppers),
                    "vo_ch11_stockpile.wav")
            };
            lib.KindClips = new[]
            {
                Kind("Parley", "vo_kind_parley.wav"),
                Kind("HatchRepel", "vo_kind_hatch.wav")
            };

            EditorUtility.SetDirty(lib);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Assign library to any live HUD hooks in open scenes
            int assigned = 0;
            foreach (var hook in Object.FindObjectsByType<FactionRadioVoHook>(
                         FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                hook.SetLibrary(lib);
                EditorUtility.SetDirty(hook);
                assigned++;
            }
            foreach (var strip in Object.FindObjectsByType<RadioInterceptHUD>(
                         FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                strip.VoHook.SetLibrary(lib);
                EditorUtility.SetDirty(strip);
                assigned++;
            }

            Debug.Log($"[ASHFALL] FactionRadioVoLibrary ready at {LibraryPath} " +
                      $"(assigned to {assigned} scene components). WAV stubs in {AudioDir}.");
        }

        private static FactionRadioVoLibrarySO.ChannelEntry Entry(string tag, string file)
        {
            return new FactionRadioVoLibrarySO.ChannelEntry
            {
                ChannelTag = tag,
                Clip = LoadClip(file)
            };
        }

        private static FactionRadioVoLibrarySO.KindEntry Kind(string kind, string file)
        {
            return new FactionRadioVoLibrarySO.KindEntry
            {
                Kind = kind,
                Clip = LoadClip(file)
            };
        }

        private static AudioClip LoadClip(string fileName)
        {
            string path = $"{AudioDir}/{fileName}";
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            if (clip == null)
                Debug.LogWarning($"[ASHFALL] Missing VO stub: {path}");
            return clip;
        }
    }
}
#endif
