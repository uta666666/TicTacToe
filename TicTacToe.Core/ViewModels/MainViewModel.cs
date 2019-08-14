using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Core.Commons;
using TicTacToe.Core.Models;

namespace TicTacToe.Core.ViewModels
{
    public class MainViewModel : Livet.ViewModel
    {
        public MainViewModel()
        {
            _board = new Board();
            _player1 = new Player(CellType.Circle);
            _player1.Board = _board;
            _player2 = new Player(CellType.Cross);
            _player2.Board = _board;
            _player2.IsCPU = true;
            _isFirstPlayer = true;

            InitializeProperty();
            CreateCommand();
        }

        private Board _board;
        private Player _player1;
        private Player _player2;
        private bool _isFirstPlayer;
        private Player CurrentPlayer {
            get {
                if (_isFirstPlayer)
                {
                    return _player1;
                }
                else
                {
                    return _player2;
                }
            }
        }

        private void InitializeProperty()
        {
            Cell_1_1 = _board.Cells[0, 0].ObserveProperty(n => n.TypeString).ToReactiveProperty();
            Cell_1_2 = _board.Cells[0, 1].ObserveProperty(n => n.TypeString).ToReactiveProperty();
            Cell_1_3 = _board.Cells[0, 2].ObserveProperty(n => n.TypeString).ToReactiveProperty();
            Cell_2_1 = _board.Cells[1, 0].ObserveProperty(n => n.TypeString).ToReactiveProperty();
            Cell_2_2 = _board.Cells[1, 1].ObserveProperty(n => n.TypeString).ToReactiveProperty();
            Cell_2_3 = _board.Cells[1, 2].ObserveProperty(n => n.TypeString).ToReactiveProperty();
            Cell_3_1 = _board.Cells[2, 0].ObserveProperty(n => n.TypeString).ToReactiveProperty();
            Cell_3_2 = _board.Cells[2, 1].ObserveProperty(n => n.TypeString).ToReactiveProperty();
            Cell_3_3 = _board.Cells[2, 2].ObserveProperty(n => n.TypeString).ToReactiveProperty();
        }

        private void CreateCommand()
        {
            CreateCellCilckCommand();
        }

        private void CreateCellCilckCommand()
        {
            CellClickCommand = new ReactiveCommand<string>();
            CellClickCommand.Subscribe((tag) =>
            {
                int rowIndex;
                int colIndex;
                if (!TryGetPos(tag, out rowIndex, out colIndex))
                {
                    return;
                }

                if (_board.Cells[rowIndex, colIndex].Type != CellType.None)
                {
                    return;
                }

                CurrentPlayer.SelectCell(rowIndex, colIndex);
                ChangePlayer();
            });
        }

        private bool TryGetPos(string source, out int rowIndex, out int colIndex)
        {
            rowIndex = 0;
            colIndex = 0;

            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }

            var pos = source.Split(":");
            if (pos.Length != 2)
            {
                return false;
            }

            rowIndex = int.Parse(pos[0]);
            colIndex = int.Parse(pos[1]);
            return true;
        }

        private void ChangePlayer()
        {
            _isFirstPlayer = !_isFirstPlayer;

            if (CurrentPlayer.IsCPU)
            {
                CurrentPlayer.SelectCell();
                ChangePlayer();
            }
        }

        public ReactiveCommand<string> CellClickCommand { get; private set; }

        public ReactiveProperty<string> Cell_1_1 { get; set; }
        public ReactiveProperty<string> Cell_1_2 { get; set; }
        public ReactiveProperty<string> Cell_1_3 { get; set; }
        public ReactiveProperty<string> Cell_2_1 { get; set; }
        public ReactiveProperty<string> Cell_2_2 { get; set; }
        public ReactiveProperty<string> Cell_2_3 { get; set; }
        public ReactiveProperty<string> Cell_3_1 { get; set; }
        public ReactiveProperty<string> Cell_3_2 { get; set; }
        public ReactiveProperty<string> Cell_3_3 { get; set; }
    }
}
