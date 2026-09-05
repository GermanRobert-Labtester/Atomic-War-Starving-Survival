using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Definition of a starting bunker survivor from starting_survivors.json (the authority).
    /// </summary>
    [Serializable]
    public class StartingSurvivorDefinition
    {
        public string id { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public float health { get; set; } = 100f;
        public float hunger { get; set; } = 0f;
        public float thirst { get; set; } = 0f;
        public float warmth { get; set; } = 100f;
        public float morale { get; set; } = 50f;
        public float lifetimeDose { get; set; } = 0f;
        public bool acuteRad { get; set; } = false;
        public int joinedDay { get; set; } = 0;
    }

    public enum StartingSurvivorsLoadStatus
    {
        Success,
        MissingFile,
        EmptyFile,
        ParseFailure,
        InvalidRow,
        DuplicateRow,
        OutOfRange,
        UnknownSurvivor
    }

    public sealed class StartingSurvivorsLoadResult
    {
        public StartingSurvivorsLoadStatus Status { get; set; } = StartingSurvivorsLoadStatus.Success;
        public string ErrorMessage { get; set; } = string.Empty;
        public List<StartingSurvivorDefinition> Survivors { get; } = new List<StartingSurvivorDefinition>();
        public int AcceptedRowCount => Survivors.Count;
        public bool IsSuccess => Status == StartingSurvivorsLoadStatus.Success;
    }

    /// <summary>
    /// Shared Core loader for the starting survivor roster and their initial conditions.
    /// Zero engine dependencies; adheres to Invariant 1 and Invariant 6.
    /// </summary>
    public static class SurvivorStartingStateLoader
    {
        public const string FileName = "starting_survivors.json";

        public static List<StartingSurvivorDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var res = LoadDetailed(dataDir, fileIO, serializer);
            return res.IsSuccess ? res.Survivors : new List<StartingSurvivorDefinition>();
        }

        public static StartingSurvivorsLoadResult LoadDetailed(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer,
            SurvivorRosterSystem? masterCatalog = null)
        {
            var result = new StartingSurvivorsLoadResult();
            if (fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
            {
                result.Status = StartingSurvivorsLoadStatus.MissingFile;
                result.ErrorMessage = "fileIO, serializer, or dataDir is null or empty.";
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Status = StartingSurvivorsLoadStatus.MissingFile;
                result.ErrorMessage = $"Authoritative starting survivors file missing: {path}";
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Status = StartingSurvivorsLoadStatus.EmptyFile;
                result.ErrorMessage = $"Authoritative starting survivors file is empty: {path}";
                return result;
            }

            List<StartingSurvivorDefinition> dtos;
            try
            {
                dtos = CatalogLocator.LoadWrappedList<StartingSurvivorDefinition>(raw, SystemTextJsonSerializer.Options);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("SurvivorStartingStateLoader", FileName, ex);
                result.Status = StartingSurvivorsLoadStatus.ParseFailure;
                result.ErrorMessage = $"Failed to parse starting survivors from {path}: {ex.Message}";
                return result;
            }

            if (dtos == null || dtos.Count == 0)
            {
                result.Status = StartingSurvivorsLoadStatus.EmptyFile;
                result.ErrorMessage = $"No survivor entries found in {path}";
                return result;
            }

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                if (dto == null || string.IsNullOrWhiteSpace(dto.id))
                {
                    result.Status = StartingSurvivorsLoadStatus.InvalidRow;
                    result.ErrorMessage = $"Row {i + 1} has null or empty ID in {path}";
                    return result;
                }

                if (!seenIds.Add(dto.id))
                {
                    result.Status = StartingSurvivorsLoadStatus.DuplicateRow;
                    result.ErrorMessage = $"Row {i + 1} has duplicate survivor ID '{dto.id}' in {path}";
                    return result;
                }

                if (dto.health < 0f || dto.health > 100f ||
                    dto.hunger < 0f || dto.hunger > 100f ||
                    dto.thirst < 0f || dto.thirst > 100f ||
                    dto.warmth < 0f || dto.warmth > 100f ||
                    dto.morale < 0f || dto.morale > 100f ||
                    dto.lifetimeDose < 0f || dto.joinedDay < 0)
                {
                    result.Status = StartingSurvivorsLoadStatus.OutOfRange;
                    result.ErrorMessage = $"Row {i + 1} ('{dto.id}') has out-of-range needs or dose values in {path}";
                    return result;
                }

                if (masterCatalog != null && masterCatalog.FindDefinition(dto.id) == null)
                {
                    result.Status = StartingSurvivorsLoadStatus.UnknownSurvivor;
                    result.ErrorMessage = $"Row {i + 1} survivor ID '{dto.id}' not present in master survivor catalog.";
                    return result;
                }

                if (string.IsNullOrEmpty(dto.displayName))
                {
                    dto.displayName = dto.id;
                }

                result.Survivors.Add(dto);
            }

            result.Status = StartingSurvivorsLoadStatus.Success;
            return result;
        }
    }
}
