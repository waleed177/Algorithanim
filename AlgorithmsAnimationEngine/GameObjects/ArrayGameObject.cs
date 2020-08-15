using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlgorithmsAnimationEngine.GameObjects {
    public class ArrayGameObject : GameObject {
        private int arraySize;
        private Texture2DRectangle valueTexture;
        private Text[] values;
        public float ArrayItemSize { get; set; } = 32;
        public float SpaceBetweenItems { get; set; } = 1;

        public ArrayGameObject(int arraySize, string value, Texture2DRectangle valueTexture, SpriteFont font) {
            this.arraySize = arraySize;
            this.valueTexture = valueTexture;
            values = new Text[arraySize];

            for (int i = 0; i < arraySize; i++) {
                values[i] = new Text(value, valueTexture, font) {
                    Position = Position + new Vector2((ArrayItemSize + SpaceBetweenItems) * i, 0),
                    Size = new Vector2(ArrayItemSize, ArrayItemSize)
                };
                AddChild(values[i]);
            }
        }

        public void SetValue(int id, string value) {
            values[id].Value = value;
        }

        public Text GetTextObject(int id) {
            return values[id];
        }

        public override void OnRender(SpriteBatch spriteBatch, float deltaTime) {

        }

    }
}
