using FindFiddo_server.Entities;
using System.Data.SqlClient;
using System.Data;

namespace FindFiddo_server.DataAccess
{
    public interface ITranslatorContext
    {
        Idioma ObtenerIdiomaDefault();
        List<Idioma> ObtenerIdiomas();
        IDictionary<string, Traduccion> ObtenerTraducciones(Idioma idioma);
        List<Traduccion> GetAllTerminos(Idioma idioma = null);
        bool InsertIdioma(Idioma idioma);
        bool SaveTraduccion(List<Traduccion> traduc, Idioma idioma);
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

        public Idioma ObtenerIdiomaDefault()
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
                             Id = Convert.ToInt32(reader["ID"].ToString()),
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

        public List<Idioma> ObtenerIdiomas()
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
                             Id = Convert.ToInt32(reader["ID"].ToString()),
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

        public IDictionary<string, Traduccion> ObtenerTraducciones(Idioma idioma)
        {

            if (idioma == null)
            {
                idioma = ObtenerIdiomaDefault();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
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
                                 id = Convert.ToInt32(etiqueta),
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

        public List<Traduccion> GetAllTerminos(Idioma idioma = null)
        {
            using (SqlConnection conn = _conn)
            {
                IDataReader reader = null;
                List<Traduccion> _lista = new List<Traduccion>();
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
                        if (idioma == null)
                        {
                            _lista.Add(
                            new Traduccion()
                            {
                                texto = "",
                                termino = new Termino()
                                {
                                    id = Convert.ToInt32(reader["ID"].ToString()),
                                    termino = reader["Termino"].ToString()
                                }
                            });
                        }
                        else
                        {
                            _lista.Add(
                            new Traduccion()
                            {
                                texto = reader["Traduccion"].ToString(),
                                termino = new Termino()
                                {
                                    id = Convert.ToInt32(reader["ID"].ToString()),
                                    termino = reader["Termino"].ToString()
                                }
                            });
                        }

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

        public bool InsertIdioma(Idioma idioma)
        {
            using (SqlConnection conn = _conn)
            {

                SqlCommand cmd = new SqlCommand("sp_InsertIdioma", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nom", idioma.nombre);
                cmd.Connection = conn;
                conn.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
                catch (Exception ex2)
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        public bool SaveTraduccion(List<Traduccion> traduc, Idioma idioma)
        {
            using (SqlConnection conn = _conn)
            {
                conn.Open();

                try
                {
                    foreach (Traduccion item in traduc)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.CommandText = "sp_InsertTraduccion";

                        cmd.Parameters.AddWithValue("@IdIdioma", idioma.Id);
                        cmd.Parameters.AddWithValue("@IdTermino", item.termino.id);
                        cmd.Parameters.AddWithValue("@Traduccion", item.texto);
                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
                catch (Exception ex2)
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

    }
}

