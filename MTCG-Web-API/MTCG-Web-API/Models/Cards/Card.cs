namespace MTCG_Web_API.Models.Cards
{
    /// <summary>
    /// Making  spell cards as enum
    /// </summary>
    public enum ElementOfCard {Normal = 0, Water = 1, Fire = 2}

    /// <summary>
    /// Making different card types as enum
    /// </summary>
    public enum TypeOfCard {Spell = 0, Goblin = 1, Dragon = 2, Wizard = 3,
        Ork = 4, Knight = 5, Kraken= 6, Elf = 7, Troll = 8}


    public abstract class Card
    {
        protected float damage;
        protected ElementOfCard element;
        protected TypeOfCard type;

        public float Damage { get; set; }
        public ElementOfCard Element { get; set; }
        public TypeOfCard Type { get; set; }

        public abstract bool isProtectedVsMonster(TypeOfCard opponentType);

        public abstract double damageEffectivenessCalculation(Card opponentCard);
    }
}
