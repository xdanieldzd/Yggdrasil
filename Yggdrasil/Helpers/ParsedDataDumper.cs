using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.ComponentModel;

using WebUI = System.Web.UI;

using Yggdrasil.TableParsing;

namespace Yggdrasil.Helpers
{
    public static class ParsedDataDumper
    {
        static string jsToggleFunction =
            "function toggle(showHide) {\n" +
            "   var table = document.getElementById(showHide);\n" +
            "   table.style.display = (table.style.display != \"table\" ? \"table\" : \"none\");\n" +
            "}\n";

        static string css =
            "body { font-family:sans-serif; }\n" +
            "a { text-decoration:none; }" +
            ".container { width:50em; }\n" +
            ".header { background-color:rgb(196,196,196); }\n" +
            ".header-text { text-align:left; float:left; }\n" +
            ".header-toggle { text-align:right; display:block; }\n" +
            "table { width:100%; border:1px solid black; display:none; }\n" +
            ".desc-column { width:20em; }\n" +
            "span.tooltip { border-bottom: 2px black dotted; cursor:help; }\n" +
            "span.tooltip span { display:none; padding:2px 3px; margin-left:8px; max-width:30em; }" +
            "span.tooltip:hover span { display:inline; position:absolute; background-color:white; border:1px solid #cccccc; color:#6c6c6c; }";

        public static void Dump(GameDataManager gameDataManager, Type parserType, string outputFilename)
        {
            List<BaseParser> parsedToDump = gameDataManager.ParsedData.Where(x => x.GetType() == parserType).ToList();
            PropertyInfo[] properties = parserType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(x => !x.GetGetMethod().IsVirtual && (x.GetAttribute<BrowsableAttribute>() == null || x.GetAttribute<BrowsableAttribute>().Browsable == true) && x.DeclaringType != typeof(BaseParser))
                .OrderBy(x => x.GetAttribute<Yggdrasil.Attributes.PrioritizedCategory>().Category)
                .ToArray();

            TextWriter tw = File.CreateText(outputFilename);
            using (WebUI.HtmlTextWriter h = new WebUI.HtmlTextWriter(tw))
            {
                h.RenderBeginTag(WebUI.HtmlTextWriterTag.Html);
                {
                    h.RenderBeginTag(WebUI.HtmlTextWriterTag.Head);
                    {
                        h.RenderBeginTag(WebUI.HtmlTextWriterTag.Title);
                        {
                            h.Write("{0} Data Dump for {1}", System.Windows.Forms.Application.ProductName, parserType.GetAttribute<Yggdrasil.Attributes.ParserDescriptor>().Description);
                        }
                        h.RenderEndTag();

                        h.AddAttribute(WebUI.HtmlTextWriterAttribute.Type, "text/javascript");
                        h.RenderBeginTag(WebUI.HtmlTextWriterTag.Script);
                        {
                            h.Write(jsToggleFunction);
                        }
                        h.RenderEndTag();

                        h.AddAttribute(WebUI.HtmlTextWriterAttribute.Type, "text/css");
                        h.RenderBeginTag(WebUI.HtmlTextWriterTag.Style);
                        {
                            h.Write(css);
                        }
                        h.RenderEndTag();
                    }
                    h.RenderEndTag();

                    h.RenderBeginTag(WebUI.HtmlTextWriterTag.Body);
                    {
                        h.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "container");
                        h.RenderBeginTag(WebUI.HtmlTextWriterTag.Div);
                        {
                            h.WriteEncodedText(string.Format("Data dump created by {0} {1}; dumping {2} entries of type '{3}'...",
                                System.Windows.Forms.Application.ProductName,
                                VersionManagement.CreateVersionString(System.Windows.Forms.Application.ProductVersion),
                                parsedToDump.Count,
                                parserType.GetAttribute<Yggdrasil.Attributes.ParserDescriptor>().Description));
                            h.WriteBreak();
                            h.WriteBreak();
                        }
                        h.RenderEndTag();

                        foreach (BaseParser parser in parsedToDump)
                        {
                            string parserId = string.Format("table-{0:D4}", parser.EntryNumber);

                            h.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "container");
                            h.RenderBeginTag(WebUI.HtmlTextWriterTag.Div);
                            {
                                h.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header");
                                h.RenderBeginTag(WebUI.HtmlTextWriterTag.Div);
                                {
                                    h.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header-text");
                                    h.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
                                    {
                                        h.WriteEncodedText(string.Format("Entry {0:D4}: {1}", parser.EntryNumber, parser.EntryDescription));
                                    }
                                    h.RenderEndTag();

                                    h.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header-toggle");
                                    h.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
                                    {
                                        h.AddAttribute(WebUI.HtmlTextWriterAttribute.Href, string.Format("javascript:toggle('{0}');", parserId), false);
                                        h.RenderBeginTag(WebUI.HtmlTextWriterTag.A);
                                        {
                                            h.Write("+/-");
                                        }
                                        h.RenderEndTag();
                                    }
                                    h.RenderEndTag();
                                }
                                h.RenderEndTag();

                                h.AddAttribute(WebUI.HtmlTextWriterAttribute.Id, parserId);
                                h.RenderBeginTag(WebUI.HtmlTextWriterTag.Table);
                                {
                                    string lastCategory = string.Empty;
                                    foreach (PropertyInfo property in properties)
                                    {
                                        string propCategory = ((string)property.GetAttribute<Yggdrasil.Attributes.PrioritizedCategory>().Category).Replace("\t", "");
                                        if (propCategory != lastCategory)
                                        {
                                            lastCategory = propCategory;
                                            h.RenderBeginTag(WebUI.HtmlTextWriterTag.Tr);
                                            {
                                                h.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header");
                                                h.AddAttribute(WebUI.HtmlTextWriterAttribute.Colspan, "2");
                                                h.RenderBeginTag(WebUI.HtmlTextWriterTag.Td);
                                                {
                                                    h.Write(propCategory);
                                                }
                                                h.RenderEndTag();
                                            }
                                            h.RenderEndTag();
                                        }
                                        h.RenderBeginTag(WebUI.HtmlTextWriterTag.Tr);
                                        {
                                            h.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "desc-column");
                                            h.RenderBeginTag(WebUI.HtmlTextWriterTag.Td);
                                            {
                                                DisplayNameAttribute displayName = property.GetAttribute<DisplayNameAttribute>();
                                                DescriptionAttribute description = property.GetAttribute<DescriptionAttribute>();

                                                h.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "tooltip");
                                                h.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
                                                {
                                                    h.WriteEncodedText(displayName.DisplayName);
                                                    if (description != null)
                                                    {
                                                        h.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
                                                        {
                                                            h.WriteEncodedText(description.Description);
                                                        }
                                                        h.RenderEndTag();
                                                    }
                                                }
                                                h.RenderEndTag();
                                            }
                                            h.RenderEndTag();

                                            h.RenderBeginTag(WebUI.HtmlTextWriterTag.Td);
                                            {
                                                object v = property.GetValue(parser, null);
                                                TypeConverterAttribute ca = (TypeConverterAttribute)property.GetCustomAttributes(typeof(TypeConverterAttribute), false).FirstOrDefault();
                                                TypeConverter c = new TypeConverter();
                                                if (ca != null)
                                                {
                                                    Type ct = Type.GetType(ca.ConverterTypeName);
                                                    if (ct == typeof(EnumConverter))
                                                        c = (TypeConverter)Activator.CreateInstance(ct, new object[] { property.PropertyType });
                                                    else
                                                        c = (TypeConverter)Activator.CreateInstance(ct);
                                                }
                                                var tmp = c.ConvertTo(v, typeof(string));
                                                h.WriteEncodedText(tmp as string);
                                            }
                                            h.RenderEndTag();
                                        }
                                        h.RenderEndTag();
                                    }
                                }
                                h.RenderEndTag();
                            }
                            h.RenderEndTag();
                        }
                    }
                    h.RenderEndTag();
                }
                h.RenderEndTag();
            }
            tw.Close();
        }
    }
}
