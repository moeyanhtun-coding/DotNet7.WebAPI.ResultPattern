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

    public async Task<Result<BlogResponseModel>> CreateBlog(BlogRequestModel reqModel)
    {
        Result<BlogResponseModel> model ;
        try
        {
            if (string.IsNullOrEmpty(reqModel.BlogName))
            {
                model = Result<BlogResponseModel>.ValidationError("Blog name is required");
                goto Result;
            }

            if (string.IsNullOrEmpty(reqModel.BlogAuthor))
            {
                model = Result<BlogResponseModel>.ValidationError("Blog author is required");
                goto Result;
            }

            if (string.IsNullOrEmpty(reqModel.BlogContent))
            {
                model = Result<BlogResponseModel>.ValidationError("Blog content is required");
                goto Result;
            }

            TblBlog blog = new TblBlog()
            {
                BlogCode = Ulid.NewUlid().ToString(),
                BlogTitle = reqModel.BlogName,
                BlogAuthor = reqModel.BlogAuthor,
                BlogContent = reqModel.BlogContent,
            };
            
           await _dbContext.TblBlogs.AddAsync(blog);
           var result =  await _dbContext.SaveChangesAsync();
           if (result == 0)
           {
               model = Result<BlogResponseModel>.SystemError("An error has occured");
               goto Result; 
           }

           BlogResponseModel respModel = new BlogResponseModel()
           {
               Blog = blog
           };

           model = Result<BlogResponseModel>.Success(respModel, "Blog Created");
            Result:
            return model;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}