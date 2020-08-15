using System;
using System.Collections;
using System.IO;
using AlgorithmsAnimationEngine.GameObjects;
using AlgorithmsAnimationEngine.ScriptingLanguage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScriptingLanguage;

namespace AlgorithmsAnimationEngine {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont arial;
        private Texture2D whitePixel;
        private World world;
        private AlgorithmsAnimation animation;
        private KeyboardState previousKeyboardState;

        ArrayGameObject testObject;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            world = new World();
        }

        protected override void Initialize() {
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            arial = Content.Load<SpriteFont>("arial");
            whitePixel = Content.Load<Texture2D>("WhitePixel");

            world.Size = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            animation = new AlgorithmsAnimation(world, new Texture2DRectangle(whitePixel, new Rectangle(1, 1, 1, 1), Color.Gray), arial);
            int[] array = new int[] { 100, 20, 30, 50, 90, 0, 102 };

            //animation.PushFunctionStack("main");
            //animation.DeclareVariable("res");
            //animation.CreateArray("g_A", array.Length, 0);
            //for (int i = 0; i < array.Length; i++)
            //    animation.SetArrayValue("g_A", i, array[i]);
            //int res = AlgorithmsTesting.LasVegasRandomizedSelect(array, "g_A", 0, array.Length - 1, 2, animation);
            //animation.SetVariable("res", res);

            animation.PushFunctionStack("main");
            AnimationLanguage lang = new AnimationLanguage(animation);
            lang.LoadCode(File.ReadAllText(@"C:\Users\waldohp\source\repos\AlgorithmsAnimationEngine\AlgorithmsAnimationEngine\Algo.txt"));
            lang.Compile()();

            //animation.PopFunctionStack();
        }

        protected override void UnloadContent() {

        }

        protected override void Update(GameTime gameTime) {
            KeyboardState keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Space)) {
                animation.Enabled = true;
            }
            if(keyboardState.IsKeyDown(Keys.S) && !previousKeyboardState.IsKeyDown(Keys.S)) {
                animation.Step();
            }

            animation.Update(deltaTime);

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.Begin();
            world.Render(spriteBatch, deltaTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
