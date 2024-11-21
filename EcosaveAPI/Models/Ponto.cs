namespace EcosaveAPI.Models
{
    public class Ponto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public decimal ValorPonto { get; set; }
        public string Descricao { get; set; }

        // Relacionamento com Usuario
        public Usuario Usuario { get; set; }
    }
}
