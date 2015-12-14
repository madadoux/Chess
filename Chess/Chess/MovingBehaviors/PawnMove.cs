using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.MovingBehaviors
{
    class PawnMove:PieceMovingBehavior
    {
        bool firstMove;
        bool upperDirection;

        public void Moved()
        {
            firstMove = false;
        }

        public PawnMove(bool direction)
        {
            upperDirection = direction;
            firstMove = true;
        }

        public bool isLegalMove(int oldRow, int oldColumn, int newRow, int newColumn, bool kill)
        {
            int rowMove = newRow - oldRow;
            int colMove = newColumn - oldColumn;
            if(kill)
            {
                if (Math.Abs(colMove)==1 && Math.Abs(rowMove) == 1)
                {
                    if(upperDirection && rowMove<0)
                        return true;
                    if(!upperDirection && rowMove>0)
                        return true;
                }
            }
            else
            {
                if (Math.Abs(colMove) > 0)
                    return false;
                if(firstMove)
                {
                    if (Math.Abs(rowMove) == 1 || Math.Abs(rowMove) == 2)
                    {
                        if (upperDirection && rowMove < 0)
                            return true;
                        if (!upperDirection && rowMove > 0)
                            return true;
                    }
                }
                else
                {
                    if (Math.Abs(rowMove) == 1)
                    {
                        if (upperDirection && rowMove < 0)
                            return true;
                        if (!upperDirection && rowMove > 0)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
