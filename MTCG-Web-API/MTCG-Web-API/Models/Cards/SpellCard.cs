using Microsoft.VisualBasic;

namespace MTCG_Web_API.Models.Cards
{
    public class SpellCard : Card
    {
        public SpellCard(int damage, ElementType element)
        {
            this.damage = damage;
            this.element = element;
            this.type = CardType.Spell;
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
            return false;
        }
    }
}
