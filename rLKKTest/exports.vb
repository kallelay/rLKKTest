Imports System.IO

Public Module exports

    Sub SavePlotAsSvg(plotModel As PlotModel, filePath As String)
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
    Sub SavePlotAsPdf(plotModel As PlotModel, filePath As String)
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
    Sub SavePlotAsPng(plotModel As PlotModel, filePath As String)
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


    Sub ExportDataGridViewToTextFile(dataGridView As DataGridView, filePath As String)

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

End Module
