namespace DriverGuide.Domain.Entities
{
    public class Test
    {
        public string? Id { get; set; }
        public double Wynik { get; set; }
        public DateTime DataOd { get; set; }
        public DateTime? DataDo { get; set; }
        public Guid UzytkownikId { get; set; }
    }
}