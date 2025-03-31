using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServerDTO;
using ServerServices.IServices;
using ServerServices.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Server.controllers;

[ApiController]
[Route("[controller]")]
public class BannerController : ControllerBase
{
    
    private readonly IBannerService _bannerService;
    
    public BannerController(IBannerService bannerService)
    {
        _bannerService = bannerService;
   
    }
    
    /*
    [HttpGet("GenerateAIBanner")]
    public async Task<ServiceResult<string>> GenerateAIBanner(string BannerTitle)
    {
        try
        {   
            //get the banner service 
            var bannerResult = await _bannerService.GetBanner(BannerTitle);
            //get the bannerlogo service 
            #pragma warning disable SKEXP0001
            var bannerlogo = await _bannerLogoService.GetBannerLogo(BannerTitle);
            var responseObject = JsonSerializer.Deserialize<Dictionary<string, string>>(bannerResult.Data);
            if (responseObject != null)
            {
                responseObject["logoUrl"] = bannerlogo.Data;
                string updatedResponse = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                return new ServiceResult<string>()
                {
                    Data = updatedResponse,
                    Success = true,
                    StatusCode = 200
                };
            }

            return new ServiceResult<string>()
            {
               Data = "Service result is null",
               Success = false,
               StatusCode = 404
            };
        }
        catch (Exception ex)
        {
            return new ServiceResult<string>()
            {
                Data = ex.Message.ToString(),
                Success = false,
                StatusCode = 500
            };
        }
    }*/

    [HttpGet("generatebanner")]
    public async Task<IActionResult> GetBanner(string BannerTittle)
    {
        var response = await _bannerService.GenerateBanner(BannerTittle);
        return Ok(response);
    }
}