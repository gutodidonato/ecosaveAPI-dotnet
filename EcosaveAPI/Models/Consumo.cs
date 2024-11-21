namespace EcosaveAPI.Models
{
    public class Consumo
    {
        public int Id { get; set; }
        public int IdDispositivo { get; set; }
        public decimal ConsumoKWh { get; set; }
        public decimal Custo { get; set; }

        // Relacionamento com Dispositivo
        public Dispositivo Dispositivo { get; set; }
    }
}
