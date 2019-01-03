using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebParser.App
{
    public interface IValidator<T>
    {
        string GetValidationResultString(T checkedObject);
        bool IsValid(T checkedObject);
    }
}