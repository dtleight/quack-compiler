target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)

@SVAR_18 =  constant [4 x i8] c"\0aA\0a\00", align 1
@SVAR_21 =  constant [4 x i8] c"\0aB\0a\00", align 1
@SVAR_38 =  constant [3 x i8] c"C\0a\00", align 1
@SVAR_41 =  constant [3 x i8] c"D\0a\00", align 1


define i32 @main()
{
	mainstart:
		%R_0 = call i32 (i32) @fib(i32 10)
		call void (i32) @lambda7c_printint(i32 %R_0)
		%x_7 = alloca i32
		store i32 0, i32* %x_7
		%x_7_4 = load i32, i32* %x_7
		%R_5 = icmp sgt i32 %x_7_4, 0
		%R_6 = zext i1 %R_5 to i32
		%R_7 = trunc i32 %R_6 to i1
		br i1 %R_7, label %IfTrue_8, label %IfFalse_8

	IfTrue_8:
		%x_7_9 = load i32, i32* %x_7
		%R_10 = sdiv i32 10, %x_7_9
		%R_11 = icmp sgt i32 %R_10, 1
		%R_12 = zext i1 %R_11 to i32
		br label %EndIf_8

	IfFalse_8:
		br label %EndIf_8

	EndIf_8:
		%result_15 = phi i32 [%R_12, %IfTrue_8], [0, %IfFalse_8]
		%R_16 = trunc i32 %result_15 to i1
		br i1 %R_16, label %IfTrue_17, label %IfFalse_17

	IfTrue_17:
		%A_18 = getelementptr inbounds [4 x i8], [4 x i8]* @SVAR_18, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_18)
		br label %EndIf_17

	IfFalse_17:
		%A_21 = getelementptr inbounds [4 x i8], [4 x i8]* @SVAR_21, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_21)
		br label %EndIf_17

	EndIf_17:
		%x_7_24 = load i32, i32* %x_7
		%R_25 = icmp slt i32 %x_7_24, 1
		%R_26 = zext i1 %R_25 to i32
		%R_27 = trunc i32 %R_26 to i1
		br i1 %R_27, label %IfTrue_28, label %IfFalse_28

	IfTrue_28:
		br label %EndIf_28

	IfFalse_28:
		%x_7_30 = load i32, i32* %x_7
		%R_31 = call i32 (i32) @fib(i32 100)
		%R_32 = icmp slt i32 %x_7_30, %R_31
		%R_33 = zext i1 %R_32 to i32
		br label %EndIf_28

	EndIf_28:
		%result_35 = phi i32 [1, %IfTrue_28], [%R_33, %IfFalse_28]
		%R_36 = trunc i32 %result_35 to i1
		br i1 %R_36, label %IfTrue_37, label %IfFalse_37

	IfTrue_37:
		%A_38 = getelementptr inbounds [3 x i8], [3 x i8]* @SVAR_38, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_38)
		br label %EndIf_37

	IfFalse_37:
		%A_41 = getelementptr inbounds [3 x i8], [3 x i8]* @SVAR_41, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_41)
		br label %EndIf_37

	EndIf_37:
		ret i32 0

}

define i32 @fib(i32 %farg_n_4)
{
	fibmain:
		%n_4 = alloca i32
		store i32 %farg_n_4, i32* %n_4
		%n_4_2 = load i32, i32* %n_4
		%R_3 = icmp sle i32 %n_4_2, 2
		%R_4 = zext i1 %R_3 to i32
		%R_5 = trunc i32 %R_4 to i1
		br i1 %R_5, label %IfTrue_6, label %IfFalse_6

	IfTrue_6:
		br label %EndIf_6

	IfFalse_6:
		%n_4_8 = load i32, i32* %n_4
		%R_9 = sub i32 %n_4_8, 1
		%R_10 = call i32 (i32) @fib(i32 %R_9)
		%n_4_11 = load i32, i32* %n_4
		%R_12 = sub i32 %n_4_11, 2
		%R_13 = call i32 (i32) @fib(i32 %R_12)
		%R_14 = add i32 %R_10, %R_13
		%x_5 = alloca i32
		store i32 %R_14, i32* %x_5
		%x_5_17 = load i32, i32* %x_5
		br label %EndIf_6

	EndIf_6:
		%result_19 = phi i32 [1, %IfTrue_6], [%x_5_17, %IfFalse_6]
		ret i32 %result_19

}

