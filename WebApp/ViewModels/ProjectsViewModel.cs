using WebApp.Models;

namespace WebApp.ViewModels
{
    public class ProjectsViewModel
    {
        public IEnumerable<Project> Projects { get; set; } = [];
        public EditProjectFormModel EditProject { get; set; } = new();
        public AddProjectFormModel AddProject { get; set; } = new();
    }
}
