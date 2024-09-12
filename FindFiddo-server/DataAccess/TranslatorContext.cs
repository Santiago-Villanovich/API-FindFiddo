using FindFiddo_server.Entities;
using System.Data.SqlClient;
using System.Data;

namespace FindFiddo_server.DataAccess
{
    public interface ITranslatorContext
    {
        Idioma GetIdiomaDefault();

        List<Idioma> GetAllIdiomas();

        IDictionary<string, Traduccion> GetAllTraducciones(Idioma idioma);

        List<Termino> GetAllTerminos(Idioma idioma = null);

        bool SaveOrUpdateIdioma(Idioma idioma);

        bool SaveOrUpdateTraduccion(Traduccion traduc, Idioma idioma);

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
                             nombre = reader["NombreIdioma"].ToString(),
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
                    SqlCommand cmd = new SqlCommand("sp_GetAllIdiomas", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        _idiomas.Add(
                         new Idioma()
                         {
                             Id = Guid.Parse(reader["ID"].ToString()),
                             nombre = reader["NombreIdioma"].ToString(),
                             isDefault = Convert.ToBoolean(reader["defaultIdioma"])

                         });
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

        public IDictionary<string, Traduccion> GetAllTraducciones(Idioma idioma)
        {

            if (idioma == null)
            {
                idioma = GetIdiomaDefault();
            }

            using (SqlConnection conn = new SqlConnection(_conn.ConnectionString))
            {
                IDataReader reader = null;
                IDictionary<string, Traduccion> _traducciones = new Dictionary<string, Traduccion>();
                try
                {

                    SqlCommand cmd = new SqlCommand("sp_GetAllTraducciones", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdIdioma", idioma.Id);
                    conn.Open();

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var etiqueta = reader["ID"].ToString();

                        _traducciones.Add(etiqueta,
                         new Traduccion()
                         {

                             texto = reader["traduccion"].ToString(),

                             termino = new Termino()
                             {
                                 Id = Guid.Parse(etiqueta),
                                 termino = reader["termino"].ToString()
                             }

                         });
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

        public List<Termino> GetAllTerminos(Idioma idioma = null)
        {
            using (SqlConnection conn = new SqlConnection(_conn.ConnectionString) )
            {
                IDataReader reader = null;
                List<Termino> _lista = new List<Termino>();

                try
                {
                    SqlCommand cmd = new SqlCommand("sp_GetAllTerminos", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (idioma != null)
                    { cmd.Parameters.AddWithValue("@id_Idioma", idioma.Id); }
                    else
                    { cmd.Parameters.AddWithValue("@id_Idioma", DBNull.Value); }

                    cmd.Connection = conn;
                    conn.Open();

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        _lista.Add(new Termino()
                        {

                            Id = Guid.Parse(reader["ID"].ToString()),
                            termino = reader["Termino"].ToString()

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

        public bool SaveOrUpdateIdioma(Idioma idioma)
        {
            using (SqlConnection conn = _conn)
            {

                SqlCommand cmd = new SqlCommand("sp_InsertIdioma", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id",idioma.Id);
                cmd.Parameters.AddWithValue("@Nom", idioma.nombre);

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

        public bool SaveOrUpdateTraduccion(Traduccion traduc, Idioma idioma)
        {
            using (SqlConnection conn = _conn)
            {
                conn.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertTraduccion", conn);
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

