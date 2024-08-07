using System.Net.Http.Json;
using System.Text.Json;
using Common.Custom;
using CommonWeb.Models;

namespace BusinessLayer.Services;

public class GeoLocationService
{
    private readonly HttpClient _httpClient;

    public GeoLocationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

        public async Task<GetGeoLocationModel?> GetGeoLocation(string postId)
        {
            string key = "PD55-ZM54-HJ92-HP62";
            string url = $"https://api.addressy.com/GovernmentData/Postzon/RetrieveByPostcode/v1.50/json.ws?Key={key}&Postcode={postId}";

            string data = await _httpClient.GetStringAsync(url);
            string jsondata = data.Trim();
            string laststep = jsondata.Substring(1, jsondata.Length - 2);
            if (string.IsNullOrEmpty(laststep))
            {
                throw new ArgumentException("Failed to retrieve geo location data.");
            }

            GetGeoLocationModel? response = JsonSerializer.Deserialize<GetGeoLocationModel>(laststep);

            if (response.WardName == null)
            {
                GeneralError? error = JsonSerializer.Deserialize<GeneralError>(laststep);
                throw new ArgumentException($"Error :{error.Error}. Description: {error.Description}");
            }

            response.LocalAuthority = $"{response.DistrictName} Council";
            response.UnitaryAuthority = (string.IsNullOrEmpty(response.CountyCode) && !string.IsNullOrEmpty(response.DistrictName))
                ? response.DistrictName
                : "none";

            return response;
        }
    }