using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinStarEntity.Models;
using AutoMapper;

namespace FinStarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly FinStarContext _context;
        private readonly IMapper _mapper;

        public ItemsController(IMapper mapper, FinStarContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO.Items>>> GetItems([FromQuery] Filter.ItemFilter filter)
        {
            if (_context.Items == null)
            {
                return NotFound();
            }
            if (filter.Page <= 0)
            { filter.Page = 1; }

            return Ok(_mapper.Map<IEnumerable<DTO.Items>>
                (
                await _context.Items
                .AsNoTracking()
                .Skip((filter.Page - 1) * filter.Limit)
                .Take(filter.Limit)
                .ToListAsync())
                );
        }


        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Array")]
        public async Task<ActionResult<IEnumerable<Items>>> PostItems(IEnumerable<DTO.InputItems> _items)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'FinStarContext.Items'  is null.");
            }

            _context.Database.ExecuteSqlInterpolated($"TRUNCATE TABLE [Items]");

            var items = _mapper.Map<List<Items>>(_items);

            items.OrderBy(x => x.ID);

            int i = 1;
            items.ForEach(x =>
            {
                x.Number = i; i++;
            });

            _context.Items.AddRange(items);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItems", items);
        }

        [HttpPost]
        [Route("fill")]
        public void Fill(int count)
        {
            _context.Database.ExecuteSqlInterpolated($"TRUNCATE TABLE [Items]");


            for (int i = 0; i < count; i++)
            {
                _context.Items.Add(new Items(0, "value" + i, i));
            }
            _context.SaveChanges();
        }

        [HttpGet]
        [Route("Pages")]
        public int Pages([FromQuery] Filter.ItemFilter filter)
        {
            var pages = _context.Items.AsNoTracking().ToArray().Count() / filter.Limit;
            return pages > 0 ? pages : 1;
        }
    }
}
