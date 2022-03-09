using AuthenticationService.IRepository;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AuthenticationService.Repository
{
    public class MasterRepository : IMasterRepository
    {
        readonly AuthContext _dbContext;
        IConfiguration _configuration;
        private int _tokenTimespan = 0;

        public MasterRepository(AuthContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
            try
            {
                var span = "30";
                if (span != "")
                    _tokenTimespan = Convert.ToInt32(span.ToString());
                if (_tokenTimespan <= 0)
                {
                    _tokenTimespan = 30;
                }
            }
            catch
            {
                _tokenTimespan = 30;
            }
        }

        #region Authentication

        public Client FindClient(string clientId)
        {
            try
            {
                var client = _dbContext.Clients.Find(clientId);
                return client;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/FindClient : - ", ex);
                return null;
            }
        }

        public async Task<AuthenticationResult> AuthenticateUser(string UserName, string Password)
        {
            try
            {
                AuthenticationResult authenticationResult = new AuthenticationResult();
                List<string> MenuItemList = new List<string>();
                string MenuItemNames = "";
                string ProfilesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");
                string Profile = "Empty";
                User user = null;
                string isChangePasswordRequired = "No";
                string DefaultPassword = _configuration["DefaultPassword"];

                if (UserName.Contains('@') && UserName.Contains('.'))
                {
                    user = (from tb in _dbContext.Users
                            where tb.Email == UserName && tb.IsActive
                            select tb).FirstOrDefault();
                }
                else
                {
                    user = (from tb in _dbContext.Users
                            where tb.UserName == UserName && tb.IsActive
                            select tb).FirstOrDefault();
                }

                if (user != null)
                {
                    bool isValidUser = false;
                    string DecryptedPassword = Decrypt(user.Password, true);
                    isValidUser = DecryptedPassword == Password;
                    if (user.IsLocked && DateTime.Now >= user.IsLockDuration)
                    {
                        user.IsLocked = false;
                    }
                    if (isValidUser && !user.IsLocked)
                    {
                        user.Attempts = 0;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        if (Password == DefaultPassword)
                        {
                            isChangePasswordRequired = "Yes";
                        }
                        //MasterController MasterController = new MasterController();
                        //await MasterController.LoginHistory(user.UserID, user.UserName);
                        Role userRole = (from tb1 in _dbContext.Roles
                                         join tb2 in _dbContext.UserRoleMaps on tb1.RoleID equals tb2.RoleID
                                         join tb3 in _dbContext.Users on tb2.UserID equals tb3.UserID
                                         where tb3.UserID == user.UserID && tb1.IsActive && tb2.IsActive && tb3.IsActive
                                         select tb1).FirstOrDefault();

                        if (userRole != null)
                        {
                            MenuItemList = (from tb1 in _dbContext.Apps
                                            join tb2 in _dbContext.RoleAppMaps on tb1.AppID equals tb2.AppID
                                            where tb2.RoleID == userRole.RoleID && tb1.IsActive && tb2.IsActive
                                            select tb1.AppName).ToList();
                            foreach (string item in MenuItemList)
                            {
                                if (MenuItemNames == "")
                                {
                                    MenuItemNames = item;
                                }
                                else
                                {
                                    MenuItemNames += "," + item;
                                }
                            }
                        }
                        authenticationResult.IsSuccess = true;
                        authenticationResult.UserID = user.UserID;
                        authenticationResult.UserName = user.UserName;
                        authenticationResult.DisplayName = user.UserName;
                        authenticationResult.EmailAddress = user.Email;
                        authenticationResult.UserRole = userRole != null ? userRole.RoleName : string.Empty;
                        authenticationResult.MenuItemNames = MenuItemNames;
                        authenticationResult.IsChangePasswordRequired = isChangePasswordRequired;
                        authenticationResult.Profile = string.IsNullOrEmpty(Profile) ? "Empty" : Profile;
                    }
                    else
                    {
                        authenticationResult.IsSuccess = false;
                        authenticationResult.Message = "The user name or password is incorrect.";
                        user.Attempts++;
                        if (user.Attempts == 5)
                        {
                            user.IsLocked = true;
                            user.IsLockDuration = DateTime.Now.AddMinutes(15);
                            authenticationResult.Message = "Wait for 15mins to login";
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    authenticationResult.IsSuccess = false;
                    authenticationResult.Message = "The user name or password is incorrect.";
                }

                return authenticationResult;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/AuthenticateUser : - ", ex);
                return null;
            }
        }

        #endregion

        #region User

        public List<UserWithRole> GetAllUsers()
        {

            try
            {
                var result = (from tb in _dbContext.Users
                              join tb1 in _dbContext.UserRoleMaps on tb.UserID equals tb1.UserID
                              where tb.IsActive && tb1.IsActive
                              select new
                              {
                                  tb.UserID,
                                  tb.UserName,
                                  tb.Email,
                                  tb.ContactNumber,
                                  tb.Password,
                                  tb.IsActive,
                                  tb.CreatedOn,
                                  tb.ModifiedOn,
                                  tb1.RoleID
                              }).ToList();

                List<UserWithRole> UserWithRoleList = new List<UserWithRole>();

                result.ForEach(record =>
                {
                    UserWithRoleList.Add(new UserWithRole()
                    {
                        UserID = record.UserID,
                        UserName = record.UserName,
                        Email = record.Email,
                        ContactNumber = record.ContactNumber,
                        Password = Decrypt(record.Password, true),
                        IsActive = record.IsActive,
                        CreatedOn = record.CreatedOn,
                        ModifiedOn = record.ModifiedOn,
                        RoleID = record.RoleID
                    });

                });
                return UserWithRoleList;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllUsers : - ", ex);
                return null;
            }
        }

        public List<User> GetAdminUsers()
        {

            try
            {
                var result = (from tb in _dbContext.Users
                              join tb1 in _dbContext.UserRoleMaps on tb.UserID equals tb1.UserID
                              join tb2 in _dbContext.Roles on tb1.RoleID equals tb2.RoleID
                              where tb.IsActive && tb1.IsActive && tb2.IsActive && tb2.RoleName.ToLower() == "administrator"
                              select tb).ToList();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAdminUsers : - ", ex);
                return null;
            }
        }
        public List<User> GetApprovers()
        {

            try
            {
                var result = (from tb in _dbContext.Users
                              join tb1 in _dbContext.UserRoleMaps on tb.UserID equals tb1.UserID
                              join tb2 in _dbContext.Roles on tb1.RoleID equals tb2.RoleID
                              where tb.IsActive && tb1.IsActive && tb2.IsActive && tb2.RoleName.ToLower() == "approver"
                              select tb).ToList();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetApprovers : - ", ex);
                return null;
            }
        }
        public async Task<UserWithRole> CreateUser(UserWithRole userWithRole)
        {
            UserWithRole userResult = new UserWithRole();
            try
            {
                // Creating User
                User user1 = (from tb1 in _dbContext.Users
                              where tb1.UserName == userWithRole.UserName && tb1.IsActive
                              select tb1).FirstOrDefault();

                if (user1 == null)
                {
                    User user2 = (from tb1 in _dbContext.Users
                                  where tb1.Email == userWithRole.Email && tb1.IsActive
                                  select tb1).FirstOrDefault();
                    if (user2 == null)
                    {
                        //string DefaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
                        string DefaultPassword = "Exalca@123";
                        User user = new User();
                        user.UserID = Guid.NewGuid();
                        user.UserName = userWithRole.UserName;
                        user.Email = userWithRole.Email;
                        user.Password = Encrypt(DefaultPassword, true);
                        user.ContactNumber = userWithRole.ContactNumber;
                        user.CreatedBy = userWithRole.CreatedBy;
                        user.IsActive = true;
                        user.CreatedOn = DateTime.Now;
                        user.IsLocked = false;
                        user.Attempts = 0;
                        //user.ExpiryDate = DateTime.Now.AddDays(90);
                        var result = _dbContext.Users.Add(user);
                        //_dbContext.Users.Add(user);
                        await _dbContext.SaveChangesAsync();

                        UserRoleMap UserRole = new UserRoleMap()
                        {
                            RoleID = userWithRole.RoleID,
                            UserID = user.UserID,
                            IsActive = true,
                            CreatedOn = DateTime.Now
                        };
                        var result1 = _dbContext.UserRoleMaps.Add(UserRole);
                        await _dbContext.SaveChangesAsync();

                        userResult.UserName = user.UserName;
                        userResult.Email = user.Email;
                        userResult.ContactNumber = user.ContactNumber;
                        userResult.UserID = user.UserID;
                        userResult.Password = user.Password;
                        userResult.RoleID = UserRole.RoleID;
                        // Attachment
                    }
                    else
                    {
                        return userResult;
                        //return BadRequest("User with same email address already exist");
                    }
                }
                else
                {
                    return userResult;
                    //return Content(HttpStatusCode.BadRequest, "User with same name already exist");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateUser : - ", ex);
                return null;
            }
            return userResult;
            //_dbContext.Users.Add(entity);
            //_dbContext.SaveChanges();
        }

        public async Task<UserWithRole> UpdateUser(UserWithRole userWithRole)
        {
            UserWithRole userResult = new UserWithRole();
            try
            {

                User user1 = (from tb1 in _dbContext.Users
                              where tb1.UserName == userWithRole.UserName && tb1.IsActive && tb1.UserID != userWithRole.UserID
                              select tb1).FirstOrDefault();

                if (user1 == null)
                {
                    User user2 = (from tb1 in _dbContext.Users
                                  where tb1.Email == userWithRole.Email && tb1.IsActive && tb1.UserID != userWithRole.UserID
                                  select tb1).FirstOrDefault();
                    if (user2 == null)
                    {
                        //Updating User details
                        var user = (from tb in _dbContext.Users
                                    where tb.IsActive &&
                                    tb.UserID == userWithRole.UserID
                                    select tb).FirstOrDefault();
                        user.UserName = userWithRole.UserName;
                        user.Email = userWithRole.Email;
                        //user.Password = Encrypt(userWithRole.Password, true);
                        user.ContactNumber = userWithRole.ContactNumber;
                        user.IsActive = true;
                        user.ModifiedOn = DateTime.Now;
                        user.ModifiedBy = userWithRole.ModifiedBy;
                        await _dbContext.SaveChangesAsync();

                        UserRoleMap OldUserRole = _dbContext.UserRoleMaps.Where(x => x.UserID == userWithRole.UserID && x.IsActive).FirstOrDefault();
                        if (OldUserRole.RoleID != userWithRole.RoleID)
                        {
                            //Delete old role related to the user
                            _dbContext.UserRoleMaps.Remove(OldUserRole);
                            _dbContext.SaveChanges();

                            //Add new roles for the user
                            UserRoleMap UserRole = new UserRoleMap()
                            {
                                RoleID = userWithRole.RoleID,
                                UserID = user.UserID,
                                IsActive = true,
                                CreatedBy = userWithRole.ModifiedBy,
                                CreatedOn = DateTime.Now,
                            };
                            var r = _dbContext.UserRoleMaps.Add(UserRole);
                            await _dbContext.SaveChangesAsync();

                            userResult.UserName = user.UserName;
                            userResult.Email = user.Email;
                            userResult.ContactNumber = user.ContactNumber;
                            userResult.UserID = user.UserID;
                            userResult.Password = user.Password;
                            userResult.RoleID = UserRole.RoleID;
                        }

                    }
                    else
                    {
                        return userResult;
                        //return Content(HttpStatusCode.BadRequest, "User with same email address already exist");
                    }
                }
                else
                {
                    return userResult;
                    //return Content(HttpStatusCode.BadRequest, "User with same name already exist");
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/UpdateUser : - ", ex);
                return null;
            }
            return userResult;
        }

        public async Task<UserWithRole> DeleteUser(UserWithRole userWithRole)
        {
            UserWithRole userResult = new UserWithRole();
            try
            {
                var result = (from tb in _dbContext.Users
                              where tb.IsActive &&
                              tb.UserID == userWithRole.UserID
                              select tb).FirstOrDefault();
                if (result == null)
                {
                    return userResult;
                }
                else
                {
                    //result.IsActive = false;
                    //result.ModifiedOn = DateTime.Now;
                    //result.ModifiedBy = userWithRole.ModifiedBy;
                    _dbContext.Users.Remove(result);
                    await _dbContext.SaveChangesAsync();

                    //Changing the Status of role related to the user
                    UserRoleMap UserRole = _dbContext.UserRoleMaps.Where(x => x.UserID == userWithRole.UserID && x.IsActive).FirstOrDefault();
                    //UserRole.IsActive = false;
                    //UserRole.ModifiedOn = DateTime.Now;
                    //UserRole.ModifiedBy = userWithRole.ModifiedBy;
                    _dbContext.UserRoleMaps.Remove(UserRole);
                    await _dbContext.SaveChangesAsync();

                    userResult.UserName = result.UserName;
                    userResult.Email = result.Email;
                    userResult.ContactNumber = result.ContactNumber;
                    userResult.UserID = result.UserID;
                    userResult.Password = result.Password;
                    userResult.RoleID = UserRole.RoleID;
                    return userResult;
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/DeleteUser : - ", ex);
                return null;
            }
        }

        public async Task<UserWithRole> CreateVendorUser(VendorUser vendorUser)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                UserWithRole userResult = new UserWithRole();
                try
                {
                    // Creating User
                    User user1 = (from tb1 in _dbContext.Users
                                  where tb1.UserName == vendorUser.Email && tb1.IsActive
                                  select tb1).FirstOrDefault();

                    if (user1 == null)
                    {
                        User user2 = (from tb1 in _dbContext.Users
                                      where tb1.Email == vendorUser.Email && tb1.IsActive
                                      select tb1).FirstOrDefault();
                        if (user2 == null)
                        {
                            //string DefaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
                            string DefaultPassword = vendorUser.Phone;
                            User user = new User();
                            user.UserID = Guid.NewGuid();
                            user.UserName = vendorUser.Email;
                            user.Email = vendorUser.Email;
                            user.Password = Encrypt(DefaultPassword, true);
                            user.ContactNumber = vendorUser.Phone;
                            user.CreatedBy = vendorUser.Email;
                            user.IsActive = true;
                            user.Attempts = 0;
                            user.IsLocked = false;
                            //user.
                            user.CreatedOn = DateTime.Now;
                            var result = _dbContext.Users.Add(user);
                            //_dbContext.Users.Add(user);
                            await _dbContext.SaveChangesAsync();

                            var VendorRoleID = _dbContext.Roles.Where(x => x.RoleName.ToLower() == "vendor").Select(y => y.RoleID).FirstOrDefault();

                            UserRoleMap UserRole = new UserRoleMap()
                            {
                                RoleID = VendorRoleID,
                                UserID = user.UserID,
                                IsActive = true,
                                CreatedOn = DateTime.Now
                            };
                            var result1 = _dbContext.UserRoleMaps.Add(UserRole);
                            await _dbContext.SaveChangesAsync();

                            userResult.UserID = user.UserID;
                            userResult.UserName = user.UserName;
                            userResult.Email = user.Email;
                            userResult.ContactNumber = user.ContactNumber;
                            userResult.UserID = user.UserID;
                            userResult.Password = user.Password;
                            userResult.RoleID = UserRole.RoleID;

                            //var SiteURL = _configuration["SiteURL"];
                            await SendMailToVendor(vendorUser.Email, vendorUser.Phone);
                            transaction.Commit();
                            transaction.Dispose();
                            return userResult;
                            // Attachment
                        }
                        else
                        {
                            //return userResult;
                            transaction.Rollback();
                            transaction.Dispose();
                            throw new Exception("User with same email address already exist");
                        }
                    }
                    else
                    {
                        //return userResult;
                        transaction.Rollback();
                        transaction.Dispose();
                        throw new Exception("User with same name already exist");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteToFile("Master/CreateUser : - ", ex);
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public async Task<UserWithRole> CreateApproverUser(ApproverUser approverUser)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                UserWithRole userResult = new UserWithRole();
                try
                {
                    // Creating User
                    User user1 = (from tb1 in _dbContext.Users
                                  where tb1.UserName == approverUser.UserName && tb1.IsActive
                                  select tb1).FirstOrDefault();

                    if (user1 == null)
                    {
                        //User user2 = (from tb1 in _dbContext.Users
                        //              where tb1.Email == approverUser.Email && tb1.IsActive
                        //              select tb1).FirstOrDefault();
                        //if (user2 == null)
                        //{
                        //string DefaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
                        string DefaultPassword = _configuration["DefaultPassword"];
                        User user = new User();
                        user.UserID = Guid.NewGuid();
                        user.UserName = approverUser.UserName;
                        user.Email = approverUser.Email;
                        user.Password = Encrypt(DefaultPassword, true);
                        user.ContactNumber = "";
                        user.CreatedBy = approverUser.Email;
                        user.IsActive = true;
                        user.CreatedOn = DateTime.Now;
                        var result = _dbContext.Users.Add(user);
                        //_dbContext.Users.Add(user);
                        await _dbContext.SaveChangesAsync();

                        var VendorRoleID = _dbContext.Roles.Where(x => x.RoleName.ToLower() == "approver").Select(y => y.RoleID).FirstOrDefault();

                        UserRoleMap UserRole = new UserRoleMap()
                        {
                            RoleID = VendorRoleID,
                            UserID = user.UserID,
                            IsActive = true,
                            CreatedOn = DateTime.Now
                        };
                        var result1 = _dbContext.UserRoleMaps.Add(UserRole);
                        await _dbContext.SaveChangesAsync();

                        userResult.UserID = user.UserID;
                        userResult.UserName = user.UserName;
                        userResult.Email = user.Email;
                        userResult.ContactNumber = user.ContactNumber;
                        userResult.UserID = user.UserID;
                        userResult.Password = user.Password;
                        userResult.RoleID = UserRole.RoleID;

                        //var SiteURL = _configuration["SiteURL"];
                        //await SendMailToVendor(approverUser.Email, approverUser.Phone);
                        transaction.Commit();
                        transaction.Dispose();
                        return userResult;
                        // Attachment
                        //}
                        //else
                        //{
                        //    //return userResult;
                        //    transaction.Rollback();
                        //    transaction.Dispose();
                        //    throw new Exception("User with same email address already exist");
                        //}
                    }
                    else
                    {
                        //return userResult;
                        ErrorLog.WriteToFile($"Master/CreateUser : - User with same {approverUser.UserName} already exist");
                        transaction.Rollback();
                        transaction.Dispose();
                        //throw new Exception("User with same name already exist");
                        return userResult;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteToFile("Master/CreateUser : - ", ex);
                    transaction.Rollback();
                    transaction.Dispose();
                    //throw ex;
                    return userResult;
                }
            }
        }

        public List<string> GetApproverPlants(Guid UserID)
        {
            try
            {
                var result = (from tb in _dbContext.UserPlantMaps
                              where tb.UserID == UserID
                              select tb.Plant).ToList();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetApproverPlants : - ", ex);
                throw ex;
            }
        }

        #endregion

        #region SessionMaster

        public SessionMaster GetSessionMasterByProject(string ProjectName)
        {
            try
            {
                SessionMaster sessionMasters = (from tb in _dbContext.SessionMasters
                                                where tb.IsActive && tb.ProjectName.ToLower() == ProjectName.ToLower()
                                                select tb).FirstOrDefault();

                return sessionMasters;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetSessionMasterByProject : - ", ex);
                return null;
            }
        }

        public List<SessionMaster> GetAllSessionMasters()
        {
            try
            {
                var result = (from tb in _dbContext.SessionMasters
                              where tb.IsActive
                              select tb).ToList();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllSessionMasters : - ", ex);
                return null;

            }
        }

        public List<SessionMaster> GetAllSessionMastersByProject(string ProjectName)
        {
            try
            {
                var result = (from tb in _dbContext.SessionMasters
                              where tb.IsActive && tb.ProjectName.ToLower() == ProjectName.ToLower()
                              select tb).ToList();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllSessionMastersByProject : - ", ex);
                return null;

            }
        }


        public async Task<SessionMaster> CreateSessionMaster(SessionMaster SessionMaster)
        {
            SessionMaster SessionMasterResult = new SessionMaster();
            try
            {
                SessionMaster SessionMaster1 = (from tb in _dbContext.SessionMasters
                                                where tb.IsActive && tb.ProjectName == SessionMaster.ProjectName
                                                select tb).FirstOrDefault();
                if (SessionMaster1 == null)
                {
                    SessionMaster.CreatedOn = DateTime.Now;
                    SessionMaster.IsActive = true;
                    var result = _dbContext.SessionMasters.Add(SessionMaster);
                    await _dbContext.SaveChangesAsync();

                    SessionMasterResult.ID = SessionMaster.ID;
                    SessionMasterResult.ProjectName = SessionMaster.ProjectName;
                    SessionMasterResult.SessionTimeOut = SessionMaster.SessionTimeOut;
                }
                else
                {
                    return SessionMasterResult;
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateSessionMaster : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return SessionMasterResult;
        }

        public async Task<SessionMaster> UpdateSessionMaster(SessionMaster SessionMaster)
        {
            SessionMaster SessionMasterResult = new SessionMaster();
            try
            {
                SessionMaster SessionMaster1 = (from tb in _dbContext.SessionMasters
                                                where tb.IsActive && tb.ProjectName == SessionMaster.ProjectName && tb.ID != SessionMaster.ID
                                                select tb).FirstOrDefault();
                if (SessionMaster1 == null)
                {
                    SessionMaster SessionMaster2 = (from tb in _dbContext.SessionMasters
                                                    where tb.IsActive && tb.ID == SessionMaster.ID
                                                    select tb).FirstOrDefault();
                    SessionMaster2.SessionTimeOut = SessionMaster.SessionTimeOut;
                    SessionMaster2.IsActive = true;
                    SessionMaster2.ModifiedOn = DateTime.Now;
                    SessionMaster2.ModifiedBy = SessionMaster.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    SessionMasterResult.ID = SessionMaster.ID;
                    SessionMasterResult.ProjectName = SessionMaster.ProjectName;
                    SessionMasterResult.SessionTimeOut = SessionMaster.SessionTimeOut;
                }
                else
                {
                    return SessionMasterResult;
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateSessionMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return SessionMasterResult;
        }

        public async Task<SessionMaster> DeleteSessionMaster(SessionMaster SessionMaster)
        {
            SessionMaster SessionMasterResult = new SessionMaster();
            try
            {
                SessionMaster SessionMaster1 = (from tb in _dbContext.SessionMasters
                                                where tb.IsActive && tb.ID == SessionMaster.ID
                                                select tb).FirstOrDefault();
                if (SessionMaster1 != null)
                {
                    _dbContext.SessionMasters.Remove(SessionMaster1);
                    await _dbContext.SaveChangesAsync();
                    SessionMasterResult.ID = SessionMaster.ID;
                    SessionMasterResult.ProjectName = SessionMaster.ProjectName;
                    SessionMasterResult.SessionTimeOut = SessionMaster.SessionTimeOut;
                }
                else
                {
                    return SessionMasterResult;
                }
                //SessionMaster1.IsActive = false;
                //SessionMaster1.ModifiedOn = DateTime.Now;
                //SessionMaster1.ModifiedBy = SessionMaster.ModifiedBy;

            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/DeleteSessionMaster : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return SessionMasterResult;
        }

        #endregion

        #region Role

        public List<RoleWithApp> GetAllRoles()
        {
            try
            {
                List<RoleWithApp> RoleWithAppList = new List<RoleWithApp>();
                List<Role> RoleList = (from tb in _dbContext.Roles
                                       where tb.IsActive
                                       select tb).ToList();
                foreach (Role rol in RoleList)
                {
                    RoleWithAppList.Add(new RoleWithApp()
                    {
                        RoleID = rol.RoleID,
                        RoleName = rol.RoleName,
                        IsActive = rol.IsActive,
                        CreatedOn = rol.CreatedOn,
                        ModifiedOn = rol.ModifiedOn,
                        AppIDList = _dbContext.RoleAppMaps.Where(x => x.RoleID == rol.RoleID && x.IsActive).Select(r => r.AppID).ToArray()
                    });
                }
                return RoleWithAppList;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllRoles : - ", ex);
                return null;

            }
        }

        public async Task<RoleWithApp> CreateRole(RoleWithApp roleWithApp)
        {
            RoleWithApp roleResult = new RoleWithApp();
            try
            {
                Role role1 = (from tb in _dbContext.Roles
                              where tb.IsActive && tb.RoleName == roleWithApp.RoleName
                              select tb).FirstOrDefault();
                if (role1 == null)
                {
                    Role role = new Role();
                    role.RoleID = Guid.NewGuid();
                    role.RoleName = roleWithApp.RoleName;
                    role.CreatedOn = DateTime.Now;
                    role.CreatedBy = roleWithApp.CreatedBy;
                    role.IsActive = true;
                    var result = _dbContext.Roles.Add(role);
                    await _dbContext.SaveChangesAsync();

                    foreach (int AppID in roleWithApp.AppIDList)
                    {
                        RoleAppMap roleApp = new RoleAppMap()
                        {
                            AppID = AppID,
                            RoleID = role.RoleID,
                            IsActive = true,
                            CreatedOn = DateTime.Now
                        };
                        _dbContext.RoleAppMaps.Add(roleApp);
                    }
                    await _dbContext.SaveChangesAsync();
                    roleResult.RoleName = roleWithApp.RoleName;
                    roleResult.RoleID = roleWithApp.RoleID;
                    roleResult.AppIDList = roleWithApp.AppIDList;
                }
                else
                {
                    return roleResult;
                    //return Content(HttpStatusCode.BadRequest, "Role with same name already exist");
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateRole : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return roleResult;
        }

        public async Task<RoleWithApp> UpdateRole(RoleWithApp roleWithApp)
        {
            RoleWithApp roleResult = new RoleWithApp();
            try
            {
                Role role = (from tb in _dbContext.Roles
                             where tb.IsActive && tb.RoleName == roleWithApp.RoleName && tb.RoleID != roleWithApp.RoleID
                             select tb).FirstOrDefault();
                if (role == null)
                {
                    Role role1 = (from tb in _dbContext.Roles
                                  where tb.IsActive && tb.RoleID == roleWithApp.RoleID
                                  select tb).FirstOrDefault();
                    role1.RoleName = roleWithApp.RoleName;
                    role1.IsActive = true;
                    role1.ModifiedOn = DateTime.Now;
                    role1.ModifiedBy = roleWithApp.ModifiedBy;
                    await _dbContext.SaveChangesAsync();

                    List<RoleAppMap> OldRoleAppList = _dbContext.RoleAppMaps.Where(x => x.RoleID == roleWithApp.RoleID && x.IsActive).ToList();
                    List<RoleAppMap> NeedToRemoveRoleAppList = OldRoleAppList.Where(x => !roleWithApp.AppIDList.Any(y => y == x.AppID)).ToList();
                    List<int> NeedToAddAppList = roleWithApp.AppIDList.Where(x => !OldRoleAppList.Any(y => y.AppID == x)).ToList();

                    //Delete Old RoleApps which is not exist in new List
                    NeedToRemoveRoleAppList.ForEach(x =>
                    {
                        _dbContext.RoleAppMaps.Remove(x);
                    });
                    await _dbContext.SaveChangesAsync();

                    //Create New RoleApps
                    foreach (int AppID in NeedToAddAppList)
                    {
                        RoleAppMap roleApp = new RoleAppMap()
                        {
                            AppID = AppID,
                            RoleID = role1.RoleID,
                            IsActive = true,
                            CreatedOn = DateTime.Now,
                        };
                        _dbContext.RoleAppMaps.Add(roleApp);
                    }
                    await _dbContext.SaveChangesAsync();
                    roleResult.RoleName = roleWithApp.RoleName;
                    roleResult.RoleID = roleWithApp.RoleID;
                    roleResult.AppIDList = roleWithApp.AppIDList;
                }
                else
                {
                    return roleResult;
                    //return Content(HttpStatusCode.BadRequest, "Role with same name already exist");
                }

            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateRole : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return roleResult;
        }
        public async Task<BPVendorOnBoardingView> SendMailToApprover(BPVendorOnBoardingView vendor)
        {
            try
            {
                //var result = (from tb1 in _dbContext.Users
                //              join tb2 in _dbContext.UserRoleMaps on tb1.UserID equals tb2.UserID
                //              join tb3 in _dbContext.Roles on tb2.RoleID equals tb3.RoleID
                //              where tb3.RoleName.ToLower() == "approver"
                //              select new
                //              {
                //                  UserId = tb1.UserID,
                //                  Username = tb1.UserName,
                //                  Email = tb1.Email,
                //                  ContactNumber = tb1.ContactNumber
                //              }).FirstOrDefault();
                //if (result != null)
                //{
                //    string BaseAddress = _configuration.GetValue<string>("SiteURL");
                //    await SendMailToApprovalVendor(result.Email, BaseAddress);
                //    return vendor;
                //}
                //return null;
                string BaseAddress = _configuration.GetValue<string>("SiteURL");
                await SendMailToApprovalVendor(vendor.EmamiContactPersonMail, BaseAddress);
                return vendor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> SendMailToApprovalVendor(string toEmail, string siteURL)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> <h2>Wipro Vendor Onboarding</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear concern,</p> <p>New vendor has been registered</p> <div style='text-align: end;'>" + "<a href=\"" + siteURL + "/#/auth/login" + "\"" + "><button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Approve</button></a></div> <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>");
                subject = "New vendor has been registered";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);

                ErrorLog.WriteToFile($"SendMailToApprovalVendor - Approval link has been sent successfully to {toEmail}");
                return true;

            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        ErrorLog.WriteToFile("VendorOnBoardingRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        ErrorLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                ErrorLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message);
                return false;
            }
            catch (SmtpException ex)
            {
                ErrorLog.WriteToFile("VendorOnBoardingRepository/SendMail/SmtpException:- " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("VendorOnBoardingRepository/SendMail/Exception:- " + ex.Message);
                return false;
            }

        }
        public async Task<RoleWithApp> DeleteRole(RoleWithApp roleWithApp)
        {
            RoleWithApp roleResult = new RoleWithApp();
            try
            {
                Role role1 = (from tb in _dbContext.Roles
                              where tb.IsActive && tb.RoleID == roleWithApp.RoleID
                              select tb).FirstOrDefault();
                if (role1 == null)
                {
                    return roleResult;
                }
                else
                {
                    //role1.IsActive = false;
                    //role1.ModifiedOn = DateTime.Now;
                    _dbContext.Roles.Remove(role1);
                    await _dbContext.SaveChangesAsync();

                    //Change the status of the RoleApps related to the role
                    List<RoleAppMap> RoleAppList = _dbContext.RoleAppMaps.Where(x => x.RoleID == roleWithApp.RoleID && x.IsActive).ToList();
                    RoleAppList.ForEach(x =>
                    {
                        //x.IsActive = false;
                        //x.ModifiedOn = DateTime.Now;
                        //x.ModifiedBy = roleWithApp.ModifiedBy;
                        _dbContext.RoleAppMaps.Remove(x);
                    });
                    await _dbContext.SaveChangesAsync();
                    roleResult.RoleName = role1.RoleName;
                    roleResult.RoleID = role1.RoleID;

                    return roleResult;
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/DeleteRole : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region App

        public List<App> GetAllApps()
        {
            try
            {
                var result = (from tb in _dbContext.Apps
                              where tb.IsActive
                              select tb).ToList();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllApps : - ", ex);
                return null;

            }
        }
        public async Task<App> CreateApp(App App)
        {
            App appResult = new App();
            try
            {
                App App1 = (from tb in _dbContext.Apps
                            where tb.IsActive && tb.AppName == App.AppName
                            select tb).FirstOrDefault();
                if (App1 == null)
                {
                    App.CreatedOn = DateTime.Now;
                    App.IsActive = true;
                    var result = _dbContext.Apps.Add(App);
                    await _dbContext.SaveChangesAsync();

                    appResult.AppName = App.AppName;
                    appResult.AppID = App.AppID;
                }
                else
                {
                    return appResult;
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/CreateApp : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return appResult;
        }
        public async Task<App> UpdateApp(App App)
        {
            App appResult = new App();
            try
            {
                App App1 = (from tb in _dbContext.Apps
                            where tb.IsActive && tb.AppName == App.AppName && tb.AppID != App.AppID
                            select tb).FirstOrDefault();
                if (App1 == null)
                {
                    App App2 = (from tb in _dbContext.Apps
                                where tb.IsActive && tb.AppID == App.AppID
                                select tb).FirstOrDefault();
                    App2.AppName = App.AppName;
                    App2.IsActive = true;
                    App2.ModifiedOn = DateTime.Now;
                    App2.ModifiedBy = App.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                    appResult.AppName = App.AppName;
                    appResult.AppID = App.AppID;
                }
                else
                {
                    return appResult;
                }
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/UpdateApp : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return appResult;
        }
        public async Task<App> DeleteApp(App App)
        {
            App appResult = new App();
            try
            {
                App App1 = (from tb in _dbContext.Apps
                            where tb.IsActive && tb.AppID == App.AppID
                            select tb).FirstOrDefault();
                if (App1 != null)
                {
                    _dbContext.Apps.Remove(App1);
                    await _dbContext.SaveChangesAsync();
                    appResult.AppName = App1.AppName;
                    appResult.AppID = App1.AppID;
                }
                else
                {
                    return appResult;
                }
                //App1.IsActive = false;
                //App1.ModifiedOn = DateTime.Now;
                //App1.ModifiedBy = App.ModifiedBy;

            }
            catch (Exception ex)
            {

                ErrorLog.WriteToFile("Master/DeleteApp : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return appResult;
        }

        #endregion

        #region LogInAndChangePassword

        public async Task<UserLoginHistory> LoginHistory(Guid UserID, string Username)
        {
            try
            {
                UserLoginHistory loginData = new UserLoginHistory();
                loginData.UserID = UserID;
                loginData.UserName = Username;
                loginData.LoginTime = DateTime.Now;
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                _dbContext.UserLoginHistory.Add(loginData);
                await _dbContext.SaveChangesAsync();
                return loginData;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/LoginHistory : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
                return null;
            }

        }
        public List<UserLoginHistory> GetAllUsersLoginHistory()
        {
            try
            {
                var UserLoginHistoryList = (from tb1 in _dbContext.UserLoginHistory
                                            orderby tb1.LoginTime descending
                                            select tb1).ToList();

                return UserLoginHistoryList;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetAllUsersLoginHistory : ", ex);
                return null;
            }
        }
        public List<UserLoginHistory> GetCurrentUserLoginHistory(Guid UserID)
        {
            try
            {
                var UserLoginHistoryList = (from tb1 in _dbContext.UserLoginHistory
                                            where tb1.UserID == UserID
                                            orderby tb1.LoginTime descending
                                            select tb1).ToList();
                return UserLoginHistoryList;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/GetCurrentUserLoginHistory : ", ex);
                return null;
            }
        }
        public async Task<UserLoginHistory> SignOut(Guid UserID)
        {
            try
            {
                var result = _dbContext.UserLoginHistory.Where(data => data.UserID == UserID).OrderByDescending(d => d.LoginTime).FirstOrDefault();
                if (result == null)
                {
                    return null;
                }
                var time = DateTime.Now;
                result.LogoutTime = time;
                await _dbContext.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SignOut : - ", ex);
                return null;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        #endregion

        #region ChangePassword
        public async Task<User> ChangePassword(ChangePassword changePassword)
        {
            User userResult = new User();
            try
            {
                User user = (from tb in _dbContext.Users
                             where tb.UserName == changePassword.UserName && tb.IsActive
                             select tb).FirstOrDefault();
                if (user != null)
                {
                    string DecryptedPassword = Decrypt(user.Password, true);
                    if (DecryptedPassword == changePassword.CurrentPassword)
                    {
                        string DefaultPassword = _configuration["DefaultPassword"];
                        if (changePassword.NewPassword == DefaultPassword || user.Pass1 != null && Decrypt(user.Pass1, true) == changePassword.NewPassword || user.Pass2 != null && Decrypt(user.Pass2, true) == changePassword.NewPassword ||
                            user.Pass3 != null && Decrypt(user.Pass3, true) == changePassword.NewPassword || user.Pass4 != null && Decrypt(user.Pass4, true) == changePassword.NewPassword || user.Pass5 != null && Decrypt(user.Pass5, true) == changePassword.NewPassword)
                        {
                            userResult = null;
                            return userResult;
                        }
                        else
                        {
                            var previousPWD = user.Password;
                            user.Password = Encrypt(changePassword.NewPassword, true);
                            var index = user.LastChangedPassword;
                            var lastchangedIndex = 0;

                            //To find lastchangedpassword
                            if (!string.IsNullOrEmpty(index))
                            {
                                if (user.Pass1 != null)
                                {
                                    var strings = "user.Pass1";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 2;

                                    }
                                }
                                if (user.Pass2 != null)
                                {
                                    var strings = "user.Pass2";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 3;

                                    }
                                }
                                if (user.Pass3 != null)
                                {
                                    var strings = "user.Pass3";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 4;

                                    }
                                }
                                if (user.Pass4 != null)
                                {
                                    var strings = "user.Pass4";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 5;

                                    }
                                }
                                if (user.Pass5 != null)
                                {
                                    var strings = "user.Pass5";
                                    if (strings.Contains(index))
                                    {
                                        lastchangedIndex = 1;

                                    }
                                }
                            }

                            if (lastchangedIndex <= 0)
                            {
                                lastchangedIndex = 1;
                            }
                            // TO change previous password
                            if (lastchangedIndex == 1)
                            {
                                user.Pass1 = previousPWD;
                            }
                            else if (lastchangedIndex == 2)
                            {
                                user.Pass2 = previousPWD;
                            }
                            else if (lastchangedIndex == 3)
                            {
                                user.Pass3 = previousPWD;
                            }
                            else if (lastchangedIndex == 4)
                            {
                                user.Pass4 = previousPWD;
                            }
                            else if (lastchangedIndex == 5)
                            {
                                user.Pass5 = previousPWD;
                            }

                            user.LastChangedPassword = lastchangedIndex.ToString();
                            user.IsActive = true;
                            user.ModifiedOn = DateTime.Now;
                            user.ExpiryDate = DateTime.Now.AddDays(90);
                            await _dbContext.SaveChangesAsync();
                            userResult = user;
                        }
                    }
                    else
                    {
                        return userResult;
                    }
                }
                else
                {
                    return userResult;
                    //return Content(HttpStatusCode.BadRequest, "The user name or password is incorrect.");
                }
            }

            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/ChangePassword : - ", ex);
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
            return userResult;
        }

        //public async Task<User> ChangePassword(ChangePassword changePassword)
        //{
        //    User userResult = new User();
        //    try
        //    {
        //        User user = (from tb in _dbContext.Users
        //                     where tb.UserName == changePassword.UserName && tb.IsActive
        //                     select tb).FirstOrDefault();
        //        if (user != null)
        //        {
        //            string DecryptedPassword = Decrypt(user.Password, true);
        //            if (DecryptedPassword == changePassword.CurrentPassword)
        //            {
        //                //string DefaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
        //                string DefaultPassword = "Exalca@123";
        //                if (changePassword.NewPassword == DefaultPassword)
        //                {
        //                    //return Content(HttpStatusCode.BadRequest, "New password should be different from default password.");
        //                    return userResult;
        //                }
        //                else
        //                {
        //                    user.Password = Encrypt(changePassword.NewPassword, true);
        //                    user.IsActive = true;
        //                    user.ModifiedOn = DateTime.Now;
        //                    await _dbContext.SaveChangesAsync();
        //                    userResult = user;
        //                }
        //            }
        //            else
        //            {
        //                //return Content(HttpStatusCode.BadRequest, "Current password is incorrect.");
        //                return userResult;
        //            }
        //        }
        //        else
        //        {
        //            return userResult;
        //            //return Content(HttpStatusCode.BadRequest, "The user name or password is incorrect.");
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ErrorLog.WriteToFile("Master/ChangePassword : - ", ex);
        //        //return Content(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //    return userResult;
        //}

        public async Task<TokenHistory> SendResetLinkToMail(EmailModel emailModel)
        {
            TokenHistory tokenHistoryResult = new TokenHistory();
            try
            {
                DateTime ExpireDateTime = DateTime.Now.AddMinutes(_tokenTimespan);
                User user = (from tb in _dbContext.Users
                             where tb.UserName == emailModel.UserName && tb.IsActive
                             select tb).FirstOrDefault();

                if (user != null)
                {
                    string code = Encrypt(user.UserID.ToString() + '|' + user.UserName + '|' + ExpireDateTime, true);

                    bool sendresult = await SendMail(HttpUtility.UrlEncode(code), user.UserName, user.Email, user.UserID.ToString(), emailModel.siteURL);
                    if (sendresult)
                    {
                        try
                        {
                            TokenHistory history1 = (from tb in _dbContext.TokenHistories
                                                     where tb.UserID == user.UserID && !tb.IsUsed
                                                     select tb).FirstOrDefault();
                            if (history1 == null)
                            {
                                TokenHistory history = new TokenHistory()
                                {
                                    UserID = user.UserID,
                                    Token = code,
                                    EmailAddress = user.Email,
                                    CreatedOn = DateTime.Now,
                                    ExpireOn = ExpireDateTime,
                                    IsUsed = false,
                                    Comment = "Reset Token sent successfully"
                                };
                                var result = _dbContext.TokenHistories.Add(history);
                            }
                            else
                            {
                                ErrorLog.WriteToFile("Master/SendLinkToMail : Token already present, updating new token to the user whose mail id is " +user.Email);
                                history1.Token = code;
                                history1.CreatedOn = DateTime.Now;
                                history1.ExpireOn = ExpireDateTime;
                            }
                            await _dbContext.SaveChangesAsync();

                            tokenHistoryResult = history1;
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.WriteToFile("AuthenticationService-Master/SendLinkToMail : Add record to TokenHistories - ", ex);
                            throw ex;
                        }
                        return tokenHistoryResult;
                        //return Content(HttpStatusCode.OK, string.Format("Reset password link sent successfully to {0}", user.Email));
                    }
                    else
                    {
                        //return tokenHistoryResult;
                        //throw new Exception("Sorry! There is some problem on sending mail");
                        throw new Exception("Something went wrong");
                        //return Content(HttpStatusCode.BadRequest, "Sorry! There is some problem on sending mail");
                    }
                }
                else
                {
                    //tokenHistoryResult=null;
                    //return tokenHistoryResult;
                    throw new Exception($"User name {emailModel.UserName} is not registered!");
                    //throw new Exception("Your email address is not registered!");
                    //return Content(HttpStatusCode.BadRequest, "Your email address is not registered!");
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SendLinkToMail : - ", ex);
                //return null;
                throw ex;
                //return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<TokenHistory> ForgotPassword(ForgotPassword forgotPassword)
        {
            string[] decryptedArray = new string[3];
            string result = string.Empty;
            TokenHistory tokenHistoryResult = new TokenHistory();
            try
            {
                try
                {
                    result = Decrypt(forgotPassword.Token, true);
                }
                catch
                {
                    return tokenHistoryResult;
                    //return Content(HttpStatusCode.BadRequest, "Invalid token!");
                    //var errors = new string[] { "Invalid token!" };
                    //IHttpActionResult errorResult = GetErrorResult(new IdentityResult(errors));
                    //return errorResult;
                }
                if (result.Contains('|') && result.Split('|').Length == 3)
                {
                    decryptedArray = result.Split('|');
                }
                else
                {
                    return tokenHistoryResult;
                    //return Content(HttpStatusCode.BadRequest, "Invalid token!");
                }

                if (decryptedArray.Length == 3)
                {
                    DateTime date = DateTime.Parse(decryptedArray[2].Replace('+', ' '));
                    if (DateTime.Now > date)// Convert.ToDateTime(decryptedarray[2]))
                    {
                        throw new Exception("Token Expired");
                    }
                    var DecryptedUserID = decryptedArray[0];

                    User user = (from tb in _dbContext.Users
                                 where tb.UserID.ToString() == DecryptedUserID && tb.IsActive
                                 select tb).FirstOrDefault();

                    if (user.UserName == decryptedArray[1] && forgotPassword.UserID == user.UserID)
                    {
                        try
                        {
                            string DefaultPassword = _configuration["DefaultPassword"];

                            TokenHistory history = _dbContext.TokenHistories.Where(x => x.UserID == user.UserID && !x.IsUsed && x.Token == forgotPassword.Token).Select(r => r).FirstOrDefault();
                            if (history != null)
                            {
                                var index = user.LastChangedPassword;
                                var lastchangedIndex = 0;
                                var previousPWD = user.Password;
                                if (forgotPassword.NewPassword == DefaultPassword || user.Pass1 != null && Decrypt(user.Pass1, true) == forgotPassword.NewPassword || user.Pass2 != null && Decrypt(user.Pass2, true) == forgotPassword.NewPassword ||
                                    user.Pass3 != null && Decrypt(user.Pass3, true) == forgotPassword.NewPassword || user.Pass4 != null && Decrypt(user.Pass4, true) == forgotPassword.NewPassword || user.Pass5 != null && Decrypt(user.Pass5, true) == forgotPassword.NewPassword)
                                {
                                    return null;
                                }
                                else
                                {
                                    //To find lastchangedpassword
                                    if (!string.IsNullOrEmpty(index))
                                    {
                                        if (user.Pass1 != null)
                                        {
                                            var strings = "user.Pass1";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 2;
                                            }
                                        }
                                        if (user.Pass2 != null)
                                        {
                                            var strings = "user.Pass2";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 3;
                                            }
                                        }
                                        if (user.Pass3 != null)
                                        {
                                            var strings = "user.Pass3";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 4;

                                            }
                                        }
                                        if (user.Pass4 != null)
                                        {
                                            var strings = "user.Pass4";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 5;

                                            }
                                        }
                                        if (user.Pass5 != null)
                                        {
                                            var strings = "user.Pass5";
                                            if (strings.Contains(index))
                                            {
                                                lastchangedIndex = 1;

                                            }
                                        }
                                    }
                                    if (lastchangedIndex <= 0)
                                    {
                                        lastchangedIndex = 1;
                                    }
                                    // TO change previous password
                                    if (lastchangedIndex == 1)
                                    {
                                        user.Pass1 = previousPWD;
                                    }
                                    else if (lastchangedIndex == 2)
                                    {
                                        user.Pass2 = previousPWD;
                                    }
                                    else if (lastchangedIndex == 3)
                                    {
                                        user.Pass3 = previousPWD;
                                    }
                                    else if (lastchangedIndex == 4)
                                    {
                                        user.Pass4 = previousPWD;
                                    }
                                    else if (lastchangedIndex == 5)
                                    {
                                        user.Pass5 = previousPWD;
                                    }

                                    user.LastChangedPassword = lastchangedIndex.ToString();
                                    // Updating Password
                                    user.Password = Encrypt(forgotPassword.NewPassword, true);
                                    user.IsActive = true;
                                    user.ModifiedOn = DateTime.Now;
                                    user.ExpiryDate = DateTime.Now.AddDays(90);
                                    await _dbContext.SaveChangesAsync();

                                    // Updating TokenHistory
                                    history.UsedOn = DateTime.Now;
                                    history.IsUsed = true;
                                    history.Comment = "Token Used successfully";
                                    await _dbContext.SaveChangesAsync();

                                    tokenHistoryResult = history;
                                }
                            }
                            else
                            {
                                throw new Exception("Token Might have Already Used!");
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.WriteToFile("Master/ForgotPassword : Getting TokenHistory - ", ex);
                            throw ex;
                        }

                    }
                    else
                    {
                        throw new Exception("invalid Token");
                        //return Content(HttpStatusCode.BadRequest, "Invalid token!");
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/ForgotPassword : - ", ex);
                throw ex;
            }
            return tokenHistoryResult;
        }

        #endregion

        #region sendMail

        public async Task<bool> SendMail(string code, string UserName, string toEmail, string userID, string siteURL)
        {
            try
            {
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                sb.Append(@"<html><head></head><body> <div style='border:1px solid #dbdbdb;'> <div style='padding: 20px 20px; background-color: #fff06769;text-align: center;font-family: Segoe UI;'> <p> <h2>Wipro Vendor Onboadring</h2> </p> </div> <div style='background-color: #f8f7f7;padding: 20px 20px;font-family: Segoe UI'> <div style='padding: 20px 20px;border:1px solid white;background-color: white !important'> <p>Dear concern,</p> <p>We have received a request to reset your password, you can reset it now by clicking reset button</p> <div style='text-align: end;'>" + "<a href=\"" + siteURL + "?token=" + code + "&Id=" + userID + "\"" + "><button style='width: 90px;height: 28px; background-color: #039be5;color: white'>Reset</button></a></div> <p>Regards,</p> <p>Admin</p> </div> </div> </div></body></html>");
                subject = "Reset password";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = false;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                await client.SendMailAsync(reportEmail);
                ErrorLog.WriteToFile($"AuthenticationService-MasterRepository Reset link has been sent successfully to {UserName} - {toEmail}");
                return true;

            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/MailboxBusy/MailboxUnavailable/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                    else
                    {
                        ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/SmtpFailedRecipientsException:Inner- " + ex.InnerExceptions[i].Message);
                    }
                }
                ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/SmtpFailedRecipientsException:- " + ex.Message, ex);
                return false;
            }
            catch (SmtpException ex)
            {
                ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/SmtpException:- " + ex.Message, ex);
                return false;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("AuthenticationService-MasterRepository/SendMail/Exception:- " + ex.Message, ex);
                return false;
            }
        }

        public async Task<bool> SendMailToVendor(string toEmail, string password)
        {
            try
            {
                //string hostName = ConfigurationManager.AppSettings["HostName"];
                //string SMTPEmail = ConfigurationManager.AppSettings["SMTPEmail"];
                ////string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
                //string SMTPEmailPassword = ConfigurationManager.AppSettings["SMTPEmailPassword"];
                //string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];
                var STMPDetailsConfig = _configuration.GetSection("STMPDetails");
                string hostName = STMPDetailsConfig["Host"];
                string SMTPEmail = STMPDetailsConfig["Email"];
                string siteURL = _configuration["SiteURL"];
                string SMTPEmailPassword = STMPDetailsConfig["Password"];
                string SMTPPort = STMPDetailsConfig["Port"];
                var message = new MailMessage();
                string subject = "";
                StringBuilder sb = new StringBuilder();
                //string UserName = _dbContext.TBL_User_Master.Where(x => x.Email == toEmail).Select(y => y.UserName).FirstOrDefault();
                //UserName = string.IsNullOrEmpty(UserName) ? toEmail.Split('@')[0] : UserName;
                sb.Append(string.Format("Dear {0},<br/>", toEmail));
                sb.Append("<p>Thank you for subscribing to BP Cloud.</p>");
                sb.Append("<p>Please Login by clicking <a href=\"" + siteURL + "/#/auth/login\">here</a></p>");
                sb.Append(string.Format("<p>User name: {0}</p>", toEmail));
                sb.Append(string.Format("<p>Password: {0}</p>", password));
                sb.Append("<p>Regards,</p><p>Admin</p>");
                subject = "BP Cloud Vendor Registration";
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(SMTPPort);
                client.Host = hostName;
                client.EnableSsl = true;
                client.Timeout = 60000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(SMTPEmail, SMTPEmailPassword);
                MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
                reportEmail.BodyEncoding = UTF8Encoding.UTF8;
                reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                reportEmail.IsBodyHtml = true;
                await client.SendMailAsync(reportEmail);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToFile("Master/SendMail : - ", ex);
                throw ex;
            }
        }

        #endregion


        #region EncryptAndDecrypt

        public string Decrypt(string Password, bool UseHashing)
        {
            string EncryptionKey = "Exalca";
            byte[] KeyArray;
            byte[] ToEncryptArray = Convert.FromBase64String(Password);
            if (UseHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                hashmd5.Clear();
            }
            else
            {
                KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = KeyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 ToEncryptArray, 0, ToEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public string Encrypt(string Password, bool useHashing)
        {
            //string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
            string EncryptionKey = "Exalca";
            byte[] KeyArray;
            byte[] ToEncryptArray = UTF8Encoding.UTF8.GetBytes(Password);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
                hashmd5.Clear();
            }
            else
                KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = KeyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray =
              cTransform.TransformFinalBlock(ToEncryptArray, 0,
              ToEncryptArray.Length);

            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        #endregion

    }
}
