import os

def export_unity_codebase(assets_folder_path, output_filename="unity_codebase.txt"):
    """
    Recursively scans your Unity Assets folder and concatenates all .cs files
    into a single text file with clear file path headers.
    """
    script_count = 0
    with open(output_filename, "w", encoding="utf-8") as outfile:
        for root, dirs, files in os.walk(assets_folder_path):
            for file in files:
                if file.endswith(".cs"):
                    file_path = os.path.join(root, file)
                    relative_path = os.path.relpath(file_path, assets_folder_path)
                    
                    outfile.write(f"// ==================================================\n")
                    outfile.write(f"// FILE PATH: Assets/{relative_path}\n")
                    outfile.write(f"// ==================================================\n\n")
                    
                    try:
                        with open(file_path, "r", encoding="utf-8", errors="ignore") as infile:
                            outfile.write(infile.read())
                    except Exception as e:
                        outfile.write(f"// Error reading file: {e}\n")
                    
                    outfile.write("\n\n")
                    script_count += 1

    print(f"Done! Combined {script_count} scripts into '{output_filename}'.")

# Usage — auto-detects the Assets folder relative to this script.
SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
ASSETS_DIR = os.path.join(SCRIPT_DIR, "Assets")

if __name__ == "__main__":
    export_unity_codebase(ASSETS_DIR)