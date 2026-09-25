package parser

import (
	"testing"
)

func TestParseDotnetOutput(t *testing.T) {
	out := `
  Ashfall.Core -> /bin/Ashfall.Core.dll
  Ashfall.Core.Tests -> /bin/Ashfall.Core.Tests.dll
Test run for Ashfall.Core.Tests.dll (.NETCoreApp,Version=v9.0)
Passed!  - Failed:     0, Passed:    32, Skipped:     0, Total:    32, Duration: 49 ms - Ashfall.Core.Tests.dll (net9.0)
`
	res := ParseTestOutput(out, 0)
	if res.Runner != "dotnet/xunit" {
		t.Fatalf("expected dotnet/xunit, got %s", res.Runner)
	}
	if res.Passed != 32 || res.Failed != 0 {
		t.Fatalf("expected 32 passed 0 failed, got passed=%d failed=%d", res.Passed, res.Failed)
	}
	if res.Taxonomy != TaxonomyPass {
		t.Fatalf("expected pass taxonomy, got %s", res.Taxonomy)
	}
}

func TestParsePytestOutput(t *testing.T) {
	out := `
tests/test_audio_pipeline.py::TestAudioPipeline::test_presets_ceiling PASSED [ 80%]
tests/test_audio_pipeline.py::TestAudioPipeline::test_reproducibility PASSED [100%]
============================== 5 passed in 0.58s ===============================
`
	res := ParseTestOutput(out, 0)
	if res.Runner != "pytest" {
		t.Fatalf("expected pytest, got %s", res.Runner)
	}
	if res.Passed != 5 || res.Failed != 0 {
		t.Fatalf("expected 5 passed 0 failed, got passed=%d failed=%d", res.Passed, res.Failed)
	}
}
