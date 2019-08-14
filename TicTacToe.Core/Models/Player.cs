using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{
    public class Player : BindableBase
    {
        public Player(CellType type)
        {
            Board = new Board();
            _type = type;

            if (_type == CellType.Circle)
            {
                _nonSelfType = CellType.Cross;
            }
            else if (_type == CellType.Cross)
            {
                _nonSelfType = CellType.Circle;
            }
            else
            {
                _nonSelfType = CellType.None;
            }
        }

        private CellType _type;
        private CellType _nonSelfType;

        public Board Board { get; set; }

        public bool IsCPU { get; set; }

        public void SelectCell(int rowIndex, int colIndex)
        {
            Board.SetCellType(rowIndex, colIndex, _type);
        }

        public void SelectCell()
        {
            int? spaceRowIndex;
            int? spaceColIndex;

            if (CheckNonSelfReach(out spaceRowIndex, out spaceColIndex))
            {
                SelectCell(spaceRowIndex.Value, spaceColIndex.Value);
                return;
            }

            var emptyCells = Board.GetEmptyCells().ToList();
            Random r = new Random();
            var randomIndex = r.Next(0, emptyCells.Count - 1);
            SelectCell((int)emptyCells[randomIndex].Y, (int)emptyCells[randomIndex].X);
        }

        private bool CheckNonSelfReach(out int? spaceRowIndex, out int? spaceColIndex) {
            return Board.CheckReach(_nonSelfType, out spaceRowIndex, out spaceColIndex);
        }
    }
}
