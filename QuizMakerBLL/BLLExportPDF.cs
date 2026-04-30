using iTextSharp.text;
using iTextSharp.text.pdf;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QuizMakerBLL
{
    public class BLLExportPDF
    {
        public void ExportQuizToPdf(QuizModel quiz, string filePath)
        {
            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            // Titolo del quiz
            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 24, iTextSharp.text.Font.BOLD);
            document.Add(new Paragraph(quiz.Title, titleFont));

            // Domande
            foreach (QuestionModel question in quiz.QuestionsList.OrderBy(q => q.OrderNumber))
            {
                iTextSharp.text.Font questionFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD);
                Paragraph questionParagraph = new Paragraph($"Question {question.OrderNumber}: {question.Text}", questionFont);
                questionParagraph.SpacingBefore = 10f;
                document.Add(questionParagraph);

                // Immagine (se presente)
                if (question.Image != null)
                {
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(question.Image);
                    image.ScaleToFit(200f, 100f);
                    image.SpacingAfter = 5f;
                    document.Add(image);
                }

                // Risposte
                foreach (AnswerModel answer in question.AnswersList.OrderBy(a => a.OrderNumber))
                {
                    iTextSharp.text.Font answerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12);
                    Paragraph answerParagraph = new Paragraph($"- {answer.Text}", answerFont);
                    answerParagraph.IndentationLeft = 20f;
                    document.Add(answerParagraph);
                }
            }

            document.Close();
        }
    }
}
