// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : BackstorySaveStore
// Core State : Ashfall.Core.Survivors.BackstoryState
// Host Caller: Main.Backstory (SetupBackstory / SaveBackstory)
// Purpose    : Plan 174 — Procedural survivor backstories & origin mechanics:
//              occupations, life experiences, skill bonuses, trait modifiers,
//              and narrative secrets.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class BackstorySaveStore
    {
        public const string FileName = "backstory_save.json";
        public const string SectionName = "backstory";

        private static readonly SaveStore<BackstoryState> s_store =
            SaveStoreHub.Checksummed<BackstoryState>(FileName, nameof(BackstorySaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(BackstoryState state) => s_store.CaptureBare(state);
        public static BackstoryState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(BackstoryState state) => s_store.TrySave(state);
        public static BackstoryState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 174 (Procedural Survivor Backstories & Origin Mechanics).
    /// Binds catalog data from backstory_templates.json, assigns procedural origins to survivors,
    /// evaluates mechanical skill bonuses and starting traits, and persists state across campaigns.
    /// </summary>
    public sealed class BackstoryHostSession : HostSessionBase
    {
        private readonly BackstorySystem _system;
        private string _lastEvent = string.Empty;

        public BackstorySystem System => _system;
        public string LastEvent => _lastEvent;

        public BackstoryCensus Census => _system.GetCensus();
        public IReadOnlyList<SurvivorBackstory> Backstories => _system.GetAllAssignedBackstories();

        public BackstoryHostSession(string? dataDir = null, BackstorySystem? system = null)
        {
            _system = system ?? new BackstorySystem();

            _system.OnBackstoryAssigned += backstory =>
            {
                _lastEvent = $"Assigned backstory for {backstory.SurvivorId}: Occ={backstory.OccupationId}, Tmpl={backstory.TemplateId}";
                RaiseStateChanged();
            };

            _system.OnSecretRevealed += (survivorId, secretId) =>
            {
                _lastEvent = $"Secret revealed for {survivorId}: {secretId}";
                RaiseStateChanged();
            };

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalog(dataDir);
            }
        }

        public static BackstoryHostSession Create(string dataDir, BackstorySystem? system = null)
        {
            return new BackstoryHostSession(dataDir, system);
        }

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            string path = Path.Combine(dataDir, "backstory_templates.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _system.LoadCatalog(json);
                _lastEvent = $"Loaded {_system.GetAllOccupations().Count} occupations, {_system.GetAllExperiences().Count} experiences, {_system.GetAllTemplates().Count} templates.";
                RaiseStateChanged();
            }
        }

        public SurvivorBackstory AssignFromTemplate(string survivorId, string templateId, int day)
        {
            return _system.AssignFromTemplate(survivorId, templateId, day);
        }

        public SurvivorBackstory AssignCustom(
            string survivorId,
            string occupationId,
            IEnumerable<string> experienceIds,
            string preWarLife,
            string definingMoment,
            string reasonForSurvival,
            int day)
        {
            return _system.AssignCustom(survivorId, occupationId, experienceIds, preWarLife, definingMoment, reasonForSurvival, day);
        }

        public SurvivorBackstory? GetBackstory(string survivorId)
        {
            return _system.GetBackstory(survivorId);
        }

        public BackstoryProjection ProjectEffects(string survivorId)
        {
            return _system.ProjectEffects(survivorId);
        }

        public bool RevealSecret(string survivorId, string secretId)
        {
            return _system.RevealSecret(survivorId, secretId);
        }

        public BackstoryState CaptureState() => _system.CaptureState();

        public void RestoreState(BackstoryState? state) => _system.RestoreState(state);
    }
}
