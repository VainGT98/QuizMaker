using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMakerBLL
{
    public class BLLExportCSV
    {
        public void ExportQuizToCsv(QuizModel quiz, string filePath)
        {
            using StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8);

            // Header
            writer.WriteLine("QuestionNumber;QuestionText;AnswerNumber;AnswerText");

            foreach (QuestionModel question in quiz.QuestionsList.OrderBy(q => q.OrderNumber))
            {
                foreach (AnswerModel answer in question.AnswersList.OrderBy(a => a.OrderNumber))
                {
                    string line = $"{question.OrderNumber};" +
                                  $"{EscapeCsv(question.Text)};" +
                                  $"{answer.OrderNumber};" +
                                  $"{EscapeCsv(answer.Text)}";
                    writer.WriteLine(line);
                }
            }
        }

        private string EscapeCsv(string value)
        {
            if (value.Contains(';') || value.Contains('"') || value.Contains('\n'))
                return $"\"{value.Replace("\"", "\"\"")}\"";
            return value;
        }
    }
}
