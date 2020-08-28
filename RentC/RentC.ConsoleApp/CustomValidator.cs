using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.ConsoleApp
{
    public class CustomValidator
    {
        private InAppBehavior inAppBehavior;

        public CustomValidator(InAppBehavior inAppBehavior)
        {
            this.inAppBehavior = inAppBehavior;
        }

        public bool IsValid<T>(T model, Action<bool> curAction, bool creating)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            bool isValid = Validator.TryValidateObject(model, context, results, true);
            if (!isValid)
            {
                Console.WriteLine();
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                inAppBehavior.ContinueOrQuit(curAction, inAppBehavior.Menu, creating);
            }

            return isValid;
        }
    }
}
