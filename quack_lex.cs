namespace Fussless
{
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
/* test */


internal class Yylex
{
private const int YY_BUFFER_SIZE = 512;
private const int YY_F = -1;
private const int YY_NO_STATE = -1;
private const int YY_NOT_ACCEPT = 0;
private const int YY_START = 1;
private const int YY_END = 2;
private const int YY_NO_ANCHOR = 4;
delegate RawToken AcceptMethod();
AcceptMethod[] accept_dispatch;
private const int YY_BOL = 128;
private const int YY_EOF = 129;

private static int comment_count = 0;
private static int line_char = 0;
private System.IO.TextReader yy_reader;
private int yy_buffer_index;
private int yy_buffer_read;
private int yy_buffer_start;
private int yy_buffer_end;
private char[] yy_buffer;
private int yychar;
private int yyline;
private bool yy_at_bol;
private int yy_lexical_state;

internal Yylex(System.IO.TextReader reader) : this()
  {
  if (null == reader)
    {
    throw new System.ApplicationException("Error: Bad input stream initializer.");
    }
  yy_reader = reader;
  }

internal Yylex(System.IO.FileStream instream) : this()
  {
  if (null == instream)
    {
    throw new System.ApplicationException("Error: Bad input stream initializer.");
    }
  yy_reader = new System.IO.StreamReader(instream);
  }

private Yylex()
  {
  yy_buffer = new char[YY_BUFFER_SIZE];
  yy_buffer_read = 0;
  yy_buffer_index = 0;
  yy_buffer_start = 0;
  yy_buffer_end = 0;
  yychar = 0;
  yyline = 0;
  yy_at_bol = true;
  yy_lexical_state = YYINITIAL;
accept_dispatch = new AcceptMethod[] 
 {
  null,
  null,
  new AcceptMethod(this.Accept_2),
  new AcceptMethod(this.Accept_3),
  new AcceptMethod(this.Accept_4),
  new AcceptMethod(this.Accept_5),
  new AcceptMethod(this.Accept_6),
  new AcceptMethod(this.Accept_7),
  new AcceptMethod(this.Accept_8),
  new AcceptMethod(this.Accept_9),
  new AcceptMethod(this.Accept_10),
  new AcceptMethod(this.Accept_11),
  new AcceptMethod(this.Accept_12),
  new AcceptMethod(this.Accept_13),
  new AcceptMethod(this.Accept_14),
  new AcceptMethod(this.Accept_15),
  new AcceptMethod(this.Accept_16),
  new AcceptMethod(this.Accept_17),
  new AcceptMethod(this.Accept_18),
  new AcceptMethod(this.Accept_19),
  new AcceptMethod(this.Accept_20),
  new AcceptMethod(this.Accept_21),
  new AcceptMethod(this.Accept_22),
  new AcceptMethod(this.Accept_23),
  new AcceptMethod(this.Accept_24),
  new AcceptMethod(this.Accept_25),
  new AcceptMethod(this.Accept_26),
  new AcceptMethod(this.Accept_27),
  new AcceptMethod(this.Accept_28),
  new AcceptMethod(this.Accept_29),
  new AcceptMethod(this.Accept_30),
  new AcceptMethod(this.Accept_31),
  new AcceptMethod(this.Accept_32),
  new AcceptMethod(this.Accept_33),
  new AcceptMethod(this.Accept_34),
  new AcceptMethod(this.Accept_35),
  new AcceptMethod(this.Accept_36),
  new AcceptMethod(this.Accept_37),
  new AcceptMethod(this.Accept_38),
  new AcceptMethod(this.Accept_39),
  new AcceptMethod(this.Accept_40),
  new AcceptMethod(this.Accept_41),
  new AcceptMethod(this.Accept_42),
  new AcceptMethod(this.Accept_43),
  new AcceptMethod(this.Accept_44),
  new AcceptMethod(this.Accept_45),
  new AcceptMethod(this.Accept_46),
  new AcceptMethod(this.Accept_47),
  new AcceptMethod(this.Accept_48),
  new AcceptMethod(this.Accept_49),
  new AcceptMethod(this.Accept_50),
  new AcceptMethod(this.Accept_51),
  new AcceptMethod(this.Accept_52),
  new AcceptMethod(this.Accept_53),
  new AcceptMethod(this.Accept_54),
  new AcceptMethod(this.Accept_55),
  new AcceptMethod(this.Accept_56),
  new AcceptMethod(this.Accept_57),
  new AcceptMethod(this.Accept_58),
  new AcceptMethod(this.Accept_59),
  new AcceptMethod(this.Accept_60),
  new AcceptMethod(this.Accept_61),
  new AcceptMethod(this.Accept_62),
  new AcceptMethod(this.Accept_63),
  new AcceptMethod(this.Accept_64),
  new AcceptMethod(this.Accept_65),
  new AcceptMethod(this.Accept_66),
  null,
  new AcceptMethod(this.Accept_68),
  new AcceptMethod(this.Accept_69),
  new AcceptMethod(this.Accept_70),
  new AcceptMethod(this.Accept_71),
  new AcceptMethod(this.Accept_72),
  new AcceptMethod(this.Accept_73),
  new AcceptMethod(this.Accept_74),
  new AcceptMethod(this.Accept_75),
  null,
  new AcceptMethod(this.Accept_77),
  new AcceptMethod(this.Accept_78),
  new AcceptMethod(this.Accept_79),
  new AcceptMethod(this.Accept_80),
  null,
  new AcceptMethod(this.Accept_82),
  new AcceptMethod(this.Accept_83),
  new AcceptMethod(this.Accept_84),
  new AcceptMethod(this.Accept_85),
  null,
  new AcceptMethod(this.Accept_87),
  new AcceptMethod(this.Accept_88),
  null,
  new AcceptMethod(this.Accept_90),
  null,
  new AcceptMethod(this.Accept_92),
  null,
  new AcceptMethod(this.Accept_94),
  new AcceptMethod(this.Accept_95),
  new AcceptMethod(this.Accept_96),
  new AcceptMethod(this.Accept_97),
  new AcceptMethod(this.Accept_98),
  new AcceptMethod(this.Accept_99),
  new AcceptMethod(this.Accept_100),
  new AcceptMethod(this.Accept_101),
  new AcceptMethod(this.Accept_102),
  new AcceptMethod(this.Accept_103),
  new AcceptMethod(this.Accept_104),
  new AcceptMethod(this.Accept_105),
  new AcceptMethod(this.Accept_106),
  new AcceptMethod(this.Accept_107),
  new AcceptMethod(this.Accept_108),
  new AcceptMethod(this.Accept_109),
  new AcceptMethod(this.Accept_110),
  new AcceptMethod(this.Accept_111),
  new AcceptMethod(this.Accept_112),
  new AcceptMethod(this.Accept_113),
  new AcceptMethod(this.Accept_114),
  new AcceptMethod(this.Accept_115),
  new AcceptMethod(this.Accept_116),
  new AcceptMethod(this.Accept_117),
  new AcceptMethod(this.Accept_118),
  new AcceptMethod(this.Accept_119),
  new AcceptMethod(this.Accept_120),
  new AcceptMethod(this.Accept_121),
  new AcceptMethod(this.Accept_122),
  new AcceptMethod(this.Accept_123),
  new AcceptMethod(this.Accept_124),
  new AcceptMethod(this.Accept_125),
  new AcceptMethod(this.Accept_126),
  new AcceptMethod(this.Accept_127),
  new AcceptMethod(this.Accept_128),
  new AcceptMethod(this.Accept_129),
  new AcceptMethod(this.Accept_130),
  new AcceptMethod(this.Accept_131),
  new AcceptMethod(this.Accept_132),
  new AcceptMethod(this.Accept_133),
  new AcceptMethod(this.Accept_134),
  new AcceptMethod(this.Accept_135),
  new AcceptMethod(this.Accept_136),
  new AcceptMethod(this.Accept_137),
  new AcceptMethod(this.Accept_138),
  new AcceptMethod(this.Accept_139),
  new AcceptMethod(this.Accept_140),
  new AcceptMethod(this.Accept_141),
  new AcceptMethod(this.Accept_142),
  new AcceptMethod(this.Accept_143),
  new AcceptMethod(this.Accept_144),
  };
  }

RawToken Accept_2()
    { // begin accept action #2
{ line_char=yychar+yytext().Length; return null; }
    } // end accept action #2

RawToken Accept_3()
    { // begin accept action #3
{ line_char = yychar+yytext().Length; return null; }
    } // end accept action #3

RawToken Accept_4()
    { // begin accept action #4
{ return null; }
    } // end accept action #4

RawToken Accept_5()
    { // begin accept action #5
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #5

RawToken Accept_6()
    { // begin accept action #6
{ return new RawToken("-",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #6

RawToken Accept_7()
    { // begin accept action #7
{ return new RawToken(">",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #7

RawToken Accept_8()
    { // begin accept action #8
{ return new RawToken("+",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #8

RawToken Accept_9()
    { // begin accept action #9
{ return new RawToken("{",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #9

RawToken Accept_10()
    { // begin accept action #10
{ return new RawToken("/",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #10

RawToken Accept_11()
    { // begin accept action #11
{ return new RawToken("]",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #11

RawToken Accept_12()
    { // begin accept action #12
{ return new RawToken("=",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #12

RawToken Accept_13()
    { // begin accept action #13
{ return new RawToken(":",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #13

RawToken Accept_14()
    { // begin accept action #14
{ return new RawToken("}",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #14

RawToken Accept_15()
    { // begin accept action #15
{ return new RawToken(")",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #15

RawToken Accept_16()
    { // begin accept action #16
{ return new RawToken(",",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #16

RawToken Accept_17()
    { // begin accept action #17
{ return new RawToken(".",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #17

RawToken Accept_18()
    { // begin accept action #18
{
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
    } // end accept action #18

RawToken Accept_19()
    { // begin accept action #19
{ return new RawToken("^",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #19

RawToken Accept_20()
    { // begin accept action #20
{ return new RawToken("~",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #20

RawToken Accept_21()
    { // begin accept action #21
{ return new RawToken("<",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #21

RawToken Accept_22()
    { // begin accept action #22
{ return new RawToken("*",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #22

RawToken Accept_23()
    { // begin accept action #23
{ return new RawToken("%",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #23

RawToken Accept_24()
    { // begin accept action #24
{ return new RawToken("[",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #24

RawToken Accept_25()
    { // begin accept action #25
{ return new RawToken("(",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #25

RawToken Accept_26()
    { // begin accept action #26
{ return new RawToken(";",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #26

RawToken Accept_27()
    { // begin accept action #27
{ return new RawToken("'",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #27

RawToken Accept_28()
    { // begin accept action #28
{
	String str =  yytext().Substring(1,yytext().Length);
	Utility.error(Utility.E_UNCLOSEDSTR);
        return new RawToken("Unclosed String",str,yyline,yychar-line_char,yychar);
}
    } // end accept action #28

RawToken Accept_29()
    { // begin accept action #29
{ 
  return new RawToken("Num",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #29

RawToken Accept_30()
    { // begin accept action #30
{ return new RawToken("in",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #30

RawToken Accept_31()
    { // begin accept action #31
{ return new RawToken("if",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #31

RawToken Accept_32()
    { // begin accept action #32
{ return new RawToken("--",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #32

RawToken Accept_33()
    { // begin accept action #33
{ return new RawToken(">=",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #33

RawToken Accept_34()
    { // begin accept action #34
{ return new RawToken("++",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #34

RawToken Accept_35()
    { // begin accept action #35
{ yybegin(COMMENT); comment_count = comment_count + 1; return null;
}
    } // end accept action #35

RawToken Accept_36()
    { // begin accept action #36
{ return new RawToken("eq",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #36

RawToken Accept_37()
    { // begin accept action #37
{ 
  return new RawToken("Float",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #37

RawToken Accept_38()
    { // begin accept action #38
{ return new RawToken("&&",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #38

RawToken Accept_39()
    { // begin accept action #39
{ return new RawToken("!=",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #39

RawToken Accept_40()
    { // begin accept action #40
{ return new RawToken("<=",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #40

RawToken Accept_41()
    { // begin accept action #41
{ return new RawToken("||",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #41

RawToken Accept_42()
    { // begin accept action #42
{
        return new RawToken("StrLit",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #42

RawToken Accept_43()
    { // begin accept action #43
{ 
return new RawToken("Hexnum",yytext(),yyline,yychar-line_char,yychar);  
}
    } // end accept action #43

RawToken Accept_44()
    { // begin accept action #44
{ return new RawToken("int",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #44

RawToken Accept_45()
    { // begin accept action #45
{ return new RawToken("nil",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #45

RawToken Accept_46()
    { // begin accept action #46
{ return new RawToken("not",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #46

RawToken Accept_47()
    { // begin accept action #47
{ return new RawToken("for",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #47

RawToken Accept_48()
    { // begin accept action #48
{ line_char=yychar+yytext().Length; return null; }
    } // end accept action #48

RawToken Accept_49()
    { // begin accept action #49
{ return new RawToken("let",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #49

RawToken Accept_50()
    { // begin accept action #50
{ return new RawToken("cdr",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #50

RawToken Accept_51()
    { // begin accept action #51
{ return new RawToken("car",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #51

RawToken Accept_52()
    { // begin accept action #52
{ return new RawToken("true",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #52

RawToken Accept_53()
    { // begin accept action #53
{ return new RawToken("else",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #53

RawToken Accept_54()
    { // begin accept action #54
{ return new RawToken("cons",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #54

RawToken Accept_55()
    { // begin accept action #55
{ return new RawToken("float",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #55

RawToken Accept_56()
    { // begin accept action #56
{ return new RawToken("false",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #56

RawToken Accept_57()
    { // begin accept action #57
{ return new RawToken("class",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #57

RawToken Accept_58()
    { // begin accept action #58
{ return new RawToken("while",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #58

RawToken Accept_59()
    { // begin accept action #59
{ return new RawToken("import",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #59

RawToken Accept_60()
    { // begin accept action #60
{ return new RawToken("lambda",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #60

RawToken Accept_61()
    { // begin accept action #61
{ return new RawToken("define",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #61

RawToken Accept_62()
    { // begin accept action #62
{ return new RawToken("String",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #62

RawToken Accept_63()
    { // begin accept action #63
{ return new RawToken("boolean",yytext(),yyline,yychar-line_char,yychar); }
    } // end accept action #63

RawToken Accept_64()
    { // begin accept action #64
{ return null; }
    } // end accept action #64

RawToken Accept_65()
    { // begin accept action #65
{ comment_count = comment_count + 1; return null; }
    } // end accept action #65

RawToken Accept_66()
    { // begin accept action #66
{ 
	comment_count = comment_count - 1;
	if (comment_count == 0) {
            yybegin(YYINITIAL);
        }
        return null;
}
    } // end accept action #66

RawToken Accept_68()
    { // begin accept action #68
{ line_char=yychar+yytext().Length; return null; }
    } // end accept action #68

RawToken Accept_69()
    { // begin accept action #69
{ line_char = yychar+yytext().Length; return null; }
    } // end accept action #69

RawToken Accept_70()
    { // begin accept action #70
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #70

RawToken Accept_71()
    { // begin accept action #71
{
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
    } // end accept action #71

RawToken Accept_72()
    { // begin accept action #72
{ 
  return new RawToken("Num",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #72

RawToken Accept_73()
    { // begin accept action #73
{ 
  return new RawToken("Float",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #73

RawToken Accept_74()
    { // begin accept action #74
{
        return new RawToken("StrLit",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #74

RawToken Accept_75()
    { // begin accept action #75
{ return null; }
    } // end accept action #75

RawToken Accept_77()
    { // begin accept action #77
{ line_char=yychar+yytext().Length; return null; }
    } // end accept action #77

RawToken Accept_78()
    { // begin accept action #78
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #78

RawToken Accept_79()
    { // begin accept action #79
{
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
    } // end accept action #79

RawToken Accept_80()
    { // begin accept action #80
{ return null; }
    } // end accept action #80

RawToken Accept_82()
    { // begin accept action #82
{ line_char=yychar+yytext().Length; return null; }
    } // end accept action #82

RawToken Accept_83()
    { // begin accept action #83
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #83

RawToken Accept_84()
    { // begin accept action #84
{
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
    } // end accept action #84

RawToken Accept_85()
    { // begin accept action #85
{ return null; }
    } // end accept action #85

RawToken Accept_87()
    { // begin accept action #87
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #87

RawToken Accept_88()
    { // begin accept action #88
{
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
    } // end accept action #88

RawToken Accept_90()
    { // begin accept action #90
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #90

RawToken Accept_92()
    { // begin accept action #92
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #92

RawToken Accept_94()
    { // begin accept action #94
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #94

RawToken Accept_95()
    { // begin accept action #95
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #95

RawToken Accept_96()
    { // begin accept action #96
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #96

RawToken Accept_97()
    { // begin accept action #97
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #97

RawToken Accept_98()
    { // begin accept action #98
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #98

RawToken Accept_99()
    { // begin accept action #99
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #99

RawToken Accept_100()
    { // begin accept action #100
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #100

RawToken Accept_101()
    { // begin accept action #101
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #101

RawToken Accept_102()
    { // begin accept action #102
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #102

RawToken Accept_103()
    { // begin accept action #103
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #103

RawToken Accept_104()
    { // begin accept action #104
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #104

RawToken Accept_105()
    { // begin accept action #105
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #105

RawToken Accept_106()
    { // begin accept action #106
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #106

RawToken Accept_107()
    { // begin accept action #107
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #107

RawToken Accept_108()
    { // begin accept action #108
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #108

RawToken Accept_109()
    { // begin accept action #109
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #109

RawToken Accept_110()
    { // begin accept action #110
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #110

RawToken Accept_111()
    { // begin accept action #111
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #111

RawToken Accept_112()
    { // begin accept action #112
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #112

RawToken Accept_113()
    { // begin accept action #113
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #113

RawToken Accept_114()
    { // begin accept action #114
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #114

RawToken Accept_115()
    { // begin accept action #115
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #115

RawToken Accept_116()
    { // begin accept action #116
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #116

RawToken Accept_117()
    { // begin accept action #117
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #117

RawToken Accept_118()
    { // begin accept action #118
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #118

RawToken Accept_119()
    { // begin accept action #119
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #119

RawToken Accept_120()
    { // begin accept action #120
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #120

RawToken Accept_121()
    { // begin accept action #121
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #121

RawToken Accept_122()
    { // begin accept action #122
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #122

RawToken Accept_123()
    { // begin accept action #123
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #123

RawToken Accept_124()
    { // begin accept action #124
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #124

RawToken Accept_125()
    { // begin accept action #125
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #125

RawToken Accept_126()
    { // begin accept action #126
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #126

RawToken Accept_127()
    { // begin accept action #127
{
	String str =  yytext().Substring(1,yytext().Length);
	Utility.error(Utility.E_UNCLOSEDSTR);
        return new RawToken("Unclosed String",str,yyline,yychar-line_char,yychar);
}
    } // end accept action #127

RawToken Accept_128()
    { // begin accept action #128
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #128

RawToken Accept_129()
    { // begin accept action #129
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #129

RawToken Accept_130()
    { // begin accept action #130
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #130

RawToken Accept_131()
    { // begin accept action #131
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #131

RawToken Accept_132()
    { // begin accept action #132
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #132

RawToken Accept_133()
    { // begin accept action #133
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #133

RawToken Accept_134()
    { // begin accept action #134
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #134

RawToken Accept_135()
    { // begin accept action #135
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #135

RawToken Accept_136()
    { // begin accept action #136
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #136

RawToken Accept_137()
    { // begin accept action #137
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #137

RawToken Accept_138()
    { // begin accept action #138
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #138

RawToken Accept_139()
    { // begin accept action #139
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #139

RawToken Accept_140()
    { // begin accept action #140
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #140

RawToken Accept_141()
    { // begin accept action #141
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #141

RawToken Accept_142()
    { // begin accept action #142
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #142

RawToken Accept_143()
    { // begin accept action #143
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #143

RawToken Accept_144()
    { // begin accept action #144
{
        return new RawToken("Alphanum",yytext(),yyline,yychar-line_char,yychar);
}
    } // end accept action #144

private const int YYINITIAL = 0;
private const int COMMENT = 1;
private static int[] yy_state_dtrans = new int[] 
  {   0,
  64
  };
private void yybegin (int state)
  {
  yy_lexical_state = state;
  }

private char yy_advance ()
  {
  int next_read;
  int i;
  int j;

  if (yy_buffer_index < yy_buffer_read)
    {
    return yy_buffer[yy_buffer_index++];
    }

  if (0 != yy_buffer_start)
    {
    i = yy_buffer_start;
    j = 0;
    while (i < yy_buffer_read)
      {
      yy_buffer[j] = yy_buffer[i];
      i++;
      j++;
      }
    yy_buffer_end = yy_buffer_end - yy_buffer_start;
    yy_buffer_start = 0;
    yy_buffer_read = j;
    yy_buffer_index = j;
    next_read = yy_reader.Read(yy_buffer,yy_buffer_read,
                  yy_buffer.Length - yy_buffer_read);
    if (next_read <= 0)
      {
      return (char) YY_EOF;
      }
    yy_buffer_read = yy_buffer_read + next_read;
    }
  while (yy_buffer_index >= yy_buffer_read)
    {
    if (yy_buffer_index >= yy_buffer.Length)
      {
      yy_buffer = yy_double(yy_buffer);
      }
    next_read = yy_reader.Read(yy_buffer,yy_buffer_read,
                  yy_buffer.Length - yy_buffer_read);
    if (next_read <= 0)
      {
      return (char) YY_EOF;
      }
    yy_buffer_read = yy_buffer_read + next_read;
    }
  return yy_buffer[yy_buffer_index++];
  }
private void yy_move_end ()
  {
  if (yy_buffer_end > yy_buffer_start && 
      '\n' == yy_buffer[yy_buffer_end-1])
    yy_buffer_end--;
  if (yy_buffer_end > yy_buffer_start &&
      '\r' == yy_buffer[yy_buffer_end-1])
    yy_buffer_end--;
  }
private bool yy_last_was_cr=false;
private void yy_mark_start ()
  {
  int i;
  for (i = yy_buffer_start; i < yy_buffer_index; i++)
    {
    if (yy_buffer[i] == '\n' && !yy_last_was_cr)
      {
      yyline++;
      }
    if (yy_buffer[i] == '\r')
      {
      yyline++;
      yy_last_was_cr=true;
      }
    else
      {
      yy_last_was_cr=false;
      }
    }
  yychar = yychar + yy_buffer_index - yy_buffer_start;
  yy_buffer_start = yy_buffer_index;
  }
private void yy_mark_end ()
  {
  yy_buffer_end = yy_buffer_index;
  }
private void yy_to_mark ()
  {
  yy_buffer_index = yy_buffer_end;
  yy_at_bol = (yy_buffer_end > yy_buffer_start) &&
    (yy_buffer[yy_buffer_end-1] == '\r' ||
    yy_buffer[yy_buffer_end-1] == '\n');
  }
internal string yytext()
  {
  return (new string(yy_buffer,
                yy_buffer_start,
                yy_buffer_end - yy_buffer_start)
         );
  }
private int yylength ()
  {
  return yy_buffer_end - yy_buffer_start;
  }
private char[] yy_double (char[] buf)
  {
  int i;
  char[] newbuf;
  newbuf = new char[2*buf.Length];
  for (i = 0; i < buf.Length; i++)
    {
    newbuf[i] = buf[i];
    }
  return newbuf;
  }
private const int YY_E_INTERNAL = 0;
private const int YY_E_MATCH = 1;
private static string[] yy_error_string = new string[]
  {
  "Error: Internal error.\n",
  "Error: Unmatched input.\n"
  };
private void yy_error (int code,bool fatal)
  {
  System.Console.Write(yy_error_string[code]);
  if (fatal)
    {
    throw new System.ApplicationException("Fatal Error.\n");
    }
  }
private static int[] yy_acpt = new int[]
  {
  /* 0 */   YY_NOT_ACCEPT,
  /* 1 */   YY_NO_ANCHOR,
  /* 2 */   YY_NO_ANCHOR,
  /* 3 */   YY_NO_ANCHOR,
  /* 4 */   YY_NO_ANCHOR,
  /* 5 */   YY_NO_ANCHOR,
  /* 6 */   YY_NO_ANCHOR,
  /* 7 */   YY_NO_ANCHOR,
  /* 8 */   YY_NO_ANCHOR,
  /* 9 */   YY_NO_ANCHOR,
  /* 10 */   YY_NO_ANCHOR,
  /* 11 */   YY_NO_ANCHOR,
  /* 12 */   YY_NO_ANCHOR,
  /* 13 */   YY_NO_ANCHOR,
  /* 14 */   YY_NO_ANCHOR,
  /* 15 */   YY_NO_ANCHOR,
  /* 16 */   YY_NO_ANCHOR,
  /* 17 */   YY_NO_ANCHOR,
  /* 18 */   YY_NO_ANCHOR,
  /* 19 */   YY_NO_ANCHOR,
  /* 20 */   YY_NO_ANCHOR,
  /* 21 */   YY_NO_ANCHOR,
  /* 22 */   YY_NO_ANCHOR,
  /* 23 */   YY_NO_ANCHOR,
  /* 24 */   YY_NO_ANCHOR,
  /* 25 */   YY_NO_ANCHOR,
  /* 26 */   YY_NO_ANCHOR,
  /* 27 */   YY_NO_ANCHOR,
  /* 28 */   YY_NO_ANCHOR,
  /* 29 */   YY_NO_ANCHOR,
  /* 30 */   YY_NO_ANCHOR,
  /* 31 */   YY_NO_ANCHOR,
  /* 32 */   YY_NO_ANCHOR,
  /* 33 */   YY_NO_ANCHOR,
  /* 34 */   YY_NO_ANCHOR,
  /* 35 */   YY_NO_ANCHOR,
  /* 36 */   YY_NO_ANCHOR,
  /* 37 */   YY_NO_ANCHOR,
  /* 38 */   YY_NO_ANCHOR,
  /* 39 */   YY_NO_ANCHOR,
  /* 40 */   YY_NO_ANCHOR,
  /* 41 */   YY_NO_ANCHOR,
  /* 42 */   YY_NO_ANCHOR,
  /* 43 */   YY_NO_ANCHOR,
  /* 44 */   YY_NO_ANCHOR,
  /* 45 */   YY_NO_ANCHOR,
  /* 46 */   YY_NO_ANCHOR,
  /* 47 */   YY_NO_ANCHOR,
  /* 48 */   YY_NO_ANCHOR,
  /* 49 */   YY_NO_ANCHOR,
  /* 50 */   YY_NO_ANCHOR,
  /* 51 */   YY_NO_ANCHOR,
  /* 52 */   YY_NO_ANCHOR,
  /* 53 */   YY_NO_ANCHOR,
  /* 54 */   YY_NO_ANCHOR,
  /* 55 */   YY_NO_ANCHOR,
  /* 56 */   YY_NO_ANCHOR,
  /* 57 */   YY_NO_ANCHOR,
  /* 58 */   YY_NO_ANCHOR,
  /* 59 */   YY_NO_ANCHOR,
  /* 60 */   YY_NO_ANCHOR,
  /* 61 */   YY_NO_ANCHOR,
  /* 62 */   YY_NO_ANCHOR,
  /* 63 */   YY_NO_ANCHOR,
  /* 64 */   YY_NO_ANCHOR,
  /* 65 */   YY_NO_ANCHOR,
  /* 66 */   YY_NO_ANCHOR,
  /* 67 */   YY_NOT_ACCEPT,
  /* 68 */   YY_NO_ANCHOR,
  /* 69 */   YY_NO_ANCHOR,
  /* 70 */   YY_NO_ANCHOR,
  /* 71 */   YY_NO_ANCHOR,
  /* 72 */   YY_NO_ANCHOR,
  /* 73 */   YY_NO_ANCHOR,
  /* 74 */   YY_NO_ANCHOR,
  /* 75 */   YY_NO_ANCHOR,
  /* 76 */   YY_NOT_ACCEPT,
  /* 77 */   YY_NO_ANCHOR,
  /* 78 */   YY_NO_ANCHOR,
  /* 79 */   YY_NO_ANCHOR,
  /* 80 */   YY_NO_ANCHOR,
  /* 81 */   YY_NOT_ACCEPT,
  /* 82 */   YY_NO_ANCHOR,
  /* 83 */   YY_NO_ANCHOR,
  /* 84 */   YY_NO_ANCHOR,
  /* 85 */   YY_NO_ANCHOR,
  /* 86 */   YY_NOT_ACCEPT,
  /* 87 */   YY_NO_ANCHOR,
  /* 88 */   YY_NO_ANCHOR,
  /* 89 */   YY_NOT_ACCEPT,
  /* 90 */   YY_NO_ANCHOR,
  /* 91 */   YY_NOT_ACCEPT,
  /* 92 */   YY_NO_ANCHOR,
  /* 93 */   YY_NOT_ACCEPT,
  /* 94 */   YY_NO_ANCHOR,
  /* 95 */   YY_NO_ANCHOR,
  /* 96 */   YY_NO_ANCHOR,
  /* 97 */   YY_NO_ANCHOR,
  /* 98 */   YY_NO_ANCHOR,
  /* 99 */   YY_NO_ANCHOR,
  /* 100 */   YY_NO_ANCHOR,
  /* 101 */   YY_NO_ANCHOR,
  /* 102 */   YY_NO_ANCHOR,
  /* 103 */   YY_NO_ANCHOR,
  /* 104 */   YY_NO_ANCHOR,
  /* 105 */   YY_NO_ANCHOR,
  /* 106 */   YY_NO_ANCHOR,
  /* 107 */   YY_NO_ANCHOR,
  /* 108 */   YY_NO_ANCHOR,
  /* 109 */   YY_NO_ANCHOR,
  /* 110 */   YY_NO_ANCHOR,
  /* 111 */   YY_NO_ANCHOR,
  /* 112 */   YY_NO_ANCHOR,
  /* 113 */   YY_NO_ANCHOR,
  /* 114 */   YY_NO_ANCHOR,
  /* 115 */   YY_NO_ANCHOR,
  /* 116 */   YY_NO_ANCHOR,
  /* 117 */   YY_NO_ANCHOR,
  /* 118 */   YY_NO_ANCHOR,
  /* 119 */   YY_NO_ANCHOR,
  /* 120 */   YY_NO_ANCHOR,
  /* 121 */   YY_NO_ANCHOR,
  /* 122 */   YY_NO_ANCHOR,
  /* 123 */   YY_NO_ANCHOR,
  /* 124 */   YY_NO_ANCHOR,
  /* 125 */   YY_NO_ANCHOR,
  /* 126 */   YY_NO_ANCHOR,
  /* 127 */   YY_NO_ANCHOR,
  /* 128 */   YY_NO_ANCHOR,
  /* 129 */   YY_NO_ANCHOR,
  /* 130 */   YY_NO_ANCHOR,
  /* 131 */   YY_NO_ANCHOR,
  /* 132 */   YY_NO_ANCHOR,
  /* 133 */   YY_NO_ANCHOR,
  /* 134 */   YY_NO_ANCHOR,
  /* 135 */   YY_NO_ANCHOR,
  /* 136 */   YY_NO_ANCHOR,
  /* 137 */   YY_NO_ANCHOR,
  /* 138 */   YY_NO_ANCHOR,
  /* 139 */   YY_NO_ANCHOR,
  /* 140 */   YY_NO_ANCHOR,
  /* 141 */   YY_NO_ANCHOR,
  /* 142 */   YY_NO_ANCHOR,
  /* 143 */   YY_NO_ANCHOR,
  /* 144 */   YY_NO_ANCHOR
  };
private static int[] yy_cmap = new int[]
  {
  49, 49, 49, 49, 49, 49, 49, 49,
  3, 3, 2, 49, 49, 1, 49, 49,
  49, 49, 49, 49, 49, 49, 49, 49,
  49, 49, 49, 49, 49, 49, 49, 49,
  3, 34, 51, 49, 49, 41, 32, 48,
  46, 27, 40, 9, 28, 6, 31, 11,
  54, 53, 53, 53, 53, 53, 53, 53,
  53, 53, 21, 47, 36, 17, 7, 50,
  49, 56, 56, 56, 56, 57, 56, 58,
  58, 58, 58, 58, 58, 58, 58, 58,
  58, 58, 58, 37, 58, 58, 58, 58,
  58, 58, 58, 45, 52, 12, 33, 58,
  49, 26, 42, 25, 24, 16, 8, 38,
  30, 4, 58, 58, 15, 18, 5, 13,
  19, 22, 20, 44, 14, 43, 58, 29,
  55, 58, 58, 10, 39, 23, 35, 49,
  0, 0 
  };
private static int[] yy_rmap = new int[]
  {
  0, 1, 2, 3, 4, 5, 6, 7,
  8, 1, 9, 1, 1, 1, 1, 1,
  1, 10, 11, 1, 1, 12, 1, 1,
  1, 1, 1, 1, 13, 14, 15, 16,
  1, 1, 1, 1, 16, 17, 1, 1,
  1, 1, 1, 18, 16, 16, 16, 16,
  1, 16, 16, 16, 16, 16, 16, 16,
  16, 16, 16, 16, 16, 16, 16, 16,
  19, 1, 1, 2, 20, 21, 22, 23,
  24, 25, 13, 26, 27, 1, 28, 1,
  29, 10, 26, 30, 31, 32, 33, 34,
  35, 25, 36, 37, 38, 39, 40, 41,
  42, 43, 44, 45, 46, 47, 48, 49,
  50, 51, 52, 53, 54, 55, 56, 57,
  58, 59, 60, 61, 62, 63, 64, 65,
  66, 67, 68, 69, 70, 71, 72, 73,
  74, 75, 76, 77, 78, 79, 80, 16,
  81, 82, 83, 84, 85, 86, 87, 88,
  89 
  };
private static int[,] yy_nxt = new int[,]
  {
  { 1, 2, 3, 4, 5, 70, 6, 7,
   128, 8, 9, 10, 11, 135, 137, 138,
   78, 12, 135, 135, 135, 13, 135, 14,
   139, 140, 135, 15, 16, 141, 135, 17,
   18, 19, 71, 20, 21, 142, 135, 68,
   22, 23, 143, 135, 135, 24, 25, 26,
   27, 79, 77, 28, 79, 29, 72, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, 69, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, 67, 3, 4, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, 4, 4, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, 135, 30, -1, -1,
   31, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 144, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, -1, -1, 32, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, 33, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, 34, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, 76, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   35, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 37, 37, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   38, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, 40, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 42, 127, 28, 28, 28,
   28, 28, 28 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, 81,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 29, 29, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 44, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   86, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 37, 37, -1,
   -1, 86, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   43, -1, -1, -1, -1, -1, -1, -1,
   43, -1, -1, -1, -1, -1, -1, -1,
   43, 43, 43, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, 43, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 43, 43, -1,
   43, 43, -1 },
  { 1, 77, 77, 75, 75, 75, 75, 75,
   75, 75, 75, 84, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 82, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 82,
   88, 75, 75, 75, 75, 75, 82, 75,
   75, 75, 82, 75, 75, 75, 75, 75,
   75, 75, 75 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, 41,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, 67, 69, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, 83, 135, -1, -1,
   135, -1, -1, -1, -1, 87, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, 39, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, 81,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 29, 29, 43,
   -1, -1, -1 },
  { -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 73, 73, -1,
   -1, -1, -1 },
  { -1, -1, -1, 75, 75, 75, 75, 75,
   75, 75, 75, 91, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   93, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75 },
  { -1, -1, 48, 76, 76, 76, 76, 76,
   76, 76, 76, 76, 76, 76, 76, 76,
   76, 76, 76, 76, 76, 76, 76, 76,
   76, 76, 76, 76, 76, 76, 76, 76,
   76, 76, 76, 76, 76, 76, 76, 76,
   76, 76, 76, 76, 76, 76, 76, 76,
   76, 76, 76, 76, 76, 76, 76, 76,
   76, 76, 76 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 98,
   135, -1, 135, 135, 135, -1, 36, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, 75, 75, 75, 75, 75,
   75, 75, 75, 80, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   93, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 45,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, 75, 75, 75, 75, 75,
   75, 75, 75, 80, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   65, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75 },
  { -1, -1, -1, 75, 75, 75, 75, 75,
   75, 75, 75, 91, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   85, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75 },
  { -1, -1, -1, -1, -1, -1, 89, -1,
   -1, 89, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, -1, -1, -1,
   -1, -1, -1, -1, -1, 73, 73, -1,
   -1, -1, -1 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 46, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, 75, 75, 75, 75, 75,
   75, 75, 75, 66, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   85, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 47, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, 75, 75, 75, 75, 75,
   75, 75, 75, 80, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   -1, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 131, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, 75, 75, 75, 75, 75,
   75, 75, 75, -1, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   85, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75, 75, 75, 75, 75, 75,
   75, 75, 75 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 130,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 106, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 49, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 107, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 108, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   132, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 109, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 110, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 50, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 51, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 111, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 112, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   52, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 116, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   53, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 54, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 118, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 119,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 134, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 121, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 55, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   56, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   122, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 123, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 57, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   58, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   125, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 59, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 60, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   61, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 62, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 126, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 63, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 28, 28, 28, 28, 28,
   28, 28, 28, 74, 127, 28, 28, 28,
   28, 28, 28 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 90, 135, 92,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 94, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 133, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 115, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 114, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 117, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 120,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 124, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 113, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 95, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   96, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 97, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   99, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 100, 135, 101,
   135, -1, 135, 135, 135, -1, 135, -1,
   102, 135, 103, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 104, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 105, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 129, 135, 135,
   135, -1, 135, 135, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 },
  { -1, -1, -1, -1, 135, 135, -1, -1,
   135, -1, -1, -1, -1, 135, 135, 135,
   135, -1, 135, 136, 135, -1, 135, -1,
   135, 135, 135, -1, -1, 135, 135, -1,
   -1, -1, -1, -1, -1, 135, 135, -1,
   -1, -1, 135, 135, 135, -1, -1, -1,
   -1, -1, -1, -1, -1, 135, 135, 135,
   135, 135, 135 }
  };
public RawToken yylex()
  {
  char yy_lookahead;
  int yy_anchor = YY_NO_ANCHOR;
  int yy_state = yy_state_dtrans[yy_lexical_state];
  int yy_next_state = YY_NO_STATE;
  int yy_last_accept_state = YY_NO_STATE;
  bool yy_initial = true;
  int yy_this_accept;

  yy_mark_start();
  yy_this_accept = yy_acpt[yy_state];
  if (YY_NOT_ACCEPT != yy_this_accept)
    {
    yy_last_accept_state = yy_state;
    yy_mark_end();
    }
  while (true)
    {
    if (yy_initial && yy_at_bol)
      yy_lookahead = (char) YY_BOL;
    else
      {
      yy_lookahead = yy_advance();
      }
    yy_next_state = yy_nxt[yy_rmap[yy_state],yy_cmap[yy_lookahead]];
    if (YY_EOF == yy_lookahead && yy_initial)
      {

  return new RawToken("EOF","EOF",yyline,yychar);
      }
    if (YY_F != yy_next_state)
      {
      yy_state = yy_next_state;
      yy_initial = false;
      yy_this_accept = yy_acpt[yy_state];
      if (YY_NOT_ACCEPT != yy_this_accept)
        {
        yy_last_accept_state = yy_state;
        yy_mark_end();
        }
      }
    else
      {
      if (YY_NO_STATE == yy_last_accept_state)
        {
        throw new System.ApplicationException("Lexical Error: Unmatched Input.");
        }
      else
        {
        yy_anchor = yy_acpt[yy_last_accept_state];
        if (0 != (YY_END & yy_anchor))
          {
          yy_move_end();
          }
        yy_to_mark();
        if (yy_last_accept_state < 0)
          {
          if (yy_last_accept_state < 145)
            yy_error(YY_E_INTERNAL, false);
          }
        else
          {
          AcceptMethod m = accept_dispatch[yy_last_accept_state];
          if (m != null)
            {
            RawToken tmp = m();
            if (tmp != null)
              return tmp;
            }
          }
        yy_initial = true;
        yy_state = yy_state_dtrans[yy_lexical_state];
        yy_next_state = YY_NO_STATE;
        yy_last_accept_state = YY_NO_STATE;
        yy_mark_start();
        yy_this_accept = yy_acpt[yy_state];
        if (YY_NOT_ACCEPT != yy_this_accept)
          {
          yy_last_accept_state = yy_state;
          yy_mark_end();
          }
        }
      }
    }
  }
}

}
