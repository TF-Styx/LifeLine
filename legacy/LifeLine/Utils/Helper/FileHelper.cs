using LifeLine.MVVM.Models.MSSQL_DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace LifeLine.Utils.Helper
{
    public static class FileHelper
    {
        public static byte[] ImageToBytes(Image image)
        {
            using (MemoryStream memoryStream = new())
            {
                image.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }

        public static async Task CreateJSON<T>(List<T> data, string pathFile)
        {
            await Task.Run(() =>
            {
                if (!string.IsNullOrWhiteSpace(pathFile))
                {
                    File.WriteAllText(pathFile, JsonSerializer.Serialize(data));
                }
                else
                {
                    throw new Exception("Не указан путь!!!");
                }
            });
        }

        public static async Task CreatXml<T>(List<T> data, string pathFile)
        {
            await Task.Run(() =>
            {
                using (XmlWriter writer = XmlWriter.Create(pathFile, new XmlWriterSettings { Indent = true }))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Items");

                    foreach (var item in data)
                    {
                        writer.WriteStartElement($"{typeof(T).Name}");

                        var properties = typeof(T).GetProperties();

                        foreach (var property in properties)
                        {
                            var value = property.GetValue(item);

                            writer.WriteStartElement(property.Name);

                            if (value is IEnumerable<object> collection)
                            {
                                foreach (var element in collection)
                                {
                                    writer.WriteStartElement("Item");
                                    writer.WriteString(element != null ? element.ToString() : string.Empty);
                                    writer.WriteEndElement();
                                }
                            }
                            else if (value is byte[] byteArray)
                            {
                                writer.WriteString(Convert.ToBase64String(byteArray));
                            }
                            else
                            {
                                writer.WriteString(value != null ? value.ToString() : string.Empty);
                            }

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            });

            //if (data == null || data.Count == 0)
            //{
            //    Console.WriteLine("Список данных пуст. Создание файла XML не требуется.");
            //    return;
            //}

            //await Task.Run(() =>
            //{
            //    if (!string.IsNullOrWhiteSpace(pathFile))
            //    {
            //        using (XmlWriter writer = XmlWriter.Create(pathFile, new XmlWriterSettings { Indent = true }))
            //        {
            //            writer.WriteStartDocument();
            //            writer.WriteStartElement("Items");

            //            foreach (var item in data)
            //            {
            //                writer.WriteStartElement($"{typeof(T).Name}");
            //                var properties = typeof(T).GetProperties();

            //                foreach (var prop in properties)
            //                {
            //                    var value = prop.GetValue(prop);
            //                    writer.WriteStartElement(prop.Name);

            //                    if (value is byte[] image)
            //                    {
            //                        writer.WriteString(Convert.ToBase64String(image));
            //                    }
            //                    else
            //                    {
            //                        writer.WriteString(value != null ? value.ToString() : string.Empty);
            //                    }

            //                    if (value is IEnumerable<object> collection)
            //                    {
            //                        foreach (var element in collection)
            //                        {
            //                            writer.WriteStartElement("Item");
            //                            writer.WriteString(element != null ? element.ToString() : string.Empty);
            //                            writer.WriteEndElement();
            //                        }
            //                    }
            //                    else if (value is byte[] byteArray)
            //                    {
            //                        writer.WriteString(Convert.ToBase64String(byteArray));
            //                    }
            //                    else
            //                    {
            //                        writer.WriteString(value != null ? value.ToString() : string.Empty);
            //                    }

            //                    writer.WriteEndElement();
            //                }
            //                writer.WriteEndElement();
            //            }
            //            writer.WriteEndElement();
            //            writer.WriteEndDocument();
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception("Не указан путь!!!");
            //    }
            //});
        }
    }
}
