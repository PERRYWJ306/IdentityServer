using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroActive.Security.AuthorizationServer.Data.Models;

namespace MicroActive.Security.AuthorizationServer.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/AspNetRoles")]
    public class AspNetRolesController : Controller
    {
        private readonly IdentityContext _context;

        public AspNetRolesController(IdentityContext context)
        {
            _context = context;
        }

        // GET: api/AspNetRoles
        [HttpGet]
        public IEnumerable<AspNetRole> GetAspNetRoles()
        {
            return _context.AspNetRoles;
        }

        // GET: api/AspNetRoles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAspNetRole([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var aspNetRole = await _context.AspNetRoles.SingleOrDefaultAsync(m => m.Id == id);

            if (aspNetRole == null)
            {
                return NotFound();
            }

            return Ok(aspNetRole);
        }

        // PUT: api/AspNetRoles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAspNetRole([FromRoute] string id, [FromBody] AspNetRole aspNetRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aspNetRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(aspNetRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetRoleExists(id))
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

        // POST: api/AspNetRoles
        [HttpPost]
        public async Task<IActionResult> PostAspNetRole([FromBody] AspNetRole aspNetRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AspNetRoles.Add(aspNetRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAspNetRole", new { id = aspNetRole.Id }, aspNetRole);
        }

        // DELETE: api/AspNetRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAspNetRole([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var aspNetRole = await _context.AspNetRoles.SingleOrDefaultAsync(m => m.Id == id);
            if (aspNetRole == null)
            {
                return NotFound();
            }

            _context.AspNetRoles.Remove(aspNetRole);
            await _context.SaveChangesAsync();

            return Ok(aspNetRole);
        }

        private bool AspNetRoleExists(string id)
        {
            return _context.AspNetRoles.Any(e => e.Id == id);
        }
    }
}