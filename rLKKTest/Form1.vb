Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading
Imports MathNet.Numerics
Imports MathNet.Numerics.Data.Matlab
Imports MathNet.Numerics.LinearAlgebra
Imports OxyPlot
Imports OxyPlot.Axes
Imports OxyPlot.Legends
Imports OxyPlot.Series
Imports OxyPlot.WindowsForms

Public Class Form1

    'Loading modes (enum)
    Enum LoadModesEnum
        modecsv = 0
        modeagilent = 1


        modebin = 2
        modematlab = 3
        modeirf = 4
        modebattPana = 5

    End Enum



    Dim loadMode As LoadModesEnum = LoadModesEnum.modecsv 'Defaults to CSV 
    Dim traceAIsDetected As Boolean = False 'Trace A and B in Agilent file


    Dim oldFileName$ = "" 'mem file name
    Dim memoryData As New List(Of List(Of Double)) 'memorize impedances

    '----------------------------------------- init -----------------------------------------------'
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        'Make sure that sep is point 
        Thread.CurrentThread.CurrentCulture =
        New System.Globalization.CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture =
        New System.Globalization.CultureInfo("en-US")

        If My.Application.CommandLineArgs.Count > 0 Then
            For Each arg In My.Application.CommandLineArgs

                Select Case LCase(arg).Split("=")(0)

                    Case "-fmin"
                        Try : fminTextBox.Text = Split(arg, "=")(1) : Catch ex As Exception : End Try

                    Case "-fmax"
                        Try : fmaxTextBox.Text = Split(arg, "=")(1) : Catch ex As Exception : End Try

                    Case "-lambda"
                        Try : lambdaTextBox.Text = Split(arg, "=")(1) : Catch ex As Exception : End Try

                    Case "-npts"
                        Try : nDRTTextBox.Text = Split(arg, "=")(1) : Catch ex As Exception : End Try

                    Case "-r0"
                        Try : CheckBoxR0.Checked = Split(arg, "=")(1) : Catch ex As Exception : End Try

                    Case "-rinf"
                        Try : CheckBoxRinf.Checked = Split(arg, "=")(1) : Catch ex As Exception : End Try

                    Case "-help"
                        MsgBox("rLKKText.exe ""filepath"" <arguments> 
Arguments:
-fmin=<val> -fmax=<val> -lambda=<val> -npts=<val> -r0=<0/1> -rinf=<0/1>
-export
-exit 
")
                        End

                    Case "-exit"
                        End




                    Case "-export"
                        ExportDataGridViewToTextFile(DataGridViewImpedance, Filepathtext.Text & "_export.txt")
                    Case Else
                        Filepathtext.Text = arg

                End Select
            Next
        End If


        'initialize labels and combobox
        fileinfLabel.Text = ""
        moreinfoLabel.Text = ""
        rLKKparamsInfoLabel.Text = ""

        ComboBoxRefDev.SelectedIndex = 1
        ComboBoxWeight.SelectedIndex = 0



    End Sub

    'Update status (loading progress), sent as a status$ and ratio number out of 7
    Private Sub updateStatus(status$, progressRatio!)

        progressRatio! /= 7
        loadingscreen.status.Text = status
        loadingscreen.progress.Text = Int(progressRatio * 100) & "%"
        Application.DoEvents()
    End Sub



    '------------------------------------ internal and I/O ---------------------------------------------------'
    Private Sub LoadFile()

        Dim needToReplaceCommaWithPoints As Boolean = False


        'Show loading screen
        loadingscreen.Show(Me)
        loadingscreen.Location = Me.Location + New Point(Me.Width / 2 - loadingscreen.Width / 2, Me.Height / 2 - loadingscreen.Height / 2)
        updateStatus("Starting", 0)
        Application.DoEvents()

        'local variables 
        Dim frequencies As New List(Of Double)
        Dim realParts As New List(Of Double)
        Dim imaginaryParts As New List(Of Double)



        'Status: loading
        updateStatus("Loading File", 1)


        loadMode = LoadModesEnum.modecsv 'expecting csv or tsv by default
        traceAIsDetected = False 'reset detected trace A for agilent mode


        Dim filePath = Filepathtext.Text 'get filepath from text file (done it this way for later expansions)

        'Detect file type from extension
        If Strings.Right(filePath, 3).ToLower = "mat" Then GoTo load_matlab
        If Strings.Right(filePath, 3).ToLower = "bin" Then GoTo load_bin
        If Strings.Right(filePath, 3).ToLower = "irf" Then GoTo load_irf

        Application.DoEvents()
        '------------------- normal text file I/O section (default) -------------------------------------------'

        Dim strAll As String = File.ReadAllText(filePath) 'load all text, once


        'Further detect file type
        If InStr(strAll, "4294A") > 0 Then loadMode = LoadModesEnum.modeagilent 'use agilent mode
        If InStr(strAll, "Battery name") > 0 Then loadMode = LoadModesEnum.modebattPana  'use agilent mode

        'Most chars => guess delimiter
        Dim characterFrequencies = strAll.Where(Function(c) Not IsNumeric(c) And c <> vbCr And c <> vbLf And c <> "+" And c <> "-" And c <> "E" And c <> "e").
                                      GroupBy(Function(c) c).
                                      Select(Function(g) New With {.Character = g.Key, .Count = g.Count()}).
                                      OrderByDescending(Function(c) c.Count).
                                      ThenBy(Function(c) c.Character).
                                      ToList()

        'Guess delimiter?
        Dim decSep As Char = characterFrequencies(0).Character
        Dim valSep As Char = characterFrequencies(1).Character

        'Wrong delimiter?
        If valSep = "." Then
            decSep = valSep + Chr(0)
            valSep = characterFrequencies(0).Character
        End If


        'Bad delimiter in the loaded csv/txt file
        If decSep = "," Then
            If MsgBox("The file may be made with comma as digital separator." & vbCrLf & "Is this correct?", MsgBoxStyle.YesNo, "Question") = MsgBoxResult.Yes Then
                needToReplaceCommaWithPoints = True
            End If
        End If


        'Load lines
        Dim lines As String() = File.ReadAllLines(filePath)

        'temp variables for reading from file
        Dim frequency As Double
        Dim realPart As Double
        Dim imaginaryPart As Double


        Dim curLine = 0 'cursor for lines
        Dim steps = Int(lines.Count / 10) 'loading file is 10 steps

        For Each line As String In lines
            curLine += 1

            'update progress form
            If curLine Mod steps = 0 Then
                updateStatus(String.Format("Loading File ({0:0.0} %)", curLine / steps * 100), 1)
            End If


            'trim line
            line = Trim(line)

            'make sure that numbers are correctly formatted if user allows it
            If needToReplaceCommaWithPoints Then line = Replace(line, ",", ".")


            'Ensure that no excess delimited (e.g., space) is present
            While line IsNot Nothing AndAlso line.IndexOf(valSep & valSep) <> -1
                line = Replace(line, valSep & valSep, valSep)
            End While

            'Early retreat
            If InStr(line, """TRACE: A""") > 0 Then traceAIsDetected = True 'Trace A only in Agilent
            If line Is Nothing Then Continue For 'if nothing in the line, continue
            If traceAIsDetected And InStr(line, """TRACE: B""") > 0 Then Exit For 'exit if trace b is detected


            'We'll use classical way (instead of Regex) to separate TSV/CSV 
            Dim parts As String() = line.Split(valSep)
            If parts.Length >= 3 Then


                If loadMode = LoadModesEnum.modebattPana Then 'Is it from Kollmeyer panasonic file? => map to 13, 21, and 22 'HACK
                    If Not IsNumeric(parts(13)) Then Continue For

                    frequency = Double.Parse(parts(13))
                    realPart = Double.Parse(parts(21))
                    imaginaryPart = Double.Parse(parts(22))



                Else 'normal load
                    If Not IsNumeric(parts(0)) Then
                        If InStr(parts(0), "Frequency") > 0 Then
                            Continue For
                        End If
                        Continue For
                    End If



                    frequency = Double.Parse(parts(0))
                    realPart = Double.Parse(parts(1))
                    imaginaryPart = Double.Parse(parts(2))

                End If

                frequencies.Add(frequency)
                realParts.Add(realPart)
                imaginaryParts.Add(imaginaryPart)
            End If
        Next

        GoTo finish_loading

        '------------------- MATLAB file I/O section (default) -------------------------------------------'
load_matlab:

        'Load f and Z from mat file
        Dim freqs = MatlabReader.Read(Of Double)(Filepathtext.Text, "f")
        Dim Zall = MatlabReader.Read(Of System.Numerics.Complex)(Filepathtext.Text, "Z")

        'Faster load...
        frequencies.AddRange(MatrixToVector(freqs).ToArray)
        realParts.AddRange(MatrixToVector(Zall.Real).ToArray)
        imaginaryParts.AddRange(MatrixToVector(Zall.Imaginary).ToArray)

        GoTo finish_loading
        '------------------- Battery MST I/O section  -------------------------------------------'
load_bin:

        'Same, use the "messy" loadeisbin file from GUIISAY project 
        Dim frequencyVector() As Double
        Dim Zvector() As System.Numerics.Complex
        loadeisBinfile(Filepathtext.Text, frequencyVector, Zvector)
        Dim _tmpzvec = Vector(Of System.Numerics.Complex).Build.DenseOfArray(Zvector)

        frequencies.AddRange(frequencyVector.ToArray)
        realParts.AddRange(_tmpzvec.Real.ToArray())
        imaginaryParts.AddRange(_tmpzvec.Imaginary.ToArray())
        _tmpzvec = Nothing
        Zvector = Nothing
        frequencyVector = Nothing

        GoTo finish_loading

        '------------------- IRF file I/O section -------------------------------------------'
        'For the Safion device, again the measurements are messy in most cases and is overengineered
load_irf:
        Dim strAllXML As String = File.ReadAllText(filePath) 'load all text, once
        Dim allTokens = Split(strAllXML, "<spectrumData freq=")
        ' Dim pattern As String = "[-+]?\d*\.?\d+"
        For k = 1 To allTokens.Count - 1

            ' Match numbers using regex
            '  Dim matches As MatchCollection = Regex.Matches(allTokens(k), pattern)
            ' frequencies.Add(matches(0).Value)
            'realParts.Add(matches(1).Value)
            'maginaryParts.Add(matches(2).Value)

            frequencies.Add(Split(allTokens(k), Chr(34))(1))
            realParts.Add(Split(allTokens(k), Chr(34))(3))
            imaginaryParts.Add(Split(allTokens(k), Chr(34))(5))
        Next

        GoTo finish_loading



        '------------------------ Converge all loading possibilities -------------------------------------'
finish_loading:
        FinishLoading(frequencies, realParts, imaginaryParts)

        updateStatus("Finished", 7)
        loadingscreen.Hide()
    End Sub
    Private Sub FinishLoading(frequencies, realParts, imaginaryParts)


        updateStatus("Saving data in RAM", 2)
        'Keep data in memory
        memoryData.Clear()
        memoryData.Add(frequencies)
        memoryData.Add(realParts)
        memoryData.Add(imaginaryParts)



        updateStatus("Calculating rLKK", 3)
        'Second step: Perform rLKK
        rLKK(frequencies.ToArray(), realParts.ToArray(), imaginaryParts.ToArray(), rlkk_drt_settings, ComboBoxWeight.SelectedIndex)



        'Do plots
        updateStatus("Plotting", 4)
        PlotNyquist(realParts, imaginaryParts, CheckBoxPlotrLKK.Checked)
        PlotBode(frequencies, realParts, imaginaryParts, CheckBoxPlotrLKK.Checked)
        PlotDeviationFromMemory()
        PlotXFromMemory()


        updateStatus("Populating Grid View", 5)
        'Add to Grid view 
        PopulateDataGridView(frequencies, realParts, imaginaryParts, rLKK_Z.ToArray(), rLKK_Zdev.ToArray)

        updateStatus("Updating Info", 6)
        'Update info
        UpdateInfo(Filepathtext.Text)

        Exit Sub

    End Sub

    'Update info in file
    Private Sub UpdateInfo(Optional filename$ = "")
        If memoryData.Count = 0 Then Exit Sub

        If filename = "" Then filename = oldFileName
        oldFileName = filename

        fileinfLabel.Text = "Filename: " & Split(filename, "\").Last & vbCrLf & String.Format("Npts: {0}", memoryData(0).Count)
        moreinfoLabel.Text = "Freqs: " & memoryData(0).Min() & "Hz - " & memoryData(0).Max() & "Hz"

        rLKKparamsInfoLabel.Text = String.Format("DRT freqs: {0: 0.00e+00} Hz - {1: 0.00e+00} Hz ({2} points)" & vbCrLf & "lambda: {3:0.00e+00}", rlkk_drt_settings(0), rlkk_drt_settings(1),
                                                 rlkk_drt_settings(2), rlkk_drt_settings(3))

    End Sub

    'Populate Grid View
    Private Sub PopulateDataGridView(frequencies As List(Of Double), realParts As List(Of Double), imaginaryParts As List(Of Double), ZLKK As System.Numerics.Complex(), ZLKKRes As Double())
        _datagridisbeingfilled = True
        DataGridViewImpedance.Rows.Clear()
        DataGridViewImpedance.Columns.Clear()

        DataGridViewImpedance.Columns.Add("Frequency", "Frequency (Hz)")
        DataGridViewImpedance.Columns.Add("RealPart", "Real Part")
        DataGridViewImpedance.Columns.Add("ImaginaryPart", "Imaginary Part")
        DataGridViewImpedance.Columns.Add("RealPartrLKK", "Real Part (rLKK)")
        DataGridViewImpedance.Columns.Add("ImaginaryPartrLKK", "Imaginary Part (rLKK)")
        DataGridViewImpedance.Columns.Add("DevRLKK", "Deviation in %")


        'It was supposed to be "pigment" not pickment, changeme later
        'Highlight > 1%, saturate at 3% deviation (red)

        For i As Integer = 0 To frequencies.Count - 1
            DataGridViewImpedance.Rows.Add(frequencies(i), realParts(i), imaginaryParts(i), ZLKK(i).Real, ZLKK(i).Imaginary, ZLKKRes(i))
            If Math.Abs(ZLKKRes(i)) > 1 Then
                Dim pickment As Color
                Dim value = Math.Abs(ZLKKRes(i))

                If value <= 1 Then
                    pickment = Color.Yellow
                ElseIf value <= 2 Then
                    pickment = InterpolateColor(Color.Yellow, Color.Red, value - 1)
                ElseIf value <= 3 Then
                    pickment = InterpolateColor(Color.Red, Color.FromArgb(160, 0, 0), value - 2)
                Else
                    pickment = Color.FromArgb(160, 0, 0)
                End If


                DataGridViewImpedance.Rows(DataGridViewImpedance.Rows.Count - 1).DefaultCellStyle.BackColor = pickment
                DataGridViewImpedance.Rows(DataGridViewImpedance.Rows.Count - 1).Cells(4).Style.BackColor = pickment
            End If
        Next
        'DataGridViewImpedance.Columns("RealPartrLKK").Frozen = True
        'DataGridViewImpedance.Columns("ImaginaryPartrLKK").Frozen = True
        'DataGridViewImpedance.Columns("DevRLKK").Frozen = True
        DataGridViewImpedance.Columns("RealPartrLKK").ReadOnly = True
        DataGridViewImpedance.Columns("ImaginaryPartrLKK").ReadOnly = True
        DataGridViewImpedance.Columns("DevRLKK").ReadOnly = True
        _datagridisbeingfilled = False
    End Sub











    Public nyquistSeriesSelect As New ScatterSeries
    '----------------------------------- plotting --------------------------------------------'
    Private Sub PlotNyquist(realParts As List(Of Double), imaginaryParts As List(Of Double), Optional plotrLKK As Boolean = False)
        Dim nyquistPlotModel As New PlotModel With {
            .Title = "Nyquist Plot",
            .EdgeRenderingMode = EdgeRenderingMode.Adaptive,
            .PlotType = PlotType.Cartesian
        }



        Dim nyquistSeries As New LineSeries With {
            .Title = "Measurement",
            .MarkerType = MarkerType.Circle
        }

        nyquistSeriesSelect = New ScatterSeries With {
            .MarkerType = MarkerType.Square,
            .MarkerStroke = OxyColors.Purple,
            .MarkerStrokeThickness = 2,
            .MarkerFill = OxyColors.Transparent
        }


        AddHandler nyquistSeries.MouseDown, AddressOf sNyq_MouseDown
        Dim xAxis As New LinearAxis With {
            .Position = AxisPosition.Bottom,
            .Title = "Real Part",
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot
        }
        nyquistPlotModel.Axes.Add(xAxis)

        Dim yAxis As New LinearAxis With {
            .Position = AxisPosition.Left,
            .Title = "Imaginary Part",
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
            .StartPosition = If(CheckboxReverseNyq.Checked, 1, 0), ' Reverse the Y-axis
            .EndPosition = If(CheckboxReverseNyq.Checked, 0, 1)
        }
        nyquistPlotModel.Axes.Add(yAxis)

        For i As Integer = 0 To realParts.Count - 1
            nyquistSeries.Points.Add(New DataPoint(realParts(i), imaginaryParts(i)))
        Next

        nyquistPlotModel.Series.Add(nyquistSeries)


        If plotrLKK Then

            Dim nyquistrLKKSeries As New LineSeries With {
                .Title = "rLKK reconstruction",
                .MarkerType = MarkerType.Square
            }
            For i As Integer = 0 To realParts.Count - 1
                nyquistrLKKSeries.Points.Add(New DataPoint(rLKK_Z.Real(i), rLKK_Z.Imaginary(i)))
            Next


            nyquistPlotModel.Series.Add(nyquistrLKKSeries)

            nyquistPlotModel.Legends.Add(New Legend() With {
    .LegendPosition = LegendPosition.RightBottom
})



        End If

        nyquistPlotModel.Series.Add(nyquistSeriesSelect)








        PlotViewNyquist.Model = nyquistPlotModel


    End Sub

    Private Sub PlotBode(frequencies As List(Of Double), realParts As List(Of Double), imaginaryParts As List(Of Double), Optional plotrLKK As Boolean = False)

        Dim magnitudeSeries As New LineSeries With {
            .Title = "Magnitude",
            .MarkerType = MarkerType.Circle
        }



        Dim phaseSeries As New LineSeries With {
            .Title = "Phase",
            .MarkerType = MarkerType.Circle
        }

        For i As Integer = 0 To frequencies.Count - 1
            Dim frequency As Double = frequencies(i)
            Dim realPart As Double = realParts(i)
            Dim imaginaryPart As Double = imaginaryParts(i)

            Dim magnitude As Double = Math.Sqrt(realPart * realPart + imaginaryPart * imaginaryPart)
            Dim phase As Double = Math.Atan2(imaginaryPart, realPart) * (180.0 / Math.PI) ' Convert to degrees

            magnitudeSeries.Points.Add(New DataPoint(frequency, magnitude)) ' Magnitude in ohm
            phaseSeries.Points.Add(New DataPoint(frequency, phase)) ' Phase in degrees
        Next



        Dim bodePlotMagModel As New PlotModel With {
            .Title = "Bode Magnitude Plot"
        }

        Dim bodePlotPhModel As New PlotModel With {
            .Title = "Bode Phase Plot"
        }

        bodePlotMagModel.Series.Add(magnitudeSeries)
        bodePlotPhModel.Series.Add(phaseSeries)

        AddHandler bodePlotMagModel.MouseDown, AddressOf sBode_MouseDown
        AddHandler bodePlotPhModel.MouseDown, AddressOf sBode_MouseDown



        If plotrLKK Then
            Dim magnitudeSeriesrLKK As New LineSeries With {
            .Title = "Magnitude (rLKK)",
            .MarkerType = MarkerType.Square
        }



            Dim phaseSeriesrLKK As New LineSeries With {
            .Title = "Phase (rLKK)",
            .MarkerType = MarkerType.Square
        }

            For i As Integer = 0 To frequencies.Count - 1
                Dim frequency As Double = frequencies(i)
                Dim realPart As Double = rLKK_Z.Real(i)
                Dim imaginaryPart As Double = rLKK_Z.Imaginary(i)

                Dim magnitude As Double = Math.Sqrt(realPart * realPart + imaginaryPart * imaginaryPart)
                Dim phase As Double = Math.Atan2(imaginaryPart, realPart) * (180.0 / Math.PI) ' Convert to degrees

                magnitudeSeriesrLKK.Points.Add(New DataPoint(frequency, magnitude)) ' Magnitude in ohm
                phaseSeriesrLKK.Points.Add(New DataPoint(frequency, phase)) ' Phase in degrees
            Next

            bodePlotMagModel.Series.Add(magnitudeSeriesrLKK)
            bodePlotPhModel.Series.Add(phaseSeriesrLKK)



            bodePlotMagModel.Legends.Add(New Legend() With {
    .LegendPosition = LegendPosition.RightBottom
})



        End If



        'log scale?
        If CheckBoxLogScale.Checked Then
            Dim logXAxis As New LogarithmicAxis With {
            .Position = AxisPosition.Bottom,
            .Title = "Frequency in Hz",
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
            .Base = 10
        }
            Dim logXAxis2 As New LogarithmicAxis With {
            .Position = AxisPosition.Bottom,
            .Title = "Frequency in Hz",
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
            .Base = 10
        }

            bodePlotMagModel.Axes.Add(logXAxis)
            bodePlotPhModel.Axes.Add(logXAxis2)
        Else 'linear axis
            Dim logXAxis As New LinearAxis With {
         .Position = AxisPosition.Bottom,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
         .Title = "Frequency in Hz"
     }
            Dim logXAxis2 As New LinearAxis With {
         .Position = AxisPosition.Bottom,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
         .Title = "Frequency in Hz"
     }
            bodePlotMagModel.Axes.Add(logXAxis)
            bodePlotPhModel.Axes.Add(logXAxis2)
        End If



        ' Create linear y-axis for magnitude
        Dim magnitudeYAxis As New LinearAxis With {
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
            .Title = "Magnitude in Ohm"
        }
        bodePlotMagModel.Axes.Add(magnitudeYAxis)



        ' Add separate axis for Phase
        Dim phaseAxis As New LinearAxis With {
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
            .Title = "Phase in °"
        }
        bodePlotPhModel.Axes.Add(phaseAxis)





        '  bodePlotMagModel.Axes.Add(phaseAxis)
        PlotViewBodeMag.Model = bodePlotMagModel
        PlotViewBodePhase.Model = bodePlotPhModel
    End Sub



    Dim devSeriesSelect = New ScatterSeries
    Private Sub PlotDeviationFromMemory()
        If memoryData Is Nothing OrElse memoryData.Count = 0 OrElse memoryData(1).Count = 0 Then Exit Sub


        Dim magnitudeSeries As New ScatterSeries With {
            .Title = "Magnitude",
            .MarkerType = MarkerType.Circle
        }

        Dim realSeries As New ScatterSeries With {
            .Title = "Real",
            .MarkerType = MarkerType.Square
        }

        Dim imagSeries As New ScatterSeries With {
            .Title = "Imag",
            .MarkerType = MarkerType.Triangle
        }


        'HACK: Hard coded
        Dim threshold1Series As New LineSeries With {
            .Title = "Threshold" & If(CheckBox1perth.Checked, " 1%", " 2%"),
            .LineStyle = LineStyle.Dash,
            .Color = OxyColors.Black
        }
        '  .Title = "Threshold" & If(CheckBox1perth.Checked, " -1%", " -2%"),

        Dim threshold2Series As New LineSeries With {
        .LineStyle = LineStyle.Dash,
            .Color = OxyColors.Black
        }


        devSeriesSelect = New ScatterSeries With {
            .MarkerType = MarkerType.Square,
            .MarkerStroke = OxyColors.Purple,
            .MarkerStrokeThickness = 2,
            .MarkerFill = OxyColors.Transparent
        }

        AddHandler magnitudeSeries.MouseDown, AddressOf sDev_MouseDown


        Dim frequency, realPart, imaginaryPart, magPart As Double
        For i As Integer = 0 To memoryData(0).Count - 1

            frequency = memoryData(0)(i)
            Dim magnitude As Double = Math.Sqrt(memoryData(1)(i) * memoryData(1)(i) + memoryData(2)(i) * memoryData(2)(i))

            If RadioButtonPercent.Checked Then

                If ComboBoxRefDev.SelectedIndex = 0 Then 'reference real/imag or magnitude?
                    realPart = (rLKK_Z.Real(i) - memoryData(1)(i)) / rLKK_Z.Real(i) * 100
                    imaginaryPart = (rLKK_Z.Imaginary(i) - memoryData(2)(i)) / rLKK_Z.Imaginary(i) * 100
                    magPart = (rLKK_Z(i).Magnitude - magnitude) / rLKK_Z(i).Magnitude * 100

                Else
                    realPart = (rLKK_Z.Real(i) - memoryData(1)(i)) / rLKK_Z(i).Magnitude * 100
                    imaginaryPart = (rLKK_Z.Imaginary(i) - memoryData(2)(i)) / rLKK_Z(i).Magnitude * 100
                    magPart = (rLKK_Z(i).Magnitude - magnitude) / rLKK_Z(i).Magnitude * 100

                End If
            ElseIf RadioButtonPpm.Checked Then

                If ComboBoxRefDev.SelectedIndex = 0 Then
                    realPart = (rLKK_Z.Real(i) - memoryData(1)(i)) / rLKK_Z.Real(i) * 1000000.0
                    imaginaryPart = (rLKK_Z.Imaginary(i) - memoryData(2)(i)) / rLKK_Z.Imaginary(i) * 1000000.0
                    magPart = (rLKK_Z(i).Magnitude - magnitude) / rLKK_Z(i).Magnitude * 1000000.0
                Else
                    realPart = (rLKK_Z.Real(i) - memoryData(1)(i)) / rLKK_Z(i).Magnitude * 1000000.0
                    imaginaryPart = (rLKK_Z.Imaginary(i) - memoryData(2)(i)) / rLKK_Z(i).Magnitude * 1000000.0
                    magPart = (rLKK_Z(i).Magnitude - magnitude) / rLKK_Z(i).Magnitude * 1000000.0

                End If
            Else
                realPart = rLKK_Z.Real(i) - memoryData(1)(i)
                imaginaryPart = (rLKK_Z.Imaginary(i) - memoryData(2)(i))
                magPart = (rLKK_Z(i).Magnitude - magnitude)
            End If

            ' Dim phase As Double = Math.Atan2(imaginaryPart, realPart) * (180.0 / Math.PI) ' Convert to degrees

            realSeries.Points.Add(New ScatterPoint(frequency, realPart))
            imagSeries.Points.Add(New ScatterPoint(frequency, imaginaryPart))
            magnitudeSeries.Points.Add(New ScatterPoint(frequency, magPart))



            ' AddHandler realSeries.MouseDown, AddressOf sDev_MouseDown
            ' AddHandler imagSeries.MouseDown, AddressOf sDev_MouseDown


            If RadioButtonPercent.Checked Then
                threshold1Series.Points.Add(New DataPoint(frequency, 1 * If(CheckBox2perth.Checked, 2, 1)))
                threshold2Series.Points.Add(New DataPoint(frequency, -1 * If(CheckBox2perth.Checked, 2, 1)))
            ElseIf RadioButtonPpm.Checked Then
                threshold1Series.Points.Add(New DataPoint(frequency, 10000.0 * If(CheckBox2perth.Checked, 2, 1)))
                threshold2Series.Points.Add(New DataPoint(frequency, -10000.0 * If(CheckBox2perth.Checked, 2, 1)))
            Else
                threshold1Series.Points.Add(New DataPoint(frequency, rLKK_Z(i).Magnitude * 0.01 * If(CheckBox2perth.Checked, 2, 1)))
                threshold2Series.Points.Add(New DataPoint(frequency, -rLKK_Z(i).Magnitude * 0.01 * If(CheckBox2perth.Checked, 2, 1)))
            End If

        Next



        Dim devPlotModel As New PlotModel With {
            .Title = "Deviation"
        }

        If CheckBoxDevReal.Checked Then devPlotModel.Series.Add(realSeries)
        If CheckBoxDevImg.Checked Then devPlotModel.Series.Add(imagSeries)
        If CheckBoxDevMag.Checked Then devPlotModel.Series.Add(magnitudeSeries)
        If CheckBoxDevMag.Checked Then devPlotModel.Series.Add(devSeriesSelect)

        If CheckBox1perth.Checked Or CheckBox2perth.Checked Then devPlotModel.Series.Add(threshold1Series) : devPlotModel.Series.Add(threshold2Series)



        'log scale?
        If CheckBoxLogScale.Checked Then
            Dim logXAxis As New LogarithmicAxis With {
            .Position = AxisPosition.Bottom,
            .Title = "Frequency in Hz",
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
            .Base = 10
        }


            devPlotModel.Axes.Add(logXAxis)
        Else 'linear axis
            Dim logXAxis As New LinearAxis With {
         .Position = AxisPosition.Bottom,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
         .Title = "Frequency in Hz"
     }
            devPlotModel.Axes.Add(logXAxis)
        End If



        ' Create linear y-axis for magnitude
        Dim magnitudeYAxis As New LinearAxis With {
            .Position = AxisPosition.Left,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
            .Title = "Deviation in " & If(RadioButtonPercent.Checked, "%", If(RadioButtonPpm.Checked, "ppm", "Ohm"))
        }
        devPlotModel.Axes.Add(magnitudeYAxis)

        devPlotModel.Legends.Add(New Legend() With {
    .LegendTitle = "Legend",
    .LegendPosition = LegendPosition.RightBottom
})







        '  bodePlotMagModel.Axes.Add(phaseAxis)
        PlotViewDev.Model = devPlotModel

    End Sub





    Private Sub PlotXFromMemory()
        If memoryData Is Nothing OrElse memoryData.Count = 0 OrElse memoryData(1).Count = 0 Then Exit Sub


        Dim solSeries As New LineSeries With {
            .Title = "Solution",
            .MarkerType = MarkerType.Circle
        }
        For i As Integer = 0 To rLKK_f.Count - 1

            solSeries.Points.Add(New DataPoint(rLKK_f(i), rLKK_x(i)))
        Next



        Dim solPlotModel As New PlotModel With {
            .Title = "x"
        }

        solPlotModel.Series.Add(solSeries)

        'log scale?
        If CheckBoxLogScale.Checked Then
            Dim logXAxis As New LogarithmicAxis With {
            .Position = AxisPosition.Bottom,
            .Title = "Frequency in Hz",
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
            .Base = 10
        }


            solPlotModel.Axes.Add(logXAxis)
        Else 'linear axis
            Dim logXAxis As New LinearAxis With {
         .Position = AxisPosition.Bottom,
            .MajorGridlineStyle = LineStyle.Solid,
            .MinorGridlineStyle = LineStyle.Dot,
         .Title = "Frequency in Hz"
     }
            solPlotModel.Axes.Add(logXAxis)
        End If



        PlotViewrLKK.Model = solPlotModel

    End Sub


    Sub updateNyquistandAllFromMemory()
        If memoryData.Count = 0 Then Exit Sub
        rLKK(memoryData(0).ToArray, memoryData(1).ToArray, memoryData(2).ToArray, rlkk_drt_settings, ComboBoxWeight.SelectedIndex)
        PlotNyquist(memoryData(1), memoryData(2), CheckBoxPlotrLKK.Checked)
        PlotBode(memoryData(0), memoryData(1), memoryData(2), CheckBoxPlotrLKK.Checked)
        PlotDeviationFromMemory()
        PlotXFromMemory()


        PopulateDataGridView(memoryData(0), memoryData(1), memoryData(2), rLKK_Z.ToArray(), rLKK_Zdev.ToArray)
        UpdateInfo()

    End Sub

    '------------------ user interaction (selection) from plots -------------------------'
    Sub sDev_MouseDown(sender As Object, e As OxyMouseDownEventArgs)
        Dim series = PlotViewDev.Model.GetSeriesFromPoint(e.Position)
        If series Is Nothing Then Return

        Dim nearestPoint = series.GetNearestPoint(e.Position, False)
        If nearestPoint Is Nothing Then Return

        DataGridViewImpedance.ClearSelection()
        DataGridViewImpedance.Rows(nearestPoint.Index).Selected = True
        DataGridViewImpedance.Select()

        Dim dataPoint = nearestPoint.DataPoint
        'Dim scatterPoint = series.
        'FirstOrDefault(Function(x) x.X.Equals(dataPoint.X) AndAlso x.Y.Equals(dataPoint.Y))

    End Sub

    Sub sBode_MouseDown(sender As Object, e As OxyMouseDownEventArgs)
        Dim series = PlotViewBodeMag.ActualModel.GetSeriesFromPoint(e.Position)
        If series Is Nothing Then Return

        Dim nearestPoint = series.GetNearestPoint(e.Position, False)
        If nearestPoint Is Nothing Then Return

        DataGridViewImpedance.ClearSelection()
        DataGridViewImpedance.Rows(nearestPoint.Index).Selected = True
        DataGridViewImpedance.Select()

        Dim dataPoint = nearestPoint.DataPoint
        'Dim scatterPoint = series.
        'FirstOrDefault(Function(x) x.X.Equals(dataPoint.X) AndAlso x.Y.Equals(dataPoint.Y))

    End Sub
    Sub sNyq_MouseDown(sender As Object, e As OxyMouseDownEventArgs)
        Dim series = PlotViewNyquist.ActualModel.GetSeriesFromPoint(e.Position)
        If series Is Nothing Then Return

        Dim nearestPoint = series.GetNearestPoint(e.Position, False)
        If nearestPoint Is Nothing Then Return

        DataGridViewImpedance.ClearSelection()
        DataGridViewImpedance.Rows(nearestPoint.Index).Selected = True
        DataGridViewImpedance.Select()

        Dim dataPoint = nearestPoint.DataPoint
        'Dim scatterPoint = series.
        'FirstOrDefault(Function(x) x.X.Equals(dataPoint.X) AndAlso x.Y.Equals(dataPoint.Y))

    End Sub


    '------------------------------------ user interaction on controls ----------------------------------------------'

    Private Sub Filepathtext_TextChanged(sender As Object, e As EventArgs) Handles Filepathtext.TextChanged

        'Check file, if exist => green => load
        If IO.File.Exists(Filepathtext.Text) Then
            Filepathtext.BackColor = Color.Honeydew
            Try : LoadFile() : Catch ex As Exception : MsgBox(ex.Message) : loadingscreen.Hide() : End Try
        Else
            Filepathtext.BackColor = Color.PaleVioletRed
        End If
    End Sub
    Private Sub ButtonLoad_Click(sender As Object, e As EventArgs) Handles ButtonLoad.Click
        Dim openFileDialog As New OpenFileDialog
        openFileDialog.Filter = "All Supported Files|*.txt;*.csv;*.tsv;*.tab;*.mat;*.irf;*.bin|Text and CSV Files (*.txt, *.csv, *.tsv, *.tab, *.irf)|*.txt;*.csv;*.tsv;*.tab| f and Z stored in MATLAB matrix (*.mat)|*.mat|All Files (*.*)|*.*"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Filepathtext.Text = openFileDialog.FileName
            'Filepathtext.SelectionStart = 

            Filepathtext.Select(Len(Filepathtext.Text) - 1, 0)

        End If
    End Sub
    Private Sub reverseNyqChecked_CheckedChanged(sender As Object, e As EventArgs) Handles CheckboxReverseNyq.CheckedChanged ', CheckBoxPlotrLKK.CheckedChanged
        If memoryData Is Nothing OrElse memoryData.Count = 0 Then Exit Sub 'no memory data (no file is loaded)
        PlotNyquist(memoryData(1), memoryData(2), CheckBoxPlotrLKK.Checked) 'Update Nyquist plot
    End Sub
    Private Sub CheckBoxPlotrLKK_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxPlotrLKK.CheckedChanged
        If memoryData Is Nothing OrElse memoryData.Count = 0 Then Exit Sub 'no memory data (no file is loaded)
        PlotNyquist(memoryData(1), memoryData(2), CheckBoxPlotrLKK.Checked) 'Update Nyquist plot
        PlotBode(memoryData(0), memoryData(1), memoryData(2), CheckBoxPlotrLKK.Checked) 'update bode
    End Sub

    Private Sub DataGridViewImpedance_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewImpedance.CellEndEdit

        If e.ColumnIndex > 2 Then Exit Sub

        If memoryData(e.ColumnIndex)(e.RowIndex) = DataGridViewImpedance(e.ColumnIndex, e.RowIndex).Value Then Exit Sub 'no change
        Try
            memoryData(e.ColumnIndex)(e.RowIndex) = DataGridViewImpedance(e.ColumnIndex, e.RowIndex).Value
            updateNyquistandAllFromMemory()
        Catch Ex As Exception
            DataGridViewImpedance(e.ColumnIndex, e.RowIndex).Value = memoryData(e.ColumnIndex)(e.RowIndex)
            MsgBox(Ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub

    Dim _datagridisbeingfilled As Boolean = False
    Private Sub DataGridViewImpedance_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles DataGridViewImpedance.RowsRemoved
        If _datagridisbeingfilled Then Exit Sub

        memoryData(0).RemoveAt(e.RowIndex)
        memoryData(1).RemoveAt(e.RowIndex)
        memoryData(2).RemoveAt(e.RowIndex)
        updateNyquistandAllFromMemory()
        UpdateInfo()

    End Sub

    Dim rlkk_drt_settings(3) As Double 'fmin, fmax, 
    Private Sub rLKKSettChanged_TextChanged(sender As Object, e As EventArgs) Handles fminTextBox.TextChanged, fmaxTextBox.TextChanged, nDRTTextBox.TextChanged, lambdaTextBox.TextChanged,
        ComboBoxWeight.SelectedIndexChanged, CheckBoxR0.CheckedChanged, CheckBoxRinf.CheckedChanged

        'Any field from rLKK changed?
        If IsNumeric(sender.text) Or TypeOf sender Is CheckBox Or TypeOf sender Is ComboBox Then
            sender.BackColor = Color.Honeydew


            If sender Is fminTextBox Then
                rlkk_drt_settings(0) = Double.Parse(sender.Text)
            ElseIf sender Is fmaxTextBox Then
                rlkk_drt_settings(1) = Double.Parse(sender.Text)
            ElseIf sender Is nDRTTextBox Then
                rlkk_drt_settings(2) = Double.Parse(sender.Text)
            ElseIf sender Is lambdaTextBox Then
                rlkk_drt_settings(3) = Double.Parse(sender.Text)
            End If

            updateNyquistandAllFromMemory()
            UpdateInfo()
        Else
            sender.backColor = Color.PaleVioletRed
            Beep()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click 'Invert Im button, rename changeme later
        If memoryData Is Nothing OrElse memoryData.Count = 0 OrElse memoryData(2).Count = 0 Then Exit Sub

        For k = 0 To memoryData(2).Count - 1
            memoryData(2)(k) *= -1
        Next


        PopulateDataGridView(memoryData(0), memoryData(1), memoryData(2), rLKK_Z.ToArray(), rLKK_Zdev.ToArray)
        updateNyquistandAllFromMemory()
        UpdateInfo()
    End Sub

    Private Sub CheckBoxLogScale_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxLogScale.CheckedChanged
        If memoryData Is Nothing OrElse memoryData.Count = 0 Then Exit Sub 'no memory data (no file is loaded)
        PlotBode(memoryData(0), memoryData(1), memoryData(2), CheckBoxPlotrLKK.Checked) 'Update Nyquist plot
        PlotDeviationFromMemory()
    End Sub

    Private Sub CheckBox2perth_CheckedChanged(sender As Object, e As EventArgs)
        If CheckBox1perth.Checked And CheckBox2perth.Checked Then CheckBox1perth.Checked = False 'Mutex
    End Sub

    Private Sub CheckBox1perth_CheckedChanged(sender As Object, e As EventArgs)
        If CheckBox2perth.Checked And CheckBox1perth.Checked Then CheckBox2perth.Checked = False 'Mutex
    End Sub

    Dim _disableaccesstoactiondevchange = False
    Private Sub CheckBoxDev_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxDevReal.CheckedChanged, CheckBoxDevMag.CheckedChanged, CheckBoxDevImg.CheckedChanged,
            CheckBox1perth.CheckedChanged, CheckBox2perth.CheckedChanged, ComboBoxRefDev.SelectedIndexChanged
        'RadioButtonPercent.CheckedChanged, RadioButtonPpm.CheckedChanged, RadioButton1.CheckedChanged

        If _disableaccesstoactiondevchange Then Exit Sub 'semaphore

        _disableaccesstoactiondevchange = True

        If CheckBox2perth.Checked And CheckBox1perth.Checked Then
            CheckBox2perth.Checked = False
            CheckBox1perth.Checked = False
            sender.Checked = True
        End If

        _disableaccesstoactiondevchange = False
        PlotDeviationFromMemory()

    End Sub

    Private Sub RadioButtonPercent_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonPercent.CheckedChanged, RadioButtonPpm.CheckedChanged, RadioButton1.CheckedChanged
        If sender.Checked = False Then Exit Sub 'only allow one call
        PlotDeviationFromMemory()

    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        'MsgBox("Regularized Linear Kramers Kronig" & vbCrLf & "Version: " & Application.ProductVersion & vbCrLf & "Author: Ahmed Yahia Kallel", MsgBoxStyle.Information, "About")
        about.Show()
    End Sub


    Dim prevSelIdx = 0
    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedIndex = 3 Then
            TabPage4.Controls.Add(DataGridViewImpedance)
            DataGridViewImpedance.Dock = DockStyle.Fill

            DataGridViewImpedance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridViewImpedance.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders
        Else
            Panel1.Controls.Add(DataGridViewImpedance)
            DataGridViewImpedance.Dock = DockStyle.Bottom
            'DataGridViewImpedance.AutoResizeColumn(1, DataGridViewAutoSizeColumnMode.None)
            DataGridViewImpedance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            '
            DataGridViewImpedance.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        End If

        prevSelIdx = TabControl1.SelectedIndex
    End Sub

    Private Sub DataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataToolStripMenuItem.Click
        SaveFileDialog1.Filter = "CSV file (tab) |*.csv"
        SaveFileDialog1.FileName = ""
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            ExportDataGridViewToTextFile(DataGridViewImpedance, SaveFileDialog1.FileName)
        End If

    End Sub



    Sub savePlotModelAsImg(PlotViewi As PlotView)
        SaveFileDialog1.Filter = " PNG |*.png| PDF|*.pdf|SVG |*.svg"
        SaveFileDialog1.FileName = ""
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            'ExportDataGridViewToTextFile(DataGridViewImpedance, SaveFileDialog1.FileName)
            Select Case LCase(Strings.Right(SaveFileDialog1.FileName, 3))
                Case "svg"
                    SavePlotAsSvg(PlotViewi.Model, SaveFileDialog1.FileName)
                Case "png"
                    SavePlotAsPng(PlotViewi.Model, SaveFileDialog1.FileName)
                Case "pdf"
                    SavePlotAsPdf(PlotViewi.Model, SaveFileDialog1.FileName)
            End Select

        End If
    End Sub

    Private Sub NyquistCurveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NyquistCurveToolStripMenuItem.Click
        savePlotModelAsImg(PlotViewNyquist)
    End Sub

    Private Sub DeviationsPlotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeviationsPlotToolStripMenuItem.Click
        savePlotModelAsImg(PlotViewDev)
    End Sub

    Private Sub RLKKPlotToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RLKKPlotToolStripMenuItem.Click
        savePlotModelAsImg(PlotViewrLKK)
    End Sub

    Private Sub DataToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DataToolStripMenuItem1.Click
        ButtonLoad.PerformClick()
    End Sub

    Private Sub RLKKParametersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RLKKParametersToolStripMenuItem.Click
        OpenFileDialog2.Filter = "rLKK parameters file|*.rlkkparam"
        If OpenFileDialog2.ShowDialog() = DialogResult.OK Then
            Dim binReader As New IO.BinaryReader(New IO.FileStream(OpenFileDialog2.FileName, FileMode.Open))
            Dim version_ = binReader.ReadString()
            fminTextBox.Text = binReader.ReadString()
            fmaxTextBox.Text = binReader.ReadString()
            nDRTTextBox.Text = binReader.ReadString()
            lambdaTextBox.Text = binReader.ReadString()
            ComboBoxWeight.SelectedIndex = binReader.ReadInt32()
            CheckBoxR0.Checked = binReader.ReadBoolean()
            CheckBoxRinf.Checked = binReader.ReadBoolean()
            binReader.Close()
        End If
    End Sub

    Private Sub RLKKParametersToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RLKKParametersToolStripMenuItem1.Click
        SaveFileDialog2.Filter = "rLKK parameters file|*.rlkkparam"
        If SaveFileDialog2.ShowDialog() = DialogResult.OK Then
            Dim binWriter As New IO.BinaryWriter(New IO.FileStream(SaveFileDialog2.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            binWriter.Write("RLKK_V1.0.0.0")
            binWriter.Write(fminTextBox.Text)
            binWriter.Write(fmaxTextBox.Text)
            binWriter.Write(nDRTTextBox.Text)
            binWriter.Write(lambdaTextBox.Text)
            binWriter.Write(ComboBoxWeight.SelectedIndex)
            binWriter.Write(CheckBoxR0.Checked)
            binWriter.Write(CheckBoxRinf.Checked)
            binWriter.Flush()
            binWriter.Close()
        End If
    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy ' Indicates that the data can be dropped
        End If
    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim files As String() = DirectCast(e.Data.GetData(DataFormats.FileDrop), String())
        Filepathtext.Text = files(0)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Try
            Dim res = InputBox("Selecting deviations larger than", "Threshold selection", rLKK_Zdev.Max / 2)

            If res = "" Then Exit Sub 'no input

            DataGridViewImpedance.ClearSelection()

            Dim rows_to_remove As New List(Of DataGridViewRow)
            Dim rows_idx_to_remove As New List(Of Integer)

            For Each row As DataGridViewRow In DataGridViewImpedance.Rows
                ' Check if the row is not a new row and has cells
                If Not row.IsNewRow AndAlso row.Cells.Count >= 5 Then
                    If Math.Abs(row.Cells(5).Value) > res Then
                        row.Selected = True
                        rows_to_remove.Add(row)
                        rows_idx_to_remove.Add(row.Index)
                    End If
                End If
            Next

            If MsgBox(String.Format("Selected {0} rows, delete?", rows_to_remove.Count), MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                _datagridisbeingfilled = True
                For Each row As DataGridViewRow In rows_to_remove
                    DataGridViewImpedance.Rows.Remove(row)
                Next
                _datagridisbeingfilled = False

                For i = rows_idx_to_remove.Count - 1 To 0 Step -1
                    memoryData(0).RemoveAt(rows_idx_to_remove(i))
                    memoryData(1).RemoveAt(rows_idx_to_remove(i))
                    memoryData(2).RemoveAt(rows_idx_to_remove(i))
                Next

                updateNyquistandAllFromMemory()


            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

    End Sub

    Private Sub DataGridViewImpedance_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewImpedance.CellContentClick

    End Sub

    Private Sub DataGridViewImpedance_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewImpedance.SelectionChanged
        nyquistSeriesSelect.Points.Clear()
        devSeriesSelect.points.clear()
        For Each sel As DataGridViewRow In DataGridViewImpedance.SelectedRows()

            nyquistSeriesSelect.Points.Add(New ScatterPoint(memoryData(1)(sel.Index), memoryData(2)(sel.Index)))
            devSeriesSelect.Points.Add(New ScatterPoint(memoryData(0)(sel.Index), rLKK_Zdev(sel.Index)))


        Next
        PlotViewNyquist.InvalidatePlot(True)
        PlotViewDev.InvalidatePlot(True)
    End Sub


    'Lenient 
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim _fmin = Math.Floor(Math.Log10(memoryData(0).Min)) - 1
        Dim _fmax = Math.Ceiling(Math.Log10(memoryData(0).Max)) + 1
        fminTextBox.Text = "1e" & _fmin
        fmaxTextBox.Text = "1e" & _fmax
        nDRTTextBox.Text = 6 * (_fmax - _fmin)
        lambdaTextBox.Text = 0.001
        ComboBoxWeight.SelectedIndex = 0
        CheckBoxR0.Checked = True
        CheckBoxRinf.Checked = True


    End Sub

    'Aggressive
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        fminTextBox.Text = "1e-8"
        fmaxTextBox.Text = "1e+8"
        nDRTTextBox.Text = 1000
        lambdaTextBox.Text = "1e10"
        ComboBoxWeight.SelectedIndex = 0
        CheckBoxR0.Checked = False
        CheckBoxRinf.Checked = False

    End Sub
End Class
