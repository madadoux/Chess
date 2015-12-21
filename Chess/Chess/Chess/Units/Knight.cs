using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.MovingBehaviors;

namespace Chess.Units
{
    class Knight:Piece
    {
        public Knight(int type, int weight, string name):base(type, weight, name, new KnightMove())
        { 
        }
    }
}
