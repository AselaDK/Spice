using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models.ViewModels;
using Spice.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }
        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET INDEX
        public async Task<IActionResult> Index()
        {
            var subCategory = await _db.SubCategory.Include(s => s.Category).ToListAsync();
            return View(subCategory);
        }

        // GET - CREATE
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = new SubCategory(),
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
            };

            return View(model);
        }

        // POST - CREATE 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExist = _db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.CategoryId == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count() > 0)
                {
                    // Error
                    StatusMessage = "Error : Sub Category exists under " + 
                        doesSubCategoryExist.First().Category.Name + 
                        " category. Please use another name. ";
                }
                else
                {
                    _db.SubCategory.Add(model.SubCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }//  if modelState is not valid we have to create model again.
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToArrayAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }

        // Show subcategories dynamically of each category when Category is selected
        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = await (from SubCategory in _db.SubCategory
                                          where SubCategory.CategoryId == id
                                          select SubCategory).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        // GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            // check id is null
            if (id == null)
            {
                return NotFound(); 
            }
            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(m => m.Id == id);

            // check database record exists
            if(subCategory == null)
            {
                return NotFound();
            }

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
            };

            return View(model);
        }

        // POST - EDIT 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExist = _db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.CategoryId == model.SubCategory.CategoryId);

                if (doesSubCategoryExist.Count() > 0)
                {
                    // Error
                    StatusMessage = "Error : Sub Category exists under " +
                        doesSubCategoryExist.First().Category.Name +
                        " category. Please use another name. ";
                }
                else
                {
                    _db.SubCategory.Add(model.SubCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }//  if modelState is not valid we have to create model again.
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToArrayAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }
    }
}