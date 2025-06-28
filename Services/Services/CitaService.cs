using Models.Model;
using Models.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Services.Interfaces;

namespace Services.Services
{
    public class CitaService : ICitaService
    {
        private readonly Models.DbContexts.IpsDbContext _context;
        public CitaService(Models.DbContexts.IpsDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<Cita>> ObtenerCitasDisponiblesAsync(string especialidad)
        {
            var citas = new List<Cita>();
            try
            {
                var connection = (SqlConnection)_context.Database.GetDbConnection();
                using (connection)
                {
                    using (var command = new SqlCommand("ObtenerCitasDisponibles", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@especialidad", especialidad);
                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                citas.Add(new Cita
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Especialidad = reader.GetString(reader.GetOrdinal("especialidad")),
                                    FechaHora = reader.GetDateTime(reader.GetOrdinal("fechahora")),
                                    Estado = reader.GetString(reader.GetOrdinal("estado")),
                                    IdMedico = reader.GetInt32(reader.GetOrdinal("idmedico")),
                                    Medico = new Medico
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("idmedico")),
                                        Nombre = reader.GetString(reader.GetOrdinal("medico_nombre")),
                                        Especialidad = especialidad
                                    }
                                });
                            }
                        }
                    }
                }
                return new ResponseModel<Cita>
                {
                    Message = "Citas consultadas correctamente",
                    Content = citas,
                    StatusCode = 200,
                    Error = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Cita>
                {
                    Message = "Error al obtener citas disponibles",
                    Content = new List<Cita>(),
                    StatusCode = 500,
                    Error = ex.Message
                };
            }
        }

        public async Task<ResponseModel<Cita>> ReservarCitaAsync(int citaId, int pacienteId)
        {
            int exito = 0;
            try
            {
                var connection = (SqlConnection)_context.Database.GetDbConnection();
                using (connection)
                {
                    using (var command = new SqlCommand("ReservarCita", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@citaId", citaId);
                        command.Parameters.AddWithValue("@pacienteId", pacienteId);
                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                                exito = reader.GetInt32(reader.GetOrdinal("Exito"));
                        }
                    }
                }
                return new ResponseModel<Cita>
                {
                    Message = exito == 1 ? "Cita reservada correctamente" : "No se pudo reservar la cita",
                    Content = new List<Cita>(),
                    StatusCode = exito == 1 ? 200 : 400,
                    Error = exito == 1 ? null : "No se pudo reservar la cita"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Cita>
                {
                    Message = "Error al reservar cita",
                    Content = new List<Cita>(),
                    StatusCode = 500,
                    Error = ex.Message
                };
            }
        }
    }
}
