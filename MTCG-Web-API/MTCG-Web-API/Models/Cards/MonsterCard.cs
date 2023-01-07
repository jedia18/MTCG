using Microsoft.VisualBasic;
using System.Security.AccessControl;

namespace MTCG_Web_API.Models.Cards
{
    public class MonsterCard : Card
    {
        public MonsterCard(int damage, ElementOfCard element, TypeOfCard type)
        {
            this.damage = damage;
            this.element = element;
            this.type = type;
        }
        public override double damageEffectivenessCalculation(Card opponentCard)
        {
            // When the Card is not Spell
            if (opponentCard.Type != TypeOfCard.Spell)
            {
                return 1;
            }

            switch (element)
            {
                case ElementOfCard.Normal:
                    if (opponentCard.Element == ElementOfCard.Water)        // normal->water  effective
                        return 2;
                    else if (opponentCard.Element == ElementOfCard.Fire)    // fire->normal   not effective
                        return 0.5;

                    break;
                case ElementOfCard.Fire:
                    if (opponentCard.Element == ElementOfCard.Normal)       // fire->normal   effective
                        return 2;
                    else if (opponentCard.Element == ElementOfCard.Water)   // water->fire    not effective
                        return 0.5;

                    break;
                case ElementOfCard.Water:
                    if (opponentCard.Element == ElementOfCard.Fire)         // water->fire    effective
                        return 2;
                    else if (opponentCard.Element == ElementOfCard.Normal)  // normal->water  not effective
                        return 0.5;

                    break;
            }

            return 1;
        }

        public override bool isProtectedVsMonster(TypeOfCard opponentType)
        {
            switch (type)
            {
                // Goblins are too afraid of Dragons to attack
                case (TypeOfCard.Dragon):
                    if (opponentType == TypeOfCard.Goblin)
                        return true;
                    break;
                // Wizzard can control Orks so they are not able to damage them
                case (TypeOfCard.Wizard):
                    if (opponentType == TypeOfCard.Ork)
                        return true;
                    break;
                // The FireElves know Dragons since they were little and can evade their attacks
                case (TypeOfCard.Elf):
                    if (element == ElementOfCard.Fire && opponentType == TypeOfCard.Dragon)
                        return true;
                    break;
            }

            return false;
        }
    }
}
