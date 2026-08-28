namespace WebApiTalleresBackend.Models
{
    public class Actividad
    {
        public int IdActividad { get; set; }
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan? Hora { get; set; }
        public string? Lugar { get; set; }
        public string? Responsable { get; set; }
        public string Estado { get; set; }
        public int IdTipoActividad { get; set; }
        public string? TipoActividad { get; set; }
    }
}