using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for LibraryStudySystem.
    /// Wraps the Core library pipeline (LoadCatalog → StartStudy → TickDay)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class LibraryStudyHostSession
    {
        public LibraryStudySystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public LibraryStudyHostSession(
            LibraryStudySystem system,
            SkillProgressionSystem skills,
            ResearchSystem research,
            JournalSystem journal,
            DutyRosterSystem roster)
        {
            System = system
                ?? new LibraryStudySystem(skills, research, journal, roster, new GodotLog());

            System.OnJobCompleted += _ =>
            {
                LastEvent = "Study completed";
                StateChanged?.Invoke();
            };
            System.OnLibraryChanged += () => StateChanged?.Invoke();
        }

        public void LoadCatalog(List<ManualDefinition> manuals)
        {
            System.LoadCatalog(manuals);
            LastEvent = $"Library catalog loaded: {manuals.Count} manuals";
            StateChanged?.Invoke();
        }

        public ActionResult StartStudy(string manualId, string readerId)
        {
            var res = System.StartStudy(manualId, readerId);
            if (res.IsSuccess)
            {
                LastEvent = $"Study started: {manualId} by {readerId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            StateChanged?.Invoke();
        }
    }

    [Serializable]
    public sealed class LibraryStudyHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public LibraryStudyState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class LibraryStudySaveStore
    {
        public const string FileName = "library_study_save.json";
        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(LibraryStudyState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new LibraryStudyHostSave { State = state };
                envelope.Checksum = SaveChecksum.Compute(envelope);
                string path = SavePath;
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(path, s_json.Serialize(envelope));
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr("[Library] save failed: " + e.Message);
                return false;
            }
        }

        public static LibraryStudyState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<LibraryStudyHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }
                return s_json.Deserialize<LibraryStudyState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Library] load failed: " + e.Message);
                return null;
            }
        }
    }
}
