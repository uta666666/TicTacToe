using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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

        private bool _isWin;
        public bool IsWin {
            get {
                return _isWin;
            }
            set {
                if (_isWin == value)
                {
                    return;
                }
                SetProperty(ref _isWin, value);
            }
        }

        public bool SelectCell(int rowIndex, int colIndex)
        {
            Board.SetCellType(rowIndex, colIndex, _type);
            var resultSelf = Board.CheckGameStatus(_type);
            if (resultSelf.Status == GameStatus.Settlement)
            {
                Board.ChangeCellColorForWin((resultSelf as SettlementResult).SettlementCells);
                IsWin = true;
                return true;
            }
            return false;
        }

        public bool SelectCell()
        {
            var resultSelf = Board.CheckGameStatus(_type);
            if (resultSelf.Status == GameStatus.Reach)
            {
                var p = (resultSelf as ReachResult).ReachCells.Select(GetCellSelector());
                if (p.HasValue)
                {
                    return SelectCell((int)p.Value.Y, (int)p.Value.X);
                }
                return true;
            }

            var resultNonSelf = Board.CheckGameStatus(_nonSelfType);
            if (resultNonSelf.Status == GameStatus.Reach)
            {
                var p = (resultNonSelf as ReachResult).ReachCells.Select(GetCellSelector());
                if (p.HasValue)
                {
                    return SelectCell((int)p.Value.Y, (int)p.Value.X);
                }
                return true;
            }

            if (resultSelf.Status == GameStatus.None &&
                resultNonSelf.Status == GameStatus.None)
            {
                var p = Board.GetEmptyCells().Select(GetCellSelector());
                if (p.HasValue)
                {
                    return SelectCell((int)p.Value.Y, (int)p.Value.X);
                }
                return true;
            }

            throw new NotImplementedException("ありえない。実装ミス？");
        }

        private ICellSelector GetCellSelector()
        {
            return new WeightCellSelector(Board, _type);
        }
    }
}
