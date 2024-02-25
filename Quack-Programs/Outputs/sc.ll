target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)



define i32 @main()
{
	mainstart:
		%R_0 = call i32 (i32) @fib(i32 10)
		call void (i32) @lambda7c_printint(i32 %R_0)
		ret i32 0

}

define i32 @fib(i32 %farg_n_3)
{
	fibmain:
		%n_3 = alloca i32
		store i32 %farg_n_3, i32* %n_3
		%n_3_2 = load i32, i32* %n_3
		%R_3 = icmp sle i32 %n_3_2, 2
		%R_4 = zext i1 %R_3 to i32
		%R_5 = trunc i32 %R_4 to i1
		br i1 %R_5, label %IfTrue_6, label %IfFalse_6

	IfTrue_6:
		br label %EndIf_6

	IfFalse_6:
		%n_3_8 = load i32, i32* %n_3
		%R_9 = sub i32 %n_3_8, 1
		%R_10 = call i32 (i32) @fib(i32 %R_9)
		%n_3_11 = load i32, i32* %n_3
		%R_12 = sub i32 %n_3_11, 2
		%R_13 = call i32 (i32) @fib(i32 %R_12)
		%R_14 = add i32 %R_10, %R_13
		br label %EndIf_6

	EndIf_6:
		%result_16 = phi i32 [1, %IfTrue_6], [%R_14, %IfFalse_6]
		ret i32 %result_16

}

