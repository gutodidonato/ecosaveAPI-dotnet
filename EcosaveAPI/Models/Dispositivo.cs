namespace EcosaveAPI.Models
{
    public class Dispositivo
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdComodo { get; set; }
        public string Nome { get; set; }
        public string Modelo { get; set; }

        // Relacionamentos
        public Usuario Usuario { get; set; }
        public Comodo Comodo { get; set; }
        public ICollection<Consumo> Consumos { get; set; }
    }
}
