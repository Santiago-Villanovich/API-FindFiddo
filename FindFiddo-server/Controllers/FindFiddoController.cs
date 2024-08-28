using FindFiddo.Application;
using FindFiddo.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FindFiddo.Services;

namespace FindFiddo_Server.Controllers
{
    [Route("api/FindFiddo")]
    [ApiController]
    public class FindFiddoController : ControllerBase
    {
        private readonly ILogger<FindFiddoController> _logger;
        private DigitoVerificadorService _dv;
        private UsuarioApp _user;

        public FindFiddoController(IConfiguration config, ILogger<FindFiddoController> logger)
        {
            _logger = logger;
            _user = new UsuarioApp(config);
            _dv = new DigitoVerificadorService(config);
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

                LogedUser UsrLoged = _user.SignUP(user);

                if (UsrLoged != null)
                {
                    _dv.UpdateDVTable("usuario",_user.GetAll()); //recalculo el dv de la tabla
                    UsrLoged.rol = _user.GetUserRols(UsrLoged.Id);
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
    }
}
