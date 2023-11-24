using System;
using HtmlAgilityPack;
using SneakerService;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Caminho para o arquivo HTML gerado pelo Python
            string path = @"D:\Workspace\Web Scraping\html_nike2.html"; // Substitua pelo caminho do seu arquivo

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
                            sneaker.Desconto = linhas.Length < 12 ? "Produto não possui desconto" : linhas[14].Trim();

                            // Exibe os dados atribuídos ao objeto de teste
                            Console.WriteLine("Nome: " + sneaker.Nome);
                            Console.WriteLine("Tipo: " + sneaker.Tipo);
                            Console.WriteLine("Valor Atual: " + sneaker.ValorAtual);
                            Console.WriteLine("Valor Original: " + sneaker.ValorAnterior);
                            Console.WriteLine("Desconto: " + sneaker.Desconto);
                            Console.WriteLine("----------------------------------------------------------------");
                        }

                        #region ok
                        //// deu certo
                        //HtmlNodeCollection teste = divFilha.SelectNodes(textoXPath);
                        //if(teste != null)
                        //{
                        //    foreach(var x in teste)
                        //    {
                        //        // Imprime o conteúdo da tag <p>
                        //        Console.WriteLine("Conteúdo da Tag <p>:");
                        //        Console.WriteLine(x.InnerHtml);
                        //        Console.WriteLine("-------------------------------------------");
                        //    }
                        //}
                        #endregion

                        #region ok2
                        //HtmlNodeCollection tagsP = divFilha.SelectNodes(tagsPXPath);

                        //if (tagsP != null)
                        //{
                        //    // Percorre todas as tags <p> encontradas
                        //    foreach (HtmlNode tagP in tagsP)
                        //    {
                        //        // Extrai o conteúdo da tag <p>
                        //        string tagPContent = tagP.InnerText;

                        //        // Imprime o conteúdo da tag <p>
                        //        Console.WriteLine("Conteúdo da Tag <p>:");
                        //        Console.WriteLine(tagPContent);
                        //        Console.WriteLine("-------------------------------------------");
                        //    }
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Nenhuma tag <p> encontrada dentro da segunda div filha.");
                        //}
                        #endregion
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
    }
}