using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Net;

namespace PagadLibrarySystemDB
{
    internal class Library
    {
        //INITIALIZE SQL CONNECTION
        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\USER\source\repos\PagadLibrarySystemDB\PagadLibrarySystemDB\pagadLibraryDB.mdf;Integrated Security=True;Connect Timeout=30");

        //ADD BOOK METHOD
        internal void addBook(string name, string author, string isbn)
        {
            try
            {
                connection.Open();

                //ADD BOOK TO TABLE
                SqlCommand addBook = new SqlCommand($"INSERT INTO BookTbl (BookName, BookAuthor, BookISBN, BookIsAvailable) VALUES ('{name}', '{author}', '{isbn}', 'true')", connection);

                addBook.ExecuteNonQuery();
                Console.WriteLine($"Book '{name}' added to the database!");

                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong. Please try again...");
                Console.WriteLine(ex);
                System.Threading.Thread.Sleep(5000);
            }
        }

        //REMOVE BOOK METHOD
        internal void removeBook(int id)
        {
            try
            {
                connection.Open();

                //REMOVE BOOK
                SqlCommand removeBook = new SqlCommand($"DELETE FROM BookTbl WHERE BookID = '{id}'", connection);

                removeBook.ExecuteNonQuery();

                Console.WriteLine("Book successfully removed from the database. Returning to Main Menu...");

                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong. Please try again...");
                Console.WriteLine(ex);
            }
        }

        //SEARCH BOOK BY NAME, AUTHOR
        internal void searchBook(string value, int options)
        {
            try
            {
                connection.Open();
                string commandValue;

                //EITHER BOOK NAME OR BOOK AUTHOR
                if(options == 1)
                {
                    commandValue = $"SELECT * FROM Booktbl WHERE BookName = '{value}'";
                }
                else
                {
                    commandValue = $"SELECT * FROM Booktbl WHERE BookAuthor = '{value}'";
                }

                //INITIALIZE SQL COMMAND
                SqlCommand searchBook = new SqlCommand(commandValue, connection);
                SqlDataReader reader = searchBook.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine($"MATCH(ES) FOUND!");
                    while (reader.Read())
                    {
                        //RETRIEVES VALUES FROM TABLE
                        string bookId = reader["BookID"].ToString();
                        string bookName = reader["BookName"].ToString();
                        string bookAuthor = reader["BookAuthor"].ToString();
                        string bookISBN = reader["BookISBN"].ToString();
                        bool bookAvailable = (bool)reader["BookIsAvailable"];
                        string available;

                        //CONVERTS BOOL TO STRING
                        if (bookAvailable)
                        {
                            available = "AVAILABLE";
                        }
                        else
                        {
                            available = "NOT AVAILABLE";
                        }

                        //DISPLAY RESULTS

                        Console.WriteLine("----------------------");
                        Console.WriteLine($"ID: {bookId}");
                        Console.WriteLine($"NAME: {bookName}");
                        Console.WriteLine($"AUTHOR: {bookAuthor}");
                        Console.WriteLine($"ISBN: {bookISBN}");
                        Console.WriteLine($"STATUS: {available}");
                        Console.WriteLine("----------------------");
                    }
                }
                else
                {
                    Console.WriteLine("Book Not Found. Please try again.");
                    System.Threading.Thread.Sleep(1500);
                    Console.Clear();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong. Please try again...");
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
        }

        internal void borrowBook(string bookName)
        {
            try
            {
                Random rand = new Random();
                int borrowCodeGeneration = rand.Next(1000, 9999);
                connection.Open();

                SqlCommand borrowBook = new SqlCommand($"UPDATE BookTbl SET BookIsAvailable = 0, BookBorrowCode = {borrowCodeGeneration} WHERE BookName = '{bookName}'", connection);

                borrowBook.ExecuteNonQuery();

                Console.WriteLine($"Successfully borrowed {bookName}! Your borrow code is {borrowCodeGeneration}. Please make sure to not forget it.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Something went wrong. Please try again...");
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
        }

        internal void returnBook(int code)
        {
            try
            {
                connection.Open();

                SqlCommand borrowBook = new SqlCommand($"UPDATE BookTbl SET BookIsAvailable = 1, BookBorrowCode = NULL WHERE BookBorrowCode = {code}", connection);

                int rowsAffected = borrowBook.ExecuteNonQuery();

                if(rowsAffected > 0)
                {
                    Console.WriteLine($"Successfully returned book! Thank you for returning the book.");
                }
                else
                {
                    Console.WriteLine("Book Borrow Code Not Found. Please Try Again.");
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong. Please try again...");
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
        }

        internal void viewAllBooks()
        {
            try
            {
                connection.Open();
                SqlCommand viewBooks = new SqlCommand("SELECT * FROM BookTbl", connection);
                SqlDataReader reader = viewBooks.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //RETRIEVES VALUES FROM TABLE
                        string bookId = reader["BookID"].ToString();
                        string bookName = reader["BookName"].ToString();
                        string bookAuthor = reader["BookAuthor"].ToString();
                        string bookISBN = reader["BookISBN"].ToString();
                        bool bookAvailable = (bool)reader["BookIsAvailable"];
                        string available;

                        //CONVERTS BOOL TO STRING
                        if (bookAvailable)
                        {
                            available = "AVAILABLE";
                        }
                        else
                        {
                            available = "NOT AVAILABLE";
                        }

                        //DISPLAY RESULTS

                        Console.WriteLine($"#{bookId} - {bookName} by {bookAuthor} ({bookISBN}). STATUS: {available}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong. Please try again...");
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
        }
    }
    internal class Program
    {
        internal void introScreen()
        {
            Console.WriteLine(" ________________________________");
            Console.WriteLine("|                                |");
            Console.WriteLine("|      ZEE'S LIBRARY SYSTEM      |");
            Console.WriteLine("|        1 - ADD A BOOK          |");
            Console.WriteLine("|        2 - REMOVE A BOOK       |");
            Console.WriteLine("|        3 - SEARCH A BOOK       |");
            Console.WriteLine("|        4 - BORROW A BOOK       |");
            Console.WriteLine("|        5 - RETURN A BOOK       |");
            Console.WriteLine("|        6 - VIEW ALL BOOKS      |");
            Console.WriteLine("|        7 - EXIT PROGRAM        |");
            Console.WriteLine("|                                |");
            Console.WriteLine("|________________________________|");
        }
        static void Main(String[] args)
        {
            //INITIALIZE SQL CONNECTION
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\USER\source\repos\PagadLibrarySystemDB\PagadLibrarySystemDB\pagadLibraryDB.mdf;Integrated Security=True;Connect Timeout=30");

            //INITIALIZE OBJECTS
            Program program = new Program();
            Library library = new Library();

            //INITIALIZE MENU BOOLEAN VALUES
            bool mainRun = true; //THE MAIN PROGRAM BOOLEAN VALUE
            bool mainMenuRun = true; //THE MAIN MENU BOOLEAN VALUE
            bool[] menuOptions = new bool[7]; //MAIN MENU OPTIONS BOOLEAN ARRAY

            //PROGRAM FLOW
            while (mainRun)
            {
                //MAIN MENU
                while (mainMenuRun)
                {
                    //MAIN MENU INTERFACE
                    program.introScreen();

                    //BOOLEAN ERROR FOR CHECKING OF INPUT
                    bool error = false;

                    //MAIN MENU USER SELECTION EXCEPTION HANDLING
                    try
                    {
                        //MAIN MENU USER INPUT
                        int mainMenuOption = Convert.ToInt16(Console.ReadLine());
                        mainMenuOption -= 1;

                        //ERROR HANDLING IF STATEMENT
                        if (mainMenuOption > 7 || mainMenuOption < 0)
                        {
                            error = true;
                            Console.WriteLine("You have entered an invalid value. Please try again.");
                            System.Threading.Thread.Sleep(1500);
                            Console.Clear();
                        }
                        else
                        {
                            //ASSIGNS BOOLEAN VALUES TO EACH MAIN MENU OPTIONS
                            for (int i = 0; i < 7; i++)
                            {
                                if (i == mainMenuOption)
                                {
                                    menuOptions[i] = true;
                                }
                                else
                                {
                                    menuOptions[i] = false;
                                }
                            }

                            Console.Clear();
                        }
                    }
                    catch (Exception)
                    {
                        //EXCEPTION HANDLING FOR NON INT VALUES
                        error = true;
                        Console.WriteLine("You have entered an invalid value. Please try again.");
                        System.Threading.Thread.Sleep(1500);
                        Console.Clear();
                    }

                    //IF USER PICKS [7]
                    if (menuOptions[6])
                    {
                        mainRun = false;
                    }

                    //ERROR HANDLING
                    if (error == false)
                    {
                        mainMenuRun = false;
                    }
                }

                //ADDING BOOK INTERFACE
                while (menuOptions[0])
                {
                    //ADD BOOK HEADER
                    Console.WriteLine("          ADD BOOKS          ");
                    Console.WriteLine("_____________________________");

                    //INPUT BOOK VALUES
                    Console.Write("Book Name: ");
                    string bookName = Console.ReadLine();
                    Console.Write("Book Author: ");
                    string bookAuthor = Console.ReadLine();
                    Console.Write("Book ISBN: ");
                    string bookISBN = Console.ReadLine();

                    //INITIALIZE SUB MENUS NAD OPTIONS
                    bool subMenu = true;
                    bool subMenu2 = true;
                    int subOptions = 99;

                    //ADD BOOK SUBMENU 1
                    while (subMenu)
                    {
                        //INTERFACE
                        Console.WriteLine(" ___________________________");
                        Console.WriteLine("|                           |");
                        Console.WriteLine("|     [1] - ADD BOOK        |");
                        Console.WriteLine("|     [2] - RESET           |");
                        Console.WriteLine("|     [3] - BACK TO MENU    |");
                        Console.WriteLine("|                           |");
                        Console.WriteLine("|___________________________|");

                        //EXCEPTION HANDLING
                        try
                        {
                            subOptions = Convert.ToInt16(Console.ReadLine());

                            if (subOptions == 1)
                            {
                                //CALLS ADD BOOK METHOD FROM LIBRARY CLASS
                                library.addBook(bookName, bookAuthor, bookISBN);
                                subMenu = false;
                            }
                            else if (subOptions == 2)
                            {
                                //REITERATES THE LOOP
                                subMenu = false;
                                subMenu2 = false;
                                Console.Clear();
                                break;
                            }
                            else if (subOptions == 3)
                            {
                                //GOES BACK TO THE PREVIOUS MENU
                                subMenu = false;
                                subMenu2 = false;
                                menuOptions[0] = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please try again.");
                                System.Threading.Thread.Sleep(1000);
                                Console.Clear();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                            System.Threading.Thread.Sleep(1000);
                            Console.Clear();
                        }
                    }

                    //ADD BOOK SECOND MENU
                    while (subMenu2)
                    {
                        //INTERFACE
                        Console.WriteLine(" ________________________________");
                        Console.WriteLine("|                                |");
                        Console.WriteLine("|     [1] - ADD ANOTHER BOOK     |");
                        Console.WriteLine("|     [2] - BACK TO MENU         |");
                        Console.WriteLine("|                                |");
                        Console.WriteLine("|________________________________|");

                        //EXCEPTION HANDLING
                        try
                        {
                            subOptions = Convert.ToInt16(Console.ReadLine());

                            if (subOptions == 1)
                            {
                                //REITERATES ADD BOOK LOOP
                                subMenu2 = false;
                                Console.Clear();
                            }
                            else if (subOptions == 2)
                            {
                                //BACK TO THE PREVIOUS MAIN MENU
                                subMenu2 = false;
                                menuOptions[0] = false;
                                mainMenuRun = true;
                                Console.Clear();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please try again.");
                                System.Threading.Thread.Sleep(1500);
                                Console.Clear();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                            System.Threading.Thread.Sleep(1500);
                            Console.Clear();
                        }
                    }
                }

                //REMOVING BOOK INTERFACE
                while (menuOptions[1])
                {
                    //INITIALIZE NECESSARY VALUES
                    int bookID = 99;
                    bool subMenu = true;

                    //HEADER
                    Console.WriteLine("        REMOVE BOOKS         ");
                    Console.WriteLine("_____________________________");
                    Console.Write("SEARCH BOOK BY BOOK ID: ");

                    //EXCEPTION HANDLING
                    try
                    {
                        connection.Open();

                        //USER INPUTS BOOK ID
                        bookID = Convert.ToInt16(Console.ReadLine());

                        //PROGRAM PERFORMS A SEARCH ON THE BOOK
                        SqlCommand searchBook = new SqlCommand($"SELECT * FROM BookTbl WHERE BookID = '{bookID}'", connection);
                        SqlDataReader reader = searchBook.ExecuteReader();

                        //INITIALIZE VALUES
                        string bookId;
                        string bookName = "dummy text";
                        string bookAuthor;
                        string bookISBN;
                        string available;

                        //CHECKS WHETHER THE BOOK WAS FOUND
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //RETRIEVES VALUES FROM TABLE
                                bookId = reader["BookID"].ToString();
                                bookName = reader["BookName"].ToString();
                                bookAuthor = reader["BookAuthor"].ToString();
                                bookISBN = reader["BookISBN"].ToString();
                                bool bookAvailable = (bool)reader["BookIsAvailable"];

                                //CONVERTS BOOL TO STRING
                                if (bookAvailable)
                                {
                                    available = "AVAILABLE";
                                }
                                else
                                {
                                    available = "NOT AVAILABLE";
                                }

                                //OUTPUTS THE SEARCHED BOOK
                                Console.WriteLine("MATCH FOUND!");
                                Console.WriteLine($"ID: {bookId}");
                                Console.WriteLine($"NAME: {bookName}");
                                Console.WriteLine($"AUTHOR: {bookAuthor}");
                                Console.WriteLine($"ISBN: {bookISBN}");
                                Console.WriteLine($"STATUS: {available}");
                            }

                            while (subMenu)
                            {
                                //CONFIRM DELETE BOOK INTERFACE
                                Console.WriteLine($"Are you sure you want to permanently delete {bookName}?");
                                Console.WriteLine("[1] - YES");
                                Console.WriteLine("[2] - NO");

                                try
                                {
                                    int options = Convert.ToInt16(Console.ReadLine());

                                    if(options == 1)
                                    {
                                        //REMOVES THE BOOK AND ENDS LOOP ITERATION
                                        library.removeBook(bookID);
                                        System.Threading.Thread.Sleep(1500);
                                        subMenu = false;
                                        menuOptions[1] = false;
                                        mainMenuRun = true;
                                        Console.Clear();
                                        break;
                                    }
                                    else if(options == 2)
                                    {
                                        subMenu = false;
                                        menuOptions[1] = false;
                                        mainMenuRun = true;
                                        Console.Clear();
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Input. Please try again.");
                                        System.Threading.Thread.Sleep(1500);
                                        Console.Clear();
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Invalid Input. Please try again.");
                                    System.Threading.Thread.Sleep(1500);
                                    Console.Clear();
                                }

                            }
                        }
                        else
                        {
                            Console.WriteLine("Book Not Found. Please try again.");
                            System.Threading.Thread.Sleep(1500);
                            Console.Clear();
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                        System.Threading.Thread.Sleep(1500);
                        Console.Clear();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                //SEARCHING BOOK INTERFACE
                while (menuOptions[2])
                {
                    //INITIALIZE VALUES
                    string input;
                    int options;

                    //MAIN INTERFACE
                    Console.WriteLine(" ________________________________");
                    Console.WriteLine("|                                |");
                    Console.WriteLine("|          SEARCH BOOKS          |");
                    Console.WriteLine("|                                |");
                    Console.WriteLine("|     [1] - SEARCH BY NAME       |");
                    Console.WriteLine("|     [2] - SEARCH BY AUTHOR     |");
                    Console.WriteLine("|     [3] - BACK TO MENU         |");
                    Console.WriteLine("|                                |");
                    Console.WriteLine("|________________________________|");
                    try
                    {
                        options = Convert.ToInt16(Console.ReadLine());

                        //CHANGE TEXT DEPENDING ON WHAT THEY ARE SEARCHING
                        if(options == 1 || options == 2)
                        {
                            string searchBy;
                            if (options == 1)
                            {
                                searchBy = "NAME";
                            }
                            else
                            {
                                searchBy = "AUTHOR";
                            }
                            Console.Write($"SEARCH BY {searchBy}: ");
                            input = Console.ReadLine();
                            library.searchBook(input, options);
                            Console.WriteLine("GOING BACK TO THE MENU IN TEN SECONDS.");
                            System.Threading.Thread.Sleep(10000);
                            Console.Clear();
                            break;
                        }
                        else if(options == 3)
                        {
                            menuOptions[2] = false;
                            mainMenuRun = true;
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input. Please try again.");
                            System.Threading.Thread.Sleep(1500);
                            Console.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                        Console.WriteLine(ex);
                        System.Threading.Thread.Sleep(1500);
                        Console.Clear();
                    }
                }

                //BORROWING BOOK INTERFACE
                while (menuOptions[3])
                {
                    //INITIALIZE VALUES
                    string name = "dummy text";
                    string bookName;
                    bool bookAvailable = false;
                    
                    //HEADER
                    Console.WriteLine("        BORROW BOOKS         ");
                    Console.WriteLine("_____________________________");
                    Console.Write("SEARCH BOOK BY BOOK NAME: ");
                    try
                    {
                        bookName = Console.ReadLine();

                        connection.Open();

                        //SEARCH WHETHER THE BOOK IS AVAILABLE OR NOT
                        SqlCommand searchBook = new SqlCommand($"SELECT * FROM Booktbl WHERE BookName = '{bookName}'", connection);
                        SqlDataReader reader = searchBook.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //RETRIEVES VALUES FROM TABLE
                                name = reader["BookName"].ToString();
                                string bookAuthor = reader["BookAuthor"].ToString();
                                bookAvailable = (bool)reader["BookIsAvailable"];
                                string available;

                                //CONVERTS BOOL TO STRING
                                if (bookAvailable)
                                {
                                    available = "AVAILABLE";
                                }
                                else
                                {
                                    available = "NOT AVAILABLE";
                                }

                                //OUTPUTS THE SEARCHED BOOK
                                Console.WriteLine($"THE BOOK {name} BY {bookAuthor} IS {available}.");
                            }

                            if (bookAvailable)
                            {
                                //BORROW CONFIRM
                                Console.WriteLine("");
                                Console.WriteLine($"WOULD YOU LIKE TO BORROW THE BOOK {name}?");
                                Console.WriteLine("[1] - YES");
                                Console.WriteLine("[2] - NO");
                                Console.WriteLine("[3] - NO AND GO BACK TO MENU");
                                try
                                {
                                    int borrowConfirm = Convert.ToInt16(Console.ReadLine());

                                    if (borrowConfirm == 1)
                                    {
                                        library.borrowBook(name);

                                        Console.WriteLine("");
                                        Console.WriteLine("WILL AUTOMATICALLY GO BACK TO THE MAIN MENU IN 15 SECONDS. PLEASE TAKE NOTE OF YOUR BORROW CODE.");
                                        menuOptions[3] = false;
                                        mainMenuRun = true;
                                        System.Threading.Thread.Sleep(15000);
                                        Console.Clear();
                                        connection.Close();
                                        break;
                                    }
                                    else if(borrowConfirm == 2)
                                    {
                                        Console.Clear();
                                        connection.Close();
                                        break;
                                    }
                                    else if (borrowConfirm == 3)
                                    {
                                        Console.Clear();
                                        menuOptions[3] = false;
                                        mainMenuRun = true;
                                        connection.Close();
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input. Please try again.");
                                        System.Threading.Thread.Sleep(1500);
                                        Console.Clear();
                                    }
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine("Invalid input. Please try again.");
                                    Console.WriteLine(ex);
                                    System.Threading.Thread.Sleep(1500);
                                    Console.Clear();
                                }
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(2000);
                                Console.Clear();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Book Not Found. Please try again.");
                            System.Threading.Thread.Sleep(1500);
                            Console.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                        Console.WriteLine(ex);
                        System.Threading.Thread.Sleep(1500);
                        Console.Clear();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                //RETURNING BOOK INTERFACE
                while (menuOptions[4])
                {
                    int borrowCode = 10000;
                    Console.WriteLine("        RETURN BOOK          ");
                    Console.WriteLine("_____________________________");
                    Console.Write("ENTER THE BOOK BORROW CODE: ");
                    try
                    {
                        borrowCode = Convert.ToInt16(Console.ReadLine());

                        if(borrowCode < 1000 || borrowCode > 9999)
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                            System.Threading.Thread.Sleep(1500);
                            Console.Clear();
                        }
                        else
                        {
                            library.returnBook(borrowCode);
                            Console.WriteLine();
                            Console.WriteLine("GOING BACK TO THE MAIN MENU IN FIVE SECONDS...");
                            menuOptions[4] = false;
                            mainMenuRun = true;
                            System.Threading.Thread.Sleep(5000);
                            Console.Clear();
                            break;
                        }
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                        System.Threading.Thread.Sleep(1500);
                        Console.Clear();
                    }
                }

                //VIEW ALL BOOK INTERFACE
                while (menuOptions[5])
                {
                    library.viewAllBooks();
                    Console.WriteLine("PRESS ANY KEY TO GO BACK...");
                    Console.ReadKey();
                    menuOptions[5] = false;
                    mainMenuRun = true;
                    Console.Clear();
                }
            }

            //EXIT PROGRAM
            Console.WriteLine(" __________________________________________________");
            Console.WriteLine("|                                                  |");
            Console.WriteLine("|          THANK YOU FOR USING THE PROGRAM         |");
            Console.WriteLine("|                                                  |");
            Console.WriteLine("|__________________________________________________|");
            System.Threading.Thread.Sleep(1500);
        }
    }
}