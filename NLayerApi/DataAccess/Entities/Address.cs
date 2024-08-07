using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Address:Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        [MaxLength(300)]
        public string Address1 { get; set; }
        [MaxLength(300)]
        public string Address2 { get; set; }
        [MaxLength(300)]
        public string Address3 { get; set; }
        [MaxLength(300)]
        public string PostCode { get; set; }
        [MaxLength(300)]
        public string City { get; set; }
        public int TownId { get; set; }
        public Town Town { get; set; }
        //public Address(int addressId, string address1, string address2, string address3, string postCode, string city, int townId)
        //{
        //    AddressId = addressId;
        //    Address1 = address1;
        //    Address2 = address2;
        //    Address3 = address3;
        //    PostCode = postCode;
        //    City = city;
        //    TownId = townId;
        //}
        //public int CountyId { get; set; }
        //public Country Country { get; set; }
        //public int NationId { get; set; }
        //public Nation Nation { get; set; }
    }
}