Imports MathNet.Numerics.LinearAlgebra

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

End Module
