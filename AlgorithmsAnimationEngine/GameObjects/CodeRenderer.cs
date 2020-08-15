using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlgorithmsAnimationEngine.GameObjects {
    class CodeRenderer : GameObject {

        public Color FontColor { get; set; } = Color.Black;
        public int HighlightedLine { get; set; }

        private Texture2DRectangle texture;
        private SpriteFont font;

        private string[] code = new string[] { };


        public CodeRenderer(Texture2DRectangle texture, SpriteFont font) {
            this.texture = texture;
            this.font = font;
        }

        public override void OnRender(SpriteBatch spriteBatch, float deltaTime) {
            texture.Render(spriteBatch, IntBounds);
            int numberOfLines = (int)Size.Y / 16;
            int start = Math.Max(0, HighlightedLine - numberOfLines/2);
            for (int i = start; i < Math.Min(code.Length, start + numberOfLines); i++) {

                spriteBatch.DrawString(font, (HighlightedLine - 1 == i ? ">" : "") + code[i], Position + new Vector2(0, (i - start) * 16), FontColor);

            }
        }

        public void SetCode(string code) {
            this.code = code.Replace("\r", "").Replace("\t", "    ").Split('\n');
        }

    }
}
