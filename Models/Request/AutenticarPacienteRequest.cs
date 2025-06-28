namespace Models.Request
{
    public class AutenticarPacienteRequest
    {
        public string Documento { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
