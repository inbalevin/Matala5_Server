using System.Collections.Generic;

namespace matala1.BL
{
    public class Vacation
    {
        int id;
        string userId;
        int flatId;
        DateTime startDate;
        DateTime endDate;
        static List <Vacation> vacationsList = new List <Vacation> ();
   
      
        public int Id { get => id; set => id = value; }
        public string UserId { get => userId; set => userId = value; }
        public int FlatId { get => flatId; set => flatId = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }

        public Vacation() { }

        public Vacation(int id, string userId, int flatId, DateTime startDate, DateTime endDate)
        {
            Id = id;
            UserId = userId;
            FlatId = flatId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int Insert()
        {
            if(vacationsList.Exists(vac =>  vac.Id == Id))
            {
                return 0;
            }
            foreach (Vacation item in vacationsList)
            {
                if ((item.FlatId == this.FlatId ) && 
                    ((this.startDate <= item.StartDate && this.endDate >= item.StartDate)|| 
                    (this.startDate >= item.StartDate && this.endDate <= item.EndDate)||
                    (this.startDate <= item.EndDate && this.endDate >= item.EndDate)))
                { 
                    return 0; 
                }
            }
            vacationsList.Add(this);

            DBservices dbs = new DBservices();
            return dbs.InsertV(this);
           // return true;
        }

        public List<Vacation> Read()
        {
            DBservices db = new DBservices();
            return db.ReadOllVacations();
           
        }
        public List<Vacation> Read(int id)
        {
            DBservices db = new DBservices();
            return db.ReadVacations(id);
        }
        static  public List <Vacation> ReadgetByDates(DateTime startDate ,DateTime endDate) { 
        List <Vacation> newList = new List <Vacation>();
        List <Vacation> newList1 = new List <Vacation>();
            DBservices db = new DBservices();          
            newList1 = db.ReadVacBetween(startDate, endDate);
            foreach (Vacation item in newList)
            {
               if(item.EndDate<= endDate && item.StartDate>= startDate)
                {                    
                    newList.Add(item);
                }
            }
            return newList1;


        }


    }
}
