namespace Todo.Model;

public class Item {
    public string Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public Item(string title, string? description)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
    }
}