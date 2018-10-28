using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using QualiAPI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using MongoDB.Driver.Linq;
using System.IO;
using MongoDB.Driver.Builders;

namespace AntenaServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1
    {
        private static WebRequest req;

        static List<Antenna> antennasList;
        public static List<Antenna> AntennasList
        {
            get
            {
                if (antennasList == null)
                {
                    antennasList = InitAntennasList();
                }

                return antennasList;
            }
            set
            {
                antennasList = value;

            }
        }
        public static List<Antenna> AntennasList1 { get; set; }
        public static List<Antenna> OptionalAntennas { get; set; } = new List<Antenna>();
        public static double MaxRadius { get; set; } = 1000000;

        public static Point GetIsraelPoints(double x, double y)
        {
            int north = -1, east = -1;
            PointConverters.Instance.wgs842itm(x, y, out north, out east);
            return new Point(east, north);
        }
        //calc distance between 3 points in km get x,y of my adress , x,y of antenna, and the max distanation 
        private static double Distance(Point p1, Point p2)
        {
            return (Math.Sqrt(Math.Pow(Math.Abs(p1.X - p2.X), 2) + Math.Pow(Math.Abs(p1.Y - p2.Y), 2)));
        }
        ////get radius,x,y of my adress add to optional Antennas List all antennas in the radius
        public static void AntennasInRadius(double radius, Point myPoint)
        {
            double distance = 0;
            for (int i = 0; i < AntennasList1.Count(); i++)
            {
                Antenna item = AntennasList1[i];
                distance = Distance(myPoint, new Point(Convert.ToInt32(item.X), Convert.ToInt32(item.Y)));
                if (distance < radius)
                {
                    item.Distance = distance;
                    OptionalAntennas.Add(item);//להוסיף אוביקט כמו item + destination  
                    AntennasList1.Remove(item);
                }
            }
        }


        //public static Dictionary<string, double> SearchBestAntennas(double xPoint, double yPoint)
        //{
        //    Point point = GetIsraelPoints(xPoint, yPoint);
        //    double radius = 300000;//לבדוק מאיזה גודל להתחיל


        //    OptionalAntennas = AntennasList.

        //    //שליפת האנטנות בטווח הרצוי של הקליומטרים
        //    Where(a => Math.Abs(a.X - point.X) <= radius && Math.Abs(a.Y - point.Y) <= radius).

        //    //שליפת האנטנות עם המרחק שלהם
        //    Select(a => new { Antena = a, Distance = GetDistance(point.X, a.X, point.Y, a.Y) }).

        //    //מיון לפי המרחק, ושליפת ה20 הכי קרובות
        //    OrderBy(a => a.Distance).Take(20).Select(a => a.Antena).ToList();


        //    //חישוב ה
        //    calcResultByDensityAndRadius();


        //    return sumPointComp();

        //}
        public static Dictionary<string, double> SearchAntennas(double xPoint, double yPoint)
        {
            Point point = GetIsraelPoints(xPoint, yPoint);
            double radius = 10;//לבדוק מאיזה גודל להתחיל
            OptionalAntennas = new List<Antenna>();
            AntennasList1 = new List<Antenna>(AntennasList);
            while (radius <= MaxRadius && OptionalAntennas.Count() < 20)
            {
                AntennasInRadius(radius, point);
                radius += 10;//לברר בכמה להגדיל את הרדיוס
            }
            calcResultByDensityAndRadius();
            return sumPointComp();
        }
        public static void calcResultByDensityAndRadius()
        {
            for (int i = 0; i < OptionalAntennas.Count(); i++)
            {
                OptionalAntennas[i].result = OptionalAntennas[i].Distance / (4 * 3.14 * Math.Pow(OptionalAntennas[i].Distance, 2));
            }

        }
        public static Dictionary<string, double> sumPointComp()
        {

            Dictionary<string, double> dict = new Dictionary<string, double>();
            foreach (var item in OptionalAntennas)
            {
                if (dict.ContainsKey(item.company) == false)
                    dict.Add(item.company, item.result);
                else
                    dict[item.company] += item.result;

            }
            var dictList = dict.ToList();
            dictList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            return dict;
        }

        public static List<Antenna> InitAntennasList()
        {

            List<Antenna> antennasList = new List<Antenna>();
            int x = 100, offset = 0;
           // while (x >= 100)
            //{
                //using (StreamReader r = new StreamReader("C:/Users/user1/Desktop/QualiAPI-master/QualiAPI-master/QualiAPI/Antennas.json"))
                //{
                //    string json = r.ReadToEnd();
                //    List<Antenna> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Antenna>>(json);
                //}
                req = WebRequest.Create("http://www.followme.somee.com/e2.json");
                req.Method = "GET";
                //System.Net.ServicePointManager.Expect100Continue = false;
                req.ContentType = "application/json";
                HttpWebResponse resp =req.GetResponse()as HttpWebResponse;

                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(resp.GetResponseStream(), encoding))
                {

                    string responseText = reader.ReadToEnd();
                    var response = JArray.Parse(responseText);

                   response.ToList().ForEach(a => antennasList.Add(a.ToObject<Antenna>()));
                  //  x = (((JObject)(response.GetValue("result"))).GetValue("records")).Count();
                    //offset += 100;
                }
        //    }

            return antennasList;


        }
        public static List<Antenna> InitAntennasListNew()
        {
            try
            {
                List<Antenna> antennasList = new List<Antenna>();
                int x = 100, offset = 0;
                //while (x >= 100)
                //{
                using (StreamReader r = new StreamReader(@"d:\DZHosts\LocalUser\followme\www.followme.somee.com\e2.json"))
                {
                    string json = r.ReadToEnd();
                    antennasList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Antenna>>(json);
                }


                //}
            } catch
            {
                throw new Exception("אין קובץ");
            }

            return antennasList;


        }
        public static void BestCompany(string CompanyName)
        {
            string connectionString = "mongodb://My-bank:Sb0527189684@ds046047.mlab.com:46047/banks-db";
            //Creating Client
            MongoClient client = null;
            try
            {
                client = new MongoClient(connectionString);
                MongoServer server = null;
                server = client.GetServer();
                MongoDatabase database = null;
                database = server.GetDatabase("banks-db");
                MongoCollection userCollection = null;
                userCollection = database.GetCollection<Company>("company");
                Console.WriteLine(userCollection.Count().ToString());
                List<Company> query = userCollection.AsQueryable<Company>().ToList();
                Company company = userCollection.AsQueryable<Company>().Where<Company>(sb => sb.CompanyName.Equals(CompanyName)).ToList().FirstOrDefault();
                if (company == null)
                {
                    ObjectId id = new ObjectId();
                    //Inserting document to collection/
                    try
                    {
                        company = new Company();
                        company.id = id;
                        company.CompanyName = CompanyName;
                        company.Density = 1;
                        userCollection.Insert(company);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error :" + ex.Message);

                    }
                }
                else
                {
                    userCollection.Update(Query.EQ("CompanyName", CompanyName), Update.Set("Density", company.Density + 1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error :" + ex.Message);

            }
        }
        public static Dictionary<string, double> SummaryAntennas(Company[] companies)
        {
            Dictionary<string, double> dict = new Dictionary<string, double>();
            foreach (var item in companies)
            {
                if (dict.ContainsKey(item.CompanyName) == false)
                    dict.Add(item.CompanyName, item.Density);
                else
                    dict[item.CompanyName] += item.Density;

            }
            var dictList = dict.ToList();
            dictList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            int i = dictList.Count - 1;
            BestCompany(dictList[i].Key);

            return dict;

        }

        public static Dictionary<string, double> StatisticsAntennas()
        {
            string connectionString = "mongodb://My-bank:Sb0527189684@ds046047.mlab.com:46047/banks-db";

            MongoClient client = null;
            client = new MongoClient(connectionString);
            MongoServer server = null;
            server = client.GetServer();
            MongoDatabase database = null;
            database = server.GetDatabase("banks-db");
            MongoCollection userCollection = null;
            userCollection = database.GetCollection<Company>("company");
            List<Company> query = userCollection.AsQueryable<Company>().ToList();
            Dictionary<string, double> dict = new Dictionary<string, double>();
            foreach (var item in query)
            {
                dict.Add(item.CompanyName, item.Density / 10);
            }
            return dict;
        }
    }
}
