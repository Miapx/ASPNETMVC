using WebApp.Data.Repositories;
using WebApp.Models;

namespace WebApp.Services;

public class StatusService(StatusRepository statusRepository)
{
    private readonly StatusRepository _statusRepository = statusRepository;

    public async Task<Status> GetStatusByNameAsync(string statusName)
    {
        var result = await _statusRepository.GetAsync(x => x.StatusName == statusName);

        if (result == null)
            return null!;

        var statusModel = new Status
        {
            Id = result.Id,
            StatusName = result.StatusName
        };
        return statusModel;
    }

    public async Task<Status> GetStatusByIdAsync(int statusId)
    {
        var result = await _statusRepository.GetAsync(x => x.Id == statusId);

        if (result == null)
            return null!;

        var statusModel = new Status
        {
            Id = result.Id,
            StatusName = result.StatusName
        };
        return statusModel;
    }
}
