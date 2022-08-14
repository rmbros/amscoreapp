using AmsApp.Models;

namespace AmsApp.Dto
{
    public class LookUpDto
    {
        public List<ListItem> Disposition { get; set; } = new List<ListItem>();
        public List<ListItem> Gender { get; set; } = new List<ListItem>();
        public List<ListItem> ClinicBranch { get; set; } = new List<ListItem>();
        public List<ListItem> MainDisease { get; set; } = new List<ListItem>();
        public List<ListItemWithParent> SubDisease { get; set; } = new List<ListItemWithParent>();
        public List<ListItem> City { get; set; } = new List<ListItem>();
        public List<ListItem> State { get; set; } = new List<ListItem>();
        public List<ListItem> Country { get; set; } = new List<ListItem>();
    }

    public class ListItem
    {
        public ListItem(int id, string title)
        {
            this.Id = id;
            this.Title = title;
        }
        public int Id { get; set; }
        public string Title { get; set; } = null!;
    }
    public class ListItemWithParent
    {
        public ListItemWithParent(int id, string title, int parentId)
        {
            this.Id = id;
            this.Title = title;
            this.ParentId = parentId;
        }
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int ParentId { get; set; }
    }
}