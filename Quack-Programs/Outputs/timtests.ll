target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)

@SVAR0_6 =  constant [8 x i8] c"Testing\00", align 1
@SVAR0_9 =  constant [7 x i8] c"Second\00", align 1


define i32 @main()
{
	mainstart:
		%x = alloca i32
		store i32 3, i32* %x
		%y = alloca i32
		store i32 2, i32* %y
		%z = alloca i32
		store i32 1, i32* %z
		%A0_6 = getelementptr inbounds [8 x i8], [8 x i8]* @SVAR0_6, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A0_6)
		call i32 (i32) @func()
		%A0_9 = getelementptr inbounds [7 x i8], [7 x i8]* @SVAR0_9, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A0_9)
		ret i32 0

}

define i32 @func(i32:farg_y_0, i32*:x, i32*:y, i32*:z)
{
	funcmain:
		call void (i32) @lambda7c_printint(i32 3)
		ret i32 %printreg

}

