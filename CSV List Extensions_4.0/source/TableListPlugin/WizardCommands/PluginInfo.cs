using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableListCommands
{
    public partial class PluginInfo
    {
        public static string HashCode
        {
            get
            {
                return "ae3f68f3d87465414d8ce7981c923f7719eb224a";
            }
        }
        public List<String> ToCellItems(String row, char delimiter)
        {
            List<String> cellItems = new List<string>();
            bool quoteStated = false;
            StringBuilder cell = new StringBuilder();
            foreach (char c in row)
            {
                if (c == '"')
                {
                    quoteStated = !quoteStated;
                    cell.Append(c);
                }
                else if (c == delimiter)
                {
                    if (quoteStated)
                    {
                        cell.Append(c);
                    }
                    else
                    {
                        cellItems.Add(cell.ToString());
                        cell.Clear();
                    }
                }
                else
                {
                    cell.Append(c);
                }
            }
            if (cell.ToString().Trim().Length > 0) 
            {
                cellItems.Add(cell.ToString().Trim());
                cell.Clear();
            }
            return cellItems;
        }
        public List<string> NormalizeString(String input)
        {
            List<string> rows = new List<string>();
            bool quoteStarted = false;
            StringBuilder row = new StringBuilder();
            foreach (char c in input)
            {
                //If char is quote and quote hasn't been quoted yet then start recording row content.
                if (c == '"')
                {
                    quoteStarted = !quoteStarted;
                    row.Append(c);
                }
                else if (c == '\n')
                {
                    if (quoteStarted)
                    {
                        row.Append(c);
                    }
                    else
                    {
                        row.Append(c);
                        if (row.ToString().Trim().Length > 0)
                        {
                            rows.Add(row.ToString());
                        }
                        row.Clear();
                    }
                }
                else
                {
                    row.Append(c);
                }
            }
            if (row.ToString().Trim().Length > 0)
            {
                rows.Add(row.ToString());
                row.Clear();
            }
            return rows;
        }
    }
}
