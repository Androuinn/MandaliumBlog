using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Mandalium.API.Data;
using Mandalium.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Mandalium.API.Dtos;
using System.Text;
using System;
using Mandalium.API.Helpers;

namespace Mandalium.API.Controllers
{
    [Route("api/[controller]/writer")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository<User> _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository<User> repo, IConfiguration config, IMapper mapper)
        {
            this._mapper = mapper;
            this._config = config;
            this._repo = repo;
        }

       
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            // kullanıcı capslede girer küçük harf ile de
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Kullanıcı Mevcut");
            }

            var writerToCreate = _mapper.Map<User>(userForRegisterDto);

            var createdWriter = await _repo.Register(writerToCreate, userForRegisterDto.Password);

            var text = System.IO.File.ReadAllText(@"../Mandalium.API/MailTemplates/UserCreatedTemplate.html");

            Extensions.SendMail(createdWriter.Email, "Kayıt Doğrulama Maili", string.Format(text, createdWriter.Name + " " + createdWriter.Surname), false);

            return StatusCode(200);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {

            var writerFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (writerFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, writerFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, writerFromRepo.Username),
                new Claim(ClaimTypes.Role, Convert.ToSingle(writerFromRepo.Role).ToString())
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
            });

        }


    }
}