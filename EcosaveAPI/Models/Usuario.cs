namespace EcosaveAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        // Relacionamentos
        public ICollection<Endereco> Enderecos { get; set; }
        public ICollection<Ponto> Pontos { get; set; }
        public ICollection<Dispositivo> Dispositivos { get; set; }

        // Propriedade calculada para gasto médio
        public decimal GastoMedio
        {
            get
            {
                if (Dispositivos == null || Dispositivos.Count == 0)
                    return 0;

                return Dispositivos
                    .SelectMany(d => d.Consumos)
                    .Sum(c => c.Custo);
            }
        }
    }
}
