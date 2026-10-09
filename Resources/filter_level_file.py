import argparse
import csv
import os.path
from lxml import etree


level_paths = ["endgame/level", "engineering/cargobay/level", "engineering/lifesupport/level", "engineering/powersource/level", "executive/arboretum/level", "executive/bridge/level", "executive/corporateit/level", "executive/crewfacilities/level", "research/lobby/level", "research/prototype/level", "research/psychotronics/level", "research/shuttlebay/level", "research/simulationlabs/level", "research/zerog_utilitytunnels/level", "station/exterior/level"]

level_file_name = "mission_mission0.xml"

# Strip namespace
def local_name(element):
    return etree.QName(element).localname

def remove_element_namespace(element):
    if isinstance(element.tag, str):
        element.tag = etree.QName(element).localname

    for child in element:
        remove_element_namespace(child)

def filter_entities(xml_input, csv_input, xml_output):
    # Load supported tags
    with open(csv_input, "r", encoding="utf-8-sig", newline="") as f:
        supported_classes = {
            row[0].strip()
            for row in csv.reader(f)
            if row and row[0].strip()
        }

    parser = etree.XMLParser(
        remove_blank_text=False,
        huge_tree=True,
    )

    tree = etree.parse(xml_input, parser)
    root = tree.getroot()

    # Find <Objects>, regardless of its namespace.
    objects = next(
        (child for child in root if local_name(child) == "Objects"),
        None,
    )

    if objects is None:
        raise ValueError("Could not find <Objects> element.")

    # Remove every root-level node except <Objects>.
    for child in list(root):
        if child is not objects:
            root.remove(child)

    retained = 0
    removed = 0

    # Process direct children of <Objects>.
    for child in list(objects):
        name = local_name(child)

        # Remove <Object> nodes.
        if name == "Object":
            objects.remove(child)
            removed += 1
            continue

        # Process <Entity> nodes.
        if name == "Entity":
            entity_class = child.get("EntityClass")

            # Preserve NPC spawners and supported archetypes
            if (entity_class != "ArkNpcSpawner" and
                entity_class != "FlowgraphEntity" and
                (entity_class not in supported_classes
                or "Archetype" not in child.attrib)
            ):
                objects.remove(child)
                removed += 1
                continue

            # Remove the EntityId attribute
            child.attrib.pop("EntityId", None)

            retained += 1

    # Remove the namespace from the elements that remain.
    #
    # Attribute namespaces are NOT modified, so attributes such as
    # ch:serialize and xsi:type retain their namespaces.
    remove_element_namespace(root)

    # Write output.
    tree.write(
        xml_output,
        encoding="UTF-8",
        xml_declaration=True,
        pretty_print=False,
    )

    start_size = round(os.path.getsize(xml_input) / 1000)
    end_size = round(os.path.getsize(xml_output) / 1000)
    diff = (end_size - start_size)
    print(f"Retained entities:         {retained}")
    print(f"Removed nodes:             {removed}")
    print(f"Start file size in kb:     {start_size} kb")
    print(f"File size decrease in kb:  {diff} kb")
    print(f"Result file size in kb:     {end_size} kb")
    print(f"Output written to: {xml_output}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('-i', '--input_dir', required=True, help='path to Campaign folder with levels files')
    parser.add_argument('-o', '--output_dir', required=True, help='path to export filtered levels files to')
    parser.add_argument('-c', '--csv', required=True, help='csv file defining supported tags')
    args = parser.parse_args()

    # Verify that we are in the Campaign folder as input
    if os.path.basename(args.input_dir).lower() != "campaign" or not os.path.exists(args.input_dir):
        print("Expected to start in Campaign directory or Campaign directory does not exist")
        exit

    if not os.path.exists(args.output_dir):
        print("output dir does not exist, please create")
        exit

    if not os.path.exists(args.csv):
        print("tags file does not exist")
        exit

    # Iterate through each folder of the input directory
    for l in level_paths:
        input_level_file = os.path.join(args.input_dir, l, level_file_name)
        output_level_file = os.path.join(args.output_dir, l, level_file_name)
        filter_entities(input_level_file, args.csv, output_level_file)