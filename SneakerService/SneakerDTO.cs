using SneakerService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakerService
{
    public class SneakerDTO
    {
        public SneakerDTO() { }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public decimal ValorAtual { get; set; }
        public decimal ValorAnterior { get; set; }
        public int Desconto { get; set; }
        public DateTime DataHoraIntegracao { get; set; }

        public SneakerDTO(Teni tenis)
        {
            this.Nome = tenis.Nome;
            this.Tipo = tenis.Tipo;
            this.ValorAtual = tenis.ValorAtual;
            this.ValorAnterior = tenis.ValorAnterior;
            this.Desconto = tenis.PercentualDesconto;
            this.DataHoraIntegracao = tenis.DataIntegracao;
        }
    }
}
