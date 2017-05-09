using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private int _health;
        private int _mana;
        private int _maxMana;
        private Text _healthText;
        private Text _manaText;

        public HandController handController { get; private set; }
        public DeckController deckController { get; private set; }

        public int health
        {
            get { return this._health; }
            set { this._health = value; this.UpdateHealthText(); }
        }

        public int mana
        {
            get { return this._mana; }
            set { this._mana = value; this.UpdateManaText(); }
        }

        public int maxMana
        {
            get { return this._maxMana; }
            set {  this._maxMana = value; this.UpdateManaText(); }
        }

        public void Init(string player)
        {
            string str1 = "EnemyHealth";
            string str2 = "EnemyMana";
            string str3 = "EnemyHand";
            string str4 = "EnemyDeckPanel/EnemyDeck";
            if (player == "Player")
            {
                str1 = "PlayerHealth";
                str2 = "PlayerMana";
                str3 = "PlayerHand";
                str4 = "PlayerDeckPanel/PlayerDeck";
            }
            this._healthText = GameObject.Find(str1).GetComponent<Text>();
            this._manaText = GameObject.Find(str2).GetComponent<Text>();
            this.handController = GameObject.Find(str3).GetComponent<HandController>();
            this.deckController = GameObject.Find(str4).GetComponent<DeckController>();
            this._health = 25;
            this._mana = this._maxMana = 0;

            this.deckController.Init(Enumerable.Repeat("TestUnit", 30).ToList());
        }

        public void TakeDamage(int damage)
        {
            this.health -= damage;
        }

        public bool IsDead()
        {
            return this.health <= 0;
        }

        private void UpdateHealthText()
        {
            this._healthText.text = "Health: " + this._health.ToString();
        }

        private void UpdateManaText()
        {
            this._manaText.text = "Mana: " + this._mana.ToString() + "/" + this._maxMana.ToString();
        }
    }
}