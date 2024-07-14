using FindFiddo.Entities;
using FindFiddo_server.Entities;
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

        public bool DeleteMirror()
        {
            try
            {
                using SqlCommand cmd = new SqlCommand("mrr_delete", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                _conn.Open();
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { _conn.Close(); }
        }

        public List<MirrorUser> GetIntegrityIssues()
        {
            List<MirrorUser> issues = new List<MirrorUser> ();
            try
            {
                using SqlCommand cmd = new SqlCommand("mrr_GetIntegrityAnomalies", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MirrorUser user = new MirrorUser()
                    {
                        tabla = reader["tabla"].ToString(),
                        usuarioActual = (!reader.IsDBNull(reader.GetOrdinal("usuario_actual")))? reader["usuario_actual"].ToString() : string.Empty,
                        usuarioMrr = (!reader.IsDBNull(reader.GetOrdinal("usuario_mrr"))) ? reader["usuario_mrr"].ToString() : string.Empty,
                        actionType = reader["action_type"].ToString(),
                        camposModificados = (!reader.IsDBNull(reader.GetOrdinal("campo_modificado"))) ? reader["campo_modificado"].ToString() : string.Empty

                    };
                    issues.Add(user);
                }

                return issues;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { _conn.Close(); }
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
            string sql = @"
                             USE [master];
                             DECLARE @name VARCHAR(MAX) = 'FindFiddo_App'
                             DECLARE @SQL NVARCHAR(MAX)
                             DECLARE @FileNameOrgBackup NVARCHAR(MAX)
                             DECLARE @errorMessage NVARCHAR(4000)

                             BEGIN TRY
                                 -- Crear una tabla temporal para almacenar los nombres de archivos desde el sistema de archivos
                                 CREATE TABLE #FileList (
                                     Subdirectory NVARCHAR(255),
                                     Depth INT,
                                     FileFlag INT
                                 );

	                            SET @SQL = 'INSERT INTO #FileList (Subdirectory, Depth, FileFlag) EXEC xp_dirtree ''' + @path + ''', 1, 1;';
	                            EXEC sp_executesql @SQL;

                                 -- Filtrar y ordenar solo los archivos .BAK en orden descendente
                                 SELECT TOP 1 @FileNameOrgBackup = Subdirectory
                                 FROM #FileList
                                 WHERE FileFlag = 1 AND Subdirectory LIKE '%.BAK'
                                 ORDER BY Subdirectory DESC;

                                 -- Limpiar la tabla temporal
                                 DROP TABLE #FileList;

                                 -- Verificar si se encontró un archivo
                                 IF @FileNameOrgBackup IS NULL
                                 BEGIN
                                     RAISERROR('No se encontró ningún archivo de backup reciente.', 16, 1);
                                     RETURN;
                                 END

                                 SET @FileNameOrgBackup = @path + @FileNameOrgBackup;
                                 PRINT 'Archivo de backup seleccionado: ' + @FileNameOrgBackup

                                 -- Verificar el backup
                                 PRINT 'Verificando backup...'
                                 RESTORE VERIFYONLY FROM DISK = @FileNameOrgBackup

                                 -- Restaurar la base de datos
                                 PRINT 'Iniciando restauración...'
                                 SET @SQL = '
                                     ALTER DATABASE [' + @name + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                     RESTORE DATABASE [' + @name + '] FROM DISK = ''' + @FileNameOrgBackup + ''' 
                                     WITH REPLACE, RECOVERY, STATS = 10;
                                     ALTER DATABASE [' + @name + '] SET MULTI_USER;'

                                 EXEC sp_executesql @SQL

                                 PRINT 'Restauración completada correctamente.'
                             END TRY
                             BEGIN CATCH
                                 SET @errorMessage = ERROR_MESSAGE()
                                 RAISERROR(@errorMessage, 16, 1)
                             END CATCH
                        ";
            
            using SqlCommand cmd = new SqlCommand(sql, _conn);
            cmd.CommandType = CommandType.Text;

            SqlParameter p = new SqlParameter("path", SqlDbType.VarChar, -1);
            p.Value = _config["BackupSettings:BackupPath"].Replace("\\", "\\\\");

            cmd.Parameters.Add(p);

            try
            {
                _conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de SQL: {ex.Message}");
                throw ex;
                // Maneja el error apropiadamente
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                throw ex;
                // Maneja el error apropiadamente
            }
            finally { _conn.Close(); }
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
