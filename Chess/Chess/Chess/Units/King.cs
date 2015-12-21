using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.MovingBehaviors;

namespace Chess.Units
{
    class King:Piece
    {
        public King(int type, int weight, string name):base(type, weight, name, new KingMove())
        {
        }
    }
}
