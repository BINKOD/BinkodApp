using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using BinkodApp.Core;

namespace BinkodApp.Web.Helper
{
    public class ScheduledJobs
    {
        public void SendEmail()
        {
            try
            {
                //if (HttpContext.Current != null && HttpContext.Current.Request != null && !HttpContext.Current.Request.IsLocal && !string.IsNullOrEmpty(HttpContext.Current.Request.UserHostName))
                string HostName = Dns.GetHostName();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("royalbottle.info@gmail.com");
                mail.To.Add("binkod.info@gmail.com");
                mail.Subject = "Test Mail";
                mail.Body = "Test Scheduled Mail :" + DateTime.Now;

                //System.Net.Mail.Attachment attachment = attachment = new System.Net.Mail.Attachment("attachment file");
                //mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("royalbottle.info@gmail.com", "royalpassword");
                SmtpServer.EnableSsl = true;

                //SmtpServer.Send(mail);
            }
            catch (Exception ex) { }
        }

        public void DeleteOlderFiles()
        {
            Utils.DeleteFiles_ThreeMonthOlder();
        }
    }
}