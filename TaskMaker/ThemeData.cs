using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaker
{
    [Serializable]
    public class ThemeData
    {
        public string ThemeName;
        public List<List<QuestionData>> Questions = new List<List<QuestionData>>();

        public ThemeData() { }
    }
}
