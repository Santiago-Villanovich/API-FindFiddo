using System.Data;
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

        public bool GenerateBackUp()
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("bak_CreateBackup", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                string path = _config["BackupSettings:BackupPath"];
                cmd.Parameters.AddWithValue("@path", path);

                _conn.Open();
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {

                throw;
            }finally { _conn.Close(); }
        }
        public void RestoreBackUp()
        {
            try
            {
                string sql = @"USE [master]
                               EXEC dbo.bak_RestoreDB @path";
                using SqlCommand cmd = new SqlCommand(sql, _conn);
                cmd.CommandType = System.Data.CommandType.Text;

                string path = _config["BackupSettings:BackupPath"];
                cmd.Parameters.AddWithValue("@path", path);

                _conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool UpdateDVtable(string tName,string tDV)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("dvf_UpdateTableDV", _conn);
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
                using SqlCommand cmd = new SqlCommand("dvf_GetDVbyName", _conn);
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

        public bool UpdateDVuser(Guid idUsuario, string dv)
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("usr_UpdateUserDV", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@dv", dv);
                cmd.Parameters.AddWithValue("@id", idUsuario);

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


    }
}
