using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Core.Commons;

namespace TicTacToe.Core.Models
{    public interface ICheckGameStatusResult
    {
        GameStatusByUser Status { get; }
    }
}
