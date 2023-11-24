using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ServiceManagerApi.Data;
using ServiceManagerApi.Dtos.BacklogDto;

namespace ServiceManagerApi.Controllers.Esms;

[Route("api/[controller]")]
[ApiController]
public class BacklogsController : BaeApiController<BacklogsController>
{
    private readonly IMemoryCache _cache;
    private readonly EnpDBContext _context;
    private readonly ILogger<BacklogsController> _logger;

    public BacklogsController(EnpDBContext context, ILogger<BacklogsController> logger, IMemoryCache cache)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    // GET: api/Backlog/tenant/{tenantId}
    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<IEnumerable<BacklogDto>>> GetBacklog(
        string tenantId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize
    )
    {
        if (_cache.TryGetValue($"backlogs", out List<BacklogDto> backlogDtos))
        {
            _logger.LogInformation(
                $"BacklogController.GetBacklog: backlog items found for all tenants from cache");
            var backlogDtosForSingleTenant =
                backlogDtos.Where(backlog => backlog.TenantId == tenantId && backlog.Status != "Completed");

            if (pageNumber.HasValue && pageSize.HasValue)
                backlogDtosForSingleTenant = backlogDtosForSingleTenant
                    .OrderByDescending(b => b.Id)
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();

            return Ok(backlogDtosForSingleTenant);
        }

        if (_context.Backlogs == null) return NotFound();
        //get all backlog items for all tenants
        var backlogDtosFromDb = _mapper.Map<List<BacklogDto>>(await _context.Backlogs
            .Include(backlog => backlog.WorkOrder)
            .ToListAsync());

//cache the backlog items for single tenants
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.Normal);
        _cache.Set($"backlogs", backlogDtosFromDb, cacheEntryOptions);
        _logger.LogInformation(
            $"BacklogController.GetBacklog: backlog items found from DB");


        var backlogDtosFromDbSingleTenant =
            backlogDtosFromDb.Where(backlog => backlog.TenantId == tenantId && backlog.Status != "Completed");

        if (pageNumber.HasValue && pageSize.HasValue)
            backlogDtosFromDbSingleTenant = backlogDtosFromDbSingleTenant
                .OrderByDescending(b => b.Id)
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToList();

        return Ok(backlogDtosFromDbSingleTenant);
    }

// GET: api/Backlog/tenant/{tenantId}/status/{status}
    [HttpGet("tenant/{tenantId}/status/{status}")]
    public async Task<ActionResult<IEnumerable<BacklogDto>>> GetBacklog(string tenantId, string status,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
        if (_cache.TryGetValue($"backlogsCompleted", out List<BacklogDto> backlogDtos))
        {
            _logger.LogInformation(
                $"BacklogController.GetBacklog: backlog items found for all tenants from cache");
            var backlogDtosForSingleTenant =
                backlogDtos.Where(backlog => backlog.TenantId == tenantId && backlog.Status == status);

            //totalItem
            if (pageNumber.HasValue && pageSize.HasValue)
                backlogDtosForSingleTenant = backlogDtosForSingleTenant
                    .OrderByDescending(backlog => backlog.Cdate)
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();

            return Ok(backlogDtosForSingleTenant);
        }

        if (_context.Backlogs == null) return NotFound();
        //get all backlog items for all tenants
        var backlogDtosFromDb = _mapper.Map<List<BacklogDto>>(await _context.Backlogs.Include(
                backlog => backlog.WorkOrder
            )
            .ToListAsync());


        //cache the backlog items for all tenants
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.Normal);

        _cache.Set($"backlogsCompleted", backlogDtosFromDb, cacheEntryOptions);

        _logger.LogInformation(
            $"BacklogController.GetBacklog: backlog items found from DB");

        var backlogDtosFromDbSingleTenant =
            backlogDtosFromDb.Where(backlog => backlog.TenantId == tenantId && backlog.Status == status);

        if (pageNumber.HasValue && pageSize.HasValue)
            backlogDtosFromDbSingleTenant = backlogDtosFromDbSingleTenant
                .OrderByDescending(backlog => backlog.Cdate)
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToList();

        return Ok(backlogDtosFromDbSingleTenant);
    }

    [HttpGet("tenant/{tenantId}/status/{status}/count")]
    public async Task<ActionResult<int>> GetBacklogCount(string tenantId, string status)
    {
        if (_cache.TryGetValue($"backlogsCompleted", out List<BacklogDto> backlogDtos))
        {
            _logger.LogInformation(
                $"BacklogController.GetBacklog: backlog items found for all tenants from cache");
            var backlogDtosForSingleTenant =
                backlogDtos.Where(backlog => backlog.TenantId == tenantId && backlog.Status == status);

            return Ok(backlogDtosForSingleTenant.Count());
        }

        if (_context.Backlogs == null) return NotFound();
        //get all backlog items for all tenants
        var backlogDtosFromDb = _mapper.Map<List<BacklogDto>>(await _context.Backlogs
            .ToListAsync());

        //cache the backlog items for all tenants
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.Normal);

        _cache.Set($"backlogsCompleted", backlogDtosFromDb, cacheEntryOptions);

        _logger.LogInformation(
            $"BacklogController.GetBacklog: backlog items found from DB");

        var backlogDtosFromDbSingleTenant =
            backlogDtosFromDb.Where(backlog => backlog.TenantId == tenantId && backlog.Status == status);

        return Ok(backlogDtosFromDbSingleTenant.Count());
    }


// GET: api/Backlog/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Backlog>> GetBacklog(int id)
    {
        if (_context.Backlogs == null) return NotFound();
        var backlog = await _context.Backlogs.FindAsync(id);

        if (backlog == null) return NotFound();

        return backlog;
    }

// PUT: api/Backlogs/5
// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBacklog(int id, BacklogPutDto backlogPutDto)
    {
        if (id != backlogPutDto.Id) return BadRequest();

        var backlog = _mapper.Map<Backlog>(backlogPutDto);
        _context.Entry(backlog).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _cache.Remove($"backlogs");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BacklogExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }


// PATCH: api/Backlogs/5
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Backlog> patchBacklog)
    {
        var backlog = await _context.Backlogs.FindAsync(id);

        if (backlog == null) return BadRequest();

        patchBacklog.ApplyTo(backlog, ModelState);

        await _context.SaveChangesAsync();
        _cache.Remove($"backlogs");
        _cache.Remove($"backlogsCompleted");
        return Ok(backlog);
    }

// POST: api/Backlog
// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Backlog>> PostBacklog(BacklogPostDto backlogPostDto)
    {
        if (_context.Backlogs == null) return Problem("Entity set 'EnpDbContext.Backlogs'  is null.");

        var backlog = _mapper.Map<Backlog>(backlogPostDto);
        _context.Backlogs.Add(backlog);
        await _context.SaveChangesAsync();

        //clear the cache
        _logger.LogInformation($"BacklogController.PostBacklog: Clearing cache for 'backlogs'");
        _cache.Remove($"backlogs");

        return CreatedAtAction("GetBacklog", new { id = backlog.Id }, backlog);
    }

// DELETE: api/Backlogs/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBacklogs(int id)
    {
        if (_context.Backlogs == null) return NotFound();
        var backlog = await _context.Backlogs.FindAsync(id);
        if (backlog == null) return NotFound();

        _context.Backlogs.Remove(backlog);
        await _context.SaveChangesAsync();
        _cache.Remove($"backlogs");
        return NoContent();
    }

    private bool BacklogExists(int id)
    {
        return (_context.Backlogs?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
