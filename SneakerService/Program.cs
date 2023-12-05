using System.Text.RegularExpressions;
using HtmlAgilityPack;
using SneakerService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp
{
    class Program
    {
        private readonly ILogger<Program> _logger;

        public Program(ILogger<Program> logger)
        {
            _logger = logger;
        }
        public void Run()
        {
            // Caminho para o arquivo HTML gerado pelo Python
            try
            {
                _logger.LogWarning("Iniciando Sneaker Service");
                Console.WriteLine("Iniciando teste");

                string pasta = @"D:\Workspace\SneakerService\Pages";
                string[] arquivos = Directory.GetFiles(pasta);
                string mensagem = string.Empty;

                if (arquivos.Length > 0 )
                {
                    foreach(var arquivo in arquivos)
                    {
                        string path = arquivo;

                        // Carrega o arquivo HTML
                        HtmlDocument htmlDoc = new HtmlDocument();
                        htmlDoc.Load(path);

                        // XPath da div pai
                        string divPaiXPath = "//div[@data-testid='products-search-3']";

                        // Seleciona a div pai usando o XPath
                        HtmlNode divPaiNode = htmlDoc.DocumentNode.SelectSingleNode(divPaiXPath);

                        if (divPaiNode != null)
                        {
                            // XPath para encontrar todas as divs filhas dentro da div pai
                            string divFilhaXPath = ".//div[contains(@class, 'ProductCard-styled__ProductContentContainer-sc')]";

                            // Seleciona todas as divs filhas usando o XPath
                            HtmlNodeCollection divsFilhas = divPaiNode.SelectNodes(divFilhaXPath);

                            if (divsFilhas != null)
                            {
                                // Percorre todas as divs filhas encontradas
                                foreach (HtmlNode divFilha in divsFilhas)
                                {
                                    string innerOutput = divFilha.InnerText;
                                    string textoFormatado = innerOutput.Replace("\r", "").Replace("\n", "");
                                    string textoFinal = Regex.Replace(textoFormatado, @"\s+", " ").Trim().Replace(".","").Replace("/ Skateboarding", "");

                                    string[] partes = textoFinal.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                    // Verifica se a primeira palavra é "tênis"
                                    if (partes.Length > 0 && partes[0].Equals("tênis", StringComparison.OrdinalIgnoreCase))
                                    {
                                        // Remove a primeira palavra do array
                                        partes = partes[1..];
                                    }

                                    string[] tiposTenis = {
                                        "Tênis", "Casual", "Corrida", "Chuteiras", "Basquete",
                                        "Treino & Academia", "Skateboarding", "Para Jogar Tênis"
                                    };

                                    // Encontrar o índice onde o tipo é um dos tipos disponíveis
                                    int indexTipo = partes.Select((value, index) => new { Value = value, Index = index })
                                                         .Where(x => tiposTenis.Contains(x.Value))
                                                         .Select(x => x.Index)
                                                         .DefaultIfEmpty(-1)
                                                         .Min();

                                    // Obter o nome do tênis
                                    string nomeTenis = string.Join(" ", partes.Take(indexTipo));

                                    // Remove o nome do tênis do array
                                    partes = partes.Skip(indexTipo).ToArray();

                                    SneakerDTO sneaker = new SneakerDTO();
                                    if(partes.Length == 3)
                                    {
                                        sneaker.Nome = nomeTenis;
                                        sneaker.Tipo = partes[0];
                                        sneaker.ValorAtual = decimal.Parse(partes[2]);
                                        sneaker.ValorAnterior = 0;
                                        sneaker.Desconto = 0;
                                        sneaker.DataHoraIntegracao = DateTime.Now;
                                    }
                                    else if(partes.Length >= 7)
                                    {
                                        sneaker.Nome = nomeTenis;
                                        sneaker.Tipo = partes[0];
                                        sneaker.ValorAtual = decimal.Parse(partes[2]);
                                        sneaker.ValorAnterior = decimal.Parse(partes[4]);
                                        sneaker.Desconto = int.Parse(partes[5].Replace("%",""));
                                        sneaker.DataHoraIntegracao = DateTime.Now;
                                    }

                                    new SneakerFT().InserirOuAtualizar(sneaker, out mensagem);
                                    _logger.LogInformation(mensagem);

                                }
                            }
                            else
                            {
                                _logger.LogError("Nenhuma div filha encontrada dentro da div pai.");
                            }

                            // Deleta o arquivo
                            Console.WriteLine("Arquivo lido com sucesso, executando exclusão.");
                            Console.WriteLine(string.Format("ARQUIVO: {0}", arquivo));
                            File.Delete(path);
                        }
                        else 
                        {
                            _logger.LogError("Div pai não encontrada no arquivo HTML.");
                        }
                    }
                }

                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders(); 
                    logging.AddConsole();       
                })
                .Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            var program = new Program(logger);
            program.Run();
        }
    }
}