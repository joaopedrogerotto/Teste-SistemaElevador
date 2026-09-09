using SistemaElevador.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SistemaElevador.Model {
    internal class Elevador {
        public int AndarAtual { get; set; }
        public int AndarMaximo { get; set; }
        public int MaximoPassageiros { get; set; }
        public int QuantidadePassageiros { get; set; }
        public StatusElevadorEnum Status {  get; set; }
        public StatusPortaEnum StatusPorta { get; set; }
        public List<int> Rota { get; set; }
        public List<int> AndaresVisitados { get; set; }
        
        public void EmbarcarPassageiro() {
            if(Status != StatusElevadorEnum.Parado || StatusPorta == StatusPortaEnum.Fechada) {
                throw new InvalidOperationException("Não é possível embarcar passageiros enquanto o elevador está em movimento ou com a porta fechada.");
            }

            QuantidadePassageiros++;
        }

        public void DesembarcarPassageiro() {
            if (Status != StatusElevadorEnum.Parado || StatusPorta == StatusPortaEnum.Fechada) {
                throw new InvalidOperationException("Não é possível desembarcar passageiros enquanto o elevador está em movimento ou com a porta fechada.");
            }

            if (QuantidadePassageiros <= 0) {
                throw new InvalidOperationException("Não há passageiros para desembarcar.");
            }

            QuantidadePassageiros--;
        }

        public void SelecionarAndar() {
            try {
                if (StatusPorta == StatusPortaEnum.Fechada) {
                    throw new InvalidOperationException("Não é possível selecionar um andar com a porta fechada.");
                }

                Console.WriteLine("Digite o andar desejado");

                string andarInput = Console.ReadLine();

                if (string.IsNullOrEmpty(andarInput)) {
                    throw new ArgumentException("O andar selecionado é inválido.");
                }

                int andarSelecionado = int.Parse(andarInput);

                if (Rota.Contains(andarSelecionado)) {
                    throw new InvalidOperationException("Não é possível adicionar um andar já selecionado na rota.");
                }

                if (AndaresVisitados.Contains(andarSelecionado)) {
                    throw new InvalidOperationException("Não é possível adicionar um andar já visitado.");
                }

                if (andarSelecionado < 0 || andarSelecionado > AndarMaximo) {
                    throw new InvalidOperationException("O andar selecionado é inválido.");
                }

                if (AndarAtual == andarSelecionado) {
                    return;
                }

                DefinirRota(andarSelecionado);
            } catch (FormatException fEx) {
                throw new FormatException("Informe um numero de andar válido");
            }
        }

        private void DefinirRota(int andarSelecionado) {
            if (QuantidadePassageiros > MaximoPassageiros) {
                throw new InvalidOperationException("Não é possível movimentar o elevador com mais passageiros do que o máximo permitido.");
            }

            if (Status == StatusElevadorEnum.Subindo) {
                InserirSubida(andarSelecionado);
            } else if(Status == StatusElevadorEnum.Descendo) {
                InserirDescida(andarSelecionado);
            }else if(Status == StatusElevadorEnum.Parado) {
                InserirParado(andarSelecionado);
            }
        }
        
        private void InserirParado(int andarSelecionado) {
            if(!Rota.Any()) {
                Rota.Add(andarSelecionado);
                return;
            }

            if (Rota.First() > AndarAtual) {

                Rota.Add(andarSelecionado);

                var acima = Rota.Where(a => a > AndarAtual).OrderBy(a => a);

                var abaixo = Rota.Where(a => a < AndarAtual).OrderByDescending(a => a);

                Rota = acima.Concat(abaixo).ToList();
            } else {
                Rota.Add(andarSelecionado);

                var abaixo = Rota.Where(a => a < AndarAtual).OrderByDescending(a => a);

                var acima = Rota.Where(a => a > AndarAtual).OrderBy(a => a);

                Rota = abaixo.Concat(acima).ToList();
            }
        }

        private void InserirSubida(int andarSelecionado) {
            var frenteRota = Rota.Where(a => a > AndarAtual).ToList();
            var atrasRota = Rota.Where(a => a <= AndarAtual).ToList();

            if(andarSelecionado > AndarAtual) {
                frenteRota.Add(andarSelecionado);
                frenteRota.Sort();
            } else {
                atrasRota.Add(andarSelecionado);
                atrasRota.Sort((a,b) => b.CompareTo(a)); //Ordem decrescente
            }

            Rota = frenteRota.Concat(atrasRota).ToList();
        }


        private void InserirDescida(int andarSelecionado) {
            var frenteRota = Rota.Where(a => a < AndarAtual).ToList();
            var atrasRota = Rota.Where(a => a >= AndarAtual).ToList();

            if (andarSelecionado < AndarAtual) {
                frenteRota.Add(andarSelecionado);
                frenteRota.Sort((a, b) => b.CompareTo(a));
            } else {
                atrasRota.Add(andarSelecionado);
                atrasRota.Sort();
            }

            Rota = frenteRota.Concat(atrasRota).ToList();
        }

        public void FecharPorta() {
            if (!Rota.Any() && AndarAtual == 0) {
                throw new InvalidOperationException("Não é possível fechar a porta enquanto não há destino definido.");
            }

            if(StatusPorta == StatusPortaEnum.Fechada) {
                throw new InvalidOperationException("A porta já está fechada.");
            }

            if (AndarAtual < Rota.First()) {
                Status = StatusElevadorEnum.Subindo;
            } else {
                Status = StatusElevadorEnum.Descendo;
            }

            StatusPorta = StatusPortaEnum.Fechada;
            Console.WriteLine("Porta fechada.");
        }

        public void AbrirPorta() {
            if(Status != StatusElevadorEnum.Parado) {
                throw new InvalidOperationException("Não é possível abrir a porta enquanto o elevador está em movimento.");
            }

            StatusPorta = StatusPortaEnum.Aberta;
            Console.WriteLine("Porta aberta.");
        }

        public void Movimentar() {
            
            if(StatusPorta != StatusPortaEnum.Fechada) {
                throw new InvalidOperationException("Não é possível movimentar o elevador com a porta aberta.");
            }

            if (!Rota.Any()) {
                throw new InvalidOperationException("Rota não definida");
            }

            if (Status == StatusElevadorEnum.Subindo) {
                AndarAtual++;
            } else if (Status == StatusElevadorEnum.Descendo) {
                AndarAtual--;
            }



            if (AndarAtual == Rota.First()) {
                Rota.RemoveAt(0);
                Status = StatusElevadorEnum.Parado;
                AndaresVisitados.Add(AndarAtual);
                Console.WriteLine($"Andar {AndarAtual} visitado. Desembarque os passageiros.");
                Console.WriteLine("\nPressione ENTER para continuar...");
                Console.ReadLine();
                AbrirPorta();
            }
        }

        public void Parar() {
            Status = StatusElevadorEnum.Parado;
            Console.WriteLine($"Elevador parado no andar {AndarAtual}.");
        }
    }
}
