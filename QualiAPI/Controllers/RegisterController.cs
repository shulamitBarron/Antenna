using QualiAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using RestSharp;
using QualiAPI.Properties;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Collections.Specialized;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using System.Web.Http.Cors;

namespace QualiAPI.Controllers
{ [EnableCors("*", "*", "*")]
    public class RegisterController : ApiController
    {
       
        public IHttpActionResult Post([FromBody]User user)
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
                Console.WriteLine(ex.Message);
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
                //Console.WriteLine(userCollection.Count().ToString());
            }
            catch (Exception ex)
            {
                //Failed to Get collection from Database
                return Content(HttpStatusCode.RequestTimeout, true);
            }
            try
            {
                List<User> query = userCollection.AsQueryable<User>().ToList();

                if (userCollection.AsQueryable<User>().Where<User>(sb => sb.Email.Equals(user.Email) && sb.password.Equals(user.password)).ToList().FirstOrDefault() == null)
                {
                    ObjectId id = new ObjectId();
                    //Inserting document to collection/
                    try
                    {
                        user.id = id;
                        userCollection.Insert(user);
                        return Ok(user.FirstName + " " + user.LastName);
                    }
                    catch (Exception ex)
                    {
                        return Content(HttpStatusCode.RequestTimeout, true);

                    }
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, true);
                }
            }
            catch (Exception ex)
            {
        
                return Content(HttpStatusCode.RequestTimeout, true);

            }
            return Ok("dsfsdfsd");
            // return Content(HttpStatusCode.NotFound,true);
        }

        public void Options() { }
    }
}
