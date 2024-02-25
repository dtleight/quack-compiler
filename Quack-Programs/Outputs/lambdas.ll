target triple = "x86_64-pc-windows-msvc19.33.31629"
declare void @lambda7c_printint(i32)
declare void @lambda7c_printfloat(double)
declare void @lambda7c_printstr(i8*)
declare i8* @malloc(i32)
declare void @free(i8*)
declare i8* @memcpy(i8*, i8*, i32)

@SVAR_2 =  constant [13 x i8] c"Total Sum: \0a\00", align 1


define i32 @main()
{
	mainstart:
		%total_sum_1 = alloca i32
		store i32 5, i32* %total_sum_1
		%total_sum_1_2 = load i32, i32* %total_sum_1
		call void (i32, i32*) @ts_0(i32 %total_sum_1_2, i32* %total_sum_1)
		ret i32 0

}

define void @ts_0(i32 %larg_ts, i32* %total_sum_1)
{
	ts_0main:
		%ts_2 = alloca i32
		store i32 %larg_ts, i32* %ts_2
		%A_2 = getelementptr inbounds [13 x i8], [13 x i8]* @SVAR_2, i64 0, i64 0
		call void (i8*) @lambda7c_printstr(i8* %A_2)
		%ts_2_4 = load i32, i32* %ts_2
		call void (i32) @lambda7c_printint(i32 %ts_2_4)
		ret void

}

