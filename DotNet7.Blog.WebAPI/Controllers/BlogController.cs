using DotNet7.Blog.Database.Models;
using DotNet7.Blog.Database.RequestModel;
using DotNet7.Blog.Database.ResponseModel;
using DotNet7.Blog.Domain.Feature;
using Microsoft.EntityFrameworkCore;

namespace DotNet7.Blog.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : BaseController
{
    private readonly BlogService _blogService = new BlogService();

    [HttpGet]
    public async Task<IActionResult> GetBlogs()
    {
        try
        {
            var model = await _blogService.GetBlogs();
            return Execute(model);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostBlog(BlogRequestModel blog)
    {
        try
        {
            var model = await _blogService.CreateBlog(blog);
            return Execute(model);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}