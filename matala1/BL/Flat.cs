using System.Net;

namespace matala1.BL
{
    public class Flat
    {
        int id;
        string city;
        string address;
        double price;
        int numberOfRooms;
        static List<Flat> FlatList = new List<Flat>();
        public Flat() { }
        public Flat(int id, string city, string address, double price, int numberOfRooms)
        {
            Id = id;
            City = city;
            Address = address;
            Price = price;
            NumberOfRooms = numberOfRooms;
        }

        public int Id { get => id; set => id = value; }
        public string City { get => city; set => city = value; }
        public string Address { get => address; set => address = value; }
        public double Price { get => price; set => price = value; }
        public int NumberOfRooms { get => numberOfRooms; set => numberOfRooms = value; }

        public int Insert()
        {
           

            DBservices dbs = new DBservices();
            return dbs.InsertF(this);

        }
        public List<Flat> Read(int id)
        {
            DBservices db = new DBservices();
            return db.ReadFlats(id);
        }
        public List<Flat> Read()
        {
            DBservices db = new DBservices();
            return db.ReadOllFlats();
           
        }
        public double Discount(double p, int num)
        {
            if (p > 100 && num > 1)
            {
                return p * 0.9;
            }

            return p;
        }

        static public List<Flat> ReadCityUnderCertainPrice(double price,string city)
        {
            List<Flat> newList = new List<Flat>();
            foreach (Flat flat in FlatList) {
            if(flat.Price < price && flat.City == city )
                {
                    newList.Add(flat);
                }         
            }
            return newList;
        }


       
    }
}

