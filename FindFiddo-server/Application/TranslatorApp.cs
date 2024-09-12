using FindFiddo_server.Entities;
using FindFiddo_server.Repository;

namespace FindFiddo_server.Application
{
    public interface ITranslatorApp
    {
        Idioma GetIdiomaDefault();

        List<Idioma> GetAllIdiomas();

        IDictionary<string, Traduccion> GetAllTraducciones(Idioma idioma);

        List<Termino> GetAllTerminos(Idioma idioma = null);

        bool SaveOrUpdateIdioma(Idioma idioma);

        bool SaveOrUpdateTraducciones(List<Traduccion> traducciones, Idioma idioma);
    }
    public class TranslatorApp: ITranslatorApp
    {
        ITranslatorRepository _repo;
        public TranslatorApp(ITranslatorRepository repo) 
        {
            _repo = repo;   
        }

        public List<Idioma> GetAllIdiomas()
        {
            try
            {
                return _repo.GetAllIdiomas();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Termino> GetAllTerminos(Idioma idioma = null)
        {
            try
            {
                return _repo.GetAllTerminos(idioma);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IDictionary<string, Traduccion> GetAllTraducciones(Idioma idioma)
        {
            try
            {
                return _repo.GetAllTraducciones(idioma);
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
                return _repo.GetIdiomaDefault();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public bool SaveOrUpdateIdioma(Idioma idioma)
        {
            try
            {
                return _repo.SaveOrUpdateIdioma(idioma);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SaveOrUpdateTraducciones(List<Traduccion> traducciones, Idioma idioma)
        {
            try
            {
                return _repo.SaveOrUpdateTraducciones(traducciones, idioma);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
