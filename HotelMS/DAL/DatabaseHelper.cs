using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using HotelMS.Models;

namespace HotelMS.DAL
{
    /// <summary>
    /// Central data access helper for the Hotel Management System.
    /// Handles: user registration/login, room type pricing (CRUD), bookings,
    /// and bill generation/publishing.
    ///
    /// This is the C# / SQL Server LocalDB equivalent of the original Android
    /// app's SQLiteOpenHelper (DatabaseHelper.java) - same tables, same
    /// behaviour, same seeded sample data.
    /// </summary>
    public class DatabaseHelper
    {
        // Connection string that points at the "master" database - used only
        // to check for / create the HotelMSDb database itself.
        private static string MasterConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder(AppConnectionString)
                {
                    InitialCatalog = "master"
                };
                return builder.ConnectionString;
            }
        }

        private static string AppConnectionString =>
            ConfigurationManager.ConnectionStrings["HotelMSConnection"].ConnectionString;

        private static string ConnectionString => AppConnectionString;

        private static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        private static string Now()
        {
            return DateTime.Now.ToString("dd MMM yyyy, hh:mm tt");
        }

        // -----------------------------------------------------------------
        // DATABASE / TABLE CREATION (equivalent of onCreate / onUpgrade)
        // -----------------------------------------------------------------

        /// <summary>
        /// Creates the HotelMSDb database and its tables if they do not
        /// already exist, and seeds four sample room types on first run.
        /// Safe to call every time the application starts.
        /// </summary>
        public static void EnsureDatabase()
        {
            var builder = new SqlConnectionStringBuilder(AppConnectionString);
            string dbName = builder.InitialCatalog;

            using (var conn = new SqlConnection(MasterConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @name) " +
                    "CREATE DATABASE [" + dbName + "]", conn))
                {
                    cmd.Parameters.AddWithValue("@name", dbName);
                    cmd.ExecuteNonQuery();
                }
            }

            using (var conn = GetConnection())
            {
                RunIfMissing(conn, "users", @"
                    CREATE TABLE users (
                        id INT IDENTITY(1,1) PRIMARY KEY,
                        full_name NVARCHAR(200) NOT NULL,
                        email NVARCHAR(200) NULL,
                        phone NVARCHAR(50) NULL,
                        username NVARCHAR(100) NOT NULL UNIQUE,
                        password NVARCHAR(200) NOT NULL
                    )");

                RunIfMissing(conn, "room_types", @"
                    CREATE TABLE room_types (
                        id INT IDENTITY(1,1) PRIMARY KEY,
                        name NVARCHAR(200) NOT NULL,
                        description NVARCHAR(500) NULL,
                        price_per_night DECIMAL(10,2) NOT NULL,
                        total_rooms INT NOT NULL DEFAULT 1
                    )");

                RunIfMissing(conn, "bookings", @"
                    CREATE TABLE bookings (
                        id INT IDENTITY(1,1) PRIMARY KEY,
                        guest_name NVARCHAR(200) NOT NULL,
                        guest_phone NVARCHAR(50) NULL,
                        room_type_id INT NOT NULL,
                        room_type_name NVARCHAR(200) NULL,
                        room_price DECIMAL(10,2) NULL,
                        check_in DATE NULL,
                        check_out DATE NULL,
                        nights INT NULL,
                        guests INT NULL,
                        total_amount DECIMAL(10,2) NULL,
                        status NVARCHAR(20) NOT NULL DEFAULT 'BOOKED',
                        created_at NVARCHAR(50) NULL,
                        FOREIGN KEY (room_type_id) REFERENCES room_types(id)
                    )");

                RunIfMissing(conn, "bills", @"
                    CREATE TABLE bills (
                        id INT IDENTITY(1,1) PRIMARY KEY,
                        booking_id INT NOT NULL,
                        subtotal DECIMAL(10,2) NULL,
                        tax_amount DECIMAL(10,2) NULL,
                        grand_total DECIMAL(10,2) NULL,
                        generated_at NVARCHAR(50) NULL,
                        published BIT NOT NULL DEFAULT 1,
                        FOREIGN KEY (booking_id) REFERENCES bookings(id)
                    )");

                SeedDefaultRoomsIfEmpty(conn);
            }
        }

        private static void RunIfMissing(SqlConnection conn, string tableName, string createSql)
        {
            using (var check = new SqlCommand(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @t", conn))
            {
                check.Parameters.AddWithValue("@t", tableName);
                int count = (int)check.ExecuteScalar();
                if (count == 0)
                {
                    using (var create = new SqlCommand(createSql, conn))
                    {
                        create.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void SeedDefaultRoomsIfEmpty(SqlConnection conn)
        {
            using (var check = new SqlCommand("SELECT COUNT(*) FROM room_types", conn))
            {
                int count = (int)check.ExecuteScalar();
                if (count > 0) return;
            }

            var defaults = new (string Name, string Description, decimal Price, int Total)[]
            {
                ("Standard", "Single bed, city view", 1500m, 10),
                ("Deluxe", "Double bed, complimentary breakfast", 2800m, 6),
                ("Suite", "Living area, king bed, minibar", 4500m, 3),
                ("Executive Suite", "Premium suite with balcony & lounge access", 6500m, 2)
            };

            foreach (var r in defaults)
            {
                using (var cmd = new SqlCommand(
                    "INSERT INTO room_types (name, description, price_per_night, total_rooms) " +
                    "VALUES (@name, @description, @price, @total)", conn))
                {
                    cmd.Parameters.AddWithValue("@name", r.Name);
                    cmd.Parameters.AddWithValue("@description", r.Description);
                    cmd.Parameters.AddWithValue("@price", r.Price);
                    cmd.Parameters.AddWithValue("@total", r.Total);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // -----------------------------------------------------------------
        // USERS: registration & login
        // -----------------------------------------------------------------

        public bool IsUsernameTaken(string username)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM users WHERE username = @u", conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public int RegisterUser(User user)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "INSERT INTO users (full_name, email, phone, username, password) " +
                "OUTPUT INSERTED.id " +
                "VALUES (@fullName, @email, @phone, @username, @password)", conn))
            {
                cmd.Parameters.AddWithValue("@fullName", user.FullName);
                cmd.Parameters.AddWithValue("@email", (object)user.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@phone", (object)user.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                return (int)cmd.ExecuteScalar();
            }
        }

        /// <summary>Returns the matching User, or null if credentials are invalid.</summary>
        public User Login(string username, string password)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT * FROM users WHERE username = @u AND password = @p", conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return ReadUser(reader);
                }
            }
            return null;
        }

        private User ReadUser(IDataRecord r)
        {
            return new User
            {
                Id = (int)r["id"],
                FullName = r["full_name"] as string,
                Email = r["email"] as string,
                Phone = r["phone"] as string,
                Username = r["username"] as string,
                Password = r["password"] as string
            };
        }

        // -----------------------------------------------------------------
        // ROOM PRICING: entering / editing hotel room prices
        // -----------------------------------------------------------------

        public int AddRoomType(RoomType room)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "INSERT INTO room_types (name, description, price_per_night, total_rooms) " +
                "OUTPUT INSERTED.id " +
                "VALUES (@name, @description, @price, @total)", conn))
            {
                cmd.Parameters.AddWithValue("@name", room.Name);
                cmd.Parameters.AddWithValue("@description", (object)room.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@price", room.PricePerNight);
                cmd.Parameters.AddWithValue("@total", room.TotalRooms);
                return (int)cmd.ExecuteScalar();
            }
        }

        public void UpdateRoomType(RoomType room)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "UPDATE room_types SET name = @name, description = @description, " +
                "price_per_night = @price, total_rooms = @total WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@name", room.Name);
                cmd.Parameters.AddWithValue("@description", (object)room.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@price", room.PricePerNight);
                cmd.Parameters.AddWithValue("@total", room.TotalRooms);
                cmd.Parameters.AddWithValue("@id", room.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteRoomType(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("DELETE FROM room_types WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<RoomType> GetAllRoomTypes()
        {
            var list = new List<RoomType>();
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT * FROM room_types ORDER BY name ASC", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read()) list.Add(ReadRoomType(reader));
            }
            return list;
        }

        public RoomType GetRoomType(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT * FROM room_types WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return ReadRoomType(reader);
                }
            }
            return null;
        }

        private RoomType ReadRoomType(IDataRecord r)
        {
            return new RoomType
            {
                Id = (int)r["id"],
                Name = r["name"] as string,
                Description = r["description"] as string,
                PricePerNight = (decimal)r["price_per_night"],
                TotalRooms = (int)r["total_rooms"]
            };
        }

        // -----------------------------------------------------------------
        // BOOKINGS
        // -----------------------------------------------------------------

        public int CreateBooking(Booking b)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "INSERT INTO bookings (guest_name, guest_phone, room_type_id, room_type_name, " +
                "room_price, check_in, check_out, nights, guests, total_amount, status, created_at) " +
                "OUTPUT INSERTED.id " +
                "VALUES (@guestName, @guestPhone, @roomTypeId, @roomTypeName, @roomPrice, " +
                "@checkIn, @checkOut, @nights, @guests, @total, @status, @createdAt)", conn))
            {
                cmd.Parameters.AddWithValue("@guestName", b.GuestName);
                cmd.Parameters.AddWithValue("@guestPhone", (object)b.GuestPhone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@roomTypeId", b.RoomTypeId);
                cmd.Parameters.AddWithValue("@roomTypeName", b.RoomTypeName);
                cmd.Parameters.AddWithValue("@roomPrice", b.RoomPrice);
                cmd.Parameters.AddWithValue("@checkIn", b.CheckIn);
                cmd.Parameters.AddWithValue("@checkOut", b.CheckOut);
                cmd.Parameters.AddWithValue("@nights", b.Nights);
                cmd.Parameters.AddWithValue("@guests", b.Guests);
                cmd.Parameters.AddWithValue("@total", b.TotalAmount);
                cmd.Parameters.AddWithValue("@status", Booking.STATUS_BOOKED);
                cmd.Parameters.AddWithValue("@createdAt", Now());
                return (int)cmd.ExecuteScalar();
            }
        }

        public List<Booking> GetAllBookings()
        {
            var list = new List<Booking>();
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT * FROM bookings ORDER BY id DESC", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read()) list.Add(ReadBooking(reader));
            }
            return list;
        }

        public Booking GetBooking(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand("SELECT * FROM bookings WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return ReadBooking(reader);
                }
            }
            return null;
        }

        public void UpdateBookingStatus(int bookingId, string status)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "UPDATE bookings SET status = @status WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", bookingId);
                cmd.ExecuteNonQuery();
            }
        }

        private Booking ReadBooking(IDataRecord r)
        {
            return new Booking
            {
                Id = (int)r["id"],
                GuestName = r["guest_name"] as string,
                GuestPhone = r["guest_phone"] as string,
                RoomTypeId = (int)r["room_type_id"],
                RoomTypeName = r["room_type_name"] as string,
                RoomPrice = r["room_price"] == DBNull.Value ? 0 : (decimal)r["room_price"],
                CheckIn = r["check_in"] == DBNull.Value ? DateTime.MinValue : (DateTime)r["check_in"],
                CheckOut = r["check_out"] == DBNull.Value ? DateTime.MinValue : (DateTime)r["check_out"],
                Nights = r["nights"] == DBNull.Value ? 0 : (int)r["nights"],
                Guests = r["guests"] == DBNull.Value ? 0 : (int)r["guests"],
                TotalAmount = r["total_amount"] == DBNull.Value ? 0 : (decimal)r["total_amount"],
                Status = r["status"] as string,
                CreatedAt = r["created_at"] as string
            };
        }

        // -----------------------------------------------------------------
        // BILLS: publishing bills for a booking
        // -----------------------------------------------------------------

        public Bill GetBillForBooking(int bookingId)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT TOP 1 * FROM bills WHERE booking_id = @id ORDER BY id DESC", conn))
            {
                cmd.Parameters.AddWithValue("@id", bookingId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return ReadBill(reader);
                }
            }
            return null;
        }

        /// <summary>
        /// Creates (or returns the existing) bill for a booking and marks the
        /// booking as BILLED - mirrors DatabaseHelper.publishBill in the Android app.
        /// </summary>
        public Bill PublishBill(int bookingId, decimal taxRatePercent)
        {
            Bill existing = GetBillForBooking(bookingId);
            if (existing != null) return existing;

            Booking booking = GetBooking(bookingId);
            if (booking == null) return null;

            decimal subtotal = booking.TotalAmount;
            decimal tax = subtotal * (taxRatePercent / 100.0m);
            decimal grandTotal = subtotal + tax;

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "INSERT INTO bills (booking_id, subtotal, tax_amount, grand_total, generated_at, published) " +
                "VALUES (@bookingId, @subtotal, @tax, @grandTotal, @generatedAt, 1)", conn))
            {
                cmd.Parameters.AddWithValue("@bookingId", bookingId);
                cmd.Parameters.AddWithValue("@subtotal", subtotal);
                cmd.Parameters.AddWithValue("@tax", tax);
                cmd.Parameters.AddWithValue("@grandTotal", grandTotal);
                cmd.Parameters.AddWithValue("@generatedAt", Now());
                cmd.ExecuteNonQuery();
            }

            UpdateBookingStatus(bookingId, Booking.STATUS_BILLED);

            return GetBillForBooking(bookingId);
        }

        private Bill ReadBill(IDataRecord r)
        {
            return new Bill
            {
                Id = (int)r["id"],
                BookingId = (int)r["booking_id"],
                Subtotal = r["subtotal"] == DBNull.Value ? 0 : (decimal)r["subtotal"],
                TaxAmount = r["tax_amount"] == DBNull.Value ? 0 : (decimal)r["tax_amount"],
                GrandTotal = r["grand_total"] == DBNull.Value ? 0 : (decimal)r["grand_total"],
                GeneratedAt = r["generated_at"] as string,
                Published = (bool)r["published"]
            };
        }
    }
}
