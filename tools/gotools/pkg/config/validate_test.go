package config

import (
	"os"
	"path/filepath"
	"testing"
)

func TestValidateQuestSchema(t *testing.T) {
	repoRoot := "../../../../"
	schemaPath := filepath.Join(repoRoot, "schemas", "quest.schema.json")

	tmpDir := t.TempDir()

	// 1. Valid YAML quest
	validQuestYAML := filepath.Join(tmpDir, "quest_valid.yaml")
	_ = os.WriteFile(validQuestYAML, []byte(`
id: rescue_survivor_01
title: "Rescue Dr. Vance"
description: "Locate and extract Dr. Vance from the collapsed shelter."
objectives:
  - id: reach_shelter
    type: reach
    target: shelter_collapsed_04
  - id: talk_doctor
    type: talk
    target: npc_vance
rewards:
  xp: 150
  items:
    - medkit_military
`), 0644)

	q, err := LoadQuest(schemaPath, validQuestYAML)
	if err != nil {
		t.Fatalf("expected valid quest to pass, got: %v", err)
	}
	if q.ID != "rescue_survivor_01" || len(q.Objectives) != 2 || q.Rewards.XP != 150 {
		t.Fatalf("unexpected unmarshaled quest values: %+v", q)
	}

	// 2. Invalid YAML quest (disallowed additional property 'gold', invalid objective type 'dance')
	invalidQuestYAML := filepath.Join(tmpDir, "quest_invalid.yaml")
	_ = os.WriteFile(invalidQuestYAML, []byte(`
id: invalid_quest
title: "Invalid Quest"
objectives:
  - id: obj_1
    type: dance
rewards:
  gold: 100
`), 0644)

	err = ValidateFile(schemaPath, invalidQuestYAML)
	if err == nil {
		t.Fatalf("expected invalid quest to fail validation")
	}

	errLines := PrettyValidationError(err)
	if len(errLines) == 0 {
		t.Fatalf("expected pretty error lines, got none")
	}
}

func TestValidateSaveSchema(t *testing.T) {
	repoRoot := "../../../../"
	schemaPath := filepath.Join(repoRoot, "schemas", "save.schema.json")

	tmpDir := t.TempDir()

	validSaveJSON := filepath.Join(tmpDir, "save_slot_01.json")
	_ = os.WriteFile(validSaveJSON, []byte(`{
  "version": 1,
  "player_id": "survivor_42",
  "quests": [
    {
      "quest_id": "rescue_survivor_01",
      "state": "active"
    }
  ]
}`), 0644)

	s, err := LoadSave(schemaPath, validSaveJSON)
	if err != nil {
		t.Fatalf("expected valid save to pass, got: %v", err)
	}
	if s.Version != 1 || s.PlayerID != "survivor_42" || len(s.Quests) != 1 {
		t.Fatalf("unexpected unmarshaled save: %+v", s)
	}
}
