using System.Text;

namespace Unity.SerializationLogic
{
    public class IntendStringBuilder
    {
        private StringBuilder sb = new StringBuilder();

        public int Intend = 0;

        public void Append(object value)
        {
            sb.Append(value);
        }
        public void AppendLine(string value)
        {
            if (value == null)
            {
                return;
            }
            sb.Append(value);
            sb.Append("\n");
        }

        public void IntendAppend(string value,int offsetIntend=0)
        {
            if (value==null)
            {
                return;
            }
            for (int i = 0; i < Intend+ offsetIntend; i++)
            {
                sb.Append("\t");
            }
            sb.Append(value);
        }

        public void IntendAppendLine(string value, int offsetIntend = 0)
        {
            IntendAppend(value,offsetIntend);
            sb.Append("\n");
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}