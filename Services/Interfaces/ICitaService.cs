using Models.Response;
using Models.Model;

namespace Services.Interfaces
{
    public interface ICitaService
    {
        public Task<ResponseModel<Cita>> ObtenerCitasDisponiblesAsync(string especialidad);
        public Task<ResponseModel<Cita>> ReservarCitaAsync(int citaId, int pacienteId);
    }
}
