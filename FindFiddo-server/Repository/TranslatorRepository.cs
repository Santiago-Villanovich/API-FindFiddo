using FindFiddo.DataAccess;
using FindFiddo_server.DataAccess;
using FindFiddo_server.Entities;
using Microsoft.Extensions.Configuration;

namespace FindFiddo_server.Repository
{
    public interface ITranslatorRepository
    {
        Idioma GetIdiomaDefault();
        List<Idioma> GetAllIdiomas();
        IDictionary<string, Traduccion> GetAllTraducciones(Guid idIdioma);
        List<Termino> GetAllTerminos();
        bool SaveIdioma(Idioma idioma);
        bool SaveTraducciones(List<Traduccion> traducciones, Idioma idioma);
    }
    public class TranslatorRepository : ITranslatorRepository
    {
        ITranslatorContext _ctx;
        public TranslatorRepository(ITranslatorContext ctx) 
        {
            _ctx = ctx;
        }

        public List<Idioma> GetAllIdiomas()
        {
            try
            {
                return _ctx.GetAllIdiomas();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Termino> GetAllTerminos()
        {
            try
            {
                return _ctx.GetAllTerminos();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IDictionary<string, Traduccion> GetAllTraducciones(Guid idioma)
        {
            try
            {
                return _ctx.GetAllTraducciones(idioma);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Idioma GetIdiomaDefault()
        {
            try
            {
                return _ctx.GetIdiomaDefault();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public bool SaveIdioma(Idioma idioma)
        {
            try
            {
                return _ctx.SaveIdioma(idioma);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SaveTraducciones(List<Traduccion> traducciones, Idioma idioma)
        {
            try
            {
                foreach (Traduccion t in traducciones)
                {
                    _ctx.SaveTraduccion(t, idioma);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
