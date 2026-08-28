using WebApiTalleresBackend.Models;

namespace WebApiTalleresBackend.Services.Interfaces
{
    public interface IActividadService
    {
        List<Actividad> ObtenerTodas();
        Actividad? ObtenerPorId(int id);
        void Crear(Actividad actividad);
        bool Modificar(Actividad actividad);
        bool Eliminar(int id);
    }
}