using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Concurrent;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AuthenticationService.Controllers;
using System.Reflection.PortableExecutable;

namespace AuthenticationService.Providers
{
    //public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    //{
    //    AuthContext _ctx;
    //    public SimpleAuthorizationServerProvider(AuthContext authContext)
    //    {
    //        _ctx = authContext;
    //    }

    //    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    //    {
    //        try
    //        {
    //            //_ctx = new AuthContext();
    //            string clientId = string.Empty;
    //            string clientSecret = string.Empty;
    //            Client client = null;

    //            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
    //            {
    //                context.TryGetFormCredentials(out clientId, out clientSecret);
    //            }

    //            if (context.ClientId == null)
    //            {
    //                context.Validated();
    //                return Task.FromResult<object>(null);
    //            }

    //            using (AuthRepository _repo = new AuthRepository())
    //            {
    //                client = _repo.FindClient(context.ClientId);
    //            }

    //            if (client == null)
    //            {
    //                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
    //                return Task.FromResult<object>(null);
    //            }

    //            if (client.ApplicationType == Models.ApplicationTypes.NativeConfidential)
    //            {
    //                if (string.IsNullOrWhiteSpace(clientSecret))
    //                {
    //                    context.SetError("invalid_clientId", "Client secret should be sent.");
    //                    return Task.FromResult<object>(null);
    //                }
    //                else
    //                {
    //                    if (client.Secret != Helper.GetHash(clientSecret))
    //                    {
    //                        context.SetError("invalid_clientId", "Client secret is invalid.");
    //                        return Task.FromResult<object>(null);
    //                    }
    //                }
    //            }

    //            if (!client.Active)
    //            {
    //                context.SetError("invalid_clientId", "Client is inactive.");
    //                return Task.FromResult<object>(null);
    //            }

    //            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
    //            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

    //            context.Validated();
    //            return Task.FromResult<object>(null);
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLog.WriteToFile("AuthorizationServerProvider/ValidateClientAuthentication :- " ,ex);
    //            return Task.FromResult<object>(null);
    //        }

    //    }

    //    public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
    //    {
    //        try
    //        {
    //            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
    //            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

    //            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
    //            context.Validated(newTicket);

    //            return Task.FromResult<object>(null);
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLog.WriteToFile("AuthorizationServerProvider/GrantRefreshToken :- " ,ex);
    //            return Task.FromResult<object>(null);
    //        }
    //    }

    //    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    //    {
    //        context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
    //        try
    //        {
    //            string userId = String.Empty, role = String.Empty;
    //            //string IsLDAP = ConfigurationManager.AppSettings["IsLDAP"].ToString();
    //            string IsLDAP = "N";
    //            List<string> MenuItemList = new List<string>();
    //            string MenuItemNames = "";
    //            string ProfilesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");
    //            string Profile = "Empty";
    //            User user = null;
    //            string isChangePasswordRequired = "No";
    //            //string DefaultPassword = ConfigurationManager.AppSettings["DefaultPassword"];
    //            string DefaultPassword = "Exalca@123";

    //            //Admin admin = _ctx.Admin.AsNoTracking().Where(data => data.Username == context.UserName).SingleOrDefault();
    //            //if (admin != null)
    //            //{
    //            //    if (admin.Password == context.Password)
    //            //    {
    //            //        var props = new AuthenticationProperties(new Dictionary<string, string>
    //            //            {
    //            //                {
    //            //                    "userID",admin.Username.ToString()
    //            //                },
    //            //                {
    //            //                    "userName", admin.Username
    //            //                },
    //            //                {
    //            //                    "displayName", admin.Username
    //            //                },
    //            //                {
    //            //                    "userRole", "SuperAdmin"
    //            //                },

    //            //            });
    //            //        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
    //            //        var ticket = new AuthenticationTicket(identity, props);
    //            //        context.Validated(ticket);
    //            //    }
    //            //    else
    //            //    {
    //            //        context.SetError("invalid_grant", "The password is incorrect");
    //            //    }
    //            //}
    //            //else
    //            //{
    //            if (context.UserName.Contains('@') && context.UserName.Contains('.'))
    //            {
    //                user = (from tb in _ctx.Users
    //                        where tb.Email == context.UserName && tb.IsActive
    //                        select tb).FirstOrDefault();
    //            }
    //            else
    //            {
    //                user = (from tb in _ctx.Users
    //                        where tb.UserName == context.UserName && tb.IsActive
    //                        select tb).FirstOrDefault();
    //            }

    //            if (user != null)
    //            {
    //                bool isValidUser = false;

    //                if (IsLDAP.ToUpper() == "S")
    //                {
    //                    LdapUser ldapUser = GetAuthenticatedLdapUser(context.UserName, context.Password);
    //                    isValidUser = ldapUser != null ? true : false;
    //                    if (isValidUser)
    //                    {
    //                        //MasterController MasterController = new MasterController();
    //                        //await MasterController.LoginHistory(user.UserID, user.UserName);
    //                        Role userRole = (from tb1 in _ctx.Roles
    //                                         join tb2 in _ctx.UserRoleMaps on tb1.RoleID equals tb2.RoleID
    //                                         where tb2.UserID == user.UserID && tb1.IsActive && tb2.IsActive
    //                                         select tb1).FirstOrDefault();
    //                        if (userRole != null)
    //                        {
    //                            MenuItemList = (from tb1 in _ctx.Apps
    //                                            join tb2 in _ctx.RoleAppMaps on tb1.AppID equals tb2.AppID
    //                                            where tb2.RoleID == userRole.RoleID && tb1.IsActive && tb2.IsActive
    //                                            select tb1.AppName).ToList();

    //                            foreach (string item in MenuItemList)
    //                            {
    //                                if (MenuItemNames == "")
    //                                {
    //                                    MenuItemNames = item;
    //                                }
    //                                else
    //                                {
    //                                    MenuItemNames += "," + item;
    //                                }
    //                            }
    //                        }
    //                        // Profile
    //                        //try
    //                        //{
    //                        //    CreateProfilesFolder();
    //                        //    UserAttachment Attachment = _ctx.UserAttachment.Where(x => x.UserID == user.UserID.ToString()).Select(r => r).FirstOrDefault();
    //                        //    if (Attachment != null)
    //                        //    {
    //                        //        string ProFilePath = ProfilesFolder + "\\" + user.UserID.ToString() + Attachment.AttachmentExtension;
    //                        //        File.WriteAllBytes(ProFilePath, Attachment.AttachmentFile);
    //                        //        var VirtualPath = GetVirtualPath(ProFilePath);

    //                        //        if (!string.IsNullOrEmpty(VirtualPath))
    //                        //        {
    //                        //            Uri navigateUri = new Uri(System.Web.HttpContext.Current.Request.Url, VirtualPath);
    //                        //            Profile = navigateUri.AbsoluteUri;
    //                        //        }
    //                        //    }
    //                        //}
    //                        //catch (Exception ex)
    //                        //{
    //                        //    ErrorLog.WriteToFile("Account/GrantResourceOwnerCredentials : - " ,ex);
    //                        //}

    //                        var props = new AuthenticationProperties(new Dictionary<string, string>
    //                            {
    //                                {
    //                                    "userID",user.UserID.ToString()
    //                                },
    //                                {
    //                                    "userName", ldapUser.UserName
    //                                },
    //                                {
    //                                    "displayName", ldapUser.DisplayName
    //                                },
    //                                {
    //                                    "emailAddress",ldapUser.Email
    //                                },
    //                                {
    //                                    "userRole", userRole!=null?userRole.RoleName:null
    //                                },
    //                                {
    //                                     "menuItemNames",MenuItemNames
    //                                },
    //                                {
    //                                     "isChangePasswordRequired",isChangePasswordRequired
    //                                },
    //                                {
    //                                     "profile",string.IsNullOrEmpty(Profile)?"Empty":Profile
    //                                },

    //                           });
    //                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
    //                        var ticket = new AuthenticationTicket(identity, props);
    //                        context.Validated(ticket);
    //                    }
    //                    else
    //                    {
    //                        context.SetError("invalid_grant", "The user name or password is incorrect.");
    //                    }
    //                }
    //                else
    //                {
    //                    string DecryptedPassword = Decrypt(user.Password, true);
    //                    isValidUser = DecryptedPassword == context.Password;
    //                    if (isValidUser)
    //                    {
    //                        GC.Collect();
    //                        GC.WaitForPendingFinalizers();
    //                        if (context.Password == DefaultPassword)
    //                        {
    //                            isChangePasswordRequired = "Yes";
    //                        }
    //                        //MasterController MasterController = new MasterController();
    //                        //await MasterController.LoginHistory(user.UserID, user.UserName);
    //                        Role userRole = (from tb1 in _ctx.Roles
    //                                         join tb2 in _ctx.UserRoleMaps on tb1.RoleID equals tb2.RoleID
    //                                         join tb3 in _ctx.Users on tb2.UserID equals tb3.UserID
    //                                         where tb3.UserID == user.UserID && tb1.IsActive && tb2.IsActive && tb3.IsActive
    //                                         select tb1).FirstOrDefault();

    //                        if (userRole != null)
    //                        {
    //                            MenuItemList = (from tb1 in _ctx.Apps
    //                                            join tb2 in _ctx.RoleAppMaps on tb1.AppID equals tb2.AppID
    //                                            where tb2.RoleID == userRole.RoleID && tb1.IsActive && tb2.IsActive
    //                                            select tb1.AppName).ToList();
    //                            foreach (string item in MenuItemList)
    //                            {
    //                                if (MenuItemNames == "")
    //                                {
    //                                    MenuItemNames = item;
    //                                }
    //                                else
    //                                {
    //                                    MenuItemNames += "," + item;
    //                                }
    //                            }
    //                        }

    //                        // Profile
    //                        //try
    //                        //{
    //                        //    CreateProfilesFolder();
    //                        //    UserAttachment Attachment = _ctx.UserAttachment.Where(x => x.UserID == user.UserID.ToString()).Select(r => r).FirstOrDefault();
    //                        //    if (Attachment != null)
    //                        //    {
    //                        //        string ProFilePath = ProfilesFolder + "\\" + user.UserID.ToString() + Attachment.AttachmentExtension;
    //                        //        File.WriteAllBytes(ProFilePath, Attachment.AttachmentFile);
    //                        //        var VirtualPath = GetVirtualPath(ProFilePath);

    //                        //        if (!string.IsNullOrEmpty(VirtualPath))
    //                        //        {
    //                        //            Uri navigateUri = new Uri(System.Web.HttpContext.Current.Request.Url, VirtualPath);
    //                        //            Profile = navigateUri.AbsoluteUri;
    //                        //        }
    //                        //    }
    //                        //}
    //                        //catch (Exception ex)
    //                        //{
    //                        //    ErrorLog.WriteToFile("Account/GrantResourceOwnerCredentials : - " ,ex);
    //                        //}
    //                        var props = new AuthenticationProperties(new Dictionary<string, string>
    //                                {
    //                                    {
    //                                        "userID",user.UserID.ToString()
    //                                    },
    //                                    {
    //                                        "userName", user.UserName
    //                                    },
    //                                    {
    //                                        "displayName", user.UserName
    //                                    },
    //                                    {
    //                                        "emailAddress",user.Email
    //                                    },
    //                                    {
    //                                        "userRole", userRole!=null?userRole.RoleName:null
    //                                    },
    //                                    {
    //                                        "menuItemNames",MenuItemNames
    //                                    },
    //                                    {
    //                                     "isChangePasswordRequired",isChangePasswordRequired
    //                                    },
    //                                    {
    //                                        "profile",string.IsNullOrEmpty(Profile)?"Empty":Profile
    //                                    },
    //                                });
    //                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
    //                        var ticket = new AuthenticationTicket(identity, props);
    //                        context.Validated(ticket);
    //                    }
    //                    else
    //                    {
    //                        context.SetError("invalid_grant", "The user name or password is incorrect.");
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                context.SetError("invalid_grant", "The user name or password is incorrect.");
    //            }
    //            //}
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLog.WriteToFile("AuthorizationServerProvider/GrantResourceOwnerCredentials :- " ,ex);
    //        }
    //    }

    //    //public string CreateProfilesFolder()
    //    //{
    //    //    try
    //    //    {
    //    //        string ProfilesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");
    //    //        if (!Directory.Exists(ProfilesFolder))
    //    //        {
    //    //            Directory.CreateDirectory(ProfilesFolder);
    //    //        }
    //    //        return ProfilesFolder;
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        ErrorLog.WriteToFile("Account/CreateProfilesFolder : - " ,ex);
    //    //        return null;
    //    //    }

    //    //}


    //    //public string GetVirtualPath(string physicalPath)
    //    //{
    //    //    try
    //    //    {
    //    //        if (!physicalPath.StartsWith(HttpContext.Current.Request.PhysicalApplicationPath))
    //    //        {
    //    //            return null;
    //    //        }

    //    //        //return "~/" + physicalPath.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length)
    //    //        //      .Replace("\\", "/");
    //    //        return physicalPath.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length)
    //    //             .Replace("\\", "/");
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        ErrorLog.WriteToFile("AuthorizationServerProvider/GetVirtualPath :- " ,ex);
    //    //        return null;
    //    //    }

    //    //}

    //    public override Task TokenEndpoint(OAuthTokenEndpointContext context)
    //    {
    //        foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
    //        {
    //            context.AdditionalResponseParameters.Add(property.Key, property.Value);
    //        }

    //        return Task.FromResult<object>(null);
    //    }

    //    public LdapUser GetAuthenticatedLdapUser(string userName, string password)
    //    {
    //        //string domain = ConfigurationManager.AppSettings["DomainName"].ToString();
    //        string domain = "exalca";
    //        string domainAndUsername = string.Empty;
    //        string SAMAccountName = string.Empty;
    //        if (userName.Contains(@"\"))
    //        {
    //            domainAndUsername = userName;
    //            SAMAccountName = Regex.Split(domainAndUsername, @"\")[1];
    //        }
    //        else
    //        {
    //            domainAndUsername = domain + @"\" + userName;
    //            SAMAccountName = userName;
    //        }

    //        //var _path = ConfigurationManager.AppSettings["LDAPConnectionString"].ToString();
    //        var _path = "LDAP://exalca.corp";
    //        System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(_path, domainAndUsername, password);
    //        System.DirectoryServices.DirectoryEntry dirEntry = new System.DirectoryServices.DirectoryEntry();
    //        LdapUser user = new LdapUser();

    //        try
    //        {
    //            // Bind to the native AdsObject to force authentication.
    //            Object obj = entry.NativeObject;
    //            DirectorySearcher search = new DirectorySearcher(entry);
    //            search.Filter = "(&(objectClass=user)(SAMAccountName=" + SAMAccountName + "))";
    //            SearchResult result = search.FindOne();
    //            if (result == null)
    //            {
    //                return null;
    //            }
    //            else
    //            {
    //                dirEntry = result.GetDirectoryEntry();
    //                user.UserID = dirEntry.Guid;
    //                user.Path = result.Path;
    //                user.Email = GetProperty(result, "mail");
    //                user.UserName = GetProperty(result, "samaccountname");
    //                user.DisplayName = GetProperty(result, "displayname");
    //                user.FirstName = GetProperty(result, "givenName");
    //                user.LastName = GetProperty(result, "sn");
    //                user.Mobile = GetProperty(result, "mobile");
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLog.WriteToFile("AuthorizationServerProvider/GetAuthenticatedLdapUser :- " ,ex);
    //            return null;
    //        }
    //        finally
    //        {
    //            entry.Dispose();
    //            dirEntry.Dispose();
    //        }
    //        return user;
    //    }

    //    public string GetProperty(SearchResult searchResult, string PropertyName)
    //    {
    //        return searchResult.Properties.Contains(PropertyName) ? searchResult.Properties[PropertyName][0].ToString() : null;
    //    }

    //    #region EncryptAndDecrypt
    //    public string Decrypt(string Password, bool UseHashing)
    //    {
    //        try
    //        {
    //            //string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
    //            string EncryptionKey = "Exalca";
    //            byte[] KeyArray;
    //            byte[] ToEncryptArray = Convert.FromBase64String(Password);
    //            if (UseHashing)
    //            {
    //                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
    //                KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
    //                hashmd5.Clear();
    //            }
    //            else
    //            {
    //                KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);
    //            }

    //            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
    //            tdes.Key = KeyArray;
    //            tdes.Mode = CipherMode.ECB;
    //            tdes.Padding = PaddingMode.PKCS7;
    //            ICryptoTransform cTransform = tdes.CreateDecryptor();
    //            byte[] resultArray = cTransform.TransformFinalBlock(
    //                                 ToEncryptArray, 0, ToEncryptArray.Length);
    //            tdes.Clear();
    //            return UTF8Encoding.UTF8.GetString(resultArray);
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLog.WriteToFile("AuthorizationServerProvider/Decrypt :- " ,ex);
    //            return null;
    //        }

    //    }

    //    public string Encrypt(string Password, bool useHashing)
    //    {
    //        try
    //        {
    //            //string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
    //            string EncryptionKey = "Exalca";
    //            byte[] KeyArray;
    //            byte[] ToEncryptArray = UTF8Encoding.UTF8.GetBytes(Password);
    //            if (useHashing)
    //            {
    //                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
    //                KeyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(EncryptionKey));
    //                hashmd5.Clear();
    //            }
    //            else
    //                KeyArray = UTF8Encoding.UTF8.GetBytes(EncryptionKey);

    //            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
    //            tdes.Key = KeyArray;
    //            tdes.Mode = CipherMode.ECB;
    //            tdes.Padding = PaddingMode.PKCS7;
    //            ICryptoTransform cTransform = tdes.CreateEncryptor();
    //            byte[] resultArray = cTransform.TransformFinalBlock(ToEncryptArray, 0,
    //              ToEncryptArray.Length);
    //            tdes.Clear();
    //            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    //        }
    //        catch (Exception ex)
    //        {
    //            ErrorLog.WriteToFile("AuthorizationServerProvider/Encrypt :- " ,ex);
    //            return null;
    //        }
    //    }

    //    #endregion
    //}
    //public class RefreshTokenProvider : IAuthenticationTokenProvider
    //{
    //    private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

    //    public async Task CreateAsync(AuthenticationTokenCreateContext context)
    //    {

    //        var guid = Guid.NewGuid().ToString();

    //        // copy all properties and set the desired lifetime of refresh token  
    //        var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
    //        {
    //            IssuedUtc = context.Ticket.Properties.IssuedUtc,
    //            ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
    //        };

    //        var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

    //        _refreshTokens.TryAdd(guid, refreshTokenTicket);

    //        // consider storing only the hash of the handle  
    //        context.SetToken(guid);
    //    }


    //    public void Create(AuthenticationTokenCreateContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Receive(AuthenticationTokenReceiveContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
    //    {
    //        // context.DeserializeTicket(context.Token);
    //        AuthenticationTicket ticket;
    //        string header = context.OwinContext.Request.Headers["Authorization"];

    //        if (_refreshTokens.TryRemove(context.Token, out ticket))
    //        {
    //            context.SetTicket(ticket);
    //        }
    //    }
    //}
}