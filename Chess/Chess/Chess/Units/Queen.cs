using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.MovingBehaviors;

namespace Chess.Units
{
    class Queen:Piece
    {
        public Queen(int type, int weight, string name):base(type, weight, name, new QueenMove())
        {
        }
    }
}
