namespace MTCG_Web_API.Models.Cards
{
    /// <summary>
    /// Making  spell cards as enum
    /// </summary>
    public enum ElementType {Normal = 0, Water = 1, Fire = 2}

    /// <summary>
    /// Making different card types as enum
    /// </summary>
    public enum CardType {Spell = 0, Goblin = 1, Dragon = 2, Wizard = 3,
        Ork = 4, Knight = 5, Kraken= 6, Elf = 7, Troll = 8}


    public abstract class Card
    {
        protected float damage;
        protected ElementType element;
        protected CardType type;

        public int CardId { get; set; }
        public string CardName { get; set; }
        public float Damage { get; set; }
        public ElementType Element { get; set; }
        public CardType Type { get; set; }
        public Card(int id, string name, int damage, ElementType elementtype, CardType cardtype)
        {
            this.CardId = id;
            this.CardName = name;
            this.Damage = damage;
            this.Element = elementtype;
            this.Type = cardtype;
        }

        public abstract bool isProtectedVsMonster(CardType opponentType);

        public abstract double damageEffectivenessCalculation(Card opponentCard);
    }

}
