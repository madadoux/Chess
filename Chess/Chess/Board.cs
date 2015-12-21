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
        private Piece[,] gameBoard;
        private Vector2[,] squareCoord;
        private const int rows = 8, columns = 8;
        private bool isPressed = false, kill = false, turns = false, MoveIt = false, WisChecked = false, BisChecked = false;
        public bool wCheck = false, bCheck = false;
        private int X1, Y1, X2, Y2;
        private int wKingRow, wKingCol, bKingRow, bKingCol;
        private Vector2 StartToDisplay = new Vector2(20, 20);
        private Vector2 cell_Width_Height = new Vector2(50, 50);
        private const int PreferredWidth = 440;
        private const int PreferredHeight = 440;
        public int whitedeath = 0;
        public int blackdeath = 0;
        UserInput textMessage;
        public Board(ContentManager _CM, GraphicsDevice _GD)
        {
            CM = _CM;
            Piece.CM = _CM;
            GD = _GD;
            BoardPic = CM.Load<Texture2D>(@"Textures/boardBG");
            selectPic = CM.Load<Texture2D>(@"Textures/Select");
            setBoard();
            ResetBoard();
        }
        public void setBoard()
        {
            squareCoord = new Vector2[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    squareCoord[i, j].X = (StartToDisplay.X + (j * cell_Width_Height.X));
                    squareCoord[i, j].Y = (StartToDisplay.Y + (i * cell_Width_Height.Y));
                }
        }
        public void ResetBoard()
        {
            isPressed = false; kill = false; turns = false;
            MoveIt = false; WisChecked = false; BisChecked = false;
            whitedeath = 0; blackdeath = 0;

            gameBoard = new Piece[rows, columns];

            // Black Units
            gameBoard[0, 0] = new Rock(12, 4, "bRock");
            gameBoard[0, 1] = new Knight(13, 2, "bKnight");
            gameBoard[0, 2] = new Bishop(14, 3, "bBishop");
            gameBoard[0, 3] = new Queen(15, 5, "bQueen");
            gameBoard[0, 4] = new King(16, 6, "bKing");
            gameBoard[0, 5] = new Bishop(14, 3, "bBishop");
            gameBoard[0, 6] = new Knight(13, 2, "bKnight");
            gameBoard[0, 7] = new Rock(12, 4, "bRock");

            gameBoard[1, 0] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 1] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 2] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 3] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 4] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 5] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 6] = new Pawn(11, 1, "bPawn");
            gameBoard[1, 7] = new Pawn(11, 1, "bPawn");

            bKingRow = 0; bKingCol = 4;

            // White Units
            gameBoard[7, 0] = new Rock(2, 4, "wRock");
            gameBoard[7, 1] = new Knight(3, 2, "wKnight");
            gameBoard[7, 2] = new Bishop(4, 3, "wBishop");
            gameBoard[7, 3] = new Queen(5, 5, "wQueen");
            gameBoard[7, 4] = new King(6, 6, "wKing");
            gameBoard[7, 5] = new Bishop(4, 3, "wBishop");
            gameBoard[7, 6] = new Knight(3, 2, "wKnight");
            gameBoard[7, 7] = new Rock(2, 4, "wRock");

            gameBoard[6, 0] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 1] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 2] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 3] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 4] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 5] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 6] = new Pawn(1, 1, "wPawn");
            gameBoard[6, 7] = new Pawn(1, 1, "wPawn");

            wKingRow = 7; wKingCol = 4;
        }
        public void Draw()
        {
            SpriteBatch SP = new SpriteBatch(GD);
            SP.Begin();
            SP.Draw(BoardPic,
                new Rectangle(0, 0, PreferredWidth, PreferredHeight),
                Color.White);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    if (isPressed)
                    {
                        if (gameBoard[i, j] == null && X1 >= 0 && Y2 >= 0 && move(X1, Y1, i, j))
                        {
                            SP.Draw(selectPic,
                                   new Rectangle((int)squareCoord[i, j].X, (int)squareCoord[i, j].Y,
                                        (int)cell_Width_Height.X, (int)cell_Width_Height.Y),
                                         Color.Red);
                        }
                        if(gameBoard[i, j] != null && X1 >= 0 && Y2 >= 0 && move(X1, Y1, i, j))
                        {
                            if (turns == true && gameBoard[i, j].Type < 10)
                            {
                                SP.Draw(selectPic,
                                   new Rectangle((int)squareCoord[i, j].X, (int)squareCoord[i, j].Y,
                                        (int)cell_Width_Height.X, (int)cell_Width_Height.Y),
                                         Color.Red);
                            }
                            else if (turns == false && gameBoard[i, j].Type > 10)
                            {
                                SP.Draw(selectPic,
                                   new Rectangle((int)squareCoord[i, j].X, (int)squareCoord[i, j].Y,
                                        (int)cell_Width_Height.X, (int)cell_Width_Height.Y),
                                         Color.Red);
                            }
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

            if (wCheck)
            {
                SP.Draw(selectPic,
                                   new Rectangle((int)squareCoord[wKingRow, wKingCol].X, (int)squareCoord[wKingRow, wKingCol].Y,
                                        (int)cell_Width_Height.X, (int)cell_Width_Height.Y),
                                         Color.Red);
            }
            if (bCheck)
            {
                SP.Draw(selectPic,
                                   new Rectangle((int)squareCoord[bKingRow, bKingCol].X, (int)squareCoord[bKingRow, bKingCol].Y,
                                        (int)cell_Width_Height.X, (int)cell_Width_Height.Y),
                                         Color.Red);
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
        private Vector2 on_Which_Cell(Vector2 mousepress)
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
            Vector2 point;

            point.X = (int)on_Which_Cell(new Vector2(mouse.X, mouse.Y)).X;
            point.Y = (int)on_Which_Cell(new Vector2(mouse.X, mouse.Y)).Y;

            if (point.X != -1)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (isPressed == false)
                    {
                        MoveIt = false; WisChecked = false; BisChecked = false;
                        X1 = (int)point.X;
                        Y1 = (int)point.Y;
                        if (gameBoard[X1, Y1] != null)
                        {
                            if (turns == false && gameBoard[X1, Y1].Type < 10)
                                isPressed = true;
                            else if (turns == true && gameBoard[X1, Y1].Type > 10)
                                isPressed = true;
                        }
                        return point;
                    }
                    else
                    {
                        isPressed = false;
                        X2 = (int)point.X;
                        Y2 = (int)point.Y;
                        if (move(X1, Y1, X2, Y2))
                            handle_Eating(X1, Y1, X2, Y2);

                        return point;
                    }
                }
            }
            return new Vector2(-1, -1);
        }

        private bool move(int oldRow, int oldCol, int newRow, int newCol)
        {
            if (oldRow == -1 || oldCol == -1 || newRow == -1 || newCol == -1)
                return false;

            kill = (gameBoard[newRow, newCol] != null);

            bool LegalMove = gameBoard[oldRow, oldCol].Move(oldRow, oldCol, newRow, newCol, kill);
            if (LegalMove)
            {
                if (gameBoard[oldRow, oldCol].Type % 10 != 3)
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

        private void commmitMove(int oldRow, int oldCol, int newRow, int newCol)
        {
            if (wCheck)
                WisChecked = true;
            if (bCheck)
                BisChecked = true;

            Piece tmp = gameBoard[newRow, newCol];
            gameBoard[newRow, newCol] = gameBoard[oldRow, oldCol];
            gameBoard[oldRow, oldCol] = null;

            if (gameBoard[newRow, newCol].Type == 6)
            {
                wKingRow = newRow;
                wKingCol = newCol;
            }
            if (gameBoard[newRow, newCol].Type == 16)
            {
                bKingRow = newRow;
                bKingCol = newCol;
            }

            if (isChecked())
            {
                if (WisChecked)
                    bCheck = false;
                if (BisChecked)
                    wCheck = false;
                if (wCheck)
                {
                    if (turns == false || gameBoard[newRow, newCol].Type == 16)
                    {
                        gameBoard[oldRow, oldCol] = gameBoard[newRow, newCol];
                        gameBoard[newRow, newCol] = tmp;
                        MoveIt = true;
                    }
                }
                if (bCheck)
                {
                    if (turns == true)
                    {
                        gameBoard[oldRow, oldCol] = gameBoard[newRow, newCol];
                        gameBoard[newRow, newCol] = tmp;
                        MoveIt = true;
                    }
                }
            }
            if (MoveIt == false)
            {
                if ((gameBoard[newRow, newCol].Type == 11 || gameBoard[newRow, newCol].Type == 1))
                {
                    gameBoard[newRow, newCol].PawnFirstMove = true;
                }
                if (tmp!=null && turns == false)
                {
                    blackdeath++;
                }
                else if (tmp!=null && turns == true)
                {
                    whitedeath++;
                }

                if (turns == false)
                    turns = true;
                else
                    turns = false;
                Promotion();              
            }
            else
            {
                if (gameBoard[oldRow, oldCol].Type == 6)
                {
                    wKingRow = oldRow;
                    wKingCol = oldCol;
                }
                if (gameBoard[oldRow, oldCol].Type == 16)
                {
                    bKingRow = oldRow;
                    bKingCol = oldCol;
                }
            }
        }

        private void handle_Eating(int oldRow, int oldCol, int newRow, int newCol)
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
            //kill = false;
        }

        public bool isChecked()
        {
            wCheck = false; bCheck = false;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    if (gameBoard[i, j] != null)
                    {
                        if (gameBoard[i, j].Type < 10)
                        {
                            if (move(i, j, bKingRow, bKingCol))
                            {
                                bCheck = true;
                            }
                        }
                        if (gameBoard[i, j].Type > 10)
                        {
                            if (move(i, j, wKingRow, wKingCol))
                            {
                                wCheck = true;
                            }
                        }
                    }
                }
            if (wCheck || bCheck)
                return true;

            wCheck = false; bCheck = false;
            return false;
        }

        public void Promotion()
        {
            for (int j = 0; j < columns; j++)
            {
                if (gameBoard[0, j] != null)
                {
                    if (gameBoard[0, j].Type == 1)
                    {
                            textMessage = new UserInput();
                            textMessage.ShowDialog();
                            string mycoice = textMessage.Data;
                            if (mycoice.Equals("K") || mycoice.Equals("k"))
                            {
                                gameBoard[0, j] = new Knight(3, 3, "wKnight");
                                break;
                            }
                            else if (mycoice.Equals("q") || mycoice.Equals("Q"))
                            {
                                gameBoard[0, j] = new Queen(5, 5, "wQueen");
                                break;
                            }
                            else if (mycoice.Equals("R") || mycoice.Equals("r"))
                            {
                                gameBoard[0, j] = new Rock(2, 4, "wRock");
                                break;
                            }
                            else if (mycoice.Equals("B") || mycoice.Equals("b"))
                            {
                                gameBoard[0, j] = new Bishop(4, 2, "wBishop");
                                break;
                            }
                            else
                            {
                                gameBoard[0, j] = new Pawn(1, 1, "wPawn");
                                break;
                            }
                        }
                }
            }
            for (int j = 0; j < columns; j++)
            {
                if (gameBoard[7,j] != null)
                {
                    if (gameBoard[7, j].Type == 11)
                    {
                            textMessage = new UserInput();
                            textMessage.ShowDialog();
                            string mycoice = textMessage.Data;
                            if (mycoice.Equals("K") || mycoice.Equals("k"))
                            {
                                gameBoard[7, j] = new Knight(13, 3, "bKnight");
                                break;
                            }
                            else if (mycoice.Equals("q") || mycoice.Equals("Q"))
                            {
                                gameBoard[7, j] = new Queen(15, 5, "bQueen");
                                break;
                            }
                            else if (mycoice.Equals("R") || mycoice.Equals("r"))
                            {
                                gameBoard[7, j] = new Rock(12, 4, "bRock");
                                break;
                            }
                            else if (mycoice.Equals("b") || mycoice.Equals("B"))
                            {
                                gameBoard[7, j] = new Bishop(14, 2, "bBishop");
                                break;
                            }
                            else
                            {
                                gameBoard[7, j] = new Pawn(11, 1, "bPawn");
                                break;
                            }
                        }
                    }
                }
            }
    }
}

