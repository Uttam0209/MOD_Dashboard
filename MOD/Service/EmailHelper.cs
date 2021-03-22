using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MOD.Service
{
    public class EmailHelper
    {
        public static string SendChangePasswordMail(string UserMail, string newpassord, string mailPath)
        {

            try
            {
                string Body = EmailHelper.SendChangePasswordOTpPopulateBody(newpassord, mailPath);
                EmailHelper.SendEmail(UserMail, Body);
            }
            catch (Exception)
            {
            }
            return "success";
        }
        private static string SendChangePasswordOTpPopulateBody(string newpassord, string mailPath)
        {
            return mailPath.Replace("{password}", newpassord);
        }

        public static void SendEmail(string Email, string Body)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage();
                MailAddress mailAddress = new MailAddress(ConfigurationManager.AppSettings["FromAddress"].ToString(), ConfigurationManager.AppSettings["FromName"].ToString());
                smtpClient.Host = ConfigurationManager.AppSettings["smtpAddress"].ToString();
                message.From = mailAddress;
                message.To.Add(Email);
                message.Priority = MailPriority.High;
                message.Subject = "MoD (ACQUISITION) DASHBOARD";
                message.Body = Body.ToString();
                message.IsBodyHtml = true;
                smtpClient.EnableSsl = false;
                smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(ConfigurationManager.AppSettings["MailUserID"].ToString(), ConfigurationManager.AppSettings["MailPassword"].ToString());
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                string msg = ex.StackTrace;
            }
        }

    }
}