import argparse
import csv
import os
from lxml import etree
from pathlib import Path

entities_to_tags_filename = "entities_to_tags.csv"
tags_to_entities_filename = "tags_to_entities.csv"

bool_tag_name_to_internal_name = {
    "bIsCarryable": "_CARRYABLE",
    "bIsMimicable": "_MIMICABLE",
    "bUsable": "_USABLE",
    "bConsumable": "_CONSUMABLE",
    "bIsTrash": "_TRASH",
    "bImportant": "_IMPORTANT",
    "bIsPlotCritical": "_PLOT_CRITICAL",
    "bIsRandom": "_RANDOM",
    "bSurvivalMode": "_SURVIVAL",
    "bCanTriggerAreas": "_CAN_TRIGGER_AREAS"
}

progression_items = {"Player.ZeroGSuit",
      "Player.Psychoscope", "Mods.Psychoscope.CanScanCoral", "MissionItems.BigNullwaveTransmitter",
      "MissionItems.BigNullwave_Alex_Cinematic", "MissionItems.SelfDestructKey_Alex",
      "MissionItems.SelfDestructKey_Morgan", "MissionItems.SelfDestructKey_Morgan_Cinematic",
      "Crafting.FabricationPlans.PropulsionSystemFabPlan",
      "Crafting.FabricationPlans.BigNullwaveTransmitterFabPlan",
      "Crafting.FabricationPlans.BigNullwaveTransmitterFabPlan_Cinematic",
      "Crafting.FabricationPlans.AlexStationKeyFabPlan",
      "Crafting.FabricationPlans.MorganStationKeyFabPlan",
      "Crafting.FabricationPlans.CoralScanModFabPlan"}

carry_req_mapping = {
    "3149325216928599448": "_LEVERAGE_I",
    "3149325216928599824": "_LEVERAGE_II",
    "3149325216933436173": "_LEVERAGE_III",
}

entities_to_tags = {}
tags_to_entities = {}

def parse_entity_file(filename):
    parser = etree.XMLParser(
            remove_blank_text=False,
            huge_tree=True,
    )
    tree = etree.parse(filename, parser)
    root = tree.getroot()
    for e in root:
        tags = get_tags_for_entity(e)
        entity_name = (e.attrib['Library'] + "." + e.attrib['Name'])
        entities_to_tags[entity_name] = tags
        for t in tags:
            if t not in tags_to_entities:
                tags_to_entities[t] = {entity_name}
            else:
                tags_to_entities[t].add(entity_name)

def get_tags_for_entity(e):
    tags = {e.attrib['Library'], e.attrib['Class']}
    tags.update(e.attrib['Name'].split("."))

    for properties in e.iter():
        child_name = etree.QName(properties).localname
        if (child_name != "Properties"):
            continue
        for bool_tag_name in bool_tag_name_to_internal_name.keys():    
            if (properties.attrib.get(bool_tag_name) == '1'):
                tags.add(bool_tag_name_to_internal_name[bool_tag_name])
        carry_req = properties.attrib.get('ability_CarryRequirement')
        if carry_req in carry_req_mapping:
            tags.add(carry_req_mapping.get(carry_req))

    if e.attrib['Name'] in progression_items:
        tags.add("_PROGRESSION")

    return tags

def write_csv(filename, dict_to_write):
    with Path(filename).open("w", newline="", encoding="utf-8") as file:
        writer = csv.writer(file)
        for key in sorted(dict_to_write):
            values = sorted(dict_to_write[key])
            writer.writerow([key, f"[{','.join(values)}]"])

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument('-i', '--input_dir', required=True, help='Path to entity archetypes folder')
    args = parser.parse_args()
    for f in os.scandir(args.input_dir):
        if not f.is_file() or not f.path.endswith('.xml'):
            continue
        parse_entity_file(f.path)
    write_csv(entities_to_tags_filename, entities_to_tags)
    write_csv(tags_to_entities_filename, tags_to_entities)

    