using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.MovingBehaviors;

namespace Chess.Units
{
    class Pawn:Piece
    {
        public Pawn(int type, int weight, string name):base(type, weight, name, new PawnMove(type<10))
        {
        }
    }
}
