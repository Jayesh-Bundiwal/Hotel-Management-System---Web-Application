<<<<<<< HEAD
# HotelMS - ASP.NET Web Forms (.NET Framework)

This is a C# **ASP.NET Web Application (.NET Framework)** port of the original
HotelMS Android app. It reproduces the same features:

- **Staff accounts** - register and log in (session-based auth).
- **Room Pricing** - add, edit, delete room types (name, description, price/night, total rooms).
- **Bookings** - create a booking for a guest with a room type, dates and guest count,
  with a live nights / rate / estimated-total preview before confirming.
- **Bookings & Billing list** - view every booking and its status (Booked / Billed).
- **Bill / Invoice** - publish a bill for a booking (subtotal + tax + grand total) and print it.

## How to open and run

1. **Requirements**: Visual Studio 2022 (or 2019) with the **ASP.NET and web development**
   workload, and **SQL Server Express LocalDB** (installed automatically with that workload).
2. Open `HotelMS.sln` in Visual Studio.
3. Press **F5** (or Ctrl+F5) to run with IIS Express.
4. On first run, the app automatically creates a `HotelMSDb` database on your
   `(localdb)\MSSQLLocalDB` instance and seeds four sample room types
   (Standard, Deluxe, Suite, Executive Suite) - see `Global.asax.cs` /
   `DAL/DatabaseHelper.cs`.
5. Register a staff account on first visit, then log in.

## Project structure

```
HotelMS.sln
HotelMS/
  Models/            Plain C# model classes (User, RoomType, Booking, Bill)
  DAL/
    DatabaseHelper.cs   All data access (ADO.NET / SqlClient) - table creation,
                         seeding, CRUD for rooms/bookings/bills
    SessionManager.cs   Thin wrapper over ASP.NET Session state (logged-in user)
  Content/Site.css   Site-wide styling
  Site.Master        Shared header / navigation
  Login.aspx, Register.aspx
  Dashboard.aspx
  RoomPricing.aspx    Add/edit/delete room types
  Booking.aspx        Create a new booking
  BookingList.aspx    All bookings + link to each bill
  Bill.aspx           View / publish / print an invoice
  Web.config          LocalDB connection string + tax rate setting
```

## Notes

- The database connection string lives in `Web.config` under `HotelMSConnection`
  and points at `(localdb)\MSSQLLocalDB`. Change it if you want to use a full
  SQL Server instance instead.
- The tax rate used when publishing a bill is configurable via the
  `TaxRatePercent` app setting in `Web.config` (defaults to 12%).
- No external NuGet packages are required - everything uses the built-in
  `System.Data.SqlClient` and Web Forms server controls.
=======
# Hotel-Management-System---Web-Application
>>>>>>> a2712bed82dc2b346945df3da82e60f5289ae844
