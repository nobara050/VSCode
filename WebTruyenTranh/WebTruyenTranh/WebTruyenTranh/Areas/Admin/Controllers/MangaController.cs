using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebTruyenTranh.Areas.Admin.Models;
using WebTruyenTranh.Data;

namespace WebTruyenTranh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MangaController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public MangaController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // Hiển thị danh sách
        public IActionResult Index()
        {
            var mangas = _context.Mangas.ToList();
            return View(mangas);
        }

        // Thêm
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MangaModel manga)
        {
            if (ModelState.IsValid)
            {
                // Xử lý ảnh bìa
                if (manga.CoverImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa tồn tại
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(manga.CoverImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await manga.CoverImageFile.CopyToAsync(fileStream);
                    }
                    manga.CoverImage = "/uploads/" + uniqueFileName;
                }

                // Xử lý ảnh nền
                if (manga.BackgroundImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Tạo thư mục nếu chưa tồn tại
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(manga.BackgroundImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await manga.BackgroundImageFile.CopyToAsync(fileStream);
                    }
                    manga.BackgroundImage = "/uploads/" + uniqueFileName;
                }

                _context.Mangas.Add(manga);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(manga);
        }
    

        // Chỉnh sửa
        public IActionResult Edit(int id)
        {
            var mangas = _context.Mangas.Find(id);
            if (mangas == null) return NotFound();
            return View(mangas);
        }

        [HttpPost]
        public IActionResult Edit(MangaModel manga)
        {
            if (ModelState.IsValid)
            {
                _context.Mangas.Update(manga);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(manga);
        }

        // Xóa
        public IActionResult Delete(int id)
        {
            var manga = _context.Mangas.Find(id);
            if (manga == null) return NotFound();
            return View(manga);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var manga = _context.Mangas.Find(id);
            if (manga == null) return NotFound();

            _context.Mangas.Remove(manga);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
