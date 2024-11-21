namespace EcosaveAPI.Models
{
    public class Comodo
    {
        public int Id { get; set; }
        public string Nome { get; set; }


        // Relacionamento com Dispositivos
        public ICollection<Dispositivo> Dispositivos { get; set; }
    }
}
