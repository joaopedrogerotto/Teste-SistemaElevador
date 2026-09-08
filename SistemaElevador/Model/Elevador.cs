using SistemaElevador.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaElevador.Model {
    internal class Elevador {
        public int AndarAtual { get; set; }
        public int AndarMaximo { get; set; }
        public int QuantidadePassageiros { get; set; }
        public StatusElevadorEnum Status {  get; set; }
        public StatusPortaEnum StatusPorta { get; set; }
        public List<int> Rota { get; set; }
        
        public void EmbarcarPassageiro() {
            if(Status != StatusElevadorEnum.Parado || StatusPorta == StatusPortaEnum.Fechada) {
                throw new InvalidOperationException("Não é possível embarcar passageiros enquanto o elevador está em movimento ou com a porta fechada.");
            }

            QuantidadePassageiros++;
        }
    }
}
