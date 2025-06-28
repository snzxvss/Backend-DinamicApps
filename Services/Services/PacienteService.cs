using Models.Model;
using Models.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Services.Interfaces;

namespace Services.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly Models.DbContexts.IpsDbContext _context;
        public PacienteService(Models.DbContexts.IpsDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<Paciente>> AutenticarPacienteAsync(string documento, DateTime fechaNacimiento)
        {
            var pacientes = new List<Paciente>();
            try
            {
                var connection = (SqlConnection)_context.Database.GetDbConnection();
                using (connection)
                {
                    using (var command = new SqlCommand("AutenticarPaciente", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@documento", documento);
                        command.Parameters.AddWithValue("@fechanacimiento", fechaNacimiento);
                        if (connection.State != ConnectionState.Open)
                            await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                pacientes.Add(new Paciente
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Nombres = reader.GetString(reader.GetOrdinal("nombres")),
                                    Apellidos = reader.GetString(reader.GetOrdinal("apellidos")),
                                    Documento = reader.GetString(reader.GetOrdinal("documento")),
                                    FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fechanacimiento")),
                                    Telefono = reader.GetString(reader.GetOrdinal("telefono"))
                                });
                            }
                        }
                    }
                }
                return new ResponseModel<Paciente>
                {
                    Message = pacientes.Any() ? "Paciente autenticado" : "No existe paciente",
                    Content = pacientes,
                    StatusCode = pacientes.Any() ? 200 : 404,
                    Error = null
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<Paciente>
                {
                    Message = "Error al autenticar paciente",
                    Content = new List<Paciente>(),
                    StatusCode = 500,
                    Error = ex.Message
                };
            }
        }
    }
}
