using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using TicTacToe.Core.Commons;
using System.Linq;

namespace TicTacToe.Core.Models
{
    public class Board : BindableBase
    {
        public Board()
        {
            Cells = new Cell[3, 3];
            Reset();
        }

        public void Reset()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j] == null)
                    {
                        Cells[i, j] = new Cell();
                    }
                    else
                    {
                        Cells[i, j].ResetCell();
                    }
                }
            }
            SettledPattern = SettledPattern.None;
        }

        public Cell[,] Cells { get; set; }


        private SettledPattern _settledPattern;
        public SettledPattern SettledPattern {
            get {
                return _settledPattern;
            }
            set {
                SetProperty(ref _settledPattern, value);
            }
        }



        public void SetCellType(int rowIndex, int colIndex, CellType type)
        {
            Cells[rowIndex, colIndex].Type = type;
        }

        public void Undo(int rowIndex, int colIndex)
        {
            Cells[rowIndex, colIndex].Type = CellType.None;
        }

        public IEnumerable<Point> GetEmptyCells()
        {
            return GetCells(CellType.None);
        }

        public IEnumerable<Point> GetCells(CellType type)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j].Type == type)
                    {
                        yield return new Point(j, i);
                    }
                }
            }
        }

        public CellType GetWinner()
        {
            GameStatus status;
            SettledPattern pattern;
            Cells.GetGameStatus(out status, out pattern);
            if (status == GameStatus.Win_Circle)
            {
                return CellType.Circle;
            }
            else if (status == GameStatus.Win_Cross)
            {
                return CellType.Cross;
            }
            else
            {
                return CellType.None;
            }
        }

        public ICheckGameStatusResult CheckGameStatus(CellType type)
        {
            return Cells.GetGameStatus(type, GetEmptyCells().Any());
        }

        public void ChangeCellColorForWin(SettlementResult result)
        {
            //foreach (var cell in result.SettlementCells)
            //{
            //    Cells[(int)cell.Y, (int)cell.X].ChangeCellColor();
            //}
            SettledPattern = result.SettlementPattern;
        }

        public Board Clone()
        {
            var cloneBoard = new Board();
            cloneBoard.Cells = new Cell[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    cloneBoard.Cells[i, j] = new Cell()
                    {
                        Type = Cells[i, j].Type
                    };
                }
            }
            return cloneBoard;
        }



        //public bool CheckReach(CellType type, out int? spaceRowIndex, out int? spaceColIndex)
        //{
        //    spaceRowIndex = null;
        //    spaceColIndex = null;

        //    if (CheckReachRow(type, out spaceRowIndex, out spaceColIndex))
        //    {
        //        return true;
        //    }
        //    if (CheckReachColumn(type, out spaceRowIndex, out spaceColIndex))
        //    {
        //        return true;
        //    }
        //    if (CheckReachCross(type, out spaceRowIndex, out spaceColIndex))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //private bool CheckReachRow(CellType type, out int? spaceRowIndex, out int? spaceColIndex)
        //{
        //    spaceRowIndex = null;
        //    spaceColIndex = null;

        //    for (int i = 0; i < 3; i++)
        //    {
        //        spaceRowIndex = null;
        //        spaceColIndex = null;
        //        int count = 0;
        //        for (int j = 0; j < 3; j++)
        //        {
        //            if (Cells[i, j].Type == type)
        //            {
        //                count++;
        //            }
        //            else if (Cells[i, j].Type == CellType.None)
        //            {
        //                spaceRowIndex = i;
        //                spaceColIndex = j;
        //            }
        //        }
        //        if (count >= 2)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private bool CheckReachColumn(CellType type, out int? spaceRowIndex, out int? spaceColIndex)
        //{
        //    spaceRowIndex = null;
        //    spaceColIndex = null;

        //    for (int i = 0; i < 3; i++)
        //    {
        //        spaceRowIndex = null;
        //        spaceColIndex = null;
        //        int count = 0;
        //        for (int j = 0; j < 3; j++)
        //        {
        //            if (Cells[j, i].Type == type)
        //            {
        //                count++;
        //            }
        //            else if (Cells[i, j].Type == CellType.None)
        //            {
        //                spaceRowIndex = i;
        //                spaceColIndex = j;
        //            }
        //        }
        //        if (count >= 2)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private bool CheckReachCross(CellType type, out int? spaceRowIndex, out int? spaceColIndex)
        //{
        //    spaceRowIndex = null;
        //    spaceColIndex = null;
        //    int count = 0;
        //    if (Cells[0, 0].Type == type)
        //    {
        //        count++;
        //    }
        //    else if (Cells[0, 0].Type == CellType.None)
        //    {
        //        spaceRowIndex = 0;
        //        spaceColIndex = 0;
        //    }

        //    if (Cells[1, 1].Type == type)
        //    {
        //        count++;
        //    }
        //    else if (Cells[1, 1].Type == CellType.None)
        //    {
        //        spaceRowIndex = 1;
        //        spaceColIndex = 1;
        //    }

        //    if (Cells[2, 2].Type == type)
        //    {
        //        count++;
        //    }
        //    else if (Cells[2, 2].Type == CellType.None)
        //    {
        //        spaceRowIndex = 2;
        //        spaceColIndex = 2;
        //    }

        //    if (count >= 2)
        //    {
        //        return true;
        //    }


        //    spaceRowIndex = null;
        //    spaceColIndex = null;
        //    count = 0;
        //    if (Cells[0, 2].Type == type)
        //    {
        //        count++;
        //    }
        //    else if (Cells[0, 2].Type == CellType.None)
        //    {
        //        spaceRowIndex = 0;
        //        spaceColIndex = 2;
        //    }

        //    if (Cells[1, 1].Type == type)
        //    {
        //        count++;
        //    }
        //    else if (Cells[1, 1].Type == CellType.None)
        //    {
        //        spaceRowIndex = 1;
        //        spaceColIndex = 1;
        //    }

        //    if (Cells[2, 0].Type == type)
        //    {
        //        count++;
        //    }
        //    else if (Cells[2, 0].Type == CellType.None)
        //    {
        //        spaceRowIndex = 2;
        //        spaceColIndex = 0;
        //    }

        //    if (count >= 2)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

    }
}
