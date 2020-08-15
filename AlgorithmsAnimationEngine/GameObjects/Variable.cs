using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlgorithmsAnimationEngine.GameObjects {
    public class Variable : GameObject {
        public string Name { get => textName.Value; set { textName.Value = value; } }
        public string Value { get => textValue.Value; set { textValue.Value = value; } }
        public Color FontColor { get => textValue.FontColor; set { textValue.FontColor = textName.FontColor = value; } }

        private Text textName, textValue;


        public Variable(string name, string value, Texture2DRectangle texture, SpriteFont font) {
            Size = new Vector2(96, 32);
            AddChild(textName = new Text(name, texture, font));
            AddChild(textValue = new Text(value, texture, font));
            OnSizeChanged += Variable_OnSizeChanged;
        }

        private void Variable_OnSizeChanged(GameObject sender) {
            textValue.Size = textName.Size = new Vector2(Size.X/2, Size.Y);
            textValue.Position = Position + new Vector2(Size.X/2, 0);
        }

        public override void OnRender(SpriteBatch spriteBatch, float deltaTime) {}
    }
}
