using System;
using System.Collections.Generic;

namespace SneakerService.Domain;

public partial class Teni
{
    public string Nome { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public decimal ValorAtual { get; set; }

    public decimal ValorAnterior { get; set; }

    public int PercentualDesconto { get; set; }

    public int Id { get; set; }

    public DateTime DataIntegracao { get; set; }
}
