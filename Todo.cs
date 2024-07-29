using System.ComponentModel.DataAnnotations;

class Todo
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;

    public string? Content { get; set; } = default!;

    public DateOnly? DueOn { get; set; }

    public DateOnly? CompletedOn { get; set; }
}