namespace HierarchicalSortedList.Core.Models;

public class Category
{
    public int Id { get; set; }
    public int ParentId { get; set; }
    public int Level { get; set; }
    public int Priority { get; set; }
    public string Name  { get; set; } = String.Empty;
}