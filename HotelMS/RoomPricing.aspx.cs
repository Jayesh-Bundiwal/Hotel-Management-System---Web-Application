using System;
using HotelMS.DAL;
using HotelMS.Models;

namespace HotelMS
{
    public partial class RoomPricing : System.Web.UI.Page
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLoggedIn())
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadRooms();
            }
        }

        private void LoadRooms()
        {
            gvRooms.DataSource = db.GetAllRoomTypes();
            gvRooms.DataBind();
        }

        protected void btnAddRoom_Click(object sender, EventArgs e)
        {
            ShowForm(null);
        }

        protected void gvRooms_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRoom")
            {
                RoomType room = db.GetRoomType(id);
                ShowForm(room);
            }
            else if (e.CommandName == "DeleteRoom")
            {
                db.DeleteRoomType(id);
                LoadRooms();
                ShowMessage("Room type deleted.");
            }
        }

        private void ShowForm(RoomType existing)
        {
            pnlForm.Visible = true;
            if (existing == null)
            {
                litFormTitle.Text = "Add Room Type";
                hfRoomId.Value = "0";
                txtName.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtPrice.Text = string.Empty;
                txtTotalRooms.Text = string.Empty;
            }
            else
            {
                litFormTitle.Text = "Edit Room Type";
                hfRoomId.Value = existing.Id.ToString();
                txtName.Text = existing.Name;
                txtDescription.Text = existing.Description;
                txtPrice.Text = existing.PricePerNight.ToString("0.##");
                txtTotalRooms.Text = existing.TotalRooms.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string priceStr = txtPrice.Text.Trim();
            string totalStr = txtTotalRooms.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceStr))
            {
                ShowMessage("Name and price are required.", true);
                pnlForm.Visible = true;
                return;
            }

            decimal price;
            if (!decimal.TryParse(priceStr, out price))
            {
                ShowMessage("Enter a valid price.", true);
                pnlForm.Visible = true;
                return;
            }

            int total = string.IsNullOrEmpty(totalStr) ? 1 : Convert.ToInt32(totalStr);
            int id = Convert.ToInt32(hfRoomId.Value);

            var room = new RoomType
            {
                Id = id,
                Name = name,
                Description = description,
                PricePerNight = price,
                TotalRooms = total
            };

            if (id == 0)
            {
                db.AddRoomType(room);
                ShowMessage("Room type added.");
            }
            else
            {
                db.UpdateRoomType(room);
                ShowMessage("Room type updated.");
            }

            pnlForm.Visible = false;
            LoadRooms();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = false;
        }

        private void ShowMessage(string message, bool isError = false)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = isError ? "message error" : "message success";
            litMessage.Text = message;
        }
    }
}
