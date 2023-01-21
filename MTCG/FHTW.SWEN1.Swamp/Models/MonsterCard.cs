using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MTCG.Models
{
    public class MonsterCard : Card
    {
        //public MonsterCard(string name, float damage, string element, CardType type)
        //{
        //    this.name = name;
        //    this.damage = damage;
        //    this.element = element;
        //    this.type = type;
        //}

        public MonsterCard(string id, string name, float damage, string element, string type)
        {
            this.Id = id;
            this.Name = name;
            this.Damage = damage;
            this.Element = element;
            this.Type = type;
        }
        public double damageEffectivenessCalculation(Card opponentCard)
        {
            // When the Card is not Spell
            if (opponentCard.Type != "Spell")
            {
                return 1;
            }

            switch (element)
            {
                case "Regular":
                    if (opponentCard.Element == "Water")        // regular->water  effective
                        return 2;
                    else if (opponentCard.Element == "Fire")    // fire->regular   not effective
                        return 0.5;
                    break;

                case "Fire":
                    if (opponentCard.Element == "Regular")       // fire->regular   effective
                        return 2;
                    else if (opponentCard.Element == "Water")   // water->fire    not effective
                        return 0.5;
                    break;

                case "Water":
                    if (opponentCard.Element == "Fire")         // water->fire    effective
                        return 2;
                    else if (opponentCard.Element == "Regular")  // regular->water  not effective
                        return 0.5;
                    break;
            }

            return 1;
        }

        public bool isProtectedVsMonster(string opponentType)
        {
            switch (type)
            {
                // Goblins are too afraid of Dragons to attack
                case ("Dragon"):
                    if (opponentType == "Goblin")
                        return true;
                    break;
                // Wizzard can control Orks so they are not able to damage them
                case ("Wizard"):
                    if (opponentType == "Ork")
                        return true;
                    break;
                // The FireElves know Dragons since they were little and can evade their attacks
                case ("Elf"):
                    if (element == "Fire" && opponentType == "Dragon")
                        return true;
                    break;
            }

            return false;
        }
    }
}
