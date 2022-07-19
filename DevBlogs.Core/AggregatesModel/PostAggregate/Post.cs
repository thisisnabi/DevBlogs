namespace DevBlogs.Core.AggregatesModel.PostAggregate;

public class Post : EntityBase, IAggregateRoot
{
    public string IdentityGuid { get; private set; }
    public string Title { get; private set; }

    private DateTime _postDate;

    public string Content  { get; set; }

    private bool _isDraft;


    private readonly List<Discussion> _discussionItems;
    public IReadOnlyCollection<Discussion> Discussions => _discussionItems;

    public static Post NewDraft()
    {
        var order = new Post();
        order._isDraft = true;
        return order;
    }
  
    protected Post()
    {
        _discussionItems = new List<Discussion>();
        _isDraft = false;
        _postDate = DateTime.UtcNow;
    }

    public Post(string title, string content, string identityGuid) : this()
    {
        Title = !string.IsNullOrWhiteSpace(title) ? title : throw new ArgumentNullException(nameof(title));
        Content = !string.IsNullOrWhiteSpace(content) ? content : throw new ArgumentNullException(nameof(content));
        IdentityGuid = !string.IsNullOrWhiteSpace(identityGuid) ? identityGuid : throw new ArgumentNullException(nameof(identityGuid));

    }

}
