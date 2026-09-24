<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HotelMS.Login" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Login - HotelMS</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-wrap">
        <h1>Welcome back</h1>
        <p class="subtitle">Sign in to manage bookings, room pricing and bills.</p>

        <asp:Panel ID="pnlMessage" runat="server" CssClass="message error" Visible="false">
            <asp:Literal ID="litMessage" runat="server" />
        </asp:Panel>

        <div class="field">
            <label for="<%= txtUsername.ClientID %>">Username</label>
            <asp:TextBox ID="txtUsername" runat="server" placeholder="Enter your username" />
        </div>
        <div class="field">
            <label for="<%= txtPassword.ClientID %>">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter your password" />
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />

        <div class="footer-link">
            Don't have an account? <a href="~/Register.aspx" runat="server">Create one</a>
        </div>
    </div>
</asp:Content>
