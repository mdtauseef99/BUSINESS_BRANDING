using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizzBranding.Models
{
    public class BizzFrndRequestModel
    {

    }

    public class FriendListEntity
    {
        private string _imageName = "ee.gif";
        private string _Username = "";
        private Nullable<global::System.Int32> _intUserFriendid;
        private Nullable<global::System.Int32> _intUid;
        private Nullable<global::System.Int32> _intFid;


        private int? _Usertype = 0;

        public string Image_Name
        {
            get { return _imageName; }
            set { _imageName = value; }
        }
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }
        public Nullable<global::System.Int32> UserFriendid
        {
            get { return _intUserFriendid; }
            set { _intUserFriendid = value; }
        }
        public Nullable<global::System.Int32> Uid
        {
            get { return _intUid; }
            set { _intUid = value; }
        }
        public Nullable<global::System.Int32> Fid
        {
            get { return _intFid; }
            set { _intFid = value; }
        }

        //public int? Usertype
        //{
        //    get { return _Usertype; }
        //    set { _Usertype = value; }
        //}
    }

    public class UserFriendRequest
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public bool? RequestSent { get; set; }
        public int userFriendMapid { get; set; }
    }

    public class PersonViewModel
    {
        public string Message { get; set; }
    }
}