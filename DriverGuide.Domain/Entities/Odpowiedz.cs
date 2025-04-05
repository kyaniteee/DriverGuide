namespace DriverGuide.Domain.Entities
{
    public class Odpowiedz
    {
        public string? Id { get; set; }
        public string? TestId { get; set; }
        public string? PytanieId { get; set; }
        public string? Tekst { get; set; }
        public bool Poprawna { get; set; }
    }
}