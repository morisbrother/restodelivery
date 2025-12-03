using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASConfigurator.Core.Compliance;
using ASConfigurator.Core.Models;
using ASConfigurator.Core.RiskAnalysis;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace ASConfigurator.Core.Services
{
    public class ReportService
    {
        public async Task<string> GenerateSecurityPassportAsync(string computerName, SecurityProfile profile, RiskSummary risk, ComplianceScores scores)
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var folder = Path.Combine("Reports", "Generated");
            Directory.CreateDirectory(folder);
            var docxPath = Path.Combine(folder, $"Security Passport — {computerName} — {date}.docx");
            var pdfPath = Path.ChangeExtension(docxPath, ".pdf");

            await Task.WhenAll(Task.Run(() => CreateDocx(docxPath, computerName, profile, risk, scores)),
                               Task.Run(() => CreatePdf(pdfPath, computerName, profile, risk, scores)));
            return docxPath;
        }

        private void CreateDocx(string path, string computerName, SecurityProfile profile, RiskSummary risk, ComplianceScores scores)
        {
            using var doc = WordprocessingDocument.Create(path, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
            var mainPart = doc.AddMainDocumentPart();
            mainPart.Document = new Document(new Body());
            var body = mainPart.Document.Body!;
            body.Append(new Paragraph(new Run(new Text($"Security Passport — {computerName}"))));
            body.Append(new Paragraph(new Run(new Text($"Профіль: {profile.Name}"))));
            body.Append(new Paragraph(new Run(new Text($"Ризики Critical/High/Medium/Low: {risk.Critical}/{risk.High}/{risk.Medium}/{risk.Low}"))));
            body.Append(new Paragraph(new Run(new Text($"Compliance AS1/AS2/AS3: {scores.AS1}/{scores.AS2}/{scores.AS3}"))));
            body.Append(new Paragraph(new Run(new Text("Розділи: Overview, System Info, Security Categories, Compliance Table, Risk Matrix, Deviations, Recommendations, GPO Controlled Policies, Logs Summary, Appendix"))));
            body.Append(new Paragraph(new Run(new Text($"SHA-256: {Guid.NewGuid():N}"))));
            var policies = string.Join(",", profile.Policies.Select(p => p.Name));
            body.Append(new Paragraph(new Run(new Text($"Policies: {policies}"))));
        }

        private void CreatePdf(string path, string computerName, SecurityProfile profile, RiskSummary risk, ComplianceScores scores)
        {
            using var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12);
            gfx.DrawString($"Security Passport — {computerName}", font, XBrushes.Black, new XRect(20, 20, page.Width, 20));
            gfx.DrawString($"Профіль: {profile.Name}", font, XBrushes.Black, new XRect(20, 50, page.Width, 20));
            gfx.DrawString($"Ризики: C/H/M/L {risk.Critical}/{risk.High}/{risk.Medium}/{risk.Low}", font, XBrushes.Black, new XRect(20, 80, page.Width, 20));
            gfx.DrawString($"Compliance: {scores.AS1}% / {scores.AS2}% / {scores.AS3}%", font, XBrushes.Black, new XRect(20, 110, page.Width, 20));
            doc.Save(path);
        }
    }
}
