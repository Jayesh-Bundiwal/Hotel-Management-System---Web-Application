<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="HotelMS.Dashboard" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Dashboard - HotelMS</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="welcome">Welcome, <asp:Literal ID="litUserName" runat="server" /></div>
    <div class="welcome-sub">What would you like to do today?</div>

    <div class="card-grid">
        <a class="dash-card" href="~/Booking.aspx" runat="server">
            <div class="icon">&#128198;</div>
            <h3>New Booking</h3>
            <p>Book a room for a guest with dates, guest count and live pricing.</p>
        </a>
        <a class="dash-card" href="~/BookingList.aspx" runat="server">
            <div class="icon">&#128203;</div>
            <h3>Bookings &amp; Billing</h3>
            <p>View all bookings and publish or share an invoice.</p>
        </a>
        <a class="dash-card" href="~/RoomPricing.aspx" runat="server">
            <div class="icon">&#127976;</div>
            <h3>Room Pricing</h3>
            <p>Add, edit or remove room types and their nightly rates.</p>
        </a>
    </div>
</asp:Content>
