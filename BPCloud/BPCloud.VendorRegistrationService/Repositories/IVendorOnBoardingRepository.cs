using BPCloud.VendorRegistrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.Repositories
{
    public interface IVendorOnBoardingRepository
    {
        List<BPVendorOnBoarding> GetAllVendorOnBoardings();
        List<BPVendorOnBoarding> GetAllOpenVendorOnBoardings();
        List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardings();
        List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardings();
        List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsCount();
        Task<BPVendorOnBoarding> UpdateVendorOnBoardingStatus(BPVendorOnBoarding vendor,string status);
        List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsByPlant(List<string> Plants);
        List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsByPlant(List<string> Plants);
        List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsByPlant(List<string> Plants);
        List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsByApprover(string Approver);
        List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsByApprover(string Approver);
        List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsByApprover(string Approver);
        List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsCountByApprover(string Approver);
        //List<BPVendorOnBoarding> GetAllOpenVendorOnBoardingsCountByPlant(List<string> Plants);

        List<BPIdentity> GetAllIdentity(int TransID);
        List<BPVendorOnBoarding> GetDeclarationID(int TransID);
        BPVendorOnBoarding GetAttachmentId(string arg1);
        List<BPVendorOnBoarding> GetDeclaration_toogle(int TransID);
        List<BPAttachment1> GetAttachmentforXML(int transid);
        List<FTPAttachment> GetIdentityFTPAttachment(int TransID);
        List<FTPAttachment> GetBankFTPAttachment(int TransID);
        List<FTPAttachment> GetMSMEFTPAttachment(int TransID, string MSME_Att_ID);
        List<FTPAttachment> GetRPFTPAttachment(int TransID, string RP_Att_ID);
        List<FTPAttachment> GetTDSFTPAttachment(int TransID, string TDS_Att_ID);
        List<FTPAttachment> GetAllAttachmentsToFTP(BPVendorOnBoarding bPVendorOnBoarding);
        List<BPAttachment1> GetAttachmentforXML_MSME(int MSME_ID);
        List<BPAttachment1> GetAttachmentforXML_RP_ID(int RP_ID);
        List<BPAttachment1> GetAttachmentforXML_TDS(int TDS);

        List<BPBank> bank(int TransID);
        List<BPAttachment1> bank_doc(int TransID, string Accono, string Attachmentname);
        List<BPAttachment> GetAttachmentFile(int TransID, string attachmentname);
        List<BPVendorOnBoarding> GetAllApprovedVendorOnBoardingsCount();
        List<BPVendorOnBoarding> GetAllRejectedVendorOnBoardingsCount();
        BPVendorOnBoarding GetVendorOnBoardingsByID(int TransID);
        BPVendorOnBoarding GetVendorOnBoardingsByEmailID(string EmailID);
        List<BPVendorOnBoarding> GetRegisteredVendorOnBoardings();
        Task<BPVendorOnBoarding> InitializeVendorRegistration(VendorInitialzationClass vendorInitialzationClass);
        bool ChectTokenValidity(VendorTokenCheck tokenCheck);
        Task<BPVendorOnBoarding> CreateVendorOnBoarding(BPVendorOnBoardingView VendorOnBoardingView);
        Task<BPVendorOnBoarding> UpdateVendorOnBoarding(BPVendorOnBoardingView VendorOnBoardingView);
        Task<BPVendorOnBoarding> DeleteVendorOnBoarding(BPVendorOnBoarding VendorOnBoarding);
        Task<BPVendorOnBoarding> ApproveVendor(BPVendorOnBoarding VendorOnBoardingView);
        //bool ApproveVendor_MSMEFTP(BPVendorOnBoarding VendorOnBoardingView);
        Task<BPVendorOnBoarding> RejectVendor(BPVendorOnBoarding VendorOnBoardingView);

        Task<bool> SendMailToApprovalVendor(string toEmail, string password);
        Task<bool> SendMailToRejectVendor(string toEmail, string password, string Remarks);
        QuestionnaireResultSet GetQuestionnaireResultSetByQRID();
        Task DeleteVendorOnboardingById(string TransId);

        BPAttachment GetBPAttachmentByAttachmentId(int attachAttachmentId);
    }
}
