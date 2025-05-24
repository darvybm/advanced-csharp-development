using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPattern
{
    internal class EstrategiaHacerCaraDeGalan : IBorracho
    {
        public void Conquistar()
        {
            Console.WriteLine("> Hacer cara de galan");
        }
    }
}
