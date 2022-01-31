namespace lexer {
    
    /// <summary>
    /// Represents a parsed token.
    /// </summary>
    public struct Token {
        public TokenType type;
        public Location location;
        public string lexeme;

        public override string ToString() {
            return $"{location} [{type}] - '{lexeme}'";
        }
    }
    
}
