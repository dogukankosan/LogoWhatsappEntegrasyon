using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MigraColor = MigraDoc.DocumentObjectModel.Color;

namespace NoktaBilgiNotificationUI.Classes
{
    internal class PDFCreat
    {
        internal static byte[] PdfCreater(string id)
        {
            try
            {
                DataTable dt = SQLiteCrud.GetDataFromSQLite("SELECT LogoCompanyCode, LogoPeriod, LogoCompanyName, CompanyPicture,CompanyNo FROM CompanySettings LIMIT 1");
                if (dt is null || dt.Rows.Count <= 0 || string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    return null;
                DataTable connectionSQLITE = SQLiteCrud.GetDataFromSQLite("SELECT SQLConnectString FROM SqlConnectionString LIMIT 1");
                if (connectionSQLITE is null || connectionSQLITE.Rows.Count <= 0 || string.IsNullOrEmpty(connectionSQLITE.Rows[0][0].ToString()))
                    return null;
                DataTable NKT_TABLE = SQLCrud.LoadDataIntoGridView($"SELECT TOP 1 * FROM NKT_ORFFICHELOGICALREFWP WHERE ID={id}", connectionSQLITE.Rows[0][0].ToString());
                if (NKT_TABLE is null || NKT_TABLE.Rows.Count <= 0 || string.IsNullOrEmpty(NKT_TABLE.Rows[0][0].ToString()))
                    return null;
                string queryFiche = $@"
                SELECT 
                    MAN.DEFINITION_ 'Satış Elemanı',
                    FICHE.FICHENO AS 'Sipariş No',
                    CONCAT(FICHE.TRRATE,' ',ISNULL(CUR.CURSYMBOL,'TL')) AS 'Kur',
                    CONVERT(DATE,FICHE.DATE_,104) AS 'Sipariş Tarihi',
                    CL.DEFINITION_ AS 'Cari Açıklama',
                    CL.TELNRS1 AS 'Cari Telefon',
                    FORMAT(FICHE.GROSSTOTAL,'C','tr-TR') AS 'Sipariş Brüt',
                    FORMAT(FICHE.TOTALVAT,'C','tr-TR') AS 'Sipariş KDV Tutar',
                    FORMAT(FICHE.TOTALDISCOUNTS,'C','tr-TR') AS 'Sipariş İndirim Tutari',
                    FORMAT(FICHE.NETTOTAL,'C','tr-TR') AS 'Sipariş Net Toplam',
                    ISNULL(PAY.DEFINITION_,'') 'ODEME'
                FROM LG_{dt.Rows[0][0].ToString()}_{dt.Rows[0][1].ToString()}_ORFICHE FICHE WITH (NOLOCK)
                JOIN LG_{dt.Rows[0][0].ToString()}_CLCARD CL WITH (NOLOCK) ON CL.LOGICALREF=FICHE.CLIENTREF
                LEFT JOIN L_CURRENCYLIST CUR WITH (NOLOCK) ON CUR.CURTYPE=FICHE.TRCURR AND FIRMNR={dt.Rows[0]["CompanyNo"].ToString()}
                LEFT JOIN LG_{dt.Rows[0][0].ToString()}_PAYPLANS PAY WITH (NOLOCK) ON PAY.LOGICALREF=FICHE.PAYDEFREF
                LEFT JOIN LG_SLSMAN MAN WITH (NOLOCK) ON MAN.LOGICALREF=FICHE.SALESMANREF
                WHERE FICHE.LOGICALREF={NKT_TABLE.Rows[0][1].ToString()}";
                DataTable FicheOrder = SQLCrud.LoadDataIntoGridView(queryFiche, connectionSQLITE.Rows[0][0].ToString());
                if (FicheOrder is null || FicheOrder.Rows.Count <= 0 || string.IsNullOrEmpty(FicheOrder.Rows[0]["Sipariş No"].ToString()))
                    return null;
                string queryLine = $@"SELECT 
IIF(
    NULLIF(
        CASE 
            WHEN LINE.LINETYPE = 0 THEN ITM.NAME 
            ELSE SRV.DEFINITION_ 
        END, 
    '') IS NULL, 
    'İndirim Satırı', 
    CASE 
        WHEN LINE.LINETYPE = 0 THEN ITM.NAME 
        ELSE SRV.DEFINITION_ 
    END
) AS [Ürün Açıklama],
CONCAT(LINE.AMOUNT, ' ', ISNULL(UNIT.CODE, '')) 'Miktar',
FORMAT(LINE.PRICE, 'C', 'tr-TR') 'Birim Fiyat',
LINE.VAT 'KDV',
FORMAT(LINE.VATAMNT, 'C', 'tr-TR') 'KDV Tutari',
FORMAT(LINE.DISTDISC, 'C', 'tr-TR') 'İndirim_Tutari',
FORMAT(LINE.TOTAL, 'C', 'tr-TR') 'Tutar',
FORMAT(LINE.LINENET+LINE.VATAMNT, 'C', 'tr-TR') 'Net Tutar',
CONVERT(DATE,LINE.DUEDATE, 104) 'Teslim Tarihi'
FROM LG_{dt.Rows[0][0].ToString()}_{dt.Rows[0][1].ToString()}_ORFLINE LINE WITH (NOLOCK)
LEFT JOIN LG_{dt.Rows[0][0].ToString()}_ITEMS ITM WITH(NOLOCK) ON LINE.STOCKREF = ITM.LOGICALREF AND LINE.LINETYPE=0
LEFT JOIN LG_{dt.Rows[0][0].ToString()}_SRVCARD SRV WITH(NOLOCK) ON LINE.STOCKREF = SRV.LOGICALREF AND LINE.LINETYPE=4
LEFT JOIN LG_{dt.Rows[0][0].ToString()}_UNITSETL UNIT WITH(NOLOCK) ON UNIT.LOGICALREF = LINE.UOMREF
WHERE LINE.ORDFICHEREF = {NKT_TABLE.Rows[0][1].ToString()}
ORDER BY LINE.LOGICALREF";
                DataTable OrderDetails = SQLCrud.LoadDataIntoGridView(queryLine, connectionSQLITE.Rows[0][0].ToString());
                if (OrderDetails is null || OrderDetails.Rows.Count <= 0 || string.IsNullOrEmpty(OrderDetails.Rows[0]["Tutar"].ToString()))
                    return null;
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
                    image.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);
                var logo = section.AddImage(tempImagePath);
                logo.Width = Unit.FromCentimeter(4);
                logo.LockAspectRatio = true;
                Paragraph title = section.AddParagraph("SATIŞ SİPARİŞİ");
                title.Format.Font.Size = 16;
                title.Format.Font.Name = "Arial";
                title.Format.Font.Bold = true;
                title.Format.Alignment = ParagraphAlignment.Center;
                Paragraph line = section.AddParagraph("___________________");
                Paragraph cariUnvan = section.AddParagraph();
                FormattedText boldPart = cariUnvan.AddFormattedText("Cari Ünvan: ", TextFormat.Bold);
                cariUnvan.AddText(FicheOrder.Rows[0]["Cari Açıklama"].ToString());
                cariUnvan.Format.Font.Size = 12;
                cariUnvan.Format.Font.Name = "Arial";
                cariUnvan.Format.Alignment = ParagraphAlignment.Left;
                cariUnvan.Format.SpaceAfter = Unit.FromCentimeter(0.5);
                line.Format.Alignment = ParagraphAlignment.Center;
                line.Format.SpaceAfter = Unit.FromCentimeter(1.5);
                Table infoTable = section.AddTable();
                infoTable.Borders.Width = 0.5;
                infoTable.Borders.Color = Colors.Black;
                infoTable.Borders.Width = 0.5;
                infoTable.Borders.Color = Colors.Black;
                infoTable.Format.Alignment = ParagraphAlignment.Left;
                infoTable.Rows.LeftIndent = Unit.FromCentimeter(2);
                infoTable.AddColumn(Unit.FromCentimeter(3));
                infoTable.AddColumn(Unit.FromCentimeter(6));
                void AddInfoRow(string label, string value)
                {
                    Row row = infoTable.AddRow();
                    row.Borders.Visible = true;
                    row.Borders.Color = Colors.Black;
                    row.Borders.Width = 0.5;
                    row.TopPadding = Unit.FromMillimeter(1);
                    row.BottomPadding = Unit.FromMillimeter(1);
                    var labelPara = row.Cells[0].AddParagraph(label);
                    labelPara.Format.Font.Name = "Arial";
                    labelPara.Format.Font.Size = 9;
                    labelPara.Format.Font.Bold = true;
                    labelPara.Format.Alignment = ParagraphAlignment.Right;
                    infoTable.Rows.LeftIndent = 300;
                    var valuePara = row.Cells[1].AddParagraph(value);
                    valuePara.Format.Font.Name = "Arial";
                    valuePara.Format.Font.Size = 9;
                    valuePara.Format.Alignment = ParagraphAlignment.Left;
                }
                DateTime teslimTarihi = Convert.ToDateTime(OrderDetails.Rows[0]["Teslim Tarihi"]);
                string formattedTeslim = teslimTarihi.ToString("dd.MM.yyyy");
                DateTime siparisTarihi = Convert.ToDateTime(FicheOrder.Rows[0]["Sipariş Tarihi"]);
                string formattedSiparis = siparisTarihi.ToString("dd.MM.yyyy");
                AddInfoRow("Teslim Tarihi:", formattedTeslim);
                AddInfoRow("Satış Elemanı:", FicheOrder.Rows[0]["Satış Elemanı"].ToString());
                AddInfoRow("Fiş No:", FicheOrder.Rows[0]["Sipariş No"].ToString());
                AddInfoRow("Fiş Tarihi:", formattedSiparis);
                AddInfoRow("Ödeme Planı:", FicheOrder.Rows[0]["ODEME"].ToString());
                string kurbas = FicheOrder.Rows[0]["Kur"].ToString();
                AddInfoRow("Döviz Kuru:", $"{kurbas.Replace("$", " TL").Replace("€", " TL").Replace("£", " TL")}");
                section.AddParagraph().Format.SpaceAfter = Unit.FromCentimeter(0.5);
                Table table = section.AddTable();
                table.Borders.Width = 0.75;
                string[] headers = { "Ürün Açıklama", "Miktar", "Birim Fiyat", "KDV", "KDV Tutarı", "İndirim Tutarı", "Tutar", "Net Tutar" };
                foreach (var width in new[] { 4.5, 1.5, 2, 1, 3, 2, 3, 3 })
                    table.AddColumn(Unit.FromCentimeter(width));
                Row header = table.AddRow();
                header.Shading.Color = Colors.LightSlateGray;
                header.Format.Font.Name = "Arial";
                header.Format.Font.Size = 10;
                header.Format.Font.Bold = true;
                header.Format.Alignment = ParagraphAlignment.Center;
                for (int i = 0; i < headers.Length; i++)
                {
                    var p = header.Cells[i].AddParagraph(headers[i]);
                    p.Format.Alignment = ParagraphAlignment.Center;
                    header.Cells[i].VerticalAlignment = VerticalAlignment.Center;
                }
                decimal kur = 1;
                string kurText = FicheOrder.Rows[0]["Kur"].ToString(); // örnek: "35 $"
                string kurBirim = "TL";

                // Kuru ayrıştırıyoruz
                if (!kurText.StartsWith("0") && kurText.Contains(" "))
                {
                    string[] kurParca = kurText.Split(' ');
                    kur = Convert.ToDecimal(kurParca[0]);
                    if (kur == 0) kur = 1;
                    kurBirim = kurParca[1]; // örnek: USD
                }
                string FormatMoney(string value)
                {
                    if (decimal.TryParse(value.Replace("₺", "").Replace(".", "").Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal val))
                    {
                        decimal yeniDeger = val / kur;
                        return $"{yeniDeger:N2} {kurBirim}";
                    }
                    return value;
                }
                foreach (DataRow row in OrderDetails.Rows)
                {
                    Row newRow = table.AddRow();
                    newRow.Format.Font.Name = "Arial";
                    newRow.Format.Font.Size = 9;
                    newRow.Borders.Color = Colors.Gray;
                    newRow.Borders.Width = 0.5;

                    for (int i = 0; i < OrderDetails.Columns.Count - 1; i++)
                    {
                        string value = row[i].ToString();
                        if (kur > 1 && (i == 2 || i == 4 || i == 5 || i == 6 || i == 7))
                            value = FormatMoney(value);

                        Paragraph p = newRow.Cells[i].AddParagraph(value);
                        p.Format.Alignment = ParagraphAlignment.Center;
                        newRow.Cells[i].VerticalAlignment = VerticalAlignment.Center;
                    }
                }

                section.AddParagraph().Format.SpaceAfter = Unit.FromCentimeter(0.5);
                Table totalTable = section.AddTable();
                totalTable.Borders.Width = 0.75;
                totalTable.Rows.LeftIndent = Unit.FromCentimeter(12);
                totalTable.AddColumn(Unit.FromCentimeter(3.5));
                totalTable.AddColumn(Unit.FromCentimeter(4));
                void AddTotalRow(string label, string value, MigraColor color)
                {
                    Row row = totalTable.AddRow();
                    var labelPara = row.Cells[0].AddParagraph(label);
                    labelPara.Format.Alignment = ParagraphAlignment.Left;
                    labelPara.Format.Font.Name = "Arial";
                    labelPara.Format.Font.Size = 10;
                    labelPara.Format.Font.Bold = true;
                    var valuePara = row.Cells[1].AddParagraph(value);
                    valuePara.Format.Alignment = ParagraphAlignment.Right;
                    valuePara.Format.Font.Name = "Arial";
                    valuePara.Format.Font.Size = 10;
                    row.Shading.Color = color;
                    row.Format.Font.Bold = true;
                    row.Format.Font.Size = 11;
                }
                AddTotalRow("Toplam:", FormatMoney(FicheOrder.Rows[0]["Sipariş Brüt"].ToString()), new MigraColor(211, 211, 211));
                AddTotalRow("KDV:", FormatMoney(FicheOrder.Rows[0]["Sipariş KDV Tutar"].ToString()), new MigraColor(211, 211, 211));
                AddTotalRow("İndirim:", FormatMoney(FicheOrder.Rows[0]["Sipariş İndirim Tutari"].ToString()), new MigraColor(255, 255, 153));
                AddTotalRow("Genel Toplam:", FormatMoney(FicheOrder.Rows[0]["Sipariş Net Toplam"].ToString()), new MigraColor(160, 160, 160));
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                pdfRenderer.Document = doc;
                pdfRenderer.RenderDocument();
                using (MemoryStream ms = new MemoryStream())
                {
                    pdfRenderer.PdfDocument.Save(ms, false);
                    return ms.ToArray();
                }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show(e.Message, "Hatalı PDF Oluşturma İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TextLog.TextLogging(e.Message + " --- " + e.ToString());
                return null;
            }
        }
    }
}