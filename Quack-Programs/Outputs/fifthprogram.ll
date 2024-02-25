target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)

@SVAR_4 =  constant [2 x i8] c"\0a\00", align 1
@SVAR_22 =  constant [4 x i8] c"\0aA\0a\00", align 1
@SVAR_25 =  constant [4 x i8] c"\0aB\0a\00", align 1


define i32 @main()
{
	mainstart:
		%x_1 = alloca i32
		store i32 1, i32* %x_1
		%R_2 = call double (i32, i32*) @f2(i32 1, i32* %x_1)
		call void (double) @lambda7c_printfloat(double %R_2)
		%A_4 = getelementptr inbounds [2 x i8], [2 x i8]* @SVAR_4, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_4)
		%R_6 = call i32 (i32, i32*) @fib(i32 10, i32* %x_1)
		call void (i32) @lambda7c_printint(i32 %R_6)
		%x_1_8 = load i32, i32* %x_1
		%R_9 = icmp sgt i32 %x_1_8, 0
		%R_10 = zext i1 %R_9 to i32
		%R_11 = trunc i32 %R_10 to i1
		br i1 %R_11, label %IfTrue_12, label %IfFalse_12

	IfTrue_12:
		%x_1_13 = load i32, i32* %x_1
		%R_14 = sdiv i32 10, %x_1_13
		%R_15 = icmp sgt i32 %R_14, 1
		%R_16 = zext i1 %R_15 to i32
		br label %EndIf_12

	IfFalse_12:
		br label %EndIf_12

	EndIf_12:
		%result_19 = phi i32 [%R_16, %IfTrue_12], [0, %IfFalse_12]
		%R_20 = trunc i32 %result_19 to i1
		br i1 %R_20, label %IfTrue_21, label %IfFalse_21

	IfTrue_21:
		%A_22 = getelementptr inbounds [4 x i8], [4 x i8]* @SVAR_22, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_22)
		br label %EndIf_21

	IfFalse_21:
		%A_25 = getelementptr inbounds [4 x i8], [4 x i8]* @SVAR_25, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_25)
		br label %EndIf_21

	EndIf_21:
		ret i32 0

}

define i32 @f(i32 %farg_y_4, i32* %x_1)
{
	fmain:
		%y_4 = alloca i32
		store i32 %farg_y_4, i32* %y_4
		%y_4_2 = load i32, i32* %y_4
		%x_1_3 = load i32, i32* %x_1
		%R_4 = add i32 %y_4_2, %x_1_3
		ret i32 %R_4

}

define double @f2(i32 %farg_y_9, i32* %x_1)
{
	f2main:
		%y_9 = alloca i32
		store i32 %farg_y_9, i32* %y_9
		%r_2 = call double (i32, i32*, i32*) @x_0(i32 2, i32* %x_1, i32* %y_9)
		ret double %r_2

}

define double @x_0(i32 %larg_x, i32* %x_1, i32* %y_9)
{
	x_0main:
		%x_10 = alloca i32
		store i32 %larg_x, i32* %x_10
		%x_10_2 = load i32, i32* %x_10
		call void (i32) @lambda7c_printint(i32 %x_10_2)
		ret double 0.5

}

define i32 @fib(i32 %farg_n_14, i32* %x_1)
{
	fibmain:
		%n_14 = alloca i32
		store i32 %farg_n_14, i32* %n_14
		%n_14_2 = load i32, i32* %n_14
		%R_3 = icmp sle i32 %n_14_2, 2
		%R_4 = zext i1 %R_3 to i32
		%R_5 = trunc i32 %R_4 to i1
		br i1 %R_5, label %IfTrue_6, label %IfFalse_6

	IfTrue_6:
		br label %EndIf_6

	IfFalse_6:
		%n_14_8 = load i32, i32* %n_14
		%R_9 = sub i32 %n_14_8, 1
		%R_10 = call i32 (i32, i32*) @fib(i32 %R_9, i32* %x_1)
		%n_14_11 = load i32, i32* %n_14
		%R_12 = sub i32 %n_14_11, 2
		%R_13 = call i32 (i32, i32*) @fib(i32 %R_12, i32* %x_1)
		%R_14 = add i32 %R_10, %R_13
		br label %EndIf_6

	EndIf_6:
		%result_16 = phi i32 [1, %IfTrue_6], [%R_14, %IfFalse_6]
		ret i32 %result_16

}

