using Models.Model;
using Models.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Services.Interfaces;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace Services.Services
{
    public class CitaService : ICitaService
    {
        private readonly Models.DbContexts.IpsDbContext _context;
        private readonly IConfiguration _configuration;
        public CitaService(Models.DbContexts.IpsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseModel<Cita>> ObtenerCitasDisponiblesAsync(string especialidad)
        {
            var citas = new List<Cita>();
            try
            {
                var connection = (SqlConnection)_context.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();
                using (var command = new SqlCommand("ObtenerCitasDisponibles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@especialidad", especialidad);
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
            try
            {
                var connection = (SqlConnection)_context.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                string paciente = "", fecha = "", hora = "", especialidad = "", profesional = "", numero = "";
                int exito = 0;

                using (var command = new SqlCommand("ReservarCita", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@citaId", citaId);
                    command.Parameters.AddWithValue("@pacienteId", pacienteId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            exito = reader.GetInt32(reader.GetOrdinal("Exito"));
                            if (exito == 1)
                            {
                                paciente = reader["Paciente"]?.ToString() ?? "";
                                fecha = reader["Fecha"]?.ToString() ?? "";
                                hora = reader["Hora"]?.ToString() ?? "";
                                especialidad = reader["Especialidad"]?.ToString() ?? "";
                                profesional = reader["Profesional"]?.ToString() ?? "";
                                numero = reader["Numero"]?.ToString() ?? "";
                            }
                        }
                    }
                }

                if (exito == 1)
                {
                    var reminderObj = new
                    {
                        paciente,
                        fecha,
                        hora,
                        especialidad,
                        profesional,
                        numero
                    };

                    var reminderUrl = _configuration["ExternalApis:ReminderUrl"];
                    try
                    {
                        using (var httpClient = new HttpClient())
                        {
                            await httpClient.PostAsJsonAsync(reminderUrl, reminderObj);
                        }
                    }
                    catch { /* Continue... */ }

                    return new ResponseModel<Cita>
                    {
                        Message = "Cita reservada correctamente",
                        Content = new List<Cita>(),
                        StatusCode = 200,
                        Error = null
                    };
                }

                return new ResponseModel<Cita>
                {
                    Message = "No se pudo reservar la cita",
                    Content = new List<Cita>(),
                    StatusCode = 400,
                    Error = "No se pudo reservar la cita"
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
