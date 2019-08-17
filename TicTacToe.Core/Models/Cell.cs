using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{
    public class Cell : BindableBase
    {
        public Cell()
        {
            _background = (Brush)App.Current.Resources["PrimaryHueMidBrush"];
            _foreground = (Brush)App.Current.Resources["PrimaryHueMidForegroundBrush"];
        }

        private CellType _type = CellType.None;
        public CellType Type {
            get {
                return _type;
            }
            set {
                if (_type == value)
                {
                    return;
                }
                SetProperty(ref _type, value);
                RaisePropertyChanged(nameof(TypeString));
            }
        }

        public string TypeString {
            get {
                if (Type == CellType.Circle)
                {
                    return "○";
                }
                else if (Type == CellType.Cross)
                {
                    return "✕";
                }
                return string.Empty;
            }
        }

        private Brush _background;
        public Brush Background {
            get {
                return _background;
            }
            set {
                if (_background == value)
                {
                    return;
                }
                SetProperty(ref _background, value);
            }
        }

        private Brush _foreground;
        public Brush Foreground {
            get {
                return _foreground;
            }
            set {
                if (_foreground == value)
                {
                    return;
                }
                SetProperty(ref _foreground, value);
            }
        }

        public void ChangeCellColor()
        {
            Background = (Brush)App.Current.Resources["SecondaryAccentBrush"];
            Foreground = (Brush)App.Current.Resources["SecondaryAccentForegroundBrush"];
        }

        public void ResetCell()
        {
            Type = CellType.None;
            Background = (Brush)App.Current.Resources["PrimaryHueMidBrush"];
            Foreground = (Brush)App.Current.Resources["PrimaryHueMidForegroundBrush"];
        }
    }
}
