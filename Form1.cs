using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.MonthCalendar;

namespace Lab6_OOAP_
{
    // Абстрактний клас Prototype, який слугує базовим класом для котеджів і апартаментів
    abstract class Prototype
    {
        // Властивість для зберігання площі
        public double Area { get; set; }

        // Властивість для зберігання кількості поверхів
        public int Floors { get; set; }

        // Властивість для зберігання адреси
        public string Address { get; set; }

        // Властивість для зберігання списку власників
        public List<string> Owners { get; set; }

        // Конструктор класу, який ініціалізує властивості
        protected Prototype(double area, int floors, string address, List<string> owners)
        {
            Area = area; // Присвоєння значення площі
            Floors = floors; // Присвоєння кількості поверхів
            Address = address; // Присвоєння адреси
            Owners = new List<string>(owners); // Копіювання списку власників
        }

        // Абстрактний метод для клону, який буде реалізований у похідних класах
        public abstract Prototype Clone();

        // Перевизначення методу ToString для виводу інформації про об'єкт
        public override string ToString()
        {
            // Форматування рядка для відображення інформації про будинок
            return $"Площа: {Area}, Кількість поверхів: {Floors}, Адреса: {Address}, Власники: {string.Join(", ", Owners)}";
        }
    }

    // Клас Cottage, який успадковує від класу Prototype
    class Cottage : Prototype
    {
        // Конструктор класу Cottage
        public Cottage(double area, int floors, string address, List<string> owners)
            : base(area, floors, address, owners) // Виклик базового конструктора
        {
        }

        // Реалізація методу Clone для створення копії котеджу
        public override Prototype Clone()
        {
            // Повертаємо новий об'єкт Cottage з поточними значеннями
            return new Cottage(Area, Floors, Address, new List<string>(Owners));
        }
    }

    // Клас Apartments, який також успадковує від класу Prototype
    class Apartments : Prototype
    {
        // Конструктор класу Apartments
        public Apartments(double area, int floors, string address, List<string> owners)
            : base(area, floors, address, owners) // Виклик базового конструктора
        {
        }

        // Реалізація методу Clone для створення копії апартаментів
        public override Prototype Clone()
        {
            // Повертаємо новий об'єкт Apartments з поточними значеннями
            return new Apartments(Area, Floors, Address, new List<string>(Owners));
        }
    }

    // Основний клас форми
    public partial class Form1 : Form
    {
        // Список для зберігання об'єктів типу Prototype (котеджі та апартаменти)
        private List<Prototype> houses;

        // Елементи управління для відображення списку будинків і кнопок
        private ListBox listBoxHouses;
        private Button buttonAddCottage;
        private Button buttonAddApartment;
        private Button buttonEdit;

        // Конструктор форми
        public Form1()
        {
            InitializeComponent(); // Ініціалізація компонентів форми
            InitializeControls(); // Виклик методу для налаштування контролів
            houses = new List<Prototype>(); // Ініціалізація списку будинків
        }

        // Метод для налаштування контролів форми
        private void InitializeControls()
        {
            // Створення нового ListBox для відображення будинків
            listBoxHouses = new ListBox { Location = new Point(20, 20), Size = new Size(400, 200) };

            // Створення кнопки для додавання котеджу
            buttonAddCottage = new Button { Text = "Додати котедж", Location = new Point(20, 230), Size = new Size(100, 50) };

            // Створення кнопки для додавання апартаментів
            buttonAddApartment = new Button { Text = "Додати апартаменти", Location = new Point(130, 230), Size = new Size(100, 50) };

            // Створення кнопки для редагування будинку
            buttonEdit = new Button { Text = "Редагувати будинок", Location = new Point(240, 230), Size = new Size(100, 50) };

            // Додавання обробників подій для кнопок
            buttonAddCottage.Click += (sender, e) => AddCottage(); // Додавання котеджу
            buttonAddApartment.Click += (sender, e) => AddApartment(); // Додавання апартаментів
            buttonEdit.Click += (sender, e) => EditHouse(); // Редагування будинку

            // Додавання елементів управління на форму
            Controls.Add(listBoxHouses);
            Controls.Add(buttonAddCottage);
            Controls.Add(buttonAddApartment);
            Controls.Add(buttonEdit);
        }

        // Метод для додавання котеджу
        private void AddCottage()
        {
            // Використання форми для введення даних про будинок
            using (var houseForm = new HouseForm())
            {
                // Перевірка, чи форма закрилася з результатом OK
                if (houseForm.ShowDialog() == DialogResult.OK)
                {
                    // Створення нового котеджу на основі введених даних
                    var cottage = new Cottage(houseForm.Area, houseForm.Floors, houseForm.Address, houseForm.Owners);

                    // Додавання нового котеджу до списку
                    houses.Add(cottage);

                    // Оновлення списку будинків на формі
                    UpdateHouseList();
                }
            }
        }

        // Метод для додавання апартаментів
        private void AddApartment()
        {
            // Використання форми для введення даних про будинок
            using (var houseForm = new HouseForm())
            {
                // Перевірка, чи форма закрилася з результатом OK
                if (houseForm.ShowDialog() == DialogResult.OK)
                {
                    // Створення нових апартаментів на основі введених даних
                    var apartment = new Apartments(houseForm.Area, houseForm.Floors, houseForm.Address, houseForm.Owners);

                    // Додавання нових апартаментів до списку
                    houses.Add(apartment);

                    // Оновлення списку будинків на формі
                    UpdateHouseList();
                }
            }
        }

        // Метод для редагування вибраного будинку
        private void EditHouse()
        {
            // Перевірка, чи вибрано будинок у списку
            if (listBoxHouses.SelectedItem is Prototype selectedHouse)
            {
                // Створення форми для редагування даних про будинок
                var houseForm = new HouseForm(selectedHouse.Area, selectedHouse.Floors, selectedHouse.Address, selectedHouse.Owners);

                // Перевірка, чи форма закрилася з результатом OK
                if (houseForm.ShowDialog() == DialogResult.OK)
                {
                    // Клонування об'єкта для збереження старих даних
                    Prototype clonedHouse = selectedHouse.Clone();

                    // Оновлення значень клону з новими даними
                    clonedHouse.Area = houseForm.Area;
                    clonedHouse.Floors = houseForm.Floors;
                    clonedHouse.Address = houseForm.Address;
                    clonedHouse.Owners = houseForm.Owners;

                    // Отримання індексу вибраного будинку у списку
                    var index = houses.IndexOf(selectedHouse);

                    // Оновлення списку будинків новим клоном
                    houses[index] = clonedHouse;

                    // Оновлення списку будинків на формі
                    UpdateHouseList();
                }
            }
            else
            {
                // Виведення повідомлення, якщо будинок не вибрано
                MessageBox.Show("Виберіть будинок для редагування.");
            }
        }

        // Метод для оновлення списку будинків у ListBox
        private void UpdateHouseList()
        {
            // Очищення поточного списку
            listBoxHouses.Items.Clear();

            // Додавання кожного будинку з списку до ListBox
            foreach (var house in houses)
            {
                listBoxHouses.Items.Add(house);
            }
        }
    }

    // Частина класу HouseForm, що успадковує клас Form для створення форми
    public partial class HouseForm : Form
    {
        // Властивості для зберігання інформації про будинок
        public double Area { get; set; } // Площа будинку
        public int Floors { get; set; } // Кількість поверхів
        public string Address { get; set; } // Адреса будинку
        public List<string> Owners { get; set; } // Список власників

        // Текстові поля для введення інформації
        private TextBox txtArea; // Поле для площі
        private TextBox txtFloors; // Поле для кількості поверхів
        private TextBox txtAddress; // Поле для адреси
        private TextBox txtOwners; // Поле для власників

        // Конструктор для нової форми HouseForm, з можливістю передати існуючих власників
        public HouseForm(List<string> existingOwners = null)
        {
            // Встановлення розмірів форми
            Width = 500;
            Height = 500;

            // Ініціалізація текстових полів з позиціями та ширинами
            txtArea = new TextBox { Location = new Point(300, 10), Width = 100 };
            txtFloors = new TextBox { Location = new Point(300, 40), Width = 100 };
            txtAddress = new TextBox { Location = new Point(300, 70), Width = 100 };
            txtOwners = new TextBox { Location = new Point(300, 100), Width = 100 };

            // Якщо існуючі власники передані, заповнюємо поле власників
            if (existingOwners != null)
            {
                txtOwners.Text = string.Join(", ", existingOwners); // Об'єднуємо список в текст
            }

            // Створення міток для кожного текстового поля
            Label lblArea = new Label { Text = "Площа (число):", Location = new Point(10, 10), Width = 200 };
            Label lblFloors = new Label { Text = "Кількість поверхів (число):", Location = new Point(10, 40), Width = 200 };
            Label lblAddress = new Label { Text = "Адреса (текст):", Location = new Point(10, 70), Width = 200 };
            Label lblOwners = new Label { Text = "Власники (текст через кому):", Location = new Point(10, 100), Width = 200 };

            // Створення кнопки OK
            Button btnOK = new Button { Text = "OK", Location = new Point(80, 130) };

            // Додавання обробника події для кнопки OK
            btnOK.Click += btnOK_Click;

            // Додавання міток та текстових полів до контролів форми
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

        // Конструктор для редагування існуючого будинку
        public HouseForm(double existingArea, int existingFloors, string existingAddress, List<string> existingOwners)
        {
            // Встановлення розмірів форми
            Width = 500;
            Height = 500;

            // Ініціалізація текстових полів з позиціями та ширинами
            txtArea = new TextBox { Location = new Point(300, 10), Width = 100 };
            txtFloors = new TextBox { Location = new Point(300, 40), Width = 100 };
            txtAddress = new TextBox { Location = new Point(300, 70), Width = 100 };
            txtOwners = new TextBox { Location = new Point(300, 100), Width = 100 };

            // Заповнення текстових полів існуючими значеннями
            txtArea.Text = existingArea.ToString(); // Перетворення площі в текст
            txtFloors.Text = existingFloors.ToString(); // Перетворення кількості поверхів в текст
            txtAddress.Text = existingAddress; // Заповнення адреси
            txtOwners.Text = string.Join(", ", existingOwners); // Об'єднання списку власників в текст

            // Створення міток для кожного текстового поля
            Label lblArea = new Label { Text = "Площа (число):", Location = new Point(10, 10), Width = 200 };
            Label lblFloors = new Label { Text = "Кількість поверхів (число):", Location = new Point(10, 40), Width = 200 };
            Label lblAddress = new Label { Text = "Адреса (текст):", Location = new Point(10, 70), Width = 200 };
            Label lblOwners = new Label { Text = "Власники (текст через кому):", Location = new Point(10, 100), Width = 200 };

            // Створення кнопки OK
            Button btnOK = new Button { Text = "OK", Location = new Point(80, 130) };

            // Додавання обробника події для кнопки OK
            btnOK.Click += btnOK_Click;

            // Додавання міток та текстових полів до контролів форми
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

        // Обробник події для кнопки OK
        private void btnOK_Click(object sender, EventArgs e)
        {
            double area; // Змінна для зберігання площі
            int floors; // Змінна для зберігання кількості поверхів

            // Спроба перетворення введених даних в числа
            if (double.TryParse(txtArea.Text, out area) && // Перевірка площі
                int.TryParse(txtFloors.Text, out floors) && // Перевірка кількості поверхів
                !string.IsNullOrWhiteSpace(txtAddress.Text) && // Перевірка адреси на порожнечу
                !string.IsNullOrWhiteSpace(txtOwners.Text)) // Перевірка власників на порожнечу
            {
                // Якщо всі дані коректні, присвоюємо значення властивостям
                Area = area; // Присвоєння площі
                Floors = floors; // Присвоєння кількості поверхів
                Address = txtAddress.Text; // Присвоєння адреси
                Owners = new List<string>(txtOwners.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)); // Розділення рядка власників на список

                DialogResult = DialogResult.OK; // Встановлення результату діалогу на OK
                Close(); // Закриття форми
            }
            else
            {
                // Якщо дані не коректні, показуємо повідомлення
                MessageBox.Show("Введіть коректні дані.");
            }
        }
    }
}