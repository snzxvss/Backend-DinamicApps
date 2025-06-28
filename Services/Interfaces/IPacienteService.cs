using Models.Response;
using Models.Model;


namespace Services.Interfaces
{
    public interface IPacienteService
    {
        public Task<ResponseModel<Paciente>> AutenticarPacienteAsync(string documento, DateTime fechaNacimiento);
    }
}
