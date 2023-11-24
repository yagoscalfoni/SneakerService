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
        public string ValorAtual { get; set; }
        public string ValorAnterior { get; set; }
        public string Desconto { get; set; }
    }
}
