using System.Linq.Expressions;
namespace graver
{
    public enum TokenType
    {
        IDENTIFIER,
        KEYWORD,
        OPERATOR,
        SYMBOL,
        NUMBER,
        STRING,
        COMMENT,
        NEWLINE
    }
}