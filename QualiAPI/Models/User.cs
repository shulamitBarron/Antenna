using MongoDB.Bson;
using System.Collections.Generic;
using System.Drawing;

namespace QualiAPI.Models
{
    public class User
    {   public ObjectId id { get; set; }
        public string tz { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public List<Point> addresses { get; set; }
    }
}