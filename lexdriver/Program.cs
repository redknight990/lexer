using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace lexer {
    
    class Program {

        public class Options {
            [Value(0, Required = true, HelpText = "Target input file to parse.")]
            public string Input { get; set; }
            
            [Option('o', "output", Required = false, HelpText = "Target output file to which to write token list.", Default = null)]
            public string Output { get; set; }
            
            [Option('e', "error", Required = false, HelpText = "Target output file to which to write parsing errors.", Default = null)]
            public string OutputError { get; set; }
        }

        static void Main(string[] args) {
            
            // Parse command line options
            Options options = Parser.Default.ParseArguments<Options>(args).Value;
            
            // Output files
            string outputTokensFile = options.Output ?? $"{Path.GetFileNameWithoutExtension(options.Input)}.outlextokens";
            string outputErrorsFile = options.OutputError ?? $"{Path.GetFileNameWithoutExtension(options.Input)}.outlexerrors";
            
            // Create output directories
            FileInfo info = new FileInfo(outputTokensFile);
            if (!info.Directory.Exists)
                Directory.CreateDirectory(info.DirectoryName);
            
            info = new FileInfo(outputErrorsFile);
            if (!info.Directory.Exists)
                Directory.CreateDirectory(info.DirectoryName);
            
            // Open output files
            StreamWriter outTokens  = new StreamWriter(outputTokensFile);
            StreamWriter outErrors = new StreamWriter(outputErrorsFile);
            
            // Parse token list
            IList<Token> tokens = new List<Token>();
            ITokenizer tokenizer = new TokenizerDefault();
            tokenizer.Accept(options.Input);
            while (tokenizer.HasNext()) {
                Token token = tokenizer.Next();
                if (token.type == TokenType.Whitespace)
                    continue;
                tokens.Add(token);
            }
            
            // Write tokens and errors
            foreach (var token in tokens) {
                string line = $"{token.location} [{token.type}, {token.lexeme}]";
                Console.WriteLine(line);
                outTokens.WriteLine(line);

                if (token.type == TokenType.Error) {
                    outErrors.WriteLine($"{token.location} Error — {token.lexeme}");
                } 
            }
            
            // Close files
            outTokens.Close();
            outErrors.Close();
            
        }
        
    }
    
}
