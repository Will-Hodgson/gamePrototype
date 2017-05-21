using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class CardFactory
    {
        private AbilityFactory _abilityFactory = new AbilityFactory();

        public Transform CreateCard(string cardName)
        {
            GameObject cardPrefab = Resources.Load("Prefabs/Card", typeof(GameObject)) as GameObject;
            GameObject numberedComponentPrefab = Resources.Load("Prefabs/NumberedComponent", typeof(GameObject)) as GameObject;
            Transform card = GameObject.Instantiate(cardPrefab).transform;

            XmlDocument doc = new XmlDocument();
            doc.Load(Application.dataPath + "/Resources/Cards/" + cardName + ".xml");

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

                List<Ability> abilities = new List<Ability>();
                foreach (XmlNode ability in doc.DocumentElement.SelectNodes("/card/ability"))
                {
                    abilities.Add(this._abilityFactory.CreateAbility(ability.InnerText));
                }
                Transform manaComponent = GameObject.Instantiate(numberedComponentPrefab).transform;
                Transform healthComponent = GameObject.Instantiate(numberedComponentPrefab).transform;
                Transform attackComponent = GameObject.Instantiate(numberedComponentPrefab).transform;
                RectTransform manaRTransform = manaComponent.GetComponent<RectTransform>();
                RectTransform healthRTransform = healthComponent.GetComponent<RectTransform>();
                RectTransform attackRTransform = attackComponent.GetComponent<RectTransform>();
                manaComponent.SetParent(card);
                healthComponent.SetParent(card);
                attackComponent.SetParent(card);
                Vector2 size = manaRTransform.sizeDelta;
                manaRTransform.anchorMin = (new Vector2(0, 1));
                manaRTransform.anchorMax = (new Vector2(0, 1));
                healthRTransform.anchorMin = (new Vector2(1,0));
                healthRTransform.anchorMax = (new Vector2(1,0));
                attackRTransform.anchorMin = (new Vector2(0,0));
                attackRTransform.anchorMax = (new Vector2(0,0));
                manaComponent.position = new Vector3(size[0]/2, (size[1]/2)*-1, 0);
                healthComponent.position = new Vector3((size[0]/2)*-1, (size[1]/2), 0);
                attackComponent.position = new Vector3(size[0]/2, size[1]/2, 0);

                Unit unit = card.gameObject.AddComponent<Unit>();
                unit.Init(name, manaCost, health, attack, attackDistance, diagonalAttackDistance, moveDistance, diagonalMoveDistance, abilities);
                card.gameObject.AddComponent<UnitController>();
                return card;
            }
            else
            {
                return null;
            }
        }
    }
}