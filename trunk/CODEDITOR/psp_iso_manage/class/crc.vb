Imports System.Security.Cryptography
Imports System
Imports System.Text
Imports System.IO

Public Class CRC32
    Inherits HashAlgorithm

    Public Const DefaultPolynomial As UInt32 = &HEDB88320UI
    Public Const DefaultSeed As UInt32 = &HFFFFFFFFUI

    Private Shadows hash As UInt32
    Private seed As UInt32
    Private table() As UInt32
    Private Shared defaultTable() As UInt32

    Public Sub New()

        table = InitializeTable(DefaultPolynomial)
        seed = DefaultSeed
        Initialize()
    End Sub

    Public Sub New(ByVal polynomial As UInt32, ByVal seed As UInt32)

        table = InitializeTable(polynomial)
        Me.seed = seed
        Initialize()
    End Sub

    Property Text As String

    Public Overrides Sub Initialize()
        hash = seed
    End Sub

    Protected Overrides Sub HashCore(ByVal buffer As Byte(), ByVal start As Int32, ByVal length As Int32)

        hash = CalculateHash(table, hash, buffer, start, length)
    End Sub

    Protected Overrides Function HashFinal() As Byte()

        Dim hashBuffer As Byte() = UInt32ToBigEndianBytes(Not hash)
        Me.HashValue = hashBuffer
        Return hashBuffer
    End Function

    Public Overrides ReadOnly Property HashSize() As Int32
        Get
            Return 32
        End Get
    End Property

    Public Shared Function Compute(ByVal buffer As Byte()) As UInt32

        Return Not CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length)
    End Function

    Public Shared Function Compute(ByVal seed As UInt32, ByVal buffer As Byte()) As UInt32

        Return Not CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length)

    End Function

    Public Shared Function Compute(ByVal polynomial As UInt32, ByVal seed As UInt32, ByVal buffer As Byte()) As UInt32

        Return Not CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length)

    End Function

    Private Shared Function InitializeTable(ByVal polynomial As UInt32) As UInt32()

        If polynomial = DefaultPolynomial AndAlso Not defaultTable Is Nothing Then
            Return defaultTable
        End If

        Dim createTable(255) As UInt32

        For i As Integer = 0 To 255

            Dim entry As UInt32 = CUInt(i)
            For j As Integer = 0 To 7
                If (entry And 1) = 1 Then
                    entry = (entry >> 1) Xor polynomial
                Else
                    entry = entry >> 1
                End If
            Next
            createTable(i) = entry
        Next

        If polynomial = DefaultPolynomial Then
            defaultTable = createTable
        End If
        Return createTable

    End Function

    Private Shared Function CalculateHash(ByVal table As UInt32(), ByVal seed As UInt32, ByVal buffer As Byte(), ByVal start As Int32, ByVal size As Int32) As UInt32

        Dim crc As UInt32 = seed
        For i As Integer = start To size - 1
            crc = (crc >> 8) Xor table(CInt(buffer(i) Xor crc And &HFF))
        Next

        Return crc

    End Function

    Private Function UInt32ToBigEndianBytes(ByVal x As UInt32) As Byte()

        Return New Byte() {CByte(((x >> 24) And &HFF)), CByte(((x >> 16) And &HFF)), CByte(((x >> 8) And &HFF)), CByte((x And &HFF))}

    End Function
End Class