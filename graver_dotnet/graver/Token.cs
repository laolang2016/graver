namespace graver
{
    public class Token<T>
    {

        /// <summary>
        /// token 类型
        /// </summary>
        private TokenType tokenType;
        public TokenType TokenType
        {
            get { return tokenType; }
            set { tokenType = value; }
        }


        private int flags;
        public int Flags
        {
            get { return flags; }
            set { flags = value; }
        }


        /// <summary>
        /// 数据
        /// </summary>
        private T data;

        public T Data()
        {
            return data;
        }

        public void Data(T data)
        {
            this.data = data;
        }

        /// <summary>
        /// 是否空白自负
        /// </summary>
        private bool isWhitespace;
        public bool IsWhitespace
        {
            get { return isWhitespace; }
            set { isWhitespace = value; }
        }

        /// <summary>
        /// 括号中间的数据 (for debug)
        /// </summary>
        private string betweenBrackets;
        public string BetweenBrackets
        {
            get { return betweenBrackets; }
            set { betweenBrackets = value; }
        }
    }
}