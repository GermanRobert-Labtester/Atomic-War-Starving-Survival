using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Journal
{
    [Serializable]
    public class JournalVoiceProseEntry
    {
        public string paranoid = string.Empty;
        public string cautious = string.Empty;
        public string realist = string.Empty;
        public string reckless = string.Empty;
        public string denialist = string.Empty;
        public string fatalist = string.Empty;
        public string empath = string.Empty;
        public string sociopath = string.Empty;
        public string @default = string.Empty;

        public string GetProseForBias(RiskBiasTrait bias)
        {
            return bias switch
            {
                RiskBiasTrait.Paranoid => paranoid,
                RiskBiasTrait.Cautious => cautious,
                RiskBiasTrait.Realist => realist,
                RiskBiasTrait.Reckless => reckless,
                RiskBiasTrait.Denialist => denialist,
                RiskBiasTrait.Fatalist => fatalist,
                RiskBiasTrait.Empath => empath,
                RiskBiasTrait.Sociopath => sociopath,
                _ => @default
            };
        }

        public bool HasVariantForBias(RiskBiasTrait bias)
        {
            return bias switch
            {
                RiskBiasTrait.Paranoid => !string.IsNullOrEmpty(paranoid),
                RiskBiasTrait.Cautious => !string.IsNullOrEmpty(cautious),
                RiskBiasTrait.Realist => !string.IsNullOrEmpty(realist),
                RiskBiasTrait.Reckless => !string.IsNullOrEmpty(reckless),
                RiskBiasTrait.Denialist => !string.IsNullOrEmpty(denialist),
                RiskBiasTrait.Fatalist => !string.IsNullOrEmpty(fatalist),
                RiskBiasTrait.Empath => !string.IsNullOrEmpty(empath),
                RiskBiasTrait.Sociopath => !string.IsNullOrEmpty(sociopath),
                _ => !string.IsNullOrEmpty(@default)
            };
        }
    }

    [Serializable]
    public sealed class JournalVoiceProseCatalog
    {
        private readonly IReadOnlyDictionary<string, JournalVoiceProseEntry> _entries;

        public JournalVoiceProseCatalog(IReadOnlyDictionary<string, JournalVoiceProseEntry> entries)
        {
            _entries = entries ?? throw new ArgumentNullException(nameof(entries));
        }

        public JournalVoiceProseEntry? GetEntry(string knowledgeKey)
        {
            if (string.IsNullOrEmpty(knowledgeKey)) return null;
            _entries.TryGetValue(knowledgeKey, out var entry);
            return entry;
        }

        public string GetProse(string knowledgeKey, RiskBiasTrait bias)
        {
            var entry = GetEntry(knowledgeKey);
            if (entry != null)
            {
                string prose = entry.GetProseForBias(bias);
                if (!string.IsNullOrEmpty(prose))
                    return prose;
            }
            return "Something changed. I wrote it down so I would not forget.";
        }

        public bool HasKey(string knowledgeKey)
        {
            return !string.IsNullOrEmpty(knowledgeKey) && _entries.ContainsKey(knowledgeKey);
        }

        public IReadOnlyCollection<string> GetAllKeys() => new List<string>(_entries.Keys);
        public int Count => _entries.Count;
    }

    public sealed class JournalVoiceProseCatalogLoader
    {
        public const string ProseFile = "journal_voice_prose.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;

        public JournalVoiceProseCatalogLoader(IFileIO files, IJsonSerializer json)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
        }

        public JournalVoiceProseCatalog Load(string dataDirectory)
        {
            string path = _files.Combine(dataDirectory, ProseFile);

            if (!_files.FileExists(path))
                return Empty;

            try
            {
                string jsonText = _files.ReadAllText(path);
                var root = _json.Deserialize<JournalVoiceProseRoot>(jsonText);
                return new JournalVoiceProseCatalog(root?.prose_variants ?? new());
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(ProseFile, "<root>", ex);
                return Empty;
            }
        }

        public static JournalVoiceProseCatalog Empty => new JournalVoiceProseCatalog(new Dictionary<string, JournalVoiceProseEntry>());

        public static JournalVoiceProseCatalog LoadDefault(IFileIO? files = null, IJsonSerializer? json = null)
        {
            files ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();
            var loader = new JournalVoiceProseCatalogLoader(files, json);
            if (CatalogLocator.TryFindDataDirectory(Environment.CurrentDirectory, out string dataDir)
                || CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir))
            {
                return loader.Load(dataDir);
            }
            return Empty;
        }
    }

    [Serializable]
    internal class JournalVoiceProseRoot
    {
#pragma warning disable CS0649
        public int schema_version;
        public Dictionary<string, JournalVoiceProseEntry> prose_variants;
#pragma warning restore CS0649
    }
}
