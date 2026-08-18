using System.ComponentModel.DataAnnotations;

namespace CourseManager.Api.Contracts.Courses;

public sealed class CreateCourseRequest
{
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; init; } = string.Empty;

    [Range(1, 10_000)]
    public int DurationHours { get; init; }
}
