using Syncfusion.UI.Xaml.CellGrid;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TemplateDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Random random = new Random();
        public MainPage()
        {
            this.InitializeComponent();
            grid.RowCount = 15;
            grid.ColumnCount = 6;
            grid.ColumnWidths[0] = 35;
            grid.Model.QueryCellInfo += this.Model_QueryCellInfo;
        }
        private void Model_QueryCellInfo(object sender, Syncfusion.UI.Xaml.CellGrid.Styles.GridQueryCellInfoEventArgs e)
        {
            if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 0)
            {
                if (e.Cell.ColumnIndex == 1)
                    e.Style.CellValue = name1[e.Cell.RowIndex % 6];
                else if (e.Cell.ColumnIndex == 2)
                    e.Style.CellValue = country[e.Cell.RowIndex % 6];
                else if (e.Cell.ColumnIndex == 3)
                    e.Style.CellValue = city[e.Cell.RowIndex % 6];
                else if (e.Cell.ColumnIndex == 4)
                    e.Style.CellValue = scountry[e.Cell.RowIndex % 6];
                else if (e.Cell.ColumnIndex == 5)
                    e.Style.CellValue = DateTime.Now;
            }
            if (e.Cell.ColumnIndex == 0 && e.Cell.RowIndex > 0)
            {
                e.Style.CellValue = e.Style.RowIndex;
            }
            if (e.Cell.RowIndex == 0 && e.Cell.ColumnIndex > 0)
            {
                e.Style.CellValue = columnNames[e.Cell.ColumnIndex - 1];
            }
            if (e.Cell.RowIndex == 0 || e.Style.ColumnIndex == 0)
            {
                e.Style.HorizontalAlignment = HorizontalAlignment.Center;
                e.Style.Font.FontFamily = new FontFamily("Segoe UI");
                e.Style.Font.FontSize = 14f;
                e.Style.Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        private async void ExcelExport_Click(object sender, RoutedEventArgs e)
        {
            //Convert to excel
            ExcelEngine excelEngine = new ExcelEngine();
            excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2016;
            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Create(1);

            IWorksheet sheet = workbook.Worksheets[0];

            for (int i = 0; i < grid.RowCount; i++)
            {
                for (int j = 0; j < grid.ColumnCount; j++)
                {
                    IRange range = sheet[i + 1, j + 1];
                    var style = grid.Model[i, j];
                    var brush = (style.Background as SolidColorBrush);
                    //Export with style
                    if (brush != null)
                        range.CellStyle.Color = brush.Color;
                    range.Value = style.CellValue.ToString();
                }
            }
            StorageFile storageFile;
            StorageFolder local = ApplicationData.Current.LocalFolder;
            storageFile = await local.CreateFileAsync("Sample.xlsx", CreationCollisionOption.ReplaceExisting);
            await workbook.SaveAsAsync(storageFile);
            Windows.System.Launcher.LaunchFileAsync(storageFile);
           
        }
        #region "DataTable"
        string[] name1 = new string[] { "John", "Peter", "Smith", "Jay", "Krish", "Mike" };
        string[] country = new string[] { "UK", "USA", "Pune", "India", "China", "England" };
        string[] city = new string[] { "Graz", "Resende", "Bruxelles", "Aires", "Rio de janeiro", "Campinas" };
        string[] scountry = new string[] { "Brazil", "Belgium", "Austria", "Argentina", "France", "Beiging" };
        string[] columnNames = new string[] { "Name", "Country", "City", "Scountry", "Date" };
    }
    #endregion

}