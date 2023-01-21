using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.Models
{
    /// <summary>
    /// Making  spell cards as enum
    /// </summary>
    //public enum ElementType {Normal = 0, Water = 1, Fire = 2}


    /// <summary>
    /// Making different card types as enum
    /// </summary>
    public enum CardType
    {
        Spell = 0, Goblin = 1, Dragon = 2, Wizard = 3,
        Ork = 4, Knight = 5, Kraken = 6, Elf = 7, Troll = 8
    }
    public abstract class Card
    {
        public string id;
        public string name;
        public float damage;
        public string element;
        public CardType type;

        public string Id { get; set; }
        public string Name { get; set; }
        public float Damage { get; set; }
        public string Element { get; set; }
        public CardType Type { get; set; }

        public abstract bool isProtectedVsMonster(CardType opponentType);

        public abstract double damageEffectivenessCalculation(Card opponentCard);
    }
}
