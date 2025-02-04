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
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        private readonly CarRentalDBEntities _db;

        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _db = new CarRentalDBEntities();
            _manageVehicleListing = manageVehicleListing;
        }

        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            this.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;
            //isEditMode = true;
            //_db = new CarRentalDBEntities();
            if (carToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalDBEntities();
                PopulateFields(carToEdit);
            }
        }

        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.Id.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVin.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicenseNum.Text = car.LicensePlateNumber;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Added Validation for make and model
                if (string.IsNullOrWhiteSpace(tbMake.Text) ||
                    string.IsNullOrWhiteSpace(tbModel.Text))
                {
                    MessageBox.Show("Please ensure that you provide a make and a model");
                }
                else
                {
                    //if(isEditMode == true)
                    if (isEditMode)
                    {
                        //Edit Code here
                        var id = int.Parse(lblId.Text);
                        var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                        car.Model = tbModel.Text;
                        car.Make = tbMake.Text;
                        car.VIN = tbVin.Text;
                        car.Year = int.Parse(tbYear.Text);
                        car.LicensePlateNumber = tbLicenseNum.Text;

                        //_db.SaveChanges();
                        //MessageBox.Show("Update Operation completed. Refresh Grid to see changes");
                        //Close();
                    }
                    else
                    {
                        // Added validation for make and model of cars being added
                        // Add Code here
                        var newCar = new TypesOfCar
                        {
                            LicensePlateNumber = tbLicenseNum.Text,
                            Make = tbMake.Text,
                            Model = tbModel.Text,
                            VIN = tbVin.Text,
                            Year = int.Parse(tbYear.Text),
                        };

                        _db.TypesOfCars.Add(newCar);                        
                    }
                    _db.SaveChanges();
                    _manageVehicleListing.PopulateGrid();
                    MessageBox.Show("Operation Completed. Refresh Grid to see changes");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
