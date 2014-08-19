using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Yggdrasil.FileTypes;

namespace Yggdrasil.Dumpers
{
    public static class MessageDumper
    {
        public static void DumpToDirectory(GameDataManager gameDataManager, string path, bool skipNulls)
        {
            path = Path.Combine(path, gameDataManager.Header.GameCode);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            foreach (TBB tbb in gameDataManager.MessageFiles)
            {
                string filePath = Path.Combine(path, Path.GetFileNameWithoutExtension(tbb.Filename) + ".htm");

                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">");
                string style = "<style type=\"text/css\">table, th, td { border: 1px solid black; }</style>";
                builder.AppendFormat("<html>\n<head><title>{0}</title><meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">{1}</head>\n<body>\n", Path.GetFileName(tbb.Filename), style);

                builder.AppendFormat("File: {0}; skip null entries? {1}<br><br>", Path.GetFileName(tbb.Filename), skipNulls);

                for (int i = 0; i < tbb.NumTables; i++)
                {
                    if (!(tbb.Tables[i] is TBB.MTBL)) continue;

                    TBB.MTBL messageTable = (tbb.Tables[i] as TBB.MTBL);
                    builder.AppendFormat("Message table #{0}, offset 0x{1:X8}, {2} message(s)<br><br>\n", i, messageTable.Offset, messageTable.NumMessages);
                    builder.AppendLine("<table><tr><th>Number</th><th>Offset</th><th>Text</th></tr>\n");

                    for (int j = 0; j < messageTable.NumMessages; j++)
                    {
                        string message = messageTable.Messages[j].ToString().Replace("\n", "<br>").Replace(" ", "&nbsp;");

                        if (messageTable.MessageOffsets[j] != 0)
                            builder.AppendFormat("<tr><td>{0}</td><td>0x{1:X8}</td><td>{2}</td></tr>\n", j, messageTable.Offset + 0x10 + messageTable.MessageOffsets[j], message);
                        else if (!skipNulls)
                            builder.AppendFormat("<tr><td>{0}</td><td>null</td><td> </td></tr>\n", j);
                    }

                    builder.AppendLine("</table><br>");
                }

                builder.AppendLine("</body></html>");

                StreamWriter writer = File.CreateText(filePath);
                writer.Write(builder.ToString());
                writer.Close();
            }
        }
    }
}
