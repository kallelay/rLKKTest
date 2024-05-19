Imports MathNet.Numerics.Statistics

Public Module loadEISbattery_mst

    Function loadeisBinfile(fpath$, ByRef frequencyVector() As Double, ByRef ZVector() As System.Numerics.Complex)
        Try
            Dim cellID% = 1
            Try : cellID% = Split(fpath, "_Cell_")(1).Substring(0, 1) : Catch : End Try 'Cell ID defaults to 1

            Dim CellVoltage(), CellCurrent(), CellTime() As Double


            Dim filename$ = Split(fpath, "\").Last '20200914_095058_IS_U_Sense_Cell_2 '20170805_021228_IS_Discharge_SoC_20_U_Sense_Cell_2
            Dim filepath$ = Mid(fpath, 1, fpath.Length - filename.Length)
            '20210602_121010_IS_I_Sense_DCP130_Global '20180516_041042_IS_Discharge_SoC_50_I_Sense_DCP130_Global

            Dim cellVoltageFilename$, cellCurrentFilename$

            Dim BlindFilename = Replace(filename, "U_Sense_Cell_1", "?").Replace("U_Sense_Cell_2", "?").Replace("U_Sense_Cell_3", "?").Replace("U_Sense_Cell_4", "?").Replace("U_Sense_Cell_3", "?").Replace("U_Sense_Cell_5", "?").Replace("I_Sense_DCP130_Global", "?").Replace("I_Sense_DCP780_Global", "?").Replace("U_Sense_Stack", "?").Replace("time", "?")

            If cellID <> 0 Then
                cellVoltageFilename = filepath & BlindFilename.Replace("?", "U_Sense_Cell_" & cellID)
            Else
                cellVoltageFilename = filepath & BlindFilename.Replace("?", "U_Sense_Stack")

            End If
            cellCurrentFilename = filepath & BlindFilename.Replace("?", "I_Sense_DCP130_Global")



            If IO.File.Exists(cellVoltageFilename) = False Or IO.File.Exists(cellCurrentFilename) = False Then
                MsgBox("File doesn't exist" & cellVoltageFilename)
            End If

            'read voltage
            Dim f As New IO.FileStream(cellVoltageFilename, IO.FileMode.Open, IO.FileAccess.Read)
            Dim bs As New IO.BinaryReader(f)

            Dim TAKE_TIME = 0
            Dim N%
            Dim Fs = 5000.0
            Dim SKIP_TIME% = 0

            If TAKE_TIME <= 0 Then N = f.Length / 8 Else N = TAKE_TIME * Fs '- Fs
            'If SKIP_TIME > 0 Then N -= SKIP_TIME * Fs

            N = Math.Max(Math.Min(N, f.Length / 8), 0)






            ReDim CellVoltage(N - 1)
            ReDim CellCurrent(N - 1)

            ' N = f.Length / 8 - 1 'length 


            Dim tmpbuffer() As Byte ' = bs.ReadBytes(N * 8)
            ' Buffer.BlockCopy(tmpbuffer, 0, CellVoltage, 0, f.Length)

            bs.ReadBytes(SKIP_TIME * Fs * 8)
            tmpbuffer = bs.ReadBytes(N * 8)
            Buffer.BlockCopy(tmpbuffer, 0, CellVoltage, 0, N * 8) '-AAX

            ' Buffer.BlockCopy(tmpbuffer, SKIP_TIME * Fs * 8, CellVoltage, 0, N * 8)
            bs.Close()
            tmpbuffer = Nothing
            f = Nothing
            bs = Nothing



            'read current
            f = New IO.FileStream(cellCurrentFilename, IO.FileMode.Open, IO.FileAccess.Read)
            bs = New IO.BinaryReader(f)

            'tmpbuffer = bs.ReadBytes(f.Length)
            bs.ReadBytes(SKIP_TIME * Fs * 8)
            tmpbuffer = bs.ReadBytes(N * 8)
            Buffer.BlockCopy(tmpbuffer, 0, CellCurrent, 0, N * 8) '-AAX

            'Buffer.BlockCopy(tmpbuffer, 0, CellCurrent, 0, f.Length) '-AAX
            ' Buffer.BlockCopy(tmpbuffer, SKIP_TIME * Fs * 8, CellCurrent, 0, )
            bs.Close()
            tmpbuffer = Nothing
            f = Nothing
            bs = Nothing





            'ReDim Preserve CellCurrent(f.Length / 8 - 1)
            'status_loadingfile$ = "Removing Voltage Offset"

            Dim OCV = MathNet.Numerics.Statistics.Statistics.Mean(CellVoltage)
            Dim CVmOCV = MathNet.Numerics.LinearAlgebra.Double.Vector.Build.DenseOfArray(CellVoltage) '
            CVmOCV -= OCV
            Dim CellVoltageR = CVmOCV.ToArray()
            Dim CellVoltageI(CellVoltageR.Count - 1) As Double

            Dim CellCurrentR = CellCurrent.Clone()
            Dim cellcurrenti(CellCurrent.Count - 1) As Double

            MathNet.Numerics.IntegralTransforms.Fourier.Forward(CellVoltageR, CellVoltageI)



            MathNet.Numerics.IntegralTransforms.Fourier.Forward(CellCurrentR, cellcurrenti)



            Dim MAX_POINTS = 1000
            Dim Cursor = 0

            ReDim ZVector(MAX_POINTS)
            ' ReDim ZVector_Im(MAX_POINTS)
            'ReDim ZVector_Re(MAX_POINTS)
            'ReDim ZVector_Abs(MAX_POINTS)
            'ReDim ZVector_Angle(MAX_POINTS)
            ReDim frequencyVector(MAX_POINTS)


            Dim actualThreshold = cellcurrenti.MaximumAbsolute * 0.8



            For k = 0 To cellcurrenti.Length / 2  '- 1
                ' Debug.WriteLine(New Complex(CellCurrentR(k), cellcurrenti(k)).Magnitude)
                Dim trytoguessv = 0


                Dim tmpv As Double = New System.Numerics.Complex(CellCurrentR(k), cellcurrenti(k)).Magnitude
                '  If Rnd() < 0.1 Then
                '  listV.Add(tmpv) 'add pickup values
                '   End If
                '


                If tmpv > actualThreshold Then
                    ZVector(Cursor) = New System.Numerics.Complex(CellVoltageR(k), CellVoltageI(k)) / New System.Numerics.Complex(CellCurrentR(k), cellcurrenti(k))
                    ' ZVector_Re(Cursor) = ZVector(Cursor).Real
                    ' ZVector_Im(Cursor) = ZVector(Cursor).Imaginary
                    ' ZVector_Abs(Cursor) = ZVector(Cursor).Magnitude
                    ' ZVector_Angle(Cursor) = ZVector(Cursor).Phase
                    frequencyVector(Cursor) = k / cellcurrenti.Length * Fs
                    Cursor += 1


                End If

            Next




            ReDim Preserve ZVector(Cursor - 1)
            ' ReDim Preserve ZVector_Re(Cursor - 1)
            ' ReDim Preserve ZVector_Im(Cursor - 1)
            '  ReDim Preserve ZVector_Abs(Cursor - 1)
            '  ReDim Preserve ZVector_Angle(Cursor - 1)
            ReDim Preserve frequencyVector(Cursor - 1)






            'End Using




            '    Public frequencyVector() As Double
            '  Public ZVector_Re(), ZVector_Im() As Double
            '  Public ZVector_Abs(), ZVector_Angle() As Double
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Function
End Module
