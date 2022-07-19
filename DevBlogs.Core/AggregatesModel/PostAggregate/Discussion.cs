namespace DevBlogs.Core.AggregatesModel.PostAggregate;

public class Discussion : EntityBase
{
    public int PostId { get; private set; }

    protected Discussion() { }

}
