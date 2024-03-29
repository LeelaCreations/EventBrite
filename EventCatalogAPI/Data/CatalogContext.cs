﻿using EventCatalogAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.Data
{
    public class CatalogContext:DbContext
    {
        public CatalogContext(DbContextOptions options) : base(options) { }
        public DbSet<EventsCatalog> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Location> Locations { get; set; }

        //on model creation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventsCatalog>(ConfigureEventsCatalog);
            modelBuilder.Entity<EventCategory>(ConfigureEventCategory);
            modelBuilder.Entity<EventType>(ConfigureEventType);
            modelBuilder.Entity<Location>(ConfigureLocation);

        }

        private void ConfigureEventsCatalog(EntityTypeBuilder<EventsCatalog> builder)
        {
            builder.ToTable("Catalog");
            builder.Property(e => e.Id).IsRequired().ForSqlServerUseSequenceHiLo("event_hilo");
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Price).IsRequired();
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.PictureUrl).IsRequired();

            //Foreign Key relation with EventCategory
            builder.HasOne(e => e.EventCategory)
                .WithMany()
                .HasForeignKey(e => e.EventCategoryId);

            //Foreign Key relation with EventType
            builder.HasOne(e => e.EventType)
                .WithMany()
                .HasForeignKey(e => e.EventTypeId);

            //Foreign Key relation with Location
            builder.HasOne(e => e.Location)
                .WithMany()
                .HasForeignKey(e => e.LocationId);


        }

        private void ConfigureEventCategory(EntityTypeBuilder<EventCategory> builder)
        {
            builder.ToTable("EventCategories");
            builder.Property(c => c.Id).IsRequired().ForSqlServerUseSequenceHiLo("event_categories_hilo");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(250);
        }

        private void ConfigureEventType(EntityTypeBuilder<EventType> builder)
        {
            builder.ToTable("EventTypes");
            builder.Property(t => t.Id).IsRequired().ForSqlServerUseSequenceHiLo("event_types_hilo");
            builder.Property(t => t.Type).IsRequired().HasMaxLength(100);
        }

        private void ConfigureLocation(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");
            builder.Property(l => l.Id).IsRequired().ForSqlServerUseSequenceHiLo("locations_hilo");
            builder.Property(l => l.UserId).IsRequired();
            builder.Property(l => l.VenueName).HasMaxLength(100);
            builder.Property(l => l.Address).IsRequired();
            builder.Property(l => l.Address2).HasMaxLength(50);
            builder.Property(l => l.City).IsRequired().HasMaxLength(250);
            builder.Property(l => l.State).IsRequired().HasMaxLength(250);
            builder.Property(l => l.PostalCode).IsRequired();
            builder.Property(l => l.Country).IsRequired().HasMaxLength(50);
        }

    }
}
