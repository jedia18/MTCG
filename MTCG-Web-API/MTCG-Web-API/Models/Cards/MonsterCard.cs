using Microsoft.VisualBasic;
using System.Security.AccessControl;
using System;

namespace MTCG_Web_API.Models.Cards
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

        public MonsterCard(string id, string name, float damage, string element)
        {
            this.Id = id;
            this.Name = name;
            this.Damage = damage;
            this.Element = element;
        }
        public override double damageEffectivenessCalculation(Card opponentCard)
        {
            // When the Card is not Spell
            if (opponentCard.Type != CardType.Spell)
            {
                return 1;
            }

            switch (element)
            {
                case "Normal":
                    if (opponentCard.Element == "Water")        // normal->water  effective
                        return 2;
                    else if (opponentCard.Element == "Fire")    // fire->normal   not effective
                        return 0.5;

                    break;
                case "Fire":
                    if (opponentCard.Element == "Normal")       // fire->normal   effective
                        return 2;
                    else if (opponentCard.Element == "Water")   // water->fire    not effective
                        return 0.5;

                    break;
                case "Water":
                    if (opponentCard.Element == "Fire")         // water->fire    effective
                        return 2;
                    else if (opponentCard.Element == "Normal")  // normal->water  not effective
                        return 0.5;

                    break;
            }

            return 1;
        }

        public override bool isProtectedVsMonster(CardType opponentType)
        {
            switch (type)
            {
                // Goblins are too afraid of Dragons to attack
                case (CardType.Dragon):
                    if (opponentType == CardType.Goblin)
                        return true;
                    break;
                // Wizzard can control Orks so they are not able to damage them
                case (CardType.Wizard):
                    if (opponentType == CardType.Ork)
                        return true;
                    break;
                // The FireElves know Dragons since they were little and can evade their attacks
                case (CardType.Elf):
                    if (element == "Fire" && opponentType == CardType.Dragon)
                        return true;
                    break;
            }

            return false;
        }
    }
}
