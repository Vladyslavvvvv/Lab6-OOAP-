using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.MonthCalendar;

namespace Lab6_OOAP_
{
    abstract class Prototype
    {
        public double Area { get; set; }
        public int Floors { get; set; }
        public string Address { get; set; }
        public List<string> Owners { get; set; }

        protected Prototype(double area, int floors, string address, List<string> owners)
        {
            Area = area;
            Floors = floors;
            Address = address;
            Owners = new List<string>(owners);
        }

        public abstract Prototype Clone();

        public override string ToString()
        {
            return $"Площа: {Area}, Кількість поверхів: {Floors}, Адреса: {Address}, Власники: {string.Join(", ", Owners)}";
        }
    }

    // Клас Cottage
    class Cottage : Prototype
    {
        public Cottage(double area, int floors, string address, List<string> owners)
            : base(area, floors, address, owners)
        {
        }

        public override Prototype Clone()
        {
            return new Cottage(Area, Floors, Address, new List<string>(Owners));
        }
    }

    // Клас Apartments
    class Apartments : Prototype
    {
        public Apartments(double area, int floors, string address, List<string> owners)
            : base(area, floors, address, owners)
        {
        }

        public override Prototype Clone()
        {
            return new Apartments(Area, Floors, Address, new List<string>(Owners));
        }
    }

    public partial class Form1 : Form
    {
        private List<Prototype> houses;
        private ListBox listBoxHouses;
        private Button buttonAddCottage;
        private Button buttonAddApartment;
        private Button buttonEdit;

        public Form1()
        {
            InitializeComponent();
            InitializeControls();
            houses = new List<Prototype>();
        }

        private void InitializeControls()
        {
            listBoxHouses = new ListBox { Location = new Point(20, 20), Size = new Size(400, 200) };
            buttonAddCottage = new Button { Text = "Додати котедж", Location = new Point(20, 230), Size = new Size(100, 50) };
            buttonAddApartment = new Button { Text = "Додати апартаменти", Location = new Point(130, 230), Size = new Size(100, 50) };
            buttonEdit = new Button { Text = "Редагувати будинок", Location = new Point(240, 230), Size = new Size(100, 50) };

            buttonAddCottage.Click += (sender, e) => AddCottage();
            buttonAddApartment.Click += (sender, e) => AddApartment();
            buttonEdit.Click += (sender, e) => EditHouse();

            Controls.Add(listBoxHouses);
            Controls.Add(buttonAddCottage);
            Controls.Add(buttonAddApartment);
            Controls.Add(buttonEdit);
        }

        private void AddCottage()
        {
            using (var houseForm = new HouseForm())
            {
                if (houseForm.ShowDialog() == DialogResult.OK)
                {
                    var cottage = new Cottage(houseForm.Area, houseForm.Floors, houseForm.Address, houseForm.Owners);
                    houses.Add(cottage);
                    UpdateHouseList();
                }
            }
        }

        private void AddApartment()
        {
            using (var houseForm = new HouseForm())
            {
                if (houseForm.ShowDialog() == DialogResult.OK)
                {
                    var apartment = new Apartments(houseForm.Area, houseForm.Floors, houseForm.Address, houseForm.Owners);
                    houses.Add(apartment);
                    UpdateHouseList();
                }
            }
        }

        private void EditHouse()
        {
            if (listBoxHouses.SelectedItem is Prototype selectedHouse)
            {
                var houseForm = new HouseForm(selectedHouse.Area, selectedHouse.Floors, selectedHouse.Address, selectedHouse.Owners);

                if (houseForm.ShowDialog() == DialogResult.OK)
                {
                    // Клонування об'єкта
                    Prototype clonedHouse = selectedHouse.Clone();
                    clonedHouse.Area = houseForm.Area; // Оновлення значень
                    clonedHouse.Floors = houseForm.Floors;
                    clonedHouse.Address = houseForm.Address;
                    clonedHouse.Owners = houseForm.Owners;

                    var index = houses.IndexOf(selectedHouse);
                    houses[index] = clonedHouse; // Оновлення списку
                    UpdateHouseList();
                }
            }
            else
            {
                MessageBox.Show("Виберіть будинок для редагування.");
            }
        }

        private void UpdateHouseList()
        {
            listBoxHouses.Items.Clear();
            foreach (var house in houses)
            {
                listBoxHouses.Items.Add(house);
            }
        }
    }

    public partial class HouseForm : Form
    {
        public double Area { get; set; }
        public int Floors { get; set; }
        public string Address { get; set; }
        public List<string> Owners { get; set; }

        private TextBox txtArea;
        private TextBox txtFloors;
        private TextBox txtAddress;
        private TextBox txtOwners;

        public HouseForm(List<string> existingOwners = null)
        {
            Width = 500;
            Height = 500;

            txtArea = new TextBox { Location = new Point(300, 10), Width = 100 };
            txtFloors = new TextBox { Location = new Point(300, 40), Width = 100 };
            txtAddress = new TextBox { Location = new Point(300, 70), Width = 100 };
            txtOwners = new TextBox { Location = new Point(300, 100), Width = 100 };

            if (existingOwners != null)
            {
                txtOwners.Text = string.Join(", ", existingOwners);
            }

            Label lblArea = new Label { Text = "Площа (число):", Location = new Point(10, 10), Width = 200 };
            Label lblFloors = new Label { Text = "Кількість поверхів (число):", Location = new Point(10, 40), Width = 200 };
            Label lblAddress = new Label { Text = "Адреса (текст):", Location = new Point(10, 70), Width = 200 };
            Label lblOwners = new Label { Text = "Власники (текст через кому):", Location = new Point(10, 100), Width = 200 };
            Button btnOK = new Button { Text = "OK", Location = new Point(80, 130) };

            btnOK.Click += btnOK_Click;

            Controls.Add(lblArea);
            Controls.Add(txtArea);
            Controls.Add(lblFloors);
            Controls.Add(txtFloors);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(lblOwners);
            Controls.Add(txtOwners);
            Controls.Add(btnOK);
        }

        public HouseForm(double existingArea, int existingFloors, string existingAddress, List<string> existingOwners)
        {
            Width = 500;
            Height = 500;

            txtArea = new TextBox { Location = new Point(300, 10), Width = 100 };
            txtFloors = new TextBox { Location = new Point(300, 40), Width = 100 };
            txtAddress = new TextBox { Location = new Point(300, 70), Width = 100 };
            txtOwners = new TextBox { Location = new Point(300, 100), Width = 100 };

            txtArea.Text = existingArea.ToString();
            txtFloors.Text = existingFloors.ToString();
            txtAddress.Text = existingAddress.ToString();
            txtOwners.Text = string.Join(", ", existingOwners);

            Label lblArea = new Label { Text = "Площа (число):", Location = new Point(10, 10), Width = 200 };
            Label lblFloors = new Label { Text = "Кількість поверхів (число):", Location = new Point(10, 40), Width = 200 };
            Label lblAddress = new Label { Text = "Адреса (текст):", Location = new Point(10, 70), Width = 200 };
            Label lblOwners = new Label { Text = "Власники (текст через кому):", Location = new Point(10, 100), Width = 200 };
            Button btnOK = new Button { Text = "OK", Location = new Point(80, 130) };

            btnOK.Click += btnOK_Click;

            Controls.Add(lblArea);
            Controls.Add(txtArea);
            Controls.Add(lblFloors);
            Controls.Add(txtFloors);
            Controls.Add(lblAddress);
            Controls.Add(txtAddress);
            Controls.Add(lblOwners);
            Controls.Add(txtOwners);
            Controls.Add(btnOK);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double area;
            int floors;

            if (double.TryParse(txtArea.Text, out area) &&
                int.TryParse(txtFloors.Text, out floors) &&
                !string.IsNullOrWhiteSpace(txtAddress.Text) &&
                !string.IsNullOrWhiteSpace(txtOwners.Text))
            {
                Area = area;
                Floors = floors;
                Address = txtAddress.Text;
                Owners = new List<string>(txtOwners.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Введіть коректні дані.");
            }
        }
    }
}