namespace DotNet7.Blog.Domain.Feature;

public class BlogService
{
    private readonly AppDbContext _dbContext = new AppDbContext();

    public async Task<Result<BlogListResponseModel>> GetBlogs()
    {
        Result<BlogListResponseModel> model = new Result<BlogListResponseModel>();
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

    public async Task<Result<BlogResponseModel>> GetBlogByCode(string code)
    {
        Result<BlogResponseModel> model = new Result<BlogResponseModel>();
        try
        {
            var blog = await FindBlogByCode(code);

            model = Result<BlogResponseModel>.Success(blog, "Blog is found");
            Result:
            return model;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Result<BlogResponseModel>> CreateBlog(BlogRequestModel reqModel)
    {
        Result<BlogResponseModel> model = new Result<BlogResponseModel>();
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

            var blogCode = Ulid.NewUlid().ToString();
            TblBlog blog = new TblBlog()
            {
                BlogCode = "B-" + blogCode.Substring(0, 15),
                BlogTitle = reqModel.BlogName,
                BlogAuthor = reqModel.BlogAuthor,
                BlogContent = reqModel.BlogContent,
            };

            await _dbContext.TblBlogs.AddAsync(blog);
            var result = await _dbContext.SaveChangesAsync();

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

    public async Task<Result<BlogResponseModel>> UpdateBlog(string code, BlogRequestModel reqModel)
    {
        Result<BlogResponseModel> model = new Result<BlogResponseModel>();
        try
        {
            var blog = await FindBlogByCode(code);
            var item = blog.Blog!;

            if (!string.IsNullOrEmpty(reqModel.BlogName))
                item.BlogTitle = reqModel.BlogName;
            if (!string.IsNullOrEmpty(reqModel.BlogAuthor))
                item.BlogAuthor = reqModel.BlogAuthor;
            if (!string.IsNullOrEmpty(reqModel.BlogContent))
                item.BlogContent = reqModel.BlogContent;

            _dbContext.Entry(item).State = EntityState.Modified;
            var result = await _dbContext.SaveChangesAsync();
            if (result is 0)
            {
                model = Result<BlogResponseModel>.SystemError("An error has occured");
                goto Result;
            }

            model = Result<BlogResponseModel>.Success("Blog Updated Successfully");
            Result:
            return model;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Result<BlogResponseModel>> DeleteBlog(string code)
    {
        Result<BlogResponseModel> model = new Result<BlogResponseModel>();
        try
        {
            var blog = await FindBlogByCode(code);
            var item = blog.Blog!;
            item.DeleteFlag = true;

            _dbContext.Entry(item).State = EntityState.Modified;
            var result = await _dbContext.SaveChangesAsync();

            if (result is 0)
            {
                model = Result<BlogResponseModel>.SystemError("An error has occured");
                goto Result;
            }

            model = Result<BlogResponseModel>.Success("Blog Deleted Successfully");

            Result:
            return model;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BlogResponseModel> FindBlogByCode(string code)
    {
        BlogResponseModel model = new BlogResponseModel();
        try
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException("Blog code is required");

            var blog = await _dbContext.TblBlogs.AsNoTracking().FirstOrDefaultAsync(x => x.BlogCode == code);

            if (blog is null)
                throw new KeyNotFoundException("Blog code does not exist");
            model.Blog = blog;
            return model;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}