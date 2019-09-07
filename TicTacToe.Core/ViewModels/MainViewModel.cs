using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using TicTacToe.Core.Commons;
using TicTacToe.Core.Models;
using System.Windows.Media;
using System.Threading.Tasks;

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
            IsSettledCL = _board.ObserveProperty(n => n.SettledPattern).Select(n => n == Commons.SettledPattern.CrossLeft).ToReactiveProperty();
            IsSettledCR = _board.ObserveProperty(n => n.SettledPattern).Select(n => n == Commons.SettledPattern.CrossRight).ToReactiveProperty();
            IsSettledVL = _board.ObserveProperty(n => n.SettledPattern).Select(n => n == Commons.SettledPattern.VerticalLeft).ToReactiveProperty();
            IsSettledVC = _board.ObserveProperty(n => n.SettledPattern).Select(n => n == Commons.SettledPattern.VerticalCenter).ToReactiveProperty();
            IsSettledVR = _board.ObserveProperty(n => n.SettledPattern).Select(n => n == Commons.SettledPattern.VerticalRight).ToReactiveProperty();
            IsSettledHT = _board.ObserveProperty(n => n.SettledPattern).Select(n => n == Commons.SettledPattern.HorizontalTop).ToReactiveProperty();
            IsSettledHM = _board.ObserveProperty(n => n.SettledPattern).Select(n => n == Commons.SettledPattern.HorizontalMiddle).ToReactiveProperty();
            IsSettledHB = _board.ObserveProperty(n => n.SettledPattern).Select(n => n == Commons.SettledPattern.HorizontalBottom).ToReactiveProperty();
            //IsSettledCL = IsSettled.CombineLatest(SettledPattern, (x, y) => x && (y == Commons.SettledPattern.CrossLeft)).ToReadOnlyReactiveProperty();
            //IsSettledCR = IsSettled.CombineLatest(SettledPattern, (x, y) => x && (y == Commons.SettledPattern.CrossRight)).ToReadOnlyReactiveProperty();
            //IsSettledVL = IsSettled.CombineLatest(SettledPattern, (x, y) => x && (y == Commons.SettledPattern.VerticalLeft)).ToReadOnlyReactiveProperty();
            //IsSettledVC = IsSettled.CombineLatest(SettledPattern, (x, y) => x && (y == Commons.SettledPattern.VerticalCenter)).ToReadOnlyReactiveProperty();
            //IsSettledVR = IsSettled.CombineLatest(SettledPattern, (x, y) => x && (y == Commons.SettledPattern.VerticalRight)).ToReadOnlyReactiveProperty();
            //IsSettledHT = IsSettled.CombineLatest(SettledPattern, (x, y) => x && (y == Commons.SettledPattern.HorizontalTop)).ToReadOnlyReactiveProperty();
            //IsSettledHM = IsSettled.CombineLatest(SettledPattern, (x, y) => x && (y == Commons.SettledPattern.HorizontalMiddle)).ToReadOnlyReactiveProperty();
            //IsSettledHB = IsSettled.CombineLatest(SettledPattern, (x, y) => x && (y == Commons.SettledPattern.HorizontalBottom)).ToReadOnlyReactiveProperty();

            IsThinking = new ReactiveProperty<bool>(false);
        }

        private void CreateCommand()
        {
            CreateResetCommand();
            CreateChangePlayerCommand();
            CreateCellCilckCommand();
        }

        /// <summary>
        /// リセットコマンド
        /// </summary>
        private void CreateResetCommand()
        {
            ResetCommand = new ReactiveCommand();
            ResetCommand.Subscribe(async () =>
            {
                _board.Reset();
                _isFirstPlayerTurn = true;
                _player1.IsWin = false;
                _player2.IsWin = false;

                await SelectCellForCPUAsync();
            });
        }

        /// <summary>
        /// 先攻・後攻切り替え
        /// </summary>
        private void CreateChangePlayerCommand()
        {
            SelectPlayerCommand = new ReactiveCommand<bool>();
            SelectPlayerCommand.Subscribe(async (isFirstPlayer) =>
            {
                if (IsFirstPlayer.Value == isFirstPlayer)
                {
                    return;
                }

                _board.Reset();
                _isFirstPlayerTurn = true;
                _player1.IsWin = false;
                _player2.IsWin = false;

                IsFirstPlayer.Value = isFirstPlayer;
                _player1.IsCPU = !isFirstPlayer;
                _player2.IsCPU = isFirstPlayer;

                await SelectCellForCPUAsync();
            });
        }

        /// <summary>
        /// セル選択
        /// </summary>
        private void CreateCellCilckCommand()
        {
            CellClickCommand = new ReactiveCommand<string>();
            CellClickCommand.Subscribe(async (tag) =>
            {
                if (IsSettled.Value)
                {
                    return;
                }
                if (IsThinking.Value)
                {
                    return;
                }

                int rowIndex;
                int colIndex;
                if (!TryGetPos(tag, out rowIndex, out colIndex))
                {
                    return;
                }
                //すでに選択済みの場所はダメ
                if (_board.Cells[rowIndex, colIndex].Type != CellType.None)
                {
                    return;
                }
                //選択して決着がついた窯で確認
                if (CurrentPlayer.SelectCell(rowIndex, colIndex))
                {
                    return;
                }
                //決着がついてないときは次のプレイヤーへ
                await ChangePlayerAsync();
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

        private async Task ChangePlayerAsync()
        {
            _isFirstPlayerTurn = !_isFirstPlayerTurn;

            await SelectCellForCPUAsync();
        }

        private async Task SelectCellForCPUAsync()
        {
            if (CurrentPlayer.IsCPU)
            {
                //IsThinking.Value = true;
                var t = StartThinking();
                try
                {
                    if (await CurrentPlayer.SelectCellAutoAsync())
                    {
                        return;
                    }
                    await ChangePlayerAsync();
                }
                finally
                {
                    await t;
                    IsThinking.Value = false;
                }
            }
        }

        private Task StartThinking()
        {
            return Task.Run(async () =>
            {
                await Task.Delay(1000);
                IsThinking.Value = true;
            });
        }


        public ReactiveCommand<bool> SelectPlayerCommand { get; private set; }
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
        public ReactiveProperty<bool> IsSettledCL { get; set; }
        public ReactiveProperty<bool> IsSettledCR { get; set; }
        public ReactiveProperty<bool> IsSettledVL { get; set; }
        public ReactiveProperty<bool> IsSettledVC { get; set; }
        public ReactiveProperty<bool> IsSettledVR { get; set; }
        public ReactiveProperty<bool> IsSettledHT { get; set; }
        public ReactiveProperty<bool> IsSettledHM { get; set; }
        public ReactiveProperty<bool> IsSettledHB { get; set; }
        private ReactiveProperty<SettledPattern> SettledPattern { get; set; }

        public ReactiveProperty<bool> IsThinking { get; set; }
    }
}
