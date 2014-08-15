using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.FileTypes;
using Yggdrasil.TableParsers;

namespace Yggdrasil.Dumpers
{
    public static class ItemDataDumper
    {
        public static void DumpToDirectory(GameDataManager game, string file)
        {
            if (!Directory.Exists(Path.GetDirectoryName(file))) Directory.CreateDirectory(Path.GetDirectoryName(file));

            TBB itemNameFile = game.GetMessageFile("ItemName");
            TBB itemInfoFile = game.GetMessageFile("ItemInfo");

            string equipFileName = game.GetParsedData<EquipItemParser>().FirstOrDefault().ParentTable.GetParent().Filename;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">");
            string style = "<style type=\"text/css\">table, th, td { border: 1px solid black; }</style>";
            builder.AppendFormat("<html>\n<head><title>Item Data</title><meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">{0}</head>\n<body>\n", style);

            builder.AppendFormat("Game title: {0}, ID: {1}<br><br>\nItem data file: {2}, names file: {3}, info file: {4}<br><br>\n",
                game.Header.GameTitle, game.Header.GameCode, Path.GetFileName(equipFileName), Path.GetFileName(itemNameFile.Filename), Path.GetFileName(itemInfoFile.Filename));

            builder.AppendLine("Table #0; equipable items:<br><br>\n");
            builder.AppendLine("<table><tr>");

            builder.AppendLine(
                "<th rowspan=\"2\">Number</th> <th rowspan=\"2\">Name</th> <th rowspan=\"2\">Description</th> <th rowspan=\"2\">(Type/ Element?)</th> <th rowspan=\"2\">Atk</th> <th rowspan=\"2\">(Atk Alt?)</th> " +
                "<th rowspan=\"2\">Def</th> <th rowspan=\"2\">Group</th> <th rowspan=\"2\">Unk 1</th>");

            builder.AppendLine("<th colspan=\"11\">Resist</th> <th colspan=\"8\">Modifier</th>");

            builder.AppendLine("<th rowspan=\"2\">Unk 2</th> <th rowspan=\"2\">Buy Price</th> <th rowspan=\"2\">Sell Price</th> <th rowspan=\"2\">(Class Usability?)</th> <th rowspan=\"2\">Unk 3</th>");
            builder.AppendLine("</tr><tr>");

            builder.AppendLine("<th>Slash</th> <th>Blunt</th> <th>Pierce</th> <th>Fire</th> <th>Ice</th> <th>Volt</th>");
            builder.AppendLine("<th>Death</th> <th>Ailment</th> <th>Head Bind</th> <th>Arm Bind</th> <th>Leg Bind</th>");

            builder.AppendLine("<th>Str</th> <th>Vit</th> <th>Agi</th> <th>Luc</th> <th>Tec</th> <th>HP</th> <th>TP</th> <th>Boost</th></tr>");

            foreach (EquipItemParser item in game.GetParsedData<EquipItemParser>().OrderBy(x => x.ItemNumber))
            {
                string name = (itemNameFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemNumber - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                string description = (itemInfoFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemNumber - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;");

                builder.AppendLine("<tr>");
                builder.AppendFormat("<td>{0} (0x{0:X})</td> <td>{1}</td> <td>{2}</td> <td>0x{3:X4}</td> <td>{4}</td> <td>{5}</td> <td>{6}</td> <td>{7}</td> <td>{8}</td>",
                    item.ItemNumber, name, description, item.ItemType, item.Attack, item.AttackAlt, item.Defense, item.Group, item.Unknown1);

                builder.AppendFormat("<td>{0}%</td> <td>{1}%</td> <td>{2}%</td> <td>{3}%</td> <td>{4}%</td> <td>{5}%</td>",
                    -item.ResistSlash, -item.ResistBlunt, -item.ResistPierce, -item.ResistFire, -item.ResistIce, -item.ResistVolt);
                builder.AppendFormat("<td>{0}%</td> <td>{1}%</td> <td>{2}%</td> <td>{3}%</td> <td>{4}%</td>",
                    -item.ResistDeath, -item.ResistAilment, -item.ResistHeadBind, -item.ResistArmBind, -item.ResistLegBind);

                builder.AppendFormat("<td>{0}</td> <td>{1}</td> <td>{2}</td> <td>{3}</td> <td>{4}</td> <td>{5}</td> <td>{6}</td> <td>{7}</td>",
                    item.StrModifier, item.VitModifier, item.AgiModifier, item.LucModifier, item.TecModifier, item.HPModifier, item.TPModifier, item.BoostModifier);

                builder.AppendFormat("<td>{0}</td> <td>{1} en</td> <td>{2} en</td> <td>0x{3:X4}</td> <td>{4}</td>\n",
                    item.Unknown2, item.BuyPrice, item.SellPrice, item.ClassUsability, item.Unknown3);
                builder.AppendLine("</tr>\n");
            }
            builder.AppendLine("</table><br>");

            builder.AppendLine("Table #1; misc. items:<br><br>\n");
            builder.AppendLine("<table><tr>");
            builder.AppendLine(
                "<th>Number</th> <th>Name</th> <th>Description</th> " +
                "<th>Unk 1</th> <th>Unk 2</th> <th>Recovered HP</th> <th>Recovered TP</th> <th>Unk 3</th> <th>Unk 4</th> <th>Unk 5</th> <th>Buy Price</th> <th>Sell Price</th>");

            foreach (MiscItemParser item in game.GetParsedData<MiscItemParser>().OrderBy(x => x.ItemNumber))
            {
                string name = (itemNameFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemNumber - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;");
                string description = (itemInfoFile.Tables.FirstOrDefault() as TBB.MTBL).Messages[item.ItemNumber - 1].ConvertedString.Replace("\n", "<br>").Replace(" ", "&nbsp;");

                builder.AppendLine("<tr>");
                builder.AppendFormat("<td>{0} (0x{0:X})</td> <td>{1}</td> <td>{2}</td> <td>0x{3:X4}</td> <td>0x{4:X4}</td> <td>{5}</td> <td>{6}</td> <td>0x{7:X4}</td> <td>0x{8:X4}</td>",
                    item.ItemNumber, name, description, item.Unknown1, item.Unknown2, item.RecoveredHP, item.RecoveredTP, item.Unknown3, item.Unknown4);

                builder.AppendFormat("<td>0x{0:X4}</td> <td>{1} en</td> <td>{2} en</td>",
                    item.Unknown5, item.BuyPrice, item.SellPrice);
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
