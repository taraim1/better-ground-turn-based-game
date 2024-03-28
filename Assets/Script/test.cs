using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    Playable_Character a = new Playable_Character();
    Playable_Character b = new Playable_Character();
    Playable_Character c = new Playable_Character();
    Playable_Character d = new Playable_Character();
    void Start()
    {
        Playable_Character a = new Playable_Character();
        Playable_Character b = new Playable_Character();
        Playable_Character c = new Playable_Character();
        Playable_Character d = new Playable_Character();

        a.set_character_name("John");
        a.set_character_int_property(CharacterManager.character_int_properties.max_health, 20);
        a.set_character_int_property(CharacterManager.character_int_properties.current_health, 20);
        a.set_character_int_property(CharacterManager.character_int_properties.max_willpower, 10);
        a.set_character_int_property(CharacterManager.character_int_properties.current_willpower, 10);
        a.set_character_int_property(CharacterManager.character_int_properties.number_of_skill_slots, 2);

        b.set_character_name("Karl");
        b.set_character_int_property(CharacterManager.character_int_properties.max_health, 40);
        b.set_character_int_property(CharacterManager.character_int_properties.current_health, 40);
        b.set_character_int_property(CharacterManager.character_int_properties.max_willpower, 20);
        b.set_character_int_property(CharacterManager.character_int_properties.current_willpower, 20);
        b.set_character_int_property(CharacterManager.character_int_properties.number_of_skill_slots, 2);

        c.set_character_name("Asha");
        c.set_character_int_property(CharacterManager.character_int_properties.max_health, 15);
        c.set_character_int_property(CharacterManager.character_int_properties.current_health, 15);
        c.set_character_int_property(CharacterManager.character_int_properties.max_willpower, 8);
        c.set_character_int_property(CharacterManager.character_int_properties.current_willpower, 8);
        c.set_character_int_property(CharacterManager.character_int_properties.number_of_skill_slots, 4);

        d.set_character_name("Drake");
        d.set_character_int_property(CharacterManager.character_int_properties.max_health, 30);
        d.set_character_int_property(CharacterManager.character_int_properties.current_health, 30);
        d.set_character_int_property(CharacterManager.character_int_properties.max_willpower, 12);
        d.set_character_int_property(CharacterManager.character_int_properties.current_willpower, 12);
        d.set_character_int_property(CharacterManager.character_int_properties.number_of_skill_slots, 2);

       
        PartyManager.instance.set_character_to_party(a, 0);
        PartyManager.instance.set_character_to_party(b, 1);
        PartyManager.instance.set_character_to_party(c, 2);
        PartyManager.instance.set_character_to_party(d, 3);

        PartyManager.instance.write_Json_file();

    }

}


