namespace Models.Model
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public ICollection<Cita> Citas { get; set; }
    }
}
