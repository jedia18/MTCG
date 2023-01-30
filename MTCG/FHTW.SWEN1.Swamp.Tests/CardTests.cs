using MTCG.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Numerics;

namespace MTCG.Tests
{
    internal class CardTests
    {
        // Test the DamageEffectivenessCalculation method for a MonsterCard with Fire element against a SpellCard with defferent spells
        [Test]
        public void DamageEffectivenessCalculation_MonsterCard()
        {
            // Arrange
            var monsterCard = new MonsterCard();
            monsterCard.Element = "Fire";
            monsterCard.Type = "Spell";
            var opponentCard = new SpellCard();
            opponentCard.Type = "Spell";

            // Act & Assert
            opponentCard.Element = "Regular";
            Assert.That(monsterCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(2.0));

            opponentCard.Element = "Water";
            Assert.That(monsterCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(0.5));

            opponentCard.Element = "Fire";
            Assert.That(monsterCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(1));
        }

        // Test the DamageEffectivenessCalculation method for a SpellCard with Water element against a SpellCard with defferent spells
        [Test]
        public void DamageEffectivenessCalculation_SpellCard()
        {
            // Arrange
            var spellCard = new SpellCard(10, "Water");
            var opponentCard = new SpellCard();
            opponentCard.Type = "Spell";

            // Act & Assert
            opponentCard.Element = "Regular";
            Assert.That(spellCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(0.5));

            opponentCard.Element = "Water";
            Assert.That(spellCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(1.0));

            opponentCard.Element = "Fire";
            Assert.That(spellCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(2.0));
        }

        // Test the DamageEffectivenessCalculation method for a SpellCard with Water element against a MonsterCard with type != Spell
        [Test]
        public void DamageEffectivenessCalculation_SpellCardVSMonsterCard()
        {
            // Arrange
            var spellCard = new SpellCard(10, "Water");
            var opponentCard = new MonsterCard();
            opponentCard.Type = "Goblin";

            // Act & Assert
            opponentCard.Element = "";
            Assert.That(spellCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(1));
        }

        // Test the IsProtectedVsMonster method for a MonsterCard with Dragon Type against a MonsterCard with Goblin Type
        [Test]
        public void TestIsProtectedVsMonster()
        {
            // Arrange
            var monsterCard = new MonsterCard();
            monsterCard.Type = "Dragon";

            // Act
            var result = monsterCard.isProtectedVsMonster("Goblin");

            // Assert
            Assert.IsTrue(result);
        }

        // Test the IsProtectedVsMonster method for a SpellCard with an opponet which is MonsterCard
        [Test]
        public void Test_isProtectedVsMonster()
        {
            // Arrange
            string opponentType = "Goblin";
            var testObject = new SpellCard();

            // Act
            var result = testObject.isProtectedVsMonster(opponentType);

            // Assert
            Assert.IsFalse(result);
        }

        // Test the DamageEffectivenessCalculation method for a MonsterCard with Water element against a SpellCard with defferent spells
        [Test]
        public void DamageEffectivenessCalculation_MonsterCard_WaterVsSpell()
        {
            // Arrange
            var monsterCard = new MonsterCard();
            monsterCard.Element = "Water";
            monsterCard.Type = "Spell";
            var opponentCard = new SpellCard();
            opponentCard.Type = "Spell";

            // Act & Assert
            opponentCard.Element = "Regular";
            Assert.That(monsterCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(0.5));

            opponentCard.Element = "Water";
            Assert.That(monsterCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(1));

            opponentCard.Element = "Fire";
            Assert.That(monsterCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(2.0));
        }

        // Test the DamageEffectivenessCalculation method for a MonsterCard without Spell vs. another Monstercard
        [Test]
        public void DamageEffectivenessCalculation_MonsterCard_MonsterVsMonster()
        {
            // Arrange
            var monsterCard = new MonsterCard();
            monsterCard.Element = "";
            monsterCard.Type = "Goblin";
            var opponentCard = new SpellCard();
            opponentCard.Type = "Ork";

            // Act & Assert
            Assert.That(monsterCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(1));
        }

        // Test the DamageEffectivenessCalculation method for a SpellCard with Fire element against a SpellCard with defferent spells
        [Test]
        public void DamageEffectivenessCalculation_SpellCardVsSpellCard()
        {
            // Arrange
            var spellCard = new SpellCard(10, "Fire");
            var opponentCard = new SpellCard();
            opponentCard.Type = "Spell";

            // Act & Assert
            opponentCard.Element = "Regular";
            Assert.That(spellCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(2.0));

            opponentCard.Element = "Water";
            Assert.That(spellCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(0.5));

            opponentCard.Element = "Fire";
            Assert.That(spellCard.damageEffectivenessCalculation(opponentCard), Is.EqualTo(1));
        }

    }
}
