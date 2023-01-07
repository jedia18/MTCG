using Microsoft.VisualBasic;

namespace MTCG_Web_API.Models.Cards
{
    public class SpellCard : Card
    {
        public SpellCard(int damage, ElementOfCard element)
        {
            this.damage = damage;
            this.element = element;
            this.type = TypeOfCard.Spell;
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
            return false;
        }
    }
}
