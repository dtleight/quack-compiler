target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i32 @lambda7c_cin()
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i32 @lambda7c_cin()
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)



define i32 @main()
{
	mainstart:
		%sum_1 = alloca i32
		store i32 0, i32* %sum_1
		%i_2 = alloca i32
		store i32 1, i32* %i_2
		%i_2_4 = load i32, i32* %i_2
		%R_5 = icmp sle i32 %i_2_4, 100
		%R_6 = zext i1 %R_5 to i32
		%R_7 = trunc i32 %R_6 to i1
		br i1 %R_7, label %WhileBody_8, label %WhileEnd_8

	WhileBody_8:
		%sum_1_9 = load i32, i32* %sum_1
		%i_2_10 = load i32, i32* %i_2
		%R_11 = add i32 %sum_1_9, %i_2_10
		store i32 %R_11, i32* %sum_1
		%i_2_13 = load i32, i32* %i_2
		%R_14 = add i32 %i_2_13, 1
		store i32 %R_14, i32* %i_2
		%i_2_16 = load i32, i32* %i_2
		%R_17 = icmp sle i32 %i_2_16, 100
		%R_18 = zext i1 %R_17 to i32
		%R_19 = trunc i32 %R_18 to i1
		br i1 %R_19, label %WhileBody_8, label %WhileEnd_8

	WhileEnd_8:
		%result_21 = phi i32 [%R_14, %WhileBody_8], [%R_14, %WhileBody_8]
		%sum_1_22 = load i32, i32* %sum_1
		call void (i32) @lambda7c_printint(i32 %sum_1_22)
		ret i32 0

}

