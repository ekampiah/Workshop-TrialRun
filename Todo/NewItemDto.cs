namespace Todo.Model;

public class NewItemDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }

    public NewItemDto(string title, string description)
    {
        Title = title;
        Description = description;
    }
}