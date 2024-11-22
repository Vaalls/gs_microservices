namespace GlobalS_2.Models
{
    public class Consumo
    {
        public int Id { get; set; } // ID do consumo
        public string? Data { get; set; } // Data do consumo
        public double Valor { get; set; } // Valor do consumo (em kWh)
    }
}
