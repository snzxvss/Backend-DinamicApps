namespace Models.Model
{
    public class Cita
    {
        public int Id { get; set; }
        public string Especialidad { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }
        public int IdMedico { get; set; }
        public Medico Medico { get; set; }
        public int? IdPaciente { get; set; }
        public Paciente Paciente { get; set; }
    }
}
