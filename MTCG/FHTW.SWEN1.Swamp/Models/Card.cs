﻿using Microsoft.VisualBasic;
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
    //public enum CardType
    //{
    //    Spell = 0, Goblin = 1, Dragon = 2, Wizard = 3,
    //    Ork = 4, Knight = 5, Kraken = 6, Elf = 7, Troll = 8
    //}
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

        //public virtual bool IsProtectedVsMonster(Card opponentType)
        //{
        //    return false;
        //}

        //public virtual double damageEffectivenessCalculation(Card opponentCard)
        //{
        //    return 0;
        //}
    }
}
