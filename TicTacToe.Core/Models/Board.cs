﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{
    public class Board
    {
        public Board()
        {
            Cells = new Cell[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Cells[i, j] = new Cell();
                }
            }
        }

        public Cell[,] Cells;

        public void SetCellType(int rowIndex, int colIndex, CellType type)
        {
            Cells[rowIndex, colIndex].Type = type;
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

        public bool CheckReach(CellType type, out int? spaceRowIndex, out int? spaceColIndex)
        {
            spaceRowIndex = null;
            spaceColIndex = null;

            if (CheckReachRow(type, out spaceRowIndex, out spaceColIndex))
            {
                return true;
            }
            if (CheckReachColumn(type, out spaceRowIndex, out spaceColIndex))
            {
                return true;
            }
            if (CheckReachCross(type, out spaceRowIndex, out spaceColIndex))
            {
                return true;
            }
            return false;
        }

        private bool CheckReachRow(CellType type, out int? spaceRowIndex, out int? spaceColIndex)
        {
            spaceRowIndex = null;
            spaceColIndex = null;

            for (int i = 0; i < 3; i++)
            {
                spaceRowIndex = null;
                spaceColIndex = null;
                int count = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j].Type == type)
                    {
                        count++;
                    }
                    else if (Cells[i, j].Type == CellType.None)
                    {
                        spaceRowIndex = i;
                        spaceColIndex = j;
                    }
                }
                if (count >= 2)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckReachColumn(CellType type, out int? spaceRowIndex, out int? spaceColIndex)
        {
            spaceRowIndex = null;
            spaceColIndex = null;

            for (int i = 0; i < 3; i++)
            {
                spaceRowIndex = null;
                spaceColIndex = null;
                int count = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[j, i].Type == type)
                    {
                        count++;
                    }
                    else if (Cells[i, j].Type == CellType.None)
                    {
                        spaceRowIndex = i;
                        spaceColIndex = j;
                    }
                }
                if (count >= 2)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckReachCross(CellType type, out int? spaceRowIndex, out int? spaceColIndex)
        {
            spaceRowIndex = null;
            spaceColIndex = null;
            int count = 0;
            if (Cells[0, 0].Type == type)
            {
                count++;
            }
            else if (Cells[0, 0].Type == CellType.None)
            {
                spaceRowIndex = 0;
                spaceColIndex = 0;
            }

            if (Cells[1, 1].Type == type)
            {
                count++;
            }
            else if (Cells[1, 1].Type == CellType.None)
            {
                spaceRowIndex = 1;
                spaceColIndex = 1;
            }

            if (Cells[2, 2].Type == type)
            {
                count++;
            }
            else if (Cells[2, 2].Type == CellType.None)
            {
                spaceRowIndex = 2;
                spaceColIndex = 2;
            }

            if (count >= 2)
            {
                return true;
            }


            spaceRowIndex = null;
            spaceColIndex = null;
            count = 0;
            if (Cells[0, 2].Type == type)
            {
                count++;
            }
            else if (Cells[0, 2].Type == CellType.None)
            {
                spaceRowIndex = 0;
                spaceColIndex = 2;
            }

            if (Cells[1, 1].Type == type)
            {
                count++;
            }
            else if (Cells[1, 1].Type == CellType.None)
            {
                spaceRowIndex = 1;
                spaceColIndex = 1;
            }

            if (Cells[2, 0].Type == type)
            {
                count++;
            }
            else if (Cells[2, 0].Type == CellType.None)
            {
                spaceRowIndex = 2;
                spaceColIndex = 0;
            }

            if (count >= 2)
            {
                return true;
            }

            return false;
        }
    }
}