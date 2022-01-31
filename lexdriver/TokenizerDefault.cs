using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace lexer {
    
    /// <summary>
    /// Default implementation of the tokenizer for the language.
    /// </summary>
    public class TokenizerDefault : ITokenizer {

        private string _file;
        private string _input;
        private int _cursor;

        private static readonly IDictionary<Regex, TokenType> TokenMap = new Dictionary<Regex, TokenType>() {
            { new Regex(@"^\s+"), TokenType.Whitespace },
            { new Regex(@"^\/\/.*"), TokenType.Comment },
            { new Regex(@"^\/\*[\s\S]*?\*\/"), TokenType.BlockComment },
            { new Regex(@"^(?:==|<>|<=|>=|->|<|>|\+|-|\*|/|=|\||&|!)"), TokenType.Operator },
            { new Regex(@"^(?:\(|\)|{|}|\[|]|;|,|\.|::|:)"), TokenType.Punctuator },
            { new Regex(@"^(?:if|then|else|integer|float|void|public|private|func|var|struct|while|read|write|return|self|inherits|let|impl)"), TokenType.Keyword },
            { new Regex(@"^[a-zA-Z][a-zA-Z0-9_]*"), TokenType.Identifier },
            { new Regex(@"^[+\-]?(?:[1-9][0-9]*|0)?(?:\.[0-9]*[1-9]|\.0)(?:[eE][+\-]?(?:[1-9][0-9]*|0))?"), TokenType.Float },
            { new Regex(@"^(?:[1-9][0-9]*|0)"), TokenType.Integer }
        };

        /// <inheritdoc/>
        public void Accept(string inputFile) {
            _file = inputFile;
            _input = System.IO.File.ReadAllText(inputFile).Replace("\r\n", "\n");
            _cursor = 0;
        }

        /// <inheritdoc/>
        public bool HasNext() {
            return _cursor < _input.Length;
        }

        /// <inheritdoc/>
        public Token Next() {
            // Return end of file (eof) token if no more input
            if (!HasNext()) {
                return new Token {
                    type = TokenType.EOF,
                    location = GetCursorLocation(),
                    lexeme = "$"
                };
            }
            
            // Truncate the input string to get the remaining input
            string remainingInput = _input.Substring(_cursor);
            KeyValuePair<Regex, TokenType> longestMatchEntry = new KeyValuePair<Regex, TokenType>();
            Match longestMatch = null;
            
            // Find longest regex match
            foreach (var entry in TokenMap) {
                Match match = entry.Key.Match(remainingInput);
                if (!match.Success)
                    continue;

                if (longestMatch == null || match.Groups[0].Value.Length > longestMatch.Groups[0].Value.Length) {
                    longestMatch = match;
                    longestMatchEntry = entry;
                }
            }
            
            // Get current cursor location
            var location = GetCursorLocation();
            
            // We have a match!
            if (longestMatch != null) {
                _cursor += longestMatch.Groups[0].Length;
                return new Token {
                    type = longestMatchEntry.Value,
                    location = location,
                    lexeme = longestMatch.Groups[0].Value
                };
            }
            
            // Match the next character sequence and return it as an error token
            longestMatch = new Regex(@"^\S+").Match(remainingInput);
            _cursor += longestMatch.Groups[0].Length;
            return new Token {
                type = TokenType.Error,
                location = location,
                lexeme = $"Invalid character sequence: {longestMatch.Groups[0].Value}"
            };
        }

        /// <summary>
        /// Returns a Location object describing the current cursor location.
        /// </summary>
        /// <returns>
        /// The current cursor location.
        /// </returns>
        private Location GetCursorLocation() {
            string str = _input.Substring(0, _cursor);
            string[] lines = str.Split("\n");
            int line = lines.Length;
            int column = lines[^1].Length + 1;
            return new Location {
                sourceFile = _file,
                line = line,
                column = column
            };
        }

    }
    
}
