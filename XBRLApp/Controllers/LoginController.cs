using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using XBRLApp.Parser;

namespace XBRLApp.Controllers
{
    public class LoginController : Controller
    {
    private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
    WebServiceInvoker invoker;

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    #region Action
    protected void Button_Login_Click(object sender, EventArgs e)
    {
        try
        {
            Label_Error.Visible = false;
            Label_Error.Text = "";

            bool isLoginToLDAP = Convert.ToBoolean(new MsXbrlParameterDAL().GetXbrlParameterValueByParameterName("LoginToLDAP"));

            MsUser oUser1 = new MsUserDAL().GetUserByUserId(TextBox_Username.Text);
            if (oUser1 != null)
            {
                //berarti adalah user lokal dan superuser
                BlowFish bf = new BlowFish(oUser1.PasswordSalt);
                string password = TextBox_Password.Text;

                if (password != bf.Decrypt_CBC(oUser1.Password))
                {
                    WrongPassword += 1;
                    Label_Error.Visible = true;
                    Label_Error.Text = "Password type not match. Please insert correct password.";
                    TextBox_Password.Focus();
                    AuditTrailDAL.AuditTrailPageAccessInsert(oUser1.UserId, oUser1.UserName, "Sign in for user '" + TextBox_Username.Text + "' failed");
                    return;
                }


                //cek apakah user isDisabled apa engga
                if (oUser1.IsDisabled == true)
                {
                    Label_Error.Visible = true;
                    Label_Error.Text = "Your user id is disabled. Please contact your administrator to enabled it.";
                    TextBox_Username.Text = "";
                    TextBox_Username.Focus();
                    AuditTrailDAL.AuditTrailPageAccessInsert(oUser1.UserId, oUser1.UserName, "Sign in for user '" + TextBox_Username.Text + "' failed");
                    return;
                }

                SetSessionUser(oUser1);
                UpdateSuccessfullLoginUser(oUser1);
                AuditTrailDAL.AuditTrailPageAccessInsert(Common.SessionUserID, Common.SessionUserName,
                                                         "Sign in successfully");
                Response.Redirect("~/Member/Default.aspx");
            }
            else
            {
                if (isLoginToLDAP)
                {
                    //jika true, maka harus pakai webservice invoker nya
                    string webserviceURI = new WebServicesLdapDAL().GetParameterValueByParameterName("WebservicesLink");
                    string webserviceMethod = new WebServicesLdapDAL().GetParameterValueByParameterName("WebservicesMethod");
                    bool requiredEncryption = Convert.ToBoolean(
                        new WebServicesLdapDAL().GetParameterValueByParameterName("WebservicesEncryptionRequired"));
                    string domainName = new WebServicesLdapDAL().GetParameterValueByParameterName("UserDomainName");

                    invoker = new WebServiceInvoker(new Uri(webserviceURI));

                    //ambil available service yang ada di webservices
                    List<string> listAvailableServices = invoker.AvailableServices;
                    string serviceName = listAvailableServices[0];  //ambil selalu yang ke 0

                    //ambil available methode yang ada di webservices
                    List<string> listMethods = invoker.EnumerateServiceMethods(serviceName);

                    //cek method yang ada di parameter dengan hasil invoker,
                    //jika ga match, maka langsung error
                    if (!listMethods.Contains(webserviceMethod))
                    {
                        SetErrorMsg("Webservice Method Parameter does not match query result value");
                    }
                    else
                    {
                        //start to invoke the method
                        if (requiredEncryption)
                        {
                            string[] args = new string[] { };
                            string encryptRSAKey = invoker.InvokeMethod<string>(serviceName, "GetRSAPublicKey", args);
                            RSACryptoServiceProvider x = new RSACryptoServiceProvider();
                            x.FromXmlString(encryptRSAKey);
                            byte[] bPassword = x.Encrypt(System.Text.Encoding.Default.GetBytes(TextBox_Password.Text), false);
                            string password = Convert.ToBase64String(bPassword);

                            //cek existing user
                            args = new string[] { domainName, TextBox_Username.Text, TextBox_Password.Text };
                            string xmlUser = invoker.InvokeMethod<string>(serviceName, "GetAllADDomainUsers", args);
                            //if (new LDAPLoginParser().IsUserRegisteredToLDAP(xmlUser, TextBox_Username.Text))
                            //{
                            //cek user group setelah cek user existingnya
                            //cek group usernya, sudah ke register di localdb belum untuk security privilege nya
                            args = new string[] { domainName, TextBox_Username.Text, password };

                            string xmlGrpUser = invoker.InvokeMethod<string>(serviceName, webserviceMethod, args);
                            string groupUser = new LDAPLoginParser().ExtractGroupUser(xmlGrpUser);

                            //cek group user terhadap local tablenya, jika terdaftar ya terus, jika tidak yah harus keluar error msg
                            MsGroupApproval grpLocal = new MsGroupApprovalDAL().GetGroupApprovalName(groupUser);
                            if (grpLocal == null)
                            {
                                SetErrorMsg(
                                    "Group User not exist in database. <br/>Please contact Administrator to register it.");
                            }
                            else
                            {
                                //masuk ke aplikasi
                                SetSessionUser(grpLocal, TextBox_Username.Text);
                                //UpdateSuccessfullLoginUser(oUser, ent);
                                AuditTrailDAL.AuditTrailPageAccessInsert(Common.SessionUserID,
                                                                         Common.SessionUserName,
                                                                         "Sign in successfully");
                                Response.Redirect("~/Member/Default.aspx");
                            }
                            //}
                            //else
                            //{
                            //    SetErrorMsg("User '" + TextBox_Username.Text + "' not found or invalid.");
                            //}
                        }
                        else
                        {
                            //jika tidak butuh encryptkey
                            string[] args = new string[] { };
                            //string encryptRSAKey = invoker.InvokeMethod<string>(serviceName, "GetRSAPublicKey", args);
                            //RSACryptoServiceProvider x = new RSACryptoServiceProvider();
                            //x.FromXmlString(encryptRSAKey);
                            //byte[] bPassword = x.Encrypt(System.Text.Encoding.Default.GetBytes(TextBox_Password.Text), false);
                            string password = TextBox_Password.Text;

                            //cek existing user
                            args = new string[] { domainName, TextBox_Username.Text, TextBox_Password.Text };
                            string xmlUser = invoker.InvokeMethod<string>(serviceName, "GetAllADDomainUsers", args);
                            //if (new LDAPLoginParser().IsUserRegisteredToLDAP(xmlUser, TextBox_Username.Text))
                            //{
                            //cek user group setelah cek user existingnya
                            //cek group usernya, sudah ke register di localdb belum untuk security privilege nya
                            args = new string[] { domainName, TextBox_Username.Text, password };

                            string xmlGrpUser = invoker.InvokeMethod<string>(serviceName, webserviceMethod, args);
                            string groupUser = new LDAPLoginParser().ExtractGroupUser(xmlGrpUser);

                            //cek group user terhadap local tablenya, jika terdaftar ya terus, jika tidak yah harus keluar error msg
                            MsGroupApproval grpLocal = new MsGroupApprovalDAL().GetGroupApprovalName(groupUser);
                            if (grpLocal == null)
                            {
                                SetErrorMsg(
                                    "Group User not exist in database. <br/>Please contact Administrator to register it.");
                            }
                            else
                            {
                                //masuk ke aplikasi
                                SetSessionUser(grpLocal, TextBox_Username.Text);
                                //UpdateSuccessfullLoginUser(oUser, ent);
                                AuditTrailDAL.AuditTrailPageAccessInsert(Common.SessionUserID, Common.SessionUserName,
                                                                         "Sign in successfully");
                                Response.Redirect("~/Member/Default.aspx");
                            }
                            //}
                            //else
                            //{
                            //    SetErrorMsg("User '" + TextBox_Username.Text + "' not found or invalid.");
                            //}
                        }
                    }
                }
                else
                {
                    //login dengan user local
                    using (XbrlEbizEntities ent = new XbrlEbizEntities())
                    {
                        List<MsXbrlParameter> lLoginParam = new MsXbrlParameterDAL().GetAllLoginParameter(ent);

                        int pwdExpired = new MsXbrlParameterDAL().GetLoginSingleParameterFromList(lLoginParam, sEnumLoginParam.sPasswordExpired);
                        int maxFalseType = new MsXbrlParameterDAL().GetLoginSingleParameterFromList(lLoginParam, sEnumLoginParam.sLockUserAfterNWrongPassword);
                        int maxNeverLoginDays = new MsXbrlParameterDAL().GetLoginSingleParameterFromList(lLoginParam, sEnumLoginParam.sLockUserAfterNDays);

                        //start query ke msuser
                        string userid = TextBox_Username.Text.Trim();
                        string password = TextBox_Password.Text.Trim();

                        MsUser oUser = new MsUserDAL().GetUserByUserId(ent, userid);
                        if (oUser == null)
                        {
                            //cara lama tanpa LDAP
                            Label_Error.Visible = true;
                            Label_Error.Text = "User Id: " + userid + " not found.";
                        }
                        else
                        {
                            //cek apakah superuser apa enggak
                            //jika super user, maka login local user yang superuser
                            //cek kesesuaian password dulu
                            BlowFish bf = new BlowFish(oUser.PasswordSalt);
                            if (WrongPassword > maxFalseType)
                            {
                                Label_Error.Visible = true;
                                Label_Error.Text = "You entered wrong password more than " + maxFalseType.ToString() + " times. Your user id will be disabled";

                                oUser.IsDisabled = true;
                                oUser.LastUpdateBy = "System";
                                oUser.LastUpdateDate = DateTime.Now;

                                AuditTrailDAL.AuditTrailPageAccessInsert(oUser.UserId, oUser.UserName, "Sign in failed");

                                ent.SaveChanges();

                                return;
                            }
                            else
                            {
                                if (password != bf.Decrypt_CBC(oUser.Password))
                                {
                                    WrongPassword += 1;
                                    Label_Error.Visible = true;
                                    Label_Error.Text = "Password type not match. Please insert correct password.";
                                    TextBox_Password.Focus();
                                    AuditTrailDAL.AuditTrailPageAccessInsert(oUser.UserId, oUser.UserName, "Sign in failed");
                                    return;
                                }
                            }


                            //cek apakah user isDisabled apa engga
                            if (oUser.IsDisabled == true)
                            {
                                Label_Error.Visible = true;
                                Label_Error.Text = "Your user id is disabled. Please contact your administrator to enabled it.";
                                TextBox_Username.Text = "";
                                TextBox_Username.Focus();
                                AuditTrailDAL.AuditTrailPageAccessInsert(oUser.UserId, oUser.UserName, "Sign in failed");
                                return;
                            }

                            //cek apakah user sudah terpakai
                            if (oUser.CheckerMakerLevel != (byte)CheckerMaker.Superuser)
                            {
                                if (oUser.IsInUsed == true)
                                {
                                    //jika sudah terpakai tapi masih dari ip address yang sama, harus bisa login!!!
                                    if (oUser.UsedInIPAddress != GetUserIdAddress())
                                    {
                                        Label_Error.Visible = true;
                                        Label_Error.Text = "User Id: " + userid + " is in used at IP Address: " + oUser.UsedInIPAddress;
                                        TextBox_Username.Text = "";
                                        TextBox_Username.Focus();
                                        AuditTrailDAL.AuditTrailPageAccessInsert(oUser.UserId, oUser.UserName, "Sign in failed");
                                        return;
                                    }
                                }
                            }

                            //cek apakah user sudah lama tidak login
                            //jika tidak pernah login sebelumnya, maka langsung bisa masuk
                            if (oUser.LastLogin != null)
                            {
                                //cek kapan user terakhir login
                                int logDays = ((TimeSpan)(DateTime.Now - oUser.LastLogin)).Days;
                                if (logDays > maxNeverLoginDays)
                                {
                                    Label_Error.Visible = true;
                                    Label_Error.Text = "Your user id will be disabled because your id not log in more than " + maxNeverLoginDays.ToString() + " days.";

                                    oUser.IsDisabled = true;
                                    oUser.LastUpdateBy = "System";
                                    oUser.LastUpdateDate = DateTime.Now;

                                    AuditTrailDAL.AuditTrailPageAccessInsert(oUser.UserId, oUser.UserName, "Sign in failed");
                                    ent.SaveChanges();
                                }
                            }
                            //else
                            //{
                            //    SetSessionUser(oUser);
                            //    Response.Redirect("ForceChangePassword.aspx?UserId=" + userid);
                            //}

                            //cek last change password
                            //jika tidak pernah change password sama sekali, maka harus di force change password
                            //jika superuser role, maka tidak perlu hal ini
                            if (oUser.CheckerMakerLevel != (byte)CheckerMaker.Superuser)
                            {
                                if (oUser.LastChangePassword == null)
                                {
                                    SetSessionUser(oUser);
                                    //Response.Redirect("ForceChangePassword.aspx?UserId=" + userid);
                                    Response.Redirect("ForceChangePassword.aspx?UserId=" + userid);
                                }
                                else
                                {
                                    //cek apakah password sudah expired, jika iya maka masuk force change password juga
                                    int expDays = ((TimeSpan)(DateTime.Now - oUser.LastChangePassword)).Days;
                                    if (expDays > pwdExpired)
                                    {
                                        SetSessionUser(oUser);
                                        Response.Redirect("ForceChangePassword.aspx?UserId=" + userid);
                                    }
                                    else
                                    {
                                        SetSessionUser(oUser);
                                        UpdateSuccessfullLoginUser(oUser, ent);
                                        AuditTrailDAL.AuditTrailPageAccessInsert(Common.SessionUserID, Common.SessionUserName,
                                                                                 "Sign in successfully");
                                        Response.Redirect("~/Member/Default.aspx");
                                    }
                                }
                            }
                            else
                            {
                                //role superuser langsung melakukan login
                                SetSessionUser(oUser);
                                UpdateSuccessfullLoginUser(oUser, ent);
                                AuditTrailDAL.AuditTrailPageAccessInsert(Common.SessionUserID, Common.SessionUserName,
                                                                         "Sign in successfully");
                                Response.Redirect("~/Member/Default.aspx");
                            }
                        }
                    }
                }
            }
        }
        catch (ThreadAbortException ex1)
        {

        }
        catch (Exception ex)
        {
            logger.Error(ex);
            Label_Error.Visible = true;
            Label_Error.Text = "<div class='alert alert-error'>The system could not log you on. <br/>Username and Password are not correct.</div>";
        }
    }
    #endregion Action

    #region Additional Methods
    private int WrongPassword
    {
        get
        {
            if (ViewState["Login.WrongPassword"] == null) ViewState["Login.WrongPassword"] = 0;
            return (int)ViewState["Login.WrongPassword"];
        }
        set { ViewState["Login.WrongPassword"] = value; }
    }

    private void UpdateSuccessfullLoginUser(MsUser oUser, XbrlEbizEntities ent)
    {
        oUser.IsInUsed = true;
        oUser.IsDisabled = false;
        oUser.LastLogin = DateTime.Now;
        oUser.LastUpdateBy = oUser.UserId;
        oUser.LastUpdateDate = DateTime.Now;
        oUser.UsedInIPAddress = GetUserIdAddress();

        ent.SaveChanges();
    }

    private string GetUserIdAddress()
    {
        //HttpContext.Current.Request.UserHostAddress; 
        //or 
        //HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //or
        //HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        return Request.UserHostAddress.Trim();
    }

    private void SetSessionUser(MsUser oUser)
    {
        //Common.SessionFKMsGroupMenuId = oUser.FK_MsGroupMenu_Id.GetValueOrDefault();
        Common.SessionLoginDate = DateTime.Now;
        Common.SessionPkUserId = oUser.PK_MsUser_Id;
        Common.SessionRoleName = new MsGroupApprovalDAL().GetMsGroupApprovalNameByPkId(oUser.FK_MsGroupApproval_Id);
        Common.SessionUserID = oUser.UserId;
        Common.SessionUserName = oUser.UserName;
        //Common.SessionCheckerMakerLevel = oUser.CheckerMakerLevel;
        Common.SessionBranchCode = oUser.BranchCode;
        Common.SessionPKGroupApproval = oUser.FK_MsGroupApproval_Id.GetValueOrDefault();
    }

    private void SetSessionUser(MsGroupApproval grpLocal, string userId)
    {
        Common.SessionLoginDate = DateTime.Now;
        //Common.SessionPkUserId = oUser.PK_MsUser_Id;
        //Common.SessionRoleName = oUser.MsGroupMenu.GroupMenuName;
        Common.SessionUserID = userId;
        Common.SessionUserName = userId;
        //Common.SessionCheckerMakerLevel = oUser.CheckerMakerLevel;
        //Common.SessionBranchCode = oUser.BranchCode;
        Common.SessionPKGroupApproval = grpLocal.PK_MsGroupApproval_Id;
    }

    private void UpdateSuccessfullLoginUser(MsUser oUser1)
    {
        new MsUserDAL().UpdateMsUser(oUser1, Request.UserHostAddress.Trim());
    }

    private void SetErrorMsg(string msg)
    {
        Label_Error.Visible = true;
        Label_Error.Text = msg;
    }
    #endregion Additional Methods
}


	}
