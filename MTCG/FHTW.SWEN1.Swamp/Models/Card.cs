using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Models
{
    public class Card
    {
        public string id;
        public string name;
        public float damage;
        public string element;
        public string type;

        public string Id { get; set; }
        public string Name { get; set; }
        public float Damage { get; set; }
        public string Element { get; set; }        // Regular, Water, Fire
        public string Type { get; set; }           // Spell, Goblin, Dragon, Wizard, Ork, Knight, Kraken, Elf, Troll
    }
}
