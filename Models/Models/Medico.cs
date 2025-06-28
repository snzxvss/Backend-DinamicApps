namespace Models.Model
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public ICollection<Cita> Citas { get; set; }
    }
}
