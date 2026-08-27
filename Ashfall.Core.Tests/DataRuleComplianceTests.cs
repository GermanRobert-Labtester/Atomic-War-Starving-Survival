using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Data-authority rule gate (AGENTS H10 / "no real countries/wars/people").
    /// Scans every top-level JSON catalog under Assets/StreamingAssets/Data and
    /// fails the build if a real-world country, alliance, or regime name leaks in.
    /// The game world is fictional; real geopolitical references are a tone and
    /// content violation, not just a naming nit.
    /// </summary>
    public class DataRuleComplianceTests
    {
        private const string RealCountryPattern =
            @"\b(China|Soviet|Russia|Russian|America|United States|USSR|Britain|British|Germany|" +
            @"France|Japan|India|Pakistan|Iran|Iraq|Israel|Korea|Vietnam|Afghanistan|" +
            @"Italy|Spain|Poland|Ukraine|Canada|Australia|Brazil|Mexico|Egypt|Turkey|" +
            @"NATO|Warsaw Pact|European Union|ASEAN|PRC|USA)\b";

        private static string FindDataDir()
        {
            string dataDir = string.Empty;
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) { dataDir = candidate; break; }
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return dataDir;
        }

        [Fact]
        public void NoCatalogReferencesRealWorldCountriesOrAlliances()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return; // data absent in this environment

            var offenders = new System.Collections.Generic.List<string>();
            foreach (var file in Directory.GetFiles(dataDir, "*.json"))
            {
                if (!File.Exists(file) || file.EndsWith("_tmp.json")) continue;
                string raw = File.ReadAllText(file);
                if (Regex.IsMatch(raw, RealCountryPattern, RegexOptions.IgnoreCase))
                    offenders.Add(Path.GetFileName(file));
            }

            Assert.True(offenders.Count == 0,
                "Real-world country/alliance references found in: " + string.Join(", ", offenders));
        }
    }
}
