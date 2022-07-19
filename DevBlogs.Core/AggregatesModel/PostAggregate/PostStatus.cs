 namespace DevBlogs.Core.AggregatesModel.PostAggregate;

public class PostStatus : Enumeration
{
    private static PostStatus Submitted = new (1, nameof(Submitted).ToLowerInvariant(), "ثبت شده");
    private static PostStatus Closed = new (2, nameof(Closed).ToLowerInvariant(), "بسته شده");
    private static PostStatus Published = new (3, nameof(Published).ToLowerInvariant(), "انتشار یافته");
    private static PostStatus Deleted = new (3, nameof(Deleted).ToLowerInvariant(), "حذف شده");

    public PostStatus(int id, string name, string persianName) 
        : base(id, name, persianName)
    {
    }
     
    public static IEnumerable<PostStatus> List() =>
           new[] { Submitted, Closed, Published, Deleted };
}
