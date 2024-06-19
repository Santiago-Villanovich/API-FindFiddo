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
        [Route("Signup")]
        public IActionResult SignUp([FromBody] User user)
        {
            try
            {
                if (!(user.rol.Count() > 0)) //Si no tiene rol le asigno el usuario default
                {
                    user.rol = new List<Rol>
                    {
                        new Rol() { Id = Guid.Parse("05953C7E-3250-49B5-89C3-3C44C2D46A83"), nombre = "User" }
                    };

                }

                LogedUser UsrLoged = _user.SignUP(user);

                if (UsrLoged != null)
                {
                    _dv.UpdateUserDVTable("usuario",_user.GetAll()); //recalculo el dv de la tabla

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
                var lista = _dv.VerificarDigitoXuser(_user.GetAll());
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
                _dv.UpdateUserDVTable("usuario", _user.GetAll());
                return Ok("Restablecimiento de Digito Verificador exitoso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
