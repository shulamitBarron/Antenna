using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QualiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace QualiAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class LoginController : ApiController
    {
        public class LoginUser
        {
            public string password { get; set; }
            public string email { get; set; }
        }
        public IHttpActionResult Post([FromBody]LoginUser registerUser)
        {
            string connectionString = "mongodb://My-bank:Sb0527189684@ds046047.mlab.com:46047/banks-db";
            //Creating Client
            MongoClient client = null;
            try
            {
                client = new MongoClient(connectionString);
            }
            catch (Exception ex)
            {
                //Filed to Create Client/
                return Content(HttpStatusCode.RequestTimeout, true);
            }
            //Initianting Mongo Db Server
            MongoServer server = null;
            try
            {
                server = client.GetServer();
            }
            catch (Exception ex)
            {
                //Filed to getting Server Details
                return Content(HttpStatusCode.RequestTimeout, true);
            }
            //Initianting Mongo Database
            MongoDatabase database = null;
            try
            {
                //Getting reference of database
                database = server.GetDatabase("banks-db");
            }
            catch (Exception ex)
            {
                //Failed to Get reference of Database
                return Content(HttpStatusCode.RequestTimeout, true);
            }
            //"Getting Collections from database Database
            MongoCollection userCollection = null;
            try
            {
                userCollection = database.GetCollection<User>("jhj");
              
            }
            catch (Exception ex)
            {
                //Failed to Get collection from Database
                return Content(HttpStatusCode.NotFound, true);
            }
            try
            {
                List<User> query = userCollection.AsQueryable<User>().ToList();
                User user = userCollection.AsQueryable<User>().Where<User>(sb => sb.Email.Equals(registerUser.email) && sb.password.Equals(registerUser.password)).ToList().FirstOrDefault();
                if (user != null)
                    return Ok(user.FirstName + " " + user.LastName);
                else
                    return Content(HttpStatusCode.NotFound, true);

            }
            catch (Exception ex)
            { 
                return Content(HttpStatusCode.RequestTimeout, true);
            }
            // return Content(HttpStatusCode.NotFound,true);
        }

        public void Options() { }
        //how to get user??
        //[HttpPost]
        //public IHttpActionResult register(User user)
        //{
        //    string connectionString = "mongodb://My-bank:Sb0527189684@ds046047.mlab.com:46047/banks-db";
        //    //Creating Client
        //    MongoClient client = null;
        //    try
        //    {
        //        client = new MongoClient(connectionString);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Filed to Create Client/
        //        Console.WriteLine(ex.Message);
        //    }
        //    //Initianting Mongo Db Server
        //    MongoServer server = null;
        //    try
        //    {
        //        server = client.GetServer();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Filed to getting Server Details
        //        Console.WriteLine(ex.Message);
        //    }
        //    //Initianting Mongo Database
        //    MongoDatabase database = null;
        //    try
        //    {
        //        //Getting reference of database
        //        database = server.GetDatabase("banks-db");
        //    }
        //    catch (Exception ex)
        //    {
        //        //Failed to Get reference of Database
        //        Console.WriteLine("Error :" + ex.Message);
        //    }
        //    //"Getting Collections from database Database
        //    MongoCollection userCollection = null;
        //    try
        //    {
        //        userCollection = database.GetCollection<User>("jhj");
        //        Console.WriteLine(userCollection.Count().ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        //Failed to Get collection from Database
        //        Console.WriteLine("Error :" + ex.Message);
        //    }
        //    try
        //    {
        //        List<User> query = userCollection.AsQueryable<User>().ToList();

        //        if (userCollection.AsQueryable<User>().Where<User>(sb => sb.Email.Equals(user.Email) && sb.password.Equals(user.password)).ToList().FirstOrDefault() == null)
        //        {
        //            ObjectId id = new ObjectId();
        //            //Inserting document to collection/
        //            try
        //            {
        //                user.id = id;
        //                userCollection.Insert(user);
        //                return Ok(user.FirstName + " " + user.LastName);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error :" + ex.Message);

        //            }
        //        }
        //        else
        //        {
        //            return Content(HttpStatusCode.NotFound, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Failed to query from collection");
        //        Console.WriteLine("Exception :" + ex.Message);
        //        return Content(HttpStatusCode.NotFound, true);

        //    }
        //    return Ok("dsfsdfsd");
        //    // return Content(HttpStatusCode.NotFound,true);
        //}
    }
}

