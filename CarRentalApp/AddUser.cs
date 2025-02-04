using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddUser : Form
    {
        private readonly CarRentalDBEntities _db;
        private ManageUsers _manageUsers;

        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            _db = new CarRentalDBEntities();            
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();                
            cbRoles.DataSource = roles;
            cbRoles.ValueMember = "id";
            cbRoles.DisplayMember = "name";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var username = tbUsername.Text;
                var roleId = (int)cbRoles.SelectedValue;
                var password = Utils.DefaultHashedPassword();
                var user = new User
                {
                    Username = username,
                    Password = password,
                    isActive = true
                };
                _db.Users.Add(user);
                _db.SaveChanges();

                var userId = user.id;
                var userRole = new UserRole
                {
                    RoleId = roleId,
                    UserId = userId,
                };

                _db.UserRoles.Add(userRole);
                _db.SaveChanges();

                MessageBox.Show("New User Added Successfully");
                _manageUsers.PopulateGrid();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error has occured");
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
