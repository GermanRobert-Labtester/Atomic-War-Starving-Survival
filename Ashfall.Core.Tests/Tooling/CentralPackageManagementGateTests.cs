// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Central Package Management (CPM) Gate.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CentralPackageManagementGateTests
    {
        private static string RepoRoot()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    string probeProps = Path.Combine(dir.FullName, "Directory.Packages.props");
                    if (File.Exists(probeProps))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate Directory.Packages.props from test execution context.");
        }

        [Fact]
        public void DirectoryPackagesProps_IsConfiguredAndEnabled()
        {
            string root = RepoRoot();
            string propsPath = Path.Combine(root, "Directory.Packages.props");
            Assert.True(File.Exists(propsPath), "Directory.Packages.props must exist at repository root.");

            var doc = XDocument.Load(propsPath);
            var manageElem = doc.Descendants("ManagePackageVersionsCentrally").FirstOrDefault();
            Assert.NotNull(manageElem);
            Assert.True(string.Equals(manageElem!.Value.Trim(), "true", StringComparison.OrdinalIgnoreCase),
                "ManagePackageVersionsCentrally must be set to 'true'.");

            var packageVersions = doc.Descendants("PackageVersion")
                .Select(e => e.Attribute("Include")?.Value)
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();

            Assert.True(packageVersions.Count >= 5,
                $"Expected at least 5 centrally managed packages, found {packageVersions.Count}.");
        }

        [Fact]
        public void ProjectFiles_HaveZeroInlinePackageReferenceVersions()
        {
            string root = RepoRoot();
            string propsPath = Path.Combine(root, "Directory.Packages.props");
            var propsDoc = XDocument.Load(propsPath);

            var declaredPackages = new HashSet<string>(
                propsDoc.Descendants("PackageVersion")
                    .Select(e => e.Attribute("Include")?.Value ?? "")
                    .Where(s => !string.IsNullOrEmpty(s)),
                StringComparer.OrdinalIgnoreCase);

            var csprojFiles = Directory.EnumerateFiles(root, "*.csproj", SearchOption.AllDirectories)
                .Where(f => !f.Replace('\\', '/').Contains("/bin/") &&
                            !f.Replace('\\', '/').Contains("/obj/"))
                .OrderBy(f => f, StringComparer.Ordinal)
                .ToList();

            Assert.True(csprojFiles.Count >= 2, $"Expected at least 2 .csproj files, found {csprojFiles.Count}.");

            var inlineVersionErrors = new List<string>();
            var undeclaredPackageErrors = new List<string>();

            foreach (string csproj in csprojFiles)
            {
                var doc = XDocument.Load(csproj);
                var pkgRefs = doc.Descendants("PackageReference");

                foreach (var pref in pkgRefs)
                {
                    string pkgName = pref.Attribute("Include")?.Value ?? "";
                    string? ver = pref.Attribute("Version")?.Value;
                    string relPath = Path.GetRelativePath(root, csproj).Replace('\\', '/');

                    if (!string.IsNullOrEmpty(ver))
                    {
                        inlineVersionErrors.Add($"{relPath}: PackageReference '{pkgName}' specifies inline Version='{ver}'.");
                    }

                    if (!string.IsNullOrEmpty(pkgName) && !declaredPackages.Contains(pkgName))
                    {
                        undeclaredPackageErrors.Add($"{relPath}: PackageReference '{pkgName}' is not defined in Directory.Packages.props.");
                    }
                }
            }

            Assert.True(inlineVersionErrors.Count == 0,
                $"Discovered {inlineVersionErrors.Count} inline PackageReference versions. All package versions must be managed centrally in Directory.Packages.props:\n  " +
                string.Join("\n  ", inlineVersionErrors));

            Assert.True(undeclaredPackageErrors.Count == 0,
                $"Discovered {undeclaredPackageErrors.Count} package references without entries in Directory.Packages.props:\n  " +
                string.Join("\n  ", undeclaredPackageErrors));
        }
    }
}
