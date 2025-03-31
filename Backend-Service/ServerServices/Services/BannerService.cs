using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextToImage;
using ServerDTO;
using ServerServices.IServices;
using Microsoft.Extensions.Configuration;

namespace ServerServices.Services;

public class BannerService:IBannerService
{
    private readonly Kernel _kernel;
    private readonly IConfiguration _configuration;
    [Experimental("SKEXP0010")]
    public BannerService(IConfiguration configuration)
    {
        _configuration = configuration;
        var configs = _configuration.GetSection("ModelKeys");
        string endpoint = configs["endPoint"];
        string apiKey = configs["apiKey"];
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion("gpt-4o", endpoint, apiKey);
        builder.Services.AddAzureOpenAITextToImage("dall-e-3",endpoint,apiKey);
        /*builder.Plugins.AddFromType<BannerPlugin>();*/
        _kernel = builder.Build();
    }

    /*public async Task<ServiceResult<string>> GetBanner(string title)
    {
        try
        {
            string prompt = @$"
You are a creative AI assistant. Given a banner title, generate a JSON object with:
- Background color in HEX
- Description color in HEX
- short, engaging Button Text
- Button Text color in HEX
- Button Background color in HEX
- Short, engaging banner description

Output **only a valid JSON object** with no additional text or explanations. Do not include any comments. The response must be directly parseable as JSON.

Provide the output in JSON format like:
{{
  ""backgroundColor"": ""#FF5733"",
  ""descriptionColor"": ""#FFFFFF"",
  ""ButtonText"": ""Click here"",
  ""ButtonTextColor"" : ""#000080"",
  ""ButtonBackgroundColor"": ""#FF7F50"",
  ""bannerDescription"": ""Get ready for the biggest sale of the season!""
}}

Title: {title}
JSON Response:
";
            
            var result = await _kernel.InvokePromptAsync<string>(prompt);
            JsonDocument.Parse(result);
            return new ServiceResult<string>()
            {
                Data = result,
                Success = true,
                StatusCode = 200
            };

        }
        catch (Exception e)
        {
            return new ServiceResult<string>()
            {
                Data = e.Message.ToString(),
                Success = false,
                StatusCode = 500
            }
            ;
        }
    }*/

    [Experimental("SKEXP0001")]
    public async Task<ServiceResult<string>> GenerateBanner(string title)
    {
        try
        {
            string prompt = @$"
You are a creative AI assistant. Given a banner title, generate a JSON object with:
- Background color in HEX
- Description color in HEX
- short, engaging Button Text
- Button Text color in HEX
- Button Background color in HEX
- Short, engaging banner description

Output **only a valid JSON object** with no additional text or explanations. Do not include any comments. The response must be directly parseable as JSON.

Provide the output in JSON format like:
{{
  ""backgroundColor"": ""#FF5733"",
  ""descriptionColor"": ""#FFFFFF"",
  ""ButtonText"": ""Click here"",
  ""ButtonTextColor"" : ""#000080"",
  ""ButtonBackgroundColor"": ""#FF7F50"",
  ""bannerDescription"": ""Get ready for the biggest sale of the season!""
}}

Title: {title}
JSON Response:
";         
            var result = await _kernel.InvokePromptAsync<string>(prompt);
            var responseObject = JsonSerializer.Deserialize<Dictionary<string, string>>(result);
            if (responseObject == null)
            {
                return new ServiceResult<string>()
                {
                    Data = null,
                    Success = false,
                    Message = "Invalid JSON response received.",
                    StatusCode = 400
                };
            }
            // Generate logo
            var bannerLogoUrl = await GenerateBannerlogo(title);
            responseObject["logoUrl"] = bannerLogoUrl;
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
        catch (Exception ex)
        {
            return new ServiceResult<string>()
            {
                Data = ex.Message.ToString(),
                Success = false,
                StatusCode = 500,
                ErrorMessage = ex.Message
            };
        }
    }

    [Experimental("SKEXP0001")]
    private async Task<string> GenerateBannerlogo(string title)
    {
        try
        {
            string prompt =
                @$"Create a visually appealing website banner for the banner title {title}. The design should be modern, clean, and engaging. 
                           It should reflect the purpose of Socxly, a platform that enhances social media experiences.
                           
                           Key Requirements:
                           - Provide a high-resolution digital banner
                           - Avoid using any copyrighted or private images
                           
                           The banner should evoke excitement and curiosity, encouraging users to explore Socxly. 
                           No additional text or descriptions are needed in the output, only the banner image.
                           ";
            var dallE = _kernel.GetRequiredService<ITextToImageService>();
            var imageUrl = await dallE.GenerateImageAsync(prompt, 1024, 1024);

            return imageUrl;
        }
        catch (Exception e)
        {
            return e.Message.ToString();
        }
    }
}