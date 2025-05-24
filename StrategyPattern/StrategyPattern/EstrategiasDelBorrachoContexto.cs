using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPattern
{
    internal class EstrategiasDelBorrachoContexto
    {
        private IBorracho oBorracho;

        public enum Comportamiento
        {
            HacerOjitos,
            InvitarCerveza,
            HacerCaraDeGalan
        }

        public EstrategiasDelBorrachoContexto() 
        {
            this.oBorracho = new EstrategiaOjitos();
        }

        public void Conquistar(Comportamiento opcion)
        {
            switch (opcion)
            {
                case Comportamiento.HacerOjitos:
                    {
                        this.oBorracho = new EstrategiaOjitos();
                    } break;

                case Comportamiento.InvitarCerveza:
                    {
                        this.oBorracho = new EstrategiaInvitarCerveza();
                    } break;
                case Comportamiento.HacerCaraDeGalan:
                    {
                        this.oBorracho = new EstrategiaHacerCaraDeGalan();
                    } break;
            }
            this.oBorracho.Conquistar();
        }
    }
}
