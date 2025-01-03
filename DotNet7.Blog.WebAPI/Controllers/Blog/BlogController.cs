using DotNet7.Blog.Domain.Features;

namespace DotNet7.Blog.WebAPI.Controllers.Blog;

[Route("api/[controller]")]
[ApiController]
public class BlogController : BaseController
{
    private readonly BlogService _blogService;

    public BlogController()
    {
        _blogService = new BlogService();
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogs()
    {
        var model = await _blogService.GetBlogs();
        return Execute(model);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetBlogByCode(string code)
    {
        var model = await _blogService.GetBlogByCode(code);
        return Execute(model);
    }

    [HttpPost]
    public async Task<IActionResult> PostBlog(BlogRequestModel blog)
    {
        var model = await _blogService.CreateBlog(blog);
        return Execute(model);
    }

    [HttpPatch("{code}")]
    public async Task<IActionResult> UpdateBlog(string code, BlogRequestModel blog)
    {
        var model = await _blogService.UpdateBlog(code, blog);
        return Execute(model);
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> DeleteBlog(string code)
    {
        var model = await _blogService.DeleteBlog(code);
        return Execute(model);
    }
}