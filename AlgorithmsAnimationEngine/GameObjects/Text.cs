using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlgorithmsAnimationEngine.GameObjects {
    public class Text : GameObject {
        public string Value { get; set; }
        public Color FontColor { get; set; } = Color.Black;

        private Texture2DRectangle texture;
        private SpriteFont font;

        public Text(string value, Texture2DRectangle texture, SpriteFont font) {
            Value = value;
            this.texture = texture;
            this.font = font;
        }

        public override void OnRender(SpriteBatch spriteBatch, float deltaTime) {
            texture.Render(spriteBatch, IntBounds);
            spriteBatch.DrawString(font, Value, Position, FontColor);
        }
    }
}
