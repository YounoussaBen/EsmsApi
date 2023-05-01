﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceManagerApi.Data;
using ServiceManagerApi.Dtos.ProHaulerUnits;
using ServiceManagerApi.Dtos.ProLoaderUnits;

namespace ServiceManagerApi.Controllers.Production
{
  
    public class ProLoaderUnitController : BaeApiController<ProLoaderUnitController>
    {
        private readonly EnpDBContext _context;
        public ProLoaderUnitController(EnpDBContext context)
        {
            _context = context;
        }
        //get list
        [HttpGet]
        [ProducesResponseType(typeof(ProloaderUnit), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IEnumerable<ProhaulerUnit>> Get()
        {
            return await _context.ProhaulerUnits.ToListAsync();
        }

        // get by id
        [HttpGet("id")]
        [ProducesResponseType(typeof(ProhaulerUnit), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var ProloaderUnit = await _context.ProloaderUnits.FindAsync(id);
            if (ProloaderUnit == null)
            {
                return NotFound();
            }
            return Ok(ProloaderUnit);
        }

        // post groups
        [HttpPost]
        [ProducesResponseType(typeof(ProloaderUnit), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(ProLoaderUnitDto proLoaderUnitDto)
        {
            ProloaderUnit proloaderUnit = _mapper.Map<ProloaderUnit>(proLoaderUnitDto);

            _context.ProloaderUnits.Add(proloaderUnit);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProloaderUnitExists(proloaderUnit.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction(nameof(GetById), new { id = proloaderUnit.Id }, proloaderUnit);
        }

        // put groups
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, ProloaderUnit proloaderUnit)
        {
            if (id != proloaderUnit.Id)
            {
                return BadRequest();
            }

            _context.Entry(proloaderUnit).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProloaderUnitExists(proloaderUnit.Id))
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

        // delete groups
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var proloaderUnit = await _context.ProloaderUnits.FindAsync(id);
            if (proloaderUnit == null)
            {
                return NotFound();
            }
            _context.ProloaderUnits.Remove(proloaderUnit);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ProloaderUnitExists(int id)
        {
            return _context.ProloaderUnits.Any(e => e.Id == id);
        }
    }
}
}
