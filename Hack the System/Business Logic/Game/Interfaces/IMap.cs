using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Game.Interfaces
{
    public interface IMap
    {
        int ID { get; }
        int Score { get; }
        object MapData { get; }
        bool IsEnabled { get; }
    }
}
