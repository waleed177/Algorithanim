using AlgorithmsAnimationEngine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsAnimationEngine {
    public class AlgorithmsAnimation {
        public float variableChangedAnimationHalfTime = 0.1f;
        public float stackPushPopTime = 0.2f;
        public float arrayOneThirdSwapTime = 0.3f;

        private Animation animation;
        private GameObject world;
        private CodeRenderer code;
        private GameObject stackWorld;

        private Dictionary<string, GameObject> currentVariableObjects;
        private Dictionary<string, GameObject> globalVariables;
        private StackFrame currentStackFrameDisplay;

        private Texture2DRectangle texture;
        private SpriteFont font;

        private int numVariables;

        private Stack<StackFrameData> stack;
        float curStackHeight;

        public AlgorithmsAnimation(GameObject world, Texture2DRectangle texture, SpriteFont font) {
            animation = new Animation();
            stack = new Stack<StackFrameData>();
            globalVariables = new Dictionary<string, GameObject>();
            stackWorld = new World();
            world.AddChild(stackWorld);
            this.texture = texture;
            this.font = font;

            this.world = world;

            code = new CodeRenderer(texture, font);
            code.Size = new Vector2(world.Size.X / 2, world.Size.Y);
            code.Position = new Vector2(world.Size.X - code.Size.X, 0);

            world.AddChild(code);
        }

        public bool Enabled { get => animation.Enabled; set { animation.Enabled = value; } }

        public void SetCode(string code) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                this.code.SetCode(code);
                yield return null;
            }
        }

        public void SetCodeHighlight(int row, int column) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                this.code.HighlightedLine = row;
                yield return null;
            }
        }

        public void DeclareVariable(string var) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                Variable text = new Variable(var, "null", texture, font) {
                    Position = new Vector2(0, numVariables++ * 32),
                    Size = new Vector2(96, 32)
                };
                currentStackFrameDisplay.AddVariable(text);
                F_setVariable(var, text);
                AddStackHeight(text.Size.Y);
                yield return null;
            }
        }

        public void SetVariable(string var, string value) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                Variable text = ((Variable)F_getVariable(var));
                text.Value = value.ToString();
                text.FontColor = Color.Red;

                if (F_variableExists("pointer$" + var)) {
                    Text pointer = (Text)F_getVariable("pointer$" + var);
                    ArrayGameObject array = (ArrayGameObject)F_getVariable("pointer$" + var + "$array");
                    pointer.Position = new Vector2(array.Position.X + (array.ArrayItemSize + array.SpaceBetweenItems)*int.Parse(value), pointer.Position.Y);
                }

                for (float t = 0; t < variableChangedAnimationHalfTime; t += animation.DeltaTime) yield return null;
                text.FontColor = Color.Black;
                for (float t = 0; t < variableChangedAnimationHalfTime; t += animation.DeltaTime) yield return null;
            }
        }

        public void Step() {
            animation.Step();
        }

        //TODO put local arrays in the stack.
        public void CreateArray(string var, int arraySize, string value) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                ArrayGameObject arrayGameObject = new ArrayGameObject(arraySize, value.ToString(), texture, font);
                arrayGameObject.Position = new Vector2(132, 128); //TODO: no hardcoding
                world.AddChild(arrayGameObject);
                F_setVariable(var, arrayGameObject);
                yield return null;
            }
        }

        public void SetArrayValue(string var, int id, string value) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                ArrayGameObject arrayGameObject = (ArrayGameObject)F_getVariable(var);
                Text text = arrayGameObject.GetTextObject(id);
                text.Value = value.ToString();
                text.FontColor = Color.Red;
                for (float t = 0; t < variableChangedAnimationHalfTime; t += animation.DeltaTime) yield return null;
                text.FontColor = Color.Black;
                for (float t = 0; t < variableChangedAnimationHalfTime; t += animation.DeltaTime) yield return null;
                yield return null;
            }
        }

        public void SwapArrayValues(string var, int id1, int id2) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                ArrayGameObject arrayGameObject = (ArrayGameObject)F_getVariable(var);
                Text text1 = arrayGameObject.GetTextObject(id1);
                Text text2 = arrayGameObject.GetTextObject(id2);
                Vector2 Position1 = text1.Position;
                Vector2 Position2 = text2.Position;
                float deltaX = (Position2.X - Position1.X);

                for (float t = 0; t < 1; t += animation.DeltaTime / arrayOneThirdSwapTime) {
                    text1.Position = new Vector2(Position1.X, Position1.Y + 64 * t);
                    text2.Position = new Vector2(Position2.X, Position2.Y + 32 * t);
                    yield return null;
                }

                for (float t = 0; t < 1; t += animation.DeltaTime / arrayOneThirdSwapTime) {
                    text1.Position = new Vector2(Position1.X + deltaX * t, Position1.Y + 64);
                    text2.Position = new Vector2(Position2.X - deltaX * t, Position2.Y + 32);
                    yield return null;
                }

                {
                    string text1_temp = text1.Value;
                    text1.Value = text2.Value;
                    text2.Value = text1_temp;
                }

                for (float t = 1; t >= 0; t -= animation.DeltaTime / arrayOneThirdSwapTime) {
                    text1.Position = new Vector2(Position1.X, Position1.Y + 32 * t);
                    text2.Position = new Vector2(Position2.X, Position2.Y + 64 * t);
                    yield return null;
                }

                text1.Position = Position1;
                text2.Position = Position2;
            }
        }

        //Currently only local
        public void CreatePointer(string arrayName, string pointerName, int offsetY) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                Text pointer = new Text("^" + pointerName, texture, font) {
                    Size = new Vector2(8, 8)
                };

                ArrayGameObject array = (ArrayGameObject)F_getVariable(arrayName);
                Variable variable = (Variable) F_getVariable(pointerName);

                pointer.Position = array.Position + new Vector2(int.Parse(variable.Value)*(array.ArrayItemSize + array.SpaceBetweenItems), offsetY + array.ArrayItemSize);
                world.AddChild(pointer);

                stack.Peek().gameObjects.Add(pointer);
                F_setVariable("pointer$" + pointerName, pointer);
                F_setVariable("pointer$" + pointerName + "$array", array);
                Step(); Step();
                yield return null;
            }
        }

        public void RemovePointer(string arrayName, string pointerName) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                Text pointer =(Text) F_getVariable("pointer$" + pointerName);
                stack.Peek().gameObjects.Remove(pointer);
                pointer.Parent.RemoveChild(pointer);
                F_removeVariable("pointer$" + pointerName + "$array");
                F_removeVariable("pointer$" + pointerName);
                Step(); Step();
                yield return null;
            }
        }

        public void PushFunctionStack(string name) {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                currentVariableObjects = new Dictionary<string, GameObject>();
                currentStackFrameDisplay = new StackFrame(name, texture, font);
                stackWorld.AddChild(currentStackFrameDisplay);

                currentStackFrameDisplay.LocalPosition = new Vector2(0, curStackHeight);
                AddStackHeight(currentStackFrameDisplay.Size.Y + 8);

                stack.Push(new StackFrameData() {
                    display = currentStackFrameDisplay,
                    variableObjects = currentVariableObjects,
                    gameObjects = new List<GameObject>()
                });
                for (float t = 0; t < stackPushPopTime; t += animation.DeltaTime) yield return null;
            }

        }

        public void PopFunctionStack() {
            animation.AddAnimationIteratorGenerator(generator);

            IEnumerator generator(Animation animation) {
                currentStackFrameDisplay.Font = Color.Red;
                for (float t = 0; t < stackPushPopTime; t += animation.DeltaTime) yield return null;
                stackWorld.RemoveChild(currentStackFrameDisplay);
                AddStackHeight(-currentStackFrameDisplay.Size.Y - 8);
                if (stack.Count != 0) {
                    StackFrameData stackFrameData = stack.Peek();
                    foreach (var item in stackFrameData.gameObjects)
                        item.Parent.RemoveChild(item);
                    stack.Pop();
                }
                if (stack.Count != 0) {
                    StackFrameData data = stack.Peek();
                    currentVariableObjects = data.variableObjects;
                    currentStackFrameDisplay = data.display;
                }

                for (float t = 0; t < stackPushPopTime; t += animation.DeltaTime) yield return null;
            }
        }

        private void AddStackHeight(float height) {
            curStackHeight += height;
            FixStackYPosition();
        }

        private void FixStackYPosition() {
            if (curStackHeight + 64 >= world.Size.Y) {
                stackWorld.Position = new Vector2(0, world.Size.Y - (curStackHeight + 64));
            } else {
                stackWorld.Position = new Vector2(0, 0);
            }
        }

        public void Update(float deltaTime) {
            animation.Update(deltaTime);
        }



        private void F_setVariable(string var, GameObject value) {
            if (var.StartsWith("g_"))
                globalVariables[var] = value;
            else
                currentVariableObjects[var] = value;
        }

        private GameObject F_getVariable(string var) {
            if (var.StartsWith("g_"))
                return globalVariables[var];
            else
                return currentVariableObjects[var];
        }

        private void F_removeVariable(string var) {
            if (var.StartsWith("g_"))
                globalVariables.Remove(var);
            else
                currentVariableObjects.Remove(var);
        }

        private bool F_variableExists(string var) {
            if (var.StartsWith("g_"))
                return globalVariables.ContainsKey(var);
            else
                return currentVariableObjects.ContainsKey(var);
        }

        struct StackFrameData {
            public StackFrame display;
            public Dictionary<string, GameObject> variableObjects;
            public List<GameObject> gameObjects;
        }
    }
}
