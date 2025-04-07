using System;
using System.Data;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Tables;
using System.Drawing;


namespace NoktaBilgiNotificationService.Classes
{
    internal class PDFCreat
    {
        internal static string PdfCreater(string id)
        {
            try
            {
                DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode, LogoPeriod, LogoCompanyName, CompanyPicture,IISPath FROM CompanySettings LIMIT 1");
                if (dt is null)
                    return null;
                if (dt.Rows.Count <= 0)
                    return null;
                try
                {
                    if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        return null;
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    return null;
                }
                DataTable connectionSQLITE = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
                if (connectionSQLITE is null)
                    return null;
                if (connectionSQLITE.Rows.Count <= 0)
                    return null;
                try
                {
                    if (string.IsNullOrEmpty(connectionSQLITE.Rows[0][0].ToString()))
                        return null;
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    return null;
                }
                DataTable NKT_TABLE = SQLCrud.LoadDataIntoGridView($"SELECT TOP 1 * FROM NKT_ORFFICHELOGICALREFWP WITH (NOLOCK) WHERE ID={id}", connectionSQLITE.Rows[0][0].ToString());
                if (NKT_TABLE is null)
                    return null;
                if (NKT_TABLE.Rows.Count <= 0)
                    return null;
                try
                {
                    if (string.IsNullOrEmpty(NKT_TABLE.Rows[0][0].ToString()))
                        return null;
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    return null;
                }
                string queryFiche = $@"
                SELECT 
                    IIF(MAN.CODE='',MAN.DEFINITION_,MAN.CODE) 'Satış Elemanı',
                    FICHE.FICHENO AS 'Sipariş No',
                    CONCAT(FICHE.TRRATE,' ',ISNULL(CUR.CURSYMBOL,'TL')) AS 'Kur',
                    CONVERT(DATE,FICHE.DATE_,104) AS 'Sipariş Tarihi',
                    CL.DEFINITION_ AS 'Cari Açıklama',
                    CL.TELNRS1 AS 'Cari Telefon',
                    FORMAT(FICHE.GROSSTOTAL,'C','tr-TR') AS 'Sipariş Brüt',
                    FORMAT(FICHE.TOTALVAT,'C','tr-TR') AS 'Sipariş KDV Tutar',
                    FORMAT(FICHE.TOTALDISCOUNTS,'C','tr-TR') AS 'Sipariş İndirim Tutari',
                    FORMAT(FICHE.NETTOTAL,'C','tr-TR') AS 'Sipariş Net Toplam'
                FROM LG_{dt.Rows[0][0].ToString()}_{dt.Rows[0][1].ToString()}_ORFICHE FICHE WITH (NOLOCK)
                JOIN LG_{dt.Rows[0][0].ToString()}_CLCARD CL WITH (NOLOCK) ON CL.LOGICALREF=FICHE.CLIENTREF
                LEFT JOIN L_CURRENCYLIST CUR WITH (NOLOCK) ON CUR.LOGICALREF=FICHE.TRCURR AND FIRMNR=1
                LEFT JOIN LG_SLSMAN MAN WITH (NOLOCK) ON MAN.LOGICALREF=FICHE.SALESMANREF
                WHERE FICHE.LOGICALREF={NKT_TABLE.Rows[0][1]}";
                DataTable FicheOrder = SQLCrud.LoadDataIntoGridView(queryFiche, connectionSQLITE.Rows[0][0].ToString());
                if (FicheOrder is null)
                    return null;
                if (FicheOrder.Rows.Count <= 0)
                    return null;
                try
                {
                    if (string.IsNullOrEmpty(FicheOrder.Rows[0][0].ToString()))
                        return null;
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    return null;
                }
                string queryLine = $@"SELECT 
ITM.NAME 'Ürün Açiklama',
CONCAT(LINE.AMOUNT, ' ', ISNULL(UNIT.CODE, '')) 'Miktar',
FORMAT(LINE.PRICE, 'C', 'tr-TR') 'Birim Fiyat',
LINE.VAT 'KDV',
FORMAT(LINE.VATAMNT, 'C', 'tr-TR') 'KDV Tutari',
FORMAT(LINE.DISTDISC, 'C', 'tr-TR') 'İndirim_Tutari',
FORMAT(LINE.TOTAL, 'C', 'tr-TR') 'Tutar',
FORMAT(LINE.LINENET+LINE.VATAMNT, 'C', 'tr-TR') 'Net Tutar',
CONVERT(DATE,LINE.DUEDATE, 104) 'Teslim Tarihi'
FROM LG_{dt.Rows[0][0].ToString()}_{dt.Rows[0][1].ToString()}_ORFLINE LINE WITH (NOLOCK)
JOIN LG_{dt.Rows[0][0].ToString()}_ITEMS ITM WITH(NOLOCK) ON LINE.STOCKREF = ITM.LOGICALREF
LEFT JOIN LG_{dt.Rows[0][0].ToString()}_UNITSETL UNIT WITH(NOLOCK) ON UNIT.LOGICALREF = LINE.UOMREF
WHERE LINE.ORDFICHEREF = {NKT_TABLE.Rows[0][1]}
ORDER BY LINE.LOGICALREF";
                DataTable OrderDetails = SQLCrud.LoadDataIntoGridView(queryLine, connectionSQLITE.Rows[0][0].ToString());
                if (OrderDetails is null)
                    return null;
                if (OrderDetails.Rows.Count <= 0)
                    return null;
                try
                {
                    if (string.IsNullOrEmpty(OrderDetails.Rows[0][0].ToString()))
                        return null;
                }
                catch (Exception ex)
                {
                    TextLog.TextLogging(ex.Message + " -- " + ex.ToString());
                    return null;
                }
                string filePath = dt.Rows[0]["IISPath"] + "\\PDF\\order_" + id + "_" + FicheOrder.Rows[0]["Sipariş No"] + ".pdf";
                Document doc = new Document();
                Section section = doc.AddSection();
                section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
                section.PageSetup.RightMargin = Unit.FromCentimeter(1);
                section.PageSetup.TopMargin = Unit.FromCentimeter(2);
                section.PageSetup.BottomMargin = Unit.FromCentimeter(2);
                byte[] imageBytes = Convert.FromBase64String(dt.Rows[0]["CompanyPicture"].ToString());
                string tempImagePath = Path.Combine(Path.GetTempPath(), $"tempLogo_{Guid.NewGuid()}.png");
                using (MemoryStream ms = new MemoryStream(imageBytes))
                using (Image image = Image.FromStream(ms))
                {
                    image.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);
                }
                var logo = section.AddImage(tempImagePath);
                logo.Width = Unit.FromCentimeter(4);
                logo.LockAspectRatio = true;
                Paragraph title = section.AddParagraph("SATIŞ SİPARİŞİ\n___________________________");
                title.Format.Font.Size = 16;
                title.Format.Font.Bold = true;
                title.Format.Alignment = ParagraphAlignment.Center;
                section.AddParagraph().Format.SpaceAfter = Unit.FromCentimeter(1);
                Table headerTable = section.AddTable();
                headerTable.Borders.Width = 0;
                Column colLeft = headerTable.AddColumn(Unit.FromCentimeter(10));
                Column colRight = headerTable.AddColumn(Unit.FromCentimeter(7));
                Row headerRow = headerTable.AddRow();
                Paragraph leftText = headerRow.Cells[0].AddParagraph($"Cari Ünvanı: {FicheOrder.Rows[0]["Cari Açıklama"]}");
                leftText.Format.Font.Bold = true;
                leftText.Format.Font.Size = 12;
                Paragraph rightText = headerRow.Cells[1].AddParagraph();
                headerRow.Cells[1].Format.LeftIndent = Unit.FromCentimeter(1);
                DateTime teslimTarihi = Convert.ToDateTime(OrderDetails.Rows[0]["Teslim Tarihi"]);
                string formattedDateTeslim = teslimTarihi.ToString("dd.MM.yyyy");
                rightText.AddFormattedText("Teslim Tarihi: ", TextFormat.Bold);
                rightText.AddText($"{formattedDateTeslim}\n");
                rightText.Format.Alignment = ParagraphAlignment.Left;
                rightText.AddFormattedText("Satış Elemanı : ", TextFormat.Bold);
                rightText.AddText($"{FicheOrder.Rows[0]["Satış Elemanı"]}\n");
                rightText.Format.Alignment = ParagraphAlignment.Center;
                DateTime siparisTarihi = Convert.ToDateTime(FicheOrder.Rows[0]["Sipariş Tarihi"]);
                string formattedDate = siparisTarihi.ToString("dd.MM.yyyy");
                rightText.AddFormattedText("Fiş No: ", TextFormat.Bold);
                rightText.AddText($"{FicheOrder.Rows[0]["Sipariş No"]}\n");
                rightText.Format.Alignment = ParagraphAlignment.Center;
                rightText.AddFormattedText("Fiş Tarihi: ", TextFormat.Bold);
                rightText.AddText($"{formattedDate}\n");
                rightText.Format.Alignment = ParagraphAlignment.Left;
                rightText.AddFormattedText("Döviz Kuru: ", TextFormat.Bold);
                rightText.AddText($"{FicheOrder.Rows[0]["Kur"]}");
                rightText.Format.Alignment = ParagraphAlignment.Left;
                rightText.Format.SpaceAfter = Unit.FromCentimeter(1);
                Table table = section.AddTable();
                table.Borders.Width = 0.75;
                table.AddColumn(Unit.FromCentimeter(4.5)); // Ürün Açıklama
                table.AddColumn(Unit.FromCentimeter(1.5));  // Miktar
                table.AddColumn(Unit.FromCentimeter(2)); // Birim Fiyat
                table.AddColumn(Unit.FromCentimeter(1)); // KDV
                table.AddColumn(Unit.FromCentimeter(3)); // KDV Tutarı
                table.AddColumn(Unit.FromCentimeter(2)); // İndirim Tutarı
                table.AddColumn(Unit.FromCentimeter(3)); // Tutar
                table.AddColumn(Unit.FromCentimeter(3)); // Net Tutar (Artık sığıyor)
                Row header = table.AddRow();
                header.Shading.Color = Colors.SkyBlue;
                header.Format.Font.Color = Colors.Black;
                header.Format.Font.Bold = true;
                header.Format.Font.Size = 9;
                header.Format.Alignment = ParagraphAlignment.Center;
                string[] headers = { "Ürün Açıklama", "Miktar", "Birim Fiyat", "KDV", "KDV Tutarı", "İndirim Tutarı", "Tutar", "Net Tutar" };
                for (int i = 0; i < headers.Length; i++)
                {
                    Paragraph headerParagraph = header.Cells[i].AddParagraph(headers[i]);
                    headerParagraph.Format.Font.Bold = true;
                    headerParagraph.Format.Alignment = ParagraphAlignment.Center;
                }
                foreach (DataRow row in OrderDetails.Rows)
                {
                    Row newRow = table.AddRow();
                    newRow.Format.Font.Size = 9;
                    newRow.Borders.Color = Colors.Gray;
                    newRow.Borders.Width = 0.5;
                    for (int i = 0; i < OrderDetails.Columns.Count - 1; i++)
                    {
                        Paragraph p = newRow.Cells[i].AddParagraph(row[i].ToString());
                        p.Format.Alignment = ParagraphAlignment.Center;
                    }
                }
                section.AddParagraph().Format.SpaceBefore = Unit.FromCentimeter(1);
                Table totalTable = section.AddTable();
                totalTable.Borders.Width = 0.75;
                totalTable.Format.Alignment = ParagraphAlignment.Right;
                totalTable.AddColumn(Unit.FromCentimeter(3.5));
                totalTable.AddColumn(Unit.FromCentimeter(4));
                void AddTotalRow(string label, string value, bool isLast = false)
                {
                    Row row = totalTable.AddRow();
                    row.Cells[0].AddParagraph(label).Format.Font.Bold = true;
                    row.Cells[1].AddParagraph(value).Format.Alignment = ParagraphAlignment.Right;
                    row.Format.Font.Size = 9;
                    row.Borders.Color = Colors.LightGray;
                    if (isLast)
                    {
                        row.Shading.Color = Colors.LightGray;
                        row.Format.Font.Bold = true;
                        row.Format.Font.Size = 10;
                    }
                    row.TopPadding = Unit.FromCentimeter(0.1);
                    row.BottomPadding = Unit.FromCentimeter(0.1);
                }
                AddTotalRow("Toplam", $"{FicheOrder.Rows[0]["Sipariş Brüt"]}");
                AddTotalRow("KDV", $"{FicheOrder.Rows[0]["Sipariş KDV Tutar"]}");
                AddTotalRow("İndirim", $"{FicheOrder.Rows[0]["Sipariş İndirim Tutari"]}");
                AddTotalRow("Genel Toplam", $"{FicheOrder.Rows[0]["Sipariş Net Toplam"]}", isLast: true);
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                pdfRenderer.Document = doc;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(filePath);
                return filePath;
            }
            catch (Exception e)
            {
                TextLog.TextLogging(e.Message + " --- " + e.ToString());
                return null;
            }
        }
    }
}