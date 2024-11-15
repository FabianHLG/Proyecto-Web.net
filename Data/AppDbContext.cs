﻿namespace Proyecto_1.Data
{
    using Microsoft.EntityFrameworkCore;
    using Proyecto_1.Models;
    using System.Collections.Generic;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Promociones> Promociones { get; set; }
    }

}
