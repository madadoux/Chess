using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    interface PieceMovingBehavior
    {
        bool isLegalMove(int oldRow, int oldColumn, int newRow, int newColumn, bool kill);
    }
}
