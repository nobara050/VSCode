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

                // Đặt UserId luôn bằng 0 (admin khi thêm bằng tài khoản admin)
                manga.UserId = 0;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDo(MangaModel manga)
        {
                // Tìm Manga trong cơ sở dữ liệu
                var existingManga = await _context.Mangas.FindAsync(manga.MangaId);
                if (existingManga == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin cơ bản
                existingManga.Title = manga.Title;
                existingManga.Description = manga.Description;
                existingManga.Status = manga.Status;

                // Kiểm tra ảnh bìa mới
                if (manga.CoverImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Đảm bảo thư mục tồn tại

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existingManga.CoverImage))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingManga.CoverImage.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Lưu ảnh bìa mới
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(manga.CoverImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await manga.CoverImageFile.CopyToAsync(fileStream);
                    }
                    existingManga.CoverImage = "/uploads/" + uniqueFileName; // Cập nhật đường dẫn ảnh bìa
                }


                // Lưu các thay đổi vào cơ sở dữ liệu
                _context.Mangas.Update(existingManga);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // Chuyển hướng về trang Index sau khi lưu
        }


        // Xóa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var manga = await _context.Mangas.FindAsync(id);
            if (manga == null)
            {
                return NotFound();
            }

            // Xóa ảnh bìa nếu có
            if (!string.IsNullOrEmpty(manga.CoverImage))
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, manga.CoverImage.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Xóa manga khỏi cơ sở dữ liệu
            _context.Mangas.Remove(manga);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
