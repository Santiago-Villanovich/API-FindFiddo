using FindFiddo.DataAccess;
using FindFiddo_server.DataAccess;
using FindFiddo_server.Entities;
using Microsoft.Extensions.Configuration;

namespace FindFiddo_server.Repository
{
    public interface ITranslatorRepository
    {
        Idioma ObtenerIdiomaDefault();
        List<Idioma> ObtenerIdiomas();
        IDictionary<string, Traduccion> ObtenerTraducciones(Idioma idioma);
        List<Traduccion> GetAllTerminos(Idioma idioma = null);
        bool InsertIdioma(Idioma idioma);
        bool SaveTraduccion(List<Traduccion> traduc, Idioma idioma);
    }
    public class TranslatorRepository : ITranslatorRepository
    {
        ITranslatorContext _ctx;
        public TranslatorRepository(ITranslatorContext ctx) 
        {
            _ctx = ctx;
        }
        public List<Traduccion> GetAllTerminos(Idioma idioma = null)
        {
            try
            {
                return _ctx.GetAllTerminos(idioma);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool InsertIdioma(Idioma idioma)
        {
            try
            {
                return _ctx.InsertIdioma(idioma);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SaveTraduccion(List<Traduccion> traduc, Idioma idioma)
        {
            try
            {
                return _ctx.SaveTraduccion(traduc, idioma);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Idioma ObtenerIdiomaDefault()
        {
            return _ctx.ObtenerIdiomaDefault();
        }

        public List<Idioma> ObtenerIdiomas()
        {
            try
            {
                return _ctx.ObtenerIdiomas();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IDictionary<string, Traduccion> ObtenerTraducciones(Idioma idioma)
        {
            try
            {
                return _ctx.ObtenerTraducciones(idioma);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
