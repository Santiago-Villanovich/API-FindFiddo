using System.Data.SqlClient;

namespace FindFiddo_server.DataAccess
{
    public class DVContext
    {
        private SqlConnection _conn;
        private readonly IConfiguration _config;
        public DVContext(IConfiguration configuration) 
        { 
            _config = configuration;
            _conn = new SqlConnection(_config.GetConnectionString("default"));
        }

        public bool UpdateDVtable(string tName,string tDV)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("UpdateTableDV", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@tNombre", tName);
                cmd.Parameters.AddWithValue("@DV",tDV);

                _conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }
        }

        public string GetDVbyName(string tName)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("GetDVbyName", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@tNombre", tName);

                _conn.Open();
                var dv = cmd.ExecuteScalar().ToString();

                if (!string.IsNullOrEmpty(dv))
                {
                    return dv.ToString();
                }
                else
                {
                    throw new Exception("Nombre de la tabla incorrecto");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conn.Close();
            }
        }
    }
}
