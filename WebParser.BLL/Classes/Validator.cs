using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.App;

namespace WebParser.App
{
    public class Validator<T> : IValidator<T>
    {
        public bool IsValid(T checkedObject, out string validationResultString)
        {
            var context = new ValidationContext(checkedObject);
            var results = new List<ValidationResult>();
            StringBuilder resultString = new StringBuilder();

            var validationResult = Validator.TryValidateObject(checkedObject, context, results, true);

            if (results.Count() > 0)
            {
                resultString.AppendLine("We found the next validations errors:");
                results.ToList()
                    .ForEach(
                        s =>
                            resultString.AppendFormat("  {0} --> {1}{2}", s.MemberNames.FirstOrDefault(), s.ErrorMessage, Environment.NewLine));
            }
            else
            {
                resultString.AppendLine("We not found validations errors.");
            }

            validationResultString = resultString.ToString();

            return validationResult;
        }
    }
}











//public string GetValidationResultString(T checkedObject)
//{
//    var context = new ValidationContext(checkedObject);
//    var results = new List<ValidationResult>();
//    StringBuilder resultString = new StringBuilder();

//    Validator.TryValidateObject(checkedObject, context, results, true);


//    if (results.Count() > 0)
//    {
//        resultString.AppendLine("We found the next validations errors:");
//        results.ToList()
//            .ForEach(
//                s =>
//                    resultString.AppendFormat("  {0} --> {1}{2}", s.MemberNames.FirstOrDefault(), s.ErrorMessage, Environment.NewLine));
//    }
//    else
//        resultString.AppendLine("We not found validations errors.");

//    return resultString.ToString();
//}

/*
    if (!Validator.TryValidateObject(checkedObject, context, results, true))
    {
        foreach (var error in results)
        {
            Console.WriteLine(error.ErrorMessage);
        }
    }

    return true;


    var results = new List<ValidationResult>();
    var context = new ValidationContext(requestDTO);
    if (!Validator.TryValidateObject(requestDTO, context, results, true))
    {
        foreach (var error in results)
        {
            Console.WriteLine(error.ErrorMessage);
        }
    }


    var results2 = new List<ValidationResult>();
    var context2 = new ValidationContext(requestDTO, null, null);
    var isValid = Validator.TryValidateObject(requestDTO, context2, results2, true);


    ValidationContext valContext = new ValidationContext(source, null, null);
    var result = new List<ValidationResult>();
    Validator.TryValidateObject(source, valContext, result, true);

    return result;

    public static string ToDescErrorsString(this IEnumerable<ValidationResult> source, string messageEmptyCollection = null)
    {
        StringBuilder result = new StringBuilder();

        if (source.Count() > 0)
        {
            result.AppendLine("We found the next validations errors:");
            source.ToList()
                .ForEach(
                    s =>
                        result.AppendFormat("  {0} --> {1}{2}", s.MemberNames.FirstOrDefault(), s.ErrorMessage,
                            Environment.NewLine));
        }
        else
            result.AppendLine(messageEmptyCollection ?? string.Empty);

        return result.ToString();
    }



}
*/



