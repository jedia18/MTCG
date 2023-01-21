using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace MTCG.Models
{
    public class SpellCard : Card
    {
        public SpellCard(float damage, string element)
        {
            this.damage = damage;
            this.element = element;
            this.type = "Spell";
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
            return false;
        }
    }
}
