﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TableListCommands
{
    public partial class TableRowInsert : Window
    {
        public String csvFileContent = string.Empty;
        public bool containsHeader = false;
        public String newContent = string.Empty;
        public char delimiter = ' ';
        public DataTable table = new DataTable();

        public TableRowInsert()
        {
            InitializeComponent();
        }
        private void GetLatestData()
        {
            List<String> rows = new List<string>();
            if (containsHeader)
            {
                List<String> firstRow = new List<string>();

                for (int i = 0; i < gridCSVContent.Columns.Count; i++)
                {
                    if (gridCSVContent.Columns[i].Visibility == System.Windows.Visibility.Visible)
                    {
                        firstRow.Add(gridCSVContent.Columns[i].Header.ToString().Replace(':',','));
                    }
                }
                if (firstRow.Count > 0)
                {
                    if (String.Join("", firstRow.ToString()).Trim().Length > 0)
                    {
                        rows.Add(String.Join(delimiter.ToString(), firstRow).Trim());
                    }
                }
            }
            for (int i = 0; i < gridCSVContent.Items.Count; i++)
            {
                string singleRow = String.Join(delimiter.ToString(), ((DataRowView)gridCSVContent.Items[i]).Row.ItemArray);
                rows.Add(singleRow);
            }
            newContent = String.Join(Environment.NewLine, rows);
        }
        
        private void frmTableCSVGrid_Loaded(object sender, RoutedEventArgs e)
        {
            table = new DataTable();
            
            if (csvFileContent != null && csvFileContent.Trim().Length > 1)
            {
                String[] rows = csvFileContent.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int columnCount = 0;
                for (int i = 0; i < rows.Length; i++)
                {
                    String[] columns = rows[i].Split(new char[] { delimiter });
                    //first row gives us info about how many columns are present and that is
                    //taken as a basis.
                    if (i == 0)
                    {
                        columnCount = columns.Length;
                        if (containsHeader) 
                        {
                            for (int j = 0; j < columnCount; j++) 
                            {
                                table.Columns.Add(columns[j].Trim().Replace(',', ':'), typeof(string));
                            }
                            continue;
                        }
                        else
                        {
                            for (int j = 0; j < columnCount; j++)
                            {
                                table.Columns.Add("Column: " + j.ToString(), typeof(string));
                            }
                        }
                    }
                    if (columns.Length <= columnCount)
                    {
                        DataRow drow = table.NewRow();
                        drow.ItemArray = columns;
                        table.Rows.Add(drow);
                    }
                    else 
                    {
                        if (columns.Length > columnCount) 
                        {
                            DataRow drow = table.NewRow();
                            String[] arr = new string[columnCount];
                            for (int k = 0; k < columnCount; k++)
                            {
                                arr[k] = columns[k];
                            }
                            drow.ItemArray = arr;
                            table.Rows.Add(drow);
                        }
                    }
                }
            }
            gridCSVContent.ItemsSource = table.DefaultView;
            gridCSVContent.AutoGenerateColumns = true;
            gridCSVContent.SelectionMode = DataGridSelectionMode.Single;
            gridCSVContent.SelectionUnit = DataGridSelectionUnit.FullRow;
            gridCSVContent.CanUserAddRows = false;
            gridCSVContent.CanUserResizeRows = false;
            gridCSVContent.IsReadOnly = true;
            gridCSVContent.CanUserReorderColumns = false;
            
        }

        private void btnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = (gridCSVContent.SelectedItems.Count - 1); i >= 0; i--)
                {
                    try
                    {
                        DataRowView row = (DataRowView)gridCSVContent.SelectedItems[i];
                        row.Row.Delete();
                    }
                    catch (Exception) { }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Row Delete Error: " + ex.Message);
                throw;
            }
            
        }

        private void frmTableCSVGrid_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GetLatestData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GetLatestData();
        }

        private void gridCSVContent_Sorting(object sender, DataGridSortingEventArgs e)
        {
          
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnInsertRow_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtRow.Text.Trim().Length == 0)
            {
                MessageBox.Show("Nothing to add");
            }
            else if (txtRow.Text.Trim().IndexOf(delimiter.ToString()) == -1)
            {
                MessageBox.Show("Delimiter not found");
            }
            else
            {
                DataView view = (DataView)gridCSVContent.ItemsSource;
                DataTable dt = view.ToTable();

                //Now split based on line
                string[] lines = txtRow.Text.Trim().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                int insertRowIndex = -1;
                if (gridCSVContent.SelectedIndex != -1)
                {
                    insertRowIndex = gridCSVContent.SelectedIndex;
                }

                foreach (string line in lines)
                {
                    DataRow drow = dt.NewRow();
                    //string[] items = txtRow.Text.Split(new char[] { delimiter });
                    string[] items = line.Trim().Split(new char[] { delimiter });
                    if (items.Length > table.Columns.Count)
                    {
                        List<string> itms = new List<string>();
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            itms.Add(items[i]);
                        }
                        drow.ItemArray = itms.ToArray();
                    }
                    else
                        drow.ItemArray = line.Trim().Split(new char[] { delimiter });
                    if (gridCSVContent.SelectedIndex == -1)
                    {
                        dt.Rows.InsertAt(drow, dt.Rows.Count);
                    }
                    else
                    {
                        dt.Rows.InsertAt(drow, insertRowIndex++);
                    }
                }
                gridCSVContent.ItemsSource = dt.DefaultView;
            }
        }
        
        public void MakeReadOnly(bool readOnly)
        {
            if (readOnly)
            {
                txtRow.Visibility = System.Windows.Visibility.Hidden;
                btnInsertRow.Visibility = System.Windows.Visibility.Hidden;
            }
            else 
            {
                txtRow.Visibility = System.Windows.Visibility.Visible;
                btnInsertRow.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
