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
        public ActionResult<User> Login([FromBody] LoginUser logUser)
        {
            var user = _user.GetUserByEmail(logUser.email);
            if (user != null)
            {
                if (EncryptService.VerifyPassword(logUser.password,user.salt,user.password))
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

        /*[Route("register")]
        [HttpPost]
        public ActionResult<LoginResponseSchema> Register([FromBody] RegisterSchema schema)
        {

            using SqlConnection con = new SqlConnection(_config.GetConnectionString("defaultConnection"));
            using SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "registerUser";

            var salt = Hash.GenerateSalt();

            var hashedPassword = Hash.Compute(schema.Password, salt);

            cmd.Parameters.AddWithValue("@Email", schema.Email);
            cmd.Parameters.AddWithValue("@DNI", schema.DNI);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);
            cmd.Parameters.AddWithValue("@Name", schema.Name);
            cmd.Parameters.AddWithValue("@Salt", salt);

            con.Open();

            cmd.ExecuteNonQuery();

            return Ok(new LoginResponseSchema() { Name = schema.Name, Email = schema.Email, DNI = schema.DNI });

        }*/
    }
}
