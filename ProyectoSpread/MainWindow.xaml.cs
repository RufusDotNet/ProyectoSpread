using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GrapeCity.Windows.SpreadSheet.Data;

namespace ProyectoSpread
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitUI();
            InitializeSpread();
        }

        private void InitUI()
        {
            this.btnSearch.Content = ProyectoSpread.Properties.Resources.Search;
            this.btnClear.Content = ProyectoSpread.Properties.Resources.Clear;
            this.btnUpdate.Content = ProyectoSpread.Properties.Resources.Update;
            this.btnDelete.Content = ProyectoSpread.Properties.Resources.Delete;
            this.btnAdd.Content = ProyectoSpread.Properties.Resources.Add;
        }
        private void InitializeSpread()
        {
            this.gcSpreadSheet1.ValueChanged += new System.EventHandler<GrapeCity.Windows.SpreadSheet.UI.CellEventArgs>(gcSpreadSheet1_ValueChanged);
            this.gcSpreadSheet1.TabStripVisibility = System.Windows.Visibility.Collapsed;
            this.gcSpreadSheet1.AutoClipboard = false;
            this.gcSpreadSheet1.CanCellOverflow = false;
            this.gcSpreadSheet1.CanUserDragFill = false;
            this.gcSpreadSheet1.CanUserDragDrop = false;
            this.gcSpreadSheet1.ColumnSplitBoxPolicy = GrapeCity.Windows.SpreadSheet.UI.SplitBoxPolicy.Never;
            this.gcSpreadSheet1.RowSplitBoxPolicy = GrapeCity.Windows.SpreadSheet.UI.SplitBoxPolicy.Never;
            var sheet = this.gcSpreadSheet1.ActiveSheet;
            sheet.SelectionPolicy = SelectionPolicy.Single;
            sheet.SelectionUnit = SelectionUnit.Row;
            sheet.DataSource = getDataSource();
            sheet.AddSelection(0, 0, 1, 1);
            sheet.Columns[0].Locked = false;
            sheet.Columns[1].Locked = false;
            sheet.Columns[2].Locked = false;
            sheet.Columns[3].Locked = false;
            sheet.Columns[4].Locked = false;
            sheet.Columns[0].Width = 100;
            sheet.Columns[1].Width = 100;
            sheet.Columns[2].Width = 200;
            sheet.Columns[3].Width = 100;
            sheet.Columns[4].Width = 300;
            sheet.RowFilter = new HideRowFilter(new CellRange(-1, -1, -1, -1));
            sheet.Protect = true;
        }

        void gcSpreadSheet1_ValueChanged(object sender, GrapeCity.Windows.SpreadSheet.UI.CellEventArgs e)
        {
            if (this.gcSpreadSheet1.ActiveSheet.Rows[e.Row].Tag != null) return;
            this.gcSpreadSheet1.ActiveSheet.Rows[e.Row].Background = new SolidColorBrush(Color.FromArgb(30, 0, 0, 255));
            this.gcSpreadSheet1.ActiveSheet.Rows[e.Row].Tag = "Edit";
            btnUpdate.IsEnabled = true;
        }

        private Employee[] getDataSource()
        {
            return new Employee[] {
                new Employee(){ LastName="Freehafer",   FirstName="Nancy",  Title="Sales Representative", Phone="(123)555-0100", Email="nancy@northwindtraders.com"},
                new Employee(){ LastName="Cencini", FirstName="Andrew", Title="Vice President, Sales", Phone="(123)555-0100", Email="andrew@northwindtraders.com"},
                new Employee(){ LastName="Kotas",   FirstName="Jan",    Title="Sales Representative", Phone="(123)555-0100", Email="jan@northwindtraders.com"},
                new Employee(){ LastName="Sergienko",   FirstName="Mariya", Title="Sales Representative", Phone="(123)555-0100", Email="mariya@northwindtraders.com"},
                new Employee(){ LastName="Thorpe",  FirstName="Steven", Title="Sales Manager", Phone="(123)555-0100", Email="steven@northwindtraders.com"},
                new Employee(){ LastName="Neipper", FirstName="Michael",    Title="Sales Representative", Phone="(123)555-0100", Email="michael@northwindtraders.com"},
                new Employee(){ LastName="Zare",    FirstName="Robert", Title="Sales Representative", Phone="(123)555-0100", Email="robert@northwindtraders.com"},
                new Employee(){ LastName="Giussani",    FirstName="Laura",  Title="Sales Coordinator", Phone="(123)555-0100", Email="laura@northwindtraders.com"},
                new Employee(){ LastName="Hellung-Larsen",  FirstName="Anne",   Title="Sales Representative", Phone="(123)555-0100", Email="anne@northwindtraders.com"},
            };
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var sheet = this.gcSpreadSheet1.ActiveSheet;
            sheet.RowCount = sheet.RowCount + 1;
            sheet.Rows[sheet.RowCount - 1].Background = new SolidColorBrush(Color.FromArgb(30, 0, 255, 0));
            sheet.Rows[sheet.RowCount - 1].Tag = "New";
            sheet.AddSelection(sheet.RowCount - 1, 0, 1, 1);
            btnUpdate.IsEnabled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var sheet = this.gcSpreadSheet1.ActiveSheet;
            sheet.ConditionalFormats.ClearRule();
            sheet.ConditionalFormats.AddSpecificTextRule(TextComparisonOperator.Contains, this.txtSearch.Text,
                new StyleInfo() { Foreground = new SolidColorBrush(Colors.Red), FontWeight = FontWeights.Bold },
                new CellRange(0, 0, sheet.RowCount, sheet.ColumnCount));
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var sheet = this.gcSpreadSheet1.ActiveSheet;
            for (int i = sheet.RowCount - 1; i >= 0; i--)
            {
                if (sheet.Rows[i].Tag != null)
                {
                    if (sheet.Rows[i].Tag.ToString() == "Delete")
                    {
                        sheet.RemoveRows(i, 1);
                        continue;
                    }
                    else
                    {
                        sheet.Rows[i].ResetBackground();
                        sheet.Rows[i].Tag = null;
                    }
                }
            }
            if (sheet.ActiveRowIndex == -1 && sheet.RowCount > 0) sheet.AddSelection(sheet.RowCount - 1, 0, 1, 1);
            btnUpdate.IsEnabled = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var sheet = this.gcSpreadSheet1.ActiveSheet;
            if (sheet.ActiveRowIndex == -1) return;
            sheet.Rows[sheet.ActiveRowIndex].Background = new SolidColorBrush(Color.FromArgb(30, 255, 0, 0));
            sheet.Rows[sheet.ActiveRowIndex].Tag = "Delete";
            btnUpdate.IsEnabled = true;
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            var sheet = this.gcSpreadSheet1.ActiveSheet;
            sheet.ConditionalFormats.ClearRule();
        }
    }

    public class Employee
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}

