using FindFiddo_server.Entities;
using FindFiddo_server.Repository;
using System.Reflection.Metadata.Ecma335;

namespace FindFiddo_server.Application
{
    public interface IPublicacionApp
    {
        void SavePublicacion(Publicacion publicacion);
        IList<Publicacion> GetPublicacionesByUser(Guid idUser);
        IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag);

        void SaveOpcion(Opcion opcion);

        void SaveCategory(Categoria categoria);
        IList<Categoria> GetCategories(Guid catagory);
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

        public void SaveOpcion(Opcion opcion)
        {
            _repo.SaveOpcion(opcion);
        }

        public void SaveCategory(Categoria categoria)
        {
            _repo.SaveCategory(categoria);
        }

        public IList<Categoria> GetCategories(Guid catagory)
        {
            return _repo.GetCategories(catagory);
        }
    }
}
