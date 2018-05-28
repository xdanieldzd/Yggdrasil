using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.ComponentModel;

using WebUI = System.Web.UI;

using Yggdrasil.FileHandling;
using Yggdrasil.FileHandling.TableHandling;
using Yggdrasil.TableParsing;
using Yggdrasil.TextHandling;

namespace Yggdrasil.Helpers
{
	public static class DataDumpers
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
			"table { width:100%; border:1px solid black; display:none; border-collapse:collapse; }\n" +
			"tr { border-bottom:1px solid rgb(191,191,191); vertical-align:top; }\n" +
			".desc-column { width:20em; border-bottom:1px solid rgb(191,191,191); }\n" +
			".desc-column-mesg { border-right:1px solid rgb(191,191,191); }\n" +
			"span.tooltip { border-bottom:2px black dotted; cursor:help; }\n" +
			"span.tooltip span { display:none; padding:2px 3px; margin-left:8px; max-width:30em; }" +
			"span.tooltip:hover span { display:inline; position:absolute; background-color:white; border:1px solid #cccccc; color:#6c6c6c; }";

		private static void WriteHeader(WebUI.HtmlTextWriter html, string title)
		{
			html.RenderBeginTag(WebUI.HtmlTextWriterTag.Head);
			{
				html.RenderBeginTag(WebUI.HtmlTextWriterTag.Title);
				{
					html.Write(title);
				}
				html.RenderEndTag();

				html.AddAttribute(WebUI.HtmlTextWriterAttribute.Type, "text/javascript");
				html.RenderBeginTag(WebUI.HtmlTextWriterTag.Script);
				{
					html.Write(jsToggleFunction);
				}
				html.RenderEndTag();

				html.AddAttribute(WebUI.HtmlTextWriterAttribute.Type, "text/css");
				html.RenderBeginTag(WebUI.HtmlTextWriterTag.Style);
				{
					html.Write(css);
				}
				html.RenderEndTag();
			}
			html.RenderEndTag();
		}

		public static void DumpMessages(GameDataManager gameDataManager, List<TableFile> tableFiles, string outputFilename)
		{
			string strippedName = Path.GetFileNameWithoutExtension(tableFiles.FirstOrDefault().Filename);
			if (gameDataManager.Version == GameDataManager.Versions.European)
			{
				foreach (KeyValuePair<GameDataManager.Languages, string> pair in gameDataManager.LanguageSuffixes) strippedName = strippedName.Replace(pair.Value, "");
			}

			int numTables = (int)tableFiles.FirstOrDefault().NumTables;
			if (!tableFiles.All(x => x.NumTables == numTables)) throw new Exception("Num tables mismatch!");

			TextWriter writer = File.CreateText(outputFilename);
			using (WebUI.HtmlTextWriter html = new WebUI.HtmlTextWriter(writer))
			{
				html.WriteLine("<!DOCTYPE html>");
				html.RenderBeginTag(WebUI.HtmlTextWriterTag.Html);
				{
					WriteHeader(html, string.Format("{0} Message Dump for {1}", System.Windows.Forms.Application.ProductName, strippedName));

					html.RenderBeginTag(WebUI.HtmlTextWriterTag.Body);
					{
						html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "container");
						html.RenderBeginTag(WebUI.HtmlTextWriterTag.Div);
						{
							html.WriteEncodedText(string.Format("Message dump created by {0} {1}; dumping {2}, {3} tables...",
								System.Windows.Forms.Application.ProductName,
								VersionManagement.CreateVersionString(System.Windows.Forms.Application.ProductVersion),
								(tableFiles.FirstOrDefault().FileNumber != -1 ? string.Format("{0}, file #{1}", strippedName, tableFiles.FirstOrDefault().FileNumber) : strippedName),
								numTables));
							html.WriteBreak();
							html.WriteBreak();
						}
						html.RenderEndTag();

						for (int i = 0; i < numTables; i++)
						{
							string tableId = string.Format("table-{0:D4}", i);

							html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header");
							html.RenderBeginTag(WebUI.HtmlTextWriterTag.Div);
							{
								html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header-text");
								html.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
								{
									html.Write("Table {0}", i + 1);
								}
								html.RenderEndTag();

								html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header-toggle");
								html.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
								{
									html.AddAttribute(WebUI.HtmlTextWriterAttribute.Href, string.Format("javascript:toggle('{0}');", tableId), false);
									html.RenderBeginTag(WebUI.HtmlTextWriterTag.A);
									{
										html.Write("+/-");
									}
									html.RenderEndTag();
								}
								html.RenderEndTag();
							}
							html.RenderEndTag();

							html.AddAttribute(WebUI.HtmlTextWriterAttribute.Id, tableId);
							html.AddStyleAttribute(WebUI.HtmlTextWriterStyle.Display, "table");
							html.RenderBeginTag(WebUI.HtmlTextWriterTag.Table);
							{
								html.RenderBeginTag(WebUI.HtmlTextWriterTag.Tr);
								{
									html.RenderBeginTag(WebUI.HtmlTextWriterTag.Th);
									{
										html.Write("ID");
									}
									html.RenderEndTag();

									foreach (TableFile file in tableFiles)
									{
										html.RenderBeginTag(WebUI.HtmlTextWriterTag.Th);
										{
											string language = Path.GetFileNameWithoutExtension(file.Filename);
											language = gameDataManager.LanguageSuffixes.FirstOrDefault(x => x.Value == language.Substring(language.LastIndexOf('_'), 3)).Key.ToString();
											html.Write(language);
										}
										html.RenderEndTag();
									}
								}
								html.RenderEndTag();

								int numMessages = (int)(tableFiles.FirstOrDefault().Tables[i] as MessageTable).NumMessages;
								for (int j = 0; j < numMessages; j++)
								{
									if ((tableFiles.FirstOrDefault().Tables[i] as MessageTable).Messages[j].RawData.Length == 0) continue;

									html.RenderBeginTag(WebUI.HtmlTextWriterTag.Tr);
									{
										html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "desc-column-mesg");
										html.RenderBeginTag(WebUI.HtmlTextWriterTag.Th);
										{
											html.Write("#{0}", j);
										}
										html.RenderEndTag();

										for (int k = 0; k < tableFiles.Count; k++)
										{
											html.RenderBeginTag(WebUI.HtmlTextWriterTag.Td);
											{
												string message = (tableFiles[k].Tables[i] as MessageTable).Messages[j].ConvertedString;
												message = message.Replace("<!pg>", "");
												message = message.Replace(" ", "&nbsp;");
												message = message.Replace("<", "&lt;");
												message = message.Replace(">", "&gt;");
												message = message.Replace(Environment.NewLine, "<br />");
												html.Write(message);
											}
											html.RenderEndTag();
										}
									}
									html.RenderEndTag();
								}
							}
							html.RenderEndTag();
							html.WriteBreak();
						}
					}
					html.RenderEndTag();
				}
				html.RenderEndTag();
			}
			writer.Close();
		}

		public static void DumpParsers(GameDataManager gameDataManager, Type parserType, string outputFilename)
		{
			List<BaseParser> parsedToDump = gameDataManager.ParsedData.Where(x => x.GetType() == parserType).ToList();
			PropertyInfo[] properties = parserType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
				.Where(x => !x.GetGetMethod().IsVirtual && (x.GetAttribute<BrowsableAttribute>() == null || x.GetAttribute<BrowsableAttribute>().Browsable == true) && x.DeclaringType != typeof(BaseParser))
				.OrderBy(x => x.GetAttribute<Yggdrasil.Attributes.PrioritizedCategory>().Category)
				.ToArray();

			TextWriter writer = File.CreateText(outputFilename);
			using (WebUI.HtmlTextWriter html = new WebUI.HtmlTextWriter(writer))
			{
				html.WriteLine("<!DOCTYPE html>");
				html.RenderBeginTag(WebUI.HtmlTextWriterTag.Html);
				{
					WriteHeader(html, string.Format("{0} Data Dump for {1}", System.Windows.Forms.Application.ProductName, parserType.GetAttribute<Yggdrasil.Attributes.ParserDescriptor>().Description));

					html.RenderBeginTag(WebUI.HtmlTextWriterTag.Body);
					{
						html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "container");
						html.RenderBeginTag(WebUI.HtmlTextWriterTag.Div);
						{
							html.WriteEncodedText(string.Format("Data dump created by {0} {1}; dumping {2} entries of type '{3}'...",
								System.Windows.Forms.Application.ProductName,
								VersionManagement.CreateVersionString(System.Windows.Forms.Application.ProductVersion),
								parsedToDump.Count,
								parserType.GetAttribute<Yggdrasil.Attributes.ParserDescriptor>().Description));
							html.WriteBreak();
							html.WriteBreak();
						}
						html.RenderEndTag();

						foreach (BaseParser parser in parsedToDump)
						{
							string parserId = string.Format("table-{0:D4}", parser.EntryNumber);

							html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "container");
							html.RenderBeginTag(WebUI.HtmlTextWriterTag.Div);
							{
								html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header");
								html.RenderBeginTag(WebUI.HtmlTextWriterTag.Div);
								{
									html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header-text");
									html.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
									{
										html.WriteEncodedText(string.Format("Entry {0:D4}: {1}", parser.EntryNumber, parser.EntryDescription));
									}
									html.RenderEndTag();

									html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header-toggle");
									html.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
									{
										html.AddAttribute(WebUI.HtmlTextWriterAttribute.Href, string.Format("javascript:toggle('{0}');", parserId), false);
										html.RenderBeginTag(WebUI.HtmlTextWriterTag.A);
										{
											html.Write("+/-");
										}
										html.RenderEndTag();
									}
									html.RenderEndTag();
								}
								html.RenderEndTag();

								html.AddAttribute(WebUI.HtmlTextWriterAttribute.Id, parserId);
								html.RenderBeginTag(WebUI.HtmlTextWriterTag.Table);
								{
									string lastCategory = string.Empty;
									foreach (PropertyInfo property in properties)
									{
										string propCategory = ((string)property.GetAttribute<Yggdrasil.Attributes.PrioritizedCategory>().Category).Replace("\t", "");
										if (propCategory != lastCategory)
										{
											lastCategory = propCategory;
											html.RenderBeginTag(WebUI.HtmlTextWriterTag.Tr);
											{
												html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "header");
												html.AddAttribute(WebUI.HtmlTextWriterAttribute.Colspan, "2");
												html.RenderBeginTag(WebUI.HtmlTextWriterTag.Td);
												{
													html.Write(propCategory);
												}
												html.RenderEndTag();
											}
											html.RenderEndTag();
										}
										html.RenderBeginTag(WebUI.HtmlTextWriterTag.Tr);
										{
											html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "desc-column");
											html.RenderBeginTag(WebUI.HtmlTextWriterTag.Td);
											{
												DisplayNameAttribute displayName = property.GetAttribute<DisplayNameAttribute>();
												DescriptionAttribute description = property.GetAttribute<DescriptionAttribute>();

												html.AddAttribute(WebUI.HtmlTextWriterAttribute.Class, "tooltip");
												html.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
												{
													html.WriteEncodedText(displayName.DisplayName);
													if (description != null)
													{
														html.RenderBeginTag(WebUI.HtmlTextWriterTag.Span);
														{
															html.WriteEncodedText(description.Description);
														}
														html.RenderEndTag();
													}
												}
												html.RenderEndTag();
											}
											html.RenderEndTag();

											html.RenderBeginTag(WebUI.HtmlTextWriterTag.Td);
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
												html.WriteEncodedText(tmp as string);
											}
											html.RenderEndTag();
										}
										html.RenderEndTag();
									}
								}
								html.RenderEndTag();
							}
							html.RenderEndTag();
						}
					}
					html.RenderEndTag();
				}
				html.RenderEndTag();
			}
			writer.Close();
		}
	}
}
