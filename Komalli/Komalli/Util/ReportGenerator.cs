using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Documents;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Word = DocumentFormat.OpenXml.Wordprocessing;
using Komalli.DataBaseManagement.DataModel;


namespace Komalli.Util
{
    public class ReportGenerator
    {
        internal static void GenerateDailySalesReport(string filePath, List<SalePOCO> sales)
        {
            try
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Word.Document(new Word.Body());
                    Word.Body body = mainPart.Document.Body;
                    AddHeaderSales(body);

                    AddSalesTable(body, sales);
                    AddFooter(body);
                    mainPart.Document.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el reporte de ventas diarias.", ex);
            }
        }

        internal static void GenerateDailyInventoryReport(string filePath, List<ProductPOCO> products)
        {
            try
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Word.Document(new Word.Body());
                    Word.Body body = mainPart.Document.Body;
                    AddHeaderProducts(body);

                    AddProductsTable(body, products);
                    AddFooter(body);
                    mainPart.Document.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el reporte de ventas diarias.", ex);
            }
        }

        private static void AddHeaderSales(Word.Body body)
        {
            body.AppendChild(new Word.Paragraph(
                new Word.Run(
                    new Word.Text($"Fecha de creación: {DateTime.Now:dd/MM/yyyy}"))));

            Word.Paragraph title = new Word.Paragraph(
                new Word.Run(
                    new Word.Text("Reporte diario de ventas"))
                {
                    RunProperties = new Word.RunProperties(
                        new Word.Bold())
                });

            title.ParagraphProperties = new Word.ParagraphProperties
            {
                Justification = new Word.Justification { Val = Word.JustificationValues.Center }
            };

            body.AppendChild(title);
        }

        private static void AddHeaderProducts(Word.Body body)
        {
            body.AppendChild(new Word.Paragraph(
                new Word.Run(
                    new Word.Text($"Fecha de creación: {DateTime.Now:dd/MM/yyyy}"))));

            Word.Paragraph title = new Word.Paragraph(
                new Word.Run(
                    new Word.Text("Reporte diario de inventario"))
                {
                    RunProperties = new Word.RunProperties(
                        new Word.Bold())
                });

            title.ParagraphProperties = new Word.ParagraphProperties
            {
                Justification = new Word.Justification { Val = Word.JustificationValues.Center }
            };

            body.AppendChild(title);
        }

        private static void AddSalesTable(Word.Body body, List<SalePOCO> sales)
        {

            Word.Table table = new Word.Table();
            Word.TableProperties tableProperties = new Word.TableProperties(
                new Word.TableBorders(
                    new Word.TopBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.BottomBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.LeftBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.RightBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.InsideHorizontalBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.InsideVerticalBorder { Val = Word.BorderValues.Single, Size = 12 }));
            table.AppendChild(tableProperties);

            Word.TableRow headerRow = new Word.TableRow();
            string[] headers = { "#", "Fecha de venta", "Total de ventas", "ID Empleado", "Estado" };
            foreach (string header in headers)
            {
                Word.TableCell headerCell = new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(header)))
                    {
                        ParagraphProperties = new Word.ParagraphProperties
                        {
                            Justification = new Word.Justification { Val = Word.JustificationValues.Center }
                        }
                    });
                headerRow.AppendChild(headerCell);
            }
            table.AppendChild(headerRow);

            int rowIndex = 1;
            foreach (var sale in sales)
            {
                Word.TableRow row = new Word.TableRow();

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(rowIndex.ToString())))));

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(sale.SaleDate.ToShortDateString())))));

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(sale.TotalSale.ToString("C"))))));

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(sale.StaffID.ToString())))));

                string statusName;
                switch (sale.SaleStatus)
                {
                    case 1:
                        statusName = "En preparación";
                        break;
                    case 2:
                        statusName = "Preparado";
                        break;
                    case 3:
                        statusName = "Entregado";
                        break;
                    case 4:
                        statusName = "Cancelado";
                        break;
                    default:
                        statusName = "Desconocido";
                        break;
                }

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(statusName)))));

                table.AppendChild(row);
                rowIndex++;
            }

            Word.TableRow totalRow = new Word.TableRow();
            totalRow.AppendChild(new Word.TableCell(
                new Word.Paragraph(
                    new Word.Run(
                        new Word.Text("Total de ventas:")))
                {
                    ParagraphProperties = new Word.ParagraphProperties
                    {
                        Justification = new Word.Justification { Val = Word.JustificationValues.Right }
                    }
                })
            {
                TableCellProperties = new Word.TableCellProperties
                {
                    GridSpan = new Word.GridSpan { Val = 4 }
                }
            });
            totalRow.AppendChild(new Word.TableCell(
                new Word.Paragraph(
                    new Word.Run(
                        new Word.Text(sales.Sum(s => s.TotalSale).ToString("C"))))));

            table.AppendChild(totalRow);
            body.AppendChild(table);
        }

        private static void AddProductsTable(Word.Body body, List<ProductPOCO> products)
        {
            Word.Table table = new Word.Table();

            Word.TableProperties tableProperties = new Word.TableProperties(
                new Word.TableBorders(
                    new Word.TopBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.BottomBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.LeftBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.RightBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.InsideHorizontalBorder { Val = Word.BorderValues.Single, Size = 12 },
                    new Word.InsideVerticalBorder { Val = Word.BorderValues.Single, Size = 12 }));
            table.AppendChild(tableProperties);

            Word.TableRow headerRow = new Word.TableRow();
            string[] headers = { "#", "Precio", "Cantidad disponible", "Nombre", "Tipo" };
            foreach (string header in headers)
            {
                Word.TableCell headerCell = new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(header)))
                    {
                        ParagraphProperties = new Word.ParagraphProperties
                        {
                            Justification = new Word.Justification { Val = Word.JustificationValues.Center }
                        }
                    });
                headerRow.AppendChild(headerCell);
            }
            table.AppendChild(headerRow);

            int rowIndex = 1;
            foreach (var product in products)
            {
                Word.TableRow row = new Word.TableRow();

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                new Word.Run(
                            new Word.Text(product.ProductId.ToString())))));

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(product.ProductPrice.ToString("C"))))));

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(product.ProductAvailableQuantity.ToString())))));

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(product.ProductName)))));

                string typeName;
                switch (product.ProductTypeId)
                {
                    case 1:
                        typeName = "Desayuno del día";
                        break;
                    case 2:
                        typeName = "Comida del día";
                        break;
                    case 3:
                        typeName = "De la carta";
                        break;
                    case 4:
                        typeName = "Bebidas";
                        break;
                    case 5:
                        typeName = "Postres";
                        break;
                    default:
                        typeName = "Desconocido";
                        break;
                }

                row.AppendChild(new Word.TableCell(
                    new Word.Paragraph(
                        new Word.Run(
                            new Word.Text(typeName)))));

                table.AppendChild(row);
                rowIndex++;
            }

            body.AppendChild(table);
        }

        private static void AddFooter(Word.Body body)
        {

            Word.Paragraph footer = new Word.Paragraph(
                new Word.Run(
                    new Word.Text("Comedor Universitario \"Komalli\" © Derechos reservados")));

            footer.ParagraphProperties = new Word.ParagraphProperties
            {
                Justification = new Word.Justification { Val = Word.JustificationValues.Center }
            };

            body.AppendChild(footer);
        }
    }
}
