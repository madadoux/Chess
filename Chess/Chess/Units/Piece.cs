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

using Chess.MovingBehaviors;

namespace Chess
{
    class Piece
    {
        public static ContentManager CM;
        public Texture2D gamePiece;
        private int type;
        private int weight; 
        private string pieceName;
        private PieceMovingBehavior movingBehavior;
        public bool PawnFirstMove = false;

        public int Type
        {
            get { return type; }
        }
        public int Weight
        {
            get { return weight; }
        }
        protected Piece(int _type, int _weight, string _pieceName, PieceMovingBehavior _movingBehavior)
        {
            type = _type;
            weight = _weight;
            pieceName = _pieceName;
            movingBehavior = _movingBehavior;
            gamePiece = CM.Load<Texture2D>(@"Textures/" + pieceName);
        }

        public bool Move(int oldRow, int oldColumn, int newRow, int newColumn, bool kill)
        {
            if (movingBehavior.isLegalMove(oldRow, oldColumn, newRow, newColumn, kill))
            {
                if ((type == 11 || type == 1) && PawnFirstMove) 
                {
                    ((PawnMove)movingBehavior).Moved();
                }
                return true;
            }
            return false;
        }
    }
}
