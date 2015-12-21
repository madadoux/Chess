using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.MovingBehaviors
{
    class KnightMove:PieceMovingBehavior
    {
        public bool isLegalMove(int oldRow, int oldColumn, int newRow, int newColumn, bool kill)
        {
            int rowMove = Math.Abs(newRow - oldRow);
            int colMove = Math.Abs(newColumn - oldColumn);

            if (colMove == 2 && rowMove == 1)
                return true;
            if (colMove == 1 && rowMove == 2)
                return true;

            return false;
        }
    }
}
