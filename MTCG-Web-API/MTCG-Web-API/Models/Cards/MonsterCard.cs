using Microsoft.VisualBasic;
using System.Security.AccessControl;

namespace MTCG_Web_API.Models.Cards
{
    public class MonsterCard : Card
    {
        public MonsterCard(int damage, ElementType element, CardType type)
        {
            this.damage = damage;
            this.element = element;
            this.type = type;
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
                case ElementType.Normal:
                    if (opponentCard.Element == ElementType.Water)        // normal->water  effective
                        return 2;
                    else if (opponentCard.Element == ElementType.Fire)    // fire->normal   not effective
                        return 0.5;

                    break;
                case ElementType.Fire:
                    if (opponentCard.Element == ElementType.Normal)       // fire->normal   effective
                        return 2;
                    else if (opponentCard.Element == ElementType.Water)   // water->fire    not effective
                        return 0.5;

                    break;
                case ElementType.Water:
                    if (opponentCard.Element == ElementType.Fire)         // water->fire    effective
                        return 2;
                    else if (opponentCard.Element == ElementType.Normal)  // normal->water  not effective
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
                    if (element == ElementType.Fire && opponentType == CardType.Dragon)
                        return true;
                    break;
            }

            return false;
        }
    }
}
