<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingList.aspx.cs" Inherits="HotelMS.BookingList" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Bookings &amp; Billing - HotelMS</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section-header">
        <h1>Bookings &amp; Billing</h1>
        <div>
            <a class="btn btn-accent" href="~/Booking.aspx" runat="server">+ New Booking</a>
            <a class="back-link" href="~/Dashboard.aspx" runat="server">&larr; Dashboard</a>
        </div>
    </div>

    <div class="panel">
        <asp:GridView ID="gvBookings" runat="server" AutoGenerateColumns="false" CssClass="grid"
            GridLines="None" DataKeyNames="Id" OnRowCommand="gvBookings_RowCommand" Width="100%">
            <Columns>
                <asp:BoundField DataField="GuestName" HeaderText="Guest" />
                <asp:BoundField DataField="RoomTypeName" HeaderText="Room Type" />
                <asp:TemplateField HeaderText="Stay">
                    <ItemTemplate><%# Eval("CheckIn", "{0:dd MMM yyyy}") %> &rarr; <%# Eval("CheckOut", "{0:dd MMM yyyy}") %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Nights" HeaderText="Nights" />
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate><%# string.Format("₹{0:N2}", Eval("TotalAmount")) %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='pill <%# GetPillClass(Eval("Status").ToString()) %>'><%# Eval("Status") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkViewBill" runat="server" CommandName="ViewBill" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-outline">View Bill</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="empty-state">No bookings yet. Click "New Booking" to create one.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
