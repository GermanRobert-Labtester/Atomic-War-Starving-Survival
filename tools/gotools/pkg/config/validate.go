package config

import (
	"errors"
	"fmt"
	"os"
	"path/filepath"
	"strings"

	"github.com/goccy/go-yaml"
	"github.com/santhosh-tekuri/jsonschema/v6"
)

// ValidateFile validates a JSON or YAML file against a JSON Schema file.
// It returns a detailed error with instance path if validation fails.
func ValidateFile(schemaPath, instancePath string) error {
	absSchema, err := filepath.Abs(schemaPath)
	if err != nil {
		return fmt.Errorf("resolve schema path: %w", err)
	}

	c := jsonschema.NewCompiler()
	sch, err := c.Compile(absSchema)
	if err != nil {
		return fmt.Errorf("compile schema: %w", err)
	}

	// Load instance (JSON or YAML)
	data, err := os.ReadFile(instancePath)
	if err != nil {
		return fmt.Errorf("read instance: %w", err)
	}

	var v any
	// Try YAML first; it also accepts pure JSON.
	if err := yaml.Unmarshal(data, &v); err != nil {
		return fmt.Errorf("parse YAML/JSON: %w", err)
	}

	// Validate
	if err := sch.Validate(v); err != nil {
		return fmt.Errorf("validate %s: %w", instancePath, err)
	}
	return nil
}

// ValidateBytes validates in-memory JSON/YAML bytes against a schema file.
func ValidateBytes(schemaPath string, data []byte) error {
	absSchema, err := filepath.Abs(schemaPath)
	if err != nil {
		return fmt.Errorf("resolve schema path: %w", err)
	}

	c := jsonschema.NewCompiler()
	sch, err := c.Compile(absSchema)
	if err != nil {
		return fmt.Errorf("compile schema: %w", err)
	}

	var v any
	if err := yaml.Unmarshal(data, &v); err != nil {
		return fmt.Errorf("parse YAML/JSON: %w", err)
	}

	if err := sch.Validate(v); err != nil {
		return err
	}
	return nil
}

// PrettyValidationError formats a jsonschema.ValidationError into readable lines.
func PrettyValidationError(err error) []string {
	var ve *jsonschema.ValidationError
	if !errors.As(err, &ve) {
		return []string{err.Error()}
	}

	var lines []string
	collectErrors(ve, &lines)
	if len(lines) == 0 {
		return []string{ve.Error()}
	}
	return lines
}

func collectErrors(ve *jsonschema.ValidationError, lines *[]string) {
	loc := "/" + strings.Join(ve.InstanceLocation, "/")
	if loc == "/" {
		loc = "/"
	}
	if len(ve.Causes) == 0 {
		msg := ve.Error()
		if ve.ErrorKind != nil {
			msg = fmt.Sprintf("%v", ve.ErrorKind)
		}
		*lines = append(*lines, fmt.Sprintf("- %s: %s", loc, msg))
		return
	}
	for _, c := range ve.Causes {
		collectErrors(c, lines)
	}
}

// Typed Game Structs with Two-Step Validation
type Quest struct {
	ID          string      `yaml:"id" json:"id"`
	Title       string      `yaml:"title" json:"title"`
	Description string      `yaml:"description,omitempty" json:"description,omitempty"`
	Objectives  []Objective `yaml:"objectives" json:"objectives"`
	Rewards     *Reward     `yaml:"rewards,omitempty" json:"rewards,omitempty"`
}

type Objective struct {
	ID     string `yaml:"id" json:"id"`
	Type   string `yaml:"type" json:"type"` // "kill", "collect", "talk", "reach"
	Count  int    `yaml:"count,omitempty" json:"count,omitempty"`
	Target string `yaml:"target,omitempty" json:"target,omitempty"`
}

type Reward struct {
	XP    int      `yaml:"xp,omitempty" json:"xp,omitempty"`
	Items []string `yaml:"items,omitempty" json:"items,omitempty"`
}

type Save struct {
	Version  int    `yaml:"version" json:"version"`
	PlayerID string `yaml:"player_id" json:"player_id"`
	Quests   []struct {
		QuestID string `yaml:"quest_id" json:"quest_id"`
		State   string `yaml:"state" json:"state"` // "active", "completed", "failed"
	} `yaml:"quests"`
}

func LoadQuest(schemaPath, path string) (*Quest, error) {
	if err := ValidateFile(schemaPath, path); err != nil {
		return nil, err
	}

	data, err := os.ReadFile(path)
	if err != nil {
		return nil, err
	}

	var q Quest
	if err := yaml.Unmarshal(data, &q); err != nil {
		return nil, fmt.Errorf("unmarshal quest: %w", err)
	}
	return &q, nil
}

func LoadSave(schemaPath, path string) (*Save, error) {
	if err := ValidateFile(schemaPath, path); err != nil {
		return nil, err
	}

	data, err := os.ReadFile(path)
	if err != nil {
		return nil, err
	}

	var s Save
	if err := yaml.Unmarshal(data, &s); err != nil {
		return nil, fmt.Errorf("unmarshal save: %w", err)
	}
	return &s, nil
}
