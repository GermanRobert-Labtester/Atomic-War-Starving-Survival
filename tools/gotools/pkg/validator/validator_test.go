package validator

import (
	"os"
	"path/filepath"
	"testing"
)

func TestValidateJSONFile(t *testing.T) {
	tmpDir := t.TempDir()

	validFile := filepath.Join(tmpDir, "valid.json")
	_ = os.WriteFile(validFile, []byte(`{"schema_version": 1, "data": "test"}`), 0644)
	v := ValidateJSONFile(validFile)
	if len(v) != 0 {
		t.Fatalf("expected 0 violations for valid file, got %d: %+v", len(v), v)
	}

	bareArrayFile := filepath.Join(tmpDir, "array.json")
	_ = os.WriteFile(bareArrayFile, []byte(`[{"schema_version": 1}]`), 0644)
	vArray := ValidateJSONFile(bareArrayFile)
	if len(vArray) == 0 || vArray[0].Rule != "object_root" {
		t.Fatalf("expected object_root violation, got %+v", vArray)
	}

	missingVersionFile := filepath.Join(tmpDir, "no_version.json")
	_ = os.WriteFile(missingVersionFile, []byte(`{"id": "foo"}`), 0644)
	vNoVer := ValidateJSONFile(missingVersionFile)
	if len(vNoVer) == 0 || vNoVer[0].Rule != "schema_version_present" {
		t.Fatalf("expected schema_version_present violation, got %+v", vNoVer)
	}
}
