// SPDX-License-Identifier: MIT
// Wave 11 B3 — host persistence for the user-level completion-history authority.

using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp.Host
{
    /// <summary>
    /// Atomic user-level persistence adapter for append-only completion records.
    /// It never reads or writes a campaign save section: Main supplies only a stable
    /// campaign identity and frozen epilogue facts after the ending authority seals.
    /// </summary>
    public sealed class CompletionHistoryStore
    {
        public const string DefaultPath = "user://completion_history.json";

        private readonly string _path;
        private CampaignCompletionHistory _history;

        private CompletionHistoryStore(string path, CampaignCompletionHistory history, string? diagnosticMessage)
        {
            _path = path;
            _history = history;
            DiagnosticMessage = diagnosticMessage;
        }

        public string? DiagnosticMessage { get; private set; }
        public bool IsValid => string.IsNullOrEmpty(DiagnosticMessage);

        /// <summary>
        /// Returns a detached record list so presentation cannot mutate stored history.
        /// </summary>
        public IReadOnlyList<CampaignCompletionRecord> Records =>
            CampaignCompletionHistoryService.Clone(_history).records.AsReadOnly();

        public static CompletionHistoryStore Load(string path = DefaultPath)
        {
            string globalPath = ProjectSettings.GlobalizePath(path);
            if (!File.Exists(globalPath))
                return new CompletionHistoryStore(path, new CampaignCompletionHistory(), null);

            try
            {
                string json = File.ReadAllText(globalPath);
                if (CampaignCompletionHistoryService.TryDeserialize(json, out CampaignCompletionHistory history, out string error))
                    return new CompletionHistoryStore(path, history, null);

                string diagnostic = "[CompletionHistoryStore] " + error;
                GD.PrintErr(diagnostic);
                return new CompletionHistoryStore(path, new CampaignCompletionHistory(), diagnostic);
            }
            catch (Exception ex)
            {
                string diagnostic = "[CompletionHistoryStore] Failed to read completion history from '" + path + "' (" + ex.Message + ").";
                GD.PrintErr(diagnostic);
                return new CompletionHistoryStore(path, new CampaignCompletionHistory(), diagnostic);
            }
        }

        /// <summary>
        /// Appends one completion only after current history validates. A failed atomic write
        /// restores the in-memory history, so application state never claims an unpersisted append.
        /// </summary>
        public CompletionHistoryAppendResult Append(
            CampaignCompletionObservation observation,
            out CampaignCompletionRecord? appendedRecord)
        {
            appendedRecord = null;
            if (!IsValid)
                return CompletionHistoryAppendResult.InvalidHistory;

            CampaignCompletionHistory before = CampaignCompletionHistoryService.Clone(_history);
            CompletionHistoryAppendResult result = CampaignCompletionHistoryService.Append(_history, observation, out appendedRecord);
            if (result != CompletionHistoryAppendResult.Appended)
                return result;

            if (TryWrite(_history))
                return result;

            _history = before;
            appendedRecord = null;
            return CompletionHistoryAppendResult.InvalidHistory;
        }

        private bool TryWrite(CampaignCompletionHistory history)
        {
            string globalPath = ProjectSettings.GlobalizePath(_path);
            string tempPath = globalPath + ".tmp";
            try
            {
                string? directory = Path.GetDirectoryName(globalPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllText(tempPath, CampaignCompletionHistoryService.Serialize(history));
                if (File.Exists(globalPath))
                    File.Replace(tempPath, globalPath, null);
                else
                    File.Move(tempPath, globalPath);

                DiagnosticMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                DiagnosticMessage = "[CompletionHistoryStore] Failed to persist completion history to '" + _path + "' (" + ex.Message + ").";
                GD.PrintErr(DiagnosticMessage);
                try
                {
                    if (File.Exists(tempPath)) File.Delete(tempPath);
                }
                catch (Exception cleanupEx)
                {
                    GD.PrintErr("[CompletionHistoryStore] Failed to clean temporary history file: " + cleanupEx.Message);
                }
                return false;
            }
        }
    }
}
