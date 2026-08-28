using Microsoft.Data.SqlClient;
using WebApiTalleresBackend.Models;
using WebApiTalleresBackend.Services.Interfaces;

namespace WebApiTalleresBackend.Services.Implementations
{
    public class ActividadService : IActividadService
    {
        private readonly string _connectionString;

        public ActividadService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("No se encontró la cadena de conexión.");
        }

        public List<Actividad> ObtenerTodas()
        {
            var actividades = new List<Actividad>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"
                SELECT A.IdActividad, A.Titulo, A.Descripcion, A.Fecha, A.Hora,
                       A.Lugar, A.Responsable, A.Estado, A.IdTipoActividad,
                       T.Nombre AS TipoActividad
                FROM Actividades A
                INNER JOIN TipoActividad T
                    ON A.IdTipoActividad = T.IdTipoActividad
                ORDER BY A.Fecha DESC";

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                actividades.Add(MapearActividad(reader));
            }

            return actividades;
        }

        public Actividad? ObtenerPorId(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"
                SELECT A.IdActividad, A.Titulo, A.Descripcion, A.Fecha, A.Hora,
                       A.Lugar, A.Responsable, A.Estado, A.IdTipoActividad,
                       T.Nombre AS TipoActividad
                FROM Actividades A
                INNER JOIN TipoActividad T
                    ON A.IdTipoActividad = T.IdTipoActividad
                WHERE A.IdActividad = @IdActividad";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdActividad", id);

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapearActividad(reader);
            }

            return null;
        }

        public void Crear(Actividad actividad)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"
                INSERT INTO Actividades
                    (Titulo, Descripcion, Fecha, Hora, Lugar, Responsable,
                     Estado, IdTipoActividad)
                VALUES
                    (@Titulo, @Descripcion, @Fecha, @Hora, @Lugar,
                     @Responsable, @Estado, @IdTipoActividad)";

            using SqlCommand command = new SqlCommand(query, connection);

            AgregarParametros(command, actividad);
            command.ExecuteNonQuery();
        }

        public bool Modificar(Actividad actividad)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"
                UPDATE Actividades
                SET Titulo = @Titulo,
                    Descripcion = @Descripcion,
                    Fecha = @Fecha,
                    Hora = @Hora,
                    Lugar = @Lugar,
                    Responsable = @Responsable,
                    Estado = @Estado,
                    IdTipoActividad = @IdTipoActividad
                WHERE IdActividad = @IdActividad";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@IdActividad",
                actividad.IdActividad
            );

            AgregarParametros(command, actividad);

            int filasModificadas = command.ExecuteNonQuery();

            return filasModificadas > 0;
        }

        public bool Eliminar(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = @"
                DELETE FROM Actividades
                WHERE IdActividad = @IdActividad";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@IdActividad", id);

            int filasEliminadas = command.ExecuteNonQuery();

            return filasEliminadas > 0;
        }

        private Actividad MapearActividad(SqlDataReader reader)
        {
            return new Actividad
            {
                IdActividad = Convert.ToInt32(reader["IdActividad"]),
                Titulo = reader["Titulo"].ToString() ?? "",
                Descripcion = reader["Descripcion"].ToString(),
                Fecha = Convert.ToDateTime(reader["Fecha"]),

                Hora = reader["Hora"] == DBNull.Value
                    ? null
                    : (TimeSpan)reader["Hora"],

                Lugar = reader["Lugar"].ToString(),
                Responsable = reader["Responsable"].ToString(),
                Estado = reader["Estado"].ToString() ?? "",

                IdTipoActividad =
                    Convert.ToInt32(reader["IdTipoActividad"]),

                TipoActividad = reader["TipoActividad"].ToString()
            };
        }

        private void AgregarParametros(
            SqlCommand command,
            Actividad actividad
        )
        {
            command.Parameters.AddWithValue(
                "@Titulo",
                actividad.Titulo
            );

            command.Parameters.AddWithValue(
                "@Descripcion",
                actividad.Descripcion ?? ""
            );

            command.Parameters.AddWithValue(
                "@Fecha",
                actividad.Fecha
            );

            command.Parameters.AddWithValue(
                "@Hora",
                actividad.Hora.HasValue
                    ? actividad.Hora.Value
                    : DBNull.Value
            );

            command.Parameters.AddWithValue(
                "@Lugar",
                actividad.Lugar ?? ""
            );

            command.Parameters.AddWithValue(
                "@Responsable",
                actividad.Responsable ?? ""
            );

            command.Parameters.AddWithValue(
                "@Estado",
                actividad.Estado
            );

            command.Parameters.AddWithValue(
                "@IdTipoActividad",
                actividad.IdTipoActividad
            );
        }
    }
}