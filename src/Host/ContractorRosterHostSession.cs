using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ContractorRosterSystem.
    /// Wraps the Core contractor pipeline (GenerateOffer → AcceptOffer → Dismiss → TickDay)
    /// and forwards StateChanged for host wiring. Engine-agnostic Core authority.
    /// </summary>
    public sealed class ContractorRosterHostSession
    {
        public ContractorRosterSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public ContractorRosterHostSession(
            ContractorRosterSystem system,
            Inventory inventory,
            DutyRosterSystem roster,
            ExpeditionSystem expeditions)
        {
            System = system
                ?? new ContractorRosterSystem(new SeededRng(1986), inventory, roster, expeditions, new GodotLog());

            System.OnContractorStatusChanged += contractor =>
            {
                LastEvent = $"Contractor status changed: {contractor.contractorId}";
                StateChanged?.Invoke();
            };
            System.OnOfferStatusChanged += offer =>
            {
                LastEvent = $"Offer status changed: {offer.offerId}";
                StateChanged?.Invoke();
            };
            System.OnRosterChanged += () => StateChanged?.Invoke();
        }

        public ActionResult GenerateOffer(string candidateId, string role, List<string> requiredSkills, int initialFee, int dailyPay, int termDays)
        {
            var res = System.GenerateOffer(candidateId, role, requiredSkills, initialFee, dailyPay, termDays);
            if (res.IsSuccess)
            {
                LastEvent = $"Offer generated for {candidateId} ({role})";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult AcceptOffer(string offerId)
        {
            var res = System.AcceptOffer(offerId);
            if (res.IsSuccess)
            {
                LastEvent = $"Offer accepted: {offerId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult Dismiss(string contractorId)
        {
            var res = System.Dismiss(contractorId);
            if (res.IsSuccess)
            {
                LastEvent = $"Contractor dismissed: {contractorId}";
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
    public sealed class ContractorRosterHostSave
    {
        public string SchemaVersion { get; set; } = "1.0";
        public ContractorRosterState State { get; set; }
        public string Checksum { get; set; } = string.Empty;
    }

    public static class ContractorRosterSaveStore
    {
        public const string FileName = "contractor_roster_save.json";
        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public static string SavePath =>
            Path.Combine(ProjectSettings.GlobalizePath("user://"), FileName);
        public static bool Exists => s_files.FileExists(SavePath);

        public static bool TrySave(ContractorRosterState state)
        {
            try
            {
                if (state == null) return false;
                var envelope = new ContractorRosterHostSave { State = state };
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
                GD.PrintErr("[Contractor] save failed: " + e.Message);
                return false;
            }
        }

        public static ContractorRosterState? TryLoad()
        {
            try
            {
                string path = SavePath;
                if (!s_files.FileExists(path)) return null;
                string raw = s_files.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var envelope = s_json.Deserialize<ContractorRosterHostSave>(raw);
                if (envelope != null && envelope.State != null)
                {
                    if (string.IsNullOrEmpty(envelope.Checksum)) return null;
                    return envelope.State;
                }
                return s_json.Deserialize<ContractorRosterState>(raw);
            }
            catch (Exception e)
            {
                GD.PrintErr("[Contractor] load failed: " + e.Message);
                return null;
            }
        }
    }
}
