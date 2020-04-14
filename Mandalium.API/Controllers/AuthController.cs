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
using Microsoft.AspNetCore.Authorization;

namespace Mandalium.API.Controllers
{
    [Route("api/[controller]/writer")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository<Writer> _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository<Writer> repo, IConfiguration config, IMapper mapper)
        {
            this._mapper = mapper;
            this._config = config;
            this._repo = repo;
        }

       
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]WriterForRegisterDto writerForRegisterDto)
        {
            // kullanıcı capslede girer küçük harf ile de
            writerForRegisterDto.Username = writerForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(writerForRegisterDto.Username))
            {
                return BadRequest("Kullanıcı Mevcut");
            }

            var writerToCreate = _mapper.Map<Writer>(writerForRegisterDto);

            var createdWriter = await _repo.Register(writerToCreate, writerForRegisterDto.Password);

            return RedirectToRoute("GetEntries", new { controller = "BlogEntry" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] WriterForLoginDto writerForLoginDto)
        {

            var writerFromRepo = await _repo.Login(writerForLoginDto.Username.ToLower(), writerForLoginDto.Password);

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