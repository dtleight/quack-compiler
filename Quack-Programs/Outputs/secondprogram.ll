target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)

@SVAR_48 =  constant [11 x i8] c"The gcd = \00", align 1
@SVAR_54 =  constant [2 x i8] c"\0a\00", align 1


define i32 @main()
{
	mainstart:
		%a_1 = alloca i32
		store i32 12, i32* %a_1
		%b_2 = alloca i32
		store i32 18, i32* %b_2
		%a_1_4 = load i32, i32* %a_1
		%R_5 = icmp ne i32 %a_1_4, 0
		%R_6 = zext i1 %R_5 to i32
		%R_7 = trunc i32 %R_6 to i1
		br i1 %R_7, label %IfTrue_8, label %IfFalse_8

	IfTrue_8:
		%b_2_9 = load i32, i32* %b_2
		%R_10 = icmp ne i32 %b_2_9, 0
		%R_11 = zext i1 %R_10 to i32
		br label %EndIf_8

	IfFalse_8:
		br label %EndIf_8

	EndIf_8:
		%result_14 = phi i32 [%R_11, %IfTrue_8], [0, %IfFalse_8]
		%R_15 = trunc i32 %result_14 to i1
		br i1 %R_15, label %WhileBody_16, label %WhileEnd_16

	WhileBody_16:
		%a_1_17 = load i32, i32* %a_1
		%b_2_18 = load i32, i32* %b_2
		%R_19 = icmp sgt i32 %a_1_17, %b_2_18
		%R_20 = zext i1 %R_19 to i32
		%R_21 = trunc i32 %R_20 to i1
		br i1 %R_21, label %IfTrue_22, label %IfFalse_22

	IfTrue_22:
		%a_1_23 = load i32, i32* %a_1
		%b_2_24 = load i32, i32* %b_2
		%R_25 = srem i32 %a_1_23, %b_2_24
		store i32 %R_25, i32* %a_1
		br label %EndIf_22

	IfFalse_22:
		%b_2_28 = load i32, i32* %b_2
		%a_1_29 = load i32, i32* %a_1
		%R_30 = srem i32 %b_2_28, %a_1_29
		store i32 %R_30, i32* %b_2
		br label %EndIf_22

	EndIf_22:
		%result_33 = phi i32 [%R_25, %IfTrue_22], [%R_30, %IfFalse_22]
		%a_1_34 = load i32, i32* %a_1
		%R_35 = icmp ne i32 %a_1_34, 0
		%R_36 = zext i1 %R_35 to i32
		%R_37 = trunc i32 %R_36 to i1
		br i1 %R_37, label %IfTrue_38, label %IfFalse_38

	IfTrue_38:
		%b_2_39 = load i32, i32* %b_2
		%R_40 = icmp ne i32 %b_2_39, 0
		%R_41 = zext i1 %R_40 to i32
		br label %EndIf_38

	IfFalse_38:
		br label %EndIf_38

	EndIf_38:
		%result_44 = phi i32 [%R_41, %IfTrue_38], [0, %IfFalse_38]
		%R_45 = trunc i32 %result_44 to i1
		br i1 %R_45, label %WhileBody_16, label %WhileEnd_16

	WhileEnd_16:
		%result_47 = phi i32 [%result_33, %WhileBody_16], [%result_33, %WhileBody_16]
		%A_48 = getelementptr inbounds [11 x i8], [11 x i8]* @SVAR_48, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_48)
		%a_1_50 = load i32, i32* %a_1
		%b_2_51 = load i32, i32* %b_2
		%R_52 = add i32 %a_1_50, %b_2_51
		call void (i32) @lambda7c_printint(i32 %R_52)
		%A_54 = getelementptr inbounds [2 x i8], [2 x i8]* @SVAR_54, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_54)
		ret i32 0

}

