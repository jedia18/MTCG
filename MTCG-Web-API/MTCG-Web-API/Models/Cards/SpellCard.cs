using Microsoft.VisualBasic;

namespace MTCG_Web_API.Models.Cards
{
    public class SpellCard : Card
    {
        public SpellCard(float damage, string element)
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
            return false;
        }
    }
}
