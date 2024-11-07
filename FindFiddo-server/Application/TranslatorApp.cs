using FindFiddo_server.Entities;
using FindFiddo_server.Repository;

namespace FindFiddo_server.Application
{
    public interface ITranslatorApp
    {
        Idioma ObtenerIdiomaDefault();
        List<Idioma> ObtenerIdiomas();
        IDictionary<string, Traduccion> ObtenerTraducciones(Guid idioma);
        List<Traduccion> GetAllTerminos(Idioma idioma = null);
        bool SaveIdioma(Idioma idioma);
        bool SaveTraduccion(List<Traduccion> lista, Idioma idioma);
    }
    public class TranslatorApp: ITranslatorApp
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

        public IDictionary<string, Traduccion> ObtenerTraducciones(Guid idioma)
        {
            try
            {
                IDictionary<string, Traduccion> traducciones = _repo.ObtenerTraducciones(idioma);
                
                return traducciones;
            }
            catch (Exception)
            {

        }

        public bool SaveTraduccion(List<Traduccion> lista, Idioma idioma)
        {
            try
            {
                return _repo.SaveTraduccion(lista, idioma);
            }
            catch (Exception e)
            {
                throw;
            }

        }


        public List<Traduccion> GetAllTerminos(Idioma idioma = null)
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

        public bool SaveOrUpdateTraducciones(List<Traduccion> traducciones, Idioma idioma)
        {
            try
            {
                List<TraduccionDTO> traduccionDTOs = new List<TraduccionDTO>();
                foreach (var item in _repo.GetAllTerminos(idioma))
                {
                    traduccionDTOs.Add(new TraduccionDTO
                    {
                        id = item.termino.id,
                        termino = item.termino.termino,
                        traduccion = item.texto
                    }
                    );
                }
                return traduccionDTOs;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

    }
}
