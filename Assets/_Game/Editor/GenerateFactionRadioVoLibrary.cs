#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using AtomicWar._Game.UI;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Data;

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
            // Expansion II Part II: ensure KindClips is non-null before
            // appending the four faction-pressure rows. (Idempotent.)
            lib.KindClips = lib.KindClips ?? new FactionRadioVoLibrarySO.KindEntry[0];

            // Existing kinds + 4 new pressure kinds.
            lib.KindClips = AppendKind(lib.KindClips, Kind("Parley", "vo_kind_parley.wav"));
            lib.KindClips = AppendKind(lib.KindClips, Kind("HatchRepel", "vo_kind_hatch.wav"));
            lib.KindClips = AppendKind(lib.KindClips, Kind("FactionPressure", "vo_kind_faction_pressure.wav"));
            lib.KindClips = AppendKind(lib.KindClips, Kind("LedgerStrike", "vo_kind_ledger_strike.wav"));
            lib.KindClips = AppendKind(lib.KindClips, Kind("TaxChange", "vo_kind_tax_change.wav"));
            lib.KindClips = AppendKind(lib.KindClips, Kind("CultCommunion", "vo_kind_cult_communion.wav"));

            EditorUtility.SetDirty(lib);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Assign library to any live HUD hooks in open scenes
            int assigned = 0;
            foreach (var hook in Object.FindObjectsByType<FactionRadioVoHook>(
                         FindObjectsInactive.Include))
            {
                hook.SetLibrary(lib);
                EditorUtility.SetDirty(hook);
                assigned++;
            }
            foreach (var strip in Object.FindObjectsByType<RadioInterceptHUD>(
                         FindObjectsInactive.Include))
            {
                strip.VoHook.SetLibrary(lib);
                EditorUtility.SetDirty(strip);
                assigned++;
            }

            // Print all faction lore lines to the Console so designers
            // can audit the voice catalog at a glance. (Text only.)
            foreach (var fid in new[]
                     {
                         FactionSO.Ids.MilitaryRemnants,
                         FactionSO.Ids.UplandMilitia,
                         FactionSO.Ids.CultOfTheGlow,
                         FactionSO.Ids.ScavengerCamp
                     })
            {
                AddLoreLinesAsKindComments(fid);
            }

            Debug.Log($"[ASHFALL] FactionRadioVoLibrary ready at {LibraryPath} " +
                      $"(assigned to {assigned} scene components). WAV stubs in {AudioDir}.");
        }

        private static FactionRadioVoLibrarySO.KindEntry[] AppendKind(
            FactionRadioVoLibrarySO.KindEntry[] existing,
            FactionRadioVoLibrarySO.KindEntry next)
        {
            if (next == null) return existing;
            if (existing != null)
            {
                for (int i = 0; i < existing.Length; i++)
                {
                    var e = existing[i];
                    if (e != null && string.Equals(e.Kind, next.Kind, System.StringComparison.OrdinalIgnoreCase))
                        return existing;
                }
            }
            int n = existing != null ? existing.Length : 0;
            var grown = new FactionRadioVoLibrarySO.KindEntry[n + 1];
            if (n > 0) System.Array.Copy(existing, grown, n);
            grown[n] = next;
            return grown;
        }

        /// <summary>
        /// Print the lore lines for a given faction id to the Unity Console.
        /// Designers use this when auditing the voice catalog; the lines
        /// don't need to land in the SO.
        /// </summary>
        public static void AddLoreLinesAsKindComments(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return;
            var lore = FactionLoreVoiceLines.LinesForFaction(factionId);
            if (lore == null || lore.Count == 0)
            {
                Debug.Log($"[FactionLore] No lines catalogued for {factionId}.");
                return;
            }
            var sb = new StringBuilder();
            sb.Append("[FactionLore] ").Append(factionId).Append(" (").Append(lore.Count).Append(" lines):\n");
            for (int i = 0; i < lore.Count; i++)
            {
                sb.Append("  ").Append(i + 1).Append(". ").Append(lore[i]).Append("\n");
            }
            Debug.Log(sb.ToString());
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
