package checkplan

import (
	"os"
	"path/filepath"
	"testing"
)

func TestIsCodeFile(t *testing.T) {
	tests := []struct {
		path     string
		expected bool
	}{
		{"Assets/Ashfall.Core/Needs/NeedsSystem.cs", true},
		{"src/Host/HostCli.cs", true},
		{"tools/gotools/main.go", true},
		{"tests/test_something.py", true},
		{"Assets/StreamingAssets/Data/items.json", true},
		{".ai/state.md", false},
		{".ai/plans/my_plan.md", false},
		{"docs/INDEX.md", false},
		{".gitignore", false},
		{"assets/sprites/player.png", false},
	}

	for _, tt := range tests {
		got := IsCodeFile(tt.path)
		if got != tt.expected {
			t.Errorf("IsCodeFile(%q) = %v, want %v", tt.path, got, tt.expected)
		}
	}
}

func TestCheckApprovedPlan(t *testing.T) {
	tempDir := t.TempDir()
	plansDir := filepath.Join(tempDir, ".ai", "plans")
	if err := os.MkdirAll(plansDir, 0755); err != nil {
		t.Fatalf("failed to create temp dir: %v", err)
	}

	// 1. No code files changed -> should pass
	ok, _, err := CheckApprovedPlan(tempDir, false, "", []string{"docs/INDEX.md"})
	if err != nil || !ok {
		t.Errorf("expected pass for non-code files, got ok=%v, err=%v", ok, err)
	}

	// 2. Code file changed, but no plan -> should fail
	ok, msg, err := CheckApprovedPlan(tempDir, false, "", []string{"src/Host/HostCli.cs"})
	if err != nil || ok {
		t.Errorf("expected failure when no approved plan exists, got ok=%v, err=%v", ok, err)
	}
	if msg == "" {
		t.Errorf("expected error message explaining missing plan")
	}

	// 3. Add approved plan -> should pass
	planFile := filepath.Join(plansDir, "feature_plan.md")
	if err := os.WriteFile(planFile, []byte("# Feature Plan\nSTATUS: APPROVED BY USER\n"), 0644); err != nil {
		t.Fatalf("failed to write plan file: %v", err)
	}

	ok, _, err = CheckApprovedPlan(tempDir, false, "", []string{"src/Host/HostCli.cs"})
	if err != nil || !ok {
		t.Errorf("expected pass when approved plan exists, got ok=%v, err=%v", ok, err)
	}

	// 4. Plan in integrated subfolder -> should also pass
	_ = os.Remove(planFile)
	integratedDir := filepath.Join(plansDir, "integrated", "systems")
	if err := os.MkdirAll(integratedDir, 0755); err != nil {
		t.Fatalf("failed to create integrated subfolder: %v", err)
	}
	integratedPlan := filepath.Join(integratedDir, "feature_plan.md")
	if err := os.WriteFile(integratedPlan, []byte("# Feature Plan\nSTATUS: APPROVED BY USER\n"), 0644); err != nil {
		t.Fatalf("failed to write integrated plan file: %v", err)
	}
	ok, _, err = CheckApprovedPlan(tempDir, false, "", []string{"src/Host/HostCli.cs"})
	if err != nil || !ok {
		t.Errorf("expected pass when approved plan exists in integrated subfolder, got ok=%v, err=%v", ok, err)
	}
}
