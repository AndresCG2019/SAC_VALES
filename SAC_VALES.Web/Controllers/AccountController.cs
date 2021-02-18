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
using System.Net.Mail;
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
                    Debug.WriteLine(/*LogLevel.Warning,*/ passwordResetLink);

                    // ----- Start   CODIGO DE EMAIL
                    string EmailDestino = model.Email;
                    string EmailOrigen = "EvolSoftSoporte@gmail.com";
                    string Contraseña = "EvolSoft12345";
                    MailMessage oMailMessage = new MailMessage(EmailOrigen, EmailDestino, "Recuperación de contraseña",
                        "<p>Correo para recuperación de contraseña</p><br>" +
                        "<a href='" + passwordResetLink + "'>Click para recuperar</a>");

                    oMailMessage.IsBodyHtml = true;

                    SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
                    oSmtpClient.EnableSsl = true;
                    oSmtpClient.UseDefaultCredentials = false;
                    oSmtpClient.Port = 587;
                    oSmtpClient.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);

                    oSmtpClient.Send(oMailMessage);

                    oSmtpClient.Dispose();
                    // -- End CODIGO DE EMAIL


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

        public IActionResult Register(int? id)
        {
            Debug.WriteLine("ENTRE A REGISTER");

            if (User.Identity.IsAuthenticated && User.IsInRole("Distribuidor") || id == 1)
            {
                int NavID = 0;

                if (id == 1) NavID = 1; else NavID = 2;

                AddUserViewModel modelCliente = new AddUserViewModel
                {
                    UserTypes = _combosHelper.GetComboRolesDistribuidor(),
                    UserTypeId = 2,
                    navId = NavID

                };

                return View(modelCliente);

            }

            else if ( !User.Identity.IsAuthenticated ||(User.Identity.IsAuthenticated && User.IsInRole("Empresa")) ) 
            {
                AddUserViewModel modelDist = new AddUserViewModel
                {
                    UserTypes = _combosHelper.GetComboRolesEmpresa(),
                    UserTypeId = 1
                };

                return View(modelDist);

            }
          
                AddUserViewModel modelAdmin = new AddUserViewModel
                {
                    UserTypes = _combosHelper.GetComboRolesAdmin(),
                    UserTypeId = 0
                };

                return View(modelAdmin);
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
                        Email =model.Username,
                        AdminAuth = user
                    });

                    await _dataContext.SaveChangesAsync();
                }
                else if ((int)user.UserType == 3)
                {
                    _dataContext.Empresa.Add(new EmpresaEntity
                    {
                        NombreEmpresa = "Nombre Pendiente...",
                        NombreRepresentante = model.FirstName,
                        ApellidosRepresentante = model.LastName,
                        TelefonoRepresentante = model.PhoneNumber,
                        Email = model.Username,
                        Direccion = model.Address,
                        EmpresaAuth = user
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
                        DistribuidorAuth = user,

                    });
                    await _dataContext.SaveChangesAsync();
                }
                else if ((int)user.UserType == 2 && model.navId != 1)
                {
                    var distribuidor = _dataContext.Distribuidor.Where(d => d.Email == User.Identity.Name).FirstOrDefault();
                    //var testDist = _dataContext.Distribuidor.Where(d => d.Email == "ortega@yopmail.com").FirstOrDefault();

                    ClienteEntity cliente = new ClienteEntity()
                    {
                        Nombre = model.FirstName,
                        Apellidos = model.LastName,
                        Direccion = model.Address,
                        Telefono = model.PhoneNumber,
                        Email = model.Username,
                        ClienteAuth = user,
                    };

                    _dataContext.Cliente.Add(cliente);
                    await _dataContext.SaveChangesAsync();

                    _dataContext.ClienteDistribuidor.Add(new ClienteDistribuidor
                    {
                        Cliente = cliente,
                        ClienteId = cliente.id,
                        Distribuidor = distribuidor,
                        DistribuidorId = distribuidor.id
                    });

                    await _dataContext.SaveChangesAsync();
                }

                else if (model.navId == 1) 
                {
                    ClienteEntity cliente = new ClienteEntity()
                    {
                        Nombre = model.FirstName,
                        Apellidos = model.LastName,
                        Direccion = model.Address,
                        Telefono = model.PhoneNumber,
                        Email = model.Username,
                        ClienteAuth = user,
                    };

                    _dataContext.Cliente.Add(cliente);
                    await _dataContext.SaveChangesAsync();
                }
            }

            //model.UserTypes = _combosHelper.GetComboRolesAdmin()
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ChangeUser()
        {
            UsuarioEntity authUser = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            if (authUser == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel();

            switch (authUser.UserType)
            {
                case UserType.Admin:
                    AdministradorEntity admin = _dataContext.Administrador
                        .Where(a => a.Email == User.Identity.Name).FirstOrDefault();

                    //esto se hace solo en el caso de admin ya que es el unico con registros en seed
                    if (admin == null)
                    {
                        return NotFound();
                    }

                    Debug.WriteLine("user type en casa admin es :");
                    Debug.WriteLine(authUser.UserType);

                    model.FirstName = admin.Nombre;
                    model.LastName = admin.Apellidos;
                    model.PhoneNumber = admin.Telefono;
                    model.userType = UserType.Admin;

                    break;

                case UserType.Distribuidor:

                    DistribuidorEntity dist = _dataContext.Distribuidor
                        .Where(d => d.Email == User.Identity.Name).FirstOrDefault();

                    Debug.WriteLine("user type en casa dist es :");
                    Debug.WriteLine(authUser.UserType);

                    model.FirstName = dist.Nombre;
                    model.LastName = dist.Apellidos;
                    model.PhoneNumber = dist.Telefono;
                    model.Address = dist.Direccion;
                    model.userType = UserType.Distribuidor;

                    break;

                case UserType.Cliente:

                    ClienteEntity cliente = _dataContext.Cliente
                        .Where(c => c.Email == User.Identity.Name).FirstOrDefault();

                    Debug.WriteLine("user type en casa cliente es :");
                    Debug.WriteLine(authUser.UserType);

                    model.FirstName = cliente.Nombre;
                    model.LastName = cliente.Apellidos;
                    model.PhoneNumber = cliente.Telefono;
                    model.Address = cliente.Direccion;
                    model.userType = UserType.Cliente;

                    break;

                case UserType.Empresa:

                    EmpresaEntity empresa = _dataContext.Empresa
                        .Where(e => e.Email == User.Identity.Name).FirstOrDefault();

                    Debug.WriteLine("user type en casa empresa es :");
                    Debug.WriteLine(authUser.UserType);

                    model.FirstName = empresa.NombreRepresentante;
                    model.LastName = empresa.ApellidosRepresentante;
                    model.PhoneNumber = empresa.TelefonoRepresentante;
                    model.Address = empresa.Direccion;
                    model.userType = UserType.Empresa;

                    break;
            }

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

                await _userHelper.UpdateUserAsync(user);

                switch (model.userType)
                {
                    case UserType.Admin:

                        AdministradorEntity admin =  _dataContext.Administrador
                            .Where(a => a.Email == User.Identity.Name).FirstOrDefault();

                        admin.Nombre = model.FirstName;
                        admin.Apellidos = model.LastName;
                        admin.Telefono = model.PhoneNumber;

                        _dataContext.Update(admin);
                        await _dataContext.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");

                    case UserType.Distribuidor:
                        DistribuidorEntity dist = _dataContext.Distribuidor
                            .Where(d => d.Email == User.Identity.Name).FirstOrDefault();

                        dist.Nombre = model.FirstName;
                        dist.Apellidos = model.LastName;
                        dist.Telefono = model.PhoneNumber;

                        _dataContext.Update(dist);
                        await _dataContext.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");

                    case UserType.Cliente:

                        ClienteEntity cliente = _dataContext.Cliente
                            .Where(c => c.Email == User.Identity.Name).FirstOrDefault();

                        cliente.Nombre = model.FirstName;
                        cliente.Apellidos = model.LastName;
                        cliente.Telefono = model.PhoneNumber;

                        _dataContext.Update(cliente);
                        await _dataContext.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");

                    case UserType.Empresa:

                        EmpresaEntity empresa = _dataContext.Empresa
                            .Where(e => e.Email == User.Identity.Name).FirstOrDefault();

                        empresa.NombreRepresentante = model.FirstName;
                        empresa.ApellidosRepresentante = model.LastName;
                        empresa.TelefonoRepresentante = model.PhoneNumber;

                        _dataContext.Update(empresa);
                        await _dataContext.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");


                    default:
                        break;
                }

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
