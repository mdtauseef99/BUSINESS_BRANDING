using BizzBranding.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBranding.CommonUtility;
using BizzBranding.Models;
using System.IO;
using BizzBranding.Filters;

namespace BizzBranding.Controllers
{
    public class MessagesController : Controller
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();
        //
        // GET: /Messages/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyMessage()
        {
            return View();
        }

        public ActionResult EnquiriesUnread()
        {
            return View();
        }

        public JsonResult PopulateInbox(int skip, int take)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserID"]);
                var MessageList = (from objMessage in objdb.Messages
                                   where objMessage.MsgTo == UserId && objMessage.ToStatus == true && objMessage.IsEnquiry == false && objMessage.Status == true
                                   orderby objMessage.CreatedOn descending
                                   select new
                                   {
                                       CreatedOn = objMessage.CreatedOn,
                                       IsReadTo = objMessage.IsReadTo,
                                       IsReadFrom = objMessage.IsReadFrom,
                                       MessageTitle = objMessage.MsgTitle,
                                       MessageId = objMessage.MessageId,
                                       MessageFrom = objMessage.MsgFrom,
                                       MessageFromName = (from objUser in objdb.BussinessUsers where objUser.UserId == objMessage.MsgFrom select objUser.BusinessName).FirstOrDefault(),
                                       Messagefile = (from a in objdb.MessagesFiles where a.MessageId == objMessage.MessageId select a.MessageFile).FirstOrDefault(),
                                   }).ToList();

                var blocked = objdb.MessagesBlocks.ToList();

                foreach (var item in blocked)
                {
                    foreach (var item1 in MessageList.ToList())
                    {
                        if (item.ToUser == UserId && item.FromUser == item1.MessageFrom)
                        {
                            MessageList.Remove(item1);
                        }
                    }
                }
                var MessageData = MessageList.Skip(skip).Take(take);

                return Json(MessageData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        public JsonResult PopulateOutbox(int skip, int take)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserID"]);
                var MessageList = (from objMessage in objdb.Messages
                                   where objMessage.MsgFrom == UserId && objMessage.ToStatus == true && objMessage.Status == true
                                   orderby objMessage.CreatedOn descending
                                   select new
                                   {
                                       CreatedOn = objMessage.CreatedOn,
                                       IsReadTo = objMessage.IsReadTo,
                                       IsReadFrom = objMessage.IsReadFrom,
                                       MessageTitle = objMessage.MsgTitle,
                                       MessageID = objMessage.MessageId,
                                       MessageToName = (from objUser in objdb.BussinessUsers where objUser.UserId == objMessage.MsgTo select objUser.BusinessName).FirstOrDefault(),
                                       Messagefile = (from a in objdb.MessagesFiles where a.MessageId == objMessage.MessageId select a.MessageFile).FirstOrDefault(),
                                   }).Skip(skip).Take(take).ToList();

                List<MessageDataModel> messagedata = new List<MessageDataModel>();
                foreach (var item in MessageList)
                {
                    messagedata.Add(new MessageDataModel
                    {
                        CreatedOn = item.CreatedOn,
                        IsReadTo = item.IsReadTo,
                        IsReadFrom = item.IsReadFrom,
                        MsgTitle = item.MessageTitle,
                        MessageId = item.MessageID,
                        MessageToName = item.MessageToName,
                        Messagefile = item.Messagefile,
                    });
                }

                return Json(messagedata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(0);
            }
        }

        public JsonResult PopulateTrash(int skip, int take)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserID"]);

                var MessageList = (from objMessage in objdb.Messages
                                   where (objMessage.MsgTo == UserId || objMessage.MsgFrom == UserId) && objMessage.ToStatus == false && objMessage.Status == true
                                   orderby objMessage.MessageId descending
                                   select new
                                   {
                                       CreatedOn = objMessage.CreatedOn,
                                       IsReadTo = objMessage.IsReadTo,
                                       IsReadFrom = objMessage.IsReadFrom,
                                       MessageTitle = objMessage.MsgTitle,
                                       MessageId = objMessage.MessageId,
                                       MessageFrom = objMessage.MsgFrom,
                                       Messagefile = (from a in objdb.MessagesFiles where a.MessageId == objMessage.MessageId select a.MessageFile).FirstOrDefault(),
                                       MessageFromName = (from objUser1 in objdb.BussinessUsers where objUser1.UserId == objMessage.MsgFrom select objUser1.BusinessName).FirstOrDefault(),
                                       MessageToName = (from objUser2 in objdb.BussinessUsers where objUser2.UserId == objMessage.MsgTo select objUser2.BusinessName).FirstOrDefault(),
                                   }).Skip(skip).Take(take).ToList();

                var blocked = objdb.MessagesBlocks.Where(x => x.ToUser == UserId).Select(x => x.FromUser).ToList();

                List<MessageDataModel> returnmessage = new List<MessageDataModel>();

                foreach (var mess in MessageList.ToList())
                {
                    if (blocked.Contains(mess.MessageFrom))
                    {
                        returnmessage.Add(new MessageDataModel
                        {
                            CreatedOn = mess.CreatedOn,
                            IsReadTo = mess.IsReadTo,
                            IsReadFrom = mess.IsReadFrom,
                            MsgTitle = "[Spam]  " + mess.MessageTitle,
                            MessageId = mess.MessageId,
                            MessageFromName = mess.MessageFromName,
                            MessageToName = mess.MessageToName,
                            Messagefile = mess.Messagefile,
                        });
                    }
                    else
                    {
                        returnmessage.Add(new MessageDataModel
                        {
                            CreatedOn = mess.CreatedOn,
                            IsReadTo = mess.IsReadTo,
                            IsReadFrom = mess.IsReadFrom,
                            MsgTitle = mess.MessageTitle,
                            MessageId = mess.MessageId,
                            MessageFromName = mess.MessageFromName,
                            MessageToName = mess.MessageToName,
                            Messagefile = mess.Messagefile,
                        });
                    }
                }


                return Json(returnmessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult ComposeMail(FormCollection form, IEnumerable<HttpPostedFileBase> msgattachfile, bool chkSelectAll = false)
        {
            try
            {
                if (chkSelectAll == false)
                {
                    string[] mail = form[0].Split(',');
                    for (int i = 0; i < mail.Length; i++)
                    {
                        if (mail[i] != "")
                        {
                            string Email = mail[i].ToString();
                            string Message = form[2];
                            string MessageTitle = form[1];
                            string MailAttachmentFile = "";
                            int ToMail = objdb.BussinessUsers.Where(x => x.EmailId == Email).Select(x => x.UserId).SingleOrDefault();
                            if (ToMail == 0)
                            {
                                EmailSettingsModel sendEmail = new EmailSettingsModel();

                                //Save Attachment For Mail
                                foreach (var item in msgattachfile)
                                {
                                    if (item != null)
                                    {
                                        string AttachmentName = item.FileName.ToString();
                                        string folderpath = Server.MapPath("~/MessageAttachments/" + "/Trash");
                                        if (!Directory.Exists(folderpath))
                                        {
                                            Directory.CreateDirectory(folderpath);
                                        }
                                        string strfilepath = Path.Combine(folderpath, AttachmentName);
                                        MailAttachmentFile += strfilepath + ",";
                                        item.SaveAs(strfilepath);
                                    }
                                }

                                bool result = sendEmail.SendMail(Email, Message, MessageTitle, MailAttachmentFile);
                                if (!result)
                                {
                                    return new WrappedJsonResult
                                    {
                                        Data = "Sending Failed."
                                    };
                                }
                                else
                                {
                                    //string Filetodelete = Server.MapPath("~/MessageAttachments/" + "/MailAttachments/" + msgattachfile.FileName);
                                    //System.IO.File.Delete(Filetodelete);
                                }
                            }
                            else
                            {
                                Message message = new Message();

                                int ToUser = Convert.ToInt32(ToMail);
                                int FromUser = Convert.ToInt32(Session["UserID"].ToString());
                                message.CreatedOn = DateTime.Now;
                                message.FromStatus = true;

                                var data = objdb.MessagesBlocks.Where(x => x.FromUser == FromUser && x.ToUser == ToUser).SingleOrDefault();

                                if (data != null)
                                {
                                    message.ToStatus = false;
                                }
                                else
                                {
                                    message.ToStatus = true;
                                }

                                message.IsReadFrom = false;
                                message.IsReadTo = false;
                                message.IsEnquiry = false;
                                message.Status = true;
                                message.Updateddate = DateTime.Now;
                                message.MsgTo = ToUser;
                                message.MsgFrom = FromUser;
                                message.MsgTitle = MessageTitle;
                                message.Message1 = Message;
                                objdb.Messages.Add(message);
                                objdb.SaveChanges();

                                int recordid = message.MessageId;

                                //Save Attachment
                                foreach (var item in msgattachfile)
                                {

                                    if (item != null)
                                    {
                                        //save path for to user
                                        MessagesFile multipleattachments = new MessagesFile();
                                        string AttachmentName = item.FileName.ToString();
                                        string folderpath = Server.MapPath("~/MessageAttachments" + "/" + FromUser + "/");
                                        if (!Directory.Exists(folderpath))
                                        {
                                            Directory.CreateDirectory(folderpath);
                                        }
                                        string strfilepath = Path.Combine(folderpath, AttachmentName);
                                        item.SaveAs(strfilepath);
                                        multipleattachments.MessageFile = FromUser + "/" + AttachmentName;
                                        multipleattachments.MessageId = recordid;
                                        multipleattachments.ToUser = Convert.ToInt32(ToMail);
                                        multipleattachments.FromUser = FromUser;
                                        objdb.MessagesFiles.Add(multipleattachments);
                                        objdb.SaveChanges();
                                    }
                                    else if (form[3] != "")
                                    {
                                        string[] files = form[3].Split(',');
                                        for (int a = 0; a < files.Length; a++)
                                        {
                                            if (files[a] != "")
                                            {
                                                MessagesFile attachfiles = new MessagesFile();
                                                attachfiles.MessageFile = files[a];
                                                attachfiles.MessageId = recordid;
                                                attachfiles.ToUser = Convert.ToInt32(ToMail);
                                                attachfiles.FromUser = FromUser;
                                                objdb.MessagesFiles.Add(attachfiles);
                                                objdb.SaveChanges();
                                            }
                                        }
                                    }
                                    else
                                    {

                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    // Sending mail to All Contact List
                    if (Session["AllEmailList"] != null)
                    {
                        string[] mail = Session["AllEmailList"] as string[];
                        for (int i = 0; i < mail.Length; i++)
                        {
                            if (mail[i] != "")
                            {
                                string Email = mail[i].ToString();
                                string Message = form[3];
                                string MessageTitle = form[2];
                                string MailAttachmentFile = "";
                                int ToMail = objdb.BussinessUsers.Where(x => x.EmailId == Email).Select(x => x.UserId).SingleOrDefault();
                                if (ToMail == 0)
                                {
                                    EmailSettingsModel sendEmail = new EmailSettingsModel();

                                    //Save Attachment For Mail
                                    foreach (var item in msgattachfile)
                                    {
                                        if (item != null)
                                        {
                                            string AttachmentName = item.FileName.ToString();
                                            string folderpath = Server.MapPath("~/MessageAttachments/" + "/Trash");
                                            if (!Directory.Exists(folderpath))
                                            {
                                                Directory.CreateDirectory(folderpath);
                                            }
                                            string strfilepath = Path.Combine(folderpath, AttachmentName);
                                            MailAttachmentFile += strfilepath + ",";
                                            item.SaveAs(strfilepath);
                                        }
                                    }

                                    bool result = sendEmail.SendMail(Email, Message, MessageTitle, MailAttachmentFile);
                                    if (!result)
                                    {
                                        return new WrappedJsonResult
                                        {
                                            Data = "Sending Failed."
                                        };
                                    }
                                    else
                                    {
                                        //string Filetodelete = Server.MapPath("~/MessageAttachments/" + "/MailAttachments/" + msgattachfile.FileName);
                                        //System.IO.File.Delete(Filetodelete);
                                    }
                                }
                                else
                                {
                                    Message message = new Message();

                                    int ToUser = Convert.ToInt32(ToMail);
                                    int FromUser = Convert.ToInt32(Session["UserID"].ToString());
                                    message.CreatedOn = DateTime.Now;
                                    message.FromStatus = true;

                                    var data = objdb.MessagesBlocks.Where(x => x.FromUser == FromUser && x.ToUser == ToUser).SingleOrDefault();

                                    if (data != null)
                                    {
                                        message.ToStatus = false;
                                    }
                                    else
                                    {
                                        message.ToStatus = true;
                                    }

                                    message.IsReadFrom = false;
                                    message.IsReadTo = false;
                                    message.IsEnquiry = false;
                                    message.Status = true;
                                    message.Updateddate = DateTime.Now;
                                    message.MsgTo = ToUser;
                                    message.MsgFrom = FromUser;
                                    message.MsgTitle = MessageTitle;
                                    message.Message1 = Message;
                                    objdb.Messages.Add(message);
                                    objdb.SaveChanges();

                                    int recordid = message.MessageId;

                                    //Save Attachment
                                    foreach (var item in msgattachfile)
                                    {

                                        if (item != null)
                                        {
                                            //save path for to user
                                            MessagesFile multipleattachments = new MessagesFile();
                                            string AttachmentName = item.FileName.ToString();
                                            string folderpath = Server.MapPath("~/MessageAttachments" + "/" + FromUser + "/");
                                            if (!Directory.Exists(folderpath))
                                            {
                                                Directory.CreateDirectory(folderpath);
                                            }
                                            string strfilepath = Path.Combine(folderpath, AttachmentName);
                                            item.SaveAs(strfilepath);
                                            multipleattachments.MessageFile = FromUser + "/" + AttachmentName;
                                            multipleattachments.MessageId = recordid;
                                            multipleattachments.ToUser = Convert.ToInt32(ToMail);
                                            multipleattachments.FromUser = FromUser;
                                            objdb.MessagesFiles.Add(multipleattachments);
                                            objdb.SaveChanges();
                                        }
                                        else if (form[3] != "")
                                        {
                                            string[] files = form[3].Split(',');
                                            for (int a = 0; a < files.Length; a++)
                                            {
                                                if (files[a] != "")
                                                {
                                                    MessagesFile attachfiles = new MessagesFile();
                                                    attachfiles.MessageFile = files[a];
                                                    attachfiles.MessageId = recordid;
                                                    attachfiles.ToUser = Convert.ToInt32(ToMail);
                                                    attachfiles.FromUser = FromUser;
                                                    objdb.MessagesFiles.Add(attachfiles);
                                                    objdb.SaveChanges();
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }

                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception)
            {
                return new WrappedJsonResult
                {
                    Data = "Sending Failed."
                };
            }
            return new WrappedJsonResult
            {
                Data = "Successfully Sent."
            };
        }

        public class WrappedJsonResult : JsonResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.Write(Data);
                context.HttpContext.Response.ContentType = "text";
            }
        }

        public JsonResult DeleteMessage(int MessageID)
        {
            try
            {
                var GetMessageRecord = (from MSG in objdb.Messages where MSG.MessageId == MessageID select MSG).First();
                if (GetMessageRecord != null)
                {
                    GetMessageRecord.ToStatus = false;
                }
                objdb.SaveChanges();
                return Json(1);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        //restore message
        public JsonResult RestoreMessages(int MessageID)
        {
            try
            {
                var Mesage = objdb.Messages.Where(x => x.MessageId == MessageID).FirstOrDefault();
                Message obj = new Message();
                if (Mesage != null)
                {
                    Mesage.ToStatus = true;
                    objdb.SaveChanges();
                }
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        //Delete record from Trash
        public JsonResult DeleteTrashMessage(int MessageID)
        {
            try
            {
                var TrashMessage = (from MSG in objdb.Messages where MSG.MessageId == MessageID select MSG).First();
                if (TrashMessage != null)
                {
                    TrashMessage.Status = false;
                    objdb.SaveChanges();
                }
                return Json(1);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        public JsonResult ViewMessage(int MessageID, string type)
        {
            try
            {
                int userid = Convert.ToInt32(Session["UserID"].ToString());
                int? fromid = objdb.Messages.Where(x => x.MessageId == MessageID).Select(x => x.MsgFrom).SingleOrDefault();
                var GetMessageRecord = (from MSG in objdb.Messages where MSG.MessageId == MessageID select MSG).First();
                if (fromid == userid)
                {
                    GetMessageRecord.IsReadFrom = true;
                    GetMessageRecord.IsReadTo = true;
                }
                else
                {
                    GetMessageRecord.IsReadTo = true;
                }
                objdb.SaveChanges();

                Array GetMessageDetails = null;

                switch (type.ToLower())
                {

                    case "inbox":
                        GetMessageDetails = (from mess in objdb.Messages
                                             where mess.MessageId == MessageID
                                             select new
                                             {
                                                 From = from objUser in objdb.BussinessUsers where objUser.UserId == mess.MsgFrom select objUser.EmailId,
                                                 Subject = mess.MsgTitle,
                                                 Date = mess.CreatedOn,
                                                 Message = mess.Message1,
                                                 MessageFile = (from attach in objdb.MessagesFiles where attach.MessageId == mess.MessageId select attach.MessageFile),
                                             }).ToArray();
                        break;
                    case "outbox":
                        GetMessageDetails = (from mess in objdb.Messages
                                             where mess.MessageId == MessageID
                                             select new
                                             {
                                                 From = "me",
                                                 ToName = from objUser in objdb.BussinessUsers where objUser.UserId == mess.MsgTo select objUser.EmailId,
                                                 Subject = mess.MsgTitle,
                                                 Date = mess.CreatedOn,
                                                 Message = mess.Message1,
                                                 MessageFile = (from attach in objdb.MessagesFiles where attach.MessageId == mess.MessageId select attach.MessageFile),
                                             }).ToArray();
                        break;
                    case "trash":

                        var data = objdb.Messages.Where(x => x.MessageId == MessageID).Select(x => new { x.MsgFrom, x.MsgTo }).ToList();
                        int fr = Convert.ToInt32(data[0].MsgFrom.ToString());
                        int to = Convert.ToInt32(data[0].MsgTo.ToString());
                        var blk = objdb.MessagesBlocks.Where(x => x.FromUser == fr && x.ToUser == to).SingleOrDefault();
                        if (blk != null)
                        {
                            GetMessageDetails = (from mess in objdb.Messages
                                                 where mess.MessageId == MessageID
                                                 select new
                                                 {
                                                     From = from objUser in objdb.BussinessUsers where objUser.UserId == mess.MsgFrom select objUser.EmailId,
                                                     ToName = from objUser in objdb.BussinessUsers where objUser.UserId == mess.MsgTo select objUser.EmailId,
                                                     Subject = "[Spam]" + mess.MsgTitle,
                                                     Date = mess.CreatedOn,
                                                     Message = mess.Message1,
                                                     MessageFile = (from attach in objdb.MessagesFiles where attach.MessageId == mess.MessageId select attach.MessageFile),
                                                 }).ToArray();
                        }
                        else
                        {
                            GetMessageDetails = (from mess in objdb.Messages
                                                 where mess.MessageId == MessageID
                                                 select new
                                                 {
                                                     From = from objUser in objdb.BussinessUsers where objUser.UserId == mess.MsgFrom select objUser.EmailId,
                                                     ToName = from objUser in objdb.BussinessUsers where objUser.UserId == mess.MsgTo select objUser.EmailId,
                                                     Subject = mess.MsgTitle,
                                                     Date = mess.CreatedOn,
                                                     Message = mess.Message1,
                                                     MessageFile = (from attach in objdb.MessagesFiles where attach.MessageId == mess.MessageId select attach.MessageFile),
                                                 }).ToArray();
                        }
                        break;
                    case "enquiry":
                        GetMessageDetails = (from mess in objdb.Messages
                                             where mess.MessageId == MessageID
                                             select new
                                             {
                                                 ToName = "me",
                                                 Subject = mess.MsgTitle,
                                                 Date = mess.CreatedOn,
                                                 Message = mess.Message1,
                                                 MessageFile = (from attach in objdb.MessagesFiles where attach.MessageId == mess.MessageId select attach.MessageFile),
                                             }).ToArray();
                        break;
                }



                return Json(GetMessageDetails);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        //Download Attachment
        public ActionResult UserAttachmentName(int MessageID)
        {
            var Filename = objdb.MessagesFiles.Where(x => x.MessageId == MessageID).Select(x => x.MessageFile).FirstOrDefault();
            string[] file = Filename.Split('/');
            if (Filename != null)
            {
                string path = Server.MapPath("/MessageAttachments/" + file[0] + "/" + file[1]);
                if (System.IO.File.Exists(path))
                {
                    return File(path, "multipart/mixed", file[1]);
                }
            }
            return null;
        }

        //AutoFill
        public JsonResult FindNames()
        {
            int uid = Convert.ToInt32(Session["UserId"]);

            List<BussinessUser> objuser = new List<BussinessUser>();
            List<BussinessUser> objuser1 = new List<BussinessUser>();
            List<BussinessUser> objuser2 = new List<BussinessUser>();
            objuser = (from user in objdb.BussinessUsers
                       join uf in objdb.BusinessConnections on user.UserId equals uf.UserId
                       where uf.BusinessPartnerId == uid && user.IsActive == true && uf.IsActive == true
                       select user).ToList();
            objuser1 = (from user in objdb.BussinessUsers
                        join uf in objdb.BusinessConnections on user.UserId equals uf.BusinessPartnerId
                        where uf.UserId == uid && user.IsActive == true && uf.IsActive == true
                        select user).ToList();
            objuser2 = (from user in objdb.BussinessUsers where user.UserId == uid && user.IsActive == true select user).ToList();
            foreach (var item in objuser1)
            {
                objuser.Add(item);
            }
            foreach (var item1 in objuser2)
            {
                objuser.Add(item1);
            }
            string[] emails = new string[objuser.Count];
            int i = 0;
            foreach (BussinessUser useremail in objuser)
            {
                string userwmail = useremail.EmailId;// +",";
                emails[i] = userwmail;
                i++;
            }
            Session["AllEmailList"] = emails;
            return Json(emails);
        }

        //fill enquiry messages
        public JsonResult FillEnquiryMessages(int skip, int take)
        {
            try
            {
                int UserId = Convert.ToInt32(Session["UserID"]);
                var EnquiryList = (from objMessage in objdb.Messages
                                   where objMessage.MsgTo == UserId && objMessage.IsEnquiry == true && objMessage.ToStatus == true && objMessage.Status == true
                                   orderby objMessage.CreatedOn descending
                                   select new
                                   {
                                       CreatedOn = objMessage.CreatedOn,
                                       IsReadTo = objMessage.IsReadTo,
                                       Message = objMessage.Message1,
                                       MessageTitle = objMessage.MsgTitle,
                                       MessageId = objMessage.MessageId,
                                       Messagefile = (from a in objdb.MessagesFiles where a.MessageId == objMessage.MessageId select a.MessageFile).FirstOrDefault(),
                                   }).Skip(skip).Take(take).ToList();

                return Json(EnquiryList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(0);
            }
        }

        //Insert enquiry details
        [HttpPost]
        public JsonResult InsertEnquiry(FormCollection form, HttpPostedFileBase enqattachfile)
        {
            try
            {
                EmailSettingsModel sendEmail = new EmailSettingsModel();
                Message message = new Message();
                message.CreatedOn = DateTime.Now;
                message.FromStatus = true;
                message.ToStatus = true;
                message.Status = true;
                message.IsReadFrom = false;
                message.IsReadTo = false;
                message.Updateddate = DateTime.Now;
                message.IsEnquiry = true;
                int ToUser = Convert.ToInt32(form["email"]);
                message.MsgTo = ToUser;
                message.MsgFrom = null;
                message.MsgTitle = form["subject"];
                message.Message1 = form["txtmessage"] + " <tr><td style='padding-top: 15px;'>From : <b id='messagefrom'>" + form["from"] + "</b></td></tr>";
                objdb.Messages.Add(message);
                objdb.SaveChanges();

                int recordid = message.MessageId;

                //Save Attachment
                MessagesFile messgefile = new MessagesFile();
                if (enqattachfile != null)
                {
                    string AttachmentName = enqattachfile.FileName.ToString();
                    string folderpath = Server.MapPath("~/MessageAttachments" + "/" + ToUser + "/");
                    if (!Directory.Exists(folderpath))
                    {
                        Directory.CreateDirectory(folderpath);
                    }
                    string filename = Path.GetFileName(enqattachfile.FileName);
                    string strfilepath = Path.Combine(folderpath, filename);
                    enqattachfile.SaveAs(strfilepath);

                    messgefile.MessageFile = ToUser + "/" + AttachmentName;
                    messgefile.ToUser = ToUser;
                    messgefile.MessageId = recordid;
                }

                objdb.MessagesFiles.Add(messgefile);
                objdb.SaveChanges();


            }
            catch (Exception)
            {
                return new WrappedJsonResult
                {
                    Data = "Sending Failed."
                };
            }
            return new WrappedJsonResult
            {
                Data = "Successfully sent."
            };
        }

        //Fill enquiry details for product & services
        public PartialViewResult Enquiry(int id, string type)
        {
            BussinessUserModel data = new BussinessUserModel();
            switch (type)
            {
                case "product":
                    var productdata = objdb.Products.Where(x => x.ProductId == id).Select(x => new
                    {

                        x.BussinessUser.BusinessName,
                        x.BussinessUser.UserId,
                    }).FirstOrDefault();
                    data.BusinessName = productdata.BusinessName;
                    data.UserId = productdata.UserId;
                    break;
                //case "service":
                //    var servicedata = objdb.Services.Where(x => x.ServiceID == id).Select(x => new
                //    {
                //        x.User.UserName,
                //        x.User.UserID,
                //    }).FirstOrDefault();
                //    data.BusinessName = servicedata.UserName;
                //    data.UserId = servicedata.UserID;
                //    break;
                //case "tradelead":
                //    var tradeleaddata = objdb.TradeLeads.Where(x => x.TradeLeadID == id).Select(x => new
                //    {
                //        x.User.UserName,
                //        x.User.UserID,
                //    }).FirstOrDefault();
                //    data.BusinessName = tradeleaddata.UserName;
                //    data.UserId = tradeleaddata.UserID;
                //    break;
                case "siteEnquiry":
                    var siteownerdata = objdb.BussinessUsers.Where(x => x.UserId == id).Select(x => new
                    {
                        x.BusinessName,
                        x.UserId,
                    }).FirstOrDefault();
                    data.BusinessName = siteownerdata.BusinessName;
                    data.UserId = siteownerdata.UserId;
                    break;
                default:
                    break;
            }
            ViewBag.data = data;
            return PartialView("_EnquiryPopUp");
        }

        //EmailID by userID

        public MvcHtmlString GetUserNameByID(int id)
        {
            var name = (from a in objdb.BussinessUsers
                        where a.UserId == id
                        select a.EmailId).SingleOrDefault();
            return new MvcHtmlString(name);
        }

        //get count for paging
        public JsonResult GetTotalCount(string type)
        {
            try
            {
                int uid = Convert.ToInt32(Session["UserID"].ToString());
                int count = 0;
                switch (type)
                {
                    case "inbox":
                        var data = objdb.Messages.Where(x => x.MsgTo == uid && x.ToStatus == true && x.IsEnquiry == false && x.Status == true).Select(x => new { x.MsgFrom, x.MsgTo }).ToList();

                        var blocked = objdb.MessagesBlocks.ToList();
                        foreach (var item in blocked)
                        {
                            foreach (var item1 in data.ToList())
                            {
                                if (item.ToUser == uid && item.FromUser == item1.MsgFrom)
                                {
                                    data.Remove(item1);
                                }
                            }
                        }

                        count = data.Count;
                        break;
                    case "outbox":
                        count = objdb.Messages.Where(x => x.MsgFrom == uid && x.ToStatus == true && x.Status == true).Select(x => x.MessageId).Count();
                        break;
                    case "trash":
                        count = objdb.Messages.Where(x => (x.MsgTo == uid || x.MsgFrom == uid) && x.ToStatus == false && x.Status == true).Select(x => x.MessageId).Count();
                        break;
                    case "enquiry":
                        count = objdb.Messages.Where(x => x.MsgTo == uid && x.ToStatus == true && x.IsEnquiry == true && x.Status == true).Select(x => x.MessageId).Count();
                        break;
                    default:
                        break;
                }
                return Json(count);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        //Block Messages
        public JsonResult BlockMessages(int id)
        {
            try
            {
                int userid = Convert.ToInt32(Session["UserID"].ToString());
                var messge = objdb.Messages.Where(x => x.MessageId == id).SingleOrDefault();
                MessagesBlock msgblk = new MessagesBlock();
                if (messge != null)
                {
                    msgblk.ToUser = userid;
                    msgblk.FromUser = messge.MsgFrom;
                    msgblk.IsBlocked = true;
                    objdb.MessagesBlocks.Add(msgblk);
                    objdb.SaveChanges();
                }
                return Json(1);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        //UnBlock User
        public JsonResult UnBlockUser(int id)
        {
            try
            {
                int userid = Convert.ToInt32(Session["UserID"].ToString());
                var messge = objdb.Messages.Where(x => x.MessageId == id).SingleOrDefault();
                int fromid = Convert.ToInt32(messge.MsgFrom.ToString());
                var messgeblock = objdb.MessagesBlocks.Where(x => x.ToUser == userid && x.FromUser == fromid).SingleOrDefault();
                if (messgeblock != null)
                {
                    objdb.MessagesBlocks.Remove(messgeblock);
                    //objdb.DeleteObject(messgeblock);
                    objdb.SaveChanges();
                }
                return Json(1);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        //Messages Search
        public JsonResult MessagesSearch(string searchtxt, string options, int skip, int take)
        {
            int userid = Convert.ToInt32(Session["UserID"].ToString());
            string text = searchtxt;
            string option;
            if (options == null || options == "")
            {
                option = "from";
            }
            else
            {
                option = options.ToLower();
            }

            List<SearchModel> result = new List<SearchModel>();
            try
            {
                switch (option)
                {
                    case "from":
                        var fromresult = (from mess in objdb.Messages
                                          join file in objdb.MessagesFiles on mess.MessageId equals file.MessageId
                                          into messFile
                                          from fil in messFile.DefaultIfEmpty()
                                          where (mess.MsgTo == userid || mess.MsgFrom == userid) && mess.ToStatus == true //&& mess.User.UserName.StartsWith(searchtxt)
                                          select new
                                          {
                                              MessageFrom = mess.BussinessUser.BusinessName,
                                              MessageTo = mess.BussinessUser1.BusinessName,
                                              CreatedOn = mess.CreatedOn,
                                              MessageTitle = mess.MsgTitle,
                                              MessageId = mess.MessageId,
                                              Message = mess.Message1,
                                              IsReadTo = mess.IsReadTo,
                                              MessageFile = fil.MessageFile,
                                              Email = mess.BussinessUser.EmailId,
                                          }).ToList();

                        foreach (var item in fromresult)
                        {
                            if (item.MessageFrom != null)
                            {
                                if (item.MessageFrom.Contains(text) || item.Email.Contains(text))
                                {
                                    result.Add(new SearchModel
                                    {
                                        MessageFrom = item.MessageFrom,
                                        MessageTo = item.MessageTo,
                                        CreatedOn = item.CreatedOn,
                                        Message = item.Message,
                                        MessageTitle = item.MessageTitle,
                                        MessageId = item.MessageId,
                                        IsReadTo = item.IsReadTo,
                                    });
                                }
                            }
                            else
                            {
                                if (item.Message.Contains(text))
                                {
                                    result.Add(new SearchModel
                                    {
                                        MessageFrom = item.MessageFrom,
                                        MessageTo = item.MessageTo,
                                        CreatedOn = item.CreatedOn,
                                        Message = item.Message,
                                        MessageTitle = item.MessageTitle,
                                        MessageId = item.MessageId,
                                        IsReadTo = item.IsReadTo,
                                    });
                                }
                            }
                        }
                        break;
                    case "to":
                        var to = objdb.Messages.Where(x => x.MsgFrom == userid && x.BussinessUser1.BusinessName.Contains(text)).Select(x => new
                        {
                            MessageFrom = x.BussinessUser.BusinessName,
                            MessageTo = x.BussinessUser1.BusinessName,
                            CreatedOn = x.CreatedOn,
                            Message = x.Message1,
                            MessageTitle = x.MsgTitle,
                            MessageId = x.MessageId,
                            IsReadTo = x.IsReadTo,
                        }).ToList();
                        foreach (var item in to)
                        {
                            result.Add(new SearchModel
                            {
                                MessageFrom = item.MessageFrom,
                                MessageTo = item.MessageTo,
                                CreatedOn = item.CreatedOn,
                                Message = item.Message,
                                MessageTitle = item.MessageTitle,
                                MessageId = item.MessageId,
                                IsReadTo = item.IsReadTo,
                            });
                        }
                        break;
                    case "subject":
                        var subject = objdb.Messages.Where(x => x.MsgFrom == userid || x.MsgTo == userid).Select(x => new
                        {
                            MessageFrom = x.BussinessUser.BusinessName,
                            MessageTo = x.BussinessUser1.BusinessName,
                            CreatedOn = x.CreatedOn,
                            Message = x.Message1,
                            MessageTitle = x.MsgTitle,
                            MessageId = x.MessageId,
                            IsReadTo = x.IsReadTo,
                        }).ToList();

                        foreach (var item in subject)
                        {
                            if (item.MessageTitle.Contains(text))
                            {
                                result.Add(new SearchModel
                                {
                                    MessageFrom = item.MessageFrom,
                                    MessageTo = item.MessageTo,
                                    CreatedOn = item.CreatedOn,
                                    Message = item.Message,
                                    MessageTitle = item.MessageTitle,
                                    MessageId = item.MessageId,
                                    IsReadTo = item.IsReadTo,
                                });
                            }
                        }
                        break;
                    case "has attachments":
                        var attachment = (from mess in objdb.Messages
                                          join file in objdb.MessagesFiles on mess.MessageId equals file.MessageId
                                          into messFile
                                          from fil in messFile.DefaultIfEmpty()
                                          where (mess.MsgTo == userid || mess.MsgFrom == userid) && mess.ToStatus == true
                                          select new
                                          {
                                              MessageFrom = mess.BussinessUser.BusinessName,
                                              MessageTo = mess.BussinessUser1.BusinessName,
                                              CreatedOn = mess.CreatedOn,
                                              MessageTitle = mess.MsgTitle,
                                              MessageId = mess.MessageId,
                                              Message = mess.Message1,
                                              IsReadTo = mess.IsReadTo,
                                              MessageFile = fil.MessageFile
                                          }).ToList();

                        foreach (var item in attachment)
                        {
                            if (item.MessageFile.Contains(text))
                            {
                                result.Add(new SearchModel
                                {
                                    MessageFrom = item.MessageFrom,
                                    MessageTo = item.MessageTo,
                                    CreatedOn = item.CreatedOn,
                                    Message = item.Message,
                                    MessageTitle = item.MessageTitle,
                                    MessageId = item.MessageId,
                                    Messagefile = item.MessageFile,
                                    IsReadTo = item.IsReadTo,
                                });
                            }
                        }
                        break;
                }
                return Json(result.Skip(skip).Take(take));
            }
            catch (Exception)
            {
                return Json(0);
                throw;
            }
        }

        //[Authorize]
        public ActionResult SendEnquiryUser(string email, int userId, string Message)
        {
            try
            {
                Message objmessage = new Message();
                objmessage.CreatedOn = DateTime.Now;
                objmessage.FromStatus = true;
                objmessage.ToStatus = true;
                objmessage.Status = true;
                objmessage.IsReadFrom = false;
                objmessage.IsReadTo = false;
                objmessage.Updateddate = DateTime.Now;
                objmessage.IsEnquiry = true;
                int ToUser = userId;
                objmessage.MsgTo = ToUser;
                if (Session["UserId"] != null)
                {
                    // int CurrentUserId = Convert.ToInt32(Session["UserId"]);
                    objmessage.MsgFrom = Convert.ToInt32(Session["UserId"]);
                }
                else
                {
                    objmessage.MsgFrom = null;
                }

                objmessage.MsgTitle = email;
                objmessage.Message1 = Message;
                objdb.Messages.Add(objmessage);
                objdb.SaveChanges();

                int recordid = objmessage.MessageId;
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }

        

    }
}
