using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.ViewModel;

namespace Project.Controllers
{
    public class StyleController : Controller
    {
        private readonly DbuniPayContext _context;

        public StyleController(DbuniPayContext context)
        {
            _context = context;
        }

        //前台StyleList
        public async Task<IActionResult> StyleList(string category)
        {
            var query = from tsi in _context.TstyleImgs
                        join ts in _context.Tstyles on tsi.Sid equals ts.Sid
                        join tp in _context.Tproducts on ts.Pid equals tp.Pid
                        select new { tsi.Simg, tsi.SimgCategory, tp.Pid, tp.Pname, tp.Pphoto, tp.Pprice };

            // 如果有篩選條件，則進行篩選
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.SimgCategory == category);
            }

            var styles = query
                         .ToList()
                         .GroupBy(x => x.Simg)
                         .Select(g => new StyleViewModel
                         {
                             SImg = g.Key,
                             Products = g.Select(p => new ProductViewModel
                             {
                                 PID = p.Pid,
                                 PName = p.Pname,
                                 PPhoto = p.Pphoto,
                                 PPrice = p.Pprice
                             }).ToList()
                         }).ToList();

            return View(styles);
        }


        //後台StyleList
        // GET: Style
        public async Task<IActionResult> List()
        {

            var styles = (from tsi in _context.TstyleImgs
                          join ts in _context.Tstyles on tsi.Sid equals ts.Sid
                          join tp in _context.Tproducts on ts.Pid equals tp.Pid
                          select new { tsi.Simg, tp.Pid, tp.Pname, tp.Pphoto, tp.Pprice })
                      .ToList()
                      .GroupBy(x => x.Simg)
                      .Select(g => new StyleViewModel
                      {
                          SImg = g.Key,
                          Products = g.Select(p => new ProductViewModel
                          {
                              PID = p.Pid,
                              PName = p.Pname,
                              PPhoto = p.Pphoto,
                              PPrice = p.Pprice
                          }).ToList()
                      }).ToList();

            return View(styles);
        }


        // GET: Style/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Style/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sid,Simg,SimgCategory")] TstyleImg tstyleImg)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tstyleImg);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tstyleImg);
        }

        // GET: Style/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tstyleImg = await _context.TstyleImgs.FindAsync(id);
            if (tstyleImg == null)
            {
                return NotFound();
            }
            return View(tstyleImg);
        }

        // POST: Style/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Sid,Simg,SimgCategory")] TstyleImg tstyleImg)
        {
            if (id != tstyleImg.Sid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tstyleImg);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TstyleImgExists(tstyleImg.Sid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tstyleImg);
        }

        // GET: Style/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tstyleImg = await _context.TstyleImgs
                .FirstOrDefaultAsync(m => m.Sid == id);
            if (tstyleImg == null)
            {
                return NotFound();
            }

            return View(tstyleImg);
        }

        // POST: Style/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tstyleImg = await _context.TstyleImgs.FindAsync(id);
            if (tstyleImg != null)
            {
                _context.TstyleImgs.Remove(tstyleImg);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TstyleImgExists(int id)
        {
            return _context.TstyleImgs.Any(e => e.Sid == id);
        }
    }
}
