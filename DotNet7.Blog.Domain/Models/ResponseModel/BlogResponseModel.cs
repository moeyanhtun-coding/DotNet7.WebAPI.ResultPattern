using DotNet7.Blog.Database.Models;

namespace DotNet7.Blog.Database.ResponseModel;

public class BlogResponseModel
{
    public TblBlog? Blog { get; set; }
}

public class BlogListResponseModel
{
    public List<TblBlog> Blogs { get; set; }
}