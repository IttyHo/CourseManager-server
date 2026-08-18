using CourseManager.Api.Contracts.Courses;
using CourseManager.Api.Data;
using CourseManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context) => _context = context;

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<CourseResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CourseResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var courses = await _context.Courses
            .AsNoTracking()
            .OrderBy(course => course.Name)
            .Select(course => ToResponse(course))
            .ToListAsync(cancellationToken);

        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(course => course.Id == id, cancellationToken);

        return course is null ? NotFound() : Ok(ToResponse(course));
    }

    [HttpPost]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CourseResponse>> Create(
        CreateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            DurationHours = request.DurationHours
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);

        var response = ToResponse(course);
        return CreatedAtAction(nameof(GetById), new { id = course.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        UpdateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FindAsync([id], cancellationToken);
        if (course is null)
        {
            return NotFound();
        }

        course.Name = request.Name.Trim();
        course.Description = request.Description.Trim();
        course.DurationHours = request.DurationHours;

        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FindAsync([id], cancellationToken);
        if (course is null)
        {
            return NotFound();
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static CourseResponse ToResponse(Course course) => new(
        course.Id,
        course.Name,
        course.Description,
        course.DurationHours,
        course.CreatedAt);
}
