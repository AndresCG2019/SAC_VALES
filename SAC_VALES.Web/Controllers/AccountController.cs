﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAC_VALES.Common.Enums;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;
using SAC_VALES.Web.Helpers;
using SAC_VALES.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAC_VALES.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _dataContext;

        public AccountController(IUserHelper userHelper, ICombosHelper combosHelper, DataContext dataContext)
        {
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _dataContext = dataContext;
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
                        Nombre = "omar",
                        ApellidoP = "gomez",
                        ApellidoM = "arreola",
                        Telefono = "6184192931",
                        Usuario = user
                    });

                    await _dataContext.SaveChangesAsync();
                }
                else if ((int)user.UserType == 3)
                {
                    _dataContext.Empresa.Add(new EmpresaEntity
                    {
                        NombreEmpresa = "EL PORTON",
                        representante = user
                    });
                    await _dataContext.SaveChangesAsync();
                }
                else if ((int)user.UserType == 1)
                {
                    var empresa = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                    _dataContext.Distribuidor.Add(new DistribuidorEntity
                    {
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
                        Distribuidor = distribuidor,
                        Cliente

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
                Address = user.Address,
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PicturePath = user.PicturePath
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = model.PicturePath;

                UsuarioEntity user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                user.Document = model.Document;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.PicturePath = path;

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
