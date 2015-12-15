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
    public enum ch
    {
        quit, reset, player1, player2
    }

    public delegate void onClick();

    public delegate void onHover();

    public class stringBtn
    {
        public string lable;
        public Vector2 loc;
        public Color col ;

        Color _def;
        public Color defcolor
        {
            set
            {
                _def = value;
                col = _def;
            }
            get { return _def; }
        }
        
        public stringBtn(string _lab, Vector2 _loc, Color _col)
        {
            lable = _lab;
            loc = _loc;
            defcolor = _col; 
            col = _col;
        }

        public onClick onClicked;
        public onHover onHovered;
    }


    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int PreferredWidth = 600;
        const int PreferredHeight = 440;

        Rectangle WhiteRect;
        Rectangle BlackRect;
        Rectangle MainScreen;
        Rectangle scorePanleContainer;


        Texture2D ScorePanel;
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

        const float MinTimeSinceLastInput = 0.15f;
        float TimeSinceLastInput = 0.0f;

        Dictionary<ch, stringBtn> StringButtons = new Dictionary<ch, stringBtn>();

        public void quite()
        {
            this.Exit();
        }

        public void reset()
        {
            board.ResetBoard();
        }


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

            // mada sys
            scorePanleContainer = new Rectangle(440, 0, 160, 440);

            StringButtons.Add(ch.player1, new stringBtn("Player1 ", new Vector2(450, 100), Color.White));
            StringButtons.Add(ch.player2, new stringBtn("Player2", new Vector2(450, 200), Color.White));
            StringButtons.Add(ch.reset, new stringBtn("Reset ", new Vector2(450, 300), Color.White));
            StringButtons.Add(ch.quit, new stringBtn("Quit ", new Vector2(450, 325), Color.White));

            StringButtons[ch.quit].onClicked = () => quite();
            StringButtons[ch.reset].onClicked = () => reset();

            PVSPPosition = new Vector2(120, 30);
            PVSCPosition = new Vector2(110, 365);
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

            ScorePanel = Content.Load<Texture2D>(@"Textures/leftPnl");

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
                KeyboardState keyState = Keyboard.GetState();
                // TODO: Add your update logic here

                if (keyState.IsKeyDown(Keys.NumPad1))
                {
                    buttoncolor = new Color(8, 22, 233);
                    gameState = GameStates.White;
                }
                else if (keyState.IsKeyDown(Keys.NumPad2))
                {
                    buttoncolor2 = new Color(8, 22, 233);
                    gameState = GameStates.ChoosingMenu;
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
                Color cl = new Color();

                if (gameState == GameStates.Black)
                    cl = Color.Black;
                else
                    cl = Color.White;

                StringButtons[ch.player1].defcolor = cl;

                if (cl == Color.White)
                    StringButtons[ch.player2].defcolor = Color.Black;
                else
                    StringButtons[ch.player2].defcolor = Color.White;


                TimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (TimeSinceLastInput >= MinTimeSinceLastInput)
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        TimeSinceLastInput = 0.0f;
                        //this.Window.Title = mouse.X + " " + mouse.Y;
                        board.Handle_Input(mouse);
                        if (board.isChecked())
                        {
                            if (board.wCheck)
                                this.Window.Title = "Check White King";
                            else if (board.bCheck)
                                this.Window.Title = "Check Black King";
                        }
                        else
                            this.Window.Title = "toz";
                    }

                    foreach (var item in StringButtons)
                    {
                        // hover 
                        if (isStringHovered(item.Value))
                        {
                            item.Value.col = Color.Blue;
                            if (item.Value.onHovered != null)
                                item.Value.onHovered.Invoke();
                        }

                        else { item.Value.col = item.Value.defcolor; }

                        // click 
                        if (isStringClicked(item.Value))
                        {
                            item.Value.col = Color.Yellow;

                            if (item.Value.onClicked != null)
                                item.Value.onClicked();

                            TimeSinceLastInput = 0.0f;
                        }
                    }
                }
            }
            base.Update(gameTime);
        }
        bool isStringHovered(stringBtn stb)
        {
            var mouse = Mouse.GetState();

            var strP = stb.loc;
            var str = stb.lable;

            if (mouse.X >= strP.X && mouse.Y >= strP.Y &&
                mouse.X <= strP.X + font.MeasureString(str).X &&
                mouse.Y <= strP.Y + font.MeasureString(str).Y)
            {
                return true;
            }
            else return false;
        }

        bool isStringClicked(stringBtn stb)
        {
            var mouse = Mouse.GetState();
            var strP = stb.loc;
            var str = stb.lable;

            if (mouse.X >= strP.X && mouse.Y >= strP.Y &&
                mouse.X <= strP.X + font.MeasureString(str).X &&
                mouse.Y <= strP.Y + font.MeasureString(str).Y
               && (mouse.LeftButton == ButtonState.Pressed))
            {
                return true;
            }

            else
                return false;
        }
        void drawScorepanel()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                ScorePanel,
                scorePanleContainer,
                Color.White);

            foreach (var item in StringButtons)
                spriteBatch.DrawString(font, item.Value.lable, item.Value.loc, item.Value.col);

            spriteBatch.End();
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
                drawScorepanel();
            }
            base.Draw(gameTime);
        }
    }
}
