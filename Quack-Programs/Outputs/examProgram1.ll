target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i32 @lambda7c_cin()
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)

@SVAR_0 =  constant [64 x i8] c"Think of a number between 1 and 100 and I will try to guess it\0a\00", align 1
@SVAR_2 =  constant [5 x i8] c" ...\00", align 1
@SVAR_4 =  constant [2 x i8] c"\0a\00", align 1
@SVAR_35 =  constant [16 x i8] c"Is your number \00", align 1
@SVAR_39 =  constant [51 x i8] c" (enter 0 if yes), or is it higher(1) or lower(2):\00", align 1
@SVAR_41 =  constant [5 x i8] c"test\00", align 1


define i32 @main()
{
	mainstart:
		%A_0 = getelementptr inbounds [64 x i8], [64 x i8]* @SVAR_0, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_0)
		%A_2 = getelementptr inbounds [5 x i8], [5 x i8]* @SVAR_2, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_2)
		%A_4 = getelementptr inbounds [2 x i8], [2 x i8]* @SVAR_4, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_4)
		%min_1 = alloca i32
		store i32 1, i32* %min_1
		%max_2 = alloca i32
		store i32 100, i32* %max_2
		%guess_3 = alloca i32
		store i32 0, i32* %guess_3
		%response_4 = alloca i32
		store i32 1, i32* %response_4
		%min_1_14 = load i32, i32* %min_1
		%max_2_15 = load i32, i32* %max_2
		%R_16 = icmp sle i32 %min_1_14, %max_2_15
		%R_17 = zext i1 %R_16 to i32
		%R_18 = trunc i32 %R_17 to i1
		br i1 %R_18, label %IfTrue_19, label %IfFalse_19

	IfTrue_19:
		%response_4_20 = load i32, i32* %response_4
		%R_21 = icmp ne i32 %response_4_20, 0
		%R_22 = zext i1 %R_21 to i32
		br label %EndIf_19

	IfFalse_19:
		br label %EndIf_19

	EndIf_19:
		%result_25 = phi i32 [%R_22, %IfTrue_19], [0, %IfFalse_19]
		%R_26 = trunc i32 %result_25 to i1
		br i1 %R_26, label %WhileBody_27, label %WhileEnd_27

	WhileBody_27:
		%min_1_28 = load i32, i32* %min_1
		%max_2_29 = load i32, i32* %max_2
		%R_30 = add i32 %min_1_28, %max_2_29
		store i32 %R_30, i32* %guess_3
		%guess_3_32 = load i32, i32* %guess_3
		%R_33 = sdiv i32 %guess_3_32, 2
		store i32 %R_33, i32* %guess_3
		%A_35 = getelementptr inbounds [16 x i8], [16 x i8]* @SVAR_35, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_35)
		%guess_3_37 = load i32, i32* %guess_3
		call void (i32) @lambda7c_printint(i32 %guess_3_37)
		%A_39 = getelementptr inbounds [51 x i8], [51 x i8]* @SVAR_39, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_39)
		%A_41 = getelementptr inbounds [5 x i8], [5 x i8]* @SVAR_41, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_41)
		%R_43 = call i32 () @lambda7c_cin()
		store i32 %R_43, i32* %response_4
		%response_4_45 = load i32, i32* %response_4
		%R_46 = icmp eq i32 %response_4_45, 1
		%R_47 = zext i1 %R_46 to i32
		%R_48 = trunc i32 %R_47 to i1
		br i1 %R_48, label %IfTrue_49, label %IfFalse_49

	IfTrue_49:
		%guess_3_50 = load i32, i32* %guess_3
		%R_51 = add i32 %guess_3_50, 1
		store i32 %R_51, i32* %min_1
		br label %EndIf_49

	IfFalse_49:
		%guess_3_54 = load i32, i32* %guess_3
		store i32 %guess_3_54, i32* %max_2
		br label %EndIf_49

	EndIf_49:
		%result_57 = phi i32 [%R_51, %IfTrue_49], [%guess_3_54, %IfFalse_49]
		%min_1_58 = load i32, i32* %min_1
		%max_2_59 = load i32, i32* %max_2
		%R_60 = icmp sle i32 %min_1_58, %max_2_59
		%R_61 = zext i1 %R_60 to i32
		%R_62 = trunc i32 %R_61 to i1
		br i1 %R_62, label %IfTrue_63, label %IfFalse_63

	IfTrue_63:
		%response_4_64 = load i32, i32* %response_4
		%R_65 = icmp ne i32 %response_4_64, 0
		%R_66 = zext i1 %R_65 to i32
		br label %EndIf_63

	IfFalse_63:
		br label %EndIf_63

	EndIf_63:
		%result_69 = phi i32 [%R_66, %IfTrue_63], [0, %IfFalse_63]
		%R_70 = trunc i32 %result_69 to i1
		br i1 %R_70, label %WhileBody_27, label %WhileEnd_27

	WhileEnd_27:
		%result_72 = phi i32 [%result_57, %WhileBody_27], [%result_57, %WhileBody_27]
		ret i32 0

}

