<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="HotelMS.Booking" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">New Booking - HotelMS</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section-header">
        <h1>New Booking</h1>
        <a class="back-link" href="~/Dashboard.aspx" runat="server">&larr; Dashboard</a>
    </div>

    <asp:Panel ID="pnlMessage" runat="server" CssClass="message error" Visible="false">
        <asp:Literal ID="litMessage" runat="server" />
    </asp:Panel>

    <div class="panel">
        <div class="form-grid">
            <div class="field">
                <label for="<%= txtGuestName.ClientID %>">Guest Name</label>
                <asp:TextBox ID="txtGuestName" runat="server" placeholder="e.g. Ananya Rao" />
            </div>
            <div class="field">
                <label for="<%= txtGuestPhone.ClientID %>">Guest Phone</label>
                <asp:TextBox ID="txtGuestPhone" runat="server" placeholder="e.g. 98765 43210" />
            </div>
            <div class="field">
                <label for="<%= ddlRoomType.ClientID %>">Room Type</label>
                <asp:DropDownList ID="ddlRoomType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Recalculate" />
            </div>
            <div class="field">
                <label for="<%= txtGuestCount.ClientID %>">Number of Guests</label>
                <asp:TextBox ID="txtGuestCount" runat="server" placeholder="1" />
            </div>
            <div class="field">
                <label for="<%= txtCheckIn.ClientID %>">Check-in Date</label>
                <asp:TextBox ID="txtCheckIn" runat="server" TextMode="Date" AutoPostBack="true" OnTextChanged="Recalculate" />
            </div>
            <div class="field">
                <label for="<%= txtCheckOut.ClientID %>">Check-out Date</label>
                <asp:TextBox ID="txtCheckOut" runat="server" TextMode="Date" AutoPostBack="true" OnTextChanged="Recalculate" />
            </div>
        </div>

        <div class="summary-box">
            <div class="summary-row"><span>Rate / night</span><span><asp:Literal ID="litRate" runat="server" Text="₹0.00" /></span></div>
            <div class="summary-row"><span>Nights</span><span><asp:Literal ID="litNights" runat="server" Text="0" /></span></div>
            <div class="summary-row total"><span>Estimated Total</span><span><asp:Literal ID="litEstimatedTotal" runat="server" Text="₹0.00" /></span></div>
        </div>

        <asp:Button ID="btnConfirmBooking" runat="server" Text="Confirm Booking" CssClass="btn btn-primary" OnClick="btnConfirmBooking_Click" />
    </div>
</asp:Content>
