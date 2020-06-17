using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MicroActive.Security.AuthorizationServer.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace MicroActive.Security.AuthorizationServer.Web.Controllers
{
	[Authorize]
    public class ManageUsersController : Controller
    {
        private readonly IdentityContext _context;

        public ManageUsersController(IdentityContext context)
        {
            _context = context;    
        }

        // GET: ManageUsers
        public async Task<IActionResult> Index()
        {
            var identityContext = _context.AspNetUsers.Include(a => a.DeletedByUser);
            return View(await identityContext.ToListAsync());
        }

        // GET: ManageUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .Include(a => a.DeletedByUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // GET: ManageUsers/Create
        public IActionResult Create()
        {
            ViewData["DeletedByUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: ManageUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName,Deleted,DeletedByUserId,DeletedDate,FirstName,LastName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DeletedByUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUser.DeletedByUserId);
            return View(aspNetUser);
        }

        // GET: ManageUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            ViewData["DeletedByUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUser.DeletedByUserId);
            return View(aspNetUser);
        }

        // POST: ManageUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName,Deleted,DeletedByUserId,DeletedDate,FirstName,LastName")] AspNetUser aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserExists(aspNetUser.Id))
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
            ViewData["DeletedByUserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUser.DeletedByUserId);
            return View(aspNetUser);
        }

        // GET: ManageUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .Include(a => a.DeletedByUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // POST: ManageUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUser = await _context.AspNetUsers.SingleOrDefaultAsync(m => m.Id == id);
            _context.AspNetUsers.Remove(aspNetUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
