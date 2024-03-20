
namespace matala1.BL
{
    public class User
    {
        private string firstName;
        private string familyName;
        private string email;
        private string password;
        private bool isAdmin;
        private bool isActive;

        static List<User> UserList = new List<User>();

        public string FirstName { get => firstName; set => firstName = value; }
        public string FamilyName { get => familyName; set => familyName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public bool IsActive { get => isActive; set => isActive = value; }

        public User() { }
        public User(string firstName, string familyName, string email, string password, bool isAdmin, bool isActive)
        {
            FirstName = firstName;
            FamilyName = familyName;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
            IsActive = isActive;
        }

        public int Insert()
        {
            
            DBservices dbs = new DBservices();
            return dbs.InsertU(this);
         
        }
        public List<User> Read()
        {
            DBservices db = new DBservices();
            UserList = db.ReadAllUsers();
            return UserList;
        }

      
         public User CheckUser(string password, string email)
        {
            DBservices dbs = new DBservices();
            User u = dbs.LoginUs(email, password);
            return u;
        }



         public int EditU()
        {
            DBservices dbs = new DBservices();
            return dbs.Update(this);

           
        }
    }
}
