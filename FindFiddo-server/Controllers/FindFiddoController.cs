using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using FindFiddo.Entities;
using FindFiddo.Application;
using FindFiddo.Services;
using Microsoft.AspNetCore.Authorization;


namespace FindFiddo_Server.Controllers
{
    [Route("api/FindFiddo")]
    [ApiController]
    public class FindFiddoController : ControllerBase
    {
        private readonly ILogger<FindFiddoController> _logger;
        private readonly IConfiguration _config;
        private UsuarioApp _user; 

        public FindFiddoController(IConfiguration config, ILogger<FindFiddoController> logger)
        {
            _config = config;
            _logger = logger;
            _user = new UsuarioApp();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginUser logUser)
        {
            var user = _user.GetUserByEmail(logUser.email);
            if (user != null)
            {
                //if (EncryptService.VerifyPassword(logUser.password,user.salt,user.password))
                if (user.password == logUser.password)
                {
                    return Ok(user);
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
                User UsuarioNew = _user.SignUP(user);
                return Ok(UsuarioNew);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
