using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace BizzBranding.CommonUtility
{
    public class EmailSettingsModel
    {
        public int EmailSettingsID { get; set; }
        public int TemplateTypeID { get; set; }
        public string TemplateName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public bool IsSystemTemplete { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsActive { get; set; }


        public bool SendMail(string MailTo, string Body, string Subject, string file)
        {
            try
            {
                SmtpClient serv = new SmtpClient();
                MailMessage msg = new MailMessage();
                if (file != "")
                {
                    string[] files = file.Split(',');
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i] != "")
                        {
                            Attachment attach = new Attachment(files[i]);
                            msg.Attachments.Add(attach);
                        }
                    }
                }
                msg.To.Add(MailTo);
                msg.Body = Body;
                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress("businessbranding.in@gmail.com");
                serv.Host = "smtp.gmail.com";
                serv.Port = 587;
                serv.UseDefaultCredentials = true;
                serv.Credentials = new System.Net.NetworkCredential("businessbranding.in@gmail.com", "amirpasha");
                serv.EnableSsl = true;
                serv.Send(msg);
                //MailAddress mailfrom = new MailAddress("sk8885443070@gmail.com");
                //MailAddress mailto = new MailAddress("mohd.masi.ece@gmail.com");
                //MailMessage newmsg = new MailMessage(mailfrom, mailto);

                //newmsg.Subject = "Test Mail";
                //newmsg.Body = "Body(message) of email "+pswd;

                //SmtpClient smtps = new SmtpClient("smtp.gmail.com", 587);
                //smtps.UseDefaultCredentials = false;
                //smtps.Credentials = new NetworkCredential("sk8885443070@gmail.com", "8885443070");
                //smtps.EnableSsl = true;

                //smtps.Send(newmsg);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
