using BursaFuarMerkezi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BursaFuarMerkezi.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<FuarPage> FuarPages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Clothing", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Home Appliances", DisplayOrder = 3 }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 7,
                    Title = "Gaming Laptop X1",
                    Author = "TechPro",
                    Description = "High-performance gaming laptop with RTX graphics, 16GB RAM, and 1TB SSD. Perfect for gamers and content creators.",
                    ISBN = "TECH7001001",
                    ListPrice = 1299,
                    Price = 1249,
                    Price50 = 1199,
                    Price100 = 1149,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 8,
                    Title = "Bluetooth Speaker",
                    Author = "SoundWave",
                    Description = "Portable Bluetooth speaker with 24-hour battery life, waterproof design, and superior sound quality.",
                    ISBN = "SOUND8001001",
                    ListPrice = 89,
                    Price = 79,
                    Price50 = 74,
                    Price100 = 69,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 9,
                    Title = "Fitness Tracker",
                    Author = "HealthTech",
                    Description = "Advanced fitness tracker with heart rate monitor, sleep tracking, and smartphone notifications.",
                    ISBN = "HEALTH9001001",
                    ListPrice = 79,
                    Price = 69,
                    Price50 = 65,
                    Price100 = 59,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 10,
                    Title = "Designer Jeans",
                    Author = "FashionPlus",
                    Description = "Premium denim jeans with modern slim fit design. Available in multiple washes.",
                    ISBN = "FASHION10001",
                    ListPrice = 89,
                    Price = 79,
                    Price50 = 75,
                    Price100 = 69,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 11,
                    Title = "Blender Pro",
                    Author = "KitchenElite",
                    Description = "Professional-grade blender with variable speed control and pulse function. Perfect for smoothies and food preparation.",
                    ISBN = "KITCHEN11001",
                    ListPrice = 129,
                    Price = 119,
                    Price50 = 109,
                    Price100 = 99,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 12,
                    Title = "Microwave Oven",
                    Author = "HomeElectronics",
                    Description = "Digital microwave oven with multiple power settings, timer, and child lock feature.",
                    ISBN = "HOME12001001",
                    ListPrice = 109,
                    Price = 99,
                    Price50 = 95,
                    Price100 = 89,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 13,
                    Title = "Smart TV 55-inch",
                    Author = "VisualTech",
                    Description = "4K Ultra HD Smart TV with built-in streaming apps, voice control, and HDR support.",
                    ISBN = "VISUAL13001",
                    ListPrice = 599,
                    Price = 549,
                    Price50 = 519,
                    Price100 = 499,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 14,
                    Title = "Running Shoes",
                    Author = "SportGear",
                    Description = "Lightweight running shoes with cushioned soles and breathable mesh. Ideal for long-distance running.",
                    ISBN = "SPORT14001001",
                    ListPrice = 119,
                    Price = 109,
                    Price50 = 99,
                    Price100 = 89,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 15,
                    Title = "Air Purifier",
                    Author = "CleanAir",
                    Description = "HEPA air purifier that removes 99.97% of airborne particles. Perfect for allergies and asthma sufferers.",
                    ISBN = "CLEAN15001001",
                    ListPrice = 199,
                    Price = 179,
                    Price50 = 169,
                    Price100 = 159,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 16,
                    Title = "Mechanical Keyboard",
                    Author = "TechInput",
                    Description = "RGB mechanical keyboard with customizable key switches and programmable macros for gaming and typing.",
                    ISBN = "TECH16001001",
                    ListPrice = 129,
                    Price = 119,
                    Price50 = 109,
                    Price100 = 99,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 17,
                    Title = "Leather Jacket",
                    Author = "StyleWear",
                    Description = "Genuine leather jacket with quilted lining. Classic design suitable for casual and formal occasions.",
                    ISBN = "STYLE17001001",
                    ListPrice = 229,
                    Price = 209,
                    Price50 = 199,
                    Price100 = 189,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 18,
                    Title = "Robot Vacuum",
                    Author = "CleanTech",
                    Description = "Smart robot vacuum with mapping technology, app control, and automatic charging. Keeps your floors spotless.",
                    ISBN = "CLEAN18001001",
                    ListPrice = 249,
                    Price = 229,
                    Price50 = 219,
                    Price100 = 199,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 19,
                    Title = "Digital Camera",
                    Author = "PhotoPro",
                    Description = "20MP digital camera with 4K video recording, optical zoom, and image stabilization. Perfect for photography enthusiasts.",
                    ISBN = "PHOTO19001001",
                    ListPrice = 449,
                    Price = 419,
                    Price50 = 399,
                    Price100 = 379,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 20,
                    Title = "Winter Gloves",
                    Author = "ColdGear",
                    Description = "Thermal winter gloves with touchscreen compatibility and waterproof outer layer. Keeps your hands warm and dry.",
                    ISBN = "COLD20001001",
                    ListPrice = 49,
                    Price = 45,
                    Price50 = 42,
                    Price100 = 39,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 21,
                    Title = "Toaster Oven",
                    Author = "KitchenPro",
                    Description = "Multi-function toaster oven with convection, broil, and toast settings. Compact design for small kitchens.",
                    ISBN = "KITCHEN21001",
                    ListPrice = 89,
                    Price = 79,
                    Price50 = 75,
                    Price100 = 69,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 22,
                    Title = "Wireless Mouse",
                    Author = "InputTech",
                    Description = "Ergonomic wireless mouse with adjustable DPI settings and long battery life. Comfortable for all-day use.",
                    ISBN = "INPUT22001001",
                    ListPrice = 39,
                    Price = 35,
                    Price50 = 32,
                    Price100 = 29,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 23,
                    Title = "Casual Polo Shirt",
                    Author = "ComfortWear",
                    Description = "100% cotton polo shirt with moisture-wicking technology. Available in multiple colors.",
                    ISBN = "COMFORT23001",
                    ListPrice = 35,
                    Price = 32,
                    Price50 = 30,
                    Price100 = 28,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 24,
                    Title = "Electric Kettle",
                    Author = "BoilFast",
                    Description = "1.7L electric kettle with rapid boil technology, auto shut-off, and boil-dry protection. Perfect for tea and coffee lovers.",
                    ISBN = "BOIL24001001",
                    ListPrice = 45,
                    Price = 42,
                    Price50 = 39,
                    Price100 = 35,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 25,
                    Title = "Wireless Headphones",
                    Author = "AudioElite",
                    Description = "Over-ear wireless headphones with active noise cancellation, 30-hour battery life, and premium sound quality.",
                    ISBN = "AUDIO25001001",
                    ListPrice = 159,
                    Price = 149,
                    Price50 = 139,
                    Price100 = 129,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 26,
                    Title = "Backpack Pro",
                    Author = "TravelGear",
                    Description = "Water-resistant backpack with laptop compartment, multiple pockets, and USB charging port. Perfect for travel and daily use.",
                    ISBN = "TRAVEL26001",
                    ListPrice = 79,
                    Price = 69,
                    Price50 = 65,
                    Price100 = 59,
                    CategoryId = 2,
                    ImageUrl = ""
                }
            );
        }
    }
}
