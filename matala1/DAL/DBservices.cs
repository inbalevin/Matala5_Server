//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System.Data.SqlClient;
using System.Data;
//using System.Text;
using matala1.BL;
//using RuppinProj.Models;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }


    //--------------------------------------------------------------------------------------------------
    // This method update a user to the user table 
    //--------------------------------------------------------------------------------------------------
    public int Update(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("SP_updateUser", con, user);  // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }



    //--------------------------------------------------------------------------------------------------
    // This method insert a user to the user table 
    //--------------------------------------------------------------------------------------------------
    public int InsertU(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateInsertWithStoredProcedure("SP_insertUser", con, user);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private SqlCommand CreateInsertWithStoredProcedure(String spName, SqlConnection con, User user)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@firstName", user.FirstName);

        cmd.Parameters.AddWithValue("@LastName", user.FamilyName);

        cmd.Parameters.AddWithValue("@email", user.Email);

        cmd.Parameters.AddWithValue("@password", user.Password);

        cmd.Parameters.AddWithValue("@isActive", user.IsActive);

        cmd.Parameters.AddWithValue("@isAdmin", user.IsAdmin);



        return cmd;
    }

    //ReadUsers

    public List<User> ReadUsers(string email)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<User> users = new List<User>();

        cmd = buildReadStoredProcedureCommandUsers(con, "SP_getUser", email);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            User u = new User();

            u.FirstName = dataReader["firstName"].ToString();
            u.FamilyName = dataReader["LastName"].ToString();
            u.Email = dataReader["email"].ToString();
            u.Password= dataReader["password"].ToString();
            users.Add(u);

        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }

        return users;


    }


    public User LoginUs(string email, string password)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateInsertWithStoredProcedureLogU("SP_LoginUsers", con,  email,  password);             // create the command
        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        if (dataReader == null)
        {
            return null;
        }
        User u = new User();
        while (dataReader.Read())
        {         
            u.Email = dataReader["email"].ToString();
            u.FirstName = dataReader["firstName"].ToString();
            u.FamilyName = dataReader["lastName"].ToString();
            u.Password = dataReader["password"].ToString();
            u.IsActive = (bool)dataReader["isActive"];
            u.IsAdmin = (bool)dataReader["isAdmin"];
        }

        if (con != null)
        {
            con.Close();
        }

        return u;

    }
    private SqlCommand CreateInsertWithStoredProcedureLogU(String spName, SqlConnection con, string email, string password)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text  

        cmd.Parameters.AddWithValue("@email", email);

        cmd.Parameters.AddWithValue("@password", password);


        return cmd;
    }

    public int LogOutUs(string email)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateInsertWithStoredProcedureLogOu("SP_LogOutUser", con, email);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    private SqlCommand CreateInsertWithStoredProcedureLogOu(String spName, SqlConnection con, string email)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text  

        cmd.Parameters.AddWithValue("@email", email);



        return cmd;
    }
    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateDeleteWithStoredProcedure(String spName, SqlConnection con, int id)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@id", id);




        return cmd;
    }



    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, User user)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@firstName", user.FirstName);

        cmd.Parameters.AddWithValue("@LastName", user.FamilyName);

        cmd.Parameters.AddWithValue("@email", user.Email);

        cmd.Parameters.AddWithValue("@password", user.Password);

        cmd.Parameters.AddWithValue("@isActive", user.IsActive);

        cmd.Parameters.AddWithValue("@isAdmin", user.IsAdmin);


        return cmd;
    }



    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
   

    //KKK
    private SqlCommand buildReadStoredProcedureCommandUsers(SqlConnection con, String spName, string email)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@email", email);

        return cmd;
    }

    public List<User> ReadAllUsers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        List<User> users = new List<User>();

        cmd = buildReadStoredProcedureCommandAllUsers(con, "SP_ReadAllUsers"); 

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            User u = new User();

            u.FirstName = dataReader["firstName"].ToString();
            u.FamilyName = dataReader["LastName"].ToString();
            u.Email = dataReader["email"].ToString();
            u.Password = dataReader["password"].ToString();
            u.IsActive = (bool)dataReader["isActive"];
            u.IsAdmin = (bool)dataReader["isAdmin"];
            users.Add(u);

        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }

        return users;


    }
    private SqlCommand buildReadStoredProcedureCommandAllUsers(SqlConnection con, String spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method insert a Flat to the flat table 
    //--------------------------------------------------------------------------------------------------
    public int InsertF(Flat flat)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateInsertWithStoredProcedureFlats("SP_insertFlat", con, flat);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateInsertWithStoredProcedureFlats(String spName, SqlConnection con, Flat flat)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@City", flat.City);

        cmd.Parameters.AddWithValue("@Address", flat.Address);

        cmd.Parameters.AddWithValue("@Price", flat.Price);

        cmd.Parameters.AddWithValue("@NumberOfRooms", flat.NumberOfRooms);


        return cmd;
    }


    //ReadFlats

    public List<Flat> ReadFlats(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Flat> flats = new List<Flat>();

        cmd = buildReadStoredProcedureCommandFlats(con, "SP_getFlat", id);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Flat F = new Flat();

            F.Id = Convert.ToInt32(dataReader["id"]);
            F.City = dataReader["City"].ToString();
            F.Address = dataReader["Address"].ToString();
            F.Price = Convert.ToInt32(dataReader["Price"]);
            F.NumberOfRooms = Convert.ToInt32(dataReader["Number Of Rooms"]);
            flats.Add(F);

        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }

        return flats;

    }
    //KKK
    private SqlCommand buildReadStoredProcedureCommandFlats(SqlConnection con, String spName, int id)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@id", id);

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method insert a vacation to the vacation table 
    //--------------------------------------------------------------------------------------------------
    public int InsertV(Vacation vacation)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateInsertWithStoredProcedureVacation("SP_insertVacation", con, vacation);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateInsertWithStoredProcedureVacation(String spName, SqlConnection con, Vacation vacation)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@StartDate", vacation.StartDate);

        cmd.Parameters.AddWithValue("@EndDate", vacation.EndDate);

        cmd.Parameters.AddWithValue("@VacationId", vacation.Id);

        cmd.Parameters.AddWithValue("@UserId", vacation.UserId);

        cmd.Parameters.AddWithValue("@FlatId", vacation.FlatId);


        return cmd;
    }


    //ReadVacations

    public List<Vacation> ReadVacations(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Vacation> vacations = new List<Vacation>();

        cmd = buildReadStoredProcedureCommandVac(con, "SP_getVacation", id);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Vacation v = new Vacation();

            v.Id = Convert.ToInt32(dataReader["VacationId"]);
            v.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
            v.EndDate = Convert.ToDateTime(dataReader["EndDate"]);
            v.UserId = dataReader["UserId"].ToString();
            v.FlatId = Convert.ToInt32(dataReader["FlatId"]);
            vacations.Add(v);

        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }

        return vacations;

    }
    //KKK
    private SqlCommand buildReadStoredProcedureCommandVac(SqlConnection con, String spName, int id)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@id", id);

        return cmd;
    }
    public List<Flat> ReadOllFlats()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Flat> flat = new List<Flat>();

        cmd = buildReadStoredProcedureCommand(con, "SP_ReadAllFlats");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Flat F = new Flat();

            F.Id = Convert.ToInt32(dataReader["id"]);
            F.City = dataReader["City"].ToString();
            F.Address = dataReader["Address"].ToString();
            F.Price = Convert.ToInt32(dataReader["Price"]);
            F.NumberOfRooms = Convert.ToInt32(dataReader["Number Of Rooms"]);
            flat.Add(F);

        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }

        return flat;


    }

    SqlCommand buildReadStoredProcedureCommand(SqlConnection con, string spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;

    }

    public List<Vacation> ReadOllVacations()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Vacation> vac = new List<Vacation>();

        cmd = buildReadStoredProcedureCommandV(con, "SP_ReadAllVacations");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Vacation v = new Vacation();

            v.Id = Convert.ToInt32(dataReader["VacationId"]);
            v.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
            v.EndDate = Convert.ToDateTime(dataReader["EndDate"]);
            v.UserId = dataReader["UserId"].ToString();
            v.FlatId = Convert.ToInt32(dataReader["FlatId"]);
            vac.Add(v);

        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }

        return vac;


    }


    SqlCommand buildReadStoredProcedureCommandV(SqlConnection con, string spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;

    }


    public List<Vacation> ReadVacBetween(DateTime startDate, DateTime endDate)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Vacation> vac = new List<Vacation>();

        cmd = buildReadStoredProcedureCommandVB(con, "SP_getVac", startDate, endDate);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Vacation v = new Vacation();

            v.Id = Convert.ToInt32(dataReader["VacationId"]);
            v.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
            v.EndDate = Convert.ToDateTime(dataReader["EndDate"]);
            v.UserId = dataReader["UserId"].ToString();
            v.FlatId = Convert.ToInt32(dataReader["FlatId"]);
            vac.Add(v);

        }

        if (con != null)
        {
            // close the db connection
            con.Close();
        }

        return vac;


    }

    SqlCommand buildReadStoredProcedureCommandVB(SqlConnection con, string spName, DateTime startDate, DateTime endDate)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text


        cmd.Parameters.AddWithValue("@startDate", startDate);
        cmd.Parameters.AddWithValue("@endDate", endDate);
        return cmd;

    }

    public List<Object> GetAverage(int month)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Object> avgList = new List<Object>();

        cmd = buildReadVacsStoredProcedureCommand(con, "SP_GetAveragePricePerNightByCityAndMonth", month);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            avgList.Add(new
            {
                City = dataReader["City"].ToString(),
                averagePerCity = Convert.ToDouble(dataReader["AveragePricePerNight"]),
            });

        }

        if (con != null)
        {
            con.Close();
        }

        return avgList;

    }

    SqlCommand buildReadVacsStoredProcedureCommand(SqlConnection con, string spName, int month)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@month", month);
        return cmd;

    }

}

