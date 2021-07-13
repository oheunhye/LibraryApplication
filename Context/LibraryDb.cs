using LibraryApplication.Models;
using System.Data.Entity;

namespace LibraryApplication.Context
{
    //Entity Framework를 통해서 DB서버와 연결을 도움
    public class LibraryDb : DbContext
    {
        //Connectio String
        public LibraryDb() :base("name=DBCS") { }
    
        public DbSet<Book> Books { get; set; }
    }
}