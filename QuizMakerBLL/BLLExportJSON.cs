using QuizMakerModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizMakerBLL
{
    public class BLLExportJSON
    {
        public void ExportQuizToJson(QuizModel quiz, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(quiz, options);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }
    }
}
