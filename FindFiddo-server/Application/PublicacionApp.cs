using FindFiddo_server.Entities;
using FindFiddo_server.Repository;
using System.Reflection.Metadata.Ecma335;

namespace FindFiddo_server.Application
{
    public interface IPublicacionApp
    {
        void SavePublicacion(Publicacion publicacion);
        IList<TipoPublicacion> GetAllTipos();
        IList<Publicacion> GetPublicacionesByUser(Guid idUser);
        IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag);

        void DeleteCategoria(Guid idCategoria);
        Categoria SaveCategoria(Categoria categoria);
        IList<Categoria> GetAllcategorias(string nombre);

        void Dislike_publicacion(Guid id_user,Guid id_publicacion);
        void like_publicacion(Guid id_user, Guid id_publicacion);
        IList<Publicacion> get_matchs(Guid id_user);
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

        public void DeleteCategoria(Guid idCategoria)
        {
            try
            {
                _repo.DeleteCategoria(idCategoria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Categoria SaveCategoria(Categoria categoria)
        {
            try
            {
                return _repo.SaveCategoria(categoria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<Categoria> GetAllcategorias(string nombre)
        {
            return _repo.GetAllcategorias(nombre);
        }

        public void Dislike_publicacion(Guid id_user, Guid id_publicacion)
        {
            _repo.Dislike_publicacion(id_user, id_publicacion);
        }

        public void like_publicacion(Guid id_user, Guid id_publicacion)
        {
            _repo.like_publicacion(id_user,id_publicacion);
        }

        public IList<Publicacion> get_matchs(Guid id_user)
        {
            return _repo.get_matchs(id_user);
        }

        public IList<TipoPublicacion> GetAllTipos()
        {
            return _repo.GetAllTipos();
        }
    }
}
