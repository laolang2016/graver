namespace graver
{
    public class GraverException : Exception
    {

        public const string CODE_OK = "0";
        public const string CODE_FAILURE = "1";

        public GraverException() : base("系统错误")
        {
            this.code = CODE_FAILURE;
            this.msg = "系统错误";
        }

        public GraverException(string msg) : base(msg)
        {
            this.code = CODE_FAILURE;
            this.msg = msg;
        }

        public GraverException(string code, string msg) : base(msg)
        {
            this.code = code;
            this.msg = msg;
        }

        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }


        private string msg;
        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }
    }
}