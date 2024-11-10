using FindFiddo_server.DataAccess;
using FindFiddo_server.Entities;

namespace FindFiddo_server.Repository
{
    public interface IPublicacionRepository
    {
        void SavePublicacion(Publicacion publicacion);
        IList<TipoPublicacion> GetAllTipos();
        IList<Publicacion> GetPublicacionesByUser(Guid idUser);
        IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag);

        void DeleteCategoria(Guid idCategoria);
        Categoria SaveCategoria(Categoria categoria);
        IList<Categoria> GetAllcategorias(string nombre);
    }
    public class PublicacionRepository:IPublicacionRepository
    {
        IPublicacionContext _ctx;
        public PublicacionRepository( IPublicacionContext ctx)
        { 
            _ctx = ctx;
        }
        public void SavePublicacion(Publicacion publicacion)
        {
            _ctx.SavePublicacion(publicacion);
        }
        
        public IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag)
        {
            return _ctx.GetPublicaciones(from, to, tipo, pag);
        }

        public IList<Publicacion> GetPublicacionesByUser(Guid idUser)
        {
            return _ctx.GetPublicacionesByUser(idUser);
        }

        public void DeleteCategoria(Guid idCategoria)
        {
            try
            {
                _ctx.DeleteCategoria(idCategoria);
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
                return _ctx.SaveCategoria(categoria);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<Categoria> GetAllcategorias(string nombre)
        {
            return _ctx.GetAllcategorias(nombre);
        }

        public IList<TipoPublicacion> GetAllTipos()
        {
            return _ctx.GetAllTipos();
        }
    }
}
