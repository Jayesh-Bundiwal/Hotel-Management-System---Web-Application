<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bill.aspx.cs" Inherits="HotelMS.Bill" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Invoice - HotelMS</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section-header no-print">
        <h1>Invoice</h1>
        <a class="back-link" href="~/BookingList.aspx" runat="server">&larr; Bookings &amp; Billing</a>
    </div>

    <asp:Panel ID="pnlMessage" runat="server" CssClass="message error" Visible="false">
        <asp:Literal ID="litMessage" runat="server" />
    </asp:Panel>

    <asp:Panel ID="pnlBill" runat="server" CssClass="panel" style="max-width:520px;margin:0 auto;">
        <div class="bill-header">
            <h2>Hotel Management System</h2>
            <p>Invoice</p>
        </div>

        <div class="bill-line"><span>Guest</span><span><asp:Literal ID="litGuest" runat="server" /></span></div>
        <div class="bill-line"><span>Phone</span><span><asp:Literal ID="litPhone" runat="server" /></span></div>
        <div class="bill-line"><span>Room</span><span><asp:Literal ID="litRoom" runat="server" /></span></div>
        <div class="bill-line"><span>Stay</span><span><asp:Literal ID="litDates" runat="server" /></span></div>
        <div class="bill-line"><span>Nights</span><span><asp:Literal ID="litNights" runat="server" /></span></div>

        <div class="summary-box">
            <div class="summary-row"><span>Subtotal</span><span><asp:Literal ID="litSubtotal" runat="server" /></span></div>
            <div class="summary-row"><span><asp:Literal ID="litTaxLabel" runat="server" /></span><span><asp:Literal ID="litTax" runat="server" /></span></div>
            <div class="summary-row total"><span>Grand Total</span><span><asp:Literal ID="litGrandTotal" runat="server" /></span></div>
        </div>

        <p style="color:#6b7789;font-size:13px;"><asp:Literal ID="litGeneratedAt" runat="server" /></p>

        <div class="actions-row no-print">
            <asp:Button ID="btnPublish" runat="server" Text="Publish Bill" CssClass="btn btn-primary" OnClick="btnPublish_Click" />
            <asp:Button ID="btnPrint" runat="server" Text="Print / Share Bill" CssClass="btn btn-outline" UseSubmitBehavior="false" OnClientClick="window.print(); return false;" Visible="false" />
        </div>
    </asp:Panel>
</asp:Content>
