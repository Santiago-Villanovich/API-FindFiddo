using FindFiddo_server.DataAccess;
using FindFiddo_server.Entities;

namespace FindFiddo_server.Repository
{
    public interface IPublicacionRepository
    {
        void SavePublicacion(Publicacion publicacion);
        IList<Publicacion> GetPublicacionesByUser(Guid idUser);
        IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag);


        void SaveOpcion(Opcion opcion);

        void SaveCategory(Categoria categoria);
        IList<Categoria> GetCategories(Guid catagory);
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

        public void SaveOpcion(Opcion opcion)
        {
            _ctx.SaveOpcion(opcion);
        }

        public void SaveCategory(Categoria categoria)
        {
            _ctx.SaveCategory(categoria);
        }

        public IList<Categoria> GetCategories(Guid catagory)
        {
            return _ctx.GetCategories(catagory);  
        }
    }
}
