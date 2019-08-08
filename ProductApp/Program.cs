using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Bson.Serialization.IdGenerators;
//using MongoDB.Bson.Serialization;

namespace ProductApp
{
    //public class profileidgenerator : iidgenerator
    //{
    //    public object generateid(object container, object document)
    //    {
    //        return "test-" + guid.newguid().tostring();
    //    }

    //    public bool isempty(object id)
    //    {
    //        return id == null || string.isnullorempty(id.tostring());
    //    }
    //}
    public class Products
    {
        //[bsonid(idgenerator = typeof(profileidgenerator))]
        //public string _id { get; set; }

        [BsonId]
        public ObjectId _id { get; set; }

        public int ProductID { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string Color { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime SellEndDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        /*    product.ModifiedDate,
              public bool MakeFlag { get; set; }
              public bool FinishedGoodsFlag { get; set; }
              public short SafetyStockLevel { get; set; }
              public short ReorderPoint { get; set; }
              public int DaysToManufacture { get; set; }
              public string rowguid { get; set; }*/
    }

    public class Program
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public static Products GetProduct()
        {             
            Console.WriteLine("Please enter ProductID : ");
            string PId = Console.ReadLine();

            Console.WriteLine("Please enter product Name : ");
            string PNm = Console.ReadLine();

            Console.WriteLine("Please enter ProductNumber : ");
            string PNum = Console.ReadLine();

            Console.WriteLine("Please enter Color : ");
            string Color = Console.ReadLine();

            Console.WriteLine("Please enter StandardCost : ");
            string StandardCost = Console.ReadLine();

            Console.WriteLine("Please enter ListPrice : ");
            string ListPrice = Console.ReadLine();

            Console.WriteLine("Please enter SellStartDate : ");
            string SellStartD = Console.ReadLine();

            Console.WriteLine("Please enter SellEndDate : ");
            string SellEndD = Console.ReadLine();

            Console.WriteLine("Please enter ModifiedDate : ");
            string ModifiedDat = Console.ReadLine();
            
            Products product = new Products()
            {
                ProductID = int.Parse(PId),
                Name = PNm,
                ProductNumber = PNum,
                Color = Color,
                StandardCost = Convert.ToDecimal(StandardCost),
                ListPrice = Convert.ToDecimal(ListPrice),
                SellStartDate = Convert.ToDateTime(SellStartD),
                SellEndDate = Convert.ToDateTime(SellEndD),
                ModifiedDate = Convert.ToDateTime(ModifiedDat),
            };

            return product;
        }

        public void CRUDwithMongoDb()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("adventureworks");
            var _collection = _database.GetCollection<Products>("Product1");

            Console.WriteLine
                ("Press select your option from the following\n1 - Insert\n2 - Update One Document\n3 - Delete\n4 - Read All\n");
            string userSelection = Console.ReadLine();

            switch (userSelection)
            {
                case "1":
                    //Insert  
                    _collection.InsertOne(GetProduct());
                    break;

                case "2":
                    //Update  
                    var obj1 = GetProduct();
                    
                    _collection.FindOneAndUpdate<Products>
                        (Builders<Products>.Filter.Eq("ProductID", obj1.ProductID),
                            Builders<Products>.Update.Set("Name", obj1.Name).
                            Set("ProductNumber", obj1.ProductNumber).
                            Set("Color", obj1.Color).Set("StandardCost", obj1.StandardCost).
                            Set("ListPrice", obj1.ListPrice).
                            Set("SellStartDate", obj1.SellStartDate).Set("SellEndDate", obj1.SellEndDate).
                            Set("ModifiedDate", obj1.ModifiedDate));

                    break;

                case "3":
                    //Find and Delete  
                    Console.WriteLine("Please Enter the ProductID to delete the record(so called document) : ");
                    var deleteProduct = Console.ReadLine();
                    _collection.DeleteOne(s => s.ProductID == int.Parse(deleteProduct));

                    break;

                case "4":
                    //Read all existing document  

                    var products = _collection.AsQueryable<Products>().ToList();
                    foreach (var y in products)
                    {
                        Console.WriteLine(y._id + "  " + y.ProductID + "\t" + y.Name + "\t" +
                            y.ProductNumber + "\t" + y.Color + "\t" +
                            y.StandardCost + "\t" + y.ListPrice + "\t" +
                            y.SellStartDate + "\t" + y.SellEndDate + "\t" + y.ModifiedDate);
                    }


                    var all = _collection.Find(new BsonDocument());
                    Console.WriteLine();

                    foreach (var i in all.ToEnumerable())
                    {
                        Console.WriteLine(i._id + "  " + i.ProductID + "\t" + i.Name + "\t" + 
                            i.ProductNumber + "\t" + i.Color + "\t" + 
                            i.StandardCost + "\t" + i.ListPrice + "\t" + 
                            i.SellStartDate + "\t" + i.SellEndDate + "\t" + i.ModifiedDate);
                    }

                    break;

                default:
                    Console.WriteLine("Please choose a correct option");
                    break;
            }

            //To continue with Program  
            Console.WriteLine("\n--------------------------------------------------------------\nPress Y for continue...\n");
            string userChoice = Console.ReadLine();

            if (userChoice == "Y" || userChoice == "y")
            {
                this.CRUDwithMongoDb();
            }
        }

        public static void Main(string[] args)
        {
            Program p = new Program();
            p.CRUDwithMongoDb();


            //Hold the screen by logic  
            Console.WriteLine("Press any key to teminated the program");
            Console.ReadKey();
        }
    }
}