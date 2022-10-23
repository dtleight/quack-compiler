//CsLex file generated from grammar quack
#pragma warning disable 0414
using System;
using System.Text;

public class quacklexer<ET> : AbstractLexer<ET>  {
  Yylex lexer;
  ET shared_state;
  public quacklexer(string n) { lexer = new Yylex(new System.IO.StringReader(n)); }
  public quacklexer(System.IO.FileStream f) { lexer=new Yylex(f); }
  public RawToken next_lt() => lexer.yylex();
  public void set_shared(ET shared) {shared_state=shared;}
}//lexer class

%%
%namespace Fussless
%type RawToken
%eofval{
  return new RawToken("EOF","EOF",yyline,yychar);
%eofval}  
%{
private static int comment_count = 0;
private static int line_char = 0;
%}
%line
%char
%state COMMENT

ALPHA=[A-Za-z]
DIGIT=[0-9]
DIGITS=[0-9]+
FLOATS = [0-9]*\.[0-9]+([eE]([+-]?){DIGITS})?
HEXDIGITS=(0x)[0-9A-Fa-f]*
NEWLINE=((\r\n)|\n)
NONNEWLINE_WHITE_SPACE_CHAR=[\ \t\b\012]
WHITE_SPACE_CHAR=[{NEWLINE}\ \t\b\012]
STRING_TEXT=(\\\"|[^{NEWLINE}\"]|{WHITE_SPACE_CHAR}+)*
COMMENT_TEXT=([^*/\r\n]|[^*\r\n]"/"[^*\r\n]|[^/\r\n]"*"[^/\r\n]|"*"[^/\r\n]|"/"[^*\r\n])*
ALPHANUM=[A-Za-z_][A-Za-z0-9_]*

%% 
<YYINITIAL> {NEWLINE}+ { line_char = yychar+yytext().Length; return null; }
<YYINITIAL> {NONNEWLINE_WHITE_SPACE_CHAR}+ { return null; }
<YYINITIAL> "float" { return new RawToken("float",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "/" { return new RawToken("/",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "%" { return new RawToken("%",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "=" { return new RawToken("=",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "eq" { return new RawToken("eq",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "||" { return new RawToken("||",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "<" { return new RawToken("<",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> ":" { return new RawToken(":",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "}" { return new RawToken("}",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> ")" { return new RawToken(")",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "in" { return new RawToken("in",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "else" { return new RawToken("else",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "^" { return new RawToken("^",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "not" { return new RawToken("not",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> ">" { return new RawToken(">",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "(" { return new RawToken("(",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "-" { return new RawToken("-",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "<=" { return new RawToken("<=",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "," { return new RawToken(",",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "define" { return new RawToken("define",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "lambda" { return new RawToken("lambda",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "while" { return new RawToken("while",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "~" { return new RawToken("~",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "&&" { return new RawToken("&&",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "cons" { return new RawToken("cons",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "]" { return new RawToken("]",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "+" { return new RawToken("+",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "cdr" { return new RawToken("cdr",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "car" { return new RawToken("car",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "if" { return new RawToken("if",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "{" { return new RawToken("{",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "boolean" { return new RawToken("boolean",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "neq" { return new RawToken("neq",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "true" { return new RawToken("true",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "." { return new RawToken(".",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> ">=" { return new RawToken(">=",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "display" { return new RawToken("display",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "nil" { return new RawToken("nil",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "[" { return new RawToken("[",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "--" { return new RawToken("--",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "for" { return new RawToken("for",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "*" { return new RawToken("*",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "++" { return new RawToken("++",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "int" { return new RawToken("int",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "let" { return new RawToken("let",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "String" { return new RawToken("String",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "false" { return new RawToken("false",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> ";" { return new RawToken(";",yytext(),yyline,yychar-line_char,yychar); }
<YYINITIAL> "'" { return new RawToken("'",yytext(),yyline,yychar-line_char,yychar); }

<YYINITIAL> "#".*\n { line_char=yychar+yytext().Length; return null; }
<YYINITIAL,COMMENT> [(\r\n?|\n)] { line_char=yychar+yytext().Length; return null; }

<YYINITIAL> "/*" { yybegin(COMMENT); comment_count = comment_count + 1; return null;
}
<COMMENT> "/*" { comment_count = comment_count + 1; return null; }
<COMMENT> "*/" { 
	comment_count = comment_count - 1;
	if (comment_count == 0) {
            yybegin(YYINITIAL);
        }
        return null;
}

<COMMENT> {COMMENT_TEXT} { return null; }

<YYINITIAL> \"{STRING_TEXT}\" {
        return new RawToken("StrLit",yytext(),yyline,yychar-line_char,yychar);
}
<YYINITIAL> \"{STRING_TEXT} {
	String str =  yytext().Substring(1,yytext().Length);
	Utility.error(Utility.E_UNCLOSEDSTR);
        return new RawToken("Unclosed String",str,yyline,yychar-line_char,yychar);
}

<YYINITIAL> {DIGIT}+ { 
  return new RawToken("Num",yytext(),yyline,yychar-line_char,yychar);
}
<YYINITIAL> {HEXDIGITS} { 
return new RawToken("Hexnum",yytext(),yyline,yychar-line_char,yychar);  
}
<YYINITIAL> {FLOATS} { 
  return new RawToken("Float",yytext(),yyline,yychar-line_char,yychar);
}	
<YYINITIAL> ({ALPHA}|_)({ALPHA}|{DIGIT}|_)* {
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}	
<YYINITIAL,COMMENT> . {
	StringBuilder sb = new StringBuilder("Illegal character: <");
	String s = yytext();
	for (int i = 0; i < s.Length; i++)
	  if (s[i] >= 32)
	    sb.Append(s[i]);
	  else
	    {
	    sb.Append("^");
	    sb.Append(Convert.ToChar(s[i]+'A'-1));
	    }
        sb.Append(">");
	Console.WriteLine(sb.ToString());	
	Utility.error(Utility.E_UNMATCHED);
        return null;
}
