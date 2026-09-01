// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests;

public class LegacyAssetGateTests
{
    private static string GateScript(string name) =>
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "scripts", "ci", name));

    private static string RunGateAndCollectOutput(string scriptPath)
    {
        // ProcessStartInfo with UseShellExecute=false splits argument strings
        // on whitespace, so paths containing spaces (e.g. ".../Atomic War/...")
        // cannot be passed as a single quoted command line. Use ArgumentList
        // to keep the script path as a single argv element.
        var psi = new ProcessStartInfo
        {
            FileName = "/usr/bin/bash",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        psi.ArgumentList.Add(scriptPath);
        using var p = Process.Start(psi)!;
        p.WaitForExit(30_000);
        string stdout = p.StandardOutput.ReadToEnd();
        string stderr = p.StandardError.ReadToEnd();
        if (p.ExitCode != 0)
        {
            throw new Xunit.Sdk.XunitException(
                $"Gate script exited with code {p.ExitCode}.\nstdout:\n{stdout}\nstderr:\n{stderr}");
        }
        return stdout + stderr;
    }

    [Fact]
    public void LegacyAssetPathGate_Passes()
    {
        string output = RunGateAndCollectOutput(GateScript("legacy-asset-path-gate.sh"));
        Assert.Contains("Legacy Asset Path Gate PASSED", output);
    }

    [Fact]
    public void LegacyReferenceGate_Passes()
    {
        string output = RunGateAndCollectOutput(GateScript("legacy-reference-gate.sh"));
        Assert.Contains("Legacy Reference Gate PASSED", output);
    }
}
