using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageUsers : Form
    {
        private readonly CarRentalDBEntities _db;
       
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalDBEntities(); 
           
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
            
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                //get Id of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                //query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                //var genericPassword = "Password@123";
                var hashed_password = Utils.DefaultHashedPassword();
                user.Password = hashed_password;
                _db.SaveChanges();

                MessageBox.Show($"{user.Username}'s Password has been reset!");
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void btnDeactiveUser_Click(object sender, EventArgs e)
        {
            try
            {
                //get Id of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                //query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id); 
                
                //if (user.isActive == true)
                //    user.isActive = false;
                //else
                //    user.isActive = true;

                user.isActive = user.isActive == true ? false : true;
                _db.SaveChanges();

                MessageBox.Show($"{user.Username}'s active status has changed!");
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var users = _db.Users
                .Select( q => new 
                { 
                    q.id,
                    q.Username,
                    q.UserRoles.FirstOrDefault().Role.Name,
                    q.isActive
            
                })
                .ToList();
            gvUserList.DataSource = users;
            gvUserList.Columns["Username"].HeaderText = "User Name";
            gvUserList.Columns["Name"].HeaderText = "Role Name";
            gvUserList.Columns["isActive"].HeaderText = "Active";

            gvUserList.Columns["id"].Visible = false;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}
