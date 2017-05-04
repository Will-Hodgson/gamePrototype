using UnityEngine;
using System.Collections;
using System.Xml;

namespace Assets.Scripts
{
    public class CardFactory
    {
        [SerializeField] private static Transform _cardPrefab;

        public static Card CreateCard(string cardName)
        {
            Transform card = GameObject.Instantiate(CardFactory._cardPrefab);
            card.gameObject.SetActive(false);

            XmlDocument doc = new XmlDocument();
            doc.Load(Application.dataPath + "/Cards/" + cardName + ".xml");
            XmlNode node = doc.DocumentElement.SelectSingleNode("/card");

            string name = node.Attributes["name"].Value;
            int manaCost = int.Parse(node.Attributes["manaCost"].Value);

            if (node.Attributes["type"].Value == "Unit")
            {
                int health = int.Parse(node.Attributes["health"].Value);
                int attack = int.Parse(node.Attributes["attack"].Value);
                int attackDistance = int.Parse(node.Attributes["attackDistance"].Value);
                int diagonalAttackDistance = int.Parse(node.Attributes["diagonalAttackDistance"].Value);
                int moveDistance = int.Parse(node.Attributes["moveDistance"].Value);
                int diagonalMoveDistance = int.Parse(node.Attributes["diagonalMoveDistance"].Value);
                Unit unit = new Unit();
                unit.Init(name, manaCost, health, attack, attackDistance, diagonalAttackDistance, moveDistance, diagonalMoveDistance);
                return unit;
            }
            else
            {
                return null;
            }
        }
    }
}