using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace timetrees
{
    class AnswerLogic
    {
        public static bool GetAnswer()
        {
            string answer;
            do
            {
                answer = Console.ReadLine();
                if (GetNegativeAnswer(answer))
                {
                    return false;
                }
                if (GetPositiveAnswer(answer))
                {
                    return true;
                }
                if (!GetPositiveAnswer(answer)) Console.WriteLine("Ваш ответ некорректен, введите Y/N");
            } while (!GetPositiveAnswer(answer) & !GetNegativeAnswer(answer));
            return false;
        }
        public static bool GetNegativeAnswer(string answer)
        {
            string[] negativeAnswers = new[] { "n", "no", "N", "No", "NO", "н", "Н", "нет", "Нет", "НЕТ" };
            if (negativeAnswers.Contains(answer)) return true;
            else return false;
        }

        public static bool GetPositiveAnswer(string answer)
        {
            string[] positiveAnswers = new[] { "y", "yes", "Y", "Yes", "YES", "д", "Д", "да", "Да", "ДА" };
            if (positiveAnswers.Contains(answer)) return true;
            else return false;
        }

    }
}
