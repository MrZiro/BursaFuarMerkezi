using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BursaFuarMerkezi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Title" },
                values: new object[,]
                {
                    { 7, "TechPro", 1, "High-performance gaming laptop with RTX graphics, 16GB RAM, and 1TB SSD. Perfect for gamers and content creators.", "TECH7001001", "", 1299.0, 1249.0, 1149.0, 1199.0, "Gaming Laptop X1" },
                    { 8, "SoundWave", 1, "Portable Bluetooth speaker with 24-hour battery life, waterproof design, and superior sound quality.", "SOUND8001001", "", 89.0, 79.0, 69.0, 74.0, "Bluetooth Speaker" },
                    { 9, "HealthTech", 1, "Advanced fitness tracker with heart rate monitor, sleep tracking, and smartphone notifications.", "HEALTH9001001", "", 79.0, 69.0, 59.0, 65.0, "Fitness Tracker" },
                    { 10, "FashionPlus", 2, "Premium denim jeans with modern slim fit design. Available in multiple washes.", "FASHION10001", "", 89.0, 79.0, 69.0, 75.0, "Designer Jeans" },
                    { 11, "KitchenElite", 3, "Professional-grade blender with variable speed control and pulse function. Perfect for smoothies and food preparation.", "KITCHEN11001", "", 129.0, 119.0, 99.0, 109.0, "Blender Pro" },
                    { 12, "HomeElectronics", 3, "Digital microwave oven with multiple power settings, timer, and child lock feature.", "HOME12001001", "", 109.0, 99.0, 89.0, 95.0, "Microwave Oven" },
                    { 13, "VisualTech", 1, "4K Ultra HD Smart TV with built-in streaming apps, voice control, and HDR support.", "VISUAL13001", "", 599.0, 549.0, 499.0, 519.0, "Smart TV 55-inch" },
                    { 14, "SportGear", 2, "Lightweight running shoes with cushioned soles and breathable mesh. Ideal for long-distance running.", "SPORT14001001", "", 119.0, 109.0, 89.0, 99.0, "Running Shoes" },
                    { 15, "CleanAir", 3, "HEPA air purifier that removes 99.97% of airborne particles. Perfect for allergies and asthma sufferers.", "CLEAN15001001", "", 199.0, 179.0, 159.0, 169.0, "Air Purifier" },
                    { 16, "TechInput", 1, "RGB mechanical keyboard with customizable key switches and programmable macros for gaming and typing.", "TECH16001001", "", 129.0, 119.0, 99.0, 109.0, "Mechanical Keyboard" },
                    { 17, "StyleWear", 2, "Genuine leather jacket with quilted lining. Classic design suitable for casual and formal occasions.", "STYLE17001001", "", 229.0, 209.0, 189.0, 199.0, "Leather Jacket" },
                    { 18, "CleanTech", 3, "Smart robot vacuum with mapping technology, app control, and automatic charging. Keeps your floors spotless.", "CLEAN18001001", "", 249.0, 229.0, 199.0, 219.0, "Robot Vacuum" },
                    { 19, "PhotoPro", 1, "20MP digital camera with 4K video recording, optical zoom, and image stabilization. Perfect for photography enthusiasts.", "PHOTO19001001", "", 449.0, 419.0, 379.0, 399.0, "Digital Camera" },
                    { 20, "ColdGear", 2, "Thermal winter gloves with touchscreen compatibility and waterproof outer layer. Keeps your hands warm and dry.", "COLD20001001", "", 49.0, 45.0, 39.0, 42.0, "Winter Gloves" },
                    { 21, "KitchenPro", 3, "Multi-function toaster oven with convection, broil, and toast settings. Compact design for small kitchens.", "KITCHEN21001", "", 89.0, 79.0, 69.0, 75.0, "Toaster Oven" },
                    { 22, "InputTech", 1, "Ergonomic wireless mouse with adjustable DPI settings and long battery life. Comfortable for all-day use.", "INPUT22001001", "", 39.0, 35.0, 29.0, 32.0, "Wireless Mouse" },
                    { 23, "ComfortWear", 2, "100% cotton polo shirt with moisture-wicking technology. Available in multiple colors.", "COMFORT23001", "", 35.0, 32.0, 28.0, 30.0, "Casual Polo Shirt" },
                    { 24, "BoilFast", 3, "1.7L electric kettle with rapid boil technology, auto shut-off, and boil-dry protection. Perfect for tea and coffee lovers.", "BOIL24001001", "", 45.0, 42.0, 35.0, 39.0, "Electric Kettle" },
                    { 25, "AudioElite", 1, "Over-ear wireless headphones with active noise cancellation, 30-hour battery life, and premium sound quality.", "AUDIO25001001", "", 159.0, 149.0, 129.0, 139.0, "Wireless Headphones" },
                    { 26, "TravelGear", 2, "Water-resistant backpack with laptop compartment, multiple pockets, and USB charging port. Perfect for travel and daily use.", "TRAVEL26001", "", 79.0, 69.0, 59.0, 65.0, "Backpack Pro" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);
        }
    }
}
