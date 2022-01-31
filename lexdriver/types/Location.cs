namespace lexer {
    
    /// <summary>
    /// Represents a specific location in a source file.
    /// </summary>
    public struct Location {
        public string sourceFile;
        public int line; 
        public int column;

        public override string ToString() {
            return $"({sourceFile}:{line}:{column})";
        }
    }

}
