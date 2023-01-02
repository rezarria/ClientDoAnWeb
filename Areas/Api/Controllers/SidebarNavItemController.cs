#region

using Client.Areas.Admin.Contexts;
using Client.Areas.Admin.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

#endregion

namespace Client.Areas.Api.Controllers;

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

	private async Task<TOutputType[]> Get<TOutputType>(
		Expression<Func<SidebarNavItem, TOutputType>> expression,
		[FromQuery] Guid[]? ids,
		[FromQuery] int take = -1,
		[FromQuery] int skip = 0)
		where TOutputType : class
	{
		IQueryable<SidebarNavItem> query = _context.SidebarNavItem.AsQueryable();

		if (ids is not null && ids.Any())
			query = query.Where(x => ids.Contains(x.Id));

		if (take > -1)
			query = query.Take(take);

		if (skip > 0)
			query = query.Skip(skip);

		return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
	}

	[HttpGet]
	public async Task<IActionResult> GetAll(Guid[]? ids, int skip = -0, int take = -1)
	{
		return Ok(await Get(DTOs.SidebarNavItem.Get.Expression, ids, take, skip));
	}

	private async Task<SidebarNavItem?> GetById(Guid id)
	{
		SidebarNavItem? sidebarNavItem = await _context.SidebarNavItem
													   .Include(x => x.Childs)
													   .FirstOrDefaultAsync(predicate: x => x.Id == id, HttpContext.RequestAborted);

		if (sidebarNavItem == null) return null;

		return sidebarNavItem;
	}

	[HttpGet]
	[Route("danhsach")]
	public async Task<IActionResult> DanhSach()
	{
		return Ok(await _context.SidebarNavItem.Select(x => new
														    {
															    x.Id,
															    x.Name
														    }).ToListAsync(HttpContext.RequestAborted));
	}

	// [HttpGet]
	// [Route("single")]
	// public async Task<IActionResult> Single(Guid id)
	// {
	// 	_context.SidebarNavItem.Where(x => x.Id == id).Select(DTOs.SidebarNavItem.Get.Expression);
	// 	return Ok();
	// }

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
		return CreatedAtAction(nameof(GetAll), new { id = sidebarNavItem.Id }, sidebarNavItem);
	}

	// DELETE: api/SidebarNavItem/5
	[HttpDelete]
	public async Task<IActionResult> DeleteSidebarNavItem([FromBody] Guid[] id)
	{
		IQueryable<SidebarNavItem> sidebarNavItem = _context.SidebarNavItem.Where(x => id.Contains(x.Id));
		_context.SidebarNavItem.RemoveRange(sidebarNavItem);
		await _context.SaveChangesAsync();
		return Ok();
	}

	public async Task<IActionResult> Update(Guid id, JsonPatchDocument<SidebarNavItem> patchDocument)
	{
		SidebarNavItem? target = await _context.SidebarNavItem.FindAsync(new object[] { id }, HttpContext.RequestAborted);
		if (target is null) return NotFound();
		patchDocument.ApplyTo(target, ModelState);
		if (!ModelState.IsValid) return BadRequest(ModelState);
		_context.SaveChanges();
		return Ok();
	}

	/// <summary>
	///     Cập nhật mục theo id
	/// </summary>
	/// <param name="id">Guid</param>
	/// <param name="path">theo cấu trúc fast joson patch</param>
	/// <returns></returns>
	/// <response code="200">Cập nhật mục thành công và trả về mục</response>
	[HttpPatch]
	public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<SidebarNavItem> path)
	{
		SidebarNavItem? item = await _context.SidebarNavItem.FirstOrDefaultAsync(predicate: x => x.Id == id, HttpContext.RequestAborted);
		if (item is null)
			return NotFound();

		path.ApplyTo(item, ModelState);

		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		await _context.SaveChangesAsync(HttpContext.RequestAborted);

		return Ok(item);
	}

	private bool SidebarNavItemExists(Guid id)
	{
		return _context.SidebarNavItem.Any(e => e.Id == id);
	}
}