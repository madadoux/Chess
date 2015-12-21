using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.MovingBehaviors
{
    class KingMove:PieceMovingBehavior
    {
        public bool isLegalMove(int oldRow, int oldColumn, int newRow, int newColumn, bool kill)
        {
            int rowMove = Math.Abs(newRow - oldRow);
            int colMove = Math.Abs(newColumn - oldColumn);

            if (rowMove == 1 && colMove == 1)
                return true;
            if (rowMove == 0 && colMove == 1)
                return true;
            if (rowMove == 1 && colMove == 0)
                return true;

            return false;
        }
    }
}
