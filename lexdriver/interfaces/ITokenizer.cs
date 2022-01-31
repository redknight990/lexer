namespace lexer {
    
    /// <summary>
    /// Represents a generic tokenizer implementation.
    /// </summary>
    public interface ITokenizer {

        /// <summary>
        /// Resets the tokenizer to the default state and reads the specified
        /// input file to retrieve the character stream to tokenize.
        /// </summary>
        /// <param name="inputFile">The path the file to use as a character stream.</param>
        public void Accept(string inputFile);

        /// <summary>
        /// Returns true if the current character stream still contains more tokens.
        /// </summary>
        /// <returns>True if there are more tokens left to be parsed, false otherwise.</returns>
        public bool HasNext();
        
        /// <summary>
        /// Returns the next token parsed from the character stream. If there are no more tokens
        /// to parse, this method will return an EOF token.
        /// </summary>
        /// <returns>The next parsed token or an EOF token if there are no more to be parsed.</returns>
        public Token Next();

    }
    
}
