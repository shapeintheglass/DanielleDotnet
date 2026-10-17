import argparse
import xml.etree.ElementTree as ET
from pathlib import Path

fruit_tree_files = ["arkpickups.xml", "arkgameplayarchitecture.xml"]

PREY_NS = "https://thelivingdiamond.github.io/Chairloader/Xsd/Prey"
XSI_NS = "http://www.w3.org/2001/XMLSchema-instance"
CH_NS = "https://thelivingdiamond.github.io/Chairloader/Xsd/Chairloader"


def local_name(tag: str) -> str:
    """Get the element name without its namespace."""
    return tag.split("}", 1)[-1] if tag.startswith("{") else tag


def filter_fruit_tree_files(input_dir: Path, output_dir: Path):
    missing_files = [
        filename
        for filename in fruit_tree_files
        if not (input_dir / filename).is_file()
    ]

    if missing_files:
        print(f"Missing required input files: {', '.join(missing_files)}")
        return

    output_dir.mkdir(parents=True, exist_ok=True)

    # Preserve the original namespace prefixes.
    ET.register_namespace("", PREY_NS)
    ET.register_namespace("ch", CH_NS)
    ET.register_namespace("xsi", XSI_NS)

    for filename in fruit_tree_files:
        input_path = input_dir / filename
        output_path = output_dir / filename

        tree = ET.parse(input_path)
        root = tree.getroot()

        # Remove every EntityPrototype that isn't a fruit tree.
        for parent in root.iter():
            for child in list(parent):
                if (
                    local_name(child.tag) == "EntityPrototype"
                    and child.get("Class") != "ArkFruitTree"
                ):
                    parent.remove(child)

        tree.write(
            output_path,
            encoding="utf-8",
            xml_declaration=True,
        )

        print(f"Filtered {filename} -> {output_path}")


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument(
        "-i", "--input_dir",
        required=True,
        help="Path to entities directory",
    )
    parser.add_argument(
        "-o", "--output_dir",
        required=True,
        help="Path to output filtered files",
    )

    args = parser.parse_args()

    filter_fruit_tree_files(
        Path(args.input_dir),
        Path(args.output_dir),
    )