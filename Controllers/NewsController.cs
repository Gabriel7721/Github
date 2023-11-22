using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEx1.Models;

namespace MyEx1.Controllers
{
    public class NewsController : Controller
    {
        NewsDbContext ctx;
        public NewsController(NewsDbContext ctx)
        {
            this.ctx = ctx;
        }

        // login admin
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin adm)
        {
            var user = await ctx.Admins!.SingleOrDefaultAsync(u => u.Username==adm.Username && u.Password==adm.Password);
            if (user!=null)
            {
                return RedirectToAction("Admin");
            }
            return View("Index"); // view index là view login
        }
        public IActionResult Admin() // view admin là view quản lý tin tức
        {
            return View(); // view admin là view quản lý tin tức    
        }
        //public async Task<IActionResult> ShowAll()
        //{
        //    var content = await ctx.News!.ToListAsync();
        //    return View(content);
        //}
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(News newsItem)
        {
            if (ModelState.IsValid)
            {
                ctx.News.Add(newsItem); // 'ctx' là context của database
                await ctx.SaveChangesAsync();

                // Sử dụng TempData để lưu thông báo tạm thời qua request
                TempData["SuccessMessage"] = "News created successfully!";

                return RedirectToAction("ShowAll"); // chuyển hướng đến action 'ShowAll'
            }
            return View(newsItem); // nếu có lỗi, hiển thị lại form với dữ liệu đã nhập
        }
        [HttpPost] // Sử dụng phương thức POST để thực hiện hành động xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Không tìm thấy tin tức nào với ID này
            }

            var newsItem = await ctx.News.FirstOrDefaultAsync(m => m.NewsId == id);
            if (newsItem == null)
            {
                return NotFound(); // Không tìm thấy tin tức nào với ID này
            }

            ctx.News.Remove(newsItem); // Xóa tin tức
            await ctx.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

            return RedirectToAction(nameof(ShowAll)); // Quay trở lại trang danh sách tin tức
        }
        // Phương thức GET để hiển thị form chỉnh sửa
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Không tìm thấy tin tức nào với ID này
            }

            var newsItem = await ctx.News.FindAsync(id);
            if (newsItem == null)
            {
                return NotFound(); // Không tìm thấy tin tức nào với ID này
            }

            return View(newsItem); // Trả về view với mục tin tức cần chỉnh sửa
        }

        // Phương thức POST để xử lý dữ liệu đã chỉnh sửa từ form
        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ chống lại tấn công CSRF
        public async Task<IActionResult> Edit(int id, News newsItem)
        {
            if (id != newsItem.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid) // Kiểm tra xem dữ liệu có hợp lệ không
            {
                try
                {
                    ctx.Update(newsItem); // Cập nhật tin tức trong cơ sở dữ liệu
                    await ctx.SaveChangesAsync(); // Lưu thay đổi
                }
                catch (DbUpdateConcurrencyException) // Xử lý các ngoại lệ có thể xảy ra
                {
                    if (!NewsExists(newsItem.NewsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ShowAll)); // Chuyển hướng đến trang danh sách tin tức
            }
            return View(newsItem); // Nếu có lỗi, hiển thị lại form với thông tin đã nhập
        }

        // Phương thức GET để hiển thị chi tiết tin tức
        private bool NewsExists(int id) // Kiểm tra xem tin tức có tồn tại không
        {
            return ctx.News.Any(e => e.NewsId == id);
        }

        // Số lượng tin tức trên mỗi trang
        private const int PageSize = 1;

        public async Task<IActionResult> ShowAll(int page = 1)
        {
            var totalItems = await ctx.News.CountAsync();
            var itemsToShow = await ctx.News
                                        .OrderBy(n => n.NewsId) // sắp xếp có thể thay đổi tùy ý
                                        .Skip((page - 1) * PageSize)
                                        .Take(PageSize)
                                        .ToListAsync();

            // Tạo một model phân trang
            var paginatedViewModel = new PaginatedList<News>
            {
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize),
                Items = itemsToShow
            };

            return View(paginatedViewModel);
        }

    }
}
