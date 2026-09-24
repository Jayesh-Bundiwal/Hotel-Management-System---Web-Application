<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="HotelMS.Register" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Create Account - HotelMS</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-wrap wide">
        <h1>Create a staff account</h1>
        <p class="subtitle">Register to start managing bookings and room pricing.</p>

        <asp:Panel ID="pnlMessage" runat="server" CssClass="message error" Visible="false">
            <asp:Literal ID="litMessage" runat="server" />
        </asp:Panel>

        <div class="field">
            <label for="<%= txtFullName.ClientID %>">Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server" placeholder="e.g. Priya Sharma" />
        </div>
        <div class="field">
            <label for="<%= txtEmail.ClientID %>">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="you@hotel.com" />
        </div>
        <div class="field">
            <label for="<%= txtPhone.ClientID %>">Phone</label>
            <asp:TextBox ID="txtPhone" runat="server" placeholder="+91 98765 43210" />
        </div>
        <div class="field">
            <label for="<%= txtUsername.ClientID %>">Username</label>
            <asp:TextBox ID="txtUsername" runat="server" placeholder="Choose a username" />
        </div>
        <div class="field">
            <label for="<%= txtPassword.ClientID %>">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Choose a password" />
        </div>
        <div class="field">
            <label for="<%= txtConfirmPassword.ClientID %>">Confirm Password</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" placeholder="Re-enter password" />
        </div>

        <asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="btn btn-primary btn-block" OnClick="btnRegister_Click" />

        <div class="footer-link">
            Already have an account? <a href="~/Login.aspx" runat="server">Sign in</a>
        </div>
    </div>
</asp:Content>
