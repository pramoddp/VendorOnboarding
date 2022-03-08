using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.Models
{
    public class CommonClass
    {
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    [Table("cbp_type")]
    public class CBPType : CommonClass
    {
        [Key, Column(Order = 1)]
        public string Type { get; set; }
        [Key, Column(Order = 2)]
        public string Language { get; set; }
        public string Text { get; set; }
    }
    [Table("cbp_postal")]
    public class CBPPostal : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }
    [Table("cbp_id")]
    public class CBPIdentity : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        //public string Format { get; set; }
        //public string DocReq { get; set; }
        //public DateTime? ExpDateReq { get; set; }
        //public string Country { get; set; }
        public string RegexFormat { get; set; }
        public bool Mandatory { get; set; }
        public string FileFormat { get; set; }
        public double MaxSizeInKB { get; set; }
    }
    [Table("cbp_bank")]
    public class CBPBank : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankCity { get; set; }
        public string BankCountry { get; set; }
        public string BankBranch { get; set; }
    }
    [Table("cbp_title")]
    public class CBPTitle : CommonClass
    {
        [Key, Column(Order = 1)]
        public string Title { get; set; }
        [Key, Column(Order = 2)]
        public string Language { get; set; }
        public string TitleText { get; set; }
    }
    [Table("cbp_dept")]
    public class CBPDepartment : CommonClass
    {
        [Key, Column(Order = 1)]
        public string Department { get; set; }
        [Key, Column(Order = 2)]
        public string Language { get; set; }
        public string Text { get; set; }
    }

    [Table("cbp_app")]
    public class CBPApp : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string CCode { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }
        public string User { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    [Table("cbp_location")]
    public class CBPLocation
    {
        [Key]
        [MaxLength(20)]
        public string Pincode { get; set; }
        [MaxLength(100)]
        public string Location { get; set; }
        [MaxLength(100)]
        public string Taluk { get; set; }
        [MaxLength(100)]
        public string District { get; set; }
        [MaxLength(100)]
        public string State { get; set; }
        [MaxLength(10)]
        public string StateCode { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }
        [MaxLength(10)]
        public string CountryCode { get; set; }
    }

    [Table("cbp_gstin")]
    public class CBPGstin : CommonClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Gstin { get; set; }
        public string CustomerUserName { get; set; }
        public string CustomerPassword { get; set; }
        public string GSPClientID { get; set; }
        public string GSPClientSecret { get; set; }
        public string PublicKeyPath { get; set; }
    }

    [Table("cbp_fieldmaster")]
    public class CBPFieldMaster : CommonClass
    {
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        public string Field { get; set; }
        public string FieldName { get; set; }
        public string Text { get; set; }
        public string DefaultValue { get; set; }
        public bool Mandatory { get; set; }
        public bool Invisible { get; set; }
    }

    
    public class TaxPayerDetailsError
    {
        public string errorMessage { get; set; }
        public int status { get; set; }
    }

    public class StateDetails
    {
        public string State { get; set; }
        public string StateCode { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class PostOffice
    {
        public string Name { get; set; }
        public object Description { get; set; }
        public string BranchType { get; set; }
        public string DeliveryStatus { get; set; }
        public string Circle { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string Region { get; set; }
        public string Block { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
    }

    public class MyArray
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public List<PostOffice> PostOffice { get; set; }
    }

    public class Postal
    {
        public List<MyArray> MyArray { get; set; }
    }
    public class GspAuthResponse
    {
        // Properties
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string scope { get; set; }
        public string jti { get; set; }

        public string token_type { get; set; }
        public bool errorStatus { get; set; }
        public string errorMessage { get; set; }
    }
    public class GSTRResponse
    {
        public string data { get; set; }
        public string rek { get; set; }
        public string hmac { get; set; }
        public string status_cd { get; set; }
        public bool errorStatus { get; set; }
        public string errorMessage { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Addr
    {
        public string bnm { get; set; }
        public string st { get; set; }
        public string loc { get; set; }
        public string bno { get; set; }
        public string stcd { get; set; }
        public string dst { get; set; }
        public string city { get; set; }
        public string flno { get; set; }
        public string lt { get; set; }
        public string pncd { get; set; }
        public string lg { get; set; }
    }

    public class Adadr
    {
        public Addr addr { get; set; }
        public string ntr { get; set; }
    }

    public class Pradr
    {
        public Addr addr { get; set; }
        public string ntr { get; set; }
    }

    public class TaxPayer
    {
        public string stjCd { get; set; }
        public string lgnm { get; set; }
        public string stj { get; set; }
        public string dty { get; set; }
        public List<Adadr> adadr { get; set; }
        public string cxdt { get; set; }
        public string gstin { get; set; }
        public List<string> nba { get; set; }
        public string lstupdt { get; set; }
        public string rgdt { get; set; }
        public string ctb { get; set; }
        public Pradr pradr { get; set; }
        public string sts { get; set; }
        public string tradeNam { get; set; }
        public string ctjCd { get; set; }
        public string ctj { get; set; }
    }

    public class Errors
    {
        public string message { get; set; }
        public string error_cd { get; set; }
    }

    public class ErrorResponse
    {
        public Errors error { get; set; }
        public int status_cd { get; set; }
    }
    public class TaxPayerDetails
    {
        public string gstin { get; set; }
        public string tradeName { get; set; }
        public string legalName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string stateCode { get; set; }
        public string pinCode { get; set; }
        public string txpType { get; set; }
        public string status { get; set; }
        public string blkStatus { get; set; }
    }
}
