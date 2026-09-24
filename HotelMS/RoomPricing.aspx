<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoomPricing.aspx.cs" Inherits="HotelMS.RoomPricing" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Room Pricing - HotelMS</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section-header">
        <h1>Room Pricing</h1>
        <div>
            <asp:Button ID="btnAddRoom" runat="server" Text="+ Add Room Type" CssClass="btn btn-accent" OnClick="btnAddRoom_Click" />
            <a class="back-link" href="~/Dashboard.aspx" runat="server">&larr; Dashboard</a>
        </div>
    </div>

    <asp:Panel ID="pnlMessage" runat="server" CssClass="message success" Visible="false">
        <asp:Literal ID="litMessage" runat="server" />
    </asp:Panel>

    <asp:Panel ID="pnlForm" runat="server" CssClass="panel" Visible="false">
        <h2><asp:Literal ID="litFormTitle" runat="server">Add Room Type</asp:Literal></h2>
        <asp:HiddenField ID="hfRoomId" runat="server" Value="0" />
        <div class="form-grid">
            <div class="field full">
                <label for="<%= txtName.ClientID %>">Room Name</label>
                <asp:TextBox ID="txtName" runat="server" placeholder="e.g. Deluxe" />
            </div>
            <div class="field full">
                <label for="<%= txtDescription.ClientID %>">Description</label>
                <asp:TextBox ID="txtDescription" runat="server" placeholder="e.g. Double bed, complimentary breakfast" />
            </div>
            <div class="field">
                <label for="<%= txtPrice.ClientID %>">Price per Night (&#8377;)</label>
                <asp:TextBox ID="txtPrice" runat="server" placeholder="e.g. 2800" />
            </div>
            <div class="field">
                <label for="<%= txtTotalRooms.ClientID %>">Total Rooms</label>
                <asp:TextBox ID="txtTotalRooms" runat="server" placeholder="e.g. 6" />
            </div>
        </div>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline" CausesValidation="false" OnClick="btnCancel_Click" />
    </asp:Panel>

    <div class="panel">
        <asp:GridView ID="gvRooms" runat="server" AutoGenerateColumns="false" CssClass="grid"
            GridLines="None" DataKeyNames="Id" OnRowCommand="gvRooms_RowCommand" Width="100%">
            <HeaderStyle CssClass="" />
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:TemplateField HeaderText="Price / Night">
                    <ItemTemplate><%# string.Format("₹{0:N2}", Eval("PricePerNight")) %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TotalRooms" HeaderText="Total Rooms" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditRoom" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-outline">Edit</asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteRoom" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-danger"
                            OnClientClick='<%# "return confirm(\"Remove \\\"" + Eval("Name") + "\\\" from room pricing?\");" %>'>Delete</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="empty-state">No room types yet. Click "Add Room Type" to create one.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
