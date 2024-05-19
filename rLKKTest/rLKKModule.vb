Imports MathNet.Numerics
Imports System.Math
Imports MathNet.Numerics.LinearAlgebra
Imports System.Net
Imports MathNet.Numerics.LinearAlgebra.Factorization
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Module rLKKModule

    Public rLKK_x() As Double
    Public rLKK_f() As Double
    Public rLKK_Z As Vector(Of System.Numerics.Complex)
    Public rLKK_Zdev As Vector(Of Double)



    Public Sub rLKK(frequencies As Double(), realParts As Double(), imaginaryParts As Double(), rLKKparams() As Double, weightingMethod As Integer)



        'This is a wrapper from .NET expression to mathnet

        Dim Z As Vector(Of System.Numerics.Complex) = Vector(Of Double).Build.Dense(realParts).ToComplex.Add(Vector(Of Double).Build.Dense(imaginaryParts).ToComplex.Multiply(System.Numerics.Complex.ImaginaryOne))

        Dim f As Vector(Of Double) = Vector(Of Double).Build.Dense(frequencies)



        ' Build fdrt (fx) vector
        Dim fx0 As Double() = MathNet.Numerics.Generate.LogSpaced(rLKKparams(2), Log10(rLKKparams(0)), Log10(rLKKparams(1)))

        If Form1.CheckBoxR0.Checked Then
            ReDim Preserve fx0(fx0.Count)
            fx0(fx0.Count - 1) = Double.PositiveInfinity
        End If

        'If Form1.CheckBoxRinf.Checked Then
        'ReDim Preserve fx0(fx0.Count)
        'fx0(fx0.Count - 1) = 0
        ' End If
        Dim fx As Vector(Of Double) = Vector(Of Double).Build.DenseOfArray(fx0)
        rLKK_f = fx0.Clone()
        fx0 = Nothing 'remove fx0


        ' %% weight vectors (TODO)



        '  If TypeOf lambd Is Double Then
        '  lambd = DirectCast(lambd, Double) * Vector(Of Double).Build.Dense(Z.Count, 1.0)
        ' ElseIf TypeOf lambd Is Integer Then
        '   lambd = DirectCast(lambd, Integer) * Vector(Of Double).Build.Dense(Z.Count, 1.0)
        '  ElseIf TypeOf lambd Is Vector(Of Double) AndAlso lambd.Count = 1 Then
        '   lambd = Vector(Of Double).Build.Dense(Z.Count, lambd(0))
        '   End If

        ' lambd = lambd.ToColumnMatrix()

        '%% DRT function


        '%% Construct A matrix (4MxN) as in Ax = b
        Dim AC As Matrix(Of System.Numerics.Complex) = Z_ww0(1.0, f, fx)
        Dim A As Matrix(Of Double) = AC.Real()
        A = A.Stack(AC.Imaginary())

        '%% Construct dA matrix
        'Dim dA As Matrix(Of Double) = Matrix(Of Double).Build.Dense(A.RowCount, A.ColumnCount)

        Dim dA
        If weightingMethod = 0 Then 'unit weight
            dA = Matrix(Of Double).Build.DenseDiagonal(A.RowCount, A.ColumnCount, rLKKparams(3))
        Else

            'Weight using 1/|Z|
            dA = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.CreateDiagonal(A.RowCount, A.ColumnCount, Function(i)
                                                                                                                 If i < Z.Count AndAlso Z(i) <> 0.0 Then
                                                                                                                     Return rLKKparams(3) / Z(i).Magnitude
                                                                                                                 Else
                                                                                                                     Return 0.0
                                                                                                                 End If
                                                                                                             End Function)

        End If

        'dA.SetSubMatrix(0, A.RowCount \ 2, 0, A.ColumnCount, Matrix(Of Double).Build.DiagonalOfDiagonalVector(lambd))
        'A.SetSubMatrix(A.RowCount \ 2, A.RowCount \ 2, 0, A.ColumnCount, Matrix(Of Double).Build.DiagonalOfDiagonalVector(lambd))


        'add weights
        A = A.Stack(dA)
        ' A = A.Transpose()

        '%% Construct vector b (4Mx1)
        Dim b As Vector(Of Double) = Z.Real()

        If Form1.CheckBoxRinf.Checked Then b = b.Subtract(Z.Real().Minimum())

        b = AppendVectors(b, Z.Imaginary)
        b = AppendVectors(b, Vector(Of Double).Build.Dense(Z.Count * 2, 0.0))

        Dim x As Vector(Of Double)
        Try

            x = A.PseudoInverse * b


        Catch ex1 As MathNet.Numerics.NonConvergenceException
            Try
                '%% Use pseudo-inverse instead of inverse due to ill-posed problem
                Dim ASVD = A.Svd

                ' Compute the pseudo-inverse of Σ (S)
                Dim S_pseudoInverse As Matrix(Of Double) = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.CreateDiagonal(A.ColumnCount, A.RowCount, Function(i)
                                                                                                                                                           If i < ASVD.S.Count AndAlso ASVD.S(i) <> 0.0 Then
                                                                                                                                                               Return 1.0 / ASVD.S(i)
                                                                                                                                                           Else
                                                                                                                                                               Return 0.0
                                                                                                                                                           End If
                                                                                                                                                       End Function)

                ' Compute the pseudo-inverse of A using the pseudo-inverse of Σ (S), U, and V*
                Dim A_pseudoInverse As Matrix(Of Double) = ASVD.VT.TransposeThisAndMultiply(S_pseudoInverse).Multiply(ASVD.U.Transpose())
                'Thank you chatGPT for the wrong formula, corrected from: https://math.stackexchange.com/questions/1939962/singular-value-decomposition-and-inverse-of-square-matrix



                'Dim pinvA As Matrix(Of Double) = ASVD.U * ASVD.S.PointwiseMultiply(ASVD.S.PointwiseReciprocal) * ASVD.VT.Transpose()


                x = A_pseudoInverse * b
            Catch ex2 As Exception
                ' Calculate (A^T * A)

                Dim ATA As Matrix(Of Double) = A.Transpose().Multiply(A)

                ' Calculate (A^T * A)^-1
                Dim ATAInverse As Matrix(Of Double) = ATA.Inverse()

                ' Calculate A^T * b
                Dim ATb As Vector(Of Double) = A.Transpose().Multiply(b)

                x = ATAInverse.Multiply(ATb)

            End Try
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try

        '%% Get the results, remove 2M, get real and imaginary, and return complex vector
        Dim bb As Vector(Of Double) = A.Multiply(x)
        bb = bb.SubVector(0, bb.Count \ 2)

        'If Form1.CheckBoxRinf.Checked Then bb.Add(Z.Real().Minimum())

        rLKK_x = x.ToArray()
        rLKK_Z = bb.SubVector(0, bb.Count \ 2).ToComplex() + bb.SubVector(bb.Count \ 2, bb.Count \ 2).ToComplex().Multiply(System.Numerics.Complex.ImaginaryOne)
        If Form1.CheckBoxRinf.Checked Then rLKK_Z = rLKK_Z.Add(Z.Real().Minimum())
        rLKK_Zdev = ((rLKK_Z.PointwiseAbs() - Z.PointwiseAbs()) / rLKK_Z.PointwiseAbs() * 100).Real
        '  Return rLKK_Z
    End Sub
    Function Z_ww0(R As Double, w As Vector(Of Double), w0 As Vector(Of Double)) As Matrix(Of System.Numerics.Complex)


        Dim R_Matrix = Matrix(Of System.Numerics.Complex).Build.Dense(w.Count, w0.Count, R)



        Dim F_Matrix = w.ToColumnMatrix()
        For k = 1 To w0.Count - 1
            F_Matrix = F_Matrix.Append(w.ToColumnMatrix())
        Next


        Dim F0_Matrix = w0.ToRowMatrix()
        For k = 1 To w.Count - 1
            F0_Matrix = F0_Matrix.Stack(w0.ToRowMatrix())
        Next



        Dim numerator = (F_Matrix.PointwiseDivide(F0_Matrix).ToComplex()).Multiply(System.Numerics.Complex.ImaginaryOne).Add(1)
        Return R_Matrix.PointwiseDivide(numerator)
    End Function

    Function AppendVectors(vector1 As Vector(Of Double), vector2 As Vector(Of Double)) As Vector(Of Double)
        ' Concatenate two vectors and return the result
        Dim concatenatedArray As Double() = vector1.ToArray().Concat(vector2.ToArray()).ToArray()
        Return Vector(Of Double).Build.DenseOfArray(concatenatedArray)
    End Function
End Module
