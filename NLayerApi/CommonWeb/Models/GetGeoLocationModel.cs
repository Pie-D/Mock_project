namespace CommonWeb.Models;

public class GetGeoLocationModel
{
    public string WardName { get; set; }    //Ward
    public string DistrictName { get; set; }  //Borough
    public string? CountyCode { get; set; }

    public string LocalAuthority { get; set; } //DistrictName + " Council"
    public string UnitaryAuthority { get; set; } // DistrictName
    public string NhsShaName { get; set; } // NHS Authority
}