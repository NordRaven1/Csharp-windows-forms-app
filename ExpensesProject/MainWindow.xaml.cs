using System;
using System.Collections.Generic;
using System.IO;
using ExpensesAndIncomes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Collections;

namespace ExpensesProject 
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string fileName = string.Empty;
        private string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }
        private string fileType = string.Empty;
        private string FileType
        {
            get
            {
                return this.fileType;
            }
            set
            {
                this.fileType = value;
            }
        }
        private int choices = 0;
        private int Choices
        {
            get
            {
                return this.choices;
            }
            set
            {
                this.choices = value;
            }
        }
        private List<Expenses> categoriesList = new List<Expenses>();
        private List<Expenses> CategoriesList
        {
            get
            {
                return this.categoriesList;
            }
        }
        private List<Income> incomesList = new List<Income>();
        private List<Income> IncomesList
        {
            get
            {
                return this.incomesList;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReadFile1Button_Click(object sender, RoutedEventArgs e)
        {
            if (Choices == 0)
            {
                FileName = GetFileName();
                if (FileName != string.Empty)
                {
                    FileStream sourceFile = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(FileName);
                    try
                    {
                        while (!reader.EndOfStream)
                        {
                            string data = reader.ReadLine();
                            string[] words = data.Split(' ');
                            if (FileType == "Expenses")
                            {
                                string category = words[0];
                                int amount = Convert.ToInt32(words[1]);
                                Expenses expenses = new Expenses(category, amount);
                                CategoriesList.Add(expenses);
                            }
                            else
                            {
                                if (words.Length == 2)
                                {
                                    string category = words[0];
                                    string month = words[1];
                                    Income income = new Income(category, month);
                                    IncomesList.Add(income);
                                }
                                else
                                {
                                    throw new FormatException();
                                }
                            }
                        }
                        Choices += 1;
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Выбранный файл содержит неподходящую информацию");
                        FileType = string.Empty;
                    }
                    reader.Close();
                    sourceFile.Close();
                }
            }
            else
            {
                MessageBox.Show("Вы уже выбирали файл 1");
            }
        }

        private void ReadFile2Button_Click(object sender, RoutedEventArgs e)
        {
            if (Choices > 0)
            {
                if(Choices == 1)
                {
                    FileName = GetFileName();
                    if (FileName != string.Empty)
                    {
                        FileStream sourceFile = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                        StreamReader reader = new StreamReader(FileName);
                        try
                        {
                            while (!reader.EndOfStream)
                            {
                                string data = reader.ReadLine();
                                string[] words = data.Split(' ');
                                string category = words[0];
                                string service = words[1];
                                int amount = Convert.ToInt32(words[2]);
                                if (FileType == "Expenses")
                                {
                                    foreach (Expenses expense in categoriesList)
                                    {
                                        if (expense.Title == category)
                                        {
                                            Service serv = new Service(service, amount);
                                            expense.ServicesList.Add(serv);
                                        }
                                    }
                                }
                                if (FileType == "Incomes")
                                {
                                    foreach (Income income in IncomesList)
                                    {
                                        if (income.Month == category)
                                        {
                                            Service serv = new Service(service, amount);
                                            income.ServicesList.Add(serv);
                                        }
                                    }
                                }
                            }
                            Choices += 1;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            MessageBox.Show("Выбранный файл содержит неподходящую информацию");
                        }
                        reader.Close();
                        sourceFile.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Вы уже выбирали файл 2");
                }
            }
            else
            {
                MessageBox.Show("Сначала необходимо выбрать файл 1");
            }   
        }

        private void WriteInfoButton_Click(object sender, RoutedEventArgs e)
        {
            infoBox.Clear();
            chartExpense.Children.Clear();
            if(Choices == 2)
            {
                switch (categoriesBox.Text)
                {
                    case "Общая информация":
                        if (FileType == "Expenses")
                        {
                            int realsum = 0;
                            int expectedsum = 0;
                            foreach (Expenses expense in CategoriesList)
                            {
                                infoBox.Text += $"{expense.Title} {expense.ExpectedExpenses} {expense.SumUpElements()} " +
                                    $"{expense.Comparison(expense.ExpectedExpenses)} Разница: {expense.Difference(0)}\n";
                                expectedsum += expense.ExpectedExpenses;
                                realsum += expense.SumUpElements();

                            }
                            DrawGraph(expectedsum, realsum);
                        }
                        if (FileType == "Incomes")
                        {
                            infoBox.Text += $"{IncomesList[0].Month} {IncomesList[0].SumUpElements()} {IncomesList[1].Month} " +
                                $"{IncomesList[1].SumUpElements()} {IncomesList[0].Comparison(IncomesList[1].SumUpElements())} " +
                                $"Разница: {IncomesList[0].Difference(IncomesList[1].SumUpElements())}\n";
                            DrawGraph(IncomesList[0].SumUpElements(), IncomesList[1].SumUpElements());
                        }
                        break;
                    case "Продукты":
                        try
                        {
                            InfoToWrite("Продукты");
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            MessageBox.Show("Эта категория предзначена для  расходов, а не начислений");
                        }
                        break;
                    case "Квартплата":
                        try
                        {
                            InfoToWrite("Квартплата");
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            MessageBox.Show("Эта категория предзначена для  расходов, а не начислений");
                        }
                        break;
                    case "Транспорт":
                        try
                        {
                            InfoToWrite("Транспорт");
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            MessageBox.Show("Эта категория предзначена для  расходов, а не начислений");
                        }
                        break;
                    case "Медицина":
                        try
                        {
                            InfoToWrite("Медицина");
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            MessageBox.Show("Эта категория предзначена для  расходов, а не начислений");
                        }
                        break;
                    case "Развлечения":
                        try
                        {
                            InfoToWrite("Развлечения");
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            MessageBox.Show("Эта категория предзначена для  расходов, а не начислений");
                        }
                        break;
                    case "Месяц 1":
                        try
                        {
                            InfoToWrite(IncomesList[0].Month);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            MessageBox.Show("Эта категория предзначена для начислений, а не расходов");
                        }
                        break;
                    case "Месяц 2":
                        try
                        {
                            InfoToWrite(IncomesList[1].Month);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            MessageBox.Show("Эта категория предзначена для начислений, а не расходов");
                        }
                        break;
                }
            }
            else
            {
                MessageBox.Show("Сначала необходимо выбрать оба файла");
            }
        }

        private string GetFileName()
        {
            string fname = string.Empty;

            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.InitialDirectory = Directory.GetParent(@"..\..\..\").FullName;
            openFileDlg.DefaultExt = ".txt";
            openFileDlg.Filter = "Text documents (.txt)|*.txt";

            bool? result = openFileDlg.ShowDialog();

            if (result == true)
            {
                fname = openFileDlg.FileName;
                if (Choices == 0)
                {
                    if (fname.Contains("Real") || fname.Contains("Expected"))
                    {
                        FileType = "Expenses";
                    }
                    else if (fname.Contains("Incomes"))
                    {
                        FileType = "Incomes";
                    }
                    else
                    {
                        MessageBox.Show("Вы выбрали файл, не относящийся к проекту");
                        fname = string.Empty;
                    }
                }
                else
                {
                    if ( ( (FileType == "Expenses") && (fname.Contains("Incomes")) ) || 
                        ( (FileType == "Incomes") && ( (fname.Contains("Real") || fname.Contains("Expected"))) ) )
                    {
                        MessageBox.Show("Нельзя выбрать этот файл - ранее вы выбрали и обработали файл другого типа");
                        fname = string.Empty;
                    }
                }
            }
            return fname;
        }

        private void DrawGraph(int expected, int real)
        {
            Orientation orientation = Orientation.Horizontal;

            double expectedProportion;
            double realProportion;

            BrushConverter bc = new BrushConverter();
            Brush bExpected = Brushes.Green;
            Brush bReal = Brushes.Red;

            Rectangle rExpected = new Rectangle();
            rExpected.Stroke = bExpected;
            rExpected.Fill = bExpected;
            rExpected.VerticalAlignment = VerticalAlignment.Bottom;
            rExpected.HorizontalAlignment = HorizontalAlignment.Left;

            Rectangle rReal = new Rectangle();
            rReal.Stroke = bReal;
            rReal.Fill = bReal;
            rReal.VerticalAlignment = VerticalAlignment.Bottom;
            rReal.HorizontalAlignment = HorizontalAlignment.Left;

            if (expected > real)
            {
                expectedProportion = 1;
                realProportion = (double)real / (double)expected;
            }
            else if (expected < real)
            {
                realProportion = 1;
                expectedProportion = (double)expected / (double)real;
            }
            else
            {
                expectedProportion = realProportion = 1;
            }

            chartExpense.Orientation = orientation;
            rExpected.Height = chartExpense.ActualHeight * expectedProportion;
            rReal.Height = chartExpense.ActualHeight * realProportion;
            rExpected.Width = chartExpense.ActualWidth / 2;
            rReal.Width = chartExpense.ActualWidth / 2;

            chartExpense.Children.Add(rExpected);
            chartExpense.Children.Add(rReal);
        }

        private Expenses FindCategoryE(string category)
        {
            int i = 0;
            foreach (Expenses expense in CategoriesList)
            {
                if (expense.Title == category)
                {
                    break;
                }
                i++;
            }
            return CategoriesList[i];
        }

        private Income FindCategoryI(string category)
        {
            int i = 0;
            foreach (Income income in IncomesList)
            {
                if (income.Month == category)
                {
                    break;
                }
                i++;
            }
            return IncomesList[i];
        }

        private void InfoToWrite(string category)
        {
            if (FileType == "Expenses")
            {
                infoBox.Text += $"{category}, Ожидание: {FindCategoryE(category).ExpectedExpenses} Реальность: {FindCategoryE(category).SumUpElements()} " +
                    $"Разница: {FindCategoryE(category).Difference(0)}\n";

                FindCategoryE(category).SortServicesList();

                for (int i = 0; i < FindCategoryE(category).ServicesList.Count; i++)
                {
                    infoBox.Text += $"{FindCategoryE(category)[i]}\n";
                }

                DrawGraph(FindCategoryE(category).ExpectedExpenses, FindCategoryE(category).SumUpElements());
            }
            else if (FileType == "Incomes")
            {
                infoBox.Text += $"{category}, Начисления: {FindCategoryI(category).SumUpElements()}\n";

                FindCategoryI(category).SortServicesList();

                for (int i = 0; i < FindCategoryI(category).ServicesList.Count; i++)
                {
                    infoBox.Text += $"{FindCategoryI(category)[i]}\n";
                }
            }
        }
    }
}