using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebParser.App
{
    public interface IValidator<T>
    {
        bool IsValid(T checkedObject, out string validationResultString);
        //string GetValidationResultString(T checkedObject);
    }
}