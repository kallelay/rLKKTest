Imports System.IO
Imports MathNet.Numerics.Data.Matlab
Imports MathNet.Numerics.LinearAlgebra
Imports OxyPlot
Imports OxyPlot.Axes
Imports OxyPlot.Legends
Imports OxyPlot.Series
Imports OxyPlot.WindowsForms

Public Class Form1
    Private Sub ButtonLoad_Click(sender As Object, e As EventArgs) Handles ButtonLoad.Click
        Dim openFileDialog As New OpenFileDialog
        openFileDialog.Filter = "All Supported Files|*.txt;*.csv;*.tsv;*.tab;*.mat|Text and CSV Files (*.txt, *.csv, *.tsv, *.tab)|*.txt;*.csv;*.tsv;*.tab| f and Z stored in MATLAB matrix (*.mat)|*.mat|All Files (*.*)|*.*"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Filepathtext.Text = openFileDialog.FileName
            'Filepathtext.SelectionStart = 

            Filepathtext.Select(Len(Filepathtext.Text) - 1, 0)

        End If
    End Sub



    Enum LoadModesEnum
        modecsv = 0
        modeagilent = 1


        modebin = 2
        modematlab = 3

    End Enum

    Dim loadMode As LoadModesEnum = LoadModesEnum.modecsv
    Dim traceAIsDetected As Boolean = False


    Dim memoryData As New List(Of List(Of Double))

    Private Sub updateStatus(status$, progressRatio!)

        progressRatio! /= 7
        loadingscreen.status.Text = status
        loadingscreen.progress.Text = Int(progressRatio * 100) & "%"
        Application.DoEvents()
    End Sub



    '------------------------------------ internal and I/O ---------------------------------------------------'
    Private Sub LoadFile()


        loadingscreen.Show(Me)
        loadingscreen.Location = Me.Location + New Point(Me.Width / 2 - loadingscreen.Width / 2, Me.Height / 2 - loadingscreen.Height / 2)

        updateStatus("Starting", 0)
        Application.DoEvents()

        'local variables 
        Dim frequencies As New List(Of Double)
        Dim realParts As New List(Of Double)
        Dim imaginaryParts As New List(Of Double)



        updateStatus("Loading File", 1)


        loadMode = LoadModesEnum.modecsv 'expecting csv or tsv by default

        traceAIsDetected = False 'reset detected trace A for agilent mode

        Dim filePath = Filepathtext.Text 'get filepath from text file (done it this way for later expansions)


        If Strings.Right(filePath, 3).ToLower = "mat" Then GoTo load_matlab

        Application.DoEvents()
        '------------------- normal text file I/O section (default) -------------------------------------------'

        Dim strAll As String = File.ReadAllText(filePath) 'load all text, once

        If InStr(strAll, "4294A") > 0 Then loadMode = LoadModesEnum.modeagilent 'use agilent mode

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



        Dim lines As String() = File.ReadAllLines(filePath)



        Dim curLine = 0
        Dim steps = Int(lines.Count / 10)
        For Each line As String In lines
            curLine += 1

            'progress
            If curLine Mod steps = 0 Then
                updateStatus(String.Format("Loading File ({0:0.0} %)", curLine / steps * 100), 1)
            End If


            line = Trim(line)


            While line IsNot Nothing AndAlso line.IndexOf(valSep & valSep) <> -1
                line = Replace(line, valSep & valSep, valSep)
            End While


            If InStr(line, """TRACE: A""") > 0 Then traceAIsDetected = True
            If line Is Nothing Then Continue For
            If traceAIsDetected And InStr(line, """TRACE: B""") > 0 Then Exit For 'exit if trace b


            Dim parts As String() = line.Split(valSep)
            If parts.Length >= 3 Then

                If Not IsNumeric(parts(0)) Then
                    'TODO check decimal separator first
                    If InStr(parts(0), "Frequency") > 0 Then
                        Continue For
                    End If
                End If

                Dim frequency As Double = Double.Parse(parts(0))
                Dim realPart As Double = Double.Parse(parts(1))
                Dim imaginaryPart As Double = Double.Parse(parts(2))

                frequencies.Add(frequency)
                realParts.Add(realPart)
                imaginaryParts.Add(imaginaryPart)
            End If
        Next

        GoTo finish_loading

        '------------------- MATLAB file I/O section (default) -------------------------------------------'
load_matlab:


        Dim freqs = MatlabReader.Read(Of Double)(Filepathtext.Text, "f")
        Dim Zall = MatlabReader.Read(Of System.Numerics.Complex)(Filepathtext.Text, "Z")

        frequencies.AddRange(MatrixToVector(freqs).ToArray)
        realParts.AddRange(MatrixToVector(Zall.Real).ToArray)
        imaginaryParts.AddRange(MatrixToVector(Zall.Imaginary).ToArray)

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


    Dim oldFileName$ = "" 'mem file name
    Private Sub UpdateInfo(Optional filename$ = "")
        If memoryData.Count = 0 Then Exit Sub

        If filename = "" Then filename = oldFileName
        oldFileName = filename

        fileinfLabel.Text = "Filename: " & Split(filename, "\").Last & Space(5) & String.Format("Npts: {0}", memoryData(0).Count)
        moreinfoLabel.Text = "Freqs: " & memoryData(0).Min() & "Hz - " & memoryData(0).Max() & "Hz"

        rLKKparamsInfoLabel.Text = String.Format("DRT freqs: {0: 0.00e+00} Hz - {1: 0.00e+00} Hz ({2} points)" & vbCrLf & "lambda: {3:0.00e+00}", rlkk_drt_settings(0), rlkk_drt_settings(1),
                                                 rlkk_drt_settings(2), rlkk_drt_settings(3))

    End Sub

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
    Private Function InterpolateColor(color1 As Color, color2 As Color, factor As Double) As Color
        Dim r As Int16 = (color1.R * 1.0 + (color2.R * 1.0 - color1.R * 1.0) * factor)
        Dim g As Int16 = (color1.G * 1.0 + (color2.G * 1.0 - color1.G * 1.0) * factor)
        Dim b As Int16 = (color1.B * 1.0 + (color2.B * 1.0 - color1.B * 1.0) * factor)
        r = Math.Min(Math.Max(0, r), 255)
        b = Math.Min(Math.Max(0, b), 255)
        g = Math.Min(Math.Max(0, g), 255)
        Return Color.FromArgb(r, g, b)
    End Function











    '----------------------------------- plotting --------------------------------------------'
    Private Sub PlotNyquist(realParts As List(Of Double), imaginaryParts As List(Of Double), Optional plotrLKK As Boolean = False)
        Dim nyquistPlotModel As New PlotModel With {
            .Title = "Nyquist Plot",
            .EdgeRenderingMode = EdgeRenderingMode.Adaptive,
            .PlotType = PlotType.Cartesian
        }



        Dim nyquistSeries As New LineSeries With {
            .Title = "Measurements",
            .MarkerType = MarkerType.Circle
        }

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

        If CheckBoxDevMag.Checked Then devPlotModel.Series.Add(magnitudeSeries)
        If CheckBoxDevReal.Checked Then devPlotModel.Series.Add(realSeries)
        If CheckBoxDevImg.Checked Then devPlotModel.Series.Add(imagSeries)

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

    End Sub

    'TODO: binary files
    'TODO: decimal separator (,)

    'TODO: drag & drop

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fileinfLabel.Text = ""
        moreinfoLabel.Text = ""
        rLKKparamsInfoLabel.Text = ""

        ComboBoxRefDev.SelectedIndex = 1
        ComboBoxWeight.SelectedIndex = 0
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

    End Sub



    '------------------------------------ user interaction on controls ----------------------------------------------'
    Private Sub Filepathtext_TextChanged(sender As Object, e As EventArgs) Handles Filepathtext.TextChanged

        'Check file, if exist => green => load
        If IO.File.Exists(Filepathtext.Text) Then
            Filepathtext.BackColor = Color.Honeydew
            LoadFile()
        Else
            Filepathtext.BackColor = Color.PaleVioletRed
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

    Private Sub DataGridViewImpedance_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewImpedance.CellContentClick

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

    'TODO: multicursor

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

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
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
        If CheckBox1perth.Checked And CheckBox2perth.Checked Then CheckBox1perth.Checked = False
    End Sub

    Private Sub CheckBox1perth_CheckedChanged(sender As Object, e As EventArgs)
        If CheckBox2perth.Checked And CheckBox1perth.Checked Then CheckBox2perth.Checked = False
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
        MsgBox("Regularized Linear Kramers Kronig" & vbCrLf & "Version: " & Application.ProductVersion & vbCrLf & "Author: Ahmed Yahia Kallel", MsgBoxStyle.Information, "About")
    End Sub

    Private Sub TabPage4_Click(sender As Object, e As EventArgs) Handles TabPage4.Click

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

    Private Sub ExportDataGridViewToTextFile(dataGridView As DataGridView, filePath As String)

        Dim sep = CStr("1.23")(1)
        'If sep = "." Then sep = "," Else sep = ";"
        sep = ";"
        Try
            Using writer As New StreamWriter(filePath)
                ' Write the headers
                For i As Integer = 0 To dataGridView.Columns.Count - 1
                    writer.Write(dataGridView.Columns(i).HeaderText)
                    If i < dataGridView.Columns.Count - 1 Then
                        writer.Write(sep)
                        ' writer.Write(vbTab)
                    End If
                Next
                writer.WriteLine()

                ' Write the data rows
                For Each row As DataGridViewRow In dataGridView.Rows
                    If Not row.IsNewRow Then
                        For i As Integer = 0 To dataGridView.Columns.Count - 1
                            writer.Write(row.Cells(i).Value.ToString())
                            If i < dataGridView.Columns.Count - 1 Then
                                '        writer.Write(",")
                                writer.Write(sep)
                            End If
                        Next
                        writer.WriteLine()
                    End If
                Next
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Error")
        End Try
    End Sub

    Private Sub SavePlotAsSvg(plotModel As PlotModel, filePath As String)
        ' Define the width and height for the SVG
        Dim width As Double = plotModel.Width
        Dim height As Double = plotModel.Height

        ' Create an SVG exporter
        Dim exporter As New OxyPlot.SvgExporter With {
            .Width = width,
            .Height = height
        }

        ' Export the plot to an SVG file
        Using stream As New IO.FileStream(filePath, IO.FileMode.Create)
            exporter.Export(plotModel, stream)
        End Using

    End Sub
    Private Sub SavePlotAsPdf(plotModel As PlotModel, filePath As String)
        ' Define the width and height for the pdf
        Dim width As Double = plotModel.Width * 0.75
        Dim height As Double = plotModel.Height * 0.75

        ' Create a PNG exporter
        Dim exporter As New PdfExporter With {
            .Width = width,
            .Height = height
        }

        ' Export the plot to a PNG file
        Using stream As New IO.FileStream(filePath, IO.FileMode.Create)
            exporter.Export(plotModel, stream)
        End Using

    End Sub
    Private Sub SavePlotAsPng(plotModel As PlotModel, filePath As String)
        ' Define the width and height for the PNG
        Dim width As Double = plotModel.Width
        Dim height As Double = plotModel.Height

        ' Create a PNG exporter
        Dim exporter As New PngExporter With {
            .Width = width,
            .Height = height
        }

        ' Export the plot to a PNG file
        Using stream As New IO.FileStream(filePath, IO.FileMode.Create)
            exporter.Export(plotModel, stream)
        End Using

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


End Class
