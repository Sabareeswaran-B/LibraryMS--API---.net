#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Model;

    public class LMSContext : DbContext
    {
        public LMSContext (DbContextOptions<LMSContext> options)
            : base(options)
        {
        }

        public DbSet<LibraryMS.Model.Visitor> Visitor { get; set; }

        public DbSet<LibraryMS.Model.Employee> Employee { get; set; }

        public DbSet<LibraryMS.Model.Book> Book { get; set; }

        public DbSet<LibraryMS.Model.Author> Author { get; set; }

        public DbSet<LibraryMS.Model.Lending> Lending { get; set; }
    }
