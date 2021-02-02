using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNQuiz.Alice.Models
{
    public class AnswersModel : List<KeyValuePair<string, string>>
    {
        public void Add(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            this.Add(new KeyValuePair<string, string>(PrepareText(value)!, value));
        }

        public void Insert(int index, string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Insert(index, new KeyValuePair<string, string>(PrepareText(value)!, value));
        }

        public static string? PrepareText(string? value)
        {
            if(value == null)
            {
                return null;
            }
            value = value.Replace("+", string.Empty);
            if(value.EndsWith("-м"))
            {
                value = value.TrimEnd('м').TrimEnd('-');
            }
            return value;
        }


    }
}
