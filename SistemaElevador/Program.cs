using SistemaElevador.Factory;

ElevadorFactory factory = new ElevadorFactory();
var elevador = factory.Configurar();

string menu = 
@"Elevador 
    
1 - Embarcar Passageiro
2 - Desembarcar Passageiro
3 - Selecionar Andar
4 - Proxima Parada
5 - Fechar Porta
6 - Abrir Porta
7 - Sair
";

int opcao = 0;

do {
    Console.Clear();

    try {
        Console.WriteLine(menu);
        Console.WriteLine(ExibirSituacaoElevador());
        opcao = int.Parse(Console.ReadLine());

        switch (opcao) {
            case 1:
                elevador.EmbarcarPassageiro();
                break;
            case 2:
                elevador.DesembarcarPassageiro();
                break;
            case 3:
                elevador.SelecionarAndar();
                break;
            case 4:
                elevador.Movimentar();
                break;
            case 5:
                elevador.FecharPorta();
                break;
            case 6:
                elevador.AbrirPorta();
                break;
            case 7:
                break;
            default:
                Console.WriteLine("Digite uma opção válida.");
                break;
        }

    }catch(FormatException ex) {
        Console.WriteLine("Digite uma opção valida");
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    } catch (Exception ex) {
        Console.WriteLine(ex.Message);
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }
} while (opcao != 7);

string ExibirSituacaoElevador() {
    return $@"ELEVADOR
Passageiros: {elevador.QuantidadePassageiros}
Status: {elevador.Status}
Status Porta: {elevador.StatusPorta}
Andar Atual: {elevador.AndarAtual}
Rota: {string.Join(", ", elevador.Rota)}
Andares Visitados: {string.Join(", ", elevador.AndaresVisitados)}
";
}