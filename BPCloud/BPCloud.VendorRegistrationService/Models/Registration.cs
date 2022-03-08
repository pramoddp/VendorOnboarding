using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Models
{
    public class CommonClass
    {
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    [Table("bp_vob")]
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
        //public string VendorCode { get; set; }
        //public string ParentVendor { get; set; }
        public string Status { get; set; }
        public string AccountGroup { get; set; }
        public string PurchaseOrg { get; set; }
        public string Department { get; set; }
        public string CompanyCode { get; set; }
        public string EmamiContactPerson { get; set; }
        public string EmamiContactPersonMail { get; set; }
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

    //[Table("bp_vob")]
    //public class BPVendorOnBoarding : CommonClass
    //{
    //    [Key, Column(Order = 1)]
    //    public int TransID { get; set; }
    //    public string Name { get; set; }
    //    public string Role { get; set; }
    //    //public string LegalName { get; set; }
    //    public string Address { get; set; }
    //    //public string AddressLine2 { get; set; }
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string Country { get; set; }
    //    public string PinCode { get; set; }
    //    public string Type { get; set; }
    //    public string Plant { get; set; }
    //    public string C { get; set; }
    //    public string GSTStatus { get; set; }
    //    public string PANNumber { get; set; }
    //    public string Email { get; set; }
    //    public string Phone { get; set; }
    //    public string ContactPerson { get; set; }
    //    //public string Phone2 { get; set; }
    //    //public string Email1 { get; set; }
    //    //public string Email2 { get; set; }
    //    //public string Invoice { get; set; }

    //    //public string WebsiteAddress { get; set; }
    //    //public string VendorCode { get; set; }
    //    //public string ParentVendor { get; set; }
    //    public string Status { get; set; }
    //    //public string Field1 { get; set; }
    //    //public string Field2 { get; set; }
    //    //public string Field3 { get; set; }
    //    //public string Field4 { get; set; }
    //    //public string Field5 { get; set; }
    //    //public string Field6 { get; set; }
    //    //public string Field7 { get; set; }
    //    //public string Field8 { get; set; }
    //    //public string Field9 { get; set; }
    //    //public string Field10 { get; set; }

    //    public string Remarks { get; set; }
    //    public bool MSME { get; set; }
    //    public string MSME_TYPE { get; set; }
    //    public string MSME_Att_ID { get; set; }
    //    public bool Reduced_TDS { get; set; }
    //    //public string TDS_CAT { get; set; }
    //    public string TDS_RATE { get; set; }
    //    public string TDS_Att_ID { get; set; }
    //    public bool RP { get; set; }
    //    public string RP_Name { get; set; }
    //    public string RP_Type { get; set; }
    //    public string RP_Att_ID{get; set;}
    //}
    [Table("bp_id")]
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
    [Table("bp_bank")]
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
    [Table("bp_contact")]
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
    [Table("bp_act_log")]
    public class BPActivityLog : CommonClass
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }
        [ForeignKey("BPVendorOnBoarding"), Column(Order = 2)]
        public int TransID { get; set; }
        public string Activity { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public string Text { get; set; }
        public virtual BPVendorOnBoarding BPVendorOnBoarding { get; set; }
    }
    [Table("bp_text")]
    public class BPText : CommonClass
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TextID { get; set; }
        public string Text { get; set; }
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
    [Table("bp_doc")]
    public class BPAttachment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentID { get; set; }
        public string ProjectName { get; set; }
        public int AppID { get; set; }
        public string AppNumber { get; set; }
        public bool IsHeaderExist { get; set; }
        public string HeaderNumber { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public byte[] AttachmentFile { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class BPAttachment1
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentID { get; set; }
        public string ProjectName { get; set; }
        public int AppID { get; set; }
        public string Type { get; set; }
        public string AppNumber { get; set; }
        public bool IsHeaderExist { get; set; }
        public string HeaderNumber { get; set; }
        public string AttachmentName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public byte[] AttachmentFile { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class FTPAttachment
    {
        public int AttachmentID { get; set; }
        public string TransID { get; set; }
        public string Type { get; set; }
        public string AttachmentName { get; set; }
        public byte[] AttachmentFile { get; set; }
    }

    public class IdentityItem : CommonClass
    {
        public int TransID { get; set; }
        public string Type { get; set; }
        public string IDNumber { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string DocID { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentContents { get; set; }
        public bool IsValid { get; set; }
    }
    public class BankItem : CommonClass
    {
        public int TransID { get; set; }
        public string AccountNo { get; set; }
        public string Name { get; set; }
        public string IFSC { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string City { get; set; }
        public string DocID { get; set; }
        public string AttachmentName { get; set; }
        public bool IsValid { get; set; }
    }
    public class ContactItem : CommonClass
    {
        public int TransID { get; set; }
        public string Item { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }

    #region Question 

    public class Questionnaires : CommonClass
    {
        public int QRID { get; set; }

        [Required]
        [StringLength(100)]
        public string QRText { get; set; }

        public bool IsInActive { get; set; }
    }
    public class QuestionnaireGroup : CommonClass
    {
        public int QRGID { get; set; }
        public int? QRID { get; set; }

        [StringLength(35)]
        public string Language { get; set; }

        [StringLength(50)]
        public string QRGText { get; set; }

        [StringLength(200)]
        public string QRGLText { get; set; }

        public int QRGSortPriority { get; set; }

        public bool DefaultExpanded { get; set; }
    }

    public class QuestionnaireGroupQuestion : CommonClass
    {

        public int QRID { get; set; }
        public int QRGID { get; set; }

        public int QID { get; set; }

        public bool IsMandatory { get; set; }

        public int SortPriority { get; set; }

    }
    public class Answers : CommonClass
    {
        [Key]
        public int AppID { get; set; }

        public int AppUID { get; set; }
        public int QRID { get; set; }

        public int QID { get; set; }

        [StringLength(1000)]
        public string Answer { get; set; }
        public Guid AnsweredBy { get; set; }
        public DateTime AnswredOn { get; set; }

    }

    public class AnswerList
    {
        public List<Answers> Answerss { get; set; }
    }

    public class QAnswerChoice : CommonClass
    {
        public int ChoiceID { get; set; }

        public int QID { get; set; }

        [StringLength(35)]
        public string Language { get; set; }

        [StringLength(50)]
        public string ChoiceText { get; set; }

        public bool IsDefault { get; set; }
    }

    public class Question : CommonClass
    {
        public int QID { get; set; }

        [StringLength(35)]
        public string Language { get; set; }

        [Required]
        [StringLength(50)]
        public string QText { get; set; }

        [StringLength(200)]
        public string QLText { get; set; }

        [StringLength(25)]
        public string QAnsType { get; set; }
    }

    public class QuestionnaireResultSet
    {
        public int QRID { get; set; }
        public List<Questionnaires> Questionnaire { get; set; }
        public List<QuestionnaireGroup> QuestionnaireGroup { get; set; }
        public List<QuestionnaireGroupQuestion> QuestionnaireGroupQuestion { get; set; }
        public List<Question> Questions { get; set; }
        public List<QAnswerChoice> QuestionAnswerChoices { get; set; }
        public List<Answers> Answers { get; set; }
    }
    #endregion

    public class VendorInitialzationClass
    {
        public int TransID { get; set; }
        public string Name { get; set; }
        public string Plant { get; set; }
        public string GSTNumber { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string AccountGroup { get; set; }
        public string PurchaseOrg { get; set; }
        public string Department { get; set; }
        public string CompanyCode { get; set; }

        public string EmamiContactPerson { get; set; }
        public string EmamiContactPersonMail { get; set; }
    }

    public class TokenHistory
    {
        [Key]
        public int TokenHistoryID { get; set; }
        public int TransID { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string OTP { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpireOn { get; set; }
        public DateTime? UsedOn { get; set; }
        public bool IsUsed { get; set; }
        public string Comment { get; set; }
    }

    public class VendorTokenCheck
    {
        public int TransID { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }

    public class User
    {
        public Guid UserID { get; set; }
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
    public class ApproverUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
