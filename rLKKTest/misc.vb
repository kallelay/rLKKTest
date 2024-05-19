Imports MathNet.Numerics.LinearAlgebra
Imports MathNet.Numerics.Statistics

Public Module misc
    Function MatrixToVector(matrix As Matrix(Of Double)) As Vector(Of Double)
        ' Flatten the matrix into a single vector
        Dim vector As Vector(Of Double) = MathNet.Numerics.LinearAlgebra.Vector(Of Double).Build.Dense(matrix.RowCount * matrix.ColumnCount)

        Dim index As Integer = 0
        For i As Integer = 0 To matrix.RowCount - 1
            For j As Integer = 0 To matrix.ColumnCount - 1
                vector(index) = matrix(i, j)
                index += 1
            Next
        Next

        Return vector
    End Function

    Public Function InterpolateColor(color1 As Color, color2 As Color, factor As Double) As Color
        Dim r As Int16 = (color1.R * 1.0 + (color2.R * 1.0 - color1.R * 1.0) * factor)
        Dim g As Int16 = (color1.G * 1.0 + (color2.G * 1.0 - color1.G * 1.0) * factor)
        Dim b As Int16 = (color1.B * 1.0 + (color2.B * 1.0 - color1.B * 1.0) * factor)
        r = Math.Min(Math.Max(0, r), 255)
        b = Math.Min(Math.Max(0, b), 255)
        g = Math.Min(Math.Max(0, g), 255)
        Return Color.FromArgb(r, g, b)
    End Function

End Module
