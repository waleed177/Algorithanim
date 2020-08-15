using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlgorithmsAnimationEngine {
    public abstract class GameObject {
        private Vector2 backingfield_position;
        public Vector2 Position {
            get => backingfield_position;
            set {
                Vector2 delta = value - Position;
                backingfield_position = value;
                foreach (var child in children)
                    child.Position += delta;
                OnPositionChanged?.Invoke(this);
            }
        }
        public Vector2 LocalPosition {
            get => Position - Parent.Position;
            set {
                Position = Parent.Position + value;
            }
        }
        private Vector2 backingfield_size;
        public Vector2 Size {
            get => backingfield_size;
            set {
                backingfield_size = value;
                OnSizeChanged?.Invoke(this);
            }
        }
        public Rectangle IntBounds => new Rectangle((int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y);

        public GameObject Parent { get; private set; }

        public event Action<GameObject> OnPositionChanged;
        public event Action<GameObject> OnSizeChanged;

        protected List<GameObject> children;

        public GameObject() {
            children = new List<GameObject>();
        }

        public void AddChild(GameObject child) {
            child.Parent = this;
            children.Add(child);
        }

        public void RemoveChild(GameObject child) {
            child.Parent = null;
            children.Remove(child);
        }

        public abstract void OnRender(SpriteBatch spriteBatch, float deltaTime);

        public void Render(SpriteBatch spriteBatch, float deltaTime) {
            OnRender(spriteBatch, deltaTime);
            foreach (var item in children) {
                item.Render(spriteBatch, deltaTime);
            }
        }
    }
}