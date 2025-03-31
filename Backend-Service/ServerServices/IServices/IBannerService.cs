using ServerDTO;

namespace ServerServices.IServices;

public interface IBannerService
{
    /*Task<ServiceResult<string>> GetBanner(string title);*/
    Task<ServiceResult<string>> GenerateBanner(string title);
}