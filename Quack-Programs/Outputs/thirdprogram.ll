target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)

@SVAR_7 =  constant [2 x i8] c"\0a\00", align 1


define i32 @main()
{
	mainstart:
		call void (i32) @mymain(i32 2)
		ret i32 0

}

define void @mymain(i32 %farg_z_14)
{
	mymainmain:
		%z_14 = alloca i32
		store i32 %farg_z_14, i32* %z_14
		%pi_15 = alloca double
		store double 3.1415927, double* %pi_15
		call void (double, double*, i32*) @intelligence_test(double 51.01, double* %pi_15, i32* %z_14)
		ret void

}

define double @area(double %farg_radius_18, double* %pi_15, i32* %z_14)
{
	areamain:
		%radius_18 = alloca double
		store double %farg_radius_18, double* %radius_18
		%radius_18_2 = load double, double* %radius_18
		%radius_18_3 = load double, double* %radius_18
		%R_4 = fmul double %radius_18_2, %radius_18_3
		%pi_15_5 = load double, double* %pi_15
		%R_6 = fmul double %R_4, %pi_15_5
		ret double %R_6

}

define void @intelligence_test(double %farg_x_23, double* %pi_15, i32* %z_14)
{
	intelligence_testmain:
		%x_23 = alloca double
		store double %farg_x_23, double* %x_23
		%z_14_2 = load i32, i32* %z_14
		%pi_24 = alloca i32
		store i32 %z_14_2, i32* %pi_24
		%R_5 = call double (double, double*, i32*) @area(double 10.01, double* %pi_15, i32* %z_14)
		call void (double) @lambda7c_printfloat(double %R_5)
		%A_7 = getelementptr inbounds [2 x i8], [2 x i8]* @SVAR_7, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_7)
		ret void

}

