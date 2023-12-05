using Microsoft.EntityFrameworkCore;
using SneakerService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakerService
{
    public class SneakerFT
    {
        public bool InserirOuAtualizar(SneakerDTO sneaker, out string mensagem)
        {
            mensagem = string.Empty;

            try
            {
                using (Contexto cntx = new Contexto())
                {
                    Teni tenis = cntx.Tenis.FirstOrDefault(x => x.Nome.ToUpper() == sneaker.Nome.ToUpper());

                    if (tenis != null && (tenis.ValorAtual != sneaker.ValorAtual))
                    {
                        Teni novoTenis = new Teni()
                        {
                            Nome = sneaker.Nome.ToUpper(),
                            DataIntegracao = sneaker.DataHoraIntegracao,
                            PercentualDesconto = sneaker.Desconto,
                            Tipo = sneaker.Tipo.ToUpper(),
                            ValorAnterior = sneaker.ValorAnterior,
                            ValorAtual = sneaker.ValorAtual
                        };

                        cntx.Tenis.Add(novoTenis);
                        cntx.SaveChanges();

                        mensagem = string.Format("Sneaker {0} foi inserido com sucesso na base de dados!", sneaker.Nome.ToUpper());
                        return true;
                    }
                    else if (tenis == null)
                    {
                        Teni novoTenis = new Teni()
                        {
                            Nome = sneaker.Nome.ToUpper(),
                            DataIntegracao = sneaker.DataHoraIntegracao,
                            PercentualDesconto = sneaker.Desconto,
                            Tipo = sneaker.Tipo.ToUpper(),
                            ValorAnterior = sneaker.ValorAnterior,
                            ValorAtual = sneaker.ValorAtual
                        };

                        cntx.Tenis.Add(novoTenis);
                        cntx.SaveChanges();

                        mensagem = string.Format("Sneaker {0} sem registros anteriores foi salvo com sucesso na base de dados!", sneaker.Nome.ToUpper());
                        return true;
                    }
                    else
                    {
                        mensagem = string.Format("Não há diferença nos valores para atualizar o sneaker {0}.", sneaker.Nome.ToUpper());
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                mensagem = "Sneaker error: "+ sneaker.Nome + ex.Message;
                return false;
            }
        }

    }
}
