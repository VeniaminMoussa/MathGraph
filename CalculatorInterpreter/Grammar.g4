grammar Grammar;

/*
 * Parser Rules
 */

compileUnit
	: (mathStatement|commandStatement)+
	;

mathStatement
   : function QM
   | variable QM
   ;

commandStatement
	: CLEAR QM														#command_Clear
	| FINDROOTOF LP (IDENTIFIER|function) RP QM						#command_FindRoot
	| MAXEXTREMEOF LP (IDENTIFIER|function) RP QM					#command_MaxExtreme
	| MINEXTREMEOF LP (IDENTIFIER|function) RP QM					#command_MinExtreme
	| TANLINEOFAT LP (IDENTIFIER|function) COMMA value RP QM		#command_TanLine
	;

function
	: (IDENTIFIER ASSIGN)? expression 
	;

expression
	: NUMBER													#expr_Number
	| IDENTIFIER												#expr_Identifier
	| IDENTIFIER LP args RP										#expr_Fcall
	| VERTICAL expression VERTICAL								#expr_Abs
	| op = (PLUS|MINUS) expression								#expr_PlusOrMinus
	| expression FACTORIAL										#expr_Factorial
	| expression POW<assoc=right> expression					#expr_Power
	| expression op = (MULT|DIV) expression						#expr_MultiplicationOrDivision
	| expression op = (PLUS|MINUS) expression					#expr_AdditionOrSubtraction
	| LP expression RP											#expr_Parenthesis
	;

args
	: expression (COMMA expression)?
	;

variable
   : IDENTIFIER object
   ;

object
   : LB pairList RB
   | LB RB
   ;

pairList
	: pair (COMMA pair)*
	;

pair
   : IDENTIFIER ':' value
   ;

value
   : NUMBER															#value_Number
   | op = (PLUS|MINUS) value										#value_PlusOrMinus
   | left_op = (LP|LSB) value (COMMA value)* right_op = (RSB|RP) 	#value_Array
   ;

/*
 * Lexer Rules
 */

 // Keywords
 CLEAR:'clear';
 FINDROOTOF:'FindRootsOf';
 MAXEXTREMEOF:'FindMaxExtremeOf';
 MINEXTREMEOF:'FindMinExtremeOf';
 TANLINEOFAT:'TanLineOfAt';

// Operators
PLUS:'+'; 
MINUS:'-';
DIV:'/'; 
MOD:'%'; 
MULT:'*';
POW:'^';
FACTORIAL:'!';
VERTICAL:'|';
LP:'(';
RP:')';
LSB:'[';
RSB:']';
LB:'{';
RB:'}';
ASSIGN:'=';
QM:';';
COMMA:',';

// Identifiers - Numbers
fragment INT
   : '0' | [1-9] [0-9]*
   ;
   
fragment EXP
   : [eE] [+-]? INT
   ;
   
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
NUMBER: INT ('.' [0-9]+)? EXP?;

// Whitespaces-Comments

 WS: (COMMENTS|SPACE|CR|ENTER|TAB)-> skip;

COMMENTS : LINECOMMENT
		 | BLOCKCOMMENT
		 ;

LINECOMMENT : '//'.*? ENTER
			| '>>'.*? ENTER
			;

BLOCKCOMMENT : '/*'.*? '*/' ;

fragment ENTER
	:'\n'
	;

fragment TAB
	:'\t'
	;

fragment CR
	:'\r'
	;

fragment SPACE
	:' '
	;