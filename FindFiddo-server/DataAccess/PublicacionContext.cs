using FindFiddo.Entities;
using FindFiddo_server.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FindFiddo_server.DataAccess
{
    public interface IPublicacionContext
    {
        void SavePublicacion(Publicacion publicacion);
        IList<Publicacion> GetPublicacionesByUser(Guid idUser);
        IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag);

       

        void SaveOpcion(Opcion opcion);

        void SaveCategory(Categoria categoria);
        IList<Categoria> GetCategories(Guid catagory);
    }
    public class PublicacionContext:IPublicacionContext
    {
        private SqlConnection _conn;
        private readonly IConfiguration _config;
        public PublicacionContext(IConfiguration configuration)
        {
            _config = configuration;
            _conn = new SqlConnection(_config.GetConnectionString("default"));
        }

        public void SavePublicacion(Publicacion publicacion)
        {
            bool isNew = false;
            try
            {
                using SqlCommand cmd = new SqlCommand("pub_SavePublicacion", _conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if(publicacion.Id == Guid.Empty)
                {
                    publicacion.Id = Guid.NewGuid();
                    isNew = true;
                }
                    
                cmd.Parameters.AddWithValue("@id", publicacion.Id);
                cmd.Parameters.AddWithValue("@idUser", publicacion.usuario.Id);
                cmd.Parameters.AddWithValue("@idTipo", publicacion.tipo.Id);
                cmd.Parameters.AddWithValue("@titulo", publicacion.titulo);
                cmd.Parameters.AddWithValue("@historia", publicacion.historia);
                cmd.Parameters.AddWithValue("@descripcion", publicacion.descripcion);
                cmd.Parameters.AddWithValue("@ubicacion", publicacion.ubicacion);

                _conn.Open();
                cmd.ExecuteNonQuery();

                if (isNew && publicacion.opciones.Count()>0) {
                    cmd.CommandText = "pub_SavePublicacionOpcion";

                    foreach (var op in publicacion.opciones)
                    {
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@idPublicacion", publicacion.Id);
                        cmd.Parameters.AddWithValue("@idOpcion", op.Id);

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public IList<Publicacion> GetPublicaciones(DateTime from, DateTime to, string tipo, int pag)
        {
            throw new NotImplementedException();
        }

        public IList<Publicacion> GetPublicacionesByUser(Guid idUser)
        {
            try
            {
                IList<Publicacion> Publicaciones = new List<Publicacion>();

                using SqlCommand cmd = new SqlCommand("pub_GetAllPublicacionByUser", _conn);

                cmd.Parameters.AddWithValue("@idUser", idUser);

                _conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                
                while (reader.Read())
                {
                    Guid id = reader.GetGuid(reader.GetOrdinal("id_publicacion"));
                    Publicacion publi = Publicaciones.FirstOrDefault(p => p.Id.Equals(id));

                    Opcion op = null;
                    if (!reader.IsDBNull(reader.GetOrdinal("opcion_id")))
                    {
                        op = new Opcion(
                            reader.GetGuid(reader.GetOrdinal("opcion_id")),
                            reader.GetString(reader.GetOrdinal("opcion_nombre"))
                        );

                        publi.opciones.Add(op);
                    }

                    if (publi == null)
                    {
                        publi = new Publicacion();
                        publi.Id = id;

                        publi.tipo = new TipoPublicacion(
                            reader.GetGuid(reader.GetOrdinal("tipo_id")),
                            reader.GetString(reader.GetOrdinal("tipo_descripcion"))
                        );

                        publi.titulo = reader.GetString(reader.GetOrdinal("titulo"));
                        publi.descripcion = (string)reader["descripcion"];
                        publi.historia = (string)reader["historia"];
                        publi.ubicacion = reader.GetString(reader.GetOrdinal("ubicacion"));
                        publi.fechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion"));

                        publi.opciones = new List<Opcion>();
                        if (op != null)
                            publi.opciones.Add(op);

                        Publicaciones.Add(publi);
                    }
                    else
                    {
                        if(op!=null)
                            publi.opciones.Add(op);
                    }
                }

                return Publicaciones;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
            
        }

        public void SaveOpcion(Opcion opcion)
        {
          
            try
            {
                using SqlCommand cmd = new SqlCommand("pub_SaveOpcion", _conn);//Crear Script
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (opcion.Id == Guid.Empty)
                {
                    opcion.Id = Guid.NewGuid();
                }

                cmd.Parameters.AddWithValue("@id", opcion.Id);
                cmd.Parameters.AddWithValue("@nombre", opcion.nombre);
               

                _conn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public void SaveOpcionForCategory(Opcion opcion,Guid category)
        {

            try
            {
                using SqlCommand cmd = new SqlCommand("pub_SaveOpcion", _conn);//Crear Script
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (opcion.Id == Guid.Empty)
                {
                    opcion.Id = Guid.NewGuid();
                }

                cmd.Parameters.AddWithValue("@id", opcion.Id);
                cmd.Parameters.AddWithValue("@nombre", opcion.nombre);
                cmd.Parameters.AddWithValue("@categoria_id", category);


                _conn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }
        public void SaveCategory(Categoria categoria)
        {
            bool isNew = false;
            try
            {
                using SqlCommand cmd = new SqlCommand("pub_SaveCategoria", _conn);//crear Script
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (categoria.Id == Guid.Empty)
                {
                    categoria.Id = Guid.NewGuid();
                    isNew = true;
                }

                cmd.Parameters.AddWithValue("@categoria_id", categoria.Id);
                cmd.Parameters.AddWithValue("@categoria_nombre", categoria.nombre);
                if (categoria.opciones.Count > 0) 
                {
                    foreach (Opcion op in categoria.opciones)
                    {
                        if (op.Id == Guid.Empty)
                        {
                            op.Id = Guid.NewGuid();

                        }
                        SaveOpcionForCategory(op, categoria.Id);
                    }
                }
                
               

                _conn.Open();
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { _conn.Close(); }
        }

        public IList<Categoria> GetCategories(Guid catagory)
        {
            try
            {
                IList<Categoria> categorias = new List<Categoria>();

                using (SqlCommand cmd = new SqlCommand("pub_GetAllCategorias", _conn))
                {
                    _conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Categoria currentCategoria = null;

                        while (reader.Read())
                        {
                            Guid id = reader.GetGuid(reader.GetOrdinal("id_publicacion"));
                            currentCategoria = categorias.FirstOrDefault(c => c.Id.Equals(id));

                            if (currentCategoria == null)
                            {

                                currentCategoria = new Categoria
                                {
                                    Id = id,
                                    nombre = reader.GetString(reader.GetOrdinal("categoria_nombre")),
                                    opciones = new List<Opcion>() 
                                };

                                categorias.Add(currentCategoria); 
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("opcion_id")))
                            {
                                Opcion op = new Opcion
                                (
                                    reader.GetGuid(reader.GetOrdinal("opcion_id")),
                                    reader.GetString(reader.GetOrdinal("opcion_nombre"))
                                );

                                currentCategoria.opciones.Add(op);
                            }
                        }
                    }
                }

                return categorias;
            }
            catch (Exception)
            {
                throw;
            }
            finally { _conn.Close(); }
        }
    }
}
