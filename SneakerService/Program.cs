using HtmlAgilityPack;
using MongoDB.Driver;
using SneakerService;
using System.Security.Authentication;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Caminho para o arquivo HTML gerado pelo Python
            string path = @"D:\Workspace\Web Scraping\html_nike2.html";

            string pathWorkspace = @"C:\Pessoal\WebScraping\html_nike2.html";

            // Carrega o arquivo HTML
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load(pathWorkspace);

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
                        string tagsPXPath = ".//p";
                        string textoXPath = ".//p[contains(@class, 'Typography')]";

                        string innerOutput = divFilha.InnerText;
                        string[] linhas = innerOutput.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

                        SneakerDTO sneaker = new SneakerDTO();
                        
                        if(linhas.Length >= 5)
                        {
                            sneaker.Nome = linhas[1].Trim();
                            sneaker.Tipo = linhas[4].Trim();
                            sneaker.ValorAtual = linhas[8].Trim();
                            sneaker.ValorAnterior = linhas[11].Trim();
                            sneaker.Desconto = linhas.Length <= 12 ? "Produto não possui desconto" : linhas[14].Trim().Replace("%", "").Replace("off", "") + "% off";

                            // Exibe os dados atribuídos ao objeto de teste
                            Console.WriteLine("Nome: " + sneaker.Nome);
                            Console.WriteLine("Tipo: " + sneaker.Tipo);
                            Console.WriteLine("Valor Atual: " + sneaker.ValorAtual);
                            Console.WriteLine("Valor Original: " + sneaker.ValorAnterior);
                            Console.WriteLine("Desconto: " + sneaker.Desconto);
                            Console.WriteLine("----------------------------------------------------------------");
                        }

                        // Enviar objeto para o Mongo
                        EnviarParaMongoDB(sneaker);
                    }
                }
                else
                {
                    Console.WriteLine("Nenhuma div filha encontrada dentro da div pai.");
                }
            }
            else
            {
                Console.WriteLine("Div pai não encontrada no arquivo HTML.");
            }
        }
        static void EnviarParaMongoDB(SneakerDTO sneaker)
        {
            // Configurar a conexão com o MongoDB
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress("localhost", 27017);

            settings.UseTls = false;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoClient client = new MongoClient(settings);

            IMongoDatabase database = client.GetDatabase("SneakerService");
            IMongoCollection<SneakerDTO> collection = database.GetCollection<SneakerDTO>("sneakerdb");

            // Inserir o objeto SneakerDTO na coleção
            collection.InsertOne(sneaker);
        }
    }
}