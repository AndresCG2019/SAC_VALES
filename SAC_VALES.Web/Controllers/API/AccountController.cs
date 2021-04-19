using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using SAC_VALES.Common.Enums;
using SAC_VALES.Common.Models;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SAC_VALES.Web.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace SAC_VALES.Web.Controllers.API
{
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IConverterHelper _converterHelper;

        public AccountController(
            DataContext dataContext,
            IUserHelper userHelper,
            IConfiguration configuration,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _configuration = configuration;
            _converterHelper = converterHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            UsuarioEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user != null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "El usuario ya existe"
                });
            }

            user = new UsuarioEntity
            {
                Email = request.Email,
                UserName = request.Email,
                UserType = request.UserTypeId == 1 ? UserType.Distribuidor : UserType.Cliente
            };

            IdentityResult result = await _userHelper.AddUserAsync(user, request.Password);
            await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());

            if (user.UserType == UserType.Distribuidor)
            {
                _dataContext.Distribuidor.Add(new DistribuidorEntity
                {
                    Nombre = request.Nombre,
                    Apellidos = request.Apellidos,
                    Direccion = request.Direccion,
                    Telefono = request.Telefono,
                    Email = request.Email,
                    DistribuidorAuth = user,

                });
                await _dataContext.SaveChangesAsync();
            }
            else if (user.UserType == UserType.Cliente) 
            {
                _dataContext.Cliente.Add(new ClienteEntity
                {
                    Nombre = request.Nombre,
                    Apellidos = request.Apellidos,
                    Direccion = request.Direccion,
                    Telefono = request.Telefono,
                    Email = request.Email,
                    ClienteAuth = user,

                });
                await _dataContext.SaveChangesAsync();
            }

            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "completado"
            });
        }

        [HttpPost]
        [Route("PostClientAsDist")]
        public async Task<IActionResult> PostClientAsDist([FromBody] PostClieAsDistRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            UsuarioEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user != null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "El usuario ya existe"
                });
            }

            user = new UsuarioEntity
            {
                Email = request.Email,
                UserName = request.Email,
                UserType = UserType.Cliente
            };

            IdentityResult result = await _userHelper.AddUserAsync(user, request.Password);
            await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());

            ClienteEntity cliente = new ClienteEntity()
            {
                Nombre = request.Nombre,
                Apellidos = request.Apellidos,
                Direccion = request.Direccion,
                Telefono = request.Telefono,
                Email = request.Email,
                ClienteAuth = user,
            };

            _dataContext.Cliente.Add(cliente);
            await _dataContext.SaveChangesAsync();

            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

            var distribuidor = _dataContext.Distribuidor.Where(d => d.id == request.DistId).FirstOrDefault();

            if (distribuidor == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "El distribuidor especificado no existe."
                });
            }

            _dataContext.ClienteDistribuidor.Add(new ClienteDistribuidor
            {
                Cliente = cliente,
                ClienteId = cliente.id,
                Distribuidor = distribuidor,
                DistribuidorId = distribuidor.id
            });

            await _dataContext.SaveChangesAsync();

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "completado"
            });
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UsuarioEntity userEntity = await _userHelper.GetUserAsync(request.Email);
            if (userEntity == null)
            {
                return BadRequest("No se encontro el usuario");
            }


            IdentityResult respose = await _userHelper.UpdateUserAsync(userEntity);
            if (!respose.Succeeded)
            {
                return BadRequest(respose.Errors.FirstOrDefault().Description);
            }

            UsuarioEntity updatedUser = await _userHelper.GetUserAsync(request.Email);
            return Ok(updatedUser);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            UsuarioEntity user = await _userHelper.GetUserAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "No se encontro el usuario"
                });
            }

            IdentityResult result = await _userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = result.Errors.FirstOrDefault().Description
                });
            }

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "Se cambio la contraseña exitosamente"
            });
        }

        [HttpPost]
        [Route("CreateToken")]

        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(user, model.Contraseña);

                    if (result.Succeeded)
                    {

                        
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(99),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            UsuarioEntity userEntity = await _userHelper.GetUserAsync(emailRequest.Email);
            if (userEntity == null)
            {
                return NotFound("no se encontro el usuario");
            }

            if (userEntity.UserType == UserType.Distribuidor)
            {
                DistribuidorEntity dist = _dataContext.Distribuidor
                    .Where(d => d.Email == emailRequest.Email)
                    .FirstOrDefault();

                return Ok(_converterHelper.ToDistUserResponse(userEntity, dist));
            }
            else
            {
                ClienteEntity clie = _dataContext.Cliente
                    .Where(c => c.Email == emailRequest.Email)
                    .FirstOrDefault();

                return Ok(_converterHelper.ToClieUserResponse(userEntity, clie));
            }
        }


    }
}
