using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MicroActive.Security.AuthorizationServer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.Models;

namespace MicroActive.Security.AuthorizationServer.Web.Controllers
{
	[Authorize]
    public class ManageClientSecretsController : Controller
    {
        private readonly IdentityContext _context;

        public ManageClientSecretsController(IdentityContext context)
        {
            _context = context;    
        }

        // GET: ManageClientSecrets
        public async Task<IActionResult> Index()
        {
            var identityContext = _context.ClientSecrets.Include(c => c.Client);
            return View(await identityContext.ToListAsync());
        }

        // GET: ManageClientSecrets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientSecret = await _context.ClientSecrets
                .Include(c => c.Client)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (clientSecret == null)
            {
                return NotFound();
            }

            return View(clientSecret);
        }

        // GET: ManageClientSecrets/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientId");
            return View();
        }

        // POST: ManageClientSecrets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,Description,Expiration,Type,Value")] ClientSecret clientSecret)
        {
            if (ModelState.IsValid)
            {
				clientSecret.Value = clientSecret.Value.Sha256();

				_context.Add(clientSecret);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientId", clientSecret.ClientId);
            return View(clientSecret);
        }

        // GET: ManageClientSecrets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientSecret = await _context.ClientSecrets.SingleOrDefaultAsync(m => m.Id == id);
            if (clientSecret == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientId", clientSecret.ClientId);
            return View(clientSecret);
        }

        // POST: ManageClientSecrets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,Description,Expiration,Type,Value")] ClientSecret clientSecret)
        {
            if (id != clientSecret.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					clientSecret.Value = clientSecret.Value.Sha256();

					_context.Update(clientSecret);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientSecretExists(clientSecret.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "ClientId", clientSecret.ClientId);
            return View(clientSecret);
        }

        // GET: ManageClientSecrets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientSecret = await _context.ClientSecrets
                .Include(c => c.Client)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (clientSecret == null)
            {
                return NotFound();
            }

            return View(clientSecret);
        }

        // POST: ManageClientSecrets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientSecret = await _context.ClientSecrets.SingleOrDefaultAsync(m => m.Id == id);
            _context.ClientSecrets.Remove(clientSecret);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ClientSecretExists(int id)
        {
            return _context.ClientSecrets.Any(e => e.Id == id);
        }
    }
}
