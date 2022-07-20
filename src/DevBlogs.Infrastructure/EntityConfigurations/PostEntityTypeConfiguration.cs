namespace DevBlogs.Infrastructure.EntityConfigurations;

class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> postConfiguration)
    {
        postConfiguration.ToTable("posts", DevBlogsContext.DEFAULT_SCHEMA);

        postConfiguration.HasKey(b => b.Id);

        postConfiguration.Ignore(b => b.DomainEvents);

        postConfiguration.Property(b => b.Id)
            .UseHiLo("postseq", DevBlogsContext.DEFAULT_SCHEMA);

        postConfiguration.HasIndex("IdentityGuid")
            .IsUnique(true);

        postConfiguration.Property(b => b.Title)
            .IsRequired();

        //postConfiguration.HasMany(b => b.PaymentMethods)
        //    .WithOne()
        //    .HasForeignKey("BuyerId")
        //    .OnDelete(DeleteBehavior.Cascade);

        //var navigation = buyerConfiguration.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));

        //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
