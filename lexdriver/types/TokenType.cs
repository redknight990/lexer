namespace lexer {
    
    /// <summary>
    /// These are the only valid token types that can be parsed.
    /// </summary>
    public enum TokenType {
        Error,
        Whitespace,
        Comment,
        BlockComment,
        Identifier,
        Integer,
        Float,
        Operator,
        Punctuator,
        Keyword,
        EOF
    }
    
}
