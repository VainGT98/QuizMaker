using Microsoft.Extensions.Configuration;
using Microsoft.Testing.Platform.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizMakerDAL;
using QuizMakerModel;
using QuizMakerBLL;
using System.Configuration;


namespace TestProject1
{
    [TestClass]

    [DeploymentItem("appsettings.json")]

    public sealed class Test1
    {
        private Microsoft.Extensions.Configuration.IConfiguration _config;

        [TestInitialize]
        public void Setup()
        {
            // Costruisce la configurazione leggendo dal file nella cartella di output del test
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        [TestMethod]
        public void TestConnection()
        {
            // Recupera la stringa di connessione
            string? connectionString = _config.GetConnectionString("QuizConnectionString");

            Assert.IsNotNull(connectionString, "The connection string must not be null.");
        }

        //[TestMethod]
        //public void TestConfiguration()
        //{
        //    BLLQuiz bLLQuiz = new BLLQuiz();
        //    QuizModel quiz = new QuizModel
        //    {
        //        Title = "Quiz Test",
        //        UserID = 1,
        //        QuestionsList = new List<QuestionModel>
        //        {
        //            new QuestionModel
        //            {
        //                Text = "Question Test",
        //                OrderNumber = 1,
        //                AnswersList = new List<AnswerModel>
        //                {
        //                    new AnswerModel { Text = "Answer Test", OrderNumber = 1, IsCorrect = true },
        //                    new AnswerModel { Text = "Answer Test", OrderNumber = 2, IsCorrect = false }
        //                }
        //            },
        //            new QuestionModel
        //            {
        //                Text = "Question Test",
        //                OrderNumber = 2,
        //                AnswersList = new List<AnswerModel>
        //                {
        //                    new AnswerModel { Text = "Answer Test", OrderNumber = 1, IsCorrect = false },
        //                    new AnswerModel { Text = "Answer Test", OrderNumber = 2, IsCorrect = true }
        //                }
        //            }
        //        }
        //    };
        //    bLLQuiz.InsertQuiz(quiz);
        //}

        //[TestMethod]
        //public void TestConfiguration()
        //{
        //    QuizModel quiz = new QuizModel();
        //    quiz.QuizID = 20;

        //    QuestionModel question1 = new QuestionModel();
        //    question1.QuestionID = 13;
        //    QuestionModel question2 = new QuestionModel();
        //    question2.QuestionID = 14;


        //    AnswerModel answer1 = new AnswerModel();
        //    answer1.AnswerID = 13;
        //    AnswerModel answer2 = new AnswerModel();
        //    answer2.AnswerID = 14;
        //    AnswerModel answer3 = new AnswerModel();
        //    answer3.AnswerID = 15;
        //    AnswerModel answer4 = new AnswerModel();
        //    answer4.AnswerID = 16;

        //    quiz.QuestionsList = new List<QuestionModel> { question1, question2};
        //    question1.AnswersList = new List<AnswerModel> { answer1, answer2};
        //    question2.AnswersList = new List<AnswerModel> { answer3, answer4};

        //    BLLQuiz bLLQuiz = new BLLQuiz();
        //    bLLQuiz.DeleteQuiz(quiz);

        //}

        [TestMethod]
        public void TestConfiguration()
        {
            BLLQuiz bLLQuiz = new BLLQuiz();
            QuizModel quiz = new QuizModel
            {
                QuizID = 30,
                Title = "Quiz Test Updated5",
                QuestionsList = new List<QuestionModel>
                    {
                        new QuestionModel
                        {
                            QuestionID = 24,
                            Text = "Question 1 Updated5",
                            OrderNumber = 2,
                            AnswersList = new List<AnswerModel>
                            {
                                new AnswerModel { AnswerID = 24, Text = "Answer 1 Updated5", OrderNumber = 1, IsCorrect = false },
                                new AnswerModel { AnswerID = 25, Text = "Answer 2 Updated5", OrderNumber = 2, IsCorrect = true }
                            }
                        },
                        new QuestionModel
                        {
                            QuestionID = 25,
                            Text = "Question 2 Updated5",
                            OrderNumber = 1,
                            AnswersList = new List<AnswerModel>
                            {
                                new AnswerModel { QuestionID = 25, Text = "New Answer1 Update5", OrderNumber = 1, IsCorrect = false },
                                new AnswerModel { QuestionID = 25, Text = "New Answer2 Update5", OrderNumber = 2, IsCorrect = true },
                            }
                        }
                    }
            };
            bLLQuiz.UpdateQuiz(quiz);
        }
    }
}
