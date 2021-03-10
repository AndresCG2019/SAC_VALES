﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAC_VALES.Web.Data;
using SAC_VALES.Web.Data.Entities;

namespace SAC_VALES.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteEntitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public ClienteEntitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ClienteEntities
        [HttpGet]
        public IEnumerable<ClienteEntity> GetCliente()
        {
            return _context.Cliente;
        }

        // GET: api/ClienteEntities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteEntity = await _context.Cliente.FindAsync(id);

            if (clienteEntity == null)
            {
                return NotFound();
            }

            return Ok(clienteEntity);
        }

        // PUT: api/ClienteEntities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClienteEntity([FromRoute] int id, [FromBody] ClienteEntity clienteEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clienteEntity.id)
            {
                return BadRequest();
            }

            _context.Entry(clienteEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ClienteEntities
        [HttpPost]
        public async Task<IActionResult> PostClienteEntity([FromBody] ClienteEntity clienteEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Cliente.Add(clienteEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClienteEntity", new { id = clienteEntity.id }, clienteEntity);
        }

        // DELETE: api/ClienteEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClienteEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteEntity = await _context.Cliente.FindAsync(id);
            if (clienteEntity == null)
            {
                return NotFound();
            }

            _context.Cliente.Remove(clienteEntity);
            await _context.SaveChangesAsync();

            return Ok(clienteEntity);
        }

        private bool ClienteEntityExists(int id)
        {
            return _context.Cliente.Any(e => e.id == id);
        }
    }
}