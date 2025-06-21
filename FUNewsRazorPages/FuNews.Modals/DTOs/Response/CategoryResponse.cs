namespace FuNews.Modals.DTOs.Response;

public class CategoryResponse
{
    public short CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
    public short? ParentCategoryId { get; set; }
    public bool? IsActive { get; set; }
    
    // Tree structure properties
    public string? ParentCategoryName { get; set; }
    public int Level { get; set; } = 0;
    public bool HasChildren { get; set; } = false;
    public List<CategoryResponse>? Children { get; set; }
    
    // Display properties
    public string IndentedName => new string('—', Level * 2) + (Level > 0 ? " " : "") + CategoryName;
    public string StatusDisplay => IsActive == true ? "Active" : "Inactive";
}