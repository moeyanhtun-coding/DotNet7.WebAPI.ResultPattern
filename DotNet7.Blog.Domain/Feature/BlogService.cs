using DotNet7.Blog.Database.RequestModel;

namespace DotNet7.Blog.Domain.Feature;

public class BlogService
{
    private readonly AppDbContext _dbContext = new AppDbContext();

    public async Task<Result<BlogListResponseModel>> GetBlogs()
    {
        Result<BlogListResponseModel> model;
        try
        {
            var lst = await _dbContext.TblBlogs.ToListAsync();

            var data = new BlogListResponseModel
            {
                Blogs = lst
            };

            if (lst.Count == 0)
            {
                model = Result<BlogListResponseModel>.Success(data, "No blogs found");
                goto Result;
            }

            model = Result<BlogListResponseModel>.Success(data, "Blogs found");

            Result:
            return model;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

  
}