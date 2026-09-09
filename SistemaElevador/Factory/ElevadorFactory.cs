using SistemaElevador.Enums;
using SistemaElevador.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaElevador.Factory {
    internal class ElevadorFactory {
        public ElevadorFactory() {
            
        }

        public Elevador Configurar() {
            return new Elevador {
                AndarAtual = 0,
                AndarMaximo = 10,
                MaximoPassageiros = 5,
                QuantidadePassageiros = 0,
                Status = StatusElevadorEnum.Parado,
                StatusPorta = StatusPortaEnum.Aberta,
                Rota = new List<int>()
            };
        }
    }
}
