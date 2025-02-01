namespace api_zombies.Models
{
    public class Ronda
    {
        public int Id { get; set; }
        public int UsuariId { get; set; }
        public string NomUsuari { get; set; } = string.Empty;
        public float Segons { get; set; }
        public string Creat { get; set; } = string.Empty;
        public string Modificat { get; set; } = string.Empty;
    }
}