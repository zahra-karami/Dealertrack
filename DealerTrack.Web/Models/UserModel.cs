﻿using System;

namespace DealerTrack.Web.Models
{
    [Serializable]
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
