using FindFiddo.Application;
using FindFiddo.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FindFiddo.Services;
using FindFiddo_server.Services;
using FindFiddo_server.Application;
using FindFiddo_server.Entities;

namespace FindFiddo_Server.Controllers
{
    [Route("api/FindFiddo")]
    [ApiController]
    public class FindFiddoController : ControllerBase
    {
        private readonly ILogger<FindFiddoController> _logger;
        private IDVService _dv;
        private IUsuarioApp _user;
        private ITranslatorApp _translate;
        private IPublicacionApp _publicacion;

        public FindFiddoController
        (
            ILogger<FindFiddoController> logger, 
            IDVService dvService,
            IUsuarioApp usuarioApp,
            ITranslatorApp translate,
            IPublicacionApp publicacion
        )
        {
            _logger = logger;
            _user = usuarioApp;
            _dv = dvService;
            _translate = translate;
            _publicacion = publicacion;
        }

       

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginUser logUser)
        {
            var user = _user.GetUserByEmail(logUser.email);
            if (user != null)
            {
                if (EncryptService.VerifyPassword(logUser.password,user.salt,user.password))
                {
                    LogedUser lUser = new LogedUser(user.Id, user.telefono, user.email, user.nombres, user.apellidos);
                    lUser.rol = _user.GetUserRols(lUser.Id);
                    _user.InsertUserLog(user.Id,"LogIn");
                    SessionManager.Login(user);
                    return Ok(lUser);
                }
                else
                {
                    return BadRequest("Mail o Contraseña incorrecta");
                }
            }
            else
            {
                return BadRequest("Usuario no registrado");
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        public IActionResult SignUp([FromBody] User user)
        {
            try
            {
                if (user.rol == null ) //Si no tiene rol le asigno el usuario default
                {
                    user.rol = new List<Rol>
                    {
                        new Rol() { Id = Guid.Parse("05953C7E-3250-49B5-89C3-3C44C2D46A83"), nombre = "User" }
                    };

                }
                if (user.idioma_preferido == null)
                    user.idioma_preferido = Guid.Parse("0DAC9C9C-B1C3-4FF5-9B85-595427E6210C"); //espaniol default

                LogedUser UsrLoged = _user.SignUP(user);

                if (UsrLoged != null)
                {
                    _dv.UpdateDVTable("usuario",_user.GetAll()); //recalculo el dv de la tabla
                    UsrLoged.rol = _user.GetUserRols(UsrLoged.Id);
                    SessionManager.Login(user);
                    return Ok(UsrLoged);
                }
                else
                {
                    return BadRequest ("Algo ha salido mal");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("logOut")]
        public IActionResult LogOut()
        {
            try
            {
                SessionManager.Logout();
                return Ok("Se realizo el log out con exito");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("session/data")]
        public IActionResult Session_data()
        {
            try
            {
               SessionManager session = SessionManager.GetInstance;
                return Ok(session);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpDelete]
        [Route("user/{id}")]
        public IActionResult DeleteUserById(Guid id)
        {
            try
            {
                _user.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var all = _user.GetAll();
                var users = all.Select(u => new LogedUser(u.Id,u.telefono,u.email,u.nombres,u.apellidos));
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("data/status")]
        public IActionResult validateDataBaseState()
        {
            try
            {
                var users = _user.GetAll();
                if (_dv.VerificarDigitoTable("usuario",users))
                {
                    return Ok("La base de datos se encuentra en correcto estado");
                }
                else
                {
                    return StatusCode(911, new { message = "Se detectaron inconsistencias en la base de datos" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("data/integrity-issues")]
        public IActionResult getAllCorruptedElements()
        {
            try
            {
                var lista = _dv.GetIntegrityIssues();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

        [AllowAnonymous]
        [HttpPost]
        [Route("data/dv-restore")]
        public IActionResult restoreAllTablesDV()
        {
            try
            {
                _dv.RecalcularUserDVTable("usuario", _user.GetAll() );
                _dv.DeleteMirror();
                return Ok("Restablecimiento de Digito Verificador exitoso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("data/backup")]
        public IActionResult GenerateBackUp()
        {
            try
            {
                bool state = _dv.GenerateBackUp();
                if (state) {
                    return Ok("Se ha generado el backup exitosamente");
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("data/restore")]
        public IActionResult RestoreBackUp()
        {
            try
            {
                _dv.RestoreBackUp();
                _dv.DeleteMirror();
                return Ok("La base de datos se ha restaurado correctamente");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/{id}")]
        public IActionResult GetUserById(Guid id)
        {
            try
            {
                var user = _user.GetById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/log")]
        public IActionResult GetUserLog(string? from, string? to, string? action, int pag)
        {
            try
            {
                DateTime desde, hasta;

                if (DateTime.TryParse(from, out desde) && DateTime.TryParse(to, out hasta))
                {
                    var all = _user.GetLog(desde, hasta,action,pag);
                    return Ok(all); 
                }
                else
                {
                    return BadRequest("Formato de las fechas incorrecto");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("organizaciones")]
        public IActionResult SaveOrganizaciones(Organizacion org)
        {
            try
            {
                if (org == null)
                    return BadRequest("El objeto no puede ser nulo");

                var o = _user.SaveOrganizacion(org);
                return Ok(o);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("organizaciones")]
        public IActionResult GetAllOrganizaciones(string? idUser,string? idOrg)
        {
            try
            {
                Guid user = Guid.Empty;
                Guid org = Guid.Empty;

                if (!String.IsNullOrEmpty(idUser))
                    user = Guid.Parse(idUser);
                if (!String.IsNullOrEmpty(idOrg))
                    org = Guid.Parse(idOrg);

                var all = _user.GetAllOrganizaciones(user,org);
                return Ok(all);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[AllowAnonymous]
        [HttpGet]
        [Route("organizaciones/{id}")]
        public IActionResult GetOrganizacionById(Guid id)
        {
            try
            {
                var org = _user.getOrganizacionByID(id);
                return Ok(org);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        [AllowAnonymous]
        [HttpPost]
        [Route("organizaciones/{id}/asignar/{user}")]
        public IActionResult Asignar_user_organizacion(Guid user,Guid id)
        {
            try
            {
                _user.Asignar_Usuario_Organizacion(user, id);
                return Ok("Se asigno el ususario a la organizacion :"+id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("organizaciones/{id}/desasignar/{user}")]
        public IActionResult Desasignar_user_organizacion(Guid id,Guid user)
        {
            try
            {
                _user.Desasignar_Usuario_Organizacion(user, id);
                return Ok("Se desasigno el ususario" + user+" a la organizacion :" + id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("idiomas/{idioma}/traducciones")]
        public IActionResult GetIdiomaTraducciones(string idioma, string? pag)
        {
            try
            {
                var all = _translate.GetAllTraducciones(idioma,pag);
                return Ok(all);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("idiomas/{idioma}/traducciones")]
        public IActionResult SaveIdiomaTraducciones(Guid idioma, [FromBody] List<Traduccion> traducciones)
        {
            try
            {
                if (!(traducciones.Count() > 0))
                    return BadRequest("La lista no puede estar vacia");
                
                var all = _translate.SaveTraducciones(traducciones,new Idioma() { Id = idioma});
                return Ok(all);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("idiomas/{idioma}/traducciones/import")]
        public async Task<IActionResult> SaveIdiomaTraduccionesXML(Guid idioma, IFormFile xmlFile)
        {
            if (xmlFile == null || xmlFile.Length == 0)
            {
                return BadRequest("Archivo XML no proporcionado o vacío.");
            }

            try
            {
                // Leer el contenido del archivo XML y convertirlo a una cadena
                string xmlContent;
                using (var stream = new StreamReader(xmlFile.OpenReadStream()))
                {
                    xmlContent = await stream.ReadToEndAsync();
                }

                // Deserializar el XML en objetos de traducción
                var traducciones = XMLservice.DeserializarTraduccionXML(xmlContent);
                var all = _translate.SaveTraducciones(traducciones, new Idioma() { Id = idioma });

                return Ok(all);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("idiomas/terminos")]
        public IActionResult GetAllTerminos()
        {
            try
            {
                var all = _translate.GetAllTerminos();
                return Ok(all);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("idiomas")]
        public IActionResult SaveIdioma(Idioma idioma)
        {
            try
            {
                var status = _translate.SaveIdioma(idioma);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("idiomas")]
        public IActionResult GetAllIdiomas()
        {
            try
            {
                var all = _translate.GetAllIdiomas();
                return Ok(all);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("organizacion/XML")]
        public IActionResult Exportar_a_XML(Guid Id_organizacion)
        {
            try
            {
                Organizacion org = _user.GetAllOrganizaciones(Guid.Empty,Id_organizacion).FirstOrDefault();

                string xml = XMLservice.generateXML(org);
                return Ok(xml);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("organizacion/XML")]
        public IActionResult Importar_XML([FromBody] string xml)
        {
            try
            {

                Organizacion org = XMLservice.deserializarXML(xml);
                _user.SaveOrganizacion(org);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("publicaciones")]
        public IActionResult SavePublicacion([FromBody] Publicacion publi)
        {
            try
            {
                if (publi == null)
                    return BadRequest("La publicacion no puede ser nula");
            
                _publicacion.SavePublicacion(publi);
                return Ok("Publicacion guardada maquina");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("publicaciones/tipos")]
        public IActionResult GetAllPublicacionTipos()
        {
            try
            {
                var all = _publicacion.GetAllTipos();
                return Ok(all);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("publicaciones/user/{idUser}")]
        public IActionResult getPublicacionesByUser(Guid idUser)
        {
            try
            {
                var all = _publicacion.GetPublicacionesByUser(idUser);
                return Ok(all);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("user/preferencias")]
        public IActionResult SavePreferenciasForUser(Guid User,[FromBody] List<Opcion> op)
        {
            try
            {
                if (op == null)
                    return BadRequest("La opcion no puede ser nula");
                if (User == Guid.Empty)
                    return BadRequest("Se necesita un id de ususario");

                _user.SavePreferenciaForUser(User, op);
                return Ok("Preferencias de ususario guardada maquina");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/match")]//probar
        public IActionResult getMatchsForUser(Guid idUser)
        {
            try
            {
                if (idUser == Guid.Empty)
                    return BadRequest("Necesito un id de usuario mi rey");

                var all = _user.GetMatchsForUser(idUser);
                return Ok(all);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("categorias")]
        public IActionResult GetAllCategories(string? nombre)
        {
            try
            {

                var all = _publicacion.GetAllcategorias(nombre);
                return Ok(all);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("categorias")]
        public IActionResult SaveCategoria([FromBody] Categoria categoria)
        {
            try
            {
                if (categoria == null)
                    return BadRequest("La categoria no puede ser nula");


                var cat = _publicacion.SaveCategoria(categoria);
                return Ok(cat);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("categorias/{idCategoria}")]
        public IActionResult DeleteCategoria(Guid idCategoria)
        {
            try
            {
                if (idCategoria == Guid.Empty)
                    return BadRequest("La categoria no puede ser nula");


                _publicacion.DeleteCategoria(idCategoria);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("publicacion/like")]//funca bien
        public IActionResult like_publicacion(Guid id_user,Guid id_publicacion)
        {
            try
            {
                if (id_user == Guid.Empty)
                    return BadRequest("Necesito un id de ususario");
                if (id_publicacion == Guid.Empty)
                    return BadRequest("Necesito un id de publicacion");


                _publicacion.like_publicacion(id_user,id_publicacion);
                return Ok("Publicacion matcheada, like guardado");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("publicacion/dislike")]//probar
        public IActionResult dislike_publicacion(Guid id_user, Guid id_publicacion)
        {
            try
            {
                if (id_user == Guid.Empty)
                    return BadRequest("Necesito un id de ususario");
                if (id_publicacion == Guid.Empty)
                    return BadRequest("Necesito un id de publicacion");


                _publicacion.Dislike_publicacion(id_user,id_publicacion);
                return Ok("Dislike guardado");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/like")]//funca bien
        public IActionResult GetLikesForUser(Guid Id_user)
        {
            try
            {
                if (Id_user == Guid.Empty)
                    return BadRequest("Necesito un id de usuario rey");

                var all = _publicacion.get_matchs(Id_user);
                return Ok(all);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
