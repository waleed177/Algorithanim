using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlgorithmsAnimationEngine {
    public struct Texture2DRectangle {
        public Texture2D texture;
        public Rectangle rectangle;
        public Color color;

        public Texture2DRectangle(Texture2D texture, Rectangle rectangle, Color color) {
            this.texture = texture;
            this.rectangle = rectangle;
            this.color = color;
        }

        public Texture2DRectangle(Texture2D texture, Rectangle rectangle) : this(texture, rectangle, Color.White) { }

        public void Render(SpriteBatch spriteBatch, Rectangle rectangle) {
            spriteBatch.Draw(texture, rectangle, this.rectangle, color, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
