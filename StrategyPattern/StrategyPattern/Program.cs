using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EstrategiasDelBorrachoContexto oBorracho = new EstrategiasDelBorrachoContexto();
            oBorracho.Conquistar(EstrategiasDelBorrachoContexto.Comportamiento.HacerOjitos);
            oBorracho.Conquistar(EstrategiasDelBorrachoContexto.Comportamiento.InvitarCerveza);
            oBorracho.Conquistar(EstrategiasDelBorrachoContexto.Comportamiento.HacerCaraDeGalan);
        }
    }
}
