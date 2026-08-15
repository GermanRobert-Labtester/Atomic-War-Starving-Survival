using UnityEditor;
using UnityEngine;
using AtomicWar._Game.Survivors;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// One-off editor tool that hand-authors the six BeliefProfileSO assets (one per
    /// RiskBiasTrait). The trait set is closed, unlike open content like Items/Recipes/
    /// Events, so these are not part of JsonDataImporter's JSON pipeline.
    /// </summary>
    public static class CreateDefaultBeliefProfiles
    {
        private const string OutputFolder = "Assets/_Game/Data/Generated/BeliefProfiles";

        [MenuItem("Tools/ASHFALL/Create Default Belief Profiles")]
        public static void CreateAll()
        {
            EnsureFolder(OutputFolder);

            CreateOrUpdate(RiskBiasTrait.Paranoid, 0.15f, 0.05f, 1.8f, 0.3f, 0.2f, 0.15f, 0.01f, 0.05f, 0.35f, AnimationCurve.Linear(0f, 0f, 1f, 1f));
            CreateOrUpdate(RiskBiasTrait.Cautious, 0.15f, 0.08f, 1.3f, 0.6f, 0.35f, 0.08f, 0.015f, 0.1f, 0.7f, AnimationCurve.Linear(0f, 0f, 1f, 0.8f));
            CreateOrUpdate(RiskBiasTrait.Realist, 0.15f, 0.1f, 1f, 1f, 0.5f, 0.05f, 0.02f, 0.3f, 1f, AnimationCurve.Linear(0f, 0f, 1f, 0.6f));
            CreateOrUpdate(RiskBiasTrait.Reckless, 0.15f, 0.15f, 0.5f, 1.8f, 0.6f, 0.03f, 0.05f, 0.6f, 1.6f, AnimationCurve.Linear(0f, 0f, 1f, 0.3f));
            CreateOrUpdate(RiskBiasTrait.Denialist, 0.15f, 0.2f, 0.3f, 2f, 0.1f, 0.02f, 0.06f, 0.7f, 1.7f, AnimationCurve.Linear(0f, 0f, 1f, 0.1f));
            CreateOrUpdate(RiskBiasTrait.Fatalist, 0.1f, 0.1f, 0.8f, 1.2f, 0.4f, 0.02f, 0.04f, 0.5f, 1.1f, AnimationCurve.Linear(0f, 0f, 1f, 0.5f));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateOrUpdate(
            RiskBiasTrait trait, float experienceGainRate, float experienceDecayRate,
            float sicknessObservedGainMultiplier, float survivedHotTripOverconfidenceMultiplier,
            float uncertaintyDampens, float anxietyGainRate, float numbnessGainRate,
            float numbnessProneness, float riskBiasFactor, AnimationCurve scavengeUncertaintyCurve)
        {
            string path = $"{OutputFolder}/{trait}.asset";
            var profile = AssetDatabase.LoadAssetAtPath<BeliefProfileSO>(path);
            bool isNew = profile == null;
            if (isNew)
            {
                profile = ScriptableObject.CreateInstance<BeliefProfileSO>();
            }

            profile.Trait = trait;
            profile.ExperienceGainRate = experienceGainRate;
            profile.ExperienceDecayRate = experienceDecayRate;
            profile.SicknessObservedGainMultiplier = sicknessObservedGainMultiplier;
            profile.SurvivedHotTripOverconfidenceMultiplier = survivedHotTripOverconfidenceMultiplier;
            profile.UncertaintyDampens = uncertaintyDampens;
            profile.AnxietyGainRate = anxietyGainRate;
            profile.NumbnessGainRate = numbnessGainRate;
            profile.NumbnessProneness = numbnessProneness;
            profile.RiskBiasFactor = riskBiasFactor;
            profile.ScavengeUncertaintyCurve = scavengeUncertaintyCurve;

            if (isNew)
            {
                AssetDatabase.CreateAsset(profile, path);
            }
            else
            {
                EditorUtility.SetDirty(profile);
            }
        }

        private static void EnsureFolder(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder)) return;

            string[] parts = folder.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }
                current = next;
            }
        }
    }
}
