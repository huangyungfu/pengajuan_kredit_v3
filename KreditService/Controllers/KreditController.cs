using KreditService.Data;
using KreditService.DTOs;
using KreditService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace KreditService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class KreditController : ControllerBase
{
    private readonly KreditDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly ILogger<KreditController> _logger;
    private const string CacheKeyAll = "all_kredit_data";

    public KreditController(KreditDbContext context, IDistributedCache cache, ILogger<KreditController> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Processing GetAll applications data payload request.");

        string? cachedData = await _cache.GetStringAsync(CacheKeyAll);

        if (!string.IsNullOrEmpty(cachedData))
        {
            _logger.LogInformation("Redis cache hit returned.");
            var cachedList = JsonSerializer.Deserialize<List<PengajuanKredit>>(cachedData);
            return Ok(cachedList);
        }

        var list = await _context.PengajuanKredits.ToListAsync();

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        await _cache.SetStringAsync(
            CacheKeyAll,
            JsonSerializer.Serialize(list),
            cacheOptions);

        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var record = await _context.PengajuanKredits.FindAsync(id);

        if (record == null)
            return NotFound(new { message = $"Data with ID {id} not found." });

        return Ok(record);
    }

    [HttpPost("override-create")]
    public async Task<IActionResult> OverrideCreate(OverrideCreateRequest request)
    {
        var validationError = KreditValidator.ValidateCoreMetrics(
            request.Plafon,
            request.Bunga,
            request.Tenor);

        if (validationError != null)
            return BadRequest(new { error = validationError });

        if (request.Angsuran <= 0)
            return BadRequest(new { error = "Angsuran cannot be 0 or negative." });

        var entity = new PengajuanKredit
        {
            Plafon = request.Plafon,
            Bunga = request.Bunga,
            Tenor = request.Tenor,
            Angsuran = request.Angsuran
        };

        _context.PengajuanKredits.Add(entity);
        await _context.SaveChangesAsync();
        await _cache.RemoveAsync(CacheKeyAll);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPost("proper-create")]
    public async Task<IActionResult> ProperCreate(ProperCreateRequest request)
    {
        var validationError2 = KreditValidator.ValidateCoreMetrics(
            request.Plafon,
            request.Bunga,
            request.Tenor);

        if (validationError2 != null)
            return BadRequest(new { error = validationError2 });

        decimal computedAngsuran = RunAngsuranFormula(
            request.Plafon,
            request.Bunga,
            request.Tenor);

        var entity = new PengajuanKredit
        {
            Plafon = request.Plafon,
            Bunga = request.Bunga,
            Tenor = request.Tenor,
            Angsuran = computedAngsuran
        };

        _context.PengajuanKredits.Add(entity);
        await _context.SaveChangesAsync();
        await _cache.RemoveAsync(CacheKeyAll);

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("override-update/{id}")]
    public async Task<IActionResult> OverrideUpdate(Guid id, OverrideCreateRequest request)
    {
        var validationError = KreditValidator.ValidateCoreMetrics(
            request.Plafon,
            request.Bunga,
            request.Tenor);

        if (validationError != null)
            return BadRequest(new { error = validationError });

        if (request.Angsuran <= 0)
            return BadRequest(new { error = "Angsuran cannot be 0 or negative." });

        var record = await _context.PengajuanKredits.FindAsync(id);

        if (record == null)
            return NotFound(new { message = $"Data with ID {id} not found." });

        record.Plafon = request.Plafon;
        record.Bunga = request.Bunga;
        record.Tenor = request.Tenor;
        record.Angsuran = request.Angsuran;
        record.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await _cache.RemoveAsync(CacheKeyAll);

        return Ok(record);
    }

    [HttpPut("proper-update/{id}")]
    public async Task<IActionResult> ProperUpdate(Guid id, ProperCreateRequest request)
    {
        var validationError2 = KreditValidator.ValidateCoreMetrics(
            request.Plafon,
            request.Bunga,
            request.Tenor);

        if (validationError2 != null)
            return BadRequest(new { error = validationError2 });

        var record = await _context.PengajuanKredits.FindAsync(id);

        if (record == null)
            return NotFound(new { message = $"Data with ID {id} not found." });

        decimal computedAngsuran = RunAngsuranFormula(
            request.Plafon,
            request.Bunga,
            request.Tenor);

        record.Plafon = request.Plafon;
        record.Bunga = request.Bunga;
        record.Tenor = request.Tenor;
        record.Angsuran = computedAngsuran;
        record.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await _cache.RemoveAsync(CacheKeyAll);

        return Ok(record);
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateOrUpdate(CalculationRequest request)
    {
        var validationError = KreditValidator.ValidateCoreMetrics(
            request.Plafon,
            request.Bunga,
            request.Tenor);

        if (validationError != null)
            return BadRequest(new { error = validationError });

        decimal computedAngsuran = RunAngsuranFormula(
            request.Plafon,
            request.Bunga,
            request.Tenor);

        if (request.Id.HasValue)
        {
            var record = await _context.PengajuanKredits.FindAsync(request.Id.Value);

            if (record == null)
                return NotFound(new { message = "Target ID context record to update not found." });

            record.Plafon = request.Plafon;
            record.Bunga = request.Bunga;
            record.Tenor = request.Tenor;
            record.Angsuran = computedAngsuran;
            record.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await _cache.RemoveAsync(CacheKeyAll);

            return Ok(record);
        }

        return Ok(new { calculatedAngsuran = computedAngsuran });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var record = await _context.PengajuanKredits.FindAsync(id);

        if (record == null)
            return NotFound(new { message = "Data not found." });

        _context.PengajuanKredits.Remove(record);

        await _context.SaveChangesAsync();
        await _cache.RemoveAsync(CacheKeyAll);

        return Ok(new { message = "Record deleted successfully." });
    }

    private static decimal RunAngsuranFormula(decimal plafon, decimal bungaPertahun, int tenor)
    {
        double p = (double)plafon;
        double r = (double)(bungaPertahun / 12 / 100);
        int t = tenor;

        double top = r * Math.Pow(1 + r, t);
        double bottom = Math.Pow(1 + r, t) - 1;

        return (decimal)(p * (top / bottom));
    }
}