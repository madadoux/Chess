using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Chess
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int PreferredWidth = 440;
        const int PreferredHeight = 440;

        Rectangle WhiteRect;
        Rectangle BlackRect;
        Rectangle MainScreen;

        Texture2D Board;
        Texture2D wPlayer;
        Texture2D bPlayer;
        Texture2D mains;
        Board board;

        SpriteFont font;
        string PVSP;
        string PVSC;
        Vector2 PVSPPosition;
        Vector2 PVSCPosition;
        Color buttoncolor;
        Color buttoncolor2;
        enum GameStates { mainscreen, ChoosingMenu, White, Black };
        GameStates gameState = GameStates.mainscreen;

        const float MinTimeSinceLastInput = 0.25f;
        float TimeSinceLastInput = 0.0f;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = PreferredWidth;
            graphics.PreferredBackBufferHeight = PreferredHeight;
            graphics.ApplyChanges();
            MainScreen = new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            WhiteRect = new Rectangle(0, 0, (this.Window.ClientBounds.Width / 2), this.Window.ClientBounds.Height);
            BlackRect = new Rectangle((this.Window.ClientBounds.Width / 2),
            0, (this.Window.ClientBounds.Width / 2),
            this.Window.ClientBounds.Height);
            PVSP = "Press 1 To Player VS Player";
            PVSC = "Press 2 To Player VS Computer";
            PVSPPosition = new Vector2(30, 30);
            PVSCPosition = new Vector2(20, 365);
            board = new Board(Content, GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>(@"Fonts/SpriteFont1");
            // TODO: use this.Content to load your game content here
            Board = Content.Load<Texture2D>(@"Textures/BoardBG");
            mains = Content.Load<Texture2D>(@"Textures/mainscreen");
            wPlayer = Content.Load<Texture2D>(@"Textures/wPlayer");
            bPlayer = Content.Load<Texture2D>(@"Textures/bPlayer");

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            MouseState mouse = Mouse.GetState();
            buttoncolor = new Color(0, 0, 0);
            buttoncolor2 = new Color(0, 0, 0);
           if (gameState == GameStates.mainscreen)
            {
                TimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
                KeyboardState keyState = Keyboard.GetState();
                // TODO: Add your update logic here
                if (mouse.X < PVSPPosition.X || mouse.Y < PVSPPosition.Y || mouse.X > PVSPPosition.X + font.MeasureString(PVSP).X ||
                    mouse.Y > PVSPPosition.Y + font.MeasureString(PVSP).Y)
                {
                    buttoncolor = new Color(0, 0, 0);
                }
                else
                {
                    buttoncolor = new Color(2, 22, 233);
                    if (TimeSinceLastInput >= MinTimeSinceLastInput)
                    {
                        if (keyState.IsKeyDown(Keys.NumPad1) || mouse.LeftButton == ButtonState.Pressed)
                        {
                            TimeSinceLastInput = 0.0f;
                            buttoncolor = new Color(8, 22, 233);
                            gameState = GameStates.ChoosingMenu;
                        }
                        else if (keyState.IsKeyDown(Keys.NumPad2))
                        {
                            buttoncolor2 = new Color(8, 22, 233);
                            gameState = GameStates.ChoosingMenu;
                        }
                    }
                }
            }
            if (gameState == GameStates.ChoosingMenu)
            {
                TimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TimeSinceLastInput >= MinTimeSinceLastInput)
                {
                    if ((mouse.LeftButton == ButtonState.Pressed) && WhiteRect.Contains(mouse.X, mouse.Y))
                    {
                        TimeSinceLastInput = 0.0f;
                        gameState = GameStates.White;
                    }
                    else if ((mouse.LeftButton == ButtonState.Pressed) && BlackRect.Contains(mouse.X, mouse.Y))
                    {
                        TimeSinceLastInput = 0.0f;
                        gameState = GameStates.Black;
                    }
                }
            }
            if (gameState == GameStates.Black || gameState == GameStates.White)
            {
                TimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TimeSinceLastInput >= MinTimeSinceLastInput)
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        TimeSinceLastInput = 0.0f;
                        this.Window.Title = mouse.X + " " + mouse.Y;
                        board.Handle_Input(mouse);
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (gameState == GameStates.mainscreen)
            {
                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin();
                spriteBatch.Draw(
                    mains,
                    MainScreen,
                    Color.White);
                spriteBatch.DrawString(font, PVSP, PVSPPosition, buttoncolor);
                spriteBatch.DrawString(font, PVSC, PVSCPosition, buttoncolor2);
                spriteBatch.End();
            }
            if (gameState == GameStates.ChoosingMenu)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(
                    wPlayer,
                    WhiteRect,
                    Color.White);
                spriteBatch.Draw(
                    bPlayer,
                    BlackRect,
                    Color.White);
                spriteBatch.End();
            }
            if (gameState == GameStates.White || gameState == GameStates.Black)
            {
                board.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
