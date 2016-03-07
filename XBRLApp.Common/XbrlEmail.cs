using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace XBRLApp.Common
{
    public class XbrlEmail
    {
        private string m_sSender;
        private string m_sRecipient;
        private string m_sRecipientCC;
        private string m_sRecipientBCC;
        private string m_sServer;
        private string m_sSubject;
        private string m_sBody;
        private MailPriority m_mPriority;
        private string m_sMailAttachment;
        private string m_sLastErrorMessage;
        private bool m_IsBodyHtml;
        private bool m_bLastSendState;

        public string Subject
        {
            get { return m_sSubject; }
            set { m_sSubject = value; }
        }

         public string Body
        {
            get { return m_sBody; }
            set { m_sBody = value; }
        }

         public string Sender
        {
            get { return m_sSender; }
            set { m_sSender = value; }
        }

        /// <summary>
        /// Get or set Recipient of email. For multiple recipient, separate it using ";"
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Recipient
        {
            get { return m_sRecipient; }
            set { m_sRecipient = value; }
        }

        /// <summary>
        /// Get or set Recipient CC of email. For multiple recipient, separate it using ";"
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RecipientCC
        {
            get { return m_sRecipientCC; }
            set { m_sRecipientCC = value; }
        }

        /// <summary>
        /// Get or set Recipient BCC of email. For multiple recipient, separate it using ";"
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RecipientBCC
        {
            get { return m_sRecipientBCC; }
            set { m_sRecipientBCC = value; }
        }

        /// <summary>
        /// Get or set value of SMTP Server
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Server
        {
            get { return m_sServer; }
            set { m_sServer = value; }
        }

        /// <summary>
        /// Get or set priority of the email
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public MailPriority Priority
        {
            get { return m_mPriority; }
            set { m_mPriority = value; }
        }

        /// <summary>
        /// Get or set mail attachment with fullpath of filename. For multiple attachment, separate it using ";"
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string MailAttachment
        {
            get { return m_sMailAttachment; }
            set { m_sMailAttachment = value; }
        }

        /// <summary>
        /// Get or set last send state.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool LastSendState
        {
            get { return m_bLastSendState; }
            set { m_bLastSendState = value; }
        }

        /// <summary>
        /// Get or set mail body type is HTML or not
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsBodyHtml
        {
            get { return m_IsBodyHtml; }
            set { m_IsBodyHtml = value; }
        }

        /// <summary>
        /// Get last error message
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string LastErrorMessage
        {
            get { return m_sLastErrorMessage; }
        }

        private string _smtpServer;
        public string SmtpServer
        {
            get { return _smtpServer; }
            set { _smtpServer = value; }
        }

        private bool _isUseCredential;
        public bool IsUseCredential
        {
            get { return _isUseCredential; }
            set { _isUseCredential = value; }
        }

        private string _userCredential;
        public string UserCredential
        {
            get { return _userCredential; }
            set { _userCredential = value; }
        }

        private string _password;
        public string PasswordCredential
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _smtpPort;
        public string SmtpPort
        {
            get { return _smtpPort; }
            set { _smtpPort = value; }
        }

        private bool _isUseSSL;
        public bool IsUseSSL
        {
            get { return _isUseSSL; }
            set { _isUseSSL = value; }
        }

        /// <summary>
        /// Initial email
        /// </summary>
        /// <remarks></remarks>
        private void Init()
        {
            m_sBody = "";
            m_sSubject = "";
            m_sSender = "";
            m_sRecipient = "";
            m_sRecipientCC = "";
            m_sRecipientBCC = "";
            m_sLastErrorMessage = "";
            m_sServer = SmtpServer;
            m_mPriority = MailPriority.Normal;
            m_IsBodyHtml = IsBodyHtml;
            m_bLastSendState = false;
            _isUseCredential = false;
            _smtpPort = "";
            _isUseSSL = false;
        }

        /// <summary>
        /// New implementation of Email class
        /// </summary>
        /// <remarks></remarks>
        public XbrlEmail()
        {
            Init();
        }

        /// <summary>
        /// Dispose implementation of Email class
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {

        }

        /// <summary>
        /// To initial email with default initial value
        /// </summary>
        /// <remarks></remarks>
        public void Clear()
        {
            Init();
        }

        /// <summary>
        /// Check if recipient and sender is valid
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool isValidMail()
        {
            bool tempisValidMail = false;
            if (m_sSender.Trim().Length > 0)
            {
                if (m_sRecipient.Trim().Length > 0)
                {
                    tempisValidMail = true;
                }
                else
                {
                    tempisValidMail = false;
                    m_sLastErrorMessage = "Invalid Recipient Address";
                }
            }
            else
            {
                tempisValidMail = false;
                m_sLastErrorMessage = "Invalid Sender Address";
            }
            return tempisValidMail;
        }

        /// <summary>
        /// 'Send Email Synchronously with all properties.
        /// </summary>
        /// <returns>Return TRUE if send mail done successfully, otherwise return FALSE</returns>
        /// <remarks></remarks>
        public bool SendEmail()
        {
            //For a Windows Forms Application, add a reference to 
            //System.Web.dll using [Project Add References]

            MailMessage email = null;
            char chrDelimiter = ';';
            string[] arrFileName = null;
            int intFileNameCounter = 0;
            Attachment att = null;
            bool blnSend = false;
            SmtpClient smtpMail = null;
            int Counter = 0;
            string[] arrMailAddress = null;
            try
            {
                smtpMail = new SmtpClient(SmtpServer);
                if (SmtpPort != "")
                {
                    smtpMail.Port = Convert.ToInt32(SmtpPort);
                    
                }
                    
                if (isValidMail())
                {
                    email = new MailMessage();
                    //The email address of the sender
                    email.From = new MailAddress(Sender);

                    //The email address of the recipient. 
                    //Seperate multiple email adresses with a comma seperator
                    m_sRecipient = m_sRecipient.Replace(",", ";");
                    arrMailAddress = m_sRecipient.Split(chrDelimiter);
                    for (Counter = 0; Counter < arrMailAddress.Length; Counter++)
                    {
                        email.To.Add(arrMailAddress[Counter]);
                    }

                    if (m_sRecipientCC.Trim().Length > 0)
                    {
                        m_sRecipientCC = m_sRecipientCC.Replace(",", ";");
                        arrMailAddress = m_sRecipientCC.Split(chrDelimiter);
                        for (Counter = 0; Counter < arrMailAddress.Length; Counter++)
                        {
                            email.CC.Add(arrMailAddress[Counter]);
                        }
                    }

                    if (m_sRecipientBCC.Trim().Length > 0)
                    {
                        m_sRecipientBCC = m_sRecipientBCC.Replace(",", ";");
                        arrMailAddress = m_sRecipientBCC.Split(chrDelimiter);
                        for (Counter = 0; Counter < arrMailAddress.Length; Counter++)
                        {
                            email.Bcc.Add(arrMailAddress[Counter]);
                        }
                    }

                    //The subject of the email
                    email.Subject = m_sSubject;

                    //The Priority attached and displayed for the email
                    email.Priority = m_mPriority;

                    //The format of the contents of the email
                    //The email format can either be MailFormat.Html or MailFormat.Text
                    //MailFormat.Html : Html Content
                    //MailFormat.Text : Text Message
                    email.IsBodyHtml = m_IsBodyHtml;

                    //The contents of the email 
                    email.Body = m_sBody;

                    //EMail Attachment 
                    // Build an IList of mail attachments using the files named in the string.
                    if (m_sMailAttachment != null)
                    {
                        if (m_sMailAttachment.Trim().Length > 0)
                        {
                            arrFileName = m_sMailAttachment.Split(chrDelimiter);
                            for (intFileNameCounter = 0; intFileNameCounter < arrFileName.Length; intFileNameCounter++)
                            {
                                att = new Attachment(arrFileName[intFileNameCounter].ToString());
                                email.Attachments.Add(att);
                                att = null;
                            }
                        }
                    }

                    //The name of the smtp server to use for sending emails
                    //SmtpMail.SmtpServer is commonly ignored in many applications 


                    //Send the email and handle any error that occurs
                    try
                    {
                        if (IsUseCredential)
                        {
                            smtpMail.Credentials =
                            new NetworkCredential(UserCredential,PasswordCredential);
                            }
                        if (IsUseSSL)
                        {
                            smtpMail.EnableSsl = IsUseSSL;
                        }
                        
                        smtpMail.Send(email);
                        blnSend = true;
                    }
                    catch (Exception ex)
                    {
                        //myLog.Error("Failed send email to " & m_sRecipient & " with subject " & m_sSubject & " cause: " & ex.Message, ex)
                        m_sLastErrorMessage = ex.Message;
                        blnSend = false;
                        throw ex;
                    }
                }
                else
                {
                    blnSend = false;
                }
                m_bLastSendState = blnSend;
                return blnSend;

            }
            catch (Exception ex)
            {
                //myLog.Error("Failed send email to " & m_sRecipient & " with subject " & m_sSubject & " cause: " & ex.Message, ex)
                m_sLastErrorMessage = ex.Message;
                blnSend = false;
                throw ex;
            }
        }
    }
}