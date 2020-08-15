using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlgorithmsAnimationEngine.GameObjects {
    public class StackFrame : GameObject {
        public string Name { get => textName.Value; set { textName.Value = value; } }

        public Color Font {
            get => textName.FontColor;
            set {
                foreach (var item in variables) {
                    item.FontColor = value;
                }
                textName.FontColor = value;
            }
        }

        private Text textName;

        private List<Variable> variables;

        public StackFrame(string name, Texture2DRectangle texture, SpriteFont font) {
            variables = new List<Variable>();
            AddChild(textName = new Text(name, texture, font) { Size = new Vector2(128,32) });
            Size = new Vector2(128, textName.Size.Y);
            OnSizeChanged += StackFrame_OnSizeChanged;
        }

        private void StackFrame_OnSizeChanged(GameObject obj) {
            
        }

        public void AddVariable(Variable variable) {
            variables.Add(variable);
            AddChild(variable);
            variable.Position = Position + new Vector2(0, Size.Y);
            variable.Size = new Vector2(Size.X, variable.Size.Y);
            Size += new Vector2(0, variable.Size.Y);
        }

        public override void OnRender(SpriteBatch spriteBatch, float deltaTime) {}
    }
}
