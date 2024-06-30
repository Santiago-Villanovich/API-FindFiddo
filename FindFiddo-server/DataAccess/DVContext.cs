﻿using System.Data;
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
            string sql = @"
                             USE [master];
        DECLARE @name VARCHAR(MAX) = 'FindFiddo_App'
        DECLARE @SQL NVARCHAR(MAX)
        DECLARE @FileNameOrgBackup NVARCHAR(MAX)
        DECLARE @errorMessage NVARCHAR(4000)

        BEGIN TRY
            -- Encontrar el backup más reciente
            SELECT TOP 1 @FileNameOrgBackup = mf.physical_device_name
            FROM msdb.dbo.backupset bs
            INNER JOIN msdb.dbo.backupmediafamily mf ON bs.media_set_id = mf.media_set_id
            WHERE bs.database_name = @name
              AND mf.physical_device_name LIKE @path + '%'
              AND bs.type = 'D' -- 'D' para full database backup
            ORDER BY bs.backup_start_date DESC

            IF @FileNameOrgBackup IS NULL
            BEGIN
                RAISERROR('No se encontró ningún archivo de backup reciente.', 16, 1)
                RETURN
            END

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
