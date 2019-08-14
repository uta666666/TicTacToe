using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{
    public class Cell : BindableBase
    {
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
    }
}
