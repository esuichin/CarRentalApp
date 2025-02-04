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
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;       
        private readonly CarRentalDBEntities _db;
        public AddEditRentalRecord()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental Record";
            this.Text = "Add New Rental Record";
            isEditMode = false;            
            _db = new CarRentalDBEntities();
        }

        public AddEditRentalRecord(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";           
            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalDBEntities();
                PopulateFields(recordToEdit);
            }            
        }

        private void PopulateFields(CarRentalRecord recordToEdit)
        {
            tbCustomerName.Text = recordToEdit.CustomerName;
            dtRented.Value = (DateTime)recordToEdit.DateRented;
            dtReturned.Value = (DateTime)recordToEdit.DateReturned;
            tbCost.Text = recordToEdit.Cost.ToString();
            lblRecordId.Text = recordToEdit.id.ToString();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {            
                string customerName = tbCustomerName.Text;
                var dateOut = dtRented.Value;
                var dateIn = dtReturned.Value;
                double cost = Convert.ToDouble(tbCost.Text);

                var carType = cbTypeOfCar.SelectedItem.ToString();
                var isValid = true;
                var errorMessage = "";

                if(string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    //MessageBox.Show("Please enter missing data.");
                    errorMessage += "Error: Please enter missing data.\n\r";
                }

                if (dateOut > dateIn)
                {   
                    isValid = false;
                    //MessageBox.Show("Illegal Date Selection");
                    errorMessage += "Error: Illegal Date Selection\n\r";
                }

                // if(isValid == true)
                if (isValid)
                {
                    //Declare an object of the record to be added
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        //If in edit mode, then get the ID and retrieve the record from the database and place
                        //the result in the record object
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    }
                    //Populate record object with values from the form
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = dateOut;
                    rentalRecord.DateReturned = dateIn;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarId = (int)cbTypeOfCar.SelectedValue;
                    //If not in edit mode, then add the record object to the database
                    if (!isEditMode)
                    {
                        _db.CarRentalRecords.Add(rentalRecord);
                    }
                    //Save changes made to the entity
                    _db.SaveChanges();

                    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                        $"Date Rented: {dateOut}\n\r" +
                        $"Date Returned: {dateIn}\n\r" +
                        $"Cost: {cost}\n\r" +
                        $"Car Type: {carType}\n\r" +
                        $"Thank you for your business");

                    Close();

                }
                    //else
                    //{
                    //    var rentalRecord = new CarRentalRecord();
                    //    rentalRecord.CustomerName = customerName;
                    //    rentalRecord.DateRented = dateOut;
                    //    rentalRecord.DateReturned = dateIn;
                    //    rentalRecord.Cost = (decimal)cost;
                    //    rentalRecord.TypeOfCarId = (int)cbTypeOfCar.SelectedValue;

                    //    _db.CarRentalRecords.Add(rentalRecord);
                    //    _db.SaveChanges();

                    //    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                    //    $"Date Rented: {dateOut}\n\r" +
                    //    $"Date Returned: {dateIn}\n\r" +
                    //    $"Cost: {cost}\n\r" +
                    //    $"Car Type: {carType}\n\r" +
                    //    $"Thank you for your business");
                    //}
                
                else 
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Select * from TypesOfCars
            //var cars = carRentalDBEntities.TypesOfCars.ToList();

            var cars = _db.TypesOfCars
                .Select(q => new { Id = q.Id, Name = q.Make + " " + q.Model})
                .ToList();
            cbTypeOfCar.DisplayMember = "Name";
            cbTypeOfCar.ValueMember = "Id";
            cbTypeOfCar.DataSource = cars;
        }
        
    }
}
