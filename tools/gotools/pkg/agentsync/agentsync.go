package agentsync

import (
	"fmt"
	"os"
	"path/filepath"
	"strings"
	"time"
)

var TargetClients = map[string]string{
	"CLAUDE.md":      "CLAUDE CODE INSTRUCTIONS — ASHFALL PROJECT",
	"CODEX.md":       "ASHFALL PROJECT — CODEX Instructions",
	"CRUSH.md":       "ASHFALL PROJECT — CRUSH Instructions",
	"GOOSE.md":       "ASHFALL PROJECT — GOOSE Instructions",
	"QWEN.md":        "ASHFALL PROJECT — QWEN Instructions",
	"VIBE.md":        "ASHFALL PROJECT — VIBE Instructions",
	"MIMOCODE.md":    "ASHFALL PROJECT — MIMOCODE Instructions",
	"OPENSETUP.md":   "ASHFALL PROJECT — OPENSETUP Instructions",
	"ANTIGRAVITY.md": "ASHFALL PROJECT — ANTIGRAVITY Instructions",
	"GEMINI.md":      "ASHFALL PROJECT — GEMINI Instructions",
	".clinerules":    "ASHFALL PROJECT — Cline Rules",
	".cursorrules":   "ASHFALL PROJECT — Cursor Rules",
	".windsurfrules": "ASHFALL PROJECT — Windsurf Rules",
}

const NonNegotiableMarker = "## READ THIS FIRST — NON-NEGOTIABLE RULES"

func SyncAgentRulebooks(repoRoot string, checkOnly bool) (bool, []string, error) {
	canonicalPath := filepath.Join(repoRoot, "AGENTS.md")
	contentBytes, err := os.ReadFile(canonicalPath)
	if err != nil {
		return false, nil, fmt.Errorf("canonical AGENTS.md not found: %w", err)
	}

	content := string(contentBytes)
	idx := strings.Index(content, NonNegotiableMarker)
	if idx == -1 {
		return false, nil, fmt.Errorf("marker %q not found in AGENTS.md", NonNegotiableMarker)
	}

	body := content[idx:]
	today := time.Now().UTC().Format("2006-01-02")

	var drifted []string
	var updated []string

	for filename, title := range TargetClients {
		targetPath := filepath.Join(repoRoot, filename)
		header := fmt.Sprintf("# %s\n# AUTO-GENERATED from AGENTS.md (canonical source). Run sync-agent-rulebooks.py to regenerate.\n# Last generated: %s\n\n---\n\n",
			title, today)
		expected := header + body

		existingBytes, err := os.ReadFile(targetPath)
		if err != nil || string(existingBytes) != expected {
			drifted = append(drifted, filename)
			if !checkOnly {
				if err := os.WriteFile(targetPath, []byte(expected), 0644); err != nil {
					return false, nil, fmt.Errorf("failed to write %s: %w", filename, err)
				}
				updated = append(updated, filename)
			}
		}
	}

	// Update report
	if !checkOnly {
		reportPath := filepath.Join(repoRoot, "docs", "agents", "AGENTS_SYNC_REPORT.md")
		_ = os.MkdirAll(filepath.Dir(reportPath), 0755)
		var report strings.Builder
		report.WriteString(fmt.Sprintf("# Agent Rulebooks Synchronization Report\nGenerated: %s\n\n", today))
		report.WriteString("All client rulebooks are synchronized with canonical `AGENTS.md` via `ashfall-dev sync-agents`:\n")
		for fn := range TargetClients {
			report.WriteString(fmt.Sprintf("- [x] `%s`\n", fn))
		}
		_ = os.WriteFile(reportPath, []byte(report.String()), 0644)
	}

	return len(drifted) == 0, updated, nil
}
