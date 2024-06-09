using ThurstonMVC.Models.Entities;

namespace ThurstonMVC.Models.ViewModel
{
    public class IndexEmployeeViewModel
    {
        public string? SearchQuery { get; set; }
        public IEnumerable<Employee> Employee { get; set; }
    }
}
