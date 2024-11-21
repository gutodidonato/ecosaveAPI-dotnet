namespace EcosaveAPI.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string CEP { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }

        // Relacionamento com Usuario
        public Usuario Usuario { get; set; }
    }
}
