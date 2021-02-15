using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAC_VALES.Common.Enums;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;
using SAC_VALES.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SAC_VALES.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _dataContext;
        private readonly UserManager<UsuarioEntity> _userManager;

        public AccountController(IUserHelper userHelper, ICombosHelper combosHelper
            , DataContext dataContext, UserManager<UsuarioEntity> userManager)
        {
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _dataContext = dataContext;
            _userManager = userManager;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            Debug.WriteLine("TOKEN en reset 1");
            Debug.WriteLine(token);

            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Recuperación de contraseña invalido");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            Debug.WriteLine("TOKEN EN RESET 2");
            Debug.WriteLine(model.Token);
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var decoded_token = HttpUtility.UrlDecode(model.Token);

                    Debug.WriteLine("DECODED TOKEN");
                    Debug.WriteLine(decoded_token);

                    var result = await _userManager.ResetPasswordAsync(user, decoded_token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordCofirmation");
                    }
                    foreach ( var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordCofirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            Debug.WriteLine("entre a forgot password 1");

            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            Debug.WriteLine("entre a forgot password 2");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var token = HttpUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new { email = model.Email, token = token }, Request.Scheme);
                    //logger.Log(LogLevel.Warning, passwordResetLink);
                    Debug.WriteLine("RESET PASSWORD LINK");
                    Debug.WriteLine(LogLevel.Warning, passwordResetLink);
                    return View("ForgotPasswordConfirmation");
                }
                
                return View("ForgotPasswordConfirmation");
            }
           
            return View(model);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to login.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Empresa,Distribuidor")]
        public IActionResult Register()
        {

            if (User.Identity.IsAuthenticated && User.IsInRole("Empresa"))
            {
                AddUserViewModel modelDist = new AddUserViewModel
                {
                    UserTypes = _combosHelper.GetComboRolesEmpresa(),
                    UserTypeId = 1
                };

                return View(modelDist);

            }
            else if (User.Identity.IsAuthenticated && User.IsInRole("Distribuidor"))
            {
                AddUserViewModel modelCliente = new AddUserViewModel
                {
                    UserTypes = _combosHelper.GetComboRolesDistribuidor(),
                    UserTypeId = 2
                };

                return View(modelCliente);

            }

            AddUserViewModel model = new AddUserViewModel
            {
                UserTypes = _combosHelper.GetComboRolesAdmin()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Data.Entities.UsuarioEntity user = await _userHelper.AddUserAsync(model, "");
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.UserTypes = _combosHelper.GetComboRolesAdmin();
                    return View(model);
                }

                // registro de la tabla extendida

                if ((int)user.UserType == 0)
                {
                    _dataContext.Administrador.Add(new AdministradorEntity
                    {
                        status = true,
                        Nombre = model.FirstName,
                        Apellidos = model.LastName,
                        Telefono = model.PhoneNumber,
                        Email = model.Username,
                        Usuario = user
                    });

                    await _dataContext.SaveChangesAsync();
                }
                else if ((int)user.UserType == 3)
                {
                    _dataContext.Empresa.Add(new EmpresaEntity
                    {
                        NombreEmpresa = "",
                        NombreRepresentante = model.FirstName,
                        ApellidosRepresentante = model.LastName,
                        TelefonoRepresentante = model.PhoneNumber,
                        Email = model.Username,
                        representante = user
                    });
                    await _dataContext.SaveChangesAsync();
                }
                else if ((int)user.UserType == 1)
                {
                    var empresa = _dataContext.Empresa.Where(e => e.Email == User.Identity.Name).FirstOrDefault();

                    _dataContext.Distribuidor.Add(new DistribuidorEntity
                    {
                        Nombre = model.FirstName,
                        Apellidos = model.LastName,
                        Direccion = model.Address,
                        Telefono = model.PhoneNumber,
                        Email = model.Username,
                        EmpresaVinculada = empresa,
                        UsuarioVinculado = user
                    });
                    await _dataContext.SaveChangesAsync();
                }
                else if ((int)user.UserType == 2)
                {
                    var distribuidor = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    _dataContext.Cliente.Add(new ClienteEntity
                    {
                        Nombre = model.FirstName,
                        Apellidos = model.LastName,
                        Direccion = model.Address,
                        Telefono = model.PhoneNumber,
                        Email = model.Username,
                        Distribuidor = distribuidor,
                        Cliente = user

                    });
                    await _dataContext.SaveChangesAsync();
                }
            }

            model.UserTypes = _combosHelper.GetComboRolesAdmin();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ChangeUser()
        {
            UsuarioEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Address = user.Direccion,
                FirstName = user.Nombre,
                LastName = user.Apellidos,
                PhoneNumber = user.PhoneNumber,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = "";

                UsuarioEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                user.Nombre = model.FirstName;
                user.Apellidos = model.LastName;
                user.Direccion = model.Address;
                user.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }

        

    }
}
