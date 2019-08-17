using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using TicTacToe.Core.Commons;
using TicTacToe.Core.Models;
using System.Windows.Media;

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
            _isFirstPlayerTurn = true;

            InitializeProperty();
            CreateCommand();
        }

        private Board _board;
        private Player _player1;
        private Player _player2;
        private bool _isFirstPlayerTurn;
        private Player CurrentPlayer {
            get {
                if (_isFirstPlayerTurn)
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

            Cell_1_1_Background = _board.Cells[0, 0].ObserveProperty(n => n.Background).ToReactiveProperty();
            Cell_1_1_Foreground = _board.Cells[0, 0].ObserveProperty(n => n.Foreground).ToReactiveProperty();
            Cell_1_2_Background = _board.Cells[0, 1].ObserveProperty(n => n.Background).ToReactiveProperty();
            Cell_1_2_Foreground = _board.Cells[0, 1].ObserveProperty(n => n.Foreground).ToReactiveProperty();
            Cell_1_3_Background = _board.Cells[0, 2].ObserveProperty(n => n.Background).ToReactiveProperty();
            Cell_1_3_Foreground = _board.Cells[0, 2].ObserveProperty(n => n.Foreground).ToReactiveProperty();
            Cell_2_1_Background = _board.Cells[1, 0].ObserveProperty(n => n.Background).ToReactiveProperty();
            Cell_2_1_Foreground = _board.Cells[1, 0].ObserveProperty(n => n.Foreground).ToReactiveProperty();
            Cell_2_2_Background = _board.Cells[1, 1].ObserveProperty(n => n.Background).ToReactiveProperty();
            Cell_2_2_Foreground = _board.Cells[1, 1].ObserveProperty(n => n.Foreground).ToReactiveProperty();
            Cell_2_3_Background = _board.Cells[1, 2].ObserveProperty(n => n.Background).ToReactiveProperty();
            Cell_2_3_Foreground = _board.Cells[1, 2].ObserveProperty(n => n.Foreground).ToReactiveProperty();
            Cell_3_1_Background = _board.Cells[2, 0].ObserveProperty(n => n.Background).ToReactiveProperty();
            Cell_3_1_Foreground = _board.Cells[2, 0].ObserveProperty(n => n.Foreground).ToReactiveProperty();
            Cell_3_2_Background = _board.Cells[2, 1].ObserveProperty(n => n.Background).ToReactiveProperty();
            Cell_3_2_Foreground = _board.Cells[2, 1].ObserveProperty(n => n.Foreground).ToReactiveProperty();
            Cell_3_3_Background = _board.Cells[2, 2].ObserveProperty(n => n.Background).ToReactiveProperty();
            Cell_3_3_Foreground = _board.Cells[2, 2].ObserveProperty(n => n.Foreground).ToReactiveProperty();

            IsFirstPlayer = new ReactiveProperty<bool>(true);

            IsWinPlayer1 = _player1.ObserveProperty(p => p.IsWin).ToReactiveProperty();
            IsWinPlayer2 = _player2.ObserveProperty(p => p.IsWin).ToReactiveProperty();
            IsSettled = IsWinPlayer1.CombineLatest(IsWinPlayer2, (x, y) => x || y).ToReadOnlyReactiveProperty();
            IsFirstPlayer.PropertyChanged += IsSettled_PropertyChanged;
        }

        private void IsSettled_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ;
            //if (IsSettled.Value)
            //{

            //    Cell_1_1_Background.Value = (Brush)App.Current.Resources["PrimaryHueBrush"];
            //    Cell_1_1_Foreground.Value = (Brush)App.Current.Resources["PrimaryHueForegroundBrush"];
            //}
        }

        private void CreateCommand()
        {
            CreateResetCommand();
            CreateCellCilckCommand();
            CreateChangePlayerCommand();
        }

        private void CreateResetCommand()
        {
            ResetCommand = new ReactiveCommand();
            ResetCommand.Subscribe(() =>
            {
                _board.Reset();
                _isFirstPlayerTurn = true;

                SelectCellForCPU();
            });
        }

        private void CreateChangePlayerCommand()
        {
            ChangePlayerCommand = new ReactiveCommand<bool>();
            ChangePlayerCommand.Subscribe((isFirst) =>
            {
                _board.Reset();
                _isFirstPlayerTurn = true;

                IsFirstPlayer.Value = isFirst;
                _player1.IsCPU = !isFirst;
                _player2.IsCPU = isFirst;

                SelectCellForCPU();
            });
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

                if (CurrentPlayer.SelectCell(rowIndex, colIndex))
                {
                    return;
                }
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
            _isFirstPlayerTurn = !_isFirstPlayerTurn;

            SelectCellForCPU();
        }

        private void SelectCellForCPU()
        {
            if (CurrentPlayer.IsCPU)
            {
                if (CurrentPlayer.SelectCell())
                {
                    return;
                }
                ChangePlayer();
            }
        }


        public ReactiveCommand<bool> ChangePlayerCommand { get; private set; }
        public ReactiveCommand<string> CellClickCommand { get; private set; }
        public ReactiveCommand ResetCommand { get; private set; }

        public ReactiveProperty<string> Cell_1_1 { get; set; }
        public ReactiveProperty<string> Cell_1_2 { get; set; }
        public ReactiveProperty<string> Cell_1_3 { get; set; }
        public ReactiveProperty<string> Cell_2_1 { get; set; }
        public ReactiveProperty<string> Cell_2_2 { get; set; }
        public ReactiveProperty<string> Cell_2_3 { get; set; }
        public ReactiveProperty<string> Cell_3_1 { get; set; }
        public ReactiveProperty<string> Cell_3_2 { get; set; }
        public ReactiveProperty<string> Cell_3_3 { get; set; }

        public ReactiveProperty<Brush> Cell_1_1_Background { get; set; }
        public ReactiveProperty<Brush> Cell_1_1_Foreground { get; set; }
        public ReactiveProperty<Brush> Cell_1_2_Background { get; set; }
        public ReactiveProperty<Brush> Cell_1_2_Foreground { get; set; }
        public ReactiveProperty<Brush> Cell_1_3_Background { get; set; }
        public ReactiveProperty<Brush> Cell_1_3_Foreground { get; set; }
        public ReactiveProperty<Brush> Cell_2_1_Background { get; set; }
        public ReactiveProperty<Brush> Cell_2_1_Foreground { get; set; }
        public ReactiveProperty<Brush> Cell_2_2_Background { get; set; }
        public ReactiveProperty<Brush> Cell_2_2_Foreground { get; set; }
        public ReactiveProperty<Brush> Cell_2_3_Background { get; set; }
        public ReactiveProperty<Brush> Cell_2_3_Foreground { get; set; }
        public ReactiveProperty<Brush> Cell_3_1_Background { get; set; }
        public ReactiveProperty<Brush> Cell_3_1_Foreground { get; set; }
        public ReactiveProperty<Brush> Cell_3_2_Background { get; set; }
        public ReactiveProperty<Brush> Cell_3_2_Foreground { get; set; }
        public ReactiveProperty<Brush> Cell_3_3_Background { get; set; }
        public ReactiveProperty<Brush> Cell_3_3_Foreground { get; set; }

        public ReactiveProperty<bool> IsFirstPlayer { get; set; }
        public ReactiveProperty<bool> IsWinPlayer1 { get; set; }
        public ReactiveProperty<bool> IsWinPlayer2 { get; set; }
        public ReadOnlyReactiveProperty<bool> IsSettled { get; set; }
    }
}
