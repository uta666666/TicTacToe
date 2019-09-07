﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{
    public class Player : BindableBase
    {
        public Player(CellType type)
        {
            Board = new Board();
            _selfType = type;
        }

        private CellType _selfType;

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

        private SettledPattern _settledPattern;
        public SettledPattern SettledPattern {
            get {
                return _settledPattern;
            }
            set {
                if (_settledPattern == value)
                {
                    return;
                }
                SetProperty(ref _settledPattern, value);
            }
        }

        /// <summary>
        /// セルを選択（プレイヤー、CPU共通）
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns>true:勝負あり　false:継続中</returns>
        public bool SelectCell(int rowIndex, int colIndex)
        {
            Board.SetCellType(rowIndex, colIndex, _selfType);
            var resultSelf = Board.CheckGameStatus(_selfType);
            if (resultSelf.Status == GameStatusByUser.Settlement)
            {
                var settlementResult = (resultSelf as SettlementResult);
                Board.ChangeCellColorForWin(settlementResult);
                IsWin = true;
                //SettledPattern = settlementResult.SettlementPattern;
                return true;
            }
            return false;
        }

        /// <summary>
        /// セルを選択（CPU）
        /// </summary>
        /// <returns>true:勝負あり　false:継続中</returns>
        public async Task<bool> SelectCellAutoAsync()
        {
            //セルを探す
            var p = await CallSelectorAsync(GetCellSelector(), Board.GetEmptyCells());
            //var p = await Board.GetEmptyCells().SelectAsync(GetCellSelector());
            if (p.HasValue)
            {
                return SelectCell((int)p.Value.Y, (int)p.Value.X);
            }
            //見つからないときは勝負あり
            return true;
        }

        private async Task<Point?> CallSelectorAsync(ICellSelector selector, IEnumerable<Point> cells)
        {
            if (!cells.Any())
            {
                return null;
            }
            return await selector.SelectAsync(cells);
        }


        private MinMaxFunctionSelector _minMaxSelector;
        private ICellSelector GetCellSelector()
        {
            //return new WeightCellSelector(Board, _type);
            if (_minMaxSelector == null)
            {
                _minMaxSelector = new MinMaxFunctionSelector(Board, _selfType, true);
            }
            return _minMaxSelector;
        }
    }
}
