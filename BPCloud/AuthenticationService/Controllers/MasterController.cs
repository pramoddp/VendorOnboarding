using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.IRepository;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterRepository _masterRepository;
        public MasterController(IMasterRepository masterRepository)
        {
            //ErrorLog.WriteToFile("Auth/MasterController:- Master Controller Called");
            _masterRepository = masterRepository;
        }

        #region Authentication

        [HttpGet]
        public Client FindClient(string clientId)
        {
            try
            {
                var client = _masterRepository.FindClient(clientId);
                return client;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/FindClient : - " , ex);
                return null;
            }
        }
        [HttpGet]
        public IActionResult AuthenticateUser(string UserName, string Password)
        {
            try
            {
                var result = _masterRepository.AuthenticateUser(UserName,Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/FindClient : - ", ex);
                return null;
            }
        }

        #endregion

        #region SessionMaster

        [HttpGet]
        public SessionMaster GetSessionMasterByProject(string ProjectName)
        {
            try
            {
                var result = _masterRepository.GetSessionMasterByProject(ProjectName);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetSessionMasterByProject : -", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SessionMaster> GetAllSessionMasters()
        {
            try
            {
                var result = _masterRepository.GetAllSessionMasters();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllSessionMasters : -", ex);
                return null;
            }
        }

        [HttpGet]
        public List<SessionMaster> GetAllSessionMastersByProject(string ProjectName)
        {
            try
            {
                var result = _masterRepository.GetAllSessionMastersByProject(ProjectName);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllSessionMastersByProject : -", ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSessionMaster(SessionMaster SessionMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateSessionMaster(SessionMaster);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateSessionMaster : -", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSessionMaster(SessionMaster SessionMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.UpdateSessionMaster(SessionMaster);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateSessionMaster : -", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSessionMaster(SessionMaster SessionMaster)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _masterRepository.DeleteSessionMaster(SessionMaster);
                return new OkResult();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteSessionMaster : - ", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region User
        [Authorize]
        [HttpGet]
        public List<UserWithRole> GetAllUsers()
        {
            try
            {
                var userWithRole = _masterRepository.GetAllUsers();
                return userWithRole;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllUsers", ex);
                return null;
            }
        }

        [HttpGet]
        public List<User> GetAdminUsers()
        {
            try
            {
                var userWithRole = _masterRepository.GetAdminUsers();
                return userWithRole;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAdminUsers", ex);
                return null;
            }
        }

        [HttpGet]
        public List<User> GetApprovers()
        {
            try
            {
                var userWithRole = _masterRepository.GetApprovers();
                return userWithRole;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetApprovers", ex);
                return null;
            }
        }

        [HttpPost]
        public IActionResult CreateUser(UserWithRole userWithRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = _masterRepository.CreateUser(userWithRole);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateUser", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateUser(UserWithRole userWithRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = _masterRepository.UpdateUser(userWithRole);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateUser", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteUser(UserWithRole userWithRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _masterRepository.DeleteUser(userWithRole);
                return new OkResult();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteUser", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateVendorUser(VendorUser vendorUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateVendorUser(vendorUser);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateUser", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateApproverUser(ApproverUser approverUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.CreateApproverUser(approverUser);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateUser", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public List<string> GetApproverPlants(Guid UserID)
        {
            try
            {
                var result = _masterRepository.GetApproverPlants(UserID);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetApproverPlants", ex);
                return null;
            }
        }

        #endregion

        #region Role

        [HttpGet]
        public List<RoleWithApp> GetAllRoles()
        {
            try
            {
                var result = _masterRepository.GetAllRoles();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllRoles", ex);
                return null;
            }
        }

        [HttpPost]
        public IActionResult CreateRole(RoleWithApp roleWithApp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = _masterRepository.CreateRole(roleWithApp);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateRole", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateRole(RoleWithApp roleWithApp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = _masterRepository.UpdateRole(roleWithApp);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateRole", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteRole(RoleWithApp roleWithApp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _masterRepository.DeleteRole(roleWithApp);
                return new OkResult();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteRole", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendMailToApprover (BPVendorOnBoardingView vendor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await _masterRepository.SendMailToApprover(vendor);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/SendMailToApprover", ex);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region App

        [HttpGet]
        public List<App> GetAllApps()
        {
            try
            {
                var result = _masterRepository.GetAllApps();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllApps", ex);
                return null;
            }
        }

        [HttpPost]
        public IActionResult CreateApp(App app)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = _masterRepository.CreateApp(app);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/CreateApp", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult UpdateApp(App app)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = _masterRepository.UpdateApp(app);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateApp", ex);
                return BadRequest(ex.Message);
            }
            //return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteApp(App app)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = _masterRepository.DeleteApp(app);
                return new OkResult();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteApp", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region LogInAndChangePassword

        [HttpPost]
        public IActionResult LoginHistory(Guid UserID, string Username)
        {
            try
            {
                var result = _masterRepository.LoginHistory(UserID,Username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/LoginHistory : - ", ex);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public List<UserLoginHistory> GetAllUsersLoginHistory()
        {
            try
            {
                var result = _masterRepository.GetAllUsersLoginHistory();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllUsersLoginHistory : - ", ex);
                return null;
            }
        }
        [HttpGet]
        public List<UserLoginHistory> GetCurrentUserLoginHistory(Guid UserID)
        {
            try
            {
                var result = _masterRepository.GetCurrentUserLoginHistory(UserID);
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetCurrentUserLoginHistory : - ", ex);
                return null;
            }
        }
        [HttpGet]
        public IActionResult SignOut(Guid UserID)
        {
            try
            {
                var result = _masterRepository.SignOut(UserID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SignOut : - ", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region ChangePassword

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            try
            {
                var result = await _masterRepository.ChangePassword(changePassword);
                if (result != null)
                    return Ok(result);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/ChangePassword : - ", ex);
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> SendResetLinkToMail(EmailModel emailModel)
        {
            try
            {
                var result = await _masterRepository.SendResetLinkToMail(emailModel);
                //if (result != null)
                    return Ok(result);
                //else
                //    return BadRequest(result);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SendResetLinkToMail : - ", ex);
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var result = await _masterRepository.ForgotPassword(forgotPassword);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest("New Password Should Not Same As Default Or Previous 5 Passwords");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/ForgotPassword : - ", ex);
                return BadRequest(ex.Message);
            }
        }

        #endregion

    }
}
