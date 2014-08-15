using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.FileTypes;
using Yggdrasil.TableParsers;

namespace Yggdrasil.Dumpers
{
    public static class ItemCompoundDumper
    {
        public static void DumpToDirectory(GameDataManager game, string file)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file))) Directory.CreateDirectory(Path.GetDirectoryName(file));

            TBB itemNameFile = game.GetMessageFile("ItemName");
            string compoundFileName = game.GetParsedData<ItemCompoundParser>().FirstOrDefault().ParentTable.GetParent().Filename;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">");
            string style = "<style type=\"text/css\">table, th, td { border: 1px solid black; }</style>";
            builder.AppendFormat("<html>\n<head><title>Item Compounds</title><meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">{0}</head>\n<body>\n", style);

            builder.AppendFormat("Game title: {0}, ID: {1}<br><br>\nItem compound file: {2}, names file: {3}<br><br>\n",
                game.Header.GameTitle, game.Header.GameCode, Path.GetFileName(compoundFileName), Path.GetFileName(itemNameFile.Filename));

            builder.AppendLine("<table><tr>");

            builder.AppendLine(
                "<th>Number</th><th>Item 1</th><th>Item 2</th><th>Item 3</th><th>Item 4</th><th>Item 5</th><th>Count 1</th><th>Count 2</th><th>Count 3</th><th>Count 4</th><th>Count 5</th><th>Unk/Padding</th></tr>");

            foreach (ItemCompoundParser item in game.GetParsedData<ItemCompoundParser>().OrderBy(x => x.ItemNumber))
            {
                builder.AppendLine("<tr>");
                builder.AppendFormat("<td>{0} ({1})</td> <td>{2} ({3})</td> <td>{4} ({5})</td> <td>{6} ({7})</td> <td>0x{8:X4}</td> <td>0x{9:X4}</td>",
                    (itemNameFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemNumber - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;"),
                    item.ItemNumber,
                    (item.ItemCompound1 == 0 ? "---" : (itemNameFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemCompound1 - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;")),
                    item.ItemCompound1,
                    (item.ItemCompound2 == 0 ? "---" : (itemNameFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemCompound2 - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;")),
                    item.ItemCompound2,
                    (item.ItemCompound3 == 0 ? "---" : (itemNameFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemCompound3 - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;")),
                    item.ItemCompound3,
                    (item.ItemCompound4 == 0 ? "---" : (itemNameFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemCompound4 - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;")),
                    item.ItemCompound4,
                    (item.ItemCompound5 == 0 ? "---" : (itemNameFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemCompound5 - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;")),
                    item.ItemCompound5
                    );

                builder.AppendFormat("<td>{0}</td> <td>{1}</td> <td>{2}</td> <td>{3}</td> <td>{4}</td> <td>0x{5:X2}</td>",
                    item.ItemCount1, item.ItemCount2, item.ItemCount3, item.ItemCount4, item.ItemCount5, item.UnknownPadding);

                builder.AppendLine("</tr>\n");
            }
            builder.AppendLine("</table><br>");

            builder.AppendLine("</body></html>");

            StreamWriter writer = File.CreateText(file);
            writer.Write(builder.ToString());
            writer.Close();
        }
    }
}
