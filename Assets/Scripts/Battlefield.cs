using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Battlefield : MonoBehaviour
    {
        [SerializeField] private Transform _squarePrefab;
        private int _width = 5;
        private int _height = 5;
        private Transform[,] _battlefield;
        private List<Transform> _cards;

        public int width { get { return this._width; } }

        public int height { get { return this._height; } }

        public List<Transform> cards { get { return this._cards; } }

        void Awake()
        {
            // Resize the cellSize for different screen sizes
            GridLayoutGroup gridLayoutGroup = this.GetComponent<GridLayoutGroup>();
            RectTransform rect = this.GetComponent<RectTransform>();
            this._cards = new List<Transform>();
            gridLayoutGroup.cellSize = new Vector2(rect.rect.width / this._width, rect.rect.height / this._height);
            this._battlefield = new Transform[this._width, this._height];
            for (int y = 0; y < this._height; y++)
            {
                for (int x = 0; x < this._width; x++)
                {
                    Transform square = Instantiate(this._squarePrefab, (new Vector3(x, y, 0)) + (Vector3)this.GetComponent<RectTransform>().offsetMin, Quaternion.identity) as Transform;
                    square.gameObject.GetComponent<Image>().color = UnityEngine.Color.clear;
                    square.SetParent(this.transform, false);
                    this._battlefield[x, y] = square;
                    SquareController controller = square.GetComponent<SquareController>();
                    controller.Init(new int[] { x, y });
                }
            }
        }

        public Transform GetSquareAt(int x, int y)
        {
            if (x < 0 || y < 0 || x >= this._width || y >= this._height)
            {
                Debug.LogWarning("Coodinates are out of bounds!");
                return null;
            }
            return this._battlefield[x, y];
        }

        public void AddCard(Transform card)
        {
            this._cards.Add(card);
        }

        public void DeleteCard(Transform card)
        {
            if (!this._cards.Contains(card))
            {
                Debug.LogWarning("Tryed to delete a card from the battlefield list that is not there!");
                return;
            }
            this._cards.Remove(card);
        }

        public Transform[] GetSquares()
        {
            var squares = new Transform[this._width * this._height];
            for (int y = 0; y < this._height; y++)
            {
                for (int x = 0; x < this._width; x++)
                {
                    squares[(y * this._width) + x] = this._battlefield[x, y];
                }
            }
            return squares;
        }
    }
}
