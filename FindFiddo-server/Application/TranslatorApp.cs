using FindFiddo_server.Entities;
using FindFiddo_server.Repository;

namespace FindFiddo_server.Application
{
    public interface ITranslatorApp
    {
        Idioma ObtenerIdiomaDefault();
        List<Idioma> GetAllIdiomas();
        IDictionary<string, Traduccion> GetAllTraducciones(Guid idioma);
        List<Termino> GetAllTerminos();
        bool SaveIdioma(Idioma idioma);
        bool SaveTraducciones(List<Traduccion> lista, Idioma idioma);
    }
    public class TranslatorApp : ITranslatorApp
    {
        ITranslatorRepository _repo;
        public TranslatorApp(ITranslatorRepository repo)
        {
            _repo = repo;
        }

        public bool SaveIdioma(Idioma idioma)
        {
            try
            {
                return _repo.SaveIdioma(idioma);
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
                return _repo.GetAllTerminos();
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
                return _repo.GetAllTraducciones(idioma);
            }
            catch (Exception)
            {  throw;}

        }

        public bool SaveTraducciones(List<Traduccion> lista, Idioma idioma)
        {
            try
            {
                return _repo.SaveTraducciones(lista, idioma);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Idioma ObtenerIdiomaDefault()
        {
            throw new NotImplementedException();
        }

        public List<Idioma> GetAllIdiomas()
        {
            return _repo.GetAllIdiomas();
        }

    }
}
