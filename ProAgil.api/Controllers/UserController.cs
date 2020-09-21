using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProAgil.api.DTOs;
using ProAgil.Dominio.Identity;

namespace ProAgil.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _conif;
        public UserController(IMapper mapper, 
                              IConfiguration conif,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager)
        {
            this._mapper = mapper;
            this._conif = conif;
            this._userManager = userManager;
            this._signInManager = signInManager;
            
        }


        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
           
           try{
               
               return Ok(new UserDTO());

           }
           catch(Exception)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro");
           }
            
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
           
           try{
               var user = _mapper.Map<User>(userDTO);
               var result = await _userManager.CreateAsync(user, userDTO.Password);
               var userToReturn = _mapper.Map<UserDTO>(user);
               if(result.Succeeded)
               {
                   return Created("GetUser",userToReturn);
               }

                return BadRequest(result.Errors);

           }
           catch(Exception ex)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
           }
            
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
           
           try{
                var user = await _userManager.FindByNameAsync(userLogin.UserName);
                var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password,false);
               
                if(result.Succeeded)
                {
                    var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userLogin.UserName.ToUpper());
                    var userToReturn = _mapper.Map<UserLoginDTO>(appUser);

                    return Ok(new {
                        token = GenerateJWTToken(appUser).Result,
                        user = userToReturn
                    });
                }
               
               return Unauthorized();

           }
           catch(Exception ex)
           {
               return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
           }
        }

        private async Task<string> GenerateJWTToken(User appUser)
        {
            try{
                
                List<Claim> claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier,appUser.Id.ToString()),
                    new Claim(ClaimTypes.Name,appUser.UserName)
                };

                var roles = await _userManager.GetRolesAsync(appUser);
                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_conif.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
    }
        
}