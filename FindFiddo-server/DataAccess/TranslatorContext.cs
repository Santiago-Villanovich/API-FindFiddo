using FindFiddo_server.Entities;
using System.Data.SqlClient;
using System.Data;

namespace FindFiddo_server.DataAccess
{
    public interface ITranslatorContext
    {
        bool SaveIdioma(Idioma idioma);
        List<Idioma> GetAllIdiomas();
        List<Termino> GetAllTerminos();
        bool SaveTraduccion(Traduccion traduc, Idioma idioma);
        IDictionary<string, TraduccionDTO> GetAllTraducciones(Guid idioma);

        Idioma GetIdiomaDefault();  
 
    }
    public class TranslatorContext: ITranslatorContext
    {
        
        private SqlConnection _conn;
        private readonly IConfiguration _config;
        public TranslatorContext(IConfiguration configuration)
        {
            _config = configuration;
            _conn = new SqlConnection(_config.GetConnectionString("default"));
        }

        public Idioma GetIdiomaDefault()
        {
            using (SqlConnection conn = _conn)
            {
                IDataReader reader = null;
                Idioma _idioma = null;
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_GetIdiomaDefault", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        _idioma =
                         new Idioma()
                         {
                             Id = Guid.Parse(reader["ID"].ToString()),
                             descripcion = reader["NombreIdioma"].ToString(),
                             isDefault = Convert.ToBoolean(reader["defaultIdioma"])

                         };
                    }
                    return _idioma;
                }
                catch (Exception e)
                {

                    throw e;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    conn.Close();
                }
            }
        }

        public List<Idioma> GetAllIdiomas()
        {
            using (SqlConnection conn = _conn)
            {
                IDataReader reader = null;
                List<Idioma> _idiomas = new List<Idioma>();
                try
                {
                    SqlCommand cmd = new SqlCommand("lng_GetAllIdiomas", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        _idiomas.Add(
                            new Idioma(){

                                Id = Guid.Parse(reader["id_idioma"].ToString()),
                                descripcion = reader["descripcion"].ToString()
                            }
                        );
                    }
                    return _idiomas;
                }
                catch (Exception e)
                {

                    throw e;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    conn.Close();
                }
            }
        }

        public IDictionary<string, TraduccionDTO> GetAllTraducciones(Guid idioma)
        {

            using (SqlConnection conn = new SqlConnection(_conn.ConnectionString))
            {
                IDataReader reader = null;
                IDictionary<string, TraduccionDTO> _traducciones = new Dictionary<string, TraduccionDTO>();
                try
                {

                    SqlCommand cmd = new SqlCommand("lng_GetAllTraducciones", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idIdioma", idioma);
                    conn.Open();

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var termino = reader["id_termino"].ToString();

                        _traducciones.Add(termino,
                             new TraduccionDTO(){

                                 id = Guid.Parse(termino),
                                 termino = reader["descripcion"].ToString(),
                                 traduccion = reader.GetString(reader.GetOrdinal("traduccion"))
                             }
                        );
                    }
                    return _traducciones;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    conn.Close();
                }

            }
        }

        public List<Termino> GetAllTerminos()
        {
            using (SqlConnection conn = new SqlConnection(_conn.ConnectionString) )
            {
                IDataReader reader = null;
                List<Termino> _lista = new List<Termino>();

                try
                {
                    SqlCommand cmd = new SqlCommand("lng_GetAllTerminos", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    conn.Open();

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        _lista.Add(new Termino()
                        {
                            Id = Guid.Parse(reader["id_termino"].ToString()),
                            termino = reader["descripcion"].ToString()

                        });
                    }
                    return _lista;
                }
                catch (Exception e)
                {

                    throw e;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    conn.Close();
                }
            }
        }

        public bool SaveIdioma(Idioma idioma)
        {
            using (SqlConnection conn = _conn)
            {

                if (idioma.Id == Guid.Empty)
                {
                    idioma.Id = Guid.NewGuid();
                }

                SqlCommand cmd = new SqlCommand("lng_SaveIdioma", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", idioma.Id);
                cmd.Parameters.AddWithValue("@descripcion", idioma.descripcion);
                cmd.Connection = conn;
                conn.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        public bool SaveTraduccion(Traduccion traduc, Idioma idioma)
        {
            using (SqlConnection conn = _conn)
            {
                conn.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("lng_SaveTraduccion", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdIdioma", idioma.Id);
                    cmd.Parameters.AddWithValue("@IdTermino", traduc.termino.Id);
                    cmd.Parameters.AddWithValue("@Traduccion", traduc.texto);

                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

    }
}

