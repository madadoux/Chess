using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Chess.Units;


namespace Chess
{
    class Board
    {
        ContentManager CM;
        GraphicsDevice GD;
        Texture2D BoardPic, selectPic;
        private const string BoardPicName = "boardBG", selectImg = "Select";
        private Piece[,] gameBoard;
        private Vector2[,] squareCoord;
        private const int rows = 8, columns = 8;
        private bool isPressed = false;
        private bool kill = false;
        public bool turns = false;
        private int X1, Y1, X2, Y2;
        private /*const*/ Vector2 StartToDisplay = new Vector2(20, 20);
        private /*const*/ Vector2 cell_Width_Height = new Vector2(50, 50);
        private const int PreferredWidth = 440;
        private const int PreferredHeight = 440;


        public Board(ContentManager _CM, GraphicsDevice _GD)
        {
            CM = _CM;
            Piece.CM = _CM;
            GD = _GD;
            BoardPic = CM.Load<Texture2D>(@"Textures/" + BoardPicName);
            selectPic = CM.Load<Texture2D>(@"Textures/" + selectImg);
            setBoard();
            ResetBoard();
        }
        public void setBoard()
        {
            squareCoord = new Vector2[rows, columns];
            for (int i = 0; i < columns; i++)
                for (int j = 0; j < rows; j++)
                {
                    squareCoord[i, j].X = (StartToDisplay.X + (j * cell_Width_Height.X));
                    squareCoord[i, j].Y = (StartToDisplay.Y + (i * cell_Width_Height.Y));
                }
        }
        public void ResetBoard()
        {
            gameBoard = new Piece[rows, columns];

            // Black Units
            gameBoard[0, 0] = new Rock(12, 4, "bRock");
            gameBoard[0, 1] = new Knight(13, 3, "bKnight");
            gameBoard[0, 2] = new Bishop(14, 2, "bBishop");
            gameBoard[0, 3] = new Queen(15, 5, "bQueen");
            gameBoard[0, 4] = new King(16, 6, "bKing");
            gameBoard[0, 5] = new Bishop(14, 2, "bBishop");
            gameBoard[0, 6] = new Knight(13, 3, "bKnight");
            gameBoard[0, 7] = new Rock(12, 4, "bRock");

            gameBoard[1, 0] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 1] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 2] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 3] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 4] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 5] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 6] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 7] = new Pawn(11, 1, "bPawn");

            // White Units
            gameBoard[7, 0] = new Rock(2, 4, "wRock");
            gameBoard[7, 1] = new Knight(3, 3, "wKnight");
            gameBoard[7, 2] = new Bishop(4, 2, "wBishop");
            gameBoard[7, 3] = new Queen(5, 5, "wQueen");
            gameBoard[7, 4] = new King(6, 6, "wKing");
            gameBoard[7, 5] = new Bishop(4, 2, "wBishop");
            gameBoard[7, 6] = new Knight(3, 3, "wKnight");
            gameBoard[7, 7] = new Rock(2, 4, "wRock");

            gameBoard[6, 0] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 1] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 2] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 3] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 4] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 5] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 6] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 7] = new Pawn(1, 1, "wPawn");
        }
        public void Draw()
        {
            SpriteBatch SP = new SpriteBatch(GD);
            SP.Begin();
            SP.Draw(BoardPic,
                new Rectangle(0, 0, PreferredWidth, PreferredHeight),
                Color.White);
            for (int i = 0; i < columns; i++)
                for (int j = 0; j < rows; j++)
                {
                    if (isPressed)
                    {

                        if (gameBoard[j, i] == null && X1 >= 0 && Y2 >= 0 && move(X1, Y1, j, i))
                        {
                            SP.Draw(selectPic,
                                        new Rectangle((int)StartToDisplay.Y + i * (int)cell_Width_Height.X, (int)StartToDisplay.X + j * (int)cell_Width_Height.X,
                                            (int)cell_Width_Height.X, (int)cell_Width_Height.Y),
                                            Color.Red);
                        }
                    }
                    if (gameBoard[i, j] != null)
                    {
                        SP.Draw(gameBoard[i, j].gamePiece,
                            new Rectangle((int)squareCoord[i, j].X, (int)squareCoord[i, j].Y,
                                (int)cell_Width_Height.X, (int)cell_Width_Height.Y),
                                Color.White);
                    }
                }
            SP.End();
        }

        /*Vector2 on_Which_Cell(Vector2 MLoc)
        {
            float
             Tw = StartToDisplay.X + cell_Width_Height.X * columns,
             Th = StartToDisplay.Y + cell_Width_Height.Y * columns;

            if (MLoc.X >= StartToDisplay.X && MLoc.X <= Tw && MLoc.Y >= StartToDisplay.Y && MLoc.Y <= Th)
            {
                return new Vector2((int)Math.Round((MLoc.Y / Th) * (rows - 1)), (int)Math.Round(((MLoc.X / Tw) * (columns - 1))));
            }
            else return new Vector2(-1, -1);
        }*/
        public Vector2 on_Which_Cell(Vector2 mousepress)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (mousepress.X < (squareCoord[i, j].X + cell_Width_Height.X) && mousepress.Y < (squareCoord[i, j].Y + cell_Width_Height.Y))
                    {
                        return new Vector2(i, j);
                    }
                }
            }
            return new Vector2(-1, -1);
        }

        public Vector2 Handle_Input(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Vector2 point;
                if (isPressed == false)
                {
                    point.X = (int)on_Which_Cell(new Vector2(mouse.X, mouse.Y)).X;
                    point.Y = (int)on_Which_Cell(new Vector2(mouse.X, mouse.Y)).Y;
                    X1 = (int)point.X;
                    Y1 = (int)point.Y;
                    if (gameBoard[X1, Y1] != null)
                        isPressed = true;
                    return point;
                }
                else
                {
                    point.X = (int)on_Which_Cell(new Vector2(mouse.X, mouse.Y)).X;
                    point.Y = (int)on_Which_Cell(new Vector2(mouse.X, mouse.Y)).Y;
                    isPressed = false;
                    X2 = (int)point.X;
                    Y2 = (int)point.Y;
                    if (handle_Turns(X1, Y1, X2, Y2))
                        handle_Eating(X1, Y1, X2, Y2);

                    return point;
                }
            }
            return new Vector2(-1, -1);
        }

        public bool move(int oldRow, int oldCol, int newRow, int newCol)
        {
            if (oldRow == -1 || oldCol == -1 || newRow == -1 || newCol == -1)
                return false;
            kill = (gameBoard[newRow, newCol] != null);
            if (gameBoard[oldRow, oldCol] == null)
                return false;

            bool LegalMove = gameBoard[oldRow, oldCol].Move(oldRow, oldCol, newRow, newCol, kill);
            if (LegalMove)
            {
                if (gameBoard[oldRow, oldCol] != null && gameBoard[oldRow, oldCol].Type % 10 != 3)
                {
                    int
                        startRow = oldRow,
                        startCol = oldCol,
                        endRow = newRow,
                        endCol = newCol,
                        unitsNum = 0;
                    while (startRow != endRow || startCol != endCol)
                    {
                        if (gameBoard[startRow, startCol] != null)
                        {
                            unitsNum++;
                        }
                        if (startRow > endRow)
                            startRow--;
                        if (startRow < endRow)
                            startRow++;

                        if (startCol > endCol)
                            startCol--;
                        if (startCol < endCol)
                            startCol++;
                    }
                    if (unitsNum > 1)
                        return false;
                }
                return true;
            }
            return false;
        }

        public bool handle_Turns(int oldRow, int oldCol, int newRow, int newCol)
        {
            if (turns == false && gameBoard[oldRow, oldCol].Type < 10)
            {
                return move(oldRow, oldCol, newRow, newCol);
            }
            if (turns == true && gameBoard[oldRow, oldCol].Type > 10)
            {
                return move(oldRow, oldCol, newRow, newCol);
            }
            return false;
        }

        void commmitMove(int oldRow, int oldCol, int newRow, int newCol)
        {
            if ((gameBoard[oldRow, oldCol].Type == 11 || gameBoard[oldRow, oldCol].Type == 1))
            {
                gameBoard[oldRow, oldCol].PawnFirstMove = true;
            }
            gameBoard[newRow, newCol] = gameBoard[oldRow, oldCol];
            gameBoard[oldRow, oldCol] = null;

            if (turns == false)
                turns = true;
            else
                turns = false;
        }

        public void handle_Eating(int oldRow, int oldCol, int newRow, int newCol)
        {
            if (kill)
            {
                if ((gameBoard[oldRow, oldCol].Type > 10 && gameBoard[newRow, newCol].Type < 10) ||
                    (gameBoard[oldRow, oldCol].Type < 10 && gameBoard[newRow, newCol].Type > 10))
                {
                    commmitMove(oldRow, oldCol, newRow, newCol);
                }
            }
            else
            {
                commmitMove(oldRow, oldCol, newRow, newCol);
            }
            kill = false;
        }
    }
}