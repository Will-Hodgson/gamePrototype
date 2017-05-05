using UnityEngine;
using System.Collections;
using System.Xml;

namespace Assets.Scripts
{
    public class CardFactory
    {
        public Transform CreateCard(string cardName)
        {
            GameObject cardPrefab = Resources.Load("Prefabs/Card", typeof(GameObject)) as GameObject;
            Transform card = GameObject.Instantiate(cardPrefab.transform);

            XmlDocument doc = new XmlDocument();
            doc.Load(Application.dataPath + "/Resources/Cards/" + cardName + ".xml");
            XmlNode node = doc.DocumentElement.SelectSingleNode("/card");

            string name = doc.DocumentElement.SelectSingleNode("/card/name").InnerText;
            int manaCost = int.Parse(doc.DocumentElement.SelectSingleNode("/card/manaCost").InnerText);

            if (doc.DocumentElement.SelectSingleNode("/card/type").InnerText == "Unit")
            {
                int health = int.Parse(doc.DocumentElement.SelectSingleNode("/card/health").InnerText);
                int attack = int.Parse(doc.DocumentElement.SelectSingleNode("/card/attack").InnerText);
                int attackDistance = int.Parse(doc.DocumentElement.SelectSingleNode("/card/attackDistance").InnerText);
                int diagonalAttackDistance = int.Parse(doc.DocumentElement.SelectSingleNode("/card/diagonalAttackDistance").InnerText);
                int moveDistance = int.Parse(doc.DocumentElement.SelectSingleNode("/card/moveDistance").InnerText);
                int diagonalMoveDistance = int.Parse(doc.DocumentElement.SelectSingleNode("/card/diagonalMoveDistance").InnerText);
                Unit unit = card.gameObject.AddComponent<Unit>();
                unit.Init(name, manaCost, health, attack, attackDistance, diagonalAttackDistance, moveDistance, diagonalMoveDistance);
                return card;
            }
            else
            {
                return null;
            }
        }
    }
}