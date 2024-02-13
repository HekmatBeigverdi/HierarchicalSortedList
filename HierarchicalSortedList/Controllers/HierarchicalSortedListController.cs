using HierarchicalSortedList.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HierarchicalSortedList.Controllers;


[ApiController]
[Route("[controller]")]

public class HierarchicalSortedListController : ControllerBase
{
    // A list of products
    private static List<Category> _categories = new List<Category>()
    {
        new Category { Id = 1, ParentId = 0, Name = "Laptop"},
        new Category { Id = 2, ParentId = 0, Name = "Mouse"},
        new Category { Id = 3, ParentId = 0, Name = "Keyboard"},
        new Category { Id = 4, ParentId = 0, Name = "Monitor"},
        new Category { Id = 5, ParentId = 0, Name = "Printer"},
        new Category { Id = 6, ParentId = 1, Name = "Samsung"},
        new Category { Id = 7, ParentId = 1, Name = "Sony"},
        new Category { Id = 8, ParentId = 1, Name = "Lenovo"}, 
        new Category { Id = 9, ParentId = 4, Name = "LCD"},
        new Category { Id = 10, ParentId = 4, Name = "LED"},
        new Category { Id = 11, ParentId = 10, Name = "Sony"},
        new Category { Id = 12, ParentId = 10, Name = "LG"},
        new Category { Id = 13, ParentId = 10, Name = "Samsung"},
        new Category { Id = 14, ParentId = 9, Name = "Sony"},
        new Category { Id = 15, ParentId = 9, Name = "LG"},
        new Category { Id = 16, ParentId = 9, Name = "Samsung"},
    };

    private readonly ILogger<HierarchicalSortedListController> _logger;

    public HierarchicalSortedListController(ILogger<HierarchicalSortedListController> logger)
    {
        _logger = logger;
    }

    [HttpGet("sample")]
    public IEnumerable<Category> Get()
    {
        // Create the list from the sample data
        var list = CreateList(_categories);
        // Sort the list by hierarchical order
        list.Sort(new NodeComparer(list));
        var priority = 1;

        foreach (var item in list)
        {
            item!.Priority = priority; 
            priority++;
        }
        return list;
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    public List<Category> CreateList(List<Category> items)
    {
        
        var list = new List<Category>();

        void CreateListRecursively(int parentId, int level)
        {
            var nodes = items.Where(n => n.ParentId == parentId);

            foreach (var node in nodes)
            {
                node.Level = level;
                list.Add(node);
                CreateListRecursively(node.Id, level + 1);
            }
        }
        CreateListRecursively(0, 0);
        return list;
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    private class NodeComparer : IComparer<Category>
    {
        private readonly List<Category> _nodes;
        public NodeComparer(List<Category> nodes)
        {
            _nodes = nodes;
        }
        public int Compare(Category? x, Category? y)
        {
            var xPath = GetPath(x!);
            var yPath = GetPath(y!);

            return xPath.CompareTo(yPath);
        }

        private string GetPath(Category node)
        {
            var ids = new List<int>();
            var current = node;
            while (current != null)
            {
                ids.Add(current.Id);
                current = _nodes.FirstOrDefault(n => n.Id == current.ParentId);
            }
            ids.Reverse();
            return string.Join(".", ids);
        }
    }  
}