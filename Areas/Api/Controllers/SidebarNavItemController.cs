#region

using Client.Areas.Admin.Contexts;
using Client.Areas.Admin.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Client.Areas.Api.Controllers
{
	[Area("Api")]
	[Route("Api/[controller]")]
	[ApiController]
	public class SidebarNavItemController : ControllerBase
	{
		private readonly ClientDbContext _context;

		public SidebarNavItemController(ClientDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Get(Guid? id, int? skip, int? take)
		{
			if (id is not null)
				return Ok(await GetById(id.Value));
			return Ok(await GetAll(skip, take));
		}

		private async Task<IEnumerable<SidebarNavItem>> GetAll(int? skip, int? take)
		{
			skip ??= 0;
			take ??= 1000;
			return await _context.SidebarNavItem
				.Where(x => x.ParentId == null)
				.Skip(skip.Value)
				.Take(take.Value)
				.ToListAsync(HttpContext.RequestAborted);
		}

		private async Task<SidebarNavItem?> GetById(Guid id)
		{
			var sidebarNavItem = await _context.SidebarNavItem
				.Include(x => x.Childs)
				.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);

			if (sidebarNavItem == null) return null;

			return sidebarNavItem;
		}

		// PUT: api/SidebarNavItem/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut]
		public async Task<IActionResult> PutSidebarNavItem(Guid id, SidebarNavItem sidebarNavItem)
		{
			if (id != sidebarNavItem.Id) return BadRequest();

			_context.Entry(sidebarNavItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SidebarNavItemExists(id))
					return NotFound();
				throw;
			}

			return NoContent();
		}

		// POST: api/SidebarNavItem
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<SidebarNavItem>> PostSidebarNavItem(SidebarNavItem sidebarNavItem)
		{
			_context.SidebarNavItem.Attach(sidebarNavItem);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { id = sidebarNavItem.Id }, sidebarNavItem);
		}

		// DELETE: api/SidebarNavItem/5
		[HttpDelete]
		public async Task<IActionResult> DeleteSidebarNavItem(Guid id)
		{
			var sidebarNavItem = await _context.SidebarNavItem.FindAsync(id);
			if (sidebarNavItem == null) return NotFound();

			_context.SidebarNavItem.Remove(sidebarNavItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		public async Task<IActionResult> Update(Guid id, JsonPatchDocument<SidebarNavItem> patchDocument)
		{
			var target = await _context.SidebarNavItem.FindAsync(new object[] { id }, HttpContext.RequestAborted);
			if (target is null) return NotFound();
			patchDocument.ApplyTo(target, ModelState);
			if (!ModelState.IsValid) return BadRequest(ModelState);
			_context.SaveChanges();
			return Ok();
		}

		private bool SidebarNavItemExists(Guid id)
		{
			return _context.SidebarNavItem.Any(e => e.Id == id);
		}
	}
}