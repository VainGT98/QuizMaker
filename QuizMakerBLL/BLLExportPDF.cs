using iTextSharp.text;
using iTextSharp.text.pdf;
using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Font titleFont = new Font(Font.FontFamily.HELVETICA, 24, Font.BOLD);
            document.Add(new Paragraph(quiz.Title, titleFont));

            // Domande
            foreach (QuestionModel question in quiz.QuestionsList.OrderBy(q => q.OrderNumber))
            {
                Font questionFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);
                Paragraph questionParagraph = new Paragraph($"Question {question.OrderNumber}: {question.Text}", questionFont);
                questionParagraph.SpacingBefore = 10f;
                document.Add(questionParagraph);

                // Immagine (se presente)
                if (question.Image != null)
                {
                    Image image = Image.GetInstance(question.Image);
                    image.ScaleToFit(200f, 100f);
                    image.SpacingAfter = 5f;
                    document.Add(image);
                }

                // Risposte
                foreach (AnswerModel answer in question.AnswersList.OrderBy(a => a.OrderNumber))
                {
                    Font answerFont = new Font(Font.FontFamily.HELVETICA, 12);
                    Paragraph answerParagraph = new Paragraph($"- {answer.Text}", answerFont);
                    answerParagraph.IndentationLeft = 20f;
                    document.Add(answerParagraph);
                }
            }

            document.Close();
        }
    }
}
