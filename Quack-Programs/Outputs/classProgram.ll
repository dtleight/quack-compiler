target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)

@SVAR3_26 =  constant [4 x i8] c"   \00", align 1
@SVAR4_29 =  constant [3 x i8] c"  \00", align 1
@SVAR6_40 =  constant [2 x i8] c"\0a\00", align 1


define i32 @main()
{
	mainstart:
		%x = alloca i32
		store i32 1, i32* %x
		%y = alloca i32
		store i32 0, i32* %y
		%R0_4 = load i32, i32* %x
		%R0_5 = icmp slt i32 %R0_4, 10
		%R0_6 = zext i1 %R0_5 to i32
		%R0_7 = trunc i32 %R0_6 to i1
		br i1 %R0_7, label %WhileBody0_8, label %WhileEnd0_8

	WhileBody0_8:
		store i32 1, i32* %y
		%R1_10 = load i32, i32* %y
		%R1_11 = icmp slt i32 %R1_10, 10
		%R1_12 = zext i1 %R1_11 to i32
		%R1_13 = trunc i32 %R1_12 to i1
		br i1 %R1_13, label %WhileBody1_14, label %WhileEnd1_14

	WhileBody1_14:
		%R2_15 = load i32, i32* %x
		%R2_16 = load i32, i32* %y
		%R2_17 = mul i32 %R2_15, %R2_16
		call void (i32) @lambda7c_printint(i32 %R2_17)
		%R2_19 = load i32, i32* %x
		%R2_20 = load i32, i32* %y
		%R2_21 = mul i32 %R2_19, %R2_20
		%R2_22 = icmp slt i32 %R2_21, 10
		%R2_23 = zext i1 %R2_22 to i32
		%R2_24 = trunc i32 %R2_23 to i1
		br i1 %R2_24, label %IfTrue2_25, label %IfFalse2_25

	IfTrue2_25:
		%A3_26 = getelementptr inbounds [4 x i8], [4 x i8]* @SVAR3_26, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A3_26)
		br label %EndIf2_25

	IfFalse2_25:
		%A4_29 = getelementptr inbounds [3 x i8], [3 x i8]* @SVAR4_29, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A4_29)
		br label %EndIf2_25

	EndIf2_25:
		%R5_32 = load i32, i32* %y
		%R5_33 = add i32 %R5_32, 1
		store i32 %R5_33, i32* %y
		%R5_35 = load i32, i32* %y
		%R5_36 = icmp slt i32 %R5_35, 10
		%R5_37 = zext i1 %R5_36 to i32
		%R5_38 = trunc i32 %R5_37 to i1
		br i1 %R5_38, label %WhileBody1_14, label %WhileEnd1_14

	WhileEnd1_14:
		%A6_40 = getelementptr inbounds [2 x i8], [2 x i8]* @SVAR6_40, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A6_40)
		%R6_42 = load i32, i32* %x
		%R6_43 = add i32 %R6_42, 1
		store i32 %R6_43, i32* %x
		%R6_45 = load i32, i32* %x
		%R6_46 = icmp slt i32 %R6_45, 10
		%R6_47 = zext i1 %R6_46 to i32
		%R6_48 = trunc i32 %R6_47 to i1
		br i1 %R6_48, label %WhileBody0_8, label %WhileEnd0_8

	WhileEnd0_8:
		ret i32 0

}

