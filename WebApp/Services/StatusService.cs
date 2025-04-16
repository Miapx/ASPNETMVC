using WebApp.Data.Repositories;
using WebApp.Models;

namespace WebApp.Services;

//Endast en metod för att hämta en status för att kunna filtrera projektlistan efter skapat osv.

public class StatusService(StatusRepository statusRepository)
{
    private readonly StatusRepository _statusRepository = statusRepository;

    public async Task<Status> GetStatusByNameAsync(string statusName)
    {
        var result = await _statusRepository.GetAsync(x => x.StatusName == statusName);
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
        var statusModel = new Status
        {
            Id = result.Id,
            StatusName = result.StatusName
        };
        return statusModel;
    }
}
