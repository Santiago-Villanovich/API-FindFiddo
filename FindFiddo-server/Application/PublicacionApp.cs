using FindFiddo_server.Entities;
using FindFiddo_server.Repository;

namespace FindFiddo_server.Application
{
    public interface IPublicacionApp
    {
        void SavePublicacion(Publicacion publicacion);
        IList<Publicacion> GetPublicacionesByUser(Guid idUser);
        IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag);
    }
    public class PublicacionApp:IPublicacionApp
    {
        IPublicacionRepository _repo;
        public PublicacionApp(IPublicacionRepository repo) 
        {
            _repo = repo;
        }

        public void SavePublicacion(Publicacion publicacion)
        {
            _repo.SavePublicacion(publicacion);
        }

        public IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag)
        {
            return _repo.GetPublicaciones(from, to, tipo, pag);
        }

        public IList<Publicacion> GetPublicacionesByUser(Guid idUser)
        {
            return _repo.GetPublicacionesByUser(idUser);
        }
    }
}
