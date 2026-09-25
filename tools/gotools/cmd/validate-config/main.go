package main

import (
	"fmt"
	"os"

	"ashfall/gotools/pkg/config"
)

func main() {
	if len(os.Args) < 3 {
		fmt.Fprintf(os.Stderr, "Usage: validate-config <schema.json> <file.[json|yaml]>\n")
		os.Exit(2)
	}
	schemaPath := os.Args[1]
	instancePath := os.Args[2]

	if err := config.ValidateFile(schemaPath, instancePath); err != nil {
		for _, line := range config.PrettyValidationError(err) {
			fmt.Fprintln(os.Stderr, line)
		}
		os.Exit(1)
	}
	fmt.Println("OK")
}
