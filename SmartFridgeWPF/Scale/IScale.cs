using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.Scale
{
    public interface IScale 
    {
        ulong getWeightInGrams();
    }
}
