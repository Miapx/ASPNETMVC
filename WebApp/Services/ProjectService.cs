using WebApp.Models;

namespace WebApp.Services;

public class ProjectService
{
    private List<Project> _projects =
        [
        new Project { Id = 1, ProjectName = "Project 1" },
        new Project { Id = 2, ProjectName = "Project 2" },
        new Project { Id = 3, ProjectName = "Project 3" }
        ];

    public IEnumerable<Project> GetProjects()
    {
        return _projects;
    }
}
