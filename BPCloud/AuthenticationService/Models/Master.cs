using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class LdapUser
    {
        public Guid UserID { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Path { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }

    public class User
    {
        [Key]
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public string Pass3 { get; set; }
        public string Pass4 { get; set; }
        public string Pass5 { get; set; }
        public string LastChangedPassword { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? IsLockDuration { get; set; }
        public int Attempts { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class Role
    {
        [Key]
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class UserRoleMap
    {
        [Column(Order = 0), Key, ForeignKey("User")]
        public Guid UserID { get; set; }
        [Column(Order = 1), Key, ForeignKey("Role")]
        public Guid RoleID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class App
    {
        [Key]
        public int AppID { get; set; }
        public string AppName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class RoleAppMap
    {

        [Key]
        [Column(Order = 1)]
        //[Column(Order = 0), Key, ForeignKey("Role")]
        public Guid RoleID { get; set; }
        //[Column(Order = 1), Key, ForeignKey("App")]
        [Key]
        [Column(Order = 2)]
        public int AppID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class UserPlantMap
    {
        [Column(Order = 0), Key, ForeignKey("User")]
        public Guid UserID { get; set; }
        [Column(Order = 1), Key]
        public string Plant { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public virtual User User { get; set; }
    }

    public class UserWithRole
    {
        public Guid UserID { get; set; }
        public Guid RoleID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class UserView
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
    }

    public class RoleWithApp
    {
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public int[] AppIDList { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class UserLoginHistory
    {
        [Key]
        public int ID { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        //public string IP { get; set; }
    }

    public class ChangePassword
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class EmailModel
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string siteURL { get; set; }
    }

    public class ForgotPassword
    {
        public Guid UserID { get; set; }
        public string EmailAddress { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }

    public class TokenHistory
    {
        [Key]
        public int TokenHistoryID { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpireOn { get; set; }
        public DateTime? UsedOn { get; set; }
        public bool IsUsed { get; set; }
        public string Comment { get; set; }
    }
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string clientId { get; set; }
    }
    public class AuthenticationResult
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string UserRole { get; set; }
        public string Token { get; set; }
        public string MenuItemNames { get; set; }
        public string IsChangePasswordRequired { get; set; }
        public string ReasonForReset { get; set; }
        public string Profile { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class VendorUser
    {
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public class ApproverUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class STMPDetails
    {
        public string Host { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
    }
    public class SessionMaster
    {
        [Key]
        public int ID { get; set; }
        public string ProjectName { get; set; }
        public int SessionTimeOut { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class BPVendorOnBoardingView : CommonClass
    {
        public int TransID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string LegalName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Type { get; set; }
        public string Plant { get; set; }
        public string GSTNumber { get; set; }
        public string GSTStatus { get; set; }
        public string PANNumber { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Invoice { get; set; }

        public string WebsiteAddress { get; set; }
        public string VendorCode { get; set; }
        public string ParentVendor { get; set; }
        public string Status { get; set; }
        //public string Field1 { get; set; }
        //public string Field2 { get; set; }
        //public string Field3 { get; set; }
        //public string Field4 { get; set; }
        //public string Field5 { get; set; }
        //public string Field6 { get; set; }
        //public string Field7 { get; set; }
        //public string Field8 { get; set; }
        //public string Field9 { get; set; }
        //public string Field10 { get; set; }
        public string AccountGroup { get; set; }
        public string PurchaseOrg { get; set; }
        public string Department { get; set; }
        public string CompanyCode { get; set; }
        public string EmamiContactPerson { get; set; }
        public string EmamiContactPersonMail { get; set; }
        public string TypeofIndustry { get; set; }
        public string Remarks { get; set; }
        public bool MSME { get; set; }
        public string MSME_TYPE { get; set; }
        public string MSME_Att_ID { get; set; }
        public bool Reduced_TDS { get; set; }
        //public string TDS_CAT { get; set; }
        public string TDS_RATE { get; set; }
        public string TDS_Att_ID { get; set; }
        public bool RP { get; set; }
        public string RP_Name { get; set; }
        public string RP_Type { get; set; }
        public string RP_Att_ID { get; set; }
        public string Token { get; set; }
        public List<BPIdentity> bPIdentities { get; set; }
        public List<BPBank> bPBanks { get; set; }
        public List<BPContact> bPContacts { get; set; }

    }
    public class BPIdentity : CommonClass
    {
        [Key, ForeignKey("BPVendorOnBoarding"), Column(Order = 1)]
        public int TransID { get; set; }
        [Key, Column(Order = 2)]
        public string Type { get; set; }
        public string Option { get; set; }
        public string IDNumber { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string DocID { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentContents { get; set; }
        public bool IsValid { get; set; }
        public virtual BPVendorOnBoarding BPVendorOnBoarding { get; set; }
    }
    public class BPBank : CommonClass
    {
        [Key, ForeignKey("BPVendorOnBoarding"), Column(Order = 1)]
        public int TransID { get; set; }
        [Key, Column(Order = 2)]
        public string AccountNo { get; set; }
        public string Name { get; set; }
        public string IFSC { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string DocID { get; set; }
        public string AttachmentName { get; set; }
        public bool IsValid { get; set; }
        public virtual BPVendorOnBoarding BPVendorOnBoarding { get; set; }
    }
    public class BPContact : CommonClass
    {
        [Key, ForeignKey("BPVendorOnBoarding"), Column(Order = 1)]
        public int TransID { get; set; }
        [Key, Column(Order = 2)]
        public string Item { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public virtual BPVendorOnBoarding BPVendorOnBoarding { get; set; }
    }
    public class BPVendorOnBoarding : CommonClass
    {
        [Key, Column(Order = 1)]
        public int TransID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string LegalName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Type { get; set; }
        public string Plant { get; set; }
        public string GSTNumber { get; set; }
        public string GSTStatus { get; set; }
        public string PANNumber { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        //public string ContactPerson { get; set; }
        //public string Email1 { get; set; }
        //public string Email2 { get; set; }
        //public string Invoice { get; set; }
        //public string WebsiteAddress { get; set; }
        public string VendorCode { get; set; }
        //public string ParentVendor { get; set; }
        public string Status { get; set; }
        public string AccountGroup { get; set; }
        public string PurchaseOrg { get; set; }
        public string Department { get; set; }
        public string CompanyCode { get; set; }
        public string EmamiContactPerson { get; set; }
        public string EmamiContactPersonMail { get; set; }

        //public string VendorCode { get; set; }
        public string TypeofIndustry { get; set; }
        //public string Field1 { get; set; }
        //public string Field2 { get; set; }
        //public string Field3 { get; set; }
        //public string Field4 { get; set; }
        //public string Field5 { get; set; }
        //public string Field6 { get; set; }
        //public string Field7 { get; set; }
        //public string Field8 { get; set; }
        //public string Field9 { get; set; }
        //public string Field10 { get; set; }

        public string Remarks { get; set; }
        public bool MSME { get; set; }
        public string MSME_TYPE { get; set; }
        public string MSME_Att_ID { get; set; }
        public bool Reduced_TDS { get; set; }
        //public string TDS_CAT { get; set; }
        public string TDS_RATE { get; set; }
        public string TDS_Att_ID { get; set; }
        public bool RP { get; set; }
        public string RP_Name { get; set; }
        public string RP_Type { get; set; }
        public string RP_Att_ID { get; set; }
    }
    public class CommonClass
    {
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
