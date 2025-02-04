using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class MainWindow : Form
    {
        private readonly CarRentalDBEntities _db;
        private Login _login;
        public string _roleName;
        public User _user;
        
        public MainWindow()
        {
            InitializeComponent();
            _db = new CarRentalDBEntities();
        }

        public MainWindow(Login login, User user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleName = user.UserRoles.FirstOrDefault().Role.ShortName;
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddEditRentalRecord"))
            {
                var addRentalRecord = new AddEditRentalRecord();
                //addRentalRecord.ShowDialog();
                addRentalRecord.MdiParent = this;
                addRentalRecord.Show();
            }
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            var OpenForms = Application.OpenForms.Cast<Form>();
            var isOpen = OpenForms.Any(q => q.Name == "ManageVehicleListing");

            if (!isOpen)
            {
                var vehicleListing = new ManageVehicleListing();
                vehicleListing.MdiParent = this;
                vehicleListing.Show();
            }            
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var manageRentalRecords = new ManageRentalRecords();
            manageRentalRecords.MdiParent = this;
            manageRentalRecords.Show();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageRentalRecords"))
            { 
                var manageUser = new ManageUsers();
                manageUser.MdiParent = this;
                manageUser.Show();
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (_user.Password == Utils.DefaultHashedPassword())
            {
                var resetPassword = new ResetPassword(_user);
                resetPassword.ShowDialog();
            }

            var username = _user.Username;
            tsiLoginText.Text = $"Logged In As: {username}";
            if (_roleName != "admin")
            {
                manageUsersToolStripMenuItem.Enabled = false;// visible would be disapper the tag
            }
        }
    }
}
