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

        private UsuarioApp _user;

        public FindFiddoController(IConfiguration config, ILogger<FindFiddoController> logger)
        {
            _logger = logger;
            _user = new UsuarioApp(config);
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
                    return Ok(new LogedUser(user.Id,user.telefono,user.email,user.nombres,user.apellidos));
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
                return Ok(UsrLoged);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
