target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)

@SVAR_15 =  constant [5 x i8] c"Test\00", align 1


define i32 @main()
{
	mainstart:
		%R_0 = icmp eq i32 3, 3
		%R_1 = zext i1 %R_0 to i32
		%R_2 = trunc i32 %R_1 to i1
		br i1 %R_2, label %WhileBody_3, label %WhileEnd_3

	WhileBody_3:
		%R_4 = add i32 1, 2
		%R_5 = sub i32 4, 2
		%R_6 = icmp eq i32 1, 2
		%R_7 = zext i1 %R_6 to i32
		%R_8 = trunc i32 %R_7 to i1
		br i1 %R_8, label %IfTrue_9, label %IfFalse_9

	IfTrue_9:
		%R_10 = sub i32 1, 2
		br label %EndIf_9

	IfFalse_9:
		%R_12 = sub i32 2, 1
		br label %EndIf_9

	EndIf_9:
		%result_14 = phi i32 [%R_10, %IfTrue_9], [%R_12, %IfFalse_9]
		%A_15 = getelementptr inbounds [5 x i8], [5 x i8]* @SVAR_15, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_15)
		%R_17 = icmp eq i32 3, 3
		%R_18 = zext i1 %R_17 to i32
		%R_19 = trunc i32 %R_18 to i1
		br i1 %R_19, label %WhileBody_3, label %WhileEnd_3

	WhileEnd_3:
		ret i32 0

}

