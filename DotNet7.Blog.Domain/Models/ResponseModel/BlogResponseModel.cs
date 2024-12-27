namespace DotNet7.Blog.Domain.Models.ResponseModel;

public class BlogResponseModel
{
    public TblBlog? Blog { get; set; }
}

public class BlogListResponseModel
{
    public List<TblBlog> Blogs { get; set; }
}