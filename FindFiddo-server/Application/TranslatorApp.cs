using FindFiddo_server.Entities;
using FindFiddo_server.Repository;

namespace FindFiddo_server.Application
{
    public interface ITranslatorApp
    {
        Idioma ObtenerIdiomaDefault();
        List<Idioma> ObtenerIdiomas();
        IDictionary<string, Traduccion> ObtenerTraducciones(Idioma idioma);
        List<Traduccion> GetAllTerminos(Idioma idioma = null);
        bool InsertIdioma(Idioma idioma);
        bool SaveTraduccion(List<Traduccion> traduc, Idioma idioma);
    }
    public class TranslatorApp: ITranslatorApp
    {
        ITranslatorRepository _repo;
        public TranslatorApp(ITranslatorRepository repo) 
        {
            _repo = repo;   
        }

        public bool InsertIdioma(Idioma idioma)
        {
            try
            {
                return _repo.InsertIdioma(idioma);
            }
            catch (Exception e)
            {
                throw;
            }

        }
        public Idioma ObtenerIdiomaDefault()
        {
            try
            {
                return _repo.ObtenerIdiomaDefault();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<Idioma> ObtenerIdiomas()
        {
            try
            {
                return _repo.ObtenerIdiomas();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IDictionary<string, Traduccion> ObtenerTraducciones(Idioma idioma)
        {
            try
            {
                IDictionary<string, ITraduccion> traducciones = new DAL_Traductor().ObtenerTraducciones(idioma);
                IDictionary<string, ITraduccion> traDefault = new DAL_Traductor().ObtenerTraducciones(ObtenerIdiomaDefault());
                foreach (var item in traducciones)
                {
                    if (item.Value.texto == "" || item.Value.texto == null)
                    {
                        item.Value.texto = traDefault[item.Key].texto;
                    }
                }
                return traducciones;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public bool InsertTraducciones(List<Traduccion> lista, Idioma idioma)
        {
            try
            {
                return new DAL_Traductor().InsertTraduccion(lista, idioma);
            }
            catch (Exception e)
            {
                var bitacora = new Bitacora();
                bitacora.Detalle = e.Message;
                bitacora.Responsable = Session.GetInstance.Usuario;
                bitacora.Tipo = Convert.ToInt32(BitacoraTipoEnum.Error);
                new BLL_Bitacora().Insert(bitacora);
                throw;
            }

        }

        public bool UpdateTraducciones(List<Traduccion> lista, Idioma idioma)
        {
            try
            {
                return new DAL_Traductor().UpdateTraduccion(lista, idioma);
            }
            catch (Exception e)
            {
                var bitacora = new Bitacora();
                bitacora.Detalle = e.Message;
                bitacora.Responsable = Session.GetInstance.Usuario;
                bitacora.Tipo = Convert.ToInt32(BitacoraTipoEnum.Error);
                new BLL_Bitacora().Insert(bitacora);
                throw;
            }

        }

        public List<Traduccion> GetAllTerminos()
        {
            try
            {
                return new DAL_Traductor().GetAllTerminos();
            }
            catch (Exception e)
            {
                var bitacora = new Bitacora();
                bitacora.Detalle = e.Message;
                bitacora.Responsable = Session.GetInstance.Usuario;
                bitacora.Tipo = Convert.ToInt32(BitacoraTipoEnum.Error);
                new BLL_Bitacora().Insert(bitacora);
                throw;
            }

        }

        public List<TraduccionDTO> GetAllTerminosDTO(Idioma idioma = null)
        {
            try
            {
                List<TraduccionDTO> traduccionDTOs = new List<TraduccionDTO>();
                foreach (var item in new DAL_Traductor().GetAllTerminos(idioma))
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
            catch (Exception e)
            {
                var bitacora = new Bitacora();
                bitacora.Detalle = e.Message;
                bitacora.Responsable = Session.GetInstance.Usuario;
                bitacora.Tipo = Convert.ToInt32(BitacoraTipoEnum.Error);
                new BLL_Bitacora().Insert(bitacora);
                throw;
            }

        }
    }
}
