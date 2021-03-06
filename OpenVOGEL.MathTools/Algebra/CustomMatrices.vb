﻿'Open VOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2018 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

'This program Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program.  If Not, see < http:  //www.gnu.org/licenses/>.

Imports DotNumerics
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace Algebra.CustomMatrices

    Public Structure Matrix

        Public Sub New(ByVal i As Integer, ByVal j As Integer)
            ReDim FElements(i - 1, j - 1)
            Me.FRows = i
            Me.FColumns = j
            FTransponse = False
        End Sub

        Private FElements(,) As Double

        Public Property Element(ByVal i As Integer, ByVal j As Integer) As Double
            Get
                If Not FTransponse Then
                    Return FElements(i - 1, j - 1)
                Else
                    Return FElements(j - 1, i - 1)
                End If
            End Get
            Set(ByVal value As Double)
                If Not FTransponse Then
                    FElements(i - 1, j - 1) = value
                Else
                    FElements(j - 1, i - 1) = value
                End If
            End Set
        End Property

        Private FRows As Integer

        Public ReadOnly Property Rows As Integer
            Get
                If Not FTransponse Then
                    Return FRows
                Else
                    Return FColumns
                End If
            End Get
        End Property

        Private FColumns As Integer

        Public ReadOnly Property Columns As Integer
            Get
                If Not FTransponse Then
                    Return FColumns
                Else
                    Return FRows
                End If
            End Get
        End Property

        Private FTransponse As Boolean

        Public Sub Transponse()
            FTransponse = Not FTransponse
        End Sub

        Public Sub Clear()
            For i = 1 To Me.Rows
                For j = 1 To Me.Columns
                    Element(i, j) = 0
                Next
            Next
        End Sub

        Public Sub Assign(ByVal Matrix As Matrix)
            If Matrix.Rows = Me.Rows And Matrix.Columns = Me.Columns Then
                For i = 1 To Me.Rows
                    For j = 1 To Me.Columns
                        Element(i, j) = Matrix.Element(i, j)
                    Next
                Next
            End If
        End Sub

#Region " Matrix operators "

        'Sum:
        Public Shared Operator +(ByVal M1 As Matrix, ByVal M2 As Matrix) As Matrix

            If M1.Rows = M2.Rows And M1.Columns = M2.Columns Then
                Dim Sum As New Matrix(M1.Rows, M1.Columns)

                For i = 1 To Sum.Rows
                    For j = 1 To Sum.Columns

                        Sum.Element(i, j) = M1.Element(i, j) + M2.Element(i, j)

                    Next
                Next

                Return Sum

            Else
                Return New Matrix(0, 0)
            End If

        End Operator

        'Scalar multiplication:
        Public Shared Operator *(ByVal Escalar As Double, ByVal M As Matrix) As Matrix
            Dim Product As New Matrix(M.Rows, M.Columns)

            For i = 1 To Product.Rows
                For j = 1 To Product.Columns

                    M.Element(i, j) = Escalar * M.Element(i, j)

                Next
            Next

            Return Product

        End Operator

        'Matrix multiplication:
        Public Shared Operator ^(ByVal M1 As Matrix, ByVal M2 As Matrix) As Matrix

            If M1.Columns = M2.Rows Then
                Dim Product As New Matrix(M1.Rows, M2.Columns)
                For i = 1 To M1.Rows
                    For j = 1 To M2.Columns

                        For m = 1 To M1.Columns
                            For n = 1 To M2.Rows
                                Product.Element(i, j) = Product.Element(i, j) + M1.Element(i, m) * M2.Element(n, j)
                            Next
                        Next

                    Next
                Next
                Return Product
            Else
                Return New Matrix(0, 0)
            End If

        End Operator

#End Region

    End Structure

    Public Class Matrix3x3

        Private RM(2, 2) As Double

        Public Property Item(ByVal i As Integer, ByVal j As Integer) As Double
            Get
                If 1 < i < 3 And 1 < i < 3 Then
                    Return RM(i - 1, j - 1)
                Else
                    Return 0
                End If
            End Get
            Set(ByVal value As Double)
                If 1 < i < 3 And 1 < i < 3 Then
                    RM(i - 1, j - 1) = value
                End If
            End Set
        End Property

        Public ReadOnly Property Determinante As Double
            Get
                Return Item(1, 1) * (Item(2, 2) * Item(3, 3) - Item(3, 2) * Item(2, 3)) - _
                Item(1, 2) * (Item(2, 1) * Item(3, 3) - Item(3, 1) * Item(3, 2)) + _
                Item(1, 3) * (Item(2, 1) * Item(3, 2) - Item(3, 1) * Item(2, 2))
            End Get
        End Property

        Public Sub Transponer()
            Dim Matriz As New Matrix3x3
            Matriz.RM = Me.RM
            For i = 1 To 3
                For j = 1 To 3
                    Me.Item(i, j) = Matriz.Item(j, i)
                Next
            Next
        End Sub

        Public Sub Invertir()

            Dim m As New DotNumerics.LinearAlgebra.Matrix(3)

            m.Item(0, 0) = Item(0, 0)
            m.Item(0, 1) = Item(0, 1)
            m.Item(0, 2) = Item(0, 2)

            m.Item(1, 0) = Item(1, 0)
            m.Item(1, 1) = Item(1, 1)
            m.Item(1, 2) = Item(1, 2)

            m.Item(2, 0) = Item(2, 0)
            m.Item(2, 1) = Item(2, 1)
            m.Item(2, 2) = Item(2, 2)

            Dim i As DotNumerics.LinearAlgebra.Matrix = m.Inverse

            Item(0, 0) = m.Item(0, 0)
            Item(0, 1) = m.Item(0, 1)
            Item(0, 2) = m.Item(0, 2)

            Item(1, 0) = m.Item(1, 0)
            Item(1, 1) = m.Item(1, 1)
            Item(1, 2) = m.Item(1, 2)

            Item(2, 0) = m.Item(2, 0)
            Item(2, 1) = m.Item(2, 1)
            Item(2, 2) = m.Item(2, 2)

        End Sub

    End Class

    Public Class RotationMatrix

        Inherits Matrix3x3

        Public Sub Generate(ByVal t1 As Double, ByVal t2 As Double, ByVal t3 As Double)

            Item(1, 1) = Math.Cos(t1) * Math.Cos(t2)
            Item(1, 2) = Math.Cos(t1) * Math.Sin(t2) * Math.Sin(t3) - Math.Sin(t1) * Math.Cos(t3)
            Item(1, 3) = Math.Sin(t1) * Math.Sin(t3) + Math.Cos(t1) * Math.Sin(t2) * Math.Cos(t3)
            Item(2, 1) = Math.Sin(t1) * Math.Cos(t2)
            Item(2, 2) = Math.Sin(t1) * Math.Sin(t2) * Math.Sin(t3) + Math.Cos(t1) * Math.Cos(t3)
            Item(2, 3) = Math.Sin(t1) * Math.Sin(t2) * Math.Cos(t3) - Math.Cos(t1) * Math.Sin(t3)
            Item(3, 1) = -Math.Sin(t2)
            Item(3, 2) = Math.Cos(t2) * Math.Sin(t3)
            Item(3, 3) = Math.Cos(t2) * Math.Cos(t3)

        End Sub

        Public Sub Generate(ByVal Orientation As EulerAngles)

            Dim c1 As Double = Math.Cos(Orientation.Psi)
            Dim s1 As Double = Math.Sin(Orientation.Psi)

            Dim c2 As Double = Math.Cos(Orientation.Tita)
            Dim s2 As Double = Math.Sin(Orientation.Tita)

            Dim c3 As Double = Math.Cos(Orientation.Fi)
            Dim s3 As Double = Math.Sin(Orientation.Fi)

            Select Case Orientation.Secuence

                Case EulerAngles.RotationSecuence.ZYX

                    Item(1, 1) = c1 * c2
                    Item(1, 2) = c1 * s2 * s3 - s1 * c3
                    Item(1, 3) = s1 * s3 + c1 * s2 * c3
                    Item(2, 1) = s1 * c2
                    Item(2, 2) = s1 * s2 * s3 + c1 * c3
                    Item(2, 3) = s1 * s2 * c3 - c1 * s3
                    Item(3, 1) = -s2
                    Item(3, 2) = c2 * s3
                    Item(3, 3) = c2 * c3

                Case EulerAngles.RotationSecuence.XYZ

                    Item(1, 1) = c2 * c3
                    Item(1, 2) = -c2 * s3
                    Item(1, 3) = s2
                    Item(2, 1) = c1 * s3 + c3 * s1 * s2
                    Item(2, 2) = c1 * c3 - s1 * s2 * s3
                    Item(2, 3) = -c2 * s1
                    Item(3, 1) = s1 * s3 - c1 * c3 * s2
                    Item(3, 2) = c3 * s1 + c1 * s2 * s3
                    Item(3, 3) = c1 * c2

            End Select

        End Sub

    End Class

End Namespace
