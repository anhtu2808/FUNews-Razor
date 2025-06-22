using System.ComponentModel.DataAnnotations;

namespace FuNews.Modals.DTOs.Request;

public class CategoryRequest
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string CategoryDescription { get; set; } = string.Empty;

    public short? ParentCategoryId { get; set; }

    public bool IsActive { get; set; } = true;
} 